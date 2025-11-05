/*
Author: Sachin Mishra         
Date:11-08-2015         
Description:  Get pnl with fund and symbol Wise
Execution Method:          
[P_MW_FundSymbolWisePNL_EZE] '2015-11-17', '1286' ,1,'1020' ,1
*/

                                                                                             
ALTER Procedure [dbo].[P_MW_FundSymbolWisePNL_EZE]                                                                                            
(                                                                                            
 @EndDate datetime,                                                                                            
 @Fund Varchar(max),                                            
 @IncludeHistoricalPNL Bit,                                            
 @PTHFund Varchar(Max),
 @IncludeCash Bit                                                                                             
)                                                                                            
AS                                                                                            
BEGIN                                                                                       
                                                                                    
--Declare @EndDate datetime                                                                                  
--Declare @Fund Varchar(2000)                                            
--Declare @IncludeHistoricalPNL Bit                                                                                      
--Declare @PTHFund Varchar(max)
--Declare @IncludeCash Bit                                            
--                                                                                 
--Set @EndDate = '2015-11-17'                                                                              
--Set @Fund ='1286'               
--Set @IncludeHistoricalPNL=1                                            
--Set @PTHFund = '1307' 
--Set @IncludeCash = 0
                             
                                                                                            
Declare @DefaultAUECID int                                                                                              
Set @DefaultAUECID=(select top 1 DefaultAUECID  from T_Company where companyId <> -1)                                                                                            
-------------------------------------------------------------------                                                               
--                                                                                            
-------------------------------------------------------------------                                                      
Declare @T_FundIDs Table                                                
(                                                                                                                                            
 FundId int                                                                                                                             
)                                       
Insert Into @T_FundIDs Select * From dbo.Split(@Fund, ',')                                 
--------------------------------------------------------------------------------                                                 
---- PTH Funds Removal Section                                
--------------------------------------------------------------------------------                                              
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
-----------------------------------------------------------------------------------------------------------                                            
--REMOVING PTH FUNDS FROM FUND PARAMETER STRING FOR [F_MW_GetLinkedPortfolioACB_DailyReport] FUNCTION                                            
-----------------------------------------------------------------------------------------------------------                                            
DECLARE @FundsExcludingPTHFunds Varchar(max)                                            
SELECT  @FundsExcludingPTHFunds = ISNULL(@FundsExcludingPTHFunds,'')+ CONVERT(varchar(4), FundId)+',' FROM @T_FundIDs                                              
-----------------------------------------------------------------------------------------------------------                                                                                                                                           
                                                                                                                                              
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
----------------------------------------------------------------------------                                                                                            
--Adjusting Dates AUECwise PreviousBusinessDay, MonthStartDate,YearStartDate                                                                             
----------------------------------------------------------------------------                                                                              
                                                                                            
Declare                                                            
@PreviousBusinessDay DateTime,                                                                            
@MTDFromdate datetime,                                                                        
@YTDFromdate datetime                                                                                            
                                                                
Set @PreviousBusinessDay =  dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID)                                                                             
Select @MTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,3)                                                                                          
Select @YTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,7)                                                                                                                     
                                                                                       
IF (Datediff(Day,@EndDate,@MTDFromdate)=0 and Datediff(Day,@EndDate,@YTDFromdate)=0)                                                                                         
BEGIN                                                                      
Set @MTDFromdate=dbo.F_MW_GetWeekendAdjusted (dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID),1,3)                                                                                                                                        
Set @YTDFromdate=dbo.F_MW_GetWeekendAdjusted (dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID),1,7)                                                                                                                                        
END                                                                                         
 ----------------------------------------------------------------------------------          
--THIS PART IS HARD CODED BECAUSE TIGERVEDA USES HISTORICAL PNL BEFORE '7/31/2015'          
----------------------------------------------------------------------------------          
Declare @ITDDate Datetime          
Set @ITDDate='08/01/2015'                                                                                          
----------------------------------------------------------------------------------                                                                                        
--Select Symbols which are OPEN                                                                                      
-------------------------------------------------------------------           
Select  
 Fund,               
