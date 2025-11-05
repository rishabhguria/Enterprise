  
/*************************************************                            
Author : Sachin Mishra                              
Creation Date : 1 Oct 2015                            
Description : This Sp returns dataset for TPH Risk Management Report PRANA-10756               
Execution Statement:                           
    
[P_MW_GetPortfolioComposition_RiskReport]  '2015-10-10','1182,1183,1184,1185,1186,1189','MTM_V0',5    
    
*************************************************/         
CREATE Procedure [dbo].[P_MW_GetPortfolioComposition_RiskReport]              
(              
@EndDate datetime,              
@Fund varchar(max),              
@ReportID Varchar(100),    
@TopNValue int             
)              
As        
        
--Declare @EndDate datetime,              
--@Fund varchar(max),              
--@ReportID Varchar(100),    
--@TopNValue int    
       
--Set @EndDate='2015-09-30'        
--Set @Fund='1182,1183,1184,1185,1186,1189'    
--Set @ReportID='MTM_V0'     
--Set @TopNValue=5    
    
       
        
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
              
Declare  @EndOFDayNav float        
       
Set @EndOFDayNav = (Select dbo.[F_MW_GetEndOfTheDayNAV_RiskReport] (@EndDate,@fund,@ReportID))        
      
-----------------------------Select All values From T_MW_GetGenericPNL ----------        
        
DECLARE @DefaultAUECID INT        
 SET @DefaultAUECID =         
        (        
          SELECT TOP 1 DefaultAUECID FROM T_Company        
    WHERE CompanyId <> - 1        
   )        
        
DECLARE         
@PreviousBusinessDay DATETIME,        
@MTDFromdate DATETIME,        
@YTDFromdate DATETIME        
        
SET @PreviousBusinessDay = dbo.AdjustBusinessDays(@EndDate, - 1, @DefaultAUECID)        
SELECT @MTDFromdate = dbo.F_MW_GetWeekendAdjusted(@EndDate, 1, 3)        
SELECT @YTDFromdate = dbo.F_MW_GetWeekendAdjusted(@EndDate, 1, 7)        
      
 IF ( Datediff(Day, @EndDate, @MTDFromdate) = 0 AND Datediff(Day, @EndDate, @YTDFromdate) = 0)        
 BEGIN        
  SET @MTDFromdate = dbo.F_MW_GetWeekendAdjusted(dbo.AdjustBusinessDays(@EndDate, - 1, @DefaultAUECID), 1, 3)        
  SET @YTDFromdate = dbo.F_MW_GetWeekendAdjusted(dbo.AdjustBusinessDays(@EndDate, - 1, @DefaultAUECID), 1, 7)        
 END        
        
      
SELECT         
Max(ISNULL(SecurityName , Symbol)) AS SecurityName        
,Symbol        
,Sum(      
  CASE         
  WHEN Datediff(Day, @MTDFromdate, Rundate) >= 0 AND Datediff(day, Rundate, @EndDate) >= 0        
  THEN TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend        
  ELSE 0.0        
  END) AS MTDPNL        
,Sum(TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend) AS YTDPNL        
INTo #PortfolioComposition       
FROM T_MW_GenericPNL PNL        
INNER JOIN #T_CompanyFunds ON #T_CompanyFunds.FundName = PNL.Fund        
WHERE Datediff(day, @YTDFromdate, Rundate) >= 0 AND Datediff(day, Rundate, @EndDate) >= 0        
AND Open_CloseTag <> 'Accruals' And Asset <> 'Cash'        
GROUP BY Symbol        
        
--Added MTDPerOFGMV,YTDPerOFGMV columns in #PortfolioComposition        
        
 ALTER TABLE #PortfolioComposition        
 ADD MTDPerOFGMV float        
        
 ALTER TABLE #PortfolioComposition        
 ADD YTDPerOFGMV float        
        
--- Calculate values of MTDPerOFGMV,YTDPerOFGMV         
        
Update #PortfolioComposition Set MTDPerOFGMV = ISNULL(((MTDPNL*10000)/NULLIF(@EndOFDayNav,0)),0) From #PortfolioComposition        
Update #PortfolioComposition set YTDPerOFGMV = ISNULL(((YTDPNL*10000)/NULLIF(@EndOFDayNav,0)),0) from #PortfolioComposition        
    
Create Table #TempPortFolio    
(    
 SecurityName Varchar(200)    
,Symbol Varchar(200)    
,MTDPNL float    
,YTDPNL float    
,MTDPerOFGMV float    
,YTDPerOFGMV float    
,Flag Varchar(100)    
)    
    
EXEC(    
'Insert into #TempPortFolio     
Select Top '+@TopNValue+    
'     
SecurityName,    
Symbol,    
MTDPNL,    
YTDPNL,    
MTDPerOFGMV,    
YTDPerOFGMV,'+    
'''PMTD'''+     
'From  #PortfolioComposition where MTDPNL >=0       
order by MTDPNL desc')    
    
    
EXEC(    
'Insert into #TempPortFolio     
Select Top '+@TopNValue+    
'     
SecurityName,    
Symbol,    
MTDPNL,    
YTDPNL,    
MTDPerOFGMV,    
YTDPerOFGMV,'+    
'''NMTD'''+     
'From  #PortfolioComposition where MTDPNL < 0    
order by MTDPNL Asc')    
    
    
EXEC(    
'Insert into #TempPortFolio     
Select Top '+@TopNValue+    
'     
SecurityName,    
Symbol,    
MTDPNL,    
YTDPNL,    
MTDPerOFGMV,    
YTDPerOFGMV,'+    
'''PYTD'''+     
'From  #PortfolioComposition where YTDPNL >=0    
order by YTDPNL desc')    
    
    
EXEC(    
'Insert into #TempPortFolio     
Select Top '+@TopNValue+    
'     
SecurityName,    
Symbol,    
MTDPNL,    
YTDPNL,    
MTDPerOFGMV,    
YTDPerOFGMV,'+    
'''NYTD'''+     
'From  #PortfolioComposition where YTDPNL <0     
order by YTDPNL Asc')    
    
    
    
Select * from #TempPortFolio      
--Drop All temp tables.        
Drop table #Funds,#T_CompanyFunds,#PortfolioComposition,#TempPortFolio