CREATE PROCEDURE dbo.P_GetUserCounterParties (@userID INT)
AS
SELECT DISTINCT cpv.CounterPartyID
	,cp.ShortName
FROM T_CompanyUserCounterPartyVenues ucpv
INNER JOIN T_CounterPartyVenue cpv
	ON ucpv.CounterPartyVenueID = cpv.CounterPartyVenueID
INNER JOIN T_CounterParty cp
	ON cpv.CounterPartyID = cp.CounterPartyID
WHERE (ucpv.CompanyUserID = @userID AND cp.IsOTDorEMS = 0)
