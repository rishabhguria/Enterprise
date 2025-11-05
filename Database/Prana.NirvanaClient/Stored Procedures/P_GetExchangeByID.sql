
/****** Object:  Stored Procedure dbo.P_GetExchangeByID    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_GetExchangeByID
	(
		@exchangeByID int		
	)
AS
	/* SELECT ExchangeID, FullName, DisplayName, TimeZone, RegularTradingStartTime, 
		RegularTradingEndTime, PreMarketTradingStartTime, PreMarketTradingEndTime, LunchTimeStartTime, 
        LunchTimeEndTime, PostMarketTradingStartTime, PostMarketTradingEndTime, Country, StateID,
        Currency, Unit, SettlementDays, DayLightSaving
	FROM	T_Exchange
	Where ExchangeID = @exchangeByID */

	SELECT ExchangeID, FullName, DisplayName, TimeZone, RegularTradingStartTime, RegularTradingEndTime, 
		LunchTimeStartTime, LunchTimeEndTime, Country, StateID, RegularTime, LunchTime, CountryFlagID, LogoID,
		ExchangeIdentifier, TimeZoneOffSet
	FROM	T_Exchange
	Where ExchangeID = @exchangeByID