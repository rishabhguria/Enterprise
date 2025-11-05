CREATE TABLE [dbo].[PM_CloseTradeReportRuns] (
    [CloseTradeReportID]   INT      IDENTITY (1, 1) NOT NULL,
    [UserID]               INT      NOT NULL,
    [CloseTradeReportDate] DATETIME NOT NULL,
    [ThirdPartyID]         INT      NOT NULL,
    [ReportMethodology]    TINYINT  NOT NULL,
    [ReportAlgorithm]      TINYINT  NOT NULL,
    [Comments]             TEXT     NULL,
    [AssetID]              INT      NOT NULL,
    CONSTRAINT [PK_PM_CloseTradeReportRuns] PRIMARY KEY CLUSTERED ([CloseTradeReportID] ASC),
    CONSTRAINT [FK_PM_CloseTradeReportRuns_T_ThirdParty] FOREIGN KEY ([ThirdPartyID]) REFERENCES [dbo].[T_ThirdParty] ([ThirdPartyID])
);

