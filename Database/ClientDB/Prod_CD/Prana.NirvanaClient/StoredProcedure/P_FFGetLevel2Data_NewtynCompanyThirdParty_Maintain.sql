CREATE PROCEDURE [dbo].[P_FFGetLevel2Data_NewtynCompanyThirdParty_Maintain] (
	@thirdPartyID INT
	,@companyFundIDs VARCHAR(max)
	,@inputDate DATETIME
	,@companyID INT
	,@auecIDs VARCHAR(max)
	,@TypeID INT
	,-- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                                                                                                                            
	@dateType INT
	,-- 0 means for Process Date and 1 means Trade Date.             
	@fileFormatID INT
	)
AS
DECLARE @Fund TABLE (FundID INT)
DECLARE @AUECID TABLE (AUECID INT)
DECLARE @FXForwardAuecID INT

SET @FXForwardAuecID = (
		SELECT TOP 1 Auecid
		FROM T_AUEC
		WHERE assetid = 11
		)

--Declare  @auecIDs varchar(max)                                                                            
--Set  @auecIDs='1,11,12,15,18,61,62,81'                                                                            
INSERT INTO @Fund
SELECT Cast(Items AS INT)
FROM dbo.Split(@companyFundIDs, ',')

INSERT INTO @AUECID
SELECT Cast(Items AS INT)
FROM dbo.Split(@auecIDs, ',')

CREATE TABLE #VT (
	TaxLotID VARCHAR(50)
	,FundID INT
	,OrderTypeTagValue VARCHAR(10)
	,SideID VARCHAR(10)
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
	,GroupRefID INT
	,Level1AllocationID VARCHAR(50)
	,Level2Percentage FLOAT
	,TaxLotQty FLOAT
	,SettlementDate DATETIME
	,Commission FLOAT
	,OtherBrokerFees FLOAT
	,TaxlotState VARCHAR(50)
	,TaxlotStateID INT
	,StampDuty FLOAT
	,TransactionLevy FLOAT
	,ClearingFee FLOAT
	,TaxOnCommissions FLOAT
	,MiscFees FLOAT
	,AUECLocalDate DATETIME
	,Level2ID INT
	,PBID INT
	,FXRate FLOAT
	,FXConversionMethodOperator VARCHAR(10)
	,FromDeleted VARCHAR(10)
	,ProcessDate DATETIME
	,OriginalPurchaseDate DATETIME
	,AccruedInterest FLOAT
	,FXRate_Taxlot FLOAT
	,FXConversionMethodOperator_Taxlot VARCHAR(10)
	,SideMultiplier INT
	,Description VARCHAR(200)
	,LotID VARCHAR(50)
	,ForexRate FLOAT
	,BenchMarkRate FLOAT
	,Differential FLOAT
	,SwapDescription VARCHAR(500)
	,DayCount INT
	,FirstResetDate DATETIME
	,IsSwapped BIT
	,TransactionType VARCHAR(200)
	)

INSERT INTO #VT
SELECT VT.TaxLotID AS TaxLotID
	,ISNULL(VT.FundID, 0) AS FundID
	,VT.OrderTypeTagValue
	,VT.OrderSideTagValue AS SideID
	,VT.Symbol
	,IsNull(VT.CounterPartyID, 0) AS CounterPartyID
	,IsNull(VT.VenueID, 0) AS VenueID
	,VT.TaxLotQty AS OrderQty
	,VT.AvgPrice
	,VT.CumQty
	,VT.Quantity
	,VT.AUECID
	,VT.AssetID
	,VT.UnderlyingID
	,VT.ExchangeID
	,VT.CurrencyID
	,VT.GroupRefID
	,VT.Level1AllocationID AS Level1AllocationID
	,VT.Level2Percentage
	,VT.TaxLotQty
	,VT.SettlementDate
	,ISNULL(VT.Commission, 0) AS Commission
	,IsNull(VT.OtherBrokerFees, 0) AS OtherBrokerFees
	,0 AS TaxlotState
	,0 AS TaxlotStateID
	,ISNULL(VT.StampDuty, 0) AS StampDuty
	,ISNULL(VT.TransactionLevy, 0) AS TransactionLevy
	,ISNULL(ClearingFee, 0) AS ClearingFee
	,ISNULL(TaxOnCommissions, 0) AS TaxOnCommissions
	,ISNULL(MiscFees, 0) AS MiscFees
	,VT.AUECLocalDate
	,VT.Level2ID AS Level2ID
	,@thirdPartyID
	,IsNull(VT.FXRate, 0) AS FXRate
	,VT.FXConversionMethodOperator
	,'No' AS FromDeleted
	,VT.ProcessDate
	,VT.OriginalPurchaseDate
	,IsNull(VT.AccruedInterest, 0) AS AccruedInterest
	,IsNull(VT.FXRate_Taxlot, 0) AS FXRate_Taxlot
	,VT.FXConversionMethodOperator_Taxlot
	,SideMultiplier
	,VT.Description
	,VT.LotID
	,1
	,ISNULL(VT.BenchMarkRate, 0) AS BenchMarkRate
	,ISNULL(VT.Differential, 0) AS Differential
	,ISNULL(VT.SwapDescription, '') AS SwapDescription
	,ISNULL(VT.DayCount, 0) AS DayCount
	,ISNULL(VT.FirstResetDate, '') AS FirstResetDate
	,VT.IsSwapped
	,VT.TransactionType
