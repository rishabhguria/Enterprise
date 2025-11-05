/*************************************************                                        
Author : Ankit Misra                                       
Creation Date : 10th June , 2015                                          
Description : Script for Calculating End Of The Day NAV for Daily Report                          
                           
Execution Statement:                                       
P_MW_GetEndOfTheDayNAV_DailyReport @EndDate='2015-06-15',@Fund=N'1245,1213,1214,1238,1239,1240,1241,1242,1244,1243,1246',@PTHFund = '1270'                
*************************************************/                      
ALTER Procedure [dbo].[P_MW_GetEndOfTheDayNAV_DailyReport]                      
(                      
 @EndDate datetime,                      
 @Fund Varchar(max),            
 @PTHFund Varchar(Max),    
 @ReportID Varchar(100)                      
)                      
AS                
                
--Declare @EndDate datetime                      
--Declare @Fund Varchar(max)        
--Declare @PTHFund Varchar(Max)              
--Set @EndDate ='2015-04-02'                
--Set @Fund = '1245,1213,1214,1238,1239,1240,1241,1242,1244,1243,1246'        
--Set @PTHFund  = 1270    
--Declare @ReportID Varchar(100)        
--Set @ReportID = 'DailyReport_MW'                
                      
BEGIN        
      
       
        
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
------------------------------------------------------------------------------------------                      
SELECT       
SUM(      
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
) As EODNAV        
                     
--ISNULL(SUM(EndingmarketValueBase + Dividend),0) AS EODNAV                       
FROM T_MW_GenericPNL PNL                      
INNER JOIN #T_CompanyFunds ON #T_CompanyFunds.FundName = PNL.Fund                         
WHERE                       
DATEDIFF(Day,Rundate,@EndDate)=0 And Open_CloseTag IN ('O','Accruals')  
               
                      
Drop Table #T_CompanyFunds,#ParameterPreferences                    
END