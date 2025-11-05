using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using WinSCP;

namespace BusinessObjects
{

    /// <summary>
    /// Class to connect with FTP servers using WinSCp
    /// </summary>
    public class NirvanaWinSCPUtility
    {
        /// <summary>
        /// The session options
        /// </summary>
        private SessionOptions sessionOptions = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="NirvanaWinSCPUtility"/> class.
        /// </summary>
        /// <param name="FTPSettings">The FTP settings.</param>
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
            }
            catch (Exception ex)
            {
                    throw ex;
            }

        }

        /// <summary>
        /// upload files on FTP server with gived FTP settings
        /// </summary>
        /// <param name="SourceFilePath">The source file path.</param>
        /// <param name="DestinationPath">The destination path.</param>
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
            }
            catch (Exception ex)
            {
                //Status(this, new StatusEventArgs("Error in sending file."));

                throw ex;
            }
            return transferResultSb.ToString();
        }

        /// <summary>
        /// Open/Connect session to transfer files on FTP
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        private bool OpenSession(Session session)
        {
            try
            {
                //session.SessionLogPath = Application.StartupPath + "\\Logs\\ThirdPartySessionLog" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
                //session.DebugLogPath = Application.StartupPath + "\\Logs\\ThirdPartyDebugLog" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".txt";
                session.DebugLogLevel = 2;
                session.Open(sessionOptions);
                return true;
            }
            catch (Exception ex)
            {
                // Status(this, new StatusEventArgs("Error in connecting FTP server! "));
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                throw ex;
            }
        }

        /// <summary>
        /// Deletes the files.
        /// </summary>
        /// <param name="DestinationPath">The destination path.</param>
        public void DeleteFiles(string DestinationPath)
        {
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

                            RemovalOperationResult transferResult;
                            transferResult = session.RemoveFiles(DestinationPath);
                            // Throw on any error
                            transferResult.Check();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

    }
        
}