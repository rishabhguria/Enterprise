 /*************************************************                                                    
Author : Ankit Misra                                                   
Creation Date : 28th May , 2015                                                      
Description : Script for DashBoard PNL part of Daily Report                                      
                                       
Execution Statement:                                                   
P_MW_DashBoardPNL_DailyReport_WIthQTD @EndDate='2015-07-02 00:00:00:000',@Fund=N'1245,1213,1214,1238,1239,1240,1241,1242,1244,1243,1246',@IncludeHistoricalPNL = 1,@PTHFund = '1270'                               
*************************************************/                                  
CREATE Procedure [dbo].[P_MW_DashBoardPNL_DailyReport_WIthQTD]                                  
(                                  
 @EndDate datetime,                                  
 @Fund Varchar(max),                      
 @IncludeHistoricalPNL bit,                  
 @PTHFund Varchar(Max),    
 @IncludeCash bit                                 
)                                  
AS                                  
BEGIN                    
                  
--Declare @EndDate datetime                                  
--Declare @Fund Varchar(max)                      
--Declare @IncludeHistoricalPNL bit                
--Declare @PTHFund Varchar(Max)                 
--Declare @IncludeCash bit                   
--                  
--Set @EndDate ='10/26/2015'                        
--Set @Fund = '1282,1279,1280,1281,1294,1265,1305,1266,1304,1263,1264,1277,1302,1268,1269,1267,1303,1292'                       
--Set @IncludeHistoricalPNL =1                
--Set @PTHFund = '1307'                                    
--Set @IncludeCash =1                                      
--                                  
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
@YTDFromdate datetime,  
@QTDFromdate datetime                                
                                
Set @PreviousBusinessDay =  dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID)                                                                                  
Select @MTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,3)                                                                                           
Select @YTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,7)  
Select @QTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,5)                                                                            
                                                                            
IF (Datediff(Day,@EndDate,@MTDFromdate)=0 and Datediff(Day,@EndDate,@YTDFromdate)=0)                                                                       
BEGIN                           
Set @MTDFromdate=dbo.F_MW_GetWeekendAdjusted (dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID),1,3)               
Set @YTDFromdate=dbo.F_MW_GetWeekendAdjusted (dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID),1,7)   
Set @QTDFromdate=dbo.F_MW_GetWeekendAdjusted (dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID),1,5)                                                                             
END                                 
          
----------------------------------------------------------------------------------          
--THIS PART IS HARD CODED BECAUSE TIGERVEDA USES HISTORICAL PNL BEFORE '7/31/2015'          
----------------------------------------------------------------------------------          
Declare @ITDDate Datetime          
Set @ITDDate='08/01/2015'                                      
-------------------------------------------------------------------                                
--Selection of Required Fields From Generic Middleware Table                                
-------------------------------------------------------------------                      
SELECT                                 
--                              
--Case                              
-- When DateDiff(Day,@EndDate,Rundate) = 0 and Side='Long' and Asset NOT IN ('FX','FX Forward')                          
-- Then TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend                                                                                  
-- Else 0.0                               
--End As TodaysLongPNL,                              
--                              
--Case                              
-- When DateDiff(Day,@EndDate,Rundate) = 0 and Side='Short' and Asset Not IN('FX','FX Forward')                          
-- Then TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend                                                                                  
-- Else 0.0                               
--End As TodaysShortPNL,                              
--                              
--Case                              
-- When DateDiff(Day,@EndDate,Rundate) = 0 and Asset IN('FX','FX Forward')                              
-- Then TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend                                                                                  
-- Else 0.0                               
--End As TodaysFXPNL,                               
Sum(                            
Case                                             
 When DateDiff(Day,@EndDate,Rundate) = 0                                                                                  
 Then TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend                                                                                  
 Else 0.0                      
End) As TodayPNL,                                                                            
                                                           
