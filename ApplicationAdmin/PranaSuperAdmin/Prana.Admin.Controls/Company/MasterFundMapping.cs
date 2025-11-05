using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinListView;
using Prana.Admin.BLL;
using Prana.AuditManager.Definitions.Constants;
using Prana.AuditManager.Definitions.Interface;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prana.Admin.Controls.Company
{
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.MasterFundCreated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.MasterFundUpdated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.MasterFundApproved, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.MasterFundDeleted, ShowAuditUI = true)]
    public partial class MasterFundMapping : UserControl, IAuditSource
    {
        /// <summary>
        /// old master Fund name needed at rename master Account 
        /// </summary>
        String _oldMasterFundName;

        /// <summary>
        /// set true or false is item in exited Edit Mode
        /// </summary>
        bool _isItemExistedEditMode = false;

        /// <summary>
        /// The is trading account initialing
        /// </summary>
        bool _isTradingAccountInitializing = false;
        /// <summary>
        /// chang master Fund in dictionary on basis of _ismasterFundId 1 for rename, 0 for add and 2 for delete 
        /// </summary>
        int _isMasterFundId = -1;

        /// <summary>
        /// create object of text box at run time
        /// </summary>
        private TextBox txtRenameAdd = new TextBox();

        /// <summary>
        /// ID of the selected company
        /// </summary>
        public int _companyID = int.MinValue;

        /// <summary>
        /// Flag variable to indicate whether the many to many mapping is allowed
        /// </summary>
        private static bool _isManyToManyMapping = false;

        /// <summary>
        /// _is Show master Fund As Client
        /// </summary>
        private static bool _isShowmasterFundAsClient = false;

        /// <summary>
        /// _is Show Master Fund on TT
        /// </summary>
        private static bool _isShowMasterFundonTT = false;



        /// <summary>
        /// initilaied masterFund mapping control
        /// </summary>
        [AuditManager.Attributes.AuditSourceConstAttri]
        public MasterFundMapping()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Applying Black Gray Theme
        /// </summary>
        public void ApplyTheme()
        {
            try
            {
                this.ultraButton1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.ultraButton2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.ultraButton3.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.ultraButton4.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
                this.ForeColor = System.Drawing.Color.White;

                this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.groupBox1.ForeColor = System.Drawing.Color.White;

                this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.groupBox2.ForeColor = System.Drawing.Color.White;

                this.groupBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.groupBox4.ForeColor = System.Drawing.Color.White;

                this.groupBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.groupBox6.ForeColor = System.Drawing.Color.White;

                this.groupBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
                this.groupBox5.ForeColor = System.Drawing.Color.White;
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
        /// initialized data at load time
        /// </summary>
        public void InitializeControl(int companyID, bool isManyToManyMapping, bool isShowmasterFundAsClient, bool isShowMasterFundonTT)
        {
            try
            {
                #region get selected item(if selected) from different lists on UI
                string selectedMasterFund = string.Empty;
                if (listMasterFund.SelectedItems.Count > 0)
                {
                    selectedMasterFund = listMasterFund.SelectedItems.First.Key;
                }

                string selectedAssigned = string.Empty;
                if (uLstViewAssignedAccounts.SelectedItems.Count > 0)
                {
                    selectedAssigned = uLstViewAssignedAccounts.SelectedItems.First.Key;
                }

                string selectedUnassigned = string.Empty;
                if (uLstViewUnAssignedAccounts.SelectedItems.Count > 0)
                {
                    selectedUnassigned = uLstViewUnAssignedAccounts.SelectedItems.First.Key;
                }
                #endregion
                _companyID = companyID;
                _isManyToManyMapping = isManyToManyMapping;
                chkBoxManyToMany.Checked = _isManyToManyMapping;

                _isShowmasterFundAsClient = isShowmasterFundAsClient;
                ChkBoxShowmasterFundAsClient.Checked = _isShowmasterFundAsClient;

                _isShowMasterFundonTT = isShowMasterFundonTT;
                chkBoxShowMasterFundonTT.Checked = _isShowMasterFundonTT;

                RefreshMasterFundAuditFor(int.MinValue);
                //initialized textbox
                InitTextBoxes();
                AccountMasterFundMappingManager.InitialiseData(companyID);
                cmbTradingAccounts.DataSource = AccountMasterFundMappingManager.GetTradingAccounts();
                cmbTradingAccounts.DropDownStyle = UltraComboStyle.DropDownList;
                cmbTradingAccounts.DisplayLayout.Bands[0].Columns["TradingAccountID"].Hidden = true;
                cmbTradingAccounts.ValueMember = "TradingAccountID";
                cmbTradingAccounts.DisplayMember = "TradingAccountName";
                cmbTradingAccounts.DisplayLayout.Bands[0].ColHeadersVisible = false;

                listMasterFund.ItemSettings.AllowEdit = DefaultableBoolean.True;
                BindMasterFund();
                //modified by: Bharat raturi, 23 apr 2014
                //purpose: Bind unmapped accounts according to the chosen mapping style
                //bind Unmapped Account to control
                //BindUnMappedAccount();
                if (chkBoxManyToMany.Checked)
                {
                    BindUnMappedAccount();
                }
                else
                {
                    BindUnMappedAccountForOneToMany();
                }
                listMasterFund.ItemSettings.HideSelection = false;
                #region retain focus on selected item in lists on UI
                if (!string.IsNullOrEmpty(selectedMasterFund) && listMasterFund.Items.Exists(selectedMasterFund))
                {
                    listMasterFund.SelectedItems.Add(listMasterFund.Items[selectedMasterFund]);
                }

                if (!string.IsNullOrEmpty(selectedAssigned) && uLstViewAssignedAccounts.Items.Exists(selectedAssigned))
                {
                    uLstViewAssignedAccounts.SelectedItems.Add(uLstViewAssignedAccounts.Items[selectedAssigned]);
                }

                if (!string.IsNullOrEmpty(selectedUnassigned) && uLstViewUnAssignedAccounts.Items.Exists(selectedUnassigned))
                {
                    uLstViewUnAssignedAccounts.SelectedItems.Add(uLstViewUnAssignedAccounts.Items[selectedUnassigned]);
                }
                #endregion
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
        /// check for unsaved data at close Form time 
        /// </summary>
        /// <param name="sender">company master Form </param>
        /// <param name="e">null</param>
        void MasterFundMapping_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AccountMasterFundMappingManager.IsBackInitialiedMasterFundCollection() || Prana.Admin.BLL.AccountMasterFundMappingManager.IsBackInitialied() || AccountMasterFundMappingManager.IsBackInitialiedTradingAccount())
            {
                DialogResult dr = MessageBox.Show("MasterFund and/or Mapping not saved. Saved unsaved changes?", "Mapper", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    AccountMasterFundMappingManager.SaveMapping(_companyID);
                }
            }
        }

        /// <summary>
        /// initialized  text boxs at run time 
        /// </summary>
        private void InitTextBoxes()
        {
            try
            {
                this.listMasterFund.Controls.Add(txtRenameAdd);
                txtRenameAdd.Visible = false;
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
        /// show all masterFunds in List box named  listMasterFund
        /// </summary>
        public void BindMasterFund()
        {
            try
            {
                listMasterFund.Items.Clear();
                List<String> getMasterFundNames = new List<string>();

                //Get all masterFund name From _masterFundCollection and add intl list then show in list box
                getMasterFundNames = AccountMasterFundMappingManager.GetAllMasterFundName().OrderBy(x => x).ToList();

                for (int i = 0; i < getMasterFundNames.Count; i++)
                {
                    UltraListViewItem item = new UltraListViewItem();
                    item.Tag = item.Key = getMasterFundNames[i];
                    //item.Key = accountName;
                    item.Value = getMasterFundNames[i];
                    listMasterFund.Items.Add(item);
                }
                // set index 0 at start level
                //if (listMasterFund.SelectedItems.Count >= 1)
                //{

                if (listMasterFund.Items.Count > 0)
                {
                    listMasterFund.SelectedItems.Add(listMasterFund.Items[0]);//[0];
                }

                //}
                //else
                //{ }
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
        /// Show all Un mapped accounts in ultra list view 
        /// </summary>
        public void BindUnMappedAccount()
        {
            try
            {
                List<String> unMappedAccountNames = new List<string>();
                if (listMasterFund.SelectedItems.Count > 0)
                {
                    unMappedAccountNames = AccountMasterFundMappingManager.GetUnmappedAccounts(listMasterFund.SelectedItems[0].Text);
                }
                else
                {
                    unMappedAccountNames = AccountMasterFundMappingManager.GetUnmappedAccounts(string.Empty);
                }
                uLstViewUnAssignedAccounts.Items.Clear();
                foreach (String accountName in unMappedAccountNames)
                {
                    UltraListViewItem item = new UltraListViewItem();
                    item.Tag = item.Key = accountName;
                    //item.Key = accountName;
                    item.Value = accountName;
                    if (!uLstViewUnAssignedAccounts.Items.Contains(item))
                    {
                        uLstViewUnAssignedAccounts.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        //added by: Bharat raturi, 23 apr 2014
        //purpose: Bind unmapped accounts when one to many mapping is activated
        /// <summary>
        /// Show all Un mapped accounts in ultra list view 
        /// </summary>
        public void BindUnMappedAccountForOneToMany()
        {
            try
            {
                List<String> unMappedAccountNames = new List<string>();
                if (listMasterFund.SelectedItems.Count > 0)
                {
                    if (chkBoxManyToMany.Checked)
                    {
                        unMappedAccountNames = AccountMasterFundMappingManager.GetUnmappedAccounts(listMasterFund.SelectedItems[0].Text);
                    }
                    else
                    {
                        unMappedAccountNames = AccountMasterFundMappingManager.GetUnmappedAccountsForOnetoMany();
                    }
                }
                else
                {
                    if (chkBoxManyToMany.Checked)
                    {
                        unMappedAccountNames = AccountMasterFundMappingManager.GetUnmappedAccounts(string.Empty);
                    }
                    else
                    {
                        unMappedAccountNames = AccountMasterFundMappingManager.GetUnmappedAccountsForOnetoMany();
                    }
                }
                uLstViewUnAssignedAccounts.Items.Clear();
                foreach (String accountName in unMappedAccountNames)
                {
                    UltraListViewItem item = new UltraListViewItem();
                    item.Tag = item.Key = accountName;
                    //item.Key = accountName;
                    item.Value = accountName;
                    if (!uLstViewUnAssignedAccounts.Items.Contains(item))
                    {
                        uLstViewUnAssignedAccounts.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Show mapped Accounts in ultra list view 
        /// </summary>
        /// <param name="accountNames">List of mappedAccount Name for A selected MAsterAccount Name </param>
        public void BindMappedAccount(List<String> accountNames)
        {
            try
            {
                if (accountNames == null)
                    uLstViewAssignedAccounts.Items.Clear();
                else
                {

                    uLstViewAssignedAccounts.Items.Clear();
                    foreach (String accountName in accountNames)
                    {
                        UltraListViewItem item = new UltraListViewItem();
                        item.Tag = item.Key = accountName;
                        //item.Key = accountName;
                        item.Value = accountName;
                        if (!uLstViewAssignedAccounts.Items.Contains(item.Key))
                        {
                            uLstViewAssignedAccounts.Items.Add(item);
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

        /// <summary>
        /// add Account names in list to be unassigned 
        /// </summary>
        /// <param name="isOnlySelectedRequired">return true of false only selected Require to unassigned  of all to be un assigned</param>
        /// <returns>List of toBeUnAssignedAccounts</returns>
        private List<String> GetToBeUnAssignedAccounts(bool isOnlySelectedRequired)
        {
            int count;
            List<string> toBeUnAssignedAccounts = new List<string>();
            try
            {

                if (isOnlySelectedRequired)
                {
                    count = uLstViewAssignedAccounts.SelectedItems.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeUnAssignedAccounts.Add(uLstViewAssignedAccounts.SelectedItems[i].Text);
                    }
                }
                else
                {
                    count = uLstViewAssignedAccounts.Items.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeUnAssignedAccounts.Add(uLstViewAssignedAccounts.Items[i].Text);
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
            return toBeUnAssignedAccounts;
        }

        /// <summary>
        /// To be unassign selected  Accounts on button click and show selected accounts in unmapped list view
        /// </summary>
        /// <param name="sender">button</param>
        /// <param name="e">null</param>
        private void UBtnUnSelectAssignedAccounts_Click(object sender, EventArgs e)
        {

            try
            {
                if (listMasterFund.SelectedItems.Count > 0)
                {
                    int index = listMasterFund.SelectedItems[0].Index;
                    if (index >= 0)
                    {
                        string masterFundName = listMasterFund.Items[index].Text;
                        List<string> unSelectAccounts = GetToBeUnAssignedAccounts(true);
                        string associatedAccountName = AccountMasterFundMappingManager.CheckFundAssociated(_companyID, masterFundName, unSelectAccounts);
                        if (associatedAccountName != null)
                        {
                            MessageBox.Show("Fund " + associatedAccountName + " is associated with some Master Fund Preference(s).\n It cannot be removed from masterfund " + masterFundName + ".", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            AccountMasterFundMappingManager.UnassignAccounts(masterFundName, unSelectAccounts);
                            List<String> accountNames = AccountMasterFundMappingManager.GetAccountNamesForMasterFund(masterFundName);
                            uLstViewAssignedAccounts.Items.Clear();
                            BindMappedAccount(accountNames);
                            //modified by: Bharat raturi, 23 apr 2014
                            //purpose: Bind unmapped accounts according to the chosen mapping style
                            //BindUnMappedAccount();
                            if (chkBoxManyToMany.Checked)
                            {
                                BindUnMappedAccount();
                            }
                            else
                            {
                                BindUnMappedAccountForOneToMany();
                            }
                            //modification finished
                            listMasterFund.SelectedItems.Add(listMasterFund.Items[index]);
                            //Contain search to assigned key in UnassignedAccounts list view
                            uTxtUnassignedAccounts_TextChanged(uTxtUnassignedAccounts, null);
                            //Contain search to assigned key in Assigned Accounts List view
                            uTxtAssignedAccounts_TextChanged(uTxtAssignedAccounts, null);
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

        /// <summary>
        /// To be unassign all  Accounts from assigned list view on button click and show all accounts in unmapped list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uBtnAllUnSelectAssignedAccounts_Click(object sender, EventArgs e)
        {
            try
            {
                if (listMasterFund.SelectedItems.Count > 0)
                {
                    int index = listMasterFund.SelectedItems[0].Index;// FocusedItem.Index;
                    if (listMasterFund.SelectedItems.Count >= 0)
                    {
                        string masterFundName = listMasterFund.SelectedItems[0].Text;
                        List<string> unSelectAccounts = GetToBeUnAssignedAccounts(false);
                        string associatedAccountName = AccountMasterFundMappingManager.CheckFundAssociated(_companyID, masterFundName, unSelectAccounts);
                        if (associatedAccountName != null)
                        {
                            MessageBox.Show("Fund " + associatedAccountName + " is associated with some Master Fund Preference(s).\n It cannot be removed from masterfund " + masterFundName + ".", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            AccountMasterFundMappingManager.UnassignAccounts(masterFundName, unSelectAccounts);
                            List<String> accountNames = AccountMasterFundMappingManager.GetAccountNamesForMasterFund(masterFundName);
                            uLstViewAssignedAccounts.Items.Clear();
                            BindMappedAccount(accountNames);
                            //modified by: Bharat raturi, 23 apr 2014
                            //purpose: Bind unmapped accounts according to the chosen mapping style
                            //BindUnMappedAccount();
                            if (chkBoxManyToMany.Checked)
                            {
                                BindUnMappedAccount();
                            }
                            else
                            {
                                BindUnMappedAccountForOneToMany();
                            }
                            //modification finished
                            //Contain search to assigned key in UnassignedAccounts list view
                            uTxtUnassignedAccounts_TextChanged(uTxtUnassignedAccounts, null);
                            //Contain search to assigned key in Assigned Accounts List view
                            uTxtAssignedAccounts_TextChanged(uTxtAssignedAccounts, null);
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

        /// <summary>
        /// add Account names in list to be assigned 
        /// </summary>
        /// <param name="isOnlySelectedRequired">return true of false only selected Require to assigned  of all to be  assigned</param>
        /// <returns>List of toBeAssignedAccounts</returns>
        private List<string> GetToBeAssignedAccounts(bool isOnlySelectedRequired)
        {
            int count;
            List<string> toBeAssignedAccounts = new List<string>();
            try
            {
                if (isOnlySelectedRequired)
                {
                    count = uLstViewUnAssignedAccounts.SelectedItems.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeAssignedAccounts.Add(uLstViewUnAssignedAccounts.SelectedItems[i].Text);
                    }
                }
                else
                {
                    count = uLstViewUnAssignedAccounts.Items.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeAssignedAccounts.Add(uLstViewUnAssignedAccounts.Items[i].Text);
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
            return toBeAssignedAccounts;
        }

        /// <summary>
        /// Slelected unassigned accounts assign in selected master Fund
        /// </summary>
        /// <param name="sender">button</param>
        /// <param name="e">null</param>
        private void uBtnSelectUnassignedAccounts_Click(object sender, EventArgs e)
        {
            try
            {
                if (listMasterFund.SelectedItems.Count > 0)
                {
                    int index = listMasterFund.SelectedItems[0].Index;
                    if (index >= 0)
                    {
                        string masterFundName = listMasterFund.Items[index].Text;
                        List<string> assignedAccounts = GetToBeAssignedAccounts(true);
                        AccountMasterFundMappingManager.AssignAccounts(masterFundName, assignedAccounts);
                        List<String> accountNames = AccountMasterFundMappingManager.GetAccountNamesForMasterFund(masterFundName);
                        uLstViewUnAssignedAccounts.Items.Clear();
                        BindMappedAccount(accountNames);
                        //modified by: Bharat raturi, 23 apr 2014
                        //purpose: Bind unmapped accounts according to the chosen mapping style
                        //BindUnMappedAccount();
                        if (chkBoxManyToMany.Checked)
                        {
                            BindUnMappedAccount();
                        }
                        else
                        {
                            BindUnMappedAccountForOneToMany();
                        }
                        //modification finished 
                        listMasterFund.SelectedItems.Add(listMasterFund.Items[index]);
                        //Contain search to assigned key in UnassignedAccounts list view
                        uTxtUnassignedAccounts_TextChanged(uTxtUnassignedAccounts, null);
                        //Contain search to assigned key in Assigned Accounts List view
                        uTxtAssignedAccounts_TextChanged(uTxtAssignedAccounts, null);
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
        /// all assigned account assign in unassinged accounts
        /// </summary>
        /// <param name="sender">button</param>
        /// <param name="e"></param>
        private void uBtnAllUnSelectUnassignedAccounts_Click(object sender, EventArgs e)
        {
            try
            {
                if (listMasterFund.SelectedItems.Count > 0)
                {
                    int index = listMasterFund.SelectedItems[0].Index;
                    if (index >= 0)
                    {
                        string masterFundName = listMasterFund.Items[index].Text;
                        List<string> assignedAccounts = GetToBeAssignedAccounts(false);
                        AccountMasterFundMappingManager.AssignAccounts(masterFundName, assignedAccounts);
                        List<String> accountNames = AccountMasterFundMappingManager.GetAccountNamesForMasterFund(masterFundName);
                        uLstViewUnAssignedAccounts.Items.Clear();
                        BindMappedAccount(accountNames);
                        //modified by: Bharat raturi, 23 apr 2014
                        //purpose: Bind unmapped accounts according to the chosen mapping style
                        //BindUnMappedAccount();
                        if (chkBoxManyToMany.Checked)
                        {
                            BindUnMappedAccount();
                        }
                        else
                        {
                            BindUnMappedAccountForOneToMany();
                        }
                        //Contain search to assigned key in UnassignedAccounts list view
                        uTxtUnassignedAccounts_TextChanged(uTxtUnassignedAccounts, null);
                        //Contain search to assigned key in Assigned Accounts List view
                        uTxtAssignedAccounts_TextChanged(uTxtAssignedAccounts, null);
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
        /// save all changes in data base and clean back up of changes
        /// </summary>
        /// <param name="sender">button save</param>
        /// <param name="e">e</param>
        public int uBtnSave_Click()
        {
            int i = 0;
            Dictionary<int, int> dictAuditStatus = new Dictionary<int, int>();
            try
            {
                if (AccountMasterFundMappingManager.isbackInitialied || (AccountMasterFundMappingManager.isbackUpInitialiedMasterFund) || (AccountMasterFundMappingManager.isbackInitialiedTradingAccount))
                {
                    AccountMasterFundMappingManager.CleanBackUp();
                    AccountMasterFundMappingManager.CleanBackUpMasterFund();
                    AccountMasterFundMappingManager.CleanBackUpTA();
                    i = AccountMasterFundMappingManager.SaveMapping(_companyID);

                    dictAuditStatus = AccountMasterFundMappingManager.GetStatusForAudit();
                    if (dictAuditStatus != null)
                    {
                        foreach (KeyValuePair<int, int> status in dictAuditStatus)
                        {
                            if (status.Value == 3)
                            {
                                AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, AuditMasterFundData(_companyID, status.Key), AuditManager.Definitions.Enum.AuditAction.MasterFundUpdated);
                                //RefreshMasterFundAuditFor(AccountMasterFundMappingManager.GetMasterFundIdByName(listMasterFund.SelectedItems[0].Text));
                            }
                            else if (status.Value == 1)
                            {
                                AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, AuditMasterFundData(_companyID, status.Key), AuditManager.Definitions.Enum.AuditAction.MasterFundCreated);
                                // RefreshMasterFundAuditFor(AccountMasterFundMappingManager.GetMasterFundIdByName(listMasterFund.SelectedItems[0].Text));
                            }
                            else if (status.Value == 2)
                            {
                                AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, AuditMasterFundData(_companyID, status.Key), AuditManager.Definitions.Enum.AuditAction.MasterFundDeleted);
                                // RefreshMasterFundAuditFor(AccountMasterFundMappingManager.GetMasterFundIdByName(listMasterFund.SelectedItems[0].Text));
                            }
                        }
                    }
                    if (i == -11)
                    {
                        MessageBox.Show("There is already a Master Fund with the same name but in Inactive State", "Prana Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
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
            return i;
        }

        /// <summary>
        /// Approve master Funds changes
        /// </summary>
        /// <param name="sender">button approve</param>
        /// <param name="e">e</param>
        public int uBtnApprove_Click()
        {
            int i = 0;
            try
            {
                if (listMasterFund.SelectedItems.Count > 0)
                {
                    if (_companyID != int.MinValue && listMasterFund.Items.Count > 0)
                    {
                        AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, AuditMasterFundData(_companyID, AccountMasterFundMappingManager.GetMasterFundIdByName(listMasterFund.SelectedItems[0].Text)), AuditManager.Definitions.Enum.AuditAction.MasterFundApproved);
                        RefreshMasterFundAuditFor(AccountMasterFundMappingManager.GetMasterFundIdByName(listMasterFund.SelectedItems[0].Text));
                    }
                }
                else
                {
                    MessageBox.Show("Please select Master fund for approval", "Master Fund Mapping", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
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
            return i;
        }

        /// <summary>
        /// Function to Get dictionary for details of Master Funds
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        private Dictionary<String, List<String>> AuditMasterFundData(int _companyID, int masterFundID)
        {
            Dictionary<String, List<String>> auditDataForMasterFund = new Dictionary<string, List<string>>();
            try
            {
                auditDataForMasterFund.Add(CustomAuditSourceConstants.AuditSourceTypeMasterFund, new List<string>());
                auditDataForMasterFund[CustomAuditSourceConstants.AuditSourceTypeMasterFund].Add(_companyID.ToString());
                //foreach (int id in AccountMasterFundMappingManager.GetMasterFundID(_companyID))
                //{
                auditDataForMasterFund[CustomAuditSourceConstants.AuditSourceTypeMasterFund].Add(masterFundID.ToString());
                //}
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

            return auditDataForMasterFund;
        }

        /// <summary>
        /// cancle all changes apply on Account or masterFund
        /// </summary>
        /// <param name="sender">button cancel</param>
        /// <param name="e">e</param>
        public bool uBtnCancel_Click()
        {
            bool r = false;
            try
            {
                // if Change is exist before savethen ask to revert the changes or not
                if ((AccountMasterFundMappingManager.isbackInitialied) || (AccountMasterFundMappingManager.isbackUpInitialiedMasterFund) || (AccountMasterFundMappingManager.isbackInitialiedTradingAccount))
                {
                    //if copyof dictionary _accountMasterFundMapping is exist then give message of want revert or not if yes delete copy of dictionay _accountMasterFundMapping()
                    // if no then action is required 
                    DialogResult result = MessageBox.Show("Do you want to save changes of Master fund mapping?", "Alert", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                    if (result == DialogResult.Cancel)
                        r = true;
                    if (result == DialogResult.Yes)
                    {
                        uBtnSave_Click();
                    }
                    else if (result == DialogResult.No)
                    {
                        AccountMasterFundMappingManager.RestoreBackUp();
                        AccountMasterFundMappingManager.RestoreBackUpMasterFund();
                        AccountMasterFundMappingManager.RestoreBackUpTA();
                        BindMasterFund();
                        //modified by: Bharat raturi, 23 apr 2014
                        //purpose: Bind unmapped accounts according to the chosen mapping style
                        //BindUnMappedAccount();
                        if (chkBoxManyToMany.Checked)
                        {
                            BindUnMappedAccount();
                        }
                        else
                        {
                            BindUnMappedAccountForOneToMany();
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
            return r;

        }

        /// <summary>
        /// contain search in masterFund list  to a typed key
        /// </summary>
        /// <param name="sender">textbox</param>
        /// <param name="e">e</param>
        private void uTxtMasterFund_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(uTxtMasterFund.Text.ToUpper()) && uTxtMasterFund.Text.ToUpper() != "SEARCH")
                    AddSearchedItemMasterFund();
                else
                {
                    listMasterFund.Items.Clear();
                    BindMasterFund();
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
        /// search items from typed key in text box and show itmes in lasterAccount Box and set sellected index 0
        /// </summary>
        public void AddSearchedItemMasterFund()
        {
            try
            {
                List<String> searchingList = AccountMasterFundMappingManager.GetAllMasterFundName();
                List<String> result = AccountMasterFundMappingManager.SerachForKeyword(uTxtMasterFund.Text, searchingList);
                listMasterFund.Items.Clear();
                uLstViewAssignedAccounts.Items.Clear();
                if (result.Count > 0)
                {
                    foreach (String foundItem in result)
                    {
                        UltraListViewItem item = new UltraListViewItem();
                        item.Tag = item.Key = foundItem;
                        item.Value = foundItem;
                        listMasterFund.Items.Add(item);
                    }
                    listMasterFund.SelectedItems.Add(listMasterFund.Items[0]);
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
        /// texbox  for contain search in unassigned account list
        /// </summary>
        /// <param name="sender">tex box</param>
        /// <param name="e"></param>
        private void uTxtUnassignedAccounts_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(uTxtUnassignedAccounts.Text.ToUpper()) && uTxtUnassignedAccounts.Text.ToUpper() != "SEARCH")
                    AddSearchedItemUnassignedAccount();
                else
                {
                    uLstViewUnAssignedAccounts.Items.Clear();
                    //modified by: Bharat raturi, 23 apr 2014
                    //purpose: Bind unmapped accounts according to the chosen mapping style
                    //BindUnMappedAccount();
                    if (chkBoxManyToMany.Checked)
                    {
                        BindUnMappedAccount();
                    }
                    else
                    {
                        BindUnMappedAccountForOneToMany();
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
        /// show only contain searched items in list
        /// </summary>
        public void AddSearchedItemUnassignedAccount()
        {
            List<String> searchingList = new List<string>();
            try
            {
                if (listMasterFund.SelectedItems.Count > 0 && !String.IsNullOrEmpty(listMasterFund.SelectedItems[0].Text.ToString()))
                {
                    searchingList = AccountMasterFundMappingManager.GetUnmappedAccounts(listMasterFund.SelectedItems[0].Text);//new List<string>();
                }
                else
                    searchingList = uLstViewUnAssignedAccounts.Items.Cast<UltraListViewItem>().Select(x => x.Text).ToList();
                List<String> result = AccountMasterFundMappingManager.SerachForKeyword(uTxtUnassignedAccounts.Text, searchingList);
                uLstViewUnAssignedAccounts.Items.Clear();
                if (result.Count > 0)
                {
                    foreach (String foundItem in result)
                    {

                        UltraListViewItem item = new UltraListViewItem(foundItem);
                        item.Key = foundItem;
                        item.Tag = foundItem;
                        item.Value = foundItem;
                        if (!uLstViewAssignedAccounts.Items.Contains(item.Key))
                        {
                            uLstViewUnAssignedAccounts.Items.Add(item);
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

        /// <summary>
        /// contain search in assigned accounts list and show searched items in assigned list view
        /// </summary>
        /// <param name="sender">textbox</param>
        /// <param name="e">e</param>
        private void uTxtAssignedAccounts_TextChanged(object sender, EventArgs e)
        {
            string masterFundName;
            try
            {
                if (!String.IsNullOrEmpty(uTxtAssignedAccounts.Text.ToUpper()) && uTxtAssignedAccounts.Text.ToUpper() != "SEARCH")
                    AddSearchedItemAssignedAccount();
                else
                {
                    uLstViewAssignedAccounts.Items.Clear();
                    if (listMasterFund.SelectedItems.Count >= 1)
                    {
                        masterFundName = listMasterFund.SelectedItems[0].Text;
                    }
                    else
                    {
                        masterFundName = null;
                    }
                    List<String> accountNames = AccountMasterFundMappingManager.GetAccountNamesForMasterFund(masterFundName);
                    if (accountNames != null)
                    {
                        foreach (String foundItem in accountNames)
                        {
                            UltraListViewItem item = new UltraListViewItem();
                            item.Key = foundItem;
                            item.Tag = foundItem;
                            item.Value = foundItem;
                            uLstViewAssignedAccounts.Items.Add(item);
                        }
                    }
                    else
                    {
                        uLstViewAssignedAccounts.Items.Clear();
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
        /// Show only Searched item in Assigned list view for a slecected masterFund name
        /// </summary>
        public void AddSearchedItemAssignedAccount()
        {
            string masterFundName;
            try
            {
                if (listMasterFund.SelectedItems.Count >= 1)
                {
                    masterFundName = listMasterFund.SelectedItems[0].Text;
                }
                else
                {
                    masterFundName = null;
                }
                List<String> searchingList = AccountMasterFundMappingManager.GetAccountNamesForMasterFund(masterFundName);
                List<String> result = AccountMasterFundMappingManager.SerachForKeyword(uTxtAssignedAccounts.Text, searchingList);
                uLstViewAssignedAccounts.Items.Clear();
                if (result.Count > 0)
                {
                    foreach (String foundItem in result)
                    {
                        UltraListViewItem item = new UltraListViewItem(foundItem);
                        item.Key = foundItem;
                        item.Tag = foundItem;
                        item.Value = foundItem;
                        uLstViewAssignedAccounts.Items.Add(item);
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
        /// contain search in Unassigned accounts list and show searched items in Unassigned list view
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">e</param>
        private void uTxtUnassignedAccounts_Click(object sender, EventArgs e)
        {
            try
            {
                if (uTxtUnassignedAccounts.Text.Trim().ToLower() == "search")
                {
                    uTxtUnassignedAccounts.SelectAll();
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
        /// event enable context menu depending upon selection if multiple selected then only  delete  enable if 1 selected then  then add rename and delete enabled 
        /// </summary>
        /// <param name="sender">list view </param>
        /// <param name="e">e</param>
        private void listMasterFund_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                UltraListViewItem itemClicked = listMasterFund.ItemFromPoint(e.X, e.Y);
                if (e.Button == MouseButtons.Right)
                {
                    if (itemClicked != null && !listMasterFund.Items[itemClicked.Key].IsSelected)
                    {
                        listMasterFund.SelectedItems.Clear();
                        listMasterFund.SelectedItems.Add(listMasterFund.Items[itemClicked.Key]);
                    }
                    if (listMasterFund.SelectedItems.Count > 1)
                    {
                        addMasterFundToolStripMenuItem.Enabled = false;
                        renameMasterFundToolStripMenuItem.Enabled = false;
                        deleteMasterFundToolStripMenuItem.Enabled = true;
                    }
                    else if (listMasterFund.SelectedItems.Count == 1)
                    {

                        addMasterFundToolStripMenuItem.Enabled = true;
                        renameMasterFundToolStripMenuItem.Enabled = true;
                        deleteMasterFundToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        addMasterFundToolStripMenuItem.Enabled = true;
                        renameMasterFundToolStripMenuItem.Enabled = false;
                        deleteMasterFundToolStripMenuItem.Enabled = false;
                    }
                }
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-9690
                else if (e.Button == MouseButtons.Left && (!_isItemExistedEditMode))
                {
                    int indexSeleted = 0;
                    if (listMasterFund.Items.Count > 0)
                    {
                        //indexSeleted = e.Y / listMasterFund.Items[0].UIElement.Rect.Height;
                        if (itemClicked != null)
                        {
                            indexSeleted = listMasterFund.Items[itemClicked.Key].Index;
                        }
                        if (indexSeleted > listMasterFund.Items.Count)
                            listMasterFund.SelectedItems.Clear();
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
        /// add new master Fund on click on add new master Account tool strip menu item 
        /// </summary>
        /// <param name="sender">menu item</param>
        /// <param name="e">e</param>
        private void addMasterFundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                String runtimeMasterFundName = AccountMasterFundMappingManager.GetRuntimeMasterFundName();
                UltraListViewItem item = new UltraListViewItem(runtimeMasterFundName);

                item.Tag = item.Key = runtimeMasterFundName;
                item.Value = runtimeMasterFundName;
                AccountMasterFundMappingManager.ManageMasterFund(0, runtimeMasterFundName);
                listMasterFund.Items.Add(item);
                listMasterFund.SelectedItems.Clear();
                listMasterFund.SelectedItems.Add(listMasterFund.Items[listMasterFund.Items.Count - 1]);
                listMasterFund.Items[runtimeMasterFundName].BeginEdit();

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
        /// rename master Fund name on click on rename master Account tool strip menu item 
        /// </summary>
        /// <param name="sender">rename menu item</param>
        /// <param name="e">e</param>
        private void renameMasterFundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listMasterFund.SelectedItems.Count == 1)
                {
                    listMasterFund.ItemSettings.AllowEdit = DefaultableBoolean.True;
                    listMasterFund.SelectedItems[0].BeginEdit();
                    _isMasterFundId = 1;
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
        /// delete Seleted master Fund on click on delete master Account tool strip master Account
        /// </summary>
        /// <param name="sender">delete menu item</param>
        /// <param name="e">e</param>
        private void deleteMasterFundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //int idx = 0;
                //modified by: Bharat Raturi, 05 may 2014
                //purpose: prevent the master strategy from getting deleted if it has assoicated strategies
                //Boolean doNotAskAgain = false;
                for (int i = 0; i <= listMasterFund.SelectedItems.Count - 1; i++)
                {
                    string name = listMasterFund.SelectedItems[i].Text;
                    if (AccountMasterFundMappingManager.CheckMasterFundAssociation(_companyID, name))
                    {
                        MessageBox.Show("MasterFund " + name + " is associated with some Master Fund Preference(s).\n It cannot be deleted", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }
                    //if (!doNotAskAgain && AccountMasterFundMappingManager.GetAccountNamesForMasterFund(name).Count > 0)
                    if (AccountMasterFundMappingManager.GetAccountNamesForMasterFund(name).Count > 0)
                    {
                        //doNotAskAgain = true;
                        //DialogResult dr = MessageBox.Show("MasterFund " + name + " has some accounts associated with it.\n Do you want to delete this master account?", "Warning", MessageBoxButtons.YesNo);
                        MessageBox.Show("MasterFund " + name + " has some accounts associated with it.\n It cannot be deleted", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //if (dr == DialogResult.No)
                        //{
                        break;
                        //}
                    }

                    if (AccountMasterFundMappingManager.GetTradingAccountForMasterFund(name) > 0)
                    {
                        //doNotAskAgain = true;
                        //DialogResult dr = MessageBox.Show("MasterFund " + name + " has some accounts associated with it.\n Do you want to delete this master account?", "Warning", MessageBoxButtons.YesNo);
                        MessageBox.Show("MasterFund " + name + " has a Trading Account associated with it.\n It cannot be deleted", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //if (dr == DialogResult.No)
                        //{
                        break;
                        //}
                    }
                    _isMasterFundId = 2;
                    AccountMasterFundMappingManager.ManageMasterFund(_isMasterFundId, null, name);
                }
                listMasterFund.Items.Clear();
                BindMasterFund();
                if (chkBoxManyToMany.Checked)
                {
                    BindUnMappedAccount();
                }
                else
                {
                    BindUnMappedAccountForOneToMany();
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

        void cmbTradingAccounts_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                UltraCombo combo = (UltraCombo)sender;
                if (!_isTradingAccountInitializing && combo != null && combo.Value != null)
                {
                    if (listMasterFund.SelectedItems.Count > 0)
                    {
                        int index = listMasterFund.SelectedItems[0].Index;
                        if (index >= 0)
                        {
                            string masterFundName = listMasterFund.Items[index].Text;
                            AccountMasterFundMappingManager.AssignTradingAccount(masterFundName, (int)combo.Value);
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

        /// <summary>
        /// bind associated account on Seleted master Master Fund
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //[AuditManager.Attributes.AuditRefreshMethAttri(AuditManager.Attributes.AuditMehodType.Arguments, 0)]
        private void listMasterFund_ItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItems.Count == 1)
                {
                    BindMappedAccount(AccountMasterFundMappingManager.GetAccountNamesForMasterFund(e.SelectedItems[0].Text));
                    int mFID = AccountMasterFundMappingManager.GetMasterFundIdByName(e.SelectedItems[0].Text);
                    // For audit trail
                    RefreshMasterFundAuditFor(mFID);
                    //BindUnMappedAccount();
                    //modified by: Bharat Raturi, Date: 03/04/2014
                    //Purpose: Load the list of unmapped accounts for the selected master funds
                    if (chkBoxManyToMany.Checked)
                    {
                        BindUnMappedAccount();
                    }
                    else
                    {
                        BindUnMappedAccountForOneToMany();
                    }
                    _isTradingAccountInitializing = true;
                    cmbTradingAccounts.Value = AccountMasterFundMappingManager.GetTradingAccountForMasterFund(mFID);
                    _isTradingAccountInitializing = false;
                }
                else
                {
                    BindMappedAccount(null);
                    BindUnMappedAccount();
                    _isTradingAccountInitializing = true;
                    cmbTradingAccounts.Value = -1;
                    _isTradingAccountInitializing = false;
                }
                if (listMasterFund.SelectedItems.Count == 1)
                {
                    uBtnAllUnSelectAssignedAccounts.Enabled = true;
                    uBtnAllUnSelectUnassignedAccounts.Enabled = true;
                    uBtnSelectUnassignedAccounts.Enabled = true;
                    UBtnUnSelectAssignedAccounts.Enabled = true;
                    cmbTradingAccounts.Enabled = true;
                }
                else
                {
                    uBtnAllUnSelectAssignedAccounts.Enabled = false;
                    uBtnAllUnSelectUnassignedAccounts.Enabled = false;
                    uBtnSelectUnassignedAccounts.Enabled = false;
                    UBtnUnSelectAssignedAccounts.Enabled = false;
                    cmbTradingAccounts.Enabled = false;
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
        /// Function to get Account ID
        /// </summary>
        /// <param name="SelectedMasterFundID"></param>
        [AuditManager.Attributes.AuditRefreshMethAttri(AuditManager.Attributes.AuditMehodType.Arguments, 0)]
        public void RefreshMasterFundAuditFor(int SelectedMasterFundID) { }

        /// <summary>
        /// add or rename or delete from dictionary and list view when edit mode disable
        /// </summary>
        /// <param name="sender">ListMasterFund</param>
        /// <param name="e">e</param>
        private void listMasterFund_ItemExitedEditMode(object sender, ItemExitedEditModeEventArgs e)
        {
            _isItemExistedEditMode = true;
            bool isExist = false;
            string newName = e.Item.Text;
            try
            {
                Regex r = new Regex("^[a-zA-Z0-9_\\s]+$");
                if (!r.IsMatch(e.Item.Text))
                {
                    BindMasterFund();
                    //modified by: Bharat raturi, 23 apr 2014
                    //purpose: Bind unmapped accounts according to the chosen mapping style
                    //BindUnMappedAccount();
                    if (chkBoxManyToMany.Checked)
                    {
                        BindUnMappedAccount();
                    }
                    else
                    {
                        BindUnMappedAccountForOneToMany();
                    }
                }
                else
                {
                    if (e.Item.Text != _oldMasterFundName && !String.IsNullOrEmpty(e.Item.Text))
                    {
                        isExist = AccountMasterFundMappingManager.IsMasterFundNameExist(e.Item.Text);
                        if (isExist)
                        {
                            MessageBox.Show("Already Exist!", "Master Fund", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            listMasterFund.SelectedItems.Clear();
                            listMasterFund.Items[_oldMasterFundName].Value = _oldMasterFundName;
                            listMasterFund.SelectedItems.Clear();
                            listMasterFund.SelectedItems.Add(listMasterFund.Items[_oldMasterFundName]);
                        }
                        else if (e.Item.Text.Length > 100)
                        {
                            MessageBox.Show("Master Fund Name can contain maximum 100 characters.", "Master Fund", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            listMasterFund.SelectedItems.Clear();
                            listMasterFund.Items[_oldMasterFundName].Value = _oldMasterFundName;
                            listMasterFund.SelectedItems.Clear();
                            listMasterFund.SelectedItems.Add(listMasterFund.Items[_oldMasterFundName]);
                        }
                        else
                        {
                            listMasterFund.Items[_oldMasterFundName].Key = e.Item.Text;
                            listMasterFund.Items[e.Item.Text].Tag = e.Item.Text;
                            listMasterFund.Items[e.Item.Text].Value = e.Item.Text;
                            AccountMasterFundMappingManager.ManageMasterFund(1, e.Item.Text, _oldMasterFundName);
                            int masterFundIDForRenamedMF = AccountMasterFundMappingManager.GetMasterFundIdByName(e.Item.Text);
                        }

                    }
                    else if (String.IsNullOrEmpty(e.Item.Text))
                    {
                        BindMasterFund();
                        //modified by: Bharat raturi, 23 apr 2014
                        //purpose: Bind unmapped accounts according to the chosen mapping style
                        //BindUnMappedAccount();
                        if (chkBoxManyToMany.Checked)
                        {
                            BindUnMappedAccount();
                        }
                        else
                        {
                            BindUnMappedAccountForOneToMany();
                        }
                    }
                    else
                    {
                        listMasterFund.SelectedItems.Clear();
                        listMasterFund.SelectedItems.Add(listMasterFund.Items[listMasterFund.Items.Count - 1]);
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
        /// get old name of master fund before changed 
        /// </summary>
        /// <param name="sender">ultralist view</param>
        /// <param name="e"></param>
        private void listMasterFund_ItemEnteringEditMode(object sender, ItemEnteringEditModeEventArgs e)
        {
            try
            {
                _oldMasterFundName = e.Item.Text;
                //Modified by: sachin mishra solution of JIRA no - CHMW-2283 date-19/jan/2015
                e.Item.SelectedAppearance.ForeColor = System.Drawing.Color.Black;
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
        /// set false allowedit when item activated
        /// </summary>
        /// <param name="sender">ultra list view </param>
        /// <param name="e">e</param>
        private void listMasterFund_ItemActivated(object sender, ItemActivatedEventArgs e)
        {
            try
            {
                listMasterFund.ItemSettings.AllowEdit = DefaultableBoolean.False;
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
        /// allow edit on double click
        /// </summary>
        /// <param name="sender">ultra list view</param>
        /// <param name="e">e</param>
        private void listMasterFund_ItemDoubleClick(object sender, ItemDoubleClickEventArgs e)
        {
            try
            {
                listMasterFund.ItemSettings.AllowEdit = DefaultableBoolean.True;
                e.Item.BeginEdit();
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

        private void chkBoxManyToMany_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxManyToMany.Checked)
            {
                _isManyToManyMapping = true;
            }
            else
            {
                _isManyToManyMapping = false;
            }
            if (!chkBoxManyToMany.Checked && AccountMasterFundMappingManager.IsManyToManyMapping())
            {
                MessageBox.Show("Some accounts are associated with more than one master funds.\nRemove that mapping in order to proceed.", "Master Account Mapping Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _isManyToManyMapping = true;
                chkBoxManyToMany.Checked = true;
            }
            else if (!chkBoxManyToMany.Checked && !AccountMasterFundMappingManager.IsManyToManyMapping())
            {
                BindUnMappedAccountForOneToMany();
                _isManyToManyMapping = false;
                return;
            }
            BindUnMappedAccount();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ChkBoxShowmasterFundAsClient_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (ChkBoxShowmasterFundAsClient.Checked)
                {
                    _isShowmasterFundAsClient = true;
                }
                else
                {
                    _isShowmasterFundAsClient = false;
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBoxShowMasterFundonTT_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (chkBoxShowMasterFundonTT.Checked)
                {
                    _isShowMasterFundonTT = true;
                }
                else
                {
                    _isShowMasterFundonTT = false;
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
        /// returns the value of variable _isManyToManyMapping
        /// </summary>
        /// <returns>Return true if the many to many mapping is allowed</returns>
        public bool GetIsManyToManymapping()
        {
            return _isManyToManyMapping;
        }

        /// <summary>
        /// returns the value of variable _isShowmasterFundAsClient
        /// </summary>
        /// <returns>Return true if the _isShow masterFund As Client is allowed</returns>
        public bool GetIsShowmasterFundAsClient()
        {
            return _isShowmasterFundAsClient;
        }

        /// <summary>
        /// returns the value of variable _isShowMasterFundonTT
        /// </summary>
        /// <returns>Return true if the  Show MasterFund onTT is allowed</returns>
        public bool GetIsShowMasterFundonTT()
        {
            return _isShowMasterFundonTT;
        }


        /// <summary>
        /// To unselect UnAssigned Master Funds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uLstViewAssignedAccounts_ItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItems.Count > 0)
                {
                    uLstViewUnAssignedAccounts.SelectedItems.Clear();
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
        /// To unselect Assigned Master Funds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uLstViewUnAssignedAccounts_ItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItems.Count > 0)
                {
                    uLstViewAssignedAccounts.SelectedItems.Clear();
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
    }
}