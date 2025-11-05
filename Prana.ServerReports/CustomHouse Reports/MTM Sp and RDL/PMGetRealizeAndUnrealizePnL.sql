    
/******************************************************************      
                                                                                                                                                                                                                                             
Author:  <Author: Sandeep>          
Create date: Create Date: 17-Oct-2008                                 
Description: Description: To get Realize and UnRealized PnL for the Date Ranges                                                             
  
Modified Date: 24-April-2012                                                                  
Modified By : Sandeep Singh                                                                                                                                      
Description: FX Spot, FX Forward and Fixed Income related changes       
      
Modified Date: 06-MAY-2013                                                                  
Modified By : Ankit                                                                                                                                      
Description: Removed all configurable parameters and added in Table T_ReportPreferences      
    
Modified Date: 03-April-2014                                                                  
Modified By : Ankit                                                                                                                                      
Description: Added Trade Attributed for both Closed and Open Trades    
  
Modified Date: 13-March-2015                                                                  
Modified By : Narendra Kumar Jangir                                                                                                                                      
Description: Fund wise mark price, Fund wise fx rate and fund wise base currency handling 

Modified Date: 10-October-2015                                                                      
Modified By : Pankaj Sharma
Description: New Column added Commission in the SP 
  
exec [PMGetRealizeAndUnrealizePnL] '10/1/2013','4/07/2015' ,'1182,1183,1184','1,2,3,4,5,6,7,8,9','MTM_V0','','Symbol',1                                                                                          
                         
******************************************************************/                          
CREATE PROCEDURE  [dbo].[PMGetRealizeAndUnrealizePnL]                                                                           
(                                                                                                                                                                                                                                                
 -- Add the parameters for the stored procedure here                                
 @StartDate datetime,       
 @EndDate datetime,      
 @Fund Varchar(max),      
 @Asset varchar(max),      
 @ReportId Varchar(100) ,          
 @SearchString Varchar(5000) ,                      
 @SearchBy Varchar(100),          
 @NAVOfCurrentOrPriorDay int              
)  with Recompile                                                                                                                                                                                                                                              
AS                                                                                                                                                                                                                                                
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from                                        
-- interfering with SELECT statements.                                                         
SET NOCOUNT ON;

--Declare @StartDate datetime                                
--Declare @EndDate datetime                                
--Declare @Fund varchar(max)                              
--Declare @Asset varchar(max)               
--Declare @ReportId varchar(max)                                      
--Declare @SearchString Varchar(5000)
--Declare @SearchBy Varchar(100)
--Declare @NAVOfCurrentOrPriorDay Int
--                                    
--Set @StartDate='08-18-2015'                                      
--Set @EndDate='08-18-2015'                
--Set @Fund = '1265'              
--Set @Asset= '1'              
--Set @ReportId = 'MTM_V0'                   
--Set @SearchString = '' 
--Set @SearchBy  ='Symbol'  
--Set @NAVOfCurrentOrPriorDay = 0              

-----------------------------------------------------------------------------------              
SELECT
	* INTO #TempSymbol
FROM dbo.split(@SearchString, ',')

SELECT DISTINCT
	* INTO #Symbol
FROM #TempSymbol

DECLARE @ShowFutureMktValueAsUnrealizedOrZero int
SET @ShowFutureMktValueAsUnrealizedOrZero = (SELECT
	FutureMV_ZeroOrEndingMVOrUnrealized
FROM T_ReportPreferences
WHERE ReportId = @ReportId)

DECLARE @ShowInternationalFutureOptionMktValueAsMVorUnrealizedOrZero int
SET @ShowInternationalFutureOptionMktValueAsMVorUnrealizedOrZero = (SELECT
	InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized
FROM T_ReportPreferences
WHERE ReportId = @ReportId)

DECLARE @ShowFXMktValueAsUnrealizedOrZero int
SET @ShowFXMktValueAsUnrealizedOrZero = (SELECT
	FXMV_ZeroOrEndingMVOrUnrealized
FROM T_ReportPreferences
WHERE ReportId = @ReportId)

DECLARE @SwapMV_ZeroOrEndingMVOrUnrealized int
SET @SwapMV_ZeroOrEndingMVOrUnrealized = (SELECT
	SwapMV_ZeroOrEndingMVOrUnrealized
FROM T_ReportPreferences
WHERE ReportId = @ReportId)

-----------------------------------------------------------------------------------        
DECLARE @IncludeFXPNLinEquity bit
SET @IncludeFXPNLinEquity = (SELECT
	IncludeFXPNLinEquity
FROM T_ReportPreferences
WHERE ReportId = @ReportId)

DECLARE @IncludeFXPNLinEquityOption bit
SET @IncludeFXPNLinEquityOption = (SELECT
	IncludeFXPNLinEquityOption
FROM T_ReportPreferences
WHERE ReportId = @ReportId)

DECLARE @FuturePNLWithBothOrEndFXRate bit
SET @FuturePNLWithBothOrEndFXRate = (SELECT
	IncludeFXPNLinFutures
FROM T_ReportPreferences
WHERE ReportId = @ReportId)

DECLARE @InternationalFutureOptionPNLWithBothOrEndFXRate bit
SET @InternationalFutureOptionPNLWithBothOrEndFXRate = (SELECT
	IncludeFXPNLinInternationalFutOptions
FROM T_ReportPreferences
WHERE ReportId = @ReportId)

DECLARE @IncludeFXPNLinFX bit
SET @IncludeFXPNLinFX = (SELECT
	IncludeFXPNLinFX
FROM T_ReportPreferences
WHERE ReportId = @ReportId)

DECLARE @IncludeFXPNLinSwaps bit
SET @IncludeFXPNLinSwaps = (SELECT
	IncludeFXPNLinSwaps
FROM T_ReportPreferences
WHERE ReportId = @ReportId)

DECLARE @IncludeFXPNLinOther bit
SET @IncludeFXPNLinOther = (SELECT
	IncludeFXPNLinOther
FROM T_ReportPreferences
WHERE ReportId = @ReportId)

-----------------------------------------------------------------------------------        
DECLARE @IncludeCommissionInPNL_Equity bit
SET @IncludeCommissionInPNL_Equity = (SELECT
	IncludeCommissionInPNL_Equity
FROM T_ReportPreferences
WHERE ReportId = @ReportId)

DECLARE @IncludeCommissionInPNL_EquityOption bit
SET @IncludeCommissionInPNL_EquityOption = (SELECT
	IncludeCommissionInPNL_EquityOption
FROM T_ReportPreferences
WHERE ReportId = @ReportId)

DECLARE @IncludeCommissionInPNL_Futures bit
SET @IncludeCommissionInPNL_Futures = (SELECT
	IncludeCommissionInPNL_Futures
FROM T_ReportPreferences
WHERE ReportId = @ReportId)

DECLARE @IncludeCommissionInPNL_FutOptions bit
SET @IncludeCommissionInPNL_FutOptions = (SELECT
	IncludeCommissionInPNL_FutOptions
FROM T_ReportPreferences
WHERE ReportId = @ReportId)

DECLARE @IncludeCommissionInPNL_Swaps bit
SET @IncludeCommissionInPNL_Swaps = (SELECT
	IncludeCommissionInPNL_Swaps
FROM T_ReportPreferences
WHERE ReportId = @ReportId)

DECLARE @IncludeCommissionInPNL_FX bit
SET @IncludeCommissionInPNL_FX = (SELECT
	IncludeCommissionInPNL_FX
FROM T_ReportPreferences
WHERE ReportId = @ReportId)

DECLARE @IncludeCommissionInPNL_Other bit
SET @IncludeCommissionInPNL_Other = (SELECT
	IncludeCommissionInPNL_Other
FROM T_ReportPreferences
WHERE ReportId = @ReportId)

---------------------------------------------------------------------------------------------------------        
---------------------------------------------------------------------------------------------------------        

DECLARE @RecentDateForNonZeroCash datetime
SET @RecentDateForNonZeroCash = (SELECT
	dbo.[GetRecentDateForNonZeroCash](@EndDate))

DECLARE @MinTradeDate datetime
--Declare @BaseCurrencyID int                                                                    
--Set @BaseCurrencyID=(select BaseCurrencyID from T_Company)                                                         

DECLARE @DefaultAUECID int
SET @DefaultAUECID = (SELECT TOP 1
	DefaultAUECID
FROM T_Company
WHERE CompanyID > 0)

CREATE TABLE #FundsInMP(FundID int)
INSERT INTO #FundsInMP
	SELECT DISTINCT
		FundID
	FROM PM_DayMarkPrice

CREATE TABLE #FundsInFXMP(FundID int)
INSERT INTO #FundsInFXMP
	SELECT DISTINCT
		FundID
	FROM T_CurrencyConversionRate

-- get Mark Price for Start Date                                                         
CREATE TABLE #MarkPriceForStartDate(Finalmarkprice float,
Symbol varchar(max),
FundID int)

-- get Mark Price for End Date                                                                                                                                                     
CREATE TABLE #MarkPriceForEndDate(Finalmarkprice float,
Symbol varchar(max),
FundID int)
-- get forex rates for 2 date ranges           
CREATE TABLE #FXConversionRates(FromCurrencyID int,
ToCurrencyID int,
RateValue float,
ConversionMethod int,
Date datetime,
eSignalSymbol varchar(max),
FundID int)
-- get yesterday business day AUEC wise                                     
CREATE TABLE #AUECYesterDates(AUECID int,
YESTERDAYBIZDATE datetime)
-- get business day AUEC wise for End Date                                                                                            
CREATE TABLE #AUECBusinessDatesForEndDate(AUECID int,
YESTERDAYBIZDATE datetime)
-- get Security Master Data in a Temp Table                                                                                                                             
CREATE TABLE #SecMasterDataTempTable(AUECID int,
TickerSymbol varchar(100),
CompanyName varchar(500),
AssetID int,
AssetName varchar(100),
SecurityTypeName varchar(200),
SectorName varchar(100),
SubSectorName varchar(100),
CountryName varchar(100),
PutOrCall varchar(5),
Multiplier float,
LeadCurrencyID int,
VsCurrencyID int,
CurrencyID int,
ExpirationDate datetime,
UnderlyingSymbol varchar(100),
BloombergSymbol varchar(100),
SedolSymbol varchar(100),
ISINSymbol varchar(100),
CusipSymbol varchar(100),
OSISymbol varchar(100),
IDCOSymbol varchar(100),
Symbol_PK bigint,
SecurityName varchar(100)
)    

INSERT INTO #SecMasterDataTempTable
	SELECT
		AUECID,
		TickerSymbol,
		CompanyName,
		AssetID,
		AssetName,
		SecurityTypeName,
		SectorName,
		SubSectorName,
		CountryName,
		PutOrCall,
		Multiplier,
		LeadCurrencyID,
		VsCurrencyID,
		CurrencyID,
		ExpirationDate,
		UnderlyingSymbol,
		BloombergSymbol,
		SedolSymbol,
		ISINSymbol,
		CusipSymbol,
		OSISymbol,
		IDCOSymbol,
                Symbol_PK,
                CompanyName    
	FROM V_SecMasterData


DECLARE @T_FundIDs TABLE(FundId int)
INSERT INTO @T_FundIDs
	SELECT
		*
	FROM dbo.Split(@Fund, ',')

DECLARE @T_AssetIDs TABLE(AssetId int)
INSERT INTO @T_AssetIDs
	SELECT
		*
	FROM dbo.Split(@Asset, ',')

SELECT
	T_Asset.* INTO #T_Asset
FROM T_Asset
INNER JOIN @T_AssetIDs AssetIDs
	ON T_Asset.AssetID = AssetIDs.AssetId

CREATE TABLE #T_CompanyFunds(CompanyFundID int,
FundName varchar(50),
FundShortName varchar(50),
CompanyID int,
FundTypeID int,
UIOrder int NULL,
LocalCurrency int)

INSERT INTO #T_CompanyFunds
	SELECT
		CompanyFundID,
		FundName,
		FundShortName,
		CompanyID,
		FundTypeID,
		UIOrder,
		LocalCurrency
	FROM T_CompanyFunds
	INNER JOIN @T_FundIDs FundIDs
		ON T_CompanyFunds.CompanyFundID = FundIDs.FundID

CREATE TABLE #PM_Taxlots([TaxLot_PK] [bigint] NOT NULL,
[TaxLotID] [varchar](50) NOT NULL,
[Symbol] [varchar](100) NOT NULL,
[TaxLotOpenQty] [float] NOT NULL,
[AvgPrice] [float] NOT NULL,
[TimeOfSaveUTC] [datetime] NULL,
[GroupID] [nvarchar](50) NULL,
[AUECModifiedDate] [datetime] NULL,
[FundID] [int] NULL,
[Level2ID] [int] NULL,
[OpenTotalCommissionandFees] [float] NULL,
[ClosedTotalCommissionandFees] [float] NULL,
[PositionTag] [int] NULL,
[OrderSideTagValue] [nchar](10) NULL,
[TaxLotClosingId_Fk] [uniqueidentifier] NULL,
[ParentRow_Pk] [bigint] NULL,
[AccruedInterest] [Float] NULL,
[FXRate] [Float] NULL,
[FXConversionMethodOperator] [varchar](3) NULL)

INSERT INTO #PM_Taxlots
	SELECT
		TaxLot_PK,
		TaxLotID,
		Symbol,
		TaxLotOpenQty,
		AvgPrice,
		TimeOfSaveUTC,
		GroupID,
		AUECModifiedDate,
		PM_Taxlots.FundID,
		Level2ID,
		OpenTotalCommissionandFees,
		ClosedTotalCommissionandFees,
		PositionTag,
		OrderSideTagValue,
		TaxLotClosingId_Fk,
		ParentRow_Pk,
		AccruedInterest,
		FXRate,
		FXConversionMethodOperator

	FROM PM_Taxlots
	INNER JOIN @T_FundIDs FundIDs
		ON PM_Taxlots.FundID = FundIDs.FundID
	INNER JOIN #SecMasterDataTempTable SM
		ON SM.TickerSymbol = PM_Taxlots.Symbol
	INNER JOIN @T_AssetIDs AssetIDs
		ON SM.AssetID = AssetIDs.AssetID
	WHERE DATEDIFF(DAY, AUECModifiedDate, @EndDate) >= 0


CREATE TABLE #T_CorpActionData(Symbol varchar(100),
SplitFactor float,
EffectiveDate datetime,
CorpActionID varchar(100),
IsApplied bit)

INSERT INTO #T_CorpActionData (Symbol, SplitFactor, EffectiveDate, CorpActionID, IsApplied)
	SELECT
		Symbol,
		SplitFactor,
		EffectiveDate,
		CorpActionID,
		IsApplied
	FROM V_CorpActionData
	WHERE IsApplied = 1
	AND CorpActionTypeID = 6
	AND DATEDIFF(D, @StartDate, Effectivedate) >= 0
	AND DATEDIFF(D, Effectivedate, @EndDate) >= 0

CREATE TABLE #TempSplitFactorForOpen(TaxlotID varchar(50),
Symbol varchar(100),
SplitFactor float,
EffectiveDate datetime)

INSERT INTO #TempSplitFactorForOpen
	SELECT
		NA.TaxlotID,
		NA.Symbol,
		ISNULL(EXP(SUM(LOG(NA.splitFactor))), 1) AS SplitFactor,
		NA.EffectiveDate
	FROM (SELECT
		A.Taxlotid,
		A.symbol,
		VCA.SplitFactor,
		VCA.EffectiveDate
	FROM (SELECT
		TaxlotId,
		PT.Symbol AS Symbol,
		G.ProcessDate AS TradeDate
	FROM #PM_Taxlots PT
	INNER JOIN T_Group G
		ON G.GroupID = PT.GroupID
	WHERE TaxLotOpenQty <> 0
	AND Taxlot_PK IN (SELECT
		MAX(Taxlot_PK)
	FROM #PM_Taxlots
	WHERE DATEDIFF(D, #PM_Taxlots.AUECModifiedDate, @EndDate) >= 0
	GROUP BY TaxlotId)) AS A
	INNER JOIN #T_CorpActionData VCA
		ON A.Symbol = VCA.Symbol
	WHERE DATEDIFF(DAY, A.TradeDate, VCA.EffectiveDate) >= 0) AS NA
	GROUP BY	NA.TaxlotId,
				NA.symbol,
				NA.EffectiveDate

CREATE TABLE #TempSplitFactorForClosed_1(TaxlotID varchar(50),
Symbol varchar(100),
SplitFactor float,
Effectivedate datetime)

INSERT INTO #TempSplitFactorForClosed_1
	SELECT
		A.Taxlotid,
		A.symbol,
		VCA.SplitFactor,
		VCA.Effectivedate AS Effectivedate
	FROM (SELECT
		PT.TaxlotId,
		PT.Symbol AS Symbol,
		G.ProcessDate AS Tradedate


	FROM PM_TaxlotClosing PTC
	INNER JOIN #PM_Taxlots PT
		ON (PTC.PositionalTaxlotID = PT.TaxlotID
		AND PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)
	INNER JOIN T_Group G
		ON G.GroupID = PT.GroupID
	WHERE DATEDIFF(D, @StartDate, PTC.AUECLocalDate) >= 0
	AND DATEDIFF(D, PTC.AUECLocalDate, @EndDate) >= 0
	AND PTC.ClosingMode <> 7) AS A
	INNER JOIN #T_CorpActionData VCA
		ON A.Symbol = VCA.Symbol
	WHERE DATEDIFF(DAY, A.TradeDate, VCA.EffectiveDate) >= 0
SELECT DISTINCT
	TaxlotID,
	Symbol,
	SplitFactor,
	Effectivedate INTO #TempSplitFactorForClosed_2
FROM #TempSplitFactorForClosed_1

CREATE TABLE #TempSplitFactorForClosed(TaxlotID varchar(50),
Symbol varchar(100),
SplitFactor float,
Effectivedate datetime)

INSERT INTO #TempSplitFactorForClosed
	SELECT
		NA.TaxlotID,
		NA.Symbol,
		ISNULL(EXP(SUM(LOG(NA.splitFactor))), 1) AS SplitFactor,
		MAX(Effectivedate) AS Effectivedate
	FROM #TempSplitFactorForClosed_2 NA
	GROUP BY	NA.TaxlotID,
				NA.symbol


