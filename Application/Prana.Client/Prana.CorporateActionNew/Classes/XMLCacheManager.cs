using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml;

namespace Prana.CorporateActionNew.Classes
{
    internal class XMLCacheManager
    {
        #region Singleton Implementation
        private static XMLCacheManager _xmlCacheManager;
        static object _locker = new object();

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
                string pathCorporateActionRules = Application.StartupPath + "\\XMLs\\CorporateActionDlls.xml";

                if (!System.IO.File.Exists(pathCorporateActionRules))
                {
                    return "CorporateActionDlls XML File is not present in XMLs folder. Please put the xml file and restart corpaction module.";
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

        internal void AssignColumnPropertiesForCA(CorporateActionType caType, RowLayout rowLayout1, ControlType controlType)
        {
            try
            {
                if (_caColumnInfo.ContainsKey((int)caType))
                {
                    XmlNode caXMLNode = _caColumnInfo[(int)caType];
                    if (caXMLNode == null)
                    {
                        return;
                    }
                    XmlNodeList nodeList = caXMLNode.SelectNodes("Columns/Column");

                    // Setup the columns specific locations. 
                    foreach (XmlNode colNode in nodeList)
                    {
                        string colName = string.Empty;
                        if (colNode.Attributes["Name"] != null)
                        {
                            colName = colNode.Attributes["Name"].Value.ToString();
                        }

                        RowLayoutColumnInfo colInfo = null;
                        if (String.IsNullOrEmpty(colName))
                        {
                            continue;
                        }
                        else
                        {
                            colInfo = rowLayout1.ColumnInfos[colName];
                        }
                        if (controlType == ControlType.Apply && colNode.Attributes["OriginX"] != null && colNode.Attributes["OriginY"] != null)
                        {
                            int originX = Convert.ToInt32(colNode.Attributes["OriginX"].Value);
                            int originY = Convert.ToInt32(colNode.Attributes["OriginY"].Value);
                            colInfo.Initialize(originX, originY);
                        }

                        if (colNode.Attributes["Column.Hidden"] != null)
                        {
                            colInfo.Column.Hidden = Convert.ToBoolean(colNode.Attributes["Column.Hidden"].Value);
                        }
                        if (colNode.Attributes["Column.Width"] != null)
                        {
                            colInfo.Column.MinWidth = Convert.ToInt32(colNode.Attributes["Column.Width"].Value);
                        }
                        if ((colNode.Attributes["Column.Width"] != null) && (colNode.Attributes["Column.Height"] != null))
                        {
                            colInfo.MinimumCellSize = new System.Drawing.Size(Convert.ToInt32(colNode.Attributes["Column.Width"].Value), Convert.ToInt32(colNode.Attributes["Column.Height"].Value));
                        }
                        if (colNode.Attributes["ButtonDisplayStyle"] != null)
                        {
                            colInfo.Column.ButtonDisplayStyle = (ButtonDisplayStyle)Enum.Parse(typeof(ButtonDisplayStyle), colNode.Attributes["ButtonDisplayStyle"].Value, true);
                        }
                        if (colNode.Attributes["CellActivation"] != null)
                        {
                            colInfo.Column.CellActivation = (Activation)Enum.Parse(typeof(Activation), colNode.Attributes["CellActivation"].Value);

                        }
                        if (colNode.Attributes["CharacterCasing"] != null)
                        {
                            colInfo.Column.CharacterCasing = (CharacterCasing)Enum.Parse(typeof(CharacterCasing), colNode.Attributes["CharacterCasing"].Value);
                        }

                        if (colNode.Attributes["Column.Caption"] != null)
                        {
                            colInfo.Column.Header.Caption = Convert.ToString(colNode.Attributes["Column.Caption"].Value);
                        }



                        #region Commented
                        //TODO : Dynamically assign properties from xml.
                        //object obj = colInfo;
                        //foreach (XmlAttribute attrib in colNode.Attributes)
                        //{
                        //    string property = attrib.Name;
                        //    string[] propArr = property.Split('.');

                        //    if (propArr.Length > 1 && propArr[0].Contains("Column"))
                        //    {
                        //        obj = colInfo.Column;
                        //        property = propArr[1];
                        //    }

                        //    PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
                        //    if ((propertyInfo != null) && (attrib.Value != string.Empty))
                        //    {
                        //        System.Type propType = propertyInfo.PropertyType;

                        //        switch (propType.FullName)
                        //        {
                        //            case "System.Boolean":
                        //                propertyInfo.SetValue(obj, Boolean.Parse(attrib.Value), null);
                        //                break;
                        //            case "System.String":
                        //                propertyInfo.SetValue(obj, attrib.Value, null);
                        //                break;
                        //            case "System.Int32":
                        //                propertyInfo.SetValue(obj, Int32.Parse(attrib.Value), null);
                        //                break;
                        //            default:
                        //                break;
                        //        }

                        //    }
                        //}
                        #endregion Commented

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
                                if (colNode.Attributes["DefaultValue"] != null)
                                    row[col] = colNode.Attributes["DefaultValue"].Value;
                                else
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

        //internal void GetCADllClassPath(CorporateActionType caType, out string className, out string dllPath)
        //{
        //    className = string.Empty;
        //    dllPath = string.Empty;

        //    try
        //    {
        //        if (_caColumnInfo.ContainsKey((int)caType))
        //        {
        //            XmlNode corpActionXMLNode = _caColumnInfo[(int)caType];
        //            className = corpActionXMLNode.Attributes["ClassName"].Value;
        //            dllPath = corpActionXMLNode.Attributes["DllFullPath"].Value;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        //public static List<DataColumn> GetCorporateActionColumnList(DataTable dt, CorporateActionType corporateActionType)
        //{
        //    List<DataColumn> corpActionColList = null;

        //    try
        //    {
        //        XmlNode corpActionXMLNode = _caColumnInfo[(int)corporateActionType];
        //        corpActionColList = new List<DataColumn>();

        //        XmlNodeList nodeList = corpActionXMLNode.SelectNodes("Columns/Column");

        //        foreach (XmlNode colNode in nodeList)
        //        {
        //            string name = colNode.Attributes["Name"].Value.ToString();
        //            if (dt.Columns.Contains(name))
        //            {
        //                corpActionColList.Add(dt.Columns[name]);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //    return corpActionColList;
        //}

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

        public static CAPreferences GetCompanyCAPreferences(int companyID)
        {

            CAPreferences caPrefs = new CAPreferences();

            try
            {

                object[] parameter = new object[1];
                parameter[0] = companyID;
                try
                {
                    using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyCAPrefs", parameter))
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            if (!row[0].Equals(System.DBNull.Value))
                            {
                                byte[] data = (byte[])row[0];

                                MemoryStream stream = new MemoryStream(data);
                                BinaryFormatter bf = new BinaryFormatter();
                                caPrefs = (CAPreferences)bf.Deserialize(stream);
                            }
                        }
                    }
                }
                #region Catch
                catch (Exception ex)
                {
                    //					throw(ex);
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                    if (rethrow)
                    {
                        throw;
                    }
                }
                #endregion
            }
            #region Catch
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
            #endregion
            return caPrefs;

        }

    }
}
