
-- =============================================              
-- Author:  Vinod Nayal            
-- Create date: 19-MARCH-2008              
-- Description: Returns the Forex rate with standard pairs and reverse of standard pairs              
-- Usage:               
-- Declare @Date datetime              
-- Set @Date = DateAdd(d,-3,GetUTCDate())              
-- Select * from GetAllFXConversionRates_New()            
-- TODO : For now we have picked up all the conversion rates. We need to pickup only those conversion rates which are for open positions/unallocated trades.        
-- =============================================               
CREATE FUNCTION [dbo].[GetAllFXConversionRates_New] ()
RETURNS @FXConversionRatesForDate TABLE (
	--FromCurrencyID int,            
	--ToCurrencyID int,           
	Currency1 VARCHAR(50)
	,Currency2 VARCHAR(50)
	,RateValue FLOAT
	,DATE DATETIME
	,eSignalSymbol VARCHAR(max)
	,FundID INT
	)
AS
BEGIN
	INSERT INTO @FXConversionRatesForDate
	SELECT Currency1.CurrencySymbol AS Currency1
		,Currency2.CurrencySymbol AS Currency2
		,CCR.ConversionRate AS RateValue
		,CCR.DATE AS DATE
		,StanPair.eSignalSymbol AS eSignalSymbol
		,FundID
	FROM T_CurrencyConversionRate AS CCR
	INNER JOIN T_CurrencyStandardPairs StanPair ON StanPair.CurrencyPairId = CCR.CurrencyPairID_FK
	JOIN T_Currency AS Currency1 ON Currency1.CurrencyID = StanPair.FromCurrencyID
	JOIN T_Currency AS Currency2 ON Currency2.CurrencyID = StanPair.ToCurrencyID

	RETURN
END
