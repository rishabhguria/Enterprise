using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

// This is a generic UserControl which binds AUEC Details onto the Grid, provides search facility

namespace Prana.PM.Common
{
    public partial class CtrlValidAUEC : UserControl
    {

        //public delegate void AUECSelectedHandler(ValidAUEC validAuec);
        public event EventHandler ValidAUECSelected;
        public event EventHandler CloseForm = null;

        public CtrlValidAUEC()
        {
            InitializeComponent();
        }

        public virtual void SetUP()
        {
            try
            {
                BindAuec();
                BindGrid(_dictAuec);
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }


        protected Dictionary<int, ValidAUEC> _dictAuec = new Dictionary<int, ValidAUEC>();
        //public Dictionary<int, ValidAUEC> DictAuec
        //{
        //    get { return _dictAuec; }
        //    set { _dictAuec = value; }
        //}

        //Bind all AUECS in the dictionary...
        //Dictionary<int, ValidAUEC> _dictAuec = new Dictionary<int, ValidAUEC>();
        protected virtual void BindAuec()
        {
            try
            {
                CachedDataManager cachedDataManager = CachedDataManager.GetInstance;

                Dictionary<int, string> dictAuecs = cachedDataManager.GetAllAuecs();
                Dictionary<int, double> dictMultipliers = CachedDataManager.GetInstance.AuecMultipliers;
                Dictionary<int, decimal> dictRoundLots = CommonDataCache.CachedDataManager.GetInstance.AuecRoundLot;
                //Dictionary<int, int> dictRoundOff = cachedDataManager.GetRoundOffRulesFromAUECID();

                foreach (KeyValuePair<int, string> kvpAuec in dictAuecs)
                {
                    string[] auecdetails = (kvpAuec.Value).Split(',');
                    int auecID = kvpAuec.Key;
                    ValidAUEC auecdetailwise = new ValidAUEC();

                    auecdetailwise.AssetID = int.Parse(auecdetails[0].ToString());
                    auecdetailwise.UnderlyingID = int.Parse(auecdetails[1].ToString());
                    auecdetailwise.ExchangeID = int.Parse(auecdetails[2].ToString());
                    auecdetailwise.CurrencyID = int.Parse(auecdetails[3].ToString());

                    auecdetailwise.Asset = cachedDataManager.GetAssetText(auecdetailwise.AssetID);
                    auecdetailwise.Underlying = cachedDataManager.GetUnderLyingText(auecdetailwise.UnderlyingID);
                    auecdetailwise.Exchange = cachedDataManager.GetExchangeText(auecdetailwise.ExchangeID);
                    auecdetailwise.DefaultCurrency = cachedDataManager.GetCurrencyText(auecdetailwise.CurrencyID);
                    auecdetailwise.ExchangeIdentifier = cachedDataManager.GetAUECText(auecID);
                    //added to set Multiplier and Roundlot value, PRANA-10856
                    auecdetailwise.Multiplier = dictMultipliers[auecID];
                    auecdetailwise.RoundLot = dictRoundLots[auecID];
                    _dictAuec.Add(auecID, auecdetailwise);

                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }


        // Binds AUEC RoundOff Rules on the Grid
        protected void BindGridWithoutCheckBox(Dictionary<int, ValidAUEC> dictAUEC)
        {
            try
            {

                grdAuec.DataSource = null;
                List<ValidAUEC> auec = new List<ValidAUEC>();
                foreach (KeyValuePair<int, ValidAUEC> auecDetails in dictAUEC)
                {
                    auec.Add(auecDetails.Value);
                }
                grdAuec.DataSource = auec;
                SetGridColumns(grdAuec);
                //AddCheckBox(grdAuec);

            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        // Binds AUEC RoundOff Rules on the Grid
        protected void BindGrid(Dictionary<int, ValidAUEC> dictAUEC)
        {
            try
            {

                BindGridWithoutCheckBox(dictAUEC);
                AddCheckBox(grdAuec);

            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        // Adds CheckBoxes on the Grid
        private void AddCheckBox(UltraGrid grid)
        {
            try
            {
                grid.DisplayLayout.Bands[0].Columns.Add("checkBox", "Select");
                grid.DisplayLayout.Bands[0].Columns["checkBox"].DataType = typeof(bool);
                grid.DisplayLayout.Bands[0].Columns["checkBox"].CellClickAction = CellClickAction.EditAndSelectText;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].Hidden = false;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].Header.VisiblePosition = 0;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].Width = 35;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].AllowRowFiltering = DefaultableBoolean.False;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }


        //private void ValidAUECs_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    _validAuecs = null;
        //    _symbolLookUp.AUEC = null;
        //}

        private void SelectedRow()
        {
            try
            {
                ValidAUEC loadAuec = null;
                if (grdAuec.ActiveRow != null)
                {
                    if (grdAuec.ActiveCell != null && grdAuec.ActiveCell.Column.Key == "checkBox")
                    {
                        loadAuec = (ValidAUEC)grdAuec.ActiveRow.ListObject;
                        //_symbolLookUp.AUEC = loadAuec;

                        ListEventAargs args = new ListEventAargs();
                        args.argsObject = loadAuec;
                        ValidAUECSelected(this, args);

                        grdAuec.ActiveCell = null;

                        //this.Hide();
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        private void grdAuec_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (ValidAUECSelected != null)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        SelectedRow();
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        private void grdAuec_KeyDown(object sender, KeyEventArgs e)
        {
            //Press space to select AUEC rather than clicking checkbox by mouse.
            try
            {
                if (e.KeyData.Equals(Keys.Space))
                {
                    grdAuec.ActiveCell = grdAuec.ActiveRow.Cells["checkbox"];
                    if (ValidAUECSelected != null)
                    {
                        SelectedRow();
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        //private void ValidAUECs_Load(object sender, EventArgs e)
        //{
        //    txtSearch.Focus();
        //}

        private void btnSkip_Click(object sender, EventArgs e)
        {
            try
            {
                if (CloseForm != null)
                {
                    CloseForm(null, null);
                }

            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            //CloseForm(null, null);
        }


        private void btnGetData_Click(object sender, EventArgs e)
        {
            //Search the text in the AUECS..
            string txt = txtSearch.Text.Trim().ToUpperInvariant();
            //Dictionary<int, ValidAUEC> dictauecs = new Dictionary<int, ValidAUEC>();
            try
            {
                //Empty textbox will load all AUECS...
                if (txt.Equals(string.Empty))
                {

                    grdAuec.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                    //    BindGrid(_dictAuec);
                }
                else
                {
                    //Text without space search...
                    if (!txt.Contains(" "))
                    {
                        SetGridFilters(txt);
                    }
                    else
                    {
                        // Multiple Search like (Equity Nasdaq)....
                        string[] txtDetails = txt.Split(' ');
                        foreach (string text in txtDetails)
                        {
                            SetGridFilters(text);

                        }
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        public void ClearPreviousColumnFilters()
        {
            try
            {
                grdAuec.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
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


        private void SetGridFilters(string text)
        {
            try
            {
                grdAuec.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();

                if (!string.IsNullOrEmpty(text))
                {

                    grdAuec.DisplayLayout.Bands[0].ColumnFilters.LogicalOperator = FilterLogicalOperator.Or;


                    grdAuec.DisplayLayout.Bands[0].Columns["Asset"].FilterComparisonType = FilterComparisonType.CaseInsensitive;
                    grdAuec.DisplayLayout.Bands[0].ColumnFilters["Asset"].LogicalOperator = FilterLogicalOperator.Or;
                    grdAuec.DisplayLayout.Bands[0].ColumnFilters["Asset"].FilterConditions.Add(FilterComparisionOperator.Contains, text);


                    grdAuec.DisplayLayout.Bands[0].Columns["Exchange"].FilterComparisonType = FilterComparisonType.CaseInsensitive;
                    grdAuec.DisplayLayout.Bands[0].ColumnFilters["Exchange"].LogicalOperator = FilterLogicalOperator.Or;
                    grdAuec.DisplayLayout.Bands[0].ColumnFilters["Exchange"].FilterConditions.Add(FilterComparisionOperator.Contains, text);

                    grdAuec.DisplayLayout.Bands[0].Columns["DefaultCurrency"].FilterComparisonType = FilterComparisonType.CaseInsensitive;
                    grdAuec.DisplayLayout.Bands[0].ColumnFilters["DefaultCurrency"].LogicalOperator = FilterLogicalOperator.Or;
                    grdAuec.DisplayLayout.Bands[0].ColumnFilters["DefaultCurrency"].FilterConditions.Add(FilterComparisionOperator.Contains, text);


                    grdAuec.DisplayLayout.Bands[0].Columns["ExchangeIdentifier"].FilterComparisonType = FilterComparisonType.CaseInsensitive;
                    grdAuec.DisplayLayout.Bands[0].ColumnFilters["ExchangeIdentifier"].LogicalOperator = FilterLogicalOperator.Or;
                    grdAuec.DisplayLayout.Bands[0].ColumnFilters["ExchangeIdentifier"].FilterConditions.Add(FilterComparisionOperator.Contains, text);


                    grdAuec.DisplayLayout.Bands[0].Columns["Underlying"].FilterComparisonType = FilterComparisonType.CaseInsensitive;
                    grdAuec.DisplayLayout.Bands[0].ColumnFilters["Underlying"].LogicalOperator = FilterLogicalOperator.Or;
                    grdAuec.DisplayLayout.Bands[0].ColumnFilters["Underlying"].FilterConditions.Add(FilterComparisionOperator.Contains, text);

                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion


            //grdLong.DisplayLayout.Bands[0].ColumnFilters[COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_Buy_Closed);
            //grdLong.DisplayLayout.Bands[0].ColumnFilters[COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_Buy_Open);
            //grdLong.DisplayLayout.Bands[0].ColumnFilters[COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_Buy_Cover);


        }

        // Set Grid Columns, sets their visibility
        protected virtual void SetGridColumns(UltraGrid grid)
        {
            try
            {
                ColumnsCollection columns = grid.DisplayLayout.Bands[0].Columns;

                foreach (UltraGridColumn column in columns)
                {
                    string caption = column.Header.Caption;
                    if (caption.Equals("Asset") || caption.Equals("Underlying") || caption.Equals("Exchange") || caption.Equals("DefaultCurrency") || caption.Equals("ExchangeIdentifier"))
                    {
                        column.Hidden = false;
                    }
                    else
                    {
                        column.Hidden = true;
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        //public Prana.Tools.SymbolLookUp SaveAuec()
        //{
        //    return _symbolLookUp;
        //}

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData.Equals(Keys.Enter))
                {
                    btnGetData_Click(this.btnGetData, e);
                    e.Handled = true;
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            try
            {
                txtSearch.SelectAll();
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        private void CtrlValidAUEC_Load(object sender, EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.IsDesignMode())
                {
                    SetButtonsColor();
                    CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_SYMBOL_LOOKUP);
                }
                txtSearch.Focus();
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
                btnGetData.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetData.ForeColor = System.Drawing.Color.White;
                btnGetData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetData.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetData.UseAppStyling = false;
                btnGetData.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSkip.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnSkip.ForeColor = System.Drawing.Color.White;
                btnSkip.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSkip.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSkip.UseAppStyling = false;
                btnSkip.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        //  Search AUEC On Load
        public void SearchAUECOnLoad(string exchangeIdentifier)
        {
            try
            {
                txtSearch.Text = exchangeIdentifier;
                btnGetData_Click(this, null);
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

        private void grdAuec_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {

                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdAuec);
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

        private void grdAuec_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        /// <summary>
        /// used to Export Data for automation
        /// </summary>
        /// <param name="exportFilePath"></param>
        public void ExportGridData(string exportFilePath)
        {
            try
            {
                // Create a new instance of the exporter
                UltraGridExcelExporter exporter = new UltraGridExcelExporter();
                string directoryPath = Path.GetDirectoryName(exportFilePath);
                if (!System.IO.Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                // Perform the export
                exporter.Export(grdAuec, exportFilePath);
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
    }
}
