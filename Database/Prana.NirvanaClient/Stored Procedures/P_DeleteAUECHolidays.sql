  
/****** Object:  Stored Procedure dbo.P_DeleteAUECHolidays    Script Date: 11/28/2005 5:10:24 PM ******/  
CREATE PROCEDURE [dbo].[P_DeleteAUECHolidays]  
 (  
  @year int,
  @auecID int  
 )  
AS  
 DELETE T_AUECHolidays  
WHERE datepart(yy,holidaydate) = @year and auecid = @auecid
  