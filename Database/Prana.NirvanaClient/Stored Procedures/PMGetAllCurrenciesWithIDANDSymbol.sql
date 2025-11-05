

/****************************************************************************    
Name :   [PMGetAllCurrenciesWithIDANDSymbol]    
Date Created: 6-Mar-2008 
Purpose:  Gets the list of other currencies for a given asset ID.    
Author: Bhupesh Bareja
Parameters:     
   @assetID int
Execution StateMent:     
   EXEC PMGetAllCurrenciesWithIDANDSymbol
Date Modified:     
Description:   
Modified By:   

****************************************************************************/    
CREATE PROCEDURE [dbo].[PMGetAllCurrenciesWithIDANDSymbol] AS     
    
BEGIN TRY    
    
SELECT       
 CurrencyID,    
 CurrencySymbol
FROM    
 T_Currency C

ORDER BY CurrencySymbol
       
END TRY    
BEGIN CATCH    
-- SET @ERROR = ERROR_NUMBER();    
-- SET @ErrorMessage = ERROR_MESSAGE();    
END CATCH;    
--RETURN @ERROR
