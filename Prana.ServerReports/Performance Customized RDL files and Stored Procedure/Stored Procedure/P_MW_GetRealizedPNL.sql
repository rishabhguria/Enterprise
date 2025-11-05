                    
/*****************************************************************                  
Author:Ashish Poddar                                                                                                             
Creation Date: September 07, 2012                       
Description : Get Realized PNL from Middleware DB             
            
Modified By: Rahul Gupta \ Ankit        
Modified Date: Nov 27,2012            
Description: Configurable Parameters added.            
            
Modified Date: 20-Mar-2015                                                                                
Modified By : Pooja Porwal                                                                                                                                                   
Description: Added a new field UnderlyingSymbolCompanyName                             
                                                 
Parameters Description:            
@IncludeFXPNLinFutures : True - Include FX gain For futures in PNL else False.         
@IncludeFXPNLinFX : True - Include FX gain For  FX and FXForwards in PNL else False.           
@IncludeFXPNLinSwaps : True - Include FX gain For Swaps in PNL else False.              
@TotalCostAsZero - True - Show Total Cost as Zero for futures, FX, FXForwards and Swaps.  

Modified By: Sachin Mishra
Date: 28 AUG 2015
Desc:http://jira.nirvanasolutions.com:8080/browse/PRANA-8010 
"Currency" in reports
                    
Usage :                 
P_MW_GetRealizedPNL '2012-4-30','2012-7-26','MTM_V0','1195','1,2,3,4,5,6'      
******************************************************************/                     
                
                
ALTER Procedure [dbo].[P_MW_GetRealizedPNL]                 
(                  
@StartDate datetime  ,                
@EndDate datetime,        
@ReportID Varchar(20),      
@Funds Varchar(max),      
@Assets Varchar(max),                
@SearchString Varchar(5000) ,                        
@SearchBy Varchar(100),          
@DivideFuturePnlIn6040 bit                     
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
      
              
SELECT                       
                
Rundate,      
Broker,    
PNL.Exchange,        
--Symbology Codes                 
Symbol,              
CUSIPSymbol ,                 
ISINSymbol ,                 
CASE        
WHEN (Asset = 'CASH')                       
Then 'Currency'        
Else SEDOLSymbol        
END AS SEDOLSymbol,                 
CASE        
WHEN (Asset = 'CASH')                       
Then 'Currency'        
Else BloombergSYmbol        
END AS BloombergSYmbol,                  
ReutersSYmbol ,                 
IDCOSymbol ,                 
OSISymbol,              
--Grouping Fields                   
Strategy,                
Fund,                 
Asset,                 
TradeCurrency,                 
Side,                 
SecurityName,                
MasterFund,                  
UDASector,                 
UDACountry,                
UDASecurityType,                
UDAAssetClass,                
UDASubSector ,    
  
--Open trade Attributes  
OpenTradeAttribute1,  
OpenTradeAttribute2,  
OpenTradeAttribute3,  
OpenTradeAttribute4,  
OpenTradeAttribute5,  
OpenTradeAttribute6,  
  
--Closed Trade Attributes  
ClosedTradeAttribute1,  
ClosedTradeAttribute2,  
ClosedTradeAttribute3,  
ClosedTradeAttribute4,  
ClosedTradeAttribute5,  
ClosedTradeAttribute6,  
             
--Basic Fields                
UnitCostLocal,                 
OpeningFXRate,                  
UnitCostBase,                 
EndingFXRate,                 
ClosingPriceLocal,                   
ClosingPriceBase ,                 
EndingQuantity,                 
Multiplier,                 
SideMultiplier,                 
UnderlyingSymbol,                
TradeDate,                  
PutOrCall,                
ClosingDate,                 
Open_CloseTag,              
IsSwapped,               
TotalOpenCommissionAndFees_Local ,                 
TotalOpenCommissionAndFees_Base ,               
TotalClosedCommissionAndFees_Local ,                 
TotalClosedCommissionAndFees_Base ,    
PNL.OriginalPurchaseDate as OriginalPurchaseDate,                   
PNL.TotalProceeds_Local,  
PNL.TotalProceeds_Base,  
      
              
(TotalOpenCommissionAndFees_Local + TotalClosedCommissionAndFees_Local) as TotalCommissionAndFees_Local ,            
      
      
Case          
--Commission With Single FX Rate        
When       
(      
(Asset = 'Equity'  And IsSwapped = 0 and @IncludeFXPNLinEquity = 0 and @IncludeCommissionInPNL_Equity = 1) or       
(Asset = 'EquityOption' and @IncludeFXPNLinEquityOption = 0 and @IncludeCommissionInPNL_EquityOption =1 ) or       
(Asset = 'Future' and @IncludeFXPNLinFutures = 0 and @IncludeCommissionInPNL_Futures = 1) or       
(Asset = 'FX' and @IncludeFXPNLinFX = 0 and @IncludeCommissionInPNL_FX =1 ) or       
(Asset = 'Equity' And IsSwapped = 1 and @IncludeFXPNLinSwaps = 0 AND @IncludeCommissionInPNL_Swaps = 1 ) or       
(Asset = 'FutureOption' and @IncludeFXPNLinInternationalFutOptions = 0 and @IncludeCommissionInPNL_FutOptions = 1 ) or       
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeFXPNLinOther = 0 and @IncludeCommissionInPNL_Other =1 )      
)      
Then         
(TotalOpenCommissionAndFees_Local + TotalClosedCommissionAndFees_Local) * EndingFXRate            
--Commission With Single FX Rate      
When       
(      
(Asset = 'Equity'  And IsSwapped = 0 and @IncludeFXPNLinEquity = 1 and @IncludeCommissionInPNL_Equity = 1) or       
(Asset = 'EquityOption' and @IncludeFXPNLinEquityOption = 1 and @IncludeCommissionInPNL_EquityOption =1) or       
(Asset = 'Future' and @IncludeFXPNLinFutures = 1 and @IncludeCommissionInPNL_Futures = 1) or       
(Asset = 'FX' and @IncludeFXPNLinFX = 1 and @IncludeCommissionInPNL_FX =1 ) or       
(Asset = 'Equity' And IsSwapped = 1 and @IncludeFXPNLinSwaps = 1 AND @IncludeCommissionInPNL_Swaps = 1) or       
(Asset = 'FutureOption' and @IncludeFXPNLinInternationalFutOptions = 1 and @IncludeCommissionInPNL_FutOptions = 1) or       
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeFXPNLinOther = 1 and @IncludeCommissionInPNL_Other =1)         
)      
Then         
(TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base)   
  
  
-----------------------------------------------------------------------------------------  
  
