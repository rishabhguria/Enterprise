--Declare @thirdPartyID INT                
--Declare @companyFundIDs VARCHAR(max)                
--Declare @inputDate DATETIME                
--Declare @companyID INT                
--Declare @auecIDs VARCHAR(max)                
--Declare @TypeID INT                
--Declare @dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                      
--Declare @fileFormatID INT                
--                
--SET @thirdPartyID=32        
--SET @companyFundIDs=N'1259,1258,1261,1256,1255,1257,1263,1266,1260,1264,1265,1262,1253,1252,1254'        
--SET @inputDate='08-01-2016'        
--SET @companyID=7        
--SET @auecIDs=N'65,63,44,34,43,78,59,54,21,18,61,1,15,11,62,73,104,12,32,33'        
--SET @TypeID=0        
--SET @dateType=0        
--SET @fileFormatID=96        
CREATE PROCEDURE [dbo].[P_FFGetThirdPartyFundsDetails_Forex_CL] (
	@thirdPartyID INT
	,@companyFundIDs VARCHAR(max)
	,@inputDate DATETIME
	,@companyID INT
	,@auecIDs VARCHAR(max)
	,@TypeID INT
	,-- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                     
	@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                    
	,@fileFormatID INT
	)
AS
DECLARE @BaseCurrencyID INT

SET @BaseCurrencyID = (
		SELECT TOP 1 BaseCUrrencyID
		FROM T_Company
		WHERE CompanyID > 0
		)

DECLARE @StartDate DATETIME

SET @StartDate = (dbo.AdjustBusinessDays(DateAdd(d, 1, @inputDate), - 5, 1))

CREATE TABLE #TempForexRates (
	FromCurrencyID INT
	,ToCurrencyID INT
	,RateValue FLOAT
	,ConversionMethod VARCHAR(50)
	,DATE DATETIME
	,eSignalSymbol VARCHAR(max)
	,FundID INT
	)

INSERT INTO #TempForexRates (
	FromCurrencyID
	,ToCurrencyID
	,RateValue
	,ConversionMethod
	,DATE
	,eSignalSymbol
	,FundID
	)
EXEC P_GetAllFXConversionRatesForGivenDateRange @StartDate
	,@inputDate

UPDATE #TempForexRates
SET RateValue = 1.0 / RateValue
WHERE RateValue <> 0
	AND ConversionMethod = 1

DECLARE @Fund TABLE (FundID INT)
DECLARE @AUECID TABLE (AUECID INT)
DECLARE @IncludeExpiredSettledTransaction INT
DECLARE @IncludeExpiredSettledUnderlyingTransaction INT
DECLARE @IncludeCATransaction INT

SET @IncludeExpiredSettledTransaction = (
		SELECT IncludeExercisedAssignedTransaction
		FROM T_ThirdPartyFileFormat
		WHERE FileFormatId = @fileFormatID
		)
SET @IncludeExpiredSettledUnderlyingTransaction = (
		SELECT IncludeExercisedAssignedUnderlyingTransaction
		FROM T_ThirdPartyFileFormat
		WHERE FileFormatId = @fileFormatID
		)
SET @IncludeCATransaction = (
		SELECT IncludeCATransaction
		FROM T_ThirdPartyFileFormat
		WHERE FileFormatId = @fileFormatID
		)

DECLARE @FXForwardAuecID INT

SET @FXForwardAuecID = (
		SELECT TOP 1 Auecid
		FROM T_AUEC
		WHERE assetid = 11
		)

INSERT INTO @Fund
SELECT Cast(Items AS INT)
FROM dbo.Split(@companyFundIDs, ',')

INSERT INTO @AUECID
SELECT Cast(Items AS INT)
FROM dbo.Split(@auecIDs, ',')

