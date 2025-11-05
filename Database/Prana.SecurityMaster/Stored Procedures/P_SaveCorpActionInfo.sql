

CREATE PROCEDURE [dbo].[P_SaveCorpActionInfo]                                                              
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
  Declare @UTCdatetime datetime                                    
  exec sp_xml_preparedocument @handle OUTPUT,@Xml                             
  
CREATE TABLE #TempCorp                                                                                                  
(                                 
	OrigSymbol varchar(100),                 
	CorpActionID uniqueidentifier,                    
	CorporateActionType varchar(5),                            
	EffectiveDate datetime,    
	CorporateAction xml , 
)                                                                                                                    
  
Insert Into #TempCorp                                                                                                                                                   
(                                                                                                                                                  
	OrigSymbol,                 
	CorpActionID,                    
	CorporateActionType,                            
	EffectiveDate,    
	CorporateAction   
)                                                                                                                       
Select                                                                                                                                                   
	OrigSymbol,                 
	CorpActionID,                    
	CorporateActionType,                            
	EffectiveDate, --cast(EffectiveDate as xml).value('xs:dateTime(.)', 'DATETIME')                         
	CorporateAction                                 
FROM OPENXML(@handle, '//CaFullTable', 2)                                                                                                                                                     
WITH                                                                        
(                                
	OrigSymbol varchar(100),                 
	CorpActionID uniqueidentifier,                    
	CorporateActionType varchar(5),                            
	EffectiveDate datetime,    
	CorporateAction xml '@mp:xmltext'    
)                            
  
  
Insert into T_SMCorporateActions 
(
	CorpActionID, 
	CorporateAction, 
	EffectiveDate, 
	CorporateActionType, 
	IsApplied, 
	Symbol,
	UTCInsertionTime
)                 
Select 
	CorpActionID,
	CorporateAction, 
	EffectiveDate, 
	CorporateActionType, 
	@isApplied, 
	OrigSymbol,
	getutcdate() 
	From #TempCorp              
	Where #TempCorp.CorpActionID not in (Select CorpActionID from T_SMCorporateActions)              
  
  
  
Update T_SMCorporateActions              
Set 
IsApplied = @isApplied, 
CorporateAction = Temp.CorporateAction,
EffectiveDate = Temp.EffectiveDate,
Symbol =  Temp.OrigSymbol,
UTCInsertiontime = getutcdate()
From #TempCorp Temp
Inner Join T_SMCorporateActions SMCA On SMCA.CorpActionID =  Temp.CorpActionID           
Where Temp.CorpActionID in (Select CorpActionID from #TempCorp)              
  
Drop Table #TempCorp              
  
exec sp_xml_removedocument @handle

END TRY                                                                                        
BEGIN CATCH                                                 
 SET @ErrorMessage = ERROR_MESSAGE()                                                       
 SET @ErrorNumber = Error_number()                                  
      Drop Table #TempCorp                         
END CATCH     
  

