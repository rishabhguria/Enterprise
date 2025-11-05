
/****** Object:  Stored Procedure dbo.P_GetAllExchanges    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAllExchanges
AS
	/* SELECT ExchangeID, FullName, DisplayName, TimeZone, RegularTradingStartTime, 
		RegularTradingEndTime, PreMarketTradingStartTime, PreMarketTradingEndTime, LunchTimeStartTime, 
        LunchTimeEndTime, PostMarketTradingStartTime, PostMarketTradingEndTime, Country, StateID,
        Currency, Unit, SettlementDays, DayLightSaving
	FROM	T_Exchange */
	
	SELECT ExchangeID, FullName, DisplayName, TimeZone, RegularTradingStartTime, RegularTradingEndTime, 
		LunchTimeStartTime, LunchTimeEndTime, Country, StateID, RegularTime, LunchTime, CountryFlagID, LogoID,
		ExchangeIdentifier, TimeZoneOffSet
	FROM	T_Exchange
	

