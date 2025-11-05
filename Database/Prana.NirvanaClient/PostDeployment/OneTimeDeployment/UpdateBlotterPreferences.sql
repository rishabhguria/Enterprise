IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='T_pranauserprefs')
BEGIN
	-- Delete Current blotter Preferences.Dat file
	DELETE FROM T_pranauserprefs WHERE FileName = 'Blotter Preferences.dat' OR FileName = 'Blotter Preferences.xml'
	DELETE FROM T_pranauserprefs WHERE FileName like '%BlotterGridLayout.xml'

	-- Add Preferences.Dat file with default values for all users
	INSERT INTO T_pranauserprefs(UserID, FileName, FileData, LastSaveTime)
	SELECT UserID, 'Blotter Preferences.dat', 0x0001000000FFFFFFFF010000000000000006010000001D302C302C303B302C302C303B302C302C303B302C302C303B46616C73650B, GETDATE() FROM T_CompanyUser
END
