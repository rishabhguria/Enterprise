/***************************************************************                            
Modified Date: 17-August-2015                                                                                          
Modified By : Pankaj Sharma       
Base Sp (P_MW_GetUnrealizedPNL) 
Jira: http://jira.nirvanasolutions.com:8080/browse/PRANA-10311     
Description:       
1) Added three new fields       
 a) Delta(T_MW_genericPNL).      
 b) Average volume(T_MW_genericPNL).      
 c) Shares Outstanding(PM_DailyOutStandings).      
2) Added Date range in the Report.      
      
      
Usage:                          
exec P_MW_GetUnrealizedPNL_MultipleDates      
@Startdate='2015-08-02 00:00:00:000',      
@EndDate='2015-08-05 00:00:00:000',      
@ReportID=N'VSR_MW',      
@Funds=N'1254,1261,1256,1255,1257,1263,1259,1262,1253,1252,1254',      
@Assets=N'6,1,2,5,11',      
@SearchString=N'',@SearchBy=N'Symbol',      
@paramGroupByLevel1=N'Fund',@paramGroupByLevel2=N'Symbol',@paramGroupByLevel3=N'Select',@paramGroupByLevel4=N'Select',      
@paramIncludeAccruals = 'False'      
*******************************************************************/                            
                          
                         
CREATE Procedure [dbo].[P_MW_GetUnrealizedPNL_MultipleDates]                            
(                            
@StartDate datetime,           
----------added a new parameter end date because the report is required to run for date range--------
@EndDate datetime,          
@ReportID Varchar(100),              
@Funds Varchar(max),              
@Assets Varchar(max),                      
@SearchString Varchar(5000) ,                            
@SearchBy Varchar(100),                
@paramGroupByLevel1 Varchar(100),                
@paramGroupByLevel2 Varchar(100),                
@paramGroupByLevel3 Varchar(100),                
@paramGroupByLevel4 Varchar(100),  
@paramIncludeAccruals bit                    
)                            
As                     
              
              
SELECT  
 * INTO #Funds  
from dbo.Split(@Funds, ',')              
              
              
SELECT  
 * INTO #Assets  
from dbo.Split(@Assets, ',')              
              
SELECT  
 * INTO #TempSymbol  
From dbo.split(@SearchString , ',')                       
                        
SELECT DISTINCT  
 * INTO #Symbol  
From #TempSymbol                             
                            
--Select * from #Funds              
--Select * from #Assets              
              
Create table #ParameterPreferences              
([FutureMV_ZeroOrEndingMVOrUnrealized] [int] NOT NULL,  
 [FXMV_ZeroOrEndingMVOrUnrealized] [int] NOT NULL,              
 [SwapMV_ZeroOrEndingMVOrUnrealized] [int] NOT NULL,              
 [InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized] [int] NOT NULL,              
 [TotalCostASZero_Futures] [int] NOT NULL,              
 [TotalCostASZero_FutOptions] [bit] NOT NULL,              
 [TotalCostASZero_FX] [bit] NOT NULL,              
 [TotalCostASZero_Swaps] [bit] NOT NULL,              
 [IncludeFXPNLinEquity] [bit] NOT NULL,              
 [IncludeFXPNLinEquityOption] [bit] NOT NULL,              
 [IncludeFXPNLinFX] [bit] NOT NULL,              
 [IncludeFXPNLinFutures] [bit] NOT NULL,              
 [IncludeFXPNLinSwaps] [bit] NOT NULL,              
 [IncludeFXPNLinInternationalFutOptions] [bit] NOT NULL,              
 [IncludeFXPNLinOther] [bit] NOT NULL,              
 [IncludeCommissionInPNL_Equity] [bit] NOT NULL,              
 [IncludeCommissionInPNL_EquityOption] [bit] NOT NULL,            
 [IncludeCommissionInPNL_Futures] [bit] NOT NULL,              
 [IncludeCommissionInPNL_FutOptions] [bit] NOT NULL,              
 [IncludeCommissionInPNL_Swaps] [bit] NOT NULL,              
 [IncludeCommissionInPNL_FX] [bit] NOT NULL,              
[IncludeCommissionInPNL_Other] [bit] NOT NULL)  
                                          
Insert Into #ParameterPreferences                                            
Select              
 FutureMV_ZeroOrEndingMVOrUnrealized,              
 FXMV_ZeroOrEndingMVOrUnrealized,              
 SwapMV_ZeroOrEndingMVOrUnrealized,              
 InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized,              
 TotalCostASZero_Futures,             
 TotalCostASZero_FutOptions,              
 TotalCostASZero_FX,              
 TotalCostASZero_Swaps,              
 IncludeFXPNLinEquity,              
 IncludeFXPNLinEquityOption,              
IncludeFXPNLinFX,              
 IncludeFXPNLinFutures,              
 IncludeFXPNLinSwaps,              
 IncludeFXPNLinInternationalFutOptions,              
 IncludeFXPNLinOther,              
 IncludeCommissionInPNL_Equity,              
 IncludeCommissionInPNL_EquityOption,              
 IncludeCommissionInPNL_Futures,              
 IncludeCommissionInPNL_FutOptions,              
 IncludeCommissionInPNL_Swaps,              
 IncludeCommissionInPNL_FX,              
 IncludeCommissionInPNL_Other              
 FROM T_ReportPreferences  
 WHERE ReportID = @ReportID  
              
              
