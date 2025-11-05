CREATE Function dbo.fnDateTable
(
  @StartDate datetime,
  @EndDate datetime ---,
--  @DayPart char(5) -- support 'day','month','year','hour', default 'day'
)
Returns @Result Table
(
  [Date] datetime
)
As
Begin

Declare @CurrentDate datetime


  Set @CurrentDate=@StartDate
  While @CurrentDate<=@EndDate
  Begin
	Insert Into @Result Values (@CurrentDate)
    Select @CurrentDate= DateAdd(dd,1,@CurrentDate)
  End
Return
End
