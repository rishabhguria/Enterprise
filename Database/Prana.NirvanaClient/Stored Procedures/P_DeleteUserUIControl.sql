CREATE PROCEDURE dbo.P_DeleteUserUIControl
	
	(
	@userID int ,
	@auecID int
	)
	
AS
	DELETE FROM T_RMCompanyUserUI
	WHERE        (CompanyUserID = @userID) AND (CompanyUserAUECID = @auecID)
