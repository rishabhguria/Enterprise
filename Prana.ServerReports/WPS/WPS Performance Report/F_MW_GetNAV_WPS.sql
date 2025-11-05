create FUNCTION [dbo].[F_MW_GetNAV_WPS] (                    
  @EndDate DATETIME                    
 ,@paramNAVbyMWorPM INT                    
 ,@entity VARCHAR(MAX)              
 ,@IsITD bit      
 ,@ReportID Varchar(100)      
 ,@IncludeAccured bit                    
 )                    
RETURNS FLOAT                    
AS                    
BEGIN                    
DECLARE @result FLOAT      
      
Declare @Funds TABLE (FUND VARCHAR(MAX))      
      
insert Into @Funds(FUND)              
select *       
from dbo.Split(@entity, ',')           
        
      
Declare @PNL TABLE(      
 [Rundate] [datetime] NULL,      
 [Symbol] [nvarchar](300) NULL,      
 [UnderlyingSymbol] [varchar](100) NULL,      
 [TradeDate] [datetime] NULL,      
 [Strategy] [varchar](100) NULL,      
 [MasterFund] [varchar](100) NULL,      
 [Fund] [varchar](100) NULL,      
 [Asset] [varchar](100) NULL,      
 [Underlyer] [varchar](100) NULL,      
 [Exchange] [varchar](100) NULL,      
 [Currency] [varchar](10) NULL,      
 [UDASector] [varchar](100) NULL,      
 [UDACountry] [varchar](100) NULL,      
 [UDASecurityType] [varchar](100) NULL,      
 [UDAAssetClass] [varchar](100) NULL,      
 [UDASubSector] [varchar](100) NULL,      
 [TradeCurrency] [varchar](10) NULL,      
 [Side] [varchar](10) NULL,      
 [SecurityName] [varchar](500) NULL,      
 [UnitCostLocal] [float] NULL,      
 [UnitCostBase] [float] NULL,      
 [ClosingPriceLocal] [float] NULL,      
 [ClosingPriceBase] [float] NULL,      
 [OpeningFXRate] [float] NULL,      
 [TradeDateFXRate] [float] NULL,      
 [BeginningFXRate] [float] NULL,      
 [EndingFXRate] [float] NULL,      
 [BeginningPriceLocal] [float] NULL,      
 [EndingPriceLocal] [float] NULL,      
 [BeginningPriceBase] [float] NULL,      
 [EndingPriceBase] [float] NULL,      
 [BeginningQuantity] [float] NULL,      
 [EndingQuantity] [float] NULL,      
 [Multiplier] [float] NULL,      
 [SideMultiplier] [varchar](5) NULL,      
 [Broker] [varchar](100) NULL,      
 [TotalOpenCommissionAndFees_Local] [float] NULL,      
 [TotalOpenCommissionAndFees_Base] [float] NULL,      
 [TotalClosedCommissionAndFees_Local] [float] NULL,      
 [TotalClosedCommissionAndFees_Base] [float] NULL,      
 [BeginningMarketValueLocal] [float] NULL,      
 [BeginningMarketValueBase] [float] NULL,      
 [BeginningMarketValue_BaseD2FX] [float] NULL,      
 [EndingMarketValueLocal] [float] NULL,      
 [EndingMarketValueBase] [float] NULL,      
 [UnrealizedTotalGainOnCostD0_Local] [float] NULL,      
 [UnrealizedTotalGainOnCostD0_Base] [float] NULL,      
 [UnrealizedTotalGainOnCostD2_Local] [float] NULL,      
 [UnrealizedTotalGainOnCostD2_Base] [float] NULL,      
 [ChangeInUnrealizedPNL_Local] [float] NULL,      
 [ChangeInUnrealizedPNL] [float] NULL,      
 [DividendLocal] [float] NOT NULL ,      
 [Dividend] [float] NULL,      
 [ClosingDate] [datetime] NULL,      
 [Open_CloseTag] [varchar](20) NULL,      
 [Delta] [float] NULL,      
 [Beta] [float] NULL,      
 [impliedVol] [float] NULL,      
 [Theta] [float] NULL,      
 [Rho] [float] NULL,      
 [Vega] [float] NULL,      
 [Gamma] [float] NULL,      
 [DeltaExposureLocal] [float] NULL,      
 [DeltaExposureBase] [float] NULL,      
 [BetaExposureLocal] [float] NULL,      
 [BetaExposureBase] [float] NULL,      
 [AverageVolume] [float] NULL,      
 [AverageLiquidation] [float] NULL,      
 [UnderlyingSymbolPrice] [float] NULL,      
 [UnrealizedTradingGainOnCostD0_Base] [float] NULL,      
 [UnrealizedFXGainOnCostD0_Base] [float] NULL,      
 [UnrealizedTradingGainOnCostD2_Base] [float] NULL,      
 [UnrealizedFXGainOnCostD2_Base] [float] NULL,      
 [TotalCost_Local] [float] NULL,      
 [TotalCost_Base] [float] NULL,      
 [TotalCost_BaseD0FX] [float] NULL,      
 [TotalCost_BaseD2FX] [float] NULL,      
 [TotalRealizedPNLOnCostLocal] [float] NULL,      
 [RealizedTradingPNLOnCost] [float] NULL,      
 [RealizedFXPNLOnCost] [float] NULL,      
 [TotalRealizedPNLOnCost] [float] NULL,      
 [AUECID] [int] NULL,      
 [PutOrCall] [varchar](10) NULL,      
 [StrikePrice] [float] NULL,      
 [ExpirationDate] [datetime] NULL,      
 [CUSIPSymbol] [varchar](50) NULL,      
 [SEDOLSymbol] [varchar](50) NULL,      
 [ISINSymbol] [varchar](50) NULL,      
 [BloombergSYmbol] [varchar](50) NULL,      
 [ReutersSYmbol] [varchar](50) NULL,      
 [IDCOSymbol] [varchar](50) NULL,      
 [OSISymbol] [varchar](50) NULL,      
 [Coupon] [float] NULL,      
 [IssueDate] [datetime] NULL,      
 [MaturityDate] [datetime] NULL,      
 [FirstCouponDate] [datetime] NULL,      
 [CouponFrequency] [varchar](20) NULL,      
 [AccrualBasis] [varchar](20) NULL,      
 [ISSwapped] [bit] NOT NULL ,      
 [NotionalValue] [float] NULL,      
 [BenchMarkRate] [float] NULL,      
 [Differential] [float] NULL,      
 [OrigCostBasis] [float] NULL,      
 [DayCount] [float] NULL,      
 [FirstResetDate] [datetime] NULL,      
 [OrigTransDate] [datetime] NULL,      
 [SwapInterestLeg_Local] [float] NULL,      
 [SwapInterestLeg_Base] [float] NULL,      
 [UnrealizedTradingPNLMTM] [float] NULL,      
 [UnrealizedFXPNLMTM] [float] NULL,      
 [RealizedTradingPNLMTM] [float] NULL,      
 [RealizedFXPNLMTM] [float] NULL,      
 [TotalRealizedPNLMTM] [float] NULL,      
 [TotalUnrealizedPNLMTM] [float] NULL,      
 [TradePNLMTMBase] [float] NOT NULL ,      
 [FxPNLMTMBase] [float] NOT NULL ,      
 [TotalUnrealizedPNLMTM_Local] [float] NULL,      
 [TotalRealizedPNLMTMLocal] [float] NULL,      
 [TotalPNLMTMLocal] [float] NULL,      
 [TotalPNLMTMBase] [float] NULL,      
 [TaxlotID] [varchar](50) NULL,      
 [TaxlotClosingID] [uniqueidentifier] NULL,      
 [StartDate] [datetime] NULL,      
 [EndDate] [datetime] NULL,      
 [DatesDiff] [int] NULL,      
 [LeveragedFactor] [float] NULL,      
 [OriginalPurchaseDate] [datetime] NULL      
,[OpenTradeAttribute1] [varchar](200) NULL      
,[OpenTradeAttribute2] [varchar](200) NULL      
,[OpenTradeAttribute3] [varchar](200) NULL      
,[OpenTradeAttribute4] [varchar](200) NULL      
,[OpenTradeAttribute5] [varchar](200) NULL      
,[OpenTradeAttribute6] [varchar](200) NULL      
,[ClosedTradeAttribute1] [varchar](200) NULL      
,[ClosedTradeAttribute2] [varchar](200) NULL      
,[ClosedTradeAttribute3] [varchar](200) NULL      
,[ClosedTradeAttribute4] [varchar](200) NULL      
,[ClosedTradeAttribute5] [varchar](200) NULL      
,[ClosedTradeAttribute6] [varchar](200) NULL      
,[TotalProceeds_Local] [float] NULL      
,[TotalProceeds_Base] [float] NULL      
,[ExchangeIdentifier] [varchar](50) NULL      
)      
      