SET @MinTradeDate = (SELECT
	MIN(AuecLocalDate)
FROM PM_Taxlots T
INNER JOIN T_group G
	ON G.groupID = T.GroupID
WHERE T.taxlotopenqty <> 0
AND T.Taxlot_pk IN (SELECT
	MAX(Taxlot_pk)
FROM PM_taxlots
WHERE DATEDIFF(D, Auecmodifieddate, @StartDate) >= 0
GROUP BY Taxlotid)
AND DATEDIFF(D, AuecLocalDate, '1-1-1800') <> 0)
--(      
--Select min(PT.AUECModifiedDate) as TradeDate                                                                                                 
--      from #PM_Taxlots PT  Where taxlot_PK in                                                                                                         
--      (                                                                
--        Select max(taxlot_PK) from #PM_Taxlots                                                            
--        where Datediff(d,PT.AUECModifiedDate,@StartDate) >= 0                                                                                               
--        group by taxlotid                                                                                                                                 
--      )                                                            
--      and TaxLotOpenQty<>0      
--)                                                        

IF (@MinTradeDate IS NOT NULL AND (DATEDIFF(D, @StartDate, @MinTradeDate)) > 0) BEGIN
SET @MinTradeDate = @StartDate
END ELSE IF (@MinTradeDate IS NULL) BEGIN
SET @MinTradeDate = @StartDate
END

SET @MinTradeDate = dbo.AdjustBusinessDays(@MinTradeDate, -1, @DefaultAUECID)

INSERT INTO #FXConversionRates EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange	@MinTradeDate,
																						@EndDate

UPDATE #FXConversionRates
SET RateValue = 1.0 / RateValue
WHERE RateValue <> 0
AND ConversionMethod = 1

-- For Fund Zero
SELECT * INTO #ZeroFundFxRate
FROM #FXConversionRates
WHERE fundID = 0


-- Create a Temp table and insert Data in that table and remove Union All                                                                                   

CREATE TABLE #MTMDataTable(FundID int,
Symbol varchar(100),
TaxLotOpenQty float,
AvgPrice float,
ClosingPrice float,
AssetID int,
CurrencyID int,
CurrencySymbol varchar(50),
AUECID int,
TotalOpenCommission_Local float,
TotalOpenCommission_Base float,
TotalClosedCommission_Local float,
TotalClosedCommission_Base float,
AssetMultiplier float,
TradeDate datetime,
ClosingDate datetime,
Mark1 float,
Mark2 float,
IsSwapped bit,
NotionalValue float,
BenchMarkRate float,
Differential float,
OrigCostBasis float,
DayCount float,
FirstResetDate datetime,
OrigTransDate datetime,
SwapData float,
BeginningMarketValue_Local float,
BeginningMarketValue_Base float,
EndingMarketValue_Local float,
EndingMarketValue_Base float,
Open_CloseTag varchar(5),
ConversionRateTrade float,
ConversionRateStart float,
ConversionRateEnd float,
CompanyName varchar(200),
FundName varchar(100),
StrategyName varchar(100),
Side varchar(10),
Asset varchar(50),
UDAAsset varchar(100),
UDASecurityTypeName varchar(100),
UDASectorName varchar(100),
UDASubSectorName varchar(100),
UDACountryName varchar(100),
PutOrCall varchar(5),
MasterFundName varchar(100),
Dividend float,
MasterStrategyName varchar(100),
UnderlyingSymbol varchar(100),
CashFXUnrealizedPNL float,
NetTotalCost_Base float,
BloombergSymbol varchar(100),
SedolSymbol varchar(100),
ISINSymbol varchar(100),
CusipSymbol varchar(100),
OSISymbol varchar(100),
IDCOSymbol varchar(100),
UnrealizedPNLMTM_Local float,
RealizedPNLMTM_Local float,
UnrealizedPNLMTM_Base float,
RealizedPNLMTM_Base float,
ExpirationDate datetime,
OpenTradeAttribute1 varchar(200),
OpenTradeAttribute2 varchar(200),
OpenTradeAttribute3 varchar(200),
OpenTradeAttribute4 varchar(200),
OpenTradeAttribute5 varchar(200),
OpenTradeAttribute6 varchar(200),
ClosedTradeAttribute1 varchar(200),
ClosedTradeAttribute2 varchar(200),
ClosedTradeAttribute3 varchar(200),
ClosedTradeAttribute4 varchar(200),
ClosedTradeAttribute5 varchar(200),
ClosedTradeAttribute6 varchar(200),
BaseCurrencyID int,
SecurityName varchar(500))    

-------------------region new mark price insert region---------------------------------     
INSERT INTO #AUECYesterDates
	SELECT DISTINCT
		V_SymbolAUEC.AUECID,
		dbo.AdjustBusinessDays(@StartDate, -1, V_SymbolAUEC.AUECID)
	FROM V_SymbolAUEC

INSERT INTO #MarkPriceForStartDate (Finalmarkprice, Symbol, FundID)
	SELECT
		FinalMarkPrice,
		PM_DayMarkPrice.Symbol,
		PM_DayMarkPrice.FundID
	FROM PM_DayMarkPrice
	INNER JOIN V_SymbolAuec
		ON PM_DayMarkPrice.Symbol = V_SymbolAuec.Symbol
	INNER JOIN #AUECYesterDates AUECYesterDates
		ON AUECYesterDates.AUECID = V_SymbolAuec.AUECID
	WHERE DATEDIFF(D, PM_DayMarkPrice.Date, AUECYesterDates.YESTERDAYBIZDATE) = 0

INSERT INTO #AUECBusinessDatesForEndDate
	SELECT DISTINCT
		V_SymbolAUEC.AUECID,
		dbo.AdjustBusinessDays(DATEADD(D, 1, @EndDate), -1, V_SymbolAUEC.AUECID)
	FROM V_SymbolAUEC

INSERT INTO #MarkPriceForEndDate (Finalmarkprice, Symbol, FundID)
	SELECT
		FinalMarkPrice,
		PM_DayMarkPrice.Symbol,
		PM_DayMarkPrice.FundID
	FROM PM_DayMarkPrice
	INNER JOIN V_SymbolAuec
		ON PM_DayMarkPrice.Symbol = V_SymbolAuec.Symbol
	INNER JOIN #AUECBusinessDatesForEndDate AUECBusinessDatesForEndDate
		ON AUECBusinessDatesForEndDate.AUECID = V_SymbolAuec.AUECID
	WHERE DATEDIFF(D, PM_DayMarkPrice.Date, AUECBusinessDatesForEndDate.YESTERDAYBIZDATE) = 0

-- For Fund Zero
SELECT *
INTO #ZeroFundMarkPriceStartDate
FROM #MarkPriceForStartDate
WHERE fundID = 0

SELECT *
INTO #ZeroFundMarkPriceEndDate
FROM #MarkPriceForEndDate
WHERE fundID = 0


-----------------------------------------------------------------------------------------------------------                        
CREATE TABLE #TempClosingData(PTTaxLot_PK bigint,
PTTaxLotID varchar(50),
PTSymbol varchar(100),
PTTaxLotOpenQty float,
PTAvgPrice float,
PTTimeOfSaveUTC datetime,
PTGroupID nvarchar(50),
PTAUECModifiedDate datetime,
PTFundID int,
PTLevel2ID int,
PTOpenTotalCommissionandFees float,
PTClosedTotalCommissionandFees float,
PTPositionTag int,
PTOrderSideTagValue nchar(10),
PTTaxLotClosingId_Fk uniqueidentifier,
PT1TaxLot_PK bigint,
PT1TaxLotID varchar(50),
PT1Symbol varchar(100),
PT1TaxLotOpenQty float,
PT1AvgPrice float,
PT1TimeOfSaveUTC datetime,
PT1GroupID nvarchar(50),
PT1AUECModifiedDate datetime,
PT1FundID int,
PT1Level2ID int,
PT1OpenTotalCommissionandFees float,
PT1ClosedTotalCommissionandFees float,
PT1PositionTag int,
PT1OrderSideTagValue nchar(10),
PT1TaxLotClosingId_Fk uniqueidentifier,
TaxLotClosingID uniqueidentifier,
PositionalTaxlotId varchar(50),
ClosingTaxlotId varchar(50),
ClosedQty float,
ClosingMode int,
TimeOfSaveUTC datetime,
AUECLocalDate datetime,
PositionSide nchar(10),
G1Symbol varchar(100),
G1AssetID int,
G1CurrencyID int,
G1FXRate float,
G1FXConversionMethodOperator varchar(3),
G1AUECLocalDate datetime,
G1OrderSideTagValue varchar(3),
G1AvgPrice float,
G1CumQty float,
G1GroupID varchar(50),
G1IsSwapped bit,
G1AllocationDate datetime,
G1ProcessDate datetime,
G1OriginalPurchaseDate datetime,
GSymbol varchar(100),
GAUECID int,
GCurrencyID int,
GUnderlyingID int,
GExchangeID int,
GAUECLocalDate datetime,
GAvgPrice float,
GCumQty float,
GFXRate float,
GFXConversionMethodOperator varchar(3),
GAssetID int,
GOrderSideTagValue varchar(3),
GCounterPartyID int,
GGroupID varchar(50),
GIsswapped bit,
GAllocationDate datetime,
GProcessDate datetime,
GOriginalPurchaseDate datetime,
OpeningFXRate float,
EndingFXRate float,
TradeDateFXRate float,
StartDateFXRate float,
ClosingDateFXRate float,
GTradeAttribute1 varchar(200),
GTradeAttribute2 varchar(200),
GTradeAttribute3 varchar(200),
GTradeAttribute4 varchar(200),
GTradeAttribute5 varchar(200),
GTradeAttribute6 varchar(200),
G1TradeAttribute1 varchar(200),
G1TradeAttribute2 varchar(200),
G1TradeAttribute3 varchar(200),
G1TradeAttribute4 varchar(200),
G1TradeAttribute5 varchar(200),
G1TradeAttribute6 varchar(200)
)

INSERT INTO #TempClosingData
	SELECT
		PT.[TaxLot_PK] AS [PTTaxLot_PK],
		PT.[TaxLotID] AS [PTTaxLotID],
		PT.[Symbol] AS [PTSymbol],
		PT.[TaxLotOpenQty] AS [PTTaxLotOpenQty],
		PT.[AvgPrice] AS [PTAvgPrice],
		PT.[TimeOfSaveUTC] AS [PTTimeOfSaveUTC],
		PT.[GroupID] AS [PTGroupID],
		PT.[AUECModifiedDate] AS [PTAUECModifiedDate],
		PT.[FundID] AS [PTFundID],
		PT.[Level2ID] AS [PTLevel2ID],
		PT.[OpenTotalCommissionandFees] AS [PTOpenTotalCommissionandFees],
		PT.[ClosedTotalCommissionandFees] AS [PTClosedTotalCommissionandFees],
		PT.[PositionTag] AS [PTPositionTag],
		PT.[OrderSideTagValue] AS [PTOrderSideTagValue],
		PT.[TaxLotClosingId_Fk] AS [PTTaxLotClosingId_Fk],
		PT1.[TaxLot_PK] AS [PT1TaxLot_PK],
		PT1.[TaxLotID] AS [PT1TaxLotID],
		PT1.[Symbol] AS [PT1Symbol],
		PT1.[TaxLotOpenQty] AS [PT1TaxLotOpenQty],
		PT1.[AvgPrice] AS [PT1AvgPrice],
		PT1.[TimeOfSaveUTC] AS [PT1TimeOfSaveUTC],
		PT1.[GroupID] AS [PT1GroupID],
		PT1.[AUECModifiedDate] AS [PT1AUECModifiedDate],
		PT1.[FundID] AS [PT1FundID],
		PT1.[Level2ID] AS [PT1Level2ID],
		PT1.[OpenTotalCommissionandFees] AS [PT1OpenTotalCommissionandFees],
		PT1.[ClosedTotalCommissionandFees] AS [PT1ClosedTotalCommissionandFees],
		PT1.[PositionTag] AS [PT1PositionTag],
		PT1.[OrderSideTagValue] AS [PT1OrderSideTagValue],
		PT1.[TaxLotClosingId_Fk] AS [PT1TaxLotClosingId_Fk],
		PTC.[TaxLotClosingID] AS [TaxLotClosingID],
		PTC.[PositionalTaxlotId] AS [PositionalTaxlotId],
		PTC.[ClosingTaxlotId] AS [ClosingTaxlotId],
		PTC.[ClosedQty] AS [ClosedQty],
		PTC.[ClosingMode] AS [ClosingMode],
		PTC.[TimeOfSaveUTC] AS [TimeOfSaveUTC],
		PTC.[AUECLocalDate] AS [AUECLocalDate],
		PTC.[PositionSide] AS [PositionSide],
		G1.Symbol AS G1Symbol,
		G1.AssetID AS G1AssetID,
		G1.CurrencyID AS G1CurrencyID,
		G1.FXRate AS G1FXRate,
		G1.FXConversionMethodOperator AS G1FXConversionMethodOperato,
		G1.AUECLocalDate AS G1AUECLocalDate,
		G1.OrderSideTagValue AS G1OrderSideTagValue,
		G1.AvgPrice AS G1AvgPrice,
		G1.CumQty AS G1CumQty,
		G1.GroupID AS G1GroupID,
		G1.IsSwapped AS G1IsSwapped,
		G1.AllocationDate AS G1AllocationDate,
		G1.ProcessDate AS G1ProcessDate,
		G1.OriginalPurchaseDate AS G1OriginalPurchaseDate,
		G.Symbol AS GSymbol,
		G.AUECID AS GAUECID,
		G.CurrencyID AS GCurrencyID,
		G.UnderlyingID AS GUnderlyingID,
		G.ExchangeID AS GExchangeID,
		G.AUECLocalDate AS GAUECLocalDate,
		G.AvgPrice AS GAvgPrice,
		G.CumQty AS GCumQty,
		G.FXRate AS GFXRate,
		G.FXConversionMethodOperator AS GFXConversionMethodOperator,
		G.AssetID AS GAssetID,
		G.OrderSideTagValue AS G1OrderSideTagValue,
		G.CounterPartyID AS GCounterPartyID,
		G.GroupID AS GGroupID,
		G.IsSwapped AS GIsSwapped,
		G.AllocationDate AS GAllocationDate,
		G.ProcessDate AS GProcessDate,
		G.OriginalPurchaseDate AS GOriginalPurchaseDate,

		CASE
			WHEN ISNULL(PT.FXRate, G.FXrate) > 0 AND
			ISNULL(PT.FXConversionMethodOperator, G.FXConversionMethodOperator) = 'M' THEN ISNULL(PT.FXRate, G.FXrate)
			WHEN ISNULL(PT.FXRate, G.FXrate) > 0 AND
			ISNULL(PT.FXConversionMethodOperator, G.FXConversionMethodOperator) = 'D' THEN 1 / ISNULL(PT.FXRate, G.FXrate)
			ELSE IsNull(FXRatesForTradeDate.Val,0)
		END AS OpeningFXRate,

		CASE
			WHEN ISNULL(PT1.FXRate, G1.FXrate) > 0 AND
			ISNULL(PT1.FXConversionMethodOperator, G1.FXConversionMethodOperator) = 'M' THEN ISNULL(PT1.FXRate, G1.FXrate)
			WHEN ISNULL(PT1.FXRate, G1.FXrate) > 0 AND
			ISNULL(PT1.FXConversionMethodOperator, G1.FXConversionMethodOperator) = 'D' THEN 1 / ISNULL(PT1.FXRate, G1.FXrate)
			ELSE ISNULL(FXRatesForClosingDate.Val, 0)
		END AS EndingFXRate,

		ISNULL(FXRatesForTradeDate.Val, 0) AS TradeDateFXRate,
		ISNULL(FXRatesForStartDate.Val, 0) AS StartDateFXRate,
		ISNULL(FXRatesForClosingDate.Val, 0) AS ClosingDateFXRate,
		G.TradeAttribute1 AS GTradeAttribute1,
		G.TradeAttribute2 AS GTradeAttribute2,
		G.TradeAttribute3 AS GTradeAttribute3,
		G.TradeAttribute4 AS GTradeAttribute4,
		G.TradeAttribute5 AS GTradeAttribute5,
		G.TradeAttribute6 AS GTradeAttribute6,
		G1.TradeAttribute1 AS G1TradeAttribute1,
		G1.TradeAttribute2 AS G1TradeAttribute2,
		G1.TradeAttribute3 AS G1TradeAttribute3,
		G1.TradeAttribute4 AS G1TradeAttribute4,
		G1.TradeAttribute5 AS G1TradeAttribute5,
		G1.TradeAttribute6 AS G1TradeAttribute6

	FROM PM_TaxlotClosing PTC
	INNER JOIN #PM_Taxlots PT
		ON (PTC.PositionalTaxlotID = PT.TaxlotID
		AND PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)
	INNER JOIN #PM_Taxlots PT1
		ON (PTC.ClosingTaxlotID = PT1.TaxlotID
		AND PTC.TaxLotClosingId = PT1.TaxLotClosingId_Fk)
	INNER JOIN T_Group G
		ON G.GroupID = PT.GroupID
	INNER JOIN T_Group G1
		ON G1.GroupID = PT1.GroupID
	LEFT OUTER JOIN T_CompanyFunds TC
		ON PT.FundID = TC.CompanyFundID
	--get yesterday business day                                                                                                                        
	LEFT OUTER JOIN #AUECYesterDates AUECYesterDates
		ON G.AUECID = AUECYesterDates.AUECID

	-- Forex Price for Trade Date                                                                                                                                                                     
 LEFT OUTER JOIN #FXConversionRates FXDayRatesForTradeDate ON (  
   FXDayRatesForTradeDate.FromCurrencyID = G.CurrencyID  
		AND FXDayRatesForTradeDate.ToCurrencyID = TC.LocalCurrency
   AND DateDiff(d, G.ProcessDate, FXDayRatesForTradeDate.DATE) = 0  
			AND FXDayRatesForTradeDate.FundID = PT.FundID
    )  
	LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateTradeDate ON (
			ZeroFundFxRateTradeDate.FromCurrencyID = G.CurrencyID
			AND ZeroFundFxRateTradeDate.ToCurrencyID = TC.LocalCurrency
			AND DateDiff(d, G.ProcessDate, ZeroFundFxRateTradeDate.DATE) = 0
			AND ZeroFundFxRateTradeDate.FundID = 0
   )  
	-- Forex Price for Start Date                                                                                                                                 
 LEFT OUTER JOIN #FXConversionRates FXDayRatesForStartDate ON (  
   FXDayRatesForStartDate.FromCurrencyID = G.CurrencyID  
		AND FXDayRatesForStartDate.ToCurrencyID = TC.LocalCurrency
   AND DateDiff(d, AUECYesterDates.YESTERDAYBIZDATE, FXDayRatesForStartDate.DATE) = 0  
			AND FXDayRatesForStartDate.FundID = PT.FundID
    )  
	LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateStartDate ON (
			ZeroFundFxRateStartDate.FromCurrencyID = G.CurrencyID
			AND ZeroFundFxRateStartDate.ToCurrencyID = TC.LocalCurrency
			AND DateDiff(d, AUECYesterDates.YESTERDAYBIZDATE, ZeroFundFxRateStartDate.DATE) = 0
			AND ZeroFundFxRateStartDate.FundID = 0
   )  
	-- Forex Price for Closing Date                                                                                                                                             
 LEFT OUTER JOIN #FXConversionRates FXDayRatesForClosingDate ON (  
   FXDayRatesForClosingDate.FromCurrencyID = G.CurrencyID  
		AND FXDayRatesForClosingDate.ToCurrencyID = TC.LocalCurrency
   AND DateDiff(d, G1.ProcessDate, FXDayRatesForClosingDate.DATE) = 0  
			AND FXDayRatesForClosingDate.FundID = PT.FundID
    )  
	LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateClosingDate ON (
			ZeroFundFxRateClosingDate.FromCurrencyID = G.CurrencyID
			AND ZeroFundFxRateClosingDate.ToCurrencyID = TC.LocalCurrency
			AND DateDiff(d, G1.ProcessDate, ZeroFundFxRateClosingDate.DATE) = 0
			AND ZeroFundFxRateClosingDate.FundID = 0
   )  
	CROSS APPLY (
		SELECT CASE 
				WHEN FXDayRatesForTradeDate.RateValue IS NULL
					THEN CASE 
						WHEN ZeroFundFxRateTradeDate.RateValue IS NULL
							THEN 0
							ELSE ZeroFundFxRateTradeDate.RateValue
						END
					ELSE FXDayRatesForTradeDate.RateValue
				END
		) AS FXRatesForTradeDate(Val)
	CROSS APPLY (
		SELECT CASE 
				WHEN FXDayRatesForStartDate.RateValue IS NULL
					THEN CASE 
						WHEN ZeroFundFxRateStartDate.RateValue IS NULL
							THEN 0
							ELSE ZeroFundFxRateStartDate.RateValue
						END
					ELSE FXDayRatesForStartDate.RateValue
				END
		) AS FXRatesForStartDate(Val)
	CROSS APPLY (
		SELECT CASE 
				WHEN FXDayRatesForClosingDate.RateValue IS NULL
					THEN CASE 
						WHEN ZeroFundFxRateClosingDate.RateValue IS NULL
							THEN 0
							ELSE ZeroFundFxRateClosingDate.RateValue
						END
					ELSE FXDayRatesForClosingDate.RateValue
				END
		) AS FXRatesForClosingDate(Val)

	WHERE DATEDIFF(D, @StartDate, PTC.AUECLocalDate) >= 0
	AND DATEDIFF(D, PTC.AUECLocalDate, @EndDate) >= 0
