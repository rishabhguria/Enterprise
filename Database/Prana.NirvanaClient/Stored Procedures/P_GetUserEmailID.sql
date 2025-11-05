CREATE PROCEDURE dbo.P_GetUserEmailID
	(
		@userID int
	)
AS
	SELECT        EMail
	FROM            T_User
	WHERE        (UserID = @userID)
