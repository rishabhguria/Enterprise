
CREATE PROCEDURE [dbo].[P_RunDailyAssetRevaluation] (
	@StartDate DATETIME
	,@EndDate DATETIME
	,@FundIDs VARCHAR(max)
	,@userID INT
	,@isincludecommission BIT = 0
	,@isIncludeFXPNLonSwapForDiffSettleAndBaseCurr BIT = 1
	,@isIncludeFXPNLonSwapForSameSettleAndBaseCurr BIT = 1
	,@IsManualRevaluation BIT
	)
AS
BEGIN

      ----------------Log in trade Server logs, if @LogInFile=1 then it start logging, 0 for not ------------------- 
        DECLARE @LogInFile int
		set  @LogInFile =0

	   --------------------------------------------------------------------------------------------------------------
		

      RAISERROR (
				N'SP Run Daily Asset Revaluation started for Selected Funds.$%i$%i$Funds:%s'
				,0
				,1
				,@userID
				,@LogInFile
				,@FundIDs
				
				)
		WITH NOWAIT


	CREATE TABLE #Funds (FundID INT)

	INSERT INTO #Funds
	SELECT Items AS FundID
	FROM dbo.Split(@FundIDs, ',')

	DECLARE @CurrentDate DATETIME
	--This variable will be required to delete the Fx and Accruals Data for the fund for which the revaluation has to be done.     
	DECLARE @OriginalFundIDs VARCHAR(max)

	SET NOCOUNT ON

	SELECT @CurrentDate = MIN(LastCalcDate)
	FROM T_LastCalcDateRevaluation WITH (NOLOCK)
	WHERE FundID IN (
			SELECT FundID
			FROM #Funds
			)

	SET @OriginalFundIDs = dbo.GetEligibleFundIDs(@FundIDs, @CurrentDate)

	IF EXISTS (
			SELECT *
			FROM sys.objects
			WHERE object_id = OBJECT_ID(N'[dbo].[T_TempAllActivity]')
				AND type IN (N'U')
			)
	BEGIN
		DELETE T_TempAllActivity
		WHERE FundID IN (
				SELECT *
				FROM #Funds
				)
	END

	IF NOT EXISTS (
			SELECT *
			FROM sys.objects
			WHERE object_id = OBJECT_ID(N'[dbo].[T_TempAllActivity]')
				AND type IN (N'U')
			)
	BEGIN
	CREATE TABLE T_TempAllActivity (
		[ActivityTypeId_FK] [int] NOT NULL
		,[FKID] [varchar](50) NULL
		,[FundID] [int] NULL
		,[TransactionSource] [int] NOT NULL
		,[ActivitySource] [int] NOT NULL
		,[Symbol] [varchar](100) NULL
		,[Amount] [float] NULL
		,[CurrencyID] [int] NULL
		,[Description] [varchar](3000) NULL
		,[SideMultiplier] [int] NULL
		,[TradeDate] [datetime] NULL
		,[FxRate] [float] NULL
		,[FXConversionMethodOperator] VARCHAR(3) NULL
		,[ActivityState] VARCHAR(50) NULL
		,[ActivityNumber] INT NULL
		,[SubAccountID] INT NULL
		)

		CREATE NONCLUSTERED INDEX [NonClustered_FundID] ON [dbo].[T_TempAllActivity]
		(
			[FundID] ASC
		)

	END

	IF EXISTS (
			SELECT *
			FROM sys.objects
			WHERE object_id = OBJECT_ID(N'[dbo].[T_TempTradeAudit]')
				AND type IN (N'U')
			)
	BEGIN
		DELETE T_TempTradeAudit
		WHERE FundID IN (
				SELECT *
				FROM #Funds
				)
	END

	IF NOT EXISTS (
			SELECT *
			FROM sys.objects
			WHERE object_id = OBJECT_ID(N'[dbo].[T_TempTradeAudit]')
				AND type IN (N'U')
			)
	BEGIN
	CREATE TABLE T_TempTradeAudit (
		AuditID [int] NOT NULL
		,Symbol [varchar](Max) NULL
		,Action [int] NULL
		,FundId [int] NULL	
		,OriginalDate [datetime] NULL		
		)

		CREATE NONCLUSTERED INDEX [NonClustered_AccountID] ON [dbo].[T_TempTradeAudit]
		(
			[AuditID] ASC,
	        [FundId] ASC
		)

	END

	RAISERROR (
				N'Fetching Changed Data for Revaluation for Selected Funds.$%i$%i'
				,0
				,1
				,@userID
				,@LogInFile
				)WITH NOWAIT


	CREATE TABLE #T_ChangedDataforRevaluation (
		[Symbol] [varchar](200) NOT NULL
		,[FundID] [int] NOT NULL
		,[FromDate] [datetime] NOT NULL
		,[ToDate] [datetime] NOT NULL
		,[ID] [int] IDENTITY(1, 1) NOT NULL
		)
        CREATE Table #ChangedData 
        ( 
		CurrencyID int,
		Fundid Int,
		FromDate datetime,
		ToDate datetime
         )

	IF (@IsManualRevaluation = 1)
	BEGIN
		INSERT INTO #T_ChangedDataforRevaluation
		SELECT 'ALL_NIRVSYMBOL'
			,CAST(Items AS INT)
			,CAST(FLOOR(CAST(@StartDate AS FLOAT)) AS DATETIME)
			,CAST(FLOOR(CAST(@EndDate AS FLOAT)) AS DATETIME)
		FROM dbo.Split(@FundIDs, ',')
	END
	ELSE
	BEGIN
		INSERT INTO #T_ChangedDataforRevaluation
		EXEC [P_GetChangedDataForRevaluation] @EndDate
			,@FundIDs
			,@isincludecommission

        Insert into #ChangedData
	Exec P_GetCurrencyWiseChangedDataForRevaluation @EndDate,@FundIDs

	Alter Table #ChangedData
	Add ID int identity(1,1)
	END

		RAISERROR (
				N'Fetched Changed Data for Revaluation for Selected Funds.$%i$%i'
				,0
				,1
				,@userID
				,@LogInFile
				)WITH NOWAIT
	------------------------------------------------------      
	DECLARE @BaseCurrencyID INT

	SELECT @BaseCurrencyID = BaseCurrencyID
	FROM T_Company WITH (NOLOCK)

	ALTER TABLE #T_ChangedDataforRevaluation ADD CurrencyID INT;

	UPDATE CDFR
	SET CDFR.CurrencyID = G.CurrencyID
	FROM #T_ChangedDataforRevaluation CDFR
	INNER JOIN T_Group G ON G.Symbol = CDFR.Symbol

	UPDATE #T_ChangedDataforRevaluation
	SET CurrencyID = 0
	WHERE Symbol = 'ALL_NIRVSYMBOL'

	SELECT max(symbol) AS Symbol
		,FundID
		,min(FromDate) AS FromDate
		,max(ToDate) AS ToDate
		,CurrencyID
	INTO #T_ChangedDataforRevaluationActual
	FROM #T_ChangedDataforRevaluation CD
	WHERE CD.CurrencyID <> @BaseCurrencyID
	GROUP BY FundID
		,CurrencyID

	INSERT INTO #T_ChangedDataforRevaluationActual
	SELECT Symbol
		,FundID
		,FromDate
		,ToDate
		,CurrencyID
	FROM #T_ChangedDataforRevaluation CD
	WHERE CD.CurrencyID = @BaseCurrencyID

	ALTER TABLE #T_ChangedDataforRevaluationActual ADD ID INT IDENTITY (
		1
		,1
		)

	---------------------------------------------      
	-----Revaluation Progress parameters----------------------------------------------------  
	DECLARE @increment FLOAT
	DECLARE @progress FLOAT
	DECLARE @message INT

	IF (
			(
				SELECT COUNT(*)
				FROM #T_ChangedDataforRevaluationActual
				) >= 1
			)
	BEGIN
		SELECT @increment = SUM(DATEDIFF(DAY, FromDate, ToDate))
		FROM #T_ChangedDataforRevaluationActual

		IF @increment = 0
		BEGIN
			SET @increment = 1
		END

		SET @increment = 50 / @increment
		SET @progress = 0
	END

	SELECT *
	INTO #NonChangedFunds
	FROM #Funds
	WHERE Fundid NOT IN (
			SELECT Fundid
			FROM #T_ChangedDataforRevaluationActual
			)

	INSERT INTO T_TempAllActivity (
		[ActivityTypeId_FK]
		,[FKID]
		,[FundID]
		,[TransactionSource]
		,[ActivitySource]
		,[Symbol]
		,[Amount]
		,[CurrencyID]
		,[Description]
		,[SideMultiplier]
		,[TradeDate]
		,[FxRate]
		,[FXConversionMethodOperator]
		,[ActivityState]
		,[ActivityNumber]
		)
	SELECT [ActivityTypeId_FK]
		,[FKID]
		,act.[FundID]
		,[TransactionSource]
		,[ActivitySource]
		,[Symbol]
		,[Amount]
		,act.CurrencyID
		,[Description]
		,[SideMultiplier]
		,[TradeDate]
		,[FxRate]
		,[FXConversionMethodOperator]
		,'Deleted'
		,[ActivityNumber]
	FROM [dbo].T_AllActivity act WITH (NOLOCK)
	INNER JOIN #ChangedData fund    
        ON fund.FundID = act.FundID 
	INNER JOIN T_Currency cur WITH (NOLOCK)
		ON cur.CurrencyID = fund.CurrencyID 
      WHERE ActivitySource = 3    
      --Here Activity source 3 is Revaluation        
      AND     
      (    
      @IsManualRevaluation = 0    
      AND DATEDIFF(D, FromDate, TradeDate) >= 0    
      AND DATEDIFF(D, ToDate, TradeDate) <= 0    
      )    
      AND 
	    (
			(act.Symbol = cur.CurrencySymbol 
			OR act.CurrencyID = fund.CurrencyID)
		) 
      AND 
		 Description IN (
			'Cash PNL Entry'
			,'Contra Cash PNL Entry'
			,'Accruals PNL Entry'
			,'Contra Accruals PNL Entry'
			,'Cash to Unrealized FXRate PNL Entry'
			,'Unrealized To Cash FXRate PNL Entry'
			,'Contra Accrual DayEnd FXRate PNL Entry'
			,'Accrual to Unrealized DayEnd FXRate PNL Entry'
			,'Unrealized To Accrual DayEnd FXRate PNL Entry'
			,'Contra FXRate PNL Entry'
			,'Realized PNL To Cash  Entry'
			,'Cash to realized PNL entry'
			)

