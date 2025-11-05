CREATE PROCEDURE [dbo].[PMGetAllSymbolsBeta] (
	@FromDate DATETIME
	,@ToDate DATETIME
	,@Type INT
	,-- 0 for Same Date,1 for Week , 2 for Month                                          
	@ErrorMessage VARCHAR(500) OUTPUT
	,@ErrorNumber INT OUTPUT
	)
AS
DECLARE @FirstDateofMonth VARCHAR(50)
DECLARE @LastDateofMonth VARCHAR(50)

IF (@Type = 0) -- Daily view                          
BEGIN
	SET @FirstDateofMonth = CONVERT(VARCHAR(25), @fromDate, 101)
	SET @LastDateofMonth = CONVERT(VARCHAR(25), @fromDate, 101)
END
ELSE IF (@Type = 1) -- Weekly view                          
BEGIN
	SET @FirstDateofMonth = CONVERT(VARCHAR(25), @fromDate, 101)
	SET @LastDateofMonth = CONVERT(VARCHAR(25), @ToDate, 101)
END
ELSE IF (@Type = 2) -- Monthly view                          
BEGIN
	SET @FirstDateofMonth = CONVERT(VARCHAR(25), DATEADD(dd, - (DAY(@fromDate) - 1), @fromDate), 101)
	SET @LastDateofMonth = CONVERT(VARCHAR(25), DATEADD(dd, - (DAY(DATEADD(mm, 1, @fromDate))), DATEADD(mm, 1, @fromDate)), 101) -- CONVERT(VARCHAR(25),GetUTcDate(),101)--                                               
END

SET @ErrorMessage = 'Success'
SET @ErrorNumber = 0

BEGIN TRY
	CREATE TABLE #PM_Taxlots (
		GroupID VARCHAR(50)
		,Symbol VARCHAR(100)
		)

	INSERT INTO #PM_Taxlots (
		GroupID
		,Symbol
		)
	SELECT GroupID
		,Symbol
	FROM PM_Taxlots
	WHERE TaxLotOpenQty <> 0
		AND Taxlot_PK IN (
			SELECT max(Taxlot_PK)
			FROM PM_Taxlots
			WHERE DateDiff(d, PM_Taxlots.AUECModifiedDate, @LastDateofMonth) >= 0
			GROUP BY taxlotid
			)

	CREATE TABLE #T_AUEC (
		AuecID INT
		,ExchangeIdentifier VARCHAR(100)
		)

	INSERT INTO #T_AUEC (
		AuecID
		,ExchangeIdentifier
		)
	SELECT AuecID
		,ExchangeIdentifier
	FROM T_AUEC

	CREATE TABLE #SecMasterData (
		TickerSymbol VARCHAR(100)
		,UnderlyingSymbol VARCHAR(100)
		,AUECID INT
		,AssetID INT
		,BloombergSymbol VARCHAR(100)
		)

	INSERT INTO #SecMasterData
	SELECT TickerSymbol
		,UnderlyingSymbol
		,AUECID
		,AssetID
		,BloombergSymbol
	FROM V_SecMasterData

	CREATE TABLE [dbo].#TempPositionsAndAllocatedTradesALL (
		Symbol VARCHAR(200)
		,DayBetaID INT
		,Date_Associated DATETIME
		,AUECID INT
		,AUECIdentifier VARCHAR(200)
		,AssetID INT
		,BloombergSymbol NVARCHAR(200)
		)

	INSERT INTO #TempPositionsAndAllocatedTradesALL (
		Symbol
		,Date_Associated
		,AUECID
		,AUECIdentifier
		,AssetID
		,BloombergSymbol
		)
	SELECT DISTINCT T_Group.Symbol AS Symbol
		,MAX(T_Group.AUECLocalDate) AS Date_Associated
		,MAX(T_Group.AUECID) AS AUECID
		,MAX(AUEC.ExchangeIdentifier) AS AUECIdentifier
		,MAX(T_Group.AssetID) AS AssetID
		,MAX(SM.BloombergSymbol) AS BloombergSymbol
	FROM [dbo].T_Group
	LEFT JOIN #T_AUEC AUEC ON T_Group.AUECID = AUEC.AUECID
	LEFT JOIN #SecMasterData SM ON T_Group.Symbol = SM.TickerSymbol
	WHERE (
			(
				DateDiff(day, T_Group.AUECLocaldate, @LastDateofMonth) >= 0
				AND T_Group.StateID = 1
				)
			OR (
				DateDiff(day, T_Group.AUECLocaldate, @FirstDateofMonth) <= 0
				AND datediff(day, T_Group.AUECLocaldate, @LastDateofMonth) >= 0
				)
			)
