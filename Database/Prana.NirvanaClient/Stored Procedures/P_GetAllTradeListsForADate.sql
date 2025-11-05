CREATE PROCEDURE [dbo].[P_GetAllTradeListsForADate]
	@TradeListDate DateTime
AS

BEGIN

SELECT TradeListId,TradeListName 
FROM T_RebalancersTradeList
WHERE TradeListDate=@TradeListDate

END
