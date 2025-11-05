
CREATE PROCEDURE [dbo].[P_FFGetThirdPartyFundsDetails_Amend] 
 (
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

--declare	@thirdPartyID INT
--declare	@companyFundIDs VARCHAR(max)
--declare @inputDate DATETIME
--declare	@companyID INT
--declare	@auecIDs VARCHAR(max)
--declare	@TypeID INT                         
--declare	@dateType INT                                                                                                                                                           
--declare	@fileFormatID INT

--Set @thirdPartyID=54
--Set @companyFundIDs=N'1301,1299,1343,1341,1342,1344,1302,1298,1300'
--Set @inputDate='2023-12-13 04:19:31.917'
--Set @companyID=7
--Set @auecIDs=N'72,20,69,65,77,76,63,111,53,49,44,34,43,78,56,59,31,54,45,27,21,18,123,61,1,15,11,62,73,12,32,81'
--Set @TypeID=1
--Set @dateType=0
--Set @fileFormatID=121

Declare @CounterPartyId Int

Set @CounterPartyId = 
(
Select CounterPartyID From T_ThirdParty
Where @TypeID <> 0 And  ThirdPartyTypeID = 3 And ThirdPartyID = @thirdPartyID 
And CounterPartyID Is Not Null and CounterPartyID <> -2147483648
)

--Select @CounterPartyId

Create Table #Temp_CounterPartyID
(
CounterPartyID INT
)

If (@CounterPartyId Is Null Or @CounterPartyId = '')
Begin
	Insert InTo #Temp_CounterPartyID
	Select CounterPartyID From T_CounterParty
End
Else
	Begin
		Insert InTo #Temp_CounterPartyID
		Select @CounterPartyId
	End

DECLARE @Fund TABLE (FundID INT)
DECLARE @AUECID TABLE (AUECID INT)
DECLARE @IncludeExpiredSettledTransaction INT
DECLARE @IncludeExpiredSettledUnderlyingTransaction INT
DECLARE @IncludeCATransaction INT

SELECT @IncludeExpiredSettledTransaction = IncludeExercisedAssignedTransaction
	,@IncludeExpiredSettledUnderlyingTransaction = IncludeExercisedAssignedUnderlyingTransaction
	,@IncludeCATransaction = IncludeCATransaction
FROM T_ThirdPartyFileFormat
WHERE FileFormatId = @fileFormatID

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
	,StrikePrice Float
	,ExpirationDate DATETIME
	,Multiplier Float
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
	,Coupon Float
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
	,TaxLotQty FLOAT
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
	,TaxLotID_1 VARCHAR(50)

	,Commission_BlockLevel FLOAT
	,OtherBrokerFees_BlockLevel FLOAT
	,StampDuty_BlockLevel FLOAT
	,TransactionLevy_BlockLevel FLOAT
	,ClearingFee_BlockLevel FLOAT
	,TaxOnCommissions_BlockLevel FLOAT
	,MiscFees_BlockLevel FLOAT
	,SecFee_BlockLevel FLOAT
	,OccFee_BlockLevel FLOAT
	,OrfFee_BlockLevel FLOAT
	,ClearingBrokerFee_BlockLevel FLOAT
	,SoftCommission_BlockLevel FLOAT
	,AccruedInterest_BlockLevel FLOAT
	)

