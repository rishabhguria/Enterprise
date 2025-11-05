CREATE PROCEDURE dbo.P_GetPermissionType
	(
		@permissionTypeID int
	)
AS
	SELECT     PermissionTypeID, PermissionType
	FROM         T_PermissionType
	Where PermissionTypeID = @permissionTypeID
