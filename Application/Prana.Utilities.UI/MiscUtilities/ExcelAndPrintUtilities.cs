using Infragistics.Documents.Excel;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prana.Utilities.UI.MiscUtilities
{
    public class ExcelAndPrintUtilities : Form
    {
        private UltraGridExcelExporter ExcelExporter;
        private SaveFileDialog saveFileDialog1;

        // To Export list of grids in one excel worksheet
        public void ExportToExcel(List<UltraGrid> lstGrids, string workSheetName, bool concatColumnWise)
        {
            try
            {
                string pathName = null;
                saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
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

                ExcelExporter = new UltraGridExcelExporter();
                Workbook workBook = new Workbook();
                workSheetName = workSheetName.Trim();
                workBook.Worksheets.Add(workSheetName);
                workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[workSheetName];

                if (concatColumnWise)
                {
                    int nextColumn = 0;
                    foreach (UltraGrid grd in lstGrids)
                    {
                        this.ExcelExporter.Export(grd, workBook, 0, nextColumn);
                        nextColumn += CountVisibleColumns(grd);
                    }
                }
                else
                {
                    int nextRow = 0;
                    foreach (UltraGrid grd in lstGrids)
                    {
                        this.ExcelExporter.Export(grd, workBook, nextRow, 0);
                        nextRow += (CountVisibleRows(grd) + 1);
                    }
                }
                workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[0];
                workBook.Save(pathName);
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

        private VisiblePosComparer comparer = new VisiblePosComparer();

        // To Export list of grids in one excel worksheet in CSV format.
        // It will export grouped data without Headers (only row description specifically for risk),with headers and also ungroupedData.
        public void SetExcelLayoutAndWrite(List<UltraGrid> grids, bool isGroupedWithHeaders, string fullPath)
        {
            string pathName = null;
            if (string.IsNullOrEmpty(fullPath))
            {
                saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                saveFileDialog1.Filter = "XLS|*.xls";
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pathName = saveFileDialog1.FileName;
                }
                else
                {
                    return;
                }
            }
            else
            {
                pathName = fullPath;
            }
            WriteGridToExcelFile(grids, isGroupedWithHeaders, pathName);
        }

        public void SetinCSVFormat(List<UltraGrid> grids, bool isGroupedWithHeaders, string fullPath)
        {
            try
            {
                string pathName = null;
                if (string.IsNullOrEmpty(fullPath))
                {
                    saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.InitialDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    saveFileDialog1.Filter = "CSV|*.csv";
                    saveFileDialog1.RestoreDirectory = true;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        pathName = saveFileDialog1.FileName;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    pathName = fullPath;
                }
                WriteGridToExcelFile(grids, isGroupedWithHeaders, pathName, true);
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

        // To Export list of grids in one excel worksheet in CSV format.
        // It will export grouped data without Headers (only row description specifically for risk),with headers and also ungroupedData.It will also export with filename already given
        public void SetExcelLayoutAndWriteForRisk(List<UltraGrid> grids, bool isGroupedWithHeaders, string filename)
        {
            string pathName = null;
            saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            saveFileDialog1.Filter = "XLS|*.xls";
            saveFileDialog1.RestoreDirectory = true;
            if (!String.IsNullOrEmpty(filename))
            {
                saveFileDialog1.FileName = filename;
            }
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pathName = saveFileDialog1.FileName;
            }
            else
            {
                return;
            }
            if (!String.IsNullOrEmpty(pathName))
            {
                WriteGridToExcelFile(grids, isGroupedWithHeaders, pathName);
            }
        }

        private void WriteGridToExcelFile(List<UltraGrid> grids, bool isGroupedWithHeaders, string pathName, bool IsNewLineNeeded = false)
        {
            StreamWriter sw = File.CreateText(pathName);
            try
            {
                if (!String.IsNullOrEmpty(pathName))
                {
                    StringBuilder s = new StringBuilder();

                    foreach (UltraGrid grid in grids)
                    {
                        string groupByColCaption = string.Empty;

                        UltraGridBand band = grid.DisplayLayout.Bands[0];

                        if (band.SortedColumns != null && band.SortedColumns.Count > 0)
                        {
                            foreach (UltraGridColumn col in band.SortedColumns)
                            {
                                if (col.IsGroupByColumn)
                                {
                                    groupByColCaption = band.SortedColumns[0].Header.Caption;
                                    break;
                                }
                            }
                        }

                        //TODO: check the code for not writing while building the string, and write only in the end.
                        // sw.WriteLine(s.ToString().TrimEnd(','));
                        //this is done as length = 0 will initialize the string pointed to by the string builder.
                        //Another inefficient way would be to call s= new stringbuilder();
                        //s.Length = 0;

                        if (!string.IsNullOrEmpty(groupByColCaption))
                        {
                            if (!isGroupedWithHeaders)
                            {
                                foreach (UltraGridRow row in grid.Rows)
                                {
                                    if (row.IsGroupByRow && row.Hidden.Equals(false))
                                    {
                                        s.Append("\"").Append(row.Description.ToString()).Append("\"").Append(",");
                                        s.Remove(s.Length - 1, 1);
                                        s.Append(Environment.NewLine);
                                    }
                                }
                            }
                            else
                            {
                                UltraGridColumn[] colArr = new UltraGridColumn[band.Columns.All.Length];

                                band.Columns.All.CopyTo(colArr, 0);
                                Array.Sort(colArr, comparer);

                                foreach (UltraGridColumn col in colArr)
                                {
                                    if (!col.Hidden && (!col.Header.Caption.Equals("")))
                                    {
                                        s.Append(col.Header.Caption).Append(",");
                                    }
                                }
                                foreach (UltraGridRow row in grid.Rows)
                                {
                                    if (row.IsGroupByRow && row.Hidden.Equals(false))
                                    {
                                        s.Append(Environment.NewLine);
                                        SummaryValuesCollection summaryCol = row.ChildBands[0].Rows.SummaryValues;
                                        bool notInSummary = false;
                                        foreach (UltraGridColumn col in colArr)
                                        {
                                            notInSummary = false;
                                            if (!col.Hidden && (!col.Header.Caption.Equals("")))
                                            {
                                                foreach (SummaryValue summary in summaryCol)
                                                {
                                                    if (summary.Key.Equals(col.Key))
                                                    {
                                                        if (summary.Value.ToString().Contains(","))
                                                        {
                                                            s.Append("\"").Append(summary.Value.ToString()).Append("\"").Append(",");
                                                        }
                                                        else
                                                        {
                                                            s.Append(summary.Value.ToString()).Append(",");
                                                        }

                                                        notInSummary = true;
                                                        break;
                                                    }
                                                }
                                                // if column is visible true and does not exists in the summary, then also add in the export file
                                                // so that number of column will be same
                                                if (!notInSummary)
                                                {
                                                    s.Append("").Append(",");
                                                }
                                            }
                                        }
                                        s.Remove(s.Length - 1, 1);
                                    }
                                }
                            }
                        }
                        else
                        {
                            UltraGridColumn[] colArr = new UltraGridColumn[band.Columns.All.Length];

                            band.Columns.All.CopyTo(colArr, 0);

                            Array.Sort(colArr, comparer);
                            foreach (UltraGridColumn col in colArr)
                            {
                                if (!col.Hidden && (!col.Header.Caption.Equals("")))
                                {
                                    s.Append(col.Header.Caption).Append(",");
                                }
                            }

                            if (IsNewLineNeeded == true)
                            {
                                string cleaned = s.ToString().Replace("\n", "").Replace("\r", "");
                                cleaned = cleaned.Replace("  ", " ");
                                s = new StringBuilder(cleaned);
                            }

                            UltraGridRow[] filterednonGropuedRows = grid.Rows.GetFilteredInNonGroupByRows();
                            foreach (UltraGridRow row in filterednonGropuedRows)
                            {
                                s.Append(Environment.NewLine);
                                foreach (UltraGridColumn col in colArr)
                                {
                                    if (!col.Hidden && (!col.Header.Caption.Equals("")))
                                    {
                                        if (row.Cells[col.Key].Value != null)
                                        {
                                            if (row.Cells[col.Key].Value.ToString().Contains(","))
                                            {
                                                if (grid.Name.Equals("alertPopupGridCompliance"))
                                                    s.Append("\"").Append(row.Cells[col.Key].Value.ToString().Replace("\"", "'")).Append("\"").Append(",");
                                                else
                                                    s.Append("\"").Append(row.Cells[col.Key].Value).Append("\"").Append(",");
                                            }
                                            else
                                            {
                                                s.Append(row.Cells[col.Key].Value).Append(",");
                                            }
                                        }
                                        else
                                        {
                                            s.Append("").Append(",");
                                        }
                                    }
                                }
                                s.Remove(s.Length - 1, 1);
                                //NewLine does the job, and writing is done only once
                            }
                        }
                        //Adding New line for separating two grid
                        s.Append(Environment.NewLine);
                    }
                    if (pathName.EndsWith(".xls"))
                    {
                        s.Replace(",", "\t");
                    }
                    sw.WriteLine(s.ToString().TrimEnd(','));
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
            finally
            {
                sw.Close();
            }
        }

        /// <summary>
        /// To return visible column count of band[0] of passed grid
        /// </summary>
        /// <param name="grd">Grid</param>
        /// <returns>VisibleColumnCount</returns>

        private int CountVisibleColumns(UltraGrid grd)
        {
            int visibleGridColumns = 0;

            if (null == grd) return (-1);

            foreach (UltraGridColumn ugCol in grd.DisplayLayout.Bands[0].Columns)
            {
                if (!ugCol.Hidden) visibleGridColumns++;
            }

            return visibleGridColumns;
        }

        /// <summary>
        /// To return Visible row count of a passed grid
        /// </summary>
        /// <param name="grd">Grid</param>
        /// <returns>VisibleRowCount</returns>

        private int CountVisibleRows(UltraGrid grd)
        {
            int GridRows = 0;

            if (null == grd) return (-1);

            foreach (UltraGridRow ugRow in grd.DisplayLayout.Rows)
            {
                if (!ugRow.IsFilteredOut)
                {
                    GridRows++;
                }
            }
            return GridRows;
        }

        public void ExportToExcel(UltraGrid grdBasketDisplay)
        {
            try
            {
                int _count = 0;
                Workbook workBook = new Workbook();

                string pathName = null;
                saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
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
                workBook = OnExportToExcel(_count, workBook, pathName, grdBasketDisplay);
                _count++;
                workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[0];
                try
                {
                    workBook.Save(pathName);
                }
                catch //(Exception ex)
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

        public async System.Threading.Tasks.Task ExportToExcelAsync(UltraGrid grdBasketDisplay)
        {
            ExportToExcel(grdBasketDisplay);

            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;
        }

        private Workbook OnExportToExcel(int key, Workbook workBook, string fileName, UltraGrid grdBasketDisplay)
        {
            try
            {
                ExcelExporter = new UltraGridExcelExporter();

                if (workBook == null)
                {
                    workBook = ExcelExporter.Export(grdBasketDisplay, fileName);
                }

                workBook.Worksheets.Add(grdBasketDisplay.Name + key);
                workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[grdBasketDisplay.Name + key];
                ExcelExporter.Export(grdBasketDisplay, workBook);
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

        private void ExcelExporter_InitializeRow(Object sender, ExcelExportInitializeRowEventArgs e)
        {
            e.Row.Band.ColHeadersVisible = false;
        }

        private string _filePath = null;

        public void SetFilePath(string filename)
        {
            saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            saveFileDialog1.Filter = "Excel WorkBook Files (*.xls)|*.xls|All Files (*.*)|*.*";
            saveFileDialog1.RestoreDirectory = true;
            if (!String.IsNullOrEmpty(filename))
            {
                saveFileDialog1.FileName = filename;
            }
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _filePath = saveFileDialog1.FileName;
            }
            else
            {
                return;
            }
        }

        public void OnExportToExcel(Dictionary<string, UltraGrid> grdDict, Dictionary<string, string> headerDetails, bool isTwoRowsinHeader = false)
        {
            try
            {
                if (_filePath != null)
                {
                    ExcelExporter = new UltraGridExcelExporter();
                    ExcelExporter.ExportFormattingOptions = ExportFormattingOptions.None;
                    ExcelExporter.BandSpacing = 0;
                    ExcelExporter.InitializeRow += ExcelExporter_InitializeRow;
                    ExcelExporter.HeaderRowExporting += new HeaderRowExportingEventHandler(ExcelExporter_HeaderRowExporting);
                    ExcelExporter.CellExporting += new CellExportingEventHandler(ExcelExporter_CellExporting);
                    ExcelExporter.SummaryCellExported += new SummaryCellExportedEventHandler(ExcelExporter_SummaryCellExported);
                    Workbook workBook = new Workbook();

                    foreach (KeyValuePair<string, UltraGrid> grid in grdDict)
                    {
                        //In case workbook is null
                        if (workBook == null)
                        {
                            workBook = ExcelExporter.Export(grid.Value, _filePath);
                        }

                        //The worksheet is added with name as view name of grid to be written.
                        workBook.Worksheets.Add(grid.Key);
                        workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[grid.Key];
                        int excelColumnIndex = 0;
                        bool isHeaderExportNeeded = false;
                        int offSet = 0;
                        if (headerDetails != null && headerDetails.Count > 0 && !isTwoRowsinHeader)
                        {
                            //Extract header details and export to worksheet.
                            foreach (string values in headerDetails[grid.Key].Split(Seperators.SEPERATOR_6))
                            {
                                string[] value = values.Split('=');
                                workBook.Worksheets[grid.Key].Rows[0].Cells[excelColumnIndex].Value = value[0];
                                workBook.Worksheets[grid.Key].Rows[0].Cells[excelColumnIndex].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                workBook.Worksheets[grid.Key].Columns[excelColumnIndex].Width = 6000;
                                workBook.Worksheets[grid.Key].Rows[1].Cells[excelColumnIndex].Value = value[1];
                                excelColumnIndex++;
                            }
                            isHeaderExportNeeded = true;
                        }
                        else if (headerDetails != null && headerDetails.Count > 0)
                        {
                            foreach (string values in headerDetails[grid.Key].Split(Seperators.SEPERATOR_6))
                            {
                                string[] value = values.Split('=');
                                string[] finalValues = value[1].Split(Seperators.SEPERATOR_5);
                                workBook.Worksheets[grid.Key].Rows[0].Cells[excelColumnIndex].Value = value[0];
                                workBook.Worksheets[grid.Key].Rows[0].Cells[excelColumnIndex].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                workBook.Worksheets[grid.Key].Columns[excelColumnIndex].Width = 6000;
                                workBook.Worksheets[grid.Key].Rows[1].Cells[excelColumnIndex].Value = finalValues[0];
                                workBook.Worksheets[grid.Key].Rows[2].Cells[excelColumnIndex].Value = finalValues[1];
                                excelColumnIndex++;
                            }
                            isHeaderExportNeeded = true;
                            offSet = 1;
                        }

                        ColumnsCollection columnsCollection = grid.Value.DisplayLayout.Bands[0].Columns;
                        List<UltraGridColumn> listColumns = columnsCollection.Cast<UltraGridColumn>().Where(column => !column.Hidden).ToList();

                        listColumns.Sort(delegate (UltraGridColumn column1, UltraGridColumn column2)
                        {
                            return column1.Header.VisiblePosition.CompareTo(column2.Header.VisiblePosition);
                        });

                        //int j = grid.Value.DisplayLayout.Bands[0].SortedColumns.Count;
                        int j = 0;
                        foreach (UltraGridColumn column in grid.Value.DisplayLayout.Bands[0].SortedColumns)
                        {
                            if (column.Hidden)
                                listColumns.Add(column);
                            if (column.IsGroupByColumn)
                                j++;
                        }
                        int lastGroupingIndex = 0;                           // keeps a count of last grouping column
                        int totalGroupingColumns = j;
                        if (totalGroupingColumns == 0)
                        {
                            lastGroupingIndex = -1;
                        }
                        else
                        {
                            lastGroupingIndex = totalGroupingColumns - 1;
                        }
                        int excelRowIndex = -1;                                     // keeps a count of excel row, is -1 when export of grid needs to be done from zeroth row in excel

                        if (isHeaderExportNeeded)
                        {
                            excelRowIndex = 2 + offSet;                                    // To leave 1 space after header
                            lastGroupingIndex = excelRowIndex + totalGroupingColumns;
                        }

                        foreach (UltraGridColumn column in listColumns)
                        {
                            if (column.IsGroupByColumn)
                            {
                                int index = grid.Value.DisplayLayout.Bands[0].SortedColumns.IndexOf(column.Key);

                                if (index == 0)
                                {
                                    workBook.Worksheets[grid.Key].Rows[excelRowIndex + 1].Cells[0].Value = column.Header.Caption;
                                    workBook.Worksheets[grid.Key].Rows[excelRowIndex + 1].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    workBook.Worksheets[grid.Key].Columns[0].Width = 6000;
                                    lastGroupingIndex = excelRowIndex + 1 + totalGroupingColumns - 1;
                                }
                                else if (index == 1)
                                {
                                    workBook.Worksheets[grid.Key].Rows[(excelRowIndex + 2)].Cells[1].Value = column.Header.Caption;
                                    workBook.Worksheets[grid.Key].Rows[(excelRowIndex + 2)].Cells[1].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    workBook.Worksheets[grid.Key].Columns[1].Width = 6000;
                                    lastGroupingIndex = excelRowIndex + 2 + totalGroupingColumns - 2;
                                }
                                else if (index == 2)
                                {
                                    workBook.Worksheets[grid.Key].Rows[(excelRowIndex + 3)].Cells[2].Value = column.Header.Caption;
                                    workBook.Worksheets[grid.Key].Rows[(excelRowIndex + 3)].Cells[2].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    workBook.Worksheets[grid.Key].Columns[2].Width = 6000;
                                    lastGroupingIndex = excelRowIndex + 3 + totalGroupingColumns - 3;
                                }
                                else
                                {
                                    workBook.Worksheets[grid.Key].Rows[(excelRowIndex + 4)].Cells[3].Value = column.Header.Caption;
                                    workBook.Worksheets[grid.Key].Columns[3].Width = 6000;
                                    workBook.Worksheets[grid.Key].Rows[(excelRowIndex + 4)].Cells[3].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    lastGroupingIndex = excelRowIndex + 4;
                                }
                            }
                            workBook.Worksheets[grid.Key].Columns[j].Width = 6000;
                            workBook.Worksheets[grid.Key].Rows[lastGroupingIndex + 1].Cells[j].Value = column.Header.Caption;
                            workBook.Worksheets[grid.Key].Rows[lastGroupingIndex + 1].Cells[j].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                            j++;
                        }
                        //Exports grid data to selected worksheet.
                        ExcelExporter.Export(grid.Value, workBook, lastGroupingIndex + 2, 0);
                        if (null != grid.Value && !grid.Value.IsDisposed && !grid.Value.Disposing)
                        {
                            grid.Value.Dispose();
                        }
                    }
                    workBook.Save(_filePath);
                    ExcelExporter.InitializeRow -= ExcelExporter_InitializeRow;
                    _filePath = null;
                    workBook = null;
                    ExcelExporter = null;
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

        //private bool _isHeaderExported = false;

        private void ExcelExporter_HeaderRowExporting(object sender, HeaderRowExportingEventArgs e)
        {
        }

        public void PrintGridContent(UltraGrid grdToPrint)
        {
            UltraGridLayout layout = new UltraGridLayout();

            layout.Appearance.BackColor = Color.White;
            layout.Appearance.ForeColor = Color.Black;
            try
            {
                PrintDialog printersettings = new PrintDialog();
                //printersettings.ShowDialog();

                DialogResult dlgresult = printersettings.ShowDialog();
                if (dlgresult == DialogResult.OK)
                    grdToPrint.Print(layout);
            }
            catch//(Exception ex )
            {
                throw;
            }
        }

        public class VisiblePosComparer : IComparer
        {
            public VisiblePosComparer()
            {
            }

            public int Compare(object x, object y)
            {
                UltraGridColumn xCol = (UltraGridColumn)x;
                UltraGridColumn yCol = (UltraGridColumn)y;

                if (xCol.Group != null && yCol.Group != null && xCol.Group.Index != yCol.Group.Index)
                    return xCol.Group.Index.CompareTo(yCol.Group.Index);
                else
                    return xCol.Header.VisiblePosition.CompareTo(yCol.Header.VisiblePosition);
            }
        }

        private void ExcelExporter_CellExporting(object sender, CellExportingEventArgs e)
        {
            try
            {
                Worksheet ws = e.CurrentWorksheet;
                WorksheetRow wsRow = ws.Rows[e.CurrentRowIndex];
                WorksheetCell wsCell = wsRow.Cells[e.CurrentColumnIndex];
                if (e.ExportValue != null)
                {
                    double convertedDoubleValue = 0.0;
                    bool result = double.TryParse(e.ExportValue.ToString(), out convertedDoubleValue);
                    if (result)
                    {
                        wsCell.Value = convertedDoubleValue;
                        wsCell.CellFormat.FormatString = "#,##,###0";
                        wsCell.CellFormat.Alignment = HorizontalCellAlignment.Right;
                    }
                }
                else
                {
                    e.ExportValue = ApplicationConstants.C_Dash;
                    wsCell.CellFormat.Alignment = HorizontalCellAlignment.Right;
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

        private void ExcelExporter_SummaryCellExported(object sender, SummaryCellExportedEventArgs e)
        {
            Worksheet ws = e.CurrentWorksheet;
            WorksheetRow wsRow = ws.Rows[e.CurrentRowIndex];
            WorksheetCell wsCell = wsRow.Cells[e.CurrentColumnIndex];
            double convertedDoubleValue = 0.0;
            if (e.Summary.Value != null)
            {
                bool result = double.TryParse(e.Summary.Value.ToString(), out convertedDoubleValue);
                if (result)
                {
                    wsCell.Value = convertedDoubleValue;
                    string formatString = string.Empty;
                    string summaryFormat = e.Summary.SummarySettings.DisplayFormat;
                    if (summaryFormat.Contains("{0:"))
                    {
                        formatString = summaryFormat.Replace("{0:", "").Replace("}", "");
                        wsCell.CellFormat.FormatString = formatString;
                        wsCell.CellFormat.Alignment = HorizontalCellAlignment.Right;
                    }
                }
            }
        }
    }
}