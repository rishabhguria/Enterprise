/***********************************************        
Created by: Suraj nataraj        
Script Date: 09/08/2015        
Desc: For retrieving old and new data of amended trades        
http://jira.nirvanasolutions.com:8080/browse/PRANA-10893        
*************************************************/
CREATE PROCEDURE [dbo].[P_FFGetThirdPartyFundsDetails_Amend] (
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
--DECLARE	@thirdPartyID INT
--	,@companyFundIDs VARCHAR(max)
--	,@inputDate DATETIME
--	,@companyID INT
--	,@auecIDs VARCHAR(max)
--	,@TypeID INT
--	,-- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                      
--	@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                      
--	,@fileFormatID INT
--set @thirdPartyID=70
--set @companyFundIDs=N'28,29,30,31,27,'
--set @inputDate='2019-06-20 16:08:16'
--set @companyID=7
--set @auecIDs=N'71,63,34,43,1,15,11,62,73,12,80,32,88,81,'
--set @TypeID=0
--set @dateType=1
--set @fileFormatID=146
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
	,Multiplier INT
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
	TaxLotID VARCHAR(50)
	,Level2AllocationID VARCHAR(50)
	,FundID INT
	,OrderTypeTagValue VARCHAR(3)
	,SideID VARCHAR(5)
	,Symbol VARCHAR(100)
	,CounterPartyID INT
	,VenueID INT
	,OrderQty FLOAT
	,AvgPrice FLOAT
	,CumQty FLOAT
	,Quantity FLOAT
	,AUECID INT
	,AssetID INT
	,UnderlyingID INT
	,ExchangeID INT
	,CurrencyID INT
	,Level1AllocationID VARCHAR(50)
	,Level2Percentage FLOAT
	,--Percentage,                                                                             
	TaxLotQty FLOAT
	,IsBasketGroup VARCHAR(20)
	,SettlementDate DATETIME
	,Commission FLOAT
	,OtherBrokerFees FLOAT
	,GroupRefID INT
	,TaxlotState VARCHAR(50)
	,StampDuty FLOAT
	,TransactionLevy FLOAT
	,ClearingFee FLOAT
	,TaxOnCommissions FLOAT
	,MiscFees FLOAT
	,AUECLocalDate DATETIME
	,Level2ID INT
	,PBID INT
	,FXRate FLOAT
	,FXConversionMethodOperator VARCHAR(3)
	,FromDeleted VARCHAR(5)
	,ProcessDate DATETIME
	,OriginalPurchaseDate DATETIME
	,AccruedInterest FLOAT
	,BenchMarkRate FLOAT
	,Differential FLOAT
	,SwapDescription VARCHAR(500)
	,DayCount INT
	,FirstResetDate DATETIME
	,IsSwapped BIT
	,FXRate_Taxlot FLOAT
	,FXConversionMethodOperator_Taxlot VARCHAR(3)
	,LotID VARCHAR(200)
	,ExternalTransID VARCHAR(200)
	,TradeAttribute1 VARCHAR(200)
	,TradeAttribute2 VARCHAR(200)
	,TradeAttribute3 VARCHAR(200)
	,TradeAttribute4 VARCHAR(200)
	,TradeAttribute5 VARCHAR(200)
	,TradeAttribute6 VARCHAR(200)
	,Description VARCHAR(200)
	,SecFee FLOAT
	,OccFee FLOAT
	,OrfFee FLOAT
	,ClearingBrokerFee FLOAT
	,SoftCommission FLOAT
	,TransactionType VARCHAR(200)
	,SettlCurrency INT
	,ChangeType INT
	,TaxlotStateID INT
	)

