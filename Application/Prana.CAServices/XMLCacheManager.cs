using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;

namespace Prana.CAServices
{
    internal class XMLCacheManager
    {
        #region Singleton Implementation
        private static XMLCacheManager _xmlCacheManager;
        static readonly object _locker = new object();

        internal static XMLCacheManager Instance
        {
            get
            {
                if (_xmlCacheManager == null)
                {
                    lock (_locker)
                    {
                        if (_xmlCacheManager == null)
                        {
                            _xmlCacheManager = new XMLCacheManager();
                        }
                    }
                }

                return _xmlCacheManager;
            }
        }
        #endregion


        private static Dictionary<int, XmlNode> _caColumnInfo = new Dictionary<int, XmlNode>();
        public static Dictionary<int, XmlNode> CAColumnInfo
        {
            get { return _caColumnInfo; }
        }

        XmlDocument _xmlDocCorporateAction = new XmlDocument();

        internal string LoadXML()
        {
            try
            {
                string pathCorporateActionRules = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\xmls\\CorporateActionDlls.xml";

                if (!System.IO.File.Exists(pathCorporateActionRules))
                {
                    throw new Exception("CorporateActionDlls XML File is not present in XMLs folder. Please put the xml file and restart corpaction module.");
                }

                _xmlDocCorporateAction.Load(pathCorporateActionRules);

                FillCorporateActionNodes();
                // String.empty represents that there is no error message...
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        /// Fills the node information for the loading of the Dlls at run time
        /// </summary>
        /// <param name="xmlDocCorporateAction"></param>
        private void FillCorporateActionNodes()
        {
            try
            {
                XmlNodeList xmlCorporateActionNodeList = _xmlDocCorporateAction.SelectNodes("CorporateActions/CorporateAction");

                foreach (XmlNode xmlNode in xmlCorporateActionNodeList)
                {
                    //string name = xmlNode.Attributes["Name"].Value;
                    int corporateActionType = Convert.ToInt32(xmlNode.Attributes["CorporateActionType"].Value);

                    if (!_caColumnInfo.ContainsKey(corporateActionType))
                    {
                        _caColumnInfo.Add(corporateActionType, xmlNode);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        internal DataTable GetFullCATable()
        {
            DataTable caFullTable = new DataTable();
            caFullTable.TableName = "CaFullTable";

            try
            {
                XmlNodeList xmlCorporateActionColoumnList = _xmlDocCorporateAction.SelectNodes("CorporateActions/CorporateActionColoumnsSuperSet/Column");

                foreach (XmlNode xmlNode in xmlCorporateActionColoumnList)
                {
                    string name = xmlNode.Attributes["Name"].Value;
                    string typeStr = xmlNode.Attributes["Type"].Value;
                    Type type = System.Type.GetType(typeStr);

                    if (!caFullTable.Columns.Contains(name) && type != null)
                    {
                        caFullTable.Columns.Add(name, type);
                    }
                }

                caFullTable.PrimaryKey = new DataColumn[] { caFullTable.Columns[CorporateActionConstants.CONST_CorporateActionId] };
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

            return caFullTable;
        }



        /// <summary>
        /// Need to make the function code better/dynamic
        /// </summary>
        /// <param name="row"></param>
        /// <param name="corporateActionType"></param>
        internal void InitializeCARow(CorporateActionType corporateActionType, DataRow row)
        {
            try
            {
                if (_caColumnInfo.ContainsKey((int)corporateActionType))
                {
                    XmlNode corpActionXMLNode = _caColumnInfo[(int)corporateActionType];
                    XmlNodeList nodeList = corpActionXMLNode.SelectNodes("Columns/Column");

                    foreach (XmlNode colNode in nodeList)
                    {
                        string name = colNode.Attributes["Name"].Value.ToString();
                        if (row.Table.Columns.Contains(name))
                        {
                            DataColumn col = row.Table.Columns[name];
                            Type type = col.DataType;

                            if (type == typeof(System.String))
                            {
                                row[col] = string.Empty;
                                continue;
                            }
                            if (type == typeof(System.DateTime))
                            {
                                row[col] = DateTime.Now;
                                continue;
                            }
                            if (type == typeof(System.Int32) || type == typeof(System.Int64) || type == typeof(System.Single) || type == typeof(System.Double))
                            {
                                row[col] = 0;
                                continue;
                            }
                            if (type == typeof(ApplicationConstants.SymbologyCodes))
                            {
                                row[col] = -1;
                                continue;
                            }
                            if (type == typeof(System.Boolean))
                            {
                                row[col] = false;
                                continue;
                            }
                            if (type == typeof(System.Guid))
                            {
                                row[col] = Guid.NewGuid();
                                continue;
                            }
                            if (type == typeof(Prana.BusinessObjects.AppConstants.AssetCategory))
                            {
                                row[col] = AssetCategory.Equity;
                                continue;
                            }
                            if (type == typeof(Prana.BusinessObjects.AppConstants.Underlying))
                            {
                                row[col] = Underlying.US;
                                continue;
                            }
                            if (type == typeof(Prana.BusinessObjects.AppConstants.CorporateActionType))
                            {
                                row[col] = corporateActionType;
                                continue;
                            }
                            else
                            {
                                row[col] = string.Empty;
                                continue;
                            }
                        }
                    }
                }

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

        internal void GetCADllClassPath(CorporateActionType caType, out string className, out string dllPath)
        {
            className = string.Empty;
            dllPath = string.Empty;

            try
            {
                if (_caColumnInfo.ContainsKey((int)caType))
                {
                    XmlNode corpActionXMLNode = _caColumnInfo[(int)caType];
                    className = corpActionXMLNode.Attributes["ClassName"].Value;
                    dllPath = corpActionXMLNode.Attributes["DllFullPath"].Value;
                }
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

        public static List<DataColumn> GetCorporateActionColumnList(DataTable dt, CorporateActionType corporateActionType)
        {
            List<DataColumn> corpActionColList = null;

            try
            {
                XmlNode corpActionXMLNode = _caColumnInfo[(int)corporateActionType];
                corpActionColList = new List<DataColumn>();

                XmlNodeList nodeList = corpActionXMLNode.SelectNodes("Columns/Column");

                foreach (XmlNode colNode in nodeList)
                {
                    string name = colNode.Attributes["Name"].Value.ToString();
                    if (dt.Columns.Contains(name))
                    {
                        corpActionColList.Add(dt.Columns[name]);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }


            return corpActionColList;
        }

        internal static void FillDataRowFromXML(DataRow dr, string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);


            XmlNode node = xmlDoc.SelectSingleNode("CaFullTable");

            foreach (XmlNode colNode in node.ChildNodes)
            {
                string name = colNode.Name;
                if (dr.Table.Columns.Contains(name))
                {
                    dr[name] = colNode.InnerText;
                }
            }
        }
    }
}
