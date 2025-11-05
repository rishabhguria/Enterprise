    
-- Author  : Rajat    
-- Date   : 20 Oct 2011    
-- Description : It returns the calender between the given date range. The date range should not exceed total of 32767 days     
--    (More than 89 years), else it would give an error    
-- Reference : http://www.sqlpointers.com/2006/07/generating-temporary-calendar-tables.html    
-- Usage  : Select * from dbo.F_GetCalenderForDateRange('2011-07-25 00:00:00.000', '2011-10-21 00:00:00.000')    
Create Function F_GetCalenderForDateRange    
(    
 @start datetime,    
 @end datetime    
)    
    
RETURNS     
@Result TABLE     
(    
 Date datetime,     
 IsWeekDay VARCHAR(255),    
 YearNo int,     
 Quarter int,     
 MonthNo int,     
 DateNo int,     
 DateOfWeek int,     
 MonthName varchar(10),     
 DayName varchar(10),     
 WeekNo int    
)    
AS    
BEGIN    
    
with calendar(Date,IsWeekday, YearNo, Quarter,MonthNo,DateNo,DateOfweek,Monthname,Dayname,WeekNo) as    
(    
select @start ,    
case when datepart(dw,@start) in (1,7) then 0 else 1 end,    
year(@start),    
datepart(qq,@start),    
datepart(mm,@start),    
datepart(dd,@start),    
datepart(dw,@start),    
datename(month, @start),    
datename(dw, @start),    
datepart(wk, @start)    
union all    
select date + 1,    
case when datepart(dw,date + 1) in (1,7) then 0 else 1 end,    
year(date + 1),    
datepart(qq,date + 1),    
datepart(mm,date + 1),    
datepart(dd,date + 1),    
datepart(dw,date + 1),    
datename(month, date + 1),    
datename(dw, date + 1),    
datepart(wk, date + 1) from calendar where date + 1 <= @end    
)    
    
Insert into @Result    
select * from calendar option(maxrecursion 32767)    
    
RETURN     
END    
    