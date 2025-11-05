using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.CostAdjustment.Definitions;
using Infragistics.Win;
using Prana.BusinessObjects.CostAdjustment.Delegates;
using Prana.BusinessObjects.CostAdjustment.EventArguments;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.BusinessLogic;
using Prana.CommonDataCache;

namespace Prana.AllocationNew.Allocation.UI.CostAdjustment
{
    public partial class CostAdjustmentUndoGridControl : UserControl
    {
        /// <summary>
        /// The _taxlot list
        /// </summary>
        private List<CostAdjustmentTaxlotForUndo> _taxlotList = new List<CostAdjustmentTaxlotForUndo>();

        /// <summary>
        /// The locker object
        /// </summary>
        object lockerObject = new object();

        //List that will add selected column list on Grid
        private List<UltraGridRow> _selectedColumnList = new List<UltraGridRow>();

        public CostAdjustmentUndoGridControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the InitializeLayout event of the ultraGrid1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs" /> instance containing the event data.</param>
        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                if (ultraGrid1.DisplayLayout.Bands[0].Columns.Exists("Checkbox"))
                    return;

                UltraGridColumn checkColumn = ultraGrid1.DisplayLayout.Bands[0].Columns.Add("Checkbox", String.Empty);
                checkColumn.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                checkColumn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                //The checkbox and the cell values are kept in synch to affect only the RowsCollection
                checkColumn.Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.RowsCollection;
                //'Aligns the Header checkbox to the right of the Header caption
                checkColumn.Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center;
                checkColumn.DataType = typeof(Boolean);
                checkColumn.CellActivation = Activation.AllowEdit;
                checkColumn.Header.VisiblePosition = 0;
                checkColumn.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                checkColumn.AutoSizeMode = ColumnAutoSizeMode.None;
                checkColumn.AllowRowFiltering = DefaultableBoolean.False;
                checkColumn.SortIndicator = SortIndicator.Disabled;
                checkColumn.Width = 1;

