
Create PROCEDURE [dbo].[P_GetMarketDataTypesForUser]
(
		@companyUserID int		
)
AS
	select MDT.MarketDataTypeID, MDT.MarketDataType from T_MarketDataType MDT, T_CompanyUserMarketDataTypes CUMDT
	where MDT.MarketDataTypeID = CUMDT.MarketDataTypeID and CUMDT.companyUserID = @companyUserID
	
