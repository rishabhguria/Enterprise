using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

using Prana.Global;
using Prana.WCFConnectionMgr;
using Prana.Interfaces;
using Prana.BusinessObjects;
using Prana.Utilities.UIUtilities;

namespace Prana.AllocationNew.Allocation.UI.UserControls
{
    public partial class CtlTradeAttributes : UserControl
    {
        const string COL_TradeAttribute1 = "TradeAttribute1";
        const string COL_TradeAttribute2 = "TradeAttribute2";
        const string COL_TradeAttribute3 = "TradeAttribute3";
        const string COL_TradeAttribute4 = "TradeAttribute4";
        const string COL_TradeAttribute5 = "TradeAttribute5";
        const string COL_TradeAttribute6 = "TradeAttribute6";

        private UltraGrid _gridAmend = null;
        private UltraGrid _gridCommission = null;

        private int _userID = 0;

        private Dictionary<int, List<int>> _masterFundAssociation;
        private Dictionary<int, string> _accountPBDetails;

        ProxyBase<IAllocationServices> _allocationServices = null;
        public ProxyBase<IAllocationServices> AllocationServices
        {
            set
            {
                _allocationServices = value;

            }

        }

        public delegate void DisplayMessageDelegate(string msg, bool timerly);
        public DisplayMessageDelegate DisplayMessage;

