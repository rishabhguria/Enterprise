                                 
/******************************************************************                                                          
Author : Pooja Porwal                                                                                                                                                     
Creation Date: May 18,2015                                                                 
Description : Get PNL for WPS performance from Middleware DB                                                       
                            
Usage:                                                        
[P_MW_GetUnrealizedPNL] '2012-07-26','MTM_V0','1183,1182','3','','TickerSymbol'                                             
*******************************************************************/                                                          
                                                        
                                                       
CREATE Procedure [dbo].[P_MW_GetWPSPerformance]                                                          
(                                                          
@StartDate datetime,                                                      
@EndDate datetime,                                            
@Funds Varchar(max),                             
@ReportID Varchar(100),                                         
@SearchString Varchar(5000) ,                                            
@SearchBy Varchar(100),                      
@paramGroupByLevel1 Varchar(100),                      
@paramGroupByLevel2 Varchar(100)                              
)                                                          
As                                                 
                                            
Select * Into #Funds                                                                          
from dbo.Split(@Funds, ',')                              
                                            
Select * InTo #Symbol                                       
From dbo.split(@SearchString , ',')                    
                
----------------------------------------------------------Closing Mark Price of Start Date------------------------------------                                        
                         
Create Table #ClosingMarkPriceForStartDate                            
(                            
ClosingPriceAtStartDate float,                            
Symbol varchar(max)                              
)                         
                      
insert INTO #ClosingMarkPriceForStartDate (ClosingPriceAtStartDate,Symbol)                        
SELECT EndingPriceBase,Symbol from T_MW_GenericPNL where DATEDIFF(D,Rundate,@StartDate)=0                      
                      
--------------------------------------------------------Closing Mark Price of Start Date------------------------------------                         
                     
-----------------------------------------------------NET PNL/ACB For Group 1------------------------------------------------------                                     
            
select symbol into #T_distinctSymbol from T_MW_GenericPNL Where                    
datediff (day , @Startdate , Rundate) >= 0                 
and datediff(day , Rundate , @EndDate) >= 0                  
and Open_CloseTag <> 'Accruals'                  
group BY symbol              
            
            
select underlyingSymbol into #T_distinctUnderlyingSymbol from T_MW_GenericPNL Where                    
datediff (day , @Startdate , Rundate) >= 0                 
and datediff(day , Rundate , @EndDate) >= 0                  
and Open_CloseTag <> 'Accruals'                  
group BY UnderlyingSymbol              
             
           
declare @fNameList varchar(max)            
            
