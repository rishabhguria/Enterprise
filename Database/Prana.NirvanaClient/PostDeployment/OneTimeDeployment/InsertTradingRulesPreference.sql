TRUNCATE TABLE T_TradingRulesPreferences

SELECT T_Company.CompanyID
	,0 AS IsOversellTradingRule
	,0 AS IsOverbuyTradingRule
	,0 AS IsUnallocatedTradeAlert
	,0 AS IsFatFingerTradingRule
	,1 AS IsDuplicateTradeAlert
	,0 AS IsPendingNewTradeAlert
	,5 AS DefineFatFingerValue
	,30 AS DuplicateTradeAlertTime
	,300 AS PendingNewOrderAlertTime
	,0 AS FatFingerAccountOrMasterFund
	,0 AS IsAbsoluteAmountOrDefinePercent
	,0 AS IsInMarketIncluded
	,0 AS IsSharesOutstandingRule
	,0 AS SharesOutstandingAccountOrMF
	,0 AS SharesOutstandingPercent
INTO #temp_TradingRulesPrefs
FROM T_Company

INSERT INTO T_TradingRulesPreferences
SELECT *
FROM #temp_TradingRulesPrefs

DROP TABLE #temp_TradingRulesPrefs