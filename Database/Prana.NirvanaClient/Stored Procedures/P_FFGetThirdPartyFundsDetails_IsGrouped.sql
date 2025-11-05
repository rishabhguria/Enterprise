/***********************************************
Created by: Suraj nataraj
Script Date: 09/08/2015
Desc: For adding a column to show if a trade is auto-grouped
http://jira.nirvanasolutions.com:8080/browse/PRANA-6398
*************************************************/
CREATE PROCEDURE [dbo].[P_FFGetThirdPartyFundsDetails_IsGrouped] (
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
DECLARE @GroupedIDs TABLE (GroupID VARCHAR(20));

WITH SecMasterCTE (
	Symbol
	,[GroupID]
	,Ranking
	)
AS (
	SELECT Symbol
		,[GroupID]
		,Ranking = DENSE_RANK() OVER (
			PARTITION BY [GroupID] ORDER BY NEWID() ASC
			)
	FROM T_TradedOrders
	)
INSERT INTO @GroupedIDs
SELECT cast(GroupID AS VARCHAR)
FROM SecMasterCTE
WHERE Ranking > 1
GROUP BY GroupID
	,Symbol

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
	,--Percentage,                                                                                 
	TaxLotQty FLOAT
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
	,IsOldData BIT
	,SecFee FLOAT
	,OccFee FLOAT
	,OrfFee FLOAT
	,ClearingBrokerFee FLOAT
	,SoftCommission FLOAT
	,TransactionType VARCHAR(200)
	,
	-------------------Suraj---------------------
	GroupID VARCHAR(20)
	,IsGrouped INT
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
	,--AllocatedQty                                                                                                                                
	VT.AvgPrice
	,VT.CumQty
	,--ExecutedQty                                                                                                        
	VT.Quantity
	,--TotalQty                                                                                     
	VT.AUECID
	,VT.AssetID
	,VT.UnderlyingID
	,VT.ExchangeID
	,VT.CurrencyID
	,VT.Level1AllocationID AS Level1AllocationID
	,(VT.Level2Percentage)
	,--Percentage,                                                                     
	(VT.TaxLotQty) AS OrderQty
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
	,isnull(VT.BenchMarkRate, 0)
	,isnull(VT.Differential, 0)
	,VT.SwapDescription
	,isnull(VT.DayCount, 0)
	,isnull(VT.FirstResetDate, '01/01/1800')
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
	,CASE 
		WHEN PB.FileFormatID = 0
			THEN 1
		ELSE 0
		END AS IsOldData
	,ISNULL(VT.SecFee, 0) AS SecFee
	,ISNULL(VT.OccFee, 0) AS OccFee
	,ISNULL(VT.OrfFee, 0) AS OrfFee
	,VT.ClearingBrokerFee
	,VT.SoftCommission
	,VT.TransactionType
	,
	------------------Suraj-------------------
	VT.GroupID AS GroupID
	,(
		SELECT count(*)
		FROM @GroupedIDs
		WHERE GroupID = VT.GroupID
		) AS IsGrouped
FROM V_TaxLots VT
INNER JOIN T_PBWiseTaxlotState PB  with (nolock) ON PB.TaxlotID = VT.TaxlotID
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
		)

UNION ALL

SELECT TDT.Level1AllocationID AS TaxlotID
	,ISNULL(TDT.FundID, 0) AS FundID
	,TDT.OrderTypeTagValue
	,TDT.OrderSideTagValue AS SideID
	,TDT.Symbol
	,TDT.CounterPartyID
	,TDT.VenueID
	,(TDT.TaxLotQty) AS OrderQty
	,--AllocatedQty                                                                                                                          
	TDT.AvgPrice
	,TDT.CumQty
	,--ExecutedQty                                                                  
	TDT.Quantity
	,--TotalQty                                                                                                                                          
	TDT.AUECID
	,TDT.AssetID
	,TDT.UnderlyingID
	,TDT.ExchangeID
	,TDT.CurrencyID
	,TDT.Level1AllocationID AS Level1AllocationID
	,(TDT.Level2Percentage)
	,--Percentage,                                                                                       
	(TDT.TaxLotQty)
	,' ' AS IsBasketGroup
	,--IsBasketGroup,                                                                                                                                           
	TDT.SettlementDate
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
	,isnull(TDT.BenchMarkRate, 0)
	,isnull(TDT.Differential, 0)
	,TDT.SwapDescription
	,isnull(TDT.DayCount, 0)
	,isnull(TDT.FirstResetDate, '01/01/1800')
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
	,CASE 
		WHEN TDT.FileFormatID = 0
			THEN 1
		ELSE 0
		END AS IsOldData
	,(ISNULL(TDT.SecFee, 0)) AS SecFee
	,(ISNULL(TDT.OccFee, 0)) AS OccFee
	,(ISNULL(TDT.OrfFee, 0)) AS OrfFee
	,(TDT.ClearingBrokerFee)
	,(TDT.SoftCommission)
	,TDT.TransactionType
	,
	------------------Suraj-------------------
	TDT.GroupID AS GroupID
	,(
		SELECT count(*)
		FROM @GroupedIDs
		WHERE GroupID = TDT.GroupID
		) AS IsGrouped
