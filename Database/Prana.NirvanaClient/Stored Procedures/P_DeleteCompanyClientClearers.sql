


/****** Object:  Stored Procedure dbo.P_DeleteCompanyClientClearers    Script Date: 01/24/2006 4:35:24 PM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyClientClearers
(
	@companyClientID int
)
AS
	
	Delete T_CompanyClientClearer
	Where CompanyClientID = @companyClientID


