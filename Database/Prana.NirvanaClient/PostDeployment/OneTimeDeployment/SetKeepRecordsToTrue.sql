IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_AttributeNames'
			AND COLUMN_NAME = 'KeepRecord'
		)
	BEGIN
	Update T_AttributeNames
	set KeepRecord = 1
	END
