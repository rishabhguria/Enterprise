using Prana.BusinessObjects;
using Prana.BusinessObjects.PositionManagement;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Import.Helper;
using Prana.LogManager;
using Prana.PM.DAL;
using Prana.TaskManagement.Definition.Definition;
using Prana.Utilities.MiscUtilities.FTPUtility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;

namespace Prana.Import.BAL
{
    public class FTPTradeImportWatcher : IDisposable
    {
        private static readonly FTPTradeImportWatcher instance = new FTPTradeImportWatcher();
        public static FTPTradeImportWatcher Instance
        {
            get { return instance; }
        }
        private FTPTradeImportWatcher()
        {
            PositionMaster.AccountsList = CachedDataManager.GetInstance.GetUserAccounts();
            PositionMaster.TotalAccounts = CachedDataManager.GetInstance.GetAllAccountsCount();
            lstAllPricesFileProcessed = RunUploadManager.GetImportedFilesForType(BusinessObjects.AppConstants.ImportType.Transaction);
            _thirdPartyFtp = GetFTPDetails();
            fileDataHubTimer = new System.Threading.Timer(fileDataHubTimer_TimerTickHandler, null, 0, _importFileReadInterval);
        }

        private System.Threading.Timer fileDataHubTimer = null;
        private readonly object _timerLocker = new object();
        private ThirdPartyFtp _thirdPartyFtp = null;

        private ThirdPartyFtp GetFTPDetails()
        {
            ThirdPartyFtp thirdPartyFtp = new ThirdPartyFtp();
            try
            {
                NameValueCollection collection = ConfigurationHelper.Instance.GetSectionBySectionName(ConfigurationHelper.SECTION_DataHubTradeImport);
                thirdPartyFtp.FtpName = collection[ConfigurationHelper.CONFIG_APPSETTING_FtpName];
                thirdPartyFtp.FtpType = collection[ConfigurationHelper.CONFIG_APPSETTING_FtpType];
                thirdPartyFtp.Host = collection[ConfigurationHelper.CONFIG_APPSETTING_Host];
                thirdPartyFtp.Port = Convert.ToInt32(collection[ConfigurationHelper.CONFIG_APPSETTING_Port]);
                thirdPartyFtp.UsePassive = Convert.ToBoolean(collection[ConfigurationHelper.CONFIG_APPSETTING_UsePassive]);
                thirdPartyFtp.UserName = collection[ConfigurationHelper.CONFIG_APPSETTING_UserName];
                thirdPartyFtp.PassPhrase = collection[ConfigurationHelper.CONFIG_APPSETTING_PassPhrase];
                thirdPartyFtp.Password = collection[ConfigurationHelper.CONFIG_APPSETTING_Password];
                thirdPartyFtp.KeyFingerPrint = collection[ConfigurationHelper.CONFIG_APPSETTING_KeyFile];
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
            return thirdPartyFtp;
        }

        private int _importFileReadInterval = Convert.ToInt32(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_DataHubTradeImport, "FileReadInterval"));
        List<string> lstAllPricesFileProcessed = new List<string>();
        List<RunUpload> _ftpImports = new List<RunUpload>();

        public void AddRunUpload(RunUpload runUpload)
        {
            string directory = Path.Combine(Application.StartupPath, runUpload.TableFormatName + "\\");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            else
            {
                FileInfo[] files = new DirectoryInfo(directory).GetFiles();
                foreach (FileInfo file in files)
                {
                    file.Delete();
                }
            }
            lock (_timerLocker)
            {
                _ftpImports.Add(runUpload);
            }
        }

        private void fileDataHubTimer_TimerTickHandler(object state)
        {
            try
            {
                lock (_timerLocker)
                {
                    foreach (RunUpload runUpload in _ftpImports)
                    {
                        List<string> lstFiles = FTPHelper.GetFilesListOnFTP(_thirdPartyFtp, runUpload.FTPWatcherFilePath, false, -1, ref lstAllPricesFileProcessed, false);
                        foreach (string filePath in lstFiles)
                        {
                            string downloadedFile;
                            if (FTPHelper.DownloadFile(_thirdPartyFtp, filePath, out downloadedFile, false, runUpload.TableFormatName))
                            {
                                RunUpload clone = DeepCopyHelper.Clone(runUpload);
                                clone.ProcessedFilePath = downloadedFile;
                                clone.FilePath = downloadedFile;
                                clone.RawFilePath = filePath;
                                TaskResult taskResult = new TaskResult();
                                BusinessObjects.Classes.NAVLockDateRule.NAVLockDate = CachedDataManager.GetInstance.NAVLockDate;
                                ImportManager.Instance.UploadDataThruLocalFile(clone, taskResult, true);
                                object status = taskResult.TaskStatistics.TaskSpecificData.GetValueForKey("Status");
                                if(taskResult.TaskStatistics.TaskSpecificData.GetValueForKey("IsNavLockDateValidated") != null)
                                {
                                    string mailBody = "For some of the orders the date in the file " + Path.GetFileName(filePath) + " to for this import precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString() + "). Please investigate it at the earliest.";
                                    FTPTradeEmailHelper.SendErrorEmail(downloadedFile, mailBody);
                                }
                                else if (status == null || status.ToString() != BusinessObjects.AppConstants.NirvanaTaskStatus.Importing.ToString())
                                {
                                    //Send mail with downloaded file                               
                                    FTPTradeEmailHelper.SendErrorEmail(downloadedFile);
                                }
                            }
                        }
                    }
                }
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

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (fileDataHubTimer != null)
                {
                    fileDataHubTimer.Dispose();
                    fileDataHubTimer = null;
                }
            }
        }

        #endregion
    }
}
