--Author  : Rajat Tandon                      
-- Date   : 30 July 2008                      
-- Description : Undo the effect of supplied corporate actions.                      
CREATE PROCEDURE [dbo].[P_SMUndoNameChange]                      
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
  
 EXEC P_SMUndoCA @corpactionIDs,'',0  
           
 delete from T_SMReuters where CorpActionID in (Select Items from dbo.Split(@corpactionIDs,',')) and ISPrimaryExchange = '1'          
  
 Delete From T_SMEquityNonHistoryData                      
 where CorpActionID in (Select Items from dbo.Split(@corpactionIDs,','))                      
                      
 Delete From T_SMSymbolLookUpTable                      
 where CorpActionID in (Select Items from dbo.Split(@corpactionIDs,','))         
                        
 COMMIT TRANSACTION TRAN1                            
                                        
END TRY                                                    
BEGIN CATCH                                                     
 SET @ErrorMessage = ERROR_MESSAGE();                                                    
 SET @ErrorNumber = Error_number();                                                     
 ROLLBACK TRANSACTION TRAN1                                                       
END CATCH; 