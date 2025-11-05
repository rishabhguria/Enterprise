


CREATE    Procedure 
P_GetCounterPartiesByAUIDAndUserID

(
@CompanyUserID int,
@AssetID int,
@UnderLyingID int
)
as

select distinct T_CounterParty.CounterPartyID,
 
T_CounterParty.FullName,ShortName 
from T_CompanyUserCounterPartyVenues
join T_CounterPartyVenue on 
T_CounterPartyVenue.CounterPartyVenueID=T_CompanyUserCounterPartyVenues.CounterPartyVenueID
join T_CVAUEC
on T_CounterPartyVenue.CounterPartyVenueID=T_CVAUEC.CounterPartyVenueID
join T_AUEC 
on T_AUEC.AUECID=T_CVAUEC.AUECID
join T_CounterParty on
T_CounterParty.CounterPartyID=T_CounterPartyVenue.CounterPartyID
where CompanyUserID=@CompanyUserID
and AssetID=@AssetID
and UnderLyingID=@UnderLyingID




