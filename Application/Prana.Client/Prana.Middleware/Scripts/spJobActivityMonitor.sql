
/****** Object:  StoredProcedure [dbo].[sp_JobActivityMonitor]    Script Date: 05/30/2013 20:18:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_JobActivityMonitor]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_JobActivityMonitor]

/****** Object:  StoredProcedure [dbo].[sp_JobActivityMonitor]    Script Date: 05/30/2013 20:19:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Matt Carlucci
-- Create date: 5/30/2013
-- Description:	View SQL Job Status
-- exec sp_JobActivityMonitor
-- =============================================
CREATE PROCEDURE [dbo].[sp_JobActivityMonitor] 
	-- Current Job or All Jobs
	@job_name nvarchar(255) = '*'
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if OBJECT_ID('tempdb..#xp_results') is not null
	drop table #xp_results

	if OBJECT_ID('tempdb..#xp_monitor') is not null
		drop table #xp_monitor

	CREATE TABLE #xp_results
	 (job_id                UNIQUEIDENTIFIER NOT NULL,      
	  last_run_date         INT              NOT NULL,      
	  last_run_time         INT              NOT NULL,      
	  next_run_date         INT              NOT NULL,      
	  next_run_time         INT              NOT NULL,      
	  next_run_schedule_id  INT              NOT NULL,      
	  requested_to_run      INT              NOT NULL,      
	  request_source        INT              NOT NULL,      
	  request_source_id     sysname          COLLATE database_default NULL,      
	  running               INT              NOT NULL,       
	  current_step          INT              NOT NULL,      
	  current_retry_attempt INT              NOT NULL,      
	  job_state             INT              NOT NULL)      

	insert into #xp_results 
	exec master.dbo.xp_sqlagent_enum_jobs 1, ''

	CREATE TABLE #xp_monitor
	   ([Name]				nvarchar(255) NOT NULL,
		[Status]			nvarchar(255) NOT NULL,
		[State]				nvarchar(255) NOT NULL,
		[LastRun]			DateTime  NOT NULL)
		

	insert into #xp_monitor
	select 
		[Name], 	
		case running
			when 0 then			
				case last_run_outcome
					when 0 then 'Failed'
					when 1 then 'Succeeded'
					when 3 then 'Canceled'
					else 'Unknown' end			
			else 'Running'  end as 
		[Status],		
		case job_state 
			when 0 then 'Not idle or suspended'
			when 1 then 'Executing Step ' + CAST(current_step as nvarchar(2))
			when 2 then 'Waiting For Thread'
			when 3 then 'Between Retries'
			when 4 then 'Idle' 
			when 5 then 'Suspended' 
			when 6 then 'WaitingForStepToFinish' 
			when 7 then 'PerformingCompletionActions'
			else'Unknown' end as 
		[State], 
		case sjs.last_run_date 
			when 0 then 0 
			else CONVERT(DATETIME,RTRIM(sjs.last_run_date)) +
				 (sjs.last_run_time * 9 + sjs.last_run_time % 10000 * 6 + sjs.last_run_time % 100 * 10) / 216e4 end AS 
		last_run_date
			
	from #xp_results rj
	inner join msdb.dbo.sysjobs sj
		on sj.job_id = rj.job_id   
	inner join msdb.dbo.sysjobservers sjs
		on sj.job_id = sjs.job_id

	if @job_name = '*'
		select * from #xp_monitor Order by [Name]
	else
		select * from #xp_monitor where Name = @job_name
	    
	--where name = @JobName


END
