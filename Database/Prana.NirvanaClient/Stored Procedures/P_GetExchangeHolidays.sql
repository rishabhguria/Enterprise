


/****** Object:  Stored Procedure dbo.P_GetAUECHolidays    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_GetExchangeHolidays
	(
		@exchangeID int
	)
AS
Select ExchangeHolidayID, HolidayDate, Description, ExchangeID
From T_ExchangeHolidays
Where ExchangeID = @exchangeID
	


