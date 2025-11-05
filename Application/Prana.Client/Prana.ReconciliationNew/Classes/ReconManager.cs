using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.TaskManagement.Definition.Definition;
using Prana.TaskManagement.Execution;
using Prana.Utilities;
using Prana.Utilities.ImportExportUtilities;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.XMLUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Prana.ReconciliationNew
{
    public class ReconManager : NirvanaTask, IDisposable
    {

        #region singleton
        private static volatile ReconManager instance;
        private static object syncRoot = new Object();

        private ReconManager()
        {
            CreateProxy();
        }

        private void CreateProxy()
        {
            try
            {
                _closingServicesProxy = new ProxyBase<IClosingServices>("TradeClosingServiceEndpointAddress");
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

        public static ReconManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ReconManager();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region local variables

        //string _executionName;
        //string _dashboardXmlDirectoryPath;
        //string _refDataDirectoryPath;
        public TaskResult _currentResult = new TaskResult();
        public static int DefaultEquityAUECID;
        bool _isSMResponsewiredup = false;
        int _hashCode = 0;
        System.Timers.Timer timerSMRequest;
        bool _isTimerTick = false;
        private static object syncLock = new object();
        DataTable _dtPB = new DataTable();

        //todo: move these constants to recon constants
        const string COLUMNSymbology = "Symbology";
        const string TickerSymbol = "Symbol";
        const string RICSymbol = "RIC";
        const string ISINSymbol = "ISIN";
        const string SEDOLSymbol = "SEDOL";
        const string CUSIPSymbol = "CUSIP";
        const string BloombergSymbol = "Bloomberg";
        const string OSIOptionSymbol = "OSIOptionSymbol";
        const string IDCOOptionSymbol = "IDCOOptionSymbol";
        const string OpraOptionSymbol = "OpraOptionSymbol";
        const string SMRequest = "SMRequest";

        ProxyBase<IClosingServices> _closingServicesProxy = null;

        /// <summary>
        /// Dictionary of file settings
        /// </summary>
        public static Dictionary<int, FileSettingItem> _dictFileSetting = new Dictionary<int, FileSettingItem>();

        #endregion

        #region event handler

        public static event EventHandler SetDataTableEvent = delegate { };

        #endregion

        /// <summary>
        /// Update DataTable Schema For Custom Columns 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static DataTable UpdateTableSchemaForCustomColumns(DataTable dt)
        {
            try
            {
                DataTable NewDT = dt.Copy();
                List<string> lstCustomColumns = new List<string>();
                //make expression column readonly property to false so that on that column grouping can be done
                //http://jira.nirvanasolutions.com:8080/browse/GUGGENHEIM-12
                foreach (DataColumn col in dt.Columns)
                {
                    if (!string.IsNullOrEmpty(col.Expression))
                    {
                        col.Expression = string.Empty;
                        col.ReadOnly = false;
                        lstCustomColumns.Add(col.ColumnName);
                    }
                }
                if (lstCustomColumns.Count > 0)
                {
                    dt.Clear();
                    foreach (DataRow row in NewDT.Rows)
                    {
                        DataRow newRow = dt.NewRow();
                        newRow.ItemArray = row.ItemArray;
                        dt.Rows.Add(newRow);
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
            return dt;
        }

        public static DataTable TransformData(DataTable dt, string reconXmlDataDirectoryPath, ReconParameters reconParameters)
        {
            DataTable _dt = new DataTable();
            try
            {
                if (dt != null && reconParameters != null)
                {
                    string tempPath = reconXmlDataDirectoryPath + "\\xmls\\Transformation\\Temp";
                    // string tempPath = Application.StartupPath + "\\xmls\\Transformation\\Temp";
                    // DirectoryInfo dir = new DirectoryInfo(tempPath);
                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }
                    //changes for new recon directory structure
                    string inputXML = tempPath + "\\InputXML" + Seperators.SEPERATOR_6 + reconParameters.FromDate + Seperators.SEPERATOR_6 + reconParameters.ToDate + ".xml";
                    string outputXML = tempPath + "\\OutPutXML" + Seperators.SEPERATOR_6 + reconParameters.FromDate + Seperators.SEPERATOR_6 + reconParameters.ToDate + ".xml";


                    //string inputXML = Application.StartupPath + "\\xmls\\Transformation\\Temp\\InputXML.xml";
                    //string outputXML = Application.StartupPath + "\\xmls\\Transformation\\Temp\\OutPutXML.xml";
                    string path = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconXSLT.ToString() + "\\";
                    string xsltName = ReconPrefManager.ReconPreferences.GetXsltFileName(reconParameters.TemplateKey);
                    string xsltPath = path + xsltName;
                    XMLUtilities.WriteXML(dt, inputXML);
                    //dt.WriteXml(inputXML);
                    XMLUtilities.GetTransformedXML(inputXML, outputXML, xsltPath);
                    //TODO: This code is hardcoded, make it generic
                    #region enritching of data
                    //path = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconXSLT.ToString() + "\\";
                    //xsltName = "Recon_Append.xslt";
                    //xsltPath = path + xsltName;
                    //if (File.Exists(xsltPath))
                    //{
                    //    inputXML = outputXML;
                    //    outputXML = tempPath + "\\OutPutXMLNew.xml";
                    //    FileTransformer.GetTransformedXML(inputXML, outputXML, xsltPath);
                    //}
                    #endregion

                    DataSet ds = new DataSet();
                    //CHMW-2181	[Reconciliation] [Code Review] Replace dataset read xml with BufferedStream
                    //ds.ReadXml(outputXML);
                    ds = XMLUtilities.ReadXmlUsingBufferedStream(outputXML);
                    if (ds.Tables.Count > 0 && ds.Tables[0] != null)
                    {
                        _dt = ds.Tables[0];
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
            return _dt;
        }

        /// <summary>
        /// TODO: Put summary
        /// </summary>
        /// <param name="inputData"></param>
        /// <param name="taskResult"></param>
        private void RunReconciliationForSelectedTemplate(List<object> InputObjects, TaskResult taskResult)
        {
            DataTable dtExceptionReport = new DataTable();
            string errMsg = string.Empty;
            try
            {
                ReconParameters reconParameters = (ReconParameters)InputObjects[0];
                #region recon input parameters
                //set data for the reconciler
                //List<string> input = new List<string>(inputData.Split(Seperators.SEPERATOR_8[0]));
                //string templateKey = input[0];
                //string reconType = input[1];
                //DateTime fromDate = DateTime.ParseExact(input[2], ApplicationConstants.DateFormat, CultureInfo.InvariantCulture);
                //DateTime toDate = DateTime.ParseExact(input[3], ApplicationConstants.DateFormat, CultureInfo.InvariantCulture);
                //string filePath = input[4];
                //string clientID = input[5];
                //bool isReconReportToBeGenerated = bool.Parse(input[6]);
                ReconTemplate reconTemplate = ReconPrefManager.ReconPreferences.GetTemplates(reconParameters.TemplateKey);
                #endregion
                if (reconTemplate != null)
                {

                    reconParameters.SpName = reconTemplate.SpName;
                    reconParameters.FormatName = reconTemplate.FormatName;
                    reconParameters.ReconDateType = reconTemplate.ReconDateType;
                    reconParameters.IsShowCAGeneratedTrades = reconTemplate.IsShowCAGeneratedTrades;
                    // Added by Ankit Gupta on 13 Oct, 2014.
                    // http://jira.nirvanasolutions.com:8080/browse/CHMW-1471
                    //string templateName = ReconUtilities.GetTemplateNameFromTemplateKey(templateKey);
                    if (taskResult != null)
                    {
                        _currentResult = taskResult;
                        #region Set Dashboard Data
                        //_executionName = Path.GetFileName(_currentResult.GetDashBoardXmlPath());
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("DashboardFile", _currentResult.GetDashBoardXmlPath() + ".xml", null);

                        //_dashboardXmlDirectoryPath = Path.GetDirectoryName(_executionName);
                        //_refDataDirectoryPath = _dashboardXmlDirectoryPath + @"\RefData";
                        //CHMW-2127	[Recon] Recon batch status is failure but on view and amend button , data is coming on UI

                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("Status", "Running", null);

                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("TemplateKey", reconParameters.TemplateKey, null);

                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ReconType", reconParameters.ReconType, null);

                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("FromDate", reconParameters.FromDate, null);

                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ToDate", reconParameters.ToDate, null);
                        //CHMW-3150	[Recon] - Message "no data to show" on Amend Button when data points are available.
                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("RunDate", reconParameters.RunDate, null);

                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ClientID", reconParameters.ClientID, null);

                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("TemplateName", reconParameters.TemplateName, null);

                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("PBFile", reconParameters.PBFilePath, null);

                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ReconDateType", reconParameters.ReconDateType.ToString(), null);

                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("LoggedInUser", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, null);

                        if (!string.IsNullOrEmpty(reconParameters.FormatName))
                        {
                            _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("FormatName", reconParameters.FormatName, null);
                        }
                        _currentResult.LogResult();
                        //UpdateTaskSpecificDataPoints(this, _currentResult);
                        #endregion
                    }
                    //Added by sachin mishra Purpose : Jira-CHMW-3563
                    if (!string.IsNullOrEmpty(reconParameters.SpName.Trim()))
                    {
                        if (reconTemplate.ReconFilters.DictAccounts.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(reconTemplate.XsltPath))
                            {
                                #region Fetch PB data
                                DataTable dtPB = FetchPBData(reconTemplate, reconParameters, ref errMsg);
                                #endregion
                                if (dtPB != null)
                                {
                                    #region Fetch NirvanaData
                                    DataTable dtNirvana = FetchNirvanaData(reconTemplate, reconParameters);
                                    #endregion
                                    #region Run Reconciliation
                                    dtExceptionReport = RunReconciliation(reconTemplate, dtPB, dtNirvana, ref errMsg);
                                    #endregion
                                    if (string.IsNullOrEmpty(errMsg))
                                    {
                                        #region Generate Exception and other Report
                                        string exceptionFileName = ReconUtilities.GetReconFilePath(ReconConstants.ReconDataDirectoryPath, reconParameters);

                                        //This path will be used to save data in dashboard.
                                        string relativeReconFilePath = exceptionFileName.Substring(Application.StartupPath.Length);

                                        ReconUtilities.CreateDirectoryIfNotExists(exceptionFileName);

                                        //Logger.LoggerWrite("Exception report file path: " + exceptionFileName);
                                        //generate the exception report
                                        //Logger.LoggerWrite("Started generating exception report");
                                        //false is passed because files are not to be generated on runrecon_click
                                        dtExceptionReport = DataReconciler.GenerateExceptionsReport(dtExceptionReport, exceptionFileName, reconTemplate.ExpReportFormat, reconTemplate.SelectedColumnList, reconTemplate.ListSortByColumns, reconTemplate.ListGroupByColumns, false);


                                        Logger.LoggerWrite("Done generating exception report");
                                        DataSet dsXml = new DataSet();
                                        dsXml.Tables.Add(dtExceptionReport);

                                        //add info to task statistics
                                        _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ExceptionReport", dtExceptionReport.Rows.Count, relativeReconFilePath);
                                        UpdateTaskSpecificDataPoints(this, _currentResult);

                                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-1417
                                        //Save Recon Report only when user clicks on save button only not on run recon
                                        if (reconParameters.IsReconReportToBeGenerated)
                                        {
                                            XMLUtilities.WriteXMLWithSchema(dsXml, exceptionFileName + ".xml");
                                            //dsXml.WriteXml(exceptionFileName + ".xml");
                                            SetTaskSpecificData(dtExceptionReport, _currentResult);
                                            // added by amit on 03.03.2015 CHMW-2723(update task status)
                                            UpdateAmendmentsFile(exceptionFileName + ".xml");
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        _currentResult.Error = new Exception(errMsg);
                                        UpdateTaskSpecificDataPoints(this, _currentResult);
                                    }
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(errMsg))
                                    {
                                        //MessageBox.Show("File not found.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                        errMsg = "File not found.";
                                        _currentResult.Error = new Exception("File not found.");
                                    }
                                    else
                                    {
                                        _currentResult.Error = new Exception(errMsg);
                                    }
                                    UpdateTaskSpecificDataPoints(this, _currentResult);
                                }
                            }
                            else
                            {
                                //MessageBox.Show("Please select XSLT for the Template.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                errMsg = "Please select XSLT for the Template.";
                                _currentResult.Error = new Exception("XSLT not set for Template.");
                                UpdateTaskSpecificDataPoints(this, _currentResult);
                            }
                        }
                        else
                        {
                            //MessageBox.Show("No Account Selected.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            errMsg = "No Account Selected.";
                            _currentResult.Error = new Exception("No Account Selected.");
                            UpdateTaskSpecificDataPoints(this, _currentResult);
                        }
                    }
                    else
                    {
                        errMsg = "SP Name cannot be blank. Please fill the SP Name for " + reconTemplate.FormatName.Trim() + " format.";
                        _currentResult.Error = new Exception("SPName not found.");
                        UpdateTaskSpecificDataPoints(this, _currentResult);
                    }
                }
                else
                {
                    //MessageBox.Show("Template either modified or deleted.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("DashboardFile", _currentResult.GetDashBoardXmlPath() + ".xml", null);
                    errMsg = "Template either modified or deleted.";
                    _currentResult.Error = new Exception("Template either modified or deleted.");
                    UpdateTaskSpecificDataPoints(this, taskResult);
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                _currentResult.TaskStatistics.Status = NirvanaTaskStatus.Failure;
                _currentResult.Error = ex;
                _currentResult.LogResult();
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                ListEventAargs listEventAargs = new ListEventAargs();
                listEventAargs.listOfValues.Add(errMsg);
                listEventAargs.argsObject = dtExceptionReport;
                listEventAargs.argsObject2 = taskResult;
                SetDataTableEvent(this, listEventAargs);
                //CHMW-2294 [Recon Dashboard] On running batch from recon dashboard,its view and amend buttons become disable
                //Statistics data is not saved by taskresult but in writexml so that file is updated.
                //updating currentresult will remove the statistics data of matched rows and other details
                // this.ExecutionComplete(_currentResult);
            }
        }
        // added by amit on 03.03.2015 for updating task status on recon dashboard ui.
        /// <summary>
        /// http://jira.nirvanasolutions.com:8080/browse/CHMW-2723
        /// </summary>
        /// <param name="p"></param>
        private void UpdateAmendmentsFile(string p)
        {
            try
            {
                SerializableDictionary<string, int> amendmends = new SerializableDictionary<string, int>();
                amendmends = ReconUtilities.LoadAmendmentDictionary();
                // if file is already write then update its value else add an entry

                string relativeExceptionFilePath = ReconUtilities.GetRelativeExceptionFilePath(p);
                if (amendmends.ContainsKey(relativeExceptionFilePath))
                {
                    amendmends[relativeExceptionFilePath] = 0;
                }
                else
                {
                    amendmends.Add(relativeExceptionFilePath, 0);
                }

                ReconUtilities.WriteAmendmentDictionary(amendmends);
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

        private DataTable RunReconciliation(ReconTemplate reconTemplate, DataTable dtPB, DataTable dtNirvana, ref string errMsg)
        {
            try
            {
                DataTable dtExceptionReport = new DataTable();
                //DataTable dtExceptions = new DataTable();
                DataTable dtAppMatchedData = new DataTable();
                DataTable dtBrokerMatchedData = new DataTable();

                ReconPrefManager.ReconPreferences.SetDefaultReportGeneratedProperty();
                List<MatchingRule> rules = ReconPrefManager.ReconPreferences.GetListOfRules(reconTemplate.TemplateKey);
                //List<ColumnInfo> listMasterColumns = ReconPrefManager.ReconPreferences.GetNirvanaMasterColumns(reconTemplate.TemplateKey);

                foreach (MatchingRule rule in rules)
                {
                    //This method is called in DataReconciler.Reconcile() method
                    //DataReconciler.GenerateExceptionsDataTableSchema(template.SelectedColumnList);

                    // dtExceptions = ReconUtilities.GetExceptionsDataTableSchema(rule.ComparisonFields, listMasterColumns);
                    reconTemplate.isReconReportToBeGenerated = true;

                    //TODO: isClearExCache value is sent to true
                    //Logger.LoggerWrite("Started Reconciling data");
                    // To remove reference casted from list to array and then back to list
                    List<ColumnInfo> listExceptionReportColumns = new List<ColumnInfo>(reconTemplate.SelectedColumnList.ToArray());
                    //trade date of file should always be available
                    #region Add date Temp columns
                    // TODO : A better approach is to be found than adding and removing columns
                    // Column are added as they are needed while saving amendments
                    ColumnInfo colNirvana = new ColumnInfo();
                    colNirvana.ColumnName = "TradeDate";
                    colNirvana.GroupType = ColumnGroupType.Nirvana;
                    colNirvana.SortOrder = SortingOrder.Ascending;
                    //TODO: Check proper way to check contains in the columns, here is object matching 
                    if (!listExceptionReportColumns.Contains(colNirvana))
                    {
                        listExceptionReportColumns.Add(colNirvana);
                    }
                    //ColumnInfo colBroker = new ColumnInfo();
                    //colBroker.ColumnName = "TradeDate";
                    //colBroker.GroupType = ColumnGroupType.PrimeBroker;
                    //colBroker.SortOrder = SortingOrder.Ascending;
                    //if (!listExceptionReportColumns.Contains(colBroker))
                    //{
                    //    listExceptionReportColumns.Add(colBroker);
                    //}
                    ColumnInfo colRowID = new ColumnInfo();
                    colRowID.ColumnName = ReconConstants.COLUMN_RowIndex;
                    colRowID.GroupType = ColumnGroupType.PrimeBroker;
                    colRowID.SortOrder = SortingOrder.Ascending;
                    if (!listExceptionReportColumns.Contains(colRowID))
                    {
                        listExceptionReportColumns.Add(colRowID);
                    }
                    #endregion
                    HashSet<string> hSetCommonColumn = GetCommonRule(reconTemplate.AvailableColumnList, reconTemplate.SelectedColumnList);
                    dtExceptionReport = DataReconciler.Reconcile(dtNirvana, dtPB, out dtAppMatchedData, out dtBrokerMatchedData, rule, reconTemplate.IsIncludeMatchedItems, reconTemplate.IsIncludeToleranceMacthedItems, listExceptionReportColumns, true, reconTemplate.ReconType, ref errMsg, reconTemplate.IsReconTemplateGroupping(), hSetCommonColumn);
                    #region sort dtExceptionReport
                    // http://jira.nirvanasolutions.com:8080/browse/CHMW-2222
                    // Default sorting in any recon should be the breaks, then the matches within tolerance, then the exact matches
                    // Adding another column 'SortHelper' to 'dtExceptionReprot', which will be used to custom sort the data table, and
                    // Setting the expression property of the column, so that its value automatically gets filled according to column 'MismatchType'
                    if (dtExceptionReport.Columns.Contains(ReconConstants.CAPTION_MismatchType))
                    {
                        String expression = "IIF([" + ReconConstants.CAPTION_MismatchType + "] LIKE '*Exactly*', '4', IIF([" + ReconConstants.CAPTION_MismatchType + "] LIKE '*Tolerance*', 3, IIF([" + ReconConstants.CAPTION_MismatchType + "] LIKE '*Mismatch*', 2, 1)))";
                        dtExceptionReport.Columns.Add("SortHelper", typeof(Int16), expression);
                        dtExceptionReport = sortDataTable(dtExceptionReport, "SortHelper ASC", null);
                        dtExceptionReport.Columns.Remove("SortHelper");
                    }
                    #endregion
                    //Logger.LoggerWrite("Done Reconciling data");
                }
                //object[] results = new object[5];
                //results[0] = dtExceptions;
                //results[1] = dtNirvana;
                //results[2] = dtPB;
                //results[3] = dtAppMatchedData;
                //results[4] = dtBrokerMatchedData;
                //e.Result = results;
                return dtExceptionReport;
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

        private DataTable FetchPBData(ReconTemplate reconTemplate, ReconParameters reconParameters, ref string errMsg)
        {
            try
            {
                //bool isMultipleDaysRecon = false;
                //if user has specified file path then file should not be downloaded from ftp
                if (string.IsNullOrEmpty(reconParameters.PBFilePath))
                {
                    // filePath = RunUploadProcess(_currentResult, reconTemplate.FormatName, toDate);                    
                    reconParameters.PBFilePath = RunUploadProcess(_currentResult, reconParameters);
                    //isMultipleDaysRecon = true;
                }
                //if ftp downloaded file is null or empty then return
                if (!string.IsNullOrEmpty(reconParameters.PBFilePath))
                {

                    DataTable dtPB = new DataTable();
                    //TODO: Make this part generic as much as possible
                    //pass it through import format type
                    //Logger.LoggerWrite("Started reading prime broker file");
                    if (reconParameters.PBFilePath.EndsWith(".xml") && reconParameters.PBFilePath.Contains("InputXML"))
                    {
                        DataSet dsConsolidated = new DataSet();
                        //CHMW-2181	[Reconciliation] [Code Review] Replace dataset read xml with BufferedStream
                        //dsConsolidated.ReadXml(filePath);
                        dsConsolidated = XMLUtilities.ReadXmlUsingBufferedStream(reconParameters.PBFilePath);
                        if (dsConsolidated != null & dsConsolidated.Tables.Count > 0)
                        {
                            dtPB = dsConsolidated.Tables[0];
                        }
                        else
                        {
                            //TODO : Handel the condition here
                        }
                    }
                    else
                    {
                        dtPB = FileReaderFactory.GetDataTableFromDifferentFileFormats(reconParameters.PBFilePath);
                    }
                    //Logger.LoggerWrite("Done reading prime broker file");
                    //Logger.LoggerWrite("Datatable have " + dtPB.Rows.Count + " rows after reading file");
                    dtPB.AcceptChanges();
                    dtPB.TableName = "Comparision";
                    //btnTransform_Click(null, null);
                    //Logger.LoggerWrite("Started adding primary key to datatable");
                    AddPrimaryKey(dtPB);
                    //Logger.LoggerWrite("Done adding primary key to datatable");
                    //TODO: Here date is hardcoded, it should be t-1
                    //Logger.LoggerWrite("Started transforming data");
                    string reconXmlDataDirectoryPath = ReconUtilities.GetReconDirectoryPath(ReconConstants.ReconDataDirectoryPath, reconParameters);
                    dtPB = ReconManager.TransformData(dtPB, reconXmlDataDirectoryPath, reconParameters);
                    errMsg = ReconManager.ValidateXML(reconXmlDataDirectoryPath, reconParameters);
                    if (string.IsNullOrEmpty(errMsg))
                    {
                        //Logger.LoggerWrite("Done transforming data");
                        //Logger.LoggerWrite("Datatable have " + dtPB.Rows.Count + "rows after transforming");
                        //TODO: Currently FillSMData is commented because it is handled in prana.tools which is ui control
                        if (IsSecurityMasterRequestRequired(dtPB))
                        {

                            //http://jira.nirvanasolutions.com:8080/browse/CHMW-1983
                            //To Fill Symbol in local table
                            dtPB = FillSMData();
                            //timerSMRequest.Tick -= new EventHandler(timerSMRequest_Tick);
                        }
                        dtPB = ReconManager.ProcessReconData(dtPB, reconParameters.TemplateKey, DataSourceType.PrimeBroker, _currentResult);
                        //btnGroup_Click(null, null);
                        //sort datatable according to the sorted column in matching rules files
                        if (dtPB.Rows.Count > 0)
                        {

                            dtPB = sortDataTable(dtPB, reconTemplate.SortingColumnOrder, reconTemplate.RulesList);
                        }
                        return dtPB;
                    }
                }
            }
            catch (Exception ex)
            {
                _currentResult.TaskStatistics.Status = NirvanaTaskStatus.Failure;
                _currentResult.Error = ex;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        private void SetUpTimer()
        {
            try
            {
                timerSMRequest = new System.Timers.Timer();
                timerSMRequest.Elapsed += new System.Timers.ElapsedEventHandler(timerSMRequest_Tick);
                //timer will ticks after every 3 seconds and 
                //tick timer to write the xml after fetching symbols for sedols from SMRequest
                timerSMRequest.Enabled = true;                       // Enable the timer
                timerSMRequest.Interval = 10000;              // Timer will tick after every 10 second
                timerSMRequest.Start();                             //start timer
            }
            catch (Exception ex)
            {
                _currentResult.TaskStatistics.Status = NirvanaTaskStatus.Failure;
                _currentResult.Error = ex;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private static string ValidateXML(string reconXmlDataDirectoryPath, ReconParameters reconParameters)
        {
            string tempError = string.Empty;
            try
            {
                #region get XSD Path
                string path = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconXSD.ToString() + "\\";
                string xsdName = "ReconValidation.xsd";
                string xsdPath = path + xsdName;
                #endregion

                #region get inputXML Path
                string tempPath = reconXmlDataDirectoryPath + "\\xmls\\Transformation\\Temp";
                //DirectoryInfo dir = new DirectoryInfo(tempPath);
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                string outputXML = tempPath + "\\OutPutXML" + Seperators.SEPERATOR_6 + reconParameters.FromDate + Seperators.SEPERATOR_6 + reconParameters.ToDate + ".xml";

                #endregion

                if (!string.IsNullOrEmpty(xsdPath) && File.Exists(xsdPath) && File.Exists(outputXML))
                {
                    bool isXmlValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(outputXML, xsdPath, "", out tempError, false);
                    if (!isXmlValidated)
                    {
                        return tempError;
                    }
                }
                else
                {
                    tempError = @"File not found: ""MappingFiles\ReconXSD\ReconValidation.xsd""";
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
            return tempError;
        }

        private DataTable FetchNirvanaData(ReconTemplate reconTemplate, ReconParameters reconParameters)
        {
            DataTable dtNirvana = new DataTable();
            try
            {
                //TODO: Make this part generic as much as possible
                string commaSeparatedAssetIDs = ReconUtilities.GetCommaSeparatedAssetIds(reconParameters.TemplateKey);
                //Logger.LoggerWrite("length of commaSeparatedAssetIDs" + commaSeparatedAssetIDs.Length);
                StringBuilder commaSeparatedAccountIDs = ReconUtilities.GetCommaSeparatedAccountIds(reconParameters.TemplateKey);
                //Logger.LoggerWrite("length of commaSeparatedAccountIDs" + commaSeparatedAccountIDs.Length);

                //Logger.LoggerWrite("SP Name:" + spName);
                //TODO: Centralize create position services proxy which is defined in recon utilities
                //Logger.LoggerWrite("Started fetching data from database");
                DataSet ds = ReconUtilities.FetchDataForGivenSPName(reconParameters, commaSeparatedAssetIDs, commaSeparatedAccountIDs.ToString());
                //Logger.LoggerWrite("Data fetched from database");

                if (ds != null && ds.Tables.Count > 0)
                {
                    dtNirvana = ds.Tables[0];
                }
                if (dtNirvana.Columns.Count > 0)
                {
                    HandleEquitySwapsInDataTable(reconParameters.TemplateKey, dtNirvana);
                    dtNirvana = ReconManager.ProcessReconData(dtNirvana, reconParameters.TemplateKey, DataSourceType.Nirvana, _currentResult);
                    if (dtNirvana.Rows.Count > 0)
                    {
                        //sort datatable according to the sorted column in matching rules files                       
                        dtNirvana = sortDataTable(dtNirvana, reconTemplate.SortingColumnOrder, reconTemplate.RulesList);
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
            return dtNirvana;
        }
        /// <summary>
        /// Temp Handling of equity Swaps for Recon
        /// </summary>
        /// <param name="templateKey"></param>
        /// <param name="dtNirvana"></param>
        private static void HandleEquitySwapsInDataTable(string templateKey, DataTable dtNirvana)
        {
            try
            {
                #region Handling Equity Swaps
                //CHMW-1869	[Recon] Equity Swaps asset class is not available in recon template
                bool isEquitySwapsIncluded = true;
                bool isEquityIncluded = true;

                Dictionary<ReconFilterType, Dictionary<int, string>> dictReconFilters = ReconPrefManager.ReconPreferences.GetReconFilters(templateKey);
                if (dictReconFilters.ContainsKey(ReconFilterType.Asset))
                {
                    List<int> listAssetIDs = new List<int>(dictReconFilters[ReconFilterType.Asset].Keys);
                    if (!listAssetIDs.Contains(500))
                    {
                        isEquitySwapsIncluded = false;
                    }
                    if (!listAssetIDs.Contains((int)AssetCategory.Equity))
                    {
                        isEquityIncluded = false;
                    }
                }
                if (dtNirvana.Columns.Contains("AssetID") && dtNirvana.Columns.Contains("IsSwapped"))
                {
                    DataRow[] result = dtNirvana.Select("AssetID = '" + (int)AssetCategory.Equity + "'");//+ "' AND IsSwapped = True");
                    foreach (DataRow row in result)
                    {
                        if (row["IsSwapped"] != DBNull.Value)
                        {
                            if (row["IsSwapped"].ToString().Equals("True", StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (!isEquitySwapsIncluded)
                                {
                                    dtNirvana.Rows.Remove(row);
                                }
                                else
                                {
                                    if (dtNirvana.Columns.Contains("Asset"))
                                    {
                                        row["Asset"] = "EquitySwap";
                                    }
                                    if (dtNirvana.Columns.Contains("AssetName"))
                                    {
                                        row["AssetName"] = "EquitySwap";
                                    }
                                }
                            }
                            else if (!isEquityIncluded)
                            {
                                dtNirvana.Rows.Remove(row);
                            }
                        }
                    }
                }
                #endregion
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
        /// Update Task Specific Data Points
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTaskSpecificDataPoints(object sender, EventArgs e)
        {
            try
            {
                TaskResult taskResult = e as TaskResult;
                this.ExecutionComplete(taskResult);

                //added by: Aman Seth, 30 July 2014
                //purpose: update the dictionary when the batch is completed               
                string dashboardFile = taskResult.TaskStatistics.TaskSpecificData.GetValueForKey("DashboardFile").ToString();
                string batchKey = Path.GetFileName(dashboardFile);
                string batchValue = dashboardFile;
                CachedDataManagerRecon.AddItemInDashBoardFileCache(batchKey, batchValue);


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
        /// TODO: summary should be given here
        /// </summary>
        /// <param name="taskResult"></param>
        /// <param name="template"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        private string RunUploadProcess(TaskResult taskResult, ReconParameters reconParameters)
        {

            string consolidatedFileName = string.Empty;
            DataTable dt = new DataTable();
            bool isPromptForDuplicityCheck = false;
            bool isFileUploadedSuccessfully = false;
            bool isCheckForFileDuplicityCheck = true;

            try
            {
                string formatName = reconParameters.FormatName;
                DateTime date = reconParameters.DTFromDate.Date;

                string mailSubject = "Recon: File Import Error.";
                StringBuilder mailBody = new StringBuilder();
                mailBody.AppendLine("Recon: File Import Error.");
                #region Commented
                RunUpload runUpload = null;
                if (ReconPreferences.DictRunUpload != null && ReconPreferences.DictRunUpload.ContainsKey(formatName))
                {
                    runUpload = ReconPreferences.DictRunUpload[formatName];
                }
                if (runUpload == null)
                {
                    //Todo: Mail has to be send here also.
                    taskResult.Error = new Exception("Run Upload object is null");
                    UpdateTaskSpecificDataPoints(this, taskResult);
                    return string.Empty;
                }
                #endregion
                if (taskResult != null)
                {
                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ThirdPartyType", runUpload.DataSourceNameIDValue.ShortName, null);
                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("FileType", runUpload.ImportTypeAcronym.ToString(), null);
                }


                #region Get file names for the date range for the naming convention defined in admin
                Dictionary<string, DateTime> dictAllFileNames = new Dictionary<string, DateTime>();
                Dictionary<string, DateTime> dictListFiles = new Dictionary<string, DateTime>();
                bool isPullFromStagingArea = false;
                while (date.Date.CompareTo(reconParameters.DTToDate.Date) <= 0)
                {
                    string fileName = FileNameParser.GetFileNameFromNamingConvention(runUpload.FtpFilePath, date);

                    if (runUpload.FtpDetails != null)
                    {  //File system is not case sensitive so while comparing file name case is to be ignored.
                        fileName = Path.GetFileName(fileName).ToLower();
                    }

                    if (!dictAllFileNames.ContainsKey(fileName))
                    {
                        dictAllFileNames.Add(fileName, date);
                    }
                    date = date.AddDays(1);
                }
                #endregion

                #region Check if file exist in ftp or staging area
                if (dictAllFileNames.Count > 0)
                {
                    if (runUpload.FtpDetails != null)
                    {
                        //check that from list how much files are available in ftp
                        NirvanaWinSCPUtility scpUtil = new NirvanaWinSCPUtility(runUpload.FtpDetails);
                        dictListFiles = scpUtil.ListDirectory(Path.GetDirectoryName(runUpload.FtpFilePath), dictAllFileNames);
                    }
                    else
                    {
                        isPullFromStagingArea = true;
                        foreach (KeyValuePair<string, DateTime> kvp in dictAllFileNames)
                        {
                            if (File.Exists(kvp.Key))
                            {
                                dictListFiles.Add(kvp.Key, kvp.Value);
                            }
                        }
                    }
                }
                #endregion

                if (dictListFiles.Count > 0)
                {
                    //for each available file run the batch
                    //file name also amended in batch execution name
                    foreach (KeyValuePair<string, DateTime> kvp in dictListFiles)
                    {
                        if (isPullFromStagingArea)
                        {
                            //runUpload.ProcessedFilePath = kvp.Key;
                            if (!string.IsNullOrEmpty(kvp.Key) && File.Exists(kvp.Key))
                            {
                                if (!runUpload.ProcessedFilePath.Contains("ProcessedData"))
                                {
                                    runUpload.ProcessedFilePath = runUpload.FilePath + @"ProcessedData\";
                                }
                                else
                                {
                                    runUpload.ProcessedFilePath = Path.GetDirectoryName(runUpload.ProcessedFilePath) + "\\"; ;
                                }
                                //runUpload.ProcessedFilePath = runUpload.RawFilePath;
                                //runUpload.FilePath += Path.GetFileName(kvp.Key);
                                if (!Directory.Exists(runUpload.ProcessedFilePath))
                                    Directory.CreateDirectory(runUpload.ProcessedFilePath);
                                File.Copy(kvp.Key, runUpload.ProcessedFilePath + Path.GetFileName(kvp.Key), true);
                                runUpload.ProcessedFilePath = runUpload.ProcessedFilePath + Path.GetFileName(kvp.Key);
                            }
                        }
                        else
                        {
                            #region 1. ParseFileName
                            string fileName = RunUploadProcessHelper.ParseFileName(runUpload, kvp.Value.Date);
                            #endregion

                            #region 2. DownloadFileFromFTP
                            Dictionary<string, Tuple<string, string>> dictTaskResult = new Dictionary<string, Tuple<string, string>>();
                            string errMsg = RunUploadProcessHelper.DownloadFileFromFTP(runUpload, dictTaskResult, ref isCheckForFileDuplicityCheck, mailSubject, mailBody, ref fileName, false, true);
                            foreach (KeyValuePair<string, Tuple<string, string>> taskPoint in dictTaskResult)
                            {
                                taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint(taskPoint.Key, taskPoint.Value.Item1, taskPoint.Value.Item2);
                            }
                            if (!string.IsNullOrEmpty(errMsg))
                            {
                                isFileUploadedSuccessfully = false;
                                taskResult.Error = new Exception(errMsg);
                                continue;
                            }
                            #endregion

                            #region 3. Decryption section
                            runUpload.RawFilePath = runUpload.RawFilePath + fileName;
                            isFileUploadedSuccessfully = RunUploadProcessHelper.Decrypt(runUpload, mailSubject, mailBody, fileName);
                            if (!isFileUploadedSuccessfully)
                            {
                                if (taskResult != null)
                                {
                                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("RetrievalStatus", "Failure", null);
                                    taskResult.Error = new Exception("File decryption error");
                                    UpdateTaskSpecificDataPoints(this, taskResult);
                                }
                                UpdateTaskSpecificDataPoints(this, taskResult);
                                continue;
                            }
                            else
                            {
                                if (taskResult != null)
                                {
                                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("RetrievalStatus", "Success", null);
                                    UpdateTaskSpecificDataPoints(this, taskResult);
                                }
                            }
                            #endregion

                            #region 4. Check Duplicity of file section
                            dictTaskResult.Clear();
                            errMsg = RunUploadProcessHelper.CheckAndAppyDuplicityValidation(runUpload, isPromptForDuplicityCheck, dictTaskResult, isCheckForFileDuplicityCheck, mailSubject, mailBody, fileName, true);
                            foreach (KeyValuePair<string, Tuple<string, string>> taskPoint in dictTaskResult)
                            {
                                taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint(taskPoint.Key, taskPoint.Value.Item1, taskPoint.Value.Item2);
                            }
                            if (!string.IsNullOrEmpty(errMsg))
                            {
                                isFileUploadedSuccessfully = false;
                                taskResult.Error = new Exception(errMsg);
                                continue;
                            }
                            #endregion
                        }
                        //We need to set here file name, as filename is used in key of runupload
                        runUpload.FileName = Path.GetFileName(runUpload.ProcessedFilePath);
                        runUpload.FilePath = runUpload.ProcessedFilePath;

                        DataTable dtTemp = new DataTable();
                        dtTemp = FileReaderFactory.GetDataTableFromDifferentFileFormats(runUpload.FilePath);
                        dt.Merge(dtTemp);

                    }
                    dt.TableName = "Consolidated";
                    if (dt.Rows.Count == 0)
                    {
                        taskResult.Error = new Exception("No file found.");
                        UpdateTaskSpecificDataPoints(this, taskResult);
                        return string.Empty;
                    }
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);
                    string reconXmlDataDirectoryPath = ReconUtilities.GetReconDirectoryPath(ReconConstants.ReconDataDirectoryPath, reconParameters);
                    string tempPath = reconXmlDataDirectoryPath + "\\xmls\\Transformation\\Temp\\";
                    consolidatedFileName = tempPath + "InputXML" + Seperators.SEPERATOR_6 + reconParameters.FromDate + Seperators.SEPERATOR_6 + reconParameters.ToDate + ".xml";

                    //Update consolidated File Name
                    _currentResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("PBFile", consolidatedFileName, null);

                    ReconUtilities.SaveTransformedPBData(ds, tempPath, consolidatedFileName);
                }
            }
            catch (Exception ex)
            {
                taskResult.Error = ex;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return consolidatedFileName;
        }

        private static void AddValuesInTaskResult(Dictionary<string, Tuple<string, string>> taskResult, string itemName, string value1, string value2)
        {
            try
            {
                if (taskResult.ContainsKey(itemName))
                {
                    taskResult[itemName] = new Tuple<string, string>(value1, value2);
                }
                else
                {
                    taskResult.Add(itemName, new Tuple<string, string>(value1, value2));
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
        /// set Task Specific Data
        /// </summary>
        /// <param name="dt"></param>
        // Modified by Ankit Gupta on 3rd Sep, 2014.
        // http://jira.nirvanasolutions.com:8080/browse/CHMW-1353
        public static void SetTaskSpecificData(DataTable dtExceptionReport, TaskResult taskResult)
        {
            try
            {
                if (taskResult != null && dtExceptionReport != null)
                {
                    #region commented
                    //string dashBoardFilePath;
                    //if (!string.IsNullOrEmpty(taskResult.TaskStatistics.TaskSpecificData.AsDictionary["execBatch"].ToString()) && File.Exists(Application.StartupPath + CachedDataManagerRecon.GetExecutionDashboardFilePath(taskResult.TaskStatistics.TaskSpecificData.AsDictionary["execBatch"].ToString())))
                    //{
                    //    dashBoardFilePath = Application.StartupPath + CachedDataManagerRecon.GetExecutionDashboardFilePath(execBatch);
                    //}
                    //else
                    //{
                    //Return if dashboard File is not found
                    //return;
                    //}
                    //DataRow row;
                    // DataSet ds = new DataSet();
                    //CHMW-2181	[Reconciliation] [Code Review] Replace dataset read xml with BufferedStream
                    //ds.ReadXml(dashBoardFilePath);
                    //ds = XMLUtilities.ReadXmlUsingBufferedStream(dashBoardFilePath);
                    //if (ds.Tables.Count == 0 || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
                    //{
                    // return;
                    //}
                    //row = ds.Tables[0].Rows[0];
                    #endregion
                    string dashBoardFile = string.Empty;
                    if (taskResult.TaskStatistics.TaskSpecificData.AsDictionary.ContainsKey("DashboardFile") && !string.IsNullOrEmpty(taskResult.TaskStatistics.TaskSpecificData.AsDictionary["DashboardFile"].ToString()))
                    {
                        dashBoardFile = taskResult.TaskStatistics.TaskSpecificData.AsDictionary["DashboardFile"].ToString();
                    }

                    string dashboardXmlDirectoryPath = Path.GetDirectoryName(dashBoardFile);
                    if (!Directory.Exists(Application.StartupPath + dashboardXmlDirectoryPath))
                    {
                        Directory.CreateDirectory(Application.StartupPath + dashboardXmlDirectoryPath);
                    }
                    string refDataDirectoryPath = dashboardXmlDirectoryPath + @"\RefData";
                    if (!Directory.Exists(Application.StartupPath + refDataDirectoryPath))
                    {
                        Directory.CreateDirectory(Application.StartupPath + refDataDirectoryPath);
                    }

                    //clone datatable structure 
                    DataTable dtMatched = dtExceptionReport.Clone();
                    DataTable dtMisMatch = dtExceptionReport.Clone();
                    DataTable dtToleranceMatch = dtExceptionReport.Clone();

                    if (dtExceptionReport.Columns.Contains(ReconConstants.MismatchType))
                    {
                        //get EnumerableRowCollection datarow from linq Query from dtExceptionReport
                        EnumerableRowCollection<DataRow> enumerableMatched = from myRow in dtExceptionReport.AsEnumerable()
                                                                             where myRow.Field<string>(ReconConstants.MismatchType).Contains("Exactly")
                                                                             select myRow;
                        EnumerableRowCollection<DataRow> enumerableMisMatch = (from myRow in dtExceptionReport.AsEnumerable()
                                                                               where myRow.Field<string>(ReconConstants.MismatchType).Contains("Mis")
                                                                               select myRow);

                        EnumerableRowCollection<DataRow> enumerableToleranceMatch = (from myRow in dtExceptionReport.AsEnumerable()
                                                                                     where myRow.Field<string>(ReconConstants.MismatchType).Contains("olerance")
                                                                                     select myRow);
                        if (enumerableMatched.ToArray().Length > 0)
                        {
                            dtMatched = enumerableMatched.ToArray().CopyToDataTable();
                        }
                        if (enumerableMisMatch.ToArray().Length > 0)
                        {
                            dtMisMatch = enumerableMisMatch.ToArray().CopyToDataTable();
                        }
                        if (enumerableToleranceMatch.ToArray().Length > 0)
                        {
                            dtToleranceMatch = enumerableToleranceMatch.ToArray().CopyToDataTable();
                        }
                    }
                    //set table name  and write xml file of all tables
                    string task = string.Empty;
                    if (!string.IsNullOrEmpty(taskResult.ExecutionInfo.ExecutionName.ToString()))
                    {
                        task = taskResult.ExecutionInfo.ExecutionName.ToString();
                    }
                    string perfectMatchesFilePath = refDataDirectoryPath + @"\" + task + "_PerfectMatches" + ".xml";
                    dtMatched.TableName = "PerfectMatches";
                    XMLUtilities.WriteXML(dtMatched, Application.StartupPath + perfectMatchesFilePath);
                    //dtMatched.WriteXml(Application.StartupPath + perfectMatchesFilePath);

                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("PerfectMatches", dtMatched.Rows.Count, perfectMatchesFilePath);

                    string breaksFilePath = refDataDirectoryPath + @"\" + task + "_Breaks" + ".xml";
                    dtMisMatch.TableName = "Breaks";
                    XMLUtilities.WriteXML(dtMisMatch, Application.StartupPath + breaksFilePath);
                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("Breaks", dtMisMatch.Rows.Count, breaksFilePath);

                    string fallingWithInToleranceFilePath = refDataDirectoryPath + @"\" + task + "_FallingWithinTolerance" + ".xml";
                    dtToleranceMatch.TableName = "FallingWithinTolerance";
                    XMLUtilities.WriteXML(dtToleranceMatch, Application.StartupPath + fallingWithInToleranceFilePath);
                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("FallingWithinTolerance", dtToleranceMatch.Rows.Count, fallingWithInToleranceFilePath);

                    string dataPointsReconciledFilePath = refDataDirectoryPath + @"\" + task + "_DataPointsReconciled" + ".xml";
                    XMLUtilities.WriteXML(dtExceptionReport, Application.StartupPath + dataPointsReconciledFilePath);

                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("DataPointsReconciled", dtExceptionReport.Rows.Count, dataPointsReconciledFilePath);

                    #region Commented
                    //if (!ds.Tables[0].Columns.Contains("PerfectMatches"))
                    //{
                    //    ds.Tables[0].Columns.Add("PerfectMatches");
                    //}
                    //row["PerfectMatches"] = dtMatched.Rows.Count;
                    //if (!ds.Tables[0].Columns.Contains("PerfectMatchesRef"))
                    //{
                    //    ds.Tables[0].Columns.Add("PerfectMatchesRef");
                    //}
                    //row["PerfectMatchesRef"] = perfectMatchesFilePath;
                    //dtMisMatch.WriteXml(Application.StartupPath + breaksFilePath);
                    //if (!ds.Tables[0].Columns.Contains("Breaks"))
                    //{
                    //    ds.Tables[0].Columns.Add("Breaks");
                    //}
                    //row["Breaks"] = dtMisMatch.Rows.Count;
                    //if (!ds.Tables[0].Columns.Contains("BreaksRef"))
                    //{
                    //    ds.Tables[0].Columns.Add("BreaksRef");
                    //}
                    //row["BreaksRef"] = breaksFilePath;
                    // dtToleranceMatch.WriteXml(Application.StartupPath + fallingWithInToleranceFilePath);
                    // if (!ds.Tables[0].Columns.Contains("FallingWithinTolerance"))
                    // {
                    //     ds.Tables[0].Columns.Add("FallingWithinTolerance");
                    // }
                    // row["FallingWithinTolerance"] = dtToleranceMatch.Rows.Count;
                    // if (!ds.Tables[0].Columns.Contains("FallingWithinToleranceRef"))
                    // {
                    //     ds.Tables[0].Columns.Add("FallingWithinToleranceRef");
                    // }
                    // row["FallingWithinToleranceRef"] = fallingWithInToleranceFilePath;
                    // string dataPointsReconciledFilePath = refDataDirectoryPath + @"\" + row["Task"].ToString() + "_DataPointsReconciled" + ".xml";
                    // XMLUtilities.WriteXML(dtExceptionReport, Application.StartupPath + dataPointsReconciledFilePath);
                    //// dtExceptionReport.WriteXml(Application.StartupPath + dataPointsReconciledFilePath);
                    // if (!ds.Tables[0].Columns.Contains("DataPointsReconciled"))
                    // {
                    //     ds.Tables[0].Columns.Add("DataPointsReconciled");
                    // }
                    // row["DataPointsReconciled"] = dtExceptionReport.Rows.Count;
                    // if (!ds.Tables[0].Columns.Contains("DataPointsReconciledeRef"))
                    // {
                    //     ds.Tables[0].Columns.Add("DataPointsReconciledeRef");
                    // }
                    // row["DataPointsReconciledeRef"] = dataPointsReconciledFilePath;

                    // if (!ds.Tables[0].Columns.Contains("Comments"))
                    // {
                    //     ds.Tables[0].Columns.Add("Comments", typeof(string));
                    // }
                    //XMLUtilities.WriteXML(ds, dashBoardFilePath);
                    //ds.WriteXml(dashBoardFilePath);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                taskResult.Error = ex;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            if (taskResult != null)
            {
                taskResult.LogResult();
            }
        }
        /// <summary>
        /// Sorts the datatable for multiple columns respective to that recontype
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="reconTemplate"></param>
        /// <returns></returns>
        public static DataTable sortDataTable(DataTable dt, string dtSortingColumnOrder, List<MatchingRule> rulesList)
        {
            try
            {
                DataTable cloneTable = dt;
                if (string.IsNullOrWhiteSpace(dtSortingColumnOrder))
                {
                    return dt;
                }
                string[] columns = dtSortingColumnOrder.Split(',');
                StringBuilder sortingColumnOrder = new StringBuilder();
                foreach (string column in columns)
                {
                    //Removes ASC or DESC from the default string pattern which is to sort the column
                    string coll = column.Substring(0, column.IndexOf(' '));
                    //Only Sort column if available in datable
                    if (cloneTable.Columns.Contains(coll))
                    {
                        sortingColumnOrder.Append(coll).Append(column.Substring(coll.Length)).Append(",");
                    }

                    //to check if the rule list is passed null or not
                    //rule list is passed null in case of sorting from ReconOutput/Approve Changes UI

                    //modified by amit 24.03.2015
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-3092
                    if (rulesList != null && rulesList.Count > 0)
                    {
                        //to convert the Default string column to double if they contain numeric fields
                        if (rulesList[0].NumericFields.Contains(coll))
                        {
                            DataTable dtCloned = cloneTable.Clone();
                            if (dtCloned.Columns.Contains(coll))
                            {
                                dtCloned.Columns[coll].DataType = typeof(Double);
                                foreach (DataRow row in cloneTable.Rows)
                                {
                                    if (!string.IsNullOrWhiteSpace(row[coll].ToString()))
                                    {
                                        dtCloned.ImportRow(row);
                                    }
                                }
                                cloneTable = dtCloned;
                            }
                        }
                    }
                }
                DataView view = cloneTable.DefaultView;
                //sorts the dataview
                //sortingColumnOrder contains the column name and sorting type sepratted by ',' 
                if (sortingColumnOrder.Length > 0)
                {
                    view.Sort = sortingColumnOrder.ToString(0, sortingColumnOrder.Length - 1);
                }
                return view.ToTable();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dt;
        }

        //TODO: This method is duplicate of Prana.tools.NewUtilities
        private static void AddPrimaryKey(DataTable dt)
        {
            try
            {
                //CHMW-2074	Recon enhancement (View file being reconciled)
                if (!dt.Columns.Contains(ReconConstants.COLUMN_RowIndex))
                {
                    dt.Columns.Add(ReconConstants.COLUMN_RowIndex);
                }
                if (!dt.Columns.Contains(ReconConstants.PrimaryKey))
                {
                    dt.Columns.Add(ReconConstants.PrimaryKey);
                }
                int rowID = 0;

                foreach (DataRow row in dt.Rows)
                {
                    row[ReconConstants.PrimaryKey] = rowID;
                    row[ReconConstants.COLUMN_RowIndex] = rowID;
                    rowID++;
                }
                dt.PrimaryKey = new DataColumn[] { dt.Columns[ReconConstants.PrimaryKey] };
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
        /// 
        /// </summary>
        /// <param name="executionData"></param>
        /// <param name="result"></param>
        /// <param name="previousTaskResults"></param>
        protected override bool Execute(ExecutionInfo executionData, ref TaskResult result, List<TaskResult> previousTaskResults)
        {
            try
            {
                if (executionData != null)
                {
                    RunReconciliationForSelectedTemplate(executionData.InputObjects, result);
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
        /// 
        /// </summary>
        /// <param name="info"></param>
        protected override void InitializeTask(TaskInfo info)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Create the datasource for the Grid Control 
        /// </summary>
        /// <returns>Datatable holding the data</returns>
        public static void GetFileSettingDetails()
        {
            try
            {
                DataSet dsThirdPartyDetails = DatabaseManager.GetThirdPartyFileSettingDetails();
                //This dataset will contain FormatNAme and RunUpload Class
                ReconPreferences.DictRunUpload = FillRunUploadDetails(dsThirdPartyDetails);
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
        /// Fill run upload details fetched from database
        /// </summary>
        /// <param name="dsThirdPartyDetails"></param>
        /// <returns></returns>
        private static Dictionary<string, RunUpload> FillRunUploadDetails(DataSet dsThirdPartyDetails)
        {
            try
            {
                Dictionary<string, RunUpload> runUploadValueList = new Dictionary<string, RunUpload>();
                RunUpload runUpload;
                if (dsThirdPartyDetails == null || dsThirdPartyDetails.Tables.Count == 0 || dsThirdPartyDetails.Tables[0].Rows.Count == 0)
                {
                    return null;
                }
                DataTable dtThirdPartyFileSettingDetails = dsThirdPartyDetails.Tables[0];
                //Fill ftp details in business object FTPDetails
                foreach (DataRow dr in dtThirdPartyFileSettingDetails.Rows)
                {
                    if (runUploadValueList.Keys.Contains(Convert.ToString(dr["FormatName"]), StringComparer.InvariantCultureIgnoreCase))
                    {
                        runUploadValueList[Convert.ToString(dr["FormatName"])].LstAccountID.Add(Convert.ToInt32(dr["FundID"]));
                    }
                    else
                    {
                        runUpload = new RunUpload();
                        #region Fill runUpload
                        string[] formatName = Convert.ToString(dr["FormatName"]).Split('.');
                        //string thirdPartyTypeShortName = formatName[0];
                        //ThirdPartyName 
                        runUpload.DataSourceNameIDValue.ShortName = formatName[1];
                        //TODO: Third Party Short name is assigned to fullname, need to fetch full name from DB
                        //ThirdPartyName 
                        runUpload.DataSourceNameIDValue.FullName = formatName[1];
                        //ImportFormatName e.g. englTradeImport
                        //string ImportFormatName = formatName[3];
                        //FTPFolderPath
                        runUpload.FileName = Convert.ToString(dr["FTPFolderPath"]);
                        //LocalFolderPath
                        runUpload.FtpFilePath = Convert.ToString(dr["FTPFolderPath"]);
                        runUpload.FilePath = Convert.ToString(dr["LocalFolderPath"]);
                        runUpload.RawFilePath = Convert.ToString(dr["LocalFolderPath"]);
                        runUpload.ProcessedFilePath = Convert.ToString(dr["LocalFolderPath"]);
                        //XSLTPath-only xslt name



                        runUpload.DataSourceXSLT = Convert.ToString(dr["XSLTPath"]);
                        //XSDPath-only xsd name
                        runUpload.XSDName = Convert.ToString(dr["XSDPath"]);
                        //ThirdPartyID
                        runUpload.DataSourceNameIDValue.ID = Convert.ToInt32(dr["ThirdPartyID"]);
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-1834
                        if (!CachedDataManager.GetInstance.CompanyThirdPartyAccounts().ContainsKey(runUpload.DataSourceNameIDValue.ID))
                        {
                            continue;
                        }
                        int ftpID = Convert.ToInt32(dr["FtpID"]);
                        int decryptionID = Convert.ToInt32(dr["DecryptionID"]);
                        int emailID = Convert.ToInt32(dr["EmailID"]);

                        runUpload.IsPriceToleranceChecked = Convert.ToBoolean(dr["EnablePriceTolerance"]);
                        runUpload.PriceTolerance = Convert.ToDouble(dr["PriceTolerance"]);
                        runUpload.PriceToleranceColumns = Convert.ToString(dr["PriceToleranceColumns"]);

                        //read Recon Sp Name
                        runUpload.SPName = Convert.ToString(dr["ImportSPName"]);

                        runUpload.LstAccountID.Add(Convert.ToInt32(dr["FundID"]));

                        if (ftpID > 0)
                            runUpload.FtpDetails = ThirdPartyClientManager.ServiceInnerChannel.GetThirdPartyFtp(ftpID);

                        if (decryptionID > 0)
                            runUpload.DecryptionDetails = (ThirdPartyGnuPG)(ThirdPartyClientManager.ServiceInnerChannel.GetThirdPartyGnuPGForDecryption(decryptionID)[0]);

                        if (emailID > 0)
                            runUpload.EmailDetails = ThirdPartyClientManager.ServiceInnerChannel.GetThirdPartyEmail(emailID);
                        #endregion
                        runUploadValueList.Add(Convert.ToString(dr["FormatName"]), runUpload);
                    }
                }
                return runUploadValueList;
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

        /// <summary>
        /// Fill smdate if sm request is on
        /// </summary>
        public DataTable FillSMData()
        {
            try
            {
                SetUpTimer();
                if (!_isSMResponsewiredup)
                {
                    SecMasterHelper.SecurityMaster.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(_secMaster_SecMstrDataResponse);
                    //new SecMasterDataHandler(_secMaster_SecMstrDataResponse);
                    _isSMResponsewiredup = true;
                }
                _hashCode = this.GetHashCode();

                // NewUtilities.AddPrimaryKey(_dt);
                //timerSMRequest.Enabled = true;                       // Enable the timer
                //timerSMRequest.Interval = (1000) * (3);              // Timer will tick after every 3 second
                //timerSMRequest.Start();                             //start timer
                GetUniqueSymbolCollection(_dtPB);
                _dtPB = SecMasterHelper.GetSMData(_uniqueSymbolDict, _dtPB.Copy(), _hashCode);
                while (!_isTimerTick)
                {
                    Thread.Sleep(2000);
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
            return _dtPB;
        }

        /// <summary>
        /// checks weather SMRequest is to be sent or not
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private bool IsSecurityMasterRequestRequired(DataTable dt)
        {
            try
            {
                lock (syncLock)
                {
                    SetDataTableToValidate(dt);
                }
                if (_dtPB.Columns.Contains("SMRequest"))
                {
                    if (_dtPB.Rows[0]["SMRequest"].ToString().ToUpper().Equals("TRUE"))
                    {
                        return true;
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

        public void SetDataTableToValidate(DataTable dt)
        {
            try
            {
                _dtPB = dt;
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

        Dictionary<int, List<string>> _uniqueSymbolDict = new Dictionary<int, List<string>>();

        /// <summary>
        /// get unique symbol collection
        /// </summary>
        /// <param name="dt"></param>
        private void GetUniqueSymbolCollection(DataTable dt)
        {
            try
            {
                _uniqueSymbolDict.Clear();
                if (!dt.Columns.Contains(COLUMNSymbology))
                {
                    dt.Columns.Add(COLUMNSymbology, typeof(string));
                }
                foreach (DataRow dataRow in dt.Rows)
                {
                    if (string.IsNullOrEmpty(dataRow[COLUMNSymbology].ToString()))
                    {
                        FillSymbology(dataRow);
                    }
                    FillSymbol(dataRow);
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
        /// Fill Symbol in Data Row as per symbology
        /// </summary>
        /// <param name="dataRow"></param>
        private void FillSymbol(DataRow dataRow)
        {
            try
            {
                switch (dataRow[COLUMNSymbology].ToString())
                {
                    case RICSymbol:
                        UpdateDataRowWithSymbol(dataRow, 1, RICSymbol);
                        break;
                    case ISINSymbol:
                        UpdateDataRowWithSymbol(dataRow, 2, ISINSymbol);
                        break;
                    case SEDOLSymbol:
                        UpdateDataRowWithSymbol(dataRow, 3, SEDOLSymbol);
                        break;
                    case CUSIPSymbol:
                        UpdateDataRowWithSymbol(dataRow, 4, CUSIPSymbol);
                        break;
                    case BloombergSymbol:
                        UpdateDataRowWithSymbol(dataRow, 5, BloombergSymbol);
                        break;
                    case OSIOptionSymbol:
                        UpdateDataRowWithSymbol(dataRow, 6, OSIOptionSymbol);
                        break;
                    case IDCOOptionSymbol:
                        UpdateDataRowWithSymbol(dataRow, 7, IDCOOptionSymbol);
                        break;
                    case OpraOptionSymbol:
                        UpdateDataRowWithSymbol(dataRow, 8, OpraOptionSymbol);
                        break;
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
        /// Update Data Row With Symbol
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="key"></param>
        /// <param name="symbology"></param>
        private void UpdateDataRowWithSymbol(DataRow dataRow, int key, string symbology)
        {
            try
            {
                if (dataRow.Table.Columns.Contains(symbology))
                {
                    dataRow[symbology] = dataRow[symbology].ToString().ToUpper();
                    if (_uniqueSymbolDict.ContainsKey(key))
                    {
                        List<string> sameSymbolList = _uniqueSymbolDict[key];
                        if (!sameSymbolList.Contains(dataRow[symbology].ToString()))
                        {
                            sameSymbolList.Add(dataRow[symbology].ToString());
                        }
                    }
                    else
                    {
                        List<string> symbolList = new List<string>();
                        symbolList.Add(dataRow[symbology].ToString());
                        _uniqueSymbolDict.Add(key, symbolList);
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
        ///  Fill the Symbology of first column in which value is found
        /// </summary>
        /// <param name="row"></param>
        private void FillSymbology(DataRow row)
        {
            try
            {
                if (row.Table.Columns.Contains(RICSymbol) && !String.IsNullOrEmpty(row[RICSymbol].ToString()))
                {
                    row[COLUMNSymbology] = RICSymbol;
                    return;
                }
                else if (row.Table.Columns.Contains(ISINSymbol) && !String.IsNullOrEmpty(row[ISINSymbol].ToString()))
                {
                    row[COLUMNSymbology] = ISINSymbol;
                    return;
                }
                else if (row.Table.Columns.Contains(CUSIPSymbol) && !String.IsNullOrEmpty(row[CUSIPSymbol].ToString()))
                {
                    row[COLUMNSymbology] = CUSIPSymbol;
                    return;
                }
                else if (row.Table.Columns.Contains(SEDOLSymbol) && !String.IsNullOrEmpty(row[SEDOLSymbol].ToString()))
                {
                    row[COLUMNSymbology] = SEDOLSymbol;
                    return;
                }
                else if (row.Table.Columns.Contains(BloombergSymbol) && !String.IsNullOrEmpty(row[BloombergSymbol].ToString()))
                {
                    row[COLUMNSymbology] = BloombergSymbol;
                    return;
                }
                else if (row.Table.Columns.Contains(OSIOptionSymbol) && !String.IsNullOrEmpty(row[OSIOptionSymbol].ToString()))
                {
                    row[COLUMNSymbology] = OSIOptionSymbol;
                    return;
                }
                else if (row.Table.Columns.Contains(IDCOOptionSymbol) && !String.IsNullOrEmpty(row[IDCOOptionSymbol].ToString()))
                {
                    row[COLUMNSymbology] = IDCOOptionSymbol;
                    return;
                }
                else if (row.Table.Columns.Contains(OpraOptionSymbol) && !String.IsNullOrEmpty(row[OpraOptionSymbol].ToString()))
                {
                    row[COLUMNSymbology] = OpraOptionSymbol;
                    return;
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
        public delegate void SecMasterObjHandler(SecMasterBaseObj secMasterObj);

        /// <summary>
        /// asynchronous sec master response
        /// </summary>
        /// <param name="secMasterObj"></param>
        void _secMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                //todo: Need to handle this as we need to update datatable asynchronously for sec master response
                lock (syncLock)
                {
                    SecMasterBaseObj secMasterObj = e.Value;
                    FillObjFromSM(secMasterObj);
                    timerSMRequest.Stop();
                    timerSMRequest.Start();
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
        /// fill secmaster object
        /// </summary>
        /// <param name="secMasterObj"></param>
        private void FillObjFromSM(SecMasterBaseObj secMasterObj)
        {
            try
            {
                string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                int requestedSymbologyID = secMasterObj.RequestedSymbology;
                string requestedSymbology = TickerSymbol;
                switch (requestedSymbologyID)
                {
                    case 0:
                        requestedSymbology = TickerSymbol;
                        break;
                    case 1:
                        requestedSymbology = RICSymbol;
                        break;
                    case 2:
                        requestedSymbology = ISINSymbol;
                        break;
                    case 3:
                        requestedSymbology = SEDOLSymbol;
                        break;
                    case 4:
                        requestedSymbology = CUSIPSymbol;
                        break;
                    case 5:
                        requestedSymbology = BloombergSymbol;
                        break;
                    case 6:
                        requestedSymbology = OSIOptionSymbol;
                        break;
                    case 7:
                        requestedSymbology = IDCOOptionSymbol;
                        break;
                    case 8:
                        requestedSymbology = OpraOptionSymbol;
                        break;
                    default:
                        break;
                }

                if (_uniqueSymbolDict.ContainsKey(requestedSymbologyID))
                {
                    List<string> symbolList = _uniqueSymbolDict[requestedSymbologyID];
                    if (symbolList.Contains(secMasterObj.RequestedSymbol))
                    {
                        DataRow[] rows = _dtPB.Select(requestedSymbology + " = '" + secMasterObj.RequestedSymbol + "'");
                        foreach (DataRow dataRow in rows)
                        {
                            // http://jira.nirvanasolutions.com:8080/browse/CHMW-1752
                            if (dataRow != null && dataRow.ItemArray.Length > 0)
                            {
                                dataRow[TickerSymbol] = pranaSymbol;
                                //dataRow["CompanyName"] = secMasterObj.LongName;
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
        }

        /// <summary>
        /// put the timer value to false to terminate execution of while loop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timerSMRequest_Tick(object sender, EventArgs e)
        {
            try
            {
                _isTimerTick = true;
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
            timerSMRequest.Stop();
        }

        /// <summary>
        /// This method will prepare data for reconciliation and also fetch the previous batch name if exist
        /// </summary>
        /// <param name="templateKey"></param>
        /// <param name="ReconType"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filePath"></param>
        /// <param name="clientID"></param>
        public static void ExecuteTask(ReconParameters reconParameters)
        {
            try
            {
                if (reconParameters != null)
                {
                    //StringBuilder inputData = new StringBuilder();
                    //inputData.Append(templateKey).Append(Seperators.SEPERATOR_8);
                    //inputData.Append(ReconType).Append(Seperators.SEPERATOR_8);
                    //inputData.Append(fromDate).Append(Seperators.SEPERATOR_8);
                    //inputData.Append(toDate).Append(Seperators.SEPERATOR_8);
                    //inputData.Append(filePath).Append(Seperators.SEPERATOR_8);
                    //inputData.Append(clientID).Append(Seperators.SEPERATOR_8);
                    //inputData.Append(isReconReportToBeGenerated);

                    ExecutionInfo eInfo = DeepCopyHelper.Clone(TaskExecutionCache.Instance.GetExecutionInfo("Recon_-1"));
                    if (eInfo != null)
                    {
                        eInfo.InputObjects = new List<object>() { reconParameters };
                        eInfo.ExecutionName = reconParameters.TemplateKey + Seperators.SEPERATOR_6 + reconParameters.FromDate + Seperators.SEPERATOR_6 + reconParameters.ToDate + Seperators.SEPERATOR_6 + reconParameters.FormatName + Seperators.SEPERATOR_6 + reconParameters.ReconDateType + Seperators.SEPERATOR_6 + reconParameters.RunDate;
                        NirvanaTask task = ReconManager.Instance;
                        task.Initialize(eInfo.TaskInfo);

                        //purpose: Supply the dashboard filepath
                        string dashboardPath = CachedDataManagerRecon.GetExecutionDashboardFilePath(eInfo.ExecutionName);
                        if (!string.IsNullOrWhiteSpace(dashboardPath))
                        {
                            TaskResult result = new TaskResult();
                            result.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("DashboardFile", dashboardPath, dashboardPath);
                            //CHMW-2127	[Recon] Recon batch status is failure but on view and amend button , data is coming on UI
                            result.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("PerfectMatches", string.Empty, null);
                            result.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("Breaks", string.Empty, null);
                            result.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("FallingWithinTolerance", string.Empty, null);
                            result.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("DataPointsReconciled", string.Empty, null);
                            task.ExecuteTask(eInfo, result);
                        }
                        else
                        {
                            task.ExecuteTask(eInfo, null);
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
        }

        public static DataTable ProcessReconData(DataTable dt, string _reconTemplate, DataSourceType dsType, TaskResult taskResult)
        {
            try
            {
                #region Apply Filters
                ApplyFiltersOnDataTable(ref dt, _reconTemplate);
                #endregion
                #region Enrich data
                AddCustomColumns(ref dt, _reconTemplate, dsType, taskResult);
                #endregion
                #region group data
                GroupData(ref dt, _reconTemplate, dsType);
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
            return dt;
        }

        /// <summary>
        /// Add Custom Columns on DataTable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="_reconTemplate"></param>
        /// <param name="dsType"></param>
        /// <param name="taskResult"></param>
        /// <returns></returns>
        private static void AddCustomColumns(ref DataTable dt, string _reconTemplate, DataSourceType dsType, TaskResult taskResult)
        {
            try
            {
                //Logger.LoggerWrite("Started Adding custon column for " + dsType.ToString());
                List<ColumnInfo> listCustomColumns = ReconPrefManager.ReconPreferences.GetPranaCustomColumns(_reconTemplate, dsType);
                List<MatchingRule> listOfRules = ReconPrefManager.ReconPreferences.GetListOfRules(_reconTemplate);

                #region Create Custom Columns dictionary (CH Custom Columns)
                Dictionary<string, string> customColumns = GetCustomColumnDictionary(_reconTemplate, dsType);
                #endregion

                #region Add Custom Columns (Prana Custom Columns)
                AddPranaCustomColumns(ref dt, dsType, listCustomColumns, listOfRules, customColumns);
                #endregion

                #region Add Custom Columns (CH Custom Columns) CHMW-1080
                AddCHCustomColumns(dt, taskResult, customColumns);
                #endregion
                //Logger.LoggerWrite("Done Adding custom column for" + dsType.ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            //return dt;
        }

        private static void AddCHCustomColumns(DataTable dt, TaskResult taskResult, Dictionary<string, string> customColumns)
        {
            try
            {
                if (customColumns.Count > 0 && dt.Columns.Count > 0)
                {
                    customColumns.Keys.ToList().ForEach(customColumn => dt.Columns.Add(customColumn));

                    DataTableExpressionHelper.ConvertUGExpressionToDTExpression(customColumns);

                    foreach (string customColumn in customColumns.Keys)
                    {
                        try
                        {
                            dt.Columns[customColumn].Expression = customColumns[customColumn];

                        }
                        catch (System.Data.EvaluateException)
                        {
                            if (taskResult != null)
                            {
                                taskResult.Error = new Exception("Invalid formula expression: " + customColumns[customColumn] +
                                    ", for custom column " + customColumn + "." +
                                    System.Environment.NewLine + "Cannot perform numeric operations on non-numeric columns.");

                                dt.Columns[customColumn].Expression = "'Formula Error'";
                            }
                        }
                    }



                    //customColumns.Keys.ToList().ForEach
                    //    (customColumn =>
                    //    {
                    //        if (customColumns[customColumn] != null)
                    //        {
                    //            if (customColumns[customColumn].IndexOf("iif(", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    //            {
                    //                customColumns[customColumn] = customColumns[customColumn].ToString().Replace("iif(", "if(");
                    //            }
                    //        }
                    //    }
                    //    );
                    //frmTempCustomColumn form = new frmTempCustomColumn();
                    //form.InitializeComponent();
                    //customColumns.Keys.ToList().ForEach(customColumn => dt.Columns.Add(customColumn));
                    //form.ultraGrid1.DataSource = dt;
                    //foreach (UltraGridColumn item in form.ultraGrid1.DisplayLayout.Bands[0].Columns)
                    //{
                    //if (customColumns.Keys.Contains(item.Key))
                    //{
                    //IFormulaProvider formulaProvider = (IFormulaProvider)item;
                    //formulaProvider.Formula = customColumns[item.Key];
                    //}
                    //}
                    //form.ShowDialog();
                    //dt = (DataTable)form.ultraGrid1.DataSource;
                    //form.Dispose();
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

        private static DataTable AddPranaCustomColumns(ref DataTable dt, DataSourceType dsType, List<ColumnInfo> listCustomColumns, List<MatchingRule> listOfRules, Dictionary<string, string> customColumns)
        {
            try
            {
                if (listCustomColumns.Count > 0 || customColumns.Count > 0)
                {
                    if (dsType.Equals(DataSourceType.PrimeBroker))
                    {
                        DataTable dtNew = dt.Clone();
                        bool isdataTypeSet = false;
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!isdataTypeSet)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (listOfRules[0].NumericFields.Contains(column.ColumnName))
                                    {
                                        dtNew.Columns[column.ColumnName].DataType = typeof(double);
                                    }
                                }
                                isdataTypeSet = true;
                            }
                            dtNew.Rows.Add(dr.ItemArray);
                        }
                        dt = dtNew;
                    }

                    DataEnritcher.AddCustomColumns(dt, listCustomColumns);
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
            return dt;
        }

        private static Dictionary<string, string> GetCustomColumnDictionary(string _reconTemplate, DataSourceType dsType)
        {
            //http://jira.nirvanasolutions.com:8080/browse/CHMW-1080
            Dictionary<string, string> customColumns = new Dictionary<string, string>();
            try
            {
                DataSet dsCustomColumns = ReconPrefManager.ReconPreferences.GetCustomColumns(_reconTemplate);
                if (dsCustomColumns != null && dsCustomColumns.Tables.Contains(ReconConstants.CustomColumnsTableName))
                {
                    if (dsType == DataSourceType.Nirvana)
                    {
                        customColumns = dsCustomColumns.Tables[ReconConstants.CustomColumnsTableName].AsEnumerable().ToDictionary<DataRow, string, string>(row => row.Field<string>(ReconConstants.COLUMN_Name), row => row.Field<string>(ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_FormulaExpression));
                    }
                    else
                    {
                        customColumns = dsCustomColumns.Tables[ReconConstants.CustomColumnsTableName].AsEnumerable().ToDictionary<DataRow, string, string>(row => row.Field<string>(ReconConstants.COLUMN_Name), row => row.Field<string>(ReconConstants.CONST_Broker + ReconConstants.COLUMN_FormulaExpression));
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
            return customColumns;
        }

        /// <summary>
        /// Groups Data
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="_reconTemplate"></param>
        /// <param name="dsType"></param>
        private static void GroupData(ref DataTable dt, string _reconTemplate, DataSourceType dsType)
        {
            try
            {
                GroupingCriteria groupingCriteria = ReconPrefManager.ReconPreferences.GetGroupingCriteria(_reconTemplate);
                ReconTemplate template = ReconPrefManager.ReconPreferences.GetTemplates(_reconTemplate);
                //if (rules != null)
                //{
                if (template != null)
                {
                    UpdateTableSchemaForCustomColumns(dt);
                    //For CH user disable grouping
                    // if (CachedDataManager.GetPranaReleaseType() != PranaReleaseViewType.CHMiddleWare)
                    {
                        Logger.LoggerWrite("Started Grouping " + dsType.ToString() + " Data");
                        GroupingComponent.Group(dt, groupingCriteria, template.ReconType, template.DictGroupingSummary);
                        Logger.LoggerWrite("Done Grouping " + dsType.ToString() + " Data");
                    }
                    //BindData() method commented and moved to runworkercompleted because it invokes main ui controls                    
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
        /// Apply Recon Filters On Data Table
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="_reconTemplate"></param>
        /// <returns></returns>
        private static void ApplyFiltersOnDataTable(ref DataTable dt, string _reconTemplate)
        {
            try
            {
                Dictionary<ReconFilterType, Dictionary<int, string>> dictReconFilters = ReconPrefManager.ReconPreferences.GetReconFilters(_reconTemplate);
                //Logger.LoggerWrite("recon filters fetched for the recon template");
                if (dictReconFilters != null)
                {
                    //Logger.LoggerWrite("Started filtering Data for " + dsType.ToString());
                    //Logger.LoggerWrite("Datatable have " + dt.Rows.Count + " rows before filtering");
                    dt = FilteringLogic.GetFilteredData(dictReconFilters, dt);
                    //Logger.LoggerWrite("Done filtering Data for " + dsType.ToString());
                    //dt = dtFiltered;
                    //Logger.LoggerWrite("Datatable have " + dtFiltered.Rows.Count + " rows after filtering");
                }
                //return dt;
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

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_closingServicesProxy != null)
                {
                    _closingServicesProxy.Dispose();
                }
                ThirdPartyClientManager.Dispose();
                timerSMRequest.Dispose();
                _dtPB.Dispose();
            }
        }
        #endregion

        public Dictionary<string, string> GetDictionaryForClosing(ReconParameters reconParameters)
        {
            Dictionary<string, string> closingStatus = new Dictionary<string, string>();
            try
            {
                Dictionary<string, TaxlotClosingInfo> dictclosingStatus = _closingServicesProxy.InnerChannel.GetClosingStatusInfo(reconParameters.DTToDate, DateTime.MinValue, reconParameters.DTToDate, DateTime.MinValue);
                foreach (KeyValuePair<string, TaxlotClosingInfo> item in dictclosingStatus)
                {
                    if (!closingStatus.ContainsKey(item.Key))
                        closingStatus.Add(item.Key, item.Value.ClosingStatus.ToString());
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
            return closingStatus;
        }

        private HashSet<string> GetCommonRule(List<ColumnInfo> AvailableColumnList, List<ColumnInfo> SelectedColumnList)
        {
            HashSet<string> hSet = new HashSet<string>();
            try
            {
                AvailableColumnList.ForEach(x =>
                {
                    if (x.GroupType.ToString().Equals("Common"))
                    {
                        hSet.Add(x.ColumnName);
                    }
                });

                SelectedColumnList.ForEach(x =>
                {
                    if (x.GroupType.ToString().Equals("Common"))
                    {
                        hSet.Add(x.ColumnName);
                    }
                });
                hSet.Remove("MismatchType");
                hSet.Remove("Matched");
                hSet.Remove("MismatchDetails");

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return hSet;
        }
    }
}
