CREATE TABLE [dbo].[PM_DailyVolatility] (
    [DayVolatilityID] INT           IDENTITY (1, 1) NOT NULL,
    [Date]            DATETIME      NOT NULL,
    [Symbol]          VARCHAR (100) NOT NULL,
    [Volatility]      FLOAT (53)    NOT NULL,
    CONSTRAINT [PK_PM_DayVolatility] PRIMARY KEY CLUSTERED ([DayVolatilityID] ASC)
);

