
/*******************************************************************                                                                      

                                                                  
Author: SURENDRA BISHT       
  
This SP is for Combined use of Yesterday's position + Today's TRANSACTIONS.  

combination of (PMGetFundOpenPositionsForDateBase_New + using P_GetTransactions ) WithCustomConditions

SP Revision used :  ClientDB_1481_PMGetFundOpenPositionsForDateBase_New

Usage:
exec P_GetOpenPositionsAndTransactions @ToAllAUECDatesString=N'0^10/9/2014 12:00:00 AM~1^10/9/2014 9:55:52 AM~15^10/9/2014 9:55:52 AM~',@FromAllAUECDatesString=N'0^10/9/2014 12:00:00 AM~1^10/9/2014 9:55:52 AM~15^10/9/2014 9:55:52 AM~',@AssetIds=N'' ,@FundIds=N'1184,',@Symbols=N'DELL,', @ReconDateType=1 , @CustomConditions=N'AND(Symbol LIKE ''DELL'')'

********************************************************************/
CREATE PROCEDURE [dbo].[P_GetOpenPositionsAndTransactions] (                           
	@AssetIds VARCHAR(MAX)
	,@FundIds VARCHAR(MAX)
	,@StartDate datetime
	,@EndDate datetime                            
	,@ReconDateType INT
	,@Symbols VARCHAR(max)
	,@CustomConditions VARCHAR(MAX)
	)
