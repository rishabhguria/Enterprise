create PROCEDURE [dbo].[P_CA_SetRuleOverRiddenPermission]
(
@userId int,
@Xml nText                      
 , @ErrorMessage varchar(500) output                      
 , @ErrorNumber int output   
 )
 As     

SET @ErrorMessage = 'Success'                      
SET @ErrorNumber = 0     
                     
                                 
BEGIN TRY     
BEGIN TRAN TRAN1       

  delete from T_CA_RuleUserPermissions where   T_CA_RuleUserPermissions.UserId=@userId

DECLARE @handle int                         
exec sp_xml_preparedocument @handle OUTPUT,@Xml         
    
 CREATE TABLE #TempMasterFund                                                                             
  (                                                                             
  [RuleId] VARCHAR(50), 
    [PopUp] BIT  ,  
    [OverRiddenPermission] INT ,             
   )      
    
INSERT INTO #TempMasterFund                  
 (                                                                            
    RuleId,
	PopUp,
	OverRiddenPermission
 )                                                                            
SELECT                                                                             
    RuleId,
	PopUp,
	OverRiddenPermission                               
    FROM OPENXML(@handle, '//Permissions/Permission', 2)                                                                               
 WITH                                                                             
 (                                                       
    RuleId VARCHAR(50) 'RuleId',
	PopUp bit 'PopUp',
	OverRiddenPermission int 'AlertTypePermission'        
 )                                              
 --  select * from #TempMasterFund 
--Delete from T_CA_RuleUserPermissions    

   
Insert into T_CA_RuleUserPermissions(RuleId,ShowPopup,UserId,RuleOverrideType) 
select T.RuleId,T.PopUp,@userId,T.OverRiddenPermission   from #TempMasterFund as T
    select * from T_CA_RuleUserPermissions
drop table #TempMasterFund    
    
EXEC sp_xml_removedocument @handle                      
                      
COMMIT TRANSACTION TRAN1                      
                     
END TRY     
                   
BEGIN CATCH                       
SET @ErrorMessage = ERROR_MESSAGE();                      
--  print @errormessage                      
SET @ErrorNumber = Error_number();              
ROLLBACK TRANSACTION TRAN1                       
END CATCH;

