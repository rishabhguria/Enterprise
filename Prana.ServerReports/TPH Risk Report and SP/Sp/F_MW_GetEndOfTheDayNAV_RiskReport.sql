/*************************************************                                                  
Author : Sachin Mishra                                                 
Creation Date : 01 Oct, 2015                                                    
Description : Script for Calculating End Of The Day NAV     
                                     
Execution Statement:                                                 
         
Select DBO.[F_MW_GetEndOfTheDayNAV_RiskReport] ('10/13/2015','1,2,3,4,5,6,7','MTM_V0')          
      
*************************************************/                                
CREATE Function [dbo].[F_MW_GetEndOfTheDayNAV_RiskReport]                                
(                                
 @EndDate datetime,                                
 @Fund Varchar(max),       
 @ReportID Varchar(100)                              
)                                
          
RETURNS Float              
AS                           
--Declare @EndDate datetime                                
--Declare @Fund Varchar(max)                  
                     
--Set @EndDate ='2015-04-02'                          
--Set @Fund = '1245,1213,1214,1238,1239,1240,1241,1242,1244,1243,1246'            
          
                         
                                
BEGIN             
          
Declare @NAV float           
--Declare @ReportID Varchar(100)                  
--Set @ReportID = 'VSR_MW'               
                 
                  
Declare @ParameterPreferences TABLE                  
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
                  
INSERT INTO @ParameterPreferences                  
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
FROM @ParameterPreferences)                  
SET @FXMV_ZeroOrEndingMVOrUnrealized = (SELECT                  
 FXMV_ZeroOrEndingMVOrUnrealized                  
FROM @ParameterPreferences)                  
SET @SwapMV_ZeroOrEndingMVOrUnrealized = (SELECT SwapMV_ZeroOrEndingMVOrUnrealized FROM @ParameterPreferences)                  
SET @InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized = (SELECT InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized FROM @ParameterPreferences)                  
SET @TotalCostASZero_Futures = (SELECT TotalCostASZero_Futures FROM @ParameterPreferences)                  
SET @TotalCostASZero_FutOptions = (SELECT TotalCostASZero_FutOptions  FROM @ParameterPreferences)                  
SET @TotalCostASZero_FX = (SELECT  TotalCostASZero_FX  FROM @ParameterPreferences)                  
SET @TotalCostASZero_Swaps = (SELECT TotalCostASZero_Swaps FROM @ParameterPreferences)                  
SET @IncludeFXPNLinEquity = (SELECT IncludeFXPNLinEquity FROM @ParameterPreferences)                  
SET @IncludeFXPNLinEquityOption = (SELECT   IncludeFXPNLinEquityOption FROM @ParameterPreferences)                  
SET @IncludeFXPNLinFX = (SELECT IncludeFXPNLinFX FROM @ParameterPreferences)                  
SET @IncludeFXPNLinFutures = (SELECT IncludeFXPNLinFutures FROM @ParameterPreferences)                  
SET @IncludeFXPNLinSwaps = (SELECT IncludeFXPNLinSwaps FROM @ParameterPreferences)                  
SET @IncludeFXPNLinInternationalFutOptions = (SELECT IncludeFXPNLinInternationalFutOptions FROM @ParameterPreferences)                  
SET @IncludeFXPNLinOther = (SELECT  IncludeFXPNLinOther FROM @ParameterPreferences)                  
SET @IncludeCommissionInPNL_Equity = (SELECT IncludeCommissionInPNL_Equity FROM @ParameterPreferences)                  
SET @IncludeCommissionInPNL_EquityOption = (SELECT IncludeCommissionInPNL_EquityOption FROM @ParameterPreferences)                  
SET @IncludeCommissionInPNL_Futures = (SELECT IncludeCommissionInPNL_Futures FROM @ParameterPreferences)                  
SET @IncludeCommissionInPNL_FutOptions = (SELECT IncludeCommissionInPNL_FutOptions FROM @ParameterPreferences)                  
SET @IncludeCommissionInPNL_Swaps = (SELECT IncludeCommissionInPNL_Swaps FROM @ParameterPreferences)                  
SET @IncludeCommissionInPNL_FX = (SELECT IncludeCommissionInPNL_FX FROM @ParameterPreferences)                  
SET @IncludeCommissionInPNL_Other = (SELECT IncludeCommissionInPNL_Other FROM @ParameterPreferences)                 
                           
------------------------------------------------------------------------------------------                                
--Pick Selected Funds Based on their ID                                
------------------------------------------------------------------------------------------                                
Declare @T_FundIDs Table                                                                                                                                              
(                                
 FundId int                                
)                      
                                
Insert Into @T_FundIDs Select * From dbo.Split(@Fund, ',')                                                                                  
                                                                                 
Declare @T_CompanyFunds TABLE                                                                                                                                      
(                                                                                                                   
 CompanyFundID int,                                
 FundName varchar(50)                                
)                                
                                                    
Insert Into @T_CompanyFunds                                                          
Select                                                                                                                           
CompanyFundID,                                                                                    
FundName                                                                                    
From T_CompanyFunds INNER JOIN @T_FundIDs FundIDs ON T_CompanyFunds.CompanyFundID = FundIDs.FundID    
  
Declare @BaseCurrency Varchar(10)                  
Set  @BaseCurrency = (select top 1 CurrencySymbol from t_company Company          
     left outer join T_Currency Currency               
     on Company.BaseCurrencyID = Currency.CurrencyID)                
                                       
------------------------------------------------------------------------------------------                                
--Pick Required Fields from T_MW_GenericPNL                                
------------------------------------------------------------------------------------------                                
SELECT @NAV =          
SUM(                
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
                               
--ISNULL(SUM(EndingmarketValueBase + Dividend),0) AS EODNAV                                 
FROM T_MW_GenericPNL PNL                                
INNER JOIN @T_CompanyFunds Temp ON Temp.FundName = PNL.Fund                                   
WHERE                                 
DATEDIFF(Day,Rundate,@EndDate)=0 And Open_CloseTag IN ('O','Accruals')  
                         
Return @NAV          
                          
END