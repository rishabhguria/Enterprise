-----------------------------------------------------------------  
  
--modified BY: Omshiv  
--Date: 1/11/14  
--Purpose: Save funds  Workflow Stats

-----------------------------------------------------------------  
CREATE Proc [dbo].[P_SaveWorkflowStats]                                    
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
                                     
 CREATE TABLE #Temp                                          
 (                                                                                           
  EventRunTime datetime                 
  ,FileExecutionDate datetime                                  
  ,EventID int                  
  ,FundID int
  ,Comments Varchar(500),
  ContextID int,
  StatusID int,
  ContextValue Varchar(200)                              
 )                                                 
                                                                                          
 INSERT INTO #Temp                                
 (                                                                               
   EventRunTime                  
  ,FileExecutionDate                                   
  ,EventID                   
  ,FundID 
  ,Comments 
  ,ContextID
  ,StatusID,
ContextValue             
  )                                                                                          
 SELECT                                                                                          
   EventRunTime                  
  ,FileExecutionDate                                   
  ,EventID                   
  ,FundID 
  ,Comments 
  ,ContextID
  ,StatusID,
  ContextValue  
                          
                                   
 FROM OPENXML(@handle, '//Sheet1', 2)                                                                                             
  WITH                                                                                           
  (                                                                     
  EventRunTime datetime                 
  ,FileExecutionDate datetime                                  
  ,EventID int                  
  ,FundID int
  ,Comments Varchar(500),
  ContextID int,
  StatusID int,
  ContextValue Varchar(200)                                     
  )         

--Select * from #Temp

update T_FundWorkflowStats
SET StateID = StatusID,
Comments = T.Comments
--SELECT * 
from T_FundWorkflowStats FW
inner JOIN #Temp T ON FW.Date =T.FileExecutionDate and FW.FundID =T.FundID and FW.TaskID =T.EventID and FW.ContextValue =T.ContextValue
--


 INSERT INTO          
 T_FundWorkflowStats                                    
 (                                    
   Date
  ,TaskRunTime                 
  ,FundID                                   
  ,TaskID                   
  ,StateID 
  ,Comments
  ,ContextID
  ,ContextValue
   
 )                               

 SELECT  Distinct                                   
   T.FileExecutionDate
  ,T.EventRunTime                  
  ,T.FundID
  ,T.EventID 
  ,T.StatusID  
  ,T.Comments 
  ,T.ContextID
  ,T.ContextValue
  
FROM    #Temp T
        LEFT JOIN T_FundWorkflowStats FW
             ON FW.Date =T.FileExecutionDate and FW.FundID =T.FundID and FW.TaskID =T.EventID and FW.ContextValue =T.ContextValue
WHERE   FW.Date IS NULL

--  
--SELECT * from T_FundWorkflowStats              
 DROP TABLE #Temp                      
                                
 EXEC sp_xml_removedocument @handle                                    
                                     
COMMIT TRANSACTION TRAN1                                    
                               
END TRY
 BEGIN CATCH                                     
  SET @ErrorMessage = ERROR_MESSAGE();                                    
  print @errormessage                                    
  SET @ErrorNumber = Error_number();                                     
  ROLLBACK TRANSACTION TRAN1                                     
END CATCH;

