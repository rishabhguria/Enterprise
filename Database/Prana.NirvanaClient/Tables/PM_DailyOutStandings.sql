CREATE TABLE [dbo].[PM_DailyOutStandings] (
    [DayOutStandingID] INT           IDENTITY (1, 1) NOT NULL,
    [Date]             DATETIME      NOT NULL,
    [Symbol]           VARCHAR (100) NOT NULL,
    [OutStandings]     FLOAT (53)    NOT NULL,
    CONSTRAINT [PK_PM_DayOutStandingID] PRIMARY KEY CLUSTERED ([DayOutStandingID] ASC)
);

