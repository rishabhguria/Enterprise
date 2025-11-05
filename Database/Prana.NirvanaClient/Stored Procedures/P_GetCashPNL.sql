
CREATE PROCEDURE [dbo].[P_GetCashPNL] (
	@FromDate DATETIME
	,@ToDate DATETIME
	,@Symbols VARCHAR(max)
	,@fundIDs VARCHAR(max)
	,@userID INT = NULL
	,@isincludecommission BIT = 0
	,@isIncludeFXPNLonSwapForDiffSettleAndBaseCurr BIT = 1
	,@isIncludeFXPNLonSwapForSameSettleAndBaseCurr BIT = 1
	,@TotalEntries INT
	,@RowNumber INT
	,@MinTradeDate DATETIME
	,@IsManualRevaluation BIT
	,@EndDate DATETIME
	,@OriginalFundIDs VARCHAR(max)
	)
AS

--Declare @FromDate DATETIME
--	,@ToDate DATETIME
--	,@Symbols VARCHAR(200)
--	,@fundIDs VARCHAR(max)
--	,@userID INT = NULL
--	,@isincludecommission BIT = 0
--	,@isIncludeFXPNLonSwapForDiffSettleAndBaseCurr BIT = 1
--	,@isIncludeFXPNLonSwapForSameSettleAndBaseCurr BIT = 1
--	,@TotalEntries INT
--	,@RowNumber INT
--	,@MinTradeDate DATETIME
--	,@IsManualRevaluation BIT
--	,@EndDate DATETIME
--	,@OriginalFundIDs VARCHAR(max)


BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON;

	-- added by:Kashish,
	-- Variable for calculating commission for swaps
	BEGIN TRY
		DECLARE @IsSwappedPrefTrue BIT

		----------------Log in trade Server logs, if @LogInFile=1 then it start logging, 0 for not ------------------- 
        DECLARE @LogInFile int
		set  @LogInFile =0

		--------------------------------------------------------------------------------------------------------------
		SELECT @IsSwappedPrefTrue = IsChecked
		FROM T_CashPreferencesforCommission
		WHERE AssetClass = 'EquitySwap'

		--added by: Bharat Raturi, 25 sep 2014  
		--Create fund Table of the IDs provided
		DECLARE @Step INT
		DECLARE @BaseCurrId INT

		CREATE TABLE #Funds (FundID INT)

		INSERT INTO #Funds
		SELECT Items AS FundID
		FROM dbo.Split(@FundIDs, ',')

		CREATE TABLE #OriginalFunds (OriginalFundID INT)

		INSERT INTO #OriginalFunds
		SELECT Items AS OriginalFundID
		FROM dbo.Split(@OriginalFundIDs, ',')

		CREATE TABLE #Symbols (Symbol VARCHAR(200))

		CREATE TABLE #Symbols_New (Symbol VARCHAR(200))

		SELECT @BaseCurrId = BaseCurrencyID
		FROM T_Company

		IF (@Symbols <> 'ALL_NIRVSYMBOL')
		BEGIN
			INSERT INTO #Symbols
			--VALUES (@Symbols)
            SELECT Items AS Symbol
		    FROM dbo.Split(@Symbols, ',')
			
			SELECT DISTINCT CurrencyID
			INTO #CurrID
			FROM T_Group
			WHERE Symbol IN (
					SELECT Symbol
					FROM #Symbols
					)
				AND CurrencyID <> @BaseCurrId

			INSERT INTO #Symbols_New
			SELECT DISTINCT G.Symbol
			FROM T_Group G
			INNER JOIN #CurrID C ON G.CurrencyID = C.CurrencyID
			WHERE DATEDIFF(d, aueclocaldate, @toDate) >= 0

			INSERT INTO #Symbols_New
			SELECT DISTINCT PT.Symbol
			FROM PM_TaxlotClosing PTC  WITH (NOLOCK)
			INNER JOIN PM_Taxlots PT WITH (NOLOCK) ON (
					PTC.PositionalTaxlotID = PT.TaxlotID
					AND PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk
					)
			INNER JOIN PM_Taxlots PT1 WITH (NOLOCK) ON (
					PTC.ClosingTaxlotID = PT1.TaxlotID
					AND PTC.TaxLotClosingId = PT1.TaxLotClosingId_Fk
					)
			INNER JOIN #Funds fnd ON PT1.[FundID] = fnd.FundID
			INNER JOIN T_Group G WITH (NOLOCK) ON G.GroupID = PT.GroupID
			INNER JOIN #CurrID C ON G.CurrencyID = C.CurrencyID
			WHERE DATEDIFF(D, @FromDate, PTC.AUECLocalDate) >= 0
				AND DATEDIFF(D, PTC.AUECLocalDate, @ToDate) >= 0
				--AND DATEDIFF(D, PTC.AUECLocalDate, PT1.AUECModifiedDate) = 0

			INSERT INTO #Symbols
			SELECT DISTINCT symbol
			FROM #Symbols_New
			WHERE symbol NOT IN (
					SELECT Symbol
					FROM #Symbols
					)
		END

		SELECT *
		INTO #T_ActivityType
		FROM T_ActivityType WITH (NOLOCK)

		-- 1. Kuldeep:- Prepared open positions separately and then used this in generating unrealized pnl entries.
		SELECT @Step = (@RowNumber - 1) * 6 + 1

		declare @FromDateText varchar(30)
        set @FromDateText = cast(@FromDate as varchar)

        declare @ToDateText varchar(30)
        set @ToDateText = cast(@ToDate as varchar)

		RAISERROR (
				N'Step %i/%i: Fetching Open Positions for Selected Funds.$%i$%i$Funds:%s,Date: %s,To:%s,Symbols:%s'
				,0
				,1
				,@Step
				,@TotalEntries
				,@userID
				,@LogInFile
				,@fundIDs
				,@FromDateText
				,@ToDateText
				,@Symbols
				)
		WITH NOWAIT

		CREATE TABLE #OpenPositions (
			RunDate DATETIME
			,Symbol NVARCHAR(200)
			,TradeDate DATETIME
			,OrderSideTagValue NCHAR(10)
			,AvgPrice FLOAT
			,TaxLotOpenQty FLOAT
			,FXRate FLOAT
			,FXConversionMethodOperator VARCHAR(3)
			,OpenTotalCommissionAndFees FLOAT
			,TaxlotID VARCHAR(50)
			,GroupID NVARCHAR(50)
			,FundID INT
			,Level2ID INT
			,IsSwapped BIT
			,SettlCurrency INT
			)

		DECLARE @DateCounter DATETIME

		SET @DateCounter = @FromDate
		
		Create Table #PM_TaxlotsSelectedData
		(
			Taxlot_PK BigInt,
			FundID Int,
			Symbol Varchar(200),
			TaxlotID Varchar(50),
			AUECModifiedDate DateTime,
			OrderSideTagValue Varchar(20),
			AvgPrice Float,
			TaxLotOpenQty Float,
			FXRate Float,
			FXConversionMethodOperator Varchar(10),
			OpenTotalCommissionandFees Float,
			GroupID Varchar(50),
			Level2ID Int,
			SettlCurrency Varchar(20),
			AuecLocalDate DateTime,
			IsSwapped Bit,
			CurrencyID Int,
			AssetID Int
			)

		IF (@Symbols <> 'ALL_NIRVSYMBOL')
		BEGIN
			Insert InTo #PM_TaxlotsSelectedData
			SELECT 
			Taxlot_PK,
			PT.FundID as FundID,
			PT.Symbol,
			TaxlotID,
			AUECModifiedDate,
			PT.OrderSideTagValue,
			PT.AvgPrice,
			TaxLotOpenQty,
			PT.FXRate,
			PT.FXConversionMethodOperator,
			OpenTotalCommissionandFees,
			pt.GroupID,
			Level2ID,
			PT.SettlCurrency,
			G.AuecLocalDate,
			G.IsSwapped,
			G.CurrencyID,
			G.AssetID
			FROM PM_Taxlots PT  WITH (NOLOCK)
			INNER JOIN #Funds fnd ON PT.FundID = fnd.FundID
			INNER JOIN T_Group G WITH (NOLOCK) ON G.GroupID = PT.GroupID
			WHERE DATEDIFF(D, PT.AUECModifiedDate, @EndDate) >= 0
			AND PT.Symbol IN (SELECT Symbol FROM #Symbols)
										
		END
		Else
			BEGIN
			Insert InTo #PM_TaxlotsSelectedData
			SELECT 
			Taxlot_PK,
			PT.FundID as FundID,
			PT.Symbol,
			TaxlotID,
			AUECModifiedDate,
			PT.OrderSideTagValue,
			PT.AvgPrice,
			TaxLotOpenQty,
			PT.FXRate,
			PT.FXConversionMethodOperator,
			OpenTotalCommissionandFees,
			pt.GroupID,
			Level2ID,
			PT.SettlCurrency,
			G.AuecLocalDate,
			G.IsSwapped,
			G.CurrencyID,
			G.AssetID			
			FROM PM_Taxlots PT  WITH (NOLOCK)
			INNER JOIN #Funds fnd ON PT.FundID = fnd.FundID
			INNER JOIN T_Group G WITH (NOLOCK) ON G.GroupID = PT.GroupID
			WHERE DATEDIFF(D, PT.AUECModifiedDate, @EndDate) >= 0
		
		END

		Select TickerSymbol,LeadCurrencyID  
		Into #symbolInfo 
		From V_SecMasterData 
		Where TickerSymbol In (Select Distinct Symbol From #PM_TaxlotsSelectedData)

		WHILE (DATEDIFF(D, @DateCounter, @ToDate) >= 0)
		BEGIN
		
		SELECT MAX(Taxlot_PK) As Taxlot_PK
		InTo #TempTaxlotPK
					FROM #PM_TaxlotsSelectedData PT
					INNER JOIN #Funds fnd ON PT.FundID = fnd.FundID
					WHERE DATEDIFF(D, PT.AUECModifiedDate, @DateCounter) >= 0
                    GROUP BY TaxlotID


			INSERT INTO #OpenPositions
			SELECT @DateCounter AS Rundate
				,PT.Symbol
				,PT.AuecLocalDate
				,PT.OrderSideTagValue
				,CASE 
					WHEN (A.AssetName='FX' OR A.AssetName='FXForward') AND PT.CurrencyID <> S.LeadCurrencyID AND PT.AvgPrice<>0
						THEN 1/PT.AvgPrice
					ELSE
						PT.AvgPrice
					END AS AvgPrice
				,PT.TaxLotOpenQty
				,CASE 
					WHEN (A.AssetName='FX' OR A.AssetName='FXForward') AND PT.CurrencyID <> 1
						THEN 1
					ELSE
						PT.FXRate
					END AS FXRate
				,PT.FXConversionMethodOperator
				,CASE 
					WHEN CP.IsChecked = 1
						OR PT.IsSwapped = 1
						THEN ISNULL(PT.OpenTotalCommissionandFees, 0)
					ELSE 0
					END AS OpenTotalCommissionAndFees
				,PT.TaxLotID
				,PT.GroupID
				,PT.FundID
				,PT.Level2ID
				,PT.IsSwapped
				,PT.SettlCurrency
			FROM #PM_TaxlotsSelectedData PT
            Inner Join #TempTaxlotPK TPK On TPK.Taxlot_PK = PT.Taxlot_PK
			INNER JOIN #symbolInfo S ON PT.Symbol=S.TickerSymbol
			--INNER JOIN T_Group G ON G.GroupID = PT.GroupID
			INNER JOIN T_Asset A ON PT.AssetID = A.AssetID
			INNER JOIN T_CashPreferencesforCommission CP ON A.AssetName = CP.AssetClass
			WHERE TaxLotOpenQty <> 0

			Drop Table #TempTaxlotPK

			IF (@IsSwappedPrefTrue = 0)
			BEGIN
				UPDATE #OpenPositions
				SET OpenTotalCommissionAndFees = 0
				WHERE IsSwapped = 1
			END

			SET @DateCounter = DATEADD(DAY, 1, @DateCounter)
		END

		IF (@Symbols = 'ALL_NIRVSYMBOL')
		BEGIN
			INSERT INTO #Symbols
			SELECT DISTINCT Symbol
			FROM #OpenPositions
			
			UNION
			
			SELECT DISTINCT Symbol
			FROM T_TradeAudit TA WITH (NOLOCK)
			INNER JOIN #Funds fund ON fund.FundID = TA.FundID
			WHERE IsProcessed = 0
				AND TA.Comment IN (
					'Trade Reallocated Taxlots Deleted'
					,'Group Unallocated Taxlots Deleted'
					)

			INSERT INTO #Symbols
			SELECT DISTINCT PT.Symbol
			FROM PM_TaxlotClosing PTC WITH (NOLOCK)
			INNER JOIN PM_Taxlots PT  WITH (NOLOCK)ON (
					PTC.PositionalTaxlotID = PT.TaxlotID
					AND PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk
					)
			INNER JOIN PM_Taxlots PT1  WITH (NOLOCK) ON (
					PTC.ClosingTaxlotID = PT1.TaxlotID
					AND PTC.TaxLotClosingId = PT1.TaxLotClosingId_Fk
					)
			INNER JOIN #Funds fnd ON PT1.[FundID] = fnd.FundID
			WHERE DATEDIFF(D, @FromDate, PTC.AUECLocalDate) >= 0
				AND DATEDIFF(D, PTC.AUECLocalDate, @ToDate) >= 0
				--AND DATEDIFF(D, PTC.AUECLocalDate, PT1.AUECModifiedDate) = 0
		END

		RAISERROR (
				N'Fetched Open Positions for Selected Funds.$%i$%i$Funds:%s'
				,0
				,1
				,@userID
				,@LogInFile
				,@fundIDs
				,@Symbols
				)
		WITH NOWAIT

		BEGIN
			-----------------------------------------------------Id Section----------------------------------------------------------  
			DECLARE @EquityLongRealizedPNLActivityId INT
			DECLARE @EquityLongRealizedPNLSTActivityId INT
			DECLARE @EquityLongRealizedPNLLTActivityId INT
			DECLARE @EquityShortRealizedPNLActivityId INT
			DECLARE @EquityShortRealizedPNLSTActivityId INT
			DECLARE @EquityOptionLongRealizedPNLLTActivityId INT
			DECLARE @EquityOptionLongRealizedPNLSTActivityId INT
			DECLARE @EquityOptionShortRealizedPNLSTActivityId INT
			DECLARE @EquitySwapLongRealizedPNLLTActivityId INT
			DECLARE @EquitySwapLongRealizedPNLSTActivityId INT
			DECLARE @EquitySwapShortRealizedPNLSTActvityId INT
			DECLARE @BondLongRealizedPNLLTActivityId INT
			DECLARE @BondLongRealizedPNLSTActivityId INT
			DECLARE @BondShortRealizedPNLSTActivityId INT
			DECLARE @FutureOptionLongRealizedPNLLTActivityId INT
			DECLARE @FutureOptionLongRealizedPNLSTActivityId INT
			DECLARE @FutureOptionShortRealizedPNLSTActivityId INT
			DECLARE @FXLongRealizedPNLLTActivityId INT
			DECLARE @FXLongRealizedPNLSTActivityId INT
			DECLARE @FXShortRealizedPNLSTActivityId INT
			DECLARE @FXForwardLongRealizedPNLLTActivityId INT
			DECLARE @FXForwardLongRealizedPNLSTActivityId INT
			DECLARE @FXForwardShortRealizedPNLSTActivityId INT
			DECLARE @PrivateEquityLongRealizedPNLLTActivityId INT
			DECLARE @PrivateEquityLongRealizedPNLSTActivityId INT
			DECLARE @PrivateEquityShortRealizedPNLSTActivityId INT
			DECLARE @EquityOptionLongRealizedPNLActivityId INT
			DECLARE @EquityOptionShortRealizedPNLActivityId INT
			DECLARE @FutureRealizedPNLActivityId INT
			DECLARE @FutureLongRealizedPNLLTActivityId INT
			DECLARE @FutureLongRealizedPNLSTActivityId INT
			DECLARE @FutureShortRealizedPNLLTActivityId INT
			DECLARE @FutureShortRealizedPNLSTActivityId INT
			DECLARE @FutureOptionLongRealizedPNLActivityId INT
			DECLARE @FutureOptionShortRealizedPNLActivityId INT
			DECLARE @BondLongRealizedPNLActivityId INT
			DECLARE @BondShortRealizedPNLActivityId INT
			DECLARE @FXLongRealizedPNLActivityId INT
			DECLARE @FXShortRealizedPNLActivityId INT
			DECLARE @PrivateEquityLongRealizedPNLActivityId INT
			DECLARE @PrivateEquityShortRealizedPNLActivityId INT
			DECLARE @CDSLongRealizedPNLActivityId INT
			DECLARE @CDSShortRealizedPNLActivityId INT
			DECLARE @ConvertibleBondLongRealizedPNLActivityId INT
			DECLARE @ConvertibleBondShortRealizedPNLActivityId INT
			DECLARE @FXForwardLongRealizedPNLActivityId INT
			DECLARE @FXforwardShortRealizedPNLActivityId INT
			DECLARE @EquityLongAdditionRealizedPNLActivityId INT
			DECLARE @EquityShortAdditionRealizedPNLActivityId INT
			DECLARE @EquityLongWithdrawalRealizedPNLActivityId INT
			DECLARE @EquityShortWithdrawalRealizedPNLActivityId INT
			DECLARE @EquityOptionLongAdditionRealizedPNLActivityId INT
			DECLARE @EquityOptionShortAdditionRealizedPNLActivityId INT
			DECLARE @EquityOptionLongWithdrawalRealizedPNLActivityId INT
			DECLARE @EquityOptionShortWithdrawalRealizedPNLActivityId INT
			DECLARE @FutureLongAdditionRealizedPNLActivityId INT
			DECLARE @FutureShortAdditionRealizedPNLActivityId INT
			DECLARE @FutureLongWithdrawalRealizedPNLActivityId INT
			DECLARE @FutureShortWithdrawalRealizedPNLActivityId INT
			DECLARE @FutureOptionLongAdditionRealizedPNLActivityId INT
			DECLARE @FutureOptionShortAdditionRealizedPNLActivityId INT
			DECLARE @FutureOptionLongWithdrawalRealizedPNLActivityId INT
			DECLARE @FutureOptionShortWithdrawalRealizedPNLActivityId INT
			DECLARE @FXLongAdditionRealizedPNLActivityId INT
			DECLARE @FXShortAdditionRealizedPNLActivityId INT
			DECLARE @FXLongWithdrawalRealizedPNLActivityId INT
			DECLARE @FXShortWithdrawalRealizedPNLActivityId INT
			DECLARE @BondLongAdditionRealizedPNLActivityId INT
			DECLARE @BondShortAdditionRealizedPNLActivityId INT
			DECLARE @BondLongWithdrawalRealizedPNLActivityId INT
			DECLARE @BondShortWithdrawalRealizedPNLActivityId INT
			DECLARE @PrivateEquityLongAdditionRealizedPNLActivityId INT
			DECLARE @PrivateEquityShortAdditionRealizedPNLActivityId INT
			DECLARE @PrivateEquityLongWithdrawalRealizedPNLActivityId INT
			DECLARE @PrivateEquityShortWithdrawalRealizedPNLActivityId INT
			DECLARE @FXForwardLongAdditionRealizedPNLActivityId INT
			DECLARE @FXForwardShortAdditionRealizedPNLActivityId INT
			DECLARE @FXForwardLongWithdrawalRealizedPNLActivityId INT
			DECLARE @FXForwardShortWithdrawalRealizedPNLActivityId INT
			DECLARE @ConvertibleBondLongAdditionRealizedPNLActivityId INT
			DECLARE @ConvertibleBondShortAdditionRealizedPNLActivityId INT
			DECLARE @ConvertibleBondLongWithdrawalRealizedPNLActivityId INT
			DECLARE @ConvertibleBondShortWithdrawalRealizedPNLActivityId INT
			DECLARE @CDSLongAdditionRealizedPNLActivityId INT
			DECLARE @CDSShortAdditionRealizedPNLActivityId INT
			DECLARE @CDSLongWithdrawalRealizedPNLActivityId INT
			DECLARE @CDSShortWithdrawalRealizedPNLActivityId INT
			DECLARE @EquitySwapLongAdditionRealizedPNLActivityId INT
			DECLARE @EquitySwapShortAdditionRealizedPNLActivityId INT
			DECLARE @EquitySwapLongWithdrawalRealizedPNLActivityId INT
			DECLARE @EquitySwapShortWithdrawalRealizedPNLActivityId INT
			DECLARE @EquitySwapRealizedPNLActivityId INT

			SELECT @EquityLongRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityLongRealizedPNL'

  			SELECT @EquityLongRealizedPNLSTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityLongRealizedPNL (ST)'

			SELECT @EquityLongRealizedPNLLTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityLongRealizedPNL (LT)'

			SELECT @EquityShortRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityShortRealizedPNL'

			SELECT @EquityShortRealizedPNLSTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityShortRealizedPNL (ST)'

			SELECT @EquitySwapRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquitySwapRealizedPNL'

			SELECT @EquitySwapLongRealizedPNLLTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquitySwapLongRealizedPNL (LT)'

			SELECT @EquitySwapLongRealizedPNLSTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquitySwapLongRealizedPNL (ST)'

			SELECT @EquitySwapShortRealizedPNLSTActvityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquitySwapShortRealizedPNL (ST)'

			SELECT @EquityOptionLongRealizedPNLLTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityOptionLongRealizedPNL (LT)'

			SELECT @EquityOptionLongRealizedPNLSTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityOptionLongRealizedPNL (ST)'

			SELECT @EquityOptionShortRealizedPNLSTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityOptionShortRealizedPNL (ST)'

			SELECT @BondLongRealizedPNLLTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'BondLongRealizedPNL (LT)'

			SELECT @BondLongRealizedPNLSTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'BondLongRealizedPNL (ST)'

			SELECT @BondShortRealizedPNLSTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'BondShortRealizedPNL (ST)'

			SELECT @FutureOptionLongRealizedPNLLTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureOptionLongRealizedPNL (LT)'

			SELECT @FutureOptionLongRealizedPNLSTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureOptionLongRealizedPNL (ST)'

			SELECT @FutureOptionShortRealizedPNLSTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureOptionShortRealizedPNL (ST)'

			SELECT @FXLongRealizedPNLLTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXLongRealizedPNL (LT)'

			SELECT @FXLongRealizedPNLSTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXLongRealizedPNL (ST)'

			SELECT @FXShortRealizedPNLSTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXShortRealizedPNL (ST)'

			SELECT @FXForwardLongRealizedPNLLTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXForwardLongRealizedPNLActivityId (LT)'

			SELECT @FXForwardLongRealizedPNLSTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXForwardLongRealizedPNLActivityId (ST)'

			SELECT @FXForwardShortRealizedPNLSTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXForwardShortRealizedPNLActivityId (ST)'

			SELECT @PrivateEquityLongRealizedPNLLTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'PrivateEquityLongRealizedPNL (LT)'

			SELECT @PrivateEquityLongRealizedPNLSTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'PrivateEquityLongRealizedPNL (ST)'

			SELECT @PrivateEquityShortRealizedPNLSTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'PrivateEquityShortRealizedPNL (ST)'

			SELECT @EquityOptionLongRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityOptionLongRealizedPNL'

			SELECT @EquityOptionShortRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityOptionShortRealizedPNL'

			SELECT @FutureRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureRealizedPNL'

			SELECT @FutureLongRealizedPNLLTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureLongRealizedPNL (LT)'

			SELECT @FutureLongRealizedPNLSTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureLongRealizedPNL (ST)'

			SELECT @FutureShortRealizedPNLLTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureShortRealizedPNL (LT)'

			SELECT @FutureShortRealizedPNLSTActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureShortRealizedPNL (ST)'

			SELECT @FutureOptionLongRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureOptionLongRealizedPNL'

			SELECT @FutureOptionShortRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureOptionShortRealizedPNL'

			SELECT @BondLongRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'BondLongRealizedPNL'

			SELECT @BondShortRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'BondShortRealizedPNL'

			SELECT @FXLongRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXLongRealizedPNL'

			SELECT @FXShortRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXShortRealizedPNL'

			SELECT @PrivateEquityLongRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'PrivateEquityLongRealizedPNL'

			SELECT @PrivateEquityShortRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'PrivateEquityShortRealizedPNL'

			SELECT @CDSLongRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'CDSLongRealizedPNL'

			SELECT @CDSShortRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'CDSShortRealizedPNL'

			SELECT @ConvertibleBondLongRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'ConvertibleBondLongRealizedPNL'

			SELECT @ConvertibleBondShortRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'ConvertibleBondShortRealizedPNL'

			SELECT @FXForwardLongRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXForwardLongRealizedPNLActivityId'

			SELECT @FXforwardShortRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXforwardShortRealizedPNLActivityId'

			--------------------------------------------------- New Realized PNL ----------------------------------------------------------------------
			SELECT @EquityLongAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityLongAdditionRealizedPNL'

			SELECT @EquityShortAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityShortAdditionRealizedPNL'

			SELECT @EquityLongWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityLongWithdrawalRealizedPNL'

			SELECT @EquityShortWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityShortWithdrawalRealizedPNL'

			SELECT @EquityOptionLongAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityOptionLongAdditionRealizedPNL'

			SELECT @EquityOptionShortAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityOptionShortAdditionRealizedPNL'

			SELECT @EquityOptionLongWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityOptionLongWithdrawalRealizedPNL'

			SELECT @EquityOptionShortWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityOptionShortWithdrawalRealizedPNL'

			SELECT @FutureLongAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureLongAdditionRealizedPNL'

			SELECT @FutureShortAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureShortAdditionRealizedPNL'

			SELECT @FutureLongWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureLongWithdrawalRealizedPNL'

			SELECT @FutureShortWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureShortWithdrawalRealizedPNL'

			SELECT @FutureOptionLongAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureOptionLongAdditionRealizedPNL'

			SELECT @FutureOptionShortAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureOptionShortAdditionRealizedPNL'

			SELECT @FutureOptionLongWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureOptionLongWithdrawalRealizedPNL'

			SELECT @FutureOptionShortWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureOptionShortWithdrawalRealizedPNL'

			SELECT @FXLongAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXLongAdditionRealizedPNL'

			SELECT @FXShortAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXShortAdditionRealizedPNL'

			SELECT @FXLongWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXLongWithdrawalRealizedPNL'

			SELECT @FXShortWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXShortWithdrawalRealizedPNL'

			SELECT @BondLongAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'BondLongAdditionRealizedPNL'

			SELECT @BondShortAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'BondShortAdditionRealizedPNL'

			SELECT @BondLongWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'BondLongWithdrawalRealizedPNL'

			SELECT @BondShortWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'BondShortWithdrawalRealizedPNL'

			SELECT @PrivateEquityLongAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'PrivateEquityLongAdditionRealizedPNL'

			SELECT @PrivateEquityShortAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'PrivateEquityShortAdditionRealizedPNL'

			SELECT @PrivateEquityLongWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'PrivateEquityLongWithdrawalRealizedPNL'

			SELECT @PrivateEquityShortWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'PrivateEquityShortWithdrawalRealizedPNL'

			SELECT @FXForwardLongAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXForwardLongAdditionRealizedPNL'

			SELECT @FXForwardShortAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXForwardShortAdditionRealizedPNL'

			SELECT @FXForwardLongWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXForwardLongWithdrawalRealizedPNL'

			SELECT @FXForwardShortWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXForwardShortWithdrawalRealizedPNL'

			SELECT @ConvertibleBondLongAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'ConvertibleBondLongAdditionRealizedPNL'

			SELECT @ConvertibleBondShortAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'ConvertibleBondShortAdditionRealizedPNL'

			SELECT @ConvertibleBondLongWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'ConvertibleBondLongWithdrawalRealizedPNL'

			SELECT @ConvertibleBondShortWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'ConvertibleBondShortWithdrawalRealizedPNL'

			SELECT @CDSLongAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'CDSLongAdditionRealizedPNL'

			SELECT @CDSShortAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'CDSShortAdditionRealizedPNL'

			SELECT @CDSLongWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'CDSLongWithdrawalRealizedPNL'

			SELECT @CDSShortWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'CDSShortWithdrawalRealizedPNL'

			SELECT @EquitySwapLongAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquitySwapLongAdditionRealizedPNL'

			SELECT @EquitySwapShortAdditionRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquitySwapShortAdditionRealizedPNL'

			SELECT @EquitySwapLongWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquitySwapLongWithdrawalRealizedPNL'

			SELECT @EquitySwapShortWithdrawalRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquitySwapShortWithdrawalRealizedPNL'

			--------------------------------------------------- End Realized PNL -----------------------------------------
			----------------------------------------------RealizedPNLActivities-------------------------------------------  
			DECLARE @EquityLongUnRealizedPNLActivityId INT
			DECLARE @EquityShortUnRealizedPNLActivityId INT
			DECLARE @EquitySwapLongUnRealizedPNLActivityId INT
			DECLARE @EquitySwapShortUnRealizedPNLActivityId INT
			DECLARE @EquityOptionLongUnRealizedPNLActivityId INT
			DECLARE @EquityOptionShortUnRealizedPNLActivityId INT
			DECLARE @FutureLongUnRealizedPNLActivityId INT
			DECLARE @FutureShortUnRealizedPNLActivityId INT
			DECLARE @FutureOptionLongUnRealizedPNLActivitydId INT
			DECLARE @FutureOptionShortUnRealizedPNLActivitydId INT
			DECLARE @BondLongUnRealizedPNLActivityId INT
			DECLARE @BondShortUnRealizedPNLActivityId INT
			DECLARE @FXLongUnRealizedPNLActivityId INT
			DECLARE @FXShortUnRealizedPNLActivityId INT
			DECLARE @PrivateEquityLongUnRealizedPNLActivityId INT
			DECLARE @PrivateEquityShortUnRealizedPNLActivityId INT
			DECLARE @CDSLongUnRealizedPNLActivityId INT
			DECLARE @CDSShortUnRealizedPNLActivityId INT
			DECLARE @ConvertibleBondLongUnRealizedPNLActivityId INT
			DECLARE @ConvertibleBondShortUnRealizedPNLActivityId INT
			DECLARE @FXForwardLongUnRealizedPNLActivityId INT
			DECLARE @FXforwardShortUnRealizedPNLActivityId INT
			DECLARE @EquityLongAdditionUnRealizedPNLActivityId INT
			DECLARE @EquityShortAdditionUnRealizedPNLActivityId INT
			DECLARE @EquityLongWithdrawalUnRealizedPNLActivityId INT
			DECLARE @EquityShortWithdrawalUnRealizedPNLActivityId INT
			DECLARE @EquityOptionLongAdditionUnRealizedPNLActivityId INT
			DECLARE @EquityOptionShortAdditionUnRealizedPNLActivityId INT
			DECLARE @EquityOptionLongWithdrawalUnRealizedPNLActivityId INT
			DECLARE @EquityOptionShortWithdrawalUnRealizedPNLActivityId INT
			DECLARE @FutureLongAdditionUnRealizedPNLActivityId INT
			DECLARE @FutureShortAdditionUnRealizedPNLActivityId INT
			DECLARE @FutureLongWithdrawalUnRealizedPNLActivityId INT
			DECLARE @FutureShortWithdrawalUnRealizedPNLActivityId INT
			DECLARE @FutureOptionLongAdditionUnRealizedPNLActivityId INT
			DECLARE @FutureOptionShortAdditionUnRealizedPNLActivityId INT
			DECLARE @FutureOptionLongWithdrawalUnRealizedPNLActivityId INT
			DECLARE @FutureOptionShortWithdrawalUnRealizedPNLActivityId INT
			DECLARE @FXLongAdditionUnRealizedPNLActivityId INT
			DECLARE @FXShortAdditionUnRealizedPNLActivityId INT
			DECLARE @FXLongWithdrawalUnRealizedPNLActivityId INT
			DECLARE @FXShortWithdrawalUnRealizedPNLActivityId INT
			DECLARE @BondLongAdditionUnRealizedPNLActivityId INT
			DECLARE @BondShortAdditionUnRealizedPNLActivityId INT
			DECLARE @BondLongWithdrawalUnRealizedPNLActivityId INT
			DECLARE @BondShortWithdrawalUnRealizedPNLActivityId INT
			DECLARE @PrivateEquityLongAdditionUnRealizedPNLActivityId INT
			DECLARE @PrivateEquityShortAdditionUnRealizedPNLActivityId INT
			DECLARE @PrivateEquityLongWithdrawalUnRealizedPNLActivityId INT
			DECLARE @PrivateEquityShortWithdrawalUnRealizedPNLActivityId INT
			DECLARE @FXForwardLongAdditionUnRealizedPNLActivityId INT
			DECLARE @FXForwardShortAdditionUnRealizedPNLActivityId INT
			DECLARE @FXForwardLongWithdrawalUnRealizedPNLActivityId INT
			DECLARE @FXForwardShortWithdrawalUnRealizedPNLActivityId INT
			DECLARE @ConvertibleBondLongAdditionUnRealizedPNLActivityId INT
			DECLARE @ConvertibleBondShortAdditionUnRealizedPNLActivityId INT
			DECLARE @ConvertibleBondLongWithdrawalUnRealizedPNLActivityId INT
			DECLARE @ConvertibleBondShortWithdrawalUnRealizedPNLActivityId INT
			DECLARE @CDSLongAdditionUnRealizedPNLActivityId INT
			DECLARE @CDSShortAdditionUnRealizedPNLActivityId INT
			DECLARE @CDSLongWithdrawalUnRealizedPNLActivityId INT
			DECLARE @CDSShortWithdrawalUnRealizedPNLActivityId INT
			DECLARE @EquitySwapLongAdditionUnRealizedPNLActivityId INT
			DECLARE @EquitySwapShortAdditionUnRealizedPNLActivityId INT
			DECLARE @EquitySwapLongWithdrawalUnRealizedPNLActivityId INT
			DECLARE @EquitySwapShortWithdrawalUnRealizedPNLActivityId INT

			SELECT @EquityLongUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityLongUnRealizedPNL'

			SELECT @EquityShortUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityShortUnRealizedPNL'

			SELECT @EquitySwapLongUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquitySwapLongUnRealizedPNL'

			SELECT @EquitySwapShortUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquitySwapShortUnRealizedPNL'

			SELECT @EquityOptionLongUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityOptionLongUnRealizedPNL'

			SELECT @EquityOptionShortUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityOptionShortUnRealizedPNL'

			SELECT @FutureLongUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureLongUnRealizedPNL'

			SELECT @FutureShortUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureShortUnRealizedPNL'

			SELECT @FutureOptionLongUnRealizedPNLActivitydId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureOptionLongUnRealizedPNL'

			SELECT @FutureOptionShortUnRealizedPNLActivitydId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureOptionShortUnRealizedPNL'

			SELECT @BondLongUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'BondLongUnRealizedPNL'

			SELECT @BondShortUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'BondShortUnRealizedPNL'

			SELECT @FXLongUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXLongUnRealizedPNL'

			SELECT @FXShortUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXShortUnRealizedPNL'

			SELECT @PrivateEquityLongUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'PrivateEquityLongUnRealizedPNL'

			SELECT @PrivateEquityShortUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'PrivateEquityShortUnRealizedPNL'

			SELECT @CDSLongUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'CDSLongUnRealizedPNL'

			SELECT @CDSShortUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'CDSShortUnRealizedPNL'

			SELECT @ConvertibleBondLongUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'ConvertibleBondLongUnRealizedPNL'

			SELECT @ConvertibleBondShortUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'ConvertibleBondShortUnRealizedPNL'

			SELECT @FXForwardLongUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXForwardLongUnRealizedPNLActivityId'

			SELECT @FXforwardShortUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXforwardShortUnRealizedPNLActivityId'

			--------------------------------------------------- New UnRealized PNL ----------------------------------------------------------------------
			SELECT @EquityLongAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityLongAdditionUnRealizedPNL'

			SELECT @EquityShortAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityShortAdditionUnRealizedPNL'

			SELECT @EquityLongWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityLongWithdrawalUnRealizedPNL'

			SELECT @EquityShortWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityShortWithdrawalUnRealizedPNL'

			SELECT @EquityOptionLongAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityOptionLongAdditionUnRealizedPNL'

			SELECT @EquityOptionShortAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityOptionShortAdditionUnRealizedPNL'

			SELECT @EquityOptionLongWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityOptionLongWithdrawalUnRealizedPNL'

			SELECT @EquityOptionShortWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquityOptionShortWithdrawalUnRealizedPNL'

			SELECT @FutureLongAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureLongAdditionUnRealizedPNL'

			SELECT @FutureShortAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureShortAdditionUnRealizedPNL'

			SELECT @FutureLongWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureLongWithdrawalUnRealizedPNL'

			SELECT @FutureShortWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureShortWithdrawalUnRealizedPNL'

			SELECT @FutureOptionLongAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureOptionLongAdditionUnRealizedPNL'

			SELECT @FutureOptionShortAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureOptionShortAdditionUnRealizedPNL'

			SELECT @FutureOptionLongWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureOptionLongWithdrawalUnRealizedPNL'

			SELECT @FutureOptionShortWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FutureOptionShortWithdrawalUnRealizedPNL'

			SELECT @FXLongAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXLongAdditionUnRealizedPNL'

			SELECT @FXShortAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXShortAdditionUnRealizedPNL'

			SELECT @FXLongWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXLongWithdrawalUnRealizedPNL'

			SELECT @FXShortWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXShortWithdrawalUnRealizedPNL'

			SELECT @BondLongAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'BondLongAdditionUnRealizedPNL'

			SELECT @BondShortAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'BondShortAdditionUnRealizedPNL'

			SELECT @BondLongWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'BondLongWithdrawalUnRealizedPNL'

			SELECT @BondShortWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'BondShortWithdrawalUnRealizedPNL'

			SELECT @PrivateEquityLongAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'PrivateEquityLongAdditionUnRealizedPNL'

			SELECT @PrivateEquityShortAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'PrivateEquityShortAdditionUnRealizedPNL'

			SELECT @PrivateEquityLongWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'PrivateEquityLongWithdrawalUnRealizedPNL'

			SELECT @PrivateEquityShortWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'PrivateEquityShortWithdrawalUnRealizedPNL'

			SELECT @FXForwardLongAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXForwardLongAdditionUnRealizedPNL'

			SELECT @FXForwardShortAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXForwardShortAdditionUnRealizedPNL'

			SELECT @FXForwardLongWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXForwardLongWithdrawalUnRealizedPNL'

			SELECT @FXForwardShortWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'FXForwardShortWithdrawalUnRealizedPNL'

			SELECT @ConvertibleBondLongAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'ConvertibleBondLongAdditionUnRealizedPNL'

			SELECT @ConvertibleBondShortAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'ConvertibleBondShortAdditionUnRealizedPNL'

			SELECT @ConvertibleBondLongWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'ConvertibleBondLongWithdrawalUnRealizedPNL'

			SELECT @ConvertibleBondShortWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'ConvertibleBondShortWithdrawalUnRealizedPNL'

			SELECT @CDSLongAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'CDSLongAdditionUnRealizedPNL'

			SELECT @CDSShortAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'CDSShortAdditionUnRealizedPNL'

			SELECT @CDSLongWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'CDSLongWithdrawalUnRealizedPNL'

			SELECT @CDSShortWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'CDSShortWithdrawalUnRealizedPNL'

			SELECT @EquitySwapLongAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquitySwapLongAdditionUnRealizedPNL'

			SELECT @EquitySwapShortAdditionUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquitySwapShortAdditionUnRealizedPNL'

			SELECT @EquitySwapLongWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquitySwapLongWithdrawalUnRealizedPNL'

			SELECT @EquitySwapShortWithdrawalUnRealizedPNLActivityId = ActivityTypeID
			FROM [dbo].#T_ActivityType
			WHERE Acronym = 'EquitySwapShortWithdrawalUnRealizedPNL'
				--------------------------------------------------- End UnRealized PNL ---------------------------------------------------
				--------------------------------------------------- End ID Section -------------------------------------------------------
		END

		------==========================================================================================================---------
		------==========================================================================================================---------
		BEGIN
			SELECT @Step = (@RowNumber - 1) * 6 + 2

			RAISERROR (
					N'Step %i/%i: Fetching Fx rate and CA details for Selected Funds.$%i$%i$Funds:%s'
					,0
					,1
					,@Step
					,@TotalEntries
					,@userID
					,@LogInFile
					,@fundIDs
					,@Symbols
					)
			WITH NOWAIT

			DECLARE @DefaultAUECID INT

			SET @DefaultAUECID = CASE 
					WHEN @userID IS NULL
						THEN (
								SELECT TOP 1 DefaultAUECID
								FROM T_Company
								)
					ELSE (
							SELECT DefaultAUECID
							FROM T_Company
							WHERE CompanyID = (
									SELECT CompanyID
									FROM T_CompanyUser
									WHERE UserID = @userID
									)
							)
					END

			-- get Mark Price for Start Date                                                                                            
			CREATE TABLE #MarkPriceForStartDate (
				Finalmarkprice FLOAT
				,Symbol VARCHAR(50)
				,FundID INT
				,DATE DATETIME
				)

			-- get Mark Price for End Date                                                                                                                                   
			CREATE TABLE #MarkPriceForEndDate (
				Finalmarkprice FLOAT
				,Symbol VARCHAR(50)
				,FundID INT
				,DATE DATETIME
				)
		END

		CREATE TABLE #FXConversionRates (
			FromCurrencyID INT
			,ToCurrencyID INT
			,RateValue FLOAT
			,ConversionMethod INT
			,DATE DATETIME
			,eSignalSymbol VARCHAR(max)
			,fundID INT
			)

		-- get yesterday business day AUEC wise                                                  
		CREATE TABLE #AUECBusinessDatesForStartDate (
			AUECID INT
			,YESTERDAYBIZDATE DATETIME
			,RealDate DATETIME
			)

		--get business day AUEC wise for End Date
		CREATE TABLE #AUECBusinessDatesForEndDate (
			AUECID INT
			,YESTERDAYBIZDATE DATETIME
			,RealDate DATETIME
			)

		-- get Security Master Data in a Temp Table                                       
		CREATE TABLE #SecMasterDataTempTable (
			TickerSymbol VARCHAR(100)
			,AUECID INT
			,CurrencyID INT
			,Multiplier FLOAT
			,LeadCurrencyID Int
			,VsCurrencyID Int
			)

		IF (@Symbols = 'ALL_NIRVSYMBOL')
		BEGIN
			DECLARE @Symbol VARCHAR(max)

			SELECT @Symbol = COALESCE(@Symbol + ',', '') + Symbol
			FROM #Symbols
		END
		ELSE
		BEGIN
			SELECT @Symbol = COALESCE(@Symbol + ',', '') + Symbol
			FROM #Symbols
		END

		INSERT INTO #SecMasterDataTempTable
		EXEC [P_GetSecMasterData] @Symbol

		IF (
				@MinTradeDate IS NOT NULL
				AND (DATEDIFF(D, @FromDate, @MinTradeDate)) > 0
				)
		BEGIN
			SET @MinTradeDate = @FromDate
		END
		ELSE IF (@MinTradeDate IS NULL)
		BEGIN
			SET @MinTradeDate = @FromDate
		END

		SET @MinTradeDate = dbo.AdjustBusinessDays(@MinTradeDate, - 1, @DefaultAUECID)

		SELECT DISTINCT CurrencyID
		INTO #CurrencyIDs
		FROM #SecMasterDataTempTable
		WHERE TickerSymbol IN (
				SELECT Symbol
				FROM #Symbols
				)

		DECLARE @CurrencyIDs VARCHAR(200)

		SELECT @CurrencyIDs = COALESCE(@CurrencyIDs + ',', '') + CAST(CurrencyID AS VARCHAR(max))
		FROM #CurrencyIDs

		--insert FX Rate for date ranges in the temp table                                                                                                                            
		--If count >1 and Currency is non USD
		/*IF (
				(
					SELECT COUNT(*)
					FROM #CurrencyIDs
					) > 1
				OR (
					(
						SELECT currencyID
						FROM #CurrencyIDs
						) <> 1
					)
				)
		BEGIN*/
		--	INSERT INTO #FXConversionRates
		--	EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRangeGetCashPnL @MinTradeDate
		--		,@ToDate
		--		,@CurrencyIDs