-----------------------------------------------------------------------------------------------------------                         

INSERT INTO #MTMDataTable

			/**************************************************************************                                    
			       OPEN POSITIONS HANDLING                                    
			**************************************************************************/
			SELECT
				PT.FundID AS FundID,
				PT.Symbol AS Symbol,
				PT.TaxLotOpenQty AS TaxLotOpenQty,
				PT.AvgPrice AS AvgPrice,
				0 AS ClosingPrice,
				G.AssetID AS AssetID,
				G.CurrencyID AS CurrencyID,
				C.CurrencySymbol AS CurrencySymbol,
				G.AUECID AS AUECID,

				ISNULL(PT.OpenTotalCommissionAndFees, 0) AS TotalOpenCommission_Local,

				CASE
					WHEN G.CurrencyID = TC.LocalCurrency THEN ISNULL(PT.OpenTotalCommissionAndFees, 0)
					ELSE  --When Company and Traded Currency are different                                                                                                                                    
					CASE
						WHEN ISNULL(PT.FXRate, G.FXRate) > 0 AND
						ISNULL(PT.FXConversionMethodOperator, G.FXConversionMethodOperator) = 'M' 
						THEN ISNULL(PT.OpenTotalCommissionAndFees * ISNULL(PT.FXRate, G.FXRate), 0)
						WHEN ISNULL(PT.FXRate, G.FXRate) > 0 AND
						ISNULL(PT.FXConversionMethodOperator, G.FXConversionMethodOperator) = 'D' 
						THEN ISNULL(PT.OpenTotalCommissionAndFees * 1 / ISNULL(PT.FXRate, G.FXRate), 0)
						ELSE ISNULL(PT.OpenTotalCommissionAndFees * ISNULL(FXRatesForTradeDate.Val, 0), 0)
					END
				END AS TotalOpenCommission_Base,

				0 AS TotalClosedCommission_Local,
				0 AS TotalClosedCommission_Base,
				SM.Multiplier AS AssetMultiplier,
				G.ProcessDate AS TradeDate,
				NULL AS ClosingDate,
				ISNULL(MPStartDate.Val, 0) AS Mark1,
				ISNULL(MPEndDate.Val, 0) AS Mark2,





				G.IsSwapped AS IsSwapped,
				ISNULL((PT.TaxLotOpenQty * SW.NotionalValue /
					CASE
						WHEN G.CumQty = 0 THEN 1
						ELSE G.CumQty
					END), 0) AS NotionalValue,
				ISNULL(SW.BenchMarkRate, 0) AS BenchMarkRate,
				ISNULL(SW.Differential, 0) AS Differential,
				ISNULL(SW.OrigCostBasis, 0) AS OrigCostBasis,
				ISNULL(SW.DayCount, 0) AS DayCount,
				ISNULL(SW.FirstResetDate, '') AS FirstResetDate,
				ISNULL(SW.OrigTransDate, '') AS OrigTransDate,




				--Case G.IsSwapped                                                                                                                                                                                                                               
				-- When 1                                           
				-- Then                                                                                                                                                                                
				--  Case                                                                                                                   
				--   When  DateDiff(d,@StartDate,SW.OrigTransDate) >=0                                                                   
				--   Then                                     
				--    case                                     
				--     when G.CurrencyID = @BaseCurrencyID                   
				--     then (((isnull((PT.TaxLotOpenQty * SW.NotionalValue / G.CumQty) ,0) * (isnull(SW.BenchMarkRate,0) + isnull(SW.Differential,0)) * DateDiff(d,SW.OrigTransDate,@EndDate))/100)/SW.DayCount)* dbo.GetSideMultiplier(PT.OrderSideTagValue)                





				--     else (((isnull((PT.TaxLotOpenQty * SW.NotionalValue / G.CumQty) ,0) * (isnull(SW.BenchMarkRate,0) + isnull(SW.Differential,0)) * DateDiff(d,SW.OrigTransDate,@EndDate))/100)/SW.DayCount)* dbo.GetSideMultiplier(PT.OrderSideTagValue)                





				--*  Isnull(FXDayRatesForEndDate.RateValue,0)                                                        
				--    end                                    
				--   Else                                     
				--    case                                     
				--     when G.CurrencyID = @BaseCurrencyID                                    
				--     then (((isnull((PT.TaxLotOpenQty * SW.NotionalValue / G.CumQty) ,0) * (isnull(SW.BenchMarkRate,0) + isnull(SW.Differential,0)) * DateDiff(d,@StartDate,@EndDate))/100)/SW.DayCount ) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                    





				--     else (((isnull((PT.TaxLotOpenQty * SW.NotionalValue / G.CumQty) ,0) * (isnull(SW.BenchMarkRate,0) + isnull(SW.Differential,0)) * DateDiff(d,@StartDate,@EndDate))/100)/SW.DayCount ) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                    



				--*  Isnull(FXDayRatesForEndDate.RateValue,0)                                                        
				--    end                                    
				--  End                       
				-- Else 0                                                                                                       
				--End as SwapData,                      

				CASE G.IsSwapped
					WHEN 1 THEN CASE
						WHEN G.CurrencyID = TC.LocalCurrency THEN (((ISNULL((PT.TaxLotOpenQty * SW.NotionalValue / G.CumQty), 0) * (ISNULL(SW.BenchMarkRate, 0) + ISNULL(SW.Differential, 0)) * DATEDIFF(D, SW.OrigTransDate, @EndDate)) / 100) / SW.DayCount) * dbo.GetSideMultiplier(PT.OrderSideTagValue)  
    ELSE (((ISNULL((PT.TaxLotOpenQty * SW.NotionalValue / G.CumQty),
				0) *
					(ISNULL(SW.BenchMarkRate, 0) + ISNULL(SW.Differential, 0)) * DATEDIFF(D, SW.OrigTransDate, @EndDate)) / 100) / SW.DayCount) * dbo.GetSideMultiplier(PT.OrderSideTagValue) * ISNULL(FXRatesForEndDate.Val, 0)  
   END  
   ELSE 0  
  END AS SwapData,  
  
  CASE  
   WHEN DATEDIFF(D, @StartDate, G.ProcessDate) >= 0 THEN CASE  
    WHEN DATEDIFF(DAY, SplitTab.EffectiveDate, G.ProcessDate) = 0  
    ----Then IsNull(G.AvgPrice * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                               
    ----Else IsNull((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                           
    THEN ISNULL(PT.AvgPrice * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0)  
    ELSE ISNULL((PT.AvgPrice) * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0)  
   END  
   ELSE ISNULL((ISNULL(MPStartDate.Val, 0) / ISNULL(SplitTab.SplitFactor, 1)) * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0)  
  END AS BeginningMarketValue_Local,  
  
  --Market Value in Base on Start Date                                                               
  CASE  
   WHEN G.CurrencyID = TC.LocalCurrency THEN CASE  
    WHEN DATEDIFF(D, @StartDate, G.ProcessDate) >= 0 THEN CASE  
     WHEN DATEDIFF(DAY, SplitTab.EffectiveDate, G.ProcessDate) = 0 THEN ISNULL(PT.AvgPrice * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0)  
     ELSE ISNULL((PT.AvgPrice) * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0)  
    END  
    ELSE ISNULL((ISNULL(MPStartDate.Val, 0) / ISNULL(SplitTab.SplitFactor, 1)) * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0)  
   END  
   ELSE CASE  
    WHEN DATEDIFF(D, @StartDate, G.ProcessDate) >= 0 THEN CASE  
     WHEN DATEDIFF(DAY, SplitTab.EffectiveDate, G.ProcessDate) = 0 THEN CASE  
      WHEN ISNULL(PT.FXRate, G.FXRate) > 0 AND  
      ISNULL(PT.FXConversionMethodOperator, G.FXConversionMethodOperator) = 'M' THEN ISNULL(PT.AvgPrice * ISNULL(PT.FXRate, G.FXRate) * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0)  
      WHEN ISNULL(PT.FXRate, G.FXRate) > 0 AND  
      ISNULL(PT.FXConversionMethodOperator, G.FXConversionMethodOperator) = 'D' THEN ISNULL((PT.AvgPrice * 1 / ISNULL(PT.FXRate, G.FXRate)) * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0)  
      ELSE ISNULL(PT.AvgPrice * ISNULL(FXRatesForTradeDate.Val, 0) * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0)  
     END  
     ELSE CASE  
      WHEN ISNULL(PT.FXRate, G.FXRate) > 0 AND  
      ISNULL(PT.FXConversionMethodOperator, G.FXConversionMethodOperator) = 'M' THEN ISNULL((PT.AvgPrice) * ISNULL(PT.FXRate, G.FXRate) * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0)  
      WHEN ISNULL(PT.FXRate, G.FXRate) > 0 AND  
      ISNULL(PT.FXConversionMethodOperator, G.FXConversionMethodOperator) = 'D' THEN ISNULL(((PT.AvgPrice) * 1 / ISNULL(PT.FXRate, G.FXRate)) * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0)  
      ELSE ISNULL((PT.AvgPrice) * ISNULL(FXRatesForTradeDate.Val, 0) * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0)  
     END  
    END  
    ELSE ISNULL((ISNULL(MPStartDate.Val, 0) / ISNULL(SplitTab.SplitFactor, 1)) * FXRatesForStartDate.Val * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0)  
   END  
  END AS BeginningMarketValue_Base,  
  
  ISNULL(ISNULL(MPEndDate.Val, 0) * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0) AS EndingMarketValue_Local,  
  
  CASE  
   WHEN G.CurrencyID = TC.LocalCurrency THEN ISNULL(ISNULL(MPEndDate.Val, 0) * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0)  
  
  
  
  
   ELSE ISNULL(ISNULL(MPEndDate.Val, 0) * ISNULL(FXRatesForEndDate.Val, 0) * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0)  
  END AS EndingMarketValue_Base,  
  
  --Case                                     
  --When G.AssetID=3 then 'I'                                                                                                                                            
  -- Else 'O'                         
  --End as Open_CloseTag,                 
  'O' AS Open_CloseTag,  
  
  CASE  
   WHEN ISNULL(PT.FXRate, G.FXRate) > 0 AND  
   ISNULL(PT.FXConversionMethodOperator, G.FXConversionMethodOperator) = 'M' THEN ISNULL(PT.FXRate, G.FXRate)  
   WHEN ISNULL(PT.FXRate, G.FXRate) > 0 AND  
   ISNULL(PT.FXConversionMethodOperator, G.FXConversionMethodOperator) = 'D' THEN 1 / ISNULL(PT.FXRate, G.FXRate)  
   ELSE ISNULL(FXRatesForTradeDate.Val, 0)  
  END AS ConversionRateTrade,  
  
  ISNULL(FXRatesForStartDate.Val, 0) AS ConversionRateStart,  
  
  CASE  
   WHEN G.CurrencyID = TC.LocalCurrency THEN 1  
   ELSE ISNULL(FXRatesForEndDate.Val, 0)  
  END AS ConversionRateEnd,  
  
  SM.CompanyName,  
  TC.FundName AS FundName,  
  ISNULL(CompanyStrategy.StrategyName, 'Strategy Unallocated') AS StrategyName,  
  
  
  
  
  CASE dbo.GetSideMultiplier(PT.OrderSideTagValue)  
   WHEN 1 THEN 'Long'  
   ELSE 'Short'  
  END AS Side,  
  #T_Asset.AssetName AS Asset,  
  ISNULL(SM.AssetName, 'Undefined') AS UDAAsset,  
  ISNULL(SM.SecurityTypeName, 'Undefined') AS UDASecurityTypeName,  
  ISNULL(SM.SectorName, 'Undefined') AS UDASectorName,  
  ISNULL(SM.SubSectorName, 'Undefined') AS UDASubSectorName,  
  ISNULL(SM.CountryName, 'Undefined') AS UDACountryName,  
  ISNULL(SM.PutOrCall, '') AS PutOrCall,  
  ISNULL(CMF.MasterFundName, 'Unassigned') AS MasterFundName,  
  0 AS Dividend,  
  ISNULL(CMS.MasterStrategyName, 'Unassigned') AS MasterStrategyName,  
  ISNULL(SM.UnderlyingSymbol, '') AS UnderlyingSymbol,  
  0 AS CashFXUnrealizedPNL,  
  
  
  
  
  
  CASE  
   -- When Trade date FX Rate is used        
   WHEN  
   (  
   (G.AssetID = 1 AND  
   G.IsSwapped = 1 AND  
   @IncludeFXPNLinSwaps = 1) OR  
   (G.AssetID = 5 AND  
   @IncludeFXPNLinFX = 1) OR  
   (G.AssetID = 11 AND  
   @IncludeFXPNLinFX = 1) OR  
   (G.AssetID = 3 AND  
   @FuturePNLWithBothOrEndFXRate = 1) OR  
   (G.AssetID = 4 AND  
   TC.LocalCurrency <> G.CurrencyID AND  
   @InternationalFutureOptionPNLWithBothOrEndFXRate = 1)  
   ) THEN CASE  
    WHEN G.CurrencyID = TC.LocalCurrency THEN ISNULL(PT.AvgPrice * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0) + ISNULL(PT.OpenTotalCommissionAndFees, 0)  
  
    ELSE CASE  
     WHEN ISNULL(PT.FXRate, G.FXRate) > 0 AND  
     ISNULL(PT.FXConversionMethodOperator, G.FXConversionMethodOperator) = 'M' THEN (ISNULL(PT.AvgPrice * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0) + ISNULL(PT.OpenTotalCommissionAndFees, 0)) * ISNULL(PT.FXRate, G.FXRate)  
  
     WHEN ISNULL(PT.FXRate, G.FXRate) > 0 AND  
     ISNULL(PT.FXConversionMethodOperator, G.FXConversionMethodOperator) = 'D' THEN (ISNULL(PT.AvgPrice * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0) + ISNULL(PT.OpenTotalCommissionAndFees, 0)) * 1 / ISNULL(PT.FXRate, G.FXRate)  
  
     ELSE (ISNULL(PT.AvgPrice * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0) + ISNULL(PT.OpenTotalCommissionAndFees, 0)) * ISNULL(FXRatesForTradeDate.Val, 0)  
  
    END  
   END  
   -- When No FX Rate is used                  
   WHEN  
   (  
   (G.AssetID = 1 AND  
   G.IsSwapped = 1 AND  
   @IncludeFXPNLinSwaps = 0) OR  
   (G.AssetID = 5 AND  
   @IncludeFXPNLinFX = 0) OR  
   (G.AssetID = 11 AND  
   @IncludeFXPNLinFX = 0) OR  
   (G.AssetID = 3 AND  
   @FuturePNLWithBothOrEndFXRate = 0) OR  
   (G.AssetID = 4 AND  
   TC.LocalCurrency <> G.CurrencyID AND  
   @InternationalFutureOptionPNLWithBothOrEndFXRate = 0)  
   ) THEN CASE  
    WHEN G.CurrencyID = TC.LocalCurrency THEN ISNULL(PT.AvgPrice * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0) + ISNULL(PT.OpenTotalCommissionAndFees, 0)  
    ELSE (ISNULL(PT.AvgPrice * TaxlotOpenQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0) + ISNULL(PT.OpenTotalCommissionAndFees, 0)) * ISNULL(FXRatesForEndDate.Val, 0)  
  
   END  
  
  
   ELSE 0  
  END AS NetTotalCost_Base,  
  
  
  
  SM.BloombergSymbol,  
  SM.SedolSymbol,  
  SM.ISINSymbol,  
  SM.CusipSymbol,  
  SM.OSISymbol,  
  SM.IDCOSymbol,  
  0 AS UnrealizedPNLMTM_Local,  
  0 AS RealizedPNLMTM_Local,  
  0 AS UnrealizedPNLMTM_Base,  
  0 AS RealizedPNLMTM_Base,  
  CAST(FLOOR(CAST(SM.ExpirationDate AS float)) AS datetime) AS ExpirationDate,  
  G.TradeAttribute1 AS OpenTradeAttribute1,  
  G.TradeAttribute2 AS OpenTradeAttribute2,  
  G.TradeAttribute3 AS OpenTradeAttribute3,  
  G.TradeAttribute4 AS OpenTradeAttribute4,  
  G.TradeAttribute5 AS OpenTradeAttribute5,  
  G.TradeAttribute6 AS OpenTradeAttribute6,  
  '' AS ClosedTradeAttribute1,  
  '' AS ClosedTradeAttribute2,  
  '' AS ClosedTradeAttribute3,  
  '' AS ClosedTradeAttribute4,  
  '' AS ClosedTradeAttribute5,  
  '' AS ClosedTradeAttribute6,  
  TC.LocalCurrency AS BaseCurrencyID,
  SM.SecurityName as SecurityName     
  
 FROM #PM_Taxlots PT  
 INNER JOIN #T_CompanyFunds TC  
  ON PT.FundID = TC.CompanyFundID  
 INNER JOIN T_Group G  
  ON G.GroupID = PT.GroupID  
 INNER JOIN #T_Asset  
  ON #T_Asset.AssetId = G.AssetID  
  
 LEFT OUTER JOIN #MarkPriceForStartDate MPS ON (  
   MPS.Symbol = PT.Symbol  
			AND MPS.FundID = PT.FundID
    )  
	LEFT OUTER JOIN #ZeroFundMarkPriceStartDate MPZeroStartDate ON (
			PT.Symbol = MPZeroStartDate.Symbol
			AND MPZeroStartDate.FundID = 0
   )  
 LEFT OUTER JOIN #MarkPriceForEndDate MPE ON (  
   MPE.Symbol = PT.Symbol  
			AND MPE.FundID = PT.FundID
    )  
	LEFT OUTER JOIN #ZeroFundMarkPriceEndDate MPZeroEndDate ON (
			PT.Symbol = MPZeroEndDate.Symbol
			AND MPZeroEndDate.FundID = 0
   )  
  
 --Left Outer Join #MarkPriceForStartDate MPS on MPS.Symbol=PT.Symbol                                                                  
 --Left Outer Join #MarkPriceForEndDate MPE on MPE.Symbol=PT.Symbol                                                                                                                                                    
 LEFT OUTER JOIN T_SwapParameters SW  
  ON G.GroupID = SW.GroupID  
 LEFT OUTER JOIN T_Currency C  
  ON G.CurrencyID = C.CurrencyID  
  
 -- join to get yesterday business day                                                                                
 LEFT OUTER JOIN #AUECYesterDates AUECYesterDates  
  ON G.AUECID = AUECYesterDates.AUECID  
 LEFT OUTER JOIN #AUECBusinessDatesForEndDate AUECBusinessDatesForEndDate  
  ON G.AUECID = AUECBusinessDatesForEndDate.AUECID  
  
  /* Forex Price for Trade Date other than FX Trade */
 LEFT OUTER JOIN #FXConversionRates FXDayRatesForTradeDate ON (  
   FXDayRatesForTradeDate.FromCurrencyID = G.CurrencyID  
  AND FXDayRatesForTradeDate.ToCurrencyID = TC.LocalCurrency  
   AND DateDiff(d, G.AUECLocalDate, FXDayRatesForTradeDate.DATE) = 0  
			AND FXDayRatesForTradeDate.FundID = PT.FundID
    )  
	LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateTradeDate ON (
			ZeroFundFxRateTradeDate.FromCurrencyID = G.CurrencyID
			AND ZeroFundFxRateTradeDate.ToCurrencyID = TC.LocalCurrency
			AND DateDiff(d, G.AUECLocalDate, ZeroFundFxRateTradeDate.DATE) = 0
			AND ZeroFundFxRateTradeDate.FundID = 0
   )  
 /* Forex Price for Start Date other than FX Trade */  
 LEFT OUTER JOIN #FXConversionRates FXDayRatesForStartDate ON (  
   FXDayRatesForStartDate.FromCurrencyID = G.CurrencyID  
  AND FXDayRatesForStartDate.ToCurrencyID = TC.LocalCurrency  
   AND DateDiff(d, AUECYesterDates.YESTERDAYBIZDATE, FXDayRatesForStartDate.DATE) = 0  
			AND FXDayRatesForStartDate.FundID = PT.FundID
    )  
	LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateStartDate ON (
			ZeroFundFxRateStartDate.FromCurrencyID = G.CurrencyID
			AND ZeroFundFxRateStartDate.ToCurrencyID = TC.LocalCurrency
			AND DateDiff(d, AUECYesterDates.YESTERDAYBIZDATE, ZeroFundFxRateStartDate.DATE) = 0
			AND ZeroFundFxRateStartDate.FundID = 0
   )  
 /* Forex Price for End Date other than FX Trade */  
 LEFT OUTER JOIN #FXConversionRates FXDayRatesForEndDate ON (  
   FXDayRatesForEndDate.FromCurrencyID = G.CurrencyID  
  AND FXDayRatesForEndDate.ToCurrencyID = TC.LocalCurrency  
   AND DateDiff(d, AUECBusinessDatesForEndDate.YESTERDAYBIZDATE, FXDayRatesForEndDate.DATE) = 0  
			AND FXDayRatesForEndDate.FundID = PT.FundID
    )  
	LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateEndDate ON (
			ZeroFundFxRateEndDate.FromCurrencyID = G.CurrencyID
			AND ZeroFundFxRateEndDate.ToCurrencyID = TC.LocalCurrency
			AND DateDiff(d, AUECBusinessDatesForEndDate.YESTERDAYBIZDATE, ZeroFundFxRateEndDate.DATE) = 0
			AND ZeroFundFxRateEndDate.FundID = 0
   ) 

 /* Security Master DB Join */  
