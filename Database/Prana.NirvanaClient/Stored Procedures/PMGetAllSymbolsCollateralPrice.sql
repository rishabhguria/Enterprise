CREATE PROCEDURE [dbo].[PMGetAllSymbolsCollateralPrice]    (
	@Date DATETIME
	,@Type INT -- 0 for Same Date,1 for Week , 2 for Month
	,@ErrorMessage VARCHAR(500) OUTPUT
	,@ErrorNumber INT OUTPUT
	,@GetSameDayClosedData BIT
	,@IsOnlyFixedIncomeSymbols BIT
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
		,UnderlyingSymbol
		,AUECID
		,AssetID
		,BloombergSymbol
	INTO #SecMasterData
	FROM V_SecMasterData

	--Getting SecMaster Underlying Details
	SELECT TickerSymbol
		,UnderlyingSymbol
		,AUECID
		,AssetID
		,BloombergSymbol
	INTO #SecMasterDataWithUnderlying
	FROM V_SecMasterData_WithUnderlying

	CREATE TABLE #PM_Taxlots (
		Symbol VARCHAR(200)
		,FundId INT
		,GroupID VARCHAR(100)
		)
		
	declare @AllowEntryTillSettlementDate INT
	Select @AllowEntryTillSettlementDate = CONVERT(INT, PreferenceValue)        
	FROM T_PranaKeyValuePreferences        
	WHERE PreferenceKey = 'IsPriceEnterTillSettlementDateInDailyValuation'

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
				,FundId
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

	SELECT PT.Symbol
		,G.AUECLocalDate AS Date_Associated
		,SM.AUECID AS AUECID
		,AUEC.ExchangeIdentifier AS AUECIdentifier
		,SM.AssetID AS AssetID
		,SM.BloombergSymbol AS BloombergSymbol
		,PT.FundId AS FundId
	INTO #TempPositionsAndAllocatedTradesALL
	FROM #PM_Taxlots PT
	LEFT JOIN T_Group G ON G.GroupID = PT.GroupID
	LEFT JOIN #SecMasterData SM ON PT.Symbol = SM.TickerSymbol
	LEFT JOIN #T_AUEC AUEC ON G.AUECID = AUEC.AUECID
	
	IF(@IsOnlyFixedIncomeSymbols =1)
	BEGIN
		DELETE FROM #TempPositionsAndAllocatedTradesALL where AssetID NOT IN (8,13)
	END
	ELSE
	BEGIN
	--Adding Underlying Symbols for Allocated Symbols
		INSERT INTO #TempPositionsAndAllocatedTradesALL (
			Symbol
			,Date_Associated
			,AUECID
			,AUECIdentifier
			,BloombergSymbol
			,AssetID
			,FundId
			)
		SELECT SecMaster.UnderlyingSymbol AS Symbol
			,G.AUECLocalDate AS Date_Associated
			,SecMasterWithUnderlying.AUECID AS AUEDID
			,AUEC.ExchangeIdentifier AS AUECIdentifier
			,SecMasterWithUnderlying.BloombergSymbol AS BloombergSymbol
			,SecMasterWithUnderlying.AssetID AS AssetID
			,PT.FundId AS FundId
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
	END

	SET TRANSACTION ISOLATION LEVEL READ COMMITTED

	CREATE TABLE [dbo].#TempPositionsAndAllocatedTradesAllFinal (
		Symbol VARCHAR(200)
		,CollateralPrice NUMERIC(18, 4)
		,Haircut  NUMERIC(18, 4)
		,RebateOnMV  NUMERIC(18, 4)
		,RebateOnCollateral  NUMERIC(18, 4)
		,DayCollateralPriceID INT
		,Date1 DATETIME
		,AUECID INT
		,AUECIdentifier VARCHAR(200)
		,AssetID INT
		,BloombergSymbol NVARCHAR(200)
		,FundId INT
		)

	INSERT INTO [dbo].#TempPositionsAndAllocatedTradesAllFinal (
		Symbol
		,Date1
		,AUECID
		,AUECIdentifier
		,AssetID
		,BloombergSymbol
		,FundId
		)
	SELECT SYMBOL
		,MAX(Date_Associated)
		,MAX(TPAATA.AUECID)
		,MAX(TPAATA.AUECIdentifier)
		,MAX(AssetID)
		,MAX(TPAATA.BloombergSymbol)
		,FundId
	FROM #TempPositionsAndAllocatedTradesALL TPAATA
	GROUP BY Symbol
		,FundId

