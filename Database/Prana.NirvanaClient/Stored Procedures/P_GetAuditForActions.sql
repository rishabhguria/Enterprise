
CREATE PROCEDURE [dbo].[P_GetAuditForActions] 	
	@ActionList XML

AS begin

WITH AuditData(AuditDimensionValue,ActionId,LastExecutionTime) AS
(
SELECT 
AuditDimensionValue,
ActionId, 
MAX(ActualActionDate) LastExecutionTime
FROM T_AdminAuditTrail
where ActionID in (
            select ParamValues.ID.value('.','VARCHAR(50)') 
                    FROM @ActionList.nodes('/ActionIds/ActionId') as ParamValues(id)
            ) 
AND IsActive = 1
group by AuditDimensionValue, ActionId
)
SELECT 
	TA.CompanyId,
	TA.CompanyFundId,
	TA.UserId,
	TA.ActionId,
	TA.ActualActionDate,
    TA.ExecutionTime,
	TA.Comments,
	TA.ModuleId,
    TA.StatusID,
	TA.AuditDimensionValue,
    TA.IsActive
FROM AuditData AS A left outer join 
T_AdminAuditTrail TA 
	on TA.AuditDimensionValue=A.AuditDimensionValue 
	AND TA.ActionID=A.ActionID
	AND TA.ActualActionDate = A.LastExecutionTime

END
