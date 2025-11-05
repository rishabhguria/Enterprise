using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Prana.Import
{
    public class ImportCacheManager
    {
        private static Dictionary<string, string> _dictDashboardBatches = new Dictionary<string, string>();

        /// <summary>
        /// Gets the dashBoard File Path for the batch already exist in cache or null
        /// </summary>
        /// <param name="executionName"></param>
        /// <returns></returns>
        public static string GetExecutionDashboardFilePath(string executionName)
        {
            string dashBoardFilePath = string.Empty;

            try
            {
                string execName = _dictDashboardBatches.Keys.FirstOrDefault(key => key.Contains(executionName));

                if (!string.IsNullOrEmpty(execName))
                {
                    dashBoardFilePath = _dictDashboardBatches[execName];
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
            return dashBoardFilePath;
        }
        /// <summary>
        /// Added by: Bharat raturi, 25 jun 2014
        /// Get the list of batches that are executed already
        /// <param name="startupPath"> startup path of the application</param>
        /// </summary>
        public static void CreateRunBatchDictionary(string startupPath)
        {
            try
            {
                string rootDir = startupPath + "\\DashBoardData\\Import";
                string[] childDirs = Directory.GetDirectories(rootDir);
                List<string> fileList = new List<string>();
                foreach (string directory in childDirs)
                {
                    fileList.AddRange(Directory.GetFiles(directory, "*.*", SearchOption.TopDirectoryOnly));
                }
                foreach (string fileName in fileList)
                {
                    string file = Path.GetFileName(fileName);
                    if (!_dictDashboardBatches.ContainsKey(file))
                    {
                        string relPath = fileName.Replace(startupPath.ToString(), string.Empty);
                        _dictDashboardBatches.Add(file, relPath);
                    }
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
        /// Remove the batch from the dictionary
        /// </summary>
        /// <param name="batchKey">Name of the dashboard file</param>
        public static void RemoveItemFromDashBoardFileCache(string batchKey)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(batchKey) && _dictDashboardBatches.ContainsKey(batchKey))
                {
                    _dictDashboardBatches.Remove(batchKey);
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
        /// Add the batch to the dictionary
        /// </summary>
        /// <param name="batchKey">Name of the dashboard file</param>
        /// <param name="batchValue">relative path of the dashboard file</param>
        public static void AddItemInDashBoardFileCache(string batchKey, string batchValue)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(batchKey) && !_dictDashboardBatches.ContainsKey(batchKey))
                {
                    _dictDashboardBatches.Add(batchKey, batchValue);
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
    }
}
