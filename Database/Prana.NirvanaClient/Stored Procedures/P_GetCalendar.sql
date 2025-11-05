
    
/*********************************    
Stored Procedure P_GetCalendar  Created On : 11/16/2011 6:00PM Created By : Rahul Gupta    
**********************************/    
    
CREATE Procedure [dbo].[P_GetCalendar]        
As    
Select CalendarId,CalendarName,CalendarYear from dbo.T_Calendar --where calendarYear = @year    
    