using Prana.BusinessObjects;
using Prana.CashManagement.Classes;
using Prana.CashManagement.Controls;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.CashManagement
{
    public partial class frmCashManagementMain : Form, Prana.Interfaces.ICashManagement
    {
        public frmCashManagementMain()
        {
            InitializeComponent();
            //added by: Bharat raturi, 30 sep 2014
            //Lock the toolbar
            //http://jira.nirvanasolutions.com:8080/browse/PRANA-4950
            ultraToolbarsManager1.LockToolbars = true;
            Disposed += new EventHandler(frmCashManagementMain_Disposed);
        }

        #region ICashManagement Members

        void frmCashManagementMain_Disposed(object sender, EventArgs e)
        {
            if (FormClosedHandler != null)
            {
                FormClosedHandler(this, e);
            }
        }

        public Form Reference()
        {
            return this;
        }

        public event EventHandler FormClosedHandler;
        public event EventHandler LaunchAccountSetUpUI;
        public event EventHandler LaunchActivitySetUpUI;
        ctrlDailyCalc _dailyCalculationControl;
        ctrlJournal _cashJournalControl;
        ctrlAccountsChart _ctrlAccountsChart = null;
        ctrlEditableActivity _ctrlActivity;
        ctrlCashForm _cashControl;
        ctrlJournalExceptions _ctrlJournalException;
        ctrlActivityExceptions _ctrlActivityException;
        ctrlCashTransactions _cashTransaction;
        public void SetUp()
        {
            try
            {

                _ctrlActivity = new ctrlEditableActivity();
                _ctrlActivity.Dock = DockStyle.Fill;
                this.tbCtrlMain.Tabs["tbActivity"].TabPage.Controls.Add(_ctrlActivity);

                _dailyCalculationControl = new ctrlDailyCalc();
                _dailyCalculationControl.Dock = DockStyle.Fill;
                this.tbCtrlMain.Tabs["tbDailyCalc"].TabPage.Controls.Add(_dailyCalculationControl);

                _cashJournalControl = new ctrlJournal();
                _cashJournalControl.Dock = DockStyle.Fill;
                this.tbCtrlMain.Tabs["tbCashJrnl"].TabPage.Controls.Add(_cashJournalControl);

                _ctrlAccountsChart = new ctrlAccountsChart();
                _ctrlAccountsChart.Dock = DockStyle.Fill;
                _ctrlAccountsChart.SetUp();
                this.tbCtrlMain.Tabs["tbAccountsChart"].TabPage.Controls.Add(_ctrlAccountsChart);


                _cashControl = new ctrlCashForm(loginUser);
                _cashControl.Dock = System.Windows.Forms.DockStyle.Fill;
                this.tbCtrlMain.Tabs["tbDayEndCash"].TabPage.Controls.Add(_cashControl);

                _ctrlJournalException = new ctrlJournalExceptions();
                _ctrlJournalException.Dock = DockStyle.Fill;
                this.tbCtrlMain.Tabs["tbJournalExceptions"].TabPage.Controls.Add(_ctrlJournalException);

                _ctrlActivityException = new ctrlActivityExceptions();
                _ctrlActivityException.Dock = DockStyle.Fill;
                this.tbCtrlMain.Tabs["tbActivityExceptions"].TabPage.Controls.Add(_ctrlActivityException);
                CashDataManager.GetInstance().SecurityMaster = _securityMaster;

                _cashTransaction = new ctrlCashTransactions();
                _cashTransaction.Dock = System.Windows.Forms.DockStyle.Fill;
                this.tbCtrlMain.Tabs["tbCashTransaction"].TabPage.Controls.Add(_cashTransaction);
                _cashTransaction.SecurityMaster = _securityMaster;
                SetTheme();

                CashPreferenceManager.SetUser(loginUser);
                LoadPreferences();
                //proxies will be created in individual constructor of each control
                //when proxies is made here control are not subscribed untill we click on get data button.
                //CreateSubscriptionProxy();
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

        ISecurityMasterServices _securityMaster = null;
        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                _securityMaster = value;
            }
        }

        public void AddCashTransaction(TaxLot taxlot)
        {
            try
            {
                if (_cashTransaction == null)
                    SetUp();

                this.BringToFront();
                tbCtrlMain.SelectedTab = this.tbCtrlMain.Tabs["tbCashTransaction"];
                _cashTransaction.AddCashTransaction(taxlot);

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

        CompanyUser _loginUser = null;
        public CompanyUser loginUser
        {
            get
            {
                return _loginUser;
            }
            set
            {
                _loginUser = value;
            }
        }
        #endregion

        #region Load Layout Details

        internal const string Activity = "Activity";
        private static CashPreferenceManager _cashPreferenceManager = null;
        CashPreferencesList _cashPreferencesList = null;

        private void LoadPreferences()
        {
            try
            {
                _cashPreferenceManager = new CashPreferenceManager();
                if (_cashPreferenceManager != null)
                {
                    _cashPreferencesList = _cashPreferenceManager.GetLayoutDetails();

                    // Load available layouts over CM UIs
                    foreach (KeyValuePair<string, CashManagementLayout> cashPreference in _cashPreferencesList)
                    {
                        if (cashPreference.Key == "Activity")
                            _ctrlActivity.ActivityLayout = cashPreference.Value;
                        #region Cash Tab
                        else if (cashPreference.Key == "CashJournal_TradingTransaction")
                            _cashJournalControl.Set("CashJournal_TradingTransaction", cashPreference.Value);
                        else if (cashPreference.Key == "CashJournal_NonTradingTransaction")
                            _cashJournalControl.Set("CashJournal_NonTradingTransaction", cashPreference.Value);
                        else if (cashPreference.Key == "CashJournal_Dividend")
                            _cashJournalControl.Set("CashJournal_Dividend", cashPreference.Value);
                        else if (cashPreference.Key == "CashJournal_Revaluation")
                            _cashJournalControl.Set("CashJournal_Revaluation", cashPreference.Value);
                        else if (cashPreference.Key == "CashJournal_OpeningBalance")
                            _cashJournalControl.Set("CashJournal_OpeningBalance", cashPreference.Value);
                        #endregion
                        #region Daily Calculations
                        else if (cashPreference.Key == "DailyCalculations_AllOpenPositions")
                            _dailyCalculationControl.CashManagementLayoutForAllOpenPositions = cashPreference.Value;
                        else if (cashPreference.Key == "DailyCalculations_CalculatedTransactions")
                            _dailyCalculationControl.CashManagementLayoutForCalculatedTransactions = cashPreference.Value;
                        #endregion
                        #region Day End Cash
                        else if (cashPreference.Key == "DayEndCash_LastDayEndCash")
                            _cashControl.AddLayout("DayEndCash_LastDayEndCash", cashPreference.Value);
                        else if (cashPreference.Key == "DayEndCash_TransactionDetails")
                            _cashControl.AddLayout("DayEndCash_TransactionDetails", cashPreference.Value);
                        else if (cashPreference.Key == "DayEndCash_DayEndCash")
                            _cashControl.AddLayout("DayEndCash_DayEndCash", cashPreference.Value);
                        #endregion
                        #region Chart of CashAccounts
                        else if (cashPreference.Key == "ChartOfAccounts_AccountBalances")
                            _ctrlAccountsChart.AccountsChartBalancesLayout = cashPreference.Value;
                        else if (cashPreference.Key == "ChartOfAccounts_AccountDetails")
                            _ctrlAccountsChart.AccountsChartDetailsLayout = cashPreference.Value;
                        #endregion
                        else if (cashPreference.Key == "ActivityExceptions")
                            _ctrlActivityException.ActivityExceptionsLayout = cashPreference.Value;
                        else if (cashPreference.Key == "JournalExceptions")
                            _ctrlJournalException.JournalExceptionsLayout = cashPreference.Value;

                    }
                    _cashJournalControl.SetLayoutForAllCashJournalGrids();
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

        #endregion

        private void tbCtrlMain_ActiveTabChanging(object sender, Infragistics.Win.UltraWinTabControl.ActiveTabChangingEventArgs e)
        {
            try
            {
                if (tbCtrlMain.ActiveTab != null && tbCtrlMain.ActiveTab.Key == "tbDailyCalc")
                    if (_dailyCalculationControl != null && !_dailyCalculationControl.AllowTabChange)
                        e.Cancel = true;
                if (tbCtrlMain.ActiveTab != null && tbCtrlMain.ActiveTab.Key == "tbCashTransaction")
                    _cashTransaction.UpdateActivityTypeValueList();
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

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            try
            {
                if (e.Tool.Key == "AccountSetup" && LaunchAccountSetUpUI != null)
                {
                    LaunchAccountSetUpUI(this, e);
                }
                if (e.Tool.Key == "Snapshot")
                {
                    SnapShotManager.GetInstance().TakeSnapshot(this);
                }
                if (e.Tool.Key == "ActivitySetup" && LaunchActivitySetUpUI != null)
                {
                    LaunchActivitySetUpUI(this, e);
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

        private void frmCashManagementMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_cashJournalControl != null && _cashJournalControl.lsModifiedTransactions != null && _cashJournalControl.lsModifiedTransactions.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to save the changes ?", "Transaction Value Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (dialogResult == DialogResult.Yes)
                {
                    e.Cancel = true;
                    _cashJournalControl.SaveChanges();
                }
            }
        }
        void frmCashManagementMain_Load(object sender, System.EventArgs e)
        {
            SetTheme();
        }
        private void SetTheme()
        {
            try
            {
                if (this.IsHandleCreated)
                {
                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CASH_MANAGEMENT);
                    CustomThemeHelper.SetThemeProperties(_cashJournalControl, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_JOURNAL_PANAL);
                    CustomThemeHelper.SetThemeProperties(_ctrlJournalException, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_JOURNAL_PANAL);
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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

        private void frmCashManagementMain_Resize(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState == FormWindowState.Maximized)
                {
                    this.Height = Screen.PrimaryScreen.Bounds.Height;
                    this.Width = Screen.PrimaryScreen.Bounds.Width;
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