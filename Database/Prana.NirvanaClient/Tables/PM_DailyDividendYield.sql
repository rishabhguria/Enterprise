CREATE TABLE [dbo].[PM_DailyDividendYield] (
    [DayDividendYieldID] INT           IDENTITY (1, 1) NOT NULL,
    [Date]               DATETIME      NOT NULL,
    [Symbol]             VARCHAR (100) NOT NULL,
    [DividendYield]      FLOAT (53)    NOT NULL,
    CONSTRAINT [PK_PM_DayDividendYield] PRIMARY KEY CLUSTERED ([DayDividendYieldID] ASC)
);

