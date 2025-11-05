/****** Object:  Stored Procedure dbo.P_GetCompanyCounterPartyVenueDetails    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE [dbo].[P_GetCompanyCounterPartyVenueDetails]
(
	@companyCounterPartyVenueID int
)
AS
	SELECT   CompanyCounterPartyVenueDetailsID, CCPVD.CompanyCounterPartyVenueID, CompanyClearingFirmsPrimeBrokersID, 
				CompanyMPID, DeliverToCompanyID, SenderCompanyID, TargetCompID, ClearingFirm, CompanyFundID, 
				CompanyStrategyID, OnBehalfOfSubID, 0 as CompanyCounterPartyVenueIdentifierID, 
				'' as CMTAIdentifier, '' as GiveUpIdentifier
FROM         T_CompanyCounterPartyVenueDetails CCPVD--, T_CompanyCounterPartyVenueIdentifier CCPVI
Where CCPVD.CompanyCounterPartyVenueID = @companyCounterPartyVenueID --AND
		--CCPVI.CompanyCounterPartyVenueID = @companyCounterPartyVenueID





