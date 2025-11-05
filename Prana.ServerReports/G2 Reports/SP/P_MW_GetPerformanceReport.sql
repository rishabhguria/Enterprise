/************************************************************************************                                    
                                    
P_MW_GetPerformanceReport 'G2 Investment Partners LP,G2 Investment Partners QP LP','MTM_V0',1,'2014/05/21',0,'2014/05/01'                                     
@NAVfromPMorMW = 0 For MW                                    
      = 1 For PM                                      
                                    
************************************************************************************/                                    
                                    
                                    
CREATE Procedure [dbo].[P_MW_GetPerformanceReport]                                    
(                                    
  @Fund varchar(max),                                    
  @ReportID Varchar(100),                                    
  @TimeFrame Int,                                    
  @EndDate datetime,                                    
  @NAVfromPMorMW int,                                
  @ITDDate datetime                                     
)                                    
as                                     
BEGIN                                    
                                    
SET NOCOUNT on                                    
                                    
--Declare @TimeFrame Int                                    
--Declare @Fund varchar(max)                                    
--Declare @ReportID Varchar(100)                                    
--DECLARE @companyID int                                    
--DECLARE @EndDate datetime                 
--DECLARE @NAVfromPMorMW int                
--declare @ITDDate datetime                              
--              
--Set @TimeFrame = 1                                    
--Set @Fund = 'G2 Investment Partners LP,G2 Investment Partners QP LP'                                    
--Set @ReportID = 'MTM_V0'                                    
--SET @companyID = 6                                    
--SET @EndDate ='2014/05/21'               
--set @NAVfromPMorMW =0               
--set @ITDDate ='2014/05/01'                              
              
DECLARE @auecID int                                      
DECLARE @GlobalNAV float                                
                   
        
                             
SELECT                                 
 distinct(symbol) as Symbol              
 ,Asset              
 ,Side                                    
 ,SecurityName                                    
into #SymbolsTillEndDate                                 
from t_mw_genericPNL                                 
where  DateDiff(d,Rundate,@EndDate)=0   And Fund in (Select * from dbo.split(@Fund,',')) and Open_CloseTag <> 'Accruals'                     
      
      
insert into #SymbolsTillEndDate      
SELECT                                 
 distinct(symbol) as Symbol              
 ,Asset              
 ,Side                                    
 ,SecurityName                                    
