
CREATE procedure P_GetOrderTypesByAUCVID
(
@AssetID int,
@UnderLyingID int,
@CounterPartyID int,
@VenueID int
)
as
select distinct T_OrderType.OrderTypesID,OrderTypes,OrderTypeTagValue
from 
T_CVAUECOrderTypes
join
T_CVAUEC on 
T_CVAUECOrderTypes.CVAUECID=T_CVAUEC.CVAUECID
join T_AUEC on
T_CVAUEC.AUECID=T_AUEC.AUECID
join T_CounterPartyVenue on
T_CVAUEC.CounterPartyVenueID=T_CounterPartyVenue.CounterPartyVenueID
join T_OrderType on
T_OrderType.OrderTypesID=T_CVAUECOrderTypes.OrderTypesID
where 
T_AUEC.AssetID=@AssetID
and T_AUEC.UnderlyingID=@UnderLyingID
and T_CounterPartyVenue.CounterPartyID=@CounterPartyID
and T_CounterPartyVenue.VenueID=@VenueID

