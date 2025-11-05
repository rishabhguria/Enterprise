

/****************************************************************************                                        
Name :   P_GetCurrenciesWithSymbol
Purpose:  Gets the distinct currency details.
Module Name: MarkPriceAndForexCoversion/PM
Author: Sandeep Singh                                        

Execution StateMent:                                         
   EXEC [P_GetCurrenciesWithSymbol]
                                        
Date Modified:                                         
Description:                                           
Modified By:                                           
****************************************************************************/  
  
  
CREATE PROCEDURE [dbo].[P_GetCurrenciesWithSymbol] AS      
      
  SELECT DISTINCT C.CURRENCYID,C.CURRENCYNAME ,C.CurrencySymbol FROM T_CURRENCY C ORDER BY C.CurrencySymbol

