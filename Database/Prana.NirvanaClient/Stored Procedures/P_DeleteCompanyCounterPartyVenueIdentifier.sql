

/****** Object:  Stored Procedure dbo.P_DeleteCompanyCounterPartyVenueIdentifier    Script Date: 01/09/2006 7:30:22 PM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyCounterPartyVenueIdentifier
	(
		@companyCounterPartyVenueID int
	)
AS
	Delete T_CompanyCounterPartyVenueIdentifier
	Where CompanyCounterPartyVenueID = @companyCounterPartyVenueID



