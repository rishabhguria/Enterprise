/*************************************************  
Author : Narendra Kumar Jangir  
Creation date : Nov 11 , 2013  
Descritpion : Batch for Cash and accruals revaluation
Execution Method :   
exec [P_RunDailyCashAccrualsRevaluation] '2014-10-10','1182,1183,1184,1190',null
*************************************************/
CREATE PROCEDURE [dbo].[P_RunDailyCashAccrualsRevaluation] (
	@EndDate DATETIME
	,@FundIDs VARCHAR(max)
	,@userID INT
	,@isManualReval BIT
	)
AS
BEGIN
	CREATE TABLE #Funds (FundID INT)

	INSERT INTO #Funds
	SELECT Items AS FundID
	FROM dbo.Split(@FundIDs, ',')

	DECLARE @CurrentDate DATETIME
	--This variable will be required to get the funds for which the revaluation has to be done  
	DECLARE @NewFundIDs VARCHAR(max)

	SET NOCOUNT ON

	IF @isManualReval = 0
	BEGIN
		SELECT @CurrentDate = MIN(LastCalcDate)
		FROM T_LastCalcDateRevaluation
		WHERE FundID IN (
				SELECT FundID
				FROM #Funds
				)
	END
	ELSE
	BEGIN
		SET @CurrentDate = @EndDate
	END

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
	END

	SET @CurrentDate = CAST(FLOOR(CAST(@CurrentDate AS FLOAT)) AS DATETIME)
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
			IF @isManualReval = 0
			BEGIN
				SET @NewFundIDs = dbo.GetEligibleFundIDs(@FundIDs, @CurrentDate)
			END
			ELSE
			BEGIN
				SET @NewFundIDs = @FundIDs
			END

			EXEC [P_CalculateFXPNLOncash] @CurrentDate
				,@CurrentDate
				,@NewFundIDs
				,@userID

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

	--DROP TABLE T_TempAllActivity
	CREATE TABLE #T_LastCalculatedBalanceDate (
		FundID INT
		,MinCalcDate DATETIME
		)

	INSERT INTO #T_LastCalculatedBalanceDate
	SELECT FundID
		,min(Lastcalcdate)
	FROM T_LastCalculatedBalanceDate
	GROUP BY FundID

	UPDATE t1
	SET t1.LastCalcDate = t2.LastCalcDate
	FROM T_LastCalculatedBalanceDate t1
	INNER JOIN T_LastCalcDateRevaluation t2 ON t1.FundID = t2.FundID
	INNER JOIN #T_LastCalculatedBalanceDate T ON T.FundID = t1.FundID
	WHERE Datediff(d, T.MinCalcDate, t2.LastCalcDate) < 0
END