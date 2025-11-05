Declare	@FromDate Datetime 
Declare @ToDate Datetime

--Declare @Smdb

set @FromDate='' 
set @ToDate='' 


Declare @ErrorMsg Varchar(Max)
Set @ErrorMsg=''

Declare @CurrentYear varchar(10)
set @CurrentYear = DATEPART(yyyy, getdate())

Declare @DaysName varchar(10)

select @DaysName = datename(dw,@ToDate) --Friday


Declare @DayID int
select @DayID = (datepart(dw,@ToDate) -1 )--6

--Select @DayID
--Select @DaysName

Create Table #HolidayeTable
(
Date datetime,
[Days Name] varchar(10),
AUECID int,
[Display Name] varchar(Max),
[Full Name] varchar(Max)
)


Select 
Distinct
AUECID,
DisplayName,
FullName
Into #TempAUEC 
from T_AUEC

Insert Into #HolidayeTable
Select
@ToDate AS [Run Date],
@DaysName As [Days Name],
TA.AUECID,
DisplayName As [Display Name],
FullName As [Full Name]
FROM  #TempAUEC TA
Inner Join T_AUECWeeklyHolidays TAWH ON TA.AUECID=TAWH.AUECID AND TAWH.WeeklyHolidayID = @DayID
Inner join T_CalendarAUEC CA ON CA.AUECID = TA.AUECID
Inner Join T_Calendar C ON C.CalendarID = CA.CalendarID
Where C.CalendarYear = @CurrentYear

--Select * from #HolidayeTable

Insert Into #HolidayeTable
Select
TAH.HolidayDate AS [Run Date],
 datename(dw,TAH.HolidayDate) As [Days Name],
TA.AUECID,
DisplayName As [Display Name],
FullName As [Full Name]
FROM  #TempAUEC TA
Inner Join T_AUECHolidays TAH 
ON TA.AUECID=TAH.AUECID 
AND  datediff(d,@FromDate,TAH.HolidayDate)>=0 
And datediff(d,TAH.HolidayDate,@ToDate)>=0 



IF EXISTS(Select * from #HolidayeTable)
Begin

set @errormsg='Holidays found Please delete holidays accordingly !'

Select * from #HolidayeTable  
where [Days Name] not in ('sunday','Saturday')
order by [Date] asc

END

Select @errormsg as ErrorMsg


Drop Table #TempAUEC,#HolidayeTable