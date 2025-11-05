

CREATE PROCEDURE dbo.P_GetRLVenue
(
		@CounterPartyID int
)
AS
SELECT DISTINCT T_Venue.VenueID, T_Venue.VenueName
FROM         T_CounterPartyVenue INNER JOIN
                      T_Venue ON T_CounterPartyVenue.VenueID = T_Venue.VenueID
WHERE     (T_CounterPartyVenue.CounterPartyID = @CounterPartyID)
ORDER BY T_Venue.VenueName


