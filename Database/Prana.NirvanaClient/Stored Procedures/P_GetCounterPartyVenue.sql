
/****** Object:  Stored Procedure dbo.P_GetCounterPartyVenue    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetCounterPartyVenue
	(
		@counterPartyVenueID int
	)
AS
	SELECT     CounterPartyVenueID, DisplayName, IsElectronic, OatsIdentifier, SymbolConventionID, CounterPartyID, 
				VenueID, CurrencyID, -1 -- -1 for the CompanyCounterPartyCVID which is not required in this proceduere but used to match the fill parameters.
	FROM         T_CounterPartyVenue Where CounterPartyVenueID = @counterPartyVenueID