LEFT OUTER JOIN #SecMasterDataTempTable SM ON SM.TickerSymbol = PT.Symbol  
LEFT OUTER JOIN T_CompanyStrategy AS CompanyStrategy ON CompanyStrategy.CompanyStrategyID = PT.Level2ID  
LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON TC.CompanyFundID = CMFSSAA.CompanyFundID  
LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID  
LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CompanyStrategy.CompanyStrategyID = CMSSSAA.CompanyStrategyID  
 LEFT OUTER JOIN T_CompanyMasterStrategy CMS    ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID  
LEFT OUTER JOIN #TempSplitFactorForOpen SplitTab ON SplitTab.TaxlotID = PT.TaxlotID  
CROSS APPLY (
		SELECT CASE 
				WHEN FXDayRatesForTradeDate.RateValue IS NULL
					THEN CASE 
						WHEN ZeroFundFxRateTradeDate.RateValue IS NULL
							THEN 0
							ELSE ZeroFundFxRateTradeDate.RateValue
						END
					ELSE FXDayRatesForTradeDate.RateValue
				END
		) AS FXRatesForTradeDate(Val)
	CROSS APPLY (
		SELECT CASE 
				WHEN FXDayRatesForStartDate.RateValue IS NULL
					THEN CASE 
						WHEN ZeroFundFxRateStartDate.RateValue IS NULL
							THEN 0
							ELSE ZeroFundFxRateStartDate.RateValue
						END
					ELSE FXDayRatesForStartDate.RateValue
				END
		) AS FXRatesForStartDate(Val)
	CROSS APPLY (
		SELECT CASE 
				WHEN FXDayRatesForEndDate.RateValue IS NULL
					THEN CASE 
						WHEN ZeroFundFxRateEndDate.RateValue IS NULL
							THEN 0
							ELSE ZeroFundFxRateEndDate.RateValue
						END
					ELSE FXDayRatesForEndDate.RateValue
				END
		) AS FXRatesForEndDate(Val)
	CROSS APPLY (
		SELECT CASE 
				WHEN MPS.Finalmarkprice IS NULL
					THEN CASE 
						WHEN MPZeroStartDate.Finalmarkprice IS NULL
							THEN 0
							ELSE MPZeroStartDate.Finalmarkprice
						END
					ELSE MPS.Finalmarkprice
				END
		) AS MPStartDate(Val)
	CROSS APPLY (
		SELECT CASE 
				WHEN MPE.Finalmarkprice IS NULL
					THEN CASE 
						WHEN MPZeroEndDate.Finalmarkprice IS NULL
							THEN 0
							ELSE MPZeroEndDate.Finalmarkprice
						END
					ELSE MPE.Finalmarkprice
				END
		) AS MPEndDate(Val)
  
 WHERE TaxLotOpenQty <> 0                                                                                                                                              
 AND taxlot_PK IN (
SELECT
	MAX(Taxlot_PK)
FROM #PM_Taxlots
WHERE DATEDIFF(D, #PM_Taxlots.AUECModifiedDate, @EndDate) >= 0
GROUP BY TaxlotId)


/**************************************************************************                                    
       CLOSED POSITIONS HANDLING                                    
**************************************************************************/
INSERT INTO #MTMDataTable

	SELECT
		PTFundID AS FundID,
		PTSymbol AS Symbol,
		ClosedQty AS ClosedQty,
		PTAvgPrice AS AvgPrice,
		ISNULL(PT1AvgPrice, 0) AS ClosingPrice,
		GAssetID AS AssetID,
		GCurrencyID AS CurrencyID,
		C.CurrencySymbol AS CurrencySymbol,
		GAUECID AS AUECID,

		ISNULL(PTClosedTotalCommissionandFees, 0) AS TotalOpenCommission_Local,

		CASE
			WHEN GCurrencyID = TC.LocalCurrency THEN ISNULL(PTClosedTotalCommissionandFees, 0)
			ELSE  ----When Company and Traded Currency are different                                                                                   
			--  Case                                   
			--   When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                                                
			--   Then IsNull(PTClosedTotalCommissionandFees * G.FXRate,0)                                                                                                                                                                                               





			--   When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                                                                            
			--   Then IsNull(PTClosedTotalCommissionandFees * 1/G.FXRate,0)                                                                                                                                                                                             





			--   When G.FXRate <= 0 OR G.FXRate is null                                                                                                                                                                                                                 

			--   Then  IsNull(PTClosedTotalCommissionandFees * IsNull(FXDayRatesForTradeDate.RateValue,0),0)                                                                                                                                                            
			--  End                          
			ISNULL(PTClosedTotalCommissionandFees * OpeningFXRate, 0)
		END AS TotalOpenCommission_Base,

		ISNULL(PT1ClosedTotalCommissionandFees, 0) AS TotalClosedCommission_Local,





		CASE
			WHEN GCurrencyID = TC.LocalCurrency THEN ISNULL(PT1ClosedTotalCommissionandFees, 0)
			ELSE  --When Company and Traded Currency are different                                                                                                         
			--  Case                                                     
			--   When G1.FXRate > 0 And G1.FXConversionMethodOperator='M'                                                                                                                                       
			--   Then IsNull(PT1.ClosedTotalCommissionandFees * G1.FXRate,0)                                                                                                                                                                                            





			--   When G1.FXRate > 0 And G1.FXConversionMethodOperator='D'                                                                                 
			--   Then IsNull(PT1.ClosedTotalCommissionandFees * 1/G1.FXRate,0)                                                                                                                                                  
			--  When G1.FXRate <= 0 OR G1.FXRate is null                                                                                                                     
			--   Then  IsNull(PT1.ClosedTotalCommissionandFees * IsNull(FXDayRatesForClosingDate.RateValue,0),0)                                                                                            
			--  End                          
			ISNULL(PT1ClosedTotalCommissionandFees * EndingFXRate, 0)
		END AS TotalClosedCommission_Base,

		SM.Multiplier AS AssetMultiplier,
		GProcessDate AS TradeDate,
		AUECLocalDate AS ClosingDate, --now closing taxlot Trade date is cloisng date                                                                                                                                                                           
		ISNULL(MPStartDate.Val, 0) Mark1,
		ISNULL(MPEndDate.Val, 0) Mark2,
		GIsSwapped,
		ISNULL(SW.NotionalValue * ((PT1TaxLotOpenQty + ClosedQty) /
			CASE
				WHEN G1CumQty = 0 THEN 1
				ELSE G1CumQty
			END), 0) AS NotionalValue,


		ISNULL(SW.BenchMarkRate, 0) AS BenchMarkRate,
		ISNULL(SW.Differential, 0) AS Differential,
		ISNULL(SW.OrigCostBasis, 0) AS OrigCostBasis,
		ISNULL(SW.DayCount, 0) AS DayCount,
		SW1.FirstResetDate AS FirstResetDate,
		SW1.OrigTransDate AS OrigTransDate,

		CASE GIsSwapped
			WHEN 1 THEN CASE
				WHEN GCurrencyID = TC.LocalCurrency THEN (((ISNULL(SW.NotionalValue * ((PTTaxLotOpenQty + ClosedQty) /
					CASE
						WHEN GCumQty = 0 THEN 1
						ELSE GCumQty
					END), 0) * (ISNULL(SW.BenchMarkRate, 0) + ISNULL(SW.Differential, 0)) * DATEDIFF(D, SW.OrigTransDate, AUECLocalDate)) / 100) / SW.DayCount)
				* dbo.GetSideMultiplier(PTOrderSideTagValue) * ClosingDateFXRate
				ELSE (((ISNULL(SW.NotionalValue * ((PTTaxLotOpenQty + ClosedQty) /
					CASE
						WHEN GCumQty = 0 THEN 1
						ELSE GCumQty
					END), 0) * (ISNULL(SW.BenchMarkRate, 0) + ISNULL(SW.Differential, 0)) * DATEDIFF(D, SW.OrigTransDate, AUECLocalDate)) / 100) / SW.DayCount)
				* dbo.GetSideMultiplier(PTOrderSideTagValue)
			END
			ELSE 0
		END AS SwapData,

		CASE
			WHEN DATEDIFF(D, GProcessDate, @StartDate) > 0 THEN (ISNULL(MPStartDate.Val, 0) / ISNULL(SplitTab.SplitFactor, 1)) * ClosedQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PTOrderSideTagValue)
			ELSE (PTAvgPrice) * ClosedQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PTOrderSideTagValue)
		END AS BeginningMarketValue_Local,

		CASE
			WHEN GCurrencyID = TC.LocalCurrency THEN CASE
				WHEN DATEDIFF(D, GProcessDate, @StartDate) > 0 THEN (ISNULL(MPStartDate.Val, 0) / ISNULL(SplitTab.SplitFactor, 1)) * ClosedQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PTOrderSideTagValue)
				ELSE (PTAvgPrice) * ClosedQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PTOrderSideTagValue)
			END
			ELSE CASE
				WHEN DATEDIFF(D, GProcessDate, @StartDate) > 0 THEN (ISNULL(MPStartDate.Val, 0) / ISNULL(SplitTab.SplitFactor, 1)) * StartDateFXRate * ClosedQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PTOrderSideTagValue)
				ELSE ISNULL((PTAvgPrice) * OpeningFXRate * ClosedQty * ISNULL(SM.Multiplier, 0) * dbo.GetSideMultiplier(PTOrderSideTagValue), 0)
			END
		END AS BeginningMarketValue_Base,

		ISNULL(PT1AvgPrice, 0) * ClosedQty * ISNULL(SM.Multiplier, 0) * ISNULL(dbo.GetSideMultiplierForClosing(GOrderSideTagValue, G1OrderSideTagValue), 1) AS EndingMarketValue_Local,

		CASE
			WHEN GCurrencyID <> TC.LocalCurrency THEN ISNULL(PT1AvgPrice, 0) * EndingFXRate * ClosedQty * ISNULL(SM.Multiplier, 0) * ISNULL(dbo.GetSideMultiplierForClosing(GOrderSideTagValue, G1OrderSideTagValue), 1)
			ELSE ISNULL(PT1AvgPrice, 0) * ClosedQty * ISNULL(SM.Multiplier, 0) * ISNULL(dbo.GetSideMultiplierForClosing(GOrderSideTagValue, G1OrderSideTagValue), 1)
		END AS EndingMarketValue_Base,

		'C' AS Open_CloseTag,

		--Case                                                                                                                                         
		-- When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                                                                                         
		-- Then G.FXRate                                              
		-- When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                                                   
		-- Then 1/G.FXRate                                              
		-- When G.FXRate <= 0 OR G.FXRate is null                                                              
		-- Then IsNull(FXDayRatesForTradeDate.RateValue,0)                                     
		--End                         
		OpeningFXRate AS ConversionRateTrade,

		--IsNull(FXDayRatesForStartDate.RateValue,0) as ConversionRateStart,                                                              
		--IsNull(FXDayRatesForClosingDate.RateValue,0) as ConversionRateClosing,                                       

		StartDateFXRate AS ConversionRateStart,

		CASE
			WHEN GCurrencyID = TC.LocalCurrency THEN 1
			ELSE ClosingDateFXRate
		END AS ConversionRateClosing,

		SM.CompanyName,

		TC.FundName AS FundName,
		ISNULL(CompanyStrategy.StrategyName, 'Strategy Unallocated') AS StrategyName,




		CASE dbo.GetSideMultiplierForClosing(GOrderSideTagValue, G1OrderSideTagValue)
			WHEN 1 THEN 'Long'
			WHEN -1 THEN 'Short'
			ELSE ''
		END AS Side,

		#T_Asset.AssetName AS Asset,
		ISNULL(SM.AssetName, 'Undefined') AS UDAAsset,
		ISNULL(SM.SecurityTypeName, 'Undefined') AS UDASecurityTypeName,
		ISNULL(SM.SectorName, 'Undefined') AS UDASectorName,
		ISNULL(SM.SubSectorName, 'Undefined') AS UDASubSectorName,
		ISNULL(SM.CountryName, 'Undefined') AS UDACountryName,
		ISNULL(SM.PutOrCall, '') AS PutOrCall,
		ISNULL(CMF.MasterFundName, 'Unassigned') AS MasterFundName,
		0 AS Dividend,
		ISNULL(CMS.MasterStrategyName, 'Unassigned') AS MasterStrategyName,
		ISNULL(SM.UnderlyingSymbol, '') AS UnderlyingSymbol,
		0 AS CashFXUnrealizedPNL,
		0 AS NetTotalCost_Base,
		SM.BloombergSymbol,
		SM.SedolSymbol,
		SM.ISINSymbol,
		SM.CusipSymbol,
		SM.OSISymbol,
		SM.IDCOSymbol,
		0 AS UnrealizedPNLMTM_Local,
		0 AS RealizedPNLMTM_Local,
		0 AS UnrealizedPNLMTM_Base,
		0 AS RealizedPNLMTM_Base,
		CAST(FLOOR(CAST(SM.ExpirationDate AS float)) AS datetime) AS ExpirationDate,
		GTradeAttribute1 AS OpenTradeAttribute1,
		GTradeAttribute2 AS OpenTradeAttribute2,
		GTradeAttribute3 AS OpenTradeAttribute3,
		GTradeAttribute4 AS OpenTradeAttribute4,
		GTradeAttribute5 AS OpenTradeAttribute5,
		GTradeAttribute6 AS OpenTradeAttribute6,
		G1TradeAttribute1 AS ClosedTradeAttribute1,
		G1TradeAttribute2 AS ClosedTradeAttribute2,
		G1TradeAttribute3 AS ClosedTradeAttribute3,
		G1TradeAttribute4 AS ClosedTradeAttribute4,
		G1TradeAttribute5 AS ClosedTradeAttribute5,
		G1TradeAttribute6 AS ClosedTradeAttribute6,
  TC.LocalCurrency AS BaseCurrencyID,
  SM.SecurityName as SecurityName

	--  from PM_TaxlotClosing  PTC                                                                            
	--  Inner Join #PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)            




	--  Inner Join #PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)                                                                                                                                   
	--  Inner Join T_Group G on G.GroupID = PT.GroupID                      
	--  Inner Join T_Group G1 on G1.GroupID = PT1.GroupID                          
	FROM #TempClosingData
	INNER JOIN #T_CompanyFunds TC
		ON #TempClosingData.PTFundID = TC.CompanyFundID
	INNER JOIN #T_Asset
		ON #T_Asset.AssetId = #TempClosingData.GAssetID
	--Inner Join T_AUEC AUEC on G.AUECID = AUEC.AUECID     

	LEFT OUTER JOIN #MarkPriceForStartDate MPS ON (
			MPS.Symbol = #TempClosingData.PTSymbol
			AND MPS.FundID = #TempClosingData.PTFundID
			)
	LEFT OUTER JOIN #ZeroFundMarkPriceStartDate MPZeroStartDate ON (
			#TempClosingData.PTSymbol = MPZeroStartDate.Symbol
			AND MPZeroStartDate.FundID = 0
			)
	LEFT OUTER JOIN #MarkPriceForEndDate MPE ON (
			MPE.Symbol = #TempClosingData.PTSymbol
			AND MPE.FundID = #TempClosingData.PTFundID
			)
	LEFT OUTER JOIN #ZeroFundMarkPriceEndDate MPZeroEndDate ON (
			#TempClosingData.PTSymbol = MPZeroEndDate.Symbol
			AND MPZeroEndDate.FundID = 0
			)

	--Left Outer Join #MarkPriceForStartDate MPS on MPS.Symbol=#TempClosingData.PTSymbol                                                                         
	--Left Outer Join #MarkPriceForEndDate MPE on MPE.Symbol=#TempClosingData.PTSymbol                                                                            
	--  --get yesterday business day                                                                                                                                      
	--  LEFT OUTER JOIN #AUECYesterDates AUECYesterDates ON #TempClosingData.GAUECID = AUECYesterDates.AUECID                                                                                                            
	-- Security Master DB join                                                                                                                                                        
	LEFT OUTER JOIN #SecMasterDataTempTable SM
		ON SM.TickerSymbol = #TempClosingData.PTSymbol
	LEFT OUTER JOIN T_SwapParameters SW
		ON SW.GroupID = #TempClosingData.GGroupID
	LEFT OUTER JOIN T_Currency C
		ON GCurrencyID = C.CurrencyID

	LEFT OUTER JOIN T_SwapParameters SW1
		ON SW1.GroupID = #TempClosingData.G1GroupID
	LEFT OUTER JOIN T_CompanyStrategy AS CompanyStrategy
		ON CompanyStrategy.CompanyStrategyID = #TempClosingData.PTLevel2ID
	LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA
		ON TC.CompanyFundID = CMFSSAA.CompanyFundID

	LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID
	LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CompanyStrategy.CompanyStrategyID = CMSSSAA.CompanyStrategyID
	LEFT OUTER JOIN T_CompanyMasterStrategy CMS ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID
	LEFT OUTER JOIN #TempSplitFactorForClosed SplitTab ON SplitTab.TaxlotID = #TempClosingData.PTTaxlotID
	AND DATEDIFF(DAY, #TempClosingData.PTAUECModifiedDate, SplitTab.Effectivedate) <= 0
CROSS APPLY (
		SELECT CASE 
				WHEN MPS.Finalmarkprice IS NULL
					THEN CASE 
						WHEN MPZeroStartDate.Finalmarkprice IS NULL
							THEN 0
							ELSE MPZeroStartDate.Finalmarkprice
						END
					ELSE MPS.Finalmarkprice
				END
		) AS MPStartDate(Val)
	CROSS APPLY (
		SELECT CASE 
				WHEN MPE.Finalmarkprice IS NULL
					THEN CASE 
						WHEN MPZeroEndDate.Finalmarkprice IS NULL
							THEN 0
							ELSE MPZeroEndDate.Finalmarkprice
						END
					ELSE MPE.Finalmarkprice
				END
		) AS MPEndDate(Val)	

	WHERE ClosingMode <> 7 --and SSymbol.FutSymbol is null --7 means CoperateAction!                                                                                                                                  


/**************************************************************************                                    
       DIVIDEND HANDLING                                    
**************************************************************************/
INSERT INTO #MTMDataTable
	SELECT
		ISNULL(CashDiv.FundId, 0) AS FundID,
		CashDiv.Symbol,
		0 AS TaxLotOpenQty,
		0 AS AvgPrice,
		0 AS ClosingPrice,
		MIN(ISNULL(SM.AssetID, 0)) AS AssetID,
		MIN(CashDiv.CurrencyID) AS CurrencyID,
		MIN(C.CurrencySymbol) AS CurrencySymbol,
		MIN(ISNULL(SM.AUECID, 0)) AS AUECID,
		0 AS TotalOpenCommission_Local,
		0 AS TotalOpenCommission_Base,
		0 AS TotalClosedCommission_Local,
		0 AS TotalClosedCommission_Base,
		0 AS AssetMultiplier,
		MIN(CashDiv.ExDate) AS TradeDate,
		'1800-01-01 00:00:00.000' AS ClosingDate,
		0 AS Mark1,
		0 AS Mark2,
		0 AS IsSwapped,
		0 AS NotionalValue,
		0 AS BenchMarkRate,
		0 AS Differential,
		0 AS OrigCostBasis,
		0 AS DayCount,
		'1800-01-01 00:00:00.000' AS FirstResetDate,
		'1800-01-01 00:00:00.000' AS OrigTransDate,
		0 AS SwapData,
		0 AS BeginningMarketValue_Local,
		0 AS BeginningMarketValue_Base,
		0 AS EndingMarketValue_Local,
		0 AS EndingMarketValue_Base,
		'D' AS Open_CloseTag,
--		MAX(ISNULL(FXDayRatesForDiviDate.RateValue, 0)) AS ConversionRateTrade,
		Max(IsNull(FXDayRatesForDiviDate.RateValue, ISNULL(ZeroFundFxRateDiviDate.RateValue,0))) AS ConversionRateTrade, 
		0 AS ConversionRateStart,
		0 AS ConversionRateEnd,
		MIN(SM.CompanyName) AS CompanyName,
		MIN(TC.FundName) AS FundName,
		'Strategy Unallocated' AS StrategyName,
		CASE
			WHEN SUM(CashDiv.Amount) >= 0 THEN 'Long'
			ELSE 'Short'
		END AS Side,
		MIN(ISNULL(#T_Asset.AssetName, 'Undefined')) AS Asset,
		MIN(ISNULL(SM.AssetName, 'Undefined')) AS UDAAsset,
		MIN(ISNULL(SM.SecurityTypeName, 'Undefined')) AS UDASecurityTypeName,
		MIN(ISNULL(SM.SectorName, 'Undefined')) AS UDASectorName,
		MIN(ISNULL(SM.SubSectorName, 'Undefined')) AS UDASubSectorName,
		MIN(ISNULL(SM.CountryName, 'Undefined')) AS UDACountryName,
		MIN(ISNULL(SM.PutOrCall, '')) AS PutOrCall,
		MIN(ISNULL(CMF.MasterFundName, 'Unassigned')) AS MasterFundName,
		CASE
			WHEN MIN(CashDiv.CurrencyID) = MIN(TC.LocalCurrency) THEN SUM(CashDiv.Amount)
--			ELSE MAX(ISNULL(FXDayRatesForDiviDate.RateValue, 0)) * SUM(CashDiv.Amount)
			Else Max(IsNull(FXDayRatesForDiviDate.RateValue, ISNULL(ZeroFundFxRateDiviDate.RateValue,0))) * SUM(CashDiv.Amount) 
		END AS Dividend,
		'Unassigned' AS MasterStrategyName,
		MIN(ISNULL(SM.UnderlyingSymbol, '')) AS UnderlyingSymbol,
		0 AS CashFXUnrealizedPNL,
		0 AS NetTotalCost_Base,
		MAX(SM.BloombergSymbol) AS BloombergSymbol,
		MAX(SM.SedolSymbol) AS SedolSymbol,
		MAX(SM.ISINSymbol) AS ISINSymbol,
		MAX(SM.CusipSymbol) AS CusipSymbol,
		MAX(SM.OSISymbol) AS OSISymbol,
		MAX(SM.IDCOSymbol) AS IDCOSymbol,
		0 AS UnrealizedPNLMTM_Local,
		0 AS RealizedPNLMTM_Local,
		0 AS UnrealizedPNLMTM_Base,
		0 AS RealizedPNLMTM_Base,
		MIN(CAST(FLOOR(CAST(SM.ExpirationDate AS float)) AS datetime)) AS ExpirationDate,
		'' AS OpenTradeAttribute1,
		'' AS OpenTradeAttribute2,
		'' AS OpenTradeAttribute3,
		'' AS OpenTradeAttribute4,
		'' AS OpenTradeAttribute5,
		'' AS OpenTradeAttribute6,
		'' AS ClosedTradeAttribute1,
		'' AS ClosedTradeAttribute2,
		'' AS ClosedTradeAttribute3,
		'' AS ClosedTradeAttribute4,
		'' AS ClosedTradeAttribute5,
		'' AS ClosedTradeAttribute6,
		--min(SM.ExpirationDate) as ExpirationDate     
  MIN(TC.LocalCurrency) AS BaseCurrencyID,
  MAX(SM.SecurityName) AS SecurityName    

	FROM T_CashTransactions CashDiv
	INNER JOIN T_ActivityType
		ON (T_ActivityType.ActivityTypeId = CashDiv.ActivityTypeId
		AND ActivitySource = 2)
	INNER JOIN #T_CompanyFunds TC
		ON CashDiv.FundID = TC.CompanyFundID
	LEFT OUTER JOIN #SecMasterDataTempTable SM
		ON SM.TickerSymbol = CashDiv.Symbol
	LEFT OUTER JOIN T_Currency C
		ON CashDiv.CurrencyID = C.CurrencyID

	INNER JOIN #T_Asset
		ON #T_Asset.AssetID = SM.AssetId
	--Left outer Join T_AUEC AUEC On AUEC.AUECID=SM.AUECID                                                            
	LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA
		ON TC.CompanyFundID = CMFSSAA.CompanyFundID
	LEFT OUTER JOIN T_CompanyMasterFunds CMF
		ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID

LEFT OUTER JOIN #FXConversionRates FXDayRatesForDiviDate ON (  
	FXDayRatesForDiviDate.FromCurrencyID = CashDiv.CurrencyID   
		AND FXDayRatesForDiviDate.ToCurrencyID = TC.LocalCurrency
	AND DateDiff(d, CashDiv.ExDate, FXDayRatesForDiviDate.DATE) = 0  
		AND FXDayRatesForDiviDate.FundID = CashDiv.FundID
	)  
LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateDiviDate ON (
	ZeroFundFxRateDiviDate.FromCurrencyID = CashDiv.CurrencyID   
 AND ZeroFundFxRateDiviDate.ToCurrencyID = TC.LocalCurrency
	AND DateDiff(d, CashDiv.ExDate, ZeroFundFxRateDiviDate.DATE) = 0
		AND ZeroFundFxRateDiviDate.FundID = 0
	)  
WHERE DATEDIFF(D, @StartDate, CashDiv.ExDate) >= 0 AND DATEDIFF(D, CashDiv.ExDate, @EndDate) >= 0
GROUP BY CashDiv.FundId,CashDiv.Symbol,CashDiv.ExDate,CashDiv.CurrencyID

/**************************************************************************                                    
      NAME CHANGE HANDLING  (This Part is commented by Sandeep Singh on 12 DEC 2014. It will be handle in Jan 15 Release)                              
**************************************************************************/
--Insert into #MTMDataTable                                                                                            
--select                                                                                                        
--  PTFundID as FundID,                                                             
--  PTSymbol as Symbol,                                                                                                               
--  ClosedQty as ClosedQty ,                                                                                                             
--  PTAvgPrice as AvgPrice ,                                                                                                           
--  IsNull(PT1AvgPrice,0)as ClosingPrice ,                                                                                                                                                                                                               
--  GAssetID as AssetID,                                                                                                                                     
--  GCurrencyID as CurrencyID,       
--  C.CurrencySymbol as CurrencySymbol,                                                                   
--  GAUECID as AUECID,                                                                                    
--                                                                                                                          
--IsNull(PTOpenTotalCommissionandFees,0) As  TotalOpenCommission_Local,                                                                                                             
--          
--Case                                
-- When GCurrencyID =  @BaseCurrencyID   --When Company and Traded Currency both are same                                                  
-- Then IsNull(PTOpenTotalCommissionandFees,0)                                                                              
-- Else  ----When Company and Traded Currency are different                                                                                                    
---- Case                                                                    
----  When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                             
----  Then IsNull(PTOpenTotalCommissionandFees * G.FXRate,0)                               
----  When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                              
----  Then IsNull(PTOpenTotalCommissionandFees * 1/G.FXRate,0)                                                                                             
----  When G.FXRate <= 0 OR G.FXRate is null                                                                                                                                                                      
----  Then  IsNull(PTOpenTotalCommissionandFees * IsNull(FXDayRatesForTradeDate.RateValue,0),0)                                                                                                                                           
---- End                       
--IsNull(PTOpenTotalCommissionandFees * OpeningFXRate,0)                                                                                                                                                                                           
--End as TotalOpenCommission_Base,                       
--                                                                                                                        
-- --Closed Commission                                                                   
--0 as TotalClosedCommission_Local,                                                                    
--0 as TotalClosedCommission_Base,                                                                        
--SM.Multiplier as AssetMultiplier,                                                                                                                                                                          
--GProcessDate as TradeDate,                                                                                                                                                         
--AUECLocalDate  as ClosingDate, --now closing taxlot Trade date is cloisng date                                                                                                                                                                     
--IsNull(MPS.FinalMarkPrice,0) as Mark1,                                                       
--IsNull(MPE.FinalMarkPrice,0) As Mark2,                                                                                                                                                
--0 as IsSwapped,                                                            
--0 as NotionalValue,                                                                                                                                                                        
--0 as BenchMarkRate,                                                                   
--0 as Differential,                                                                                                                                                            
--0 as OrigCostBasis,                                                                          
--0 as DayCount,                                                                                                         
--'1800-01-01 00:00:00.000'  as FirstResetDate,                                                                                                                                                                                                      
--'1800-01-01 00:00:00.000'  as OrigTransDate ,                                                                                                             
--0 as SwapData,                                                                                                                                                             
--                                                                                                                                                                        
--Case                                                                                                                                                                   
-- When DateDiff(d,GProcessDate,@StartDate) > 0                                                                                                
-- Then IsNull(MPS.FinalMarkPrice,0) * ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue)                                                                                                                                       

-- Else PTAvgPrice * ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue)                                                                     
--End As BeginningMarketValue_Local ,                       
--                                                                                                                                                            
--Case                                                                                                                                                                     
--When GCurrencyID =  @BaseCurrencyID                                                                                                                                                    
--Then                              
-- Case                                                                        
-- When DateDiff(d,GProcessDate,@StartDate) > 0                                                 
-- Then IsNull(MPS.FinalMarkPrice,0) * ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue)                                                                                                                                        

