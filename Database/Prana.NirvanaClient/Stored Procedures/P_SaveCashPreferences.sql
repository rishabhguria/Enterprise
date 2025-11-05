
-----------------------------------------------------------------
--Created By: Manvendra Prajapati
--Date: 05-Feb-15
--http://jira.nirvanasolutions.com:8080/browse/PRANA-5923
--Purpose: To change the table name from T_JournalManualTransactionBackup to T_JournalManualTransaction
-----------------------------------------------------------------
CREATE PROCEDURE [dbo].[P_SaveCashPreferences] (@xmlPreferences NTEXT)
AS
DECLARE @handle1 INT
DECLARE @savecount INT

EXEC sp_xml_preparedocument @handle1 OUTPUT
	,@xmlPreferences

BEGIN TRANSACTION;

BEGIN TRY
	-------------------------------------------------------------------------------
	CREATE TABLE #TempPrefs (
		colFundID INT
		,colCashMgmtStartDate DATETIME
		,colMarginPercentage VARCHAR(50)
		,colCollateralFrequencyInterest VARCHAR(50)
		,colIsCalculatePnL BIT
		,colIsCalulateDividend BIT
		,colIsCalculateBondAccural BIT
		,colIsCalculateCollateral  BIT
		,colIsBreakRealizedPnlSubaccount BIT
		,colIsBreakTotalIntoTradingandfxPnl BIT
		,colStartDateChanged BIT
		,colIsCashSettlementEntriesVisible BIT
  ,colIsAccruedTillSettlement BIT 
  ,colSymbolWiseRevaluationDate DATETIME 
  ,IsCreateManualJournals BIT
		)

	INSERT INTO #TempPrefs (
		colFundID
		,colCashMgmtStartDate
		,colMarginPercentage
		,colCollateralFrequencyInterest
		,colIsCalculatePnL
		,colIsCalulateDividend
		,colIsCalculateBondAccural
		,colIsCalculateCollateral
		,colIsBreakRealizedPnlSubaccount
		,colIsBreakTotalIntoTradingandfxPnl
		,colStartDateChanged
		,colIsCashSettlementEntriesVisible
  ,colIsAccruedTillSettlement  
  ,colSymbolWiseRevaluationDate
  ,IsCreateManualJournals
		)
	SELECT AccountID
		,CashMgmtStartDate
		,MarginPercentage
		,CalculatedCollateralInterestFrequency
		,CalculatedPnL
		,CalculatedDividend
		,CalculatedBondAccrual
		,CalculatedCollateral
		,CalculatedIsBreakRealizedPnlSubaccount
		,CalculatedIsBreakTotalIntoTradingandfxPnl
		,isStartDateAdvanced
		,IsCashSettlementEntriesVisible
  ,IsAccruedTillSettlement  
  ,SymbolWiseRevaluationDate
  ,IsCreateManualJournals
	FROM openXML(@handle1, 'DsPref/DtPref', 2) WITH (
			AccountID INT
			,CashMgmtStartDate DATETIME
			,MarginPercentage VARCHAR(50)
			,CalculatedCollateralInterestFrequency  VARCHAR(50)
			,CalculatedPnL BIT
			,CalculatedDividend BIT
			,CalculatedBondAccrual BIT
			,CalculatedCollateral BIT
			,CalculatedIsBreakRealizedPnlSubaccount BIT
			,CalculatedIsBreakTotalIntoTradingandfxPnl BIT
			,isStartDateAdvanced BIT
			,IsCashSettlementEntriesVisible BIT
   ,IsAccruedTillSettlement BIT  
   ,SymbolWiseRevaluationDate DATETIME
   ,IsCreateManualJournals BIT
			)

	----------------------To Get the List of Changed Funds--------------------
	SELECT #TempPrefs.*
	INTO #TempChangedData
	FROM #TempPrefs
	INNER JOIN T_CashPreferences ON T_CashPreferences.FundID = #TempPrefs.colFundID
	WHERE DATEDIFF(d, T_CashPreferences.CashMgmtStartDate, #TempPrefs.colCashMgmtStartDate) <> 0 --T_CashPreferences.CashMgmtStartDate<>#TempPrefs.colCashMgmtStartDate 

	----------------------End Of To Get the List of Changed Funds--------------------
	---------------------------------------------------------------------------------------------------
	DELETE
	FROM T_CashPreferences
	WHERE FundID NOT IN (
			SELECT colFundID
			FROM #TempPrefs
			)

	UPDATE T_CashPreferences
	SET T_CashPreferences.CashMgmtStartDate = #TempPrefs.colCashMgmtStartDate
		,T_CashPreferences.MarginPercentage = #TempPrefs.colMarginPercentage
		,T_CashPreferences.CollateralFrequencyInterest= #TempPrefs.colCollateralFrequencyInterest
		,T_CashPreferences.IsCalculatePnL = #TempPrefs.colIsCalculatePnL
		,T_CashPreferences.IsCalulateDividend = #TempPrefs.colIsCalulateDividend
		,T_CashPreferences.IsCalculateBondAccural = #TempPrefs.colIsCalculateBondAccural
		,T_CashPreferences.IsCalculateCollateral= #TempPrefs.colIsCalculateCollateral
		,T_CashPreferences.IsBreakRealizedPnlSubaccount = #TempPrefs.colIsBreakRealizedPnlSubaccount
		,T_CashPreferences.IsBreakTotalIntoTradingAndFxPnl= #TempPrefs.colIsBreakTotalIntoTradingandfxPnl
		,T_CashPreferences.IsCashSettlementEntriesVisible = #TempPrefs.colIsCashSettlementEntriesVisible
  ,T_CashPreferences.IsAccruedTillSettlement = #TempPrefs.colIsAccruedTillSettlement  
  ,T_CashPreferences.SymbolWiseRevaluationDate = #TempPrefs.colSymbolWiseRevaluationDate
  ,T_CashPreferences.IsCreateManualJournals = #TempPrefs.IsCreateManualJournals
  
	FROM T_CashPreferences
	INNER JOIN #TempPrefs ON T_CashPreferences.FundID = #TempPrefs.colFundID

	--------------------------------------------------------------------------------------
	INSERT INTO T_CashPreferences (
		ID
		,FundID
		,CashMgmtStartDate
		,MarginPercentage
		,CollateralFrequencyInterest
		,IsCalculatePnL
		,IsCalulateDividend
		,IsCalculateBondAccural
		,IsCalculateCollateral
		,IsBreakRealizedPnlSubaccount
		,IsBreakTotalIntoTradingAndFxPnl
		,IsCashSettlementEntriesVisible
  ,IsAccruedTillSettlement  
  ,SymbolWiseRevaluationDate
  ,IsCreateManualJournals
		)
	SELECT colFundID
		,colFundID
		,colCashMgmtStartDate
		,colMarginPercentage
		,colCollateralFrequencyInterest
		,colIsCalculatePnL
		,colIsCalulateDividend
		,colIsCalculateBondAccural
		,colIsCalculateCollateral
		,colIsBreakRealizedPnlSubaccount
		,colIsBreakTotalIntoTradingandfxPnl
		,colIsCashSettlementEntriesVisible
  ,colIsAccruedTillSettlement  
  ,colSymbolWiseRevaluationDate
  ,IsCreateManualJournals
	FROM #TempPrefs
	WHERE colFundID NOT IN (
			SELECT FundID
			FROM T_CashPreferences
			)

	------------------------------------------------------------------------------------------------------------------------------
	--insert newly added accounts in T_WashSalePreferences
	INSERT INTO T_WashSalePreferences(
		FundID,
		WashSaleStartDate
	)
	SELECT colFundID, colCashMgmtStartDate
	FROM #TempPrefs
	WHERE colFundID NOT IN (
		SELECT FundID
		FROM T_WashSalePreferences
	)

	------------------------------------------------------------------------------------------------------------------------------
	--insert the manual transactions before the cash management start date into another table
	INSERT INTO T_JournalManualTransaction (
		TaxLotID
		,FundID
		,SubAccountID
		,CurrencyID
		,Symbol
		,PBDesc
		,TransactionDate
		,TransactionID
		,DR
		,CR
		,TransactionSource
		,TransactionEntryID
		,TransactionNumber
		,AccountSide
		,FxRate
		,FXConversionMethodOperator
		,ActivityId_FK
		,ActivitySource
		,InsertionDate
		)
	SELECT TaxLotID
		,FundID
		,SubAccountID
		,CurrencyID
		,Symbol
		,PBDesc
		,TransactionDate
		,TransactionID
		,DR
		,CR
		,TransactionSource
		,TransactionEntryID
		,TransactionNumber
		,AccountSide
		,FxRate
		,FXConversionMethodOperator
		,ActivityId_FK
		,ActivitySource
		,GETDATE()
	FROM #TempPrefs
	INNER JOIN T_Journal ON #TempPrefs.colFundID = T_Journal.FundID
	WHERE DATEDIFF(d, TransactionDate, colCashMgmtStartDate) >= 0 --TransactionDate<colCashMgmtStartDate 
		AND TransactionSource = 2

	--Update last calculated balance date in T_LastCalculatedBalanceDate
	UPDATE calc
 SET LastCalcDate = colCashMgmtStartDate
	FROM T_LastCalculatedBalanceDate calc
	INNER JOIN #TempPrefs tp ON tp.colFundID = calc.FundID
 WHERE tp.colFundID = calc.FundID and calc.FundId IN(SELECT colFundID  
   FROM #TempChangedData )

	--Remove the transactions from db that are before the cash management start date
	DELETE T_Journal
	FROM #TempPrefs
	INNER JOIN T_Journal ON #TempPrefs.colFundID = T_Journal.FundID
	WHERE (
			DATEDIFF(d, TransactionDate, colCashMgmtStartDate) = 0
			AND T_Journal.TransactionSource <> 8
			)
		OR (DATEDIFF(d, TransactionDate, colCashMgmtStartDate) > 0)

	--TransactionDate<colCashMgmtStartDate
	-- Deleting all the data from T_SubaccountBalances for which cashmanagement start date is changed
	--So that data will be calculated cleanly from start
	DELETE T_SubaccountBalances
	WHERE FundID IN (
			SELECT colFundID
			FROM #TempChangedData
			)

	--insert the new funds
	INSERT INTO T_LastCalcDateRevaluation (
		FundID
		,LastCalcDate
                ,LastRevalRunDate
		)
	SELECT f.colFundID
		,f.colCashMgmtStartDate
                ,f.colCashMgmtStartDate  
	FROM #TempPrefs f
	LEFT JOIN T_LastCalcDateRevaluation rev ON f.colFundID = rev.FundID
	WHERE rev.LastCalcDate IS NULL

	DROP TABLE #TempPrefs

	DROP TABLE #TempChangedData

	COMMIT TRANSACTION;

	EXEC sp_xml_removedocument @handle1

END TRY

BEGIN CATCH
	ROLLBACK TRANSACTION;
END CATCH