


/****** Object:  Stored Procedure dbo.P_GetAllModules    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAllModules
AS
	SELECT     ModuleID, ModuleName
	FROM         T_Module order by ModuleName



