/*              
Created By: Ankit Gupta        
On: April 29, 2013        
Purpose: To make customized changes for EOD files for Prompt Date, Last TradeDate, Delivery Date.                               
*/
CREATE PROCEDURE [dbo].[P_GetThirdPartyCitrineDetails] (
	@thirdPartyID INT
	,@companyFundIDs VARCHAR(max)
	,@inputDate DATETIME
	,@companyID INT
	,@auecIDs VARCHAR(max)
	,@TypeID INT -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                                                                                                            
	,@dateType INT -- 0 means for Process Date and 1 means Trade Date.            
	,@fileFormatID INT
	)
AS
--DECLARE 	@thirdPartyID INT
--	,@companyFundIDs VARCHAR(max)
--	,@inputDate DATETIME
--	,@companyID INT
--	,@auecIDs VARCHAR(max)
--	,@TypeID INT -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                                                                                                            
--	,@dateType INT -- 0 means for Process Date and 1 means Trade Date.            
--	,@fileFormatID INT
--SET @thirdPartyID=27
--SET @companyFundIDs=N'1184,1190,1183,1182'
--SET @inputDate='2019-07-08 18:26:33'
--SET @companyID=5
--SET @auecIDs=N'1,15,12'
--SET @TypeID=0
--SET @dateType=0
--SET @fileFormatID=55
DECLARE @Fund TABLE (FundID INT)
DECLARE @AUECID TABLE (AUECID INT)
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

CREATE TABLE #VT (
	TaxlotID VARCHAR(MAX)
	,FundID INT
	,OrderTypeTagValue VARCHAR(MAX)
	,SideID VARCHAR(3)
	,Symbol VARCHAR(100)
	,CounterPartyID INT
	,VenueID INT
	,TaxLotQty FLOAT
	,AvgPrice FLOAT
	,CumQty FLOAT
	,Quantity FLOAT
	,AUECID INT
	,AssetID INT
	,UnderlyingID INT
	,ExchangeID INT
	,CurrencyID INT
	,Level1AllocationID VARCHAR(MAX)
	,Level2Percentage INT
	,AllocatedQty FLOAT
	,SettlementDate DATETIME
	,Commission FLOAT
	,OtherBrokerFees FLOAT
	,GroupRefID INT
	,TaxlotState INT
	,StampDuty FLOAT
	,TransactionLevy FLOAT
	,ClearingFee FLOAT
	,TaxOnCommissions FLOAT
	,MiscFees FLOAT
	,AUECLocalDate DATETIME
	,Level2ID VARCHAR(MAX)
	,PBID INT
	,FXRate FLOAT
	,FXConversionMethodOperator VARCHAR(3)
	,ProcessDate DATETIME
	,OriginalPurchaseDate DATETIME
	,AccruedInterest FLOAT
	,SwapDescription VARCHAR(MAX)
	,IsSwapped BIT
	,FXRate_Taxlot FLOAT
	,FXConversionMethodOperator_Taxlot VARCHAR(3)
	,LotID VARCHAR(100)
	,ExternalTransID VARCHAR(100)
	,TradeAttribute1 VARCHAR(200)
	,TradeAttribute2 VARCHAR(200)
	,TradeAttribute3 VARCHAR(200)
	,TradeAttribute4 VARCHAR(200)
	,TradeAttribute5 VARCHAR(200)
	,TradeAttribute6 VARCHAR(200)
	,Description VARCHAR(500)
	,FileFormatID INT
	)