AS
BEGIN
	DECLARE @Local_AssetIds VARCHAR(max)
	DECLARE @Local_FundIds VARCHAR(max)
	DECLARE @Local_Symbols VARCHAR(max)
	DECLARE @Local_CustomConditions VARCHAR(max)
	DECLARE @Local_IsFetchMarkPriceAndExecutedQty BIT

	SET @Local_FundIds = @FundIds
	SET @Local_AssetIds = @AssetIds
	SET @Local_Symbols = @Symbols
	SET @Local_CustomConditions = @CustomConditions

	SELECT @Local_IsFetchMarkPriceAndExecutedQty = CAST(PreferenceValue AS BIT)
	FROM T_PranaKeyValuePreferences
	WHERE PreferenceKey = 'ReleaseViewType'

	CREATE TABLE #PositionTable (
		 RunDate datetime 
		,TaxLotID VARCHAR(50)
		,AUECLocalDate DATETIME
		,SideID CHAR(1)
		,Symbol VARCHAR(200)
		,OpenQuantity FLOAT
		,AvgPX FLOAT
		,FundID INT
		,AssetID INT
		,UnderLyingID INT
		,ExchangeID INT
		,CurrencyID INT
		,AUECID INT
		,TotalCommissionandFees FLOAT
		,Multiplier FLOAT
		,SettlementDate DATETIME
		,LeadCurrencyID INT
		,VsCurrencyID INT
		,ExpirationDate DATETIME
		,Description VARCHAR(max)
		,Level2ID INT
		,NotionalValue FLOAT
		,BenchMarkRate FLOAT
		,Differential FLOAT
		,OrigCostBasis FLOAT
		,DayCount INT
		,SwapDescription VARCHAR(max)
		,FirstResetDate DATETIME
		,OrigTransDate DATETIME
		,IsSwapped BIT
		,AllocationDate DATETIME
		,GroupID VARCHAR(50)
		,PositionTag INT
		,FXRate FLOAT
		,FXConversionMethodOperator VARCHAR(5)
		,CompanyName VARCHAR(500)
		,UnderlyingSymbol VARCHAR(50)
		,Delta FLOAT
		,PutOrCall VARCHAR(5)
		,IsGrPreAllocated BIT
		,GrCumQty FLOAT
		,GrAllocatedQty FLOAT
		,GrQuantity FLOAT
		,Taxlot_Pk BIGINT
		,ParentRow_Pk BIGINT
		,StrikePrice FLOAT
		,UserID INT
		,CounterPartyID INT
		,CorpActionID UNIQUEIDENTIFIER
		,Coupon FLOAT
		,IssueDate DATETIME
		,MaturityDate DATETIME
		,FirstCouponDate DATETIME
		,CouponFrequencyID INT
		,AccrualBasisID INT
		,BondTypeID INT
		,IsZero BIT
		,ProcessDate DATETIME
		,OriginalPurchaseDate DATETIME
		,IsNDF BIT
		,FixingDate DATETIME
		,IDCOSymbol VARCHAR(50)
		,OSISymbol VARCHAR(50)
		,SEDOLSymbol VARCHAR(50)
		,CUSIPSymbol VARCHAR(50)
		,BloombergSymbol VARCHAR(200)
		,MasterFund VARCHAR(max)
		,UnderlyingDelta FLOAT
		,ISINSymbol VARCHAR(50)
		,LotId VARCHAR(200)
		,ExternalTransId VARCHAR(100)
		,TradeAttribute1 VARCHAR(100)
		,TradeAttribute2 VARCHAR(100)
		,TradeAttribute3 VARCHAR(100)
		,TradeAttribute4 VARCHAR(100)
		,TradeAttribute5 VARCHAR(100)
		,TradeAttribute6 VARCHAR(100)
		,ProxySymbol VARCHAR(100)
		,AssetName VARCHAR(100)
		,SecurityTypeName VARCHAR(200)
		,SectorName VARCHAR(100)
		,SubSectorName VARCHAR(100)
		,CountryName VARCHAR(100)
		,BBGID VARCHAR(20)
		,TransactionType VARCHAR(200)
		,ExecutedQty FLOAT
		,ClosingTaxlotId VARCHAR(50)
		,ReutersSymbol VARCHAR(50)
		,InternalComments VARCHAR(max)
		,SettlCurrency VARCHAR(4)
		,IsCurrencyFuture BIT
		,Symbol_PK BIGINT
		,AdditionalTradeAttributes VARCHAR(MAX)
		)

	Create Table #TransactionTable                                          
		(            
		RunDate Datetime,                                      
		TaxLotID varchar (50),
		TradeDate datetime,   
		OriginalPurchaseDate DATETIME,
		ProcessDate datetime,  
		SideID CHAR(1),
		Symbol VARCHAR(50),
		TaxLotQuantity float,
		AvgPX float, 
		FundID INT,
		AssetID INT,
		UnderLyingID INT,
		ExchangeID INT,
		CurrencyID INT,
		CurrencySymbol varchar(50),
		AUECID INT,
		TotalCommissionandFees FLOAT,
		Multiplier FLOAT,
		SettlementDate DATEtime,
		LeadCurrencyID INT,
		VsCurrencyID INT,
		ExpirationDate DATETIME,
		Description VARCHAR(max),
		Level2ID INT,
		NotionalValue FLOAT,
		BenchMarkRate FLOAT,
		Differential FLOAT,
		OrigCostBasis FLOAT,
		DayCount INT,
		SwapDescription VARCHAR(max),
		FirstResetDate DATETIME,
		OrigTransDate DATETIME,
		IsSwapped BIT ,
		AUECLocalDate DATETIME,
		 GroupID VARCHAR(50),
		PositionTag INT,
		FXRate FLOAT,
		FXConversionMethodOperator VARCHAR(5),
		CompanyName VARCHAR(500),
		UnderlyingSymbol VARCHAR(50),
		Delta FLOAT,
		PutOrCall VARCHAR(5),
		IsGrPreAllocated BIT,
		CumQty FLOAT,
		AllocatedQty FLOAT,
		Quantity float,
		StrikePrice FLOAT,
		UserID INT,
		CounterPartyID INT,
		Coupon FLOAT,
		IssueDate DATETIME,
		MaturityDate DATETIME,
		FirstCouponDate DATETIME,
		CouponFrequencyID INT,
		AccrualBasisID INT,
		BondTypeID INT,
		IsZero int,
		IsNDF BIT,
		FixingDate DATETIME,
		GrossNotionalValue FLOAT,
		NetNotionalValue FLOAT,
		FundName varchar(50),
		Commission float,
		Fees Float,
		ClearingFee float,                                                  
		MiscFees float,                                                  
		StampDuty float,
		IDCO VARCHAR(200),
		OSI VARCHAR(50),
		SEDOL VARCHAR(50),
		CUSIP VARCHAR(50),
		Bloomberg VARCHAR(50),
		Side varchar(20),
		Asset VARCHAR(100),
		CounterParty varchar(20),
		MasterFund VARCHAR(max),
		PrimeBroker varchar(50),
		UnderlyingDelta FLOAT,
		GrossNotionalValueBase FLOAT,
		NetNotionalValueBase Float,
		TotalCommissionandFeesBase FLOAT,
		CommissionBase FLOAT,
		FeeBase Float,
		ClearingFeeBase float, 
		MiscFeesBase float, 
		StampDutyBase float,
		LotId VARCHAR(200),
		ExternalTransId VARCHAR(100),
		TradeAttribute1 VARCHAR(100),
		TradeAttribute2 VARCHAR(100),
		TradeAttribute3 VARCHAR(100),
		TradeAttribute4 VARCHAR(100),
		TradeAttribute5 VARCHAR(100),
		TradeAttribute6 VARCHAR(100),
		ProxySymbol VARCHAR(100),
		AssetName VARCHAR(100),
		SecurityTypeName VARCHAR(200),
		SectorName VARCHAR(100),
		SubSectorName VARCHAR(100),
		CountryName VARCHAR(100),
		SecFee float,
		OccFee float,
		ORFFee float,
		SecFeeBase float,
		OccFeeBase float,
		OrfFeeBase float,
		ClearingBrokerFee float,
		ClearingBrokerFeeBase float,
		SoftCommission float,
		SoftCommissionBase Float,
		TaxOnCommission float,
		TransactionLevy float,
		SideMultiplier int,
		TransactionType VARCHAR(200),
		ReutersSymbol varchar(50),
		SettlCurrency VARCHAR(4),
		AdditionalTradeAttributes VARCHAR(MAX)
		)     

