  
/*************************************************                                                    
Author : Ankit Misra                                                   
Creation Date : 27th May , 2015                                                      
Description : Script for DashBoard Exposure part of Daily Report                                      
                                       
Execution Statement:                                                   
P_MW_DashBoardExposure_DailyReport @EndDate='7/1/2015',@Fund= '1270,1271,1298,1302,1304,1305,1306,1307,1308,1309,1310',@PTHFund = '1270'                            
*************************************************/                  
ALTER Procedure [dbo].[P_MW_DashBoardExposure_DailyReport]                                  
(                                  
 @EndDate datetime,                                  
 @Fund Varchar(max),                
 @PTHFund Varchar(Max),    
 @ReportID Varchar(100)                                  
)                                  
AS                            
                            
--Declare @EndDate datetime                                              
--Declare @Fund Varchar(2000)                
--Declare @PTHFund Varchar(Max)                                                  
--                                              
--Set @EndDate = '7/2/2015'                                          
--Set @Fund = '1309'----'1270,1271,1298,1302,1304,1305,1306,1307,1308,1309,1310'                
--Set @PTHFund='1270'         
        
--Declare @ReportID Varchar(100)        
--Set @ReportID = 'VSR_MW'        
        
CREATE TABLE #ParameterPreferences        
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
        
INSERT INTO #ParameterPreferences        
 SELECT        
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
 FROM T_ReportPreferences        
 WHERE ReportID = @ReportID        
        
        
DECLARE @FutureMV_ZeroOrEndingMVOrUnrealized int        
DECLARE @FXMV_ZeroOrEndingMVOrUnrealized int        
DECLARE @SwapMV_ZeroOrEndingMVOrUnrealized int        
DECLARE @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized int        
DECLARE @TotalCostASZero_Futures bit        
DECLARE @TotalCostASZero_FutOptions bit        
DECLARE @TotalCostASZero_FX bit        
DECLARE @TotalCostASZero_Swaps bit        
DECLARE @IncludeFXPNLinEquity bit        
DECLARE @IncludeFXPNLinEquityOption bit        
DECLARE @IncludeFXPNLinFX bit        
DECLARE @IncludeFXPNLinFutures bit        
DECLARE @IncludeFXPNLinSwaps bit        
DECLARE @IncludeFXPNLinInternationalFutOptions bit        
DECLARE @IncludeFXPNLinOther bit        
DECLARE @IncludeCommissionInPNL_Equity bit        
DECLARE @IncludeCommissionInPNL_EquityOption bit        
DECLARE @IncludeCommissionInPNL_Futures bit        
DECLARE @IncludeCommissionInPNL_FutOptions bit        
DECLARE @IncludeCommissionInPNL_Swaps bit        
DECLARE @IncludeCommissionInPNL_FX bit        
DECLARE @IncludeCommissionInPNL_Other bit        
        
