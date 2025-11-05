--exec [P_CalculateFXPNLOnSymbolLevelAccruals] '2019-07-24' , '2019-07-24','1',17

CREATE PROCEDURE [dbo].[P_CalculateFXPNLOnSymbolLevelAccruals](
	@StartDate DATETIME
	,@EndDate DATETIME
	,@FundIDs VARCHAR(max)
	,@userID INT = NULL )
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON;

----------------Table of fundIDs for calculating the PNL fundwise-----  
CREATE TABLE #Funds (FundID INT)

INSERT INTO #Funds
SELECT Items AS FundID
FROM dbo.Split(@FundIDs, ',')

----------------------------------------------------------------------
--get prior day to Start Date so that Holiday in not considerd      
DECLARE @DayPriorToStartDate DATETIME
SET @DayPriorToStartDate = DateAdd(Day, - 1, @StartDate)

DECLARE @CashSubAccounId INT
SELECT @CashSubAccounId = SubAccountID
FROM [dbo].T_SubAccounts
WHERE Acronym = 'Cash'

DECLARE @CashUnRealizedPNLActivityId INT
SELECT @CashUnRealizedPNLActivityId = ActivityTypeID
FROM [dbo].T_ActivityType
WHERE Acronym = 'CashUnrealizedPnL'

DECLARE @AccrualsUnRealizedPNLActivityId INT
SELECT @AccrualsUnRealizedPNLActivityId = ActivityTypeID
FROM [dbo].T_ActivityType
WHERE Acronym = 'AccrualsRevaluation'

----------------------------------------------------------------------------

-- get forex rates for 2 date ranges                                                                                                                                                                                                      
CREATE TABLE #FXConversionRates (
	FromCurrencyID INT
	,ToCurrencyID INT
	,RateValue FLOAT
	,ConversionMethod INT
	,DATE DATETIME
	,eSignalSymbol VARCHAR(max)
	,fundID INT
	)

--insert FX Rate for date ranges in the temp table                                                                                                                                                             
INSERT INTO #FXConversionRates
EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange @DayPriorToStartDate,@EndDate

-- Adjusting FxRates based on the conversion method....                                                                        
UPDATE #FXConversionRates
SET RateValue = 1.0 / RateValue
WHERE RateValue <> 0
	AND ConversionMethod = 1

 -- For Fund Zero    
 SELECT *    
 INTO #ZeroFundFxRate    
 FROM #FXConversionRates    
 WHERE fundID = 0 

CREATE TABLE #PNLTable (
	Rundate DATETIME
	,Symbol VARCHAR(100)
	,FundID INT
	,CurrencyID INT
	,Side VARCHAR(10)
	,BeginningFXRate FLOAT
	,EndingFXRate FLOAT
	,Open_CloseTag VARCHAR(50)
	,BeginningMarketValueLocal FLOAT
	,EndingMarketValueLocal FLOAT
	,SubAccountID INT
	)

-----------------------------------Start Section: Accrual Balances ------------------------------------                                                                                                                             
--Temporary table for balance handling                                    
CREATE TABLE #T_AccrualBalances (
	DATE DATETIME
	,FundID INT
	,LocalCurrencyID INT
	,Symbol VARCHAR(100)
	,SubAccountID INT
	,BeginningMarketValueLocal FLOAT
	,EndingMarketValueLocal FLOAT
	)

--Beginning Accrual Balance                                         
INSERT INTO #T_AccrualBalances
SELECT MAX(SubAccountBalances.TransactionDate) AS DATE
	,SubAccountBalances.FundID AS FundID
	,SubAccountBalances.CurrencyID AS LocalCurrencyID
	,SubAccountBalances.Symbol AS Symbol
	,SubAccountBalances.SubAccountID AS SubAccountID
	,SUM(IsNull(SubAccountBalances.CloseDRBal - SubAccountBalances.CloseCRBal, 0)) AS BeginningMarketValueLocal
	,0 AS EndingMarketValueLocal