IF(@IsManualRevaluation =1)
BEGIN
	INSERT INTO T_TempAllActivity (
		[ActivityTypeId_FK]
		,[FKID]
		,[FundID]
		,[TransactionSource]
		,[ActivitySource]
		,[Symbol]
		,[Amount]
		,[CurrencyID]
		,[Description]
		,[SideMultiplier]
		,[TradeDate]
		,[FxRate]
		,[FXConversionMethodOperator]
		,[ActivityState]
		,[ActivityNumber]
		)
	SELECT [ActivityTypeId_FK]
		,[FKID]
		,[T_AllActivity].[FundID]
		,[TransactionSource]
		,[ActivitySource]
		,[Symbol]
		,[Amount]
		,T_AllActivity.CurrencyID
		,[Description]
		,[SideMultiplier]
		,[TradeDate]
		,[FxRate]
		,[FXConversionMethodOperator]
		,'Deleted'
		,[ActivityNumber]
	FROM [dbo].T_AllActivity WITH (NOLOCK)
	INNER JOIN #Funds fund    
        ON fund.FundID = T_AllActivity.FundID 
      WHERE ActivitySource = 3    
      --Here Activity source 3 is Revaluation        
      AND     
      (    
      @IsManualRevaluation = 1
	  AND DATEDIFF(D, @StartDate, TradeDate) >= 0    
      AND DATEDIFF(D, @EndDate, TradeDate) <= 0    
      )    
      AND 
		 Description IN (
			'Cash PNL Entry'
			,'Contra Cash PNL Entry'
			,'Accruals PNL Entry'
			,'Contra Accruals PNL Entry'
			,'Cash to Unrealized FXRate PNL Entry'
			,'Unrealized To Cash FXRate PNL Entry'
			,'Contra Accrual DayEnd FXRate PNL Entry'
			,'Accrual to Unrealized DayEnd FXRate PNL Entry'
			,'Unrealized To Accrual DayEnd FXRate PNL Entry'
			,'Contra FXRate PNL Entry'
			,'Realized PNL To Cash  Entry'
			,'Cash to realized PNL entry'
			)
