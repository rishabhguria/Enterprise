/****** Object:  Stored Procedure dbo.P_GetCompanyCounterPartyVenuesChanged  Script Date: 01/07/2006 2:35:22 PM ******/
CREATE PROCEDURE dbo.P_GetCompanyCounterPartyVenuesChanged
	(
		@companyID int,
		@companyCounterPartyID int		
	)
AS
	
	SELECT     CCPV.CompanyCounterPartyCVID, CCPV.CompanyID, CCPV.CounterPartyVenueID, CPV.DisplayName
				
FROM         
 T_CompanyCounterPartyVenues CCPV  INNER JOIN
                      T_CounterPartyVenue CPV ON 
                      CCPV.CounterPartyVenueID = CPV.CounterPartyVenueID 

Where CCPV.companyID = @companyID AND CCPV.CounterPartyID = @companyCounterPartyID
