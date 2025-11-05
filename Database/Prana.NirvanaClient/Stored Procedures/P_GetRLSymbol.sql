

CREATE PROCEDURE dbo.P_GetRLSymbol

		

AS
SELECT DISTINCT SymbolID, Symbol
FROM         T_Symbol

