using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infragistics.Win;
using Prana.Allocation.Common.Enums;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.Allocation.Common.Definitions;
using Infragistics.Win.UltraWinGrid;
using Prana.AllocationNew.Allocation.BusinessObjects;
using Prana.Utilities.MiscUtilities;
using System.Collections;

namespace Prana.AllocationNew.Allocation.UI.UserControls
{
    public partial class DefaultRuleControl : UserControl
    {
        DataTable _defalutTable = new DataTable();
        public DefaultRuleControl()
        {
            InitializeComponent();
        }

        private void ultraGrdDefaultRule_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
           
                      
        }

        /// <summary>
        /// On control load event Initializes grid columns and default values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DefaultRuleControl_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeDataTable();
                ultraGrdDefaultRule.DataSource = _defalutTable;
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

				//Binding multiple selection account list
                ValueList accountValueList = new ValueList();
                accountValueList.CheckBoxStyle = CheckStyle.CheckBox;
                accountValueList.CheckBoxAlignment = ContentAlignment.MiddleLeft;
                foreach (int id in accountList.Keys)
                {
                    accountValueList.ValueListItems.Add(accountList[id]);
                }

                
                ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["AllocationBase"].ValueList = allocationBaseList;                
                ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["MatchingRule"].ValueList = matchingRuleList;
                ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["PreferenceAccount"].ValueList = preferenceAccountList;
                ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["MatchPortfolioPosition"].ValueList = matchPositionList;
                ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["ProrataAccountList"].ValueList = accountValueList;

                ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["MatchingRule"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["PreferenceAccount"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["MatchPortfolioPosition"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["AllocationBase"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["ProrataAccountList"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["DaysUpto"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.IntegerNonNegativeWithSpin;

               // ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["ProrataAccountList"].Hidden = true;
                //ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["DaysUpto"].Hidden = true;

                //TODO: Change to default values saved in DB.
                _defalutTable.Rows.Add(new Object[] { 
                         AllocationBaseType.Notional,
                         MatchingRuleType.SinceInception,
                         preferenceAccountList.ValueListItems[0],
                         true,
						"",
						0}
                       );
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
        /// Initializes Data table that is binded to grid.
        /// </summary>
        private void InitializeDataTable()
        {
            try
            {
                DataColumn baseColumn = new DataColumn();
                baseColumn.ColumnName = "AllocationBase";
                baseColumn.Caption = "Allocation Method";
                _defalutTable.Columns.Add(baseColumn);

                DataColumn matchRuleColumn = new DataColumn();
                matchRuleColumn.ColumnName = "MatchingRule";
                matchRuleColumn.Caption = "Target % as of";
                _defalutTable.Columns.Add(matchRuleColumn);

                DataColumn accountColumn = new DataColumn();
                accountColumn.ColumnName = "PreferenceAccount";
                accountColumn.Caption = "Remainder allocation to";
                _defalutTable.Columns.Add(accountColumn);

                DataColumn matchPositionColumn = new DataColumn();
                matchPositionColumn.ColumnName = "MatchPortfolioPosition";
                matchPositionColumn.Caption = "Match closing transactions exactly";
                _defalutTable.Columns.Add(matchPositionColumn);

                DataColumn accountListColumnColumn = new DataColumn();
                accountListColumnColumn.ColumnName = "ProrataAccountList";
                accountListColumnColumn.Caption = "Prorata Account List";
                _defalutTable.Columns.Add(accountListColumnColumn);

                DataColumn daysUptoColumn = new DataColumn();
                daysUptoColumn.ColumnName = "DaysUpto";
                daysUptoColumn.Caption = "Days Back";
                _defalutTable.Columns.Add(daysUptoColumn);
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
        /// updates default rule grid after open,save and other operations.
        /// </summary>
        /// <param name="allocationRule"></param>
        internal void UpdateGrid(AllocationRule allocationRule)
        {
            try
            {
                _defalutTable.Rows.Clear();
                string prefAccountId = CommonDataCache.CachedDataManager.GetInstance.GetAccountText(allocationRule.PreferenceAccountId);
                _defalutTable.Rows.Add(new Object[] { 
                        allocationRule.BaseType,
                        allocationRule.RuleType,
                        prefAccountId = (prefAccountId == "") ? "Select" : prefAccountId,//If account id is -1 then show select.
                        allocationRule.MatchPortfolioPosition,
                        ControlHelper.GetCsvForList("Account",allocationRule.ProrataAccountList),
                        allocationRule.ProrataDaysBack
                });
                //if (allocationRule.RuleType == MatchingRuleType.Prorata)
                //{
                //    ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["ProrataAccountList"].Hidden = false;
                //    ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["DaysUpto"].Hidden = false;
                //}
                //else
                //{
                //    ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["ProrataAccountList"].Hidden = true;
                //    ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["DaysUpto"].Hidden = true;
                //}
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
        /// Returns current vaule in grid for saving.
        /// </summary>
        /// <returns></returns>
        internal AllocationRule GetCurrentValues()
        {
            try
            {
                AllocationRule defaultRule = new AllocationRule();
                defaultRule.BaseType = (AllocationBaseType)Enum.Parse(typeof(AllocationBaseType), _defalutTable.Rows[0]["AllocationBase"].ToString());
                defaultRule.RuleType = (MatchingRuleType)Enum.Parse(typeof(MatchingRuleType), _defalutTable.Rows[0]["MatchingRule"].ToString());
                defaultRule.PreferenceAccountId = CommonDataCache.CachedDataManager.GetInstance.GetAccountID(_defalutTable.Rows[0]["PreferenceAccount"].ToString());
                defaultRule.PreferenceAccountId = defaultRule.PreferenceAccountId == int.MinValue ? -1 : defaultRule.PreferenceAccountId;
                defaultRule.MatchPortfolioPosition = Convert.ToBoolean(_defalutTable.Rows[0]["MatchPortfolioPosition"]);
                defaultRule.ProrataAccountList = ControlHelper.GetListId(_defalutTable.Rows[0]["ProrataAccountList"].ToString(), "Account");
                defaultRule.ProrataDaysBack = Convert.ToInt32(_defalutTable.Rows[0]["DaysUpto"].ToString());
                return defaultRule;
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
        /// Apply bulk change to selected default rule
        /// </summary>
        /// <param name="e"></param>
        internal void ApplyBulkChange(ApplyBulkChangeEventArgs e)
        {
            try
            {

                foreach (DataRow dr in _defalutTable.Rows)
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
        /// Event raised when Default rule is updated.
        /// </summary>
        public event EventHandler Event;

        /// <summary>
        /// Raises event when cell is updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrdDefaultRule_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column == ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["MatchingRule"] && (MatchingRuleType)Enum.Parse(typeof(MatchingRuleType), e.Cell.Value.ToString()) == MatchingRuleType.Prorata)
                {
                    _defalutTable.Rows[0]["AllocationBase"] = AllocationBaseType.CumQuantity;
                    ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["AllocationBase"].CellActivation = Activation.Disabled;
                }
                else if (e.Cell.Column == ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["MatchingRule"] && (MatchingRuleType)Enum.Parse(typeof(MatchingRuleType), e.Cell.Value.ToString()) != MatchingRuleType.Prorata)
                {
                    ultraGrdDefaultRule.DisplayLayout.Bands[0].Columns["AllocationBase"].CellActivation = Activation.AllowEdit;
                }
              
                if (e.Cell.Column.Key.Equals("ProrataAccountList"))
                {
                    ultraGrdDefaultRule.AfterCellUpdate -= ultraGrdDefaultRule_AfterCellUpdate;
                    e.Cell.Value = String.Empty;
                    ValueList list = (ValueList)e.Cell.Column.ValueList;
                    e.Cell.Value = ControlHelper.GetCsv(list);
                    ultraGrdDefaultRule.AfterCellUpdate += ultraGrdDefaultRule_AfterCellUpdate;
                    ultraGrdDefaultRule.PerformAction(UltraGridAction.ExitEditMode);
                }

                if (Event != null)
                    Event(this, EventArgs.Empty);
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
        /// On cell active Check selected Accounts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrdDefaultRule_AfterCellActivate(object sender, EventArgs e)
        {
            try
            {
                UltraGridCell cell = ultraGrdDefaultRule.ActiveCell;
                ValueList list = (ValueList)cell.Column.ValueList;
                if (list != null)
                {

                    List<ValueListItem> checkedList = ControlHelper.GetValueListFromCsv(cell.Text);
                    list.SetCheckState(CheckState.Unchecked);
                    if (checkedList != null && list != null)
                    {
                        foreach (ValueListItem item in checkedList)
                        {
                            ValueListItem findItem = list.FindByDataValue(item.DisplayText);
                            if (findItem != null)
                                findItem.CheckState = CheckState.Checked;
                        }
                        //list.SetCheckState(checkedList, CheckState.Checked);
                    }
                }
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