INSERT INTO #VT
SELECT L2.Level1AllocationID AS TaxlotID
	,L2.TaxLotID AS Level2AllocationID
	,L1.FundID AS FundID
	,G.OrderTypeTagValue
	,G.OrderSideTagValue AS SideID
	,G.Symbol
	,G.CounterPartyID
	,G.VenueID
	,(L2.TaxLotQty) AS OrderQty
	,G.AvgPrice
	,G.CumQty
	,G.Quantity
	,G.AUECID
	,G.AssetID
	,G.UnderlyingID
	,G.ExchangeID
	,G.CurrencyID
	,L2.Level1AllocationID AS Level1AllocationID
	,(L2.Level2Percentage)
	,(L2.TaxLotQty) As TaxLotQty
	,'' AS IsBasketGroup
	,G.SettlementDate
	,L2.Commission
	,L2.OtherBrokerFees
	,G.GroupRefID
	,0 AS TaxlotState
	,L2.StampDuty AS StampDuty
	,L2.TransactionLevy AS TransactionLevy
	,L2.ClearingFee AS ClearingFee
	,L2.TaxOnCommissions AS TaxOnCommissions
	,L2.MiscFees AS MiscFees
	,G.AUECLocalDate
	,0 AS Level2ID
	,@thirdPartyID AS PBID
	,G.FXRate
	,G.FXConversionMethodOperator
	,'No' AS FromDeleted
	,G.ProcessDate
	,G.OriginalPurchaseDate
	,L2.AccruedInterest
	,SAWP.BenchMarkRate
	,SAWP.Differential
	,SAWP.SwapDescription
	,SAWP.DayCount
	,SAWP.FirstResetDate
	,G.IsSwapped
	,L2.FXRate As FXRate_Taxlot
	,L2.FXConversionMethodOperator As FXConversionMethodOperator_Taxlot
	,L2.LotID
	,L2.ExternalTransID
	,L2.TradeAttribute1
	,L2.TradeAttribute2
	,L2.TradeAttribute3
	,L2.TradeAttribute4
	,L2.TradeAttribute5
	,L2.TradeAttribute6
	,G.Description
	,L2.SecFee AS SecFee
	,L2.OccFee AS OccFee
	,L2.OrfFee AS OrfFee
	,L2.ClearingBrokerFee
	,L2.SoftCommission
	,G.TransactionType
	,L2.SettlCurrency AS SettlCurrency
	,G.ChangeType AS ChangeType
	,0 AS TaxlotStateID
	,L2.TaxLotID As TaxLotID_1
	,G.Commission As Commission_BlockLevel
	,G.OtherBrokerFees As OtherBrokerFees_BlockLevel
	,G.StampDuty As StampDuty_BlockLevel
	,G.TransactionLevy As TransactionLevy_BlockLevel
	,G.ClearingFee As ClearingFee_BlockLevel
	,G.TaxOnCommissions As TaxOnCommissions_BlockLevel
	,G.MiscFees As MiscFees_BlockLevel
	,G.SecFee As SecFee_BlockLevel
	,G.OccFee As OccFee_BlockLevel
	,G.OrfFee As OrfFee_BlockLevel
	,G.ClearingBrokerFee As ClearingBrokerFee_BlockLevel
	,G.SoftCommission As SoftCommission_BlockLevel
	,G.AccruedInterest As AccruedInterest_BlockLevel