FROM T_SymbolLevelAccrualsSubAccountBalances SubAccountBalances
INNER JOIN T_SubAccounts SubAccounts ON SubAccounts.SubAccountID = SubAccountBalances.SubAccountID
INNER JOIN T_TransactionType TransType ON SubAccounts.TransactionTypeId = TransType.TransactionTypeId
INNER JOIN #Funds fnd ON fnd.FundID = SubAccountBalances.FundID
INNER JOIN T_CashPreferences tcpref ON tcpref.FundID = SubAccountBalances.FundID
WHERE DateDiff(Day, SubAccountBalances.TransactionDate, @DayPriorToStartDate) = 0
	AND TransType.TransactionType = 'Accrued Balance'
	AND DATEDIFF(d, tcpref.CashMgmtStartDate, SubAccountBalances.TransactionDate) >= 0
GROUP BY SubAccountBalances.FundID
	,SubAccountBalances.Symbol
	,SubAccountBalances.CurrencyID
	,SubAccountBalances.SubAccountID

--Ending Accrual Balance                                         
INSERT INTO #T_AccrualBalances
SELECT MAX(SubAccountBalances.TransactionDate) AS DATE
	,SubAccountBalances.FundID AS FundID
	,SubAccountBalances.CurrencyID AS LocalCurrencyID
	,SubAccountBalances.Symbol AS Symbol
	,SubAccountBalances.SubAccountID AS SubAccountID
	,0 AS BeginningMarketValueLocal
	,SUM(IsNull(SubAccountBalances.CloseDRBal - SubAccountBalances.CloseCRBal, 0)) AS EndingMarketValueLocal
FROM T_SymbolLevelAccrualsSubAccountBalances SubAccountBalances
INNER JOIN T_SubAccounts SubAccounts ON SubAccounts.SubAccountID = SubAccountBalances.SubAccountID
INNER JOIN T_TransactionType TransType ON SubAccounts.TransactionTypeId = TransType.TransactionTypeId
INNER JOIN #Funds fnd ON fnd.FundID = SubAccountBalances.FundID
INNER JOIN T_CashPreferences tcpref ON tcpref.FundID = SubAccountBalances.FundID
WHERE DateDiff(Day, SubAccountBalances.TransactionDate, @EndDate) = 0
	AND TransType.TransactionType = 'Accrued Balance'
	AND DATEDIFF(d, tcpref.CashMgmtStartDate, SubAccountBalances.TransactionDate) >= 0
GROUP BY SubAccountBalances.FundID
	,SubAccountBalances.Symbol
	,SubAccountBalances.CurrencyID
	,SubAccountBalances.SubAccountID

-------------------------------------------------------------------------------------------------                        
INSERT INTO #PNLTable
SELECT @StartDate AS rundate
	,DailyBalance.Symbol AS Symbol
	,DailyBalance.FundID AS FundID
	,DailyBalance.LocalCurrencyID AS CurrencyID
	,CASE 
		WHEN SUM(DailyBalance.EndingMarketValueLocal) >= 0
			THEN 'Long'
		ELSE 'Short'
		END AS Side
	,Min(CASE 
			WHEN (DailyBalance.LocalCurrencyID) <> CF.LocalCurrency
				THEN CASE 
						WHEN FXDayRatesForStartDate.RateValue IS NULL
							THEN (IsNull(ZeroFundFxRateStartDate.RateValue, 0))
						ELSE FXDayRatesForStartDate.RateValue
						END
		ELSE 1
			END) AS BeginningFXRate
	,Min(CASE 
			WHEN (DailyBalance.LocalCurrencyID) <> CF.LocalCurrency
				THEN CASE 
						WHEN FXDayRatesForEndDate.RateValue IS NULL
							THEN (IsNull(ZeroFundFxRateEndDate.RateValue, 0))
						ELSE FXDayRatesForEndDate.RateValue
						END
		ELSE 1
		END) AS EndingFXRate
	,'Accruals' AS Open_CloseTag
	,SUM(BeginningMarketValueLocal) AS BeginningMarketValueLocal
	,SUM(EndingMarketValueLocal) AS EndingMarketValueLocal
	,DailyBalance.SubAccountID AS SubAccountID
