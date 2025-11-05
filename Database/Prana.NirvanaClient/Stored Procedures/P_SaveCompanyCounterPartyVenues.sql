


/****** Object:  Stored Procedure dbo.P_SaveCompanyCounterPartyVenues    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_SaveCompanyCounterPartyVenues
	(
		@companyID int,
		@counterPartyVenueID int
	)
AS

	Insert T_CompanyCounterPartyVenues(companyID, CounterPartyVenueID)
	Values(@companyID, @counterPartyVenueID)
	
	


