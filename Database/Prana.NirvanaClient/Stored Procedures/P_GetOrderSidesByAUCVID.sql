CREATE procedure P_GetOrderSidesByAUCVID
(
@AssetID int,
@UnderLyingID int,
@CounterPartyID int,
@VenueID int
)
as
select distinct 
T_Side.SideID, 
 Side,
SideTagValue
from T_CVAUECSide

join T_CVAUEC on T_CVAUECSide.CVAUECID=T_CVAUEC.CVAUECID
join T_AUEC on T_CVAUEC.AUECID=T_AUEC.AUECID
join T_CounterPartyVenue on T_CVAUEC.CounterPartyVenueID=T_CounterPartyVenue.CounterPartyVenueID
join T_Side on T_Side.SideID=T_CVAUECSide.SideID
where 
T_AUEC.AssetID=@AssetID
and T_AUEC.UnderlyingID=@UnderLyingID
and T_CounterPartyVenue.CounterPartyID=@CounterPartyID
and T_CounterPartyVenue.VenueID=@VenueID
order by Side