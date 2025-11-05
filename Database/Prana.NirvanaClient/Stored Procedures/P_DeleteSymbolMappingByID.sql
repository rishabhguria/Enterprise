CREATE PROCEDURE dbo.P_DeleteSymbolMappingByID
	(
		@cvSymbolMappingID int	
	)
AS
Delete T_CVSymbolMapping Where CVSymbolMappingID = @cvSymbolMappingID