END


SELECT FundID, FromDate,ToDate,  Symbol = 
    STUFF((SELECT ',' + Symbol
           FROM #T_ChangedDataforRevaluationActual b 
           WHERE b.FundID = a.FundID and b.FromDate = a.FromDate and b.ToDate = a.ToDate
          FOR XML PATH('')), 1, 1, '')
		  into #T_ChangedDataforRevaluationActualFinal
FROM #T_ChangedDataforRevaluationActual a
GROUP BY FundID, FromDate,ToDate
order by FundID,FromDate



ALTER TABLE #T_ChangedDataforRevaluationActualFinal ADD ID INT IDENTITY (
		1
		,1
		)

--select * from #T_ChangedDataforRevaluationActual
--select * from #T_ChangedDataforRevaluationActualFinal
	----------------------------------------------------------------------------------------  
		RAISERROR (
				N'Fetching Minimun Trade Date Revaluation for Selected Funds.$%i$%i'
				,0
				,1
				,@userID
				,@LogInFile
				)WITH NOWAIT

	Create Table #Temp_FundIds_ChangedDataforRevaluationActual
		(
		FundID Int,
		ToDate DateTime
		)

		CREATE TABLE #Temp_TaxlotPK 
		(
		TaxlotPK BigInt
		)

		Insert InTo #Temp_FundIds_ChangedDataforRevaluationActual
		Select Distinct FundID,ToDate From #T_ChangedDataforRevaluationActual

