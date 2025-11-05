-- =============================================                            
-- Author:  <Pankaj Sharma>                            
-- Create date: <7/13/2015>                            
-- Description: <returns a table for benchmark Performance given the symbols>                            
--                     
-- exec [P_EODBenchmarkPerformanceWPS] '2015/07/07','2014/10/14', '$SPX'                            
--                            
-- =============================================                            
create proc [dbo].[P_EODBenchmarkPerformanceWPS]                            
(                            
@ToDate Datetime,                          
@ITD  Datetime,                            
@Benchmarks Varchar(max)                          
)                            
as                        
Begin                        
                        
DECLARE @result table                          
(                            
rSymbol varchar(200),                            
Name varchar(200),                            
toDatePrice float,                            
toDateDate Datetime,                            
[day] float,                            
dayPrice float,                            
dayDate datetime,                            
MTD float,                            
MTDPrice float,                            
MTDDate Datetime,                            
ITD float,                            
ITDPrice float,                            
ITDDate datetime,                            
YTD float,                            
YTDPrice float,                            
YTDDate Datetime,           
QTD float,                            
QTDPrice float,                            
QTDDate Datetime                          
                            
)                            
                        
 -- Fill the table variable with the rows for your result set                            
                            
declare @MTDFromdate datetime                            
select @MTDFromdate =  CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@todate)-1),@todate),101)                            
  
select @MTDFromdate =   
  case when datediff(d,@MTDFromdate,@ITD)>0  
   then @ITD  
  else @MTDFromdate    
end  
    
                     
declare @YTDFromdate datetime                            
select @YTDFromdate =  DATEADD(yy, DATEDIFF(yy,0,@todate), 0)            
  
select @YTDFromdate =   
  case when datediff(d,@YTDFromdate,@ITD)>0  
   then @ITD  
  else @YTDFromdate    
end  
  
        
declare @QTDFromdate datetime                            
select @QTDFromdate =  DATEADD(qq, DATEDIFF(qq,0,@todate), 0)                            
select @QTDFromdate =   
  case when datediff(d,@QTDFromdate,@ITD)>0  
   then @ITD  
  else @QTDFromdate    
end  
                        
                            
insert into @result(rSymbol)                            
select * from dbo.split(@Benchmarks, ',')                      
                  
-------------------------fixed index value for particular date----                  
create table #TempIndexHardCode                  
(Symbol varchar(200),                  
Date Datetime,                  
[Close] Float                  
)                  
                  
                  
---------all required values ---------------                  
create table #TempIndexHelperTable                  
(Symbol varchar(200),                  
Date Datetime,                  
[Close] Float                  
)                  
                  
                  
                  
DECLARE @rsymbol varchar(200)                  
select @rSymbol =  rsymbol from @result                  
                  
-----------------insert fixed value------------                  
insert INTO  #TempIndexHardCode VALUES(@rSymbol,'10/13/2014',3422.43 )                   
insert INTO  #TempIndexHardCode VALUES(@rSymbol,'12/31/2014',3769.440 )                      
                  
declare @date datetime                  
select @date=  min(date) from [Historical].[Historicals].[dbo].DailyBars where  symbol  in                   
(select rSymbol from @result)                   
and Date <= @toDate                   
                 
              
              
                  
insert INTO #TempIndexHelperTable                   
SELECT DB.symbol,DB.DATE,coalesce(IHC.[Close],DB.[Close],0) from #TempIndexHardCode IHC                   
full OUTER join [Historical].[Historicals].[dbo].DailyBars DB                   
on datediff(d,DB.date,IHC.date)=0                  
where  DB.symbol in (select rSymbol from @result) and Db.date<=@todate                  
                  
