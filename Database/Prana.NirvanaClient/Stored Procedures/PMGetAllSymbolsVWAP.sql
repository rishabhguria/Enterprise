CREATE PROCEDURE [dbo].[PMGetAllSymbolsVWAP]    (
	@Date DATETIME
	,@Type INT -- 0 for Same Date,1 for Week , 2 for Month
	,@ErrorMessage VARCHAR(500) OUTPUT
	,@ErrorNumber INT OUTPUT
	,@GetSameDayClosedData BIT
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
		,GroupID VARCHAR(100)
		)

	
		--Getting All Open Taxlots
		INSERT INTO #PM_Taxlots
		SELECT Symbol
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
		,SM.BloombergSymbol AS BloombergSymbol
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
		,BloombergSymbol
		)
	SELECT PMD.Symbol AS Symbol
		,PMD.DATE AS Date_Associated
		,0 AS AUECID
		,'Indices-Indices' AS AUECIdentifier
		,0 AS AssetID
		,SM.BloombergSymbol AS BloombergSymbol
	FROM PM_DailyVWAP PMD
	INNER JOIN #SecMasterData SM ON SM.TickerSymbol = PMD.Symbol
	WHERE DATEDIFF(d, PMD.DATE, @LastDateofMonth) >= 0
		AND SM.AssetID = 7

	INSERT INTO #TempPositionsAndAllocatedTradesALL (
		Symbol
		,Date_Associated
		,AUECID
		,AUECIdentifier
		,AssetID
		,BloombergSymbol
		)
	SELECT PT.Symbol
		,G.AUECLocalDate AS Date_Associated
		,SM.AUECID AS AUEDID
		,AUEC.ExchangeIdentifier AS AUECIdentifier
		,SM.AssetID AS AssetID
		,SM.BloombergSymbol AS BloombergSymbol
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
		)
	SELECT SecMaster.UnderlyingSymbol AS Symbol
		,G.AUECLocalDate AS Date_Associated
		,SecMasterWithUnderlying.AUECID AS AUEDID
		,AUEC.ExchangeIdentifier AS AUECIdentifier
		,SecMasterWithUnderlying.BloombergSymbol AS BloombergSymbol
		,SecMasterWithUnderlying.AssetID AS AssetID
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
		)
	SELECT SecMaster.UnderlyingSymbol AS Symbol
		,GP.AUECLocalDate AS Date_Associated
		,SecMasterWithUnderlying.AUECID AS AUEDID
		,AUEC.ExchangeIdentifier AS AUECIdentifier
		,SecMasterWithUnderlying.BloombergSymbol AS BloombergSymbol
		,SecMasterWithUnderlying.AssetID AS AssetID
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
		,FinalVWAP NUMERIC(18, 4)
		,DayVWAPID INT
		,Date1 DATETIME
		,AUECID INT
		,AUECIdentifier VARCHAR(200)
		,AssetID INT
		,BloombergSymbol NVARCHAR(200)
		)

	INSERT INTO [dbo].#TempPositionsAndAllocatedTradesAllFinal (
		Symbol
		,Date1
		,AUECID
		,AUECIdentifier
		,AssetID
		,BloombergSymbol
		)
	SELECT SYMBOL
		,MAX(Date_Associated)
		,MAX(TPAATA.AUECID)
		,MAX(TPAATA.AUECIdentifier)
		,MAX(AssetID)
		,MAX(TPAATA.BloombergSymbol)
	FROM #TempPositionsAndAllocatedTradesALL TPAATA
	GROUP BY Symbol

---From Here
DECLARE @Dates varchar(2000)                                      
 SET @Dates = ''                                      
 SELECT @Dates = @Dates + '[' + convert(varchar(12),Items,101) + '],'                                      
     FROM (select Top 35 AllDates.Items                                         
  from dbo.GetDateRange(@FirstDateofMonth, @LastDateofMonth) as AllDates Order By AllDates.Items desc) MarkDate                                      
   SET @Dates = LEFT(@Dates, LEN(@Dates) - 1)    

	 exec ('select *                                        
  from (Select TPATAF.Symbol, Date, IsNull(PMDB.VWAP,0) AS FBV, TPATAF.AUECID, TPATAF.AUECIdentifier,TPATAF.BloombergSymbol FROM #TempPositionsAndAllocatedTradesAllFinal TPATAF     
  LEFT OUTER JOIN PM_DailyVWAP PMDB                                      
  ON PMDB.Symbol = TPATAF.Symbol                                    
  ) AS DB                        
  PIVOT (MAX(FBV) FOR Date IN (' + @Dates + ')) AS pvt;')         
	
--To here
	
	DROP TABLE #PM_Taxlots
		,#T_AUEC
		,#SecMasterData

	DROP TABLE #TempPositionsAndAllocatedTradesALL
		,#TempPositionsAndAllocatedTradesAllFinal
		,#TempUnallocateTGroup
		,#SecMasterDataWithUnderlying
END TRY

BEGIN CATCH
	SET @ErrorMessage = ERROR_MESSAGE();
	SET @ErrorNumber = Error_number();
END CATCH;
