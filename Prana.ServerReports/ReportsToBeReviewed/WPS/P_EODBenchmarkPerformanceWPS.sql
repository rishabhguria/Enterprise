-- =============================================        
-- Author:  <Pankaj Sharma>        
-- Create date: <7/13/2015>        
-- Description: <returns a table for benchmark Performance given the symbols>        
-- 
-- exec [P_EODBenchmarkPerformanceWPS] '2015/04/01','2015/01/02', 'DJI,COMP,IUX,SPX'        
--        
-- =============================================        
Create proc [dbo].[P_EODBenchmarkPerformanceWPS]        
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
YTDDate Datetime        
        
)        
    
 -- Fill the table variable with the rows for your result set        
        
declare @MTDFromdate datetime        
select @MTDFromdate =  CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@todate)-1),@todate),101)        
        
declare @YTDFromdate datetime        
select @YTDFromdate =  DATEADD(yy, DATEDIFF(yy,0,@todate), 0)        
        
insert into @result(rSymbol)        
select * from dbo.split(@Benchmarks, ',')         
    
select Symbol,Date,[Close] into #TempIndexHelperTable    
from [Historical_Pankaj].[dbo].DailyBars where Date <= @toDate and symbol in (select rSymbol from @result)    
    
--select T.name,T.symbol,T.exchange into #TempIndexHelperTable_1     
--from [VEDA].[VEDA_SharedDAta].[dbo].T_EODFundamentals T     
--where T.exchange = 'INDEX' and symbol in (select rSymbol from @result)    
        
--original PSR column for benchmark  day price is retarded and using @todate's price        
update @result set toDateDate = B.Date        
from @result A join        
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
        
update @result set toDatePrice = [close] from #TempIndexHelperTable where Date = toDateDate   and symbol = rSymbol        
update @result set DayPrice = [close] from #TempIndexHelperTable where Date = dayDate   and symbol = rSymbol        
update @result set MTDPrice = [close] from #TempIndexHelperTable where Date = MTDDate   and symbol = rSymbol        
update @result set ITDPrice = [close] from #TempIndexHelperTable where Date = ITDDate   and symbol = rSymbol        
update @result set YTDPrice = [close] from #TempIndexHelperTable where Date = YTDDate   and symbol = rSymbol        
        
update @result set         
[Day] = (todateprice - dayPrice)/ isnull(dayPrice, 0),        
MTD = (todateprice - MTDPrice)/ isnull(MTDPrice,0),        
ITD = (todateprice - ITDPrice)/ isnull(ITDPrice,0),        
YTD = (todateprice - YTDPrice)/ isnull(YTDPrice,0)        
        
--update @result set         
--name = F.name         
--from @result R join #TempIndexHelperTable_1 F on R.rSymbol = F.symbol         
        
select * from @result        
    
END 