FROM V_TaxLots VT
INNER JOIN @Fund Fund ON VT.FundID = Fund.FundID
INNER JOIN @AUECID AUEC ON VT.AUECID = AUEC.AUECID
WHERE datediff(d, (
			CASE 
				WHEN @dateType = 1
					THEN VT.AUECLocalDate
				ELSE VT.ProcessDate
				END
			), @inputdate) = 0

UNION ALL

SELECT TDT.TaxLotID AS TaxLotID
	,ISNULL(TDT.FundID, 0) AS FundID
	,TDT.OrderTypeTagValue
	,TDT.OrderSideTagValue AS SideID
	,TDT.Symbol
	,IsNull(TDT.CounterPartyID, 0) AS CounterPartyID
	,IsNull(TDT.VenueID, 0) AS VenueID
	,TDT.TaxLotQty AS OrderQty
	,TDT.AvgPrice
	,TDT.CumQty
	,TDT.Quantity
	,TDT.AUECID
	,TDT.AssetID
	,TDT.UnderlyingID
	,TDT.ExchangeID
	,TDT.CurrencyID
	,TDT.GroupRefID
	,TDT.Level1AllocationID AS Level1AllocationID
	,TDT.Level2Percentage
	,TDT.TaxLotQty AS AllocatedQty
	,TDT.SettlementDate
	,IsNull(TDT.Commission, 0) AS Commission
	,IsNull(TDT.OtherBrokerFees, 0) AS OtherBrokerFees
	,TDT.TaxLotState
	,ISNULL(TDT.TaxlotState, 0) AS TaxlotStateID
	,ISNULL(TDT.StampDuty, 0) AS StampDuty
	,ISNULL(TDT.TransactionLevy, 0) AS TransactionLevy
	,ISNULL(TDT.ClearingFee, 0) AS ClearingFee
	,ISNULL(TDT.TaxOnCommissions, 0) AS TaxOnCommissions
	,ISNULL(TDT.MiscFees, 0) AS MiscFees
	,TDT.AUECLocalDate
	,0 AS Level2ID
	,TDT.PBID
	,IsNull(TDT.FXRate, 0) AS FXRate
	,TDT.FXConversionMethodOperator AS FXConversionMethodOperator
	,'Yes' AS FromDeleted
	,TDT.ProcessDate
	,TDT.OriginalPurchaseDate
	,IsNull(TDT.AccruedInterest, 0) AS AccruedInterest
	,IsNull(TDT.FXRate_Taxlot, 0) AS FXRate_Taxlot
	,TDT.FXConversionMethodOperator_Taxlot
	,CASE 
		WHEN (
				ORderSideTagValue IN (
					'2'
					,'5'
					,'6'
					,'C'
					,'D'
					)
				)
			THEN - 1
		ELSE 1
		END AS SideMultiplier
	,TDT.Description
	,TDT.LotID
	,1
	,ISNULL(TDT.BenchMarkRate, 0) AS BenchMarkRate
	,ISNULL(TDT.Differential, 0) AS Differential
	,ISNULL(TDT.SwapDescription, '') AS SwapDescription
	,ISNULL(TDT.DayCount, 0) AS DayCount
	,ISNULL(TDT.FirstResetDate, '') AS FirstResetDate
	,TDT.IsSwapped
	,TDT.TransactionType
