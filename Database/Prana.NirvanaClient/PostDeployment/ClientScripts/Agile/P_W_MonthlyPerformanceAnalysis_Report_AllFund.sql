/*=============================================                                                        
 Author:        <Author,,LYNN>                                                        
 Create date: <Create Date,,>                                                        
 Description:    <Description,,>                                                        
 Return VAMI of Fund VS. Index with daily return according to funds for a given period of time                                                        
 EXEC dbo.P_W_MonthlyPerformanceAnalysis_Report '7/22/2015','10/1/2014','Clover Street'                                                  
                                                
 Modified By:  Pooja Porwal                                                     
 Create date: 16 Dec 2015                                                      
 Description: Pick exposure values from provided sample file if not provided than calculate.                                                
 Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-10582  
  
Modified By:  Pankaj Sharma  
Create date: 29 JAN 2016  
Description:   
1) Pick values from provided sample file if not provided than calculate.  
2) Pick indexes from Security Master.  
3) Have a dropdown to select from choice of indexes.  
4) Implement the logic to pick Index values from provided sample file if not provided than calculate it from Historicals DB.  
5) Adjust prior values for MTD, QTD, YTD and ITD if a value already exists for months prior to inception date.  
6) Calculate AUM, Fair value long and Fair value Short using the same scheme defined above.  
7) Exposure section dynamic in the report for each fund.  
8) Attribution section dynamic in the report for each section.  
                                               
Jira :   
http://jira.nirvanasolutions.com:8080/browse/PRANA-13332  
http://jira.nirvanasolutions.com:8080/browse/PRANA-14003  
http://jira.nirvanasolutions.com:8080/browse/PRANA-14035  
http://jira.nirvanasolutions.com:8080/browse/PRANA-13776  
                                                      
EXEC dbo.[P_W_MonthlyPerformanceAnalysis_Report_AllFund]   
@Todate = '12/31/2015',  
@Fund = '1218',  
@Indexes = '$HGX,$NDX,$RUT,$SPX,$SPXPM,$VIX'   
                         
 =============================================*/                        
CREATE Procedure [dbo].[P_W_MonthlyPerformanceAnalysis_Report_AllFund]          
(                                                      
    @ToDate Datetime,                                                 
    @Fund varchar(max),                        
    @Indexes varchar(max)                        
)                                                        
AS                                                        
                                                     
--Declare @ToDate Datetime                                                                                          
--Declare @Fund varchar(max)                        
--Declare @Indexes varchar(max)                        
--                        
--Set @ToDate =  '12/31/2014'                
--Set @Fund = '1218'--,1270,1271,1286'                         
--Set @Indexes ='$HGX,$NDX,$RUT,$SPX,$SPXPM,$VIX'                
                                              
--select * into #FundTable from dbo.split(@Fund,',')                         
Create table #Dimensions(Dimension Varchar(50),Sortorder int)                        
Insert into  #Dimensions VALUES('MTD',1)                        
Insert into  #Dimensions VALUES('QTD',2)                        
Insert into  #Dimensions VALUES('YTD',3)                        
Insert into  #Dimensions VALUES('ITD',4)                        
                            
                        
Select SecmasterData.Symbol_PK as IndexID,CompanyName as IndexName,TickerSymbol as IndexSymbol,1 as IsIndex                  
into #Indexes                
from V_SecMasterData SecmasterData                
where SecmasterData.TickerSymbol in (Select Items from dbo.split(@Indexes,','))                
                
--Select * from #Indexes                
                
Select IndexSymbol + ISNULL(' ' + Dimension, '') as [Type],IndexName + ISNULL(' ' + Dimension, '') as [TypeName],* into #FinalIndexes                             
from #Indexes                            
Cross Join                            
#Dimensions                            
                            
--Select * From #FinalIndexes                          
                        
/*--------------------------------------------------------------------------------------------------                                                          
CREATE TABLE to get FUND Wise DAILY, MTD, QTD, YTD, CashManagementStartDate(ITD) and END dates                                                         
---------------------------------------------------------------------------------------------------*/                                                        
CREATE TABLE #tempFundWiseDates                        
(               
FundID INT                                                        
,CashManagentStartDate DATETIME                
,Enddate DATETIME                                                        
,Dailydate DATETIME                                    
,MTDFromdate DATETIME                                                        
,QTDFromdate DATETIME                                                      
,YTDFromdate DATETIME                                                        
)                                                
                                          
