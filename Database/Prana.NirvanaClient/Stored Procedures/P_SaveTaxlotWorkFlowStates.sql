CREATE Procedure [dbo].[P_SaveTaxlotWorkFlowStates]                                                                                                                             
(                                                                                                                                                                                                  
@xml  nText ,                                                                                                                                                                                                  
@ErrorMessage varchar(500) output,                                                                                                                            
@ErrorNumber int output                                                                                                                                                                                                                  
)                                                                                                                                                                                                    
as                                                                                                                                                                                                  
SET @ErrorNumber = 0                                                                                                                                                                                                                              
SET @ErrorMessage = 'Success'                                                                                                                                                                                                                              
                                                                                                                                                                                                                              
BEGIN TRAN TRAN1                                                                                                                                                                                                                            
BEGIN TRY                                                                                                                                                                                                   
DECLARE @handle int          
               
exec sp_xml_preparedocument @handle OUTPUT,@xml                           
                                                                                                                                                                                               
create table #Temp_Taxlots                                                                                                                                                                                                                  
(                                                                                                                                                                                                                  
 TaxLotID varchar(50), 
 WorkflowStateID int Null                  
)                                                                                                          
                                                                               
Insert into #Temp_Taxlots                                                
(                                                                                                                                   
 TaxLotID,
 WorkflowStateID
)                                      
        
Select                                                                      
 TaxLotID,                                                               
 WorkflowStateID                                                      
        
FROM  OPENXML(@handle, '//workflowStates',2)                                                
WITH                                                             
(                                                                                                                                     
 TaxLotID varchar(50) ,                                       
 WorkflowStateID INT                                                
)                                                                                                           

-------- update taxlot wise workflowState  
update  T_TaxlotWorkflowState 
SET WorkflowStateID = temp.workflowStateID,
LastUpdatedTime = GETDATE(),
Comments =t.Comments +','+W.WorkFlowState
from  T_TaxlotWorkflowState t WITH (NOLOCK)
inner JOIN #Temp_Taxlots temp ON t.TaxlotID = temp.TaxLotID
inner JOIN T_WorkflowStats W ON W.WorkflowStatID = temp.WorkflowStateID

-- Insert for taxlot wise workflowState                     
INSERT INTO T_TaxlotWorkflowState WITH (ROWLOCK)                                             
(                                                                                                      
  TaxlotID,
  WorkflowStateID,
  LastUpdatedTime,
  Comments                                                                                       
)                       
Select   
temp.TaxLotID,
temp.workflowStateID,
GETDATE(),
W.WorkFlowState
--select *
from #Temp_Taxlots temp
inner JOIN T_WorkflowStats W ON W.WorkflowStatID = temp.WorkflowStateID
where temp.TaxLotID not in(select TaxLotID from T_TaxlotWorkflowState) and temp.TaxLotID <>''

                                             
exec sp_xml_removedocument @handle    
COMMIT TRANSACTION TRAN1   
drop table #Temp_Taxlots                                                                                                                                
END TRY                                                                    
                                                                      
BEGIN CATCH                                                                                                                      
 SET @ErrorMessage = ERROR_MESSAGE();                                   
 SET @ErrorNumber = Error_number();                                                                                                     
  ROLLBACK TRANSACTION TRAN1    
END CATCH;     
    

  
