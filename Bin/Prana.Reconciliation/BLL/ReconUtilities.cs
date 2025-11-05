using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Prana.BusinessObjects;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using System.Windows.Forms;
using System.IO;
using Infragistics.Documents.Excel;
using Prana.Utilities.MiscUtilities;
using Prana.BusinessObjects.AppConstants;
using Infragistics.Win.UltraWinGrid.DocumentExport;
namespace Prana.Reconciliation
{
    public class ReconUtilities
    {


        private static List<String> numericColumn;

        public static List<String> NumericColumn
        {
            get { return numericColumn; }
            set { numericColumn = value; }
        }
        //public static void CopyColumns(DataTable dtCopyFrom, DataTable dtCopyTo)
        //{

        //    DataColumn[] columnNameList = new DataColumn[dtCopyFrom.Columns.Count];
        //    dtCopyFrom.Columns.CopyTo(columnNameList, 0);
        //    foreach (DataColumn column in columnNameList)
        //    {
        //        if (!dtCopyTo.Columns.Contains(column.ColumnName))
        //            dtCopyTo.Columns.Add(column.ColumnName);
        //    }
        //}





        //public static void AddPrimaryKey(DataTable dt, MatchingRule rule)
        //{
        //    if (!dt.Columns.Contains("RowID"))
        //    {
        //        dt.Columns.Add("RowID");
        //    }

        //    //if (!dt.Columns.Contains("ComparisonKey"))
        //    //{
        //    //    dt.Columns.Add("ComparisonKey");
        //    //}


        //    int rowID = 0;

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        row["RowID"] = rowID;
        //        rowID++;
        //    }
        //    dt.PrimaryKey = new DataColumn[] { dt.Columns["RowID"] };

        //}