When       
(      
(Asset = 'Equity'  And IsSwapped = 0 and @IncludeFXPNLinEquity = 0 AND @IncludeCommissionInPNL_Equity = 0) or       
(Asset = 'EquityOption' and @IncludeFXPNLinEquityOption = 0 AND @IncludeCommissionInPNL_EquityOption = 0) or       
(Asset = 'Future' and @IncludeFXPNLinFutures = 0 AND @IncludeCommissionInPNL_Futures = 0 ) or       
(Asset = 'FX' and @IncludeFXPNLinFX = 0 AND @IncludeCommissionInPNL_FX = 0) or       
(Asset = 'Equity' And IsSwapped = 1 and @IncludeFXPNLinSwaps = 0 AND @IncludeCommissionInPNL_Swaps = 0) or       
(Asset = 'FutureOption' and @IncludeFXPNLinInternationalFutOptions = 0 AND @IncludeCommissionInPNL_FutOptions = 0 ) or       
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeFXPNLinOther = 0 AND @IncludeCommissionInPNL_Other = 0)      
)      
Then         
0  
--Commission With Single FX Rate      
When       
(      
(Asset = 'Equity'  And IsSwapped = 0 and @IncludeFXPNLinEquity = 1 AND @IncludeCommissionInPNL_Equity =0 ) or       
(Asset = 'EquityOption' and @IncludeFXPNLinEquityOption = 1 AND @IncludeCommissionInPNL_EquityOption = 0) or       
(Asset = 'Future' and @IncludeFXPNLinFutures = 1 AND @IncludeCommissionInPNL_Futures =0 ) or       
(Asset = 'FX' and @IncludeFXPNLinFX = 1 AND @IncludeCommissionInPNL_FX = 0 ) or       
(Asset = 'Equity' And IsSwapped = 1 and @IncludeFXPNLinSwaps = 1 AND @IncludeCommissionInPNL_Swaps = 0) or       
(Asset = 'FutureOption' and @IncludeFXPNLinInternationalFutOptions = 1 AND @IncludeCommissionInPNL_FutOptions = 0) or       
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeFXPNLinOther = 1 AND @IncludeCommissionInPNL_Other = 0)         
)      
Then         
0  
  