--insert INTO #TempIndexHelperTable                   
--SELECT IHC.symbol,IHC.DATE,IHC.[Close] from #TempIndexHardCode IHC                   
--left OUTER join Historical_Pankaj.[dbo].DailyBars DB on datediff(d,DB.date,IHC.date)=0                  
--where  DB.symbol in (select rSymbol from @result) and IHC.date<=@todate                  
--                  
--insert INTO #TempIndexHelperTable                   
--SELECT DB.symbol,DB.DATE,DB.[Close] from #TempIndexHardCode IHC                   
--right OUTER join Historical_Pankaj.[dbo].DailyBars DB on datediff(d,DB.date,IHC.date)!=0                  
--where  DB.symbol in (select rSymbol from @result) and IHC.date<=@todate                  
----select * from #TempIndexHelperTable                  
                       
                    
--select T.name,T.symbol,T.exchange into #TempIndexHelperTable_1                         
--from [VEDA].[VEDA_SharedDAta].[dbo].T_EODFundamentals T                         
--where T.exchange = 'INDEX' and symbol in (select rSymbol from @result)                        
                            
--original PSR column for benchmark  day price is retarded and using @todate's price                            
update @result set toDateDate = B.Date                from @result A join                            
(select symbol, max(Date) as Date from #TempIndexHelperTable where Date <= @toDate   and symbol in (select rSymbol from @result) group by symbol ) B on B.symbol = A.rsymbol                            
                            
update @result set DayDate = B.Date                            
from @result A join                            
(select symbol, max(Date) as Date from #TempIndexHelperTable where Date <= Dateadd(dd,-1,@todate)   and symbol in (select rSymbol from @result) group by symbol ) B on B.symbol = A.rsymbol                            
                            
                            
update @result set MTDDate = B.Date                            
from @result A join                            
(select symbol, max(Date) as Date from #TempIndexHelperTable where Date <= Dateadd(dd,-1,@MTDFromdate)   and symbol in (select rSymbol from @result) group by symbol ) B on B.symbol = A.rsymbol                            
                            
update @result set ITDDate = B.Date                            
from @result A join                            
(select symbol, max(Date) as Date from #TempIndexHelperTable where Date <= Dateadd(dd,-1,@ITD)   and symbol in (select rSymbol from @result) group by symbol ) B on B.symbol = A.rsymbol                            
                            
update @result set YTDDate = B.Date                            
from @result A join                            
(select symbol, max(Date) as Date from #TempIndexHelperTable where Date <= Dateadd(dd,-1,@YTDFromdate)   and symbol in (select rSymbol from @result) group by symbol ) B on B.symbol = A.rsymbol                            
        
update @result set QTDDate = B.Date                            
from @result A join                            
(select symbol, max(Date) as Date from #TempIndexHelperTable where Date <= Dateadd(dd,-1,@QTDFromdate)   and symbol in (select rSymbol from @result) group by symbol ) B on B.symbol = A.rsymbol                            
        
        
update @result set toDatePrice = [close] from #TempIndexHelperTable where Date = toDateDate   and symbol = rSymbol                            
update @result set DayPrice = [close] from #TempIndexHelperTable where Date = dayDate   and symbol = rSymbol                            
update @result set MTDPrice = [close] from #TempIndexHelperTable where Date = MTDDate   and symbol = rSymbol                            
update @result set ITDPrice = [close] from #TempIndexHelperTable where Date = ITDDate   and symbol = rSymbol                            
update @result set YTDPrice = [close] from #TempIndexHelperTable where Date = YTDDate   and symbol = rSymbol                            
update @result set QTDPrice = [close] from #TempIndexHelperTable where Date = QTDDate   and symbol = rSymbol                            
        
        
update @result set                             
[Day] = (todateprice - dayPrice)/ isnull(dayPrice, 0),                            
MTD = (todateprice - MTDPrice)/ isnull(MTDPrice,0),                            
ITD = (todateprice - ITDPrice)/ isnull(ITDPrice,0),                            
YTD = (todateprice - YTDPrice)/ isnull(YTDPrice,0),                     
QTD = (todateprice - QTDPrice)/ isnull(QTDPrice,0)                     
select * from @result                     
                  
drop table #TempIndexHelperTable,  #TempIndexHardCode                       
                        
END 