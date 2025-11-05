using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using Prana.TaskManagement.Definition.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Prana.TaskManagement.Definition.Definition
{
    public abstract class NirvanaTask
    {


        public TaskInfo TaskInfo { get; private set; }

        public NirvanaTaskStatus TaskStatus { get; set; }


        private TaskResult ExecuteTask(ExecutionInfo executionData, List<TaskResult> previousTaskResults)
        {
            TaskResult result = new TaskResult();

            //If we want to run same task again the need to pass previous task result
            if (previousTaskResults != null && previousTaskResults.Count > 0)
            {
                result = previousTaskResults[0];
            }
            //result.ExecutionInfo.TaskInfo = this.TaskInfo;
            try
            {
                result.ExecutionInfo = executionData;
                result.TaskStatistics.StartTime = DateTime.Now;

                this.TaskStatus = NirvanaTaskStatus.Running;
                result.TaskStatistics.Status = this.TaskStatus;
                bool isTaskCompleted = false;

                if (executionData.InputObjects != null && executionData.InputObjects.Count > 1 && !string.IsNullOrWhiteSpace(executionData.InputObjects[1].ToString()))
                {
                    DateTime dtExecutionDate = DateTime.Parse(executionData.InputObjects[1].ToString());
                    result.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ExecutionDate", dtExecutionDate.ToString(ApplicationConstants.DateFormat), dtExecutionDate.ToString(ApplicationConstants.DateFormat));
                }
                //We need to log and publish result here because running status of xml will not be shown on dashboard UI until xml generation
                //Currently xml is generated when task is completed
                LogAndPublishResult(result);
                //here we need to set Is re run Task then after publish set to false.
                result.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("IsReRun", false, null);
                try
                {
                    isTaskCompleted = Execute(executionData, ref result, previousTaskResults);
                    if (isTaskCompleted)
                    {
                        if (result.Error == null)
                            this.TaskStatus = NirvanaTaskStatus.Completed;
                        else
                            this.TaskStatus = NirvanaTaskStatus.Failure;
                    }

                }
                catch (Exception ex)
                {
                    result.Error = ex;
                    this.TaskStatus = NirvanaTaskStatus.Failure;
                }

                if (isTaskCompleted)
                {
                    result.TaskStatistics.Status = this.TaskStatus;
                    result.TaskStatistics.EndTime = DateTime.Now;

                    LogAndPublishResult(result);
                }
                else
                    result = null;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        protected void ExecutionComplete(TaskResult result)
        {
            try
            {
                if (result.Error == null)
                    this.TaskStatus = NirvanaTaskStatus.Completed;
                else
                    this.TaskStatus = NirvanaTaskStatus.Failure;

                result.TaskStatistics.Status = this.TaskStatus;
                result.TaskStatistics.EndTime = DateTime.Now;

                LogAndPublishResult(result);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void SaveWorkflowResult()
        {
            WorkflowHandler.Instance.SaveWorkflowData();
        }

        private void LogAndPublishResult(TaskResult result)
        {
            try
            {
                result.LogResult();
                DataTable taskStats = result.AsTable;
                if (taskStats != null && taskStats.Rows.Count > 0)
                {
                    DataTable tempDT = taskStats.Copy();
                    BackgroundWorker bgw = new BackgroundWorker();
                    bgw.DoWork += bgw_DoWork;
                    bgw.RunWorkerAsync(tempDT);

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {

            try
            {
                DataTable workflowStatsDT = e.Argument as DataTable;
                if (workflowStatsDT != null)
                    WorkflowHandler.Instance.PublishWorkflowEvent(workflowStatsDT);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public TaskResult ExecuteTask(ExecutionInfo executionData, TaskResult taskResult)
        {
            TaskResult result = null;

            try
            {
                List<TaskResult> lstTaskResult = new List<TaskResult>();
                if (taskResult != null)
                    lstTaskResult.Add(taskResult);
                result = ExecuteTask(executionData, lstTaskResult);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        public async void ExecuteTaskAsync(ExecutionInfo executionData, ITaskCallback callbackInstance)
        {
            try
            {
                TaskResult result = await System.Threading.Tasks.Task.FromResult<TaskResult>(ExecuteTask(executionData, new List<TaskResult>()));
                callbackInstance.TaskExecutionComplete(result);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void Initialize(TaskInfo taskInfo)
        {
            try
            {
                this.TaskInfo = taskInfo;
                this.InitializeTask(taskInfo);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        #region Abstract Methods

        protected abstract bool Execute(ExecutionInfo executionData, ref TaskResult result, List<TaskResult> previousTaskResults);
        protected abstract void InitializeTask(TaskInfo info);

        #endregion




    }
}
