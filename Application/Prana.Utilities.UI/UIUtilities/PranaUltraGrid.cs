using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public class PranaUltraGrid : Infragistics.Win.UltraWinGrid.UltraGrid
    {
        public void ScrollToRow(int row)
        {
            try
            {
                var r = this.Rows[row];
                this.ActiveRowScrollRegion.ScrollRowIntoView(r);
            }
            catch
            {

            }
        }

        public void ScrollToColumn(int col)
        {
            try
            {
                var c = this.Rows.Band.Columns[col];
                this.ActiveColScrollRegion.ScrollColIntoView(c);
            }
            catch
            {

            }
        }

        public void ScrollToColumnName(string columnName)
        {
            try
            {
                foreach (UltraGridColumn col in this.DisplayLayout.Bands[0].Columns)
                {
                    if (col.Header.Caption.Equals(columnName))
                    {
                        this.ActiveColScrollRegion.ScrollColIntoView(col);
                        break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetAllVisibleDataFromTheGrid()
        {
            StringBuilder dataString = new StringBuilder();
            try
            {
                List<String> columnKeys = new List<string>();
                UltraGridColumn column = this.DisplayLayout.Bands[0].Columns[0];
                column = column.GetRelatedVisibleColumn(VisibleRelation.First);
                while (null != column)
                {
                    columnKeys.Add(column.Key);
                    if (!String.IsNullOrEmpty(column.Header.Caption))
                    {
                        dataString.Append(column.Header.Caption).Append("#~#");
                    }
                    else
                    {
                        dataString.Append(column.Key).Append("#~#");
                    }
                    /// Get the next visible column by passing in VisibleRelation.Next.
                    column = column.GetRelatedVisibleColumn(VisibleRelation.Next);
                }
                dataString.Append("\n");
                foreach (UltraGridRow row in this.Rows.GetAllNonGroupByRows())
                {
                    foreach (String key in columnKeys)
                    {
                        dataString.Append(row.GetCellText(this.DisplayLayout.Bands[0].Columns[key]).Trim()).Append("#~#");
                    }

                    dataString.Append("\n");
                }
            }
            catch (Exception ex)
            {
                return dataString + " Error reading grd " + ex.Message;
            }
            return dataString.ToString();
        }
        /*public string getallcolumns()
        {
            try
            {
            StringBuilder s1 = new StringBuilder();

          //  foreach (UltraGridRow row in ultraGrid1.DisplayLayout.Rows)
          //  {
                foreach (UltraGridColumn col in this.DisplayLayout.Bands[0].Columns)
                {
                    //if(col.Hidden==false) s1.Append(ultraGrid1.Rows[0].Cells[col].Value.ToString()).Append("#~#");
                    if(!String.IsNullOrEmpty(col.Header.Caption)) s1.Append(col.Header.Caption).Append("#~#");
                    else s1.Append(col.Key).Append("#~#");
                }
                return s1.ToString();
         //   }
            }
            catch(Exception)
            {
                throw;
            }
        }*/
        public string GetAllVisibleGroupedDataFromTheGrid()
        {
            StringBuilder dataString = new StringBuilder();
            try
            {
                List<String> columnKeys = new List<string>();
                UltraGridColumn column = this.DisplayLayout.Bands[0].Columns[0];
                column = column.GetRelatedVisibleColumn(VisibleRelation.First);
                while (null != column)
                {
                    columnKeys.Add(column.Key);
                    if (!String.IsNullOrEmpty(column.Header.Caption))
                    {
                        dataString.Append(column.Header.Caption).Append("#~#");
                    }
                    else
                    {
                        dataString.Append(column.Key).Append("#~#");
                    }

                    /// Get the next visible column by passing in VisibleRelation.Next.
                    column = column.GetRelatedVisibleColumn(VisibleRelation.Next);
                }
                dataString.Append("\n");
                foreach (UltraGridRow row in this.Rows)
                {
                    if (row.ParentRow == null && row.IsGroupByRow == true)
                    {
                        foreach (String key in columnKeys)
                        {
                            if (this.DisplayLayout.Bands[0].Summaries.Exists(key))
                            {
                                UltraGridGroupByRow groupRowObject = (UltraGridGroupByRow)row;
                                dataString.Append(groupRowObject.Rows.SummaryValues[key].SummaryText.ToString()).Append("#~#");
                            }
                            else
                            {
                                dataString.Append(string.Empty).Append("#~#");
                            }
                        }
                        dataString.Append("\n");
                    }
                }
            }
            catch (Exception ex)
            {
                return dataString + " Error reading grd " + ex.Message;
            }
            return dataString.ToString();
        }

        public string GetSummaryFromGrid()
        {
            StringBuilder dataString = new StringBuilder();
            try
            {
                List<String> columnKeys = new List<string>();
                UltraGridColumn column = this.DisplayLayout.Bands[0].Columns[0];
                column = column.GetRelatedVisibleColumn(VisibleRelation.First);
                while (null != column)
                {
                    columnKeys.Add(column.Key);
                    if (!String.IsNullOrEmpty(column.Header.Caption))
                    {
                        dataString.Append(column.Header.Caption).Append("#~#");
                    }
                    else
                    {
                        dataString.Append(column.Key).Append("#~#");
                    }

                    /// Get the next visible column by passing in VisibleRelation.Next.
                    column = column.GetRelatedVisibleColumn(VisibleRelation.Next);
                }
                dataString.Append("\n");
                foreach (String key in columnKeys)
                {
                    if (this.DisplayLayout.Bands[0].Summaries.Exists(key))
                    {
                        dataString.Append(this.Rows.SummaryValues[key].SummaryText).Append("#~#");

                    }
                    else
                    {
                        dataString.Append(string.Empty).Append("#~#");
                    }
                }
                dataString.Append("\n");
            }
            catch (Exception ex)
            {
                return dataString + " Error reading grd " + ex.Message;
            }
            return dataString.ToString();
        }

        public string Description
        {
            get
            {
                StringBuilder s = new StringBuilder();
                try
                {
                    List<String> keys = new List<string>();
                    foreach (UltraGridColumn c in this.DisplayLayout.Bands[0].Columns)
                    {
                        if (c.IsVisibleInLayout)
                        {
                            s.Append(c.Header.Caption).Append("~");
                            keys.Add(c.Key);
                        }
                    }

                    s.Append("\n");
                    foreach (UltraGridRow row in this.Rows.GetAllNonGroupByRows())
                    {
                        foreach (String key in keys)
                            s.Append(row.GetCellValue(key)).Append("~");

                        s.Append("\n");
                    }

                    return s.ToString();
                }
                catch (Exception e)
                {
                    return s + " Error reading grd " + e.Message;

                }
            }
        }

        private UltraGridColumn GetColumnFromCaption(string columnName)
        {
            UltraGridColumn column = null;
            try
            {
                foreach (var gridColumn in this.DisplayLayout.Bands[0].Columns)
                {
                    if (String.IsNullOrEmpty(gridColumn.Header.Caption) && gridColumn.Key.Equals(columnName))
                    {
                        column = gridColumn;
                        break;
                    }
                    if (gridColumn.Header.Caption.Equals(columnName))
                    {
                        column = gridColumn;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return column;
        }

        public bool IsColumnPresentInGrid(string columnName)
        {
            return !GetColumnFromCaption(columnName).Hidden;
        }

        public void AddColumnsToGrid(string columnName)
        {
            try
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in this.DisplayLayout.Bands[0].Columns)
                {
                    if (column.Header.Caption.Equals(columnName))
                        column.Hidden = false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void AddColumnsToGrid(List<string> columnNames)
        {
            try
            {
                foreach (string columnName in columnNames)
                {
                    AddColumnsToGrid(columnName);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void AddFilter(string columnName, List<string> filterList)
        {
            try
            {
                if (!IsColumnPresentInGrid(columnName))
                {
                    AddColumnsToGrid(columnName);
                }
                UltraGridColumn column = GetColumnFromCaption(columnName);
                ColumnFilter columnFilter = column.Band.ColumnFilters[column];
                foreach (string filter in filterList)
                {
                    columnFilter.FilterConditions.Add(FilterComparisionOperator.Equals, filter);
                }
                columnFilter.LogicalOperator = FilterLogicalOperator.Or;
                this.OnAfterRowFilterChanged(new AfterRowFilterChangedEventArgs(column, columnFilter));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddGrouping(List<string> columnList)
        {
            try
            {
                this.DisplayLayout.ViewStyleBand = ViewStyleBand.OutlookGroupBy;
                UltraGridBand band = this.DisplayLayout.Bands[0];
                foreach (string columnName in columnList)
                {
                    if (!IsColumnPresentInGrid(columnName))
                    {
                        AddColumnsToGrid(columnName);
                    }
                    band.SortedColumns.Add(GetColumnFromCaption(columnName), false, true);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveGrouping()
        {
            try
            {
                this.DisplayLayout.Bands[0].SortedColumns.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetActiveRow()
        {
            int index = -1;
            try
            {

                if (this.ActiveRow != null && this.ActiveRow.ListObject != null)
                {
                    index = this.ActiveRow.Index;
                }

            }
            catch (Exception e)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(e, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }

            return index;
        }

        public string GetAllVisibleMultiBandDataFromTheGrid()
        {
            StringBuilder dataString = new StringBuilder();
            try
            {
                List<String> columnKeys = new List<string>();
                List<String> parentColumnKeys = new List<string>();
                for (int j = 0; j < 2; j++)
                {
                    UltraGridColumn column = this.DisplayLayout.Bands[j].Columns[0];
                    column = column.GetRelatedVisibleColumn(VisibleRelation.First);
                    while (null != column)
                    {
                        columnKeys.Add(column.Key);
                        if (j == 0)
                            parentColumnKeys.Add(column.Key);
                        if (!String.IsNullOrEmpty(column.Header.Caption) && !dataString.ToString().Contains(column.Header.Caption))
                        {
                            dataString.Append(column.Header.Caption).Append("#~#");
                        }
                        else if (!dataString.ToString().Contains(column.Header.Caption))
                        {
                            dataString.Append(column.Key).Append("#~#");
                        }
                        /// Get the next visible column by passing in VisibleRelation.Next.
                        column = column.GetRelatedVisibleColumn(VisibleRelation.Next);

                        //this delimiter is used to seperate the columns of parent row from child row
                        dataString.Append("$~$");
                    }
                    dataString.Append("\n");
                    foreach (UltraGridRow row in this.Rows.GetAllNonGroupByRows())
                    {
                        foreach (String key in columnKeys)
                        {
                            if (parentColumnKeys.Contains(key))
                                dataString.Append(row.GetCellText(this.DisplayLayout.Bands[0].Columns[key]).Trim()).Append("#~#");
                            else
                            {
                                dataString.Append(row.ChildBands[0].Rows[0].GetCellText(this.DisplayLayout.Bands[1].Columns[key]).Trim()).Append("#~#");
                            }

                        }

                        dataString.Append("\n");
                    }
                }
            }
            catch (Exception ex)
            {
                return dataString + " Error reading grd " + ex.Message;
            }
            return dataString.ToString();
        }

        /// <summary>
        /// This method launches the field chooser for specified shortcut
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Shift | Keys.L))
            {
                var form = FindForm(); // Gets the parent form hosting this grid
                form?.AddCustomColumnChooser(this);
                return true; // Mark as handled
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
