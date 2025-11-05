using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ReconTemplate : IDisposable
    {

        [XmlIgnore]
        private bool _isDirtyForSaving = false;
        public bool IsDirtyForSaving
        {
            get { return _isDirtyForSaving; }
            set { _isDirtyForSaving = value; }
        }

        //stores the formatName for the template
        private string _formatName = string.Empty;
        public string FormatName
        {
            get { return _formatName; }
            set { _formatName = value; }
        }

        private string _exceptionReportSavePath = string.Empty;

        public string ExceptionReportSavePath
        {
            get { return _exceptionReportSavePath; }
            set { _exceptionReportSavePath = value; }
        }

        private string _spName = string.Empty;

        public string SpName
        {
            get { return _spName; }
            set { _spName = value; }
        }

        private string _exceptionReportPassword = string.Empty;

        public string ExceptionReportPassword
        {
            get { return _exceptionReportPassword; }
            set { _exceptionReportPassword = value; }
        }

        /// <summary>
        /// Name is saved of Recon XSlT
        /// </summary>
        private string _xsltPath = string.Empty;
        public string XsltPath
        {
            get { return _xsltPath; }
            set { _xsltPath = value; }
        }
        private List<string> _visibleRules = new List<string>();
        //visible rules are not to be saved in preference
        [XmlIgnore]
        public List<string> VisibleRules
        {
            get { return _visibleRules; }
            set { _visibleRules = value; }
        }

        private string _userGroup;
        public string UserGroup
        {
            get { return _userGroup; }
            set { _userGroup = value; }
        }
        private string _templateKey;
        public string TemplateKey
        {
            get { return _templateKey; }
            set { _templateKey = value; }
        }


        private string _templateName;
        public string TemplateName
        {
            get { return _templateName; }
            set { _templateName = value; }
        }


        private int _clientID;
        public int ClientID
        {
            get { return _clientID; }
            set { _clientID = value; }
        }

        private ReconType _reconType;
        public ReconType ReconType
        {
            get { return _reconType; }
            set { _reconType = value; }
        }
        private ReconDateType _reconDateType;
        public ReconDateType ReconDateType
        {
            get { return _reconDateType; }
            set { _reconDateType = value; }
        }
        //exp report formatte
        private AutomationEnum.FileFormat _expReportFormat = new AutomationEnum.FileFormat();
        public AutomationEnum.FileFormat ExpReportFormat
        {
            get { return _expReportFormat; }
            set { _expReportFormat = value; }
        }

        //For New Grouping Logic for PNL
        private SerializableDictionary<string, string> _dictGroupingSummary = new SerializableDictionary<string, string>();

        public SerializableDictionary<string, string> DictGroupingSummary
        {
            get { return _dictGroupingSummary; }
            set { _dictGroupingSummary = value; }
        }

        private ReconFilters _reconFilters = new ReconFilters();

        public ReconFilters ReconFilters
        {
            get { return _reconFilters; }
            set { _reconFilters = value; }
        }

        private DataSet _dsCustomColumns = new DataSet();
        public DataSet DsCustomColumns
        {
            get { return _dsCustomColumns; }
            set { _dsCustomColumns = value; }
        }

        private DataSet _dsMatchingRules = new DataSet();
        public DataSet DsMatchingRules
        {
            get { return _dsMatchingRules; }
            set
            {
                _dsMatchingRules = value;
                UpdateMatchingRulesDict();
            }
        }

        #region to be removed
        //this property should be removed later this is embedded because xsltsetup is changed from grid to textbox
        private DataSet _dsXSLTMapping = new DataSet();
        public DataSet DsXSLTMapping
        {
            get { return _dsXSLTMapping; }
            set
            {
                _dsXSLTMapping = value;
                //UpdateXSLTMappingDict();
            }
        }
        #endregion

        private DataSet _dsMasterColumns = new DataSet();
        public DataSet DsMasterColumns
        {
            get { return _dsMasterColumns; }

            set
            {
                _dsMasterColumns = value;
                UpdateMasterColumnsList();
            }
        }

        private GroupingCriteria _groupingCrieria = new GroupingCriteria();

        public GroupingCriteria GroupingCrieria
        {
            get { return _groupingCrieria; }
            set { _groupingCrieria = value; }
        }


        private ImportFileDetail _importFileDetails = new ImportFileDetail();

        public ImportFileDetail ImportFileDetails
        {
            get { return _importFileDetails; }
            set { _importFileDetails = value; }
        }


        private List<ColumnInfo> _listNirvanaColumns = new List<ColumnInfo>();
        [XmlIgnore]
        public List<ColumnInfo> ListNirvanaColumns
        {
            get { return _listNirvanaColumns; }
            set { _listNirvanaColumns = value; }
        }

        private string _SortingColumnOrder = string.Empty;
        public string SortingColumnOrder
        {
            get { return _SortingColumnOrder; }
            set { _SortingColumnOrder = value; }
        }


        //private List<string> _listNirvanaGridColumns = new List<string>();
        //[XmlIgnore]
        //public List<string> ListNirvanaGridColumns
        //{
        //    get { return _listNirvanaGridColumns; }
        //    set { _listNirvanaGridColumns = value; }
        //}

        //private List<string> _listPBGridColumns = new List<string>();
        //[XmlIgnore]
        //public List<string> ListPBGridColumns
        //{
        //    get { return _listPBGridColumns; }
        //    set { _listPBGridColumns = value; }
        //}

        private List<ColumnInfo> _listPBColumns = new List<ColumnInfo>();
        [XmlIgnore]
        public List<ColumnInfo> ListPBColumns
        {
            get { return _listPBColumns; }
            set { _listPBColumns = value; }
        }


        private List<MatchingRule> _rulesList = new List<MatchingRule>();
        [XmlIgnore]
        public List<MatchingRule> RulesList
        {
            get { return _rulesList; }
            set { _rulesList = value; }
        }

        #region to be removed
        ////this property should be removed later this is embedded because xsltsetup is changed from grid to textbox
        //private List<XsltSetup> _xsltMappingList = new List<XsltSetup>();
        ////[XmlIgnore]
        //public List<XsltSetup> XsltMappingList
        //{
        //    get { return _xsltMappingList; }
        //    set { _xsltMappingList = value; }
        //}
        #endregion

        //list for available columns
        private List<ColumnInfo> _availableColumnList = new List<ColumnInfo>();
        public List<ColumnInfo> AvailableColumnList
        {
            get { return _availableColumnList; }
            set { _availableColumnList = value; }
        }

        //list for selected columns
        private List<ColumnInfo> _selectedColumnList = new List<ColumnInfo>();
        public List<ColumnInfo> SelectedColumnList
        {
            get { return _selectedColumnList; }
            set { _selectedColumnList = value; }
        }

        private List<ColumnInfo> _listGroupByColumns = new List<ColumnInfo>();
        public List<ColumnInfo> ListGroupByColumns
        {
            get { return _listGroupByColumns; }
            set { _listGroupByColumns = value; }
        }

        private List<string> _EditableColumns = new List<string>();
        public List<string> EditableColumns
        {
            get { return _EditableColumns; }
            set { _EditableColumns = value; }
        }


        private List<ColumnInfo> _listSortByColumns = new List<ColumnInfo>();
        public List<ColumnInfo> ListSortByColumns
        {
            get { return _listSortByColumns; }
            set { _listSortByColumns = value; }
        }

        private bool _isIncludeMatchedItems;

        public bool IsIncludeMatchedItems
        {
            get { return _isIncludeMatchedItems; }
            set { _isIncludeMatchedItems = value; }
        }


        private bool _isIncludeToleranceMacthedItems;

        public bool IsIncludeToleranceMacthedItems
        {
            get { return _isIncludeToleranceMacthedItems; }
            set { _isIncludeToleranceMacthedItems = value; }
        }

        [XmlIgnore]
        private bool _isReconReportToBeGenerated = false;
        public bool isReconReportToBeGenerated
        {
            get { return _isReconReportToBeGenerated; }
            set { _isReconReportToBeGenerated = value; }
        }

        private void UpdateMatchingRulesDict()
        {
            try
            {
                _rulesList.Clear();
                if (_dsMatchingRules.Tables.Count > 0)
                {
                    string myXML = _dsMatchingRules.GetXml();
                    XmlDocument xmldocMatchingRule = new XmlDocument();
                    xmldocMatchingRule.LoadXml(myXML);

                    XmlNodeList xmlNodesRulesDetails = xmldocMatchingRule.SelectNodes("RuleDetails/Rule");
                    foreach (XmlNode xmlNode in xmlNodesRulesDetails)
                    {
                        //string name = xmlNode.Attributes["Name"].Value;
                        ////string SP = xmlNode.Attributes["SP"].Value;
                        //string isVisible = xmlNode.Attributes["IsVisible"].Value;
                        string reconType = xmlNode.Attributes["ReconType"].Value;
                        // List<MatchingRule> rules = new List<MatchingRule>();

                        if (reconType.Equals(((int)(_reconType)).ToString()))
                        {
                            MatchingRule rule1 = new MatchingRule(xmlNode);
                            //rules.Add(rule1);
                            _rulesList.Add(rule1);
                        }
                        //if (_rulesDict.ContainsKey(name))
                        //{
                        //  _rulesDict[name] =  rules;
                        //}
                        //else
                        //{.
                        //if (!_rulesList.ContainsKey(name))
                        //{
                        //    _rulesList.Add(name, rules);
                        //}
                        // }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public object Clone()
        {
            return Prana.Global.Utilities.DeepCopyHelper.Clone(this);
        }


        private bool _isShowCAGeneratedTrades = true;
        public bool IsShowCAGeneratedTrades
        {
            get { return _isShowCAGeneratedTrades; }
            set { _isShowCAGeneratedTrades = value; }
        }
        private void UpdateMasterColumnsList()
        {
            try
            {
                _listNirvanaColumns.Clear();
                _listPBColumns.Clear();

                DataTable dtNirvanaGridColumns = null;
                DataTable dtPbGridColumns = null;

                if (DsMasterColumns.Tables.Contains("NirvanaGridMasterColumns"))
                {
                    dtNirvanaGridColumns = DsMasterColumns.Tables["NirvanaGridMasterColumns"];
                }
                if (DsMasterColumns.Tables.Contains("PBGridMasterColumns"))
                {
                    dtPbGridColumns = DsMasterColumns.Tables["PBGridMasterColumns"];
                }

                if (dtNirvanaGridColumns != null)
                {
                    foreach (DataRow dr in dtNirvanaGridColumns.Rows)
                    {
                        ColumnInfo column = new ColumnInfo();
                        if (dr["Name"] != System.DBNull.Value)
                        {
                            column.ColumnName = dr["Name"].ToString();
                        }
                        if (dr["IsSelected"] != System.DBNull.Value)
                        {
                            column.IsSelected = bool.Parse((dr["IsSelected"].ToString()));
                        }

                        if (dr["FormulaExpression"] != System.DBNull.Value)
                        {
                            column.FormulaExpression = dr["FormulaExpression"].ToString();
                        }

                        _listNirvanaColumns.Add(column);
                    }
                }
                if (dtPbGridColumns != null)
                {
                    foreach (DataRow dr in dtPbGridColumns.Rows)
                    {
                        ColumnInfo column = new ColumnInfo();
                        if (dr["Name"] != System.DBNull.Value)
                        {
                            column.ColumnName = dr["Name"].ToString();
                        }
                        if (dr["IsSelected"] != System.DBNull.Value)
                        {
                            column.IsSelected = bool.Parse((dr["IsSelected"].ToString()));
                        }
                        if (dr["FormulaExpression"] != System.DBNull.Value)
                        {
                            column.FormulaExpression = dr["FormulaExpression"].ToString();
                        }
                        _listPBColumns.Add(column);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_dsMasterColumns != null)
                        _dsMasterColumns.Dispose();
                    if (_dsMatchingRules != null)
                        _dsMatchingRules.Dispose();
                    if (_dsCustomColumns != null)
                        _dsCustomColumns.Dispose();
                    if (_dsXSLTMapping != null)
                        _dsXSLTMapping.Dispose();
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
        public bool IsReconTemplateGroupping()
        {
            try
            {
                if (this.GroupingCrieria.IsGroupByBroker
                    || this.GroupingCrieria.IsGroupByAccount
                     || this.GroupingCrieria.IsGroupbyMasterFund
                     || this.GroupingCrieria.IsGroupBySide
                     || this.GroupingCrieria.IsGroupBySymbol)
                {
                    return true;
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
            return false;
        }
    }
}
