


/****** Object:  Stored Procedure dbo.P_IsAdmin    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_IsAdmin
(
		@userID int		
)
AS
Declare @totalPermisions int
Declare @userPermisions int

Set @totalPermisions = 0
Set @userPermisions = 0

Select @totalPermisions = Count(1)
FROM T_Permission 

SELECT  @userPermisions = COUNT(1) 
FROM         T_User INNER JOIN
                      T_UserPermission ON T_User.UserID = T_UserPermission.UserID INNER JOIN
                      T_Permission ON T_UserPermission.PermissionID = T_Permission.PermissionID
WHERE     (T_User.UserID = @userID)


if(@userPermisions = @totalPermisions)
Begin
	Select 1
End
else
Begin
	Select 0
End
	



