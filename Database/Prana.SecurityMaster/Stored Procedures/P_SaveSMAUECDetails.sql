/********************************************  
  
AUTHOR : RAHUL GUPTA     CREATED ON : 2012-03-28 

MODIFIED BY : RAHUL GUPTA	
MODIFIED DATE : 2013-02-07 
DESCRIPTION : 
Checking T_AUEC table has identity column or not and if it exists then setting IDENTITY_INSERT ON
 
********************************************/                  
                  
CREATE PROCEDURE [dbo].[P_SaveSMAUECDetails] (                  
  @auecID int,                  
  @exchangeID int,                  
  @FullName varchar(50),                  
  @DisplayName varchar(50),                   
  @Unit int,                   
  @TimeZone varchar(100),                   
  @PreMarketTradingStartTime datetime,                   
  @PreMarketTradingEndTime datetime,                   
  @LunchTimeStartTime datetime,                   
  @LunchTimeEndTime datetime,                   
  @RegularTradingStartTime datetime,                   
  @RegularTradingEndTime datetime,                       
  @PostMarketTradingStartTime datetime,                   
  @PostMarketTradingEndTime datetime,                   
  @SettlementDaysBuy int,                   
  @DayLightSaving varchar(50),                  
  @Country varchar(50),                   
  @StateID int,                      
  @preMarketCheck int,                  
  @postMarketCheck int,                  
  @regularTimeCheck int,                  
  @lunchTimeCheck int,                  
  @currencyConversion int,                  
  @countryFlagID int,                  
  @LogoID int,                  
  @TimeZoneOffSet float,                  
  @SymbolConventionID int,                  
  @ExchangeIdentifier varchar(50),
  @MarketDataProviderExchangeIdentifier varchar(50),
  @BaseCurrencyID int,                  
  @OtherCurrencyID int,                  
  @Multiplier float,                  
  @IsShortSaleConfirmation int,                  
  @ProvideFundNameWithTrade int,                  
  @ProvideIdentifierNameWithTrade int,                  
  @IdentifierID int,                       
  @AssetID int,                  
  @UnderlyingID int,                  
  @PurchaseSecFees float,                  
  @SaleSecFees float,                  
  @PurchaseStamp float,                  
  @SaleStamp float,                  
  @PurchaseLevy float,                  
  @SaleLevy float,                  
  @SettlementDaysSell int,                
  @result int                  
 )                  
