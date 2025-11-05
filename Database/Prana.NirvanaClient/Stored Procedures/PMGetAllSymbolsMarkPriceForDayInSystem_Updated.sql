CREATE PROCEDURE [dbo].[PMGetAllSymbolsMarkPriceForDayInSystem_Updated] (
	@Date DATETIME
	,@Type INT -- 0 for Same Date,1 for Week , 2 for Month
	,@isFxFXForwardData BIT
	,@ErrorMessage VARCHAR(500) OUTPUT
	,@ErrorNumber INT OUTPUT
	,@GetSameDayClosedData BIT
	,@IsFundWiseMarkPrice BIT -- 0 for Not fund wise mark price, 1 For fund wise mark price
	)
AS
DECLARE @FirstDateofMonth VARCHAR(50)
DECLARE @LastDateofMonth VARCHAR(50)

IF (@Type = 0) -- Daily view
BEGIN
	SET @FirstDateofMonth = CONVERT(VARCHAR(25), @Date, 101)
	SET @LastDateofMonth = CONVERT(VARCHAR(25), @Date, 101)
END
ELSE IF (@Type = 1) -- Weekly view
BEGIN
	SET @FirstDateofMonth = CONVERT(VARCHAR(25), @Date, 101)
	SET @LastDateofMonth = CONVERT(VARCHAR(25), @Date, 101)
END
ELSE IF (@Type = 2) -- Monthly view
BEGIN
	SET @FirstDateofMonth = CONVERT(VARCHAR(25), DATEADD(dd, - (DAY(@Date) - 1), @Date), 101)
	SET @LastDateofMonth = CONVERT(VARCHAR(25), DATEADD(dd, - (DAY(DATEADD(mm, 1, @Date))), DATEADD(mm, 1, @Date)), 101)
END

SET @ErrorMessage = 'Success'
SET @ErrorNumber = 0

