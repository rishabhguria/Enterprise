/****** Object:  Stored Procedure dbo.P_SaveMasterHolidays    Script Date: 12/13/2005 12:35:24 PM ******/
CREATE PROCEDURE dbo.P_SaveMasterHolidays 
	(
		@holidayID int,
		@date datetime,
		@description varchar(50),
		@result	int
	)
AS
	Declare @total int
	set @total = 0
	Declare @Count int
	set @Count = 0

	
	Select @total = Count(*)
	From T_Holidays
	Where HolidayID = @holidayID	

if @total > 0
begin
        Select @Count=Count(*) From T_Holidays
    	Where HolidayDate= @Date and HolidayID <> @holidayID

		if @Count = 0 
			begin	
					Update T_Holidays
					Set HolidayDate = @Date, 
						Description = @Description
					Where  HolidayID = @holidayID		
					Set @result = @holidayID
			end 
      Else
             Set @result = -1
end
else
      begin

		--Insert Data
		Insert Into T_Holidays(HolidayDate, Description)
		Values(@date, @description)
		
		Set @result = scope_identity()
	end
	
select @result


