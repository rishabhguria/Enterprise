
/****** Object:  Stored Procedure dbo.P_CopyAUECExchangeHolidays    Script Date: 11/25/2005 7:45:24 PM ******/
CREATE PROCEDURE dbo.P_CopyAUECExchangeHolidays
	(
		@destinationAUECID int,
		@sourceAUECID int
	)
AS
	
DELETE FROM T_AUECHolidays WHERE AUECID = @destinationAUECID

Insert Into T_AUECHolidays(AUECID, HolidayDate, Description)
	(SELECT @destinationAUECID, AH.HolidayDate, AH.Description from T_AUECHolidays AH
		WHERE AH.AUECID = @sourceAUECID)
		
		