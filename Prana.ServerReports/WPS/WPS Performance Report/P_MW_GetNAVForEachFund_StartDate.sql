/******************************************************************************            
Date : 2015/06/24            
Author:   PANKAJ           
Description : Gets NAV Number ( MV + Accurals) for each funds for a given date range          
 @NAVFromPMorMW : 0 for PM          
     : 1 for MW          
          
@fund :: CompanyFundId          
          
            
[P_MW_GetNAVForEachFund_StartDate_WithAccruedDividend] '2014/07/11','2014/07/21',          
'Growth and Income Fund:43001028,Income and Principal Preservation:43001030',1,'MTM_V0',1           
           
******************************************************************************/            
            
CREATE Proc [dbo].[P_MW_GetNAVForEachFund_StartDate]                              
(                 
 @StartDate datetime,                                               
 @fund varchar(max),           
 @NAVFromPMorMW int,                            
 @ReportID Varchar(100),        
 @IncludeAccured bit,      
 @AuecId int                              
)                              
AS                              
      
           
   select * INTO #FundNames            
   from dbo.Split(@fund, ',')        
        
   select         
 CF.CompanyFundId as Items        
 into   #FundIds        
   from t_companyFunds CF         
   inner join #FundNames on (CF.FundName = #FundNames.Items)        
           
   create table #FundsNAV              
   (             
    FundName varchar(200)            
   ,FundId varchar(200)            
   ,Date datetime            
   ,MarketValue float            
   ,AccruedDividend float           
   ,NAV float           
   )           
          
           
IF(@NAVFromPMorMW = 1)          
BEGIN          
      
  insert INTO #FundsNAV(FundName,FundId,Date)                
  SELECT                 
     T_CompanyFunds.FundName           
    ,#FundIds.items      
 ,@StartDate       
  from                 
  #FundIds       
  INNER JOIN T_CompanyFunds on T_CompanyFunds.CompanyFundID = #FundIds.items      
          
 Select           
 *           
 into #PNL           
 from T_MW_GenericPNL              
 Inner join  #FundsNAV           
  on  #FundsNAV.FundName = T_MW_GenericPNL.Fund          
  and (T_MW_GenericPNL.Open_CloseTag = 'O' or T_MW_GenericPNL.open_closetag = 'Accruals')      
  and DateDiff(Day,T_MW_GenericPNL.RunDate,@StartDate) = 0       
          
-- /****************************************************************************************************************************************          
-- All funds and dates stored in temp table           
-- ****************************************************************************************************************************************/          
                                                                                                                                             
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
      
--------------------------------------------------------------------ACCRUALS      
   IF @IncludeAccured = 1        
 BEGIN     
  Update #FundsNAV            
   set  AccruedDividend =  Accurals.Accural            
   FROM #FundsNAV       
   inner join            
   (            
  SELECT            
   SUM(endingmarketvaluebase) as Accural            
  ,Fund as FundName           
  from          
  #PNL  PNL       
  where PNL.open_closetag = 'Accruals'            
  GROUP BY fund      
    ) Accurals            
   ON Accurals.FundName = #FundsNAV.FundName      
 END      
      
--------------------------------------------------------------------Market value      
               
                       
 Update #FundsNAV            
 set  MarketValue =  MarketValues.MarketValue            
 FROM  #FundsNAV inner JOIN            
 (            
   select                       
    sum(                              
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
   ) as MarketValue            
   ,#FundsNAV.FundId as CompanyFundID          
   ,#FundsNAV.Date as Date              
   from                               
   #PNL PNL                              
   inner join #FundsNAV on (#FundsNAV.FundName = PNL.Fund and DATEDIFF(d,#FundsNAV.Date,PNL.rundate)=0 and PNL.open_closetag = 'o')             
   group by #FundsNAV.FundId,#FundsNAV.Date            
  ) MArketValues            
  ON MArketValues.CompanyFundID = #FundsNAV.FundId  and  #FundsNAV.Date = MArketValues.Date          
             
            
           
--------------------------------------------------------------------NAV      
          
  update #FundsNAV          
  SET NAV = isnull(MarketValue,0)+isnull(AccruedDividend,0)          
       
  drop TABLE #PNL      
END        
---------------------------------------------------------------------------data from PM        
ELSE IF(@NAVFromPMorMW = 0)          
BEGIN          
           
    insert INTO #FundsNAV          
  select           
     CF.FundName          
    ,CF.CompanyFundID          
    ,PM_NAVValue.Date          
    ,0          
    ,0          
    ,PM_NAVValue.NAVValue               
    from           
  PM_NAVValue           
  inner JOIN T_CompanyFunds CF ON CF.CompanyFundID = PM_NAVValue.FundID       
  and DATEDIFF(d,@StartDate,PM_NAVValue.Date)=0       
  inner join #FundIds on #FundIds.Items = CF.CompanyFundID          
           
END      
      
  Select           
   FundName          
  ,FundId          
  ,Date          
  ,isnull(NAV,0) as NAV          
  from          
  #FundsNAV        
          
drop TABLE #FundsNAV,#FundIds,#FundNames 