/****** Object:  Stored Procedure dbo.P_GetCompanyCounterPartyVenue  Script Date: 01/10/2006 4:20:22 PM ******/
CREATE PROCEDURE dbo.P_GetCompanyCounterPartyVenue
	(
		@companyCounterPartyVenueID int		
	)
AS
	
	SELECT     CCPV.CompanyCounterPartyCVID, CCPV.CompanyID, CCPV.CounterPartyVenueID, CPV.DisplayName
				
FROM         
 T_CompanyCounterPartyVenues CCPV  INNER JOIN
                      T_CounterPartyVenue CPV ON 
                      CCPV.CounterPartyVenueID = CPV.CounterPartyVenueID 

Where CompanyCounterPartyCVID = @companyCounterPartyVenueID