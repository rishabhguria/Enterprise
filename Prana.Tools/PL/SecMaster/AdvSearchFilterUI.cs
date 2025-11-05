using Infragistics.Win;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Prana.Tools.PL.SecMaster
{
    public partial class AdvSearchFilterUI : Form
    {
        int _conditionID = 1;

        public event EventHandler<EventArgs<string>> SearchDataEvent;

        Type _type;
        Dictionary<string, ValueList> _dataValuesList = new Dictionary<string, ValueList>();
        List<string> _removeColumnsList = new List<string>();

        /// <summary>
        /// dictionary to store name of ambigous columns and their corresponding table names
        /// </summary>
        private Dictionary<string, string> _dbAmbigousColumnsDictionary = new Dictionary<string, string>();

        /// <summary>
        /// dictionary to store name of columns in SM and their corresponding column names in database
        /// </summary>
        private Dictionary<string, string> _renameColumnsDictionary = new Dictionary<string, string>();

        //string _MainTableName = string.Empty;
        //List<string> _dbAmbigousColumnsList = new List<string>();
        bool _isIncludeOrExclude;
        int _initializationType;

        //private List<EnumerationValue> _fieldsNamesList = new List<EnumerationValue>();
        //public List<EnumerationValue> FieldsNamesList
        //{
        //    get { return _fieldsNamesList; }
        //    set { _fieldsNamesList = value; }
        //}

        //private Dictionary<String, String> _propNameTypePairs = new Dictionary<string, String>();
        //public Dictionary<String, String> propNameTypePairs
        //{
        //    get { return _propNameTypePairs; }
        //    set { _propNameTypePairs = value; }
        //}

        //Dictionary<string, ValueList> _ValueListDict = new Dictionary<string, ValueList>();
        //public Dictionary<String, ValueList> ValueListDict
        //{
        //    get { return _ValueListDict; }
        //    set { _ValueListDict = value; }
        //}

        /// <summary>
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public AdvSearchFilterUI()
        {
            try
            {
                InitializeComponent();
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        //private void btnAddCondition_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        SearchingCntrl seachCntrl = new SearchingCntrl();
        //        seachCntrl.SetUp(_fieldsNamesList);
        //        seachCntrl.Name = "Condition" + _conditionID;
        //        panelConditions.Controls.Add(seachCntrl);
        //        seachCntrl.RemoveConditionEvent += new SearchingCntrl.RemoveConditionEventHandler(seachCntrl_RemoveConditionEvent);
        //        seachCntrl.GetColumnTypeReq += new SearchingCntrl.GetColumnTypeEventHander(seachCntrl_GetColumnTypeReq);
        //        _conditionID++;
        //        CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
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

        //void seachCntrl_GetColumnTypeReq(string FieldName, object SearchControl)
        //{
        //    SearchingCntrl searchCntrl = SearchControl as SearchingCntrl;
        //    if (searchCntrl != null)
        //    {

        //        if (FieldName.IndexOf('.') > 0)
        //        {
        //            String[] StringArray = FieldName.Split('.');
        //            FieldName = StringArray[1];
        //        }

        //        if (_propNameTypePairs.ContainsKey(FieldName))
        //        {
        //            String fieldType = _propNameTypePairs[FieldName];
        //            ValueList dataValueList = new ValueList();
        //            if (_ValueListDict.ContainsKey(FieldName))
        //            {
        //                dataValueList = _ValueListDict[FieldName];
        //                fieldType = "Enum";
        //            }
        //            searchCntrl.SetConditionControls(fieldType, dataValueList);

        //        }


        //    }
        //    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
        //}

        void searchGroupCntrl_RemoveConditionEvent(object sender, EventArgs<string> e)
        {
            try
            {
                if (panelConditions.Controls.ContainsKey(e.Value))
                {
                    Control cntrl = panelConditions.Controls[e.Value];
                    cntrl.Dispose();
                    panelConditions.Controls.Remove(cntrl);
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //List<SearchCondition> condtionsList = new List<SearchCondition>();
            StringBuilder searchQuery = new StringBuilder();
            try
            {
                int conditionCount = panelConditions.Controls.Count;
                foreach (Control cntrl in panelConditions.Controls)
                {
                    //SearchingCntrl searchControls = cntrl as SearchingCntrl;
                    //if (searchControls != null)
                    //{
                    //    SearchCondition condition = searchControls.GetSearchCondition();
                    //    String conditionString = GetConditonString(condition, conditionCount);
                    //    if (!string.IsNullOrEmpty(conditionString))
                    //    {
                    //        searchQuery.Append(conditionString);
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("Please check your search conditions.", "Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //        return;
                    //    }
                    //}
                    SearchGroup searchGroupCtrls = cntrl as SearchGroup;
                    if (searchGroupCtrls != null)
                    {
                        String condition = searchGroupCtrls.GetGroupSearchCondition(conditionCount);
                        if (!string.IsNullOrEmpty(condition))
                        {
                            searchQuery.Append(condition);
                        }
                        else
                        {
                            MessageBox.Show("Please check your search conditions.", "Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    conditionCount--;
                }
                if (SearchDataEvent != null && searchQuery.Length > 0)
                {
                    SearchDataEvent(sender, new EventArgs<string>(searchQuery.ToString()));
                }
                this.WindowState = FormWindowState.Minimized;
                //  MessageBox.Show("Your Query: " + searchQuery.ToString(), "For Demo only ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraButton2_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
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
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="type"></param>
        //internal void SetUp(Type type)
        //{
        //    try
        //    {
        //        //_fieldsNamesList = GeneralUtilities.GetValueListFromClass(type);
        //        //_propNameTypePairs = GeneralUtilities.GetObjPropNameNTypePair(type);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        // For Report template and create batch
        /// <summary>
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataValuesList"></param>
        /// <param name="ColumnsList"></param>
        /// <param name="isIncludeOrExclude"></param>
        internal void SetUp(Type type, Dictionary<string, ValueList> dataValuesList, List<string> ColumnsList, bool isIncludeOrExclude)
        {
            try
            {
                _type = type;
                _dataValuesList = dataValuesList;
                _removeColumnsList = ColumnsList;
                _isIncludeOrExclude = isIncludeOrExclude;
                _initializationType = 1;

                SearchGroup searchGroupCntrl = new SearchGroup();
                searchGroupCntrl.SetUp(_type, _dataValuesList, _removeColumnsList, _isIncludeOrExclude);
                //panelConditions.Controls.Add(searchGroupCntrl);
                //_fieldsNamesList = GeneralUtilities.GetValueListFromClass(type);
                //_propNameTypePairs = GeneralUtilities.GetObjPropNameNTypePair(type);
                //_ValueListDict = dataValuesList;

                //if (ColumnsList.Count > 0)
                //{
                //    if (isIncludeOrExclude)
                //    {

                //        RemoveUnwantedFieldsNames(ColumnsList);
                //    }
                //    else
                //    {
                //        KeepOnlySelectedFieldsNames(ColumnsList);
                //    }
                //}
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

        //internal void SetUp(Dictionary<String, String> fieldsNameList, Dictionary<string, ValueList> dataValuesList)
        //{
        //    //TODO - omshiv
        //    // this will use when you have XML with field name and field type like int32, String, Boolean,Enum
        //    // and value list for assest class, currencies
        //}

        // For symbol look up
        // Need to make setup function as generic method to be called from other controls
        /// <summary>
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataValuesList"></param>
        /// <param name="removeColumnsList"></param>
        /// <param name="dbAmbigousColumnsDictionary"></param>
        /// <param name="renameColumnsDictionary"></param>
        internal void SetUp(Type type, Dictionary<string, ValueList> dataValuesList, List<string> removeColumnsList, Dictionary<string, string> dbAmbigousColumnsDictionary, Dictionary<string, string> renameColumnsDictionary)
        {
            try
            {
                _type = type;
                _dataValuesList = dataValuesList;
                _removeColumnsList = removeColumnsList;
                _dbAmbigousColumnsDictionary = dbAmbigousColumnsDictionary;
                _renameColumnsDictionary = renameColumnsDictionary;
                //_MainTableName = MainTableName;
                //_dbAmbigousColumnsList = dbAmbigousColumnsList;
                _initializationType = 2;
                SearchGroup searchGroupCntrl = new SearchGroup();

                searchGroupCntrl.SetUp(_type, _dataValuesList, _removeColumnsList, _dbAmbigousColumnsDictionary, _renameColumnsDictionary);
                //panelConditions.Controls.Add(searchGroupCntrl);
                //searchGroupCntrl.RemoveConditionEvent += new SearchGroup.RemoveConditionEventHandler(searchGroupCntrl_RemoveConditionEvent);
                //_propNameTypePairs = GeneralUtilities.GetObjPropNameNTypePair(type);
                //_ValueListDict = dataValuesList;
                //if (removeColumnsList.Count > 0)
                //{
                //    RemoveUnwantedFieldsNames(removeColumnsList);
                //}
                //if (dbAmbigousColumnsList.Count > 0)
                //{
                //    SetFieldsNamesDrpDwnValue(dbAmbigousColumnsList, MainTableName);
                //}
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

        private void AdvSearchFilertUI_Load_1(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_SYMBOL_LOOKUP);
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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

        private void btnAddNewGroup_Click(object sender, EventArgs e)
        {
            try
            {
                // Need to handle for more than 3 logical groups
                if (panelConditions.Controls.Count > 2)
                {
                    return;
                }
                SearchGroup searchGroupCntrl = new SearchGroup();
                switch (_initializationType)
                {
                    case 1:
                        searchGroupCntrl.SetUp(_type, _dataValuesList, _removeColumnsList, _isIncludeOrExclude);
                        break;
                    case 2:
                        searchGroupCntrl.SetUp(_type, _dataValuesList, _removeColumnsList, _dbAmbigousColumnsDictionary, _renameColumnsDictionary);
                        break;
                }
                searchGroupCntrl.Name = "Condition" + _conditionID;
                panelConditions.Controls.Add(searchGroupCntrl);
                searchGroupCntrl.RemoveGroupEvent += new EventHandler<EventArgs<string>>(searchGroupCntrl_RemoveConditionEvent);
                _conditionID++;
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_SYMBOL_LOOKUP);
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
    }
}