----------------------------------------------------
--Modified By : Bharat Kumar Jangir
--Modification Date : 24 July 2013
--Description : Optimisation - removing union and selecting unique symbols
----------------------------------------------------
CREATE PROCEDURE [dbo].[PMGetAllSymbolsOutStanding] (
	@FromDate DATETIME
	,@ToDate DATETIME
	,@Type INT -- 0 for Same Date,1 for Week , 2 for Month                                          
	,@ErrorMessage VARCHAR(500) OUTPUT
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
	DECLARE @Dates VARCHAR(2000)

	SET @Dates = ''

	SELECT @Dates = @Dates + '[' + CONVERT(VARCHAR(12), Items, 101) + '],'
	FROM (
		SELECT TOP 35 AllDates.Items
		FROM dbo.GetDateRange(@FirstDateofMonth, @LastDateofMonth) AS AllDates
		ORDER BY AllDates.Items DESC
		) MarkDate

	SET @Dates = LEFT(@Dates, LEN(@Dates) - 1)

	CREATE TABLE #TempTaxLotId (Taxlot_PK BIGINT)

	INSERT INTO #TempTaxLotId
	SELECT max(Taxlot_PK)
	FROM PM_Taxlots
	WHERE DateDiff(d, PM_Taxlots.AUECModifiedDate, @LastDateofMonth) >= 0
	GROUP BY taxlotid

	CREATE TABLE #PM_Taxlots (
		Symbol VARCHAR(100)
		,AssetID INT
		,AUECID INT
		)

	INSERT INTO #PM_Taxlots (
		Symbol
		,AssetID
		,AUECID
		)
	SELECT T_GROUP.Symbol
		,T_GROUP.AssetID
		,T_GROUP.AUECID
	FROM PM_Taxlots
	INNER JOIN #TempTaxLotId TaxLot ON PM_Taxlots.TaxLot_PK = TaxLot.Taxlot_PK
	INNER JOIN T_GROUP ON T_GROUP.GroupID = PM_Taxlots.GroupId
	WHERE TaxLotOpenQty <> 0

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
		,AssetID INT
		,BloombergSymbol VARCHAR(100)
		)

	INSERT INTO #SecMasterData
	SELECT TickerSymbol
		,UnderlyingSymbol
		,AssetID
		,BloombergSymbol
	FROM V_SecMasterData

	CREATE TABLE #TempSecMasterUnderLying (
		TickerSymbol VARCHAR(100)
		,BloombergSymbol VARCHAR(100)
		,AUECID INT
		)

	INSERT INTO #TempSecMasterUnderLying
	SELECT TickerSymbol
		,BloombergSymbol
		,AUECID
	FROM V_SecMasterData_WithUnderlying

	-- Get Unallocate Symbol --  
	CREATE TABLE #TempUnallocateSymbol (
		Symbol VARCHAR(200)
		,AUECID INT
		,AssetID INT
		)

	INSERT INTO #TempUnallocateSymbol
	SELECT DISTINCT Symbol
		,AUECID
		,AssetID
	FROM T_Group
	WHERE (
			DateDiff(day, T_Group.AUECLocaldate, @LastDateofMonth) >= 0
			AND T_Group.StateID = 1
			AND T_Group.CumQty <> 0
			)

	CREATE TABLE [dbo].#TempUniqueSymbol (
		Symbol VARCHAR(200)
		,AUECID INT
		,BloombergSymbol NVARCHAR(200)
		)

	INSERT INTO #TempUniqueSymbol (
		Symbol
		,AUECID
		,BloombergSymbol
		)
	SELECT DISTINCT GP.Symbol AS Symbol
		,MAX(GP.AUECID) AS AUEDID
		,MAX(SM.BloombergSymbol) AS BloombergSymbol
	FROM #TempUnallocateSymbol GP
	LEFT JOIN #T_AUEC AUEC ON GP.AUECID = AUEC.AUECID
	LEFT JOIN #SecMasterData SM ON GP.Symbol = SM.TickerSymbol
	GROUP BY Symbol

	-- collect open trade symbols                                            
	INSERT INTO #TempUniqueSymbol (
		Symbol
		,AUECID
		,BloombergSymbol
		)
	SELECT DISTINCT PT.Symbol
		,MAX(PT.AUECID) AS AUEDID
		,MAX(SM.BloombergSymbol) AS BloombergSymbol
	FROM #PM_Taxlots PT
	LEFT JOIN #SecMasterData SM ON PT.Symbol = SM.TickerSymbol
	LEFT JOIN #T_AUEC AUEC ON PT.AUECID = AUEC.AUECID
	GROUP BY PT.Symbol

	-- In case of options, we also need to fetch the underlying symbol mark as these would be utilized in no of places                                     
	--getting underlying symbols                                       
	INSERT INTO #TempUniqueSymbol (
		Symbol
		,AUECID
		,BloombergSymbol
		)
	SELECT DISTINCT MAX(SecMaster.UnderlyingSymbol) AS Symbol
		,MAX(SecMasterWithUnderlying.AUECID) AS AUEDID
		,max(SecMasterWithUnderlying.BloombergSymbol) AS BloombergSymbol
	FROM #TempUnallocateSymbol GP
	LEFT JOIN #SecMasterData SecMaster ON GP.Symbol = SecMaster.TickerSymbol
	LEFT JOIN #TempSecMasterUnderLying SecMasterWithUnderlying ON SecMaster.UnderlyingSymbol = SecMasterWithUnderlying.TickerSymbol
	LEFT JOIN #T_AUEC AUEC ON SecMasterWithUnderlying.AUECID = AUEC.AUECID
	WHERE GP.AssetID IN (
			2
			,4
			,10
			)
		AND SecMaster.UnderlyingSymbol NOT IN (
			SELECT symbol
			FROM #TempUniqueSymbol
			)
	GROUP BY Symbol

	-- getting open underlying symbols  
	INSERT INTO #TempUniqueSymbol (
		Symbol
		,AUECID
		,BloombergSymbol
		)
	SELECT DISTINCT MAX(SecMaster.UnderlyingSymbol) AS Symbol
		,MAX(SecMasterWithUnderlying.AUECID) AS AUEDID
		,MAX(SecMasterWithUnderlying.BloombergSymbol) AS BloombergSymbol
	FROM #pm_taxlots
	LEFT JOIN #SecMasterData SecMaster ON #pm_taxlots.Symbol = SecMaster.TickerSymbol
	LEFT JOIN #TempSecMasterUnderLying SecMasterWithUnderlying ON SecMaster.UnderlyingSymbol = SecMasterWithUnderlying.TickerSymbol
	LEFT JOIN #T_AUEC AUEC ON SecMasterWithUnderlying.AUECID = AUEC.AUECID
	WHERE #pm_taxlots.AssetID IN (
			2
			,4
			,10
			)
		AND SecMaster.UnderlyingSymbol NOT IN (
			SELECT symbol
			FROM #TempUniqueSymbol
			)
	GROUP BY #pm_taxlots.Symbol

	CREATE TABLE [dbo].#TempUniqueSymbolFinal (
		Symbol VARCHAR(200)
		,AUECID INT
		,BloombergSymbol NVARCHAR(200)
		)

	INSERT INTO [dbo].#TempUniqueSymbolFinal (
		Symbol
		,AUECID
		,BloombergSymbol
		)
	SELECT DISTINCT SYMBOL
		,MAX(TPAATA.AUECID)
		,MAX(TPAATA.BloombergSymbol)
	FROM #TempUniqueSymbol TPAATA
	GROUP BY Symbol

	EXEC (
			'select *                                  
from (Select DISTINCT UniqueSymbol.Symbol,Date,OutStandings,UniqueSymbol.AUECID,sec.BloombergSymbol  
from #TempUniqueSymbolFinal UniqueSymbol 
left outer join  PM_DailyOutStandings on PM_DailyOutStandings.Symbol=UniqueSymbol.Symbol  
Left Outer Join #SecMasterData as sec on sec.TickerSymbol = UniqueSymbol.Symbol )                  
AS DDE PIVOT (MAX(OutStandings) FOR Date IN (' + @Dates + ')) AS pvt ; '
			)

	DROP TABLE #TempTaxLotId

	DROP TABLE #PM_Taxlots

	DROP TABLE #SecMasterData

	DROP TABLE #T_AUEC

	DROP TABLE #TempSecMasterUnderLying

	DROP TABLE #TempUnallocateSymbol

	DROP TABLE #TempUniqueSymbol

	DROP TABLE #TempUniqueSymbolFinal
END TRY

BEGIN CATCH
	SET @ErrorMessage = ERROR_MESSAGE();
	SET @ErrorNumber = ERROR_NUMBER();
END CATCH;