        public static void GenerateExceptionsReport(DataTable dtExceptions, string exceptionFileName, AutomationEnum.FileFormat ExpReportFormat, List<ColumnInfo> selectedColumnList, List<ColumnInfo> sortByColumnList, List<ColumnInfo> groupByColumnList)
        {


            // Infragistics.Documents.Excel.Workbook exceptionReport = new Infragistics.Documents.Excel.Workbook();
            try
            {

                //listMasterColumns.Sort(delegate(ColumnInfo m1, ColumnInfo m2) { return m1.VisibleOrder.CompareTo(m2.VisibleOrder); });

                UltraGrid gridExceptions = new UltraGrid();
                //gridExceptions.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
                //gridExceptions.DisplayLayout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.False;
                //gridExceptions.DisplayLayout.Override.GroupByRowDescriptionMask = "[value] ([count] [count,items,item,items])";

                Form UI = new Form();
                UI.Controls.Add(gridExceptions);

                // string columnToSortBy = "MismatchType";
                gridExceptions.DataSource = dtExceptions;


                int columnIndexMismatchReason = gridExceptions.DisplayLayout.Bands[0].Columns.IndexOf(ReconConstants.MismatchReason);
                if (columnIndexMismatchReason != -1)
                {
                    gridExceptions.DisplayLayout.Bands[0].Columns[ReconConstants.MismatchReason].Header.Caption = ReconConstants.CAPTION_MismatchReason;
                }
                int columnIndexMismatchType = gridExceptions.DisplayLayout.Bands[0].Columns.IndexOf(ReconConstants.MismatchType);
                if (columnIndexMismatchType != -1)
                {
                    gridExceptions.DisplayLayout.Bands[0].Columns[ReconConstants.MismatchType].Header.Caption = ReconConstants.CAPTION_MismatchType;
                }

                #region commented
                //int visiblePosition = 1;
                //foreach (ColumnInfo masterColumn in selectedColumnList)
                //{
                //   //arrange the view order of the columns
                //    if (dtExceptions.Columns.Contains(masterColumn.ColumnName))
                //    {
                //        UltraGridColumn column = gridExceptions.DisplayLayout.Bands[0].Columns[masterColumn.ColumnName];
                //        //column.Header.VisiblePosition = visiblePosition;

                //    }
                //    //else if (listComparisonFields.Contains(masterColumn.ColumnName))
                //    //{
                //    //    string NirvanaColumnKey = "Nirvana" + masterColumn.ColumnName;
                //    //    string BrokerColumnKey = "Broker" + masterColumn.ColumnName;
                //    //    string DiffColumnKey = "Diff" + masterColumn.ColumnName;

                //    //    UltraGridColumn NirvanaColumn = gridExceptions.DisplayLayout.Bands[0].Columns[NirvanaColumnKey];
                //    //    NirvanaColumn.Header.VisiblePosition = visiblePosition;
                //    //    visiblePosition++;
                //    //    UltraGridColumn BrokerColumn = gridExceptions.DisplayLayout.Bands[0].Columns[BrokerColumnKey];
                //    //    BrokerColumn.Header.VisiblePosition = visiblePosition;
                //    //    visiblePosition++;
                //    //    if (listNumericFields.Contains(masterColumn.ColumnName))
                //    //    {
                //    //        UltraGridColumn DiffColumn = gridExceptions.DisplayLayout.Bands[0].Columns[DiffColumnKey];
                //    //        DiffColumn.Header.VisiblePosition = visiblePosition;
                //    //    }
                //    //}
                //    //visiblePosition++;
                //}

                #endregion
                if (groupByColumnList.Count > 0 && ExpReportFormat == AutomationEnum.FileFormat.xls)
                {
                    //grouping logic
                    gridExceptions.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
                    gridExceptions.DisplayLayout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.False;
                    gridExceptions.DisplayLayout.Override.GroupByRowDescriptionMask = "[value]";
                    gridExceptions.DisplayLayout.Bands[0].SortedColumns.Clear();
                    string columnName = string.Empty;
                    foreach (ColumnInfo groupByColumn in groupByColumnList)
                    {
                        switch (groupByColumn.GroupType)
                        {
                            case ColumnGroupType.Nirvana:
                                columnName = "Nirvana" + groupByColumn.ColumnName;
                                break;
                            case ColumnGroupType.PrimeBroker:
                                columnName = "Broker" + groupByColumn.ColumnName;
                                break;
                            case ColumnGroupType.Common:
                                columnName = groupByColumn.ColumnName;
                                break;
                            case ColumnGroupType.Both:
                                break;
                            case ColumnGroupType.Diff:
                                columnName = "Diff" + groupByColumn.ColumnName;
                                break;
                            default:
                                break;
                        }
                        int columnIndex = gridExceptions.DisplayLayout.Bands[0].Columns.IndexOf(columnName);
                        if (columnIndex != -1)
                        {
                            gridExceptions.DisplayLayout.Bands[0].SortedColumns.Add(columnName, false, true);
                        }
                    }
                }

                if (sortByColumnList.Count > 0)
                {
                    //logic for sorting
                    //allow multiple sorting
                    gridExceptions.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;

                    string columnName = string.Empty;
                    foreach (ColumnInfo sortByColumn in sortByColumnList)
                    {

                        switch (sortByColumn.GroupType)
                        {
                            case ColumnGroupType.Nirvana:
                                columnName = "Nirvana" + sortByColumn.ColumnName;
                                break;
                            case ColumnGroupType.PrimeBroker:
                                columnName = "Broker" + sortByColumn.ColumnName;
                                break;
                            case ColumnGroupType.Common:
                                columnName = sortByColumn.ColumnName;
                                break;
                            case ColumnGroupType.Both:
                                break;
                            case ColumnGroupType.Diff:
                                columnName = "Diff" + sortByColumn.ColumnName;
                                break;
                            default:
                                break;
                        }
                        int columnIndex = gridExceptions.DisplayLayout.Bands[0].Columns.IndexOf(columnName);
                        if (columnIndex != -1)
                        {
                            if (sortByColumn.SortOrder.Equals(SortingOrder.Ascending))
                            {
                                gridExceptions.DisplayLayout.Bands[0].Columns[columnName].SortIndicator = SortIndicator.Ascending;
                            }
                            else
                            {
                                gridExceptions.DisplayLayout.Bands[0].Columns[columnName].SortIndicator = SortIndicator.Descending;
                            }
                        }
                    }
                }
                //Modified By: Surendra Bisht
                //Modification date: Dec 17, 2013
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-3009


                List<string> str = new List<string>();
                foreach (ColumnInfo ci in selectedColumnList)
                {
                    str.Add(ci.ColumnName);
                }

                if (gridExceptions.DisplayLayout.Bands[0].Columns.Exists("MismatchType"))
                {
                if (str.Contains("MismatchType"))
                    gridExceptions.DisplayLayout.Bands[0].Columns["MismatchType"].Hidden = false;
                else
                    gridExceptions.DisplayLayout.Bands[0].Columns["MismatchType"].Hidden = true;
                }
                if (gridExceptions.DisplayLayout.Bands[0].Columns.Exists("Matched"))
                {
                gridExceptions.DisplayLayout.Bands[0].Columns["Matched"].Hidden = true;
                }

                ExcelAndPrintUtilities excelUtils = new ExcelAndPrintUtilities();

                List<UltraGrid> grids = new List<UltraGrid>();
                grids.Add(gridExceptions);

                if (ExpReportFormat.Equals(AutomationEnum.FileFormat.csv))
                {
                    excelUtils.SetExcelLayoutAndWrite(grids, true, exceptionFileName + ".csv");
                }
                else if (ExpReportFormat.Equals(AutomationEnum.FileFormat.xls))
                {
                    UltraGridExcelExporter ultragridExporter = new UltraGridExcelExporter();
                    ultragridExporter.Export(gridExceptions, exceptionFileName + ".xls");






                    //Modified By: Surendra Bisht
                    //Modification date: Jan 29, 2013
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-2934 
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-3287

                    Infragistics.Documents.Excel.Workbook workbook = Infragistics.Documents.Excel.Workbook.Load(exceptionFileName + ".xls");


                    workbook.Worksheets[0].Rows[0].CellFormat.Font.Bold = Infragistics.Documents.Excel.ExcelDefaultableBoolean.True;
                    //workbook.Worksheets[0].Rows[0].Worksheet.DisplayOptions.GridlineColor = System.Drawing.Color.Orange;  
  
                    int numColumns = 0;    // total no. of columns in Exception Report
                    while (workbook.Worksheets[0].Rows[0].Cells[numColumns].Value != null)
                        numColumns++;
                    int numRows = workbook.Worksheets[0].Rows[((ICollection<WorksheetRow>)workbook.Worksheets[0].Rows).Count - 1].Index + 1;    // no. of rows.    
                    int IndexOfMismatchType = -1;           // index of Mismatch type column in excel file. 


                    int indexOfdiffMarketvaluebase = -1;
                    int indexOfFundname = -1;

                    for (int j = 0; j < numColumns; j++)
                    {
                        string a = (String)workbook.Worksheets[0].Rows[0].Cells[j].Value;
                        if (a.Contains("Diff"))
                            workbook.Worksheets[0].Rows[0].Cells[j].CellFormat.Fill = CellFillPattern.CreateSolidFill(System.Drawing.Color.Olive);
                        else if (a.Equals("MismatchType"))
                            IndexOfMismatchType = j;
                        else if (a.Equals("Diff_MarketValueBase"))
                            indexOfdiffMarketvaluebase = j;
                        else if (a.Equals("Account"))
                            indexOfFundname = j;


                        // aligning the Numeric columns to right
                        a = a.Replace("Nirvana ", string.Empty);
                        a = a.Replace("Broker ", string.Empty);
                        a = a.Replace("Diff_", string.Empty);
                        if (ReconUtilities.NumericColumn.Contains(a))
                        {
                            workbook.Worksheets[0].Columns[j].CellFormat.Alignment = Infragistics.Documents.Excel.HorizontalCellAlignment.Right;
                            if (a.Equals("Quantity"))
                                workbook.Worksheets[0].Columns[j].CellFormat.FormatString = "#,##0"; // comma seperated values
                            else
                                workbook.Worksheets[0].Columns[j].CellFormat.FormatString = "#,##0.00";   // workbook.Worksheets[0].Columns[j].CellFormat.FormatString = "#,##0.00#####";



                            for (int b = 1; b < numRows; b++)       // for saving all the numeric columns as numeric columns.
                            {
                                Double no = Convert.ToDouble(workbook.Worksheets[0].Rows[b].Cells[j].Value.ToString());
                                workbook.Worksheets[0].Rows[b].Cells[j].Value = no;
                            }

                        }
                    }



                    for (int i = 1; i < numRows; i++)
                    {
                        string a = (String)workbook.Worksheets[0].Rows[i].Cells[IndexOfMismatchType].Value;
                        if (a.Contains("Matched"))                                                       // Highlight matched data in exception report
                            workbook.Worksheets[0].Rows[i].CellFormat.Fill = CellFillPattern.CreateSolidFill(System.Drawing.Color.Yellow);
                        else if (a.Equals("App data in Sync"))
                            workbook.Worksheets[0].Rows[i].Cells[IndexOfMismatchType].CellFormat.Fill = CellFillPattern.CreateSolidFill(System.Drawing.Color.Green);

                        if (indexOfdiffMarketvaluebase != -1 & indexOfFundname != -1)                // Highlight break in marketbvaluebase according to fundname 
                        {
                            int Diff_MarketValueBase = (int)workbook.Worksheets[0].Rows[0].Cells[indexOfdiffMarketvaluebase].Value;
                            String Fundname = (String)workbook.Worksheets[0].Rows[0].Cells[indexOfFundname].Value;
                            if (Diff_MarketValueBase > 25000 && (Fundname.Equals("BP") || Fundname.Equals("MF")))
                                workbook.Worksheets[0].Rows[i].Cells[IndexOfMismatchType].CellFormat.Fill = CellFillPattern.CreateSolidFill(System.Drawing.Color.Gray);
                            else if (Diff_MarketValueBase > 2500 && Fundname.Equals("Whitney"))
                                workbook.Worksheets[0].Rows[i].Cells[IndexOfMismatchType].CellFormat.Fill = CellFillPattern.CreateSolidFill(System.Drawing.Color.Gray);

                        }
                    }

                    workbook.Save(exceptionFileName + ".xls");    // saving the changes made in excel File.

                    //excelUtils.ExportReport(gridExceptions, exceptionFileName, "XLS");
                }
                else if (ExpReportFormat.Equals(AutomationEnum.FileFormat.pdf))                 // surendra
                {
                    //excelUtils.ExportReport(gridExceptions, exceptionFileName, "PDF");

                    UltraGridDocumentExporter ugde = new UltraGridDocumentExporter();
                    ugde.Export(gridExceptions, exceptionFileName + ".pdf", GridExportFileFormat.PDF);
                }




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


            //  return exceptionReport;
        }




        public static void AddPassword(string exceptionFileName, string password)
        {
            try
            {

                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.DisplayAlerts = false;
                    Microsoft.Office.Interop.Excel.Workbook exceptionReport = excelApp.Workbooks.Open(exceptionFileName, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, false, false, false);
                    exceptionReport.SaveAs(exceptionFileName, Type.Missing, password, Type.Missing, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    excelApp.DisplayAlerts = true;
                excelApp.Quit();



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
        public static void AddCustomColumns(DataTable dt, List<ColumnInfo> listColumns)
        {
            try
            {
                foreach (ColumnInfo column in listColumns)
                {
                    if (!string.IsNullOrEmpty(column.ColumnName) && column.IsSelected && !string.IsNullOrEmpty(column.FormulaExpression))
                    {
                        DataColumn customColumn = new DataColumn();
                        customColumn.ColumnName = column.ColumnName;
                        customColumn.Expression = column.FormulaExpression;

                        dt.Columns.Add(customColumn);

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


        public static void SetGridColumns(UltraGrid grid, List<string> listColumns)
        {
            //
            try
            {
                Infragistics.Win.UltraWinGrid.ColumnsCollection columns = grid.DisplayLayout.Bands[0].Columns;
                if (listColumns != null)
                {
                    //Hide all columns
                    foreach (UltraGridColumn col in columns)
                    {
                        columns[col.Key].Hidden = true;
                        col.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    }
                    //Unhide and Set postions for required columns
                    int visiblePosition = 1;
                    foreach (string col in listColumns)
                    {                            
                        if (columns.Exists(col))
                        {
                            UltraGridColumn column = columns[col];
                            column.Hidden = false;
                            column.Header.VisiblePosition = visiblePosition;
                            column.Width = 80;
                            column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                            visiblePosition++;
                        }
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

        public static DataTable GetExceptionsDataTableSchema(List<ColumnInfo> selectedColumnList)
        {
            DataTable dtExceptions = new DataTable();
            try
            {
                foreach (ColumnInfo column in selectedColumnList)
                {
                    //if (!comparisonColumns.Contains(column.ColumnName))
                    //{

                    switch (column.GroupType)
                    {
                        case ColumnGroupType.Nirvana:
                            string NirvanaColumnKey = "Nirvana" + column.ColumnName;

                            string NirvanaCaption = "Nirvana" +" " + column.ColumnName;

                            DataColumn NirvanaColumn = new DataColumn(NirvanaColumnKey);
                            NirvanaColumn.Caption = NirvanaCaption;

                            dtExceptions.Columns.Add(NirvanaColumn);

                            break;
                        case ColumnGroupType.PrimeBroker:

                            string BrokerColumnKey = "Broker" + column.ColumnName;

                            string BrokerCaption = "Broker" + " " + column.ColumnName;

                            DataColumn BrokerColumn = new DataColumn(BrokerColumnKey);
                            BrokerColumn.Caption = BrokerCaption;

                            dtExceptions.Columns.Add(BrokerColumn);
                            break;
                        case ColumnGroupType.Common:
                            string CommonColumnKey = column.ColumnName;
                            DataColumn CommonColumn = new DataColumn(CommonColumnKey);

                            dtExceptions.Columns.Add(CommonColumn);
                            break;
                        case ColumnGroupType.Both:
                            
                            break;
                        case ColumnGroupType.Diff:
                            string DiffColumnKey = "Diff" + column.ColumnName;
                            string DiffCaption = "Diff" + '_' + column.ColumnName;
                            DataColumn diffColumn = new DataColumn(DiffColumnKey);
                            diffColumn.Caption = DiffCaption;
                            dtExceptions.Columns.Add(diffColumn);
                            break;
                        default:
                            break;
                    }
                        
                    //}
                }
                if (!dtExceptions.Columns.Contains(ReconConstants.MismatchType))
                {
                    dtExceptions.Columns.Add(ReconConstants.MismatchType);
                }
                if (!dtExceptions.Columns.Contains(ReconConstants.Matched))
                {
                    dtExceptions.Columns.Add(ReconConstants.Matched);
                }

                if (dtExceptions.Columns.Contains("MasterFund"))
                {
                    dtExceptions.Columns["MasterFund"].Caption = "Fund";
                }
                if (dtExceptions.Columns.Contains("FundName"))
                {
                    dtExceptions.Columns["FundName"].Caption = "Account";
                }
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

            return dtExceptions;
        }


        public static void SaveTransformedPbData(DataSet dsPbdata, string dirName, string FileName)
        {
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            dsPbdata.WriteXml(FileName + ".xml");
        }
    }
}
