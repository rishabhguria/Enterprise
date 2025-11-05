IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_CompanyExecutionsInstructions'
			AND COLUMN_NAME = 'CompanyID'
		)
	AND EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_CompanyExecutionsInstructions'
			AND COLUMN_NAME = 'ExecutionInstructionsID'
		)
BEGIN
	INSERT INTO T_CompanyExecutionsInstructions (
		CompanyID
		,ExecutionInstructionsID
		)
	SELECT CompanyID AS CompanyID
		,ExecutionInstructionsID AS ExecutionInstructionsID
	FROM T_Company
		,T_ExecutionInstructions
END