



/****** Object:  Stored Procedure dbo.P_GetCompanyCounterPartyVenueIdentifier    Script Date: 09/01/2006 7:25:21 PM ******/
CREATE PROCEDURE [dbo].[P_GetCompanyCounterPartyVenueIdentifier]
(
	@companyCounterPartyVenueID int
)
AS
	SELECT   CompanyCounterPartyVenueIdentifierID, CompanyCounterPartyVenueID, CMTAIdentifier, GiveUpIdentifier
FROM         T_CompanyCounterPartyVenueIdentifier
Where CompanyCounterPartyVenueID = @companyCounterPartyVenueID