                for (int i = 0; i < 2; i++)
                {
                    HideColumns(i);

                    foreach (UltraGridColumn col in ultraGrid1.DisplayLayout.Bands[i].Columns)
                    {
                        ultraGrid1.DisplayLayout.Bands[i].Columns[col.Index].Header.Caption = SplitCamelCase(col.ToString());
                    }

                    if (ultraGrid1.DisplayLayout.Bands[i].Columns.Exists("OrderSide"))
                        ultraGrid1.DisplayLayout.Bands[i].Columns["OrderSide"].Header.Caption = "Side";
                    if (ultraGrid1.DisplayLayout.Bands[i].Columns.Exists("OrderQuantity"))
                        ultraGrid1.DisplayLayout.Bands[i].Columns["OrderQuantity"].Header.Caption = "Executed Quantity";
                    if (ultraGrid1.DisplayLayout.Bands[i].Columns.Exists("CAID"))
                        ultraGrid1.DisplayLayout.Bands[i].Columns["CAID"].Header.Caption = "CostAdjustmentID";
                    if (ultraGrid1.DisplayLayout.Bands[i].Columns.Exists("Checkbox"))
                        ultraGrid1.DisplayLayout.Bands[i].Columns["Checkbox"].Header.Caption = "";
                }
                lock (lockerObject)
                {
                    this.ultraGrid1.DataSource = _taxlotList;
                }
                this.ultraGrid1.DisplayLayout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;

            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Splits string in camel case.
        /// </summary>
        /// <param name="p">String</param>
        /// <returns>Split String by Camel case</returns>
        private string SplitCamelCase(string input)
        {
            try
            {
                return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// Hides specific columns on UI
        /// </summary>
        /// <param name="i">Index of Band</param>
        private void HideColumns(int i)
        {
            try
            {
                if (ultraGrid1.DisplayLayout.Bands[i].Columns.Exists("PositionTag"))
                    ultraGrid1.DisplayLayout.Bands[i].Columns["PositionTag"].Hidden = true;
                if (ultraGrid1.DisplayLayout.Bands[i].Columns.Exists("OrderSideTagValue"))
                    ultraGrid1.DisplayLayout.Bands[i].Columns["OrderSideTagValue"].Hidden = true;
                if (ultraGrid1.DisplayLayout.Bands[i].Columns.Exists("NewUnitCost"))
                    ultraGrid1.DisplayLayout.Bands[i].Columns["NewUnitCost"].Hidden = true;
                if (ultraGrid1.DisplayLayout.Bands[i].Columns.Exists("NewTotalCost"))
                    ultraGrid1.DisplayLayout.Bands[i].Columns["NewTotalCost"].Hidden = true;
                if (ultraGrid1.DisplayLayout.Bands[i].Columns.Exists("CashImpactOfAdjustment"))
                    ultraGrid1.DisplayLayout.Bands[i].Columns["CashImpactOfAdjustment"].Hidden = true;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Binds the data source to grid.
        /// </summary>
        /// <param name="bindingList">The binding list.</param>
        internal void BindDataSourceToGrid(List<CostAdjustmentTaxlotForUndo> bindingList)
        {
            try
            {
                lock (lockerObject)
                {
                    _taxlotList.Clear();
                    _taxlotList.AddRange(bindingList);
                    ultraGrid1.DataSource = _taxlotList;
                    ultraGrid1.Rows.Refresh(Infragistics.Win.UltraWinGrid.RefreshRow.ReloadData);
                }
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Binds the new taxlots to grid.
        /// </summary>
        /// <param name="bindingList">Taxlot list</param>
        internal void BindNewTaxlotsToGrid(List<CostAdjustmentTaxlotForUndo> bindingList)
        {
            try
            {
                lock (lockerObject)
                {
                    if (bindingList != null && bindingList.Count > 0)
                    {
                        _taxlotList.AddRange(bindingList.Where(x => !_taxlotList.Any(y => x.TaxlotId == y.TaxlotId)));
                        ultraGrid1.Rows.Refresh(Infragistics.Win.UltraWinGrid.RefreshRow.ReloadData);
                    }
                }
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Removes CostAdjustmentTaxlotForUndo taxlots from grid
        /// </summary>
        /// <param name="taxlotIds">list of taxlot Ids</param>
        internal void RemoveTaxlotsFromGrid(List<string> taxlotIds)
        {
            try
            {
                lock (lockerObject)
                {
                    if (taxlotIds != null && taxlotIds.Count > 0)
                    {
                        foreach (string taxlotID in taxlotIds)
                        {
                            CostAdjustmentTaxlotForUndo removeTaxlot = _taxlotList.FirstOrDefault(x => x.TaxlotId == taxlotID);
                            _taxlotList.Remove(removeTaxlot);
                        }
                        ultraGrid1.Rows.Refresh(Infragistics.Win.UltraWinGrid.RefreshRow.ReloadData);
                    }
                }
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Handles the ClickCell event of the ultraGrid1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ClickCellEventArgs" /> instance containing the event data.</param>
        private void ultraGrid1_ClickCell(object sender, ClickCellEventArgs e)
        {
            try
            {
                if (e.Cell.StyleResolved == Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox)
                {
                    Boolean cellChecked = (Boolean)e.Cell.Row.GetCellValue("Checkbox");
                    if (cellChecked == true)
                        e.Cell.SetValue(false, true);
                    else
                        e.Cell.SetValue(true, true);
                    ultraGrid1.ActiveCell = null;
                }
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        ///Before Header Check State Changed Event that will update gird after header selected.
        /// Issue was On Checkbox selection,All Trades are being selected although they are not filtered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrid1_BeforeHeaderCheckStateChanged(object sender, BeforeHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                if (ultraGrid1.Rows.Count > 0)
                {
                    _selectedColumnList.Clear();
                    foreach (UltraGridRow row in ultraGrid1.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells["Checkbox"].Value) == true)
                        {
                            _selectedColumnList.Add(row);
                        }
                        else
                        {
                            if (_selectedColumnList.Contains(row))
                                _selectedColumnList.Remove(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// After Header Check State Changed Event that will update gird after header selected.
        /// Issue was On Checkbox selection,All Trades are being selected although they are not filtered.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">AfterHeaderCheckStateChangedEventArgs</param>
        private void ultraGrid1_AfterHeaderCheckStateChanged(object sender, AfterHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                if (ultraGrid1.DisplayLayout.Bands[0].Columns["Checkbox"].Header.Selected != true)
                {
                    _selectedColumnList.Clear();
                }

                if (ultraGrid1.Rows.Count > 0 && ultraGrid1.Rows.GetFilteredOutNonGroupByRows() != null)
                {
                    UltraGridRow[] grdrows = ultraGrid1.Rows.GetFilteredOutNonGroupByRows();

                    if (grdrows.Length > 0 && ultraGrid1.Rows.Count > 0)
                    {
                        foreach (UltraGridRow row in grdrows)
                        {
                            string state = row.Cells["Checkbox"].Value.ToString();
                            if (state.Equals("True"))
                            {
                                row.Cells["Checkbox"].Value = false;
                            }
                        }
                    }
                    foreach (UltraGridRow row in _selectedColumnList)
                    {
                        row.Cells["Checkbox"].Value = true;
                    }
                }
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the selected taxlot.
        /// </summary>
        /// <returns>List CostAdjustmentTaxlot.</returns>
        internal List<CostAdjustmentTaxlotForUndo> GetSelectedTaxlot(bool getAll)
        {
            try
            {
                List<CostAdjustmentTaxlotForUndo> selectedRows = new List<CostAdjustmentTaxlotForUndo>();
                UltraGridRow[] rows = ultraGrid1.Rows.GetFilteredInNonGroupByRows();
                foreach (UltraGridRow row in rows)
                {
                    CostAdjustmentTaxlotForUndo taxlot = (CostAdjustmentTaxlotForUndo)row.ListObject;
                    if (Convert.ToBoolean(row.Cells["checkBox"].Value.ToString()))
                    {
                        selectedRows.Add(taxlot);
                    }
                    else
                    {
                        if (getAll)
                        {
                            selectedRows.Add(taxlot);
                        }
                    }
                }
                return selectedRows;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

    }
}
