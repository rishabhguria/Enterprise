-- =============================================    
-- Author:  Bhupesh Bareja    
-- Create date: 16 October, 2008    
-- Description: To get the local currency cash value starting balance for the given date for all the currencies     
--    present in the DB.     
-- Execute:  exec P_GetCashForGivenDate '07-31-2008'    
-- =============================================    
CREATE PROCEDURE [dbo].[P_GetCashForGivenDate]    
 @date datetime = Null    
AS    
BEGIN    
    
select      
Sum(CashValueLocal) AS CashValue,     
LocalCurrencyID,    
min(CurrencySymbol) AS CurrencySymbol    
    
from T_DayEndBalances 
 CFCC INNER JOIN T_Currency C    
 ON CFCC.LocalCurrencyID = C.CurrencyID    
     
where CFCC.BalanceType=1 AND Date = @date    
group by LocalCurrencyID    
  
order by CurrencySymbol  
    
END    