Symbol AS OpenSymbol              
Into #TempOpenPosSymbols                                                        
From T_MW_GenericPNL          
Inner Join #T_CompanyFunds ON #T_CompanyFunds.FundName = T_MW_GenericPNL.Fund                 
Where Datediff(day , Rundate , @EndDate) = 0 
 And Open_CloseTag IN ('O')
AND Asset <> Case WHEN @IncludeCash=0 THEN 'Cash' ELSE '' END
           
Group By Fund,Symbol       
      
                                                              
-------------------------------------------------------------------                                                                                            
--Selection of Required Fields From Generic Middleware Table                                                                                            
-------------------------------------------------------------------                                                                                            
SELECT   
 PNL.Fund,
Max(                     
Case                                                                              
When (Asset = 'FX' Or Asset = 'FXForward') And SM.LeadCurrency <> 'USD'                                                                                    
Then IsNull(SM.LeadCurrency,PNL.TradeCurrency)                                       
When (Asset = 'FX' Or Asset = 'FXForward') And SM.LeadCurrency = 'USD'                                                                                    
Then IsNull(SM.VSCurrency,PNL.TradeCurrency)                                                                      
Else SecurityName                                                                              
End) As SecurityName,                                                                                    
Symbol, 
Case
WHEN (Max(PNL.Asset)='CASH')                                 
THEN '@CASH'+Max(pnl.Currency)         
ELSE Symbol                      
END as SecurityIdentifier,                                                        
Max(PNL.BloombergSymbol) As BloombergSymbol,                                                                                    
Max(PNL.OSISymbol) As OSISymbol,
Max(PNL.SEDOLSymbol) As SEDOLSymbol,
Max(PNL.CUSIPSymbol) As CUSIPSymbol,
Sum(                                                  
Case
 When DateDiff(Day,@EndDate,Rundate) = 0 And Open_CloseTag = 'O' And Asset = 'CASH'
 THEN BeginningMarketValueLocal * SideMultiplier                                                         
 When DateDiff(Day,@EndDate,Rundate) = 0 And Open_CloseTag = 'O' And Asset <> 'FX' And Asset <> 'FXForward'                                                                                     
 Then BeginningQuantity * SideMultiplier                                               
 When DateDiff(Day,@EndDate,Rundate) = 0 And Open_CloseTag = 'O' And (Asset = 'FX' Or Asset = 'FXForward')                                                                                    
 Then                                       
 Case                                      
  When PNL.BaseCurrency = SM.LeadCurrency                                        
  Then (BeginningQuantity * SideMultiplier * UnitCostLocal) * (-1)                                     
  Else (BeginningQuantity * SideMultiplier)                                
 End                                        
 Else 0.0                                                                                        
End) As Position,  
Max(Asset) As Asset,                                                          
Sum(                                                                                         
 Case                                                             
 When Datediff (day ,@MTDFromdate , Rundate) >= 0 And Datediff(day ,Rundate ,@EndDate) >= 0 AND Datediff(day ,@ITDDate ,Rundate) >= 0                                                      
                           
 Then TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend                                                                                                                               
 Else 0.0                              
 End) As  MTDPNL,                                                                                        
 Sum(          
Case          
WHEN Datediff(day ,@ITDDate ,Rundate) >= 0          
THEN (TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend)           
ELSE 0.0          
END) As YTDPNL                                                                                    
                                                       
Into #TempBodyPNL                                                                                    
                                                                                            
From T_MW_GenericPNL PNL                                                                                                                          
Inner Join #T_CompanyFunds ON #T_CompanyFunds.FundName = PNL.Fund                                                                                      
Inner Join #TempOpenPosSymbols TOPS ON  TOPS.OpenSymbol = PNL.Symbol and  TOPS.fund =PNL.Fund                                                              
Left Outer Join V_SecMasterData SM On SM.TickerSymbol = PNL.Symbol                                                                
Where                                                                                                                                                                     
Datediff (day , @YTDFromdate , Rundate) >= 0 And                                                                                        
Datediff(day , Rundate , @EndDate) >= 0 And                                                                                             
Open_CloseTag <> 'Accruals'
AND Asset <> Case WHEN @IncludeCash=0 THEN 'Cash' ELSE '' END                                                                                  
Group By  PNL.Fund,Symbol                            
                                    
                             
--------------------------------------                                  
--Add MTD and YTD Performance Columns                                   
--------------------------------------                                  
                                  
