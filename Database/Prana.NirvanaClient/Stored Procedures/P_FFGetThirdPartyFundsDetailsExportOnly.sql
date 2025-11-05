/*                                                                                                                      
        
Modified by : <Sandeep Singh>                                                                                                                        
Date : <08-19-2012>                     
DESC: FX Rate and FX Conversion Method Operator at Taxlot Level Parameters added   

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
CREATE PROCEDURE [dbo].[P_FFGetThirdPartyFundsDetailsExportOnly] (
	@thirdPartyID INT
	,@companyFundIDs VARCHAR(max)
	,@inputDate DATETIME
	,@companyID INT
	,@auecIDs VARCHAR(max)
	,@TypeID INT
	,-- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties       
	@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                      
	,@fileFormatID INT
	,@includeSent BIT = 1
	)
AS

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

SELECT VT.Level1AllocationID
	,ISNULL(VT.FundID, 0) AS FundID
	,ISNULL(T_OrderType.OrderTypesID, 0) AS OrderTypesID
	,ISNULL(T_OrderType.OrderTypes, 'Multiple') AS OrderTypes
	,VT.OrderSideTagValue AS SideID
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
	,VT.Level1AllocationID
	,sum(VT.Level2Percentage)
	,sum(VT.TaxLotQty)
	,' ' AS IsBasketGroup
	,SM.PutOrCall
	,SM.StrikePrice
	,SM.ExpirationDate
	,VT.SettlementDate
	,sum(VT.Commission)
	,sum(VT.OtherBrokerFees)
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
	,0 AS TaxLotState
	,sum(ISNULL(VT.StampDuty, 0)) AS StampDuty
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
	,sum(ISNULL(VT.SecFee, 0)) AS SecFee
	,sum(ISNULL(VT.OccFee, 0)) AS OccFee
	,sum(ISNULL(VT.OrfFee, 0)) AS OrfFee
	,sum(VT.ClearingBrokerFee)
	,sum(VT.SoftCommission)
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
	,VWAP
----------Dynamic UDA---------------
FROM V_TaxLots VT
INNER JOIN @Fund Fund ON Fund.FundID = VT.FundID
INNER JOIN @AUECID auec ON auec.AUECID = VT.AUECID
Inner Join #Temp_CounterPartyID CP On CP.CounterPartyID = VT.CounterPartyID
LEFT OUTER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID
INNER JOIN T_FundType AS FT ON FT.FundTypeID = CTPM.FundTypeID_FK
LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
LEFT JOIN T_Currency AS TC ON TC.CurrencyID = VT.SettlCurrency_Taxlot
LEFT JOIN T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue
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
LEFT OUTER JOIN V_UDA_DynamicUDA UDA ON UDA.Symbol_PK = SM.Symbol_PK
LEFT OUTER JOIN PM_DailyVWAP AS DV ON VT.Symbol = DV.Symbol AND DATEDIFF(d,VT.AUECLocalDate,DV.Date) = 0 AND VWAP != 0
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
		OR TransactionSource = 13
		)
GROUP BY VT.GroupID
	,VT.FundID
	,
	--VT.TaxlotID,                                  
	VT.Level1AllocationID
	,T_OrderType.OrderTypesID
	,T_OrderType.OrderTypes
	,VT.OrderSideTagValue
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
	,VT.TransactionType
	,VT.SettlCurrency_Taxlot
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

Drop Table #Temp_CounterPartyID