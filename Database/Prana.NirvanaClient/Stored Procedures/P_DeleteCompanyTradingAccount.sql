


/****** Object:  Stored Procedure dbo.P_DeleteCompanyTradingAccount    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyTradingAccount
	(
		@companyTradingAccountsID int	
	)
AS
Delete T_CompanyTradingAccounts
Where CompanyTradingAccountsID = @companyTradingAccountsID


