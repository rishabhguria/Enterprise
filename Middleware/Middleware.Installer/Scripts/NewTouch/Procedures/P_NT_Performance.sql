/*************************************************                                                                
Author : Sumeet Kumar                                       
Creation Date : 3 May , 2016                                         
                                        
                               
Execution Statement:                                    
exec  P_NT_Performance @fundId=N'2',@accountId='1213',@EndDate='2016-5-2 00:00:00:000' 
*************************************************/
CREATE PROCEDURE [dbo].[P_NT_Performance] (
	@fundId VARCHAR(max) = NULL
	,@accountId VARCHAR(max) = NULL
	,@EndDate DATETIME
	)
AS
---------------------------------------------------------------------------------------------------*/                                         
-- Add account based on FundID or accountID    
SET NOCOUNT OFF;
SET FMTONLY OFF;

CREATE TABLE #accounts (AcctId INT)

DECLARE @aggregateEntity VARCHAR(100)
DECLARE @masterfundlevel BIT

IF @accountId IS NULL
BEGIN
	IF @FundId IS NULL
	BEGIN
		INSERT INTO #accounts
		SELECT CompanyFundID
		FROM T_CompanyFunds
		WHERE StartDate IS NOT NULL

		SET @aggregateEntity = 'All Funds'
		SET @masterfundlevel = 'true'
	END
	ELSE
	BEGIN
		INSERT INTO #accounts
		SELECT CompanyFundID
		FROM T_CompanyMasterFundSubAccountAssociation
		WHERE CompanyMasterFundID = @FundId

		SET @aggregateEntity = 'All Accounts'
		SET @masterfundlevel = 'false'
	END
END
ELSE
BEGIN
	INSERT INTO #accounts
	SELECT @accountId

	SET @masterfundlevel = 'false'
END

/*--------------------------------------------------------------------------------------------------                                        
CREATE TABLE to get FUND Wise DAILY, MTD, QTD, YTD, CashManagementStartDate(ITD) and END dates                                       
---------------------------------------------------------------------------------------------------*/
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

/*--------------------------------------------------------------------------------------------------                                
comma seprated FundID's                   
---------------------------------------------------------------------------------------------------*/
DECLARE @fund_ID VARCHAR(max)

SET @fund_ID = Stuff((
			SELECT ',' + Convert(VARCHAR(100), AcctId)
			FROM #accounts
			FOR XML Path('')
			), 1, 1, '')

INSERT INTO #PerformanceTable
EXEC P_NT_GetPerformanceCalculation @fundID = @fund_ID
	,@date = @EndDate
	,@level = @masterfundlevel

/***************                                      
Final Output of the Procedure                         
******************/
DECLARE @count INT

SET @count = 0

SELECT @Count = Count(*)
FROM #accounts

PRINT @Count

IF @Count <= 1
BEGIN
	DELETE
	FROM #PerformanceTable
	WHERE FundID = - 1
END

IF @fundId IS NULL
	AND @accountId IS NULL
BEGIN
	(
			SELECT CMF.CompanyMasterFundID AS FundID
				,isnull(CMF.MasterFundName, @aggregateEntity) AS FundName
				,(DailyPerformance) / 100 AS DayPerformance
				,(WeekPerformance) / 100 AS WeekToDatePerformance
				,(MTDPerformance) / 100 AS MonthToDatePerformance
				,(QTDPerformance) / 100 AS QuarterToDatePerformance
				,(YTDPerformance) / 100 AS YearToDatePerformance
			FROM #PerformanceTable TempFWD
			INNER JOIN T_CompanymasterFunds CMF ON TempFWD.FundID = CMF.CompanyMasterFundID
			)
	
	UNION
	
	(
		SELECT TempFWD.FundId AS FundID
			,@aggregateEntity AS FundName
			,(DailyPerformance) / 100 AS DayPerformance
			,(WeekPerformance) / 100 AS WeekToDatePerformance
			,(MTDPerformance) / 100 AS MonthToDatePerformance
			,(QTDPerformance) / 100 AS QuarterToDatePerformance
			,(YTDPerformance) / 100 AS YearToDatePerformance
		FROM #PerformanceTable TempFWD
		WHERE TempFWD.FundId = - 1
		)
END
ELSE
BEGIN
	SELECT TempFWD.FundID AS FundID
		,isnull(CF.FundName, @aggregateEntity) AS FundName
		,DailyPerformance / 100 AS DayPerformance
		,MTDPerformance / 100 AS WeekToDatePerformance
		,WeekPerformance / 100 AS MonthToDatePerformance
		,QTDPerformance / 100 AS QuarterToDatePerformance
		,YTDPerformance / 100 AS YearToDatePerformance
	FROM #PerformanceTable TempFWD
	LEFT JOIN T_CompanyFunds CF ON CF.CompanyFundID = TempFWD.FundID
	ORDER BY FundID

	/***************                                      
Drop Temporary TABLES                       
******************/
	DROP TABLE #PerformanceTable
		,#accounts --#Funds,#TempFields,#TempFinalData
END