INSERT INTO #VT
SELECT VT.Level1AllocationID AS TaxlotID
	,VT.TaxLotID AS Level2AllocationID
	,ISNULL(VT.FundID, 0) AS FundID
	,VT.OrderTypeTagValue
	,VT.OrderSideTagValue AS SideID
	,VT.Symbol
	,VT.CounterPartyID
	,VT.VenueID
	,(VT.TaxLotQty) AS OrderQty
	,--AllocatedQty                                                                                                                                                          
	VT.AvgPrice
	,VT.CumQty
	,--ExecutedQty                                                                                              
	VT.Quantity
	,--TotalQty                                                                                                               
	VT.AUECID
	,VT.AssetID
	,VT.UnderlyingID
	,VT.ExchangeID
	,VT.CurrencyID
	,VT.Level1AllocationID AS Level1AllocationID
	,(VT.Level2Percentage)
	,--Percentage,                                                                                               
	(VT.TaxLotQty)
	,'' AS IsBasketGroup
	,VT.SettlementDate
	,VT.Commission
	,VT.OtherBrokerFees
	,VT.GroupRefID
	,0 AS TaxlotState
	,ISNULL(VT.StampDuty, 0) AS StampDuty
	,ISNULL(VT.TransactionLevy, 0) AS TransactionLevy
	,ISNULL(ClearingFee, 0) AS ClearingFee
	,ISNULL(TaxOnCommissions, 0) AS TaxOnCommissions
	,ISNULL(MiscFees, 0) AS MiscFees
	,VT.AUECLocalDate
	,0 AS Level2ID
	,@thirdPartyID
	,VT.FXRate
	,VT.FXConversionMethodOperator
	,'No' AS FromDeleted
	,VT.ProcessDate
	,VT.OriginalPurchaseDate
	,VT.AccruedInterest
	,VT.BenchMarkRate
	,VT.Differential
	,VT.SwapDescription
	,VT.DayCount
	,VT.FirstResetDate
	,VT.IsSwapped
	,VT.FXRate_Taxlot
	,VT.FXConversionMethodOperator_Taxlot
	,VT.LotID
	,VT.ExternalTransID
	,VT.TradeAttribute1
	,VT.TradeAttribute2
	,VT.TradeAttribute3
	,VT.TradeAttribute4
	,VT.TradeAttribute5
	,VT.TradeAttribute6
	,VT.Description
	,ISNULL(VT.SecFee, 0) AS SecFee
	,ISNULL(VT.OccFee, 0) AS OccFee
	,ISNULL(VT.OrfFee, 0) AS OrfFee
	,VT.ClearingBrokerFee
	,VT.SoftCommission
	,VT.TransactionType
	,VT.SettlCurrency_Taxlot AS SettlCurrency
	,VT.ChangeType AS ChangeType
	,0 AS TaxlotStateID
FROM V_TaxLots VT
INNER JOIN @Fund Fund ON Fund.FundID = VT.FundID
INNER JOIN @AUECID AUEC ON AUEC.AUECID = VT.AUECID
INNER JOIN #SecMasterData SM ON VT.Symbol = SM.TickerSymbol
WHERE datediff(d, (
			CASE 
				WHEN @dateType = 1
					THEN VT.AUECLocalDate
				ELSE VT.ProcessDate
				END
			), @inputdate) >= 0
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
		OR TransactionSource = 13
		)

UNION ALL

SELECT TDT.Level1AllocationID AS TaxlotID
	,TDT.TaxLotID AS Level2AllocationID
	,ISNULL(TDT.FundID, 0) AS FundID
	,TDT.OrderTypeTagValue
	,TDT.OrderSideTagValue AS SideID
	,TDT.Symbol
	,TDT.CounterPartyID
	,TDT.VenueID
	,(TDT.TaxLotQty) AS OrderQty
	,TDT.AvgPrice
	,TDT.CumQty
	,TDT.Quantity
	,TDT.AUECID
	,TDT.AssetID
	,TDT.UnderlyingID
	,TDT.ExchangeID
	,TDT.CurrencyID
	,TDT.Level1AllocationID AS Level1AllocationID
	,(TDT.Level2Percentage)
	,(TDT.TaxLotQty)
	,' ' AS IsBasketGroup
	,TDT.SettlementDate
	,(TDT.Commission)
	,(TDT.OtherBrokerFees)
	,TDT.GroupRefID
	,TDT.TaxLotState
	,(ISNULL(TDT.StampDuty, 0)) AS StampDuty
	,(ISNULL(TDT.TransactionLevy, 0)) AS TransactionLevy
	,(ISNULL(TDT.ClearingFee, 0)) AS ClearingFee
	,(ISNULL(TDT.TaxOnCommissions, 0)) AS TaxOnCommissions
	,(ISNULL(TDT.MiscFees, 0)) AS MiscFees
	,TDT.AUECLocalDate
	,0 AS Level2ID
	,TDT.PBID
	,TDT.FXRate AS FXRate
	,TDT.FXConversionMethodOperator AS FXConversionMethodOperator
	,'Yes' AS FromDeleted
	,TDT.ProcessDate
	,TDT.OriginalPurchaseDate
	,TDT.AccruedInterest
	,TDT.BenchMarkRate
	,TDT.Differential
	,TDT.SwapDescription
	,TDT.DayCount
	,TDT.FirstResetDate
	,TDT.IsSwapped
	,TDT.FXRate_Taxlot
	,TDT.FXConversionMethodOperator_Taxlot
	,TDT.LotID
	,TDT.ExternalTransID
	,TDT.TradeAttribute1
	,TDT.TradeAttribute2
	,TDT.TradeAttribute3
	,TDT.TradeAttribute4
	,TDT.TradeAttribute5
	,TDT.TradeAttribute6
	,TDT.Description
	,(ISNULL(TDT.SecFee, 0)) AS SecFee
	,(ISNULL(TDT.OccFee, 0)) AS OccFee
	,(ISNULL(TDT.OrfFee, 0)) AS OrfFee
	,(TDT.ClearingBrokerFee)
	,(TDT.SoftCommission)
	,TDT.TransactionType
	,TDT.SettlCurrency
	,'3' AS ChangeType
	,TDT.TaxlotState AS TaxlotStateID
