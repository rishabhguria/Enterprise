Declare	@FromDate Datetime 
Declare @ToDate Datetime

set @FromDate='' 
set @ToDate='' 

if (@FromDate='')
begin 
Set @FromDate=@ToDate
end
Declare @ErrorMsg Varchar(Max)
Set @ErrorMsg=''

select [Date],Fundid,Conversionrate into #TempData from T_CurrencyConversionRate 
where fundid<> 0 
AND  [Date] between @FromDate and @ToDate  

IF EXISTS(Select * from #TempData)
Begin
select * from #TempData
set @errormsg='Fund wise FX Rate is available in database'
END
Select @errormsg as ErrorMsg

Drop Table #TempData