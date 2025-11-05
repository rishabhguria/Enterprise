
CREATE PROCEDURE P_GetTodaysSplitFactor (@AllAUECDatesString VARCHAR(MAX))
AS
CREATE TABLE #LatestauecdatesTable (
	AUECID INT
	,LatestAUECDate DATETIME
	)

INSERT INTO #LatestauecdatesTable
SELECT *
FROM dbo.GetAllAUECDatesFromString(@AllAUECDatesString)

SELECT Symbol
	,SplitFactor
	,IsApplied
	,EffectiveDate
INTO #Temp_CorpData
FROM V_CorpActionData
WHERE CorpActionTypeId = 6 -- For Split      

SELECT TickerSymbol
	,AUECID
INTO #Temp_Sec
FROM V_SecMasterData
WHERE TickerSymbol IN (
		SELECT Symbol
		FROM #Temp_CorpData
		)

SELECT CA.Symbol
	,CA.SplitFactor
FROM #Temp_CorpData CA
INNER JOIN #Temp_Sec Sec
	ON CA.Symbol = Sec.TickerSymbol
INNER JOIN #LatestauecdatesTable CurrentAUECDates
	ON Sec.AUECID = CurrentAUECDates.AUECID
WHERE CA.IsApplied = 1
	AND (datediff(d, CA.EffectiveDate, CurrentAUECDates.LatestAUECDate) = 0)

DROP TABLE #LatestauecdatesTable

DROP TABLE #Temp_Sec

DROP TABLE #Temp_CorpData
