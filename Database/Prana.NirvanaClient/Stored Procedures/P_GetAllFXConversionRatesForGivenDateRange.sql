/********************************************************                           
 Author:  Sandeep Singh                            
 Create date: April 13,2010                            
 Description: Returns the Forex rate with standard pairs and reverse of standard pairs for the date range given              
 Usage:                             
 Exec [P_GetAllFXConversionRatesForGivenDateRange] '11-01-2009','03-20-2010' 

Modified By: Rahul Gupta
Description: Place Isnull check for conversion rate in this script only.
Modified On: 05-10-2012
                         
********************************************************/
CREATE PROCEDURE [P_GetAllFXConversionRatesForGivenDateRange] (
	@startingDate DATETIME
	,@endingDate DATETIME
	)
AS
BEGIN
	-- ConversionMethod = 0 represents mulitply and ConversionMethod = 1 represents divide.                          
	SELECT StanPair.FromCurrencyID AS FromCurrencyID
		,StanPair.ToCurrencyID AS ToCurrencyID
		,Isnull(CCR.ConversionRate, 0) AS RateValue
		,0 AS ConversionMethod
		,CCR.DATE AS DATE
		,StanPair.eSignalSymbol AS eSignalSymbol
		,CCR.FundID AS FundID
	FROM T_CurrencyConversionRate AS CCR
	INNER JOIN T_CurrencyStandardPairs StanPair
		ON StanPair.CurrencyPairId = CCR.CurrencyPairID_FK
	WHERE DateDiff(d, @startingDate, CCR.DATE) >= 0 -- dbo.GetFormattedDatePart(CCR.Date) >= dbo.GetFormattedDatePart(@startingDate)                
		AND DateDiff(d, CCR.DATE, @endingDate) >= 0 -- dbo.GetFormattedDatePart(CCR.Date) <= dbo.GetFormattedDatePart(@endingDate)                  
	
	UNION
	
	SELECT StanPair.ToCurrencyID AS FromCurrencyID
		,StanPair.FromCurrencyID AS ToCurrencyID
		,Isnull(CCR.ConversionRate, 0) AS RateValue
		,1 AS ConversionMethod
		,CCR.DATE AS DATE
		,StanPair.eSignalSymbol AS eSignalSymbol
		,CCR.FundID AS FundID
	FROM T_CurrencyConversionRate AS CCR
	INNER JOIN T_CurrencyStandardPairs StanPair
		ON StanPair.CurrencyPairId = CCR.CurrencyPairID_FK
	WHERE DateDiff(d, @startingDate, CCR.DATE) >= 0 --dbo.GetFormattedDatePart(CCR.Date) >= dbo.GetFormattedDatePart(@startingDate)                
		AND DateDiff(d, CCR.DATE, @endingDate) >= 0 -- dbo.GetFormattedDatePart(CCR.Date) <= dbo.GetFormattedDatePart(@endingDate)                     
END
