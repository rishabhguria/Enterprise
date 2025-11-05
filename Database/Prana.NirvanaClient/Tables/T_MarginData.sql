CREATE TABLE [dbo].[T_MarginData] (
    [Date]             DATETIME      NULL,
    [Account]          VARCHAR (100) NULL,
    [CUSIP]            VARCHAR (100) NULL,
    [RIC]              VARCHAR (100) NULL,
    [Currency]         VARCHAR (100) NULL,
    [VMargin]          FLOAT (53)    NULL,
    [Quantity]         FLOAT (53)    NULL,
    [SettlementPrice]  FLOAT (53)    NULL,
    [Description]      VARCHAR (150) NULL,
    [FXRate]           FLOAT (53)    NULL,
    [Bloomberg]        VARCHAR (100) NULL,
    [ConvertedVMargin] FLOAT (53)    NULL,
    [FundID]           VARCHAR (100) NULL,
    [FundShortName]    VARCHAR (100) NULL,
    [AccountShortName] VARCHAR (100) NULL
);

