
-----------------------------------------------------------------
--Created BY: Bharat Raturi
--Date: 2-july-14
--Purpose: Get the cash preferences from DB
--usage: P_GetAllCashPreferences
-----------------------------------------------------------------
CREATE PROCEDURE [dbo].[P_GetAllCashPreferences]
AS
SELECT FundID
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
FROM T_CashPreferences