/*--------------------------------------------------------------------------------------------------                                                      
Data Populated from P_FundWiseToDatesOnITD IN THE TABLE                                           
---------------------------------------------------------------------------------------------------*/                                                        
INSERT INTO #tempFundWiseDates EXEC P_FundWiseToDatesOnITD  @ToDate,@fund,'False'                          
                        
alter table #tempFundWiseDates add                         
FundName varchar(MAX),                        
NetQTDValuePriorToITD float,                        
GrossQTDValuePriorToITD float,                        
NetYTDValuePriorToITD float,                        
GrossYTDValuePriorToITD float,                        
NetITDValuePriorToITD float,                        
GrossITDValuePriorToITD float                        
                        
                        
Update #tempFundWiseDates set FundName = CF.FundName                        
from #tempFundWiseDates FundWiseName                        
Inner JOIN T_companyFunds CF on  FundWiseName.FundID = CF.CompanyFundID                        
                        
DECLARE @ITD DATETIME                        
SELECT  @ITD=min(CashManagentStartDate) from #tempFundWiseDates                        
                                    
-----------------------------------------------------------------------------------------------------------------------------------                                    
------------------Date handling in the Function starts                                    
-----------------------------------------------------------------------------------------------------------------------------------                                    
                                              
DECLARE @Date TABLE (                                              
 Year VARCHAR(max)                                              
 ,Month VARCHAR(max)                                              
 ,Day VARCHAR(max)                                              
 ,DATE DATETIME                                              
 )                                              
                                              
--------------------------------For ITD                                      
INSERT INTO @Date (Month)                                              
 SELECT number                                              
 FROM master..spt_values                                              
 WHERE type = 'P'                                              
  AND number >= Month(@ITD)                                              
  AND number < = 12                                             
                                      
 UPDATE @Date                                              
 SET [Year] = (                                              
   SELECT TOP 1 number                                              
   FROM master..spt_values                                              
   WHERE type = 'P'                                              
    AND number = year(@ITD)                                              
   )                                              
where Year is NULL                                      
                                      
                                      
                                      
--------------------------------For middle years                                   
If(year(@ITD)-year(@ToDate) <> 0)                                
Begin                                   
 Declare @Count int                                      
 Select @Count = Count(*)                                       
FROM master..spt_values                                    
 WHERE type = 'P'                                              
 AND number > year(@ITD)                                      
 AND number < year(@ToDate)                                    
            
 While(@Count>0)                                      
 Begin                                      
 Declare @Year int                                      
 Select @Year = Max(Year) + 1 from @Date                                      
 INSERT INTO @Date (Month)                         
  SELECT number                                              
  FROM master..spt_values                                              
  WHERE type = 'P'                                              
   AND number >= 1                                             
   AND number <= 12                                          
                                       
  UPDATE @Date                                              
  SET [Year] = (                                              
    SELECT TOP 1 number                                              
    FROM master..spt_values                                              
   WHERE type = 'P'                                              
  AND number = @year                                      
    )                                              
 where Year is NULL                                      
                                       
 Set @Count = @Count-1                                      
                                       
 END                           
                                       
                                       
 --------------------------------For YTD                                      
 INSERT INTO @Date (Month)                                              
  SELECT number                                              
  FROM master..spt_values                                         
  WHERE type = 'P'                                              
   AND number >= 1                                      
   AND number <= Month(@ToDate)                                        
                                       
                                            
                                       
  UPDATE @Date                                              
  SET [Year] = (                                              
    SELECT TOP 1 number                                              
    FROM master..spt_values                                              
    WHERE type = 'P'                                              
  AND number = year(@ToDate)   
    )                                              
 where Year is NULL                                     
                                
END                                 
                                              
UPDATE @Date                                              
SET Day = CASE                                               
  WHEN Month = 1                                              
   THEN 31                                              
  WHEN Month = 2                                              
   THEN CASE                                               
     WHEN Year % 4 = 0                                              
      THEN 29                                              
     ELSE 28                                              
     END                                              
  WHEN Month = 3                                              
   THEN 31                                              
  WHEN Month = 4                                              
   THEN 30                                              
  WHEN Month = 5                                              
   THEN 31                                              
  WHEN Month = 6                                              
   THEN 30                                              
  WHEN Month = 7                                              
   THEN 31                                              
  WHEN Month = 8                                              
   THEN 31                                              
  WHEN Month = 9                                              
   THEN 30                                              
  WHEN Month = 10                    
   THEN 31                                              
  WHEN Month = 11                                              
   THEN 30                                              
  WHEN Month = 12                                              
   THEN 31                                              
  END                                              
                   
