
/****** Object:  Stored Procedure dbo.P_InsertExchangeForm    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE [dbo].[P_InsertExchangeForm]
	(
		@ExchangeID int,
		@FullName varchar(50),
		@DisplayName varchar(50), 
		@TimeZone varchar(100), 
		@LunchTimeStartTime datetime, 
		@LunchTimeEndTime datetime, 
		@RegularTradingStartTime datetime, 
		@RegularTradingEndTime datetime, 				
		@RegularTime int,
		@LunchTime int,
		@Country varchar(50), 
		@StateID int,
		@CountryFlagID int,
		@LogoID int,
		@ExchangeIdentifier varchar(10),
		@TimeZoneOffSet float,
		@result	int
	)
AS
	Declare @total int
	set @total = 0
	declare @count int
	set @count = 0
	
	Select @total = Count(*)
	From T_Exchange
	Where ExchangeID = @ExchangeID
	
	if(@total > 0)
	Begin
		select @count = count(*)
		from T_Exchange 
		Where (FullName = @FullName OR DisplayName = @DisplayName) AND ExchangeID <> @ExchangeID
		if(@count = 0)
		begin
			/* select @count = count(*)
				from T_Exchange 
				Where ExchangeIdentifier = @exchangeIdentifier AND ExchangeID <> @ExchangeID
				if(@count = 0)
				begin */
					--Update Data
					Update T_Exchange
					Set FullName = @FullName, 
						DisplayName = @DisplayName, 
						TimeZone = @TimeZone, 
						RegularTime = @RegularTime,
						RegularTradingStartTime = @RegularTradingStartTime,
						RegularTradingEndTime = @RegularTradingEndTime, 
						LunchTime = @LunchTime,
						LunchTimeStartTime = @LunchTimeStartTime, 
						LunchTimeEndTime = @LunchTimeEndTime, 
						Country = @Country, 
						StateID = @StateID,
						CountryFlagID = @CountryFlagID,
						LogoID = @LogoID,
						ExchangeIdentifier = @ExchangeIdentifier,
						TimeZoneOffSet = @TimeZoneOffSet
					Where ExchangeID = @ExchangeID
					Set @result = @ExchangeID
				/* End
				else
				begin
					Set @result = -2
				end */
			end
			else
			begin
				Set @result = -1
			end
		end
		else
		begin
		
			/* select @count = count(*)
				from T_Exchange
				Where ExchangeIdentifier = @exchangeIdentifier
				if(@count > 0)
				begin
					Select @result = -2
				end
				else
				begin */
					select @count = count(*)
					from T_Exchange 
					Where FullName = @FullName OR DisplayName = @DisplayName
					
					if(@count > 0)
					begin
						
						Set @result = -1
					end
					else
					begin
						--Insert Data
						INSERT INTO T_Exchange(FullName, DisplayName, TimeZone, RegularTime, RegularTradingStartTime, 
							RegularTradingEndTime, LunchTime, LunchTimeStartTime, LunchTimeEndTime, Country, StateID, CountryFlagID,
							LogoID, ExchangeIdentifier, TimeZoneOffSet)
						Values(@FullName, @DisplayName, @TimeZone, @RegularTime, @RegularTradingStartTime, 
							@RegularTradingEndTime, @LunchTime, @LunchTimeStartTime, @LunchTimeEndTime, @Country, @StateID,
							@CountryFlagID, @LogoID, @ExchangeIdentifier, @TimeZoneOffSet)
							
							Set @result = scope_identity()
					end
				--end 
		end
select @result
	
	/*
	
	
	
	if(@total = 0)
	Begin
		--Insert Data
INSERT INTO T_Exchange(FullName, DisplayName, TimeZone, RegularTime, RegularTradingStartTime, 
			RegularTradingEndTime, LunchTime, LunchTimeStartTime, LunchTimeEndTime, Country, StateID, CountryFlagID,
			LogoID)
		Values(@FullName, @DisplayName, @TimeZone, @RegularTime, @RegularTradingStartTime, 
			@RegularTradingEndTime, @LunchTime, @LunchTimeStartTime, @LunchTimeEndTime, @Country, @StateID,
			@CountryFlagID, @LogoID)
			
			Set @result = scope_identity()
	end
	else
	Begin
		--Update Data
		Update T_Exchange
		Set FullName = @FullName, 
			DisplayName = @DisplayName, 
			--Currency = @Currency, 
			--Unit = @Unit, 
			TimeZone = @TimeZone, 
			RegularTime = @RegularTime,
			RegularTradingStartTime = @RegularTradingStartTime,
			RegularTradingEndTime = @RegularTradingEndTime, 
			--PreMarketTradingStartTime = @PreMarketTradingStartTime, 
			--PreMarketTradingEndTime = @PreMarketTradingEndTime, 
			LunchTime = @LunchTime,
			LunchTimeStartTime = @LunchTimeStartTime, 
			LunchTimeEndTime = @LunchTimeEndTime, 
			--PostMarketTradingStartTime = @PostMarketTradingStartTime, 
			--PostMarketTradingEndTime = @PostMarketTradingEndTime, 
			--SettlementDays = @SettlementDays, 
			--DayLightSaving = @DayLightSaving,
			Country = @Country, 
			StateID = @StateID,
			CountryFlagID = @CountryFlagID,
			LogoID = @LogoID
		Where ExchangeID = @ExchangeID
		
		Set @result = @ExchangeID
	end
	select @result
	*/

