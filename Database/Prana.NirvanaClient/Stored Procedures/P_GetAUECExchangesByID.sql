/****** Object:  Stored Procedure dbo.P_GetAUECExchangesByID    Script Date: 11/25/2005 7:40:22 AM ******/  
CREATE PROCEDURE dbo.P_GetAUECExchangesByID  
 (  
  @auecID int    
 )  
AS  
   
 SELECT AUECID, ExchangeID, FullName, DisplayName, TimeZone,   
  RegularTradingStartTime, RegularTradingEndTime, PreMarketTradingStartTime, PreMarketTradingEndTime,   
  LunchTimeStartTime, LunchTimeEndTime, PostMarketTradingStartTime, PostMarketTradingEndTime, Country, StateID,  
        UnitID, SettlementDaysBuy, DayLightSaving, PreMarket, PostMarket, RegularTime, LunchTime,   
        CurrencyConversion, CountryFlagID, ExchangeLogoID, TimeZoneOffSet,SettlementDaysSell  
 FROM T_AUEC  
 Where  AUECID NOT IN (Select AUECID from T_AUEC where AUECID = @auecID)