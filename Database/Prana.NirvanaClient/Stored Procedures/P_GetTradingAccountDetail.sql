CREATE PROCEDURE dbo.P_GetTradingAccountDetail
	(
	@companyID int,
	@tradAccntID int
	)
	
AS
	SELECT        CompanyTradingAccountsID, TradingAccountName, TradingShortName, CompanyID
	FROM            T_CompanyTradingAccounts
	WHERE        (CompanyTradingAccountsID = @tradAccntID) AND (CompanyID = @companyID)
