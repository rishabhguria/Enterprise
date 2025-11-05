



Create PROCEDURE [dbo].[P_CA_GetAlertCount]
@UserId int
as

SELECT 
	DISTINCT 
	a.RuleType, 
	s.RuleName, 
	a.Dimension,
	COUNT(*)  as TriggerCount
from 
	T_CA_AlertHistory as a RIGHT OUTER JOIN 
	T_CA_RulesUserDefined as s on a.RuleId = s.RuleId
where a.ruleId <> '-1' 
	and  DATEDIFF(d, a.ValidationTime,GETDATE())=0 
	and a.UserId in (@UserId,0)
GROUP BY a.RuleType, s.RuleName, a.Dimension





