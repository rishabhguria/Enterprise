-- =============================================          
-- Author:  Sumit Kakra          
-- Create date: 19-MARCH-2008          
-- Description: Returns the Forex rate with standard pairs and reverse of standard pairs          
-- Usage:           
-- Declare @Date datetime          
-- Set @Date = DateAdd(d,-3,GetUTCDate())          
-- Select * from GetFXConversionRatesForDate('2008-05-21') --@Date)        
-- =============================================           
CREATE FUNCTION [dbo].[GetFXConversionRatesForDate] (
	@Date DATETIME
	,@isHistoricalModeAllowed BIT -- if 1 is passed then the function will return historical data otherwise on for the given date.  
	)
RETURNS @FXConversionRatesForDate TABLE (
	FromCurrencyID INT
	,ToCurrencyID INT
	,RateValue FLOAT
	,ConversionMethod INT
	,Date DATETIME
	,eSignalSymbol VARCHAR(max)
	,InputDatePriceIndicator INT
	,TickerSymbol VARCHAR(50)
	,FundID INT              
	)
AS
BEGIN
	DECLARE @tblMaxDateForCurrencyPair TABLE (
		CurrencyPairId INT
		,MaxDate DATETIME
		,FundID INT
		)

	IF (@isHistoricalModeAllowed = 1)
	BEGIN
		---------returns historical data.------------  
		INSERT INTO @tblMaxDateForCurrencyPair
		SELECT CurrencyPairID_FK
			,MAX(Date)
			,FundID
		FROM T_CurrencyConversionRate InnerCCR
		WHERE Date <= @Date
			AND (
				InnerCCR.ConversionRate <> NULL
				OR InnerCCR.ConversionRate <> 0
				) --@Date  -- Check if we need to apply GetFormattedDatePart function here.        
		GROUP BY CurrencyPairID_FK, FundID
			-- ConversionMethod = 0 represents mulitply and ConversionMethod = 1 represents divide.        
	END
	ELSE
	BEGIN
		---------returns data on the data passed.------------  
		INSERT INTO @tblMaxDateForCurrencyPair
		SELECT CurrencyPairID_FK
			,MAX(Date)
			,FundID
		FROM T_CurrencyConversionRate InnerCCR
		WHERE datediff(d, Date, @Date) = 0 --and (InnerCCR.ConversionRate <> null or InnerCCR.ConversionRate <> 0)  --@Date  -- Check if we need to apply GetFormattedDatePart function here.        
		GROUP BY CurrencyPairID_FK, FundID
			-- ConversionMethod = 0 represents mulitply and ConversionMethod = 1 represents divide.        
	END

	INSERT INTO @FXConversionRatesForDate
	SELECT StanPair.FromCurrencyID AS FromCurrencyID
		,StanPair.ToCurrencyID AS ToCurrencyID
		,CCR.ConversionRate AS RateValue
		,0 AS ConversionMethod
		,CCR.Date AS Date
		,StanPair.eSignalSymbol AS eSignalSymbol
		,CASE 
			WHEN datediff(d, CCR.Date, @Date) = 0
				THEN 0
			ELSE 1
			END AS InputDatePriceIndicator
		,Currency1.CurrencySymbol + '-' + Currency2.CurrencySymbol AS TickerSymbol
		,CCR.FundID AS FundID
	FROM T_CurrencyConversionRate AS CCR
	INNER JOIN @tblMaxDateForCurrencyPair MaxCurrPair
		ON CCR.CurrencyPairID_FK = MaxCurrPair.CurrencyPairId
			AND CCR.Date = MaxCurrPair.MaxDate
			AND CCR.FundID = MaxCurrPair.FundID
	INNER JOIN T_CurrencyStandardPairs StanPair
		ON StanPair.CurrencyPairId = MaxCurrPair.CurrencyPairId
	JOIN T_Currency AS Currency1
		ON Currency1.CurrencyID = StanPair.FromCurrencyID
	JOIN T_Currency AS Currency2
		ON Currency2.CurrencyID = StanPair.ToCurrencyID
	
	UNION ALL
	
	SELECT StanPair.ToCurrencyID AS FromCurrencyID
		,StanPair.FromCurrencyID AS ToCurrencyID
		,CCR.ConversionRate AS RateValue
		,1 AS ConversionMethod
		,CCR.Date AS Date
		,StanPair.eSignalSymbol AS eSignalSymbol
		,CASE 
			WHEN datediff(d, CCR.Date, @Date) = 0
				THEN 0
			ELSE 1
			END AS InputDatePriceIndicator
		,Currency1.CurrencySymbol + '-' + Currency2.CurrencySymbol AS TickerSymbol
		,CCR.FundID AS FundID
	FROM T_CurrencyConversionRate AS CCR
	INNER JOIN @tblMaxDateForCurrencyPair MaxCurrPair
		ON CCR.CurrencyPairID_FK = MaxCurrPair.CurrencyPairId
			AND CCR.Date = MaxCurrPair.MaxDate
			AND CCR.FundID = MaxCurrPair.FundID
	INNER JOIN T_CurrencyStandardPairs StanPair
		ON StanPair.CurrencyPairId = MaxCurrPair.CurrencyPairId
	JOIN T_Currency AS Currency1
		ON Currency1.CurrencyID = StanPair.FromCurrencyID
	JOIN T_Currency AS Currency2
		ON Currency2.CurrencyID = StanPair.ToCurrencyID
	RETURN
END
