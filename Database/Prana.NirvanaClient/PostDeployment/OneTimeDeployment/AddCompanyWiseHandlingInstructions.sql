IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_CompanyHandlingInstructions'
			AND COLUMN_NAME = 'CompanyID'
		)
	AND EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_CompanyHandlingInstructions'
			AND COLUMN_NAME = 'HandlingInstructionsID'
		)
BEGIN
	INSERT INTO T_CompanyHandlingInstructions (
		CompanyID
		,HandlingInstructionsID
		)
	SELECT CompanyID AS CompanyID
		,HandlingInstructionsID AS HandlingInstructionsID
	FROM T_Company
		,T_HandlingInstructions
END