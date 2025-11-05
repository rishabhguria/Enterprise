CREATE PROCEDURE dbo.P_GetAUECDetails  
 (  
  @auecID int  
 )  
AS  
  
 SELECT AUECID, AssetID, UnderlyingID, ExchangeID, FullName, DisplayName, UnitID, TimeZone, TimeZoneOffSet,  
           PreMarket, PreMarketTradingStartTime, PreMarketTradingEndTime, RegularTime, RegularTradingStartTime,   
           RegularTradingEndTime, LunchTime, LunchTimeStartTime, LunchTimeEndTime, PostMarket,   
           PostMarketTradingStartTime, PostMarketTradingEndTime, SettlementDaysBuy, DayLightSaving, Country, StateID,  
           CountryFlagID, ExchangeLogoID, CurrencyConversion, SymbolConventionID, ExchangeIdentifier, MarketDataProviderExchangeIdentifier,
           BaseCurrencyID, OtherCurrencyID, Multiplier, IsShortSaleConfirmation, ProvideFundNameWithTrade,   
           ProvideIdentifierNameWithTrade, IdentifierID, PurchaseSecFees, SaleSecFees, PurchaseStamp, SaleStamp,   
           PurchaseLevy, SaleLevy ,SettlementDaysSell,RoundLot 
 FROM T_AUEC  
 Where AUECID = @auecID  
  
    
    
   
  