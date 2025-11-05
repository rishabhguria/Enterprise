
-----------------------------------------------------------------   
--Purpose: Get SM Batch status from workflow
-----------------------------------------------------------------  
CREATE procedure [dbo].[P_GetSMBatchStatusFromWorkFlow]
(
@TaskID int
)
as

WITH SMBatchDetails(TaskRunTime,SMBatchName) as
(
SELECT MAX(TaskRunTime) as TaskRunTime, ContextValue as SMBatchName FROM T_FundWorkflowStats
GROUP BY T_FundWorkflowStats.ContextValue
)
SELECT SMBatchDetails.SMBatchName, T_FundWorkflowStats.StateID from SMBatchDetails
inner JOIN T_FundWorkflowStats on T_FundWorkflowStats.TaskRunTime = SMBatchDetails.TaskRunTime
and T_FundWorkflowStats.ContextValue = SMBatchDetails.SMBatchName where TaskID = @TaskID

