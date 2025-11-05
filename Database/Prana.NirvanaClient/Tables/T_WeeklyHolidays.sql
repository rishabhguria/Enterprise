CREATE TABLE [dbo].[T_WeeklyHolidays] (
    [WeeklyHolidayID] INT           NOT NULL,
    [HolidayName]     NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_WeeklyHolidays] PRIMARY KEY CLUSTERED ([WeeklyHolidayID] ASC)
);