Select * 
InTo #Fund_WithZeroFundId
From #Funds

Insert InTo #Fund_WithZeroFundId
Select 0

Declare @Fund_Ids Varchar(3000)

--SELECT  @Fund_Ids =STUFF((SELECT ', ' + CAST(FundID AS VARCHAR(10)) [text()]
--         FROM #Fund_WithZeroFundId 
--         FOR XML PATH(''), TYPE)
--        .value('.','NVARCHAR(MAX)'),1,2,' ') 
--FROM #Fund_WithZeroFundId T

SELECT @Fund_Ids = COALESCE(@Fund_Ids + ', ', '') + CAST(FundID AS VARCHAR(15))
FROM #Fund_WithZeroFundId

			INSERT INTO #FXConversionRates
			EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRangeGetCashPnL
			     @MinTradeDate
				,@ToDate
				,@CurrencyIDs
				,@Fund_Ids
	--	END

		-- Adjusting FxRates based on the conversion method....                                                                          
		UPDATE #FXConversionRates
		SET RateValue = 1.0 / RateValue
		WHERE RateValue <> 0
			AND ConversionMethod = 1

		-- For Fund Zero
		SELECT *
		INTO #ZeroFundFxRate
		FROM #FXConversionRates
		WHERE fundID = 0

        Delete From #FXConversionRates WHERE fundID = 0

		DECLARE @StartBusinessDate DATETIME

		SET @StartBusinessDate = @FromDate

		WHILE (DATEDIFF(D, @StartBusinessDate, @ToDate) >= 0)
		BEGIN
			--grouping required in case of multiple symbols of same AuecID
			INSERT INTO #AUECBusinessDatesForStartDate
			SELECT DISTINCT #SecMasterDataTempTable.AUECID
				,dbo.AdjustBusinessDays(@StartBusinessDate, - 1, #SecMasterDataTempTable.AUECID)
				,@StartBusinessDate
			FROM #SecMasterDataTempTable
			WHERE #SecMasterDataTempTable.TickerSymbol IN (
					SELECT Symbol
					FROM #Symbols
					)

			INSERT INTO #AUECBusinessDatesForEndDate
			SELECT DISTINCT #SecMasterDataTempTable.AUECID
				,dbo.AdjustBusinessDays(DATEADD(D, 1, @StartBusinessDate), - 1, #SecMasterDataTempTable.AUECID)
				,@StartBusinessDate
			FROM #SecMasterDataTempTable
			WHERE #SecMasterDataTempTable.TickerSymbol IN (
					SELECT Symbol
					FROM #Symbols
					)

			SET @StartBusinessDate = DATEADD(DAY, 1, @StartBusinessDate)
		END

		--Corporate Action Split
		CREATE TABLE #T_CorpActionData (
			Symbol VARCHAR(100)
			,SplitFactor FLOAT
			,EffectiveDate DATETIME
			,CorpActionID VARCHAR(100)
			,IsApplied BIT
			)

		INSERT INTO #T_CorpActionData (
			Symbol
			,SplitFactor
			,EffectiveDate
			,CorpActionID
			,IsApplied
			)
		SELECT Symbol
			,SplitFactor
			,EffectiveDate
			,CorpActionID
			,IsApplied
		FROM V_CorpActionData
		WHERE IsApplied = 1
			AND CorpActionTypeID = 6
			AND DATEDIFF(D, @FromDate, Effectivedate) >= 0
			AND DATEDIFF(D, Effectivedate, @ToDate) >= 0 -- 2. now we need this data for whole data range
			AND V_CorpActionData.Symbol IN (
				SELECT Symbol
				FROM #Symbols
				)

		CREATE TABLE #TempSplitFactorForOpen (
			TaxlotID VARCHAR(50)
			,Symbol VARCHAR(100)
			,SplitFactor FLOAT
			,EffectiveDate DATETIME
			)

		INSERT INTO #TempSplitFactorForOpen
		SELECT NA.TaxlotID
			,NA.Symbol
			,ISNULL(EXP(SUM(LOG(NA.splitFactor))), 1) AS SplitFactor
			,NA.EffectiveDate
		FROM (
			SELECT A.Taxlotid
				,A.symbol
				,VCA.SplitFactor
				,VCA.EffectiveDate
			FROM (
				SELECT TaxlotId
					,PT.Symbol AS Symbol
				FROM PM_Taxlots PT WITH (NOLOCK)
				INNER JOIN T_Group G  WITH (NOLOCK) ON G.GroupID = PT.GroupID
				INNER JOIN #Funds fnd ON fnd.FundID = PT.FundID
				INNER JOIN T_CashPreferences preftab ON preftab.FundID = PT.FundID
				WHERE TaxLotOpenQty <> 0
					AND DATEDIFF(D, preftab.CashMgmtStartDate, pt.AUECModifiedDate) >= 0
					AND Taxlot_PK IN (
						SELECT MAX(Taxlot_PK)
						FROM PM_Taxlots WITH (NOLOCK)
						WHERE DATEDIFF(D, PM_Taxlots.AUECModifiedDate, @ToDate) >= 0
							AND PM_Taxlots.FundID = fnd.FundID
							AND DATEDIFF(D, preftab.CashMgmtStartDate, pm_taxlots.AUECModifiedDate) >= 0
						GROUP BY TaxlotId
						)
				) AS A
			INNER JOIN #T_CorpActionData VCA ON A.Symbol = VCA.Symbol
			) AS NA
		GROUP BY NA.TaxlotId
			,NA.symbol
			,NA.EffectiveDate

		SELECT @Step = (@RowNumber - 1) * 6 + 3

		RAISERROR (
				N'Fetched Fx rate and CA details for Selected Funds.$%i$%i$Funds:%s'
				,0
				,1
				,@userID
				,@LogInFile
				,@fundIDs
				,@Symbols
				)
		WITH NOWAIT

		RAISERROR (
				N'Step %i/%i: Fetching Closed Positions for Selected Funds.$%i$%i$Funds:%s'
				,0
				,1
				,@Step
				,@TotalEntries
				,@userID
				,@LogInFile
				,@fundIDs
				,@Symbols
				)
		WITH NOWAIT

		/*************************************************************************                                              
      Temporary table to hold closed data which will be used for optimization                                              
    **************************************************************************/
		CREATE TABLE #TempClosingData (
			RunDate DATETIME
			,-- 3. Kuldeep- this is working like Startdate here now.
			PTTaxLotID VARCHAR(50)
			,PTSymbol VARCHAR(100)
			,PTTaxLotOpenQty FLOAT
			,PTAvgPrice FLOAT
			,PTAUECModifiedDate DATETIME
			,PTFundID INT
			,PTLevel2ID INT
			,PTOpenTotalCommissionandFees FLOAT
			,PTClosedTotalCommissionandFees FLOAT
			,PTOrderSideTagValue NCHAR(10)
			,PT1AvgPrice FLOAT
			,PT1ClosedTotalCommissionandFees FLOAT
			,TaxLotClosingID UNIQUEIDENTIFIER
			,ClosingTaxlotId VARCHAR(50)
			,ClosedQty FLOAT
			,ClosingMode INT
			,AUECLocalDate DATETIME
			,PositionSide NCHAR(10)
			,G1OrderSideTagValue VARCHAR(3)
			,GCurrencyID INT
			,GAUECID INT
			,GAUECLocalDate DATETIME
			,GAssetID INT
			,GOrderSideTagValue VARCHAR(3)
			,GCounterPartyID INT
			,GGroupID VARCHAR(50)
			,GIsswapped BIT
			,OpeningFXRate FLOAT
			,EndingFXRate FLOAT
			,TradeDateFXRate FLOAT
			,StartDateFXRate FLOAT
			,ClosingDateFXRate FLOAT
			,TransactionType VARCHAR(50)
			,PTSettleCurrency INT
			,PTSettlCurrAmt FLOAT
			,PT1SettlCurrency INT
			,PT1SettlCurrAmt FLOAT
			,PTSettlCurrClosedTotalCommissionandFees FLOAT
			,PT1SettlCurrClosedTotalCommissionandFees FLOAT
			)

			SELECT PositionalTaxlotID AS TaxlotId
				INTO #TaxlotID
				FROM PM_TaxlotClosing WITH (NOLOCK)
				WHERE DateDiff(d, @FromDate, PM_TaxlotClosing.AUECLocalDate) >= 0
				AND DateDiff(d, PM_TaxlotClosing.AUECLocalDate, @ToDate) >= 0

				INSERT INTO #TaxlotID
				SELECT ClosingTaxlotID AS TaxlotId
				FROM PM_TaxlotClosing WITH (NOLOCK)
				WHERE DateDiff(d, @FromDate, PM_TaxlotClosing.AUECLocalDate) >= 0
				AND DateDiff(d, PM_TaxlotClosing.AUECLocalDate, @ToDate) >= 0

				SELECT
				GroupID,
				TaxLotID,
				Symbol,
				OrderSideTagValue,
				TaxLotOpenQty,
				FundID,
				FXRate,
				FXConversionMethodOperator,
				AvgPrice,
				OpenTotalCommissionandFees,
				ClosedTotalCommissionandFees,
				AUECModifiedDate,
				Level2ID,
				TaxLotClosingId_Fk,
				SettlCurrency
				INTO #PM_Taxlots
				FROM PM_Taxlots WITH (NOLOCK)
				WHERE TaxLotClosingId_Fk IS NOT NULL
				AND TaxlotId IN (
				SELECT DISTINCT TaxlotId
				FROM #TaxlotID
				)

				SELECT
				GroupID,
				FXRate,
				FXConversionMethodOperator,
				IsSwapped,
				OrderSideTagValue,
				CurrencyID,
				AUECID,
				AUECLocalDate,
				ProcessDate,
				AssetID,
				CounterPartyID,
				TransactionType,
				SettlCurrency
				INTO #Group_1
				FROM T_Group WITH (NOLOCK)
				WHERE Groupid IN 
				(
				SELECT DISTINCT GroupID
				FROM #PM_Taxlots
				)

		INSERT INTO #TempClosingData
		SELECT PTC.AUECLocalDate
			,PT.[TaxLotID] AS [PTTaxLotID]
			,PT.[Symbol] AS [PTSymbol]
			,PT.[TaxLotOpenQty] AS [PTTaxLotOpenQty]
			,CASE 
					WHEN (A.AssetName='FX' OR A.AssetName='FXForward') AND G.CurrencyID <> SM.LeadCurrencyID AND PT.[AvgPrice]<>0
						THEN 1/PT.[AvgPrice]
					ELSE
						PT.[AvgPrice]
					END 
			 AS [PTAvgPrice]
			,PT.[AUECModifiedDate] AS [PTAUECModifiedDate]
			,PT.[FundID] AS [PTFundID]
			,PT.[Level2ID] AS [PTLevel2ID]
			,CASE 
				WHEN CP.IsChecked = 1
					OR G.IsSwapped = 1
					THEN ISNULL(PT.[OpenTotalCommissionandFees], 0)
				ELSE 0
				END AS [PTOpenTotalCommissionandFees]
			,CASE 
				WHEN CP.IsChecked = 1
					OR G.IsSwapped = 1
					THEN ISNULL(PT.[ClosedTotalCommissionandFees], 0)
				ELSE 0
				END AS [PTClosedTotalCommissionandFees]
			,PT.[OrderSideTagValue] AS [PTOrderSideTagValue]
			,CASE 
					WHEN (A.AssetName='FX' OR A.AssetName='FXForward') AND G.CurrencyID <> SM.LeadCurrencyID AND PT1.[AvgPrice]<>0
						THEN 1/PT1.[AvgPrice]
					ELSE
						PT1.[AvgPrice]
					END 
			 AS [PT1AvgPrice]
			,CASE 
				WHEN CP.IsChecked = 1
					OR G.IsSwapped = 1
					THEN ISNULL(PT1.[ClosedTotalCommissionandFees], 0)
				ELSE 0
				END AS [PT1ClosedTotalCommissionandFees]
			,PTC.[TaxLotClosingID] AS [TaxLotClosingID]
			,PTC.[ClosingTaxlotId] AS [ClosingTaxlotId]
			,PTC.[ClosedQty] AS [ClosedQty]
			,PTC.[ClosingMode] AS [ClosingMode]
			,PTC.[AUECLocalDate] AS [AUECLocalDate]
			,PTC.[PositionSide] AS [PositionSide]
			,G1.OrderSideTagValue AS G1OrderSideTagValue
			,G.CurrencyID AS GCurrencyID
			,G.AUECID AS GAUECID
			,G.AUECLocalDate AS GAUECLocalDate
			,G.AssetID AS GAssetID
			,G.OrderSideTagValue AS G1OrderSideTagValue
			,G.CounterPartyID AS GCounterPartyID
			,G.GroupID AS GGroupID
			,G.IsSwapped AS GIsSwapped
			,ISNULL(OpeningFXRate.Val, 0) AS OpeningFXRate
			,ISNULL(EndingFXRate.Val, 0) AS EndingFXRate
			,
			/* --TODO - should be redundant, dont use anywhere. */
			ISNULL(FXRatesForTradeDate.Val, 0) AS TradeDateFXRate
			,ISNULL(FXRatesForStartDate.Val, 0) AS StartDateFXRate
			,ISNULL(FXRatesForClosingDate.Val, 0) AS ClosingDateFXRate
			,G.TransactionType
			,PT.SettlCurrency AS PTSettleCurrency
			,CASE 
				WHEN COALESCE(PT.SettlCurrency, G.SettlCurrency) <> G.CurrencyID
					THEN PT.AvgPrice * OpeningFXRate.Val
				ELSE PT.AvgPrice
				END AS PTSettlCurrAmt
			,PT1.SettlCurrency AS PT1SettlCurrency
			,CASE 
				WHEN COALESCE(PT1.SettlCurrency, G1.SettlCurrency) <> G1.CurrencyID
					THEN PT1.AvgPrice * EndingFXRate.Val
				ELSE PT1.AvgPrice
				END AS PT1SettlCurrAmt
			,CASE 
				WHEN (
						(COALESCE(PT.SettlCurrency, G.SettlCurrency, 0) > 0)
						AND (COALESCE(PT.SettlCurrency, G.SettlCurrency) <> G.CurrencyID)
						AND OpeningFXRate.Val > 0
						AND PT.ClosedTotalCommissionandFees > 0
						)
					THEN PT.ClosedTotalCommissionandFees * OpeningFXRate.Val
				ELSE PT.ClosedTotalCommissionandFees
				END AS PTSettlCurrClosedTotalCommissionandFees
			,CASE 
				WHEN (
						(COALESCE(PT1.SettlCurrency, G1.SettlCurrency, 0) > 0)
						AND (COALESCE(PT1.SettlCurrency, G1.SettlCurrency) <> G1.CurrencyID)
						AND (EndingFXRate.Val > 0)
						AND PT1.ClosedTotalCommissionandFees > 0
						)
					THEN PT1.ClosedTotalCommissionandFees * EndingFXRate.Val
				ELSE PT1.ClosedTotalCommissionandFees
				END AS PT1SettlCurrClosedTotalCommissionandFees
		FROM PM_TaxlotClosing PTC WITH (NOLOCK)
		
		INNER JOIN #PM_Taxlots PT ON (
				PTC.PositionalTaxlotID = PT.TaxlotID
				AND PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk
				)
		INNER JOIN #PM_Taxlots PT1 ON (
				PTC.ClosingTaxlotID = PT1.TaxlotID
				AND PTC.TaxLotClosingId = PT1.TaxLotClosingId_Fk
				)
		--INNER JOIN V_SecMasterData S ON PT.Symbol=S.TickerSymbol
		INNER JOIN #SecMasterDataTempTable SM ON PT.Symbol=SM.TickerSymbol
		--INNER JOIN V_SecMasterData SM ON PT.Symbol=SM.TickerSymbol
		INNER JOIN #Funds fnd ON PT1.[FundID] = fnd.FundID
		INNER JOIN #Funds fnd1 ON PT.[FundID] = fnd1.FundID
		INNER JOIN T_CashPreferences preftab ON preftab.FundID = PT.FundID
			AND DATEDIFF(D, preftab.CashMgmtStartDate, PT.AUECModifiedDate) >= 0
		INNER JOIN #Group_1 G ON G.GroupID = PT.GroupID
		INNER JOIN #Group_1 G1 ON G1.GroupID = PT1.GroupID
		INNER JOIN T_CompanyFunds TCF ON TCF.CompanyFundID = PT1.FundID
		INNER JOIN T_Asset A ON G.AssetID = A.AssetID
		INNER JOIN T_CashPreferencesforCommission CP ON A.AssetName = CP.AssetClass
		--get yesterday business day                                                                                                                                                                         
		LEFT OUTER JOIN #AUECBusinessDatesForStartDate AUECYesterDates ON (
				G.AUECID = AUECYesterDates.AUECID
				AND PTC.AUECLocalDate = AUECYesterDates.RealDate
				)
		--Forex Price for Trade Date                                                                                                                                                                                                     		
		LEFT OUTER JOIN #FXConversionRates FXDayRatesForTradeDate ON (
				FXDayRatesForTradeDate.FromCurrencyID = G.CurrencyID
				AND FXDayRatesForTradeDate.ToCurrencyID = TCF.LocalCurrency
				AND DATEDIFF(D, G.ProcessDate, FXDayRatesForTradeDate.DATE) = 0
				AND FXDayRatesForTradeDate.FundID = PT.FundID
				)
		LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateTradeDate ON (
				ZeroFundFxRateTradeDate.FromCurrencyID = G.CurrencyID
				AND ZeroFundFxRateTradeDate.ToCurrencyID = TCF.LocalCurrency
				AND DATEDIFF(D, G.ProcessDate, ZeroFundFxRateTradeDate.DATE) = 0
				AND ZeroFundFxRateTradeDate.FundID = 0
				)
		-- Forex Price for Start Date                                                                                
		LEFT OUTER JOIN #FXConversionRates FXDayRatesForStartDate ON (
				FXDayRatesForStartDate.FromCurrencyID   = CASE WHEN G.AssetID IN (5,11) THEN 
				CASE WHEN G.CurrencyID = SM.LeadCurrencyID THEN SM.VsCurrencyID ELSE SM.LeadCurrencyID END ELSE G.CurrencyID END
				AND FXDayRatesForStartDate.ToCurrencyID = CASE WHEN G.AssetID IN (5,11) THEN G.CurrencyID ELSE TCF.LocalCurrency END   
				AND DATEDIFF(D, DATEADD(DAY, - 1, PTC.AUECLocalDate), FXDayRatesForStartDate.DATE) = 0
				AND FXDayRatesForStartDate.FundID = PT.FundID
				)
		LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateStartDate ON (
				ZeroFundFxRateStartDate.FromCurrencyID   = CASE WHEN G.AssetID IN (5,11)THEN 
				CASE WHEN G.CurrencyID = SM.LeadCurrencyID THEN SM.VsCurrencyID ELSE SM.LeadCurrencyID END ELSE G.CurrencyID END
				AND ZeroFundFxRateStartDate.ToCurrencyID = CASE WHEN G.AssetID IN (5,11) THEN G.CurrencyID ELSE TCF.LocalCurrency END 
				AND DATEDIFF(D, DATEADD(DAY, - 1, PTC.AUECLocalDate), ZeroFundFxRateStartDate.DATE) = 0
				AND ZeroFundFxRateStartDate.FundID = 0
				)
		-- Forex Price for Closing Date                                                                                              
		LEFT OUTER JOIN #FXConversionRates FXDayRatesForClosingDate ON (
				FXDayRatesForClosingDate.FromCurrencyID = G.CurrencyID
				AND FXDayRatesForClosingDate.ToCurrencyID = TCF.LocalCurrency
				AND DATEDIFF(D, G1.ProcessDate, FXDayRatesForClosingDate.DATE) = 0
				AND FXDayRatesForClosingDate.FundID = PT.FundID
				)
		LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateClosingDate ON (
				ZeroFundFxRateClosingDate.FromCurrencyID = G.CurrencyID
				AND ZeroFundFxRateClosingDate.ToCurrencyID = TCF.LocalCurrency
				AND DATEDIFF(D, G1.ProcessDate, ZeroFundFxRateClosingDate.DATE) = 0
				AND ZeroFundFxRateClosingDate.FundID = 0
				)
		CROSS APPLY (
			SELECT CASE 
					WHEN FXDayRatesForTradeDate.RateValue IS NULL
						THEN CASE 
								WHEN ZeroFundFxRateTradeDate.RateValue IS NULL
									THEN 0
								ELSE ZeroFundFxRateTradeDate.RateValue
								END
					ELSE FXDayRatesForTradeDate.RateValue
					END
			) AS FXRatesForTradeDate(Val)
		CROSS APPLY (
			SELECT CASE 
					WHEN FXDayRatesForStartDate.RateValue IS NULL
						THEN CASE 
								WHEN ZeroFundFxRateStartDate.RateValue IS NULL
									THEN 0
								ELSE ZeroFundFxRateStartDate.RateValue
								END
					ELSE FXDayRatesForStartDate.RateValue
					END
			) AS FXRatesForStartDate(Val)
		CROSS APPLY (
			SELECT CASE 
					WHEN FXDayRatesForClosingDate.RateValue IS NULL
						THEN CASE 
								WHEN ZeroFundFxRateClosingDate.RateValue IS NULL
									THEN 0
								ELSE ZeroFundFxRateClosingDate.RateValue
								END
					ELSE FXDayRatesForClosingDate.RateValue
					END
			) AS FXRatesForClosingDate(Val)
		CROSS APPLY (
			SELECT CASE 
					WHEN ISNULL(PT.FXRate, 0) > 0
						THEN CASE ISNULL(PT.FXConversionMethodOperator, 'M')
								WHEN 'M'
									THEN PT.FXRate
								WHEN 'D'
									THEN 1 / PT.FXRate
								END
					WHEN ISNULL(G.FXRate, 0) > 0
						THEN CASE ISNULL(G.FXConversionMethodOperator, 'M')
								WHEN 'M'
									THEN G.FXRate
								WHEN 'D'
									THEN 1 / G.FXRate
								END
					ELSE ISNULL(FXRatesForTradeDate.Val, 0)
					END
			) AS OpeningFXRate(Val)
		CROSS APPLY (
			SELECT CASE 
					WHEN ISNULL(PT1.FXRate, 0) > 0
						THEN CASE ISNULL(PT1.FXConversionMethodOperator, 'M')
								WHEN 'M'
									THEN PT1.FXRate
								WHEN 'D'
									THEN 1 / PT1.FXRate
								END
					WHEN ISNULL(G1.FXRate, 0) > 0
						THEN CASE ISNULL(G1.FXConversionMethodOperator, 'M')
								WHEN 'M'
									THEN G1.FXRate
								WHEN 'D'
									THEN 1 / G1.FXRate
								END
					ELSE ISNULL(FXRatesForClosingDate.Val, 0)
					END
			) AS EndingFXRate(Val)
		WHERE PT.Symbol IN 
				(
					SELECT Symbol FROM #Symbols
				  
	  
				)
		----WHERE DATEDIFF(D, @FromDate, PTC.AUECLocalDate) >= 0 --- we need closing data symbol wise
		----	AND DATEDIFF(D, PTC.AUECLocalDate, @ToDate) >= 0 -- 4. now we need this data for whole data range
		----	AND (
		----		PT.Symbol IN (
		----			SELECT Symbol
		----			FROM #Symbols
		----			)
		----		)

				Drop Table #TaxlotID, #Group_1, #PM_Taxlots
																							   
		RAISERROR (
				N'Fetched Closed Positions for Selected Funds.$%i$%i$Funds:%s'
				,0
				,1
				,@userID
				,@LogInFile
				,@fundIDs
				,@Symbols
				)
		WITH NOWAIT

		IF (@IsSwappedPrefTrue = 0)
		BEGIN
			UPDATE #TempClosingData
			SET PTOpenTotalCommissionandFees = 0
				,PTClosedTotalCommissionandFees = 0
				,PT1ClosedTotalCommissionandFees = 0
			WHERE GIsswapped = 1
		END

		-----------------------------------------------------------------------------------------------------------    
		------------------------------Getting SM information for Same Date Closed Data-----------------------------          
		SELECT PTSymbol
		INTO #SameDayClosedSymbols
		FROM #TempClosingData
		WHERE PTSymbol NOT IN (
				SELECT tickersymbol
				FROM #secmasterdatatemptable
				)

		IF EXISTS (
				SELECT 1
				FROM #SameDayClosedSymbols
				)
		BEGIN
			DECLARE @SameDayClosedSymbols VARCHAR(max)

			SELECT @SameDayClosedSymbols = COALESCE(@SameDayClosedSymbols + ',', '') + PTSymbol
			FROM #SameDayClosedSymbols

			INSERT INTO #SecMasterDataTempTable
			EXEC [P_GetSecMasterData] @SameDayClosedSymbols
		END

		INSERT INTO #Symbols
		SELECT DISTINCT PTSymbol
		FROM #SameDayClosedSymbols

		-----------------------------------------------------------------------------------------------------------     
		CREATE TABLE #PNLTable (
			Rundate DATETIME
			,Symbol VARCHAR(100)
			,FundId INT
			,Asset VARCHAR(100)
			,TradeCurrency VARCHAR(10)
			,CurrencyId INT
			,Side VARCHAR(10)
			,UnitCostLocal FLOAT
			,OpeningFXRate FLOAT
			,ClosingPriceLocal FLOAT
			,TradeDateFXRate FLOAT
			,BeginningFXRate FLOAT
			,EndingFXRate FLOAT
			,BeginningPriceLocal FLOAT
			,EndingPriceLocal FLOAT
			,BeginningQuantity FLOAT
			,EndingQuantity FLOAT
			,Multiplier FLOAT
			,SideMultiplier VARCHAR(5)
			,TradeDate DATETIME
			,Open_CloseTag VARCHAR(50)
			,IsSwapped BIT
			,TotalCost_Local FLOAT
			,TotalCost_Base FLOAT
			,TotalCost_BaseD0FX FLOAT
			,TotalCost_BaseD2FX FLOAT
			,BeginningMarketValueLocal FLOAT
			,EndingMarketValueLocal FLOAT
			,TotalOpenCommissionAndFees_Local FLOAT
			,TotalClosedCommissionAndFees_Local FLOAT
			,
			/*- derived fields which are calculated using basic fields... */
			UnrealizedTotalGainOnCostD0_Local FLOAT
			,UnrealizedFXGainOnCostD0_Base FLOAT
			,UnrealizedTotalGainOnCostD2_Local FLOAT
			,UnrealizedFXGainOnCostD2_Base FLOAT
			,TotalRealizedPNLOnCostLocal FLOAT
			,RealizedFXPNLOnCost FLOAT
			,TaxlotID VARCHAR(50)
			,ClosingTaxlotID VARCHAR(50)
			,TaxlotClosingID UNIQUEIDENTIFIER
			,LongOrShort VARCHAR(10)
			,TransactionType VARCHAR(50)
			,BaseCurrencyName VARCHAR(10)
			,BaseCurrencyID INT
			,SettlementCurrency INT
			,Settl_TotalCost_Local FLOAT
			)

		BEGIN
			----------------------------------------------Mark Prices--------------------------------------------------  
			--------------------------------------------Symbol Level Info----------------------------------------------  
			SELECT @Step = (@RowNumber - 1) * 6 + 4

			RAISERROR (
					N'Step %i/%i: Fetching Mark Prices for Selected Funds.$%i$%i'
					,0
					,1
					,@Step
					,@TotalEntries
					,@userID
					,@LogInFile
					)
			WITH NOWAIT

			DECLARE @StartDate DATETIME

			SET @StartDate = @FromDate

			WHILE (DATEDIFF(D, @StartDate, @ToDate) >= 0)
			BEGIN
				INSERT INTO #MarkPriceForStartDate (
					Finalmarkprice
					,Symbol
					,FundID
					,DATE
					)
				SELECT FinalMarkPrice
					,PM_DayMarkPrice.Symbol
					,PM_DayMarkPrice.FundID
					,AUECYesterDates.RealDate
				FROM PM_DayMarkPrice WITH (NOLOCK)
				INNER JOIN #SecMasterDataTempTable ON PM_DayMarkPrice.Symbol = #SecMasterDataTempTable.TickerSymbol
				INNER JOIN #AUECBusinessDatesForStartDate AUECYesterDates ON AUECYesterDates.AUECID = #SecMasterDataTempTable.AUECID
				WHERE DATEDIFF(D, PM_DayMarkPrice.DATE, AUECYesterDates.YESTERDAYBIZDATE) = 0
					AND PM_DayMarkPrice.FundId IN (Select FundId From #Fund_WithZeroFundId)
					AND AUECYesterDates.RealDate = @StartDate

				INSERT INTO #MarkPriceForEndDate (
					Finalmarkprice
					,Symbol
					,FundID
					,DATE
					)
				SELECT FinalMarkPrice
					,PM_DayMarkPrice.Symbol
					,PM_DayMarkPrice.FundID
					,AUECBusinessDatesForEndDate.RealDate
				FROM PM_DayMarkPrice WITH (NOLOCK)
				INNER JOIN #SecMasterDataTempTable ON PM_DayMarkPrice.Symbol = #SecMasterDataTempTable.TickerSymbol
				INNER JOIN #AUECBusinessDatesForEndDate AUECBusinessDatesForEndDate ON AUECBusinessDatesForEndDate.AUECID = #SecMasterDataTempTable.AUECID
				WHERE DATEDIFF(D, PM_DayMarkPrice.DATE, AUECBusinessDatesForEndDate.YESTERDAYBIZDATE) = 0
					AND PM_DayMarkPrice.FundId IN (Select FundId From #Fund_WithZeroFundId)
					AND AUECBusinessDatesForEndDate.RealDate = @StartDate

				SET @StartDate = DATEADD(DAY, 1, @StartDate)
			END

			SELECT DISTINCT Finalmarkprice
				,Symbol
				,FundID
				,DATE
			INTO #DistinctMarkPriceForStartDate
			FROM #MarkPriceForStartDate

			SELECT DISTINCT Finalmarkprice
				,Symbol
				,FundID
				,DATE
			INTO #DistinctMarkPriceForEndDate
			FROM #MarkPriceForEndDate

			DELETE
			FROM #MarkPriceForStartDate

			DELETE
			FROM #MarkPriceForEndDate

			INSERT INTO #MarkPriceForStartDate
			SELECT *
			FROM #DistinctMarkPriceForStartDate

			INSERT INTO #MarkPriceForEndDate
			SELECT *
			FROM #DistinctMarkPriceForEndDate

			SELECT *
			INTO #ZeroFundMarkPriceStartDate
			FROM #MarkPriceForStartDate
			WHERE fundID = 0

			SELECT *
			INTO #ZeroFundMarkPriceEndDate
			FROM #MarkPriceForEndDate
			WHERE fundID = 0

	                Delete FROM #MarkPriceForStartDate
			WHERE fundID = 0

			Delete FROM #MarkPriceForEndDate
			WHERE fundID = 0
		END

		RAISERROR (
				N'Fetched Mark Prices for Selected Funds.$%i$%i$Funds:%s'
				,0
				,1
				,@userID
				,@LogInFile
				,@fundIDs
				,@Symbols
				)
		WITH NOWAIT

		------==========================================================================================================---------
		BEGIN
			/* ------------------------------- Open Positions i.e. Unrealized P&L ------------------------------------- */
			SELECT @Step = (@RowNumber - 1) * 6 + 5

			RAISERROR (
					N'Step %i/%i: Generating PNL Entries for Selected Funds.$%i$%i$Funds:%s'
					,0
					,1
					,@Step
					,@TotalEntries
					,@userID
					,@LogInFile
					,@fundIDs
					,@Symbols
					)
			WITH NOWAIT
			-- Added By Gaurav on April 20, 2021 based on the execution plan
			Select 
			GroupID,
			CurrencyID,
			AUECLocalDate,
			IsSwapped,
			TransactionType,
			AssetID,
			AUECID,
			CounterPartyID,
			FXRate,
			FXConversionMethodOperator	
			INTO #Group from  T_group WITH (NOLOCK) where GroupID in (select distinct GroupID from #OpenPositions)

			INSERT INTO #PNLTable
			SELECT rundate
				,PT.Symbol AS Symbol
				,TCF.CompanyFundID AS FundId
				,T_Asset.AssetName AS Asset
				,TradedCurrency.CurrencySymbol AS TradeCurrency
				,CASE 
					WHEN (T_Asset.AssetName='FX' OR T_Asset.AssetName='FXForward') AND G.CurrencyID = SM.LeadCurrencyID
						THEN SM.VsCurrencyID
					WHEN (T_Asset.AssetName='FX' OR T_Asset.AssetName='FXForward') AND G.CurrencyID <> SM.LeadCurrencyID
						THEN SM.LeadCurrencyID
					ELSE TradedCurrency.CurrencyId
						END AS CurrencyId
				,CASE dbo.GetSideMultiplier(PT.OrderSideTagValue)
					WHEN 1
						THEN 'Long'
					ELSE 'Short'
					END AS Side
				,PT.AvgPrice AS UnitCostLocal
				,CASE 
					WHEN G.CurrencyID <> TCF.LocalCurrency
						THEN ISNULL(TaxLotTradeDateFxRate.Val, 0)
					ELSE 1
					END AS OpeningFXRate
				,0 AS ClosingPriceLocal
				,
				/* TODO should be redundant, dont use anywhere.  */
				ISNULL(FXRatesForTradeDate.Val, 0) AS TradeDateFXRate
				,CASE 
					WHEN (T_Asset.AssetName='FX' OR T_Asset.AssetName='FXForward') AND G.CurrencyID = 1
						THEN ISNULL(FXRatesForStartDate.Val, 0)
					WHEN (T_Asset.AssetName='FX' OR T_Asset.AssetName='FXForward') AND G.CurrencyID <> 1
						THEN 1
					WHEN G.CurrencyID <> TCF.LocalCurrency
						THEN ISNULL(FXRatesForStartDate.Val, 0)
					ELSE 1
					END AS BeginningFXRate
				,CASE 
					WHEN (T_Asset.AssetName='FX' OR T_Asset.AssetName='FXForward') AND G.CurrencyID = 1
						THEN ISNULL(FXRatesForEndDate.Val, 0)
					WHEN (T_Asset.AssetName='FX' OR T_Asset.AssetName='FXForward') AND G.CurrencyID <> 1
						THEN 1
					WHEN G.CurrencyID <> TCF.LocalCurrency
						THEN ISNULL(FXRatesForEndDate.Val, 0)
					ELSE 1
					END AS EndingFXRate
				,CASE 
					WHEN (T_Asset.AssetName='FX' OR T_Asset.AssetName='FXForward') AND G.CurrencyID <> SM.LeadCurrencyID AND MPStartDate.Val <>0
						THEN 1/MPStartDate.Val
					ELSE
						ISNULL(MPStartDate.Val, 0) 
					END AS BeginningPriceLocal
				,CASE 
					WHEN (T_Asset.AssetName='FX' OR T_Asset.AssetName='FXForward') AND G.CurrencyID <> SM.LeadCurrencyID AND MPEndDate.Val <> 0
						THEN 1/MPEndDate.Val
					ELSE
						ISNULL(MPEndDate.Val, 0)
					END AS EndingPriceLocal
				,PT.TaxLotOpenQty AS BeginningQuantity
				,0 AS EndingQuantity
				,SM.Multiplier AS Multiplier
				,dbo.GetSideMultiplier(PT.OrderSideTagValue) AS SideMultiplier
				,G.AUECLocalDate AS TradeDate
				,'O' AS Open_CloseTag
				,G.IsSwapped AS IsSwapped
				,
				/* [LOG0002] Handling For Fixed Income. */
				
				CASE 
					--EUR/USD with USD Dealin and USD/CAD with USD Dealin
					WHEN (T_Asset.AssetName='FXForward') AND G.CurrencyID = 1 AND FXRatesForEndDate.Val <> 0
						THEN 
						CASE dbo.GetSideMultiplier(PT.OrderSideTagValue)
							WHEN 1
								THEN ISNULL(((TaxlotOpenQty * ISNULL(SM.Multiplier, 0)) + ISNULL(PT.OpenTotalCommissionAndFees, 0))*-1 * dbo.GetSideMultiplier(PT.OrderSideTagValue)/FXRatesForEndDate.Val, 0)
							ELSE ISNULL(((TaxlotOpenQty * ISNULL(SM.Multiplier, 0)) - ISNULL(PT.OpenTotalCommissionAndFees, 0))*-1 * dbo.GetSideMultiplier(PT.OrderSideTagValue)/FXRatesForEndDate.Val, 0)
							END 
					ELSE
						CASE dbo.GetSideMultiplier(PT.OrderSideTagValue)
							WHEN 1
								THEN ISNULL(((PT.AvgPrice * TaxlotOpenQty * ISNULL(SM.Multiplier, 0)) + ISNULL(PT.OpenTotalCommissionAndFees, 0)) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0)
							ELSE ISNULL(((PT.AvgPrice * TaxlotOpenQty * ISNULL(SM.Multiplier, 0)) - ISNULL(PT.OpenTotalCommissionAndFees, 0)) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0)
							END 
					END AS TotalCost_Local
				,0 AS TotalCost_Base
				,
				/* Total net cost in base currency.. */
				0 AS TotalCost_BaseD0FX
				,
				/* Total Net cost Adjusted for EOD fx rate on D0 (either start date or Trade date)  */
				0 AS TotalCost_BaseD2FX
				,
				/* Total Net cost Adjusted for EOD fx rate on D2 ie end date...  */
				/*  When trade date is greater than start date MV D0 is zero  (remove the = sign here, because even if a trade is done on the start date, it does not count as open position)  */
				CASE 
					--EUR/USD with USD Dealing
					WHEN DATEDIFF(D, PT.RunDate, G.AUECLocalDate) < 0 AND (T_Asset.AssetName='FXForward') AND G.CurrencyID = 1 and SM.LeadCurrencyID <> 1 AND FXRatesForStartDate.Val<>0
						THEN ISNULL(((ISNULL(MPStartDate.Val, 0) / ISNULL(SplitTab.SplitFactor, 1)) * TaxlotOpenQty * PT.AvgPrice * ISNULL(SM.Multiplier, 0) *-1* dbo.GetSideMultiplier(PT.OrderSideTagValue))/FXRatesForStartDate.Val, 0)
					--USD/CAD with USD Dealing
					WHEN DATEDIFF(D, PT.RunDate, G.AUECLocalDate) < 0 AND (T_Asset.AssetName='FXForward') AND G.CurrencyID = 1 and SM.LeadCurrencyID = 1 AND FXRatesForStartDate.Val<>0 AND MPStartDate.Val <>0
						THEN ISNULL(((ISNULL(1/MPStartDate.Val, 0) / ISNULL(SplitTab.SplitFactor, 1)) * TaxlotOpenQty * PT.AvgPrice * ISNULL(SM.Multiplier, 0) *-1* dbo.GetSideMultiplier(PT.OrderSideTagValue))/FXRatesForStartDate.Val, 0)

					WHEN DATEDIFF(D, PT.RunDate, G.AUECLocalDate) < 0 AND (T_Asset.AssetName='FX' OR T_Asset.AssetName='FXForward') AND G.CurrencyID <> SM.LeadCurrencyID AND MPStartDate.Val <>0
						THEN ISNULL(((1/MPStartDate.Val) / ISNULL(SplitTab.SplitFactor, 1)) * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0)

					WHEN DATEDIFF(D, PT.RunDate, G.AUECLocalDate) < 0
						THEN ISNULL((ISNULL(MPStartDate.Val, 0) / ISNULL(SplitTab.SplitFactor, 1)) * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0)
					ELSE 0
					END AS BeginningMarketValueLocal
				,
				/* Beginning Market Value Adjusted for EOD FX Rate on end date... */
				CASE 
					--EUR/USD with USD Dealing
					WHEN (T_Asset.AssetName='FXForward') AND G.CurrencyID = 1 and SM.LeadCurrencyID <> 1 AND FXRatesForEndDate.Val<>0
					THEN 
						ISNULL((ISNULL(MPEndDate.Val, 0) * TaxlotOpenQty * PT.AvgPrice * ISNULL(SM.Multiplier, 0) *-1* dbo.GetSideMultiplier(PT.OrderSideTagValue))/FXRatesForEndDate.Val, 0) 
					--USD/CAD with CAD Dealing
					WHEN (T_Asset.AssetName='FXForward') AND G.CurrencyID = 1 and SM.LeadCurrencyID = 1 AND FXRatesForEndDate.Val<>0 AND MPEndDate.Val <> 0
					THEN 
						ISNULL((ISNULL(1/MPEndDate.Val, 0) * TaxlotOpenQty * PT.AvgPrice * ISNULL(SM.Multiplier, 0) *-1*dbo.GetSideMultiplier(PT.OrderSideTagValue))/FXRatesForEndDate.Val, 0) 
					WHEN (T_Asset.AssetName='FX' OR T_Asset.AssetName='FXForward') AND G.CurrencyID <> SM.LeadCurrencyID AND MPEndDate.Val <> 0
					THEN 
						ISNULL((1/MPEndDate.Val) * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0) 
					ELSE
						ISNULL(ISNULL(MPEndDate.Val, 0) * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0) 
				END AS EndingMarketValueLocal
				,ISNULL(PT.OpenTotalCommissionAndFees, 0) AS TotalOpenCommissionAndFees_Local
				,0 AS TotalClosedCommissionAndFees_Local
				/* setting all the derived fields to zero as they are calculated seperately below.....*/
				,0 AS UnrealizedTotalGainOnCostD0_Local
				,0 AS UnrealizedFXGainOnCostD0_Base
				,0 AS UnrealizedTotalGainOnCostD2_Local
				,0 AS UnrealizedFXGainOnCostD2_Base
				,0 AS TotalRealizedPNLOnCostLocal
				,0 AS RealizedFXPNLOnCost
				,PT.TaxlotID AS TaxlotID
				,NULL AS ClosingTaxLotID
				,NULL AS TaxlotClosingID
				,CASE
					WHEN G.AssetID IN (11,5) THEN 
					CASE
						WHEN PT.OrderSideTagValue = '1' THEN 'Long'
						WHEN PT.OrderSideTagValue = '2' THEN 'Short'
						ELSE ''
					END
						ELSE	dbo.GetLongOrShort(PT.OrderSideTagValue) 
				 END AS LongOrShort
				,G.TransactionType
				,CUR.CurrencySymbol AS BaseCurrencyName
				,CUR.CurrencyID AS BaseCurrencyID
				,PT.SettlCurrency AS SettlementCurrency
				,0 AS Settl_TotalCost_Local
			FROM #OpenPositions PT
			--INNER JOIN V_SecMasterData S ON PT.Symbol= S.TickerSymbol
			INNER JOIN #SecMasterDataTempTable SM ON PT.Symbol= SM.TickerSymbol
			-- COmmented By Gaurav on April 20, 2021 based on the execution plan
			INNER JOIN #Group G ON G.GroupID = PT.GroupID
			LEFT OUTER JOIN #MarkPriceForStartDate MPS ON (
					MPS.Symbol = PT.Symbol
					AND MPS.FundID = PT.FundID
					AND DateDiff(Day, MPS.DATE,PT.RunDate) = 0
					--AND MPS.DATE = CAST(FLOOR(CAST(PT.RunDate AS FLOAT)) AS DATETIME)
					)
			LEFT OUTER JOIN #ZeroFundMarkPriceStartDate MPZeroStartDate ON (
					PT.Symbol = MPZeroStartDate.Symbol
					AND MPZeroStartDate.FundID = 0
					--AND MPZeroStartDate.DATE = CAST(FLOOR(CAST(PT.RunDate AS FLOAT)) AS DATETIME)
					AND DateDiff(Day, MPZeroStartDate.DATE,PT.RunDate) = 0
					)
			LEFT OUTER JOIN #MarkPriceForEndDate MPE ON (
					MPE.Symbol = PT.Symbol
					AND MPE.FundID = PT.FundID
					--AND MPE.DATE = CAST(FLOOR(CAST(PT.RunDate AS FLOAT)) AS DATETIME)
					AND DateDiff(Day, MPE.DATE,PT.RunDate) = 0
					)
			LEFT OUTER JOIN #ZeroFundMarkPriceEndDate MPZeroEndDate ON (
					PT.Symbol = MPZeroEndDate.Symbol
					AND MPZeroEndDate.FundID = 0
					--AND MPZeroEndDate.DATE = CAST(FLOOR(CAST(PT.RunDate AS FLOAT)) AS DATETIME)
					AND DateDiff(Day, MPZeroEndDate.DATE,PT.RunDate) = 0
					)
			--Modified ( AND G.IsSwapped=1) By Gaurav on Oct 26, 2021 based on the execution plan wrt CI-3795
			LEFT OUTER JOIN T_SwapParameters SW ON G.GroupID = SW.GroupID AND G.IsSwapped=1
			--INNER JOIN #Funds fnd ON PT.FundID = fnd.FundID
			--TODO: T_CompanyFunds join consume lots of resource .... By Gaurav on Oct 26, 2021 based on the execution plan wrt CI-3795
			INNER JOIN T_CompanyFunds TCF ON PT.FundID = TCF.CompanyFundID
			LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMF ON CMF.CompanyFundID = PT.FundID
			LEFT OUTER JOIN T_companyMasterFunds MF ON MF.CompanyMasterFundID = CMF.CompanyMasterFundID
			/* join to get yesterday business day */
			LEFT OUTER JOIN #AUECBusinessDatesForStartDate AUECYesterDates ON (
					G.AUECID = AUECYesterDates.AUECID
					AND RunDate = AUECYesterDates.RealDate
					)
			LEFT OUTER JOIN #AUECBusinessDatesForEndDate AUECBusinessDatesForEndDate ON (
					G.AUECID = AUECBusinessDatesForEndDate.AUECID
					AND G.AUECLocalDate = AUECBusinessDatesForEndDate.RealDate
					)
			/* Forex Price for Trade Date other than FX Trade */
			LEFT OUTER JOIN #FXConversionRates FXDayRatesForTradeDate ON (
					FXDayRatesForTradeDate.FromCurrencyID = G.CurrencyID
					AND FXDayRatesForTradeDate.ToCurrencyID = TCF.LocalCurrency
					AND DATEDIFF(D, G.AUECLocalDate, FXDayRatesForTradeDate.DATE) = 0
					AND FXDayRatesForTradeDate.FundID = PT.FundID
					)
			LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateTradeDate ON (
					ZeroFundFxRateTradeDate.FromCurrencyID = G.CurrencyID
					AND ZeroFundFxRateTradeDate.ToCurrencyID = TCF.LocalCurrency
					AND DATEDIFF(D, G.AUECLocalDate, ZeroFundFxRateTradeDate.DATE) = 0
					AND ZeroFundFxRateTradeDate.FundID = 0
					)
			/* Forex Price for Start Date other than FX Trade */
			LEFT OUTER JOIN #FXConversionRates FXDayRatesForStartDate ON (
					FXDayRatesForStartDate.FromCurrencyID   = CASE WHEN G.AssetID IN (5,11)THEN 
					CASE WHEN G.CurrencyID = SM.LeadCurrencyID THEN SM.VsCurrencyID ELSE SM.LeadCurrencyID END ELSE G.CurrencyID END
					AND FXDayRatesForStartDate.ToCurrencyID = CASE WHEN G.AssetID IN (5,11) THEN G.CurrencyID ELSE TCF.LocalCurrency END 
					AND DATEDIFF(D, DATEADD(DAY, - 1, rundate), FXDayRatesForStartDate.DATE) = 0
					AND FXDayRatesForStartDate.FundID = PT.FundID
					)
			LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateStartDate ON (
					ZeroFundFxRateStartDate.FromCurrencyID   = CASE WHEN G.AssetID IN (5,11)THEN 
					CASE WHEN G.CurrencyID = SM.LeadCurrencyID THEN SM.VsCurrencyID ELSE SM.LeadCurrencyID END ELSE G.CurrencyID END
					AND ZeroFundFxRateStartDate.ToCurrencyID = CASE WHEN G.AssetID IN (5,11) THEN G.CurrencyID ELSE TCF.LocalCurrency END  
					AND DATEDIFF(D, DATEADD(DAY, - 1, rundate), ZeroFundFxRateStartDate.DATE) = 0
					AND ZeroFundFxRateStartDate.FundID = 0
					)
			/* Forex Price for End Date other than FX Trade */
			LEFT OUTER JOIN #FXConversionRates FXDayRatesForEndDate ON (
					FXDayRatesForEndDate.FromCurrencyID   = CASE WHEN G.AssetID IN (5,11)THEN 
					CASE WHEN G.CurrencyID = SM.LeadCurrencyID THEN SM.VsCurrencyID ELSE SM.LeadCurrencyID END ELSE G.CurrencyID END
					AND FXDayRatesForEndDate.ToCurrencyID = CASE WHEN G.AssetID IN (5,11) THEN G.CurrencyID ELSE TCF.LocalCurrency END  
					AND DATEDIFF(D, rundate, FXDayRatesForEndDate.DATE) = 0
					AND FXDayRatesForEndDate.FundID = PT.FundID
					)
			LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateEndDate ON (
					ZeroFundFxRateEndDate.FromCurrencyID   = CASE WHEN G.AssetID IN (5,11)THEN 
					CASE WHEN G.CurrencyID = SM.LeadCurrencyID THEN SM.VsCurrencyID ELSE SM.LeadCurrencyID END ELSE G.CurrencyID END
					AND ZeroFundFxRateEndDate.ToCurrencyID = CASE WHEN G.AssetID IN (5,11) THEN G.CurrencyID ELSE TCF.LocalCurrency END 
					AND DATEDIFF(D, rundate, ZeroFundFxRateEndDate.DATE) = 0
					AND ZeroFundFxRateEndDate.FundID = 0
					)
			/* Security Master DB Join  */
			--LEFT OUTER JOIN #SecMasterDataTempTable SM ON SM.TickerSymbol = PT.Symbol
			LEFT OUTER JOIN T_CompanyStrategy AS CompanyStrategy ON CompanyStrategy.CompanyStrategyID = PT.Level2ID
			INNER JOIN T_AUEC AUEC ON AUEC.AUECID = G.AUECID
			LEFT OUTER JOIN T_Underlying ON T_Underlying.UnderlyingID = AUEC.UnderlyingID
			LEFT OUTER JOIN T_Exchange ON T_Exchange.ExchangeID = AUEC.ExchangeID
			LEFT OUTER JOIN T_Currency TradedCurrency ON TradedCurrency.CurrencyId = G.CurrencyId
			LEFT OUTER JOIN T_Asset ON T_Asset.AssetId = G.AssetID
			LEFT OUTER JOIN T_CounterParty ON T_CounterParty.CounterPartyId = G.CounterPartyID
			LEFT OUTER JOIN #TempSplitFactorForOpen SplitTab ON SplitTab.TaxlotID = PT.TaxlotID
			LEFT OUTER JOIN T_Currency CUR ON CUR.CurrencyID = TCF.LocalCurrency
			CROSS APPLY (
				SELECT CASE 
						WHEN FXDayRatesForTradeDate.RateValue IS NULL
							THEN CASE 
									WHEN ZeroFundFxRateTradeDate.RateValue IS NULL
										THEN 0
									ELSE ZeroFundFxRateTradeDate.RateValue
									END
						ELSE FXDayRatesForTradeDate.RateValue
						END
				) AS FXRatesForTradeDate(Val)
			CROSS APPLY (
				SELECT CASE 
						WHEN FXDayRatesForStartDate.RateValue IS NULL
							THEN CASE 
									WHEN ZeroFundFxRateStartDate.RateValue IS NULL
										THEN 0
									ELSE ZeroFundFxRateStartDate.RateValue
									END
						ELSE FXDayRatesForStartDate.RateValue
						END
				) AS FXRatesForStartDate(Val)
			CROSS APPLY (
				SELECT CASE 
						WHEN FXDayRatesForEndDate.RateValue IS NULL
							THEN CASE 
									WHEN ZeroFundFxRateEndDate.RateValue IS NULL
										THEN 0
									ELSE ZeroFundFxRateEndDate.RateValue
									END
						ELSE FXDayRatesForEndDate.RateValue
						END
				) AS FXRatesForEndDate(Val)
			CROSS APPLY (
				SELECT CASE 
						WHEN MPS.Finalmarkprice IS NULL
							THEN CASE 
									WHEN MPZeroStartDate.Finalmarkprice IS NULL
										THEN 0
									ELSE MPZeroStartDate.Finalmarkprice
									END
						ELSE MPS.Finalmarkprice
						END
				) AS MPStartDate(Val)
			CROSS APPLY (
				SELECT CASE 
						WHEN MPE.Finalmarkprice IS NULL
							THEN CASE 
									WHEN MPZeroEndDate.Finalmarkprice IS NULL
										THEN 0
									ELSE MPZeroEndDate.Finalmarkprice
									END
						ELSE MPE.Finalmarkprice
						END
				) AS MPEndDate(Val)
			CROSS APPLY (
				SELECT CASE 
						WHEN ISNULL(PT.FXRate, 0) > 0
							THEN CASE ISNULL(PT.FXConversionMethodOperator, 'M')
									WHEN 'M'
										THEN PT.FXRate
									WHEN 'D'
										THEN 1 / PT.FXRate
									END
						WHEN ISNULL(G.FXRate, 0) > 0
							THEN CASE ISNULL(G.FXConversionMethodOperator, 'M')
									WHEN 'M'
										THEN G.FXRate
									WHEN 'D'
										THEN 1 / G.FXRate
									END
						ELSE ISNULL(FXRatesForTradeDate.Val, 0)
						END
				) AS TaxLotTradeDateFxRate(Val)
				-- Added By Gaurav on April 20, 2021 based on the execution plan
				where PT.FundID in (Select FundID from #Funds)

			/* ----------------------------Closed Positions i.e. Realized P&L------------------------------------------ */
			
			INSERT INTO #PNLTable
			SELECT PT.RunDate AS rundate
				,PTSymbol AS Symbol
				,TCF.CompanyFundID AS FundId
				,T_Asset.AssetName AS Asset
				,TradedCurrency.CurrencySymbol AS TradeCurrency
				,CASE 
					WHEN (T_Asset.AssetName='FX' OR T_Asset.AssetName='FXForward') AND GCurrencyID = SM.LeadCurrencyID
						THEN SM.VsCurrencyID
					WHEN (T_Asset.AssetName='FX' OR T_Asset.AssetName='FXForward') AND GCurrencyID <> SM.LeadCurrencyID
						THEN SM.LeadCurrencyID
					ELSE TradedCurrency.CurrencyId
						END AS CurrencyId
				,
				-- Added this as for FXforward entry, Contra entry should be made long on the closing day if it was buy or sell trade. 
				--We doing this as G1OrderSideTagValue should not be considered here. As it settles on its own. not closes with any new trade.
				CASE 
					WHEN T_Asset.AssetName LIKE 'FXForward'
						AND (
							GOrderSideTagValue LIKE '2'
							OR GOrderSideTagValue LIKE '1'
							)
						THEN 'Long'
					ELSE CASE dbo.GetSideMultiplierForClosing(GOrderSideTagValue, G1OrderSideTagValue)
							WHEN 1
								THEN 'Long'
							WHEN - 1
								THEN 'Short'
							ELSE ''
							END
					END AS Side
				,PTAvgPrice AS UnitCostLocal
				,CASE 
					WHEN GCurrencyID <> TCF.LocalCurrency
						THEN ISNULL(OpeningFXRate, 0)
					ELSE 1
					END AS OpeningFXRate
				,PT1AvgPrice AS ClosingPriceLocal
				,
				/*                                               
  --==============Ashish 20120904====                                                            
  --TODO: Redundant Field, should be truncated                                                            
  */
				TradeDateFXRate AS TradeDateFXRate
				,
				/*                                                       
  --Ashish: This is only required for calculating Starting Market Value Base, when Trade is openend before Start Date                                                                              
   */
				CASE 
					WHEN (T_Asset.AssetName='FX' OR T_Asset.AssetName='FXForward') AND GCurrencyID = 1
						THEN StartDateFXRate
					WHEN (T_Asset.AssetName='FX' OR T_Asset.AssetName='FXForward') AND GCurrencyID <> 1
						THEN 1
					WHEN GCurrencyID <> TCF.LocalCurrency
						THEN StartDateFXRate
					ELSE 1
					END AS BeginningFXRate
				,CASE 
					WHEN (T_Asset.AssetName='FX' OR T_Asset.AssetName='FXForward') AND GCurrencyID = 1
						THEN ISNULL(EndingFXRate, 0)
					WHEN (T_Asset.AssetName='FX' OR T_Asset.AssetName='FXForward') AND GCurrencyID <> 1
						THEN 1
					WHEN GCurrencyID <> TCF.LocalCurrency
						THEN ISNULL(EndingFXRate, 0)
					ELSE 1
					END AS EndingFXRate
				,CASE 
					WHEN (T_Asset.AssetName='FX' OR T_Asset.AssetName='FXForward') AND GCurrencyID <> SM.LeadCurrencyID AND MPStartDate.Val <>0
						THEN 1/MPStartDate.Val
					ELSE
						ISNULL(MPStartDate.Val, 0) 
					END AS BeginningPriceLocal
				,CASE 
					WHEN (T_Asset.AssetName='FX' OR T_Asset.AssetName='FXForward') AND GCurrencyID <> SM.LeadCurrencyID AND MPEndDate.Val <> 0
						THEN 1/MPEndDate.Val
					ELSE
						ISNULL(MPEndDate.Val, 0)
					END AS EndingPriceLocal
				--==============END Section Ashish 20120904====       
				,PTTaxLotOpenQty AS BeginningQuantity
				,ClosedQty AS EndingQuantity
				,SM.Multiplier AS Multiplier
				,dbo.GetSideMultiplierForClosing(GOrderSideTagValue, G1OrderSideTagValue) AS SideMultiplier
				,GAUECLocalDate AS TradeDate
				,
				/* now closing taxlot Trade date is closing date */
				'C' AS Open_CloseTag
				,GIsSwapped
				,
				/*[LOG0002] Handling for fixed Income commissions.*/
				CASE 
					--EUR/USD with USD Dealin and USD/CAD with USD Dealin
					WHEN (T_Asset.AssetName='FXForward') AND GCurrencyID = 1 AND EndingFXRate <> 0
						THEN 
						CASE dbo.GetSideMultiplier(PTOrderSideTagValue)
							WHEN 1
								THEN ISNULL(((ClosedQty * ISNULL(SM.Multiplier, 0)) + ISNULL(PTOpenTotalCommissionAndFees, 0)) *-1 *dbo.GetSideMultiplier(PTOrderSideTagValue)/EndingFXRate, 0)
							ELSE ISNULL(((ClosedQty * ISNULL(SM.Multiplier, 0)) - ISNULL(PTOpenTotalCommissionAndFees, 0)) *-1* dbo.GetSideMultiplier(PTOrderSideTagValue)/EndingFXRate, 0)
							END 
					ELSE
					CASE dbo.GetSideMultiplierForClosing(GOrderSideTagValue, G1OrderSideTagValue)
						WHEN 1
							THEN ISNULL(((PTAvgPrice * ClosedQty * ISNULL(SM.Multiplier, 0)) + ISNULL(PTClosedTotalCommissionandFees, 0)) * dbo.GetSideMultiplier(PTOrderSideTagValue), 0)
						ELSE ISNULL(((PTAvgPrice * ClosedQty * ISNULL(SM.Multiplier, 0)) - ISNULL(PTClosedTotalCommissionandFees, 0)) * dbo.GetSideMultiplier(PTOrderSideTagValue), 0)
						END
					END AS TotalCost_Local
				,0 AS TotalCost_Base
				,
				/*total net cost base is the cost of the postitional taxlot adjusted for EOD FX RAte on D0( either start date or trade date)... */
				0 AS TotalCost_BaseD0FX
				,
				/* total net cost base is the cost of the postitional taxlot adjusted for EOD FX RAte on D2 ie end date... */
				0 AS TotalCost_BaseD2FX
				,
				/*  Beginning Market Value in Local Currency */
				CASE 
					--EUR/USD with USD Dealing
					WHEN DATEDIFF(D, GAUECLocalDate, PT.AUECLocalDate) > 0 AND (T_Asset.AssetName='FXForward') AND GCurrencyID = 1 and SM.LeadCurrencyID <> 1 AND StartDateFXRate<>0
						THEN ISNULL(((ISNULL(MPStartDate.Val, 0) / ISNULL(SplitTab.SplitFactor, 1)) * ClosedQty *  PTAvgPrice * ISNULL(SM.Multiplier, 0) *-1* dbo.GetSideMultiplier(PTOrderSideTagValue))/StartDateFXRate, 0)
					--USD/CAD with USD Dealing
					WHEN DATEDIFF(D, GAUECLocalDate, PT.AUECLocalDate) > 0 AND (T_Asset.AssetName='FXForward') AND GCurrencyID = 1 and SM.LeadCurrencyID = 1 AND StartDateFXRate<>0 AND MPStartDate.Val IS NOT NULL and MPStartDate.Val<>0
						THEN ISNULL(((ISNULL(1/MPStartDate.Val, 0) / ISNULL(SplitTab.SplitFactor, 1)) * ClosedQty * PTAvgPrice * ISNULL(SM.Multiplier, 0) *-1* dbo.GetSideMultiplier(PTOrderSideTagValue))/StartDateFXRate, 0)

					WHEN DATEDIFF(D, GAUECLocalDate, PT.AUECLocalDate) > 0 AND (T_Asset.AssetName='FX' OR T_Asset.AssetName='FXForward') AND GCurrencyID <> SM.LeadCurrencyID AND MPStartDate.Val IS NOT NULL and MPStartDate.Val<>0
						THEN ((1/MPStartDate.Val) / ISNULL(SplitTab.SplitFactor, 1)) * ClosedQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PTOrderSideTagValue)
					WHEN DATEDIFF(D, GAUECLocalDate, PT.AUECLocalDate) > 0 -- 5. Kuldeep
						THEN (ISNULL(MPStartDate.Val, 0) / ISNULL(SplitTab.SplitFactor, 1)) * ClosedQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PTOrderSideTagValue)
					ELSE 0
						/*                    
-- beginning market is zero for all those trades which are opened after start date as beginning market value is the market value on start date-1 EOD...                           */
					END AS BeginningMarketValueLocal
				,
				-- beginning market value adjusted for EOD FX rate on end date...                    
				/* --[LOG0002] Handling For Fixed Income. */
				CASE 
					WHEN PositionSide = 'Long'
						THEN CASE 
								WHEN PT1SettlCurrency > 0
									AND PT1SettlCurrency <> GCurrencyId
									AND T_Asset.AssetName = 'Future'
									THEN ((ISNULL(PT1SettlCurrAmt, 0) * ClosedQty * ISNULL(SM.Multiplier, 0)) - ISNULL(PT1SettlCurrClosedTotalCommissionandFees, 0)) * ISNULL(dbo.GetSideMultiplierForClosing(GOrderSideTagValue, G1OrderSideTagValue), 1)
								ELSE ((ISNULL(PT1AvgPrice, 0) * ClosedQty * ISNULL(SM.Multiplier, 0)) - ISNULL(PT1ClosedTotalCommissionandFees, 0)) * ISNULL(dbo.GetSideMultiplierForClosing(GOrderSideTagValue, G1OrderSideTagValue), 1)
								END
					ELSE CASE 
							WHEN PT1SettlCurrency > 0
								AND PT1SettlCurrency <> GCurrencyId
								AND T_Asset.AssetName = 'Future'
								THEN ((ISNULL(PT1SettlCurrAmt, 0) * ClosedQty * ISNULL(SM.Multiplier, 0)) + ISNULL(PT1SettlCurrClosedTotalCommissionandFees, 0)) * ISNULL(dbo.GetSideMultiplierForClosing(GOrderSideTagValue, G1OrderSideTagValue), 1)
							ELSE ((ISNULL(PT1AvgPrice, 0) * ClosedQty * ISNULL(SM.Multiplier, 0)) + ISNULL(PT1ClosedTotalCommissionandFees, 0)) * ISNULL(dbo.GetSideMultiplierForClosing(GOrderSideTagValue, G1OrderSideTagValue), 1)
							END
					END AS EndingMarketValueLocal
				,ISNULL(PTClosedTotalCommissionandFees, 0) AS TotalOpenCommissionAndFees_Local
				,
				/* Total Closed Commission And Fee in Local Currency */
				ISNULL(PT1ClosedTotalCommissionandFees, 0) AS TotalClosedCommissionAndFees_Local
				,0 AS UnrealizedTotalGainOnCostD0_Local
				,0 AS UnrealizedFXGainOnCostD0_Base
				,0 AS UnrealizedTotalGainOnCostD2_Local
				,0 AS UnrealizedFXGainOnCostD2_Base
				,0 AS TotalRealizedPNLOnCostLocal
				,0 AS RealizedFXPNLOnCost
				,PTTaxlotID AS TaxlotID
				,ClosingTaxlotId
				,TaxlotClosingID
				,CASE
					WHEN GAssetID IN (5,11) THEN 
						CASE 
							WHEN GOrderSideTagValue = '1' THEN 'Long'
							WHEN GOrderSideTagValue = '2' THEN 'Short'
						ELSE ''
						END
					ELSE 
					--Modified by Gaurav on Oct 26, 2021 based on the execution plan wrt CI-3795
						CASE dbo.GetSideMultiplierForClosing(GOrderSideTagValue, G1OrderSideTagValue)
							WHEN 1 THEN 'Long'
							WHEN - 1 THEN 'Short'
						ELSE ''
						END
				 END AS LongOrShort
				,PT.TransactionType
				,CUR.CurrencySymbol AS BaseCurrencyName
				,CUR.CurrencyID AS BaseCurrencyID
				,CASE 
					WHEN PTSettleCurrency > 0
						AND (
							T_Asset.AssetName = 'Future'
							OR (
								T_Asset.AssetName = 'Equity'
								AND GIsSwapped = 1
								)
							)
						THEN PTSettleCurrency
					ELSE TradedCurrency.CurrencyId
					END AS SettlementCurrency
				,CASE dbo.GetSideMultiplierForClosing(GOrderSideTagValue, G1OrderSideTagValue)
					WHEN 1
						THEN CASE 
								WHEN PT1SettlCurrency > 0
									AND PT1SettlCurrency <> GCurrencyId
									AND T_Asset.AssetName = 'Future'
									THEN ISNULL(((PTSettlCurrAmt * ClosedQty * ISNULL(SM.Multiplier, 0)) + ISNULL(PTSettlCurrClosedTotalCommissionandFees, 0)) * dbo.GetSideMultiplier(PTOrderSideTagValue), 0)
								ELSE ISNULL(((PTAvgPrice * ClosedQty * ISNULL(SM.Multiplier, 0)) + ISNULL(PTClosedTotalCommissionandFees, 0)) * dbo.GetSideMultiplier(PTOrderSideTagValue), 0)
								END
					ELSE CASE 
							WHEN PT1SettlCurrency > 0
								AND PT1SettlCurrency <> GCurrencyId
								AND T_Asset.AssetName = 'Future'
								THEN ISNULL(((PTSettlCurrAmt * ClosedQty * ISNULL(SM.Multiplier, 0)) - ISNULL(PTSettlCurrClosedTotalCommissionandFees, 0)) * dbo.GetSideMultiplier(PTOrderSideTagValue), 0)
							ELSE ISNULL(((PTAvgPrice * ClosedQty * ISNULL(SM.Multiplier, 0)) - ISNULL(PTClosedTotalCommissionandFees, 0)) * dbo.GetSideMultiplier(PTOrderSideTagValue), 0)
							END
					END AS Settl_TotalCost_Local
			FROM #TempClosingData PT
			--INNER JOIN V_SecMasterData S ON PT.PTSymbol= S.TickerSymbol
			INNER JOIN #SecMasterDataTempTable SM ON PT.PTSymbol= SM.TickerSymbol
			--swap Parameter                                                              
			LEFT OUTER JOIN T_SwapParameters SW ON SW.GroupID = PT.GGroupID
			--Left Outer Join T_SwapParameters SW1 on SW1.GroupID=G1.GroupID                                                                                                       
			INNER JOIN T_AUEC AUEC ON PT.GAUECID = AUEC.AUECID
			LEFT OUTER JOIN #MarkPriceForStartDate MPS ON (
					MPS.Symbol = PT.PTSymbol
					AND MPS.FundID = PT.PTFundID
					AND MPS.DATE = CAST(FLOOR(CAST(PT.RunDate AS FLOAT)) AS DATETIME)
					)
			LEFT OUTER JOIN #ZeroFundMarkPriceStartDate MPZeroStartDate ON (
					PT.PTSymbol = MPZeroStartDate.Symbol
					AND MPZeroStartDate.FundID = 0
					AND MPZeroStartDate.DATE = CAST(FLOOR(CAST(PT.RunDate AS FLOAT)) AS DATETIME)
					)
			LEFT OUTER JOIN #MarkPriceForEndDate MPE ON (
					MPE.Symbol = PT.PTSymbol
					AND MPE.FundID = PT.PTFundID
					AND MPE.DATE = CAST(FLOOR(CAST(PT.RunDate AS FLOAT)) AS DATETIME)
					)
			LEFT OUTER JOIN #ZeroFundMarkPriceEndDate MPZeroEndDate ON (
					PT.PTSymbol = MPZeroEndDate.Symbol
					AND MPZeroEndDate.FundID = 0
					AND MPZeroEndDate.DATE = CAST(FLOOR(CAST(PT.RunDate AS FLOAT)) AS DATETIME)
					)
			-- Security Master DB join                                                                                                                                                                                                    
			--LEFT OUTER JOIN #SecMasterDataTempTable SM ON SM.TickerSymbol = PT.PTSymbol
			INNER JOIN T_CompanyFunds TCF ON PT.PTFundID = TCF.CompanyFundID
			LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMF ON CMF.CompanyFundID = PT.PTFundID
			LEFT OUTER JOIN T_companyMasterFunds MF ON MF.CompanyMasterFundID = CMF.CompanyMasterFundID
			LEFT OUTER JOIN T_CompanyStrategy AS CompanyStrategy ON CompanyStrategy.CompanyStrategyID = PT.PTLevel2ID
			LEFT OUTER JOIN T_Asset ON T_Asset.AssetId = PT.GAssetID
			LEFT OUTER JOIN T_Underlying ON T_Underlying.UnderlyingID = AUEC.UnderlyingID
			LEFT OUTER JOIN T_Exchange ON T_Exchange.ExchangeID = AUEC.ExchangeID
			--Left Outer Join T_Currency On T_Currency.CurrencyId=AUEC.BaseCurrencyId                                                                                                          
			LEFT OUTER JOIN T_Currency TradedCurrency ON TradedCurrency.CurrencyId = PT.GCurrencyId
			LEFT OUTER JOIN T_CounterParty ON T_CounterParty.CounterPartyId = PT.GCounterPartyID
			LEFT OUTER JOIN #TempSplitFactorForOpen SplitTab ON SplitTab.TaxlotID = PT.PTTaxlotID
				AND DATEDIFF(DAY, PT.PTAUECModifiedDate, SplitTab.Effectivedate) <= 0
			LEFT OUTER JOIN T_Currency CUR ON CUR.CurrencyID = TCF.LocalCurrency
			CROSS APPLY (
				SELECT CASE 
						WHEN MPS.Finalmarkprice IS NULL
							THEN CASE 
									WHEN MPZeroStartDate.Finalmarkprice IS NULL
										THEN 0
									ELSE MPZeroStartDate.Finalmarkprice
									END
						ELSE MPS.Finalmarkprice
						END
				) AS MPStartDate(Val)
			CROSS APPLY (
				SELECT CASE 
						WHEN MPE.Finalmarkprice IS NULL
							THEN CASE 
									WHEN MPZeroEndDate.Finalmarkprice IS NULL
										THEN 0
									ELSE MPZeroEndDate.Finalmarkprice
									END
						ELSE MPE.Finalmarkprice
						END
				) AS MPEndDate(Val)
			--WHERE ClosingMode <> 7 --7 means CoperateAction!

			/* ------------------------------ Name Change Corporate Action  ------------------------------------------ */
