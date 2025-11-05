/*                                                                                          
Select * from T_ThirdPArty                                                                                                   
Select * from V_TaxLots                                                                                                        
P_FFGetThirdPartyLevel2AllocationExportOnly 26,1184,'8/11/2008 12:00:00 AM',5                                                                                                         
Created by : <Sandeep Singh>                                                                                                                        
Date : <08-09-2008>                                                                                                                        
purpose: <To get the L2 data>                   
                  
P_FFGetThirdPartyLevel2AllocationExportOnly 23,'1184,1183,1182,1186,1185','1/25/2010 3:51:24 PM',5,'1,20,21,18,1,15,12,16,33',1,0,0                    
                  
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
CREATE PROCEDURE [dbo].[P_FFGetThirdPartyLevel2AllocationExportOnly] (
	@thirdPartyID INT
	,@companyFundIDs VARCHAR(max)
	,@inputDate DATETIME
	,@companyID INT
	,@auecIDs VARCHAR(max)
	,@TypeID INT
	,-- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties        
	@dateType INT -- 0 for Process Date and 1 for Trade Date                                                                                                                 
	,@fileFormatID INT
	,@includeSent BIT = 1
	)
AS
--declare	@thirdPartyID INT
--declare	@companyFundIDs VARCHAR(max)
--declare @inputDate DATETIME
--declare	@auecIDs VARCHAR(max)
--declare	@TypeID INT                         
--declare	@dateType INT                                                                                                                                                           
--declare	@fileFormatID INT
--declare @includeSent INT
--set @thirdPartyID=70
--set @companyFundIDs=N'28,29,30,31,27,'
--set @inputDate='2019-06-11 16:08:16'
--set @auecIDs=N'71,63,34,43,1,15,11,62,73,12,80,32,88,81,'
--set @TypeID=1
--set @dateType=1
--set @fileFormatID=189
--set @includeSent=1

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

SELECT DISTINCT VT.TaxLotID
	,ISNULL(VT.FundID, 0) AS FundID
	,ISNULL(T_OrderType.OrderTypesID, 0) AS OrderTypesID
	,ISNULL(T_OrderType.OrderTypes, 'Multiple') AS OrderTypes
	,VT.OrderSideTagValue AS SideID
	,T_Side.Side AS Side
	,VT.Symbol
	,VT.CounterPartyID
	,VT.VenueID
	,VT.TaxLotQty AS OrderQty
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
	,VT.TaxLotID
	,VT.Level2Percentage
	,VT.TaxLotQty
	,'' AS IsBasketGroup
	,SM.PutOrCall
	,SM.StrikePrice
	,SM.ExpirationDate
	,VT.SettlementDate
	,VT.Commission
	,VT.OtherBrokerFees
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
	,IsNull(VT.StampDuty, 0)
	,IsNull(VT.TransactionLevy, 0)
	,IsNull(VT.ClearingFee, 0)
	,IsNull(VT.TaxOnCommissions, 0)
	,IsNull(VT.MiscFees, 0)
	,VT.AUECLocalDate
	,SM.Multiplier
	,VT.Level2ID
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
	,SM.AccrualBasisID
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
	,IsNull(VT.SecFee, 0)
	,IsNull(VT.OccFee, 0)
	,IsNull(VT.OrfFee, 0)
	,VT.ClearingBrokerFee
	,VT.SoftCommission
	,VT.TransactionType
	,TC.CurrencySymbol AS SettlCurrency
	,VT.ChangeType
	----------Start of Dynamic UDA---------------
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
----------End of Dynamic UDA---------------        
FROM V_TaxLots VT
INNER JOIN @Fund Fund ON Fund.FundID = VT.FundID
INNER JOIN @AUECID auec ON auec.AUECID = VT.AUECID
LEFT OUTER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID
Inner Join #Temp_CounterPartyID CP On CP.CounterPartyID = VT.CounterPartyID
INNER JOIN T_FundType AS FT ON FT.FundTypeID = CTPM.FundTypeID_FK
LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
LEFT JOIN T_Currency AS TC ON TC.CurrencyID = VT.SettlCurrency_Taxlot
LEFT JOIN T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue
LEFT JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID
LEFT JOIN T_Country ON dbo.T_Country.CountryID = T_Exchange.Country
LEFT JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue
INNER JOIN dbo.T_CompanyThirdParty ON T_CompanyThirdParty.CompanyThirdPartyID = CTPM.CompanyThirdPartyID_FK
INNER JOIN dbo.T_ThirdParty ON T_ThirdParty.ThirdPartyId = T_CompanyThirdParty.ThirdPartyId
INNER JOIN dbo.T_ThirdPartyType ON T_ThirdPartyType.ThirdPartyTypeId = T_ThirdParty.ThirdPartyTypeID
	AND T_CompanyThirdParty.CompanyID = @companyID
LEFT OUTER JOIN dbo.T_CompanyThirdPartyFlatFileSaveDetails CTPFD ON CTPFD.CompanyThirdPartyID = CTPM.CompanyThirdPartyID_FK
LEFT OUTER JOIN T_CounterPartyVenue ON T_CounterPartyVenue.CounterPartyID = VT.CounterPartyID
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
ORDER BY GroupRefID

Drop Table #Temp_CounterPartyID