using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.CashManagement.Classes;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.Utilities.UI;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Prana.CashManagement.Controls
{
    public partial class ctrlActivityExceptions : UserControl
    {
        public ctrlActivityExceptions()
        {
            try
            {
                InitializeComponent();
                BindGrid();
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

        GenericBindingList<CashActivity> _lsCashActivityToBind = new GenericBindingList<CashActivity>();
        bool IsGetException = false;
        bool IsOverride = false;
        string _activitySource = string.Empty;

        private void BindGrid()
        {
            try
            {
                if (!this.IsDisposed)
                {

                    CashActivity trEntryToInitiallizeGrid = new CashActivity();
                    if (_lsCashActivityToBind.Count == 0)
                        _lsCashActivityToBind.Add(trEntryToInitiallizeGrid);

                    grdActivityExceptions.DataSource = _lsCashActivityToBind;

                    HelperClass.SetColumnDisplayNames(grdActivityExceptions, null);
                    _lsCashActivityToBind.Clear();

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
                if (_lsCashActivityToBind != null)
                    _lsCashActivityToBind.Clear();
                if (CashDataManager.GetInstance().lsActivityException != null)
                    CashDataManager.GetInstance().lsActivityException.Clear();
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
            try
            {
                if (workCompleted)
                {
                    btnGetEx.Text = "Get Exceptions";
                    btnSave.Text = "Save";
                    ugbxActivityExcepParams.Enabled = true;
                    btnOverriding.Text = "Get Overriding Data";
                    toolStripStatusLabel1.Text = string.Empty;
                }
                else
                {
                    if (isGetExceptions)
                    {
                        btnGetEx.Text = "Getting...";
                        toolStripStatusLabel1.Text = "Getting Exceptions...";
                        IsGetException = true;
                        IsOverride = false;
                        if (!CustomThemeHelper.ApplyTheme)
                        {
                            toolStripStatusLabel1.ForeColor = System.Drawing.Color.Green;
                        }
                    }
                    else if (isGetOverridingData)
                    {
                        btnOverriding.Text = "Getting...";
                        toolStripStatusLabel1.Text = "Getting Override Data...";
                        IsGetException = false;
                        IsOverride = true;
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
                        if (!CustomThemeHelper.ApplyTheme)
                        {
                            toolStripStatusLabel1.ForeColor = System.Drawing.Color.Blue;

                        }
                    }
                    ugbxActivityExcepParams.Enabled = false;
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

        #region Getting Exceptions Section

        private void btnGetEx_Click(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Compare(Convert.ToDateTime(dtToDate.DateTime), Convert.ToDateTime(dtFromDate.DateTime)) >= 0)
                {
                    string accountIds = ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',');
                    if (!CashDataManager.GetInstance().GetIsRevaluationInProgress(accountIds))
                    {
                        _activitySource = "Activities generated using 'Get Exceptions' button";
                        ClearData();
                        GetExceptionsAsync();
                    }
                    else
                        MessageBox.Show("Revaluation is in progress for selected fund(s). Proceed after revaluation is done.", "Revaluation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("To Date is before From Date", "Activity Exceptions", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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
        void bgwrkrGetData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] arguments = e.Argument as object[];
                CashDataManager.GetInstance().GetActivityExceptions((DateTime)arguments[0], (DateTime)arguments[1], (string)arguments[2]);
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
                HelperClass.GlobalGridSetting(grdActivityExceptions.DisplayLayout.Bands[0]);
                _lsCashActivityToBind.AddList(DeepCopyHelper.Clone(CashDataManager.GetInstance().lsActivityException));
                NameValueFiller.SetNameValues(_lsCashActivityToBind);
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
                        _activitySource = "Activities generated using 'Get Overriding Data' button";
                        ClearData();
                        BackgroundWorker bgWorkerGetOverridingDataAsyn = new BackgroundWorker();
                        bgWorkerGetOverridingDataAsyn.DoWork += new DoWorkEventHandler(bgWorkerGetOverridingDataAsyn_DoWork);
                        bgWorkerGetOverridingDataAsyn.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwrkrGetData_RunWorkerCompleted);
                        ChangeStatus(false, false, true);
                        bgWorkerGetOverridingDataAsyn.RunWorkerAsync(new object[] { dtFromDate.DateTime.Date, dtToDate.DateTime.Date, ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',') });
                    }
                    else
                        MessageBox.Show("Revaluation is in progress for selected fund(s). Proceed after revaluation is done.", "Revaluation", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                    MessageBox.Show("To Date is before From Date", "Overriding Activity", MessageBoxButtons.OK, MessageBoxIcon.Warning);


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

        void bgWorkerGetOverridingDataAsyn_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] parameters = e.Argument as object[];
                CashDataManager.GetInstance().GetOverridingActivity((DateTime)parameters[0], (DateTime)parameters[1], (string)parameters[2]);
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

        #region Persistance Section
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_lsCashActivityToBind != null && _lsCashActivityToBind.Count > 0)
                {
                    string accountIds = ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',');
                    if (!CashDataManager.GetInstance().GetIsRevaluationInProgress(accountIds))
                    {
                        DialogResult dlgResult = new DialogResult();
                        if (_lsCashActivityToBind != null && _lsCashActivityToBind.Count > 0)
                        {
                            dlgResult = ConfirmationMessageBox.DisplayYesNo("Revaluation date may be set to back date. Do you want to continue?", "Activity Exception");
                        }

                        if (dlgResult == DialogResult.Yes)
                        {
                            string userName = CachedDataManager.GetInstance.LoggedInUser.LoginID;
                            if (IsGetException)
                                Logger.LoggerWrite("Get Exception(Activity Exception)... FormDate: " + Convert.ToString(dtFromDate.DateTime) + " Todate: " + Convert.ToString(dtToDate.DateTime) + " Account Id: " + accountIds + " User Name: " + userName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                            if (IsOverride)
                                Logger.LoggerWrite("Get Override(Activity Exception)... FormDate: " + Convert.ToString(dtFromDate.DateTime) + " Todate: " + Convert.ToString(dtToDate.DateTime) + " Account Id: " + accountIds + " User Name: " + userName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

                            SaveDataAsync();
                        }
                        else
                        {
                            ClearData();
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
                if (_lsCashActivityToBind != null && _lsCashActivityToBind.Count > 0)
                    CashDataManager.GetInstance().Save(CashDataManager.GetInstance().lsActivityException, _activitySource);
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


        private void grdCashExceptions_AfterSortChange(object sender, Infragistics.Win.UltraWinGrid.BandEventArgs e)
        {
            try
            {

                if (grdActivityExceptions.Rows.IsGroupByRows)
                {
                    grdActivityExceptions.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.InGroupByRows;
                }
                else
                {
                    grdActivityExceptions.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
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

        void grdActivityExceptions_BeforeRowFilterDropDown(object sender, Infragistics.Win.UltraWinGrid.BeforeRowFilterDropDownEventArgs e)
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

        void grdActivityExceptions_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(CashManagementConstants.COLUMN_TRADEDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_MODIFYDATE) || e.Column.Key.Equals(CashManagementConstants.COLUMN_ENTRYDATE)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    grdActivityExceptions.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    grdActivityExceptions.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
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

        private void grdCashExceptions_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            HelperClass.ActivitySummarySettings(e);
            grdActivityExceptions.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
            if (grdActivityExceptions.DisplayLayout.Bands[0].Columns.Exists(OrderFields.PROPERTY_SETTLCURRENCYID))
            {
                grdActivityExceptions.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SETTLCURRENCYID].Hidden = true;
                grdActivityExceptions.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SETTLCURRENCYID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }
        }

        private void grdCashExceptions_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            HelperClass.GroupByRowSetting(e);
        }


        private void generateJournalExceptions_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Compare(Convert.ToDateTime(dtToDate.DateTime), Convert.ToDateTime(dtFromDate.DateTime)) >= 0)
                {
                    string accountIds = ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',');
                    if (!CashDataManager.GetInstance().GetIsRevaluationInProgress(accountIds))
                    {
                        _activitySource = "Activities generated using 'Generate Journal Exceptions' button";
                        BackgroundWorker bgSaveActivity = new BackgroundWorker();
                        bgSaveActivity.DoWork += bgSaveActivity_DoWork;
                        bgSaveActivity.RunWorkerCompleted += bgSaveActivity_RunWorkerCompleted;
                        generateJournalExceptions.Text = "Saving Exceptions";
                        toolStripStatusLabel1.Text = "Saving Direct Generated JournalExceptions ..";
                        generateJournalExceptions.Enabled = false;
                        bgSaveActivity.RunWorkerAsync(new object[] { dtFromDate.DateTime, dtToDate.DateTime, ctrlMasterFundAndAccountsDropdown1.MultiAccounts.GetCommaSeperatedAccountIds().TrimEnd(',') });
                    }
                    else
                        MessageBox.Show("Revaluation is in progress for selected fund(s). Proceed after revaluation is done.", "Revaluation", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                    MessageBox.Show("To Date is before From Date", "Activity Exceptions from Journal ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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

        long no = 0;
        Timer time;

        private void bgSaveActivity_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                generateJournalExceptions.Enabled = true;
                generateJournalExceptions.Text = "Generate Journal Exception";
                // if (no < 0)
                //   toolStripStatusLabel1.Text = "Error"; //MessageBox.Show("Error");
                if (no > 0)
                    toolStripStatusLabel1.Text = no + " Activities Created .";  // MessageBox.Show(no + " Activities Created .");
                else
                    toolStripStatusLabel1.Text = "No Exceptions Found.";
                time = new Timer();
                time.Tick += time_Tick;
                time.Interval = 5000;
                time.Enabled = true;
                InitializeGridLayout();

            }
            catch (Exception ex)
            {
                generateJournalExceptions.Enabled = true;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }

        }

        private void time_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = String.Empty;
            time.Enabled = false;
            time.Stop();
        }

        private void bgSaveActivity_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] param = e.Argument as object[];
                no = CashDataManager.GetInstance().SaveJournalException((DateTime)param[0], (DateTime)param[1], (string)param[2]);
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

        private void ctrlActivityExceptions_Load(object sender, EventArgs e)
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
                generateJournalExceptions.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                generateJournalExceptions.ForeColor = System.Drawing.Color.White;
                generateJournalExceptions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                generateJournalExceptions.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                generateJournalExceptions.UseAppStyling = false;
                generateJournalExceptions.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

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

        private void grdActivityExceptions_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {

                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdActivityExceptions);
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
                CashManagementLayout cashManagementLayout = saveGridLayout.GetLayout(grdActivityExceptions, "ActivityExceptions");
                CashPreferenceManager.GetInstance().SetCashGridLayout(cashManagementLayout, "ActivityExceptions");
                ActivityExceptionsLayout = cashManagementLayout;
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
                if (ExportToExcelHelper.ExportToExcel(grdActivityExceptions))
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

        private CashManagementLayout _activityExceptionsLayout = null;
        public CashManagementLayout ActivityExceptionsLayout
        {
            get { return _activityExceptionsLayout; }
            set { _activityExceptionsLayout = value; }
        }

        public void InitializeGridLayout()
        {
            try
            {
                if (ActivityExceptionsLayout != null)
                {
                    if (ActivityExceptionsLayout.GroupByColumnsCollection.Count > 0)
                    {
                        //Set GroupBy Columns
                        bool flag = true;
                        int GroupedColumns = grdActivityExceptions.DisplayLayout.Bands[0].SortedColumns.Count;
                        for (int i = 0; i < GroupedColumns - 1; i++)
                        {
                            grdActivityExceptions.DisplayLayout.Bands[0].SortedColumns.Remove(0);
                        }

                        foreach (string item in ActivityExceptionsLayout.GroupByColumnsCollection)
                        {
                            if (grdActivityExceptions.DisplayLayout.Bands[0].Columns.Exists(item) && !grdActivityExceptions.DisplayLayout.Bands[0].SortedColumns.Contains(item))
                            {
                                grdActivityExceptions.DisplayLayout.Bands[0].SortedColumns.Add(item, true, true);
                                if (grdActivityExceptions.DisplayLayout.Bands[0].SortedColumns.Count == 1)
                                {
                                    flag = false;
                                }
                                if (flag)
                                {
                                    grdActivityExceptions.DisplayLayout.Bands[0].SortedColumns.Remove(0);
                                    flag = false;
                                }
                            }
                        }

                        grdActivityExceptions.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                        grdActivityExceptions.DisplayLayout.RefreshSummaries();
                    }

                    ColumnFiltersCollection columnFilters = grdActivityExceptions.DisplayLayout.Bands[0].ColumnFilters;
                    ///TODO : When we apply the custom filters we need to change the code so the filters won't be on a common field
                    columnFilters.ClearAllFilters();

                    foreach (UltraGridColumn col in grdActivityExceptions.DisplayLayout.Bands[0].Columns)
                    {
                        col.Hidden = true;
                    }

                    foreach (CashGridColumn selectedCols in ActivityExceptionsLayout.SelectedColumnsCollection)
                    {
                        UltraGridColumn col = null;
                        if (grdActivityExceptions.DisplayLayout.Bands[0].Columns.Exists(selectedCols.Name))
                        {
                            col = grdActivityExceptions.DisplayLayout.Bands[0].Columns[selectedCols.Name];
                            if (col != null)
                            {
                                col.HiddenWhenGroupBy = DefaultableBoolean.False;
                                col.Hidden = selectedCols.Hidden;
                                col.Header.Fixed = selectedCols.IsHeaderFixed;


                                if (!grdActivityExceptions.DisplayLayout.Bands[0].SortedColumns.Contains(col))
                                {
                                    col.SortIndicator = selectedCols.SortIndicator;
                                    if (col.SortIndicator.Equals(SortIndicator.Ascending))

                                        grdActivityExceptions.DisplayLayout.Bands[0].SortedColumns.Add(col, false);
                                    if (col.SortIndicator.Equals(SortIndicator.Descending))
                                        grdActivityExceptions.DisplayLayout.Bands[0].SortedColumns.Add(col, true);

                                }

                                grdActivityExceptions.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
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
                                            grdActivityExceptions.DisplayLayout.Bands[0].ColumnFilters[col].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.ToString(DateTimeConstants.DateformatForClosing));
                                        }
                                        else
                                        {
                                            grdActivityExceptions.DisplayLayout.Bands[0].ColumnFilters[col].FilterConditions.Add(filCond);
                                        }
                                    }
                                    grdActivityExceptions.DisplayLayout.Bands[0].ColumnFilters[col].LogicalOperator = selectedCols.FilterLogicalOperator;
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

        private void grdActivityExceptions_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }
    }
}
