-- =============================================
-- Author:		<Ashish Poddar>
-- Create date: <3rd Nov, 2006>
-- Description:	<To get CV and AUEC permissions for all users>
-- =============================================
CREATE PROCEDURE P_GetCVandAUECPermissionsForUser
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

select distinct 
CUCPV.CompanyUserID,
T_AUEC.AUECID,
T_CounterParty.CounterPartyID,--T_CounterParty.FullName, ShortName ,
T_Venue.VenueID-- T_Venue.VenueName

from T_CompanyUserCounterPartyVenues as CUCPV
join T_CounterPartyVenue on T_CounterPartyVenue.CounterPartyVenueID= CUCPV.CounterPartyVenueID
join T_CVAUEC 		on T_CounterPartyVenue.CounterPartyVenueID=T_CVAUEC.CounterPartyVenueID
join T_AUEC 			on T_AUEC.AUECID=T_CVAUEC.AUECID
join T_CounterParty 		on T_CounterParty.CounterPartyID=T_CounterPartyVenue.CounterPartyID
join T_Venue 			on T_Venue.VenueID=T_CounterPartyVenue.VenueID

END
