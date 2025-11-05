-----------------------------------------------------------------
--Created BY: Bhavana
--Date: 2-june-14
--Purpose: Save the release details
-----------------------------------------------------------------
CREATE procedure [dbo].[P_SaveReleaseDetails]
(@xmlDoc ntext)
as
declare @handle int
exec sp_xml_preparedocument @handle OUTPUT,@xmlDoc

---------- Updation in Release details -----------------------------------------------
CREATE TABLE #TempReleaseUpdate                                                                               
(                                                                               
    ReleaseID int,      
	CompanyID int,
	FundID int,
    ReleaseName varchar(100),
	IP varchar(100),
	ReleasePath varchar(500) ,
	ClientDB_Name varchar(100),
	SMDB_Name varchar(100)
)        
insert INTO #TempReleaseUpdate                                                                               
(ReleaseID,CompanyID,FundID,ReleaseName,IP,ReleasePath,ClientDB_Name,SMDB_Name)
SELECT                                                                               
ReleaseID,CompanyID,FundID,ReleaseName,IP,ReleasePath,ClientDB_Name,SMDB_Name                                      
FROM OPENXML(@handle, '/dsReleaseDetail/dtReleaseDetail', 2)                                                                                 
WITH                                                                               
(                                                         
	ReleaseID int,      
	CompanyID int,
	FundID int,
    ReleaseName varchar(100),
	IP varchar(100),
	ReleasePath varchar(500) ,
	ClientDB_Name varchar(100),
	SMDB_Name varchar(100)
)
where ReleaseID != 0

UPDATE T_CompanyReleaseDetails  
SET ReleaseName = TU.ReleaseName,
IP = TU.IP,
ReleasePath = TU.ReleasePath,
ClientDB_Name = TU.ClientDB_Name,
SMDB_Name = TU.SMDB_Name
from #TempReleaseUpdate TU
INNER JOIN T_CompanyReleaseDetails ON T_CompanyReleaseDetails.CompanyReleaseID = TU.ReleaseID

DELETE FROM T_CompanyReleaseMapping

Insert INTO T_CompanyReleaseMapping(ReleaseID, CompanyID) SELECT ReleaseID,CompanyID from #TempReleaseUpdate

------ Deletion from T_CompanyReleaseDetails --------------------------------------------
DELETE FROM T_CompanyReleaseDetails where T_CompanyReleaseDetails.CompanyReleaseID not in 
(SELECT ReleaseID FROM #TempReleaseUpdate) 
------------------------------------------------------------------------------------------

------------------- Insertion of new Release -------------------------------
CREATE TABLE #TempReleaseInsert                                                                               
(                                                                               
    ReleaseID int,      
	CompanyID int,
	FundID int,
    ReleaseName varchar(100),
	IP varchar(100),
	ReleasePath varchar(500) ,
	ClientDB_Name varchar(100),
	SMDB_Name varchar(100)
)        
insert INTO #TempReleaseInsert                                                                               
(ReleaseID,CompanyID,FundID,ReleaseName,IP,ReleasePath,ClientDB_Name,SMDB_Name)
SELECT                                                                               
ReleaseID,CompanyID,FundID,ReleaseName,IP,ReleasePath,ClientDB_Name,SMDB_Name                                      
FROM OPENXML(@handle, '/dsReleaseDetail/dtReleaseDetail', 2)                                                                                 
WITH                                                                               
(                                                         
	ReleaseID int,      
	CompanyID int,
	FundID int,
    ReleaseName varchar(100),
	IP varchar(100),
	ReleasePath varchar(500) ,
	ClientDB_Name varchar(100),
	SMDB_Name varchar(100)
)
where ReleaseID = 0                     

Insert into T_CompanyReleaseDetails  
(
ReleaseName,
IP,
ReleasePath,
ClientDB_Name,
SMDB_Name)
select 
DISTINCT
ReleaseName,
IP,
ReleasePath,
ClientDB_Name,
SMDB_Name
from #TempReleaseInsert

Update #TempReleaseInsert SET 
ReleaseID = TP.CompanyReleaseID
FROM T_CompanyReleaseDetails TP
INNER JOIN #TempReleaseInsert ON #TempReleaseInsert.ReleaseName = TP.ReleaseName

Insert INTO T_CompanyReleaseMapping(ReleaseID, CompanyID) SELECT ReleaseID,CompanyID from #TempReleaseInsert

------- Updation in T_CompanyFunds for ReleaseID----------------------------------------------------
Update T_CompanyFunds SET FundReleaseID = NULL

Update T_CompanyFunds SET 
FundReleaseID = TP.ReleaseID
from #TempReleaseUpdate TP
INNER JOIN T_CompanyFunds ON T_CompanyFunds.CompanyFundID = TP.FundID

Update T_CompanyFunds SET 
FundReleaseID = TP.ReleaseID
from #TempReleaseInsert TP
INNER JOIN T_CompanyFunds ON T_CompanyFunds.CompanyFundID = TP.FundID
exec sp_xml_removedocument @handle

