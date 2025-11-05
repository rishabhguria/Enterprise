

IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_CompanyTTGeneralPreferences'
			AND COLUMN_NAME = 'IsShowTargetQTY'
		)
BEGIN
	update T_CompanyTTGeneralPreferences set IsShowTargetQTY=1 where IsShowTargetQTY is null
END