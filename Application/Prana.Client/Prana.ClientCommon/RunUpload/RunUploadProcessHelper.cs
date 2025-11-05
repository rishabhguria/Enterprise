using Cryptography.GnuPG;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.LogManager;
using Prana.Utilities.ImportExportUtilities;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Prana.ClientCommon
{
    public class RunUploadProcessHelper
    {

        public static string ParseFileName(RunUpload runUpload, DateTime date)
        {
            string fileName = string.Empty;
            try
            {
                #region Commented
                //We have tried all the date formats
                //We can use wild-card characters * and ? here, same as windows
                //string fileNameBeforeParsing = "a{M/d/yyyy}a{M/d/yy}a{MM/dd/yy}a{MM/dd/yy}a{yy/MM/dd}a{yyyy-MM-dd}a{dd-MMM-yy}a{dd}a{mmm}a{yy}a{yyyy}a{mm}a{}.csv";
                //string fileNameBeforeParsing = "ENLANDER_cash_{yyyymmdd}.CSV???????????????????";
                //string fileNameBeforeParsing = "ENLANDER_cash_{yyyymmdd}.CSV*";
                //TODO: In the englander file there are two dates in the middle there are process date (for which date data is) and in the end there is date when file is saved on FTP
                //string fileNameBeforeParsing = "ENLANDER_trades_{yyyymmdd}.CSV*";

                //Value of FtpFilePath field is in runUpload.FileName               

                //TODO: DefaultEquityAUECID is passed to Apply last business day logic while importing data

                //  DefaultEquityAUECID = int.Parse(ConfigurationManager.AppSettings["DefaultEquityAUECID"]);
                // DateTime LastBusinessDay = BusinessDayCalculator.GetInstance().GetPreviousBusinessDay(DateTime.UtcNow.Date, DefaultEquityAUECID);
                #endregion
                fileName = FileNameParser.GetFileNameFromNamingConvention(runUpload.FtpFilePath, date);
                #region Commented
                //fileName = Path.GetFileName(runUpload.FtpFilePath);
                //string dateformat = fileName.Substring(fileName.IndexOf('{'), fileName.IndexOf('}') - fileName.IndexOf('{'));
                //fileName = fileName.Substring(0, fileName.IndexOf('{')) + date.ToString(dateformat) + fileName.Substring(fileName.IndexOf('}'));
                #endregion
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return fileName;
        }

        /// <summary>
        /// Download file from ftp using existing NirvanaWinSCPUtility class
        /// </summary>
        /// <param name="runUpload"></param>
        /// <param name="fileName"></param>
        /// <param name="ftpDownloadedFilePath"></param>
        /// <returns></returns>
        private static bool DownloadFile(RunUpload runUpload, string fileName, out string ftpDownloadedFilePath)
        {
            bool isFileDownloadedSuccessfully = false;
            ftpDownloadedFilePath = string.Empty;
            try
            {
                //string destinationPath = @"d:\FtpDownload\";
                //ftpFilePathToDownload is complete ftp path.

                NirvanaWinSCPUtility winScp = new NirvanaWinSCPUtility(runUpload.FtpDetails);
                //Here runUpload.FilePath is destination path to save decrypted file
                isFileDownloadedSuccessfully = winScp.ReceiveFile(fileName, out ftpDownloadedFilePath, runUpload.RawFilePath);
                //Need to handle multiple files in case where files are downloaded using wild cards (e.g * or ?)
                //if no file is downloaded from FTP, we are returning method
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
            return isFileDownloadedSuccessfully;
        }

        /// <summary>
        /// Decrypt file if decryption is required
        /// </summary>
        /// <param name="runUpload"></param>
        /// <param name="taskResult"></param>
        /// <param name="mailSubject"></param>
        /// <param name="mailBody"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool Decrypt(RunUpload runUpload, string mailSubject, StringBuilder mailBody, string fileName)
        {
            bool isFileDecryptedSuccessfully = false;
            try
            {
                if (runUpload.DecryptionDetails != null)
                {
                    string encryptedFilename = FileNameParser.GetFileNameUsingExtension(fileName);
                    string decryptedFilePath = Path.GetDirectoryName(runUpload.RawFilePath) + "\\" + encryptedFilename;
                    runUpload.RawFilePath = decryptedFilePath;
                    string encryptedFilePath = Path.GetDirectoryName(runUpload.RawFilePath) + "\\" + fileName;
                    isFileDecryptedSuccessfully = DecryptFile(runUpload, ref encryptedFilePath, decryptedFilePath);
                    if (!isFileDecryptedSuccessfully)
                    {
                        mailBody.AppendLine("File Decryption Error");
                        //Error Mail
                        EmailSender.SendEmail(runUpload.EmailDetails, null, mailSubject, mailBody.ToString());
                    }
                }
                else
                {
                    isFileDecryptedSuccessfully = true;
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
            return isFileDecryptedSuccessfully;
        }

        /// <summary>
        /// Decrypt file using existing class GnuPGWrapper
        /// </summary>
        /// <param name="runUpload"></param>
        /// <param name="fileName"></param>
        /// <param name="decryptedFilePath"></param>
        /// <returns></returns>
        private static bool DecryptFile(RunUpload runUpload, ref string fileName, string decryptedFilePath)
        {
            bool isFileDecryptedSuccessfully = false;
            try
            {
                GnuPGWrapper gpg = new GnuPGWrapper();
                // Set command
                gpg.command = (Commands)runUpload.DecryptionDetails.Command;
                // Set some parameters from on Web.Config file
                gpg.homedirectory = runUpload.DecryptionDetails.HomeDirectory;
                //setup pass phrase
                gpg.passphrase = runUpload.DecryptionDetails.PassPhrase;
                // Set other parameters from Web Controls
                gpg.originator = runUpload.DecryptionDetails.Originator;
                gpg.recipient = runUpload.DecryptionDetails.Recipient;
                gpg.batch = runUpload.DecryptionDetails.UseCmdBatch;
                gpg.yes = runUpload.DecryptionDetails.UseCmdYes;
                gpg.armor = runUpload.DecryptionDetails.UseCmdArmor;
                //TODO: Pass only directory name, file should be decrypted to this file path

                isFileDecryptedSuccessfully = gpg.DecryptFile(ref fileName, decryptedFilePath, runUpload.DecryptionDetails.ExtensionToAdd);
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
            return isFileDecryptedSuccessfully;
        }

        /// <summary>
        /// Check duplicity for file while import
        /// </summary>
        /// <param name="runUpload"></param>
        /// <param name="isPromptForDuplicityCheck">User will be prompted to overwrite the file</param>
        /// <param name="taskResult"></param>
        /// <param name="isCheckForFileDuplicityCheck">File Duplicity check will be skipped</param>
        /// <param name="mailSubject"></param>
        /// <param name="mailBody"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string CheckAndAppyDuplicityValidation(RunUpload runUpload, bool isPromptForDuplicityCheck, Dictionary<string, Tuple<string, string>> taskResult, bool isCheckForFileDuplicityCheck, string mailSubject, StringBuilder mailBody, string fileName, bool isRecon)
        {
            string errMsg = string.Empty;
            try
            {
                string errorMessage = string.Empty;
                bool isFileDuplicate = false;
                string amendedFilePath = string.Empty;
                if (isRecon)
                {
                    amendedFilePath = runUpload.ProcessedFilePath + Path.GetFileNameWithoutExtension(runUpload.RawFilePath) + "_" + DateTime.UtcNow.Date.ToString("yyyyMMdd") + Path.GetExtension(runUpload.RawFilePath);
                }
                else if (!isRecon)
                {
                    amendedFilePath = runUpload.ProcessedFilePath + Path.GetFileNameWithoutExtension(runUpload.RawFilePath) + Path.GetExtension(runUpload.RawFilePath);
                }
                #region IFELSE
                if (File.Exists(runUpload.RawFilePath) && isCheckForFileDuplicityCheck)
                {
                    //metaData of raw file
                    FileMetaData rawFileMetaDataInfo = GetMetaDataOfFile(runUpload.RawFilePath, fileName, out errorMessage);
                    #region IFELSE
                    if (string.IsNullOrEmpty(errorMessage) && rawFileMetaDataInfo != null)
                    {
                        //Read meta data of existing file and check that no of columns and no of rows are same as of new file
                        FileMetaData existingFileMetaData = null;
                        #region IFelse if (File.Exists(amendedFilePath + "_MetaData.xml"))
                        if (File.Exists(amendedFilePath + "_MetaData.xml"))
                        {
                            #region using
                            using (FileStream fs = File.OpenRead(amendedFilePath + "_MetaData.xml"))
                            {
                                XmlSerializer serializer = new XmlSerializer(typeof(FileMetaData));
                                existingFileMetaData = (FileMetaData)serializer.Deserialize(fs);
                                if (rawFileMetaDataInfo.NoOfRows == existingFileMetaData.NoOfRows && rawFileMetaDataInfo.NoOFColumns == existingFileMetaData.NoOFColumns && rawFileMetaDataInfo.Size == existingFileMetaData.Size)
                                {
                                    #region IFelse
                                    if (isRecon)
                                    {
                                        File.Delete(amendedFilePath);
                                        File.Move(runUpload.RawFilePath, amendedFilePath);
                                        runUpload.ProcessedFilePath = amendedFilePath;
                                    }
                                    else
                                    {
                                        #region IFelse
                                        if (isPromptForDuplicityCheck)
                                        {
                                            DialogResult dlgResult = new DialogResult();
                                            dlgResult = ConfirmationMessageBox.DisplayYesNo("File is duplicate, do you want to overwrite the existing file?", "Import");
                                            //MessageBox.Show(lblYesterdayDate.Text + " Day End Cash Not Present ! So Can't Create DayEnd Data for Future Dates !");
                                            if (dlgResult == DialogResult.No)
                                            {
                                                //Added By : Manvendra P.
                                                // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3357
                                                // Date : 15-Apr-1025
                                                errMsg = "User cancelled.";
                                                return errMsg;
                                            }
                                        }
                                        else
                                        {
                                            isFileDuplicate = true;
                                            mailBody.AppendLine("File " + Path.GetFileName(runUpload.FilePath) + " already exists");
                                            //Error Mail
                                            EmailSender.SendEmail(runUpload.EmailDetails, null, mailSubject, mailBody.ToString());
                                            AddValuesInTaskResult(taskResult, "RetrievalStatus", "Success", null);
                                            //taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", "Failure", null);
                                            string relativeProcessedFilePath = existingFileMetaData.FullName.Substring(Application.StartupPath.Length);
                                            //Here we are storing existing file name which is already in processed data folder
                                            //and we are showing file using processed file path from dashboard data.
                                            AddValuesInTaskResult(taskResult, "ProcessedFilePath", relativeProcessedFilePath, null);
                                            AddValuesInTaskResult(taskResult, "FileMetaData", existingFileMetaData.NoOfRows.ToString(), relativeProcessedFilePath + "_MetaData.xml");
                                            AddValuesInTaskResult(taskResult, "FileSize", existingFileMetaData.Size, relativeProcessedFilePath + "_MetaData.xml");
                                            runUpload.ProcessedFilePath = existingFileMetaData.FullName;
                                            errMsg = "File " + amendedFilePath + " already exists";
                                            AddValuesInTaskResult(taskResult, "Comments", "Duplicate file", null);
                                            return errMsg;
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                        }
                        #region commented
                        //else
                        //{
                        //    AddValuesInTaskResult(taskResult, "RetrievalStatus", "Success", null);
                        //    runUpload.ProcessedFilePath = amendedFilePath;
                        //    if (File.Exists(runUpload.RawFilePath))
                        //    {
                        //        File.Delete(runUpload.ProcessedFilePath);
                        //        File.Move(runUpload.RawFilePath, runUpload.ProcessedFilePath);
                        //    }
                        //    FileMetaData newFileMetaDataInfo = GetMetaDataOfFile(runUpload.ProcessedFilePath, fileName, out errorMessage);
                        //    #region IFELSE
                        //    if (!string.IsNullOrEmpty(errorMessage) || rawFileMetaDataInfo == null)
                        //    {
                        //        #region write metadata of file
                        //        XmlSerializer serializer = new XmlSerializer(typeof(FileMetaData));
                        //        using (TextWriter writer = new StreamWriter(runUpload.ProcessedFilePath + "_MetaData.xml"))
                        //        {
                        //            serializer.Serialize(writer, newFileMetaDataInfo);
                        //        }
                        //        string relativePath = runUpload.ProcessedFilePath.Remove(0, Application.StartupPath.Length);
                        //        AddValuesInTaskResult(taskResult, "FileMetaData", newFileMetaDataInfo.NoOfRows.ToString(), relativePath + "_MetaData.xml");
                        //        AddValuesInTaskResult(taskResult, "FileSize", newFileMetaDataInfo.Size, relativePath + "_MetaData.xml");
                        //        #endregion
                        //        errMsg = string.Empty;
                        //    }
                        //    else
                        //    {
                        //        mailBody.AppendLine(errorMessage);
                        //        //Error Mail
                        //        EmailSender.SendEmail(runUpload.EmailDetails, null, mailSubject, mailBody.ToString());
                        //        errMsg = "Error while getting meta data info of file: " + runUpload.ProcessedFilePath;
                        //    }
                        //    #endregion
                        //}
                        #endregion
                        #endregion
                    }
                    else
                    {
                        #region IFELSE
                        if (!errorMessage.Equals("No Data") && isRecon)
                        {
                            AddValuesInTaskResult(taskResult, "Comments", "No Data", null);
                            return errMsg;
                        }
                        else
                        {
                            mailBody.AppendLine(errorMessage);
                            //Error Mail
                            EmailSender.SendEmail(runUpload.EmailDetails, null, mailSubject, mailBody.ToString());
                            errMsg = "Error while getting meta data info of file: " + runUpload.RawFilePath;
                            return errMsg;
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    if (taskResult.ContainsKey("ProcessedFilePath"))
                    {
                        string processedFilePath = taskResult["ProcessedFilePath"].Item1;
                        if (processedFilePath != null && File.Exists(Application.StartupPath + processedFilePath.ToString()))
                        {
                            amendedFilePath = Application.StartupPath + processedFilePath.ToString();
                        }
                    }
                }
                #endregion

                #region IFelse
                if (isFileDuplicate)
                {
                    if (isCheckForFileDuplicityCheck)
                    {
                        if (File.Exists(runUpload.RawFilePath))
                        {
                            File.Delete(amendedFilePath);
                            File.Move(runUpload.RawFilePath, runUpload.ProcessedFilePath);
                        }
                    }
                }
                else
                {
                    AddValuesInTaskResult(taskResult, "RetrievalStatus", "Success", null);
                    runUpload.ProcessedFilePath = amendedFilePath;
                    if (File.Exists(runUpload.RawFilePath))
                    {
                        File.Delete(amendedFilePath);
                        File.Move(runUpload.RawFilePath, runUpload.ProcessedFilePath);
                    }
                    string relativeRawFilePath = runUpload.RawFilePath.Substring(Application.StartupPath.Length);
                    string relativeProcessedFilePath = runUpload.ProcessedFilePath.Substring(Application.StartupPath.Length);
                    AddValuesInTaskResult(taskResult, "RawFilePath", relativeRawFilePath, null);
                    AddValuesInTaskResult(taskResult, "ProcessedFilePath", relativeProcessedFilePath, null);

                    #region write ProcessedFilePath metadata of file
                    FileMetaData newFileMetaDataInfo = GetMetaDataOfFile(runUpload.ProcessedFilePath, fileName, out errorMessage);

                    if (string.IsNullOrEmpty(errorMessage) && newFileMetaDataInfo != null)
                    {
                        #region write metadata of file
                        XmlSerializer serializer = new XmlSerializer(typeof(FileMetaData));

                        using (TextWriter writer = new StreamWriter(runUpload.ProcessedFilePath + "_MetaData.xml"))
                        {
                            serializer.Serialize(writer, newFileMetaDataInfo);
                        }
                        string relativePath = runUpload.ProcessedFilePath.Remove(0, Application.StartupPath.Length);


                        AddValuesInTaskResult(taskResult, "FileMetaData", newFileMetaDataInfo.NoOfRows.ToString(), relativePath + "_MetaData.xml");
                        AddValuesInTaskResult(taskResult, "FileSize", newFileMetaDataInfo.Size, relativePath + "_MetaData.xml");
                        if (!isRecon)
                        {
                            AddValuesInTaskResult(taskResult, "Comments", "All data verified", null);
                        }
                        #endregion
                    }
                    else
                    {
                        mailBody.AppendLine(errorMessage);
                        //Error Mail
                        EmailSender.SendEmail(runUpload.EmailDetails, null, mailSubject, mailBody.ToString());
                        errMsg = "Error while getting meta data info of file: " + runUpload.ProcessedFilePath;
                    }
                    #endregion
                }
                #endregion
                //Added By : Manvendra P.
                // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3423
                //errMsg = string.Empty;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return errMsg;
        }
        private static void AddValuesInTaskResult(Dictionary<string, Tuple<string, string>> taskResult, string itemName, string value1, string value2)
        {
            if (taskResult.ContainsKey(itemName))
            {
                taskResult[itemName] = new Tuple<string, string>(value1, value2);
            }
            else
            {
                taskResult.Add(itemName, new Tuple<string, string>(value1, value2));
            }
        }

        /// <summary>
        /// extract info about file into FileMetaData
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="encryptedFileName"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private static FileMetaData GetMetaDataOfFile(string filePath, string encryptedFileName, out string errorMessage)
        {
            FileMetaData metaDataInfo = new FileMetaData();
            errorMessage = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                //returns null if file is corrupt
                if (File.Exists(filePath))
                {
                    dt = FileReaderFactory.GetDataTableFromDifferentFileFormats(filePath);
                }
                else
                {
                    errorMessage = "File: " + filePath + " does not exist";
                    metaDataInfo = null;
                    return metaDataInfo;
                }

                if (dt == null)
                {
                    errorMessage = "The Downloaded file is Corrupt";
                    metaDataInfo = null;
                    return metaDataInfo;
                }
                if (dt.Rows.Count == 0)
                {
                    errorMessage = "No Data";
                }
                FileInfo info = new FileInfo(filePath);
                metaDataInfo.OriginalFileName = Path.GetFileName(filePath);
                metaDataInfo.NoOFColumns = dt.Columns.Count;
                metaDataInfo.NoOfRows = dt.Rows.Count;
                metaDataInfo.EncryptedFileName = Path.GetFileName(encryptedFileName);
                metaDataInfo.Attributes = info.Attributes;
                metaDataInfo.CreationTimeUtc = info.CreationTimeUtc;
                metaDataInfo.DirectoryName = info.DirectoryName;
                metaDataInfo.Exists = info.Exists;
                metaDataInfo.Extension = info.Extension;
                metaDataInfo.FullName = info.FullName;
                metaDataInfo.IsReadOnly = info.IsReadOnly;
                metaDataInfo.LastAccessTimeUtc = info.LastAccessTimeUtc;
                metaDataInfo.Length = info.Length;
                metaDataInfo.Name = info.Name;
                metaDataInfo.Size = metaDataInfo.GetSize();
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
            return metaDataInfo;
        }

        public static string DownloadFileFromFTP(RunUpload runUpload, Dictionary<string, Tuple<string, string>> taskResult, ref bool isCheckForFileDuplicityCheck, string mailSubject, StringBuilder mailBody, ref string fileName, bool isRunFromExistingFile, bool isRecon)
        {
            string errMsg = string.Empty;
            try
            {
                string ftpDownloadedFilePath = string.Empty;

                if (isRecon)
                {
                    fileName = Path.GetDirectoryName(runUpload.FtpFilePath) + @"/" + Path.GetFileName(fileName);
                    if (!runUpload.RawFilePath.Contains("RawData"))
                    {
                        runUpload.RawFilePath = runUpload.FilePath + @"RawData\";
                    }
                    else
                    {
                        runUpload.RawFilePath = Path.GetDirectoryName(runUpload.RawFilePath) + "\\";
                    }
                    if (!runUpload.ProcessedFilePath.Contains("ProcessedData"))
                    {
                        runUpload.ProcessedFilePath = runUpload.FilePath + @"ProcessedData\";
                    }
                    else
                    {
                        runUpload.ProcessedFilePath = Path.GetDirectoryName(runUpload.ProcessedFilePath) + "\\"; ;
                    }
                }
                else
                {

                    runUpload.RawFilePath = Application.StartupPath + @"\Incoming\" + runUpload.DataSourceNameIDValue.ShortName + @"\DataFiles\" + runUpload.ImportTypeAcronym.ToString() + @"\RawData\";
                    runUpload.ProcessedFilePath = Application.StartupPath + @"\Incoming\" + runUpload.DataSourceNameIDValue.ShortName + @"\DataFiles\" + runUpload.ImportTypeAcronym.ToString() + @"\ProcessedData\";
                }
                if (!Directory.Exists(runUpload.RawFilePath))
                {
                    Directory.CreateDirectory(runUpload.RawFilePath);
                }
                if (!Directory.Exists(runUpload.ProcessedFilePath))
                {
                    Directory.CreateDirectory(runUpload.ProcessedFilePath);
                }

                if (runUpload.FtpDetails != null)
                {
                    bool isFileDownloadedSuccessfully = DownloadFile(runUpload, fileName, out ftpDownloadedFilePath);
                    if (isFileDownloadedSuccessfully && !string.IsNullOrEmpty(ftpDownloadedFilePath))
                    {
                        AddValuesInTaskResult(taskResult, "RetrievalStatus", "Success", null);
                        fileName = Path.GetFileName(ftpDownloadedFilePath);
                    }
                    else
                    {
                        string processedFilePath = string.Empty;
                        if (taskResult.ContainsKey("ProcessedFilePath"))
                        {
                            processedFilePath = taskResult["ProcessedFilePath"].Item1;
                        }
                        if (isRunFromExistingFile && processedFilePath != null && File.Exists(Application.StartupPath + processedFilePath))
                        {
                            DialogResult dlgResult = new DialogResult();
                            dlgResult = ConfirmationMessageBox.DisplayYesNo("File is missing on FTP, do you want to proceed with the existing file?", "Import");
                            //MessageBox.Show(lblYesterdayDate.Text + " Day End Cash Not Present ! So Can't Create DayEnd Data for Future Dates !");
                            if (dlgResult == DialogResult.No)
                            {
                                mailBody.AppendLine("File download Error").AppendLine("File Path: " + ftpDownloadedFilePath);
                                //Error Mail
                                EmailSender.SendEmail(runUpload.EmailDetails, null, mailSubject, mailBody.ToString());
                                AddValuesInTaskResult(taskResult, "RetrievalStatus", "Success", null);
                                errMsg = "Ftp File Download Error";
                                AddValuesInTaskResult(taskResult, "Comments", "MissingFile", null);
                                AddValuesInTaskResult(taskResult, "SymbolValidation", "", null);
                            }
                            else
                            {
                                errMsg = "Ftp File Download Error";
                            }
                        }
                        else
                        {
                            mailBody.AppendLine("File download Error").AppendLine("File Path: " + ftpDownloadedFilePath);
                            //Error Mail
                            EmailSender.SendEmail(runUpload.EmailDetails, null, mailSubject, mailBody.ToString());
                            AddValuesInTaskResult(taskResult, "RetrievalStatus", "Failure", null);
                            errMsg = "Ftp File Download Error";
                            if (!isRecon)
                            {
                                AddValuesInTaskResult(taskResult, "Comments", "MissingFile", null);
                            }
                        }
                    }
                }
                else
                {
                    isCheckForFileDuplicityCheck = false;
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return errMsg;
        }

    }
}