FROM #T_AccrualBalances DailyBalance
LEFT OUTER JOIN T_CompanyFunds CF ON DailyBalance.FundID = CF.CompanyFundID
LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON CF.CompanyFundID = CMFSSAA.CompanyFundID
LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID
LEFT OUTER JOIN #FXConversionRates FXDayRatesForStartDate ON (
		FXDayRatesForStartDate.FromCurrencyID = DailyBalance.LocalCurrencyID
		AND FXDayRatesForStartDate.ToCurrencyID = CF.LocalCurrency
--		AND DateDiff(d, @RecentStartDateforNonZeroCash, FXDayRatesForStartDate.DATE) = 0
		AND DateDiff(d, @DayPriorToStartDate, FXDayRatesForStartDate.DATE) = 0
		AND FXDayRatesForStartDate.FundID = DailyBalance.FundID
			)
LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateStartDate ON (
		ZeroFundFxRateStartDate.FromCurrencyID = DailyBalance.LocalCurrencyID
		AND ZeroFundFxRateStartDate.ToCurrencyID = CF.LocalCurrency
--		AND DateDiff(d, @RecentStartDateforNonZeroCash, ZeroFundFxRateStartDate.DATE) = 0
		AND DateDiff(d, @DayPriorToStartDate, ZeroFundFxRateStartDate.DATE) = 0
		AND ZeroFundFxRateStartDate.FundID = 0
		)
LEFT OUTER JOIN #FXConversionRates FXDayRatesForEndDate ON (
		FXDayRatesForEndDate.FromCurrencyID = DailyBalance.LocalCurrencyID
		AND FXDayRatesForEndDate.ToCurrencyID = CF.LocalCurrency
--		AND DateDiff(d, @RecentEndDateforNonZeroCash, FXDayRatesForEndDate.DATE) = 0
		AND DateDiff(d, @EndDate, FXDayRatesForEndDate.DATE) = 0
		AND FXDayRatesForEndDate.FundID = DailyBalance.FundID
			)
LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateEndDate ON (
		ZeroFundFxRateEndDate.FromCurrencyID = DailyBalance.LocalCurrencyID
		AND ZeroFundFxRateEndDate.ToCurrencyID = CF.LocalCurrency
--		AND DateDiff(d, @RecentEndDateforNonZeroCash, ZeroFundFxRateEndDate.DATE) = 0
		AND DateDiff(d, @EndDate, ZeroFundFxRateEndDate.DATE) = 0
		AND ZeroFundFxRateEndDate.FundID = 0
		)
GROUP BY DailyBalance.FundID
    ,DailyBalance.Symbol
	,DailyBalance.LocalCurrencyID
	,DailyBalance.SubAccountID
	,CF.LocalCurrency
	

------------------------------ End Section: Accrual Balances  ----------------------------------------------------                           
------------------------------- Rounding all basic field to Two decimal for the symbol wise accruals ------------------------------------------                                                                                       
UPDATE #PNLTable
SET BeginningMarketValueLocal = Round(BeginningMarketValueLocal, 2)
	,EndingMarketValueLocal = Round(EndingMarketValueLocal, 2)

---------------------------------------------------Make journal entries-------------------------------------------

----------------------------------------Start of FX PNL entries on Accruals-------------------------------------  
CREATE TABLE #AccrualsFXPNL (
	Rundate DATETIME
	,Symbol VARCHAR(100)
	,FundId INT
	,Asset VARCHAR(100)
	,Side VARCHAR(10)
	,Cash FLOAT
	,CurrencyId INT
	,BeginningFxRate FLOAT
	,EndingFxRate FLOAT
	,SubAccountID INT
	)