from t_mw_genericPNL                                 
where DateDiff(d,@ITDDate,Rundate)>=0 and DateDiff(d,Rundate,@EndDate)>0   And Fund in (Select * from dbo.split(@Fund,',')) and Open_CloseTag <> 'Accruals'                     
and Symbol not in (select Symbol from #SymbolsTillEndDate)      
        
Select @GlobalNAV = SUM(ISNULL(EndingMarketValueBase,0)) From T_MW_GenericPNL                                           
Where Open_CLoseTag = 'O' And DATEDIFF(d,Rundate,@EndDate)=0 And Fund In (select * from  dbo.Split(@Fund,',')) and Open_CloseTag <> 'Accruals'                                           
                                    
                                    
CREATE table #PnLandNAVForEachMonth                                    
(                                    
 Symbol varchar(200),                                    
 QTDStartDate datetime,                                    
 QuarterId int,            
 YearId int,                                    
 DailyROR float,                                  
 MTDROR float,                                    
 QTDROR float,                                    
 YTDROR FLOAT,                                    
 DailyContributionValue float,                                   
 MTDContributionValue float,                                  
 QTDContributionValue float,                                    
 YTDContributionValue float,                                    
 IsSavedRecord bit                                    
)               
              
--- getROR and Contribution Values              
create table #TempRORandContributionTable              
(              
 Symbol varchar(200),              
 FromDate datetime,              
 ToDate datetime,              
 DataType varchar(20),              
 rtNegativeCount int,              
 ContribNegativeCount int,              
 rt float,              
 Contrib float              
)              
            
declare @StartDate datetime            
set @StartDate =DATEADD(yy, DATEDIFF(yy,0,@EndDate), 0)            
            
insert into #TempRORandContributionTable              
exec  P_MW_PeriodLinkedReturn_Report @StartDate,@EndDate,@ITDDate,@Fund              
              
              
              
-- To get Saved Data from T_InceptionSymbolPerformance -- Needed Only for 2014 as we have hard coded to get data.                                     
insert into #PnLandNAVForEachMonth                                    
select                                     
 T_InceptionSymbolPerformance.Symbol,                                  
 dbo.F_MW_GetWeekendAdjusted(@EndDate,1,5),                                     
 Datepart(qq,@EndDate),                                    
 Datepart(year,@EndDate),                                  
 0,                                  
 0,                                  
 QTDROR,                                    
 YTDROR,                                   
 0,                                  
 0,                          
 QTDContribution,                                    
 YTDContribution,                                    
 1                                    
from T_InceptionSymbolPerformance                                    
inner join #SymbolsTillEndDate on #SymbolsTillEndDate.Symbol = T_InceptionSymbolPerformance.Symbol                                    
                                  
                             
                     
-- GET Data From T_MW_GENRICPNL                                     
Select                                     
T_MW_GenericPNL.Symbol,                                    
Rundate,                                  
--Fund,                                    
--Side,                                    
--SideMultiplier,                                    
--SecurityName,                                    
Case                                    
When Open_CloseTag = 'O'                                    
Then                                    
 CASE                                          
  WHEN (T_MW_GenericPNL.asset = 'CASH')                                                         
  Then endingmarketvaluelocal        
  Else BeginningQuantity * SideMultiplier                                    
 END                                     
Else 0                                    
END AS BeginningQuantity,                                    
CASE                                    
 When T_MW_GenericPNL.Asset ='CASH'                                  
 Then OpeningFXRate                                  
 When (Open_CloseTag = 'O')                                  
 Then UnitCostBase                                   
                                   
 Else 0                                    
End as UnitCostBase,                                    
Case                                  
 when (T_MW_GenericPNL.Asset = 'CASH')                                  
 Then  EndingFXRate            
 else EndingPriceBase                                  
End as EndingPriceBase,                                    
0 As TotalPNLMTMBase,                                    
CASE                                    
 When Open_CloseTag = 'O'                                    
 Then TotalUnrealizedPNLMTM                                    
 Else 0                                   
End TotalUnrealizedPNLMTM,                                    
                                    
CASE                                    
When Open_CloseTag = 'C'                                    
Then TotalRealizedPNLMTM          
Else 0                                    
End TotalRealizedPNLMTM,                                    
                                    
CASE                                    
When Open_CloseTag = 'O'                                    
Then EndingMarketValueBase                                    
Else 0                                    
End EndingMarketValueBase,                                    
                                    
Dividend,                                    
Open_CloseTag,                                    
0 As ROI,                                    
T_MW_GenericPNL.Asset                                    
Into #Temp                                    
from T_MW_GenericPNL                                     
--inner JOIN #SymbolsTillEndDate on #SymbolsTillEndDate.Symbol = T_MW_GenericPNL.Symbol                      
Where  DateDiff(Day,RunDate,@EndDate) = 0                                    
And Fund in (Select * from dbo.split(@Fund,',')) and Open_CloseTag <> 'Accruals'                              
                  
                           
insert INTO                                 
#PnLandNAVForEachMonth                                
(                    
  Symbol                                
 ,QTDStartDate                                
 ,QuarterId                                
 ,YearId                                
 ,DailyROR                                
 ,MTDROR                                
 ,QTDROR                                
 ,YTDROR                                
 ,DailyContributionValue                                
 ,MTDContributionValue                                
 ,QTDContributionValue                                
 ,YTDContributionValue                                
 ,IsSavedRecord                                
)                                   
                                 
select               
 Symbol              
,MAX(Case               
 When Datatype = 'QTD'              
 Then FromDate              
 End) as QTDStartDate               
              
,Datepart(qq,@EndDate)  as QuarterId              
,Datepart(year,@EndDate) as YearId              
,MAX(Case               
 When Datatype = 'Daily'              
 Then Isnull(100*rt,0)              
 End) as DailyROR              
,MAX(Case               
 When Datatype = 'MTD'              
 Then Isnull(100*rt,0)   End) as MTDROR              
,MAX(Case               
 When Datatype = 'QTD'              
 Then Isnull(100*rt,0)              
 End) as QTDROR               
,MAX(Case               
 When Datatype = 'YTD'              
 Then Isnull(100*rt,0)              
 End) as YTDROR              
,MAX(Case               
 When Datatype = 'Daily'              
 Then Isnull(100*Contrib,0)              
 End) as DailyContributionValue              
,MAX(Case               
 When Datatype = 'MTD'              
 Then Isnull(100*Contrib,0)              
 End )as MTDContributionValue              
,MAX(Case               
 When Datatype = 'QTD'              
 Then Isnull(100*Contrib,0)              
 End) as QTDContributionValue               
,MAX(Case               
 When Datatype = 'YTD'              
 Then Isnull(100*Contrib,0)              
 End) as YTDContributionValue              
,0 as IsSavedRecord              
from #TempRORandContributionTable              
group by Symbol                                
              
              
                
                                    
Select                                     
#PnLandNAVForEachMonth.Symbol,                                    
#SymbolsTillEndDate.Side,                                    
#SymbolsTillEndDate.SecurityName,                                    
Sum(#Temp.BeginningQuantity) As BeginningQuantity,                                    
Case                                     
 When Sum(#Temp.BeginningQuantity) <> 0         
 Then Sum(#Temp.UnitCostBase * #Temp.BeginningQuantity) /  Sum(#Temp.BeginningQuantity)                                     
 Else 0                                     
End as WtdPrice,                                    
Max(#Temp.EndingPriceBase) As EndingPriceBase,                                    
Sum(#Temp.EndingMarketValueBase) As EndingMarketValueBase,                                    
MAX(#PnLandNAVForEachMonth.DailyROR) As DailyROR,                                    
MAX(#PnLandNAVForEachMonth.MTDROR) as MTDROR,                                    
Cast(0 AS float) as QTDROR,                                    
Cast(0 AS float) As YTDROR,                                    
Cast(0 AS float) As PercEquityClosed,                                    
Cast(0 AS float) As PricePercChangefromCost,                                    
ISNULL(Sum(ISNULL(#Temp.TotalRealizedPNLMTM,0))+Sum(ISNULL(#Temp.TotalUnrealizedPNLMTM,0))+Sum(ISNULL(#Temp.Dividend,0)),0) As TotalPNL,                                    
MAX(#PnLandNAVForEachMonth.DailyContributionValue) As DailyPNLContri,                                    
MAX(#PnLandNAVForEachMonth.MTDContributionValue) As MTDPNLContri,                                    
Cast(0 AS float) As QTDPNLContri,                                    
Cast(0 AS float) As YTDPNLContri,                                    
#SymbolsTillEndDate.Asset                                    
InTo #Temp_ROI                                    
From #Temp                                    
right outer Join #PnLandNAVForEachMonth On #PnLandNAVForEachMonth.Symbol = #Temp.Symbol               
Inner join #SymbolsTillEndDate on  #PnLandNAVForEachMonth.Symbol =  #SymbolsTillEndDate.Symbol                           
where IsSavedRecord = 0                                  
Group By #SymbolsTillEndDate.Side,#SymbolsTillEndDate.SecurityName,#PnLandNAVForEachMonth.Symbol,#SymbolsTillEndDate.Asset                                    
                            
                      
Update #Temp_ROI                                    
Set PricePercChangefromCost =                                     
CASE                                    
 WHEN WtdPrice = 0                                    
 THEN 0                                    
 ELSE ((EndingPriceBase - WtdPrice)/WtdPrice) * 100                                    
End                                    
                                    
update #Temp_ROI                                    
SET PercEquityClosed =                                     
CASE                                     
 WHEN EMVCalc.EMV <> 0                                    
 THEN (EMVCalc.EMV/@GlobalNAV)*100                                    
 Else 0                                 
END                       
from #Temp_ROI                                    
inner JOIN                                     
(                                    
 SELECT                                     
  #Temp.Symbol as Symbol,                                    
  SUM(isnull(EndingMarketValueBase,0)) as EMV                                      
 FROM #Temp                                    
 where DATEDIFF(d,Rundate,@EndDate)=0                                    
 group BY #Temp.Symbol                                    
) EMVCalc                                    
on EMVCalc.Symbol = #Temp_ROI.Symbol                                    
                                    
                                   
                                    
Update #Temp_ROI        
Set QTDPNLContri =                                    
  CASE                         
 WHEN CountNegative % 2 <> 0                                    
   Then ((-1*QtdContValue)-1)*100                                    
   else                                    
   (QtdContValue-1)*100                                    
  End                                      
FROM #Temp_ROI                                     
inner join                                    
(                                    
 SELECT                                     
  Symbol,                                    
 EXP(SUM(Log(                                     
                                       
     CASE                                    
   WHEN (1 + (QTDContributionValue/100))<0                                    
   THEN (1 + (QTDContributionValue/100))*-1                                     
   ELSE (1 + (QTDContributionValue/100))                                    
  END                             
  ))) as QtdContValue,                                    
  count(                                    
     CASE WHEN 1 + (QTDContributionValue/100) < 0 Then 1 END                                    
  ) as CountNegative                                    
 From #PnLandNAVForEachMonth                                     
 where datepart(qq,@EndDate) =  #PnLandNAVForEachMonth.QuarterId                                     
 group by Symbol                                    
) QTDContribution                                    
on QTDContribution.Symbol =  #Temp_ROI.Symbol                                     
                                    
                                  
Update #Temp_ROI                                    
Set YTDPNLContri =                                     
  CASE                                    
   WHEN CountNegative % 2 <> 0                                    
   Then ((-1*YTDContValue)-1)*100                                    
   else                                    
   (YTDContValue-1)*100                                    
  End                                      
FROM #Temp_ROI inner join                                    
(                                    
 SELECT                                     
  Symbol,                                    
  EXP(SUM(Log(                                    
  CASE                                    
   WHEN (1 + (YTDContributionValue/100))<0                                    
   THEN (1 + (YTDContributionValue/100))*-1                                     
   ELSE (1 + (YTDContributionValue/100))                                    
  END                                   
  ))) as YTDContValue,                                    
  count(                                    
     CASE WHEN 1 + (YTDContributionValue/100) < 0 Then 1 END                                    
  ) as CountNegative                                    
 From #PnLandNAVForEachMonth                                    
 where datepart(year,@EndDate) =  YearId                                     
 group by Symbol                                    
)YTDContribution                                    
on YTDContribution.Symbol =  #Temp_ROI.Symbol               
                                    
                                    
Update #Temp_ROI                                    
Set YTDROR  =  CASE                                    
    WHEN CountNegative % 2 <>0                                    
    THEN ((-1*YTDRORCalc.YTDROR) - 1)*100                                    
    ELSE  ((YTDRORCalc.YTDROR) - 1)*100                                    
      END                                     
FROM #Temp_ROI inner join                                    
(                                    
 SELECT                          
  Symbol,                                    
  EXP(SUM(Log(                                    
      CASE                                    
   WHEN (1 + (YTDROR/100))<0                                    
   THEN (1 + (YTDROR/100))*-1                                     
   ELSE (1 + (YTDROR/100))                                    
   END                                    
  ))) as YTDROR,                                    
  count(                                     
     CASE WHEN (1 + (YTDROR/100)) < 0 Then 1 END                                    
  ) as CountNegative                                    
 From #PnLandNAVForEachMonth                                   
where datepart(year,@EndDate) =  #PnLandNAVForEachMonth.YearId                                      
 group by Symbol                                    
)YTDRORCalc                                    
on YTDRORCalc.Symbol =  #Temp_ROI.Symbol                     
                     
                                    
Update #Temp_ROI                                    
Set QTDROR  = CASE                                    
    WHEN CountNegative % 2 <>0                                    
    THEN ((-1*QTDRORCalc.QTDROR) - 1)*100                                    
    ELSE  ((QTDRORCalc.QTDROR) - 1)*100                                    
      END                                     
FROM #Temp_ROI inner join                                    
(                                    
 SELECT                                     
  Symbol,                                    
  EXP(SUM(Log(                                    
    CASE                                    
     WHEN (1 + (QTDROR/100))<0                                    
     THEN (1 + (QTDROR/100))*-1          
     ELSE (1 + (QTDROR/100))                                    
    END                                    
   ))) as QTDROR,                                    
  count(                                     
      CASE WHEN (1 + (QTDROR/100)) < 0 Then 1 END                                    
  ) as CountNegative                                    
 From #PnLandNAVForEachMonth                                    
 where datepart(qq,@EndDate) =  #PnLandNAVForEachMonth.QuarterId                                     
 group by Symbol                                    
)QTDRORCalc                                    
on QTDRORCalc.Symbol =  #Temp_ROI.Symbol                                    
                                    
SELECT               
Asset,                             
case             
 when Asset ='Cash'            
 Then 'Cash'            
 else Side            
End as Side,                                    
SecurityName,                                    
Symbol,                                    
ISnull(BeginningQuantity,0) as BeginningQuantity,                                    
Isnull(ROUND(WtdPrice,2),0) as WtdPrice,                                    
Isnull(ROUND(EndingPriceBase,2),0) as EndingPriceBase,                                    
Isnull(ROUND(PricePercChangefromCost,2),0) as PricePercChangefromCost,                                    
Isnull(cast(TotalPNL as int),0) as TotalPNL,                                    
                                    
--EndingMarketValueBase,                                    
ISnull(Round(DailyROR,4),0) as DailyROR,                                    
ISnull(Round(DailyPNLContri,4),0) as DailyPNLContri,                                    
ISnull(ROUND(MTDROR,2),0) as MTDROR,                                    
ISnull(ROUND(MTDPNLContri,2),0) as MTDPNLContri,                                    
ISnull(ROUND(QTDROR,2),0) as QTDROR,                                    
ISnull(ROUND(QTDPNLContri,2),0) as QTDPNLContri,                                    
ISnull(ROUND(YTDROR,2),0) as YTDROR,                                    
ISnull(ROUND(YTDPNLContri,2),0) as YTDPNLContri,                                    
ISnull(ROUND(PercEquityClosed,2),0) as PercEquityClosed                              
FROM #Temp_ROI                         
order by Symbol                      
                                 
--order by Asset                                    
drop table #PnLandNAVForEachMonth,#temp,#Temp_ROI,#TempRORandContributionTable,#SymbolsTillEndDate                                 
                                    
END