IF (@TypeID = 0)
BEGIN
	INSERT INTO #VT
	SELECT VT.Level1AllocationID AS TaxlotID
		,ISNULL(VT.FundID, 0) AS FundID
		,VT.OrderTypeTagValue
		,VT.OrderSideTagValue AS SideID
		,VT.Symbol
		,VT.CounterPartyID
		,VT.VenueID
		,(VT.TaxLotQty)
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
		,VT.ProcessDate
		,VT.OriginalPurchaseDate
		,VT.AccruedInterest
		,VT.SwapDescription
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
		,@fileFormatID
	FROM V_TaxLots VT
	INNER JOIN @Fund Fund ON VT.FundID = Fund.FundID
	INNER JOIN @AUECID AUEC ON VT.AUECID = AUEC.AUECID
	WHERE datediff(d, (
				CASE 
					WHEN @dateType = 1
						THEN VT.AUECLocalDate
					ELSE VT.ProcessDate
					END
				), @inputdate) >= 0
	
	UNION ALL
	
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
		,TDT.Level1AllocationID AS EntityID
		,(TDT.Level2Percentage)
		,(TDT.TaxLotQty) AS AllocatedQty
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
		,TDT.AUECLocalDate AS TradeDate
		,0 AS Level2ID
		,TDT.PBID
		,TDT.FXRate AS FXRate
		,TDT.FXConversionMethodOperator AS FXConversionMethodOperator
		,TDT.ProcessDate
		,TDT.OriginalPurchaseDate
		,TDT.AccruedInterest
		,TDT.SwapDescription
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
		,TDT.FileFormatID
	FROM T_DeletedTaxLots TDT
	INNER JOIN @Fund Fund ON TDT.FundID = Fund.FundID
	INNER JOIN @AUECID AUEC ON TDT.AUECID = AUEC.AUECID
	WHERE datediff(d, (
				CASE 
					WHEN @dateType = 1
						THEN TDT.AUECLocalDate
					ELSE TDT.ProcessDate
					END
				), @inputdate) >= 0
		AND TDT.FIleFormatID = @fileFormatID

	UPDATE #VT
	SET #VT.TaxlotState = PB.TaxlotState
	FROM #VT
	INNER JOIN T_PBWiseTaxlotState PB ON PB.TaxLotID = #VT.Level1AllocationID
	WHERE PB.TaxlotState <> 0
		AND PB.PBID = @ThirdPartyID
		AND PB.FileFormatID = @fileFormatID

	SELECT VT.Level1AllocationID
		,ISNULL(VT.FundID, 0) AS FundID
		,T_CompanyFunds.FundShortName AS AccountName
		,ISNULL(T_OrderType.OrderTypesID, 0) AS OrderTypesID
		,ISNULL(T_OrderType.OrderTypes, 'Multiple') AS OrderTypes
		,VT.SideID
		,T_Side.Side AS Side
		,VT.Symbol
		,VT.CounterPartyID
		,T_CounterParty.ShortName AS CounterParty
		,VT.VenueID
		,Sum(VT.TaxLotQty) AS OrderQty
		,VT.AvgPrice AS AveragePrice
		,VT.CumQty AS ExecutedQty
		,VT.Quantity AS TotalQty
		,VT.AUECID
		,VT.AssetID
		,T_Asset.AssetName AS Asset
		,VT.UnderlyingID
		,VT.ExchangeID
		,T_Exchange.DisplayName AS Exchange
		,Currency.CurrencyID
		,Currency.CurrencyName
		,Currency.CurrencySymbol
		,CTPM.MappedName
		,CTPM.FundAccntNo
		,CTPM.FundTypeID_FK
		,FT.FundTypeName
		,VT.Level1AllocationID AS EntityID
		,Sum(VT.Level2Percentage) AS Level2Percentage
		,Sum(VT.TaxLotQty) AS AllocatedQty
		,SM.PutOrCall
		,SM.StrikePrice
		,convert(VARCHAR, SM.ExpirationDate, 101) AS ExpirationDate
		,convert(VARCHAR, VT.SettlementDate, 101) AS SettlementDate
		,Sum(VT.Commission) AS CommissionCharged
		,Sum(VT.OtherBrokerFees) AS OtherBrokerFee
		,T_ThirdPartyType.ThirdPartyTypeID
		,T_ThirdPartyType.ThirdPartyTypeName
		,0 AS SecFee
		,ISNULL(T_CounterPartyVenue.DisplayName, '') AS CVName
		,ISNULL(T_CompanyThirdPartyCVIdentifier.CVIdentifier, '') CVIdentifier
		,VT.GroupRefID
		,VT.TaxlotState AS TaxlotStateID
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
		,ISNULL(SM.Multiplier, 1) AS AssetMultiplier
		,0 AS Level2ID
		,SM.ISINSymbol
		,SM.CUSIPSymbol
		,SM.SEDOLSymbol
		,SM.ReutersSymbol
		,SM.BloombergSymbol AS BBCode
		,SM.CompanyName
		,SM.UnderlyingSymbol
		,SM.OSISymbol
		,SM.IDCOSymbol
		,SM.OpraSymbol
		,VT.FXRate
		,VT.FXConversionMethodOperator
		,VT.ProcessDate
		,VT.OriginalPurchaseDate
		,VT.AccruedInterest
		,
		-- Reserved for future use                                
		'' AS Comment1
		,'' AS Comment2
		,
		--Swap Parameters                              
		VT.SwapDescription
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
		,CASE 
			WHEN (datediff(m, VT.AUECLocalDate, SM.ExpirationDate) = 3)
				AND (Day(SM.ExpirationDate) = Day(VT.AUECLocalDate))
				THEN '3M'
			WHEN datepart(dw, SM.ExpirationDate) = 4
				AND DATEPART(DAY, SM.ExpirationDate - 1) / 7 = 2
				THEN '3W'
			ELSE 'NO'
			END AS PromptDateCheck
		,CASE 
			WHEN dbo.IsBusinessDay(SM.ExpirationDate, VT.AUECID) = 0
				THEN convert(VARCHAR, dbo.AdjustBusinessDays(SM.ExpirationDate, 1, VT.AUECID), 101)
			ELSE convert(VARCHAR, SM.ExpirationDate, 101)
			END AS PromptDate
		,CASE 
			WHEN VT.AssetID = 4
				AND (
					T_Exchange.DisplayName = 'LME'
					OR T_Exchange.DisplayName = 'LME-FO'
					)
				THEN convert(VARCHAR, dbo.AdjustBusinessDays(SM.ExpirationDate, - 1, VT.AUECID), 101)
			ELSE convert(VARCHAR, dbo.AdjustBusinessDays(SM.ExpirationDate, - 2, VT.AUECID), 101)
			END AS LastTradeDate
		,convert(VARCHAR, dbo.AdjustBusinessDays(SM.ExpirationDate, 2, VT.AUECID), 101) AS DeliveryDate
	FROM #VT AS VT
	INNER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID
	INNER JOIN T_FundType AS FT ON FT.FundTypeID = CTPM.FundTypeID_FK
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
	WHERE datediff(d, (
				CASE 
					WHEN @dateType = 1
						THEN VT.AUECLocalDate
					ELSE VT.ProcessDate
					END
				), @inputdate) >= 0
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
		AND VT.PBID = @thirdPartyID
		AND (VT.FileFormatID = @fileFormatID)
	GROUP BY VT.TaxlotID
		,VT.Level1AllocationID
		,VT.FundID
		,T_CompanyFunds.FundShortName
		,T_OrderType.OrderTypesID
		,T_OrderType.OrderTypes
		,VT.SideID
		,T_Side.Side
		,VT.Symbol
		,VT.CounterPartyID
		,T_CounterParty.ShortName
		,VT.VenueID
		,VT.AvgPrice
		,VT.CumQty
		,VT.Quantity
		,VT.AUECID
		,VT.AssetID
		,T_Asset.AssetName
		,VT.UnderlyingID
		,VT.ExchangeID
		,T_Exchange.DisplayName
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
		,T_CounterPartyVenue.DisplayName
		,T_CompanyThirdPartyCVIdentifier.CVIdentifier
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
		,SM.OSISymbol
		,SM.IDCOSymbol
		,SM.OpraSymbol
		,VT.FXRate
		,VT.FXConversionMethodOperator
		,VT.ProcessDate
		,VT.OriginalPurchaseDate
		,VT.AccruedInterest
		,
		--Swap Parameters                              
		VT.SwapDescription
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
	ORDER BY GroupRefID
