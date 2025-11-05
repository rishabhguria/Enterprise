
/****** Object:  Stored Procedure dbo.P_InsertAUECHolidays    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_InsertAUECHolidaysExchangeDefault
	(
		@auecID int,
		@HolidayID int,
		@Date datetime,
		@Description varchar(60)
	)
AS
			
	--Insert Data
		Insert Into T_AUECHolidays(AUECID, HolidayDate, Description)
		Values(@auecID, @Date, @Description)
	
	
