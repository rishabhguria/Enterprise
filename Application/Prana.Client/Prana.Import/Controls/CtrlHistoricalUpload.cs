using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Utilities.ImportExportUtilities;
using Prana.Utilities.UI.ImportExportUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Import.Controls
{
    public partial class CtrlHistoricalUpload : UserControl
    {
        Form _frmUploadedFile;
        Form _frmReport;
        ctrlImportReport ctrlReport = null;
        UltraGrid _grdReport = null;

        public CtrlHistoricalUpload()
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
                btnExport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnExport.ForeColor = System.Drawing.Color.White;
                btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnExport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnExport.UseAppStyling = false;
                btnExport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGetData.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetData.ForeColor = System.Drawing.Color.White;
                btnGetData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetData.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetData.UseAppStyling = false;
                btnGetData.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void grdHistoricalUpload_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {

                //add filters on grid if there are rows in grid
                if (grdHistoricalUpload.Rows.Count > 0)
                {
                    UltraWinGridUtils.EnableFixedFilterRow(e);
                }
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

                band.Override.ButtonStyle = UIElementButtonStyle.Button3D;

                string[] array = { "Select", "Task", "Status", "StartTime", "ThirdPartyType", "FileType", "RetrievalStatus", "FileMetaData", "FileSize", "Comments", "SymbolValidation", "ImportStatus", "btnView", "btnReRunUpload", "btnReRunSymbolValidation", "btnManualUpload", "btnImportInApp" };
                List<string> lstColumns = new List<string>(array);

                //add all the columns to the grid given in lstColumns
                SetupGridColumns(band, lstColumns);
                int visiblePosition = 0;

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
                colSelect.Header.VisiblePosition = visiblePosition++;
                colSelect.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;

                UltraGridColumn colFormatName = band.Columns["Task"];
                colFormatName.Header.Caption = "Format Name";
                colFormatName.Header.Appearance.TextHAlign = HAlign.Center;
                colFormatName.Header.VisiblePosition = visiblePosition++;
                colFormatName.CellActivation = Activation.NoEdit;

                UltraGridColumn colBatchStatus = band.Columns["Status"];
                colBatchStatus.Header.Caption = "Batch Status";
                colBatchStatus.Header.Appearance.TextHAlign = HAlign.Center;
                colBatchStatus.Header.VisiblePosition = visiblePosition++;
                colBatchStatus.CellActivation = Activation.NoEdit;

                //if (band.Columns.Exists("StartTime"))
                //{
                UltraGridColumn colRunDate = band.Columns["StartTime"];
                colRunDate.Header.Caption = "Run Date";
                colRunDate.Header.Appearance.TextHAlign = HAlign.Center;
                colRunDate.Header.VisiblePosition = visiblePosition++;
                colRunDate.CellActivation = Activation.NoEdit;
                //}

                //if (band.Columns.Exists("ThirdPartyType"))
                //{
                UltraGridColumn colThirdParty = band.Columns["ThirdPartyType"];
                colThirdParty.Header.Caption = "Third Party";
                colThirdParty.Header.Appearance.TextHAlign = HAlign.Center;
                colThirdParty.Header.VisiblePosition = visiblePosition++;
                colThirdParty.CellActivation = Activation.NoEdit;
                //}

                //if (band.Columns.Exists("FileType"))
                //{
                UltraGridColumn colFileType = band.Columns["FileType"];
                colFileType.Header.Caption = "File Type";
                colFileType.Header.VisiblePosition = visiblePosition++;
                colFileType.Header.Appearance.TextHAlign = HAlign.Center;
                colFileType.CellActivation = Activation.NoEdit;
                UltraGridColumn colStatusRetrieval = band.Columns["RetrievalStatus"];
                colStatusRetrieval.Header.Caption = "Status - Retrieval";
                colStatusRetrieval.Header.Appearance.TextHAlign = HAlign.Center;
                colStatusRetrieval.Header.VisiblePosition = visiblePosition++;
                colStatusRetrieval.CellActivation = Activation.NoEdit;
                //}

                //if (band.Columns.Exists("FileMetaData"))
                //{
                UltraGridColumn colRows = band.Columns["FileMetaData"];
                colRows.Header.Caption = "Rows";
                colRows.Header.Appearance.TextHAlign = HAlign.Center;
                colRows.Header.VisiblePosition = visiblePosition++;
                colRows.CellActivation = Activation.NoEdit;
                //}

                //if (band.Columns.Exists("FileSize"))
                //{
                UltraGridColumn colFileSize = band.Columns["FileSize"];
                colFileSize.Header.Caption = "File Size";
                colFileSize.Header.Appearance.TextHAlign = HAlign.Center;
                colFileSize.Header.VisiblePosition = visiblePosition++;
                colFileSize.CellActivation = Activation.NoEdit;
                //}

                //if (band.Columns.Exists("Comments"))
                //{
                UltraGridColumn colComments = band.Columns["Comments"];
                colComments.Header.Caption = "Comments";
                colComments.Header.Appearance.TextHAlign = HAlign.Center;
                colComments.Header.VisiblePosition = visiblePosition++;
                colComments.CellActivation = Activation.NoEdit;
                //}

                //if (band.Columns.Exists("btnView"))
                //{
                UltraGridColumn colBtnView = band.Columns["btnView"];
                colBtnView.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                colBtnView.Width = 50;
                colBtnView.Header.Caption = "View";
                colBtnView.Header.Appearance.TextHAlign = HAlign.Center;
                colBtnView.NullText = "View";
                colBtnView.Header.VisiblePosition = visiblePosition++;
                colBtnView.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                //}

                //if (band.Columns.Exists("SymbolValidation"))
                //{
                UltraGridColumn colSymbolValidation = band.Columns["SymbolValidation"];
                colSymbolValidation.Header.Caption = "Symbol Validation";
                colSymbolValidation.Header.Appearance.TextHAlign = HAlign.Center;
                colSymbolValidation.Header.VisiblePosition = visiblePosition++;
                colSymbolValidation.CellActivation = Activation.NoEdit;
                //}

                //if (band.Columns.Exists("btnReRunUpload"))
                //{
                UltraGridColumn colBtnReRunupload = band.Columns["btnReRunUpload"];
                colBtnReRunupload.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                colBtnReRunupload.Width = 50;
                colBtnReRunupload.Header.Caption = "Re-Run Upload";
                colBtnReRunupload.Header.Appearance.TextHAlign = HAlign.Center;
                colBtnReRunupload.NullText = "Re-Run Upload";
                //colBtnReRunupload.Header.VisiblePosition = visiblePosition++;
                colBtnReRunupload.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colBtnReRunupload.Hidden = true;
                //}

                //if (band.Columns.Exists("btnReRunSymbolValidation"))
                //{
                UltraGridColumn colReRunSymbolValidation = band.Columns["btnReRunSymbolValidation"];
                colReRunSymbolValidation.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                colReRunSymbolValidation.Width = 50;
                colReRunSymbolValidation.Header.Caption = "Re-Run Symbol Validation";
                colReRunSymbolValidation.Header.Appearance.TextHAlign = HAlign.Center;
                colReRunSymbolValidation.NullText = "Re-Run Symbol Validation";
                //colReRunSymbolValidation.Header.VisiblePosition = visiblePosition++;
                colReRunSymbolValidation.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colReRunSymbolValidation.Hidden = true;
                //}

                //if (band.Columns.Exists("btnManualUpload"))
                //{
                UltraGridColumn colBtnManualUpload = band.Columns["btnManualUpload"];
                colBtnManualUpload.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                colBtnManualUpload.Width = 50;
                colBtnManualUpload.Header.Caption = "Manual Upload";
                colBtnManualUpload.Header.Appearance.TextHAlign = HAlign.Center;
                colBtnManualUpload.NullText = "Manual Upload";
                //colBtnManualUpload.Header.VisiblePosition = visiblePosition++;
                colBtnManualUpload.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colBtnManualUpload.Hidden = true;
                //}

                //if (band.Columns.Exists("btnImportInApp"))
                //{
                UltraGridColumn colBtnImportInApp = band.Columns["btnImportInApp"];
                colBtnImportInApp.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                colBtnImportInApp.Width = 50;
                colBtnImportInApp.Header.Caption = "Import Into Application";
                colBtnImportInApp.Header.Appearance.TextHAlign = HAlign.Center;
                colBtnImportInApp.NullText = "Import Into Application";
                //colBtnImportInApp.Header.VisiblePosition = visiblePosition++;
                colBtnImportInApp.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colBtnImportInApp.Hidden = true;
                //}

                //if (band.Columns.Exists("ImportStatus"))
                //{
                UltraGridColumn colBtnImportStatus = band.Columns["ImportStatus"];
                colBtnImportStatus.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                colBtnImportStatus.Width = 50;
                colBtnImportStatus.Header.VisiblePosition = visiblePosition++;
                colBtnImportStatus.Header.Appearance.TextHAlign = HAlign.Center;
                colBtnImportStatus.Header.Caption = "Import Status";
                colBtnImportStatus.NullText = "Import Status";
                colBtnImportStatus.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                //}

                foreach (UltraGridColumn col in band.Columns)
                {
                    if (!lstColumns.Contains(col.Key))
                    {
                        col.Hidden = true;
                    }
                    //following line auto adjust width of columns of ultragrid accocrding to text size of header.
                    col.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All);
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
        /// Get the details of successful uploads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetData_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime endDate = DateTime.Parse(dtToDate.Value.ToString());
                DateTime startDate = DateTime.Parse(dtFromDate.Value.ToString());
                if (endDate < startDate)
                {
                    MessageBox.Show("End Date cannot be less than the start date", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtToDate.Value = dtFromDate.Value;
                    dtToDate.Focus();
                    return;
                }

                string folderPath = Application.StartupPath + "\\DashBoardData\\Import";
                DataTable dtMain = new DataTable();
                DateTime dtStartDate = Convert.ToDateTime(dtFromDate.Value);
                DateTime dtEndDate = Convert.ToDateTime(dtToDate.Value);
                List<String> filesCollection = new List<string>();
                if (Directory.Exists(folderPath))
                {
                    string[] folders = Directory.GetDirectories(folderPath);

                    foreach (string folderName in folders)
                    {
                        string[] files = Directory.GetFiles(folderName);
                        filesCollection.AddRange(files);
                    }
                }
                string folderPathForHisttoricalUploads = Application.StartupPath + @"\DashBoardData";
                string[] historicalFolders = Directory.GetDirectories(folderPathForHisttoricalUploads);
                if (Directory.Exists(folderPathForHisttoricalUploads))
                {
                    foreach (string folderName in historicalFolders)
                    {
                        if (folderName.Equals(folderPathForHisttoricalUploads + "\\HistoricalUploads"))
                        {
                            string[] files = Directory.GetFiles(folderPathForHisttoricalUploads + "\\HistoricalUploads");
                            filesCollection.AddRange(files);
                        }
                        else if (folderName.Equals(folderPathForHisttoricalUploads + "\\Archive"))
                        {
                            string[] files = Directory.GetFiles(folderPathForHisttoricalUploads + "\\Archive\\Import");
                            filesCollection.AddRange(files);
                        }
                    }

                    Dictionary<int, List<int>> CompanyThirdPartyAccounts = CachedDataManager.GetInstance.CompanyThirdPartyAccounts();
                    foreach (string file in filesCollection)
                    {
                        DataSet ds = new DataSet();
                        ds.ReadXml(file);
                        if (ds.Tables.Count > 0)
                        {

                            DataTable dt = ds.Tables[0].Copy();
                            if (dt.Columns.Contains("StartTime") && !string.IsNullOrWhiteSpace(dt.Rows[0]["StartTime"].ToString()))
                            {
                                DateTime date = Convert.ToDateTime(dt.Rows[0]["StartTime"].ToString());
                                if (date.Date < dtStartDate.Date || date.Date > dtEndDate.Date)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }

                            #region Remove non permitted rows
                            // http://jira.nirvanasolutions.com:8080/browse/CHMW-1634
                            // user is able to see data of other accounts on import UI for which user not have permission
                            string thirdPartyName = string.Empty;

                            if (dt.Columns.Contains("ThirdPartyType"))
                            {
                                thirdPartyName = dt.Rows[0]["ThirdPartyType"].ToString();
                            }
                            if (!CompanyThirdPartyAccounts.Keys.Contains(CachedDataManager.GetInstance.GetThirdPartyIDByShortName(thirdPartyName)))
                            {
                                continue;
                            }
                            #endregion

                            if (dt.Columns.Contains("ImportStatus"))
                            {
                                dtMain.Merge(dt, true, MissingSchemaAction.Add);
                            }
                        }
                    }
                    grdHistoricalUpload.DataSource = dtMain;
                }
                else
                {
                    MessageBox.Show("No Data to show", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Export the data to the file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                UltraGridFileExporter.LoadFilePathAndExport(grdHistoricalUpload, this);
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
        /// Show the report and import status report on grid button clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdHistoricalUpload_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key == "btnView")
                {
                    if (e.Cell.Row.Band.Columns.Exists("Status") && e.Cell.Row.Cells["Status"].Text.Equals("Running"))
                    {
                        MessageBox.Show("Batch is running, wait for the completion of batch", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (e.Cell.Row.Cells["RetrievalStatus"].Text.ToString().Equals("Failure"))
                    {
                        MessageBox.Show("File missing", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (e.Cell.Row.Band.Columns.Exists("ProcessedFilePath"))
                    {
                        string filePath = Application.StartupPath + e.Cell.Row.Cells["ProcessedFilePath"].Value.ToString();
                        //check if file exsits at path
                        if (File.Exists(filePath))
                        {
                            DataTable dataSource = FileReaderFactory.GetDataTableFromDifferentFileFormats(filePath);
                            if (dataSource != null)
                            {
                                //if form is not created
                                if (_frmUploadedFile == null || _frmUploadedFile.IsDisposed)
                                {
                                    _grdReport = new UltraGrid();
                                    _frmUploadedFile = new Form();
                                    _frmUploadedFile.ShowIcon = false;
                                    _frmUploadedFile.FormClosed += frmUploadedFile_FormClosed;
                                    //set the form and grid properties
                                    _grdReport.DisplayLayout.Bands[0].ColHeadersVisible = false;

                                    SetThemeAtDynamicForm(_frmUploadedFile, _grdReport);
                                    //grdReport.DataSource = dataSource;
                                    //disable editing in first row as it will be the hearers
                                    if (_grdReport.DisplayLayout.Rows.Count > 0)
                                        _grdReport.DisplayLayout.Rows[0].Activation = Activation.NoEdit;
                                    _grdReport.InitializeRow += grdReport_InitializeRow;
                                }
                                else
                                {
                                    //else previos form is bring to front
                                    _frmUploadedFile.BringToFront();
                                }

                                _frmUploadedFile.Text = e.Cell.Row.Cells["Task"].Value.ToString();
                                _grdReport.DataSource = dataSource;

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

                else if (e.Cell.Column.Key == "ImportStatus")
                {
                    if (e.Cell.Row.Band.Columns.Exists("Status") && e.Cell.Row.Cells["Status"].Text.Equals("Running"))
                    {
                        MessageBox.Show("Batch is running, wait for the completion of batch", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (e.Cell.Row.Band.Columns.Exists("ImportDataRef"))
                    {
                        string fileName = Application.StartupPath + e.Cell.Row.Cells["ImportDataRef"].Value.ToString();
                        Dictionary<string, string> dictReportData = new Dictionary<string, string>();
                        if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                        {
                            dictReportData.Add("FileName", fileName);
                            if (!string.IsNullOrEmpty(e.Cell.Row.Cells["FileType"].Value.ToString()))
                            {
                                dictReportData.Add("FileType", e.Cell.Row.Cells["FileType"].Value.ToString());
                            }
                            if (!string.IsNullOrEmpty(e.Cell.Row.Cells["ThirdPartyType"].Value.ToString()))
                            {
                                dictReportData.Add("ThirdPartyType", e.Cell.Row.Cells["ThirdPartyType"].Value.ToString());
                            }
                            if (!string.IsNullOrEmpty(e.Cell.Row.Cells["TotalSymbols"].Value.ToString()))
                            {
                                dictReportData.Add("TotalSymbols", e.Cell.Row.Cells["TotalSymbols"].Value.ToString());
                            }
                            if (!string.IsNullOrEmpty(e.Cell.Row.Cells["NonValidatedSymbols"].Value.ToString()))
                            {
                                dictReportData.Add("NonValidatedSymbols", e.Cell.Row.Cells["NonValidatedSymbols"].Value.ToString());
                            }
                            if (!string.IsNullOrEmpty(e.Cell.Row.Cells["ValidatedSymbols"].Value.ToString()))
                            {
                                dictReportData.Add("ValidatedSymbols", e.Cell.Row.Cells["ValidatedSymbols"].Value.ToString());
                            }
                            if (!string.IsNullOrEmpty(e.Cell.Row.Cells["EndTime"].Value.ToString()))
                            {
                                dictReportData.Add("Date", e.Cell.Row.Cells["EndTime"].Value.ToString());
                            }
                            if (!string.IsNullOrEmpty(e.Cell.Row.Cells["AccountCount"].Value.ToString()))
                            {
                                dictReportData.Add("AccountCount", e.Cell.Row.Cells["AccountCount"].Value.ToString());
                            }
                            if (!string.IsNullOrEmpty(e.Cell.Row.Cells["SecApproveFailedCount"].Value.ToString()))
                            {
                                dictReportData.Add("PendingSymbols", e.Cell.Row.Cells["SecApproveFailedCount"].Value.ToString());
                            }

                            if (_frmReport == null)
                            {
                                ctrlReport = new ctrlImportReport();
                                _frmReport = new Form();
                                _frmReport.ShowIcon = false;
                                _frmReport.FormClosed += frmReport_FormClosed;
                                SetThemeAtDynamicForm(_frmReport, ctrlReport);
                                _frmReport.Size = new System.Drawing.Size(700, 500);
                                _frmReport.MinimumSize = _frmReport.Size;
                                _frmReport.StartPosition = FormStartPosition.Manual;
                            }
                            _frmReport.Text = "Import status report - " + e.Cell.Row.Cells["Task"].Value.ToString();
                            if (ctrlReport == null)
                            {
                                ctrlReport = new ctrlImportReport();
                            }
                            //Taskesult and runUpload  is sent null as they are not required while accessing through ctrlHistroicalupload



                            CustomThemeHelper.SetThemeProperties(_frmReport, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                            _frmReport.Show();
                            //Load data after showing Forms
                            ctrlReport.SetProperties(dictReportData, null, null);
                            ctrlReport.FillReport();
                            BringFormToFront(_frmReport);
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
        /// bring form at the top of the queue
        /// </summary>
        /// <param name="form">Form object</param>
        private void BringFormToFront(Form form)
        {
            try
            {
                if (form.WindowState == FormWindowState.Minimized)
                {
                    form.WindowState = FormWindowState.Normal;

                }
                form.Location = new Point(this.Location.X + 30, this.Location.Y + 30);
                form.BringToFront();
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
        /// Dispose the report form on close
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
        /// Make the rows non-editable when they initialize
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
        /// sets theme for the dynamically created form
        /// </summary>
        /// <param name="dynamicForm">From name</param>
        /// <param name="control">Control</param>
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
        /// Close the dynamically created form
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

        private void dtToDate_AfterCloseUp(object sender, EventArgs e)
        {
            try
            {
                DateTime endDate = DateTime.Parse(dtToDate.Value.ToString());
                DateTime startDate = DateTime.Parse(dtFromDate.Value.ToString());
                if (endDate < startDate)
                {
                    MessageBox.Show("End Date cannot be less than the start date", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtToDate.Value = dtFromDate.Value;
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
    }
}
