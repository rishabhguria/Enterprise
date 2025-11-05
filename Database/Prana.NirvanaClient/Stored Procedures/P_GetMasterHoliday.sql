


/****** Object:  Stored Procedure dbo.P_GetMasterHolidays    Script Date: 12/13/2005 12:40:24 PM ******/
CREATE PROCEDURE dbo.P_GetMasterHoliday
	(
		@holidayID int
	)
AS
Select HolidayID, HolidayDate, Description
From T_Holidays
Where HolidayID = @holidayID
	