FROM T_DeletedTaxLots TDT
INNER JOIN @Fund Fund ON Fund.FundID = TDT.FundID
INNER JOIN @AUECID AUEC ON AUEC.AUECID = TDT.AUECID
INNER JOIN #SecMasterData SM ON SM.TickerSymbol = TDT.Symbol
WHERE datediff(d, (
			CASE 
				WHEN @dateType = 1
					THEN TDT.AUECLocalDate
				ELSE TDT.ProcessDate
				END
			), @inputdate) >= 0
	AND (
		FileFormatID = @fileFormatID
		OR FileFormatID = 0
		)
	AND TDT.TaxlotState = 3

UPDATE #VT
SET #VT.TaxlotState = PB.TaxlotState
	,#VT.TaxlotStateID = PB.TaxlotState
FROM #VT
INNER JOIN T_PBWiseTaxlotState PB ON PB.TaxLotID = #VT.Level1AllocationID
WHERE PB.TaxlotState <> 0
	AND PB.PBID = @ThirdPartyID
	AND PB.FileFormatID = @fileFormatID

CREATE TABLE #TaxlotsDates (TaxlotId NVARCHAR(20))

CREATE TABLE #T_TradeAuditAction (
	ActionType INT PRIMARY KEY
	,ActionName NVARCHAR(50)
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	1
	,'REALLOCATE'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	2
	,'UNALLOCATE'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	3
	,'GROUP'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	4
	,'UNGROUP'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	5
	,'DELETE'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	6
	,'TradeDate_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	7
	,'OrderSide_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	8
	,'Counterparty_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	9
	,'ExecutedQuantity_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	10
	,'AvgPrice_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	11
	,'SettlementDate_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	12
	,'FxRate_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	13
	,'Commission_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	14
	,'OtherBrokerFees_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	15
	,'StampDuty_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	16
	,'TransactionLevy_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	17
	,'ClearingFee_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	18
	,'TaxOnCommission_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	19
	,'MiscFees_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	20
	,'Venue_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	21
	,'FxConversionMethodOperator_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	22
	,'ProcessDate_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	23
	,'OriginalPurchaseDate_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	24
	,'Description_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	25
	,'AccruedInterest_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	26
	,'UnderlyingDelta_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	27
	,'LotId_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	28
	,'CommissionAmount_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	29
	,'CommissionRate_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	30
	,'TradeAttribute1_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	31
	,'TradeAttribute2_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	32
	,'TradeAttribute3_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	33
	,'TradeAttribute4_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	34
	,'TradeAttribute5_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	35
	,'TradeAttribute6_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	36
	,'ExternalTransId_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	37
	,'TradeEdited'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	38
	,'SecFee_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	39
	,'OccFee_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	40
	,'OrfFee_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	41
	,'ClearingBrokerFee_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	42
	,'SoftCommission_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	43
	,'SoftCommissionAmount_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	44
	,'SoftCommissionRate_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	45
	,'TransactionType_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	46
	,'InternalComments_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	47
	,'SettlCurrency_Changed'
	)

INSERT INTO #T_TradeAuditAction
VALUES (
	48
	,'OptionPremiumAdjustment_Changed'
	)