BEGIN TRY
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	--Getting AUEC Details
	SELECT AuecID
		,ExchangeIdentifier
	INTO #T_AUEC
	FROM T_AUEC

	--Getting SecMaster Details
	SELECT TickerSymbol
		,LeadCurrencyID
		,VsCurrencyID
		,UnderlyingSymbol
		,AUECID
		,AssetID
		,BloombergSymbol
	INTO #SecMasterData
	FROM V_SecMasterData

	--Getting SecMaster Underlying Details
	SELECT TickerSymbol
		,LeadCurrencyID
		,VsCurrencyID
		,UnderlyingSymbol
		,AUECID
		,AssetID
		,BloombergSymbol
	INTO #SecMasterDataWithUnderlying
	FROM V_SecMasterData_WithUnderlying

	CREATE TABLE #PM_Taxlots (
		Symbol VARCHAR(200)
		,FundID INT
		,GroupID VARCHAR(100)
		)

	declare @AllowEntryTillSettlementDate INT
	Select @AllowEntryTillSettlementDate = CONVERT(INT, PreferenceValue)        
	FROM T_PranaKeyValuePreferences        
	WHERE PreferenceKey = 'IsPriceEnterTillSettlementDateInDailyValuation'
	
	-- Not Fund Wise Mark Price
	IF (@IsFundWiseMarkPrice = 0)
	BEGIN
		--Getting All Open Taxlots
		INSERT INTO #PM_Taxlots
		SELECT Symbol
			,0
			,GroupID
		FROM PM_Taxlots
		WHERE TaxLotOpenQty <> 0
			AND Taxlot_PK IN (
				SELECT max(Taxlot_PK)
				FROM PM_Taxlots
				WHERE DateDiff(d, PM_Taxlots.AUECModifiedDate, @LastDateofMonth) >= 0
				GROUP BY TaxLotID
				)
		GROUP BY Symbol
			,GroupID

		IF (@GetSameDayClosedData = 1)
		BEGIN
			--Getting All Closed Taxlots on last date of month            
			INSERT INTO #PM_Taxlots
			SELECT Symbol
				,0
				,GroupID
			FROM PM_Taxlots
			WHERE TaxLotOpenQty = 0
				AND Taxlot_PK IN (
					SELECT max(Taxlot_PK)
					FROM PM_Taxlots
					WHERE DateDiff(d, PM_Taxlots.AUECModifiedDate, @LastDateofMonth) = 0
					GROUP BY TaxLotID
					)
			GROUP BY Symbol
				,GroupID
		END

		if (@AllowEntryTillSettlementDate=1)
		BEGIN
			INSERT INTO #PM_Taxlots
			SELECT PT.Symbol
				,0
				,PT.GroupID
			FROM PM_Taxlots PT
			Inner Join T_Group G on G.GroupID = PT.GroupID AND DateDiff(d, G.SettlementDate, @LastDateofMonth) <= 0 AND DateDiff(d, G.AllocationDate, @LastDateofMonth) >= 0
		END
	END
			--Fund Wise Mark Price
	ELSE
	BEGIN
		--Getting All Open Taxlots
		INSERT INTO #PM_Taxlots
		SELECT Symbol
			,FundID
			,GroupID
		FROM PM_Taxlots PT
		LEFT JOIN T_CompanyFunds CF on CF.CompanyFundID=PT.FundId
		WHERE (CF.IsActive=1 OR CF.IsActive is null)
		 AND TaxLotOpenQty <> 0
			AND Taxlot_PK IN (
				SELECT max(Taxlot_PK)
				FROM PM_Taxlots
				WHERE DateDiff(d, PM_Taxlots.AUECModifiedDate, @LastDateofMonth) >= 0
				GROUP BY TaxLotID
				)
		GROUP BY Symbol
			,FundID
			,GroupID

		IF (@GetSameDayClosedData = 1)
		BEGIN
			--Getting All Closed Taxlots on last date of month      
			INSERT INTO #PM_Taxlots
			SELECT Symbol
				,FundID
				,GroupID
			FROM PM_Taxlots PT
		LEFT JOIN T_CompanyFunds CF on CF.CompanyFundID=PT.FundId
		WHERE (CF.IsActive=1 OR CF.IsActive is null)
		 AND TaxLotOpenQty = 0
				AND Taxlot_PK IN (
					SELECT max(Taxlot_PK)
					FROM PM_Taxlots
					WHERE DateDiff(d, PM_Taxlots.AUECModifiedDate, @LastDateofMonth) = 0
					GROUP BY TaxLotID
					)
			GROUP BY Symbol
				,FundID
				,GroupID
		END

		if (@AllowEntryTillSettlementDate=1)
		BEGIN
			INSERT INTO #PM_Taxlots
			SELECT PT.Symbol
				,PT.FundID
				,PT.GroupID
			FROM PM_Taxlots PT
			Inner Join T_Group G on G.GroupID = PT.GroupID AND DateDiff(d, G.SettlementDate, @LastDateofMonth) <= 0 AND DateDiff(d, G.AllocationDate, @LastDateofMonth) >= 0
		END

		--Copying All Open Taxlots with 0 FundID
		SELECT *
		INTO #PM_Taxlots_Temp
		FROM #PM_Taxlots

		INSERT INTO #PM_Taxlots (
			Symbol
			,FundID
			,GroupID
			)
		SELECT Symbol
			,0
			,GroupID
		FROM #PM_Taxlots_Temp

		DROP TABLE #PM_Taxlots_Temp
	END

	-- Getting All Unallocate Symbols
	SELECT DISTINCT Symbol
		,AUECLocalDate
		,AUECID
		,AssetID
	INTO #TempUnallocateTGroup
	FROM T_Group
	WHERE (
			DateDiff(day, T_Group.AUECLocaldate, @LastDateofMonth) >= 0
			AND T_Group.StateID = 1
			AND T_Group.CumQty <> 0
			)

	--Collecting all open traded symbols
	SELECT GP.Symbol AS Symbol
		,GP.AUECLocalDate AS Date_Associated
		,GP.AUECID AS AUECID
		,AUEC.ExchangeIdentifier AS AUECIdentifier
		,GP.AssetID AS AssetID
		,SM.LeadCurrencyID AS LeadCurrencyID
		,SM.VsCurrencyID AS VsCurrencyID
		,SM.BloombergSymbol AS BloombergSymbol
		,0 AS FundID
	INTO #TempPositionsAndAllocatedTradesALL
	FROM #TempUnallocateTGroup GP
	LEFT JOIN #T_AUEC AUEC ON GP.AUECID = AUEC.AUECID
	LEFT JOIN #SecMasterData SM ON GP.Symbol = SM.TickerSymbol

	INSERT INTO #TempPositionsAndAllocatedTradesALL (
		Symbol
		,Date_Associated
		,AUECID
		,AUECIdentifier
		,AssetID
		,LeadCurrencyID
		,VsCurrencyID
		,BloombergSymbol
		,FundID
		)
	SELECT PMD.Symbol AS Symbol
		,PMD.DATE AS Date_Associated
		,0 AS AUECID
		,'Indices-Indices' AS AUECIdentifier
		,0 AS AssetID
		,0 AS LeadCurrencyID
		,0 AS VsCurrencyID
		,SM.BloombergSymbol AS BloombergSymbol
		,0 AS FundID
	FROM PM_DayMarkPrice PMD
	INNER JOIN #SecMasterData SM ON SM.TickerSymbol = PMD.Symbol
	WHERE DATEDIFF(d, PMD.DATE, @LastDateofMonth) >= 0
		AND SM.AssetID = 7

	INSERT INTO #TempPositionsAndAllocatedTradesALL (
		Symbol
		,Date_Associated
		,AUECID
		,AUECIdentifier
		,AssetID
		,LeadCurrencyID
		,VsCurrencyID
		,BloombergSymbol
		,FundID
		)
	SELECT PT.Symbol
		,G.AUECLocalDate AS Date_Associated
		,SM.AUECID AS AUEDID
		,AUEC.ExchangeIdentifier AS AUECIdentifier
		,SM.AssetID AS AssetID
		,SM.LeadCurrencyID AS LeadCurrencyID
		,SM.VsCurrencyID AS VsCurrencyID
		,SM.BloombergSymbol AS BloombergSymbol
		,PT.FundID AS FundID
	FROM #PM_Taxlots PT
	LEFT JOIN T_Group G ON G.GroupID = PT.GroupID
	LEFT JOIN #SecMasterData SM ON PT.Symbol = SM.TickerSymbol
	LEFT JOIN #T_AUEC AUEC ON G.AUECID = AUEC.AUECID

	--Adding Underlying Symbols for Allocated Symbols
	INSERT INTO #TempPositionsAndAllocatedTradesALL (
		Symbol
		,Date_Associated
		,AUECID
		,AUECIdentifier
		,BloombergSymbol
		,AssetID
		,LeadCurrencyID
		,VsCurrencyID
		,FundID
		)
	SELECT SecMaster.UnderlyingSymbol AS Symbol
		,G.AUECLocalDate AS Date_Associated
		,SecMasterWithUnderlying.AUECID AS AUEDID
		,AUEC.ExchangeIdentifier AS AUECIdentifier
		,SecMasterWithUnderlying.BloombergSymbol AS BloombergSymbol
		,SecMasterWithUnderlying.AssetID AS AssetID
		,SecMasterWithUnderlying.LeadCurrencyID AS LeadCurrencyID
		,SecMasterWithUnderlying.VsCurrencyID AS VsCurrencyID
		,PT.FundID AS FundID
	FROM #PM_Taxlots PT
	LEFT JOIN T_Group G ON G.GroupID = PT.GroupID
	LEFT JOIN #SecMasterData SecMaster ON PT.Symbol = SecMaster.TickerSymbol
	LEFT JOIN #SecMasterDataWithUnderlying SecMasterWithUnderlying ON SecMaster.UnderlyingSymbol = SecMasterWithUnderlying.TickerSymbol
	LEFT JOIN #T_AUEC AUEC ON SecMasterWithUnderlying.AUECID = AUEC.AUECID
	WHERE G.AssetID IN (
			2
			,3
			,4
			,10
			)

	--Adding Underlying Symbols for Unallocated Symbols
	INSERT INTO #TempPositionsAndAllocatedTradesALL (
		Symbol
		,Date_Associated
		,AUECID
		,AUECIdentifier
		,BloombergSymbol
		,AssetID
		,LeadCurrencyID
		,VsCurrencyID
		,FundID
		)
	SELECT SecMaster.UnderlyingSymbol AS Symbol
		,GP.AUECLocalDate AS Date_Associated
		,SecMasterWithUnderlying.AUECID AS AUEDID
		,AUEC.ExchangeIdentifier AS AUECIdentifier
		,SecMasterWithUnderlying.BloombergSymbol AS BloombergSymbol
		,SecMasterWithUnderlying.AssetID AS AssetID
		,SecMasterWithUnderlying.LeadCurrencyID AS LeadCurrencyID
		,SecMasterWithUnderlying.VsCurrencyID AS VsCurrencyID
		,0 AS FundID
	FROM #TempUnallocateTGroup GP
	LEFT JOIN #SecMasterData SecMaster ON GP.Symbol = SecMaster.TickerSymbol
	LEFT JOIN #SecMasterDataWithUnderlying SecMasterWithUnderlying ON SecMaster.UnderlyingSymbol = SecMasterWithUnderlying.TickerSymbol
	LEFT JOIN #T_AUEC AUEC ON SecMasterWithUnderlying.AUECID = AUEC.AUECID
	WHERE GP.AssetID IN (
			2
			,3
			,4
			,10
			)

	SET TRANSACTION ISOLATION LEVEL READ COMMITTED

	CREATE TABLE [dbo].#TempPositionsAndAllocatedTradesAllFinal (
		Symbol VARCHAR(200)
		,ApplicationMarkPrice NUMERIC(18, 4)
		,FinalMarkPrice NUMERIC(18, 4)
		,DayMarkPriceID INT
		,Date1 DATETIME
		,AUECID INT
		,AUECIdentifier VARCHAR(200)
		,ForwardPoints NUMERIC(18, 4)
		,AssetID INT
		,LeadCurrencyID INT
		,VsCurrencyID INT
		,BloombergSymbol NVARCHAR(200)
		,FundID INT
		)

	INSERT INTO [dbo].#TempPositionsAndAllocatedTradesAllFinal (
		Symbol
		,Date1
		,AUECID
		,AUECIdentifier
		,AssetID
		,LeadCurrencyID
		,VsCurrencyID
		,BloombergSymbol
		,FundID
		)
	SELECT SYMBOL
		,MAX(Date_Associated)
		,MAX(TPAATA.AUECID)
		,MAX(TPAATA.AUECIdentifier)
		,MAX(AssetID)
		,MAX(LeadCurrencyID)
		,MAX(VsCurrencyID)
		,MAX(TPAATA.BloombergSymbol)
		,FundID
	FROM #TempPositionsAndAllocatedTradesALL TPAATA
	GROUP BY Symbol
		,FundID

	--We are just taking safe side by taking 35 days            
	SELECT TOP 35 AllDates.Items
	INTO #TempDates
	FROM dbo.GetDateRange(@FirstDateofMonth, @LastDateofMonth) AS AllDates
	ORDER BY AllDates.Items DESC

	SELECT *
	INTO #TempPositionsAndAllocatedTradesAllFinalNew
	FROM #TempDates AS TEMP
		,#TempPositionsAndAllocatedTradesAllFinal AS TPATAF

	SELECT Symbol
		,MAX(DATE) AS DATE
		,FundID
	INTO #TempSymbolMaxDate
	FROM PM_DayMarkPrice
	WHERE (
			(FinalMarkPrice <> 0)
			OR (
				FinalMarkPrice = 0
				OR ForwardPoints <> 0
				)
			)
		AND DATEDIFF(d, @Date, DATE) <= 0
	GROUP BY Symbol
		,FundID

	IF (@Type = 2)
	BEGIN
		SELECT DISTINCT TPATAF.Symbol
			,TPATAF.Items AS DATE
			,ISNULL(PMDMP.FinalMarkPrice, 0) AS FinalMarkPrice
			,TPATAF.AUECID
			,TPATAF.AUECIdentifier
			,CASE 
				WHEN DATEDIFF(d, PMDMP.DATE, TPATAF.Items) = 0
					THEN 0
				ELSE 1
				END AS MarkPriceIndicator
			,ISNULL(PMDMP.ForwardPoints, 0) AS ForwardPoints
			,TPATAF.AssetID
			,TPATAF.LeadCurrencyID
			,TPATAF.VsCurrencyID
			,TPATAF.BloombergSymbol
			,TPATAF.FundID
		FROM #TempPositionsAndAllocatedTradesAllFinalNew AS TPATAF
		LEFT JOIN PM_DayMarkPRice PMDMP ON TPATAF.Symbol = PMDMP.Symbol
			AND TPATAF.FundID = PMDMP.FundID
			AND DATEDIFF(d, TPATAF.Items, PMDMP.DATE) = 0
		ORDER BY TPATAF.Symbol
			,DATE DESC
	END
	ELSE
	BEGIN
		SELECT DISTINCT TPATAF.Symbol
			,ISNULL(PMDMP1.DATE, TPATAF.Items) AS DATE
			,ISNULL(PMDMP1.FinalMarkPrice, 0) AS FinalMarkPrice
			,TPATAF.AUECID
			,TPATAF.AUECIdentifier
			,TPATAF.BloombergSymbol
			,CASE 
				WHEN DATEDIFF(d, PMDMP1.DATE, TPATAF.Items) = 0
					THEN 0
				ELSE 1
				END AS MarkPriceIndicator
			,ISNULL(PMDMP1.ForwardPoints, 0) AS ForwardPoints
			,TPATAF.AssetID
			,TPATAF.LeadCurrencyID
			,TPATAF.VsCurrencyID
			,TPATAF.FundID
		INTO #TempWithSecMasterJoin
		FROM #TempPositionsAndAllocatedTradesAllFinalNew AS TPATAF
		LEFT JOIN (
			SELECT PMDMP.Symbol
				,PMDMP.FinalMarkPrice
				,PMDMP.DATE
				,PMDMP.ForwardPoints
				,PMDMP.FundID
			FROM PM_DayMarkPRice PMDMP
			INNER JOIN #TempSymbolMaxDate symbolDate ON DATEDIFF(d, symbolDate.DATE, PMDMP.DATE) = 0
				AND symbolDate.Symbol = PMDMP.Symbol
				AND symbolDate.FundID = PMDMP.FundID
			) PMDMP1 ON TPATAF.Symbol = PMDMP1.Symbol
			AND TPATAF.FundID = PMDMP1.FundID
		ORDER BY TPATAF.Symbol
			,DATE DESC
	END

	IF (@isFxFXForwardData = 1)
	BEGIN
		SELECT FromCurrencyID
			,ToCurrencyID
			,RateValue AS FxRate
			,ConversionMethod
			,FundID
		INTO #FXRates
		FROM GetFXConversionRatesForDate(@Date, 0)

		SELECT Final.Symbol
			,Final.DATE
			,Final.FinalMarkPrice
			,Final.AUECID
			,Final.AUECIdentifier
			,Final.MarkPriceIndicator
			,Final.BloombergSymbol
			,ISNULL(Final.ForwardPoints, 0) AS ForwardPoints
			,CAST(CASE 
					WHEN FXRates.ConversionMethod = 1
						AND FXRates.FxRate <> 0
						THEN 1 / FXRates.FxRate
					ELSE FXRates.FxRate
					END AS FLOAT) AS FxRate
			,Final.AssetID
			,Final.LeadCurrencyID
			,Final.VsCurrencyID
			,Final.FundID
		FROM #TempWithSecMasterJoin AS Final
		LEFT JOIN #SecMasterData secMasterData ON Symbol = secMasterData.TickerSymbol
		LEFT JOIN #FXRates FXRates ON FXRates.FromCurrencyID = secMasterData.LeadCurrencyID
			AND FXRates.ToCurrencyID = secMasterData.VsCurrencyID
			AND FXRates.FundID = Final.FundID
		WHERE (
				secMasterData.AssetID = 5
				OR secMasterData.AssetID = 11
				)
	END

	IF (
			@isFxFXForwardData = 0
			AND @type <> 2
			)
	BEGIN
		SELECT *
		FROM #TempWithSecMasterJoin

		DROP TABLE #TempWithSecMasterJoin
	END
	ELSE IF (@isFxFXForwardData = 1)
	BEGIN
		DROP TABLE #FXRates
	END

	DROP TABLE #PM_Taxlots
		,#T_AUEC
		,#SecMasterData

	DROP TABLE #TempPositionsAndAllocatedTradesALL
		,#TempPositionsAndAllocatedTradesAllFinal
		,#TempPositionsAndAllocatedTradesAllFinalNew
		,#TempDates
		,#TempSymbolMaxDate
		,#TempUnallocateTGroup
		,#SecMasterDataWithUnderlying
END TRY

BEGIN CATCH
	SET @ErrorMessage = ERROR_MESSAGE();
	SET @ErrorNumber = Error_number();
END CATCH;
