/*  
EXEC P_GetMasterFundNAV_EOD_EternalCapital 1,'15','09-08-2020',1,1,1,1,1  
*/  
  
CREATE Procedure [dbo].[P_GetAccountNAV_EOD_EternalCapital]                          
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
  
--Declare @InputDate DATETIME, @CompanyFundIDs VARCHAR(max)  
--Set @InputDate = '2020-09-08'  
--Set @CompanyFundIDs = '15'  
  
Declare @Funds TABLE  
(  
 FundID INT  
)  
INSERT INTO @Funds  
SELECT Items  
FROM dbo.Split(@CompanyFundIDs, ',')  
  
SELECT   
--MF.MasterFundName As MasterFund,  
--Funds.FundName As MasterFund,  
Max(TC.CurrencySymbol) As CurrencySymbol,  
Sum( SubBal.CloseDrBalBase  -SubBal.CloseCrBalBase) As NAV,  
Convert(Varchar,@InputDate,110) As [Date]  
  
FROM T_SubAccountBalances SubBal WITH(NOLOCK)  
Inner Join @Funds TempF On TempF.fundID = SubBal.FundID  
INNER JOIN T_SubAccounts SubAcc WITH(NOLOCK) ON SubAcc.SubAccountID = SubBal.SubAccountID  
INNER JOIN T_CompanyFunds Funds WITH(NOLOCK) ON Funds.CompanyFundID = SubBal.FundID  
Inner Join T_CompanyMasterFundSubAccountAssociation MFA On MFA.CompanyFundID = Funds.CompanyFundID   
Inner Join T_CompanyMasterFunds MF On MF.CompanyMasterFundID = MFA.CompanyMasterFundID  
INNER JOIN T_SubCategory SubCat WITH(NOLOCK) ON SubCat.SubCategoryID = SubAcc.SubCategoryID  
INNER JOIN T_Currency TC WITH(NOLOCK) ON TC.CurrencyID = SubBal.CurrencyID  
INNER JOIN T_MasterCategory MastCat WITH(NOLOCK) ON MastCat.MasterCategoryID = SubCat.MasterCategoryID  
INNER JOIN T_TransactionType AccType WITH(NOLOCK) ON AccType.TransactionTypeId = SubAcc.TransactionTypeId  
INNER JOIN T_CashPreferences CashPref WITH(NOLOCK) ON CashPref.FundID = SubBal.FundID  
WHERE DateDiff(d, TransactionDate, @InputDate) = 0  
 AND DATEDIFF(d, SubBal.TransactionDate, CashPref.CashMgmtStartDate) <= 0  
 And MastCat.MasterCategoryID In (1,2,6)  
--Group By MF.MasterFundName  
--Order By MF.MasterFundName  
--Group By Funds.FundName  
--Order By Funds.FundName