-----------------------------------------------------------------------------------------    
         
Else (TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base)            
End as TotalCommissionAndFees_Base ,            
                
      
      
      
      
                
Case                 
When (@TotalCostASZero_FX = 1) and (Asset = 'FX')      
Then 0            
When (@TotalCostASZero_Futures= 1) and (Asset = 'Future')      
Then 0       
When (@TotalCostASZero_FutOptions = 1) and (Asset = 'FutureOption')      
Then 0       
When (@TotalCostASZero_Swaps = 1) and (Asset = 'Equity' And IsSwapped = 1)                
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
Else TotalCost_Base                
End as TotalCost_Base,        
      
          
               
RealizedTradingPNLOnCost ,                 
RealizedFXPNLOnCost ,                
--TotalRealizedPNLOnCostLocal,                
  
Case   
-- Realized P&L with Commission  
When  
(  
(Asset = 'Equity'  And IsSwapped = 0  and @IncludeCommissionInPNL_Equity = 1) or     
(Asset = 'EquityOption' And @IncludeCommissionInPNL_EquityOption = 1) or     
(Asset = 'Future' And @IncludeCommissionInPNL_Futures = 1) or     
(Asset = 'FX' And @IncludeCommissionInPNL_FX =1) or     
(Asset = 'Equity' And IsSwapped = 1 And @IncludeCommissionInPNL_Swaps = 1) or     
(Asset = 'FutureOption' and @IncludeCommissionInPNL_FutOptions = 1) or     
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeCommissionInPNL_Other = 1)        
)  
Then  
TotalRealizedPNLOnCostLocal + TotalClosedCommissionAndFees_Local   
  
-- Realized P&L without Commission    
When   
(  
(Asset = 'Equity'  And IsSwapped = 0 and @IncludeCommissionInPNL_Equity = 0) or     
(Asset = 'EquityOption' And @IncludeCommissionInPNL_EquityOption = 0) or     
(Asset = 'Future' And @IncludeCommissionInPNL_Futures = 0) or     
(Asset = 'FX' And @IncludeCommissionInPNL_FX =0) or     
(Asset = 'Equity' And IsSwapped = 1 and @IncludeCommissionInPNL_Swaps = 0) or     
(Asset = 'FutureOption' and @IncludeCommissionInPNL_FutOptions = 0) or     
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and  @IncludeCommissionInPNL_Other = 0)         
)  
Then  
TotalRealizedPNLOnCostLocal              
else  
TotalRealizedPNLOnCostLocal  
End as TotalRealizedPNLOnCostLocal,        
  
           
      
         
Case        
--Realized P&L with Commission and Both FX Rate          
When       
(      
(Asset = 'Equity'  And IsSwapped = 0 and @IncludeFXPNLinEquity = 1 and @IncludeCommissionInPNL_Equity = 1) or       
(Asset = 'EquityOption' and @IncludeFXPNLinEquityOption = 1 and @IncludeCommissionInPNL_EquityOption = 1) or       
(Asset = 'Future' and @IncludeFXPNLinFutures = 1 and @IncludeCommissionInPNL_Futures = 1) or       
(Asset = 'FX' and @IncludeFXPNLinFX = 1 and @IncludeCommissionInPNL_FX =1) or       
(Asset = 'Equity' And IsSwapped = 1 and @IncludeFXPNLinSwaps = 1 and @IncludeCommissionInPNL_Swaps = 1) or       
(Asset = 'FutureOption' and @IncludeFXPNLinInternationalFutOptions = 1 and @IncludeCommissionInPNL_FutOptions = 1) or       
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeFXPNLinOther = 1 and @IncludeCommissionInPNL_Other = 1)          
)      
Then             
RealizedTradingPNLOnCost + RealizedFXPNLOnCost        
      
