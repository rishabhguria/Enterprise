IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_FutureMultipliers'
			AND COLUMN_NAME = 'CutOffTime'
		)
	
BEGIN
    Update T_FutureMultipliers  set  CutOffTime=null
    FROM T_FutureMultipliers where CutOffTime  =('NULL')
END