-----------------------------------------------Insert data-----------------------------------------  
INSERT INTO #AccrualsFXPNL (
	Rundate
	,Symbol
	,FundId
	,Side
	,Cash
	,CurrencyID
	,BeginningFxRate
	,EndingFxRate
	,SubAccountID
	)
SELECT Rundate
	,Symbol
	,FundId
	,Side
	,BeginningMarketValueLocal AS Cash
	,CurrencyID
	,BeginningFxRate
	,EndingFxRate
	,SubAccountID
FROM #PNLTable
INNER JOIN T_CompanyFunds TCF ON TCF.CompanyFundID = FundId
WHERE CurrencyID <> TCF.LocalCurrency
	AND BeginningMarketValueLocal <> 0
	AND Open_CloseTag = 'Accruals'

---------------------------------End of FX PNL entries on Accruals----------------------------------------  

----following temporary table will be used to store journal data and journal data will be imported at once    
CREATE TABLE #T_AllActivity (
	[ActivityTypeId_FK] [int] NOT NULL
	,[FKID] [varchar](50) NULL
	,[FundID] [int] NULL
	,[TransactionSource] [int] NOT NULL
	,[ActivitySource] [int] NOT NULL
	,[Symbol] [varchar](100) NULL
	,[Amount] FLOAT NULL
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

-------------------------------------Start of Activities of FX PNL on Accruals------------------------------------  
INSERT INTO [dbo].[#T_AllActivity] (
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
	,[SubAccountID]
	)
SELECT
	--TODO: Use SubAccountID here      
	@AccrualsUnRealizedPNLActivityId AS ActivityTypeId_FK
	,NULL AS FKID
	,FundID
	,9 AS TransactionSource
	,3 AS ActivitySource
	,Symbol
	,Cash AS Amount
	,CurrencyId
	,'Accruals PNL Entry' AS Description
	,1 AS SideMultiplier
	,RunDate AS TradeDate
	,EndingFxRate AS FxRate
	,'M' AS FXConversionMethodOperator
	,'New'
	,1
	,SubAccountID
FROM #AccrualsFXPNL
WHERE Cash * (EndingFxRate - BeginningFxRate) <> 0

-------------------------------------End of Activities of FX PNL on Accruals------------------------------------  

-------------------------------------Start of Activities of contra FX PNL on Accruals--------------------------- 
INSERT INTO [dbo].[#T_AllActivity] (
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
	,[SubAccountID]
	)
SELECT
	--TODO: Use SubAccountID here      
	@AccrualsUnRealizedPNLActivityId AS ActivityTypeId_FK
	,NULL AS FKID
	,FundID
	,9 AS TransactionSource
	,3 AS ActivitySource
	,Symbol
	,Cash * (- 1) AS Amount
	,CurrencyId
	,'Contra Accruals PNL Entry' AS Description
	,1 AS SideMultiplier
	,RunDate AS TradeDate
	,BeginningFxRate AS FxRate
	,'M' AS FXConversionMethodOperator
	,'New'
	,1
	,SubAccountID
FROM #AccrualsFXPNL
WHERE Cash * (EndingFxRate - BeginningFxRate) <> 0

-------------------------------------End of Activities of contra FX PNL on Accruals------------------------------------
----------------------------Accrual PNL Entry for DAY END FX RATE-----------------------------------------------------------
---Contra Entry Activity-------------
INSERT INTO [dbo].[#T_AllActivity] (
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
	,[SubAccountID]
	)
