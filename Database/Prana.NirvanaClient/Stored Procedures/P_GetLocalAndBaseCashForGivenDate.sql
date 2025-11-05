      
-- =============================================      
-- Author:  Bhupesh Bareja      
-- Create date: 20 October, 2008      
-- Description: To get the local and base currency cash value for the given date for all the currencies.    
--    present in the DB.       
-- Execute:  exec P_GetLocalAndBaseCashForGivenDate '10-17-2008'      
-- =============================================      
CREATE PROCEDURE [dbo].[P_GetLocalAndBaseCashForGivenDate]      
 @date datetime = Null      
AS      
BEGIN      
    
select        
Sum(CashValueLocal) AS CashValueLocal,       
LocalCurrencyID,      
Sum(CashValueBase) AS CashValueBase,       
min(BaseCurrencyID) AS BaseCurrencyID,      
min(C.CurrencySymbol) AS LocalCurrencySymbol,    
min(CBase.CurrencySymbol) AS BaseCurrencySymbol,  
CFCC.FundID AS FundID,  
min(CF.FundName) AS FundName  
      
from T_DayEndBalances CFCC INNER JOIN T_Currency C      
 ON CFCC.LocalCurrencyID = C.CurrencyID AND CFCC.BalanceType=1    
 INNER JOIN T_Currency CBase ON    
 BaseCurrencyID = CBase.CurrencyID    
 INNER JOIN T_CompanyFunds CF ON CFCC.FundID = CF.CompanyFundID  
       
 WHERE Date = @date      
group by CFCC.FundID, LocalCurrencyID      
    
--order by C.CurrencySymbol    
    
END      
