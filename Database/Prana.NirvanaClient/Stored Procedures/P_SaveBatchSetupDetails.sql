
CREATE procedure [dbo].[P_SaveBatchSetupDetails]
@xmlDoc ntext
as
declare @handle int
exec sp_xml_preparedocument @handle OUTPUT,@xmlDoc
CREATE TABLE #TempBatch                                                                               
(                                                                               
BatchSchedulerID int,
EnablePriceTolerance bit,
PriceTolerance decimal(5,2),
AutoExecution bit,
CronExpression varchar(max),
ExecutionTime varchar(max),
ScheduleTypeID int 
)        
insert INTO #TempBatch
(BatchSchedulerID,EnablePriceTolerance,PriceTolerance,AutoExecution, CronExpression, ExecutionTime, ScheduleTypeID)
SELECT 
BatchID, EnablePriceTolerance, PriceCheckTolerance, AutoExec, CronExpression,ExecutionTime,ScheduleTypeID
from openxml (@handle, '/dsBatch/dtBatch', 2)
with
(
BatchID int,
EnablePriceTolerance bit,
PriceCheckTolerance decimal(5,2),
AutoExec bit,
CronExpression varchar(max),
ExecutionTime varchar(max),
ScheduleTypeID int
)
--select * from #TempBatch
--select BatchSchedulerID from #TempBatch
update T_BatchSchedulers
SET 
T_BatchSchedulers.PriceTolerance= #TempBatch.PriceTolerance,
T_BatchSchedulers.EnablePriceTolerance=#TempBatch.EnablePriceTolerance,
T_BatchSchedulers.AutoExecution=#TempBatch.AutoExecution,
T_BatchSchedulers.CronExpression=#TempBatch.CronExpression,
T_BatchSchedulers.ExecutionTime=#TempBatch.ExecutionTime,
T_BatchSchedulers.ScheduleTypeID=#TempBatch.ScheduleTypeID
from #TempBatch
where T_BatchSchedulers.BatchSchedulerID =#TempBatch.BatchSchedulerID
exec sp_xml_removedocument @handle 
drop TABLE #TempBatch

