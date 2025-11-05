using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.LiveFeed;
using Prana.BusinessObjects.NewLiveFeed;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SMObjects;
using Prana.ClientCommon;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.TaskManagement.Definition.Definition;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prana.Tools
{
    /// <summary>
    /// User control for SM batch Setup
    /// </summary>
    public partial class ctrlSMBatchSetup : UserControl
    {
        /// <summary>
        /// Control to get the cron details 
        /// </summary>
        TaskSchedulerForm ctrlTaskScheduler;

        /// <summary>
        /// Control to generate new batch
        /// </summary>
        ctrlSMBatchCreator ctrl;

        /// <summary>
        /// value list holding the batch types
        /// </summary>
        //ValueList _vlType = new ValueList();

        /// <summary>
        /// Valuelist for run time types
        /// </summary>
        ValueList _vlRunTimeType = new ValueList();

        /// <summary>
        /// Form to hold the control for creating batches
        /// </summary>
        Form frmBatch;

        ConcurrentDictionary<string, HashSet<int>> _requestsBatchId = new ConcurrentDictionary<string, HashSet<int>>();

        /// <summary>
        /// To get requested batch fields
        /// </summary>
        ConcurrentDictionary<string, string> _requestedBatchFields = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 
        /// </summary>
        ConcurrentDictionary<string, string> _smBatchStatusBackup = new ConcurrentDictionary<string, string>();

        static int _userID = int.MinValue;
        static string _smBatchFilePath = string.Empty;
        static string _smBatchLayoutDirectoryPath = string.Empty;

        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public ctrlSMBatchSetup()
        {
            try
            {
                InitializeComponent();
                _requestsBatchId = SMBatchDataCache.GetInstance().RequestsBatchId;
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

        PranaBinaryFormatter binaryFormatter = new PranaBinaryFormatter();
        public delegate void PriceRespUIInvokeDelegate(DataTable dt);

        GridCellColorizer _grdColorizer;
        /// <summary>
        /// Initialize the batch data
        /// 
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        public void InitializeData(bool isSMBatchSaved = false)
        {
            try
            {
                // modified by Bhavana on July 2, 2014 
                // Purpose : to display account names in grid.

                BindingList<SMBatch> smBatchList = new BindingList<SMBatch>();
                smBatchList = SMBatchManager.GetSMBatchData();
                Dictionary<string, int> smBatchStatus = SMBatchManager.GetSMBatchStatusfromWorkFlow((int)NirvanaWorkFlows.SMBatch);

                if (isSMBatchSaved)
                {
                    SetPreviousSMBatchStatus();
                }
                grdSMBatch.DataSource = smBatchList;
                if (_grdColorizer == null)
                    _grdColorizer = new GridCellColorizer(grdSMBatch);
                if (grdSMBatch.Rows.Count > 0)
                {
                    foreach (UltraGridRow gridRow in grdSMBatch.Rows)
                    {
                        if (gridRow.Band.Columns.Exists("FundIDs") && gridRow.Band.Columns.Exists("FundName"))
                        {
                            string[] accounts = gridRow.Cells["FundIDs"].Value.ToString().Split(',');
                            StringBuilder accountName = new StringBuilder();
                            foreach (string accountID in accounts)
                            {
                                if (!string.IsNullOrEmpty(accountID))
                                {
                                    if (accountName.Length > 0)
                                    {
                                        accountName.Append(", ");
                                    }
                                    accountName.Append(CachedDataManager.GetInstance.GetAccountText(Convert.ToInt32(accountID)));
                                }
                            }
                            gridRow.Cells["FundName"].Value = accountName.ToString();
                            gridRow.Cells["FundName"].Activation = Activation.NoEdit;
                        }

                        if (gridRow.Band.Columns.Exists("CronExpression") && gridRow.Band.Columns.Exists("RunSchedule") && gridRow.Band.Columns.Exists("RunTimeDesc"))
                        {
                            gridRow.Cells["RunSchedule"].Value = SMBatchManager.GetRunSchedule(gridRow.Cells["CronExpression"].Value.ToString());
                            gridRow.Cells["RunTimeDesc"].Value = SMBatchManager.GetRunTime(gridRow.Cells["CronExpression"].Value.ToString());
                        }

                        if (gridRow.Band.Columns.Exists("Select"))
                        {
                            gridRow.Cells["Select"].Value = false;
                        }

                        // Purpose : to show SM batch status 
                        if (gridRow.Band.Columns.Exists("SystemLevelName") && gridRow.Band.Columns.Exists("Status"))
                        {
                            NirvanaTaskStatus batchStatus = NirvanaTaskStatus.Pending;
                            if (!isSMBatchSaved && smBatchStatus.ContainsKey(gridRow.Cells["SystemLevelName"].Value.ToString()))
                            {
                                batchStatus = (NirvanaTaskStatus)Enum.ToObject(typeof(NirvanaTaskStatus), smBatchStatus[gridRow.Cells["SystemLevelName"].Value.ToString()]);
                            }
                            else if (_smBatchStatusBackup.ContainsKey(gridRow.Cells["SystemLevelName"].Value.ToString()))
                            {
                                batchStatus = (NirvanaTaskStatus)Enum.ToObject(typeof(NirvanaTaskStatus), _smBatchStatusBackup[gridRow.Cells["SystemLevelName"].Value.ToString()]);
                            }
                            gridRow.Cells["Status"].Value = batchStatus.ToString();
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
        /// get SMBatchStatus from Work flow data 
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        private void SetPreviousSMBatchStatus()
        {
            try
            {
                _smBatchStatusBackup.Clear();
                foreach (UltraGridRow gridRow in grdSMBatch.Rows)
                {
                    if (gridRow.Band.Columns.Exists("SystemLevelName") && gridRow.Band.Columns.Exists("Status"))
                    {
                        string batchName = gridRow.Cells["SystemLevelName"].Value.ToString();
                        string batchStatus = gridRow.Cells["Status"].Value.ToString();
                        _smBatchStatusBackup.TryAdd(batchName, batchStatus);
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
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="qMsg"></param>
        /// <param name="requestId"></param>
        void SecurityMaster_SMGenericPriceResponse(QueueMessage qMsg, Guid requestId)
        {
            try
            {
                List<object> listDatareturned = binaryFormatter.DeSerializeParams(qMsg.Message.ToString());
                DataTable priceData = listDatareturned[0] as DataTable;
                bool pricingSuccess = (bool)listDatareturned[1];
                //string comment = listDatareturned[2] as string;

                #region To update batch success
                HashSet<int> batchIDs = UpdateBatchStatus(requestId, priceData, pricingSuccess);
                #endregion

                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        PriceRespUIInvokeDelegate priceResp = new PriceRespUIInvokeDelegate(GetInstance_PriceResponseEventHandler);
                        this.BeginInvoke(priceResp, new object[] { priceData });
                    }
                    #region Update Status On Grid
                    UpdateStatusOnGrid(pricingSuccess, batchIDs);
                    #endregion
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

        private void UpdateStatusOnGrid(bool pricingSuccess, HashSet<int> batchIDs)
        {
            if (grdSMBatch != null && grdSMBatch.Rows.Count > 0 && batchIDs.Count > 0)
            {
                foreach (UltraGridRow gridRow in grdSMBatch.Rows)
                {
                    if (gridRow.Band.Columns.Exists("SMBatchID") && gridRow.Band.Columns.Exists("Status") && gridRow.Band.Columns.Exists("SystemLevelName"))
                    {
                        foreach (int ID in batchIDs)
                        {
                            if (gridRow.Cells["SMBatchID"].Value.ToString().Equals(ID.ToString()))
                            {
                                if (pricingSuccess)
                                {
                                    Dictionary<string, int> smBatchStatus = SMBatchManager.GetSMBatchStatusfromWorkFlow((int)NirvanaWorkFlows.SMBatch);
                                    if (smBatchStatus.ContainsKey(gridRow.Cells["SystemLevelName"].Value.ToString()))
                                    {
                                        NirvanaTaskStatus batchStatus = (NirvanaTaskStatus)Enum.ToObject(typeof(NirvanaTaskStatus), smBatchStatus[gridRow.Cells["SystemLevelName"].Value.ToString()]);
                                        gridRow.Cells["Status"].Value = batchStatus.ToString();
                                    }
                                }
                                else
                                {
                                    //gridRow.Cells["Status"].Value = "Failure";
                                }
                            }
                        }
                    }
                }
            }
        }

        private HashSet<int> UpdateBatchStatus(Guid requestId, DataTable priceData, bool pricingSuccess)
        {
            HashSet<int> batchIDs = new HashSet<int>();
            try
            {
                ConcurrentDictionary<int, string> dictSMBatch = SMBatchManager.GetSMBatchList();
                if (_requestsBatchId.ContainsKey(requestId.ToString()))
                {
                    batchIDs = _requestsBatchId[requestId.ToString()];
                }

                foreach (int batchID in batchIDs)
                {
                    if (pricingSuccess && dictSMBatch.ContainsKey(batchID))
                    {
                        string smBatchName = dictSMBatch[batchID].ToString();

                        #region Save batch status through workFlowHandler
                        SaveBatchStatusThroughWorkFlowHandler(smBatchName);
                        #endregion

                        #region SMBatch response log
                        LogSMBatchResponse(priceData, smBatchName);
                        #endregion
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
            return batchIDs;
        }

        private void LogSMBatchResponse(DataTable priceData, string smBatchName)
        {
            try
            {
                int uniqueID = 0;
                priceData.TableName = "SMBatch";

                if (priceData != null)
                {
                    string SMBatchDirectoryPath = Application.StartupPath + @"\DashBoardData\SMBatch";
                    if (!Directory.Exists(SMBatchDirectoryPath))
                    {
                        Directory.CreateDirectory(SMBatchDirectoryPath);
                    }
                    string currentDate = DateTime.Today.ToString("dd-MM-yyyy");
                    var directory = new DirectoryInfo(SMBatchDirectoryPath);
                    if (directory.GetFiles().Length > 0)
                    {
                        var latestFileName = (from f in directory.GetFiles()
                                              where f.Name.Contains(EscapedDelimiter.CombineStrings('_', '^', smBatchName, currentDate))
                                              && EscapedDelimiter.SplitDelimitedString(f.Name, '_', '^')[0].Equals(smBatchName)
                                              orderby f.LastWriteTime descending
                                              select f).FirstOrDefault();
                        // To generate incremented id if there exists xml file of current date and same batch
                        if (latestFileName != null)
                        {
                            if (latestFileName.ToString().Contains(EscapedDelimiter.CombineStrings('_', '^', smBatchName, currentDate)))
                            {
                                int posStart = latestFileName.ToString().LastIndexOf('_') + 1;
                                int posEnd = latestFileName.ToString().IndexOf('.') + 1;
                                Int32.TryParse(latestFileName.ToString().Substring(posStart, posEnd - posStart - 1), out uniqueID);
                            }
                        }
                    }
                    string SMBatchFilePath = SMBatchDirectoryPath + @"\" + EscapedDelimiter.CombineStrings('_', '^', smBatchName, currentDate, ++uniqueID + ".xml");
                    CalcMidPrice(priceData, smBatchName);
                    priceData.WriteXml(SMBatchFilePath, XmlWriteMode.WriteSchema);
                    //priceData.WriteXml(SMBatchFilePath);
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
        /// To calculate mid price using average (ask, bid)
        /// </summary>
        /// <param name="priceData"></param>
        /// <param name="smBatchName"></param>
        private void CalcMidPrice(DataTable priceData, string smBatchName)
        {
            if (_requestedBatchFields.ContainsKey(smBatchName) && _requestedBatchFields[smBatchName].Contains(PricingDataType.Avg_AskBid.ToString()))
            {
                IEnumerable<DataColumn> requiredColumns = priceData.Columns.Cast<DataColumn>().Where(x => x.ColumnName.Equals(PricingDataType.Ask.ToString()) || x.ColumnName.Equals(PricingDataType.Bid.ToString()));
                DataColumn colMid = new DataColumn(PricingDataType.Avg_AskBid.ToString(), Type.GetType("System.String", true, false));
                priceData.Columns.Add(colMid);
                DataColumn colComments = new DataColumn("Comments", Type.GetType("System.String", true, false));
                priceData.Columns.Add(colComments);

                foreach (DataRow dataRow in priceData.Rows)
                {
                    double price, avgAsk_Bid = 0;
                    bool isDataInsufficient = false;
                    foreach (DataColumn col in requiredColumns)
                    {
                        if (dataRow[col.ColumnName] != null && dataRow[col.ColumnName] != DBNull.Value && !String.IsNullOrWhiteSpace(dataRow[col.ColumnName].ToString()))
                        {
                            if (double.TryParse(dataRow[col.ColumnName].ToString(), out price))
                                avgAsk_Bid += price;
                        }
                        else
                        {
                            dataRow["Comments"] = "Insufficient pricing data";
                            isDataInsufficient = true;
                            break;
                        }
                    }
                    if (!isDataInsufficient)
                    {
                        dataRow[colMid.ColumnName] = avgAsk_Bid / 2;
                    }
                }
            }
        }

        private static void SaveBatchStatusThroughWorkFlowHandler(string smBatchName)
        {
            try
            {
                WorkflowItem workflowItem = new WorkflowItem();
                workflowItem.ContextID = 3;
                workflowItem.ContextValue = smBatchName;
                workflowItem.EventID = (int)NirvanaWorkFlows.SMBatch;
                workflowItem.EventRunTime = DateTime.Now;
                workflowItem.FileExecutionDate = DateTime.Now;
                workflowItem.StatusID = (int)NirvanaTaskStatus.Success;

                WorkflowHandler.Instance.PublishWorkflow(workflowItem);
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

        private void GetInstance_PriceResponseEventHandler(DataTable dt)
        {
            try
            {
                if (dt.Columns.Contains("Beta"))
                {
                    ClientPricingManager.GetInstance.SaveDailyValuationData(SMBatchType.Beta, dt);

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
        /// Initialize the value list
        /// </summary>
        //private void InitializeValueLists()
        //{
        //    try
        //    {
        //        _vlType.ValueListItems.Clear();
        //        //_vlRunType.ValueListItems.Clear();
        //        //_vlRunTimeType.ValueListItems.Clear();

        //        Dictionary<int, string> dictBatchTypes = SMBatchManager.GetSMBatchTypes();
        //        foreach (int typeId in dictBatchTypes.Keys)
        //        {
        //            _vlType.ValueListItems.Add(typeId, dictBatchTypes[typeId]);
        //        }

        //        //Dictionary<int, string> dictRunTimeType = SMBatchManager.GetRunTimeTypes();
        //        //foreach (int key in dictRunTimeType.Keys)
        //        //{
        //        //    _vlRunTimeType.ValueListItems.Add(key,dictRunTimeType[key]);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //{
        //            throw;
        //}
        //}
        //    //Dictionary<int, string> dictBatchRunTypes = SMBatchManager.GetSMBatchRunTypes();
        //    //foreach (int runTypeId in dictBatchTypes.Keys)
        //    //{
        //    //    _vlType.ValueListItems.Add(runTypeId, dictBatchTypes[runTypeId]);
        //    //}
        //}

        /// <summary>
        /// initialize the layout of the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSMBatch_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                grdSMBatch.DisplayLayout.UseFixedHeaders = true;

                if (grdSMBatch.DataSource != null)
                {
                    UltraGridBand band = grdSMBatch.DisplayLayout.Bands[0];

                    foreach (UltraGridColumn column in band.Columns)
                    {
                        //following line auto adjust width of columns of ultragrid accocrding to text size of header.
                        column.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All);
                    }
                    foreach (UltraGridColumn col in band.Columns)
                    {
                        col.CellActivation = Activation.NoEdit;
                    }
                    if (!band.Columns.Exists("Select"))
                    {
                        band.Columns.Add("Select", "Select");
                    }

                    UltraGridColumn colBtnSelect = band.Columns["Select"];
                    colBtnSelect.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    colBtnSelect.Width = 50;
                    colBtnSelect.Header.Caption = "";
                    colBtnSelect.Header.Fixed = true;
                    colBtnSelect.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                    colBtnSelect.Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                    colBtnSelect.Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center;
                    colBtnSelect.Header.VisiblePosition = 0;
                    colBtnSelect.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colBtnSelect.SortIndicator = SortIndicator.None;
                    colBtnSelect.CellActivation = Activation.AllowEdit;

                    if (band.Columns.Exists("SMBatchID"))
                    {
                        band.Columns["SMBatchID"].Hidden = true;
                        band.Columns["SMBatchID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    }
                    if (band.Columns.Exists("FundIDs"))
                    {
                        band.Columns["FundIDs"].Hidden = true;
                        band.Columns["FundIDs"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    }
                    if (band.Columns.Exists("SystemLevelName"))
                    {
                        UltraGridColumn colBatchName = band.Columns["SystemLevelName"];
                        colBatchName.Header.Caption = "System Level Name";
                        colBatchName.Header.VisiblePosition = 1;
                    }

                    if (band.Columns.Exists("UserDefinedName"))
                    {
                        UltraGridColumn colBatchName = band.Columns["UserDefinedName"];
                        colBatchName.Header.Caption = "User Defined Name";
                        colBatchName.Header.VisiblePosition = 2;
                    }

                    if (!band.Columns.Exists("FundName"))
                    {
                        band.Columns.Add("FundName", "FundName");
                    }

                    UltraGridColumn colAccountName = band.Columns["AccountName"];
                    colAccountName.Header.Caption = "Accounts";
                    colAccountName.Header.VisiblePosition = 3;

                    if (band.Columns.Exists("CronExpression"))
                    {
                        band.Columns["CronExpression"].Hidden = true;
                        band.Columns["CronExpression"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    }
                    //if (band.Columns.Exists("SMBatchType"))
                    //{
                    //    UltraGridColumn colBatchType = band.Columns["SMBatchType"];
                    //    colBatchType.Header.Caption = "Batch Type";
                    //    colBatchType.ValueList = _vlType;
                    //    colBatchType.Header.VisiblePosition = 4;
                    //}
                    if (band.Columns.Exists("RunTime"))
                    {
                        UltraGridColumn colDate = band.Columns["RunTime"];
                        colDate.Header.Caption = "Run Time";
                        //colDate.ValueList = _vlRunTimeType;
                        colDate.Header.VisiblePosition = 5;
                        //colDate.MaskInput = "{LOC}mm/dd/yyyy hh:mm:ss";
                        //colDate.Format = "yyyy-MM-dd HH:mm:ss";
                        colDate.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                        colDate.Hidden = true;
                    }
                    if (!band.Columns.Exists("RunSchedule"))
                    {
                        band.Columns.Add("RunSchedule", "RunSchedule");
                        UltraGridColumn colSchedule = band.Columns["RunSchedule"];
                        colSchedule.Header.Caption = "Schedule";
                        colSchedule.CellActivation = Activation.NoEdit;
                        colSchedule.DataType = typeof(string);
                        colSchedule.Header.VisiblePosition = 6;
                    }

                    if (!band.Columns.Exists("RunTimeDesc"))
                    {
                        band.Columns.Add("RunTimeDesc", "RunTimeDesc");
                        UltraGridColumn colRunTime = band.Columns["RunTimeDesc"];
                        colRunTime.Header.Caption = "Run Time";
                        colRunTime.CellActivation = Activation.NoEdit;
                        colRunTime.DataType = typeof(string);
                        colRunTime.Header.VisiblePosition = 7;
                    }

                    if (band.Columns.Exists("IsHistoricDataRequired"))
                    {
                        UltraGridColumn colHistDataRequired = band.Columns["IsHistoricDataRequired"];
                        colHistDataRequired.Header.Caption = "Historic Data Required";
                        colHistDataRequired.CellActivation = Activation.NoEdit;
                        colHistDataRequired.Header.VisiblePosition = 8;
                    }
                    if (band.Columns.Exists("DaysOfHistoricData"))
                    {
                        UltraGridColumn colHistDays = band.Columns["DaysOfHistoricData"];
                        colHistDays.Header.Caption = "# Days Historical Data Required";
                        colHistDays.CellActivation = Activation.NoEdit;
                        colHistDays.Header.VisiblePosition = 9;
                    }
                    if (band.Columns.Exists("BatType"))
                    {
                        UltraGridColumn colHistDays = band.Columns["BatType"];
                        colHistDays.Header.Caption = "Batch Type";
                        colHistDays.CellActivation = Activation.NoEdit;
                        colHistDays.Header.VisiblePosition = 10;
                    }
                    //if (band.Columns.Exists("StartDate"))
                    //{
                    //    UltraGridColumn colDate = band.Columns["StartDate"];
                    //    colDate.Header.Caption = "Date";
                    //    colDate.MaskInput = "{LOC}mm/dd/yyyy hh:mm:ss";
                    //    colDate.Format = "yyyy-MM-dd HH:mm:ss";
                    //    colDate.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    //    colDate.Header.VisiblePosition = 4;
                    //}
                    if (!band.Columns.Exists("Status"))
                    {
                        UltraGridColumn colStatus = band.Columns.Add("Status");
                        colStatus.Header.Caption = "Status";
                        colStatus.CellActivation = Activation.NoEdit;
                        colStatus.Header.VisiblePosition = 11;
                    }
                    if (!band.Columns.Exists("Report"))
                    {
                        UltraGridColumn colReport = band.Columns.Add("Report");
                        colReport.Header.Caption = "Report";
                        colReport.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.URL;
                        colReport.NullText = "View";
                        colReport.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                        colReport.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                        colReport.Hidden = true;
                    }
                    //if (band.Columns.Exists("RunSchedule"))
                    //{
                    //    UltraGridColumn colReport = band.Columns.Add("Report");
                    //    colReport.Header.Caption = "Report";
                    //    colReport.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.URL;
                    //    colReport.NullText = "View";
                    //    colReport.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    //}
                    if (!band.Columns.Exists("Rerun"))
                    {
                        UltraGridColumn colRerun = band.Columns.Add("Rerun");
                        colRerun.Header.Caption = "Rerun Batch";
                        colRerun.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                        colRerun.NullText = "Rerun";
                        colRerun.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                        colRerun.Header.VisiblePosition = 12;
                    }
                    if (!band.Columns.Exists("ManualUpload"))
                    {
                        UltraGridColumn colManualUpload = band.Columns.Add("ManualUpload");
                        colManualUpload.Header.Caption = "Manual Upload";
                        colManualUpload.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                        colManualUpload.NullText = "Manual Upload";
                        colManualUpload.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                        colManualUpload.Header.VisiblePosition = 13;
                        colManualUpload.Hidden = true;
                        colManualUpload.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    }
                    if (!band.Columns.Exists("EditBatch"))
                    {
                        UltraGridColumn colEditSchedule = band.Columns.Add("EditBatch");
                        colEditSchedule.Header.Caption = "Edit Schedule";
                        colEditSchedule.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                        colEditSchedule.NullText = "Edit Schedule";
                        colEditSchedule.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                        //colEditSchedule.Header.VisiblePosition = 6;
                        colEditSchedule.Hidden = true;
                        colEditSchedule.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    }


                    //if (band.Columns.Exists("Select"))
                    //{
                    //    UltraGridColumn colBtnSelect = band.Columns["Select"];
                    //    colBtnSelect.CellActivation = Activation.AllowEdit;
                    //}
                    if (band.Columns.Exists("FilterClause"))
                    {
                        band.Columns["FilterClause"].Hidden = true;
                    }

                    // load the saveout file if it exists
                    LoadReportSaveLayoutXML();
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
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                frmBatch = new Form();
                ctrl = new ctrlSMBatchCreator((SMBatch.BatchType)rbSetBatchType.Value);
                ctrl.InitializeControl();
                frmBatch.Controls.Add(ctrl);
                ctrl.Dock = DockStyle.Fill;
                frmBatch.ShowIcon = false;
                frmBatch.Text = "Create Batch";
                frmBatch.Size = new System.Drawing.Size(480, 430);
                frmBatch.MinimizeBox = frmBatch.MaximizeBox = false;
                frmBatch.StartPosition = FormStartPosition.CenterParent;
                frmBatch.MaximumSize = frmBatch.MinimumSize = new System.Drawing.Size(480, 430);
                CustomThemeHelper.AddUltraFormManagerToDynamicForm(frmBatch);
                frmBatch.Load += new System.EventHandler(frmBatch_Load);
                frmBatch.ShowInTaskbar = false;
                frmBatch.ShowDialog(this.Parent);
                InitializeData();
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

        private void frmBatch_Load(object sender, EventArgs e)
        {
            try
            {

                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
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
        /// initalize the control on load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrlSMBatchSetup_Load(object sender, EventArgs e)
        {
            try
            {
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    InitializeData();
                    if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                    {
                        SetButtonsColor();
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

        private void SetButtonsColor()
        {
            try
            {
                btnDelete.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnDelete.ForeColor = System.Drawing.Color.White;
                btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnDelete.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnDelete.UseAppStyling = false;
                btnDelete.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnEdit.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnEdit.ForeColor = System.Drawing.Color.White;
                btnEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnEdit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnEdit.UseAppStyling = false;
                btnEdit.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnRun.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRun.ForeColor = System.Drawing.Color.White;
                btnRun.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRun.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRun.UseAppStyling = false;
                btnRun.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnCreate.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnCreate.ForeColor = System.Drawing.Color.White;
                btnCreate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCreate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCreate.UseAppStyling = false;
                btnCreate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                string flag = "NoRows";
                foreach (UltraGridRow gridRow in grdSMBatch.Rows)
                {
                    if (gridRow.Cells["Select"].Value.ToString() != "True")
                    {
                        continue;
                    }
                    if (gridRow.Band.Columns.Exists("Status"))
                    {
                        if (gridRow.Cells["Status"].Value != null)
                        {
                            if (gridRow.Cells["Status"].Value.ToString().Equals("Running.."))
                            {
                                MessageBox.Show("Can not delete since batch is in running state");
                                return;
                            }
                            else
                            {
                                int smBatchID = Convert.ToInt32(gridRow.Cells["SMBatchID"].Value);
                                i = SMBatchManager.DeleteSMBatch(smBatchID);
                            }
                        }
                        else
                        {
                            int smBatchID = Convert.ToInt32(gridRow.Cells["SMBatchID"].Value);
                            i = SMBatchManager.DeleteSMBatch(smBatchID);
                        }
                    }
                    else
                    {
                        int smBatchID = Convert.ToInt32(gridRow.Cells["SMBatchID"].Value);
                        i = SMBatchManager.DeleteSMBatch(smBatchID);
                    }
                    if (i > 0)
                    {
                        if (flag == "NoRows")
                            flag = "Success";
                    }
                    else
                    {
                        flag = "Fail";
                    }
                }
                switch (flag)
                {
                    case "NoRows":
                        MessageBox.Show("Please select a row to delete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case "Success":
                        MessageBox.Show("Batch deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case "Fail":
                        MessageBox.Show("Some batches could not be deleted", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
                InitializeData();
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
        /// check whether the grid has some invalid data
        /// </summary>
        /// <returns>true if invalid data, false otherwise</returns>
        //private bool HasEmpty()
        //{
        //    try
        //    {
        //        int rowCount = grdSMBatch.Rows.Count - 1;
        //        for (int i = rowCount; i >= 0; i--)
        //        {
        //            UltraGridRow row = grdSMBatch.Rows[i];
        //            if (string.IsNullOrWhiteSpace(row.Cells["SMBatchID"].Text)
        //                && string.IsNullOrWhiteSpace(row.Cells["FormatName"].Text)
        //                && string.IsNullOrWhiteSpace(row.Cells["CronExpression"].Text)
        //                && string.IsNullOrWhiteSpace(row.Cells["SMBatchType"].Text)
        //                && string.IsNullOrWhiteSpace(row.Cells["StartDate"].Text))
        //            {
        //                row.Delete(false);
        //                continue;
        //            }
        //            if (string.IsNullOrWhiteSpace(row.Cells["FormatName"].Text) || string.IsNullOrWhiteSpace(row.Cells["CronExpression"].Text)
        //                || string.IsNullOrWhiteSpace(row.Cells["SMBatchType"].Text) || string.IsNullOrWhiteSpace(row.Cells["StartDate"].Text))
        //            {
        //                return true;
        //            }
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
        //    return false;
        //}

        /// <summary>
        /// Set the flag variable to indicate thatg save would be required now
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSMBatch_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key == "Select")
                {
                    this.grdSMBatch.PerformAction(UltraGridAction.ExitEditMode);
                }
                //_isSaveRequired = true;
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
        /// handle the cell button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSMBatch_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {

                if (e.Cell.Column.Key == "EditBatch")
                {
                    //TaskSchedulerForm taskForm = new TaskSchedulerForm();
                    ctrlTaskScheduler = new TaskSchedulerForm();
                    if (grdSMBatch.ActiveRow.Cells["CronExpression"].Value != DBNull.Value)
                    {
                        string cronExp = Convert.ToString(grdSMBatch.ActiveRow.Cells["CronExpression"].Value);
                        ctrlTaskScheduler.GetCronToFill(cronExp);
                    }
                    ctrlTaskScheduler.ShowDialog(this.Parent);
                    DialogResult dr = ctrlTaskScheduler.DialogResult;
                    if (dr == DialogResult.OK)
                    {
                        string cronExp = ctrlTaskScheduler.GetCronExpression();
                        if (!string.IsNullOrEmpty(cronExp))
                        {
                            grdSMBatch.ActiveRow.Cells["CronExpression"].Value = cronExp;
                            grdSMBatch.ActiveRow.Cells["RunSchedule"].Value = SMBatchManager.GetRunSchedule(cronExp);
                            grdSMBatch.ActiveRow.Cells["RunTimeDesc"].Value = SMBatchManager.GetRunTime(cronExp);
                        }
                        //int batchID = int.Parse(grdSMBatch.ActiveRow.Cells["BatchID"].Value.ToString());
                        //foreach (DataRow row in ((DataTable)grdSMBatch.DataSource).Rows)
                        //{
                        //    if (batchID == int.Parse(row["BatchID"].ToString()))
                        //    {
                        //        SMBatchManager.FillCronDetails(cronExp, dr);
                        //    }
                        //}
                        //CronDescription cronDetail=CronUtility.GetCronDescriptionObject(cronExp);
                        //int schedule = (int)cronDetail.Type;
                        //DateTime startTime = Convert.ToDateTime(cronDetail.StartDate.ToString() + cronDetail.StartTime.ToString());
                        //DateTime nxtExecTime=startTime
                    }
                }

                if (e.Cell.Column.Key == "Rerun")
                {

                    DialogResult result = MessageBox.Show("On rerun batch data will be overwritten for existing symbols.\nDo you want to continue ?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result.Equals(DialogResult.No))
                    {
                        return;
                    }
                    else if (result.Equals(DialogResult.Yes))
                    {
                        bool isGetDataFromCacheOrDB = false;

                        if (e.Cell.Row.Cells.Exists("Status") && e.Cell.Row.Cells["Status"].Text.Equals("Running.."))
                        {
                            MessageBox.Show("Batch is running, wait for the completion of batch", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else
                        {
                            ConcurrentDictionary<string, List<int>> requestsAccount = new ConcurrentDictionary<string, List<int>>();
                            ConcurrentDictionary<string, PricingRequestMappings> requestsMapping = new ConcurrentDictionary<string, PricingRequestMappings>();

                            RunSMBatch(e.Cell.Row, requestsAccount, requestsMapping, isGetDataFromCacheOrDB);

                            foreach (KeyValuePair<string, PricingRequestMappings> kvp in requestsMapping)
                            {
                                if (SecurityMaster != null && SecurityMaster.IsConnected)
                                {
                                    String SecondaryPricingSource = kvp.Value.SecondaryPricingSource;
                                    if (SecondaryPricingSource.Equals("BGN", StringComparison.OrdinalIgnoreCase)) //TODO- handle BGN- omshiv
                                    {
                                        SecondaryPricingSource = String.Empty;
                                    }
                                    kvp.Value.RequestObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                                    kvp.Value.RequestObj.HashCode = this.GetHashCode();
                                    SecurityMaster.GetMarkPricesForSymbolAndDate(SecondaryPricingSource, kvp.Value.FieldNames.ToList(), kvp.Value.RequestObj, kvp.Value.StartDate, kvp.Value.EndDate, Guid.Parse(kvp.Value.RequestID), SecurityMaster_SMGenericPriceResponse, isGetDataFromCacheOrDB);
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #region Commented
        //public static bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        //{
        //    var cnt = new Dictionary<T, int>();
        //    try
        //    {
        //        foreach (T s in list1)
        //        {
        //            if (cnt.ContainsKey(s))
        //            {
        //                cnt[s]++;
        //            }
        //            else
        //            {
        //                cnt.Add(s, 1);
        //            }
        //        }
        //        foreach (T s in list2)
        //        {
        //            if (cnt.ContainsKey(s))
        //            {
        //                cnt[s]--;
        //            }
        //            else
        //            {
        //                return false;
        //            }
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
        //    return cnt.Values.All(c => c == 0);
        //}
        #endregion
        /// <summary>
        /// run batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                bool flag = false;
                bool isGetDataFromCacheOrDB = true;

                ConcurrentDictionary<string, List<int>> requestsAccount = new ConcurrentDictionary<string, List<int>>();
                //ConcurrentDictionary<string, HashSet<int>> requestsBatchId = new ConcurrentDictionary<string, HashSet<int>>();
                ConcurrentDictionary<string, PricingRequestMappings> requestsMapping = new ConcurrentDictionary<string, PricingRequestMappings>();

                foreach (UltraGridRow gridRow in grdSMBatch.Rows)
                {
                    if (gridRow.Cells["Select"].Value.ToString() != "True")
                    {
                        continue;
                    }
                    else
                    {
                        flag = true;
                        RunSMBatch(gridRow, requestsAccount, requestsMapping, isGetDataFromCacheOrDB);

                        #region Save batch status through workFlowHandler

                        WorkflowItem workflowItem = new WorkflowItem();
                        workflowItem.ContextID = 3;
                        workflowItem.ContextValue = gridRow.Cells["SystemLevelName"].Value.ToString();
                        workflowItem.EventID = (int)NirvanaWorkFlows.SMBatch;
                        workflowItem.EventRunTime = DateTime.Now;
                        workflowItem.FileExecutionDate = DateTime.Now;
                        workflowItem.StatusID = (int)NirvanaTaskStatus.Pending;

                        WorkflowHandler.Instance.PublishWorkflow(workflowItem);
                        #endregion
                    }
                }

                foreach (KeyValuePair<string, PricingRequestMappings> kvp in requestsMapping)
                {
                    if (SecurityMaster != null && SecurityMaster.IsConnected)
                    {
                        String SecondaryPricingSource = kvp.Value.SecondaryPricingSource;
                        if (SecondaryPricingSource.Equals("BGN", StringComparison.OrdinalIgnoreCase)) //TODO- handle BGN- omshiv
                        {
                            SecondaryPricingSource = String.Empty;
                        }
                        kvp.Value.RequestObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                        kvp.Value.RequestObj.HashCode = this.GetHashCode();
                        SecurityMaster.GetMarkPricesForSymbolAndDate(SecondaryPricingSource, kvp.Value.FieldNames.ToList(), kvp.Value.RequestObj, kvp.Value.StartDate, kvp.Value.EndDate, Guid.Parse(kvp.Value.RequestID), SecurityMaster_SMGenericPriceResponse, isGetDataFromCacheOrDB);
                    }
                }
                if (!flag)
                {
                    MessageBox.Show("Please select a batch to run", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void RunSMBatch(UltraGridRow gridRow, ConcurrentDictionary<string, List<int>> requestsAccount, ConcurrentDictionary<string, PricingRequestMappings> requestsMapping, bool isGetDataFromCacheOrDB)
        {
            DateTime startDate = ultraDateTimeEditor1.DateTime.Date;
            DateTime endDate = ultraDateTimeEditor1.DateTime.Date;
            try
            {
                if (!gridRow.Band.Columns.Exists("FundIDs"))
                {
                    MessageBox.Show("Error in SMBatch. No AccountIds column present");
                    return;
                }
                if (gridRow.Cells["FundIDs"].Value == null)
                {
                    MessageBox.Show("Error in SMBatch Setup. Accounts not set for some batch");
                    return;
                }
                if (!gridRow.Band.Columns.Exists("SMBatchID"))
                {
                    MessageBox.Show("Error in SMBatch Setup. No SMBatchID present.");
                    return;
                }
                if (!gridRow.Band.Columns.Exists("Fields"))
                {
                    MessageBox.Show("Error in SMBatch Setup. Fields not set for some batch.");
                    return;
                }
                string[] accounts = gridRow.Cells["FundIDs"].Value.ToString().Split(',');
                List<int> accountIds = new List<int>();
                foreach (string account in accounts)
                {
                    int accountId;
                    if (int.TryParse(account, out accountId))
                        accountIds.Add(accountId);
                }


                string requestID;
                string[] fields = gridRow.Cells["Fields"].Value.ToString().Split(',');
                ConcurrentBag<string> fieldList = new ConcurrentBag<string>();
                foreach (string field in fields)
                {
                    if (!field.Trim().Equals(PricingDataType.Avg_AskBid.ToString()))
                    {
                        if (!fieldList.Contains(field.Trim()))
                        {
                            fieldList.Add(field.Trim());
                        }
                    }
                    else
                    {
                        if (!fieldList.Contains(PricingDataType.Ask.ToString()))
                        {
                            fieldList.Add(PricingDataType.Ask.ToString());
                        }
                        if (!fieldList.Contains(PricingDataType.Bid.ToString()))
                        {
                            fieldList.Add(PricingDataType.Bid.ToString());
                        }
                    }
                }

                Dictionary<String, SecMasterRequestObj> sourceWiseSMRequestDict = new Dictionary<string, SecMasterRequestObj>();

                requestID = Guid.NewGuid().ToString();
                requestsAccount.TryAdd(requestID, accountIds);
                _requestsBatchId.TryAdd(requestID, new HashSet<int>() { int.Parse(gridRow.Cells["SMBatchID"].Value.ToString()) });
                DataTable symbolsDT = new DataTable();

                DataSet ds = PositionDataManager.GetSymbolsForOpenPositionsAccountsAndDate(accountIds, startDate, int.Parse(gridRow.Cells["SMBatchID"].Value.ToString()));
                if (ds != null && ds.Tables.Count > 0)
                {
                    symbolsDT = ds.Tables[0];
                    string batchName = string.Empty;
                    if (gridRow.Band.Columns.Exists("SystemLevelName"))
                    {
                        batchName = gridRow.Cells["SystemLevelName"].Value.ToString();
                        _requestedBatchFields.TryAdd(batchName, gridRow.Cells["Fields"].Value.ToString());
                    }
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("No account exists for batch " + batchName);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Error while fetching data for open symbols. Please contact administrator");
                    return;
                }
                foreach (DataRow dr in symbolsDT.Rows)
                {
                    String SecondarySource = dr["SecondarySource"].ToString();
                    if (!string.IsNullOrWhiteSpace(SecondarySource) && sourceWiseSMRequestDict.ContainsKey(SecondarySource))
                    {
                        SecMasterRequestObj reqObj = sourceWiseSMRequestDict[SecondarySource];
                        getSMRequest(dr, reqObj);
                    }
                    else
                    {
                        // TODO : should be handled for blank SecondarySource
                        // currently sourceWiseSMRequestDict can be filled with blank key(sec. source)
                        if (sourceWiseSMRequestDict.ContainsKey(SecondarySource))
                        {
                            getSMRequest(dr, sourceWiseSMRequestDict[SecondarySource]);
                        }
                        else
                        {
                            SecMasterRequestObj reqObj = new SecMasterRequestObj();
                            sourceWiseSMRequestDict.Add(SecondarySource, reqObj);
                            getSMRequest(dr, reqObj);
                        }
                    }
                }
                if (gridRow.Cells["Indices"].Value != null && gridRow.Cells["Indices"].Value != DBNull.Value)
                {
                    DataSet dtSet = PositionDataManager.GetSymbolsForSymbolPK(gridRow.Cells["Indices"].Value.ToString());
                    if (dtSet != null && dtSet.Tables.Count > 0)
                        symbolsDT = dtSet.Tables[0];
                    else
                    {
                        MessageBox.Show("Error while fetching data for open symbols. Please contact administrator");
                        return;
                    }
                    foreach (DataRow dr in symbolsDT.Rows)
                    {
                        String SecondarySource = "";
                        if (!string.IsNullOrWhiteSpace(SecondarySource) && sourceWiseSMRequestDict.ContainsKey(SecondarySource))
                        {
                            SecMasterRequestObj reqObj = sourceWiseSMRequestDict[SecondarySource];
                            getSMRequest(dr, reqObj);
                        }
                        else
                        {
                            // TODO : should be handled for blank SecondarySource
                            // currently sourceWiseSMRequestDict can be filled with blank key(sec. source)
                            if (sourceWiseSMRequestDict.ContainsKey(SecondarySource))
                            {
                                getSMRequest(dr, sourceWiseSMRequestDict[SecondarySource]);
                            }
                            else
                            {
                                SecMasterRequestObj reqObj = new SecMasterRequestObj();
                                sourceWiseSMRequestDict.Add(SecondarySource, reqObj);
                                getSMRequest(dr, reqObj);
                            }
                        }
                    }
                }


                foreach (KeyValuePair<String, SecMasterRequestObj> smRequest in sourceWiseSMRequestDict)
                {
                    if (String.Compare(gridRow.Cells["IsHistoric"].Value.ToString(), "True", true) == 0 || String.Compare(gridRow.Cells["IsHistoric"].Value.ToString(), "0", true) == 0)
                    {
                        int noOfDays;
                        int.TryParse(gridRow.Cells["HistoricDaysRequired"].Value.ToString(), out noOfDays);
                        endDate.AddDays(noOfDays * -1);
                    }
                    requestsMapping.TryAdd(requestID, new PricingRequestMappings(requestID, smRequest.Key, fieldList, smRequest.Value, startDate, endDate, null, isGetDataFromCacheOrDB));
                }
                gridRow.Cells["Status"].Value = "Running..";

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

        private static void getSMRequest(DataRow dr, SecMasterRequestObj reqObj)
        {
            try
            {
                string symbol = dr["BloombergSymbol"].ToString();
                if (!string.IsNullOrEmpty(symbol))
                {
                    reqObj.AddData(symbol, ApplicationConstants.SymbologyCodes.BloombergSymbol);
                    return;
                }
                symbol = dr["CUSIPSymbol"].ToString();
                if (!string.IsNullOrEmpty(symbol))
                {
                    reqObj.AddData(symbol, ApplicationConstants.SymbologyCodes.CUSIPSymbol);
                    return;
                }
                symbol = dr["ISINSymbol"].ToString();
                if (!string.IsNullOrEmpty(symbol))
                {
                    reqObj.AddData(symbol, ApplicationConstants.SymbologyCodes.ISINSymbol);
                    return;
                }
                symbol = dr["SEDOLSymbol"].ToString();
                if (!string.IsNullOrEmpty(symbol))
                {
                    reqObj.AddData(symbol, ApplicationConstants.SymbologyCodes.SEDOLSymbol);
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

        //private void RunSMBatch(SMBatchType smBatchType, DateTime StartDate, DateTime EndDate)
        //{
        //    try
        //    {
        //        SecMasterRequestObj reqObj = new SecMasterRequestObj();
        //        ApplicationConstants.SymbologyCodes symbology = ApplicationConstants.SymbologyCodes.BloombergSymbol;

        //        DataTable dtDailybeta = new DataTable();

        //        dtDailybeta = MarkDataManager.GetBetaValueDateWise(StartDate, EndDate, 0);

        //        List<string> pricingFields = new List<string>();
        //        switch (smBatchType)
        //        {
        //            case SMBatchType.Beta:



        //                pricingFields.Add(smBatchType.ToString());

        //                foreach (DataRow dr in dtDailybeta.Rows)
        //                {
        //                    string symbol = dr["BloombergSymbol"].ToString();
        //                    if (!string.IsNullOrEmpty(symbol))
        //                    {

        //                        reqObj.AddData(symbol, symbology);

        //                    }
        //                }
        //                break;

        //            case SMBatchType.DailyVolume:


        //                dtDailybeta = MarkDataManager.GetBetaValueDateWise(StartDate, EndDate, 0);


        //                pricingFields.Add(smBatchType.ToString());

        //                foreach (DataRow dr in dtDailybeta.Rows)
        //                {
        //                    string symbol = dr["BloombergSymbol"].ToString();
        //                    if (!string.IsNullOrEmpty(symbol))
        //                    {

        //                        reqObj.AddData(symbol, symbology);

        //                    }
        //                }
        //                break;
        //        }

        //        if (SecurityMaster != null && SecurityMaster.IsConnected)
        //        {

        //            reqObj.HashCode = this.GetHashCode();
        //            SecurityMaster.GetMarkPricesForSymbolAndDate(pricingFields, reqObj, StartDate, EndDate, Guid.NewGuid(), SecurityMaster_SMGenericPriceResponse);
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
        //}

        public Interfaces.ISecurityMasterServices SecurityMaster { get; set; }


        /// <summary>
        /// To edit selected SMBatch details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                string flag = string.Empty;
                foreach (UltraGridRow gridRow in grdSMBatch.Rows)
                {
                    if (gridRow.Cells["Select"].Value.ToString() == "True")
                    {
                        i++;
                    }
                }
                if (i == 0)
                {
                    flag = "NoRows";
                }
                else if (i > 1)
                {
                    flag = "MultipleRows";
                }
                else
                {
                    flag = "SingleRow";
                }

                switch (flag)
                {
                    case "NoRows":
                        MessageBox.Show("Please select a row to edit", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case "MultipleRows":
                        MessageBox.Show("Please select a single row to edit", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case "SingleRow":
                        CheckStatusForEdit();
                        break;
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
        /// Check batch status before edit
        /// </summary>
        private void CheckStatusForEdit()
        {
            try
            {
                foreach (UltraGridRow gridRow in grdSMBatch.Rows)
                {
                    if (gridRow.Cells["Select"].Value.ToString() != "True")
                    {
                        continue;
                    }
                    int smBatchID = Convert.ToInt32(gridRow.Cells["SMBatchID"].Value);
                    if (gridRow.Band.Columns.Exists("Status"))
                    {
                        if (gridRow.Cells["Status"].Value != null)
                        {
                            if (gridRow.Cells["Status"].Value.ToString().Equals("Running.."))
                            {
                                MessageBox.Show("Can not edit since batch is in running state");
                                return;
                            }
                            else
                            {
                                GetSMBatchCreatorUI(smBatchID);
                            }
                        }
                        else
                        {
                            GetSMBatchCreatorUI(smBatchID);
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
        /// To create instance of SMBatch Creator UI for editing
        /// </summary>
        /// <param name="smBatchID"></param>
        private void GetSMBatchCreatorUI(int smBatchID)
        {
            frmBatch = new Form();
            ctrl = new ctrlSMBatchCreator((SMBatch.BatchType)rbSetBatchType.Value);
            ctrl.InitializeSMBatchForEdit(smBatchID);
            frmBatch.Controls.Add(ctrl);
            ctrl.Dock = DockStyle.Fill;
            frmBatch.ShowIcon = false;
            frmBatch.Text = "Create Batch";
            frmBatch.Size = new System.Drawing.Size(480, 430);
            frmBatch.MinimizeBox = frmBatch.MaximizeBox = false;
            frmBatch.StartPosition = FormStartPosition.CenterParent;
            frmBatch.MaximumSize = frmBatch.MinimumSize = new System.Drawing.Size(480, 430);
            CustomThemeHelper.AddUltraFormManagerToDynamicForm(frmBatch);
            frmBatch.Load += new System.EventHandler(frmBatch_Load);
            frmBatch.ShowInTaskbar = false;
            frmBatch.ShowDialog(this);
            InitializeData();
        }

        private void rbSet_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if ((int)rbSetBatchType.Value == 1)
                {
                    ultraDateTimeEditor1.Enabled = false;
                    ultraDateTimeEditor1.DateTime = DateTime.Now;
                }
                else
                {
                    ultraDateTimeEditor1.Enabled = true;
                }

                grdSMBatch.DisplayLayout.Bands[0].Columns["Select"].SetHeaderCheckedState(grdSMBatch.Rows, CheckState.Unchecked);
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

        private void grdSMBatch_BeforeCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key == "Select" && String.Compare(e.NewValue.ToString(), "true", true) == 0)
                {
                    if (((int)e.Cell.Row.Cells["BatType"].Value) != ((int)rbSetBatchType.Value))
                    {
                        if (_grdColorizer != null)
                            _grdColorizer.AddCell(e.Cell, Color.Red, 2000);
                        //MessageBox.Show("Select only batches with correct batch type.");
                        e.Cancel = true;
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
        /// This method display warning at parent form closing
        /// </summary>
        internal bool SetUpWarningBeforeClose()
        {
            try
            {
                bool isBatchinRunningMode = false;
                if (grdSMBatch.DisplayLayout.Bands[0].Columns.Exists("Status"))
                {
                    foreach (UltraGridRow row in grdSMBatch.Rows)
                    {
                        if (row.Cells["Status"].Text.Equals("Running.."))
                        {
                            isBatchinRunningMode = true;
                            break;
                        }
                    }
                    if (isBatchinRunningMode)
                    {
                        DialogResult dialogResult = MessageBox.Show("Some batches are still in running process and will run even if UI is closed.\nAre you sure to Exit ?", "Alert", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.No)
                        {
                            return false;
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
            return true;
        }


        /// <summary>
        /// Load report layout xml if file exist
        /// </summary>
        private void LoadReportSaveLayoutXML()
        {
            try
            {
                _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                _smBatchLayoutDirectoryPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID;
                _smBatchFilePath = _smBatchLayoutDirectoryPath + @"\SMBatchLayout.xml";

                if (!Directory.Exists(_smBatchLayoutDirectoryPath))
                {
                    Directory.CreateDirectory(_smBatchLayoutDirectoryPath);
                }
                if (File.Exists(_smBatchFilePath))
                {
                    grdSMBatch.DisplayLayout.LoadFromXml(_smBatchFilePath);
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
        /// Save sm batch layout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdSMBatch != null)
                {
                    if (grdSMBatch.DisplayLayout.Bands[0].Columns.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(_smBatchFilePath))
                        {
                            grdSMBatch.DisplayLayout.SaveAsXml(_smBatchFilePath);
                            MessageBox.Show(this, "Layout Saved.", "SM Batch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
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
        }

        private void grdSMBatch_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdSMBatch);
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

        private void grdSMBatch_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.Cells.Exists("Status"))
                {
                    UltraGridCell cell = e.Row.Cells["Status"];
                    cell.Value = cell.Text;
                    switch (cell.Text)
                    {
                        case "Success":
                            cell.Appearance.ForeColor = Color.Green;
                            cell.ActiveAppearance.ForeColor = Color.Green;
                            cell.ButtonAppearance.ForeColor = Color.Green;
                            break;
                        case "Failure":
                            cell.Appearance.ForeColor = Color.Red;
                            cell.ActiveAppearance.ForeColor = Color.Red;
                            cell.ButtonAppearance.ForeColor = Color.Red;
                            break;
                        default:
                            cell.Appearance.ForeColor = Color.Yellow;
                            cell.ActiveAppearance.ForeColor = Color.Yellow;
                            cell.ButtonAppearance.ForeColor = Color.Yellow;
                            break;
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

        private void grdSMBatch_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }
    }
}
