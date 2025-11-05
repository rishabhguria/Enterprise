CREATE PROCEDURE dbo.P_DeleteUserTradingAccount 
	
	(
	@tradAccntID int ,
	@userID int
	)
	
AS
	DELETE FROM T_RMUserTradingAccount
	WHERE        (UserTradingAccntID = @tradAccntID) AND (CompanyUserID = @userID)
