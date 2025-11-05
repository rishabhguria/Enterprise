Declare @FromDate Datetime
Declare @ToDate Datetime
Declare @errormsg varchar(max)


set @FromDate=''
set @ToDate=''
set @errormsg=''


Select MP1.Symbol, MP1.FinalMarkPrice, MP1.Date, MP1.FundID into #temp
From PM_daymarkprice MP1
Join( SELECT Symbol, FinalMarkPrice, FundID 
FROM PM_DayMarkPrice
Where datediff(dd, date, @FromDate) <=0 and datediff(dd, date, @ToDate) >=0
GROUP BY Symbol, FinalMarkPrice,  FundID
HAVING COUNT(*) > 1) MP2 on MP1.Symbol = MP2.Symbol and MP1.FinalMarkPrice = MP2.FinalMarkPrice and MP1.FundID = MP2.FundID
Where datediff(dd, date, @FromDate) <=0 and datediff(dd, date, @ToDate) >=0
order by symbol,date

IF EXISTS(Select * from #temp) 
Begin 
SELECT Symbol, FinalMarkPrice, Date, FundID  from #temp
Set @errormsg='Same Mark Price for a Symbol in Different Date' 
END 

SELECT @errormsg AS ErrorMsg 

DROP TABLE #temp