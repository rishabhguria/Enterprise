using System;
using System.IO;
using System.Windows;
using WinSCP;

namespace Prana.Utilities.FTP
{
    public static class FtpUploader
    {
        public static void UploadFileToFtp(string filePath, string host, string username, string password, string privateKeyPath, string passphrase, string directory, string protocol)
        {
            try
            {
                switch (protocol.ToUpper())
                {
                    case "SFTP":
                        // SFTP with password authentication
                        using (Session session = new Session())
                        {
                            // Define session options
                            SessionOptions sessionOptions = new SessionOptions
                            {
                                Protocol = Protocol.Sftp,
                                HostName = host,
                                UserName = username,
                                Password = password,
                                GiveUpSecurityAndAcceptAnySshHostKey = true
                            };

                            // Open the session
                            session.Open(sessionOptions);

                            // Build the remote file path
                            string remoteFilePath = directory.TrimEnd('/') + "/" + System.IO.Path.GetFileName(filePath);

                            // Set transfer options to ignore permission and timestamp errors
                            TransferOptions transferOptions = new TransferOptions
                            {
                                TransferMode = TransferMode.Binary,
                                PreserveTimestamp = false,
                                OverwriteMode = OverwriteMode.Overwrite
                            };

                            // Upload the file
                            session.PutFiles(filePath, remoteFilePath, false, transferOptions).Check();

                        }
                        break;

                    case "SFTPKEY":
                        // SFTP with private key authentication
                        if (!File.Exists(privateKeyPath))
                            throw new FileNotFoundException("Private key file not found.");

                        using (Session session = new Session())
                        {
                            session.Open(new SessionOptions
                            {
                                Protocol = Protocol.Sftp,
                                HostName = host,
                                UserName = username,
                                SshPrivateKeyPath = privateKeyPath,
                                PrivateKeyPassphrase = passphrase, // Optional: set passphrase if required
                                GiveUpSecurityAndAcceptAnySshHostKey = true
                            });

                            string remoteFilePath = directory.TrimEnd('/') + "/" + Path.GetFileName(filePath);
                            // Set transfer options to ignore permission and timestamp errors
                            TransferOptions transferOptions = new TransferOptions
                            {
                                TransferMode = TransferMode.Binary,
                                PreserveTimestamp = false,
                                OverwriteMode = OverwriteMode.Overwrite
                            };
                            session.PutFiles(filePath, remoteFilePath, false, transferOptions).Check();
                        }
                        break;

                    case "FTP":
                        // FTP with password authentication
                        using (Session session = new Session())
                        {
                            session.Open(new SessionOptions
                            {
                                Protocol = Protocol.Ftp,
                                HostName = host,
                                UserName = username,
                                Password = password
                            });

                            string remoteFilePath = directory.TrimEnd('/') + "/" + Path.GetFileName(filePath);
                            session.PutFiles(filePath, remoteFilePath).Check();
                        }
                        break;

                    default:
                        throw new ArgumentException("Invalid protocol specified.");
                }

                System.Windows.MessageBox.Show("File successfully uploaded via " + protocol, "Upload Status", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error uploading file via " + protocol + ": " + ex.Message, "Upload Status", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}