Alter Table #TempBodyPNL                                                                                
Add MTDPercent Float                                                                     
                  
Alter Table #TempBodyPNL                                                                         
Add YTDPercent Float                                  
                                  
Update #TempBodyPNL                                  
Set MTDPercent =0.0,                             
 YTDPercent =0.0                                  
                                  
--Select * from  #TempBodyPNL                                     
 ----------------------------------------------------------------------------------          
--THIS PART IS HARD CODED BECAUSE TIGERVEDA USES HISTORICAL PNL BEFORE '7/31/2015'          
----------------------------------------------------------------------------------          
IF(DATEDIFF(Day,@YTDFromdate,@ITDDate)>0)           
BEGIN          
SET @YTDFromdate= @ITDDate          
END          
IF(DATEDIFF(Day,@MTDFromdate,@ITDDate)>0)           
BEGIN          
SET @MTDFromdate= @ITDDate          
END                             
---------------------------------------------------------------------------------------------------------------------------                                  
--This Part Includes Historical PNL which is not detailed and only used for calculating YTD PNL of JAN,FEB,MAR,APR of 2015                                                  
---------------------------------------------------------------------------------------------------------------------------                                                
                                                  
IF(@IncludeHistoricalPNL=1 AND DATEDIFF(Day,'2015/01/01',@EndDate)>=0 AND DATEDIFF(Day,@EndDate,'2015/12/31')>=0)                                                  
BEGIN                                  
                                                 
SELECT                                  
MAX(InvestmentDescription) AS SecurityName,                                  
InvestmentCode AS Symbol,          
InvestmentCode AS BloombergSymbol,                                  
0 AS Position,                                  
0 AS SideMultiplier,                                  
MAX(Asset) AS Asset,                                                  
                                        
CASE                                  
WHEN DATEDIFF(Day,'2015/07/31',@EndDate)=0                                        
THEN ISNULL(SUM(JulPNL),0)                                        
WHEN DATEDIFF(Day,'2015/06/30',@EndDate)=0                                        
THEN ISNULL(SUM(JunPNL),0)                                        
WHEN DATEDIFF(Day,'2015/05/31',@EndDate)=0                                        
THEN ISNULL(SUM(MayPNL),0)                                        
WHEN DATEDIFF(Day,'2015/04/30',@EndDate)=0                                        
THEN ISNULL(SUM(AprPNL),0)                                        
WHEN DATEDIFF(Day,'2015/03/31',@EndDate)=0                                        
THEN ISNULL(SUM(MarPNL),0)                                        
WHEN DATEDIFF(Day,'2015/02/28',@EndDate)=0                                        
THEN ISNULL(SUM(FebPNL),0)                                        
WHEN DATEDIFF(Day,'2015/01/31',@EndDate)=0                                        
THEN ISNULL(SUM(JanPNL),0)                                        
ELSE 0.0                                        
END AS MTDPNL,                                        
                                        
CASE                                  
WHEN DATEDIFF(Day,'2015/07/31',@EndDate)>=0                                        
THEN ISNULL(SUM(JanPNL+FebPNL+MarPNL+AprPNL+MayPNL+JunPNL+JulPNL),0)                                         
WHEN DATEDIFF(Day,'2015/06/30',@EndDate)=0                                        
THEN ISNULL(SUM(JanPNL+FebPNL+MarPNL+AprPNL+MayPNL+JunPNL),0)                                        
WHEN DATEDIFF(Day,'2015/05/31',@EndDate)=0                                        
THEN ISNULL(SUM(JanPNL+FebPNL+MarPNL+AprPNL+MayPNL),0)                                        
WHEN DATEDIFF(Day,'2015/04/30',@EndDate)=0                                        
THEN ISNULL(SUM(JanPNL+FebPNL+MarPNL+AprPNL),0)                                        
WHEN DATEDIFF(Day,'2015/03/31',@EndDate)=0                                        
THEN ISNULL(SUM(JanPNL+FebPNL+MarPNL),0)                                        
WHEN DATEDIFF(Day,'2015/02/28',@EndDate)=0                                 
THEN ISNULL(SUM(JanPNL+FebPNL),0)                                        
WHEN DATEDIFF(Day,'2015/01/31',@EndDate)=0                                        
THEN ISNULL(SUM(JanPNL),0)                                        
ELSE 0.0                                        
END AS YTDPNL,                                  
                                  
