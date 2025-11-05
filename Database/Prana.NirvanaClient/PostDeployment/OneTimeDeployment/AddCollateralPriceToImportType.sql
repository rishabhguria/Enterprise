IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PM_TableTypes')
	BEGIN
	Insert Into PM_TableTypes
	(TableTypeName, Acronym)
	Values('Collateral Price','DailyCollateralPrice')
	END
