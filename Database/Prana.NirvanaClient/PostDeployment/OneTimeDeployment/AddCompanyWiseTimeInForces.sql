IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_CompanyTimeInForce'
			AND COLUMN_NAME = 'CompanyID'
		)
	AND EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_CompanyTimeInForce'
			AND COLUMN_NAME = 'TimeInForceID'
		)
BEGIN
	INSERT INTO T_CompanyTimeInForce (
		CompanyID
		,TimeInForceID
		)
	SELECT CompanyID AS CompanyID
		,TimeInForceID AS TimeInForceID
	FROM T_Company
		,T_TimeInForce
END