Insert InTo #Temp_TaxlotPK
	SELECT MAX(PMT.Taxlot_pk)
			FROM PM_taxlots PMT WITH (NOLOCK)
			INNER JOIN #Funds fnd ON fnd.FundID = PMT.FundID
			INNER JOIN T_CashPreferences preftab WITH (NOLOCK) ON preftab.FundID = PMT.FundID
				AND DATEDIFF(D, preftab.CashMgmtStartDate, PMT.AUECModifiedDate) >= 0
			--INNER JOIN #T_ChangedDataforRevaluationActual CD ON CD.FundID = PMT.FundID
			INNER JOIN #Temp_FundIds_ChangedDataforRevaluationActual CD ON CD.FundID = PMT.FundID
			WHERE DATEDIFF(D, PMT.Auecmodifieddate, DATEADD(D, - 1, CD.ToDate)) >= 0
			GROUP BY PMT.Taxlotid
declare @startdateformindate datetime 


IF (@IsManualRevaluation=1)
BEGIN
set @startdateformindate=@StartDate
END
else
begin
select   @startdateformindate =  min(fromdate) from #T_ChangedDataforRevaluationActualFinal
IF(@startdateformindate IS NULL)
BEGIN
set @startdateformindate=@EndDate
END
END
	DECLARE @MinTradeDate DATETIME
	DECLARE @DefaultAUECID INT
SET @DefaultAUECID = (
		SELECT TOP 1 DefaultAUECID
		FROM T_Company
		WHERE companyId <> - 1
		)
	SELECT PT.FundID
	,PT.Symbol
	,G.OriginalPurchaseDate
INTO #AUECLocalDate_1
FROM PM_Taxlots PT
INNER JOIN T_Group G ON PT.GroupID = G.GroupID
WHERE Taxlot_PK IN (
		SELECT Max(IPT.Taxlot_PK)
		FROM PM_Taxlots IPT
		WHERE DateDiff(d, IPT.AUECModifiedDate, dateadd(d, - 1, @startdateformindate)) > 0
		GROUP BY IPT.TaxlotID
		)
	AND TaxlotOpenQty <> 0
SELECT OriginalPurchaseDate, PT.FundID
INTO #AUECLocalDate
FROM #Funds FS
INNER JOIN #AUECLocalDate_1 PT ON FS.FundID = PT.FundID
	--AND (
	--	FS.Symbol = PT.Symbol
	--	OR FS.Symbol IS NULL
	--	)
	
SELECT fundid, Min(OriginalPurchaseDate) as Tradedate into #MinTradeDate
		FROM #AUECLocalDate
		WHERE DateDiff(d, OriginalPurchaseDate, '01/01/1800') <> 0
		group by FundID