UPDATE @Date                                              
SET DATE = (                                              
  SELECT CAST(CAST(Year AS VARCHAR) + '-' + CAST(Month AS VARCHAR) + '-' + CAST(Day AS VARCHAR) AS DATETIME)                                              
  )                                              
WHERE DATE IS NULL                                              
             
UPDATE @Date                                              
SET DATE = @ToDate                                              
WHERE DATE = (                                              
  SELECT max(DATE)                                              
  FROM @Date                                              
  )                                              
                                              
UPDATE @Date                        
SET DATE = dbo.AdjustBusinessDays(DATE, - 1, 11)                                           
WHERE dbo.isbusinessday(DATE, 1) = 0 --set date to previous business day.  
  
Delete from @Date where Year = Year(@Todate) and Month >Month(@Todate)  
                                                 
                        
-------------------------------------------------------------------------------------------------------------------------------------                                    
--------------------Date handling in the Function ends                                    
-------------------------------------------------------------------------------------------------------------------------------------                                    
                         
Declare @Type table                                                        
(                                                        
 Type varchar(max),                                                        
 IndexSymbol varchar(max),                                                        
 IsIndex int                                                        )                    
insert into @Type Values ('GROSS MTD',null,0 )                                                        
insert into @Type Values ('GROSS QTD',null,0 )                                                        
insert into @Type Values ('GROSS YTD',null,0)                                                   
insert into @Type Values ('GROSS ITD',null,0)                                                        
insert into @Type Values ('NET MTD',null,0)                                                        
insert into @Type Values ('NET QTD',null,0)                                                        
insert into @Type Values ('NET YTD',null,0)                                                        
insert into @Type Values ('NET ITD',null,0)                                                        
insert into @Type Values ('NET EXP',null,0)                                             
insert into @Type Values ('GROSS EXP',null,0)                                                      
insert into @Type Values ('AUM',null,0)                        
Insert into @Type Select [Type],IndexSymbol,IsIndex from #FinalIndexes  order by IndexSymbol,Sortorder                       
insert into @Type Values ('FAIR VALUE LONGS',null,null)                        
insert into @Type Values ('FAIR VALUE SHORTS',null,null)                        
                                                   
                        
CREATE  table #Result                        
(                                        
Fund varchar(max),                        
Fund_Name varchar(max),                        
CashManagentStartDate DATETIME,                        
 Year varchar(4) ,                                                        
 FromDate datetime,                                                        
 ToDate datetime,                                                        
 DataType varchar(max),                                                        
 DataTypeDescription varchar(max),                
 Value float,                                        
 IndexSymbol varchar(max),                                                        
 ISIndex int,                                                        
 FromDatePrice float,                                                     
 ToDatePrice float,                                                        
 ID int identity(1,1)                                                        
)                                                  
                        
                
                        
insert into #Result(Fund,Fund_Name,CashManagentStartDate,Year,ToDate,DataType,IndexSymbol,IsIndex)                                     
select FundID,FundName,CashManagentStartDate,Year,Date,Type,IndexSymbol,IsIndex from @Date  left outer join @Type on 1=1                                               
Cross JOIN #tempFundWiseDates                        
                        
Delete from #Result where Datediff(d,CashManagentStartDate,ToDate)<0                        
                        
update #Result set FromDate = @ITD where  DataType like ('%ITD') or (DataType like ('%YTD') and Year=Year(@ITD))                                                        
update #Result set FromDate = DATEADD(yy, DATEDIFF(yy,0,Todate), 0) where DataType like ('%YTD') --and Year <>Year(@ITD)                                                        
update #Result set FromDate = DateAdd(qq,DateDiff(qq,0,ToDate),0) where DataType like ('%QTD') --and Year <>Year(@ITD)                  
update #Result set FromDate = CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(Todate)-1),todate),101) where DataType like ('%MTD')                                                        
update #Result set FromDate = dbo.AdjustBusinessDays(FromDate,-1,11) where dbo. isbusinessday(FromDate,1)=0 --set Fromdate to previous business day.                                                        
                        
Update #Result Set DataTypeDescription = DataType where IsIndex<>1 or IsIndex is NULL                
Update #Result Set DataTypeDescription = FIndices.TypeName from #Result RT INNER JOIN #FinalIndexes FIndices on RT.Datatype = FIndices.Type                
                
                
--Select * from #Result                
                        