-- Else PTAvgPrice * ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue)                                                                                                                                                          

-- End                           
-- Else                                                   
--   Case                                                                                                                 
--   When  DateDiff(d,GProcessDate,@StartDate) > 0                                                                
--   Then IsNull(MPS.FinalMarkPrice,0) * StartDateFXRate * ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue)                                                                                                                    

--   Else IsNull(PTAvgPrice * OpeningFXRate * ClosedQty  * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue),0)                                                                                                                             

--   End                         
--End as BeginningMarketValue_Base ,                    
--                                                                     
--ISNULL(PT1AvgPrice,0)* ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(GOrderSideTagValue,G1OrderSideTagValue),1) as  EndingMarketValue_Local,                                                        
--                        
-- Case                                                                               
--When GCurrencyID <> @BaseCurrencyID                                                                         
-- Then IsNull(PT1AvgPrice,0)* EndingFXRate * ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(GOrderSideTagValue,G1OrderSideTagValue),1)                                                                                          

-- Else ISNULL(PT1AvgPrice,0)* ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(GOrderSideTagValue,G1OrderSideTagValue),1)                                                                                                         

--End as  EndingMarketValue_Base,                                                                
--                                                                                                    
--'O' as Open_CloseTag,               
----IsNull(FXDayRatesForTradeDate.RateValue,0) As ConversionRateTrade,                                                                                               
----IsNull(FXDayRatesForStartDate.RateValue,0) as ConversionRateStart,                                                                                                                                                          
----IsNull(FXDayRatesForClosingDate.RateValue,0) as ConversionRateClosing,                    
--                     
--TradeDateFXRate As ConversionRateTrade,                                                                                                                                      
--StartDateFXRate as ConversionRateStart,                       
--ClosingDateFXRate as ConversionRateClosing,                    
--                                                                                           
--SM.CompanyName,                                                                                                                                                                                                                                              

--    
--     
--         
--                                                                                
--#T_CompanyFunds.FundName  as FundName,                                                                                                                                                                                     
--IsNull(CompanyStrategy.StrategyName,'Strategy Unallocated') AS StrategyName,                                                                                              
--               
--Case dbo.GetSideMultiplierForClosing(GOrderSideTagValue,G1OrderSideTagValue)                                                                                                                                                                                 

