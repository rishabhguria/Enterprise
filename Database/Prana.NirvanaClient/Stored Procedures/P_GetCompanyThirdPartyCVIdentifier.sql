
/*
Name : <P_GetCompanyThirdPartyCVIdentifier>
Created By : <Kanupriya>
Dated : <11/03/2006>
Purpose :<To fetch the identifier for a companyCounterPartyVenueID.>
*/
CREATE PROCEDURE dbo.P_GetCompanyThirdPartyCVIdentifier
	
	(
	@companyThirdPartyID int,
	@cVID int
	)
	
AS
	SELECT     T_CompanyThirdPartyCVIdentifier.ThirdPartyCVID, T_CompanyThirdPartyCVIdentifier.CompanyThirdPartyID_FK, 
	                      T_CompanyThirdPartyCVIdentifier.CompanyCounterPartyVenueID_FK, T_CompanyThirdPartyCVIdentifier.CVIdentifier, 
	                      T_CounterPartyVenue.DisplayName
	FROM         T_CounterPartyVenue INNER JOIN
	                      T_CompanyCounterPartyVenues ON T_CounterPartyVenue.CounterPartyVenueID = T_CompanyCounterPartyVenues.CounterPartyVenueID INNER JOIN
	                      T_CompanyThirdPartyCVIdentifier ON 
	                      T_CompanyCounterPartyVenues.CompanyCounterPartyCVID = T_CompanyThirdPartyCVIdentifier.CompanyCounterPartyVenueID_FK
	WHERE     (T_CompanyThirdPartyCVIdentifier.CompanyThirdPartyID_FK = @companyThirdPartyID)and  T_CompanyThirdPartyCVIdentifier.CompanyCounterPartyVenueID_FK = @cVID
