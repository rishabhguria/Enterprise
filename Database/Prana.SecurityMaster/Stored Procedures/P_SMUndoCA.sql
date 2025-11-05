--Author  : Rajat Tandon                      
-- Date   : 04 May 2009                      
-- Description : Undo the effect of supplied corporate actions.                      
CREATE PROCEDURE [dbo].[P_SMUndoCA]                      
(                      
 @corpactionIDs varchar(max),  
 @ErrorMessage varchar(500) output,  
 @ErrorNumber int output                          
)                      
As                      
  
                      
SET @ErrorNumber = 0                                                  
SET @ErrorMessage = 'Success'                                
                                          
BEGIN TRY                                   
    
 Update T_SMCorporateActions                      
 Set IsApplied = 0 where CorpActionID in (Select Items from dbo.Split(@corpactionIDs,','))                      
                        
END TRY                                                    
BEGIN CATCH                                                     
 SET @ErrorMessage = ERROR_MESSAGE();                                                    
 SET @ErrorNumber = Error_number();                                                     
 ROLLBACK TRANSACTION TRAN1                                                       
END CATCH; 