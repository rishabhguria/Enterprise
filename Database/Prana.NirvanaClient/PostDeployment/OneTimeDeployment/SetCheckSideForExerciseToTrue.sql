IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_GlobalClosingPreferences'
			AND COLUMN_NAME = 'SplitunderlyingBasedOnPosition'
		)
	BEGIN
	Update T_GlobalClosingPreferences
	set SplitunderlyingBasedOnPosition = 1
	END
