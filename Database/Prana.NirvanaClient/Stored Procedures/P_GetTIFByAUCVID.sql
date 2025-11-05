
create procedure P_GetTIFByAUCVID
(
@AssetID int,
@UnderLyingID int,
@CounterPartyID int,
@VenueID int
)
as
select distinct T_TimeInForce.TimeInForceID,TimeInForce,TimeInForceTagValue
from 
T_CVAUECTimeInForce
join
T_CVAUEC on 
T_CVAUECTimeInForce.CVAUECID=T_CVAUEC.CVAUECID
join T_AUEC on
T_CVAUEC.AUECID=T_AUEC.AUECID
join T_CounterPartyVenue on
T_CVAUEC.CounterPartyVenueID=T_CounterPartyVenue.CounterPartyVenueID
join T_TimeInForce on
T_TimeInForce.TimeInForceID=T_CVAUECTimeInForce.TimeInForceID
where 
T_AUEC.AssetID=@AssetID
and T_AUEC.UnderlyingID=@UnderLyingID
and T_CounterPartyVenue.CounterPartyID=@CounterPartyID
and T_CounterPartyVenue.VenueID=@VenueID

