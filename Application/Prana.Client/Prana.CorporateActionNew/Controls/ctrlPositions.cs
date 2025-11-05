using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.ClientCommon;
using Prana.LogManager;
using Prana.Utilities.UI;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Prana.CorporateActionNew.Controls
{
    public partial class ctrlPositions : UserControl
    {
        TaxlotBaseCollection _positions = new TaxlotBaseCollection();

        internal TaxlotBaseCollection Positions
        {
            get
            {
                return _positions;
            }
        }

        public ctrlPositions()
        {
            InitializeComponent();
        }

        public bool _hasWriteAccess = true;

        /// <summary>
        /// Initializes the control.
        /// </summary>
        /// <param name="controlType">Type of the control.</param>
        /// <param name="listColumn">The list column.</param>
        internal void InitControl(ControlType controlType, List<ColumnData> listColumn)
        {
            try
            {
                AssignPositions();
                AddFilter();
                //string lstColumns = string.Empty;
                //string _caApplyPreferencesFilePath = Application.StartupPath + "\\" + ApplicationConstants.PREFS_FOLDER_NAME;
                switch (controlType)
                {
                    case ControlType.Apply:
                        SetGridColumnLayout(grdPositions, listColumn);
                        mnuRedoCA.Visible = false;
                        mnuUndoCA.Visible = false;
                        mnuSaveLayoutApply.Visible = true;
                        mnuSaveLayoutRedo.Visible = false;
                        mnuSaveLayoutUndo.Visible = false;
                        mnuRemoveFiltersApply.Visible = true;
                        mnuRemoveFiltersUndo.Visible = false;
                        mnuRemoveFiltersRedo.Visible = false;
                        break;
                    case ControlType.Undo:
                        SetGridColumnLayout(grdPositions, listColumn);
                        mnuUndoCA.Visible = true;
                        mnuRedoCA.Visible = false;
                        mnuSaveLayoutUndo.Visible = true;
                        mnuSaveLayoutRedo.Visible = false;
                        mnuSaveLayoutApply.Visible = false;
                        mnuRemoveFiltersApply.Visible = false;
                        mnuRemoveFiltersUndo.Visible = true;
                        mnuRemoveFiltersRedo.Visible = false;
                        break;
                    case ControlType.Redo:
                        SetGridColumnLayout(grdPositions, listColumn);
                        mnuUndoCA.Visible = false;
                        mnuRedoCA.Visible = true;
                        mnuSaveLayoutRedo.Visible = true;
                        mnuSaveLayoutApply.Visible = false;
                        mnuSaveLayoutUndo.Visible = false;
                        mnuRemoveFiltersApply.Visible = false;
                        mnuRemoveFiltersUndo.Visible = false;
                        mnuRemoveFiltersRedo.Visible = true;
                        break;
                }

                this.mnuExportToExcel.Visible = true;

                //if (File.Exists(_caApplyPreferencesFilePath))
                //{
                //    //r = new StreamReader(_allocationPreferencesFilePath);
                //    lstColumns = (string)_Xml.ReadXml(_caApplyPreferencesFilePath, lstColumns);
                //    List<string> visibleColums = GeneralUtilities.GetListFromString(lstColumns, ',');

                //    ShowColumns(grdPositions, visibleColums);
                //    //   _allocationPreferences.AccountingMethods.AccountingMethodsTable = CachedDataManager.GetInstance.GetAccountingMethodsTable();

                //    //r.Close();
                //}
                //if No Preferences File Exist Take Default Preferences
                SetColumnCaptionsAndFormat();
                HideColumnsOnUI();
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
        /// Hides the columns on UI.
        /// </summary>
        private void HideColumnsOnUI()
        {
            try
            {
                List<string> colsToBeRemoved = new List<string>();
                colsToBeRemoved.Add("ClosedTotalCommissionandFees");
                UltraGridBand band = grdPositions.DisplayLayout.Bands[0];
                foreach (UltraGridColumn gridCol in band.Columns)
                {
                    if (colsToBeRemoved.Contains(gridCol.Key))
                    {
                        gridCol.Hidden = true;
                        gridCol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
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

        // private void ShowColumns(UltraGrid grid, List<string> columns)
        // {
        //    try
        //   {
        //       List<string> band0Column = new List<string>();
        //    band0Column = columns;
        //     List<List<string>> bandsColumns = new List<List<string>>();
        //    bandsColumns.Add(band0Column);
        //   Prana.Utilities.UI.UIUtilities.UltraWinGridUtils.DisplayColumns(bandsColumns, grid);
        // }
        // catch (Exception ex)
        // {
        //   // Invoke our policy that is responsible for making sure no secure information
        //   // gets out of our layer.
        //    bool rethrow = ExceptionLogger.HandleException(ex,LoggingConstants.POLICY_LOGANDTHROW);
        //   if (rethrow)
        //{
        //     throw;
        // }
        //}
        //  }

        /// <summary>
        /// Assigns the positions.
        /// </summary>
        private void AssignPositions()
        {
            try
            {
                grdPositions.DataSource = _positions;
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
        /// Assigns the positions.
        /// </summary>
        /// <param name="positions">The positions.</param>
        internal void AssignPositions(TaxlotBaseCollection positions)
        {
            try
            {
                _positions = positions;
                grdPositions.DataSource = _positions;
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
        /// Clears the positions.
        /// </summary>
        internal void ClearPositions()
        {
            try
            {
                _positions.Clear();
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
            //AssignPositions(_positions);            
        }

        /// <summary>
        /// Sets the column captions and format.
        /// </summary>
        private void SetColumnCaptionsAndFormat()
        {
            try
            {
                UltraGridBand band = null;
                band = grdPositions.DisplayLayout.Bands[0];

                UltraGridColumn colL2TaxlotID = band.Columns["L2TaxlotID"];
                colL2TaxlotID.Header.Caption = "TaxlotID";

                UltraGridColumn colCompanyName = band.Columns["NewCompanyName"];
                colCompanyName.Header.Caption = "Company Name";

                UltraGridColumn colAvgPrice = band.Columns["AvgPrice"];
                colAvgPrice.Header.Caption = "Unit Cost";

                UltraGridColumn colOldAvgPrice = band.Columns["OldAveragePrice"];
                colOldAvgPrice.Header.Caption = "Average Price";
                colOldAvgPrice.Format = "#,##0.000";

                UltraGridColumn colNotionalChange = band.Columns["NotionalChange"];
                colNotionalChange.Header.Caption = "Adjusted Cost Basis";
                colNotionalChange.Hidden = true;

                UltraGridColumn colNotionalValue = band.Columns["NotionalValue"];
                colNotionalValue.Header.Caption = "Notional Value (Local)";
                colNotionalValue.Format = "#,##0.00";

                UltraGridColumn colNotionalValueBase = band.Columns["NotionalValueBase"];
                colNotionalValueBase.Header.Caption = "Notional Value (Base)";
                colNotionalValueBase.Format = "#,##0.00";

                UltraGridColumn colCorpActionID = band.Columns["CorpActionID"];
                colCorpActionID.Header.Caption = "Corporate ActionID";

                UltraGridColumn colNewAvgPrice = band.Columns["NewAvgPrice"];
                colNewAvgPrice.Header.Caption = "New Average Price";
                colNewAvgPrice.Format = "#,##0.000";
                colNewAvgPrice.Hidden = true;

                UltraGridColumn colNewOpenQty = band.Columns["NewTaxlotOpenQty"];
                colNewOpenQty.Header.Caption = "New Taxlot Open Quantity";

                UltraGridColumn colPositionTag = band.Columns["PositionTag"];
                //colPositionTag.Header.Caption = "Transaction Type";
                colPositionTag.Hidden = true;

                UltraGridColumn colTransactionType = band.Columns["TransactionType"];
                colTransactionType.Header.Caption = "Transaction Type";

                UltraGridColumn colTradeDate = band.Columns["AUECLocalDate"];
                colTradeDate.Header.Caption = "Trade Date";

                UltraGridColumn colOpenQty = band.Columns["OpenQty"];
                colOpenQty.Header.Caption = "Open Qty";
                colOpenQty.Format = "#,##0.00";

                UltraGridColumn colOpenTotalCommissionandFees = band.Columns["OpenTotalCommissionandFees"];
                colOpenTotalCommissionandFees.Header.Caption = "Open Total Commission and Fees";

                UltraGridColumn colProcessDate = band.Columns["ProcessDate"];
                colProcessDate.Header.Caption = "Process Date";

                UltraGridColumn colOriginalPurchaseDate = band.Columns["OriginalPurchaseDate"];
                colOriginalPurchaseDate.Header.Caption = "Original Open Date";

                UltraGridColumn colDivPayoutDate = band.Columns["DivPayoutDate"];
                colDivPayoutDate.Header.Caption = "Dividend Payout Date";

                UltraGridColumn colExDivDate = band.Columns["ExDivDate"];
                colExDivDate.Header.Caption = "Ex Dividend Date";

                UltraGridColumn colRecordDate = band.Columns["RecordDate"];
                colRecordDate.Header.Caption = "Record Date";

                UltraGridColumn colDivDeclarationDate = band.Columns["DivDeclarationDate"];
                colDivDeclarationDate.Header.Caption = "Dividend Declaration Date";

                if (grdPositions.DisplayLayout.Bands[0].Columns.Contains("Dividend"))
                    grdPositions.DisplayLayout.Bands[0].Columns["Dividend"].Format = "#,##0.00";

                UltraGridColumn colSettlementDate = band.Columns["SettlementDate"];
                colSettlementDate.Header.Caption = "Settlement Date";

                UltraGridColumn colFXConversionMethodOperator = band.Columns["FXConversionMethodOperator"];
                colFXConversionMethodOperator.Header.Caption = "FX Conversion Operator";

                UltraGridColumn colFXRate = band.Columns["FXRate"];
                colFXRate.Header.Caption = "FX Rate";
                colFXRate.Format = "#,##0.0000";

                grdPositions.Refresh();

                grdPositions.DisplayLayout.Override.GroupByRowAppearance.ForeColor = Color.White;
                grdPositions.DisplayLayout.Override.GroupByRowAppearance.FontData.Name = "Tahoma";
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
        /// Rowwise Filters on Strike Price and Expiration Month
        /// </summary>
        /// <param name="multipleGridMonths"></param>
        private void AddFilter()
        {
            try
            {
                // If the row filter mode is band based, then get the column filters off the band
                ColumnFiltersCollection columnFilters = grdPositions.DisplayLayout.Bands[0].ColumnFilters;
                ///TODO : When we apply the custom filters we need to change the code so the filters won't be on a common field
                columnFilters.ClearAllFilters();

                //if (grdPositions.DataSource != null)
                //{
                //    //columnFilters["OpenQty"].FilterConditions.Add(FilterComparisionOperator.NotEquals, 0);
                //}
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

        private void grdPositions_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                string positionTag = e.Row.Cells["PositionTag"].Value.ToString();

                if (positionTag.Contains("Addition") || positionTag.Contains("Withdrawal") || positionTag.Contains("CostAdj"))
                {
                    e.Row.Appearance.BackColor = Color.OrangeRed;
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

        private void grdPositions_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdPositions);
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

        internal event EventHandler OnUndoCAClick;
        internal event EventHandler OnRedoCAClick;

        private void mnuUndoCA_Click(object sender, EventArgs e)
        {
            if (OnUndoCAClick != null)
            {
                OnUndoCAClick(this, EventArgs.Empty);
            }
        }

        private void mnuRedoCA_Click(object sender, EventArgs e)
        {
            if (OnRedoCAClick != null)
            {
                OnRedoCAClick(this, EventArgs.Empty);
            }
        }

        internal void DisableContextMenu()
        {
            try
            {
                mnuUndoCA.Enabled = false;
                mnuRedoCA.Enabled = false;
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

        internal void EnableContextMenuandUpdateCaption(CorporateActionType caType)
        {
            try
            {
                UltraGridBand band = grdPositions.DisplayLayout.Bands[0];
                foreach (UltraGridColumn col in band.Columns)
                {

                    if (col.Header.Caption == "Dividend" && caType == CorporateActionType.StockDividend)
                    {
                        col.Header.Caption = "Dividend Factor";
                    }
                    else if (col.Header.Caption == "Dividend Factor" && caType != CorporateActionType.StockDividend)
                    {
                        col.Header.Caption = "Dividend";
                    }
                    if (col.Header.Caption == "NewTaxlotOpenQty")
                        if (caType == CorporateActionType.Split)
                        {
                            col.Hidden = false;
                        }
                        else
                            col.Hidden = true;

                }
                mnuUndoCA.Enabled = true;
                mnuRedoCA.Enabled = true;
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
        /// Events to save Layout on three CA controls
        /// </summary>
        /// 
        internal event EventHandler OnApplyLayoutSave;
        internal event EventHandler OnRedoLayoutSave;
        internal event EventHandler OnUndoLayoutSave;

        internal event EventHandler OnApplyRemoveFilters;
        internal event EventHandler OnUndoRemoveFilters;
        internal event EventHandler OnRedoRemoveFilters;

        static CustomXmlSerializer _Xml = new CustomXmlSerializer();
        //string _caApplyPreferencesFilePath = string.Empty;


        private void saveLayoutToolStripMenuItemApply_Click(object sender, EventArgs e)
        {
            if (OnApplyLayoutSave != null)
            {
                OnApplyLayoutSave(this, EventArgs.Empty);
            }

        }

        private void saveLayoutToolStripMenuItemUndo_Click(object sender, EventArgs e)
        {
            if (OnUndoLayoutSave != null)
            {
                OnUndoLayoutSave(this, EventArgs.Empty);
            }
        }

        private void saveLayoutToolStripMenuItemRedo_Click(object sender, EventArgs e)
        {
            if (OnRedoLayoutSave != null)
            {
                OnRedoLayoutSave(this, EventArgs.Empty);
            }

        }

        private void removeFiltersToolStripMenuItemUndo_Click(object sender, EventArgs e)
        {
            if (OnUndoRemoveFilters != null)
            {
                OnUndoRemoveFilters(this, EventArgs.Empty);
            }
        }

        private void removeFiltersToolStripMenuItemRedo_Click(object sender, EventArgs e)
        {
            if (OnRedoRemoveFilters != null)
            {
                OnRedoRemoveFilters(this, EventArgs.Empty);
            }
        }

        private void removeFiltersToolStripMenuItemApply_Click(object sender, EventArgs e)
        {
            if (OnApplyRemoveFilters != null)
            {
                OnApplyRemoveFilters(this, EventArgs.Empty);
            }

        }

        private void SetGridColumnLayout(UltraGrid grid, List<ColumnData> listColData)
        {
            UltraGridBand band = grid.DisplayLayout.Bands[0];
            ColumnsCollection gridColumns = band.Columns;
            listColData.Sort();

            try
            {
                if (listColData != null && listColData.Count > 0)
                {
                    // Hide All
                    foreach (UltraGridColumn gridCol in gridColumns)
                    {
                        gridCol.Hidden = true;
                    }

                    //Set Columns Properties
                    foreach (ColumnData colData in listColData)
                    {
                        if (gridColumns.Exists(colData.Key))
                        {
                            UltraGridColumn gridCol = gridColumns[colData.Key];
                            gridCol.Width = colData.Width;
                            gridCol.Format = colData.Format;
                            gridCol.Header.Caption = colData.Caption;
                            gridCol.Header.VisiblePosition = colData.VisiblePosition;
                            gridCol.Hidden = colData.Hidden;
                            gridCol.ExcludeFromColumnChooser = colData.ExcludeFromColumnChooser;
                            gridCol.Header.Fixed = colData.Fixed;
                            gridCol.SortIndicator = colData.SortIndicator;
                            gridCol.CellActivation = colData.CellActivation;

                            // Filter Settings
                            if (colData.FilterConditionList.Count > 0)
                            {
                                band.ColumnFilters[colData.Key].LogicalOperator = colData.FilterLogicalOperator;
                                foreach (FilterCondition fCond in colData.FilterConditionList)
                                {
                                    band.ColumnFilters[colData.Key].FilterConditions.Add(fCond);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (UltraGridColumn gridCol in gridColumns)
                    {
                        gridCol.Hidden = false;
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        private void ctrlPositions_Load(object sender, EventArgs e)
        {

        }

        private void grdPositions_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(MousePosition);
            }
        }


        internal void RenameDivField()
        {
            try
            {
                UltraGridBand band = grdPositions.DisplayLayout.Bands[0];
                foreach (UltraGridColumn col in band.Columns)
                {
                    if (col.Header.Caption == "Dividend")
                    {
                        col.Header.Caption = "Dividend Factor";
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
        /// <summary>
        /// Added By Faisal Shah
        /// Set Grid as read only if user has only Read permissions
        /// </summary>
        internal void SetGridAsReadOnly()
        {
            try
            {
                grdPositions.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                foreach (ToolStripMenuItem item in mnuSaveLayoutApply.DropDownItems)
                {
                    item.Enabled = false;
                }
                foreach (ToolStripMenuItem item in mnuSaveLayoutRedo.DropDownItems)
                {
                    item.Enabled = false;
                }
                foreach (ToolStripMenuItem item in mnuSaveLayoutUndo.DropDownItems)
                {
                    item.Enabled = false;
                }
                foreach (ToolStripMenuItem item in mnuRedoCA.DropDownItems)
                {
                    item.Enabled = false;
                }
                foreach (ToolStripMenuItem item in mnuUndoCA.DropDownItems)
                {
                    item.Enabled = false;
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

        private void grdPositions_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        /// <summary>
        /// Exports the positions.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        internal void ExportPositions(string fileName)
        {
            try
            {
                fileName = String.Format("{0}\\{1}{2}{3}", Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName), "_Positions", Path.GetExtension(fileName));
                this.ultraGridExcelExporter1.Export(this.grdPositions, fileName);
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
        /// Handles the Click event of the mnuExportToExcel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void mnuExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                List<UltraGrid> lstGrids = new List<UltraGrid>();
                lstGrids.Add(grdPositions);
                ExcelAndPrintUtilities excelUtils = new ExcelAndPrintUtilities();
                excelUtils.SetExcelLayoutAndWrite(lstGrids, false, null);
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
    }

}
