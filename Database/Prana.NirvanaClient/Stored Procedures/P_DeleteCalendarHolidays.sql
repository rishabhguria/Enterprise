CREATE PROCEDURE [dbo].[P_DeleteCalendarHolidays](  
@CalendarID int
)  
AS  
  
delete T_calendarholidays where calendarid_fk = @CalendarID