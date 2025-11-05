      
      
        
/********************************************************        
CREATED BY : RAHUL GUPTA   CREATED ON : 25-NOV-2011         
*********************************************************/        
CREATE PROCEDURE [dbo].[P_SaveAuecCalendar]          
(          
@CalendarName nvarchar(max),      
@CalendarYear int,          
@AuecID int          
)          
AS          
          
DECLARE @id int          
SET @id = (SELECT calendarID FROM T_calendar WHERE calendarname = @CalendarName and calendaryear = @CalendarYear)  
  
DECLARE @previousID int
SET @previousID = (SELECT cal.calendarid FROM T_calendar cal right join T_calendarauec calAUEC 
				   ON cal.calendarid = calAUEC.calendarid  
			   	   WHERE cal.calendaryear = @CalendarYear and calAUEC.auecid = @AuecID)

IF( @previousID > 0)  
BEGIN   
UPDATE T_calendarauec  
SET calendarid = @id  where auecid = @auecid and calendarid = @previousID
END  
  
ELSE   
BEGIN         
INSERT INTO T_CalendarAUEC(AuecID, CalendarID) VALUES          
(          
@auecid,          
@id          
)   
end