
CREATE     Procedure 
P_GetVenuesByAUIDCounterPartyAndUserID

(
@CompanyUserID int,
@CounterPartyID int,
@AssetID int,
@UnderLyingID int

)
as

select distinct T_Venue.VenueID,T_Venue.VenueName
 
--T_CounterParty.FullName,ShortName 
from T_CompanyUserCounterPartyVenues
join T_CounterPartyVenue on 
T_CounterPartyVenue.CounterPartyVenueID=T_CompanyUserCounterPartyVenues.CounterPartyVenueID
join T_CVAUEC
on T_CounterPartyVenue.CounterPartyVenueID=T_CVAUEC.CounterPartyVenueID
join T_AUEC 
on T_AUEC.AUECID=T_CVAUEC.AUECID
join T_Venue on
T_Venue.VenueID=T_CounterPartyVenue.VenueID
where CompanyUserID=@CompanyUserID
and AssetID=@AssetID
and UnderLyingID=@UnderLyingID
and T_CounterPartyVenue.CounterPartyID=@CounterPartyID

