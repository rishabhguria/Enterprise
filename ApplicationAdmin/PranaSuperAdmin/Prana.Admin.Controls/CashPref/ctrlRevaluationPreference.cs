using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    public partial class ctrlRevaluationPreference : UserControl
    {
        public ctrlRevaluationPreference()
        {
            InitializeComponent();
        }

        private ValueList _vlOperationMode = new ValueList();
        private DataTable _dtSource = new DataTable();
        private ValueList _vlOperationModeDropDown = new ValueList();

        /// <summary>
        /// load event of the ctrlRevaluationPreference user control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CtrlRevaluationPreference_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (CustomThemeHelper.ApplyTheme)
                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);


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
        /// Initialize the data of grid and valuelist
        /// </summary>
        public void InitializeData()
        {
            try
            {
                CreateValueListData();
                grdRevaluationPref.DataSource = GetDataSourceforGrid();
                string dailyProcessDays = CommonDataCache.CachedDataManager.GetInstance.GetPranaPreferenceByKey(ApplicationConstants.CONST_REVALUATION_DAILY_PROCESS_DAYS);
                dailyProcessDaysNumericEditor.Value = int.Parse(dailyProcessDays);

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
        /// InitializeLayout event of the grdRevaluationPref
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdRevaluationPref_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                if (!e.Layout.Bands[0].Columns.Exists(RevaluationPrefConstants.CONST_SELECT))
                    e.Layout.Bands[0].Columns.Add(RevaluationPrefConstants.CONST_SELECT);

                if (e.Layout.Bands[0].Columns.Exists(RevaluationPrefConstants.CONST_SELECT))
                {
                    UltraGridColumn colSelect = e.Layout.Bands[0].Columns[RevaluationPrefConstants.CONST_SELECT];
                    colSelect.Key = RevaluationPrefConstants.CONST_SELECT;
                    colSelect.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    colSelect.Header.Caption = "";
                    colSelect.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                    colSelect.Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                    colSelect.Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center;
                    colSelect.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colSelect.CellActivation = Activation.AllowEdit;
                    colSelect.DataType = typeof(bool);
                    colSelect.Header.VisiblePosition = 0;
                    colSelect.Width = 30;
                }

                if (e.Layout.Bands[0].Columns.Exists(RevaluationPrefConstants.CONST_ACCOUNT_NAME))
                {
                    UltraGridColumn colAccountName = e.Layout.Bands[0].Columns[RevaluationPrefConstants.CONST_ACCOUNT_NAME];
                    colAccountName.Header.Caption = RevaluationPrefConstants.CONST_ACCOUNT_NAME_CAPTION;
                    colAccountName.CellActivation = Activation.NoEdit;
                    colAccountName.Width = 270;
                }

                if (e.Layout.Bands[0].Columns.Exists(RevaluationPrefConstants.CONST_OPERATION_MODE))
                {
                    UltraGridColumn colOperationMode = e.Layout.Bands[0].Columns[RevaluationPrefConstants.CONST_OPERATION_MODE];
                    colOperationMode.Header.Caption = RevaluationPrefConstants.CONST_OPERATION_MODE_CAPTION;
                    colOperationMode.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colOperationMode.Width = 195;
                }

                e.Layout.Override.FixedRowStyle = FixedRowStyle.Top;
                if (e.Layout.Rows.Count > 0)
                    e.Layout.Rows.FixedRows.Add(e.Layout.Rows[0]);
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
        /// InitializeRow event of grdRevaluationPref
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdRevaluationPref_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.Index.Equals(0))
                {
                    if (e.Row.Band.Columns.Exists(RevaluationPrefConstants.CONST_SELECT))
                    {
                        e.Row.Cells[RevaluationPrefConstants.CONST_SELECT].Hidden = true;
                    }
                    e.Row.RowSpacingAfter = 10;
                    e.Row.Cells[RevaluationPrefConstants.CONST_OPERATION_MODE].ValueList = _vlOperationMode;
                }
                else if (e.Row.Index > 0)
                {
                    e.Row.Cells[RevaluationPrefConstants.CONST_OPERATION_MODE].ValueList = _vlOperationModeDropDown;
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
        /// CellChange event of the grdRevaluationPref
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdRevaluationPref_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                grdRevaluationPref.CellChange -= grdRevaluationPref_CellChange;
                if (grdRevaluationPref.ActiveRow.Index.Equals(0) && e.Cell.Column.Key.Equals(RevaluationPrefConstants.CONST_OPERATION_MODE))
                {
                    grdRevaluationPref.UpdateData();
                    int selectedOperationMode = (int)grdRevaluationPref.ActiveRow.Cells[RevaluationPrefConstants.CONST_OPERATION_MODE].Value;
                    if (selectedOperationMode != int.MinValue)
                        for (int rowIndex = 1; rowIndex < grdRevaluationPref.Rows.Count; rowIndex++)
                        {
                            if (grdRevaluationPref.Rows[rowIndex].Cells.Exists(RevaluationPrefConstants.CONST_SELECT)
                                && grdRevaluationPref.Rows[rowIndex].Cells.Exists(RevaluationPrefConstants.CONST_OPERATION_MODE))
                            {
                                if (bool.Parse(grdRevaluationPref.Rows[rowIndex].Cells[RevaluationPrefConstants.CONST_SELECT].Value.ToString()))
                                    grdRevaluationPref.Rows[rowIndex].Cells[RevaluationPrefConstants.CONST_OPERATION_MODE].Value = selectedOperationMode;
                            }
                        }
                }
                else if (grdRevaluationPref.ActiveRow.Index >= 0 && e.Cell.Column.Key.Equals(RevaluationPrefConstants.CONST_OPERATION_MODE))
                {
                    int selectedOperationMode = (int)grdRevaluationPref.ActiveRow.Cells[RevaluationPrefConstants.CONST_OPERATION_MODE].Value;
                    grdRevaluationPref.UpdateData();
                    int currentOperationMode = (int)grdRevaluationPref.ActiveRow.Cells[RevaluationPrefConstants.CONST_OPERATION_MODE].Value;

                    if (currentOperationMode == int.MinValue)
                    {
                        grdRevaluationPref.ActiveRow.Cells[RevaluationPrefConstants.CONST_OPERATION_MODE].Value = selectedOperationMode;
                    }

                }
                if (e.Cell.Column.Key.Equals(RevaluationPrefConstants.CONST_SELECT))
                {
                    grdRevaluationPref.Rows[0].Cells[RevaluationPrefConstants.CONST_OPERATION_MODE].Value = int.MinValue;
                }

                grdRevaluationPref.CellChange += grdRevaluationPref_CellChange;
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
        /// get data table for the grdRevaluationPref
        /// </summary>
        /// <returns></returns>
        private DataTable GetDataSourceforGrid()
        {
            _dtSource = new DataTable();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetRevaluationPreference";
                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                _dtSource.Columns.Add(RevaluationPrefConstants.CONST_ACCOUNT_NAME, typeof(string));
                _dtSource.Columns.Add(RevaluationPrefConstants.CONST_OPERATION_MODE, typeof(int));

                _dtSource.Rows.Add(string.Empty, int.MinValue);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int fundId = int.Parse(row[RevaluationPrefConstants.CONST_FUND_ID].ToString());
                    int operationMode = int.Parse(row[RevaluationPrefConstants.CONST_OPERATION_MODE].ToString());
                    if (CachedDataManager.GetInstance.GetAccounts().ContainsKey(fundId))
                    {
                        _dtSource.Rows.Add(CachedDataManager.GetInstance.GetAccounts()[fundId], operationMode);
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
            return _dtSource;
        }

        /// <summary>
        /// Creates valuelist for the drop down column of the grid
        /// </summary>
        private void CreateValueListData()
        {
            try
            {
                _vlOperationMode.ValueListItems.Clear();
                _vlOperationModeDropDown.ValueListItems.Clear();

                _vlOperationMode.ValueListItems.Add(int.MinValue, "-Select-");
                _vlOperationMode.ValueListItems.Add(1, RevaluationPrefConstants.CONST_DAILY_PROCESS);
                _vlOperationMode.ValueListItems.Add(2, RevaluationPrefConstants.CONST_IMPLEMENTATION);

                _vlOperationModeDropDown.ValueListItems.Add(1, RevaluationPrefConstants.CONST_DAILY_PROCESS);
                _vlOperationModeDropDown.ValueListItems.Add(2, RevaluationPrefConstants.CONST_IMPLEMENTATION);

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
        /// Create the XML of the records for Saving into DB
        /// </summary>
        /// <returns>XML document of Preferences</returns>
        private string CreateXmlPreference()
        {
            string strXmlPreference = string.Empty;
            try
            {
                DataSet dsRevaluationPref = new DataSet("dsRevaluationPref");
                DataTable dtRevaluationPref = new DataTable("dtRevaluationPref");
                dtRevaluationPref.Columns.Add(RevaluationPrefConstants.CONST_FUND_ID, typeof(int));
                dtRevaluationPref.Columns.Add(RevaluationPrefConstants.CONST_OPERATION_MODE, typeof(int));

                grdRevaluationPref.UpdateData();
                DataTable dtGrdSource = (DataTable)grdRevaluationPref.DataSource;

                for (int index = 1; index < dtGrdSource.Rows.Count; index++)
                {
                    DataRow row = dtGrdSource.Rows[index];
                    string accountName = row[RevaluationPrefConstants.CONST_ACCOUNT_NAME].ToString(); int accountID = CachedDataManager.GetInstance.GetAccounts().FirstOrDefault(x => x.Value == accountName).Key;
                    int operationMode = int.Parse(row[RevaluationPrefConstants.CONST_OPERATION_MODE].ToString());

                    dtRevaluationPref.Rows.Add(accountID, operationMode);
                }
                dsRevaluationPref.Tables.Add(dtRevaluationPref);

                strXmlPreference = dsRevaluationPref.GetXml();
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
            return strXmlPreference;
        }

        /// <summary>
        /// Save the grid and numeric editor value in database
        /// </summary>
        public void SaveRevaluationPreference()
        {
            try
            {
                int dailyProcessDays = (int)dailyProcessDaysNumericEditor.Value;
                CommonDataCache.CachedDataManager.GetInstance.UpdateandSavePranaPreferenceRevaluationPref(dailyProcessDays);

                object[] paramRevaluationPref = { CreateXmlPreference(), 0 };
                SaveRevaluationPreferenceToDB(paramRevaluationPref);
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
        /// Save the xml string of Dataset into the DB
        /// </summary>
        /// <param name="paramRevaluationPref">xml string of the Dataset</param>
        public void SaveRevaluationPreferenceToDB(object[] paramRevaluationPref)
        {
            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveRevaluationPreference", paramRevaluationPref);
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
        /// Before sort change event of grdRevaluationPref
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdRevaluationPref_BeforeSortChange(object sender, BeforeSortChangeEventArgs e)
        {
            try
            {
                if (e.SortedColumns.Count > 0 && e.SortedColumns[0].Key.Equals(RevaluationPrefConstants.CONST_SELECT))
                    e.Cancel = true;
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
