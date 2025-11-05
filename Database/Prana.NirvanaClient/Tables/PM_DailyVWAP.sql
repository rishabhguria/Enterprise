CREATE TABLE [dbo].[PM_DailyVWAP] (
    [DayVWAPID] INT           IDENTITY (1, 1) NOT NULL,
    [Date]            DATETIME      NOT NULL,
    [Symbol]          VARCHAR (100) NOT NULL,
    [VWAP]      FLOAT (53)    NOT NULL,
    CONSTRAINT [PK_PM_DailyVWAP] PRIMARY KEY CLUSTERED ([DayVWAPID] ASC)
);

