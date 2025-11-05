
/****** Object:  Stored Procedure dbo.P_GetAllSymbolMappings    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAllSymbolMappings
AS
	SELECT   CVSymbolMappingID, CVAUECID, Symbol, MappedSymbol
FROM         T_CVSymbolMapping

