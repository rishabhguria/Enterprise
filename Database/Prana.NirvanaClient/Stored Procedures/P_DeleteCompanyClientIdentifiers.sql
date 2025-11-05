


/****** Object:  Stored Procedure dbo.P_DeleteCompanyClientIdentifiers    Script Date: 01/24/2006 9:10:24 PM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyClientIdentifiers
(
	@companyClientID int	
)
AS

Delete T_CompanyClientIdentifier
Where CompanyClientID = @companyClientID
	