-----------------get monthly return from beginning-----------------------------------------------                                                        
                                                        
declare  @MonthlyRT table                                                        
(                                                        
  Companyfund varchar(max),                                                        
  day1 datetime,                                                        
  day2 datetime,                                                        
  Pnl float,                                                        
  ExpFee float,                                                        
  CF float,                                            
  IncentiveFee float,                                                        
  ManagementFee float,                                                        
  GrossRT float,                                                        
  NetRT float,                                                        
  ID int                                                         
)                                                        
---------------------------------------------------------changes for fund via ID                          
insert into @MonthlyRT                                                        
select Companyfund,day1,day2,Pnl,ExpFee, CF,IncentiveFee,ManagementFee,GrossRT,NetRT,ID from dbo.F_MW_MonthlySimpleRT_ALLFunds(@ToDate, @Fund,@ITD) order by day2                                                        
                        
--Select * from @MonthlyRT                        
                                 
update #Result set Value=                                                 
(select                               
CASE                               
WHEN                       
MIN(abs(GrossRT+1)) = 0 then 0 ELSE                              
EXP(SUM(Log(abs(nullif(GrossRT+1,0))))) -- the base mathematics                              
* round(0.5-count(nullif(sign(sign(GrossRT+1)+0.5),1))%2,0) -- pairs up negatives                              
       END                              
-1 from @MonthlyRT  where day2<=Todate and Day1>=fromdate) where IsIndex=0 and  Datatype like '%Gross%'                          
                                                  
update #Result set Value=                                                  
(select CASE                               
WHEN                               
MIN(abs(NetRT+1)) = 0 then 0 ELSE                              
EXP(SUM(Log(abs(nullif(NetRT+1,0))))) -- the base mathematics                              
* round(0.5-count(nullif(sign(sign(NetRT+1)+0.5),1))%2,0) -- pairs up negatives                              
       END                              
-1 from @MonthlyRT  where day2<=Todate and Day1>=fromdate) Where IsIndex=0 and Datatype like '%Net%'                        
                            
                        
                             
----- update AUM values fund and datewise           
Update #Result                                               
Set Value = MW.EndingMarketValueBase                                              
--,Fund=MW.Fund                                                                                        
From #Result RData                                                                          
inner join                                                                                          
(                                                                                          
select Rundate,Fund,Sum(EndingMarketValueBase) as EndingMarketValueBase from t_mw_genericpnl                         
Inner JOIN T_CompanyFunds CF on CF.FundName = t_mw_genericpnl.Fund                        
where open_closetag<>'C' and CF.CompanyFundID in (select * from dbo.split(@fund,','))                                              
Group by Rundate,Fund                                              
) as MW                                   
on datediff(d,MW.Rundate,RData.Todate)=0  and RData.Fund_Name = MW.Fund                                              
WHERE                         
RData.DataType = 'AUM'                        
                                              
----- update AUM values fund and datewise                                                
                                               
----- update FAIR VALUE LONGS values fund and datewise                                                 
Update #Result                                
Set Value = MW.EndingMarketValueBase                                              
--,Fund=MW.Fund                                                                                        
From #Result RData                                                                          
inner join                                                                                          
(                                                                                          
select sum(EndingMarketValuebase) as EndingMarketValuebase,Fund,Rundate from t_mw_genericpnl      
Inner JOIN T_CompanyFunds CF on CF.FundName = t_mw_genericpnl.Fund        
where (                                              
open_closetag='O'                                               
and Asset<>'Cash'                 
and CF.CompanyFundID in (select * from dbo.split(@fund,','))                                               
and sideMultiplier=1                                              
)                                              
Group by Rundate,Fund                                              
)as MW                                                                    
on datediff(d,MW.Rundate,RData.Todate)=0  and RData.Fund_Name =MW.Fund                                              
WHERE RData.DataType = 'FAIR VALUE LONGS'                                              
                                                
----- update FAIR VALUE LONGS values fund and datewise                            
                                              
