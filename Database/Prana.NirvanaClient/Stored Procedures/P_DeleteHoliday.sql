
/****** Object:  Stored Procedure dbo.P_DeleteHoliday    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_DeleteHoliday
	(
		@holidayID int
	)
AS
	DELETE T_AUECHolidays
WHERE	HolidayID = @holidayID

