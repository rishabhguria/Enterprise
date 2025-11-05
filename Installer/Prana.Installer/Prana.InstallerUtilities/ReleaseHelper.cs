using System;
using System.Configuration;
using System.Xml;

namespace Prana.InstallerUtilities
{
    public class ReleaseHelper
    {
        /// <summary>
        /// Get the connection strings from the release
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static String[] GetConnectionStringForRelease(String path)
        {
            try
            {
                String[] conStrings = new String[2];
                String configPath = path + '\\' + ConfigurationManager.AppSettings["TradeServiceConfiguration"];
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configPath);
                XmlNodeList GeneralInformationNode = xmlDoc.SelectNodes("/configuration/connectionStrings/add");
                foreach (XmlNode node in GeneralInformationNode)
                {
                    if (node.Attributes.GetNamedItem("name").Value == "PranaConnectionString")
                        conStrings[0] = node.Attributes.GetNamedItem("connectionString").Value;
                    if (node.Attributes.GetNamedItem("name").Value == "SMConnectionString")
                        conStrings[1] = node.Attributes.GetNamedItem("connectionString").Value;
                }

                return conStrings;
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
                return null;
            }
        }
    }
}