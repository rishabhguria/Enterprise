    

CREATE PROCEDURE [dbo].PMDeleteDailyCashValue      
(                      
  @fundID int      
 ,@localCurrencyID int      
 ,@baseCurrencyID int      
 ,@date datetime      
)                      
AS      
                      
Delete from PM_CompanyFundCashCurrencyValue      
where  FundID = @fundID      
 AND LocalCurrencyID = @localCurrencyID      
 AND BaseCurrencyID = @baseCurrencyID      
 AND Date = @date  
             
