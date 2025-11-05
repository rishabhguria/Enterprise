 	Create PROCEDURE [dbo].[P_GetThirdPartyDetaiWithUnderlying_EOD_HelmCapital] (
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
	,UnderlyingISINSymbol VARCHAR(20)
	,UnderlyingCUSIPSymbol VARCHAR(50)
	,UnderlyingSEDOLSymbol VARCHAR(50)
	,UnderlyingCurrencySymbol VARCHAR(50)
	
	)

INSERT INTO #SecMasterData
SELECT SM.TickerSymbol
	,SM.PutOrCall
	,SM.StrikePrice
	,SM.ExpirationDate
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
	,SM.Coupon
	,SM.IssueDate
	,SM.FirstCouponDate
	,SM.CouponFrequencyID
	,SM.AccrualBasisID
	,SM.BondTypeID
	,SM.AssetName
	,SM.SecurityTypeName
	,SM.SectorName
	,SM.SubSectorName
	,SM.CountryName
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
	,USM.ISINSymbol
	,USM.CUSIPSymbol
	,USM.SEDOLSymbol
	,UCT.CurrencySymbol
	
FROM V_SecMasterData SM
LEFT OUTER JOIN V_UDA_DynamicUDA UDA ON UDA.Symbol_PK = SM.Symbol_PK
--LEFT OUTER JOIN V_SecMasterData_WithUnderlying USM ON USM.TickerSymbol = SM.UnderLyingSymbol
LEFT OUTER JOIN [HelmCapitalSM].dbo.T_SMSymbolLookUpTable USM ON SM.UnderlyingSymbol = USM.TickerSymbol
INNER JOIN T_Currency UCT on UCT.CurrencyID=USM.CurrencyID


