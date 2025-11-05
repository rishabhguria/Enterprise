CREATE PROCEDURE dbo.P_GetAllPermissions
AS
	/* SELECT   PermissionID, PermissionName
FROM         T_Permission */

SELECT   PermissionID, PermissionTypeID, ModuleID
FROM         T_Permission