SELECT VT.Level1AllocationID
	,ISNULL(VT.FundID, 0) AS FundID
	,CF.FundName AS AccountName
	,TA.AssetName AS Asset
	,T_Side.Side AS Side
	,VT.Symbol
	,Sum(VT.TaxLotQty) AS OrderQty
	,VT.AvgPrice
	,VT.CumQty
	,VT.Quantity
	,Currency.CurrencyID
	,Currency.CurrencyName
	,Currency.CurrencySymbol
	,VT.Level1AllocationID
	,sum(VT.TaxLotQty) AS AllocatedQty
	,SM.PutOrCall
	,SM.StrikePrice
	,Convert(VARCHAR(25), SM.ExpirationDate, 101) AS ExpirationDate
	,VT.SettlementDate
	,sum(VT.Commission) AS CommissionCharged
	,sum(VT.OtherBrokerFees) AS OtherBrokerFee
	,VT.GroupRefID
	,0 AS TaxLotState
	,sum(ISNULL(VT.StampDuty, 0)) AS StampDuty
	,Sum(ISNULL(VT.TransactionLevy, 0)) AS TransactionLevy
	,Sum(ISNULL(ClearingFee, 0)) AS ClearingFee
	,Sum(ISNULL(TaxOnCommissions, 0)) AS TaxOnCommissions
	,Sum(ISNULL(MiscFees, 0)) AS MiscFees
	,Convert(VARCHAR(25), VT.AUECLocalDate, 101) AS TradeDate
	,SM.Multiplier
	,SM.ISINSymbol AS ISIN
	,SM.CUSIPSymbol AS CUSIP
	,SM.SEDOLSymbol AS SEDOL
	,SM.ReutersSymbol
	,SM.BloombergSymbol
	,SM.CompanyName AS FullSecurityName
	,SM.UnderlyingSymbol
	,SM.LeadCurrencyID
	,SM.LeadCurrency
	,SM.VsCurrencyID
	,SM.VsCurrency
	,SM.OSISymbol AS OSIOptionSymbol
	,SM.IDCOSymbol AS IDCOOptionSymbol
	,SM.OpraSymbol
	,VT.FXRate
	,VT.FXConversionMethodOperator
	,'No' AS FromDeleted
	,VT.ProcessDate
	,VT.OriginalPurchaseDate
	,CASE 
		WHEN VT.AssetID = 11
			AND SM.ExpirationDate <> '1800-01-01 00:00:00.000'
			THEN dbo.AdjustBusinessDays(SM.ExpirationDate, - 1, @FXForwardAuecID)
		ELSE ''
		END AS RerateDateBusDayAdjusted1
	,CASE 
		WHEN VT.AssetID = 11
			AND SM.ExpirationDate <> '1800-01-01 00:00:00.000'
			THEN dbo.AdjustBusinessDays(SM.ExpirationDate, - 2, @FXForwardAuecID)
		ELSE ''
		END AS RerateDateBusDayAdjusted2
	,VT.FXRate_Taxlot
	,COALESCE(TC.CurrencySymbol, 'None') AS SettlCurrency
	,VT.SettlCurrFxRateCalc_Taxlot AS SettlCurrFxRateCalc
	,VT.SettlCurrFxRate_Taxlot AS SettlCurrFxRate
	,VT.SettlCurrAmt_Taxlot AS SettlCurrAmt
	,sum(ISNULL(VT.SecFee, 0)) AS SecFee
	,sum(ISNULL(VT.OccFee, 0)) AS OccFee
	,sum(ISNULL(VT.OrfFee, 0)) AS OrfFee
	,sum(VT.ClearingBrokerFee) AS ClearingBrokerFee
	,sum(VT.SoftCommission) AS SoftCommissionCharged
	,VT.TransactionType
	,ISNULL(VT.BenchMarkRate, 0) AS BenchMarkRate
	,ISNULL(VT.Differential, 0) AS Differential
	,ISNULL(VT.SwapDescription, '') AS SwapDescription
	,ISNULL(VT.DayCount, 0) AS DayCount
	,ISNULL(VT.FirstResetDate, '') AS FirstResetDate
	,VT.IsSwapped
	--,TF.RateValue as ForexRate        
	,IsNull(FXDayRatesForTradeDate.RateValue, 0) AS ForexRate
FROM V_TaxLots VT
INNER JOIN T_CompanyFunds AS CF ON CF.CompanyFundId = VT.FundID
INNER JOIN t_Asset AS TA ON TA.AssetId = VT.AssetId
LEFT JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID
LEFT JOIN T_Country ON dbo.T_Country.CountryID = T_Exchange.Country
LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
LEFT JOIN T_Currency AS TC ON TC.CurrencyID = VT.SettlCurrency_Taxlot
LEFT JOIN T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue
LEFT JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue
LEFT OUTER JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol
LEFT OUTER JOIN #TempForexRates FXDayRatesForTradeDate ON (
		FXDayRatesForTradeDate.FromCurrencyID = @BaseCurrencyID
		AND FXDayRatesForTradeDate.ToCurrencyID = VT.CurrencyID
		AND DateDiff(D, FXDayRatesForTradeDate.DATE, VT.AuecLocalDate) = 0
		)
