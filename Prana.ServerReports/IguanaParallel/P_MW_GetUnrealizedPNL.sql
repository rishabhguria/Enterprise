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
               
Modified Date: 24-DEC-2014                                                                                  
Modified By : Ankit Misra                                                                                                                                                     
Description: Search By and Search String parameters added        
      
Modified Date: 20-Mar-2015                                                                                  
Modified By : Pooja Porwal                                                                                                                                                     
Description: Added a new field UnderlyingSymbolCompanyName                 
                 
                         
Select * from T_CompanyFunds                    
Usage:                  
[P_MW_GetUnrealizedPNL] '2012-07-26','MTM_V0','1183,1182','3','','TickerSymbol'                     
*******************************************************************/                    
                  
                 
ALTER Procedure [dbo].[P_MW_GetUnrealizedPNL]                    
(                    
@Rundate datetime,                
@ReportID Varchar(100),      
@Funds Varchar(max),      
@Assets Varchar(max),              
@SearchString Varchar(5000) ,                    
@SearchBy Varchar(100),        
@paramGroupByLevel1 Varchar(100),        
@paramGroupByLevel2 Varchar(100),        
@paramGroupByLevel3 Varchar(100),        
@paramGroupByLevel4 Varchar(100)                    
)                    
As             
      
      
Select * Into #Funds                                    
from dbo.Split(@Funds, ',')      
      
      
Select * Into #Assets                                   
from dbo.Split(@Assets, ',')      
      
Select * InTo #TempSymbol                 
From dbo.split(@SearchString , ',')               
                
Select Distinct * InTo #Symbol                 
From #TempSymbol                     
                    
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
      
      
--Declare @BaseCurrency Varchar(10)          
--Set  @BaseCurrency = (select CurrencySymbol from t_company Company          
--     left outer join T_Currency Currency          
--     on Company.BaseCurrencyID = Currency.CurrencyID)          
--Select  @BaseCurrency               
                 
SELECT                     
                    
Rundate,                  
TradeDate, 
ExpirationDate,                  
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
    
--Open Trade Attributes    
OpenTradeAttribute1,    
OpenTradeAttribute2,    
OpenTradeAttribute3,    
OpenTradeAttribute4,    
OpenTradeAttribute5,    
OpenTradeAttribute6,    
    
                
CASE        
WHEN (asset = 'CASH')                       
----http://jira.nirvanasolutions.com:8080/browse/PRANA-7912                  
----Then EndingMarketValueBase     
Then EndingMarketValueLocal        
Else BeginningQuantity        
END AS BeginningQuantity,                       
Multiplier,          
CASE        
WHEN (asset = 'CASH')                       
Then 1         
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
When (@TotalCostASZero_FX = 1) and (Asset = 'FX' Or Asset = 'FXForward')        
Then 0            
When (@TotalCostASZero_Futures= 1) and (Asset = 'Future')      
Then 0       
When (@TotalCostASZero_FutOptions = 1) and (Asset = 'FutureOption')      
Then 0       
When (@TotalCostASZero_Swaps = 1) and (Asset = 'Equity' And IsSwapped = 1)                
Then 0        
When (asset = 'CASH')               
Then 0          
When (@IncludeCommissionInPNL_Equity = 0) and (Asset = 'Equity' And IsSwapped = 0)                
Then TotalCost_Local-TotalOpenCommissionAndFees_Local    
When (@IncludeCommissionInPNL_EquityOption = 0) and (Asset = 'EquityOption')                
Then TotalCost_Local-TotalOpenCommissionAndFees_Local    
When (@IncludeCommissionInPNL_Futures = 0) and (Asset = 'Future')                
Then TotalCost_Local-TotalOpenCommissionAndFees_Local    
When (@IncludeCommissionInPNL_FutOptions = 0) and (Asset = 'FutureOption')                
Then TotalCost_Local-TotalOpenCommissionAndFees_Local    
When (@IncludeCommissionInPNL_Swaps = 0) and (Asset = 'Equity' And IsSwapped = 1)                
Then TotalCost_Local-TotalOpenCommissionAndFees_Local    
    
Else TotalCost_Local                
End as TotalCost_Local,                
                
