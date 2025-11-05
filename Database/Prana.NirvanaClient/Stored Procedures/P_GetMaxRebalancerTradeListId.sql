CREATE PROCEDURE [dbo].[P_GetMaxRebalancerTradeListId]
AS
	SELECT MAX(TradeListId) AS MaxRebalancerTradeListId from T_RebalancersTradeList
RETURN 0
