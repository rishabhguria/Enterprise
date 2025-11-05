


/****** Object:  Stored Procedure dbo.P_DeleteUser    Script Date: 11/17/2005 9:50:20 AM ******/
CREATE PROCEDURE dbo.P_DeleteUser
	(
		@userID int	
	)
AS
Delete T_UserPermission Where UserID = @userID


Delete T_User
Where UserID = @userID
	--And UserID <> 37



