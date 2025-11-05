CREATE PROCEDURE [dbo].[P_GetCompanyPermittedVenues] (
	@companyID INT
	,@counterPartyID INT
	)
AS
SELECT DISTINCT CPV.VenueID
	,V.VenueName
FROM T_CompanyCounterPartyVenues CCPV
INNER JOIN T_CounterPartyVenue CPV
	ON CPV.CounterPartyVenueID = CCPV.CounterPartyVenueID
INNER JOIN T_Venue V
	ON CPV.VenueID = V.VenueID
WHERE (CCPV.CompanyID = @companyID)
	AND CPV.CounterPartyID = @counterPartyID
