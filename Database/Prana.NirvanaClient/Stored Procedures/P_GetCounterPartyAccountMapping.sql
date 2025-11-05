-- exec P_GetCounterPartyAccountMapping 7
CREATE PROCEDURE [dbo].[P_GetCounterPartyAccountMapping]
(
	@companyID int

)
AS
select distinct 
T_CounterPartyVenue.CounterPartyVenueID,
T_CompanyFunds.CompanyFundID as AccountID
from T_CompanyCounterParties
inner join T_CounterPartyVenue on T_CounterPartyVenue.CounterPartyID=T_CompanyCounterParties.CounterPartyID
inner join T_CounterParty on T_CounterParty.CounterPartyID=T_CompanyCounterParties.CounterPartyID
inner join T_CVACCOUNT		on T_CounterPartyVenue.CounterPartyVenueID=T_CVACCOUNT.CounterPartyVenueID
inner join T_CompanyFunds 			on T_CompanyFunds.CompanyFundID=T_CVACCOUNT.AccountID
where T_CompanyCounterParties.CompanyID= @companyID





