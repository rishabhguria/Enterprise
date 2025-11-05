
/****** Object:  Stored Procedure dbo.P_P_GetCompanyThirdPartyCVIdentifiers    Script Date: 03/22/2006 8:45:22 PM ******/
/*
 Modified by :<Kanupriya>
 Date : <10/06/2006>
 Description :<Instead of fetching the data against a companyID, now the data is fetched against a ThirdpartyID>
*/

CREATE PROCEDURE [dbo].[P_GetCompanyThirdPartyCVIdentifiers]
	(
		@companyThirdPartyID int	
	)
AS
	
	SELECT     T_CompanyThirdPartyCVIdentifier.ThirdPartyCVID, T_CompanyThirdPartyCVIdentifier.CompanyThirdPartyID_FK, 
	                      T_CompanyThirdPartyCVIdentifier.CompanyCounterPartyVenueID_FK, T_CompanyThirdPartyCVIdentifier.CVIdentifier, 
	                      T_CounterPartyVenue.DisplayName
	FROM         T_CounterPartyVenue INNER JOIN
	                      T_CompanyCounterPartyVenues ON T_CounterPartyVenue.CounterPartyVenueID = T_CompanyCounterPartyVenues.CounterPartyVenueID INNER JOIN
	                      T_CompanyThirdPartyCVIdentifier ON 
	                      T_CompanyCounterPartyVenues.CompanyCounterPartyCVID = T_CompanyThirdPartyCVIdentifier.CompanyCounterPartyVenueID_FK
	WHERE     (T_CompanyThirdPartyCVIdentifier.CompanyThirdPartyID_FK = @companyThirdPartyID)
