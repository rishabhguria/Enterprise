using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.TaskManagement.Definition.Definition;
using Prana.TaskManagement.Execution;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Prana.Import
{
    public class ImportExecutionHelper
    {

        /// <summary>
        /// Execute import batch.
        /// Batch will be executed for date range for the defined file naming convention in admin.
        /// </summary>
        /// <param name="formatName"></param>
        public static StringBuilder ExecuteImportBatch(string formatName)
        {
            StringBuilder message = new StringBuilder();
            try
            {
                #region 1. GetRunUploadFromFormatName
                //This dataset will contain ftpid and decryption id
                RunUpload runUpload = ImportDataManager.FillRunUploadDetails(formatName);
                #endregion

                if (runUpload != null)
                {
                    runUpload.FormatName = formatName;

                    if (runUpload.ImportDataSource == ImportDataSource.Function)
                    {
                        ExecuteBatchForFunction(formatName);
                    }
                    else
                    {
                        #region 2. update batch execution start date  based on last execution and batch start date defined in admin
                        DateTime date = GetBatchExecutionStartDate(formatName, runUpload.BatchStartDate);
                        #endregion

                        #region 3. get file names for the date range for the naming convention defined in admin
                        Dictionary<string, DateTime> dictAllFileNames = GetDictFileNames(runUpload.FtpFilePath, ref date);
                        #endregion

                        if (dictAllFileNames.Count > 0)
                        {
                            if (runUpload.FtpDetails != null)
                            {
                                ExecuteBatchForFiles(formatName, message, runUpload, dictAllFileNames);
                            }
                            else
                            {
                                //Show Proper Message to user if FTP is not sets.
                                message.Append("FTP details missing for batch  " + formatName + ".");
                            }
                        }
                        else if (!message.ToString().Contains("No files available from "))
                        {
                            message.Append("No files available from " + date.Date.ToString());
                        }
                    }
                }
                else
                {
                    message.Append("Batch " + formatName + " is either deleted or modified");
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
            return message;
        }

        private static void ExecuteBatchForFiles(string formatName, StringBuilder message, RunUpload runUpload, Dictionary<string, DateTime> dictAllFileNames)
        {
            try
            {
                #region check that from list how much files are available in ftp and return list and for available files execute batch
                Dictionary<string, DateTime> dictListFiles = GetFilesListOnFTP(runUpload.FtpDetails, runUpload.FtpFilePath, dictAllFileNames);

                if (dictListFiles.Count > 0)
                {
                    //for each available file run the batch
                    //file name also amended in batch execution name
                    foreach (KeyValuePair<string, DateTime> kvp in dictListFiles)
                    {
                        List<object> lstObjects = new List<object>();
                        lstObjects.Add(Path.GetDirectoryName(runUpload.FtpFilePath) + @"/" + kvp.Key);
                        lstObjects.Add(kvp.Value);


                        ExecutionInfo eInfo = DeepCopyHelper.Clone(TaskExecutionCache.Instance.GetExecutionInfo("Import_-1"));
                        eInfo.InputData = formatName;
                        eInfo.ExecutionName = formatName + Seperators.SEPERATOR_6 + kvp.Key;
                        eInfo.InputObjects = lstObjects;
                        eInfo.IsAutoImport = true;
                        NirvanaTask task = ImportManager.Instance;
                        task.Initialize(eInfo.TaskInfo);

                        //added by: Bharat Raturi, 25 jun 2014
                        //purpose: Supply the dashboard filepath
                        string dashboardPath = ImportCacheManager.GetExecutionDashboardFilePath(eInfo.ExecutionName);
                        if (!string.IsNullOrWhiteSpace(dashboardPath))
                        {
                            TaskResult result = new TaskResult();
                            result.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("DashboardFile", dashboardPath, dashboardPath);
                            task.ExecuteTask(eInfo, result);
                        }
                        else
                        {
                            task.ExecuteTask(eInfo, null);
                        }
                    }
                }
                else
                {
                    //Show Proper Message to user if there are no file to upload.
                    message.Append("File is missing on FTP for the batch " + formatName + "." + " Check FTP settings and file path.");
                }
                #endregion
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

        private static Dictionary<string, DateTime> GetFilesListOnFTP(ThirdPartyFtp thirdPartyFtp, string ftpFilePath, Dictionary<string, DateTime> dictAllFileNames)
        {
            Dictionary<string, DateTime> dictListFiles = new Dictionary<string, DateTime>();
            try
            {
                //check that from list how much files are available in ftp
                NirvanaWinSCPUtility scpUtil = new NirvanaWinSCPUtility(thirdPartyFtp);
                dictListFiles = scpUtil.ListDirectory(Path.GetDirectoryName(ftpFilePath), dictAllFileNames);
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
            return dictListFiles;
        }

        private static Dictionary<string, DateTime> GetDictFileNames(string ftpFilePath, ref DateTime date)
        {
            //This method can be moved in utilities to get file names for a date range for the given naming convention
            Dictionary<string, DateTime> dictAllFileNames = new Dictionary<string, DateTime>();
            try
            {//we are adding a day in date because batch should run for the next day of last execution
                date = date.AddDays(1);
                while (date.CompareTo(DateTime.Now.Date) < 0)
                {
                    string fileName = FileNameParser.GetFileNameFromNamingConvention(ftpFilePath, date);
                    //File system is not case sensitive so while comparing file name case is to be ignored.
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        if (!dictAllFileNames.ContainsKey(Path.GetFileName(fileName).ToLower()))
                        {
                            dictAllFileNames.Add(Path.GetFileName(fileName).ToLower(), date);
                        }
                    }
                    date = date.AddDays(1);
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
            return dictAllFileNames;
        }

        private static DateTime GetBatchExecutionStartDate(string formatName, DateTime batchStartDate)
        {
            DateTime date = DateTime.MinValue;
            try
            {
                string filePath = Application.StartupPath + @"\DashBoardData\Import\" + formatName + ".xml";

                //TODO:  Move this code in separate method
                //Read last execution date and run the batch for next dates
                if (File.Exists(filePath))
                {
                    DataSet ds = new DataSet();
                    //CHMW-2181	[Reconciliation] [Code Review] Replace dataset read xml with BufferedStream
                    ds = XMLUtilities.ReadXmlUsingBufferedStream(filePath);
                    //ds.ReadXml(filePath);
                    //check if file is present and has a valid datetime value
                    if (!(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Columns.Contains("LastExecutionDate") &&
                        DateTime.TryParseExact(ds.Tables[0].Rows[0]["LastExecutionDate"].ToString(), ApplicationConstants.DateFormat,
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out date)))
                    {
                        date = batchStartDate;
                    }
                }
                else
                {
                    date = batchStartDate;
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
            return date;
        }

        private static void ExecuteBatchForFunction(string formatName)
        {
            try
            {
                ExecutionInfo eInfo = DeepCopyHelper.Clone(TaskExecutionCache.Instance.GetExecutionInfo("Import_-1"));
                eInfo.InputData = formatName;
                eInfo.ExecutionName = formatName;

                NirvanaTask task = ImportManager.Instance;
                task.Initialize(eInfo.TaskInfo);
                task.ExecuteTask(eInfo, (new TaskResult()));
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
