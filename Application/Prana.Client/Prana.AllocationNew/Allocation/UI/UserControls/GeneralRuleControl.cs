using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Prana.Allocation.Common.Definitions;
using Infragistics.Win.UltraWinGrid;
using Prana.Allocation.Common.Enums;
using Infragistics.Win;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.BusinessObjects;
using Prana.AllocationNew.Allocation.BusinessObjects;
using Infragistics.Win.UltraWinMaskedEdit;
using Prana.Utilities.MiscUtilities;

namespace Prana.AllocationNew.Allocation.UI.UserControls
{
    public partial class GeneralRuleControl : UserControl
    {
        //List<AllocationOperationPreference> _list = new List<AllocationOperationPreference>();
        DataTable generalTable = new DataTable();
        /// <summary>
        /// Rule for adding new row.
        /// </summary>
        AllocationRule _defaultRule = null;

        public GeneralRuleControl()
        {
            InitializeComponent();
            // ultraGrdGnrlRule.DataSource = _list;
        }

        private void ultraGrdGnrlRule_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {


        }


        /// <summary>
        /// Initializes General rule Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GeneralRuleControl_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeDataTable();

                ultraGrdGnrlRule.DataSource = generalTable;

                Dictionary<int, string> exchangeList = CommonDataCache.CachedDataManager.GetInstance.GetAllExchanges();
                ValueList exchangeValueList = new ValueList();

                foreach (int id in exchangeList.Keys)
                {
                    exchangeValueList.CheckBoxStyle = CheckStyle.CheckBox;
                    exchangeValueList.CheckBoxAlignment = ContentAlignment.MiddleLeft;
                    exchangeValueList.ValueListItems.Add(exchangeList[id]);
                }


                Dictionary<int, string> assetList = CommonDataCache.CachedDataManager.GetInstance.GetAllAssets();
                ValueList assetValueList = new ValueList();

                foreach (int id in assetList.Keys)
                {
                    assetValueList.CheckBoxStyle = CheckStyle.CheckBox;
                    assetValueList.CheckBoxAlignment = ContentAlignment.MiddleLeft;
                    assetValueList.ValueListItems.Add(assetList[id]);
                }

                ValueList customOperatorList = new ValueList();
                foreach (CustomOperator op in Enum.GetValues(typeof(CustomOperator)))
                {
                    customOperatorList.ValueListItems.Add(op.ToString());
                }

                ValueList allocationBaseList = new ValueList();
                foreach (AllocationBaseType op in Enum.GetValues(typeof(AllocationBaseType)))
                {
                    allocationBaseList.ValueListItems.Add(op.ToString(), EnumHelper.GetDescription(op));
                }

                ValueList matchingRuleList = new ValueList();
                foreach (MatchingRuleType op in Enum.GetValues(typeof(MatchingRuleType)))
                {
                    matchingRuleList.ValueListItems.Add(op.ToString(), EnumHelper.GetDescription(op));
                }

                ValueList preferenceAccountList = new ValueList();
                Dictionary<int, string> accountList = CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict();
                preferenceAccountList.ValueListItems.Add("Select");
                foreach (int id in accountList.Keys)
                {
                    preferenceAccountList.ValueListItems.Add(accountList[id]);                    
                }

                ValueList matchPositionList = new ValueList();
                foreach (DefaultableBoolean op in Enum.GetValues(typeof(DefaultableBoolean)))
                {
                    if (op != DefaultableBoolean.Default)
                        matchPositionList.ValueListItems.Add(op.ToString());
                }