INSERT INTO #TaxlotsDates
SELECT DISTINCT TA.TaxlotId
FROM T_TradeAudit TA
INNER JOIN #T_TradeAuditAction TAA ON TA.Action = TAA.ActionType
INNER JOIN T_ThirdPartyFFGenerationDate TDG ON TDG.TaxlotID = TA.TaxlotId
WHERE DATEDIFF(d, TDG.GenerationDate, ta.ActionDate) >= 0
	AND TAA.ActionName NOT IN (
		'REALLOCATE'
		,'UNALLOCATE'
		)

SELECT VT.TaxlotID AS TaxlotID
	,VT.Level1AllocationID AS EntityID
	,ISNULL(VT.FundID, 0) AS FundID
	,ISNULL(F.FundName, '') AS FundName
	,ISNULL(T_OrderType.OrderTypesID, 0) AS OrderTypesID
	,ISNULL(T_OrderType.OrderTypes, 'Multiple') AS OrderTypes
	,VT.SideID
	,T_Side.Side AS Side
	,VT.Symbol
	,VT.CounterPartyID
	,C.Shortname AS CounterParty
	,VT.VenueID
	,Sum(VT.TaxLotQty) AS OrderQty                                                           
	,VT.AvgPrice
	,VT.CumQty                                                                                                                                                                   
	,VT.Quantity                                                                           
	,VT.AUECID
	,T_Asset.AssetName AS Asset
	,VT.UnderlyingID
	,VT.ExchangeID
	,E.DisplayName AS Exchange
	,Currency.CurrencyID
	,Currency.CurrencyName
	,Currency.CurrencySymbol
	,CTPM.MappedName
	,CTPM.FundAccntNo
	,CTPM.FundTypeID_FK
	,FT.FundTypeName
	,VT.Level1AllocationID AS Level1AllocationID
	,Sum(VT.Level2Percentage) AS Level2Percentage
	,Sum(VT.TaxLotQty)
	,'' AS IsBasketGroup
	,SM.PutOrCall
	,SM.StrikePrice
	,SM.ExpirationDate
	,VT.SettlementDate
	,Sum(VT.Commission) AS CommissionCharged
	,Sum(VT.OtherBrokerFees) AS OtherBrokerFees
	,T_ThirdPartyType.ThirdPartyTypeID
	,T_ThirdPartyType.ThirdPartyTypeName                 
	,VT.GroupRefID
	,VT.GroupRefID AS PBUniqueID
	,CASE 
		WHEN VT.TaxLotState = '0'
			THEN 'Allocated'
		WHEN VT.TaxLotState = '1'
			THEN 'Sent'
		WHEN VT.TaxLotState = '2'
			THEN 'Amemded'
		WHEN VT.TaxLotState = '3'
			THEN 'Deleted'
		WHEN VT.TaxLotState = '4'
			THEN 'Ignored'
		END AS TaxLotState
	,Sum(ISNULL(VT.StampDuty, 0)) AS StampDuty
	,Sum(ISNULL(VT.TransactionLevy, 0)) AS TransactionLevy
	,Sum(ISNULL(VT.ClearingFee, 0)) AS ClearingFee
	,Sum(ISNULL(VT.TaxOnCommissions, 0)) AS TaxOnCommissions
	,Sum(ISNULL(VT.MiscFees, 0)) AS MiscFees
	,VT.AUECLocalDate AS TradeDate
	,SM.Multiplier AS AssetMultiplier
	,0 AS Level2ID
	,SM.ISINSymbol AS ISIN
	,SM.CUSIPSymbol AS CUSIP
	,SM.SEDOLSymbol AS SEDOL
	,SM.ReutersSymbol AS RIC
	,SM.BloombergSymbol AS BBCode
	,SM.CompanyName
	,SM.UnderlyingSymbol
	,SM.LeadCurrencyID
	,SM.LeadCurrency AS LeadCurrencyName
	,SM.VsCurrencyID
	,SM.VsCurrency AS VsCurrencyName
	,SM.OSISymbol AS OSIOptionSymbol
	,SM.IDCOSymbol
	,SM.OpraSymbol
	,VT.FXRate
	,VT.FXConversionMethodOperator
	,VT.FromDeleted
	,VT.ProcessDate
	,VT.OriginalPurchaseDate
	,VT.AccruedInterest
	,
	-- Reserved for future use                                                          
	'' AS Comment1
	,'' AS Comment2
	,
	--FixedIncome Members                                                        
	SM.Coupon
	,SM.IssueDate
	,SM.FirstCouponDate
	,SM.CouponFrequencyID
	,Sm.AccrualBasisID
	,SM.BondTypeID
	--Swap Parameters                                   
	,isnull(VT.BenchMarkRate, 0) AS BenchMarkRate
	,isnull(VT.Differential, 0) AS Differential
	,isnull(VT.SwapDescription, '') AS SwapDescription
	,isnull(VT.DayCount, 0) AS DayCount
	,isnull(VT.FirstResetDate, '1800-01-01') AS FirstResetDate
	,isnull(VT.IsSwapped, 0) AS IsSwapped
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
	,VT.LotID
	,VT.ExternalTransID
	,VT.TradeAttribute1
	,VT.TradeAttribute2
	,VT.TradeAttribute3
	,VT.TradeAttribute4
	,VT.TradeAttribute5
	,VT.TradeAttribute6
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
	,VT.ChangeType AS ChangeType
	----------Dynamic UDA---------------                  
	,ISNULL(Analyst, '') AS Analyst
	,ISNULL(CountryOfRisk, '') AS CountryOfRisk
	,ISNULL(CustomUDA1, '') AS CustomUDA1
	,ISNULL(CustomUDA2, '') AS CustomUDA2
	,ISNULL(CustomUDA3, '') AS CustomUDA3
	,ISNULL(CustomUDA4, '') AS CustomUDA4
	,ISNULL(CustomUDA5, '') AS CustomUDA5
	,ISNULL(CustomUDA6, '') AS CustomUDA6
	,ISNULL(CustomUDA7, '') AS CustomUDA7
	,ISNULL(Issuer, '') AS Issuer
	,ISNULL(LiquidTag, '') AS LiquidTag
	,ISNULL(MarketCap, '') AS MarketCap
	,ISNULL(Region, '') AS Region
	,ISNULL(RiskCurrency, '') AS RiskCurrency
	,ISNULL(UCITSEligibleTag, '') AS UCITSEligibleTag
	--,taa.ActionName                  
	--,ta.OriginalValue                  
	,isnull(TGD.TradeDate, '1800-01-01') AS OldTradeDate
	,isnull(S.Side, '') AS OldSide
	,isnull(TGD.CounterpartyID, 0) AS OldCounterpartyID
	,isnull(OldC.ShortName, '') AS OldCounterparty
	,isnull(TGD.ExecutedQuantity, 0) AS OldExecutedQuantity
	,ISNULL(TGD.AvgPrice, 0) AS OldAvgPrice
	,isnull(TGD.SettlmentDate, '1800-01-01') AS OldSettlementDate
	,isnull(TGD.FXRate, 0) AS OldFXRate
	,ISNULL(TGD.Commission, 0) AS OldCommission
	,ISNULL(TGD.OtherBrokerFees, 0) AS OldOtherBrokerFees
	,ISNULL(TGD.StampDuty, 0) AS OldStampDuty
	,ISNULL(TGD.TransactionLevy, 0) AS OldTransactionLevy
	,ISNULL(TGD.ClearingFee, 0) AS OldClearingFee
	,ISNULL(TGD.MiscFees, 0) AS OldMiscFees
	,ISNULL(TGD.VenueID, 0) AS OldVenueID
	,TGD.FXConversionMethodOperator AS OldFXConversionMethodOperator
	,ISNULL(TGD.ProcessDate, '1800-01-01') AS OldProcessDate
	,ISNULL(TGD.OriginalPurchaseDate, '1800-01-01') AS OldOriginalPurchaseDate
	,TGD.Description AS OldDescription
	,ISNULL(TGD.AccruedInterest, 0) AS OldAccruedInterest
	,ISNULL(TGD.UnderlyingDelta, 0) AS OldUnderlyingDelta
	,TGD.LotID AS OldLotID
	,ISNULL(TGD.CommissionAmount, 0) AS OldCommissionAmount
	,ISNULL(TGD.CommissionRate, 0) AS OldCommissionRate
	,ISNULL(TGD.SecFee, 0) AS OldSecFee
	,ISNULL(TGD.OccFee, 0) AS OldOccFee
	,ISNULL(TGD.OrfFee, 0) AS OldOrfFee
	,ISNULL(TGD.ClearingBrokerFee, 0) AS OldClearingBrokerFee
	,ISNULL(TGD.SoftCommission, 0) AS OldSoftCommission
	,ISNULL(TGD.SoftCommissionAmount, 0) AS OldSoftCommissionAmount
	,TGD.TransactionType AS OldTransactionType
	,ISNULL(OTC.CurrencySymbol, '') AS OldSettlCurrency
	,isnull(TGD.GenerationDate, '1800-01-01') AS GenerationDate
	,isnull(TGD.AmendTaxLotId1, '') AS AmendTaxLotId1
	,isnull(TGD.AmendTaxLotId2, '') AS AmendTaxLotId2
	,isnull(TGD.TaxOnCommissions, 0) AS OldTaxOnCommissions
	,VT.TaxlotStateID AS TaxlotStateID
