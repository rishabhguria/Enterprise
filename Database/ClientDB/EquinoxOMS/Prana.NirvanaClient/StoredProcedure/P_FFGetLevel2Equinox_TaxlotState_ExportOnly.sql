/****** Object:  StoredProcedure [dbo].[P_FFGetLevel2_NewtynAdmin_TaxlotState_ExportOnly]    Script Date: 08/07/2018 13:05:05 ******/
/*          
 Created By: Ankit Gupta          
 Date: November 02, 2013          
 Description: To fetch Level2 Intraday trades'data for closed and open taxlots with their respective closing information           
     and maintain taxlot states for Executing brokers and Administrator.          
*/
CREATE PROCEDURE [dbo].[P_FFGetLevel2Equinox_TaxlotState_ExportOnly] (
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
	,OrderTypeTagValue VARCHAR(3)
	,OrderSideTagValue VARCHAR(3)
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
	,--Percentage,                                                                                                   
	TaxLotQty FLOAT
	,SettlementDate DATETIME
	,Commission FLOAT
	,OtherBrokerFees FLOAT
	,TaxlotState VARCHAR(50)
	,TradeAttribute1 VARCHAR(200)
	,TradeAttribute2 VARCHAR(200)
	,TradeAttribute3 VARCHAR(200)
	,TradeAttribute4 VARCHAR(200)
	,TradeAttribute5 VARCHAR(200)
	,TradeAttribute6 VARCHAR(200)
	,StampDuty FLOAT
	,TransactionLevy FLOAT
	,ClearingFee FLOAT
	,TaxOnCommissions FLOAT
	,SoftCommission FLOAT
	,MiscFees FLOAT
	,SecFee FLOAT
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
	,ForexRate FLOAT
	)

INSERT INTO #VT
SELECT VT.TaxLotID AS TaxLotID
	,ISNULL(VT.FundID, 0) AS AccountID
	,VT.OrderTypeTagValue
	,VT.OrderSideTagValue
	,VT.Symbol
	,CASE 
		WHEN VT.CounterPartyID = '-2147483648'
			OR VT.CounterPartyID IS NULL
			THEN 0
		ELSE VT.CounterPartyID
		END AS CounterPartyID
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
	,ISNULL(VT.OtherBrokerFees, 0) AS OtherBrokerFees
	,T_PBWiseTaxlotState.TaxLotState AS TaxlotState
	,VT.TradeAttribute1
	,VT.TradeAttribute2
	,VT.TradeAttribute3
	,VT.TradeAttribute4
	,VT.TradeAttribute5
	,VT.TradeAttribute6
	,ISNULL(VT.StampDuty, 0) AS StampDuty
	,ISNULL(VT.TransactionLevy, 0) AS TransactionLevy
	,ISNULL(ClearingFee, 0) AS ClearingFee
	,ISNULL(TaxOnCommissions, 0) AS TaxOnCommissions
	,ISNULL(VT.SoftCommission, 0) AS SoftCommission
	,ISNULL(MiscFees, 0) AS MiscFees
	,ISNULL(SecFee, 0) AS SecFee
	,VT.AUECLocalDate
	,VT.Level2ID AS Level2ID
	,T_PBWiseTaxlotState.PBID
	,ISNULL(VT.FXRate, 0) AS FXRate
	,VT.FXConversionMethodOperator
	,'No' AS FromDeleted
	,VT.ProcessDate
	,VT.OriginalPurchaseDate
	,ISNULL(VT.AccruedInterest, 0) AS AccruedInterest
	,ISNULL(VT.FXRate_Taxlot, 0) AS FXRate_Taxlot
	,VT.FXConversionMethodOperator_Taxlot
	,SideMultiplier
	,VT.Description
	,1
FROM V_TaxLots VT
LEFT OUTER JOIN T_PBWiseTaxlotState ON T_PBWiseTaxlotState.TaxlotID = VT.TaxlotID
	AND (
		@fileFormatID = FileFormatID
		OR FileFormatID = 0
		)

UNION ALL

