/****************************                                                        
Author : Ankit                                                        
Creation date : Sep 19 , 2012                                                        
Description : Get Mark to Market from Middleware DB                                                        
                                                    
Modified By: Rahul Gupta                                                    
Modified Date: Nov 27,2012                                                    
Description: Configurable Parameters added.                                                    
                                                    
Parameters Description:                                                    
@FutureMV_ZeroOrEndingMVOrUnrealized : 0-Zero , 1-Full Market Value , 2- Unrealized Cost Basis PNL .                                                    
@FXMV_ZeroOrEndingMVOrUnrealized : 0-Zero , 1-Full Market Value , 2- Unrealized Cost Basis PNL .                                                  
@SwapMV_ZeroOrEndingMVOrUnrealized : 0-Zero , 1-Full Market Value , 2- Unrealized Cost Basis PNL .                                                 
@IncludeFXPNLinFX : True - Include FX gain For  FX and FXForwards  in PNL else False.                                                  
@IncludeFXPNLinFutures : True - Include FX gain For Futures  in PNL else False.                                                  
@IncludeFXPNLinSwaps : True - Include FX gain For Swaps in PNL else False.                                                     
select * from t_asset                                                     
Execution Method :                                                        
[P_MW_GetMTM_AvgPrice_CommissionAdjusted] '2014-08-1','2014-08-25',null,null,null,null,null,null, '1309,1310,1311,1312,1313,1314,1315,1316,1317,1318,1319,1320,1321','false'                                             
    
[P_MW_GetMTM_AvgPrice_CommissionAdjusted] '2014-08-1','2014-08-25',null,null,null,null,null,null, '1309,1310,1311,1312,1313,1314,1315,1316,1317,1318,1319,1320,1321','false'                                             
    
    
****************************/                                                        
                                                        
                                                        
                                      
CREATE Procedure [dbo].[P_MW_GetMTM_AvgPrice_CommissionAdjusted]                                                         
(                                                        
@StartDate datetime,                                                        
@EndDate datetime,                                                    
@FutureMV_ZeroOrEndingMVOrUnrealized int,                                                   
@FXMV_ZeroOrEndingMVOrUnrealized int,                                                    
@SwapMV_ZeroOrEndingMVOrUnrealized int,                                                    
@IncludeFXPNLinFX bit,                                                    
@IncludeFXPNLinFutures bit,                                                    
@IncludeFXPNLinSwaps bit,                                        
@FundID Varchar(1000),                              
@IncludeCommission bit                                                              
)                                                        
As             
          
--Declare @StartDate datetime                                                    
--Declare @EndDate datetime                                                
--Declare @FutureMV_ZeroOrEndingMVOrUnrealized int                                               
--Declare @FXMV_ZeroOrEndingMVOrUnrealized int                                                
--Declare @SwapMV_ZeroOrEndingMVOrUnrealized int                                                
--Declare @IncludeFXPNLinFX bit                                                
--Declare @IncludeFXPNLinFutures bit                                                
--Declare @IncludeFXPNLinSwaps bit                                    
--Declare @FundID Varchar(1000)                          
--Declare @IncludeCommission bit           
--          
--Set @Startdate='2014-08-14 00:00:00:000'          
--Set @EndDate='2014-08-15 00:00:00:000'          
--Set @FutureMV_ZeroOrEndingMVOrUnrealized=2          
--Set @FXMV_ZeroOrEndingMVOrUnrealized=2         
--Set @SwapMV_ZeroOrEndingMVOrUnrealized=2          
--Set @IncludeFXPNLinFX=1          
--Set @IncludeFXPNLinFutures=0          
--Set @IncludeFXPNLinSwaps=0          
--Set @FundID= '1309'          
--Set @IncludeCommission=0                    
                                            
IF(@FutureMV_ZeroOrEndingMVOrUnrealized not in(0,1,2) or @FutureMV_ZeroOrEndingMVOrUnrealized is null)                                            
Set @FutureMV_ZeroOrEndingMVOrUnrealized = 2                                            
                                            
