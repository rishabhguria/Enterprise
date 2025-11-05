CREATE PROCEDURE [dbo].[P_WC_RemoveHistoricalData] (@hrs INT)
AS
BEGIN
	IF (@hrs = 0)
	BEGIN
		CREATE TABLE #TempSets (
			[SetID] [int] IDENTITY(1, 1) NOT NULL
			,SetCreatedOn DATETIME
			)

		INSERT INTO #TempSets
		SELECT DISTINCT CreatedOn
		FROM T_PMDataDump
		ORDER BY CreatedOn DESC

		DELETE T_PMDataDump
		WHERE CreatedOn <= (
				SELECT TOP 1 SetCreatedOn
				FROM #TempSets
				WHERE SetID >= 3
				)

		DROP TABLE #TempSets

		CREATE TABLE #TempIndicesSets (
			[SetID] [int] IDENTITY(1, 1) NOT NULL
			,SetCreatedOn DATETIME
			)

		INSERT INTO #TempIndicesSets
		SELECT DISTINCT CreatedOn
		FROM T_PMIndicesDataDump
		ORDER BY CreatedOn DESC

		DELETE T_PMIndicesDataDump
		WHERE CreatedOn <= (
				SELECT TOP 1 SetCreatedOn
				FROM #TempIndicesSets
				WHERE SetID >= 3
				)

		DROP TABLE #TempIndicesSets
	END

	IF (@hrs > 0)
	BEGIN
		DELETE T_PMDataDump
		WHERE datediff(hh, CreatedOn, getdate()) > @hrs

		DELETE T_PMIndicesDataDump
		WHERE datediff(hh, CreatedOn, getdate()) > @hrs
	END
END
