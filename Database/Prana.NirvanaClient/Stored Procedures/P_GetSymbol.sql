


/****** Object:  Stored Procedure dbo.P_GetSymbol    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_GetSymbol
	(
		@symbolConventionID int
	)
AS
	SELECT     SymbolConventionID, SymbolConventionName, SymbolConventionShortName
	FROM         T_SymbolConvention
	Where SymbolConventionID = @symbolConventionID



