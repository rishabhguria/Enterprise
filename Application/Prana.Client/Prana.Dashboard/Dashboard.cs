using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Dashboard.BLL;
using Prana.Dashboard.View;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Dashboard
{
    public partial class Dashboard : Form, IPluggableTools
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// account wise final dashboard data collection
        /// </summary>
        internal List<MasterDashboardUIObj> _dashboardUIObjects = new List<MasterDashboardUIObj>();

        /// <summary>
        /// Account wise each workflow event status
        /// </summary>
        internal ConcurrentDictionary<String, List<WorkflowItem>> _accountWiseDashboardData = new ConcurrentDictionary<String, List<WorkflowItem>>();
        CtrlWorkFlowDetails ctrlWorkFlowDetails = null;
        Form _frmWorkFlowDetails = null;

        private delegate void UIThreadMarsheller(object sender, EventArgs e);

        /// <summary>
        /// Setup the layout of the account control
        /// </summary>
        private void SetAccountControl()
        {
            if (ucbAccount.DataSource != null)
            {
                if (!ucbAccount.DisplayLayout.Bands[0].Columns.Exists("Selected"))
                {
                    UltraGridColumn cbAccount = ucbAccount.DisplayLayout.Bands[0].Columns.Add();
                    cbAccount.Key = "Selected";
                    cbAccount.Header.Caption = string.Empty;
                    cbAccount.Width = 25;
                    cbAccount.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                    cbAccount.DataType = typeof(bool);
                    cbAccount.Header.VisiblePosition = 1;
                }
                ucbAccount.CheckedListSettings.CheckStateMember = "Selected";
                ucbAccount.CheckedListSettings.EditorValueSource = EditorWithComboValueSource.CheckedItems;
                ucbAccount.CheckedListSettings.ListSeparator = " , ";
                ucbAccount.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
                ucbAccount.DisplayMember = "AccountName";
                ucbAccount.ValueMember = "AccountID";
                ucbAccount.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Select All";
            }
        }

        /// <summary>
        /// bind data on client combobox on startup
        /// </summary>
        private void BindClientCombo()
        {
            try
            {
                List<EnumerationValue> listValues = new List<EnumerationValue>();
                // To bind all permitted clients for selected user
                Dictionary<int, string> dictClients = new Dictionary<int, string>();
                foreach (KeyValuePair<int, List<int>> clients in CachedDataManager.GetInstance.GetAllCompanyAccounts())
                {
                    if (CachedDataManager.GetUserPermittedCompanyList().ContainsKey(clients.Key))
                    {
                        if (!dictClients.ContainsKey(clients.Key))
                        {
                            dictClients.Add(clients.Key, CachedDataManager.GetUserPermittedCompanyList()[clients.Key]);
                        }
                    }
                }
                foreach (KeyValuePair<int, string> kvp in dictClients)
                {
                    EnumerationValue value = new EnumerationValue(kvp.Value, kvp.Key);
                    listValues.Add(value);
                }
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-2288
                listValues = listValues.OrderBy(e => e.DisplayText).ToList();
                listValues.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
                // cmbClient = new UltraCombo();
                ucbClient.DataSource = null;
                ucbClient.DataSource = listValues;
                ucbClient.DisplayMember = "DisplayText";
                ucbClient.ValueMember = "Value";
                ucbClient.DataBind();
                ucbClient.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                ucbClient.Value = -1;
                ucbClient.DisplayLayout.Bands[0].ColHeadersVisible = false;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        private void UpdateAccountWiseWorkflowData(List<WorkflowItem> items)
        {
            try
            {
                foreach (WorkflowItem workflowItem in items)
                {
                    String key = DashboardHelper.GetKey(workflowItem.AccountID, workflowItem.FileExecutionDate);
                    List<WorkflowItem> list;
                    bool isfound = _accountWiseDashboardData.TryGetValue(key, out list);

                    //only for selected date data updating on UI
                    if (isfound && list != null && list.Count > 0)
                    {
                        list.Add(workflowItem);
                        NirvanaWorkFlows workflowID = (NirvanaWorkFlows)Enum.ToObject(typeof(NirvanaWorkFlows), workflowItem.EventID);
                        NirvanaTaskStatus status = DashboardHelper.GetWorkflowStatus(list, NirvanaWorkFlows.FileUpload);
                        pranaGridControl1.UpdateStatusOnDashboard(workflowItem.AccountID, workflowID, status);

                    }
                    else
                    {

                        _accountWiseDashboardData.TryAdd(key, new List<WorkflowItem> { workflowItem });

                        MasterDashboardUIObj Uiobj = DashboardHelper.GetDashboardUIObject(workflowItem);
                        _dashboardUIObjects.Add(Uiobj);
                        if (_dashboardUIObjects != null && _dashboardUIObjects.Count > 0)
                            pranaGridControl1.SetGridData(_dashboardUIObjects);
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

        /// <summary>
        /// Action to be performed on change of client from the combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucbClient_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(ucbClient.SelectedRow.Cells["Value"].Text))
                {
                    int companyID = Convert.ToInt32(ucbClient.SelectedRow.Cells["Value"].Text);
                    if (companyID == -1)
                    {
                        ucbAccount.Enabled = false;
                        return;
                    }
                    ucbAccount.Enabled = true;
                    ucbAccount.Text = string.Empty;
                    Dictionary<int, string> userAccounts = CachedDataManager.GetInstance.GetUserAccountsAsDict();
                    userAccounts = GeneralUtilities.SortDictionaryByValues<int, string>(userAccounts);
                    Dictionary<int, string> userAccountsForSelectedCompany = new Dictionary<int, string>();
                    DataTable dtUserPermittedAccounts = new DataTable();
                    dtUserPermittedAccounts.Columns.Add("FundID", typeof(int));
                    dtUserPermittedAccounts.Columns.Add("FundName", typeof(string));
                    if (CachedDataManager.GetInstance.CompanyAccountsMapping.ContainsKey(companyID))
                    {
                        foreach (Account account in CachedDataManager.GetInstance.CompanyAccountsMapping[companyID])
                        {
                            if (userAccounts.ContainsKey(account.AccountID) && !userAccountsForSelectedCompany.ContainsKey(account.AccountID))
                            {
                                userAccountsForSelectedCompany.Add(account.AccountID, account.FullName);
                            }
                        }
                    }
                    userAccountsForSelectedCompany = GeneralUtilities.SortDictionaryByValues<int, string>(userAccountsForSelectedCompany);
                    foreach (int accountID in userAccountsForSelectedCompany.Keys)
                    {
                        dtUserPermittedAccounts.Rows.Add(accountID, userAccountsForSelectedCompany[accountID]);
                    }

                    ucbAccount.DataSource = dtUserPermittedAccounts;
                    ucbAccount.DisplayLayout.Bands[0].Columns["AccountID"].Hidden = true;
                    SetAccountControl();
                    if (ucbAccount.Rows.Count > 0)
                    {
                        ucbAccount.SelectedRow = ucbAccount.Rows[0];
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


        #region IPluggableTools Members

        public void SetUP()
        {

        }

        public Form Reference()
        {
            return this;
        }

        public event EventHandler PluggableToolsClosed;

        public ISecurityMasterServices SecurityMaster
        {
            set {; }
        }

        public IPostTradeServices PostTradeServices
        {
            set {; }
        }

        public IPricingAnalysis PricingAnalysis
        {
            set {; }
        }

        #endregion

        private void Dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                DashboardManager.WorkflowListener -= Dashborad_WorkflowListener;
                if (PluggableToolsClosed != null)
                {
                    PluggableToolsClosed(this, null);
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
        /// On load wire events and initialize data 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dashboard_Load(object sender, EventArgs e)
        {
            try
            {
                //InitializeDashboardData();
                DashboardManager.WorkflowListener += Dashborad_WorkflowListener;
                pranaGridControl1.RefreshData += pranaGridControl1_RefreshData;
                pranaGridControl1.ShowDetailsHandler += pranaGridControl1_ShowDetailsHandler;
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                BindClientCombo();
                toolStripStatusLabel1.Text = "Please set filters and click \'Refresh\' to show work flow status.";
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
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

        private void SetButtonsColor()
        {
            try
            {
                btnGetData.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetData.ForeColor = System.Drawing.Color.White;
                btnGetData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetData.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetData.UseAppStyling = false;
                btnGetData.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

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

        void pranaGridControl1_RefreshData(object sender, EventArgs e)
        {
            try
            {
                btnGetData_Click(this, null);
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
        /// Show account wise workflow details 
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="workflow"></param>
        /// <param name="date"></param>
        void pranaGridControl1_ShowDetailsHandler(int accountID, NirvanaWorkFlows workflow, DateTime date)
        {
            try
            {
                String key = DashboardHelper.GetKey(accountID, date);
                List<WorkflowItem> list;
                bool isfound = _accountWiseDashboardData.TryGetValue(key, out list);

                //only for selected date data updating on UI
                if (isfound && list != null && list.Count > 0)
                {

                    if (_frmWorkFlowDetails == null || _frmWorkFlowDetails.IsDisposed)
                    {
                        ctrlWorkFlowDetails = new CtrlWorkFlowDetails();
                        _frmWorkFlowDetails = new Form();
                        _frmWorkFlowDetails.ShowIcon = false;
                        _frmWorkFlowDetails.StartPosition = FormStartPosition.Manual;
                        SetThemeAtDynamicForm(_frmWorkFlowDetails, ctrlWorkFlowDetails);
                        _frmWorkFlowDetails.Size = new System.Drawing.Size(1000, 500);
                        _frmWorkFlowDetails.MinimumSize = _frmWorkFlowDetails.Size;
                        // _frmWorkFlowDetails.FormClosed +;
                    }
                    _frmWorkFlowDetails.Text = CommonDataCache.CachedDataManager.GetInstance.GetAccountText(accountID) + " Work Flow Details";
                    //_frmWorkFlowDetails.Controls.Add(ctrlWorkFlowDetails);
                    ctrlWorkFlowDetails.FillData(list);
                    CustomThemeHelper.SetThemeProperties(_frmWorkFlowDetails, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                    _frmWorkFlowDetails.Show();
                    BringFormToFront(_frmWorkFlowDetails);

                }
                else
                {
                    MessageBox.Show("No details for work flow", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

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
        /// sets theme at the form if the `
        /// </summary>
        /// <param name="dynamicForm"></param>
        /// <param name="control"></param>
        private void SetThemeAtDynamicForm(Form dynamicForm, UserControl control)
        {
            try
            {
                System.ComponentModel.IContainer dynamicComponents = new System.ComponentModel.Container();
                Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
                Infragistics.Win.Misc.UltraPanel dynamicForm_Fill_Panel;
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea dynamicForm_Toolbars_Dock_Area_Left;
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea dynamicForm_Toolbars_Dock_Area_Right;
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea dynamicForm_Toolbars_Dock_Area_Bottom;
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea dynamicForm_Toolbars_Dock_Area_Top;
                // 
                // ultraToolbarsManager1
                // 
                ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(dynamicComponents);
                dynamicForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
                dynamicForm_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                dynamicForm_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                dynamicForm_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                dynamicForm_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                ((System.ComponentModel.ISupportInitialize)(ultraToolbarsManager1)).BeginInit();
                dynamicForm_Fill_Panel.SuspendLayout();
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1691
                //SuspendLayout();
                // 
                // ultraToolbarsManager1
                // 
                ultraToolbarsManager1.DesignerFlags = 1;
                ultraToolbarsManager1.DockWithinContainer = dynamicForm;
                ultraToolbarsManager1.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
                ultraToolbarsManager1.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.RoundedSizable;
                ultraToolbarsManager1.IsGlassSupported = false;
                // 
                // frmReconCancelAmend_Fill_Panel
                // 
                dynamicForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
                dynamicForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
                dynamicForm_Fill_Panel.Location = new System.Drawing.Point(4, 52);
                dynamicForm_Fill_Panel.Name = "dynamicForm_Fill_Panel";
                dynamicForm_Fill_Panel.Size = new System.Drawing.Size(576, 261);
                dynamicForm_Fill_Panel.TabIndex = 0;
                // 
                // _frmReconCancelAmend_Toolbars_Dock_Area_Left
                // 
                dynamicForm_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                dynamicForm_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                dynamicForm_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
                dynamicForm_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
                dynamicForm_Toolbars_Dock_Area_Left.InitialResizeAreaExtent = 4;
                dynamicForm_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 52);
                dynamicForm_Toolbars_Dock_Area_Left.Name = "dynamicForm_Toolbars_Dock_Area_Left";
                dynamicForm_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(4, 261);
                dynamicForm_Toolbars_Dock_Area_Left.ToolbarsManager = ultraToolbarsManager1;
                // 
                // _frmReconCancelAmend_Toolbars_Dock_Area_Right
                // 
                dynamicForm_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                dynamicForm_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                dynamicForm_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
                dynamicForm_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
                dynamicForm_Toolbars_Dock_Area_Right.InitialResizeAreaExtent = 4;
                dynamicForm_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(580, 52);
                dynamicForm_Toolbars_Dock_Area_Right.Name = "dynamicForm_Toolbars_Dock_Area_Right";
                dynamicForm_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(4, 261);
                dynamicForm_Toolbars_Dock_Area_Right.ToolbarsManager = ultraToolbarsManager1;
                // 
                // _frmReconCancelAmend_Toolbars_Dock_Area_Top
                // 
                dynamicForm_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                dynamicForm_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                dynamicForm_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
                dynamicForm_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
                dynamicForm_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
                dynamicForm_Toolbars_Dock_Area_Top.Name = "dynamicForm_Toolbars_Dock_Area_Top";
                dynamicForm_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(584, 52);
                dynamicForm_Toolbars_Dock_Area_Top.ToolbarsManager = ultraToolbarsManager1;
                // 
                // _frmReconCancelAmend_Toolbars_Dock_Area_Bottom
                // 
                dynamicForm_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                dynamicForm_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                dynamicForm_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
                dynamicForm_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
                dynamicForm_Toolbars_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
                dynamicForm_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 313);
                dynamicForm_Toolbars_Dock_Area_Bottom.Name = "dynamicForm_Toolbars_Dock_Area_Bottom";
                dynamicForm_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(584, 4);
                dynamicForm_Toolbars_Dock_Area_Bottom.ToolbarsManager = ultraToolbarsManager1;
                // 
                // frm
                //                 
                control.Dock = DockStyle.Fill;
                dynamicForm.Owner = this.FindForm();
                dynamicForm.ShowInTaskbar = false;
                dynamicForm.Controls.Add(control);
                dynamicForm.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                dynamicForm.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                dynamicForm.Controls.Add(dynamicForm_Fill_Panel);
                dynamicForm.Controls.Add(dynamicForm_Toolbars_Dock_Area_Left);
                dynamicForm.Controls.Add(dynamicForm_Toolbars_Dock_Area_Right);
                dynamicForm.Controls.Add(dynamicForm_Toolbars_Dock_Area_Bottom);
                dynamicForm.Controls.Add(dynamicForm_Toolbars_Dock_Area_Top);
                ((System.ComponentModel.ISupportInitialize)(ultraToolbarsManager1)).EndInit();
                dynamicForm_Fill_Panel.ClientArea.ResumeLayout(false);
                dynamicForm_Fill_Panel.ClientArea.PerformLayout();
                dynamicForm_Fill_Panel.ResumeLayout(false);
                dynamicForm.ResumeLayout(false);
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
        private void BringFormToFront(Form form)
        {
            if (form.WindowState == FormWindowState.Minimized)
            {
                form.WindowState = FormWindowState.Normal;

            }
            form.Location = new Point(this.Location.X + 30, this.Location.Y + 30);
            form.BringToFront();
        }

        /// <summary>
        /// Handle when workflow event has raised
        /// </summary>
        /// <param name="items"></param>
        void Dashborad_WorkflowListener(object sender, EventArgs e)
        {
            try
            {
                WorkFlowEventArgs workflowEventArgs = e as WorkFlowEventArgs;
                List<WorkflowItem> items = workflowEventArgs.items;
                if (this.InvokeRequired)
                {
                    UIThreadMarsheller del = new UIThreadMarsheller(Dashborad_WorkflowListener);
                    this.BeginInvoke(del, new object[] { items });
                }
                else
                {
                    UpdateAccountWiseWorkflowData(items);
                    //if (_dashboardUIObjects != null)
                    //    pranaGridControl1.SeGridData(_dashboardUIObjects);

                    //pranaGridControl1.Refresh();
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
        /// Get the comma separated IDs of the accounts
        /// </summary>
        /// <returns>The string holding the Comma Separated IDs of the selected accounts</returns>
        private String GetAccountIDs()
        {
            String accountID = string.Empty;
            DataTable dtAccount = new DataTable("dtAccount");
            DataSet dsAccount = new DataSet("dsAccount");
            dtAccount.Columns.Add("AccountID", typeof(int));

            try
            {
                foreach (UltraGridRow row in ucbAccount.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["Selected"].Value))
                    {
                        if (!string.IsNullOrEmpty(row.Cells["AccountID"].Value.ToString()))
                        {
                            int account = Convert.ToInt32(row.Cells["AccountID"].Value);
                            dtAccount.Rows.Add(account);
                        }
                    }
                }
                dsAccount.Tables.Add(dtAccount);
                accountID = dsAccount.GetXml();
                return accountID;
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
            return null;
        }

        /// <summary>
        /// Fetch data based on filters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetData_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ucbAccount.Enabled || ucbAccount.Text == string.Empty)
                {
                    MessageBox.Show("Select Client and at least one account to show the data", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string xmlAccounts = GetAccountIDs();
                Boolean isSearchByFileExecutionDate = true;
                String date = ultraCalendarCombo1.Text;
                _accountWiseDashboardData = DashboardManager.GetDashboardDataForDate(DateTime.Parse(date), xmlAccounts, isSearchByFileExecutionDate);

                if (_accountWiseDashboardData.Count > 0)
                {
                    _dashboardUIObjects = DashboardHelper.GetConsolidatedDashboardData(_accountWiseDashboardData);

                    if (_dashboardUIObjects != null && _dashboardUIObjects.Count > 0)
                        pranaGridControl1.SetGridData(_dashboardUIObjects);

                    toolStripStatusLabel1.Text = "Check detailed status of account  by \'Show details\' on right click menu.";

                }
                else
                {
                    //modified by amit on 15.04.2015
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-3330
                    pranaGridControl1.SetGridData(new List<MasterDashboardUIObj>());
                    MessageBox.Show("There is no batch specified for selected accounts.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
