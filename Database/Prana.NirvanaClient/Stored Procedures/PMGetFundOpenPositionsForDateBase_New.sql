
CREATE PROCEDURE [dbo].[PMGetFundOpenPositionsForDateBase_New] (
	@ToAllAUECDatesString VARCHAR(MAX)
	,--Optional Parameters...
	@AssetIds VARCHAR(MAX)
	,@FundIds VARCHAR(MAX)
	,@Symbols VARCHAR(MAX)
	,--MUKUL// contains custom conditions in the format "AND(Symbol like 'value1' or Symbol like 'Value2%')"
	@CustomConditions VARCHAR(MAX)
        ,@ReconDateType INT = 1
	)
AS
BEGIN
	DECLARE @Local_ToAllAUECDatesString VARCHAR(MAX)
	DECLARE @Local_AssetIds VARCHAR(MAX)
	DECLARE @Local_FundIds VARCHAR(MAX)
	DECLARE @Local_Symbols VARCHAR(MAX)
	DECLARE @Local_CustomConditions VARCHAR(MAX)
	DECLARE @Local_IsFetchMarkPriceAndExecutedQty BIT

	SET @Local_ToAllAUECDatesString = @ToAllAUECDatesString
	SET @Local_FundIds = @FundIds
	SET @Local_AssetIds = @AssetIds
	SET @Local_Symbols = @Symbols
	SET @Local_CustomConditions = @CustomConditions

	SELECT @Local_IsFetchMarkPriceAndExecutedQty = CAST(PreferenceValue AS BIT)
	FROM T_PranaKeyValuePreferences
	WHERE PreferenceKey = 'ReleaseViewType' --Set @Local_ToAllAUECDatesString = N'0^10/6/2014 12:00:00 AM~1^10/6/2014 5:54:17 AM~15^10/6/2014 5:54:17 AM~16^10/6/2014 5:54:17 AM~20^10/6/2014 5:54:17 AM~32^10/6/2014 5:54:17 AM~81^10/6/2014 5:54:17 AM~84^10/6/2014 5:54:17 AM~86^10/6/2014 5:54:17 AM~89^10/6/2014 5:54:17 AM~99^10/6/2014 5:54:17 AM~100^10/6/2014 5:54:17 AM~'
		--Set @Local_FundIds = N'1184,1185,1190,1183,1182,'
		--Set @Local_AssetIds = N''
		--Set @Local_Symbols = N''
		--Set @Local_CustomConditions = N''

	CREATE TABLE #PositionTable (
		TaxLotID VARCHAR(50)
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
		,Description VARCHAR(MAX)
		,Level2ID INT
		,NotionalValue FLOAT
		,BenchMarkRate FLOAT
		,Differential FLOAT
		,OrigCostBasis FLOAT
		,DayCount INT
		,SwapDescription VARCHAR(MAX)
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
		,MasterFund VARCHAR(MAX)
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
		,--Added UDA columns ,by Omshiv, Nov, 2013
		AssetName VARCHAR(100)
		,SecurityTypeName VARCHAR(200)
		,SectorName VARCHAR(100)
		,SubSectorName VARCHAR(100)
		,CountryName VARCHAR(100)
		,BBGID VARCHAR(20)
		,TransactionType VARCHAR(200)
		,ExecutedQty FLOAT
		,ClosingTaxlotId VARCHAR(50)
		,ReutersSymbol VARCHAR(100)
		,InternalComments VARCHAR(500)
		,SettlCurrency VARCHAR(4)
		,IsCurrencyFuture BIT
		,Symbol_PK BIGINT
		,VenueID INT
		,OrderTypeTagValue VARCHAR(50)
		,FactSetSymbol VARCHAR(100)
		,ActivSymbol VARCHAR(100)
		,BloombergSymbolWithExchangeCode VARCHAR(100)
		,AdditionalTradeAttributes VARCHAR(MAX)
		)

	INSERT INTO #PositionTable
	EXEC P_GetPositions @Local_ToAllAUECDatesString
		,@Local_AssetIds
		,@Local_FundIds
		,@Local_Symbols
                ,@ReconDateType

	IF (@Local_IsFetchMarkPriceAndExecutedQty = 1)
	BEGIN
		BEGIN
			DECLARE @AUECDatesTable TABLE (
				AUECID INT
				,CurrentAUECDate DATETIME
				)

			INSERT INTO @AUECDatesTable
			SELECT *
			FROM dbo.GetAllAUECDatesFromString(@Local_ToAllAUECDatesString)

			DECLARE @UTCDate DATETIME

			SELECT @UTCDate = CurrentAUECDate
			FROM @AUECDatesTable
			WHERE AUECID = 0
		END

		BEGIN
			ALTER TABLE #PositionTable ADD MarkPrice FLOAT NULL

			UPDATE POSITION
			SET POSITION.MarkPrice = IsNull(MP.FinalMarkPrice, 0)
			FROM #PositionTable POSITION
			LEFT OUTER JOIN PM_DayMarkPrice AS MP ON (
					POSITION.FundID = MP.FundID
					AND MP.Symbol = POSITION.Symbol
					AND DateDiff(d, MP.DATE, @UTCDate) = 0
					)
		END
	END

	DECLARE @sqlCommand VARCHAR(MAX)

	SET @sqlCommand = 'SELECT * FROM #PositionTable LEFT OUTER JOIN V_UDA_DynamicUDA ON #PositionTable.Symbol_PK = V_UDA_DynamicUDA.Symbol_PK WHERE 1=1'

	IF (@Local_CustomConditions <> '')
	BEGIN
		SELECT @sqlCommand = @sqlCommand + @Local_CustomConditions

		EXEC (@sqlCommand)
	END
	ELSE
	BEGIN
		SELECT *
		FROM #PositionTable
		LEFT OUTER JOIN V_UDA_DynamicUDA ON #PositionTable.Symbol_PK = V_UDA_DynamicUDA.Symbol_PK
	END

	DROP TABLE #PositionTable
END