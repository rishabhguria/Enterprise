Declare	@FromDate Datetime 
Declare @ToDate Datetime

Declare @ErrorMsg Varchar(Max)
Set @ErrorMsg=''


set @FromDate='' 
set @ToDate='' 

if (@FromDate='')
begin 
Set @FromDate=@ToDate
end

select [Date],Symbol,Fundid,FinalMarkprice into #TempData from PM_Daymarkprice 
where fundid<> 0 
AND  [Date] between @FromDate and @ToDate  

select * from #tempdata
IF EXISTS(Select * from #TempData)
Begin
set @errormsg='Data Is present in both Symbol as well as FundWise'
END

Select @errormsg as ErrorMsg
Drop Table #TempData


