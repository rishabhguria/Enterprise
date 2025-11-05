DECLARE @moduleIDLive INT
DECLARE @moduleIDManual INT

SET @moduleIDLive = (Select ModuleID FROM T_Module WHERE ModuleName = 'Live Trading Ticket')
SET @moduleIDManual = (Select ModuleID FROM T_Module WHERE ModuleName = 'Manual Trading Ticket')

DELETE FROM  T_CompanyUserModule where CompanyModuleID = @moduleIDLive
DELETE FROM  T_CompanyModule where ModuleID = @moduleIDLive
Delete from T_LayoutComponentDetails where ModuleID = @moduleIDLive or ModuleID = @moduleIDManual