----- update FAIR VALUE SHORTS values fund and datewise                                                 
Update #Result                                               
Set Value = MW.EndingMarketValueBase                                              
--,Fund=MW.Fund                                                                                   
From #Result RData                                                                          
inner join                                                                                          
(                   
select sum(EndingMarketValuebase) as EndingMarketValuebase,Fund,Rundate from t_mw_genericpnl      
Inner JOIN T_CompanyFunds CF on CF.FundName = t_mw_genericpnl.Fund      
where (                                              
open_closetag='O' and Asset<>'Cash'                                                
and CF.CompanyFundID in (select * from dbo.split(@fund,','))       
and sideMultiplier=-1                                              
)                                              
Group by Rundate,Fund                                              
)as MW                                                                    
on datediff(d,MW.Rundate,RData.Todate)=0  and RData.Fund_Name =MW.Fund                                          
WHERE RData.DataType = 'FAIR VALUE SHORTS'                                              
               
 ----- update FAIR VALUE SHORTS values fund and datewise                                                 
                                                
declare @ExpPct table               
(                                                        
 FundName varchar(max),                                                        
 Date datetime,                                                        
 NetExp float,                                                        
 GrossExp float,                                                        
 FundAum float                                                        
)                                                        
                        
insert into @ExpPct (FundName,Date,NetExp)                                                        
select Fund,ToDate,Sum(Value) from #Result where datatype Like ('FAIR VALUE%') group by Fund,ToDate                                                        
                                              
update @ExpPct set GrossExp= (select sum(abs(Value)) from #Result                                               
Where datatype Like ('FAIR VALUE%') and Date=ToDate and Fund=FundName)                                                         
                                              
update @ExpPct set FundAum=Value from #Result                                               
Where datatype='AUM' and Date=ToDate and Fund=FundName                                                  
                                              
update #Result set Value= coalesce(NetExp/nullif(FundAum,0),0)                                               
from  @ExpPct                                                     
 Where Date=ToDate and DataType='Net Exp' and Fund=FundName                                                          
                                                 
update #Result set Value= COALESCE(GrossExp/nullif(FundAum,0),0)                                                       
 from @ExpPct Where Date=ToDate and DataType='Gross Exp' and Fund=FundName                                                           
                                                  
------------------------------------------------------------INDEXES          
declare  @ReturnsIndex table                                                        
(                                                        
  Companyfund varchar(max)          
  ,day1 datetime          
  ,day2 datetime          
  ,Index_Symbol varchar(MAX)          
  ,IndexName varchar(max)          
  ,RT float          
  ,ID int          
)                                                        
---------------------------------------------------------changes for fund via ID                                                 
insert into @ReturnsIndex          
select Companyfund,day1,day2,Index_Symbol,IndexName,RT,ID from dbo.F_MW_MonthlySimpleRT_ALLFunds_Indexes(@ToDate,@Fund,@ITD,@Indexes) order by day2                                                        
          
update #Result set Value=                                                 
(select                               
CASE                               
WHEN                               
MIN(abs(RT+1)) = 0 then 0 ELSE                              
EXP(SUM(Log(abs(nullif(RT+1,0))))) -- the base mathematics                              
* round(0.5-count(nullif(sign(sign(RT+1)+0.5),1))%2,0) -- pairs up negatives                              
       END                              
-1 from @ReturnsIndex  where day2<=Todate and Day1>=fromdate and IndexSymbol = Index_Symbol) where IsIndex=1 --and IndexSymbol like '%$Rut%'  -- Datatype like '%Gross%'                          
          
                        
----Select * from @ReturnsIndex          
                                            
          
------------------------------------------------------------INDEXES          
                                              
UPDATE #Result SET ISINDEX=2 WHERE DATATYPE LIKE ('FAIR VALUE%')                                                        
                                             
                                                    
update #Result                                           
SET                         
Value=COALESCE(Performance,Value,0)                                                    
From T_MW_PerformanceOnBoard MW inner JOIN #result R                                                    
on DATEDIFF(d,AsOfDate,ToDate)=0 and R.DataType=MW.DataType                                     
and MW.Fund=R.Fund_Name                         
where                         
MW.IsBenchmark =0 and MW.datatype not like '%MTD%' and MW.datatype not like '%QTD%' and MW.datatype not like '%YTD%' and MW.datatype not like '%ITD%'                        
                        
--update #Result                                                     
--SET Value=COALESCE(Performance,Value,0)                        
--From T_MW_PerformanceOnBoard MW inner JOIN #result R                                                    
--on DATEDIFF(d,AsOfDate,ToDate)=0 and R.DataType=MW.DataType                                           
--and MW.IsBenchmark=1                                          
                                          
update #Result                                           
SET Fund=FundReportname                                          
from T_MW_FundReportName inner join #Result on Fund=FundName                        
                        
