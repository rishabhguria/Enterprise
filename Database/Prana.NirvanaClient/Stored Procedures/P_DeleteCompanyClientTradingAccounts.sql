

CREATE PROCEDURE dbo.P_DeleteCompanyClientTradingAccounts

	(
		@CompanyClientID int
		
	)

AS
delete from T_CompanyClientTradingAccount where  CompanyClientID=@CompanyClientID


