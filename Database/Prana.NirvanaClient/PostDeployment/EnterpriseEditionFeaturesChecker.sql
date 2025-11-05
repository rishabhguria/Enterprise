IF NOT EXISTS (SELECT * FROM SYS.MESSAGES WHERE MESSAGE_ID = 50001)
	EXEC master.sys.sp_addmessage 50001, 16, N'You are not allowed to use Enterprise Edition Features. Please remove and re-publish';

IF ((SELECT COUNT(*) FROM SYS.DM_DB_PERSISTED_SKU_FEATURES) <> 0)
RAISERROR (50001,-1,-1, '')