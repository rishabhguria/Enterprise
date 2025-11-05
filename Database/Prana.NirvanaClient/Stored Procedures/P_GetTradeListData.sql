CREATE PROCEDURE [dbo].[P_GetTradeListData]
	@TradeListId int
AS

BEGIN
SELECT TradeListName, TradeListData
FROM T_RebalancersTradeList
WHERE TradeListId=@TradeListId
END
