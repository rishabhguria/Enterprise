create PROCEDURE [dbo].[P_CA_GetGroupIdForRules] 
	
AS
BEGIN
	
	SELECT RuleId,GroupId from T_CA_RulesUserDefined where PackageName='PostTrade'
END
