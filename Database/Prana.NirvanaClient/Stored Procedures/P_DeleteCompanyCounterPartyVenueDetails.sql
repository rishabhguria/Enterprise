

/****** Object:  Stored Procedure dbo.P_DeleteCompanyCounterPartyVenueDetails    Script Date: 01/09/2006 7:35:22 PM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyCounterPartyVenueDetails
	(
		@companyCounterPartyVenueID int
	)
AS
	Delete T_CompanyCounterPartyVenueDetails
	Where CompanyCounterPartyVenueID = @companyCounterPartyVenueID



