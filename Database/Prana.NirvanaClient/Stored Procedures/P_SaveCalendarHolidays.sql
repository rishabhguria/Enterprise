-----------------------------------------------------------------------------------
--Updated BY: Bhavana Rao
--Date: 31/03/14
--Purpose: added columns IsMarketOff and IsSettlementOff in table T_calendarholidays
-----------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[P_SaveCalendarHolidays]    
(       
 @holidayID int,    
 @holidaydate datetime,    
 @description varchar(200),
 @calendarID int,
 @Marketoff bit, 
 @SettlementOff bit    
)    
AS    

DECLARE @id int  
IF (@calendarID > 0)
SET @id = @calendarID
ELSE
SET @id = (SELECT max(calendarID) FROM T_calendar)  
    
INSERT INTO T_CalendarHolidays    
(    
 CalendarID_FK,    
 HolidayDate,    
 HolidayDescription,
 IsMarketoff, 
 IsSettlementOff    
)    
VALUES
(  
 @id,  
 @holidaydate,    
 @description,
 @Marketoff,
 @SettlementOff    
)
