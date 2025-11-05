CREATE TABLE [dbo].[PM_CloseTradeReportRunsFunds] (
    [CloseTradeReportRunsFundsID] INT IDENTITY (1, 1) NOT NULL,
    [CloseTradeReportID]          INT NULL,
    [CompanyFundID]               INT NULL,
    CONSTRAINT [PK_CloseTradeReportRunsFunds] PRIMARY KEY CLUSTERED ([CloseTradeReportRunsFundsID] ASC),
    CONSTRAINT [FK_PM_CloseTradeReportRunsFunds_PM_CloseTradeReportRuns] FOREIGN KEY ([CloseTradeReportID]) REFERENCES [dbo].[PM_CloseTradeReportRuns] ([CloseTradeReportID])
);

