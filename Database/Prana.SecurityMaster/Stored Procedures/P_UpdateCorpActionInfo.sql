-- Author    : Abhilash Katiyar                         
-- Modification Date : 14 May 09                          
                          
CREATE PROCEDURE [dbo].[P_UpdateCorpActionInfo]                                                        
(                    
  @xml varchar(max),        
  @isApplied bit,                     
  @ErrorMessage varchar(500) output,                                                                   
  @ErrorNumber int output        
)                          
As                 
             
SET @ErrorNumber = 0                                                  
SET @ErrorMessage = 'Success'                                
                                          
BEGIN TRY                                   
                          
  DECLARE @handle int                             
  exec sp_xml_preparedocument @handle OUTPUT,@Xml                       
                   
  CREATE TABLE #TempCorp                                                                                            
  (                           
	OrigSymbol varchar(100),           
	CorpActionID uniqueidentifier,              
	CorporateActionType varchar(5),                      
	CorporateActionString xml,        
	EffectiveDate datetime                         
  )                                                                                                              
                                    
  Insert Into #TempCorp                                                                                                                                             
  (                                                                                                                                            
	OrigSymbol,           
	CorpActionID,              
	CorporateActionType,                      
	CorporateActionString,         
	EffectiveDate                     
  )                                                                                                                 
  Select                                                                                                                                             
	OrigSymbol,           
	CorpActionID,              
	CorporateActionType,                      
	CorporateActionString,        
	EffectiveDate                  
                               
  FROM OPENXML(@handle, '//CaFullTable', 2)                                                                                                                                               
  WITH                                                                  
  (                          
	OrigSymbol varchar(100),           
	CorpActionID uniqueidentifier,              
	CorporateActionType varchar(5),                      
	CorporateActionString xml '@mp:xmltext',        
	EffectiveDate datetime        
  )                         
    
UPDATE T_SMCorporateActions  
 SET CorporateAction = #TempCorp.CorporateActionString,  
     EffectiveDate = #TempCorp.EffectiveDate,  
     CorporateActionType = #TempCorp.CorporateActionType,  
     IsApplied = @isApplied,  
     Symbol = #TempCorp.OrigSymbol  
 FROM #TempCorp  
 WHERE #TempCorp.CorpActionID = T_SMCorporateActions.CorpActionID   
  
                  
Drop Table #TempCorp        
EXEC sp_xml_removedocument @handle                                                                       
END TRY                                                                                  
BEGIN CATCH                                           
 SET @ErrorMessage = ERROR_MESSAGE()                                                 
 SET @ErrorNumber = Error_number()                            
 Drop table #TempCorp         
 EXEC sp_xml_removedocument @handle                  
END CATCH   
