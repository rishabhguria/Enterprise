
create procedure P_GetExecutionInstructionByAUCVID
(
@AssetID int,
@UnderLyingID int,
@CounterPartyID int,
@VenueID int
)
as
select distinct T_ExecutionInstructions.ExecutionInstructionsID,ExecutionInstructions,
ExecutionInstructionsTagValue
from 
T_CVAUECExecutionInstructions
join
T_CVAUEC on 
T_CVAUECExecutionInstructions.CVAUECID=T_CVAUEC.CVAUECID
join T_AUEC on
T_CVAUEC.AUECID=T_AUEC.AUECID
join T_CounterPartyVenue on
T_CVAUEC.CounterPartyVenueID=T_CounterPartyVenue.CounterPartyVenueID
join T_ExecutionInstructions on
T_ExecutionInstructions.ExecutionInstructionsID=T_CVAUECExecutionInstructions.ExecutionInstructionsID
where 
T_AUEC.AssetID=@AssetID
and T_AUEC.UnderlyingID=@UnderLyingID
and T_CounterPartyVenue.CounterPartyID=@CounterPartyID
and T_CounterPartyVenue.VenueID=@VenueID

