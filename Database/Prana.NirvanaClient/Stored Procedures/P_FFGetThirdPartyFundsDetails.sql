CREATE PROCEDURE [dbo].[P_FFGetThirdPartyFundsDetails] (
	@thirdPartyID INT
	,@companyFundIDs VARCHAR(max)
	,@inputDate DATETIME
	,@companyID INT
	,@auecIDs VARCHAR(max)
	,@TypeID INT
	,-- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                            
	@dateType INT -- 0 for Process Date and 1 for Trade Date.                                                                                                                                                            
	,@fileFormatID INT
	,@includeSent INT = 0
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
--declare @includeSent INT

--set @thirdPartyID=27
--set @companyFundIDs=N'1182,1183,1184,1190,1185,1186,1186,1185,1189,'
--set @inputDate='2019-06-24 16:08:16'
--set @companyID=5
--set @auecIDs=N'1,15,11,'
--set @TypeID=1
--set @dateType=1
--set @fileFormatID=145
--set @includeSent=1

Declare @CounterPartyId Int

Set @CounterPartyId = 
(
Select CounterPartyID From T_ThirdParty
Where @TypeID <> 0 And ThirdPartyTypeID = 3 And ThirdPartyID = @thirdPartyID 
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
	,StrikePrice FLOAT
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
	,OSISymbol VARCHAR(25)
	,IDCOSymbol VARCHAR(25)
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
	,FundID INT
	,OrderTypeTagValue VARCHAR(3)
	,SideID VARCHAR(3)
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
	, TaxLotID_1 VARCHAR(50)
	)

INSERT INTO #VT
SELECT VT.Level1AllocationID AS TaxlotID
	,ISNULL(VT.FundID, 0) AS FundID
	,VT.OrderTypeTagValue
	,VT.OrderSideTagValue AS SideID
	,VT.Symbol
	,VT.CounterPartyID
	,VT.VenueID
	,(VT.TaxLotQty) AS OrderQty
	,VT.AvgPrice
	,VT.CumQty
	,VT.Quantity
	,VT.AUECID
	,VT.AssetID
	,VT.UnderlyingID
	,VT.ExchangeID
	,VT.CurrencyID
	,VT.Level1AllocationID AS Level1AllocationID
	,(VT.Level2Percentage)
	,(VT.TaxLotQty)
	,'' AS IsBasketGroup
	,VT.SettlementDate
	,VT.Commission
	,VT.OtherBrokerFees
	,VT.GroupRefID
	,0 AS TaxlotState
	,ISNULL(VT.StampDuty, 0) AS StampDuty
	,ISNULL(VT.TransactionLevy, 0) AS TransactionLevy
	,ISNULL(VT.ClearingFee, 0) AS ClearingFee
	,ISNULL(VT.TaxOnCommissions, 0) AS TaxOnCommissions
	,ISNULL(VT.MiscFees, 0) AS MiscFees
	,VT.AUECLocalDate
	,0 AS Level2ID
	,@thirdPartyID AS PBID
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
	,VT.TaxLotID As TaxLotID_1
FROM V_TaxLots VT
INNER JOIN @Fund Fund ON Fund.FundID = VT.FundID
Inner Join #Temp_CounterPartyID CP On CP.CounterPartyID = VT.CounterPartyID
INNER JOIN #SecMasterData SM ON VT.Symbol = SM.TickerSymbol
INNER JOIN @AUECID auec ON auec.AUECID = VT.AUECID
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
			AND VT.TaxlotClosingID_FK IS NOT NULL
			AND VT.AssetID IN (
				1
				,3
				)
			)
		OR (
			@IncludeCATransaction = 1
			AND (
				VT.TransactionSource IN (
					6
					,7
					,8
					,9
					,10
					,11
					)
				)
			)
		OR VT.TransactionSource = 13
		)

INSERT INTO #VT
SELECT TDT.Level1AllocationID AS TaxlotID
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
	,3 AS ChangeType
	,TDT.TaxLotID As TaxLotID_1
FROM T_DeletedTaxLots TDT
INNER JOIN @Fund Fund ON Fund.FundID = TDT.FundID
INNER JOIN @AUECID auec ON auec.AUECID = TDT.AUECID
Inner Join #Temp_CounterPartyID CP On CP.CounterPartyID = TDT.CounterPartyID
WHERE datediff(d, (
			CASE 
				WHEN @dateType = 1
					THEN TDT.AUECLocalDate
				ELSE TDT.ProcessDate
				END
			), @inputdate) >= 0
AND (FileFormatID = @fileFormatID)
	AND TDT.TaxLotState = 3