WHERE Datediff(d, (
			CASE 
				WHEN @dateType = 1
					THEN VT.AUECLocalDate
				ELSE VT.ProcessDate
				END
			), @Startdate) <= 0
	AND Datediff(d, (
			CASE 
				WHEN @dateType = 1
					THEN VT.AUECLocalDate
				ELSE VT.ProcessDate
				END
			), @inputdate) >= 0
	AND VT.FundID IN (
		SELECT FundID
		FROM @Fund
		)
	AND VT.AUECID IN (
		SELECT AUECID
		FROM @AUECID
		)
	AND (
		(
			VT.TransactionType IN (
				'Buy'
				,'BuytoClose'
				,'BuytoOpen'
				,'Sell'
				,'Sellshort'
				,'SelltoClose'
				,'SelltoOpen'
				,'LongAddition'
				,'LongWithdrawal'
				,'ShortAddition'
				,'ShortWithdrawal'
				,''
				)
			AND (
				VT.TransactionSource IN (
					0
					,1
					,2
					,3
					,4
					)
				)
			)
		OR (
			@IncludeExpiredSettledTransaction = 1
			AND VT.TransactionType IN (
				'Exercise'
				,'Expire'
				,'Assignment'
				)
			AND VT.AssetID IN (
				2
				,4
				)
			)
		OR (
			@IncludeExpiredSettledTransaction = 1
			AND VT.TransactionType IN (
				'CSCost'
				,'CSZero'
				,'DLCost'
				,'CSClosingPx'
				,'Expire'
				,'DLCostAndPNL'
				)
			AND VT.AssetID IN (3)
			)
		OR (
			@IncludeExpiredSettledUnderlyingTransaction = 1
			AND VT.TransactionType IN (
				'Exercise'
				,'Expire'
				,'Assignment'
				)
			AND TaxlotClosingID_FK IS NOT NULL
			AND VT.AssetID IN (
				1
				,3
				)
			)
		OR (
			@IncludeCATransaction = 1
			AND VT.TransactionType IN (
				'LongAddition'
				,'LongWithdrawal'
				,'ShortAddition'
				,'ShortWithdrawal'
				,'LongCostAdj'
				,'ShortCostAdj'
				,'LongWithdrawalCashInLieu'
				,'ShortWithdrawalCashInLieu'
				)
			AND (
				VT.TransactionSource IN (
					6
					,7
					,8
					,9
					,11
					)
				)
			)
		)
GROUP BY VT.GroupID
	,VT.AssetID
	,TA.AssetName
	,VT.FundID
	,CF.FundName
	,VT.Level1AllocationID
	,T_Side.Side
	,VT.Symbol
	,VT.AvgPrice
	,VT.CumQty
	,VT.Quantity
	,Currency.CurrencyID
	,Currency.CurrencyName
	,Currency.CurrencySymbol
	,SM.PutOrCall
	,SM.StrikePrice
	,SM.ExpirationDate
	,VT.SettlementDate
	,VT.GroupRefID
	,VT.AUECLocalDate
	,SM.Multiplier
	,SM.ISINSymbol
	,SM.CUSIPSymbol
	,SM.SEDOLSymbol
	,SM.ReutersSymbol
	,SM.BloombergSymbol
	,SM.CompanyName
	,SM.UnderlyingSymbol
	,SM.LeadCurrencyID
	,SM.LeadCurrency
	,SM.VsCurrencyID
	,SM.VsCurrency
	,SM.OSISymbol
	,SM.IDCOSymbol
	,SM.OpraSymbol
	,VT.FXRate
	,VT.FXConversionMethodOperator
	,VT.ProcessDate
	,VT.OriginalPurchaseDate
	,VT.FXRate_Taxlot
	,VT.FXConversionMethodOperator_Taxlot
	,VT.SettlCurrency_Taxlot
	,VT.SettlCurrFxRateCalc_Taxlot
	,VT.SettlCurrFxRate_Taxlot
	,VT.SettlCurrAmt_Taxlot
	,TC.CurrencySymbol
	,VT.BenchMarkRate
	,VT.Differential
	,VT.SwapDescription
	,VT.DayCount
	,VT.FirstResetDate
	,VT.IsSwapped
	,VT.TransactionType
	,FXDayRatesForTradeDate.RateValue
ORDER BY GroupRefID

DROP TABLE #TempForexRates