

-- =============================================
-- Author:		<Harsh Kumar>
-- Create date: <03/20/2007>
-- Description:	<Used for TT, after selecting CP and Venue we need CRVenueID for CMTA & GiveUp>
-- =============================================
Create PROCEDURE [dbo].[P_GetCompanyCPVenueIDbyCPIDVenueID]
(
@counterPartyID int,
@venueID int,
@companyID int
)
AS
Select T_CompanyCounterPartyVenues.CompanyCounterPartyCVID
 
from T_CounterPartyVenue join T_CompanyCounterPartyVenues on T_CounterPartyVenue.CounterPartyVenueID = T_CompanyCounterPartyVenues.CounterPartyVenueID

where 
T_CounterPartyVenue.CounterPartyID = @counterPartyID
and
T_CounterPartyVenue.VenueID = @venueID 
and 
T_CompanyCounterPartyVenues.CompanyID = @companyID

select * from T_CompanyCounterPartyVenues