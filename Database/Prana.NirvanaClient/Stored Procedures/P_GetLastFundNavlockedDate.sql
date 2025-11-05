-----------------------------------------------------------------

--Modified BY: Omshiv
--Date:25-7-14
--Purpose: Get the last lock date and Auidit Trail ID

--Created BY: Bharat Raturi
--Date: 12-4-14
--Purpose: Get the last lock date for the fund
-----------------------------------------------------------------
CREATE Procedure [dbo].[P_GetLastFundNavlockedDate]
@CurrentLockDate datetime, @fundID int
as
declare @lastLockdate datetime
declare @lastLockAuditTrailId int

SELECT top 1 @lastLockdate =ActualActionDate --ExecutionTime 
FROM T_AdminAuditTrail 
where ActualActionDate<@CurrentLockDate AND CompanyFundID=@fundID and ActionID=1 and StatusID =1
ORDER BY ActualActionDate DESC 

SELECT top 1 @lastLockAuditTrailId = AuditTrailID --ExecutionTime 
FROM T_AdminAuditTrail 
where ActualActionDate<=@CurrentLockDate AND CompanyFundID=@fundID and ActionID=1 and StatusID =1
ORDER BY ActualActionDate DESC 

select  @lastLockdate as lastLockDate, @lastLockAuditTrailId as lastLockAuditTrailId

