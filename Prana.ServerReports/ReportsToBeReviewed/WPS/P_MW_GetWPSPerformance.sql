CREATE Procedure [dbo].[P_MW_GetWPSPerformance]                  
(                                                                                      
@StartDate datetime,                                                                                  
@EndDate datetime,                                                                        
@Funds Varchar(max),                                                         
@ReportID Varchar(100),                                                                     
@SearchString Varchar(5000) ,                                                                        
@SearchBy Varchar(100),                                                  
@paramGroupByLevel1 Varchar(100),                                                  
@paramGroupByLevel2 Varchar(100),          
@IncludeAccruals bit                                                          
)                                                                                      
As                           
                          
                          
DECLARE @DEFAULTAUECID INT                          
select @DEFAULTAUECID=  DefaultAUECID from t_Company                           
      
declare @DateForBeginning datetime      
set @DateForBeginning = dbo.AdjustBusinessdays(@StartDate,-1,@DEFAULTAUECID)        
                          
--set @StartDate= dbo.AdjustBusinessdays(@StartDate,-1,@DEFAULTAUECID)                                                                         
                                                                        
Select * Into #Funds                                                                                                      
from dbo.Split(@Funds, ',')                                                          
                                                                        
Select * InTo #Symbol                                                                   
From dbo.split(@SearchString , ',')                             
                            
------Creating a temp table from T_MW_GenericPNL to get data between date range                             
------for asked funds so that it irs not required to be queried again and again                            
                            
select T_MW_GenericPNL.* into #tempT_MW_GenericPNL  from T_MW_GenericPNL                     
inner join t_companyfunds on T_MW_GenericPNL.Fund =   t_companyfunds.FundName                   
inner join #Funds on #Funds.items =   t_companyfunds.CompanyFundID                  
where datediff(d,@DateForBeginning,rundate) >=0 and datediff(d,rundate,@EndDate)>=0                       
      
select #tempT_MW_GenericPNL.* into #PrevDayT_MW_GenericPNL  from #tempT_MW_GenericPNL                  
where datediff(d,@DateForBeginning,rundate) =0       
      
delete from  #tempT_MW_GenericPNL                   
where datediff(d,@DateForBeginning,rundate) =0      
      
-----------------------------------------------------ACB-----------------------------------------------------                                        
           
