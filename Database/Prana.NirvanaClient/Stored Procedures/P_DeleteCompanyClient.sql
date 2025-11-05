

CREATE PROCEDURE dbo.P_DeleteCompanyClient
	(
		@companyID int,
		@companyClientID int
	)
AS
	------------------------ Start : Delete dependent data ---------------------------------
	exec P_DeleteCompanyClientFund @companyClientID
	
	exec P_DeleteCompanyClientTrader @companyClientID
	------------------------ End   : Delete dependent data ----------------------------------
	
	Delete T_CompanyClient
	Where CompanyID = @companyID

