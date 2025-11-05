/***************************************************************                                  
Author : Mukul Bhandari                                                                                                                             
Creation Date: August 29,2012                                         
Description : Get Unrealized PNL from Middleware DB                               
                                   
Modified By: Rahul Gupta \ Ankit                           
Modified Date: Nov 27,2012                              
Description: Configurable Parameters added.                        
                      
Modified Date: 16-May-2013                                                                                  
Modified By : Ankit                                                                                                                                                      
Description: Removed all configurable parameters and added in Table T_ReportPreferences                     
select * from t_companyfunds                             
                
Usage:                                
[P_MW_GetUnrealizedPNL] '2014/08/14','VSR_MW_V1','1309,1310,1311,1312,1313,1314,1315,1316,1317,1318,1319,1320,1321','1,2,3,4,5,6,7,8,9,10,11,12'   
  
 http://jira.nirvanasolutions.com:8080/browse/PRANA-4802                    
*******************************************************************/                                  
                                
                               
CREATE Procedure [dbo].[P_MW_GetUnrealizedPNL_RJO]                                  
(                                  
@Rundate datetime,                              
@ReportID Varchar(100),                    
@Funds Varchar(max),                    
@Assets Varchar(max)                              
)                                  
As                           
                    
                    
Select * Into #Funds                                                  
from dbo.Split(@Funds, ',')                    
                    
                    
Select * Into #Assets                                                 
from dbo.Split(@Assets, ',')                    
                    
--Select * from #Funds                    
--Select * from #Assets                    
                    
Create table #ParameterPreferences                    
(                    
 [FutureMV_ZeroOrEndingMVOrUnrealized] [int] NOT NULL,                    
 [FXMV_ZeroOrEndingMVOrUnrealized] [int] NOT NULL,                    
 [SwapMV_ZeroOrEndingMVOrUnrealized] [int] NOT NULL,                    
 [InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized] [int] NOT NULL,                    
 [TotalCostASZero_Futures] [bit] NOT NULL,                    
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
 [IncludeCommissionInPNL_Other] [bit] NOT NULL                    
)                      
                                                                                                         
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
from T_ReportPreferences where ReportID = @ReportID                    
                    
                    
Declare @FutureMV_ZeroOrEndingMVOrUnrealized int                    
Declare @FXMV_ZeroOrEndingMVOrUnrealized int                    
Declare @SwapMV_ZeroOrEndingMVOrUnrealized int                    
Declare @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized int                    
Declare @TotalCostASZero_Futures bit                    
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
                          