SELECT @AccrualsUnRealizedPNLActivityId AS ActivityTypeId_FK
	,NULL AS FKID
	,min(journal.FundID) AS FundID
	,9 AS TransactionSource
	,3 AS ActivitySource
	,min(journal.Symbol) AS Symbol
	,(sum(dr) - sum(cr)) * - 1 AS Amount
	,journal.CurrencyID AS CurrencyID
	,'Contra Accrual DayEnd FXRate PNL Entry' AS Description
	,1 AS SideMultiplier
	,min(TransactionDate) AS TradeDate
	,isnull(max(journal.fxrate), 1) AS FxRate
	,isNull(journal.FXConversionMethodOperator,'M') AS FXConversionMethodOperator
	,'New' AS ActivityState
	,1 AS ActivityNumber
	,journal.SubAccountID
FROM T_SymbolLevelAccrualsJournal journal
INNER JOIN #Funds fnd ON fnd.FundID = journal.FundID
INNER JOIN t_subaccounts sub ON journal.subaccountid = sub.subaccountid
INNER JOIN t_transactiontype acctype ON sub.transactionTypeId = acctype.transactionTypeId
INNER JOIN T_CompanyFunds TCF ON TCF.CompanyFundID = journal.FundID
INNER JOIN T_AllActivity ON Activityid_fk = Activityid
LEFT OUTER JOIN #FXConversionRates FXDayRatesForEndDate ON (
		FXDayRatesForEndDate.FromCurrencyID = journal.CurrencyID
		AND FXDayRatesForEndDate.ToCurrencyID = TCF.LocalCurrency
		AND DateDiff(d, TransactionDate, FXDayRatesForEndDate.DATE) = 0
		AND FXDayRatesForEndDate.FundID = journal.FundID
			)
LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateEndDate ON (
		ZeroFundFxRateEndDate.FromCurrencyID = journal.CurrencyID
		AND ZeroFundFxRateEndDate.ToCurrencyID = TCF.LocalCurrency
		AND DateDiff(d, TransactionDate, ZeroFundFxRateEndDate.DATE) = 0
		AND ZeroFundFxRateEndDate.FundID = 0
		)
WHERE acctype.TransactionTypeAcronym = 'ACB'
	AND journal.CurrencyID <> TCF.LocalCurrency
	AND journal.fxrate <> 0
	AND (
(
			DateDiff(d, TradeDate, isNull(Settlementdate,GETDATE())) > 0
			AND journal.ActivitySource = 2
			)
		OR journal.ActivitySource <> 2
)
GROUP BY transactionid
	,journal.CurrencyID
	,journal.FXConversionMethodOperator
	,TCF.LocalCurrency
	,ActivityTypeId_FK
	,TradeDate
	,Settlementdate
	,journal.SubAccountID
HAVING (
		datediff(d, max(journal.transactiondate), @enddate) = 0
		AND max(journal.CurrencyID) <> TCF.LocalCurrency
		AND (
			(
				max(journal.fxrate) <> max(IsNull(FXDayRatesForEndDate.RateValue, ZeroFundFxRateEndDate.RateValue))
				AND journal.FXConversionMethodOperator = 'M'
				)
			OR (
				journal.FXConversionMethodOperator = 'D'
				AND (1 / max(journal.fxrate)) <> max(IsNull(FXDayRatesForEndDate.RateValue, ZeroFundFxRateEndDate.RateValue))
				)
			)
		AND (sum(dr) - sum(cr)) <> 0
		)

---Accrual to Unrealized OR Unrealized to Accrual Entry Activity-------------
INSERT INTO [dbo].[#T_AllActivity] (
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
	,[SubAccountID]
	)
