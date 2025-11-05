// ***********************************************************************
// Assembly         : Prana.AllocationNew
// Author           : Shagoon.Gurtata
// Created          : 10-03-2014
//
// Last Modified By : Shagoon.Gurtata
// Last Modified On : 02-23-2015
// ***********************************************************************
// <copyright file="CostAdjustmentGridControl.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
using System.IO;

/// <summary>
/// The CostAdjustment namespace.
/// </summary>
namespace Prana.AllocationNew.Allocation.UI.CostAdjustment
{
    /// <summary>
    /// Class CostAdjustmentGridControl.
    /// </summary>
    public partial class CostAdjustmentGridControl : UserControl
    {
        /// <summary>
        /// Occurs when [total quantity updated].
        /// </summary>
        public event CostAdjustmentHandler TotalQuantityUpdated;

        /// <summary>
        /// Initializes a new instance of the <see cref="CostAdjustmentGridControl" /> class.
        /// </summary>
        public CostAdjustmentGridControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The _taxlot list
        /// </summary>
        private List<CostAdjustmentTaxlot> _taxlotList = new List<CostAdjustmentTaxlot>();

        /// <summary>
        /// The taxlot list for taxlots generated on preview
        /// </summary>
        private List<CostAdjustmentTaxlot> _previewBindingList = new List<CostAdjustmentTaxlot>();

        /// <summary>
        /// Handles the InitializeLayout event of the ultraGrid1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs" /> instance containing the event data.</param>
        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                if (!e.Layout.Bands[0].Columns.Exists("IsPreview"))
                {
                    this.ultraGrid1.DisplayLayout.Bands[0].Columns.Add("IsPreview");
                    this.ultraGrid1.DisplayLayout.Bands[0].Columns["IsPreview"].DataType = typeof(Boolean);
                    this.ultraGrid1.DisplayLayout.Bands[0].Columns["IsPreview"].DefaultCellValue = false;
                    this.ultraGrid1.DisplayLayout.Bands[0].Columns["IsPreview"].Hidden = true;
                }

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
				
				//Hide PositionTag and OrderSideTagValue column on cost adjustment UI 
                if (ultraGrid1.DisplayLayout.Bands[0].Columns.Exists("PositionTag"))
                    ultraGrid1.DisplayLayout.Bands[0].Columns["PositionTag"].Hidden = true;
                if (ultraGrid1.DisplayLayout.Bands[0].Columns.Exists("OrderSideTagValue"))
                    ultraGrid1.DisplayLayout.Bands[0].Columns["OrderSideTagValue"].Hidden = true;

                foreach (UltraGridColumn col in ultraGrid1.DisplayLayout.Bands[0].Columns)
                {
                    ultraGrid1.DisplayLayout.Bands[0].Columns[col.Index].Header.Caption = SplitCamelCase(col.ToString());
                }

                lock (lockerObject)
                {
                    this.ultraGrid1.DataSource = _taxlotList;
                }
                this.ultraGrid1.DisplayLayout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
				
				//Changed Caption of OrderSide and Order Quantity to Side and Executed Quantity
                if (ultraGrid1.DisplayLayout.Bands[0].Columns.Exists("OrderSide"))
                    ultraGrid1.DisplayLayout.Bands[0].Columns["OrderSide"].Header.Caption = "Side";
                if (ultraGrid1.DisplayLayout.Bands[0].Columns.Exists("OrderQuantity"))
                    ultraGrid1.DisplayLayout.Bands[0].Columns["OrderQuantity"].Header.Caption = "Executed Quantity";
                if (ultraGrid1.DisplayLayout.Bands[0].Columns.Exists("Checkbox"))
                    ultraGrid1.DisplayLayout.Bands[0].Columns["Checkbox"].Header.Caption = "";

