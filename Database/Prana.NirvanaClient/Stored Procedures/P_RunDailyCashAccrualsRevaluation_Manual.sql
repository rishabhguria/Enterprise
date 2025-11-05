CREATE PROCEDURE [dbo].[P_RunDailyCashAccrualsRevaluation_Manual] (
	@StartDate DATETIME
	,@EndDate DATETIME
	,@FundIDs VARCHAR(max)
	,@userID INT
	)
AS
BEGIN
	declare @fundIdsForSymbolRevaluation varchar(1000)
	CREATE TABLE #FundsForSymbolWiseRevaluation (FundID INT)
	CREATE TABLE #Funds (FundID INT)

	INSERT INTO #Funds
	SELECT Items AS FundID
	FROM dbo.Split(@FundIDs, ',')

	INSERT INTO #FundsForSymbolWiseRevaluation 
    SELECT CP.FundID FROM T_CashPreferences CP 
    INNER JOIN #Funds F ON CP.FundID=F.FundID
    WHERE CP.SymbolWiseRevaluationDate IS NOT NULL AND CP.SymbolWiseRevaluationDate<=@EndDate

	select @fundIdsForSymbolRevaluation = coalesce(@fundIdsForSymbolRevaluation+',','') + cast(FundID as varchar(10)) from #FundsForSymbolWiseRevaluation

	DECLARE @CurrentDate DATETIME

	SET NOCOUNT ON

	SET @CurrentDate = @StartDate

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

		DELETE T_IntermediateSymbolLevelAccrualAllActivity
		WHERE FundID IN (
				SELECT *
				FROM #FundsForSymbolWiseRevaluation
			)

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
	END

	SET @CurrentDate= CAST(FLOOR(CAST(@CurrentDate AS FLOAT)) AS DATETIME)
	SET @EndDate = CAST(FLOOR(CAST(@EndDate AS FLOAT)) AS DATETIME)
	-----Revaluation Progress parameters----------------------------------------------------
	DECLARE @increment FLOAT
	DECLARE @progress FLOAT
	DECLARE @message INT

	IF (DATEDIFF(day, @CurrentDate, @EndDate) >= 0)
	BEGIN
		SET @increment = DATEDIFF(day, @CurrentDate - 1, @EndDate)
		SET @increment = 50 / @increment
		SET @progress = 50
	END

	----------------------------------------------------------------------------------------
	IF (DATEDIFF(day, @CurrentDate, @EndDate) >= 0)
		WHILE (DATEDIFF(day, @CurrentDate, @EndDate) >= 0)
		BEGIN		

			EXEC [P_CalculateFXPNLOncash] @CurrentDate
				,@CurrentDate
				,@FundIDs
				,@userID
			IF @fundIdsForSymbolRevaluation is not null
			BEGIN
				EXEC [P_CalculateFXPNLOnSymbolLevelAccruals] @CurrentDate
					,@CurrentDate
					,@fundIdsForSymbolRevaluation
					,@userID
			END

			----Sending Progress to Code------------------------------------------------------
			SET @progress = @progress + @increment
			SET @message = @progress

			RAISERROR (
					'%i$%i'
					,0
					,1
					,@message
					,@userID
					)
			WITH NOWAIT

			----------------------------------------------------------------------------------
			SET @CurrentDate = DATEADD(day, 1, @CurrentDate)
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
	FROM T_TempAllActivity
	WHERE FundID IN (
			SELECT FundID
			FROM #Funds
			)
IF @fundIdsForSymbolRevaluation is not null
BEGIN
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
	FROM T_IntermediateSymbolLevelAccrualAllActivity
	WHERE FundID IN (
			SELECT FundID
			FROM #FundsForSymbolWiseRevaluation
			)
END

	CREATE TABLE #T_LastCalculatedBalanceDate
    (  
	   FundID int,
       MinCalcDate Datetime
    )

    INSERT INTO #T_LastCalculatedBalanceDate
    Select FundID,min(Lastcalcdate)
    from T_LastCalculatedBalanceDate
	WHERE FundID IN (SELECT FundID FROM #Funds)
    Group By FundID

    UPDATE t1  
    SET t1.LastCalcDate = t2.LastCalcDate  
    FROM T_LastCalculatedBalanceDate t1  
    INNER JOIN T_LastCalcDateRevaluation t2 ON t1.FundID = t2.FundID 
    INNER JOIN #T_LastCalculatedBalanceDate T
    ON T.FundID=t1.FundID
    where Datediff(d,T.MinCalcDate,t2.LastCalcDate)<0

END