--   
--  When  1                                                                                                            
--  Then  'Long'                                                                                                                           
--  When  -1                                                                                                            
--  Then  'Short'                                                                                                                                                                                        
--  Else  ''                                                                          
--End as Side,                                                                                                                             
--                                                                                                                                      
--#T_Asset.AssetName as Asset,                                                       
--IsNull(SM.AssetName,'Undefined') as UDAAsset,                                             
--IsNull(SM.SecurityTypeName,'Undefined') as UDASecurityTypeName,                                                                               
--IsNull(SM.SectorName,'Undefined') as UDASectorName,                                                                                                                                                                                     
--IsNull(SM.SubSectorName,'Undefined') as UDASubSectorName,                            
--IsNull(SM.CountryName,'Undefined') as UDACountryName,                                                                      
--IsNUll(SM.PutOrCall,'') as PutOrCall,                                                                                                                                                                                            
--IsNull(CMF.MasterFundName,'Unassigned') As MasterFundName,                                                                                                    
--0 as Dividend,                                                                           
--IsNull(CMS.MasterStrategyName,'Unassigned') as MasterStrategyName,                                                                            
--IsNull(SM.UnderlyingSymbol,'') as UnderlyingSymbol,                                                    
--0 as CashFXUnrealizedPNL,                        
--0 As NetTotalCost_Base,                    
--SM.BloombergSymbol,                              
--SM.SedolSymbol,                              
--SM.ISINSymbol,                              
--SM.CusipSymbol,                              
--SM.OSISymbol,                              
--SM.IDCOSymbol,          
--0 As UnrealizedPNLMTM_Local,          
--0 As RealizedPNLMTM_Local,          
--0 As UnrealizedPNLMTM_Base,          
--0 As RealizedPNLMTM_Base     ,  
-- CAST(FLOOR(CAST(SM.ExpirationDate AS FLOAT ) ) AS DATETIME ) as ExpirationDate,  
--GTradeAttribute1 as OpenTradeAttribute1,  
--GTradeAttribute2 as OpenTradeAttribute2,  
--GTradeAttribute3 as OpenTradeAttribute3,  
--GTradeAttribute4 as OpenTradeAttribute4,  
--GTradeAttribute5 as OpenTradeAttribute5,  
--GTradeAttribute6 as OpenTradeAttribute6,     
--G1TradeAttribute1 As ClosedTradeAttribute1 ,  
--G1TradeAttribute2 As ClosedTradeAttribute2 ,  
--G1TradeAttribute3 As ClosedTradeAttribute3 ,  
--G1TradeAttribute4 As ClosedTradeAttribute4 ,  
--G1TradeAttribute5 As ClosedTradeAttribute5 ,  
--G1TradeAttribute6 As ClosedTradeAttribute6   
----SM.ExpirationDate                                                                                                                  
--                                                                                                                                                                     
--                      
--from #TempClosingData                    
--  Inner Join #T_CompanyFunds ON  #TempClosingData.PTFundID= #T_CompanyFunds.CompanyFundID                             
--  Inner Join #T_Asset On #T_Asset.AssetId=#TempClosingData.GAssetID                                                                                                                                                                                         

--  Left OUTER JOIN T_Currency C on GCurrencyID = C.CurrencyID  
--Left Outer Join #MarkPriceForStartDate MPS on MPS.Symbol=#TempClosingData.PTSymbol                                                                                  
--Left Outer Join #MarkPriceForEndDate MPE on MPE.Symbol=#TempClosingData.PTSymbol                                                                                                                           
--  --get yesterday business day                                                                                                                      
--  LEFT OUTER JOIN #AUECYesterDates AUECYesterDates ON #TempClosingData.GAUECID = AUECYesterDates.AUECID                                                                                                                        
--  -- Security Master DB join                                                     
--  LEFT OUTER JOIN #SecMasterDataTempTable SM ON SM.TickerSymbol = #TempClosingData.PTSymbol                                                                                                                     
-- Left Outer Join T_CompanyStrategy as CompanyStrategy On CompanyStrategy.CompanyStrategyID=#TempClosingData.PTLevel2ID                                                                                                                                       

--    
-- LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON #T_CompanyFunds.CompanyFundID = CMFSSAA.CompanyFundID                                                                                        
-- LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                                                                          
-- LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CompanyStrategy.CompanyStrategyID = CMSSSAA.CompanyStrategyID                                                                                                                       

--    
-- LEFT OUTER JOIN T_CompanyMasterStrategy CMS ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID                                                                         
-- Where ClosingMode=7 --7 means CoperateAction!                                                                                                        


/**************************************************************************                                    
       CASH HANDLING                                    
**************************************************************************/
INSERT INTO #MTMDataTable
	SELECT
		ISNULL(DailyCash.FundId, 0) AS FundID,
		MIN(CLocal.CurrencySymbol) AS Symbol,
		0 AS TaxLotOpenQty,
		0 AS AvgPrice,
		0 AS ClosingPrice,
		6 AS AssetID,
		MIN(DailyCash.LocalCurrencyID) AS CurrencyID,
		MIN(CLocal.CurrencySymbol) AS CurrencySymbol,
		0 AS AUECID,
		0 AS TotalOpenCommission_Local,
		0 AS TotalOpenCommission_Base,
		0 AS TotalClosedCommission_Local,
		0 AS TotalClosedCommission_Base,
		1 AS AssetMultiplier,
		MAX(DailyCash.Date) AS TradeDate,
		'1800-01-01 00:00:00.000' AS ClosingDate,
		0 AS Mark1,
		0 AS Mark2,
		0 AS IsSwapped,
		0 AS NotionalValue,
		0 AS BenchMarkRate,
		0 AS Differential,
		0 AS OrigCostBasis,
		0 AS DayCount,
		'1800-01-01 00:00:00.000' AS FirstResetDate,
		'1800-01-01 00:00:00.000' AS OrigTransDate,
		0 AS SwapData,
		0 AS BeginningMarketValue_Local,
		0 AS BeginningMarketValue_Base,
		SUM(ISNULL(DailyCash.CashValueLocal, 0)) AS EndingMarketValue_Local,
		SUM(ISNULL(DailyCash.CashValueBase, 0)) AS EndingMarketValue_Base,
		'CASH' AS Open_CloseTag,
		0 AS ConversionRateTrade,
Min(
	CASE 
	WHEN FXDayRatesForStartDate.RateValue IS NULL
	THEN (IsNull(ZeroFundFxRateStartDate.RateValue, 0))
	ELSE FXDayRatesForStartDate.RateValue
	END
) As ConversionRateStart,

Min(
	CASE 
		WHEN FXDayRatesForEndDate.RateValue IS NULL
		THEN (IsNull(ZeroFundFxRateEndDate.RateValue, 0))
		ELSE FXDayRatesForEndDate.RateValue
	END
) AS ConversionRateEnd,

		'Undefined' AS CompanyName,
		MIN(CF.FundName) AS FundName,
		'Strategy Unallocated' AS StrategyName,
		CASE
			WHEN SUM(DailyCash.CashValueLocal) >= 0 THEN 'Long'
			ELSE 'Short'
		END AS Side,

		'Cash' AS Asset,
		'Undefined' AS UDAAsset,
		'Undefined' AS UDASecurityTypeName,
		'Undefined' AS UDASectorName,
		'Undefined' AS UDASubSectorName,
		'Undefined' AS UDACountryName,
		'' AS PutOrCall,
		--'Undefined' as MasterFundName,                                   
		ISNULL(CMF.MasterFundName, 'Unassigned') AS MasterFundName,
		0 AS Dividend,
		'Unassigned' AS MasterStrategyName,
		'' AS UnderlyingSymbol,
		CASE
			WHEN DailyCash.LocalCurrencyID <> CF.LocalCurrency 
			THEN (Min(COALESCE(FXDayRatesForEndDate.RateValue,ISNULL(ZeroFundFxRateEndDate.RateValue,0))) - 
				Min(COALESCE(FXDayRatesForStartDate.RateValue,ISNULL(ZeroFundFxRateStartDate.RateValue,0)))) * SUM(DailyCash.CashValueLocal)
			ELSE 0
		END AS CashFXUnrealizedPNL,
		0 AS NetTotalCost_Base,
		'' AS BloombergSymbol,
		'' AS SedolSymbol,
		'' AS ISINSymbol,
		'' AS CusipSymbol,
		'' AS OSISymbol,
		'' AS IDCOSymbol,
		0 AS UnrealizedPNLMTM_Local,
		0 AS RealizedPNLMTM_Local,
		0 AS UnrealizedPNLMTM_Base,
		0 AS RealizedPNLMTM_Base,
		'1800-01-01' AS ExpirationDate,
		'' AS OpenTradeAttribute1,
		'' AS OpenTradeAttribute2,
		'' AS OpenTradeAttribute3,
		'' AS OpenTradeAttribute4,
		'' AS OpenTradeAttribute5,
		'' AS OpenTradeAttribute6,
		'' AS ClosedTradeAttribute1,
		'' AS ClosedTradeAttribute2,
		'' AS ClosedTradeAttribute3,
		'' AS ClosedTradeAttribute4,
		'' AS ClosedTradeAttribute5,
		'' AS ClosedTradeAttribute6,
		--'1800-01-01 00:00:00.000' as ExpirationDate                                  
  CF.LocalCurrency AS BaseCurrencyID, 
  '' as SecurityName   
	--From T_DayEndBalances DailyCash               
	FROM PM_companyFundCashCurrencyValue DailyCash
	INNER JOIN #T_CompanyFunds CF
		ON DailyCash.FundID = CF.CompanyFundID
	INNER JOIN T_Currency CLocal
		ON DailyCash.LocalCurrencyID = CLocal.CurrencyID
	INNER JOIN T_Currency CBase
		ON BaseCurrencyID = CBase.CurrencyID
	LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA
		ON CF.CompanyFundID = CMFSSAA.CompanyFundID

	LEFT OUTER JOIN T_CompanyMasterFunds CMF
		ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID

LEFT OUTER JOIN #FXConversionRates FXDayRatesForStartDate ON (  
  FXDayRatesForStartDate.FromCurrencyID = DailyCash.LocalCurrencyID  
		AND FXDayRatesForStartDate.ToCurrencyID = CF.LocalCurrency
  AND DateDiff(d, dbo.AdjustBusinessDays(@StartDate, -1, @DefaultAUECID), FXDayRatesForStartDate.DATE) = 0  
		AND FXDayRatesForStartDate.FundID = DailyCash.FundID
   )  
LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateStartDate ON (
		ZeroFundFxRateStartDate.FromCurrencyID = DailyCash.LocalCurrencyID
		AND ZeroFundFxRateStartDate.ToCurrencyID = CF.LocalCurrency
		AND DateDiff(d, dbo.AdjustBusinessDays(@StartDate, -1, @DefaultAUECID), ZeroFundFxRateStartDate.DATE) = 0
		AND ZeroFundFxRateStartDate.FundID = 0
  )  
LEFT OUTER JOIN #FXConversionRates FXDayRatesForEndDate ON (  
  FXDayRatesForEndDate.FromCurrencyID = DailyCash.LocalCurrencyID  
		AND FXDayRatesForEndDate.ToCurrencyID = CF.LocalCurrency
  AND DateDiff(d, dbo.AdjustBusinessDays(DATEADD(D, 1, @EndDate), -1, @DefaultAUECID), FXDayRatesForEndDate.DATE) = 0  
		AND FXDayRatesForEndDate.FundID = DailyCash.FundID
   )  
LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateEndDate ON (
		ZeroFundFxRateEndDate.FromCurrencyID = DailyCash.LocalCurrencyID
		AND ZeroFundFxRateEndDate.ToCurrencyID = CF.LocalCurrency
		AND DateDiff(d, dbo.AdjustBusinessDays(DATEADD(D, 1, @EndDate), -1, @DefaultAUECID), ZeroFundFxRateEndDate.DATE) = 0
		AND ZeroFundFxRateEndDate.FundID = 0
  ) 

	WHERE DATEDIFF(DAY, DailyCash.Date, @RecentDateForNonZeroCash) = 0
GROUP BY CMF.MasterFundName,DailyCash.FundID,DailyCash.LocalCurrencyID,CF.LocalCurrency


-------------------------------------------------------------------------------------------------------------              
UPDATE #MTMDataTable
SET	ClosedTradeAttribute1 =
		CASE
			WHEN (Open_CloseTag = 'o') THEN OpenTradeAttribute1
			ELSE ClosedTradeAttribute1
		END,
	ClosedTradeAttribute2 =
		CASE
			WHEN (Open_CloseTag = 'o') THEN OpenTradeAttribute2
			ELSE ClosedTradeAttribute2
		END,
	ClosedTradeAttribute3 =
		CASE
			WHEN (Open_CloseTag = 'o') THEN OpenTradeAttribute3
			ELSE ClosedTradeAttribute3
		END,
	ClosedTradeAttribute4 =
		CASE
			WHEN (Open_CloseTag = 'o') THEN OpenTradeAttribute4
			ELSE ClosedTradeAttribute4
		END,
	ClosedTradeAttribute5 =
		CASE
			WHEN (Open_CloseTag = 'o') THEN OpenTradeAttribute5
			ELSE ClosedTradeAttribute5
		END,
	ClosedTradeAttribute6 =
		CASE
			WHEN (Open_CloseTag = 'o') THEN OpenTradeAttribute6
			ELSE ClosedTradeAttribute6
		END

---------------------------------------------------------------------------------------------------------------------        

UPDATE #MTMDataTable
SET	ClosedTradeAttribute1 =
		CASE
			WHEN ((ClosedTradeAttribute1 = '' OR
			ClosedTradeAttribute1 = NULL) AND
			(Open_CloseTag = 'C')) THEN OpenTradeAttribute1
			ELSE ClosedTradeAttribute1
		END,
	ClosedTradeAttribute2 =
		CASE
			WHEN ((ClosedTradeAttribute2 = '' OR
			ClosedTradeAttribute2 = NULL) AND
			(Open_CloseTag = 'C')) THEN OpenTradeAttribute2
			ELSE ClosedTradeAttribute2
		END,
	ClosedTradeAttribute3 =
		CASE
			WHEN ((ClosedTradeAttribute3 = '' OR
			ClosedTradeAttribute3 = NULL) AND
			(Open_CloseTag = 'C')) THEN OpenTradeAttribute3
			ELSE ClosedTradeAttribute3
		END,
	ClosedTradeAttribute4 =
		CASE
			WHEN ((ClosedTradeAttribute4 = '' OR
			ClosedTradeAttribute4 = NULL) AND
			(Open_CloseTag = 'C')) THEN OpenTradeAttribute4
			ELSE ClosedTradeAttribute4
		END,
	ClosedTradeAttribute5 =
		CASE
			WHEN ((ClosedTradeAttribute5 = '' OR
			ClosedTradeAttribute5 = NULL) AND
			(Open_CloseTag = 'C')) THEN OpenTradeAttribute5
			ELSE ClosedTradeAttribute5
		END,
	ClosedTradeAttribute6 =
		CASE
			WHEN ((ClosedTradeAttribute6 = '' OR
			ClosedTradeAttribute6 = NULL) AND
			(Open_CloseTag = 'C')) THEN OpenTradeAttribute6
			ELSE ClosedTradeAttribute6
		END,
	OpenTradeAttribute1 =
		CASE
			WHEN ((OpenTradeAttribute1 = '' OR
			OpenTradeAttribute1 = NULL) AND
			(Open_CloseTag = 'C')) THEN ClosedTradeAttribute1
			ELSE OpenTradeAttribute1
		END,
	OpenTradeAttribute2 =
		CASE
			WHEN ((OpenTradeAttribute2 = '' OR
			OpenTradeAttribute2 = NULL) AND
			(Open_CloseTag = 'C')) THEN ClosedTradeAttribute2
			ELSE OpenTradeAttribute2
		END,
	OpenTradeAttribute3 =
		CASE
			WHEN ((OpenTradeAttribute3 = '' OR
			OpenTradeAttribute3 = NULL) AND
			(Open_CloseTag = 'C')) THEN ClosedTradeAttribute3
			ELSE OpenTradeAttribute3
		END,
	OpenTradeAttribute4 =
		CASE
			WHEN ((OpenTradeAttribute4 = '' OR
			OpenTradeAttribute4 = NULL) AND
			(Open_CloseTag = 'C')) THEN ClosedTradeAttribute4
			ELSE OpenTradeAttribute4
		END,
	OpenTradeAttribute5 =
		CASE
			WHEN ((OpenTradeAttribute5 = '' OR
			OpenTradeAttribute5 = NULL) AND
			(Open_CloseTag = 'C')) THEN ClosedTradeAttribute5
			ELSE OpenTradeAttribute5
		END,
	OpenTradeAttribute6 =
		CASE
			WHEN ((OpenTradeAttribute6 = '' OR
			OpenTradeAttribute6 = NULL) AND
			(Open_CloseTag = 'C')) THEN ClosedTradeAttribute6
			ELSE OpenTradeAttribute6
		END

---------------------------------------------------------------------------------------------------------------------        

UPDATE #MTMDataTable
SET	ClosedTradeAttribute1 =
		CASE
			WHEN (ClosedTradeAttribute1 = '' OR
			ClosedTradeAttribute1 = NULL) THEN 'undefined'
			ELSE ClosedTradeAttribute1
		END,
	ClosedTradeAttribute2 =
		CASE
			WHEN (ClosedTradeAttribute2 = '' OR
			ClosedTradeAttribute2 = NULL) THEN 'undefined'
			ELSE ClosedTradeAttribute2
		END,
	ClosedTradeAttribute3 =
		CASE
			WHEN (ClosedTradeAttribute3 = '' OR
			ClosedTradeAttribute3 = NULL) THEN 'undefined'
			ELSE ClosedTradeAttribute3
		END,
	ClosedTradeAttribute4 =
		CASE
			WHEN (ClosedTradeAttribute4 = '' OR
			ClosedTradeAttribute4 = NULL) THEN 'undefined'
			ELSE ClosedTradeAttribute4
		END,
	ClosedTradeAttribute5 =
		CASE
			WHEN (ClosedTradeAttribute5 = '' OR
			ClosedTradeAttribute5 = NULL) THEN 'undefined'
			ELSE ClosedTradeAttribute5
		END,
	ClosedTradeAttribute6 =
		CASE
			WHEN (ClosedTradeAttribute6 = '' OR
			ClosedTradeAttribute6 = NULL) THEN 'undefined'
			ELSE ClosedTradeAttribute6
		END,
	OpenTradeAttribute1 =
		CASE
			WHEN (OpenTradeAttribute1 = '' OR
			OpenTradeAttribute1 = NULL) THEN 'undefined'
			ELSE OpenTradeAttribute1
		END,
	OpenTradeAttribute2 =
		CASE
			WHEN (OpenTradeAttribute2 = '' OR
			OpenTradeAttribute2 = NULL) THEN 'undefined'
			ELSE OpenTradeAttribute2
		END,
	OpenTradeAttribute3 =
		CASE
			WHEN (OpenTradeAttribute3 = '' OR
			OpenTradeAttribute3 = NULL) THEN 'undefined'
			ELSE OpenTradeAttribute3
		END,
	OpenTradeAttribute4 =
		CASE
			WHEN (OpenTradeAttribute4 = '' OR
			OpenTradeAttribute4 = NULL) THEN 'undefined'
			ELSE OpenTradeAttribute4
		END,
	OpenTradeAttribute5 =
		CASE
			WHEN (OpenTradeAttribute5 = '' OR
			OpenTradeAttribute5 = NULL) THEN 'undefined'
			ELSE OpenTradeAttribute5
		END,
	OpenTradeAttribute6 =
		CASE
			WHEN (OpenTradeAttribute6 = '' OR
			OpenTradeAttribute6 = NULL) THEN 'undefined'
			ELSE OpenTradeAttribute6
		END
