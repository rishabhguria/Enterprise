CREATE PROCEDURE P_CA_UpdateRenamedRuleAlerts 
	-- Add the parameters for the stored procedure here
	@OldRuleId varchar(50), 
	@NewRuleId varchar(50)
AS
BEGIN
	UPDATE T_CA_AlertHistory
SET RuleId=@NewRuleId
WHERE RuleId=@OldRuleId;
END

BEGIN
	UPDATE T_CA_RulesUserDefined
SET RuleId=@NewRuleId
WHERE RuleId=@OldRuleId;
END

BEGIN
	UPDATE T_CA_RuleUserPermissions
SET RuleId=@NewRuleId
WHERE RuleId=@OldRuleId;
END
