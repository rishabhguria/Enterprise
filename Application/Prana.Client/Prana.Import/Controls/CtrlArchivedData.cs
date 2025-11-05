using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Import.Classes;
using Prana.LogManager;
using Prana.TaskManagement.Execution;
using Prana.Utilities.ImportExportUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.Import.Controls
{
    public partial class CtrlArchivedData : UserControl
    {
        public static int DefaultEquityAUECID;
        Form _frmReport;
        UltraGrid _grdArchivalReport = null;
        Form _frmUploadedFile;
        static ArchiveDashBoardLayout _archiveDashboardLayout = null;
        static string _archiveDashboardLayoutFilePath = string.Empty;
        static string _archiveDashboardLayoutDirectoryPath = string.Empty;
        static int _userID = int.MinValue;

        public CtrlArchivedData()
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
                btnDelete.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnDelete.ForeColor = System.Drawing.Color.White;
                btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnDelete.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnDelete.UseAppStyling = false;
                btnDelete.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnView.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnView.ForeColor = System.Drawing.Color.White;
                btnView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnView.UseAppStyling = false;
                btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        public static ArchiveDashBoardLayout ArchiveDashboardLayout
        {
            get
            {
                if (_archiveDashboardLayout == null)
                {
                    _archiveDashboardLayout = GetArchiveDashboardLayout();
                }
                return _archiveDashboardLayout;
            }
        }



        /// <summary>
        /// Read the XMl file and load the data to the Ultragrid
        /// </summary>
        /// <param name="filesList">list of xml file paths</param>
        private void LoadData(List<string> filesList)
        {
            try
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
                        ds.ReadXml(filePath, XmlReadMode.ReadSchema);
                        if (ds.Tables.Count > 0)
                        {

                            #region Remove non permitted rows
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
                            #endregion
                            //add the column third party

                            if (!ds.Tables[0].Columns.Contains("ExecutionID"))
                            {
                                ds.Tables[0].Columns.Add("ExecutionID", typeof(string));
                            }
                            //get the third party id from the file name
                            //ds.Tables[0].Rows[0]["ExecutionID"] = GetExecutionID(filePath);

                            //if (!ds.Tables[0].Columns.Contains("Select"))
                            //{
                            //    ds.Tables[0].Columns.Add("Select", typeof(bool));
                            //}

                            if (!ds.Tables[0].Columns.Contains("DashboardFile"))
                            {
                                ds.Tables[0].Columns.Add("DashboardFile", typeof(String));
                                ds.Tables[0].Rows[0]["DashboardFile"] = filePath;
                            }
                            if (!ds.Tables[0].Columns.Contains("ExecutionDate"))
                            {
                                ds.Tables[0].Columns.Add("ExecutionDate", typeof(DateTime));
                                //string rawFileName = string.Empty;
                                //if (ds.Tables[0].Columns.Contains("RawFilePath"))
                                //{
                                //    rawFileName = ds.Tables[0].Rows[0]["RawFilePath"].ToString();
                                //}
                                //ds.Tables[0].Rows[0]["ExecutionDate"] = GetExecutionDate(rawFileName);
                            }

                            //We have to copy data first in another datatable as ds have relationships
                            //FormatName made primary key here
                            DataTable dt = ds.Tables[0].Copy();
                            foreach (DataRow row in dt.Rows)
                            {
                                DateTime StartTime = DateTime.MinValue;
                                DateTime ExecutionDate = DateTime.MinValue;
                                DateTime.TryParse(row["StartTime"].ToString(), out StartTime);
                                DateTime.TryParse(row["ExecutionDate"].ToString(), out ExecutionDate);
                                row["StartTime"] = StartTime.ToString();
                                row["ExecutionDate"] = ExecutionDate.ToString();
                            }
                            if (dt.Columns.Contains("Task"))
                            {
                                DataColumn[] columns = new DataColumn[1];
                                columns[0] = dt.Columns["Task"];
                                dt.PrimaryKey = columns;
                            }

                            DefaultEquityAUECID = int.Parse(ConfigurationManager.AppSettings["DefaultEquityAUECID"]);
                            //DateTime LastBusinessDay1 = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(DateTime.UtcNow.Date, -5, DefaultEquityAUECID);

                            //if (dt.Columns.Contains("ImportStatus") && dt.Columns.Contains("StartTime"))
                            //{
                            //    if ((dt.Rows[0]["ImportStatus"].ToString().Equals("Success") || dt.Rows[0]["ImportStatus"].ToString().Equals("Partial Success")) && Convert.ToDateTime(dt.Rows[0]["StartTime"]) >= LastBusinessDay1)
                            //    {
                            //        dtMain.Merge(dt, true, MissingSchemaAction.Add);
                            //    }
                            //    else if ((dt.Rows[0]["ImportStatus"].ToString().Equals("Failure") || string.IsNullOrEmpty(dt.Rows[0]["ImportStatus"].ToString())))
                            //    {
                            //        dtMain.Merge(dt, true, MissingSchemaAction.Add);
                            //    }
                            //}
                            //else
                            //{
                            //    //schema for each datatable may be different because of symbol validation
                            dtMain.Merge(dt, true, MissingSchemaAction.Add);
                            //}

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
                    //_dtLastRundate = GetLastRunDate(dtMain);
                    //txtLastRunDate.Text = _dtLastRundate.ToShortDateString();
                    //txtLastRunTime.Text = _dtLastRundate.ToLongTimeString();
                }

                if (dtMain.Rows.Count > 0)
                {
                    grdArchiveDashBoard.DataSource = dtMain;
                }
                else
                {
                    grdArchiveDashBoard.DataSource = new DataTable();
                }
                //setDefaultCheckBox();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("The specified XML file could not be found", "Import File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void grdArchiveDashBoard_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                //add filters on grid if there are rows in grid
                //if (grdImportDashboard.Rows.Count > 0)
                //{
                //    UltraWinGridUtils.EnableFixedFilterRow(e);
                //}
                UltraGridBand band = e.Layout.Bands[0];
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
                e.Layout.Bands[0].Override.RowSelectors = DefaultableBoolean.True;
                e.Layout.Bands[0].Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;

                band.Override.ButtonStyle = UIElementButtonStyle.Button3D;
                if (!band.Columns.Exists("Select"))
                {
                    band.Columns.Add("Select", "Select");
                }
                UltraGridColumn colSelect = band.Columns["Select"];
                colSelect.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                colSelect.Width = 50;
                colSelect.Header.Caption = "";
                colSelect.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                colSelect.Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                colSelect.Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center;
                colSelect.Header.VisiblePosition = 0;
                colSelect.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                UltraGridLayout gridLayout = grdArchiveDashBoard.DisplayLayout;
                if (!band.Columns.Exists("Select"))
                {
                    band.Columns.Add("Select", "Select");
                }
                if (!band.Columns.Exists("Task"))
                {
                    band.Columns.Add("Task", "Format Name");
                }
                if (!band.Columns.Exists("Status"))
                {
                    band.Columns.Add("Status", "Batch Status");
                }
                if (!band.Columns.Exists("StartTime"))
                {
                    band.Columns.Add("StartTime", "Run Date");
                }
                if (!band.Columns.Exists("ExecutionDate"))
                {
                    band.Columns.Add("ExecutionDate", "ExecutionDate");
                }
                if (!band.Columns.Exists("ThirdPartyType"))
                {
                    band.Columns.Add("ThirdPartyType", "Third Party");
                }
                if (!band.Columns.Exists("FileType"))
                {
                    band.Columns.Add("FileType", "FileType");
                }
                if (!band.Columns.Exists("RetrievalStatus"))
                {
                    band.Columns.Add("RetrievalStatus", "Status - Retrieval");
                }
                if (!band.Columns.Exists("FileMetaData"))
                {
                    band.Columns.Add("FileMetaData", "Rows");
                }

                if (!band.Columns.Exists("FileSize"))
                {
                    band.Columns.Add("FileSize", "File Size");
                }
                if (!band.Columns.Exists("Comments"))
                {
                    band.Columns.Add("Comments", "Comments");
                }
                if (!band.Columns.Exists("btnView"))
                {
                    band.Columns.Add("btnView", "View");
                }
                if (!band.Columns.Exists("btnViewArchived"))
                {
                    band.Columns.Add("btnViewArchived", "View Archived Data");
                }
                if (!band.Columns.Exists("SymbolValidation"))
                {
                    band.Columns.Add("SymbolValidation", "Symbol Validation");
                }
                if (!band.Columns.Exists("ImportStatus"))
                {
                    band.Columns.Add("ImportStatus", "Import Status");
                }
                if (!band.Columns.Exists(OrderFields.PROPERTY_OPTIONPREMIUMADJUSTMENT))
                {
                    band.Columns.Add(OrderFields.PROPERTY_OPTIONPREMIUMADJUSTMENT, OrderFields.CAPTION_OPTION_PREMIUM_ADJUSTMENT);
                }
                SetColumnsForArchiveGrid(grdArchiveDashBoard.DisplayLayout.Bands[0]);
                if (ArchiveDashboardLayout.archiveGridColumns.Count > 0)
                {
                    List<ColumnData> listArchiveColData = ArchiveDashboardLayout.archiveGridColumns;
                    SetGridColumnLayout(grdArchiveDashBoard, listArchiveColData);
                    foreach (string col in AllArchiveGridColumns)
                    {
                        if (gridLayout.Bands[0].Columns.Exists(col))
                        {
                            UltraGridColumn column = gridLayout.Bands[0].Columns[col];
                            column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                        }
                    }
                }
                else
                {
                    //load default layout
                    SetColumnsForArchiveGrid(grdArchiveDashBoard.DisplayLayout.Bands[0]);
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

        private void btnView_Click(object sender, EventArgs e)
        {
            InitializeControl();
        }

        /// <summary>
        /// initialize the control
        /// </summary>
        private void InitializeControl()
        {
            try
            {
                List<string> filesList = new List<string>();
                DateTime startDate = DateTime.Now.AddDays(-5);
                DateTime endDate = DateTime.Now;
                int taskId = 1;
                TaskExecutionManager.Initialize(Application.StartupPath);
                filesList = TaskExecutionManager.GetArchiveStatisticsAsXML(taskId).Keys.ToList();
                LoadData(filesList);
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

        private void grdArchiveDashBoard_ClickCellButton(object sender, CellEventArgs e)
        {
            if (e.Cell.Column.Key == "btnView")
            {
                if (e.Cell.Row.Band.Columns.Exists("Status") && e.Cell.Row.Cells["Status"].Text.Equals("Running"))
                {
                    MessageBox.Show("Batch is running, wait for the completion of batch", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (e.Cell.Row.Band.Columns.Exists("RetrievalStatus") && e.Cell.Row.Cells["RetrievalStatus"].Text.ToString().Equals("Failure"))
                {
                    MessageBox.Show("File missing", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (e.Cell.Row.Band.Columns.Exists("ProcessedFilePath"))
                {
                    string filePath = Application.StartupPath + e.Cell.Row.Cells["ProcessedFilePath"].Value.ToString();
                    //check if file exists at path
                    if (File.Exists(filePath))
                    {
                        DataTable dataSource = FileReaderFactory.GetDataTableFromDifferentFileFormats(filePath);
                        if (dataSource != null)
                        {
                            //if form is not created
                            if (_frmUploadedFile == null || _frmUploadedFile.IsDisposed)
                            {
                                _grdArchivalReport = new UltraGrid();
                                _frmUploadedFile = new Form();
                                _frmUploadedFile.ShowIcon = false;
                                _frmUploadedFile.FormClosed += frmUploadedFile_FormClosed;
                                //set the form and grid properties
                                //_grdArchivalReport.DisplayLayout.Bands[0].ColHeadersVisible = false;

                                SetThemeAtDynamicForm(_frmUploadedFile, _grdArchivalReport);
                                //grdReport.DataSource = dataSource;
                                //disable editing in first row as it will be the hearers
                                if (_grdArchivalReport.DisplayLayout.Rows.Count > 0)
                                    _grdArchivalReport.DisplayLayout.Rows[0].Activation = Activation.NoEdit;
                                _grdArchivalReport.InitializeRow += grdReport_InitializeRow;
                            }
                            else
                            {
                                //else previos form is bring to front
                                _frmUploadedFile.BringToFront();
                            }

                            _frmUploadedFile.Text = e.Cell.Row.Cells["Task"].Value.ToString();
                            _grdArchivalReport.DataSource = dataSource;
                            CustomThemeHelper.SetThemeProperties(_frmUploadedFile, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                            _frmUploadedFile.Show();
                        }
                    }
                    else
                        MessageBox.Show("File does not exist", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("File retrieval process failed", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (e.Cell.Column.Key == "btnViewArchived")
            {
                //if (e.Cell.Row.Band.Columns.Exists("Status") && e.Cell.Row.Cells["Status"].Text.Equals("Running"))
                //{
                //    MessageBox.Show("Batch is running, wait for the completion of batch", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                if (e.Cell.Row.Band.Columns.Exists("RetrievalStatus") && e.Cell.Row.Cells["RetrievalStatus"].Text.ToString().Equals("Failure"))
                {
                    MessageBox.Show("File missing", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (e.Cell.Row.Band.Columns.Exists("ImportDataRef"))
                {

                    string fileName = GetFileNameFromRef(e.Cell.Row.Cells["ImportDataRef"].Value.ToString());
                    Dictionary<string, string> dictReportData = new Dictionary<string, string>();
                    if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                    {
                        dictReportData.Add("FileName", fileName);
                        string archivedFileName = fileName.Replace(".xml", "_NonValidatedTrades.xml");
                        if (File.Exists(archivedFileName))
                        {
                            dictReportData.Add("ArchiveFileName", archivedFileName);
                        }
                        if (e.Cell.Band.Columns.Exists("FileType") && !string.IsNullOrEmpty(e.Cell.Row.Cells["FileType"].Value.ToString()))
                        {
                            dictReportData.Add("FileType", e.Cell.Row.Cells["FileType"].Value.ToString());
                        }
                        if (e.Cell.Band.Columns.Exists("ThirdPartyType") && !string.IsNullOrEmpty(e.Cell.Row.Cells["ThirdPartyType"].Value.ToString()))
                        {
                            dictReportData.Add("ThirdPartyType", e.Cell.Row.Cells["ThirdPartyType"].Value.ToString());
                        }
                        //if (e.Cell.Band.Columns.Exists("TotalSymbols") && !string.IsNullOrEmpty(e.Cell.Row.Cells["TotalSymbols"].Value.ToString()))
                        //{
                        //    dictReportData.Add("TotalSymbols", e.Cell.Row.Cells["TotalSymbols"].Value.ToString());
                        //}
                        //if (e.Cell.Band.Columns.Exists("NonValidatedSymbols") && !string.IsNullOrEmpty(e.Cell.Row.Cells["NonValidatedSymbols"].Value.ToString()))
                        //{
                        //    dictReportData.Add("NonValidatedSymbols", e.Cell.Row.Cells["NonValidatedSymbols"].Value.ToString());
                        //}
                        //if (e.Cell.Band.Columns.Exists("ValidatedSymbols") && !string.IsNullOrEmpty(e.Cell.Row.Cells["ValidatedSymbols"].Value.ToString()))
                        //{
                        //    dictReportData.Add("ValidatedSymbols", e.Cell.Row.Cells["ValidatedSymbols"].Value.ToString());
                        //}
                        if (e.Cell.Band.Columns.Exists("EndTime") && !string.IsNullOrEmpty(e.Cell.Row.Cells["EndTime"].Value.ToString()))
                        {
                            dictReportData.Add("Date", e.Cell.Row.Cells["EndTime"].Value.ToString());
                        }
                        //if (e.Cell.Band.Columns.Exists("AccountCount") && !string.IsNullOrEmpty(e.Cell.Row.Cells["AccountCount"].Value.ToString()))
                        //{
                        //    dictReportData.Add("AccountCount", e.Cell.Row.Cells["AccountCount"].Value.ToString());
                        //}
                        //if (e.Cell.Band.Columns.Exists("SecApproveFailedCount") && !string.IsNullOrEmpty(e.Cell.Row.Cells["SecApproveFailedCount"].Value.ToString()))
                        //{
                        //    dictReportData.Add("PendingSymbols", e.Cell.Row.Cells["SecApproveFailedCount"].Value.ToString());
                        //}

                        if (_frmReport == null)
                        {
                            //ctrlReport = new ctrlImportReport();
                            _frmReport = new Form();
                            _grdArchivalReport = new UltraGrid();
                            _frmReport.ShowIcon = false;
                            _frmReport.FormClosed += frmReport_FormClosed;
                            SetThemeAtDynamicForm(_frmReport, _grdArchivalReport);
                            _grdArchivalReport.InitializeLayout += _grdReport_InitializeLayout;
                            _frmReport.Size = new System.Drawing.Size(700, 500);
                            _frmReport.MinimumSize = _frmReport.Size;
                            _frmReport.StartPosition = FormStartPosition.Manual;
                        }
                        _frmReport.Text = "Import status report - " + e.Cell.Row.Cells["Task"].Value.ToString();
                        //if (ctrlReport == null)
                        //{
                        //    ctrlReport = new ctrlImportReport();
                        //}

                        //ctrlReport.FillReport(dictReportData);
                        CustomThemeHelper.SetThemeProperties(_frmReport, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                        _frmReport.Show();
                        BringFormToFront(_frmReport);
                        DataSet dsData = new DataSet();
                        if (dictReportData.ContainsKey("ArchiveFileName") && !string.IsNullOrWhiteSpace(dictReportData["ArchiveFileName"]))
                        {
                            dsData.ReadXml(dictReportData["ArchiveFileName"]);
                            _grdArchivalReport.DataSource = dsData.Tables[0];
                        }
                        //FillData(dictReportData);
                    }
                }
                else
                    MessageBox.Show("File retrieval process failed", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (e.Cell.Column.Key == "btnView")
            {

            }
        }

        /// <summary>
        /// Make the grid cells read only
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdReport_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                foreach (UltraGridCell cell in e.Row.Cells)
                {
                    cell.Activation = Activation.NoEdit;
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
        /// Dispose the form when that is closed
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

        private void _grdReport_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraWinGridUtils.EnableFixedFilterRow(e);
                #region column choser

                //Set the HeaderCheckBoxVisibility so it will display the CheckBox whenever a CheckEditor is used within the UltraGridColumn 
                _grdArchivalReport.DisplayLayout.Override.HeaderCheckBoxVisibility = HeaderCheckBoxVisibility.WhenUsingCheckEditor;
                //Set the HeaderCheckBoxAlignment so the CheckBox will appear to the Right of the caption. 
                _grdArchivalReport.DisplayLayout.Override.HeaderCheckBoxAlignment = HeaderCheckBoxAlignment.Right;
                //Set the HeaderCheckBoxSynchronization so all rows within the GridBand will be synchronized with the CheckBox 
                _grdArchivalReport.DisplayLayout.Override.HeaderCheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                // Set the RowSelectorHeaderStyle to ColumnChooserButton.
                _grdArchivalReport.DisplayLayout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
                // Enable the RowSelectors. This is necessary because the column chooser
                // button is displayed over the row selectors in the column headers area.
                _grdArchivalReport.DisplayLayout.Override.RowSelectors = DefaultableBoolean.True;

                #endregion

                UltraGridBand grdDataBand = null;
                grdDataBand = _grdArchivalReport.DisplayLayout.Bands[0];
                SetGridColumns(grdDataBand);
                //if (ImportReportLayout.ImportReportColumns.Count > 0)
                //{
                //    List<ColumnData> listColData = ImportReportLayout.ImportReportColumns;
                //    SetGridColumnLayout(_grdReport, listColData);
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
                        //gridCol.ExcludeFromColumnChooser = colData.ExcludeFromColumnChooser;
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
                            foreach (FilterCondition fCond in colData.FilterConditionList)
                            {
                                band.ColumnFilters[colData.Key].FilterConditions.Add(fCond);
                            }
                        }
                    }
                }
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

            // Sorted Columns are returned as they need to be handled after data is binded.
            //  return listSortedGridCols;
        }

        private void SetGridColumns(UltraGridBand gridBand)
        {

            try
            {
                int visiblePosition = 0;

                foreach (UltraGridColumn column in gridBand.Columns)
                {
                    column.CellActivation = Activation.ActivateOnly;
                    column.Hidden = true;
                    column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    column.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.IncludeHeader);
                }

                if (gridBand.Columns.Exists("Select"))
                {
                    UltraGridColumn ColSelect = gridBand.Columns["Select"];
                    ColSelect.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    // ColSelect.
                    ColSelect.Header.Caption = "";
                    ColSelect.DataType = typeof(Boolean);
                    ColSelect.CellActivation = Activation.AllowEdit;
                    ColSelect.Header.VisiblePosition = 0;
                    ColSelect.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                    ColSelect.Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                    ColSelect.Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center;
                    ColSelect.AllowRowFiltering = DefaultableBoolean.False;
                    ColSelect.Hidden = false;
                    ColSelect.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists(ApplicationConstants.CONST_SEC_APPROVED_STATUS))
                {
                    //we are showing Approval Status based on isSecApproved property.
                    UltraGridColumn colApprovalStatus = gridBand.Columns[ApplicationConstants.CONST_SEC_APPROVED_STATUS];
                    colApprovalStatus.Header.Caption = "Security Approval Status";
                    colApprovalStatus.Header.VisiblePosition = visiblePosition++;
                    //colApprovalStatus.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    //  colApprovalStatus.ValueList = _approvalSatus;
                    colApprovalStatus.Hidden = false;
                    colApprovalStatus.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("ValidationStatus"))
                {
                    //we are showing Approval Status based on isSecApproved property.
                    UltraGridColumn colApprovalStatus = gridBand.Columns["ValidationStatus"];
                    colApprovalStatus.Header.Caption = "Trade Validation Status";
                    colApprovalStatus.Header.VisiblePosition = visiblePosition++;
                    //colApprovalStatus.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    //  colApprovalStatus.ValueList = _approvalSatus;
                    colApprovalStatus.Hidden = false;
                    colApprovalStatus.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("Symbol"))
                {
                    UltraGridColumn ColTickerSymbol = gridBand.Columns["Symbol"];
                    ColTickerSymbol.Header.VisiblePosition = visiblePosition++;
                    ColTickerSymbol.Header.Caption = OrderFields.CAPTION_TICKERSYMBOL;
                    ColTickerSymbol.CharacterCasing = CharacterCasing.Upper;
                    ColTickerSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    ColTickerSymbol.NullText = String.Empty;
                    ColTickerSymbol.SortIndicator = SortIndicator.Ascending;
                    ColTickerSymbol.Hidden = false;
                    ColTickerSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("Bloomberg"))
                {
                    UltraGridColumn ColBloombergSymbol = gridBand.Columns["Bloomberg"];
                    ColBloombergSymbol.Header.VisiblePosition = visiblePosition++;
                    ColBloombergSymbol.CharacterCasing = CharacterCasing.Upper;
                    ColBloombergSymbol.Header.Caption = OrderFields.CAPTION_BLOOMBERGSYMBOL;
                    ColBloombergSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    ColBloombergSymbol.NullText = String.Empty;
                    ColBloombergSymbol.Hidden = false;
                    ColBloombergSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("CUSIP"))
                {
                    UltraGridColumn ColCusipSymbol = gridBand.Columns["CUSIP"];
                    ColCusipSymbol.Header.VisiblePosition = visiblePosition++;
                    ColCusipSymbol.CharacterCasing = CharacterCasing.Upper;
                    ColCusipSymbol.Header.Caption = OrderFields.CAPTION_CUSIPSYMBOL;
                    ColCusipSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    ColCusipSymbol.NullText = String.Empty;
                    ColCusipSymbol.Hidden = false;
                    ColCusipSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("FundName"))
                {
                    UltraGridColumn colAccount = gridBand.Columns["FundName"];
                    colAccount.Header.VisiblePosition = visiblePosition++;
                    colAccount.Header.Caption = "Account Name";
                    colAccount.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    colAccount.Hidden = false;
                    colAccount.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("ImportStatus"))
                {
                    UltraGridColumn ColImportStatus = gridBand.Columns["ImportStatus"];
                    ColImportStatus.Header.VisiblePosition = visiblePosition++;
                    ColImportStatus.CharacterCasing = CharacterCasing.Upper;
                    ColImportStatus.Hidden = false;
                    ColImportStatus.Header.Caption = "Import Status";
                    ColImportStatus.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    ColImportStatus.NullText = String.Empty;
                    ColImportStatus.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }
                if (gridBand.Columns.Exists("PositionStartDate"))
                {
                    UltraGridColumn ColPositionStartDate = gridBand.Columns["PositionStartDate"];
                    ColPositionStartDate.Header.VisiblePosition = visiblePosition++;
                    ColPositionStartDate.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColPositionStartDate.Header.Caption = "Position StartDate";
                    ColPositionStartDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColPositionStartDate.Hidden = false;
                    ColPositionStartDate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }


                if (gridBand.Columns.Exists(OrderFields.PROPERTY_ASSET_ID))
                {
                    UltraGridColumn colAsset = gridBand.Columns[OrderFields.PROPERTY_ASSET_ID];
                    colAsset.Header.VisiblePosition = visiblePosition++;
                    colAsset.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colAsset.Header.Caption = OrderFields.CAPTION_ASSET_CLASS;
                    colAsset.ValueList = SecMasterHelper.getInstance().Assets.Clone();
                    colAsset.Hidden = false;
                    colAsset.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }
                if (gridBand.Columns.Exists(OrderFields.PROPERTY_CURRENCYID))
                {
                    UltraGridColumn ColCurrency = gridBand.Columns[OrderFields.PROPERTY_CURRENCYID];
                    ColCurrency.Header.VisiblePosition = visiblePosition++;
                    ColCurrency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    ColCurrency.Header.Caption = "Currency";
                    //we are making clone because there was error we cannot add same valuelist again
                    ColCurrency.ValueList = SecMasterHelper.getInstance().Currencies.Clone();
                    ColCurrency.Hidden = false;
                    ColCurrency.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("SideTagValue"))
                {
                    UltraGridColumn ColSide = gridBand.Columns["SideTagValue"];
                    ColSide.Header.VisiblePosition = visiblePosition++;
                    ColSide.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    ColSide.Header.Caption = "Side";
                    ColSide.ValueList = GetSideValueLists();
                    ColSide.Hidden = false;
                    ColSide.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("NetPosition"))
                {
                    UltraGridColumn ColNetPosition = gridBand.Columns["NetPosition"];
                    ColNetPosition.Header.VisiblePosition = visiblePosition++;
                    ColNetPosition.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColNetPosition.Header.Caption = "Quantity";
                    ColNetPosition.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColNetPosition.Hidden = false;
                    ColNetPosition.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }
                if (gridBand.Columns.Exists("CostBasis"))
                {
                    UltraGridColumn ColCostBasis = gridBand.Columns["CostBasis"];
                    ColCostBasis.Header.VisiblePosition = visiblePosition++;
                    ColCostBasis.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColCostBasis.Header.Caption = OrderFields.CAPTION_AVGPRICE;
                    ColCostBasis.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColCostBasis.Hidden = false;
                    ColCostBasis.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }
                if (gridBand.Columns.Exists("MarkPrice"))
                {
                    UltraGridColumn ColCostBasis = gridBand.Columns["MarkPrice"];
                    ColCostBasis.Header.VisiblePosition = visiblePosition++;
                    ColCostBasis.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColCostBasis.Header.Caption = "Mark Price";
                    ColCostBasis.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColCostBasis.Hidden = false;
                    ColCostBasis.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }
                if (gridBand.Columns.Exists(OrderFields.PROPERTY_COMMISSION))
                {
                    UltraGridColumn ColCommission = gridBand.Columns[OrderFields.PROPERTY_COMMISSION];
                    ColCommission.Header.VisiblePosition = visiblePosition++;
                    ColCommission.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColCommission.Header.Caption = OrderFields.CAPTION_COMMISSION;
                    ColCommission.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColCommission.Hidden = false;
                    ColCommission.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }
                if (gridBand.Columns.Exists(OrderFields.PROPERTY_FEES))
                {
                    UltraGridColumn ColFees = gridBand.Columns[OrderFields.PROPERTY_FEES];
                    ColFees.Header.VisiblePosition = visiblePosition++;
                    ColFees.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColFees.Header.Caption = OrderFields.CAPTION_FEES;
                    ColFees.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColFees.Hidden = false;
                    ColFees.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }
                if (gridBand.Columns.Exists(OrderFields.PROPERTY_STAMPDUTY))
                {
                    UltraGridColumn ColStampDuty = gridBand.Columns[OrderFields.PROPERTY_STAMPDUTY];
                    ColStampDuty.Header.VisiblePosition = visiblePosition++;
                    ColStampDuty.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColStampDuty.Header.Caption = OrderFields.CAPTION_STAMPDUTY;
                    ColStampDuty.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColStampDuty.Hidden = false;
                    ColStampDuty.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }
                if (gridBand.Columns.Exists(OrderFields.PROPERTY_TRANSACTIONLEVY))
                {
                    UltraGridColumn ColTransactionLevy = gridBand.Columns[OrderFields.PROPERTY_TRANSACTIONLEVY];
                    ColTransactionLevy.Header.VisiblePosition = visiblePosition++;
                    ColTransactionLevy.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColTransactionLevy.Header.Caption = OrderFields.CAPTION_TRANSACTIONLEVY;
                    ColTransactionLevy.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColTransactionLevy.Hidden = false;
                    ColTransactionLevy.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }
                if (gridBand.Columns.Exists(OrderFields.PROPERTY_CLEARINGFEE))
                {
                    UltraGridColumn ColClearingFee = gridBand.Columns[OrderFields.PROPERTY_CLEARINGFEE];
                    ColClearingFee.Header.VisiblePosition = visiblePosition++;
                    ColClearingFee.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColClearingFee.Header.Caption = OrderFields.CAPTION_CLEARINGFEE;
                    ColClearingFee.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColClearingFee.Hidden = false;
                    ColClearingFee.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists(OrderFields.PROPERTY_MISCFEES))
                {
                    UltraGridColumn ColMiscFees = gridBand.Columns[OrderFields.PROPERTY_MISCFEES];
                    ColMiscFees.Header.VisiblePosition = visiblePosition++;
                    ColMiscFees.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColMiscFees.Header.Caption = OrderFields.CAPTION_MISCFEES;
                    ColMiscFees.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColMiscFees.Hidden = false;
                    ColMiscFees.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists(OrderFields.PROPERTY_OPTIONPREMIUMADJUSTMENT))
                {
                    UltraGridColumn ColOptionPremiumAdjustMent = gridBand.Columns[OrderFields.PROPERTY_OPTIONPREMIUMADJUSTMENT];
                    ColOptionPremiumAdjustMent.Header.VisiblePosition = visiblePosition++;
                    ColOptionPremiumAdjustMent.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColOptionPremiumAdjustMent.Header.Caption = OrderFields.CAPTION_OPTION_PREMIUM_ADJUSTMENT;
                    ColOptionPremiumAdjustMent.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColOptionPremiumAdjustMent.Hidden = false;
                    ColOptionPremiumAdjustMent.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists(OrderFields.PROPERTY_OCCFEE))
                {
                    UltraGridColumn ColOccFee = gridBand.Columns[OrderFields.PROPERTY_OCCFEE];
                    ColOccFee.Header.VisiblePosition = visiblePosition++;
                    ColOccFee.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColOccFee.Header.Caption = OrderFields.CAPTION_OCCFEE;
                    ColOccFee.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColOccFee.Hidden = false;
                    ColOccFee.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists(OrderFields.PROPERTY_ORFFEE))
                {
                    UltraGridColumn ColOrfFee = gridBand.Columns[OrderFields.PROPERTY_ORFFEE];
                    ColOrfFee.Header.VisiblePosition = visiblePosition++;
                    ColOrfFee.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColOrfFee.Header.Caption = OrderFields.CAPTION_ORFFEE;
                    ColOrfFee.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColOrfFee.Hidden = false;
                    ColOrfFee.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists(OrderFields.PROPERTY_SECFEE))
                {
                    UltraGridColumn ColSecFee = gridBand.Columns[OrderFields.PROPERTY_SECFEE];
                    ColSecFee.Header.VisiblePosition = visiblePosition++;
                    ColSecFee.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColSecFee.Header.Caption = OrderFields.CAPTION_SECFEE;
                    ColSecFee.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColSecFee.Hidden = false;
                    ColSecFee.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists(OrderFields.PROPERTY_SOFTCOMMISSION))
                {
                    UltraGridColumn ColSoftCommission = gridBand.Columns[OrderFields.PROPERTY_SOFTCOMMISSION];
                    ColSoftCommission.Header.VisiblePosition = visiblePosition++;
                    ColSoftCommission.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColSoftCommission.Header.Caption = OrderFields.CAPTION_SOFTCOMMISSION;
                    ColSoftCommission.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColSoftCommission.Hidden = false;
                    ColSoftCommission.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists(OrderFields.PROPERTY_CLEARINGBROKERFEE))
                {
                    UltraGridColumn ColClearingBrokerFee = gridBand.Columns[OrderFields.PROPERTY_CLEARINGBROKERFEE];
                    ColClearingBrokerFee.Header.VisiblePosition = visiblePosition++;
                    ColClearingBrokerFee.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColClearingBrokerFee.Header.Caption = OrderFields.CAPTION_CLEARINGBROKERFEE;
                    ColClearingBrokerFee.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColClearingBrokerFee.Hidden = false;
                    ColClearingBrokerFee.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("VenueID"))
                {
                    UltraGridColumn ColVenue = gridBand.Columns["VenueID"];
                    ColVenue.Header.VisiblePosition = visiblePosition++;
                    ColVenue.Header.Caption = "Venue";
                    ColVenue.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    ColVenue.ValueList = GetValueList(Prana.CommonDataCache.CachedDataManager.GetInstance.GetAllVenues());
                    ColVenue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }
                if (gridBand.Columns.Exists("CounterPartyID"))
                {
                    UltraGridColumn ColCounterParty = gridBand.Columns["CounterPartyID"];
                    ColCounterParty.Header.VisiblePosition = visiblePosition++;
                    ColCounterParty.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    ColCounterParty.Header.Caption = "CounterParty";
                    ColCounterParty.ValueList = GetValueList(CachedDataManager.GetInstance.GetUserCounterParties());
                    ColCounterParty.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("Strategy"))
                {
                    UltraGridColumn ColStrategy = gridBand.Columns["Strategy"];
                    ColStrategy.Header.VisiblePosition = visiblePosition++;
                    ColStrategy.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColStrategy.Header.Caption = "Strategy";
                    ColStrategy.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColStrategy.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("PBSymbol"))
                {
                    UltraGridColumn ColPBSymbol = gridBand.Columns["PBSymbol"];
                    ColPBSymbol.Header.VisiblePosition = visiblePosition++;
                    ColPBSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColPBSymbol.Header.Caption = "PB Symbol";
                    ColPBSymbol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColPBSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("PBAssetType"))
                {
                    UltraGridColumn ColPBAssetType = gridBand.Columns["PBAssetType"];
                    ColPBAssetType.Header.VisiblePosition = visiblePosition++;
                    ColPBAssetType.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColPBAssetType.Header.Caption = "PB AssetType";
                    ColPBAssetType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColPBAssetType.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("Description"))
                {
                    UltraGridColumn ColDescription = gridBand.Columns["Description"];
                    ColDescription.Header.VisiblePosition = visiblePosition++;
                    ColDescription.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColDescription.Header.Caption = "Description";
                    ColDescription.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColDescription.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("FXConversionMethodOperator"))
                {
                    UltraGridColumn ColFXConversionMethodOperator = gridBand.Columns["FXConversionMethodOperator"];
                    ColFXConversionMethodOperator.Header.VisiblePosition = visiblePosition++;
                    ColFXConversionMethodOperator.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColFXConversionMethodOperator.Header.Caption = "FXConversionMethodOperator";
                    ColFXConversionMethodOperator.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    //TODO: Add value lis here
                    ColFXConversionMethodOperator.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("FXRate"))
                {
                    UltraGridColumn ColFXRate = gridBand.Columns["FXRate"];
                    ColFXRate.Header.VisiblePosition = visiblePosition++;
                    ColFXRate.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColFXRate.Header.Caption = "FXRate";
                    ColFXRate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColFXRate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("RIC"))
                {
                    UltraGridColumn ColReutresSymbol = gridBand.Columns["RIC"];
                    ColReutresSymbol.Header.VisiblePosition = visiblePosition++;
                    ColReutresSymbol.CharacterCasing = CharacterCasing.Upper;
                    ColReutresSymbol.Header.Caption = OrderFields.CAPTION_RICSYMBOL;
                    ColReutresSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    ColReutresSymbol.NullText = String.Empty;
                    ColReutresSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("ISIN"))
                {
                    UltraGridColumn ColISINSymbol = gridBand.Columns["ISIN"];
                    ColISINSymbol.Header.VisiblePosition = visiblePosition++;
                    ColISINSymbol.CharacterCasing = CharacterCasing.Upper;
                    ColISINSymbol.Header.Caption = OrderFields.CAPTION_ISINSYMBOL;
                    ColISINSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    ColISINSymbol.NullText = String.Empty;
                    ColISINSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("SEDOL"))
                {
                    UltraGridColumn ColSEDOLSymbol = gridBand.Columns["SEDOL"];
                    ColSEDOLSymbol.Header.VisiblePosition = visiblePosition++;
                    ColSEDOLSymbol.CharacterCasing = CharacterCasing.Upper;
                    ColSEDOLSymbol.Header.Caption = OrderFields.CAPTION_SEDOLSYMBOL;
                    ColSEDOLSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    ColSEDOLSymbol.NullText = String.Empty;
                    ColSEDOLSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists(ApplicationConstants.SymbologyCodes.OSIOptionSymbol.ToString()))
                {
                    UltraGridColumn ColOSIOptionSymbol = gridBand.Columns[ApplicationConstants.SymbologyCodes.OSIOptionSymbol.ToString()];
                    ColOSIOptionSymbol.Header.VisiblePosition = visiblePosition++;
                    ColOSIOptionSymbol.CharacterCasing = CharacterCasing.Upper;
                    ColOSIOptionSymbol.Header.Caption = OrderFields.CAPTION_OSIOPTIONSYMBOL;
                    ColOSIOptionSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    ColOSIOptionSymbol.NullText = String.Empty;
                    ColOSIOptionSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists(ApplicationConstants.SymbologyCodes.IDCOOptionSymbol.ToString()))
                {
                    UltraGridColumn ColIDCOOptionSymbol = gridBand.Columns[ApplicationConstants.SymbologyCodes.IDCOOptionSymbol.ToString()];
                    ColIDCOOptionSymbol.Header.VisiblePosition = visiblePosition++;
                    ColIDCOOptionSymbol.CharacterCasing = CharacterCasing.Upper;
                    ColIDCOOptionSymbol.Header.Caption = OrderFields.CAPTION_IDCOOPTIONSYMBOL;
                    ColIDCOOptionSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    ColIDCOOptionSymbol.NullText = String.Empty;
                    ColIDCOOptionSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                // Hide column as gennaro asked to do.
                if (gridBand.Columns.Exists(ApplicationConstants.SymbologyCodes.OPRAOptionSymbol.ToString()))
                {
                    UltraGridColumn ColOPRAOptionSymbol = gridBand.Columns[ApplicationConstants.SymbologyCodes.OPRAOptionSymbol.ToString()];
                    ColOPRAOptionSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("Multiplier"))
                {
                    UltraGridColumn ColMultiplier = gridBand.Columns["Multiplier"];
                    ColMultiplier.Header.VisiblePosition = visiblePosition++;
                    ColMultiplier.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColMultiplier.NullText = null;
                    ColMultiplier.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                    ColMultiplier.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }


                if (gridBand.Columns.Exists(OrderFields.PROPERTY_UNDERLYING_ID))
                {
                    UltraGridColumn colUnderLying = gridBand.Columns[OrderFields.PROPERTY_UNDERLYING_ID];
                    colUnderLying.Header.VisiblePosition = visiblePosition++;
                    colUnderLying.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colUnderLying.Header.Caption = OrderFields.CAPTION_UNDERLYING_NAME;
                    colUnderLying.ValueList = SecMasterHelper.getInstance().UnderLyings.Clone();
                    colUnderLying.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                }

                if (gridBand.Columns.Exists(OrderFields.PROPERTY_EXCHANGEID))
                {
                    UltraGridColumn ColExchnage = gridBand.Columns[OrderFields.PROPERTY_EXCHANGEID];
                    ColExchnage.Header.VisiblePosition = visiblePosition++;
                    ColExchnage.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    ColExchnage.Header.Caption = OrderFields.CAPTION_EXCHANGE;
                    ColExchnage.ValueList = SecMasterHelper.getInstance().Exchanges.Clone();
                    ColExchnage.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("IssueDate"))
                {
                    UltraGridColumn ColIssueDate = gridBand.Columns["IssueDate"];
                    ColIssueDate.Header.VisiblePosition = visiblePosition++;
                    ColIssueDate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("FirstCouponDate"))
                {
                    UltraGridColumn ColFirstCouponDate = gridBand.Columns["FirstCouponDate"];
                    ColFirstCouponDate.Header.VisiblePosition = visiblePosition++;
                    ColFirstCouponDate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("ExpirationDate"))
                {
                    UltraGridColumn ColExpirationOrSettlementDate = gridBand.Columns["ExpirationDate"];
                    ColExpirationOrSettlementDate.Header.VisiblePosition = visiblePosition++;
                    ColExpirationOrSettlementDate.Header.Caption = OrderFields.CAPTION_EXPIRATIONDATE; //"Expiration Date";
                    ColExpirationOrSettlementDate.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    ColExpirationOrSettlementDate.NullText = "1/1/1800";
                    ColExpirationOrSettlementDate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists(OrderFields.PROPERTY_UNDERLYINGSYMBOL))
                {
                    UltraGridColumn ColUnderLyingSymbol = gridBand.Columns[OrderFields.PROPERTY_UNDERLYINGSYMBOL];
                    ColUnderLyingSymbol.Header.VisiblePosition = visiblePosition++;
                    ColUnderLyingSymbol.Header.Caption = OrderFields.CAPTION_UNDERLYINGSYMBOL;
                    ColUnderLyingSymbol.CharacterCasing = CharacterCasing.Upper;
                    ColUnderLyingSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    ColUnderLyingSymbol.NullText = String.Empty;
                    ColUnderLyingSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("Call_Put"))
                {
                    UltraGridColumn ColOptionType = gridBand.Columns["Call_Put"];
                    ColOptionType.Header.VisiblePosition = visiblePosition++;
                    ColOptionType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    ColOptionType.ValueList = SecMasterHelper.getInstance().OptionTypes.Clone();
                    ColOptionType.Header.Caption = OrderFields.CAPTION_PUT_CALL;
                    ColOptionType.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    ColOptionType.NullText = String.Empty;
                    ColOptionType.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists(OrderFields.PROPERTY_STRIKE_PRICE))
                {
                    UltraGridColumn ColStrikePirce = gridBand.Columns[OrderFields.PROPERTY_STRIKE_PRICE];
                    ColStrikePirce.Header.VisiblePosition = visiblePosition++;
                    ColStrikePirce.Header.Caption = OrderFields.CAPTION_STRIKE_PRICE;
                    ColStrikePirce.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    ColStrikePirce.NullText = null;
                    ColStrikePirce.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists(OrderFields.PROPERTY_LEADCURRENCYID))
                {
                    UltraGridColumn ColLeadCurrency = gridBand.Columns[OrderFields.PROPERTY_LEADCURRENCYID];
                    UltraGridColumn ColVsCurrency = gridBand.Columns[OrderFields.PROPERTY_VSCURRENCYID];
                    ValueList listCurencies = GetValueList(CachedDataManager.GetInstance.GetAllCurrencies());
                    ColLeadCurrency.ValueList = listCurencies;
                    ColVsCurrency.ValueList = listCurencies;
                    ColVsCurrency.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("AccrualBasis"))
                {
                    UltraGridColumn ColAccrualBasis = gridBand.Columns["AccrualBasis"];
                    ColAccrualBasis.Header.VisiblePosition = visiblePosition++;
                    ColAccrualBasis.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    ColAccrualBasis.Header.Caption = OrderFields.CAPTION_ACCRUALBASIS;
                    ColAccrualBasis.ValueList = SecMasterHelper.getInstance().AccrualBasis.Clone();
                    ColAccrualBasis.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("Freq"))
                {
                    UltraGridColumn ColFrequency = gridBand.Columns["Freq"];
                    ColFrequency.Header.VisiblePosition = visiblePosition++;
                    ColFrequency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    ColFrequency.Header.Caption = "Coupon Frequency";
                    ColFrequency.ValueList = SecMasterHelper.getInstance().Frequencies.Clone();
                    ColFrequency.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("Coupon"))
                {
                    UltraGridColumn ColCoupon = gridBand.Columns["Coupon"];
                    ColCoupon.Header.Caption = "Coupon (%)";
                    ColCoupon.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists(ApplicationConstants.CONST_IS_SECURITY_APPROVED))
                {
                    //Hide isSecApproved Column becuase it showing checkbox , we dont want it.
                    UltraGridColumn colIsSecApproved = gridBand.Columns[ApplicationConstants.CONST_IS_SECURITY_APPROVED];
                    colIsSecApproved.Hidden = true;
                    colIsSecApproved.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }


                if (gridBand.Columns.Exists("ValidationError"))
                {
                    //we are showing Approval Status based on isSecApproved property.
                    UltraGridColumn colComments = gridBand.Columns["ValidationError"];
                    colComments.Header.Caption = "Comments";
                    //colApprovalStatus.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    //  colApprovalStatus.ValueList = _approvalSatus;
                    colComments.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }
                if (gridBand.Columns.Exists("MismatchType"))
                {
                    UltraGridColumn colMismatchType = gridBand.Columns["MismatchType"];
                    colMismatchType.Header.VisiblePosition = visiblePosition++;
                    colMismatchType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colMismatchType.Header.Caption = "MismatchType";
                    colMismatchType.Hidden = false;
                    colMismatchType.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("ApprovedBy"))
                {
                    UltraGridColumn ColApprovedBy = gridBand.Columns["ApprovedBy"];
                    ColApprovedBy.Header.Caption = "Last Approved By ";
                    ColApprovedBy.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    ColApprovedBy.ValueList = GetValueList(CachedDataManager.GetInstance.GetAllUsersName());
                    ColApprovedBy.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("UDAAssetClassID"))
                {
                    UltraGridColumn colUDAAsset = gridBand.Columns["UDAAssetClassID"];
                    colUDAAsset.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colUDAAsset.Header.Caption = "UDA Asset";
                    colUDAAsset.ValueList = SecMasterHelper.getInstance().UDAAssets.Clone();
                    colUDAAsset.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("UDASectorID"))
                {
                    UltraGridColumn colUDAUDASector = gridBand.Columns["UDASectorID"];
                    colUDAUDASector.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colUDAUDASector.Header.Caption = "UDA Sector";
                    colUDAUDASector.ValueList = SecMasterHelper.getInstance().UDASectors.Clone();
                    colUDAUDASector.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("UDASubSectorID"))
                {
                    UltraGridColumn colUDASubSector = gridBand.Columns["UDASubSectorID"];
                    colUDASubSector.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colUDASubSector.Header.Caption = "UDA SubSector";
                    colUDASubSector.ValueList = SecMasterHelper.getInstance().UDASubSectors.Clone();
                    colUDASubSector.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("UDASecurityTypeID"))
                {
                    UltraGridColumn colUDASecurityType = gridBand.Columns["UDASecurityTypeID"];
                    colUDASecurityType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colUDASecurityType.Header.Caption = "UDA Security";
                    colUDASecurityType.ValueList = SecMasterHelper.getInstance().UDASecurityTypes.Clone();
                    colUDASecurityType.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                if (gridBand.Columns.Exists("UDACountryID"))
                {
                    UltraGridColumn colUDACountry = gridBand.Columns["UDACountryID"];
                    colUDACountry.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colUDACountry.Header.Caption = "UDA Country";
                    colUDACountry.ValueList = SecMasterHelper.getInstance().UDACountries.Clone();
                    colUDACountry.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
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

        private ValueList GetSideValueLists()
        {
            #region side value lists
            ValueList valueListSides = new ValueList();
            try
            {
                valueListSides.ValueListItems.Clear();
                valueListSides.ValueListItems.Add(FIXConstants.SIDE_Buy, "Buy");
                valueListSides.ValueListItems.Add(FIXConstants.SIDE_Buy_Closed, "Buy to Close");
                valueListSides.ValueListItems.Add(FIXConstants.SIDE_Buy_Open, "Buy to Open");
                valueListSides.ValueListItems.Add(FIXConstants.SIDE_Sell, "Sell");
                valueListSides.ValueListItems.Add(FIXConstants.SIDE_Sell_Closed, "Sell to Close");
                valueListSides.ValueListItems.Add(FIXConstants.SIDE_Sell_Open, "Sell to Open");
                valueListSides.ValueListItems.Add(FIXConstants.SIDE_SellShort, "Sell short");
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return valueListSides;
            #endregion
        }

        private ValueList GetValueList(Dictionary<int, string> values)
        {
            ValueList list = new ValueList();
            list.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
            foreach (KeyValuePair<int, string> value in values)
            {
                list.ValueListItems.Add(value.Key, value.Value);
            }
            return list;
        }

        /// <summary>
        /// Gets the name of the file for preparing report
        /// </summary>
        /// <returns>The name of the file</returns>
        private string GetFileNameFromRef(string refString)
        {
            string fileName = string.Empty;
            string fileFullName = string.Empty;
            try
            {
                //fileName=refString.Replace("DashBoardData", "DashBoardData\\Archive");
                fileName = refString.Substring(refString.LastIndexOf("\\") + 1);
                //fileName = Path.GetFileName(refString);
                fileFullName = Application.StartupPath + "\\DashBoardData\\Archive\\Import\\" + fileName;
                //if (!string.IsNullOrWhiteSpace(refString))
                //{
                //    fileFullName = Application.StartupPath + fileName;
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
            return fileFullName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmReport_FormClosed(object sender, FormClosedEventArgs e)
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
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
        /// File the Control with the details 
        /// </summary>
        /// <param name="dictReport">Dictionary of details</param>
        public void FillReport(Dictionary<string, String> dictReport, UltraGrid grdReport)
        {
            try
            {
                DataSet ds = new DataSet();
                string xmlFile = dictReport["FileName"];
                if (File.Exists(xmlFile))
                {
                    ds.ReadXml(xmlFile);
                    SecMasterHelper.getInstance().GetAllDefaults();
                    //if (!string.IsNullOrEmpty(dictReport["Date"].ToString()))
                    //{
                    //    txtStartDate.Text = DateTime.Parse(dictReport["Date"]).ToShortDateString();
                    //}
                    //if (!string.IsNullOrEmpty(dictReport["FileType"].ToString()))
                    //{
                    //    txtFileType.Text = dictReport["FileType"];
                    //}
                    //if (!string.IsNullOrEmpty(dictReport["AccountCount"].ToString()))
                    //{
                    //    txtAccount.Text = dictReport["AccountCount"];
                    //}
                    //if (!string.IsNullOrEmpty(dictReport["NonValidatedSymbols"].ToString()))
                    //{
                    //    txtSymFail.Text = dictReport["NonValidatedSymbols"];
                    //}
                    //if (!string.IsNullOrEmpty(dictReport["PendingSymbols"].ToString()))
                    //{
                    //    txtSymPending.Text = dictReport["PendingSymbols"];
                    //}
                    //if (!string.IsNullOrEmpty(dictReport["ValidatedSymbols"].ToString()))
                    //{
                    //    txtSymValid.Text = dictReport["ValidatedSymbols"];
                    //}
                    //if (!string.IsNullOrEmpty(dictReport["ThirdPartyType"].ToString()))
                    //{
                    //    txtThirdParty.Text = dictReport["ThirdPartyType"];
                    //}
                    //if (!string.IsNullOrEmpty(dictReport["TotalSymbols"].ToString()))
                    //{
                    //    txttotalSymbol.Text = dictReport["TotalSymbols"];
                    //}

                    ds.Relations.Clear();

                    grdReport.DataSource = ds.Tables[0];
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

        private void BringFormToFront(Form form)
        {
            if (form.WindowState == FormWindowState.Minimized)
            {
                form.WindowState = FormWindowState.Normal;

            }
            form.Location = new Point(this.Location.X + 30, this.Location.Y + 30);
            form.BringToFront();
        }

        private void grdArchiveDashBoard_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Band.Columns.Exists("RetrievalStatus"))
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
                    case "Running":
                        //modified by amit on 15.04.2015
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-6866
                        cell.Appearance.ForeColor = Color.OrangeRed;
                        cell.ActiveAppearance.ForeColor = Color.OrangeRed;
                        break;
                    default:
                        //modified by amit on 15.04.2015
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-6866
                        cell.Appearance.ForeColor = Color.OrangeRed;
                        cell.ActiveAppearance.ForeColor = Color.OrangeRed;
                        break;
                }
            }

            if (e.Row.Band.Columns.Exists("SymbolValidation"))
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
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-6866
                        cell.Appearance.ForeColor = Color.OrangeRed;
                        cell.ActiveAppearance.ForeColor = Color.OrangeRed;
                        break;
                }
            }
        }
        /// <summary>
        /// Added By Faisal Shah
        /// Purpose to delete the Selected Archived Batches
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> selectedRowCollection = new List<int>();
                foreach (UltraGridRow row in grdArchiveDashBoard.Rows)
                {
                    if (row.Cells.Exists("Select") && row.Cells["Select"].Text == "True")
                    {
                        selectedRowCollection.Add(row.Index);
                    }
                }
                int changeInIndex = 0;
                if (selectedRowCollection != null && selectedRowCollection.Count > 0)
                {
                    foreach (int index in selectedRowCollection)
                    {
                        int rowIndex = index - changeInIndex; ;
                        int startpoint = grdArchiveDashBoard.Rows[rowIndex].Cells["DashboardFile"].Value.ToString().IndexOf("Import_-1");
                        string fileName = Path.GetFileNameWithoutExtension(grdArchiveDashBoard.Rows[rowIndex].Cells["DashboardFile"].Value.ToString().Substring(startpoint, grdArchiveDashBoard.Rows[rowIndex].Cells["DashboardFile"].Value.ToString().Length - startpoint));
                        string filePath = Application.StartupPath + "\\DashBoardData\\Archive\\Import\\" + fileName;
                        if (File.Exists(filePath + "_ImportData.xml"))
                        {
                            File.Delete(filePath + "_ImportData.xml");
                        }

                        if (File.Exists(filePath + "_ImportData_NonValidatedTrades.xml"))
                        {
                            File.Delete(filePath + "_ImportData_NonValidatedTrades.xml");
                        }
                        if (File.Exists(filePath + ".xml"))
                        {
                            File.Delete(filePath + ".xml");
                        }
                        if (File.Exists(filePath + "_RunUpload.xml"))
                        {
                            File.Delete(filePath + "_RunUpload.xml");
                        }
                        if (File.Exists(filePath + "_ValidatedSymbols.xml"))
                        {
                            File.Delete(filePath + "_ValidatedSymbols.xml");
                        }
                        if (File.Exists(Application.StartupPath + "\\DashBoardData\\Archive\\Import\\" + grdArchiveDashBoard.Rows[rowIndex].Cells["FileName"].Value.ToString() + "_MetaData.xml"))
                        {
                            File.Delete(Application.StartupPath + "\\DashBoardData\\Archive\\Import\\" + grdArchiveDashBoard.Rows[rowIndex].Cells["FileName"].Value.ToString() + "_MetaData.xml");
                        }
                        grdArchiveDashBoard.Rows[rowIndex].Delete();
                        changeInIndex++;
                    }
                    grdArchiveDashBoard.UpdateData();
                    MessageBox.Show("Selected items deleted successfully", "Delete Archived Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    MessageBox.Show("Please Select a row to be deleted", "Delete Archived Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        private void grdArchiveDashBoard_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
        {
            e.DisplayPromptMsg = false;
        }
        /// <summary>
        /// Added By Faisal Shah
        /// List Contains all the columns that are to be Shown to the user on the grid
        /// </summary>
        public List<string> DisplayableArchiveGridColumns
        {
            get
            {
                List<string> archiveGridColumns = new List<string>();
                archiveGridColumns.Add("Select");
                archiveGridColumns.Add("Status");
                archiveGridColumns.Add("StartTime");
                archiveGridColumns.Add("ExecutionDate");
                archiveGridColumns.Add("RetrievalStatus");
                archiveGridColumns.Add("Comments");
                archiveGridColumns.Add("SymbolValidation");
                archiveGridColumns.Add("ImportStatus");
                archiveGridColumns.Add("btnView");
                archiveGridColumns.Add("btnViewArchived");
                return archiveGridColumns;
            }
        }
        /// <summary>
        /// Added By Faisal Shah
        /// List Contains all the columns that are to be Shown to the user both Visible and Hidden
        /// </summary>
        public List<string> AllArchiveGridColumns
        {
            get
            {
                List<string> archiveGridColumns = new List<string>();
                archiveGridColumns.Add("Select");
                archiveGridColumns.Add("Task");
                archiveGridColumns.Add("Status");
                archiveGridColumns.Add("StartTime");
                archiveGridColumns.Add("ExecutionDate");
                archiveGridColumns.Add("ThirdPartyType");
                archiveGridColumns.Add("FileType");
                archiveGridColumns.Add("RetrievalStatus");
                archiveGridColumns.Add("Comments");
                archiveGridColumns.Add("SymbolValidation");
                archiveGridColumns.Add("ImportStatus");
                archiveGridColumns.Add("FileMetaData");
                archiveGridColumns.Add("btnView");
                archiveGridColumns.Add("btnViewArchived");
                archiveGridColumns.Add("FileSize");
                return archiveGridColumns;
            }
        }
        /// <summary>
        /// Added bY faisal shah
        /// Set Columns if No layout is Saved Previously
        /// </summary>
        /// <param name="gridBand"></param>
        private void SetColumnsForArchiveGrid(UltraGridBand gridBand)
        {
            try
            {

                int visiblePosition = 1;
                UltraGridColumn colSelect = gridBand.Columns["Select"];
                colSelect.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                colSelect.Width = 50;
                colSelect.Header.Caption = "";
                colSelect.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                colSelect.Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                colSelect.Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center;
                colSelect.Header.VisiblePosition = visiblePosition++;
                colSelect.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;

                UltraGridColumn colFormatName = gridBand.Columns["Task"];
                colFormatName.Header.Caption = "Format Name";
                colFormatName.Header.Appearance.TextHAlign = HAlign.Center;
                colFormatName.Header.VisiblePosition = visiblePosition++;
                colFormatName.CellActivation = Activation.NoEdit;

                UltraGridColumn colBatchStatus = gridBand.Columns["Status"];
                colBatchStatus.Header.Caption = "Batch Status";
                colBatchStatus.Header.Appearance.TextHAlign = HAlign.Center;
                colBatchStatus.Header.VisiblePosition = visiblePosition++;
                colBatchStatus.CellActivation = Activation.NoEdit;
                colBatchStatus.Hidden = true;
                //if (band.Columns.Exists("StartTime"))
                //{
                UltraGridColumn colRunDate = gridBand.Columns["StartTime"];
                colRunDate.Header.Caption = "Run Date";
                colRunDate.Header.Appearance.TextHAlign = HAlign.Center;
                colRunDate.Header.VisiblePosition = visiblePosition++;
                colRunDate.CellActivation = Activation.NoEdit;
                //}

                UltraGridColumn colExecutionDate = gridBand.Columns["ExecutionDate"];
                colExecutionDate.Header.Caption = "Execution Date";
                colExecutionDate.Header.Appearance.TextHAlign = HAlign.Center;
                colExecutionDate.Header.VisiblePosition = visiblePosition++;
                colExecutionDate.CellActivation = Activation.NoEdit;

                //if (band.Columns.Exists("ThirdPartyType"))
                //{
                UltraGridColumn colThirdParty = gridBand.Columns["ThirdPartyType"];
                colThirdParty.Header.Caption = "Third Party";
                colThirdParty.Header.Appearance.TextHAlign = HAlign.Center;
                colThirdParty.Header.VisiblePosition = visiblePosition++;
                colThirdParty.CellActivation = Activation.NoEdit;
                //}

                //if (band.Columns.Exists("FileType"))
                //{
                UltraGridColumn colFileType = gridBand.Columns["FileType"];
                colFileType.Header.Caption = "File Type";
                colFileType.Header.Appearance.TextHAlign = HAlign.Center;
                colFileType.Header.VisiblePosition = visiblePosition++;
                colFileType.CellActivation = Activation.NoEdit;

                UltraGridColumn colStatusRetrieval = gridBand.Columns["RetrievalStatus"];
                colStatusRetrieval.Header.Caption = "Status - Retrieval";
                colStatusRetrieval.Header.Appearance.TextHAlign = HAlign.Center;
                colStatusRetrieval.Header.VisiblePosition = visiblePosition++;
                colStatusRetrieval.CellActivation = Activation.NoEdit;
                //}

                //if (band.Columns.Exists("FileMetaData"))
                //{
                UltraGridColumn colRows = gridBand.Columns["FileMetaData"];
                colRows.Header.Caption = "Rows";
                colRows.Header.Appearance.TextHAlign = HAlign.Center;
                colRows.Header.VisiblePosition = visiblePosition++;
                colRows.CellActivation = Activation.NoEdit;
                //}

                //if (band.Columns.Exists("FileSize"))
                //{
                UltraGridColumn colFileSize = gridBand.Columns["FileSize"];
                colFileSize.Header.Caption = "File Size";
                colFileSize.Header.Appearance.TextHAlign = HAlign.Center;
                colFileSize.Header.VisiblePosition = visiblePosition++;
                colFileSize.CellActivation = Activation.NoEdit;
                //}

                //if (band.Columns.Exists("Comments"))
                //{
                UltraGridColumn colComments = gridBand.Columns["Comments"];
                colComments.Header.Caption = "Comments";
                colComments.Header.Appearance.TextHAlign = HAlign.Center;
                colComments.Header.VisiblePosition = visiblePosition++;
                colComments.CellActivation = Activation.NoEdit;
                //}

                if (gridBand.Columns.Exists("btnView"))
                {
                    UltraGridColumn colBtnView = gridBand.Columns["btnView"];
                    colBtnView.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    colBtnView.Width = 50;
                    colBtnView.Header.Caption = "View";
                    colBtnView.Header.Appearance.TextHAlign = HAlign.Center;
                    colBtnView.NullText = "View";
                    colBtnView.Header.VisiblePosition = visiblePosition++;
                    colBtnView.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                }

                if (gridBand.Columns.Exists("btnViewArchived"))
                {
                    UltraGridColumn colBtnViewArchieved = gridBand.Columns["btnViewArchived"];
                    colBtnViewArchieved.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    colBtnViewArchieved.Width = 50;
                    colBtnViewArchieved.Header.Appearance.TextHAlign = HAlign.Center;
                    colBtnViewArchieved.Header.Caption = "View Archived Data";
                    colBtnViewArchieved.NullText = "View Archived Data";
                    colBtnViewArchieved.Header.VisiblePosition = visiblePosition++;
                    colBtnViewArchieved.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                }
                //if (band.Columns.Exists("SymbolValidation"))
                //{
                UltraGridColumn colSymbolValidation = gridBand.Columns["SymbolValidation"];
                colSymbolValidation.Header.Caption = "Symbol Validation";
                colSymbolValidation.Header.Appearance.TextHAlign = HAlign.Center;
                colSymbolValidation.Header.VisiblePosition = visiblePosition++;
                colSymbolValidation.CellActivation = Activation.NoEdit;
                //}

                //if (band.Columns.Exists("btnReRunUpload"))
                //{
                //UltraGridColumn colBtnReRunupload = band.Columns["btnReRunUpload"];
                //colBtnReRunupload.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                //colBtnReRunupload.Width = 50;
                //colBtnReRunupload.Header.Caption = "Re-Run Upload";
                //colBtnReRunupload.NullText = "Re-Run Upload";
                //colBtnReRunupload.Header.VisiblePosition = visiblePosition++;
                //colBtnReRunupload.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                //}

                //if (band.Columns.Exists("btnReRunSymbolValidation"))
                //{
                //UltraGridColumn colReRunSymbolValidation = band.Columns["btnReRunSymbolValidation"];
                //colReRunSymbolValidation.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                //colReRunSymbolValidation.Width = 50;
                //colReRunSymbolValidation.Header.Caption = "Re-Run Symbol Validation";
                //colReRunSymbolValidation.NullText = "Re-Run Symbol Validation";
                //colReRunSymbolValidation.Header.VisiblePosition = visiblePosition++;
                //colReRunSymbolValidation.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                //}

                //if (band.Columns.Exists("btnManualUpload"))
                //{
                //UltraGridColumn colBtnManualUpload = band.Columns["btnManualUpload"];
                //colBtnManualUpload.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                //colBtnManualUpload.Width = 50;
                //colBtnManualUpload.Header.Caption = "Manual Upload";
                //colBtnManualUpload.NullText = "Manual Upload";
                //colBtnManualUpload.Header.VisiblePosition = visiblePosition++;
                //colBtnManualUpload.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                //}

                //if (band.Columns.Exists("btnImportInApp"))
                //{
                //UltraGridColumn colBtnImportInApp = band.Columns["btnImportInApp"];
                //colBtnImportInApp.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                //colBtnImportInApp.Width = 50;
                //colBtnImportInApp.Header.Caption = "Import Into Application";
                //colBtnImportInApp.NullText = "Import Into Application";
                //colBtnImportInApp.Header.VisiblePosition = visiblePosition++;
                //colBtnImportInApp.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                //}

                if (gridBand.Columns.Exists("ImportStatus"))
                {
                    UltraGridColumn colBtnImportStatus = gridBand.Columns["ImportStatus"];
                    //colBtnImportStatus.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    colBtnImportStatus.Width = 50;
                    colBtnImportStatus.Header.Appearance.TextHAlign = HAlign.Center;
                    colBtnImportStatus.Header.VisiblePosition = visiblePosition++;
                    colBtnImportStatus.Header.Caption = "Import Status";
                    colBtnImportStatus.CellActivation = Activation.NoEdit;
                    //colBtnImportStatus.NullText = "Import Status";
                    //colBtnImportStatus.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                }
                //UltraGridColumn colBtnErrorReport = band.Columns["ShowErrorReport"];
                //colBtnErrorReport.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                //colBtnErrorReport.Width = 50;
                //colBtnErrorReport.Header.VisiblePosition = visiblePosition++;
                //colBtnErrorReport.Header.Caption = "Error Report";
                //colBtnErrorReport.NullText = "Show";
                //colBtnErrorReport.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                Infragistics.Win.UltraWinGrid.ColumnsCollection columns = gridBand.Columns;

                UltraWinGridUtils.SetColumns(DisplayableArchiveGridColumns, grdArchiveDashBoard);
                foreach (UltraGridColumn col in columns)
                {
                    col.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                }

                foreach (string col in AllArchiveGridColumns)
                {

                    if (!columns.Exists(col))
                    {
                        gridBand.Columns.Add(col);
                    }
                    UltraGridColumn column = columns[col];
                    column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    //column.Hidden = false;
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
        /// Set Column Properties and make all Columns in AllColumns List Visible 
        /// </summary>
        /// <param name="columnList"></param>
        /// <param name="grd"></param>
        public static void SetColumns(List<string> columnList, UltraGrid grd)
        {
            try
            {
                Infragistics.Win.UltraWinGrid.ColumnsCollection columns = grd.DisplayLayout.Bands[0].Columns;
                if (columnList != null)
                {
                    //Hide all columns
                    foreach (UltraGridColumn col in columns)
                    {
                        columns[col.Key].Hidden = true;
                        //following line auto adjust width of columns of ultragrid accocrding to text size of header.
                        col.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All);
                    }

                    //Unhide and Set postions for required columns
                    int visiblePosition = 1;
                    foreach (string col in columnList)
                    {
                        if (columns.Exists(col))
                        {
                            UltraGridColumn column = columns[col];
                            column.Hidden = false;
                            column.Header.VisiblePosition = visiblePosition;
                            column.Width = 80;
                            visiblePosition++;
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
        /// Added By Faisal Shah
        /// Purpose to Load Layout from the File
        /// </summary>
        /// <returns></returns>
        private static ArchiveDashBoardLayout GetArchiveDashboardLayout()
        {
            _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            _archiveDashboardLayoutDirectoryPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID;
            _archiveDashboardLayoutFilePath = _archiveDashboardLayoutDirectoryPath + @"\ArchiveDashboardLayout.xml";

            ArchiveDashBoardLayout archiveLayout = new ArchiveDashBoardLayout();
            try
            {
                if (!Directory.Exists(_archiveDashboardLayoutDirectoryPath))
                {
                    Directory.CreateDirectory(_archiveDashboardLayoutDirectoryPath);
                }
                if (File.Exists(_archiveDashboardLayoutFilePath))
                {
                    using (FileStream fs = File.OpenRead(_archiveDashboardLayoutFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ArchiveDashBoardLayout));
                        archiveLayout = (ArchiveDashBoardLayout)serializer.Deserialize(fs);
                    }
                }

                _archiveDashboardLayout = archiveLayout;
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

            return archiveLayout;
        }
        /// <summary>
        /// Added By Faisal Shah
        /// Context Menu Click Operation to Save Layout on Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdArchiveDashBoard != null)
                {
                    if (grdArchiveDashBoard.DisplayLayout.Bands[0].Columns.Count > 0)
                    {
                        ArchiveDashboardLayout.archiveGridColumns = GetGridColumnLayout(grdArchiveDashBoard);
                    }
                }
                SaveArchiveGridLayout();
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
        /// Added By Faisal Shah
        /// Purpose to Get the Layout details before Saving
        /// </summary>
        /// <param name="grdArchiveDashBoard"></param>
        /// <returns></returns>
        private List<ColumnData> GetGridColumnLayout(UltraGrid grdArchiveDashBoard)
        {
            List<ColumnData> listGridCols = new List<ColumnData>();
            UltraGridBand band = grdArchiveDashBoard.DisplayLayout.Bands[0];
            try
            {
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
            return listGridCols;
        }
        /// <summary>
        /// Added By Faisal Shah
        /// Purpose to Save Layout of  grid in UserID Foler in Prana Preferences
        /// </summary>
        private void SaveArchiveGridLayout()
        {
            try
            {

                using (XmlTextWriter writer = new XmlTextWriter(_archiveDashboardLayoutFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(ArchiveDashBoardLayout));
                    serializer.Serialize(writer, _archiveDashboardLayout);

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

        private void grdArchiveDashBoard_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {

            try
            {

                e.Cancel = true;
                if (grdArchiveDashBoard.DataSource != null)
                {
                    (this.FindForm()).AddCustomColumnChooser(this.grdArchiveDashBoard);
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

        private void grdArchiveDashBoard_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }
    }
}
