CREATE TABLE [dbo].[PM_ClosedTrades] (
    [ClosedTradeID]      INT           NOT NULL,
    [ReconciledTradeID]  INT           NOT NULL,
    [AllocatedOrderID]   NVARCHAR (50) NOT NULL,
    [OpenTradeCode]      INT           NOT NULL,
    [ClosingQty]         INT           NOT NULL,
    [CloseTradeReportID] INT           NOT NULL,
    CONSTRAINT [PK_PM_ClosedTrades] PRIMARY KEY CLUSTERED ([ClosedTradeID] ASC),
    CONSTRAINT [FK_PM_ClosedTrades_PM_CloseTradeReportRuns] FOREIGN KEY ([CloseTradeReportID]) REFERENCES [dbo].[PM_CloseTradeReportRuns] ([CloseTradeReportID]),
    CONSTRAINT [FK_PM_ClosedTrades_PM_ReconciledTrades] FOREIGN KEY ([ReconciledTradeID]) REFERENCES [dbo].[PM_ReconciledTrades] ([ReconciledTradeID])
);

