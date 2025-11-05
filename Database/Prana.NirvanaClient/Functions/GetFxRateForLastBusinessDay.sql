-- ==============================================================                             
-- Description: This function returns a table with Fx rates   
-- for all currency pairs based on fx rates available on last business day.                         
-- select * from dbo.[GetFxRateForLastBusinessDay]('2015-07-12')              
-- ==============================================================   
CREATE FUNCTION [dbo].[GetFxRateForLastBusinessDay] (@UTCDate DATETIME)
RETURNS @DayFXRates TABLE (
	FromCurrencyID INT
	, ToCurrencyID INT
	, RateValue FLOAT
	, ConversionMethod INT
	, DATE DATETIME
	, eSignalSymbol VARCHAR(100)
	, FundID INT
	)
AS
BEGIN
		;
	WITH ConversionRate (
		CurrencyPairId
		, FromCurrencyID
		, ToCurrencyID
		, ConversionMethod
		, DATE
		, eSignalSymbol
		, FundID
		)
	AS (
		SELECT StanPair.CurrencyPairId
			, StanPair.FromCurrencyID AS FromCurrencyID
			, StanPair.ToCurrencyID AS ToCurrencyID
			, 0 AS ConversionMethod
			, MAX(CCR.DATE) AS DATE
			, StanPair.eSignalSymbol AS eSignalSymbol
			, CCR.FundID AS FundID
		FROM T_CurrencyConversionRate AS CCR
		INNER JOIN T_CurrencyStandardPairs StanPair ON StanPair.CurrencyPairId = CCR.CurrencyPairID_FK
		WHERE DateDiff(d, CCR.DATE, @UTCDate) >= 0
		GROUP BY StanPair.CurrencyPairId
			, StanPair.FromCurrencyID
			, StanPair.ToCurrencyID
			, StanPair.eSignalSymbol
			, CCR.FundID
		
		UNION
		
		SELECT StanPair.CurrencyPairId
			, StanPair.ToCurrencyID AS FromCurrencyID
			, StanPair.FromCurrencyID AS ToCurrencyID
			, 1 AS ConversionMethod
			, MAX(CCR.DATE) AS DATE
			, StanPair.eSignalSymbol AS eSignalSymbol
			, CCR.FundID AS FundID
		FROM T_CurrencyConversionRate AS CCR
		INNER JOIN T_CurrencyStandardPairs StanPair ON StanPair.CurrencyPairId = CCR.CurrencyPairID_FK
		WHERE DateDiff(d, CCR.DATE, @UTCDate) >= 0
		GROUP BY StanPair.CurrencyPairId
			, StanPair.FromCurrencyID
			, StanPair.ToCurrencyID
			, StanPair.eSignalSymbol
			, CCR.FundID
		)
	INSERT INTO @DayFXRates
	SELECT DISTINCT TEMP.FromCurrencyID
		, TEMP.ToCurrencyID
		, Isnull(CCR.ConversionRate, 0) AS RateValue
		, TEMP.ConversionMethod
		, TEMP.DATE AS DATE
		, TEMP.eSignalSymbol
		, TEMP.FundID
	FROM ConversionRate TEMP
	INNER JOIN T_CurrencyConversionRate CCR ON CCR.CurrencyPairID_FK = TEMP.CurrencyPairId
		AND DateDiff(d, TEMP.DATE, CCR.DATE) = 0
	ORDER BY TEMP.FromCurrencyID
		, TEMP.ToCurrencyID
	RETURN
END
