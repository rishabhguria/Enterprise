CREATE PROCEDURE [dbo].[P_GetUserPermissions]
(
@userID int
)
AS
/* SELECT T_Permission.PermissionID, T_Permission.PermissionName
FROM T_UserPermission INNER JOIN
T_Permission ON T_UserPermission.PermissionID = T_Permission.PermissionID
WHERE T_UserPermission.UserID = @userID
Order By T_Permission.PermissionID */

SELECT T_Permission.PermissionID, T_Permission.PermissionTypeID, T_Permission.ModuleID
FROM T_UserPermission INNER JOIN
T_Permission ON T_UserPermission.PermissionID = T_Permission.PermissionID
WHERE T_UserPermission.UserID = @userID
Order By T_Permission.PermissionID