                // load the saveout file if it exists
                string gridLayoutFile = Application.StartupPath + "\\Prana Preferences\\" + Prana.CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + "\\CostAdjustmentLayout.xml";
                if (File.Exists(gridLayoutFile))
                {
                    ultraGrid1.DisplayLayout.LoadFromXml(gridLayoutFile);
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
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// Binds the data source to grid.
        /// </summary>
        /// <param name="bindingList">The binding list.</param>
        internal void BindDataSourceToGrid(List<CostAdjustmentTaxlot> bindingList)
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
        /// <param name="isAddTaxlots">Add taxlots if true, remove taxlots otherwise</param>
        internal void BindTaxlotsToGrid(List<CostAdjustmentTaxlot> bindingList, bool isAddTaxlots)
        {
            try
            {
                lock (lockerObject)
                {
                    if (isAddTaxlots)
                    {
                        _taxlotList.AddRange(bindingList.Where(x => !_taxlotList.Any(y => x.TaxlotId == y.TaxlotId)));
                        ultraGrid1.Rows.Refresh(Infragistics.Win.UltraWinGrid.RefreshRow.ReloadData);
                    }
                    else
                    {
                        foreach (CostAdjustmentTaxlot taxlot in bindingList)
                        {
                            CostAdjustmentTaxlot removeTaxlot = _taxlotList.FirstOrDefault(x => x.TaxlotId == taxlot.TaxlotId);
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
        /// The _total qauntity
        /// </summary>
        decimal _totalQauntity = 0;

        /// <summary>
        /// The _total cost
        /// </summary>
        decimal _totalCost = 0;

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
        /// Updates the total cost.
        /// </summary>
        /// <param name="cellChecked">if set to <c>true</c> [cell checked].</param>
        /// <param name="ultraGridCell">The ultra grid cell.</param>
        private void UpdateTotalCost(bool cellChecked, UltraGridCell ultraGridCell)
        {
            try
            {
                int sideMultiplier = PMCalculations.GetSideMultilpier(TagDatabaseManager.GetInstance.GetOrderSideValue(ultraGridCell.Row.GetCellValue("OrderSide").ToString()));
                if (cellChecked == true)
                {
                    _totalQauntity += Convert.ToDecimal(ultraGridCell.Row.GetCellValue("RemainingOpenQuantity"));
                    _totalCost += Convert.ToDecimal(ultraGridCell.Row.GetCellValue("TotalCost"));
                }
                else
                {
                    _totalQauntity -= Convert.ToDecimal(ultraGridCell.Row.GetCellValue("RemainingOpenQuantity"));
                    _totalCost -= Convert.ToDecimal(ultraGridCell.Row.GetCellValue("TotalCost"));
                }
                if (TotalQuantityUpdated != null)
                    TotalQuantityUpdated(this, new CostAdjustmentEventArgs() { TotalQty = _totalQauntity, TotalCost = _totalCost });
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
        /// Gets the selected taxlot.
        /// </summary>
        /// <returns>List&lt;CostAdjustmentTaxlot&gt;.</returns>
        internal List<CostAdjustmentTaxlot> GetSelectedTaxlot(out string errorMessage)
        {
            try
            {
                List<CostAdjustmentTaxlot> selectedRows = new List<CostAdjustmentTaxlot>();
                UltraGridRow[] rows = ultraGrid1.Rows.GetFilteredInNonGroupByRows();
                foreach (UltraGridRow row in rows)
                {
                    if (Convert.ToBoolean(row.Cells["checkBox"].Value.ToString()))
                    {
                        CostAdjustmentTaxlot taxlot = (CostAdjustmentTaxlot)row.ListObject;
                        //if (!(taxlot.TransactionType.Equals(TradingTransactionType.ShortAddition.ToString()) || taxlot.TransactionType.Equals(TradingTransactionType.LongAddition.ToString())))
                        //{
                            //if (taxlot.TransactionType.Equals(TradingTransactionType.ShortCostAdj.ToString()) || taxlot.TransactionType.Equals(TradingTransactionType.LongCostAdj.ToString()))
                            //{

                            selectedRows.Add(taxlot);
                            //} 
                        //}
                    }
                }
                foreach (CostAdjustmentTaxlot taxlot in selectedRows)
                { 
                    if(taxlot.Symbol != selectedRows[0].Symbol)
                    {
                        errorMessage = "Please select taxlots with same Symbols.";
                        return null;
                    }
                }
                errorMessage = string.Empty;
                return selectedRows;
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                errorMessage = string.Empty;
                return null;
            }
        }


        /// <summary>
        /// The locker object
        /// </summary>
        object lockerObject = new object();

        /// <summary>
        /// Updates the data in grid.
        /// </summary>
        /// <param name="list">The list.</param>
        internal void UpdateDataInGrid(List<CostAdjustmentTaxlot> list, bool isPreview)
        {
            try
            {
                lock (lockerObject)
                {
                    foreach (CostAdjustmentTaxlot taxlot in list)
                    {
                        List<CostAdjustmentTaxlot> taxlotList = new List<CostAdjustmentTaxlot>();
                        foreach(CostAdjustmentTaxlot selectedTaxlot in _taxlotList)//.SingleOrDefault(a => a.TaxlotId == taxlot.TaxlotId));
                        {
                            if(selectedTaxlot != null && selectedTaxlot.TaxlotId == taxlot.TaxlotId)
                                taxlotList.Add(selectedTaxlot);
                        }
                        foreach(CostAdjustmentTaxlot selectedTaxlot in taxlotList)
                        {
                            _taxlotList.Remove(selectedTaxlot);
                        }
                        //_taxlotList.Add(taxlot);
                    }
                    _taxlotList.AddRange(list);
                    _previewBindingList.AddRange(list);
                    ultraGrid1.Rows.Refresh(Infragistics.Win.UltraWinGrid.RefreshRow.ReloadData);
                    //ultraGrid1.DataSource = _previewBindingList;
                    UpdateCostAndQuantity();
                    foreach(CostAdjustmentTaxlot taxlot in list)
                    {
                       foreach (UltraGridRow row in ultraGrid1.Rows)
                        {
                            if(taxlot.TaxlotId == row.GetCellValue("TaxlotId").ToString())
                            row.Cells["IsPreview"].Value = isPreview;
                        }
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
        /// Updates the cost and quantity.
        /// </summary>
        private void UpdateCostAndQuantity()
        {
            try
            {
                _totalQauntity = 0.0M;
                _totalCost = 0.0M;
                List<CostAdjustmentTaxlot> selectedRows = new List<CostAdjustmentTaxlot>();
                UltraGridRow[] rows = ultraGrid1.Rows.GetFilteredInNonGroupByRows();

                //To check if any of the row in unchecked then count will increment and will compare with shows row on grid count to reset value of text boxes
                //If No row is selected
                int count = 0;
                foreach (UltraGridRow row in rows)
                {
                    if (!Convert.ToBoolean(row.Cells["checkBox"].Value.ToString()))
                    {
                        count++;
                    }
                }
                
                //If No row selected and displayed row is o then update total quantity, total cost, adjust cost, adjust quantity with 0.
                if (rows.Count() == 0 || count==rows.Count())
                {
                    if (TotalQuantityUpdated != null)
                        TotalQuantityUpdated(this, new CostAdjustmentEventArgs() { TotalQty = 0, TotalCost = 0, AdjustCost = 0, AdjustQty = 0 });
                }
                foreach (UltraGridRow row in rows)
                {
                    if (Convert.ToBoolean(row.Cells["checkBox"].Value.ToString()))
                    {
                        CostAdjustmentTaxlot taxlot = (CostAdjustmentTaxlot)row.ListObject;
                        //if (!(taxlot.TransactionType.Equals(TradingTransactionType.ShortAddition.ToString()) || taxlot.TransactionType.Equals(TradingTransactionType.LongAddition.ToString())))
                            UpdateTotalCost(true, row.Cells["checkBox"]);
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
        /// Handles the AfterCellUpdate event of the ultraGrid1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellEventArgs"/> instance containing the event data.</param>
        private void ultraGrid1_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.StyleResolved == Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox)
                {
                    Boolean cellChecked = (Boolean)e.Cell.Row.GetCellValue("Checkbox");
                    if (!(e.Cell.Row.GetCellValue("TransactionType").ToString().Equals(TradingTransactionType.ShortAddition.ToString()) || e.Cell.Row.GetCellValue("TransactionType").ToString().Equals(TradingTransactionType.LongAddition.ToString()) || e.Cell.Row.GetCellValue("TransactionType").ToString().Equals(TradingTransactionType.LongCostAdj.ToString()) || e.Cell.Row.GetCellValue("TransactionType").ToString().Equals(TradingTransactionType.ShortCostAdj.ToString())))
                    {
                        UpdateTotalCost(cellChecked, e.Cell);
                    }
                    else if (e.Cell.Row.GetCellValue("TransactionType").ToString().Equals(TradingTransactionType.ShortAddition.ToString()) || e.Cell.Row.GetCellValue("TransactionType").ToString().Equals(TradingTransactionType.LongAddition.ToString()))
                    {
                        if (e.Cell.Row.GetCellValue("IsPreview").ToString() == "True")
                        {
                            if (cellChecked)
                                e.Cell.SetValue(false, true);
                        }
                        else
                            UpdateTotalCost(cellChecked, e.Cell);
                    }
                    else
                    {
                        if (cellChecked)
                            e.Cell.SetValue(false, true);
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
        /// Gets the total cost.
        /// </summary>
        /// <value>The total cost.</value>
        public decimal TotalCost { get { return _totalCost;} }

        /// <summary>
        /// Gets the total quantity.
        /// </summary>
        /// <value>The total quantity.</value>
        public decimal TotalQuantity { get { return _totalQauntity;} }

        /// <summary>
        /// Updates data on grid on save click
        /// </summary>
        /// <param name="list">List of original taxlots to be adjusted</param>
        internal void UpdateDataInGridOnSave(List<TaxLot> list)
        {
            try
            {
                //If any original taxlot exist in _taxlotList on which cost adjustment has been done, then it is removed from _taxlotList
                if (list != null && list.Count > 0)
                {
                    CostAdjustmentTaxlot oldTaxlot = new CostAdjustmentTaxlot();
                    foreach (TaxLot t in list)
                    {
                        oldTaxlot = _taxlotList.FirstOrDefault(x => x.TaxlotId == t.TaxLotID);
                        _taxlotList.Remove(oldTaxlot);
                    }
                }
                //removed taxlots added during preview from grid
                foreach (CostAdjustmentTaxlot taxlot in _previewBindingList)
                {
                    _taxlotList.Remove(taxlot);
                }
                _previewBindingList.Clear();
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
        /// Updates the taxlots
        /// </summary>
        /// <param name="list">The taxlots list</param>
        internal void UpdateTaxlots(List<CostAdjustmentTaxlot> list)
        {
            try
            {
                lock (lockerObject)
                {
                        foreach (CostAdjustmentTaxlot taxlot in list)
                        {
                            CostAdjustmentTaxlot removeTaxlot = _taxlotList.FirstOrDefault(x => x.TaxlotId == taxlot.TaxlotId);
                            _taxlotList.Remove(removeTaxlot);
                        }
                        _taxlotList.AddRange(list.Where(x => !_taxlotList.Any(y => x.TaxlotId == y.TaxlotId)));
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
        /// resets total cost and quntity to 0
        /// </summary>
        internal void ResetValues()
        {
            try
            {
                _totalCost = 0;
                _totalQauntity = 0;
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

        //List that will add selected column list on Grid
        private List<UltraGridRow> _selectedColumnList = new List<UltraGridRow>();
        
        /// <summary>
        /// After Header Check State Changed Event that will update gird after header selected.
        /// Issue was On Checkbox selection,All Trades are being selected although they are not filtered.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">AfterHeaderCheckStateChangedEventArgs</param>
        private void ultraGrid1_AfterHeaderCheckStateChanged_1(object sender, AfterHeaderCheckStateChangedEventArgs e)
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
               UpdateCostAndQuantity();
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
        private void ultraGrid1_BeforeHeaderCheckStateChanged_1(object sender, BeforeHeaderCheckStateChangedEventArgs e)
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
        /// ultraGrid1_AfterRowFilterChanged Event will update total quantity, total cost, adjust cost, adjust quantity text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrid1_AfterRowFilterChanged(object sender, AfterRowFilterChangedEventArgs e)
        {
            try
            {
                UpdateCostAndQuantity();
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
        /// Remove Taxlot By Group Id
        /// </summary>
        /// <param name="removeTaxlots">Taxlot List</param>
        internal void RemoveTaxlotsByGroupID(List<CostAdjustmentTaxlot> removeTaxlots)
        {
            try
            {
                foreach (CostAdjustmentTaxlot taxlot in removeTaxlots)
                {
                    List<CostAdjustmentTaxlot> removeTaxlot = _taxlotList.Where(x => x.GroupId == taxlot.GroupId).ToList();
                    removeTaxlot.ForEach(x => _taxlotList.Remove(x));
    }
                ultraGrid1.Rows.Refresh(Infragistics.Win.UltraWinGrid.RefreshRow.ReloadData);
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

        // <summary>
        /// Save the layout to xml file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                string gridLayoutFile = Application.StartupPath + "\\Prana Preferences\\" + Prana.CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + "\\CostAdjustmentLayout.xml";
                ultraGrid1.DisplayLayout.SaveAsXml(gridLayoutFile);
                MessageBox.Show(this, "Layout Saved.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
