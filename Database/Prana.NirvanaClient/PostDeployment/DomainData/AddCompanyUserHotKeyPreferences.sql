IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'T_CompanyUserHotKeyPreferences'	)
BEGIN
	Insert Into T_CompanyUserHotKeyPreferences 
	Select CU.UserID, 'Allocation^Broker^Venue^TIF^Order Type^Execution Type', 0, 0, 0, GETDATE() From T_CompanyUserHotKeyPreferences HP
	Right Join T_CompanyUser CU On CU.UserID = HP.CompanyUserID
	Where HP.CompanyUserID Is Null
END