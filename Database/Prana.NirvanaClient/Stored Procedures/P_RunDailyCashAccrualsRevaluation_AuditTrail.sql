CREATE PROCEDURE [dbo].[P_RunDailyCashAccrualsRevaluation_AuditTrail] (
	@StartDate DATETIME
	,@EndDate DATETIME
	,@FundIDs VARCHAR(max)
	,@userID INT
	)
AS
BEGIN
	CREATE TABLE #Funds (FundID INT)
	declare @fundIdsForSymbolRevaluation varchar(1000)
	CREATE TABLE #FundsForSymbolWiseRevaluation (FundID INT)

	INSERT INTO #Funds
	SELECT Items AS FundID
	FROM dbo.Split(@FundIDs, ',')

	CREATE Table #ALLChangedData 
	(
		CurrencyID int,
		Fundid Varchar(Max),
		FromDate datetime,
		ToDate datetime
	)

	INSERT INTO #FundsForSymbolWiseRevaluation 
    SELECT CP.FundID FROM T_CashPreferences CP 
    INNER JOIN #Funds F ON CP.FundID=F.FundID
    WHERE CP.SymbolWiseRevaluationDate IS NOT NULL AND CP.SymbolWiseRevaluationDate<=@EndDate

	select @fundIdsForSymbolRevaluation = coalesce(@fundIdsForSymbolRevaluation+',','') + cast(FundID as varchar(10)) from #FundsForSymbolWiseRevaluation

	Insert into #ALLChangedData
	Exec P_GetCurrencyWiseChangedDataForRevaluation @EndDate,@FundIDs

	CREATE Table #ChangedData 
	(
		CurrencyID int,
		Fundid Varchar(Max),
		FromDate datetime,
		ToDate datetime
	)

	Insert Into #ChangedData(FromDate, ToDate, CurrencyID, Fundid)
	SELECT FromDate,ToDate, CurrencyID, Fundid =
			STUFF((SELECT ', ' + B.Fundid
			FROM #ALLChangedData B
			WHERE B.FromDate = A.FromDate AND B.ToDate = A.Todate AND B.CurrencyId = A.CurrencyId
			FOR XML PATH('')), 1, 1, '')
	FROM #ALLChangedData A
	GROUP BY A.FromDate,A.ToDate, A.CurrencyID	

	Alter Table #ChangedData
	Add ID int identity(1,1)

	DECLARE @CurrentDate DATETIME
	--This variable will be required to get the funds for which the revaluation has to be done  
	DECLARE @NewFundIDs VARCHAR(max)

	SET NOCOUNT ON

	--SELECT @CurrentDate = MIN(LastCalcDate)
	--FROM T_LastCalcDateRevaluation
	--WHERE FundID IN (
	--		SELECT FundID
	--		FROM #Funds
	--		)

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

	IF ((SELECT COUNT(*) FROM #ChangedData) >= 1)
	BEGIN
	 SELECT @increment = SUM(DATEDIFF(DAY, FromDate, ToDate)) FROM #ChangedData 
	  IF @increment = 0
    BEGIN
      SET @increment = 1
    END
		SET @increment = 50 / @increment
		SET @progress = 50
	END

	----------------------------------------------------------------------------------------
	DECLARE @ID int
	DECLARE @FromDate datetime
	DECLARE @FromDateForProgess datetime
	DECLARE @ToDate datetime
	DECLARE @Funds varchar(max)
	DECLARE @CurrencyID int

	SET @ID = 1


	IF ((SELECT COUNT(*) FROM #ChangedData) >= 1)
		WHILE (@ID <= (SELECT COUNT(*) FROM #ChangedData))
		BEGIN
			
	SELECT
      @CurrencyID = CurrencyID,
      @FromDate = FromDate,
	  @FromDateForProgess = FromDate,
      @ToDate = ToDate,
      @FundIDs = FundID
    FROM #ChangedData      
    WHERE ID = @ID

			SET @NewFundIDs = dbo.GetEligibleFundIDs(@FundIDs, @FromDate)
			---------------For Symbol Level Accrual Revaluation--------------------------------------------
			declare @NewFundIdsForSymbolLevelRevaluation varchar(50)=''

			SELECT @NewFundIdsForSymbolLevelRevaluation = coalesce(@NewFundIdsForSymbolLevelRevaluation+',','') + cast(Items as varchar(10)) from (
			select Items FROM dbo.Split(@NewFundIDs, ',')
			intersect
			SELECT Items FROM dbo.Split(@fundIdsForSymbolRevaluation, ','))
			as tempTable
			set @NewFundIdsForSymbolLevelRevaluation = stuff(@NewFundIdsForSymbolLevelRevaluation,1,1,'')
			--------------------------------------------------------------------------------------------------
			WHILE(@FromDate <= @ToDate)
			BEGIN
				EXEC [P_CalculateFXPNLOncash_AuditTrail] @FromDate
					,@FromDate
					,@NewFundIDs
					,@userID
					,@CurrencyID
				
				IF @NewFundIdsForSymbolLevelRevaluation is not null
				BEGIN
				EXEC [P_CalculateFXPNLOnSymbolLevelAccruals_AuditTrail] @FromDate
				,@FromDate
						,@NewFundIdsForSymbolLevelRevaluation
				,@userID
				,@CurrencyID
				END
				
				SET @FromDate = DATEADD(DAY, 1, @FromDate)
			END
			
			----Sending Progress to Code------------------------------------------------------
			SET @progress = @progress + (SELECT
      @increment * DATEDIFF(DAY, @FromDateForProgess, @Todate))
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
	FROM T_TempAllActivity
	WHERE FundID IN (
			SELECT FundID
			FROM #Funds
			)
IF @NewFundIdsForSymbolLevelRevaluation is not null
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
    (  FundID int,
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