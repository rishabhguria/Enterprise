/*                                                                                                                                                          
Select * from T_ThirdParty                                                                                                                                                                       
                                                                                                                                          
Select * from V_TaxLots                                                                                                                                          
P_FFGetThirdPartyFundsDetailsExportOnly 26,1184,'8/11/2008 12:00:00 AM',5                                                                                                                                           
Modified by : <Sandeep Singh>                                                                                                                                                          
Date : <08-09-2008>                                                                        
[P_FFGetThirdPartyFundsDetails] 27,'1184,1183,1182,1186,1185','7/25/2012 3:51:24 PM',5,'1,20,21,18,1,15,12,16,33, 80',0,0,10                                                      
                                                    
Modified by : <Sandeep Singh>                                                                                                                                                          
Date : <08-19-2012>                                                       
DESC: FX Rate and FX Conversion Method Operator at Taxlot Level added                             
                          
Modified by : <Pooja Porwal>                                                                                                                                                          
Date : <DEC-08-2014>                                                       
DESC: FIX Tag updated for all data parties and executing broker                          
JIRA: http://jira.nirvanasolutions.com:8080/browse/PRANA-5521                         
                        
Modified By : Suraj Nataraj                        
Date : 07-02-2015                        
DESC : Add Dynamic UDA Fields                        
JIRA : http://jira.nirvanasolutions.com:8080/browse/PRANA-9129                        
                
Modified By : Sunil Sharma                        
Date : 10-11-2016                        
DESC : Optimization and Some extra redundant Code has been removed from ThirdParty Sp's                        
JIRA : https://jira.nirvanasolutions.com:8443/browse/PRANA-19454                 
                
Modified By : Sagar Tagra                
Date : 10-25-2016                        
DESC : Changed Multiplier (INT) to Multiplier (Float)                      
JIRA : https://jira.nirvanasolutions.com:8443/browse/PRANA-19454                 
                                                                                                                                                                               
*/
CREATE PROCEDURE [dbo].[P_FFGetThirdPartyFundsDetails_Markston_SideGroup] (
	@thirdPartyID INT
	,@companyFundIDs VARCHAR(max)
	,@inputDate DATETIME
	,@companyID INT
	,@auecIDs VARCHAR(max)
	,@TypeID INT
	,-- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                            
	@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                            
	,@fileFormatID INT
	,@includeSent INT = 0
	)
AS
--DECLARE @thirdPartyID INT
--	,@companyFundIDs VARCHAR(max)
--	,@inputDate DATETIME
--	,@companyID INT
--	,@auecIDs VARCHAR(max)
--	,@TypeID INT
--	,-- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                            
--	@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                            
--	,@fileFormatID INT
--	,@includeSent INT = 0
--SET  @thirdPartyID=27
--SET @companyFundIDs=N'1184,1190,1183,1182'
--SET @inputDate='2019-07-08 12:32:38'
--SET @companyID=5
--SET @auecIDs=N'1,15,12'
--SET @TypeID=0
--SET @dateType=0
--SET @fileFormatID=54
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
FROM V_TaxLots VT
INNER JOIN @Fund Fund ON Fund.FundID = VT.FundID
INNER JOIN #SecMasterData SM ON VT.Symbol = SM.TickerSymbol
INNER JOIN @AUECID auec ON auec.AUECID = VT.AUECID
WHERE datediff(d, (
			CASE 
				WHEN @dateType = 1
					THEN VT.AUECLocalDate
				ELSE VT.ProcessDate
				END
			), @inputdate) = 0
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
	,'3' AS ChangeType
FROM T_DeletedTaxLots TDT
INNER JOIN @Fund Fund ON Fund.FundID = TDT.FundID
INNER JOIN @AUECID auec ON auec.AUECID = TDT.AUECID
WHERE datediff(d, (
			CASE 
				WHEN @dateType = 1
					THEN TDT.AUECLocalDate
				ELSE TDT.ProcessDate
				END
			), @inputdate) = 0
	AND (
		FileFormatID = @fileFormatID
		OR FileFormatID = 0
		)
	AND TDT.TaxLotState = 3

