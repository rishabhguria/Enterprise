
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='T_AllocationScheme' AND COLUMN_NAME='CreationSource')
BEGIN

	--Set Creation Source to 3(ProrataUI) for Allocation Scheme named Positions'
	UPDATE T_AllocationScheme
	SET CreationSource=3 
	WHERE AllocationSchemeName = 'Positions'

END