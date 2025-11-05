


/****** Object:  Stored Procedure dbo.P_DeleteCompanyVenueDetails    Script Date: 04/01/2005 9:45:22 PM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyVenueDetails
	(
		@companyID int
	)
AS
	Delete T_CompanyVenue
	Where CompanyID = @companyID