Sum(                                                                                
Case                                                                                   
 When Datediff (day ,@MTDFromdate , Rundate) >= 0 And Datediff(day ,Rundate ,@EndDate) >= 0  And Datediff(day,@ITDDate,Rundate) >= 0                                                                                       
 Then TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend                                     
 Else 0.0                                                                               
End) As  MTDPNL,   
  
Sum(                                                                                
Case                                                                                   
 When Datediff (day ,@QTDFromdate , Rundate) >= 0 And Datediff(day ,Rundate ,@EndDate) >= 0  And Datediff(day,@ITDDate,Rundate) >= 0                                                                                       
 Then TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend                                     
 Else 0.0                      
End) As  QTDPNL,                           
                               
Sum(          
Case                                                                                         
 When Datediff(day ,@ITDDate,Rundate) >= 0                                                                                         
 Then TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend          
 Else 0.0          
End)  As YTDPNL                                 
Into #TempPNL                             
From T_MW_GenericPNL PNL                                                                               
Inner Join #T_CompanyFunds ON #T_CompanyFunds.FundName = PNL.Fund                                                                                        
Where                                                                                                         
Datediff (day , @YTDFromdate , Rundate) >= 0 and                                                                                                 
Datediff(day , Rundate , @EndDate) >= 0 AND           
Open_CloseTag <> 'Accruals'     
And Asset <>     
 CASE     
  WHEN  @IncludeCash <> 1     
  THEN 'Cash'     
  ELSE ''     
 END      
----Asset NOT IN ('Cash','FX','FXForward')        
        
--Select * from  #TempPNL                             
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
IF(DATEDIFF(Day,@QTDFromdate,@ITDDate)>0)           
BEGIN          
SET @QTDFromdate= @ITDDate          
END    
    
--Select  @MTDFromdate        
-----------------------------------------------------------------------------          
ALTER Table #TempPNL          
Add DailyPrecent Float,  
 MTDPercent Float,  
 YTDPercent Float,  
 QTDPercent Float          
          
Update #TempPNL          
Set DailyPrecent=ISNULL(DailyPrecent,0),          
 MTDPercent=ISNULL(MTDPercent,0),          
 YTDPercent=ISNULL(YTDPercent,0),  
 QTDPercent=ISNULL(QTDPercent,0)           
          
--Select * from #TempPNL          
                                    
-----------------------------------------------------------------------------------------------------------------------------                                      
--This Part Includes Historical PNL which is not detailed and only used for calculatingMTD PNL and YTD PNL from Historical DB                           
-----------------------------------------------------------------------------------------------------------------------------                           
IF(@IncludeHistoricalPNL=1 AND DATEDIFF(Day,'2015/01/01',@EndDate)>=0 AND DATEDIFF(Day,@EndDate,'2015/12/31')>=0)                      
BEGIN                      
UPDATE #TempPNL                      
SET YTDPNL = ISNULL(YTDPNL,0) +              
(              
SELECT               
CASE            
WHEN DATEDIFF(Day,@EndDate,'2015/07/31')<=0                    
THEN ISNULL(SUM(JanPNL+FebPNL+MarPNL+AprPNL+MayPNL+JunPNL+JulPNL),0)               
WHEN DATEDIFF(Day,@EndDate,'2015/06/30')=0              
THEN ISNULL(SUM(JanPNL+FebPNL+MarPNL+AprPNL+MayPNL+JunPNL),0)              
WHEN DATEDIFF(Day,@EndDate,'2015/05/31')=0              
THEN ISNULL(SUM(JanPNL+FebPNL+MarPNL+AprPNL+MayPNL),0)              
WHEN DATEDIFF(Day,@EndDate,'2015/04/30')=0              
THEN ISNULL(SUM(JanPNL+FebPNL+MarPNL+AprPNL),0)              
WHEN DATEDIFF(Day,@EndDate,'2015/03/31')=0              
THEN ISNULL(SUM(JanPNL+FebPNL+MarPNL),0)              
WHEN DATEDIFF(Day,@EndDate,'2015/02/28')=0              
THEN ISNULL(SUM(JanPNL+FebPNL),0)              
WHEN DATEDIFF(Day,@EndDate,'2015/01/31')=0             
THEN ISNULL(SUM(JanPNL),0)              
ELSE 0.0              
END                
FROM T_TigerVedaMonthWiseHistoricalPNL TVPS                      
Inner Join #T_CompanyFunds ON #T_CompanyFunds.FundName = TVPS.Account                  
--Where TVPS.Asset<>'Cash'                    
)               
              