----------------------------------------------------------------------                        
---------Adjust Prior to ITD values if any-------------                        
----------------------------------------------------------------------                        
---------------Gross values                        
Update #tempFundWiseDates Set GrossQTDValuePriorToITD = COALESCE(MW.Performance,0)                        
from #tempFundWiseDates FundWiseDate                        
Inner JOIN                        
(                        
select * FROM T_MW_PerformanceOnBoard MW where MW.DataType = 'Gross QTD'                        
)MW                      
on DATEDIFF(m,MW.AsOfDate,FundWiseDate.CashManagentStartDate)=1 and MW.DataType = 'Gross QTD' and Fund=FundName                  
                        
                        
Update #tempFundWiseDates Set GrossYTDValuePriorToITD = COALESCE(MW.Performance,0)                        
from #tempFundWiseDates FundWiseDate                        
Inner JOIN                        
(                        
select * FROM T_MW_PerformanceOnBoard MW where MW.DataType = 'Gross YTD'                        
)MW                        
on DATEDIFF(m,MW.AsOfDate,FundWiseDate.CashManagentStartDate)=1 and MW.DataType = 'Gross YTD' and Fund=FundName                        
                        
                        
Update #tempFundWiseDates Set GrossITDValuePriorToITD = COALESCE(MW.Performance,0)                        
from #tempFundWiseDates FundWiseDate                        
Inner JOIN                        
(                        
select * FROM T_MW_PerformanceOnBoard MW where MW.DataType = 'Gross ITD'                        
)MW                        
on DATEDIFF(m,MW.AsOfDate,FundWiseDate.CashManagentStartDate)=1 and MW.DataType = 'Gross ITD' and Fund=FundName                        
                        
                        
---------------Net values                        
Update #tempFundWiseDates Set NetQTDValuePriorToITD = COALESCE(MW.Performance,0)                        
from #tempFundWiseDates FundWiseDate                        
Inner JOIN                        
(                        
select * FROM T_MW_PerformanceOnBoard MW where MW.DataType = 'Net QTD'                        
)MW                        
on DATEDIFF(m,MW.AsOfDate,FundWiseDate.CashManagentStartDate)=1 and MW.DataType = 'Net QTD' and Fund=FundName                        
                        
                        
Update #tempFundWiseDates Set NetYTDValuePriorToITD = COALESCE(MW.Performance,0)                        
from #tempFundWiseDates FundWiseDate                        
Inner JOIN                        
(             
select * FROM T_MW_PerformanceOnBoard MW where MW.DataType = 'Net YTD'                        
)MW                        
on DATEDIFF(m,MW.AsOfDate,FundWiseDate.CashManagentStartDate)=1 and MW.DataType = 'Net YTD' and Fund=FundName                        
                        
                        
Update #tempFundWiseDates Set NetITDValuePriorToITD = COALESCE(MW.Performance,0)                     
from #tempFundWiseDates FundWiseDate                        
Inner JOIN                        
(                        
select * FROM T_MW_PerformanceOnBoard MW where MW.DataType = 'Net ITD'                        
)MW                        
on DATEDIFF(m,MW.AsOfDate,FundWiseDate.CashManagentStartDate)=1 and MW.DataType = 'Net ITD' and Fund=FundName                        
                      
--Select * from #tempFundWiseDates          
          
------------------------------------------------------------------------------------------          
-----------Have Fund WISE dates for all the indexes with their values          
------------------------------------------------------------------------------------------          
select FundName,CashMgmtStartDate into #Funds from dbo.split(@Fund,',') Temp Inner JOIN T_CompanyFunds CF on CF.CompanyFundID =  Temp.items INNER JOIN T_CashPreferences CP on CP.ID = CF.CompanyFundID          
          
Select FundName,CashMgmtStartDate,Type,Indexsymbol into #PriorEnteriesForINDEXES from #Funds CROSS JOIN #FinalIndexes          
          
Alter table #PriorEnteriesForINDEXES add PrevRT float          
          
Update #PriorEnteriesForINDEXES SET PrevRT =MW.Performance          
from #PriorEnteriesForINDEXES Temp          
Inner JOIN                        
(                        
select * FROM T_MW_PerformanceOnBoard where ISBenchMark=1          
)MW                        
on DATEDIFF(m,MW.AsOfDate,Temp.CashMgmtStartDate)=1 and MW.DataType = Temp.Type and Temp.FundName=MW.Fund          
          
--Select * from #PriorEnteriesForINDEXES                      
                        
