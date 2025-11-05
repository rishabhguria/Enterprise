CREATE TABLE [dbo].[PM_CloseTradeReportRunsAUEC] (
    [CloseTradeReportRunsAUECID] INT IDENTITY (1, 1) NOT NULL,
    [CloseTradeReportID]         INT NULL,
    [AUECID]                     INT NULL,
    CONSTRAINT [PK_CloseTradeReportRunsAUEC] PRIMARY KEY CLUSTERED ([CloseTradeReportRunsAUECID] ASC),
    CONSTRAINT [FK_PM_CloseTradeReportRunsAUEC_PM_CloseTradeReportRuns] FOREIGN KEY ([CloseTradeReportID]) REFERENCES [dbo].[PM_CloseTradeReportRuns] ([CloseTradeReportID])
);

