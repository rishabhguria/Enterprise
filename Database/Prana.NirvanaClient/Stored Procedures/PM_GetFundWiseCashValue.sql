
CREATE Procedure [dbo].[PM_GetFundWiseCashValue]                
(                
 @companyID int,                
 @localdate datetime                
)                
                
AS                
Begin                
                
 Declare @PreviousUSBusinessDay datetime        
 Set @PreviousUSBusinessDay = dbo.AdjustBusinessDays(@localdate, -1,1) -- 1 is NASDAQ's AUECID        
                                             
Declare @companyBaseCurrencyID int                
Select Top 1 @companyBaseCurrencyID = BaseCurrencyID from T_Company      
         
 Select CompanyFundID As FundID, SUM(IsNull(CashValueBase,0)) as Cash             
  From T_CompanyFunds      
 Left Outer Join (Select FundID, CashValueBase, BaseCurrencyID, Date         
      from PM_CompanyFundCashCurrencyValue where BalanceType=1        
      AND datediff(d,Date,@PreviousUSBusinessDay) = 0) As FC         
 ON FC.FundID = T_CompanyFunds.CompanyFundID  
 Where T_CompanyFunds.IsActive=1      
Group By CompanyFundID, BaseCurrencyID            
                
End   
  