SET @FutureMV_ZeroOrEndingMVOrUnrealized = (SELECT        
 FutureMV_ZeroOrEndingMVOrUnrealized        
FROM #ParameterPreferences)        
SET @FXMV_ZeroOrEndingMVOrUnrealized = (SELECT        
 FXMV_ZeroOrEndingMVOrUnrealized        
FROM #ParameterPreferences)        
SET @SwapMV_ZeroOrEndingMVOrUnrealized = (SELECT SwapMV_ZeroOrEndingMVOrUnrealized FROM #ParameterPreferences)        
SET @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = (SELECT InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized FROM #ParameterPreferences)        
SET @TotalCostASZero_Futures = (SELECT TotalCostASZero_Futures FROM #ParameterPreferences)        
SET @TotalCostASZero_FutOptions = (SELECT TotalCostASZero_FutOptions  FROM #ParameterPreferences)        
SET @TotalCostASZero_FX = (SELECT  TotalCostASZero_FX  FROM #ParameterPreferences)        
SET @TotalCostASZero_Swaps = (SELECT TotalCostASZero_Swaps FROM #ParameterPreferences)        
SET @IncludeFXPNLinEquity = (SELECT IncludeFXPNLinEquity FROM #ParameterPreferences)        
SET @IncludeFXPNLinEquityOption = (SELECT   IncludeFXPNLinEquityOption FROM #ParameterPreferences)        
SET @IncludeFXPNLinFX = (SELECT IncludeFXPNLinFX FROM #ParameterPreferences)        
SET @IncludeFXPNLinFutures = (SELECT IncludeFXPNLinFutures FROM #ParameterPreferences)        
SET @IncludeFXPNLinSwaps = (SELECT IncludeFXPNLinSwaps FROM #ParameterPreferences)        
SET @IncludeFXPNLinInternationalFutOptions = (SELECT IncludeFXPNLinInternationalFutOptions FROM #ParameterPreferences)        
SET @IncludeFXPNLinOther = (SELECT  IncludeFXPNLinOther FROM #ParameterPreferences)        
SET @IncludeCommissionInPNL_Equity = (SELECT IncludeCommissionInPNL_Equity FROM #ParameterPreferences)        
SET @IncludeCommissionInPNL_EquityOption = (SELECT IncludeCommissionInPNL_EquityOption FROM #ParameterPreferences)        
SET @IncludeCommissionInPNL_Futures = (SELECT IncludeCommissionInPNL_Futures FROM #ParameterPreferences)        
SET @IncludeCommissionInPNL_FutOptions = (SELECT IncludeCommissionInPNL_FutOptions FROM #ParameterPreferences)        
SET @IncludeCommissionInPNL_Swaps = (SELECT IncludeCommissionInPNL_Swaps FROM #ParameterPreferences)        
SET @IncludeCommissionInPNL_FX = (SELECT IncludeCommissionInPNL_FX FROM #ParameterPreferences)        
SET @IncludeCommissionInPNL_Other = (SELECT IncludeCommissionInPNL_Other FROM #ParameterPreferences)                 
                  
BEGIN                                  
                                  
Declare @DefaultAUECID int                                  
Set @DefaultAUECID=(select top 1 DefaultAUECID  from T_Company where companyId <> -1)                                  
                                  
Declare @PreviousBusinessDay DateTime                                  
Set @PreviousBusinessDay = dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID)                  
                  
------------------------------------------------------------------------------------------                                  
--Pick Selected Funds Based on their ID                                  
------------------------------------------------------------------------------------------                                  
Declare @T_FundIDs Table                                            
(                                  
 FundId int                                  
)                                  
                                  
Insert Into @T_FundIDs Select * From dbo.Split(@Fund, ',')                                                                                    
              
---- PTH Funds                
Declare @T_PTHFundIDs Table                                                                                                                                                                          
(                                                                                                                                   
 FundId int                                                                                                                                                                          
)                                                                                                                                                                          
Insert Into @T_PTHFundIDs Select * From dbo.Split(@PTHFund, ',')                  
                        
Declare @PTHFundCount Int                
Set @PTHFundCount = (Select Count(FundID) from @T_PTHFundIDs)                
                    
If ( @PTHFundCount > 0)                
Begin                
Delete From @T_FundIDs Where FundID In ( Select FUndID From @T_PTHFundIDs)                
End                 
                                                                                  
                                                                                    
CREATE TABLE #T_CompanyFunds                                                                                                                                                
(                                                                                                                     
 CompanyFundID int,                                  
 FundName varchar(50)                                  
)                                  
                                                      
Insert Into #T_CompanyFunds                                                            
Select                                                               
CompanyFundID,                                 
FundName                                                                                      
From T_CompanyFunds INNER JOIN @T_FundIDs FundIDs ON T_CompanyFunds.CompanyFundID = FundIDs.FundID                   
------------------------------------------------------------------------------------------                                  
--Pick Required Fields from T_MW_GenericPNL                                  
-----------------------------------------------------------------------------------------                  
                  
SELECT                                  
UnderlyingSymbol,                   
Sum                  
(                                  
 CASE                                  
  WHEN (DATEDIFF(d,rundate,@PreviousBusinessDay)=0)                                  
  THEN         
 CASE        
   -- When Market Value is Equal to Market Value                        
  WHEN        
  (        
    (Asset = 'Future' AND @FutureMV_ZeroOrEndingMVOrUnrealized = 1) OR ((Asset = 'FX' OR Asset = 'FXForward') AND  @FXMV_ZeroOrEndingMVOrUnrealized = 1) OR        
    (Asset = 'Equity' AND IsSwapped = 1 AND @SwapMV_ZeroOrEndingMVOrUnrealized = 1) OR        
    (Asset = 'FutureOption' AND (BaseCurrency <> TradeCurrency) AND @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 1)        
  )       
  THEN EndingMarketValueBase       
   -- When Market Value is Equal to Unrealize P&L and without Commission and with Single FX Rate              
   WHEN        
   (        
   (Asset = 'Future' AND        
   @FutureMV_ZeroOrEndingMVOrUnrealized = 2 AND        
   @IncludeCommissionInPNL_Futures = 0 AND        
   @IncludeFXPNLinFutures = 0) OR        
   ((Asset = 'FX' OR        
   Asset = 'FXForward') AND        
   @FXMV_ZeroOrEndingMVOrUnrealized = 2 AND        
   @IncludeCommissionInPNL_FX = 0 AND        
   @IncludeFXPNLinFX = 0) OR        
   (Asset = 'Equity' AND        
   IsSwapped = 1 AND        
   @SwapMV_ZeroOrEndingMVOrUnrealized = 2 AND        
   @IncludeCommissionInPNL_Swaps = 0 AND        
   @IncludeFXPNLinSwaps = 0) OR        
   (Asset = 'FutureOption' AND        
   (BaseCurrency <> TradeCurrency) AND        
   @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 AND        
   @IncludeCommissionInPNL_FutOptions = 0 AND        
   @IncludeFXPNLinInternationalFutOptions = 0)        
   ) THEN UnrealizedTradingGainOnCostD2_Base + TotalOpenCommissionAndFees_Local        
   -- When Market Value is Equal to Unrealize P&L and with Commission and with Single FX Rate              
   WHEN        
   (        
   (Asset = 'Future' AND        
   @FutureMV_ZeroOrEndingMVOrUnrealized = 2 AND        
   @IncludeCommissionInPNL_Futures = 1 AND        
   @IncludeFXPNLinFutures = 0) OR        
   ((Asset = 'FX' OR        
   Asset = 'FXForward') AND        
   @FXMV_ZeroOrEndingMVOrUnrealized = 2 AND        
   @IncludeCommissionInPNL_FX = 1 AND        
   @IncludeFXPNLinFX = 0) OR        
   (Asset = 'Equity' AND        
   IsSwapped = 1 AND        
   @SwapMV_ZeroOrEndingMVOrUnrealized = 2 AND        
   @IncludeCommissionInPNL_Swaps = 1 AND        
   @IncludeFXPNLinSwaps = 0) OR        
   (Asset = 'FutureOption' AND        
   (BaseCurrency <> TradeCurrency) AND        
   @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 AND        
   @IncludeCommissionInPNL_FutOptions = 1 AND        
   @IncludeFXPNLinInternationalFutOptions = 0)        
   ) THEN UnrealizedTradingGainOnCostD2_Base        
   -- When Market Value is Equal to Unrealize P&L and without Commission and with Both FX Rate              
   WHEN        
   (        
   (Asset = 'Future' AND        
   @FutureMV_ZeroOrEndingMVOrUnrealized = 2 AND        
   @IncludeCommissionInPNL_Futures = 0 AND        
   @IncludeFXPNLinFutures = 1) OR        
   ((Asset = 'FX' OR        
   Asset = 'FXForward') AND        
   @FXMV_ZeroOrEndingMVOrUnrealized = 2 AND        
   @IncludeCommissionInPNL_FX = 0 AND        
   @IncludeFXPNLinFX = 1) OR        
   (Asset = 'Equity' AND        
   IsSwapped = 1 AND        
   @SwapMV_ZeroOrEndingMVOrUnrealized = 2 AND        
   @IncludeCommissionInPNL_Swaps = 0 AND        
   @IncludeFXPNLinSwaps = 1) OR        
   (Asset = 'FutureOption' AND        
   (BaseCurrency <> TradeCurrency) AND        
   @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 AND        
   @IncludeCommissionInPNL_FutOptions = 0 AND        
   @IncludeFXPNLinInternationalFutOptions = 1)        
   ) THEN 
		Case 
			When Asset = 'Equity' AND IsSwapped = 1
			Then UnrealizedTotalGainOnCostD2_Base + TotalOpenCommissionAndFees_Base
			Else UnrealizedTotalGainOnCostD2_Base + (TotalOpenCommissionAndFees_Local) * EndingFXRate ---- TotalOpenCommissionAndFees_Base        
		End
   -- When Market Value is Equal to Unrealize P&L and with Commission and with Both FX Rate              
   WHEN        
   (        
   (Asset = 'Future' AND        
   @FutureMV_ZeroOrEndingMVOrUnrealized = 2 AND        
   @IncludeCommissionInPNL_Futures = 1 AND        
   @IncludeFXPNLinFutures = 1) OR        
   ((Asset = 'FX' OR        
   Asset = 'FXForward') AND        
   @FXMV_ZeroOrEndingMVOrUnrealized = 2 AND        
   @IncludeCommissionInPNL_FX = 1 AND        
   @IncludeFXPNLinFX = 1) OR        
   (Asset = 'Equity' AND        
   IsSwapped = 1 AND        
   @SwapMV_ZeroOrEndingMVOrUnrealized = 2 AND        
   @IncludeCommissionInPNL_Swaps = 1 AND        
   @IncludeFXPNLinSwaps = 1) OR        
   (Asset = 'FutureOption' AND        
   (BaseCurrency <> TradeCurrency) AND        
   @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 AND        
   @IncludeCommissionInPNL_FutOptions = 1 AND        
   @IncludeFXPNLinInternationalFutOptions = 1)        
   ) THEN UnrealizedTotalGainOnCostD2_Base      
   -- When Market Value is Equal to 0              
   WHEN        
   (        
   (Asset = 'Future' AND        
   @FutureMV_ZeroOrEndingMVOrUnrealized NOT IN (1, 2)) OR        
   ((Asset = 'FX' OR        
   Asset = 'FXForward') AND        
   @FXMV_ZeroOrEndingMVOrUnrealized NOT IN (1, 2)) OR        
   (Asset = 'Equity' AND        
   IsSwapped = 1 AND        
   @SwapMV_ZeroOrEndingMVOrUnrealized NOT IN (1, 2)) OR        
 (Asset = 'FutureOption' AND        
   (BaseCurrency <> TradeCurrency) AND        
   @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized NOT IN (1, 2))        
   ) THEN 0        
   ELSE EndingMarketValueBase       
  END                                  
  ELSE 0.0                                  
 END                   
) AS StartOfDay,                   
                     
Sum                  
(                              
 CASE                                  
  WHEN DATEDIFF(d,rundate,@EndDate)=0                                  
  THEN         
  CASE        
  -- When Market Value is Equal to Market Value                        
  WHEN        
  (        
  (Asset = 'Future' AND       
  @FutureMV_ZeroOrEndingMVOrUnrealized = 1) OR        
  ((Asset = 'FX' OR        
  Asset = 'FXForward') AND        
  @FXMV_ZeroOrEndingMVOrUnrealized = 1) OR        
  (Asset = 'Equity' AND        
  IsSwapped = 1 AND        
  @SwapMV_ZeroOrEndingMVOrUnrealized = 1) OR        
  (Asset = 'FutureOption' AND        
  (BaseCurrency <> TradeCurrency) AND        
  @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 1)        
  ) THEN EndingMarketValueBase       
  -- When Market Value is Equal to Unrealize P&L and without Commission and with Single FX Rate              
  WHEN        
  (        
  (Asset = 'Future' AND        
  @FutureMV_ZeroOrEndingMVOrUnrealized = 2 AND        
  @IncludeCommissionInPNL_Futures = 0 AND        
  @IncludeFXPNLinFutures = 0) OR        
  ((Asset = 'FX' OR        
  Asset = 'FXForward') AND        
  @FXMV_ZeroOrEndingMVOrUnrealized = 2 AND        
  @IncludeCommissionInPNL_FX = 0 AND        
  @IncludeFXPNLinFX = 0) OR        
  (Asset = 'Equity' AND        
  IsSwapped = 1 AND        
  @SwapMV_ZeroOrEndingMVOrUnrealized = 2 AND        
  @IncludeCommissionInPNL_Swaps = 0 AND        
  @IncludeFXPNLinSwaps = 0) OR        
  (Asset = 'FutureOption' AND        
  (BaseCurrency <> TradeCurrency) AND        
  @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 AND        
  @IncludeCommissionInPNL_FutOptions = 0 AND        
  @IncludeFXPNLinInternationalFutOptions = 0)        
  ) THEN UnrealizedTradingGainOnCostD2_Base + TotalOpenCommissionAndFees_Local        
  -- When Market Value is Equal to Unrealize P&L and with Commission and with Single FX Rate              
  WHEN        
  (        
  (Asset = 'Future' AND        
  @FutureMV_ZeroOrEndingMVOrUnrealized = 2 AND        
  @IncludeCommissionInPNL_Futures = 1 AND        
  @IncludeFXPNLinFutures = 0) OR        
  ((Asset = 'FX' OR        
  Asset = 'FXForward') AND        
  @FXMV_ZeroOrEndingMVOrUnrealized = 2 AND        
  @IncludeCommissionInPNL_FX = 1 AND        
  @IncludeFXPNLinFX = 0) OR        
  (Asset = 'Equity' AND        
  IsSwapped = 1 AND        
  @SwapMV_ZeroOrEndingMVOrUnrealized = 2 AND        
  @IncludeCommissionInPNL_Swaps = 1 AND        
  @IncludeFXPNLinSwaps = 0) OR        
  (Asset = 'FutureOption' AND        
  (BaseCurrency <> TradeCurrency) AND        
  @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 AND        
  @IncludeCommissionInPNL_FutOptions = 1 AND        
  @IncludeFXPNLinInternationalFutOptions = 0)        
  ) THEN UnrealizedTradingGainOnCostD2_Base       
  -- When Market Value is Equal to Unrealize P&L and without Commission and with Both FX Rate              
  WHEN        
  (        
  (Asset = 'Future' AND        
  @FutureMV_ZeroOrEndingMVOrUnrealized = 2 AND        
  @IncludeCommissionInPNL_Futures = 0 AND        
  @IncludeFXPNLinFutures = 1) OR        
  ((Asset = 'FX' OR        
  Asset = 'FXForward') AND      
  @FXMV_ZeroOrEndingMVOrUnrealized = 2 AND        
  @IncludeCommissionInPNL_FX = 0 AND        
  @IncludeFXPNLinFX = 1) OR        
  (Asset = 'Equity' AND        
  IsSwapped = 1 AND        
  @SwapMV_ZeroOrEndingMVOrUnrealized = 2 AND        
  @IncludeCommissionInPNL_Swaps = 0 AND        
  @IncludeFXPNLinSwaps = 1) OR        
  (Asset = 'FutureOption' AND        
  (BaseCurrency <> TradeCurrency) AND        
  @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 AND        
  @IncludeCommissionInPNL_FutOptions = 0 AND        
  @IncludeFXPNLinInternationalFutOptions = 1)        
  ) THEN 
	Case 
			When Asset = 'Equity' AND IsSwapped = 1
			Then UnrealizedTotalGainOnCostD2_Base + TotalOpenCommissionAndFees_Base
			Else UnrealizedTotalGainOnCostD2_Base + (TotalOpenCommissionAndFees_Local) * EndingFXRate ---- TotalOpenCommissionAndFees_Base        
		End      
  -- When Market Value is Equal to Unrealize P&L and with Commission and with Both FX Rate              
  WHEN        
  (        
  (Asset = 'Future' AND        
  @FutureMV_ZeroOrEndingMVOrUnrealized = 2 AND        
  @IncludeCommissionInPNL_Futures = 1 AND        
  @IncludeFXPNLinFutures = 1) OR        
  ((Asset = 'FX' OR        
  Asset = 'FXForward') AND        
  @FXMV_ZeroOrEndingMVOrUnrealized = 2 AND        
  @IncludeCommissionInPNL_FX = 1 AND        
  @IncludeFXPNLinFX = 1) OR        
  (Asset = 'Equity' AND        
  IsSwapped = 1 AND        
  @SwapMV_ZeroOrEndingMVOrUnrealized = 2 AND        
  @IncludeCommissionInPNL_Swaps = 1 AND        
  @IncludeFXPNLinSwaps = 1) OR        
  (Asset = 'FutureOption' AND        
  (BaseCurrency <> TradeCurrency) AND        
  @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = 2 AND        
  @IncludeCommissionInPNL_FutOptions = 1 AND        
  @IncludeFXPNLinInternationalFutOptions = 1)        
  ) THEN UnrealizedTotalGainOnCostD2_Base       
  -- When Market Value is Equal to 0              
  WHEN        
  (        
  (Asset = 'Future' AND        
  @FutureMV_ZeroOrEndingMVOrUnrealized NOT IN (1, 2)) OR        
  ((Asset = 'FX' OR        
  Asset = 'FXForward') AND        
  @FXMV_ZeroOrEndingMVOrUnrealized NOT IN (1, 2)) OR        
  (Asset = 'Equity' AND        
  IsSwapped = 1 AND        
  @SwapMV_ZeroOrEndingMVOrUnrealized NOT IN (1, 2)) OR        
  (Asset = 'FutureOption' AND        
  (BaseCurrency <> TradeCurrency) AND        
  @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized NOT IN (1, 2))        
  ) THEN 0        
  ELSE EndingMarketValueBase      
 END        
  ELSE 0.0                                  
 END                  
) AS EndOfDay,                    
                                
                  
Sum                  
(                                 
 CASE                                  
  WHEN DATEDIFF(d,rundate,@EndDate)=0 AND Asset <> 'Cash' AND Asset <> ('FX') AND Asset <> ('FXForward') AND Open_CloseTag='O'                         
  THEN ISNULL(DeltaExposureBase,0)                                  
  ELSE 0.0                                  
 END                  
) AS DeltaExposureBase,                   
                                 
Sum                  
(                               
CASE                                  
 WHEN DATEDIFF(d,rundate,@EndDate)=0 AND Asset <> 'Cash' AND Asset <> ('FX') AND Asset <> ('FXForward')  AND Open_CloseTag='O'                               
 THEN ISNULL(BetaExposureBase,0)                                  
 ELSE 0.0                                  
END                  
) AS BetaAdjustedNet                  
                  
INTO #PNL                                   
FROM T_MW_GenericPNL PNL                                  
INNER JOIN #T_CompanyFunds ON #T_CompanyFunds.FundName = PNL.Fund                                     
WHERE DATEDIFF(Day,Rundate,@EndDate)>=0 AND DATEDIFF(Day,@PreviousBusinessDay,Rundate)>=0                                  
And Open_CloseTag IN ('O','Accruals')  
Group By UnderlyingSymbol                  
                  
Alter Table #PNL                  
Add Side Varchar(10) Null                  
                  
---- Update side based on DeltaNetExposure                  
Update #PNL                  
Set Side =                   
 Case                  
  When DeltaExposureBase >= 0                  
  Then 'Long'                  
  Else 'Short'                  
 End                  
                  
                  
-----------------------------------------------------------------------------------------------                                  
--Declare Required Table and Fill with Appropriate Fields                                  
-----------------------------------------------------------------------------------------------                                  
DECLARE @T TABLE                                      
(                                      
 Item  VARCHAR(MAX),                                      
 Quantity FLOAT                              
)                            
                            
                                  
INSERT INTO @t(Item) values('Start of Day')                                  
INSERT INTO @t(Item) values('End of Day')                                  
INSERT INTO @t(Item) values('Pct. Gross*')                                  
INSERT INTO @t(Item) values('Pct. Net*')                                  
INSERT INTO @t(Item) values('Long/Short Ratio*')                                  
INSERT INTO @t(Item) values('Beta Adjusted Gross*')                                  
INSERT INTO @t(Item) values('Beta Adjusted Net*')                         
INSERT INTO @t(Item) values('Beta Long/Short Ratio')                            
--INSERT INTO @t(Item) values('Long Percent')                            
--INSERT INTO @t(Item) values('Short Percent')                            
                            
----------------------------------------------------------------------------------------------------------------------------        
-- Since End of the day NAV is used at multiple places for calculation of percentage therefore stored in a seperate variable                            
----------------------------------------------------------------------------------------------------------------------------                            
                            
Declare @EndingNAV float                            
Set  @EndingNAV = (Select NULLIF(Sum(EndOfDay),0) from #PNL)                  
                  
Declare @longDeltaExposure Float, @shortDeltaExposure Float                  
                  
Select @longDeltaExposure = Sum(DeltaExposureBase) From #PNL Where Side  = 'Long'                  
                  
Select @shortDeltaExposure = Sum(DeltaExposureBase) From #PNL Where Side  = 'Short'                   
                  
Declare @longBetaExposure Float, @shortBetaExposure Float                  
                  
Select @longBetaExposure = Sum(BetaAdjustedNet) From #PNL Where Side  = 'Long'                  
                  
Select @shortBetaExposure = Sum(BetaAdjustedNet) From #PNL Where Side  = 'Short'                             
                                  
-----------------------------------------------------------------------------------------------                                 
--Update Table With Required Calculated Fields                                  
-----------------------------------------------------------------------------------------------                                  
                                  
Update @t                                  
Set Quantity = (Select Sum(StartOfDay) from #PNL) Where Item = 'Start of Day'                                  
                                  
Update @t                                  
Set Quantity = ISNULL(@EndingNAV,0) Where Item = 'End of Day'                                
                                  
Update @t                                  
Set Quantity = (Select ISNULL(Sum(Abs(DeltaExposureBase))/@EndingNAV,0) from #PNL)*100 Where Item = 'Pct. Gross*'                                  
                                  
Update @t                                  
--Set Quantity = Abs((Select ISNULL(Sum(DeltaExposureBase)/@EndingNAV,0) from #PNL)) * 100 Where Item = 'Pct. Net*'                                  
Set Quantity = (Select ISNULL(Sum(DeltaExposureBase)/@EndingNAV,0) from #PNL) * 100 Where Item = 'Pct. Net*'                                  
                                  
Update @t                                  
Set Quantity = Abs(ISNULL(@longDeltaExposure / NULLIF(@shortDeltaExposure,0),0)) Where Item = 'Long/Short Ratio*'                                  
                           
Update @t                                  
Set Quantity = (Select ISNULL(Sum(Abs(BetaAdjustedNet)) / @EndingNAV,0) From #PNL)*100 Where Item = 'Beta Adjusted Gross*'                                  
                                  
Update @t                                  
--Set Quantity = Abs((Select ISNULL(Sum(BetaAdjustedNet)/@EndingNAV,0) From #PNL))*100 Where Item = 'Beta Adjusted Net*'                                  
Set Quantity = (Select ISNULL(Sum(BetaAdjustedNet)/@EndingNAV,0) From #PNL)*100 Where Item = 'Beta Adjusted Net*'                                  
                           
Update @t                                  
Set Quantity = Abs(ISNULL(@longBetaExposure / NULLIF(@shortBetaExposure,0),0)) Where Item = 'Beta Long/Short Ratio'                            
                            
--Update @t                                  
--Set Quantity = (Select ISNULL(Sum(@longDeltaExposure)/@EndingNAV,0)*100) Where Item = 'Long Percent'                           
--                            
--Update @t                                  
--Set Quantity = Abs(ISNULL(@shortDeltaExposure / @EndingNAV,0) * 100) Where Item = 'Short Percent'                                   
                                  
Select * from @t                    
                  
                  
Drop Table #PNL,#T_CompanyFunds,#ParameterPreferences      
End 