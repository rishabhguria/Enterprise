-------------------------------------------

--modified BY: omshiv
--date: 25/07/14
--Purpose: update pervious lock record

--Created BY: Bharat raturi
--date: 25/03/14
--Purpose: Save the NAV Lock Details
--P_GetFundsNAVLockDetails
-------------------------------------------
CREATE procedure [dbo].[P_SaveFundsNAVLockDetails]
@xmlDoc nText 
as
declare @handle int
exec sp_xml_preparedocument @handle output, @xmlDoc 
create table #temp
(
CompanyID int, 
CompanyFundID int, 
UserID int, 
ActionID int, 
ExecutionTime datetime, 
ActualActionDate datetime,
LockedAuditTrailId int
)
insert into #temp
(CompanyID, CompanyFundID, UserID, ActionID, ExecutionTime, ActualActionDate,LockedAuditTrailId)

SELECT CompanyID,CompanyFundID, UserID, ActionID,ExecutionTime,ActualActionDate,LockedAuditTrailId 
from openxml(@handle,'dsNavLock/dtLock',2) 
with
(
CompanyID INT,
CompanyFundID INT, 
UserID INT,
ActionID INT,
ExecutionTime DATETIME,
ActualActionDate DATETIME,
LockedAuditTrailId int
)


insert INTO
T_AdminAuditTrail
(CompanyID,CompanyFundID, UserID, ActionID, ExecutionTime, ActualActionDate,StatusID)
SELECT 
CompanyID, CompanyFundID, UserID, ActionID, ExecutionTime, ActualActionDate,1 from #temp

update T_AdminAuditTrail
SET StatusID =0
where AuditTrailID in (select LockedAuditTrailId from #temp WHERE ActionID=2)

exec sp_xml_removedocument @handle
drop table #temp

