using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CashManagement.Classes;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Utilities.UI;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prana.CashManagement.Controls
{
    public partial class ctrlJournalExceptions : UserControl
    {
        public ctrlJournalExceptions()
        {
            InitializeComponent();
            BindGrid();
        }

        GenericBindingList<TransactionEntry> _lsTransactionEntryToBind = new GenericBindingList<TransactionEntry>();
        List<Transaction> _transactionsFromDB = new List<Transaction>();
        GroupSortComparer _groupSortComparer = new GroupSortComparer();
        UltraGridColumn _columnSorted = null;
        List<string> GroupByColumnsCollection = new List<string>();
        bool IsGetException = false;
        bool IsOverride = false;
        string _journalSource = string.Empty;

        private void BindGrid()
        {
            try
            {
                if (!this.IsDisposed)
                {

                    TransactionEntry trEntryToInitiallizeGrid = new TransactionEntry();
                    if (_lsTransactionEntryToBind.Count == 0)
                        _lsTransactionEntryToBind.Add(trEntryToInitiallizeGrid);

                    grdCashExceptions.DataSource = _lsTransactionEntryToBind;

                    HelperClass.SetColumnDisplayNames(grdCashExceptions, null);
                    HelperClass.GridSettingForJournalLook(grdCashExceptions);
                    _lsTransactionEntryToBind.Clear();
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

        private void ClearData()
        {
            try
            {
                if (_lsTransactionEntryToBind != null)
                    _lsTransactionEntryToBind.Clear();
                if (_transactionsFromDB != null)
                    _transactionsFromDB.Clear();
                //if (_dtDiv != null)
                //    _dtDiv.Clear();
                if (_lsExceptionalTransactions != null)
                    _lsExceptionalTransactions.Clear();

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

        private void ChangeStatus(bool workCompleted, bool isGetExceptions, bool isGetOverridingData)
        {
            if (workCompleted)
            {
                btnGetEx.Text = "Get Exceptions";
                btnSave.Text = "Save";
                ugbxJExcepParams.Enabled = true;
                btnOverriding.Text = "Get Overriding Data";
                toolStripStatusLabel1.Text = "Successfully Completed";
                if (!CustomThemeHelper.ApplyTheme)
                {
                    toolStripStatusLabel1.ForeColor = System.Drawing.Color.DarkBlue;

                }
            }
            else
            {
                if (isGetExceptions)
                {
                    btnGetEx.Text = "Getting...";
                    toolStripStatusLabel1.Text = "Getting Exceptions...";
                    IsGetException = true;
                    IsOverride = false;
                    toolStripProgressBar.Visible = false;
                    if (!CustomThemeHelper.ApplyTheme)
                    {
                        toolStripStatusLabel1.ForeColor = System.Drawing.Color.Green;

                    }
                }
                else if (isGetOverridingData)
                {
                    btnOverriding.Text = "Getting...";
                    toolStripStatusLabel1.Text = "Getting OverridingData...";
                    IsGetException = false;
                    IsOverride = true;
                    toolStripProgressBar.Visible = false;
                    if (!CustomThemeHelper.ApplyTheme)
                    {
                        toolStripStatusLabel1.ForeColor = System.Drawing.Color.Green;

                    }
                }
                else
                {
                    btnSave.Text = "Saving...";
                    toolStripStatusLabel1.Text = "Saving Data...";
                    IsGetException = false;
                    IsOverride = false;
                    toolStripProgressBar.Visible = false;
                    if (!CustomThemeHelper.ApplyTheme)
                    {
                        toolStripStatusLabel1.ForeColor = System.Drawing.Color.Blue;

                    }
                }
                ugbxJExcepParams.Enabled = false;
            }
        }

        #region Getting Exceptions Section

        private void btnGetEx_Click(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Compare(Convert.ToDateTime(dtToDate.DateTime), Convert.ToDateTime(dtFromDate.DateTime)) >= 0)
                {
                    if (ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetNoOfCheckedItems() == 0)
                    {
                        if (MessageBox.Show("Please select at least one account to proceed.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            ctrlMasterFundAndAccountsDropdown1.MultiAccounts.Focus();
                            return;
                        }
                    }
                    ClearData();
                    string accountIds = ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',');
                    if (!CashDataManager.GetInstance().GetIsRevaluationInProgress(accountIds))
                    {
                        _journalSource = "Journals generated using 'Get Exceptions' button";
                        GetExceptionsAsync();
                    }
                    else
                        MessageBox.Show("Revaluation is in progress for selected fund(s). Proceed after revaluation is done.", "Revaluation", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                    MessageBox.Show("To Date is before From Date.", "Journal Exceptions", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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

        private void GetExceptionsAsync()
        {
            try
            {
                BackgroundWorker bgwrkrGetData = new BackgroundWorker();
                bgwrkrGetData.DoWork += new DoWorkEventHandler(bgwrkrGetData_DoWork);
                bgwrkrGetData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwrkrGetData_RunWorkerCompleted);
                ChangeStatus(false, true, false);
                bgwrkrGetData.RunWorkerAsync(new object[] { dtFromDate.DateTime, dtToDate.DateTime, ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',') });
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        //DataTable _dtDiv;
        List<Transaction> _lsExceptionalTransactions;
        void bgwrkrGetData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] arguments = e.Argument as object[];
                _lsExceptionalTransactions = CashDataManager.GetInstance().GetJournalExceptions((DateTime)arguments[0], (DateTime)arguments[1], (string)arguments[2]);
                if (_lsExceptionalTransactions != null && _lsExceptionalTransactions.Count > 0)
                    _transactionsFromDB.AddRange(_lsExceptionalTransactions);
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

        void bgwrkrGetData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                ChangeStatus(true, true, true);
                if (_transactionsFromDB != null)
                {
                    HelperClass.GlobalGridSetting(grdCashExceptions.DisplayLayout.Bands[0]);
                    //Code to bind Transaction Entries to ui instead of Transactions
                    foreach (Transaction tr in _transactionsFromDB)
                    {
                        foreach (TransactionEntry trEntry in tr.TransactionEntries)
                        {
                            if (!_lsTransactionEntryToBind.Contains(trEntry))
                                _lsTransactionEntryToBind.Add(trEntry);
                        }
                    }
                }
                InitializeGridLayout();

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }


        #endregion

        #region overriding Data Section

        private void btnOverriding_Click(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Compare(Convert.ToDateTime(dtToDate.DateTime), Convert.ToDateTime(dtFromDate.DateTime)) >= 0)
                {
                    if (!CachedDataManager.GetInstance.ValidateNAVLockDate(dtFromDate.DateTime))
                    {
                        MessageBox.Show("The start date you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                            + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    string accountIds = ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',');
                    if (!CashDataManager.GetInstance().GetIsRevaluationInProgress(accountIds))
                    {
                        _journalSource = "Journals generated using 'Get Overriding Data' button";
                        ClearData();
                        BackgroundWorker bgWorkerGetOverridingDataAsyn = new BackgroundWorker();
                        bgWorkerGetOverridingDataAsyn.DoWork += new DoWorkEventHandler(bgWorkerGetOverridingDataAsyn_DoWork);
                        bgWorkerGetOverridingDataAsyn.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwrkrGetData_RunWorkerCompleted);
                        ChangeStatus(false, false, true);
                        bgWorkerGetOverridingDataAsyn.RunWorkerAsync();
                    }
                    else
                        MessageBox.Show("Revaluation is in progress for selected fund(s). Proceed after revaluation is done.", "Revaluation", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                    MessageBox.Show("To Date is before From Date", "Overriding journal", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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

        private async void btnRunRevaluation_Click(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Compare(Convert.ToDateTime(dtToDate.DateTime), Convert.ToDateTime(dtFromDate.DateTime)) >= 0)
                {
                    if (!CachedDataManager.GetInstance.ValidateNAVLockDate(dtFromDate.DateTime))
                    {
                        MessageBox.Show("The start date you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                            + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    toolStripStatusLabel1.Text = "";
                    string accountIds = ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',');
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
                        dlgResult = ConfirmationMessageBox.Display("Revaluation process is a time and resource consuming process, do you want to continue?", "Revaluation");
                        if (dlgResult == DialogResult.Yes)
                        {
                            string invalidFundNames = CashDataManager.GetInstance().GetInvalidFundsForSymbolLevel(accountIds, dtFromDate.DateTime, dtToDate.DateTime);
                            if (!string.IsNullOrEmpty(invalidFundNames))
                            {
                                DialogResult result1 = MessageBox.Show("Changes have been made prior to the accrual balance start date for " + invalidFundNames + " . Running revaluation may cause mismatches in the accrual balance. Please confirm you want to continue. If you are unsure of these changes please select 'No' and reach out to your Nirvana support representative", "Incorrect Revaluation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (result1 == DialogResult.Yes)
                                {
                                    Logger.LoggerWrite("Revaluation run prior to Symbol wise accrual date for funds:" + invalidFundNames, "Revaluation Run");
                                    if (!CashDataManager.GetInstance().GetIsRevaluationInProgress(accountIds))
                                    {
                                        toolStripStatusLabel1.Text = "Running Manual Revaluation Process...";
                                        if (!CustomThemeHelper.ApplyTheme)
                                        {
                                            toolStripStatusLabel1.ForeColor = System.Drawing.Color.DarkGreen;
                                        }
                                        ugbxJExcepParams.Enabled = false;
                                        string userName = CachedDataManager.GetInstance.LoggedInUser.LoginID;
                                        Logger.LoggerWrite("Revaluation Process Starts... Time: " + DateTime.UtcNow + "   Account Id: " + accountIds + "   User Name: " + userName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                                        DateTime revaluationStartTime = DateTime.Now;
                                        toolStripProgressBar.Visible = true;
                                        toolStripProgressBar.Value = 0;
                                        //Revaluation Method Call
                                        bool result = await CashDataManager.GetInstance().RunRevaluationProcess(dtFromDate.DateTime, dtToDate.DateTime, ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(','), true, false, LoggingConstants.RUN_MANUAL_REVAL);
                                        if (result)
                                        {
                                            if (!this.IsDisposed)
                                            {
                                                toolStripProgressBar.Value = 100;  // this is used as progress takes integer value of progress.so it shows approximate but not accruate Progress. So to adjust to 100 at the end.
                                                toolStripStatusLabel1.Text = "Revaluation process completed";
                                                if (!CustomThemeHelper.ApplyTheme)
                                                    toolStripStatusLabel1.ForeColor = System.Drawing.Color.Green;
                                            }
                                            Logger.LoggerWrite("Revaluation Process Completed... Time: " + DateTime.UtcNow + "   Fund Id: " + accountIds + "   User Name: " + userName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                                            Logger.LoggerWrite("Total time taken by revaluation process:" + Convert.ToString(DateTime.Now - revaluationStartTime), LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                                        }
                                        else
                                        {
                                            if (!this.IsDisposed)
                                            {
                                                toolStripProgressBar.Visible = false;
                                                toolStripStatusLabel1.Text = "Revaluation Process Failed";
                                                if (!CustomThemeHelper.ApplyTheme)
                                                    toolStripStatusLabel1.ForeColor = System.Drawing.Color.Red;
                                                Logger.LoggerWrite("Revaluation process failed... Time: " + DateTime.UtcNow + "   Account Id: " + accountIds + "   User Name: " + userName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                                                Logger.LoggerWrite("Total time taken by revaluation process(failed):" + Convert.ToString(DateTime.Now - revaluationStartTime), LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                                            }
                                        }
                                        ugbxJExcepParams.Enabled = true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Revaluation is in progress for selected fund(s). Proceed after revaluation is done.", "Revaluation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }
                            else
                            {
                                if (!CashDataManager.GetInstance().GetIsRevaluationInProgress(accountIds))
                                {
                                    toolStripStatusLabel1.Text = "Running Manual Revaluation Process...";
                                    if (!CustomThemeHelper.ApplyTheme)
                                    {
                                        toolStripStatusLabel1.ForeColor = System.Drawing.Color.DarkGreen;
                                    }
                                    ugbxJExcepParams.Enabled = false;
                                    string userName = CachedDataManager.GetInstance.LoggedInUser.LoginID;
                                    Logger.LoggerWrite("Revaluation Process Starts... Time: " + DateTime.UtcNow + "   Account Id: " + accountIds + "   User Name: " + userName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                                    DateTime revaluationStartTime = DateTime.Now;
                                    toolStripProgressBar.Visible = true;
                                    toolStripProgressBar.Value = 0;
                                    //Revaluation Method Call
                                    bool result = await CashDataManager.GetInstance().RunRevaluationProcess(dtFromDate.DateTime, dtToDate.DateTime, ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(','), true, false, LoggingConstants.RUN_MANUAL_REVAL);
                                    if (result)
                                    {
                                        if (!this.IsDisposed)
                                        {
                                            toolStripProgressBar.Value = 100;  // this is used as progress takes integer value of progress.so it shows approximate but not accruate Progress. So to adjust to 100 at the end.
                                            toolStripStatusLabel1.Text = "Revaluation process completed";
                                            if (!CustomThemeHelper.ApplyTheme)
                                                toolStripStatusLabel1.ForeColor = System.Drawing.Color.Green;
                                        }
                                        Logger.LoggerWrite("Revaluation Process Completed... Time: " + DateTime.UtcNow + "   Fund Id: " + accountIds + "   User Name: " + userName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                                        Logger.LoggerWrite("Total time taken by revaluation process:" + Convert.ToString(DateTime.Now - revaluationStartTime), LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                                    }
                                    else
                                    {
                                        if (!this.IsDisposed)
                                        {
                                            toolStripProgressBar.Visible = false;
                                            toolStripStatusLabel1.Text = "Revaluation Process Failed";
                                            if (!CustomThemeHelper.ApplyTheme)
                                                toolStripStatusLabel1.ForeColor = System.Drawing.Color.Red;
                                            Logger.LoggerWrite("Revaluation process failed... Time: " + DateTime.UtcNow + "   Account Id: " + accountIds + "   User Name: " + userName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                                            Logger.LoggerWrite("Total time taken by revaluation process(failed):" + Convert.ToString(DateTime.Now - revaluationStartTime), LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                                        }
                                    }
                                    ugbxJExcepParams.Enabled = true;
                                }
                                else
                                {
                                    MessageBox.Show("Revaluation is in progress for selected fund(s). Proceed after revaluation is done.", "Revaluation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }

                        }
                        else
                        {
                            toolStripProgressBar.Visible = false;
                            toolStripStatusLabel1.Text = "Revaluation Process canceled";
                            Logger.LoggerWrite("Revaluation Process Canceled... Time: " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                            if (!CustomThemeHelper.ApplyTheme)
                            {
                                toolStripStatusLabel1.ForeColor = System.Drawing.Color.Blue;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("To Date is before From Date.", "Journal Exceptions", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        ///  Method to check if the Date choosen on UI is greater than Cash Management Start Date for which Revaluation needs to done.
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
                        if (accountCashManagementStartDate > dtFromDate.DateTime)
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

        void bgWorkerGetOverridingDataAsyn_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //DateTime CashmgmtStartDate = CashDataManager.GetInstance().GetCashPreferences().CashMgmtStartDate;

                //if (CashmgmtStartDate <= dtFromDate.DateTime)
                //{


                #region Trading Activity Overriding Section

                //_lsExceptionalTransactions = CashDataManager.GetInstance().GetTransactionsForOverriding(dtFromDate.DateTime, dtToDate.DateTime);
                _lsExceptionalTransactions = CashDataManager.GetInstance().GetTransactionsForOverriding(dtFromDate.DateTime, dtToDate.DateTime, GetCommaSeparatedAccountIDs().TrimEnd(','));
                if (_lsExceptionalTransactions != null && _lsExceptionalTransactions.Count > 0)
                    _transactionsFromDB.AddRange(_lsExceptionalTransactions);

                #endregion

                #region Dividend Overriding Section

                //_dtDiv = CashDataManager.GetInstance().GetCashDividendsForGivenDates(string.Empty, dtFromDate.DateTime, dtToDate.DateTime).Tables[0];
                //if (_dtDiv != null && _dtDiv.Rows.Count > 0)
                //{
                //    TaxlotBaseCollection modifiedTaxlots = CashDataManager.GetInstance().GetTaxlotBaseCollection(_dtDiv);
                //    List<Transaction> lsDividendTransactions = CashDataManager.GetInstance().GetDividendEntriesFromTaxlotBase(modifiedTaxlots);

                //    _transactionsFromDB.AddRange(lsDividendTransactions);

                //}
                #endregion
                //}
                //else
                //    MessageBox.Show("Please Select Date Greater then Cash Management Start Date " + CashmgmtStartDate);

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
        /// Added by: Bharat raturi, 15 jul 2014
        /// Get the Comma separated accountIDs in the form of string
        /// </summary>
        /// <returns>String holding the comma separated AccountIDs</returns>
        private String GetCommaSeparatedAccountIDs()
        {
            String accountIDs = string.Empty;
            foreach (int accountID in ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetSelectedItemsInDictionary().Keys)
            {
                accountIDs += accountID.ToString() + ",";
            }
            return accountIDs.Trim(',');
        }

        #endregion

        #region Persistance Section

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_transactionsFromDB != null && _transactionsFromDB.Count > 0)
                {
                    string accountIds = ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',');
                    if (!CashDataManager.GetInstance().GetIsRevaluationInProgress(accountIds))
                    {
                        DialogResult dlgResult = new DialogResult();
                        if (_lsExceptionalTransactions != null && _lsExceptionalTransactions.Count > 0)
                        {
                            dlgResult = ConfirmationMessageBox.DisplayYesNo("Revaluation date may be set to back date. Do you want to continue?", "Journal Exception");
                        }

                        if (dlgResult == DialogResult.Yes)
                        {
                            string userName = CachedDataManager.GetInstance.LoggedInUser.LoginID;
                            if (IsGetException)
                                Logger.LoggerWrite("Get Exception(Journal Exception)... FormDate: " + Convert.ToString(dtFromDate.DateTime) + " Todate: " + Convert.ToString(dtToDate.DateTime) + " Account Id: " + accountIds + " User Name: " + userName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                            if (IsOverride)
                                Logger.LoggerWrite("Get Override(Journal Exception)... FormDate: " + Convert.ToString(dtFromDate.DateTime) + " Todate: " + Convert.ToString(dtToDate.DateTime) + " Account Id: " + accountIds + " User Name: " + userName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

                            SaveDataAsync();
                        }
                        else
                        {
                            ClearData();
                            toolStripProgressBar.Visible = false;
                            toolStripStatusLabel1.Text = "Save operation cancelled.";
                        }
                    }
                    else
                        MessageBox.Show("Revaluation is in progress for selected fund(s). Proceed after revaluation is done.", "Revaluation", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


        private void SaveDataAsync()
        {
            try
            {
                BackgroundWorker bgwrkrSaveData = new BackgroundWorker();
                bgwrkrSaveData.DoWork += new DoWorkEventHandler(bgwrkrSaveData_DoWork);
                bgwrkrSaveData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwrkrSaveData_RunWorkerCompleted);
                ChangeStatus(false, false, false);
                bgwrkrSaveData.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        void bgwrkrSaveData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                #region Trading Activity Section

                if (_lsExceptionalTransactions != null && _lsExceptionalTransactions.Count > 0)
                    CashDataManager.GetInstance().Save(_lsExceptionalTransactions, null, _journalSource, "Journal Exceptions");


                #endregion

                #region Dividend Section

                //if (_dtDiv != null && _dtDiv.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in _dtDiv.Rows)
                //        dr.SetModified();
                //    CashDataManager.GetInstance().CreateAndSaveJournalEntriesForCashDividend(_dtDiv);
                //}

                #endregion


            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        void bgwrkrSaveData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                ClearData();
                ChangeStatus(true, false, false);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion

        private void grdCashExceptions_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {
                if (e == null || e.Row == null)
                {
                    return;
                }
                else
                {
                    #region Hide local currency cash journal transaction entries
                    //CHMW-3114 [Foreign Positions Settling in Base Currency] [Implementation] [Cash Management] Hide local currency cash journal transaction entries
                    TransactionEntry trEntry = (TransactionEntry)e.Row.ListObject;
                    if (trEntry != null
                        && trEntry.TransactionSource == CashTransactionType.SettlementTransaction
                        && CashDataManager.GetInstance().GetCashPreferences(trEntry.AccountID) != null
                        && !CashDataManager.GetInstance().GetCashPreferences(trEntry.AccountID).IsCashSettlementEntriesVisible)
                    {
                        e.Row.Hidden = true;
                    }
                    else
                    {
                        e.Row.Hidden = false;
                    }
                    #endregion
                }
                //double Amount = Convert.ToDouble(e.Row.Cells["Amount"].Value);

                //if (Amount >= 0)
                //{
                //    //e.Row.Appearance.ForeColor = Color.PaleGreen;
                //    e.Row.Appearance.ForeColor = Color.FromArgb(177, 216, 64);
                //}
                //else
                //{

                //    //e.Row.Appearance.ForeColor = Color.IndianRed;
                //    e.Row.Appearance.ForeColor = Color.FromArgb(255, 91, 71);
                //}
                //e.Row.Cells["Amount"].SetValue(Amount.ToString("#0.00"), true);

                if (e.Row.ListObject is TransactionEntry)
                {
                    AccountSideColumnSetting(e.Row);
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

        void grdCashExceptions_BeforeRowFilterDropDown(object sender, Infragistics.Win.UltraWinGrid.BeforeRowFilterDropDownEventArgs e)
        {
            try
            {
                if (e.Column.Key.Equals(CashManagementConstants.COLUMN_TRANSACTIONDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_MODIFYDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_ENTRYDATE))
                {
                    e.ValueList.ValueListItems.Insert(4, "(Today)", "(Today)");
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

        void grdCashExceptions_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(CashManagementConstants.COLUMN_TRANSACTIONDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_MODIFYDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_ENTRYDATE)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    grdCashExceptions.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    grdCashExceptions.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
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

        private void AccountSideColumnSetting(UltraGridRow ultraGridRow)
        {
            try
            {
                AccountSide entryAcSide = (AccountSide)Enum.Parse(typeof(AccountSide), ultraGridRow.Cells["EntryAccountSide"].Text.ToString());
                if (!CustomThemeHelper.ApplyTheme)
                {
                    if (entryAcSide == AccountSide.DR)
                    {
                        ultraGridRow.Appearance.BackColor = Color.Black;
                        ultraGridRow.Cells["CR"].Appearance.ForeColor = ultraGridRow.Appearance.BackColor;
                        ultraGridRow.Cells["DR"].Appearance.ForeColor = Color.White;
                    }
                    else if (entryAcSide == AccountSide.CR)
                    {

                        ultraGridRow.Appearance.BackColor = Color.Gray;
                        ultraGridRow.Cells["DR"].Appearance.ForeColor = ultraGridRow.Appearance.BackColor;
                        ultraGridRow.Cells["CR"].Appearance.ForeColor = Color.White;
                        //if (!isCreditRowSpacingDone)
                        //{

                        //    e.Row.RowSpacingBefore = 10;
                        //    isCreditRowSpacingDone = true;
                        //    //UIElement ele = e.Row.Cells["CR"].Column.Layout.UIElement;

                        //    //Rectangle rec =  e.Row.Cells["CR"].GetUIElement().Rect;
                        //}

                        //e.Row.Appearance.TextHAlign = HAlign.Right;

                    }
                }
                else
                {
                    if (entryAcSide == AccountSide.DR)
                    {
                        foreach (UltraGridCell cell in ultraGridRow.Cells)
                        {
                            if (!(cell.Column.Key.Equals(CashManagementConstants.COLUMN_SYMBOL) ||
                            cell.Column.Key.Equals(CashManagementConstants.COLUMN_ACCOUNT) ||
                                cell.Column.Key.Equals(CashManagementConstants.COLUMN_CURRENCYNAME) ||
                                cell.Column.Key.Equals("TransactionDate") ||
                                cell.Column.Key.Equals("TransactionID") ||
                                cell.Column.Key.Equals(CashManagementConstants.COLUMN_DESCRIPTION)))
                            {
                                cell.Appearance.BackColor = Color.FromArgb(231, 232, 233);
                            }
                        }
                        ultraGridRow.Cells["CR"].Appearance.ForeColor = Color.FromArgb(231, 232, 233);
                        ultraGridRow.Cells["DR"].Appearance.ForeColor = Color.Black;
                    }
                    else if (entryAcSide == AccountSide.CR)
                    {
                        foreach (UltraGridCell cell in ultraGridRow.Cells)
                        {
                            if (!(cell.Column.Key.Equals(CashManagementConstants.COLUMN_SYMBOL) ||
                            cell.Column.Key.Equals(CashManagementConstants.COLUMN_ACCOUNT) ||
                                cell.Column.Key.Equals(CashManagementConstants.COLUMN_CURRENCYNAME) ||
                                cell.Column.Key.Equals("TransactionDate") ||
                                cell.Column.Key.Equals("TransactionID") ||
                                cell.Column.Key.Equals(CashManagementConstants.COLUMN_DESCRIPTION)))
                            {
                                cell.Appearance.BackColor = Color.FromArgb(134, 134, 134);
                            }
                        }
                        ultraGridRow.Cells["DR"].Appearance.ForeColor = Color.FromArgb(134, 134, 134);
                        ultraGridRow.Cells["CR"].Appearance.ForeColor = Color.Black;
                        //if (!isCreditRowSpacingDone)
                        //{

                        //    e.Row.RowSpacingBefore = 10;
                        //    isCreditRowSpacingDone = true;
                        //    //UIElement ele = e.Row.Cells["CR"].Column.Layout.UIElement;

                        //    //Rectangle rec =  e.Row.Cells["CR"].GetUIElement().Rect;
                        //}

                        //e.Row.Appearance.TextHAlign = HAlign.Right;

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

        private void grdCashExceptions_AfterSortChange(object sender, Infragistics.Win.UltraWinGrid.BandEventArgs e)
        {
            if (grdCashExceptions.Rows.IsGroupByRows)
            {
                grdCashExceptions.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.InGroupByRows;
            }
            else
            {
                grdCashExceptions.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
            }
            if (_columnSorted != null)
            {
                string[] lsColumnToSort = new string[] { "TransactionDate" };
                var lsColumnSortQuery = from column in lsColumnToSort where column == _columnSorted.Key select column;
                if (lsColumnSortQuery.Count() > 0)
                    SortGridByColumnName(grdCashExceptions);
            }
        }

        private void grdCashExceptions_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            HelperClass.SummarySettings(e);
        }

        private void grdCashExceptions_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            HelperClass.GroupByRowSetting(e);
        }

        private void ctrlJournalExceptions_Load(object sender, EventArgs e)
        {
            try
            {
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    this.statusStrip1.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripStatusLabel1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.WhiteSmoke;
                }
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
                ctrlMasterFundAndAccountsDropdown1.Setup();
                ugbxJExcepParams.Appearance.BackColor = System.Drawing.Color.LightGray;
                InitializeGridLayout();
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
                btnOverriding.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnOverriding.ForeColor = System.Drawing.Color.White;
                btnOverriding.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnOverriding.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnOverriding.UseAppStyling = false;
                btnOverriding.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGetEx.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetEx.ForeColor = System.Drawing.Color.White;
                btnGetEx.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetEx.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetEx.UseAppStyling = false;
                btnGetEx.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnRunRevaluation.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRunRevaluation.ForeColor = System.Drawing.Color.White;
                btnRunRevaluation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRunRevaluation.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRunRevaluation.UseAppStyling = false;
                btnRunRevaluation.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void grdCashExceptions_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {

                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdCashExceptions);
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

        private void grdCashExceptions_MouseClick(object sender, MouseEventArgs e)
        {
            MouseClickCommanForSort(sender, e);
        }

        private void grdCashExceptions_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownCommanForSort(e, grdCashExceptions);
        }

        private void MouseClickCommanForSort(object sender, MouseEventArgs e)
        {
            try
            {
                UltraGrid grid = sender as UltraGrid;
                UIElement controlElement = grid.DisplayLayout.UIElement;
                UIElement elementAtPoint = controlElement != null ? controlElement.ElementFromPoint(e.Location) : null;
                while (elementAtPoint != null)
                {
                    Infragistics.Win.UltraWinGrid.UltraGridUIElement uiElement = elementAtPoint.ControlElement as Infragistics.Win.UltraWinGrid.UltraGridUIElement;
                    HeaderUIElement headerElement = uiElement.ElementWithMouseCapture as HeaderUIElement;
                    if (headerElement != null &&
                         headerElement.Header is Infragistics.Win.UltraWinGrid.ColumnHeader)
                    {
                        _columnSorted = headerElement.GetContext(typeof(UltraGridColumn)) as UltraGridColumn;
                        break;
                    }

                    elementAtPoint = elementAtPoint.Parent;
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

        private void MouseDownCommanForSort(MouseEventArgs e, UltraGrid grd)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    // Get a reference to the UIElement at the current mouse position
                    UIElement thisElem = grd.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));

                    // Exit the event handler if no UIElement is found
                    if (thisElem == null)
                        return;

                    //See if the UIElement at the current mouse position is a GroupByBoxUIElement,
                    // or if it is contained as a child of a GroupByBoxUIElement
                    if (thisElem is GroupByBoxUIElement ||
                        thisElem.GetAncestor(typeof(GroupByBoxUIElement)) is GroupByBoxUIElement)
                    {
                        string columnSortedName = string.Empty;
                        if (thisElem is Infragistics.Win.TextUIElement)
                        {
                            columnSortedName = ((Infragistics.Win.TextUIElement)(thisElem)).Text;
                        }
                        else if (thisElem is Infragistics.Win.UltraWinGrid.SortIndicatorUIElement)
                        {
                            columnSortedName = ((Infragistics.Win.UltraWinGrid.SortIndicatorUIElement)(thisElem)).ToString();
                        }
                        else if (thisElem is GroupByButtonUIElement)
                        {
                            foreach (object thisElemChild in thisElem.ChildElements)
                            {
                                if (thisElemChild is Infragistics.Win.TextUIElement)
                                {
                                    columnSortedName = ((Infragistics.Win.TextUIElement)(thisElemChild)).Text;
                                    break;
                                }
                            }
                        }

                        for (int counter = 0; counter < grd.DisplayLayout.Bands[0].SortedColumns.Count; counter++)
                        {
                            if (columnSortedName.Equals(grd.DisplayLayout.Bands[0].SortedColumns[counter].Header.Caption))
                            {
                                _columnSorted = grd.DisplayLayout.Bands[0].SortedColumns[grd.DisplayLayout.Bands[0].SortedColumns[counter].Key];
                                break;
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

        public class GroupSortComparer : IComparer
        {
            private int _multiplier = -1;
            private string _columnName;
            public string Column
            {
                get { return _columnName; }
                set { _columnName = value; }
            }

            private SortIndicator _sortIndicator = SortIndicator.Descending;
            public SortIndicator SortIndicator
            {
                get { return _sortIndicator; }
                set
                {
                    _sortIndicator = value;
                    switch (_sortIndicator)
                    {
                        case SortIndicator.Ascending:
                            _multiplier = 1;
                            break;

                        case SortIndicator.Descending:
                        case SortIndicator.Disabled:
                        case SortIndicator.None:
                            _multiplier = -1;
                            break;

                        default:
                            break;
                    }
                }
            }

            public int Compare(object xObj, object yObj)
            {
                try
                {
                    UltraGridGroupByRow x = (UltraGridGroupByRow)xObj;
                    UltraGridGroupByRow y = (UltraGridGroupByRow)yObj;
                    IComparable xValue;
                    IComparable yValue;

                    if (Equals(xObj, yObj))
                    {
                        return 0;
                    }
                    if (!(string.IsNullOrEmpty(_columnName)))
                    {
                        if (x.Rows.SummaryValues[_columnName].Value == null)
                        {
                            return _multiplier;
                        }
                        if (y.Rows.SummaryValues[_columnName].Value == null)
                        {
                            return (-(_multiplier));
                        }
                        if (!x.Rows.SummaryValues[_columnName].Value.GetType().Equals(y.Rows.SummaryValues[_columnName].Value.GetType()))
                        {
                            if (x.Rows.SummaryValues[_columnName].Value is IComparable && y.Rows.SummaryValues[_columnName].Value is IComparable)
                            {
                                xValue = (IComparable)x.Rows.SummaryValues[_columnName].Value;
                                yValue = (IComparable)y.Rows.SummaryValues[_columnName].Value;
                                return xValue.ToString().CompareTo(yValue.ToString()) * _multiplier;
                            }
                            if (x.Rows.SummaryValues[_columnName].Value is IComparable)
                            {
                                return _multiplier;
                            }
                            if (y.Rows.SummaryValues[_columnName].Value is IComparable)
                            {
                                return (-(_multiplier));
                            }
                        }
                        else
                        {
                            xValue = (IComparable)x.Rows.SummaryValues[_columnName].Value;
                            yValue = (IComparable)y.Rows.SummaryValues[_columnName].Value;
                            return xValue.CompareTo(yValue) * _multiplier;
                        }
                    }
                    else
                    {
                        return 0;
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
                return 0;
            }
        }

        private void SortGridByColumnName(UltraGrid grd)
        {
            try
            {
                int sortCount = grd.DisplayLayout.Bands[0].SortedColumns.Count;
                if (sortCount > 0)
                {
                    //Correction made as it was not returning the column that has been sorted.
                    //Now the sorted column will be detected by mouse click event on column header.
                    UltraGridColumn sortColumn;
                    if (grd.DisplayLayout.Bands[0].SortedColumns.Contains(_columnSorted))
                    {
                        sortColumn = grd.DisplayLayout.Bands[0].SortedColumns[_columnSorted.Key];
                    }
                    else
                    {
                        foreach (UltraGridColumn var in grd.DisplayLayout.Bands[0].SortedColumns)
                        {
                            var.GroupByComparer = null;
                        }
                        //Set the Group by row summary display settings 
                        RowSummarySettings(grd);
                        return;
                    }
                    if (sortColumn.Formula != null && !(sortColumn.DataType.Equals(typeof(System.Double))))
                    {
                        //Set the Group by row summary display settings 
                        RowSummarySettings(grd);
                        return;
                    }

                    if (!sortColumn.IsGroupByColumn && !GroupByColumnsCollection.Contains(sortColumn.Key))
                    {
                        _groupSortComparer.Column = sortColumn.Key;
                        _groupSortComparer.SortIndicator = sortColumn.SortIndicator;
                        foreach (UltraGridColumn var in grd.DisplayLayout.Bands[0].SortedColumns)
                        {
                            var.GroupByComparer = null;
                            if (var.IsGroupByColumn)
                            {
                                if (var.SortIndicator == SortIndicator.Descending)
                                {
                                    if (_groupSortComparer.SortIndicator == SortIndicator.Ascending)
                                    {
                                        _groupSortComparer.SortIndicator = SortIndicator.Descending;
                                    }
                                    else if (_groupSortComparer.SortIndicator == SortIndicator.Descending)
                                    {
                                        _groupSortComparer.SortIndicator = SortIndicator.Ascending;
                                    }
                                }

                                var.GroupByComparer = _groupSortComparer;
                            }
                        }
                        sortColumn.GroupByComparer = _groupSortComparer;
                    }
                    else if (sortColumn.IsGroupByColumn && GroupByColumnsCollection.Contains(sortColumn.Key))
                    {
                        _groupSortComparer.Column = sortColumn.Key;
                        _groupSortComparer.SortIndicator = sortColumn.SortIndicator;
                        foreach (UltraGridColumn var in grd.DisplayLayout.Bands[0].SortedColumns)
                        {
                            if (var.IsGroupByColumn)
                            {
                                if (var.SortIndicator == SortIndicator.Descending)
                                {
                                    if (_groupSortComparer.SortIndicator == SortIndicator.Ascending)
                                    {
                                        _groupSortComparer.SortIndicator = SortIndicator.Descending;
                                    }
                                    else if (_groupSortComparer.SortIndicator == SortIndicator.Descending)
                                    {
                                        _groupSortComparer.SortIndicator = SortIndicator.Ascending;
                                    }
                                }
                                var.GroupByComparer = _groupSortComparer;
                            }
                        }
                    }
                    else
                    {
                        foreach (UltraGridColumn var in grd.DisplayLayout.Bands[0].SortedColumns)
                        {
                            var.GroupByComparer = null;
                        }
                    }
                    grd.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                }
                RowSummarySettings(grd);
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

        private void RowSummarySettings(UltraGrid grd)
        {
            try
            {
                bool groupedBySomeColumn = false;
                foreach (UltraGridColumn col in grd.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (col.IsGroupByColumn)
                    {
                        groupedBySomeColumn = true;
                        break;
                    }
                }
                if (!groupedBySomeColumn)
                {
                    grd.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
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
                CashManagementLayout cashManagementLayout = saveGridLayout.GetLayout(grdCashExceptions, "JournalExceptions");
                CashPreferenceManager.GetInstance().SetCashGridLayout(cashManagementLayout, "JournalExceptions");
                JournalExceptionsLayout = cashManagementLayout;
                toolStripStatusLabel1.Text = "Layout Saved Successfully.";
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

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ExportToExcelHelper.ExportToExcel(grdCashExceptions))
                {
                    toolStripStatusLabel1.Text = "Report Successfully Saved.";
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

        #region Initialize Grid with already saved layout from XML

        private CashManagementLayout _journalExceptionsLayout = null;
        public CashManagementLayout JournalExceptionsLayout
        {
            get { return _journalExceptionsLayout; }
            set { _journalExceptionsLayout = value; }
        }

        public void InitializeGridLayout()
        {
            try
            {
                if (JournalExceptionsLayout != null)
                {
                    if (JournalExceptionsLayout.GroupByColumnsCollection.Count > 0)
                    {
                        //Set GroupBy Columns
                        bool flag = true;
                        int GroupedColumns = grdCashExceptions.DisplayLayout.Bands[0].SortedColumns.Count;
                        for (int i = 0; i < GroupedColumns - 1; i++)
                        {
                            grdCashExceptions.DisplayLayout.Bands[0].SortedColumns.Remove(0);
                        }

                        foreach (string item in JournalExceptionsLayout.GroupByColumnsCollection)
                        {
                            if (grdCashExceptions.DisplayLayout.Bands[0].Columns.Exists(item) && !grdCashExceptions.DisplayLayout.Bands[0].SortedColumns.Contains(item))
                            {
                                grdCashExceptions.DisplayLayout.Bands[0].SortedColumns.Add(item, true, true);
                                if (grdCashExceptions.DisplayLayout.Bands[0].SortedColumns.Count == 1)
                                {
                                    flag = false;
                                }
                                if (flag)
                                {
                                    grdCashExceptions.DisplayLayout.Bands[0].SortedColumns.Remove(0);
                                    flag = false;
                                }
                            }
                        }

                        grdCashExceptions.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                        grdCashExceptions.DisplayLayout.RefreshSummaries();
                    }

                    ColumnFiltersCollection columnFilters = grdCashExceptions.DisplayLayout.Bands[0].ColumnFilters;
                    ///TODO : When we apply the custom filters we need to change the code so the filters won't be on a common field
                    columnFilters.ClearAllFilters();

                    foreach (UltraGridColumn col in grdCashExceptions.DisplayLayout.Bands[0].Columns)
                    {
                        col.Hidden = true;
                    }

                    foreach (CashGridColumn selectedCols in JournalExceptionsLayout.SelectedColumnsCollection)
                    {
                        UltraGridColumn col = null;
                        if (grdCashExceptions.DisplayLayout.Bands[0].Columns.Exists(selectedCols.Name))
                        {
                            col = grdCashExceptions.DisplayLayout.Bands[0].Columns[selectedCols.Name];
                            if (col != null)
                            {
                                col.HiddenWhenGroupBy = DefaultableBoolean.False;
                                col.Hidden = selectedCols.Hidden;
                                col.Header.Fixed = selectedCols.IsHeaderFixed;


                                if (!grdCashExceptions.DisplayLayout.Bands[0].SortedColumns.Contains(col))
                                {
                                    col.SortIndicator = selectedCols.SortIndicator;
                                    if (col.SortIndicator.Equals(SortIndicator.Ascending))

                                        grdCashExceptions.DisplayLayout.Bands[0].SortedColumns.Add(col, false);
                                    if (col.SortIndicator.Equals(SortIndicator.Descending))
                                        grdCashExceptions.DisplayLayout.Bands[0].SortedColumns.Add(col, true);

                                }

                                grdCashExceptions.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
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
                                        if ((selectedCols.Name.Equals(CashManagementConstants.COLUMN_TRANSACTIONDATE) || selectedCols.Name.Equals(CashManagementConstants.COLUMN_ENTRYDATE) || selectedCols.Name.Equals(CashManagementConstants.COLUMN_MODIFYDATE)) && selectedCols.FilterConditionList.Count == 1 && selectedCols.FilterConditionList[0].ComparisionOperator == FilterComparisionOperator.StartsWith && selectedCols.FilterConditionList[0].CompareValue.Equals("(Today)"))
                                        {
                                            grdCashExceptions.DisplayLayout.Bands[0].ColumnFilters[col].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.ToString(DateTimeConstants.DateformatForClosing));
                                        }
                                        else
                                        {
                                            grdCashExceptions.DisplayLayout.Bands[0].ColumnFilters[col].FilterConditions.Add(filCond);
                                        }
                                    }
                                    grdCashExceptions.DisplayLayout.Bands[0].ColumnFilters[col].LogicalOperator = selectedCols.FilterLogicalOperator;
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

        #endregion

        private void grdCashExceptions_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }
    }
}
