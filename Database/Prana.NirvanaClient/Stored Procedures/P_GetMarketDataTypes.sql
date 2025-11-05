create PROCEDURE [dbo].[P_GetMarketDataTypes]
AS
	select MarketDataTypeID, MarketDataType from T_MarketDataType
