/*
Name :<P_GetCounterPartyVenueDetail>
CreatedBy : <Kanupriya>
Dated :<11/03/2006>
purpose:<to fetch the counterpartyvenue id and the companycvid.>
*/
CREATE PROCEDURE dbo.P_GetCounterPartyVenueDetail
	
	(
	@counterPartyID int ,
	@venueID int,
	@companyID int
	)
	
AS
	SELECT     T_CounterPartyVenue.CounterPartyVenueID, --T_CounterPartyVenue.CounterPartyID, 
	           T_CompanyCounterPartyVenues.CompanyCounterPartyCVID
	FROM         T_CounterPartyVenue INNER JOIN
	                      T_CompanyCounterPartyVenues ON T_CounterPartyVenue.CounterPartyVenueID = T_CompanyCounterPartyVenues.CounterPartyVenueID
	WHERE     (T_CounterPartyVenue.CounterPartyID = @counterPartyID) AND (T_CounterPartyVenue.VenueID = @venueID) AND 
	                      (T_CompanyCounterPartyVenues.CompanyID = @companyID)