IF(@FXMV_ZeroOrEndingMVOrUnrealized not in(0,1,2) or @FXMV_ZeroOrEndingMVOrUnrealized is null)                                            
Set @FXMV_ZeroOrEndingMVOrUnrealized = 2                                            
                                            
IF(@SwapMV_ZeroOrEndingMVOrUnrealized not in(0,1,2) or @SwapMV_ZeroOrEndingMVOrUnrealized is null)                                            
Set @SwapMV_ZeroOrEndingMVOrUnrealized = 2                                                         
                
        
select   BeginningPriceLocal,BloombergSymbol        
into #BeginningMarkPrice from T_mw_genericPNL         
where datediff(d,rundate,@StartDate)=0         
group by  BeginningPriceLocal,BloombergSymbol        
    
--Select Distinct (Symbol) as CashSymbol,BeginningFXRate as StartingFXRate,Rundate as Date     
--InTO #TempFXRate     
--From t_mw_genericPNL    
--Where datediff (day , @StartDate , Rundate) >= 0 and                                                      
--datediff(day , Rundate , @EndDate ) >= 0  And Asset='Cash' and open_closeTag='O'    
    
--select CashSymbol,min(Date) as Date    
--into #ValidData    
--from  #TempFXRate     
--group by CashSymbol    
  
  
        
Declare @T_Fund Table                                                                                    
(                                                                                    
 FundName Varchar(200)                                             
)                                                                                    
Insert Into @T_Fund Select FundName from T_CompanyFunds Where CompanyFundID In (Select * From dbo.Split(@FundID, ','))          
     
Declare @DefaultAUECID int                                                                    
Set @DefaultAUECID=(select DefaultAUECID from T_Company)     
  
Declare @MinTradeDate DateTime                                                                                
Set @MinTradeDate =  dbo.AdjustBusinessDays(@StartDate,-1, @DefaultAUECID)  
  
Declare @BaseCurrency Varchar(10)                                                                            
Set @BaseCurrency = (Select CurrencySymbol From T_Company  
Inner Join T_Currency On CurrencyID = BaseCurrencyID  
)                                                                 
                                                                                                              
   
                                   
Create Table #FXConversionRates                                                                                                                                    
(                                                                                                                                                                                                                    
 FromCurrencyID int,                                                                          
 ToCurrencyID int,                                                                                              
 RateValue float,                                                                                                              
 ConversionMethod int,                                                                              
 Date DateTime,                                                                     
 eSignalSymbol varchar(max)                                                                        
)          
                                                                                                                               
Insert into #FXConversionRates Exec P_GetAllFXConversionRatesForGivenDateRange @MinTradeDate,@MinTradeDate      
  
 Update #FXConversionRates                                                                                                                                                        
 Set RateValue = 1.0/RateValue                                                                              
 Where RateValue <> 0 and ConversionMethod = 1    
  
alter  Table #FXConversionRates   
add FromCurrency Varchar(10),  
ToCurrency Varchar(10)  
  
  
update #FXConversionRates  
set FromCurrency = FCurrency.CurrencySymbol,  
ToCurrency = TCurrency.CurrencySymbol  
From #FXConversionRates  
Inner Join T_Currency FCurrency On FCurrency.CurrencyId = #FXConversionRates.FromCurrencyID  
Inner Join T_Currency TCurrency On TCurrency.CurrencyId = #FXConversionRates.ToCurrencyID  
  
    
SELECT                                              
                                                      
Symbol ,                                                        
Fund as Fund,                                                       
(Asset) as Asset,                                                         
(MasterFund)  as  MasterFund,                                                        
(TradeCurrency) as TradeCurrency,                           
(SecurityName) as SecurityName,                                                        
(OpeningFXRate) as OpeningFXRate ,                                 
Strategy as Strategy ,                                                         
PutOrCall as PutOrCall ,                                
Open_closeTag,                                             
                                                  
                                                  
 (                                                  
 case when datediff(dd,Rundate,@StartDate) = 0                                                   
 then BeginningFXRate                                                   
 else 0                                                   
 end                                                  
 )                                                   
as   BeginningFXRate  ,                                              
                                                  