--Realized P&L without Commission and Both FX Rate          
When       
(      
(Asset = 'Equity'  And IsSwapped = 0 and @IncludeFXPNLinEquity = 1 and @IncludeCommissionInPNL_Equity = 0) or       
(Asset = 'EquityOption' and @IncludeFXPNLinEquityOption = 1 and @IncludeCommissionInPNL_EquityOption = 0) or       
(Asset = 'Future' and @IncludeFXPNLinFutures = 1 and @IncludeCommissionInPNL_Futures = 0) or       
(Asset = 'FX' and @IncludeFXPNLinFX = 1 and @IncludeCommissionInPNL_FX =0) or       
(Asset = 'Equity' And IsSwapped = 1 and @IncludeFXPNLinSwaps = 1 and @IncludeCommissionInPNL_Swaps = 0) or       
(Asset = 'FutureOption' and @IncludeFXPNLinInternationalFutOptions = 1 and @IncludeCommissionInPNL_FutOptions = 0) or       
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeFXPNLinOther = 1 and @IncludeCommissionInPNL_Other = 0)           
)      
Then       
   (RealizedTradingPNLOnCost + RealizedFXPNLOnCost)  + TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base                                             
--Realized P&L with Commission and Single FX Rate          
When       
(      
(Asset = 'Equity'  And IsSwapped = 0 and @IncludeFXPNLinEquity = 0 and @IncludeCommissionInPNL_Equity = 1) or       
(Asset = 'EquityOption' and @IncludeFXPNLinEquityOption = 0 and @IncludeCommissionInPNL_EquityOption = 1) or       
(Asset = 'Future' and @IncludeFXPNLinFutures = 0 and @IncludeCommissionInPNL_Futures = 1) or       
(Asset = 'FX' and @IncludeFXPNLinFX = 0 and @IncludeCommissionInPNL_FX =1) or       
(Asset = 'Equity' And IsSwapped = 1 and @IncludeFXPNLinSwaps = 0 and @IncludeCommissionInPNL_Swaps = 1) or       
(Asset = 'FutureOption' and @IncludeFXPNLinInternationalFutOptions = 0 and @IncludeCommissionInPNL_FutOptions = 1) or       
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeFXPNLinOther = 0 and @IncludeCommissionInPNL_Other = 1)          
)      
Then             
RealizedTradingPNLOnCost       
      
--Realized P&L without Commission and Single FX Rate          
When       
(      
(Asset = 'Equity'  And IsSwapped = 0 and @IncludeFXPNLinEquity = 0 and @IncludeCommissionInPNL_Equity = 0) or       
(Asset = 'EquityOption' and @IncludeFXPNLinEquityOption = 0 and @IncludeCommissionInPNL_EquityOption = 0) or       
(Asset = 'Future' and @IncludeFXPNLinFutures = 0 and @IncludeCommissionInPNL_Futures = 0) or       
(Asset = 'FX' and @IncludeFXPNLinFX = 0 and @IncludeCommissionInPNL_FX =0) or       
(Asset = 'Equity' And IsSwapped = 1 and @IncludeFXPNLinSwaps = 0 and @IncludeCommissionInPNL_Swaps = 0) or       
(Asset = 'FutureOption' and @IncludeFXPNLinInternationalFutOptions = 0 and @IncludeCommissionInPNL_FutOptions = 0) or       
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeFXPNLinOther = 0 and @IncludeCommissionInPNL_Other = 0)           
)      
Then       
RealizedTradingPNLOnCost  + TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base        
        
Else TotalRealizedPNLOnCost            
End as TotalRealizedPNLOnCost ,            
          
 -------------------------------------------------------------------------------------------------------------------------------------------------------       
             
