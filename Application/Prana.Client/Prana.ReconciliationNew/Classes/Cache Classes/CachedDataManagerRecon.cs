using Prana.BusinessObjects.Enums;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Prana.ReconciliationNew
{
    public class CachedDataManagerRecon
    {
        //
        // TODO: Add constructor logic here
        //
        private static CachedDataManagerRecon _cachedDataManagerRecon = null;
        static object _locker = new object();

        public static CachedDataManagerRecon GetInstance
        {
            get
            {
                if (_cachedDataManagerRecon == null)
                {
                    lock (_locker)
                    {
                        if (_cachedDataManagerRecon == null)
                        {
                            _cachedDataManagerRecon = new CachedDataManagerRecon();
                        }
                    }
                }
                return _cachedDataManagerRecon;
            }
        }
        /// <summary>
        /// returns dictionary of all permitted accounts
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAllPermittedAccounts()
        {
            return CachedDataRecon.GetInstance().Accounts;
        }

        /// <summary>
        /// returns dictionary of Company wise accounts
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, List<int>> GetAllCompanyAccounts()
        {
            return CachedDataRecon.GetInstance().CompanyAccounts;
        }

        /// <summary>
        /// returns dictionary of Thirdparty and accounts
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, List<int>> GetAllCompanyThirdPartyAccounts()
        {
            return CachedDataRecon.GetInstance().CompanyThirdPartyAccounts;
        }

        /// <summary>
        /// returns permission level of Form ReconCancelAmend
        /// </summary>
        /// <returns></returns>
        public AuthAction GetPermissionLevel()
        {
            return CachedDataRecon.GetInstance().PermissionLevel;
        }

        /// <summary>
        /// Refresh cache on user action so that there is no need to restart client application
        /// </summary>
        /// <param name="companyUserID"></param>
        public static void RefreshCache(int companyUserID)
        {
            try
            {
                CachedDataRecon.SetCommonData(companyUserID);
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

        #region Dashboard Cache Methods

        /// <summary>
        /// Returns cache of dashboard batch maintained
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> GetDictDashboardBatches()
        {
            return CachedDataRecon.GetInstance().DictDashboardBatches;
        }

        /// <summary>
        /// Added by: Aman  Seth, 30 july 2014
        /// Get the list of batches that are executed already
        /// <param name="startupPath"> startup path of the application</param>
        /// </summary>
        public static void CreateRunBatchDictionary(string startupPath)
        {
            try
            {
                if (startupPath != null)
                {
                    string rootDir = startupPath + "\\DashBoardData\\Recon";
                    if (!Directory.Exists(rootDir))
                    {
                        Directory.CreateDirectory(rootDir);
                    }
                    string[] childDirs = Directory.GetDirectories(rootDir);
                    List<string> fileList = new List<string>();
                    foreach (string directory in childDirs)
                    {
                        fileList.AddRange(Directory.GetFiles(directory, "*.*", SearchOption.TopDirectoryOnly));
                    }
                    foreach (string fileName in fileList)
                    {
                        string file = Path.GetFileName(fileName);
                        if (!CachedDataRecon.GetInstance().DictDashboardBatches.ContainsKey(file))
                        {
                            string relPath = fileName.Replace(startupPath.ToString(), string.Empty);
                            CachedDataRecon.GetInstance().DictDashboardBatches.Add(file, relPath);
                        }
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
                if (!string.IsNullOrWhiteSpace(batchKey) && CachedDataRecon.GetInstance().DictDashboardBatches.ContainsKey(batchKey))
                {
                    CachedDataRecon.GetInstance().DictDashboardBatches.Remove(batchKey);
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
                if (!string.IsNullOrWhiteSpace(batchKey) && !CachedDataRecon.GetInstance().DictDashboardBatches.ContainsKey(batchKey))
                {
                    CachedDataRecon.GetInstance().DictDashboardBatches.Add(batchKey, batchValue);
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

        #endregion


        /// <summary>
        /// Gets the full Execution name to check if import for that batch already exist or not
        /// </summary>
        /// <param name="executionName"></param>
        /// <returns></returns>
        public static string GetExecutionDashboardFilePath(string executionName)
        {
            string dashBoardFilePath = string.Empty;

            try
            {
                string execName = GetDictDashboardBatches().Keys.FirstOrDefault(key => key.Contains(executionName));

                if (!string.IsNullOrEmpty(execName))
                {
                    dashBoardFilePath = GetDictDashboardBatches()[execName];
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
        /// get dashboard file path from task execution name from cache maintained
        /// </summary>
        /// <param name="execBatch"></param>
        /// <returns></returns>
        public static string getDashBoardFile(string execBatch)
        {
            string dashBoardFilePath = string.Empty;
            try
            {
                dashBoardFilePath = GetDictDashboardBatches()[execBatch];
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
        /// Sets the permission in Cache
        /// </summary>
        /// <param name="permissionLevel"></param>
        public static void SetUserPermissionLevel(BusinessObjects.Enums.AuthAction permissionLevel)
        {
            try
            {
                CachedDataRecon.GetInstance().SetUserPermissionLevel(permissionLevel);
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
