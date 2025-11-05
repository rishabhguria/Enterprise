
/*
exec [P_GetStartOfDayNAV_ThirdParty] @thirdPartyID=47,@companyFundIDs=N'2,1',
@inputDate='2019-11-20 01:44:40',@companyID=7,@auecIDs=N'18,202,1,15,12',@TypeID=0,@dateType=0,@fileFormatID=108
*/

Create Procedure [dbo].[P_GetStartOfDayNAV_ThirdParty]                        
(                                 
@ThirdPartyID int,                                            
@CompanyFundIDs varchar(max),                                                                                                                                                                          
@InputDate datetime,                                                                                                                                                                      
@CompanyID int,                                                                                                                                      
@AUECIDs varchar(max),                                                                            
@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                            
@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                            
@FileFormatID int                                  
)                                  
AS      
    
--Declare @InputDate DateTime    
--Set @InputDate = '07-22-2017'

--Declare @CompanyFundIDs varchar(max) 
--Set @companyFundIDs = '1257'   

Declare @Fund Table                                                           
(                
FundID int                      
)  

Insert into @Fund                                                                                                    
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',') 


SELECT @InputDate = dbo.AdjustBusinessDays(@InputDate,-1,1)
BEGIN

Select * into #tempbalances 
from T_SubAccountBalances SubAccBal
where fundid in(select FundID from @Fund)
and datediff(dd,transactiondate,@InputDate)=0

Select fundId, Sum(CloseDRBalBase - CloseCRBalBase) as CashBalances
       into #tempCashBalancesDatewise
		FROM #tempbalances tempSubAccBal
		   INNER JOIN T_SubAccounts SubAccounts ON SubAccounts.SubAccountID = tempSubAccBal.SubAccountID
           INNER JOIN T_TransactionType TransType ON SubAccounts.TransactionTypeID = TransType.TransactionTypeID
           JOIN T_CompanyFunds FS ON tempSubAccBal.FundID = FS.CompanyFundID
	   where TransType.TransactionType = 'Cash'
	  group by Fundid

Select fundId, Sum(CloseDRBalBase - CloseCRBalBase) as Accruals 
into #tempAccrualsDatewise		
		FROM #tempbalances tempSubAccBal
		   INNER JOIN T_SubAccounts SubAccounts ON SubAccounts.SubAccountID = tempSubAccBal.SubAccountID
           INNER JOIN T_TransactionType TransType ON SubAccounts.TransactionTypeID = TransType.TransactionTypeID
           JOIN T_CompanyFunds FS ON tempSubAccBal.FundID = FS.CompanyFundID
	       where TransType.TransactionType = 'Accrued Balance'
		   group by Fundid


Select fundId, Sum(CloseDRBalBase - CloseCRBalBase) as LMV into #tempLMVDatewise
		FROM #tempbalances tempSubAccBal
		 INNER JOIN T_SubAccounts SubAccounts ON SubAccounts.SubAccountID = tempSubAccBal.SubAccountID
         INNER Join T_SubCategory SubCategory on SubAccounts.SubCategoryID = SubCategory.SubCategoryID
         JOIN T_CompanyFunds FS ON tempSubAccBal.FundID = FS.CompanyFundID
		 where SubCategory.SubCategoryID in('3')	       
		 group by Fundid


Select fundId, Sum(CloseDRBalBase - CloseCRBalBase) as SMV into #tempSMVDatewise
		FROM #tempbalances tempSubAccBal
		INNER JOIN T_SubAccounts SubAccounts ON SubAccounts.SubAccountID = tempSubAccBal.SubAccountID
         INNER Join T_SubCategory SubCategory on SubAccounts.SubCategoryID = SubCategory.SubCategoryID
         JOIN T_CompanyFunds FS ON tempSubAccBal.FundID = FS.CompanyFundID
	       where SubCategory.SubCategoryName ='Investments - Short'
		   group by Fundid


SELECT CMF.MasterFundName As MasterFundName , CMFSA.CompanyFundID As FundID Into #TempMasterfund
FROM T_CompanyMasterFunds CMF
INNER JOIN T_CompanyMasterFundSubAccountAssociation CMFSA ON CMFSA.CompanyMasterFundID = CMF.CompanyMasterFundID
INNER JOIN @Fund TCF ON CMFSA.CompanyFundID = TCF.FundID

Select @InputDate as Date,
		TMF.MasterFundName as MasterFundName,		
		 IsNull(sum(CashBalances),0) as CashBalances, 
		 IsNull(sum(Accruals),0) as Accruals,
		 IsNull(sum(LMV),0) as LMV,
		 IsNull(sum(SMV),0) as SMV,
		Sum (IsNull(CashBalances,0) + IsNull(Accruals,0) + IsNull(LMV,0) + IsNull(SMV,0)) As StarOfDayNav 
From #TempMasterfund TMF
Left Join #tempCashBalancesDatewise on #tempCashBalancesDatewise.FundID = TMF.FundID
Left Join #tempAccrualsDatewise On #tempAccrualsDatewise.FundID=TMF.FundID
Left Join #tempLMVDatewise On #tempLMVDatewise.FundID=TMF.FundID
Left Join #tempSMVDatewise On #tempSMVDatewise.FundID=TMF.FundID
Inner Join T_CompanyFunds CFS On CFS.CompanyFundID = TMF.FundID                
Group by TMF.MasterFundName


Drop Table #tempCashBalancesDatewise, #tempAccrualsDatewise,#tempLMVDatewise,#tempSMVDatewise,#tempbalances,#TempMasterfund
END