FROM T_DeletedTaxLots TDT
WHERE (
		FileFormatID = @fileFormatID
		OR FileFormatID = 0
		)
	AND TDT.TaxLotState = 3

SELECT DISTINCT TaxlotID
INTO #temp_OLD
FROM #VT
WHERE IsOldData = 1

SELECT *
INTO #temp_NEW
FROM #VT
WHERE IsOldData = 0

IF (@TypeID = 0)
BEGIN
	SELECT VT.TaxlotID AS TaxlotID
		,ISNULL(VT.FundID, 0) AS FundID
		,ISNULL(T_OrderType.OrderTypesID, 0) AS OrderTypesID
		,ISNULL(T_OrderType.OrderTypes, 'Multiple') AS OrderTypes
		,VT.SideID
		,T_Side.Side AS Side
		,VT.Symbol
		,C.ShortName
		,VT.VenueID
		,Sum(VT.TaxLotQty) AS AllocatedQty
		,--AllocatedQty                                                                                                                                
		VT.AvgPrice
		,VT.CumQty
		,--ExecutedQty                                                                                                                                          
		VT.Quantity
		,--TotalQty                                                                                                                                          
		VT.AUECID
		,VT.AssetID
		,VT.UnderlyingID
		,T_Exchange.DisplayName
		,Currency.CurrencyID
		,Currency.CurrencyName
		,Currency.CurrencySymbol
		,CTPM.MappedName
		,CTPM.FundAccntNo
		,CTPM.FundTypeID_FK
		,FT.FundTypeName
		,VT.Level1AllocationID AS Level1AllocationID
		,Sum(VT.Level2Percentage) AS Level2Percentage
		,--Percentage,       
		--Sum(VT.TaxLotQty),                                                                         
		'' AS IsBasketGroup
		,SM.PutOrCall
		,SM.StrikePrice
		,SM.ExpirationDate
		,VT.SettlementDate
		,Sum(VT.Commission) AS Commission
		,Sum(VT.OtherBrokerFees) AS OtherBrokerFees
		,T_ThirdPartyType.ThirdPartyTypeID
		,T_ThirdPartyType.ThirdPartyTypeName
		,CTPFD.CompanyIdentifier
		,0 AS SecFee
		,ISNULL(T_CounterPartyVenue.DisplayName, '') AS CVName
		,ISNULL(T_CompanyThirdPartyCVIdentifier.CVIdentifier, '') CVIdentifier
		,
		--T_CompanyCounterPartyVenues.CompanyCounterPartyCVID as CompanyCounterPartyCVID,                                                                              
		VT.GroupRefID
		,
		--VT.TaxLotState,     
		CASE 
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
		,SM.BondTypeID AS BondTypeID
		,
		--Swap Parameters         
		isnull(VT.BenchMarkRate, 0) AS BenchMarkRate
		,isnull(VT.Differential, 0) AS Differential
		,VT.SwapDescription
		,isnull(VT.DayCount, 0) AS DayCount
		,isnull(VT.FirstResetDate, '01/01/1800') AS FirstResetDate
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
		,Sum(VT.ClearingBrokerFee) AS ClearingBrokerFee
		,Sum(VT.SoftCommission) AS SoftCommission
		,SUM((VT.TaxLotQty * VT.AvgPrice * isnull(SM.Multiplier, 1)) + ISNULL((VT.SecFee + VT.OccFee + VT.OrfFee + VT.ClearingBrokerFee + VT.SoftCommission + MiscFees + TaxOnCommissions + ClearingFee + VT.TransactionLevy + VT.StampDuty + VT.OtherBrokerFees + VT.Commission), 0)) AS NetAmount
		,VT.TransactionType
		,
		------------------Suraj-------------------
		VT.GroupID AS GroupID
		,(
			SELECT count(*)
			FROM @GroupedIDs
			WHERE GroupID = VT.GroupID
			) AS IsGrouped
	FROM #VT VT
	INNER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID
	INNER JOIN T_FundType AS FT ON FT.FundTypeID = CTPM.FundTypeID_FK
	LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
	LEFT JOIN T_Side ON dbo.T_Side.SideTagValue = VT.SideID
	LEFT JOIN T_CounterParty C ON C.CounterPartyID = VT.CounterPartyID
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
		AND VT.AUECID IN (
			SELECT AUECID
			FROM @AUECID
			)
		AND VT.PBID = @thirdPartyID
	GROUP BY VT.TaxlotID
		,VT.Level1AllocationID
		,VT.FundID
		,T_OrderType.OrderTypesID
		,T_OrderType.OrderTypes
		,VT.SideID
		,T_Side.Side
		,VT.Symbol
		,C.ShortName
		,VT.VenueID
		,VT.AvgPrice
		,VT.CumQty
		,VT.Quantity
		,VT.AUECID
		,VT.AssetID
		,VT.UnderlyingID
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
		,
		--T_CompanyCounterPartyVenues.CompanyCounterPartyCVID,               
		VT.GroupRefID
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
		,VT.GroupID
		,VT.IsGrouped
	ORDER BY GroupRefID
