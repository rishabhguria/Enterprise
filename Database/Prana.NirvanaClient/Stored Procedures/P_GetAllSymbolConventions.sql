


/****** Object:  Stored Procedure dbo.P_GetAllSymbolConventions    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAllSymbolConventions
AS
	SELECT   SymbolConventionID, SymbolConventionName, SymbolConventionShortName
FROM         T_SymbolConvention Order By SymbolConventionName