---------------------------------------------------------------------------------------------------------------------        

/*  
Sandeep Singh: 21 April 2015  
Desc: Now no customization for fized income asset class. A proper multiplier will be entered in SM database  
http://jira.nirvanasolutions.com:8080/browse/PRANA-6291 (CLONE -Bond multiplier is 1 when it should be .01 [Reports])  
*/

----Update #MTMDataTable                                                            
----Set                                                             
----BeginningMarketValue_Local = BeginningMarketValue_Local /100  ,                                      
----BeginningMarketValue_Base = BeginningMarketValue_Base /100,                                                           
----EndingMarketValue_Local = EndingMarketValue_Local /100,                                                          
----EndingMarketValue_Base = EndingMarketValue_Base /100                                                                         
----Where Asset = 'FixedIncome'               

---------------------------------------------------------------------------------------------------------------------        

UPDATE #MTMDataTable
SET	UnrealizedPNLMTM_Local =
		CASE
			--Local Unrealized PnL of Open Trade with Commission                                                  
			WHEN
			(
			(Open_CloseTag = 'O' AND
			Asset = 'Future' AND
			@IncludeCommissionInPNL_Futures = 1) OR
			(Open_CloseTag = 'O' AND
			Asset = 'FutureOption' AND
			@IncludeCommissionInPNL_FutOptions = 1) OR
			(Open_CloseTag = 'O' AND
			Asset = 'FX' AND
			@IncludeCommissionInPNL_FX = 1) OR
			(Open_CloseTag = 'O' AND
			Asset = 'Equity' AND
			IsSwapped = 1 AND
			@IncludeCommissionInPNL_Swaps = 1) OR
			(Open_CloseTag = 'O' AND
			Asset = 'Equity' AND
			@IncludeCommissionInPNL_Equity = 1) OR
			(Open_CloseTag = 'O' AND
			Asset = 'EquityOption' AND
			@IncludeCommissionInPNL_EquityOption = 1) OR
			(Open_CloseTag = 'O' AND
			Asset NOT IN ('Future', 'FutureOption', 'FX', 'Equity', 'EquityOption') AND
			@IncludeCommissionInPNL_Other = 1)
			) THEN CASE
				WHEN DATEDIFF(D, @StartDate, TradeDate) >= 0 THEN (ISNULL(EndingMarketValue_Local, 0) - ISNULL(BeginningMarketValue_Local, 0)) - TotalOpenCommission_Local
				ELSE ISNULL(EndingMarketValue_Local, 0) - ISNULL(BeginningMarketValue_Local, 0)
			END
			--Local Unrealized PnL of Open Trade without Commission                                                   
			WHEN
			(
			(Open_CloseTag = 'O' AND
			Asset = 'Future' AND
			@IncludeCommissionInPNL_Futures = 0) OR
			(Open_CloseTag = 'O' AND
			Asset = 'FutureOption' AND
			@IncludeCommissionInPNL_FutOptions = 0) OR
			(Open_CloseTag = 'O' AND
			Asset = 'FX' AND
			@IncludeCommissionInPNL_FX = 0) OR
			(Open_CloseTag = 'O' AND
			Asset = 'Equity' AND
			IsSwapped = 1 AND
			@IncludeCommissionInPNL_Swaps = 0) OR
			(Open_CloseTag = 'O' AND
			Asset = 'Equity' AND
			@IncludeCommissionInPNL_Equity = 0) OR
			(Open_CloseTag = 'O' AND
			Asset = 'EquityOption' AND
			@IncludeCommissionInPNL_EquityOption = 0) OR
			(Open_CloseTag = 'O' AND
			Asset NOT IN ('Future', 'FutureOption', 'FX', 'Equity', 'EquityOption') AND
			@IncludeCommissionInPNL_Other = 0)
			) THEN ISNULL(EndingMarketValue_Local, 0) - ISNULL(BeginningMarketValue_Local, 0)
			ELSE 0
		END,



	UnrealizedPNLMTM_Base =
		CASE
			--Unrealized PnL of Open Trade with Ending FX rate and Commission         
			WHEN
			(
			(Open_CloseTag = 'O' AND
			Asset = 'Future' AND
			@FuturePNLWithBothOrEndFXRate = 0 AND
			@IncludeCommissionInPNL_Futures = 1) OR
			(Open_CloseTag = 'O' AND
			Asset = 'FutureOption' AND
			@InternationalFutureOptionPNLWithBothOrEndFXRate = 0 AND
			@IncludeCommissionInPNL_FutOptions = 1) OR
			(Open_CloseTag = 'O' AND
			Asset = 'FX' AND
			@IncludeFXPNLinFX = 0 AND
			@IncludeCommissionInPNL_FX = 1) OR
			(Open_CloseTag = 'O' AND
			Asset = 'Equity' AND
			IsSwapped = 1 AND
			@IncludeFXPNLinSwaps = 0 AND
			@IncludeCommissionInPNL_Swaps = 1) OR
			(Open_CloseTag = 'O' AND
			Asset = 'Equity' AND
			@IncludeFXPNLinEquity = 0 AND
			@IncludeCommissionInPNL_Equity = 1) OR
			(Open_CloseTag = 'O' AND
			Asset = 'EquityOption' AND
			@IncludeFXPNLinEquityOption = 0 AND
			@IncludeCommissionInPNL_EquityOption = 1) OR
			(Open_CloseTag = 'O' AND
			Asset NOT IN ('Future', 'FutureOption', 'FX', 'Equity', 'EquityOption') AND
			@IncludeFXPNLinOther = 0 AND
			@IncludeCommissionInPNL_Other = 1)
			) THEN CASE
				WHEN DATEDIFF(D, @StartDate, TradeDate) >= 0 THEN ((ISNULL(EndingMarketValue_Local, 0) - ISNULL(BeginningMarketValue_Local, 0)) - TotalOpenCommission_Local) * ConversionRateEnd
				ELSE (ISNULL(EndingMarketValue_Local, 0) - ISNULL(BeginningMarketValue_Local, 0)) * ConversionRateEnd
			END

			--Unrealized PnL of Open Trade with Ending FX rate and no Commission        
			WHEN
			(
			(Open_CloseTag = 'O' AND
			Asset = 'Future' AND
			@FuturePNLWithBothOrEndFXRate = 0 AND
			@IncludeCommissionInPNL_Futures = 0) OR
			(Open_CloseTag = 'O' AND
			Asset = 'FutureOption' AND
			@InternationalFutureOptionPNLWithBothOrEndFXRate = 0 AND
			@IncludeCommissionInPNL_FutOptions = 0) OR
			(Open_CloseTag = 'O' AND
			Asset = 'FX' AND
			@IncludeFXPNLinFX = 0 AND
			@IncludeCommissionInPNL_FX = 0) OR
			(Open_CloseTag = 'O' AND
			Asset = 'Equity' AND
			IsSwapped = 1 AND
			@IncludeFXPNLinSwaps = 0 AND
			@IncludeCommissionInPNL_Swaps = 0) OR
			(Open_CloseTag = 'O' AND
			Asset = 'Equity' AND
			@IncludeFXPNLinEquity = 0 AND
			@IncludeCommissionInPNL_Equity = 0) OR
			(Open_CloseTag = 'O' AND
			Asset = 'EquityOption' AND
			@IncludeFXPNLinEquityOption = 0 AND
			@IncludeCommissionInPNL_EquityOption = 0) OR
			(Open_CloseTag = 'O' AND
			Asset NOT IN ('Future', 'FutureOption', 'FX', 'Equity', 'EquityOption') AND
			@IncludeFXPNLinOther = 0 AND
			@IncludeCommissionInPNL_Other = 0)
			) THEN (ISNULL(EndingMarketValue_Local, 0) - ISNULL(BeginningMarketValue_Local, 0)) * ConversionRateEnd

			--Unrealized PnL of Open Trade with both FX rates and Commission        
			WHEN
			(
			(Open_CloseTag = 'O' AND
			Asset = 'Future' AND
			@FuturePNLWithBothOrEndFXRate = 1 AND
			@IncludeCommissionInPNL_Futures = 1) OR
			(Open_CloseTag = 'O' AND
			Asset = 'FutureOption' AND
			@InternationalFutureOptionPNLWithBothOrEndFXRate = 1 AND
			@IncludeCommissionInPNL_FutOptions = 1) OR
			(Open_CloseTag = 'O' AND
			Asset = 'FX' AND
			@IncludeFXPNLinFX = 1 AND
			@IncludeCommissionInPNL_FX = 1) OR
			(Open_CloseTag = 'O' AND
			Asset = 'Equity' AND
			IsSwapped = 1 AND
			@IncludeFXPNLinSwaps = 1 AND
			@IncludeCommissionInPNL_Swaps = 1) OR
			(Open_CloseTag = 'O' AND
			Asset = 'Equity' AND
			@IncludeFXPNLinEquity = 1 AND
			@IncludeCommissionInPNL_Equity = 1) OR
			(Open_CloseTag = 'O' AND
			Asset = 'EquityOption' AND
			@IncludeFXPNLinEquityOption = 1 AND
			@IncludeCommissionInPNL_EquityOption = 1) OR
			(Open_CloseTag = 'O' AND
			Asset NOT IN ('Future', 'FutureOption', 'FX', 'Equity', 'EquityOption') AND
			@IncludeCommissionInPNL_Other = 1 AND
			@IncludeFXPNLinOther = 1)
			) THEN CASE
				WHEN DATEDIFF(D, @StartDate, TradeDate) >= 0 THEN (ISNULL(EndingMarketValue_Base, 0) - ISNULL(BeginningMarketValue_Base, 0)) - TotalOpenCommission_Base
				ELSE ISNULL(EndingMarketValue_Base, 0) - ISNULL(BeginningMarketValue_Base, 0)
			END

			--Unrealized PnL of Open Trade with both FX rates and no Commission        
			WHEN
			(
			(Open_CloseTag = 'O' AND
			Asset = 'Future' AND
			@FuturePNLWithBothOrEndFXRate = 1 AND
			@IncludeCommissionInPNL_Futures = 0) OR
			(Open_CloseTag = 'O' AND
			Asset = 'FutureOption' AND
			@InternationalFutureOptionPNLWithBothOrEndFXRate = 1 AND
			@IncludeCommissionInPNL_FutOptions = 0) OR
			(Open_CloseTag = 'O' AND
			Asset = 'FX' AND
			@IncludeFXPNLinFX = 1 AND
			@IncludeCommissionInPNL_FX = 0) OR
			(Open_CloseTag = 'O' AND
			Asset = 'Equity' AND
			IsSwapped = 1 AND
			@IncludeFXPNLinSwaps = 1 AND
			@IncludeCommissionInPNL_Swaps = 0) OR
			(Open_CloseTag = 'O' AND
			Asset = 'Equity' AND
			@IncludeFXPNLinEquity = 1 AND
			@IncludeCommissionInPNL_Equity = 0) OR
			(Open_CloseTag = 'O' AND
			Asset = 'EquityOption' AND
			@IncludeFXPNLinEquityOption = 1 AND
			@IncludeCommissionInPNL_EquityOption = 0) OR
			(Open_CloseTag = 'O' AND
			Asset NOT IN ('Future', 'FutureOption', 'FX', 'Equity', 'EquityOption') AND
			@IncludeFXPNLinOther = 1 AND
			@IncludeCommissionInPNL_Other = 0)
			) THEN ISNULL(EndingMarketValue_Base, 0) - ISNULL(BeginningMarketValue_Base, 0)


			WHEN Open_CloseTag = 'CASH' THEN CashFXUnrealizedPNL
			ELSE 0
		END,




	RealizedPNLMTM_Local =
		CASE
			--Local Realized PnL of closed Trade with Commission                                                  
			WHEN
			(
			(Open_CloseTag = 'C' AND
			Asset = 'Future' AND
			@IncludeCommissionInPNL_Futures = 1) OR
			(Open_CloseTag = 'C' AND
			Asset = 'FutureOption' AND
			@IncludeCommissionInPNL_FutOptions = 1) OR
			(Open_CloseTag = 'C' AND
			Asset = 'FX' AND
			@IncludeCommissionInPNL_FX = 1) OR
			(Open_CloseTag = 'C' AND
			Asset = 'Equity' AND
			IsSwapped = 1 AND
			@IncludeCommissionInPNL_Swaps = 1) OR
			(Open_CloseTag = 'C' AND
			Asset = 'Equity' AND
			@IncludeCommissionInPNL_Equity = 1) OR
			(Open_CloseTag = 'C' AND
			Asset = 'EquityOption' AND
			@IncludeCommissionInPNL_EquityOption = 1) OR
			(Open_CloseTag = 'C' AND
			Asset NOT IN ('Future', 'FutureOption', 'FX', 'Equity', 'EquityOption') AND
			@IncludeCommissionInPNL_Other = 1)
			) THEN CASE
				WHEN DATEDIFF(D, @StartDate, TradeDate) >= 0 THEN (ISNULL(EndingMarketValue_Local, 0) - ISNULL(BeginningMarketValue_Local, 0)) - TotalOpenCommission_Local - TotalClosedCommission_Local
				ELSE (ISNULL(EndingMarketValue_Local, 0) - ISNULL(BeginningMarketValue_Local, 0)) - TotalClosedCommission_Local
			END
			--Local Realized PnL of closed Trade without Commission            
			WHEN
			(
			(Open_CloseTag = 'C' AND
			Asset = 'Future' AND
			@IncludeCommissionInPNL_Futures = 0) OR
			(Open_CloseTag = 'C' AND
			Asset = 'FutureOption' AND
			@IncludeCommissionInPNL_FutOptions = 0) OR
			(Open_CloseTag = 'C' AND
			Asset = 'FX' AND
			@IncludeCommissionInPNL_FX = 0) OR
			(Open_CloseTag = 'C' AND
			Asset = 'Equity' AND
			IsSwapped = 1 AND
			@IncludeCommissionInPNL_Swaps = 0) OR
			(Open_CloseTag = 'C' AND
			Asset = 'Equity' AND
			@IncludeCommissionInPNL_Equity = 0) OR
			(Open_CloseTag = 'C' AND
			Asset = 'EquityOption' AND
			@IncludeCommissionInPNL_EquityOption = 0) OR
			(Open_CloseTag = 'C' AND
			Asset NOT IN ('Future', 'FutureOption', 'FX', 'Equity', 'EquityOption') AND
			@IncludeCommissionInPNL_Other = 0)
			) THEN (ISNULL(EndingMarketValue_Local, 0) - ISNULL(BeginningMarketValue_Local, 0))

			ELSE 0
		END,





	RealizedPNLMTM_Base =
		CASE
			--Realized PnL of closed Trade with Ending FX rate and Commission         
			WHEN
			(
			(Open_CloseTag = 'C' AND
			Asset = 'Future' AND
			@FuturePNLWithBothOrEndFXRate = 0 AND
			@IncludeCommissionInPNL_Futures = 1) OR
			(Open_CloseTag = 'C' AND
			Asset = 'FutureOption' AND
			@InternationalFutureOptionPNLWithBothOrEndFXRate = 0 AND
			@IncludeCommissionInPNL_FutOptions = 1) OR
			(Open_CloseTag = 'C' AND
			Asset = 'FX' AND
			@IncludeFXPNLinFX = 0 AND
			@IncludeCommissionInPNL_FX = 1) OR
			(Open_CloseTag = 'C' AND
			Asset = 'Equity' AND
			IsSwapped = 1 AND
			@IncludeFXPNLinSwaps = 0 AND
			@IncludeCommissionInPNL_Swaps = 1) OR
			(Open_CloseTag = 'C' AND
			Asset = 'Equity' AND
			@IncludeFXPNLinEquity = 0 AND
			@IncludeCommissionInPNL_Equity = 1) OR
			(Open_CloseTag = 'C' AND
			Asset = 'EquityOption' AND
			@IncludeFXPNLinEquityOption = 0 AND
			@IncludeCommissionInPNL_EquityOption = 1) OR
			(Open_CloseTag = 'C' AND
			Asset NOT IN ('Future', 'FutureOption', 'FX', 'Equity', 'EquityOption') AND
			@IncludeFXPNLinOther = 0 AND
			@IncludeCommissionInPNL_Other = 1)
			) THEN CASE
				WHEN DATEDIFF(D, @StartDate, TradeDate) >= 0 THEN ((ISNULL(EndingMarketValue_Local, 0) - ISNULL(BeginningMarketValue_Local, 0)) - TotalOpenCommission_Local - TotalClosedCommission_Local) * ConversionRateEnd
				ELSE ((ISNULL(EndingMarketValue_Local, 0) - ISNULL(BeginningMarketValue_Local, 0)) - TotalClosedCommission_Local) * ConversionRateEnd
			END

			--Realized PnL of closed Trade with Ending FX rate and no Commission        
			WHEN
			(
			(Open_CloseTag = 'C' AND
			Asset = 'Future' AND
			@FuturePNLWithBothOrEndFXRate = 0 AND
			@IncludeCommissionInPNL_Futures = 0) OR
			(Open_CloseTag = 'C' AND
			Asset = 'FutureOption' AND
			@InternationalFutureOptionPNLWithBothOrEndFXRate = 0 AND
			@IncludeCommissionInPNL_FutOptions = 0) OR
			(Open_CloseTag = 'C' AND
			Asset = 'FX' AND
			@IncludeFXPNLinFX = 0 AND
			@IncludeCommissionInPNL_FX = 0) OR
			(Open_CloseTag = 'C' AND
			Asset = 'Equity' AND
			IsSwapped = 1 AND
			@IncludeFXPNLinSwaps = 0 AND
			@IncludeCommissionInPNL_Swaps = 0) OR
			(Open_CloseTag = 'C' AND
			Asset = 'Equity' AND
			@IncludeFXPNLinEquity = 0 AND
			@IncludeCommissionInPNL_Equity = 0) OR
			(Open_CloseTag = 'C' AND
			Asset = 'EquityOption' AND
			@IncludeFXPNLinEquityOption = 0 AND
			@IncludeCommissionInPNL_EquityOption = 0) OR
			(Open_CloseTag = 'C' AND
			Asset NOT IN ('Future', 'FutureOption', 'FX', 'Equity', 'EquityOption') AND
			@IncludeFXPNLinOther = 0 AND
			@IncludeCommissionInPNL_Other = 0)
			) THEN (ISNULL(EndingMarketValue_Local, 0) - ISNULL(BeginningMarketValue_Local, 0)) * ConversionRateEnd

			--Realized PnL of closed Trade with both FX rates and Commission        
			WHEN
			(
			(Open_CloseTag = 'C' AND
			Asset = 'Future' AND
			@FuturePNLWithBothOrEndFXRate = 1 AND
			@IncludeCommissionInPNL_Futures = 1) OR
			(Open_CloseTag = 'C' AND
			Asset = 'FutureOption' AND
			@InternationalFutureOptionPNLWithBothOrEndFXRate = 1 AND
			@IncludeCommissionInPNL_FutOptions = 1) OR
			(Open_CloseTag = 'C' AND
			Asset = 'FX' AND
			@IncludeFXPNLinFX = 1 AND
			@IncludeCommissionInPNL_FX = 1) OR
			(Open_CloseTag = 'C' AND
			Asset = 'Equity' AND
			IsSwapped = 1 AND
			@IncludeFXPNLinSwaps = 1 AND
			@IncludeCommissionInPNL_Swaps = 1) OR
			(Open_CloseTag = 'C' AND
			Asset = 'Equity' AND
			@IncludeFXPNLinEquity = 1 AND
			@IncludeCommissionInPNL_Equity = 1) OR
			(Open_CloseTag = 'C' AND
			Asset = 'EquityOption' AND
			@IncludeFXPNLinEquityOption = 1 AND
			@IncludeCommissionInPNL_EquityOption = 1) OR
			(Open_CloseTag = 'C' AND
			Asset NOT IN ('Future', 'FutureOption', 'FX', 'Equity', 'EquityOption') AND
			@IncludeCommissionInPNL_Other = 1 AND
			@IncludeFXPNLinOther = 1)
			) THEN CASE
				WHEN DATEDIFF(D, @StartDate, TradeDate) >= 0 THEN (ISNULL(EndingMarketValue_Base, 0) - ISNULL(BeginningMarketValue_Base, 0)) - TotalOpenCommission_Base - TotalClosedCommission_Base
				ELSE (ISNULL(EndingMarketValue_Base, 0) - ISNULL(BeginningMarketValue_Base, 0)) - TotalClosedCommission_Base
			END

			--Realized PnL of closed Trade with both FX rates and no Commission        
			WHEN
			(
			(Open_CloseTag = 'C' AND
			Asset = 'Future' AND
			@FuturePNLWithBothOrEndFXRate = 1 AND
			@IncludeCommissionInPNL_Futures = 0) OR
			(Open_CloseTag = 'C' AND
			Asset = 'FutureOption' AND
			@InternationalFutureOptionPNLWithBothOrEndFXRate = 1 AND
			@IncludeCommissionInPNL_FutOptions = 0) OR
			(Open_CloseTag = 'C' AND
			Asset = 'FX' AND
			@IncludeFXPNLinFX = 1 AND
			@IncludeCommissionInPNL_FX = 0) OR
			(Open_CloseTag = 'C' AND
			Asset = 'Equity' AND
			IsSwapped = 1 AND
			@IncludeFXPNLinSwaps = 1 AND
			@IncludeCommissionInPNL_Swaps = 0) OR
			(Open_CloseTag = 'C' AND
			Asset = 'Equity' AND
			@IncludeFXPNLinEquity = 1 AND
			@IncludeCommissionInPNL_Equity = 0) OR
			(Open_CloseTag = 'C' AND
			Asset = 'EquityOption' AND
			@IncludeFXPNLinEquityOption = 1 AND
			@IncludeCommissionInPNL_EquityOption = 0) OR
			(Open_CloseTag = 'C' AND
			Asset NOT IN ('Future', 'FutureOption', 'FX', 'Equity', 'EquityOption') AND
			@IncludeFXPNLinOther = 1 AND
			@IncludeCommissionInPNL_Other = 0)
			) THEN (ISNULL(EndingMarketValue_Base, 0) - ISNULL(BeginningMarketValue_Base, 0))


			ELSE 0
		END

