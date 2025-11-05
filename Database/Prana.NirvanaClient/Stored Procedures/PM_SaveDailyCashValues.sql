         
/****************************************************************************                  
Name :   PM_SaveMarkPrices                  
Date Created: 08-Oct-2008                   
Purpose:  Save daily cash values for the given date in database.                  
Module: MarkPriceAndForexConversion/PM              
Usage: exec PM_SaveMarkPrices               
  , ' ', 0              
Author: Bhupesh Bareja        
Parameters:                   
                
 ****************************************************************************/                  
CREATE Proc [dbo].[PM_SaveDailyCashValues]        
(                  
   @Xml nText                  
 , @ErrorMessage varchar(500) output                  
 , @ErrorNumber int output                  
)                  
AS                   
                  
SET @ErrorMessage = 'Success'                  
SET @ErrorNumber = 0                  
BEGIN TRAN TRAN1                   
                  
BEGIN TRY                  
                  
                  
DECLARE @handle int                     
exec sp_xml_preparedocument @handle OUTPUT,@Xml                     
                  
  CREATE TABLE #TempDailyCashValues        
  (                                                                         
    FundID int                     
   ,Date datetime                
   ,BaseCurrencyID int        
   ,CashValueBase float        
   ,LocalCurrencyID int        
   ,CashValueLocal float        
   )                                                                        
                                                                        
INSERT INTO #TempDailyCashValues        
 (                                                                        
   FundID          
   ,Date         
   ,BaseCurrencyID         
   ,CashValueBase         
   ,LocalCurrencyID         
   ,CashValueLocal        
 )                                                                        
SELECT                                                                         
  FundID          
   ,Date         
   ,BaseCurrencyID         
   ,CashValueBase         
   ,LocalCurrencyID         
   ,CashValueLocal         
                
FROM OPENXML(@handle, '//Table1', 2)                                                                           
 WITH                                                                         
 (                                                   
    FundID int                     
   ,Date datetime                
   ,BaseCurrencyID int        
   ,CashValueBase float        
   ,LocalCurrencyID int        
   ,CashValueLocal float        
 )                  
                
DELETE  PM_CompanyFundCashCurrencyValue 
WHERE
 DATEADD(day, DATEDIFF(day, 0, PM_CompanyFundCashCurrencyValue.Date), 0) in (select DATEADD(day, DATEDIFF(day, 0, Convert(datetime, #TempDailyCashValues.Date, 113)), 0) from   #TempDailyCashValues)      
 AND PM_CompanyFundCashCurrencyValue.FundID in (select #TempDailyCashValues.FundID From #TempDailyCashValues)      
 AND PM_CompanyFundCashCurrencyValue.BaseCurrencyID in (select #TempDailyCashValues.BaseCurrencyID From #TempDailyCashValues)      
 AND PM_CompanyFundCashCurrencyValue.LocalCurrencyID in (select #TempDailyCashValues.LocalCurrencyID From #TempDailyCashValues)      
                
                 
                  
 INSERT INTO                 
 PM_CompanyFundCashCurrencyValue        
   (                  
    FundID          
   ,Date         
   ,BaseCurrencyID         
   ,CashValueBase         
   ,LocalCurrencyID         
   ,CashValueLocal    )                  
                  
  SELECT                   
    FundID          
   ,Date         
   ,BaseCurrencyID         
   ,CashValueBase         
   ,LocalCurrencyID         
   ,CashValueLocal        
                      
  FROM                   
   #TempDailyCashValues                         
                  
                  
DROP TABLE #TempDailyCashValues        
                  
                  
EXEC sp_xml_removedocument @handle                  
                  
COMMIT TRANSACTION TRAN1                  
                  
END TRY                  
BEGIN CATCH              
 SET @ErrorMessage = ERROR_MESSAGE();                  
print @errormessage                  
 SET @ErrorNumber = Error_number();                   
 ROLLBACK TRANSACTION TRAN1                   
END CATCH;                                          
             
