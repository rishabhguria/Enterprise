CREATE TABLE [dbo].[PM_NAVValue] (
    [NAVValueID] INT        IDENTITY (1, 1) NOT NULL,
    [NAVValue]   FLOAT (53) NOT NULL,
    [Date]       DATETIME   NOT NULL,
    [FundID]     INT        CONSTRAINT [DF_PM_NAVValue_FundID] DEFAULT ((0)) NOT NULL
);

