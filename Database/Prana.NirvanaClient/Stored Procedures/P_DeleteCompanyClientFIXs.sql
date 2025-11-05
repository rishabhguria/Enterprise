


/****** Object:  Stored Procedure dbo.P_DeleteCompanyClientFIXs    Script Date: 01/24/2005 4:45:22 PM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyClientFIXs
	(
		@companyClientID int
	)
AS
	Delete T_CompanyClientFIX
	Where CompanyClientID = @companyClientID