SELECT TDT.TaxLotID AS TaxLotID
	,ISNULL(TDT.FundID, 0) AS AccountID
	,TDT.OrderTypeTagValue
	,TDT.OrderSideTagValue AS SideID
	,TDT.Symbol
	,ISNULL(TDT.CounterPartyID, 0) AS CounterPartyID
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
	,TDT.TradeAttribute1
	,TDT.TradeAttribute2
	,TDT.TradeAttribute3
	,TDT.TradeAttribute4
	,TDT.TradeAttribute5
	,TDT.TradeAttribute6
	,ISNULL(TDT.StampDuty, 0) AS StampDuty
	,ISNULL(TDT.TransactionLevy, 0) AS TransactionLevy
	,ISNULL(TDT.ClearingFee, 0) AS ClearingFee
	,ISNULL(TDT.TaxOnCommissions, 0) AS TaxOnCommissions
	,ISNULL(TDT.SoftCommission, 0) AS SoftCommission
	,ISNULL(TDT.MiscFees, 0) AS MiscFees
	,ISNULL(TDT.SecFee, 0) AS SecFee
	,TDT.AUECLocalDate
	,0 AS Level2ID
	,TDT.PBID
	,ISNULL(TDT.FXRate, 0) AS FXRate
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
	,1
FROM T_DeletedTaxLots TDT

UPDATE #VT
SET VenueID = 1

