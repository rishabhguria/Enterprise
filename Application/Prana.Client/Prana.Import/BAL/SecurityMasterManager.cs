using Prana.BusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml;

namespace Prana.Import
{
    public class SecurityMasterManager
    {
        #region singleton
        private static SecurityMasterManager instance;
        private static object syncRoot = new Object();

        private SecurityMasterManager() { }

        public static SecurityMasterManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new SecurityMasterManager();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        static ISecurityMasterServices _securityMaster = null;
        public ISecurityMasterServices SecurityMaster
        {
            get
            {
                return _securityMaster;
            }
            set
            {
                _securityMaster = value;
            }
        }

        #region SM Mapping
        public void GenerateSMMapping(DataSet ds)
        {
            try
            {
                string dirPath = ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.PMImportXSLT.ToString();
                DataColumn[] keys = new DataColumn[1];

                bool blnSMMappingReq = ds.Tables[0].Columns.Contains("SMMappingReq");

                if (blnSMMappingReq)
                {

                    Dictionary<string, XmlNode> smMappingCOLList = new Dictionary<string, XmlNode>();
                    DataTable dtSMMapping = new DataTable();
                    string smMappingXMLName = ds.Tables[0].Rows[0]["SMMappingReq"].ToString();

                    smMappingXMLName = Application.StartupPath + @"\" + dirPath + @"\" + smMappingXMLName;

                    XmlDocument xmldocSMMapping = new XmlDocument();
                    xmldocSMMapping.Load(smMappingXMLName);

                    XmlNodeList xmlMappingColumns = xmldocSMMapping.SelectNodes("SecMasterMapping/SMData");

                    foreach (XmlNode node in xmlMappingColumns)
                    {
                        if (!smMappingCOLList.ContainsKey(node.Attributes["PMCOLName"].Value))
                        {
                            smMappingCOLList.Add(node.Attributes["PMCOLName"].Value, node);

                            DataColumn dc = new DataColumn(node.Attributes["SMCOLName"].Value);

                            string type = node.Attributes["type"].Value.ToString();

                            if (type.ToLower().Equals("string"))
                            {
                                dc.DataType = typeof(string);
                            }
                            else if (type.ToLower().Equals("int"))
                            {
                                dc.DataType = typeof(int);
                            }
                            else if (type.ToLower().Equals("double"))
                            {
                                dc.DataType = typeof(double);
                            }
                            else if (type.ToLower().Equals("datetime"))
                            {
                                dc.DataType = typeof(DateTime);
                            }

                            if (dc.ColumnName.Equals("TickerSymbol"))
                            {
                                keys[0] = dc;
                            }
                            dtSMMapping.Columns.Add(dc);
                            dtSMMapping.PrimaryKey = keys;
                        }
                    }
                    if (smMappingCOLList.Count > 0)
                    {
                        GeneralUtilities.InsertDataIntoOneTableFromAnotherTable(ds.Tables[0], dtSMMapping, smMappingCOLList);
                        RemoveSMCachedDataFromEnRichedTable(dtSMMapping);
                        SendSMEnRichData(dtSMMapping);
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
        }

        private void RemoveSMCachedDataFromEnRichedTable(DataTable dtSMMapping)
        {
            try
            {
                if (dtSMMapping != null && dtSMMapping.Rows.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (DataRow drow in dtSMMapping.Rows)
                    {
                        if (!string.IsNullOrEmpty(drow["TickerSymbol"].ToString()))
                        {
                            secMasterRequestObj.AddData(drow["TickerSymbol"].ToString(), ApplicationConstants.SymbologyCodes.TickerSymbol);
                        }
                    }

                    if (secMasterRequestObj != null && secMasterRequestObj.Count > 0)
                    {
                        secMasterRequestObj.HashCode = this.GetHashCode();
                        List<SecMasterBaseObj> secMasterCollection = _securityMaster.GetSMCachedData(secMasterRequestObj);

                        if (secMasterCollection != null && secMasterCollection.Count > 0)
                        {
                            foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                            {
                                DataRow[] rows = dtSMMapping.Select("TickerSymbol=" + "'" + secMasterObj.TickerSymbol + "'");
                                foreach (DataRow row in rows)
                                {
                                    dtSMMapping.BeginInit();
                                    dtSMMapping.Rows.Remove(row);
                                    dtSMMapping.EndInit();
                                    dtSMMapping.AcceptChanges();
                                }
                            }
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
        }

        public void SendSMEnRichData(DataTable dtSMMapping)
        {
            try
            {
                if (dtSMMapping != null && dtSMMapping.Rows.Count > 0)
                {
                    _securityMaster.EnRichSecMasterObj(dtSMMapping);
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
        }
        #endregion

        public List<SecMasterBaseObj> SendRequest(SecMasterRequestObj secMasterRequestObj)
        {
            try
            {
                if (SecurityMaster != null && SecurityMaster.IsConnected)
                {
                    secMasterRequestObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                    return SecurityMaster.SendRequestList(secMasterRequestObj);
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
            return null;
        }

        public void SaveNewSymbols_Import(SecMasterbaseList secMasterData)
        {
            try
            {
                if (SecurityMaster != null && SecurityMaster.IsConnected)
                    SecurityMaster.SaveNewSymbols_Import(secMasterData);
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
        }

        public void UpdateSymbols_Import(SecMasterUpdateDataByImportList secMasterData)
        {
            try
            {
                if (SecurityMaster != null && SecurityMaster.IsConnected)
                    SecurityMaster.UpdateSymbols_Import(secMasterData);
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
        }
    }
}