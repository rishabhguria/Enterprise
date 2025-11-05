CREATE TABLE [dbo].[T_AUECWeeklyHolidays] (
    [AUECWeeklyHolidayID] INT IDENTITY (1, 1) NOT NULL,
    [WeeklyHolidayID]     INT NOT NULL,
    [AUECID]              INT NOT NULL,
    CONSTRAINT [FK_T_AUECWeeklyHolidays_T_AUEC] FOREIGN KEY ([AUECID]) REFERENCES [dbo].[T_AUEC] ([AUECID]),
    CONSTRAINT [FK_T_AUECWeeklyHolidays_T_WeeklyHolidays] FOREIGN KEY ([WeeklyHolidayID]) REFERENCES [dbo].[T_WeeklyHolidays] ([WeeklyHolidayID])
);

