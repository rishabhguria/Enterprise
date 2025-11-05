CREATE TABLE [dbo].[T_ThirdPartyFFRunReport] (
    [ThirdPartyFFRunID]   INT            IDENTITY (1, 1) NOT NULL,
    [FFUserID]            INT            NOT NULL,
    [CompanyID]           INT            NOT NULL,
    [CompanyThirdPartyID] INT            NOT NULL,
    [TradeDate]           DATETIME       NOT NULL,
    [StatusID]            INT            NOT NULL,
    [FilePathName]        NVARCHAR (500) NULL,
    [FormatId]            INT            CONSTRAINT [DF_T_ThirdPartyFFRunReport_FormatId] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_T_ThirdPartyFFRunReport] PRIMARY KEY CLUSTERED ([ThirdPartyFFRunID] ASC),
    CONSTRAINT [FK_T_ThirdPartyFFRunReport_T_ThirdPartyFFRunStatus] FOREIGN KEY ([StatusID]) REFERENCES [dbo].[T_ThirdPartyFFRunStatus] ([StatusID])
);

