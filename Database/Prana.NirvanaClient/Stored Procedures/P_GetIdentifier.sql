


/****** Object:  Stored Procedure dbo.P_GetIdentifier    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_GetIdentifier
(
	@identifierID int
)
AS
	SELECT   IdentifierID, IdentifierName
FROM         T_Identifier WHERE IdentifierID = @identifierID



