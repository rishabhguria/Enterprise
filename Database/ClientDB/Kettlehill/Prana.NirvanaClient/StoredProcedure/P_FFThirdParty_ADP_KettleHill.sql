CREATE PROCEDURE [dbo].[P_FFThirdParty_ADP_KettleHill] (
	@ThirdPartyID INT
	,@CompanyFundIDs VARCHAR(max)
	,@InputDate DATETIME
	,@CompanyID INT
	,@AuecIDs VARCHAR(max)
	,@TypeID INT -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties             
	,@DateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                  
	,@FileFormatID INT
	,@includeSent INT = 0
	)
AS

--Declare	@ThirdPartyID INT
--	,@CompanyFundIDs VARCHAR(max)
--	,@InputDate DATETIME
--	,@CompanyID INT
--	,@AuecIDs VARCHAR(max)
--	,@TypeID INT -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties             
--	,@DateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                  
--	,@FileFormatID INT
--	,@includeSent INT =0

--SET @thirdPartyID=70
--SET @companyFundIDs=N'28,29,30,31,27,'
--SET @inputDate='2019-06-19 16:22:33'
--SET @companyID=7
--SET @auecIDs=N'71,63,34,43,1,15,11,62,73,12,80,32,88,81,'
--SET @TypeID=0
--SET @dateType=1
--SET @fileFormatID=146

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

CREATE TABLE #SecMasterData (
	TickerSymbol VARCHAR(200)
	,PutOrCall VARCHAR(10)
	,StrikePrice INT
	,ExpirationDate DATETIME
	,Multiplier FLOAT
	,ISINSymbol VARCHAR(20)
	,CUSIPSymbol VARCHAR(50)
	,SEDOLSymbol VARCHAR(50)
	,ReutersSymbol VARCHAR(200)
	,BloombergSymbol VARCHAR(200)
	,CompanyName VARCHAR(500)
	,UnderlyingSymbol VARCHAR(100)
	,LeadCurrencyID INT
	,LeadCurrency VARCHAR(3)
	,VsCurrencyID INT
	,VsCurrency VARCHAR(3)
	,OSISymbol VARCHAR(21)
	,IDCOSymbol VARCHAR(22)
	,OpraSymbol VARCHAR(21)
	,Coupon INT
	,IssueDate DATETIME
	,FirstCouponDate DATETIME
	,CouponFrequencyID INT
	,AccrualBasisID INT
	,BondTypeID INT
	,AssetName VARCHAR(100)
	,SecurityTypeName VARCHAR(100)
	,SectorName VARCHAR(100)
	,SubSectorName VARCHAR(100)
	,CountryName VARCHAR(100)
	,Analyst VARCHAR(500)
	,CountryOfRisk VARCHAR(500)
	,CustomUDA1 VARCHAR(500)
	,CustomUDA2 VARCHAR(500)
	,CustomUDA3 VARCHAR(500)
	,CustomUDA4 VARCHAR(500)
	,CustomUDA5 VARCHAR(500)
	,CustomUDA6 VARCHAR(500)
	,CustomUDA7 VARCHAR(500)
	,Issuer VARCHAR(500)
	,LiquidTag VARCHAR(500)
	,MarketCap VARCHAR(500)
	,Region VARCHAR(500)
	,RiskCurrency VARCHAR(500)
	,UCITSEligibleTag VARCHAR(500)
	)

INSERT INTO #SecMasterData
SELECT TickerSymbol
	,PutOrCall
	,StrikePrice
	,ExpirationDate
	,Multiplier
	,ISINSymbol
	,CUSIPSymbol
	,SEDOLSymbol
	,ReutersSymbol
	,BloombergSymbol
	,CompanyName
	,UnderlyingSymbol
	,LeadCurrencyID
	,LeadCurrency
	,VsCurrencyID
	,VsCurrency
	,OSISymbol
	,IDCOSymbol
	,OpraSymbol
	,Coupon
	,IssueDate
	,FirstCouponDate
	,CouponFrequencyID
	,AccrualBasisID
	,BondTypeID
	,AssetName
	,SecurityTypeName
	,SectorName
	,SubSectorName
	,CountryName
	,ISNULL(Analyst, '')
	,ISNULL(CountryOfRisk, '')
	,ISNULL(CustomUDA1, '')
	,ISNULL(CustomUDA2, '')
	,ISNULL(CustomUDA3, '')
	,ISNULL(CustomUDA4, '')
	,ISNULL(CustomUDA5, '')
	,ISNULL(CustomUDA6, '')
	,ISNULL(CustomUDA7, '')
	,ISNULL(Issuer, '')
	,ISNULL(LiquidTag, '')
	,ISNULL(MarketCap, '')
	,ISNULL(Region, '')
	,ISNULL(RiskCurrency, '')
	,ISNULL(UCITSEligibleTag, '')
FROM V_SecMasterData SM
LEFT OUTER JOIN V_UDA_DynamicUDA UDA ON UDA.Symbol_PK = SM.Symbol_PK