CASE                                  
WHEN DATEDIFF(Day,'2015/07/31',@EndDate)=0                                        
THEN log(1 + SUM(JulPNLPercent/100))          
WHEN DATEDIFF(Day,'2015/06/30',@EndDate)=0                                        
THEN log(1 + SUM(JunPNLPercent/100))                                         
WHEN DATEDIFF(Day,'2015/05/31',@EndDate)=0                            
THEN log(1 + SUM(MayPNLPercent/100))                                          
WHEN DATEDIFF(Day,'2015/04/30',@EndDate)=0                                        
THEN log(1 + SUM(AprPNLPercent/100))                                         
WHEN DATEDIFF(Day,'2015/03/31',@EndDate)=0                                        
THEN log(1 + SUM(MarPNLPercent/100))                                          
WHEN DATEDIFF(Day,'2015/02/28',@EndDate)=0                                        
THEN log(1 + SUM(FebPNLPercent/100))                                        
WHEN DATEDIFF(Day,'2015/01/31',@EndDate)=0                                        
THEN log(1 + SUM(JanPNLPercent/100))                                    
ELSE 0.0                                        
END AS MTDPercent,                                  
                                  
CASE                                  
WHEN DATEDIFF(Day,'2015/07/31',@EndDate)>=0                                        
THEN (log(1 + SUM(JulPNLPercent/100))+log(1 + SUM(JunPNLPercent/100))+log(1 + SUM(MayPNLPercent/100))+log(1 + SUM(AprPNLPercent/100))+ log(1 + SUM(MarPNLPercent/100))+ log(1 + SUM(FebPNLPercent/100))+log(1 + SUM(JanPNLPercent/100)))          
WHEN DATEDIFF(Day,'2015/06/30',@EndDate)=0                     
THEN (log(1 + SUM(JunPNLPercent/100))+log(1 + SUM(MayPNLPercent/100))+log(1 + SUM(AprPNLPercent/100))+ log(1 + SUM(MarPNLPercent/100))+ log(1 + SUM(FebPNLPercent/100))+log(1 + SUM(JanPNLPercent/100)))          
WHEN DATEDIFF(Day,'2015/05/31',@EndDate)=0                                        
THEN (log(1 + SUM(MayPNLPercent/100))+log(1 + SUM(AprPNLPercent/100))+ log(1 + SUM(MarPNLPercent/100))+ log(1 + SUM(FebPNLPercent/100))+log(1 + SUM(JanPNLPercent/100)))          
WHEN DATEDIFF(Day,'2015/04/30',@EndDate)=0                                        
THEN (log(1 + SUM(AprPNLPercent/100))+ log(1 + SUM(MarPNLPercent/100))+ log(1 + SUM(FebPNLPercent/100))+log(1 + SUM(JanPNLPercent/100)))                                         
WHEN DATEDIFF(Day,'2015/03/31',@EndDate)=0                                        
THEN (log(1 + SUM(MarPNLPercent/100))+ log(1 + SUM(FebPNLPercent/100))+log(1 + SUM(JanPNLPercent/100)))          
WHEN DATEDIFF(Day,'2015/02/28',@EndDate)=0                                        
THEN (log(1 + SUM(FebPNLPercent/100))+log(1 + SUM(JanPNLPercent/100)))                                        
WHEN DATEDIFF(Day,'2015/01/31',@EndDate)=0                                        
THEN (log(1 + SUM(FebPNLPercent/100)))                                    
ELSE 0.0                                        
END AS YTDPercent                                  
                                  
INTO #TempTigerVedaHistoricalPNL                                                    
FROM T_TigerVedaMonthWiseHistoricalPNL TVPS                                                
Inner Join #T_CompanyFunds ON #T_CompanyFunds.FundName = TVPS.Account                                                   
WHERE Asset<>'Cash'                                                   
GROUP BY                                                  
InvestmentCode                                  
    
                                 
                                  
