/*                  
Created By: Ankit Gupta            
On: April 29, 2013            
Purpose: To make customized changes for EOD files for Prompt Date, Last TradeDate, Delivery Date.            
            
            
[P_GetThirdPartyCitrineDetails] 27,'1184,1183,1182,1186,1185','04/26/2013',5,'1,20,21,18,1,15,12,16,33, 80',1                                  
*/
CREATE PROCEDURE [dbo].[P_GetThirdPartyCitrineDetails_Admin] (
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
				), @inputdate) = 0
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