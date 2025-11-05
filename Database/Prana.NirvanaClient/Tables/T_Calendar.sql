CREATE TABLE [dbo].[T_Calendar] (
    [CalendarID]   INT           IDENTITY (1, 1) NOT NULL,
    [CalendarName] NVARCHAR (50) NOT NULL,
    [CalendarYear] INT           NOT NULL,
    CONSTRAINT [PK_T_Calendar] PRIMARY KEY CLUSTERED ([CalendarID] ASC)
);

