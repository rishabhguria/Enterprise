
--Modified By Sachin mishra
-- Dated 03/02/15
-- Purpose: For saving master_funt to all users (http://jira.nirvanasolutions.com:8080/browse/CHMW-2460)
------------------------------------------------------------------
---Modified By Faisal Gani Shah
---Dated 21/07/14
---Needed to separately add delete or modify MasterStrategies.
-----------------------------------------------------------------
--Created BY: Bharat Raturi
--Date: 28/02/14
--Purpose: Save the strategy to master strategy mapping
-----------------------------------------------------------------
CREATE PROCEDURE [dbo].[P_SaveStrategyMasterStrategyMappingForCH]          
(      
  @XMLDoc nText 
 ,@XMLMDoc nText                      
 , @ErrorMessage varchar(500) output                        
 , @ErrorNumber int output
, @companyID int                       
 )               
AS          
      
SET @ErrorMessage = 'Success'                        
SET @ErrorNumber = 0       
                                                       
      
        
BEGIN TRY  
BEGIN TRANSACTION TRAN1    
DECLARE @handle int                           
exec sp_xml_preparedocument @handle OUTPUT,@XMLDoc 

DECLARE @handle1 int                           
exec sp_xml_preparedocument @handle1 OUTPUT,@XMLMDoc             
      
 CREATE TABLE #TempTableNames                                                                               
  (                                                                               
    CompanyMasterStrategyId int,      
	CompanyStrategyId int                
   )        
      
INSERT INTO #TempTableNames                     
 (                                                                              
    CompanyMasterStrategyId                        
   ,CompanyStrategyId                                                         
 )                                                                              
SELECT                                                                               
	CompanyMasterStrategyId                        
   ,CompanyStrategyId                                        
    FROM OPENXML(@handle, '/DSStrategyMasterStrategyMapping/TABStrategyMasterStrategyMapping', 2)                                                                                 
 WITH                                                                               
 (                                                         
   CompanyMasterStrategyId int,      
CompanyStrategyId int                  
 )                          
 

CREATE TABLE #TempTableNames1                                                                               
  (                                                                               
    CompanyMasterStrategyId int,      
	MasterStrategyName Nvarchar(100),
     CompanyId int ,
	QueryType int                         
   )        
      
INSERT INTO #TempTableNames1                     
 (                                                                              
    CompanyMasterStrategyId                        
   ,MasterStrategyName ,
     CompanyId  ,
	QueryType                                                            
 )                                                                              
SELECT                                                                               
	CompanyMasterStrategyID                        
   ,MasterStrategyName,
CompanyId ,
QueryType                                   
    FROM OPENXML(@handle1, '/NewDataSet/Table1', 2)                                                                                 
 WITH                                                                               
 (                                                         
   CompanyMasterStrategyID int,      
MasterStrategyName Nvarchar(100),
CompanyId  int  ,
QueryType INT          
 )        

If (Select COUNT(*) from #TempTableNames1 where MasterStrategyName IN (SELECT T_CompanyMasterStrategy.MasterStrategyName FROM T_CompanyMasterStrategy)) > 0
BEGIN 
SET @ErrorNumber = -11
SELECT @ErrorNumber
END
ELSE
If (Select COUNT(*) from #TempTableNames1 WHERE QueryType = 0) > 0
BEGIN
SET IDENTITY_INSERT T_CompanyMasterStrategy ON
insert into T_CompanyMasterStrategy (CompanyMasterStrategyId,MasterStrategyName,CompanyID) select CompanyMasterStrategyId,MasterStrategyName,CompanyId from #TempTableNames1 
where CompanyMasterStrategyId not in (SELECT CompanyMasterStrategyId from T_CompanyMasterStrategy WHERE CompanyID=@companyID)

END
ELSE IF (Select COUNT(*) from #TempTableNames1 WHERE QueryType = 2) > 0
BEGIN

-- To set isActive to false for deleted master strategy
Update T_CompanyMasterStrategy SET IsActive = 0
where CompanyMasterStrategyID  IN(select CompanyMasterStrategyId from #TempTableNames1 WHERE QueryType = 2)

END
ELSE IF (Select COUNT(*) from #TempTableNames1 WHERE QueryType = 1) > 0
BEGIN
Update T_CompanyMasterStrategy 
Set MasterStrategyName = TN.MasterStrategyName
from T_CompanyMasterStrategy TC
inner join #TempTableNames1 TN ON TN.CompanyMasterStrategyId = TC.CompanyMasterStrategyID
where TN.QueryType =1
 
END

-- Deletion from mapping table for existing master strategy
--Modified By : sachin mishra 03-02-15 http://jira.nirvanasolutions.com:8080/browse/CHMW-2460 for saving strategyFund 
delete MFA from T_CompanyMasterStrategySubAccountAssociation as MFA
left join T_CompanyMasterStrategy MF on MF.CompanyMasterStrategyID = MFA.CompanyMasterStrategyID
where MF.CompanyID =@companyID 

-- Insertion of mapping details of existing master strategy in mapping table
Insert into T_CompanyMasterStrategySubAccountAssociation (CompanyMasterStrategyId,CompanyStrategyId) select CompanyMasterStrategyId,CompanyStrategyId from #TempTableNames       

SELECT @ErrorNumber
-- Insertion of new master strategy in T_CompanyMasterStrategy
--SET IDENTITY_INSERT T_CompanyMasterStrategy ON
--insert into T_CompanyMasterStrategy (CompanyMasterStrategyId,MasterStrategyName,CompanyID) select CompanyMasterStrategyId,MasterStrategyName,CompanyId from #TempTableNames1 
--where CompanyMasterStrategyId not in (SELECT CompanyMasterStrategyId from T_CompanyMasterStrategy WHERE CompanyID=@companyID)

-- Insertion of mapping details of existing master strategy in mapping table
--Insert into T_CompanyMasterStrategySubAccountAssociation (CompanyMasterStrategyId,CompanyStrategyId) select CompanyMasterStrategyId,CompanyStrategyId from #TempTableNames       
      
EXEC sp_xml_removedocument @handle   
EXEC sp_xml_removedocument @handle1                          
                       
                       
COMMIT TRANSACTION TRAN1 
END TRY       
                     
BEGIN CATCH                         
SET @ErrorMessage = ERROR_MESSAGE();                        
print @errormessage                        
SET @ErrorNumber = ERROR_NUMBER();                
ROLLBACK TRANSACTION TRAN1                      
END CATCH;