UPDATE #VT
SET #VT.TaxlotState = PB.TaxlotState
FROM #VT
INNER JOIN T_PBWiseTaxlotState PB  with (nolock) ON PB.TaxlotID = #VT.TaxlotID
WHERE PB.TaxlotState <> 0
	AND PB.FileFormatID = @fileFormatID

SELECT Max(VT.TaxlotID) AS TaxlotID
	,Max(ISNULL(VT.FundID, 0)) AS FundID
	,Max(IsNull(F.FundName, '')) AS AccountName
	,Max(ISNULL(T_OrderType.OrderTypesID, 0)) AS OrderTypesID
	,Max(ISNULL(T_OrderType.OrderTypes, 'Multiple')) AS OrderTypes
	,Max(VT.SideID) AS SideID
	,T_Side.Side AS Side
	,VT.Symbol
	,Max(VT.CounterPartyID) AS CounterPartyID
	,Max(CT.ShortName) AS Counterparty
	,Sum(VT.TaxLotQty) AS OrderQty
	,Avg(VT.AvgPrice) AS AvgPrice
	,Sum(VT.CumQty) AS CumQty
	,Avg(VT.Quantity) AS Quantity
	,Max(VT.AUECID) AS AUECID
	,Max(VT.AssetID) AS AssetID
	,Max(TA.AssetName) AS Asset
	,Max(VT.UnderlyingID) AS UnderlyingID
	,Max(VT.ExchangeID) AS ExchangeID
	,Max(Currency.CurrencyID) AS CurrencyID
	,Max(Currency.CurrencyName) AS CurrencyName
	,Max(Currency.CurrencySymbol) AS CurrencySymbol
	,Max(CTPM.MappedName) AS MappedName
	,Max(CTPM.FundAccntNo) AS FundAccntNo
	,Max(CTPM.FundTypeID_FK) AS FundTypeID_FK
	,Max(FT.FundTypeName) AS FundTypeName
	,Max(VT.Level1AllocationID) AS Level1AllocationID
	,Sum(VT.TaxLotQty) AS TaxlotQty
	,Max(ISNULL(SM.PutOrCall, '')) AS PutOrCall
	,Sum(SM.StrikePrice) AS StrikePrice
	,Max(Convert(VARCHAR(10), SM.ExpirationDate, 101)) AS ExpirationDate
	,Max(Convert(VARCHAR(10), VT.SettlementDate, 101)) AS SettlementDate
	,Sum(VT.Commission) AS Commission
	,Sum(VT.OtherBrokerFees) AS OtherBrokerFees
	,Max(VT.GroupRefID) AS GroupRefID
	,Max(VT.TaxLotState) AS TaxLotState
	,Sum(ISNULL(VT.StampDuty, 0)) AS StampDuty
	,Sum(ISNULL(VT.TransactionLevy, 0)) AS TransactionLevy
	,Sum(ISNULL(ClearingFee, 0)) AS ClearingFee
	,Sum(ISNULL(TaxOnCommissions, 0)) AS TaxOnCommissions
	,Sum(ISNULL(MiscFees, 0)) AS MiscFees
	,Max(Convert(VARCHAR(10), VT.AUECLocalDate, 101)) AS TradeDate
	,Max(SM.Multiplier) AS Multiplier
	,0 AS Level2ID
	,Max(SM.ISINSymbol) AS ISINSymbol
	,Max(SM.CUSIPSymbol) AS CUSIPSymbol
	,Max(SM.SEDOLSymbol) AS SEDOLSymbol
	,Max(SM.ReutersSymbol) AS ReutersSymbol
	,Max(SM.BloombergSymbol) AS BloombergSymbol
	,Max(SM.CompanyName) AS CompanyName
	,Max(SM.UnderlyingSymbol) AS UnderlyingSymbol
	,Max(SM.LeadCurrencyID) AS LeadCurrencyID
	,Max(SM.LeadCurrency) AS LeadCurrency
	,Max(SM.VsCurrencyID) AS VsCurrencyID
	,Max(SM.VsCurrency) AS VsCurrency
	,Max(SM.OSISymbol) AS OSISymbol
	,Max(SM.IDCOSymbol) AS IDCOSymbol
	,max(SM.OpraSymbol) AS OpraSymbol
	,Max(VT.FXRate) AS FXRate
	,Max(VT.FXConversionMethodOperator) AS FXConversionMethodOperator
	,Max(VT.FromDeleted) AS FromDeleted
	,Max(VT.ProcessDate) AS ProcessDate
	,Max(VT.OriginalPurchaseDate) AS OriginalPurchaseDate
	,Sum(VT.AccruedInterest) AS AccruedInterest
	,Sum(ISNULL(VT.BenchMarkRate, 0)) AS BenchMarkRate
	,Sum(ISNULL(VT.Differential, 0)) AS Differential
	,Max(VT.SwapDescription) AS SwapDescription
	,Sum(ISNULL(VT.DayCount, 0)) AS DayCount
	,max(ISNULL(VT.FirstResetDate, '')) AS FirstResetDate
	,Max(CAST(VT.IsSwapped AS INT)) AS IsSwapped
	,Max(T_Country.CountryName) AS CountryName
	,Max(VT.FXRate_Taxlot) AS FXRate_Taxlot
	,Max(VT.FXConversionMethodOperator_Taxlot) AS FXConversionMethodOperator_Taxlot
	,max(VT.TradeAttribute1) AS TradeAttribute1
	,Max(VT.TradeAttribute2) AS TradeAttribute2
	,Max(VT.TradeAttribute3) AS TradeAttribute3
	,Max(VT.TradeAttribute4) AS TradeAttribute4
	,Max(VT.TradeAttribute5) AS TradeAttribute5
	,Max(VT.TradeAttribute6) AS TradeAttribute6
	,Max(VT.Description) AS Description
	,Sum(ISNULL(VT.SecFee, 0)) AS SecFee
	,Sum(ISNULL(VT.OccFee, 0)) AS OccFee
	,Sum(ISNULL(VT.OrfFee, 0)) AS OrfFee
	,Sum(VT.ClearingBrokerFee) AS ClearingBrokerFee
	,Sum(VT.SoftCommission) AS SoftCommission
	,Max(VT.TransactionType) AS TransactionType
	,Max(COALESCE(TC.CurrencySymbol, 'None')) AS SettlCurrency
	,Max(SM.Coupon) AS Coupon