Case   
When (@TotalCostASZero_FX = 1) and (Asset = 'FX' Or Asset = 'FXForward')        
Then 0            
When (@TotalCostASZero_Futures= 1) and (Asset = 'Future')      
Then 0       
When (@TotalCostASZero_FutOptions = 1) and (Asset = 'FutureOption')      
Then 0       
When (@TotalCostASZero_Swaps = 1) and (Asset = 'Equity' And IsSwapped = 1)                 
Then 0          
When (asset = 'CASH')               
Then 0     
When (@IncludeCommissionInPNL_Equity = 0) and (Asset = 'Equity' And IsSwapped = 0)                
Then TotalCost_Base-TotalOpenCommissionAndFees_Base    
When (@IncludeCommissionInPNL_EquityOption = 0) and (Asset = 'EquityOption')                
Then TotalCost_Base-TotalOpenCommissionAndFees_Base    
When (@IncludeCommissionInPNL_Futures = 0) and (Asset = 'Future')                
Then TotalCost_Base-TotalOpenCommissionAndFees_Base    
When (@IncludeCommissionInPNL_FutOptions = 0) and (Asset = 'FutureOption')                
Then TotalCost_Base-TotalOpenCommissionAndFees_Base    
When (@IncludeCommissionInPNL_Swaps = 0) and (Asset = 'Equity' And IsSwapped = 1)                
Then TotalCost_Base-TotalOpenCommissionAndFees_Base    
              
Else TotalCost_Base                
End as TotalCost_Base,                
                  
              
               