        public CtlTradeAttributes()
        {
            InitializeComponent();
            if (!CustomThemeHelper.IsDesignMode() && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }

            ckbTradeAttribute1.CheckedChanged += ckbTradeAttribute_CheckedChanged;
            ckbTradeAttribute2.CheckedChanged += ckbTradeAttribute_CheckedChanged;
            ckbTradeAttribute3.CheckedChanged += ckbTradeAttribute_CheckedChanged;
            ckbTradeAttribute4.CheckedChanged += ckbTradeAttribute_CheckedChanged;
            ckbTradeAttribute5.CheckedChanged += ckbTradeAttribute_CheckedChanged;
            ckbTradeAttribute6.CheckedChanged += ckbTradeAttribute_CheckedChanged;

            ckbTradeAttribute1.Click += ckbTradeAttribute_Click;
            ckbTradeAttribute2.Click += ckbTradeAttribute_Click;
            ckbTradeAttribute3.Click += ckbTradeAttribute_Click;
            ckbTradeAttribute4.Click += ckbTradeAttribute_Click;
            ckbTradeAttribute5.Click += ckbTradeAttribute_Click;
            ckbTradeAttribute6.Click += ckbTradeAttribute_Click;

           
        }
        private void SetButtonsColor()
        {
            try
            {
                ubApply.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                ubApply.ForeColor = System.Drawing.Color.White;
                ubApply.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ubApply.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ubApply.UseAppStyling = false;
                ubApply.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

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

        private void ckbTradeAttribute_Click(object sender, EventArgs e)
        {
            chkSelectAllBulkChanges.Checked = ckbTradeAttribute1.Checked && ckbTradeAttribute2.Checked && ckbTradeAttribute3.Checked && ckbTradeAttribute4.Checked && ckbTradeAttribute5.Checked && ckbTradeAttribute6.Checked;
        }

        public void initControl(int userID)
        {
            _userID = userID;
            BindThirdParties();
            _masterFundAssociation = Prana.CommonDataCache.CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
            BindMasterFunds();
        }

        void ckbTradeAttribute_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            UltraComboEditor uce = null;
            if (cb == ckbTradeAttribute1)
                uce = cboTradeAttribute1;
            else if (cb == ckbTradeAttribute2)
                uce = cboTradeAttribute2;
            else if (cb == ckbTradeAttribute3)
                uce = cboTradeAttribute3;
            else if (cb == ckbTradeAttribute4)
                uce = cboTradeAttribute4;
            else if (cb == ckbTradeAttribute5)
                uce = cboTradeAttribute5;
            else if (cb == ckbTradeAttribute6)
                uce = cboTradeAttribute6;


            if (uce != null)
                enableEdit(cb, uce);
            uce = null;
        }

        public void bandGrid(UltraGrid gridAmend, UltraGrid gridCommission) 
        {
            _gridAmend = gridAmend;
            _gridCommission = gridCommission;

            UltraGridBand band = _gridAmend.DisplayLayout.Bands[0];
			// adding check for column count, PRANA -7941
            if (band != null && band.Columns.Count > 0)
            {
                ckbTradeAttribute1.Text = band.Columns[COL_TradeAttribute1].Header.Caption;
                ckbTradeAttribute2.Text = band.Columns[COL_TradeAttribute2].Header.Caption;
                ckbTradeAttribute3.Text = band.Columns[COL_TradeAttribute3].Header.Caption;
                ckbTradeAttribute4.Text = band.Columns[COL_TradeAttribute4].Header.Caption;
                ckbTradeAttribute5.Text = band.Columns[COL_TradeAttribute5].Header.Caption;
                ckbTradeAttribute6.Text = band.Columns[COL_TradeAttribute6].Header.Caption;

                cboTradeAttribute1.ValueList = (ValueList)band.Columns[COL_TradeAttribute1].ValueList;
                cboTradeAttribute2.ValueList = (ValueList)band.Columns[COL_TradeAttribute2].ValueList;
                cboTradeAttribute3.ValueList = (ValueList)band.Columns[COL_TradeAttribute3].ValueList;
                cboTradeAttribute4.ValueList = (ValueList)band.Columns[COL_TradeAttribute4].ValueList;
                cboTradeAttribute5.ValueList = (ValueList)band.Columns[COL_TradeAttribute5].ValueList;
                cboTradeAttribute6.ValueList = (ValueList)band.Columns[COL_TradeAttribute6].ValueList;
            }


        }


        private void clear()
        {
            ckbTradeAttribute1.Checked = false;
            ckbTradeAttribute2.Checked = false;
            ckbTradeAttribute3.Checked = false;
            ckbTradeAttribute4.Checked = false;
            ckbTradeAttribute5.Checked = false;
            ckbTradeAttribute6.Checked = false;

            cboTradeAttribute1.Text = string.Empty;
            cboTradeAttribute2.Text = string.Empty;
            cboTradeAttribute3.Text = string.Empty;
            cboTradeAttribute4.Text = string.Empty;
            cboTradeAttribute5.Text = string.Empty;
            cboTradeAttribute6.Text = string.Empty;
        }

        private void enableEdit(CheckBox cb, UltraComboEditor uce)
        {
            uce.Enabled = cb.Checked;
        }

        private int updateGridAttributes(UltraGrid grid, bool updateTaxLot)
        {
            int rtn = 0;
            UltraGridRow[] rows = grid.Rows.GetFilteredInNonGroupByRows();
            ColumnsCollection columns = grid.DisplayLayout.Bands[0].Columns;

            foreach (UltraGridRow row in rows)
            {
                if (row.Cells["checkBox"].Value.ToString().ToLower().Equals("true"))
                {
                    AllocationGroup allocatedGroup = (AllocationGroup)row.ListObject;
                    AllocationManager.GetInstance().DictUnsavedAdd(allocatedGroup.GroupID, (AllocationGroup)allocatedGroup.Clone());
                    allocatedGroup.UpdateGroupPersistenceStatus();
                    allocatedGroup.CompanyUserID = _userID;
                    allocatedGroup.IsModified = true;
                    allocatedGroup.IsAnotherTaxlotAttributesUpdated = false;

                    if (ckbTradeAttribute1.Checked)
                    {
                        row.Cells[COL_TradeAttribute1].Value = cboTradeAttribute1.Text;
                        if (updateTaxLot)
                        {
                            foreach (TaxLot taxlot in allocatedGroup.TaxLots)
                            {
                                taxlot.TradeAttribute1 = cboTradeAttribute1.Text;
                            }
                        }

                        //Updating trade attributes to order as well so that it can be maintained while grouping and ungrouping
                        if (allocatedGroup.State != PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                            allocatedGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.Updated;
                        else
                            allocatedGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;

                        if (allocatedGroup.Orders.Count == 1)
                        {
                            foreach (AllocationOrder ord in allocatedGroup.Orders)
                            {
                                ord.TradeAttribute1 = cboTradeAttribute1.Text;
                            }
                        }

                        allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute1_Changed);
                    }

                    if (ckbTradeAttribute2.Checked)
                    {
                        row.Cells[COL_TradeAttribute2].Value = cboTradeAttribute2.Text;
                        if (updateTaxLot)
                        {
                            foreach (TaxLot taxlot in allocatedGroup.TaxLots)
                            {
                                taxlot.TradeAttribute2 = cboTradeAttribute2.Text;
                            }
                        }

                        //Updating trade attributes to order as well so that it can be maintained while grouping and ungrouping
                        if (allocatedGroup.State != PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                            allocatedGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.Updated;
                        else
                            allocatedGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;

                        if (allocatedGroup.Orders.Count == 1)
                        {
                            foreach (AllocationOrder ord in allocatedGroup.Orders)
                            {
                                ord.TradeAttribute2 = cboTradeAttribute2.Text;
                            }
                        }

                        allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute2_Changed);
                    }

                    if (ckbTradeAttribute3.Checked)
                    {
                        row.Cells[COL_TradeAttribute3].Value = cboTradeAttribute3.Text;
                        if (updateTaxLot)
                        {
                            foreach (TaxLot taxlot in allocatedGroup.TaxLots)
                            {
                                taxlot.TradeAttribute3 = cboTradeAttribute3.Text;
                            }
                        }

                        //Updating trade attributes to order as well so that it can be maintained while grouping and ungrouping
                        if (allocatedGroup.State != PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                            allocatedGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.Updated;
                        else
                            allocatedGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;

                        if (allocatedGroup.Orders.Count == 1)
                        {
                            foreach (AllocationOrder ord in allocatedGroup.Orders)
                            {
                                ord.TradeAttribute3 = cboTradeAttribute3.Text;
                            }
                        }

                        allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute3_Changed);
                    }

                    if (ckbTradeAttribute4.Checked)
                    {
                        row.Cells[COL_TradeAttribute4].Value = cboTradeAttribute4.Text;
                        if (updateTaxLot)
                        {
                            foreach (TaxLot taxlot in allocatedGroup.TaxLots)
                            {
                                taxlot.TradeAttribute4 = cboTradeAttribute4.Text;
                            }
                        }

                        //Updating trade attributes to order as well so that it can be maintained while grouping and ungrouping
                        if (allocatedGroup.State != PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                            allocatedGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.Updated;
                        else
                            allocatedGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;

                        if (allocatedGroup.Orders.Count == 1)
                        {
                            foreach (AllocationOrder ord in allocatedGroup.Orders)
                            {
                                ord.TradeAttribute4 = cboTradeAttribute4.Text;
                            }
                        }

                        allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute4_Changed);
                    }

                    if (ckbTradeAttribute5.Checked)
                    {
                        row.Cells[COL_TradeAttribute5].Value = cboTradeAttribute5.Text;
                        if (updateTaxLot)
                        {
                            foreach (TaxLot taxlot in allocatedGroup.TaxLots)
                            {
                                taxlot.TradeAttribute5 = cboTradeAttribute5.Text;
                            }
                        }

                        //Updating trade attributes to order as well so that it can be maintained while grouping and ungrouping
                        if (allocatedGroup.State != PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                            allocatedGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.Updated;
                        else
                            allocatedGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;

                        if (allocatedGroup.Orders.Count == 1)
                        {
                            foreach (AllocationOrder ord in allocatedGroup.Orders)
                            {
                                ord.TradeAttribute5 = cboTradeAttribute5.Text;
                            }
                        }

                        allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute5_Changed);
                    }

                    if (ckbTradeAttribute6.Checked)
                    {
                        row.Cells[COL_TradeAttribute6].Value = cboTradeAttribute6.Text;
                        if (updateTaxLot)
                        {
                            foreach (TaxLot taxlot in allocatedGroup.TaxLots)
                            {
                                taxlot.TradeAttribute6 = cboTradeAttribute6.Text;
                            }
                        }

                        //Updating trade attributes to order as well so that it can be maintained while grouping and ungrouping
                        if (allocatedGroup.State != PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                            allocatedGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.Updated;
                        else
                            allocatedGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;

                        if (allocatedGroup.Orders.Count == 1)
                        {
                            foreach (AllocationOrder ord in allocatedGroup.Orders)
                            {
                                ord.TradeAttribute6 = cboTradeAttribute6.Text;
                            }
                        }

                        allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute6_Changed);
                    }
                    DisplayMessage("Bulk Trade Attributes changes updated for the selected record(s).", false);
                    rtn++;
                }
            }
            return rtn;
        }

        private void updateGradColumnValueLists()
        {
            ColumnsCollection columns = _gridAmend.DisplayLayout.Bands[0].Columns;

            if (ckbTradeAttribute1.Checked)
            {
                updateGradColumnValueList(columns[COL_TradeAttribute1], cboTradeAttribute1.Text);
            }

            if (ckbTradeAttribute2.Checked)
            {
                updateGradColumnValueList(columns[COL_TradeAttribute2], cboTradeAttribute2.Text);
            }

            if (ckbTradeAttribute3.Checked)
            {
                updateGradColumnValueList(columns[COL_TradeAttribute3], cboTradeAttribute3.Text);
            }

            if (ckbTradeAttribute4.Checked)
            {
                updateGradColumnValueList(columns[COL_TradeAttribute4], cboTradeAttribute4.Text);
            }

            if (ckbTradeAttribute5.Checked)
            {
                updateGradColumnValueList(columns[COL_TradeAttribute5], cboTradeAttribute5.Text);
            }

            if (ckbTradeAttribute6.Checked)
            {
                updateGradColumnValueList(columns[COL_TradeAttribute6], cboTradeAttribute6.Text);
            }
        }

        private void updateGradColumnValueList(UltraGridColumn column, string txt)
        {
            //ColumnsCollection columns = _gridAmend.DisplayLayout.Bands[0].Columns;

            BindableValueList bvl = (BindableValueList)column.ValueList;
            BindingSource bs = (BindingSource)bvl.DataSource;
            DataView dw = (DataView)bs.DataSource;
            if (dw.Find(txt) < 0)
            {
                DataRowView drw = dw.AddNew();
                drw.BeginEdit();
                drw[0] = txt;
                drw.EndEdit();
            }
        }

        private void ubApply_Click(object sender, EventArgs e)
        {
            if (!validUI())
            {
                return;
            }
            try
            {
                if (rdbtnGroup.Checked)
                {
                    int totalupdated = updateGridAttributes(_gridAmend, false) + updateGridAttributes(_gridCommission, true);
                    if (totalupdated == 0)
                    {
                        DisplayMessage("Please select atleast one record!", true);
                    }
                    else
                    {
                        DisplayMessage("Bulk Trade Attributes changes updated " + totalupdated + " record(s).", true);
                    }
                }
                else if (rdbtnTaxlot.Checked)
                {
                    List<int> accountIDs = GetSelectedAccountIDs();
                    if (accountIDs != null && accountIDs.Count > 0)
                    {
                        updateTaxLogFilterGroups(accountIDs);
                    }
                    else
                    {
                        DisplayMessage("Please select atleast one account!", true);
                    }
                }
                updateGradColumnValueLists();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void rdbtnGroup_CheckedChanged(object sender, EventArgs e)
        {
            grpTaxlot.Enabled = rdbtnTaxlot.Checked;
        }

        private void BindCombo(UltraCombo comboBox, DataTable dt, string displayMember, string valueMember)
        {
            try
            {
                comboBox.DataSource = null;
                comboBox.DataSource = dt;

                comboBox.DisplayMember = displayMember;
                comboBox.ValueMember = valueMember;

                foreach (UltraGridColumn column in comboBox.DisplayLayout.Bands[0].Columns)
                {
                    column.Hidden = true;
                    if (column.Key.Equals(displayMember))
                    {
                        column.Hidden = false;
                    }
                }
                comboBox.Text = ApplicationConstants.C_COMBO_SELECT;
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

        private void BindMasterFunds()
        {
            try
            {
                DataTable dtMasterFunds = new DataTable();
                DataColumn colMasterFundID = new DataColumn("MasterFundID");
                colMasterFundID.DataType = typeof(int);

                DataColumn colMasterFundName = new DataColumn("MasterFundName");
                colMasterFundName.DataType = typeof(string);

                dtMasterFunds.Columns.Add(colMasterFundID);
                dtMasterFunds.Columns.Add(colMasterFundName);

                DataRow row = dtMasterFunds.NewRow();
                row[colMasterFundID] = int.MinValue;
                row[colMasterFundName] = ApplicationConstants.C_COMBO_SELECT;

                dtMasterFunds.Rows.Add(row);

                Dictionary<int, string> userMasterFunds = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAllMasterFunds();
                foreach (KeyValuePair<int, string> kvp in userMasterFunds)
                {
                    DataRow dRow = dtMasterFunds.NewRow();
                    dRow[colMasterFundID] = kvp.Key;
                    dRow[colMasterFundName] = kvp.Value;
                    dtMasterFunds.Rows.Add(dRow);
                }

                BindCombo(cmbMasterFund, dtMasterFunds, "MasterFundName", "MasterFundID");
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

        private void ToggleFXPanelControlsState(bool state)
        {
            try
            {
                chkSelectAllAccounts.Enabled = state;
                cmbMasterFund.Enabled = state;
                cmbFXPrimeBroker.Enabled = state;
                chkLstPrimeBrokerAccounts.Enabled = state;
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

        private void cmbMasterFund_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbMasterFund.Value != null)
                {
                    int masterFundID = int.Parse(cmbMasterFund.Value.ToString());

                    chkLstPrimeBrokerAccounts.Items.Clear();

                    cmbFXPrimeBroker.ValueChanged -= new EventHandler(cmbFXPrimeBroker_ValueChanged);
                    cmbFXPrimeBroker.Text = ApplicationConstants.C_COMBO_SELECT;
                    cmbFXPrimeBroker.ValueChanged += new EventHandler(cmbFXPrimeBroker_ValueChanged);

                    if (!masterFundID.Equals(int.MinValue))
                    {
                        List<int> accountIDs = _masterFundAssociation[masterFundID];
                        if (accountIDs != null)
                        {
                            foreach (int accountID in accountIDs)
                            {
                                BindAccount(accountID);
                            }
                        }
                    }
                }
                if (chkLstPrimeBrokerAccounts.Items.Count > 0)
                {
                    SetAllAccountsCheckState(true);
                }
                else
                {
                    SetAllAccountsCheckState(false);
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

        private void cmbFXPrimeBroker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbMasterFund.Value != null)
                {
                    int masterFundID = int.Parse(cmbMasterFund.Value.ToString());
                    string thirdPartyName = string.Empty;

                    if (!masterFundID.Equals(int.MinValue))
                    {
                        List<int> accountIDs = _masterFundAssociation[masterFundID];
                        List<int> pbAccountIDs = GetThirdPartyName();

                        if (pbAccountIDs != null)
                        {
                            chkLstPrimeBrokerAccounts.Items.Clear();
                            foreach (int accountID in accountIDs)
                            {
                                foreach (int pbAccountID in pbAccountIDs)
                                {
                                    if (accountID.Equals(pbAccountID))
                                    {
                                        BindAccount(pbAccountID);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        chkLstPrimeBrokerAccounts.Items.Clear();
                        List<int> accountIDs = GetThirdPartyName();
                        if (accountIDs != null)
                        {
                            foreach (int accountID in accountIDs)
                            {
                                BindAccount(accountID);
                            }
                        }

                    }
                }
                if (chkLstPrimeBrokerAccounts.Items.Count > 0)
                {
                    SetAllAccountsCheckState(true);
                }
                else
                {
                    SetAllAccountsCheckState(false);
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


        private void BindAccount(int accountID)
        {
            try
            {
                string accountName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAccountText(accountID);
                chkLstPrimeBrokerAccounts.Items.Add(accountName);
                for (int j = 0; j < chkLstPrimeBrokerAccounts.Items.Count; j++)
                {
                    chkLstPrimeBrokerAccounts.SetItemCheckState(j, CheckState.Checked);
                }
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

        private void SetAllAccountsCheckState(bool check)
        {
            chkSelectAllAccounts.CheckedChanged -= new EventHandler(chkSelectAllAccounts_CheckedChanged);
            chkSelectAllAccounts.Checked = check;
            chkSelectAllAccounts.CheckedChanged += new EventHandler(chkSelectAllAccounts_CheckedChanged);
        }

        private void chkSelectAllAccounts_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkSelectAllAccounts.Checked)
                {
                    for (int j = 0; j < chkLstPrimeBrokerAccounts.Items.Count; j++)
                    {
                        chkLstPrimeBrokerAccounts.SetItemChecked(j, true);
                    }
                }
                else
                {
                    for (int j = 0; j < chkLstPrimeBrokerAccounts.Items.Count; j++)
                    {
                        chkLstPrimeBrokerAccounts.SetItemChecked(j, false);
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

        private List<int> GetThirdPartyName()
        {
            List<int> accountIDs = null;
            try
            {
                if (cmbFXPrimeBroker.Value != null)
                {
                    string thirdPartyName = cmbFXPrimeBroker.Text;
                    if (!thirdPartyName.Equals(ApplicationConstants.C_COMBO_SELECT) && !thirdPartyName.Equals(string.Empty))
                    {
                        accountIDs = GetAccountIDByPBName(thirdPartyName);
                    }
                }
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
            return accountIDs;
        }

        private List<int> GetAccountIDByPBName(string pbName)
        {
            List<int> accountIDs = new List<int>();
            try
            {
                foreach (KeyValuePair<int, string> kvp in _accountPBDetails)
                {
                    if (string.Compare(kvp.Value, pbName, true) == 0)
                    {
                        accountIDs.Add(kvp.Key);
                    }
                }
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
            return accountIDs;
        }

        private void BindThirdParties()
        {
            try
            {
                DataTable dtThirdParties = new DataTable();
                DataColumn colThirdPartyID = new DataColumn("ThirdPartyID");
                colThirdPartyID.DataType = typeof(int);

                DataColumn colThirdPartyName = new DataColumn("ThirdPartyName");
                colThirdPartyName.DataType = typeof(string);

                dtThirdParties.Columns.Add(colThirdPartyID);
                dtThirdParties.Columns.Add(colThirdPartyName);

                DataRow row = dtThirdParties.NewRow();
                row[colThirdPartyID] = int.MinValue;
                row[colThirdPartyName] = ApplicationConstants.C_COMBO_SELECT;

                dtThirdParties.Rows.Add(row);

                int companyID = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCompanyID();

                _accountPBDetails = _allocationServices.InnerChannel.GetAccountPBDetails();
                Dictionary<int, string> thirdPartyDetails = _allocationServices.InnerChannel.GetThirdPartyDetails(companyID);

                foreach (KeyValuePair<int, string> kvp in thirdPartyDetails)
                {
                    DataRow dRow = dtThirdParties.NewRow();
                    dRow[colThirdPartyID] = kvp.Key;
                    dRow[colThirdPartyName] = kvp.Value;
                    dtThirdParties.Rows.Add(dRow);
                }

                BindCombo(cmbFXPrimeBroker, dtThirdParties, "ThirdPartyName", "ThirdPartyID");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkLstPrimeBrokerAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkLstPrimeBrokerAccounts.CheckedItems.Count < chkLstPrimeBrokerAccounts.Items.Count)
                {
                    SetAllAccountsCheckState(false);
                }
                else if (chkLstPrimeBrokerAccounts.CheckedItems.Count.Equals(chkLstPrimeBrokerAccounts.Items.Count))
                {
                    SetAllAccountsCheckState(true);
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


        private List<int> GetSelectedAccountIDs()
        {
            List<int> accountIDs = new List<int>();
            try
            {
                foreach (Object item in chkLstPrimeBrokerAccounts.CheckedItems)
                {
                    int accountID = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAccountID(item.ToString());
                    accountIDs.Add(accountID);
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
            return accountIDs;
        }

        private bool gridRowsSelected(UltraGrid grid)
        {
            foreach (UltraGridRow row in grid.Rows.GetFilteredInNonGroupByRows())
            {
                if (row.Cells["checkBox"].Value.ToString().ToLower().Equals("true"))
                {
                    return true;
                }
            }
            return false;
        }

        public void updateUI()
        {
            _gridAmend.UpdateData();
            if (gridRowsSelected(_gridAmend))
            {
                rdbtnGroup.Checked = true;
                rdbtnTaxlot.Enabled = false;
            }
            else
            {
                rdbtnTaxlot.Enabled = true;
            }
        }

        private void chkSelectAllBulkChanges_Click(object sender, EventArgs e)
        {
            bool allChecked = chkSelectAllBulkChanges.Checked;
            ckbTradeAttribute1.Checked = allChecked;
            ckbTradeAttribute2.Checked = allChecked;
            ckbTradeAttribute3.Checked = allChecked;
            ckbTradeAttribute4.Checked = allChecked;
            ckbTradeAttribute5.Checked = allChecked;
            ckbTradeAttribute6.Checked = allChecked;
        }

        private void updateTaxLogFilterGroups(List<int> accountIDs)
        {
            UltraGridRow[] filteredRows = _gridCommission.Rows.GetFilteredInNonGroupByRows();
            ColumnsCollection columns = _gridCommission.DisplayLayout.Bands[0].Columns;
            if (filteredRows.Length > 0)
            {
                int totalChanged = 0;
                foreach (UltraGridRow existingRow in filteredRows)
                {
                    if (existingRow.Cells["checkBox"].Value.ToString().ToLower().Equals("true"))
                    {
                        bool groupUpdated = false;
                        AllocationGroup allocatedGroup = (AllocationGroup)existingRow.ListObject;
                        foreach (TaxLot taxlot in allocatedGroup.TaxLots)
                        {
                            if (accountIDs.Contains(taxlot.Level1ID))
                            {
                                AllocationManager.GetInstance().DictUnsavedAdd(allocatedGroup.GroupID, (AllocationGroup)allocatedGroup.Clone());
                                allocatedGroup.UpdateGroupPersistenceStatus();
                                allocatedGroup.CompanyUserID = _userID;
                                allocatedGroup.IsModified = true;
                                allocatedGroup.IsAnotherTaxlotAttributesUpdated = false;
                                if (ckbTradeAttribute1.Checked)
                                {
                                    existingRow.Cells[columns[COL_TradeAttribute1]].Value = cboTradeAttribute1.Text;
                                    taxlot.TradeAttribute1 = cboTradeAttribute1.Text;

                                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute1_Changed);

                                }
                                if (ckbTradeAttribute2.Checked)
                                {
                                    existingRow.Cells[columns[COL_TradeAttribute2]].Value = cboTradeAttribute2.Text;
                                    taxlot.TradeAttribute2 = cboTradeAttribute2.Text;
                                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute2_Changed);
                                }
                                if (ckbTradeAttribute3.Checked)
                                {
                                    existingRow.Cells[columns[COL_TradeAttribute3]].Value = cboTradeAttribute3.Text;
                                    taxlot.TradeAttribute3 = cboTradeAttribute3.Text;
                                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute4_Changed);
                                }
                                if (ckbTradeAttribute4.Checked)
                                {
                                    existingRow.Cells[columns[COL_TradeAttribute4]].Value = cboTradeAttribute4.Text;
                                    taxlot.TradeAttribute4 = cboTradeAttribute4.Text;
                                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute4_Changed);
                                }
                                if (ckbTradeAttribute5.Checked)
                                {
                                    existingRow.Cells[columns[COL_TradeAttribute5]].Value = cboTradeAttribute5.Text;
                                    taxlot.TradeAttribute5 = cboTradeAttribute5.Text;
                                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute5_Changed);
                                }
                                if (ckbTradeAttribute6.Checked)
                                {
                                    existingRow.Cells[columns[COL_TradeAttribute6]].Value = cboTradeAttribute6.Text;
                                    taxlot.TradeAttribute6 = cboTradeAttribute6.Text;
                                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute6_Changed);
                                }
                                existingRow.Update();
                                totalChanged++;
                                groupUpdated = true;
                                //break;
                            }
                        }
                        if (groupUpdated)
                        {
                            DisplayMessage("Bulk changes updated for the selected record(s).", false);
                        }
                        else
                        {
                            DisplayMessage("No valid record(s) for the required change(s).", false);
                        }
                    }
                }
                DisplayMessage("Bulk Trade Attribute changes updated " + totalChanged + " record(s).", true);
            }
            else
            {
                DisplayMessage("Please select atleast one record!", true);
            }
        }

        private bool validUI()
        {
            statusProvider.Clear();
            if (!ckbTradeAttribute1.Checked
                && !ckbTradeAttribute2.Checked
                && !ckbTradeAttribute3.Checked
                && !ckbTradeAttribute4.Checked
                && !ckbTradeAttribute5.Checked
                && !ckbTradeAttribute6.Checked)
            {
               if (DisplayMessage != null)
                {
                    DisplayMessage("Please select atleast one value!", true);
                }
               return false;
            }
            return true;
        }
    }
} 
