CREATE TABLE [dbo].[PM_AmendedPositions] (
    [PositionAmendmentID] INT            NOT NULL,
    [ReconciledTradeID]   INT            NULL,
    [FieldName]           NVARCHAR (100) NULL,
    [OriginalValue]       NVARCHAR (100) NULL,
    [ChangedValue]        NVARCHAR (100) NULL,
    [AmendedByID]         INT            NOT NULL,
    [AmmendmentDateTime]  DATETIME       NOT NULL,
    [Comment]             NVARCHAR (500) NULL,
    CONSTRAINT [PK_PM_AmendedPositions] PRIMARY KEY CLUSTERED ([PositionAmendmentID] ASC),
    CONSTRAINT [FK_PM_AmendedPositions_PM_ReconciledTrades] FOREIGN KEY ([ReconciledTradeID]) REFERENCES [dbo].[PM_ReconciledTrades] ([ReconciledTradeID]),
    CONSTRAINT [FK_PM_AmendedPositions_T_User] FOREIGN KEY ([AmendedByID]) REFERENCES [dbo].[T_User] ([UserID])
);

