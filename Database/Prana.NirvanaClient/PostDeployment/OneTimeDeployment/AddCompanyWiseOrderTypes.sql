IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_CompanyOrderTypes'
			AND COLUMN_NAME = 'CompanyID'
		)
	AND EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_CompanyOrderTypes'
			AND COLUMN_NAME = 'OrderTypeID'
		)
BEGIN
	INSERT INTO T_CompanyOrderTypes (
		CompanyID
		,OrderTypeID
		)
	SELECT CompanyID AS CompanyID
		,OrderTypesID AS OrderTypeID
	FROM T_Company
		,T_OrderType
END