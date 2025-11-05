SELECT FundID, CashMgmtStartDate
INTO #temp_WashSalePrefs
FROM T_CashPreferences

INSERT INTO T_WashSalePreferences
SELECT CashMgmtStartDate ,FundID
FROM #temp_WashSalePrefs

DROP TABLE #temp_WashSalePrefs


