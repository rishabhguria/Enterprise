
-----------------------------------------------------------------------------------

--Purpose:		Insert data into Table T_CVACCOUNT
-----------------------------------------------------------------------------------
IF (SELECT COUNT(*) FROM T_CVACCOUNT) = 0
BEGIN

select CounterPartyVenueID, CompanyID
into #T_CounterPartyVenue
from T_CounterPartyVenue
inner join T_CompanyCounterParties on  T_CounterPartyVenue.CounterPartyID =T_CompanyCounterParties.CounterPartyID
 where CompanyID >0


INSERT INTO T_CVACCOUNT(
		CounterPartyVenueID
		,AccountID
		)
SELECT 
CounterPartyVenueID,T_CompanyFunds.CompanyFundID
from T_CompanyFunds,#T_CounterPartyVenue
where T_CompanyFunds.CompanyID  = T_CompanyFunds.CompanyID

drop table #T_CounterPartyVenue
END