(                                                  
 case when Rundate = @EndDate                                                   
 then EndingFXRate                                                   
 else 0                                                   
 end                                                 
) as   EndingFXRate ,                                                         
                                                  
(UDASector) as UDASector,                                                         
(UDACountry) as UDACountry,                                        
(UDASecurityType) as UDASecurityType,                                                         
(UDAAssetClass) as UDAAssetClass,                                                        
(UDASubSector) as UDASubSector ,                                           
(CUSIPSymbol) as CUSIPSymbol,                                                        
(ISINSymbol) as ISINSymbol,                              
(SEDOLSymbol)  as  SEDOLSymbol,                                              
(T_MW_genericPNL.BloombergSYmbol) as BloombergSYmbol,                                                         
(ReutersSYmbol) as ReutersSYmbol,                                                        
(IDCOSymbol) as IDCOSymbol ,                                                         
(OSISymbol) as OSISymbol ,                                    
UnitCostLocal as UnitCostLocal,                                    
BeginningQuantity as BeginningQuantity,                                    
-- This Quantity is used for Avg Price calculations since Avg price needs Beginning Quantity for Open Positions Only        
Case                       
  when Open_closeTag = 'O' and Datediff(d,rundate,@EndDate)=0                     
  Then BeginningQuantity                      
  Else                      
  0                      
End as  BeginningQuantityProxy,                      
Side as Side,                                    
(case when datediff(dd,Rundate,@StartDate) = 0 then T_MW_GenericPNL.BeginningPriceLocal else 0 end) as   BeginningPriceLocal  ,                                                        
(case when datediff(dd,Rundate,@StartDate) = 0 then BeginningPriceBase else 0 end) as   BeginningPriceBase  ,                                                        
(case when datediff(dd,Rundate,@EndDate) = 0 then EndingPriceLocal else 0 end) as   EndingPriceLocal  ,                                                        
(case when datediff(dd,Rundate,@EndDate) = 0 then EndingPriceBase else 0 end) as   EndingPriceBase ,                                                        
(case when datediff(dd,Rundate,@StartDate) = 0 then UnrealizedTotalGainOnCostD0_Local else 0 end) as   D0UnrealizedLocal  ,                                                        
(case when datediff(dd,Rundate,@StartDate) = 0 then UnrealizedTotalGainOnCostD0_Base else 0 end) as   D0UnrealizedBase  ,                                                        
(case when datediff(dd,Rundate,@EndDate) = 0 then UnrealizedTotalGainOnCostD2_Local else 0 end) as   D2UnrealizedLocal  ,                                                        
(case when datediff(dd,Rundate,@EndDate) = 0 then UnrealizedTotalGainOnCostD2_Base else 0 end) as   D2UnrealizedBase ,                     
--                                                  
--(ChangeInUNRealizedPNL) as ChangeInUNRealizedPNL,                                                         
--(TotalRealizedPNLOnCost) as TotalRealizedPNLOnCost,                        
                
--Case                                                     
--When Asset <> 'Future'                                                    
--Then                   
-- Case                  
--  When (datediff(d,@StartDate,tradedate)>=0 and datediff(d,tradedate,@EndDate)>=0 and Open_closeTag = 'O' and datediff(d,rundate,tradedate)=0)                          
--  Then ChangeInUNRealizedPNL +TotalOpenCommissionAndFees_Base                          
--  When (datediff(d,tradedate,closingDate)<>0 and Open_closeTag = 'C')                          
--  Then ChangeInUNRealizedPNL-TotalOpenCommissionAndFees_Base                          
--  Else ChangeInUNRealizedPNL                          
-- End                
--Else               
-- Case                  
--  When (datediff(d,@StartDate,tradedate)>0 and Open_closeTag = 'O' --and datediff(d,rundate,tradedate)=0)                          
--  Then ChangeInUnrealizedPNL  + TotalOpenCommissionAndFees_local*(EndingFXRate - BeginningFxRate)           
--  When (Open_closeTag = 'O' )                          
--  Then ChangeInUnrealizedPNL  + TotalOpenCommissionAndFees_local*(EndingFXRate)                           
--  When (datediff(d,tradedate,closingDate)=0 and Open_closeTag = 'C' and datediff(d,@StartDate,tradedate)>0)                          
--  Then (ChangeInUnrealizedPNL + TotalClosedCommissionAndFees_Base) + TotalOpenCommissionAndFees_Local * (EndingFXRate - BeginningFxRate)          
--  When (datediff(d,tradedate,closingDate)<>0 and Open_closeTag = 'C')                        
--  Then (ChangeInUnrealizedPNL + TotalClosedCommissionAndFees_base  +  TotalOpenCommissionAndFees_Local*EndingFXRate)          
----  Then ChangeInUNRealizedPNL - TotalOpenCommissionAndFees_Base --- TotalClosedCommissionAndFees_Base          
--  Else (ChangeInUnrealizedPNL_Local * EndingFXRate)                          
-- End              
--End As ChangeInUNRealizedPNL,              
              
  -- Original          
              
