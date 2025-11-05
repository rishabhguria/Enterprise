
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='T_AL_AllocationPreferenceDef' AND COLUMN_NAME = 'IsPrefVisible')
BEGIN
	-- Set IsPrefVisible to false for Custom, PTT, WorkArea and MasterFund associated preferences
	UPDATE	T_AL_AllocationPreferenceDef
	SET		IsPrefVisible = CASE WHEN Name LIKE '*%#_%'	
								THEN 0
								ELSE 1
							END		
END

	
