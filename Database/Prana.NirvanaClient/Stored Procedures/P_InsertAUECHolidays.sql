-----------------------------------------------------------------------------------
--Updated BY: Bhavana Rao
--Date: 31/03/14
--Purpose: added columns IsMarketOff and IsSettlementOff in table T_AUECHolidays
----------------------------------------------------------------------------------- 
CREATE PROCEDURE [dbo].[P_InsertAUECHolidays]
	(
		@auecID int,
		@HolidayID int,
		@Date datetime,
		@Description varchar(60),
        @Marketoff bit, 
        @SettlementOff bit
	)
AS
	
if @HolidayID > 0
begin
		Update T_AUECHolidays
		Set HolidayDate = @Date, 
			Description = @Description,
            IsMarketoff = @Marketoff,
            IsSettlementOff = @SettlementOff
		Where  HolidayID = @HolidayID

end
else
begin

		--Insert Data
		Insert Into T_AUECHolidays(AUECID, HolidayDate, Description, IsMarketoff, IsSettlementOff)
		Values(@auecID, @Date, @Description, @Marketoff, @SettlementOff)
	end
