CREATE PROCEDURE [dbo].[P_GetClOrderIdsOfMultiDayTrades]
AS
SELECT DISTINCT trades.ClOrderId FROM T_TradedOrders trades
INNER JOIN T_Fills fills ON trades.CLOrderID = fills.ClOrderID 
WHERE trades.TimeInForce IN (1, 6) 
-- 1 = GTC, 6 = GTD