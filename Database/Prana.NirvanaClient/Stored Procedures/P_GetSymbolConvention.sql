


/****** Object:  Stored Procedure dbo.P_GetSymbolConvention    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_GetSymbolConvention
	(
		@symbolConventionID int
	)
AS
	SELECT     SymbolConventionID, SymbolConventionName, SymbolConventionShortName
	FROM         T_SymbolConvention
	Where SymbolConventionID = @symbolConventionID



