/****** Object:  Stored Procedure dbo.P_GetCVFIXForCompanyCPVID    Script Date: 12/29/2005 12:25:22 PM ******/
CREATE PROCEDURE dbo.P_GetCVFIXForCompanyCPVID
	(
		@companyCounterPartyVenueID int	
	)
AS
	
	Select CVFIXID, CVF.CounterPartyVenueID, Acronymn, FixVersionID, TargetCompID,
						DeliverToCompID, DeliverToSubID 
	From T_CompanyCounterPartyVenues CCPV inner join T_CounterPartyVenue CPV 
		on CCPV.CounterPartyVenueID = CPV.CounterPartyVenueID inner join T_CVFIX CVF
		on CPV.CounterPartyVenueID = CVF.CounterPartyVenueID
	Where CCPV.CompanyCounterPartyCVID = @companyCounterPartyVenueID