Set  @FutureMV_ZeroOrEndingMVOrUnrealized = (Select FutureMV_ZeroOrEndingMVOrUnrealized from #ParameterPreferences)                    
Set  @FXMV_ZeroOrEndingMVOrUnrealized = (Select FXMV_ZeroOrEndingMVOrUnrealized from #ParameterPreferences)                    
Set  @SwapMV_ZeroOrEndingMVOrUnrealized = (Select SwapMV_ZeroOrEndingMVOrUnrealized from #ParameterPreferences)                    
Set  @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = (Select InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized from #ParameterPreferences)                    
Set  @TotalCostASZero_Futures = (Select TotalCostASZero_Futures from #ParameterPreferences)                    
Set  @TotalCostASZero_FutOptions = (Select TotalCostASZero_FutOptions from #ParameterPreferences)                    
Set  @TotalCostASZero_FX = (Select TotalCostASZero_FX from #ParameterPreferences)                    
Set  @TotalCostASZero_Swaps = (Select TotalCostASZero_Swaps from #ParameterPreferences)                    
Set  @IncludeFXPNLinEquity = (Select IncludeFXPNLinEquity from #ParameterPreferences)                    
Set  @IncludeFXPNLinEquityOption = (Select IncludeFXPNLinEquityOption from #ParameterPreferences)                    
Set  @IncludeFXPNLinFX = (Select IncludeFXPNLinFX from #ParameterPreferences)                    
Set  @IncludeFXPNLinFutures = (Select IncludeFXPNLinFutures from #ParameterPreferences)                    
Set  @IncludeFXPNLinSwaps = (Select IncludeFXPNLinSwaps from #ParameterPreferences)                    
Set  @IncludeFXPNLinInternationalFutOptions = (Select IncludeFXPNLinInternationalFutOptions from #ParameterPreferences)        
Set  @IncludeFXPNLinOther = (Select IncludeFXPNLinOther from #ParameterPreferences)                    
Set  @IncludeCommissionInPNL_Equity = (Select IncludeCommissionInPNL_Equity from #ParameterPreferences)                    
Set  @IncludeCommissionInPNL_EquityOption = (Select IncludeCommissionInPNL_EquityOption from #ParameterPreferences)                    
Set  @IncludeCommissionInPNL_Futures = (Select IncludeCommissionInPNL_Futures from #ParameterPreferences)                    
Set  @IncludeCommissionInPNL_FutOptions = (Select IncludeCommissionInPNL_FutOptions from #ParameterPreferences)                    
Set  @IncludeCommissionInPNL_Swaps = (Select IncludeCommissionInPNL_Swaps from #ParameterPreferences)                    
Set  @IncludeCommissionInPNL_FX = (Select IncludeCommissionInPNL_FX from #ParameterPreferences)                    
Set  @IncludeCommissionInPNL_Other = (Select IncludeCommissionInPNL_Other from #ParameterPreferences)                    
                    
                    
--select * from #ParameterPreferences              
              
Declare @BaseCurrency Varchar(10)                        
Set  @BaseCurrency = (select CurrencySymbol from t_company Company                        
     left outer join T_Currency Currency                        
     on Company.BaseCurrencyID = Currency.CurrencyID)                        
--Select  @BaseCurrency                             
                               
SELECT                 
                                  
Rundate,                                
TradeDate,                 
PNL.Exchange,                                
-- Symbology Codes                                
Symbol,                                
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
--CASE                      
--WHEN (asset = 'CASH')                                     
--Then 'Cashhh'                     
--Else                      
--UDASector                      
--END AS           
UDASector,            
                             
UDACountry,                                
UDASecurityType,                                
UDAAssetClass,           
          
--CASE                      
--WHEN (asset = 'CASH')                                     
--Then 'Cashhh'                     
--Else                      
--UDASubSector                      
--END AS           
UDASubSector,            
                               
-- Basic Fields                                
UnitCostLocal,                                 
OpeningFXRate,                                 
UnitCostBase,                                 
EndingFXRate,                                 
EndingPriceLocal,                                 
EndingPriceBase ,                     
                  
--Open Trade Attributes                  
OpenTradeAttribute1,                  
OpenTradeAttribute2,                  
OpenTradeAttribute3,                  
OpenTradeAttribute4,                  
OpenTradeAttribute5,                  
OpenTradeAttribute6,                  
                  
                              
CASE                      
WHEN (asset = 'CASH')                                     
Then 0                      
Else                      
BeginningQuantity                      
END AS BeginningQuantity,                                     
--BeginningQuantity,                                       
Multiplier,                        
CASE                      
WHEN (asset = 'CASH')                                     
Then 1                       
Else                      
SideMultiplier                      
END AS SideMultiplier,                                      
--SideMultiplier,                                 
PutOrCall,                               
IsSwapped,                               
Open_CloseTag ,                                 
TotalOpenCommissionAndFees_Local ,                                 
TotalOpenCommissionAndFees_Base ,                              
--Derived Fields                               
          
Case                               
When (@TotalCostASZero_FX = 1) and (Asset = 'FX')                    
Then 0                          
When (@TotalCostASZero_Futures= 1) and (Asset = 'Future')                    
Then 0                     
When (@TotalCostASZero_FutOptions = 1) and (Asset = 'FutureOption')                    
Then 0                     
When (@TotalCostASZero_Swaps = 1) and (Asset = 'Equity' And IsSwapped = 1)                              
Then 0                      
When (asset = 'CASH')                             
Then 0                        
Else TotalCost_Local                              
End as TotalCost_Local,                              
                              
Case                               
When (@TotalCostASZero_FX = 1) and (Asset = 'FX')                    
Then 0                          
When (@TotalCostASZero_Futures= 1) and (Asset = 'Future')                    
Then 0                     
When (@TotalCostASZero_FutOptions = 1) and (Asset = 'FutureOption')                    
Then 0                     
When (@TotalCostASZero_Swaps = 1) and (Asset = 'Equity' And IsSwapped = 1)                               
Then 0                        
When (asset = 'CASH')                             
Then 0                             
Else TotalCost_Base                              
End as TotalCost_Base,                              
                                
                            
                             
Case                     
-- When Market Value is Equal to Market Value                              
When                     
(                    
(Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized = 1) or                     
(Asset = 'FX' and @FXMV_ZeroOrEndingMVOrUnrealized = 1) or                     
(Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized = 1) or                    
(Asset = 'FutureOption' And (@BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 1)                    
)                    
Then                     
EndingMarketValueLocal                      
-- When Market Value is Equal to Unrealize P&L and without Commission                    
When                     
(                    
(Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Futures = 0) or                     
(Asset = 'FX' and @FXMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FX = 0) or                     
(Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Swaps = 0) or                    
(Asset = 'FutureOption' And (@BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FutOptions = 0)                    
)                    
Then                     
UnrealizedTotalGainOnCostD2_Local  + TotalOpenCommissionAndFees_Local                    
-- When Market Value is Equal to Unrealize P&L and with Commission                    
When                     
(           
(Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Futures = 1) or                     
(Asset = 'FX' and @FXMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FX = 1) or                     
(Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Swaps = 1) or                    
(Asset = 'FutureOption' And (@BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FutOptions = 1)                    
)                    
Then        
UnrealizedTotalGainOnCostD2_Local                    
-- When Market Value is Equal to 0                    
When                     
(                    
(Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized not in (1,2)) or                     
(Asset = 'FX' and @FXMV_ZeroOrEndingMVOrUnrealized not in (1,2)) or                     
(Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized not in (1,2)) or                    
(Asset = 'FutureOption' And (@BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized not in (1,2))                    
)                    
Then                     
0                            
Else EndingMarketValueLocal                              
End as EndingMarketValueLocal,                               
                              
                      
                      
Case                     
-- When Market Value is Equal to Market Value                              
When                     
(                    
(Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized = 1) or                     
(Asset = 'FX' and @FXMV_ZeroOrEndingMVOrUnrealized = 1) or                     
(Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized = 1) or                    
(Asset = 'FutureOption' And (@BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 1)                    
)                    
Then                     
EndingMarketValueBase                      
-- When Market Value is Equal to Unrealize P&L and without Commission and with Single FX Rate                    
When                     
(                    
(Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Futures = 0 and @IncludeFXPNLinFutures = 0) or                     
(Asset = 'FX' and @FXMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FX = 0 and @IncludeFXPNLinFX = 0) or                     
(Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Swaps = 0 and @IncludeFXPNLinSwaps = 0) or                    
(Asset = 'FutureOption' And (@BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FutOptions = 0 and @IncludeFXPNLinInternationalFutOptions = 0)                    
)                    
Then                     
UnrealizedTradingGainOnCostD2_Base  + TotalOpenCommissionAndFees_Local                    
-- When Market Value is Equal to Unrealize P&L and with Commission and with Single FX Rate                    
When                     
(                    
(Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Futures = 1 and @IncludeFXPNLinFutures = 0) or                     
(Asset = 'FX' and @FXMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FX = 1 and @IncludeFXPNLinFX = 0) or                     
(Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Swaps = 1 and @IncludeFXPNLinSwaps = 0) or                    
(Asset = 'FutureOption' And (@BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FutOptions = 1 and @IncludeFXPNLinInternationalFutOptions = 0)                    
)                    
Then                     
UnrealizedTradingGainOnCostD2_Base                    
-- When Market Value is Equal to Unrealize P&L and without Commission and with Both FX Rate                    
When                     
(                    
(Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Futures = 0 and @IncludeFXPNLinFutures = 1) or                     
(Asset = 'FX' and @FXMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FX = 0 and @IncludeFXPNLinFX = 1) or                     
(Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Swaps = 0 and @IncludeFXPNLinSwaps = 1) or                    
(Asset = 'FutureOption' And (@BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FutOptions = 0 and @IncludeFXPNLinInternationalFutOptions = 1)                    
)                    
Then                     
UnrealizedTotalGainOnCostD2_Base + TotalOpenCommissionAndFees_Base                    
-- When Market Value is Equal to Unrealize P&L and with Commission and with Both FX Rate                    
When                     
(                    
(Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Futures = 1 and @IncludeFXPNLinFutures = 1) or                     
(Asset = 'FX' and @FXMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FX = 1 and @IncludeFXPNLinFX = 1) or                     
(Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Swaps = 1 and @IncludeFXPNLinSwaps = 1) or                    
(Asset = 'FutureOption' And (@BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FutOptions = 1 and @IncludeFXPNLinInternationalFutOptions = 1)                    
)                    
Then                     
UnrealizedTotalGainOnCostD2_Base                    
-- When Market Value is Equal to 0                    
When                     
(                    
(Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized not in (1,2)) or                     
(Asset = 'FX' and @FXMV_ZeroOrEndingMVOrUnrealized not in (1,2)) or                     
(Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized not in (1,2)) or                    
(Asset = 'FutureOption' And (@BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized not in (1,2))                    
)                    
Then                     
0                              
Else EndingMarketValueBase                              
End as EndingMarketValueBase,                     
                    
                    
                        
                        
UnrealizedTradingGainOnCostD2_Base ,                                 
UnrealizedFXGainOnCostD2_Base,                                
--UnrealizedTotalGainOnCostD2_Local,                                
                     
                  
Case                     
-- When Local Unrealize P&L is without Commission                  
When                     
(                    
(Asset = 'Equity' And IsSwapped = 0 and @IncludeCommissionInPNL_Equity = 0) or                     
(Asset = 'EquityOption' and @IncludeCommissionInPNL_EquityOption = 0) or                     
(Asset = 'Future' and @IncludeCommissionInPNL_Futures = 0 ) or                     
(Asset = 'FX' and @IncludeCommissionInPNL_FX = 0) or                     
(Asset = 'Equity' And IsSwapped = 1 and @IncludeCommissionInPNL_Swaps = 0) or                    
(Asset = 'FutureOption' and @IncludeCommissionInPNL_FutOptions = 0) or                    
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeCommissionInPNL_Other = 0)                    
)                    
Then                     
UnrealizedTotalGainOnCostD2_Local  + TotalOpenCommissionAndFees_Local       
-- When Local Unrealize P&L is with Commission                    
When                     
(                    
(Asset = 'Equity' And IsSwapped = 0 and @IncludeCommissionInPNL_Equity = 1) or                     
(Asset = 'EquityOption' and @IncludeCommissionInPNL_EquityOption = 1) or                     
(Asset = 'Future' and @IncludeCommissionInPNL_Futures = 1) or                     
(Asset = 'FX' and @IncludeCommissionInPNL_FX = 1) or                     
(Asset = 'Equity' And IsSwapped = 1 and @IncludeCommissionInPNL_Swaps = 1) or                    
(Asset = 'FutureOption' and @IncludeCommissionInPNL_FutOptions = 1) or                    
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeCommissionInPNL_Other = 1)                    
)                    
Then                     
UnrealizedTotalGainOnCostD2_Local                              
Else UnrealizedTotalGainOnCostD2_Local                              
End as UnrealizedTotalGainOnCostD2_Local,                                
                           
                
                            
Case                     
-- When Unrealize P&L is without Commission and with Single FX Rate                    
When                     
(                    
(Asset = 'Equity' And IsSwapped = 0 and @IncludeCommissionInPNL_Equity = 0 and @IncludeFXPNLinEquity = 0) or                     
(Asset = 'EquityOption' and @IncludeCommissionInPNL_EquityOption = 0 and @IncludeFXPNLinEquityOption = 0) or                     
(Asset = 'Future' and @IncludeCommissionInPNL_Futures = 0 and @IncludeFXPNLinFutures = 0) or                     
(Asset = 'FX' and @IncludeCommissionInPNL_FX = 0 and @IncludeFXPNLinFX = 0) or                     
(Asset = 'Equity' And IsSwapped = 1 and @IncludeCommissionInPNL_Swaps = 0 and @IncludeFXPNLinSwaps = 0) or                    
(Asset = 'FutureOption' and @IncludeCommissionInPNL_FutOptions = 0 and @IncludeFXPNLinInternationalFutOptions = 0) or                    
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeCommissionInPNL_Other = 0 and @IncludeFXPNLinOther = 0)                    
)                    
Then                     
UnrealizedTradingGainOnCostD2_Base  + TotalOpenCommissionAndFees_Local                    
-- When Unrealize P&L is with Commission and with Single FX Rate                    
When                     
(                    
(Asset = 'Equity' And IsSwapped = 0 and @IncludeCommissionInPNL_Equity = 1 and @IncludeFXPNLinEquity = 0) or                     
(Asset = 'EquityOption' and @IncludeCommissionInPNL_EquityOption = 1 and @IncludeFXPNLinEquityOption = 0) or                     
(Asset = 'Future' and @IncludeCommissionInPNL_Futures = 1 and @IncludeFXPNLinFutures = 0) or                     
(Asset = 'FX' and @IncludeCommissionInPNL_FX = 1 and @IncludeFXPNLinFX = 0) or                     
(Asset = 'Equity' And IsSwapped = 1 and @IncludeCommissionInPNL_Swaps = 1 and @IncludeFXPNLinSwaps = 0) or                    
(Asset = 'FutureOption' and @IncludeCommissionInPNL_FutOptions = 1 and @IncludeFXPNLinInternationalFutOptions = 0) or                    
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeCommissionInPNL_Other = 1 and @IncludeFXPNLinOther = 0)                    
)                    
Then                     
UnrealizedTradingGainOnCostD2_Base                     
-- When Unrealize P&L is without Commission and with Both FX Rate                    
When                     
(                    
(Asset = 'Equity' And IsSwapped = 0 and @IncludeCommissionInPNL_Equity = 0 and @IncludeFXPNLinEquity = 1) or                     
(Asset = 'EquityOption' and @IncludeCommissionInPNL_EquityOption = 0 and @IncludeFXPNLinEquityOption = 1) or                     
(Asset = 'Future' and @IncludeCommissionInPNL_Futures = 0 and @IncludeFXPNLinFutures = 1) or                     
(Asset = 'FX' and @IncludeCommissionInPNL_FX = 0 and @IncludeFXPNLinFX = 1) or     
(Asset = 'Equity' And IsSwapped = 1 and @IncludeCommissionInPNL_Swaps = 0 and @IncludeFXPNLinSwaps = 1) or                    
(Asset = 'FutureOption' and @IncludeCommissionInPNL_FutOptions = 0 and @IncludeFXPNLinInternationalFutOptions = 1) or                    
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeCommissionInPNL_Other = 0 and @IncludeFXPNLinOther = 1)                    
)                    
Then                     
UnrealizedTotalGainOnCostD2_Base + TotalOpenCommissionAndFees_Base                        
-- When Unrealize P&L is with Commission and with Both FX Rate                    
When                     
(                    
(Asset = 'Equity' And IsSwapped = 0 and @IncludeCommissionInPNL_Equity = 1 and @IncludeFXPNLinEquity = 1) or                     
(Asset = 'EquityOption' and @IncludeCommissionInPNL_EquityOption = 1 and @IncludeFXPNLinEquityOption = 1) or             
(Asset = 'Future' and @IncludeCommissionInPNL_Futures = 1 and @IncludeFXPNLinFutures = 1) or                     
(Asset = 'FX' and @IncludeCommissionInPNL_FX = 1 and @IncludeFXPNLinFX = 1) or                     
(Asset = 'Equity' And IsSwapped = 1 and @IncludeCommissionInPNL_Swaps = 1 and @IncludeFXPNLinSwaps = 1) or                    
(Asset = 'FutureOption' and @IncludeCommissionInPNL_FutOptions = 1 and @IncludeFXPNLinInternationalFutOptions = 1) or                    
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeCommissionInPNL_Other = 1 and @IncludeFXPNLinOther = 1)                    
)                    
Then                     
UnrealizedTotalGainOnCostD2_Base                            
Else UnrealizedTotalGainOnCostD2_Base                              
End as UnrealizedTotalGainOnCostD2_Base                                
                                
                            
FROM T_MW_GenericPNL  PNL                    
inner join T_asset A on A.Assetname = PNL.Asset                    
inner join T_CompanyFunds F on F.FundName = PNL.Fund                    
                          
Where                     
(Open_CloseTag ='O')                     
and                     
datediff(d,Rundate,@rundate) = 0                     
and                    
A.AssetID in (Select * from #Assets)                    
and                    
F.CompanyFundID in (Select * from #Funds)        
--and  bloombergsymbol='ADU4 CURNCY'             
order by fund,symbol                 
                    
Drop table #Assets,#Funds 