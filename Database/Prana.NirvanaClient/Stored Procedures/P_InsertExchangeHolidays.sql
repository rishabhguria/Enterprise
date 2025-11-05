


/****** Object:  Stored Procedure dbo.P_InsertExchangeHolidays    Script Date: 11/21/2005 12:55:24 PM ******/
CREATE PROCEDURE dbo.P_InsertExchangeHolidays
	(
		@exchangeID int,
		@holidayID int,
		@date datetime,
		@description varchar(50)
	)
AS
	Declare @total int
	set @total = 0
	
	Select @total = Count(*)
	From T_ExchangeHolidays
	Where ExchangeHolidayID = @holidayID	

if @total > 0
begin
		Update T_ExchangeHolidays
		Set HolidayDate = @Date, 
			Description = @Description
		Where  ExchangeHolidayID = @holidayID

end
else
begin

		--Insert Data
		Insert Into T_ExchangeHolidays(ExchangeID, HolidayDate, Description)
		Values(@exchangeID, @date, @description)
	end
	


