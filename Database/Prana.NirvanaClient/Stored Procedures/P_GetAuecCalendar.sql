CREATE PROCEDURE [dbo].[P_GetAuecCalendar]
(
@auecid int,
@year int
)
AS
select cal.calendarname from T_calendar cal right outer join T_calendarauec calAUEC 
on cal.calendarid = calAUEC.calendarid where auecid = @auecid and calendaryear = @year