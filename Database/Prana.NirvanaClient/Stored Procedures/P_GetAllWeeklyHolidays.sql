  
/* Object:  Stored Procedure dbo.P_GetAllWeeklyHolidays    Script Date: 07/10/2008
	Created: Bhupesh Bareja
	Purpose: To get all weekly holidays available in the database.
	Execute: P_GetAllWeeklyHolidays
*/  
CREATE PROCEDURE dbo.P_GetAllWeeklyHolidays 
AS  
 SELECT   WeeklyHolidayID, HolidayName 
FROM         T_WeeklyHolidays