Case       
-- When Market Value is Equal to Market Value                
When       
(      
(Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized = 1) or       
((Asset = 'FX' Or Asset = 'FXForward') and @FXMV_ZeroOrEndingMVOrUnrealized = 1) or         
(Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized = 1) or      
(Asset = 'FutureOption' And (BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 1)      
)      
Then       
EndingMarketValueLocal        
-- When Market Value is Equal to Unrealize P&L and without Commission      
When       
(      
(Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Futures = 0) or       
((Asset = 'FX' Or Asset = 'FXForward') and @FXMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FX = 0) or         
(Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Swaps = 0) or      
(Asset = 'FutureOption' And (BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FutOptions = 0)      
)      
Then       
UnrealizedTotalGainOnCostD2_Local  + TotalOpenCommissionAndFees_Local      
-- When Market Value is Equal to Unrealize P&L and with Commission      
When       
(      
(Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Futures = 1) or       
((Asset = 'FX' Or Asset = 'FXForward') and @FXMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FX = 1) or         
(Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Swaps = 1) or      
(Asset = 'FutureOption' And (BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FutOptions = 1)      
)      
Then       
UnrealizedTotalGainOnCostD2_Local      
-- When Market Value is Equal to 0      
When       
(      
(Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized not in (1,2)) or       
((Asset = 'FX' Or Asset = 'FXForward') and @FXMV_ZeroOrEndingMVOrUnrealized not in (1,2)) or         
(Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized not in (1,2)) or      
(Asset = 'FutureOption' And (BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized not in (1,2))      
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
((Asset = 'FX' Or Asset = 'FXForward') and @FXMV_ZeroOrEndingMVOrUnrealized = 1) or         
(Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized = 1) or      
(Asset = 'FutureOption' And (BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 1)      
)      
Then       
EndingMarketValueBase        
-- When Market Value is Equal to Unrealize P&L and without Commission and with Single FX Rate      
When       
(      
(Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Futures = 0 and @IncludeFXPNLinFutures = 0) or       
((Asset = 'FX' Or Asset = 'FXForward') and @FXMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FX = 0 and @IncludeFXPNLinFX = 0) or         
(Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Swaps = 0 and @IncludeFXPNLinSwaps = 0) or      
(Asset = 'FutureOption' And (BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FutOptions = 0 and @IncludeFXPNLinInternationalFutOptions = 0)      
)      
Then       
UnrealizedTradingGainOnCostD2_Base  + TotalOpenCommissionAndFees_Local      
-- When Market Value is Equal to Unrealize P&L and with Commission and with Single FX Rate      
When       
(      
(Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Futures = 1 and @IncludeFXPNLinFutures = 0) or       
((Asset = 'FX' Or Asset = 'FXForward') and @FXMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FX = 1 and @IncludeFXPNLinFX = 0) or         
(Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Swaps = 1 and @IncludeFXPNLinSwaps = 0) or      
(Asset = 'FutureOption' And (BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FutOptions = 1 and @IncludeFXPNLinInternationalFutOptions = 0)      
)      
Then       
UnrealizedTradingGainOnCostD2_Base      
-- When Market Value is Equal to Unrealize P&L and without Commission and with Both FX Rate      
When       
(      
(Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Futures = 0 and @IncludeFXPNLinFutures = 1) or       
((Asset = 'FX' Or Asset = 'FXForward') and @FXMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FX = 0 and @IncludeFXPNLinFX = 1) or         
(Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Swaps = 0 and @IncludeFXPNLinSwaps = 1) or      
(Asset = 'FutureOption' And (BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FutOptions = 0 and @IncludeFXPNLinInternationalFutOptions = 1)      
)      
Then       
UnrealizedTotalGainOnCostD2_Base + TotalOpenCommissionAndFees_Base      
-- When Market Value is Equal to Unrealize P&L and with Commission and with Both FX Rate      
When       
(      
(Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Futures = 1 and @IncludeFXPNLinFutures = 1) or       
((Asset = 'FX' Or Asset = 'FXForward') and @FXMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FX = 1 and @IncludeFXPNLinFX = 1) or         
(Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Swaps = 1 and @IncludeFXPNLinSwaps = 1) or      
(Asset = 'FutureOption' And (BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FutOptions = 1 and @IncludeFXPNLinInternationalFutOptions = 1)      
)      
Then       
UnrealizedTotalGainOnCostD2_Base      
-- When Market Value is Equal to 0      
When       
(      
(Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized not in (1,2)) or       
((Asset = 'FX' Or Asset = 'FXForward') and @FXMV_ZeroOrEndingMVOrUnrealized not in (1,2)) or         
(Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized not in (1,2)) or      
(Asset = 'FutureOption' And (BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized not in (1,2))      
)      
Then       
0                
Else EndingMarketValueBase                
End as EndingMarketValueBase,       
UnrealizedTradingGainOnCostD2_Base ,                   
UnrealizedFXGainOnCostD2_Base,                  
Case       
-- When Local Unrealize P&L is without Commission    
When       
(      
(Asset = 'Equity' And IsSwapped = 0 and @IncludeCommissionInPNL_Equity = 0) or       
(Asset = 'EquityOption' and @IncludeCommissionInPNL_EquityOption = 0) or       
(Asset = 'Future' and @IncludeCommissionInPNL_Futures = 0 ) or       
((Asset = 'FX' Or Asset = 'FXForward') and @IncludeCommissionInPNL_FX = 0) or         
(Asset = 'Equity' And IsSwapped = 1 and @IncludeCommissionInPNL_Swaps = 0) or      
(Asset = 'FutureOption' and @IncludeCommissionInPNL_FutOptions = 0) or      
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX','FXForward') and @IncludeCommissionInPNL_Other = 0)        
)      
Then UnrealizedTotalGainOnCostD2_Local  + TotalOpenCommissionAndFees_Local      
-- When Local Unrealize P&L is with Commission      
When       
(      
(Asset = 'Equity' And IsSwapped = 0 and @IncludeCommissionInPNL_Equity = 1) or       
(Asset = 'EquityOption' and @IncludeCommissionInPNL_EquityOption = 1) or       
(Asset = 'Future' and @IncludeCommissionInPNL_Futures = 1) or       
((Asset = 'FX' Or Asset = 'FXForward') and @IncludeCommissionInPNL_FX = 1) or         
(Asset = 'Equity' And IsSwapped = 1 and @IncludeCommissionInPNL_Swaps = 1) or      
(Asset = 'FutureOption' and @IncludeCommissionInPNL_FutOptions = 1) or      
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX','FXForward') and @IncludeCommissionInPNL_Other = 1)        
)      
Then UnrealizedTotalGainOnCostD2_Local                
Else UnrealizedTotalGainOnCostD2_Local                
End as UnrealizedTotalGainOnCostD2_Local,                  
             
Case       
-- When Unrealize P&L is without Commission and with Single FX Rate      
When       
(      
(Asset = 'Equity' And IsSwapped = 0 and @IncludeCommissionInPNL_Equity = 0 and @IncludeFXPNLinEquity = 0) or       
(Asset = 'EquityOption' and @IncludeCommissionInPNL_EquityOption = 0 and @IncludeFXPNLinEquityOption = 0) or       
(Asset = 'Future' and @IncludeCommissionInPNL_Futures = 0 and @IncludeFXPNLinFutures = 0) or       
((Asset = 'FX' Or Asset = 'FXForward') and @IncludeCommissionInPNL_FX = 0 and @IncludeFXPNLinFX = 0) or         
(Asset = 'Equity' And IsSwapped = 1 and @IncludeCommissionInPNL_Swaps = 0 and @IncludeFXPNLinSwaps = 0) or      
(Asset = 'FutureOption' and @IncludeCommissionInPNL_FutOptions = 0 and @IncludeFXPNLinInternationalFutOptions = 0) or      
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX','FXForward') and @IncludeCommissionInPNL_Other = 0 and @IncludeFXPNLinOther = 0)        
)      
Then UnrealizedTradingGainOnCostD2_Base  + TotalOpenCommissionAndFees_Local      
-- When Unrealize P&L is with Commission and with Single FX Rate      
When       
(      
(Asset = 'Equity' And IsSwapped = 0 and @IncludeCommissionInPNL_Equity = 1 and @IncludeFXPNLinEquity = 0) or       
(Asset = 'EquityOption' and @IncludeCommissionInPNL_EquityOption = 1 and @IncludeFXPNLinEquityOption = 0) or       
(Asset = 'Future' and @IncludeCommissionInPNL_Futures = 1 and @IncludeFXPNLinFutures = 0) or       
((Asset = 'FX' Or Asset = 'FXForward') and @IncludeCommissionInPNL_FX = 1 and @IncludeFXPNLinFX = 0) or         
(Asset = 'Equity' And IsSwapped = 1 and @IncludeCommissionInPNL_Swaps = 1 and @IncludeFXPNLinSwaps = 0) or      
(Asset = 'FutureOption' and @IncludeCommissionInPNL_FutOptions = 1 and @IncludeFXPNLinInternationalFutOptions = 0) or      
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX','FXForward') and @IncludeCommissionInPNL_Other = 1 and @IncludeFXPNLinOther = 0)        
)      
Then UnrealizedTradingGainOnCostD2_Base       
-- When Unrealize P&L is without Commission and with Both FX Rate      
When       
(      
(Asset = 'Equity' And IsSwapped = 0 and @IncludeCommissionInPNL_Equity = 0 and @IncludeFXPNLinEquity = 1) or       
(Asset = 'EquityOption' and @IncludeCommissionInPNL_EquityOption = 0 and @IncludeFXPNLinEquityOption = 1) or       
(Asset = 'Future' and @IncludeCommissionInPNL_Futures = 0 and @IncludeFXPNLinFutures = 1) or       
((Asset = 'FX' Or Asset = 'FXForward') and @IncludeCommissionInPNL_FX = 0 and @IncludeFXPNLinFX = 1) or         
(Asset = 'Equity' And IsSwapped = 1 and @IncludeCommissionInPNL_Swaps = 0 and @IncludeFXPNLinSwaps = 1) or      
(Asset = 'FutureOption' and @IncludeCommissionInPNL_FutOptions = 0 and @IncludeFXPNLinInternationalFutOptions = 1) or      
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX','FXForward') and @IncludeCommissionInPNL_Other = 0 and @IncludeFXPNLinOther = 1)        
)      
Then UnrealizedTotalGainOnCostD2_Base + TotalOpenCommissionAndFees_Base          
-- When Unrealize P&L is with Commission and with Both FX Rate      
When       
(      
(Asset = 'Equity' And IsSwapped = 0 and @IncludeCommissionInPNL_Equity = 1 and @IncludeFXPNLinEquity = 1) or       
(Asset = 'EquityOption' and @IncludeCommissionInPNL_EquityOption = 1 and @IncludeFXPNLinEquityOption = 1) or       
(Asset = 'Future' and @IncludeCommissionInPNL_Futures = 1 and @IncludeFXPNLinFutures = 1) or       
((Asset = 'FX' Or Asset = 'FXForward') and @IncludeCommissionInPNL_FX = 1 and @IncludeFXPNLinFX = 1) or         
(Asset = 'Equity' And IsSwapped = 1 and @IncludeCommissionInPNL_Swaps = 1 and @IncludeFXPNLinSwaps = 1) or      
(Asset = 'FutureOption' and @IncludeCommissionInPNL_FutOptions = 1 and @IncludeFXPNLinInternationalFutOptions = 1) or      
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX','FXForward') and @IncludeCommissionInPNL_Other = 1 and @IncludeFXPNLinOther = 1)        
)      
Then UnrealizedTotalGainOnCostD2_Base              
Else UnrealizedTotalGainOnCostD2_Base                
End as UnrealizedTotalGainOnCostD2_Base,      
OriginalPurchaseDate,        
BaseCurrency                                       
                  
InTo #UnrealizedPNLTable              
                  
FROM T_MW_GenericPNL  PNL      
inner join T_asset A on A.Assetname = PNL.Asset      
inner join T_CompanyFunds F on F.FundName = PNL.Fund      
            
Where       
(Open_CloseTag ='O') And DateDiff(Day,Rundate,@rundate) = 0       
And A.AssetID in (Select * from #Assets)      
And F.CompanyFundID in (Select * from #Funds)      
      
      
Alter table #UnrealizedPNLTable          
Add UnderlyingSymbolCompanyName nvarchar(200)          
          
Update  #UnrealizedPNLTable          
Set UnderlyingSymbolCompanyName = SM.CompanyName          
From  #UnrealizedPNLTable URPT          
Inner Join V_SecMasterData_WithUnderlying  SM on SM.TickerSymbol = URPT.UnderlyingSymbol             
          
Alter table #UnrealizedPNLTable        
Add PercentageAsset Float          
        
        
If(@SearchString <> '' and @paramGroupByLevel1<>'Select' AND (@paramGroupByLevel2<>'Select' OR @paramGroupByLevel3<>'Select' OR @paramGroupByLevel4<>'Select'))        
 BEGIN        
        
  CREATE Table #GroupWisePercentageAsset                                 
  (         
   GroupEntity Varchar(200),                   
   TotalEndingMarketValueBase Float                                                           
  )        
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
 END        
ELSE IF (@SearchString <> '' and (@paramGroupByLevel1='Select' OR (@paramGroupByLevel2='Select' AND @paramGroupByLevel3='Select' AND @paramGroupByLevel4='Select')))                                  
  BEGIN         
  Declare @TotalEndingMarketValueBase Float        
  Set @TotalEndingMarketValueBase = (Select Sum(EndingMarketValueBase) From #UnrealizedPNLTable)        
                                 
  UPDATE #UnrealizedPNLTable        
  SET #UnrealizedPNLTable.PercentageAsset =         
  CASE         
   WHEN @TotalEndingMarketValueBase<>0        
   THEN ISNULL(((EndingMarketValueBase/@TotalEndingMarketValueBase)),0)        
   ELSE 0.0        
  END                    
   END         
ELSE        
 BEGIN        
  UPDATE #UnrealizedPNLTable        
  SET #UnrealizedPNLTable.PercentageAsset =0        
 END        
              
            
                  
If(@SearchString <> '')                             
 Begin                           
  if (@searchby='Symbol')                  
  begin                  
  SELECT * FROM #UnrealizedPNLTable                  
  Inner Join #Symbol on #Symbol.items = #UnrealizedPNLTable.Symbol                  
  Order by symbol                  
  end                  
  else if (@searchby='underlyingSymbol')                  
  begin                  
  SELECT * FROM #UnrealizedPNLTable                  
  Inner Join #Symbol on #Symbol.items = #UnrealizedPNLTable.underlyingSymbol                  
  Order by symbol                  
  end                    
  else if (@searchby='BloombergSymbol')                  
  begin                  
  SELECT * FROM #UnrealizedPNLTable                  
  Inner Join #Symbol on #Symbol.items = #UnrealizedPNLTable.BloombergSymbol                  
  Order by symbol                  
  end                      
  else if (@searchby='SedolSymbol')                  
  begin                  
  SELECT * FROM #UnrealizedPNLTable                  
  Inner Join #Symbol on #Symbol.items = #UnrealizedPNLTable.SedolSymbol                  
  Order by symbol                  
  end                      
  else if (@searchby='OSISymbol')                  
  begin                  
  SELECT * FROM #UnrealizedPNLTable                  
  Inner Join #Symbol on #Symbol.items = #UnrealizedPNLTable.OSISymbol                  
  Order by symbol                  
  end                      
  else if (@searchby='IDCOSymbol')                  
  begin                  
  SELECT * FROM #UnrealizedPNLTable                  
  Inner Join #Symbol on #Symbol.items = #UnrealizedPNLTable.IDCOSymbol                  
  Order by symbol                  
  end                      
  else if (@searchby='ISINSymbol')                  
  begin                  
  SELECT * FROM #UnrealizedPNLTable                  
  Inner Join #Symbol on #Symbol.items = #UnrealizedPNLTable.ISINSymbol                  
  Order by symbol                  
  end                     
  else if (@searchby='CUSIPSymbol')                  
  begin                  
  SELECT * FROM #UnrealizedPNLTable                  
  Inner Join #Symbol on #Symbol.items = #UnrealizedPNLTable.CUSIPSymbol                  
  Order by Symbol                  
  end                               
 End                              
Else                              
 Begin                              
  Select * from #UnrealizedPNLTable Order By symbol                    
 End                    
                    
              
Drop table #Assets,#Funds,#UnrealizedPNLTable,#Symbol,#TempSymbol 