Case        
--Short term Realized P&L for Futures with Commission and Both FX Rate       
When       
(      
(Asset = 'Future' and @IncludeFXPNLinFutures = 1 and @IncludeCommissionInPNL_Futures = 1) or  
(Asset = 'FutureOption' and @IncludeFXPNLinInternationalFutOptions = 1 and @IncludeCommissionInPNL_FutOptions = 1)             
)      
Then             
0.4 * (RealizedTradingPNLOnCost + RealizedFXPNLOnCost )      
--Short term Realized P&L for Futures without Commission and Both FX Rate      
When       
(      
(Asset = 'Future' and @IncludeFXPNLinFutures = 1 and @IncludeCommissionInPNL_Futures = 0) or  
(Asset = 'FutureOption' and @IncludeFXPNLinInternationalFutOptions = 1 and @IncludeCommissionInPNL_FutOptions = 0)          
)      
Then             
0.4 * (RealizedTradingPNLOnCost + RealizedFXPNLOnCost  + TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base)      
--Short term Realized P&L for Futures with Commission and Single FX Rate       
When       
(      
(Asset = 'Future' and @IncludeFXPNLinFutures = 0 and @IncludeCommissionInPNL_Futures = 1) or  
(Asset = 'FutureOption' and @IncludeFXPNLinInternationalFutOptions = 0 and @IncludeCommissionInPNL_FutOptions = 1)          
)      
Then             
0.4 * (RealizedTradingPNLOnCost )      
--Short term Realized P&L for Futures without Commission and Single FX Rate       
When       
(      
(Asset = 'Future' and @IncludeFXPNLinFutures = 0 and @IncludeCommissionInPNL_Futures = 0) or  
(Asset = 'FutureOption' and @IncludeFXPNLinInternationalFutOptions = 0 and @IncludeCommissionInPNL_FutOptions = 0)          
)      
Then             
0.4 *(RealizedTradingPNLOnCost  + TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base  )      
When (DateDiff(d,DateAdd(year,1,PNL.OriginalPurchaseDate),ClosingDate) <= 0 or PNL.Side = 'Short')  
Then      
CASE      
--Short term Realized P&L with Commission and Both FX Rate          
When       
(      
(Asset = 'Equity'  And IsSwapped = 0 and @IncludeFXPNLinEquity = 1 and @IncludeCommissionInPNL_Equity = 1) or       
(Asset = 'EquityOption' and @IncludeFXPNLinEquityOption = 1 and @IncludeCommissionInPNL_EquityOption = 1) or      
(Asset = 'FX' and @IncludeFXPNLinFX = 1 and @IncludeCommissionInPNL_FX =1) or       
(Asset = 'Equity' And IsSwapped = 1 and @IncludeFXPNLinSwaps = 1 and @IncludeCommissionInPNL_Swaps = 1) or       
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeFXPNLinOther = 1 and @IncludeCommissionInPNL_Other = 1)          
)      
Then            
RealizedTradingPNLOnCost + RealizedFXPNLOnCost      
--Short term Realized P&L without Commission and Both FX Rate          
When       
(      
(Asset = 'Equity'  And IsSwapped = 0 and @IncludeFXPNLinEquity = 1 and @IncludeCommissionInPNL_Equity = 0) or       
(Asset = 'EquityOption' and @IncludeFXPNLinEquityOption = 1 and @IncludeCommissionInPNL_EquityOption = 0) or       
(Asset = 'Future' and @IncludeFXPNLinFutures = 1 and @IncludeCommissionInPNL_Futures = 0) or       
(Asset = 'FX' and @IncludeFXPNLinFX = 1 and @IncludeCommissionInPNL_FX =0) or       
(Asset = 'Equity' And IsSwapped = 1 and @IncludeFXPNLinSwaps = 1 and @IncludeCommissionInPNL_Swaps = 0) or          
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeFXPNLinOther = 1 and @IncludeCommissionInPNL_Other = 0)           
)      
Then       
(RealizedTradingPNLOnCost + RealizedFXPNLOnCost)  + TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base      
--Short term Realized P&L with Commission and Single FX Rate          
When       
(      
(Asset = 'Equity'  And IsSwapped = 0 and @IncludeFXPNLinEquity = 0 and @IncludeCommissionInPNL_Equity = 1) or       
(Asset = 'EquityOption' and @IncludeFXPNLinEquityOption = 0 and @IncludeCommissionInPNL_EquityOption = 1) or       
(Asset = 'Future' and @IncludeFXPNLinFutures = 0 and @IncludeCommissionInPNL_Futures = 1) or       
(Asset = 'FX' and @IncludeFXPNLinFX = 0 and @IncludeCommissionInPNL_FX =1) or       
(Asset = 'Equity' And IsSwapped = 1 and @IncludeFXPNLinSwaps = 0 and @IncludeCommissionInPNL_Swaps = 1) or            
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeFXPNLinOther = 0 and @IncludeCommissionInPNL_Other = 1)          
)      
Then             
RealizedTradingPNLOnCost       
--Short term Realized P&L without Commission and Single FX Rate          
When       
(      
(Asset = 'Equity'  And IsSwapped = 0 and @IncludeFXPNLinEquity = 0 and @IncludeCommissionInPNL_Equity = 0) or       
(Asset = 'EquityOption' and @IncludeFXPNLinEquityOption = 0 and @IncludeCommissionInPNL_EquityOption = 0) or       
(Asset = 'Future' and @IncludeFXPNLinFutures = 0 and @IncludeCommissionInPNL_Futures = 0) or       
(Asset = 'FX' and @IncludeFXPNLinFX = 0 and @IncludeCommissionInPNL_FX =0) or       
(Asset = 'Equity' And IsSwapped = 1 and @IncludeFXPNLinSwaps = 0 and @IncludeCommissionInPNL_Swaps = 0) or          
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeFXPNLinOther = 0 and @IncludeCommissionInPNL_Other = 0)           
)      
Then       
RealizedTradingPNLOnCost  + TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base        
Else 0         
End         
Else 0         
End as ShortTermTotalRealizedPNL,           
       
  
-------------------------------------------------------------------------------------------------------------------------------------------------                
      
  
Case        
--Long term Realized P&L for Futures with Commission and Both FX Rate       
When       
(      
(Asset = 'Future' and @IncludeFXPNLinFutures = 1 and @IncludeCommissionInPNL_Futures = 1) or  
(Asset = 'FutureOption' and @IncludeFXPNLinInternationalFutOptions = 1 and @IncludeCommissionInPNL_FutOptions = 1)                  
)      
Then             
0.6 * (RealizedTradingPNLOnCost + RealizedFXPNLOnCost )      
--Long term Realized P&L for Futures without Commission and Both FX Rate      
When       
(      
(Asset = 'Future' and @IncludeFXPNLinFutures = 1 and @IncludeCommissionInPNL_Futures = 0) or  
(Asset = 'FutureOption' and @IncludeFXPNLinInternationalFutOptions = 1 and @IncludeCommissionInPNL_FutOptions = 0)                  
)      
Then             
0.6 * (RealizedTradingPNLOnCost + RealizedFXPNLOnCost  + TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base)      
--Long term Realized P&L for Futures with Commission and Single FX Rate       
When       
(      
(Asset = 'Future' and @IncludeFXPNLinFutures = 0 and @IncludeCommissionInPNL_Futures = 1) or  
(Asset = 'FutureOption' and @IncludeFXPNLinInternationalFutOptions = 0 and @IncludeCommissionInPNL_FutOptions = 1)                  
)      
Then             
0.6 * (RealizedTradingPNLOnCost )      
--Long term Realized P&L for Futures without Commission and Single FX Rate       
When       
(      
(Asset = 'Future' and @IncludeFXPNLinFutures = 0 and @IncludeCommissionInPNL_Futures = 0) or  
(Asset = 'FutureOption' and @IncludeFXPNLinInternationalFutOptions = 0 and @IncludeCommissionInPNL_FutOptions = 0)                  
)      
Then             
0.6 *(RealizedTradingPNLOnCost  + TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base  )      
When (DateDiff(d,DateAdd(year,1,PNL.OriginalPurchaseDate),ClosingDate) > 0 AND PNL.Side <> 'Short')   
Then      
CASE      
--Long term Realized P&L with Commission and Both FX Rate          
When       
(      
(Asset = 'Equity'  And IsSwapped = 0 and @IncludeFXPNLinEquity = 1 and @IncludeCommissionInPNL_Equity = 1) or       
(Asset = 'EquityOption' and @IncludeFXPNLinEquityOption = 1 and @IncludeCommissionInPNL_EquityOption = 1) or      
(Asset = 'FX' and @IncludeFXPNLinFX = 1 and @IncludeCommissionInPNL_FX =1) or       
(Asset = 'Equity' And IsSwapped = 1 and @IncludeFXPNLinSwaps = 1 and @IncludeCommissionInPNL_Swaps = 1) or          
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeFXPNLinOther = 1 and @IncludeCommissionInPNL_Other = 1)          
)      
Then            
RealizedTradingPNLOnCost + RealizedFXPNLOnCost      
--Long term Realized P&L without Commission and Both FX Rate          
When       
(      
(Asset = 'Equity'  And IsSwapped = 0 and @IncludeFXPNLinEquity = 1 and @IncludeCommissionInPNL_Equity = 0) or       
(Asset = 'EquityOption' and @IncludeFXPNLinEquityOption = 1 and @IncludeCommissionInPNL_EquityOption = 0) or       
(Asset = 'Future' and @IncludeFXPNLinFutures = 1 and @IncludeCommissionInPNL_Futures = 0) or       
(Asset = 'FX' and @IncludeFXPNLinFX = 1 and @IncludeCommissionInPNL_FX =0) or       
(Asset = 'Equity' And IsSwapped = 1 and @IncludeFXPNLinSwaps = 1 and @IncludeCommissionInPNL_Swaps = 0) or           
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeFXPNLinOther = 1 and @IncludeCommissionInPNL_Other = 0)           
)      
Then       
(RealizedTradingPNLOnCost + RealizedFXPNLOnCost)  + TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base      
--Long term Realized P&L with Commission and Single FX Rate          
When       
(      
(Asset = 'Equity'  And IsSwapped = 0 and @IncludeFXPNLinEquity = 0 and @IncludeCommissionInPNL_Equity = 1) or       
(Asset = 'EquityOption' and @IncludeFXPNLinEquityOption = 0 and @IncludeCommissionInPNL_EquityOption = 1) or       
(Asset = 'Future' and @IncludeFXPNLinFutures = 0 and @IncludeCommissionInPNL_Futures = 1) or       
(Asset = 'FX' and @IncludeFXPNLinFX = 0 and @IncludeCommissionInPNL_FX =1) or       
(Asset = 'Equity' And IsSwapped = 1 and @IncludeFXPNLinSwaps = 0 and @IncludeCommissionInPNL_Swaps = 1) or           
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeFXPNLinOther = 0 and @IncludeCommissionInPNL_Other = 1)          
)      
Then             
RealizedTradingPNLOnCost      
--Long term Realized P&L without Commission and Single FX Rate          
When       
(      
(Asset = 'Equity'  And IsSwapped = 0 and @IncludeFXPNLinEquity = 0 and @IncludeCommissionInPNL_Equity = 0) or       
(Asset = 'EquityOption' and @IncludeFXPNLinEquityOption = 0 and @IncludeCommissionInPNL_EquityOption = 0) or       
(Asset = 'Future' and @IncludeFXPNLinFutures = 0 and @IncludeCommissionInPNL_Futures = 0) or       
(Asset = 'FX' and @IncludeFXPNLinFX = 0 and @IncludeCommissionInPNL_FX =0) or       
(Asset = 'Equity' And IsSwapped = 1 and @IncludeFXPNLinSwaps = 0 and @IncludeCommissionInPNL_Swaps = 0) or           
(Asset not in ('Equity','EquityOption','Future','FutureOption','FX') and @IncludeFXPNLinOther = 0 and @IncludeCommissionInPNL_Other = 0)           
)      
Then       
RealizedTradingPNLOnCost  + TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base        
Else 0         
End         
Else 0         
End as LongTermTotalRealizedPNL,    
PNL.BaseCurrency                                 
into #RealizedPNLTable                                  
FROM T_MW_GenericPNL  PNL      
inner join T_asset A on A.Assetname = PNL.Asset      
inner join T_CompanyFunds F on F.FundName = PNL.Fund                 
                  
      
Where Open_CloseTag ='C' and datediff(d,@StartDate , Rundate)>=0 and datediff(day , Rundate,@EndDate) >= 0       
and      
A.AssetID in (Select * from #Assets)      
and      
F.CompanyFundID in (Select * from #Funds)      
      
Alter Table #RealizedPNLTable            
Add FuturePNL_Total Float Default 0,          
UnderlyingSymbolCompanyName nvarchar(200)                
          
Update  #RealizedPNLTable          
Set UnderlyingSymbolCompanyName = SM.CompanyName          
From  #RealizedPNLTable RPT          
Inner Join V_SecMasterData_WithUnderlying  SM on SM.TickerSymbol = RPT.UnderlyingSymbol             
           
            
Update #RealizedPNLTable            
Set FuturePNL_Total = 0.0            
            
If (@DivideFuturePnlIn6040 = 0)            
Begin            
Update #RealizedPNLTable            
Set FuturePNL_Total = LongTermTotalRealizedPNL + ShortTermTotalRealizedPNL             
Where Asset = 'Future'            
            
Update #RealizedPNLTable            
Set LongTermTotalRealizedPNL =             
 Case            
  When (DateDiff(Day,DateAdd(year,1,OriginalPurchaseDate),ClosingDate) > 0 AND Side <> 'Short')               
  Then FuturePNL_Total            
  Else 0.0            
 End,            
ShortTermTotalRealizedPNL =            
 Case            
  When (DateDiff(d,DateAdd(year,1,OriginalPurchaseDate),ClosingDate) <= 0 or Side = 'Short')                  
  Then FuturePNL_Total            
  Else 0.0            
 End            
Where Asset = 'Future'            
End             
          
              
If(@SearchString <> '')                                 
 Begin                               
  if (@searchby='Symbol')                      
  begin                      
  SELECT * FROM #RealizedPNLTable                      
  Inner Join #Symbol on #Symbol.items = #RealizedPNLTable.Symbol                      
  Order by symbol                      
  end                      
  else if (@searchby='underlyingSymbol')                      
  begin                      
  SELECT * FROM #RealizedPNLTable                      
  Inner Join #Symbol on #Symbol.items = #RealizedPNLTable.underlyingSymbol                      
  Order by symbol                      
  end                        
  else if (@searchby='BloombergSymbol')                      
  begin                      
  SELECT * FROM #RealizedPNLTable                      
  Inner Join #Symbol on #Symbol.items = #RealizedPNLTable.BloombergSymbol                      
  Order by symbol                    
  end                          
  else if (@searchby='SedolSymbol')                      
  begin                      
  SELECT * FROM #RealizedPNLTable                      
  Inner Join #Symbol on #Symbol.items = #RealizedPNLTable.SedolSymbol                      
  Order by symbol                      
  end                          
  else if (@searchby='OSISymbol')                      
  begin                      
  SELECT * FROM #RealizedPNLTable                      
  Inner Join #Symbol on #Symbol.items = #RealizedPNLTable.OSISymbol                      
  Order by symbol                      
  end                          
  else if (@searchby='IDCOSymbol')                      
  begin                      
  SELECT * FROM #RealizedPNLTable                      
  Inner Join #Symbol on #Symbol.items = #RealizedPNLTable.IDCOSymbol                      
  Order by symbol                      
  end                          
  else if (@searchby='ISINSymbol')                      
  begin                      
  SELECT * FROM #RealizedPNLTable                      
  Inner Join #Symbol on #Symbol.items = #RealizedPNLTable.ISINSymbol                      
  Order by symbol                      
  end                         
  else if (@searchby='CUSIPSymbol')                      
  begin                      
  SELECT * FROM #RealizedPNLTable                      
  Inner Join #Symbol on #Symbol.items = #RealizedPNLTable.CUSIPSymbol                      
  Order by symbol                      
  end                                   
 End                                  
Else                                  
 Begin                                  
  Select * from #RealizedPNLTable Order By symbol                        
 End                     
                      
Drop table #Assets,#Funds,#RealizedPNLTable,#Symbol,#TempSymbol     