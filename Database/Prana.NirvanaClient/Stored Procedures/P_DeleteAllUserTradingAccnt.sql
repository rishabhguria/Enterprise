CREATE PROCEDURE dbo.P_DeleteAllUserTradingAccnt
	
	(
	@companyID int
	
	)
	
AS
	DELETE FROM T_RMUserTradingAccount
	WHERE        (CompanyID = @companyID)