CREATE TABLE #VT (
	AccountName VARCHAR(100)
	,TaxlotID VARCHAR(50)
	,FundID INT
	,Symbol VARCHAR(100)
	,AveragePrice FLOAT
	,Side VARCHAR(20)
	,Level1AllocationID VARCHAR(50)
	,TaxLotQty FLOAT
	,SettlementDate DATETIME
	,Commission FLOAT
	,OtherBrokerFee FLOAT
	---- ,GroupRefID INT              
	,StampDuty FLOAT
	,TransactionLevy FLOAT
	,ClearingFee FLOAT
	,TaxOnCommissions FLOAT
	,MiscFees FLOAT
	,AUECLocalDate DATETIME
	,PBID INT
	,FXRate FLOAT
	,FXConversionMethodOperator VARCHAR(3)
	,FromDeleted VARCHAR(5)
	,FXRate_Taxlot FLOAT
	,FXConversionMethodOperator_Taxlot VARCHAR(3)
	,Description VARCHAR(200)
	,SecFee FLOAT
	,OccFee FLOAT
	,OrfFee FLOAT
	,ClearingBrokerFee FLOAT
	,SoftCommission FLOAT
	,TaxlotState VARCHAR(50)
	,CounterParty VARCHAR(50)
	,ProcessDate DATETIME
	,Exchange VARCHAR(50)
	,CUSIP VARCHAR(50)
	,RIC VARCHAR(50)
	,BBCode VARCHAR(50)
	,ISIN VARCHAR(50)
	,SEDOL VARCHAR(50)
	,OSISymbol VARCHAR(50)
	,StrikePrice FLOAT
	,ExpirationDate DATETIME
	,PutOrCall VARCHAR(10)
	,UnderlyingSymbol VARCHAR(50)
	,Multiplier FLOAT
	,CurrencySymbol VARCHAR(20)
	,SettlCurrency VARCHAR(20)
	,Asset VARCHAR(50)
	,GroupID VARCHAR(30)
	)

INSERT INTO #VT
SELECT CF.FundName AS AccountName
	,VT.TaxlotID AS TaxlotID
	,VT.FundID AS FundID
	,VT.Symbol
	,VT.AvgPrice AS AveragePrice
	,T_Side.Side AS SideID
	,VT.Level1AllocationID AS Level1AllocationID
	,VT.TaxLotQty
	,VT.SettlementDate
	,VT.Commission
	,VT.OtherBrokerFees AS OtherBrokerFee
	---- ,VT.GroupRefID              
	,ISNULL(VT.StampDuty, 0) AS StampDuty
	,ISNULL(VT.TransactionLevy, 0) AS TransactionLevy
	,ISNULL(ClearingFee, 0) AS ClearingFee
	,ISNULL(TaxOnCommissions, 0) AS TaxOnCommissions
	,ISNULL(MiscFees, 0) AS MiscFees
	,VT.AUECLocalDate AS AUECLocalDate
	,@ThirdPartyID AS PBID
	,VT.FXRate
	,VT.FXConversionMethodOperator
	,'No' AS FromDeleted
	,VT.FXRate_Taxlot
	,VT.FXConversionMethodOperator_Taxlot
	,VT.Description
	,ISNULL(VT.SecFee, 0) AS SecFee
	,ISNULL(VT.OccFee, 0) AS OccFee
	,ISNULL(VT.OrfFee, 0) AS OrfFee
	,VT.ClearingBrokerFee
	,VT.SoftCommission
	,0 AS TaxlotState
	,CP.ShortName AS CounterParty
	,ProcessDate
	,Exchange.DisplayName AS Exchange
	,SM.CUSIPSymbol AS CUSIP
	,SM.ReutersSymbol AS RIC
	,SM.BloombergSymbol AS BBCode
	,SM.ISINSymbol AS ISIN
	,SM.SEDOLSymbol AS SEDOL
	,SM.OSISymbol AS OSISymbol
	,SM.StrikePrice AS StrikePrice
	,SM.ExpirationDate AS ExpirationDate
	,SM.PutOrCall AS PutOrCall
	,SM.UnderlyingSymbol AS UnderlyingSymbol
	,SM.Multiplier AS Multiplier
	,Currency.CurrencySymbol AS CurrencySymbol
	,TC.CurrencySymbol AS SettlCurrency
	,SM.AssetName AS Asset
	,VT.GroupID
