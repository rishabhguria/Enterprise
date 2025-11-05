-----------------------------------------------------------------  
--modified BY: Omshiv  
--Date: 19/11/14  
--Purpose: only active fund will be consider for NAV lock

--modified BY: Omshiv  
--Date: 25/07/14  
--Purpose: Use status ID to check last lock  

  
--Created by:Bharat Raturi  
--Date:25/03/14  
--Purpose: Show new column Schedule  
-- usage P_GetFundsNAVLockDetails  
  
--modified by:Bharat Raturi  
--Date:29/03/14  
--Purpose: Show new column Schedule  
-- usage P_GetFundsNAVLockDetails  
-----------------------------------------------------------------  
Create PROCEDURE [dbo].[P_GetFundsNAVLockDetails]
  
AS  
Begin  
  
select  
isnull(T1.CompanyFundID,T2.CompanyFundID) as CompanyFundID,  
isnull(T1.CompanyID,T2.CompanyID) as CompanyID,  
T1.UserID as LokedBy,  
T1.ExecutionTime as LokedOn,  
T2.UserID as UnLokedBy,  
T2.ExecutionTime as UnLokedOn,  
COALESCE(T1.ActualActionDate,0) as LockAppliedOn,  
T1.AuditTrailID as LockedAuditTrailId  
into #temp  
from  

(SELECT MAX(AuditTrailID) AS AuditTrailID,MAX(CompanyFundID) AS CompanyFundID,MAX(CompanyID) AS CompanyID ,
ActionID  ,MAX(ExecutionTime) AS ExecutionTime ,MAX(UserID) AS UserID,MAX(ActualActionDate) AS ActualActionDate
FROM T_AdminAuditTrail 
WHERE ActionID=1 AND StatusID=1
GROUP BY CompanyFundID,ActionID) AS T1
LEFT JOIN 
(SELECT MAX(AuditTrailID) AS AuditTrailID,MAX(CompanyFundID) AS CompanyFundID,MAX(CompanyID) AS CompanyID ,
ActionID  ,MAX(ExecutionTime) AS ExecutionTime ,MAX(UserID) AS UserID,MAX(ActualActionDate) AS ActualActionDate
FROM T_AdminAuditTrail 
WHERE ActionID=2 AND StatusID=1
GROUP BY CompanyFundID,ActionID) AS T2 on T2.CompanyFundID = T1.CompanyFundID

  
select c.CompanyID,c.Name, f.CompanyFundID,f.FundName,f.PostingLockScheduleID,tt.LokedBy,LokedOn,UnLokedBy,UnLokedOn,LockAppliedOn, ts.scheduleName,LockedAuditTrailId from T_CompanyFunds f --,PreviousLockDate   
inner JOIN T_Company c on c.CompanyID= f.CompanyID
inner JOIN T_CompanyFunds cf on cf.CompanyFundID= f.CompanyFundID    
left JOIN #temp tt ON f.CompanyFundID= tt.CompanyFundID  
left JOIN T_ScheduleTypes ts on f.PostingLockScheduleID=ts.ScheduleID  
where cf.IsActive = 1
drop TABLE #temp  
end 


