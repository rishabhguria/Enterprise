
create PROCEDURE [dbo].[P_CA_GetGroupValidationTime] 
	-- Add the parameters for the stored procedure here
	
AS
SELECT  distinct  t.RuleID,Dimension,rud.GroupID,Max(ValidationTime) AS ValidationTime
	FROM         T_CA_AlertHistory as t
INNER JOIN T_CA_RulesUserDefined rud ON t.RuleID=rud.RuleID
	WHERE  t.RuleType='PostTrade' 
	GROUP BY t.RuleID,t.Dimension,rud.GroupId
order by validationTime asc