END
ELSE
BEGIN
	SELECT VT.Level1AllocationID AS TaxlotID
		,ISNULL(VT.FundID, 0) AS FundID
		,ISNULL(T_OrderType.OrderTypesID, 0) AS OrderTypesID
		,ISNULL(T_OrderType.OrderTypes, 'Multiple') AS OrderTypes
		,VT.SideID
		,T_Side.Side AS Side
		,VT.Symbol
		,C.ShortName
		,VT.VenueID
		,Sum(VT.TaxLotQty) AS AllocatedQty
		,--AllocatedQty                                                                                                                                
		VT.AvgPrice
		,VT.CumQty
		,--ExecutedQty                                                                                                                                          
		VT.Quantity
		,--TotalQty                                                                                                       
		VT.AUECID
		,VT.AssetID
		,VT.UnderlyingID
		,T_Exchange.DisplayName
		,Currency.CurrencyID
		,Currency.CurrencyName
		,Currency.CurrencySymbol
		,CTPM.MappedName
		,CTPM.FundAccntNo
		,CTPM.FundTypeID_FK
		,'' AS FundTypeName
		,--FT.FundTypeName,                                                                                                                                           
		VT.Level1AllocationID AS Level1AllocationID
		,Sum(VT.Level2Percentage) AS Level2Percentage
		,--Percentage,                                                                                                                      
		--Sum(VT.TaxLotQty),                                                                                                                                     
		'' AS IsBasketGroup
		,SM.PutOrCall
		,SM.StrikePrice
		,SM.ExpirationDate
		,VT.SettlementDate
		,Sum(VT.Commission) AS Commission
		,Sum(VT.OtherBrokerFees) AS OtherBrokerFees
		,0 AS ThirdPartyTypeID
		,--T_ThirdPartyType.ThirdPartyTypeID,                       
		'' AS ThirdPartyTypeName
		,--T_ThirdPartyType.ThirdPartyTypeName,                                                                                   
		'' AS CompanyIdentifier
		,--CTPFD.CompanyIdentifier,                                                                                 
		0 AS SecFee
		,'' AS CVName
		,--ISNULL(T_CounterPartyVenue.DisplayName,'')as CVName,                                                                                                                                      
		'' AS CVIdentifier
		,--ISNULL(T_CompanyThirdPartyCVIdentifier.CVIdentifier,'')  CVIdentifier,                                                                                                                            
		--0 as CompanyCounterPartyCVID,--T_CompanyCounterPartyVenues.CompanyCounterPartyCVID as CompanyCounterPartyCVID,                                                                                                                      
		VT.GroupRefID
		,
		--VT.TaxLotState, 
		CASE 
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
		,'No' AS FromDeleted
		,VT.ProcessDate
		,VT.OriginalPurchaseDate
		,VT.AccruedInterest
		,
		-- Reserved for future use                                
		'' AS Comment1
		,'' AS Comment2
		,
		--Fixed Income members                              
		SM.Coupon
		,SM.IssueDate
		,SM.FirstCouponDate
		,SM.CouponFrequencyID
		,Sm.AccrualBasisID
		,SM.BondTypeID AS BondTypeID
		,
		--Swap Parameters                              
		isnull(VT.BenchMarkRate, 0) AS BenchMarkRate
		,isnull(VT.Differential, 0) AS Differential
		,VT.SwapDescription
		,isnull(VT.DayCount, 0) AS DayCount
		,isnull(VT.FirstResetDate, '01/01/1800') AS FirstResetDate
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
		,Sum(VT.ClearingBrokerFee) AS ClearingBrokerFee
		,Sum(VT.SoftCommission) AS SoftCommission
		,SUM((VT.TaxLotQty * VT.AvgPrice * isnull(SM.Multiplier, 1)) + ISNULL((VT.SecFee + VT.OccFee + VT.OrfFee + VT.ClearingBrokerFee + VT.SoftCommission + MiscFees + TaxOnCommissions + ClearingFee + VT.TransactionLevy + VT.StampDuty + VT.OtherBrokerFees + VT.Commission), 0)) AS NetAmount
		,VT.TransactionType
		,
		------------------Suraj-------------------
		VT.GroupID AS GroupID
		,(
			SELECT count(*)
			FROM @GroupedIDs
			WHERE GroupID = VT.GroupID
			) AS IsGrouped
	FROM #temp_NEW VT
	INNER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID
	LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
	LEFT JOIN T_Side ON dbo.T_Side.SideTagValue = VT.SideID
	LEFT JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue
	LEFT JOIN T_CounterParty C ON C.CounterPartyID = VT.CounterPartyID
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
		AND CTPM.InternalFundNameID_FK IN (
			SELECT FundID
			FROM @Fund
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
		--  VT.TaxlotID,                                                                                      
		VT.Level1AllocationID
		,VT.FundID
		,T_OrderType.OrderTypesID
		,T_OrderType.OrderTypes
		,VT.SideID
		,T_Side.Side
		,VT.Symbol
		,C.ShortName
		,VT.VenueID
		,VT.AvgPrice
		,VT.CumQty
		,VT.Quantity
		,VT.AUECID
		,VT.AssetID
		,VT.UnderlyingID
		,T_Exchange.DisplayName
		,Currency.CurrencyID
		,Currency.CurrencyName
		,Currency.CurrencySymbol
		,CTPM.MappedName
		,CTPM.FundAccntNo
		,CTPM.FundTypeID_FK
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
		,VT.GroupID
		,VT.IsGrouped
	
	UNION ALL
	
	SELECT VT.Level1AllocationID AS TaxlotID
		,ISNULL(VT.FundID, 0) AS FundID
		,ISNULL(T_OrderType.OrderTypesID, 0) AS OrderTypesID
		,ISNULL(T_OrderType.OrderTypes, 'Multiple') AS OrderTypes
		,VT.SideID
		,T_Side.Side AS Side
		,VT.Symbol
		,C.ShortName
		,VT.VenueID
		,Sum(VT.TaxLotQty) AS AllocatedQty
		,--AllocatedQty                                 
		VT.AvgPrice
		,VT.CumQty
		,--ExecutedQty                                                                                                                                        
		VT.Quantity
		,--TotalQty                                                             
		VT.AUECID
		,VT.AssetID
		,VT.UnderlyingID
		,T_Exchange.DisplayName
		,Currency.CurrencyID
		,Currency.CurrencyName
		,Currency.CurrencySymbol
		,'' AS MappedName
		,--CTPM.MappedName,                                                
		'' AS FundAccntNo
		,--CTPM.FundAccntNo,                                                         
		0 AS FundTypeID_FK
		,--CTPM.FundTypeID_FK,                                                                                 
		'' AS FundTypeName
		,--FT.FundTypeName,                                                                                                                                         
		VT.Level1AllocationID AS Level1AllocationID
		,Sum(VT.Level2Percentage) AS Level2Percentage
		,--Percentage,                                                                                                                    
		--Sum(VT.TaxLotQty),                                                                                                                                   
		'' AS IsBasketGroup
		,SM.PutOrCall
		,SM.StrikePrice
		,SM.ExpirationDate
		,VT.SettlementDate
		,Sum(VT.Commission) AS Commission
		,Sum(VT.OtherBrokerFees) AS OtherBrokerFees
		,0 AS ThirdPartyTypeID
		,--T_ThirdPartyType.ThirdPartyTypeID,                                                                 
		'' AS ThirdPartyTypeName
		,--T_ThirdPartyType.ThirdPartyTypeName,                                                                                 
		'' AS CompanyIdentifier
		,--CTPFD.CompanyIdentifier,                                                                               
		0 AS SecFee
		,'' AS CVName
		,--ISNULL(T_CounterPartyVenue.DisplayName,'')as CVName,                                                                                                                                    
		'' AS CVIdentifier
		,--ISNULL(T_CompanyThirdPartyCVIdentifier.CVIdentifier,'')  CVIdentifier,                                                                                                                          
		--0 as CompanyCounterPartyCVID,--T_CompanyCounterPartyVenues.CompanyCounterPartyCVID as CompanyCounterPartyCVID,                                                                                                                    
		VT.GroupRefID
		,
		--VT.TaxLotState,    
		CASE 
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
		,'No' AS FromDeleted
		,VT.ProcessDate
		,VT.OriginalPurchaseDate
		,VT.AccruedInterest
		,
		-- Reserved for future use                              
		'' AS Comment1
		,'' AS Comment2
		,
		--Fixed Income members                            
		SM.Coupon
		,SM.IssueDate
		,SM.FirstCouponDate
		,SM.CouponFrequencyID
		,Sm.AccrualBasisID
		,SM.BondTypeID AS BondTypeID
		,
		--Swap Parameters                            
		isnull(VT.BenchMarkRate, 0) AS BenchMarkRate
		,isnull(VT.Differential, 0) AS Differential
		,VT.SwapDescription
		,isnull(VT.DayCount, 0) AS DayCount
		,isnull(VT.FirstResetDate, '01/01/1800') AS FirstResetDate
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
		,Sum(VT.ClearingBrokerFee) AS ClearingBrokerFee
		,Sum(VT.SoftCommission) AS SoftCommission
		,SUM((VT.TaxLotQty * VT.AvgPrice * isnull(SM.Multiplier, 1)) + ISNULL((VT.SecFee + VT.OccFee + VT.OrfFee + VT.ClearingBrokerFee + VT.SoftCommission + MiscFees + TaxOnCommissions + ClearingFee + VT.TransactionLevy + VT.StampDuty + VT.OtherBrokerFees + VT.Commission), 0)) AS NetAmount
		,VT.TransactionType
		,
		------------------Suraj-------------------
		VT.GroupID AS GroupID
		,(
			SELECT count(*)
			FROM @GroupedIDs
			WHERE GroupID = VT.GroupID
			) AS IsGrouped
	FROM #VT VT
	INNER JOIN #temp_OLD ON #temp_OLD.TaxlotID = VT.TaxLotID
	LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
	LEFT JOIN T_Side ON dbo.T_Side.SideTagValue = VT.SideID
	LEFT JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue
	LEFT JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID
	LEFT JOIN T_CounterParty C ON C.CounterPartyID = VT.CounterPartyID
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
		,T_OrderType.OrderTypesID
		,T_OrderType.OrderTypes
		,VT.SideID
		,T_Side.Side
		,VT.Symbol
		,C.ShortName
		,VT.VenueID
		,VT.AvgPrice
		,VT.CumQty
		,VT.Quantity
		,VT.AUECID
		,VT.AssetID
		,VT.UnderlyingID
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
		,SM.Coupon
		,SM.IssueDate
		,SM.FirstCouponDate
		,SM.CouponFrequencyID
		,Sm.AccrualBasisID
		,SM.BondTypeID
		,
		--Swap Parameters                              
		VT.BenchMarkRate
		,isnull(VT.Differential, 0)
		,VT.SwapDescription
		,isnull(VT.DayCount, 0)
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
		,VT.GroupID
		,VT.IsGrouped
	ORDER BY GroupRefID
END

DROP TABLE #VT
	,#temp_OLD
	,#temp_NEW