FROM #VT VT
LEFT JOIN #TaxlotsDates td ON td.TaxlotId = VT.Level2AllocationID
LEFT JOIN T_ThirdPartyFFGenerationDate TGD ON TGD.TaxLotID = VT.Level2AllocationID
	AND TGD.fileFormatID = @fileFormatID
	AND TGD.thirdPartyID = @thirdPartyID
--left JOIN T_TradeAudit ta on ta.TaxlotId = td.TaxlotId and td.Date = ta.ActionDate                  
--left JOIN T_TradeAuditAction taa on ta.Action = taa.ActionType                  
INNER JOIN T_Asset ON VT.AssetID = T_Asset.AssetID
LEFT JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID
LEFT JOIN T_CompanyFunds F ON F.CompanyFundID = VT.FundID
LEFT JOIN T_CounterParty C ON C.CounterPartyID = VT.CounterPartyID
LEFT JOIN T_Exchange E ON E.ExchangeID = VT.ExchangeID
LEFT JOIN T_CounterParty OldC ON OldC.CounterPartyID = TGD.CounterPartyID
LEFT JOIN T_FundType AS FT ON FT.FundTypeID = CTPM.FundTypeID_FK
LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
LEFT JOIN T_Currency AS TC ON TC.CurrencyID = VT.SettlCurrency
LEFT JOIN T_Currency AS OTC ON OTC.CurrencyID = TGD.SettlCurrency
LEFT JOIN T_Side ON dbo.T_Side.SideTagValue = VT.SideID
LEFT JOIN T_Side S ON S.SideTagValue = TGD.SideID
LEFT JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID
LEFT JOIN T_Country ON dbo.T_Country.CountryID = T_Exchange.Country
LEFT JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue
INNER JOIN dbo.T_CompanyThirdParty ON T_CompanyThirdParty.CompanyThirdPartyID = CTPM.CompanyThirdPartyID_FK
INNER JOIN dbo.T_ThirdParty ON T_ThirdParty.ThirdPartyId = T_CompanyThirdParty.ThirdPartyId
	AND T_CompanyThirdParty.CompanyID = @companyID
