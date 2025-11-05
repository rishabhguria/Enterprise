IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_CA_RulesUserDefined'
			AND COLUMN_NAME = 'RuleId'
		)
	OR EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_CA_RulesUserDefined'
			AND COLUMN_NAME = 'Uuid'
		)
BEGIN
	UPDATE T_CA_RulesUserDefined SET RuleId = REPLACE(RuleId, 'custom_rule_', 'custom-rule-'), Uuid = REPLACE(Uuid, '_', '-')
END

IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_CA_AlertHistory'
			AND COLUMN_NAME = 'RuleId'
		)
BEGIN
	UPDATE T_CA_AlertHistory SET RuleId = REPLACE(RuleId, 'custom_rule_', 'custom-rule-')
END

IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_CA_ArchivedAlertHistory'
			AND COLUMN_NAME = 'RuleId'
		)
BEGIN
	UPDATE T_CA_ArchivedAlertHistory SET RuleId = REPLACE(RuleId, 'custom_rule_', 'custom-rule-')
END

IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_CA_RulesUserDefined'
			AND COLUMN_NAME = 'RuleId'
		)
BEGIN
	UPDATE T_CA_RuleUserPermissions SET RuleId = REPLACE(RuleId, 'custom_rule_', 'custom-rule-')
END