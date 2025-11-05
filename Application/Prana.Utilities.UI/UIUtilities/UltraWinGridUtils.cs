using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Prana.Utilities.UI.UIUtilities
{
    public class UltraWinGridUtils
    {


        /// <summary>
        /// Adds the CheckBox.
        /// </summary>
        /// <param name="gridName">Name of the grid.</param>
        public static void AddCheckBox(Infragistics.Win.UltraWinGrid.UltraGrid gridName)
        {
            try
            {
                if (gridName.DisplayLayout.Bands[0].Columns.Exists("Checkbox"))
                    return;

                UltraGridColumn checkColumn = gridName.DisplayLayout.Bands[0].Columns.Add("Checkbox", String.Empty);
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

                gridName.DisplayLayout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
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
        /// Hides All Columns of Grid in  Bands 1 and 2
        /// </summary>
        public static void HideColumnsOfAllBands(UltraGrid grid)
        {
            try
            {
                int count = 1;
                foreach (UltraGridBand band in grid.DisplayLayout.Bands)
                {
                    ColumnsCollection columnCollection1 = band.Columns;
                    foreach (UltraGridColumn column in columnCollection1)
                    {
                        column.Hidden = true;
                    }
                    if (count > 1)
                    {
                        band.ColHeadersVisible = false;
                    }
                    count++;
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
        /// <summary>
        /// It displays all columns specified in currentColumnList and hides all others in All Bands
        /// </summary>
        /// <param name="currentColumnList"></param>
        public static void DisplayColumns(List<string> displayColumnList, UltraGrid grid, bool dropDownAllowed)
        {
            try
            {
                HideColumnsOfAllBands(grid);
                foreach (UltraGridBand band in grid.DisplayLayout.Bands)
                {
                    BandSetting(displayColumnList, band, dropDownAllowed);
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
        private static void BandSetting(List<string> displayColumnList, UltraGridBand band, bool dropDownAllowed)
        {
            try
            {
                int i = 0;

                foreach (string columnCaption in displayColumnList)
                {
                    if (OrderFields.ColumnNameHeaderCollection.ContainsKey(columnCaption))
                    {
                        string columnName = OrderFields.ColumnNameHeaderCollection[columnCaption];
                        if (band.Index == 0 && dropDownAllowed)
                        {
                            if (columnCaption == OrderFields.CAPTION_ORDER_SIDE)
                            {
                                columnName = OrderFields.PROPERTY_ORDER_SIDETAGVALUE;
                            }
                            else if (columnCaption == OrderFields.CAPTION_ORDER_TYPE)
                            {

                                columnName = OrderFields.PROPERTY_ORDER_TYPETAGVALUE;
                            }
                            else if (columnCaption == OrderFields.CAPTION_COUNTERPARTY_NAME)
                            {
                                columnName = OrderFields.CAPTION_COUNTERPARTY_ID;
                            }
                            else if (columnCaption == OrderFields.CAPTION_VENUE)
                            {
                                columnName = OrderFields.CAPTION_VENUE_ID;
                            }
                            else if (columnCaption == OrderFields.CAPTION_LEVEL2NAME)
                            {
                                columnName = OrderFields.CAPTION_LEVEL2ID;
                            }
                            else if (columnCaption == OrderFields.CAPTION_LEVEL1NAME)
                            {
                                columnName = OrderFields.CAPTION_LEVEL1ID;
                            }
                            else if (columnCaption == OrderFields.CAPTION_HANDLING_INST)
                            {
                                columnName = OrderFields.PROPERTY_HANDLING_INST_TagValue;
                            }
                            else if (columnCaption == OrderFields.CAPTION_EXECUTION_INST)
                            {
                                columnName = OrderFields.PROPERTY_EXECUTION_INST_TagValue;
                            }
                            else if (columnCaption == OrderFields.CAPTION_TIF)
                            {
                                columnName = OrderFields.PROPERTY_TIF_TAGVALUE;
                            }
                        }

                        if (!band.Columns.Exists(columnName))
                        {
                            band.Columns.Add(columnName, columnCaption);
                        }
                        UltraGridColumn column = band.Columns[columnName];
                        column.Hidden = false;
                        column.Header.VisiblePosition = i;
                        column.Width = 50;
                        column.Header.Caption = columnCaption;
                        i++;
                    }

                    if (dropDownAllowed)
                    {
                        if (!band.Columns.Exists(OrderFields.PROPERTY_CHKBOX))
                        {
                            band.Columns.Add(OrderFields.PROPERTY_CHKBOX, string.Empty);
                            band.Columns[OrderFields.PROPERTY_CHKBOX].DataType = typeof(bool);
                        }
                        band.Columns[OrderFields.PROPERTY_CHKBOX].Hidden = false;
                        band.Columns[OrderFields.PROPERTY_CHKBOX].Width = 8;
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

        public static void DisplayColumns(List<List<string>> bandsColumns, UltraGrid grid)
        {
            try
            {
                int i = 0;

                foreach (UltraGridBand band in grid.DisplayLayout.Bands)
                {
                    if (i < bandsColumns.Count)
                    {
                        HideColumns(band);
                        SetBand(bandsColumns[i], band);
                        i++;
                    }
                    else
                    {
                        break;
                    }
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
        public static void DisplayColumns(List<List<string>> bandsColumns, UltraGrid grid, List<string> columnWidth)
        {
            try
            {
                int i = 0;

                foreach (UltraGridBand band in grid.DisplayLayout.Bands)
                {
                    if (i < bandsColumns.Count)
                    {
                        HideColumns(band);
                        SetBand(bandsColumns[i], band, columnWidth);
                        i++;
                    }
                    else
                    {
                        break;
                    }
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
        public static void HideColumns(UltraGridBand band)
        {
            try
            {
                ColumnsCollection columnCollection = band.Columns;
                foreach (UltraGridColumn column in columnCollection)
                {
                    column.Hidden = true;
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
        public static void SetBand(List<string> displayColumnList, UltraGridBand band)
        {
            try
            {
                int i = 0;
                foreach (string columnCaption in displayColumnList)
                {
                    string columnName = string.Empty;
                    if (OrderFields.ColumnNameHeaderCollection.ContainsKey(columnCaption))
                    {
                        columnName = OrderFields.ColumnNameHeaderCollection[columnCaption];
                    }
                    else
                    {
                        columnName = columnCaption;
                    }
                    if (!band.Columns.Exists(columnName))
                    {
                        band.Columns.Add(columnName, columnCaption);
                    }
                    UltraGridColumn column = band.Columns[columnName];
                    column.Hidden = false;
                    column.Header.VisiblePosition = i;
                    column.Width = 50;
                    column.Header.Caption = columnCaption;
                    i++;
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
        public static void SetBand(List<string> displayColumnList, UltraGridBand band, List<string> columnWidths)
        {
            try
            {
                int i = 0;
                foreach (string columnCaption in displayColumnList)
                {
                    string columnName = string.Empty;
                    if (OrderFields.ColumnNameHeaderCollection.ContainsKey(columnCaption))
                    {
                        columnName = OrderFields.ColumnNameHeaderCollection[columnCaption];
                    }
                    else
                    {
                        columnName = columnCaption;
                    }
                    if (!band.Columns.Exists(columnName))
                    {
                        band.Columns.Add(columnName, columnCaption);
                    }
                    UltraGridColumn column = band.Columns[columnName];
                    column.Hidden = false;
                    column.Header.VisiblePosition = i;
                    if (columnWidths.Count == displayColumnList.Count)
                    {
                        column.Width = Convert.ToInt32(columnWidths[i]);
                    }
                    else
                    {
                        column.Width = 50;
                    }
                    column.Header.Caption = columnCaption;
                    i++;
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
        public static string GetColumnsString(UltraGrid grid)
        {
            string colString = String.Empty;
            try
            {
                SortedList<int, string> sortedColList = new SortedList<int, string>();

                foreach (UltraGridColumn col in grid.DisplayLayout.Bands[0].Columns)
                {
                    if (col.Hidden == false)
                    {
                        sortedColList.Add(col.Header.VisiblePosition, col.Key);
                    }
                }
                foreach (KeyValuePair<int, string> item in sortedColList)
                {
                    colString += item.Value + ",";
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
            return colString;
        }
        public static void SetColumns(List<string> columnList, UltraGrid grd)
        {
            try
            {
                Infragistics.Win.UltraWinGrid.ColumnsCollection columns = grd.DisplayLayout.Bands[0].Columns;
                if (columnList != null)
                {
                    //Hide all columns
                    foreach (UltraGridColumn col in columns)
                    {
                        columns[col.Key].Hidden = true;
                    }

                    //Unhide and Set postions for required columns
                    int visiblePosition = 1;
                    foreach (string col in columnList)
                    {
                        if (columns.Exists(col))
                        {
                            UltraGridColumn column = columns[col];
                            column.Hidden = false;
                            column.Header.VisiblePosition = visiblePosition;
                            column.Width = 80;
                            visiblePosition++;
                        }
                    }
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

        /// <summary>
        /// Enables the fixed filter row as the first row on the grid.
        /// Taken from Infragistics Fixed filter row sample
        /// </summary>
        /// <param name="e"></param>
        public static void EnableFixedFilterRow(InitializeLayoutEventArgs e)
        {
            try
            {
                #region Fixed Filter Row

                // FILTER ROW FUNCTIONALITY RELATED ULTRAGRID SETTINGS
                // ----------------------------------------------------------------------------------
                // Enable the the filter row user interface by setting the FilterUIType to FilterRow.
                e.Layout.Override.FilterUIType = FilterUIType.FilterRow;

                // FilterEvaluationTrigger specifies when UltraGrid applies the filter criteria typed 
                // into a filter row. Default is OnCellValueChange which will cause the UltraGrid to
                // re-filter the data as soon as the user modifies the value of a filter cell.
                e.Layout.Override.FilterEvaluationTrigger = FilterEvaluationTrigger.OnCellValueChange;

                // By default the UltraGrid selects the type of the filter operand editor based on
                // the column's DataType. For DateTime and Boolean columns it uses the column's editors.
                // For other column types it uses the Combo. You can explicitly specify the operand
                // editor style by setting the FilterOperandStyle on the override or the individual
                // columns.
                //e.Layout.Override.FilterOperandStyle = FilterOperandStyle.Combo;

                // By default UltraGrid displays user interface for selecting the filter operator. 
                // You can set the FilterOperatorLocation to hide this user interface. This
                // property is available on column as well so it can be controlled on a per column
                // basis. Default is WithOperand. This property is exposed off the column as well.
                e.Layout.Override.FilterOperatorLocation = FilterOperatorLocation.WithOperand;

                // By default the UltraGrid uses StartsWith as the filter operator. You use
                // the FilterOperatorDefaultValue property to specify a different filter operator
                // to use. This is the default or the initial filter operator value of the cells
                // in filter row. If filter operator user interface is enabled (FilterOperatorLocation
                // is not set to None) then that ui will be initialized to the value of this
                // property. The user can then change the operator as he/she chooses via the operator
                // drop down.
                e.Layout.Override.FilterOperatorDefaultValue = FilterOperatorDefaultValue.StartsWith;

                // FilterOperatorDropDownItems property can be used to control the options provided
                // to the user for selecting the filter operator. By default UltraGrid bases 
                // what operator options to provide on the column's data type. This property is
                // available on the column as well.
                //e.Layout.Override.FilterOperatorDropDownItems = FilterOperatorDropDownItems.All;

                // By default UltraGrid displays a clear button in each cell of the filter row
                // as well as in the row selector of the filter row. When the user clicks this
                // button the associated filter criteria is cleared. You can use the 
                // FilterClearButtonLocation property to control if and where the filter clear
                // buttons are displayed.
                e.Layout.Override.FilterClearButtonLocation = FilterClearButtonLocation.RowAndCell;

                // Appearance of the filter row can be controlled using the FilterRowAppearance property.
                e.Layout.Override.FilterRowAppearance.BackColor = Color.LightYellow;
                e.Layout.Override.FilterRowAppearance.ForeColor = Color.Black;

                // You can use the FilterRowPrompt to display a prompt in the filter row. By default
                // UltraGrid does not display any prompt in the filter row.
                e.Layout.Override.FilterRowPrompt = "Click here to filter data...";

                // You can use the FilterRowPromptAppearance to change the appearance of the prompt.
                // By default the prompt is transparent and uses the same fore color as the filter row.
                // You can make it non-transparent by setting the appearance' BackColorAlpha property 
                // or by setting the BackColor to a desired value.
                e.Layout.Override.FilterRowPromptAppearance.BackColorAlpha = Alpha.Opaque;

                // By default the prompt is spread across multiple cells if it's bigger than the
                // first cell. You can confine the prompt to a particular cell by setting the
                // SpecialRowPromptField property off the band to the key of a column.
                //e.Layout.Bands[0].SpecialRowPromptField = e.Layout.Bands[0].Columns[0].Key;

                // Display a separator between the filter row other rows. SpecialRowSeparator property 
                // can be used to display separators between various 'special' rows, including for the
                // filter row. This property is a flagged enum property so it can take multiple values.
                e.Layout.Override.SpecialRowSeparator = SpecialRowSeparator.FilterRow;

                // You can control the appearance of the separator using the SpecialRowSeparatorAppearance
                // property.
                e.Layout.Override.SpecialRowSeparatorAppearance.BackColor = Color.FromArgb(233, 242, 199);

                #endregion
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

        /// <summary>
        /// Disable Sorting on grid
        /// </summary>
        /// <param name="e"></param>
        public static void DisableSortingOnGrid(InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.Override.HeaderClickAction = HeaderClickAction.Select;
                e.Layout.Override.SelectTypeCol = SelectType.None;
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

        /// <summary>
        /// check if groupping is applied on ultragrid
        /// </summary>
        /// <returns></returns>
        public static bool IsGrouppingAppliedOnGrid(UltraGrid grid)
        {
            try
            {
                if (grid != null && grid.DisplayLayout != null
                    && grid.DisplayLayout.Bands != null && grid.DisplayLayout.Bands.Count > 0)
                {
                    foreach (UltraGridBand band in grid.DisplayLayout.Bands)
                    {
                        if (band != null && band.SortedColumns != null && band.SortedColumns.Count > 0)
                        {
                            foreach (UltraGridColumn col in band.SortedColumns)
                            {
                                if (col.IsGroupByColumn)
                                {
                                    return true;
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
            return false;
        }

        /// <summary>
        /// setting columns for column chooser
        /// </summary>
        /// <param name="grid">The ultragrid</param>
        /// <param name="list">List of columns</param>
        public static void SetColumnsForColumnChooser(UltraGrid grid, List<string> list)
        {
            try
            {
                foreach (UltraGridColumn column in grid.DisplayLayout.Bands[0].Columns)
                {
                    column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    //column.Hidden = true;
                }
                foreach (string column in list)
                {
                    if (grid.DisplayLayout.Bands[0].Columns.Exists(column))
                    {
                        grid.DisplayLayout.Bands[0].Columns[column].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                        //grid.DisplayLayout.Bands[0].Columns[column].Hidden = false;
                    }
                }
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
        public static string GetColumnsWidthString(UltraGrid grid)
        {
            string colWidthString = String.Empty;
            try
            {
                SortedList<int, string> sortedColWidthList = new SortedList<int, string>();

                foreach (UltraGridColumn col in grid.DisplayLayout.Bands[0].Columns)
                {
                    if (col.Hidden == false)
                    {
                        sortedColWidthList.Add(col.Header.VisiblePosition, col.Width.ToString());
                    }
                }
                foreach (KeyValuePair<int, string> item in sortedColWidthList)
                {
                    colWidthString += item.Value + ",";
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
            return colWidthString;
        }

        /// <summary>
        /// Select row on right mouse button click
        /// </summary>
        /// <param name="ultraGrid"></param>
        /// <param name="point"></param>
        public static void RightClickRowSelect(UltraGrid ultraGrid, Point point)
        {
            try
            {
                UIElement element = ultraGrid.DisplayLayout.UIElement.ElementFromPoint(point);
                var row = element.GetContext(typeof(UltraGridRow)) as UltraGridRow;
                if (row != null && row.IsDataRow)
                {
                    ultraGrid.ActiveRow = row;
                    if (!row.Selected)
                    {
                        ultraGrid.Selected.Rows.Clear();
                        row.Selected = true;
                    }
                }
                else
                {
                    ultraGrid.Selected.Rows.Clear();
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
        /// <summary>
        /// Clear All Grid Filters
        /// Added By Sachin mishra 15/06/15
        /// </summary>
        /// <param name="Grid"></param>
        public static void ClearAllGridFilters(UltraGrid Grid)
        {
            try
            {
                if (Grid != null && Grid.DisplayLayout != null
                   && Grid.DisplayLayout.Bands != null && Grid.DisplayLayout.Bands.Count > 0)
                {
                    foreach (UltraGridBand band in Grid.DisplayLayout.Bands)
                    {
                        band.ColumnFilters.ClearAllFilters();
                    }
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

        /// <summary>
        /// Set columns visibility
        /// </summary>
        /// <param name="grid">The ultragrid</param>
        /// <param name="list">List of visible columns</param>
        public static void SetColumnsVisibility(UltraGrid grid, List<string> list)
        {
            try
            {
                foreach (UltraGridColumn column in grid.DisplayLayout.Bands[0].Columns)
                {
                    column.Hidden = true;
                }
                foreach (string column in list)
                {
                    if (grid.DisplayLayout.Bands[0].Columns.Exists(column))
                    {
                        grid.DisplayLayout.Bands[0].Columns[column].Hidden = false;
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
    }
}
