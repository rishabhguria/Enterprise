/*                                                                                                                
Created By: Ankit Gupta
On: April 29, 2013
Purpose: To make customized changes for EOD files  for Prompt Date, Last TradeDate, Delivery Date.
                                                                                                
[P_GetThirdPartyCitrineDetails_ExportOnly] 23,'1184,1183,1182,1186,1185','4/22/2013 3:51:24 PM',5,'1,20,21,18,1,15,12,16,33',1              
            
*/
CREATE PROCEDURE [dbo].[P_GetThirdPartyCitrineDetails_ExportOnly] (
	@thirdPartyID INT
	,@companyFundIDs VARCHAR(max)
	,@inputDate DATETIME
	,@companyID INT
	,@auecIDs VARCHAR(max)
	,@TypeID INT -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                                                                                 
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

INSERT INTO @Fund
SELECT Cast(Items AS INT)
FROM dbo.Split(@companyFundIDs, ',')

INSERT INTO @AUECID
SELECT Cast(Items AS INT)
FROM dbo.Split(@auecIDs, ',')

IF (@TypeID = 0)
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
		,CTPM.MappedName
		,CTPM.FundAccntNo
		,CTPM.FundTypeID_FK
		,FT.FundTypeName
		,VT.Level1AllocationID AS EntityID
		,sum(VT.Level2Percentage) AS Level2Percentage
		,--Percentage,                                                                                                                
		sum(VT.TaxLotQty) AS AllocatedQty
		,SM.PutOrCall
		,SM.StrikePrice
		,convert(VARCHAR, SM.ExpirationDate, 101) AS ExpirationDate
		,convert(VARCHAR, VT.SettlementDate, 101) AS SettlementDate
		,sum(VT.Commission) AS CommissionCharged
		,sum(VT.OtherBrokerFees) AS OtherBrokerFee
		,T_ThirdPartyType.ThirdPartyTypeID
		,T_ThirdPartyType.ThirdPartyTypeName
		,CTPFD.CompanyIdentifier
		,0 AS SecFee
		,ISNULL(T_CounterPartyVenue.DisplayName, '') AS CVName
		,ISNULL(T_CompanyThirdPartyCVIdentifier.CVIdentifier, '') CVIdentifier
		,VT.GroupRefID
		,0 AS TaxLotStateID
		,'Allocated' AS TaxLotState
		,sum(ISNULL(VT.StampDuty, 0)) AS StampDuty
		,Sum(ISNULL(VT.TransactionLevy, 0)) AS TransactionLevy
		,Sum(ISNULL(ClearingFee, 0)) AS ClearingFee
		,Sum(ISNULL(TaxOnCommissions, 0)) AS TaxOnCommissions
		,Sum(ISNULL(MiscFees, 0)) AS MiscFees
		,convert(VARCHAR, VT.AUECLocalDate, 101) AS TradeDate
		,SM.Multiplier AS AssetMultiplier
		,0 AS Level2ID
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
		,'No' AS FromDeleted
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
		,convert(VARCHAR, dbo.AdjustBusinessDays(SM.ExpirationDate, - 2, VT.AUECID), 101) AS LastTradeDate
	FROM V_TaxLots VT
	INNER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID
	INNER JOIN T_FundType AS FT ON FT.FundTypeID = CTPM.FundTypeID_FK
	LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
	LEFT JOIN T_CompanyFunds ON T_CompanyFunds.CompanyFundID = VT.FundID
	LEFT JOIN T_Asset ON T_Asset.AssetID = VT.AssetID
	LEFT JOIN T_CounterParty ON T_CounterParty.CounterPartyID = VT.CounterPartyID
	LEFT JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID
	LEFT JOIN T_Country ON dbo.T_Country.CountryID = T_Exchange.Country
	LEFT JOIN T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue
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
	WHERE DATEDIFF(d, VT.AUECLocalDate, @inputDate) = 0
		AND CTPM.InternalFundNameID_FK IN (
			SELECT FundID
			FROM @Fund
			)
		AND VT.AUECID IN (
			SELECT AUECID
			FROM @AUECID
			)
	GROUP BY VT.GroupID
		,VT.FundID
		,T_CompanyFunds.FundShortName
		,
		--VT.TaxlotID,                            
		VT.Level1AllocationID
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
		,sum(VT.Level2Percentage) AS Level2Percentage
		,sum(VT.TaxLotQty) AS AllocatedQty
		,' ' AS IsBasketGroup
		,SM.PutOrCall
		,SM.StrikePrice
		,convert(VARCHAR, SM.ExpirationDate, 101) AS ExpirationDate
		,convert(VARCHAR, VT.SettlementDate, 101) AS SettlementDate
		,sum(VT.Commission) AS CommissionCharged
		,sum(VT.OtherBrokerFees) AS OtherBrokerFee
		,0 AS ThirdPartyTypeID
		,'' AS ThirdPartyTypeName
		,'' AS CompanyIdentifier
		,0 AS SecFee
		,'' AS CVName
		,'' AS CVIdentifier
		,0 AS CompanyCounterPartyCVID
		,VT.GroupRefID
		,0 AS TaxLotStateID
		,'Allocated' AS TaxLotState
		,sum(ISNULL(VT.StampDuty, 0)) AS StampDuty
		,Sum(ISNULL(VT.TransactionLevy, 0)) AS TransactionLevy
		,Sum(ISNULL(ClearingFee, 0)) AS ClearingFee
		,Sum(ISNULL(TaxOnCommissions, 0)) AS TaxOnCommissions
		,Sum(ISNULL(MiscFees, 0)) AS MiscFees
		,convert(VARCHAR, VT.AUECLocalDate, 101) AS TradeDate
		,SM.Multiplier AS AssetMultiplier
		,0 AS Level2ID
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
		,'No' AS FromDeleted
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
		,convert(VARCHAR, dbo.AdjustBusinessDays(SM.ExpirationDate, - 2, VT.AUECID), 101) AS LastTradeDate
	FROM V_TaxLots VT
	LEFT JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID
	LEFT JOIN T_Country ON dbo.T_Country.CountryID = T_Exchange.Country
	LEFT JOIN T_CompanyFunds ON T_CompanyFunds.CompanyFundID = VT.FundID
	LEFT JOIN T_Asset ON T_Asset.AssetID = VT.AssetID
	LEFT JOIN T_CounterParty ON T_CounterParty.CounterPartyID = VT.CounterPartyID
	LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
	LEFT JOIN T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue
	LEFT JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue
	LEFT OUTER JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol
	WHERE DATEDIFF(d, VT.AUECLocalDate, @inputDate) = 0
		AND VT.FundID IN (
			SELECT FundID
			FROM @Fund
			)
		AND VT.AUECID IN (
			SELECT AUECID
			FROM @AUECID
			)
	GROUP BY VT.GroupID
		,VT.FundID
		,T_CompanyFunds.FundShortName
		,
		--VT.TaxlotID,                                              
		VT.Level1AllocationID
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