END
ELSE
BEGIN
	SELECT VT.Level1AllocationID
		,ISNULL(VT.FundID, 0) AS FundID
		,T_CompanyFunds.FundShortName AS FundName
		,ISNULL(T_OrderType.OrderTypesID, 0) AS OrderTypesID
		,ISNULL(T_OrderType.OrderTypes, 'Multiple') AS OrderTypes
		,VT.OrderSideTagValue AS SideID
		,T_Side.Side AS Side
		,VT.Symbol
		,VT.CounterPartyID
		,T_CounterParty.ShortName AS CounterParty
		,VT.VenueID
		,Sum(VT.TaxLotQty) AS OrderQty
		,VT.AvgPrice AS AveragePrice
		,VT.CumQty AS ExecutedQty
		,VT.Quantity AS TotalQty
		,VT.AUECID
		,VT.AssetID
		,T_Asset.AssetName AS Asset
		,VT.UnderlyingID
		,VT.ExchangeID
		,T_Exchange.DisplayName AS Exchange
		,Currency.CurrencyID
		,Currency.CurrencyName
		,Currency.CurrencySymbol
		,'' AS MappedName
		,'' AS FundAccntNo
		,0 AS FundTypeID_FK
		,'' AS FundTypeName
		,VT.Level1AllocationID AS EntityID
		,Sum(VT.Level2Percentage) AS Level2Percentage
		,Sum(VT.TaxLotQty) AS AllocatedQty
		,SM.PutOrCall
		,SM.StrikePrice
		,convert(VARCHAR, SM.ExpirationDate, 101) AS ExpirationDate
		,convert(VARCHAR, VT.SettlementDate, 101) AS SettlementDate
		,Sum(VT.Commission) AS CommissionCharged
		,Sum(VT.OtherBrokerFees) AS OtherBrokerFees
		,0 AS ThirdPartyTypeID
		,'' AS ThirdPartyTypeName
		,0 AS SecFee
		,'' AS CVName
		,'' AS CVIdentifier
		,VT.GroupRefID
		,ISNULL(VT.TaxlotState, 0) AS TaxLotStateID
		,CASE 
			WHEN VT.TaxLotState = '0'
				OR VT.TaxLotState IS NULL
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
		,ISNULL(SM.Multiplier, 1) AS AssetMultiplier
		,0 AS Level2ID
		,SM.ISINSymbol
		,SM.CUSIPSymbol
		,SM.SEDOLSymbol
		,SM.ReutersSymbol
		,SM.BloombergSymbol AS BBCode
		,SM.CompanyName
		,SM.UnderlyingSymbol
		,SM.OSISymbol
		,SM.IDCOSymbol
		,SM.OpraSymbol
		,VT.FXRate
		,VT.FXConversionMethodOperator
		,VT.ProcessDate
		,VT.OriginalPurchaseDate
		,VT.AccruedInterest
		,
		-- Reserved for future use                                
		'' AS Comment1
		,'' AS Comment2
		,
		--Swap Parameters                              
		VT.SwapDescription
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
		,CASE 
			WHEN (datediff(m, VT.AUECLocalDate, SM.ExpirationDate) = 3)
				AND (Day(SM.ExpirationDate) = Day(VT.AUECLocalDate))
				THEN '3M'
			WHEN datepart(dw, SM.ExpirationDate) = 4
				AND DATEPART(DAY, SM.ExpirationDate - 1) / 7 = 2
				THEN '3W'
			ELSE 'NO'
			END AS PromptDateCheck
		,CASE 
			WHEN dbo.IsBusinessDay(SM.ExpirationDate, VT.AUECID) = 0
				THEN convert(VARCHAR, dbo.AdjustBusinessDays(SM.ExpirationDate, 1, VT.AUECID), 101)
			ELSE convert(VARCHAR, SM.ExpirationDate, 101)
			END AS PromptDate
		,CASE 
			WHEN VT.AssetID = 4
				AND (
					T_Exchange.DisplayName = 'LME'
					OR T_Exchange.DisplayName = 'LME-FO'
					)
				THEN convert(VARCHAR, dbo.AdjustBusinessDays(SM.ExpirationDate, - 1, VT.AUECID), 101)
			ELSE convert(VARCHAR, dbo.AdjustBusinessDays(SM.ExpirationDate, - 2, VT.AUECID), 101)
			END AS LastTradeDate
		,convert(VARCHAR, dbo.AdjustBusinessDays(SM.ExpirationDate, 2, VT.AUECID), 101) AS DeliveryDate
	FROM V_TaxLots VT
	LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
	LEFT JOIN T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue
	LEFT JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue
	LEFT JOIN T_CompanyFunds ON T_CompanyFunds.CompanyFundID = VT.FundID
	LEFT JOIN T_Asset ON T_Asset.AssetID = VT.AssetID
	LEFT JOIN T_CounterParty ON T_CounterParty.CounterPartyID = VT.CounterPartyID
	LEFT JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID
	LEFT JOIN T_Country ON dbo.T_Country.CountryID = T_Exchange.Country
	LEFT OUTER JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol
	WHERE datediff(d, (
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
	GROUP BY
		--  VT.TaxlotID,                                                                                      
		VT.Level1AllocationID
		,VT.FundID
		,T_CompanyFunds.FundShortName
		,T_OrderType.OrderTypesID
		,T_OrderType.OrderTypes
		,VT.OrderSideTagValue
		,T_Side.Side
		,VT.Symbol
		,VT.CounterPartyID
		,T_CounterParty.ShortName
		,VT.VenueID
		,VT.AvgPrice
		,VT.CumQty
		,VT.Quantity
		,VT.AUECID
		,VT.AssetID
		,T_Asset.AssetName
		,VT.UnderlyingID
		,VT.ExchangeID
		,T_Exchange.DisplayName
		,Currency.CurrencyID
		,Currency.CurrencyName
		,Currency.CurrencySymbol
		,SM.PutOrCall
		,SM.StrikePrice
		,SM.ExpirationDate
		,VT.SettlementDate
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
		,SM.OSISymbol
		,SM.IDCOSymbol
		,SM.OpraSymbol
		,VT.FXRate
		,VT.FXConversionMethodOperator
		,VT.ProcessDate
		,VT.OriginalPurchaseDate
		,VT.AccruedInterest
		,
		--Swap Parameters                              
		VT.SwapDescription
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
	--VT.FromDeleted                                                           
	ORDER BY GroupRefID
END

DROP TABLE #VT
