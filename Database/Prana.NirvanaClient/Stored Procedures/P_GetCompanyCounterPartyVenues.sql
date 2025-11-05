

CREATE PROCEDURE dbo.P_GetCompanyCounterPartyVenues
	(
		@companyID int		
	)
AS
	
	SELECT     CPV.CounterPartyVenueID, CPV.DisplayName, CPV.IsElectronic, 
				CPV.OatsIdentifier, CPV.SymbolConventionID, CPV.CounterPartyID, CPV.VenueID, CPV.CurrencyID, 
				CCPV.CompanyCounterPartyCVID
FROM         
 T_CounterPartyVenue CPV INNER JOIN
                      T_CompanyCounterPartyVenues CCPV ON 
                      CPV.CounterPartyVenueID = CCPV.CounterPartyVenueID
/*T_CounterPartyVenueDetails CPVD INNER JOIN
                      T_CounterPartyVenue CPV ON CPVD.CounterPartyVenueID = CPV.CounterPartyVenueID INNER JOIN
                      T_CompanyCounterPartyVenues CCPV ON CPV.CounterPartyVenueID = CCPV.CounterPartyVenueID AND 
                      CPVD.CounterPartyVenueID = CCPV.CounterPartyVenueID*/
Where CCPV.companyID = @companyID
Order By CPV.CounterPartyID, CPV.VenueID







	/*(
		@counterPartyID int
	)
AS
	                  
                      
                      SELECT     T_Venue.VenueID, T_Venue.VenueName, T_Venue.VenueTypeID, T_Venue.Route
FROM         T_CompanyCounterPartyVenues INNER JOIN
                      T_CounterPartyVenue ON T_CompanyCounterPartyVenues.CounterPartyVenueID = T_CounterPartyVenue.CounterPartyVenueID INNER JOIN
                      T_Venue ON T_CounterPartyVenue.VenueID = T_Venue.VenueID AND 
                      T_CounterPartyVenue.CounterPartyID = @counterPartyID  */