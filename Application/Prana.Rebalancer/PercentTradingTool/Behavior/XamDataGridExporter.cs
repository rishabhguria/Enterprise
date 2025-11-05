using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.ExcelExporter;
using Microsoft.Xaml.Behaviors;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace Prana.Rebalancer.PercentTradingTool.Behavior
{
    public class XamDataGridExporter : Behavior<Window>
    {
        /// <summary>
        /// The export grid property
        /// </summary>
        public static readonly DependencyProperty ExportGridProperty = DependencyProperty.Register("ExportGrid", typeof(bool), typeof(XamDataGridExporter), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnExportGridValueChanged));

        /// <summary>
        /// The exporter resource property
        /// </summary>
        public static readonly DependencyProperty ExporterResourceProperty = DependencyProperty.Register("ExporterResource", typeof(DataPresenterExcelExporter), typeof(XamDataGridExporter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Gets or sets the exporter resource.
        /// </summary>
        /// <value>
        /// The exporter resource.
        /// </value>
        public DataPresenterExcelExporter ExporterResource
        {
            get { return (DataPresenterExcelExporter)GetValue(ExporterResourceProperty); }
            set { SetValue(ExporterResourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [export grid].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [export grid]; otherwise, <c>false</c>.
        /// </value>
        public bool ExportGrid
        {
            get { return (bool)GetValue(ExportGridProperty); }
            set { SetValue(ExportGridProperty, value); }
        }

        /// <summary>
        /// The _file name
        /// </summary>
        private static string _fileName;



        /// <summary>
        /// The exporter
        /// </summary>
        private static DataPresenterExcelExporter _exporter = null;

        private static bool _isCSVExportEnabled = bool.Parse(ConfigurationManager.AppSettings["IsCSVExportEnabled_PTT"]);
        private static void OnExportGridValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                {
                    return;
                }
                if (!_isCSVExportEnabled)
                {
                    XamDataGridExporter xamGridExporter = (XamDataGridExporter)d;
                    if (_exporter == null)
                    {
                        Window window = xamGridExporter.AssociatedObject;
                        SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Excel2007|*.xlsx|Excel97To2003|*.xls|Excel97To2003Template|*.xlt", DefaultExt = "xls" };
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            _exporter = xamGridExporter.ExporterResource;
                            _exporter.CellExported += _exporter_CellExported;
                            _fileName = saveFileDialog.FileName;
                            WorkbookFormat format = SetWorkBookFormat(Path.GetExtension(saveFileDialog.FileName));
                            Workbook workbook = new Workbook(format);
                            List<XamDataGrid> childXamDataGrids = FindVisualChildren<XamDataGrid>(window).ToList();
                            foreach (XamDataGrid xamDataGrid in childXamDataGrids)
                            {
                                if (xamDataGrid.Records.Count > 0)
                                {
                                    Worksheet sheet = workbook.Worksheets.Add(xamDataGrid.Name);
                                    _exporter.Export(xamDataGrid, sheet);
                                }
                            }

                            workbook.Save(saveFileDialog.FileName);
                            ExportFinished();
                        }
                        else
                        {
                            _exporter = null;
                        }
                    }
                    else
                        System.Windows.MessageBox.Show("Export is already in progress", "Nirvana Alert!", MessageBoxButton.OK, MessageBoxImage.Information);
                    xamGridExporter.ExportGrid = false;
                }
                else
                {
                    XamDataGridExporter xamGridExporter = (XamDataGridExporter)d;
                    Window window = xamGridExporter.AssociatedObject;
                    List<XamDataGrid> childXamDataGrids = FindVisualChildren<XamDataGrid>(window).ToList();

                    string filePath = GetFilePath();

                    // check for the File Path i.e. Is Path Valid
                    //bool isFilePathExists = System.IO.Directory.Exists(filePath);                   

                    if (!String.IsNullOrEmpty(filePath))
                    {
                        string delimiter = ",";

                        ExportToCSV(childXamDataGrids, filePath, delimiter);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Please select a file.", "Nirvana Alert!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    _exporter = null;
                    xamGridExporter.ExportGrid = false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        private static void ExportToCSV(List<XamDataGrid> childXamDataGrids, string filePath, string delimiter)
        {
            try
            {
                StreamWriter sw = File.CreateText(filePath);
                StringBuilder sb = new StringBuilder();

                sb.Append("Symbol").Append(delimiter).Append("Price").Append(delimiter).Append("AccountId").Append(delimiter).Append("AccountName").Append(delimiter).Append("OrderSide");
                sb.Append(delimiter).Append("TradeQuantity");
                sb.Append(delimiter).Append("FxRate");

                sw.WriteLine(sb.ToString());
                sb = new StringBuilder();

                if (childXamDataGrids != null && childXamDataGrids[0].Records.Count > 0 && childXamDataGrids[1].Records.Count > 0)
                {
                    string symbol = string.Empty;
                    string price = string.Empty;

                    foreach (DataRecord upperGridRecord in childXamDataGrids[0].Records)
                    {
                        CellCollection cells = upperGridRecord.Cells;
                        if (cells[PTTConstants.COL_SYMBOL].Value != null)
                        {
                            symbol = cells[PTTConstants.COL_SYMBOL].Value.ToString();
                        }

                        if (cells[PTTConstants.COL_SELECTEDFEEDPRICE].Value != null)
                        {
                            price = cells[PTTConstants.COL_SELECTEDFEEDPRICE].Value.ToString();
                        }
                    }

                    foreach (DataRecord lowerGridRecord in childXamDataGrids[1].Records)
                    {
                        sb.Append(symbol).Append(delimiter);
                        sb.Append(price.ToString()).Append(delimiter);

                        PTTResponseObject pttResponseObject = (PTTResponseObject)lowerGridRecord.DataItem;

                        if (pttResponseObject != null && !string.IsNullOrEmpty(pttResponseObject.AccountId.ToString()))
                        {
                            sb.Append(pttResponseObject.AccountId.ToString()).Append(delimiter);
                            string accountName = CachedDataManager.GetInstance.GetAccount(pttResponseObject.AccountId);

                            if (accountName.Contains(","))
                            {
                                sb.Append("\"" + accountName + "\"").Append(delimiter);
                            }
                            else
                            {
                                sb.Append(accountName).Append(delimiter);
                            }
                        }
                        else
                        {
                            sb.Append("").Append(delimiter);
                        }

                        if (pttResponseObject != null && !string.IsNullOrEmpty(pttResponseObject.OrderSide))
                        {
                            string side = TagDatabaseManager.GetInstance.GetOrderSideText(pttResponseObject.OrderSide);

                            sb.Append(side).Append(delimiter);
                        }
                        else
                        {
                            sb.Append("").Append(delimiter);
                        }

                        if (pttResponseObject != null)
                        {
                            string tradeQty = pttResponseObject.TradeQuantity.ToString();

                            sb.Append(tradeQty).Append(delimiter);
                        }
                        else
                        {
                            sb.Append("").Append(delimiter);
                        }

                        if (pttResponseObject != null)
                        {
                            string fxRate = pttResponseObject.FxRate.ToString();

                            sb.Append(fxRate).Append(delimiter);
                        }
                        else
                        {
                            sb.Append("0").Append(delimiter);
                        }

                        sb.Remove(sb.Length - 1, 1);
                        sb.Append(Environment.NewLine);
                    }
                }

                sw.WriteLine(sb.ToString());
                sb = new StringBuilder();

                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                }

                System.Windows.MessageBox.Show("Orders exported successfully.", "Nirvana Alert!", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private static string GetFilePath()
        {
            string exportedFilePath = string.Empty;
            string filePath = string.Empty;

            try
            {
                if (_isCSVExportEnabled)
                {
                    if (ConfigurationManager.AppSettings["CSVExportFilePath_PTT"] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("CSVExportFilePath_PTT")))
                    {
                        exportedFilePath = ConfigurationManager.AppSettings["CSVExportFilePath_PTT"].ToString();
                    }

                    if (string.IsNullOrWhiteSpace(exportedFilePath) || (!Directory.Exists(Path.GetDirectoryName(exportedFilePath))))
                    {
                        filePath = OpenSaveDialogBox();
                    }
                    else
                    {
                        var dateWithCurrentTime = DateTime.Now.Date.AddTicks(DateTime.Now.TimeOfDay.Ticks);
                        string dateFormat = dateWithCurrentTime.ToString("MMddyyyyhhmmss");

                        string fileName = Path.GetFileName(exportedFilePath);

                        string fileExtension = Path.GetExtension(exportedFilePath);

                        string fileNameWithoutExtension = fileName.Substring(0, fileName.Length - fileExtension.Length);

                        fileName = fileNameWithoutExtension + "_" + dateFormat + fileExtension;

                        filePath = Path.GetDirectoryName(exportedFilePath) + "\\" + fileName;

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
            return filePath;
        }

        private static string OpenSaveDialogBox()
        {
            string strFilePath = string.Empty;
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
                saveFileDialog.RestoreDirectory = true;
                DialogResult result = saveFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    strFilePath = saveFileDialog.FileName;
                }
                else if (result == DialogResult.Cancel)
                {
                    strFilePath = string.Empty;
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
            return strFilePath;
        }

        static void _exporter_CellExported(object sender, CellExportedEventArgs e)
        {
            try
            {
                if (e.Field != null)
                {
                    if (e.Field.Name == PTTConstants.COL_MASTERFUNDORACCOUNT ||
                        e.Field.Name == PTTConstants.COL_ACCOUNT ||
                        e.Field.Name == PTTConstants.COL_COMBINEDACCOUNTSTOTALVALUE ||
                        e.Field.Name == PTTConstants.COL_TYPE ||
                        e.Field.Name == PTTConstants.COL_ORDERSIDEID ||
                        e.Field.Name == PTTConstants.COL_ADDORSET ||
                        e.Field.Name == PTTConstants.COL_ACCOUNTID)
                    {
                        string displayValue = String.Empty;
                        if (e.Record.Cells[e.Field.Name].Value is KeyValuePair<string, string>)
                        {
                            displayValue = ((KeyValuePair<string, string>)e.Record.Cells[e.Field.Name].Value).Value;
                        }
                        else if (e.Record.Cells[e.Field.Name].Value is EnumerationValue)
                        {
                            displayValue = ((EnumerationValue)e.Record.Cells[e.Field.Name].Value).DisplayText;
                        }
                        else if (e.Field.Name == PTTConstants.COL_COMBINEDACCOUNTSTOTALVALUE)
                        {
                            displayValue = (bool)e.Record.Cells[e.Field.Name].Value ? "Yes" : "No";
                        }
                        else if (e.Field.Name == PTTConstants.COL_ACCOUNT)
                        {
                            PTTRequestObject pstRequestObject = (PTTRequestObject)e.Record.DataItem;
                            if (pstRequestObject != null)
                            {
                                displayValue = GetAccountString(pstRequestObject.Account.Cast<Account>());
                            }
                        }
                        else if (e.Field.Name == PTTConstants.COL_ACCOUNTID)
                        {
                            PTTResponseObject pttResponseObject = (PTTResponseObject)e.Record.DataItem;
                            if (pttResponseObject != null)
                                displayValue = CachedDataManager.GetInstance.GetAccount(pttResponseObject.AccountId);
                        }
                        else if (e.Field.Name == PTTConstants.COL_ORDERSIDEID)
                        {
                            PTTResponseObject pttResponseObject = (PTTResponseObject)e.Record.DataItem;
                            if (pttResponseObject != null && !string.IsNullOrEmpty(pttResponseObject.OrderSide))
                                displayValue = TagDatabaseManager.GetInstance.GetOrderSideText(pttResponseObject.OrderSide);
                        }
                        e.CurrentWorksheet.Rows[e.CurrentRowIndex].Cells[e.CurrentColumnIndex].Value = displayValue;
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


        private static string GetAccountString(IEnumerable<Account> enumerable)
        {
            StringBuilder displayValue = new StringBuilder();
            try
            {
                foreach (Account account in enumerable)
                {
                    displayValue.Append(account.Name);
                    displayValue.Append(Seperators.SEPERATOR_14);
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
            return displayValue.ToString();
        }

        static void ExportFinished()
        {
            try
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Data exported successfully. Do you want to open the file ?", "Nirvana Alert!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result.Equals(MessageBoxResult.Yes))
                {
                    try
                    {
                        Process p = new Process { StartInfo = { FileName = _fileName } };
                        p.Start();
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                        if (rethrow)
                            throw;
                    }
                }
                _exporter.CellExported -= _exporter_CellExported;
                _exporter = null;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the work book format.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        /// <returns></returns>
        private static WorkbookFormat SetWorkBookFormat(string fileExtension)
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

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
