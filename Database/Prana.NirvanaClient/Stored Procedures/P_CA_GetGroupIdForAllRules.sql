Create PROCEDURE [dbo].[P_CA_GetGroupIdForAllRules] 
AS
BEGIN
		SELECT RuleId,GroupId from T_CA_RulesUserDefined 
END



