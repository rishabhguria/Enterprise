
--Modified By Sachin mishra
-- Dated 03/02/15
-- Purpose: For saving master_funt to all users (http://jira.nirvanasolutions.com:8080/browse/CHMW-2460)
--------------------------------------------------------
--Modified By Faisal Shah
-- Dated 21/07/14
-- Needed to handle Updation Deletion and Addition Separately
--------------------------------------------------------
--Created By: Bhavana
--Date: 23/06/14
--Purpose: To save details of the master fund alongwith fund mapping details for CH Release.
---------------------------------------------------------
 
CREATE PROCEDURE [dbo].[P_SaveFundMasterFundMappingForCH]          
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
                       
                                   
       
BEGIN TRAN TRAN1         
 BEGIN TRY     
DECLARE @handle int                          
exec sp_xml_preparedocument @handle OUTPUT,@XMLDoc 

DECLARE @handle1 int                           
exec sp_xml_preparedocument @handle1 OUTPUT,@XMLMDoc             
      
 CREATE TABLE #TempTableNames                                                                               
  (                                                                               
    CompanyMasterFundId int,      
	CompanyFundId int                  
   )        
      
INSERT INTO #TempTableNames                     
 (                                                                              
    CompanyMasterFundId                        
   ,CompanyFundId                                                      
 )                                                                              
SELECT                                                                               
	CompanyMasterFundId                        
   ,CompanyFundId                                    
    FROM OPENXML(@handle, '/DSFundMasterFundMapping/TABFundMasterFundMapping', 2)                                                                                 
 WITH                                                                               
 (                                                         
   CompanyMasterFundId int,      
CompanyFundId int            
 )                          
 

CREATE TABLE #TempTableNames1                                                                               
  (                                                                               
    CompanyMasterFundId int,      
	MasterFundName Nvarchar(100),
CompanyID int,
QueryType  int                  
   )        
      
INSERT INTO #TempTableNames1                     
 (                                                                              
    CompanyMasterFundId                        
   ,MasterFundName,
CompanyID,
QueryType                                                    
 )                                                                              
SELECT                                                                               
	CompanyMasterFundID                        
   ,MasterFundName
,CompanyID 
,QueryType                                 
    FROM OPENXML(@handle1, '/NewDataSet/Table1', 2)                                                                                 
 WITH                                                                               
 (                                                         
   CompanyMasterFundID int,      
MasterFundName Nvarchar(100),
CompanyID int , 
QueryType  int           
 )        
If (Select COUNT(*) from #TempTableNames1 where MasterFundName  IN (SELECT T_CompanyMasterFunds.MasterFundName FROM T_CompanyMasterFunds)) > 0
BEGIN 
SET @ErrorNumber = -11
SELECT @ErrorNumber
END
ELSE
If (Select COUNT(*) from #TempTableNames1 WHERE QueryType = 0) > 0
BEGIN
SET IDENTITY_INSERT T_CompanyMasterFunds ON
insert into T_CompanyMasterFunds (CompanyMasterFundId, MasterFundName, CompanyID) select CompanyMasterFundId,MasterFundName, CompanyID from #TempTableNames1
where CompanyMasterFundId not in (SELECT CompanyMasterFundID from T_CompanyMasterFunds WHERE CompanyID=@companyID)


END

ELSE IF (Select COUNT(*) from #TempTableNames1 WHERE QueryType = 2) > 0
BEGIN

-- To set isActive to false for deleted master funds
Update T_CompanyMasterFunds SET IsActive=0
where CompanyMasterFundID in(select CompanyMasterFundId from #TempTableNames1 WHERE QueryType = 2)
END
ELSE IF (Select COUNT(*) from #TempTableNames1 WHERE QueryType = 1) > 0
BEGIN
Update T_CompanyMasterFunds 
Set MasterFundName = TN.MasterFundName
from T_CompanyMasterFunds TC
inner join #TempTableNames1 TN ON TN.CompanyMasterFundId = TC.CompanyMasterFundID
where TN.QueryType =1
 
END

-- Deletion from mapping table for existing master funds
delete MFA from T_CompanyMasterFundSubAccountAssociation as MFA
left join T_CompanyMasterFunds MF on MF.CompanyMasterFundID = MFA.CompanyMasterFundID
where MF.CompanyID =@companyID

-- Insertion of mapping details of existing master funds in mapping table
Insert into T_CompanyMasterFundSubAccountAssociation (CompanyMasterFundId,CompanyFundId) select CompanyMasterFundId,CompanyFundId from #TempTableNames       

SELECT @ErrorNumber
-- Insertion of new master funds in T_CompanyMasterFunds
--SET IDENTITY_INSERT T_CompanyMasterFunds ON
--insert into T_CompanyMasterFunds (CompanyMasterFundId, MasterFundName, CompanyID) select CompanyMasterFundId,MasterFundName, CompanyID from #TempTableNames1
--where CompanyMasterFundId not in (SELECT CompanyMasterFundID from T_CompanyMasterFunds WHERE CompanyID=@companyID)

-- Insertion of mapping details of existing master funds in mapping table
--Insert into T_CompanyMasterFundSubAccountAssociation (CompanyMasterFundId,CompanyFundId) select CompanyMasterFundId,CompanyFundId from #TempTableNames       
     
EXEC sp_xml_removedocument @handle   
EXEC sp_xml_removedocument @handle1                          
                       
COMMIT TRANSACTION TRAN1                        
END TRY       
                     
BEGIN CATCH                         
SET @ErrorMessage = ERROR_MESSAGE();                        
  print @errormessage                        
SET @ErrorNumber = Error_number();                
ROLLBACK TRANSACTION TRAN1                       
END CATCH;
