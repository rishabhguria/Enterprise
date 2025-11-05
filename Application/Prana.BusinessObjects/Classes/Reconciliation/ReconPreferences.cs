using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ReconPreferences
    {

        private string _xmlRulePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconRulesFile.ToString();

        public string XmlRulePath
        {
            get { return _xmlRulePath; }
        }

        static readonly object _locker = new object();
        private static ReconPreferences _reconPreferences = null;
        public ReconPreferences()
        {

        }

        public static ReconPreferences GetInstance
        {
            get
            {
                if (_reconPreferences == null)
                {
                    lock (_locker)
                    {
                        if (_reconPreferences == null)
                        {
                            _reconPreferences = new ReconPreferences();
                        }
                    }
                }
                return _reconPreferences;
            }
        }

        //  [XmlIgnore]
        //public     Dictionary<string, List<MatchingRule>> rulesDict = new Dictionary<string, List<MatchingRule>>();
        //// static Dictionary<string, string> reconTypeSPDict = new Dictionary<string, string>()

        //[XmlIgnore]
        //public Dictionary<string, List<XsltSetup>> xsltMappingDict = new Dictionary<string, List<XsltSetup>>();
        [NonSerialized]
        [XmlElement("ReconTemplatesRunUploadList")]
        private static Dictionary<string, RunUpload> _dictRunUpload = new Dictionary<string, RunUpload>();
        public static Dictionary<string, RunUpload> DictRunUpload
        {
            get { return _dictRunUpload; }
            set { _dictRunUpload = value; }
        }


        [NonSerialized]
        [XmlElement("ReconTemplatesPreferencesList")]
        private SerializableDictionary<string, ReconTemplate> _dictReconTemplates = new SerializableDictionary<string, ReconTemplate>();
        public SerializableDictionary<string, ReconTemplate> DictReconTemplates
        {
            get { return _dictReconTemplates; }
            set { _dictReconTemplates = value; }
        }

        [XmlIgnore]
        public List<string> listDeletedTemplates = new List<string>();


        //[XmlIgnore]
        //public SerializableDictionary<string, ReconTemplate> newReconTemplates = new SerializableDictionary<string, ReconTemplate>();
        //list to get root template name



        /// <summary>
        /// templates will be created based on client name, recon type, template name
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="reconType"></param>
        /// <param name="TemplateName"></param>
        /// <returns></returns>
        public ReconTemplate AddDefaultTemplate(int clientID, string reconType, string TemplateName)
        {
            ReconTemplate template = null;
            try
            {
                //check that template exist
                //Now template key = clientName+recnType+templateName

                if (_dictReconTemplates.ContainsKey(clientID.ToString() + Seperators.SEPERATOR_6 + reconType + Seperators.SEPERATOR_6 + TemplateName))
                {
                    return _dictReconTemplates[clientID.ToString() + Seperators.SEPERATOR_6 + reconType + Seperators.SEPERATOR_6 + TemplateName];
                }
                //return default value 
                template = new ReconTemplate();
                template.IsDirtyForSaving = true;
                if (GetInstance.getRootTemplates().Contains(reconType))
                {
                    template.ReconType = (ReconType)Enum.Parse(typeof(ReconType), reconType);
                    template.ClientID = clientID;
                    template.TemplateName = TemplateName;
                    template.TemplateKey = clientID.ToString() + Seperators.SEPERATOR_6 + reconType + Seperators.SEPERATOR_6 + TemplateName;
                }
                _dictReconTemplates.Add(clientID.ToString() + Seperators.SEPERATOR_6 + reconType + Seperators.SEPERATOR_6 + TemplateName, template);
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

            return template;
        }

        public bool CheckTemplateExists(string TemplateKey)
        {
            try
            {
                if (_dictReconTemplates.ContainsKey(TemplateKey))
                {
                    return true;
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
            return false;
        }
        public ReconTemplate GetTemplates(string TemplateKey)
        {
            ReconTemplate template = null;
            try
            {
                if (_dictReconTemplates.ContainsKey(TemplateKey))
                {
                    return _dictReconTemplates[TemplateKey];
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

            return template;
        }

        public void UpdateTemplates(string key, ReconTemplate Template)
        {
            try
            {
                if (_dictReconTemplates.ContainsKey(key))
                {
                    _dictReconTemplates[key] = Template;
                }
                else
                {
                    _dictReconTemplates.Add(key, Template);
                }
            }
            #region catch
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

        }


        public void Update(ReconPreferences updatedPreferences)
        {
            try
            {
                //Remove templates which have been deleted by the user..
                // Surendra Bisht       http://jira.nirvanasolutions.com:8080/browse/CI-365
                // delete templates First ,then update them .  Prevents deleting templates which are to be update .As if a new template is created, not saved.then it is renamed then  need to be deleted in the dictionary ,but not in template.
                foreach (string templateToDelete in updatedPreferences.listDeletedTemplates)
                {
                    DeleteTemplates(templateToDelete);
                }
                foreach (KeyValuePair<string, ReconTemplate> kp in updatedPreferences._dictReconTemplates)
                {
                    if (kp.Value.IsDirtyForSaving)
                    {
                        UpdateTemplates(kp.Key, kp.Value);
                        kp.Value.IsDirtyForSaving = false;
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
            //  IsClearExpCache = updatedPreferences.IsClearExpCache;
        }


        public void DeleteTemplates(string key)
        {
            try
            {
                if (_dictReconTemplates.ContainsKey(key))
                {
                    listDeletedTemplates.Add(key);
                    _dictReconTemplates.Remove(key);
                }
            }
            #region catch
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
        }


        public List<string> GetListOfTemplates(string reconType, int clientID)
        {
            List<string> listTemplates = new List<string>();
            try
            {
                foreach (KeyValuePair<string, ReconTemplate> kvp in _dictReconTemplates)
                {
                    ReconTemplate template = kvp.Value;
                    if ((template.ReconType).ToString().Equals(reconType) && template.ClientID == clientID)
                    {
                        listTemplates.Add(GetTemplateNameFromTemplateKey(kvp.Key));
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
            return listTemplates;
        }

        public static string GetTemplateNameFromTemplateKey(string key)
        {
            string templateName = string.Empty;
            try
            {
                string[] arr = key.Split(Seperators.SEPERATOR_6);
                templateName = arr[arr.Length - 1];
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
            return templateName;
        }

        public string GetXsltFileName(string templateKey)
        {
            if (_dictReconTemplates.ContainsKey(templateKey))
            {
                //path in template is relative path so intial directory path is to be appended.
                string filePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconXSLT.ToString() + @"\" + _dictReconTemplates[templateKey].XsltPath;
                string shortName = filePath.Substring(filePath.LastIndexOf('\\') + 1);
                return shortName;
            }
            else
                return string.Empty;
        }


        public List<MatchingRule> GetListOfRules(string templateKey)
        {
            if (_dictReconTemplates.ContainsKey(templateKey))
            {
                ReconTemplate template = _dictReconTemplates[templateKey];
                return template.RulesList;
                //return _oldReconPreferences.rulesDict[ruleFileName];
            }
            else
                return null;
        }

        public GroupingCriteria GetGroupingCriteria(string TemplateName)
        {
            if (_dictReconTemplates.ContainsKey(TemplateName))
            {
                ReconTemplate template = _dictReconTemplates[TemplateName];
                return template.GroupingCrieria;
                //return _oldReconPreferences.rulesDict[ruleFileName];
            }

            return new GroupingCriteria();

        }


        public List<ColumnInfo> GetNirvanaMasterColumns(string templateName)
        {
            //List<ColumnType> listMasterColumns = new List<ColumnType>();
            if (_dictReconTemplates.ContainsKey(templateName))
            {
                return _dictReconTemplates[templateName].ListNirvanaColumns;
            }
            return new List<ColumnInfo>();
        }

        public List<string> GetNirvanaGridDisplayColumnNames(string templateName)
        {
            List<string> listColumnNames = new List<string>();
            // List<MasterColumn> listMasterColumns = new List<MasterColumn>();
            if (_dictReconTemplates.ContainsKey(templateName))
            {
                foreach (ColumnInfo column in _dictReconTemplates[templateName].ListNirvanaColumns)
                {
                    if (column.IsSelected == true)
                        listColumnNames.Add(column.ColumnName);
                }
                return listColumnNames;
            }
            return new List<string>();
        }

        public List<string> GetSkipFormatColumnsList()
        {
            List<string> listSkippedColumnNames = new List<string>();
            listSkippedColumnNames.Add("TradeDate");
            listSkippedColumnNames.Add("ExpirationDate");
            listSkippedColumnNames.Add("ProcessDate");
            listSkippedColumnNames.Add("SettlementDate");
            listSkippedColumnNames.Add("OriginalPurchaseDate");
            listSkippedColumnNames.Add("PayoutDate");
            listSkippedColumnNames.Add("ExDate");

            return listSkippedColumnNames;
        }

        public List<ColumnInfo> GetPBMasterColumns(string templateName)
        {
            //List<ColumnType> listMasterColumns = new List<ColumnType>();
            if (_dictReconTemplates.ContainsKey(templateName))
            {
                return _dictReconTemplates[templateName].ListPBColumns;
            }
            return new List<ColumnInfo>();
        }


        /// <summary>
        /// Returnscustom column form prana mode recon set from masterColumn UI
        /// From Master column UI, New custom Columns are not updating after first change on Recon UI.
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-5476
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<ColumnInfo> GetPranaCustomColumns(string templateName, DataSourceType type)
        {
            List<ColumnInfo> listCustomColumns = new List<ColumnInfo>();
            if (_dictReconTemplates.ContainsKey(templateName))
            {
                if (type == DataSourceType.Nirvana)
                {
                    listCustomColumns = _dictReconTemplates[templateName].ListNirvanaColumns.FindAll(delegate (ColumnInfo column)
                      {
                          if (!string.IsNullOrEmpty(column.ColumnName) && column.IsSelected && !string.IsNullOrEmpty(column.FormulaExpression))
                          {
                              return true;
                          }
                          return false;
                      });
                }
                else if (type == DataSourceType.PrimeBroker)
                {
                    listCustomColumns = _dictReconTemplates[templateName].ListPBColumns.FindAll(delegate (ColumnInfo column)
                      {
                          if (!string.IsNullOrEmpty(column.ColumnName) && column.IsSelected && !string.IsNullOrEmpty(column.FormulaExpression))
                          {
                              return true;
                          }
                          return false;
                      });
                }
            }
            return listCustomColumns;
        }

        public List<string> GetPBGridDisplayColumnNames(string templateName)
        {
            List<string> listColumnNames = new List<string>();
            foreach (ColumnInfo column in _dictReconTemplates[templateName].ListPBColumns)
            {
                if (column.IsSelected == true)
                    listColumnNames.Add(column.ColumnName);
            }
            return listColumnNames;
        }

        public Dictionary<ReconFilterType, Dictionary<int, string>> GetReconFilters(string templateName)
        {

            Dictionary<ReconFilterType, Dictionary<int, string>> dictFilters = new Dictionary<ReconFilterType, Dictionary<int, string>>();

            if (_dictReconTemplates.ContainsKey(templateName))
            {
                ReconTemplate template = _dictReconTemplates[templateName];

                if (template.ReconFilters.DictAssets.Count > 0)
                {
                    Dictionary<int, string> dictAssets = new Dictionary<int, string>();

                    foreach (KeyValuePair<int, string> kp in template.ReconFilters.DictAssets)
                    {
                        dictAssets.Add(kp.Key, kp.Value);
                    }
                    dictFilters.Add(ReconFilterType.Asset, dictAssets);
                }

                if (template.ReconFilters.DictAUECs.Count > 0)
                {
                    Dictionary<int, string> dictAUECs = new Dictionary<int, string>();

                    foreach (KeyValuePair<int, string> kp in template.ReconFilters.DictAUECs)
                    {
                        dictAUECs.Add(kp.Key, kp.Value);
                    }
                    dictFilters.Add(ReconFilterType.AUEC, dictAUECs);
                }
                if (template.ReconFilters.DictBrokers.Count > 0)
                {
                    Dictionary<int, string> dictBrokers = new Dictionary<int, string>();

                    foreach (KeyValuePair<int, string> kp in template.ReconFilters.DictBrokers)
                    {
                        dictBrokers.Add(kp.Key, kp.Value);
                    }
                    dictFilters.Add(ReconFilterType.CounterParty, dictBrokers);
                }
                if (template.ReconFilters.DictAccounts.Count > 0)
                {
                    Dictionary<int, string> dictAccounts = new Dictionary<int, string>();

                    foreach (KeyValuePair<int, string> kp in template.ReconFilters.DictAccounts)
                    {
                        dictAccounts.Add(kp.Key, kp.Value);
                    }
                    dictFilters.Add(ReconFilterType.Account, dictAccounts);
                }

                if (template.ReconFilters.DictMasterFunds.Count > 0)
                {
                    Dictionary<int, string> dictMasterFunds = new Dictionary<int, string>();

                    foreach (KeyValuePair<int, string> kp in template.ReconFilters.DictMasterFunds)
                    {
                        dictMasterFunds.Add(kp.Key, kp.Value);
                    }
                    dictFilters.Add(ReconFilterType.MasterFund, dictMasterFunds);
                }

                if (template.ReconFilters.DictPrimeBrokers.Count > 0)
                {
                    Dictionary<int, string> dictPrimeBrokers = new Dictionary<int, string>();

                    foreach (KeyValuePair<int, string> kp in template.ReconFilters.DictPrimeBrokers)
                    {
                        dictPrimeBrokers.Add(kp.Key, kp.Value);
                    }
                    dictFilters.Add(ReconFilterType.PrimeBroker, dictPrimeBrokers);
                }

            }

            return dictFilters;

        }


        public void SetDefaultReportGeneratedProperty()
        {
            try
            {
                foreach (KeyValuePair<string, ReconTemplate> kvp in _dictReconTemplates)
                {
                    kvp.Value.isReconReportToBeGenerated = false;
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

        /// <summary>
        /// get custom columns
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public DataSet GetCustomColumns(string templateName)
        {
            try
            {
                DataSet dsCustomColumns = new DataSet();
                if (_dictReconTemplates.ContainsKey(templateName))
                {
                    dsCustomColumns = _dictReconTemplates[templateName].DsCustomColumns;
                }
                return dsCustomColumns;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }
    }

    [Serializable]
    public struct ColumnInfo
    {
        private string _columnName;
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        private ColumnGroupType _groupType;
        public ColumnGroupType GroupType
        {
            get { return _groupType; }
            set { _groupType = value; }
        }

        private bool _isCommonColumn;

        public bool IsCommonColumn
        {
            get { return _isCommonColumn; }
            set { _isCommonColumn = value; }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        private int _visibleOrder;

        public int VisibleOrder
        {
            get { return _visibleOrder; }
            set { _visibleOrder = value; }
        }

        private string _formulaExpression;

        public string FormulaExpression
        {
            get { return _formulaExpression; }
            set { _formulaExpression = value; }
        }

        private SortingOrder _sortType;

        public SortingOrder SortOrder
        {
            get { return _sortType; }
            set { _sortType = value; }
        }
    }


}
