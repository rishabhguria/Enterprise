
-- =============================================                      
-- Author:  Bhupesh Bareja                      
-- Create date: 12-JUN-2008                      
-- Description: Returns the Forex rate with standard pairs and reverse of standard pairs for the date range given        
-- Usage:                       
-- Select * from GetAllFXConversionRatesForGivenDateRange('06-12-2008', '06-20-2008')                    
-- =============================================                       
CREATE FUNCTION [dbo].[GetAllFXConversionRatesForGivenDateRange] (
	@startingDate DATETIME
	,@endingDate DATETIME
	)
RETURNS @FXConversionRatesForDate TABLE (
	FromCurrencyID INT
	,ToCurrencyID INT
	,RateValue FLOAT
	,ConversionMethod INT
	,DATE DATETIME
	,eSignalSymbol VARCHAR(max)
	,fundID INT
	)
AS
BEGIN
	-- ConversionMethod = 0 represents mulitply and ConversionMethod = 1 represents divide.                    
	INSERT INTO @FXConversionRatesForDate
	SELECT StanPair.FromCurrencyID AS FromCurrencyID
		,StanPair.ToCurrencyID AS ToCurrencyID
		,CCR.ConversionRate AS RateValue
		,0 AS ConversionMethod
		,CCR.DATE AS DATE
		,StanPair.eSignalSymbol AS eSignalSymbol
		,fundID
	FROM T_CurrencyConversionRate AS CCR
	INNER JOIN T_CurrencyStandardPairs StanPair ON StanPair.CurrencyPairId = CCR.CurrencyPairID_FK
	WHERE DateDiff(d, @startingDate, CCR.DATE) >= 0 -- dbo.GetFormattedDatePart(CCR.Date) >= dbo.GetFormattedDatePart(@startingDate)          
		AND DateDiff(d, CCR.DATE, @endingDate) >= 0 -- dbo.GetFormattedDatePart(CCR.Date) <= dbo.GetFormattedDatePart(@endingDate)        
	
	UNION
	
	SELECT StanPair.ToCurrencyID AS FromCurrencyID
		,StanPair.FromCurrencyID AS ToCurrencyID
		,CCR.ConversionRate AS RateValue
		,1 AS ConversionMethod
		,CCR.DATE AS DATE
		,StanPair.eSignalSymbol AS eSignalSymbol
		,fundID
	FROM T_CurrencyConversionRate AS CCR
	INNER JOIN T_CurrencyStandardPairs StanPair ON StanPair.CurrencyPairId = CCR.CurrencyPairID_FK
	WHERE DateDiff(d, @startingDate, CCR.DATE) >= 0 --dbo.GetFormattedDatePart(CCR.Date) >= dbo.GetFormattedDatePart(@startingDate)          
		AND DateDiff(d, CCR.DATE, @endingDate) >= 0 -- dbo.GetFormattedDatePart(CCR.Date) <= dbo.GetFormattedDatePart(@endingDate)               

	RETURN
END
