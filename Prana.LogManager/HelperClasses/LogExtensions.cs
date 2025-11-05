using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.LogManager
{
    public static class LogExtensions
    {
        /// <summary>
        /// Get Log Message
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="message"></param>
        /// <param name="trace"></param>
        /// <returns></returns>
        private static string GetLogMessage(string serviceName, string message, StackTrace trace, MethodBase method = null, params object[] arguments)
        {
            var msg = string.Empty;
            bool logEnabled = true;

            if (logEnabled)
            {
                if (method != null && arguments.Count() > 0)
                    msg = message + GetMessage(serviceName, method, arguments);
                else
                    msg = string.Format(serviceName + message);

                bool traceEnabled = true;
                if (traceEnabled)
                    msg = string.Format(msg + "\\n" + trace);
            }
            return msg;

        }

        /// <summary>
        /// Write Message on file
        /// </summary>
        /// <param name="log"></param>
        /// <param name="serviceName"></param>
        /// <param name="method"></param>
        private static string GetMessage(string serviceName, MethodBase method, params object[] arguments)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Service: ");
            sb.Append(serviceName);
            sb.Append(", Method Name: ");
            sb.AppendLine(method.Name);
            sb.Append(", ");

            int i = 0;

            if (arguments.Count() > 0)
            {
                foreach (var param in method.GetParameters())
                {
                    sb.Append(param.Name + ": ");
                    var paramValue = arguments[i];
                    if (paramValue is string || paramValue is DateTime || paramValue is bool || paramValue is Enum || paramValue is int)
                    {
                        sb.Append(arguments[i]);
                    }
                    else if (paramValue is IDictionary)
                    {
                        string fileName = SerializeDict(paramValue);
                        sb.Append("Debug file name: ");
                        sb.Append(fileName);
                    }
                    else if (paramValue.GetType().IsSerializable)
                    {
                        string fileName = ObjectToByteArray(paramValue);
                        sb.Append("Debug file name: ");
                        sb.Append(fileName);
                    }
                    else if (paramValue.GetType().IsClass)
                    {
                        string fileName = ObjectToByteArray(paramValue);
                        sb.Append("Debug file name: ");
                        sb.Append(fileName);
                    }

                    sb.Append(", ");
                    i++;
                }
            }


            return sb.ToString();
        }

        public static string SerializeDict(object dict)
        {
            Guid guid = Guid.NewGuid();
            var folderPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\DebugFiles\\";
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            try
            {
                DataContractSerializer serializer = new DataContractSerializer(dict.GetType());

                using (var writer = new StringWriter())
                {
                    var stm = new XmlTextWriter(writer);
                    serializer.WriteObject(stm, dict);
                    var xml = writer.ToString();
                    File.WriteAllText(folderPath + guid + ".xml", xml);
                }
                return guid.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static string ObjectToByteArray(object obj)
        {
            if (obj == null)
                return string.Empty;

            Guid guid = Guid.NewGuid();
            var folderPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\DebugFiles\\";
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            try
            {
                using (var stringwriter = new System.IO.StreamWriter(folderPath + guid + ".xml"))
                {
                    var serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(stringwriter, obj);

                }

                return guid.ToString();
            }
            catch (Exception)
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    using (MemoryStream ms = new MemoryStream())
                    {

                        bf.Serialize(ms, obj);

                        File.WriteAllBytes(folderPath + guid + ".txt", ms.ToArray());
                        return guid.ToString();

                    }
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        public static string ToLoggerString(object logObj)
        {
            StringBuilder sb = new StringBuilder();

            PropertyInfo[] properties = (PropertyInfo[])logObj.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                sb.Append(property.Name + " : " + property.GetValue(logObj, null));
                sb.Append("\t");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Loggers the write send to server.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="message">The message.</param>
        public static void LoggerWriteMessage(string module, string message)
        {
            try
            {
                StringBuilder sb = new StringBuilder(DateTime.UtcNow.ToString());
                sb.Append(Environment.NewLine);
                sb.Append(message);
                Logger.LoggerWrite(sb, module, 1, 1, TraceEventType.Information);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="action">The action.</param>
        /// <param name="tabName">Name of the tab.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        public static void LogMessage(string module, string userName, string action, string tabName, string fromDate, string toDate, String accountIDs)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(action);
                sb.Append(LoggingConstants.COLON);
                sb.AppendLine(tabName);
                if (!string.IsNullOrWhiteSpace(fromDate))
                {
                    sb.Append("From date: ");
                    sb.AppendLine(fromDate.ToString());
                }
                if (!string.IsNullOrWhiteSpace(toDate))
                {
                    sb.Append("To date: ");
                    sb.AppendLine(toDate.ToString());
                }
                if (!string.IsNullOrWhiteSpace(accountIDs))
                {
                    sb.Append("AccountIds: ");
                    sb.AppendLine(accountIDs);
                }
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    sb.Append(LoggingConstants.USER);
                    sb.Append(userName);
                }
                LoggerWriteMessage(module, sb.ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// This method logs the configuration sections at startup
        /// </summary>
        public static string GetStartUpConfigurations()
        {
            StringBuilder sectionTexts = new StringBuilder();
            try
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ConfigSectionNamesForLogAtApplicationStartUp"))
                {
                    string[] configSections = ConfigurationManager.AppSettings["ConfigSectionNamesForLogAtApplicationStartUp"].ToString().Split(',');
                    if (configSections.Length > 0 && !string.IsNullOrWhiteSpace(configSections[0]))
                    {
                        sectionTexts.AppendLine();
                        var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                        foreach (var section in configSections)
                        {
                            string trimmedSection = section.Trim();
                            ConfigurationSection sectionElement = config.GetSection(trimmedSection);

                            if (sectionElement != null)
                            {
                                string rawXml = sectionElement.SectionInformation.GetRawXml();
                                sectionTexts.AppendLine(rawXml);
                            }
                            else if (config.SectionGroups.Keys.Cast<string>().Contains(trimmedSection))
                            {
                                ConfigurationSectionGroup sectionGroupElement = config.GetSectionGroup(trimmedSection);
                                sectionTexts.AppendLine($"<{sectionGroupElement.Name}>");
                                foreach (ConfigurationSection currSection in sectionGroupElement.Sections)
                                {
                                    string rawXml = "\t" + currSection.SectionInformation.GetRawXml();
                                    if (!string.IsNullOrWhiteSpace(rawXml))
                                        sectionTexts.AppendLine(rawXml);
                                }
                                sectionTexts.AppendLine($"</{sectionGroupElement.Name}>");
                            }
                            else
                            {
                                sectionTexts.AppendLine($"{trimmedSection} not found.");
                            }
                            sectionTexts.AppendLine();
                        }
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
            return sectionTexts.ToString();
        }
    }
}
