using Infragistics.Documents.Excel;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Prana.ClientCommon
{
    public partial class AllocationSchemeForm : Form
    {
        public AllocationSchemeForm()
        {
            InitializeComponent();
            _currencyListForAlloScheme = AllocationSchemeDataManager.GetInstance.GetCurrencyLIstForAllocationScheme();// _proxyAllocationServices.InnerChannel.GetCurrencyListForAllocationScheme();
            BindAllocationSchemesCombo();
        }

        List<string> _currencyListForAlloScheme = null;

        #region constants
        const string CONSTSTR_TableName = "PositionMaster";
        const string PROPERTY_Validated = "Validated";
        #endregion 

        #region global variables

        private DataSet _dsAllocationScheme = null;
        private DataTable _dsSchemeNames = null;
        private bool _isInitialized = true;
        private bool _isModified = false;
        private string _allocationSchemeName = string.Empty;
        private bool _isAborted = false;

        #endregion



        private static AllocationSchemeForm _allocationSchemeForm = null;
        public static AllocationSchemeForm GetInstance()
        {

            if (_allocationSchemeForm == null)
            {
                _allocationSchemeForm = new AllocationSchemeForm();
            }
            return _allocationSchemeForm;
        }

        public void RefreshGridGroup(object sender, EventArgs<string> e)
        {
            string symbol = e.Value;
            grdScheme.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
            lblStatus.Text = "Last Updated Symbol : " + symbol;
        }

        private void AllocationSchemeForm_Load(object sender, EventArgs e)
        {
            try
            {
                SetupSnapshotControl();
                grdScheme.DisplayLayout.Override.AllowGroupBy = DefaultableBoolean.True;
                grdScheme.DisplayLayout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Expanded;

                HideAndShowControls();

                _isInitialized = false;
                SetButtonsColor();
                CustomThemeHelper.SetThemeProperties(this, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CLOSE_TRADE_GRID);
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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

        private void SetButtonsColor()
        {
            try
            {
                btnScreenshot.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnScreenshot.ForeColor = System.Drawing.Color.White;
                btnScreenshot.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnScreenshot.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnScreenshot.UseAppStyling = false;
                btnScreenshot.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnClose.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnClose.ForeColor = System.Drawing.Color.White;
                btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnClose.UseAppStyling = false;
                btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGetScheme.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetScheme.ForeColor = System.Drawing.Color.White;
                btnGetScheme.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetScheme.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetScheme.UseAppStyling = false;
                btnGetScheme.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void SetupSnapshotControl()
        {
            try
            {
                this.btnScreenshot = SnapShotManager.GetInstance().ultraButton;
                this.btnScreenshot.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
                this.btnScreenshot.Location = new System.Drawing.Point(550, 433);
                this.btnScreenshot.Name = "btnScreenshot";
                this.btnScreenshot.Size = new System.Drawing.Size(75, 23);
                this.btnScreenshot.TabIndex = 6;
                this.btnScreenshot.Click += new System.EventHandler(this.btnScreenshot_Click);
                this.Controls.Add(this.btnScreenshot);
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

        private void HideAndShowControls()
        {
            try
            {
                switch (_isAllocationSchemeImport)
                {
                    case AllocationScheme.Import:
                        UltraGridBand band = this.grdScheme.DisplayLayout.Bands[0];
                        if (!band.Columns.Exists("checkBox"))
                        {
                            band.Columns.Add("checkBox");
                        }
                        UltraGridColumn colcheckBox = band.Columns["checkBox"];
                        colcheckBox.Header.VisiblePosition = 0;
                        colcheckBox.Hidden = false;
                        btnClose.Text = "Abort";
                        btnSave.Text = "Import";
                        lblSchemeName.Hide();
                        cmbAllocationScheme.Hide();
                        btnGetScheme.Hide();
                        HideReconControls();
                        break;

                    case AllocationScheme.Edit:
                        lblSchemeDate.Text = "Date";
                        btnSave.Text = "Save";
                        btnGetScheme.Text = "Get Scheme";
                        dtSchemeDate.Location = new Point(64, 7);
                        grdScheme.DisplayLayout.GroupByBox.Hidden = true;
                        btnGetScheme.Enabled = false;
                        HideReconControls();
                        break;


                    case AllocationScheme.Recon:
                        lblSchemeDate.Text = "From";
                        btnGetScheme.Text = "Generate Report";
                        btnSave.Text = "Export";
                        //dtSchemeDate.Location = new Point(109, 8);
                        grdScheme.DisplayLayout.GroupByBox.Hidden = true;
                        btnGetScheme.Enabled = false;
                        ShowReconControls();
                        break;

                    default:
                        break;
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

        private void HideReconControls()
        {
            grpMatched.Hide();
            grpMisMatched.Hide();
            grpNotTraded.Hide();
            lblMatched.Hide();
            lblMisMatched.Hide();
            lblNotTraded.Hide();
            lblToDate.Hide();
            dtToDate.Hide();
        }

        private void ShowReconControls()
        {
            grpMatched.Show();
            grpMisMatched.Show();
            grpNotTraded.Show();
            lblMatched.Show();
            lblMisMatched.Show();
            lblNotTraded.Show();
            lblToDate.Show();
            dtToDate.Show();
        }

        private void grdScheme_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                if (_isAllocationSchemeImport.Equals(AllocationScheme.Import))
                {
                    AddCheckBoxinGrid(grdScheme, headerCheckBox);
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

        private void grdScheme_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {
                switch (_isAllocationSchemeImport)
                {
                    case AllocationScheme.Import:
                        if (e.Row.ListObject != null)
                        {
                            DataRow row = ((System.Data.DataRowView)(e.Row.ListObject)).Row;
                            SetForASchemeError(row);
                        }
                        break;

                    case AllocationScheme.Recon:
                        //According to given Legend.
                        if (!CustomThemeHelper.ApplyTheme)
                        {
                            if (!e.Row.Cells["AllocatedQty"].Text.Equals("0"))
                            {
                                if (e.Row.Cells["Matched"].Value.Equals(true))
                                {
                                    e.Row.Appearance.ForeColor = Color.YellowGreen;
                                }
                                else
                                {
                                    e.Row.Appearance.ForeColor = Color.Tomato;
                                }
                            }
                            else
                            {
                                e.Row.Appearance.ForeColor = Color.White;
                            }
                        }
                        else
                        {
                            if (!e.Row.Cells["AllocatedQty"].Text.Equals("0"))
                            {
                                if (e.Row.Cells["Matched"].Value.Equals(true))
                                {
                                    e.Row.Appearance.ForeColor = System.Drawing.Color.FromArgb(39, 174, 96);
                                }
                                else
                                {
                                    e.Row.Appearance.ForeColor = System.Drawing.Color.FromArgb(192, 57, 43);
                                }
                            }
                        }
                        break;

                    default:
                        break;
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

        private double _totalQty = 0;
        private void grdScheme_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key.Equals("Quantity") && e.Cell.Text != string.Empty)
                {
                    if (double.TryParse(e.Cell.Text, out _totalQty))
                    {
                        double quantity = double.Parse(e.Cell.Text);
                        if (quantity > 0)
                        {
                            _totalQty = double.Parse(e.Cell.Row.Cells["TotalQty"].Value.ToString());
                            _totalQty -= double.Parse(e.Cell.Value.ToString());

                            this.grdScheme.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdScheme_AfterCellUpdate);
                            e.Cell.Value = double.Parse(e.Cell.Text.ToString());
                            this.grdScheme.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdScheme_AfterCellUpdate);

                            string allocSchemeKeyName = e.Cell.Row.Cells["AllocationSchemeKey"].Value.ToString();
                            AllocationSchemeKey AllocSchemeKey = (AllocationSchemeKey)Enum.Parse(typeof(AllocationSchemeKey), allocSchemeKeyName);

                            string symbol = e.Cell.Row.Cells["Symbol"].Value.ToString();
                            string orderSideTagValue = e.Cell.Row.Cells["OrderSideTagValue"].Value.ToString();
                            string PB = e.Cell.Row.Cells["PB"].Value.ToString();
                            string TradeType = e.Cell.Row.Cells["TradeType"].Value.ToString();
                            string currency = e.Cell.Row.Cells["Currency"].Value.ToString();

                            _totalQty += quantity;
                            foreach (UltraGridRow row in grdScheme.Rows.GetFilteredInNonGroupByRows())
                            {
                                switch (AllocSchemeKey)
                                {
                                    case AllocationSchemeKey.Symbol:
                                        if (row.Cells["Symbol"].Value.ToString().Equals(symbol))
                                        {
                                            this.grdScheme.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdScheme_AfterCellUpdate);
                                            row.Cells["TotalQty"].Value = _totalQty;
                                            row.Cells["Percentage"].Value = (double.Parse(row.Cells["Quantity"].Value.ToString()) / _totalQty) * 100;
                                            this.grdScheme.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdScheme_AfterCellUpdate);
                                        }
                                        break;
                                    case AllocationSchemeKey.SymbolSide:
                                        if (row.Cells["Symbol"].Value.ToString().Equals(symbol) && row.Cells["OrderSideTagValue"].Value.ToString().Equals(orderSideTagValue))
                                        {
                                            this.grdScheme.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdScheme_AfterCellUpdate);
                                            row.Cells["TotalQty"].Value = _totalQty;
                                            row.Cells["Percentage"].Value = (double.Parse(row.Cells["Quantity"].Value.ToString()) / _totalQty) * 100;
                                            this.grdScheme.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdScheme_AfterCellUpdate);
                                        }
                                        break;
                                    case AllocationSchemeKey.PBSymbolSide:
                                        if (_currencyListForAlloScheme != null && _currencyListForAlloScheme.Count > 0 && !_currencyListForAlloScheme.Contains(currency) && TradeType.ToLower().Equals("swap"))
                                        {
                                            if (row.Cells["PB"].Value.ToString().Equals(PB) && row.Cells["Symbol"].Value.ToString().Equals(symbol) && row.Cells["OrderSideTagValue"].Value.ToString().Equals(orderSideTagValue))
                                            {
                                                this.grdScheme.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdScheme_AfterCellUpdate);
                                                row.Cells["TotalQty"].Value = _totalQty;
                                                row.Cells["Percentage"].Value = (double.Parse(row.Cells["Quantity"].Value.ToString()) / _totalQty) * 100;
                                                this.grdScheme.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdScheme_AfterCellUpdate);
                                            }
                                        }
                                        else if (row.Cells["Symbol"].Value.ToString().Equals(symbol) && row.Cells["OrderSideTagValue"].Value.ToString().Equals(orderSideTagValue))
                                        {
                                            this.grdScheme.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdScheme_AfterCellUpdate);
                                            row.Cells["TotalQty"].Value = _totalQty;
                                            row.Cells["Percentage"].Value = (double.Parse(row.Cells["Quantity"].Value.ToString()) / _totalQty) * 100;
                                            this.grdScheme.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdScheme_AfterCellUpdate);
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            _isModified = true;
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

        private void grdScheme_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {

        }

        private void SetForASchemeError(DataRow row)
        {
            try
            {
                string allocSchemeKeyName = row["AllocationSchemeKey"].ToString();

                if (!string.IsNullOrEmpty(allocSchemeKeyName))
                {
                    AllocationSchemeKey AllocSchemeKey = (AllocationSchemeKey)Enum.Parse(typeof(AllocationSchemeKey), allocSchemeKeyName);

                    int accountID = int.Parse(row["FundID"].ToString());
                    string symbol = row["Symbol"].ToString();
                    string side = row["Side"].ToString();
                    string PB = row["PB"].ToString();
                    double totalQty = double.Parse(row["TotalQty"].ToString());
                    bool isDuplicate = bool.Parse(row["IsDuplicate"].ToString());
                    string isSymbolValidated = row["IsSymbolValidatedFromSM"].ToString();

                    if (string.IsNullOrEmpty(symbol))
                    {
                        row.RowError = "Invalid Symbol";
                        row[PROPERTY_Validated] = "NotValidated";
                        return;
                    }
                    else if (isSymbolValidated.Equals("NotValidated"))
                    {
                        row.RowError = "Symbol not validated";
                        row[PROPERTY_Validated] = "NotValidated";
                        return;
                    }
                    else if (accountID.Equals(int.MinValue) || accountID.Equals(0))
                    {
                        row.RowError = "Invalid Account";
                        row[PROPERTY_Validated] = "NotValidated";
                        return;
                    }
                    else if (isDuplicate.Equals(true))
                    {
                        row.RowError = "Duplicate Record";
                        row[PROPERTY_Validated] = "NotValidated";
                        return;
                    }
                    else if (totalQty.Equals(double.MinValue) || totalQty.Equals(0))
                    {
                        row.RowError = "Invalid Total Quantity";
                        row[PROPERTY_Validated] = "NotValidated";
                        return;
                    }


                    switch (AllocSchemeKey)
                    {
                        case AllocationSchemeKey.Symbol:
                            row[PROPERTY_Validated] = "Validated";
                            row.RowError = "";
                            break;
                        case AllocationSchemeKey.SymbolSide:
                            if (string.IsNullOrEmpty(side))
                            {
                                row.RowError = "Invalid Side";
                                row[PROPERTY_Validated] = "NotValidated";
                                return;
                            }
                            else
                            {
                                row[PROPERTY_Validated] = "Validated";
                                row.RowError = "";
                            }
                            break;
                        case AllocationSchemeKey.PBSymbolSide:
                            if (string.IsNullOrEmpty(side))
                            {
                                row.RowError = "Invalid Side";
                                row[PROPERTY_Validated] = "NotValidated";
                                return;
                            }
                            else if (string.IsNullOrEmpty(PB))
                            {
                                row.RowError = "Invalid Prime broker";
                                row[PROPERTY_Validated] = "NotValidated";
                                return;
                            }
                            else
                            {
                                row[PROPERTY_Validated] = "Validated";
                                row.RowError = "";
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetCheckBoxAtFirstPosition(UltraGrid grid)
        {
            grid.DisplayLayout.Bands[0].Columns["checkBox"].Hidden = false;
            grid.DisplayLayout.Bands[0].Columns["checkBox"].Header.VisiblePosition = 0;
            grid.DisplayLayout.Bands[0].Columns["checkBox"].Width = 10;
            grid.DisplayLayout.Bands[0].Columns["checkBox"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

        }

        CheckBoxOnHeader_CreationFilter headerCheckBox = new CheckBoxOnHeader_CreationFilter();
        private void AddCheckBoxinGrid(UltraGrid grid, CheckBoxOnHeader_CreationFilter headerCheckBox)
        {
            grid.CreationFilter = headerCheckBox;
            grid.DisplayLayout.Bands[0].Columns.Add("checkBox", "");
            grid.DisplayLayout.Bands[0].Columns["checkBox"].DataType = typeof(bool);
            grid.DisplayLayout.Bands[0].Columns["checkBox"].CellClickAction = CellClickAction.EditAndSelectText;
            SetCheckBoxAtFirstPosition(grid);
        }

        public void BindImportAllocationScheme(DataSet dsAllocationScheme, AllocationScheme isImport, string strDate)
        {
            try
            {
                this.Text = "Allocation Scheme";
                grdScheme.DataSource = null;
                _isAllocationSchemeImport = isImport;
                grdScheme.DataSource = dsAllocationScheme;
                if (_isAllocationSchemeImport.Equals(AllocationScheme.Import))
                {
                    BindGrid(grdScheme);
                    if (string.IsNullOrEmpty(strDate))
                    {
                        dtSchemeDate.Value = DateTime.Now;
                    }
                    else
                    {
                        dtSchemeDate.Value = Convert.ToDateTime(strDate);
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

        public void BindEditAllocationScheme(AllocationScheme isImport, string allocationSchemeName)
        {
            try
            {
                string allocationSchemeXML = string.Empty;
                DateTime allocationSchemeDate = DateTime.MinValue;
                _isAllocationSchemeImport = isImport;
                lblStatus.Text = string.Empty;
                grdScheme.DataSource = null;

                AllocationFixedPreference fp = AllocationSchemeDataManager.GetInstance.GetAllocationSchemeByName(allocationSchemeName);// _proxyAllocationServices.InnerChannel.GetAllocationSchemeByName(allocationSchemeName);
                if (fp != null)
                {
                    allocationSchemeXML = fp.Scheme;

                    allocationSchemeDate = fp.Date;


                    DataSet dsAllocationScheme = new DataSet();

                    StringReader sr = new StringReader(allocationSchemeXML);
                    dsAllocationScheme.ReadXml(sr);

                    //grdScheme.DataSource = null;

                    grdScheme.DataSource = dsAllocationScheme;
                    dtSchemeDate.Value = allocationSchemeDate;
                    dtSchemeDate.Enabled = false;
                    if (_isAllocationSchemeImport.Equals(AllocationScheme.Edit))
                    {
                        BindGrid(grdScheme);
                        cmbAllocationScheme.Text = allocationSchemeName;
                    }
                    //if (groupsAllocated > 0)
                    //{
                    //    lblStatus.Text = "Groups have been allocated using this scheme!";
                    //    lblStatus.ForeColor = Color.Red;
                    //}
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

        public void BindReconAllocationScheme(AllocationScheme isImport, string schemeName)
        {
            try
            {
                _isAllocationSchemeImport = isImport;
                DataSet ds = null;
                grdScheme.DataSource = null;
                lblStatus.Text = string.Empty;

                if (!schemeName.Equals(ApplicationConstants.C_COMBO_SELECT))
                {
                    ds = AllocationSchemeDataManager.GetInstance.GetAllocationSchemeReconReport(schemeName, dtSchemeDate.Value, dtToDate.Value);// _proxyAllocationServices.InnerChannel.GetAllocationSchemeReconReport(schemeName, dtSchemeDate.Value, dtToDate.Value);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        //grdScheme.DataSource = null;
                        grdScheme.DataSource = ds;

                        BindGrid(grdScheme);
                    }
                    else
                    {
                        lblStatus.Text = "Please select valid allocation date";
                    }
                }
                cmbAllocationScheme.Text = schemeName;
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

        private void BindAllocationSchemesCombo()
        {
            try
            {
                Dictionary<int, string> schemes = AllocationSchemeDataManager.GetInstance.GetAllASchemeNames();
                _dsSchemeNames = new DataTable();

                _dsSchemeNames.Columns.Add("AllocationSchemeID", typeof(int));
                _dsSchemeNames.Columns.Add("AllocationSchemeName", typeof(string));
                foreach (KeyValuePair<int, string> kvp in schemes)
                {
                    DataRow row = _dsSchemeNames.NewRow();
                    row["AllocationSchemeID"] = kvp.Key;
                    row["AllocationSchemeName"] = kvp.Value;
                }
                // _proxyAllocationServices.InnerChannel.GetAllAllocationSchemeNames();
                DataRow dr = _dsSchemeNames.NewRow();
                dr["AllocationSchemeID"] = int.MinValue;
                dr["AllocationSchemeName"] = ApplicationConstants.C_COMBO_SELECT;
                _dsSchemeNames.Rows.InsertAt(dr, 0);

                cmbAllocationScheme.DataSource = null;

                cmbAllocationScheme.DataSource = _dsSchemeNames;
                cmbAllocationScheme.DataBind();
                cmbAllocationScheme.DisplayMember = "AllocationSchemeName";
                cmbAllocationScheme.ValueMember = "AllocationSchemeID";

                foreach (UltraGridColumn column in cmbAllocationScheme.DisplayLayout.Bands[0].Columns)
                {
                    column.Hidden = true;
                    if (column.Key.Equals("AllocationSchemeName"))
                    {
                        column.Hidden = false;
                        column.Width = cmbAllocationScheme.Width;
                    }
                }
                cmbAllocationScheme.DisplayLayout.Bands[0].ColHeadersVisible = false;
                cmbAllocationScheme.Value = int.MinValue;
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

        private void BindGrid(UltraGrid grid)
        {
            try
            {
                UltraGridBand band = grid.DisplayLayout.Bands[0];

                UltraGridColumn colAccountNameAllocationScheme = band.Columns["FundName"];
                colAccountNameAllocationScheme.Header.Caption = "Account Name";
                colAccountNameAllocationScheme.Width = 80;
                colAccountNameAllocationScheme.Header.VisiblePosition = 1;

                UltraGridColumn colSymbolAllocationScheme = band.Columns["Symbol"];
                colSymbolAllocationScheme.Header.Caption = "Ticker Symbol";
                colSymbolAllocationScheme.Width = 100;
                colSymbolAllocationScheme.Header.VisiblePosition = 2;

                UltraGridColumn colLongNameAllocationScheme = band.Columns["LongName"];
                colLongNameAllocationScheme.Header.Caption = "Description";
                colLongNameAllocationScheme.Width = 150;
                colLongNameAllocationScheme.Header.VisiblePosition = 3;

                UltraGridColumn colSEDOLAllocationScheme = band.Columns["SEDOL"];
                colSEDOLAllocationScheme.Header.Caption = "Sedol Symbol";
                colSEDOLAllocationScheme.Width = 100;
                colSEDOLAllocationScheme.Header.VisiblePosition = 4;

                UltraGridColumn colBloombergAllocationScheme = band.Columns["Bloomberg"];
                colBloombergAllocationScheme.Header.Caption = "Bloomberg Symbol";
                colBloombergAllocationScheme.Width = 100;
                colBloombergAllocationScheme.Header.VisiblePosition = 5;

                UltraGridColumn colSideAllocationScheme = band.Columns["Side"];
                colSideAllocationScheme.Header.Caption = "Side";
                colSideAllocationScheme.Width = 100;
                colSideAllocationScheme.Header.VisiblePosition = 6;

                UltraGridColumn colQuantityAllocationScheme = band.Columns["Quantity"];
                colQuantityAllocationScheme.Header.Caption = "Quantity";
                colQuantityAllocationScheme.Width = 100;
                colQuantityAllocationScheme.Header.VisiblePosition = 7;

                UltraGridColumn colTotalQtyAllocationScheme = band.Columns["TotalQty"];
                colTotalQtyAllocationScheme.Header.Caption = "Total Quantity";
                colTotalQtyAllocationScheme.Width = 100;
                colTotalQtyAllocationScheme.Header.VisiblePosition = 8;

                UltraGridColumn colAllocationPercentAllocationScheme = band.Columns["Percentage"];
                colAllocationPercentAllocationScheme.Header.Caption = "Allocation %";
                colAllocationPercentAllocationScheme.Width = 100;
                colAllocationPercentAllocationScheme.Header.VisiblePosition = 9;

                UltraGridColumn colRoundLotsAllocationScheme = band.Columns["RoundLot"];
                colRoundLotsAllocationScheme.Header.Caption = "RoundLots";
                colRoundLotsAllocationScheme.Width = 100;
                colRoundLotsAllocationScheme.Header.VisiblePosition = 10;

                UltraGridColumn colTradeTypeAllocationScheme = band.Columns["TradeType"];
                colTradeTypeAllocationScheme.Header.Caption = "Trade Type";
                colTradeTypeAllocationScheme.Width = 100;
                colTradeTypeAllocationScheme.Header.VisiblePosition = 11;

                UltraGridColumn colCurrencyAllocationScheme = band.Columns["Currency"];
                colCurrencyAllocationScheme.Header.Caption = "Currency";
                colCurrencyAllocationScheme.Width = 100;
                colCurrencyAllocationScheme.Header.VisiblePosition = 12;

                UltraGridColumn colPBAllocationScheme = band.Columns["PB"];
                colPBAllocationScheme.Header.Caption = "PB";
                colPBAllocationScheme.Width = 100;
                colPBAllocationScheme.Header.VisiblePosition = 13;

                //if (band.Columns.Exists("AllocationSchemeKey"))
                //{
                UltraGridColumn colAllocationSchemeKey = band.Columns["AllocationSchemeKey"];
                colAllocationSchemeKey.Header.VisiblePosition = 13;

                //colAllocationSchemeKey.Hidden = true;
                //}

                if (band.Columns.Exists("IsSymbolValidatedFromSM"))
                {
                    UltraGridColumn colIsSymbolValidated = band.Columns["IsSymbolValidatedFromSM"];
                    colIsSymbolValidated.Hidden = true;
                }

                if (band.Columns.Exists("SMMappingReq"))
                {
                    UltraGridColumn colSMMappingReq = band.Columns["SMMappingReq"];
                    colSMMappingReq.Hidden = true;
                }

                if (_isAllocationSchemeImport.Equals(AllocationScheme.Edit))
                {
                    colQuantityAllocationScheme.CellActivation = Activation.AllowEdit;
                    colQuantityAllocationScheme.CellClickAction = CellClickAction.EditAndSelectText;
                    //colRoundLotsAllocationScheme.Hidden = true;
                }
                else
                {
                    colAllocationPercentAllocationScheme.CellActivation = Activation.NoEdit;
                    colRoundLotsAllocationScheme.CellActivation = Activation.NoEdit;
                    colQuantityAllocationScheme.CellActivation = Activation.NoEdit;
                }



                UltraGridColumn colOrderSideTagValueAllocationScheme = band.Columns["OrderSideTagValue"];
                colOrderSideTagValueAllocationScheme.Hidden = true;

                UltraGridColumn colRICAllocationScheme = band.Columns["RIC"];
                colRICAllocationScheme.Hidden = true;

                UltraGridColumn colISINAllocationScheme = band.Columns["ISIN"];
                colISINAllocationScheme.Hidden = true;

                UltraGridColumn colCUSIPAllocationScheme = band.Columns["CUSIP"];
                colCUSIPAllocationScheme.Hidden = true;

                UltraGridColumn colOSIOptionSymbolAllocationScheme = band.Columns["OSIOptionSymbol"];
                colOSIOptionSymbolAllocationScheme.Hidden = true;

                UltraGridColumn colIDCOOptionSymbolAllocationScheme = band.Columns["IDCOOptionSymbol"];
                colIDCOOptionSymbolAllocationScheme.Hidden = true;

                UltraGridColumn colAccountIDAllocationScheme = band.Columns["FundID"];
                colAccountIDAllocationScheme.Hidden = true;

                if (band.Columns.Exists("IsDuplicate"))
                {
                    UltraGridColumn colIsDuplicateAllocationScheme = band.Columns["IsDuplicate"];
                    colIsDuplicateAllocationScheme.Hidden = true;
                }

                UltraGridColumn colValidateAllocationScheme = null;
                if (band.Columns.Exists(PROPERTY_Validated))
                {
                    colValidateAllocationScheme = band.Columns[PROPERTY_Validated];
                    colValidateAllocationScheme.Hidden = true;
                }
                UltraGridColumn colSchemeBasedOn = band.Columns["AllocationBasedOn"];

                if (_isAllocationSchemeImport.Equals(AllocationScheme.Import))
                {
                    colValidateAllocationScheme.Hidden = false;
                    colValidateAllocationScheme.Header.Caption = "Validation Status";
                    colValidateAllocationScheme.AllowGroupBy = DefaultableBoolean.True;

                    band.SortedColumns.Add(PROPERTY_Validated, false, true);
                    grdScheme.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value] ([count] [count,items,item,items])";
                    grdScheme.DisplayLayout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Expanded;
                }
                else
                {
                    #region AllocationSchemeReconColumns
                    if (_isAllocationSchemeImport.Equals(AllocationScheme.Recon))
                    {
                        band.RowLayoutStyle = RowLayoutStyle.ColumnLayout;

                        // Allow the columns to be sorted by default.
                        band.Layout.Override.HeaderClickAction = HeaderClickAction.SortMulti;

                        // TEMP VARIABLES
                        UltraGridColumn column = null;
                        RowLayoutColumnInfo info = null;

                        column = band.Columns.Add("DesiredAllocation", "Desired Allocation"); // Add an unbound column for Header
                        column.SortIndicator = SortIndicator.Disabled;
                        column.CellActivation = Activation.NoEdit;  // Prevent the user from being able to tab into cells
                        column.Header.Appearance.TextHAlign = HAlign.Center; // Center the text in the column header.
                        column.AllowRowFiltering = DefaultableBoolean.False;
                        info = column.RowLayoutColumnInfo;
                        info.PreferredCellSize = new Size(1, 1); // This makes the cells for this column not visible.
                        info.MinimumCellSize = new Size(1, 1);
                        info.AllowCellSizing = RowLayoutSizing.None;
                        info.SpanX = 28;
                        info.SpanY = 2;
                        info.OriginX = 0;
                        info.OriginY = 0;

                        column = colAccountNameAllocationScheme;
                        info = column.RowLayoutColumnInfo;
                        SetColumnSpan(info);
                        info.OriginX = 0;

                        column = colSymbolAllocationScheme;
                        info = column.RowLayoutColumnInfo;
                        SetColumnSpan(info);
                        info.OriginX = 2;

                        column = colLongNameAllocationScheme;
                        info = column.RowLayoutColumnInfo;
                        SetColumnSpan(info);
                        info.OriginX = 4;

                        column = colSEDOLAllocationScheme;
                        info = column.RowLayoutColumnInfo;
                        SetColumnSpan(info);
                        info.OriginX = 6;

                        column = colBloombergAllocationScheme;
                        info = column.RowLayoutColumnInfo;
                        SetColumnSpan(info);
                        info.OriginX = 8;

                        column = colSideAllocationScheme;
                        info = column.RowLayoutColumnInfo;
                        SetColumnSpan(info);
                        info.OriginX = 10;

                        column = colQuantityAllocationScheme;
                        info = column.RowLayoutColumnInfo;
                        SetColumnSpan(info);
                        info.OriginX = 12;
                        column.Format = ApplicationConstants.FORMAT_QTY;

                        column = colAllocationPercentAllocationScheme;
                        info = column.RowLayoutColumnInfo;
                        SetColumnSpan(info);
                        info.OriginX = 14;

                        column = colTotalQtyAllocationScheme;
                        info = column.RowLayoutColumnInfo;
                        SetColumnSpan(info);
                        info.OriginX = 16;

                        column = colRoundLotsAllocationScheme;
                        info = column.RowLayoutColumnInfo;
                        SetColumnSpan(info);
                        info.OriginX = 18;

                        column = colTradeTypeAllocationScheme;
                        info = column.RowLayoutColumnInfo;
                        SetColumnSpan(info);
                        info.OriginX = 20;

                        column = colCurrencyAllocationScheme;
                        info = column.RowLayoutColumnInfo;
                        SetColumnSpan(info);
                        info.OriginX = 22;

                        column = colPBAllocationScheme;
                        info = column.RowLayoutColumnInfo;
                        SetColumnSpan(info);
                        info.OriginX = 24;

                        column = colAllocationSchemeKey;
                        info = column.RowLayoutColumnInfo;
                        SetColumnSpan(info);
                        info.OriginX = 26;


                        column = band.Columns.Add("ActualAllocation", "Actual Allocation"); // Add an unbound column for Header
                        column.SortIndicator = SortIndicator.Disabled;
                        column.CellActivation = Activation.NoEdit;  // Prevent the user from being able to tab into cells
                        column.Header.Appearance.TextHAlign = HAlign.Center; // Center the text in the column header.
                        column.AllowRowFiltering = DefaultableBoolean.False;
                        info = column.RowLayoutColumnInfo;
                        info.PreferredCellSize = new Size(1, 1); // This makes the cells for this column not visible.
                        info.MinimumCellSize = new Size(1, 1);
                        info.AllowCellSizing = RowLayoutSizing.None;
                        info.SpanX = 6;
                        info.SpanY = 2;
                        info.OriginX = 28;
                        info.OriginY = 0;


                        column = band.Columns["AllocatedQty"];
                        column.Header.Caption = "Allocated Quantity";
                        info = column.RowLayoutColumnInfo;
                        SetColumnSpan(info);
                        info.OriginX = 28;
                        column.Format = ApplicationConstants.FORMAT_QTY;

                        column = band.Columns["AllocatedPercentage"];
                        column.Header.Caption = "Allocated %";
                        info = column.RowLayoutColumnInfo;
                        SetColumnSpan(info);
                        info.OriginX = 30;


                        UltraGridColumn colIsMatched = band.Columns["Matched"];
                        colIsMatched.Hidden = true;

                        column = band.Columns["PercentDifference"];
                        column.Header.Caption = "Mismatch Difference";
                        info = column.RowLayoutColumnInfo;
                        SetColumnSpan(info);
                        column.Format = ApplicationConstants.FORMAT_QTY;

                    }
                    #endregion

                    colRoundLotsAllocationScheme.Hidden = false;
                    grdScheme.DisplayLayout.GroupByBox.Hidden = true;
                    colSchemeBasedOn.Hidden = true;
                }


                //modified by omshiv, Jan 15, 2014,  Add column TargetAllocationPct in scheme for Master fund ratio scheme
                if (band.Columns.Exists("TargetAllocationPct"))
                {
                    UltraGridColumn colAllocationRatioPct = band.Columns["TargetAllocationPct"];
                    colAllocationRatioPct.Header.Caption = "Target Allocation %";
                    colAllocationRatioPct.Width = 130;
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

        private void SetColumnSpan(RowLayoutColumnInfo info)
        {
            info.AllowCellSizing = RowLayoutSizing.Horizontal;
            info.PreferredCellSize = new Size(100, 0);
            info.SpanX = 2;
            info.SpanY = 2;
            info.OriginY = 2;
        }


        AllocationScheme _isAllocationSchemeImport;
        public DataSet ValidatedAllocationScheme(ref string allocationSchemeName, ref DateTime allocationSchemeDate, ref int allocationSchemeID)
        {
            if (_isAllocationSchemeImport.Equals(AllocationScheme.Import))
            {
                allocationSchemeName = _allocationSchemeName;
                allocationSchemeID = int.MinValue;
                allocationSchemeDate = dtSchemeDate.Value;
            }
            return _dsAllocationScheme;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_isAllocationSchemeImport.Equals(AllocationScheme.Recon))
                {
                    lblStatus.Text = string.Empty;
                    _dsAllocationScheme = ((DataSet)grdScheme.DataSource).Clone();

                    foreach (UltraGridRow row in grdScheme.Rows.GetAllNonGroupByRows())
                    {
                        if (_isAllocationSchemeImport.Equals(AllocationScheme.Edit))
                        {
                            DataRowView drView = (DataRowView)row.ListObject;
                            DataRow dr = _dsAllocationScheme.Tables[CONSTSTR_TableName].NewRow();
                            dr.ItemArray = drView.Row.ItemArray;
                            _dsAllocationScheme.Tables[CONSTSTR_TableName].Rows.Add(dr);
                        }
                        else if (row.Cells["checkBox"].Value.ToString().ToLower().Equals("true") && row.Cells[PROPERTY_Validated].Value.ToString().Equals("Validated"))
                        {
                            DataRowView drView = (DataRowView)row.ListObject;
                            DataRow dr = _dsAllocationScheme.Tables[CONSTSTR_TableName].NewRow();
                            dr.ItemArray = drView.Row.ItemArray;
                            _dsAllocationScheme.Tables[CONSTSTR_TableName].Rows.Add(dr);
                        }
                    }
                }

                switch (_isAllocationSchemeImport)
                {
                    case AllocationScheme.Import:
                        if (_dsAllocationScheme != null && _dsAllocationScheme.Tables[0].Rows.Count > 0)
                        {
                            AllocationSchemes schemes = new AllocationSchemes();
                            //schemes.BindListSchemes(_dsSchemeNames, dtSchemeDate.Value);
                            schemes.SuggestNewAllocationName(dtSchemeDate.Value);
                            if (schemes.ShowDialog() == DialogResult.OK)
                            {
                                _allocationSchemeName = schemes._schemeName;
                            }
                            if (_allocationSchemeName.Equals(string.Empty))
                            {
                                return;
                            }
                            this.DialogResult = DialogResult.OK;
                            _isAborted = true;
                            this.Close();
                        }
                        else
                        {
                            lblStatus.Text = "Please select atleast one valid record";
                        }
                        break;

                    case AllocationScheme.Edit:
                        _allocationSchemeName = cmbAllocationScheme.Text;
                        if (_allocationSchemeName.Equals(string.Empty) || _allocationSchemeName.Equals(ApplicationConstants.C_COMBO_SELECT))
                        {
                            return;
                        }
                        if (_isModified)
                        {
                            AllocationFixedPreference fixedPref = new AllocationFixedPreference(Convert.ToInt32(cmbAllocationScheme.Value), _allocationSchemeName, _dsAllocationScheme.GetXml(), dtSchemeDate.Value, true, FixedPreferenceCreationSource.None);
                            int resultantID = AllocationSchemeDataManager.GetInstance.SaveAllocationScheme(fixedPref);// _proxyAllocationServices.InnerChannel.SaveAllocationScheme(_allocationSchemeName, dtSchemeDate.Value, _dsAllocationScheme.GetXml(), Convert.ToInt32(cmbAllocationScheme.Value));
                            if (resultantID > 0)
                            {
                                lblStatus.Text = "Allocation Scheme successfully saved at " + DateTime.Now;
                                _isModified = false;
                            }
                        }
                        break;

                    case AllocationScheme.Recon:
                        if (grdScheme.Rows.Count > 0)
                        {
                            ExportToExcel();
                        }
                        else
                        {
                            lblStatus.Text = "Nothing to Export.";
                        }
                        break;

                    default:
                        break;

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
        private void btnScreenshot_Click(object sender, EventArgs e)
        {
            try
            {
                SnapShotManager.GetInstance().TakeSnapshot(this);
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
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (_isModified)
                {
                    DialogResult result = MessageBox.Show("Do you want to save the changes?", "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    switch (result)
                    {
                        case DialogResult.Yes:
                            this.btnSave_Click(this.btnSave, e);
                            this.Close();
                            break;

                        case DialogResult.No:
                            _isModified = false;
                            this.Close();
                            break;

                        case DialogResult.Cancel:
                            //return;
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    this.Close();
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

        private void dtSchemeDate_ValueChanged(object sender, System.EventArgs e)
        {
            if (!cmbAllocationScheme.Value.Equals(int.MinValue))
            {
                btnGetScheme.Enabled = true;
            }
            else
            {
                btnGetScheme.Enabled = false;
            }
        }

        private void cmbAllocationScheme_ValueChanged(object sender, EventArgs e)
        {
            if (!_isAllocationSchemeImport.Equals(AllocationScheme.Import) && !_isInitialized)
            {
                btnGetScheme.Enabled = true;
            }
            if (cmbAllocationScheme.Value.Equals(int.MinValue))
            {
                btnGetScheme.Enabled = false;
            }
            else
            {
                btnGetScheme.Enabled = true;
            }
        }

        private void btnGetScheme_Click(object sender, EventArgs e)
        {
            if (!cmbAllocationScheme.Text.Equals(ApplicationConstants.C_COMBO_SELECT))
            {
                switch (_isAllocationSchemeImport)
                {
                    case AllocationScheme.Edit:
                        BindEditAllocationScheme(AllocationScheme.Edit, cmbAllocationScheme.Text);
                        break;
                    case AllocationScheme.Recon:
                        BindReconAllocationScheme(AllocationScheme.Recon, cmbAllocationScheme.Text);
                        break;
                    default:
                        break;
                }
            }
        }

        public event EventHandler FormClosedInformation;

        private void AllocationSchemeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //_allocationSchemeForm = null;
            if (FormClosedInformation != null)
            {
                FormClosedInformation(this, null);
            }
            _isModified = false;
            AllocationSchemeDataManager.GetInstance.DisposeAllocationServicesProxy();
        }


        private void AllocationSchemeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isModified)
            {
                DialogResult result = MessageBox.Show("Do you want to save the changes?", "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                switch (result)
                {
                    case DialogResult.Yes:
                        this.btnSave_Click(this.btnSave, e);
                        break;

                    case DialogResult.No:
                        e.Cancel = false;
                        break;

                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;

                    default:
                        break;

                }
            }
            if (!_isAborted && _isAllocationSchemeImport.Equals(AllocationScheme.Import))
            {
                if (MessageBox.Show("Are you sure to abort the import process ?", "Import", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.DialogResult = DialogResult.Abort;
                }
                else
                {
                    e.Cancel = true;

                }
            }
        }

        private void ExportToExcel()
        {
            try
            {
                int _count = 0;
                Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook();
                string pathName = null;
                saveFileDialog1.InitialDirectory = Application.StartupPath;
                saveFileDialog1.Filter = "Excel WorkBook Files (*.xls)|*.xls|All Files (*.*)|*.*";
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pathName = saveFileDialog1.FileName;
                }
                else
                {
                    return;

                }

                workBook = OnExportToExcel(_count, workBook, pathName);
                _count++;

                workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[0];
                try
                {
                    workBook.Save(pathName);
                }
                catch (Exception)
                {
                    MessageBox.Show("File is Open, Please Close the File then Save it.");
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

        public Workbook OnExportToExcel(int key, Workbook workBook, string fileName)
        {
            try
            {
                if (workBook == null)
                {
                    workBook = this.ExcelExporter.Export(grdScheme, fileName);
                }

                workBook.Worksheets.Add(this.grdScheme.Name + key);
                workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[grdScheme.Name + key];
                ExcelExporter.Export(this.grdScheme, workBook);
                return workBook;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        private void grdScheme_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {

                e.Cancel = true;
                if (grdScheme.DataSource != null)
                {
                    (this.FindForm()).AddCustomColumnChooser(this.grdScheme);
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

        private void grdScheme_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }
    }
}