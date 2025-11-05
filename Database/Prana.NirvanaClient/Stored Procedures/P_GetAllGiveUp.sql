CREATE PROCEDURE [dbo].[P_GetAllGiveUp]

AS
	SELECT     GiveUpIdentifier, CompanyCVenueGiveUpIdentifierID
FROM         T_CompanyCVGiveUpIdentifier Order By GiveUpIdentifier
