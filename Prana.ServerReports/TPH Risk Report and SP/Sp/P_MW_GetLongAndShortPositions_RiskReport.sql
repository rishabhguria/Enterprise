/*************************************************                                
Author : Sachin Mishra                                  
Creation Date : 7 Oct 2015                                
Description : This Sp returns dataset of Positions for TPH Risk Management Report PRANA-10756                   
Execution Statement:                               
            
            
Exec [P_MW_GetLongAndShortPositions_RiskReport] '2015-10-10','1182,1183,1184,1185,1186,1189','MTM_V0' ,10           
             
*************************************************/             
CREATE Procedure [dbo].[P_MW_GetLongAndShortPositions_RiskReport]                  
(                  
@EndDate datetime,                  
@Fund varchar(max),                  
@ReportID Varchar(100),    
@TopNValue Int                
)                  
As            
            
--Declare @EndDate datetime,                  
--@Fund varchar(max),                  
--@ReportID Varchar(100)            
--            
--Set @EndDate='2015-09-30'            
--Set @Fund='1182'            
--Set @ReportID='MTM_V0'            
            
Set Nocount ON            
Select * Into #Funds                                              
from dbo.Split(@fund, ',')             
            
-----------------------------------------------------------------------------------------            
-- Filtering Selected Funds            
-----------------------------------------------------------------------------------------            
Declare @T_FundIDs Table                                                        
(                                                                                                                                                    
 FundId int                                                                                                                                     
)                                               
Insert Into @T_FundIDs Select * From dbo.Split(@Fund, ',')              
            
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
            
-------------------------------------------------------------------------------            
--Extracting all the Open Data into Temp Table For the EndDate From Generic PNL            
-------------------------------------------------------------------------------            
Select * Into #PNL            
From T_MW_GenericPNL            
Where Datediff(d,rundate,@EndDate)=0                  
And Fund In (Select FundName From #T_CompanyFunds)              
And Open_CloseTag <> 'C'            
----------------------------------------------------------------------------            
--Importing Preferences for this Report            
----------------------------------------------------------------------------                                          
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
Set  @BaseCurrency = (Select TOP 1 CurrencySymbol from T_Company Company            
left outer join T_Currency Currency  on (Company.BaseCurrencyID = Currency.CurrencyID)             
Where CompanyID<>-1)            
            
            
-----------------------Insert UDASubSector, Side,EndingMarkeValue in #TempSubSectorWiseGMV Group By UDASubSector  and Side -----------------------            
            
Select              
PNL.Symbol As Symbol,          
Max(ISNULL(SecurityName , Symbol)) AS SecurityName,            
PNL.Side As Side,      
Max(SideMultiplier) As SideMultiplier,            
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
 ) As EndingMarketValue         
        
Into #TempSubSectorWiseGMV                    
        
From #PNL PNL                  
Where Open_CloseTag='O' And Asset <> 'Cash'                   
Group by PNL.Symbol,PNL.Side                  
            
            
---------------calculation of GrossMarketValue ----------------------------            
Declare @GrossMarketValue Float            
Select @GrossMarketValue = Sum(ABS(EndingMarketValue))  from #TempSubSectorWiseGMV              
            
Set @GrossMarketValue= IsNull(@GrossMarketValue,0)    
    
    
CREATE TABLE #TempPortFolioPositions (      
 SecurityName VARCHAR(200)      
 ,EndingMarketValue FLOAT      
 ,PerOfGMV FLOAT      
 ,Flag VARCHAR(100)      
 )      
      
EXEC (      
  'Insert into #TempPortFolioPositions       
Select Top ' + @TopNValue + '      
SecurityName,           
EndingMarketValue As EndingMarketValue,            
IsNull(EndingMarketValue/NULLIF('+@GrossMarketValue+',0),0)*100  As PerOfGMV ,    
' + '''LPosition''' + '    
From  #TempSubSectorWiseGMV  where Side=' + '''Long''' + '      
order by EndingMarketValue desc'      
  )      
      
    
EXEC (      
  'Insert into #TempPortFolioPositions       
Select Top ' + @TopNValue + '      
SecurityName,           
EndingMarketValue  As EndingMarketValue,            
IsNull(EndingMarketValue/NULLIF('+@GrossMarketValue+',0),0)*100  As PerOfGMV ,    
' + '''SPosition''' +     
'From  #TempSubSectorWiseGMV  where Side=' + '''Short''' + '      
order by EndingMarketValue Asc'      
)      
    
    
SELECT * FROM #TempPortFolioPositions      
    
            
------------Drop All temp tables            
Drop table #PNL,#ParameterPreferences,#TempSubSectorWiseGMV,#Funds,#T_CompanyFunds,#TempPortFolioPositions