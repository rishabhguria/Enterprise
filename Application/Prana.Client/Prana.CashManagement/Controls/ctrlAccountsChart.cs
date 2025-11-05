using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Constants;
using Prana.CashManagement.Classes;
using Prana.CashManagement.Forms;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.UI;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prana.CashManagement.Controls
{
    public partial class ctrlAccountsChart : UserControl, IPublishing
    {
        public ctrlAccountsChart()
        {
            InitializeComponent();
            CreateSubscriptionServicesProxy();
            Subscribe();
        }

        #region Global Variables
        frmAccountTransactionDetails transactionDetails = null;
        DuplexProxyBase<ISubscription> _proxy;
        DataSet _dataFromDB = new DataSet();
        bool runManualRevaluation = false;
        #endregion

        public void CreateSubscriptionServicesProxy()
        {
            try
            {
                _proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);
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

        private void Subscribe()
        {
            try
            {
                if (_proxy != null)
                {
                    _proxy.UnSubscribe();
                    _proxy.Subscribe(Topics.Topic_RevaluationProgress, null);
                    _proxy.Subscribe(Topics.Topic_RevaluationDate, null);
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
        /// Initialize the control.
        /// </summary>
        public void SetUp()
        {
            try
            {
                udtBalanceDate.DateTime = DateTime.Now.Date;
                LoadSubAccountCombo();
                SetAccDetailsUIDisplay((int)cmbAccounts.SelectedItem.DataValue, DateTime.Now.AddDays(-1), DateTime.Now);
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
                btnExportBalances.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnExportBalances.ForeColor = System.Drawing.Color.White;
                btnExportBalances.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnExportBalances.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnExportBalances.UseAppStyling = false;
                btnExportBalances.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGetAccBalances.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetAccBalances.ForeColor = System.Drawing.Color.White;
                btnGetAccBalances.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetAccBalances.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetAccBalances.UseAppStyling = false;
                btnGetAccBalances.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnRunRevaluation.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnRunRevaluation.ForeColor = System.Drawing.Color.White;
                btnRunRevaluation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRunRevaluation.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRunRevaluation.UseAppStyling = false;
                btnRunRevaluation.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnCalculateBalances.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnCalculateBalances.ForeColor = System.Drawing.Color.White;
                btnCalculateBalances.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCalculateBalances.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCalculateBalances.UseAppStyling = false;
                btnCalculateBalances.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnExportDetails.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnExportDetails.ForeColor = System.Drawing.Color.White;
                btnExportDetails.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnExportDetails.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnExportDetails.UseAppStyling = false;
                btnExportDetails.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGetAccountDetails.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetAccountDetails.ForeColor = System.Drawing.Color.White;
                btnGetAccountDetails.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetAccountDetails.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetAccountDetails.UseAppStyling = false;
                btnGetAccountDetails.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        #region Account Balances tab
        /// <summary>
        /// Right now manual, but eventually it would be called while fetching the balances in an optimized manner.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalculateBalances_Click(object sender, EventArgs e)
        {
            try
            {
                CalculateAccountBalances();
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
        /// Raturi: Check for the accounts that require update before calculating the balances
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-7649
        /// </summary>
        private void CalculateAccountBalances()
        {
            try
            {
                string accountIDs = AccountsToUpdate();
                DateTime balDate = udtBalanceDate.DateTime.Add(DateTime.Now.TimeOfDay);
                if (!string.IsNullOrWhiteSpace(accountIDs))
                {
                    CashDataManager.GetInstance().CalculateAndSaveBalances(balDate, accountIDs);
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
        /// 
        /// </summary>
        /// <param name="isCompleted"></param>
        private void ChangeStatus(bool isCompleted)
        {
            try
            {
                if (isCompleted)
                {
                    ugbxAccountBalParams.Enabled = true;
                    btnGetAccBalances.Text = "Get Account Balances";
                    if (!this.IsDisposed)
                        toolStripAccountsChart.Text = "A/C Balances Calculated Successfully";
                }
                else
                {
                    ugbxAccountBalParams.Enabled = false;
                    btnGetAccBalances.Text = "Getting A/C Balances...";
                    if (!this.IsDisposed)
                    {
                        toolStripProgressBar1.Visible = false;
                        toolStripAccountsChart.Text = "Getting A/C Balances...";
                        if (!CustomThemeHelper.ApplyTheme)
                            toolStripAccountsChart.ForeColor = System.Drawing.Color.DarkGreen;
                    }
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
        /// Calculate and save the account balances
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetAccBalances_Click(object sender, EventArgs e)
        {
            try
            {
                string accountIds = ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',');
                if (!CashDataManager.GetInstance().GetIsRevaluationInProgress(accountIds))
                {
                    toolStripProgressBar1.Visible = false;
                    BackgroundWorker bgwCalcAndGetAsync = new BackgroundWorker();
                    bgwCalcAndGetAsync.DoWork += new DoWorkEventHandler(bgwCalcAndGetAsync_DoWork);
                    bgwCalcAndGetAsync.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwCalcAndGetAsync_RunWorkerCompleted);

                    if (ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetNoOfCheckedItems() == 0)
                    {
                        MessageBox.Show("Select at least one account to proceed.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ctrlMasterFundAndAccountsDropdown1.MultiAccounts.Focus();
                        return;
                    }
                    //Added to check if the date of the accounts is greater than the start date
                    else if (ConflictedStartDateWithAccounts())
                    {
                        return;
                    }
                    else if (IsObsoleteData())
                    {
                        DialogResult result = MessageBox.Show("Revaluation entries are obsolete. Click Yes for Revaluation, No for account balances without updated revaluation.\n[Please note that revaluation is a time and resource consuming process and can take several minutes to complete]", "Outdated Revaluation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            btnRunRevaluation_Click(null, null);
                        }
                        else if (result == DialogResult.No)
                        {
                            ChangeStatus(false);
                            bgwCalcAndGetAsync.RunWorkerAsync(new object[] { udtBalanceDate.DateTime, ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',') });
                        }
                    }
                    else
                    {
                        ChangeStatus(false);
                        bgwCalcAndGetAsync.RunWorkerAsync(new object[] { udtBalanceDate.DateTime, ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',') });
                    }
                }
                else
                {
                    MessageBox.Show("Revaluation is in progress for selected fund(s). Proceed after revaluation is done.", "Revaluation ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        /// <summary>
        /// Added by: Bharat raturi
        /// Check if the balances are updated
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-4202
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-7649
        /// </summary>
        /// <returns>accountIDs eligible for calculation</returns>
        private string AccountsToUpdate()
        {
            string accountIDs = string.Empty;
            try
            {
                Dictionary<int, BalanceUpdateDetail> dictBalanceDates = CashDataManager.GetInstance().GetLastCalculationBalanceDetails();
                int[] accounts = ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',').Split(',').Select(account => Convert.ToInt32(account)).ToArray();
                foreach (int account in accounts)
                {
                    if (!dictBalanceDates.ContainsKey(account) || (DateTime.Compare(udtBalanceDate.DateTime.Date, dictBalanceDates[account].LastBalanceDate.Date) > 0
                         || (DateTime.Compare(udtBalanceDate.DateTime.Date, dictBalanceDates[account].LastBalanceDate.Date) == 0 && !dictBalanceDates[account].isUpdatedBalance)))
                        accountIDs += account.ToString() + ",";
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
            return accountIDs.TrimEnd(',');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ConflictedStartDateWithAccounts()
        {
            try
            {
                string[] accountIDs = ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',').Split(',');
                Dictionary<string, DateTime> dictAccountDate = new Dictionary<string, DateTime>();
                foreach (String id in accountIDs)
                {
                    if (!String.IsNullOrEmpty(id) && CashDataManager.GetInstance().GetCashPreferences(Int32.Parse(id)) != null)
                    {
                        DateTime accountCashManagementStartDate = CashDataManager.GetInstance().GetCashPreferences(Int32.Parse(id)).CashMgmtStartDate;
                        string accountName = CachedDataManager.GetInstance.GetAccountText(Int32.Parse(id));
                        if (accountCashManagementStartDate > udtBalanceDate.DateTime)
                        {
                            dictAccountDate[accountName] = accountCashManagementStartDate;
                        }
                    }
                }
                if (dictAccountDate.Count > 0)
                {
                    StringBuilder messageString = new StringBuilder();
                    if (dictAccountDate.Count == accountIDs.Length)
                    {
                        messageString.Append("None of the Selected accounts has a date greater than the Cash Management Start date. Please select an appropriate Date.");
                        MessageBox.Show(messageString.ToString());
                        return true;
                    }
                    else
                    {
                        messageString.Append("The following accounts have a date less than the Cash management start date\n");
                        foreach (KeyValuePair<string, DateTime> kvp in dictAccountDate)
                        {
                            messageString.Append(kvp.Key + " (" + kvp.Value + ")\n");
                        }
                        MessageBox.Show(messageString.ToString());
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
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bgwCalcAndGetAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //DateTime balDate = udtBalanceDate.DateTime;
                CalculateAccountBalances();
                object[] arguments = e.Argument as object[];
                _dataFromDB = CashDataManager.GetInstance().GetAccountBalancesAsOnDate((DateTime)arguments[0], (string)arguments[1]);
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

        void bgwOnlyGetAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] arguments = e.Argument as object[];
                _dataFromDB = CashDataManager.GetInstance().GetAccountBalancesAsOnDate((DateTime)arguments[0], (string)arguments[1]);
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
        /// Method to check if the account balances and/or revaluation data is obsolete
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-4202
        /// </summary>
        /// <returns>true if obsolete, false otherwise</returns>
        private bool IsObsoleteData()
        {
            Dictionary<int, RevaluationUpdateDetail> dictRevaluationDates = CashDataManager.GetInstance().GetLastCalculationRevaluationDate();
            int[] accounts = ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',').Split(',').Select(account => Convert.ToInt32(account)).ToArray();
            foreach (int account in accounts)
            {
                if (dictRevaluationDates.ContainsKey(account))
                {
                    if ((DateTime.Compare(udtBalanceDate.DateTime.Date, dictRevaluationDates[account].LastRevaluationDate.Date) > 0)
                        || (DateTime.Compare(udtBalanceDate.DateTime.Date, dictRevaluationDates[account].LastRevaluationDate.Date) == 0 && !dictRevaluationDates[account].isUpdatedReval))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        void bgwCalcAndGetAsync_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (_dataFromDB != null)
                {
                    if (!this.IsDisposed)
                    {
                        this.grdAccBalances.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
                        grdAccBalances.DataSource = _dataFromDB;
                    }
                }
                ChangeStatus(true);
                InitializeGridLayout(AccountsChartBalancesLayout, grdAccBalances);
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

        void bgwOnlyGetAsync_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                bgwCalcAndGetAsync_RunWorkerCompleted(sender, e);
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

        private void grdAccBalances_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraWinGridUtils.EnableFixedFilterRow(e);
                foreach (UltraGridColumn col in e.Layout.Bands[0].Columns)
                {
                    if (col.Key.ToUpper().Contains("ID"))
                    {
                        col.Hidden = true;
                        col.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    }
                    else if (col.DataType == typeof(System.String))
                    {
                        col.CellAppearance.TextHAlign = HAlign.Left;
                        col.CellAppearance.TextTrimming = TextTrimming.EllipsisCharacter;
                    }
                    col.PerformAutoResize();
                }
                e.Layout.Bands[0].Columns["SubCategoryName"].Hidden = true;
                e.Layout.Bands[0].Columns["SubCategoryName"].Header.Caption = "SubCategory A/c";

                e.Layout.Bands[0].Columns["OpenBalDate"].Hidden = true;
                e.Layout.Bands[0].Columns["OpenBalDate"].Header.Caption = "Opening Balance Date";

                e.Layout.Bands[0].Columns["TransactionDate"].Hidden = true;

                e.Layout.Bands[0].Columns["OpenDrBal"].Hidden = true;
                e.Layout.Bands[0].Columns["OpenDrBal"].Header.Caption = "Opening Balance (Dr)";

                e.Layout.Bands[0].Columns["OpenCrBal"].Hidden = true;
                e.Layout.Bands[0].Columns["OpenCrBal"].Header.Caption = "Opening Balance (Cr)";

                e.Layout.Bands[0].Columns["OpenDrBalBase"].Hidden = true;
                e.Layout.Bands[0].Columns["OpenDrBalBase"].Header.Caption = "Opening Balance Base (Dr)";

                e.Layout.Bands[0].Columns["OpenCrBalBase"].Hidden = true;
                e.Layout.Bands[0].Columns["OpenCrBalBase"].Header.Caption = "Opening Balance Base (Cr)";

                e.Layout.Bands[0].Columns["DayDr"].Hidden = true;
                e.Layout.Bands[0].Columns["DayDr"].Header.Caption = "Day Balance (Dr)";

                e.Layout.Bands[0].Columns["DayCr"].Hidden = true;
                e.Layout.Bands[0].Columns["DayCr"].Header.Caption = "Day Balance (Cr)";

                e.Layout.Bands[0].Columns["DayDrBase"].Hidden = true;
                e.Layout.Bands[0].Columns["DayDrBase"].Header.Caption = "Day Balance Base (Dr)";

                e.Layout.Bands[0].Columns["DayCrBase"].Hidden = true;
                e.Layout.Bands[0].Columns["DayCrBase"].Header.Caption = "Day Balance Base (Cr)";

                e.Layout.Bands[0].Columns["BaseCurrency"].Hidden = true;
                e.Layout.Bands[0].Columns["BaseCurrency"].Header.Caption = "Base Currency";

                e.Layout.Bands[0].Columns["FundName"].Header.Caption = "Account";
                e.Layout.Bands[0].Columns["CloseDrBal"].Header.Caption = "Closing Balance (Dr)";
                e.Layout.Bands[0].Columns["CloseDrBal"].Format = "#,###0.00";

                e.Layout.Bands[0].Columns["CloseDrBalBase"].Header.Caption = "Closing Balance Base (Dr)";
                e.Layout.Bands[0].Columns["CloseDrBalBase"].Format = "#,###0.00";

                e.Layout.Bands[0].Columns["CloseCrBal"].Header.Caption = "Closing Balance (Cr)";
                e.Layout.Bands[0].Columns["CloseCrBal"].Format = "#,###0.00";

                e.Layout.Bands[0].Columns["CloseCrBalBase"].Header.Caption = "Closing Balance Base (Cr)";
                e.Layout.Bands[0].Columns["CloseCrBalBase"].Format = "#,###0.00";

                e.Layout.Bands[0].Columns["MasterCategoryName"].Header.Caption = "Master A/c";
                e.Layout.Bands[0].Columns["SubAccName"].Header.Caption = "A/c";
                e.Layout.Bands[0].Columns["TransactionType"].Header.Caption = "A/c Type";
                e.Layout.Bands[0].Columns["CurrencySymbol"].Header.Caption = "Currency";

                e.Layout.Bands[0].Columns["FundName"].Header.VisiblePosition = 0;
                e.Layout.Bands[0].Columns["SubAccName"].Header.VisiblePosition = 1;
                e.Layout.Bands[0].Columns["MasterCategoryName"].Header.VisiblePosition = 2;
                e.Layout.Bands[0].Columns["TransactionType"].Header.VisiblePosition = 3;
                e.Layout.Bands[0].Columns["CurrencySymbol"].Header.VisiblePosition = 4;
                e.Layout.Bands[0].Columns["CloseDrBal"].Header.VisiblePosition = 5;
                e.Layout.Bands[0].Columns["CloseCrBal"].Header.VisiblePosition = 6;
                e.Layout.Bands[0].Columns["CloseDrBalBase"].Header.VisiblePosition = 7;
                e.Layout.Bands[0].Columns["CloseCrBalBase"].Header.VisiblePosition = 8;
                HelperClass.GlobalGridSetting(e.Layout.Bands[0]);
                HelperClass.AccountsSummarySettings(e);
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
        /// Fetches the Transaction details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdAccBalances_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            try
            {
                if (e.Row != null)
                {
                    if (e.Row.IsFilterRow.Equals(true) || e.Row.IsEmptyRow.Equals(true) || e.Row.IsGroupByRow)
                    {
                        return;
                    }
                    int subAccountID = (int)e.Row.Cells["SubAccountID"].Value;
                    DateTime startDate = (DateTime)e.Row.Cells["TransactionDate"].Value;
                    DateTime endDate = (DateTime)e.Row.Cells["TransactionDate"].Value;

                    SetAccDetailsUIDisplay(subAccountID, startDate, endDate);
                    tbcAccountsChart.SelectedTab = this.tbcAccountsChart.Tabs["tbAccDetails"];
                    tbcAccountsChart.Select();
                    GetSubAccountTransactionEntriesForDateRange(subAccountID, startDate, endDate);
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

        private void btnExportBalances_Click(object sender, EventArgs e)
        {
            ExportDataToExcel(grdAccBalances, "Account Balances");
        }

        #endregion

        #region Account Details tab

        private void SetAccDetailsUIDisplay(int subAccountID, DateTime fromDate, DateTime toDate)
        {
            cmbAccounts.Value = subAccountID;
            udtFromDate.DateTime = fromDate;
            udtToDate.DateTime = toDate;
        }

        private void LoadSubAccountCombo()
        {
            try
            {
                ValueList valueList = CashAccountCache.GetInstance.GetAllSubAccounts();
                foreach (ValueListItem item in valueList.ValueListItems)
                {
                    cmbAccounts.Items.Add(item.DataValue, item.DisplayText);
                }
                /// Loaded the combo box with the name of subaccounts.

                cmbAccounts.DisplayMember = "Name";
                cmbAccounts.ValueMember = "SubAccountID";
                cmbAccounts.SortStyle = ValueListSortStyle.Ascending;
                cmbAccounts.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;

                // If event is wired, first unwire it and then wire it back.
                cmbAccounts.ItemNotInList -= new Infragistics.Win.UltraWinEditors.UltraComboEditor.ItemNotInListEventHandler(cmbAccounts_ItemNotInList);
                cmbAccounts.ItemNotInList += new Infragistics.Win.UltraWinEditors.UltraComboEditor.ItemNotInListEventHandler(cmbAccounts_ItemNotInList);
                cmbAccounts.SelectedIndex = 0;
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

        void cmbAccounts_ItemNotInList(object sender, Infragistics.Win.UltraWinEditors.ValidationErrorEventArgs e)
        {
            try
            {
                // The ItemNotInList event is fired before the Validating event of the UltraComboEditor whenever
                //     the text value entered into the editor portion of the control is not a value in the control's
                // ValueList.  The event passes a ValidationErrorEventArgs object that contains InvalidText
                // and LastValidValue properties as well as properties for specifying that the UltraComboEditor
                // should retain focus or beep to provide an auditory cue.

                // Specifies whether the control will retain focus. Overrides the UltraComboEditor's LimitToList
                // property if RetainFocus is set to false.
                e.RetainFocus = true;

                // Provide an auditory cue that the enetered text is invalid.  If a message box is used this will be
                // unnecesary since the message box will provide a beep.
                e.Beep = true;

                // Display a message box indicating that the entered text is invalid.
                MessageBox.Show(e.InvalidText + " is not a valid value.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Restore a previously valid value if one was entered, otherwise set a valid default value.
                if (e.LastValidValue != null)
                    this.cmbAccounts.Value = e.LastValidValue;
                else
                    this.cmbAccounts.SelectedItem = this.cmbAccounts.Items[0];
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
        /// Gets the transaction for a subaccount between from and to date along with opening and closing balance.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetAccountDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Compare(Convert.ToDateTime(udtToDate.DateTime), Convert.ToDateTime(udtFromDate.DateTime)) >= 0)
                {
                    int subAccountID = (int)cmbAccounts.Value;
                    DateTime fromDate = udtFromDate.DateTime;
                    DateTime toDate = udtToDate.DateTime;
                    ugbxAccountDetailsParams.Enabled = false;
                    GetSubAccountTransactionEntriesForDateRange(subAccountID, fromDate, toDate);
                }
                else
                    MessageBox.Show("To Date is before From Date", "Chart Of CashAccounts", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                ugbxAccountDetailsParams.Enabled = true;
            }
        }

        /// <summary>
        /// Gets and display the subaccount transaction entries for the given date range.
        /// </summary>
        /// <param name="subAccountID"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        private void GetSubAccountTransactionEntriesForDateRange(int subAccountID, DateTime fromDate, DateTime toDate)
        {
            try
            {
                grdAccDetails.DataSource = CashDataManager.GetInstance().GetSubAccountTransactionEntriesForDateRange(subAccountID, fromDate, toDate);
                InitializeGridLayout(AccountsChartDetailsLayout, grdAccDetails);
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

        private void btnExportDetails_Click(object sender, EventArgs e)
        {
            try
            {
                ExportDataToExcel(grdAccDetails, "Account Details");
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

        private void grdAccDetails_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                grdAccDetails.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                UltraWinGridUtils.EnableFixedFilterRow(e);

                foreach (UltraGridColumn col in e.Layout.Bands[0].Columns)
                {
                    if (col.Key.ToUpper().Contains("ID") && !col.Key.ToUpper().Contains("TRANSACTION"))
                    {
                        col.Hidden = true;
                        col.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    }
                    else if (col.DataType == typeof(System.String))
                    {
                        col.CellAppearance.TextHAlign = HAlign.Left;
                        col.CellAppearance.TextTrimming = TextTrimming.EllipsisCharacter;
                    }
                    col.PerformAutoResize();
                }
                e.Layout.Bands[0].Columns["FundName"].Header.Caption = "Account";
                e.Layout.Bands[0].Columns["CurrencySymbol"].Header.Caption = "Currency";
                e.Layout.Bands[0].Columns["TransactionDate"].Header.Caption = "Date";
                e.Layout.Bands[0].Columns["DR"].Header.Caption = "Debit";
                e.Layout.Bands[0].Columns["DR"].Format = "#,###0.00";

                e.Layout.Bands[0].Columns["CR"].Header.Caption = "Credit";
                e.Layout.Bands[0].Columns["CR"].Format = "#,###0.00";

                e.Layout.Bands[0].Columns["TransactionType"].Header.Caption = "A/C Type";
                e.Layout.Bands[0].Columns["CurrencySymbol"].Header.Caption = "Currency";

                e.Layout.Bands[0].Columns["TransactionEntryID"].Hidden = true;
                e.Layout.Bands[0].Columns["TransactionID"].Hidden = true;

                e.Layout.Bands[0].Columns["TransactionDate"].Header.VisiblePosition = 0;
                e.Layout.Bands[0].Columns["FundName"].Header.VisiblePosition = 1;
                e.Layout.Bands[0].Columns["TransactionType"].Header.VisiblePosition = 2;
                e.Layout.Bands[0].Columns["CurrencySymbol"].Header.VisiblePosition = 3;
                e.Layout.Bands[0].Columns["DR"].Header.VisiblePosition = 4;
                e.Layout.Bands[0].Columns["CR"].Header.VisiblePosition = 5;
                HelperClass.GlobalGridSetting(e.Layout.Bands[0]);
                this.grdAccDetails.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
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
        #endregion

        #region Common code
        private void ExportDataToExcel(UltraGrid ultraGrid, string sheetName)
        {
            ExcelAndPrintUtilities excelAndPrintUtilities = new ExcelAndPrintUtilities();
            List<UltraGrid> grd = new List<UltraGrid>();
            grd.Add(ultraGrid);
            excelAndPrintUtilities.ExportToExcel(grd, sheetName, true);
        }
        #endregion

        private async void btnRunRevaluation_Click(object sender, EventArgs e)
        {
            try
            {
                bool _isGetAccountBalancesClicked = false;
                string accountIds = ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',');
                if (!string.IsNullOrEmpty(accountIds) && CachedDataManager.GetInstance.NAVLockDate.HasValue)
                {
                    var accountWiseRevalutaionDates = CashDataManager.GetInstance().GetLastCalculationRevaluationDate();
                    foreach (var accountId in accountIds.Split(','))
                    {
                        var id = int.Parse(accountId);
                        if (accountWiseRevalutaionDates.ContainsKey(id) && !CachedDataManager.GetInstance.ValidateNAVLockDate(accountWiseRevalutaionDates[id].LastRevaluationDate))
                        {
                            MessageBox.Show("The last modified date  for some of the accounts you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                                + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
                string invalidFundNames = CashDataManager.GetInstance().GetInvalidFundsForSymbolLevel(accountIds, null, udtBalanceDate.DateTime);
                if (!string.IsNullOrEmpty(invalidFundNames))
                {
                    DialogResult result1 = MessageBox.Show("Changes have been made prior to the accrual balance start date for " + invalidFundNames + " . Running revaluation may cause mismatches in the accrual balance. Please confirm you want to continue. If you are unsure of these changes please select 'No' and reach out to your Nirvana support representative", "Incorrect Revaluation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result1 == DialogResult.Yes)
                    {
                        Logger.LoggerWrite("Revaluation run prior to Symbol wise accrual date for funds:" + invalidFundNames, "Revaluation Run");

                        DialogResult dlgResult = new DialogResult();
                        if (sender != null)
                        {
                            if (String.IsNullOrEmpty(accountIds))
                            {
                                MessageBox.Show("Select at least one account to proceed.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ctrlMasterFundAndAccountsDropdown1.MultiAccounts.Focus();
                                return;
                            }
                            if (ConflictedStartDateWithAccounts())
                            {
                                return;
                            }
                            if (IsObsoleteData())
                            {
                                dlgResult = ConfirmationMessageBox.Display("Revaluation process is a time and resource consuming process, do you want to continue?", "Revaluation");
                                runManualRevaluation = false;
                                _isGetAccountBalancesClicked = false;
                            }
                            else
                            {
                                toolStripAccountsChart.Text = "Revaluation has already completed for selected funds.";
                                return;
                            }
                        }
                        else
                        {
                            dlgResult = DialogResult.Yes;
                            runManualRevaluation = false;
                            _isGetAccountBalancesClicked = true;
                        }
                        if (dlgResult == DialogResult.Yes)
                        {
                            if (!CashDataManager.GetInstance().GetIsRevaluationInProgress(accountIds))
                            {
                                toolStripAccountsChart.Text = "Running Revaluation Process...";
                                if (!CustomThemeHelper.ApplyTheme)
                                {
                                    toolStripAccountsChart.ForeColor = System.Drawing.Color.DarkGreen;
                                }
                                ugbxAccountBalParams.Enabled = false;
                                //http://jira.nirvanasolutions.com:8080/browse/PRANA-7360
                                string userName = CachedDataManager.GetInstance.LoggedInUser.LoginID;
                                Logger.LoggerWrite("Revaluation Process Starts... Time: " + DateTime.UtcNow + "   Account Id: " + accountIds + "   User Name: " + userName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                                DateTime revaluationStartTime = DateTime.Now;
                                bool result = await RunRevaluation(runManualRevaluation, _isGetAccountBalancesClicked);
                                if (result)
                                {
                                    if (!this.IsDisposed)
                                    {
                                        toolStripProgressBar1.Value = 100;  // this is used as progress takes integer value of progress.so it shows approximate but not accruate Progress. So to adjust to 100 at the end.
                                        toolStripAccountsChart.Text = "Revaluation process completed";
                                        if (!CustomThemeHelper.ApplyTheme)
                                            toolStripAccountsChart.ForeColor = System.Drawing.Color.Green;
                                    }


                                    Logger.LoggerWrite("Revaluation Process Completed... Time: " + DateTime.UtcNow + "   Fund Id: " + accountIds + "   User Name: " + userName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                                    Logger.LoggerWrite("Total time taken by revaluation process:" + Convert.ToString(DateTime.Now - revaluationStartTime), LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

                                    if (sender == null) //Call From Get Account balances Button
                                    {
                                        BackgroundWorker bgwOnlyGetAsync = new BackgroundWorker();
                                        bgwOnlyGetAsync.DoWork += new DoWorkEventHandler(bgwOnlyGetAsync_DoWork);
                                        bgwOnlyGetAsync.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwOnlyGetAsync_RunWorkerCompleted);

                                        ChangeStatus(false);
                                        bgwOnlyGetAsync.RunWorkerAsync(new object[] { udtBalanceDate.DateTime, ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',') });
                                    }
                                }
                                else
                                {
                                    if (!this.IsDisposed)
                                    {
                                        toolStripProgressBar1.Visible = false;
                                        toolStripAccountsChart.Text = "Revaluation Process Failed";
                                        if (!CustomThemeHelper.ApplyTheme)
                                            toolStripAccountsChart.ForeColor = System.Drawing.Color.Red;
                                        Logger.LoggerWrite("Revaluation process failed... Time: " + DateTime.UtcNow + "   Account Id: " + accountIds + "   User Name: " + userName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                                        Logger.LoggerWrite("Total time taken by revaluation process(failed):" + Convert.ToString(DateTime.Now - revaluationStartTime), LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                                    }
                                }
                                ugbxAccountBalParams.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Revaluation is in progress for selected fund(s). Proceed after revaluation is done.", "Revaluation ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            toolStripProgressBar1.Visible = false;
                            toolStripAccountsChart.Text = "Revaluation Process canceled";
                            Logger.LoggerWrite("Revaluation Process Canceled... Time: " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                            if (!CustomThemeHelper.ApplyTheme)
                            {
                                toolStripAccountsChart.ForeColor = System.Drawing.Color.Blue;
                            }
                        }
                    }
                }
                else
                {
                    DialogResult dlgResult = new DialogResult();
                    if (sender != null)
                    {
                        if (String.IsNullOrEmpty(accountIds))
                        {
                            MessageBox.Show("Select at least one account to proceed.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ctrlMasterFundAndAccountsDropdown1.MultiAccounts.Focus();
                            return;
                        }
                        if (ConflictedStartDateWithAccounts())
                        {
                            return;
                        }
                        if (IsObsoleteData())
                        {
                            dlgResult = ConfirmationMessageBox.Display("Revaluation process is a time and resource consuming process, do you want to continue?", "Revaluation");
                            runManualRevaluation = false;
                            _isGetAccountBalancesClicked = false;
                        }
                        else
                        {
                            toolStripAccountsChart.Text = "Revaluation has already completed for selected funds.";
                            return;
                        }
                    }
                    else
                    {
                        dlgResult = DialogResult.Yes;
                        runManualRevaluation = false;
                        _isGetAccountBalancesClicked = true;
                    }
                    if (dlgResult == DialogResult.Yes)
                    {
                        if (!CashDataManager.GetInstance().GetIsRevaluationInProgress(accountIds))
                        {
                            toolStripAccountsChart.Text = "Running Revaluation Process...";
                            if (!CustomThemeHelper.ApplyTheme)
                            {
                                toolStripAccountsChart.ForeColor = System.Drawing.Color.DarkGreen;
                            }
                            ugbxAccountBalParams.Enabled = false;
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-7360
                            string userName = CachedDataManager.GetInstance.LoggedInUser.LoginID;
                            Logger.LoggerWrite("Revaluation Process Starts... Time: " + DateTime.UtcNow + "   Account Id: " + accountIds + "   User Name: " + userName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                            DateTime revaluationStartTime = DateTime.Now;
                            bool result = await RunRevaluation(runManualRevaluation, _isGetAccountBalancesClicked);
                            if (result)
                            {
                                if (!this.IsDisposed)
                                {
                                    toolStripProgressBar1.Value = 100;  // this is used as progress takes integer value of progress.so it shows approximate but not accruate Progress. So to adjust to 100 at the end.
                                    toolStripAccountsChart.Text = "Revaluation process completed";
                                    if (!CustomThemeHelper.ApplyTheme)
                                        toolStripAccountsChart.ForeColor = System.Drawing.Color.Green;
                                }


                                Logger.LoggerWrite("Revaluation Process Completed... Time: " + DateTime.UtcNow + "   Fund Id: " + accountIds + "   User Name: " + userName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                                Logger.LoggerWrite("Total time taken by revaluation process:" + Convert.ToString(DateTime.Now - revaluationStartTime), LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

                                if (sender == null) //Call From Get Account balances Button
                                {
                                    BackgroundWorker bgwOnlyGetAsync = new BackgroundWorker();
                                    bgwOnlyGetAsync.DoWork += new DoWorkEventHandler(bgwOnlyGetAsync_DoWork);
                                    bgwOnlyGetAsync.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwOnlyGetAsync_RunWorkerCompleted);

                                    ChangeStatus(false);
                                    bgwOnlyGetAsync.RunWorkerAsync(new object[] { udtBalanceDate.DateTime, ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',') });
                                }
                            }
                            else
                            {
                                if (!this.IsDisposed)
                                {
                                    toolStripProgressBar1.Visible = false;
                                    toolStripAccountsChart.Text = "Revaluation Process Failed";
                                    if (!CustomThemeHelper.ApplyTheme)
                                        toolStripAccountsChart.ForeColor = System.Drawing.Color.Red;
                                    Logger.LoggerWrite("Revaluation process failed... Time: " + DateTime.UtcNow + "   Account Id: " + accountIds + "   User Name: " + userName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                                    Logger.LoggerWrite("Total time taken by revaluation process(failed):" + Convert.ToString(DateTime.Now - revaluationStartTime), LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                                }
                            }
                            ugbxAccountBalParams.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("Revaluation is in progress for selected fund(s). Proceed after revaluation is done.", "Revaluation ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        toolStripProgressBar1.Visible = false;
                        toolStripAccountsChart.Text = "Revaluation Process canceled";
                        Logger.LoggerWrite("Revaluation Process Canceled... Time: " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                        if (!CustomThemeHelper.ApplyTheme)
                        {
                            toolStripAccountsChart.ForeColor = System.Drawing.Color.Blue;
                        }
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

        private async Task<bool> RunRevaluation(bool runManualRevaluation, bool _isGetAccountBalancesClicked)
        {
            bool result = false;
            try
            {
                toolStripProgressBar1.Visible = true;
                toolStripProgressBar1.Value = 0;
                DateTime balDate = udtBalanceDate.DateTime.Add(DateTime.Now.TimeOfDay);

                //Incase of Normal Revaluation fromDate is null as for that we need only endDate.

                Logger.LoggerWrite("RunRevaluationProcess Starts... Time: " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                var revaluationStartTime = DateTime.Now;
                result = await CashDataManager.GetInstance().RunRevaluationProcess(null, balDate, ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(','), runManualRevaluation, _isGetAccountBalancesClicked, LoggingConstants.RUN_REVAL);

                // Merge these 2 Messages together
                Logger.LoggerWrite("RunRevaluationProcess Complete. Total time taken by RunRevaluationProcess:" + Convert.ToString(DateTime.Now - revaluationStartTime), LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

                if (result)
                {
                    Logger.LoggerWrite("GetAccountBalancesAsOnDate Starts... Time: " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                    var getAccountBalancesStartTime = DateTime.Now;
                    _dataFromDB = CashDataManager.GetInstance().GetAccountBalancesAsOnDate(balDate, ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(','));
                    Logger.LoggerWrite("GetAccountBalancesAsOnDate Complete. Total time taken by GetAccountBalancesAsOnDate process:" + Convert.ToString(DateTime.Now - getAccountBalancesStartTime), LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

                    Logger.LoggerWrite("RunRevaluation and GetAccountBalancesAsOnDate Complete. Total time taken is:" + Convert.ToString(DateTime.Now - revaluationStartTime), LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

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
            return result;
        }

        private void ctrlAccountsChart_Load(object sender, EventArgs e)
        {
            try
            {
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    this.statusStrip1.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripAccountsChart.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    this.toolStripAccountsChart.ForeColor = System.Drawing.Color.WhiteSmoke;
                }
                Dictionary<int, RevaluationUpdateDetail> dictRevalDates = CashDataManager.GetInstance().GetLastCalculationRevaluationDate();
                ctrlMasterFundAndAccountsDropdown1.Setup();
                ctrlMasterFundAndAccountsDropdown1.UnWireEvents();
                ctrlMasterFundAndAccountsDropdown1.UpdateFundDropDown(dictRevalDates);
                ctrlMasterFundAndAccountsDropdown1.WireEvents();
                InitializeGridLayout(AccountsChartBalancesLayout, grdAccBalances);
                InitializeGridLayout(accountsChartDetailsLayout, grdAccDetails);
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

        private void grdAccBalances_AfterSortChange(object sender, BandEventArgs e)
        {
            if (grdAccBalances.Rows.IsGroupByRows)
            {
                grdAccBalances.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.InGroupByRows;
            }
            else
            {
                grdAccBalances.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
            }
        }

        private void grdAccBalances_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            HelperClass.GroupByRowSetting(e);
        }

        #region IPublishing Members

        public void Publish(MessageData e, string topicName)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (InvokeRequired)
                    {
                        MethodInvoker del =
                            delegate
                            {
                                Publish(e, topicName);
                            };
                        this.BeginInvoke(del);
                        return;
                    }

                    System.Object[] publishDataList = null;
                    System.Object[] publishRevaluationDate = null;
                    switch (topicName)
                    {
                        case Topics.Topic_RevaluationProgress:
                            publishDataList = (System.Object[])e.EventData;
                            if (!(int.Parse(publishDataList.GetValue(0).ToString()) == CachedDataManager.GetInstance.LoggedInUser.CompanyUserID))
                            {
                                break;
                            }
                            int percentage;
                            bool result = Int32.TryParse(publishDataList.GetValue(1).ToString(), out percentage);
                            if (result)
                            {
                                toolStripProgressBar1.Value = percentage;
                                toolStripProgressBar1.ProgressBar.Refresh();
                                if (percentage == 100)
                                {
                                    toolStripAccountsChart.Text = "Revaluation process completed";
                                }
                                else
                                {
                                    using (Graphics gr = toolStripProgressBar1.ProgressBar.CreateGraphics())
                                    {
                                        gr.DrawString(percentage.ToString() + "%",
                                            SystemFonts.DefaultFont,
                                            Brushes.Black,
                                         new PointF(toolStripProgressBar1.Width / 2 - (gr.MeasureString(percentage.ToString() + "%",
                                             SystemFonts.DefaultFont).Width / 2.0F),
                                            toolStripProgressBar1.Height / 2 - (gr.MeasureString(percentage.ToString() + "%",
                                                SystemFonts.DefaultFont).Height / 2.0F)));
                                    }
                                }
                            }
                            else
                            {
                                if (!(publishDataList.GetValue(1).ToString().Contains("eliminated")))
                                {
                                    toolStripAccountsChart.Text = publishDataList.GetValue(1).ToString();
                                    toolStripAccountsChart.ToolTipText = toolStripAccountsChart.ToolTipText + publishDataList.GetValue(1).ToString();
                                }
                            }
                            break;

                        case Topics.Topic_RevaluationDate:
                            publishRevaluationDate = (System.Object[])e.EventData;
                            DateTime date = (DateTime)publishRevaluationDate.GetValue(0);
                            string accountId = (string)publishRevaluationDate.GetValue(1);
                            {
                                List<int> tagAccount = accountId.Split(',').Select(int.Parse).ToList();
                                ctrlMasterFundAndAccountsDropdown1.UpdateLastFundDropDown(date, tagAccount);
                            }
                            break;
                        default:
                            break;
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

        public string getReceiverUniqueName()
        {
            return "ctrlAccountChart";
        }
        #endregion

        private void grdAccBalances_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdAccBalances);
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

        private void grdAccDetails_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdAccDetails);
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

        //Added by Nishant Jain, 04-08-2015 
        private void grdAccDetails_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            try
            {
                if (e.Row.ListObject == null)
                {
                    return;
                }

                if (transactionDetails != null)
                {
                    transactionDetails.Close();
                }
                transactionDetails = new frmAccountTransactionDetails();
                transactionDetails.FormClosing += new FormClosingEventHandler(transactionDetails_FormClosing);
                transactionDetails.SetUp(Convert.ToString(e.Row.Cells["TransactionID"].Value));
                transactionDetails.Show();
                transactionDetails.BringToFront();
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

        void transactionDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                transactionDetails.FormClosing -= new FormClosingEventHandler(transactionDetails_FormClosing);
                transactionDetails = null;
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

        #region Save Layout
        private const string AccountBalances = "ChartOfAccounts_AccountBalances";
        private const string AccountDetails = "ChartOfAccounts_AccountDetails";

        private CashManagementLayout accountsChartBalancesLayout;
        public CashManagementLayout AccountsChartBalancesLayout
        {
            get { return accountsChartBalancesLayout; }
            set { accountsChartBalancesLayout = value; }
        }

        private CashManagementLayout accountsChartDetailsLayout;
        public CashManagementLayout AccountsChartDetailsLayout
        {
            get { return accountsChartDetailsLayout; }
            set { accountsChartDetailsLayout = value; }
        }

        public void InitializeGridLayout(CashManagementLayout cashManagementLayout, UltraGrid grd)
        {
            try
            {
                if (cashManagementLayout != null)
                {
                    if (cashManagementLayout.GroupByColumnsCollection.Count > 0)
                    {
                        //Set GroupBy Columns
                        bool flag = true;
                        int GroupedColumns = grd.DisplayLayout.Bands[0].SortedColumns.Count;
                        for (int i = 0; i < GroupedColumns - 1; i++)
                        {
                            grd.DisplayLayout.Bands[0].SortedColumns.Remove(0);
                        }

                        foreach (string item in cashManagementLayout.GroupByColumnsCollection)
                        {
                            if (grd.DisplayLayout.Bands[0].Columns.Exists(item) && !grd.DisplayLayout.Bands[0].SortedColumns.Contains(item))
                            {
                                grd.DisplayLayout.Bands[0].SortedColumns.Add(item, true, true);
                                if (grd.DisplayLayout.Bands[0].SortedColumns.Count == 1)
                                {
                                    flag = false;
                                }
                                if (flag)
                                {
                                    grd.DisplayLayout.Bands[0].SortedColumns.Remove(0);
                                    flag = false;
                                }
                            }
                        }
                        grd.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                        grd.DisplayLayout.RefreshSummaries();
                    }

                    ColumnFiltersCollection columnFilters = grd.DisplayLayout.Bands[0].ColumnFilters;
                    ///TODO : When we apply the custom filters we need to change the code so the filters won't be on a common field
                    columnFilters.ClearAllFilters();

                    foreach (UltraGridColumn col in grd.DisplayLayout.Bands[0].Columns)
                    {
                        col.Hidden = true;
                    }
                    foreach (CashGridColumn selectedCols in cashManagementLayout.SelectedColumnsCollection)
                    {
                        UltraGridColumn col = null;
                        if (grd.DisplayLayout.Bands[0].Columns.Exists(selectedCols.Name))
                        {
                            col = grd.DisplayLayout.Bands[0].Columns[selectedCols.Name];
                            if (col != null)
                            {
                                col.HiddenWhenGroupBy = DefaultableBoolean.False;
                                col.Hidden = selectedCols.Hidden;
                                col.Header.Fixed = selectedCols.IsHeaderFixed;
                                if (!grd.DisplayLayout.Bands[0].SortedColumns.Contains(col))
                                {
                                    col.SortIndicator = selectedCols.SortIndicator;
                                    if (col.SortIndicator.Equals(SortIndicator.Ascending))

                                        grd.DisplayLayout.Bands[0].SortedColumns.Add(col, false);
                                    if (col.SortIndicator.Equals(SortIndicator.Descending))
                                        grd.DisplayLayout.Bands[0].SortedColumns.Add(col, true);
                                }
                                grd.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                                if (selectedCols.Width == 0)
                                {
                                    col.AutoSizeMode = ColumnAutoSizeMode.Default;
                                }
                                else
                                {
                                    col.Width = selectedCols.Width;
                                }
                                col.Header.VisiblePosition = selectedCols.Position;
                                if (selectedCols.FilterConditionList.Count > 0)
                                {
                                    foreach (FilterCondition filCond in selectedCols.FilterConditionList)
                                    {
                                        grd.DisplayLayout.Bands[0].ColumnFilters[col].FilterConditions.Add(filCond);
                                    }
                                    grd.DisplayLayout.Bands[0].ColumnFilters[col].LogicalOperator = selectedCols.FilterLogicalOperator;
                                }
                            }
                        }
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

        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveGridLayout saveGridLayout = new SaveGridLayout();
                if (tbcAccountsChart.SelectedTab.TabPage == tbAccountBalance)
                {
                    CashManagementLayout cashManagementLayout = saveGridLayout.GetLayout(grdAccBalances, "ChartOfAccounts");
                    CashPreferenceManager.GetInstance().SetCashGridLayout(cashManagementLayout, AccountBalances);
                    AccountsChartBalancesLayout = cashManagementLayout;
                }
                else
                {
                    CashManagementLayout cashManagementLayout = saveGridLayout.GetLayout(grdAccDetails, "ChartOfAccounts");
                    CashPreferenceManager.GetInstance().SetCashGridLayout(cashManagementLayout, AccountDetails);
                    AccountsChartDetailsLayout = cashManagementLayout;
                }
                saveGridLayout = null;
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
        #endregion

        private void grdAccBalances_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        private void grdAccDetails_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
