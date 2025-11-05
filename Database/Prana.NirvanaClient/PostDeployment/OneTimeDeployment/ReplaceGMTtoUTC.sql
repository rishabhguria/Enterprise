/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 /*
	ScriptType: General
	Description: Replace GMT into UTC in T_AUEC and T_Exchange
	Created By: Gourav Soni
	Dated: 31-10-2019
*/

--------------------------------------------------------------------------------------
*/
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='T_AUEC' AND COLUMN_NAME='TimeZone')
BEGIN
	UPDATE T_AUEC
		SET TimeZone = REPLACE(TimeZone, 'GMT', 'UTC')
		WHERE TimeZone LIKE '%GMT%'
END

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='T_Exchange' AND COLUMN_NAME='TimeZone')
BEGIN
	UPDATE T_Exchange
		SET TimeZone = REPLACE(TimeZone, 'GMT', 'UTC')
		WHERE TimeZone LIKE '%GMT%'
END
