
-- Author : Rajat
-- Description : Delete from PM_CorpActionTaxlots table with respect to @corpactionIDs
CREATE Procedure [dbo].[P_DeleteNameChangeData]                            
(                            
 @corpactionIDs varchar(max),                          
 @ErrorMessage varchar(500) output,        
 @ErrorNumber int output                                    
)                            
As                            
                            
SET @ErrorNumber = 0                                                        
SET @ErrorMessage = 'Success'                                      
                                                
BEGIN TRY                                         
                                
 BEGIN TRAN TRAN1                                   
        
 Select * into #TempCAIDs from dbo.Split(@corpactionIDs,',') as Items    
    
 Delete PM_CorpActionTaxlots     
 from PM_CorpActionTaxlots CATaxlots inner join #TempCAIDs temp on CATaxlots.CorpActionID = temp.Items    
    
 Drop table #TempCAIDs    
                     
COMMIT TRANSACTION TRAN1                                  
                                              
END TRY                                                          
BEGIN CATCH                                                           
 SET @ErrorMessage = ERROR_MESSAGE();                                                          
 SET @ErrorNumber = Error_number();                                                           
 ROLLBACK TRANSACTION TRAN1                                                             
END CATCH;   
  