UPDATE #TempPNL                      
SET MTDPNL = ISNULL(MTDPNL,0) +              
(              
SELECT                
CASE              
WHEN DATEDIFF(Day,@EndDate,'2015/07/31')=0              
THEN ISNULL(SUM(JulPNL),0)            
WHEN DATEDIFF(Day,@EndDate,'2015/06/30')=0              
THEN ISNULL(SUM(JunPNL),0)              
WHEN DATEDIFF(Day,@EndDate,'2015/05/31')=0              
THEN ISNULL(SUM(MayPNL),0)              
WHEN DATEDIFF(Day,@EndDate,'2015/04/30')=0              
THEN ISNULL(SUM(AprPNL),0)              
WHEN DATEDIFF(Day,@EndDate,'2015/03/31')=0              
THEN ISNULL(SUM(MarPNL),0)              
WHEN DATEDIFF(Day,@EndDate,'2015/02/28')=0              
THEN ISNULL(SUM(FebPNL),0)              
WHEN DATEDIFF(Day,@EndDate,'2015/01/31')=0              
THEN ISNULL(SUM(JanPNL),0)              
ELSE 0.0              
END                
FROM T_TigerVedaMonthWiseHistoricalPNL TVPS                      
Inner Join #T_CompanyFunds ON #T_CompanyFunds.FundName = TVPS.Account                  
--Where TVPS.Asset<>'Cash'                    
)              
                
UPDATE #TempPNL                
SET TodayPNL=ISNULL(TodayPNL,0.0),  
 QTDPNL=ISNULL(QTDPNL,0.0)                     
          
UPDATE #TempPNL                            
SET MTDPercent = MTDPercent +                    
(                    
SELECT                     
CASE                   
WHEN DATEDIFF(Day,@EndDate,'2015/07/31')=0                    
THEN (SUM(JulPNLPercent)/100)          
WHEN DATEDIFF(Day,@EndDate,'2015/06/30')=0                    
THEN (SUM(JunPNLPercent)/100)          
WHEN DATEDIFF(Day,@EndDate,'2015/05/31')=0                    
THEN (SUM(MayPNLPercent)/100)          
WHEN DATEDIFF(Day,@EndDate,'2015/04/30')=0                    
THEN (SUM(AprPNLPercent)/100)          
WHEN DATEDIFF(Day,@EndDate,'2015/03/31')=0                    
THEN (SUM(MarPNLPercent)/100)          
WHEN DATEDIFF(Day,@EndDate,'2015/02/28')=0                    
THEN (SUM(FebPNLPercent)/100)          
WHEN DATEDIFF(Day,@EndDate,'2015/01/31')=0                    
THEN (SUM(JanPNLPercent)/100)          
ELSE 0.0                    
END               
FROM T_TigerVedaMonthWiseHistoricalPNL TVPS                            
Inner Join #T_CompanyFunds ON #T_CompanyFunds.FundName = TVPS.Account                        
--Where TVPS.Asset<>'Cash'          
)          
             
