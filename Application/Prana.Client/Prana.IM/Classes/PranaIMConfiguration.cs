using System;
using System.Collections.Generic;
using System.Text;
using Prana.Global;

namespace Prana.IM
{
    internal static class PranaIMConfiguration
    {
        //public static string PranaIMServerIPAddress = System.Configuration.ConfigurationManager.AppSettings["PranaIMServerIPAddress"];
        public static string PranaIMServerIPAddress = ConfigurationHelper.Instance.GetAppSettingValueByKey("PranaIMServerIPAddress");
        //public static string PranaIMServerPort = System.Configuration.ConfigurationManager.AppSettings["PranaIMServerPort"];
        public static string PranaIMServerPort = ConfigurationHelper.Instance.GetAppSettingValueByKey("PranaIMServerPort");
        //public static int PranaIMUnitID = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PranaIMUnitID"]);
        public static int PranaIMUnitID = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("PranaIMUnitID"));
        //public static string PranaIMWindowTitle = System.Configuration.ConfigurationManager.AppSettings["PranaIMWindowTitle"];
        public static string PranaIMWindowTitle = ConfigurationHelper.Instance.GetAppSettingValueByKey("PranaIMWindowTitle");
        public const string SoapClientWindowTextFixedPart = "@nim.net - SoapBox";
    }
}