CREATE TABLE #VT (
	TaxLotID VARCHAR(50)
	,FundID INT
	,OrderTypeTagValue VARCHAR(3)
	,SideID VARCHAR(3)
	,Symbol VARCHAR(100)
	,CounterPartyID INT
	--,VenueID INT
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
	--,VT.VenueID
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
	,PB.TaxLotState AS TaxlotState
	,ISNULL(VT.StampDuty, 0) AS StampDuty
	,ISNULL(VT.TransactionLevy, 0) AS TransactionLevy
	,ISNULL(ClearingFee, 0) AS ClearingFee
	,ISNULL(TaxOnCommissions, 0) AS TaxOnCommissions
	,ISNULL(MiscFees, 0) AS MiscFees
	,VT.AUECLocalDate
	,0 AS Level2ID
	,PB.PBID
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
INNER JOIN T_PBWiseTaxlotState PB ON PB.TaxlotID = VT.TaxlotID
INNER JOIN #SecMasterData SM ON VT.Symbol = SM.TickerSymbol
WHERE (
		PB.fileFormatID = 0
		OR @fileFormatID = FileFormatID
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
	AND PB.TaxLotState NOT IN (
		CASE @includeSent
			WHEN 0
				THEN 1
			ELSE - 1
			END
		)

INSERT INTO #VT
SELECT TDT.Level1AllocationID AS TaxlotID
	,ISNULL(TDT.FundID, 0) AS FundID
	,TDT.OrderTypeTagValue
	,TDT.OrderSideTagValue AS SideID
	,TDT.Symbol
	,TDT.CounterPartyID
--	,TDT.VenueID
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
FROM T_DeletedTaxLots TDT
INNER JOIN #SecMasterData SM ON SM.TickerSymbol = TDT.Symbol
WHERE (
		FileFormatID = @fileFormatID
		OR FileFormatID = 0
		)
	AND TDT.TaxLotState = 3

SELECT VT.TaxlotID AS TaxlotID
     ,T_CompanyFunds.FundShortName AS AccountName
	 ,T_CounterParty.ShortName AS CounterParty  
	,ISNULL(VT.FundID, 0) AS FundID
	,ISNULL(T_OrderType.OrderTypesID, 0) AS OrderTypesID
	,ISNULL(T_OrderType.OrderTypes, 'Multiple') AS OrderTypes
	,VT.SideID
	,T_Side.Side AS Side
	,VT.Symbol
	,VT.CounterPartyID
	
	--,VT.VenueID
	,Sum(VT.TaxLotQty) AS OrderQty
	,VT.AvgPrice
	,VT.CumQty AS ExecutedQty
	,VT.Quantity
	,VT.AUECID
	,VT.AssetID
	,VT.UnderlyingID
	,VT.ExchangeID
	,Currency.CurrencyID
	,Currency.CurrencyName
	,Currency.CurrencySymbol
	
	,VT.Level1AllocationID AS EntityID
	,Sum(VT.Level2Percentage) AS Level2Percentage
	,Sum(VT.TaxLotQty)
	,'' AS IsBasketGroup
	,SM.PutOrCall
	,SM.StrikePrice
	,ISNULL(CONVERT(datetime,SM.ExpirationDate,101),'') AS ExpirationDate
	
	,VT.SettlementDate
	,Sum(ISNULL(VT.Commission,0)) as CommissionCharged
	,Sum(ISNULL(VT.OtherBrokerFees,0)) as OtherBrokerFees
	
--	,0 AS SecFee
	,T_Asset.AssetName AS Asset
	,VT.GroupRefID
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
	,Sum(ISNULL(ClearingFee, 0)) AS ClearingFee
	,Sum(ISNULL(TaxOnCommissions, 0)) AS TaxOnCommissions
	,Sum(ISNULL(MiscFees, 0)) AS MiscFees
	,convert(VARCHAR, VT.AUECLocalDate, 101) AS TradeDate
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
		--Swap Parameters                         
	--VT.BenchMarkRate
	--,VT.Differential
	,VT.SwapDescription
	,ISNULL(VT.DayCount, '') AS DayCount
	,ISNULL(CONVERT(datetime,VT.FirstResetDate,101),'') AS FirstResetDate
	,VT.IsSwapped
	,T_Country.CountryName AS CountryName
	
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
	,Sum(ISNULL(VT.ClearingBrokerFee,0)) AS ClearingBrokerFee
	,Sum(ISNULL(VT.SoftCommission,0)) As SoftCommissionCharged
	
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
	,SM.UnderlyingCUSIPSymbol
	,SM.UnderlyingISINSymbol
	,SM.UnderlyingSEDOLSymbol
	,SM.UnderlyingCurrencySymbol

FROM #VT VT


LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
LEFT JOIN T_Currency AS TC ON TC.CurrencyID = VT.SettlCurrency
LEFT JOIN T_Side ON dbo.T_Side.SideTagValue = VT.SideID
LEFT JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID
LEFT JOIN T_Country ON dbo.T_Country.CountryID = T_Exchange.Country
LEFT JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue
LEFT JOIN T_CompanyFunds ON T_CompanyFunds.CompanyFundID = VT.FundID  
LEFT JOIN T_Asset ON T_Asset.AssetID = VT.AssetID  
LEFT JOIN T_CounterParty ON T_CounterParty.CounterPartyID = VT.CounterPartyID 

LEFT OUTER JOIN #SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol

WHERE datediff(d, (
			CASE 
				WHEN @dateType = 1
					THEN VT.AUECLocalDate
				ELSE VT.ProcessDate
				END
			), @inputdate) >= 0
	
	AND (
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
	AND VT.AUECID IN (
		SELECT AUECID
		FROM @AUECID
		)
	AND VT.PBID = @thirdPartyID 
	--and vt.symbol='O:AGI 21A12.50D15'
GROUP BY VT.TaxlotID
    ,T_CompanyFunds.FundShortName
    ,T_CounterParty.ShortName
	,VT.Level1AllocationID
	,VT.FundID
	,T_OrderType.OrderTypesID
	,T_OrderType.OrderTypes
	,VT.SideID
	,T_Side.Side
	,VT.Symbol
	,VT.CounterPartyID
	--,VT.VenueID
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
	
	,SM.PutOrCall
	,SM.StrikePrice
	,SM.ExpirationDate
	,VT.SettlementDate
	,T_Asset.AssetName
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
	
	--Swap Parameters                                  
	--VT.BenchMarkRate
	--,VT.Differential
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
	,SM.UnderlyingCUSIPSymbol
	,SM.UnderlyingISINSymbol
	,SM.UnderlyingSEDOLSymbol
	,SM.UnderlyingCurrencySymbol
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
ORDER BY GroupRefID

DROP TABLE #VT
	,#SecMasterData