FROM T_Group G
Inner Join T_FundAllocation L1 On L1.GroupID = G.GroupID
Inner Join T_Level2Allocation L2 On L2.Level1AllocationID = L1.AllocationId
INNER JOIN @Fund Fund ON Fund.FundID = L1.FundID
--INNER JOIN #SecMasterData SM ON G.Symbol = SM.TickerSymbol
INNER JOIN @AUECID auec ON auec.AUECID = G.AUECID
Inner Join #Temp_CounterPartyID CP On CP.CounterPartyID = G.CounterPartyID
LEFT OUTER JOIN dbo.T_SwapParameters SAWP ON G.GroupID = SAWP.GroupID
WHERE L2.TaxlotQty <> 0 And
DateDiff(Day, (
			CASE 
				WHEN @dateType = 1
				THEN G.AUECLocalDate
				ELSE G.ProcessDate
				END
			), @inputdate) >= 0
	AND (
		(
			G.TransactionType IN (
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
				G.TransactionSource IN (
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
			AND G.TransactionType IN (
				'Exercise'
				,'Expire'
				,'Assignment'
				)
			AND G.AssetID IN (
				2
				,4
				)
			)
		OR (
			@IncludeExpiredSettledTransaction = 1
			AND G.TransactionType IN (
				'CSCost'
				,'CSZero'
				,'DLCost'
				,'CSClosingPx'
				,'Expire'
				,'DLCostAndPNL'
				)
			AND G.AssetID IN (3)
			)
		OR (
			@IncludeExpiredSettledUnderlyingTransaction = 1
			AND G.TransactionType IN (
				'Exercise'
				,'Expire'
				,'Assignment'
				)
			AND G.TaxlotClosingID_FK IS NOT NULL
			AND G.AssetID IN (
				1
				,3
				)
			)
		OR (
			@IncludeCATransaction = 1
			AND (
				G.TransactionSource IN (
					6
					,7
					,8
					,9
					,10
					,11
					)
				)
			)
		OR G.TransactionSource = 13
		)


INSERT INTO #VT
SELECT TDT.Level1AllocationID AS TaxlotID
	,TDT.TaxLotID AS Level2AllocationID
	,ISNULL(TDT.FundID, 0) AS FundID
	,TDT.OrderTypeTagValue
	,TDT.OrderSideTagValue AS SideID
	,TDT.Symbol
	,TDT.CounterPartyID
	,TDT.VenueID
	,TDT.TaxLotQty AS OrderQty
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
	,TDT.TaxLotQty As TaxLotQty
	,' ' AS IsBasketGroup
	,TDT.SettlementDate
	,TDT.Commission
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
	,TDT.TaxLotID As TaxLotID_1
	,Cast(0 as Float) As Commission_BlockLevel
	,Cast(0 as Float) As OtherBrokerFees_BlockLevel
	,Cast(0 as Float) As StampDuty_BlockLevel
	,Cast(0 as Float) As TransactionLevy_BlockLevel
	,Cast(0 as Float) As ClearingFee_BlockLevel
	,Cast(0 as Float) As TaxOnCommissions_BlockLevel
	,Cast(0 as Float) As MiscFees_BlockLevel
	,Cast(0 as Float) As SecFee_BlockLevel
	,Cast(0 as Float) As OccFee_BlockLevel
	,Cast(0 as Float) As OrfFee_BlockLevel
	,Cast(0 as Float) As ClearingBrokerFee_BlockLevel
	,Cast(0 as Float) As SoftCommission_BlockLevel
	,Cast(0 as Float) As AccruedInterest_BlockLevel
FROM T_DeletedTaxLots TDT
INNER JOIN @Fund Fund ON Fund.FundID = TDT.FundID
INNER JOIN @AUECID auec ON auec.AUECID = TDT.AUECID
Inner Join #Temp_CounterPartyID CP On CP.CounterPartyID = TDT.CounterPartyID
WHERE (FileFormatID = @fileFormatID)
	AND TDT.TaxlotState = 3
	AND datediff(d, (
			CASE 
				WHEN @dateType = 1
					THEN TDT.AUECLocalDate
				ELSE TDT.ProcessDate
				END
			), @inputdate) >= 0

Select 
GroupRefID,
Sum(Commission) As Commission,
Sum(OtherBrokerFees) As OtherBrokerFees,
Sum(StampDuty) As StampDuty,
Sum(TransactionLevy) As TransactionLevy,
Sum(ClearingFee) As ClearingFee,
Sum(TaxOnCommissions) As TaxOnCommissions,
Sum(MiscFees) As MiscFees,
Sum(SecFee) As SecFee,
Sum(OccFee) As OccFee,
Sum(OrfFee) As OrfFee,
Sum(ClearingBrokerFee) As ClearingBrokerFee,
Sum(SoftCommission) As SoftCommission,
Sum(AccruedInterest) As AccruedInterest
InTo #Temp_DeletedGroupedData
From #VT
Where FromDeleted = 'Yes'
Group By GroupRefID

Update VT
Set 
VT.Commission_BlockLevel = GDeleted.Commission,
VT.OtherBrokerFees_BlockLevel = GDeleted.OtherBrokerFees,
VT.StampDuty_BlockLevel = GDeleted.StampDuty,
VT.TransactionLevy_BlockLevel = GDeleted.TransactionLevy,
VT.ClearingFee_BlockLevel = GDeleted.ClearingFee,
VT.TaxOnCommissions_BlockLevel = GDeleted.TaxOnCommissions,
VT.MiscFees_BlockLevel = GDeleted.MiscFees,
VT.SecFee_BlockLevel = GDeleted.SecFee,
VT.OccFee_BlockLevel = GDeleted.OccFee,
VT.OrfFee_BlockLevel = GDeleted.OrfFee,
VT.ClearingBrokerFee_BlockLevel = GDeleted.ClearingBrokerFee,
VT.SoftCommission_BlockLevel = GDeleted.SoftCommission,
VT.AccruedInterest_BlockLevel = GDeleted.AccruedInterest
From #Temp_DeletedGroupedData GDeleted
Inner Join #VT VT On VT.GroupRefID = GDeleted.GroupRefID
Where FromDeleted = 'Yes'


UPDATE #VT
SET #VT.TaxlotState = PB.TaxlotState
FROM #VT
INNER JOIN T_PBWiseTaxlotState PB With (NoLock) ON (PB.TaxlotID = #VT.TaxLotID_1)
WHERE PB.FileFormatID = @fileFormatID AND PB.TaxlotState <> 0

--Select Count(*) From #VT

CREATE TABLE #TaxlotsDates (TaxlotId NVARCHAR(20))

CREATE TABLE #T_TradeAuditAction (
	ActionType INT PRIMARY KEY
	,ActionName NVARCHAR(50)
	)

INSERT INTO #T_TradeAuditAction (
	ActionType
	,ActionName
	)
VALUES (1,'REALLOCATE')
	,(2,'UNALLOCATE')
	,(3,'GROUP')
	,(4,'UNGROUP')
	,(5,'DELETE')
	,(6,'TradeDate_Changed')
	,(7,'OrderSide_Changed')
	,(8,'Counterparty_Changed')
	,(9,'ExecutedQuantity_Changed')
	,(10,'AvgPrice_Changed')
	,(11,'SettlementDate_Changed')
	,(12,'FxRate_Changed'),
	(13,'Commission_Changed')
	,(14,'OtherBrokerFees_Changed')
	,(15,'StampDuty_Changed')
	,(16,'TransactionLevy_Changed')
	,(17,'ClearingFee_Changed')
	,(18,'TaxOnCommission_Changed')
	,(19,'MiscFees_Changed')
	,(20,'Venue_Changed')
	,(21,'FxConversionMethodOperator_Changed')
	,(22,'ProcessDate_Changed')
	,(23,'OriginalPurchaseDate_Changed')
	,(24,'Description_Changed')
	,(25,'AccruedInterest_Changed')
	,(26,'UnderlyingDelta_Changed')
	,(27,'LotId_Changed')
	,(28,'CommissionAmount_Changed')
	,(29,'CommissionRate_Changed')
	,(30,'TradeAttribute1_Changed')
	,(31,'TradeAttribute2_Changed')
	,(32,'TradeAttribute3_Changed')
	,(33,'TradeAttribute4_Changed')
	,(34,'TradeAttribute5_Changed')
	,(35,'TradeAttribute6_Changed')
	,(36,'ExternalTransId_Changed')
	,(37,'TradeEdited')
	,(38,'SecFee_Changed')
	,(39,'OccFee_Changed')
	,(40,'OrfFee_Changed')
	,(41,'ClearingBrokerFee_Changed')
	,(42,'SoftCommission_Changed')
	,(43,'SoftCommissionAmount_Changed')
	,(44,'SoftCommissionRate_Changed')
	,(45,'TransactionType_Changed')
	,(46,'InternalComments_Changed')
	,(47,'SettlCurrency_Changed')
	,(48,'OptionPremiumAdjustment_Changed')

INSERT INTO #TaxlotsDates
SELECT DISTINCT TA.TaxlotId
FROM T_TradeAudit TA
INNER JOIN #T_TradeAuditAction TAA ON TA.Action = TAA.ActionType
inner join T_Level2Allocation TL2 ON TL2.TaxLotID = TA.TaxlotId
INNER JOIN T_ThirdPartyFFGenerationDate TDG ON TDG.TaxlotID = TL2.Level1AllocationID
WHERE DATEDIFF(d, TDG.GenerationDate, ta.ActionDate) >= 0
	AND TAA.ActionName NOT IN ('REALLOCATE','UNALLOCATE')

Select *
InTo #VT_New FROM #VT VT
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
Where SM.TickerSymbol In
(
Select Distinct Symbol From #VT_New
)

DECLARE @MinTradeDate DATETIME

SELECT @MinTradeDate = MIN(Aueclocaldate) FROM #VT_New

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
EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange @MinTradeDate, @inputDate

UPDATE #FXConversionRates
SET RateValue = 1.0 / RateValue
WHERE RateValue <> 0
	AND ConversionMethod = 1

SELECT *
INTO #ZeroFundFxRate
FROM #FXConversionRates
WHERE fundID = 0

SELECT VT.TaxlotID AS TaxlotID
	,VT.Level1AllocationID AS EntityID
	,ISNULL(VT.FundID, 0) AS AccountID
	,ISNULL(F.FundName, '') AS AccountName
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
	,Sum(VT.TaxLotQty) As TaxLotQty
	,'' AS IsBasketGroup
	,ISNULL(SM.PutOrCall, 0) As PutOrCall
	,ISNULL(SM.StrikePrice, 0) As StrikePrice
	,ISNULL(SM.ExpirationDate, '1800/01/01') As ExpirationDate
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
	,ISNULL(SM.Multiplier, 1) AS AssetMultiplier
	,0 AS Level2ID
	,ISNULL(SM.ISINSymbol, ' ') AS ISIN
	,ISNULL(SM.CUSIPSymbol, ' ') AS CUSIP
	,ISNULL(SM.SEDOLSymbol, ' ') AS SEDOL
	,ISNULL(SM.ReutersSymbol, ' ') AS RIC
	,SM.BloombergSymbol AS BBCode
	,SM.CompanyName
	,SM.UnderlyingSymbol
	,ISNULL(SM.LeadCurrencyID, 0) As LeadCurrencyID
	,ISNULL(SM.LeadCurrency, '') AS LeadCurrencyName
	,ISNULL(SM.VsCurrencyID, 0) As VsCurrencyID
	,ISNULL(SM.VsCurrency, '') AS VsCurrencyName
	,SM.OSISymbol AS OSIOptionSymbol
	,SM.IDCOSymbol
	,SM.OpraSymbol
	,VT.FXRate
	,VT.FXConversionMethodOperator
	,VT.FromDeleted
	,VT.ProcessDate
	,VT.OriginalPurchaseDate
	,VT.AccruedInterest
	,'' AS Comment1
	,'' AS Comment2
	,ISNULL(SM.Coupon, 0) As Coupon
	,ISNULL(SM.IssueDate, '1800/01/01') As IssueDate
	,ISNULL(SM.FirstCouponDate, '1800/01/01') As FirstCouponDate
	,ISNULL(SM.CouponFrequencyID, 0) As CouponFrequencyID
	,ISNULL(Sm.AccrualBasisID, 0) As AccrualBasisID
	,ISNULL(SM.BondTypeID, 0) As BondTypeID
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
	,dbo.AdjustBusinessDays(ISNULL(SM.ExpirationDate, 0), 2, VT.AUECID) AS DeliveryDate
	,Sum(ISNULL(VT.SecFee, 0)) AS SecFee
	,Sum(ISNULL(VT.OccFee, 0)) AS OccFee
	,Sum(ISNULL(VT.OrfFee, 0)) AS OrfFee
	,Sum(VT.ClearingBrokerFee) AS ClearingBrokerFee
	,Sum(VT.SoftCommission) AS SoftCommissionCharged
	,VT.TransactionType
	,COALESCE(TC.CurrencySymbol, 'None') AS SettlCurrency
	,VT.ChangeType AS ChangeType
	----------Start of Dynamic UDA---------------        
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
	----------End of Dynamic UDA---------------       
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
	,max(FXRatesForTradeDate.val) AS ForexRate
	,Sum(VT.Commission_BlockLevel) As Commission_BlockLevel
	,Sum(VT.OtherBrokerFees_BlockLevel) As OtherBrokerFees_BlockLevel
	,Sum(VT.StampDuty_BlockLevel) As StampDuty_BlockLevel
	,Sum(VT.TransactionLevy_BlockLevel) As TransactionLevy_BlockLevel
	,Sum(VT.ClearingFee_BlockLevel) As ClearingFee_BlockLevel
	,Sum(VT.TaxOnCommissions_BlockLevel) As TaxOnCommissions_BlockLevel
	,Sum(VT.MiscFees_BlockLevel) As MiscFees_BlockLevel
	,Sum(VT.SecFee_BlockLevel) As SecFee_BlockLevel
	,Sum(VT.OccFee_BlockLevel) As OccFee_BlockLevel
	,Sum(VT.OrfFee_BlockLevel) As OrfFee_BlockLevel
	,Sum(VT.ClearingBrokerFee_BlockLevel) As ClearingBrokerFee_BlockLevel
	,Sum(VT.SoftCommission_BlockLevel) As SoftCommission_BlockLevel
	,Sum(VT.AccruedInterest_BlockLevel) As AccruedInterest_BlockLevel

	,Cast(0 as Float) As OldCommission_BlockLevel
	,Cast(0 as Float) As OldOtherBrokerFees_BlockLevel
	,Cast(0 as Float) As OldStampDuty_BlockLevel
	,Cast(0 as Float) As OldTransactionLevy_BlockLevel
	,Cast(0 as Float) As OldClearingFee_BlockLevel
	,Cast(0 as Float) As OldTaxOnCommissions_BlockLevel
	,Cast(0 as Float) As OldMiscFees_BlockLevel
	,Cast(0 as Float) As OldSecFee_BlockLevel
	,Cast(0 as Float) As OldOccFee_BlockLevel
	,Cast(0 as Float) As OldOrfFee_BlockLevel
	,Cast(0 as Float) As OldClearingBrokerFee_BlockLevel
	,Cast(0 as Float) As OldSoftCommission_BlockLevel
	,Cast(0 as Float) As OldAccruedInterest_BlockLevel
	,Cast(0 as Float) As OldExecutedQuantity_BlockLevel 

InTo #VT_FinalData

FROM #VT_New VT
INNER JOIN T_CompanyFunds F ON F.CompanyFundID = VT.FundID
INNER JOIN #SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol
INNER JOIN T_Asset ON VT.AssetID = T_Asset.AssetID
INNER JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
INNER JOIN T_Currency AS TC ON TC.CurrencyID = VT.SettlCurrency
INNER JOIN T_CounterParty C ON C.CounterPartyID = VT.CounterPartyID
INNER JOIN T_Exchange E ON E.ExchangeID = VT.ExchangeID
INNER JOIN T_Side ON dbo.T_Side.SideTagValue = VT.SideID
INNER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID
INNER JOIN dbo.T_CompanyThirdParty ON T_CompanyThirdParty.CompanyThirdPartyID = CTPM.CompanyThirdPartyID_FK
INNER JOIN dbo.T_ThirdParty ON T_ThirdParty.ThirdPartyId = T_CompanyThirdParty.ThirdPartyId AND T_CompanyThirdParty.CompanyID = @companyID
INNER JOIN dbo.T_ThirdPartyType ON T_ThirdPartyType.ThirdPartyTypeId = T_ThirdParty.ThirdPartyTypeID

LEFT JOIN #TaxlotsDates TD ON TD.TaxlotId = VT.Level2AllocationID
LEFT JOIN T_ThirdPartyFFGenerationDate TGD ON TGD.TaxLotID = VT.Level1AllocationID
	AND TGD.fileFormatID = @fileFormatID AND TGD.thirdPartyID = @thirdPartyID
LEFT JOIN T_CounterParty OldC ON OldC.CounterPartyID = TGD.CounterPartyID
LEFT JOIN T_FundType AS FT ON FT.FundTypeID = CTPM.FundTypeID_FK
LEFT JOIN T_Currency AS OTC ON OTC.CurrencyID = TGD.SettlCurrency
LEFT JOIN T_Side S ON S.SideTagValue = TGD.SideID
--LEFT JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID
LEFT JOIN T_Country ON dbo.T_Country.CountryID = E.Country
LEFT JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue
LEFT JOIN dbo.T_CompanyThirdPartyFlatFileSaveDetails CTPFD ON CTPFD.CompanyThirdPartyID = CTPM.CompanyThirdPartyID_FK
LEFT JOIN T_CounterPartyVenue ON T_CounterPartyVenue.CounterPartyID = VT.CounterPartyID AND T_CounterPartyVenue.VenueID = VT.VenueID
LEFT OUTER JOIN T_CompanyCounterPartyVenues ON T_CompanyCounterPartyVenues.CounterPartyVenueID = T_CounterPartyVenue.CounterPartyVenueID
	AND T_CompanyCounterPartyVenues.CompanyID = @companyID
LEFT OUTER JOIN T_CompanyThirdPartyCVIdentifier ON T_CompanyCounterPartyVenues.CompanyCounterPartyCVID = T_CompanyThirdPartyCVIdentifier.CompanyCounterPartyVenueID_FK
	AND T_CompanyThirdPartyCVIdentifier.CompanyThirdPartyID_FK = @thirdPartyID

LEFT OUTER JOIN #FXConversionRates FXDayRatesForTradeDate ON (
		FXDayRatesForTradeDate.FromCurrencyID = VT.CurrencyID
		AND FXDayRatesForTradeDate.ToCurrencyID = F.LocalCurrency
		AND DateDiff(d, VT.AUECLocalDate, FXDayRatesForTradeDate.DATE) = 0
		AND FXDayRatesForTradeDate.FundID = VT.FundID
		)
LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateTradeDate ON (
		ZeroFundFxRateTradeDate.FromCurrencyID = VT.CurrencyID
		AND ZeroFundFxRateTradeDate.ToCurrencyID = F.LocalCurrency
		AND DateDiff(d, VT.AUECLocalDate, ZeroFundFxRateTradeDate.DATE) = 0
		AND ZeroFundFxRateTradeDate.FundID = 0
		)
CROSS APPLY (
	SELECT CASE 
			WHEN FXDayRatesForTradeDate.RateValue IS NULL
				THEN CASE 
						WHEN ZeroFundFxRateTradeDate.RateValue IS NULL
							THEN 0
						ELSE ZeroFundFxRateTradeDate.RateValue
						END
			ELSE FXDayRatesForTradeDate.RateValue
			END
	) AS FXRatesForTradeDate(Val)
--WHERE (
--		(
--			(VT.TaxLotState <> 1)
--			OR (
--				VT.TaxLotState = 1
--				AND datediff(d, (
--						CASE 
--							WHEN @dateType = 1
--								THEN VT.AUECLocalDate
--							ELSE VT.ProcessDate
--							END
--						), @inputdate) = 0
--				)
--			)
--		AND (
--			VT.TaxLotState <> 4
--			OR (
--				VT.TaxLotState = 4
--				AND datediff(d, (
--						CASE 
--							WHEN @dateType = 1
--								THEN VT.AUECLocalDate
--							ELSE VT.ProcessDate
--							END
--						), @inputdate) = 0
--				)
--			)
--		)
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
	,VT.BenchMarkRate
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


