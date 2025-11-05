CREATE PROCEDURE [dbo].[P_GetCorruptedClosingData] (@StartTime DATETIME)
AS
-- First Closing Corruption Scenario,
SELECT DISTINCT (TaxLotClosingId_Fk)
INTO #TempClosingId
FROM PM_Taxlots  with (nolock)
INNER JOIN PM_TaxlotClosing PTC ON TaxLotClosingId_Fk = PTC.TaxLotClosingID
WHERE TaxLotClosingId_Fk IS NOT NULL
	AND PM_Taxlots.TimeOfSaveUTC BETWEEN @StartTime
		AND GETUTCDATE()

SELECT TaxLotClosingId_Fk
	,count(TaxLotID) AS CountTaxlotID
INTO #tempCorruptedData
FROM PM_Taxlots  with (nolock)
WHERE TaxLotClosingId_Fk IN (
		SELECT *
		FROM #TempClosingId
		)
GROUP BY TaxLotClosingId_Fk

SELECT TimeOfSaveUTC
	,Symbol
	,FundID
INTO #Scenario1
FROM PM_Taxlots  with (nolock)
WHERE TaxLotClosingId_Fk IN (
		SELECT TaxLotClosingId_Fk
		FROM #tempCorruptedData
		WHERE CountTaxlotID <> 2
		)
ORDER BY TimeOfSaveUTC
	,Symbol

-- Second Closing Corruption Scenario,
SELECT PositionalTaxlotId
	,ClosingTaxLotId
INTO #PositionanTaxLotId
FROM PM_TaxLotClosing
GROUP BY PositionalTaxlotId
	,ClosingTaxLotId
HAVING Count(*) > 1

SELECT DISTINCT PM_TaxLots.TimeOfSaveUTC AS ClosingDate
	,Symbol
	,FundID
INTO #Scenario2
FROM PM_TaxLots with (nolock)
INNER JOIN PM_TaxLotClosing PTC ON TaxLotClosingID_FK = PTC.TaxlotclosingId
WHERE TaxLotClosingID_FK IN (
		SELECT TaxLotClosingID
		FROM PM_TaxLotClosing CL
		INNER JOIN #PositionanTaxLotId N ON CL.PositionalTaxlotId = N.PositionalTaxlotId
			AND CL.ClosingTaxLotId = N.ClosingTaxLotId
		)
	AND PM_Taxlots.TimeOfSaveUTC BETWEEN @StartTime
		AND GETUTCDATE()
ORDER BY PM_TaxLots.TimeOfSaveUTC
	,Symbol

SELECT *
FROM #Scenario1

UNION ALL

SELECT *
FROM #Scenario2

DROP TABLE #TempClosingId
	,#tempCorruptedData
	,#PositionanTaxLotId
	,#Scenario1
	,#Scenario2
