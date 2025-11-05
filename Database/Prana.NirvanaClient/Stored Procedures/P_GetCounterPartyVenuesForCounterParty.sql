
/****** Object:  Stored Procedure dbo.P_GetCounterPartyVenuesForCounterParty    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetCounterPartyVenuesForCounterParty
	(
		@counterPartyID int
	)
AS
	/*SELECT     T_Venue.VenueID, T_Venue.VenueName, T_Venue.VenueTypeID, T_Venue.Route
FROM         T_CounterPartyVenue INNER JOIN
                      T_Venue ON T_CounterPartyVenue.VenueID = T_Venue.VenueID AND 
                      T_CounterPartyVenue.CounterPartyID = @counterPartyID */
                      
 
 /* SELECT     T_CounterPartyVenue.CounterPartyVenueID, T_Venue.VenueName, T_Venue.VenueTypeID, T_Venue.Route
FROM         T_CompanyCounterPartyVenues INNER JOIN
                      T_CounterPartyVenue ON T_CompanyCounterPartyVenues.CounterPartyVenueID = T_CounterPartyVenue.CounterPartyVenueID INNER JOIN
                      T_Venue ON T_CounterPartyVenue.VenueID = T_Venue.VenueID AND 
                      T_CounterPartyVenue.CounterPartyID = @counterPartyID */
                      


-- Selects all venues pertaining to the selected counterparty                       
 /*SELECT     T_CounterPartyVenue.CounterPartyVenueID, T_Venue.VenueName, T_Venue.VenueTypeID, T_Venue.Route
FROM         T_CounterParty INNER JOIN
                      T_CounterPartyVenue ON T_CounterParty.CounterPartyID = T_CounterPartyVenue.CounterPartyID INNER JOIN
                      T_Venue ON T_CounterPartyVenue.VenueID = T_Venue.VenueID AND T_CounterParty.CounterPartyID = @counterPartyID  */
                      

-- Selects all venues pertaining to the selected counterparty                     
/*SELECT     T_CounterPartyVenue.CounterPartyVenueID, T_Venue.VenueName, T_Venue.VenueTypeID, T_Venue.Route
FROM         T_CompanyCounterPartyVenues INNER JOIN
                      T_CounterPartyVenue ON T_CompanyCounterPartyVenues.CounterPartyVenueID = T_CounterPartyVenue.CounterPartyVenueID INNER JOIN
                      T_Venue ON T_CounterPartyVenue.VenueID = T_Venue.VenueID AND T_CounterPartyVenue.CounterPartyID = @counterPartyID
                      
                      */
/*SELECT     T_CounterPartyVenue.CounterPartyVenueID, T_CounterPartyVenueDetails.DisplayName, T_Venue.VenueTypeID, T_Venue.Route
FROM         T_Venue INNER JOIN
                      T_CounterPartyVenue ON T_Venue.VenueID = T_CounterPartyVenue.VenueID INNER JOIN
                      T_CounterPartyVenueDetails ON T_CounterPartyVenue.CounterPartyVenueID = T_CounterPartyVenueDetails.CounterPartyVenueID
WHERE     (T_CounterPartyVenue.CounterPartyID = @counterPartyID)*/

SELECT     T_Venue.VenueID, DisplayName, T_Venue.VenueTypeID, T_Venue.Route, T_Venue.ExchangeID
FROM         T_Venue INNER JOIN
                      T_CounterPartyVenue ON T_Venue.VenueID = T_CounterPartyVenue.VenueID
WHERE     (T_CounterPartyVenue.CounterPartyID = @counterPartyID)
