using Infragistics.Win;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.Utilities.UI.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public partial class SearchGroup : UserControl
    {
        int _conditionID = 1;

        //public delegate void RemoveGroupEventHandler(String ControlName);
        //public event RemoveGroupEventHandler RemoveGroupEvent;
        public event EventHandler<EventArgs<string>> RemoveGroupEvent;

        private List<EnumerationValue> _fieldsNamesList = new List<EnumerationValue>();
        public List<EnumerationValue> FieldsNamesList
        {
            get { return _fieldsNamesList; }
            set { _fieldsNamesList = value; }
        }

        private Dictionary<String, String> _propNameTypePairs = new Dictionary<string, String>();
        public Dictionary<String, String> propNameTypePairs
        {
            get { return _propNameTypePairs; }
            set { _propNameTypePairs = value; }
        }

        Dictionary<string, ValueList> _ValueListDict = new Dictionary<string, ValueList>();
        public Dictionary<String, ValueList> ValueListDict
        {
            get { return _ValueListDict; }
            set { _ValueListDict = value; }
        }

        public SearchGroup()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnDeleteGroup.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnDeleteGroup.ForeColor = System.Drawing.Color.White;
                btnDeleteGroup.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnDeleteGroup.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnDeleteGroup.UseAppStyling = false;
                btnDeleteGroup.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnAddCondition.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnAddCondition.ForeColor = System.Drawing.Color.White;
                btnAddCondition.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAddCondition.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAddCondition.UseAppStyling = false;
                btnAddCondition.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void btnAddCondition_Click(object sender, EventArgs e)
        {
            try
            {
                SearchingCntrl seachCntrl = new SearchingCntrl();
                seachCntrl.SetUp(_fieldsNamesList);
                seachCntrl.Name = "Condition" + _conditionID;
                panelConditions.Controls.Add(seachCntrl);
                seachCntrl.RemoveConditionEvent += new EventHandler<EventArgs<string>>(seachCntrl_RemoveConditionEvent);
                //new SearchingCntrl.RemoveConditionEventHandler(seachCntrl_RemoveConditionEvent);
                seachCntrl.GetColumnTypeReq += new EventHandler<EventArgs<string>>(seachCntrl_GetColumnTypeReq);
                //new SearchingCntrl.GetColumnTypeEventHander(seachCntrl_GetColumnTypeReq);
                _conditionID++;
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_SYMBOL_LOOKUP);
                //this.Height += 25;
                //this.panelConditions.Height += 25;
                // CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
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

        // For ReportTemplate and create batch UI
        public void SetUp(Type type, Dictionary<string, ValueList> dataValuesList, List<string> ColumnsList, bool isIncludeOrExclude)
        {
            try
            {
                _fieldsNamesList = GeneralUtilities.GetValueListFromClass(type);
                _propNameTypePairs = GeneralUtilities.GetObjPropNameNTypePair(type);
                _ValueListDict = dataValuesList;

                //if (isInitialize)
                //    btnDeleteGroup.Visible = false;
                //else
                //    btnDeleteGroup.Visible = true;

                cmbBoxAndOr.DataSource = EnumHelper.ConvertEnumForBindingWithCaption(typeof(SecMasterConstants.AllAnyCondition));
                cmbBoxAndOr.DataBind();
                cmbBoxAndOr.DisplayMember = "DisplayText";
                cmbBoxAndOr.ValueMember = "Value";
                cmbBoxAndOr.Value = SecMasterConstants.AllAnyCondition.Or;
                cmbBoxAndOr.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;

                if (ColumnsList.Count > 0)
                {
                    if (isIncludeOrExclude)
                    {

                        RemoveUnwantedFieldsNames(ColumnsList);
                    }
                    else
                    {
                        KeepOnlySelectedFieldsNames(ColumnsList);
                    }
                }
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
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
        /// Set up Advance search for symbol look up UI
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataValuesList"></param>
        /// <param name="removeColumnsList"></param>
        /// <param name="dbAmbigousColumnsDictionary"></param>
        /// <param name="renameColumnsDictionary"></param>
        /// <param name="isInitialize"></param>
        public void SetUp(Type type, Dictionary<string, ValueList> dataValuesList, List<string> removeColumnsList, Dictionary<string, string> dbAmbigousColumnsDictionary, Dictionary<string, string> renameColumnsDictionary)
        {
            try
            {
                _fieldsNamesList = GeneralUtilities.GetValueListFromClass(type);
                _propNameTypePairs = GeneralUtilities.GetObjPropNameNTypePair(type);
                _ValueListDict = dataValuesList;

                //if (isInitialize)
                //    btnDeleteGroup.Visible = false;
                //else
                //    btnDeleteGroup.Visible = true;

                cmbBoxAndOr.DataSource = EnumHelper.ConvertEnumForBindingWithCaption(typeof(SecMasterConstants.AllAnyCondition));
                cmbBoxAndOr.DataBind();
                cmbBoxAndOr.DisplayMember = "DisplayText";
                cmbBoxAndOr.ValueMember = "Value";
                cmbBoxAndOr.Value = SecMasterConstants.AllAnyCondition.Or;
                cmbBoxAndOr.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;

                /*This function will first remove columns which are not required
                 *Then rename the columns to their corresponding column name in database
                 *Then for ambigous columns set column name using their corresponding table name, PRANA-11280
                 */
                if (removeColumnsList.Count > 0)
                {
                    RemoveUnwantedFieldsNames(removeColumnsList);
                }
                if (renameColumnsDictionary.Count > 0)
                {
                    RenameFieldsNamesDrpDwnValue(renameColumnsDictionary);
                    RenamePropNameTypePairsDrpDwnValue(renameColumnsDictionary);
                }
                if (dbAmbigousColumnsDictionary.Count > 0)
                {
                    SetFieldsNamesDrpDwnValue(dbAmbigousColumnsDictionary);
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

        private void KeepOnlySelectedFieldsNames(List<string> columnsToShowOnly)
        {
            try
            {
                List<EnumerationValue> tempFieldsNames = new List<EnumerationValue>();// DeepCopyHelper.Clone(_fieldsNamesList);

                foreach (EnumerationValue enumValue in _fieldsNamesList)
                {
                    if (columnsToShowOnly.Contains(enumValue.Value.ToString()))
                    {
                        tempFieldsNames.Add(enumValue);

                    }

                }
                _fieldsNamesList = tempFieldsNames;
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

        private void RemoveUnwantedFieldsNames(List<string> removeColumnsList)
        {
            try
            {
                List<EnumerationValue> tempFieldsNames = DeepCopyHelper.Clone(_fieldsNamesList);
                int index = 0;
                foreach (EnumerationValue enumValue in tempFieldsNames)
                {
                    if (removeColumnsList.Contains(enumValue.Value.ToString()))
                    {
                        _fieldsNamesList.RemoveAt(index);
                        index--;
                    }
                    index++;
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
        /// Set field names for ambigous columns
        /// </summary>
        /// <param name="dbAmbigousColumnsDictionary"></param>
        private void SetFieldsNamesDrpDwnValue(Dictionary<string, string> dbAmbigousColumnsDictionary)
        {
            try
            {
                List<EnumerationValue> tempFieldsNames = DeepCopyHelper.Clone(_fieldsNamesList);
                int index = 0;
                foreach (EnumerationValue enumValue in tempFieldsNames)
                {
                    if (dbAmbigousColumnsDictionary.Keys.Contains(enumValue.Value.ToString()))
                    {
                        String displayText = enumValue.DisplayText;
                        String value = string.Empty;
                        List<string> tableNames = dbAmbigousColumnsDictionary[enumValue.Value.ToString()].Split(',').ToList();
                        if (tableNames.Count > 1)
                        {
                            value = "COALESCE(";
                            tableNames.ForEach(tab =>
                            {
                                if (!string.IsNullOrWhiteSpace(tab))
                                    value = value + tab + "." + enumValue.Value + ",";
                            });
                            value = value.Remove(value.Length - 1) + ")";
                        }
                        else
                        {
                            value = dbAmbigousColumnsDictionary[enumValue.Value.ToString()] + "." + enumValue.Value;
                        }
                        _fieldsNamesList.RemoveAt(index);
                        _fieldsNamesList.Add(new EnumerationValue(displayText, value));

                        index--;

                    }
                    index++;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        void seachCntrl_RemoveConditionEvent(object sender, EventArgs<string> e)
        {
            try
            {
                if (panelConditions.Controls.ContainsKey(e.Value))
                {
                    Control cntrl = panelConditions.Controls[e.Value];
                    cntrl.Dispose();
                    panelConditions.Controls.Remove(cntrl);
                    //this.Height -= 25;
                    //this.panelConditions.Height -= 25;
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

        void seachCntrl_GetColumnTypeReq(object sender, EventArgs<string> e)
        {
            string FieldName = e.Value;
            SearchingCntrl searchCntrl = sender as SearchingCntrl;
            if (searchCntrl != null)
            {

                if (FieldName.IndexOf('.') > 0)
                {
                    String[] StringArray = FieldName.Split('.');
                    FieldName = StringArray[1];
                    if (FieldName.Contains(","))
                        FieldName = FieldName.Substring(0, FieldName.IndexOf(','));
                }

                if (_propNameTypePairs.ContainsKey(FieldName))
                {
                    String fieldType = _propNameTypePairs[FieldName];
                    ValueList dataValueList = new ValueList();
                    if (_ValueListDict.ContainsKey(FieldName))
                    {
                        dataValueList = _ValueListDict[FieldName];
                        fieldType = "Enum";
                    }
                    searchCntrl.SetConditionControls(fieldType, dataValueList);
                }
            }
            CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
        }

        /// <summary>
        /// To delete search group instance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteGroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (RemoveGroupEvent != null)
                {
                    RemoveGroupEvent(this, new EventArgs<string>(this.Name));
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

        public string GetGroupSearchCondition(int countControl)
        {
            StringBuilder groupConditionString = new StringBuilder();

            try
            {
                int conditionCount = panelConditions.Controls.Count;

                if (conditionCount > 0)
                    groupConditionString.Append(" (");

                foreach (Control cntrl in panelConditions.Controls)
                {
                    SearchingCntrl searchControls = cntrl as SearchingCntrl;
                    if (searchControls != null)
                    {
                        SearchCondition condition = searchControls.GetSearchCondition();
                        String conditionString = GetConditonString(condition, conditionCount);
                        if (!string.IsNullOrEmpty(conditionString))
                        {
                            groupConditionString.Append(conditionString);
                        }
                        else
                        {
                            groupConditionString.Clear();
                            return groupConditionString.ToString();
                        }
                    }
                    conditionCount--;
                    if (conditionCount == 0)
                        groupConditionString.Append(") ");
                }

                if (countControl > 1)
                {
                    groupConditionString.Append(cmbBoxAndOr.Value.ToString());
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
            return groupConditionString.ToString();

        }

        private String GetConditonString(SearchCondition condition, int conditionCount)
        {
            StringBuilder conditionString = new StringBuilder();

            try
            {
                if (!String.IsNullOrEmpty(condition.FieldName) && !String.IsNullOrEmpty(condition.FieldValue.ToString()))
                {
                    String fieldType = "String";

                    switch (fieldType)
                    {
                        case "Boolean":
                        case "String":
                        case "DateTime":
                            condition.FieldValue = "'" + condition.FieldValue.ToString() + "'";
                            break;


                    }

                    conditionString.Append(condition.FieldName + " " + condition.Operator + " " + condition.FieldValue);

                    if (conditionCount > 1)
                        conditionString.Append(" " + condition.AndOr + " ");
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

            return conditionString.ToString();
        }

        /// <summary>
        /// rename field name value
        /// </summary>
        /// <param name="renameColumnsDictionary"></param>
        private void RenameFieldsNamesDrpDwnValue(Dictionary<string, string> renameColumnsDictionary)
        {
            try
            {
                List<EnumerationValue> tempFieldsNames = DeepCopyHelper.Clone(_fieldsNamesList);
                int index = 0;
                foreach (EnumerationValue enumValue in tempFieldsNames)
                {
                    if (renameColumnsDictionary.Keys.Contains(enumValue.Value.ToString()))
                    {
                        String displayText = enumValue.DisplayText;
                        String value = renameColumnsDictionary[enumValue.Value.ToString()];
                        _fieldsNamesList.RemoveAt(index);
                        _fieldsNamesList.Add(new EnumerationValue(displayText, value));

                        index--;

                    }
                    index++;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// rename column name in propNameTypePairs dictionary
        /// </summary>
        /// <param name="renameColumnsDictionary"></param>
        private void RenamePropNameTypePairsDrpDwnValue(Dictionary<string, string> renameColumnsDictionary)
        {
            try
            {
                foreach (string key in renameColumnsDictionary.Keys)
                {
                    if (_propNameTypePairs.ContainsKey(key))
                    {
                        string value = _propNameTypePairs[key];
                        _propNameTypePairs.Remove(key);
                        _propNameTypePairs.Add(renameColumnsDictionary[key], value);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
    }
}