CASE        
 When Asset='Cash'    
 then    
 Case   
  When T_MW_GenericPNL.Currency <> @BaseCurrency And DateDiff(Day,RunDate,@EndDate) = 0  
  Then EndingMarketValueLocal*(T_MW_GenericPNL.EndingFXRate - Isnull(FXDayRatesForStartDate.RateValue,0))  
  Else 0  
 End   
  
-- case  
--   when Asset ='Cash' and #FXConversionRates.ConversionMethod =1  
--   then EndingMarketValueLocal*(t_mw_genericPNL.EndingFXRate - isnull(#FXConversionRates.RateValue,0))  
--   when asset ='Cash' and #FXConversionRates.ConversionMethod =0  
--   then  
--   Case   
--    When  isnull(#FXConversionRates.RateValue,0)<>0  
--    then EndingMarketValueLocal*(t_mw_genericPNL.EndingFXRate - (1/isnull(#FXConversionRates.RateValue,0)))  
--    Else EndingMarketValueLocal*(t_mw_genericPNL.EndingFXRate - 0)  
--   End   
--   else  
--   EndingMarketValueLocal*(t_mw_genericPNL.EndingFXRate - isnull(#FXConversionRates.RateValue,0))  
-- End      
 Else    
 CASE    
  When (datediff(d,@EndDate,RunDate)=0 and Open_CloseTag='O' and datediff(d,@StartDate,tradeDate)>=0)        
  Then ((EndingPriceLocal*EndingFXRate*Multiplier*SideMultiplier)-(UnitCostLocal*Multiplier*SideMultiplier)*EndingFXRate)*BeginningQuantity        
  WHEN (datediff(d,@EndDate,RunDate)=0 and Open_CloseTag='O' and datediff(d,@StartDate,tradeDate)<0)        
  Then  ((EndingPriceLocal*EndingFXRate*Multiplier*SideMultiplier)-(MarkPrice.BeginningPriceLocal*Multiplier*SideMultiplier)*EndingFXRate)*BeginningQuantity        
  Else 0          
 END    
     
End as ChangeInUNRealizedPNL,               
              
              
----Praveen               
-- Case                  
--  When (datediff(d,@StartDate,tradedate)>=0 and datediff(d,tradedate,@EndDate)>=0 and Open_closeTag = 'O' and datediff(d,rundate,tradedate)=0)                          
--  Then  ChangeInUNRealizedPNL  + TotalOpenCommissionAndFees_Local* BeginningFXRate   - TotalOpenCommissionAndFees_Local*BeginningFXRate                
--  When (datediff(d,rundate,tradedate)=0 and Open_closeTag = 'O' )              
--  Then  ChangeInUNRealizedPNL  + TotalOpenCommissionAndFees_Local* EndingFXRate               
--  When (datediff(d,tradedate,closingDate)<>0 and Open_closeTag = 'C')                          
--  Then ChangeInUNRealizedPNL   + TotalClosedCommissionAndFees_Base - TotalOpenCommissionAndFees_Local*EndingFXRate + TotalOpenCommissionAndFees_Local*BeginningFXRate              
--  When ( Open_closeTag = 'C')              
--  THEN ChangeInUNRealizedPNL  + TotalClosedCommissionAndFees_Base + TotalOpenCommissionAndFees_Local*EndingFXRate               
--  Else ChangeInUNRealizedPNL              
--     End As ChangeInUNRealizedPNL,                     
                          
--Case                         
--when (open_CloseTag = 'C')                          
--Then TotalRealizedPNLOnCost + TotalClosedCommissionAndFees_Base+ TotalOpenCommissionAndFees_Base                         
--Else 0                         
--End as TotalRealizedPNLOnCost,           
        
CASE        
 When (datediff(d,@StartDate,tradeDate)>=0 and Open_CloseTag='C')        
 Then ((ClosingPriceBase*Multiplier*SideMultiplier)-(UnitCostLocal*Multiplier*SideMultiplier)*EndingFXRate)*EndingQuantity        
 WHEN (datediff(d,tradeDate,@StartDate)>0 and Open_CloseTag='C')        
    Then  ((ClosingPriceBase*Multiplier*SideMultiplier)-(MarkPrice.BeginningPriceLocal *Multiplier*SideMultiplier)*EndingFXRate)*EndingQuantity        
 Else 0          
End as TotalRealizedPNLOnCost,                    
                                       
(TotalRealizedPNLOnCostLocal) as TotalRealizedPNLOnCostLocal,                                                        
(TotalRealizedPNLMTM) as TotalRealizedPNLMTM,                                                        
(TotalUnrealizedPNLMTM) as TotalUnrealizedPNLMTM ,                                                         
                                                  
(case when datediff(dd,Rundate,@StartDate) = 0 then BeginningMarketValueLocal else 0 end) as   BeginningMarketValueLocal  ,                                                        
(case when datediff(dd,Rundate,@StartDate) = 0 then BeginningMarketValueBase else 0 end) as   BeginningMarketValueBase  ,                                             
(case when datediff(dd,Rundate,@EndDate) = 0 then                                         
                                                
Case                                                     
When Asset = 'Future'                                                    
Then                                                  
 Case                                                    
  When @FutureMV_ZeroOrEndingMVOrUnrealized = 1                                                    
  Then EndingMarketValueLocal                                                     
  When @FutureMV_ZeroOrEndingMVOrUnrealized = 2                                                    
  Then UnrealizedTotalGainOnCostD2_Local                                                    
  Else 0                                                    
 End                                                    
When Asset = 'FX' or Asset = 'FXForward'                                                  
Then                                                     
 Case                         
  When @FXMV_ZeroOrEndingMVOrUnrealized = 1                                                    
  Then EndingMarketValueLocal                                                     
  When @FXMV_ZeroOrEndingMVOrUnrealized = 2                                                    
  Then UnrealizedTotalGainOnCostD2_Local                                                    
  Else 0                                                    
 End                                                   
When Asset = 'Equity' And IsSwapped = 1                                                  
Then                                                     
 Case                                                     
  When @SwapMV_ZeroOrEndingMVOrUnrealized = 1                                                    
  Then EndingMarketValueLocal                                                     
  When @SwapMV_ZeroOrEndingMVOrUnrealized = 2                                                    
  Then UnrealizedTotalGainOnCostD2_Local                                                    
  Else 0                                                    
 End                                                    
Else EndingMarketValueLocal                                                 
 End                                                    
Else 0                                                    
End ) as   EndingMarketValueLocal  ,                                                 
                                                
                                                
                                                       
(                                                    
Case                                                     
When datediff(dd,Rundate,@EndDate) = 0                                                     
Then                                                          
Case                                                     
When Asset = 'Future'                                                    
Then                                                     
 Case                                                    
  When @FutureMV_ZeroOrEndingMVOrUnrealized = 1                                                    
 Then EndingMarketValueBase                                                     
  When @FutureMV_ZeroOrEndingMVOrUnrealized = 2                                                    
  Then                                                     
   Case                                                     
    When @IncludeFXPNLinFutures = 1                                                    
    Then UnrealizedTradingGainOnCostD2_Base + UnrealizedFXGainOnCostD2_Base                                                    
    Else UnrealizedTradingGainOnCostD2_Base                                                    
   End                                                    
  Else 0                                                    
 End                                                    
When Asset = 'FX' or Asset = 'FXForward'                                                   
Then                                                     
 Case                                                    
  When @FXMV_ZeroOrEndingMVOrUnrealized = 1                                                    
  Then EndingMarketValueBase                                                     
  When @FXMV_ZeroOrEndingMVOrUnrealized = 2                                                    
  Then                                                    
   Case                                                    
    When @IncludeFXPNLinFX = 1                                                    
    Then UnrealizedTradingGainOnCostD2_Base + UnrealizedFXGainOnCostD2_Base                              
    Else UnrealizedTradingGainOnCostD2_Base                                                    
   End                                                     
  Else 0                              
 End                                                    
When Asset = 'Equity' And IsSwapped = 1                                                  
Then                                                     
 Case                                                     
  When @SwapMV_ZeroOrEndingMVOrUnrealized = 1                                                    
  Then EndingMarketValueBase                                                     
  When @SwapMV_ZeroOrEndingMVOrUnrealized = 2                                                    
  Then                                                    
   Case                                                     
    When @IncludeFXPNLinSwaps = 1                                                    
    Then UnrealizedTradingGainOnCostD2_Base + UnrealizedFXGainOnCostD2_Base                                                    
    Else UnrealizedTradingGainOnCostD2_Base                                                    
   End                                                     
  Else 0                                                    
 End                                                  
Else EndingMarketValueBase                                                   
 End                                                    
Else 0                                                    
End ) as EndingMarketValueBase,                                                    
                                                   
                                                
 Case                               
 When @IncludeCommission = 1                                                       
 Then (TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend)                               
 When @IncludeCommission = 0                              
    Then (TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend)+ TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base                               
 End as TotalGainLossMTM_Method1_Cost,                                
                                                       
(TotalRealizedPNLMTM + TotalUnrealizedPNLMTM + Dividend) as TotalGainLossMTM_Method2_MTM,                                                     
(RealizedTradingPNLOnCost) as RealizedSecurityGainLoss,                                                         
(RealizedFXPNLOnCost) as ReazliedFXGainLoss,                                                         
(UnrealizedTradingGainOnCostD2_Base - UnrealizedTradingGainOnCostD0_Base) as ChangeInUNRealizedSecurityGainLoss,                                                         
(UnrealizedFXGainOnCostD2_Base - UnrealizedFXGainOnCostD0_Base ) as ChangeInUnReazliedFXGainLoss,                                                        
(Dividend) as Dividend,                                                  
                                     
                                        
(RealizedTradingPNLOnCost+ UnrealizedTradingGainOnCostD2_Base - UnrealizedTradingGainOnCostD0_Base)   as TotalSecurityGainLossMTM,                                           
(RealizedFXPNLOnCost  + UnrealizedFXGainOnCostD2_Base - UnrealizedFXGainOnCostD0_Base)  as TotalFXGainLossMTM                                           
                                                            
  
from t_mw_genericPNL  
left outer join #BeginningMarkPrice MarkPrice on  MarkPrice.BloombergSymbol = T_MW_GenericPNL.BloombergSymbol        
--left inner join #FXConversionRates on #FXConversionRates.Currency =  t_mw_genericPNL.Symbol and datediff(d,@MinTradeDate,Date)=0  
  
  -- Forex Price for Start Date                                                                            
 Left outer join #FXConversionRates FXDayRatesForStartDate                                                                                                                                        
 on (FXDayRatesForStartDate.FromCurrency = t_mw_genericPNL.Currency                                                                     
 And FXDayRatesForStartDate.ToCurrency = @BaseCurrency                                                                                                                                                             
 And DateDiff(d,@MinTradeDate,FXDayRatesForStartDate.Date)=0)  
  
                                                 
where                                                         
datediff (day , @Startdate , Rundate) >= 0 and                                                      
datediff(day , Rundate , @EndDate) >= 0                                  
and Open_CloseTag <> 'Accruals' And Fund In (Select FundName From @T_Fund)          
        
                
      
                         
                            
order by fund,symbol           
Drop table #BeginningMarkPrice,#FXConversionRates  