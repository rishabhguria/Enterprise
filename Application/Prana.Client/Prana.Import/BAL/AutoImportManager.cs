using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.Global;
using Prana.Import.BAL;
using Prana.Import.Helper;
using Prana.LogManager;
using Prana.TaskManagement.Definition.Definition;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Prana.Import
{
    public class AutoImportManager
    {
        #region singleton
        private static volatile AutoImportManager instance;
        private static object syncRoot = new Object();
        private string _senderID = ConfigurationHelper.Instance.GetAppSettingValueByKey("StagedOrderAutoImportSenderID");
        private string _mailServer = ConfigurationHelper.Instance.GetAppSettingValueByKey("StagedOrderAutoImportMailServer");
        private int _mailServerSMTPPort = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("StagedOrderAutoImportMailServerSMTPPort"));
        private string _senderPWD = ConfigurationHelper.Instance.GetAppSettingValueByKey("StagedOrderAutoImportSenderPWD");
        private bool _enableSSL = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("StagedOrderAutoImportEnableSSL"));
        private string _receiverIDs = ConfigurationHelper.Instance.GetAppSettingValueByKey("StagedOrderAutoImportReceiverIDs");
        private string _mailSubject = ConfigurationHelper.Instance.GetAppSettingValueByKey("StagedOrderAutoImportMailSubject");
        private bool _authenticationRequired = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("StagedOrderAutoImportAuthenticationRequired"));

        private bool _isStagedOrderAutoImportMailEnabled = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsStagedOrderAutoImportMailEnabled"));
        private int _stagedOrderAutoImportQtyColNum = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("StagedOrderAutoImportQtyColNum"));

        private const string IMPORT_FAILED_MESSAGE = "<b>Please contact your support team to rectify the problem.</b><br />";
        private const string IMPORT_PARTIALLY_SUCCESS = "<b>Please contact your support team to rectify the problem.</b><br />";
        private const string IMPORT_SUCCESSFUL = "<b>Please be sure to cross check the total share quantity on the Blotter <u>before sending your orders to the market with your records for accuracy.</u> Please contact your support team if there are differences.</b><br />";


        private AutoImportManager() { }

        public static AutoImportManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AutoImportManager();
                    }
                }

                return instance;
            }
        }
        #endregion

        /// <summary>
        /// AutoImport Completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AutoImportManager_ImportCompleted(object sender, EventArgs<TaskResult> e)
        {
            try
            {
                if (e != null)
                {
                    TaskResult taskResult = e.Value;
                    if (taskResult != null)
                    {
                        //Currently working only for the staged auto-import
                        string importFileName = e.Value.TaskStatistics.TaskSpecificData.GetValueForKey("RunUploadFileName").ToString();
                        ImportType importTypeAcronym = (ImportType)(Enum.Parse(typeof(ImportType), e.Value.TaskStatistics.TaskSpecificData.GetValueForKey("RunUploadImportTypeAcronym").ToString()));
                        if (_isStagedOrderAutoImportMailEnabled && importTypeAcronym == ImportType.StagedOrder)
                        {
                            string mailSubject = _mailSubject + " - " + importFileName;
                            string mailMessage = "<b>Uploaded at - " + taskResult.TaskStatistics.StartTime.ToString() + "</b><br />";
                            if (taskResult.Error != null)
                            {
                                //File failed to upload because of some exception
                                mailMessage += "File Failed completely<br /><br />" + IMPORT_FAILED_MESSAGE;
                            }
                            else
                            {
                                DataTable dtData = taskResult.TaskStatistics.TaskSpecificData.GetRefStatisticsData()["OriginalData"] as DataTable;
                                double cumQtySumFromFile = 0.0;
                                foreach (DataRow row in dtData.Rows)
                                {
                                    double value = 0.0;
                                    //Summing the Quantity column of the input csv file defined in the config section
                                    Double.TryParse(row[_stagedOrderAutoImportQtyColNum].ToString(), out value);
                                    cumQtySumFromFile += Math.Abs(value);
                                }

                                double quantitySumFromValidatedOrders = 0.0;
                                Double.TryParse(e.Value.TaskStatistics.TaskSpecificData.GetValueForKey("QuantitySumFromValidatedOrders").ToString(), out quantitySumFromValidatedOrders);


                                if (quantitySumFromValidatedOrders == 0)
                                    mailMessage += "File Failed completely.<br /><br />" + IMPORT_FAILED_MESSAGE;
                                else if (cumQtySumFromFile == quantitySumFromValidatedOrders)
                                    mailMessage += "File Uploaded Succesfully. Total Count of " + quantitySumFromValidatedOrders + " shares uploaded.<br /><br />" + IMPORT_SUCCESSFUL;                              
                                else
                                    mailMessage += "File Partially Uploaded. Total Count of " + quantitySumFromValidatedOrders + " shares uploaded.<br /><br />" + IMPORT_PARTIALLY_SUCCESS;
                            }
                            SendReconcillationEmail(mailSubject, mailMessage);
                        }
                        ImportLoggingHelper.LoggerWriteMessage(importTypeAcronym.ToString(), importFileName, ImportLoggingHelper.IMPORT_ENDED, string.Empty);

                    }
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            finally
            {
                ImportManager.Instance.ImportCompleted -= new EventHandler<EventArgs<TaskResult>>(AutoImportManager_ImportCompleted);
            }
        }

        private async void SendReconcillationEmail(string mailSubject, string body)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                        EmailsHelper.MailSend(mailSubject, body, _senderID, "Nirvana Support", _senderPWD, _receiverIDs.Split(','), _mailServerSMTPPort, _mailServer, _enableSSL, _authenticationRequired, true)
                    );
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }



        public void StartWatchersForAutoImport()
        {
            try
            {
                if (ImportSetUpManager.Instance.AutoImportList.Count > 0)
                {
                    foreach (RunUpload rn in ImportSetUpManager.Instance.AutoImportList)
                    {
                        if (!Directory.Exists(rn.DirPath))
                        {
                            Logger.LoggerWrite("The directory specified for Auto Import of files not found." + Environment.NewLine + "Directoy path:" + rn.DirPath);
                        }
                        else
                        {
                            ImportAlreadyPlacedFiles(rn);
                            rn.StartService();
                            rn.File_Created += rn_File_Created;
                            Logger.LoggerWrite("[REBALANCER] File watcher created for directory " + rn.DirPath);
                        }
                    }
                }
                foreach (RunUpload rn in ImportSetUpManager.Instance.FTPImportList)
                {
                    FTPTradeImportWatcher.Instance.AddRunUpload(rn);
                }
                SetSecurityMasterForHandlers();
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

        public void SetSecurityMasterForHandlers()
        {
            try
            {
                //StagedOrderHandler.Instance.WireEvents();
                //AllocationSchemeHandler.Instance.WireEvents();
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
        void rn_File_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                RunUpload runUpload = sender as RunUpload;
                ImportLoggingHelper.LoggerWriteMessage(runUpload.ImportTypeAcronym.ToString(), runUpload.FileName, ImportLoggingHelper.IMPORT_STARTED, string.Empty);
                runUpload.ProcessedFilePath = runUpload.FilePath;
                TaskResult taskResult = null;
                if (runUpload.ImportTypeAcronym == ImportType.StagedOrder)
                {
                    taskResult = new TaskResult();
                    taskResult.TaskStatistics.StartTime = DateTime.Now;
                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("IsAutoImport", true, null);
                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("RunUploadImportTypeAcronym", runUpload.ImportTypeAcronym.ToString(), null);
                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("RunUploadFileName", runUpload.FileName, null);
                    ImportManager.Instance.ImportCompleted += new EventHandler<EventArgs<TaskResult>>(AutoImportManager_ImportCompleted);
                }
                ImportManager.Instance.UploadDataThruLocalFile(runUpload, taskResult, true);
                if (runUpload.ImportTypeAcronym == ImportType.StagedOrder)
                {
                    if (taskResult.Error != null)
                    {
                        AutoImportManager_ImportCompleted(this, new EventArgs<TaskResult>(taskResult));
                        ImportLoggingHelper.LoggerWriteMessage(runUpload.ImportTypeAcronym.ToString(), runUpload.FileName, 
                            ImportLoggingHelper.EXCEPTION, "[Import Error] "+taskResult.Error.Message);
                            
                    }
                }
                else
                {
                    ImportLoggingHelper.LoggerWriteMessage(runUpload.ImportTypeAcronym.ToString(), runUpload.FileName, ImportLoggingHelper.IMPORT_ENDED, string.Empty);
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

        private void ImportAlreadyPlacedFiles(RunUpload runUpload)
        {
            try
            {
                Dictionary<string, DateTime> fileAlreadyImported = new Dictionary<string, DateTime>();
                Dictionary<string, DateTime> filesToBeImported = new Dictionary<string, DateTime>();
                string[] allFiles = Directory.GetFiles(runUpload.DirPath, "*.*");
                DataSet dsImportedFileDetails = ImportDataManager.GetImportedFileDetails(runUpload.DirPath);
                for (int counter = 0; counter < dsImportedFileDetails.Tables[0].Rows.Count; counter++)
                {
                    if (!fileAlreadyImported.ContainsKey(Convert.ToString(dsImportedFileDetails.Tables[0].Rows[counter]["ImportFileName"])))
                        fileAlreadyImported.Add(Convert.ToString(dsImportedFileDetails.Tables[0].Rows[counter]["ImportFileName"]), Convert.ToDateTime(dsImportedFileDetails.Tables[0].Rows[counter]["ImportFileLastModifiedUTCTime"]));
                }

                foreach (string file in allFiles)
                {
                    if (!fileAlreadyImported.ContainsKey(Path.GetFileName(file)))
                        filesToBeImported.Add(Path.GetFileName(file), DateTime.UtcNow);
                }

                foreach (KeyValuePair<string, DateTime> kvp in filesToBeImported)
                {
                    runUpload.ImportSource = ImportSource.Automatic;
                    runUpload.FilePath = runUpload.DirPath + "\\" + kvp.Key;
                    runUpload.FileName = kvp.Key;
                    runUpload.FileLastModifiedUTCTime = File.GetLastWriteTime(runUpload.FilePath);
                    rn_File_Created(runUpload, null);
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
