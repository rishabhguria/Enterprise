-----------------------------------------------------------------
--Created By: Ramesh Verma
--Date: 17-Jan-22
--https://jira.nirvanasolutions.com:8443/browse/PRANA-40095
--Purpose: To be used by GSM to Client SM sync process to add missing AUECs in client DB
-----------------------------------------------------------------
CREATE PROCEDURE [dbo].[P_AddNewAUEC_Wrapper]
	@AssetID int,
	@UnderLyingID int,
	@ExchangeID int,
	@FullName nvarchar(100),
	@DisplayName nvarchar(50),
	@ExchangeIdentifier nvarchar(50),
	@MarketDataProviderExchangeIdentifier nvarchar(50),
	@UnitID int,
	@TimeZone nvarchar(100),
	@TimeZoneOffSet float,
	@PreMarket int,
	@PreMarketTradingStartTime datetime,
	@PreMarketTradingEndTime datetime,
	@RegularTime int,
	@RegularTradingStartTime datetime,
	@RegularTradingEndTime datetime,
	@LunchTime int,
	@LunchTimeStartTime datetime,
	@LunchTimeEndTime datetime,
	@PostMarket int,
	@PostMarketTradingStartTime datetime,
	@PostMarketTradingEndTime datetime,
	@DayLightSaving nvarchar(50),
	@Country int,
	@StateID int,
	@CountryFlagID int,
	@ExchangeLogoID int,
	@CurrencyConversion int,
	@SymbolConventionID int,
	@BaseCurrencyID int,
	@OtherCurrencyID int,
	@Multiplier float,
	@IsShortSaleConfirmation int,
	@ProvideFundNameWithTrade int,
	@ProvideIdentifierNameWithTrade int,
	@IdentifierID int,
	@PurchaseSecFees float,
	@SaleSecFees float,
	@PurchaseStamp float,
	@SaleStamp float,
	@PurchaseLevy float,
	@SaleLevy float,
	@SettlementDaysBuy int,
	@SettlementDaysSell int,
	@roundlot float
AS
	
	DECLARE @AUECID int,
	@result int = -1

	--Save In Client DB
	EXEC P_SaveAUECDetails
  -1,      
  @exchangeID,      
  @FullName,      
  @DisplayName,       
  @UnitID,       
  @TimeZone,       
  @PreMarketTradingStartTime,       
  @PreMarketTradingEndTime,       
  @LunchTimeStartTime,       
  @LunchTimeEndTime,       
  @RegularTradingStartTime,       
  @RegularTradingEndTime,           
  @PostMarketTradingStartTime,       
  @PostMarketTradingEndTime,       
  @SettlementDaysBuy,       
  @DayLightSaving,      
  @Country,       
  @StateID,   
  @PreMarket,      
  @PostMarket,      
  @RegularTime,      
  @LunchTime,      
  @currencyConversion,      
  @countryFlagID,      
  @ExchangeLogoID,      
  @TimeZoneOffSet,      
  @SymbolConventionID,      
  @ExchangeIdentifier,  
  @MarketDataProviderExchangeIdentifier,
  @BaseCurrencyID,      
  @OtherCurrencyID,      
  @Multiplier,      
  @IsShortSaleConfirmation,      
  @ProvideFundNameWithTrade,      
  @ProvideIdentifierNameWithTrade,      
  @IdentifierID,
  @AssetID,      
  @UnderlyingID,
  @PurchaseSecFees,      
  @SaleSecFees,      
  @PurchaseStamp,      
  @SaleStamp,      
  @PurchaseLevy,      
  @SaleLevy,      
  @SettlementDaysSell, 
  @RoundLot,
  @result 

	--get inserted AUECID from client DB
	Select @AUECID = auecid from T_AUEC WHERE ExchangeIdentifier = @ExchangeIdentifier

	IF @AUECID > 0
	BEGIN
		--Insert in SM DB with same AUECID
		EXEC [$(SecurityMaster)].dbo.P_SaveSMAUECDetails              
		  @auecID,                  
		  @exchangeID,                  
		  @FullName,                  
		  @DisplayName,                   
		  @UnitID,                   
		  @TimeZone,                   
		  @PreMarketTradingStartTime,                   
		  @PreMarketTradingEndTime,                   
		  @LunchTimeStartTime,                   
		  @LunchTimeEndTime,                   
		  @RegularTradingStartTime,                   
		  @RegularTradingEndTime,                       
		  @PostMarketTradingStartTime,                   
		  @PostMarketTradingEndTime,                   
		  @SettlementDaysBuy,                   
		  @DayLightSaving,                  
		  @Country,                   
		  @StateID,                      
		  @preMarket,                  
		  @postMarket,                  
		  @regularTime,                  
		  @lunchTime,                  
		  @currencyConversion,                  
		  @countryFlagID,                  
		  @ExchangeLogoID,                  
		  @TimeZoneOffSet,                  
		  @SymbolConventionID,                  
		  @ExchangeIdentifier,
		  @MarketDataProviderExchangeIdentifier,
		  @BaseCurrencyID,                  
		  @OtherCurrencyID,                  
		  @Multiplier,                  
		  @IsShortSaleConfirmation,                  
		  @ProvideFundNameWithTrade,                  
		  @ProvideIdentifierNameWithTrade,                  
		  @IdentifierID,                       
		  @AssetID,                  
		  @UnderlyingID,                  
		  @PurchaseSecFees,                  
		  @SaleSecFees,                  
		  @PurchaseStamp,                  
		  @SaleStamp,                  
		  @PurchaseLevy,                  
		  @SaleLevy,                  
		  @SettlementDaysSell,                
		  @result     	

		--Add CounterPartyVenue mapping for new AUEC
		EXEC P_SaveCVAUECForAUEC @AUECID

		--Add new venue for inserted AUEC
		EXEC P_SaveVenueDetailsFor
			@DisplayName,
			1,
			@FullName,
			@ExchangeID
		
		--Add weekends as holidays for the new AUEC
		Insert Into T_AUECWeeklyHolidays(WeeklyHolidayID,AUECID)VALUES (0,@AUECID)
		Insert Into T_AUECWeeklyHolidays(WeeklyHolidayID,AUECID)VALUES (6,@AUECID)
		SET @result = 0
	END

RETURN @result
