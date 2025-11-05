  
        
  
  
  
CREATE PROCEDURE [dbo].[P_InterestExpenseReport]    
(    
@date datetime,      
@fund varchar(max)    
)    
As    
  
declare @MinDate datetime      
Select @MinDate= CashMgmtStartDate From T_CashPreferences       
        
declare @BaseCurrencyID int      
Select @BaseCurrencyID= BaseCurrencyID From T_Company      
     
Select * Into #Funds                                      
from dbo.Split(@fund, ',')        
        
    
    
SELECT    
SubBal.SubAccountID,    
    
   
SubAcc.Name as SubAccName,   
    
SubCat.SubCategoryID,    
    
   
SubCat.SubCategoryName,    
    
MastCat.MasterCategoryID,    
    
    
MastCat.MasterCategoryName,    
    
    
SubAcc.TransactionTypeID,    
SubBal.FundId,    
Funds.FundName as FundName,    
SubBal.CurrencyId,    
Curr.CurrencySymbol,    
SubBal.OpenBalDate,    
SubBal.TransactionDate,    
        
  
(SubBal.CloseDrBal - SubBal.CloseCrBal) as BalanceLocal,    
    
    
(SubBal.CloseDrBalBase - SubBal.CloseCrBalBase) as BalanceBase  
    
    
    
    
from T_SubAccountBalances SubBal    
INNER JOIN T_SubAccounts SubAcc on SubBal.SubAccountID = SubAcc.SubAccountID    
INNER JOIN T_CompanyFunds Funds on SubBal.FundID = Funds.CompanyFundID    
INNER JOIN T_Currency Curr on SubBal.CurrencyID = Curr.CurrencyID    
INNER JOIN T_SubCategory SubCat on SubAcc.SubCategoryID = SubCat.SubCategoryID    
INNER JOIN T_MasterCategory MastCat on SubCat.MasterCategoryID = MastCat.MasterCategoryID    
   
    
WHERE     
DateDiff(d,TransactionDate,@date) = 0    
and    
SubBal.FundId in (Select * from #Funds)    
and  
MastCat.MasterCategoryName IN ('Income','Expenses')  
  