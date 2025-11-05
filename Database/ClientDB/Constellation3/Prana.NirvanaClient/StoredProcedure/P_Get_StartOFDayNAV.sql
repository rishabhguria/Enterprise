/*
exec [P_Get_StartOFDayNAV] @thirdPartyID=47,@companyFundIDs=N'2,1',
@inputDate='2019-11-20 01:44:40',@companyID=7,@auecIDs=N'18,202,1,15,12',@TypeID=0,@dateType=0,@fileFormatID=108
*/

CREATE Procedure [dbo].[P_Get_StartOFDayNAV]                        
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
		WHERE tempSubAccBal.SubAccountID IN (
				SELECT DISTINCT SubAccountID
				FROM T_SubAccountMappingPSR
				INNER JOIN T_FieldsPSR ON T_FieldsPSR.FieldID = T_SubAccountMappingPSR.FieldID
				WHERE T_FieldsPSR.FieldName = 'LMV')
				group by Fundid

Select fundId, Sum(CloseDRBalBase - CloseCRBalBase) as SMV into #tempSMVDatewise
		FROM #tempbalances tempSubAccBal
		WHERE tempSubAccBal.SubAccountID IN (
				SELECT DISTINCT SubAccountID
				FROM T_SubAccountMappingPSR
				INNER JOIN T_FieldsPSR ON T_FieldsPSR.FieldID = T_SubAccountMappingPSR.FieldID
				WHERE T_FieldsPSR.FieldName = 'SMV')
				group by Fundid

Select @InputDate as Date,
 CFS.FundName as AccountName,
 IsNull(CashBalances,0) as CashBalances, 
 IsNull(Accruals,0) as Accruals,
 IsNull(LMV,0) as LMV,
 IsNull(SMV,0) as SMV,
Sum (IsNull(CashBalances,0) + IsNull(Accruals,0) + IsNull(LMV,0) + IsNull(SMV,0)) As StarOfDayNav 
from @Fund CF
left join #tempCashBalancesDatewise on #tempCashBalancesDatewise.FundID = CF.FundID
left Join #tempAccrualsDatewise On #tempAccrualsDatewise.FundID=CF.FundID
left Join #tempLMVDatewise On #tempLMVDatewise.FundID=CF.FundID
left Join #tempSMVDatewise On #tempSMVDatewise.FundID=CF.FundID
Inner Join T_CompanyFunds CFS On CFS.CompanyFundID = CF.FundID                
Group by #tempCashBalancesDatewise.CashBalances, #tempAccrualsDatewise.Accruals, #tempLMVDatewise.LMV, #tempSMVDatewise.SMV,CFS.FundName

Drop Table #tempCashBalancesDatewise, #tempAccrualsDatewise,#tempLMVDatewise,#tempSMVDatewise,#tempbalances
END