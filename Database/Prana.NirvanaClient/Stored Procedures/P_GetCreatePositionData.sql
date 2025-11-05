
CREATE PROCEDURE [dbo].[P_GetCreatePositionData]
(
	@companyID int	,
	@UserID int
)
AS
select distinct 
T_AUEC.AUECID,
T_Asset.AssetID , T_Asset.AssetName,
T_Underlying.UnderlyingID,T_Underlying.UnderlyingName,
T_CounterParty.CounterPartyID,T_CounterParty.FullName, ShortName ,
T_Venue.VenueID, T_Venue.VenueName,
T_Underlying.UnderlyingName
from T_CompanyUserCounterPartyVenues
join T_CounterPartyVenue on T_CounterPartyVenue.CounterPartyVenueID=T_CompanyUserCounterPartyVenues.CounterPartyVenueID
join T_CVAUEC 		on T_CounterPartyVenue.CounterPartyVenueID=T_CVAUEC.CounterPartyVenueID
join T_AUEC 			on T_AUEC.AUECID=T_CVAUEC.AUECID
join T_CounterParty 		on T_CounterParty.CounterPartyID=T_CounterPartyVenue.CounterPartyID
join T_Venue 			on T_Venue.VenueID=T_CounterPartyVenue.VenueID
join T_Asset 			on T_Asset.AssetID=T_AUEC.AssetID
join T_Underlying 		on T_Underlying.UnderlyingID=T_AUEC.UnderlyingID

where CompanyUserID= @UserID