AS                  
 DECLARE @total int                  
 SET @total = 0                  
 DECLARE @count int                  
 SET @count = 0      
 DECLARE @Identity bit
 SET @Identity = 0           
                   
 SELECT @total = COUNT(*)              
 FROM T_AUEC                  
 WHERE AUECID = @auecID                  
                   
 IF(@total > 0)                  
 BEGIN                  
  SELECT @count = COUNT(*)                  
  FROM T_AUEC                               
  WHERE DisplayName = @DisplayName AND AUECID <> @auecID                  
  IF(@count = 0)                  
  BEGIN                  
    SELECT @count = COUNT(*)                  
    FROM T_AUEC                   
    WHERE ExchangeIdentifier = @ExchangeIdentifier AND AUECID <> @auecID                  
    IF(@count = 0)                  
    BEGIN                  
    --Update Table                  
     UPDATE T_AUEC          
     SET FullName = @FullName,                   
      DisplayName = @DisplayName,                   
      UnitID = @Unit,                   
      TimeZone = @TimeZone,                   
      RegularTradingStartTime = @RegularTradingStartTime,                  
      RegularTradingEndTime = @RegularTradingEndTime,                   
      PreMarketTradingStartTime = @PreMarketTradingStartTime,                   
      PreMarketTradingEndTime = @PreMarketTradingEndTime,                   
      LunchTimeStartTime = @LunchTimeStartTime,                   
      LunchTimeEndTime = @LunchTimeEndTime,                   
      PostMarketTradingStartTime = @PostMarketTradingStartTime,                   
      PostMarketTradingEndTime = @PostMarketTradingEndTime,                   
      SettlementDaysBuy = @SettlementDaysBuy,                   
      DayLightSaving = @DayLightSaving,                  
     Country = @Country,                   
      StateID = @StateID,                  
      PreMarket = @preMarketCheck,                  
      PostMarket = @postMarketCheck,                  
      RegularTime = @regularTimeCheck,                  
      LunchTime = @lunchTimeCheck,                  
      CurrencyConversion = @currencyConversion,                  
      CountryFlagID = @countryFlagID,                  
      ExchangeLogoID = @LogoID,                  
      TimeZoneOffSet = @TimeZoneOffSet,                  
      SymbolConventionID = @SymbolConventionID,
	  MarketDataProviderExchangeIdentifier = @MarketDataProviderExchangeIdentifier,
      BaseCurrencyID = @BaseCurrencyID,
      OtherCurrencyID = @OtherCurrencyID,                  
      Multiplier = @Multiplier,                  
      IsShortSaleConfirmation = @IsShortSaleConfirmation,                  
      ProvideFundNameWithTrade = @ProvideFundNameWithTrade,                  
      ProvideIdentifierNameWithTrade = @ProvideIdentifierNameWithTrade,                  
      IdentifierID = @IdentifierID,                  
      AssetID = @AssetID,                  
      UnderlyingID = @UnderlyingID,                    
      PurchaseSecFees = @PurchaseSecFees,                  
      SaleSecFees = @SaleSecFees,                  
      PurchaseStamp = @PurchaseStamp,                  
      SaleStamp = @SaleStamp,                  
      PurchaseLevy = @PurchaseLevy,                  
      SaleLevy = @SaleLevy,                  
      SettlementDaysSell = @SettlementDaysSell                
      WHERE AUECID = @auecID                  
                         
    END                     
  END                               
 END                  
 ELSE                  
 BEGIN                  
  SELECT @count = COUNT(*)                  
  FROM T_AUEC                   
  WHERE DisplayName = @DisplayName                  
  IF(@count = 0)                  
  BEGIN                  
    SELECT @count = COUNT(*)                  
    FROM T_AUEC                   
    WHERE ExchangeIdentifier = @ExchangeIdentifier                  
    IF(@count = 0)                  
    BEGIN  
    SELECT @Identity = dbo.F_IsIdentityExists('T_AUEC')
	IF(@Identity = 1)
	SET IDENTITY_INSERT T_AUEC ON                
   --Insert Data                  
    INSERT INTO T_AUEC(AuecID,ExchangeID, FullName, DisplayName,                   
    UnitID, TimeZone, RegularTradingStartTime, RegularTradingEndTime, PreMarketTradingStartTime,                   
    PreMarketTradingEndTime, LunchTimeStartTime, LunchTimeEndTime, PostMarketTradingStartTime,                   
    PostMarketTradingEndTime, SettlementDaysBuy, DayLightSaving, Country, StateID, PreMarket, PostMarket,                  
    RegularTime, LunchTime, CurrencyConversion, CountryFlagID, ExchangeLogoID, TimeZoneOffSet,                   
    SymbolConventionID, ExchangeIdentifier, MarketDataProviderExchangeIdentifier, BaseCurrencyID, OtherCurrencyID, Multiplier,                  
    IsShortSaleConfirmation, ProvideFundNameWithTrade, ProvideIdentifierNameWithTrade, IdentifierID,                  
    AssetID, UnderlyingID, PurchaseSecFees, SaleSecFees, PurchaseStamp, SaleStamp, PurchaseLevy,                  
    SaleLevy,SettlementDaysSell)                  
    VALUES(@auecID,@exchangeID, @FullName, @DisplayName, @Unit, @TimeZone,                   
    @RegularTradingStartTime, @RegularTradingEndTime, @PreMarketTradingStartTime, @PreMarketTradingEndTime,                   
    @LunchTimeStartTime, @LunchTimeEndTime, @PostMarketTradingStartTime, @PostMarketTradingEndTime,                   
    @SettlementDaysBuy, @DayLightSaving, @Country, @StateID, @preMarketCheck, @postMarketCheck,                  
    @regularTimeCheck, @lunchTimeCheck, @currencyConversion, @countryFlagID, @LogoID,                   
    @TimeZoneOffSet, @SymbolConventionID, @ExchangeIdentifier, @MarketDataProviderExchangeIdentifier, @BaseCurrencyID,
	@OtherCurrencyID, @Multiplier, @IsShortSaleConfirmation, @ProvideFundNameWithTrade, @ProvideIdentifierNameWithTrade,                  
    @IdentifierID, @AssetID, @UnderlyingID, @PurchaseSecFees, @SaleSecFees, @PurchaseStamp, @SaleStamp,                  
    @PurchaseLevy, @SaleLevy,@SettlementDaysSell)                  
   END                                
  END                                    
 END 

