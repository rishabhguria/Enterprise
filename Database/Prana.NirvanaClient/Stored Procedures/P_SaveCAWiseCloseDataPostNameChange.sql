
-- Author : Rajat Tandon  
-- Date : 22 Sep 2010  
-- Description : Saves the cawise closing data  
CREATE Procedure P_SaveCAWiseCloseDataPostNameChange  
(                                                          
 @xml nText,                                                          
 @ErrorMessage varchar(500) output,                                                                                                   
 @ErrorNumber int output                                                                      
)                                                          
                                                          
As                                                          
                                                                                  
SET @ErrorNumber = 0                                                                                  
SET @ErrorMessage = 'Success'                                                           
                                                          
BEGIN TRY                                                                                  
                                                          
 BEGIN TRAN TRAN1                                                                                
                                                                                  
  DECLARE @handle int                                                                                
                                                                                                                               
  EXEC sp_xml_preparedocument @handle OUTPUT,@Xml                                         
  
  INSERT INTO PM_CorpActionTaxlots (CorpActionId,ClosingId, TaxlotId,ClosingTaxlotId)          
  Select                                        
	 CAID,      
	 ClosingID,  
	 PositionalTaxlotID,  
	 ClosingTaxlotID  
                                                                
  FROM OPENXML(@handle, '//CACloseData/CAClosingList/ClosingInfo', 2)                                                                    
  WITH                                                                                                  
  (                                           
	 CAID uniqueidentifier '../../CAID',      
	 ClosingID uniqueidentifier,  
	 PositionalTaxlotID varchar(50),  
	 ClosingTaxlotID varchar(50)  
  )                                                        
                                                 
  EXEC sp_xml_removedocument @handle                                                      
                                                                                     
 COMMIT TRANSACTION TRAN1                                                                                    
                                                                         
END TRY                                                                                    
BEGIN CATCH                                                                                     
 SET @ErrorMessage = ERROR_MESSAGE();                                                                          
 SET @ErrorNumber = Error_number();                          
--select      @ErrorMessage                                                                
 ROLLBACK TRANSACTION TRAN1                                                                                   
END CATCH;                     

