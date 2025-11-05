using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WinSCP;

namespace Prana.Utilities.MiscUtilities
{

    public class NirvanaWinSCPUtility
    {
        private SessionOptions sessionOptions = null;
        bool _DisableFilePermissionChangeAndPreserveTimestampForFTP = false;
        string _transferResumeSupportStateForFTP = "Default";
        bool _isLoggingEnabled = true;
        /// <summary>
        /// status event handler to set status of current event occurring/occurred, on output log tab
        /// </summary>
        // public EventHandler<StatusEventArgs> Status;

        /// <summary>
        /// Initializes a sessionOptions <see cref="NirvanaWinSCPUtility"/>
        /// </summary>
        /// <param name="FTPSettings">FTP</param>
        /// <param name="Status">Status Event Handler</param>
        public NirvanaWinSCPUtility(ThirdPartyFtp FTPSettings)
        {
            try
            {

                sessionOptions = new SessionOptions();
                sessionOptions.PortNumber = FTPSettings.Port;
                sessionOptions.HostName = FTPSettings.Host;
                sessionOptions.UserName = FTPSettings.UserName;
                sessionOptions.Password = FTPSettings.Password;

                if (FTPSettings.FtpType != null && FTPSettings.FtpType.Equals("FTP", StringComparison.OrdinalIgnoreCase))
                {
                    sessionOptions.Protocol = Protocol.Ftp;

                    //FTPS functionality - https://jira.nirvanasolutions.com:8443/browse/PRANA-40969
                    if (FTPSettings.Encryption != null && FTPSettings.Encryption.Equals(ThirdPartyConstants.FTP_ENCRYPTION_IMPLICIT, StringComparison.OrdinalIgnoreCase))
                    {
                        sessionOptions.FtpSecure = FtpSecure.Implicit;
                        if (!string.IsNullOrEmpty(FTPSettings.KeyFingerPrint))
                        {
                            sessionOptions.TlsHostCertificateFingerprint = FTPSettings.KeyFingerPrint;
                        }
                    }
                    else if (FTPSettings.Encryption != null && FTPSettings.Encryption.Equals(ThirdPartyConstants.FTP_ENCRYPTION_EXPLICIT, StringComparison.OrdinalIgnoreCase))
                    {
                        sessionOptions.FtpSecure = FtpSecure.Explicit;
                        if (!string.IsNullOrEmpty(FTPSettings.KeyFingerPrint))
                        {
                            sessionOptions.TlsHostCertificateFingerprint = FTPSettings.KeyFingerPrint;
                        }
                    }
                    else if (FTPSettings.Encryption != null && FTPSettings.Encryption.Equals(ThirdPartyConstants.FTP_ENCRYPTION_NONE, StringComparison.OrdinalIgnoreCase))
                        sessionOptions.FtpSecure = FtpSecure.None;
                }
                else if (FTPSettings.FtpType != null && FTPSettings.FtpType.Equals("SCP", StringComparison.OrdinalIgnoreCase))
                {
                    sessionOptions.Protocol = Protocol.Scp;
                    sessionOptions.SshHostKeyFingerprint = FTPSettings.KeyFingerPrint;
                }
                else if (FTPSettings.FtpType != null && FTPSettings.FtpType.Equals("SFTP", StringComparison.OrdinalIgnoreCase))
                {
                    sessionOptions.Protocol = Protocol.Sftp;
                    if (!string.IsNullOrEmpty(FTPSettings.KeyFingerPrint))
                    {
                        sessionOptions.SshHostKeyFingerprint = FTPSettings.KeyFingerPrint;
                    }
                    else
                        sessionOptions.SshHostKeyFingerprint = string.Empty;
                }
                else if (FTPSettings.FtpType != null && FTPSettings.FtpType.Equals("SFTPPasswordLess", StringComparison.OrdinalIgnoreCase))
                {
                    sessionOptions.Protocol = Protocol.Sftp;
                    if (!string.IsNullOrEmpty(FTPSettings.KeyFingerPrint))
                    {
                        sessionOptions.SshHostKeyFingerprint = FTPSettings.KeyFingerPrint;
                    }
                    else
                        sessionOptions.SshHostKeyFingerprint = string.Empty;

                    if (!string.IsNullOrEmpty(FTPSettings.PassPhrase))
                    {
                        sessionOptions.PrivateKeyPassphrase = FTPSettings.PassPhrase;
                    }
                    else
                        sessionOptions.PrivateKeyPassphrase = string.Empty;

                    if (!string.IsNullOrEmpty(FTPSettings.SshPrivateKeyPath))//it will be key path
                    {
                        sessionOptions.SshPrivateKeyPath = FTPSettings.SshPrivateKeyPath;//it will be key path
                    }
                    else
                        sessionOptions.SshPrivateKeyPath = string.Empty;
                }
                bool.TryParse(ConfigurationHelper.Instance.GetAppSettingValueByKey("DisableFilePermissionChangeAndPreserveTimestampForFTP"), out _DisableFilePermissionChangeAndPreserveTimestampForFTP);

                if (!String.IsNullOrEmpty(ConfigurationHelper.Instance.GetAppSettingValueByKey("TransferResumeSupportStateForFTP")))
                {
                    _transferResumeSupportStateForFTP = ConfigurationHelper.Instance.GetAppSettingValueByKey("TransferResumeSupportStateForFTP");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

        }

        /// <summary>
        /// Initializes a sessionOptions <see cref="NirvanaWinSCPUtility"/>
        /// </summary>
        /// <param name="FTPSettings">FTP</param>
        /// <param name="Status">Status Event Handler</param>
        public NirvanaWinSCPUtility(ThirdPartyFtp FTPSettings, bool isLoggingEnabled) : this(FTPSettings)
        {
            _isLoggingEnabled = isLoggingEnabled;
        }

        /// <summary>
        /// Check in same day File sent or not ir sent then return true
        /// </summary>
        /// <param name="SourceFilePath">Soruce file Path</param>
        /// <param name="DestinationPath">Destination File Path</param>
        /// <returns>true if already sent otherwise false</returns>
        public bool IsAlreadySend(String SourceFilePath, String DestinationPath)
        {
            bool isAlreadySend = false;
            DateTime lastModifieddate = new DateTime();
            try
            {
                if (sessionOptions != null)
                {
                    using (Session session = new Session())
                    {
                        bool isSessionOpened = OpenSession(session);
                        if (isSessionOpened)
                        {

                            string fileName = Path.GetFileName(SourceFilePath);
                            if (session.FileExists(DestinationPath + fileName))
                            {
                                RemoteFileInfo rf = session.GetFileInfo(DestinationPath + fileName);
                                lastModifieddate = rf.LastWriteTime;
                                if (lastModifieddate.Date == System.DateTime.Now.Date)
                                {
                                    isAlreadySend = true;
                                }
                            }

                        }
                    }
                }
                return isAlreadySend;
            }
            catch (Exception ex)
            {
                //Status(this, new StatusEventArgs("Error in sending file."));

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// upload files on FTP server with gived FTP settings
        /// </summary>
        /// <param name="SourceFilePath"></param>
        /// <param name="DestinationPath"></param>
        /// <returns></returns>
        public string SendFile(String SourceFilePath, String DestinationPath)
        {
            StringBuilder transferResultSb = new StringBuilder();
            try
            {
                if (sessionOptions != null)
                {
                    using (Session session = new Session())
                    {
                        // Connect
                        bool isSessionOpened = OpenSession(session);
                        if (isSessionOpened)
                        {
                            // Upload files
                            TransferOptions transferOptions = new TransferOptions();
                            transferOptions.TransferMode = TransferMode.Automatic;

                            //PRANA-40897
                            switch (_transferResumeSupportStateForFTP.ToLower())
                            {
                                case "on":
                                    transferOptions.ResumeSupport.State = TransferResumeSupportState.On;
                                    break;
                                case "off":
                                    transferOptions.ResumeSupport.State = TransferResumeSupportState.Off;
                                    break;
                                default:
                                    transferOptions.ResumeSupport.State = TransferResumeSupportState.Default;
                                    break;
                            }

                            //Made changes to resolve PRANA-23664.
                            if (_DisableFilePermissionChangeAndPreserveTimestampForFTP)
                            {
                                transferOptions.FilePermissions = null;
                                transferOptions.PreserveTimestamp = false;
                            }

                            TransferOperationResult transferResult;
                            transferResult = session.PutFiles(SourceFilePath, DestinationPath, false, transferOptions);
                            // Throw on any error
                            transferResult.Check();
                            // Print results
                            foreach (TransferEventArgs transfer in transferResult.Transfers)
                            {
                                transferResultSb.Append(Path.GetFileName(transfer.FileName) + " file successfuly sent to FTP server on path " + transfer.Destination);
                                transferResultSb.Append(System.Environment.NewLine);
                                //Status(this, new StatusEventArgs(Path.GetFileName(transfer.FileName) + " file successfuly sent to FTP server on path " + transfer.Destination));
                            }
                        }
                    }
                }
                return transferResultSb.ToString();
            }
            catch (Exception ex)
            {
                //Status(this, new StatusEventArgs("Error in sending file."));

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return string.Empty;
            }
        }


        /// <summary>
        /// Download files from FTP server with given FTP settings
        /// </summary>
        /// <param name="SourceFilePath"></param>
        /// <param name="DestinationPath"></param>
        /// <returns></returns>
        public bool ReceiveFile(string ftpFilePath, out String downloadedFileName, String DestinationPath)
        {
            bool sendingStatus = false;
            downloadedFileName = string.Empty;
            try
            {
                TransformWindowsPathToFtpPath(ref ftpFilePath);
                if (sessionOptions != null)
                {
                    //sessionOptions.GiveUpSecurityAndAcceptAnySshHostKey = true;
                    using (Session session = new Session())
                    {
                        // Connect
                        bool isSessionOpened = OpenSession(session);
                        if (isSessionOpened)
                        {
                            //if (Status != null)
                            //{
                            //    Status(this, new StatusEventArgs("Sending file to FTP server.."));
                            //}
                            // Upload files
                            TransferOptions transferOptions = new TransferOptions();
                            transferOptions.TransferMode = TransferMode.Automatic;

                            TransferOperationResult transferResult;


                            transferResult = session.GetFiles(ftpFilePath, DestinationPath, false, transferOptions);
                            // Throw on any error

                            transferResult.Check();


                            //TODO: Manage logging here for all files

                            // Print results
                            if (transferResult.Transfers.Count > 0)
                            {
                                sendingStatus = true;
                                downloadedFileName = transferResult.Transfers[0].FileName;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Status(this, new StatusEventArgs("Error in sending file."));

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    sendingStatus = false;
                    throw;
                }

            }
            return sendingStatus;
        }

        /// <summary>
        /// Open/Connect session to transfer files on FTP
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private bool OpenSession(Session session)
        {
            try
            {
                if (_isLoggingEnabled)
                {
                    string logsDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\FTPLogs\\";
                    if (!Directory.Exists(logsDirectory))
                        Directory.CreateDirectory(logsDirectory);
                    session.SessionLogPath = logsDirectory + "FTPSessionLog" + DateTime.Now.ToString("ddMMyyyyhhmmssfff") + ".txt";
                    session.DebugLogPath = logsDirectory + "FTPDebugLog" + DateTime.Now.ToString("ddMMyyyyhhmmssfff") + ".txt";
                    session.DebugLogLevel = 2;
                }
                session.Open(sessionOptions);
                return true;
            }
            catch (Exception ex)
            {
                // Status(this, new StatusEventArgs("Error in connecting FTP server! "));
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

        /// <summary>
        /// added by: Bharat Raturi, 02 jun 2014
        /// purpose: get the list of files from the specified folder
        /// </summary>
        /// <param name="uName">UserName for FTP login</param>
        /// <param name="pWord">password for FTP login</param>
        /// <param name="ftpName">Name of the FTP</param>
        /// <returns>list of strings</returns>
        public Dictionary<string, DateTime> ListDirectory(string folderPath, Dictionary<string, DateTime> dictFileNames)
        {
            Dictionary<string, DateTime> dict = new Dictionary<string, DateTime>();
            try
            {
                if (sessionOptions != null)
                {
                    //sessionOptions.GiveUpSecurityAndAcceptAnySshHostKey = true;
                    using (Session session = new Session())
                    {
                        // Connect
                        bool isSessionOpened = OpenSession(session);
                        // Added By : Manvendra P.
                        // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3542
                        TransformWindowsPathToFtpPath(ref folderPath);
                        if (isSessionOpened && session.FileExists(folderPath))
                        {
                            RemoteDirectoryInfo dirList = session.ListDirectory(folderPath);
                            foreach (RemoteFileInfo file in dirList.Files)
                            {
                                string fileName = file.Name;
                                //Compare ignoring case
                                if (dictFileNames.Keys.Contains(fileName, StringComparer.OrdinalIgnoreCase))
                                {
                                    dict.Add(fileName, dictFileNames[fileName.ToLower()]);
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
            return dict;
        }

        /// <summary>
        /// Transform windows path to Ftp Path
        /// </summary>
        /// <param name="folderPath"></param>
        private void TransformWindowsPathToFtpPath(ref string folderPath)
        {
            try
            {
                folderPath = (folderPath.Replace(@"\\", @"\")).Replace(@"\", @"/");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public Dictionary<string, DateTime> ListFiles(string folderPath)
        {
            Dictionary<string, DateTime> dict = new Dictionary<string, DateTime>();
            try
            {
                if (sessionOptions != null)
                {
                    //sessionOptions.GiveUpSecurityAndAcceptAnySshHostKey = true;
                    using (Session session = new Session())
                    {
                        // Connect
                        bool isSessionOpened = OpenSession(session);
                        if (isSessionOpened)
                        {
                            RemoteDirectoryInfo dirList = session.ListDirectory(folderPath);
                            foreach (RemoteFileInfo file in dirList.Files)
                            {
                                string fileName = file.Name;
                                //Compare ignoring case
                                //if (dictFileNames.Keys.Contains(fileName, StringComparer.OrdinalIgnoreCase))
                                {
                                    dict.Add(fileName, file.LastWriteTime);
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
            return dict;
        }

        public DateTime GetFileLastChangeDateTime(string ftpFilePath)
        {
            DateTime lastWriteTime = DateTime.MinValue;
            try
            {
                if (sessionOptions != null)
                {
                    //sessionOptions.GiveUpSecurityAndAcceptAnySshHostKey = true;
                    using (Session session = new Session())
                    {
                        // Connect
                        bool isSessionOpened = OpenSession(session);
                        if (isSessionOpened)
                        {
                            RemoteFileInfo fileInfo = session.GetFileInfo(ftpFilePath);
                            if (fileInfo != null)
                            {
                                lastWriteTime = fileInfo.LastWriteTime;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Status(this, new StatusEventArgs("Error in sending file."));

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }

            }
            return lastWriteTime;
        }
    }
}