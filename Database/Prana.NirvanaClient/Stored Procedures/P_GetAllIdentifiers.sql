


/****** Object:  Stored Procedure dbo.P_GetAllIdentifiers    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAllIdentifiers
AS
	SELECT   IdentifierID, IdentifierName
FROM         T_Identifier Order By IdentifierName



