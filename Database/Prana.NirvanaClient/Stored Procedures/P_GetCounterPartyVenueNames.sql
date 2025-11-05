
/****** Object:  Stored Procedure dbo.P_GetCounterPartyVenueNames    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetCounterPartyVenueNames
	(
		@counterPartyID int
	)
AS


SELECT     T_CounterPartyVenue.CounterPartyVenueID, T_Venue.VenueName, T_Venue.VenueTypeID, T_Venue.Route
FROM         T_Venue INNER JOIN
                      T_CounterPartyVenue ON T_Venue.VenueID = T_CounterPartyVenue.VenueID
WHERE     (T_CounterPartyVenue.CounterPartyID = @counterPartyID) 