insert into @PNL   
select   
 [Rundate],  
 [Symbol]  ,      
 [UnderlyingSymbol]  ,      
 [TradeDate]  ,      
 [Strategy]  ,      
 [MasterFund]  ,      
 [Fund]  ,      
 [Asset]  ,      
 [Underlyer]  ,      
 [Exchange]  ,      
 [Currency]  ,      
 [UDASector]  ,      
 [UDACountry]  ,      
 [UDASecurityType]  ,      
 [UDAAssetClass]  ,      
 [UDASubSector]  ,      
 [TradeCurrency]  ,      
 [Side]  ,      
 [SecurityName]  ,      
 [UnitCostLocal]  ,      
 [UnitCostBase]  ,      
 [ClosingPriceLocal]  ,      
 [ClosingPriceBase]  ,      
 [OpeningFXRate]  ,      
 [TradeDateFXRate]  ,      
 [BeginningFXRate]  ,      
 [EndingFXRate]  ,      
 [BeginningPriceLocal]  ,      
 [EndingPriceLocal]  ,      
 [BeginningPriceBase]  ,      
 [EndingPriceBase]  ,      
 [BeginningQuantity]  ,      
 [EndingQuantity]  ,      
 [Multiplier]  ,      
 [SideMultiplier] ,      
 [Broker]  ,      
 [TotalOpenCommissionAndFees_Local]  ,      
 [TotalOpenCommissionAndFees_Base]  ,      
 [TotalClosedCommissionAndFees_Local]  ,      
 [TotalClosedCommissionAndFees_Base]  ,      
 [BeginningMarketValueLocal]  ,      
 [BeginningMarketValueBase]  ,      
 [BeginningMarketValue_BaseD2FX]  ,      
 [EndingMarketValueLocal]  ,      
 [EndingMarketValueBase]  ,      
 [UnrealizedTotalGainOnCostD0_Local]  ,      
 [UnrealizedTotalGainOnCostD0_Base]  ,      
 [UnrealizedTotalGainOnCostD2_Local]  ,      
 [UnrealizedTotalGainOnCostD2_Base]  ,      
 [ChangeInUnrealizedPNL_Local]  ,      
 [ChangeInUnrealizedPNL]  ,      
 [DividendLocal]    ,      
 [Dividend]  ,      
 [ClosingDate]  ,      
 [Open_CloseTag]  ,      
 [Delta]  ,      
 [Beta]  ,      
 [impliedVol]  ,      
 [Theta]  ,      
 [Rho]  ,      
 [Vega]  ,      
 [Gamma]  ,      
 [DeltaExposureLocal]  ,      
 [DeltaExposureBase]  ,      
 [BetaExposureLocal]  ,      
 [BetaExposureBase]  ,      
 [AverageVolume]  ,      
 [AverageLiquidation]  ,      
 [UnderlyingSymbolPrice]  ,      
 [UnrealizedTradingGainOnCostD0_Base]  ,      
 [UnrealizedFXGainOnCostD0_Base]  ,      
 [UnrealizedTradingGainOnCostD2_Base]  ,      
 [UnrealizedFXGainOnCostD2_Base]  ,      
 [TotalCost_Local]  ,      
 [TotalCost_Base]  ,      
 [TotalCost_BaseD0FX]  ,      
 [TotalCost_BaseD2FX]  ,      
 [TotalRealizedPNLOnCostLocal]  ,      
 [RealizedTradingPNLOnCost]  ,      
 [RealizedFXPNLOnCost]  ,      
 [TotalRealizedPNLOnCost]  ,      
 [AUECID]  ,      
 [PutOrCall]  ,      
 [StrikePrice]  ,      
 [ExpirationDate]  ,      
 [CUSIPSymbol]  ,      
 [SEDOLSymbol]  ,      
 [ISINSymbol]  ,      
 [BloombergSYmbol]  ,      
 [ReutersSYmbol]  ,      
 [IDCOSymbol]  ,      
 [OSISymbol]  ,      
 [Coupon]  ,      
 [IssueDate]  ,      
 [MaturityDate]  ,      
 [FirstCouponDate]  ,      
 [CouponFrequency]  ,      
 [AccrualBasis]  ,      
 [ISSwapped] [bit]   ,      
 [NotionalValue]  ,      
 [BenchMarkRate]  ,      
 [Differential]  ,      
 [OrigCostBasis]  ,      
 [DayCount]  ,      
 [FirstResetDate]  ,      
 [OrigTransDate]  ,      
 [SwapInterestLeg_Local]  ,      
 [SwapInterestLeg_Base]  ,      
 [UnrealizedTradingPNLMTM]  ,      
 [UnrealizedFXPNLMTM]  ,      
 [RealizedTradingPNLMTM]  ,      
 [RealizedFXPNLMTM]  ,      
 [TotalRealizedPNLMTM]  ,      
 [TotalUnrealizedPNLMTM]  ,      
 [TradePNLMTMBase]    ,      
 [FxPNLMTMBase]    ,      
 [TotalUnrealizedPNLMTM_Local]  ,      
 [TotalRealizedPNLMTMLocal]  ,      
 [TotalPNLMTMLocal]  ,      
 [TotalPNLMTMBase]  ,      
 [TaxlotID]  ,      
 [TaxlotClosingID]  ,      
 [StartDate]  ,      
 [EndDate]  ,      
 [DatesDiff]  ,      
 [LeveragedFactor]  ,      
 [OriginalPurchaseDate]        
