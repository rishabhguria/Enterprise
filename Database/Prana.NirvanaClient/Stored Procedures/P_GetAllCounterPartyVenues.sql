
/****** Object:  Stored Procedure dbo.P_GetAllCounterPartyVenues    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAllCounterPartyVenues
	AS
	
	/*
	SELECT     CPVD.CounterPartyVenueDetailsID, CPVD.CounterPartyVenueID, CPVD.DisplayName, CPVD.IsElectronic, 
				CPVD.FixIdentifier, CPVD.AUECID, CPVD.SymbolConversionID, CPVD.SideID, CPVD.OrderTypesID, 
				CPVD.TimeInForceID, CPVD.HandlingInstructionsID, CPVD.ExecutionInstructionsID, 
				CPVD.AdvancedOrdersID, CPV.CounterPartyID, CPV.VenueID
FROM         T_CounterPartyVenueDetails CPVD INNER JOIN
                      T_CounterPartyVenue CPV ON CPVD.CounterPartyVenueID = CPV.CounterPartyVenueID INNER JOIN
                      T_CompanyCounterPartyVenues CCPV ON CPV.CounterPartyVenueID = CCPV.CounterPartyVenueID AND 
                      CPVD.CounterPartyVenueID = CCPV.CounterPartyVenueID
                      
    */
    
    SELECT     CounterPartyVenueID, DisplayName, IsElectronic, 
				OatsIdentifier, SymbolConventionID, CounterPartyID, VenueID, CurrencyID, -1 -- -1 for the CompanyCounterPartyCVID which is not required in this proceduere but used to match the fill parameters.
FROM         T_CounterPartyVenue
    Order By CounterPartyID, VenueID
                      
    /*SELECT     CPVD.CounterPartyVenueDetailsID, CPVD.CounterPartyVenueID, CPVD.DisplayName, CPVD.IsElectronic, CPVD.FixIdentifier, CPVD.AUECID, 
                      CPVD.SymbolConversionID, CPVD.SideID, CPVD.OrderTypesID, CPVD.TimeInForceID, CPVD.HandlingInstructionsID, CPVD.ExecutionInstructionsID, 
                      CPVD.AdvancedOrdersID, CPV.CounterPartyID, CPV.VenueID
FROM         T_CounterPartyVenue CPV RIGHT OUTER JOIN
             T_CounterPartyVenueDetails CPVD ON CPVD.CounterPartyVenueID = CPV.CounterPartyVenueID */
