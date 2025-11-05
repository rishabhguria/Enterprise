using Prana.LogManager;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

namespace Prana.Global
{
    public static class ClientAppConfiguration
    {
        public static ServerDetails TradeServer;
        public static ServerDetails ExpnlServer;
        public static ServerDetails PricingServer;
        static Dictionary<string, ServerDetails> _dictNames = new Dictionary<string, ServerDetails>();

        static ClientAppConfiguration()
        {
            if (ConfigurationManager.AppSettings["TradeServer"] != null)
            {
                TradeServer.CreateDetails(ConfigurationManager.AppSettings["TradeServer"].ToString());
                _dictNames.Add("TradeServer", TradeServer);
            }
            if (ConfigurationManager.AppSettings["ExpnlServer"] != null)
            {
                ExpnlServer.CreateDetails(ConfigurationManager.AppSettings["ExpnlServer"].ToString());
                _dictNames.Add("ExpnlServer", ExpnlServer);
            }
            if (ConfigurationManager.AppSettings["PricingServer"] != null)
            {
                PricingServer.CreateDetails(ConfigurationManager.AppSettings["PricingServer"].ToString());
                _dictNames.Add("PricingServer", PricingServer);
            }
        }

        public static ServerDetails getDetails(string key)
        {
            if (!string.IsNullOrEmpty(key) && _dictNames.ContainsKey(key))
            {
                return _dictNames[key];
            }
            else
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Incorrect server name found in configuration. '" + key + "'", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                return new ServerDetails();
            }
        }
    }
}