--			INSERT INTO #PNLTable
--			SELECT PT.RunDate AS rundate -- 7. Kuldeep
--				,PTSymbol AS Symbol
--				,TCF.CompanyFundID AS FundId
--				,T_Asset.AssetName AS Asset
--				,TradedCurrency.CurrencySymbol AS TradeCurrency
--				,TradedCurrency.CurrencyId AS CurrencyId
--				,CASE dbo.GetSideMultiplierForClosing(GOrderSideTagValue, G1OrderSideTagValue)
--					WHEN 1
--						THEN 'Long'
--					WHEN - 1
--						THEN 'Short'
--					ELSE ''
--					END AS Side
--				,PTAvgPrice AS UnitCostLocal
--				,CASE 
--					WHEN GCurrencyID <> TCF.LocalCurrency
--						THEN ISNULL(EndingFXRate, 0)
--					ELSE 1
--					END AS OpeningFXRate
--				,PT1AvgPrice AS ClosingPriceLocal
--				,
--				/*                  
--  --TODO - should be redundant, dont use anywhere.                                                                                               
--   */
--				TradeDateFXRate AS TradeDateFXRate
--				,CASE 
--					WHEN GCurrencyID <> TCF.LocalCurrency
--						THEN StartDateFXRate
--					ELSE 1
--					END AS BeginningFXRate
--				,CASE 
--					WHEN GCurrencyID <> TCF.LocalCurrency
--						THEN ClosingDateFXRate
--					ELSE 1
--					END AS EndingFXRate
--				,ISNULL(MPStartDate.Val, 0) AS BeginningPriceLocal
--				,ISNULL(MPEndDate.Val, 0) AS EndingPriceLocal
--				,PTTaxLotOpenQty AS BeginningQuantity
--				,ClosedQty AS EndingQuantity
--				,SM.Multiplier AS Multiplier
--				,dbo.GetSideMultiplierForClosing(GOrderSideTagValue, G1OrderSideTagValue) AS SideMultiplier
--				,GAUECLocalDate AS TradeDate
--				,
--				/* --now closing taxlot Trade date is closing date */
--				'O' AS Open_CloseTag
--				,0 AS IsSwapped
--				,ISNULL((PTAvgPrice) * ClosedQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PTOrderSideTagValue), 0) AS TotalCost_Local
--				,0 AS TotalCost_Base
--				,
--				/* -- total net cost base is the cost of the postitional taxlot... */
--				0 AS TotalCost_BaseD0FX
--				,
--				/* -- total net cost base is the cost of the postitional taxlot adjusted for EOD FX RAte on D0( either start date or trade date)... */
--				0 AS TotalCost_BaseD2FX
--				,
--				/* -- total net cost base is the cost of the postitional taxlot adjusted for EOD FX RAte on D2 ie end date... */
--				CASE 
--					WHEN DATEDIFF(D, GAUECLocalDate, pt.AUECLocalDate) > 0 -- 6. Kuldeep
--						THEN (ISNULL(MPStartDate.Val, 0)) * ClosedQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PTOrderSideTagValue)
--					ELSE 0
--						/*                                                      
---- beginning market is zero for all those trades which are opened after start date as beginning market value is the market value on start date-1 EOD...                                                        
--*/
--					END AS BeginningMarketValueLocal
--				,
--				/*  -- beginning market value adjusted for EOD FX rate on end date... */
--				/* [LOG0002] */
--				CASE dbo.GetSideMultiplierForClosing(GOrderSideTagValue, G1OrderSideTagValue)
--					WHEN 1
--						THEN ((ISNULL(PT1AvgPrice, 0) * ClosedQty * ISNULL(SM.Multiplier, 0)) - ISNULL(PT1ClosedTotalCommissionandFees, 0)) * ISNULL(dbo.GetSideMultiplierForClosing(GOrderSideTagValue, G1OrderSideTagValue), 1)
--					ELSE ((ISNULL(PT1AvgPrice, 0) * ClosedQty * ISNULL(SM.Multiplier, 0)) + ISNULL(PT1ClosedTotalCommissionandFees, 0)) * ISNULL(dbo.GetSideMultiplierForClosing(GOrderSideTagValue, G1OrderSideTagValue), 1)
--					END AS EndingMarketValueLocal
--				,
--				/*    
--  -- this market value is actually the notional value of closing taxlot but have been kept under market value tab for making PNL calculations generic...      
--  -- this value is set to zero below after the PNLS are calculated...      
--  */
--				ISNULL(PTOpenTotalCommissionandFees, 0) AS TotalOpenCommissionAndFees_Local
--				,0 AS TotalClosedCommissionAndFees_Local
--				,0 AS UnrealizedTotalGainOnCostD0_Local
--				,0 AS UnrealizedFXGainOnCostD0_Base
--				,0 AS UnrealizedTotalGainOnCostD2_Local
--				,0 AS UnrealizedFXGainOnCostD2_Base
--				,0 AS TotalRealizedPNLOnCostLocal
--				,0 AS RealizedFXPNLOnCost
--				,PTTaxlotID AS TaxlotID
--				,ClosingTaxlotId
--				,TaxlotClosingID
--				,CASE dbo.GetSideMultiplierForClosing(GOrderSideTagValue, G1OrderSideTagValue)
--					WHEN 1
--						THEN 'Long'
--					WHEN - 1
--						THEN 'Short'
--					ELSE ''
--					END AS LongOrShort
--				,PT.TransactionType
--				,CUR.CurrencySymbol AS BaseCurrencyName
--				,CUR.CurrencyID AS BaseCurrencyID
--				,PT.PTSettleCurrency AS SettlementCurrency
--				,0 AS Settl_TotalCost_Local
--			FROM #TempClosingData PT
--			INNER JOIN T_AUEC AUEC ON PT.GAUECID = AUEC.AUECID
--			LEFT OUTER JOIN #MarkPriceForStartDate MPS ON (
--					MPS.Symbol = PT.PTSymbol
--					AND MPS.FundID = PT.PTFundID
--					AND MPS.DATE = CAST(FLOOR(CAST(PT.RunDate AS FLOAT)) AS DATETIME)
--					)
--			LEFT OUTER JOIN #ZeroFundMarkPriceStartDate MPZeroStartDate ON (
--					PT.PTSymbol = MPZeroStartDate.Symbol
--					AND MPZeroStartDate.FundID = 0
--					AND MPZeroStartDate.DATE = CAST(FLOOR(CAST(PT.RunDate AS FLOAT)) AS DATETIME)
--					)
--			LEFT OUTER JOIN #MarkPriceForEndDate MPE ON (
--					MPE.Symbol = PT.PTSymbol
--					AND MPE.FundID = PT.PTFundID
--					AND MPE.DATE = CAST(FLOOR(CAST(PT.RunDate AS FLOAT)) AS DATETIME)
--					)
--			LEFT OUTER JOIN #ZeroFundMarkPriceEndDate MPZeroEndDate ON (
--					PT.PTSymbol = MPZeroEndDate.Symbol
--					AND MPZeroEndDate.FundID = 0
--					AND MPZeroEndDate.DATE = CAST(FLOOR(CAST(PT.RunDate AS FLOAT)) AS DATETIME)
--					)
--			-- Security Master DB join                                                                                                                                                    
--			LEFT OUTER JOIN #SecMasterDataTempTable SM ON SM.TickerSymbol = PT.PTSymbol
--			--LEFT OUTER JOIN T_CompanyFunds ON  #TempClosingData.PTFundID= T_CompanyFunds.CompanyFundID                  
--			INNER JOIN T_CompanyFunds TCF ON PT.PTFundID = TCF.CompanyFundID
--			LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMF ON CMF.CompanyFundID = PT.PTFundID
--			LEFT OUTER JOIN T_companyMasterFunds MF ON MF.CompanyMasterFundID = CMF.CompanyMasterFundID
--			LEFT OUTER JOIN T_CompanyStrategy AS CompanyStrategy ON CompanyStrategy.CompanyStrategyID = PT.PTLevel2ID
--			LEFT OUTER JOIN T_Asset ON T_Asset.AssetId = PT.GAssetID
--			LEFT OUTER JOIN T_Underlying ON T_Underlying.UnderlyingID = AUEC.UnderlyingID
--			LEFT OUTER JOIN T_Exchange ON T_Exchange.ExchangeID = AUEC.ExchangeID
--			--Left Outer Join T_Currency On T_Currency.CurrencyId=AUEC.BaseCurrencyId                                                                                                                        
--			LEFT OUTER JOIN T_Currency TradedCurrency ON TradedCurrency.CurrencyId = PT.GCurrencyId
--			LEFT OUTER JOIN T_CounterParty ON T_CounterParty.CounterPartyId = PT.GCounterPartyID
--			LEFT OUTER JOIN T_Currency CUR ON CUR.CurrencyID = TCF.LocalCurrency
--			CROSS APPLY (
--				SELECT CASE 
--						WHEN MPS.Finalmarkprice IS NULL
--							THEN CASE 
--									WHEN MPZeroStartDate.Finalmarkprice IS NULL
--										THEN 0
--									ELSE MPZeroStartDate.Finalmarkprice
--									END
--						ELSE MPS.Finalmarkprice
--						END
--				) AS MPStartDate(Val)
--			CROSS APPLY (
--				SELECT CASE 
--						WHEN MPE.Finalmarkprice IS NULL
--							THEN CASE 
--									WHEN MPZeroEndDate.Finalmarkprice IS NULL
--										THEN 0
--									ELSE MPZeroEndDate.Finalmarkprice
--									END
--						ELSE MPE.Finalmarkprice
--						END
--				) AS MPEndDate(Val)
--			WHERE ClosingMode = 7 --7 means Name Change Coperate Action 
				-------------------------------------------------------------------------------------------------------------------------  
		END
		
		------==========================================================================================================---------
		BEGIN
			---=========================================================================================================                                                          
			---MarketValue/NotionalValue Calculation for Fixed Income Futures-----                                                          
			---==========================================================================================================    
			RAISERROR (
					N'PNL Entries Generated for Selected Funds.$%i$%i$Funds:%s'
					,0
					,1
					,@userID
					,@LogInFile
					,@fundIDs
					,@Symbols
					)
			WITH NOWAIT

			SELECT @Step = (@RowNumber - 1) * 6 + 6

			RAISERROR (
					N'Step %i/%i: Generating Activities for Selected Funds.$%i$%i$Funds:%s'
					,0
					,1
					,@Step
					,@TotalEntries
					,@userID
					,@LogInFile
					,@fundIDs
					,@Symbols
					)
			WITH NOWAIT

			CREATE TABLE #BondFuturesMktValue (
				Symbol VARCHAR(50)
				,TaxlotID VARCHAR(max)
				,Open_CloseTag VARCHAR(50)
				,TradeDate DATETIME
				,UnitCostLocal FLOAT
				,ClosingPriceLocal FLOAT
				,BeginningPriceLocal FLOAT
				,EndingPriceLocal FLOAT
				,K0 FLOAT
				,K1 FLOAT
				,K2 FLOAT
				,BeginningMarketValueLocal FLOAT
				,EndingMarketValueLocal FLOAT
				,TotalCost_Local FLOAT
				,CouponRate FLOAT
				,CouponPayments INT
				,FaceValue FLOAT
				,BeginningQuantity FLOAT
				,EndingQuantity FLOAT
				,SideMultiplier VARCHAR(5)
				,FormulaType INT
				,DaysToMaturity INT
				,TaxlotClosingID UNIQUEIDENTIFIER
				,Multiplier FLOAT
				)

			INSERT INTO #BondFuturesMktValue (
				Symbol
				,TaxlotID
				,Open_CloseTag
				,TradeDate
				,UnitCostLocal
				,ClosingPriceLocal
				,BeginningPriceLocal
				,EndingPriceLocal
				,BeginningMarketValueLocal
				,EndingMarketValueLocal
				,TotalCost_Local
				,CouponRate
				,CouponPayments
				,FaceValue
				,BeginningQuantity
				,EndingQuantity
				,SideMultiplier
				,FormulaType
				,DaysToMaturity
				,TaxlotClosingID
				,Multiplier
				)
			SELECT PNL.Symbol
				,TaxlotID
				,Open_CloseTag
				,TradeDate
				,UnitCostLocal
				,ClosingPriceLocal
				,BeginningPriceLocal
				,EndingPriceLocal
				,BeginningMarketValueLocal
				,EndingMarketValueLocal
				,TotalCost_Local
				,CR.Coupon
				,CR.CouponPayments
				,CR.FaceValue
				,BeginningQuantity
				,EndingQuantity
				,SideMultiplier
				,CR.FormulaType
				,CR.DaysToMaturity
				,PNL.TaxlotClosingID
				,Multiplier
			FROM #PNLTable PNL
			INNER JOIN T_MW_CouponRates CR ON PNL.Symbol = CR.Symbol

			UPDATE #BondFuturesMktValue
			SET K0 = ROUND((ROUND(((CouponRate / 2) * (1 - ROUND(POWER(ROUND(1 / (1 + (100 - UnitCostLocal) / 200), 8), CouponPayments), 8))) / ((100 - UnitCostLocal) / 200), 8) + 100 * ROUND(POWER(ROUND(1 / (1 + (100 - UnitCostLocal) / 200), 8), CouponPayments), 8)) * FaceValue, 2)
				,K1 = ROUND((ROUND(((CouponRate / 2) * (1 - ROUND(POWER(ROUND(1 / (1 + (100 - BeginningPriceLocal) / 200), 8), CouponPayments), 8))) / ((100 - BeginningPriceLocal) / 200), 8) + 100 * ROUND(POWER(ROUND(1 / (1 + (100 - BeginningPriceLocal) / 200), 8), CouponPayments), 8)) * FaceValue, 2)
				,K2 = ROUND((
						ROUND((
								(CouponRate / 2) * (
									1 - ROUND(POWER(ROUND(1 / (
													1 + (
														100 - CASE 
															WHEN Open_CloseTag = 'O'
																THEN EndingPriceLocal
															WHEN Open_CloseTag = 'C'
																THEN ClosingPriceLocal
															END
														) / 200
													), 8), CouponPayments), 8)
									)
								) / (
								(
									100 - CASE 
										WHEN Open_CloseTag = 'O'
											THEN EndingPriceLocal
										WHEN Open_CloseTag = 'C'
											THEN ClosingPriceLocal
										END
									) / 200
								), 8) + 100 * ROUND(POWER(ROUND(1 / (
										1 + (
											100 - CASE 
												WHEN Open_CloseTag = 'O'
													THEN EndingPriceLocal
												WHEN Open_CloseTag = 'C'
													THEN ClosingPriceLocal
												END
											) / 200
										), 8), CouponPayments), 8)
						) * FaceValue, 2)
			WHERE FormulaType = 1

			UPDATE #BondFuturesMktValue
			SET K0 = ROUND((FaceValue * 365) / (365 + (((100 - UnitCostLocal) * DaysToMaturity) / 100)), 2)
				,K1 = ROUND((FaceValue * 365) / (365 + (((100 - BeginningPriceLocal) * DaysToMaturity) / 100)), 2)
				,K2 = ROUND((FaceValue * 365) / (
						365 + (
							(
								(
									100 - CASE 
										WHEN Open_CloseTag = 'O'
											THEN EndingPriceLocal
										WHEN Open_CloseTag = 'C'
											THEN ClosingPriceLocal
										END
									) * DaysToMaturity
								) / 100
							)
						), 2)
			WHERE FormulaType = 0

			--Ashish Poddar 20121101: This is a special handling for US T Bonds, T Notes etc. Here we have to round the market value per contract to two decimals and then multiply by Quantity                            
			UPDATE #BondFuturesMktValue
			SET K0 = ROUND((UnitCostLocal) * Multiplier, 2)
				,K1 = ROUND((BeginningPriceLocal) * Multiplier, 2)
				,K2 = ROUND((
						CASE 
							WHEN Open_CloseTag = 'O'
								THEN EndingPriceLocal
							WHEN Open_CloseTag = 'C'
								THEN ClosingPriceLocal
							END
						) * Multiplier, 2)
			WHERE FormulaType = 2

			UPDATE #BondFuturesMktValue
			SET BeginningMarketValueLocal = CASE 
					WHEN DATEDIFF(D, @FromDate, TradeDate) > 0
						THEN 0
					ELSE CASE 
							WHEN Open_CloseTag = 'O'
								THEN K1 * BeginningQuantity * SideMultiplier
							WHEN Open_CloseTag = 'C'
								THEN K1 * EndingQuantity * SideMultiplier
							END
					END
				,EndingMarketValueLocal = CASE 
					WHEN Open_CloseTag = 'O'
						THEN K2 * BeginningQuantity * SideMultiplier
					WHEN Open_CloseTag = 'C'
						THEN K2 * EndingQuantity * SideMultiplier
					END
				,TotalCost_Local = CASE 
					WHEN Open_CloseTag = 'O'
						THEN K0 * BeginningQuantity * SideMultiplier
					WHEN Open_CloseTag = 'C'
						THEN K0 * EndingQuantity * SideMultiplier
					END
				-----------------------------------------------------------------------------------------------------------------------
		END

		------==========================================================================================================---------
		BEGIN
			-------==================Merging Local Values(Basic Fields) in to the Main PNL Table for open positions================--                                                          
			UPDATE #PNLTable
			SET TotalCost_Local = CASE 
					WHEN side = 'Long'
						THEN BFM.TotalCost_Local + TotalOpenCommissionAndFees_Local
					ELSE BFM.TotalCost_Local - TotalOpenCommissionAndFees_Local
					END
				,BeginningMarketValueLocal = (BFM.BeginningMarketValueLocal)
				,EndingMarketValueLocal =
				--Ashish 20120904: WHen the positions is open, the market value should NOT be affected by Commissions                      
				(BFM.EndingMarketValueLocal)
			FROM #PNLTable PNL
			INNER JOIN #BondFuturesMktValue BFM ON (
					PNL.TaxlotID = BFM.TaxlotID
					AND PNL.Open_CloseTag = BFM.Open_CloseTag
					)
			WHERE PNL.Open_CloseTag = 'O'
				-------------------------------------------------------------------------------------------------------------------------
		END

		------==========================================================================================================---------
		BEGIN
			-------==================Merging Local Values(Basic Fields) in to the Main PNL Table for Closed Positions================--
			UPDATE #PNLTable
			SET TotalCost_Local =
				/*[LOG0003]  20121127 Corrected commissions but need to check implementation*/
				CASE 
					WHEN side = 'Long'
						THEN BFM.TotalCost_Local + TotalOpenCommissionAndFees_Local
					ELSE BFM.TotalCost_Local - TotalOpenCommissionAndFees_Local
					END
				,BeginningMarketValueLocal = (BFM.BeginningMarketValueLocal)
				,EndingMarketValueLocal = CASE 
					WHEN side = 'Long'
						THEN BFM.EndingMarketValueLocal - TotalClosedCommissionAndFees_Local
					ELSE BFM.EndingMarketValueLocal + TotalClosedCommissionAndFees_Local
					END
			FROM #PNLTable PNL
			INNER JOIN #BondFuturesMktValue BFM ON (
					PNL.TaxlotClosingID = BFM.TaxlotClosingID
					AND PNL.Open_CloseTag = BFM.Open_CloseTag
					)
			WHERE PNL.Open_CloseTag = 'C'
				---------------------------------------------------------------------------------------------------------
		END

		------==========================================================================================================---------
		BEGIN
			---==================ALL Base Calculations of Basic Fields are done here=======================   
			UPDATE #PNLTable
			SET TotalCost_Base = CASE 
					WHEN TradeCurrency <> BaseCurrencyName
						THEN OpeningFXRate * TotalCost_Local
					ELSE TotalCost_Local
					END
				,
				-- total net cost base is notional value (for closed positions its the notional value of positional taxlot)...
				TotalCost_BaseD0FX = CASE 
					WHEN TradeCurrency <> BaseCurrencyName --- If the trade is in international currency ie other than the base currency....
						THEN CASE 
								WHEN DATEDIFF(D, TradeDate, Rundate) > 0 -- When trade is opened before start date the FX rate for start date is used... 
									THEN BeginningFXRate * TotalCost_Local
										-- ASHISH 20120904: When trade is opened After or on start date - The TOtal Cost Adjusted for D0 sdoes not make any sense, hence set to zero 
								ELSE 0
								END
					ELSE TotalCost_Local -- trade is done in base currency..                         
					END
				,
				---- total net cost base is the cost of the postitional taxlot adjusted for EOD FX RAte on D0( either start date or trade date)... 
				TotalCost_BaseD2FX = CASE 
					WHEN TradeCurrency <> BaseCurrencyName
						THEN EndingFXRate * TotalCost_Local
					ELSE TotalCost_Local
					END
				----=============================================================================================================   
		END

		

		--------==========================================================================================================--------- 
		BEGIN
			-------------------------------------derived Fields are calculated here--------------------------------------------  
			UPDATE #PNLTable
			SET
				--===================                                                            
				-------P&L Calculations for open Transactions------                                                                        
				-- START SECTIOn :: Unrealized P&L ON Start Date                                                             
				-- Ashish: Formula valid for both open and close trades (any taxlot open on D0 will have unrealized gain on D0, even if it is closed on D2)                                           
				UnrealizedTotalGainOnCostD0_Local = CASE 
				WHEN DATEDIFF(D, TradeDate, Rundate) > 0 AND (Asset='FXForward') AND TradeCurrency='USD' AND BeginningFXRate <> 0
					THEN ISNULL((BeginningMarketValueLocal - (TotalCost_Local*EndingFXRate/BeginningFXRate)), 0)
				WHEN DATEDIFF(D, TradeDate, Rundate) > 0
						THEN ISNULL((BeginningMarketValueLocal - TotalCost_Local), 0)
					ELSE 0
					END
				,
				-- Ashish: Formula valid for both open and close trades (any taxlot open on D0 will have unrealized gain on D0, even if it is closed on D2) 
				UnrealizedFXGainOnCostD0_Base = CASE 
					WHEN DATEDIFF(D, TradeDate, Rundate) > 0
						THEN ISNULL((TotalCost_BaseD0FX - TotalCost_Base), 0)
					ELSE 0
					END
				,
				-- End SECTIOn :: Unrealized P&L ON Start Date                                                            
				--================================================                                                            
				--START SECTION Ending Date Unrealized PNL On Cost                                                            
				UnrealizedTotalGainOnCostD2_Local = CASE 
					WHEN (
							Open_CloseTag = 'O'
							OR Open_CloseTag = 'Accruals'
							)
						AND (Asset <> 'Cash')
						THEN ISNULL((EndingMarketValueLocal - TotalCost_Local), 0)
					ELSE 0
					END
				,UnrealizedFXGainOnCostD2_Base = CASE 
					WHEN (
							Open_CloseTag = 'O'
							OR Open_CloseTag = 'Accruals'
							)
						/*[LOG0004]*/ -- note that and (Asset <> 'Cash') condition is not applied here as we need to calculate FX Gain on T-1 Cash.           
						THEN ISNULL((TotalCost_BaseD2FX - TotalCost_Base), 0)
					ELSE 0
					END
				,RealizedFXPNLOnCost = CASE --Ashish: For futures the fx Gain Loss incured on cost is zero.                                                                                  
					WHEN (Open_CloseTag = 'C') --And  Asset not in ('Future', 'FX', 'FXForward')   /*[LOG0002]*/                                                      
						THEN ISNULL(TotalCost_BaseD2FX - TotalCost_Base, 0)
					ELSE 0
					END
				,TotalRealizedPNLOnCostLocal = CASE 
					WHEN (Open_CloseTag = 'C')
						THEN ISNULL((EndingMarketValueLocal) - CASE 
									WHEN SettlementCurrency > 0
										AND SettlementCurrency <> CurrencyID
										AND (Asset = 'Future')
										THEN Settl_TotalCost_Local
									ELSE TotalCost_Local
									END, 0)
					ELSE 0
					END
				-------End Realized P&L Calculations for Closed Transactions------   
		END
		
		--------==========================================================================================================---------
		BEGIN
			------------------------------- Rounding all basic field to two decimal ------------------------------------------                                                                                       
			UPDATE #PNLTable
			SET BeginningMarketValueLocal = ROUND(BeginningMarketValueLocal, 2)
				,EndingMarketValueLocal = ROUND(EndingMarketValueLocal, 2)
				,TotalCost_Local = ROUND(TotalCost_Local, 2)
				,TotalCost_Base = ROUND(TotalCost_Base, 2)
				,TotalCost_BaseD0FX = ROUND(TotalCost_BaseD0FX, 2)
				,TotalCost_BaseD2FX = ROUND(TotalCost_BaseD2FX, 2)
				-------------------------------------------------------------------------------------------------------------------      
		END

		------==========================================================================================================---------
		BEGIN
			---------------------------Make journal entries from realized and unrealized PNL data-------------------------  
			CREATE TABLE #RealizedPNL (
				Rundate DATETIME
				,Symbol VARCHAR(100)
				,FundId INT
				,Asset VARCHAR(100)
				,LongOrShort VARCHAR(10)
				,PNL FLOAT
				,CurrencyId INT
				,IsSwapped BIT
				,FxRate FLOAT
				,TransactionType VARCHAR(50)
				,RealizedPNLLongOrShort VARCHAR(50)
				)

			------------------------------------------Start of Realized PNL-------------------------------------
			INSERT INTO #RealizedPNL (
				Rundate
				,Symbol
				,FundId
				,Asset
				,LongOrShort
				,PNL
				,CurrencyID
				,IsSwapped
				,FxRate
				,TransactionType
				,RealizedPNLLongOrShort
				)
			--------------------------------Realized PNL in local currency-------------------------------------
			SELECT Rundate
				,Symbol
				,#PNLTable.FundId
				,Asset
				,LongOrShort
				,CASE 
					WHEN SettlementCurrency > 0
						AND (
							(
								Asset = 'Equity'
								AND IsSwapped = 1
								)
							)
						AND (SettlementCurrency = BaseCurrencyID)
						THEN TotalRealizedPNLOnCostLocal * EndingFXRate
					ELSE TotalRealizedPNLOnCostLocal
					END AS PNL
				,
				--TotalRealizedPNLOnCostLocal AS PNL,
				CASE 
					WHEN SettlementCurrency > 0
						AND (Asset = 'Future')
						THEN SettlementCurrency
					WHEN SettlementCurrency > 0
						AND (
							(
								Asset = 'Equity'
								AND IsSwapped = 1
								)
							)
						AND (SettlementCurrency = BaseCurrencyID)
						THEN BaseCurrencyID
					ELSE CurrencyID
					END AS CurrencyID
				,IsSwapped
				,CASE 
					WHEN SettlementCurrency > 0
						AND (
							Asset = 'Future'
							OR (
								Asset = 'Equity'
								AND IsSwapped = 1
								)
							)
						AND (SettlementCurrency = BaseCurrencyID)
						THEN 1
					ELSE EndingFXRate
					END AS FxRate
				,TransactionType
				,CASE 
					WHEN cp.IsBreakRealizedPnlSubaccount=0
							THEN ''
					ELSE
							CASE
								 WHEN LongOrShort = 'Long' AND DATEDIFF(D,TradeDate,Rundate) >365
										THEN 'Long Term'
								 ELSE 'Short Term'
							END
				   END AS RealizedPNLLongOrShort
			FROM #PNLTable
			INNER JOIN T_CashPreferences cp ON #PNLTable.FundId = cp.FundId
			WHERE (
					NOT (
						Asset LIKE 'Future'
						AND IsCalculatePnl = 1
						)
					)
				AND Open_CloseTag = 'C'
				AND TotalRealizedPNLOnCostLocal <> 0
				AND Asset NOT LIKE 'FXForward'
				AND Asset NOT LIKE 'FX'
			-----------------------------------------------------------------------------------------------------
			
			UNION ALL

			---------------------------------Realized FX PNL in base currency for FX and FX Forward---------------

			SELECT Rundate
				,Symbol
				,#PNLTable.FundId
				,Asset
				,LongOrShort
				,CASE 
					WHEN  (CurrencyId != BaseCurrencyID)
						THEN TotalRealizedPNLOnCostLocal * EndingFXRate
					ELSE TotalRealizedPNLOnCostLocal
					END AS PNL
				,BaseCurrencyID
				,IsSwapped
				,1
				,TransactionType
				,CASE 
					WHEN cp.IsBreakRealizedPnlSubaccount=0
							THEN ''
					ELSE
							CASE
                           		 WHEN Asset='Future'
										THEN ''
								 WHEN LongOrShort = 'Long' AND DATEDIFF(D,TradeDate,Rundate) >365
										THEN 'Long Term'
								 ELSE 'Short Term'
							END
				   END AS RealizedPNLLongOrShort
			FROM #PNLTable
			INNER JOIN T_CashPreferences cp 
			ON #PNLTable.FundId = cp.FundId
			AND (Asset LIKE 'FXForward' OR Asset LIKE 'FX')
			INNER JOIN PM_TaxlotClosing PTC WITH (NOLOCK )
			ON #PNLTable.TaxlotClosingID = PTC.TaxLotClosingID
			AND PTC.ClosingMode = 0
			WHERE Open_CloseTag = 'C'
				AND TotalRealizedPNLOnCostLocal <> 0
				
			-----------------------------------------------------------------------------------------------------

			UNION ALL
			
			------------------------------Realized FX PNL in base currency-------------------------------------
			SELECT Rundate
				,TradeCurrency
				,#PNLTable.FundId
				,Asset
				,LongOrShort
				,RealizedFXPNLOnCost AS PNL
				,BaseCurrencyID
				,IsSwapped
				,1 AS FxRate
				,TransactionType
				,CASE 
					WHEN cp.IsBreakRealizedPnlSubaccount=0
							THEN ''
					ELSE
							CASE
                           		 WHEN Asset='Future'
										THEN ''
								 WHEN LongOrShort = 'Long' AND DATEDIFF(D,TradeDate,Rundate) >365
										THEN 'Long Term'
								 ELSE 'Short Term'
							END
				   END AS RealizedPNLLongOrShort
			FROM #PNLTable
			INNER JOIN T_CashPreferences cp 
			ON #PNLTable.FundId = cp.FundId
			WHERE Open_CloseTag = 'C'
				AND RealizedFXPNLOnCost <> 0
				AND Asset NOT LIKE 'FXForward'
				AND Asset NOT LIKE 'FX'
				AND Asset NOT LIKE 'Future'
				AND (
					(
						Asset = 'Equity'
						AND IsSwapped = 1
						AND SettlementCurrency <> BaseCurrencyID
						AND @isincludeFXPNLonSwapForDiffSettleAndBaseCurr = 1
						)
								OR (
									Asset = 'Equity'
									AND IsSwapped = 1
						AND SettlementCurrency = BaseCurrencyID
						AND @isincludeFXPNLonSwapForSameSettleAndBaseCurr = 1
									)
					OR (IsSwapped <> 1)
								)
				AND (
					CASE 
						WHEN SettlementCurrency > 0
							AND (Asset = 'Future')
							AND (SettlementCurrency = BaseCurrencyID)
							THEN 0
						ELSE 1
						END
					) = 1

			----------------------------------Group Realized PNL Entries----------------------------------------
			SELECT
				--CAST(Rundate AS DATE) as Rundate,
				dateadd(dd, datediff(dd, 0, Rundate), 0) AS Rundate
				,Symbol
				,FundId
				,Asset
				,LongOrShort
				,SUM(PNL) AS PNL
				,CurrencyID
				,IsSwapped
				,MAX(FxRate) AS FxRate
				,TransactionType
				,RealizedPNLLongOrShort
			INTO #RealizedPNLGrouped
			FROM #RealizedPNL
			Where (Asset<>'Future')
			--GROUP BY CAST(Rundate AS DATE),
			GROUP BY dateadd(dd, datediff(dd, 0, Rundate), 0)
				,Symbol
				,FundId
				,Asset
				,LongOrShort
				,CurrencyID
				,IsSwapped
				,TransactionType
				,FxRate
				,RealizedPNLLongOrShort

        --------------------------------To break future realize sub account in 40% long Term and 60% short Term in the case of long side ---------------------------
			SELECT
				--CAST(Rundate AS DATE) as Rundate,
				dateadd(dd, datediff(dd, 0, Rundate), 0) AS Rundate
				,Symbol
				,#RealizedPNL.FundId
				,Asset
				,LongOrShort
				,SUM(PNL) AS PNL
				,CurrencyID
				,IsSwapped
				,MAX(FxRate) AS FxRate
				,TransactionType
				,RealizedPNLLongOrShort
            INTO #FutureRealizedPNLGrouped
			FROM #RealizedPNL
			INNER JOIN T_CashPreferences cp 
			ON #RealizedPNL.FundId = cp.FundId 
			Where (Asset='Future' AND cp.IsBreakRealizedPnlSubaccount=0)
			--GROUP BY CAST(Rundate AS DATE),
			GROUP BY dateadd(dd, datediff(dd, 0, Rundate), 0)
				,Symbol
				,#RealizedPNL.FundId
				,Asset
				,LongOrShort
				,CurrencyID
				,IsSwapped
				,TransactionType
				,FxRate
				,RealizedPNLLongOrShort		


			INSERT INTO #FutureRealizedPNLGrouped (
				Rundate
				,Symbol
				,FundId
				,Asset
				,LongOrShort
				,PNL
				,CurrencyID
				,IsSwapped
				,FxRate
				,TransactionType
				,RealizedPNLLongOrShort
				)
			SELECT
				--CAST(Rundate AS DATE) as Rundate,
				dateadd(dd, datediff(dd, 0, Rundate), 0) AS Rundate
				,Symbol
				,#RealizedPNL.FundId
				,Asset
				,LongOrShort
				,SUM((PNL * 60 )/100) AS PNL
				,CurrencyID
				,IsSwapped
				,MAX(FxRate) AS FxRate
				,TransactionType
				,'Long Term'
			FROM #RealizedPNL
			INNER JOIN T_CashPreferences cp 
			ON #RealizedPNL.FundId = cp.FundId 
			Where (Asset='Future' AND cp.IsBreakRealizedPnlSubaccount=1)
			--GROUP BY CAST(Rundate AS DATE),
			GROUP BY dateadd(dd, datediff(dd, 0, Rundate), 0)
				,Symbol
				,#RealizedPNL.FundId
				,Asset
				,LongOrShort
				,CurrencyID
				,IsSwapped
				,TransactionType
				,FxRate
				,RealizedPNLLongOrShort

		-------------------------------------------
				UNION ALL
		-------------------------------------------
		SELECT
				--CAST(Rundate AS DATE) as Rundate,
				dateadd(dd, datediff(dd, 0, Rundate), 0) AS Rundate
				,Symbol
				,#RealizedPNL.FundId
				,Asset
				,LongOrShort
				,SUM((PNL * 40 )/100) AS PNL
				,CurrencyID
				,IsSwapped
				,MAX(FxRate) AS FxRate
				,TransactionType
				,'Short Term'
			FROM #RealizedPNL
			INNER JOIN T_CashPreferences cp 
			ON #RealizedPNL.FundId = cp.FundId 
			Where (Asset='Future' AND cp.IsBreakRealizedPnlSubaccount=1 )
			--GROUP BY CAST(Rundate AS DATE),
			GROUP BY dateadd(dd, datediff(dd, 0, Rundate), 0)
				,Symbol
				,#RealizedPNL.FundId
				,Asset
				,LongOrShort
				,CurrencyID
				,IsSwapped
				,TransactionType
				,FxRate
				,RealizedPNLLongOrShort

		-----------------------------------------------------------

		INSERT INTO #RealizedPNLGrouped (
				Rundate
				,Symbol
				,FundId
				,Asset
				,LongOrShort
				,PNL
				,CurrencyID
				,IsSwapped
				,FxRate
				,TransactionType
				,RealizedPNLLongOrShort
				)
				SELECT
				--CAST(Rundate AS DATE) as Rundate,
				dateadd(dd, datediff(dd, 0, Rundate), 0) AS Rundate
				,Symbol
				,FundId
				,Asset
				,LongOrShort
				,SUM(PNL) AS PNL
				,CurrencyID
				,IsSwapped
				,MAX(FxRate) AS FxRate
				,TransactionType
				,RealizedPNLLongOrShort
			FROM #FutureRealizedPNLGrouped
			GROUP BY dateadd(dd, datediff(dd, 0, Rundate), 0)
				,Symbol
				,FundId
				,Asset
				,LongOrShort
				,CurrencyID
				,IsSwapped
				,TransactionType
				,FxRate
				,RealizedPNLLongOrShort
				----------------------------------------End of Realized PNL-------------------------------
		END
		
		------==========================================================================================================---------
		BEGIN
			----------------------------------------Start of Unrealized PNL-------------------------------
			CREATE TABLE #UnRealizedPNL (
				Rundate DATETIME
				,Symbol VARCHAR(100)
				,FundId INT
				,Asset VARCHAR(100)
				,LongOrShort VARCHAR(10)
				,PNL FLOAT
				,CurrencyId INT
				,IsSwapped BIT
				,FxRate FLOAT
				,TransactionType VARCHAR(50)
				)

			INSERT INTO #UnRealizedPNL (
				Rundate
				,Symbol
				,FundId
				,Asset
				,LongOrShort
				,PNL
				,CurrencyID
				,IsSwapped
				,FxRate
				,TransactionType
				)
			-------------------------Insert unrealized PNL-----------------------
			SELECT Rundate
				,Symbol
				,FundId
				,Asset
				,LongOrShort
				,UnrealizedTotalGainOnCostD2_Local AS PNL
				,
				--NOTE - NO fx rate is required, as Unrealized gain will always be valued at EOD fx rate
				 CurrencyID
				,IsSwapped
				,EndingFXRate AS FxRate
				,TransactionType
			FROM #PNLTable
			WHERE Open_CloseTag = 'O'
				AND UnrealizedTotalGainOnCostD2_Local <> 0
			----------------------------------------------------------------------
			
			UNION ALL
			
			---------------------Insert FX on cost--------------------------------
			SELECT Rundate
				,TradeCurrency
				,FundId
				,Asset
				,LongOrShort
				,UnrealizedFXGainOnCostD2_Base AS PNL
				,BaseCurrencyID
				,IsSwapped
				,1 AS FxRate
				,TransactionType
			FROM #PNLTable
			WHERE Open_CloseTag = 'O'
				AND UnrealizedFXGainOnCostD2_Base <> 0
				AND (UnrealizedFXGainOnCostD0_Base <> UnrealizedFXGainOnCostD2_Base)
				AND (
					(
						Asset = 'Equity'
						AND IsSwapped = 1
						AND SettlementCurrency <> BaseCurrencyID
						AND @isincludeFXPNLonSwapForDiffSettleAndBaseCurr = 1
						)
					OR (
						Asset = 'Equity'
						AND IsSwapped = 1
						AND SettlementCurrency = BaseCurrencyID
						AND @isincludeFXPNLonSwapForSameSettleAndBaseCurr = 1
						)
					OR (IsSwapped <> 1)
					)
				AND Asset NOT LIKE 'FXForward'
				AND Asset NOT LIKE 'FX'
	
			---------------------------------------------------------------------------    
			SELECT
				--CAST(Rundate AS DATE) as Rundate,
				dateadd(dd, datediff(dd, 0, Rundate), 0) AS Rundate
				,Symbol
				,#UnRealizedPNL.FundId
				,Asset
				,LongOrShort
				,SUM(PNL) AS PNL
				,CurrencyID
				,IsSwapped
				,MAX(FxRate) AS FxRate
				,TransactionType
			INTO #UnRealizedPNLGrouped
			FROM #UnRealizedPNL
			INNER JOIN T_CashPreferences cf ON cf.fundid = #UnRealizedPNL.fundid
			WHERE (
					NOT (
						Asset LIKE 'Future'
						AND IsCalculatePnl = 1
						)
					)
			--GROUP BY CAST(Rundate AS DATE),
			GROUP BY dateadd(dd, datediff(dd, 0, Rundate), 0)
				,Symbol
				,#UnRealizedPNL.FundId
				,Asset
				,LongOrShort
				,CurrencyID
				,IsSwapped
				,TransactionType
				,FxRate
				---------------------------------End of unrealized PNL-------------------------------------  
		END

		------==========================================================================================================---------
		BEGIN
			---------------------------------Start of contra unrealized PNL-------------------------------------  
			CREATE TABLE #UnRealizedPNLContraEntryTable (
				Rundate DATETIME
				,Symbol VARCHAR(100)
				,FundId INT
				,Asset VARCHAR(100)
				,LongOrShort VARCHAR(10)
				,PNL FLOAT
				,CurrencyId INT
				,IsSwapped BIT
				,FxRate FLOAT
				,TransactionType VARCHAR(50)
				)
				
			INSERT INTO #UnRealizedPNLContraEntryTable (
				Rundate
				,Symbol
				,FundId
				,Asset
				,LongOrShort
				,PNL
				,CurrencyID
				,IsSwapped
				,FxRate
				,TransactionType
				)
			----------------------Unrealized Contra PNL Entry---------------------- 
			SELECT Rundate
				,Symbol
				,FundId
				,Asset
				,LongOrShort
				,UnrealizedTotalGainOnCostD0_Local AS PNL
				,CurrencyID
				,IsSwapped
				,
				--NOTE that FX rate is a must here, becuase the entry is in local currency
				CASE 
					WHEN DATEDIFF(D, Rundate, TradeDate) = 0
						THEN TradeDateFXRate
					ELSE BeginningFXRate
					END AS FxRate
				,TransactionType
			FROM #PNLTable
			-----------------On trade date there will be no contra entry------------------
			-----------------Rundate should be greater than trade date--------------------
			WHERE DATEDIFF(D, Rundate, Tradedate) < 0
				AND UnrealizedTotalGainOnCostD0_Local <> 0
			---------------------------------------------------------------------------
			
			UNION ALL
			
			----------------------Contra FX PNL on cost Entry---------------------- 
			SELECT Rundate
				,TradeCurrency
				,FundId
				,Asset
				,LongOrShort
				,UnrealizedFXGainOnCostD0_Base AS PNL
				,BaseCurrencyID
				,IsSwapped
				,1 AS FxRate
				,TransactionType
			FROM #PNLTable
			-----------------On trade date there will be no contra entry------------------
			-----------------Rundate should be greater than trade date--------------------
			WHERE DATEDIFF(D, Rundate, Tradedate) < 0
				AND UnrealizedFXGainOnCostD0_Base <> 0
				AND (UnrealizedFXGainOnCostD0_Base <> UnrealizedFXGainOnCostD2_Base)
				AND (
					(
						Asset = 'Equity'
						AND IsSwapped = 1
						AND SettlementCurrency <> BaseCurrencyID
						AND @isincludeFXPNLonSwapForDiffSettleAndBaseCurr = 1
						)
					OR (
						Asset = 'Equity'
						AND IsSwapped = 1
						AND SettlementCurrency = BaseCurrencyID
						AND @isincludeFXPNLonSwapForSameSettleAndBaseCurr = 1
						)
					OR (IsSwapped <> 1)
					)
				AND Asset NOT LIKE 'FXForward'
				AND Asset NOT LIKE 'FX'
			
			---------------------------------------------------------------------------  
			SELECT
				--CAST(Rundate AS DATE) as Rundate,
				dateadd(dd, datediff(dd, 0, Rundate), 0) AS Rundate
				,Symbol
				,#UnRealizedPNLContraEntryTable.FundId
				,Asset
				,LongOrShort
				,SUM(PNL) AS PNL
				,CurrencyID
				,IsSwapped
				,CASE 
					WHEN (CurrencyID <> TCF.LocalCurrency)
						THEN MAX(Fxrate)
					ELSE 1
					END AS FxRate
				,TransactionType
			INTO #UnRealizedPNLContraEntryTableGrouped
			FROM #UnRealizedPNLContraEntryTable
			INNER JOIN T_CompanyFunds TCF ON TCF.CompanyFundID = #UnRealizedPNLContraEntryTable.FundId
			INNER JOIN T_CashPreferences cf ON cf.FundId = #UnRealizedPNLContraEntryTable.FundId
			WHERE (
					NOT (
						Asset LIKE 'Future'
						AND IsCalculatePnl = 1
						)
					)
			--GROUP BY CAST(Rundate AS DATE),
			GROUP BY dateadd(dd, datediff(dd, 0, Rundate), 0)
				,Symbol
				,#UnRealizedPNLContraEntryTable.FundId
				,Asset
				,LongOrShort
				,CurrencyID
				,IsSwapped
				,TransactionType
				,TCF.LocalCurrency
				,FxRate
				---------------------------------End of contra unrealized PNL-------------------------------------  
		END
		
		------==========================================================================================================---------
		BEGIN
			---------------------Return activities to the code which we want to delete--------------------  
			INSERT INTO T_TempAllActivity (
				[ActivityTypeId_FK]
				,[FKID]
				,[FundID]
				,[TransactionSource]
				,[ActivitySource]
				,[Symbol]
				,[Amount]
				,[CurrencyID]
				,[Description]
				,[SideMultiplier]
				,[TradeDate]
				,[FxRate]
				,[FXConversionMethodOperator]
				,[ActivityState]
				,[ActivityNumber]
				)
			SELECT [ActivityTypeId_FK]
				,[FKID]
				,[T_AllActivity].[FundID]
				,[TransactionSource]
				,[ActivitySource]
				,[Symbol]
				,[Amount]
				,[CurrencyID]
				,[Description]
				,[SideMultiplier]
				,[TradeDate]
				,[FxRate]
				,[FXConversionMethodOperator]
				,'Deleted'
				,[ActivityNumber]
			FROM [dbo].T_AllActivity
			INNER JOIN #funds fnd ON fnd.FundID = T_AllActivity.FundID
			WHERE ActivitySource = 3
				--Here Activity source 3 is Revaluation  
				AND DATEDIFF(D, @FromDate, TradeDate) >= 0 -- 9. Kuldeep: need to check whether we have to delete activities for whole date range or not?
				AND DATEDIFF(D, @Todate, TradeDate) <= 0
				AND (
					(
						(
							T_AllActivity.Symbol IN (
								SELECT Symbol
								FROM #Symbols
								)
							AND (
								Description = 'Realized PNL Entry'
								OR Description = 'UnRealized PNL Entry'
								OR Description = 'Contra UnRealized PNL Entry'
								)
							)
						OR (
							(@Symbols = 'ALL_NIRVSYMBOL')
							AND (
								Description = 'Realized PNL Entry'
								OR Description = 'UnRealized PNL Entry'
								OR Description = 'Contra UnRealized PNL Entry'
								)
							)
						)
					OR (
						(
							T_AllActivity.Symbol IN (
								SELECT CurrencySymbol
								FROM #CurrencyIds C
								INNER JOIN T_Currency TC ON TC.CurrencyID = C.CurrencyID
								)
							AND (
								Description = 'Realized PNL Entry'
								OR Description = 'UnRealized PNL Entry'
								OR Description = 'Contra UnRealized PNL Entry'
								)
							)
						OR (
							(@Symbols = 'ALL_NIRVSYMBOL')
							AND (
								Description = 'Realized PNL Entry'
								OR Description = 'UnRealized PNL Entry'
								OR Description = 'Contra UnRealized PNL Entry'
								)
							)
						)
					)

			DECLARE @MinCalcDate DATETIME

			SELECT @MinCalcDate = MIN(LastCalcDate)
			FROM T_LastCalcDateRevaluation
			WHERE FundID IN (
					SELECT OriginalFundID
					FROM #OriginalFunds
					)

			-- This code is moved in "P_RunDailyAssetRevaluation" SP.
			--    INSERT INTO T_TempAllActivity ([ActivityTypeId_FK]
			--    , [FKID]
			--    , [FundID]
			--    , [TransactionSource]
			--    , [ActivitySource]
			--    , [Symbol]
			--    , [Amount]
			--    , [CurrencyID]
			--    , [Description]
			--    , [SideMultiplier]
			--    , [TradeDate]
			--    , [FxRate]
			--    , [FXConversionMethodOperator]
			--    , [ActivityState]
			--    , [ActivityNumber])
			--      SELECT
			--        [ActivityTypeId_FK],
			--        [FKID],
			--        [T_AllActivity].[FundID],
			--        [TransactionSource],
			--        [ActivitySource],
			--        [Symbol],
			--        [Amount],
			--        [CurrencyID],
			--        [Description],
			--    [SideMultiplier],
			--        [TradeDate],
			--        [FxRate],
			--        [FXConversionMethodOperator],
			--        'Deleted',
			--        [ActivityNumber]
			--      FROM [dbo].T_AllActivity
			--      INNER JOIN #Funds fund      
			--        ON fund.FundID = T_AllActivity.FundID 
			--INNER JOIN      T_LastCalcDateRevaluation LCDR
			--ON LCDR.Fundid = fund.FundID
			--      WHERE ActivitySource = 3
			--      --Here Activity source 3 is Revaluation    
			--      AND (
			--      (
			--      @IsManualRevaluation = 0
			--      AND DATEDIFF(D, LastCalcDate, TradeDate) >= 0      
			--      AND DATEDIFF(D, @EndDate, TradeDate) <= 0
			--      )
			--      OR (
			--      @IsManualRevaluation = 1
			--      AND DATEDIFF(D, @FromDate, TradeDate) >= 0
			--      AND DATEDIFF(D, @Todate, TradeDate) <= 0
			--      )
			--      )
			--    AND (
			--      Description IN (
			--      'Cash PNL Entry'
			--      , 'Contra Cash PNL Entry'
			--      , 'Accruals PNL Entry'
			--      , 'Contra Accruals PNL Entry'
			--      , 'Cash to Unrealized FXRate PNL Entry'
			--      , 'Unrealized To Cash FXRate PNL Entry'
			--      , 'Contra Accrual DayEnd FXRate PNL Entry'
			--      , 'Accrual to Unrealized DayEnd FXRate PNL Entry'
			--      , 'Unrealized To Accrual DayEnd FXRate PNL Entry'
			--      , 'Contra FXRate PNL Entry'
			--      )
			--      )
			-----------------------------------------------------------------------------------------------
			----following temporary table will be used to store activity data and activity data will be imported at once  
			CREATE TABLE #T_AllActivity (
				[ActivityTypeId_FK] [int] NOT NULL
				,[FKID] [varchar](50) NULL
				,[FundID] [int] NULL
				,[TransactionSource] [int] NOT NULL
				,[ActivitySource] [int] NOT NULL
				,[Symbol] [varchar](100) NULL
				,[Amount] FLOAT NULL
				,[CurrencyID] [int] NULL
				,[Description] [varchar](3000) NULL
				,[SideMultiplier] [int] NULL
				,[TradeDate] [datetime] NULL
				,[FxRate] [float] NULL
				,[FXConversionMethodOperator] VARCHAR(3) NULL
				,[ActivityState] VARCHAR(50) NULL
				,[ActivityNumber] INT NULL
				,[SubAccountID] INT NULL
				)
				--------------------------------------------------------------------------------------------------------
		END

		------==========================================================================================================---------
		BEGIN
			-------------------------------------Handling for contra entry unrealized PNL-------------------------------    
			INSERT INTO [dbo].[#T_AllActivity] (
				[ActivityTypeId_FK]
				,[FKID]
				,[FundID]
				,[TransactionSource]
				,[ActivitySource]
				,[Symbol]
				,[Amount]
				,[CurrencyID]
				,[Description]
				,[SideMultiplier]
				,[TradeDate]
				,[FxRate]
				,[FXConversionMethodOperator]
				,[ActivityState]
				,[ActivityNumber]
				,[SubAccountID]
				)
			SELECT CASE 
					WHEN (
							Asset = 'Equity'
							OR Asset = 'PrivateEquity'
							)
						AND IsSwapped = 1
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @EquitySwapLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @EquitySwapLongWithdrawalUnRealizedPNLActivityId
											ELSE @EquitySwapLongUnRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @EquitySwapShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @EquitySwapShortWithdrawalUnRealizedPNLActivityId
										ELSE @EquitySwapShortUnRealizedPNLActivityId
										END
								END
					WHEN (Asset = 'Equity')
						AND IsSwapped = 0
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @EquityLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @EquityLongWithdrawalUnRealizedPNLActivityId
											ELSE @EquityLongUnRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @EquityShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @EquityShortWithdrawalUnRealizedPNLActivityId
										ELSE @EquityShortUnRealizedPNLActivityId
										END
								END
					WHEN (Asset = 'PrivateEquity')
						AND IsSwapped = 0
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @PrivateEquityLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @PrivateEquityLongWithdrawalUnRealizedPNLActivityId
											ELSE @PrivateEquityLongUnRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @PrivateEquityShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @PrivateEquityShortWithdrawalUnRealizedPNLActivityId
										ELSE @PrivateEquityShortUnRealizedPNLActivityId
										END
								END
					WHEN (Asset = 'CreditDefaultSwap')
						AND IsSwapped = 0
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @CDSLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @CDSLongWithdrawalUnRealizedPNLActivityId
											ELSE @CDSLongUnRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @CDSShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @CDSShortWithdrawalUnRealizedPNLActivityId
										ELSE @CDSShortUnRealizedPNLActivityId
										END
								END
					WHEN Asset = 'EquityOption'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @EquityOptionLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @EquityOptionLongWithdrawalUnRealizedPNLActivityId
											ELSE @EquityOptionLongUnRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @EquityOptionShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @EquityOptionShortWithdrawalUnRealizedPNLActivityId
										ELSE @EquityOptionShortUnRealizedPNLActivityId
										END
								END
					WHEN Asset = 'Future'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @FutureLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @FutureLongWithdrawalUnRealizedPNLActivityId
											ELSE @FutureLongUnRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @FutureShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @FutureShortWithdrawalUnRealizedPNLActivityId
										ELSE @FutureShortUnRealizedPNLActivityId
										END
								END
					WHEN Asset = 'FutureOption'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @FutureOptionLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @FutureOptionLongWithdrawalUnRealizedPNLActivityId
											ELSE @FutureOptionLongUnRealizedPNLActivitydId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @FutureOptionShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @FutureOptionShortWithdrawalUnRealizedPNLActivityId
										ELSE @FutureOptionShortUnRealizedPNLActivitydId
										END
								END
					WHEN Asset = 'FixedIncome'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @BondLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @BondLongWithdrawalUnRealizedPNLActivityId
											ELSE @BondLongUnRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @BondShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @BondShortWithdrawalUnRealizedPNLActivityId
										ELSE @BondShortUnRealizedPNLActivityId
										END
								END
					WHEN Asset = 'ConvertibleBond'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @ConvertibleBondLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @ConvertibleBondLongWithdrawalUnRealizedPNLActivityId
											ELSE @ConvertibleBondLongUnRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @ConvertibleBondShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @ConvertibleBondShortWithdrawalUnRealizedPNLActivityId
										ELSE @ConvertibleBondShortUnRealizedPNLActivityId
										END
								END
					WHEN Asset = 'FX'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @FXLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @FXLongWithdrawalUnRealizedPNLActivityId
											ELSE @FXLongUnRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @FXShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @FXShortWithdrawalUnRealizedPNLActivityId
										ELSE @FXShortUnRealizedPNLActivityId
										END
								END
					WHEN Asset = 'FXForward'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @FXForwardLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @FXForwardLongWithdrawalUnRealizedPNLActivityId
											ELSE @FXForwardLongUnRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @FXForwardShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @FXForwardShortWithdrawalUnRealizedPNLActivityId
										ELSE @FXForwardShortUnRealizedPNLActivityId
										END
								END
					END AS ActivityTypeId_FK
				,NULL AS FKID
				,FundID
				,9 AS TransactionSource
				,3 AS ActivitySource
				,Symbol
				,PNL * (- 1) AS Amount
				,CurrencyID
				,'Contra UnRealized PNL Entry' AS Description
				,- 1 AS SideMultiplier
				,RunDate AS TradeDate
				,FxRate
				,'M' AS FXConversionMethodOperator
				,'New'
				,1
				,0
			FROM #UnRealizedPNLContraEntryTableGrouped
				------------------------------end of conta entry for unrealized PNL------------------------- 
		END
		
		------==========================================================================================================---------
		BEGIN
			-------------------------------------Handling for unrealized PNL-------------------------------  
			INSERT INTO [dbo].[#T_AllActivity] (
				[ActivityTypeId_FK]
				,[FKID]
				,[FundID]
				,[TransactionSource]
				,[ActivitySource]
				,[Symbol]
				,[Amount]
				,[CurrencyID]
				,[Description]
				,[SideMultiplier]
				,[TradeDate]
				,[FxRate]
				,[FXConversionMethodOperator]
				,[ActivityState]
				,[ActivityNumber]
				,[SubAccountID]
				)
			SELECT CASE 
					WHEN (
							Asset = 'Equity'
							OR Asset = 'PrivateEquity'
							)
						AND IsSwapped = 1
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @EquitySwapLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @EquitySwapLongWithdrawalUnRealizedPNLActivityId
											ELSE @EquitySwapLongUnRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @EquitySwapShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @EquitySwapShortWithdrawalUnRealizedPNLActivityId
										ELSE @EquitySwapShortUnRealizedPNLActivityId
										END
								END
					WHEN (Asset = 'Equity')
						AND IsSwapped = 0
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @EquityLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @EquityLongWithdrawalUnRealizedPNLActivityId
											ELSE @EquityLongUnRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @EquityShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @EquityShortWithdrawalUnRealizedPNLActivityId
										ELSE @EquityShortUnRealizedPNLActivityId
										END
								END
					WHEN (Asset = 'PrivateEquity')
						AND IsSwapped = 0
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @PrivateEquityLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @PrivateEquityLongWithdrawalUnRealizedPNLActivityId
											ELSE @PrivateEquityLongUnRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @PrivateEquityShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @PrivateEquityShortWithdrawalUnRealizedPNLActivityId
										ELSE @PrivateEquityShortUnRealizedPNLActivityId
										END
								END
					WHEN (Asset = 'CreditDefaultSwap')
						AND IsSwapped = 0
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @CDSLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @CDSLongWithdrawalUnRealizedPNLActivityId
											ELSE @CDSLongUnRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @CDSShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @CDSShortWithdrawalUnRealizedPNLActivityId
										ELSE @CDSShortUnRealizedPNLActivityId
										END
								END
					WHEN Asset = 'EquityOption'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @EquityOptionLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @EquityOptionLongWithdrawalUnRealizedPNLActivityId
											ELSE @EquityOptionLongUnRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @EquityOptionShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @EquityOptionShortWithdrawalUnRealizedPNLActivityId
										ELSE @EquityOptionShortUnRealizedPNLActivityId
										END
								END
					WHEN Asset = 'Future'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @FutureLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @FutureLongWithdrawalUnRealizedPNLActivityId
											ELSE @FutureLongUnRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @FutureShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @FutureShortWithdrawalUnRealizedPNLActivityId
										ELSE @FutureShortUnRealizedPNLActivityId
										END
								END
					WHEN Asset = 'FutureOption'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @FutureOptionLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @FutureOptionLongWithdrawalUnRealizedPNLActivityId
											ELSE @FutureOptionLongUnRealizedPNLActivitydId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @FutureOptionShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @FutureOptionShortWithdrawalUnRealizedPNLActivityId
										ELSE @FutureOptionShortUnRealizedPNLActivitydId
										END
								END
					WHEN Asset = 'FixedIncome'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @BondLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @BondLongWithdrawalUnRealizedPNLActivityId
											ELSE @BondLongUnRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @BondShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @BondShortWithdrawalUnRealizedPNLActivityId
										ELSE @BondShortUnRealizedPNLActivityId
										END
								END
					WHEN Asset = 'ConvertibleBond'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @ConvertibleBondLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @ConvertibleBondLongWithdrawalUnRealizedPNLActivityId
											ELSE @ConvertibleBondLongUnRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @ConvertibleBondShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @ConvertibleBondShortWithdrawalUnRealizedPNLActivityId
										ELSE @ConvertibleBondShortUnRealizedPNLActivityId
										END
								END
					WHEN Asset = 'FX'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @FXLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @FXLongWithdrawalUnRealizedPNLActivityId
											ELSE @FXLongUnRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @FXShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @FXShortWithdrawalUnRealizedPNLActivityId
										ELSE @FXShortUnRealizedPNLActivityId
										END
								END
					WHEN Asset = 'FXForward'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @FXForwardLongAdditionUnRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @FXForwardLongWithdrawalUnRealizedPNLActivityId
											ELSE @FXForwardLongUnRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @FXForwardShortAdditionUnRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @FXForwardShortWithdrawalUnRealizedPNLActivityId
										ELSE @FXForwardShortUnRealizedPNLActivityId
										END
								END
					END AS ActivityTypeId_FK
				,NULL AS FKID
				,FundID
				,9 AS TransactionSource
				,3 AS ActivitySource
				,Symbol
				,PNL AS Amount
				,CurrencyID
				,'UnRealized PNL Entry' AS Description
				,1 AS SideMultiplier
				,RunDate AS TradeDate
				,FxRate
				,'M' AS FXConversionMethodOperator
				,'New'
				,2
				,0
			FROM #UnRealizedPNLGrouped
				------------------------------end of entry for unrealized PNL-------------------------  
		END

		------==========================================================================================================---------
		BEGIN
			-------------------------------------Handling for Realized PNL-------------------------------  
			INSERT INTO [dbo].[#T_AllActivity] (
				[ActivityTypeId_FK]
				,[FKID]
				,[FundID]
				,[TransactionSource]
				,[ActivitySource]
				,[Symbol]
				,[Amount]
				,[CurrencyID]
				,[Description]
				,[SideMultiplier]
				,[TradeDate]
				,[FxRate]
				,[FXConversionMethodOperator]
				,[ActivityState]
				,[ActivityNumber]
				,[SubAccountID]
				)
			SELECT CASE 
					WHEN (
							Asset = 'Equity'
							OR Asset = 'PrivateEquity'
							)
						AND IsSwapped = 1
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @EquitySwapLongAdditionRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @EquitySwapLongWithdrawalRealizedPNLActivityId
											WHEN RealizedPNLLongOrShort ='Long Term'
												THEN @EquitySwapLongRealizedPNLLTActivityId
											WHEN RealizedPNLLongOrShort ='Short Term'
												THEN @EquitySwapLongRealizedPNLSTActivityId
											ELSE @EquitySwapRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @EquitySwapShortAdditionRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @EquitySwapShortWithdrawalRealizedPNLActivityId
										WHEN RealizedPNLLongOrShort='Short Term'
											THEN @EquitySwapShortRealizedPNLSTActvityId
										ELSE @EquitySwapRealizedPNLActivityId
										END
								END
					WHEN (Asset = 'Equity')
						AND IsSwapped = 0
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @EquityLongAdditionRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @EquityLongWithdrawalRealizedPNLActivityId
											WHEN RealizedPNLLongOrShort = 'Long Term'
												THEN @EquityLongRealizedPNLLTActivityId 
											WHEN RealizedPNLLongOrShort='Short Term'
												THEN @EquityLongRealizedPNLSTActivityId
											ELSE 
												@EquityLongRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @EquityShortAdditionRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @EquityShortWithdrawalRealizedPNLActivityId
										WHEN RealizedPNLLongOrShort='Short Term'
											 THEN @EquityShortRealizedPNLSTActivityId 
										ELSE @EquityShortRealizedPNLActivityId
										END
								END
					WHEN (Asset = 'PrivateEquity')
						AND IsSwapped = 0
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @PrivateEquityLongAdditionRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @PrivateEquityLongWithdrawalRealizedPNLActivityId
											WHEN RealizedPNLLongOrShort = 'Long Term'
												THEN @PrivateEquityLongRealizedPNLLTActivityId
											WHEN RealizedPNLLongOrShort='Short Term'
												THEN @PrivateEquityLongRealizedPNLSTActivityId
											ELSE @PrivateEquityLongRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @PrivateEquityShortAdditionRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @PrivateEquityShortWithdrawalRealizedPNLActivityId
										WHEN RealizedPNLLongOrShort='Short Term'
											 THEN @PrivateEquityShortRealizedPNLSTActivityId 
										ELSE @PrivateEquityShortRealizedPNLActivityId
										END
								END
					WHEN (Asset = 'CreditDefaultSwap')
						AND IsSwapped = 0
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @CDSLongAdditionRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @CDSLongWithdrawalRealizedPNLActivityId
											ELSE @CDSLongRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @CDSShortAdditionRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @CDSShortWithdrawalRealizedPNLActivityId
										ELSE @CDSShortRealizedPNLActivityId
										END
								END
					WHEN Asset = 'EquityOption'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @EquityOptionLongAdditionRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @EquityOptionLongWithdrawalRealizedPNLActivityId
											WHEN RealizedPNLLongOrShort = 'Long Term'
												THEN @EquityOptionLongRealizedPNLLTActivityId
											WHEN RealizedPNLLongOrShort = 'Short Term'
												THEN @EquityOptionLongRealizedPNLSTActivityId
											ELSE @EquityOptionLongRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @EquityOptionShortAdditionRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @EquityOptionShortWithdrawalRealizedPNLActivityId
										WHEN RealizedPNLLongOrShort='Short Term'
											THEN @EquityOptionShortRealizedPNLSTActivityId
										ELSE @EquityOptionShortRealizedPNLActivityId
										END
								END
					WHEN Asset = 'Future'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @FutureLongAdditionRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @FutureLongWithdrawalRealizedPNLActivityId
											WHEN RealizedPNLLongOrShort = 'Long Term'
												THEN @FutureLongRealizedPNLLTActivityId
											WHEN RealizedPNLLongOrShort = 'Short Term'
												THEN @FutureLongRealizedPNLSTActivityId
											ELSE @FutureRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @FutureShortAdditionRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @FutureShortWithdrawalRealizedPNLActivityId
										WHEN RealizedPNLLongOrShort = 'Long Term'
											THEN @FutureShortRealizedPNLLTActivityId
										WHEN RealizedPNLLongOrShort='Short Term'
											THEN @FutureShortRealizedPNLSTActivityId
										ELSE @FutureRealizedPNLActivityId
										END
								END
					WHEN Asset = 'FutureOption'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @FutureOptionLongAdditionRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @FutureOptionLongWithdrawalRealizedPNLActivityId
											WHEN RealizedPNLLongOrShort = 'Long Term'
												THEN @FutureOptionLongRealizedPNLLTActivityId
											WHEN RealizedPNLLongOrShort = 'Short Term'
												THEN @FutureOptionLongRealizedPNLSTActivityId
											ELSE @FutureOptionLongRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @FutureOptionShortAdditionRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @FutureOptionShortWithdrawalRealizedPNLActivityId
										WHEN RealizedPNLLongOrShort='Short Term'
											THEN @FutureOptionShortRealizedPNLSTActivityId
										ELSE @FutureOptionShortRealizedPNLActivityId
										END
								END
					WHEN Asset = 'FixedIncome'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @BondLongAdditionRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @BondLongWithdrawalRealizedPNLActivityId
											WHEN RealizedPNLLongOrShort = 'Long Term'
												THEN @BondLongRealizedPNLLTActivityId
											WHEN RealizedPNLLongOrShort = 'Short Term'
												THEN @BondLongRealizedPNLSTActivityId
											ELSE @BondLongRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @BondShortAdditionRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @BondShortWithdrawalRealizedPNLActivityId
										WHEN RealizedPNLLongOrShort='Short Term'
											THEN @BondShortRealizedPNLSTActivityId
										ELSE @BondShortRealizedPNLActivityId
										END
								END
					WHEN Asset = 'ConvertibleBond'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @ConvertibleBondLongAdditionRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @ConvertibleBondLongWithdrawalRealizedPNLActivityId
											ELSE @ConvertibleBondLongRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @ConvertibleBondShortAdditionRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @ConvertibleBondShortWithdrawalRealizedPNLActivityId
										ELSE @ConvertibleBondShortRealizedPNLActivityId
										END
								END
					WHEN Asset = 'FX'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @FXLongAdditionRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @FXLongWithdrawalRealizedPNLActivityId
											WHEN RealizedPNLLongOrShort = 'Long Term'
												THEN @FXLongRealizedPNLLTActivityId
											WHEN RealizedPNLLongOrShort = 'Short Term'
												THEN @FXLongRealizedPNLSTActivityId
											ELSE @FXLongRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @FXShortAdditionRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @FXShortWithdrawalRealizedPNLActivityId
										WHEN RealizedPNLLongOrShort='Short Term'
											THEN @FXShortRealizedPNLSTActivityId
										ELSE @FXShortRealizedPNLActivityId
										END
								END
					WHEN Asset = 'FXForward'
						THEN CASE 
								WHEN LongOrShort = 'Long'
									THEN CASE 
											WHEN TransactionType = 'LongAddition'
												THEN @FXForwardLongAdditionRealizedPNLActivityId
											WHEN TransactionType = 'LongWithdrawal'
												THEN @FXForwardLongWithdrawalRealizedPNLActivityId
											WHEN RealizedPNLLongOrShort = 'Long Term'
												THEN @FXForwardLongRealizedPNLLTActivityId
											WHEN RealizedPNLLongOrShort = 'Short Term'
												THEN @FXForwardLongRealizedPNLSTActivityId
											ELSE @FXForwardLongRealizedPNLActivityId
											END
								ELSE CASE 
										WHEN TransactionType = 'ShortAddition'
											THEN @FXForwardShortAdditionRealizedPNLActivityId
										WHEN TransactionType = 'ShortWithdrawal'
											THEN @FXForwardShortWithdrawalRealizedPNLActivityId
										WHEN RealizedPNLLongOrShort='Short Term'
											THEN @FXForwardShortRealizedPNLSTActivityId
										ELSE @FXForwardShortRealizedPNLActivityId
										END
								END
					END AS ActivityTypeId_FK
				,NULL AS FKID
				,FundID
				,9 AS TransactionSource
				,3 AS ActivitySource
				,Symbol
				,PNL AS Amount
				,CurrencyID
				,'Realized PNL Entry' AS Description
				,1 AS SideMultiplier
				,RunDate AS TradeDate
				,FxRate
				,'M' AS FXConversionMethodOperator
				,'New'
				,3
				,0
			FROM #RealizedPNLGrouped
				------------------------------end of entry for realized PNL-------------------------  
		END

		------==========================================================================================================---------
		INSERT INTO T_TempAllActivity
		SELECT *
		FROM #T_AllActivity
		ORDER BY TradeDate
			,ActivityNumber DESC

		RAISERROR (
				N'Activities Generated for Selected Funds.$%i$%i$Funds:%s'
				,0
				,1
				,@userID
				,@LogInFile
				,@fundIDs
				,@Symbols
				)
		WITH NOWAIT

		RAISERROR (
				N'Running Revaluation Process...$%i$%i'
				,0
				,1
				,@userID
				,@LogInFile
				)
		WITH NOWAIT	

		DROP TABLE #MarkPriceForStartDate
			,#Funds
			,#Symbols
			,#T_ActivityType
			,#MarkPriceForEndDate
			,#SecMasterDataTempTable
			,#T_CorpActionData
			,#Symbols_New,#symbolInfo,#PM_TaxlotsSelectedData,#OpenPositions

		DROP TABLE #AUECBusinessDatesForStartDate
			,#AUECBusinessDatesForEndDate
			,#BondFuturesMktValue
			,#PNLTable
			,#CurrencyIDs
			,#TempSplitFactorForOpen
			,#UnRealizedPNLGrouped
			
		DROP TABLE #UnRealizedPNLContraEntryTableGrouped
			,#TempClosingData
			,#RealizedPNL
			,#UnRealizedPNL
			,#T_AllActivity

		DROP TABLE #UnRealizedPNLContraEntryTable
			,#RealizedPNLGrouped
			,#FutureRealizedPNLGrouped
			,#FXConversionRates
			,#ZeroFundFxRate
			,#ZeroFundMarkPriceStartDate
			,#ZeroFundMarkPriceEndDate
			,#Group
			,#OriginalFunds
			,#SameDayClosedSymbols
			,#DistinctMarkPriceForStartDate
			,#DistinctMarkPriceForEndDate                        
			,#Fund_WithZeroFundId
	END TRY

	BEGIN CATCH
		DECLARE @ErrMsg NVARCHAR(4000)

		SELECT @ErrMsg = ERROR_MESSAGE() + '$' + CONVERT(VARCHAR(10), @userID)+ '$' +CONVERT(VARCHAR(10), @LogInFile)

		RAISERROR (
				@ErrMsg
				,0
				,1			
				)
		WITH NOWAIT
	END CATCH
END