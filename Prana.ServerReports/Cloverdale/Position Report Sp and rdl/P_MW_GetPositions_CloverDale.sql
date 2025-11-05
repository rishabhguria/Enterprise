                     
/****************************                                
Author : Sandeep Singh                                
Creation date : May 31,2015                                
Description : EOD postions and Day PNL for clover dale                               
                        
                            
Parameters Description:                            
@FutureMV_ZeroOrEndingMVOrUnrealized : 0-Zero , 1-Full Market Value , 2- Unrealized Cost Basis PNL .                            
@FXMV_ZeroOrEndingMVOrUnrealized : 0-Zero , 1-Full Market Value , 2- Unrealized Cost Basis PNL .                          
@SwapMV_ZeroOrEndingMVOrUnrealized : 0-Zero , 1-Full Market Value , 2- Unrealized Cost Basis PNL .                         
@IncludeFXPNLinFX : True - Include FX gain For  FX and FXForwards  in PNL else False.                          
@IncludeFXPNLinFutures : True - Include FX gain For Futures  in PNL else False.                          
@IncludeFXPNLinSwaps : True - Include FX gain For Swaps in PNL else False.                             
                             
Execution Method :                                
[P_MW_GetPositions_CloverDale] '2015-05-28',null,null,null,null,null,null, '1255,1256,1257,1252,1253,1254',1                     
                                
****************************/                                
                                
                                
ALTER Procedure [dbo].[P_MW_GetPositions_CloverDale]                                 
(                              
@EndDate datetime,                            
@FutureMV_ZeroOrEndingMVOrUnrealized int,                           
@FXMV_ZeroOrEndingMVOrUnrealized int,                            
@SwapMV_ZeroOrEndingMVOrUnrealized int,                            
@IncludeFXPNLinFX bit,                            
@IncludeFXPNLinFutures bit,                            
@IncludeFXPNLinSwaps bit,                
@Fund Varchar(1000),            
@paramNAVbyMWorPM Int                                     
)                                
As              
                           
--Declare @EndDate datetime                           
--Declare @FutureMV_ZeroOrEndingMVOrUnrealized int                           
--Declare @FXMV_ZeroOrEndingMVOrUnrealized int                            
--Declare @SwapMV_ZeroOrEndingMVOrUnrealized int                            
--Declare @IncludeFXPNLinFX bit                            
--Declare @IncludeFXPNLinFutures bit                            
--Declare @IncludeFXPNLinSwaps bit                
--Declare @Fund Varchar(1000)            
--Declare @paramNAVbyMWorPM Int              
--            
--Set @EndDate = '6/30/2015'            
--Set @FutureMV_ZeroOrEndingMVOrUnrealized  = 2            
--Set @FXMV_ZeroOrEndingMVOrUnrealized = 2            
--Set @SwapMV_ZeroOrEndingMVOrUnrealized =2            
--Set @IncludeFXPNLinFX = 2            
--Set @IncludeFXPNLinSwaps = 2            
--Set @Fund = '1252,1253,1254,1255,1256,1257,1258,1259,1260,1261,1262,1263'--'1252,1253,1254,1255,1256,1257,1258,1259,1260'            
--Set @paramNAVbyMWorPM = 1        
      
Declare @DefaultAUECID int                                    
Set @DefaultAUECID=(select top 1 DefaultAUECID  from T_Company where companyId <> -1)        
      
Declare @PreviousBusinessDay DateTime      
Set @PreviousBusinessDay =  dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID)      
                    
IF(@FutureMV_ZeroOrEndingMVOrUnrealized not in(0,1,2) or @FutureMV_ZeroOrEndingMVOrUnrealized is null)                    
Set @FutureMV_ZeroOrEndingMVOrUnrealized = 2                    
                    
IF(@FXMV_ZeroOrEndingMVOrUnrealized not in(0,1,2) or @FXMV_ZeroOrEndingMVOrUnrealized is null)                    
Set @FXMV_ZeroOrEndingMVOrUnrealized = 2                    
                    
IF(@SwapMV_ZeroOrEndingMVOrUnrealized not in(0,1,2) or @SwapMV_ZeroOrEndingMVOrUnrealized is null)                    
Set @SwapMV_ZeroOrEndingMVOrUnrealized = 2          
                              
Declare @T_Fund Table                                                            
(                                                
 FundName Varchar(200)                     
)                                                            
Insert Into @T_Fund             
Select FundName             
From T_CompanyFunds Where CompanyFundID In (Select * From dbo.Split(@Fund, ','))             
            
