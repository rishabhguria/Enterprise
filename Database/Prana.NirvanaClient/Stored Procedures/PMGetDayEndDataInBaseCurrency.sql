/*
Modifed By: Sandeep Singh
Modified Date: 9 May, 2018
Desc: Fund wise FX Rate implementation
JIRA: https://jira.nirvanasolutions.com:8443/browse/PRANA-28113

EXEC PMGetDayEndDataInBaseCurrency '2018-05-02 00:00:00:000',0
*/
CREATE PROCEDURE [dbo].[PMGetDayEndDataInBaseCurrency] (
	@DateForCashValues DATETIME
	,@IsAccrualsNeeded BIT
	,@IsIncludeTradingDayAccruals BIT = 1
	)
AS
--Declare @DateForCashValues DATETIME
--Declare @IsAccrualsNeeded BIT
--Set @DateForCashValues = '2018-05-02 00:00:00:000' 
--Set @IsAccrualsNeeded = 0
BEGIN
	DECLARE @Local_DateForCashValues DATETIME
	DECLARE @Local_IsAccrualsNeeded BIT
	DECLARE @Local_BackDateForCashValues DATETIME

	SET @Local_DateForCashValues = @DateForCashValues
	SET @Local_IsAccrualsNeeded = @IsAccrualsNeeded
	SET @Local_BackDateForCashValues = DATEADD(dd, - 1, @Local_DateForCashValues)

	DECLARE @BaseCurrencyId INT

	SET @BaseCurrencyId = (
			SELECT TOP 1 BaseCurrencyID
			FROM T_Company
			)

	--Day End Cash
	SELECT FundID
		,SUM(CashValueBase) AS Cash
	FROM PM_CompanyFundCashCurrencyValue
	WHERE DATEDIFF(dd, DATE, @Local_BackDateForCashValues) = 0
	GROUP BY FundID

	--Today's Cash
	CREATE TABLE #FXConversionRates (
		FromCurrencyID INT
		,ToCurrencyID INT
		,RateValue FLOAT
		,ConversionMethod INT
		,DATE DATETIME
		,eSignalSymbol VARCHAR(max)
		,FundID INT
		)

	INSERT INTO #FXConversionRates
	EXEC P_GetAllFXConversionRatesForGivenDateRange @Local_BackDateForCashValues
		,@Local_BackDateForCashValues

	UPDATE #FXConversionRates
	SET RateValue = 1.0 / RateValue
	WHERE RateValue <> 0
		AND ConversionMethod = 1

	UPDATE #FXConversionRates
	SET RateValue = 0
	WHERE RateValue IS NULL

	-- Keep FX Rate for zero Fund ID
	SELECT *
	INTO #ZeroFundFxRate
	FROM #FXConversionRates
	WHERE FundID = 0

	SELECT T_Journal.FundID
		,CASE 
			WHEN T_Journal.CurrencyID <> @BaseCurrencyId
				THEN CASE 
						WHEN T_Journal.FxRate <> 0
							THEN T_Journal.DR * T_Journal.FxRate
						ELSE T_Journal.DR * ISNULL(FXRates.Val, 0)
						END
			ELSE T_Journal.DR
			END AS DR
		,CASE 
			WHEN T_Journal.CurrencyID <> @BaseCurrencyId
				THEN CASE 
						WHEN T_Journal.FxRate <> 0
							THEN T_Journal.CR * T_Journal.FxRate
						ELSE T_Journal.CR * ISNULL(FXRates.Val, 0)
						END
			ELSE T_Journal.CR
			END AS CR
	INTO #TempCashData
	FROM T_Journal
	INNER JOIN T_SubAccounts T_SubAccounts ON T_Journal.SubAccountID = T_SubAccounts.SubAccountID
	INNER JOIN T_TransactionType ON T_TransactionType.TransactionTypeId = T_SubAccounts.TransactionTypeId
	INNER JOIN T_CompanyFunds ON T_CompanyFunds.CompanyFundID = T_Journal.FundID
	INNER JOIN T_Company ON T_CompanyFunds.CompanyID = T_Company.CompanyID
	LEFT OUTER JOIN #FXConversionRates FX ON (
			FX.FromCurrencyID = T_Journal.CurrencyID
			AND FX.ToCurrencyID = T_Company.BaseCurrencyID
			AND FX.FundID = T_Journal.FundID
			)
	LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRate ON (
			ZeroFundFxRate.FromCurrencyID = T_Journal.CurrencyID
			AND ZeroFundFxRate.ToCurrencyID = T_Company.BaseCurrencyID
			AND ZeroFundFxRate.FundID = 0
			)
	CROSS APPLY (
		SELECT CASE 
				WHEN FX.RateValue IS NULL
					THEN CASE 
							WHEN ZeroFundFxRate.RateValue IS NULL
								THEN 0
							ELSE ZeroFundFxRate.RateValue
							END
				ELSE FX.RateValue
				END
		) AS FXRates(Val)
	WHERE T_TransactionType.TransactionType = 'Cash'
		AND DATEDIFF(dd, T_Journal.TransactionDate, @Local_DateForCashValues) = 0
		AND T_Journal.TransactionSource NOT IN (
			1
			,7
			,9
			,11
			)

	SELECT FundID
		,SUM(DR) AS DR
		,SUM(CR) AS CR
	FROM #TempCashData
	GROUP BY FundID

	DROP TABLE #FXConversionRates
		,#TempCashData
		,#ZeroFundFxRate

	IF @Local_IsAccrualsNeeded = 1
	BEGIN
		EXEC GetStartDayOfAccruals @Local_DateForCashValues ,@IsIncludeTradingDayAccruals
	END
END
