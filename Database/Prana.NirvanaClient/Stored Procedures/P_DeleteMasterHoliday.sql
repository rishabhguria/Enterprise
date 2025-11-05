

/****** Object:  Stored Procedure dbo.P_DeleteMasterHoliday    Script Date: 12/13/2005 12:45:20 PM ******/

CREATE PROCEDURE dbo.P_DeleteMasterHoliday
	(
		@holidayID int
	)
AS

	Delete T_Holidays
	Where HolidayID = @holidayID



