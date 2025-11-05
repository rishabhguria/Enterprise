CREATE TABLE [dbo].[PM_YTDPnL] (
    [StaticPnLID]   INT          IDENTITY (1, 1) NOT NULL,
    [FromDate]      DATETIME     NULL,
    [UpToDate]      DATETIME     NULL,
    [Symbol]        VARCHAR (50) NULL,
    [RealizedPnL]   FLOAT (53)   NULL,
    [UnrealizedPnL] FLOAT (53)   NULL,
    [FundID]        INT          NULL
);

