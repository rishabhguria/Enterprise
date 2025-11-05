

/****** Object:  Stored Procedure dbo.P_DeleteCompanyCVDetails    Script Date: 01/24/2006 1:20:22 PM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyCVDetails
	(
		@companyID int
	)
AS
	Delete T_CompanyCounterPartyVenueDetails
		Where (CompanyCounterPartyVenueID in (SELECT CompanyCounterPartyVenueID
			FROM T_CompanyCounterPartyVenues WHERE 
				CompanyID = @companyID))

	