Update #TempBodyPNL                                  
Set                                  
#TempBodyPNL.MTDPNL=ISNULL(#TempBodyPNL.MTDPNL,0)+ISNULL(TVHP.MTDPNL,0),                            
#TempBodyPNL.YTDPNL=ISNULL(#TempBodyPNL.YTDPNL,0)+ISNULL(TVHP.YTDPNL,0),                                  
#TempBodyPNL.MTDPercent =ISNULL(#TempBodyPNL.MTDPercent,0)+ISNULL(TVHP.MTDPercent,0),                        
#TempBodyPNL.YTDPercent =ISNULL(#TempBodyPNL.YTDPercent,0)+ISNULL(TVHP.YTDPercent,0)                      
FROM             
#TempBodyPNL                                  
INNER JOIN #TempTigerVedaHistoricalPNL TVHP ON (#TempBodyPNL.Symbol=TVHP.Symbol )--AND #TempBodyPNL.AssetSideCategory = TVHP.AssetSideCategory)                 
                
--Select * from  #TempBodyPNL                              
                                  
                                  
IF(                                  
--DATEDIFF(Day,'2015/08/31',@EndDate)=0 OR                 
DATEDIFF(Day,'2015/07/31',@EndDate)=0 OR DATEDIFF(Day,'2015/06/30',@EndDate)=0 OR                                  
DATEDIFF(Day,'2015/05/31',@EndDate)=0 OR DATEDIFF(Day,'2015/04/30',@EndDate)=0 OR DATEDIFF(Day,'2015/03/31',@EndDate)=0 OR                                  
DATEDIFF(Day,'2015/02/28',@EndDate)=0 OR DATEDIFF(Day,'2015/01/31',@EndDate)=0                                   
 )                                  
 BEGIN                                  
  Insert Into #TempBodyPNL                                  
  (SecurityName,                                  
  Symbol, 
  SecurityIdentifier,                                 
  BloombergSymbol,
  Position,  
  Asset,    
  OSISymbol,
  SEDOLSymbol,
  CUSIPSymbol,                                  
  MTDPNL,                                  
  YTDPNL,                                  
  MTDPercent,                                  
  YTDPercent)                                  
  Select                                  
  TVHP.SecurityName,                                  
  TVHP.Symbol, 
  Case
  WHEN (TVHP.Asset='CASH')                                 
  THEN '@CASH'+TVHP.Currency         
  ELSE Symbol                      
  END as SecurityIdentifier,                                 
  TVHP.BloombergSymbol,
  TVHP.Position,
  TVHP.Asset, 
  TVHP.OSISymbol,
  TVHP.SEDOLSymbol,
  TVHP.CUSIPSymbol,                                 
  TVHP.MTDPNL,                                  
  TVHP.YTDPNL,                                  
  TVHP.MTDPercent,                                  
  TVHP.YTDPercent                                   
  From #TempTigerVedaHistoricalPNL TVHP                                  
  Left Outer Join #TempBodyPNL ON (#TempBodyPNL.Symbol=TVHP.Symbol )                                  
  Where #TempBodyPNL.BloombergSymbol IS NULL                                 
 END                
Drop Table #TempTigerVedaHistoricalPNL                                         
END                 
                
 --------------------------------------------------------------------------------------------------------------                                                                        
--Calculation of MTD Contribution Percentage And YTD Contribution Percentage                                                               
--------------------------------------------------------------------------------------------------------------                                    
                                                                      
Update #TempBodyPNL                                                                        
Set                 
MTDPercent = dbo.[F_MW_GetLinkedPerformance_DailyReport](@MTDFromdate,@EndDate,@FundsExcludingPTHFunds,#TempBodyPNL.BloombergSymbol,ISNULL(#TempBodyPNL.MTDPercent,0)),                                                                        
YTDPercent = dbo.[F_MW_GetLinkedPerformance_DailyReport](@YTDFromdate,@EndDate,@FundsExcludingPTHFunds,#TempBodyPNL.BloombergSymbol,ISNULL(#TempBodyPNL.YTDPercent,0))                                                                        
 
UPDATE #TempBodyPNL
Set Symbol=SecurityIdentifier
WHERE Asset='Cash' 
                                                                               
Select * from #TempBodyPNL                                                                                    
Where Position <> 0                                                                                  
Order by SecurityName,Fund                                                                                   
--Order by CustomizedSorting,Side,SecurityName                   
                                                                                    
Drop Table #T_CompanyFunds ,#TempOpenPosSymbols,#TempBodyPNL

end        
                                                                            