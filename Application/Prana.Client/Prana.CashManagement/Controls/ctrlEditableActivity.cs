using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.CashManagement.Classes;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace Prana.CashManagement.Controls
{
    public partial class ctrlEditableActivity : UserControl, IPublishing
    {
        private static object _lockerCashActivity = new object();
        GenericBindingList<CashActivity> _laCashActivityToBind = new GenericBindingList<CashActivity>();
        public GenericBindingList<CashActivity> lsCashActivityToBind
        {
            get { lock (_lockerCashActivity) { return _laCashActivityToBind; } }
            set { lock (_lockerCashActivity) { _laCashActivityToBind = value; } }
        }

        private List<CashActivity> _lsModifiedActivity = new List<CashActivity>();
        public List<CashActivity> lsModifiedActivity
        {
            get { return _lsModifiedActivity; }
            set { _lsModifiedActivity = value; }
        }

        ValueList _vlTransactionSource;
        ValueList _vlCurrencies;
        public ctrlEditableActivity()
        {
            InitializeComponent();

            InitializeGrids();
            InitializeValueList();
            CreateSubscriptionServicesProxy();
        }

        private void InitializeGrids()
        {
            try
            {
                CashActivity entryToInitializeGrid = new CashActivity();
                lsCashActivityToBind.Add(entryToInitializeGrid);
                grdActivity.DataSource = lsCashActivityToBind;
                lsCashActivityToBind.Clear();
                List<string> lsColumnsToDisplay = new List<string>(new string[] { "Date", "ActivityType", "AccountName", "Symbol", "CurrencyName", "Amount", "BalanceType", "TransactionSource" });
                HelperClass.SetColumnDisplayNames(grdActivity, lsColumnsToDisplay);
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

        private CashManagementLayout _activityLayout = null;
        public CashManagementLayout ActivityLayout
        {
            get { return _activityLayout; }
            set { _activityLayout = value; }
        }

        public void InitializeGridLayout()
        {
            try
            {
                if (ActivityLayout != null)
                {
                    if (ActivityLayout.GroupByColumnsCollection.Count > 0)
                    {
                        //Set GroupBy Columns
                        bool flag = true;
                        int GroupedColumns = grdActivity.DisplayLayout.Bands[0].SortedColumns.Count;
                        for (int i = 0; i < GroupedColumns - 1; i++)
                        {
                            grdActivity.DisplayLayout.Bands[0].SortedColumns.Remove(0);
                        }

                        foreach (string item in ActivityLayout.GroupByColumnsCollection)
                        {
                            if (grdActivity.DisplayLayout.Bands[0].Columns.Exists(item) && !grdActivity.DisplayLayout.Bands[0].SortedColumns.Contains(item))
                            {
                                grdActivity.DisplayLayout.Bands[0].SortedColumns.Add(item, true, true);
                                if (grdActivity.DisplayLayout.Bands[0].SortedColumns.Count == 1)
                                {
                                    flag = false;
                                }
                                if (flag)
                                {
                                    grdActivity.DisplayLayout.Bands[0].SortedColumns.Remove(0);
                                    flag = false;
                                }
                            }
                        }

                        grdActivity.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                        grdActivity.DisplayLayout.RefreshSummaries();
                    }

                    ColumnFiltersCollection columnFilters = grdActivity.DisplayLayout.Bands[0].ColumnFilters;
                    ///TODO : When we apply the custom filters we need to change the code so the filters won't be on a common field
                    columnFilters.ClearAllFilters();

                    foreach (UltraGridColumn col in grdActivity.DisplayLayout.Bands[0].Columns)
                    {
                        col.Hidden = true;
                    }

                    foreach (CashGridColumn selectedCols in ActivityLayout.SelectedColumnsCollection)
                    {
                        UltraGridColumn col = null;
                        if (grdActivity.DisplayLayout.Bands[0].Columns.Exists(selectedCols.Name))
                        {
                            col = grdActivity.DisplayLayout.Bands[0].Columns[selectedCols.Name];
                            if (col != null)
                            {
                                col.HiddenWhenGroupBy = DefaultableBoolean.False;
                                col.Hidden = selectedCols.Hidden;
                                col.Header.Fixed = selectedCols.IsHeaderFixed;


                                if (!grdActivity.DisplayLayout.Bands[0].SortedColumns.Contains(col))
                                {
                                    col.SortIndicator = selectedCols.SortIndicator;
                                    if (col.SortIndicator.Equals(SortIndicator.Ascending))

                                        grdActivity.DisplayLayout.Bands[0].SortedColumns.Add(col, false);
                                    if (col.SortIndicator.Equals(SortIndicator.Descending))
                                        grdActivity.DisplayLayout.Bands[0].SortedColumns.Add(col, true);

                                }

                                grdActivity.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
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
                                        if ((selectedCols.Name.Equals(CashManagementConstants.COLUMN_TRADEDATE) || selectedCols.Name.Equals(CashManagementConstants.COLUMN_ENTRYDATE) || selectedCols.Name.Equals(CashManagementConstants.COLUMN_MODIFYDATE)) && selectedCols.FilterConditionList.Count == 1 && selectedCols.FilterConditionList[0].ComparisionOperator == FilterComparisionOperator.StartsWith && selectedCols.FilterConditionList[0].CompareValue.Equals("(Today)"))
                                        {
                                            grdActivity.DisplayLayout.Bands[0].ColumnFilters[col].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.ToString(DateTimeConstants.DateformatForClosing));
                                        }
                                        else
                                        {
                                            grdActivity.DisplayLayout.Bands[0].ColumnFilters[col].FilterConditions.Add(filCond);
                                        }
                                    }
                                    grdActivity.DisplayLayout.Bands[0].ColumnFilters[col].LogicalOperator = selectedCols.FilterLogicalOperator;
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

        private void InitializeValueList()
        {
            try
            {
                #region Account ValueList
                AccountCollection accountCollection = CachedDataManager.GetInstance.GetUserAccounts();
                ValueList vlAccount = new ValueList();
                ValueList vlCurrency = new ValueList();
                ValueList vlActivity = new ValueList();
                //ValueList vlActivitySourceType = new ValueList();
                //vlAccount.ValueListItems.Add(0, "Select");
                //vlCurrency.ValueListItems.Add(0, "Select");
                //vlActivity.ValueListItems.Add(0, "Select");
                foreach (Account account in accountCollection)
                {
                    if (account.AccountID != int.MinValue)
                    {
                        vlAccount.ValueListItems.Add(account.AccountID, account.Name);
                    }
                }
                grdActivity.DisplayLayout.Bands[0].Columns["AccountName"].ValueList = vlAccount;
                #endregion

                #region Currency ValueList
                Dictionary<int, String> dictCurrency = new Dictionary<int, string>();
                dictCurrency = CachedDataManager.GetInstance.GetAllCurrencies();
                foreach (int key in dictCurrency.Keys)
                {
                    vlCurrency.ValueListItems.Add(key, dictCurrency[key]);
                }
                grdActivity.DisplayLayout.Bands[0].Columns["CurrencyName"].ValueList = vlCurrency;
                #endregion
                #region Settlement Currency ValueList
                _vlCurrencies = new ValueList();
                foreach (KeyValuePair<int, string> item in dictCurrency)
                {
                    _vlCurrencies.ValueListItems.Add(item.Value, item.Value);
                }
                _vlCurrencies.ValueListItems.Add(0, ApplicationConstants.C_COMBO_NONE);
                #endregion

                #region ActivityType ValueList
                Dictionary<string, int> dictActivity = new Dictionary<string, int>();
                dictActivity = CachedDataManager.GetActivityType();
                foreach (string key in dictActivity.Keys)
                {
                    vlActivity.ValueListItems.Add(dictActivity[key], key);
                }
                grdActivity.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_ACTIVITYTYPE].ValueList = vlActivity;
                #endregion

                //Added by : Manvendra Prajapati
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-5969
                //Purpose : Set the transaction source value list for the column


                _vlTransactionSource = new ValueList();

                foreach (CashTransactionType cashType in Enum.GetValues(typeof(CashTransactionType)))
                {
                    _vlTransactionSource.ValueListItems.Add(cashType.ToString(), EnumHelper.GetDescription(cashType));
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

        private void btnGetActivities_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!ValidateCashManagementStartDate())
                //{
                //    MessageBox.Show("Some of the selected accounts do not have transaction on or before the selected Date. De-select those accounts and try again.","Alert",MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                if (DateTime.Compare(Convert.ToDateTime(dtPickerUpper.DateTime), Convert.ToDateTime(dtPickerlower.DateTime)) >= 0)
                {
                    string accountIds = ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',');
                    if (!CashDataManager.GetInstance().GetIsRevaluationInProgress(accountIds))
                        GetDataAsync();
                    else
                        MessageBox.Show("Revaluation is in progress for selected fund(s). Proceed after revaluation is done.", "Revaluation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("To Date is before From Date", "Activity", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);


            }

        }

        /// <summary>
        /// added by: Bharat raturi, 16 jul 2014
        /// check whether all of the accounts have cash management date before the mentioned data
        /// </summary>
        /// <returns>true, if date is valid for all accounts</returns>
        //private bool ValidateCashManagementStartDate()
        //{
        //    foreach (int accountID in ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetSelectedItemsInDictionary().Keys)
        //    {
        //        if (CashDataManager.GetInstance().GetCashPreferences(accountID) == null || CashDataManager.GetInstance().GetCashPreferences(accountID).CashMgmtStartDate > Convert.ToDateTime(dtPickerlower.Value))
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        private void GetDataAsync()
        {
            try
            {
                Subscribe();
                BackgroundWorker bgwrkr = new BackgroundWorker();
                bgwrkr.DoWork += new DoWorkEventHandler(bgwrkr_DoWork);
                bgwrkr.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwrkr_RunWorkerCompleted);
                changeStatus(false);
                ClearData();
                bgwrkr.RunWorkerAsync(new object[] { dtPickerlower.DateTime, dtPickerUpper.DateTime, ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(','), ultraOSDateSelection.CheckedItem.DisplayText });
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

        private void ClearData()
        {
            try
            {
                lsCashActivityToBind.Clear();
                lsModifiedActivity.Clear();
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

        private void changeStatus(bool isWorkCompleted)
        {
            try
            {
                if (isWorkCompleted)
                {
                    btnGetActivities.Text = "Get Data";
                    toolStripActivity.Text = "";

                    ugbxActivityParams.Enabled = true;
                }
                else
                {
                    btnGetActivities.Text = "Getting.....";
                    toolStripActivity.Text = "Getting.....";
                    ugbxActivityParams.Enabled = false;

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

        void bgwrkr_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //modified by: Bharat raturi, 16 jul 2014
                //purpose: Get the data for the accountIDs sent as comma separated value string
                //CashDataManager.GetInstance().GetActivity(dtPickerlower.DateTime, dtPickerUpper.DateTime);
                object[] arguments = e.Argument as object[];
                CashDataManager.GetInstance().GetActivity((DateTime)arguments[0], (DateTime)arguments[1], (string)arguments[2], (string)arguments[3]);
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
        /// Added by: Bharat raturi, 15 jul 2014
        /// Get the Comma separated accountIDs in the form of string
        /// </summary>
        /// <returns>String holding the comma separated AccountIDs</returns>
        //private String GetCommaSeparatedAccountIDs()
        //{
        //    String accountIDs = string.Empty;
        //    foreach (int accountID in ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetSelectedItemsInDictionary().Keys)
        //    {
        //        accountIDs += accountID.ToString() + ",";
        //    }
        //    return accountIDs.Trim(',');
        //}

        void bgwrkr_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (!this.IsDisposed)
                {
                    lsCashActivityToBind.Clear();
                    HelperClass.GlobalGridSetting(grdActivity.DisplayLayout.Bands[0]);
                    lsCashActivityToBind.AddList(DeepCopyHelper.Clone(CashDataManager.GetInstance().CashActivity));

                    foreach (CashActivity ca in lsCashActivityToBind)
                    {
                        ca.Amount = ca.Amount - ca.TotalCommission;
                    }

                    //Added by : Manvendra Prajapati
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-5969
                    //Purpose : Set the transaction source value list for the column

                    if (grdActivity != null && grdActivity.DisplayLayout.Bands.Count > 0 && grdActivity.DisplayLayout.Bands[0].Columns.Exists("TransactionSource"))
                    {
                        grdActivity.DisplayLayout.Bands[0].Columns["TransactionSource"].ValueList = _vlTransactionSource;
                    }
                    if (grdActivity != null && grdActivity.DisplayLayout.Bands.Count > 0 && grdActivity.DisplayLayout.Bands[0].Columns.Exists(OrderFields.PROPERTY_SETTLEMENTCURRENCY))
                    {
                        grdActivity.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCY].ValueList = _vlCurrencies;
                    }
                    if (grdActivity != null && grdActivity.DisplayLayout.Bands.Count > 0 && grdActivity.DisplayLayout.Bands[0].Columns.Exists(OrderFields.PROPERTY_SETTLCURRENCYID))
                    {
                        grdActivity.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SETTLCURRENCYID].Hidden = true;
                        grdActivity.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SETTLCURRENCYID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    }
                    changeStatus(true);
                    InitializeGridLayout();
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


        #region Subscribe Section

        DuplexProxyBase<ISubscription> _proxy;
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
                    FilterDataByDateRange filterdata = new FilterDataByDateRange();
                    filterdata.FromDate = dtPickerlower.DateTime.Date;
                    filterdata.ToDate = dtPickerUpper.DateTime.Date;
                    List<FilterData> filters = new List<FilterData>();
                    filters.Add(filterdata);
                    _proxy.Subscribe(Topics.Topic_CashActivity, filters);
                    _proxy.Subscribe(Topics.Topic_ManualJournalActivity, filters);
                    _proxy.Subscribe(Topics.Topic_HistoricalJournalActivity, filters);
                    _proxy.Subscribe(Topics.Topic_RevaluationActivity, null);
                    _proxy.Subscribe(Topics.Topic_ActivityType, null);
                    _proxy.Subscribe(Topics.Topic_ActivityJournalMapping, null);


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

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                CashActivity newCashActivity = new CashActivity();
                newCashActivity.ActivityId = CashDataManager.CashManagementServices.InnerChannel.GenerateID();
                newCashActivity.ActivityTypeId = newCashActivity.ActivityTypeId;
                newCashActivity.FKID = newCashActivity.FKID;
                newCashActivity.TransactionSource = CashTransactionType.ManualJournalEntry;
                newCashActivity.ActivityState = ApplicationConstants.TaxLotState.New;
                newCashActivity.UniqueKey = newCashActivity.GetKey();
                //isNewRowAdded = true;
                lsCashActivityToBind.Add(newCashActivity);
                lsModifiedActivity.Add(newCashActivity);

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

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CashActivity cashActivity = grdActivity.ActiveRow.ListObject as CashActivity;
                if (cashActivity != null)
                {
                    if (!lsModifiedActivity.Contains(cashActivity))
                        lsModifiedActivity.Add(cashActivity);
                    else if (lsModifiedActivity[lsModifiedActivity.IndexOf(cashActivity)].ActivityState == ApplicationConstants.TaxLotState.New)
                        lsModifiedActivity.Remove(cashActivity);

                    cashActivity.ActivityState = ApplicationConstants.TaxLotState.Deleted;

                    if (lsCashActivityToBind.Contains(cashActivity))
                        lsCashActivityToBind.Remove(cashActivity);
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

        //bool isNewRowAdded = false;
        private void grdActivity_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                //if (!e.ReInitialize && !e.Row.IsGroupByRow && !e.Row.IsSummaryRow)
                //{
                //    CashActivity cashActivity = e.Row.ListObject as CashActivity;
                //    //TransactionSource updated form ImportedEditableData to CashTransaction.
                //    if (cashActivity.ActivitySource != TransactionSource.CashTransaction && cashActivity.ActivitySource != TransactionSource.ManualEntry)
                //        e.Row.Activation = Activation.NoEdit;
                //    //e.Row.Cells["CurrencyName"].Value = e.Row.Cells["CurrencyName"].Column.ValueList.GetValue(0);

                //    if (isNewRowAdded)
                //    {
                //        setErrorsForNewEntry(e);
                //        isNewRowAdded = false;
                //    }
                //}
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

        //private void setErrorsForNewEntry(InitializeRowEventArgs e)
        //{
        //    try
        //    {
        //        CashActivity newcashActivity = e.Row.ListObject as CashActivity;
        //        foreach (UltraGridCell currentCell in e.Row.Cells)
        //            newcashActivity.properityChanged(currentCell.Column.Key, currentCell.Text);
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //}

        void grdActivity_BeforeRowFilterDropDown(object sender, Infragistics.Win.UltraWinGrid.BeforeRowFilterDropDownEventArgs e)
        {
            try
            {
                if (e.Column.Key.Equals(CashManagementConstants.COLUMN_TRADEDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_MODIFYDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_ENTRYDATE))
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

        void grdActivity_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(CashManagementConstants.COLUMN_TRADEDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_MODIFYDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_ENTRYDATE)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    grdActivity.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    grdActivity.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
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
        private void grdActivity_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                //if (e.Cell.Row.ListObject is CashActivity)
                //{
                //    if (e.Cell.Column.Header.Caption != "Symbol")
                //    {
                //        CashActivity cashActivity = e.Cell.Row.ListObject as CashActivity;
                //        cashActivity.properityChanged(e.Cell.Column.Key, e.Cell.Text);
                //    }                    
                //}
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

        //private void grdActivity_MouseDown(object sender, MouseEventArgs e)
        //{
        //    try
        //    {
        //        //HelperClass.ActivateRow(sender, e);
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        //Now activities will be generated from cash transaction UI,
        //So there is no need of save button

        //private void btnSave_Click(object sender, EventArgs e)

        //{
        //    bool isErrorsExists = false;
        //    // bool blnChangeDivDs = false;
        //    try
        //    {
        //        if(grdActivity.ActiveCell!=null)
        //            grdActivity.ActiveCell.Row.Update();
        //        if (lsModifiedActivity.Count > 0)
        //        {
        //            foreach (CashActivity cashActivity in lsModifiedActivity)
        //            {
        //                if (cashActivity.Error != string.Empty)
        //                {
        //                    isErrorsExists = true;
        //                    break;
        //                }
        //            }

        //            if (!isErrorsExists)
        //            {
        //                if (CashDataManager.GetInstance().Save(lsModifiedActivity) > 0)
        //                {
        //                    lsModifiedActivity.Clear();
        //                    //MessageBox.Show("Data Saved", "Information");

        //                    toolStripActivity.Text = "Data Saved";
        //                }
        //            }
        //            else
        //            {
        //                //MessageBox.Show("Please Correct Errors.", "Information");
        //                toolStripActivity.Text = "Please Correct Errors.";
        //            }
        //        }                
        //        else
        //        {
        //            toolStripActivity.Text = "Nothing to Save.";
        //            //MessageBox.Show("Nothing to Save", "Information");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        btnSave.Text = "Save";
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }

        //    }
        //}

        private void grdActivity_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                //if (e.Cell.Row.ListObject is CashActivity)
                //{
                //    CashActivity objEntry = e.Cell.Row.ListObject as CashActivity;

                //    if (objEntry != null)
                //    {
                //        if (e.Cell.Column.Header.Caption == "Symbol")
                //        {
                //            objEntry.Symbol = e.Cell.Text;
                //            objEntry.properityChanged(e.Cell.Column.Key, e.Cell.Text);
                //            CashDataManager.GetInstance().ValidateSymbol(objEntry);
                //        }
                //        else if ((e.Cell.Column.Header.Caption == "Currency"))
                //        {
                //            objEntry.CurrencyID = Convert.ToInt32(e.Cell.Value);
                //            objEntry.CurrencyName = e.Cell.Text;
                //        }
                //        else if (e.Cell.Column.Header.Caption == "Account")
                //        {
                //            objEntry.AccountID = Convert.ToInt32(e.Cell.Value);
                //            objEntry.AccountName = e.Cell.Text;
                //        }
                //        else if (e.Cell.Column.Key == "ActivityType")
                //        {   
                //            //objEntry.ActivityType = (Activities)Enum.Parse(typeof(Activities), e.Cell.Text);
                //            objEntry.ActivityType = e.Cell.Text;
                //            objEntry.ActivityTypeId = CachedDataManager.GetActivityTypeID(objEntry.ActivityType.ToString());
                //        }
                //        else if (e.Cell.Column.Key == "Date")
                //            objEntry.UniqueKey = objEntry.GetKey();

                //        if (!lsModifiedActivity.Contains(objEntry))
                //            lsModifiedActivity.Add(objEntry);                       
                //    }
                //}
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (ExportToExcel())
                {
                    // MessageBox.Show("Report Successfully saved.", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    toolStripActivity.Text = "Report Successfully Saved.";
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
        private bool ExportToExcel()
        {
            bool result = false;
            try
            {
                Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook();
                string pathName = null;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = Application.StartupPath;
                saveFileDialog1.Filter = "Excel WorkBook Files (*.xls)|*.xls|All Files (*.*)|*.*";
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pathName = saveFileDialog1.FileName;
                }
                else
                {
                    return result;

                }
                string workbookName = "Report" + DateTime.Now.Date.ToString("yyyyMMdd");
                workBook.Worksheets.Add(workbookName);

                workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[workbookName];

                workBook = this.ultraGridExcelExporter1.Export(grdActivity, workBook.Worksheets[workbookName]);
                workBook.Save(pathName);
                result = true;
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

            }
            return result;
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
                    switch (topicName)
                    {
                        case Topics.Topic_CashActivity:
                            publishDataList = (System.Object[])e.EventData;
                            foreach (Object obj in publishDataList)
                            {
                                CashActivity cashActivity = obj as CashActivity;
                                cashActivity.Amount = cashActivity.Amount - cashActivity.TotalCommission;
                                HelperClass.UpdateList(cashActivity, lsCashActivityToBind);
                            }
                            break;
                        case Topics.Topic_ManualJournalActivity:
                        case Topics.Topic_HistoricalJournalActivity:
                            publishDataList = (System.Object[])e.EventData;
                            foreach (Object obj in publishDataList)
                            {
                                CashActivity cashActivity = obj as CashActivity;

                                HelperClass.UpdateList(cashActivity, lsCashActivityToBind);
                            }
                            break;
                        case Topics.Topic_RevaluationActivity:
                            publishDataList = (System.Object[])e.EventData;
                            foreach (Object obj in publishDataList)
                            {
                                if (obj is String)
                                    toolStripActivity.Text = obj as string;
                                else
                                    HelperClass.UpdateList(obj as CashActivity, lsCashActivityToBind);

                            }
                            break;

                        case Topics.Topic_ActivityType:

                            System.Object[] dataList = null;

                            List<String> li = new List<string>();
                            dataList = (System.Object[])e.EventData;

                            foreach (object obj in dataList)
                                li.Add(obj.ToString());

                            DataTable newActivityType = Prana.Utilities.MiscUtilities.DataTableToListConverter.GetDataTableFromList(li);

                            //updating  Dictionary  ActivityType
                            foreach (DataRow row in newActivityType.Rows)
                                CachedDataManager.GetActivityType().Add(row[1].ToString().Trim(), int.Parse(row[0].ToString().Trim()));

                            //updating activity tables Dataset 
                            DataSet dsActivityTables = CachedDataManager.GetInstance.GetAllActivityTables();
                            dsActivityTables.Tables[CashManagementConstants.TABLE_ACTIVITYTYPE].Merge(newActivityType);
                            dsActivityTables.AcceptChanges();



                            break;

                        case Topics.Topic_ActivityJournalMapping:

                            System.Object[] dataList1 = null;
                            List<String> li1 = new List<string>();
                            dataList1 = (System.Object[])e.EventData;

                            foreach (object obj in dataList1)
                                li1.Add(obj.ToString());


                            DataTable newActivityJournalMapping = Prana.Utilities.MiscUtilities.DataTableToListConverter.GetDataTableFromList(li1);

                            //updating activity tables Dataset 
                            DataSet dsActivityMappingTables = CachedDataManager.GetInstance.GetAllActivityTables();
                            dsActivityMappingTables.Tables[CashManagementConstants.TABLE_ACTIVITYJOURNALMAPPING].Merge(newActivityJournalMapping);
                            dsActivityMappingTables.AcceptChanges();


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
            return "ctrlEditableActivity";
        }

        #endregion
        private void ctrlEditableActivity_Load(object sender, EventArgs e)
        {
            try
            {
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    this.statusStrip1.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripActivity.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    this.toolStripActivity.ForeColor = System.Drawing.Color.WhiteSmoke;
                }
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
                ctrlMasterFundAndAccountsDropdown1.Setup();
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
        /// Used for changing the color of buttons. The indices and their colors are as follows:
        /// 0 & 3: For the Green Shade
        /// 1 & 4: For the Neutral Shade
        /// 2 & 5: For the Red Shade 
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                btnExport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnExport.ForeColor = System.Drawing.Color.White;
                btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnExport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnExport.UseAppStyling = false;
                btnExport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGetActivities.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetActivities.ForeColor = System.Drawing.Color.White;
                btnGetActivities.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetActivities.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetActivities.UseAppStyling = false;
                btnGetActivities.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void grdActivity_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            HelperClass.ActivitySummarySettings(e);
            grdActivity.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;

        }

        private void grdActivity_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            HelperClass.GroupByRowSetting(e);
        }

        private void grdActivity_AfterSortChange(object sender, BandEventArgs e)
        {
            if (grdActivity.Rows.IsGroupByRows)
                grdActivity.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.InGroupByRows;
            else
                grdActivity.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;

        }

        private void grdActivity_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {

                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdActivity);
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

        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveGridLayout saveGridLayout = new SaveGridLayout();
                CashManagementLayout cashManagementLayout = saveGridLayout.GetLayout(grdActivity, "Activity");
                CashPreferenceManager.GetInstance().SetCashGridLayout(cashManagementLayout, "Activity");
                ActivityLayout = cashManagementLayout;
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

        private void grdActivity_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
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
