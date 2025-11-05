using Infragistics.Win.UltraWinTabControl;
using Prana.LogManager;
using System;
using System.IO;

namespace Prana.Utilities.UI
{
    public static class UltraTabControlHelper
    {
        public static void SaveTabOrder(string directory, string filepath, UltraTabControl ultraTabControl)
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write);
                ultraTabControl.SaveTabOrderAsXml(fs);
                fs.Close();
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

        public static void LoadTabOrder(string filePath, UltraTabControl ultraTabControl)
        {
            try
            {
                if (!File.Exists(filePath))
                    return;

                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                ultraTabControl.LoadTabOrderFromXml(fs);
                fs.Close();
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
    }
}