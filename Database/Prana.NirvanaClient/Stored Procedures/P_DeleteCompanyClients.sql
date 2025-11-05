


/****** Object:  Stored Procedure dbo.P_DeleteCompanyClients    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyClients
	(
		@companyID int
	)
AS
	------------------------ Start : Delete dependent data ---------------------------------
	Delete T_CompanyClientFund Where CompanyClientID IN (Select CompanyClientID From T_CompanyClient Where
							CompanyID = @companyID)
							
	Delete T_CompanyClientAUEC Where CompanyClientID IN (Select CompanyClientID From T_CompanyClient Where
							CompanyID = @companyID)
							
	Delete T_CompanyClientClearer Where CompanyClientID IN (Select CompanyClientID From T_CompanyClient Where
							CompanyID = @companyID)
							
	Delete T_CompanyClientFIX Where CompanyClientID IN (Select CompanyClientID From T_CompanyClient Where
							CompanyID = @companyID)
							
	Delete T_CompanyClientTrader Where CompanyClientID IN (Select CompanyClientID From T_CompanyClient Where
							CompanyID = @companyID)
							
	Delete T_CompanyClientTradingAccount Where CompanyClientID IN (Select CompanyClientID From T_CompanyClient Where
							CompanyID = @companyID)
	
	------------------------ End   : Delete dependent data ----------------------------------
	
	Delete T_CompanyClient
	Where CompanyID = @companyID