Select 
GroupRefID,
Sum(OldCommission) As OldCommission,
Sum(OldOtherBrokerFees) As OldOtherBrokerFees,
Sum(OldStampDuty) As OldStampDuty,
Sum(OldTransactionLevy) As OldTransactionLevy,
Sum(OldClearingFee) As OldClearingFee,
Sum(OldTaxOnCommissions) As OldTaxOnCommissions,
Sum(OldMiscFees) As OldMiscFees,
Sum(OldSecFee) As OldSecFee,
Sum(OldOccFee) As OldOccFee,
Sum(OldOrfFee) As OldOrfFee,
Sum(OldClearingBrokerFee) As OldClearingBrokerFee,
Sum(OldSoftCommission) As OldSoftCommission,
Sum(OldAccruedInterest) As OldAccruedInterest,
Sum(OldExecutedQuantity) As OldExecutedQuantity
inTo #Temp_GroupedData
From #VT_FinalData VT
Group By GroupRefID


Update VT
Set 
VT.OldCommission_BlockLevel = TG.OldCommission,
VT.OldOtherBrokerFees_BlockLevel = TG.OldOtherBrokerFees,
VT.OldStampDuty_BlockLevel = TG.OldStampDuty,
VT.OldTransactionLevy_BlockLevel = TG.OldTransactionLevy,
VT.OldClearingFee_BlockLevel = TG.OldClearingFee,
VT.OldTaxOnCommissions_BlockLevel = TG.OldTaxOnCommissions,
VT.OldMiscFees_BlockLevel = TG.OldMiscFees,
VT.OldSecFee_BlockLevel = TG.OldSecFee,
VT.OldOccFee_BlockLevel = TG.OldOccFee,
VT.OldOrfFee_BlockLevel = TG.OldOrfFee,
VT.OldClearingBrokerFee_BlockLevel = TG.OldClearingBrokerFee,
VT.OldSoftCommission_BlockLevel = TG.OldSoftCommission,
VT.OldAccruedInterest_BlockLevel = TG.OldAccruedInterest,
VT.OldExecutedQuantity_BlockLevel = TG.OldExecutedQuantity
From #VT_FinalData VT
Inner Join #Temp_GroupedData TG On TG.GroupRefID = VT.GroupRefID


Select *
From #VT_FinalData
ORDER BY GroupRefID

DROP TABLE #VT,#SecMasterData,#TaxlotsDates,#T_TradeAuditAction	,#FXConversionRates	
Drop Table #Temp_DeletedGroupedData,#ZeroFundFxRate,#VT_New,#VT_FinalData,#Temp_GroupedData,#Temp_CounterPartyID