UPDATE #VT
SET ForexRate = FX.ConversionRate
FROM #VT
INNER JOIN T_CurrencyConversionRate FX ON #VT.CurrencyID = FX.CurrencyPairID_FK
	AND DateDiff(D, #VT.AUECLocalDate, FX.DATE) = 0

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
	,T_Exchange.DisplayName AS Exchange
	,Currency.CurrencySymbol
	,CTPM.MappedName
	,CTPM.FundAccntNo
	,FT.FundTypeName
	,VT.TaxLotID AS EntityID
	,VT.Level2Percentage AS Level2Percentage
	,--Percentage,                                                                                                   
	VT.TaxLotQty AS AllocatedQty
	,SM.PutOrCall
	,ISNULL(SM.StrikePrice, 0) AS StrikePrice
	,convert(VARCHAR, SM.ExpirationDate, 101) AS ExpirationDate
	,convert(VARCHAR, VT.SettlementDate, 101) AS SettlementDate
	,VT.Commission AS CommissionCharged
	,VT.OtherBrokerFees AS OtherBrokerFee
	,T_ThirdPartyType.ThirdPartyTypeName
	,ISNULL(T_CounterPartyVenue.DisplayName, '') AS CVName
	,CASE 
		WHEN VT.TaxLotState = 0
			THEN 'Allocated'
		WHEN VT.TaxLotState = 1
			THEN 'Sent'
		WHEN VT.TaxLotState = 2
			THEN 'Amemded'
		WHEN VT.TaxLotState = 3
			THEN 'Deleted'
		WHEN VT.TaxLotState = 4
			THEN 'Ignored'
		END AS TaxLotState
	,VT.TradeAttribute1
	,VT.TradeAttribute2
	,VT.TradeAttribute3
	,VT.TradeAttribute4
	,VT.TradeAttribute5
	,VT.TradeAttribute6
	,VT.FromDeleted
	,ISNULL(VT.StampDuty, 0) AS StampDuty
	,ISNULL(VT.TransactionLevy, 0) AS TransactionLevy
	,ISNULL(VT.ClearingFee, 0) AS ClearingFee
	,ISNULL(VT.TaxOnCommissions, 0) AS TaxOnCommissions
	,ISNULL(VT.SoftCommission, 0) AS SoftCommission
	,ISNULL(VT.MiscFees, 0) AS MiscFees
	,ISNULL(VT.SecFee, 0) AS SecFee
	,convert(VARCHAR, VT.AUECLocalDate, 101) AS TradeDate
	,ISNULL(SM.Multiplier, 1) AS AssetMultiplier
	,CASE 
		WHEN T_CounterParty.ShortName = 'GWEP'
			AND VT.AssetID = 8
			THEN Cast(Cast(((Round(VT.AvgPrice, 4) * VT.TaxlotQty * 0.01) + ((VT.Commission + VT.SoftCommission + VT.OtherBrokerFees + VT.TransactionLevy + VT.StampDuty + VT.ClearingFee + VT.TaxOnCommissions + VT.MiscFees) * 0.01)) AS DECIMAL(32, 2)) AS VARCHAR(500))
		WHEN T_CounterParty.ShortName = 'GWEP'
			THEN Cast(Cast(((Round(VT.AvgPrice, 4) * VT.TaxlotQty * SM.Multiplier) + ((VT.Commission + VT.SoftCommission + VT.OtherBrokerFees + VT.TransactionLevy + VT.StampDuty + VT.ClearingFee + VT.TaxOnCommissions + VT.MiscFees) * VT.SideMultiplier)) AS Decimal(32, 2)) AS VARCHAR(500))
		WHEN VT.AssetID = 8
			AND T_CounterParty.ShortName != 'GWEP'
			THEN Cast(Cast(((VT.AvgPrice * VT.TaxlotQty * 0.01) + ((VT.Commission + VT.SoftCommission + VT.OtherBrokerFees + VT.TransactionLevy + VT.StampDuty + VT.ClearingFee + VT.TaxOnCommissions + VT.MiscFees) * 0.01)) AS DECIMAL(32, 2)) AS VARCHAR(500))
		ELSE Cast(Cast(((VT.AvgPrice * VT.TaxlotQty * SM.Multiplier) + ((VT.Commission + VT.SoftCommission + VT.OtherBrokerFees + VT.TransactionLevy + VT.StampDuty + VT.ClearingFee + VT.TaxOnCommissions + VT.MiscFees) * VT.SideMultiplier)) AS DECIMAL(32, 2)) AS Varchar(500))
		END AS NetAmount
	,VT.Level2ID
	,SM.ISINSymbol
	,SM.CUSIPSymbol
	,SM.SEDOLSymbol
	,SM.ReutersSymbol
	,SM.BloombergSymbol AS BBCode
	,SM.CompanyName AS FullSecurityName
	,SM.UnderlyingSymbol
	,SM.LeadCurrency
	,SM.VsCurrency
	,SM.OSISymbol
	,SM.IDCOSymbol
	,SM.OpraSymbol
	,VT.FXRate
	,VT.FXConversionMethodOperator
	,VT.ProcessDate
	,VT.OriginalPurchaseDate
	,VT.AccruedInterest
	,T_Country.CountryName AS CountryName
	,VT.FXRate_Taxlot
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
	,VT.Level1AllocationID
	,ISNULL(VT.ForexRate, 1) AS ForexRate
FROM #VT VT
INNER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID
INNER JOIN T_FundType AS FT ON FT.FundTypeID = CTPM.FundTypeID_FK
LEFT OUTER JOIN PM_TaxLotClosing Closing ON Closing.ClosingTaxLotID = VT.TaxLotID
LEFT OUTER JOIN T_ClosingAlgos ON T_ClosingAlgos.AlgorithmId = Closing.ClosingAlgo
LEFT OUTER JOIN #VTaxlot ON Closing.PositionalTaxLotID = #VTaxlot.TaxLotID
LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
LEFT JOIN T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue
LEFT JOIN T_CompanyFunds ON T_CompanyFunds.CompanyFundID = VT.FundID
LEFT JOIN T_Asset ON T_Asset.AssetID = VT.AssetID
LEFT JOIN T_CounterParty ON T_CounterParty.CounterPartyID = VT.CounterPartyID
LEFT JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID
LEFT JOIN T_Country ON dbo.T_Country.CountryID = T_Exchange.Country
LEFT JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue
INNER JOIN dbo.T_CompanyThirdParty ON T_CompanyThirdParty.CompanyThirdPartyID = CTPM.CompanyThirdPartyID_FK
LEFT JOIN T_CounterPartyVenue ON T_CounterPartyVenue.CounterPartyID = VT.CounterPartyID
	AND T_CounterPartyVenue.VenueID = VT.VenueID
INNER JOIN dbo.T_ThirdParty ON T_ThirdParty.ThirdPartyId = T_CompanyThirdParty.ThirdPartyId
	AND T_CompanyThirdParty.CompanyID = @companyID
INNER JOIN dbo.T_ThirdPartyType ON T_ThirdPartyType.ThirdPartyTypeId = T_ThirdParty.ThirdPartyTypeID
LEFT OUTER JOIN V_SecMasterData SM ON SM.TickerSymbol = VT.Symbol
WHERE datediff(d, (
			CASE 
				WHEN @dateType = 1
					THEN VT.AUECLocalDate
				ELSE VT.ProcessDate
				END
			), @inputdate) = 0
	AND CTPM.InternalFundNameID_FK IN (
		SELECT FundID
		FROM @Fund
		)
	AND (
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
	AND VT.AUECID IN (
		SELECT AUECID
		FROM @AUECID
		)
ORDER BY TaxlotId

DROP TABLE #VT
	,#VTaxlot
