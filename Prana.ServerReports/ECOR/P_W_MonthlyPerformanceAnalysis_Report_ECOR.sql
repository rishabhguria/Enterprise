
/****** Object:  StoredProcedure [dbo].[P_W_MonthlyPerformanceAnalysis_Report]    Script Date: 09/24/2015 18:04:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================      
-- Author:        <Author,,LYNN>      
-- Create date: <Create Date,,>      
-- Description:    <Description,,>      
-- Return VAMI of Fund VS. Index with daily return according to funds for a given period of time      
-- EXEC dbo.P_W_MonthlyPerformanceAnalysis_Report_TEST '09/03/2015','10/1/2014','1218'      
-- =============================================      
ALTER Procedure [dbo].[P_W_MonthlyPerformanceAnalysis_Report]      
(      
    -- Add the parameters for the function here      
    @ToDate Datetime,      
    @ITD Datetime,      
    @FundID varchar(max)       
)      
AS     
    
--Declare @ToDate Datetime      
--Declare @ITD Datetime      
--Declare @FundID varchar(max)    
--Set @ToDate =  '09/03/2015'    
--Set @ITD = '10/1/2014'    
--Set @FundID = '1218'    
    
Declare @T_FundIDs Table                                                                                                                                                
(                                                                                                                                                
 FundId int                                                                                                                                                
)                                                                                                                                                
Insert Into @T_FundIDs Select * From dbo.Split(@FundID, ',')            
    
Declare @T_CompanyFunds Table                                                                                                                                                
(                                                                                                                     
 CompanyFundID int,                                                                                                            
 FundName varchar(50)                  
)                                                      
Insert Into @T_CompanyFunds                                                            
Select                                                                                                                             
CompanyFundID,                                                                                      
FundName                                                                                      
From T_CompanyFunds     
INNER JOIN @T_FundIDs FundIDs ON T_CompanyFunds.CompanyFundID = FundIDs.FundID     
    
      
DECLARE @YTD DATETIME      
SELECT @YTD=DATEADD(yy, DATEDIFF(yy,0,@ToDate), 0)      
      
select @YTD =      
Case       
When @YTD>= @ITD Then @YTD      
Else @ITD      
End      
      
      
Declare @Date table      
(      
 Year varchar(max),      
 Month varchar(max),      
 Day varchar(max),      
 Date datetime      
)      
      
insert into @Date (Year)       
select number from master..spt_values      
where type = 'P' and number >= year (@ITD) and number < year (@YTD)      
      
update @Date set Date = '12/31/'+ Year       
insert into @Date (Year,Month)      
select Year(@ToDate),number from master..spt_values      
where type = 'P' and number between month(@YTD) and month(@ToDate)      
      
update @Date set Day =      
Case      
WHEN Month= 1 THEN 31      
WHEN Month= 2 THEN CASE WHEN Year % 4 = 0 THEN 29 ELSE 28 END      
WHEN Month= 3 THEN 31      
WHEN Month= 4 THEN 30      
WHEN Month= 5 THEN 31      
WHEN Month= 6 THEN 30      
WHEN Month= 7 THEN 31      
WHEN Month= 8 THEN 31      
WHEN Month= 9 THEN 30      
WHEN Month= 10 THEN 31      
WHEN Month= 11 THEN 30      
WHEN Month= 12 THEN 31      
End      
      
update @Date Set Date=      
(select CAST(CAST(Year AS varchar) + '-' + CAST(Month AS varchar) + '-' + CAST(Day AS varchar) AS DATETIME)) where Date is null      
      
update @Date set Date=@ToDate where Date=(select max(Date) from @Date)      
update @Date set Date = dbo.AdjustBusinessDays(Date,-1,11) where dbo. isbusinessday(Date,1)=0 --set date to previous business day.      
      
      
Declare @Type table      
(      
 Type varchar(max),      
 IndexSymbol varchar(max),      
 IsIndex int      
)      
insert into @Type Values ('GROSS MTD',null,0 )      
insert into @Type Values ('GROSS QTD',null,0 )      
insert into @Type Values ('GROSS YTD',null,0)      
insert into @Type Values ('GROSS ITD',null,0)      
insert into @Type Values ('NET MTD',null,0)      
insert into @Type Values ('NET QTD',null,0)      
insert into @Type Values ('NET YTD',null,0)      
insert into @Type Values ('NET ITD',null,0)      
insert into @Type Values ('NET EXP.',null,0)      
insert into @Type Values ('GROSS EXP.',null,0)      
insert into @Type Values ('AUM',null,0)      
insert into @Type Values ('Russell MTD','$RUTTR',1)--'$RUT'      
insert into @Type Values ('Russell QTD','$RUTTR',1)      
insert into @Type Values ('Russell YTD','$RUTTR',1)      
insert into @Type Values ('Russell ITD','$RUTTR',1)      
--insert into @Type Values ('NASDAQ MTD','$XCMP',1)--$COMPQ      
--insert into @Type Values ('NASDAQ QTD','$XCMP',1)      
--insert into @Type Values ('NASDAQ YTD','$XCMP',1)      
--insert into @Type Values ('NASDAQ ITD','$XCMP',1)      
insert into @Type Values ('S&P 500 MTD','PXT A0',1)--'$SPX'      
insert into @Type Values ('S&P 500 QTD','PXT A0',1)      
insert into @Type Values ('S&P 500 YTD','PXT A0',1)      
insert into @Type Values ('S&P 500 ITD','PXT A0',1)      
--insert into @Type Values ('Russell TECH MTD','$R3RGSTEC',1)--'$R3RGSTEC'      
--insert into @Type Values ('Russell TECH QTD','$R3RGSTEC',1)      
--insert into @Type Values ('Russell TECH YTD','$R3RGSTEC',1)      
--insert into @Type Values ('Russell TECH ITD','$R3RGSTEC',1) 

insert into @Type Values ('NASDAQ BIOTECH MTD','$NBI',1)--'$NBI'      
insert into @Type Values ('NASDAQ BIOTECH QTD','$NBI',1)      
insert into @Type Values ('NASDAQ BIOTECH YTD','$NBI',1)      
insert into @Type Values ('NASDAQ BIOTECH ITD','$NBI',1)

insert into @Type Values ('NYSE ARCA BIOTECHNOLOGY MTD','$BTKTR',1)--'$BTKTR'      
insert into @Type Values ('NYSE ARCA BIOTECHNOLOGY QTD','$BTKTR',1)      
insert into @Type Values ('NYSE ARCA BIOTECHNOLOGY YTD','$BTKTR',1)      
insert into @Type Values ('NYSE ARCA BIOTECHNOLOGY ITD','$BTKTR',1)  
     
insert into @Type Values ('FAIR VALUE LONGS',null,null)      
insert into @Type Values ('FAIR VALUE SHORTS',null,null)      
      
      
      
CREATE  table #Result      
(      
 Fund varchar(max),      
 Year varchar(4) ,      
 FromDate datetime,      
 ToDate datetime,      
 DataType varchar(max),      
 Value float,      
 IndexSymbol varchar(max),      
 ISIndex int,      
 FromDatePrice float,      
 ToDatePrice float,      
 ID int identity(1,1)      
)      
      
insert into #Result(Year,ToDate,DataType,IndexSymbol,IsIndex)      
select Year,Date,Type,IndexSymbol,IsIndex from @Date  left outer join @Type on 1=1      
      
--select * from #Result order by ISIndex      
----------------------Add Index for on board dates--------------------------------------------------------------------      
insert into #Result(Year,ToDate,DataType,IndexSymbol,IsIndex)      
select distinct Year(asofdate),asofdate,Type,IndexSymbol,IsIndex from T_MW_PerformanceOnBoard  left outer join @Type on 1=1      
and Fund in (Select FundName From @T_CompanyFunds) and ISIndex=1     
    
--Drop table #Result     
------------------------------------------------------------------------------------------------------------------------      
--select * from #Result Where ISIndex=1      
      
      
update #Result set FromDate = @ITD where  DataType like ('%ITD') or (DataType like ('%YTD') and Year=Year(@ITD))      
update #Result set FromDate = DATEADD(yy, DATEDIFF(yy,0,Todate), 0) where DataType like ('%YTD') --and Year <>Year(@ITD)      
update #Result set FromDate = DateAdd(qq,DateDiff(qq,0,ToDate),0) where DataType like ('%QTD') --and Year <>Year(@ITD)      
update #Result set FromDate = CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(Todate)-1),todate),101) where DataType like ('%MTD')      
--update #Result set FromDate = @ITD where FromDate < @ITD  and DataType like ('%MTD') and Year=Year(@ITD)      
      
--select * from #Result order by ISIndex      
      
      
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
      
insert into @MonthlyRT      
select Companyfund,day1,  day2,Pnl,ExpFee, CF,IncentiveFee,ManagementFee,GrossRT,NetRT,ID from dbo.F_MW_MonthlySimpleRT(@ToDate, @FundID) order by day2      
      
update #Result set Value=      
(select Exp(Sum(Log(GrossRT+1)))-1 from @MonthlyRT  where day2<=Todate and Day1>=fromdate) where IsIndex=0 and  Datatype like '%Gross%'      
      
update #Result set Value=      
(select Exp(Sum(Log(NetRT+1)))-1 from @MonthlyRT  where day2<=Todate and Day1>=fromdate) Where IsIndex=0 and Datatype like '%Net%'      
      
--update #Result set Value= dbo.F_MW_GetLinkedMDReturn(FromDate,ToDate, 'Fund', @fund) where ISIndex=0  and Datatype like 'Gross%'      
--update #Result set Value= dbo.F_MW_GetLinkedMDReturnNOF(FromDate,ToDate, 'Fund', @fund) where ISIndex=0  and Datatype like 'Net%'      
-----------------------------------------------------------------------------------------------------------------------      
update #Result set FromDate = dbo.AdjustBusinessDays(FromDate,-1,11) --where dbo. isbusinessday(FromDate,1)=0 --set Fromdate to previous business day.      
      
--declare @NaV float      
--select @NaV= Sum(EndingMarketValueBase) from t_mw_genericpnl where open_closetag='O' and fund in (select * from dbo.split(@fund,','))      
--and rundate=@ToDate      
      
Update #Result set Value=       
(select Sum(EndingMarketValueBase) from t_mw_genericpnl where open_closetag<>'C' and fund in (Select FundName From @T_CompanyFunds)      
and rundate=ToDate) WHERE DataType = 'AUM'       
      
update #Result set Value=       
(select sum(EndingMarketValuebase)  from t_mw_genericpnl where (open_closetag='O' and Asset<>'Cash')  and fund in (Select FundName From @T_CompanyFunds)      
and rundate=Todate and sideMultiplier=1) WHERE DataType='FAIR VALUE LONGS' --or (Open_closetag='Accruals')      
      
update #Result set Value=       
(select sum(EndingMarketValuebase)  from t_mw_genericpnl where (open_closetag='O' and Asset<>'Cash')  and fund in (Select FundName From @T_CompanyFunds)      
and rundate=Todate and sideMultiplier=-1) WHERE DataType='FAIR VALUE SHORTS' --or (Open_closetag='Accruals')      
      
--update #Result set Value=       
--(select sum(EndingMarketValuebase)  from t_mw_genericpnl where ((open_closetag='O' and Asset<>'Cash') or (Open_closetag='Accruals')) and fund in (select * from dbo.split(@fund,','))      
--and rundate=Todate) WHERE DataType='NET EXP.'      
--      
--update #Result set Value=       
--(select sum(ABS(EndingMarketValuebase))  from t_mw_genericpnl where ((open_closetag='O' and Asset<>'Cash') or (Open_closetag='Accruals')) and fund in (select * from dbo.split(@fund,','))      
--and rundate=Todate) WHERE DataType='GROSS EXP.'      
      
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
update @ExpPct set GrossExp= (select sum(abs(Value)) from #Result Where datatype Like ('FAIR VALUE%') and Date=ToDate) --and Fund=FundName       
update @ExpPct set FundAum=Value from #Result Where datatype='AUM' and Date=ToDate --and Fund=FundName       
      
update #Result set Value=      
(select Case When FundAum <> 0 Then NetExp/FundAum Else 0 End from @ExpPct Where Date=ToDate  )where DataType='Net Exp.'--and Fund=FundName      
      
update #Result set Value=      
(select Case When FundAum <> 0 Then GrossExp/FundAum Else 0 End from @ExpPct Where Date=ToDate )where DataType='Gross Exp.'--and Fund=FundName       
--select * from @ExpPct      
      
      
      
update #Result set FromDatePrice=      
(select [close] from [Historical].[Historicals].[dbo].dailybars where Date=FromDate and Symbol=IndexSymbol and IsIndex=1)      
      
update #Result set ToDatePrice=      
(select [close] from [Historical].[Historicals].[dbo].dailybars where Date=ToDate and Symbol=IndexSymbol and IsIndex=1)      
    
update #Result set Value=ToDatePrice/FromDatePrice-1 where IsIndex=1      
      
      
-------------------insert all performance brought over----------------------------------------------------------------      
--delete from #Result where  datatype  in ('Russell ITD','NASDAQ ITD','S&P 500 ITD')      
insert into #Result (Fund, Year, Value, ToDate, datatype,IsIndex)      
select Fund, Year(asofDate), Performance,asofDate,DataType,IsBenchmark from T_MW_PerformanceOnBoard       
Where Fund in (Select FundName From @T_CompanyFunds)  and DataType not in ('Russell ITD','NASDAQ ITD','S&P 500 ITD','Russell TECH ITD','NASDAQ BIOTECH ITD','NYSE ARCA BIOTECHNOLOGY ITD') order by asofdate      
--------------------------------------------------------------------------------------------------------------------------------      
UPDATE #Result SET ISINDEX=2 WHERE DATATYPE LIKE ('FAIR VALUE%')      
      
-------------------ITD Handling--------------------------------------------------      
CREATE  table #ITDHandling      
(      
 FundName varchar(max),      
 FromDate datetime,      
 Date datetime,      
 OnBoardDate datetime,      
 RT float,      
 RTOnBoard float,      
 DTType varchar(max)      
)      
insert into #ITDHandling (FundName, FromDate,Date,OnBoardDate,RT,DTType)      
select Fund,FromDate, ToDate,FromDate,(1+Value),DataType from #Result --dbo.AdjustBusinessDays(FromDate,-1,11)      
where DataType like '%ITD'and (ToDate>@ITD or ISIndex=1)--order by ToDate      
      
update #ITDHandling set RTOnBoard=(1+ Performance) from T_MW_PerformanceOnBoard where       
AsofDate=OnBoardDate and DTType=DataType --and FundName=Fund      
      
update #Result set Value= RT*RTOnBoard-1 from #ITDHandling where Date=ToDate and DataType=DTType      
update #Result set Value= RTOnBoard-1 from  #ITDHandling where ToDate=OnBoardDate and IsIndex=1 and datatype=dtType and datatype like '%ITD'      
      
delete from #ITDHandling      
insert into #ITDHandling (Date,RTOnboard, DTType)      
select Asofdate, Performance, datatype from dbo.T_MW_PerformanceOnBoard where isbenchmark=1      
      
update #Result set Value= RTOnBoard from  #ITDHandling       
where ToDate=Date and IsIndex=1 and datatype=dtType and datatype like '%ITD'      
      
--delete from #Result where Datatype In ('Russell TECH ITD' ,'NASDAQ BIOTECH ITD','NYSE ARCA BIOTECHNOLOGY ITD')     
      
--CREATE table #RusselTech       
--(      
--  Date datetime,      
--  DataName varchar(max),      
--  Ret float      
--)      
--      
--Declare @HardCodedIndexName varchar(max)      
      
/* Russell Tech Hard coded price */      
--Set @HardCodedIndexName = 'Russell TECH'      
--insert into #RusselTech values ( '12/31/2014', @HardCodedIndexName + ' MTD' , 0.0422 )      
--insert into #RusselTech values ( '12/31/2014', @HardCodedIndexName + ' QTD' , 0.1244 )      
--insert into #RusselTech values ( '12/31/2014', @HardCodedIndexName + ' YTD' , 0.0779 )      
--      
--insert into #RusselTech values ( '1/30/2015' , @HardCodedIndexName + ' MTD' , -0.0373 )      
--insert into #RusselTech values ( '1/30/2015' , @HardCodedIndexName + ' QTD' , -0.0373 )      
--insert into #RusselTech values ( '1/30/2015' , @HardCodedIndexName + ' YTD' , -0.0373 )      
--      
--insert into #RusselTech values ( '2/27/2015' , @HardCodedIndexName + ' MTD' , 0.0827 )      
--insert into #RusselTech values ( '2/27/2015' , @HardCodedIndexName + ' QTD' , 0.0423 )      
--insert into #RusselTech values ( '2/27/2015' , @HardCodedIndexName + ' YTD' , 0.0423 )      
--      
--insert into #RusselTech values ( '3/31/2015', @HardCodedIndexName + ' MTD' , 0.0087 )      
--insert into #RusselTech values ( '3/31/2015', @HardCodedIndexName + ' QTD' , 0.0513 )      
--insert into #RusselTech values ( '3/31/2015', @HardCodedIndexName + ' YTD' , 0.0513 )      
--      
--insert into #RusselTech values ( '4/30/2015' , @HardCodedIndexName + ' MTD' , -0.0196 )      
--insert into #RusselTech values ( '4/30/2015' , @HardCodedIndexName + ' QTD' , -0.0196 )      
--insert into #RusselTech values ( '4/30/2015' , @HardCodedIndexName + ' YTD' , 0.0307 )      
--      
--insert into #RusselTech values ( '5/29/2015' , @HardCodedIndexName + ' MTD' , 0.0495 )      
--insert into #RusselTech values ( '5/29/2015' , @HardCodedIndexName + ' QTD' , 0.0289 )      
--insert into #RusselTech values ( '5/29/2015' , @HardCodedIndexName + ' YTD' , 0.0817 )      
--      
--insert into #RusselTech values ( '6/30/2015' , @HardCodedIndexName + ' MTD' , -0.0097 )      
--insert into #RusselTech values ( '6/30/2015' , @HardCodedIndexName + ' QTD' , 0.0189 )      
--insert into #RusselTech values ( '6/30/2015' , @HardCodedIndexName + ' YTD' , 0.0712  )      
--      
-- insert into #RusselTech values ( '7/31/2015' , @HardCodedIndexName + ' MTD' , -0.0099 )      
-- insert into #RusselTech values ( '7/31/2015' , @HardCodedIndexName + ' QTD' , -0.0099 )      
-- insert into #RusselTech values ( '7/31/2015' , @HardCodedIndexName + ' YTD' , 0.0606  )      
--      
---- insert into #RusselTech values ( @Todate , @HardCodedIndexName + ' MTD' , -0.0099 )      
---- insert into #RusselTech values ( @Todate , @HardCodedIndexName + ' QTD' , -0.0099 )      
---- insert into #RusselTech values ( @Todate , @HardCodedIndexName + ' YTD' , 0.0606  )      
--      
--      
--/* NASDAQ Hard coded price */      
--Set @HardCodedIndexName = 'NASDAQ'      
--insert into #RusselTech values ( '12/31/2014', @HardCodedIndexName + ' MTD' , -0.0108 )      
--insert into #RusselTech values ( '12/31/2014', @HardCodedIndexName + ' QTD' , 0.0576 )      
--insert into #RusselTech values ( '12/31/2014', @HardCodedIndexName + ' YTD' , 0.1483 )      
--insert into #RusselTech values ( '12/31/2014', @HardCodedIndexName + ' ITD' , 2.2285 )      
--      
--insert into #RusselTech values ( '1/30/2015' , @HardCodedIndexName + ' MTD' , -0.0206 )      
--insert into #RusselTech values ( '1/30/2015' , @HardCodedIndexName + ' QTD' , -0.0206 )      
--insert into #RusselTech values ( '1/30/2015' , @HardCodedIndexName + ' YTD' , -0.0206 )      
--insert into #RusselTech values ( '1/30/2015' , @HardCodedIndexName + ' ITD' , 2.1620 )      
--      
--insert into #RusselTech values ( '2/27/2015' , @HardCodedIndexName + ' MTD' , 0.0730 )      
--insert into #RusselTech values ( '2/27/2015' , @HardCodedIndexName + ' QTD' , 0.0508 )      
--insert into #RusselTech values ( '2/27/2015' , @HardCodedIndexName + ' YTD' , 0.0508 )      
--insert into #RusselTech values ( '2/27/2015' , @HardCodedIndexName + ' ITD' , 2.3928 )      
--      
--insert into #RusselTech values ( '3/31/2015', @HardCodedIndexName + ' MTD' , -0.0116 )      
--insert into #RusselTech values ( '3/31/2015', @HardCodedIndexName + ' QTD' , 0.0386 )      
--insert into #RusselTech values ( '3/31/2015', @HardCodedIndexName + ' YTD' , 0.0386 )      
--insert into #RusselTech values ( '3/31/2015' , @HardCodedIndexName + ' ITD' , 2.3534 )      
--      
--insert into #RusselTech values ( '4/30/2015' , @HardCodedIndexName + ' MTD' , 0.0088 )      
--insert into #RusselTech values ( '4/30/2015' , @HardCodedIndexName + ' QTD' , 0.0088 )      
--insert into #RusselTech values ( '4/30/2015' , @HardCodedIndexName + ' YTD' , 0.0478 )      
--insert into #RusselTech values ( '4/30/2015' , @HardCodedIndexName + ' ITD' , 2.3830 )      
--      
--insert into #RusselTech values ( '5/29/2015' , @HardCodedIndexName + ' MTD' , 0.0276 )      
--insert into #RusselTech values ( '5/29/2015' , @HardCodedIndexName + ' QTD' , 0.0365 )      
--insert into #RusselTech values ( '5/29/2015' , @HardCodedIndexName + ' YTD' , 0.0767 )      
--insert into #RusselTech values ( '5/29/2015' , @HardCodedIndexName + ' ITD' , 2.4763 )      
--      
--insert into #RusselTech values ( '6/30/2015' , @HardCodedIndexName + ' MTD' , -0.0156 )      
--insert into #RusselTech values ( '6/30/2015' , @HardCodedIndexName + ' QTD' , 0.0204 )      
--insert into #RusselTech values ( '6/30/2015' , @HardCodedIndexName + ' YTD' , 0.0599  )      
--insert into #RusselTech values ( '6/30/2015' , @HardCodedIndexName + ' ITD' , 2.4221 )      
--      
-- insert into #RusselTech values ( '7/31/2015' , @HardCodedIndexName + ' MTD' , 0.0450 )      
-- insert into #RusselTech values ( '7/31/2015' , @HardCodedIndexName + ' QTD' , 0.0450 )      
-- insert into #RusselTech values ( '7/31/2015' , @HardCodedIndexName + ' YTD' , 0.1076  )      
-- insert into #RusselTech values ( '7/31/2015' , @HardCodedIndexName + ' ITD' , 2.5761  )      
--      
---- insert into #RusselTech values ( @Todate , @HardCodedIndexName + ' MTD' , 0.0450 )      
---- insert into #RusselTech values ( @Todate , @HardCodedIndexName + ' QTD' , 0.0450 )      
---- insert into #RusselTech values ( @Todate , @HardCodedIndexName + ' YTD' , 0.1076  )      
---- insert into #RusselTech values ( @Todate , @HardCodedIndexName + ' ITD' , 2.5761  )      
      
      
--update #Result set Value = Ret from #RusselTech where Todate=Date and DataType=DataName      
      
select * from #Result where ToDate >=  dbo.AdjustBusinessDays(DATEADD(yy, DATEDIFF(yy,0,@ToDate), 0),-1,11)--order by ToDate      
    
drop table #Result,#ITDHandling 