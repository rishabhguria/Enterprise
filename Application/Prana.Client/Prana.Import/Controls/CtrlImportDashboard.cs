using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PM.BLL;
using Prana.TaskManagement.Definition.Definition;
using Prana.TaskManagement.Execution;
using Prana.Utilities.DateTimeUtilities;
using Prana.Utilities.ImportExportUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.Import.Controls
{
    public partial class CtrlImportDashboard : UserControl, IDashboard
    {
        #region local variables
        DateTime _dtLastRundate;
        ctrlImportReport _ctrlReport = null;
        ctrlSymbolManagement _ctrlsymbolManagement = null;
        ctrlImportTag _ctrlImportTag = null;
        ctrlImportTag _ctrlImportStatusReport = null;
        Form _frmUploadNow = null;
        Form _frmReport;
        Form _frmSymbolManagement;
        Form _frmUploadedFile;

        Form _frmImportTag;
        Form _frmImportStatusReport;

        bool _isStartUp = true;
        private List<UltraGridRow> _selectedColumnList = new List<UltraGridRow>();

        UltraGrid _grdReport = null;
        public static event EventHandler launchForm;
        public static int DefaultEquityAUECID;
        #endregion

        //Timer for dashboard data refresh is 15 sec
        private System.Timers.Timer _timerRefresh = new System.Timers.Timer(15 * 1000);

        //List to save and revert the row primary key which are checked
        private List<string> checkedRowList = new List<string>();
        //save and revert the scroll position of the grid
        int _rowScrollPos = int.MinValue;
        int _colScrollPos = int.MinValue;
        //list to save and revert the active/selected row
        int _activeRowIndex = int.MinValue;
        static ImportDashboardLayout _importDashboardLayout = null;
        static string _importDashboardLayoutFilePath = string.Empty;
        static string _importDashboardLayoutDirectoryPath = string.Empty;
        static int _userID = int.MinValue;
        bool isDisposedFromParent = false;
        UltraGridBand band;

        public CtrlImportDashboard()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }

        }

        private void SetButtonsColor()
        {
            try
            {
                btnDataMapping.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnDataMapping.ForeColor = System.Drawing.Color.White;
                btnDataMapping.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnDataMapping.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnDataMapping.UseAppStyling = false;
                btnDataMapping.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnValidate.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnValidate.ForeColor = System.Drawing.Color.White;
                btnValidate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnValidate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnValidate.UseAppStyling = false;
                btnValidate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnUpload.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnUpload.ForeColor = System.Drawing.Color.White;
                btnUpload.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnUpload.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnUpload.UseAppStyling = false;
                btnUpload.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnPurge.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnPurge.ForeColor = System.Drawing.Color.White;
                btnPurge.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnPurge.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnPurge.UseAppStyling = false;
                btnPurge.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;


                btnImport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnImport.ForeColor = System.Drawing.Color.White;
                btnImport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnImport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnImport.UseAppStyling = false;
                btnImport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;


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


        private void CtrlSymbolManagement_AutoReRunSymbolValidation(object sender, EventArgs e)
        {
            try
            {
                AutoReRunSymbolValidation();
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

        public static ImportDashboardLayout ImportDashboardLayout
        {
            get
            {
                if (_importDashboardLayout == null)
                {
                    _importDashboardLayout = GetImportDashboardLayout();
                }
                return _importDashboardLayout;
            }
        }

        ISecurityMasterServices _securityMaster = null;
        /// <summary>
        /// setup control when form loads
        /// </summary>
        public void SetUp(ISecurityMasterServices securityMasterService)
        {
            try
            {
                _timerRefresh.Elapsed += new System.Timers.ElapsedEventHandler(TimerRefresh_Tick);
                this._securityMaster = securityMasterService;
                this.IntializeControl();
                _timerRefresh.Start();
                SetUserPermissions();
                // batch are to be checked in running mode only at startup
                _isStartUp = false;
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
        /// Set user based permission
        /// </summary>
        private void SetUserPermissions()
        {
            try
            {
                grdImportDashboard.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.True;
                btnArchive.Enabled = true;
                btnImport.Enabled = true;
                btnPurge.Enabled = true;
                btnUpload.Enabled = true;
                btnDataMapping.Enabled = true;
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
        /// Initialize te control 
        /// </summary>
        private void IntializeControl()
        {
            try
            {

                List<string> filesList = new List<string>();
                //Dictionary<string, String> filesList = new Dictionary<string, String>();
                DateTime startDate = DateTime.Now.AddDays(-5);
                DateTime endDate = DateTime.Now;
                int taskId = 1;
                TaskExecutionManager.Initialize(Application.StartupPath);
                filesList = TaskExecutionManager.GetTaskStatisticsAsXML(taskId).Keys.ToList();
                //added by: Bharat Raturi, 05 jun 2014
                //purpose: Get the files from the last run date of batch
                List<string> newFilesList = UpdatedFileList(filesList);
                //string path = "C:\\Users\\bharat.raturi\\Desktop\\FIMAT\\-2_PB.FIMAT.Transaction.EnglTradeImport.xml";
                //filesList.Add(path);
                //string path1 = "C:\\Users\\bharat.raturi\\Desktop\\FIMAT\\-1_PB.FIMAT.Transaction.EnglTradeImport - Copy.xml";
                // filesList.Add(path1);

                //Load should be called always, as dashboard files may be deleted then it should be updated on dashboard UI
                //if (filesList.Count > 0)
                //{
                LoadData(newFilesList);
                //}
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

        /// <summary>
        /// Added by: bharat raturi
        /// it is possible that a batch has been run several times on different dates
        /// in this case to show the data in the dashboard, use the last run date file of the batch
        /// </summary>
        /// <param name="filesList">List of all the files of all the batches run</param>
        /// <returns>List of most recent batch files</returns>
        private List<string> UpdatedFileList(List<string> filesList)
        {
            List<string> lstFile = new List<string>();
            try
            {
                Dictionary<string, DateTime> dictFiles = new Dictionary<string, DateTime>();
                foreach (string file in filesList)
                {
                    int sInd1 = file.LastIndexOf('\\') + 1;
                    int len1 = file.Length - sInd1;
                    string strFileName = file.Substring(sInd1, len1);

                    string strDateName = file.Substring(0, file.LastIndexOf('\\'));
                    int sInd = strDateName.LastIndexOf('\\') + 1;
                    int len = strDateName.Length - sInd;
                    string date = strDateName.Substring(sInd, len);
                    DateTime dt = DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture);
                    if (dictFiles.ContainsKey(strFileName))
                    {
                        if (dictFiles[strFileName] < dt)
                        {
                            dictFiles[strFileName] = dt;
                        }
                    }
                    else
                    {
                        dictFiles.Add(strFileName, dt);
                    }
                }
                foreach (string key in dictFiles.Keys)
                {
                    string dateFile = dictFiles[key].ToString("yyyyMMdd");
                    foreach (string fName in filesList)
                    {
                        if (fName.Contains(key) && fName.Contains(dateFile))
                        {
                            lstFile.Add(fName);
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
            return lstFile;
        }

        /// <summary>
        /// Gets the name of the file for preparing report
        /// </summary>
        /// <returns>The name of the file</returns>
        private string GetFileNameFromRef(string refString)
        {
            //string fileName = string.Empty;
            string fileFullName = string.Empty;
            try
            {
                //fileName = refString.Substring(0, refString.LastIndexOf("_"));
                ////fileName = Path.GetFileName(refString);
                //fileFullName = Application.StartupPath + fileName + ".xml";
                if (!string.IsNullOrWhiteSpace(refString))
                {
                    fileFullName = Application.StartupPath + refString;
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
            return fileFullName;
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

        #region IDashboard Members

        /// <summary>
        /// Read the XMl file and load the data to the Ultragrid
        /// </summary>
        /// <param name="filesList">list of xml file paths</param>
        public void LoadData(List<string> filesList)
        {
            try
            {
                if (filesList != null)
                {
                    Dictionary<int, List<int>> CompanyThirdPartyAccounts = CachedDataManager.GetInstance.CompanyThirdPartyAccounts();

                    //Create the dataset that will be used as the datasource for the grid
                    //DataSet dsDataSource = new DataSet();
                    DataTable dtMain = new DataTable();
                    //loop to read all the XML files one by one
                    foreach (string filePath in filesList)
                    {
                        //XmlDocument xmlFile = new XmlDocument();
                        //xmlFile.LoadXml(filePath);
                        //XmlReader xmlRead = new XmlNodeReader(xmlFile);
                        //DataSet ds = new DataSet();
                        //ds.ReadXml(filePath);
                        //Create the dataset to read the current XML file
                        if (File.Exists(filePath))
                        {
                            DataSet ds = new DataSet();
                            ds.ReadXml(filePath);
                            if (ds.Tables.Count > 0)
                            {
                                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1634
                                // user is able to see data of other accounts on import UI for which user not have permission
                                string thirdPartyName = string.Empty;

                                if (ds.Tables[0].Columns.Contains("ThirdPartyType"))
                                {
                                    thirdPartyName = ds.Tables[0].Rows[0]["ThirdPartyType"].ToString();
                                }
                                if (!CompanyThirdPartyAccounts.Keys.Contains(CachedDataManager.GetInstance.GetThirdPartyIDByShortName(thirdPartyName)))
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

                                //if (!ds.Tables[0].Columns.Contains("Select"))
                                //{
                                //    ds.Tables[0].Columns.Add("Select", typeof(bool));
                                //}

                                if (!ds.Tables[0].Columns.Contains("DashboardFile"))
                                {
                                    ds.Tables[0].Columns.Add("DashboardFile", typeof(String));
                                    // DashBoard File Column should not have the application path. It should have the relative path only.
                                    //ds.Tables[0].Rows[0]["DashboardFile"] = filePath;
                                    ds.Tables[0].Rows[0]["DashboardFile"] = filePath.Replace(Application.StartupPath, "");
                                }
                                //if (!ds.Tables[0].Columns.Contains("RawFilePath"))
                                //{
                                //    ds.Tables[0].Columns.Add("RawFilePath", typeof(String));
                                //}
                                //if (!ds.Tables[0].Columns.Contains("ExecutionDate"))
                                //{
                                //    ds.Tables[0].Columns.Add("ExecutionDate", typeof(String));
                                //}
                                //We have to copy data first in another datatable as ds have relationships
                                //FormatName made primary key here
                                DataTable dt = ds.Tables[0].Copy();
                                //if (dt.Columns.Contains("Task") && dt.Columns.Contains("FtpFilePath") && dt.Columns.Contains("FileName"))
                                //{
                                //    DataColumn[] columns = new DataColumn[3];
                                //    columns[0] = dt.Columns["Task"];
                                //    columns[1] = dt.Columns["FtpFilePath"];
                                //    columns[2] = dt.Columns["FileName"];
                                //    dt.PrimaryKey = columns;
                                //}

                                //modified by: Bharat Raturi, 26 jun 2014
                                //purpose: we can use the dashboard file as the primary key because it is unique, other columns would not be required
                                if (dt.Columns.Contains("DashboardFile"))
                                {
                                    DataColumn[] columns = new DataColumn[1];
                                    columns[0] = dt.Columns["DashboardFile"];
                                    dt.PrimaryKey = columns;
                                }

                                DefaultEquityAUECID = int.Parse(ConfigurationManager.AppSettings["DefaultEquityAUECID"]);
                                DateTime LastBusinessDay1 = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(DateTime.UtcNow.Date, -5, DefaultEquityAUECID);

                                if (dt.Columns.Contains("ImportStatus") && dt.Columns.Contains("StartTime"))
                                {
                                    //show the batch data on the dashboard if import into application is not more than 5 days old
                                    if ((dt.Rows[0]["ImportStatus"].ToString().Equals("Success") || dt.Rows[0]["ImportStatus"].ToString().Equals("Partial Success")) && Convert.ToDateTime(dt.Rows[0]["StartTime"]) >= LastBusinessDay1)
                                    {
                                        dtMain.Merge(dt, true, MissingSchemaAction.Add);
                                    }
                                    //if the data import was failed
                                    else if ((dt.Rows[0]["ImportStatus"].ToString().Equals("Failure") || string.IsNullOrEmpty(dt.Rows[0]["ImportStatus"].ToString())))
                                    {
                                        dtMain.Merge(dt, true, MissingSchemaAction.Add);
                                    }
                                }
                                else
                                {
                                    //schema for each datatable may be different because of symbol validation
                                    dtMain.Merge(dt, true, MissingSchemaAction.Add);
                                }

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
                        _dtLastRundate = GetLastRunDate(dtMain);
                        //Moved in SetGridDataSource method
                        //txtLastRunDate.Text = _dtLastRundate.ToShortDateString();
                        //txtLastRunTime.Text = _dtLastRundate.ToLongTimeString();
                    }

                    #region Get Batch Names for dashboard
                    List<string> batchNames = new List<string>();
                    batchNames = BatchSetupManager.GetBatchNamesForImportDashBoard();
                    DataTable dtDashBoard = new DataTable();
                    dtDashBoard = dtMain.Clone();
                    foreach (string batchName in batchNames)
                    {
                        var matchingRow = from DataRow row in dtMain.Rows
                                          where row.Field<string>("Task").Split(Seperators.SEPERATOR_6).Contains(batchName)
                                          select row as DataRow;
                        foreach (DataRow dr in matchingRow)
                        {
                            dtDashBoard.ImportRow(dr);
                        }
                    }
                    #endregion

                    //if (dtMain.Rows.Count > 0)
                    if (dtDashBoard.Rows.Count > 0)
                    {
                        //grdImportDashboard.DataSource = dtMain;
                        //grdImportDashboard.DataSource = dtDashBoard;
                        SetGridDataSource(dtDashBoard);
                    }
                    else
                    {
                        //grdImportDashboard.DataSource = new DataTable();
                        SetGridDataSource(new DataTable());
                    }
                    setDefaultCheckBox();
                    // batch are to be checked in running mode only at startup
                    _isStartUp = false;
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("The Specified XML file could not be found", "Import File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentNullException)
            {
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
        /// CHMW-3176	[Import] - Import UI hangs while applying filter & Sorting.
        /// </summary>
        /// <param name="dt"></param>
        private void SetGridDataSource(DataTable dt)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() => SetGridDataSource(dt)));
                }
                else
                {
                    if (!isDisposedFromParent)
                    {
                        grdImportDashboard.DataSource = dt;
                        txtLastRunDate.Text = _dtLastRundate.ToShortDateString();
                        txtLastRunTime.Text = _dtLastRundate.ToLongTimeString();
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
        /// sets the checkbox to unchecked
        /// </summary>
        public void setDefaultCheckBox()
        {
            try
            {
                if (InvokeRequired)
                {
                    this.BeginInvoke(new MethodInvoker(delegate
                    {
                        setDefaultCheckBox();
                    }));

                }
                else
                {
                    if (!isDisposedFromParent)
                    {
                        foreach (UltraGridRow row in grdImportDashboard.Rows)
                        {
                            if (row.Cells.Exists("Select"))
                            {

                                UltraGridCell cell = row.Cells["Select"];
                                cell.Value = false;
                            }
                        }
                        if (grdImportDashboard.DisplayLayout.Bands[0].Columns.Exists("Select"))
                        {
                            grdImportDashboard.DisplayLayout.Bands[0].Columns["Select"].ResetHeader();
                            grdImportDashboard.DisplayLayout.Bands[0].Columns["Select"].Header.Appearance.TextHAlign = HAlign.Center;
                            grdImportDashboard.DisplayLayout.Bands[0].Columns["Select"].Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                            grdImportDashboard.DisplayLayout.Bands[0].Columns["Select"].Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                            grdImportDashboard.DisplayLayout.Bands[0].Columns["Select"].Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center;
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
        /// Get the last run date
        /// </summary>
        /// <param name="dataTable">The datatable being used as the datasource of the grid</param>
        /// <returns>THe Last run date</returns>
        private DateTime GetLastRunDate(DataTable dataTable)
        {
            _dtLastRundate = DateTime.MinValue;
            foreach (DataRow dRow in dataTable.Rows)
            {
                if (!string.IsNullOrEmpty(dRow["StartTime"].ToString()))
                {
                    DateTime dtCurrDate = DateTime.Parse(dRow["StartTime"].ToString());
                    if (dtCurrDate > _dtLastRundate)
                    {
                        _dtLastRundate = dtCurrDate;
                    }
                }
            }
            return _dtLastRundate;
        }

        public bool ArchiveData(int ID)
        {
            return false;
            //throw new NotImplementedException();
        }

        public bool PurgeData(int ID)
        {
            return false;
            //throw new NotImplementedException();
        }

        #endregion

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, List<string>> fileList = GetBatchFiles();
                //No specific format name so passing string.Empty
                OpenManualUploadForm(string.Empty, null, fileList);
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
        /// added by: Bharat raturi, 24 jun 2014
        /// Get the Ftp file names for all the batches from the dashboard
        /// </summary>
        /// <returns>list of files</returns>
        private Dictionary<string, List<string>> GetBatchFiles()
        {
            Dictionary<string, List<string>> fileList = new Dictionary<string, List<string>>();

            try
            {
                foreach (UltraGridRow row in grdImportDashboard.Rows)
                {
                    if (!string.IsNullOrWhiteSpace(row.Cells["FileName"].Text) && !string.IsNullOrWhiteSpace(row.Cells["Task"].Text))
                    {
                        string fileName = row.Cells["FileName"].Text.Trim();
                        string batchName = row.Cells["Task"].Text;
                        if (fileList.ContainsKey(batchName))
                        {
                            fileList[batchName].Add(fileName);
                        }
                        else
                        {
                            List<string> lstFile = new List<string>();
                            lstFile.Add(fileName);
                            fileList.Add(batchName, lstFile);
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
            return fileList;
        }

        private void OpenManualUploadForm(string formatName, TaskResult taskResult, Dictionary<string, List<string>> fileList)
        {
            try
            {
                //check if form is already added or not                    
                if (_frmUploadNow == null)
                {
                    _frmUploadNow = new Form();
                    _frmUploadNow.ShowIcon = false;
                    _frmUploadNow.Text = "Upload File Now";
                    ctrlManualUpload ctrManualUpload = new ctrlManualUpload();
                    ctrManualUpload.InitializeControl(formatName, taskResult, fileList);
                    _frmUploadNow.Controls.Add(ctrManualUpload);
                    ctrManualUpload.Dock = DockStyle.Fill;
                    CustomThemeHelper.AddUltraFormManagerToDynamicForm(_frmUploadNow);

                    //modified by amit on 15.04.2015
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-6926
                    CustomThemeHelper.SetThemeProperties(_frmUploadNow, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_IMPORT_DASHBOARD);

                    _frmUploadNow.MaximumSize = _frmUploadNow.MinimumSize = new System.Drawing.Size(350, 260);
                    _frmUploadNow.FormClosing += new System.Windows.Forms.FormClosingEventHandler(formUploadNow_FormClosing);
                    _frmUploadNow.Show();
                }
                else
                {
                    _frmUploadNow.BringToFront();
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
        /// Initialize the layout of the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdImportDashboard_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {


                //add filters on grid if there are rows in grid
                if (grdImportDashboard.Rows.Count > 0)
                {
                    UltraWinGridUtils.EnableFixedFilterRow(e);
                }

                band = e.Layout.Bands[0];
                //DataSet ds = (DataSet)grdImportDashboard.DataSource;
                band.Override.ActiveRowAppearance.BackColor = Color.Black;
                band.Override.ActiveRowAppearance.BackColor2 = Color.Black;
                band.Override.ActiveRowAppearance.ForeColor = Color.White;

                band.Override.ActiveCellAppearance.BackColor = Color.Black;
                band.Override.ActiveCellAppearance.BackColor2 = Color.Black;
                band.Override.ActiveCellAppearance.ForeColor = Color.White;

                band.Override.RowAppearance.BackColor = Color.Black;
                band.Override.RowAppearance.BackColor2 = Color.Black;
                band.Override.RowAppearance.ForeColor = Color.White;

                band.Override.RowAlternateAppearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;

                band.Override.ButtonStyle = UIElementButtonStyle.Button3D;

                //if (!band.Columns.Exists("Select"))
                //{
                //    band.Columns.Add("Select", "Select");
                //}

                string[] array = { "Select", "Task", "Status", "StartTime", "FileName", "ThirdPartyType", "FileType", "RetrievalStatus", "FileMetaData", "FileSize", "btnDownLoadFile", "Comments", "SymbolValidation", "ImportStatus", "btnView", "btnReRunUpload", "btnReRunSymbolValidation", "btnManualUpload", "btnImportInApp", "ShowErrorReport", "UserName", "ImportTagReport", "ImportStatusReport" };
                List<string> lstColumns = new List<string>(array);

                foreach (string column in lstColumns)
                {
                    if (!band.Columns.Exists(column))
                    {
                        band.Columns.Add(column);
                    }
                }

                if (band.Columns.Exists("btnReRunUpload"))
                {
                    band.Columns["btnReRunUpload"].Width = 120;
                }

                if (band.Columns.Exists("btnReRunSymbolValidation"))
                {
                    band.Columns["btnReRunSymbolValidation"].Width = 170;
                }

                if (band.Columns.Exists("btnManualUpload"))
                {
                    band.Columns["btnReRunUpload"].Width = 150;
                }
                if (band.Columns.Exists("btnImportInApp"))
                {
                    band.Columns["btnImportInApp"].Width = 150;
                }
                if (band.Columns.Exists("btnDownLoadFile"))
                {
                    band.Columns["btnDownLoadFile"].Width = 150;
                }

                grdImportDashboardSetColumns(lstColumns);

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

        /// <summary>
        /// Show the report on the click of the view button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdImportDashboard_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                #region btn View
                //check the button name
                if (e.Cell.Column.Key == "btnView")
                {
                    ViewFile(e);
                }
                #endregion

                #region btn ReRun Upload
                else if (e.Cell.Column.Key == "btnReRunUpload")
                {
                    ReRunUpload(e);
                }
                #endregion

                #region btn Manual Upload
                else if (e.Cell.Column.Key == "btnManualUpload")
                {
                    ManualUpload(e);
                }
                #endregion

                #region btn ReRun Symbol Validation
                else if (e.Cell.Column.Key == "btnReRunSymbolValidation")
                {
                    ReRunSymbolValidation(e, false);
                }
                #endregion

                #region btn Import In App
                else if (e.Cell.Column.Key == "btnImportInApp")
                {
                    ImportIntoApp(e);
                }
                #endregion

                #region Show Error Report
                else if (e.Cell.Column.Key == "ShowErrorReport")
                {
                    ShowErrorReport(e);
                }
                #endregion

                #region Import Status
                else if (e.Cell.Column.Key == "ImportStatus")
                {
                    ViewPartialImportUI(e);
                }
                #endregion

                #region Import Tag Report
                else if (e.Cell.Column.Key == "ImportTagReport")
                {
                    ViewImportTagReport(e);
                }
                #endregion

                #region Import Status Report
                else if (e.Cell.Column.Key == "ImportStatusReport")
                {
                    ViewImportStatusReport(e);
                }
                #endregion

                #region Download File
                else if (e.Cell.Column.Key == "btnDownLoadFile")
                {
                    DownloadFile(e);
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

        private void ViewFile(CellEventArgs e)
        {
            try
            {
                if (e.Cell.Row.Cells.Exists("Status") && (e.Cell.Row.Cells["Status"].Text.Equals("Running") || e.Cell.Row.Cells["Status"].Text.Equals("Importing")))
                {
                    MessageBox.Show("Batch is running/importing, wait for the completion of batch", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (e.Cell.Row.Cells.Exists("RetrievalStatus") && e.Cell.Row.Cells["RetrievalStatus"].Text.ToString().Equals("Failure"))
                {
                    MessageBox.Show("File missing", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (e.Cell.Row.Cells.Exists("ProcessedFilePath"))
                {
                    string filePath = Application.StartupPath + e.Cell.Row.Cells["ProcessedFilePath"].Value.ToString();
                    string formHeading = e.Cell.Row.Cells["Task"].Value.ToString();
                    //check if file exists at path
                    if (File.Exists(filePath))
                    {
                        //if form is not created
                        if (_frmUploadedFile == null || _frmUploadedFile.IsDisposed)
                        {
                            _grdReport = new UltraGrid();
                            _frmUploadedFile = new Form();

                            _frmUploadedFile.ShowIcon = false;
                            _frmUploadedFile.FormClosed += frmUploadedFile_FormClosed;
                            SetThemeAtDynamicForm(_frmUploadedFile, _grdReport);
                            if (_grdReport.DisplayLayout.Rows.Count > 0)
                                _grdReport.DisplayLayout.Rows[0].Activation = Activation.NoEdit;
                            _grdReport.InitializeRow += grdReport_InitializeRow;
                        }
                        else
                        {
                            //else previous form is bring to front
                            _frmUploadedFile.BringToFront();
                        }
                        _frmUploadedFile.Text = formHeading;

                        //modified by amit on 15.04.2015
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-6926
                        CustomThemeHelper.SetThemeProperties(_frmUploadedFile, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_IMPORT_DASHBOARD);

                        _frmUploadedFile.Show();
                        _frmUploadedFile.Cursor = Cursors.WaitCursor;
                        BackgroundWorker bgw = new BackgroundWorker();
                        bgw.DoWork += bgw_DoWork;
                        bgw.RunWorkerCompleted += bgw_RunWorkerCompleted;
                        object[] args = new object[2];
                        args[0] = filePath;
                        bgw.RunWorkerAsync(args);
                    }
                    else
                    {
                        MessageBox.Show("File does not exist", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("File retrieval process failed", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled)
                {

                    DataTable dataSource = e.Result as DataTable;

                    if (dataSource != null && !_grdReport.IsDisposed)
                    {
                        _grdReport.DataSource = dataSource;
                        _frmUploadedFile.Cursor = Cursors.Default;

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

        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] agrs = e.Argument as object[];
                string filePath = agrs[0] as string;

                if (filePath != null)
                {
                    DataTable dataSource = FileReaderFactory.GetDataTableFromDifferentFileFormats(filePath);
                    e.Result = dataSource;

                }
                else
                {
                    e.Cancel = true;
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

        private void ReRunUpload(CellEventArgs e)
        {
            try
            {
                if (e.Cell.Row.Cells.Exists("Status") && (e.Cell.Row.Cells["Status"].Text.Equals("Running") || e.Cell.Row.Cells["Status"].Text.Equals("Importing")))
                {
                    MessageBox.Show("Batch is running/importing, wait for the completion of batch", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    bool isExecuteReRunUpload = false;
                    if (e.Cell.Row.Cells.Exists("ImportStatus") && (e.Cell.Row.Cells["ImportStatus"].Text.ToString().Equals("Success") || e.Cell.Row.Cells["ImportStatus"].Text.ToString().Equals("Partial Success")))
                    {
                        if (MessageBox.Show("Data is already imported into the application, do you want to proceed with re running the upload?", "Import", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            isExecuteReRunUpload = true;
                        }
                    }
                    else
                    {
                        isExecuteReRunUpload = true;
                    }
                    if (isExecuteReRunUpload)
                    {
                        RunUpload runUpload = GetRunUploadFromGridRow(e.Cell.Row);
                        TaskResult taskResult = GetTaskResultGridRow(e.Cell.Row);

                        if (runUpload != null && taskResult != null)
                        {
                            string formatName = e.Cell.Row.Cells["Task"].Value.ToString();
                            e.Cell.Row.Cells["Status"].Value = "Running";
                            e.Cell.Row.Cells["RetrievalStatus"].Value = String.Empty;
                            e.Cell.Row.Cells["SymbolValidation"].Value = String.Empty;
                            e.Cell.Row.Cells["FileSize"].Value = String.Empty;
                            e.Cell.Row.Cells["FileMetaData"].Value = String.Empty;
                            e.Cell.Row.Cells["Comments"].Value = String.Empty;
                            e.Cell.Row.Cells["ImportStatus"].Value = String.Empty;
                            e.Cell.Row.Cells["StartTime"].Value = DateTime.Now;
                            taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportStatus", "", null);
                            taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("IsReRun", true, null);
                            taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", "", null);
                            //Added By : Manvendra Prajapati
                            //Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-11305
                            if (e.Cell.Row.Cells.Exists("StartTime") && e.Cell.Row.Cells["StartTime"].Value != null)
                            {
                                e.Cell.Row.Cells["StartTime"].Value = DateTime.Now;
                                taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("StartTime", DateTime.Now.ToString(), null);
                            }

                            if (e.Cell.Row.Cells.Exists("Status") && e.Cell.Row.Cells["Status"].Value != null)
                            {
                                e.Cell.Row.Cells["Status"].Value = "Running";
                                taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("Status", "Running", null);
                            }
                            taskResult.LogResult();

                            //SetDefaultFileSetupForTaskResult(taskResult);

                            ExecutionInfo eInfo = DeepCopyHelper.Clone(TaskExecutionCache.Instance.GetExecutionInfo("Import_-1"));
                            eInfo.IsAutoImport = true;
                            //TODO: To prompt for user action we are passing another field separated with SEPERATOR_6
                            eInfo.InputData = formatName + Seperators.SEPERATOR_6 + true.ToString();
                            List<object> lstObjects = new List<object>();
                            if (!string.IsNullOrEmpty(runUpload.FtpFilePath) && !string.IsNullOrEmpty(runUpload.RawFilePath))
                            {
                                lstObjects.Add(Path.GetDirectoryName(runUpload.FtpFilePath) + @"/" + Path.GetFileName(runUpload.RawFilePath));
                                eInfo.InputObjects = lstObjects;
                                if (!taskResult.ExecutionInfo.ExecutionName.Contains(Path.GetFileName(runUpload.RawFilePath)))
                                {
                                    eInfo.ExecutionName = taskResult.ExecutionInfo.ExecutionName + Seperators.SEPERATOR_6 + Path.GetFileName(runUpload.RawFilePath);
                                }
                                else
                                {
                                    eInfo.ExecutionName = taskResult.ExecutionInfo.ExecutionName;
                                }

                                BackgroundWorker bgwWrkerReRunUpload = new BackgroundWorker();
                                bgwWrkerReRunUpload.DoWork += bgWorkerReRunUpload_DoWork;
                                object[] arguments = new object[2];
                                arguments[0] = eInfo;
                                arguments[1] = taskResult;

                                bgwWrkerReRunUpload.RunWorkerAsync(arguments);


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

        void bgWorkerReRunUpload_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] arguments = e.Argument as object[];
                ExecutionInfo eInfo = arguments[0] as ExecutionInfo;
                TaskResult taskResult = arguments[1] as TaskResult;

                if (eInfo != null)
                {
                    //NirvanaTask task = new ImportManager();
                    NirvanaTask task = ImportManager.Instance;
                    task.Initialize(eInfo.TaskInfo);
                    task.ExecuteTask(eInfo, taskResult);

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

        private void ManualUpload(CellEventArgs e)
        {
            try
            {
                if (e.Cell.Row.Cells.Exists("ImportStatus") && e.Cell.Row.Cells.Exists("Status") && (e.Cell.Row.Cells["Status"].Text.Equals("Running") || e.Cell.Row.Cells["Status"].Text.Equals("Importing")))
                {
                    MessageBox.Show("Batch is running/importing, wait for the completion of batch", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    bool isManualUploadRequested = false;
                    if (e.Cell.Row.Cells["ImportStatus"].Text.ToString().Equals("Success") || e.Cell.Row.Cells["ImportStatus"].Text.ToString().Equals("Partial Success"))
                    {
                        DialogResult dlgResult = new DialogResult();
                        dlgResult = ConfirmationMessageBox.DisplayYesNo("Data is already imported into the application, do you want to proceed with manual upload?", "Import");
                        if (dlgResult == DialogResult.Yes)
                        {
                            isManualUploadRequested = true;
                        }
                    }
                    else
                    {
                        isManualUploadRequested = true;
                    }
                    if (isManualUploadRequested)
                    {
                        if (e.Cell.Row.Cells.Exists("Task") && !string.IsNullOrWhiteSpace(e.Cell.Row.Cells["Task"].Text.ToString()))
                        {
                            string formatName = e.Cell.Row.Cells["Task"].Value.ToString();
                            //TaskResult taskResult = GetTaskResultGridRow(e.Cell.Row);
                            //e.Cell.Row.Cells["Status"].Value = "Running";
                            e.Cell.Row.Cells["StartTime"].Value = DateTime.Now;
                            e.Cell.Row.Cells["RetrievalStatus"].Value = String.Empty;
                            e.Cell.Row.Cells["SymbolValidation"].Value = String.Empty;
                            e.Cell.Row.Cells["ImportStatus"].Value = String.Empty;
                            e.Cell.Row.Cells["FileSize"].Value = String.Empty;
                            e.Cell.Row.Cells["FileMetaData"].Value = String.Empty;
                            e.Cell.Row.Cells["Comments"].Value = String.Empty;
                            //grdImportDashboard.UpdateData();
                            //DataRow dr = ((DataRowView)e.Cell.Row.ListObject).Row;
                            TaskResult taskResult = GetTaskResultGridRow(e.Cell.Row);
                            string filename = taskResult.TaskStatistics.TaskSpecificData.GetValueForKey("DashboardFile").ToString();
                            taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("DashboardFile", filename.Replace(Application.StartupPath, ""), null);
                            //Purpose : IsReRun set to true as on manual upload workflow data needs to be refreshed on master dashboard.
                            taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("IsReRun", true, null);

                            if (taskResult != null)
                            {
                                Dictionary<string, List<string>> fileList = new Dictionary<string, List<string>>();
                                string batchName = e.Cell.Row.Cells["Task"].Text;
                                List<string> lstFile = new List<string>();
                                if (!string.IsNullOrWhiteSpace(e.Cell.Row.Cells["FileName"].Text))
                                {
                                    lstFile.Add(e.Cell.Row.Cells["FileName"].Text);
                                }
                                fileList.Add(batchName, lstFile);
                                OpenManualUploadForm(formatName, taskResult, fileList);
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

        private void ReRunSymbolValidation(CellEventArgs e, bool isAutoValidation)
        {
            try
            {
                bool isReRunSymbolValidation = false;
                if (!isAutoValidation)
                {
                    if (e.Cell.Row.Cells.Exists("Status") && (e.Cell.Row.Cells["Status"].Text.Equals("Running") || e.Cell.Row.Cells["Status"].Text.Equals("Importing")))
                    {
                        MessageBox.Show("Batch is running/importing, wait for the completion of batch", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (e.Cell.Row.Cells.Exists("Comments") && e.Cell.Row.Cells["Comments"].Text.Equals("No Data"))
                    {
                        MessageBox.Show("No symbol/symbols to validate.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (e.Cell.Row.Cells.Exists("RetrievalStatus") && e.Cell.Row.Cells["RetrievalStatus"].Value.ToString().Equals("Success") && e.Cell.Row.Cells.Exists("ImportStatus"))
                    {
                        if (e.Cell.Row.Cells["ImportStatus"].Text.ToString().Equals("Success") || e.Cell.Row.Cells["ImportStatus"].Text.ToString().Equals("Partial Success"))
                        {
                            DialogResult dlgResult = new DialogResult();
                            dlgResult = ConfirmationMessageBox.DisplayYesNo("Data is already imported into the application, do you want to proceed with symbol validation?", "Import");
                            if (dlgResult == DialogResult.Yes)
                            {
                                isReRunSymbolValidation = true;
                            }
                        }

                        else
                        {
                            isReRunSymbolValidation = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("File retrieval process failed", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    isReRunSymbolValidation = true;
                }

                if (isReRunSymbolValidation)
                {
                    RunUpload runUpload = GetRunUploadFromGridRow(e.Cell.Row);
                    TaskResult taskResult = GetTaskResultGridRow(e.Cell.Row);


                    if (runUpload != null && taskResult != null)
                    {
                        taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("IsReRunSymbolValidation", true, null);
                        taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", "", null);

                        // Modified By : Manvendra Prajapati
                        // Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-10602

                        if (e.Cell.Row.Cells.Exists("StartTime") && e.Cell.Row.Cells["StartTime"].Value != null)
                        {
                            e.Cell.Row.Cells["StartTime"].Value = DateTime.Now;
                            taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("StartTime", DateTime.Now.ToString(), null);
                        }

                        if (e.Cell.Row.Cells.Exists("Status") && e.Cell.Row.Cells["Status"].Value != null)
                        {
                            e.Cell.Row.Cells["Status"].Value = "Running";
                            taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("Status", "Running", null);
                        }
                        taskResult.LogResult();


                        //Modified by omshiv, run manual upload using BackgroundWorker

                        BackgroundWorker bgwWorkerManualUpload = new BackgroundWorker();
                        bgwWorkerManualUpload.DoWork += bgWorkerReRunSymbolValidation_DoWork;
                        object[] arguments = new object[2];
                        arguments[0] = runUpload;
                        arguments[1] = taskResult;
                        bgwWorkerManualUpload.RunWorkerAsync(arguments);

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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bgWorkerReRunSymbolValidation_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] arguments = e.Argument as object[];
                RunUpload runUpload = arguments[0] as RunUpload;
                TaskResult taskResult = arguments[1] as TaskResult;

                if (runUpload != null)
                {

                    taskResult.TaskStatistics.Status = NirvanaTaskStatus.Running;
                    taskResult.LogResult();
                    //ImportManager importManager = new ImportManager();
                    //Save the userId  in the task result
                    taskResult.ExecutionInfo.IsAutoImport = true;
                    ImportManager.Instance.RunProceedToValidation(runUpload, taskResult);
                    ImportManager.Instance.SaveWorkflowResult();
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

        private void ImportIntoApp(CellEventArgs e)
        {
            try
            {
                bool isImportInApp = false;
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1164
                if (e.Cell.Row.Cells.Exists("Status") && (e.Cell.Row.Cells["Status"].Text.Equals("Running") || e.Cell.Row.Cells["Status"].Text.Equals("Importing")))
                {
                    MessageBox.Show("Batch is running/importing, wait for the completion of batch", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (e.Cell.Row.Cells.Exists("Comments") && (e.Cell.Row.Cells["Comments"].Text.Equals("No Data") || e.Cell.Row.Cells["Comments"].Text.Equals("Duplicate file")))
                {
                    MessageBox.Show("No data to Import.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (e.Cell.Row.Cells.Exists("RetrievalStatus") && e.Cell.Row.Cells["RetrievalStatus"].Value.ToString().Equals("Success"))
                {
                    if (e.Cell.Row.Cells.Exists("ImportStatus") && (e.Cell.Row.Cells["ImportStatus"].Text.ToString().Equals("Success") || e.Cell.Row.Cells["ImportStatus"].Text.ToString().Equals("Partial Success")))
                    {
                        if (MessageBox.Show("Data is already imported into the application, do you want to proceed with importing again?", "Import", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            isImportInApp = true;
                        }
                    }
                    else
                    {
                        isImportInApp = true;
                    }
                    if (isImportInApp)
                    {
                        //Modified By Faisal Shah
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-2100
                        e.Cell.Row.Cells["StartTime"].Value = DateTime.Now;
                        e.Cell.Row.Cells["Status"].Value = NirvanaTaskStatus.Importing;
                        //Modified By amit
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-3478
                        e.Cell.Row.Cells["ImportStatus"].Value = String.Empty;
                        List<Tuple<RunUpload, TaskResult>> tupleList = new List<Tuple<RunUpload, TaskResult>>();
                        RunUpload runUpload = GetRunUploadFromGridRow(e.Cell.Row);
                        TaskResult taskResult = GetTaskResultGridRow(e.Cell.Row);
                        Tuple<RunUpload, TaskResult> tuple = new Tuple<RunUpload, TaskResult>(runUpload, taskResult);
                        tupleList.Add(tuple);
                        if (tupleList.Count > 0)
                        {
                            //modified by amit on 20/04/2015
                            //http://jira.nirvanasolutions.com:8080/browse/CHMW-3400
                            if (bgImportIntoApp.IsBusy)
                            {
                                MessageBox.Show(this, "Please wait while previous import is running", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else
                            {
                                bgImportIntoApp.RunWorkerAsync(tupleList);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("File retrieval process failed", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private static void ShowErrorReport(CellEventArgs e)
        {
            try
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
                }
                else
                {
                    MessageBox.Show("No Error in Upload", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void ViewPartialImportUI(CellEventArgs e)
        {
            try
            {
                if (e.Cell.Row.Cells.Exists("Status") && (e.Cell.Row.Cells["Status"].Text.Equals("Running") || e.Cell.Row.Cells["Status"].Text.Equals("Importing")))
                {
                    MessageBox.Show("Batch is running/importing, wait for the completion of batch", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (e.Cell.Row.Cells.Exists("Comments") && (e.Cell.Row.Cells["Comments"].Text.Equals("No Data") || e.Cell.Row.Cells["Comments"].Text.Equals("Duplicate file")))
                {
                    MessageBox.Show("No Report to Show.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (e.Cell.Row.Cells.Exists("ImportDataRef"))
                {
                    string fileName = GetFileNameFromRef(e.Cell.Row.Cells["ImportDataRef"].Value.ToString());
                    Dictionary<string, string> dictReportData = new Dictionary<string, string>();
                    if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                    {
                        dictReportData.Add("FileName", fileName);
                        if (e.Cell.Row.Cells.Exists("FileType") && !string.IsNullOrEmpty(e.Cell.Row.Cells["FileType"].Value.ToString()))
                        {
                            dictReportData.Add("FileType", e.Cell.Row.Cells["FileType"].Value.ToString());
                        }
                        if (e.Cell.Row.Cells.Exists("ThirdPartyType") && !string.IsNullOrEmpty(e.Cell.Row.Cells["ThirdPartyType"].Value.ToString()))
                        {
                            dictReportData.Add("ThirdPartyType", e.Cell.Row.Cells["ThirdPartyType"].Value.ToString());
                        }
                        if (e.Cell.Row.Cells.Exists("TotalSymbols") && !string.IsNullOrEmpty(e.Cell.Row.Cells["TotalSymbols"].Value.ToString()))
                        {
                            dictReportData.Add("TotalSymbols", e.Cell.Row.Cells["TotalSymbols"].Value.ToString());
                        }
                        if (e.Cell.Row.Cells.Exists("NonValidatedSymbols") && !string.IsNullOrEmpty(e.Cell.Row.Cells["NonValidatedSymbols"].Value.ToString()))
                        {
                            dictReportData.Add("NonValidatedSymbols", e.Cell.Row.Cells["NonValidatedSymbols"].Value.ToString());
                        }
                        if (e.Cell.Row.Cells.Exists("ValidatedSymbols") && !string.IsNullOrEmpty(e.Cell.Row.Cells["ValidatedSymbols"].Value.ToString()))
                        {
                            dictReportData.Add("ValidatedSymbols", e.Cell.Row.Cells["ValidatedSymbols"].Value.ToString());
                        }
                        if (e.Cell.Row.Cells.Exists("EndTime") && !string.IsNullOrEmpty(e.Cell.Row.Cells["EndTime"].Value.ToString()))
                        {
                            dictReportData.Add("Date", e.Cell.Row.Cells["EndTime"].Value.ToString());
                        }
                        if (e.Cell.Row.Cells.Exists("AccountCount") && !string.IsNullOrEmpty(e.Cell.Row.Cells["AccountCount"].Value.ToString()))
                        {
                            dictReportData.Add("AccountCount", e.Cell.Row.Cells["AccountCount"].Value.ToString());
                        }
                        if (e.Cell.Row.Cells.Exists("SecApproveFailedCount") && !string.IsNullOrEmpty(e.Cell.Row.Cells["SecApproveFailedCount"].Value.ToString()))
                        {
                            dictReportData.Add("PendingSymbols", e.Cell.Row.Cells["SecApproveFailedCount"].Value.ToString());
                        }
                        if (_frmReport == null)
                        {
                            _ctrlReport = new ctrlImportReport();
                            _frmReport = new Form();
                            _frmReport.ShowIcon = false;
                            _frmReport.FormClosed += frmReport_FormClosed;
                            SetThemeAtDynamicForm(_frmReport, _ctrlReport);
                            _frmReport.Size = new System.Drawing.Size(700, 500);
                            _frmReport.MinimumSize = _frmReport.Size;
                            _frmReport.StartPosition = FormStartPosition.Manual;
                        }
                        _frmReport.Text = "Import status report - " + e.Cell.Row.Cells["Task"].Value.ToString();
                        if (_ctrlReport == null)
                        {
                            _ctrlReport = new ctrlImportReport();
                        }

                        //TaskResult & RunUpload also needs to be sent for Partial Import
                        TaskResult result = GetTaskResultGridRow(e.Cell.Row);
                        RunUpload runUpload = GetRunUploadFromGridRow(e.Cell.Row);

                        //modified by amit on 15.04.2015
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-6926
                        CustomThemeHelper.SetThemeProperties(_frmReport, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_IMPORT_DASHBOARD);

                        _frmReport.Show();
                        _ctrlReport.SetProperties(dictReportData, runUpload, result);
                        _ctrlReport.FillReport();

                        BringFormToFront(_frmReport);
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
        /// CHMW-2305 [Implementation] import dashboard updates-Part 2
        /// </summary>
        /// <param name="e"></param>
        private void ViewImportTagReport(CellEventArgs e)
        {
            try
            {
                if (e.Cell.Row.Cells.Exists("Status") && (e.Cell.Row.Cells["Status"].Text.Equals("Running") || e.Cell.Row.Cells["Status"].Text.Equals("Importing")))
                {
                    MessageBox.Show("Batch is running/importing, wait for the completion of batch", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // Modified By : Manvendra P. Jira: CHMW-3544
                else if (e.Cell.Row.Cells.Exists("Comments") && e.Cell.Row.Cells["Comments"].Text.Equals("No Data") || e.Cell.Row.Cells["Comments"].Text.Equals("Duplicate file"))
                {
                    MessageBox.Show("No Report to Show.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (e.Cell.Row.Cells.Exists("ImportTagFilePath") && !string.IsNullOrEmpty(e.Cell.Row.Cells["ImportTagFilePath"].Value.ToString()))
                {
                    string fileName = GetFileNameFromRef(e.Cell.Row.Cells["ImportTagFilePath"].Value.ToString());
                    if (File.Exists(fileName))
                    {
                        if (_frmImportTag == null)
                        {
                            _ctrlImportTag = new ctrlImportTag();
                            _frmImportTag = new Form();
                            _frmImportTag.ShowIcon = false;
                            _frmImportTag.FormClosed += frmImportTag_FormClosed;
                            SetThemeAtDynamicForm(_frmImportTag, _ctrlImportTag);
                            _frmImportTag.Size = new System.Drawing.Size(700, 500);
                            _frmImportTag.MinimumSize = _frmImportTag.Size;
                            _frmImportTag.StartPosition = FormStartPosition.Manual;
                        }
                        _frmImportTag.Text = "Import Tag report - " + e.Cell.Row.Cells["Task"].Value.ToString();
                        if (_ctrlImportTag == null)
                        {
                            _ctrlImportTag = new ctrlImportTag();
                        }
                        if (e.Cell.Row.Cells.Exists("FileType") && !string.IsNullOrEmpty(e.Cell.Row.Cells["FileType"].Value.ToString()))
                        {

                            _ctrlImportTag.SetTreeView(fileName, "Import Tag", "TransactionType", CachedDataManager.GetInstance.GetPranaImportTags(), false, null);
                        }

                        CustomThemeHelper.SetThemeProperties(_frmImportTag, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_IMPORT_DASHBOARD);

                        _frmImportTag.Show();
                        BringFormToFront(_frmImportTag);
                    }
                    else
                    {
                        MessageBox.Show("File not found.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                //Added by sachin mishra purpose:-CHMW-3229
                else
                {
                    MessageBox.Show("Please check the Mapping and Transformation file.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void ViewImportStatusReport(CellEventArgs e)
        {
            try
            {
                if (e.Cell.Row.Cells.Exists("Status") && (e.Cell.Row.Cells["Status"].Text.Equals("Running") || e.Cell.Row.Cells["Status"].Text.Equals("Importing")))
                {
                    MessageBox.Show("Batch is running/importing, wait for the completion of batch", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (e.Cell.Row.Cells.Exists("Comments") && (e.Cell.Row.Cells["Comments"].Text.Equals("No Data") || e.Cell.Row.Cells["Comments"].Text.Equals("Duplicate file")))
                {
                    MessageBox.Show("No Report to Show.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (e.Cell.Row.Cells.Exists("ImportDataRef"))
                {
                    string fileName = GetFileNameFromRef(e.Cell.Row.Cells["ImportDataRef"].Value.ToString());
                    if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                    {
                        if (_frmImportStatusReport == null)
                        {
                            _ctrlImportStatusReport = new ctrlImportTag();
                            _frmImportStatusReport = new Form();
                            _frmImportStatusReport.ShowIcon = false;
                            _frmImportStatusReport.FormClosed += frmImportStatusReport_FormClosed;
                            SetThemeAtDynamicForm(_frmImportStatusReport, _ctrlImportStatusReport);
                            _frmImportStatusReport.Size = new System.Drawing.Size(700, 500);
                            _frmImportStatusReport.MinimumSize = _frmImportStatusReport.Size;
                            _frmImportStatusReport.StartPosition = FormStartPosition.Manual;
                        }
                        _frmImportStatusReport.Text = "Import Status report - " + e.Cell.Row.Cells["Task"].Value.ToString();
                        if (_ctrlImportStatusReport == null)
                        {
                            _ctrlImportStatusReport = new ctrlImportTag();
                        }
                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        dict.Add("Imported", "Imported");
                        dict.Add("NotImported", "Non-Imported");
                        if (e.Cell.Row.Cells.Exists("FileType") && !string.IsNullOrEmpty(e.Cell.Row.Cells["FileType"].Value.ToString()))
                        {
                            string fileType = e.Cell.Row.Cells["FileType"].Value.ToString();
                            _ctrlImportStatusReport.SetTreeView(fileName, "Import Status", "ImportStatus", dict, true, fileType);
                        }

                        CustomThemeHelper.SetThemeProperties(_frmImportStatusReport, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_IMPORT_DASHBOARD);

                        _frmImportStatusReport.Show();
                        BringFormToFront(_frmImportStatusReport);
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

        private void DownloadFile(CellEventArgs e)
        {
            try
            {
                RunUpload runUpload = GetRunUploadFromGridRow(e.Cell.Row);
                if (runUpload != null && !string.IsNullOrEmpty(e.Cell.Row.Cells["FileName"].Value.ToString()))
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.InitialDirectory = @"C:\";
                    saveFileDialog1.Title = "Save text Files";
                    //saveFileDialog1.CheckFileExists = true;
                    //saveFileDialog1.CheckPathExists = true;
                    saveFileDialog1.DefaultExt = "txt";
                    saveFileDialog1.Filter = "Text files (*.txt)|*.txt|Csv Files (*.csv)|*.csv|Excel Files (*.xls)|*.xls";
                    saveFileDialog1.FilterIndex = 2;
                    saveFileDialog1.RestoreDirectory = true;
                    saveFileDialog1.OverwritePrompt = true;
                    saveFileDialog1.FileName = e.Cell.Row.Cells["FileName"].Value.ToString();

                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        FileInfo fileInfo = new FileInfo(saveFileDialog1.FileName);

                        if (File.Exists(runUpload.ProcessedFilePath))
                        {
                            File.Copy(runUpload.ProcessedFilePath, fileInfo.DirectoryName + "\\" + fileInfo.Name, true);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Problem in downloading file.\nplease rerun batch and try again!", "Download Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmImportTag_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (_frmImportTag != null)
                {
                    _frmImportTag = null;
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmImportStatusReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (_frmImportStatusReport != null)
                {
                    _frmImportStatusReport = null;
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
        ///  Added by: Aman seth
        /// Modified By Faisal Shah
        ///	http://jira.nirvanasolutions.com:8080/browse/CHMW-2100
        ///  Purpose : Import the batches into application      
        /// </summary>
        /// <param name="cell"></param>
        private void gridImportIntoAppClick(List<Tuple<RunUpload, TaskResult>> tupleList)
        {
            try
            {
                if (tupleList.Count > 0)
                {
                    foreach (Tuple<RunUpload, TaskResult> tuple in tupleList)
                    {
                        RunUpload runUpload = tuple.Item1;
                        TaskResult taskResult = tuple.Item2;
                        if (runUpload != null && taskResult != null)
                        {
                            taskResult.TaskStatistics.Status = NirvanaTaskStatus.Importing;
                            taskResult.LogResult();
                            taskResult.ExecutionInfo.IsAutoImport = true;
                            //ImportManager importManager = new ImportManager();
                            //load data from existing file exist to avoid overwriting previous data.                       
                            ImportManager.Instance.RunImportIntoApplication(runUpload, taskResult, true, null);
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


        private void grdReport_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                foreach (UltraGridCell cell in e.Row.Cells)
                {
                    cell.Activation = Activation.ActivateOnly;
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void frmReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (_frmReport != null)
                {
                    _frmReport = null;
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmUploadedFile_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (_frmUploadedFile != null)
                {
                    _frmUploadedFile = null;
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


        void _frmSymbolManagement_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (_frmSymbolManagement != null)
                {
                    _ctrlsymbolManagement.UnwireEvents();
                    _ctrlsymbolManagement.autoReRunSymbolValidation -= new EventHandler(CtrlSymbolManagement_AutoReRunSymbolValidation);
                    _frmSymbolManagement = null;
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


        private void BringFormToFront(Form form)
        {
            if (form.WindowState == FormWindowState.Minimized)
            {
                form.WindowState = FormWindowState.Normal;

            }
            form.Location = new Point(this.Location.X + 30, this.Location.Y + 30);
            form.BringToFront();
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
                //If form is closing
                if (UIValidation.GetInstance().validate(this))
                {
                    //CHMW-2322	import dashboard still refreshing
                    if (!isDisposedFromParent)
                    {
                        if (grdImportDashboard != null && grdImportDashboard.DisplayLayout.Bands[0].Columns.Count > 0)
                        {
                            GetGridColumnLayout(grdImportDashboard);
                        }
                        GetSelectedRows();
                        this.IntializeControl();
                        SetSelectedRows();
                    }
                    ImportManager.Instance.SaveWorkflowResult();
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

        /// <summary>
        /// This method stops timer at parent form closing
        /// </summary>
        internal bool StopDashBoradDataRefreshing()
        {
            try
            {
                isDisposedFromParent = true;
                bool isBatchinRunningMode = false;
                if (grdImportDashboard.DisplayLayout.Bands[0].Columns.Exists("Status") && grdImportDashboard.DisplayLayout.Bands[0].Columns.Exists("UserName"))
                {
                    foreach (UltraGridRow row in grdImportDashboard.Rows)
                    {
                        if ((row.Cells["Status"].Text == "Running" || row.Cells["Status"].Text == "Importing") && string.Compare(row.Cells["UserName"].Value.ToString(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString(), true) == 0)
                        {
                            isBatchinRunningMode = true;
                            break;
                        }
                    }
                    if (isBatchinRunningMode)
                    {
                        DialogResult dialogResult = MessageBox.Show("There are some batch still in running/importing stage. Are you sure you want to Exit", "Alert", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.No)
                        {
                            isDisposedFromParent = false;
                            return false;
                        }
                    }
                }

                _timerRefresh.Stop();
                _timerRefresh.Dispose();
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
        /// Get Files list from selected Rows
        /// </summary>
        /// <returns></returns>
        private List<String> GetFilesListForArchiving()
        {
            List<String> listOfFiles = new List<string>();
            int countFailImportStatus = 0;
            int countFiles = 0;
            StringBuilder batchesRunning = new StringBuilder();
            try
            {
                if (grdImportDashboard.Rows.Count > 0)
                {
                    foreach (UltraGridRow row in grdImportDashboard.Rows)
                    {
                        if (row.Cells["Select"].Value.ToString() == "True")
                        {
                            //DataRowView rowView = row.ListObject as DataRowView;
                            //if (rowView.ro)

                            // To count selected files for archive/purge
                            //http://jira.nirvanasolutions.com:8080/browse/CHMW-1164
                            if (row.Cells["Status"].Text.ToString() == "Running" || row.Cells["Status"].Text.ToString() == "Importing")
                            {
                                batchesRunning.Append("Batch " + row.Cells["Task"].Text.ToString() + " with batch status as running/importing and filename " + row.Cells["FileName"].Text.ToString() + " cannot be archived.\n");
                                continue;
                            }

                            #region commented

                            // Purge/archieve only those files whose ImportStatus is success or partial success
                            //commented BY: Bharat Raturi,22 may 2014
                            //purpose: count the file regradless of its import status
                            //if (grdImportDashboard.DisplayLayout.Bands[0].Columns.Exists("ImportStatus"))
                            //{
                            //We are not deleting file metadata and files
                            //because these files are used to identify duplicacy of files
                            if (row.Cells["ImportStatus"].Text.ToString().Equals("Success") || row.Cells["ImportStatus"].Text.ToString().Equals("Partial Success"))
                            {
                                if (grdImportDashboard.DisplayLayout.Bands[0].Columns.Exists("FileMetaDataRef") && !string.IsNullOrWhiteSpace(row.Cells["FileMetaDataRef"].Value.ToString()))
                                {
                                    String FileMetaDataRef = row.Cells["FileMetaDataRef"].Value.ToString();
                                    listOfFiles.Add(FileMetaDataRef);
                                }
                                #endregion
                                countFiles++;
                                if (grdImportDashboard.DisplayLayout.Bands[0].Columns.Exists("RunUploadRef"))
                                {
                                    String RunUploadRef = row.Cells["RunUploadRef"].Value.ToString();
                                    listOfFiles.Add(RunUploadRef);
                                }
                                if (grdImportDashboard.DisplayLayout.Bands[0].Columns.Exists("ImportDataRef"))
                                {
                                    String TotalImportDataRef = row.Cells["ImportDataRef"].Value.ToString();
                                    listOfFiles.Add(TotalImportDataRef);
                                }

                                #region commented
                                //if (dr.Table.Columns.Contains("RawFilePath"))
                                //{
                                //    String RawFilePath = dr["RawFilePath"].ToString();
                                //    listOfFiles.Add(RawFilePath);
                                //}
                                //if (dr.Table.Columns.Contains("ProcessedFilePath"))
                                //{
                                //    String ProcessedFilePath = dr["ProcessedFilePath"].ToString();
                                //    listOfFiles.Add(ProcessedFilePath);
                                //}
                                #endregion

                                if (grdImportDashboard.DisplayLayout.Bands[0].Columns.Exists("DashboardFile"))
                                {
                                    String DashboardPath = row.Cells["DashboardFile"].Value.ToString();
                                    listOfFiles.Add(DashboardPath);
                                }
                                if (grdImportDashboard.DisplayLayout.Bands[0].Columns.Exists("ValidatedSymbolsRef"))
                                {
                                    String DashboardPath = row.Cells["ValidatedSymbolsRef"].Value.ToString();
                                    listOfFiles.Add(DashboardPath);
                                }
                            }
                            else
                            {
                                countFailImportStatus++;
                            }
                        }
                    }
                    if (countFailImportStatus > 0)
                        MessageBox.Show("Batch with import status as success or partial success can be archived only.", "Nirvana Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (batchesRunning.Length > 0)
                    {
                        MessageBox.Show(batchesRunning.ToString(), "Nirvana Info", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else if (countFiles == 0 && countFailImportStatus == 0)
                        MessageBox.Show("Please select a file to archive.", "Nirvana Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Get Files list from selected Rows
        /// </summary>
        /// <returns></returns>
        private List<String> GetFilesListForPurging()
        {
            List<String> listOfFiles = new List<string>();
            int countFiles = 0;
            StringBuilder batchesRunning = new StringBuilder();
            try
            {
                if (grdImportDashboard.Rows.Count > 0)
                {
                    foreach (UltraGridRow row in grdImportDashboard.Rows)
                    {
                        if (row.Cells["Select"].Value.ToString() == "True")
                        {
                            //DataRowView rowView = row.ListObject as DataRowView;
                            //if (rowView.ro)

                            // To count selected files for archive/purge
                            //http://jira.nirvanasolutions.com:8080/browse/CHMW-1164
                            if (row.Cells["Status"].Text.ToString() == "Running" || row.Cells["Status"].Text.ToString() == "Importing")
                            {
                                batchesRunning.Append("Batch " + row.Cells["Task"].Text.ToString() + " with batch status as running/importing and filename " + row.Cells["FileName"].Text.ToString() + " cannot be deleted.\n");
                                continue;
                            }
                            CheckeNoDataFileOfBatch(row, true);

                            //modified by : sachin mishra   Jira-CHMW-3260
                            //if (grdImportDashboard.DisplayLayout.Bands[0].Columns.Exists("FileMetaDataRef") && !string.IsNullOrWhiteSpace(row.Cells["FileMetaDataRef"].Value.ToString()))
                            //{
                            //    String FileMetaDataRef = row.Cells["FileMetaDataRef"].Value.ToString();
                            //    listOfFiles.Add(FileMetaDataRef);
                            //}
                            countFiles++;
                            if (grdImportDashboard.DisplayLayout.Bands[0].Columns.Exists("RunUploadRef"))
                            {
                                String RunUploadRef = row.Cells["RunUploadRef"].Value.ToString();
                                listOfFiles.Add(RunUploadRef);
                            }
                            if (grdImportDashboard.DisplayLayout.Bands[0].Columns.Exists("ImportDataRef"))
                            {
                                String TotalImportDataRef = row.Cells["ImportDataRef"].Value.ToString();
                                listOfFiles.Add(TotalImportDataRef);
                            }
                            if (grdImportDashboard.DisplayLayout.Bands[0].Columns.Exists("DashboardFile"))
                            {
                                String DashboardPath = row.Cells["DashboardFile"].Value.ToString();
                                listOfFiles.Add(DashboardPath);
                            }
                            if (grdImportDashboard.DisplayLayout.Bands[0].Columns.Exists("ValidatedSymbolsRef"))
                            {
                                String DashboardPath = row.Cells["ValidatedSymbolsRef"].Value.ToString();
                                listOfFiles.Add(DashboardPath);
                            }
                        }
                    }
                    if (batchesRunning.Length > 0)
                    {
                        MessageBox.Show(batchesRunning.ToString(), "Nirvana Info", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else if (countFiles == 0)
                        MessageBox.Show("Please select a file to be Purged.", "Nirvana Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnArchive_Click(object sender, EventArgs e)
        {
            try
            {
                //modified by: Bharat raturi, 11 jun 2014
                //purpose: Get only the dashboard file and import data file to archive
                List<String> listOfFiles = GetFilesListForArchiving();
                if (listOfFiles.Count > 0)
                {
                    int _taskId = 1;
                    TaskExecutionManager.Initialize(Application.StartupPath);
                    List<string> deleteBatchList = GetBatchesToRemoveFromDictionaryForArchiving();

                    //bool isArchived = TaskExecutionManager.ArchiveFiles(listOfFiles, _taskId);
                    bool isArchived = TaskExecutionManager.ArchiveInValidData(listOfFiles, _taskId);
                    if (isArchived)
                    {
                        //added by: Bharat Raturi, 25 jun 2014
                        //purpose: remove the purged batch files from cache dictionary
                        foreach (string keyFile in deleteBatchList)
                        {
                            ImportCacheManager.RemoveItemFromDashBoardFileCache(keyFile);
                        }
                        UpdateChangesInGrid();
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
                List<String> listOfFiles = GetFilesListForPurging();
                if (listOfFiles.Count > 0)
                {
                    TaskExecutionManager.Initialize(Application.StartupPath);
                    List<string> deleteBatchList = GetBatchesToRemoveFromDictionaryForPurging();
                    Boolean isPurged = TaskExecutionManager.PurgeFiles(listOfFiles, Application.StartupPath);
                    if (isPurged)
                    {
                        //added by: Bharat Raturi, 25 jun 2014
                        //purpose: remove the purged batch files from cache dictionary
                        foreach (string keyFile in deleteBatchList)
                        {
                            ImportCacheManager.RemoveItemFromDashBoardFileCache(keyFile);
                        }
                        UpdateChangesInGrid();
                        MessageBox.Show("Data Successfully purged.", "Nirvana Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
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
        /// Get the batch files to remove from the dictionary
        /// </summary>
        /// <returns>List of files</returns>
        private List<string> GetBatchesToRemoveFromDictionaryForPurging()
        {
            List<string> listOfFiles = new List<string>();
            try
            {
                if (grdImportDashboard.Rows.Count > 0)
                {
                    foreach (UltraGridRow row in grdImportDashboard.Rows)
                    {
                        if (row.Cells["Select"].Value.ToString() == "True")
                        {
                            if (row.Cells["Status"].Text.ToString() == "Running" || row.Cells["Status"].Text.ToString() == "Importing")
                            {
                                continue;
                            }
                            if (grdImportDashboard.DisplayLayout.Bands[0].Columns.Exists("DashboardFile"))
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return listOfFiles;
        }
        /// <summary>
        /// Get the batch files to remove from the dictionary
        /// </summary>
        /// <returns>List of files</returns>
        private List<string> GetBatchesToRemoveFromDictionaryForArchiving()
        {
            List<string> listOfFiles = new List<string>();
            try
            {
                if (grdImportDashboard.Rows.Count > 0)
                {
                    foreach (UltraGridRow row in grdImportDashboard.Rows)
                    {
                        if (row.Cells["Select"].Value.ToString() == "True")
                        {
                            if (row.Cells["Status"].Text.ToString() == "Running" || row.Cells["Status"].Text.ToString() == "Importing" || row.Cells["ImportStatus"].Text.ToString() == "Failure" || row.Cells["ImportStatus"].Text.ToString() == "")
                            {
                                continue;
                            }
                            if (grdImportDashboard.DisplayLayout.Bands[0].Columns.Exists("DashboardFile"))
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return listOfFiles;
        }

        /// <summary>
        /// added by: Bharat Raturi,22 may 2014
        /// purpose: delete the rows showing the archived files 
        /// </summary>
        private void UpdateChangesInGrid()
        {
            int count = grdImportDashboard.Rows.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                //delete the rows which are checked and there import status in not "Running"
                if (grdImportDashboard.Rows[i].Cells["Select"].Value.ToString() == "True" && (grdImportDashboard.Rows[i].Cells["Status"].Text.ToString() != "Importing" || grdImportDashboard.Rows[i].Cells["Status"].Text.ToString() != "Running"))
                {
                    grdImportDashboard.Rows[i].Delete(false);
                }
            }
            DataTable dt = grdImportDashboard.DataSource as DataTable;
            dt.AcceptChanges();
            grdImportDashboard.UpdateData();
        }

        private void grdImportDashboard_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                bool isBatchFailed = false;
                int minutes = int.MinValue;
                string errMsg = string.Empty;
                bool result = int.TryParse(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_AccountLockReleaseInterval), out minutes);
                if (!result)
                {
                    throw new Exception("Cannot convert value of ImportCleanUpInterval to integer from configuration file. Please check settings");
                }
                if (!(grdImportDashboard != null && !grdImportDashboard.IsDisposed && !grdImportDashboard.Disposing))
                {
                    return;
                }
                if (_isStartUp)
                {
                    if (e.Row.Cells.Exists("Status") && (e.Row.Cells["Status"].Value.ToString() == "Running" || e.Row.Cells["Status"].Value.ToString() == "Importing") && e.Row.Cells.Exists("UserName") && e.Row.Cells["UserName"].Value.ToString() == CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString())
                    {
                        errMsg = "Batch Execution Timed Out. Please contact administrator.";
                        isBatchFailed = true;

                    }
                }

                if (e.Row.Cells.Exists("StartTime") && e.Row.Cells.Exists("Status") && e.Row.Cells["Status"].Value != null && (e.Row.Cells["Status"].Value.ToString() == "Running" || e.Row.Cells["Status"].Value.ToString() == "Importing"))
                {
                    DateTime dtBatchRunTime = DateTime.MinValue;
                    if (DateTime.TryParse(e.Row.Cells["StartTime"].Value.ToString(), out dtBatchRunTime))
                    {
                        TimeSpan dif = DateTime.Now - dtBatchRunTime;
                        if (minutes != int.MinValue && dif.TotalMinutes > double.Parse(minutes.ToString()))
                        {
                            errMsg = "Batch Execution terminated abnormally";
                            isBatchFailed = true;
                        }
                    }

                }
                if (isBatchFailed && e.Row.Cells.Exists("Status"))
                {
                    TaskResult taskResult = GetTaskResultGridRow(e.Row);
                    if (taskResult != null)
                    {

                        taskResult.Error = new Exception(errMsg);
                        taskResult.TaskStatistics.Status = NirvanaTaskStatus.Failure;
                        //taskResult.LogResult();
                        ImportManager.Instance.UpdateTaskSpecificDataPoints(this, taskResult);
                    }
                    e.Row.Cells["Status"].Value = "Failure";
                }
                if (e.Row.Cells.Exists("Task"))
                {
                    UltraGridCell cell = e.Row.Cells["Task"];
                    string[] taskCumFile = cell.Text.Split(Seperators.SEPERATOR_6);
                    cell.Value = taskCumFile[0];
                }
                if (e.Row.Cells.Exists("ImportStatus"))
                {
                    UltraGridCell cell = e.Row.Cells["ImportStatus"];
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
                            //cell.Appearance.ForeColor = Color.Blue;
                            //cell.ActiveAppearance.ForeColor = Color.Blue;
                            //cell.ButtonAppearance.ForeColor = Color.Blue;
                            //modified by amit on 15.04.2015
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-6926
                            cell.Appearance.ForeColor = Color.OrangeRed;
                            cell.ActiveAppearance.ForeColor = Color.OrangeRed;
                            cell.ButtonAppearance.ForeColor = Color.OrangeRed;
                            break;
                    }
                }

                if (e.Row.Cells.Exists("Status"))
                {
                    UltraGridCell cell = e.Row.Cells["Status"];
                    cell.Value = cell.Text;
                    switch (cell.Text)
                    {
                        case "Completed":
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
                            //cell.Appearance.ForeColor = Color.Blue;
                            //cell.ActiveAppearance.ForeColor = Color.Blue;
                            //cell.ButtonAppearance.ForeColor = Color.Blue;
                            //modified by amit on 15.04.2015
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-6926
                            cell.Appearance.ForeColor = Color.OrangeRed;
                            cell.ActiveAppearance.ForeColor = Color.OrangeRed;
                            cell.ButtonAppearance.ForeColor = Color.OrangeRed;
                            break;
                    }
                }


                if (e.Row.Cells.Exists("RetrievalStatus"))
                {
                    UltraGridCell cell = e.Row.Cells["RetrievalStatus"];
                    switch (cell.Text)
                    {
                        case "Success":
                            cell.Appearance.ForeColor = Color.Green;
                            cell.ActiveAppearance.ForeColor = Color.Green;
                            break;
                        case "Failure":
                            cell.Appearance.ForeColor = Color.Red;
                            cell.ActiveAppearance.ForeColor = Color.Red;
                            e.Row.Cells["FileMetaData"].Value = "ERROR";
                            e.Row.Cells["FileSize"].Value = "ERROR";
                            break;
                        //case "Running":
                        //    cell.Appearance.ForeColor = Color.Yellow;
                        //    cell.ActiveAppearance.ForeColor = Color.Yellow;
                        //    break;
                        default:
                            //modified by amit on 15.04.2015
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-6926
                            cell.Appearance.ForeColor = Color.OrangeRed;
                            cell.ActiveAppearance.ForeColor = Color.OrangeRed;
                            break;
                    }
                }

                if (e.Row.Cells.Exists("SymbolValidation"))
                {
                    UltraGridCell cell = e.Row.Cells["SymbolValidation"];
                    switch (cell.Text)
                    {
                        case "Success":
                            cell.Appearance.ForeColor = Color.Green;
                            cell.ActiveAppearance.ForeColor = Color.Green;
                            break;
                        case "Failure":
                            cell.Appearance.ForeColor = Color.Red;
                            cell.ActiveAppearance.ForeColor = Color.Red;
                            break;
                        default:
                            //modified by amit on 15.04.2015
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-6926
                            cell.Appearance.ForeColor = Color.OrangeRed;
                            cell.ActiveAppearance.ForeColor = Color.OrangeRed;
                            break;
                    }
                }
                //false
                CheckeNoDataFileOfBatch(e.Row, false);
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

        private void CheckeNoDataFileOfBatch(UltraGridRow row, bool isFileToBeUpdated)
        {
            try
            {
                string filePath = string.Empty;
                //modified by amit. changes done for http://jira.nirvanasolutions.com:8080/browse/CHMW-3706
                if (row != null)
                {
                    TaskResult taskResult = GetTaskResultGridRow(row);
                    if (taskResult.TaskStatistics.TaskSpecificData.AsDictionary.ContainsKey("Task"))
                    {
                        filePath = Application.StartupPath + @"\DashBoardData\Import\" + taskResult.TaskStatistics.TaskSpecificData.AsDictionary["Task"].ToString() + Seperators.SEPERATOR_6.ToString() + "NoData.xml";
                    }
                    List<string> files = new List<string>();
                    bool isFileChanged = false;
                    if (File.Exists(filePath))
                    {
                        using (FileStream fs = File.OpenRead(filePath))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
                            files = (List<string>)serializer.Deserialize(fs);
                            fs.Flush();
                            if (taskResult.TaskStatistics.TaskSpecificData.AsDictionary.ContainsKey("Comments")
                            && taskResult.TaskStatistics.TaskSpecificData.AsDictionary["Comments"].ToString().Equals("No Data"))
                            {
                                if (files.Contains(taskResult.ExecutionInfo.ExecutionName.ToLower()))
                                {
                                    row.Hidden = true;
                                }
                                else
                                {
                                    files.Add(taskResult.ExecutionInfo.ExecutionName.ToLower());
                                    isFileChanged = true;
                                }
                            }
                            else if (files.Contains(taskResult.ExecutionInfo.ExecutionName.ToLower()))
                            {
                                files.Remove(taskResult.ExecutionInfo.ExecutionName.ToLower());
                                isFileChanged = true;
                            }
                        }
                    }
                    else
                    {
                        if (taskResult.TaskStatistics.TaskSpecificData.AsDictionary.ContainsKey("Comments")
                            && taskResult.TaskStatistics.TaskSpecificData.AsDictionary["Comments"].ToString().Equals("No Data"))
                        {
                            isFileChanged = true;
                            files.Add(taskResult.ExecutionInfo.ExecutionName.ToLower());

                            //empty value to read file even if there is no data
                            //files.Add(string.Empty);
                        }
                    }
                    if (isFileChanged && isFileToBeUpdated)
                    {
                        isFileChanged = false;
                        using (XmlTextWriter writer = new XmlTextWriter(filePath, Encoding.UTF8))
                        {
                            writer.Formatting = Formatting.Indented;
                            XmlSerializer serialize;
                            serialize = new XmlSerializer(typeof(List<string>));
                            serialize.Serialize(writer, files);
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



        private void btnValidate_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, object> dictReportData = new Dictionary<string, object>();

                //int totalSymbols = 0;
                //int validationsSecMaster = 0;
                //int validationsAPI = 0;
                int faildValidations = 0;
                DateTime dtStartDate = DateTime.MinValue;
                DateTime dtEndDate = DateTime.MinValue;


                //if (!string.IsNullOrEmpty(grdImportDashboard.ActiveRow.Cells["ValidatedSymbolsRef"].Value.ToString()))
                if (grdImportDashboard.DisplayLayout.Bands[0].Columns.Exists("ValidatedSymbolsRef"))
                {
                    bool isAnyRowselected = false;
                    // Modified by Ankit Gupta on 22 Oct, 2014.
                    // http://jira.nirvanasolutions.com:8080/browse/CHMW-1667
                    // Refine message box prompt, in Import Dashboard UI.
                    StringBuilder message = new StringBuilder();
                    foreach (UltraGridRow ultraGridRow in grdImportDashboard.Rows)
                    {
                        if (ultraGridRow.Cells.Exists("Select") && ultraGridRow.Cells["Select"].Value.ToString() == "True")
                        {
                            if (ultraGridRow.Cells.Exists("Comments") && ultraGridRow.Cells["Comments"].Text.Equals("No Data"))
                            {
                                if (ultraGridRow.Cells.Exists("Task") && ultraGridRow.Cells.Exists("FileName"))
                                {
                                    message.Append(ultraGridRow.Cells["Task"].Text.ToString() + " " + ultraGridRow.Cells["FileName"].Text.ToString() + ", ");
                                }
                            }
                            else
                            {
                                isAnyRowselected = true;
                                string fileName = GetFileNameFromRef(ultraGridRow.Cells["ValidatedSymbolsRef"].Value.ToString());
                                string thirdParty = ultraGridRow.Cells["ThirdPartyType"].Value.ToString();

                                if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                                {
                                    if (dictReportData.ContainsKey("FileName"))
                                    {
                                        dictReportData["FileName"] = dictReportData["FileName"].ToString() + Seperators.SEPERATOR_8 + fileName + Seperators.SEPERATOR_6 + thirdParty;
                                    }
                                    else
                                    {
                                        dictReportData.Add("FileName", fileName + Seperators.SEPERATOR_6 + thirdParty);
                                    }

                                    //if (!string.IsNullOrEmpty(ultraGridRow.Cells["FileType"].Value.ToString()))
                                    //{
                                    //    dictReportData.Add("FileType", ultraGridRow.Cells["FileType"].Value.ToString());
                                    //}

                                    //commenting total symbols as this will be set in symbol validation ui, counting all the symbols from dashboard
                                    //if (!string.IsNullOrEmpty(ultraGridRow.Cells["TotalSymbols"].Value.ToString()))
                                    //{
                                    //    totalSymbols += Convert.ToInt32(ultraGridRow.Cells["TotalSymbols"].Value.ToString());
                                    //}
                                    if (!string.IsNullOrEmpty(ultraGridRow.Cells["NonValidatedSymbols"].Value.ToString()))
                                    {
                                        faildValidations += Convert.ToInt32(ultraGridRow.Cells["NonValidatedSymbols"].Value.ToString());
                                    }
                                    //if (!string.IsNullOrEmpty(ultraGridRow.Cells["ValidatedSymbols"].Value.ToString()))
                                    //{
                                    //    dictReportData.Add("ValidatedSymbols", ultraGridRow.Cells["ValidatedSymbols"].Value.ToString());
                                    //}
                                    if (!string.IsNullOrEmpty(ultraGridRow.Cells["StartTime"].Value.ToString()))
                                    {
                                        if (dtStartDate.Equals(DateTime.MinValue) || dtStartDate.CompareTo(Convert.ToDateTime(ultraGridRow.Cells["StartTime"].Value.ToString())) > 0)
                                            dtStartDate = Convert.ToDateTime(ultraGridRow.Cells["StartTime"].Value.ToString());
                                    }
                                    if (!string.IsNullOrEmpty(ultraGridRow.Cells["EndTime"].Value.ToString()))
                                    {
                                        if (dtEndDate.Equals(DateTime.MinValue) || dtStartDate.CompareTo(Convert.ToDateTime(ultraGridRow.Cells["EndTime"].Value.ToString())) < 0)
                                            dtEndDate = Convert.ToDateTime(ultraGridRow.Cells["EndTime"].Value.ToString());
                                    }
                                    //if (!string.IsNullOrEmpty(ultraGridRow.Cells["AccountCount"].Value.ToString()))
                                    //{
                                    //    dictReportData.Add("AccountCount", ultraGridRow.Cells["AccountCount"].Value.ToString());
                                    //}
                                    //if (!string.IsNullOrEmpty(ultraGridRow.Cells["SecApproveFailedCount"].Value.ToString()))
                                    //{
                                    //    dictReportData.Add("PendingSymbols", ultraGridRow.Cells["SecApproveFailedCount"].Value.ToString());
                                    //}
                                }
                            }
                        }
                    }
                    if (!isAnyRowselected && message.Length == 0)
                    {
                        MessageBox.Show("Please select at least one Batch for Validation", "Symbol report", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (message.Length > 0)
                    {
                        MessageBox.Show("Following batch/batches does not contain data:" + System.Environment.NewLine + message.ToString().Substring(0, message.Length - 2) + "." + System.Environment.NewLine + System.Environment.NewLine + "Data will only be shown for valid batches.", "Symbol report", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (!isAnyRowselected)
                        {
                            return;
                        }
                    }
                    //dictReportData.Add("TotalSymbols", totalSymbols);
                    dictReportData.Add("NonValidatedSymbols", faildValidations);
                    dictReportData.Add("StartDate", dtStartDate);
                    dictReportData.Add("EndDate", dtEndDate);

                    if (_frmSymbolManagement == null || _frmSymbolManagement.IsDisposed)
                    {
                        _ctrlsymbolManagement = new ctrlSymbolManagement();
                        _frmSymbolManagement = new Form();
                        _frmSymbolManagement.ShowIcon = false;
                        _frmSymbolManagement.Text = "Symbol Validation and Management";
                        _frmSymbolManagement.StartPosition = FormStartPosition.Manual;
                        SetThemeAtDynamicForm(_frmSymbolManagement, _ctrlsymbolManagement);
                        _frmSymbolManagement.Size = new System.Drawing.Size(700, 500);
                        _frmSymbolManagement.MinimumSize = _frmSymbolManagement.Size;
                        _ctrlsymbolManagement.autoReRunSymbolValidation += new EventHandler(CtrlSymbolManagement_AutoReRunSymbolValidation);
                        _frmSymbolManagement.FormClosed += _frmSymbolManagement_FormClosed;
                        _frmSymbolManagement.FormClosing += _frmSymbolManagement_FormClosing;
                    }
                    _ctrlsymbolManagement.secMasterService = _securityMaster;
                    _ctrlsymbolManagement.FillData(dictReportData);

                    CustomThemeHelper.SetThemeProperties(_frmSymbolManagement, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_IMPORT_DASHBOARD);
                    _frmSymbolManagement.Show();
                    BringFormToFront(_frmSymbolManagement);
                }
                else
                {
                    MessageBox.Show("No data available to show", "Symbol report", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        void _frmSymbolManagement_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_ctrlsymbolManagement._isApprovingInProgess && _securityMaster != null && _securityMaster.IsConnected)
                {
                    DialogResult dialogResult = MessageBox.Show("Some securities still in approval process.\nAre you sure to Exit ?", "Alert", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        e.Cancel = false;
                    }
                    else
                    {
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

        private void formUploadNow_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _frmUploadNow = null;
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
        /// Get Runupload object from ref data of dashboard row
        /// </summary>
        /// <param name="gridRow"></param>
        /// <returns></returns>
        private RunUpload GetRunUploadFromGridRow(UltraGridRow gridRow)
        {
            RunUpload runUpload = null;
            try
            {
                if (gridRow != null)
                {
                    if (gridRow.Cells.Exists("RunUploadRef") && gridRow.Cells["RunUploadRef"].Value != null)
                    {
                        string fileName = GetFileNameFromRef(gridRow.Cells["RunUploadRef"].Value.ToString());
                        if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                        {
                            using (FileStream fs = File.OpenRead(fileName))
                            {
                                XmlSerializer serializer = new XmlSerializer(typeof(RunUpload));
                                runUpload = (RunUpload)serializer.Deserialize(fs);
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
            return runUpload;
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
                        taskResult.ExecutionInfo = DeepCopyHelper.Clone(TaskExecutionCache.Instance.GetExecutionInfo("Import_-1"));
                        if (!string.IsNullOrEmpty(gridRow.Cells["Task"].Value.ToString()))
                        {
                            if (dr.Row.Table.Columns.Contains("RawFilePath"))
                            {
                                taskResult.ExecutionInfo.ExecutionName = gridRow.Cells["Task"].Value.ToString() + Seperators.SEPERATOR_6 + Path.GetFileName(dr.Row["RawFilePath"].ToString());
                            }
                            else
                                taskResult.ExecutionInfo.ExecutionName = gridRow.Cells["Task"].Value.ToString();
                        }

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
        /// <summary>
        /// sets theme at the form if the `
        /// </summary>
        /// <param name="dynamicForm"></param>
        /// <param name="control"></param>
        private void SetThemeAtDynamicForm(Form dynamicForm, Object control)
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
                if (control as UserControl == null)
                {
                    UltraGrid grid = control as UltraGrid;
                    grid.Dock = DockStyle.Fill;
                    dynamicForm.Controls.Add(grid);
                }
                else
                {
                    UserControl userControl = control as UserControl;
                    userControl.Dock = DockStyle.Fill;
                    dynamicForm.Controls.Add(userControl);
                }
                dynamicForm.Owner = this.FindForm();
                dynamicForm.ShowInTaskbar = false;
                dynamicForm.Size = new System.Drawing.Size(1107, 630);
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDataMapping_Click(object sender, EventArgs e)
        {
            try
            {
                launchForm = ImportManager.Instance.GetLaunchForm();
                if (launchForm != null)
                {
                    ListEventAargs args = new ListEventAargs();
                    args.listOfValues.Add(ApplicationConstants.CONST_DataMapping_UI.ToString());
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
                    checkedRowList.Clear();
                    if (grdImportDashboard != null && grdImportDashboard.Rows != null)
                    {
                        _rowScrollPos = grdImportDashboard.ActiveRowScrollRegion.ScrollPosition;
                        _colScrollPos = grdImportDashboard.ActiveColScrollRegion.Position;
                        foreach (UltraGridRow row in grdImportDashboard.Rows)
                        {
                            if (row.IsActiveRow)
                            {
                                _activeRowIndex = row.Index;

                            }
                            if (row.Cells.Exists("Select") && row.Cells["Select"].Text.ToString() == "True" && row.Cells.Exists("DashboardFile"))
                            {
                                checkedRowList.Add(row.Cells["DashboardFile"].Value.ToString());
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
        /// Added by: Aman Seth,  14 Jul 2014
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

                    if (grdImportDashboard != null && grdImportDashboard.Rows != null)
                    {
                        if (_activeRowIndex != int.MinValue && grdImportDashboard.Rows.Count > _activeRowIndex)
                        {
                            grdImportDashboard.Rows[_activeRowIndex].Activated = true;
                            _activeRowIndex = int.MinValue;
                        }
                        //CHMW-2204	CLONE -import data dashboard keeps refreshing to the top
                        grdImportDashboard.ActiveRowScrollRegion.ScrollPosition = _rowScrollPos;
                        grdImportDashboard.ActiveColScrollRegion.Position = _colScrollPos;
                        if (checkedRowList != null && checkedRowList.Count > 0)
                        {
                            foreach (UltraGridRow row in grdImportDashboard.Rows)
                            {
                                if (row.Cells.Exists("DashboardFile") && row.Cells.Exists("Select") && checkedRowList.Contains(row.Cells["DashboardFile"].Value.ToString()))
                                {
                                    row.Cells["Select"].Value = true;
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

        /// <summary>
        ///  Added by: Aman seth
        ///  Purpose : Import all the checked batches into application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO : Need to refactor isPartialImport logic for all handlers.
                //PositionAndTransactionHandler._isPartialImport = true;
                //Modified By Faisal Shah
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-2100
                List<string> listCell = new List<string>();
                StringBuilder errmsg = new StringBuilder();
                StringBuilder btchRunningErr = new StringBuilder();
                StringBuilder fileRetrivalErr = new StringBuilder();
                StringBuilder dataAlreadyImported = new StringBuilder();
                bool isReImportRequired = false;
                foreach (UltraGridRow row in grdImportDashboard.Rows)
                {
                    if (row.Cells.Exists("Select") && row.Cells["Select"].Value.ToString() == "True" && row.Cells.Exists("btnImportInApp"))
                    {
                        UltraGridCell cell = row.Cells["btnImportInApp"];
                        if (cell.Row.Cells.Exists("Status") && (cell.Row.Cells["Status"].Text.Equals("Running") || (cell.Row.Cells["Status"].Text.Equals("Importing"))))
                        {
                            btchRunningErr.Append(row.Cells["Task"].Value.ToString()).Append(" ,");
                            continue;
                        }
                        if (row.Cells.Exists("Comments") && row.Cells["Comments"].Text.Equals("No Data"))
                        {
                            continue;
                        }
                        if (cell.Row.Cells.Exists("RetrievalStatus") && cell.Row.Cells["RetrievalStatus"].Value.ToString().Equals("Success"))
                        {
                            if (cell.Row.Cells.Exists("ImportStatus") && (cell.Row.Cells["ImportStatus"].Text.ToString().Equals("Success") || cell.Row.Cells["ImportStatus"].Text.ToString().Equals("Partial Success")))
                            {
                                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1332
                                //dataAlreadyImported.Append(row.Cells["Task"].Value.ToString()).Append(", ");
                                dataAlreadyImported.Append(row.Cells["Task"].Value.ToString()).Append(" - ").Append(row.Cells["FileName"].Value.ToString()).Append(", ");
                            }
                            listCell.Add(cell.Row.Cells["DashboardFile"].Value.ToString());
                        }
                        else
                        {
                            fileRetrivalErr.Append(row.Cells["Task"].Value.ToString()).Append(" ,");
                        }
                    }
                }

                if (btchRunningErr.Length > 0)
                {
                    errmsg.Append(btchRunningErr.ToString().Substring(0, btchRunningErr.Length - 1)).Append(" is/are running/importing, wait for the completion of batches.").Append(Environment.NewLine);
                }
                if (fileRetrivalErr.Length > 0)
                {
                    errmsg.Append("File retrieval process failed for ").Append(fileRetrivalErr.ToString().Substring(0, fileRetrivalErr.Length - 1)).Append(Environment.NewLine);
                }
                if (errmsg.Length > 0)
                {
                    MessageBox.Show(errmsg.ToString() + ".", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (dataAlreadyImported.Length > 0)
                {
                    if (MessageBox.Show("Data is already imported into the application for:" + System.Environment.NewLine + dataAlreadyImported.ToString().Substring(0, dataAlreadyImported.Length - 2) + System.Environment.NewLine + "Do you want to proceed with importing again?", "Import", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        isReImportRequired = true;
                    //dlgResult = ConfirmationMessageBox.DisplayYesNo("Data is already imported into the application for:" + System.Environment.NewLine + dataAlreadyImported.ToString().Substring(0, dataAlreadyImported.Length - 2) + System.Environment.NewLine + "Do you want to proceed with importing again?", "Import");
                }

                if (listCell != null && listCell.Count > 0)
                {
                    List<Tuple<RunUpload, TaskResult>> tupleList = new List<Tuple<RunUpload, TaskResult>>();
                    foreach (UltraGridRow row in grdImportDashboard.Rows)
                    {

                        if (row.Cells.Exists("DashboardFile") && listCell.Contains(row.Cells["DashboardFile"].Value.ToString()))
                        {

                            if (dataAlreadyImported.ToString().Contains(row.Cells["Task"].Value.ToString() + " - " + row.Cells["FileName"].Value.ToString()) && isReImportRequired)
                            {
                                continue;
                            }
                            row.Cells["StartTime"].Value = DateTime.Now;
                            row.Cells["Status"].Value = "Importing";
                            //Modified By amit
                            //http://jira.nirvanasolutions.com:8080/browse/CHMW-3478
                            row.Cells["ImportStatus"].Value = String.Empty;

                            RunUpload runUpload = GetRunUploadFromGridRow(row);

                            TaskResult taskResult = GetTaskResultGridRow(row);
                            Tuple<RunUpload, TaskResult> tuple = new Tuple<RunUpload, TaskResult>(runUpload, taskResult);
                            tupleList.Add(tuple);
                        }

                    }
                    if (tupleList.Count > 0)
                    {
                        //modified by amit on 20/04/2015
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-3400
                        if (bgImportIntoApp.IsBusy)
                        {
                            MessageBox.Show(this, "Please wait while previous import is complete.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else
                        {
                            bgImportIntoApp.RunWorkerAsync(tupleList);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a Valid Batch to be Imported", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        ///  Added by: Aman seth
        ///  Purpose : Import all the checked batches into application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgImportIntoApp_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                List<Tuple<RunUpload, TaskResult>> tupleList = e.Argument as List<Tuple<RunUpload, TaskResult>>;
                gridImportIntoAppClick(tupleList);
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

        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdImportDashboard != null && grdImportDashboard.DisplayLayout.Bands[0].Columns.Count > 0)
                {
                    GetGridColumnLayout(grdImportDashboard);
                }
                SaveImportReportLayout();
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

        private void grdImportDashboard_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip1.Show();
            }
        }

        /// <summary>
        /// Returns the Layout as read from the Xml
        /// </summary>
        /// <returns></returns>
        private static ImportDashboardLayout GetImportDashboardLayout()
        {
            _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            _importDashboardLayoutDirectoryPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID;
            _importDashboardLayoutFilePath = _importDashboardLayoutDirectoryPath + @"\ImportDashboardLayout.xml";

            ImportDashboardLayout importLayout = new ImportDashboardLayout();
            try
            {
                if (!Directory.Exists(_importDashboardLayoutDirectoryPath))
                {
                    Directory.CreateDirectory(_importDashboardLayoutDirectoryPath);
                }
                if (File.Exists(_importDashboardLayoutFilePath))
                {
                    using (FileStream fs = File.OpenRead(_importDashboardLayoutFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ImportDashboardLayout));
                        importLayout = (ImportDashboardLayout)serializer.Deserialize(fs);
                    }
                }

                _importDashboardLayout = importLayout;
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

            return importLayout;
        }

        /// <summary>
        /// Function Writes to the XMl the Layout(Columns and associated Properties) as User is using
        /// </summary>
        public static void SaveImportReportLayout()
        {
            try
            {

                using (XmlTextWriter writer = new XmlTextWriter(_importDashboardLayoutFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(ImportDashboardLayout));
                    serializer.Serialize(writer, _importDashboardLayout);

                    writer.Flush();
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

        /// <summary>
        /// Function Returns a list of Columns of Grid grdReport with Properties as set.
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public void GetGridColumnLayout(UltraGrid grid)
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
                    ImportDashboardLayout.ImportDashboardDataColumns = listGridCols;
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

        private void grdImportDashboardSetColumns(List<string> gridColumns)
        {
            try
            {
                if (ImportDashboardLayout.ImportDashboardDataColumns.Count > 0)
                {
                    List<ColumnData> listColData = ImportDashboardLayout.ImportDashboardDataColumns;
                    SetGridColumnLayout(grdImportDashboard, listColData);
                    foreach (string column in gridColumns)
                    {
                        if (grdImportDashboard.DisplayLayout.Bands[0].Columns.Exists(column))
                        {
                            grdImportDashboard.DisplayLayout.Bands[0].Columns[column].Hidden = false;
                        }
                    }
                }
                else
                {
                    LoadColumns(grdImportDashboard);
                }
                SetGridDataColumnCustomizations(grdImportDashboard);
                //SetColumnSummaries(grdData);
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
        /// Function Sets the Grid Layout as it reads from the List of Columns Layout which are Columns read from XML
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="listColData"></param>
        public static void SetGridColumnLayout(UltraGrid grid, List<ColumnData> listColData)
        {
            List<ColumnData> listSortedGridCols = new List<ColumnData>();
            UltraGridBand band = grid.DisplayLayout.Bands[0];
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
                        gridCol.ExcludeFromColumnChooser = colData.ExcludeFromColumnChooser;
                        gridCol.Header.Fixed = colData.Fixed;
                        gridCol.SortIndicator = colData.SortIndicator;
                        gridCol.CellActivation = Activation.NoEdit;

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

        private void LoadColumns(UltraGrid grid)
        {
            try
            {

                List<string> colAll = GetAllDisplayableColumns();
                List<string> colDefault = GetAllDefaultColumns();
                List<string> colVisible = GetAllDefaultColumns();

                if (colVisible.Count < 1) // PrefFile Has No Columns
                {
                    colVisible.AddRange(colDefault);
                }

                ColumnsCollection gridColumns = grid.DisplayLayout.Bands[0].Columns;

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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private List<string> GetAllDisplayableColumns()
        {
            List<string> colAll = new List<string>();
            try
            {
                List<string> colDefault = GetAllDefaultColumns();
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return colAll;
        }

        private List<string> GetAllDefaultColumns()
        {
            List<string> colDefault = new List<string>();
            try
            {
                string[] array = { "Select", "Task", "Status", "StartTime", "FileName", "ThirdPartyType", "FileType", "RetrievalStatus", "FileMetaData", "FileSize", "btnDownLoadFile", "Comments", "SymbolValidation", "ImportStatus", "btnView", "btnReRunUpload", "btnReRunSymbolValidation", "btnManualUpload", "btnImportInApp", "ShowErrorReport", "UserName", "ImportTagReport", "ImportStatusReport" };
                List<string> lstColumns = new List<string>(array);

                SetupGridColumns(band, lstColumns);

                foreach (UltraGridColumn col in band.Columns)
                {
                    if (!lstColumns.Contains(col.Key))
                    {
                        col.Hidden = true;
                    }
                    //following line auto adjust width of columns of ultragrid according to text size of header.
                    col.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All);
                }
                return lstColumns;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return colDefault;
        }


        private void SetGridDataColumnCustomizations(UltraGrid Grid)
        {
            try
            {
                band = Grid.DisplayLayout.Bands[0];
                //int visiblePosition = 0;
                if (band.Columns.Exists("Select"))
                {
                    UltraGridColumn colSelect = band.Columns["Select"];
                    colSelect.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    //colSelect.Width = 50;
                    colSelect.Key = "Select";
                    colSelect.Header.Caption = string.Empty;
                    colSelect.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                    colSelect.Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                    colSelect.Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center;
                    //colSelect.Header.VisiblePosition = visiblePosition++;
                    colSelect.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colSelect.CellActivation = Activation.AllowEdit;
                    colSelect.DataType = typeof(bool);
                    //colSelect.Header.VisiblePosition = 1;
                }
                if (band.Columns.Exists("Task"))
                {
                    UltraGridColumn colFormatName = band.Columns["Task"];
                    colFormatName.Header.Caption = "Format Name";
                    colFormatName.Header.Appearance.TextHAlign = HAlign.Center;
                    //colFormatName.Header.VisiblePosition = visiblePosition++;
                    colFormatName.CellActivation = Activation.NoEdit;
                }
                if (band.Columns.Exists("Status"))
                {
                    UltraGridColumn colBatchStatus = band.Columns["Status"];
                    colBatchStatus.Header.Caption = "Batch Status";
                    colBatchStatus.Header.Appearance.TextHAlign = HAlign.Center;
                    //colBatchStatus.Header.VisiblePosition = visiblePosition++;
                    colBatchStatus.CellActivation = Activation.NoEdit;
                }
                if (band.Columns.Exists("StartTime"))
                {
                    UltraGridColumn colRunDate = band.Columns["StartTime"];
                    colRunDate.Header.Caption = "Run Date";
                    colRunDate.Header.Appearance.TextHAlign = HAlign.Center;
                    //colRunDate.Header.VisiblePosition = visiblePosition++;
                    colRunDate.CellActivation = Activation.NoEdit;
                }
                if (band.Columns.Exists("FileName"))
                {
                    UltraGridColumn colFileName = band.Columns["FileName"];
                    colFileName.Header.Caption = "File Name";
                    colFileName.Header.Appearance.TextHAlign = HAlign.Center;
                    //colFileName.Header.VisiblePosition = visiblePosition++;
                    colFileName.CellActivation = Activation.NoEdit;
                }
                if (band.Columns.Exists("ThirdPartyType"))
                {
                    UltraGridColumn colThirdParty = band.Columns["ThirdPartyType"];
                    colThirdParty.Header.Caption = "Third Party";
                    colThirdParty.Header.Appearance.TextHAlign = HAlign.Center;
                    //colThirdParty.Header.VisiblePosition = visiblePosition++;
                    colThirdParty.CellActivation = Activation.NoEdit;
                }

                if (band.Columns.Exists("FileType"))
                {
                    UltraGridColumn colFileType = band.Columns["FileType"];
                    colFileType.Header.Caption = "File Type";
                    colFileType.Header.Appearance.TextHAlign = HAlign.Center;
                    //colFileType.Header.VisiblePosition = visiblePosition++;
                    colFileType.CellActivation = Activation.NoEdit;
                }
                if (band.Columns.Exists("RetrievalStatus"))
                {
                    UltraGridColumn colStatusRetrieval = band.Columns["RetrievalStatus"];
                    colStatusRetrieval.Header.Caption = "Status - Retrieval";
                    colStatusRetrieval.Header.Appearance.TextHAlign = HAlign.Center;
                    //colStatusRetrieval.Header.VisiblePosition = visiblePosition++;
                    colStatusRetrieval.CellActivation = Activation.NoEdit;
                }

                if (band.Columns.Exists("FileMetaData"))
                {
                    UltraGridColumn colRows = band.Columns["FileMetaData"];
                    colRows.Header.Caption = "Rows";
                    colRows.Header.Appearance.TextHAlign = HAlign.Center;
                    //colRows.Header.VisiblePosition = visiblePosition++;
                    colRows.CellActivation = Activation.NoEdit;
                }
                if (band.Columns.Exists("FileSize"))
                {
                    UltraGridColumn colFileSize = band.Columns["FileSize"];
                    colFileSize.Header.Caption = "File Size";
                    colFileSize.Header.Appearance.TextHAlign = HAlign.Center;
                    //colFileSize.Header.VisiblePosition = visiblePosition++;
                    colFileSize.CellActivation = Activation.NoEdit;
                }
                if (band.Columns.Exists("Comments"))
                {
                    UltraGridColumn colComments = band.Columns["Comments"];
                    colComments.Header.Caption = "Comments";
                    colComments.Header.Appearance.TextHAlign = HAlign.Center;
                    //colComments.Header.VisiblePosition = visiblePosition++;
                    colComments.CellActivation = Activation.NoEdit;
                }

                if (band.Columns.Exists("btnView"))
                {
                    UltraGridColumn colBtnView = band.Columns["btnView"];
                    colBtnView.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    colBtnView.Header.Appearance.TextHAlign = HAlign.Center;
                    //colBtnView.Width = 50;
                    colBtnView.Header.Caption = "View";
                    colBtnView.NullText = "View";
                    //colBtnView.Header.VisiblePosition = visiblePosition++;
                    colBtnView.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                }

                if (band.Columns.Exists("SymbolValidation"))
                {
                    UltraGridColumn colSymbolValidation = band.Columns["SymbolValidation"];
                    colSymbolValidation.Header.Caption = "Symbol Validation";
                    colSymbolValidation.Header.Appearance.TextHAlign = HAlign.Center;
                    //colSymbolValidation.Header.VisiblePosition = visiblePosition++;
                    colSymbolValidation.CellActivation = Activation.NoEdit;
                }

                if (band.Columns.Exists("btnReRunUpload"))
                {
                    UltraGridColumn colBtnReRunupload = band.Columns["btnReRunUpload"];
                    colBtnReRunupload.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    colBtnReRunupload.Header.Appearance.TextHAlign = HAlign.Center;
                    //colBtnReRunupload.Width = 120;
                    colBtnReRunupload.Header.Caption = "Re-Run Upload";
                    colBtnReRunupload.NullText = "Re-Run Upload";
                    //colBtnReRunupload.Header.VisiblePosition = visiblePosition++;
                    colBtnReRunupload.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                }

                if (band.Columns.Exists("btnReRunSymbolValidation"))
                {
                    UltraGridColumn colReRunSymbolValidation = band.Columns["btnReRunSymbolValidation"];
                    colReRunSymbolValidation.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    //colReRunSymbolValidation.Width = 170;
                    colReRunSymbolValidation.Header.Caption = "Re-Run Symbol Validation";
                    colReRunSymbolValidation.Header.Appearance.TextHAlign = HAlign.Center;
                    colReRunSymbolValidation.NullText = "Re-Run Symbol Validation";
                    //colReRunSymbolValidation.Header.VisiblePosition = visiblePosition++;
                    colReRunSymbolValidation.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                }

                if (band.Columns.Exists("btnManualUpload"))
                {
                    UltraGridColumn colBtnManualUpload = band.Columns["btnManualUpload"];
                    colBtnManualUpload.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    //colBtnManualUpload.Width = 150;
                    colBtnManualUpload.Header.Caption = "Manual Upload";
                    colBtnManualUpload.Header.Appearance.TextHAlign = HAlign.Center;
                    colBtnManualUpload.NullText = "Manual Upload";
                    //colBtnManualUpload.Header.VisiblePosition = visiblePosition++;
                    colBtnManualUpload.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                }

                if (band.Columns.Exists("btnImportInApp"))
                {
                    UltraGridColumn colBtnImportInApp = band.Columns["btnImportInApp"];
                    colBtnImportInApp.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    //colBtnImportInApp.Width = 150;
                    colBtnImportInApp.Header.Caption = "Import Into Application";
                    colBtnImportInApp.Header.Appearance.TextHAlign = HAlign.Center;
                    colBtnImportInApp.NullText = "Import Into Application";
                    //colBtnImportInApp.Header.VisiblePosition = visiblePosition++;
                    colBtnImportInApp.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                }
                if (band.Columns.Exists("btnDownloadFile"))
                {
                    UltraGridColumn colBtnDownloadFile = band.Columns["btnDownloadFile"];
                    colBtnDownloadFile.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    colBtnDownloadFile.Header.Caption = "Download File";
                    colBtnDownloadFile.Header.Appearance.TextHAlign = HAlign.Center;
                    colBtnDownloadFile.NullText = "Download File";
                    colBtnDownloadFile.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colBtnDownloadFile.Hidden = false;
                }
                if (band.Columns.Exists("ImportStatus"))
                {
                    UltraGridColumn colBtnImportStatus = band.Columns["ImportStatus"];
                    colBtnImportStatus.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    colBtnImportStatus.Header.Appearance.TextHAlign = HAlign.Center;
                    //colBtnImportStatus.Width = 50;
                    //colBtnImportStatus.Header.VisiblePosition = visiblePosition++;
                    colBtnImportStatus.Header.Caption = "Import Status";
                    //colBtnImportStatus.NullText = "Import Status";
                    colBtnImportStatus.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                }
                if (band.Columns.Exists("ImportTagReport"))
                {
                    //CHMW-2305 [Implementation] import dashboard updates-Part 2
                    UltraGridColumn colBtnImportTag = band.Columns["ImportTagReport"];
                    colBtnImportTag.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    colBtnImportTag.Header.Appearance.TextHAlign = HAlign.Center;
                    colBtnImportTag.Header.Caption = "Import Tag Report";
                    colBtnImportTag.NullText = "View Report";
                    colBtnImportTag.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                }
                if (band.Columns.Exists("ImportStatusReport"))
                {

                    //CHMW-2305 [Implementation] import dashboard updates-Part 2
                    UltraGridColumn colBtnImportStatusReport = band.Columns["ImportStatusReport"];
                    colBtnImportStatusReport.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    colBtnImportStatusReport.Header.Caption = "Import Status Report";
                    colBtnImportStatusReport.Header.Appearance.TextHAlign = HAlign.Center;
                    colBtnImportStatusReport.NullText = "View Report";
                    colBtnImportStatusReport.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                }
                if (band.Columns.Exists("ShowErrorReport"))
                {
                    UltraGridColumn colBtnErrorReport = band.Columns["ShowErrorReport"];
                    colBtnErrorReport.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    //colBtnErrorReport.Width = 50;
                    //colBtnErrorReport.Header.VisiblePosition = visiblePosition++;
                    colBtnErrorReport.Header.Caption = "Error Report";
                    colBtnErrorReport.Header.Appearance.TextHAlign = HAlign.Center;
                    colBtnErrorReport.NullText = "Show";
                    colBtnErrorReport.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                }
                //bind User Name-ID on grid fr UserName column
                Dictionary<int, string> usersNameID = CachedDataManager.GetInstance.GetAllUsersName();
                Infragistics.Win.ValueList User = new Infragistics.Win.ValueList();
                foreach (var user in usersNameID)
                {
                    ValueListItem item = new ValueListItem();
                    item.DisplayText = user.Value;
                    item.DataValue = user.Key;
                    User.ValueListItems.Add(item);
                }

                if (band.Columns.Exists("UserName"))
                {
                    UltraGridColumn colUserName = band.Columns["UserName"];
                    colUserName.ValueList = User;
                    colUserName.Header.Appearance.TextHAlign = HAlign.Center;
                    colUserName.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                    colUserName.CellActivation = Activation.NoEdit;
                    //colUserName.Width = 50;
                    //colUserName.Header.VisiblePosition = visiblePosition++;
                    colUserName.Header.Caption = "User Name";
                    band.Columns["Select"].AllowRowFiltering = DefaultableBoolean.False;
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

        private void AutoReRunSymbolValidation()
        {
            foreach (UltraGridRow ultraGridRow in grdImportDashboard.Rows)
            {
                if (ultraGridRow.Cells.Exists("Select") && ultraGridRow.Cells["Select"].Value.ToString() == "True")
                {
                    UltraGridCell cell = ultraGridRow.Cells["btnReRunSymbolValidation"];
                    ReRunSymbolValidation(new CellEventArgs(cell), true);
                    //grdImportDashboard_ClickCellButton(this, new CellEventArgs(cell));
                }
            }
        }
        //Added by : sachin mishra 13 feb 2015
        //Purpose: Select all should select only filtered rows

        private void grdImportDashboard_BeforeHeaderCheckStateChanged(object sender, BeforeHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                if (grdImportDashboard.Rows.Count > 0)
                {
                    _selectedColumnList.Clear();
                    foreach (UltraGridRow row in grdImportDashboard.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells["Select"].Value) == true)
                        {
                            _selectedColumnList.Add(row);
                        }
                        else
                        {
                            if (_selectedColumnList.Contains(row))
                                _selectedColumnList.Remove(row);
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

        //Added by : sachin mishra 13 feb 2015
        //Purpose: Select all should select only filtered rows
        private void grdImportDashboard_AfterHeaderCheckStateChanged(object sender, AfterHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                if (grdImportDashboard.DisplayLayout.Bands[0].Columns["Select"].Header.Selected != true)
                {

                    _selectedColumnList.Clear();
                }

                if (grdImportDashboard.Rows.Count > 0 && grdImportDashboard.Rows.GetFilteredOutNonGroupByRows() != null)
                {
                    //bool state = Convert.ToBoolean(grdImportDashboard.DisplayLayout.Bands[0].Columns["Select"].ToString());
                    UltraGridRow[] grdrows = grdImportDashboard.Rows.GetFilteredOutNonGroupByRows();
                    if (grdrows.Length > 0 && grdImportDashboard.Rows.Count > 0)
                    {
                        foreach (UltraGridRow row in grdrows)
                        {
                            string state = row.Cells["Select"].Value.ToString();
                            if (state.Equals("True"))
                            {
                                row.Cells["Select"].Value = false;
                            }
                        }
                    }
                    foreach (UltraGridRow row in _selectedColumnList)
                    {
                        row.Cells["Select"].Value = true;
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

        private void grdImportDashboard_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {

            try
            {

                e.Cancel = true;
                if (grdImportDashboard.DataSource != null)
                {
                    (this.FindForm()).AddCustomColumnChooser(this.grdImportDashboard);
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
        /// created by: sachin mishra Purpose- http://jira.nirvanasolutions.com:8080/browse/PRANA-8862
        /// clear All Grid Filters from grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearAllGridFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UltraWinGridUtils.ClearAllGridFilters(grdImportDashboard);
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

        private void grdImportDashboard_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

    }
}

