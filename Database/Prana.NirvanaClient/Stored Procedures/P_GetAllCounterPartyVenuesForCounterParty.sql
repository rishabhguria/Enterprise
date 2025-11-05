
CREATE PROCEDURE dbo.P_GetAllCounterPartyVenuesForCounterParty 
(
@CounterPartyID int
) 
 AS  
      
 SELECT     CounterPartyVenueID, DisplayName, IsElectronic,   
 OatsIdentifier, SymbolConventionID, CounterPartyID, VenueID, CurrencyID, -1 
 FROM         
 T_CounterPartyVenue  where CounterPartyID=@CounterPartyID
 Order By CounterPartyID, VenueID  

