CREATE TABLE [dbo].[PM_DailyDelta] (
    [DayDeltaID] INT           IDENTITY (1, 1) NOT NULL,
    [Date]       DATETIME      NOT NULL,
    [Symbol]     VARCHAR (100) NOT NULL,
    [Delta]      FLOAT (53)    NOT NULL,
    CONSTRAINT [PK_PM_DayDeltaID] PRIMARY KEY CLUSTERED ([DayDeltaID] ASC)
);

