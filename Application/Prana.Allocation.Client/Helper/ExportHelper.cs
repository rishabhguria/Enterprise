using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.ExcelExporter;
using Prana.Allocation.Client.Behaviours;
using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Definitions;
using Prana.BusinessLogic;
using Prana.LogManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace Prana.Allocation.Client.Helper
{
    internal class ExportHelper
    {
        /// <summary>
        /// Export Level in XamDataGrid
        /// </summary>
        internal enum ExportLevel
        {
            AllTrades,
            Groups,
            Taxlots
        }

        /// <summary>
        /// The _unbound fields
        /// </summary>
        private static List<string> _unboundFields = new List<string>
        {
                AllocationUIConstants.NETAMOUNT_LOCAL,
                AllocationUIConstants.NETAMOUNT_BASE,
                AllocationUIConstants.PRINCIPAL_AMOUNT_LOCAL,
                AllocationUIConstants.PRINCIPAL_AMOUNT_BASE,
                AllocationUIConstants.COUNTER_CURRENCY,
                AllocationUIConstants.COUNTER_CURRENCY_AMOUNT,
                AllocationUIConstants.EXECUTION_TIME
        };

        /// <summary>
        /// Gets the file name to export.
        /// </summary>
        /// <param name="gridName">Name of the grid.</param>
        /// <returns></returns>
        internal static string GetFileNameToExport(string gridName, ExportHelper.ExportLevel exportLevel)
        {
            string autoFileName = string.Empty;
            try
            {
                switch (gridName)
                {
                    case AllocationClientConstants.CONST_GIRD_ALLOCATED:
                    case AllocationClientConstants.CONST_GIRD_UNALLOCATED:
                        autoFileName = gridName.Replace("Grid", String.Empty) + exportLevel.ToString() + DateTime.Now.ToString("yyyyMMdd");
                        break;

                    default:
                        autoFileName = gridName.Replace("Grid", String.Empty) + DateTime.Now.ToString("yyyyMMdd");
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return autoFileName;
        }

        /// <summary>
        /// Sets the work book format.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        /// <returns></returns>
        internal static WorkbookFormat SetWorkBookFormat(string fileExtension)
        {
            WorkbookFormat workbookFormat = WorkbookFormat.Excel97To2003;
            try
            {
                switch (fileExtension)
                {
                    case ".xlsx":
                        workbookFormat = WorkbookFormat.Excel2007;
                        break;
                    case ".xls":
                        workbookFormat = WorkbookFormat.Excel97To2003;
                        break;
                    case ".xlt":
                        workbookFormat = WorkbookFormat.Excel97To2003Template;
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return workbookFormat;
        }

        /// <summary>
        /// Updates the unbound field values.
        /// </summary>
        /// <param name="e">The <see cref="CellExportingEventArgs"/> instance containing the event data.</param>
        internal static void UpdateUnboundFieldValues(CellExportingEventArgs e, List<int> assetsWithCommissionInNetAmountList)
        {
            try
            {
                string dataItemType = e.Record.DataItem.GetType().Name;
                if (_unboundFields.Contains(e.Field.Name))
                {
                    if (!e.Field.Name.Equals(AllocationUIConstants.EXECUTION_TIME) && !e.Field.Name.Equals(AllocationUIConstants.COUNTER_CURRENCY))
                        e.Field.DataType = typeof(Double);

                    double cumQty = Convert.ToDouble(dataItemType.Equals(AllocationUIConstants.TAXLOT_FIELD_LAYOUT_NAME) ? e.Record.Cells["ExecutedQty"].Value : e.Record.Cells[AllocationUIConstants.CUMQTY].Value);

                    CalculatedFields calculatedFields = new CalculatedFields();
                    calculatedFields.FieldName = e.Field.Name;
                    calculatedFields.AssetsWithCommissionInNetAmount = assetsWithCommissionInNetAmountList;
                    calculatedFields.PrecisionFormat = CommonAllocationMethods.SetPrecisionStringFormat(15);
                    calculatedFields.AvgPrice = Convert.ToDouble(e.Record.Cells[AllocationUIConstants.AVGPRICE].Value);
                    calculatedFields.FxConversionOperator = e.Record.Cells[AllocationUIConstants.FX_CONVERSION_METHOD_OPERATOR].Value.ToString();
                    calculatedFields.Fxrate = Convert.ToDouble(e.Record.Cells[AllocationUIConstants.FXRATE].Value);
                    calculatedFields.TotalCommissionAndFee = Convert.ToDouble(e.Record.Cells[AllocationUIConstants.TOTAL_COMMISSION_AND_FEES].Value);
                    calculatedFields.CumQty = cumQty;
                    calculatedFields.ContractMultiplier = Convert.ToDouble(e.Record.Cells[AllocationUIConstants.CONTRACT_MULTIPLIER].Value);
                    calculatedFields.SideMultiplier = Calculations.GetSideMultilpier(e.Record.Cells[AllocationUIConstants.ORDERSIDE_TAGVALUE].Value.ToString());
                    calculatedFields.AssetId = Convert.ToInt32(e.Record.Cells[AllocationUIConstants.ASSETID].Value);
                    calculatedFields.AccruedInterest = Convert.ToDouble(e.Record.Cells[AllocationUIConstants.ACCRUED_INTEREST].Value);
                    calculatedFields.AuecLocaDateTime = Convert.ToDateTime(e.Record.Cells[AllocationUIConstants.AUEC_LOCAL_DATE].Value);
                    calculatedFields.CurrencyId = Convert.ToInt32(e.Record.Cells[AllocationUIConstants.CURRENCY_ID].Value.ToString());
                    calculatedFields.VSCurrencyId = Convert.ToInt32(e.Record.Cells[AllocationUIConstants.VS_CURRENCY_ID].Value.ToString());
                    calculatedFields.LeadCurrencyId = Convert.ToInt32(e.Record.Cells[AllocationUIConstants.LEAD_CURRENCY_ID].Value.ToString());
                    e.Value = FieldCalculator.CalculateFieldValue(calculatedFields);
                }
                //set excel formatting
                if (e.Field.DataType == typeof(double) || e.Field.DataType == typeof(decimal) || e.Field.DataType == typeof(int))
                    e.FormatSettings.FormatString = CommonAllocationMethods.SetPrecisionExcelFormat(15);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Opens the exported file.
        /// </summary>
        /// <param name="gridName">Name of the grid.</param>
        /// <param name="fileName">Name of the file.</param>
        internal static void OpenExportedFile(string gridName, string fileName)
        {
            try
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(gridName.Replace("Grid", String.Empty) + " data exported successfully. Do you want to open the file ?", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result.Equals(MessageBoxResult.Yes))
                {
                    try
                    {
                        Process p = new Process { StartInfo = { FileName = fileName } };
                        p.Start();
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
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
        /// Removes the empty rows and columns.
        /// </summary>
        /// <param name="e">The <see cref="ExportEndingEventArgs"/> instance containing the event data.</param>
        internal static void RemoveEmptyRowsAndColumns(Worksheet exportedDataSheet)
        {
            try
            {
                Stack rowIndex = new Stack();
                foreach (WorksheetRow row in exportedDataSheet.Rows)
                {
                    if (row.Cells.All(x => string.IsNullOrWhiteSpace(x.Value.ToString())))
                        rowIndex.Push(row.Index);
                }
                while (rowIndex.Count > 0)
                {
                    exportedDataSheet.Rows.Remove((int)rowIndex.Pop());
                }
                object firstCellValue = exportedDataSheet.GetCell("A1").Value;
                if (firstCellValue == null || string.IsNullOrWhiteSpace(firstCellValue.ToString()))
                    exportedDataSheet.HideColumns(0);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Determines whether [is export data] [the specified xam grid exporter].
        /// </summary>
        /// <param name="xamGridExporter">The xam grid exporter.</param>
        /// <param name="xamDataGrid">The xam data grid.</param>
        /// <param name="exporter">The exporter.</param>
        /// <param name="gridName">Name of the grid.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        ///   <c>true</c> if [is export data] [the specified xam grid exporter]; otherwise, <c>false</c>.
        /// </returns>
        internal static bool IsExportData(XamDataGridExporter xamGridExporter, XamDataGrid xamDataGrid, DataPresenterExcelExporter exporter, out string gridName, out string fileName, ExportHelper.ExportLevel exportLevel)
        {
            bool isStartExport = false;
            fileName = string.Empty;
            gridName = xamDataGrid.Name;
            try
            {
                if (exporter == null)
                {
                    if (xamDataGrid.Records.Count > 0)
                    {
                        SaveFileDialog saveFileDialog = GetSaveFileDialog(xamDataGrid, exportLevel);
                        if (saveFileDialog.ShowDialog().Equals(DialogResult.OK))
                        {
                            fileName = saveFileDialog.FileName;
                            isStartExport = true;
                        }
                    }
                    else
                        System.Windows.MessageBox.Show("Nothing to export for " + xamDataGrid.Name.Replace("Grid", String.Empty) + " grid.", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    System.Windows.MessageBox.Show("Export is already in progress", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isStartExport;
        }

        /// <summary>
        /// Gets the save file dialog.
        /// </summary>
        /// <param name="xamDataGrid">The xam data grid.</param>
        /// <param name="exportLevel">The export level.</param>
        /// <returns></returns>
        internal static SaveFileDialog GetSaveFileDialog(XamDataGrid xamDataGrid, ExportHelper.ExportLevel exportLevel)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Excel2007|*.xlsx|Excel97To2003|*.xls|Excel97To2003Template|*.xlt", DefaultExt = "xls" };
            try
            {
                string autoFileName = GetFileNameToExport(xamDataGrid.Name, exportLevel);
                saveFileDialog.FileName = autoFileName;
                string title = "Please select a file to export " + xamDataGrid.Name.Replace("Grid", String.Empty) + (xamDataGrid.Name.ToLower().Contains("preference") ? "" : (" " + exportLevel.ToString().ToLower()));
                saveFileDialog.Title = title;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return saveFileDialog;
        }


    }
}
