using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.ExcelExporter;
using Microsoft.Xaml.Behaviors;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using System.Linq;
using System.Text;
using Prana.Rebalancer.RebalancerNew.Models;
using System.Reflection;
using System.Xml.Linq;
using Prana.Utilities.XMLUtilities;
using Prana.Rebalancer.PercentTradingTool.Behavior;

namespace Prana.Rebalancer.RebalancerNew.Behaviours
{
    public class XamDataGridExporterForRebalancer : Behavior<XamDataGrid>
    {
        /// <summary>
        /// The export grid property
        /// </summary>
        public static readonly DependencyProperty ExportGridProperty = DependencyProperty.Register("ExportGrid", typeof(bool), typeof(XamDataGridExporterForRebalancer), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnExportGridValueChanged));

        public static readonly DependencyProperty ExportFilePathProperty = DependencyProperty.Register("ExportFilePath", typeof(string), typeof(XamDataGridExporterForRebalancer), new PropertyMetadata(string.Empty));

        /// <summary>
        /// The exporter resource property
        /// </summary>
        public static readonly DependencyProperty ExporterResourceProperty = DependencyProperty.Register("ExporterResource", typeof(DataPresenterExcelExporter), typeof(XamDataGridExporterForRebalancer), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty ExportGridDataProperty = DependencyProperty.Register("ExportGridData", typeof(bool), typeof(XamDataGridExporterForRebalancer), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnExportGridDataChanged));

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

        public bool ExportGridData
        {
            get { return (bool)GetValue(ExportGridDataProperty); }
            set { SetValue(ExportGridDataProperty, value); }
        }

        public string ExportFilePath
        {
            get { return (string)GetValue(ExportFilePathProperty); }
            set { SetValue(ExportFilePathProperty, value); }
        }

        /// <summary>
        /// The _file name
        /// </summary>
        private static string _fileName;



        /// <summary>
        /// The exporter
        /// </summary>
        private static DataPresenterExcelExporter _exporter = null;

        private static bool _isCSVExportEnabledForRebalancer = bool.Parse(ConfigurationManager.AppSettings["IsCSVExportEnabled_Rebalancer"]);

