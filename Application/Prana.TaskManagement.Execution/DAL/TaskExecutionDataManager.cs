using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.LogManager;
using Prana.TaskManagement.Definition.Definition;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Prana.TaskManagement.Execution.DAL
{
    internal static class TaskExecutionDataManager
    {
        internal static Dictionary<String, ExecutionInfo> GetExecutionInfoCollection()
        {
            Dictionary<String, ExecutionInfo> retValue = new Dictionary<String, ExecutionInfo>();
            try
            {

                #region Importbatch section

                Dictionary<int, String> importBatchData = GetImportBacthData();
                foreach (int key in importBatchData.Keys)
                {
                    ExecutionInfo importInfo = new ExecutionInfo();
                    importInfo.TaskInfo = GetTaskInfoForImport();

                    importInfo.ExecutionId = "Import_" + key;
                    string[] data = importBatchData[key].Split(Seperators.SEPERATOR_6);
                    importInfo.ExecutionName = data[0];
                    importInfo.InputData = data[0];

                    if (data.Length > 1)
                    {
                        importInfo.CronExpression = data[1];
                    }

                    retValue.Add(importInfo.ExecutionId, importInfo);
                }

                #endregion


                #region ReconBacth

                Dictionary<int, String> reconBatchData = GetReconBacthData();
                foreach (int key in reconBatchData.Keys)
                {
                    ExecutionInfo reconInfo = new ExecutionInfo();
                    reconInfo.TaskInfo = GetTaskInfoForRecon();

                    reconInfo.ExecutionId = "Recon_" + key;
                    reconInfo.ExecutionName = reconBatchData[key] + Seperators.SEPERATOR_6 + key;
                    reconInfo.InputData = reconBatchData[key];

                    retValue.Add(reconInfo.ExecutionId, reconInfo);
                }


                #endregion


                #region SMBatch

                Dictionary<int, String> smBatchData = GetSMBacthData();
                foreach (int key in smBatchData.Keys)
                {
                    ExecutionInfo smInfo = new ExecutionInfo();
                    smInfo.TaskInfo = GetTaskInfoForSM();

                    smInfo.ExecutionId = "SM_" + key;
                    smInfo.ExecutionName = reconBatchData[key] + Seperators.SEPERATOR_6 + key;
                    smInfo.InputData = reconBatchData[key];

                    retValue.Add(smInfo.ExecutionId, smInfo);
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
            return retValue;
        }


        internal static void PurgeFiles(List<String> files, String startUpPath)
        {
            try
            {

                foreach (String filePath in files)
                {
                    if (!Directory.Exists(startUpPath + @"\DashBoardData\HistoricalUploads"))
                        Directory.CreateDirectory(startUpPath + @"\DashBoardData\HistoricalUploads");

                    if (File.Exists(startUpPath + filePath))
                    {
                        // Purpose : To delete previous purged files of same batch from historical uploads
                        if (File.Exists(startUpPath + @"\DashBoardData\HistoricalUploads\" + Path.GetFileName(startUpPath + filePath)))
                        {
                            File.Delete(startUpPath + @"\DashBoardData\HistoricalUploads\" + Path.GetFileName(startUpPath + filePath));
                        }
                        File.Move(startUpPath + filePath, startUpPath + @"\DashBoardData\HistoricalUploads\" + Path.GetFileName(startUpPath + filePath));
                        Logger.LoggerWrite("File deleted: " + startUpPath + filePath);
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


        internal static void ArchiveFiles(List<String> files, int taskId, String startUpPath)
        {
            try
            {
                String rootPath = startUpPath + @"\DashBoardData\Archive\";

                switch (taskId)
                {
                    case 1:
                        rootPath += GetTaskInfoForImport().TaskName;
                        break;
                    case 2:
                        rootPath += GetTaskInfoForRecon().TaskName;
                        break;
                    case 3:
                        rootPath += GetTaskInfoForSM().TaskName;
                        break;
                }

                foreach (String originalFileName in files)
                {
                    String fileName = Path.GetFileName(originalFileName);
                    if (File.Exists(startUpPath + originalFileName))
                    {
                        if (!Directory.Exists(rootPath))
                        {
                            Directory.CreateDirectory(rootPath);
                        }
                        if (File.Exists(rootPath + "\\" + fileName))
                        {
                            File.Delete(rootPath + "\\" + fileName);
                        }
                        File.Move(startUpPath + originalFileName, rootPath + "\\" + fileName);
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

        #region New method for archiving data
        /// <summary>
        /// Added by: Bharat Raturi, 11 june 2014
        /// archive only the non-validated symbol data
        /// </summary>
        /// <param name="files"></param>
        /// <param name="taskId"></param>
        /// <param name="startUpPath"></param>
        internal static void ArchiveInvalidData(List<String> files, int taskId, String startUpPath)
        {
            try
            {
                String rootPath = startUpPath + @"\DashBoardData\Archive\";

                switch (taskId)
                {
                    case 1:
                        rootPath += GetTaskInfoForImport().TaskName;
                        break;
                    case 2:
                        rootPath += GetTaskInfoForRecon().TaskName;
                        break;
                    case 3:
                        rootPath += GetTaskInfoForSM().TaskName;
                        break;
                }

                foreach (String originalFileName in files)
                {
                    String fileName = Path.GetFileName(originalFileName);
                    if (File.Exists(startUpPath + originalFileName) && originalFileName.Contains("ImportData"))
                    {
                        if (!Directory.Exists(rootPath))
                        {
                            Directory.CreateDirectory(rootPath);
                        }
                        DataSet dsSource = new DataSet();
                        DataTable dataSource = new DataTable();
                        dsSource.ReadXml(startUpPath + originalFileName);
                        if (dsSource.Tables[0].Columns.Contains("ValidationStatus"))
                        {
                            var matchingRows = from DataRow row in dsSource.Tables[0].Rows
                                               where row.Field<string>("ValidationStatus") == "Validated"
                                               select row;

                            dataSource = dsSource.Tables[0].Clone();
                            foreach (DataRow dr in matchingRows)
                            {
                                dataSource.ImportRow(dr);
                            }
                            if (dataSource.Rows.Count == 0)
                            {
                                return;
                            }
                            DataSet dsWrite = new DataSet();
                            //string writeFileName = originalFileName.Substring(originalFileName.LastIndexOf("\\") + 1);
                            string writeFileName = Path.GetFileNameWithoutExtension(originalFileName) + "_NonValidatedTrades.xml";
                            dsWrite.Tables.Add(dataSource);
                            dsWrite.WriteXml(rootPath + "\\" + writeFileName);
                        }
                        if (File.Exists(rootPath + "\\" + fileName))
                        {
                            File.Delete(rootPath + "\\" + fileName);
                        }
                        File.Move(startUpPath + originalFileName, rootPath + "\\" + fileName);
                    }
                    else
                    if (File.Exists(startUpPath + originalFileName)) //&& !originalFileName.Contains("ImportData"))
                    {
                        if (!Directory.Exists(rootPath))
                        {
                            Directory.CreateDirectory(rootPath);
                        }
                        string source = startUpPath + originalFileName;
                        string destination = rootPath + "\\" + Path.GetFileName(originalFileName);
                        if (File.Exists(source))
                        {
                            //File.Copy(source, destination, true);
                            if (File.Exists(destination))
                            {
                                File.Delete(destination);
                            }
                            File.Move(source, destination);
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
        /// added by: Bharat raturi, 11 jun 2014
        /// Get the archived data files names
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="startUpPath"></param>
        /// <returns></returns>
        internal static Dictionary<string, String> GetArchiveStatisticsAsXML(int taskId, string startUpPath)
        {
            Dictionary<string, String> retValue = new Dictionary<string, String>();

            try
            {
                String rootPath = startUpPath + @"\DashBoardData\Archive\";

                switch (taskId)
                {
                    case 1:
                        rootPath += GetTaskInfoForImport().TaskName;
                        break;
                    case 2:
                        rootPath += GetTaskInfoForRecon().TaskName;
                        break;
                    case 3:
                        rootPath += GetTaskInfoForSM().TaskName;
                        break;
                }

                //create directory is not exists
                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }


                // TODO: Determine folders to read into based on date range provided and from rootPath
                // Currently adding all directories
                List<String> directoryList = new List<string>();
                directoryList.AddRange(Directory.GetDirectories(rootPath));



                // TODO: load files from given path within given date range latest per executionId
                List<String> fileList = new List<String>();
                foreach (String dir in directoryList)
                {
                    if (Directory.Exists(dir))
                    {
                        fileList.AddRange(Directory.GetFiles(dir, "*.xml", SearchOption.TopDirectoryOnly));
                    }
                }
                fileList.AddRange(Directory.GetFiles(rootPath));
                // TODO: Read files into XML String and return
                foreach (String filePath in fileList)
                {
                    if (File.Exists(filePath))
                    {
                        retValue.Add(filePath, File.ReadAllText(filePath));
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
            return retValue;
        }
        #endregion

        /// <summary>
        /// Return the dictionary of filename:filedata pair
        /// </summary>
        /// <param name="taskId">ID of the task to be performed</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="startUpPath">startup Path of the application</param>
        /// <returns>Dictionary</returns>
        /// Modified By: Bharat raturi, 19 apr 2014
        /// purpose: return the dictionary of filename: filedata pair rather than the list of file data
        /// Dictionary key will be used to get the execution id later
        internal static Dictionary<string, String> GetTaskStatisticsAsXML(int taskId, String startUpPath)
        {
            Dictionary<string, String> retValue = new Dictionary<string, String>();

            try
            {
                String rootPath = startUpPath + @"\DashBoardData\";

                switch (taskId)
                {
                    case 1:
                        rootPath += GetTaskInfoForImport().TaskName;
                        break;
                    case 2:
                        rootPath += GetTaskInfoForRecon().TaskName;
                        break;
                    case 3:
                        rootPath += GetTaskInfoForSM().TaskName;
                        break;
                }

                //create directory is not exists
                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }


                // TODO: Determine folders to read into based on date range provided and from rootPath
                // Currently adding all directories
                List<String> directoryList = new List<string>();
                directoryList.AddRange(Directory.GetDirectories(rootPath));



                // TODO: load files from given path within given date range latest per executionId
                List<String> fileList = new List<String>();
                foreach (String dir in directoryList)
                {
                    if (Directory.Exists(dir))
                    {
                        fileList.AddRange(Directory.GetFiles(dir, "*.xml", SearchOption.TopDirectoryOnly));
                    }
                }

                // TODO: Read files into XML String and return
                foreach (String filePath in fileList)
                {
                    //Added by Sachin mishra Purpose Jira-CHMW-3589
                    if (File.Exists(filePath) && Prana.Utilities.IO.File.IsFileOpen(filePath))
                    {
                        // modified by: Bharat Raturi, 19 apr 2014
                        // To add the file name as the key and text from the xml file as the value
                        retValue.Add(filePath, File.ReadAllText(filePath));
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
            return retValue;
        }


        #region Loading Batch information

        private static Dictionary<int, string> GetSMBacthData()
        {
            Dictionary<int, String> importBatchData = new Dictionary<int, string>();


            try
            {
                importBatchData.Add(-1, "Manual Operation");
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

            return importBatchData;
        }

        private static Dictionary<int, string> GetReconBacthData()
        {
            Dictionary<int, String> importBatchData = new Dictionary<int, string>();

            try
            {
                importBatchData.Add(-1, "Manual Operation");
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

            return importBatchData;
        }

        private static Dictionary<int, String> GetImportBacthData()
        {
            Dictionary<int, String> importBatchData = new Dictionary<int, string>();


            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllBatchDetails";
                queryData.CommandTimeout = 900;
                queryData.DictionaryDatabaseParameter.Add("@thirdPartyID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@thirdPartyID",
                    ParameterType = DbType.Int32,
                    ParameterValue = -1
                });

                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        if (!importBatchData.ContainsKey(Convert.ToInt32(dr["BatchSchedulerID"])))
                            importBatchData.Add(Convert.ToInt32(dr["BatchSchedulerID"]), dr["FormatName"].ToString() + Seperators.SEPERATOR_6 + dr["CronExpression"].ToString());
                    }
                }


                importBatchData.Add(-1, "Manual Operation");
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

            return importBatchData;


        }

        #endregion


        #region Private Task info methods

        private static TaskInfo GetTaskInfoForImport()
        {
            TaskInfo info = new TaskInfo();

            try
            {
                info.TaskId = 1;
                info.TaskName = "Import";
                info.AssemblyName = "Prana.Import.dll";
                info.QClassName = "Prana.Import.ImportManager";
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

            return info;
        }


        private static TaskInfo GetTaskInfoForRecon()
        {
            TaskInfo info = new TaskInfo();

            try
            {
                info.TaskId = 2;
                info.TaskName = "Recon";
                info.AssemblyName = "Prana.Import.dll";
                info.QClassName = "Prana.Import.ImportManager";
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

            return info;
        }

        private static TaskInfo GetTaskInfoForSM()
        {
            TaskInfo info = new TaskInfo();

            try
            {
                info.TaskId = 3;
                info.TaskName = "SM";
                info.AssemblyName = "Prana.Import.dll";
                info.QClassName = "Prana.Import.ImportManager";
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

            return info;
        }

        #endregion
    }
}