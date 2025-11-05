
Declare	@FromJ_Date Datetime 
Declare @ToJ_Date Datetime
 
set @ToJ_Date=dateAdd(d,0,getDate())
set @FromJ_Date=dateadd(d,-7,@ToJ_Date)

Declare @ErrorMsg Varchar(Max)
Set @ErrorMsg='Job Run History'

SELECT J.Name As 'Job Name',H.run_status 'Run Status',
cast(rtrim(ltrim(H.run_date)) as Datetime) As 'Run Date',H .run_duration 'Time Taken(in seconds)'
FROM msdb.dbo.sysjobs J 
INNER JOIN msdb.dbo.sysjobhistory H ON J.job_id= H.job_id
where cast(rtrim(ltrim(H.run_date)) as Datetime)>=@FromJ_Date AND cast(rtrim(ltrim(H.run_date)) as Datetime)<=@ToJ_Date 
Order By H.run_date DESC

Select @errormsg as ErrorMsg
