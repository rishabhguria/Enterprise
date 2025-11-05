using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Nirvana.TestAutomation.Utilities
{
   
    public class ConfigUpdater
    {
        /// <summary>
        /// Updating Values
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="attributeForChange"></param>
        /// <param name="configWriterSettings"></param>
        public static void ChangeValueByKey(string key, string value, ConfigModificatorSettings configWriterSettings)
        {
            try
            {
                XmlDocument doc = LoadConfigDocument(configWriterSettings.ConfigPath);
                XmlNode rootNode = doc.SelectSingleNode(configWriterSettings.RootNode);

                if (rootNode == null)
                {
                    throw new InvalidOperationException("the root node section not found in config file.");
                }
                XmlElement elem = (XmlElement)rootNode.SelectSingleNode(string.Format(configWriterSettings.NodeForEdit, key));
                elem.SetAttribute("value", value);
                doc.Save(configWriterSettings.ConfigPath);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Laoding Document
        /// </summary>
        /// <param name="configFilePath"></param>
        /// <returns></returns>
        private static XmlDocument LoadConfigDocument(string configFilePath)
        {
            XmlDocument doc = null;
            try
            {
                doc = new XmlDocument();
                doc.Load(configFilePath);
                return doc;
            }
            catch (Exception ex)
            {
                //throw new Exception("No configuration file found.", ex);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
                return null;
            }

        }
    }
    }