Create Table #FundWiseNAV            
(            
Fund Varchar(200),            
FundNAV Float,            
LocalCurrency Int            
)            
Declare @GlobalNAV Float              
                          
If (@paramNAVbyMWorPM = 1)                                    
 BEGIN            
 Insert Into #FundWiseNAV            
  Select             
   Fund,            
   SUM(ISNULL(EndingMarketValueBase,0)) As FundNAV,            
   Max(CF.LocalCurrency)            
   From T_MW_GenericPNL                                 
   Inner Join  @T_Fund TempFund On TempFund.FundName = T_MW_GenericPNL.Fund             
   Inner Join T_CompanyFunds CF on CF.FundName = TempFund.FundName                       
   Where Open_CLoseTag = 'O' And DATEDIFF(d,Rundate,@EndDate)=0                                  
   Group By Fund                                    
 END                                    
Else                                    
 Begin               
 Insert Into #FundWiseNAV             
  Select               
   CF.FundName As Fund,                          
   Sum(ISNULL(NAV.NAVValue,0)) As FundNAV,            
   Max(LocalCurrency)            
   From PM_NAVValue NAV                                      
   Inner Join T_CompanyFunds CF on CF.CompanyFundID=NAV.FundID                          
   Inner Join  @T_Fund TempFund On TempFund.FundName = CF.FundName                        
   Where datediff(d,@EndDate,Date)= 0                                   
   Group By CF.FundName                                     
 End              
            
----Select * from #FundWiseNAV                      
                                  
SELECT              
                    
T_MW_GenericPNL.Fund as Fund,                               
Max(FundCurrency.CurrencySymbol) As Currency_base,            
Max(TradeCurrency) As Currency_local,                                    
Max(Asset) as AssetClass,                                 
Max(MasterFund)  as  MasterFund,                                
--Case            
-- When OpeningFXRate Is Null Or OpeningFXRate = 0            
-- Then EndingFXRate             
-- Else OpeningFXRate            
--End As FXRateToBase,            
Max(EndingFXRate) As FXRateToBase,               
Symbol ,             
Max(UnderlyingSymbol) As UnderlyingSymbol,            
Max(SecurityName) As Description,             
Max(Multiplier) As Multiplier,             
Max(StrikePrice) As Strike,            
            
Max(CONVERT(VARCHAR(10), ExpirationDate, 101)) As Expiry,            
Max(PutOrCall) As Put_Call,             
Max(CONVERT(VARCHAR(10), @EndDate, 101)) As ReportDate,            
Round(Sum(BeginningQuantity),2) As EndofDayQuantity,            
Max(EndingPriceBase) As MarketPrice_base,            
Max(EndingPriceLocal) As MarketPrice_local,            
            
Round(Sum(TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend),2) As daily_income_base ,               
Round(Sum(TotalRealizedPNLOnCostLocal + ChangeInUnrealizedPNL_Local + DividendLocal),2) As daily_pnl,              
                      
