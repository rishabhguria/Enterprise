/*
	ScriptType: General
	Description: To rename the existing preference name from PST to PTT in T_AL_AllocationPreferenceDef
	Created By: Shubham Awasthi
	Dated: 16 March 2017
*/


IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='T_AL_AllocationPreferenceDef')
AND EXISTS(Select * from T_AL_AllocationPreferenceDef where NAME LIKE '%*PST#_%')
	BEGIN
	UPDATE T_AL_AllocationPreferenceDef
	SET NAME=REPLACE(NAME,'*PST#_','*PTT#_')
	WHERE NAME LIKE '%*PST#_%'
	END

