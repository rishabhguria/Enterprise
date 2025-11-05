using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
//TODO:Gaurav :  All the import related stuff must be moved here
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Import.Helper;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.TaskManagement.Definition.Definition;
using Prana.TaskManagement.Execution;
using Prana.Utilities.DateTimeUtilities;
using Prana.Utilities.ImportExportUtilities;
using Prana.WCFConnectionMgr;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.Import
{
    /// <summary>
    /// Need to implement ILiveFeedCallback as this class is creating Pricing service proxy
    /// </summary>
    public class ImportManager : NirvanaTask, ILiveFeedCallback, IDisposable
    {
        #region singleton
        /// <summary>
        /// The instance
        /// </summary>
        private static volatile ImportManager instance;

        /// <summary>
        /// The synchronize root
        /// </summary>
        private static object syncRoot = new Object();

        /// <summary>
        /// The _is scheduler running
        /// </summary>
        static bool _isSchedulerRunning = false;


        public event System.EventHandler<EventArgs<TaskResult>> ImportCompleted;

        /// <summary>
        /// Prevents a default instance of the <see cref="ImportManager"/> class from being created.
        /// </summary>
        private ImportManager()
        {
            try
            {
                StagedOrderHandler.Instance.GenerateNewAllocationScheme += new EventHandler<EventArgs<DataSet, string, List<SecMasterBaseObj>>>(GenerateAllocationScheme);
                StagedOrderHandler.Instance.ImportCompleted += new EventHandler<EventArgs<TaskResult>>(OnAutoStageImportCompleted);

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public static ImportManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ImportManager();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region Services

        ProxyBase<IAllocationManager> _proxyAllocationServices = null;
        private void CreateAllocationServicesProxy()
        {
            _proxyAllocationServices = new ProxyBase<IAllocationManager>("TradeAllocationServiceNewEndpointAddress");
            //This topic will Publish the acknowldegment for each chunk saved through server.
            this._allocationServices = _proxyAllocationServices;
        }

        ProxyBase<IAllocationManager> _allocationServices = null;
        public ProxyBase<IAllocationManager> AllocationServices
        {
            set
            {
                _allocationServices = value;
            }
            get
            {
                return _allocationServices;
            }
        }

        private void CreatePricingServiceProxy()
        {
            try
            {
                _pricingServicesProxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", this);
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

        DuplexProxyBase<IPricingService> _pricingServicesProxy = null;
        public DuplexProxyBase<IPricingService> PricingServices
        {
            set
            {
                _pricingServicesProxy = value;
            }
            get
            {
                return _pricingServicesProxy;
            }
        }

        public void CreateCashManagementProxy()
        {
            CashManagementServices = new ProxyBase<ICashManagementService>("TradeCashServiceEndpointAddress");
        }

        ProxyBase<ICashManagementService> _CashManagementServices = null;
        public ProxyBase<ICashManagementService> CashManagementServices
        {
            set
            {
                _CashManagementServices = value;

            }
            get { return _CashManagementServices; }
        }

        #endregion

        public static event EventHandler _launchForm;
        public static int DefaultEquityAUECID;

        public void Initialize(ISecurityMasterServices securityMasterServices)
        {
            try
            {
                CreateAllocationServicesProxy();
                CreatePricingServiceProxy();
                CreateCashManagementProxy();
                ServiceManager.Instance.AllocationServices = _allocationServices;
                ServiceManager.Instance.CashManagementServices = _CashManagementServices;
                ServiceManager.Instance.PricingServices = _pricingServicesProxy;
                SecurityMasterManager.Instance.SecurityMaster = securityMasterServices;
                AutoImportManager.Instance.StartWatchersForAutoImport();
                //StartScheduler();
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
        /// Get batch schedules for import and add batches to the quartz scheduler
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetImportBatchSchedules()
        {
            Dictionary<string, string> dictJobCron = new Dictionary<string, string>();
            try
            {
                Dictionary<String, ExecutionInfo> executionInfoCollection = TaskExecutionCache.Instance.GetExecutionInfoCollection();

                foreach (KeyValuePair<String, ExecutionInfo> kvp in executionInfoCollection)
                {
                    if (!string.IsNullOrEmpty(kvp.Value.CronExpression))
                    {
                        dictJobCron.Add(kvp.Key, kvp.Value.CronExpression);
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
            return dictJobCron;
        }

        /// <summary>
        /// UploadDataThruLocalFile method calls UpdateTaskSpecificDataPoints using event handler
        /// so we don't need to write runupload xml, it will be written automatically
        /// </summary>
        /// <param name="runUpload"></param>
        /// <param name="taskResult"></param>
        /// <param name="isSaveDataInApplication"></param>
        public void UploadDataThruLocalFile(RunUpload runUpload, TaskResult taskResult, bool isSaveDataInApplication)
        {
            try
            {
                if (SecurityMasterManager.Instance.SecurityMaster.IsConnected)
                {
                    //clear all lists and dictionaries
                    //TODO: Gaurav : Has to find out the way out for this
                    //ClearAll();

                    //TODO:Gaurav
                    //if (IsTradeServerConnected(strTableType))
                    //{
                    //    return;
                    //}

                    DataTable dtData = null;
                    IImportHandler handler = ImportHandlerManager.Instance.GetHandler(runUpload.ImportTypeAcronym);
                    if (handler != null)
                    {
                        //here path is taken to pass to XSLTManager incase if user has not defined the xsd.
                        string handlerXSDName;
                        if (String.IsNullOrWhiteSpace(runUpload.XSDName))
                            handlerXSDName = handler.GetXSDName();
                        else
                            handlerXSDName = runUpload.XSDName;

                        if (runUpload.ImportDataSource == ImportDataSource.Function)
                        {
                            DateTime date = DateTime.Today;
                            if (runUpload.IsUserSelectedDate && !runUpload.SelectedDate.Equals(String.Empty) && !runUpload.SelectedDate.Equals(DateTime.MinValue))
                            {
                                date = Convert.ToDateTime(runUpload.SelectedDate.ToString());
                            }
                            // specific for Allocation scheme App position Import. 
                            // If another import created using data from a function then hive off the logic to ImportHandlerManager
                            dtData = AllocationSchemeImportHelper.GetPositions(date);
                        }
                        else
                        {
                            dtData = FileReaderFactory.GetDataTableFromDifferentFileFormats(runUpload.ProcessedFilePath);
                        }
                        IImportITaskHandler taskHandler = (IImportITaskHandler)handler;
                        taskHandler.UpdateTaskSpecificDataPoints += new EventHandler(UpdateTaskSpecificDataPoints);
                        if (dtData != null)
                        {
                            dtData = ArrangeTable(dtData);

                            //DataTable dtCopy = dtData.Copy();
                            //DataSet dsToSerialize = dtData.DataSet;
                            //dsToSerialize.Tables.Add("ImportSettings");
                            //dsToSerialize.Tables["ImportSettings"].Columns.Add("COL1", typeof(string));
                            //dsToSerialize.Tables["ImportSettings"].Rows.Add(new Object[]{"*" as object});
                            // Purpose : To check if data is validated from xslt and xsd file
                            string errorMsg = string.Empty;
                            bool isValidated = false;
                            DataSet ds = XSLTManager.GetData(dtData, runUpload, handlerXSDName, out errorMsg, out isValidated, taskResult);

                            if (!isValidated)
                            {
                                Logger.LoggerWrite("[REBALANCER] Problem in XSLT/XSD validation.");
                                ImportLoggingHelper.LoggerWriteMessage(runUpload.ImportTypeAcronym.ToString(), runUpload.FileName, ImportLoggingHelper.EXCEPTION, "[REBALANCER] Problem in XSLT/XSD validation.");
                            }
                            if (ds != null && ds.Tables.Count > 0)
                            {
                                // Now we have arranged and updated XML
                                // as above we inserted "*" in the blank columns, but "*" needs extra treatment, so
                                // again we replace the "*" with blank string, the following looping does the same
                                ReArrangeDataSet(ds);
                                if (taskResult != null)
                                {
                                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("OriginalData", string.Empty, dtData);
                                }
                                if (handler != null)
                                {
                                    handler.ProcessRequest(ds, runUpload, taskResult, isSaveDataInApplication);
                                }
                            }
                            else
                            {
                                if (taskResult != null)
                                {
                                    if (isValidated)
                                    {
                                        taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("Comments", "No Data", null);
                                        taskResult.Error = new Exception("No Data");
                                    }
                                    else
                                    {
                                        taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("Comments", "Validation failed", null);
                                        taskResult.Error = new Exception(errorMsg);
                                    }
                                    UpdateTaskSpecificDataPoints(this, taskResult);
                                }
                            }
                        }
                        else
                        {
                            taskResult.Error = new Exception("Error in Import");
                        }
                        //taskHandler.UpdateTaskSpecificDataPoints -= new EventHandler(UpdateTaskSpecificDataPoints);
                    }
                    else
                    {
                        string errorMsg = "[REBALANCER] Import handler not assigned. Please check in database table PM_TableTypes";
                        Logger.LoggerWrite(errorMsg);
                        ImportLoggingHelper.LoggerWriteMessage(runUpload.ImportTypeAcronym.ToString(), runUpload.FileName, ImportLoggingHelper.EXCEPTION, "[REBALANCER] Import handler not assigned. Please check in database table PM_TableTypes");
                        taskResult.Error = new Exception(errorMsg);
                    }
                }
                else
                {
                    string errorMsg = "Trade Server disconnected.Can not import the file.";
                    MessageBox.Show(errorMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    taskResult.Error = new Exception(errorMsg);
                }
            }
            catch (Exception ex)
            {
                //Log the error in Import Logging as well
                ImportLoggingHelper.LoggerWriteMessage(runUpload.ImportTypeAcronym.ToString(), runUpload.FileName, ImportLoggingHelper.EXCEPTION, ex.Message);

                if (taskResult != null && runUpload.ImportTypeAcronym == ImportType.StagedOrder)
                {
                    taskResult.Error = ex;
                    UpdateTaskSpecificDataPoints(this, taskResult);
                    if (ImportCompleted != null)
                        ImportCompleted(this, new EventArgs<TaskResult>(taskResult));

                }
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
        /// Generates the allocation scheme, used for dynamic allocation scheme generation for stage import grouped orders
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs{DataSet, System.String}"/> instance containing the event data.</param>
        public void GenerateAllocationScheme(object sender, EventArgs<DataSet, string, List<SecMasterBaseObj>> e)
        {
            try
            {
                DataSet ds = e.Value;
                string FileName = e.Value2;
                List<SecMasterBaseObj> secMasterResponseStagedOrder = e.Value3;
                StagedOrderHandler.Instance.AllocationSchemeID = AllocationSchemeHandler.Instance.CreateScheme(ds, FileName, secMasterResponseStagedOrder);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

        }
        /// <summary>
        /// ProcessFormatedData method calls UpdateTaskSpecificDataPoints using event handler
        /// so we don't need to write runupload xml, it will be written automatically
        /// This loads data from existing file exist to avoid overwriting previous data.          
        /// </summary>
        /// <param name="runUpload"></param>
        /// <param name="taskResult"></param>
        /// <param name="isSaveDataInApplication"></param>
        public void ProcessFormatedData(RunUpload runUpload, TaskResult taskResult, bool isSaveDataInApplication, DataSet dsPositionMaster)
        {
            try
            {
                if (SecurityMasterManager.Instance.SecurityMaster.IsConnected)
                {
                    //Save the userId  in the task result
                    int userID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("UserName", userID, userID);

                    // Purpose : To proceed re-run symbol validation with processed file instead of ImportData xml.

                    //string executionName = Path.GetFileName(taskResult.GetDashBoardXmlPath());
                    //string refDataDirectoryPath = Path.GetDirectoryName(taskResult.GetDashBoardXmlPath()) + @"\RefData";

                    //string formattedFilePath = Application.StartupPath + refDataDirectoryPath + @"\" + executionName + "_ImportData" + ".xml";

                    if (!string.IsNullOrEmpty(Path.GetFileName(runUpload.ProcessedFilePath)))
                    {
                        runUpload.FileName = Path.GetFileName(runUpload.ProcessedFilePath);
                        runUpload.FilePath = runUpload.ProcessedFilePath;
                    }
                    DataSet ds = new DataSet();

                    if (dsPositionMaster == null)
                    {

                        ////Modified by Faisal Shah
                        ////http://jira.nirvanasolutions.com:8080/browse/CHMW-1331
                        //if (File.Exists(formattedFilePath))
                        //{
                        //    //CHMW-2181	[Reconciliation] [Code Review] Replace dataset read xml with BufferedStream
                        //    ds = XMLUtilities.ReadXmlUsingBufferedStream(formattedFilePath);
                        //    //ds.ReadXml(formattedFilePath);
                        //}
                        if (File.Exists(runUpload.FilePath))
                        {
                            UploadDataThruLocalFile(runUpload, taskResult, isSaveDataInApplication);
                        }
                    }
                    else
                    {
                        ds = dsPositionMaster;
                        IImportHandler handler = ImportHandlerManager.Instance.GetHandler(runUpload.ImportTypeAcronym);
                        if (handler != null)
                        {
                            IImportITaskHandler taskHandler = (IImportITaskHandler)handler;
                            taskHandler.UpdateTaskSpecificDataPoints += new EventHandler(UpdateTaskSpecificDataPoints);
                            if (ds != null && ds.Tables.Count > 0)
                            {
                                if (handler != null)
                                {
                                    handler.ProcessRequest(ds, runUpload, taskResult, isSaveDataInApplication);
                                }
                            }
                            else
                            {
                                taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("Comments", "No Data", null);
                                UpdateTaskSpecificDataPoints(this, taskResult);
                            }
                            //taskHandler.UpdateTaskSpecificDataPoints -= new EventHandler(UpdateTaskSpecificDataPoints);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Trade Server disconnected.Cannot import the file.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                taskResult.Error = ex;
                UpdateTaskSpecificDataPoints(this, taskResult);
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateTaskSpecificDataPoints(object sender, EventArgs e)
        {
            try
            {

                TaskResult taskResult = e as TaskResult;
                if (taskResult.ExecutionInfo.TaskInfo != null)
                {
                    this.ExecutionComplete(taskResult);

                    //added by: Bharat raturi, 25 jun 2014
                    //purpose: update the dictionary when the batch is completed
                    //if (!taskResult.TaskStatistics.Status.ToString().ToLower().Equals("running"))
                    //{
                    string dashboardFile = taskResult.TaskStatistics.TaskSpecificData.GetValueForKey("DashboardFile").ToString();
                    string batchKey = Path.GetFileName(dashboardFile);
                    string batchValue = dashboardFile;
                    ImportCacheManager.AddItemInDashBoardFileCache(batchKey, batchValue);
                    //}
                }
            }
            catch (Exception ex)
            {
                TaskResult taskResult = e as TaskResult;
                taskResult.Error = ex;
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
        /// Add runupload info to task result
        /// Run uplaod info will be updated for each task of dashboard
        /// </summary>
        /// <param name="taskResult"></param>
        private static void AddRunUploadDataToTaskResult(TaskResult taskResult, RunUpload runUpload)
        {
            try
            {
                string executionName = Path.GetFileName(taskResult.GetDashBoardXmlPath());
                string dashboardXmlDirectoryPath = Path.GetDirectoryName(taskResult.GetDashBoardXmlPath());
                if (!Directory.Exists(Application.StartupPath + dashboardXmlDirectoryPath))
                {
                    Directory.CreateDirectory(Application.StartupPath + dashboardXmlDirectoryPath);
                }

                string refDataDirectoryPath = dashboardXmlDirectoryPath + @"\RefData";
                if (!Directory.Exists(Application.StartupPath + refDataDirectoryPath))
                {
                    Directory.CreateDirectory(Application.StartupPath + refDataDirectoryPath);
                }

                //Save runUpload info with import task statistics data
                string runUploadFilePath = refDataDirectoryPath + @"\" + executionName + "_RunUpload" + ".xml";

                using (XmlTextWriter writer = new XmlTextWriter(Application.StartupPath + runUploadFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(RunUpload));
                    serializer.Serialize(writer, runUpload);
                    writer.Flush();
                }
                taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("RunUpload", 1, runUploadFilePath);
            }
            catch (Exception ex)
            {
                taskResult.Error = ex;
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }



        //This method moved to FileReader
        #region commented
        //public DataTable GetDataTableFromDifferentFileFormats(string fileName)
        //{
        //    DataTable datasourceData = null;
        //    try
        //    {
        //        string fileFormat = fileName.Substring(fileName.LastIndexOf(".") + 1);

        //        switch (fileFormat.ToUpperInvariant())
        //        {
        //            case "CSV":
        //                datasourceData = FileReaderFactory.Create(DataSourceFileFormat.Csv).GetDataTableFromUploadedDataFile(fileName);
        //                break;
        //            case "XLS":
        //                datasourceData = FileReaderFactory.Create(DataSourceFileFormat.Excel).GetDataTableFromUploadedDataFile(fileName);
        //                break;
        //            default:
        //                datasourceData = FileReaderFactory.Create(DataSourceFileFormat.Default).GetDataTableFromUploadedDataFile(fileName);
        //                break;
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
        //    return datasourceData;
        //}
        #endregion

        public DataTable ArrangeTable(DataTable dataSource)
        {
            try
            {
                // what XML we will generate, all the tagname will be like COL1,COL2 .                
                dataSource.TableName = "PositionMaster";

                // update the Table columns value with "*" where columns value blank in the excel sheet
                // when we generate the XML for that table, the blank coluns do not comes in the generated XML
                // the indexing of the generated XML changed because of blank columns
                // so defalut value of the columns will be  "*"
                for (int irow = 0; irow < dataSource.Rows.Count; irow++)
                {
                    for (int icol = 0; icol < dataSource.Columns.Count; icol++)
                    {
                        string val = dataSource.Rows[irow].ItemArray[icol].ToString();
                        if (String.IsNullOrEmpty(val.Trim()))
                        {
                            dataSource.Rows[irow][icol] = "*";
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
            return dataSource;
        }

        /// <summary>
        /// Now we have arranged and updated XML
        /// as above we inserted "*" in the blank columns, but "*" needs extra treatment, so        
        ///again we replace the "*" with blank string, the following looping does the same
        /// </summary>
        /// <param name="ds"></param>
        public void ReArrangeDataSet(DataSet ds)
        {
            try
            {
                for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
                {
                    for (int icol = 0; icol < ds.Tables[0].Columns.Count; icol++)
                    {
                        string val = ds.Tables[0].Rows[irow].ItemArray[icol].ToString();
                        if (val.Equals("*"))
                        {
                            ds.Tables[0].Rows[irow][icol] = string.Empty;
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
        }


        /// <summary>
        /// Run complete import process
        /// This method is used in complete import process
        /// i.e. run import process by taking input as format name.
        /// TODO: Instead of passing parameters in this method, we should create a class which will contain all the import parameters
        /// </summary>
        /// <param name="formatName"></param>
        public void RunImportProcess(string formatName, string uploadedFilePath, TaskResult taskResult, bool isPromptForDuplicityCheck, DateTime userSelectedDate, int userSelectedAccount, bool isUserSelectedDate, bool isUserSelectedAccount)
        {
            try
            {
                //string mailSubject = "File Import Error.";
                //StringBuilder mailBody = new StringBuilder();
                //mailBody.AppendLine("File Import Error.");
                //DataSet dsThirdPartyDetails = ImportDataManager.GetThirdPartyFileSettingDetails(formatName);
                //This dataset will contain ftpid and decryption id

                RunUpload runUpload = ImportDataManager.FillRunUploadDetails(formatName);

                if (runUpload == null)
                {
                    MessageBox.Show("Import batch is either deleted or modified", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Todo: Mail has to be send here also.
                    taskResult.Error = new Exception("RunUpload object is null");
                    UpdateTaskSpecificDataPoints(this, taskResult);
                    return;
                }

                //runUpload.ImportDataSource = ImportHandlerManager.GetImportDataSource(runUpload.ImportTypeAcronym);

                // Add user selected date and account to run upload

                if (isUserSelectedDate)
                {
                    runUpload.IsUserSelectedDate = true;
                    runUpload.SelectedDate = userSelectedDate;
                }
                else
                {
                    runUpload.IsUserSelectedDate = false;
                }
                if (isUserSelectedAccount)
                {
                    runUpload.IsUserSelectedAccount = true;
                    runUpload.SelectedAccount = userSelectedAccount;
                }
                else
                {
                    runUpload.IsUserSelectedAccount = false;
                }

                taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ThirdPartyType", runUpload.DataSourceNameIDValue.ShortName, null);
                taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ThirdPartyID", runUpload.DataSourceNameIDValue.ID, null);
                taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("FileType", runUpload.ImportTypeAcronym.ToString(), null);
                taskResult.LogResult();
                if (runUpload.ImportDataSource == ImportDataSource.Function)
                {
                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("DashboardFile", taskResult.GetDashBoardXmlPath() + ".xml", null);
                    UploadDataThruLocalFile(runUpload, taskResult, false);
                }
                else
                {
                    RunUpload(uploadedFilePath, runUpload, taskResult, isPromptForDuplicityCheck);
                }
            }
            catch (Exception ex)
            {
                taskResult.Error = ex;
                UpdateTaskSpecificDataPoints(this, taskResult);
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
        /// 
        /// </summary>
        /// <param name="executionData"></param>
        /// <param name="result"></param>
        /// <param name="previousTaskResults"></param>
        protected override bool Execute(ExecutionInfo executionData, ref TaskResult result, List<TaskResult> previousTaskResults)
        {
            try
            {
                //List<Object> importedList = new List<object>();
                //importedList.Add(15);
                //TDOD: Need to improve this code
                string[] inputData = executionData.InputData.Split(new string[] { Seperators.SEPERATOR_8 }, StringSplitOptions.None);

                DateTime userSelectedDate = DateTime.Today;
                int userSelectedAccount = int.MinValue;
                bool isUserSelectedDate = false;
                bool isUserSelectedAccount = false;
                if (inputData.Length > 3)
                {
                    if (!string.IsNullOrEmpty(inputData[2]))
                    {
                        DateTime dateTim;
                        if (DateTime.TryParse(inputData[2], out dateTim))
                        {
                            userSelectedDate = dateTim;
                            isUserSelectedDate = true;
                        }
                        else
                            isUserSelectedDate = false;
                    }
                    else
                    {
                        isUserSelectedDate = false;
                    }
                    if (!string.IsNullOrEmpty(inputData[3]))
                    {
                        int accountIdTemp;
                        if (Int32.TryParse(inputData[3], out accountIdTemp))
                        {
                            userSelectedAccount = accountIdTemp;
                            isUserSelectedAccount = true;
                        }
                        else
                            isUserSelectedAccount = false;
                    }
                    else
                    {
                        isUserSelectedAccount = false;
                    }
                }

                //Save the userId  in the task result
                int userID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                if (_isSchedulerRunning)
                {
                    result.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("UserName", "Scheduler", "Scheduler");
                    _isSchedulerRunning = false;
                }
                else
                {
                    result.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("UserName", userID, userID);
                }
                if (executionData.InputObjects != null && executionData.InputObjects.Count > 0)
                {
                    string FtpFilePath = executionData.InputObjects[0] as string;
                    if (!string.IsNullOrEmpty(FtpFilePath))
                    {
                        result.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("FtpFilePath", FtpFilePath, FtpFilePath);
                        result.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("FileName", Path.GetFileName(FtpFilePath), Path.GetFileName(FtpFilePath));
                    }
                    if (executionData.InputObjects.Count > 1 && !string.IsNullOrEmpty(FtpFilePath))
                    {
                        if (!string.IsNullOrWhiteSpace(executionData.InputObjects[1].ToString()))
                        {
                            DateTime dtExecutionDate = DateTime.Parse(executionData.InputObjects[1].ToString());
                            result.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ExecutionDate", dtExecutionDate.ToString(ApplicationConstants.DateFormat), dtExecutionDate.ToString(ApplicationConstants.DateFormat));
                        }
                    }

                    //added by: Bharat Raturi, 25 jun 2014
                    //purpose: Supply the dashboardpath already
                    //var execBatch=ImportCacheManager.DictDashboardBatches.Keys.Select(key=> key.Contains(executionData.ExecutionName)); 
                    //if (result!=null && execBatch!=null)
                    //{
                    //    result.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("DashboardFile",ImportCacheManager.DictDashboardBatches[execBatch.ToString()],ImportCacheManager.DictDashboardBatches[execBatch.ToString()]);
                    //}
                }

                //uploaded file path is passed to the run import process
                result.LogResult();
                if (inputData.Length > 1 && !string.IsNullOrEmpty(inputData[1]))
                {
                    string[] inputDataNew = inputData[1].Split(Seperators.SEPERATOR_6);
                    if (inputDataNew.Length > 1 && !string.IsNullOrEmpty(inputDataNew[1]))
                    {
                        RunImportProcess(inputData[0], inputDataNew[0], result, true, userSelectedDate, userSelectedAccount, isUserSelectedDate, isUserSelectedAccount);
                    }
                    else
                        RunImportProcess(inputData[0], inputDataNew[0], result, false, userSelectedDate, userSelectedAccount, isUserSelectedDate, isUserSelectedAccount);
                }
                else
                {
                    string[] inputDataNew = executionData.InputData.Split(Seperators.SEPERATOR_6);
                    if (inputDataNew.Length > 1 && !string.IsNullOrEmpty(inputDataNew[1]))
                    {
                        RunImportProcess(inputDataNew[0], null, result, true, userSelectedDate, userSelectedAccount, isUserSelectedDate, isUserSelectedAccount);
                    }
                    else
                        RunImportProcess(inputDataNew[0], null, result, false, userSelectedDate, userSelectedAccount, isUserSelectedDate, isUserSelectedAccount);
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
        /// This method will run only upload process
        /// </summary>
        /// <param name="runUpload"></param>
        private bool RunUploadProcess(RunUpload runUpload, bool isPromptForDuplicityCheck, TaskResult taskResult)
        {
            bool isFileUploadedSuccessfully = false;
            bool isCheckForFileDuplicityCheck = true;
            try
            {
                string mailSubject = "File Import Error.";
                StringBuilder mailBody = new StringBuilder();
                mailBody.AppendLine("File Import Error.");

                if (taskResult != null)
                {
                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("DashboardFile", taskResult.GetDashBoardXmlPath() + ".xml", null);
                }


                #region 1. ParseFileName
                #region Get Last Business Date
                DefaultEquityAUECID = int.Parse(ConfigurationManager.AppSettings["DefaultEquityAUECID"]);
                DateTime LastBusinessDay = BusinessDayCalculator.GetInstance().GetPreviousBusinessDay(DateTime.UtcNow.Date, DefaultEquityAUECID);
                #endregion
                string fileName = RunUploadProcessHelper.ParseFileName(runUpload, LastBusinessDay);

                #endregion

                #region 2. DownloadFileFromFTP
                Dictionary<string, Tuple<string, string>> dictTaskResult = new Dictionary<string, Tuple<string, string>>();
                string errMsg = RunUploadProcessHelper.DownloadFileFromFTP(runUpload, dictTaskResult, ref isCheckForFileDuplicityCheck, mailSubject, mailBody, ref fileName, true, false);
                foreach (KeyValuePair<string, Tuple<string, string>> taskPoint in dictTaskResult)
                {
                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint(taskPoint.Key, taskPoint.Value.Item1, taskPoint.Value.Item2);
                }
                if (!string.IsNullOrEmpty(errMsg))
                {
                    runUpload.RawFilePath = runUpload.RawFilePath + fileName;
                    isFileUploadedSuccessfully = false;
                    return isFileUploadedSuccessfully;
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
                        //taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", "Failure", null);
                        taskResult.Error = new Exception("File decryption error");
                        taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("Comments", "MissingFile", null);
                        UpdateTaskSpecificDataPoints(this, taskResult);
                    }
                    return isFileUploadedSuccessfully;
                }
                else
                {
                    if (taskResult != null)
                    {
                        taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("RetrievalStatus", "Success", null);
                    }
                }
                #endregion

                #region 4. Check Duplicity of file section
                dictTaskResult.Clear();
                errMsg = RunUploadProcessHelper.CheckAndAppyDuplicityValidation(runUpload, isPromptForDuplicityCheck, dictTaskResult, isCheckForFileDuplicityCheck, mailSubject, mailBody, fileName, false);
                foreach (KeyValuePair<string, Tuple<string, string>> taskPoint in dictTaskResult)
                {
                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint(taskPoint.Key, taskPoint.Value.Item1, taskPoint.Value.Item2);
                }
                if (!string.IsNullOrEmpty(errMsg))
                {
                    isFileUploadedSuccessfully = false;
                    return isFileUploadedSuccessfully;
                }
                #endregion
                taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("FileName", Path.GetFileName(runUpload.RawFilePath), null);
                AddRunUploadDataToTaskResult(taskResult, runUpload);
                //UpdateTaskSpecificDataPoints(this, taskResult);
                isFileUploadedSuccessfully = true;
            }
            catch (Exception ex)
            {
                taskResult.Error = ex;
                isFileUploadedSuccessfully = false;
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isFileUploadedSuccessfully;
        }

        #region IJob Members

        public StringBuilder Execute(JobExecutionContext context)
        {
            StringBuilder message = new StringBuilder();
            try
            {
                ExecutionInfo eInfo = DeepCopyHelper.Clone(TaskExecutionCache.Instance.GetExecutionInfo(context.JobDetail.Name.ToString()));
                if (eInfo != null)
                {
                    ImportManager.SetSchedulerRunningValue(true);
                    //To specify the import process is executed by Scheduler
                    message = ImportExecutionHelper.ExecuteImportBatch(eInfo.ExecutionName);
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
            return message;
        }

        internal static void SetSchedulerRunningValue(bool isSchedulerRunning)
        {
            //To specify the import process is executed by Scheduler                  
            _isSchedulerRunning = isSchedulerRunning;
        }

        #endregion

        /// <summary>
        /// Import and save data into application
        /// Partial data will be imported into the application on user action
        /// </summary>
        /// <param name="runUpload"></param>
        /// <param name="taskResult"></param>
        public bool RunImportIntoApplication(RunUpload runUpload, TaskResult taskResult, bool isSaveDataInApplication, DataSet dsPositionMaster)
        {
            try
            {
                //load data from existing file exist to avoid overwriting previous data.
                ProcessFormatedData(runUpload, taskResult, isSaveDataInApplication, dsPositionMaster);
            }
            catch (Exception ex)
            {
                taskResult.Error = ex;
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return true;
        }

        /// <summary>
        /// Sends control to respective handler and re run validation process
        /// </summary>
        /// <param name="runUpload"></param>
        /// <param name="taskResult"></param>
        public bool RunProceedToValidation(RunUpload runUpload, TaskResult taskResult)
        {
            try
            {
                //load data from existing file exist to avoid overwriting previous data.
                ProcessFormatedData(runUpload, taskResult, false, null);
            }
            catch (Exception ex)
            {
                taskResult.Error = ex;
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return true;
        }

        public void SetupLaunchForm(EventHandler LaunchForm)
        {
            _launchForm = LaunchForm;
        }

        public EventHandler GetLaunchForm()
        {
            return _launchForm;
        }

        /// <summary>
        /// In case when importDataSource is File
        /// TODO: Change method name to FillRunUploadDetails
        /// UploadDataThruLocalFile method should be called from method RunImportProcess to maintain uniformity
        /// </summary>
        /// <param name="uploadedFilePath"></param>
        /// <param name="runUpload"></param>
        /// <param name="taskResult"></param>
        /// <param name="isPromptForDuplicityCheck"></param>
        private void RunUpload(string uploadedFilePath, RunUpload runUpload, TaskResult taskResult, bool isPromptForDuplicityCheck)
        {
            try
            {
                object ftpFilePath = taskResult.TaskStatistics.TaskSpecificData.GetRefValueForKey("FtpFilePath");
                string runUploadFtpFilePath = string.Empty;
                if (!string.IsNullOrEmpty(runUpload.FtpFilePath))
                {
                    runUploadFtpFilePath = runUpload.FtpFilePath;
                }
                if (ftpFilePath != null)
                {
                    runUpload.FtpFilePath = ftpFilePath as string;
                }

                //We are setting ftp and decryption details to null to skip these processes as file is uploaded manually
                //we will check file for duplicity check
                //all the file that are set to null will be set back
                ThirdPartyFtp runUploadFTP = null;
                ThirdPartyGnuPG runUploadDecryptionDetails = null;
                string runUploadRawFilePath = string.Empty;
                if (!string.IsNullOrEmpty(uploadedFilePath) && File.Exists(uploadedFilePath))
                {
                    isPromptForDuplicityCheck = false;
                    runUploadFTP = runUpload.FtpDetails;
                    runUploadDecryptionDetails = runUpload.DecryptionDetails;
                    runUploadRawFilePath = runUpload.RawFilePath;

                    runUpload.FtpDetails = null;
                    runUpload.DecryptionDetails = null;
                    runUpload.RawFilePath = runUpload.FilePath + @"RawData\";

                    if (!Directory.Exists(runUpload.RawFilePath))
                    {
                        Directory.CreateDirectory(runUpload.RawFilePath);
                    }

                    runUpload.FtpFilePath = Path.GetFileName(uploadedFilePath);
                    //Copy file from given path to raw file path.
                    File.Copy(uploadedFilePath, runUpload.RawFilePath + runUpload.FtpFilePath, true);
                }
                bool isRunUploadSuccessfully = RunUploadProcess(runUpload, isPromptForDuplicityCheck, taskResult);

                //Add run upload data info to taskResult

                //We need to set here file name, as filename is used in key of runupload
                if (!string.IsNullOrEmpty(Path.GetFileName(runUpload.ProcessedFilePath)))
                {
                    runUpload.FileName = Path.GetFileName(runUpload.ProcessedFilePath);
                    runUpload.FilePath = runUpload.ProcessedFilePath;
                }
                if (!string.IsNullOrEmpty(runUploadFtpFilePath))
                {
                    runUpload.FtpFilePath = runUploadFtpFilePath;
                }
                if (!string.IsNullOrEmpty(runUploadRawFilePath))
                {
                    runUploadRawFilePath = runUpload.RawFilePath;
                }
                if (runUploadDecryptionDetails != null)
                {
                    runUpload.DecryptionDetails = runUploadDecryptionDetails;
                }
                if (runUploadFTP != null)
                {
                    runUpload.FtpDetails = runUploadFTP;
                }
                AddRunUploadDataToTaskResult(taskResult, runUpload);

                if (isRunUploadSuccessfully)
                {
                    UploadDataThruLocalFile(runUpload, taskResult, false);
                }
                else
                {
                    UpdateTaskSpecificDataPoints(this, taskResult);
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

        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            //Pricing service proxy used in handlers so ILiveFeedCallback should be implemented
        }

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        public void LiveFeedConnected()
        {
            //Pricing service proxy used in handlers so ILiveFeedCallback should be implemented
        }

        public void LiveFeedDisConnected()
        {
            //Pricing service proxy used in handlers so ILiveFeedCallback should be implemented
        }

        private void OnAutoStageImportCompleted(object sender, EventArgs<TaskResult> e)
        {
            try
            {
                if (ImportCompleted != null)
                    ImportCompleted(this, new EventArgs<TaskResult>(e.Value));
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _allocationServices.Dispose();
                _CashManagementServices.Dispose();
                _pricingServicesProxy.Dispose();
                _proxyAllocationServices.Dispose();

                if (StagedOrderHandler.Instance != null)
                {
                    StagedOrderHandler.Instance.GenerateNewAllocationScheme -= new EventHandler<EventArgs<DataSet, string,List<SecMasterBaseObj>>>(ImportManager.Instance.GenerateAllocationScheme);
                    StagedOrderHandler.Instance.ImportCompleted -= new EventHandler<EventArgs<TaskResult>>(OnAutoStageImportCompleted);
                }
            }
        }

        #endregion
    }
}