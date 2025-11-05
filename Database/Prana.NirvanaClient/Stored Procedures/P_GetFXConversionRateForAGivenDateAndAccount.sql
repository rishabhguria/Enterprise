/* =============================================
Author: <Sandeep Singh>
Create date: <24 FEB 2020>
Description:	<Get FX Rate For a Date and Account. If no FX Rate for a account, then it will pick for 0 accountID>
EXEC P_GetFXConversionRateForAGivenDateAndAccount 8,1,1,'01-15-2020'
-- ============================================= */
CREATE PROCEDURE P_GetFXConversionRateForAGivenDateAndAccount
(
@FromCurrencyID Int,
@ToCurrencyID Int,
@AccountID Int,
@Date DateTime
)
AS
BEGIN
	
	SET NOCOUNT ON;

--Declare @FromCurrencyID Int
--Declare @ToCurrencyID Int
--Declare @AccountID Int
--Declare @Date DateTime

--Set @FromCurrencyID = 8
--Set @ToCurrencyID = 1
--Set @AccountID = 10
--Set @Date = '01-10-2020'

Create Table #TempFXConversionRatesForDate
(
	FromCurrencyID INT
	,ToCurrencyID INT
	,RateValue FLOAT
	,ConversionMethod INT
	,DATE DATETIME
	,eSignalSymbol VARCHAR(500)
	,FundID INT
)

INSERT INTO #TempFXConversionRatesForDate
	SELECT 
		StanPair.FromCurrencyID AS FromCurrencyID,
		StanPair.ToCurrencyID AS ToCurrencyID,
		CCR.ConversionRate AS RateValue,
		0 AS ConversionMethod,
		CCR.DATE AS DATE,
		StanPair.eSignalSymbol AS eSignalSymbol,
		FundID
		FROM T_CurrencyConversionRate AS CCR
		INNER JOIN T_CurrencyStandardPairs StanPair ON StanPair.CurrencyPairId = CCR.CurrencyPairID_FK
		WHERE DateDiff(d, @Date, CCR.DATE) = 0
		And StanPair.FromCurrencyID = @FromCurrencyID
		And StanPair.ToCurrencyID = @ToCurrencyID
		And FundID = @AccountID
UNION
	
	SELECT 
		StanPair.ToCurrencyID AS FromCurrencyID,
		StanPair.FromCurrencyID AS ToCurrencyID,
		CCR.ConversionRate AS RateValue,
		1 AS ConversionMethod,
		CCR.DATE AS DATE,
		StanPair.eSignalSymbol AS eSignalSymbol,
		FundID
		FROM T_CurrencyConversionRate AS CCR
		INNER JOIN T_CurrencyStandardPairs StanPair ON StanPair.CurrencyPairId = CCR.CurrencyPairID_FK
		WHERE DateDiff(d, @Date, CCR.DATE) = 0
		And StanPair.FromCurrencyID = @ToCurrencyID
		And StanPair.ToCurrencyID = @FromCurrencyID
		And FundID = @AccountID      

Declare @AccountWiseFXRateCount Int
Set @AccountWiseFXRateCount = 0
Set @AccountWiseFXRateCount = (Select Count(*) From #TempFXConversionRatesForDate)

If (@AccountWiseFXRateCount Is Null Or @AccountWiseFXRateCount = 0)
Begin
	INSERT INTO #TempFXConversionRatesForDate
	SELECT 
		StanPair.FromCurrencyID AS FromCurrencyID,
		StanPair.ToCurrencyID AS ToCurrencyID,
		CCR.ConversionRate AS RateValue,
		0 AS ConversionMethod,
		CCR.DATE AS DATE,
		StanPair.eSignalSymbol AS eSignalSymbol,
		0
		FROM T_CurrencyConversionRate AS CCR
		INNER JOIN T_CurrencyStandardPairs StanPair ON StanPair.CurrencyPairId = CCR.CurrencyPairID_FK
		WHERE DateDiff(d, @Date, CCR.DATE) = 0
		And StanPair.FromCurrencyID = @FromCurrencyID
		And StanPair.ToCurrencyID = @ToCurrencyID
UNION
	
	SELECT 
		StanPair.ToCurrencyID AS FromCurrencyID,
		StanPair.FromCurrencyID AS ToCurrencyID,
		CCR.ConversionRate AS RateValue,
		1 AS ConversionMethod,
		CCR.DATE AS DATE,
		StanPair.eSignalSymbol AS eSignalSymbol,
		0
		FROM T_CurrencyConversionRate AS CCR
		INNER JOIN T_CurrencyStandardPairs StanPair ON StanPair.CurrencyPairId = CCR.CurrencyPairID_FK
		WHERE DateDiff(d, @Date, CCR.DATE) = 0
		And StanPair.FromCurrencyID = @ToCurrencyID
		And StanPair.ToCurrencyID = @FromCurrencyID
End

Update #TempFXConversionRatesForDate
Set RateValue = 1.0/RateValue 
Where RateValue <> 0 And ConversionMethod = 1


Select * From #TempFXConversionRatesForDate

Drop Table #TempFXConversionRatesForDate

END

