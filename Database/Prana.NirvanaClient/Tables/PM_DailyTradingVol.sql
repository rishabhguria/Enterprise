CREATE TABLE [dbo].[PM_DailyTradingVol] (
    [DayTradingVolID] INT           IDENTITY (1, 1) NOT NULL,
    [Date]            DATETIME      NOT NULL,
    [Symbol]          VARCHAR (100) NOT NULL,
    [TradingVolume]   FLOAT (53)    NULL,
    CONSTRAINT [PK_PM_DayTradingVolID] PRIMARY KEY CLUSTERED ([DayTradingVolID] ASC)
);

