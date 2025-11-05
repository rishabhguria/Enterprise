


/****** Object:  Stored Procedure dbo.P_GetSymbols    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_GetSymbols
AS
	Select SymbolID, Symbol, CompanyName
	From T_Symbol
	


