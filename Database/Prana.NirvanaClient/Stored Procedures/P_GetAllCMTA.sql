


CREATE PROCEDURE [dbo].[P_GetAllCMTA]

AS
	SELECT     CMTAIdentifier, CompanyCVenueCMTAIdentifierID
FROM         T_CompanyCVCMTAIdentifier Order By CMTAIdentifier