Declare @FutureMV_ZeroOrEndingMVOrUnrealized int              
Declare @FXMV_ZeroOrEndingMVOrUnrealized int              
Declare @SwapMV_ZeroOrEndingMVOrUnrealized int              
Declare @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized int              
Declare @TotalCostASZero_Futures int              
Declare @TotalCostASZero_FutOptions bit              
Declare @TotalCostASZero_FX bit               
Declare @TotalCostASZero_Swaps bit              
Declare @IncludeFXPNLinEquity bit              
Declare @IncludeFXPNLinEquityOption bit              
Declare @IncludeFXPNLinFX bit                
Declare @IncludeFXPNLinFutures bit              
Declare @IncludeFXPNLinSwaps bit              
Declare @IncludeFXPNLinInternationalFutOptions bit              
Declare @IncludeFXPNLinOther bit              
Declare @IncludeCommissionInPNL_Equity bit              
Declare @IncludeCommissionInPNL_EquityOption bit              
Declare @IncludeCommissionInPNL_Futures bit              
Declare @IncludeCommissionInPNL_FutOptions bit              
Declare @IncludeCommissionInPNL_Swaps bit              
Declare @IncludeCommissionInPNL_FX bit              
Declare @IncludeCommissionInPNL_Other bit              
                    
SET @FutureMV_ZeroOrEndingMVOrUnrealized = (SELECT  
 FutureMV_ZeroOrEndingMVOrUnrealized  
FROM #ParameterPreferences)  
SET @FXMV_ZeroOrEndingMVOrUnrealized = (SELECT  
 FXMV_ZeroOrEndingMVOrUnrealized  
FROM #ParameterPreferences)  
SET @SwapMV_ZeroOrEndingMVOrUnrealized = (SELECT  
 SwapMV_ZeroOrEndingMVOrUnrealized  
FROM #ParameterPreferences)  
SET @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = (SELECT  
 InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized  
FROM #ParameterPreferences)  
SET @TotalCostASZero_Futures = (SELECT  
 TotalCostASZero_Futures  
FROM #ParameterPreferences)  
SET @TotalCostASZero_FutOptions = (SELECT  
 TotalCostASZero_FutOptions  
FROM #ParameterPreferences)  
SET @TotalCostASZero_FX = (SELECT  
 TotalCostASZero_FX  
FROM #ParameterPreferences)  
SET @TotalCostASZero_Swaps = (SELECT  
 TotalCostASZero_Swaps  
FROM #ParameterPreferences)  
SET @IncludeFXPNLinEquity = (SELECT  
 IncludeFXPNLinEquity  
FROM #ParameterPreferences)  
SET @IncludeFXPNLinEquityOption = (SELECT  
 IncludeFXPNLinEquityOption  
FROM #ParameterPreferences)  
SET @IncludeFXPNLinFX = (SELECT  
 IncludeFXPNLinFX  
FROM #ParameterPreferences)  
SET @IncludeFXPNLinFutures = (SELECT  
 IncludeFXPNLinFutures  
FROM #ParameterPreferences)  
SET @IncludeFXPNLinSwaps = (SELECT  
 IncludeFXPNLinSwaps  
FROM #ParameterPreferences)  
SET @IncludeFXPNLinInternationalFutOptions = (SELECT  
 IncludeFXPNLinInternationalFutOptions  
FROM #ParameterPreferences)  
SET @IncludeFXPNLinOther = (SELECT  
 IncludeFXPNLinOther  
FROM #ParameterPreferences)  
SET @IncludeCommissionInPNL_Equity = (SELECT  
 IncludeCommissionInPNL_Equity  
FROM #ParameterPreferences)  
SET @IncludeCommissionInPNL_EquityOption = (SELECT  
 IncludeCommissionInPNL_EquityOption  
FROM #ParameterPreferences)  
SET @IncludeCommissionInPNL_Futures = (SELECT  
 IncludeCommissionInPNL_Futures  
FROM #ParameterPreferences)  
SET @IncludeCommissionInPNL_FutOptions = (SELECT  
 IncludeCommissionInPNL_FutOptions  
FROM #ParameterPreferences)  
SET @IncludeCommissionInPNL_Swaps = (SELECT  
 IncludeCommissionInPNL_Swaps  
FROM #ParameterPreferences)  
SET @IncludeCommissionInPNL_FX = (SELECT  
 IncludeCommissionInPNL_FX  
FROM #ParameterPreferences)  
SET @IncludeCommissionInPNL_Other = (SELECT  
 IncludeCommissionInPNL_Other  
FROM #ParameterPreferences)  
              
              
--Declare @BaseCurrency Varchar(10)                  
--Set  @BaseCurrency = (select CurrencySymbol from t_company Company                  
--     left outer join T_Currency Currency                  
--     on Company.BaseCurrencyID = Currency.CurrencyID)                  
--Select  @BaseCurrency                       
                         
SELECT                             
                            
Rundate,                          
TradeDate,                           
-- Symbology Codes                          
PNL.Symbol,                          
CUSIPSymbol ,                           
ISINSymbol ,                          
SEDOLSymbol ,                           
BloombergSYmbol ,                                       
ReutersSYmbol ,                           
IDCOSymbol ,                           
OSISymbol,                           
UnderlyingSymbol ,                          
-- Grouping parameters                          
Fund,                           
Asset,                            
TradeCurrency,                           
Side,              
SecurityName,                           
MasterFund,                          
strategy,                          
UDASector,                           
UDACountry,                          
UDASecurityType,                          
UDAAssetClass,                          
UDASubSector ,                          
-- Basic Fields                          
UnitCostLocal,                           
OpeningFXRate,                           
UnitCostBase,                           
EndingFXRate,                           
EndingPriceLocal,                           
EndingPriceBase ,    
---------------added three new columns----------------           
Delta,          
AverageVolume,        
OutStanding.OutStandings as SharesOutstanding,            
--Open Trade Attributes            
OpenTradeAttribute1,            
OpenTradeAttribute2,            
OpenTradeAttribute3,            
OpenTradeAttribute4,            
OpenTradeAttribute5,            
OpenTradeAttribute6  
-- ,RTRIM(LTRIM(ISNULL(RiskCurrency, 'Undefined'))) AS RiskCurrency,  
-- RTRIM(LTRIM(ISNULL(Issuer, 'Undefined'))) AS Issuer,  
-- RTRIM(LTRIM(ISNULL(CountryOfRisk, 'Undefined'))) AS CountryOfRisk,  
-- RTRIM(LTRIM(ISNULL(Region, 'Undefined'))) AS Region,  
-- RTRIM(LTRIM(ISNULL(Analyst, 'Undefined'))) AS Analyst,  
-- RTRIM(LTRIM(ISNULL(CustomUDA1, 'Undefined'))) AS CustomUDA1,----UCITSEligibleTag,          
-- RTRIM(LTRIM(ISNULL(CustomUDA2, 'Undefined'))) AS CustomUDA2,----LiquidTag,          
-- RTRIM(LTRIM(ISNULL(MarketCap, 'Undefined'))) AS MarketCap  
--                        
,CASE                
WHEN (asset = 'CASH')                               
----http://jira.nirvanasolutions.com:8080/browse/PRANA-7912                          
----Then EndingMarketValueBase             
Then EndingMarketValueLocal                
Else BeginningQuantity                
END AS BeginningQuantity,                               
Multiplier,                  
CASE                
  WHEN (asset = 'CASH') THEN 1  
Else SideMultiplier                
END AS SideMultiplier,                                
--SideMultiplier,                           
PutOrCall,                         
IsSwapped,                         
Open_CloseTag ,                           
TotalOpenCommissionAndFees_Local ,                           
TotalOpenCommissionAndFees_Base ,                        
--Derived Fields                         
                        
Case                         
  WHEN (@TotalCostASZero_FX = 1) AND  
  (Asset = 'FX' OR  
  Asset = 'FXForward') THEN 0  
  When (@TotalCostASZero_Futures= 1) and (Asset = 'Future')      
Then 0       
When (@TotalCostASZero_Futures= 2) and (Asset = 'Future')      
Then TotalOpenCommissionAndFees_Local
  WHEN (@TotalCostASZero_FutOptions = 1) AND  
  (Asset = 'FutureOption') THEN 0  
  WHEN (@TotalCostASZero_Swaps = 1) AND  
  (Asset = 'Equity' AND  
  IsSwapped = 1) THEN 0  
  WHEN (asset = 'CASH') THEN 0  
  WHEN (@IncludeCommissionInPNL_Equity = 0) AND  
  (Asset = 'Equity' AND  
  IsSwapped = 0) THEN TotalCost_Local - TotalOpenCommissionAndFees_Local  
  WHEN (@IncludeCommissionInPNL_EquityOption = 0) AND  
  (Asset = 'EquityOption') THEN TotalCost_Local - TotalOpenCommissionAndFees_Local  
  WHEN (@IncludeCommissionInPNL_Futures = 0) AND  
  (Asset = 'Future') THEN TotalCost_Local - TotalOpenCommissionAndFees_Local  
  WHEN (@IncludeCommissionInPNL_FutOptions = 0) AND  
  (Asset = 'FutureOption') THEN TotalCost_Local - TotalOpenCommissionAndFees_Local  
  WHEN (@IncludeCommissionInPNL_Swaps = 0) AND  
  (Asset = 'Equity' AND  
  IsSwapped = 1) THEN TotalCost_Local - TotalOpenCommissionAndFees_Local  
            
Else TotalCost_Local                        
End as TotalCost_Local,                        
                    
Case                         
  WHEN (@TotalCostASZero_FX = 1) AND  
  (Asset = 'FX' OR  
  Asset = 'FXForward') THEN 0  
When (@TotalCostASZero_Futures= 1) and (Asset = 'Future')      
Then 0       
When (@TotalCostASZero_Futures= 2) and (Asset = 'Future')      
Then TotalOpenCommissionAndFees_Base
  WHEN (@TotalCostASZero_FutOptions = 1) AND  
  (Asset = 'FutureOption') THEN 0  
  WHEN (@TotalCostASZero_Swaps = 1) AND  
  (Asset = 'Equity' AND  
  IsSwapped = 1) THEN 0  
  WHEN (asset = 'CASH') THEN 0  
  WHEN (@IncludeCommissionInPNL_Equity = 0) AND  
  (Asset = 'Equity' AND  
  IsSwapped = 0) THEN TotalCost_Base - TotalOpenCommissionAndFees_Base  
  WHEN (@IncludeCommissionInPNL_EquityOption = 0) AND  
  (Asset = 'EquityOption') THEN TotalCost_Base - TotalOpenCommissionAndFees_Base  
  WHEN (@IncludeCommissionInPNL_Futures = 0) AND  
  (Asset = 'Future') THEN TotalCost_Base - TotalOpenCommissionAndFees_Base  
  WHEN (@IncludeCommissionInPNL_FutOptions = 0) AND  
  (Asset = 'FutureOption') THEN TotalCost_Base - TotalOpenCommissionAndFees_Base  
  WHEN (@IncludeCommissionInPNL_Swaps = 0) AND  
  (Asset = 'Equity' AND  
  IsSwapped = 1) THEN TotalCost_Base - TotalOpenCommissionAndFees_Base  
                      
Else TotalCost_Base                        
End as TotalCost_Base,                        
                          
                      
                       
Case               
-- When Market Value is Equal to Market Value                        
When               
(              
  (Asset = 'Future' AND  
  @FutureMV_ZeroOrEndingMVOrUnrealized = 1) OR  
  ((Asset = 'FX' OR  
  Asset = 'FXForward') AND  
  @FXMV_ZeroOrEndingMVOrUnrealized = 1) OR  
  (Asset = 'Equity' AND  
  IsSwapped = 1 AND  
  @SwapMV_ZeroOrEndingMVOrUnrealized = 1) OR  
  (Asset = 'FutureOption' AND  
  (BaseCurrency <> TradeCurrency) AND  
  @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 1)  
  ) THEN EndingMarketValueLocal  
-- When Market Value is Equal to Unrealize P&L and without Commission              
When               
(              
  (Asset = 'Future' AND  
  @FutureMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_Futures = 0) OR  
  ((Asset = 'FX' OR  
  Asset = 'FXForward') AND  
  @FXMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_FX = 0) OR  
  (Asset = 'Equity' AND  
  IsSwapped = 1 AND  
  @SwapMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_Swaps = 0) OR  
  (Asset = 'FutureOption' AND  
  (BaseCurrency <> TradeCurrency) AND  
  @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_FutOptions = 0)  
  ) THEN UnrealizedTotalGainOnCostD2_Local + TotalOpenCommissionAndFees_Local  
-- When Market Value is Equal to Unrealize P&L and with Commission              
When               
(              
  (Asset = 'Future' AND  
  @FutureMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_Futures = 1) OR  
  ((Asset = 'FX' OR  
  Asset = 'FXForward') AND  
  @FXMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_FX = 1) OR  
  (Asset = 'Equity' AND  
  IsSwapped = 1 AND  
  @SwapMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_Swaps = 1) OR  
  (Asset = 'FutureOption' AND  
  (BaseCurrency <> TradeCurrency) AND  
  @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_FutOptions = 1)  
  ) THEN UnrealizedTotalGainOnCostD2_Local  
-- When Market Value is Equal to 0              
When               
(              
  (Asset = 'Future' AND  
  @FutureMV_ZeroOrEndingMVOrUnrealized NOT IN (1, 2)) OR  
  ((Asset = 'FX' OR  
  Asset = 'FXForward') AND  
  @FXMV_ZeroOrEndingMVOrUnrealized NOT IN (1, 2)) OR  
  (Asset = 'Equity' AND  
  IsSwapped = 1 AND  
  @SwapMV_ZeroOrEndingMVOrUnrealized NOT IN (1, 2)) OR  
  (Asset = 'FutureOption' AND  
  (BaseCurrency <> TradeCurrency) AND  
  @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized NOT IN (1, 2))  
  ) THEN 0  
Else EndingMarketValueLocal                        
End as EndingMarketValueLocal,                         
                        
                
                
Case               
-- When Market Value is Equal to Market Value                        
When               
(              
  (Asset = 'Future' AND  
  @FutureMV_ZeroOrEndingMVOrUnrealized = 1) OR  
  ((Asset = 'FX' OR  
  Asset = 'FXForward') AND  
  @FXMV_ZeroOrEndingMVOrUnrealized = 1) OR  
  (Asset = 'Equity' AND  
  IsSwapped = 1 AND  
  @SwapMV_ZeroOrEndingMVOrUnrealized = 1) OR  
  (Asset = 'FutureOption' AND  
  (BaseCurrency <> TradeCurrency) AND  
  @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 1)  
  ) THEN EndingMarketValueBase  
-- When Market Value is Equal to Unrealize P&L and without Commission and with Single FX Rate              
When               
(              
  (Asset = 'Future' AND  
  @FutureMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_Futures = 0 AND  
  @IncludeFXPNLinFutures = 0) OR  
  ((Asset = 'FX' OR  
  Asset = 'FXForward') AND  
  @FXMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_FX = 0 AND  
  @IncludeFXPNLinFX = 0) OR  
  (Asset = 'Equity' AND  
  IsSwapped = 1 AND  
  @SwapMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_Swaps = 0 AND  
  @IncludeFXPNLinSwaps = 0) OR  
  (Asset = 'FutureOption' AND  
  (BaseCurrency <> TradeCurrency) AND  
  @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_FutOptions = 0 AND  
  @IncludeFXPNLinInternationalFutOptions = 0)  
  ) THEN UnrealizedTradingGainOnCostD2_Base + TotalOpenCommissionAndFees_Local  
-- When Market Value is Equal to Unrealize P&L and with Commission and with Single FX Rate              
When               
(              
  (Asset = 'Future' AND  
  @FutureMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_Futures = 1 AND  
  @IncludeFXPNLinFutures = 0) OR  
  ((Asset = 'FX' OR  
  Asset = 'FXForward') AND  
  @FXMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_FX = 1 AND  
  @IncludeFXPNLinFX = 0) OR  
  (Asset = 'Equity' AND  
  IsSwapped = 1 AND  
  @SwapMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_Swaps = 1 AND  
  @IncludeFXPNLinSwaps = 0) OR  
  (Asset = 'FutureOption' AND  
  (BaseCurrency <> TradeCurrency) AND  
  @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_FutOptions = 1 AND  
  @IncludeFXPNLinInternationalFutOptions = 0)  
  ) THEN UnrealizedTradingGainOnCostD2_Base  
-- When Market Value is Equal to Unrealize P&L and without Commission and with Both FX Rate              
When               
(              
  (Asset = 'Future' AND  
  @FutureMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_Futures = 0 AND  
  @IncludeFXPNLinFutures = 1) OR  
  ((Asset = 'FX' OR  
  Asset = 'FXForward') AND  
  @FXMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_FX = 0 AND  
  @IncludeFXPNLinFX = 1) OR  
  (Asset = 'Equity' AND  
  IsSwapped = 1 AND  
  @SwapMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_Swaps = 0 AND  
  @IncludeFXPNLinSwaps = 1) OR  
  (Asset = 'FutureOption' AND  
  (BaseCurrency <> TradeCurrency) AND  
  @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_FutOptions = 0 AND  
  @IncludeFXPNLinInternationalFutOptions = 1)  
  ) Then  
  Case   
   When Asset = 'Equity' AND IsSwapped = 1  
   Then UnrealizedTotalGainOnCostD2_Base + TotalOpenCommissionAndFees_Base  
   Else UnrealizedTotalGainOnCostD2_Base + (TotalOpenCommissionAndFees_Local) * EndingFXRate ---- TotalOpenCommissionAndFees_Base          
  End  
-- When Market Value is Equal to Unrealize P&L and with Commission and with Both FX Rate              
When               
(              
  (Asset = 'Future' AND  
  @FutureMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_Futures = 1 AND  
  @IncludeFXPNLinFutures = 1) OR  
  ((Asset = 'FX' OR  
  Asset = 'FXForward') AND  
  @FXMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_FX = 1 AND  
  @IncludeFXPNLinFX = 1) OR  
  (Asset = 'Equity' AND  
  IsSwapped = 1 AND  
  @SwapMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_Swaps = 1 AND  
  @IncludeFXPNLinSwaps = 1) OR  
  (Asset = 'FutureOption' AND  
  (BaseCurrency <> TradeCurrency) AND  
  @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 AND  
  @IncludeCommissionInPNL_FutOptions = 1 AND  
  @IncludeFXPNLinInternationalFutOptions = 1)  
  ) THEN UnrealizedTotalGainOnCostD2_Base  
-- When Market Value is Equal to 0              
When               
(              
  (Asset = 'Future' AND  
  @FutureMV_ZeroOrEndingMVOrUnrealized NOT IN (1, 2)) OR  
  ((Asset = 'FX' OR  
  Asset = 'FXForward') AND  
  @FXMV_ZeroOrEndingMVOrUnrealized NOT IN (1, 2)) OR  
  (Asset = 'Equity' AND  
  IsSwapped = 1 AND  
  @SwapMV_ZeroOrEndingMVOrUnrealized NOT IN (1, 2)) OR  
  (Asset = 'FutureOption' AND  
  (BaseCurrency <> TradeCurrency) AND  
  @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized NOT IN (1, 2))  
  ) THEN 0  
Else EndingMarketValueBase                        
End as EndingMarketValueBase,               
UnrealizedTradingGainOnCostD2_Base ,                           
UnrealizedFXGainOnCostD2_Base,                          
Case               
-- When Local Unrealize P&L is without Commission            
When               
(              
  (Asset = 'Equity' AND  
  IsSwapped = 0 AND  
  @IncludeCommissionInPNL_Equity = 0) OR  
  (Asset = 'EquityOption' AND  
  @IncludeCommissionInPNL_EquityOption = 0) OR  
  (Asset = 'Future' AND  
  @IncludeCommissionInPNL_Futures = 0) OR  
  ((Asset = 'FX' OR  
  Asset = 'FXForward') AND  
  @IncludeCommissionInPNL_FX = 0) OR  
  (Asset = 'Equity' AND  
  IsSwapped = 1 AND  
  @IncludeCommissionInPNL_Swaps = 0) OR  
  (Asset = 'FutureOption' AND  
  @IncludeCommissionInPNL_FutOptions = 0) OR  
  (Asset NOT IN ('Equity', 'EquityOption', 'Future', 'FutureOption', 'FX', 'FXForward') AND  
  @IncludeCommissionInPNL_Other = 0)  
  ) THEN UnrealizedTotalGainOnCostD2_Local + TotalOpenCommissionAndFees_Local  
-- When Local Unrealize P&L is with Commission              
When               
(              
  (Asset = 'Equity' AND  
  IsSwapped = 0 AND  
  @IncludeCommissionInPNL_Equity = 1) OR  
  (Asset = 'EquityOption' AND  
  @IncludeCommissionInPNL_EquityOption = 1) OR  
  (Asset = 'Future' AND  
  @IncludeCommissionInPNL_Futures = 1) OR  
  ((Asset = 'FX' OR  
  Asset = 'FXForward') AND  
  @IncludeCommissionInPNL_FX = 1) OR  
  (Asset = 'Equity' AND  
  IsSwapped = 1 AND  
  @IncludeCommissionInPNL_Swaps = 1) OR  
  (Asset = 'FutureOption' AND  
  @IncludeCommissionInPNL_FutOptions = 1) OR  
  (Asset NOT IN ('Equity', 'EquityOption', 'Future', 'FutureOption', 'FX', 'FXForward') AND  
  @IncludeCommissionInPNL_Other = 1)  
  ) THEN UnrealizedTotalGainOnCostD2_Local  
Else UnrealizedTotalGainOnCostD2_Local                        
End as UnrealizedTotalGainOnCostD2_Local,                          
                     
Case               
-- When Unrealize P&L is without Commission and with Single FX Rate              
When               
(              
  (Asset = 'Equity' AND  
  IsSwapped = 0 AND  
  @IncludeCommissionInPNL_Equity = 0 AND  
  @IncludeFXPNLinEquity = 0) OR  
  (Asset = 'EquityOption' AND  
  @IncludeCommissionInPNL_EquityOption = 0 AND  
  @IncludeFXPNLinEquityOption = 0) OR  
  (Asset = 'Future' AND  
  @IncludeCommissionInPNL_Futures = 0 AND  
  @IncludeFXPNLinFutures = 0) OR  
  ((Asset = 'FX' OR  
  Asset = 'FXForward') AND  
  @IncludeCommissionInPNL_FX = 0 AND  
  @IncludeFXPNLinFX = 0) OR  
  (Asset = 'Equity' AND  
  IsSwapped = 1 AND  
  @IncludeCommissionInPNL_Swaps = 0 AND  
  @IncludeFXPNLinSwaps = 0) OR  
  (Asset = 'FutureOption' AND  
  @IncludeCommissionInPNL_FutOptions = 0 AND  
  @IncludeFXPNLinInternationalFutOptions = 0) OR  
  (Asset NOT IN ('Equity', 'EquityOption', 'Future', 'FutureOption', 'FX', 'FXForward') AND  
  @IncludeCommissionInPNL_Other = 0 AND  
  @IncludeFXPNLinOther = 0)  
)              
  THEN   
  -- existing code  
  -- UnrealizedTradingGainOnCostD2_Base  + TotalOpenCommissionAndFees_Local       
  -- Why Local commission is used here? while converting TotalOpenCommissionAndFees_Local to Base, we use FX Rate with Trade but   
  -- here we are subtracting TotalOpenCommissionAndFees_Base from UnrealizedTradingGainOnCostD2_Base, so difference due to FX will affect the total number  
  -- so here I takec local comm+fee in consideration and multiplied with Ending FX Rate  
  UnrealizedTradingGainOnCostD2_Base  + (TotalOpenCommissionAndFees_Local * EndingFXRate)   
  
-- When Unrealize P&L is with Commission and with Single FX Rate              
When               
(              
  (Asset = 'Equity' AND  
  IsSwapped = 0 AND  
  @IncludeCommissionInPNL_Equity = 1 AND  
  @IncludeFXPNLinEquity = 0) OR  
  (Asset = 'EquityOption' AND  
  @IncludeCommissionInPNL_EquityOption = 1 AND  
  @IncludeFXPNLinEquityOption = 0) OR  
  (Asset = 'Future' AND  
  @IncludeCommissionInPNL_Futures = 1 AND  
  @IncludeFXPNLinFutures = 0) OR  
  ((Asset = 'FX' OR  
  Asset = 'FXForward') AND  
  @IncludeCommissionInPNL_FX = 1 AND  
  @IncludeFXPNLinFX = 0) OR  
  (Asset = 'Equity' AND  
  IsSwapped = 1 AND  
  @IncludeCommissionInPNL_Swaps = 1 AND  
  @IncludeFXPNLinSwaps = 0) OR  
  (Asset = 'FutureOption' AND  
  @IncludeCommissionInPNL_FutOptions = 1 AND  
  @IncludeFXPNLinInternationalFutOptions = 0) OR  
  (Asset NOT IN ('Equity', 'EquityOption', 'Future', 'FutureOption', 'FX', 'FXForward') AND  
  @IncludeCommissionInPNL_Other = 1 AND  
  @IncludeFXPNLinOther = 0)  
  ) THEN UnrealizedTradingGainOnCostD2_Base  
-- When Unrealize P&L is without Commission and with Both FX Rate              
When               
(              
  (Asset = 'Equity' AND  
  IsSwapped = 0 AND  
  @IncludeCommissionInPNL_Equity = 0 AND  
  @IncludeFXPNLinEquity = 1) OR  
  (Asset = 'EquityOption' AND  
  @IncludeCommissionInPNL_EquityOption = 0 AND  
  @IncludeFXPNLinEquityOption = 1) OR  
  (Asset = 'Future' AND  
  @IncludeCommissionInPNL_Futures = 0 AND  
  @IncludeFXPNLinFutures = 1) OR  
  ((Asset = 'FX' OR  
  Asset = 'FXForward') AND  
  @IncludeCommissionInPNL_FX = 0 AND  
  @IncludeFXPNLinFX = 1) OR  
  (Asset = 'Equity' AND  
  IsSwapped = 1 AND  
  @IncludeCommissionInPNL_Swaps = 0 AND  
  @IncludeFXPNLinSwaps = 1) OR  
  (Asset = 'FutureOption' AND  
  @IncludeCommissionInPNL_FutOptions = 0 AND  
  @IncludeFXPNLinInternationalFutOptions = 1) OR  
  (Asset NOT IN ('Equity', 'EquityOption', 'Future', 'FutureOption', 'FX', 'FXForward') AND  
  @IncludeCommissionInPNL_Other = 0 AND  
  @IncludeFXPNLinOther = 1)  
  ) THEN UnrealizedTotalGainOnCostD2_Base +  (TotalOpenCommissionAndFees_Local) * EndingFXRate   
---- TotalOpenCommissionAndFees_Base  
-- When Unrealize P&L is with Commission and with Both FX Rate              
When               
(              
  (Asset = 'Equity' AND  
  IsSwapped = 0 AND  
  @IncludeCommissionInPNL_Equity = 1 AND  
  @IncludeFXPNLinEquity = 1) OR  
  (Asset = 'EquityOption' AND  
  @IncludeCommissionInPNL_EquityOption = 1 AND  
  @IncludeFXPNLinEquityOption = 1) OR  
  (Asset = 'Future' AND  
  @IncludeCommissionInPNL_Futures = 1 AND  
  @IncludeFXPNLinFutures = 1) OR  
  ((Asset = 'FX' OR  
  Asset = 'FXForward') AND  
  @IncludeCommissionInPNL_FX = 1 AND  
  @IncludeFXPNLinFX = 1) OR  
  (Asset = 'Equity' AND  
  IsSwapped = 1 AND  
  @IncludeCommissionInPNL_Swaps = 1 AND  
  @IncludeFXPNLinSwaps = 1) OR  
  (Asset = 'FutureOption' AND  
  @IncludeCommissionInPNL_FutOptions = 1 AND  
  @IncludeFXPNLinInternationalFutOptions = 1) OR  
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX','FXForward') and @IncludeCommissionInPNL_Other = 1 and @IncludeFXPNLinOther = 1)                
  ) THEN UnrealizedTotalGainOnCostD2_Base  
Else UnrealizedTotalGainOnCostD2_Base                        
End as UnrealizedTotalGainOnCostD2_Base,              
OriginalPurchaseDate,                
 BaseCurrency INTO #UnrealizedPNLTable  
                             
                          
FROM T_MW_GenericPNL  PNL              
        
inner join T_asset A on A.Assetname = PNL.Asset              
inner join T_CompanyFunds F on F.FundName = PNL.Fund              
left JOIN PM_DailyOutStandings OutStanding on OutStanding.Symbol = PNL.Symbol and OutStanding.date=PNL.rundate    


---------------------------------Data coming between date range--------------------        
Where               
(Open_CloseTag ='O') And (DateDiff(Day,@StartDate,Rundate) >= 0 and  DateDiff(Day,Rundate,@EndDate) >= 0)          
And A.AssetID in (Select * from #Assets)              
And F.CompanyFundID in (Select * from #Funds)              
And  
(  
 PNL.Open_CloseTag ='O'  
 Or  
 PNL.Open_CloseTag =      
 Case      
  When @paramIncludeAccruals = 1      
  Then 'Accruals'       
 End   
)     
              
Alter table #UnrealizedPNLTable                  
Add UnderlyingSymbolCompanyName nvarchar(200)               
       
Update  #UnrealizedPNLTable                  
Set UnderlyingSymbolCompanyName = SM.CompanyName                  
From  #UnrealizedPNLTable URPT                  
INNER JOIN V_SecMasterData_WithUnderlying SM  
 ON SM.TickerSymbol = URPT.UnderlyingSymbol  
                  
Alter table #UnrealizedPNLTable                
Add PercentageAsset Float                  
                
                
IF (@SearchString <> '' AND @paramGroupByLevel1 <> 'Select' AND (@paramGroupByLevel2 <> 'Select' OR @paramGroupByLevel3 <> 'Select' OR @paramGroupByLevel4 <> 'Select')) BEGIN  
                
CREATE TABLE #GroupWisePercentageAsset(GroupEntity varchar(200),  
TotalEndingMarketValueBase float)  
  Declare @GroupByLevel1_Sql nvarchar(1000)                
  Declare @UpdatePercentageAsset_Sql nvarchar(1000)                
                
  Set @GroupByLevel1_Sql='Insert Into #GroupWisePercentageAsset Select '+ @paramGroupByLevel1+',Sum(EndingMarketValueBase) From #UnrealizedPNLTable Group By '+ @paramGroupByLevel1                
                  
  Set @UpdatePercentageAsset_Sql=                
  'UPDATE #UnrealizedPNLTable                
  SET #UnrealizedPNLTable.PercentageAsset =                 
  CASE                 
   WHEN TotalEndingMarketValueBase<>0                
   THEN ISNULL(((EndingMarketValueBase/TotalEndingMarketValueBase)),0)                
   ELSE 0.0                
  END                
  FROM #UnrealizedPNLTable PNL                
  LEFT OUTER JOIN #GroupWisePercentageAsset GWPA ON PNL.'+@paramGroupByLevel1+' = GWPA.GroupEntity'                
                  
  Exec sp_executesql @GroupByLevel1_Sql                
  Exec sp_executesql @UpdatePercentageAsset_Sql                
END ELSE IF (@SearchString <> '' AND (@paramGroupByLevel1 = 'Select' OR (@paramGroupByLevel2 = 'Select' AND @paramGroupByLevel3 = 'Select' AND @paramGroupByLevel4 = 'Select'))) BEGIN  
  Declare @TotalEndingMarketValueBase Float                
SET @TotalEndingMarketValueBase = (SELECT  
 SUM(EndingMarketValueBase)  
FROM #UnrealizedPNLTable)  
                                         
  UPDATE #UnrealizedPNLTable                
  SET #UnrealizedPNLTable.PercentageAsset =                 
  CASE                 
  WHEN @TotalEndingMarketValueBase <> 0 THEN ISNULL(((EndingMarketValueBase / @TotalEndingMarketValueBase)), 0)  
   ELSE 0.0                
  END          
END ELSE BEGIN  
  UPDATE #UnrealizedPNLTable                
  SET #UnrealizedPNLTable.PercentageAsset =0                
 END                
                      
                    
                          
IF (@SearchString <> '') BEGIN  
IF (@searchby = 'Symbol') BEGIN  
SELECT  
 *  
FROM #UnrealizedPNLTable  
INNER JOIN #Symbol  
 ON #Symbol.items = #UnrealizedPNLTable.Symbol  
  Order by symbol                          
END ELSE IF (@searchby = 'underlyingSymbol') BEGIN  
SELECT  
 *  
FROM #UnrealizedPNLTable  
INNER JOIN #Symbol  
 ON #Symbol.items = #UnrealizedPNLTable.underlyingSymbol  
  Order by symbol                          
END ELSE IF (@searchby = 'BloombergSymbol') BEGIN  
SELECT  
 *  
FROM #UnrealizedPNLTable  
INNER JOIN #Symbol  
 ON #Symbol.items = #UnrealizedPNLTable.BloombergSymbol  
  Order by symbol                          
END ELSE IF (@searchby = 'SedolSymbol') BEGIN  
SELECT  
 *  
FROM #UnrealizedPNLTable  
INNER JOIN #Symbol  
 ON #Symbol.items = #UnrealizedPNLTable.SedolSymbol  
  Order by symbol                        
END ELSE IF (@searchby = 'OSISymbol') BEGIN  
SELECT  
 *  
FROM #UnrealizedPNLTable  
INNER JOIN #Symbol  
 ON #Symbol.items = #UnrealizedPNLTable.OSISymbol  
  Order by symbol                          
END ELSE IF (@searchby = 'IDCOSymbol') BEGIN  
SELECT  
 *  
FROM #UnrealizedPNLTable  
INNER JOIN #Symbol  
 ON #Symbol.items = #UnrealizedPNLTable.IDCOSymbol  
  Order by symbol                          
END ELSE IF (@searchby = 'ISINSymbol') BEGIN  
SELECT  
 *  
FROM #UnrealizedPNLTable  
INNER JOIN #Symbol  
 ON #Symbol.items = #UnrealizedPNLTable.ISINSymbol  
  Order by symbol                          
END ELSE IF (@searchby = 'CUSIPSymbol') BEGIN  
SELECT  
 *  
FROM #UnrealizedPNLTable  
INNER JOIN #Symbol  
 ON #Symbol.items = #UnrealizedPNLTable.CUSIPSymbol  
  Order by Symbol                          
  end                                       
END ELSE BEGIN  
SELECT  
 *  
FROM #UnrealizedPNLTable  
ORDER BY symbol  
 End                            
                            
                      
Drop table #Assets,#Funds,#UnrealizedPNLTable,#Symbol,#TempSymbol 