,[OpenTradeAttribute1]       
,[OpenTradeAttribute2]       
,[OpenTradeAttribute3]       
,[OpenTradeAttribute4]       
,[OpenTradeAttribute5]       
,[OpenTradeAttribute6]       
,[ClosedTradeAttribute1]       
,[ClosedTradeAttribute2]       
,[ClosedTradeAttribute3]       
,[ClosedTradeAttribute4]       
,[ClosedTradeAttribute5]       
,[ClosedTradeAttribute6]       
,[TotalProceeds_Local]        
,[TotalProceeds_Base]        
,[ExchangeIdentifier]     
 from T_MW_GenericPNL        
where               
 datediff(d,rundate,@EndDate)=0              
 and               
 fund in (Select * from @Funds)            
 and Open_CloseTag <> 'C'           
                       
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
Set  @BaseCurrency = (select CurrencySymbol from t_company Company                            
     left outer join T_Currency Currency                            
     on Company.BaseCurrencyID = Currency.CurrencyID)                     
                
Declare @MV float                      
                      
Select @MV = sum(                        
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
@PNL PNL                        
inner join T_Asset Asset on PNL.Asset = Asset.AssetName                        
where                         
open_closetag = 'o' AND                         
datediff(d,Rundate,@EndDate)=0 AND                        
fund in (Select * from @Funds)                         
            
Declare @AccruedDividend float                      
set  @AccruedDividend =0      
                   
IF @IncludeAccured = 1      
BEGIN      
set  @AccruedDividend =                
(                
 select                 
 sum(endingmarketvaluebase)                 
 from                 
 @PNL  PNL                
 where                 
 datediff(d,rundate,@EndDate)=0                 
 and                 
 fund in (Select * from @Funds)                 
 and               
 open_closeTag = 'Accruals'               
)          
END           
             
                      
Set @result = isnull(@AccruedDividend,0) + isnull(@MV,0)        
        
                      
      
                    
 RETURN @result                    
END 