
-- P_SaveInterestRate 2, 1 -- select * from T_InterestRate 

    
CREATE PROCEDURE [dbo].[P_SaveInterestRate]      
(      
 @Xml varchar(max),                                                                                                                            
 @ErrorMessage varchar(500) output,                                                                                                                                     
 @ErrorNumber int output   
)      
AS       
                                                                                 
SET @ErrorNumber = 0                                                                                                                    
SET @ErrorMessage = 'Success'                                                                                                  
                                                                                                            
BEGIN TRY                                                                                                     
                                                                                            
 BEGIN TRAN TRAN1             
                                                                       
DECLARE @handle int                                                                         
exec sp_xml_preparedocument @handle OUTPUT,@Xml               
            
DELETE FROM T_InterestRate       
            
INSERT INTO T_InterestRate        
(        
Period,    
Rate           
)        
SELECT             
Period,    
Rate       

FROM  OPENXML(@handle,'//InterestRate/InterestRates',3)                                                                
WITH                                                                                        
(               
Period int,    
Rate float         
)   
      
COMMIT TRANSACTION TRAN1                                                                            
                                                                                                   
exec sp_xml_removedocument @handle                                                                                 
END TRY                                                                                                    
BEGIN CATCH                                                                                          
SET @ErrorMessage = ERROR_MESSAGE();                                                                                                    
SET @ErrorNumber = Error_number();                      
ROLLBACK TRANSACTION TRAN1                                        
END CATCH;