DROP TABLE #AUECLocalDate_1
	,#AUECLocalDate


	----------------------------------------------------------------------------------------
	DECLARE @ID INT
	DECLARE @FromDate DATETIME
	DECLARE @ToDate DATETIME
	DECLARE @Symbols VARCHAR(MAX)
	DECLARE @TotalEntries INT
	--DECLARE @MinTradeDate DATETIME
	DECLARE @FinalDate DATETIME

	SELECT @FinalDate = CAST(FLOOR(CAST(@EndDate AS FLOAT)) AS DATETIME)

	SELECT @TotalEntries = MAX(ID) * 6
	FROM #T_ChangedDataforRevaluationActualFinal

	RAISERROR (
				N'Calculating CashPNL for Selected Funds.$%i$%i'
				,0
				,1
				,@userID
				,@LogInFile
				)WITH NOWAIT




	SET @ID = 1

	WHILE (
			@ID <= (
				SELECT COUNT(*)
				FROM #T_ChangedDataforRevaluationActualFinal
				)
			)
	BEGIN
		SELECT @Symbols = Symbol
			,@FromDate = FromDate
			,@ToDate = ToDate
			,@FundIDs = FundID
		FROM #T_ChangedDataforRevaluationActualFinal
		WHERE ID = @ID

		SET @MinTradeDate = (
				SELECT TradeDate
				FROM #MinTradeDate MTD
				INNER JOIN #T_ChangedDataforRevaluationActualFinal fund ON fund.FundID = MTD.FundID
				WHERE ID = @ID
				)

		--Select the fund IDs for which the revaluation date is less than current date    
		-- Kashish: Commented as now we need all fundid's  
		--SET @NewFundIDs = dbo.GetEligibleFundIDs(@FundIDs, @CurrentDate)  
		--Call the SP to get the daily cash PnL  
		EXEC [P_GetCashPNL] @FromDate
			,@ToDate
			,@Symbols
			,@FundIDs
			,@userID
			,@isincludecommission
			,@isIncludeFXPNLonSwapForDiffSettleAndBaseCurr
			,@isIncludeFXPNLonSwapForSameSettleAndBaseCurr
			,@TotalEntries
			,@ID
			,@MinTradeDate
			,@IsManualRevaluation
			,@FinalDate
			,@OriginalFundIDs

		----Sending Progress to Code------------------------------------------------------  
		SET @progress = @progress + (
				SELECT @increment * DATEDIFF(DAY, @FromDate, @Todate)
				)
		SET @message = @progress

		RAISERROR (
				'%i$%i%i'
				,0
				,1
				,@message
				,@userID
				,@LogInFile
				)
		WITH NOWAIT

		----------------------------------------------------------------------------------  
		SET @ID = @ID + 1
	END

	SELECT [ActivityTypeId_FK]
		,[FKID]
		,[FundID]
		,[TransactionSource]
		,[ActivitySource]
		,[Symbol]
		,[Amount]
		,[CurrencyID]
		,[Description]
		,[SideMultiplier]
		,[TradeDate]
		,[FxRate]
		,[FXConversionMethodOperator]
		,[ActivityState]
		,[ActivityNumber]
		,[SubAccountID]
	FROM T_TempAllActivity WITH (NOLOCK)
	WHERE FundID IN (
			SELECT FundID
			FROM #Funds
			)
 


	CREATE TABLE #T_LastCalculatedBalanceDate (
		FundID INT
		,MinCalcDate DATETIME
		)

	INSERT INTO #T_LastCalculatedBalanceDate
	SELECT FundID
		,MIN(Lastcalcdate)
	FROM T_LastCalculatedBalanceDate WITH (NOLOCK)
	WHERE FundID IN (SELECT FundID FROM #Funds)
	GROUP BY FundID

	UPDATE t1
	SET t1.LastCalcDate = t2.LastCalcDate
	FROM T_LastCalculatedBalanceDate t1 WITH (NOLOCK)
	INNER JOIN T_LastCalcDateRevaluation t2 WITH (NOLOCK) ON t1.FundID = t2.FundID
	INNER JOIN #T_LastCalculatedBalanceDate T ON T.FundID = t1.FundID
	WHERE DATEDIFF(D, T.MinCalcDate, t2.LastCalcDate) < 0

	RAISERROR (
				N'Completed Daily Asset Revaluation for Selected Funds.$%i$%i'
				,0
				,1
				,@userID
				,@LogInFile
				)WITH NOWAIT

	
     --DROP TABLE T_TempAllActivity
	DROP TABLE #T_ChangedDataforRevaluation

	DROP TABLE #MinTradeDate

	DROP TABLE #NonChangedFunds

	DROP TABLE #T_ChangedDataforRevaluationActual,#T_ChangedDataforRevaluationActualFinal
    DROP TABLE #ChangedData,#T_LastCalculatedBalanceDate,#Funds
	Drop Table #Temp_TaxlotPK, #Temp_FundIds_ChangedDataforRevaluationActual

END