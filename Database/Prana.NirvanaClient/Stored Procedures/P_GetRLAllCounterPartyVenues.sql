

CREATE PROCEDURE dbo.P_GetRLAllCounterPartyVenues
(
		@CompanyID int		
)
AS
	/* SET NOCOUNT ON */
	
	SELECT     T_CVAUEC.AUECID, T_CounterPartyVenue.CounterPartyVenueID, T_CounterParty.CounterPartyID, T_CounterParty.FullName AS CounterPartyName, T_Venue.VenueID, 
	                      T_Venue.VenueName
	FROM         T_CompanyCounterPartyVenues INNER JOIN
	                      T_CounterParty INNER JOIN
	                      T_CVAUEC INNER JOIN
	                      T_AUEC INNER JOIN
	                      T_CompanyAUEC ON T_AUEC.AUECID = T_CompanyAUEC.AUECID ON T_CVAUEC.AUECID = T_AUEC.AUECID INNER JOIN
	                      T_CounterPartyVenue ON T_CVAUEC.CounterPartyVenueID = T_CounterPartyVenue.CounterPartyVenueID ON 
	                      T_CounterParty.CounterPartyID = T_CounterPartyVenue.CounterPartyID ON 
	                      T_CompanyCounterPartyVenues.CounterPartyVenueID = T_CounterPartyVenue.CounterPartyVenueID INNER JOIN
	                      T_Venue ON T_CounterPartyVenue.VenueID = T_Venue.VenueID
	WHERE     (T_CompanyAUEC.CompanyID = @CompanyID) AND (T_CompanyCounterPartyVenues.CompanyID = @CompanyID)
	ORDER BY T_CVAUEC.AUECID, T_CounterParty.FullName, T_Venue.VenueName
	
	RETURN 


