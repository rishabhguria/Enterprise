using Prana.LogManager;
using Prana.TaskManagement.Definition.Definition;
using Prana.TaskManagement.Execution.DAL;
using System;
using System.Collections.Generic;

namespace Prana.TaskManagement.Execution
{
    public class TaskExecutionCache
    {
        #region Singleton Instance
        //Singleton is created as property see http://www.dotnetperls.com/singleton

        private static readonly TaskExecutionCache _instance = new TaskExecutionCache();
        public static TaskExecutionCache Instance
        {
            get
            {
                return _instance;
            }
        }
        private TaskExecutionCache()
        {
            IntializeTaskCache();
        }

        #endregion


        #region Private methods

        /// <summary>
        /// Load cache from DB and fills cache objects
        /// </summary>
        private void IntializeTaskCache()
        {
            // TODO:Load cache from DB
            this._executionInfoCache = TaskExecutionDataManager.GetExecutionInfoCollection();
        }


        #endregion

        #region Cache Objects


        Dictionary<String, ExecutionInfo> _executionInfoCache = new Dictionary<String, ExecutionInfo>();

        #endregion


        #region Exposed internal methods

        public ExecutionInfo GetExecutionInfo(String executionId)
        {
            ExecutionInfo exInfo = null;

            try
            {
                if (_executionInfoCache.ContainsKey(executionId))
                    exInfo = _executionInfoCache[executionId];
                //else
                //    return null;
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
            return exInfo;
        }

        public Dictionary<String, ExecutionInfo> GetExecutionInfoCollection()
        {
            // To refresh execution information on reload settings.
            IntializeTaskCache();
            return _executionInfoCache;
        }

        #endregion


    }
}