INNER JOIN dbo.T_ThirdPartyType ON T_ThirdPartyType.ThirdPartyTypeId = T_ThirdParty.ThirdPartyTypeID
LEFT JOIN dbo.T_CompanyThirdPartyFlatFileSaveDetails CTPFD ON CTPFD.CompanyThirdPartyID = CTPM.CompanyThirdPartyID_FK
LEFT JOIN T_CounterPartyVenue ON T_CounterPartyVenue.CounterPartyID = VT.CounterPartyID
	AND T_CounterPartyVenue.VenueID = VT.VenueID
LEFT OUTER JOIN T_CompanyCounterPartyVenues ON T_CompanyCounterPartyVenues.CounterPartyVenueID = T_CounterPartyVenue.CounterPartyVenueID
	AND T_CompanyCounterPartyVenues.CompanyID = @companyID
LEFT OUTER JOIN T_CompanyThirdPartyCVIdentifier ON T_CompanyCounterPartyVenues.CompanyCounterPartyCVID = T_CompanyThirdPartyCVIdentifier.CompanyCounterPartyVenueID_FK
	AND T_CompanyThirdPartyCVIdentifier.CompanyThirdPartyID_FK = @thirdPartyID
LEFT OUTER JOIN #SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol
WHERE (
		(
			(VT.TaxLotState <> 1)
			OR (
				VT.TaxLotState = 1
				AND datediff(d, (
						CASE 
							WHEN @dateType = 1
								THEN VT.AUECLocalDate
							ELSE VT.ProcessDate
							END
						), @inputdate) = 0
				)
			)
		AND (
			VT.TaxLotState <> 4
			OR (
				VT.TaxLotState = 4
				AND datediff(d, (
						CASE 
							WHEN @dateType = 1
								THEN VT.AUECLocalDate
							ELSE VT.ProcessDate
							END
						), @inputdate) = 0
				)
			)
		)
	AND VT.PBID = @thirdPartyID
