CREATE PROCEDURE [dbo].[P_GetTradingRulesPreferences]
	@id INT  
AS  
  
SELECT   IsOversellTradingRule
		,IsOverbuyTradingRule
		,IsUnallocatedTradeAlert
		,IsFatFingerTradingRule
		,IsDuplicateTradeAlert
		,IsPendingNewTradeAlert
		,DefineFatFingerValue
		,DuplicateTradeAlertTime
		,PendingNewOrderAlertTime
		,FatFingerAccountOrMasterFund
		,IsAbsoluteAmountOrDefinePercent
		,IsInMarketIncluded
		,IsSharesOutstandingRule
		,SharesOutstandingAccountOrMF
		,SharesOutstandingPercent
 
FROM T_TradingRulesPreferences  
WHERE CompanyID = @id  