-----------------------------------------------------------------
--Created By: sachin mishra
--Date: 12-Feb-15
--http://jira.nirvanasolutions.com:8080/browse/CHMW-2547
--Purpose: AUEC is not updating on user setup permission 
-----------------------------------------------------------------

CREATE PROCEDURE [dbo].[P_SaveAUECDetails] (      
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
  @RoundLot decimal(36,19),   
     
  @result int      
 )      
AS      
 Declare @total int      
 set @total = 0      
 declare @count int      
 set @count = 0      
       
 Select @total = Count(*)      
 From T_AUEC      
 Where AUECID = @auecID 
     
    
 if(@total > 0)      
 Begin      
  select @count = count(*)      
  from T_AUEC      
  Where DisplayName = @DisplayName AND AUECID <> @auecID      
  if(@count = 0)      
  begin      
    select @count = count(*)      
    from T_AUEC       
    Where ExchangeIdentifier = @ExchangeIdentifier AND AUECID <> @auecID      
    if(@count = 0)      
    begin      
    --Update Table      
     Update T_AUEC      
     Set FullName = @FullName,       
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
      ExchangeIdentifier = @ExchangeIdentifier,  
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
      SettlementDaysSell = @SettlementDaysSell,
	  RoundLot=@RoundLot    
      
     Where AUECID = @auecID      
      Select @result = @auecID      
    end      
    else      
    begin      
     Select @result = -2      
 end      
  end      
  else      
  begin      
   Select @result = -1      
  end      
 end      
 else      
 Begin      
  select @count = count(*)      
  from T_AUEC       
  Where DisplayName = @DisplayName      
  if(@count = 0)      
  begin      
    select @count = count(*)      
    from T_AUEC       
    Where ExchangeIdentifier = @ExchangeIdentifier      
    if(@count = 0)      
    begin      
   --Insert Data      
    INSERT INTO T_AUEC(ExchangeID, FullName, DisplayName,       
    UnitID, TimeZone, RegularTradingStartTime, RegularTradingEndTime, PreMarketTradingStartTime,       
    PreMarketTradingEndTime, LunchTimeStartTime, LunchTimeEndTime, PostMarketTradingStartTime,       
    PostMarketTradingEndTime, SettlementDaysBuy, DayLightSaving, Country, StateID, PreMarket, PostMarket,      
    RegularTime, LunchTime, CurrencyConversion, CountryFlagID, ExchangeLogoID, TimeZoneOffSet,       
    SymbolConventionID, ExchangeIdentifier, MarketDataProviderExchangeIdentifier, BaseCurrencyID, OtherCurrencyID, Multiplier,      
    IsShortSaleConfirmation, ProvideFundNameWithTrade, ProvideIdentifierNameWithTrade, IdentifierID,      
    AssetID, UnderlyingID, PurchaseSecFees, SaleSecFees, PurchaseStamp, SaleStamp, PurchaseLevy,      
    SaleLevy,SettlementDaysSell,RoundLot)      
   Values(@exchangeID, @FullName, @DisplayName, @Unit, @TimeZone,       
    @RegularTradingStartTime, @RegularTradingEndTime, @PreMarketTradingStartTime, @PreMarketTradingEndTime,       
    @LunchTimeStartTime, @LunchTimeEndTime, @PostMarketTradingStartTime, @PostMarketTradingEndTime,       
    @SettlementDaysBuy, @DayLightSaving, @Country, @StateID, @preMarketCheck, @postMarketCheck,      
    @regularTimeCheck, @lunchTimeCheck, @currencyConversion, @countryFlagID, @LogoID,       
    @TimeZoneOffSet, @SymbolConventionID, @ExchangeIdentifier, @MarketDataProviderExchangeIdentifier, @BaseCurrencyID,
	@OtherCurrencyID, @Multiplier, @IsShortSaleConfirmation, @ProvideFundNameWithTrade, @ProvideIdentifierNameWithTrade,      
    @IdentifierID, @AssetID, @UnderlyingID, @PurchaseSecFees, @SaleSecFees, @PurchaseStamp, @SaleStamp,      
    @PurchaseLevy, @SaleLevy,@SettlementDaysSell,@RoundLot)      
          
    Set @result = scope_identity() 
    Insert into T_PMClearanceTimes(AUECID,ClearanceTime)
    Values(@result,GetDate())

     --Insert newly added AUEC to T_AUECMapping table
     INSERT INTO T_AUECMapping ([AUECID],[ExchangeIdentifier],[ExchangeToken],[PSRootToken],[PSFormatString],[TranslateRoot],[TranslateType],[EsignalRootToken],[BloombergRootToken])
     VALUES (@result, @ExchangeIdentifier, '-', '{space}', '{Symbol}', 'False', 'False','{space}','{space}')

 Declare @ReleaseType int
 Select @Releasetype = CONVERT(int,Preferencevalue) from T_Pranakeyvaluepreferences where preferencekey = 'releaseviewtype'
	IF 	 @Releasetype = 1
	BEGIN
		IF (select  count(*)      
		from T_CompanyAUEC      
		Where CompanyID = -1 AND AUECID = @result) = 0
		BEGIN
			Insert into T_CompanyAUEC(CompanyID,AUECID)
			VALUES(-1,@result)
		END
	END  	 
   end      
          
      
   else      
    begin      
     Select @result = -2      
    end      
  end      
  else      
  begin      
   Select @result = -1      
  end       
         
 end       
select @result 