UPDATE #VT
SET #VT.TaxlotState = PB.TaxlotState
FROM #VT
INNER JOIN T_PBWiseTaxlotState PB with (nolock) ON (PB.TaxlotID = #VT.TaxLotID_1)
WHERE PB.TaxlotState <> 0
	AND PB.FileFormatID = @fileFormatID

SELECT VT.TaxlotID AS TaxlotID
	,ISNULL(VT.FundID, 0) AS FundID
	,ISNULL(T_OrderType.OrderTypesID, 0) AS OrderTypesID
	,ISNULL(T_OrderType.OrderTypes, 'Multiple') AS OrderTypes
	,VT.SideID
	,T_Side.Side AS Side
	,VT.Symbol
	,VT.CounterPartyID
	,VT.VenueID
	,Sum(VT.TaxLotQty) AS OrderQty
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
	,VT.Level1AllocationID AS Level1AllocationID
	,Sum(VT.Level2Percentage) AS Level2Percentage
	,Sum(VT.TaxLotQty)
	,'' AS IsBasketGroup
	,SM.PutOrCall
	,SM.StrikePrice
	,SM.ExpirationDate
	,VT.SettlementDate
	,Sum(VT.Commission)
	,Sum(VT.OtherBrokerFees)
	-- Added this code to remove the extra code for All Data Parties[ Here TypeID = 0 for Prime Broker] : By Sunil Sharma
	,CASE 
		WHEN (@TypeID = 0)
			THEN T_ThirdPartyType.ThirdPartyTypeID
		ELSE 0
		END AS ThirdPartyTypeID
	,CASE 
		WHEN @TypeID = 0
			THEN T_ThirdPartyType.ThirdPartyTypeName
		ELSE ''
		END AS ThirdPartyTypeName
	,CASE 
		WHEN @TypeID = 0
			THEN CTPFD.CompanyIdentifier
		ELSE ''
		END AS CompanyIdentifier
	,0 AS SecFee
	,CASE 
		WHEN @TypeID = 0
			THEN ISNULL(T_CounterPartyVenue.DisplayName, '')
		ELSE ''
		END AS CVName
	,CASE 
		WHEN @TypeID = 0
			THEN ISNULL(T_CompanyThirdPartyCVIdentifier.CVIdentifier, '')
		ELSE ''
		END AS CVIdentifier
	,CASE 
		WHEN @TypeID = 0
			THEN T_CompanyCounterPartyVenues.CompanyCounterPartyCVID
		ELSE 0
		END AS CompanyCounterPartyCVID
	,VT.GroupRefID
	,VT.TaxLotState
	,Sum(ISNULL(VT.StampDuty, 0)) AS StampDuty
	,Sum(ISNULL(VT.TransactionLevy, 0)) AS TransactionLevy
	,Sum(ISNULL(ClearingFee, 0)) AS ClearingFee
	,Sum(ISNULL(TaxOnCommissions, 0)) AS TaxOnCommissions
	,Sum(ISNULL(MiscFees, 0)) AS MiscFees
	,VT.AUECLocalDate
	,SM.Multiplier
	,0 AS Level2ID
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
	,
	--Swap Parameters                         
	VT.BenchMarkRate
	,VT.Differential
	,VT.SwapDescription
	,VT.DayCount
	,VT.FirstResetDate
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
	,Sum(VT.ClearingBrokerFee)
	,Sum(VT.SoftCommission)
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
	,VWAP
----------End of Dynamic UDA---------------        
FROM #VT VT
Inner JOIN #SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol
LEFT OUTER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID
INNER JOIN T_FundType AS FT ON FT.FundTypeID = CTPM.FundTypeID_FK
INNER JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
INNER JOIN T_Currency AS TC ON TC.CurrencyID = VT.SettlCurrency
INNER JOIN T_Side ON dbo.T_Side.SideTagValue = VT.SideID
INNER JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID
INNER JOIN dbo.T_CompanyThirdParty ON T_CompanyThirdParty.CompanyThirdPartyID = CTPM.CompanyThirdPartyID_FK
INNER JOIN dbo.T_ThirdParty ON T_ThirdParty.ThirdPartyId = T_CompanyThirdParty.ThirdPartyId AND T_CompanyThirdParty.CompanyID = @companyID
INNER JOIN dbo.T_ThirdPartyType ON T_ThirdPartyType.ThirdPartyTypeId = T_ThirdParty.ThirdPartyTypeID
LEFT JOIN T_Country ON dbo.T_Country.CountryID = T_Exchange.Country
LEFT JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue
LEFT JOIN dbo.T_CompanyThirdPartyFlatFileSaveDetails CTPFD ON CTPFD.CompanyThirdPartyID = CTPM.CompanyThirdPartyID_FK
LEFT JOIN T_CounterPartyVenue ON T_CounterPartyVenue.CounterPartyID = VT.CounterPartyID AND T_CounterPartyVenue.VenueID = VT.VenueID
LEFT OUTER JOIN T_CompanyCounterPartyVenues ON T_CompanyCounterPartyVenues.CounterPartyVenueID = T_CounterPartyVenue.CounterPartyVenueID
	AND T_CompanyCounterPartyVenues.CompanyID = @companyID
LEFT OUTER JOIN T_CompanyThirdPartyCVIdentifier ON T_CompanyCounterPartyVenues.CompanyCounterPartyCVID = T_CompanyThirdPartyCVIdentifier.CompanyCounterPartyVenueID_FK
	AND T_CompanyThirdPartyCVIdentifier.CompanyThirdPartyID_FK = @thirdPartyID
LEFT OUTER JOIN PM_DailyVWAP AS DV ON VT.Symbol = DV.Symbol AND DATEDIFF(d,VT.AUECLocalDate,DV.Date) = 0 AND VWAP != 0
WHERE (
		(
			VT.TaxLotState <> 1
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
	AND VT.TaxLotState NOT IN (
		CASE @includeSent
			WHEN 0
				THEN 1
			ELSE - 1
			END
		)
GROUP BY VT.TaxlotID
	,VT.Level1AllocationID
	,VT.FundID
	,T_OrderType.OrderTypesID
	,T_OrderType.OrderTypes
	,VT.SideID
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
	,VWAP
--------------Dynamic UDA-----------------        
ORDER BY GroupRefID

DROP TABLE #VT,#SecMasterData,#Temp_CounterPartyID