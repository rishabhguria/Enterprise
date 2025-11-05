IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'T_TradingRulesPreferences')
BEGIN 
Update T_TradingRulesPreferences
set IsAbsoluteAmountOrDefinePercent =0 where IsAbsoluteAmountOrDefinePercent is Null

End