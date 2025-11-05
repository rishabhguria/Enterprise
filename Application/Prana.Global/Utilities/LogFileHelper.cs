using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.Global.Utilities
{
    public class LogFileHelper
    {
        #region SingiltonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static LogFileHelper _logFileHelper = null;
        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        public static LogFileHelper GetInstance()
        {
            lock (_lock)
            {
                if (_logFileHelper == null)
                    _logFileHelper = new LogFileHelper();
                return _logFileHelper;
            }
        }
        #endregion
		
		/// <summary>
        /// Adds logs file to zip.
        /// </summary>
        public void AddLogFileToZip()
        {
            try
            {
                string startupPath = Directory.GetCurrentDirectory();
                startupPath += @"\Logs\Compliance_Logs";
                List<string> logFiles = Directory.GetFiles(startupPath, "*.log").ToList();

                foreach (string file in logFiles)
                {
                    FileInfo logFileInfo = new FileInfo(file);
                    int res = DateTime.Compare(logFileInfo.LastWriteTime, DateTime.Now.Date);
                    if (res > 0)
                        continue;
                    List<string> lstFileNameSection = Path.GetFileName(file).Split('.').ToList();
                    if (lstFileNameSection!=null && lstFileNameSection.Count > 1 && lstFileNameSection[1].Equals("log"))
                        continue;
                    string fileNameUpdated = GetCompressedFileName(file);
                    using (ZipArchive zip = ZipFile.Open(fileNameUpdated + ".GZ", ZipArchiveMode.Create))
                    {
                        using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read,
                            FileShare.Delete | FileShare.ReadWrite))
                        {
                            ZipArchiveEntry zipArchiveEntry = zip.CreateEntry(Path.GetFileName(file),
                                CompressionLevel.Optimal);
                            using (Stream destination = zipArchiveEntry.Open())
                                stream.CopyTo(destination);
                        }
                    }
                    if (File.Exists(file))
                    {
                        File.Delete(file);
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
        /// Returns the compressed file name.
        /// </summary>
        private string GetCompressedFileName(string fileName)
        {
            string compressedFileName = string.Empty;
            try
            {
                compressedFileName = fileName;
                bool fileExist = true;
                int sheetNum = 1;
                while(fileExist)
                {
                    if (!File.Exists(compressedFileName + ".GZ"))
                    {
                        fileExist = false;
                    }
                    else
                    {
                        compressedFileName = fileName + "_" + sheetNum.ToString();
                        sheetNum++;
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
            return compressedFileName;
        }
    }
}