FROM V_TaxLots VT
INNER JOIN @Fund Fund ON VT.FundID = Fund.FundID
INNER JOIN @AUECID AUEC ON VT.AUECID = AUEC.AUECID
INNER JOIN T_CompanyFunds CF ON CF.CompanyFundID = VT.FundID
INNER JOIN T_Side ON T_Side.SideTagValue = VT.OrderSideTagValue
INNER JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
INNER JOIN T_Currency AS TC ON TC.CurrencyID = VT.SettlCurrency_Taxlot
LEFT OUTER JOIN #SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol
LEFT JOIN T_CounterParty CP ON CP.CounterPartyID = VT.CounterPartyID
LEFT OUTER JOIN T_Exchange Exchange ON Exchange.ExchangeID = VT.ExchangeID
WHERE 
	(
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
	AND Datediff(Day, (
			CASE 
				WHEN @DateType = 1
					THEN VT.AUECLocalDate
				ELSE VT.ProcessDate
				END
			), @InputDate) = 0

CREATE TABLE #TempGroupIDTable (GroupID VARCHAR(30))

INSERT INTO #TempGroupIDTable
SELECT DISTINCT GroupID
FROM #VT
WHERE TaxlotState = 1

SELECT TaxlotID
INTO #TempTaxlotIDTable
FROM V_Taxlots
WHERE GroupID IN (
		SELECT GroupID
		FROM #TempGroupIDTable
		)

DELETE #VT
WHERE TaxlotID IN (
		SELECT TaxlotId
		FROM #TempTaxlotIDTable
		)

DROP TABLE #TempGroupIDTable
	,#TempTaxlotIDTable

UPDATE #VT
SET AccountName = CASE 
		WHEN CharIndex('Pershing', AccountName) <> 0
			THEN 'Pershing'
		WHEN CharIndex('KH Tech', AccountName) <> 0
			THEN 'Pershing'
		WHEN CharIndex('SEI - Shorts - Morgan Stanley', AccountName) <> 0
			THEN 'MorganStanley'
		WHEN CharIndex('LoCorr - Shorts - Morgan Stanley', AccountName) <> 0
			THEN 'MorganStanley'
		WHEN CharIndex('PACE - Shorts - Morgan Stanley', AccountName) <> 0
			THEN 'MorganStanley'
		WHEN CharIndex('State Street', AccountName) <> 0
			THEN 'StateStreet'
		WHEN CharIndex('BBH', AccountName) <> 0
			THEN 'BBH'
		ELSE AccountName
		END

UPDATE #VT
SET #VT.TaxlotState = PB.TaxlotState
FROM #VT
INNER JOIN T_PBWiseTaxlotState PB ON PB.TaxLotID = #VT.Level1AllocationID
WHERE PB.TaxlotState <> 0
	AND PB.PBID = @ThirdPartyID
	AND PB.FileFormatID = @fileFormatID

SELECT Max(VT.TaxlotID) AS TaxlotID
	,VT.AccountName
	,VT.Symbol AS Symbol
	,VT.AveragePrice AS AveragePrice
	,VT.Side
	,Max(VT.Level1AllocationID) AS Level1AllocationID
	,Sum(VT.TaxLotQty) AS AllocatedQty
	,Convert(VARCHAR, Max(VT.SettlementDate), 101) AS SettlementDate
	,VT.CounterParty
	,Sum(VT.Commission) AS CommissionCharged
	,Sum(VT.SoftCommission) AS SoftCommissionCharged
	,Sum(VT.OtherBrokerFee) AS OtherBrokerFee
	,ISNULL(Sum(VT.StampDuty), 0) AS StampDuty
	,ISNULL(Sum(VT.TransactionLevy), 0) AS TransactionLevy
	,ISNULL(Sum(ClearingFee), 0) AS ClearingFee
	,ISNULL(Sum(TaxOnCommissions), 0) AS TaxOnCommissions
	,ISNULL(Sum(MiscFees), 0) AS MiscFees
	,ISNULL(Sum(VT.SecFee), 0) AS SecFee
	,ISNULL(Sum(VT.OccFee), 0) AS OccFee
	,ISNULL(Sum(VT.OrfFee), 0) AS OrfFee
	,Sum(VT.ClearingBrokerFee) AS ClearingBrokerFee
	,Convert(VARCHAR, Max(VT.AUECLocalDate), 101) AS TradeDate
	,Max(VT.CUSIP) AS CUSIP
	,Max(VT.RIC) AS RIC
	,Max(VT.BBCode) AS BBCode
	,Max(VT.ISIN) AS ISIN
	,Max(VT.SEDOL) AS SEDOL
	,Max(VT.OSISymbol) AS OSISymbol
	,Max(VT.StrikePrice) AS StrikePrice
	,Convert(VARCHAR, Max(VT.ExpirationDate), 101) AS ExpirationDate
	,Max(VT.PutOrCall) AS PutOrCall
	,Max(VT.UnderlyingSymbol) AS UnderlyingSymbol
	,Max(VT.Multiplier) AS Multiplier
	,Max(VT.Exchange) AS Exchange
	,Max(VT.CurrencySymbol) AS CurrencySymbol
	,Max(VT.SettlCurrency) AS SettlCurrency
	,Max(VT.TaxlotState) AS TaxlotState
	,Max(VT.PBID) AS PBID
	,Max(VT.FXRate) AS FXRate
	,Max(VT.FXConversionMethodOperator) AS FXConversionMethodOperator
	,Max(VT.FXRate_Taxlot) AS FXRate_Taxlot
	,Max(VT.FXConversionMethodOperator_Taxlot) AS FXConversionMethodOperator_Taxlot
	,Max(VT.Description) AS Description
	,Max(Asset) AS Asset
	,'No' AS FromDeleted
FROM #VT VT
GROUP BY AccountName
	,DATEADD(dd, 0, DATEDIFF(dd, 0, AUECLocalDate))
	,Symbol
	,Side
	,AveragePrice
	,CounterParty

DROP TABLE #SecMasterData
	,#VT