GROUP BY VT.TaxlotID
	,VT.Level1AllocationID
	,VT.FundID
	,F.FundName
	,T_OrderType.OrderTypesID
	,T_OrderType.OrderTypes
	,VT.SideID
	,T_Side.Side
	,VT.Symbol
	,VT.CounterPartyID
	,C.ShortName
	,VT.VenueID
	,VT.AvgPrice
	,VT.CumQty
	,VT.Quantity
	,VT.AUECID
	,T_Asset.AssetName
	,VT.AssetID
	,VT.UnderlyingID
	,VT.ExchangeID
	,E.DisplayName
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
	,T_ThirdPartyType.ThirdPartyTypeID
	,T_ThirdPartyType.ThirdPartyTypeName
	,CTPFD.CompanyIdentifier
	,T_CounterPartyVenue.DisplayName
	,T_CompanyThirdPartyCVIdentifier.CVIdentifier
	,T_CompanyCounterPartyVenues.CompanyCounterPartyCVID
	,VT.GroupRefID
	,VT.TaxLotState
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
	,VT.FromDeleted
	,VT.ProcessDate
	,VT.OriginalPurchaseDate
	,VT.AccruedInterest
	,SM.Coupon
	,SM.IssueDate
	,SM.FirstCouponDate
	,SM.CouponFrequencyID
	,Sm.AccrualBasisID
	,SM.BondTypeID
	,
	--Swap Parameters                                                        
	VT.BenchMarkRate
	,VT.Differential
	,VT.SwapDescription
	,VT.DayCount
	,VT.FirstResetDate
	,VT.IsSwapped
	,T_Country.CountryName
	,VT.FXRate_Taxlot
	,VT.FXConversionMethodOperator_Taxlot
	,VT.LotID
	,VT.ExternalTransID
	,VT.TradeAttribute1
	,VT.TradeAttribute2
	,VT.TradeAttribute3
	,VT.TradeAttribute4
	,VT.TradeAttribute5
	,VT.TradeAttribute6
	,SM.AssetName
	,SM.SecurityTypeName
	,SM.SectorName
	,SM.SubSectorName
	,SM.CountryName
	,VT.Description
	,VT.TransactionType
	,VT.SettlCurrency
	,TC.CurrencySymbol
	,VT.ChangeType
	--------------Dynamic UDA-----------------                  
	,Analyst
	,CountryOfRisk
	,CustomUDA1
	,CustomUDA2
	,CustomUDA3
	,CustomUDA4
	,CustomUDA5
	,CustomUDA6
	,CustomUDA7
	,Issuer
	,LiquidTag
	,MarketCap
	,Region
	,RiskCurrency
	,UCITSEligibleTag
	--------------Dynamic UDA-----------------                  
	,TGD.TradeDate
	,S.Side
	,TGD.CounterpartyID
	,OldC.ShortName
	,TGD.ExecutedQuantity
	,TGD.AvgPrice
	,TGD.SettlmentDate
	,TGD.FXRate
	,TGD.Commission
	,TGD.OtherBrokerFees
	,TGD.StampDuty
	,TGD.TransactionLevy
	,TGD.ClearingFee
	,TGD.MiscFees
	,TGD.VenueID
	,TGD.FXConversionMethodOperator
	,TGD.ProcessDate
	,TGD.OriginalPurchaseDate
	,TGD.Description
	,TGD.AccruedInterest
	,TGD.UnderlyingDelta
	,TGD.LotID
	,TGD.CommissionAmount
	,TGD.CommissionRate
	,TGD.SecFee
	,TGD.OccFee
	,TGD.OrfFee
	,TGD.ClearingBrokerFee
	,TGD.SoftCommission
	,TGD.SoftCommissionAmount
	,TGD.TransactionType
	,OTC.CurrencySymbol
	,TGD.GenerationDate
	,TGD.AmendTaxLotId1
	,TGD.AmendTaxLotId2
	,TGD.TaxOnCommissions
	,VT.TaxlotStateID
ORDER BY GroupRefID

DROP TABLE #VT
	,#SecMasterData
	,#TaxlotsDates
	,#T_TradeAuditAction
