using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.ImportExportUtilities;
using Prana.Utilities.XMLUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
namespace Prana.ReconciliationNew
{
    public class ReconUtilities
    {
        static ProxyBase<IPranaPositionServices> _pranaPositionServices = null;
        private const string Col_BloombergExCode = "BloombergSymbolWithExchangeCode";
        private const string Header_BloombergExCode = "BloombergSymbol(WithExchangeCode)";
        private const string Col_NirvanaBloombergExCode = "NirvanaBloombergSymbolWithExchangeCode";
        private const string Col_BrokerBloombergExCode = "BrokerBloombergSymbolWithExchangeCode";
        private const string Header_NirvanaBloombergExCode = "Nirvana BloombergSymbol(WithExchangeCode)";
        private const string Header_BrokerBloombergExCode = "Broker BloombergSymbol(WithExchangeCode)";

        public static void CreatePositionServicesProxy()
        {
            try
            {
                if (_pranaPositionServices == null)
                {
                    _pranaPositionServices = new ProxyBase<IPranaPositionServices>("TradePositionServiceEndpointAddress");
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
        /// Set Accounts to be locked for current user
        /// Returns bool (true if all account are locked else false)
        /// </summary>
        /// <param name="accountsToBeLocked"></param>
        /// <returns></returns>
        public static bool SetAccountsLockStatus(List<int> accountsToBeLocked)
        {
            bool isSucessuful = false;
            try
            {
                //GetUserPermittedCompanyList
                int userID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                if (_pranaPositionServices == null)
                {
                    CreatePositionServicesProxy();
                }
                isSucessuful = _pranaPositionServices.InnerChannel.SetAccountsLockStatus(userID, accountsToBeLocked);
                if (isSucessuful)
                {
                    CachedDataManager.GetInstance.UpdateAccountLockData(accountsToBeLocked);
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
            return isSucessuful;
        }
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
                if (gridExceptions.DisplayLayout.Bands[0].Columns.Exists(Col_NirvanaBloombergExCode) ||
                     gridExceptions.DisplayLayout.Bands[0].Columns.Exists(Col_BrokerBloombergExCode))
                {
                    gridExceptions.DisplayLayout.Bands[0].Columns[Col_NirvanaBloombergExCode].Header.Caption = Header_NirvanaBloombergExCode;
                    gridExceptions.DisplayLayout.Bands[0].Columns[Col_BrokerBloombergExCode].Header.Caption = Header_BrokerBloombergExCode;
                }
               gridExceptions = ApplyGroupingAndSorting(ExpReportFormat, selectedColumnList, sortByColumnList, groupByColumnList, gridExceptions);
                //added by amit 09.04.2015
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-7135
                //SetThousandSeparatorFormat(gridExceptions, templateKey);
                UltraGridFileExporter.ExportFile(gridExceptions, exceptionFileName, ExpReportFormat);
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


            //  return exceptionReport;
        }
        /// <summary>
        /// writes the dictionary to file of Recon Files and Amendments left in the file
        /// </summary>
        public static void WriteAmendmentDictionary(SerializableDictionary<string, int> amendments)
        {
            try
            {
                //Recon Amendmend File Path
                string amendmentsFilePath = Application.StartupPath + "\\" + ApplicationConstants.RECON_DATA_DIRECTORY + "\\" + ApplicationConstants.RECON_AmendmentsFileName;

                using (XmlTextWriter writer = new XmlTextWriter(amendmentsFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(SerializableDictionary<string, int>));
                    serializer.Serialize(writer, amendments);
                    writer.Flush();
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
        /// read the file and store in dictionary of Recon Files and No. of Amendments in them
        /// </summary>
        public static SerializableDictionary<string, int> LoadAmendmentDictionary()
        {
            SerializableDictionary<string, int> amendmends = new SerializableDictionary<string, int>();
            try
            {
                //Recon Amendmend File Path
                string amendmentsFilePath = Application.StartupPath + "\\" + ApplicationConstants.RECON_DATA_DIRECTORY + "\\" + ApplicationConstants.RECON_AmendmentsFileName;
                if (File.Exists(amendmentsFilePath))
                {
                    using (FileStream fs = File.OpenRead(amendmentsFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<string, int>));
                        amendmends = (SerializableDictionary<string, int>)serializer.Deserialize(fs);
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
            return amendmends;
        }
        public static UltraGrid ApplyGroupingAndSorting(AutomationEnum.FileFormat ExpReportFormat, List<ColumnInfo> selectedColumnList, List<ColumnInfo> sortByColumnList, List<ColumnInfo> groupByColumnList, UltraGrid gridExceptions)
        {
            try
            {
                if (gridExceptions != null && selectedColumnList != null && sortByColumnList != null && groupByColumnList != null)
                {
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

                    if (groupByColumnList.Count > 0 && (ExpReportFormat == AutomationEnum.FileFormat.xls || ExpReportFormat == AutomationEnum.FileFormat.pdf))
                    {
                        //grouping logic
                        List<string> lstColumns = new List<string>();
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
                            if (!string.IsNullOrWhiteSpace(columnName))
                            {
                                lstColumns.Add(columnName);
                            }

                        }
                        UltraGridGroupHelper.Group(gridExceptions, lstColumns);
                    }
                    if (sortByColumnList.Count > 0)
                    {
                        //logic for sorting
                        //allow multiple sorting
                        Dictionary<string, SortingOrder> dictSortColumns = new Dictionary<string, SortingOrder>();
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

                            if (!string.IsNullOrWhiteSpace(columnName))
                            {
                                if (!dictSortColumns.ContainsKey(columnName))
                                {
                                    dictSortColumns.Add(columnName, sortByColumn.SortOrder);
                                }
                            }
                        }
                        UltraGridBaseSorter.Sort(gridExceptions, dictSortColumns);
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

                    return gridExceptions;
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
            return null;
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //Todo: Remove this method, because enritching will be done using transformation
        //public static void AddCustomColumns(DataTable dt, List<ColumnInfo> listColumns)
        //{
        //    try
        //    {
        //        foreach (ColumnInfo column in listColumns)
        //        {
        //            if (!string.IsNullOrEmpty(column.ColumnName) && column.IsSelected && !string.IsNullOrEmpty(column.FormulaExpression))
        //            {
        //                DataColumn customColumn = new DataColumn();
        //                customColumn.ColumnName = column.ColumnName;
        //                customColumn.Expression = column.FormulaExpression;
        //                dt.Columns.Add(customColumn);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}


        /// <summary>
        /// update exception report layout columns for template
        /// </summary>
        /// <param name="template"></param>
        /// <param name="NirvanaColumns"></param>
        /// <param name="PBColumns"></param>
        public static void UpdateExceptionReportLayout(ReconTemplate template, List<string> NirvanaColumns, List<string> PBColumns, List<string> CommonColumns)
        {
            try
            {
                if (template != null && NirvanaColumns != null && PBColumns != null && CommonColumns != null)
                {
                    List<string> allColumns = NirvanaColumns.Union(PBColumns).ToList().Union(CommonColumns).ToList();
                    List<string> addedExceptionReportColumns = AddExistingColumnsInTemplate(template, allColumns);

                    #region   Commented
                    ///    //remove column that is in available, remaining new columns will be added in the lower part
                    ///    if (NirvanaColumns.Contains(column.ColumnName) && (column.GroupType.Equals(ColumnGroupType.Nirvana) || column.GroupType.Equals(ColumnGroupType.Common) || column.GroupType.Equals(ColumnGroupType.Diff)))
                    ///    {
                    ///        if (template.RulesList[0].NumericFields.Contains(column.ColumnName) && column.GroupType.Equals(ColumnGroupType.Diff))
                    ///            NirvanaColumns.Remove(column.ColumnName);
                    ///    }
                    ///    if (PBColumns.Contains(column.ColumnName) && (column.GroupType.Equals(ColumnGroupType.PrimeBroker) || column.GroupType.Equals(ColumnGroupType.Common) || column.GroupType.Equals(ColumnGroupType.Diff)))
                    ///        PBColumns.Remove(column.ColumnName);
                    ///}
                    #endregion

                    //add newly added columns in the available column list of exception report columns
                    foreach (string columnName in allColumns)
                    {
                        if (!addedExceptionReportColumns.Contains(columnName))
                        {
                            if (NirvanaColumns.Contains(columnName))
                            {
                                template.AvailableColumnList.Add(GetColumnInfo(columnName, ColumnGroupType.Nirvana));
                            }
                            if (PBColumns.Contains(columnName))
                            {
                                template.AvailableColumnList.Add(GetColumnInfo(columnName, ColumnGroupType.PrimeBroker));
                            }
                            if (CommonColumns.Contains(columnName))
                            {
                                template.AvailableColumnList.Add(GetColumnInfo(columnName, ColumnGroupType.Common));
                            }
                            if (NirvanaColumns.Contains(columnName) && PBColumns.Contains(columnName))
                            {
                                if (template.RulesList.Count > 0 && template.RulesList[0].NumericFields.Contains(columnName))
                                {
                                    template.AvailableColumnList.Add(GetColumnInfo(columnName, ColumnGroupType.Diff));
                                }
                            }
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
        /// Add Existing Columns In Template
        /// </summary>
        /// <param name="template"></param>
        /// <param name="allColumns"></param>
        /// <returns></returns>
        private static List<string> AddExistingColumnsInTemplate(ReconTemplate template, List<string> allColumns)
        {
            List<string> addedExceptionReportColumns = new List<string>();
            try
            {
                UpdateColumnsInList(template.ListGroupByColumns, allColumns, null);
                UpdateColumnsInList(template.ListSortByColumns, allColumns, null);
                UpdateColumnsInList(template.SelectedColumnList, allColumns, addedExceptionReportColumns);
                UpdateColumnsInList(template.AvailableColumnList, allColumns, addedExceptionReportColumns);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return addedExceptionReportColumns;
        }

        /// <summary>
        /// Update Columns In List
        /// </summary>
        /// <param name="listTemplateColumns"></param>
        /// <param name="allColumns"></param>
        /// <param name="addedExceptionReportColumns"></param>
        private static void UpdateColumnsInList(List<ColumnInfo> listTemplateColumns, List<string> allColumns, List<string> addedExceptionReportColumns)
        {
            try
            {
                List<ColumnInfo> listColumns = new List<ColumnInfo>();
                listColumns.AddRange(listTemplateColumns);
                listTemplateColumns.Clear();
                foreach (ColumnInfo column in listColumns)
                {
                    if (allColumns.Contains(column.ColumnName))
                    {
                        listTemplateColumns.Add(column);
                    }
                    if (addedExceptionReportColumns != null && !addedExceptionReportColumns.Contains(column.ColumnName))
                    {
                        addedExceptionReportColumns.Add(column.ColumnName);
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
        /// Get Column Info
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="groupType"></param>
        /// <returns></returns>
        private static ColumnInfo GetColumnInfo(string columnName, ColumnGroupType groupType)
        {
            ColumnInfo CommonColumn = new ColumnInfo();
            try
            {
                CommonColumn.GroupType = groupType;
                CommonColumn.ColumnName = columnName;
                CommonColumn.FormulaExpression = string.Empty;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return CommonColumn;
        }

        public static void SetGridColumns(UltraGrid grid, List<string> listColumns)
        {
            try
            {
                if (grid.DisplayLayout.Bands[0].Columns.Exists(Col_BloombergExCode))
                {
                    grid.DisplayLayout.Bands[0].Columns[Col_BloombergExCode].Header.Caption = Header_BloombergExCode;
                }
                List<string> listSkipFormatColumns = ReconPrefManager.ReconPreferences.GetSkipFormatColumnsList();

                if (grid != null && listColumns != null)
                {
                    Infragistics.Win.UltraWinGrid.ColumnsCollection columns = grid.DisplayLayout.Bands[0].Columns;
                    if (listColumns != null)
                    {
                        //Hide all columns
                        foreach (UltraGridColumn col in columns)
                        {
                            columns[col.Key].Hidden = true;
                            if (!(listSkipFormatColumns != null && listSkipFormatColumns.Contains(col.Key)))
                            {
                                columns[col.Key].Format = "N4";
                            }

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
                                if (!(listSkipFormatColumns != null && listSkipFormatColumns.Contains(column.Key)))
                                {
                                    columns[column.Key].Format = "N4";
                                }

                                column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                                visiblePosition++;
                            }
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

        #region commented
        //This method is related to reconcile logic, so moved to DataReconciler
        //public static DataTable GetExceptionsDataTableSchema(List<ColumnInfo> selectedColumnList)
        //{
        //    DataTable dtExceptions = new DataTable();
        //    try
        //    {
        //        foreach (ColumnInfo column in selectedColumnList)
        //        {
        //            //if (!comparisonColumns.Contains(column.ColumnName))
        //            //{

        //            switch (column.GroupType)
        //            {
        //                case ColumnGroupType.Nirvana:
        //                    string NirvanaColumnKey = "Nirvana" + column.ColumnName;

        //                    string NirvanaCaption = "Nirvana" +" " + column.ColumnName;

        //                    DataColumn NirvanaColumn = new DataColumn(NirvanaColumnKey);
        //                    NirvanaColumn.Caption = NirvanaCaption;

        //                    dtExceptions.Columns.Add(NirvanaColumn);

        //                    break;
        //                case ColumnGroupType.PrimeBroker:

        //                    string BrokerColumnKey = "Broker" + column.ColumnName;

        //                    string BrokerCaption = "Broker" + " " + column.ColumnName;

        //                    DataColumn BrokerColumn = new DataColumn(BrokerColumnKey);
        //                    BrokerColumn.Caption = BrokerCaption;

        //                    dtExceptions.Columns.Add(BrokerColumn);
        //                    break;
        //                case ColumnGroupType.Common:
        //                    string CommonColumnKey = column.ColumnName;
        //                    DataColumn CommonColumn = new DataColumn(CommonColumnKey);

        //                    dtExceptions.Columns.Add(CommonColumn);
        //                    break;
        //                case ColumnGroupType.Both:

        //                    break;
        //                case ColumnGroupType.Diff:
        //                    string DiffColumnKey = "Diff" + column.ColumnName;
        //                    string DiffCaption = "Diff" + '_' + column.ColumnName;
        //                    DataColumn diffColumn = new DataColumn(DiffColumnKey);
        //                    diffColumn.Caption = DiffCaption;
        //                    dtExceptions.Columns.Add(diffColumn);
        //                    break;
        //                default:
        //                    break;
        //            }

        //            //}
        //        }
        //        if (!dtExceptions.Columns.Contains(ReconConstants.MismatchType))
        //        {
        //            dtExceptions.Columns.Add(ReconConstants.MismatchType);
        //        }
        //        if (!dtExceptions.Columns.Contains(ReconConstants.Matched))
        //        {
        //            dtExceptions.Columns.Add(ReconConstants.Matched);
        //        }

        //        if (dtExceptions.Columns.Contains("MasterFund"))
        //        {
        //            dtExceptions.Columns["MasterFund"].Caption = "Account";
        //        }
        //        if (dtExceptions.Columns.Contains("AccountName"))
        //        {
        //            dtExceptions.Columns["AccountName"].Caption = "Account";
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //    return dtExceptions;
        //}
        #endregion

        public static void SaveTransformedPBData(DataSet dsPBData, string dirName, string FileName)
        {
            try
            {
                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                }
                XMLUtilities.WriteXML(dsPBData, FileName);
                // dsPbdata.WriteXml(FileName);
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

        public static DataSet GetCorrespondingTransformedPBData(ReconParameters reconParameters)
        {
            DataSet dsPB = null;
            try
            {
                if (reconParameters != null)
                {
                    if (!string.IsNullOrEmpty(reconParameters.TemplateKey))
                    {
                        //get xml and xslt file name for recon template setupName and xsltname
                        string XsltFileName = ReconPrefManager.ReconPreferences.GetXsltFileName(reconParameters.TemplateKey);

                        if (!string.IsNullOrWhiteSpace(XsltFileName))
                        {
                            string fileNameToSearch = "Recon" + Seperators.SEPERATOR_6 + reconParameters.FromDate + Seperators.SEPERATOR_6 + reconParameters.ToDate;
                            ReconTemplate template = ReconPrefManager.ReconPreferences.GetTemplates(reconParameters.TemplateKey);

                            if (template != null)
                            {
                                string reconXmlDataDirectoryPath = ReconUtilities.GetReconDirectoryPath(ReconConstants.ReconDataDirectoryPath, reconParameters);
                                string searchDir = reconXmlDataDirectoryPath + "\\xmls\\Transformation\\Temp";

                                if (Directory.Exists(searchDir))
                                {
                                    DirectoryInfo dirInfo = new DirectoryInfo(searchDir);
                                    FileInfo[] files = dirInfo.GetFiles();
                                    for (int i = 0; i < files.Length; i++)
                                    {
                                        FileInfo info = files[i];
                                        string name = (info.Name.Replace(info.Extension, ""));

                                        if (name.Equals(fileNameToSearch))
                                        {
                                            dsPB = new DataSet();
                                            //CHMW-2181	[Reconciliation] [Code Review] Replace dataset read xml with BufferedStream
                                            //dsPB.ReadXml(info.FullName);
                                            dsPB = XMLUtilities.ReadXmlUsingBufferedStream(info.FullName);
                                            dsPB.AcceptChanges();
                                            break;
                                        }
                                    }
                                }
                            }
                            return dsPB;
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

            return null;
        }

        public static string GetCommaSeparatedAssetIds(string templateKey)
        {
            string commaSeparatedAssetIDs = string.Empty;
            try
            {
                Dictionary<ReconFilterType, Dictionary<int, string>> dictReconFilters = ReconPrefManager.ReconPreferences.GetReconFilters(templateKey);
                if (dictReconFilters.ContainsKey(ReconFilterType.Asset))
                {
                    List<int> listAssetIDs = new List<int>(dictReconFilters[ReconFilterType.Asset].Keys);
                    commaSeparatedAssetIDs = string.Join(",", listAssetIDs.ToArray());
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
            return commaSeparatedAssetIDs;
        }

        public static StringBuilder GetCommaSeparatedAccountIds(string templateName)
        {
            StringBuilder commaSeparatedAccountIDs = new StringBuilder();
            try
            {
                Dictionary<ReconFilterType, Dictionary<int, string>> dictReconFilters = ReconPrefManager.ReconPreferences.GetReconFilters(templateName);

                if (dictReconFilters.ContainsKey(ReconFilterType.Account) || dictReconFilters.ContainsKey(ReconFilterType.MasterFund) || dictReconFilters.ContainsKey(ReconFilterType.PrimeBroker))
                {
                    //  commaSeparatedAccountIDs = new StringBuilder();
                    List<int> listAccountFilter = new List<int>();
                    if (dictReconFilters.ContainsKey(ReconFilterType.Account))
                    {
                        List<int> listAccountIDs = new List<int>(dictReconFilters[ReconFilterType.Account].Keys);
                        if (listAccountFilter.Count >= listAccountIDs.Count)
                        {
                            listAccountFilter.Clear();
                        }
                        if (listAccountFilter.Count == 0)
                        {
                            listAccountFilter.AddRange(listAccountIDs);
                        }
                    }
                    if (dictReconFilters.ContainsKey(ReconFilterType.MasterFund))
                    {
                        List<int> listSubAccountIDs = new List<int>();
                        // StringBuilder accountIDs = new StringBuilder();
                        Dictionary<int, List<int>> dictMasterFundSubAccountAssociation = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                        List<int> listMasterFundIDs = new List<int>(dictReconFilters[ReconFilterType.MasterFund].Keys);
                        foreach (int masterFundID in listMasterFundIDs)
                        {
                            if (dictMasterFundSubAccountAssociation.ContainsKey(masterFundID))
                            {
                                listSubAccountIDs.AddRange(dictMasterFundSubAccountAssociation[masterFundID]);
                            }
                        }
                        if (listAccountFilter.Count >= listSubAccountIDs.Count)
                        {
                            listAccountFilter.Clear();
                            // listAccountFilter.AddRange(listSubAccountIDs);
                        }
                        if (listAccountFilter.Count == 0)
                        {
                            listAccountFilter.AddRange(listSubAccountIDs);
                        }
                    }
                    //if (dictReconFilters.ContainsKey(ReconFilterType.PrimeBroker))
                    //{
                    //    List<int> listSubAccountIDs = new List<int>();
                    //    // StringBuilder accountIDs = new StringBuilder();
                    //    Dictionary<int, List<int>> dictDataSourceSubAccountAssociation = CachedDataManager.GetInstance.GetDataSourceSubAccountAssociation();
                    //    List<int> thirdPartyIDs = new List<int>(dictReconFilters[ReconFilterType.PrimeBroker].Keys);
                    //    foreach (int thirdPartyID in thirdPartyIDs)
                    //    {
                    //        if (dictDataSourceSubAccountAssociation.ContainsKey(thirdPartyID))
                    //        {
                    //            listSubAccountIDs.AddRange(dictDataSourceSubAccountAssociation[thirdPartyID]);
                    //        }
                    //    }
                    //    if (listAccountFilter.Count >= listSubAccountIDs.Count)
                    //    {
                    //        listAccountFilter.Clear();
                    //        //listAccountFilter.AddRange(listSubAccountIDs);
                    //    }
                    //    if (listAccountFilter.Count == 0)
                    //    {
                    //        listAccountFilter.AddRange(listSubAccountIDs);
                    //    }
                    //}
                    foreach (int accountID in listAccountFilter)
                    {
                        commaSeparatedAccountIDs.Append(accountID);
                        commaSeparatedAccountIDs.Append(',');
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
            return commaSeparatedAccountIDs;
        }

        public static DataSet FetchDataForGivenSPName(ReconParameters reconParameters, string commaSeparatedAssetIDs, string commaSeparatedAccountIDs)
        {
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                if (_pranaPositionServices == null)
                {
                    CreatePositionServicesProxy();
                }
                //Modified by omshiv, fetch data based on NirvanaProcessDate
                //TODO - set on config for true/ false
                // Boolean isConsiderNirvanaProcessDate = true;

                ds1 = _pranaPositionServices.InnerChannel.FetchDataForGivenSpName(reconParameters, commaSeparatedAssetIDs, commaSeparatedAccountIDs);

                //Modified By: Pranay Deep 22nd Oct 2015
                //Filter data on the basis of check state of checkbox "Show CA Genetated Trades"
                //Also this is only for recon type is "Transaction"
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-11773
                if ((Convert.ToInt32(reconParameters.IsShowCAGeneratedTrades) == 0) && (reconParameters.ReconType).Equals((ReconType.Transaction).ToString()) && (ds1.Tables[0].Rows.Count > 0))
                {
                    dt = ds1.Tables[0];
                    dt.TableName = "DataTable1";
                    dt = dt.Select(
                                   "TransactionSource <> " + ((int)(TransactionSource.CAStockDividend)).ToString()
                                   + " AND " +
                                   "TransactionSource <> " + ((int)TransactionSource.CAStockMerger).ToString()
                                   + " AND " +
                                   "TransactionSource <> " + ((int)TransactionSource.CAStockNameChange).ToString()
                                   + " AND " +
                                   "TransactionSource <> " + ((int)TransactionSource.CAStockSpinoff).ToString()
                                   ).CopyToDataTable();

                    ds.Tables.Add(dt);
                    ds1.Clear();
                    ds1 = ds;
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
            return ds1;
        }

        //todo: New method GetReconFilePath have been created
        //This method can be removed
        //public static string GetExceptionFilePath(string templateName, ReconTemplate template, DateTime selectedDate)
        //{
        //    string exceptionFileName = string.Empty;
        //    try
        //    {
        //        string reconTemplatePath = string.Empty;
        //        if (!string.IsNullOrEmpty(template.ExceptionReportSavePath))
        //        {
        //            reconTemplatePath = template.ExceptionReportSavePath + @"\" + templateName;
        //        }
        //        else
        //        {
        //            reconTemplatePath = ReconConstants.ReconDataDirectoryPath + @"\" + templateName;
        //        }
        //        string reconTemplatePathWithDate = reconTemplatePath + @"\" + selectedDate.ToString("MM-dd-yyyy");
        //        if (!Directory.Exists(ReconConstants.ReconDataDirectoryPath))
        //        {
        //            Directory.CreateDirectory(ReconConstants.ReconDataDirectoryPath);
        //        }
        //        if (!Directory.Exists(reconTemplatePath))
        //        {
        //            Directory.CreateDirectory(reconTemplatePath);
        //        }
        //        if (!Directory.Exists(reconTemplatePathWithDate))
        //        {
        //            Directory.CreateDirectory(reconTemplatePathWithDate);
        //        }
        //        exceptionFileName = GetPath(templateName + '_' + selectedDate.ToString("MM-dd-yyyy"), reconTemplatePathWithDate);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return exceptionFileName;
        //}
        //todo: this method is used in GetExceptionFilePath method which is obsolete
        //This method can be removed
        private static string GetPath(string fileName, string ReconTemplateName)
        {
            return ReconTemplateName + @"\" + fileName;
        }


        public static string GetTemplateNameFromTemplateKey(string key)
        {
            string templateName = string.Empty;
            try
            {
                if (key != null)
                {
                    string[] arr = key.Split(Seperators.SEPERATOR_6);
                    templateName = arr[arr.Length - 1];
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
            return templateName;
        }

        /// <summary>
        /// Get client ID from template key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        public static string GetClientIDFromTemplateKey(string key)
        {
            string clientID = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    string[] arr = key.Split(Seperators.SEPERATOR_6);
                    clientID = arr[0];
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
            return clientID;
        }


        public static string GetTemplateKeyFromParameters(string clientID, string strReconType, string templateName)
        {
            string templateKey = string.Empty;
            try
            {
                templateKey = clientID + Seperators.SEPERATOR_6 + strReconType + Seperators.SEPERATOR_6 + templateName;
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
            return templateKey;
        }

        public static void CreateDirectoryIfNotExists(string filePath)
        {
            try
            {
                //check that directory path exists
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!System.IO.Directory.Exists(directoryPath))
                {
                    System.IO.Directory.CreateDirectory(directoryPath);
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

        public static string GetReconDirectoryPath(string rootDirectoryPath, ReconParameters reconParameters)
        {
            StringBuilder reconFilePath = new StringBuilder(rootDirectoryPath);
            try
            {
                if (reconParameters != null && !string.IsNullOrEmpty(rootDirectoryPath))
                {
                    reconFilePath.Append(@"\" + reconParameters.ClientID + @"\" + reconParameters.ReconType + @"\" + reconParameters.TemplateName);
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
            return reconFilePath.ToString();
        }

        /// <summary>
        /// Gets file name from parameters
        /// extra field not compulsory as not available in old recon
        /// </summary>
        /// <param name="rootDirectoryPath"></param>
        /// <param name="clientID"></param>
        /// <param name="reconType"></param>
        /// <param name="templateName"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="formatName"></param>
        /// <param name="runByDate"></param>
        /// <param name="runDate"></param>
        /// <returns></returns>
        public static string GetReconFilePath(string rootDirectoryPath, ReconParameters reconParameters)
        {
            StringBuilder reconFilePath = new StringBuilder();
            try
            {
                if (rootDirectoryPath != null && reconParameters != null)
                {
                    string reconDirectoryPath = GetReconDirectoryPath(rootDirectoryPath, reconParameters);
                    reconFilePath.Append(reconDirectoryPath);
                    reconFilePath.Append(@"\" + reconParameters.TemplateName + Seperators.SEPERATOR_6 + reconParameters.FromDate + Seperators.SEPERATOR_6 + reconParameters.ToDate);
                    //CHMW-2225	Rows on recon dashboard should be unique by the following characteristics: Format Name, Recon Type, Run Date, From Date, End Date, Date Type (trade date, process date, etc)
                    if (!string.IsNullOrEmpty(reconParameters.FormatName))
                    {
                        reconFilePath.Append(Seperators.SEPERATOR_6 + reconParameters.FormatName + Seperators.SEPERATOR_6 + reconParameters.ReconDateType.ToString() + Seperators.SEPERATOR_6 + reconParameters.RunDate);
                    }
                    else
                    {
                        //TODO : Handle else Condition here
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
            return reconFilePath.ToString();
        }
        /// <summary>
        /// check if Value Exist In DataSet
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataRow ValueExistInDataSet(DataSet dataSet, string tableName, string columnName, string value)
        {
            try
            {
                if (dataSet != null && tableName != null && columnName != null && value != null)
                    if (dataSet.Tables.Count > 0
                        && dataSet.Tables.Contains(tableName)
                        && dataSet.Tables[tableName].Columns.Count > 0
                        && dataSet.Tables[tableName].Columns.Contains(columnName))
                    {
                        DataRow[] rows = dataSet.Tables[tableName].Select(columnName + " = '" + value + "'");
                        if (rows.Length > 0)
                        {
                            return rows[0];
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
            return null;
        }

        /// <summary>
        /// get the relative file path of the file to be saved in recon amendments file from full file name
        /// </summary>
        /// <param name="filePathWithExtension"></param>
        /// <returns></returns>
        public static string GetRelativeExceptionFilePath(string filePath)
        {
            try
            {
                if (filePath != null && filePath.Length > ApplicationConstants.RECON_DATA_DIRECTORY.Length)
                {
                    return filePath.Substring(filePath.IndexOf(ApplicationConstants.RECON_DATA_DIRECTORY) - 1);
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
            return string.Empty;
        }

        /// <summary>
        /// update on server if recon preference is updated
        /// this is done to prompt other user if preference is altered
        /// </summary>
        public static void ReconPreferenceSaved()
        {
            try
            {
                int userID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                if (_pranaPositionServices == null)
                {
                    CreatePositionServicesProxy();
                }
                _pranaPositionServices.InnerChannel.ReconPreferenceSaved(userID);

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
        /// SaveTaxlotWorkFlowStates
        /// </summary>
        /// <param name="dt">Recon Datatable</param>
        public static void SaveTaxLotWorkFlowStates(DataTable dt)
        {
            try
            {
                if (dt != null && dt.Columns.Contains("TaxLotID"))
                {
                    DataTable workflowStatDT = new DataTable();
                    workflowStatDT.Columns.Add("TaxLotID");
                    workflowStatDT.Columns.Add("WorkflowStateID");
                    workflowStatDT.TableName = "workflowStates";
                    if (dt.Columns.Contains(ReconConstants.COLUMN_ChangedColumns))
                    {
                        //getting data that has reconciled
                        List<DataRow> reconRecords = dt.AsEnumerable().Where(X => (String.IsNullOrWhiteSpace(X[ReconConstants.COLUMN_ChangedColumns].ToString()) && String.IsNullOrWhiteSpace(X[ReconConstants.COLUMN_TaxLotStatus].ToString()))).Select(X => X).ToList();
                        if (reconRecords.Count > 0)
                        {
                            DataTable data = reconRecords.CopyToDataTable();

                            foreach (DataRow dtRow in data.Rows)
                            {

                                DataRow row = workflowStatDT.NewRow();
                                row["TaxLotID"] = dtRow["TaxLotID"];
                                row["WorkflowStateID"] = (int)NirvanaWorkFlowsStats.Reconciled1;
                                workflowStatDT.Rows.Add(row);
                            }

                            DatabaseManager.SaveTaxlotWorkFlowStates(workflowStatDT);
                        }
                    }
                    //get Data that have Mis matched or failed recon
                    DataTable reconFaileAndChangesdDT = workflowStatDT.Copy();
                    reconFaileAndChangesdDT.Clear();


                    bool isReconFailedRecordsUpdated = false;
                    if (dt.Columns.Contains(ReconConstants.CAPTION_MismatchType))
                    {
                        List<DataRow> reconFailedRecords = dt.AsEnumerable().Where(X => (!String.IsNullOrWhiteSpace(X[ReconConstants.CAPTION_MismatchType].ToString()) && X[ReconConstants.CAPTION_MismatchType].ToString().Contains("Mis"))).Select(X => X).ToList();

                        if (reconFailedRecords.Count > 0)
                        {
                            isReconFailedRecordsUpdated = true;
                            DataTable data = reconFailedRecords.CopyToDataTable();

                            foreach (DataRow dtRow in data.Rows)
                            {

                                DataRow row = reconFaileAndChangesdDT.NewRow();
                                row["TaxLotID"] = dtRow["TaxLotID"];
                                row["WorkflowStateID"] = (int)NirvanaWorkFlowsStats.FailedReconciliation;
                                reconFaileAndChangesdDT.Rows.Add(row);
                            }
                        }
                    }

                    //get Data that have changed
                    if (dt.Columns.Contains(ReconConstants.COLUMN_ChangedColumns))
                    {
                        List<DataRow> reconPendingRecords = dt.AsEnumerable().Where(X => (!String.IsNullOrWhiteSpace(X[ReconConstants.COLUMN_ChangedColumns].ToString()) || !String.IsNullOrWhiteSpace(X[ReconConstants.COLUMN_TaxLotStatus].ToString()))).Select(X => X).ToList();
                        if (reconPendingRecords.Count > 0)
                        {
                            isReconFailedRecordsUpdated = true;

                            DataTable data = reconPendingRecords.CopyToDataTable();

                            foreach (DataRow dtRow in data.Rows)
                            {

                                DataRow row = reconFaileAndChangesdDT.NewRow();
                                row["TaxLotID"] = dtRow["TaxLotID"];
                                row["WorkflowStateID"] = (int)NirvanaWorkFlowsStats.ReconPendingApproval;
                                reconFaileAndChangesdDT.Rows.Add(row);
                            }
                        }
                    }

                    if (isReconFailedRecordsUpdated)
                    {
                        //saving in DB
                        DatabaseManager.SaveTaxlotWorkFlowStates(reconFaileAndChangesdDT);
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

        /// <summary>
        /// SaveTaxlotWorkFlowStates from Approve and rescind click
        /// </summary>
        /// <param name="dictTaxLotsStates"></param>
        public static void SaveTaxLotWorkflowStates(ConcurrentDictionary<string, NirvanaWorkFlowsStats> dictTaxLotsStates)
        {
            try
            {
                DataTable workflowStatDT = new DataTable();
                workflowStatDT.Locale = System.Globalization.CultureInfo.CurrentCulture;
                workflowStatDT.Columns.Add("TaxLotID");
                workflowStatDT.Columns.Add("WorkflowStateID");
                workflowStatDT.TableName = "workflowStates";

                Parallel.ForEach(dictTaxLotsStates, item =>
                    {

                        DataRow row = workflowStatDT.NewRow();
                        row["TaxLotID"] = item.Key;
                        row["WorkflowStateID"] = (int)item.Value;
                        workflowStatDT.Rows.Add(row);

                    });


                DatabaseManager.SaveTaxlotWorkFlowStates(workflowStatDT);

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
        /// returns execution name from template name and recon file path
        /// </summary>
        /// <param name="reconTemplate"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetExecutionNameFromReconTemplateAndFilePath(ReconTemplate reconTemplate, string filePath)
        {
            string execname = string.Empty;
            try
            {
                if (reconTemplate != null && filePath != null)
                {
                    execname = reconTemplate.ClientID.ToString()
                        + Seperators.SEPERATOR_6 + reconTemplate.ReconType.ToString()
                        + Seperators.SEPERATOR_6 + Path.GetFileNameWithoutExtension(filePath);
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
            return execname;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_reconTemplate"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetDashboardFilePath(string clientID, string reconType, string templateName, string fromDate, string toDate, string formatName, string runByDate, DateTime runDate)
        {
            StringBuilder dashboardFile = new StringBuilder();
            try
            {
                dashboardFile.Append(System.Windows.Forms.Application.StartupPath + @"\DashBoardData\" + @"Recon\");
                dashboardFile.Append(runDate.ToString("yyyyMMdd") + @"\");
                dashboardFile.Append("Recon_-1_");
                dashboardFile.Append(clientID + Seperators.SEPERATOR_6);
                dashboardFile.Append(reconType + Seperators.SEPERATOR_6);
                dashboardFile.Append(templateName + Seperators.SEPERATOR_6 + fromDate + Seperators.SEPERATOR_6 + toDate);
                dashboardFile.Append(Seperators.SEPERATOR_6 + formatName + Seperators.SEPERATOR_6 + runByDate + Seperators.SEPERATOR_6 + runDate.ToString(ApplicationConstants.DateFormat));

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dashboardFile.ToString();
        }
        public static List<string> GetSelectedColumnsList(List<ColumnInfo> selectedColumnList)
        {
            List<string> selectedColumn = new List<string>();
            try
            {
                if (selectedColumnList != null)
                {
                    foreach (ColumnInfo column in selectedColumnList)
                    {
                        //append the column name to list with respective group name
                        switch (column.GroupType)
                        {
                            case Prana.BusinessObjects.AppConstants.ColumnGroupType.Nirvana:
                                selectedColumn.Add(ReconConstants.CONST_Nirvana + column.ColumnName);
                                break;
                            case Prana.BusinessObjects.AppConstants.ColumnGroupType.PrimeBroker:
                                selectedColumn.Add(ReconConstants.CONST_Broker + column.ColumnName);
                                break;
                            case Prana.BusinessObjects.AppConstants.ColumnGroupType.Diff:
                                selectedColumn.Add(ReconConstants.CONST_Diff + column.ColumnName);
                                break;
                            default:
                                selectedColumn.Add(column.ColumnName);
                                break;
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
            return selectedColumn;
        }
        /// <summary>
        /// Get List Of Numeric Fields
        /// </summary>
        /// <param name="listOfRules"></param>
        /// <returns></returns>
        /// //added by amit 09.04.2015
        ///http://jira.nirvanasolutions.com:8080/browse/PRANA-7135
        public static List<string> GetListOfNumericFields(MatchingRule listOfRules)
        {
            List<String> listOfNumericFields = new List<String>();
            try
            {

                foreach (String str in listOfRules.NumericFields)
                {
                    listOfNumericFields.Add(str);
                    listOfNumericFields.Add(ReconConstants.CONST_Nirvana + str);
                    listOfNumericFields.Add(ReconConstants.CONST_Broker + str);
                    listOfNumericFields.Add(ReconConstants.CONST_Diff + str);
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
            return listOfNumericFields;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedPreferences"></param>
        public void SaveReconPreferencesInDB(ReconPreferences updatedPreferences)
        {
            List<ReconPreference> lstReconPreference = new List<ReconPreference>();

            ReconTemplate template = new ReconTemplate();
            try
            {
                foreach (KeyValuePair<string, ReconTemplate> kp in updatedPreferences.DictReconTemplates)
                {
                    ReconPreference reconPref = new ReconPreference();
                    //kp.Value.IsShowCAGeneratedTrades = chkShowCAGeneratedTrades.CheckState == CheckState.Checked ? true : false;

                    if (kp.Value.IsDirtyForSaving)
                    {
                        //kp.Value.IsDirtyForSaving = false;
                        reconPref.ClientID = kp.Value.ClientID;
                        reconPref.ReconTypeID = (int)kp.Value.ReconType;
                        reconPref.TemplateName = kp.Value.TemplateName;
                        reconPref.TemplateKey = kp.Key;
                        reconPref.IsShowCAGeneratedTrades = kp.Value.IsShowCAGeneratedTrades;
                        lstReconPreference.Add(reconPref);
                    }
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            string xmlReconPref = XMLUtilities.SerializeToXML(lstReconPreference);
            if (xmlReconPref.Length > 0)
                DatabaseManager.SaveReconPreferencesInDB(xmlReconPref);
        }



        /// <summary>
        /// Set Thousand Separator Format
        /// </summary>
        /// <param name="gridExceptions"></param>
        /// <param name="templateKey"></param>
        /// //added by amit 09.04.2015
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-7135
        //public static void SetThousandSeparatorFormat(UltraGrid gridExceptions, string templateKey)
        //{
        //    List<MatchingRule> listOfRules = ReconPrefManager.ReconPreferences.GetListOfRules(templateKey);
        //    try
        //    {

        //        foreach (String str in listOfRules[0].NumericFields)
        //        {
        //            if (gridExceptions.DisplayLayout.Bands[0].Columns.Exists(ReconConstants.CONST_Nirvana + str))
        //            {
        //                gridExceptions.DisplayLayout.Bands[0].Columns[ReconConstants.CONST_Nirvana + str].Format = "#,0.00##";
        //                gridExceptions.DisplayLayout.Bands[0].Columns[ReconConstants.CONST_Nirvana + str].FormatInfo = System.Globalization.CultureInfo.CreateSpecificCulture("en");
        //            }
        //            if (gridExceptions.DisplayLayout.Bands[0].Columns.Exists(ReconConstants.CONST_Broker + str))
        //            {
        //                gridExceptions.DisplayLayout.Bands[0].Columns[ReconConstants.CONST_Broker + str].Format = "#,0.00##";
        //                gridExceptions.DisplayLayout.Bands[0].Columns[ReconConstants.CONST_Broker + str].FormatInfo = System.Globalization.CultureInfo.CreateSpecificCulture("en");
        //            }
        //            if (gridExceptions.DisplayLayout.Bands[0].Columns.Exists(ReconConstants.CONST_Diff + str))
        //            {
        //                gridExceptions.DisplayLayout.Bands[0].Columns[ReconConstants.CONST_Diff + str].Format = "#,0.00##";
        //                gridExceptions.DisplayLayout.Bands[0].Columns[ReconConstants.CONST_Diff + str].FormatInfo = System.Globalization.CultureInfo.CreateSpecificCulture("en");
        //            }
        //            if (gridExceptions.DisplayLayout.Bands[0].Columns.Exists(str))
        //            {
        //                gridExceptions.DisplayLayout.Bands[0].Columns[str].Format = "#,0.00##";
        //                gridExceptions.DisplayLayout.Bands[0].Columns[str].FormatInfo = System.Globalization.CultureInfo.CreateSpecificCulture("en");
        //            }
        //            gridExceptions.Update();
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
    }
}