--Atul: T_Group.AssetID Not In(5,11) This Check is added to remove  FX/FXFWD Symbols  from DailyBeta Tab in Daily Valuation 
		AND T_Group.AssetID NOT IN (
			5
			,11
			)
	GROUP BY Symbol

	-- collect open trade symbols                                            
	INSERT INTO #TempPositionsAndAllocatedTradesALL (
		Symbol
		,Date_Associated
		,AUECID
		,AUECIdentifier
		,AssetID
		,BloombergSymbol
		)
	SELECT DISTINCT PT.Symbol
		,MAX(G.AUECLocalDate) AS Date_Associated
		,MAX(G.AUECID) AS AUEDID
		,MAX(AUEC.ExchangeIdentifier) AS AUECIdentifier
		,MAX(G.AssetID) AS AssetID
		,MAX(SM.BloombergSymbol) AS BloombergSymbol
	FROM #PM_Taxlots PT
	INNER JOIN T_Group G ON G.GroupID = PT.GroupID
	LEFT JOIN #SecMasterData SM ON PT.Symbol = SM.TickerSymbol
	LEFT JOIN #T_AUEC AUEC ON G.AUECID = AUEC.AUECID
--Atul: T_Group.AssetID Not In(5,11) This Check is added to remove  FX/FXFWD Symbols  from DailyBeta Tab in Daily Valuation 
	WHERE G.AssetID NOT IN (
			5
			,11
			)
	GROUP BY PT.Symbol

	-- In case of options, we also need to fetch the underlying symbol mark as these would be utilized in no of places                                     
	--getting underlying symbols                                       
	INSERT INTO #TempPositionsAndAllocatedTradesALL (
		Symbol
		,Date_Associated
		,AUECID
		,AUECIdentifier
		,BloombergSymbol
		)
	SELECT DISTINCT MAX(SecMaster.UnderlyingSymbol) AS Symbol
		,MAX(T_Group.AUECLocalDate) AS Date_Associated
		,MAX(SecMasterWithUnderlying.AUECID) AS AUECID
		,MAX(AUEC.ExchangeIdentifier) AS AUECIdentifier
		,max(SecMasterWithUnderlying.BloombergSymbol) AS BloombergSymbol
	FROM [dbo].T_Group
	LEFT JOIN #SecMasterData SecMaster ON T_Group.Symbol = SecMaster.TickerSymbol
	LEFT JOIN V_SecMasterData_WithUnderlying SecMasterWithUnderlying ON SecMaster.UnderlyingSymbol = SecMasterWithUnderlying.TickerSymbol
	LEFT JOIN #T_AUEC AUEC ON SecMasterWithUnderlying.AUECID = AUEC.AUECID
	--Gaurav: applied handling for Asset class FutureOption=4 and FXOption=10 as well along with EquityOption=2  
	WHERE T_Group.CumQty <> 0
		AND T_Group.AssetID IN (
			2
			,3
			,4
			,10
			)
		AND DateDiff(day, T_Group.AUECLocaldate, @LastDateofMonth) >= 0
		AND T_Group.StateID = 1
		OR (
			DateDiff(day, T_Group.AUECLocaldate, @FirstDateofMonth) <= 0
			AND datediff(day, T_Group.AUECLocaldate, @LastDateofMonth) >= 0
			AND T_Group.StateID = 1
			)
		AND SecMaster.UnderlyingSymbol NOT IN (
			SELECT symbol
			FROM #TempPositionsAndAllocatedTradesALL
			)
	GROUP BY Symbol

	-- getting open underlying symbols  
	INSERT INTO #TempPositionsAndAllocatedTradesALL (
		Symbol
		,Date_Associated
		,AUECID
		,AUECIdentifier
		,BloombergSymbol
		)
	SELECT DISTINCT MAX(SecMaster.UnderlyingSymbol) AS Symbol
		,MAX(T_Group.AUECLocalDate) AS Date_Associated
		,MAX(SecMasterWithUnderlying.AUECID) AS AUECID
		,MAX(AUEC.ExchangeIdentifier) AS AUECIdentifier
		,MAX(SecMasterWithUnderlying.BloombergSymbol) AS BloombergSymbol
	FROM T_Group
	INNER JOIN #pm_taxlots ON #pm_taxlots.groupid = T_Group.groupid
	LEFT JOIN #SecMasterData SecMaster ON #pm_taxlots.Symbol = SecMaster.TickerSymbol
	LEFT JOIN V_SecMasterData_WithUnderlying SecMasterWithUnderlying ON SecMaster.UnderlyingSymbol = SecMasterWithUnderlying.TickerSymbol
	LEFT JOIN #T_AUEC AUEC ON SecMasterWithUnderlying.AUECID = AUEC.AUECID
	--Gaurav: applied handling for Asset class FutureOption=4 and FXOption=10 as well along with EquityOption=2  
	WHERE T_Group.AssetID IN (
			2
			,3
			,4
			,10
			)
		AND SecMaster.UnderlyingSymbol NOT IN (
			SELECT symbol
			FROM #TempPositionsAndAllocatedTradesALL
			)
	GROUP BY #pm_taxlots.Symbol

	CREATE TABLE [dbo].#TempBetaDataFinal (
		Symbol VARCHAR(200)
		,FinalBeta NUMERIC(18, 4)
		,DayBetaID INT
		,Date1 DATETIME
		,AUECID INT
		,AUECIdentifier VARCHAR(200)
		,AssetID INT
		,BloombergSymbol NVARCHAR(200)
		)

	INSERT INTO [dbo].#TempBetaDataFinal (
		Symbol
		,Date1
		,AUECID
		,AUECIdentifier
		,AssetID
		,BloombergSymbol
		)
	SELECT DISTINCT SYMBOL
		,MAX(Date_Associated)
		,MAX(TPAATA.AUECID)
		,MAX(TPAATA.AUECIdentifier)
		,MAX(AssetID)
		,MAX(TPAATA.BloombergSymbol)
	FROM #TempPositionsAndAllocatedTradesALL TPAATA
	GROUP BY Symbol

	DECLARE @Dates VARCHAR(2000)

	SET @Dates = ''

	SELECT @Dates = @Dates + '[' + convert(VARCHAR(12), Items, 101) + '],'
	FROM (
		SELECT TOP 35 AllDates.Items
		FROM dbo.GetDateRange(@FirstDateofMonth, @LastDateofMonth) AS AllDates
		ORDER BY AllDates.Items DESC
		) MarkDate

	SET @Dates = LEFT(@Dates, LEN(@Dates) - 1)

	EXEC (
			'select *                                        
  from (Select TPATAF.Symbol, Date, IsNull(PMDB.Beta,0) AS FBV, TPATAF.AUECID, TPATAF.AUECIdentifier,TPATAF.BloombergSymbol FROM #TempBetaDataFinal TPATAF     
  LEFT OUTER JOIN PM_DailyBeta PMDB                                      
  ON PMDB.Symbol = TPATAF.Symbol                                    
  ) AS DB                        
  PIVOT (MAX(FBV) FOR Date IN (' + @Dates + ')) AS pvt;'
			)
END TRY

BEGIN CATCH
	SET @ErrorMessage = ERROR_MESSAGE();
	SET @ErrorNumber = Error_number();
END CATCH;

