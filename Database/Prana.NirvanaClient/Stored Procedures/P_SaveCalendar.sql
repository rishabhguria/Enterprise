  
  
    
/****** Object:  Stored Procedure dbo.P_InsertCalendar    Script Date: 11/16/2011 4:52:24 PM   
       Created By : Rahul Gupta  
******/    
CREATE PROCEDURE [dbo].[P_SaveCalendar]  
 (    
  @CalendarID int,  
  @CalendarName varchar(max),  
  @CalendarYear int  
 )    
AS    

IF(@CalendarID > 0)
BEGIN
	UPDATE T_Calendar
	SET calendarname = @calendarName where calendarId = @CalendarID
END

ELSE  
BEGIN 
  Insert Into T_Calendar(CalendarName, CalendarYear) Values(@CalendarName, @CalendarYear)    
END  
     
  