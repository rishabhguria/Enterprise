CREATE PROCEDURE [dbo].[P_GetAllUsersVenues]
AS      
SELECT DISTINCT cpv.VenueID, v.VenueName      
FROM         T_CompanyUserCounterPartyVenues ucpv    
INNER JOIN  T_CounterPartyVenue cpv ON cpv.CounterPartyVenueID =  ucpv.CounterPartyVenueID     
INNER JOIN  T_Venue v ON cpv.VenueID = v.VenueID ;     
   
RETURN 0



