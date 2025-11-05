

CREATE  procedure P_GetHandlingInstructionByAUCVID
(
@AssetID int,
@UnderLyingID int,
@CounterPartyID int,
@VenueID int
)
as

select distinct T_HandlingInstructions.HandlingInstructionsID,HandlingInstructions,
HandlingInstructionsTagValue
from 
T_CVAUECHandlingInstructions
join
T_CVAUEC on 
T_CVAUECHandlingInstructions.CVAUECID=T_CVAUEC.CVAUECID
join T_AUEC on
T_CVAUEC.AUECID=T_AUEC.AUECID
join T_CounterPartyVenue on
T_CVAUEC.CounterPartyVenueID=T_CounterPartyVenue.CounterPartyVenueID
join T_HandlingInstructions on
T_HandlingInstructions.HandlingInstructionsID=T_CVAUECHandlingInstructions.HandlingInstructionsID
where 
T_AUEC.AssetID=@AssetID
and T_AUEC.UnderlyingID=@UnderLyingID
and T_CounterPartyVenue.CounterPartyID=@CounterPartyID
and T_CounterPartyVenue.VenueID=@VenueID


