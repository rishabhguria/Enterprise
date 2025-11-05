
CREATE Procedure [dbo].[PMGetDailyCash]        
(                            
@date DateTime,        
@errorMessage varchar(500) output,                                      
@errorNumber int output                               
)                            
As                            
      
SET @ErrorMessage = 'Success'                                      
SET @ErrorNumber = 0                               
                            
BEGIN TRY                        
                            
Select Date, FundID, BaseCurrencyID, CashValueBase, LocalCurrencyID, CashValueLocal, CashCurrencyID         
from PM_CompanyFundCashCurrencyValue where Datediff(d,Date,@date) = 0      
                            
END TRY                                      
BEGIN CATCH                              
                                        
 SET @ErrorMessage = ERROR_MESSAGE();                                      
 SET @ErrorNumber = Error_number();          
                                       
END CATCH;                                         
             
