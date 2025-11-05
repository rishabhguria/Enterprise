
Declare @ToDate Datetime
Declare @ErrorMsg Varchar(Max)

set @ToDate=''
Set @ErrorMsg=''



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

Insert Into #HolidayeTable
Select
@ToDate AS [Run Date],
@DaysName As [Days Name],
TA.AUECID,
DisplayName As [Display Name],
FullName As [Full Name]
FROM  #TempAUEC TA
Inner Join T_AUECHolidays TAH ON TA.AUECID=TAH.AUECID AND TAH.HolidayDate = @ToDate

If Exists (Select * from #HolidayeTable)
Begin
Select * from #HolidayeTable
Set @ErrorMsg = @ErrorMsg + 'There is a Holiday in some Exchanges today.!'
End 

Select @ErrorMsg as ErrorMsg
-- And TAH.HolidayDate = @EndDate 

--Select * from T_AUECWeeklyHolidays
--Select * from T_AUECHolidays

Drop Table #TempAUEC,#HolidayeTable