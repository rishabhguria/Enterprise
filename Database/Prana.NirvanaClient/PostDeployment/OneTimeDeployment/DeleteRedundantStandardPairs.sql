DELETE
FROM T_CurrencyStandardPairs
WHERE T_CurrencyStandardPairs.CurrencyPairID NOT IN (
		SELECT DISTINCT CurrencyPairID_FK
		FROM T_CurrencyConversionRate
		)