------------------------------------------------------------------------------------------          
---------------Update final values QTD for Gross and Net                        
------------------------------------------------------------------------------------------          
Update #Result set Value = ((1+COALESCE(Value,0))*(1+COALESCE(GrossQTDValuePriorToITD,0)))-1                        
from #Result R Inner join #tempFundWiseDates FWD                        
on R.Fund_Name = FWD.FundName and R.Datatype = 'Gross QTD'   and Year(@ITD) = Year(ToDate) and Month(ToDate) not in(1,4,7,10) and month(tODate)<=month(@ITD)                      
                         
Update #Result set Value = ((1+COALESCE(Value,0))*(1+COALESCE(NetQTDValuePriorToITD,0)))-1                        
from #Result R Inner join #tempFundWiseDates FWD                        
on R.Fund_Name = FWD.FundName and R.Datatype = 'Net QTD'   and Year(@ITD) = Year(ToDate)   and Month(ToDate) not in(1,4,7,10) and month(tODate)<=month(@ITD)                      
          
Update #Result set Value = ((1+COALESCE(Value,0))*(1+COALESCE(PrevRT,0)))-1                        
from #Result R Inner join #PriorEnteriesForINDEXES FWD                        
on R.Fund_Name = FWD.FundName and R.Datatype = FWD.Type and R.Datatype like '%QTD%' and ISindex=1 and Year(@ITD) = Year(ToDate) and Month(ToDate) not in(1,4,7,10) and month(tODate)<=month(@ITD)          
          
          
------------------------------------------------------------------------------------------                        
---------------Update final values YTD for Gross and Net                        
------------------------------------------------------------------------------------------          
Update #Result set Value = ((1+COALESCE(Value,0))*(1+COALESCE(GrossYTDValuePriorToITD,0)))-1                        
from #Result R Inner join #tempFundWiseDates FWD                        
on R.Fund_Name = FWD.FundName and R.Datatype = 'Gross YTD' and Year(@ITD) = Year(ToDate)                      
                        
Update #Result set Value = ((1+COALESCE(Value,0))*(1+COALESCE(NetYTDValuePriorToITD,0)))-1                        
from #Result R Inner join #tempFundWiseDates FWD                        
on R.Fund_Name = FWD.FundName and R.Datatype = 'Net YTD' and Year(@ITD) = Year(ToDate)                          
          
Update #Result set Value = ((1+COALESCE(Value,0))*(1+COALESCE(PrevRT,0)))-1                        
from #Result R Inner join #PriorEnteriesForINDEXES FWD                        
on R.Fund_Name = FWD.FundName and R.Datatype = FWD.Type and R.Datatype like '%YTD%' and ISindex=1 and Year(@ITD) = Year(ToDate)          
                        
------------------------------------------------------------------------------------------          
---------------Update final values ITD for Gross and Net                        
------------------------------------------------------------------------------------------          
Update #Result set Value = ((1+COALESCE(Value,0))*(1+COALESCE(GrossITDValuePriorToITD,0)))-1                        
from #Result R Inner join #tempFundWiseDates FWD                        
on R.Fund_Name = FWD.FundName and R.Datatype = 'Gross ITD'                          
                        
Update #Result set Value = ((1+COALESCE(Value,0))*(1+COALESCE(NetITDValuePriorToITD,0)))-1                        
from #Result R Inner join #tempFundWiseDates FWD                        
on R.Fund_Name = FWD.FundName and R.Datatype = 'Net ITD'               
          
Update #Result set Value = ((1+COALESCE(Value,0))*(1+COALESCE(PrevRT,0)))-1                        
from #Result R Inner join #PriorEnteriesForINDEXES FWD                        
on R.Fund_Name = FWD.FundName and R.Datatype = FWD.Type and R.Datatype like '%ITD%' and ISindex=1          
        
alter table #Result add TableDescription varchar(50)        
        
Update #Result set TableDescription = 'Table1'        
        
        
------------------------------------------------------------------------------------------          
---------------Symbol Exposures        
------------------------------------------------------------------------------------------          
        
declare @NAVEXP table        
(        
 FundID INT,        
 Fundname varchar(max),        
 Nav float        
)        
        
insert into @NAVEXP        
select distinct CF.CompanyFundID,fund, sum(EndingMarketValuebase) from t_mw_genericpnl        
Inner JOIN T_CompanyFunds CF on CF.FundName = t_mw_genericpnl.Fund                        
where open_closetag<>'C' and CF.CompanyFundID in (select * from dbo.split(@fund,',')) and rundate = @toDate        
group by fund,CF.CompanyFundID        
        
