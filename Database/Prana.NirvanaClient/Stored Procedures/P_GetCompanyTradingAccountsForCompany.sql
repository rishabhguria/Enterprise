


/****** Object:  Stored Procedure dbo.P_GetCompanyTradingAccountsForCompany    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_GetCompanyTradingAccountsForCompany
	(
		@companyID int
	)
AS
	SELECT     CompanyTradingAccountsID, TradingAccountName, TradingShortName, CompanyID
	FROM         T_CompanyTradingAccounts
	Where CompanyID = @companyID



