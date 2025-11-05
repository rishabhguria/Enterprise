Declare	@FromM_Date Datetime 
Declare @ToM_Date Datetime
 
set @ToM_Date=dateAdd(d,0,getDate())
set @FromM_Date=dateadd(d,-7,@ToM_Date)

Declare @ErrorMsg Varchar(Max)
Set @ErrorMsg='Maintenance Plan History For Last Week'

SELECT P.Name As 'Maintenance Plans',L.Start_Time As 'Date',datediff(mi,L.Start_Time,L.End_Time)'Time Taken(in min)',L.Succeeded  
FROM msdb.dbo.sysmaintplan_plans P 
INNER JOIN msdb.dbo.sysmaintplan_log L ON P.ID = L.Plan_Id 
where L.Start_Time>=@FromM_Date AND L.Start_Time<=@ToM_Date 
Order By L.Start_Time DESC

Select @errormsg as ErrorMsg