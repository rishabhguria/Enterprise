using System.IO;

namespace Prana.BusinessObjects
{
    public class RiskPathCreator
    {
        public static string GetClientPath(ClientSettings objClientSettings)
        {
            return Path.Combine(objClientSettings.BaseSettings.FilePath, objClientSettings.ClientName);
        }
        public static string GetThirdPartyPath(ClientSettings objClientSettings, string thirdPartyName)
        {
            return Path.Combine(GetClientPath(objClientSettings), thirdPartyName);
        }
        public static string GetRiskReportPath(ClientSettings objClientSettings)
        {
            return Path.Combine(GetClientPath(objClientSettings), AutomationEnum.ReprotTypeEnum.RiskReport.ToString());
        }
        public static string GetDatePath(ClientSettings objClientSettings, string thirdPartyName)
        {
            string dateStringOfSpecificClient = objClientSettings.Date.ToString("ddMMyyyy");
            return Path.Combine(GetThirdPartyPath(objClientSettings, thirdPartyName), dateStringOfSpecificClient);
        }
        public static string GetRiskReportFileName(ClientSettings objClientSettings)
        {
            return GetRiskReportPath(objClientSettings) + @"\" + objClientSettings.ReportType + "_" + objClientSettings.Date.ToString("ddMMyyyy") + "." + objClientSettings.FileFormatter.ToString();
        }

    }
}
