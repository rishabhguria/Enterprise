IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_CA_RulesUserDefined'
			AND COLUMN_NAME = 'EmailSubject'
		)
BEGIN
	UPDATE T_CA_RulesUserDefined SET EmailSubject = 'Nirvana: Compliance and Alerting - ' + PackageName +': '
END

IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_CA_RuleGroupSettings'
			AND COLUMN_NAME = 'EmailSubject'
		)
BEGIN
	UPDATE T_CA_RuleGroupSettings SET EmailSubject = 'Nirvana: Compliance and Alerting - PostTrade: '
END