

/****** Object:  Stored Procedure dbo.P_DeleteHoliday    Script Date: 11/23/2005 6:20:20 PM ******/
CREATE PROCEDURE dbo.P_DeleteExchangeHoliday
	(
		@exchangeHolidayID int	
	)
AS

		Delete T_ExchangeHolidays
		Where ExchangeHolidayID = @exchangeHolidayID


