
CREATE PROCEDURE dbo.P_GetModule
	(
		@moduleID int
	)
AS
	SELECT     ModuleID, ModuleName
	FROM         T_Module
	Where ModuleID = @moduleID