FROM T_DeletedTaxLots TDT
INNER JOIN @Fund Fund ON TDT.FundID = Fund.FundID
INNER JOIN @AUECID AUEC ON TDT.AUECID = AUEC.AUECID
WHERE datediff(d, (
			CASE 
				WHEN @dateType = 1
					THEN TDT.AUECLocalDate
				ELSE TDT.ProcessDate
				END
			), @inputdate) = 0
	AND TDT.FileFormatID = @fileFormatID

UPDATE #VT
SET VenueID = 1

UPDATE #VT
SET ForexRate = FX.ConversionRate
FROM #VT
INNER JOIN T_CurrencyConversionRate FX ON #VT.CurrencyID = FX.CurrencyPairID_FK
	AND DateDiff(D, #VT.AUECLocalDate, FX.DATE) = 0

UPDATE #VT
SET #VT.TaxlotState = PB.TaxlotState, #VT.TaxlotStateID = PB.TaxlotState
FROM #VT
INNER JOIN T_PBWiseTaxlotState PB ON  #VT.Level1AllocationID = PB.TaxLotID 
WHERE PB.TaxlotState <> 0
	AND PB.PBID = @ThirdPartyID
	AND PB.FileFormatID = @fileFormatID

SELECT *
INTO #VTaxlot
FROM #VT

SELECT VT.TaxLotID
	,T_CompanyFunds.FundShortName AS AccountName
	,T_Side.Side AS Side
	,VT.Symbol
	,T_CounterParty.ShortName AS CounterParty
	,VT.TaxLotQty AS OrderQty
	,--AllocatedQty                                       
	VT.AvgPrice AS AveragePrice
	,VT.CumQty AS ExecutedQty
	,--ExecutedQty                                                                                           
	VT.Quantity AS TotalQty
	,--TotalQty                                                                                                                                                                    
	VT.GroupRefID
	,T_Asset.AssetName AS Asset
	,IsNull(T_Exchange.DisplayName, '') AS Exchange
	,Currency.CurrencySymbol
	,CTPM.MappedName
	,CTPM.FundAccntNo
	,FT.FundTypeName
	,VT.Level1AllocationID AS EntityID
	,VT.Level2Percentage AS Level2Percentage
	,--Percentage,                                                                                                                          
	VT.TaxLotQty AS AllocatedQty
	,SM.PutOrCall
	,IsNull(SM.StrikePrice, 0) AS StrikePrice
	,convert(VARCHAR, SM.ExpirationDate, 101) AS ExpirationDate
	,convert(VARCHAR, VT.SettlementDate, 101) AS SettlementDate
	,VT.Commission AS CommissionCharged
	,VT.OtherBrokerFees AS OtherBrokerFee
	,T_ThirdPartyType.ThirdPartyTypeName
	,0 AS SecFee
	,ISNULL(T_CounterPartyVenue.DisplayName, '') AS CVName
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
	,ISNULL(VT.TaxlotStateID, 0) AS TaxlotStateID
	,VT.FromDeleted
	,ISNULL(VT.StampDuty, 0) AS StampDuty
	,ISNULL(VT.TransactionLevy, 0) AS TransactionLevy
	,ISNULL(VT.ClearingFee, 0) AS ClearingFee
	,ISNULL(VT.TaxOnCommissions, 0) AS TaxOnCommissions
	,ISNULL(VT.MiscFees, 0) AS MiscFees
	,convert(VARCHAR, VT.AUECLocalDate, 101) AS TradeDate
	,ISNULL(SM.Multiplier, 1) AS AssetMultiplier
	,CASE 
		WHEN VT.AssetID = 8
			THEN Cast(Cast(((VT.AvgPrice * VT.TaxlotQty * 0.01) + ((VT.Commission + VT.OtherBrokerFees + VT.TransactionLevy + VT.StampDuty + VT.ClearingFee + VT.TaxOnCommissions + VT.MiscFees) * 0.01)) AS DECIMAL(32, 2)) AS VARCHAR(500))
		ELSE Cast(Cast(((VT.AvgPrice * VT.TaxlotQty * SM.Multiplier) + ((VT.Commission + VT.OtherBrokerFees + VT.TransactionLevy + VT.StampDuty + VT.ClearingFee + VT.TaxOnCommissions + VT.MiscFees) * VT.SideMultiplier)) AS DECIMAL(32, 2)) AS VARCHAR(500))
		END AS NetAmount
	,VT.Level2ID
	,SM.ISINSymbol
	,SM.CUSIPSymbol
	,SM.SEDOLSymbol
	,SM.ReutersSymbol
	,SM.BloombergSymbol AS BBCode
	,SM.CompanyName
	,SM.UnderlyingSymbol
	,SM.LeadCurrency
	,SM.VsCurrency
	,SM.OSISymbol AS OSIOptionSymbol
	,SM.IDCOSymbol AS IDCOOptionSymbol
	,SM.OpraSymbol AS OpraOptionSymbol
	,IsNull(VT.FXRate, 0) AS FXRate
	,VT.FXConversionMethodOperator
	,VT.ProcessDate
	,VT.OriginalPurchaseDate
	,IsNull(VT.AccruedInterest, 0) AS AccruedInterest
	,T_Country.CountryName AS CountryName
	,IsNull(VT.FXRate_Taxlot, 0) AS FXRate_Taxlot
	,VT.FXConversionMethodOperator_Taxlot
	,ISNULL(SM.AssetName, '') AS UDAAssetName
	,ISNULL(SM.SecurityTypeName, '') AS UDASecurityTypeName
	,ISNULL(SM.SectorName, '') AS UDASectorName
	,ISNULL(SM.SubSectorName, '') AS UDASubSectorName
	,ISNULL(SM.CountryName, '') AS UDACountryName
	,VT.Description
	,ISNULL(T_ClosingAlgos.AlgorithmAcronym, '') AS ClosingAlgo
	,CASE 
		WHEN Closing.ClosedQty IS NULL
			THEN 0
		ELSE Closing.ClosedQty
		END AS ClosedQty
	,ISNULL(Closing.OpenPrice, 0) AS OpenPriceAgainstClosing
	,convert(VARCHAR, #VTaxlot.AUECLocalDate, 101) AS TradeDateAgainstClosing
	,#VTaxlot.LotID AS LotIDAgainstClosing
	,VT.Level1AllocationID
	,VT.LotID
	,CTPM.FundAccntNo AS FundAccountNo
	,ISNULL(VT.BenchMarkRate, 0) AS BenchMarkRate
	,ISNULL(VT.Differential, 0) AS Differential
	,ISNULL(VT.SwapDescription, '') AS SwapDescription
	,ISNULL(VT.DayCount, 0) AS DayCount
	,ISNULL(VT.FirstResetDate, '') AS FirstResetDate
	,VT.IsSwapped
	,VT.TransactionType
FROM #VT VT
INNER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID
INNER JOIN T_FundType AS FT ON FT.FundTypeID = CTPM.FundTypeID_FK
LEFT OUTER JOIN PM_TaxLotClosing Closing ON Closing.ClosingTaxLotID = VT.TaxLotID
LEFT OUTER JOIN T_ClosingAlgos ON T_ClosingAlgos.AlgorithmId = Closing.ClosingAlgo
LEFT OUTER JOIN #VTaxlot ON Closing.PositionalTaxLotID = #VTaxlot.TaxLotID
LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
LEFT JOIN T_Side ON dbo.T_Side.SideTagValue = VT.SideID
LEFT JOIN T_CompanyFunds ON T_CompanyFunds.CompanyFundID = VT.FundID
LEFT JOIN T_Asset ON T_Asset.AssetID = VT.AssetID
LEFT JOIN T_CounterParty ON T_CounterParty.CounterPartyID = VT.CounterPartyID
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
LEFT OUTER JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol
WHERE CTPM.InternalFundNameID_FK IN (
		SELECT FundID
		FROM @Fund
		)
ORDER BY TaxlotId

DROP TABLE #VT
	,#VTaxlot