-------------------------------------------------------------------------------------------------------------------------------------------------------          


UPDATE #MTMDataTable
SET EndingMarketValue_Base =
	CASE

		--Market Value with Commission when set to Unrealized P&L                       
		WHEN Open_CloseTag = 'O' AND
		(
		(Asset IN ('FX', 'FXForward') AND
		@ShowFXMktValueAsUnrealizedOrZero = 2 AND
		@IncludeCommissionInPNL_FX = 1) OR
		(Asset = 'Equity' AND
		IsSwapped = 1 AND
		@SwapMV_ZeroOrEndingMVOrUnrealized = 2 AND
		@IncludeCommissionInPNL_Swaps = 1) OR
		(Asset = 'Future' AND
		@ShowFutureMktValueAsUnrealizedOrZero = 2 AND
		@IncludeCommissionInPNL_Futures = 1) OR
		(Asset = 'FutureOption' AND
		BaseCurrencyID <> CurrencyID AND
		@ShowInternationalFutureOptionMktValueAsMVorUnrealizedOrZero = 2 AND
		@IncludeCommissionInPNL_FutOptions = 1)
		) THEN ISNULL(EndingMarketValue_Base, 0) - ISNULL(NetTotalCost_Base, 0)

		--Market Value without Commission when set to Unrealized P&L           
		WHEN Open_CloseTag = 'O' AND
		(
		(Asset IN ('FX', 'FXForward') AND
		@ShowFXMktValueAsUnrealizedOrZero = 2 AND
		@IncludeCommissionInPNL_FX = 0) OR
		(Asset = 'Equity' AND
		IsSwapped = 1 AND
		@SwapMV_ZeroOrEndingMVOrUnrealized = 2 AND
		@IncludeCommissionInPNL_Swaps = 0) OR
		(Asset = 'Future' AND
		@ShowFutureMktValueAsUnrealizedOrZero = 2 AND
		@IncludeCommissionInPNL_Futures = 0) OR
		(Asset = 'FutureOption' AND
		BaseCurrencyID <> CurrencyID AND
		@ShowInternationalFutureOptionMktValueAsMVorUnrealizedOrZero = 2 AND
		@IncludeCommissionInPNL_FutOptions = 0)
		) THEN
		-- CASE                  
		--  WHEN DateDiff(d,@StartDate,TradeDate) >= 0                       
		--  THEN (Isnull(EndingMarketValue_Base,0) - Isnull(NetTotalCost_Base,0)) + TotalOpenCommission_Base                                            
		--  ELSE Isnull(EndingMarketValue_Base,0) - Isnull(NetTotalCost_Base,0)                      
		--END             
		(ISNULL(EndingMarketValue_Base, 0) - ISNULL(NetTotalCost_Base, 0)) + TotalOpenCommission_Base


		WHEN Open_CloseTag = 'C' THEN 0
		ELSE EndingMarketValue_Base
	END
  
  
IF @ShowFutureMktValueAsUnrealizedOrZero = 3 BEGIN
UPDATE #MTMDataTable
SET EndingMarketValue_Base = 0
WHERE Asset = 'Future'
AND Open_CloseTag = 'O'
END

-----------------------------------------------------------------------------------------------------------                                                                                        
SELECT
	@StartDate =
		CASE
			WHEN @NAVOfCurrentOrPriorDay = 1 THEN @StartDate
			ELSE dbo.F_getAdjustedDateForGivenDate(@StartDate, 1)
		END
  
  
  
ALTER TABLE #MTMDataTable  
ADD FundNav float,  
TotalPNL float,  
PNLPercentByNAV float,  
UnderlyingSymbolCompanyName nvarchar(200),  
BaseCurrency varchar(10)

UPDATE #MTMDataTable
SET BaseCurrency = C.CurrencySymbol
FROM T_Currency C
INNER JOIN #MTMDataTable MTM
	ON MTM.BaseCurrencyID = C.CurrencyID


UPDATE #MTMDataTable
SET UnderlyingSymbolCompanyName = SM.CompanyName
FROM #MTMDataTable MTM
INNER JOIN V_SecMasterData_WithUnderlying SM
	ON SM.TickerSymbol = MTM.UnderlyingSymbol


UPDATE #MTMDataTable
SET	FundNav = ISNULL(PM_NAV.NAVValue, 0),
	TotalPNL = (#MTMDataTable.RealizedPNLMTM_Base + #MTMDataTable.UnRealizedPNLMTM_Base + #MTMDataTable.Dividend),
	PNLPercentByNAV =
		CASE
			WHEN (ISNULL(PM_NAV.NAVValue, 0) <> 0) THEN (((#MTMDataTable.RealizedPNLMTM_Base + #MTMDataTable.UnRealizedPNLMTM_Base + #MTMDataTable.Dividend) * 100) / PM_NAV.NAVValue)
			ELSE 0
		END
FROM #MTMDataTable
LEFT OUTER JOIN PM_NAVValue PM_NAV
	ON (#MTMDataTable.FundID = PM_NAV.FundID
	AND DATEDIFF(D, date, @StartDate) = 0)
  
  
  
----Dynamic UDA Added  
----New Column added Commission in the SP
  
ALTER TABLE #MTMDataTable  
ADD RiskCurrency varchar(100) NULL,  
Issuer varchar(100) NULL,  
CountryOfRisk varchar(100) NULL,  
Region varchar(100) NULL,  
Analyst varchar(100) NULL,  
UCITSEligibleTag varchar(100) NULL,  
LiquidTag varchar(100) NULL,  
MarketCap varchar(100) NULL,  
CustomUDA1 varchar(100) NULL,  
CustomUDA2 varchar(100) NULL,  
CustomUDA3 varchar(100) NULL,  
CustomUDA4 varchar(100) NULL,  
CustomUDA5 varchar(100) NULL,  
CustomUDA6 varchar(100) NULL,  
CustomUDA7 varchar(100) NULL,
Commission Float    

UPDATE #MTMDataTable
SET	RiskCurrency =
		CASE
			WHEN (VUDA.RiskCurrency = '' OR
			VUDA.RiskCurrency IS NULL) THEN 'Undefined'
			ELSE RTRIM(LTRIM(VUDA.RiskCurrency))
		END,
	Issuer =
		CASE
			WHEN (VUDA.Issuer = '' OR
			VUDA.Issuer IS NULL) THEN 'Undefined'
			ELSE RTRIM(LTRIM(VUDA.Issuer))
		END,
	CountryOfRisk =
		CASE
			WHEN (VUDA.CountryOfRisk = '' OR
			VUDA.CountryOfRisk IS NULL) THEN 'Undefined'
			ELSE RTRIM(LTRIM(VUDA.CountryOfRisk))
		END,
	Region =
		CASE
			WHEN (VUDA.Region = '' OR
			VUDA.Region IS NULL) THEN 'Undefined'
			ELSE RTRIM(LTRIM(VUDA.Region))
		END,
	Analyst =
		CASE
			WHEN (VUDA.Analyst = '' OR
			VUDA.Analyst IS NULL) THEN 'Undefined'
			ELSE RTRIM(LTRIM(VUDA.Analyst))
		END,
	UCITSEligibleTag =
		CASE
			WHEN (VUDA.UCITSEligibleTag = '' OR
			VUDA.UCITSEligibleTag IS NULL) THEN 'Undefined'
			ELSE RTRIM(LTRIM(VUDA.UCITSEligibleTag))
		END,
	LiquidTag =
		CASE
			WHEN (VUDA.LiquidTag = '' OR
			VUDA.LiquidTag IS NULL) THEN 'Undefined'
			ELSE RTRIM(LTRIM(VUDA.LiquidTag))
		END,
	MarketCap =
		CASE
			WHEN (VUDA.MarketCap = '' OR
			VUDA.MarketCap IS NULL) THEN 'Undefined'
			ELSE RTRIM(LTRIM(VUDA.MarketCap))
		END,
	CustomUDA1 =
		CASE
			WHEN (VUDA.CustomUDA1 = '' OR
			VUDA.CustomUDA1 IS NULL) THEN 'Undefined'
			ELSE RTRIM(LTRIM(VUDA.CustomUDA1))
		END,
	CustomUDA2 =
		CASE
			WHEN (VUDA.CustomUDA2 = '' OR
			VUDA.CustomUDA2 IS NULL) THEN 'Undefined'
			ELSE RTRIM(LTRIM(VUDA.CustomUDA2))
		END,
	CustomUDA3 =
		CASE
			WHEN (VUDA.CustomUDA3 = '' OR
			VUDA.CustomUDA3 IS NULL) THEN 'Undefined'
			ELSE RTRIM(LTRIM(VUDA.CustomUDA3))
		END,
	CustomUDA4 =
		CASE
			WHEN (VUDA.CustomUDA4 = '' OR
			VUDA.CustomUDA4 IS NULL) THEN 'Undefined'
			ELSE RTRIM(LTRIM(VUDA.CustomUDA4))
		END,
	CustomUDA5 =
		CASE
			WHEN (VUDA.CustomUDA5 = '' OR
			VUDA.CustomUDA5 IS NULL) THEN 'Undefined'
			ELSE RTRIM(LTRIM(VUDA.CustomUDA5))
		END,
	CustomUDA6 =
		CASE
			WHEN (VUDA.CustomUDA6 = '' OR
			VUDA.CustomUDA6 IS NULL) THEN 'Undefined'
			ELSE RTRIM(LTRIM(VUDA.CustomUDA6))
		END,
	CustomUDA7 =
		CASE
			WHEN (VUDA.CustomUDA7 = '' OR
			VUDA.CustomUDA7 IS NULL) THEN 'Undefined'
			ELSE RTRIM(LTRIM(VUDA.CustomUDA7))
		END
FROM #MTMDataTable
LEFT OUTER JOIN #SecMasterDataTempTable SM
	ON SM.TickerSymbol = #MTMDataTable.Symbol
LEFT OUTER JOIN V_UDA_DynamicUDA VUDA
	ON VUDA.Symbol_PK = SM.Symbol_PK
  
 UPDATE #MTMDataTable  
Set Commission =   
Case  
 WHEN  Open_CloseTag = 'C' AND DATEDIFF(Day,@StartDate,Tradedate)>=0  And DATEDIFF(Day,Tradedate,@EndDate)>=0    
 Then TotalOpenCommission_Base + TotalClosedCommission_Base  
 WHEN DATEDIFF(Day,Tradedate,@StartDate) >= 0 AND Open_CloseTag = 'C'   
 Then TotalClosedCommission_Base                  
 WHEN DATEDIFF(Day,@StartDate,Tradedate)>=0    
 Then TotalOpenCommission_Base  
 Else 0                  
END    
  
IF (@SearchString <> '') BEGIN
  
IF (@searchby = 'Symbol') BEGIN
SELECT
	*
FROM #MTMDataTable
INNER JOIN #Symbol
	ON #Symbol.items = #MTMDataTable.Symbol
ORDER BY symbol
END ELSE IF (@searchby = 'underlyingSymbol') BEGIN
SELECT
	*
FROM #MTMDataTable
INNER JOIN #Symbol
	ON #Symbol.items = #MTMDataTable.underlyingSymbol
ORDER BY symbol
END ELSE IF (@searchby = 'BloombergSymbol') BEGIN
SELECT
	*
FROM #MTMDataTable
INNER JOIN #Symbol
	ON #Symbol.items = #MTMDataTable.BloombergSymbol
ORDER BY symbol
END ELSE IF (@searchby = 'SedolSymbol') BEGIN
SELECT
	*
FROM #MTMDataTable
INNER JOIN #Symbol
	ON #Symbol.items = #MTMDataTable.SedolSymbol
ORDER BY symbol
END ELSE IF (@searchby = 'OSISymbol') BEGIN
SELECT
	*
FROM #MTMDataTable
INNER JOIN #Symbol
	ON #Symbol.items = #MTMDataTable.OSISymbol
ORDER BY symbol
END ELSE IF (@searchby = 'IDCOSymbol') BEGIN
SELECT
	*
FROM #MTMDataTable
INNER JOIN #Symbol
	ON #Symbol.items = #MTMDataTable.IDCOSymbol
ORDER BY symbol
END ELSE IF (@searchby = 'ISINSymbol') BEGIN
SELECT
	*
FROM #MTMDataTable
INNER JOIN #Symbol
	ON #Symbol.items = #MTMDataTable.ISINSymbol
ORDER BY symbol
END ELSE IF (@searchby = 'CUSIPSymbol') BEGIN
SELECT
	*
FROM #MTMDataTable
INNER JOIN #Symbol
	ON #Symbol.items = #MTMDataTable.CUSIPSymbol
ORDER BY symbol
END
ELSE IF (@searchby = 'SecurityName')    
BEGIN    
SELECT     
*    
FROM #MTMDataTable    
INNER JOIN #Symbol ON #MTMDataTable.SecurityName LIKE '%' + #Symbol.items + '%'    
ORDER BY symbol    
END   
END ELSE BEGIN
SELECT
	*
FROM #MTMDataTable
ORDER BY symbol
END
-------------------------------------------------------                                   

DROP TABLE #T_Asset, #PM_Taxlots, #T_CompanyFunds, #T_CorpActionData, #TempClosingData
DROP TABLE #MarkPriceForStartDate, #MarkPriceForEndDate, #SecMasterDataTempTable
DROP TABLE #TempSplitFactorForClosed_2, #TempSplitFactorForClosed_1, #TempSplitFactorForOpen, #TempSplitFactorForClosed
DROP TABLE #FXConversionRates, #AUECYesterDates, #AUECBusinessDatesForEndDate, #MTMDataTable, #FundsInMP, #FundsInFXMP, #Symbol, #TempSymbol
DRop table #ZeroFundFxRate,#ZeroFundMarkPriceStartDate,#ZeroFundMarkPriceEndDate  
  
END  
  
