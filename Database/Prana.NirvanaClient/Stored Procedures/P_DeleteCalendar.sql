CREATE PROCEDURE [dbo].[P_DeleteCalendar]  
(  
@name nvarchar(max),  
@year int,
@auecid int
)  
AS  
  
declare @CalendarID int  
set @CalendarID = (select calendarid from T_calendar where calendarname = @name and calendaryear = @year)  
  
delete T_Calendarauec where calendarid = @CalendarID  
delete T_auecholidays where datepart(yy,holidaydate) = @year and auecid = @auecid 
delete T_calendarholidays where calendarid_fk = @CalendarID  
delete T_calendar where calendarid = @CalendarID  