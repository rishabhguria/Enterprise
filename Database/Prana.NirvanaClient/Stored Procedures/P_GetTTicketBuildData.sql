
CREATE    PROCEDURE [dbo].[P_GetTTicketBuildData]
(
	@companyID int	,
	@UserID int
)
AS
select distinct 
T_AUEC.AUECID,
T_Asset.AssetID , --T_Asset.AssetName,
T_Underlying.UnderlyingID,--T_Underlying.UnderlyingName,
T_CounterParty.CounterPartyID,--T_CounterParty.FullName, ShortName ,
T_Venue.VenueID,-- T_Venue.VenueName
T_Underlying.UnderlyingName
from T_CompanyUserCounterPartyVenues
join T_CounterPartyVenue on T_CounterPartyVenue.CounterPartyVenueID=T_CompanyUserCounterPartyVenues.CounterPartyVenueID
join T_CVAUEC 		on T_CounterPartyVenue.CounterPartyVenueID=T_CVAUEC.CounterPartyVenueID
join T_AUEC 			on T_AUEC.AUECID=T_CVAUEC.AUECID
join T_CounterParty 		on T_CounterParty.CounterPartyID=T_CounterPartyVenue.CounterPartyID
join T_Venue 			on T_Venue.VenueID=T_CounterPartyVenue.VenueID
join T_Asset 			on T_Asset.AssetID=T_AUEC.AssetID
join T_Underlying 		on T_Underlying.UnderlyingID=T_AUEC.UnderlyingID

where CompanyUserID= @UserID AND T_CounterParty.IsOTDorEMS = 0;

	/*
Select 
--CA.CompanyID,
 CA.AUECID, 
AUEC.AssetID, AUEC.UnderLyingID ,
--CV.CVAUECID,
CPV.CounterPartyID, CPV.VenueID,
-- cpV.cOUNTERPARTYVENUEID,
T_Underlying.UnderLyingName

from 
T_CompanyUserAUEC as CUA
join 
T_CompanyAUEC as CA  on CUA.CompanyAUECID = CA.CompanyAUECID
join T_AUEC as AUEC on AUEC.AUECID = CA.AUECID

Left JOIN T_CVAUEC as CV on CV.AUECID = CA.AUECID

join T_CounterPartyVenue as CPV on CV.CounterPartyVenueID =  CPV.CounterPartyVenueID 
join T_CompanyCounterPartyVenues ccv on CV.CounterPartyVenueID = ccv.CounterPartyVenueID and ccv.CompanyID = 1

join T_UnderLying on T_Underlying.UnderLyingid = AUEC.UnderLyingID


where CUA.CompanyUserID = 3 
order by AUEC.AssetID, AUEC.UnderLyingID, CPV.CounterPartyID, cpv.VenueID
*/
RETURN 