--We are just taking safe side by taking 35 days            
	SELECT TOP 35 AllDates.Items
	INTO #TempDates
	FROM dbo.GetDateRange(@FirstDateofMonth, @LastDateofMonth) AS AllDates
	ORDER BY AllDates.Items DESC

	SELECT *
	INTO #TempPositionsAndAllocatedTradesAllFinalNew
	FROM #TempDates AS TEMP
		,#TempPositionsAndAllocatedTradesAllFinal AS TPATAF

	insert into #TempPositionsAndAllocatedTradesAllFinalNew
	select Items,Symbol,CollateralPrice,Haircut,RebateOnMV,RebateOnCollateral,DayCollateralPriceID,Date1,
	AUECID,AUECIdentifier,AssetID,BloombergSymbol,0 from #TempPositionsAndAllocatedTradesAllFinalNew

	SELECT Symbol
		,MAX(DATE) AS DATE
		,FundID
	INTO #TempSymbolMaxDate
	FROM PM_DailyCollateralPrice
	WHERE (
			CollateralPrice <> 0
			OR Haircut <> 0
			OR RebateOnMV <> 0
			OR RebateOnCollateral <> 0
			)
		AND DATEDIFF(d, @Date, DATE) = 0
	GROUP BY Symbol
		,FundID

	
		SELECT DISTINCT TPATAF.Symbol
			,ISNULL(PMDMP1.DATE, TPATAF.Items) AS DATE
			,ISNULL(PMDMP1.CollateralPrice, 0) AS CollateralPrice
			,TPATAF.AUECID
			,TPATAF.AUECIdentifier
			,TPATAF.BloombergSymbol
			,ISNULL(PMDMP1.Haircut, 0) AS Haircut
			,ISNULL(PMDMP1.RebateOnMV, 0) AS RebateOnMV
			,ISNULL(PMDMP1.RebateOnCollateral, 0) AS RebateOnCollateral
			,TPATAF.AssetID
			,TPATAF.FundID
		FROM #TempPositionsAndAllocatedTradesAllFinalNew AS TPATAF
		LEFT JOIN (
			SELECT PMDMP.Symbol
				,PMDMP.CollateralPrice
				,PMDMP.DATE
				,PMDMP.Haircut
				,PMDMP.RebateOnMV
				,PMDMP.RebateOnCollateral
				,PMDMP.FundID
			FROM PM_DailyCollateralPrice PMDMP
			INNER JOIN #TempSymbolMaxDate symbolDate ON DATEDIFF(d, symbolDate.DATE, PMDMP.DATE) = 0
				AND symbolDate.Symbol = PMDMP.Symbol
				AND symbolDate.FundID = PMDMP.FundID
			) PMDMP1 ON TPATAF.Symbol = PMDMP1.Symbol
			AND TPATAF.FundID = PMDMP1.FundID
		ORDER BY TPATAF.Symbol
			,DATE DESC
	

--To here
	
	DROP TABLE #PM_Taxlots
		,#T_AUEC
		,#SecMasterData
		,#TempDates
		,#TempSymbolMaxDate

	DROP TABLE #TempPositionsAndAllocatedTradesALL
		,#TempPositionsAndAllocatedTradesAllFinal
		,#TempPositionsAndAllocatedTradesAllFinalNew
		,#SecMasterDataWithUnderlying
END TRY

BEGIN CATCH
	SET @ErrorMessage = ERROR_MESSAGE();
	SET @ErrorNumber = Error_number();
END CATCH;