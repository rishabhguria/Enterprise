CREATE PROCEDURE [dbo].[P_SaveCompanyAutoGroupingPrefs]
	@CompanyId int,
	@AutoGroup BIT,
	@fundIDs VARCHAR(MAX),
	@TradeAttribute1 BIT,
	@TradeAttribute2 BIT,
	@TradeAttribute3 BIT,
	@TradeAttribute4 BIT,
	@TradeAttribute5 BIT,
	@TradeAttribute6 BIT,
	@Broker BIT,
	@Venue BIT,
	@TradingAC BIT,
	@TradeDate BIT,
	@ProcessDate BIT
AS
		 SELECT Items AS FundID  INTO #Funds
		FROM dbo.Split(@fundIDs, ',')  

		UPDATE T_AutoGroupingPref
		SET AutoGroup = @AutoGroup,
		TradeAttribute1 = @TradeAttribute1,
		TradeAttribute2 = @TradeAttribute2,
		TradeAttribute3 = @TradeAttribute3,
		TradeAttribute4 = @TradeAttribute4,
		TradeAttribute5 = @TradeAttribute5,
		TradeAttribute6 = @TradeAttribute6,
		[Broker]= @Broker,
		Venue = @Venue,
		[TradingAC] = @TradingAC,
		TradeDate = @TradeDate,
		ProcessDate = @ProcessDate
		Where [CompanyId] = @CompanyId

		Update T_AutoGroupingFunds
		SET AutoGroup = 0
		Update T_AutoGroupingFunds
		SET AutoGroup = 1 where FundId in(select FundID from #Funds)