declare @fNameList varchar(max)             
SELECT @fNameList= coalesce(@fNameList + ',', '') + EName.Fundname                                         
FROM (select fundname FROM t_CompanyFunds inner join #Funds on items=companyfundID) EName                                  
          
-------------------------------------------------fund wise NAV            
create table #NAVFunds                  
(                  
    FundName varchar(200)                        
   ,FundId varchar(200)                        
   ,Date datetime          
   ,NAV float                       
)                  
                  
insert into #NAVFunds           
exec P_MW_GetNAVForEachFund_StartDate           
 @StartDate =@StartDate,                                        
 @fund =@fNameList,                     
 @NAVFromPMorMW =  1 ,                                      
 @ReportID = @ReportId,                   
 @IncludeAccured = @IncludeAccruals,          
 @AuecId = 1            
        
create table #ACBGrouping                  
(                  
fundname varchar(max),                  
MasterFundName varchar(max),                  
ResultantCashEffect float            
)            
        
insert INTO #ACBGrouping            
exec [F_MW_GetACB_WPS]             
 @fromDate = @StartDate,            
 @toDate = @Enddate,            
 @funds = @fNameList,            
 @ReportId =@ReportID            
        
alter table #ACBGrouping            
add NAV float,ACB float         
    
-----------------------------------------------------update NAV in #ACBGrouping            
Update #ACBGrouping Set NAV = ISNULL(NavF.NAV,0)                                
From #ACBGrouping FData                 
inner join                                
(                                
select fundname,NAV from  #NAVFunds                  
) NavF                  
on NavF.fundname = FData.fundname             
-----------------------------------------------------update ACB in #ACBGrouping            
Update #ACBGrouping                   
Set ACB = ISNULL(ResultantCashEffect,0)+ISNULL(NAV,0)            
          
                                                                                                                                
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
                                                                                                     
Select                                                                        
 @FutureMV_ZeroOrEndingMVOrUnrealized = FutureMV_ZeroOrEndingMVOrUnrealized,                            
 @FXMV_ZeroOrEndingMVOrUnrealized = FXMV_ZeroOrEndingMVOrUnrealized,                            
 @SwapMV_ZeroOrEndingMVOrUnrealized = SwapMV_ZeroOrEndingMVOrUnrealized,                            
 @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized,                            
 @TotalCostASZero_Futures = TotalCostASZero_Futures,                            
 @TotalCostASZero_FutOptions = TotalCostASZero_FutOptions,                            
 @TotalCostASZero_FX =TotalCostASZero_FX,                            
 @TotalCostASZero_Swaps = TotalCostASZero_Swaps,                            
 @IncludeFXPNLinEquity = IncludeFXPNLinEquity,                            
 @IncludeFXPNLinEquityOption = IncludeFXPNLinEquityOption,                            
 @IncludeFXPNLinFX = IncludeFXPNLinFX,                            
 @IncludeFXPNLinFutures = IncludeFXPNLinFutures,                            
 @IncludeFXPNLinSwaps = IncludeFXPNLinSwaps,                            
 @IncludeFXPNLinInternationalFutOptions = IncludeFXPNLinInternationalFutOptions,                            
 @IncludeFXPNLinOther = IncludeFXPNLinOther,                            
 @IncludeCommissionInPNL_Equity = IncludeCommissionInPNL_Equity,                            
 @IncludeCommissionInPNL_EquityOption = IncludeCommissionInPNL_EquityOption,                            
 @IncludeCommissionInPNL_Futures = IncludeCommissionInPNL_Futures,                            
 @IncludeCommissionInPNL_FutOptions = IncludeCommissionInPNL_FutOptions,                            
 @IncludeCommissionInPNL_Swaps = IncludeCommissionInPNL_Swaps,                            
 @IncludeCommissionInPNL_FX = IncludeCommissionInPNL_FX,                            
 @IncludeCommissionInPNL_Other = IncludeCommissionInPNL_Other                            
from T_ReportPreferences where ReportID = @ReportID                                                                        
                                                           
Declare @BaseCurrency Varchar(10)                                                                            
(select TOP 1 @BaseCurrency = CurrencySymbol from t_company Company                                                                            
     left outer join T_Currency Currency                                                                            
     on Company.BaseCurrencyID = Currency.CurrencyID                                                                    
where CompanyID<>-1                                                                    
)                                                    
    
    
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
UnitCostBase,           
OpeningFXRate,                                                                                   
EndingFXRate,                                                                                     
BeginningPriceBase,                                            
EndingQuantity,                                                      
ClosingPriceBase,                                              
Open_closeTag,                          
EndingPriceBase,     
TaxlotID,                                          
    
---Ending Quantity                          
                          
CASE                                             
WHEN Asset<>'CASH' THEN                                            
 CASE                                             
  WHEN DATEDIFF(d,@EndDate,Rundate)=0  AND Open_CloseTag = 'O'                                            
  then PNL.beginningquantity                                            
  ELSE 0                                            
 END                                             
ELSE                          
 CASE                           
 WHEN  DATEDIFF(d,@EndDate,Rundate)=0  AND Open_CloseTag = 'O'                               
 then PNL.EndingMarketValueBase                          
 ELSE 0                           
 End                           
END as EQ,                           
                          
-- Ending Price                          
                          
CASE                          
WHEN PNL.Asset<> 'CASH'                          
THEN                          
CASE                           
  WHEN DATEDIFF(d,@ENDDATE,Rundate)=0  AND Open_CloseTag = 'O'                                 
  THEN                           
  PNL.EndingPriceBase                   
  WHEN DATEDIFF(d,@ENDDATE,Rundate)<0  AND Open_CloseTag = 'C'                                 
  THEN              
  PNL.ClosingPriceBase                  
  ELSE 0                           
  End                          
ELSE                           
 CASE                           
 WHEN DATEDIFF(d,@ENDDATE,Rundate)=0  AND Open_CloseTag = 'O'                          
 THEN PNL.EndingFXRate                           
 ELSe 0                          
End                          
End as EP,                          
                          
-- Ending value                          
                          
CASE                          
WHEN PNL.Asset<> 'CASH'                          
THEN                          
  CASE                           
  WHEN DATEDIFF(d,@ENDDATE,Rundate)=0  AND Open_CloseTag = 'O'                                 
  THEN                           
  PNL.EndingPriceBase*PNL.BeginningQuantity*PNL.SideMultiplier*PNL.Multiplier                          
  else 0                     
  --ELSE ClosingPriceBase*PNL.EndingQuantity*PNL.SideMultiplier*PNL.Multiplier                          
  End                          
ELSE   
 CASE                           
 WHEN DATEDIFF(d,@ENDDATE,Rundate)=0  AND Open_CloseTag = 'O'                          
 THEN PNL.EndingMarketValueBase                          
 ELSe 0                          
End                          
 End as EV,                           
                          
                                          
                                         
--CASE                                            
--WHEN Asset<>'CASH' THEN                                            
--Case                                            
-- WHEN DATEDIFF(d,@StartDate,Rundate)=0  AND Open_CloseTag = 'O'                                            
-- then PNL.EndingPriceBase                                            
--WHEN DATEDIFF(d,Tradedate,Rundate)=0 AND Open_CloseTag = 'O' then                                            
--  PNL.UnitCostBase                                            
--else 0                                            
--END                                            
--ELSE PNL.BeginningFXRate                                            
--END as BP,                                            
--                                            
                                     
--CASE                                            
-- WHEN Open_CloseTag = 'C'                                            
-- then PNL.ClosingPriceBase                                   
--WHEN datediff(d,PNL.Rundate,@Enddate)=0 then                                            
--  EndingPriceBase                                            
--else 0                                            
--END as EP,                                            
                                                          
                                                    
TotalrealizedPNLMTM,                                                      
TotalUnrealizedPNLMTM,                                                      
Dividend,                                                       
totalUNrealizedPNLMTM + totalrealizedPNLMTM + dividend as PNLContribution,                                               
                                            
                                                      
CASE                                                                          
WHEN (asset = 'CASH')                                                                                         
Then EndingMarketValueBase                                                                          
Else                                                                          
BeginningQuantity                                                                          
END AS BeginningQuantity,                                                       
                                     
Multiplier,                                            
CASE                                                                          
WHEN (asset = 'CASH')                                                                                         
Then 1                                                                           
Else                                  
SideMultiplier                                                                          
END AS SideMultiplier,                                             
                                                                                       
PutOrCall,                                                                                   
IsSwapped,                                                                                   
                      
TotalOpenCommissionAndFees_Local ,                                                                                     
TotalOpenCommissionAndFees_Base ,                                                                                
                                                                                  
                                                        
                                   
                                                                                 
Case                                                                
-- When Market Value is Equal to Market Value                                                          
When                                                                         
(                                                                        
(Asset = 'Future' and @FutureMV_ZeroOrEndingMVOrUnrealized = 1) or                                                                       (Asset = 'FX' and @FXMV_ZeroOrEndingMVOrUnrealized = 1) or                                                           
  
              
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
End as EndingMarketValueBase                        
                                                                       
                                                                     
InTo #UnrealizedPNLTable                                                                  
                                                       
FROM #tempT_MW_GenericPNL  PNL                                                                        
inner join T_asset A on A.Assetname = PNL.Asset                                                          
inner join T_CompanyFunds F on F.FundName = PNL.Fund                                                   
inner JOIN #Funds on  F.CompanyFundID=#Funds.Items                                              
                                                                              
Where                                                
datediff (day , @Startdate , Rundate) >= 0       
and datediff(day , Rundate , @EndDate) >= 0       
and Open_CloseTag <> 'Accruals'       
    
    
    
--------------------------------------------------------------------------Update BQ, BP and BV      
alter table #UnrealizedPNLTable add BQ float,BP float, BV float      
    
----------------------------------------------------Beginning Quantity      
update #UnrealizedPNLTable set BQ = subtable.BQ1      
from #UnrealizedPNLTable maintable      
left outer join      
(      
select       
 CASE                                             
  WHEN Asset<>'CASH' THEN                                            
  CASE                                             
   WHEN DATEDIFF(day,@DateForBeginning,Rundate)=0  AND Open_CloseTag = 'O'                                            
    then beginningquantity       
    ELSE 0      
  END                                             
END as BQ1 ,symbol,fund,masterfund,tradedate,TaxlotID    
from      
#PrevDayT_MW_GenericPNL       
)subtable      
on maintable.symbol=subtable.symbol       
and maintable.fund=subtable.fund       
and maintable.TaxlotID=subtable.TaxlotID    
and maintable.tradedate=subtable.tradedate       
and maintable.masterfund=subtable.masterfund      
    
update #UnrealizedPNLTable set BQ = (select EndingMarketvaluelocal  from #PrevDayT_MW_GenericPNL where Open_closetag='O' and Asset='CASH')    
where Open_closetag='O' and Asset='CASH'     
--    
update #UnrealizedPNLTable set BQ = 0      
where (Open_CloseTag = 'C' and beginningquantity <> 0) or BQ is null     
    
----------------------------------------------------Beginning Price    
update #UnrealizedPNLTable set BP = subtable.BP1      
from #UnrealizedPNLTable maintable      
left outer join      
(      
select       
 CASE                                             
  WHEN Asset<>'CASH' THEN                                            
  CASE                            
   WHEN DATEDIFF(day,@DateForBeginning,Rundate)=0  AND Open_CloseTag = 'O'                                            
    then EndingPriceBase     
    ELSE 0      
  END                                             
 ELSE                          
   EndingFXRate                          
END as BP1 ,symbol,fund,masterfund,tradedate,TaxlotID    
from      
#PrevDayT_MW_GenericPNL       
)subtable      
on maintable.symbol=subtable.symbol       
and maintable.fund=subtable.fund    
and maintable.masterfund=subtable.masterfund         
and maintable.TaxlotID=subtable.TaxlotID    
    
    
    
----------------------------------------------------Beginning Value    
update #UnrealizedPNLTable set BV = subtable.BV1      
from #UnrealizedPNLTable maintable      
left outer join      
(      
select       
 EndingMarketValueBase as BV1 ,symbol,fund,masterfund,tradedate,TaxlotID    
from      
#PrevDayT_MW_GenericPNL       
)subtable      
on maintable.symbol=subtable.symbol       
and maintable.fund=subtable.fund    
and maintable.tradedate=subtable.tradedate     
and maintable.masterfund=subtable.masterfund         
and maintable.TaxlotID=subtable.TaxlotID    
    
update #UnrealizedPNLTable set BV = (select EndingMarketValueBase from #PrevDayT_MW_GenericPNL where Open_closetag='O' and Asset='CASH')    
where Open_closetag='O' and Asset='CASH'    
      
update #UnrealizedPNLTable set BV = 0      
where (Open_CloseTag = 'C' and beginningquantity <> 0) or BV is null      
      
                                                                        
-------------------------------------------Finalizing Ending Prices              
Create table #EndingPriceAdjustment              
(                                                  
 Symbol varchar(MAX),              
 Price_On_Last_Day float              
)              
insert into #EndingPriceAdjustment              
select Symbol,              
Max(              
 CASE                          
 WHEN Asset<> 'CASH'                          
 THEN EndingPriceBase              
 ELSE EndingFXRate              
 END              
)               
from #tempT_MW_GenericPNL              
where               
 Open_closetag = 'O'               
 and               
 datediff(d,Rundate,@EndDate)=0              
group by symbol              
              
UPDATE UrPnl               
SET UrPnl.EP = EPAdj.Price_On_Last_Day              
FROM #UnrealizedPNLTable AS UrPnl               
INNER JOIN #EndingPriceAdjustment AS EPAdj               
       ON UrPnl.symbol = EPAdj.symbol               
                                                            
                                                            
                                             
                                                  
alter table   #UnrealizedPNLTable       
add NETACB float            
                
                                                  
IF @paramGroupByLevel1='MasterFund'                                                   
Begin                                                  
 update #UnrealizedPNLTable Set NETACB=A.MFACB             
 from #UnrealizedPNLTable UPNL            
  inner join             
  (            
   select sum(ACB) as MFACB,masterfundname from #ACBGrouping group by masterfundname            
  )A            
  on  UPNL.MasterFund =A.masterfundname            
End                                                  
                                                  
IF @paramGroupByLevel1='Fund'             
Begin                                                  
 update #UnrealizedPNLTable Set NETACB=A.ACB             
 from #UnrealizedPNLTable UPNL            
  inner join             
  (            
   select ACB,fundname from #ACBGrouping            
  )A            
  on  UPNL.fund =A.fundname            
ENd                                                  
                                                  
IF @paramGroupByLevel1='Symbol' OR @paramGroupByLevel1='UnderlyingSymbol'            
Begin                     
 declare @TotalACB float            
 select @TotalACB = sum(ACB) from #ACBGrouping            
            
 update #UnrealizedPNLTable Set NETACB=@TotalACB             
End                                                   
                            
--removing order by is improving the query performance                             
If(@SearchString <> '')                                                                                 
 Begin                                                                               
  if (@searchby='Symbol')                                                                      
  begin                                                                      
  SELECT * FROM #UnrealizedPNLTable                                                                      
  Inner Join #Symbol on #Symbol.items = #UnrealizedPNLTable.Symbol                                                           
  --Order by symbol                    
  end                                                                   
  else if (@searchby='underlyingSymbol')                                                                      
  begin                                                                      
  SELECT * FROM #UnrealizedPNLTable                                                                      
  Inner Join #Symbol on #Symbol.items = #UnrealizedPNLTable.underlyingSymbol                                                                      
  --Order by symbol                                                                      
 end                                                                        
  else if (@searchby='BloombergSymbol')                                                                      
  begin                                                                      
  SELECT * FROM #UnrealizedPNLTable                                                                      
  Inner Join #Symbol on #Symbol.items = #UnrealizedPNLTable.BloombergSymbol                                                                  
  --Order by symbol                                                                      
  end                                                                          
  else if (@searchby='SedolSymbol')                                                                      
  begin                                                             
  SELECT * FROM #UnrealizedPNLTable                                                  
  Inner Join #Symbol on #Symbol.items = #UnrealizedPNLTable.SedolSymbol                                                                      
  --Order by symbol                                                                      
 end                                                                          
  else if (@searchby='OSISymbol')                                                                      
  begin                                              
  SELECT * FROM #UnrealizedPNLTable                                                                      
  Inner Join #Symbol on #Symbol.items = #UnrealizedPNLTable.OSISymbol                                                                      
  --Order by symbol                                                                      
  end                                                  
  else if (@searchby='IDCOSymbol')                                                                      
  begin                                                                      
  SELECT * FROM #UnrealizedPNLTable                                                                      
  Inner Join #Symbol on #Symbol.items = #UnrealizedPNLTable.IDCOSymbol                                                                      
  --Order by symbol                                                                      
  end                                                              
  else if (@searchby='ISINSymbol')                                                      
  begin                                                                      
  SELECT * FROM #UnrealizedPNLTable                                                                      
  Inner Join #Symbol on #Symbol.items = #UnrealizedPNLTable.ISINSymbol                                                                      
  --Order by symbol                                                                      
 end                                                              
  else if (@searchby='CUSIPSymbol')                                                                      
  begin                                                                      
  SELECT * FROM #UnrealizedPNLTable                                                              
  Inner Join #Symbol on #Symbol.items = #UnrealizedPNLTable.CUSIPSymbol                                                                      
  --Order by symbol                                                  
  end                                                           
 End                                                                                  
Else                                                                                  
 Begin                                                                                  
  Select * from #UnrealizedPNLTable --Order by symbol                                                                        
 End                                              
                                          
                                                                        
Drop table #Funds,#UnrealizedPNLTable,#Symbol,#ACBGrouping