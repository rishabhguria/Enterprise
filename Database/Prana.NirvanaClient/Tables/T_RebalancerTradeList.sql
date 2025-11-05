CREATE TABLE [dbo].[T_RebalancersTradeList]
(
	[TradeListId] INT NOT NULL unique,
	[TradeListDate] DateTime NOT NULL,
	[TradeListName] VARCHAR(50) NOT NULL,
	[TradeListData] NVARCHAR(MAX) NOT NULL,
	PRIMARY KEY(TradeListDate,TradeListName)
)
