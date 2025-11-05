-----------------------------------------------------------------------------------
--Updated BY: Bhavana Rao
--Date: 31/03/14
--Purpose: added columns IsMarketOff and IsSettlementOff in table T_calendarholidays
-----------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[P_GetCalendarHolidays]  
(  
 @calendarID int  
)  
AS  
  
Select holidayDate,holidaydescription,IsMarketoff,IsSettlementOff  
from T_calendarholidays
where CalendarID_FK = @calendarID