SELECT @AccrualsUnRealizedPNLActivityId AS ActivityTypeId_FK
	,NULL AS FKID
	,min(journal.FundID) AS FundID
	,9 AS TransactionSource
	,3 AS ActivitySource
	,min(journal.Symbol) AS Symbol
	,(sum(dr) - sum(cr)) AS Amount
	,journal.CurrencyID AS CurrencyID
	,CASE 
		WHEN (sum(dr) - sum(cr)) > 0
			THEN 'Accrual to Unrealized DayEnd FXRate PNL Entry'
		ELSE 'Unrealized To Accrual DayEnd FXRate PNL Entry'
		END AS Description
	,1 AS SideMultiplier
	,min(TransactionDate) AS TradeDate
	,Min(CASE 
			WHEN FXDayRatesForEndDate.RateValue IS NULL
				THEN CASE 
						WHEN ZeroFundFxRateEndDate.RateValue IS NULL
							THEN 1
						ELSE ZeroFundFxRateEndDate.RateValue
						END
			ELSE FXDayRatesForEndDate.RateValue
			END) AS FxRate
	,'M' AS FXConversionMethodOperator
	,'New' AS ActivityState
	,1 AS ActivityNumber
	,journal.SubAccountID
FROM T_SymbolLevelAccrualsJournal journal
INNER JOIN #Funds fnd ON fnd.FundID = journal.FundID
INNER JOIN t_subaccounts sub ON journal.subaccountid = sub.subaccountid
INNER JOIN T_CompanyFunds TCF ON TCF.CompanyFundID = fnd.FundID
INNER JOIN t_transactiontype acctype ON sub.transactionTypeId = acctype.transactionTypeId
INNER JOIN T_AllActivity ON Activityid_fk = Activityid
LEFT OUTER JOIN #FXConversionRates FXDayRatesForEndDate ON (
		FXDayRatesForEndDate.FromCurrencyID = journal.CurrencyID
		AND FXDayRatesForEndDate.ToCurrencyID = TCF.LocalCurrency
		AND DateDiff(d, TransactionDate, FXDayRatesForEndDate.DATE) = 0
		AND FXDayRatesForEndDate.FundID = journal.FundID
			)
LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateEndDate ON (
		ZeroFundFxRateEndDate.FromCurrencyID = journal.CurrencyID
		AND ZeroFundFxRateEndDate.ToCurrencyID = TCF.LocalCurrency
		AND DateDiff(d, TransactionDate, ZeroFundFxRateEndDate.DATE) = 0
		AND ZeroFundFxRateEndDate.FundID = 0
		)
WHERE acctype.TransactionTypeAcronym = 'ACB'
	AND journal.CurrencyID <> TCF.LocalCurrency
	AND journal.fxrate <> 0
	AND (
(
			DateDiff(d, TradeDate, isNull(Settlementdate,GETDATE())) > 0
			AND journal.ActivitySource = 2
			)
		OR journal.ActivitySource <> 2
)
GROUP BY transactionid
	,journal.CurrencyID
	,journal.FXConversionMethodOperator
	,TCF.LocalCurrency
	,ActivityTypeId_FK
	,TradeDate
	,Settlementdate
	,journal.SubAccountID
HAVING (
		datediff(d, max(journal.transactiondate), @enddate) = 0
		AND max(journal.CurrencyID) <> TCF.LocalCurrency
		AND (
			(
				max(journal.fxrate) <> max(IsNull(FXDayRatesForEndDate.RateValue, ZeroFundFxRateEndDate.RateValue))
				AND journal.FXConversionMethodOperator = 'M'
				)
			OR (
				journal.FXConversionMethodOperator = 'D'
				AND (1 / max(journal.fxrate)) <> max(IsNull(FXDayRatesForEndDate.RateValue, ZeroFundFxRateEndDate.RateValue))
				)
			)
		AND (sum(dr) - sum(cr)) <> 0
		)

---End of Accrual to Unrealized OR Unrealized to Accrual Entry Activity-------------
------------------------------End of Accrual PNL Entry for DAY END FX RATE-----------------------------------------------------------  
	
INSERT INTO T_IntermediateSymbolLevelAccrualAllActivity
SELECT *
FROM #T_AllActivity

DROP TABLE #FXConversionRates
	,#T_AccrualBalances
	,#PNLTable
	,#AccrualsFXPNL
	,#ZeroFundFxRate