SELECT @fNameList= coalesce(@fNameList + ',', '') + EName.Fundname             
FROM (select fundname FROM t_CompanyFunds inner join #Funds on items=companyfundID) EName            
            
declare @Fromdate datetime            
declare @Todate datetime            
            
DECLARE @NameList varchar(max)            
            
select @Fromdate=fromdate,@Todate=TODATE from dbo.F_MW_DatesWPS(@StartDate,@Enddate)            
            
Create table #ACBGrouping1                      
(                      
 Groupinglevel varchar(MAX),            
    PNL Float,            
 BMVSecurities float,            
    BMVcash float,            
BMVcashFlow float,            
 BMVSum float,            
 result Float               
)                       
                      
IF @paramGroupByLevel1='MasterFund'             
begin            
SELECT @NameList= coalesce(@NameList + ',', '') + EName.MasterFundname             
FROM (select MasterFundname FROM T_CompanyMasterFunds ) EName            
                      
insert INTO #ACBGrouping1                      
 exec F_MW_GetMDReturnNOF_WPS @Fromdate,@Todate,@fNameList,@paramGroupByLevel1,@NameList                 
end              
                 
IF @paramGroupByLevel1='Fund'             
begin            
SELECT @NameList= coalesce(@NameList + ',', '') + EName.fundname             
FROM (select fundname FROM t_CompanyFunds inner join #Funds on items=companyfundID) EName            
print @NameList            
                    
insert INTO #ACBGrouping1                      
 exec F_MW_GetMDReturnNOF_WPS @Fromdate,@Todate,@fNameList,@paramGroupByLevel1,@NameList                 
end                      
                      
IF @paramGroupByLevel1='Symbol'                      
begin            
SELECT @NameList= coalesce(@NameList + ',', '') + EName.Symbol             
FROM (select Symbol FROM #T_distinctSymbol) EName            
                      
insert INTO #ACBGrouping1                      
 exec F_MW_GetMDReturnNOF_WPS @Fromdate,@Todate,@fNameList,@paramGroupByLevel1,@NameList                 
end                     
                      
IF @paramGroupByLevel1='UnderlyingSymbol'                      
begin            
SELECT @NameList= coalesce(@NameList + ',', '') + EName.UnderlyingSymbol             
FROM (select UnderlyingSymbol FROM #T_distinctUnderlyingSymbol) EName            
                      
insert INTO #ACBGrouping1                      
 exec F_MW_GetMDReturnNOF_WPS @Fromdate,@Todate,@fNameList,@paramGroupByLevel1,@NameList                 
end                       
                      
                       
-----------------------------------------------------NET PNL/ACB For Group 1------------------------------------------------------                                     
                                  
                     
-----------------------------------------------------NET PNL/ACB For Group 2------------------------------------------------------                                     
            
Create table #ACBGrouping2                      
(                      
 Groupinglevel varchar(MAX),            
    PNL Float,            
 BMVSecurities float,            
    BMVcash float,            
 BMVcashFlow float,            
 BMVSum float,            
 result Float               
)                       
                      
declare @List2 varchar(MAX)                    
IF @paramGroupByLevel2='Symbol'                      
begin            
SELECT @List2= coalesce(@List2 + ',', '') + EName.Symbol             
FROM (select Symbol FROM #T_distinctSymbol) EName            
                      
insert INTO #ACBGrouping2                      
 exec F_MW_GetMDReturnNOF_WPS @Fromdate,@Todate,@fNameList,@paramGroupByLevel2,@List2                 
end               
            
                      
IF @paramGroupByLevel2='UnderlyingSymbol'                      
begin            
SELECT @List2= coalesce(@List2 + ',', '') + EName.UnderlyingSymbol           
FROM (select UnderlyingSymbol FROM #T_distinctUnderlyingSymbol) EName            
                      
insert INTO #ACBGrouping2                      
 exec F_MW_GetMDReturnNOF_WPS @Fromdate,@Todate,@fNameList,@paramGroupByLevel2,@List2                 
end                  
-----------------------------------------------------NET PNL/ACB For Group 2------------------------------------------------------                                     
  Create table #ParameterPreferences     
(                                            
 [FutureMV_ZeroOrEndingMVOrUnrealized] [int] NOT NULL,                                
 [FXMV_ZeroOrEndingMVOrUnrealized] [int] NOT NULL,                                            
 [SwapMV_ZeroOrEndingMVOrUnrealized] [int] NOT NULL,                         [InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized] [int] NOT NULL,                                            
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
                            
                      
                      
Declare @BaseCurrency Varchar(10)                                                
(select TOP 1 @BaseCurrency = CurrencySymbol from t_company Company                                                
     left outer join T_Currency Currency                                                
     on Company.BaseCurrencyID = Currency.CurrencyID                                        
where CompanyID<>-1                                        
)                        
                      
                      
                                              
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
UnitCostBase,                                                      
OpeningFXRate,                                                       
EndingFXRate,                                                         
EndingPriceLocal,                 
BeginningPriceLocal,                
BeginningPriceBase,                
EndingQuantity,                          
ClosingPriceLocal,                         
ClosingPriceBase,                  
Open_closeTag,               
    
case              
When datediff(d,PNL.Rundate,@Enddate)=0              
Then EndingPriceBase              
Else 0 End as EndingPriceBase,                 
                
CASE                 
WHEN Asset<>'CASH' THEN                
CASE                 
 WHEN (DATEDIFF(d,@StartDate,Rundate)=0 or DATEDIFF(d,Tradedate,Rundate)=0) AND Open_CloseTag = 'O'                
 then beginningquantity                
 ELSE 0                
END                 
ELSE PNL.EndingMarketValueLocal                
END as BQ,                
             
CASE                
WHEN Asset<>'CASH' THEN                
Case                
 WHEN DATEDIFF(d,@StartDate,Rundate)=0  AND Open_CloseTag = 'O'                
 then PNL.EndingPriceBase                
WHEN DATEDIFF(d,Tradedate,Rundate)=0 AND Open_CloseTag = 'O' then                
  PNL.UnitCostBase                
else 0                
END                
ELSE PNL.BeginningFXRate                
END as BP,                
                
CASE                
 WHEN Open_CloseTag = 'C'                
 then Endingquantity                
 ELSE 0                
END as EQ,                
                    
CASE                
 WHEN Open_CloseTag = 'C'                
 then PNL.ClosingPriceBase                
WHEN datediff(d,PNL.Rundate,@Enddate)=0 then                
  EndingPriceBase                
else 0                
END as EP,                
                
--EQ*EP as EV,                
                
                
                 
--CASE                          
--WHEN  datediff(d,@StartDate,TradeDate)>=0                        
--Then UnitCostBase                          
--Else                 
--CMPSD.ClosingPriceAtStartDate                           
--END as BeginningPriceWPS,                        
                
                
                  
--CASE                    
--When Open_closeTag='C'                   
--THEN PNL.ClosingPriceBase                  
--ELSE EPED.EndingPriceAtEndDate end as EndingPriceWPS,                  
                        
                        
TotalrealizedPNLMTM,                          
TotalUnrealizedPNLMTM,                          
Dividend,                           
(totalUNrealizedPNLMTM + totalrealizedPNLMTM + dividend) as PNLContribution,                   
                
                       
CASE                       
WHEN (Asset='CASH')                      
THEN BeginningMarketValueBase                      
WHEN (Asset<>'CASH') and datediff(d,@StartDate,TradeDate)>=0                     
THEN BeginningQuantity*Multiplier*SideMultiplier*UnitCostBase                      
ELSE CMPSD.ClosingPriceAtStartDate*BeginningQuantity*Multiplier*SideMultiplier                      
End as BeginningValue,                      
                          
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
End as EndingMarketValueBase                        
                                           
                                         
InTo #UnrealizedPNLTable                                      
                                                        
FROM T_MW_GenericPNL  PNL                                            
inner join T_asset A on A.Assetname = PNL.Asset                              
inner join T_CompanyFunds F on F.FundName = PNL.Fund                       
left JOIN #ClosingMarkPriceForStartDate CMPSD  on PNL.Symbol=CMPSD.Symbol                
inner JOIN #Funds on  F.CompanyFundID=#Funds.Items                  
                                                  
Where                    
datediff (day , @Startdate , Rundate) >= 0                 
and datediff(day , Rundate , @EndDate) >= 0                  
and Open_CloseTag <> 'Accruals'                  
                                      
                                            
                       
                                
                                
                       
                      
alter table   #UnrealizedPNLTable                          
add NETACBPNLBasedOnGrouping1 float,                
 GrossACBPNLBasedOnGrouping1 float,            
 NETACBPNLBasedOnGrouping2 float,                            
 GrossACBPNLBasedOnGrouping2 float                      
                      
                      
IF @paramGroupByLevel1='MasterFund'                       
Begin                      
update #UnrealizedPNLTable          
Set NETACBPNLBasedOnGrouping1=ACB.result               
from #ACBGrouping1 Acb inner join #UnrealizedPNLTable Up on  ACB.GroupingLevel =Up.MasterFund                      
End                      
                      
IF @paramGroupByLevel1='Fund' Begin                      
update #UnrealizedPNLTable                  
Set NETACBPNLBasedOnGrouping1=ACB.result              
from #ACBGrouping1 Acb inner join #UnrealizedPNLTable Up on  ACB.GroupingLevel =Up.Fund                      
ENd                      
                      
IF @paramGroupByLevel1='Symbol' Begin                      
update #UnrealizedPNLTable                      
Set NETACBPNLBasedOnGrouping1=ACB.result              
from #ACBGrouping1 Acb inner join #UnrealizedPNLTable Up on  ACB.GroupingLevel =Up.Symbol                      
End                      
                      
IF @paramGroupByLevel1='UnderlyingSymbol' Begin                      
update #UnrealizedPNLTable                      
Set NETACBPNLBasedOnGrouping1=ACB.result              
from #ACBGrouping1 Acb inner join #UnrealizedPNLTable Up on  ACB.GroupingLevel =Up.UnderlyingSymbol                      
End             
            
            
IF @paramGroupByLevel2='Symbol' Begin                      
update #UnrealizedPNLTable                      
Set NETACBPNLBasedOnGrouping2=ACB.result              
from #ACBGrouping2 Acb inner join #UnrealizedPNLTable Up on  ACB.GroupingLevel =Up.Symbol                      
End                      
                      
IF @paramGroupByLevel2='UnderlyingSymbol' Begin                      
update #UnrealizedPNLTable                      
Set NETACBPNLBasedOnGrouping2=ACB.result              
from #ACBGrouping2 Acb inner join #UnrealizedPNLTable Up on  ACB.GroupingLevel =Up.UnderlyingSymbol                      
End                       
        
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
              
                                            
Drop table #Funds,#UnrealizedPNLTable,#Symbol,        
#ACBGrouping1,#ClosingMarkPriceForStartDate,#ParameterPreferences        
,#T_distinctSymbol,#T_distinctUnderlyingSymbol,#ACBGrouping2