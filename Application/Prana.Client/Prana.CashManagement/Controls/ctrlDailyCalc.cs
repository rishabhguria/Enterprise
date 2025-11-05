using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.CashManagement.Classes;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prana.CashManagement.Controls
{
    public partial class ctrlDailyCalc : UserControl
    {
        public ctrlDailyCalc()
        {
            InitializeComponent();
        }

        private const string allOpenPositions = "DailyCalculations_AllOpenPositions";
        private bool isCalculatedDataChanged = false;
        private bool _allowTabChange;
        private bool _isChRelease = false;
        bool _isNewGrid = false;
        List<TradeAuditEntry> _tradeAuditCollection_DailyCalculation = new List<TradeAuditEntry>();
        private List<ctrlDataGrid> noOfOpenPositionTab;

        public bool AllowTabChange
        {
            get
            {
                try
                {
                    _allowTabChange = true;
                    if (isCalculatedDataChanged)
                    {
                        DialogResult dlgResult = AskSaveConfirmation();
                        if (dlgResult.Equals(DialogResult.Cancel))
                        {
                            _allowTabChange = false;
                        }
                        else
                        {
                            _allowTabChange = true;
                            isCalculatedDataChanged = false;
                            if (dlgResult.Equals(DialogResult.Yes))
                            {
                                SaveDataAsync();
                            }
                            else
                            {
                                btnGetData_Click(null, null);
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
                return _allowTabChange;
            }
        }

        #region Global variables

        List<CashActivity> _lsAlreadyCalculatedM2M = new List<CashActivity>();
        List<CashActivity> _lsNewCalculatedM2M = new List<CashActivity>();
        List<CashActivity> _lsM2MToPersist = new List<CashActivity>();

        private Dictionary<string, List<TaxLot>> _dicOpenPositionsAndTransactionsDateWise;
        public Dictionary<string, List<TaxLot>> DicOpenPositionsAndTransactionsDateWise
        {
            get { return _dicOpenPositionsAndTransactionsDateWise; }
            set { _dicOpenPositionsAndTransactionsDateWise = value; }
        }
        #endregion

        #region UI Functions

        private CashManagementLayout _cashManagementLayoutForAllOpenPositions = null;
        public CashManagementLayout CashManagementLayoutForAllOpenPositions
        {
            get { return _cashManagementLayoutForAllOpenPositions; }
            set { _cashManagementLayoutForAllOpenPositions = value; }
        }

        private CashManagementLayout _cashManagementLayoutForCalculatedTransactions = null;
        public CashManagementLayout CashManagementLayoutForCalculatedTransactions
        {
            get { return _cashManagementLayoutForCalculatedTransactions; }
            set { _cashManagementLayoutForCalculatedTransactions = value; }
        }

        private void ctrlDailyCalc_Load(object sender, EventArgs e)
        {
            try
            {
                noOfOpenPositionTab = new List<ctrlDataGrid>();
                InitializeCalculationGrid();
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
                ctrlMasterFundAndAccountsDropdown1.Setup();
                InitializeGridLayout(CashManagementLayoutForCalculatedTransactions, ugCalculatedTransactions);
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
                btnCalculate.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnCalculate.ForeColor = System.Drawing.Color.White;
                btnCalculate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCalculate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCalculate.UseAppStyling = false;
                btnCalculate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

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

        #region GetData
        private void btnGetData_Click(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Compare(Convert.ToDateTime(udtToDate.DateTime), Convert.ToDateTime(udtFromDate.DateTime)) >= 0)
                {
                    if (ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetNoOfCheckedItems() < 1)
                    {
                        MessageBox.Show("Please select at least one account to proceed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    string accountIds = ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',');
                    if (!CashDataManager.GetInstance().GetIsRevaluationInProgress(accountIds))
                    {
                        ChangeStatus(false, true);
                        GetDataAsync(udtFromDate.DateTime, udtToDate.DateTime, accountIds);
                    }
                    else
                    {
                        MessageBox.Show("Revaluation is in progress for selected fund(s). Proceed after revaluation is done.", "Revaluation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("To Date is before From Date.", "Daily Calculations", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        public async void GetDataAsync(DateTime fromDate, DateTime toDate, String accountIds)
        {
            try
            {
                bool result = await getData(fromDate, toDate, accountIds);

                lsToBind.Clear();
                lsToBind.AddList(_lsAlreadyCalculatedM2M);
                InitializeOpenPositionGrid();
                ChangeStatus(true, true);
                InitializeGridLayout(CashManagementLayoutForCalculatedTransactions, ugCalculatedTransactions);
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

        //Modified by: Bharat Raturi, 15 jul 2014
        //purpose: pass an extra parameter to provide accountIDs
        //private void getData(DateTime fromDate,DateTime toDate)
        private async Task<bool> getData(DateTime fromDate, DateTime toDate, String accountIds)
        {
            try
            {
                isCalculatedDataChanged = false;
                if (_lsNewCalculatedM2M != null)
                    _lsNewCalculatedM2M.Clear();
                DicOpenPositionsAndTransactionsDateWise = new Dictionary<string, List<TaxLot>>();
                DicOpenPositionsAndTransactionsDateWise = await CashDataManager.GetInstance().GetOpenPositionsForDailyCalculation(fromDate, toDate, accountIds);
                _lsAlreadyCalculatedM2M = await CashDataManager.GetInstance().GetAlreadyCalculatedDailyCashData(fromDate, toDate, accountIds);
                NameValueFiller.SetNameValues(_lsAlreadyCalculatedM2M);
                return true;
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
            return false;
        }
        #endregion

        #region Calculate
        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                string accountIds = ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',');
                if (!CashDataManager.GetInstance().GetIsRevaluationInProgress(accountIds))
                {
                    if (!CachedDataManager.GetInstance.ValidateNAVLockDate(udtFromDate.DateTime))
                    {
                        MessageBox.Show("The start date you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                            + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    ChangeStatus(false, false);
                    BackgroundWorker bgwrkr = new BackgroundWorker();
                    bgwrkr.DoWork += new DoWorkEventHandler(bgwrkrCalculatedData_DoWork);
                    bgwrkr.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwrkrCalculatedData_RunWorkerCompleted);
                    bgwrkr.RunWorkerAsync();
                }
                else
                {
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

        void bgwrkrCalculatedData_DoWork(object sender, DoWorkEventArgs e)
        {
            Calculate();
        }

        void bgwrkrCalculatedData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                return;
            }
            try
            {
                HelperClass.GlobalGridSetting(ugCalculatedTransactions.DisplayLayout.Bands[0]);
                lsToBind.Clear();
                lsToBind.AddList(_lsNewCalculatedM2M);
                ChangeStatus(true, false);
                InitializeGridLayout(CashManagementLayoutForCalculatedTransactions, ugCalculatedTransactions);
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

        private void Calculate()
        {
            try
            {
                if (_lsM2MToPersist != null)
                {
                    _lsM2MToPersist.Clear();
                    if (_lsAlreadyCalculatedM2M != null && _lsAlreadyCalculatedM2M.Count > 0)
                    {
                        _lsAlreadyCalculatedM2M.ForEach(act => act.ActivityState = ApplicationConstants.TaxLotState.Deleted);
                        _lsM2MToPersist.AddRange(_lsAlreadyCalculatedM2M);
                    }
                }
                if (DicOpenPositionsAndTransactionsDateWise != null && DicOpenPositionsAndTransactionsDateWise.Count > 0)
                {
                    _lsNewCalculatedM2M = CashDataManager.GetInstance().CalculateDailyCashImpact(DicOpenPositionsAndTransactionsDateWise);
                    if (_lsNewCalculatedM2M != null && _lsNewCalculatedM2M.Count > 0)
                        NameValueFiller.SetNameValues(_lsNewCalculatedM2M);

                    if (_lsM2MToPersist != null)
                    {
                        if (_lsNewCalculatedM2M != null && _lsNewCalculatedM2M.Count > 0)
                            _lsM2MToPersist.AddRange(_lsNewCalculatedM2M);
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
        #endregion

        #region Save
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string accountIds = ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',');
                if (!CashDataManager.GetInstance().GetIsRevaluationInProgress(accountIds))
                {
                    btnGetData.Enabled = false;
                    btnCalculate.Enabled = false;
                    btnSave.Enabled = false;
                    toolStripStatusLabel1.Text = "Saving Daily Calculation Data...";
                    SaveDataAsync();
                }
                else
                {
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

        public async void SaveDataAsync()
        {
            try
            {
                int result = await SaveData();
                toolStripStatusLabel1.Text = "Daily Calculation Data Saved.";
                if (!CustomThemeHelper.ApplyTheme)
                {
                    toolStripStatusLabel1.ForeColor = System.Drawing.Color.DarkGreen;
                }
                btnGetData.Enabled = true;
                btnCalculate.Enabled = true;
                btnSave.Enabled = true;
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

        private async Task<int> SaveData()
        {
            try
            {
                if (_lsM2MToPersist != null && _lsM2MToPersist.Count > 0)
                {
                    await CashDataManager.GetInstance().SaveDailyCalculations(_lsM2MToPersist);
                    Dictionary<string, DateTime> dcAuditTrail = new Dictionary<string, DateTime>();
                    foreach (CashActivity cash in _lsM2MToPersist)
                    {
                        if (!dcAuditTrail.ContainsKey(Convert.ToString(cash.AccountID) + '_' + cash.Symbol))
                            dcAuditTrail.Add((Convert.ToString(cash.AccountID) + '_' + cash.Symbol), cash.Date.Date);
                        else
                        {
                            if (dcAuditTrail[Convert.ToString(cash.AccountID) + '_' + cash.Symbol] > cash.Date.Date)
                                dcAuditTrail[Convert.ToString(cash.AccountID) + '_' + cash.Symbol] = cash.Date.Date;
                        }
                    }
                    AddDailyCalculationAuditEntry(dcAuditTrail, TradeAuditActionType.ActionType.DailyCalculation, "Daily Calculation Changed", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                    AuditManager.Instance.SaveAuditList(_tradeAuditCollection_DailyCalculation);
                    DicOpenPositionsAndTransactionsDateWise.Clear();
                    _lsM2MToPersist.Clear();
                    dcAuditTrail.Clear();
                    _tradeAuditCollection_DailyCalculation.Clear();
                }
                isCalculatedDataChanged = false;
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
        #endregion

        #endregion

        public bool AddDailyCalculationAuditEntry(Dictionary<string, DateTime> dicAudittrail, TradeAuditActionType.ActionType action, string comment, int currentUserID)
        {
            try
            {
                if (dicAudittrail != null && dicAudittrail.Count > 0)
                {
                    foreach (KeyValuePair<string, DateTime> trail in dicAudittrail)
                    {
                        TradeAuditEntry newEntry = new TradeAuditEntry();
                        newEntry.Action = action;
                        newEntry.AUECLocalDate = DateTime.Now;
                        newEntry.OriginalDate = trail.Value;
                        newEntry.Comment = comment;
                        newEntry.CompanyUserId = currentUserID;
                        newEntry.TaxLotID = int.MinValue.ToString();
                        newEntry.GroupID = int.MinValue.ToString();
                        newEntry.TaxLotClosingId = "";
                        newEntry.OrderSideTagValue = "";
                        newEntry.OriginalValue = "";
                        newEntry.Symbol = trail.Key.Split('_').Last();
                        newEntry.Level1ID = Convert.ToInt32(trail.Key.Split('_').First());
                        newEntry.Source = Prana.BusinessObjects.TradeAuditActionType.ActionSource.Cash;
                        _tradeAuditCollection_DailyCalculation.Add(newEntry);
                    }
                }
                else
                    throw new NullReferenceException("The Data Set to add in audit dictionary is null");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return true;
        }

        #region Private Functions

        private void ChangeStatus(bool workCompleted, bool isGetData)
        {
            if (workCompleted)
            {
                btnGetData.Text = "Get Data";
                btnCalculate.Text = "Calculate";
                ugbxDailyCalc.Enabled = true;
                toolStripStatusLabel1.Text = "";
            }
            else
            {
                if (isGetData)
                {
                    btnGetData.Text = "Getting Data...";
                    toolStripStatusLabel1.Text = "Getting Data....";
                    if (!CustomThemeHelper.ApplyTheme)
                    {
                        toolStripStatusLabel1.ForeColor = System.Drawing.Color.DarkGreen;
                    }
                }
                else
                {
                    btnCalculate.Text = "Calculating...";
                    toolStripStatusLabel1.Text = "Calculating...";
                    if (!CustomThemeHelper.ApplyTheme)
                    {
                        toolStripStatusLabel1.ForeColor = System.Drawing.Color.Blue;
                    }
                }
                ugbxDailyCalc.Enabled = false;
            }
        }

        private DialogResult AskSaveConfirmation()
        {
            DialogResult dlgResult = DialogResult.No;
            dlgResult = ConfirmationMessageBox.Display("Do you want to save the changes done in the data?", "Daily Calculation Save");
            return dlgResult;
        }

        GenericBindingList<CashActivity> lsToBind = new GenericBindingList<CashActivity>();
        private void InitializeCalculationGrid()
        {
            try
            {
                if (!this.IsDisposed)
                {
                    CashActivity cashActivity = new CashActivity();
                    lsToBind.Add(cashActivity);
                    ugCalculatedTransactions.DataSource = lsToBind;
                    HelperClass.SetColumnDisplayNames(ugCalculatedTransactions, null);
                    lsToBind.Clear();
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

        private void InitializeOpenPositionGrid()
        {
            try
            {
                if (!this.IsDisposed)
                {
                    ctrlDataGrid ctrlGrid;
                    UltraGrid grd;
                    if (noOfOpenPositionTab != null && noOfOpenPositionTab.Count > 0)
                    {
                        for (int counter = 0; counter < noOfOpenPositionTab.Count; counter++)
                        {
                            noOfOpenPositionTab[counter].CashLayout -= new EventHandler<EventArgs<CashManagementLayout>>(SetLayoutForAllOpenPositionsGrid);
                            noOfOpenPositionTab[counter].Dispose();
                            noOfOpenPositionTab[counter] = null;
                        }
                    }
                    noOfOpenPositionTab.Clear();
                    tbctrlAllOpenPositions.Tabs.Clear();
                    if (DicOpenPositionsAndTransactionsDateWise != null && DicOpenPositionsAndTransactionsDateWise.Count > 0)
                    {
                        _isNewGrid = true;
                        foreach (string givenDate in DicOpenPositionsAndTransactionsDateWise.Keys)
                        {
                            tbctrlAllOpenPositions.Tabs.Add(givenDate, givenDate);

                            ctrlGrid = new ctrlDataGrid();
                            ctrlGrid.KeyForXML = allOpenPositions;
                            ctrlGrid.CashLayout += new EventHandler<EventArgs<CashManagementLayout>>(SetLayoutForAllOpenPositionsGrid);
                            CustomThemeHelper.SetThemeProperties(ctrlGrid, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CASH_MANAGEMENT);
                            grd = (UltraGrid)ctrlGrid.Controls["ultraGrid"];
                            grd.AfterColPosChanged += new AfterColPosChangedEventHandler(grd_AfterColPosChanged);
                            grd.InitializeRow += new InitializeRowEventHandler(grd_InitializeRow);
                            grd.InitializeLayout += new InitializeLayoutEventHandler(grd_InitializeLayout);

                            tbctrlAllOpenPositions.Tabs[givenDate].TabPage.Controls.Add(grd);

                            grd.DataSource = DicOpenPositionsAndTransactionsDateWise[givenDate];
                            HelperClass.SetTaxlotDisplayNames(grd);
                            grd.DisplayLayout.Override.RowSelectors = DefaultableBoolean.True;
                            if (isOpenPositionDisplayChanged.Equals(true))
                            {
                                grd.DisplayLayout.Load(allopenpositiondisplay, PropertyCategories.All);
                            }
                            if (_isChRelease)
                            {
                                grd.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
                                btnSave.Enabled = false;
                            }
                            InitializeGridLayout(CashManagementLayoutForAllOpenPositions, grd);
                            noOfOpenPositionTab.Add(ctrlGrid);
                        }
                        _isNewGrid = false;
                    }
                    else
                    {
                        ctrlGrid = new ctrlDataGrid();
                        CustomThemeHelper.SetThemeProperties(ctrlGrid, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CASH_MANAGEMENT);
                        grd = (UltraGrid)ctrlGrid.Controls["ultraGrid"];
                        tbctrlAllOpenPositions.Tabs.Add("0", "Positions");
                        tbctrlAllOpenPositions.Tabs[0].TabPage.Controls.Add(grd);
                        noOfOpenPositionTab.Add(ctrlGrid);
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

        void grd_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGrid grd = sender as UltraGrid;
                if (grd.DataSource != null && grd.DisplayLayout.Bands[0].Columns.Exists("AvgPrice"))
                    grd.DisplayLayout.Bands[0].Columns["AvgPrice"].Format = "#,##0.00";
                #region Currency ValueList
                if (grd != null && grd.DisplayLayout.Bands.Count > 0 && grd.DisplayLayout.Bands[0].Columns.Exists(OrderFields.PROPERTY_SETTLEMENTCURRENCYID))
                {
                    ValueList vlCurrency = new ValueList();
                    foreach (KeyValuePair<int, string> kvp in CachedDataManager.GetInstance.GetAllCurrencies())
                    {
                        vlCurrency.ValueListItems.Add(kvp.Key, kvp.Value);
                    }
                    grd.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].ValueList = vlCurrency;
                    grd.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].Header.Caption = OrderFields.CAPTION_SETTLEMENT_CURRENCY;
                }
                #endregion
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

        void grd_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                double avgPrice = Convert.ToDouble(e.Row.Cells["AvgPrice"].Value);
                if (avgPrice >= 0)
                {
                    e.Row.Appearance.ForeColor = Color.FromArgb(39, 174, 96);
                }
                else
                {
                    e.Row.Appearance.ForeColor = Color.FromArgb(192, 57, 43);
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

        string allopenpositiondisplay = Application.StartupPath.ToString() + "\\" + "Prana Preferences" + "\\ " + "allpositiondisplay";
        bool isOpenPositionDisplayChanged = false;

        void grd_AfterColPosChanged(object sender, AfterColPosChangedEventArgs e)
        {
            if (_isNewGrid.Equals(false))
            {
                isOpenPositionDisplayChanged = true;
                UltraGrid grd = sender as UltraGrid;
                HelperClass.GlobalGridSetting(grd.DisplayLayout.Bands[0]);
                if (!grd.Equals(null))
                {
                    grd.DisplayLayout.Save(allopenpositiondisplay, PropertyCategories.All);
                }
                foreach (UltraTab tb in tbctrlAllOpenPositions.Tabs)
                {
                    if (!tb.Active.Equals(true))
                    {
                        grd = tb.TabPage.Controls[0] as UltraGrid;
                        grd.DisplayLayout.Load(allopenpositiondisplay, PropertyCategories.All);
                    }
                }
            }
        }
        #endregion

        private void ugCalculatedTransactions_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                HelperClass.ActivitySummarySettings(e);
                ugCalculatedTransactions.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;

                UltraGrid grd = sender as UltraGrid;
                if (grd != null && grd.DisplayLayout.Bands.Count > 0 && grd.DisplayLayout.Bands[0].Columns.Exists(OrderFields.PROPERTY_SETTLCURRENCYID))
                {
                    grd.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SETTLCURRENCYID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    grd.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SETTLCURRENCYID].Hidden = true;
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

        private void ugCalculatedTransactions_InitializeRow(object sender, InitializeRowEventArgs e)
        {

            try
            {

                //decimal DR = Convert.ToDecimal(e.Row.Cells["DR"].Value);
                //decimal CR = Convert.ToDecimal(e.Row.Cells["CR"].Value);

                //if (DR >= 0 || CR>=0)
                //{   
                //    e.Row.Appearance.ForeColor = Color.FromArgb(177, 216, 64);
                //}
                //else
                //{
                //    e.Row.Appearance.ForeColor = Color.FromArgb(255, 91, 71);
                //}
                //e.Row.Cells["Amount"].SetValue(Amount.ToString("#0.00"), true);

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

        private void ugCalculatedTransactions_AfterSortChange(object sender, BandEventArgs e)
        {
            if (ugCalculatedTransactions.Rows.IsGroupByRows)
            {
                ugCalculatedTransactions.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.InGroupByRows;
            }
            else
            {
                ugCalculatedTransactions.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
            }
        }

        void ugCalculatedTransactions_BeforeRowFilterDropDown(object sender, Infragistics.Win.UltraWinGrid.BeforeRowFilterDropDownEventArgs e)
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

        void ugCalculatedTransactions_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(CashManagementConstants.COLUMN_TRADEDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_MODIFYDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_ENTRYDATE)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    ugCalculatedTransactions.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    ugCalculatedTransactions.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
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

        private void egbAllTransaction_ExpandedStateChanged(object sender, EventArgs e)
        {
            if (egbAllTransaction.Expanded)
            {
                if (egbCalculatedTransactions.Expanded)
                    splitContainer1.SplitterDistance = this.FindForm().Height / 2 - 50;
                else
                    splitContainer1.SplitterDistance = this.FindForm().Height - 25;
            }
            else
            {
                if (egbCalculatedTransactions.Expanded)
                    splitContainer1.SplitterDistance = 25;
                else
                    splitContainer1.SplitterDistance = this.FindForm().Height / 2;
            }
        }

        private void egbCalculatedTransactions_ExpandedStateChanged(object sender, EventArgs e)
        {
            if (egbCalculatedTransactions.Expanded)
            {
                if (egbAllTransaction.Expanded)
                    splitContainer1.SplitterDistance = this.FindForm().Height / 2 - 50;
                else
                    splitContainer1.SplitterDistance = 25;
            }
            else
            {
                splitContainer1.SplitterDistance = this.FindForm().Height - 25;
            }
        }

        private void ugCalculatedTransactions_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            HelperClass.GroupByRowSetting(e);
        }

        private void ugCalculatedTransactions_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.ugCalculatedTransactions);
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
                                        if ((selectedCols.Name.Equals(CashManagementConstants.COLUMN_TRADEDATE) || selectedCols.Name.Equals(CashManagementConstants.COLUMN_ENTRYDATE) || selectedCols.Name.Equals(CashManagementConstants.COLUMN_MODIFYDATE)) && selectedCols.FilterConditionList.Count == 1 && selectedCols.FilterConditionList[0].ComparisionOperator == FilterComparisionOperator.StartsWith && selectedCols.FilterConditionList[0].CompareValue.Equals("(Today)"))
                                        {
                                            grd.DisplayLayout.Bands[0].ColumnFilters[col].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.ToString(DateTimeConstants.DateformatForClosing));
                                        }
                                        else
                                        {
                                            grd.DisplayLayout.Bands[0].ColumnFilters[col].FilterConditions.Add(filCond);
                                        }
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
                CashManagementLayout cashManagementLayout = saveGridLayout.GetLayout(ugCalculatedTransactions, "DailyCalculations");
                CashPreferenceManager.GetInstance().SetCashGridLayout(cashManagementLayout, "DailyCalculations_CalculatedTransactions");
                CashManagementLayoutForCalculatedTransactions = cashManagementLayout;
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
                if (ExportToExcelHelper.ExportToExcel(ugCalculatedTransactions))
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

        private void SetLayoutForAllOpenPositionsGrid(object sender, EventArgs<CashManagementLayout> e)
        {
            try
            {
                CashManagementLayoutForAllOpenPositions = e.Value;
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

        private void ugCalculatedTransactions_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }
    }
}
