using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Prana.InstallerUtilities
{
    public class LoggingHelper
    {
        #region SingletonInstance
        /// <summary>
        /// Locker object
        /// </summary>
        private static Object _lock = new Object();

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static LoggingHelper _loggingHelper = null;

        public bool IsConsoleMode;

        string _saveLogsMsgFile = String.Empty;

        /// <summary>
        /// private cunstructor, Check file exist or not, If not exist then create the file
        /// </summary>
        private LoggingHelper()
        {
            try
            {
                _saveLogsMsgFile = Application.StartupPath + "\\InstallationLog.txt";
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        /// <returns></returns>
        public static LoggingHelper GetInstance()
        {
            lock (_lock)
            {
                if (_loggingHelper == null)
                    _loggingHelper = new LoggingHelper();
                return _loggingHelper;
            }
        }
        #endregion

        private static object _loggingFileLocker = new object();

        public void LoggerWrite(string errorMessage, string errorStackTrace)
        {
            LoggerWrite(errorMessage, true, MessageBoxIcon.Error);
            LoggerWrite(errorStackTrace);
        }

        public void LoggerWrite(string line, bool isShowMessageBox = false, MessageBoxIcon messageBoxIcon = MessageBoxIcon.Error)
        {
            try
            {
                lock (_loggingFileLocker)
                {
                    StringBuilder sb = new StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt"));
                    sb.Append("  " + line);
                    sb.Append(Environment.NewLine);
                    File.AppendAllText(_saveLogsMsgFile, sb.ToString());
                }

                if (!IsConsoleMode && isShowMessageBox)
                {
                    MessageBox.Show(line, "Prana Installer", MessageBoxButtons.OK, messageBoxIcon);
                }

                if (IsConsoleMode)
                {
                    Console.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }
        }
    }
}