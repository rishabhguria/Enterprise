
Create   PROCEDURE [dbo].[P_GetCounterPartyAccountMappingData]
(
	@companyID int
)
AS

select distinct 
T_CounterParty.CounterPartyID,--T_CounterParty.FullName, ShortName ,
T_CounterParty.FullName as CounterPartyName,T_Venue.VenueID, T_Venue.VenueName,
T_CounterPartyVenue.CounterPartyVenueID,T_CounterPartyVenue.DisplayName,
T_CompanyFunds.CompanyFundID 

from T_CompanyCounterParties
join T_CounterPartyVenue on T_CounterPartyVenue.CounterPartyID=T_CompanyCounterParties.CounterPartyID
join T_CompanyCounterPartyVenues CCPV ON T_CounterPartyVenue.CounterPartyVenueID = CCPV.CounterPartyVenueID 
join T_CVACCOUNT		on T_CounterPartyVenue.CounterPartyVenueID=T_CVACCOUNT.CounterPartyVenueID
join T_CounterParty 		on T_CounterParty.CounterPartyID=T_CounterPartyVenue.CounterPartyID
join T_Venue 			on T_Venue.VenueID=T_CounterPartyVenue.VenueID
join T_CompanyFunds		on T_CompanyFunds.CompanyFundID=T_CVACCOUNT.AccountID
where T_CompanyCounterParties.CompanyID = @companyID
	
RETURN 





