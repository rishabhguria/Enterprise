using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace Prana.BBGImportManager
{
    public class BBGFileWatcher : IDisposable
    {
        private static BBGFileWatcher _bbgFileWatcher;
        private BackgroundWorker _bgWorker;
        private String _fileBackUpPath = string.Empty;
        private int _bbgFileImportExecuteFor = int.Parse(ConfigurationManager.AppSettings["BBGFileImportExecuteFor"].ToString().Trim());
        private String _bbgFileImportFileName = ConfigurationManager.AppSettings["BBGFileImportFileName"].ToString().Trim();
        private String _bbgFileImportSourceFilePath = ConfigurationManager.AppSettings["BBGFileImportSourceFilePath"].ToString().Trim();
        private FileSystemWatcher _watcher = null;
        private string _bbgFileImportFileType = ConfigurationManager.AppSettings["BBGFileImportFileType"].ToString().Trim();
        private string _bbgFileImportSearchingFileNameLike = ConfigurationManager.AppSettings["BBGFileImportSearchingFileNameOnFTPStartWith"].ToString().Trim();
        public event EventHandler BBGFileImported;
        public static BBGFileWatcher GetInstance()
        {
            try
            {
                if (_bbgFileWatcher == null)
                {
                    _bbgFileWatcher = new BBGFileWatcher();
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
            return _bbgFileWatcher;
        }

        public void SetupBBGFileWatcher()
        {
            try
            {
                if (!string.IsNullOrEmpty(_bbgFileImportSourceFilePath))
                {
                    _fileBackUpPath = _bbgFileImportSourceFilePath + "\\Archived";
                    _bgWorker = new BackgroundWorker();

                    _watcher = new FileSystemWatcher();
                    _watcher.Filter = _bbgFileImportSearchingFileNameLike + "*" + _bbgFileImportFileType;
                    _watcher.Path = _bbgFileImportSourceFilePath + "\\";

                    _watcher.Created += new System.IO.FileSystemEventHandler(this.Watcher_Created);
                    _watcher.NotifyFilter = System.IO.NotifyFilters.FileName;
                    _watcher.EnableRaisingEvents = true;
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

        private void BGWorker_DoWork(object sender, DoWorkEventArgs args)
        {
            FileSystemEventArgs e = null;
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                object[] agrs = args.Argument as object[];
                e = agrs[0] as FileSystemEventArgs;
                String SourceFile = _bbgFileImportSourceFilePath + "\\" + _bbgFileImportFileName;
                int bbgFileImportFTPTimeInterval = int.Parse(ConfigurationManager.AppSettings["BBGFileImportFTPTimeInterval"].ToString());

                //if file TigerVeda.csv file is exists on FTP then delete it first
                if (File.Exists(Path.Combine(_bbgFileImportSourceFilePath, _bbgFileImportFileName)))
                {
                    File.Delete(Path.Combine(_bbgFileImportSourceFilePath, _bbgFileImportFileName));
                }

                string fileName = e.Name;
                int fileExtPos = fileName.LastIndexOf(".");
                if (fileExtPos >= 0)
                    fileName = fileName.Substring(0, fileExtPos);

                string newFileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv";

                String sourcefilepath = _fileBackUpPath + "\\" + newFileName;
                if (!Directory.Exists(_fileBackUpPath))
                {
                    Directory.CreateDirectory(_fileBackUpPath);
                }

                if (File.Exists(Path.Combine(_fileBackUpPath, newFileName)))
                {
                    File.Delete(Path.Combine(_fileBackUpPath, newFileName));
                }
                if (File.Exists(Path.Combine(_fileBackUpPath, e.Name)))
                {
                    File.Delete(Path.Combine(_fileBackUpPath, e.Name));
                }
                if (File.Exists(Path.Combine(_bbgFileImportSourceFilePath, e.Name)))
                {
                    File.Copy(e.FullPath, Path.Combine(_fileBackUpPath, e.Name));
                    File.Move(Path.Combine(_fileBackUpPath, e.Name), Path.Combine(_fileBackUpPath, newFileName));
                    File.Move(Path.Combine(_bbgFileImportSourceFilePath, e.Name), Path.Combine(_bbgFileImportSourceFilePath, _bbgFileImportFileName));
                    int records = int.MinValue, recordsForSM = int.MinValue;

                    if (_bbgFileImportExecuteFor == 1)
                    {
                        records = DirectoryManager.SaveDataIntoDB(SourceFile, ConfigurationManager.AppSettings["BBGFileImportClientSPName"].ToString().Trim());
                    }
                    else if (_bbgFileImportExecuteFor == 2)
                    {
                        recordsForSM = DirectoryManager.SaveDataIntoDB(SourceFile, ConfigurationManager.AppSettings["BBGFileImportSMSPName"].ToString().Trim());
                    }
                    else if (_bbgFileImportExecuteFor == 3)
                    {
                        records = DirectoryManager.SaveDataIntoDB(SourceFile, ConfigurationManager.AppSettings["BBGFileImportClientSPName"].ToString().Trim());
                        recordsForSM = DirectoryManager.SaveDataIntoDB(SourceFile, ConfigurationManager.AppSettings["BBGFileImportSMSPName"].ToString().Trim());
                    }

                    if (records > 0 && recordsForSM > 0 && _bbgFileImportExecuteFor == 3 || recordsForSM > 0 && _bbgFileImportExecuteFor == 2 || records > 0 && _bbgFileImportExecuteFor == 1)
                    {
                        if (File.Exists(SourceFile))
                        {
                            File.Delete(SourceFile);
                        }
                        if (BBGFileImported != null)
                        {
                            BBGFileImported(this, null);
                            SendMailToUser(newFileName);
                        }
                    }
                    else
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("No data has been imported through BBG File Import", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
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

        private void SendMailToUser(string fileNameToBeSend)
        {
            try
            {
                string filePath = _fileBackUpPath;
                string fileName = fileNameToBeSend;
                string bbgFileImportReceiverIDs = ConfigurationManager.AppSettings["BBGFileImportReceiverIDs"].ToString().Trim();
                string bbgFileImportSenderID = ConfigurationManager.AppSettings["BBGFileImportSenderID"].ToString().Trim();
                string bbgFileImportMailSubject = ConfigurationManager.AppSettings["BBGFileImportMailSubject"].ToString().Trim();
                string bbgFileImportMailBody = ConfigurationManager.AppSettings["BBGFileImportMailBody"].ToString().Trim();
                bool bbgFileImportSecureEmail = Convert.ToBoolean(ConfigurationManager.AppSettings["BBGFileImportSecureEmail"].ToString().Trim());
                string bbgFileImportCCIDs = ConfigurationManager.AppSettings["BBGFileImportCCIDs"].ToString().Trim();
                string bbgFileImportBCCIDs = ConfigurationManager.AppSettings["BBGFileImportBCCIDs"].ToString().Trim();
                string bbgFileImportSenderPWD = ConfigurationManager.AppSettings["BBGFileImportSenderPWD"].ToString().Trim();
                string bbgFileImportMailServer = ConfigurationManager.AppSettings["BBGFileImportMailServer"].ToString().Trim();
                int bbgFileImportMailServerSMTPPort = int.Parse(ConfigurationManager.AppSettings["BBGFileImportMailServerSMTPPort"].ToString().Trim());

                if (!string.IsNullOrEmpty(bbgFileImportReceiverIDs))
                    EmailsHelper.SendMail(filePath, fileName, bbgFileImportReceiverIDs, bbgFileImportSenderID, bbgFileImportMailSubject, bbgFileImportMailBody, bbgFileImportCCIDs, bbgFileImportBCCIDs, bbgFileImportSenderPWD, bbgFileImportMailServer, bbgFileImportMailServerSMTPPort, bbgFileImportSecureEmail);
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

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (!_bgWorker.IsBusy)
                {
                    _bgWorker.DoWork -= new DoWorkEventHandler(BGWorker_DoWork);
                    _bgWorker.DoWork += new DoWorkEventHandler(BGWorker_DoWork);
                    object[] args = new object[1];
                    args[0] = e;
                    _bgWorker.RunWorkerAsync(args);
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

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_bgWorker != null)
            {
                _bgWorker.Dispose();
                _watcher.Dispose();
            }
        }
        #endregion IDisposable Members
    }
}