Round(Sum(TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend),2) As daily_pnl_base,                   
Round(Sum(Dividend),2) As dividend_income_loss ,            
Round(Max(IsNull(#FundWiseNAV.FundNAV,0)),2) As net_asset_value,                  
Side,            
Max(CUSIPSymbol) As CUSIPSymbol,            
Max(ISINSymbol) As ISINSymbol,            
Max(SedolSymbol) As SedolSymbol,            
Max(ReutersSymbol) As ReutersSymbol,            
Max(BloombergSymbol) As BloombergSymbol,            
'' As UnderlyingCUSIP,            
'' As UnderlyingISIN,            
'' As UnderlyingSedol,            
'' As UnderlyingRIC,            
'' As UnderlyingBloomberg,            
            
Round(Sum(                 
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
),2) As MarketValue_local  ,                         
                        
               
Round(Sum(                                
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
),2)As MarketValue_base,                            
                           
                        
                                
Round(Sum(TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend),2) as TotalGainLossMTM_Method1_Cost,                                 
Round(Sum(TotalRealizedPNLMTM + TotalUnrealizedPNLMTM + Dividend),2) as TotalGainLossMTM_Method2_MTM,                                
Round(Sum(UnrealizedFXGainOnCostD2_Base - UnrealizedFXGainOnCostD0_Base ),2) as ChangeInUnReazliedFXGainLoss,                                
Round(Sum(UnrealizedTradingGainOnCostD2_Base - UnrealizedTradingGainOnCostD0_Base),2)   as TotalSecurityGainLossMTM,                   
Round(Sum(UnrealizedFXGainOnCostD2_Base - UnrealizedFXGainOnCostD0_Base),2)  as TotalFXGainLossMTM,    
MAX(UDASector) AS UDASector,    
Max(UDACountry) AS UDACountry,    
MAX(UDASecurityType) AS UDASecurityType                  
               
InTo #TempPositions                                 
from T_MW_GenericPNL            
Inner Join @T_Fund TempFund On TempFund.FundName =T_MW_GenericPNL.Fund             
Left Outer Join #FundWiseNAV On #FundWiseNAV.Fund =  T_MW_GenericPNL.Fund             
Left Outer Join T_Currency FundCurrency On FundCurrency.CurrencyID = #FundWiseNAV.LocalCurrency                       
Where                             
(Open_CloseTag = 'O' OR Open_CloseTag = 'D') And DateDiff(Day,Rundate,@Enddate) = 0              
And Asset <> 'Cash'                        
group by T_MW_GenericPNL.Fund,Symbol ,Side      
      
      
--Select * from #TempPositions Order By Fund,Symbol,Side      
      
Select         
T_MW_GenericPNL.Fund as Fund,      
Symbol,      
Side,      
Round(Sum(BeginningQuantity),2) AS StartofDayQuantity       
InTo #TempStartofDayQuantity                                 
from T_MW_GenericPNL            
Inner Join @T_Fund TempFund On TempFund.FundName =T_MW_GenericPNL.Fund             
Left Outer Join #FundWiseNAV On #FundWiseNAV.Fund =  T_MW_GenericPNL.Fund             
Left Outer Join T_Currency FundCurrency On FundCurrency.CurrencyID = #FundWiseNAV.LocalCurrency                       
Where                             
(Open_CloseTag = 'O' OR Open_CloseTag = 'D') And DateDiff(Day,Rundate,@PreviousBusinessDay) = 0              
And Asset <> 'Cash'                        
group by T_MW_GenericPNL.Fund,Symbol ,Side        
      
      
Select 
TP.Fund,      
TP.Currency_base,      
TP.Currency_local,      
TP.AssetClass,                                 
TP.MasterFund,                                     
TP.FXRateToBase,               
TP.Symbol ,             
TP.UnderlyingSymbol,            
TP.Description,             
TP.Multiplier,             
TP.Strike,      
TP.Expiry,      
TP.Put_Call,             
TP.ReportDate,      
ISNULL(TSDQ.StartofDayQuantity,0) AS StartofDayQuantity,           
ISNULL(TP.EndofDayQuantity,0) AS EndofDayQuantity,            
TP.MarketPrice_base,                        
TP.MarketValue_base,            
TP.MarketPrice_local,            
TP.MarketValue_local,             
TP.daily_income_base,               
TP.daily_pnl,              
TP.daily_pnl_base,                   
TP.dividend_income_loss ,            
TP.net_asset_value,                  
TP.Side,            
TP.CUSIPSymbol,            
TP.ISINSymbol,            
TP.SedolSymbol,            
TP.ReutersSymbol,            
TP.BloombergSymbol,            
TP.UnderlyingCUSIP,            
TP.UnderlyingISIN,            
TP.UnderlyingSedol,            
TP.UnderlyingRIC,            
TP.UnderlyingBloomberg,                              
TP.TotalGainLossMTM_Method1_Cost,                                 
TP.TotalGainLossMTM_Method2_MTM,                                
TP.ChangeInUnReazliedFXGainLoss,                                
TP.TotalSecurityGainLossMTM,                   
TP.TotalFXGainLossMTM,    
TP.UDASector,    
TP.UDACountry,    
TP.UDASecurityType,
Case
	When TP.Fund = 'VABI - MS - PB'
	Then '038CDKDA6'
	When TP.Fund = 'VABI - MS - FX'
	Then '0581CHXC7'
	When TP.Fund = 'VABI - MS - SWAP'
	Then '06178WXN7'
	When TP.Fund = 'CLVD - MS - PB'
	Then '038CAAPD2'
	When TP.Fund = 'CLVD - MS - FX'
	Then '0581CHFY9'
	When TP.Fund = 'CLVD - MS - SWAP'
	Then '06178WSG8'
    When TP.Fund = 'Clover - GS'
	Then '002596922'	
	Else TP.Fund
End As NirvanaFund     
from      
#TempPositions TP      
Left Outer Join  #TempStartofDayQuantity TSDQ On TP.Fund =TSDQ.Fund AND TP.Symbol =TSDQ.Symbol AND TP.Side =TSDQ.Side      
Order By Fund,Symbol,Side      
            
Drop Table #FundWiseNAV,#TempPositions,#TempStartofDayQuantity