CREATE TABLE #ClosedTable (        
   RunDate datetime        
  ,TaxLotID VARCHAR(50)        
  ,AUECLocalDate DATETIME        
  ,SideID CHAR(1)        
  ,Symbol VARCHAR(200)       
  ,Quantity FLOAT        
  ,OpenQuantity FLOAT        
  ,ClosedQuantity FLOAT        
  ,AvgPX FLOAT        
  ,FundID INT        
  ,AssetID INT        
  ,UnderLyingID INT        
  ,ExchangeID INT        
  ,CurrencyID INT        
  ,AUECID INT        
  ,TotalCommissionandFees FLOAT        
  ,Multiplier FLOAT        
  ,SettlementDate DATETIME        
  ,LeadCurrencyID INT        
  ,VsCurrencyID INT        
  ,ExpirationDate DATETIME        
  ,Description VARCHAR(max)        
  ,Level2ID INT        
  ,NotionalValue FLOAT        
  ,BenchMarkRate FLOAT        
  ,Differential FLOAT        
  ,OrigCostBasis FLOAT        
  ,DayCount INT        
  ,SwapDescription VARCHAR(max)        
  ,FirstResetDate DATETIME        
  ,OrigTransDate DATETIME        
  ,IsSwapped BIT        
  ,AllocationDate DATETIME        
  ,GroupID VARCHAR(50)        
  ,PositionTag INT        
  ,FXRate FLOAT        
  ,FXConversionMethodOperator VARCHAR(5)        
  ,CompanyName VARCHAR(500)        
  ,UnderlyingSymbol VARCHAR(50)        
  ,Delta FLOAT        
  ,PutOrCall VARCHAR(5)        
  ,IsGrPreAllocated BIT        
  ,GrCumQty FLOAT        
  ,GrAllocatedQty FLOAT        
  ,GrQuantity FLOAT        
  ,Taxlot_Pk BIGINT        
  ,ParentRow_Pk BIGINT        
  ,StrikePrice FLOAT        
  ,UserID INT        
  ,CounterPartyID INT        
  ,CorpActionID UNIQUEIDENTIFIER        
  ,Coupon FLOAT        
  ,IssueDate DATETIME        
  ,MaturityDate DATETIME        
  ,FirstCouponDate DATETIME        
  ,CouponFrequencyID INT        
  ,AccrualBasisID INT        
  ,BondTypeID INT        
  ,IsZero BIT        
  ,ProcessDate DATETIME        
  ,OriginalPurchaseDate DATETIME        
  ,IsNDF BIT        
  ,FixingDate DATETIME        
  ,IDCOSymbol VARCHAR(50)        
  ,OSISymbol VARCHAR(50)        
  ,SEDOLSymbol VARCHAR(50)        
  ,CUSIPSymbol VARCHAR(50)        
  ,BloombergSymbol VARCHAR(200)        
  ,MasterFund VARCHAR(max)        
  ,UnderlyingDelta FLOAT        
  ,ISINSymbol VARCHAR(50)        
  ,LotId VARCHAR(200)        
  ,ExternalTransId VARCHAR(100)        
  ,TradeAttribute1 VARCHAR(100)        
  ,TradeAttribute2 VARCHAR(100)        
  ,TradeAttribute3 VARCHAR(100)        
  ,TradeAttribute4 VARCHAR(100)        
  ,TradeAttribute5 VARCHAR(100)        
  ,TradeAttribute6 VARCHAR(100)        
  ,ProxySymbol VARCHAR(100)        
  ,AssetName VARCHAR(100)        
  ,SecurityTypeName VARCHAR(200)        
  ,SectorName VARCHAR(100)        
  ,SubSectorName VARCHAR(100)        
  ,CountryName VARCHAR(100)        
  ,BBGID VARCHAR(20)        
  ,TransactionType VARCHAR(200)        
  ,ExecutedQty FLOAT        
  ,ClosingTaxlotId VARCHAR(50)        
  ,ReutersSymbol VARCHAR(50)        
  ,InternalComments VARCHAR(max)        
  ,SettlCurrency VARCHAR(4)              
  ,IsCurrencyFuture BIT        
  ,Symbol_PK BIGINT        
  ,ClosingTradeDate DATETIME      
  ,ClosingSettlementDate DATETIME      
  ,ClosingStatus INT      
  ,AdditionalTradeAttributes VARCHAR(MAX)
  )      
        