                ValueList accountValueList = new ValueList();
                accountValueList.CheckBoxStyle = CheckStyle.CheckBox;
                accountValueList.CheckBoxAlignment = ContentAlignment.MiddleLeft;
                foreach (int id in accountList.Keys)
                {
                    accountValueList.ValueListItems.Add(accountList[id]);
                }

                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["ExchangeSelector"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["AssetSelector"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["PRSelector"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["AllocationBase"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["MatchingRule"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["PreferenceAccount"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["MatchPortfolioPosition"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["CheckListId"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;

                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["ExchangeSelector"].ValueList = customOperatorList;
                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["AssetSelector"].ValueList = customOperatorList;
                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["PRSelector"].ValueList = customOperatorList;
                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["AllocationBase"].ValueList = allocationBaseList;
                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["MatchingRule"].ValueList = matchingRuleList;
                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["PreferenceAccount"].ValueList = preferenceAccountList;
                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["MatchPortfolioPosition"].ValueList = matchPositionList;
                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["CheckListId"].Hidden = true;


                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["ExchangeList"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["ExchangeList"].ValueList = exchangeValueList;

                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["AssetList"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["AssetList"].ValueList = assetValueList;

                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["PRList"].CharacterCasing = CharacterCasing.Upper;

                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["ProrataAccountList"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["DaysUpto"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.IntegerNonNegativeWithSpin;

                //ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["ProrataAccountList"].Hidden = true;
                //ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["DaysUpto"].Hidden = true;
                ultraGrdGnrlRule.DisplayLayout.Bands[0].Columns["ProrataAccountList"].ValueList = accountValueList;

                //AddRowsToGrid();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Initializes data table columns that is binded to Grid.
        /// </summary>
        private void InitializeDataTable()
        {
            try
            {
                DataColumn checkListColumn = new DataColumn();
                checkListColumn.ColumnName = "CheckListId";
                checkListColumn.DataType = typeof(int);
                generalTable.Columns.Add(checkListColumn);

                generalTable.Columns.Add("ExchangeSelector");
                generalTable.Columns.Add("ExchangeList");
                generalTable.Columns.Add("AssetSelector");
                generalTable.Columns.Add("AssetList");
                generalTable.Columns.Add("PRSelector");
                generalTable.Columns.Add("PRList");

                DataColumn baseColumn = new DataColumn();
                baseColumn.ColumnName = "AllocationBase";
                baseColumn.Caption = "Allocation Method";
                generalTable.Columns.Add(baseColumn);

                DataColumn matchRuleColumn = new DataColumn();
                matchRuleColumn.ColumnName = "MatchingRule";
                matchRuleColumn.Caption = "Target % as of";
                generalTable.Columns.Add(matchRuleColumn);

                DataColumn accountColumn = new DataColumn();
                accountColumn.ColumnName = "PreferenceAccount";
                accountColumn.Caption = "Remainder allocation to";
                generalTable.Columns.Add(accountColumn);

                DataColumn matchPositionColumn = new DataColumn();
                matchPositionColumn.ColumnName = "MatchPortfolioPosition";
                matchPositionColumn.Caption = "Match closing transactions exactly";
                generalTable.Columns.Add(matchPositionColumn);

                DataColumn accountListColumnColumn = new DataColumn();
                accountListColumnColumn.ColumnName = "ProrataAccountList";
                accountListColumnColumn.Caption = "Prorata Account List";
                generalTable.Columns.Add(accountListColumnColumn);

                DataColumn daysUptoColumn = new DataColumn();
                daysUptoColumn.ColumnName = "DaysUpto";
                daysUptoColumn.Caption = "Days Back";
                generalTable.Columns.Add(daysUptoColumn);

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }


        }

        /// <summary>
        /// Adds row to grid from List of AllocationOperationPreference
        /// </summary>
        internal void AddRowsToGrid(SerializableDictionary<int, CheckListWisePreference> serializableDictionary)
        {

            try
            {
                //List<AllocationOperationPreference> list = new List<AllocationOperationPreference>();
                generalTable.Rows.Clear();
                foreach (int id in serializableDictionary.Keys)
                {
                    string prefAccountId = CommonDataCache.CachedDataManager.GetInstance.GetAccountText(serializableDictionary[id].Rule.PreferenceAccountId);
                    generalTable.Rows.Add(new Object[] { 
                         serializableDictionary[id].ChecklistId,
                         serializableDictionary[id].ExchangeOperator,
                         ControlHelper.GetCsvForList("Exchange",serializableDictionary[id].ExchangeList),                         
                         serializableDictionary[id].AssetOperator,
                         ControlHelper.GetCsvForList("Asset",serializableDictionary[id].AssetList),
                         serializableDictionary[id].PROperator,
                         ControlHelper.GetCsvForList("PR",null,serializableDictionary[id].PRList),
                         serializableDictionary[id].Rule.BaseType,
                         serializableDictionary[id].Rule.RuleType,
                         prefAccountId = (prefAccountId == "") ? "Select" : prefAccountId,
                         serializableDictionary[id].Rule.MatchPortfolioPosition,
                         ControlHelper.GetCsvForList("Account",serializableDictionary[id].Rule.ProrataAccountList),
                         serializableDictionary[id].Rule.ProrataDaysBack
                    }
                    );                    

                    // Disable list for Selector field if "All" is selected

                    DataRow dr = generalTable.Select("CheckListId = " + id)[0];
                    SetEnableStatusForCell(generalTable.Rows.IndexOf(dr), "ExchangeList", IsCustomOperatorMatch((CustomOperator)Enum.Parse(typeof(CustomOperator), serializableDictionary[id].ExchangeOperator.ToString())));
                    SetEnableStatusForCell(generalTable.Rows.IndexOf(dr), "AssetList", IsCustomOperatorMatch((CustomOperator)Enum.Parse(typeof(CustomOperator), serializableDictionary[id].AssetOperator.ToString())));
                    SetEnableStatusForCell(generalTable.Rows.IndexOf(dr), "PRList", IsCustomOperatorMatch((CustomOperator)Enum.Parse(typeof(CustomOperator), serializableDictionary[id].PROperator.ToString())));
                }
            }

            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns Current rows in grid.
        /// </summary>
        /// <returns></returns>
        internal SerializableDictionary<int, CheckListWisePreference> GetCurrentCheckListPref()
        {
            try
            {
                SerializableDictionary<int, CheckListWisePreference> dict = new SerializableDictionary<int, CheckListWisePreference>();
                foreach (DataRow dr in generalTable.Rows)
                {
                    //if (Convert.ToInt32(dr["CheckListId"]) == id)
                    //{
                    int prefAccountId = CommonDataCache.CachedDataManager.GetInstance.GetAccountID(dr["PreferenceAccount"].ToString());
                    int prorataDaysBack = Convert.ToInt32(dr["DaysUpto"]);
                    List<int> prorataAccountList = ControlHelper.GetListId(dr["ProrataAccountList"].ToString(), "Account");
                    AllocationBaseType baseType = (AllocationBaseType)Enum.Parse(typeof(AllocationBaseType), dr["AllocationBase"].ToString());
                    MatchingRuleType ruleType = (MatchingRuleType)Enum.Parse(typeof(MatchingRuleType), dr["MatchingRule"].ToString());
                    CheckListWisePreference generalRule = new CheckListWisePreference(Convert.ToInt32(dr["CheckListId"]), baseType, ruleType, prefAccountId, Convert.ToBoolean(dr["MatchPortfolioPosition"]), prorataAccountList, prorataDaysBack);
                    generalRule.Rule.PreferenceAccountId = generalRule.Rule.PreferenceAccountId == int.MinValue ? -1 : generalRule.Rule.PreferenceAccountId;//IF select account is not present then sets -1 as account Id
                    CustomOperator exchangeSelector = (CustomOperator)Enum.Parse(typeof(CustomOperator), dr["ExchangeSelector"].ToString());
                    CustomOperator assetSelector = (CustomOperator)Enum.Parse(typeof(CustomOperator), dr["AssetSelector"].ToString());
                    CustomOperator prSelector = (CustomOperator)Enum.Parse(typeof(CustomOperator), dr["PRSelector"].ToString());

                    // Returned error message when selector is not all and corresponding list is empty
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-6808
                    string errorMsg1 = generalRule.TryUpdateExchangeCheck(exchangeSelector, ControlHelper.GetListId(dr["ExchangeList"].ToString(), "Exchange"));
                    string errorMsg2 = generalRule.TryUpdateAssetCheck(assetSelector, ControlHelper.GetListId(dr["AssetList"].ToString(), "Asset"));
                    string errorMsg3 = generalRule.TryUpdatePRCheck(prSelector, ControlHelper.GetStringList(dr["PRList"].ToString()));

                    if (!string.IsNullOrEmpty(errorMsg1) || !string.IsNullOrEmpty(errorMsg2) || !string.IsNullOrEmpty(errorMsg3))
                    {
                        string list = String.IsNullOrWhiteSpace(errorMsg1) ? "" : errorMsg1;
                        list = String.IsNullOrWhiteSpace(errorMsg2) ? list : String.IsNullOrWhiteSpace(list) ? errorMsg2 : list + ", " + errorMsg2;
                        list = String.IsNullOrWhiteSpace(errorMsg3) ? list : String.IsNullOrWhiteSpace(list) ? errorMsg3 : list + ", " + errorMsg3;
                        string message = "When operator is not All, there should be some values in " + list + " list";
                        MessageBox.Show(this, message, "Nirvana Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        dict.Add(Convert.ToInt32(dr["CheckListId"]), generalRule);
                    
                    //}
                }
                return dict;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        

        /// <summary>
        /// Sets context menu on mouse uo event.
        /// If row selected then show delete
        /// else add
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrdGnrlRule_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                UIElement element = ultraGrdGnrlRule.DisplayLayout.UIElement.ElementFromPoint(e.Location);
                if (element != null)
                {
                    UltraGridRow row = element.GetContext(typeof(UltraGridRow)) as UltraGridRow;

                    if (row == null && e.Button == MouseButtons.Right)
                    {
                        cntxtMenuGrid.Visible = true;
                        addRowToolStripMenuItem.Visible = true;
                        deleteRowToolStripMenuItem.Visible = false;
                        ultraGrdGnrlRule.PerformAction(UltraGridAction.ExitEditMode);
                    }
                    else if (row != null && e.Button == MouseButtons.Right)
                    {
                        row.Selected = true;
                        addRowToolStripMenuItem.Visible = false;
                        deleteRowToolStripMenuItem.Visible = true;
                        cntxtMenuGrid.Visible = true;
                    }
                    else if (row != null && e.Button == MouseButtons.Left)
                    {
                        row.Selected = true;
                    }
                    else if (row == null && e.Button == MouseButtons.Left)
                    {
                        ultraGrdGnrlRule.PerformAction(UltraGridAction.ExitEditMode);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        Random _num = new Random(1000);

        /// <summary>
        /// If e.item is add then adds new row
        /// if delete then deletes row with message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cntxtMenuGrid_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            try
            {
                if (e.ClickedItem.Tag.ToString().Equals("AddRow"))
                {
                    string prefAccountId = CommonDataCache.CachedDataManager.GetInstance.GetAccountText(this._defaultRule.PreferenceAccountId);
                    int newID = _num.Next() * (-1);
                    generalTable.Rows.Add(new Object[] { 
                         newID,
                         CustomOperator.All,
                         "",                         
                         CustomOperator.All,
                         "",
                         CustomOperator.All,
                         "",
                         this._defaultRule.BaseType,
                         this._defaultRule.RuleType,
                         (prefAccountId == "") ? "Select" : prefAccountId,
                         this._defaultRule.MatchPortfolioPosition,
                         ControlHelper.GetCsvForList("Account", this._defaultRule.ProrataAccountList),
                         this._defaultRule.ProrataDaysBack
                    });

                    // Added code to diable field next to Selector field if "All" is selected

                    DataRow dr = generalTable.Select("CheckListId = " + newID)[0];
                    SetEnableStatusForCell(generalTable.Rows.IndexOf(dr), "ExchangeList", IsCustomOperatorMatch((CustomOperator)Enum.Parse(typeof(CustomOperator), dr["ExchangeSelector"].ToString())));
                    SetEnableStatusForCell(generalTable.Rows.IndexOf(dr), "AssetList", IsCustomOperatorMatch((CustomOperator)Enum.Parse(typeof(CustomOperator), dr["AssetSelector"].ToString())));
                    SetEnableStatusForCell(generalTable.Rows.IndexOf(dr), "PRList", IsCustomOperatorMatch((CustomOperator)Enum.Parse(typeof(CustomOperator), dr["PRSelector"].ToString())));
                }
                else if (e.ClickedItem.Tag.ToString().Equals("DeleteRow"))
                {
                    ultraGrdGnrlRule.DeleteSelectedRows(true);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Apply bulk change operation on grid.
        /// </summary>
        /// <param name="e"></param>
        internal void ApplyBulkChanges(ApplyBulkChangeEventArgs e)
        {
            try
            {
                foreach (DataRow dr in generalTable.Rows)
                {
                    string prefAccountId = CommonDataCache.CachedDataManager.GetInstance.GetAccountText(e.Rule.PreferenceAccountId);
                    if (e.allocationBaseChecked)
                        dr["AllocationBase"] = e.Rule.BaseType;
                    if (e.matchingRuleChecked)
                        dr["MatchingRule"] = e.Rule.RuleType;
                    if (e.preferencedAccountChecked)
                        dr["PreferenceAccount"] = (prefAccountId == "") ? "Select" : prefAccountId;
                    if (e.matchPortfolioPostionChecked)
                        dr["MatchPortfolioPosition"] = e.Rule.MatchPortfolioPosition;

                    dr["ProrataAccountList"] = ControlHelper.GetCsvForList("Account", e.Rule.ProrataAccountList);
                    dr["DaysUpto"] = e.Rule.ProrataDaysBack;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Shows Checked items in csv form in cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrdGnrlRule_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                string key = e.Cell.Column.Key;

                // Disable list for Selector field if "All" is selected
                int rowIndex = ultraGrdGnrlRule.ActiveRow.Index;
                if (key.Equals("ExchangeSelector"))
                {
                    SetEnableStatusForCell(rowIndex, "ExchangeList", IsCustomOperatorMatch((CustomOperator)Enum.Parse(typeof(CustomOperator), e.Cell.Value.ToString())));
                    if (!IsCustomOperatorMatch((CustomOperator)Enum.Parse(typeof(CustomOperator), e.Cell.Value.ToString())))
                        ResetCheckedList(rowIndex, "ExchangeList");
                }              
                if (key.Equals("AssetSelector"))
                {
                    SetEnableStatusForCell(rowIndex, "AssetList", IsCustomOperatorMatch((CustomOperator)Enum.Parse(typeof(CustomOperator), e.Cell.Value.ToString())));
                    if (!IsCustomOperatorMatch((CustomOperator)Enum.Parse(typeof(CustomOperator), e.Cell.Value.ToString())))
                        ResetCheckedList(rowIndex, "AssetList");
                }
                if (key.Equals("PRSelector"))
                {
                    SetEnableStatusForCell(rowIndex, "PRList", IsCustomOperatorMatch((CustomOperator)Enum.Parse(typeof(CustomOperator), e.Cell.Value.ToString())));
                    if (!IsCustomOperatorMatch((CustomOperator)Enum.Parse(typeof(CustomOperator), e.Cell.Value.ToString())))
                        ultraGrdGnrlRule.Rows[rowIndex].Cells["PRList"].Value = String.Empty;
                }
      
                if (key.Equals("ExchangeList") || key.Equals("AssetList") || key.Equals("ProrataAccountList"))
                {
                    ultraGrdGnrlRule.AfterCellUpdate -= ultraGrdGnrlRule_AfterCellUpdate;
                    e.Cell.Value = String.Empty;
                    ValueList list = (ValueList)e.Cell.Column.ValueList;
                    e.Cell.Value = ControlHelper.GetCsv(list);
                    ultraGrdGnrlRule.AfterCellUpdate += ultraGrdGnrlRule_AfterCellUpdate;
                    ultraGrdGnrlRule.PerformAction(UltraGridAction.ExitEditMode);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Checks item in csv in value list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrdGnrlRule_AfterCellActivate(object sender, EventArgs e)
        {
            try
            {

                UltraGridCell cell = ultraGrdGnrlRule.ActiveCell;
                ValueList list = (ValueList)cell.Column.ValueList;
                if (list != null)
                {

                    List<ValueListItem> checkedList = ControlHelper.GetValueListFromCsv(cell.Text);
                    list.SetCheckState(CheckState.Unchecked);
                    if (checkedList != null && list != null)
                    {
                        foreach (ValueListItem item in checkedList)
                        {
                            ValueListItem findItem=list.FindByDataValue(item.DisplayText);
                            if(findItem!=null)
                                findItem.CheckState = CheckState.Checked;
                        }
                        //list.SetCheckState(checkedList, CheckState.Checked);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

       

        /// <summary>
        /// Sets value for default rule
        /// </summary>
        /// <param name="allocationRule"></param>
        internal void UpdateDefaultRule(AllocationRule allocationRule)
        {
            try
            {
                this._defaultRule = allocationRule.Clone();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns bit status for matching operator
        /// </summary>
        /// <param name="operators"></param>
        /// <returns></returns>
        private bool IsCustomOperatorMatch(CustomOperator operators)
        {
            try
            {
                return operators == CustomOperator.All ? false : true;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Enable/Disable cell of a particular row
        /// </summary>
        /// <param name="rowIndex">Index of row</param>
        /// <param name="columnName">Name of the Column</param>
        /// <param name="value">Cell is enabled/diabled based on this value</param>
        private void SetEnableStatusForCell(int rowIndex, string columnName, bool value)
        {
            try
            {
                ultraGrdGnrlRule.Rows[rowIndex].Cells[columnName].Activation = value ? Activation.AllowEdit : Activation.NoEdit;
                // changed the cell color if the cell is disabled, PRANA-6770
                if (value)
                   ultraGrdGnrlRule.Rows[rowIndex].Cells[columnName].Appearance.BackColor = Color.White;
                else
                    ultraGrdGnrlRule.Rows[rowIndex].Cells[columnName].Appearance.BackColor = Color.LightGray;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Reset the valuelist
        /// </summary>
        /// <param name="rowIndex">Index of row</param>
        /// <param name="columnName">Name of the Column</param>
        private void ResetCheckedList(int rowIndex, string columnName)
        {
            try
            {
                ultraGrdGnrlRule.AfterCellUpdate -= ultraGrdGnrlRule_AfterCellUpdate;
                ValueList list = (ValueList)ultraGrdGnrlRule.Rows[rowIndex].Cells[columnName].ValueListResolved;
                list.SetCheckState(list.CheckedItems.ToList(), CheckState.Unchecked);
                ultraGrdGnrlRule.Rows[rowIndex].Cells[columnName].Value = ControlHelper.GetCsv(list);
                ultraGrdGnrlRule.AfterCellUpdate += ultraGrdGnrlRule_AfterCellUpdate;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
