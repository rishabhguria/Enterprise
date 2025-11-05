CREATE TABLE [dbo].[PM_TradesSentToDesk] (
    [ReconciledTradeID] INT            NOT NULL,
    [SentToID]          INT            NOT NULL,
    [SentByID]          INT            NOT NULL,
    [SymbolID]          INT            NULL,
    [Qty]               INT            NULL,
    [Comments]          NVARCHAR (500) NULL,
    CONSTRAINT [PK_PM_TradesSentToDesk] PRIMARY KEY CLUSTERED ([ReconciledTradeID] ASC),
    CONSTRAINT [FK_PM_TradesSentToDesk_PM_ReconciledTrades] FOREIGN KEY ([ReconciledTradeID]) REFERENCES [dbo].[PM_ReconciledTrades] ([ReconciledTradeID]),
    CONSTRAINT [FK_PM_TradesSentToDesk_T_User] FOREIGN KEY ([SentToID]) REFERENCES [dbo].[T_User] ([UserID]),
    CONSTRAINT [FK_PM_TradesSentToDesk_T_User1] FOREIGN KEY ([SentByID]) REFERENCES [dbo].[T_User] ([UserID])
);

