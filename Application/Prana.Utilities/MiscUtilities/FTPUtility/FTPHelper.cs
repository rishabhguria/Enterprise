using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.Utilities.ImportExportUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Prana.Utilities.MiscUtilities.FTPUtility
{
    public static class FTPHelper
    {
        public static List<string> GetAllFilesOnFTPForTimeInterval(ThirdPartyFtp thirdPartyFtp, string ftpFilePath, int timeInterval, ref List<string> lstAlreadyProcessedFiles, int mode, bool isFTPLoggingEnabled)
        {
            List<string> listFiles = new List<string>();
            try
            {
                if (mode == 0)
                {
                    listFiles = GetFilesListOnFTP(thirdPartyFtp, ftpFilePath, true, timeInterval, ref lstAlreadyProcessedFiles, isFTPLoggingEnabled);
                }
                else
                {
                    listFiles = GetFilesListOnFTP(thirdPartyFtp, ftpFilePath, false, timeInterval, ref lstAlreadyProcessedFiles, isFTPLoggingEnabled);
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
            return listFiles;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thirdPartyFtp"></param>
        /// <param name="dictListFiles"></param>
        public static bool DownloadFile(ThirdPartyFtp thirdPartyFtp, string fileName, out string downloadedfilePath, bool isFTPLoggingEnabled, string downloadFolder = "PricingImport")
        {
            bool isDownloaded = false;
            downloadedfilePath = string.Empty;
            try
            {
                string directory = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), downloadFolder + "\\");

                NirvanaWinSCPUtility winScp = new NirvanaWinSCPUtility(thirdPartyFtp, isFTPLoggingEnabled);
                //Here runUpload.FilePath is destination path to save decrypted file

                isDownloaded = winScp.ReceiveFile(fileName, out downloadedfilePath, directory);
                downloadedfilePath = Path.Combine(directory, Path.GetFileName(downloadedfilePath));
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
            return isDownloaded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable UploadDataThruPricingFile(string filePath)
        {
            DataTable dtData = null;
            try
            {
                if (File.Exists(filePath))
                {
                    dtData = FileReaderFactory.GetDataTableFromDifferentFileFormats(filePath);
                    if (dtData != null && dtData.Rows.Count > 0)
                    {
                        //First row contains headers only
                        ReArrangeDataTable(dtData, 0);
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
            return dtData;
        }

        /// <summary>
        /// First row of data table will be assigned to the column names
        /// Remove first row as it contains only headers
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rowIndexWithColumnName"></param>
        private static void ReArrangeDataTable(DataTable dt, int rowIndexWithColumnName)
        {
            try
            {
                if (dt.Rows.Count > rowIndexWithColumnName)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dt.Rows[rowIndexWithColumnName][dc.ColumnName].ToString() != string.Empty)
                        {
                            dc.ColumnName = dt.Rows[rowIndexWithColumnName][dc.ColumnName].ToString().Trim();
                        }
                    }
                    //Remove first row which contains header
                    dt.Rows.RemoveAt(rowIndexWithColumnName);
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

        public static DataTable GetDataTableFromFTP(ThirdPartyFtp thirdPartyFtp, string filePath, bool isFTPLoggingEnabled)
        {
            DataTable dt = new DataTable();
            try
            {
                string downloadedfilePath = string.Empty;
                if (DownloadFile(thirdPartyFtp, filePath, out downloadedfilePath, isFTPLoggingEnabled))
                {
                    dt = UploadDataThruPricingFile(downloadedfilePath);
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
            return dt;
        }
        public static List<string> GetFilesListOnFTP(ThirdPartyFtp thirdPartyFtp, string ftpFilePath, bool isCheckByFileModifiedDate, int timeInterval, ref List<string> lstAlreadyProcessedFiles, bool isFTPLoggingEnabled)
        {
            Dictionary<string, DateTime> dictListFiles = new Dictionary<string, DateTime>();
            try
            {
                string fileNameSyntax = Path.GetFileName(ftpFilePath);
                DateTime MaxDate = DateTime.MinValue;
                string fileDateFormat = FileNameParser.GetDateFormatFromFileName(fileNameSyntax);
                //check that from list how much files are available in ftp
                NirvanaWinSCPUtility scpUtil = new NirvanaWinSCPUtility(thirdPartyFtp, isFTPLoggingEnabled);
                Dictionary<string, DateTime> dictFiles = scpUtil.ListFiles(Path.GetDirectoryName(ftpFilePath));
                foreach (KeyValuePair<string, DateTime> kvp in dictFiles)
                {
                    if (kvp.Key.Length.Equals(fileNameSyntax.Length - ((fileNameSyntax).Count(x => x == '{') * 2)))
                    {
                        string strDate = FileNameParser.GetDateStringFromFileName(fileNameSyntax, kvp.Key);
                        DateTime fileDate = DateTime.Now;
                        if (DateTime.TryParseExact(strDate, fileDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out fileDate))
                        {
                            if (FileNameParser.GetFileNameFromNamingConvention(fileNameSyntax, fileDate).ToUpper().Equals(kvp.Key.ToUpper()))
                            {
                                if (timeInterval < 0)
                                {
                                    if (fileDate.Date == DateTime.Today)
                                    {
                                        MaxDate = fileDate;
                                        dictListFiles.Add(Path.GetDirectoryName(ftpFilePath).Replace('\\', '/') + "/" + kvp.Key, fileDate);
                                    }
                                }
                                else if (isCheckByFileModifiedDate)
                                {
                                    if (MaxDate.Ticks < kvp.Value.Ticks)
                                    {
                                        MaxDate = kvp.Value;
                                    }
                                    dictListFiles.Add(Path.GetDirectoryName(ftpFilePath) + "/" + kvp.Key, kvp.Value);

                                }
                                else
                                {
                                    if (MaxDate.Ticks < fileDate.Ticks)
                                    {
                                        MaxDate = fileDate;
                                    }
                                    dictListFiles.Add(Path.GetDirectoryName(ftpFilePath).Replace('\\', '/') + "/" + kvp.Key, fileDate);
                                }
                            }
                        }
                    }
                }
                if (timeInterval > 0)
                {
                    //https://msdn.microsoft.com/en-us/library/system.datetime.ticks%28v=vs.110%29.aspx
                    //Time interval is passed in mili seconds, so to calculate timeperiod we multiply by 10^4, instead of 10^7
                    int timeperiod = timeInterval * 10000;
                    foreach (var item in dictListFiles.Where(kvp => (MaxDate.Ticks - kvp.Value.Ticks) > timeperiod).ToList())
                    {
                        dictListFiles.Remove(item.Key);
                    }
                }

                List<string> filesToBeProcessed = new List<string>();
                filesToBeProcessed = dictListFiles.Keys.ToList();
                foreach (string file in filesToBeProcessed)
                {
                    if (lstAlreadyProcessedFiles.Contains(file))
                    {
                        dictListFiles.Remove(file);
                    }
                    else
                        lstAlreadyProcessedFiles.Add(file);
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
            return dictListFiles.Keys.ToList();
        }
    }
}
