using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Enums;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.ReconciliationNew;
using Prana.TaskManagement.Definition.Definition;
using Prana.TaskManagement.Execution;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.Tools.PL.Controls
{
    public partial class ctrlReconDashboard : UserControl, IDashboard
    {
        #region Global Member Variables

        EventHandler DisableReconOutputUI;
        EventHandler DisableApproveChanges;
        ValueList _userIDName = new ValueList();
        bool _isStartUp = true;
        bool _isBatchesTobeRemoved = false;
        int _taskId = 2; //  for recon 
        bool _isExceptionReportGenerated = false;
        bool _isDisposedFromParent = false;
        SerializableDictionary<string, int> _dictReconAmendments = new SerializableDictionary<string, int>();
        Form _formReport = null;
        ctrlExceptionReport ctrlExceptionReport1 = null;
        Form _formAddHocRecon = null;
        ctrlAdHocRecon ctrlAdHocRecon1 = null;
        Form _formAmend = null;
        static ReconDashboardLayout _reconDashboardLayout = null;
        //save and revert the scroll position of the grid
        int _rowScrollPos = int.MinValue;
        int _colScrollPos = int.MinValue;
        //list to save and revert the active/selected row
        int _activeRowIndex = int.MinValue;

        #endregion

        #region Constructors

        public ctrlReconDashboard()
        {
            InitializeComponent();
            if (!CustomThemeHelper.IsDesignMode())
            {
                _timerRefresh.Elapsed += new System.Timers.ElapsedEventHandler(TimerRefresh_Tick);
                this.IntializeControl();
                _timerRefresh.Start();
                SetUserPermissions();
            }
        }

        #endregion

        #region Private Variables

        //ctrlAmendments cntrlReconAmendments1 = null;
        //Timer for dashboard data refresh is 15 sec
        private System.Timers.Timer _timerRefresh = new System.Timers.Timer(15 * 1000);

        private List<string> checkedRowList = new List<string>();
        #endregion

        #region Private Methods

        /// <summary>
        /// Set user based permission
        /// </summary>
        private void SetUserPermissions()
        {
            try
            {
                //CHMW-1795	[Recon] - Centralize Permission code in FrmReconCancelAmend and all of its sub controls.
                //_releaseType = CachedDataManager.GetInstance.GetPranaReleaseViewType();
                //if (_releaseType == PranaReleaseViewType.CHMiddleWare)
                //{
                //    Prana.Admin.BLL.ModuleResources module = Prana.Admin.BLL.ModuleResources.ReconCancelAmend;
                //    var hasWritePermForRecon = Prana.Admin.BLL.AuthorizationManager.GetInstance().CheckAccesibilityForMoldule(module, AuthAction.Write);
                AuthAction permissionLevel = CachedDataManagerRecon.GetInstance.GetPermissionLevel();
                //CHMW-2300 [Cancel Amends] Archive and Purge buttons are disable
                if (permissionLevel == AuthAction.Write || permissionLevel == AuthAction.Approve)
                {
                    btnArchive.Enabled = true;
                    btnPurge.Enabled = true;
                }
                else
                {
                    btnArchive.Enabled = false;
                    btnPurge.Enabled = false;

                }
                //}
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

        object _locker = new object();
        private void IntializeControl()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() => IntializeControl()));
                }
                else
                {

                    lock (_locker)
                    {
                        List<string> filesList = new List<string>();

                        ReadReconAmendmentsDictionary();

                        //DateTime startDate = (DateTime)datePickerSelectDate.Value;
                        //DateTime endDate = DateTime.Now;

                        TaskExecutionManager.Initialize(Application.StartupPath);
                        filesList = TaskExecutionManager.GetTaskStatisticsAsXML(_taskId).Keys.ToList();

                        //String path = @"C:\\Users\\om.shiv\\Desktop\\dashboard\\recon\\-1_5~Transaction~Transaction_DEFAULT.xml";
                        //filesList.Add(path);

                        //Load should be called always, as dashboard files may be deleted then it should be updated on dashboard UI
                        //if (filesList.Count > 0)
                        //{
                        LoadData(filesList);
                        //}
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
        #region IDashboard Members

        /// <summary>
        /// loading data 
        /// </summary>
        /// <param name="xmlfilesList"></param>
        public void LoadData(List<string> xmlfilesList)
        {
            try
            {
                if (xmlfilesList != null)
                {
                    //Create the dataset that will be used as the datasource for the grid
                    //DataSet dsDataSource = new DataSet();
                    DataTable dtMain = new DataTable();
                    //loop to read all the XML files one by one
                    foreach (string filePath in xmlfilesList)
                    {

                        //Create the dataset to read the current XML file
                        DataSet ds = new DataSet();
                        //CHMW-2181	[Reconciliation] [Code Review] Replace dataset read xml with BufferedStream
                        //ds.ReadXml(filePath);
                        ds.ReadXml(filePath, XmlReadMode.ReadSchema);
                        if (ds.Tables.Count > 0)
                        {

                            // http://jira.nirvanasolutions.com:8080/browse/CHMW-1834
                            // user is able to see data of other accounts on import UI for which user not have permission
                            int clientID = int.MinValue;
                            if (!(ds.Tables[0].Columns.Contains("ClientID")
                                && int.TryParse(ds.Tables[0].Rows[0]["ClientID"].ToString(), out clientID)
                                && CachedDataManagerRecon.GetInstance.GetAllCompanyAccounts().ContainsKey(clientID)))
                            {
                                continue;
                            }

                            //add the column third party
                            if (!ds.Tables[0].Columns.Contains("ExecutionID"))
                            {
                                ds.Tables[0].Columns.Add("ExecutionID", typeof(string));
                            }
                            //get the third party id from the file name
                            ds.Tables[0].Rows[0]["ExecutionID"] = GetExecutionID(filePath);


                            if (!ds.Tables[0].Columns.Contains("RunTime"))
                            {
                                ds.Tables[0].Columns.Add("RunTime", typeof(string));
                            }
                            if (ds.Tables[0].Columns.Contains("EndTime") && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["EndTime"].ToString()))
                            {
                                ds.Tables[0].Rows[0]["RunTime"] = DateTime.Parse(ds.Tables[0].Rows[0]["EndTime"].ToString()).TimeOfDay;
                            }
                            if (!ds.Tables[0].Columns.Contains("btnView"))
                            {
                                ds.Tables[0].Columns.Add("btnView", typeof(string));
                                ds.Tables[0].Rows[0]["btnView"] = "View";
                            }
                            if (!ds.Tables[0].Columns.Contains("DashboardFile"))
                            {
                                ds.Tables[0].Columns.Add("DashboardFile", typeof(string));
                                ds.Tables[0].Rows[0]["DashboardFile"] = filePath;
                            }

                            //CHMW-2684	DataType mismatch error on opening Reconciliation Module.
                            //if (!ds.Tables[0].Columns.Contains("Select"))
                            //{
                            //    ds.Tables[0].Columns.Add("Select", typeof(bool));
                            //}

                            //We have to copy data first in another datatable as ds have relationships
                            //FormatName made primary key here
                            DataTable dt = ds.Tables[0].Copy();
                            //CHMW-2225 Rows on recon dashboard should be unique by the following characteristics: Format Name, Recon Type, Run Date, From Date, End Date, Date Type (trade date, process date, etc) 
                            #region Create Primary key
                            DataColumn[] columns = new DataColumn[6];
                            if (dt.Columns.Contains("Task"))
                            {
                                columns[0] = dt.Columns["Task"];
                            }
                            //Not required fields, Task value has all the fields consolidated in it.
                            //if (dt.Columns.Contains("FromDate"))
                            //{
                            //    columns[1] = dt.Columns["FromDate"];
                            //}
                            //if (dt.Columns.Contains("ToDate"))
                            //{
                            //    columns[2] = dt.Columns["ToDate"];
                            //}
                            //if (dt.Columns.Contains("FormatName"))
                            //{
                            //    columns[3] = dt.Columns["FormatName"];
                            //}
                            //if (dt.Columns.Contains("ReconDateType"))
                            //{
                            //    columns[4] = dt.Columns["ReconDateType"];
                            //}
                            //if (dt.Columns.Contains("RunDate"))
                            //{
                            //    columns[5] = dt.Columns["RunDate"];
                            //}
                            dt.PrimaryKey = columns;
                            #endregion


                            // if (dt.Columns.Contains("Status") && dt.Columns.Contains("EndTime"))
                            //{
                            //Running status batch are not comming on UI
                            //if ((dt.Rows[0]["Status"].ToString().Equals("Completed") || dt.Rows[0]["Status"].ToString().Equals("Partial Success")))
                            //{
                            //    dtMain.Merge(dt, true, MissingSchemaAction.Add);
                            //}
                            //else if (dt.Rows[0]["Status"].ToString().Equals("Failure") || string.IsNullOrEmpty(dt.Rows[0]["Status"].ToString()))
                            //{
                            //    dtMain.Merge(dt, true, MissingSchemaAction.Add);
                            //}
                            // }
                            // else
                            // {
                            //schema for each datatable may be different because of symbol validation
                            //     dtMain.Merge(dt, true, MissingSchemaAction.Add);
                            // }
                            //Row have to be added anyhow
                            dtMain.Merge(dt, true, MissingSchemaAction.Add);
                            //DataRow dr = ds.Tables[0].Rows[0];
                            ////create the structure for the data source of the grid
                            //if (dsDataSource.Tables.Count <= 0)
                            //{
                            //    dsDataSource = ds.Clone();
                            //}

                            ////add the row to the data source
                            //dsDataSource.Tables[0].ImportRow(dr);
                        }
                    }
                    if (dtMain.Rows.Count > 0)
                    {
                        ChangeDataPointsOfGridReconDashboard(dtMain);
                        grdReconDashboard.DataSource = null;
                        grdReconDashboard.DataSource = dtMain;
                    }
                    else
                    {
                        grdReconDashboard.DataSource = new DataTable();
                    }
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("The Specified XML file could not be found", "Import File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public void ChangeDataPointsOfGridReconDashboard(DataTable dt)
        {
            GeneralUtilities.ChangeColumnDataType(dt, "PerfectMatches", typeof(int));
            GeneralUtilities.ChangeColumnDataType(dt, "Breaks", typeof(int));
            GeneralUtilities.ChangeColumnDataType(dt, "FallingWithinTolerance", typeof(int));
            GeneralUtilities.ChangeColumnDataType(dt, "DataPointsReconciled", typeof(int));
            GeneralUtilities.ChangeColumnDataType(dt, "FromDate", typeof(DateTime));
            GeneralUtilities.ChangeColumnDataType(dt, "ToDate", typeof(DateTime));
        }

        public bool ArchiveData(int ID)
        {
            throw new NotImplementedException();
        }

        public bool PurgeData(int ID)
        {
            throw new NotImplementedException();
        }

        #endregion


        /// <summary>
        /// Get the batch files to remove from the dictionary
        /// </summary>
        /// <returns>List of files</returns>
        private List<string> GetBatchesToRemoveFromDictionary()
        {
            List<string> listOfFiles = new List<string>();
            try
            {
                if (grdReconDashboard.Rows.Count > 0)
                {
                    foreach (UltraGridRow row in grdReconDashboard.Rows)
                    {
                        if (row.Cells["Select"].Value.ToString() == "True")
                        {
                            if (row.Cells["Status"].Text.ToString() == "Running" || row.Cells["Status"].Text.ToString() == "Pending Completion")
                            {
                                continue;
                            }
                            if (grdReconDashboard.DisplayLayout.Bands[0].Columns.Exists("DashboardFile"))
                            {
                                String DashboardPath = row.Cells["DashboardFile"].Value.ToString();
                                listOfFiles.Add(Path.GetFileName(DashboardPath));
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
            return listOfFiles;
        }

        /// <summary>
        /// Get Files list from selected Rows
        /// </summary>
        /// <returns></returns>
        private List<String> GetFilesList()
        {
            List<String> listOfFiles = new List<string>();
            //StringBuilder batchesRunning = new StringBuilder();
            int countOfFiles = 0;
            try
            {
                bool unRemovableBatches = false;
                //DataTable dt = grdReconDashboard.DataSource as DataTable;
                if (grdReconDashboard.Rows.Count > 0)
                {
                    foreach (UltraGridRow row in grdReconDashboard.Rows)
                    {
                        if (row.Cells["Select"].Value.ToString() == "True")
                        {
                            //DataRowView rowView = row.ListObject as DataRowView;
                            //if (rowView.ro)

                            // To count selected files for archive/purge
                            //http://jira.nirvanasolutions.com:8080/browse/CHMW-1164
                            if (row.Cells["Status"].Text.ToString() == "Running" || row.Cells["Status"].Text.ToString() == "Pending Completion")
                            {
                                unRemovableBatches = true;
                                continue;
                            }
                            else
                            {

                                if (grdReconDashboard.DisplayLayout.Bands[0].Columns.Exists("ExceptionReportRef"))
                                {
                                    String ExceptionReportRef = row.Cells["ExceptionReportRef"].Value.ToString();
                                    listOfFiles.Add(ExceptionReportRef);
                                }
                                if (grdReconDashboard.DisplayLayout.Bands[0].Columns.Exists("PerfectMatchesRef"))
                                {
                                    String PerfectMatchesRef = row.Cells["PerfectMatchesRef"].Value.ToString();
                                    listOfFiles.Add(PerfectMatchesRef);
                                }
                                if (grdReconDashboard.DisplayLayout.Bands[0].Columns.Exists("BreaksRef"))
                                {
                                    String BreaksRef = row.Cells["BreaksRef"].Value.ToString();
                                    listOfFiles.Add(BreaksRef);
                                }
                                if (grdReconDashboard.DisplayLayout.Bands[0].Columns.Exists("FallingWithinToleranceRef"))
                                {
                                    String FallingWithinToleranceRef = row.Cells["FallingWithinToleranceRef"].Value.ToString();
                                    listOfFiles.Add(FallingWithinToleranceRef);
                                }
                                if (grdReconDashboard.DisplayLayout.Bands[0].Columns.Exists("DataPointsReconciledRef"))
                                {
                                    String DataPointsReconciledRef = row.Cells["DataPointsReconciledRef"].Value.ToString();
                                    listOfFiles.Add(DataPointsReconciledRef);
                                }
                                if (grdReconDashboard.DisplayLayout.Bands[0].Columns.Exists("DashboardFile"))
                                {
                                    String DashboardFile = row.Cells["DashboardFile"].Value.ToString();
                                    listOfFiles.Add(DashboardFile);
                                }
                                countOfFiles++;
                            }
                        }
                    }


                }
                if (!unRemovableBatches && countOfFiles == 0)
                {
                    MessageBox.Show("Please select a batch to archive/Purge.", "Nirvana Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            return listOfFiles;
        }


        /// <summary>
        /// Get the ID of the third party from the name of the XML file
        /// </summary>
        /// <param name="fileName">name of the XML file</param>
        /// <returns>0 if no ID found, else id of the third party</returns>
        private string GetExecutionID(string fileName)
        {
            string id = string.Empty;
            try
            {
                string newFileName = Path.GetFileName(fileName);
                string[] strName = newFileName.Split(Seperators.SEPERATOR_6);
                id = strName[0];
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
            return id;
        }


        /// <summary>
        /// delete batches on load
        /// </summary>
        private void DeleteBatches()
        {
            try
            {
                #region CHMW-975 [Recon] Automatic purge data from dashboard after a time if there is no break in recon for the template for the date range.
                if (_isBatchesTobeRemoved)
                {
                    btnPurge_Click(null, null);
                    _isStartUp = false;
                    _isBatchesTobeRemoved = false;
                }
                #endregion
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

        private void ReadReconAmendmentsDictionary()
        {
            try
            {
                //if (!string.IsNullOrEmpty(ranFilePath))
                //{
                //    string amendmentsFilePath = Application.StartupPath + "\\" + ApplicationConstants.RECON_DATA_DIRECTORY + "\\" + ApplicationConstants.RECON_AmendmentsFileName;
                //    if (File.Exists(amendmentsFilePath))
                //    {
                //        XmlDocument xmldoc = new XmlDocument();
                //        XmlNodeList xmlnode;
                //        int i = 0;
                //        xmldoc.Load(amendmentsFilePath);
                //        xmlnode = xmldoc.GetElementsByTagName("item");
                //        for (i = 0; i <= xmlnode.Count - 1; i++)
                //        {
                //            if (ranFilePath.Contains(xmlnode[i].ChildNodes.Item(0).InnerText.Trim()))
                //                xmldoc.DocumentElement.RemoveChild(xmlnode[i]);
                //        }
                //        xmldoc.Save(amendmentsFilePath);
                //        ranFilePath = string.Empty;
                //    }
                //}
                _dictReconAmendments = ReconUtilities.LoadAmendmentDictionary();
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
        /// Added by: Aman Seth,  14 Jul 2014
        /// purpose: preserve the selection of rows before data refresh
        /// </summary>
        private void GetSelectedRows()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { GetSelectedRows(); };
                    this.BeginInvoke(del);
                }
                else
                {
                    if (grdReconDashboard != null && grdReconDashboard.Rows != null && grdReconDashboard.Rows.Count > 0)
                    {
                        _rowScrollPos = grdReconDashboard.ActiveRowScrollRegion.ScrollPosition;
                        _colScrollPos = grdReconDashboard.ActiveColScrollRegion.Position;
                        checkedRowList.Clear();
                        foreach (UltraGridRow row in grdReconDashboard.Rows)
                        {
                            if (row.IsActiveRow)
                            {
                                _activeRowIndex = row.Index;

                            }
                            if (row.Cells.Exists("Select") && row.Cells["Select"].Text.ToString() == "True" && row.Cells.Exists("Task") && row.Cells.Exists("FromDate") && row.Cells.Exists("ToDate"))
                            {
                                string UniqueKey = row.Cells["Task"].Value.ToString() + Seperators.SEPERATOR_6 + row.Cells["FromDate"].Value.ToString() + Seperators.SEPERATOR_6 + row.Cells["ToDate"].Value.ToString();

                                checkedRowList.Add(UniqueKey);
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

        /// <summary>
        /// Added by:  Aman Seth,  14 Jul 2014
        /// purpose: select the previously selected rows after data refresh
        /// </summary>
        private void SetSelectedRows()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { SetSelectedRows(); };
                    this.BeginInvoke(del);
                }
                else
                {
                    if (_activeRowIndex != int.MinValue && grdReconDashboard.Rows.Count > _activeRowIndex)
                    {
                        //sets active row index
                        grdReconDashboard.Rows[_activeRowIndex].Activated = true;
                        _activeRowIndex = int.MinValue;
                    }

                    if (grdReconDashboard != null && grdReconDashboard.Rows != null && grdReconDashboard.Rows.Count > 0)
                    {
                        //Set scroll position
                        grdReconDashboard.ActiveRowScrollRegion.ScrollPosition = _rowScrollPos;
                        grdReconDashboard.ActiveColScrollRegion.Position = _colScrollPos;
                        if (checkedRowList != null && checkedRowList.Count > 0)
                        {
                            foreach (UltraGridRow row in grdReconDashboard.Rows)
                            {
                                if (row.Cells.Exists("Task") && row.Cells.Exists("FromDate") && row.Cells.Exists("ToDate") && row.Cells.Exists("Select"))
                                {
                                    string UniqueKey = row.Cells["Task"].Value.ToString() + Seperators.SEPERATOR_6 + row.Cells["FromDate"].Value.ToString() + Seperators.SEPERATOR_6 + row.Cells["ToDate"].Value.ToString();
                                    if (checkedRowList.Contains(UniqueKey))
                                    {
                                        row.Cells["Select"].Value = true;
                                    }
                                }
                            }
                            checkedRowList = new List<string>();
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
        /// Get TaskResult object from ref data of dashboard row
        /// </summary>
        /// <param name="gridRow"></param>
        /// <returns></returns>
        private TaskResult GetTaskResultGridRow(UltraGridRow gridRow)
        {
            TaskResult taskResult = null;
            try
            {
                if (gridRow != null)
                {
                    DataRowView dr = gridRow.ListObject as DataRowView;
                    if (dr != null)
                    {
                        taskResult = new TaskResult();
                        taskResult.ExecutionInfo = DeepCopyHelper.Clone(TaskExecutionCache.Instance.GetExecutionInfo("Recon_-1"));

                        if (dr.Row.Table.Columns.Contains("RawFilePath"))
                        {
                            taskResult.ExecutionInfo.ExecutionName = gridRow.Cells["Task"].Value.ToString() + Seperators.SEPERATOR_6 + Path.GetFileName(dr.Row["RawFilePath"].ToString());
                        }
                        else
                            taskResult.ExecutionInfo.ExecutionName = gridRow.Cells["Task"].Value.ToString();

                        taskResult.TaskStatistics.StartTime = DateTime.Now;
                        foreach (DataColumn column in dr.Row.Table.Columns)
                        {
                            if (!column.ColumnName.Contains("Ref"))
                            {
                                if (dr.Row.Table.Columns.Contains(column.ColumnName + "Ref"))
                                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint(column.ColumnName, dr.Row[column.ColumnName].ToString(), dr.Row[column.ColumnName + "Ref"].ToString());
                                else
                                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint(column.ColumnName, dr.Row[column.ColumnName].ToString(), null);
                            }
                        }
                        //modified by amit. changes done for http://jira.nirvanasolutions.com:8080/browse/CHMW-2938.
                        DateTime endTime = DateTime.Now;
                        DateTime.TryParse(taskResult.TaskStatistics.TaskSpecificData.AsDictionary["EndTime"].ToString(), out endTime);
                        taskResult.TaskStatistics.EndTime = endTime;
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
            return taskResult;
        }



        private void UpdaetStatusColumnRowColor(InitializeRowEventArgs e)
        {
            try
            {
                #region update row text if file has some amendments to be approved
                if (e.Row.Cells.Exists("ClientID") && e.Row.Cells["ClientID"].Value != null && !string.IsNullOrEmpty(e.Row.Cells["ClientID"].Value.ToString())
                   && e.Row.Cells.Exists("ReconType") && e.Row.Cells["ReconType"].Value != null && !string.IsNullOrEmpty(e.Row.Cells["ReconType"].Value.ToString())
                   && e.Row.Cells.Exists("TemplateName") && e.Row.Cells["TemplateName"].Value != null && !string.IsNullOrEmpty(e.Row.Cells["TemplateName"].Value.ToString())
                   && e.Row.Cells.Exists("FromDate") && e.Row.Cells["FromDate"].Value != null && !string.IsNullOrEmpty(e.Row.Cells["FromDate"].Value.ToString())
                   && e.Row.Cells.Exists("ToDate") && e.Row.Cells["ToDate"].Value != null && !string.IsNullOrEmpty(e.Row.Cells["ToDate"].Value.ToString())
                    && e.Row.Cells.Exists("FormatName") && e.Row.Cells["FormatName"].Value != null && !string.IsNullOrEmpty(e.Row.Cells["FormatName"].Value.ToString())
                    && e.Row.Cells.Exists("ReconDateType") && e.Row.Cells["ReconDateType"].Value != null && !string.IsNullOrEmpty(e.Row.Cells["ReconDateType"].Value.ToString())
                    && e.Row.Cells.Exists("RunDate") && e.Row.Cells["RunDate"].Value != null && !string.IsNullOrEmpty(e.Row.Cells["RunDate"].Value.ToString()))
                {
                    ReconParameters reconParameters = new ReconParameters();
                    reconParameters.TemplateKey = e.Row.Cells["TemplateKey"].Value.ToString();
                    reconParameters.DTFromDate = DateTime.Parse(e.Row.Cells["FromDate"].Text);
                    reconParameters.DTToDate = DateTime.Parse(e.Row.Cells["ToDate"].Text);
                    reconParameters.DTRunDate = DateTime.Parse(e.Row.Cells["RunDate"].Text);
                    reconParameters.FormatName = e.Row.Cells["FormatName"].Text;
                    reconParameters.ReconDateType = (ReconDateType)Enum.Parse(typeof(ReconDateType), e.Row.Cells["ReconDateType"].Value.ToString());
                    reconParameters.ReconFilePath = ReconUtilities.GetReconFilePath(ReconConstants.ReconDataDirectoryPath, reconParameters) + ".xml";

                    string relativeExceptionFilePath = ReconUtilities.GetRelativeExceptionFilePath(reconParameters.ReconFilePath);
                    if (_dictReconAmendments.Keys.Contains(relativeExceptionFilePath, StringComparer.InvariantCultureIgnoreCase)
                        && _dictReconAmendments[relativeExceptionFilePath] > 0 && !e.Row.Cells["Status"].Value.ToString().Equals(NirvanaTaskStatus.Running.ToString()))
                    {
                        e.Row.Cells["Status"].Value = NirvanaTaskStatus.PendingCompleted.ToString();
                    }
                }
                #endregion
                if (e.Row.Cells["Status"].Text == NirvanaTaskStatus.PendingCompleted.ToString())
                {
                    e.Row.Cells["Status"].ActiveAppearance.ForeColor = Color.Orange;
                    e.Row.Cells["Status"].Appearance.ForeColor = Color.Orange;
                }
                if (e.Row.Cells["Status"].Text == NirvanaTaskStatus.Completed.ToString())
                {
                    e.Row.Cells["Status"].ActiveAppearance.ForeColor = Color.Green;
                    e.Row.Cells["Status"].Appearance.ForeColor = Color.Green;
                }
                else if (e.Row.Cells["Status"].Text == NirvanaTaskStatus.Failure.ToString())
                {
                    e.Row.Cells["Status"].ActiveAppearance.ForeColor = Color.Red;
                    e.Row.Cells["Status"].Appearance.ForeColor = Color.Red;
                }
                else if (e.Row.Cells["Status"].Text == NirvanaTaskStatus.Canceled.ToString())
                {
                    e.Row.Cells["Status"].ActiveAppearance.ForeColor = Color.Red;
                    e.Row.Cells["Status"].Appearance.ForeColor = Color.Red;
                }
                else if (e.Row.Cells["Status"].Text == NirvanaTaskStatus.Running.ToString())
                {
                    e.Row.Cells["Status"].ActiveAppearance.ForeColor = Color.Yellow;
                    e.Row.Cells["Status"].Appearance.ForeColor = Color.Yellow;
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
        /// sets theme at the form if the `
        /// </summary>
        /// <param name="dynamicForm"></param>
        /// <param name="control"></param>
        private void SetThemeAtDynamicForm(Form dynamicForm, UserControl control)
        {
            try
            {
                System.ComponentModel.IContainer dynamicComponents = new System.ComponentModel.Container();
                Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
                Infragistics.Win.Misc.UltraPanel dynamicForm_Fill_Panel;
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea dynamicForm_Toolbars_Dock_Area_Left;
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea dynamicForm_Toolbars_Dock_Area_Right;
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea dynamicForm_Toolbars_Dock_Area_Bottom;
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea dynamicForm_Toolbars_Dock_Area_Top;
                // 
                // ultraToolbarsManager1
                // 
                ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(dynamicComponents);
                dynamicForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
                dynamicForm_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                dynamicForm_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                dynamicForm_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                dynamicForm_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                ((System.ComponentModel.ISupportInitialize)(ultraToolbarsManager1)).BeginInit();
                dynamicForm_Fill_Panel.SuspendLayout();
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1691
                //SuspendLayout();
                // 
                // ultraToolbarsManager1
                // 
                ultraToolbarsManager1.DesignerFlags = 1;
                ultraToolbarsManager1.DockWithinContainer = dynamicForm;
                ultraToolbarsManager1.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
                ultraToolbarsManager1.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.RoundedSizable;
                ultraToolbarsManager1.IsGlassSupported = false;
                // 
                // frmReconCancelAmend_Fill_Panel
                // 
                dynamicForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
                dynamicForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
                dynamicForm_Fill_Panel.Location = new System.Drawing.Point(4, 52);
                dynamicForm_Fill_Panel.Name = "dynamicForm_Fill_Panel";
                dynamicForm_Fill_Panel.Size = new System.Drawing.Size(576, 261);
                dynamicForm_Fill_Panel.TabIndex = 0;
                // 
                // _frmReconCancelAmend_Toolbars_Dock_Area_Left
                // 
                dynamicForm_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                dynamicForm_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                dynamicForm_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
                dynamicForm_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
                dynamicForm_Toolbars_Dock_Area_Left.InitialResizeAreaExtent = 4;
                dynamicForm_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 52);
                dynamicForm_Toolbars_Dock_Area_Left.Name = "dynamicForm_Toolbars_Dock_Area_Left";
                dynamicForm_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(4, 261);
                dynamicForm_Toolbars_Dock_Area_Left.ToolbarsManager = ultraToolbarsManager1;
                // 
                // _frmReconCancelAmend_Toolbars_Dock_Area_Right
                // 
                dynamicForm_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                dynamicForm_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                dynamicForm_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
                dynamicForm_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
                dynamicForm_Toolbars_Dock_Area_Right.InitialResizeAreaExtent = 4;
                dynamicForm_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(580, 52);
                dynamicForm_Toolbars_Dock_Area_Right.Name = "dynamicForm_Toolbars_Dock_Area_Right";
                dynamicForm_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(4, 261);
                dynamicForm_Toolbars_Dock_Area_Right.ToolbarsManager = ultraToolbarsManager1;
                // 
                // _frmReconCancelAmend_Toolbars_Dock_Area_Top
                // 
                dynamicForm_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                dynamicForm_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                dynamicForm_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
                dynamicForm_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
                dynamicForm_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
                dynamicForm_Toolbars_Dock_Area_Top.Name = "dynamicForm_Toolbars_Dock_Area_Top";
                dynamicForm_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(584, 52);
                dynamicForm_Toolbars_Dock_Area_Top.ToolbarsManager = ultraToolbarsManager1;
                // 
                // _frmReconCancelAmend_Toolbars_Dock_Area_Bottom
                // 
                dynamicForm_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                dynamicForm_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                dynamicForm_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
                dynamicForm_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
                dynamicForm_Toolbars_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
                dynamicForm_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 313);
                dynamicForm_Toolbars_Dock_Area_Bottom.Name = "dynamicForm_Toolbars_Dock_Area_Bottom";
                dynamicForm_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(584, 4);
                dynamicForm_Toolbars_Dock_Area_Bottom.ToolbarsManager = ultraToolbarsManager1;
                // 
                // frm
                //                 
                control.Dock = DockStyle.Fill;
                dynamicForm.Owner = this.FindForm();
                dynamicForm.ShowIcon = false;
                dynamicForm.ShowInTaskbar = false;
                dynamicForm.Controls.Add(control);
                dynamicForm.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                dynamicForm.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                dynamicForm.Controls.Add(dynamicForm_Fill_Panel);
                dynamicForm.Controls.Add(dynamicForm_Toolbars_Dock_Area_Left);
                dynamicForm.Controls.Add(dynamicForm_Toolbars_Dock_Area_Right);
                dynamicForm.Controls.Add(dynamicForm_Toolbars_Dock_Area_Bottom);
                dynamicForm.Controls.Add(dynamicForm_Toolbars_Dock_Area_Top);
                ((System.ComponentModel.ISupportInitialize)(ultraToolbarsManager1)).EndInit();
                dynamicForm_Fill_Panel.ClientArea.ResumeLayout(false);
                dynamicForm_Fill_Panel.ClientArea.PerformLayout();
                dynamicForm_Fill_Panel.ResumeLayout(false);
                dynamicForm.ResumeLayout(false);
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
        /// Add default dashboard columns
        /// </summary>
        /// <param name="band"></param>
        /// <param name="lstColumns"></param>
        private void SetupGridColumns(UltraGridBand band, List<string> lstColumns)
        {
            try
            {
                foreach (string column in lstColumns)
                {
                    if (!band.Columns.Exists(column))
                    {
                        band.Columns.Add(column);
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


        //private void grdImportDashboard_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == System.Windows.Forms.MouseButtons.Right)
        //    {
        //        contextMenuStrip1.Show();
        //    }
        //}

        /// <summary>
        /// Returns the Layout as read from the Xml
        /// </summary>
        /// <returns></returns>
        private static ReconDashboardLayout GetReconDashboardLayout()
        {
            string reconDashboardLayoutFilePath = GetReconDashboardLayoutFilePath();
            ReconDashboardLayout reconLayout = new ReconDashboardLayout();
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(reconDashboardLayoutFilePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(reconDashboardLayoutFilePath));
                }
                if (File.Exists(reconDashboardLayoutFilePath))
                {
                    using (FileStream fs = File.OpenRead(reconDashboardLayoutFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ReconDashboardLayout));
                        reconLayout = (ReconDashboardLayout)serializer.Deserialize(fs);
                    }
                }

                _reconDashboardLayout = reconLayout;
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion

            return reconLayout;
        }

        private static string GetReconDashboardLayoutFilePath()
        {
            string reconDashboardLayoutFilePath = string.Empty;
            try
            {
                int userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                string reconDashboardLayoutDirectoryPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + userID;
                reconDashboardLayoutFilePath = reconDashboardLayoutDirectoryPath + @"\ReconDashboardLayout.xml";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return reconDashboardLayoutFilePath;
        }

        /// <summary>
        /// Function Returns a list of Columns of Grid grdReport with Properties as set.
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        private void GetGridColumnLayout(UltraGrid grid)
        {

            UltraGridBand band = grid.DisplayLayout.Bands[0];
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { GetGridColumnLayout(grid); };
                    this.BeginInvoke(del);
                }
                else
                {
                    List<ColumnData> listGridCols = new List<ColumnData>();
                    foreach (UltraGridColumn gridCol in band.Columns)
                    {
                        ColumnData colData = new ColumnData();
                        colData.Key = gridCol.Key;
                        colData.Caption = gridCol.Header.Caption;
                        colData.Format = gridCol.Format;
                        colData.Hidden = gridCol.Hidden;
                        colData.VisiblePosition = gridCol.Header.VisiblePosition;
                        colData.Width = gridCol.Width;
                        colData.ExcludeFromColumnChooser = gridCol.ExcludeFromColumnChooser;
                        colData.IsGroupByColumn = gridCol.IsGroupByColumn;
                        colData.Fixed = gridCol.Header.Fixed;
                        colData.CellActivation = gridCol.CellActivation;
                        colData.ColumnStyle = gridCol.Style;
                        colData.NullText = gridCol.NullText;
                        colData.ButtonDisplayStyle = gridCol.ButtonDisplayStyle;

                        // Sorted Columns
                        colData.SortIndicator = gridCol.SortIndicator;

                        //// Summary Settings
                        //if (band.Summaries.Exists(gridCol.Key))
                        //{
                        //    string colSummKey = band.Summaries[gridCol.Key].CustomSummaryCalculator.ToString();
                        //    colData.ColSummaryKey = (colSummKey.Contains(".")) ? colSummKey.Split('.')[2] : String.Empty;
                        //    colData.ColSummaryFormat = band.Summaries[gridCol.Key].DisplayFormat;
                        //}

                        //Filter Settings
                        foreach (FilterCondition fCond in band.ColumnFilters[gridCol.Key].FilterConditions)
                        {
                            colData.FilterConditionList.Add(fCond);
                        }
                        colData.FilterLogicalOperator = band.ColumnFilters[gridCol.Key].LogicalOperator;

                        listGridCols.Add(colData);
                    }
                    ReconDashboardLayout.ReconDashboardDataColumns = listGridCols;
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

        /// <summary>
        //modified by: sachin mishra 28 jan 2015
        //Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="band"></param>
        private void grdReconDashboardSetColumns(UltraGridBand band)
        {
            try
            {
                if (ReconDashboardLayout.ReconDashboardDataColumns.Count > 0)
                {
                    List<ColumnData> listColData = ReconDashboardLayout.ReconDashboardDataColumns;
                    SetGridColumnLayout(band, listColData);
                    foreach (string col in GetAllDisplayableColumns(band))
                    {
                        band.Columns[col].Hidden = false;
                    }
                }
                else
                {
                    LoadColumns(band);
                }
                SetGridDataColumnCustomizations(band);
                //SetGridDataColumnFormatting(band);
                //SetColumnSummaries(grdData);
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
        /// Function Sets the Grid Layout as it reads from the List of Columns Layout which are Columns read from XML
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="listColData"></param>
        private static void SetGridColumnLayout(UltraGridBand band, List<ColumnData> listColData)
        {
            List<ColumnData> listSortedGridCols = new List<ColumnData>();
            ColumnsCollection gridColumns = band.Columns;// Just for readability ;)
            listColData.Sort();
            try
            {
                // Hide All
                foreach (UltraGridColumn gridCol in gridColumns)
                {
                    gridCol.Hidden = true;
                }
                //Set Columns Properties
                foreach (ColumnData colData in listColData)
                {
                    if (gridColumns.Exists(colData.Key))
                    {
                        UltraGridColumn gridCol = gridColumns[colData.Key];
                        gridCol.Width = colData.Width;
                        gridCol.Format = colData.Format;
                        gridCol.Header.Caption = colData.Caption;
                        gridCol.Header.VisiblePosition = colData.VisiblePosition;
                        gridCol.Hidden = colData.Hidden;
                        //gridCol.ExcludeFromColumnChooser = colData.ExcludeFromColumnChooser;
                        gridCol.Header.Fixed = colData.Fixed;
                        gridCol.SortIndicator = colData.SortIndicator;
                        gridCol.CellActivation = colData.CellActivation;

                        // Sorted Columns
                        if (colData.SortIndicator == SortIndicator.Descending || colData.SortIndicator == SortIndicator.Ascending)
                        {
                            listSortedGridCols.Add(colData);
                        }
                        //Summary Settings
                        //if (colData.ColSummaryKey != String.Empty)
                        //{
                        //    SummarySettings summary = band.Summaries.Add(gridCol.Key, SummaryType.Custom, riskSummFactory.GetSummaryCalculator(colData.ColSummaryKey), gridCol, SummaryPosition.UseSummaryPositionColumn, gridCol);
                        //    summary.DisplayFormat = colData.ColSummaryFormat;
                        //}
                        // Filter Settings
                        if (colData.FilterConditionList.Count > 0)
                        {
                            band.ColumnFilters[colData.Key].LogicalOperator = colData.FilterLogicalOperator;
                            band.ColumnFilters[colData.Key].FilterConditions.Clear();
                            foreach (FilterCondition fCond in colData.FilterConditionList)
                            {
                                band.ColumnFilters[colData.Key].FilterConditions.Add(fCond);
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
            // Sorted Columns are returned as they need to be handled after data is binded.
            //  return listSortedGridCols;
        }

        /// <summary>
        //modified by: sachin mishra 28 jan 2015
        //Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="band"></param>
        private void LoadColumns(UltraGridBand band)
        {
            try
            {

                List<string> colAll = GetAllDisplayableColumns(band);
                List<string> colDefault = GetAllDefaultColumns(band);
                List<string> colVisible = GetAllDefaultColumns(band);

                if (colVisible.Count < 1) // PrefFile Has No Columns
                {
                    colVisible.AddRange(colDefault);
                }

                ColumnsCollection gridColumns = band.Columns;

                foreach (UltraGridColumn gridCol in gridColumns)
                {
                    gridCol.Hidden = true;

                    if (!colAll.Contains(gridCol.Key))
                    {
                        gridCol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    }
                }
                int visiblePos = 1;
                foreach (string col in colVisible)
                {
                    if (!String.IsNullOrEmpty(col) && gridColumns.Exists(col))
                    {
                        gridColumns[col].Hidden = false;
                        gridColumns[col].Header.VisiblePosition = visiblePos;
                        gridColumns[col].Width = 100;
                        visiblePos++;
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
        //modified by: sachin mishra 28 jan 2015
        //Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="band"></param>
        /// <returns></returns>
        private List<string> GetAllDisplayableColumns(UltraGridBand band)
        {
            List<string> colAll = new List<string>();
            try
            {
                List<string> colDefault = GetAllDefaultColumns(band);
                colAll.AddRange(colDefault);
                #region Commented
                // #already added in GetAllDefaultColumns()
                //colAll.Add(COL_TradeDate);
                //colAll.Add(COL_ProcessDate);
                //colAll.Add(COL_OriginalPurchaseDate);
                //colAll.Add("TransactionType");
                //colAll.Add("ExecutedQty");
                //colAll.Add(COL_OpenQuantity);
                //colAll.Add(COL_AveragePrice);
                //colAll.Add("ClosingMark");
                //colAll.Add(COL_OpenCommission);
                //colAll.Add(COL_NetNotionalValue);
                //colAll.Add("RealizedPNL");
                //colAll.Add("UnRealizedPNL");
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
            return colAll;
        }

        /// <summary>
        //modified by: sachin mishra 28 jan 2015
        //Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="band"></param>
        /// <returns></returns>
        private List<string> GetAllDefaultColumns(UltraGridBand band)
        {
            List<string> colDefault = new List<string>();
            try
            {
                string[] array = { "Select", "TemplateName", "ReconType", "RunTime", "Status", "DataPointsReconciled", "PerfectMatches", "FallingWithinTolerance", "Breaks", "btnReport", "btnAmends", "btnRun", "ShowErrorReport", "FromDate", "ToDate", "ReconDateType", "LoggedInUser", "RunDate", "Comments" };
                List<string> lstColumns = new List<string>(array);

                SetupGridColumns(band, lstColumns);

                foreach (UltraGridColumn col in band.Columns)
                {
                    if (!lstColumns.Contains(col.Key))
                    {
                        col.Hidden = true;
                    }
                    //following line auto adjust width of columns of ultragrid according to text size of header.
                    //col.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All);                    
                }
                return lstColumns;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return colDefault;
        }

        //private void SetGridDataColumnFormatting(UltraGridBand band)
        //{
        //    try
        //    {
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
        //modified by: sachin mishra 28 jan 2015
        //Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="band"></param>
        private void SetGridDataColumnCustomizations(UltraGridBand band)
        {
            try
            {
                //int visiblePosition = 0;
                if (band.Columns.Exists("Select"))
                {
                    UltraGridColumn colBtnView = band.Columns["Select"];

                    colBtnView.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    //colBtnView.Width = 50;
                    colBtnView.Header.Caption = "";
                    colBtnView.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                    colBtnView.Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                    colBtnView.Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center;
                    //colBtnView.Header.VisiblePosition = visiblePosition++;
                    colBtnView.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    //colBtnView.Width = 20;
                    colBtnView.CellActivation = Activation.AllowEdit;
                }

                //DataSet ds = (DataSet)grdImportDashboard.DataSource;
                if (band.Columns.Exists("RunDate"))
                {
                    UltraGridColumn colRunDate = band.Columns["RunDate"];
                    colRunDate.Header.Caption = "Run Date";
                    colRunDate.Header.Appearance.TextHAlign = HAlign.Center;
                    //colRunDate.Header.VisiblePosition = visiblePosition++;
                    colRunDate.CellActivation = Activation.NoEdit;
                }

                if (band.Columns.Exists("Comments"))
                {
                    UltraGridColumn colRunDate = band.Columns["Comments"];
                    colRunDate.Header.Caption = "User Comments";
                    //colRunDate.Header.VisiblePosition = visiblePosition++;
                    colRunDate.Header.Appearance.TextHAlign = HAlign.Center;
                    colRunDate.CellActivation = Activation.NoEdit;
                }

                if (band.Columns.Exists("TemplateName"))
                {
                    UltraGridColumn colTemplateName = band.Columns["TemplateName"];
                    colTemplateName.Header.Caption = "Template";
                    //colTemplateName.Header.VisiblePosition = visiblePosition++;
                    colTemplateName.Header.Appearance.TextHAlign = HAlign.Center;
                    colTemplateName.CellActivation = Activation.NoEdit;
                }

                if (band.Columns.Exists("ReconType"))
                {
                    UltraGridColumn colReconType = band.Columns["ReconType"];
                    colReconType.Header.Caption = "Recon Type";
                    //colReconType.Header.VisiblePosition = visiblePosition++;
                    colReconType.Header.Appearance.TextHAlign = HAlign.Center;
                    colReconType.CellActivation = Activation.NoEdit;
                }

                if (band.Columns.Exists("RunTime"))
                {
                    UltraGridColumn colRunTime = band.Columns["RunTime"];
                    colRunTime.Header.Caption = "Run Time";
                    //colRunTime.Header.VisiblePosition = visiblePosition++;
                    colRunTime.Header.Appearance.TextHAlign = HAlign.Center;
                    colRunTime.CellActivation = Activation.NoEdit;
                }

                if (band.Columns.Exists("Status"))
                {
                    UltraGridColumn colStatus = band.Columns["Status"];
                    colStatus.Header.Caption = "Status";
                    colStatus.Header.Appearance.TextHAlign = HAlign.Center;
                    // colStatus.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.URL;
                    //colStatus.Header.VisiblePosition = visiblePosition++;
                    colStatus.CellActivation = Activation.NoEdit;
                }

                if (band.Columns.Exists("DataPointsReconciled"))
                {
                    UltraGridColumn colDataPointsReconciled = band.Columns["DataPointsReconciled"];
                    colDataPointsReconciled.Header.Caption = "# Data Points Reconciled";
                    colDataPointsReconciled.Header.Appearance.TextHAlign = HAlign.Center;
                    //colDataPointsReconciled.Header.VisiblePosition = visiblePosition++;
                    colDataPointsReconciled.CellActivation = Activation.NoEdit;
                }

                if (band.Columns.Exists("PerfectMatches"))
                {
                    UltraGridColumn colPerfectMatches = band.Columns["PerfectMatches"];
                    colPerfectMatches.Header.Caption = "#Perfect Matches";
                    colPerfectMatches.Header.Appearance.TextHAlign = HAlign.Center;
                    //colPerfectMatches.Header.VisiblePosition = visiblePosition++;
                    colPerfectMatches.CellActivation = Activation.NoEdit;
                }

                if (band.Columns.Exists("FallingWithinTolerance"))
                {
                    UltraGridColumn colFallingWithinTolerance = band.Columns["FallingWithinTolerance"];
                    colFallingWithinTolerance.Header.Caption = "#Falling With in Tolerance";
                    colFallingWithinTolerance.Header.Appearance.TextHAlign = HAlign.Center;
                    //colFallingWithinTolerance.Header.VisiblePosition = visiblePosition++;
                    colFallingWithinTolerance.CellActivation = Activation.NoEdit;
                }

                if (band.Columns.Exists("Breaks"))
                {
                    UltraGridColumn colBreaks = band.Columns["Breaks"];
                    colBreaks.Header.Caption = "#Breaks";
                    colBreaks.Header.Appearance.TextHAlign = HAlign.Center;
                    //colBreaks.Header.VisiblePosition = visiblePosition++;
                    colBreaks.CellActivation = Activation.NoEdit;
                }

                //add new columns
                if (band.Columns.Exists("btnReport"))
                {
                    UltraGridColumn colBtnView = band.Columns["btnReport"];

                    colBtnView.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    //colBtnView.Width = 50;
                    colBtnView.Header.Caption = "View";
                    colBtnView.Header.Appearance.TextHAlign = HAlign.Center;
                    colBtnView.NullText = "View";
                    //colBtnView.Header.VisiblePosition = visiblePosition++;
                    colBtnView.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                }
                //add new columns
                if (band.Columns.Exists("btnAmends"))
                {
                    UltraGridColumn colBtnView = band.Columns["btnAmends"];

                    colBtnView.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    //colBtnView.Width = 50;
                    colBtnView.Header.Caption = "CXL/AMDS";
                    colBtnView.Header.Appearance.TextHAlign = HAlign.Center;
                    colBtnView.NullText = "Amend";
                    //colBtnView.Header.VisiblePosition = visiblePosition++;
                    colBtnView.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                }
                if (band.Columns.Exists("btnRun"))
                {
                    UltraGridColumn colbtnRun = band.Columns["btnRun"];
                    colbtnRun.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    //colbtnRun.Width = 50;
                    colbtnRun.Header.Caption = "Run";
                    colbtnRun.Header.Appearance.TextHAlign = HAlign.Center;
                    colbtnRun.NullText = "Run";
                    //colbtnRun.Header.VisiblePosition = visiblePosition++;
                    colbtnRun.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                }
                if (band.Columns.Exists("ShowErrorReport"))
                {
                    // Added by Ankit Gupta on 13 Oct, 2014.
                    // http://jira.nirvanasolutions.com:8080/browse/CHMW-1471
                    UltraGridColumn colBtnErrorReport = band.Columns["ShowErrorReport"];
                    colBtnErrorReport.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    //colBtnErrorReport.Width = 50;
                    //colBtnErrorReport.Header.VisiblePosition = visiblePosition++;
                    colBtnErrorReport.Header.Caption = "Error Report";
                    colBtnErrorReport.Header.Appearance.TextHAlign = HAlign.Center;
                    colBtnErrorReport.NullText = "Show";
                    colBtnErrorReport.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                }
                //CHMW-2226	These fields should also be available in the manage cancel amends UI: From Date, End Date, Date Type (process date, trade date), User
                if (band.Columns.Exists("FromDate"))
                {
                    UltraGridColumn colFromDate = band.Columns["FromDate"];
                    colFromDate.Header.Caption = "From Date";
                    colFromDate.CellActivation = Activation.NoEdit;
                    colFromDate.Header.Appearance.TextHAlign = HAlign.Center;
                    colFromDate.Format = ApplicationConstants.DateFormat;
                }
                if (band.Columns.Exists("ToDate"))
                {
                    UltraGridColumn colToDate = band.Columns["ToDate"];
                    colToDate.Header.Caption = "To Date";
                    colToDate.Header.Appearance.TextHAlign = HAlign.Center;
                    colToDate.CellActivation = Activation.NoEdit;
                    colToDate.Format = ApplicationConstants.DateFormat;
                }
                if (band.Columns.Exists("ReconDateType"))
                {
                    UltraGridColumn colReconDateType = band.Columns["ReconDateType"];
                    colReconDateType.Header.Appearance.TextHAlign = HAlign.Center;
                    colReconDateType.Header.Caption = "Date Type";
                    colReconDateType.CellActivation = Activation.NoEdit;
                }
                if (band.Columns.Exists("LoggedInUser"))
                {
                    UltraGridColumn colLoggedInUser = band.Columns["LoggedInUser"];
                    colLoggedInUser.Header.Caption = "User Name";
                    colLoggedInUser.Header.Appearance.TextHAlign = HAlign.Center;
                    colLoggedInUser.CellActivation = Activation.NoEdit;
                    colLoggedInUser.ValueList = _userIDName;
                }


                //set the header checkBox to unchecked state initially       
                if (grdReconDashboard.DisplayLayout.Bands[0].Columns.Exists("Select"))
                {
                    grdReconDashboard.DisplayLayout.Bands[0].Columns["Select"].SetHeaderCheckedState(grdReconDashboard.Rows, CheckState.Unchecked);
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
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (components != null)
                    {
                        components.Dispose();
                    }
                    if (_timerRefresh != null)
                    {
                        _timerRefresh.Dispose();
                    }
                    if (_userIDName != null)
                    {
                        _userIDName.Dispose();
                    }
                    if (_formReport != null)
                    {
                        _formReport.Dispose();
                    }
                    if (_formAmend != null)
                    {
                        _formAmend.Dispose();
                    }
                    if (ctrlAdHocRecon1 != null)
                    {
                        ctrlAdHocRecon1.Dispose();
                    }
                    if (ctrlExceptionReport1 != null)
                    {
                        ctrlExceptionReport1.Dispose();
                    }
                    if (_formAddHocRecon != null)
                    {
                        _formAddHocRecon.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }


        private void SetButtonsColor()
        {
            try
            {
                btnSearchTransaction.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnSearchTransaction.ForeColor = System.Drawing.Color.White;
                btnSearchTransaction.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSearchTransaction.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSearchTransaction.UseAppStyling = false;
                btnSearchTransaction.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnAdHocRecon.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnAdHocRecon.ForeColor = System.Drawing.Color.White;
                btnAdHocRecon.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAdHocRecon.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAdHocRecon.UseAppStyling = false;
                btnAdHocRecon.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnPurge.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnPurge.ForeColor = System.Drawing.Color.White;
                btnPurge.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnPurge.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnPurge.UseAppStyling = false;
                btnPurge.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnArchive.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnArchive.ForeColor = System.Drawing.Color.White;
                btnArchive.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnArchive.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnArchive.UseAppStyling = false;
                btnArchive.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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


        #endregion

        #region Events

        /// <summary>
        /// Load all user name in value list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrlReconDashboard_Load(object sender, EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.IsDesignMode())
                {
                    Dictionary<int, string> dictUsers = CachedDataManager.GetInstance.GetAllUsersName();
                    _userIDName = new ValueList();
                    foreach (KeyValuePair<int, string> item in dictUsers)
                    {
                        _userIDName.ValueListItems.Add(item.Key, item.Value);
                    }
                    if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                    {
                        SetButtonsColor();
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

        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdReconDashboard != null && grdReconDashboard.DisplayLayout.Bands[0].Columns.Count > 0)
                {
                    GetGridColumnLayout(grdReconDashboard);
                }
                SaveReconDashboardLayout();
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This timer will hit after 15 seconds and will update dashboard grid data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerRefresh_Tick(object sender, EventArgs e)
        {
            try
            {
                // Modified by Ankit Gupta 7th Jan, 2015
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-2263
                //If form is closing, stop raising the System.Timers.Timer.Elapsed event.
                if (UIValidation.GetInstance().validate(this))
                {
                    //CHMW-2322	import dashboard still refreshing
                    if (!_isDisposedFromParent)
                    {
                        if (grdReconDashboard != null && grdReconDashboard.DisplayLayout.Bands[0].Columns.Count > 0)
                        {
                            GetGridColumnLayout(grdReconDashboard);
                        }
                        GetSelectedRows();
                        this.IntializeControl();
                        DeleteBatches();
                        SetSelectedRows();
                    }
                    ReconManager.Instance.SaveWorkflowResult();
                }
                else
                {
                    _timerRefresh.Stop();
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

        private void bgRunReconciliation_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] arguments = e.Argument as object[];
                ReconParameters reconParameters = (ReconParameters)arguments[0];
                if (!(string.IsNullOrEmpty(reconParameters.TemplateKey) || string.IsNullOrEmpty(reconParameters.ReconType) || string.IsNullOrEmpty(reconParameters.ClientID)))
                {
                    reconParameters.IsReconReportToBeGenerated = true;
                    ReconManager.ExecuteTask(reconParameters); //Recon Report is to be generated
                }
                else
                {
                    MessageBox.Show("Please select a template", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #region Buttons Events

        private void btnArchive_Click(object sender, EventArgs e)
        {

            try
            {
                //purpose: Get only the dashboard file and import data file to archive
                List<String> listOfFiles = GetFilesList();
                if (listOfFiles.Count > 0)
                {
                    TaskExecutionManager.Initialize(Application.StartupPath);
                    List<string> deleteBatchList = GetBatchesToRemoveFromDictionary();

                    Boolean isArchived = TaskExecutionManager.ArchiveFiles(listOfFiles, _taskId);
                    if (isArchived)
                    {
                        //purpose: remove the purged batch files from cache dictionary
                        foreach (string keyFile in deleteBatchList)
                        {
                            CachedDataManagerRecon.RemoveItemFromDashBoardFileCache(keyFile);
                        }
                        MessageBox.Show("Data Successfully archived.", "Nirvana Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Some Issue in archiving data. Please contact admin", "Nirvana Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnPurge_Click(object sender, EventArgs e)
        {
            try
            {

                List<String> listOfFiles = GetFilesList();
                if (listOfFiles.Count > 0)
                {
                    TaskExecutionManager.Initialize(Application.StartupPath);
                    List<string> deleteBatchList = GetBatchesToRemoveFromDictionary();
                    Boolean isPurged = TaskExecutionManager.PurgeFiles(listOfFiles, Application.StartupPath);
                    if (isPurged)
                    {
                        //purpose: remove the purged batch files from cache dictionary
                        foreach (string keyFile in deleteBatchList)
                        {
                            CachedDataManagerRecon.RemoveItemFromDashBoardFileCache(keyFile);
                        }
                        //Show message if it is not auto clean up
                        if (sender != null)
                        {
                            MessageBox.Show("Data Successfully purged.", "Nirvana Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    //Show message if it is not auto clean up
                    else if (sender != null)
                    {
                        MessageBox.Show("Some Issue in purging data. Please contact admin", "Nirvana Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Show Ad hoc recon UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdHocRecon_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: event handler should be created to handled
                ////ultraTabControl2
                //Form parent = this.FindForm();
                //if(parent!=null)
                //{
                //    if (parent.Controls.ContainsKey(""))
                //    {
                //        if (parent.Controls[""] is TabControl)
                //        {
                //            TabControl ctrl = parent.Controls["ultraTabPageRunAndManageRecon"] as TabControl;
                //            ctrl.SelectTab("ctrlAdHocRecon1");

                //        }
                //    }
                //}

                //check if form is already added or not 
                if (_formAddHocRecon == null)
                {
                    ctrlAdHocRecon1 = new ctrlAdHocRecon();
                    _formAddHocRecon = new Form();
                    _formAddHocRecon.Text = "Add Hoc Recon";
                    //setThemeAtDynamicForm(_formAddHocRecon, ctrlAdHocRecon1);
                    ctrlAdHocRecon1.Dock = DockStyle.Fill;
                    _formAddHocRecon.Controls.Add(ctrlAdHocRecon1);
                    CustomThemeHelper.AddUltraFormManagerToDynamicForm(_formAddHocRecon);

                    CustomThemeHelper.SetThemeProperties(_formAddHocRecon, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                    _formAddHocRecon.FormClosing += new System.Windows.Forms.FormClosingEventHandler(formAddHocRecon_FormClosing);
                    _formAddHocRecon.Size = new System.Drawing.Size(1107, 630);
                }
                else
                {
                    _formAddHocRecon.BringToFront();
                }
                ctrlAdHocRecon1.InitializeDataSourcesOfCombo();
                _formAddHocRecon.Show();
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

        private void btnSearchTransaction_Click(object sender, EventArgs e)
        {
            try
            {
                launchForm = ReconPrefManager.GetLaunchForm();
                if (launchForm != null)
                {
                    ListEventAargs args = new ListEventAargs();
                    args.listOfValues.Add(ApplicationConstants.CONST_Allocation.ToString());
                    launchForm(this, args);
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

        #endregion

        #region Grids Events


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdReconDashboard_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                //add filters on grid if there are rows in grid
                if (grdReconDashboard.Rows.Count > 0)
                {
                    UltraWinGridUtils.EnableFixedFilterRow(e);
                }
                //grdReconDashboard.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                UltraGridBand band = e.Layout.Bands[0];

                e.Layout.Override.ActiveRowAppearance.BackColor = Color.Black;
                e.Layout.Override.ActiveRowAppearance.BackColor2 = Color.Black;
                e.Layout.Override.ActiveRowAppearance.ForeColor = Color.White;

                e.Layout.Override.RowAppearance.BackColor = Color.Black;
                e.Layout.Override.RowAppearance.BackColor2 = Color.Black;
                e.Layout.Override.RowAppearance.ForeColor = Color.White;

                e.Layout.Override.ActiveCellAppearance.BackColor = Color.Black;
                e.Layout.Override.ActiveCellAppearance.BackColor2 = Color.Black;
                e.Layout.Override.ActiveCellAppearance.ForeColor = Color.White;

                band.Override.ButtonStyle = UIElementButtonStyle.Button3D;
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1471
                string[] array = { "Select", "TemplateName", "ReconType", "RunTime", "Status", "DataPointsReconciled", "PerfectMatches", "FallingWithinTolerance", "Breaks", "btnReport", "btnAmends", "btnRun", "ShowErrorReport", "FromDate", "ToDate", "ReconDateType", "LoggedInUser", "RunDate", "Comments" };
                List<string> lstColumns = new List<string>(array);

                //add all the columns to the grid given in lstColumns
                SetupGridColumns(band, lstColumns);

                grdReconDashboardSetColumns(band);
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


        private void grdReconDashboard_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                //Automatic purge data from dashboard after a time if there is no break in recon for the template for the date range.
                if (_isStartUp && e.Row.Cells.Exists("RunDate"))
                {
                    DateTime batchDate = DateTime.Now;
                    if (DateTime.TryParse(e.Row.Cells["RunDate"].Text.ToString(), out batchDate))
                    {
                        TimeSpan t = DateTime.Now - batchDate;
                        if (t.TotalDays > 7 && e.Row.Cells.Exists("Breaks") && e.Row.Cells.Exists("Select"))
                        {
                            if (e.Row.Cells["Breaks"].Value == null)
                            {
                                e.Row.Cells["Select"].Value = true;
                                _isBatchesTobeRemoved = true;
                            }
                            else if (e.Row.Cells["Breaks"].Value != null && (!string.IsNullOrWhiteSpace(e.Row.Cells["Breaks"].Value.ToString())) && Convert.ToInt32(e.Row.Cells["Breaks"].Value.ToString()) == 0)
                            {
                                e.Row.Cells["Select"].Value = true;
                                _isBatchesTobeRemoved = true;

                            }
                        }
                    }
                }
                e.Row.Band.Override.ActiveAppearancesEnabled = DefaultableBoolean.True;

                UpdaetStatusColumnRowColor(e);
                bool isClientPermitted = false;
                if (e.Row.Cells.Exists("ClientID")
                    && e.Row.Cells["ClientID"].Value != null
                    && Microsoft.VisualBasic.Information.IsNumeric(e.Row.Cells["ClientID"].Value.ToString())
                    && CachedDataManagerRecon.GetInstance.GetAllCompanyAccounts().Keys.Contains(int.Parse(e.Row.Cells["ClientID"].Value.ToString())))
                {
                    isClientPermitted = true;
                }
                bool _isReportAmendButtonDisabled = false;
                //CHMW-2127	[Recon] Recon batch status is failure but on view and amend button , data is coming on UI
                if (!isClientPermitted)
                {
                    // grdReconDashboard.DisplayLayout.Bands[0].Columns.All 
                    _isReportAmendButtonDisabled = true;
                    if (e.Row.Cells.Exists("btnRun"))
                    {
                        e.Row.Cells["btnRun"].Activation = Activation.Disabled;
                    }

                    if (e.Row.Cells.Exists("ShowErrorReport"))
                    {
                        e.Row.Cells["ShowErrorReport"].Activation = Activation.Disabled;
                    }
                }
                //if recon failed or not saved
                else if (e.Row.Cells["Status"].Text != NirvanaTaskStatus.Completed.ToString() && e.Row.Cells["Status"].Text != NirvanaTaskStatus.PendingCompleted.ToString() && !e.Row.Cells.Exists("Breaks"))
                {
                    _isReportAmendButtonDisabled = true;
                }
                else if (!e.Row.Cells.Exists("Breaks"))
                {
                    _isReportAmendButtonDisabled = true;
                }
                //if breaks column didn't exist or have null value---means recon is re run but not saved
                else if (e.Row.Cells["Breaks"].Value == DBNull.Value || e.Row.Cells["Breaks"].Value == null)
                {
                    _isReportAmendButtonDisabled = true;
                }
                if (_isReportAmendButtonDisabled)
                {
                    if (e.Row.Cells.Exists("btnReport"))
                    {
                        e.Row.Cells["btnReport"].Activation = Activation.Disabled;
                    }
                    if (e.Row.Cells.Exists("btnAmends"))
                    {
                        e.Row.Cells["btnAmends"].Activation = Activation.Disabled;
                    }
                }
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-2227
                // After running recon, if there is no error, then Show button should be disabled on Recon dashboard.
                if (e.Row.Cells.Exists("Status") && e.Row.Cells["Status"].Text.Equals(NirvanaTaskStatus.Completed.ToString()) &&
                    e.Row.Cells.Exists("ShowErrorReport"))
                {
                    e.Row.Cells["ShowErrorReport"].Activation = Activation.Disabled;
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
        /// Grid Button Click actions
        /// On view button click of grid row Navigate to a new Form which will show the report of the selected row 
        /// On Run Button click run the reconciliation for the selected row again 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdReconDashboard_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                ReconParameters reconParameters = new ReconParameters();
                //string client = string.Empty;
                //string reconType = string.Empty;
                //string template = string.Empty;
                //string toDate = string.Empty;
                //string fromDate = string.Empty;
                //string templateKey = string.Empty;
                //string pathPBFile = string.Empty;
                //string formatName = string.Empty;
                //string runByDate = string.Empty;
                //string runDate = string.Empty;
                string comment = string.Empty;
                string taskStatus = string.Empty;
                if (e.Cell.Band.Columns.Exists("Status"))
                {
                    taskStatus = e.Cell.Row.Cells["Status"].Text;
                }
                //if (e.Cell.Band.Columns.Exists("ClientID"))
                //{
                //    reconParameters.ClientID = e.Cell.Row.Cells["ClientID"].Text; ;
                //}
                //if (e.Cell.Band.Columns.Exists("ReconType"))
                //{
                //    reconParameters.ReconType = e.Cell.Row.Cells["ReconType"].Text;
                //}
                //if (e.Cell.Band.Columns.Exists("TemplateName"))
                //{
                //    reconParameters.TemplateName = e.Cell.Row.Cells["TemplateName"].Text;
                //}
                if (e.Cell.Band.Columns.Exists("FromDate"))
                {
                    reconParameters.FromDate = e.Cell.Row.Cells["FromDate"].Text;
                }
                if (e.Cell.Band.Columns.Exists("ToDate"))
                {
                    reconParameters.ToDate = e.Cell.Row.Cells["ToDate"].Text;
                }
                //CHMW-2225 Rows on recon dashboard should be unique by the following characteristics: Format Name, Recon Type, Run Date, From Date, End Date, Date Type (trade date, process date, etc) 
                if (e.Cell.Band.Columns.Exists("FormatName"))
                {
                    reconParameters.FormatName = e.Cell.Row.Cells["FormatName"].Text;
                }
                if (e.Cell.Band.Columns.Exists("ReconDateType"))
                {
                    reconParameters.ReconDateType = (ReconDateType)Enum.Parse(typeof(ReconDateType), e.Cell.Row.Cells["ReconDateType"].Text);
                }
                if (e.Cell.Band.Columns.Exists("RunDate") && !string.IsNullOrEmpty(e.Cell.Row.Cells["RunDate"].Text))
                {
                    reconParameters.RunDate = e.Cell.Row.Cells["RunDate"].Text;
                }
                if (e.Cell.Band.Columns.Exists("TemplateKey"))
                {
                    reconParameters.TemplateKey = e.Cell.Row.Cells["TemplateKey"].Text;
                }
                if (e.Cell.Band.Columns.Exists("PBFile"))
                {
                    reconParameters.PBFilePath = e.Cell.Row.Cells["PBFile"].Text;
                }
                if (e.Cell.Band.Columns.Exists("Comments"))
                {
                    comment = e.Cell.Row.Cells["Comments"].Text;
                }
                if (e.Cell.Text == "View")
                {
                    //check if form is already added or not                    
                    if (_formReport == null)
                    {
                        ctrlExceptionReport1 = new ctrlExceptionReport();
                        _formReport = new Form();
                        _formReport.Text = "Reconciliation Report";
                        SetThemeAtDynamicForm(_formReport, ctrlExceptionReport1);
                        CustomThemeHelper.SetThemeProperties(_formReport, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                        _formReport.Size = new System.Drawing.Size(1107, 630);
                        _formReport.FormClosing += new System.Windows.Forms.FormClosingEventHandler(formReport_FormClosing);
                    }
                    else
                    {
                        _formReport.BringToFront();
                    }
                    if (string.IsNullOrEmpty(reconParameters.ClientID))
                    {
                        MessageBox.Show("Client is not valid", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (string.IsNullOrEmpty(reconParameters.ReconType))
                    {
                        MessageBox.Show("ReconType is not valid", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (string.IsNullOrEmpty(reconParameters.TemplateKey))
                    {
                        MessageBox.Show("Template is not valid", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (string.IsNullOrEmpty(reconParameters.ToDate.ToString()) || string.IsNullOrEmpty(reconParameters.FromDate.ToString()))
                    {
                        MessageBox.Show("Date is not valid", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (string.IsNullOrEmpty(reconParameters.FormatName))
                    {
                        MessageBox.Show("Batch Name is not valid.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (string.IsNullOrEmpty(reconParameters.ReconDateType.ToString()))
                    {
                        MessageBox.Show("Run By Date  is not valid.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (string.IsNullOrEmpty(reconParameters.RunDate))
                    {
                        MessageBox.Show("Run Date is not valid.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        ctrlExceptionReport1.SetDataOnControl(reconParameters, comment);
                        _formReport.ShowDialog();
                    }
                }
                else if (e.Cell.Text == "Amend")
                {

                    //check if form is already added or not                    
                    if (_formAmend == null)
                    {
                        //cntrlReconAmendments1 = new ctrlAmendments();
                        ctrlAdHocRecon1 = new ctrlAdHocRecon();
                        _formAmend = new Form();
                        _formAmend.Text = "Reconciliation Amendments";
                        SetThemeAtDynamicForm(_formAmend, ctrlAdHocRecon1);
                        CustomThemeHelper.SetThemeProperties(_formAmend, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                        _formAmend.Size = new System.Drawing.Size(1107, 630);
                        _formAmend.FormClosing += new System.Windows.Forms.FormClosingEventHandler(formAmend_FormClosing);
                    }
                    else
                    {
                        _formAmend.BringToFront();
                    }

                    if (string.IsNullOrEmpty(reconParameters.ClientID))
                    {
                        MessageBox.Show("Client is not valid", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (string.IsNullOrEmpty(reconParameters.ReconType))
                    {
                        MessageBox.Show("ReconType is not valid", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (string.IsNullOrEmpty(reconParameters.TemplateKey))
                    {
                        MessageBox.Show("Template name is not valid", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (string.IsNullOrEmpty(reconParameters.ToDate))
                    {
                        MessageBox.Show("Date is not valid", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (string.IsNullOrEmpty(reconParameters.FormatName))
                    {
                        MessageBox.Show("Batch Name is not valid.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (string.IsNullOrEmpty(reconParameters.ReconDateType.ToString()))
                    {
                        MessageBox.Show("Run By Date  is not valid.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (string.IsNullOrEmpty(reconParameters.RunDate))
                    {
                        MessageBox.Show("Run Date is not valid.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        //if (cntrlReconAmendments1.SetDataonControl(client, reconType, template, fromDate, toDate))
                        // {
                        //    _formAmend.ShowDialog();
                        //}
                        //CHMW-2074	Recon enhancement (View file being reconciled)
                        ctrlAdHocRecon1.InitializeDataSourcesOfCombo();
                        ctrlAdHocRecon1.UpdateEvent(DisableApproveChanges);

                        if (ctrlAdHocRecon1.SetDataonControl(reconParameters, GetTaskResultGridRow(e.Cell.Row)))
                        {
                            _formAmend.ShowDialog();
                        }

                        else
                        {
                            MessageBox.Show("No data to show ", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else if (e.Cell.Text == "Run")
                {
                    //set inputData for RunReconciliationForSelectedTemplate into Comma Separated Values.
                    if (string.IsNullOrEmpty(reconParameters.TemplateKey))
                    {
                        MessageBox.Show("TemplateKey is not valid", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (string.IsNullOrEmpty(reconParameters.ReconType))
                    {
                        MessageBox.Show("ReconType is not valid", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (string.IsNullOrEmpty(reconParameters.FromDate))
                    {
                        MessageBox.Show("FromDate is not valid", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (string.IsNullOrEmpty(reconParameters.ToDate))
                    {
                        MessageBox.Show("ToDate is not valid", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (string.IsNullOrEmpty(reconParameters.ClientID.ToString()))
                    {
                        MessageBox.Show("client detail is not valid", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (string.IsNullOrEmpty(reconParameters.PBFilePath) || !File.Exists(reconParameters.PBFilePath))
                    {
                        MessageBox.Show("Trades File Missing", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (string.IsNullOrEmpty(reconParameters.FormatName))
                    {
                        MessageBox.Show("Batch Name is not valid.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (string.IsNullOrEmpty(reconParameters.ReconDateType.ToString()))
                    {
                        MessageBox.Show("Run By Date  is not valid.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (string.IsNullOrEmpty(reconParameters.RunDate))
                    {
                        MessageBox.Show("Run Date is not valid.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (NirvanaTaskStatus.Running.Equals(taskStatus))
                    {
                        MessageBox.Show("Batch already running.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        e.Cell.Row.Cells["Status"].Value = NirvanaTaskStatus.Running.ToString();
                        //Approve changes UI should be disabled if any report is changed so that unnecessary amendments cannot be approved
                        DisableApproveChanges(this, null);
                        DisableReconOutputUI(this, null);
                        //_isExceptionReportGenerated = true;
                        //Code is implemented in reconmanager                        
                        string reconFilepath = ReconUtilities.GetReconFilePath(ReconConstants.ReconDataDirectoryPath, reconParameters) + ".xml";

                        ranFilePath = ReconUtilities.GetRelativeExceptionFilePath(reconFilepath);
                        //ReconManager.ExecuteTask(templateKey, reconType, fromDate, toDate, pathPBFile, client, true, formatName, runByDate, runDate); //Recon Report is to be generated
                        object[] arguments = new object[1];
                        arguments[0] = reconParameters;

                        //modified by amit on 20/04/2015
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-3415
                        if (bgRunReconciliation.IsBusy)
                        {
                            MessageBox.Show(this, "Please wait while previous recon process is running", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else
                        {
                            bgRunReconciliation.RunWorkerAsync(arguments);
                        }
                    }
                }

                // Added by Ankit Gupta on 13 Oct, 2014.
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1471
                if (e.Cell.Column.Key == "ShowErrorReport")
                {
                    if (e.Cell.Row.Cells.Exists("Status") && e.Cell.Row.Cells["Status"].Text.Equals("Failure"))
                    {
                        if (e.Cell.Row.Cells.Exists("Error") && !string.IsNullOrWhiteSpace(e.Cell.Row.Cells["Error"].Value.ToString()))
                        {
                            string errorDetail = e.Cell.Row.Cells["Error"].Value.ToString();
                            MessageBox.Show(errorDetail, "Error Detail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("Cannot show the error details", "Error Detail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return;
                    }
                    else
                    {
                        MessageBox.Show("No Error in Recon", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public void grdReconDashboard_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdReconDashboard);
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

        public void grdReconDashboard_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }
        #endregion

        #region Form Events

        /// <summary>
        /// sets the form instance to null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void formAmend_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                ctrlAdHocRecon1.Dispose();
                //if (cntrlReconAmendments1.IsExceptionReportGenerated)
                //{
                //Approve changes UI should be disabled if any report is changed so that unnecessary amendments cannot be approved
                //_isExceptionReportGenerated = true;
                //}
                if (_formAmend != null)
                {
                    _formAmend.Dispose();
                    _formAmend = null;
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
        /// sets the form instance to null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void formAddHocRecon_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _formAddHocRecon = null;
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
        /// set the form instance to null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void formReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _formReport = null;
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

        #endregion

        #endregion

        #region Public variables
        public static string ranFilePath = string.Empty;
        //public PranaReleaseViewType _releaseType { get; set; }
        public static event EventHandler launchForm;
        #endregion

        #region Public Properties

        public bool IsExceptionReportGenerated
        {
            get { return _isExceptionReportGenerated; }
            set { _isExceptionReportGenerated = value; }
        }

        public static ReconDashboardLayout ReconDashboardLayout
        {
            get
            {
                if (_reconDashboardLayout == null)
                {
                    _reconDashboardLayout = GetReconDashboardLayout();
                }
                return _reconDashboardLayout;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// This method stops timer at parent form closing
        /// </summary>
        internal void StopDashBoradDataRefreshing()
        {
            try
            {
                _isDisposedFromParent = true;
                _timerRefresh.Stop();
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
        //modified by: sachin mishra 28 jan 2015
        //Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="eventDisableReconOutput"></param>
        /// <param name="eventDisableApproveChanges"></param>
        internal void UpdateEvent(EventHandler eventDisableReconOutput, EventHandler eventDisableApproveChanges)
        {
            try
            {
                DisableReconOutputUI = eventDisableReconOutput;
                DisableApproveChanges = eventDisableApproveChanges;
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
        /// If User changes are saved and UI is not Closed we need to restart the Timer
        /// </summary>
        internal void ReStartTimer()
        {
            _timerRefresh.Start();
            _isDisposedFromParent = false;
        }

        #endregion

        #region Public Methods

        // Added by Ankit Gupta on 1st oct, 2014.
        // http://jira.nirvanasolutions.com:8080/browse/CHMW-1369
        // Get list of templates that are currently on dashboard, so that they must not be allowed to be renamed.
        //modified by: sachin mishra 28 jan 2015
        //Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        public List<string> GetListOfTemplatesOnDashboard()
        {
            try
            {
                if (grdReconDashboard.Rows.Band.Columns.Exists("TemplateName"))
                {
                    List<string> listOfTemplates = new List<string>();
                    foreach (UltraGridRow row in grdReconDashboard.Rows.All)
                    {
                        String templateName = row.Cells["TemplateName"].Value.ToString();
                        if (!listOfTemplates.Contains(templateName))
                        {
                            listOfTemplates.Add(templateName);
                        }
                    }
                    return listOfTemplates;
                }
                return null;
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
                return null;
            }
        }


        /// <summary>
        /// Function Writes to the XMl the Layout(Columns and associated Properties) as User is using
        /// </summary>
        public static void SaveReconDashboardLayout()
        {
            try
            {
                string reconDashboardLayoutFilePath = GetReconDashboardLayoutFilePath();
                if (!string.IsNullOrEmpty(reconDashboardLayoutFilePath))
                {
                    using (XmlTextWriter writer = new XmlTextWriter(reconDashboardLayoutFilePath, Encoding.UTF8))
                    {
                        writer.Formatting = Formatting.Indented;
                        XmlSerializer serializer;
                        serializer = new XmlSerializer(typeof(ReconDashboardLayout));
                        serializer.Serialize(writer, _reconDashboardLayout);
                        writer.Flush();
                    }
                }
            }
            #region catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }


        #endregion


    }
}