CREATE TABLE [dbo].[PM_StartOfMonthCapitalAccount] (
    [StartOfMonthCapitalAccountID] INT        IDENTITY (1, 1) NOT NULL,
    [StartOfMonthCapitalAccount]   FLOAT (53) NOT NULL,
    [Date]                         DATETIME   NOT NULL,
    [FundID]                       INT        CONSTRAINT [DF_PM_StartOfMonthCapitalAccount_FundID] DEFAULT ((0)) NOT NULL
);

