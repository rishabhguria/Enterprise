


/****** Object:  Stored Procedure dbo.P_SaveCompanyCVVenues    Script Date: 11/17/2005 9:50:23 AM ******/
--This will basically save CounterPartyVenue for a company
CREATE PROCEDURE dbo.P_SaveCompanyCVVenues
	(
		@companyID int,
		@counterPartyID int,
		@venueID int
	)
AS
	Declare @CounterPartyVenueID int
	Set @CounterPartyVenueID = 0
	
	-- This is done to look for the registered or existing venue for the equivalent counterparty.
	 Select @CounterPartyVenueID = CounterPartyVenueID
	FROM T_CounterPartyVenue
	Where CounterPartyID = @counterPartyID
		AND VenueID = @venueID 
	
	if(@CounterPartyVenueID > 0)
	Begin
		Insert T_CompanyCounterPartyVenues(companyID, CounterPartyVenueID)
		Values(@companyID, @CounterPartyVenueID)
	end
	


