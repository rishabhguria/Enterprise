


/****** Object:  Stored Procedure dbo.P_GetCounterPartyVenuesForCounterParties    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetCounterPartyVenuesForCounterParties
	(
		@counterPartyID int
	)
AS
	SELECT     T_Venue.VenueID, T_Venue.VenueName, T_Venue.VenueTypeID, T_Venue.Route
FROM         T_CounterPartyVenue INNER JOIN
                      T_Venue ON T_CounterPartyVenue.VenueID = T_Venue.VenueID AND 
                      T_CounterPartyVenue.CounterPartyID = @counterPartyID