        private static bool _isSendToFTPEnabled_RebalancerMainGrid = bool.Parse(ConfigurationManager.AppSettings["IsSendToFTPEnabled_RebalancerMainGrid"]);
        private static void OnExportGridValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                {
                    return;
                }
                if (!_isCSVExportEnabledForRebalancer)
                {
                    XamDataGridExporterForRebalancer xamGridExporter = (XamDataGridExporterForRebalancer)d;
                    if (_exporter == null)
                    {

                        SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Excel2007|*.xlsx|Excel97To2003|*.xls", DefaultExt = "xls" };
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            xamGridExporter.ExportGrid = true;
                            _exporter = xamGridExporter.ExporterResource;
                            _exporter.CellExported += _exporter_CellExported;

                            _fileName = saveFileDialog.FileName;
                            WorkbookFormat format = SetWorkBookFormat(Path.GetExtension(saveFileDialog.FileName));
                            Workbook workbook = new Workbook(format);
                            XamDataGrid xamDataGrid = xamGridExporter.AssociatedObject;
                            if (xamDataGrid.Name.Equals(RebalancerConstants.Grid_ModelPortfolioGrid))
                                _exporter.ExportStarted += ExporterOnExportStarted;
                            if (xamDataGrid.Name.Equals("TradeListGrid"))
                            {
                                _exporter.ExportStarted += ExporterOnExportStartedForTradeList;
                                _exporter.SummaryRowExporting += _exporter_SummaryRowExporting;
                            }
                            if (xamDataGrid.Records.Count > 0)
                            {
                                Worksheet sheet = workbook.Worksheets.Add(xamDataGrid.Name);
                                _exporter.Export(xamDataGrid, sheet);
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
                    XamDataGridExporterForRebalancer xamGridExporter = (XamDataGridExporterForRebalancer)d;
                    XamDataGrid window = xamGridExporter.AssociatedObject;
                    string delimiter = ",";
                    if (window.Name.Equals("TradeListGrid"))
                    {
                        string filePathForBuySellGrid = GetFilePathBuySellGrid();
                        ExportToTradeListCSV(window, filePathForBuySellGrid, delimiter);
                    }
                    else if (window.Name.Equals("RebalacerDataGrid"))
                    {
                        string filePathForMainRebalancerGrid = GetFilePathMainRebalancerGrid();
                        ExportToRebalancerCSV(window, filePathForMainRebalancerGrid, delimiter);
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
        /// <summary>
        /// Exports data to excel from TradeList model
        /// </summary>
        /// <param name="window"></param>
        /// <param name="filePath"></param>
        /// <param name="delimiter"></param>
        private static void ExportToTradeListCSV(XamDataGrid window, string filePath, string delimiter)
        {
            try
            {
                StreamWriter sw = File.CreateText(filePath);
                StringBuilder sb = new StringBuilder();

                sb.Append("AccountId").Append(delimiter).Append("Account").Append(delimiter).Append("Symbol").Append(delimiter).Append("Side").Append(delimiter).Append("Quantity").Append(delimiter).Append("Price_Local").Append(delimiter).Append("Buy/Sell_Value_Base").Append(delimiter).Append("Comments").Append(delimiter).Append("FXRate").Append(delimiter).Append("FXConversionMethodOperator");

                sw.WriteLine(sb.ToString());
                sb = new StringBuilder();

                if (window != null && window.Records.Count > 0)
                {
                    foreach (DataRecord lowerGridRecord in window.Records)
                    {
                        if (lowerGridRecord.DataItem is TradeListModel RebalResponseObject)
                        {
                            if (RebalResponseObject != null && !string.IsNullOrEmpty(RebalResponseObject.AccountId.ToString()))
                            {
                                sb.Append(RebalResponseObject.AccountId.ToString()).Append(delimiter);
                                string accountName = CachedDataManager.GetInstance.GetAccount(RebalResponseObject.AccountId);

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

                            if (RebalResponseObject != null && !string.IsNullOrEmpty(RebalResponseObject.Symbol))
                            {
                                string symbol = RebalResponseObject.Symbol.ToString();

                                sb.Append(symbol).Append(delimiter);
                            }
                            else
                            {
                                sb.Append("").Append(delimiter);
                            }

                            if (RebalResponseObject != null)
                            {
                                string side = RebalResponseObject.Side.ToString();

                                sb.Append(side).Append(delimiter);
                            }
                            else
                            {
                                sb.Append("").Append(delimiter);
                            }

                            if (RebalResponseObject != null)
                            {
                                string quantity = RebalResponseObject.Quantity.ToString();

                                sb.Append(quantity).Append(delimiter);
                            }
                            else
                            {
                                sb.Append("").Append(delimiter);
                            }

                            if (RebalResponseObject != null)
                            {
                                string price = RebalResponseObject.Price.ToString();

                                sb.Append(price).Append(delimiter);
                            }
                            else
                            {
                                sb.Append("").Append(delimiter);
                            }

                            if (RebalResponseObject != null)
                            {
                                string buysellvalue = RebalResponseObject.BuySellValue.ToString();

                                sb.Append(buysellvalue).Append(delimiter);
                            }
                            else
                            {
                                sb.Append("").Append(delimiter);
                            }

                            if (RebalResponseObject != null)
                            {
                                string comments = RebalResponseObject.Comments.ToString();

                                sb.Append(comments).Append(delimiter);
                            }
                            else
                            {
                                sb.Append("").Append(delimiter);
                            }

                            if (RebalResponseObject != null)
                            {
                                string FXRate = RebalResponseObject.FXRate.ToString();

                                sb.Append(FXRate).Append(delimiter);
                            }
                            else
                            {
                                sb.Append("").Append(delimiter);
                            }

                            if (RebalResponseObject != null)
                            {
                                string FXConversionMethodOperator = BusinessObjects.AppConstants.Operator.M.ToString();

                                sb.Append(FXConversionMethodOperator).Append(delimiter);
                            }
                            else
                            {
                                sb.Append("").Append(delimiter);
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
        /// Exports data to excel from Rebalance model
        /// </summary>
        /// <param name="window"></param>
        /// <param name="filePath"></param>
        /// <param name="delimiter"></param>
        private static void ExportToRebalancerCSV(XamDataGrid window, string filePath, string delimiter)
        {
            try
            {
                string folderName = "RebalancerExportXMLs";
                string startupPath = AppDomain.CurrentDomain.BaseDirectory;
                string targetFolderPath = Path.Combine(startupPath, folderName);

                if (!Directory.Exists(targetFolderPath))
                {
                    Directory.CreateDirectory(targetFolderPath);
                }

                string xmlFileName = Path.Combine(targetFolderPath, "RebalancerModels.xml");
                string transformedXmlFileName = Path.Combine(targetFolderPath, "TransformedRebalancerModels.xml");
                string xsltFileName = ConfigurationManager.AppSettings["RebalancerMainGridExportXSLT"];

                if (window != null && window.Records.Count > 0)
                {
                    // Create an XElement to hold the root of the XML
                    XElement rootElement = new XElement("RebalancerModels");
                    foreach (DataRecord record in window.Records)
                    {
                        if (record?.DataItem is RebalancerModel item)
                        {
                            XElement modelElement = new XElement("RebalancerModel");
                            PropertyInfo[] properties = typeof(RebalancerModel).GetProperties();
                            foreach (PropertyInfo property in properties)
                            {
                                // Get the property name and value
                                string propertyName = property.Name;
                                object propertyValue = property.GetValue(item);

                                // Special handling for "AccountId"
                                if (propertyName == "AccountId")
                                {
                                    string fund = "Unknown";
                                    // Ensure propertyValue is not null and is a valid integer
                                    if (int.TryParse(propertyValue?.ToString(), out int accountId))
                                    {
                                        fund = CachedDataManager.GetInstance.GetAccount(accountId);
                                    }
                                    // Add the fund as the value of the "AccountId" element
                                    modelElement.Add(new XElement(propertyName, fund));
                                }
                                else
                                {
                                    modelElement.Add(new XElement(propertyName, propertyValue ?? "*"));
                                }

                            }
                            // Add the current date as an additional element
                            string currentDate = DateTime.Now.ToString("MM/dd/yyyy");
                            modelElement.Add(new XElement("CurrentDate", currentDate));
                            rootElement.Add(modelElement);
                        }
                    }
                    rootElement.Save(xmlFileName);
                }
                // Step 1: Transform XML using XSLT
                XMLUtilities.GetTransformedXML(xmlFileName, transformedXmlFileName, xsltFileName);


                // Step 2: Parse Transformed XML and Save to CSV
                ConvertTransformedXmlToCsvFile(transformedXmlFileName, filePath);

                System.Windows.MessageBox.Show("Columns exported successfully.", "Export Status", MessageBoxButton.OK, MessageBoxImage.Information);

                if (_isSendToFTPEnabled_RebalancerMainGrid)
                {
                    // Get FTP/SFTP details from config to send the file.
                    string host = System.Configuration.ConfigurationManager.AppSettings["Host"];
                    string username = System.Configuration.ConfigurationManager.AppSettings["Username"];
                    string password = System.Configuration.ConfigurationManager.AppSettings["Password"];
                    string privateKeyPath = System.Configuration.ConfigurationManager.AppSettings["PrivateKeyPath"];
                    string passphrase = System.Configuration.ConfigurationManager.AppSettings["Passphrase"];
                    string directory = System.Configuration.ConfigurationManager.AppSettings["Directory"];
                    string protocol = System.Configuration.ConfigurationManager.AppSettings["Protocol"];

                    Utilities.FTP.FtpUploader.UploadFileToFtp(filePath, host, username, password, privateKeyPath, passphrase, directory, protocol);
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

        private static string GetFilePathBuySellGrid()
        {
            string exportedFilePath = string.Empty;
            string filePath = string.Empty;

            try
            {
                if (_isCSVExportEnabledForRebalancer)
                {
                    if (ConfigurationManager.AppSettings["CSVExportFilePath_Rebalancer"] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("CSVExportFilePath_Rebalancer")))
                    {
                        exportedFilePath = ConfigurationManager.AppSettings["CSVExportFilePath_Rebalancer"].ToString();
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

        private static void OnExportGridDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                {
                    return;
                }
                XamDataGridExporterForRebalancer xamGridExporter = (XamDataGridExporterForRebalancer)d;
                XamDataGrid xamDataGrid = xamGridExporter.AssociatedObject;
                var folderPath = Path.GetDirectoryName(xamGridExporter.ExportFilePath);
                if (!System.IO.Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                _exporter = xamGridExporter.ExporterResource;
                WorkbookFormat format = SetWorkBookFormat(Path.GetExtension(xamGridExporter.ExportFilePath));
                ExportOptions options = new ExportOptions();
                _exporter.Export(xamGridExporter.AssociatedObject, xamGridExporter.ExportFilePath, format, options);
                xamGridExporter.ExportGridData = false;
                _exporter = null;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private static string GetFilePathMainRebalancerGrid()
        {
            string exportedFilePath = string.Empty;
            string filePath = string.Empty;

            try
            {
                if (_isCSVExportEnabledForRebalancer)
                {
                    if (ConfigurationManager.AppSettings["CSVExportFilePath_RebalancerMainGrid"] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("CSVExportFilePath_RebalancerMainGrid")))
                    {
                        exportedFilePath = ConfigurationManager.AppSettings["CSVExportFilePath_RebalancerMainGrid"].ToString();
                    }

                    if (string.IsNullOrWhiteSpace(exportedFilePath) || (!Directory.Exists(Path.GetDirectoryName(exportedFilePath))))
                    {
                        filePath = OpenSaveDialogBox();
                    }
                    else
                    {
                        //var dateWithCurrentTime = DateTime.Now.Date.AddTicks(DateTime.Now.TimeOfDay.Ticks);
                        //string dateFormat = dateWithCurrentTime.ToString("MMddyyyyhhmmss");

                        string fileName = Path.GetFileName(exportedFilePath);
                        string fileNameTobeSavedAndDisplay = string.Empty;
                        string strFileNameAfterClosingBraces = string.Empty;

                        //check if filename contains open and close braces then cast between value in the datetime format
                        int startIndex = fileName.IndexOf("{");
                        int lastIndex = fileName.LastIndexOf("}");
                        if (fileName.Contains("{") || fileName.Contains("}"))
                        {
                            int lengthOfFile = (lastIndex - startIndex) - 1;
                            string FileDateFormat = fileName.Substring(startIndex + 1, lengthOfFile);

                            string strFileNameBeforeStartBraces = fileName.Substring(0, fileName.IndexOf("{"));
                            string strFileNameBeforeClosingBraces = fileName.Substring(0, fileName.IndexOf("}"));
                            strFileNameAfterClosingBraces = fileName.Substring(strFileNameBeforeClosingBraces.Length + 1);

                            var txtDayLightSaving = System.DateTime.Now;
                            var dateWithCurrentTime = txtDayLightSaving.Date.AddTicks(DateTime.Now.TimeOfDay.Ticks);
                            string DateFormat = dateWithCurrentTime.ToString(FileDateFormat);

                            fileNameTobeSavedAndDisplay = strFileNameBeforeStartBraces + DateFormat + strFileNameAfterClosingBraces;
                            filePath = Path.GetDirectoryName(exportedFilePath) + "\\" + fileNameTobeSavedAndDisplay;
                        }
                        else
                        {
                            var dateWithCurrentTime = DateTime.Now.Date.AddTicks(DateTime.Now.TimeOfDay.Ticks);
                            string dateFormat = dateWithCurrentTime.ToString("MMddyyyyhhmmss");

                            string fileExtension = Path.GetExtension(exportedFilePath);

                            string fileNameWithoutExtension = fileName.Substring(0, fileName.Length - fileExtension.Length);

                            fileName = fileNameWithoutExtension + "_" + dateFormat + fileExtension;

                            filePath = Path.GetDirectoryName(exportedFilePath) + "\\" + fileName;
                        }

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

        private static void ConvertTransformedXmlToCsvFile(string xmlTransformedPath, string csvOutputPath)
        {
            XDocument transformedXml = XDocument.Load(xmlTransformedPath);

            using (StreamWriter writer = new StreamWriter(csvOutputPath))
            {
                // Extract column headers from the first RebalancerModel element
                var firstModel = transformedXml.Descendants("RebalancerModel").FirstOrDefault();

                var columnHeaders = firstModel.Elements().Select(e => e.Value).ToList();
                writer.WriteLine(string.Join(",", columnHeaders));

                // Extract data rows from all subsequent RebalancerModel elements
                foreach (var model in transformedXml.Descendants("RebalancerModel").Skip(1))
                {
                    var rowValues = model.Elements().Select(e => e.Value).ToList();
                    writer.WriteLine(string.Join(",", rowValues));
                }
            }
        }
        private static void _exporter_SummaryRowExporting(object sender, SummaryRowExportingEventArgs e)
        {
            e.Cancel = true;
        }

        private static void ExporterOnExportStartedForTradeList(object sender, ExportStartedEventArgs e)
        {
            e.DataPresenter.FieldLayouts[0].Fields["IsChecked"].Visibility =
                     Visibility.Collapsed;
        }

        private static void ExporterOnExportStarted(object sender, ExportStartedEventArgs e)
        {
            e.DataPresenter.FieldLayouts[0].Fields[RebalancerConstants.CONST_IsSymbolValid].Visibility =
                     Visibility.Collapsed;
        }

        static void _exporter_CellExported(object sender, CellExportedEventArgs e)
        {
            try
            {
                if (e.Field != null)
                {
                    if (e.Field.Name.Equals(RebalancerConstants.CONST_IsLock))
                    {
                        string displayValue = String.Empty;
                        displayValue = (bool)e.Record.Cells[e.Field.Name].Value ? RebalancerConstants.CONST_Locked : RebalancerConstants.CONST_UnLocked;
                        e.CurrentWorksheet.Rows[e.CurrentRowIndex].Cells[e.CurrentColumnIndex].Value = displayValue;
                    }
                    if (e.Field.Name.Equals(RebalancerConstants.COL_TargetPercentage))
                    {
                        string displayValue = String.Empty;
                        displayValue = string.Format("{0:0.00}", e.Record.Cells[e.Field.Name].Value);
                        e.CurrentWorksheet.Rows[e.CurrentRowIndex].Cells[e.CurrentColumnIndex].Value = displayValue;
                    }
                    if (e.Field.Name.Equals("BuySellValue"))
                    {
                        e.CurrentWorksheet.Rows[e.CurrentRowIndex].Cells[e.CurrentColumnIndex].Value = Math.Round(Convert.ToDecimal(e.Record.Cells[e.Field.Name].Value));
                        e.CurrentWorksheet.Rows[e.CurrentRowIndex].Cells[e.CurrentColumnIndex].CellFormat.FormatString = "#,##0";
                    }
                    if (e.Field.Name.Equals("Price"))
                    {
                        e.CurrentWorksheet.Rows[e.CurrentRowIndex].Cells[e.CurrentColumnIndex].CellFormat.FormatString = "#,##0.00";
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
                _exporter.ExportStarted -= ExporterOnExportStarted;
                _exporter.ExportStarted -= ExporterOnExportStartedForTradeList;
                _exporter.SummaryRowExporting -= _exporter_SummaryRowExporting;
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

    }
}
