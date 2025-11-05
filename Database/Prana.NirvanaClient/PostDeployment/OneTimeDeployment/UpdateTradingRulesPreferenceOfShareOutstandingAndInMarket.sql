IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='T_TradingRulesPreferences' AND COLUMN_NAME='IsInMarketIncluded')
BEGIN
	UPDATE T_TradingRulesPreferences SET IsInMarketIncluded = 0 WHERE IsInMarketIncluded IS NULL

	UPDATE T_TradingRulesPreferences SET IsSharesOutstandingRule = 0,
	SharesOutstandingAccountOrMF = 0,
	SharesOutstandingPercent = 0
	WHERE IsSharesOutstandingRule IS NULL

END