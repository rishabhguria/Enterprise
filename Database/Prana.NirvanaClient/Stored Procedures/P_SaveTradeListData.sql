CREATE PROCEDURE [dbo].[P_SaveTradeListData]
	@TradeListId int,
	@TradeListDate DateTime,
	@TradeListName varchar(50),
	@TradeListData nvarchar(max)
AS
BEGIN
if ((select count(*) from T_RebalancersTradeList where TradeListId=@TradeListId)>0)
BEGIN
	UPDATE T_RebalancersTradeList 
	SET TradeListName=@TradeListName,TradeListData=@TradeListData
	WHERE TradeListId=@TradeListId
END
ELSE
BEGIN
INSERT INTO T_RebalancersTradeList
	(
	TradeListId,
	TradeListDate,
	TradeListName,
	TradeListData
	)
	VALUES
	(
	@TradeListId,
	@TradeListDate,
	@TradeListName,
	@TradeListData
	)
END

END
