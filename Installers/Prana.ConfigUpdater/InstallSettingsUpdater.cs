using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace Prana.ConfigUpdater
{
    class InstallSettingsUpdater
    {
        private ConfigUpdaterSettings m_ConfigUpdaterSettings = ConfigUpdaterSettings.GetInstance();
        private ConfigXmlDocument m_SourceConfigxmlDoc = null;
        private ConfigXmlDocument m_DestinationConfigxmlDoc = null;
        private string m_StringMainPath = "//configuration";            


        public void UpdateInstallSettings()
        {
            string DBConnectionStringXPath = "//configuration/connectionStrings";
            //string AppsettingsXPath = "//configuration/appSettings";
            //string strSMConnectionStringName = "SMConnectionString";
            //string strSMDBFileName = "NirvanaSM.mdf";


            //Add or update DB Connection string in destination file
            try
            {
                XmlNode DBSourceNode = m_SourceConfigxmlDoc.SelectSingleNode(DBConnectionStringXPath);
                XmlNode DBDestinationNode = m_DestinationConfigxmlDoc.SelectSingleNode(DBConnectionStringXPath);
                if (DBSourceNode != null)
                {
                    //Generate SMDB Connection String
                    //XmlNodeList DBSourceNodes = m_SourceConfigxmlDoc.SelectNodes(DBConnectionStringXPath);
                    //foreach (XmlNode DBSrcNode in DBSourceNodes.Item(0).ChildNodes)
                    //{
                    //    if (DBSrcNode.NodeType != XmlNodeType.Element) continue;
                    //    if (DBSrcNode.Name.ToLower()=="add" && DBSrcNode.Attributes["name"].Value.ToLower() == strSMConnectionStringName.ToLower())
                    //    {
                    //        Guid aGuid = Guid.NewGuid();
                    //        string strNewDBName = aGuid.ToString() + "SM.mdf" ;
                    //        DBSrcNode.Attributes["connectionString"].Value = "Server=.\\SQLEXPRESS;Integrated Security=True; User Instance=True;AttachDBFilename=|DataDirectory|SecurityMasterDB\\" + strNewDBName + ";Initial Catalog=" + strNewDBName;

                    //        //Now rename sm db in file system
                    //        File.Copy(Application.StartupPath + "\\SecurityMasterDB\\" + strSMDBFileName, Application.StartupPath + "\\SecurityMasterDB\\" + strNewDBName);
                            
                    //        break;
                    //    }
                    //}

                    if (DBDestinationNode != null)
                    {
                        DBDestinationNode.InnerXml = DBSourceNode.InnerXml;
                    }
                    else
                    {
                        XmlElement newXmlElement = m_DestinationConfigxmlDoc.CreateElement(DBSourceNode.Name);
                        newXmlElement.InnerXml = DBSourceNode.InnerXml;
                        m_DestinationConfigxmlDoc.ChildNodes[0].AppendChild(newXmlElement);
                    }
                }

                AddOrUpdateConfigSectionsInAppSettingsFile();

                #region Commented

                // Add or update appsettings
                //XmlNodeList AppsettingsSourceNodes = m_SourceConfigxmlDoc.SelectNodes(AppsettingsXPath);
                //XmlNodeList AppsettingsDestinationNodes = m_DestinationConfigxmlDoc.SelectNodes(AppsettingsXPath);
                //XmlNode AppSettingNode = m_DestinationConfigxmlDoc.SelectSingleNode(AppsettingsXPath);

                //bool AppSettingiInDestination = false;
                //XmlNode AppsettingsDestinationNode = null;

                //foreach (XmlNode SrcNode in AppsettingsSourceNodes.Item(0).ChildNodes)
                //{
                //    if (SrcNode.NodeType != XmlNodeType.Element) continue;
                //    foreach (XmlNode DestNode in AppsettingsDestinationNodes.Item(0).ChildNodes)
                //    {
                //        if (DestNode.NodeType != XmlNodeType.Element) continue;
                //        if (DestNode.Attributes["key"].Value == SrcNode.Attributes["key"].Value)
                //        {
                //            AppSettingiInDestination = true;
                //            AppsettingsDestinationNode = DestNode;
                //            break;
                //        }
                //    }
                //    if (AppSettingiInDestination)
                //    {
                //        AppsettingsDestinationNode.Attributes["value"].Value = SrcNode.Attributes["value"].Value;
                //        AppSettingiInDestination = false;
                //    }
                //    else
                //    {
                //        XmlElement newXmlElement = m_DestinationConfigxmlDoc.CreateElement(SrcNode.Name);
                //        XmlAttribute key = m_DestinationConfigxmlDoc.CreateAttribute("key");
                //        key.Value = SrcNode.Attributes["key"].Value;
                //        newXmlElement.Attributes.Append(key);
                //        XmlAttribute value = m_DestinationConfigxmlDoc.CreateAttribute("value");
                //        value.Value = SrcNode.Attributes["value"].Value;
                //        newXmlElement.Attributes.Append(value);
                //        AppSettingNode.AppendChild(newXmlElement);
                //    }
                //}

                #endregion Commented

                m_DestinationConfigxmlDoc.Save(m_ConfigUpdaterSettings.DestinationFile);
            }
            catch (Exception ex)
            {
                throw new Exception("Install settings file is corrupt.",ex);
            }
        }

        /// <summary>
        /// ADDS THE NEWLY ADDED CONFIG SECTIONS AND UPDATES THE EXISTING CONFIG SECTIONS
        /// </summary>
        private void AddOrUpdateConfigSectionsInAppSettingsFile()
        {
            try
            {
                AddNewEntriesToConfigurationSection();

            }
            catch (Exception ex)
            {
                throw new Exception("Install settings file is corrupt.", ex);
            }

            UpdateAllNodes();

        }

        /// <summary>
        /// COPIES THE ENTIRE NEWLY ADDED NODES IN THE CONFIGURATION SECTIONS AS THEY ARE PRESENT IN THE 
        /// INSTALLSETTINGS CONFIG FILE
        /// </summary>
        private void AddNewEntriesToConfigurationSection()
        {
            XmlNodeList AppsettingsSourceNodes = m_SourceConfigxmlDoc.SelectNodes(m_StringMainPath);
            XmlNodeList AppsettingsDestinationNodes = m_DestinationConfigxmlDoc.SelectNodes(m_StringMainPath);
            XmlNode AppSettingNode = m_DestinationConfigxmlDoc.SelectSingleNode(m_StringMainPath);

            bool AppSettingsInDestination = false;

            foreach (XmlNode SrcNode in AppsettingsSourceNodes.Item(0).ChildNodes)
            {
                if (SrcNode.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                foreach (XmlNode DestNode in AppsettingsDestinationNodes.Item(0).ChildNodes)
                {
                    if (DestNode.Name == SrcNode.Name)
                    {
                        AppSettingsInDestination = true;
                        break;
                    }
                }
                if (AppSettingsInDestination)
                {
                    AppSettingsInDestination = false;
                }
                else
                {
                    XmlElement newXmlelement = m_DestinationConfigxmlDoc.CreateElement(SrcNode.Name);

                    if (SrcNode.ChildNodes.Count > 0)
                    {
                        foreach (XmlNode childnode in SrcNode)
                        {
                            if (childnode.Name.Contains("#comment"))
                                continue;

                            XmlElement newchildXmlelement = m_DestinationConfigxmlDoc.CreateElement(childnode.Name);

                            foreach (XmlAttribute varxmlattrib in childnode.Attributes)
                            {
                                XmlAttribute childAttribute = m_DestinationConfigxmlDoc.CreateAttribute(varxmlattrib.Name);
                                childAttribute.Value = varxmlattrib.Value;
                                newchildXmlelement.SetAttributeNode(childAttribute);
                            }

                            newXmlelement.AppendChild(newchildXmlelement);
                        }
                    }
                    AppSettingNode.AppendChild(newXmlelement);
                }

            }
        }

        /// <summary>
        /// UPDATES ALL THE KEY VALUE PAIRS OF THE NODES ALREADY PRESENT IN THE APP.COFIG FILE
        /// </summary>
        private void UpdateAllNodes()
        {
            XmlNodeList AppsettingsSourceNodesInner = m_SourceConfigxmlDoc.SelectNodes(m_StringMainPath);

            foreach (XmlNode SrcNode in AppsettingsSourceNodesInner.Item(0).ChildNodes)
            {
                switch (SrcNode.Name)
                {
                    case "configSections":
                        break;
                    case "loggingConfiguration":
                        break;
                    case "exceptionHandling":
                        break;
                    case "dataConfiguration":
                        break;
                    case "connectionStrings":
                        break;
                    default:
                        string strmainpath = m_StringMainPath + "/" + SrcNode.Name;
                        UpdateSelectedNode(strmainpath);
                        break;
                }
            }

           // m_DestinationConfigxmlDoc.Save(Application.StartupPath + "\\Prana.exe.config");

        }

        /// <summary>
        /// UPDATES THE KEY VALUE PAIR OF THE SELECTED NODE
        /// </summary>
        /// <param name="strmainpath"></param>
        private void UpdateSelectedNode(string strmainpath)
        {
            try
            {
                XmlNode AppsettingsSourceNode = m_SourceConfigxmlDoc.SelectSingleNode(strmainpath);
                XmlNode AppsettingsDestinationNode = m_DestinationConfigxmlDoc.SelectSingleNode(strmainpath);
                XmlNode AppSettingNode = m_DestinationConfigxmlDoc.SelectSingleNode(strmainpath);

                bool AppSettingsAttribute = false;

                foreach (XmlNode SrcNode in AppsettingsSourceNode.ChildNodes)
                {
                    if (SrcNode.NodeType != XmlNodeType.Element)
                    {
                        continue;
                    }

                    foreach (XmlNode DestNode in AppsettingsDestinationNode.ChildNodes)
                    {
                        if ((DestNode.Attributes != null) && (SrcNode.Attributes != null))
                        {
                            if (DestNode.Attributes["key"].Value == SrcNode.Attributes["key"].Value)
                            {
                                DestNode.Attributes["value"].Value = SrcNode.Attributes["value"].Value;
                                AppSettingsAttribute = true;
                                break;
                            }
                        }
                    }
                    if (AppSettingsAttribute)
                    {
                        AppSettingsAttribute = false;
                    }
                    else
                    {
                        XmlElement newXmlElement = m_DestinationConfigxmlDoc.CreateElement(SrcNode.Name);
                        XmlAttribute key = m_DestinationConfigxmlDoc.CreateAttribute("key");
                        key.Value = SrcNode.Attributes["key"].Value;
                        newXmlElement.Attributes.Append(key);
                        XmlAttribute value = m_DestinationConfigxmlDoc.CreateAttribute("value");
                        value.Value = SrcNode.Attributes["value"].Value;
                        newXmlElement.Attributes.Append(value);
                        AppsettingsDestinationNode.AppendChild(newXmlElement);
                    }

                }

            }
            catch (Exception ex)
            {
                throw new Exception("Install settings file is corrupt.", ex);
            }

        }


        internal void Initialize()
        {
            try
            {
                m_SourceConfigxmlDoc = new ConfigXmlDocument();
                m_DestinationConfigxmlDoc = new ConfigXmlDocument();
                m_SourceConfigxmlDoc.Load(m_ConfigUpdaterSettings.InstallSettingsFile);
                m_DestinationConfigxmlDoc.Load(m_ConfigUpdaterSettings.DestinationFile);

            }
            catch (Exception ex)
            {                
                throw new Exception("Error in initializing Install settings.",ex);
            }
        }
    }
}
