


/****** Object:  Stored Procedure dbo.P_DeleteUserPermission    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_DeleteUserPermission
	(
		@userID int
	)

AS
	Delete T_UserPermission
	Where userID = @userID



