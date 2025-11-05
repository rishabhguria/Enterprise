CREATE TABLE [dbo].[PM_UserDefinedMTDPnL] (
    [UserDefinedMTDPnLID] INT        IDENTITY (1, 1) NOT NULL,
    [UserDefinedMTDPnL]   FLOAT (53) NOT NULL,
    [Date]                DATETIME   NOT NULL,
    [FundID]              INT        CONSTRAINT [DF_PM_UserDefinedMTDPnL_FundID] DEFAULT ((0)) NOT NULL
);