FROM #VT VT
LEFT OUTER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID
INNER JOIN T_FundType AS FT ON FT.FundTypeID = CTPM.FundTypeID_FK
LEFT JOIN T_CompanyFunds F ON F.CompanyFundID = VT.FundID
LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
LEFT JOIN T_Currency AS TC ON TC.CurrencyID = VT.SettlCurrency
LEFT JOIN T_Side ON dbo.T_Side.SideTagValue = VT.SideID
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
LEFT OUTER JOIN T_Counterparty AS CT ON CT.CounterPartyID = VT.CounterpartyID
LEFT OUTER JOIN T_Asset AS TA ON TA.AssetID = VT.AssetID
WHERE F.FundName IN (
		'315'
		,'316'
		,'318'
		,'318lcv'
		,'319'
		,'319lcv'
		)
	AND (
		(
			CTPM.InternalFundNameID_FK IN (
				SELECT FundID
				FROM @Fund
				)
			AND @TypeID = 0 -- Changes made for Prime Broker [@TypeId = 0], Added By Sunil Sharma                
			)
		OR (
			VT.FundID IN (
				SELECT FundID
				FROM @Fund
				)
			AND @TypeID = 1 -- Changes made for All Data Parties/Executing Broker [@TypeId = 1], Added By Sunil Sharma                
			)
		)
	AND (
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
	AND VT.TaxLotState NOT IN (
		CASE @includeSent
			WHEN 0
				THEN 1
			ELSE - 1
			END
		)
GROUP BY T_Side.Side
	,VT.Symbol
	,VT.CounterPartyID

DROP TABLE #VT
	,#SecMasterData
