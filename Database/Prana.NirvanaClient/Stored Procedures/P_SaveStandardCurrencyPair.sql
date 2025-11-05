                        
 CREATE PROCEDURE [dbo].[P_SaveStandardCurrencyPair]                            
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
                            
  CREATE TABLE #TempCurrencyPair                                                                                   
  (                                                                                   
    FromCurrencyID int                            
   ,ToCurrencyID int                                                
   ,BloombergSymbol varchar(100)                           
   )                                                                            
INSERT INTO #TempCurrencyPair                            
 (                                                                                  
    FromCurrencyID                            
   ,ToCurrencyID                                                                             
   ,BloombergSymbol                  
 )                                                                                  
SELECT                                                                                   
  FromCurrencyID                            
 ,ToCurrencyID                                                                                  
 ,BloombergSymbol                         
    FROM OPENXML(@handle, '//StandardCurrencyPairs', 2)                                                                                     
 WITH                                                                
 (                                                             
  FromCurrencyID int                            
 ,ToCurrencyID int                                               
 ,BloombergSymbol varchar(100)                  
 )         
    
Insert into         
   T_CurrencyStandardPairs         
   (        
   FromCurrencyID,        
   ToCurrencyID,        
   eSignalSymbol,
   BloombergSymbol        
   )        
   Select        
   FromCurrencyID,        
   ToCurrencyID,
   'NA',        
   BloombergSymbol    
from #TempCurrencyPair       

DROP TABLE #TempCurrencyPair                                           
EXEC sp_xml_removedocument @handle                            
                            
COMMIT TRANSACTION TRAN1                            
                            
END TRY                            
BEGIN CATCH                             
 SET @ErrorMessage = ERROR_MESSAGE();                            
print @errormessage                            
 SET @ErrorNumber = Error_number();                    
 ROLLBACK TRANSACTION TRAN1                             
END CATCH; 