declare @tEXP table        
(        
 FundID INT,        
 Fund varchar(max),        
 Symbol varchar(max),        
 SecurityName varchar(max),        
 Exposure float        
)        
        
insert into @tEXP        
select CF.CompanyFundID,Fund,Symbol, securityName, sum(Endingmarketvaluebase)  from t_mw_genericpnl         
Inner JOIN T_CompanyFunds CF on CF.FundName = t_mw_genericpnl.Fund        
where open_closetag='O' and CF.CompanyFundID in (select * from dbo.split(@fund,',')) and rundate = @toDate and Asset<>'Cash'        
group by Symbol, SecurityName,fund,CF.CompanyFundID        
        
--select Fund, Symbol,SecurityName,Exposure/Nav as Exposure from @t inner join @NAV on Fund=Fundname        
        
------------------------------------------------------------------------------------------        
---------------Symbol Exposures columns in the final results table        
------------------------------------------------------------------------------------------        
        
alter table #Result add         
 Symbol varchar(max),        
 SecurityName varchar(max),        
 Exposure float        
        
Insert into #Result(Fund,Fund_Name,Symbol,SecurityName,Exposure,TableDescription)         
select tempT.FundID,Fund, Symbol,SecurityName,Exposure/Nav as Exposure,'Table2' as TableDescription from @tEXP as tempT inner join @NAVEXP on Fund=Fundname        
        
        
------------------------------------------------------------------------------------------          
---------------Symbol PNL        
------------------------------------------------------------------------------------------          
        
declare @MTDFromdate datetime        
select @MTDFromdate = CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@toDate)-1),@toDate),101)        
        
        
declare @NAVPNL table        
(        
 FundID INT,        
 Fundname varchar(max),        
 Nav float        
)        
        
insert into @NAVPNL         
select distinct CF.CompanyFundID,fund, sum(EndingMarketValuebase) from t_mw_genericpnl         
Inner JOIN T_CompanyFunds CF on CF.FundName = t_mw_genericpnl.Fund        
where open_closetag<>'C' and rundate = dbo.AdjustBusinessDays(@MTDFromdate,-1,11)        
and CF.CompanyFundID in (select * from dbo.split(@fund,','))         
group by fund,CF.CompanyFundID        
        
declare @tPNL table        
(        
 FundID INT,        
 Fund Varchar(max),        
 Symbol varchar(max),        
 SecurityName varchar(max),        
 Side varchar(max),        
 PnL float        
)        
        
insert into @tPNL        
select CF.CompanyFundID,Fund, Symbol, SecurityName,Side,sum(TotalRealizedPnLMTM+TotalUnrealizedPnLMTM+Dividend)  from T_MW_Genericpnl        
Inner JOIN T_CompanyFunds CF on CF.FundName = t_mw_genericpnl.Fund        
Where CF.CompanyFundID in (select * from dbo.split(@fund,',')) and rundate between @MTDFromdate and @toDate        
group by Symbol, SecurityName, Side,Fund,CF.CompanyFundID        
        
--select Fund, Symbol,SecurityName,Side,PnL/NaV as PnL from @t inner join @NAV on Fund=Fundname        
        
------------------------------------------------------------------------------------------        
---------------Symbol PNL columns in the final results table        
------------------------------------------------------------------------------------------        
        
alter table #Result add         
 Side varchar(max),        
 PnL float        
        
Insert into #Result(Fund,Fund_Name,Symbol,SecurityName,Side,PnL,TableDescription)         
select tempT.FundID,Fund, Symbol,SecurityName,Side,PnL/Nav as PnL,'Table3' as TableDescription from @tPNL as tempT inner join @NAVPNL on Fund=Fundname        
        
        
                       
                        
Select * from #Result                        
where ToDate >=  dbo.AdjustBusinessDays(DATEADD(yy, DATEDIFF(yy,0,@ToDate), 0),-1,11) or TableDescription not like 'Table1' --order by ToDate                        
                        
Drop table #tempFundWiseDates,#Result,#Dimensions,#Indexes,#FinalIndexes,#PriorEnteriesForINDEXES,#Funds      
                        
--select * from #Result                                               
--where ToDate >=  dbo.AdjustBusinessDays(DATEADD(yy, DATEDIFF(yy,0,@ToDate), 0),-1,11)--order by ToDate                        
--                                              
--drop table #Result,#FundTable eport_AllFund_Indices_Test