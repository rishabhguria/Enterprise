/*                        
Created By: Ankit Gupta                  
On: April 29, 2013                  
Purpose: To make customized changes for EOD files for Prompt Date, Last TradeDate, Delivery Date.                  
                  
                  
[P_GetThirdPartyCitrineDetails] 27,'1184,1183,1182,1186,1185','04/26/2013',5,'1,20,21,18,1,15,12,16,33, 80',1                                        
*/
CREATE PROCEDURE [dbo].[P_GetThirdPartyCitrineDetails_TaxlotState_Admin_Citrine] (
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
	CREATE TABLE #VT (
		Level1AllocationID VARCHAR(50)
		,FundID INT
		,SideID VARCHAR(3)
		,Symbol VARCHAR(100)
		,CounterPartyID INT
		,AveragePrice FLOAT
		,ExecutedQty FLOAT
		,TotalQty FLOAT
		,AUECID INT
		,AssetID INT
		,UnderlyingID INT
		,ExchangeID INT
		,CurrencyID INT
		,Level2Percentage INT
		,AllocatedQty FLOAT
		,SettlementDate DATETIME
		,CommissionCharged FLOAT
		,OtherBrokerFees FLOAT
		,TaxLotStateID INT
		,StampDuty FLOAT
		,TransactionLevy FLOAT
		,ClearingFee FLOAT
		,TaxOnCommissions FLOAT
		,MiscFees FLOAT
		,AUECLocalDate DATETIME
		,ProcessDate DATETIME
		,FXRate FLOAT
		,FXConversionMethodOperator VARCHAR(3)
		,GroupRefID INT
		,PBID INT
		,LotID VARCHAR(100)
		,ExternalTransID VARCHAR(100)
		,TradeAttribute1 VARCHAR(200)
		,TradeAttribute2 VARCHAR(200)
		,TradeAttribute3 VARCHAR(200)
		,TradeAttribute4 VARCHAR(200)
		,TradeAttribute5 VARCHAR(200)
		,TradeAttribute6 VARCHAR(200)
		,Description VARCHAR(500)
		)

	INSERT INTO #VT
	SELECT VT.Level1AllocationID
		,ISNULL(VT.FundID, 0) AS FundID
		,VT.OrderSideTagValue AS SideID
		,VT.Symbol
		,VT.CounterPartyID
		,VT.AvgPrice AS AveragePrice
		,VT.CumQty AS ExecutedQty
		,VT.Quantity AS TotalQty
		,VT.AUECID
		,VT.AssetID
		,VT.UnderlyingID
		,VT.ExchangeID
		,VT.CurrencyID
		,VT.Level2Percentage
		,VT.TaxLotQty AS AllocatedQty
		,VT.SettlementDate
		,VT.Commission AS CommissionCharged
		,VT.OtherBrokerFees AS OtherBrokerFees
		,0 AS TaxLotStateID
		,ISNULL(VT.StampDuty, 0) AS StampDuty
		,ISNULL(VT.TransactionLevy, 0) AS TransactionLevy
		,ISNULL(ClearingFee, 0) AS ClearingFee
		,ISNULL(TaxOnCommissions, 0) AS TaxOnCommissions
		,ISNULL(MiscFees, 0) AS MiscFees
		,VT.AUECLocalDate
		,VT.ProcessDate
		,VT.FXRate
		,VT.FXConversionMethodOperator
		,VT.GroupRefID
		,@thirdPartyID
		,VT.LotID
		,VT.ExternalTransID
		,VT.TradeAttribute1
		,VT.TradeAttribute2
		,VT.TradeAttribute3
		,VT.TradeAttribute4
		,VT.TradeAttribute5
		,VT.TradeAttribute6
		,VT.Description
	FROM V_Taxlots VT
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
	
	SELECT TDT.Level1AllocationID
		,ISNULL(TDT.FundID, 0) AS FundID
		,TDT.OrderSideTagValue AS SideID
		,TDT.Symbol
		,TDT.CounterPartyID
		,TDT.AvgPrice
		,TDT.CumQty AS ExecutedQty
		,--ExecutedQty                                                                
		TDT.Quantity AS TotalQty
		,--TotalQty                                                                                                                                        
		TDT.AUECID
		,TDT.AssetID
		,TDT.UnderlyingID
		,TDT.ExchangeID
		,TDT.CurrencyID
		,TDT.Level2Percentage
		,--Percentage,                                                                                     
		TDT.TaxLotQty
		,TDT.SettlementDate
		,TDT.Commission
		,TDT.OtherBrokerFees
		,TDT.TaxLotState
		,ISNULL(TDT.StampDuty, 0) AS StampDuty
		,ISNULL(TDT.TransactionLevy, 0) AS TransactionLevy
		,ISNULL(TDT.ClearingFee, 0) AS ClearingFee
		,ISNULL(TDT.TaxOnCommissions, 0) AS TaxOnCommissions
		,ISNULL(TDT.MiscFees, 0) AS MiscFees
		,TDT.AUECLocalDate
		,TDT.ProcessDate
		,TDT.FXRate AS FXRate
		,TDT.FXConversionMethodOperator AS FXConversionMethodOperator
		,TDT.GroupRefID
		,TDT.PBID
		,TDT.LotID
		,TDT.ExternalTransID
		,TDT.TradeAttribute1
		,TDT.TradeAttribute2
		,TDT.TradeAttribute3
		,TDT.TradeAttribute4
		,TDT.TradeAttribute5
		,TDT.TradeAttribute6
		,TDT.Description
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
				AND FileFormatID = @fileFormatID

	UPDATE #VT
	SET #VT.TaxLotStateID = PB.TaxlotState
	FROM #VT
	INNER JOIN T_PBWiseTaxlotState PB ON PB.TaxLotID = #VT.Level1AllocationID
	WHERE PB.TaxlotState <> 0
		AND PB.PBID = @ThirdPartyID
		AND PB.FileFormatID = @fileFormatID

	SELECT T_CompanyFunds.FundShortName AS AccountName
		,T_Side.Side AS Side
		,VT.Symbol
		,T_CounterParty.ShortName AS CounterParty
		,Sum(VT.AllocatedQty) AS OrderQty
		,VT.AveragePrice
		,VT.ExecutedQty
		,VT.TotalQty
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
		,Sum(VT.AllocatedQty) AS AllocatedQty
		,SM.PutOrCall
		,ISNULL(SM.StrikePrice, 0) AS StrikePrice
		,convert(VARCHAR, SM.ExpirationDate, 101) AS ExpirationDate
		,convert(VARCHAR, VT.SettlementDate, 101) AS SettlementDate
		,Sum(VT.CommissionCharged) AS CommissionCharged
		,Sum(VT.OtherBrokerFees) AS OtherBrokerFees
		,0 AS ThirdPartyTypeID
		,'' AS ThirdPartyTypeName
		,'' AS CVName
		,'' AS CVIdentifier
		,VT.GroupRefID
		,ISNULL(VT.TaxlotStateID, 0) AS TaxLotStateID
		,CASE 
			WHEN VT.TaxlotStateID = '0'
				OR VT.TaxlotStateID IS NULL
				THEN 'Allocated'
			WHEN VT.TaxlotStateID = '1'
				THEN 'Sent'
			WHEN VT.TaxlotStateID = '2'
				THEN 'Amemded'
			WHEN VT.TaxlotStateID = '3'
				THEN 'Deleted'
			WHEN VT.TaxlotStateID = '4'
				THEN 'Ignored'
			END AS TaxLotState
		,Sum(ISNULL(VT.StampDuty, 0)) AS StampDuty
		,Sum(ISNULL(VT.TransactionLevy, 0)) AS TransactionLevy
		,Sum(ISNULL(ClearingFee, 0)) AS ClearingFee
		,Sum(ISNULL(TaxOnCommissions, 0)) AS TaxOnCommissions
		,Sum(ISNULL(MiscFees, 0)) AS MiscFees
		,convert(VARCHAR, VT.AUECLocalDate, 101) AS TradeDate
		,ISNULL(SM.Multiplier, 1) AS AssetMultiplier
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
		,VT.ProcessDate
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
		,VT.FXRate
		,VT.FXConversionMethodOperator
		,VT.GroupRefID
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
	FROM #VT VT
	LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
	LEFT JOIN T_Side ON dbo.T_Side.SideTagValue = VT.SideID
	LEFT JOIN T_CompanyFunds ON T_CompanyFunds.CompanyFundID = VT.FundID
	LEFT JOIN T_Asset ON T_Asset.AssetID = VT.AssetID
	LEFT JOIN T_CounterParty ON T_CounterParty.CounterPartyID = VT.CounterPartyID
	LEFT JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID
	LEFT JOIN T_Country ON dbo.T_Country.CountryID = T_Exchange.Country
	LEFT OUTER JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol
	WHERE (
			datediff(d, (
					CASE 
						WHEN @dateType = 1
							THEN VT.AUECLocalDate
						ELSE VT.ProcessDate
						END
					), @inputdate) = 0
			--and CTPM.InternalFundNameID_FK in (select FundID from @Fund)                                               
			OR (
				(
					VT.TaxLotStateID = 2
					OR VT.TaxLotStateID = 3
					)
				AND datediff(d, (
						CASE 
							WHEN @dateType = 1
								THEN VT.AUECLocalDate
							ELSE VT.ProcessDate
							END
						), @inputdate) >= 0
				)
			)
		AND (
			VT.TaxLotStateID <> 4
			OR (
				VT.TaxLotStateID = 4
				AND datediff(d, (
						CASE 
							WHEN @dateType = 1
								THEN VT.AUECLocalDate
							ELSE VT.ProcessDate
							END
						), @inputdate) = 0
				)
			)
		AND VT.FundID IN (
			SELECT FundID
			FROM @Fund
			)
		AND VT.AUECID IN (
			SELECT AUECID
			FROM @AUECID
			)
		AND VT.PBID = @thirdPartyID
	GROUP BY                                                                                                
		VT.Level1AllocationID
		,T_CompanyFunds.FundShortName
		,T_Side.Side
		,VT.Symbol
		,T_CounterParty.ShortName
		,VT.AveragePrice
		,VT.ExecutedQty
		,VT.TotalQty
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
		,VT.TaxLotStateID
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
		,VT.GroupRefID
		,VT.ProcessDate
		,T_Country.CountryName
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
	ORDER BY VT.GroupRefID
END