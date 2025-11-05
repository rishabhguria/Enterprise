IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PM_Preferences')
BEGIN 
UPDATE PM_Preferences
set IsShowPMToolbar =0 where IsShowPMToolbar is Null
End