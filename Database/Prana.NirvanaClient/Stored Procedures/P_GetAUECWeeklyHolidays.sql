  
/* Object:  Stored Procedure dbo.P_GetAUECWeeklyHolidays    Script Date: 07/10/2008
	Created: Bhupesh Bareja
	Purpose: To get weekly holidays for a given AUEC available in the database.
	Execute: P_GetAUECWeeklyHolidays 1
*/  
CREATE PROCEDURE dbo.P_GetAUECWeeklyHolidays 
(  
  @auecID int
)
AS  
 SELECT   WH.WeeklyHolidayID, WH.HolidayName 
FROM         T_AUECWeeklyHolidays AWH INNER JOIN T_WeeklyHolidays WH ON AWH.WeeklyHolidayID = WH.WeeklyHolidayID
				Where AUECID = @auecID