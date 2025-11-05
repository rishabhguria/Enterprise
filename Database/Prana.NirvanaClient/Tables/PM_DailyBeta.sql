CREATE TABLE [dbo].[PM_DailyBeta] (
    [DayBetaID] INT           IDENTITY (1, 1) NOT NULL,
    [Date]      DATETIME      NOT NULL,
    [Symbol]    VARCHAR (100) NOT NULL,
    [Beta]      FLOAT (53)    NOT NULL,
    CONSTRAINT [PK_PM_DayBeta] PRIMARY KEY CLUSTERED ([DayBetaID] ASC)
);

