/*******************************************************************************                                                                      
Author : Sumeet Kumar                                            
Creation Date : 6 May,2016                                              
Description : To calculate fundwise and combined ITD,WEEK, QTD, MTD, YTD Performance                                                                        
                                            
Execution Statement:                                          
exec  P_GetPerformanceCalculation      
@fundId=N'1354,1355,1356,1357,1358,1359,1360,1361,1362,1363,1364,1365,1366,1369,1370,1371,1373,1374,1375,1376,1378,1379,1380',        
@date='2016-04-20 00:00:00:000'                          
**********************************************************************************/
CREATE PROCEDURE [dbo].[P_NT_GetPerformanceCalculation] (
	@fundId NVARCHAR(MAX)
	,@date DATETIME
	,@level BIT
	)
AS
BEGIN
	DECLARE @defualtAUECId INT

	SELECT TOP 1 @defualtAUECId = DefaultAUECID
	FROM T_Company

	CREATE TABLE #PerformanceTable (
		FundId INT
		,InceptionDate DATETIME
		,YearDate DATETIME
		,QuarterDate DATETIME
		,MonthDate DATETIME
		,WeekDate DATETIME
		,DailyDate DATETIME
		,ITDPerformance FLOAT
		,YTDPerformance FLOAT
		,QTDPerformance FLOAT
		,MTDPerformance FLOAT
		,WeekPerformance FLOAT
		,DailyPerformance FLOAT
		)

	------------------------------------------------------------------------            
	------------ Splitting funds in table and updating values --------------            
	INSERT INTO #PerformanceTable (
		FundId
		,InceptionDate
		,YearDate
		,QuarterDate
		,MonthDate
		,WeekDate
		,DailyDate
		)
	SELECT IdTable.Items AS FundId
		,Pref.CashMgmtStartDate AS InceptionDate
		,DATEADD(YEAR, DATEDIFF(YEAR, 0, @date), 0) AS YearDate
		,DATEADD(qq, DATEDIFF(qq, 0, @date), 0) AS QuarterDate
		,DATEADD(month, DATEDIFF(month, 0, @date), 0) AS MonthDate
		,DATEADD(WEEK, DATEDIFF(WEEK, 0, @date), 0) AS WeekDate
		,@date AS DailyDate
	FROM T_CashPreferences AS Pref
	JOIN split(@fundId, ',') AS IdTable ON Pref.FundID = IdTable.Items

	------------------------------------------------------------------------            
	------------------------------------------------------------------------            
	---------- Inserting defualt value for combination of funds ------------            
	INSERT INTO #PerformanceTable (
		FundId
		,InceptionDate
		,YearDate
		,QuarterDate
		,MonthDate
		,WeekDate
		,DailyDate
		)
	SELECT - 1 AS FundId
		,MIN(InceptionDate) AS InceptionDate
		,MIN(YearDate) AS YearDate
		,MIN(QuarterDate) AS QuarterDate
		,MIN(MonthDate) AS MonthDate
		,MIN(WeekDate) AS WeekDate
		,MIN(DailyDate) AS DailyDate
	FROM #PerformanceTable

	IF @level = 'true'
	BEGIN
		INSERT INTO #PerformanceTable (
			FundId
			,InceptionDate
			,YearDate
			,QuarterDate
			,MonthDate
			,WeekDate
			,DailyDate
			)
		SELECT CMF.CompanyMasterFundID AS FundId
			,'2015-01-01' AS InceptionDate
			,DATEADD(YEAR, DATEDIFF(YEAR, 0, @date), 0) AS YearDate
			,DATEADD(qq, DATEDIFF(qq, 0, @date), 0) AS QuarterDate
			,DATEADD(month, DATEDIFF(month, 0, @date), 0) AS MonthDate
			,DATEADD(WEEK, DATEDIFF(WEEK, 0, @date), 0) AS WeekDate
			,@date AS DailyDate
		FROM T_CompanymasterFunds AS CMF
	END
	ELSE
		------------------------------------------------------------------------            
		------------------------------------------------------------------------            
		------------ Updating dates according to business days -----------------            
		UPDATE #PerformanceTable
		SET InceptionDate = CASE 
				WHEN dbo.IsBusinessDay(InceptionDate, @defualtAUECId) = 1
					THEN InceptionDate
				ELSE dbo.AdjustBusinessDays(InceptionDate, 1, @defualtAUECId)
				END
			,YearDate = CASE 
				WHEN dbo.IsBusinessDay(YearDate, @defualtAUECId) = 1
					THEN YearDate
				ELSE dbo.AdjustBusinessDays(YearDate, 1, @defualtAUECId)
				END
			,QuarterDate = CASE 
				WHEN dbo.IsBusinessDay(QuarterDate, @defualtAUECId) = 1
					THEN QuarterDate
				ELSE dbo.AdjustBusinessDays(QuarterDate, 1, @defualtAUECId)
				END
			,MonthDate = CASE 
				WHEN dbo.IsBusinessDay(MonthDate, @defualtAUECId) = 1
					THEN MonthDate
				ELSE dbo.AdjustBusinessDays(MonthDate, 1, @defualtAUECId)
				END
			,WeekDate = CASE 
				WHEN dbo.IsBusinessDay(WeekDate, @defualtAUECId) = 1
					THEN WeekDate
				ELSE dbo.AdjustBusinessDays(WeekDate, 1, @defualtAUECId)
				END
			,DailyDate = CASE 
				WHEN dbo.IsBusinessDay(DailyDate, @defualtAUECId) = 1
					THEN DailyDate
				ELSE dbo.AdjustBusinessDays(DailyDate, 1, @defualtAUECId)
				END

	------------------------------------------------------------------------            
	------------------------------------------------------------------------            
	------------ Updating dates according to inception date ----------------            
	UPDATE #PerformanceTable
	SET YearDate = CASE 
			WHEN YearDate <= InceptionDate
				THEN InceptionDate
			ELSE YearDate
			END
		,QuarterDate = CASE 
			WHEN QuarterDate <= InceptionDate
				THEN InceptionDate
			ELSE QuarterDate
			END
		,MonthDate = CASE 
			WHEN MonthDate <= InceptionDate
				THEN InceptionDate
			ELSE MonthDate
			END
		,WeekDate = CASE 
			WHEN WeekDate <= InceptionDate
				THEN InceptionDate
			ELSE WeekDate
			END
		,DailyDate = CASE 
			WHEN DailyDate <= InceptionDate
				THEN InceptionDate
			ELSE DailyDate
			END

	------------------------------------------------------------------------            
	------------------------------------------------------------------------            
	---------- Getting fund wise redemption, opening equity, PNL --------            
	SELECT FundID
		,DATE
		,AddRed
		,OpeningValue
		,PNL
	INTO #tempFinal
	FROM T_SavedPerformanceNumbers
	INNER JOIN split(@fundId, ',') AS IdTable ON T_SavedPerformanceNumbers.FundID = IdTable.Items

	----------------------------------------------------------------------------------------------------            
	----- Add rows for fund wise aggregated data            
	----------------------------------------------------------------------------------------------------            
	IF @level = 'true'
	BEGIN
		INSERT INTO #tempFinal
		SELECT CMF.CompanyMasterFundID AS FundID
			,DATE
			,SUM(COALESCE(AddRed, 0)) AS AddRed
			,SUM(COALESCE(OpeningValue, 0)) AS OpeningValue
			,SUM(COALESCE(PNL, 0)) AS PNL
		FROM #tempFinal TF
		LEFT JOIN T_CompanyMasterFundSubAccountAssociation CMFSA ON CMFSA.CompanyFundID = TF.FundID
		LEFT JOIN T_CompanymasterFunds CMF ON CMF.CompanyMasterFundID = CMFSA.CompanyMasterFundID
		GROUP BY CMF.CompanyMasterFundID
			,DATE
	END

	INSERT INTO #tempFinal
	SELECT - 1 AS FUNDID
		,DATE
		,SUM(COALESCE(AddRed, 0)) AS AddRed
		,SUM(COALESCE(OpeningValue, 0)) AS OpeningValue
		,SUM(COALESCE(PNL, 0)) AS PNL
	FROM #tempFinal
	GROUP BY DATE

	-----------------------------------------------------------------------                                          
	---------------------- Performance calculation  -----------------------                          
	-----------------------------------------------------------------------                                       
	-------------------- DailyPerformance calculation ---------------------            
	ALTER TABLE #tempFinal ADD RORDaily FLOAT

	UPDATE #tempFinal
	SET RORDaily = (
			CASE 
				WHEN (
						IsNull(PNL, 0) = 0
						OR (IsNull(AddRed, 0) + IsNull(OpeningValue, 0)) = 0
						)
					THEN 0
				ELSE (IsNull(PNL, 0) / (IsNull(AddRed, 0) + IsNull(OpeningValue, 0))) * 100
				END
			)

	-----------------------------------------------------------------------            
	------------- Update DailyPerformance in PerformanceTable -------------            
	UPDATE PT
	SET DailyPerformance = tmp.RORDaily
	FROM #PerformanceTable PT
	INNER JOIN #tempFinal tmp ON PT.FundID = tmp.FundID
	WHERE PT.DailyDate = tmp.DATE

	/*--------------------------------------------------------------------------------------------------                                            
Insert in monthly running table for specific range with performance type                
#RunningTotal (FundID,DateValue,Performance)                        
---------------------------------------------------------------------------------------------------*/
	CREATE TABLE #RunningTotal (
		FundId INT
		,DATE DATETIME
		,Performance FLOAT
		)

	------------------------------------------------------------------------            
	------------------------------------------------------------------------            
	---------------------- WeekPerformance ------------------------------            
	INSERT INTO #RunningTotal
	SELECT TF.FundID
		,TF.DATE
		,0 AS Performance
	FROM #tempFinal TF
	INNER JOIN #PerformanceTable PT ON PT.FundId = TF.FundID
		AND TF.DATE >= PT.WeekDate
		AND TF.DATE <= PT.DailyDate
	GROUP BY TF.FundID
		,TF.DATE

	------------------------------------------------------------------------            
	----------------- Calculate Week performance  -----------------------            
	------------------------------------------------------------------------            
	UPDATE #RunningTotal
	SET Performance = CASE 
			WHEN CountNegative % 2 <> 0
				THEN ((- 1 * CountValue) - 1) * 100
			ELSE (CountValue - 1) * 100
			END
	FROM #RunningTotal
	INNER JOIN (
		SELECT TF1.FundID
			,TF1.DATE
			,EXP(SUM(Log(CASE 
							WHEN (1 + TF2.RORDaily / 100) < 0
								THEN (1 + TF2.RORDaily / 100) * - 1
							ELSE (1 + TF2.RORDaily / 100)
							END))) AS CountValue
			,count(CASE 
					WHEN (1 + TF2.RORDaily / 100) < 0
						THEN 1
					END) AS CountNegative
		FROM #tempFinal TF1
		INNER JOIN #PerformanceTable PT ON PT.FundId = TF1.FundID
			AND TF1.DATE >= PT.WeekDate
			AND TF1.DATE <= PT.DailyDate
		INNER JOIN #tempFinal TF2 ON (
				TF2.DATE <= TF1.DATE
				AND TF2.DATE >= PT.WeekDate
				AND TF1.FundID = TF2.FundID
				)
		GROUP BY TF1.FundID
			,TF1.DATE
		) Contribution ON Contribution.FundId = #RunningTotal.FundId
		AND Contribution.DATE = #RunningTotal.DATE

	-----------------------------------------------------------------------            
	------------- Update WeekPerformance in PerformanceTable ---------------            
	UPDATE PT
	SET WeekPerformance = RM.Performance
	FROM #PerformanceTable PT
	INNER JOIN #RunningTotal RM ON PT.FundID = RM.FundID
	WHERE PT.DailyDate = RM.DATE

	------------------------------------------------------------------------            
	------------------------------------------------------------------------            
	---------------------- MonthlyPerformance ------------------------------            
	INSERT INTO #RunningTotal
	SELECT TF.FundID
		,TF.DATE
		,0 AS Performance
	FROM #tempFinal TF
	INNER JOIN #PerformanceTable PT ON PT.FundId = TF.FundID
		AND TF.DATE >= PT.MonthDate
		AND TF.DATE <= PT.DailyDate
	GROUP BY TF.FundID
		,TF.DATE

	------------------------------------------------------------------------            
	----------------- Calculate monthly performance  -----------------------            
	------------------------------------------------------------------------            
	UPDATE #RunningTotal
	SET Performance = CASE 
			WHEN CountNegative % 2 <> 0
				THEN ((- 1 * CountValue) - 1) * 100
			ELSE (CountValue - 1) * 100
			END
	FROM #RunningTotal
	INNER JOIN (
		SELECT TF1.FundID
			,TF1.DATE
			,EXP(SUM(Log(CASE 
							WHEN (1 + TF2.RORDaily / 100) < 0
								THEN (1 + TF2.RORDaily / 100) * - 1
							ELSE (1 + TF2.RORDaily / 100)
							END))) AS CountValue
			,count(CASE 
					WHEN (1 + TF2.RORDaily / 100) < 0
						THEN 1
					END) AS CountNegative
		FROM #tempFinal TF1
		INNER JOIN #PerformanceTable PT ON PT.FundId = TF1.FundID
			AND TF1.DATE >= PT.MonthDate
			AND TF1.DATE <= PT.DailyDate
		INNER JOIN #tempFinal TF2 ON (
				TF2.DATE <= TF1.DATE
				AND TF2.DATE >= PT.MonthDate
				AND TF1.FundID = TF2.FundID
				)
		GROUP BY TF1.FundID
			,TF1.DATE
		) Contribution ON Contribution.FundId = #RunningTotal.FundId
		AND Contribution.DATE = #RunningTotal.DATE

	-----------------------------------------------------------------------            
	------------- Update MTDPerformance in PerformanceTable ---------------            
	UPDATE PT
	SET MTDPerformance = RM.Performance
	FROM #PerformanceTable PT
	INNER JOIN #RunningTotal RM ON PT.FundID = RM.FundID
	WHERE PT.DailyDate = RM.DATE

	------------------------------------------------------------------------            
	------------------------------------------------------------------------            
	---------------------- QTDPerformance ----------------------------------            
	DELETE
	FROM #RunningTotal

	INSERT INTO #RunningTotal
	SELECT TF.FundID
		,TF.DATE
		,0 AS Performance
	FROM #tempFinal TF
	INNER JOIN #PerformanceTable PT ON PT.FundId = TF.FundID
		AND TF.DATE >= PT.QuarterDate
		AND TF.DATE <= PT.DailyDate
	GROUP BY TF.FundID
		,TF.DATE

	UPDATE #RunningTotal
	SET Performance = CASE 
			WHEN CountNegative % 2 <> 0
				THEN ((- 1 * CountValue) - 1) * 100
			ELSE (CountValue - 1) * 100
			END
	FROM #RunningTotal
	INNER JOIN (
		SELECT TF1.FundID
			,TF1.DATE
			,EXP(SUM(Log(CASE 
							WHEN (1 + (TF2.RORDaily / 100)) < 0
								THEN (1 + (TF2.RORDaily / 100)) * - 1
							ELSE (1 + (TF2.RORDaily / 100))
							END))) AS CountValue
			,count(CASE 
					WHEN 1 + (TF2.RORDaily / 100) < 0
						THEN 1
					END) AS CountNegative
		FROM #tempFinal TF1
		INNER JOIN #PerformanceTable PT ON PT.FundId = TF1.FundID
			AND TF1.DATE >= PT.QuarterDate
			AND TF1.DATE <= PT.DailyDate
		INNER JOIN #tempFinal TF2 ON (
				TF2.DATE <= TF1.DATE
				AND TF2.DATE >= PT.QuarterDate
				AND TF1.FundID = TF2.FundID
				)
		GROUP BY TF1.FundID
			,TF1.DATE
		) Contribution ON Contribution.FundId = #RunningTotal.FundId
		AND Contribution.DATE = #RunningTotal.DATE

	-----------------------------------------------------------------------            
	------------- Update QTDPerformance in PerformanceTable ---------------            
	UPDATE PT
	SET QTDPerformance = RM.Performance
	FROM #PerformanceTable PT
	INNER JOIN #RunningTotal RM ON PT.FundID = RM.FundID
	WHERE PT.DailyDate = RM.DATE

	------------------------------------------------------------------------            
	------------------------------------------------------------------------            
	---------------------- YTDPerformance ----------------------------------            
	DELETE
	FROM #RunningTotal

	INSERT INTO #RunningTotal
	SELECT TF.FundID
		,TF.DATE
		,0 AS Performance
	FROM #tempFinal TF
	INNER JOIN #PerformanceTable PT ON PT.FundId = TF.FundID
		AND TF.DATE >= PT.YearDate
		AND TF.DATE <= PT.DailyDate
	GROUP BY TF.FundID
		,TF.DATE

	UPDATE #RunningTotal
	SET Performance = CASE 
			WHEN CountNegative % 2 <> 0
				THEN ((- 1 * CountValue) - 1) * 100
			ELSE (CountValue - 1) * 100
			END
	FROM #RunningTotal
	INNER JOIN (
		SELECT TF1.FundID
			,TF1.DATE
			,EXP(SUM(Log(CASE 
							WHEN (1 + (TF2.RORDaily / 100)) < 0
								THEN (1 + (TF2.RORDaily / 100)) * - 1
							ELSE (1 + (TF2.RORDaily / 100))
							END))) AS CountValue
			,count(CASE 
					WHEN 1 + (TF2.RORDaily / 100) < 0
						THEN 1
					END) AS CountNegative
		FROM #tempFinal TF1
		INNER JOIN #PerformanceTable PT ON PT.FundId = TF1.FundID
			AND TF1.DATE >= PT.YearDate
			AND TF1.DATE <= PT.DailyDate
		INNER JOIN #tempFinal TF2 ON (
				TF2.DATE <= TF1.DATE
				AND TF2.DATE >= PT.YearDate
				AND TF1.FundID = TF2.FundID
				)
		GROUP BY TF1.FundID
			,TF1.DATE
		) Contribution ON Contribution.FundId = #RunningTotal.FundId
		AND Contribution.DATE = #RunningTotal.DATE

	-----------------------------------------------------------------------            
	------------- Update YTDPerformance in PerformanceTable ---------------            
	UPDATE PT
	SET YTDPerformance = RM.Performance
	FROM #PerformanceTable PT
	INNER JOIN #RunningTotal RM ON PT.FundID = RM.FundID
	WHERE PT.DailyDate = RM.DATE

	------------------------------------------------------------------------            
	------------------------------------------------------------------------            
	---------------------- ITDPerformance ----------------------------------            
	DELETE
	FROM #RunningTotal

	INSERT INTO #RunningTotal
	SELECT TF.FundID
		,TF.DATE
		,0 AS Performance
	FROM #tempFinal TF
	INNER JOIN #PerformanceTable PT ON PT.FundId = TF.FundID
		AND TF.DATE >= PT.InceptionDate
		AND TF.DATE <= PT.DailyDate
	GROUP BY TF.FundID
		,TF.DATE

	UPDATE #RunningTotal
	SET Performance = CASE 
			WHEN CountNegative % 2 <> 0
				THEN ((- 1 * CountValue) - 1) * 100
			ELSE (CountValue - 1) * 100
			END
	FROM #RunningTotal
	INNER JOIN (
		SELECT TF1.FundID
			,TF1.DATE
			,EXP(SUM(Log(CASE 
							WHEN (1 + (TF2.RORDaily / 100)) < 0
								THEN (1 + (TF2.RORDaily / 100)) * - 1
							ELSE (1 + (TF2.RORDaily / 100))
							END))) AS CountValue
			,count(CASE 
					WHEN 1 + (TF2.RORDaily / 100) < 0
						THEN 1
					END) AS CountNegative
		FROM #tempFinal TF1
		INNER JOIN #PerformanceTable PT ON PT.FundId = TF1.FundID
			AND TF1.DATE >= PT.InceptionDate
			AND TF1.DATE <= PT.DailyDate
		INNER JOIN #tempFinal TF2 ON (
				TF2.DATE <= TF1.DATE
				AND TF2.DATE >= PT.InceptionDate
				AND TF1.FundID = TF2.FundID
				)
		GROUP BY TF1.FundID
			,TF1.DATE
		) Contribution ON Contribution.FundId = #RunningTotal.FundId
		AND Contribution.DATE = #RunningTotal.DATE

	-----------------------------------------------------------------------            
	------------- Update ITDPerformance in PerformanceTable ---------------            
	UPDATE PT
	SET ITDPerformance = RM.Performance
	FROM #PerformanceTable PT
	INNER JOIN #RunningTotal RM ON PT.FundID = RM.FundID
	WHERE PT.DailyDate = RM.DATE

	SELECT *
	FROM #PerformanceTable

	DROP TABLE #PerformanceTable
		,#tempFinal
		,#RunningTotal
END
