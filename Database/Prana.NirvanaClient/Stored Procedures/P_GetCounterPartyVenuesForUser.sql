
CREATE PROCEDURE dbo.P_GetCounterPartyVenuesForUser
	(
		@companyUserID int
	)
AS
	SELECT     CPV.CounterPartyVenueID, 
                      CPV.DisplayName, CPV.IsElectronic, CPV.OatsIdentifier, CPV.SymbolConventionID/*CPVD.AUECID,*/ , 
                      CPV.CounterPartyID, CPV.VenueID, CPV.CurrencyID, CCPV.CompanyUserCounterPartyCVID
FROM         T_CounterPartyVenue CPV INNER JOIN
                      T_CompanyUserCounterPartyVenues CCPV ON CPV.CounterPartyVenueID = CCPV.CounterPartyVenueID
                      AND CCPV.CompanyUserID = @companyUserID