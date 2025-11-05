using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using Prana.WashSale.Classes;
using Prana.WashSale.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Prana.WashSale.Controls
{
    public partial class WashSaleTradesGridUC : UserControl
    {
        /// <summary>
        /// isSaveGriddata
        /// </summary>
        public static bool isSaveGridData = false;
        /// <summary>
        /// _washSaleDataCache
        /// </summary>
        public static BindingList<WashSaleTrades> _washSaleDataCache = new BindingList<WashSaleTrades>();
        /// <summary>
        /// Constructor
        /// </summary>
        public WashSaleTradesGridUC()
        {
            InitializeComponent();
            washSaleGrid.DataSource = _washSaleDataCache;
        }
        /// <summary>
        /// Check if there is any error on the grid
        /// </summary>
        public static Dictionary<string, bool> _gridHasError = new Dictionary<string, bool>();
        /// <summary>
        /// Intialize wash sale grid layout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WashSaleGrid_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                #region Column Formatting
                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_TAXLOT_ID))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_TAXLOT_ID].Header.VisiblePosition = 0;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_TAXLOT_ID].Width = 136;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_TAXLOT_ID].Header.Caption = WashSaleConstants.CAPS_ID;
                }
                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_TYPE_OF_TRANSACTION))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_TYPE_OF_TRANSACTION].Header.VisiblePosition = 1;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_TYPE_OF_TRANSACTION].Header.Caption = WashSaleConstants.CAPS_TYPE_OF_TRANSACTION;
                }
                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_TRADE_DATE))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_TRADE_DATE].Header.VisiblePosition = 2;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_TRADE_DATE].Header.Caption = WashSaleConstants.CAPS_TRADE_DATE;
                }
                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_ORIGINAL_PURCHASE_DATE))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_ORIGINAL_PURCHASE_DATE].Header.VisiblePosition = 3;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_ORIGINAL_PURCHASE_DATE].Header.Caption = WashSaleConstants.CAPS_ORIGINAL_PURCHASE_DATE;
                }
                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_ACCOUNT))
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_ACCOUNT].Header.VisiblePosition = 4;

                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_SIDE))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_SIDE].Header.VisiblePosition = 5;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_SIDE].Header.Caption = WashSaleConstants.CONST_SIDE;
                }
                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_ASSET))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_ASSET].Header.VisiblePosition = 6;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_ASSET].Header.Caption = WashSaleConstants.CONST_ASSET;
                }
                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_CURRENCY))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_CURRENCY].Header.VisiblePosition = 7;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_CURRENCY].Header.Caption = WashSaleConstants.CONST_CURRENCY;
                }
                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_BROKER))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_BROKER].Header.VisiblePosition = 8;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_BROKER].Header.Caption = WashSaleConstants.CONST_BROKER;
                }
                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_SYMBOL))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_SYMBOL].Header.VisiblePosition = 9;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_SYMBOL].Header.Caption = WashSaleConstants.CONST_SYMBOL;
                }
                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_BLOOMBERG_SYMBOL))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_BLOOMBERG_SYMBOL].Header.VisiblePosition = 10;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_BLOOMBERG_SYMBOL].Header.Caption = WashSaleConstants.CAPS_BLOOMBERG_SYMBOL;
                }
                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_CUSIP))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_CUSIP].Header.VisiblePosition = 11;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_CUSIP].Header.Caption = WashSaleConstants.CONST_CUSIP;
                }
                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_ISSUER))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_ISSUER].Header.VisiblePosition = 12;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_ISSUER].Header.Caption = WashSaleConstants.CONST_ISSUER;
                }
                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_UNDERLYING_SYMBOL))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_UNDERLYING_SYMBOL].Header.VisiblePosition = 13;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_UNDERLYING_SYMBOL].Header.Caption = WashSaleConstants.CAPS_UNDERLYING_SYMBOL;
                }
                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_QUANTITY))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_QUANTITY].Header.VisiblePosition = 14;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_QUANTITY].Header.Caption = WashSaleConstants.CONST_QUANTITY;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_QUANTITY].Format = WashSaleConstants.CONST_FORMAT_TWO_DECIMAL;
                }
                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_UNIT_COST_LOCAL))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_UNIT_COST_LOCAL].Header.VisiblePosition = 15;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_UNIT_COST_LOCAL].Header.Caption = WashSaleConstants.CAPS_UNIT_COST_LOCAL;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_UNIT_COST_LOCAL].Format = WashSaleConstants.CONST_FORMAT_TWO_DECIMAL;
                }
                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_TOTAL_COST_LOCAL))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_TOTAL_COST_LOCAL].Header.VisiblePosition = 16;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_TOTAL_COST_LOCAL].Header.Caption = WashSaleConstants.CAPS_TOTAL_COST_LOCAL;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_TOTAL_COST_LOCAL].Format = WashSaleConstants.CONST_FORMAT_TWO_DECIMAL;
                }
                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_TOTAL_COST))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_TOTAL_COST].Header.VisiblePosition = 17;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_TOTAL_COST].Header.Caption = WashSaleConstants.CAPS_TOTAL_COST;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_TOTAL_COST].Format = WashSaleConstants.CONST_FORMAT_TWO_DECIMAL;
                }

                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS].Header.VisiblePosition = 18;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS].Header.Caption = WashSaleConstants.CAPS_WASH_SALE_ADJUSTED_REALIZED_LOSS;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS].CellClickAction = CellClickAction.EditAndSelectText;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS].CellAppearance.BackColor = Color.DarkGray;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS].Format = WashSaleConstants.CONST_FORMAT_TWO_DECIMAL;
                }
                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD].Header.VisiblePosition = 19;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD].Header.Caption = WashSaleConstants.CAPS_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD].CellClickAction = CellClickAction.EditAndSelectText;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD].CellAppearance.BackColor = Color.DarkGray;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD].Format = WashSaleConstants.CONST_FORMAT_TWO_DECIMAL;
                }
                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_COST_BASIS))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_COST_BASIS].Header.VisiblePosition = 20;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_COST_BASIS].Header.Caption = WashSaleConstants.CAPS_WASH_SALE_ADJUSTED_COST_BASIS;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_COST_BASIS].Format = WashSaleConstants.CONST_FORMAT_TWO_DECIMAL;
                }
                if (e.Layout.Bands[0].Columns.Exists(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE))
                {
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE].Header.VisiblePosition = 21;
                    e.Layout.Bands[0].Columns[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE].Header.Caption = WashSaleConstants.CAPS_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE;
                }

                #endregion

                foreach (UltraGridColumn col in e.Layout.Bands[0].Columns)
                {
                    if (col.ToString() != WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS && col.ToString() != WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD)
                        col.CellActivation = Activation.NoEdit;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Binds data to Grid
        /// </summary>
        /// <param name="wsList"></param>
        public void BindDataToWashSaleGrid(List<WashSaleTrades> wsList)
        {
            try
            {
                if (washSaleGrid.DataSource != null)
                    _washSaleDataCache.Clear();
                for (int i = 0; i < wsList.Count; i++)
                    _washSaleDataCache.Add(wsList[i]);
                washSaleGrid.Refresh();
                SetCellFormatting();
                WashSaleGrid_SummaryRow();
                WashSaleTradesFiltersUC.IsGetDataOrUploadOrSaveClick = false;
                DisableGrid(null, null);
                if (WashSaleUploadDesign._gridColumnnError.Count > 0)
                {
                    foreach (var val in WashSaleUploadDesign._gridColumnnError)
                    {
                        washSaleGrid.Rows[val.Key - 1].DataErrorInfo.SetColumnError(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD, WashSaleConstants.CONST_WASHSALE_GRID_ERROR_INDICATOR_MESSAGE_VALUE_CANNOT_BE_GREATER_THAN_10000);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

        }
        /// <summary>
        /// Set cell value and formatting at intialization
        /// </summary>
        private void SetCellFormatting()
        {
            try
            {
                foreach (UltraGridRow row in washSaleGrid.Rows)
                {
                    #region SetCellFormatting
                    if (row.Cells.Exists(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS))
                    {
                        if (row.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS].Value != null)
                        {
                            if (row.Cells.Exists(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_COST_BASIS))
                            {
                                decimal costBasis = Convert.ToDecimal(row.Cells[WashSaleConstants.CONST_TOTAL_COST].Value) + Convert.ToDecimal(row.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS].Value);
                                row.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_COST_BASIS].Value = costBasis;
                            }
                        }
                        else
                        {
                            if (row.Cells.Exists(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_COST_BASIS))
                                row.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_COST_BASIS].Value = null;
                        }
                    }
                    if (row.Cells.Exists(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD))
                    {
                        if (row.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD].Value != null)
                        {
                            if (row.Cells.Exists(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE) && Convert.ToInt32(row.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD].Value) <= 10000)
                            {
                                row.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE].Value =
                                    Convert.ToDateTime(row.Cells[WashSaleConstants.CONST_ORIGINAL_PURCHASE_DATE].Value).AddDays(-Convert.ToInt32(row.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD].Value));
                            }
                            else
                            {
                                row.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE].Value = null;
                            }
                        }
                        else
                        {
                            if (row.Cells.Exists(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE))
                                row.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE].Value = null;
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// WashSale Grid Summary Row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WashSaleGrid_SummaryRow()
        {
            try
            {
                foreach (UltraGridColumn var in washSaleGrid.DisplayLayout.Bands[0].Columns)
                {
                    if (var.Hidden)
                        continue;
                    CreateColumnSummary(var);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Custom summary for Wash Sale Grid summary Row
        /// </summary>
        /// <param name="var"></param>
        private void CreateColumnSummary(UltraGridColumn var)
        {
            try
            {
                if (var.ExcludeFromColumnChooser == ExcludeFromColumnChooser.Default ||
                         var.ExcludeFromColumnChooser == ExcludeFromColumnChooser.False)
                {
                    if (!washSaleGrid.DisplayLayout.Bands[0].Summaries.Exists(var.Key))
                    {
                        ICustomSummaryCalculator respectiveCalculator;
                        string displayFormat;
                        switch (var.Key)
                        {
                            case WashSaleConstants.CONST_TYPE_OF_TRANSACTION:
                            case WashSaleConstants.CONST_ACCOUNT:
                            case WashSaleConstants.CONST_SIDE:
                            case WashSaleConstants.CONST_ASSET:
                            case WashSaleConstants.CONST_CURRENCY:
                            case WashSaleConstants.CONST_BROKER:
                            case WashSaleConstants.CONST_SYMBOL:
                            case WashSaleConstants.CONST_BLOOMBERG_SYMBOL:
                            case WashSaleConstants.CONST_CUSIP:
                            case WashSaleConstants.CONST_ISSUER:
                            case WashSaleConstants.CONST_UNDERLYING_SYMBOL:
                                respectiveCalculator = new WashSaleCustomSummaries.TextSummary(var.Key);
                                displayFormat = WashSaleConstants.CONST_FORMAT_TEXT;
                                break;

                            case WashSaleConstants.CONST_TRADE_DATE:
                            case WashSaleConstants.CONST_ORIGINAL_PURCHASE_DATE:
                            case WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE:
                                respectiveCalculator = new WashSaleCustomSummaries.DateSummary(var.Key);
                                displayFormat = WashSaleConstants.CONST_FORMAT_DATE;
                                break;

                            case WashSaleConstants.CONST_QUANTITY:
                                respectiveCalculator = new WashSaleCustomSummaries.PositionSummary(var.Key);
                                displayFormat = WashSaleConstants.CONST_FORMAT_TEXT;
                                break;

                            case WashSaleConstants.CONST_UNIT_COST_LOCAL:
                            case WashSaleConstants.CONST_TOTAL_COST_LOCAL:
                            case WashSaleConstants.CONST_TOTAL_COST:
                            case WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS:
                            case WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD:
                            case WashSaleConstants.CONST_WASH_SALE_ADJUSTED_COST_BASIS:
                                respectiveCalculator = new WashSaleCustomSummaries.IdenticalColumnWiseNumberSummary(var.Key);
                                displayFormat = WashSaleConstants.CONST_FORMAT_COL_DECIMAL;
                                break;

                            default:
                                respectiveCalculator = new WashSaleCustomSummaries.TextSummary(var.Key);
                                displayFormat = WashSaleConstants.CONST_FORMAT_DEFAULT;
                                break;
                        }
                        var s = washSaleGrid.DisplayLayout.Bands[0].Summaries.Add(var.Key, SummaryType.Custom, respectiveCalculator, var, SummaryPosition.UseSummaryPositionColumn, var);
                        s.DisplayFormat = displayFormat;
                        s.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// WashSaleGrid_InitializeRow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WashSaleGrid_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                IntializeRowsForColor(e.Row);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets colour, for particular cell 
        /// </summary>
        private void IntializeRowsForColor(UltraGridRow row)
        {
            try
            {
                #region SetColorforCells
                if (row.Cells.Exists(WashSaleConstants.CONST_TYPE_OF_TRANSACTION))
                {
                    row.Cells[WashSaleConstants.CONST_TYPE_OF_TRANSACTION].Appearance.ForeColor = Color.White;
                    if (row.Cells[WashSaleConstants.CONST_TYPE_OF_TRANSACTION].Value.ToString().Equals(WashSaleConstants.CONST_TAXLOT))
                        row.Cells[WashSaleConstants.CONST_TYPE_OF_TRANSACTION].Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(156)))), ((int)(((byte)(46)))));
                    else if (row.Cells[WashSaleConstants.CONST_TYPE_OF_TRANSACTION].Value.ToString().Equals(WashSaleConstants.CONST_TRADE))
                        row.Cells[WashSaleConstants.CONST_TYPE_OF_TRANSACTION].Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(99)))), ((int)(((byte)(160)))));
                }

                #endregion
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// clears grid Data
        /// </summary>
        internal void ClearGridData()
        {
            try
            {
                _washSaleDataCache.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Sets the theme for user control.
        /// </summary>
        internal void SetThemeForUserControl()
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(washSaleGrid, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_WATCHLIST);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Detects Whether any Change in the data grid happens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridDataChanged(object sender, CellEventArgs e)
        {
            try
            {
                double res;
                isSaveGridData = true;
                string col = e.Cell.Column.Key;
                string text = e.Cell.Text;
                if (!double.TryParse(text, out res) || text.Contains(WashSaleConstants.CONST_BLANK_SPACE) || (col.ToString().Equals(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD) && text.Contains(".")))
                {
                    if (text.Length == 1 && text.Equals(WashSaleConstants.CONST_BLANK_SPACE))
                    {
                        _gridHasError[col + e.Cell.Row.Index] = false;
                        washSaleGrid.ActiveRow.DataErrorInfo.SetColumnError(col, WashSaleConstants.CONST_BLANK);
                        e.Cell.Row.Cells[col].Value = WashSaleConstants.CONST_BLANK;
                        return;
                    }
                    int countSpaces = text.Count(Char.IsWhiteSpace);
                    if (!string.IsNullOrEmpty(text))
                    {
                        if ((text[text.Length - 1] < '0' || text[text.Length - 1] > '9') && countSpaces == 1 && char.IsWhiteSpace(text[text.Length - 1]))
                        {
                            text = text.Substring(0, text.Length - 1);
                            e.Cell.Row.Cells[col].Value = text;
                            e.Cell.Row.Cells[col].SelStart = text.Length;
                            foreach (char c in text)
                            {
                                if (c < '0' || c > '9')
                                {
                                    _gridHasError[col + e.Cell.Row.Index] = true;
                                    if (col.ToString().Equals(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS))
                                        washSaleGrid.ActiveRow.DataErrorInfo.SetColumnError(col, WashSaleConstants.CONST_WASHSALE_GRID_ERROR_INDICATOR_MESSAGE);
                                    else washSaleGrid.ActiveRow.DataErrorInfo.SetColumnError(col, WashSaleConstants.CONST_WASHSALE_GRID_ERROR_INDICATOR_MESSAGE_HOLDING_DATE);
                                    return;
                                }
                            }
                            washSaleGrid.ActiveRow.DataErrorInfo.SetColumnError(col, WashSaleConstants.CONST_BLANK); ;
                            return;
                        }
                    }
                    if (col.ToString().Equals(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS))
                    {
                        decimal textValue = 0;
                        if (text.Length > 1)
                        {
                            if (!decimal.TryParse(text.ToString(), out textValue))
                            {
                                washSaleGrid.ActiveRow.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS].Value = null;
                                return;
                            }
                        }
                        if (text.Length == 1)
                        {
                            if (!decimal.TryParse(text.ToString(), out textValue) && !text.Equals("."))
                            {
                                washSaleGrid.ActiveRow.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS].Value = null;
                                return;
                            }
                        }

                    }
                    if (col.ToString().Equals(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD))
                    {
                        decimal textValue = 0;
                        if (text.Length > 1)
                        {
                            if (!decimal.TryParse(text.ToString(), out textValue))
                            {
                                washSaleGrid.ActiveRow.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD].Value = null;
                                return;
                            }
                        }
                        if (text.Length == 1)
                        {
                            if (!decimal.TryParse(text.ToString(), out textValue) && !text.Equals("."))
                            {
                                washSaleGrid.ActiveRow.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD].Value = null;
                                return;
                            }
                        }
                    }
                    foreach (char c in text)
                    {
                        if (c < '0' || c > '9')
                        {
                            _gridHasError[col + e.Cell.Row.Index] = true;
                            if (col.ToString().Equals(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS))
                                washSaleGrid.ActiveRow.DataErrorInfo.SetColumnError(col, WashSaleConstants.CONST_WASHSALE_GRID_ERROR_INDICATOR_MESSAGE);
                            else washSaleGrid.ActiveRow.DataErrorInfo.SetColumnError(col, WashSaleConstants.CONST_WASHSALE_GRID_ERROR_INDICATOR_MESSAGE_HOLDING_DATE);
                            return;
                        }
                    }
                }
                if (col.ToString().Equals(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD) && res > 10000)
                {
                    washSaleGrid.ActiveRow.DataErrorInfo.SetColumnError(col, WashSaleConstants.CONST_WASHSALE_GRID_ERROR_INDICATOR_MESSAGE_VALUE_CANNOT_BE_GREATER_THAN_10000);
                    _gridHasError[col + e.Cell.Row.Index] = true;
                    return;
                }
                _gridHasError[col + e.Cell.Row.Index] = false;
                washSaleGrid.ActiveRow.DataErrorInfo.SetColumnError(col, WashSaleConstants.CONST_BLANK);
                if (!IsGridContainsError())
                {
                    WashSale.SetStatusBarText(WashSaleConstants.CONST_BLANK);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Handles the AfterExitEditMode event of the washSaleGrid cells.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void washSaleGrid_AfterExitEditMode(object sender, System.EventArgs e)
        {
            try
            {
                if (washSaleGrid.ActiveRow != null && washSaleGrid.ActiveCell != null && !string.IsNullOrEmpty(Convert.ToString(washSaleGrid.ActiveCell.Value)))
                {
                    double numericValue;
                    string activeCellValue = string.Empty;
                    if (washSaleGrid.ActiveCell.Value != null)
                        activeCellValue = washSaleGrid.ActiveCell.Value.ToString();
                    bool isNumeric = double.TryParse(activeCellValue, out numericValue);
                    if (isNumeric)
                    {
                        switch (washSaleGrid.ActiveCell.Column.Key)
                        {
                            case WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS:
                                if (!string.IsNullOrEmpty(washSaleGrid.ActiveCell.Value.ToString()))
                                {
                                    decimal costBasis = Convert.ToDecimal(washSaleGrid.ActiveCell.Value) + Convert.ToDecimal(washSaleGrid.ActiveRow.Cells[WashSaleConstants.CONST_TOTAL_COST].Value);
                                    string realizedLossStr = string.Format(WashSaleConstants.CONST_FORMAT_TEXT_NEW, Convert.ToDecimal(washSaleGrid.ActiveCell.Value));
                                    string costBasisStr = string.Format(WashSaleConstants.CONST_FORMAT_TEXT_NEW, costBasis);
                                    washSaleGrid.ActiveRow.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_COST_BASIS].SetValue(costBasis, true);
                                    washSaleGrid.ActiveRow.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS].SetValue(Convert.ToDecimal(washSaleGrid.ActiveCell.Value), true);
                                }
                                break;
                            case WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD:
                                if (!string.IsNullOrEmpty(washSaleGrid.ActiveCell.Value.ToString()))
                                {
                                    int holdingPeriod;
                                    bool isWholeNumber = int.TryParse(washSaleGrid.ActiveCell.Value.ToString(), out holdingPeriod);
                                    if (isWholeNumber && holdingPeriod <= 10000)
                                    {

                                        DateTime purchaseDate = Convert.ToDateTime(washSaleGrid.ActiveRow.Cells[WashSaleConstants.CONST_ORIGINAL_PURCHASE_DATE].Value);
                                        DateTime washSaleHoldingStartDate = Convert.ToDateTime(purchaseDate.AddDays(-holdingPeriod));
                                        string holdingPeriodStr = string.Format(WashSaleConstants.CONST_FORMAT_TEXT, holdingPeriod);
                                        string washSaleHoldingStartDateStr = string.Format(WashSaleConstants.CONST_FORMAT_DATE, washSaleHoldingStartDate.Date);
                                        washSaleGrid.ActiveRow.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD].SetValue(holdingPeriod, true);
                                        washSaleGrid.ActiveRow.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE].SetValue(washSaleHoldingStartDate, true);
                                    }
                                    else
                                    {
                                        washSaleGrid.ActiveRow.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE].SetValue(null, true);
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (washSaleGrid.ActiveCell.Column.Key)
                        {
                            case WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS:
                                washSaleGrid.ActiveRow.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_COST_BASIS].SetValue(null, true);
                                break;

                            case WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD:
                                washSaleGrid.ActiveRow.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE].SetValue(null, true);
                                break;
                        }
                    }
                }
                else if (washSaleGrid.ActiveRow != null && washSaleGrid.ActiveCell != null && washSaleGrid.ActiveCell.Value == null)
                {
                    switch (washSaleGrid.ActiveCell.Column.Key)
                    {
                        case WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS:
                            washSaleGrid.ActiveRow.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS].SetValue(null, true);
                            washSaleGrid.ActiveRow.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_COST_BASIS].SetValue(null, true);
                            break;

                        case WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD:
                            washSaleGrid.ActiveRow.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD].SetValue(null, true);
                            washSaleGrid.ActiveRow.Cells[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE].SetValue(null, true);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Discard all the unsaved changes
        /// </summary>
        public static void DiscardChanges()
        {
            try
            {
                _washSaleDataCache.Clear();
                isSaveGridData = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Check if any error on the grid
        /// </summary>
        /// <returns></returns>
        public static bool IsGridContainsError()
        {
            try
            {
                foreach (var item in _gridHasError)
                {
                    if (item.Value == true)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return false;
        }
        /// <summary>
        /// Check Export To Excel
        /// </summary>
        /// <returns></returns>
        public void ExportToExcel()
        {
            try
            {
                if (washSaleGrid.Rows.Count > 0)
                {
                    WashSale.SetStatusBarText(WashSaleConstants.CONST_WASHSALE_EXPORTING_DATA);
                    Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook();
                    string pathName = null;
                    saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.InitialDirectory = Application.StartupPath;
                    saveFileDialog1.Filter = "Excel WorkBook Files (*.xls)|*.xls|All Files (*.*)|*.*";
                    saveFileDialog1.RestoreDirectory = true;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        pathName = saveFileDialog1.FileName;
                    }
                    else
                    {
                        WashSale.SetStatusBarText(WashSaleConstants.CONST_EXPORT_CANCEL);
                        return;
                    }
                    if (!string.IsNullOrEmpty(pathName))
                    {
                        UpdateWashSaleGridData();
                        string workbookName = "Report" + DateTime.Now.Date.ToString("yyyyMMdd");
                        workBook.Worksheets.Add(workbookName);
                        workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[workbookName];
                        washSaleGrid.DisplayLayout.Bands[0].Summaries.Clear();
                        workBook = this.ultraGridExcelExporter1.Export(this.washSaleGrid, workBook.Worksheets[workbookName]);
                        workBook.Worksheets[0].Columns[14].CellFormat.FormatString = WashSaleConstants.CONST_FORMAT_WASHSALE_EXPORT;
                        workBook.Worksheets[0].Columns[15].CellFormat.FormatString = WashSaleConstants.CONST_FORMAT_WASHSALE_EXPORT;
                        workBook.Worksheets[0].Columns[16].CellFormat.FormatString = WashSaleConstants.CONST_FORMAT_WASHSALE_EXPORT;
                        workBook.Worksheets[0].Columns[17].CellFormat.FormatString = WashSaleConstants.CONST_FORMAT_WASHSALE_EXPORT;
                        workBook.Worksheets[0].Columns[18].CellFormat.FormatString = WashSaleConstants.CONST_FORMAT_WASHSALE_EXPORT;
                        workBook.Worksheets[0].Columns[20].CellFormat.FormatString = WashSaleConstants.CONST_FORMAT_WASHSALE_EXPORT;
                        foreach (UltraGridColumn var in washSaleGrid.DisplayLayout.Bands[0].Columns)
                        {
                            if (var.Hidden)
                                continue;
                            CreateColumnSummary(var);
                        }
                        workBook.Save(pathName);
                    }
                    WashSale.SetStatusBarText(WashSaleConstants.CONST_WASHSALE_EXPORT_DATA);
                }
                else
                    WashSale.SetStatusBarText(WashSaleConstants.CONST_WASHSALE_BLANK_EXPORT_DATA);
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
        /// Update the Binding list of the Wash Sale Grid
        /// </summary>
        public void UpdateWashSaleGridData()
        {
            try
            {
                washSaleGrid.UpdateData();
                washSaleGrid_AfterExitEditMode(null, null);
                washSaleGrid.UpdateData();
                _washSaleDataCache = (BindingList<WashSaleTrades>)washSaleGrid.DataSource;
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
        /// Disable the grid when data already present and user perform Get data , Upload and Save Operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DisableGrid(object sender, ClickCellEventArgs e)
        {
            try
            {
                if (WashSaleTradesFiltersUC.IsGetDataOrUploadOrSaveClick)
                {
                    washSaleGrid.Enabled = false;
                    washSaleGrid.DisplayLayout.Bands[0].SummaryFooterCaption = WashSaleConstants.CONST_BLANK;
                }
                else
                {
                    washSaleGrid.Enabled = true;
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
        /// Key Press event of WashSaleGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WashSaleGrid_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                int colNum = int.MinValue;
                bool isNumerical = false;
                if (washSaleGrid.ActiveCell != null && washSaleGrid.ActiveCell.Column != null)
                {
                    if (washSaleGrid.ActiveCell.Column.Key.Equals(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS))
                    {
                        isNumerical = int.TryParse(e.KeyChar.ToString(), out colNum);
                        if (!isNumerical && !e.KeyChar.ToString().Equals(".") && !e.KeyChar.ToString().Equals(WashSaleConstants.CONST_BACKSPACE)
                            && !e.KeyChar.ToString().Equals(WashSaleConstants.CONST_CUT_KEYBOARD_SHORTCUT)
                            && !e.KeyChar.ToString().Equals(WashSaleConstants.CONST_COPY_KEYBOARD_SHORTCUT)
                            && !e.KeyChar.ToString().Equals(WashSaleConstants.CONST_PASTE_KEYBOARD_SHORCUT))
                        {
                            e.Handled = true;
                        }
                    }
                    else if (washSaleGrid.ActiveCell.Column.Key.Equals(WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD))
                    {
                        isNumerical = int.TryParse(e.KeyChar.ToString(), out colNum);
                        if (!isNumerical && !e.KeyChar.ToString().Equals(WashSaleConstants.CONST_BACKSPACE)
                            && !e.KeyChar.ToString().Equals(WashSaleConstants.CONST_CUT_KEYBOARD_SHORTCUT)
                            && !e.KeyChar.ToString().Equals(WashSaleConstants.CONST_COPY_KEYBOARD_SHORTCUT)
                            && !e.KeyChar.ToString().Equals(WashSaleConstants.CONST_PASTE_KEYBOARD_SHORCUT))
                        {
                            e.Handled = true;
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
    }
}