UPDATE #TempPNL                            
SET YTDPercent = YTDPercent +                    
(         
SELECT                     
CASE                   
WHEN DATEDIFF(Day,@EndDate,'2015/07/31')<=0                    
THEN (log(1+(SUM(JanPNLPercent)/100))+log(1+(SUM(FebPNLPercent)/100))+log(1+(SUM(MarPNLPercent)/100))+log(1+(SUM(AprPNLPercent)/100))+log(1+(SUM(MayPNLPercent)/100))+log(1+(SUM(JunPNLPercent)/100))+log(1+(SUM(JulPNLPercent)/100)))          
WHEN DATEDIFF(Day,@EndDate,'2015/06/30')=0                    
THEN (log(1+(SUM(JanPNLPercent)/100))+log(1+(SUM(FebPNLPercent)/100))+log(1+(SUM(MarPNLPercent)/100))+log(1+(SUM(AprPNLPercent)/100))+log(1+(SUM(MayPNLPercent)/100))+log(1+(SUM(JunPNLPercent)/100)))          
WHEN DATEDIFF(Day,@EndDate,'2015/05/31')=0                    
THEN (log(1+(SUM(JanPNLPercent)/100))+log(1+(SUM(FebPNLPercent)/100))+log(1+(SUM(MarPNLPercent)/100))+log(1+(SUM(AprPNLPercent)/100))+log(1+(SUM(MayPNLPercent)/100)))                    
WHEN DATEDIFF(Day,@EndDate,'2015/04/30')=0                    
THEN (log(1+(SUM(JanPNLPercent)/100))+log(1+(SUM(FebPNLPercent)/100))+log(1+(SUM(MarPNLPercent)/100))+log(1+(SUM(AprPNLPercent)/100)))                    
WHEN DATEDIFF(Day,@EndDate,'2015/03/31')=0                    
THEN (log(1+(SUM(JanPNLPercent)/100))+log(1+(SUM(FebPNLPercent)/100))+log(1+(SUM(MarPNLPercent)/100)))                    
WHEN DATEDIFF(Day,@EndDate,'2015/02/28')=0                    
THEN (log(1+(SUM(JanPNLPercent)/100))+log(1+(SUM(FebPNLPercent)/100)))                    
WHEN DATEDIFF(Day,@EndDate,'2015/01/31')=0                    
THEN (log(1+(SUM(JanPNLPercent)/100)))                    
ELSE 0.0                    
END                      
FROM T_TigerVedaMonthWiseHistoricalPNL TVPS                            
Inner Join #T_CompanyFunds ON #T_CompanyFunds.FundName = TVPS.Account                     
)          
END          
--Select * from #TempPNL                   
----------------------------------------------------------------------------------------------------------------------------                      
                      
Declare @DailyPortfolioACB Float                                  
Set @DailyPortfolioACB = NULLIF(dbo.[F_MW_GetLinkedPortfolioACB_DailyReport](@EndDate,@EndDate,@FundsExcludingPTHFunds),0)          
          
          
          
Update #TempPNL                        
Set DailyPrecent = ISNULL((TodayPNL/@DailyPortfolioACB),0)           
          
IF(Datediff(Day,'2015/07/31',@EndDate)>0)          
BEGIN          
 Update #TempPNL                                                                          
 Set                                                                                   
 YTDPercent = dbo.[F_MW_GetLinkedPerformance_DailyReport](@YTDFromdate,@EndDate,@FundsExcludingPTHFunds,'Portfolio',#TempPNL.YTDPercent)          
           
 Declare @MonthlyPortfolioACB Float                                  
 Set @MonthlyPortfolioACB = NULLIF(dbo.[F_MW_GetLinkedPortfolioACB_DailyReport](@MTDFromdate,@EndDate,@FundsExcludingPTHFunds),0)    
 --Select @MonthlyPortfolioACB AS MonthlyPortfolioACB    
 Update #TempPNL                                                                          
 Set                                                                                   
 MTDPercent=ISNULL((MTDPNL/@MonthlyPortfolioACB),0)   
  
 Update #TempPNL                                                                          
 Set                                                                                   
 QTDPercent=dbo.[F_MW_GetLinkedPerformance_DailyReport](@QTDFromdate,@EndDate,@FundsExcludingPTHFunds,'Portfolio',#TempPNL.QTDPercent)   
         
END   
         
ELSE          
 BEGIN          
  Update #TempPNL                                                                          
  Set YTDPercent =(EXP(YTDPercent)-1)          
 END          
          
--------------------------------------------------------------------------------------------------------------------                           
          
Select * from #TempPNL                            
Drop Table #T_CompanyFunds,#TempPNL                            
END