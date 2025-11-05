-- =============================================      
-- Author:  <Alan Hau>      
-- Create date: <2/4/2011>      
-- Description: <returns a table for benchmark Performance given the symbols>      
-- Sample:  select * from F_D_BenchmarkPerformance('1/31/2011', '$SPX,$OIX')      
-- exec [P_EODBenchmarkWPSPerformance] '2015/04/01','2015/01/02', 'DJI,COMP,IUX,SPX'      
--      
-- For a complete list of Index choices, see:      
--  select * from dbo.T_EODFundamentals where exchange = 'INDEX'      
-- =============================================      
CREATE proc [dbo].[P_EODBenchmarkWPSPerformance]      
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
      
--declare @QTDFromdate datetime      
--select @QTDFromdate =  DATEADD(qq, DATEDIFF(qq,0,@todate), 0)      
      
declare @YTDFromdate datetime      
select @YTDFromdate =  DATEADD(yy, DATEDIFF(yy,0,@todate), 0)      
      
insert into @result(rSymbol)      
select * from dbo.split(@Benchmarks, ',')       
      
--update @result set       
--[day] = dbo.F_EODsimpleReturn(@todate, @todate, Symbol),      
--MTD = dbo.F_EODsimpleReturn(@MTDFromdate, @todate, Symbol),      
--QTD = dbo.F_EODsimpleReturn(@QTDFromdate, @todate, Symbol),      
--YTD = dbo.F_EODsimpleReturn(@YTDFromdate, @todate, Symbol)      
  
select Symbol,exchange,asofdate,closeprice into #TempIndexHelperTable  
from [VEDA].[VEDA_SharedDAta].[dbo].T_TS_EODHistDataDump where exchange = 'INDEX' and asofDate <= @toDate and symbol in (select rSymbol from @result)  
  
select T.name,T.symbol,T.exchange into #TempIndexHelperTable_1   
from [VEDA].[VEDA_SharedDAta].[dbo].T_EODFundamentals T   
where T.exchange = 'INDEX' and symbol in (select rSymbol from @result)  
      
--original PSR column for benchmark  day price is retarded and using @todate's price      
update @result set toDateDate = B.asofdate      
from @result A join      
(select symbol, max(asofdate) as asofdate from #TempIndexHelperTable where asofDate <= @toDate and exchange = 'INDEX' and symbol in (select rSymbol from @result) group by symbol ) B on B.symbol = A.rsymbol      
      
update @result set DayDate = B.asofdate      
from @result A join      
(select symbol, max(asofdate) as asofdate from #TempIndexHelperTable where asofDate <= Dateadd(dd,-1,@todate) and exchange = 'INDEX' and symbol in (select rSymbol from @result) group by symbol ) B on B.symbol = A.rsymbol      
      
      
update @result set MTDDate = B.asofdate      
from @result A join      
(select symbol, max(asofdate) as asofdate from #TempIndexHelperTable where asofDate <= Dateadd(dd,-1,@MTDFromdate) and exchange = 'INDEX' and symbol in (select rSymbol from @result) group by symbol ) B on B.symbol = A.rsymbol      
      
update @result set ITDDate = B.asofdate      
from @result A join      
(select symbol, max(asofdate) as asofdate from #TempIndexHelperTable where asofDate <= Dateadd(dd,-1,@ITD) and exchange = 'INDEX' and symbol in (select rSymbol from @result) group by symbol ) B on B.symbol = A.rsymbol      
      
update @result set YTDDate = B.asofdate      
from @result A join      
(select symbol, max(asofdate) as asofdate from #TempIndexHelperTable where asofDate <= Dateadd(dd,-1,@YTDFromdate) and exchange = 'INDEX' and symbol in (select rSymbol from @result) group by symbol ) B on B.symbol = A.rsymbol      
      
update @result set toDatePrice = closeprice from #TempIndexHelperTable where asofDate = toDateDate and exchange = 'INDEX' and symbol = rSymbol      
update @result set DayPrice = closeprice from #TempIndexHelperTable where asofDate = dayDate and exchange = 'INDEX' and symbol = rSymbol      
update @result set MTDPrice = closeprice from #TempIndexHelperTable where asofDate = MTDDate and exchange = 'INDEX' and symbol = rSymbol      
update @result set ITDPrice = closeprice from #TempIndexHelperTable where asofDate = ITDDate and exchange = 'INDEX' and symbol = rSymbol      
update @result set YTDPrice = closeprice from #TempIndexHelperTable where asofDate = YTDDate and exchange = 'INDEX' and symbol = rSymbol      
      
update @result set       
[Day] = (todateprice - dayPrice)/ isnull(dayPrice, 0),      
MTD = (todateprice - MTDPrice)/ isnull(MTDPrice,0),      
ITD = (todateprice - ITDPrice)/ isnull(ITDPrice,0),      
YTD = (todateprice - YTDPrice)/ isnull(YTDPrice,0)      
      
update @result set       
name = F.name       
from @result R join #TempIndexHelperTable_1 F on R.rSymbol = F.symbol and F.exchange = 'INDEX'      
      
select * from @result      
  
END 