Declare @DateCounter datetime
Set @DateCounter = @StartDate
WHILE (@DateCounter <= @EndDate)
BEGIN
DECLARE @YesterDayDATE DATETIME
SET @YesterDayDATE = DATEADD(DAY,-1,@DateCounter)

	INSERT INTO #PositionTable
EXEC P_GetPositionsForCash @YesterDayDATE 
		,@Local_AssetIds
		,@Local_FundIds
		,@Local_Symbols

INSERT INTO  #TransactionTable
EXEC [P_GetTransactionsWithCustomConditions] @DateCounter 
		,@DateCounter
		,@AssetIds
		,@FundIds
		,@ReconDateType
		,@CustomConditions

 INSERT INTO #ClosedTable        
EXEC P_GetClosedPositionsForCash @DateCounter         
  ,@Local_AssetIds        
  ,@Local_FundIds        
  ,@Local_Symbols        
        
SET @DateCounter = DATEADD(DAY,1,@DateCounter)
END

	IF (@Local_IsFetchMarkPriceAndExecutedQty = 1)
	BEGIN
		BEGIN			
			ALTER TABLE #PositionTable ADD MarkPrice FLOAT NULL

			UPDATE position
			SET position.MarkPrice = IsNull(MP.FinalMarkPrice, 0)
			FROM #PositionTable position
			LEFT OUTER JOIN PM_DayMarkPrice AS MP
				ON (
						position.FundID = MP.FundID
						AND MP.Symbol = position.Symbol
						AND DateDiff(d, MP.DATE, Position.rundate) = 0
						)
		END
	END

	CREATE TABLE #FXConversionRatesForTradeDate (
		FromCurrencyID INT
		,ToCurrencyID INT
		,RateValue FLOAT
		,ConversionMethod INT
		,DATE DATETIME
		,FundID INT
		,eSignalSymbol VARCHAR(max)
		)

	INSERT INTO #FXConversionRatesForTradeDate
	SELECT StanPair.FromCurrencyID AS FromCurrencyID
		,StanPair.ToCurrencyID AS ToCurrencyID
		,CCR.ConversionRate AS RateValue
		,0 AS ConversionMethod
		,CCR.DATE AS DATE
		,CCR.FundID
		,StanPair.eSignalSymbol AS eSignalSymbol
	FROM T_CurrencyConversionRate AS CCR
	INNER JOIN T_CurrencyStandardPairs StanPair ON StanPair.CurrencyPairId = CCR.CurrencyPairID_FK
	WHERE DateDiff(d, @StartDate, CCR.DATE) >= 0	AND DateDiff(d, CCR.DATE, @EndDate) >= 0
	
	UNION
	
	SELECT StanPair.ToCurrencyID AS FromCurrencyID
		,StanPair.FromCurrencyID AS ToCurrencyID
		,CCR.ConversionRate AS RateValue
		,1 AS ConversionMethod
		,CCR.DATE AS DATE
		,CCR.FundID
		,StanPair.eSignalSymbol AS eSignalSymbol
	FROM T_CurrencyConversionRate AS CCR
	INNER JOIN T_CurrencyStandardPairs StanPair ON StanPair.CurrencyPairId = CCR.CurrencyPairID_FK
	WHERE DateDiff(d, @StartDate, CCR.DATE) >= 0	AND DateDiff(d, CCR.DATE, @EndDate) >= 0

	UPDATE #FXConversionRatesForTradeDate
	SET RateValue = 1.0 / RateValue
	WHERE RateValue <> 0
		AND ConversionMethod = 1

	UPDATE #FXConversionRatesForTradeDate
	SET RateValue = 0
	WHERE RateValue IS NULL

	--select * from #FXConversionRatesForTradeDate

	DECLARE @BaseCurrencyID INT
	SELECT @BaseCurrencyID = BaseCurrencyID from T_Company
	
	UPDATE #PositionTable 
	SET FXRate = 0
	
	UPDATE #ClosedTable 
	SET FXRate = 0

	update pt
	set pt.FXRate = ISNULL(FXDayRatesForTradeDate.RateValue,ISNULL(FXDayRatesForTradeDate1.RateValue,0)), 
	pt.FXConversionMethodOperator = 'M'
	from #PositionTable pt 
	LEFT OUTER JOIN #FXConversionRatesForTradeDate FXDayRatesForTradeDate ON (
		FXDayRatesForTradeDate.FromCurrencyID = pt.CurrencyID
		AND FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID
		AND DateDiff(d, pt.RunDate, FXDayRatesForTradeDate.DATE) = 0
		AND FXDayRatesForTradeDate.FundID = pt.FundID
		)
	LEFT OUTER JOIN #FXConversionRatesForTradeDate FXDayRatesForTradeDate1 ON (
		FXDayRatesForTradeDate1.FromCurrencyID = pt.CurrencyID
		AND FXDayRatesForTradeDate1.ToCurrencyID = @BaseCurrencyID
		AND DateDiff(d, pt.RunDate, FXDayRatesForTradeDate1.DATE) = 0
		AND FXDayRatesForTradeDate1.FundID = 0
		)
			
	UPDATE tt
	SET tt.FXRate = ISNULL(FXDayRatesForTradeDate.RateValue,ISNULL(FXDayRatesForTradeDate1.RateValue,0)),
	tt.FXConversionMethodOperator = 'M' 
	FROM #TransactionTable tt
	LEFT OUTER JOIN #FXConversionRatesForTradeDate FXDayRatesForTradeDate ON (
		FXDayRatesForTradeDate.FromCurrencyID = tt.CurrencyID
		AND FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID
		AND DateDiff(d, tt.RunDate, FXDayRatesForTradeDate.DATE) = 0
		AND FXDayRatesForTradeDate.FundID = tt.FundID
		)
	LEFT OUTER JOIN #FXConversionRatesForTradeDate FXDayRatesForTradeDate1 ON (
		FXDayRatesForTradeDate1.FromCurrencyID = tt.CurrencyID
		AND FXDayRatesForTradeDate1.ToCurrencyID = @BaseCurrencyID
		AND DateDiff(d, tt.RunDate, FXDayRatesForTradeDate1.DATE) = 0
		AND FXDayRatesForTradeDate1.FundID = 0
			)
	where tt.FxRate = 0

	UPDATE ct
	SET ct.FXRate = ISNULL(FXDayRatesForTradeDate.RateValue,ISNULL(FXDayRatesForTradeDate1.RateValue,0)),
	ct.FXConversionMethodOperator = 'M' 
	FROM #ClosedTable ct
	LEFT OUTER JOIN #FXConversionRatesForTradeDate FXDayRatesForTradeDate ON (
		FXDayRatesForTradeDate.FromCurrencyID = ct.CurrencyID
		AND FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID
		AND DateDiff(d, ct.RunDate, FXDayRatesForTradeDate.DATE) = 0
		AND FXDayRatesForTradeDate.FundID = ct.FundID
		)
	LEFT OUTER JOIN #FXConversionRatesForTradeDate FXDayRatesForTradeDate1 ON (
		FXDayRatesForTradeDate1.FromCurrencyID = ct.CurrencyID
		AND FXDayRatesForTradeDate1.ToCurrencyID = @BaseCurrencyID
		AND DateDiff(d, ct.RunDate, FXDayRatesForTradeDate1.DATE) = 0
		AND FXDayRatesForTradeDate1.FundID = 0
			)
			
	DECLARE @sqlCommand VARCHAR(MAX)

	SET @sqlCommand = 'SELECT * FROM #PositionTable WHERE 1=1'

	IF (@Local_CustomConditions <> '')
	BEGIN
		SELECT @sqlCommand = @sqlCommand + @Local_CustomConditions
		EXEC (@sqlCommand)
	END
	ELSE
	BEGIN
		SELECT *
		FROM #PositionTable
	END

SELECT *FROM #TransactionTable
      
SELECT *FROM #ClosedTable        
      
	DROP TABLE #PositionTable
	DROP TABLE #TransactionTable
	DROP TABLE #ClosedTable
	DROP TABLE #FXConversionRatesForTradeDate
END
