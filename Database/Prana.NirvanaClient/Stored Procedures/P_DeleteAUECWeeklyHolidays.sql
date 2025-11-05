  
/* Object:  Stored Procedure dbo.P_DeleteAUECWeeklyHolidays    Script Date: 07/10/2008
	Created: Bhupesh Bareja
	Purpose: To save weekly holidays for a given AUEC available.
	Execute: P_DeleteAUECWeeklyHolidays 1
*/  
CREATE PROCEDURE dbo.P_DeleteAUECWeeklyHolidays  
 (  
  @auecID int  
 )  
AS  
 DELETE T_AUECWeeklyHolidays  
WHERE AUECID = @auecID  
  