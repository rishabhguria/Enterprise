CREATE PROCEDURE [dbo].[P_FFGetThirdPartyFundsDetails_AgileMonthEOD] (
	@thirdPartyID INT
	,@companyFundIDs VARCHAR(max)
	,@inputDate DATETIME
	,@companyID INT
	,@auecIDs VARCHAR(max)
	,@TypeID INT
	,@dateType INT
	,@fileFormatID INT
	,@includeSent BIT = 1
	)
AS
DECLARE @Fund TABLE (FundID INT)
DECLARE @AUECID TABLE (AUECID INT)
DECLARE @IncludeExpiredSettledTransaction INT
DECLARE @IncludeExpiredSettledUnderlyingTransaction INT
DECLARE @IncludeCATransaction INT
DECLARE @StartDate DATETIME

SET @StartDate = DATEADD(m, DATEDIFF(m, 0, @inputDate), 0)
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

BEGIN
	SELECT VT.Level1AllocationID AS EntityID
		,ISNULL(VT.FundID, 0) AS FundID
		,VT.OrderSideTagValue AS SideID
		,T_Side.Side AS Side
		,VT.Symbol
		,VT.CounterPartyID
		,VT.VenueID
		,Sum(VT.TaxLotQty) AS AllocatedQty
		,VT.AvgPrice
		,VT.CumQty
		,VT.Quantity
		,VT.AUECID
		,VT.AssetID
		,VT.ExchangeID
		,Currency.CurrencyID
		,Currency.CurrencySymbol
		,CTPM.MappedName
		,CTPM.FundAccntNo
		,CTPM.FundTypeID_FK
		,FT.FundTypeName
		,SM.PutOrCall
		,SM.StrikePrice
		,CONVERT(VARCHAR(10), SM.ExpirationDate, 101) AS ExpirationDate
		,CONVERT(VARCHAR(10), VT.SettlementDate, 101) AS SettlementDate
		,sum(VT.Commission) AS CommissionCharged
		,sum(VT.OtherBrokerFees) AS OtherBrokerFees
		,VT.GroupRefID
		,0 AS TaxLotState
		,sum(ISNULL(VT.StampDuty, 0)) AS StampDuty
		,Sum(ISNULL(VT.TransactionLevy, 0)) AS TransactionLevy
		,Sum(ISNULL(ClearingFee, 0)) AS ClearingFee
		,Sum(ISNULL(TaxOnCommissions, 0)) AS TaxOnCommissions
		,Sum(ISNULL(MiscFees, 0)) AS MiscFees
		,CONVERT(VARCHAR(10), VT.AUECLocalDate, 101) AS TradeDate
		,SM.Multiplier AS AssetMultiplier
		,0 AS Level2ID
		,SM.ISINSymbol AS ISIN
		,SM.CUSIPSymbol AS CUSIP
		,SM.SEDOLSymbol AS SEDOL
		,SM.ReutersSymbol
		,SM.BloombergSymbol AS BBCode
		,SM.CompanyName
		,SM.UnderlyingSymbol
		,SM.LeadCurrencyID
		,SM.LeadCurrency
		,SM.VsCurrencyID
		,SM.VsCurrency
		,SM.OSISymbol AS OSIOptionSymbol
		,SM.IDCOSymbol
		,SM.OpraSymbol
		,VT.FXRate
		,VT.FXConversionMethodOperator
		,'No' AS FromDeleted
		,CONVERT(VARCHAR(10), VT.ProcessDate, 101) AS ProcessDate
		,CONVERT(VARCHAR(10), VT.OriginalPurchaseDate, 101) AS OriginalPurchaseDate
		,VT.AccruedInterest
		,'' AS Comment1
		,'' AS Comment2
		,SM.Coupon
		,SM.IssueDate
		,SM.FirstCouponDate
		,SM.CouponFrequencyID
		,Sm.AccrualBasisID
		,SM.BondTypeID
		,ISNull(VT.BenchMarkRate, '') AS BenchMarkRate
		,IsNull(VT.Differential, '') AS [Differential]
		,VT.SwapDescription
		,Isnull(VT.DayCount, '') AS DayCount
		,IsNull(VT.FirstResetDate, '') AS FirstResetDate
		,VT.IsSwapped
		,T_Country.CountryName AS CountryName
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
		,VT.FXConversionMethodOperator_Taxlot
		,SM.AssetName AS UDAAssetName
		,SM.SecurityTypeName AS UDASecurityTypeName
		,SM.SectorName AS UDASectorName
		,SM.SubSectorName AS UDASubSectorName
		,SM.CountryName AS UDACountryName
		,VT.Description
		,dbo.AdjustBusinessDays(SM.ExpirationDate, 2, VT.AUECID) AS DeliveryDate
		,Sum(ISNULL(VT.SecFee, 0)) AS SecFee
		,Sum(ISNULL(VT.OccFee, 0)) AS OccFee
		,Sum(ISNULL(VT.OrfFee, 0)) AS OrfFee
		,Sum(VT.ClearingBrokerFee) AS ClearingBrokerFee
		,Sum(VT.SoftCommission) AS SoftCommissionCharged
		,VT.TransactionType
		,COALESCE(TC.CurrencySymbol, 'None') AS SettlCurrency
		,T_Asset.AssetName AS Asset
		,CP.ShortName AS CounterParty
		,CF.FundName AS AccountName
	FROM V_TaxLots VT
	INNER JOIN T_Asset ON T_Asset.AssetID = VT.AssetID
	INNER JOIN T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue
	INNER JOIN T_CompanyFunds CF ON CF.CompanyFundID = VT.FundID
	LEFT JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID
	LEFT JOIN T_FundType AS FT ON FT.FundTypeID = CTPM.FundTypeID_FK
	LEFT JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID
	LEFT JOIN T_Country ON dbo.T_Country.CountryID = T_Exchange.Country
	LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
	LEFT JOIN T_Currency AS TC ON TC.CurrencyID = VT.SettlCurrency_Taxlot
	LEFT OUTER JOIN T_CounterParty CP ON CP.CounterPartyID = VT.CounterPartyID
	LEFT JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue
	LEFT OUTER JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol
	WHERE DATEDIFF(Day, @StartDate, VT.ProcessDate) >= 0
		AND DATEDIFF(DAY, VT.ProcessDate, @Inputdate) >= 0
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
						,14
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
			OR TransactionSource = 13
			)
	GROUP BY VT.GroupID
		,VT.FundID
		,VT.Level1AllocationID
		,VT.OrderSideTagValue
		,T_Side.Side
		,VT.Symbol
		,VT.CounterPartyID
		,VT.VenueID
		,VT.AvgPrice
		,VT.CumQty
		,VT.Quantity
		,VT.AUECID
		,VT.AssetID
		,VT.UnderlyingID
		,VT.ExchangeID
		,Currency.CurrencyID
		,Currency.CurrencyName
		,Currency.CurrencySymbol
		,CTPM.MappedName
		,CTPM.FundAccntNo
		,CTPM.FundTypeID_FK
		,FT.FundTypeName
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
		,VT.AccruedInterest
		,SM.Coupon
		,SM.IssueDate
		,SM.FirstCouponDate
		,SM.CouponFrequencyID
		,Sm.AccrualBasisID
		,SM.BondTypeID
		,VT.BenchMarkRate
		,VT.Differential
		,VT.SwapDescription
		,VT.DayCount
		,VT.FirstResetDate
		,VT.IsSwapped
		,T_Country.CountryName
		,VT.FXRate_Taxlot
		,VT.FXConversionMethodOperator_Taxlot
		,SM.AssetName
		,SM.SecurityTypeName
		,SM.SectorName
		,SM.SubSectorName
		,SM.CountryName
		,VT.Description
		,VT.TransactionType
		,VT.SettlCurrency_Taxlot
		,TC.CurrencySymbol
		,T_Asset.AssetName
		,CP.ShortName
		,CF.FundName
	ORDER BY GroupRefID
END