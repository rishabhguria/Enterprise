

CREATE PROCEDURE [P_GetVenuesByUserID]
(	
	@UserID int
)
AS
select distinct
	T_Venue.VenueID, 
	T_Venue.VenueName
from 
	T_CompanyUserCounterPartyVenues
	INNER join T_CounterPartyVenue on T_CounterPartyVenue.CounterPartyVenueID=T_CompanyUserCounterPartyVenues.CounterPartyVenueID
	INNER join T_Venue on T_Venue.VenueID=T_CounterPartyVenue.VenueID
where 
CompanyUserID = @UserID

