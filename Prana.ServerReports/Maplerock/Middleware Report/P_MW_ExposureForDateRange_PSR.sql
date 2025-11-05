/*************************************************                            
Author : Ankit Misra                            
Creation Date : 29th October , 2015                   
Description : Script for Extracting Net-Exposure and Gross Exposure For a Date Range      
Execution Statement:                           
P_MW_ExposureForDateRange_PSR  '10/01/2015','10/10/2015', 'Maple Rock MF: GS','PSR_MW'        
*************************************************/       
      
CREATE Procedure [dbo].[P_MW_ExposureForDateRange_PSR]              
(      
@StartDate Datetime,              
@EndDate Datetime,              
@Fund Varchar(max),    
@ReportID Varchar(100)           
)              
AS      
SET NOCOUNT ON      
        
--Declare      
--@StartDate Datetime,              
--@EndDate Datetime,              
--@Fund Varchar(max),    
--@ReportID Varchar(100)      
--      
--Set @StartDate = '01/01/2015'      
--Set @EndDate = '10/10/2015'      
--Set @Fund = 'Maple Rock MF: GS'    
--Set @ReportID = 'PSR_MW'     
    
Declare @LoopDate Datetime    
Set @LoopDate= @StartDate    
      
Select * Into #Funds      
From dbo.Split(@Fund, ',')  
  
Select          
 FutureMV_ZeroOrEndingMVOrUnrealized,          
 FXMV_ZeroOrEndingMVOrUnrealized,          
 SwapMV_ZeroOrEndingMVOrUnrealized,          
 InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized,          
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
    
Into #ParameterPreferences      
from T_ReportPreferences where ReportID = @ReportID          
          
          
Declare @FutureMV_ZeroOrEndingMVOrUnrealized int          
Declare @FXMV_ZeroOrEndingMVOrUnrealized int          
Declare @SwapMV_ZeroOrEndingMVOrUnrealized int          
Declare @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized int          
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
          
Declare @BaseCurrency Varchar(10)              
Set  @BaseCurrency = (select CurrencySymbol from t_company Company              
     left outer join T_Currency Currency              
     on Company.BaseCurrencyID = Currency.CurrencyID)    
    
Declare @TempDailyPerformance Float    
    
Create Table #DailyPerformance    
(    
Date Datetime,    
DailyPerformance Float    
)    
    
WHILE @LoopDate <= @EndDate    
BEGIN    
Select @TempDailyPerformance = dbo.[F_MW_GetMDReturn_Test](@LoopDate,@LoopDate,'Fund',@Fund)    
    
Insert INto #DailyPerformance    
Select     
@LoopDate,    
@TempDailyPerformance*100    
    
Set @LoopDate=DateAdd(dd,1,@LoopDate)    
END     
--      
Select      
PNL.Rundate AS Date,       
Sum(  
CASE  
WHEN PNL.Open_CloseTag = 'O' AND PNL.Asset <> 'Cash'  
THEN Abs(ISNULL(PNL.DeltaExposureBase,0))  
ELSE 0.0  
END) AS GrossExposure,      
Sum(  
CASE  
WHEN PNL.Open_CloseTag = 'O' AND PNL.Asset <> 'Cash'  
THEN ISNULL(PNL.DeltaExposureBase,0)  
ELSE 0.0  
END) AS NetExposure,  
  
  
  
SUM          
 (          
  Case           
  -- When Market Value is Equal to Market Value                    
  When           
  (          
  (Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized = 1) or           
  (Asset = 'FX' and @FXMV_ZeroOrEndingMVOrUnrealized = 1) or           
  (Asset = 'FXForward' and @FXMV_ZeroOrEndingMVOrUnrealized = 1) or           
  (Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized = 1) or          
  (Asset = 'FutureOption' And (@BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 1)          
  )          
  Then           
  PNL.EndingMarketValueLocal*PNL.EndingFXRate      
  -- When Market Value is Equal to Unrealize P&L and without Commission and with Single FX Rate             
  When           
  (          
  (Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Futures = 0 and @IncludeFXPNLinFutures = 0) or           
  (Asset = 'FX' and @FXMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FX = 0 and @IncludeFXPNLinFX = 0) or           
  (Asset = 'FXForward' and @FXMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FX = 0 and @IncludeFXPNLinFX = 0) or           
  (Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Swaps = 0 and @IncludeFXPNLinSwaps = 0) or          
  (Asset = 'FutureOption' And (@BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FutOptions = 0 and @IncludeFXPNLinInternationalFutOptions = 0)          
  )          
  Then           
  UnrealizedTradingGainOnCostD2_Base  + TotalOpenCommissionAndFees_Base          
  -- When Market Value is Equal to Unrealize P&L and with Commission and with Single FX Rate          
  When           
  (          
  (Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Futures = 1 and @IncludeFXPNLinFutures = 0) or           
  (Asset = 'FX' and @FXMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FX = 1 and @IncludeFXPNLinFX = 0) or           
  (Asset = 'FXForward' and @FXMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FX = 1 and @IncludeFXPNLinFX = 0) or           
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
  (Asset = 'FXForward' and @FXMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FX = 0 and @IncludeFXPNLinFX = 1) or           
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
  (Asset = 'FXForward' and @FXMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FX = 1 and @IncludeFXPNLinFX = 1) or           
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
  (Asset = 'FXForward' and @FXMV_ZeroOrEndingMVOrUnrealized not in (1,2)) or           
  (Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized not in (1,2)) or          
  (Asset = 'FutureOption' And (@BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized not in (1,2))          
  )          
  Then           
  0                    
  Else EndingMarketValueBase                    
  End) AS TotalMarketValue    
Into #Exposure    
From dbo.T_MW_GenericPNL PNL      
Where      
Datediff(d,@StartDate,RunDate)>=0                
And Datediff(d,Rundate,@EndDate)>=0      
And Fund in (Select * from #Funds)      
And Open_CloseTag <> 'C'     
Group By Rundate    
    
Select    
Expo.Date As Date,    
GrossExposure As GrossExposure,    
NetExposure As NetExposure,    
DailyPerformance As DailyPerformance,  
TotalMarketValue As TotalMarketValue    
From     
#Exposure Expo    
Inner Join #DailyPerformance Dp On Expo.Date=Dp.Date    
Order By Expo.Date DESC    
      
Drop Table #Funds,#DailyPerformance,#Exposure