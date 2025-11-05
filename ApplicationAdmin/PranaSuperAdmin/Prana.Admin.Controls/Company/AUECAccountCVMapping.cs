using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinListView;
using Prana.Admin.BLL;
using Prana.AuditManager.Definitions.Interface;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prana.Admin.Controls.Company
{

    public partial class AUECAccountCVMapping : UserControl, IAuditSource
    {
        /// <summary>
        /// old master Fund name needed at rename master Account 
        /// </summary>
        String _oldMasterFundName;

        bool isMappingModified = false;

        bool isChkModified = false;

        /// <summary>
        /// create object of text box at run time
        /// </summary>
        private TextBox txtRenameAdd = new TextBox();

        /// <summary>
        /// ID of the selected company
        /// </summary>
        public int _companyID = int.MinValue;

        public CounterParties counterParties = new CounterParties();

        //bool _previousCheckState;

        //Constants
        private const string CONST_ACCOUNT_NAME_COLUMN_CAPTION = "Account Name";
        private const string CONST_ACCOUNT_NAME_COLUMN_NAME = "AccountName";
        private const string CONST_EXECUTING_BROKER_COLUMN_CAPTION = "Executing Broker";
        private const string CONST_EXECUTING_BROKER_COLUMN_NAME = "ExecutingBroker";
        private const string CONST_FUND_ID = "FundId";
        private const string CONST_BROKER_ID = "BrokerId";
        private const int CONST_MAX_ACCOUNT_ON_POPUP = 12;

        /// <summary>
        /// contains fund wise executing broker mapping
        /// </summary>
        private Dictionary<int, int> _fundWiseExecutingBrokerBackup;

        /// <summary>
        /// initilaied masterFund mapping control
        /// </summary>
        [AuditManager.Attributes.AuditSourceConstAttri]
        public AUECAccountCVMapping()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Setup Control
        /// </summary>
        /// <param name="p"></param>
        public void SetupControl(int companyID)
        {
            _companyID = companyID;
            BrokerAccountAUECMappingManager.InitialiseData(companyID);
            BindBrokerList();
            InitializeControl(_companyID);
            //_previousCheckState = BrokerAccountAUECMappingManager.IntialiseBreakOrderPrefernce(companyID);
            chkBreakOrder.Checked = BrokerAccountAUECMappingManager.IntialiseBreakOrderPrefernce(companyID);
            isChkModified = false;
            grdExecutingBroker.DataSource = GetDataSourceforExecutingBrokerGrid();
        }

        /// <summary>
        /// Get data table for the grdExecutingBroker
        /// </summary>
        private DataTable GetDataSourceforExecutingBrokerGrid()
        {
            DataTable _dtSource = new DataTable();
            try
            {
                _fundWiseExecutingBrokerBackup = BrokerAccountAUECMappingManager.GetFundWiseExecutingBrokerMapping();
                //Add columns in grdExecutingBroker grid
                _dtSource.Columns.Add(CONST_ACCOUNT_NAME_COLUMN_NAME);
                _dtSource.Columns.Add(CONST_EXECUTING_BROKER_COLUMN_NAME);

                foreach (KeyValuePair<int, string> entry in BrokerAccountAUECMappingManager.GetAccounts())
                {
                    if (_fundWiseExecutingBrokerBackup.ContainsKey(entry.Key) && BrokerAccountAUECMappingManager.GetCompanyCounterParty().ContainsKey(_fundWiseExecutingBrokerBackup[entry.Key]))
                    {
                        _dtSource.Rows.Add(entry.Value, _fundWiseExecutingBrokerBackup[entry.Key]);
                    }
                    else
                    {
                        _dtSource.Rows.Add(entry.Value, string.Empty);
                    }
                }

                this.grdExecutingBroker.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdTrades_AfterCellUpdate);
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
            return _dtSource;
        }

        /// <summary>
        /// Save Executing Broker Mapping into DB
        /// </summary>
        private bool SaveExecutingBrokerMapping()
        {
            try
            {
                string strXmlMapping = string.Empty;
                StringBuilder _strUnmappedAccounts = new StringBuilder(); ;
                int _unmappedAccounts = 0;
                DataSet dsExecutingBrokerMappping = new DataSet("dsExecutingBrokerMappping");
                DataTable dtExecutingBrokerMappping = new DataTable("dtExecutingBrokerMappping");
                dtExecutingBrokerMappping.Columns.Add(CONST_FUND_ID, typeof(int));
                dtExecutingBrokerMappping.Columns.Add(CONST_BROKER_ID, typeof(int));

                DataTable dtGrdSource = (DataTable)grdExecutingBroker.DataSource;
                bool isMappingChanged = false;
                for (int index = 0; index < dtGrdSource.Rows.Count; index++)
                {
                    DataRow row = dtGrdSource.Rows[index];
                    string accountName = row[CONST_ACCOUNT_NAME_COLUMN_NAME].ToString();
                    int accountID = BrokerAccountAUECMappingManager.GetAccounts().FirstOrDefault(x => x.Value == accountName).Key;
                    int brokerId = -1;
                    if (int.TryParse(row[CONST_EXECUTING_BROKER_COLUMN_NAME].ToString(), out brokerId) && BrokerAccountAUECMappingManager.GetCompanyCounterParty().ContainsKey(brokerId))
                    {
                        isMappingChanged = isMappingChanged || (!_fundWiseExecutingBrokerBackup.ContainsKey(accountID) || _fundWiseExecutingBrokerBackup[accountID] != brokerId);
                        dtExecutingBrokerMappping.Rows.Add(accountID, brokerId);
                    }
                    else
                    {
                        _unmappedAccounts++;
                        isMappingChanged = isMappingChanged || _fundWiseExecutingBrokerBackup.ContainsKey(accountID);
                        if (_unmappedAccounts <= CONST_MAX_ACCOUNT_ON_POPUP)
                        {
                            _strUnmappedAccounts.Append(accountName + Environment.NewLine);
                        }
                    }
                }

                if(_unmappedAccounts > CONST_MAX_ACCOUNT_ON_POPUP)
                {
                    int _remainingAccounts = _unmappedAccounts - CONST_MAX_ACCOUNT_ON_POPUP;
                    _strUnmappedAccounts.Append("and " + _remainingAccounts + " more");
                }

                if (_strUnmappedAccounts.Length > 0 && isMappingChanged)
                {
                    DialogResult unmappedAccountsPopupResult = MessageBox.Show("Following Accounts are not mapped if you wish to continue please click ok\n\n" + _strUnmappedAccounts.ToString(), "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if(unmappedAccountsPopupResult == DialogResult.Cancel)
                    {
                        return false;
                    }
                }

                //Save Mapping in db ony when it is modified by user
                if (isMappingChanged)
                {
                    dsExecutingBrokerMappping.Tables.Add(dtExecutingBrokerMappping);
                    strXmlMapping = dsExecutingBrokerMappping.GetXml();
                    BrokerAccountAUECMappingManager.SaveExecutingBrokerMappping(_companyID, strXmlMapping);
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
            return true;
        }

        private void grdTrades_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                int index = -1;
                if (e.Cell.ValueListResolved.GetValue(e.Cell.Text, ref index) == null && !string.IsNullOrEmpty(e.Cell.Text))
                {
                    grdExecutingBroker.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(CONST_EXECUTING_BROKER_COLUMN_NAME, "Invalid Broker!");
                }
                else
                {
                    grdExecutingBroker.Rows[e.Cell.Row.Index].DataErrorInfo.SetColumnError(CONST_EXECUTING_BROKER_COLUMN_NAME, null);
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

        private void grdRevaluationPref_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                ValueList valueList = new ValueList();

                grdExecutingBroker.DisplayLayout.Override.SupportDataErrorInfo = SupportDataErrorInfo.RowsAndCells;

                // Disable the first column
                e.Layout.Bands[0].Columns[CONST_ACCOUNT_NAME_COLUMN_NAME].Header.Caption = CONST_ACCOUNT_NAME_COLUMN_CAPTION;
                e.Layout.Bands[0].Columns[CONST_ACCOUNT_NAME_COLUMN_NAME].CellActivation = Activation.NoEdit;

                foreach(KeyValuePair<int, string> kvp in BrokerAccountAUECMappingManager.GetCompanyCounterParty())
                {
                    valueList.ValueListItems.Add(kvp.Key, kvp.Value);
                }
                valueList.SortStyle = ValueListSortStyle.Ascending;

                e.Layout.Bands[0].Columns[CONST_EXECUTING_BROKER_COLUMN_NAME].Header.Caption = CONST_EXECUTING_BROKER_COLUMN_CAPTION;
                e.Layout.Bands[0].Columns[CONST_EXECUTING_BROKER_COLUMN_NAME].ValueList = valueList;
                e.Layout.Bands[0].Columns[CONST_EXECUTING_BROKER_COLUMN_NAME].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
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
        /// Bind Broker List
        /// </summary>
        private void BindBrokerList()
        {
            try
            {
                var counterParties = BrokerAccountAUECMappingManager.GetCompanyCounterParties().ToList();
                ucmbCounterParty.DisplayMember = "Value";
                ucmbCounterParty.ValueMember = "Key";
                ucmbCounterParty.DataSource = counterParties;
                ucmbCounterParty.Text = "-Select-";
                ucmbCounterParty.DropDownStyle = DropDownStyle.DropDownList;
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

                //this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                //this.groupBox2.ForeColor = System.Drawing.Color.White;

                //this.groupBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.groupBox4.ForeColor = System.Drawing.Color.White;

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
        public void InitializeControl(int companyID)
        {
            try
            {
                #region get selected item(if selected) from different lists on UI
                string selectedCounterParty = string.Empty;
                if (ucmbCounterParty.SelectedIndex > 0)
                {
                    selectedCounterParty = ucmbCounterParty.SelectedItem.ToString();
                }

                string selectedAssignedAccount = string.Empty;
                if (uLstViewAssignedAccounts.SelectedItems.Count > 0)
                {
                    selectedAssignedAccount = uLstViewAssignedAccounts.SelectedItems.First.Key;
                }

                string selectedUnassignedAccount = string.Empty;
                if (uLstViewUnAssignedAccounts.SelectedItems.Count > 0)
                {
                    selectedUnassignedAccount = uLstViewUnAssignedAccounts.SelectedItems.First.Key;
                }

                string selectedAssignedAUEC = string.Empty;
                if (uLstViewAssignedAccounts.SelectedItems.Count > 0)
                {
                    selectedAssignedAUEC = uLstViewAssignedAUECs.SelectedItems.First.Key;
                }

                string selectedUnassignedAUEC = string.Empty;
                if (uLstViewUnAssignedAccounts.SelectedItems.Count > 0)
                {
                    selectedUnassignedAUEC = uLstViewUnAssignedAUECs.SelectedItems.First.Key;
                }

                #endregion
                _companyID = companyID;



                BindUnMappedAccountForOneToMany();
                BindUnMappedAUEC();

                #region retain focus on selected item in lists on UI

                if (!string.IsNullOrEmpty(selectedAssignedAccount) && uLstViewAssignedAccounts.Items.Exists(selectedAssignedAccount))
                {
                    uLstViewAssignedAccounts.SelectedItems.Add(uLstViewAssignedAccounts.Items[selectedAssignedAccount]);
                }

                if (!string.IsNullOrEmpty(selectedUnassignedAccount) && uLstViewUnAssignedAccounts.Items.Exists(selectedUnassignedAccount))
                {
                    uLstViewUnAssignedAccounts.SelectedItems.Add(uLstViewUnAssignedAccounts.Items[selectedUnassignedAccount]);
                }

                if (!string.IsNullOrEmpty(selectedAssignedAUEC) && uLstViewAssignedAUECs.Items.Exists(selectedAssignedAUEC))
                {
                    uLstViewAssignedAUECs.SelectedItems.Add(uLstViewAssignedAUECs.Items[selectedAssignedAUEC]);
                }

                if (!string.IsNullOrEmpty(selectedUnassignedAUEC) && uLstViewUnAssignedAUECs.Items.Exists(selectedUnassignedAUEC))
                {
                    uLstViewUnAssignedAUECs.SelectedItems.Add(uLstViewUnAssignedAUECs.Items[selectedUnassignedAUEC]);
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
        /// initialized  text boxs at run time 
        /// </summary>
        private void InitTextBoxes()
        {
            try
            {
                //this.listMasterFund.Controls.Add(txtRenameAdd);
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
        /// Show all Un mapped accounts in ultra list view 
        /// </summary>
        public void BindUnMappedAccountForOneToMany()
        {
            try
            {
                List<String> unMappedAccountNames = new List<string>();
                int counterPartyId = Convert.ToInt32(this.ucmbCounterParty.SelectedItem.DataValue.ToString());
                if (counterPartyId > 0)
                {

                    unMappedAccountNames = BrokerAccountAUECMappingManager.GetUnmappedAccounts(counterPartyId);
                }
                else
                {
                    unMappedAccountNames = BrokerAccountAUECMappingManager.GetUnmappedAccounts(counterPartyId);
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
        /// Show all Un mapped accounts in ultra list view 
        /// </summary>
        public void BindUnMappedAUEC()
        {
            try
            {
                List<String> unMappedAUECNames = new List<string>();
                int counterPartyId = Convert.ToInt32(this.ucmbCounterParty.SelectedItem.DataValue.ToString());
                if (counterPartyId > 0)
                {

                    unMappedAUECNames = BrokerAccountAUECMappingManager.GetUnmappedAUECs(counterPartyId);
                }
                else
                {
                    unMappedAUECNames = BrokerAccountAUECMappingManager.GetUnmappedAUECs(ucmbCounterParty.SelectedIndex);
                }
                uLstViewUnAssignedAUECs.Items.Clear();
                foreach (String auecName in unMappedAUECNames)
                {
                    UltraListViewItem item = new UltraListViewItem();
                    item.Tag = item.Key = auecName;
                    item.Value = auecName;
                    if (!uLstViewUnAssignedAUECs.Items.Contains(item))
                    {
                        uLstViewUnAssignedAUECs.Items.Add(item);
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
        /// add AUEC names in list to be unassigned 
        /// </summary>
        /// <param name="isOnlySelectedRequired">return true of false only selected Require to unassigned  of all to be un assigned</param>
        /// <returns>List of toBeUnAssignedAUEC</returns>
        private List<String> GetToBeUnAssignedAUECs(bool isOnlySelectedRequired)
        {
            int count;
            List<string> toBeUnAssignedAUECs = new List<string>();
            try
            {

                if (isOnlySelectedRequired)
                {
                    count = uLstViewAssignedAUECs.SelectedItems.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeUnAssignedAUECs.Add(uLstViewAssignedAUECs.SelectedItems[i].Text);
                    }
                }
                else
                {
                    count = uLstViewAssignedAUECs.Items.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeUnAssignedAUECs.Add(uLstViewAssignedAUECs.Items[i].Text);
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
            return toBeUnAssignedAUECs;
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
        /// add AUEC names in list to be assigned 
        /// </summary>
        /// <param name="isOnlySelectedRequired">return true of false only selected Require to assigned  of all to be  assigned</param>
        /// <returns>List of toBeAssignedAUEC</returns>
        private List<string> GetToBeAssignedAUEC(bool isOnlySelectedRequired)
        {
            int count;
            List<string> toBeAssignedAUEC = new List<string>();
            try
            {
                if (isOnlySelectedRequired)
                {
                    count = uLstViewUnAssignedAUECs.SelectedItems.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeAssignedAUEC.Add(uLstViewUnAssignedAUECs.SelectedItems[i].Text);
                    }
                }
                else
                {
                    count = uLstViewUnAssignedAUECs.Items.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeAssignedAUEC.Add(uLstViewUnAssignedAUECs.Items[i].Text);
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
            return toBeAssignedAUEC;
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
                //if (!String.IsNullOrEmpty(uTxtUnassignedAccounts.Text.ToUpper()) && uTxtUnassignedAccounts.Text.ToUpper() != "SEARCH")
                //    AddSearchedItemUnassignedAccount();
                //else
                //{
                //    uLstViewUnAssignedAccounts.Items.Clear();
                //    //modified by: Bharat raturi, 23 apr 2014
                //    //purpose: Bind unmapped accounts according to the chosen mapping style
                //    //BindUnMappedAccount();
                //    //if (chkBoxManyToMany.Checked)
                //    //{
                //    //    BindUnMappedAccount();
                //    //}
                //    //else
                //    //{
                //    //    BindUnMappedAccountForOneToMany();
                //    //}
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
        }

        /// <summary>
        /// show only contain searched items in list
        /// </summary>
        public void AddSearchedItemUnassignedAccount()
        {
            List<String> searchingList = new List<string>();
            try
            {
                //if (listMasterFund.SelectedItems.Count > 0 && !String.IsNullOrEmpty(listMasterFund.SelectedItems[0].Text.ToString()))
                //{
                //    searchingList = BrokerAccountAUECMappingManager.GetUnmappedAccounts(listMasterFund.SelectedItems[0].Text);//new List<string>();
                //}
                //else
                //    searchingList = uLstViewUnAssignedAccounts.Items.Cast<UltraListViewItem>().Select(x => x.Text).ToList();
                //List<String> result = BrokerAccountAUECMappingManager.SerachForKeyword(uTxtUnassignedAccounts.Text, searchingList);
                //uLstViewUnAssignedAccounts.Items.Clear();
                //if (result.Count > 0)
                //{
                //    foreach (String foundItem in result)
                //    {

                //        UltraListViewItem item = new UltraListViewItem(foundItem);
                //        item.Key = foundItem;
                //        item.Tag = foundItem;
                //        item.Value = foundItem;
                //        if (!uLstViewAssignedAccounts.Items.Contains(item.Key))
                //        {
                //            uLstViewUnAssignedAccounts.Items.Add(item);
                //        }
                //    }
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
        }

        /// <summary>
        /// contain search in assigned accounts list and show searched items in assigned list view
        /// </summary>
        /// <param name="sender">textbox</param>
        /// <param name="e">e</param>
        private void uTxtAssignedAccounts_TextChanged(object sender, EventArgs e)
        {
            // string masterFundName;
            try
            {
                //if (!String.IsNullOrEmpty(uTxtAssignedAccounts.Text.ToUpper()) && uTxtAssignedAccounts.Text.ToUpper() != "SEARCH")
                //    AddSearchedItemAssignedAccount();
                //else
                //{
                //    uLstViewAssignedAccounts.Items.Clear();
                //    if (listMasterFund.SelectedItems.Count >= 1)
                //    {
                //        masterFundName = listMasterFund.SelectedItems[0].Text;
                //    }
                //    else
                //    {
                //        masterFundName = null;
                //    }
                //    List<String> accountNames = BrokerAccountAUECMappingManager.GetAccountNamesForMasterFund(masterFundName);
                //    if (accountNames != null)
                //    {
                //        foreach (String foundItem in accountNames)
                //        {
                //            UltraListViewItem item = new UltraListViewItem();
                //            item.Key = foundItem;
                //            item.Tag = foundItem;
                //            item.Value = foundItem;
                //            uLstViewAssignedAccounts.Items.Add(item);
                //        }
                //    }
                //    else
                //    {
                //        uLstViewAssignedAccounts.Items.Clear();
                //    }
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
        }

        /// <summary>
        /// Show only Searched item in Assigned list view for a slecected masterFund name
        /// </summary>
        public void AddSearchedItemAssignedAccount()
        {
            // string masterFundName;
            try
            {
                //if (listMasterFund.SelectedItems.Count >= 1)
                //{
                //    masterFundName = listMasterFund.SelectedItems[0].Text;
                //}
                //else
                //{
                //    masterFundName = null;
                //}
                //List<String> searchingList = BrokerAccountAUECMappingManager.GetAccountNamesForMasterFund(masterFundName);
                //List<String> result = BrokerAccountAUECMappingManager.SerachForKeyword(uTxtAssignedAccounts.Text, searchingList);
                //uLstViewAssignedAccounts.Items.Clear();
                //if (result.Count > 0)
                //{
                //    foreach (String foundItem in result)
                //    {
                //        UltraListViewItem item = new UltraListViewItem(foundItem);
                //        item.Key = foundItem;
                //        item.Tag = foundItem;
                //        item.Value = foundItem;
                //        uLstViewAssignedAccounts.Items.Add(item);
                //    }
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
                //if (uTxtUnassignedAccounts.Text.Trim().ToLower() == "search")
                //{
                //    uTxtUnassignedAccounts.SelectAll();
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

        private void ucmbCounterParty_SelectionChanged(object sender, EventArgs e)
        {
            string selectedItem = this.ucmbCounterParty.SelectedItem.ToString();

            try
            {
                int counterPartyId = Convert.ToInt32(this.ucmbCounterParty.SelectedItem.DataValue.ToString());
                if (counterPartyId > 0)
                {

                    BindMappedAccount(BrokerAccountAUECMappingManager.GetAccountNamesForCounterParty(counterPartyId));
                    BindUnMappedAccountForOneToMany();

                    BindMappedAUEC(BrokerAccountAUECMappingManager.GetAUECNamesForCounterParty(counterPartyId));
                    BindUnMappedAUEC();
                }
                else
                {
                    BindMappedAccount(null);
                    BindUnMappedAccountForOneToMany();

                    BindMappedAUEC(null);
                    BindUnMappedAUEC();
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
        /// Show mapped Accounts in ultra list view 
        /// </summary>
        /// <param name="accountNames">List of mappedAccount Name for A selected MAsterAccount Name </param>
        public void BindMappedAUEC(List<String> auecNames)
        {
            try
            {
                if (auecNames == null)
                    uLstViewAssignedAUECs.Items.Clear();
                else
                {

                    uLstViewAssignedAUECs.Items.Clear();
                    foreach (String auecName in auecNames)
                    {
                        UltraListViewItem item = new UltraListViewItem();
                        item.Tag = item.Key = auecName;
                        item.Value = auecName;
                        if (!uLstViewAssignedAUECs.Items.Contains(item.Key))
                        {
                            uLstViewAssignedAUECs.Items.Add(item);
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
        /// To be unassign selected  Accounts on button click and show selected accounts in unmapped list view
        /// </summary>
        /// <param name="sender">button</param>
        /// <param name="e">null</param>
        private void UBtnUnSelectAssignedAccounts_Click(object sender, EventArgs e)
        {

            try
            {
                int counterPartyId = Convert.ToInt32(this.ucmbCounterParty.SelectedItem.DataValue.ToString());
                if (counterPartyId > 0)
                {


                    List<string> unselectaccounts = GetToBeUnAssignedAccounts(true);
                    BrokerAccountAUECMappingManager.UnassignAccounts(counterPartyId, unselectaccounts);
                    List<string> accountnames = BrokerAccountAUECMappingManager.GetAccountNamesForCounterParty(counterPartyId);
                    uLstViewAssignedAccounts.Items.Clear();
                    BindMappedAccount(accountnames);
                    BindUnMappedAccountForOneToMany();

                    isMappingModified = true;

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
                int counterPartyId = Convert.ToInt32(this.ucmbCounterParty.SelectedItem.DataValue.ToString());
                if (counterPartyId > 0)
                {

                    List<string> unSelectAccounts = GetToBeUnAssignedAccounts(false);

                    BrokerAccountAUECMappingManager.UnassignAccounts(counterPartyId, unSelectAccounts);
                    List<String> accountNames = BrokerAccountAUECMappingManager.GetAccountNamesForCounterParty(counterPartyId);
                    uLstViewAssignedAccounts.Items.Clear();
                    BindMappedAccount(accountNames);

                    BindUnMappedAccountForOneToMany();
                    isMappingModified = true;
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
        /// Slelected unassigned accounts assign in selected master Fund
        /// </summary>
        /// <param name="sender">button</param>
        /// <param name="e">null</param>
        private void uBtnSelectUnassignedAccounts_Click(object sender, EventArgs e)
        {
            try
            {
                int counterPartyId = Convert.ToInt32(this.ucmbCounterParty.SelectedItem.DataValue.ToString());
                if (counterPartyId > 0)
                {


                    List<string> assignedAccounts = GetToBeAssignedAccounts(true);
                    BrokerAccountAUECMappingManager.AssignAccounts(counterPartyId, assignedAccounts);
                    List<String> accountNames = BrokerAccountAUECMappingManager.GetAccountNamesForCounterParty(counterPartyId);
                    uLstViewUnAssignedAccounts.Items.Clear();
                    BindMappedAccount(accountNames);

                    BindUnMappedAccountForOneToMany();

                    isMappingModified = true;

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
                int counterPartyId = Convert.ToInt32(this.ucmbCounterParty.SelectedItem.DataValue.ToString());
                if (counterPartyId > 0)
                {

                    List<string> assignedAccounts = GetToBeAssignedAccounts(false);
                    BrokerAccountAUECMappingManager.AssignAccounts(counterPartyId, assignedAccounts);
                    List<String> accountNames = BrokerAccountAUECMappingManager.GetAccountNamesForCounterParty(counterPartyId);
                    uLstViewUnAssignedAccounts.Items.Clear();
                    BindMappedAccount(accountNames);

                    BindUnMappedAccountForOneToMany();

                    isMappingModified = true;

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
        /// To be unassign selected  AUEC on button click and show selected AUEC in unmapped list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UBtnUnSelectAssignedAUECs_Click(object sender, EventArgs e)
        {
            try
            {
                int counterPartyId = Convert.ToInt32(this.ucmbCounterParty.SelectedItem.DataValue.ToString());
                if (counterPartyId > 0)
                {


                    List<string> unselectaccounts = GetToBeUnAssignedAUECs(true);
                    BrokerAccountAUECMappingManager.UnassignAUEC(counterPartyId, unselectaccounts);
                    List<string> auecNames = BrokerAccountAUECMappingManager.GetAUECNamesForCounterParty(counterPartyId);
                    uLstViewAssignedAUECs.Items.Clear();
                    BindMappedAUEC(auecNames);
                    BindUnMappedAUEC();

                    isMappingModified = true;

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
        /// To be unassign all  AUEC from assigned list view on button click and show all AUEC in unmapped list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uBtnAllUnSelectAssignedAUECs_Click(object sender, EventArgs e)
        {
            try
            {
                int counterPartyId = Convert.ToInt32(this.ucmbCounterParty.SelectedItem.DataValue.ToString());
                if (counterPartyId > 0)
                {
                    List<string> unSelectAUECs = GetToBeUnAssignedAUECs(false);

                    BrokerAccountAUECMappingManager.UnassignAUEC(counterPartyId, unSelectAUECs);
                    List<String> auecNames = BrokerAccountAUECMappingManager.GetAUECNamesForCounterParty(counterPartyId);
                    uLstViewAssignedAUECs.Items.Clear();
                    BindMappedAUEC(auecNames);
                    BindUnMappedAUEC();

                    isMappingModified = true;

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
        /// Slelected unassigned AUEC assign in selected master Fund
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uBtnSelectUnassignedAUECs_Click(object sender, EventArgs e)
        {
            try
            {
                int counterPartyId = Convert.ToInt32(this.ucmbCounterParty.SelectedItem.DataValue.ToString());
                if (counterPartyId > 0)
                {
                    List<string> assignedAUEC = GetToBeAssignedAUEC(true);
                    BrokerAccountAUECMappingManager.AssignAUEC(counterPartyId, assignedAUEC);
                    List<String> auecNames = BrokerAccountAUECMappingManager.GetAUECNamesForCounterParty(counterPartyId);
                    uLstViewUnAssignedAUECs.Items.Clear();
                    BindMappedAUEC(auecNames);
                    BindUnMappedAUEC();

                    isMappingModified = true;

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
        /// all assigned AUEC assign in unassinged accounts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uBtnAllUnSelectUnassignedAUECs_Click(object sender, EventArgs e)
        {
            try
            {
                int counterPartyId = Convert.ToInt32(this.ucmbCounterParty.SelectedItem.DataValue.ToString());
                if (counterPartyId > 0)
                {

                    List<string> assignedAUEC = GetToBeAssignedAUEC(false);
                    BrokerAccountAUECMappingManager.AssignAUEC(counterPartyId, assignedAUEC);
                    List<String> auecNames = BrokerAccountAUECMappingManager.GetAUECNamesForCounterParty(counterPartyId);
                    uLstViewUnAssignedAUECs.Items.Clear();
                    BindMappedAUEC(auecNames);
                    BindUnMappedAUEC();

                    isMappingModified = true;

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

        public int SaveMapping(int companyID)
        {
            try
            {
                bool isValidMapping = SaveExecutingBrokerMapping();
                if (isValidMapping)
                {
                    if (isChkModified)
                    {
                        BrokerAccountAUECMappingManager.SaveBreakOrderPrefernce(companyID, chkBreakOrder.Checked);
                    }
                    if (isMappingModified)
                    {
                        var result = BrokerAccountAUECMappingManager.SaveMapping(companyID);
                        return result;
                    }
                    else
                        return 0;
                }
                return -1;
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
                return -1;
            }
        }

        private void chkBreakOrder_CheckedChanged(object sender, EventArgs e)
        {
            //if (!_previousCheckState && chkBreakOrder.Checked)
            //{
            //    DialogResult dialogResult = MessageBox.Show("This action cannot be undone; this will delete all the existing account broker mapping.Do you want to proceed ?","Alert", MessageBoxButtons.YesNo,MessageBoxIcon.Information);
            //}
            isChkModified = true;
        }
    }
}