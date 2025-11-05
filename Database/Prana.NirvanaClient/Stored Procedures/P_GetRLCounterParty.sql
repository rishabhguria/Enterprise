

CREATE PROCEDURE dbo.P_GetRLCounterParty
(
		@CompanyID int,
		@AUECID int
)
AS
SELECT DISTINCT T_CounterParty.CounterPartyID, T_CounterParty.FullName
FROM         T_CounterPartyVenue INNER JOIN
                      T_CompanyCounterPartyVenues ON T_CounterPartyVenue.CounterPartyVenueID = T_CompanyCounterPartyVenues.CounterPartyVenueID INNER JOIN
                      T_CounterParty ON T_CounterPartyVenue.CounterPartyID = T_CounterParty.CounterPartyID INNER JOIN
                      T_CVAUEC ON T_CounterPartyVenue.CounterPartyVenueID = T_CVAUEC.CounterPartyVenueID
WHERE     (T_CompanyCounterPartyVenues.CompanyID = @CompanyID) AND (T_CVAUEC.AUECID = @AUECID)
ORDER BY T_CounterParty.FullName


