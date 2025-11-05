    
      
/*************************************************                      
Author : Ankit                        
Creation Date : 21st February , 2013                        
Description : Script for Portfolio composition part of Portfolio Summary Middleware Report        
         
Execution Statement:                     
  P_MW_PortfolioComposition_PSR '2012-9-25' ,'Cannon Partners Fund','MTM_V0'      
*************************************************/         
        
CREATE Procedure [dbo].[P_MW_PortfolioComposition_PSR]        
(        
@EndDate datetime,        
@fund varchar(max),        
@ReportID Varchar(100)      
)        
As        
        
Set Nocount on        
        
Select * Into #Funds                                    
from dbo.Split(@fund, ',')    
  
Create table #Temp        
(        
 Statistic varchar(max),        
 TotalMktValue float,        
 TotalPerEquity float        
)        
        
       
insert into #Temp (Statistic)values ('Cash Balances')       
insert into #Temp (Statistic)values ('Accruals')    
insert into #Temp (Statistic)values ('Long Market Value')        
insert into #Temp (Statistic)values ('Short Market Value')        
      
   
Select * into #PNL from T_MW_GenericPNL  
where         
 datediff(d,rundate,@EndDate)=0        
 and         
 fund in (Select * from #Funds)    
 and   
 Open_CloseTag <> 'C'  
       
--Create table #ParameterPreferences        
--(        
-- [FutureMV_ZeroOrEndingMVOrUnrealized] [int] NOT NULL,        
-- [FXMV_ZeroOrEndingMVOrUnrealized] [int] NOT NULL,        
-- [SwapMV_ZeroOrEndingMVOrUnrealized] [int] NOT NULL,        
-- [InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized] [int] NOT NULL,        
-- [IncludeFXPNLinEquity] [bit] NOT NULL,        
-- [IncludeFXPNLinEquityOption] [bit] NOT NULL,        
-- [IncludeFXPNLinFX] [bit] NOT NULL,        
-- [IncludeFXPNLinFutures] [bit] NOT NULL,        
-- [IncludeFXPNLinSwaps] [bit] NOT NULL,        
-- [IncludeFXPNLinInternationalFutOptions] [bit] NOT NULL,        
-- [IncludeFXPNLinOther] [bit] NOT NULL,        
-- [IncludeCommissionInPNL_Equity] [bit] NOT NULL,        
-- [IncludeCommissionInPNL_EquityOption] [bit] NOT NULL,        
-- [IncludeCommissionInPNL_Futures] [bit] NOT NULL,        
-- [IncludeCommissionInPNL_FutOptions] [bit] NOT NULL,        
-- [IncludeCommissionInPNL_Swaps] [bit] NOT NULL,        
-- [IncludeCommissionInPNL_FX] [bit] NOT NULL,        
-- [IncludeCommissionInPNL_Other] [bit] NOT NULL        
--)          
        
        
--Insert Into #ParameterPreferences                                      
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
      
--Declare @BaseCurrencyID int                                                                            
--Set @BaseCurrencyID=(select top 1 BaseCurrencyID  from T_Company)        
        
        
update #Temp set TotalMktValue=        
(        
 select SUM        
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
  End         
 )         
 from         
 #PNL PNL        
 inner join T_Asset Asset on PNL.Asset = Asset.AssetName        
 where    
 open_closetag='O'          
 and         
 Asset <> 'Cash'         
 and         
 side='Long'        
)        
where Statistic='Long Market Value'        
        
        
update #Temp set TotalMktValue=        
(        
 select SUM        
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
  ((Asset = 'FX' or Asset = 'FXForward') and @FXMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FX = 1 and @IncludeFXPNLinFX = 1) or         
  (Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_Swaps = 1 and @IncludeFXPNLinSwaps = 1) or        
  (Asset = 'FutureOption' And (@BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 and @IncludeCommissionInPNL_FutOptions = 1 and @IncludeFXPNLinInternationalFutOptions = 1)        
  )        
  Then         
  UnrealizedTotalGainOnCostD2_Base        
  -- When Market Value is Equal to 0        
  When         
  (        
  (Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized not in (1,2)) or         
  ((Asset = 'FX' or Asset = 'FXForward') and @FXMV_ZeroOrEndingMVOrUnrealized not in (1,2)) or         
  (Asset = 'Equity' And IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized not in (1,2)) or        
  (Asset = 'FutureOption' And (@BaseCurrency <> TradeCurrency) and @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized not in (1,2))        
  )        
  Then         
  0                  
  Else EndingMarketValueBase                  
  End         
 )         
 from         
 #PNL PNL        
 inner join T_Asset Asset on PNL.Asset = Asset.AssetName        
 where     
 open_closetag='O'          
 and         
 Asset <> 'Cash'         
 and         
 side='Short'        
)        
where Statistic='Short Market Value'        
        
        
    
        
update #Temp set TotalMktValue=        
(        
 select         
 sum(endingmarketvaluebase)         
 from         
 #PNL  PNL        
 --inner join T_Asset Asset on PNL.Asset = Asset.AssetName        
 where    
 Asset='Cash' and open_closeTag = 'o'       
)        
where Statistic='Cash Balances'        
      
        
       
update #Temp set TotalMktValue=        
(        
 select         
 sum(endingmarketvaluebase)         
 from         
 #PNL  PNL        
 where   
 Open_closeTag = 'Accruals'    
)        
where Statistic='Accruals'      
      
     
    
      
        
declare @TotalMktValue float        
Select @TotalMktValue = SUM(ISNULL(TotalMktValue,0)) from #Temp     
      
      
select @TotalMktValue  =       
(      
Case when (@TotalMktValue = 0)      
then  1      
else @TotalMktValue      
End      
)      
        
     
update #Temp set TotalPerEquity= Round(100*TotalMktValue/@TotalMktValue,2)        
        
select         
Statistic,         
isnull(TotalMktValue,0) as 'TotalMktValue',        
isnull(TotalPerEquity,0) as 'TotalPerEquity'         
from #Temp        
        
Drop table #Temp,#ParameterPreferences,#PNL