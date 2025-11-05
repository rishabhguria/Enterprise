using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Specialized;

namespace Prana.Import.Helper
{
    internal static class FTPTradeEmailHelper
    {
        private static string subject = "Auto Import of trade data missing on " + DateTime.Today.ToLongDateString();
        private static string body = "Please find the trades attached below which are not properly imported in the system.";

        public static async void SendErrorEmail(string filename, string mailBody = "")
        {
            NameValueCollection collection = ConfigurationHelper.Instance.GetSectionBySectionName(ConfigurationHelper.SECTION_DataHubTradeImport);
            String SenderAddress = collection[ConfigurationHelper.CONFIG_DATAHUBTRADEIMPORT_SenderAddress];
            string SenderName = collection[ConfigurationHelper.CONFIG_DATAHUBTRADEIMPORT_SenderName];
            String Password = collection[ConfigurationHelper.CONFIG_DATAHUBTRADEIMPORT_Password];
            String RecieverAddress = collection[ConfigurationHelper.CONFIG_DATAHUBTRADEIMPORT_RecieverAddress];
            String HostName = collection[ConfigurationHelper.CONFIG_DATAHUBTRADEIMPORT_HostName];
            int Port = Convert.ToInt32(collection[ConfigurationHelper.CONIFG_DATAHUBTRADEIMPORT_PORT]);
            bool EnableSSL = Convert.ToBoolean(collection[ConfigurationHelper.CONFIG_DATAHUBTRADEIMPORT_EnableSSL]);
            if (mailBody == string.Empty) mailBody = body;

            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                        EmailsHelper.SendMail("", filename, RecieverAddress, SenderAddress,
                        subject, mailBody, null, null, Password, HostName, Port, EnableSSL, SenderName)
                    );
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }

        }

    }
}
