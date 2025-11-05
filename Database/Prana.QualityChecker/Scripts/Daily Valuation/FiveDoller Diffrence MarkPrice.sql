
Declare @FromDate Datetime
Declare @ToDate Datetime
Declare @errormsg varchar(max)

--set @ToDate='12/21/2018'

set @ToDate=''
set @errormsg=''

Set @FromDate=dbo.AdjustBusinessDays(@ToDate,-1,1)

Select mp1.symbol,
mp1.finalmarkprice as Markprice1,
mp2.finalmarkprice as Markprice2,
mp1.Fundid as fundid,
mp1.Date as Fdate
into #tempMarkprice
From PM_Daymarkprice mp1
join 
PM_Daymarkprice mp2
on mp1.symbol= mp2.symbol and abs(mp1.finalmarkprice - mp2.finalmarkprice) >= 5 
and mp1.Date=@FromDate and mp2.Date=@ToDate


IF EXISTS(Select * from #tempMarkprice) 
Begin 
SELECT Symbol, Markprice1,Markprice2,fundid,Fdate from #tempMarkprice
Set @errormsg='More then five doller Diffrence in Mark Price for The Symbol in previous Date' 
END 

SELECT @errormsg AS ErrorMsg 

DROP TABLE #tempMarkprice