using Prana.LogManager;
using Prana.TaskManagement.Definition.Definition;
using Prana.TaskManagement.Definition.Interfaces;
using Prana.TaskManagement.Execution.DAL;
using System;
using System.Collections.Generic;

namespace Prana.TaskManagement.Execution
{
    public static class TaskExecutionManager
    {

        /// <summary>
        /// Variable to store the stratup path of the application
        /// </summary>
        /// added by: Bharat Raturi, 19 apr 2014
        /// to get the startup path of the application
        public static String startUpPath = String.Empty;

        /// <summary>
        /// Returns true if initialized successfully or false if already initialized
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// added by: Bharat Raturi, 19 apr 2014
        /// to get the startup path of the application
        public static bool Initialize(String path)
        {
            if (String.IsNullOrWhiteSpace(startUpPath) && !String.IsNullOrWhiteSpace(path))
            {
                startUpPath = path;
                return true;
            }
            else
                return false;

        }


        public static TaskResult ExecuteTask(String executionId)
        {
            ExecutionInfo eInfo = TaskExecutionCache.Instance.GetExecutionInfo(executionId);
            if (eInfo == null)
            {
                throw new Exception("Execution not found for given execution id");
            }
            else
            {
                NirvanaTask task = TaskActivator.Instance.CreateTaskInstanceFor(eInfo.TaskInfo);
                task.Initialize(eInfo.TaskInfo);
                return task.ExecuteTask(eInfo, null);
            }
        }

        public static void ExecuteTask(String executionId, ITaskCallback callbackInstance)
        {
            try
            {
                ExecutionInfo eInfo = TaskExecutionCache.Instance.GetExecutionInfo(executionId);
                if (eInfo == null)
                {
                    throw new Exception("Execution not found for given execution id");
                }
                else
                {
                    NirvanaTask task = TaskActivator.Instance.CreateTaskInstanceFor(eInfo.TaskInfo);
                    task.Initialize(eInfo.TaskInfo);
                    task.ExecuteTaskAsync(eInfo, callbackInstance);
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

        public static void ExecuteTask(ExecutionInfo eInfo)
        {
            try
            {
                //ExecutionInfo eInfo = TaskExecutionCache.Instance.GetExecutionInfo(executionId);
                if (eInfo == null)
                {
                    throw new Exception("Execution not found for given execution id");
                }
                else
                {
                    NirvanaTask task = TaskActivator.Instance.CreateTaskInstanceFor(eInfo.TaskInfo);
                    task.Initialize(eInfo.TaskInfo);
                    task.ExecuteTask(eInfo, null);
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

        /// <summary>
        /// Get the statistics details from file as fileName: fileData in XMl form dictionary  
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        /// Modified By: Bharat raturi, 19 apr 2014
        /// The return type of the function changed to return a dictionary but a list 
        public static Dictionary<string, String> GetTaskStatisticsAsXML(int taskId)
        {
            try
            {
                return TaskExecutionDataManager.GetTaskStatisticsAsXML(taskId, startUpPath);
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
                return null;
            }
        }


        public static bool PurgeFiles(List<String> files, string startUpPath)
        {
            try
            {
                TaskExecutionDataManager.PurgeFiles(files, startUpPath);
                return true;
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
                return false;
            }
        }

        public static bool ArchiveFiles(List<String> files, int taskId)
        {
            try
            {
                TaskExecutionDataManager.ArchiveFiles(files, taskId, startUpPath);
                return true;
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
                return false;
            }
        }

        #region methods for getting archived data
        /// <summary>
        /// Added by: Bharat Raturi, 11 jun 2014
        /// Get the archived files from dashboard folder
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static Dictionary<string, String> GetArchiveStatisticsAsXML(int taskId)
        {
            try
            {
                return TaskExecutionDataManager.GetArchiveStatisticsAsXML(taskId, startUpPath);
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
                return null;
            }
        }

        /// <summary>
        /// Added by: Bharat raturi, 12 jun 2014
        /// Archive the data
        /// </summary>
        /// <param name="files"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public static bool ArchiveInValidData(List<string> files, int taskId)
        {
            try
            {
                TaskExecutionDataManager.ArchiveInvalidData(files, taskId, startUpPath);
                return true;
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
                return false;
            }
        }
        #endregion
    }
}