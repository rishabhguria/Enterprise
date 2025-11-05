using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Prana.Dashboard
{
    public partial class PranaGridControl : UserControl
    {
        public PranaGridControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// to handle to show account wise workflow details
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="workflow"></param>
        /// <param name="Date"></param>
        internal delegate void ShowWorkFlowDetails(int accountID, NirvanaWorkFlows workflow, DateTime Date);
        internal event ShowWorkFlowDetails ShowDetailsHandler;

        static int _userID = int.MinValue;
        static string _masterDashboardFilePath = string.Empty;
        static string _masterDashboardLayoutDirectoryPath = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dashboradCol"></param>
        internal void SetGridData(List<MasterDashboardUIObj> dashboradCol)
        {
            try
            {
                grdMasterDashboard.DataSource = null;
                grdMasterDashboard.DataSource = dashboradCol;
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
        /// <param name="accountID"></param>
        /// <param name="workflow"></param>
        /// <param name="status"></param>
        internal void UpdateStatusOnDashboard(int accountID, NirvanaWorkFlows workflow, NirvanaTaskStatus status)
        {
            try
            {
                List<MasterDashboardUIObj> col = grdMasterDashboard.DataSource as List<MasterDashboardUIObj>;
                if (col != null && col.Count > 0)
                {
                    foreach (UltraGridRow row in grdMasterDashboard.Rows)
                    {
                        if (Convert.ToInt32(row.Cells["FundID"].Text) == accountID)
                        {
                            #region Update column
                            switch (workflow)
                            {
                                case NirvanaWorkFlows.FileUpload:
                                    row.Cells["FileUploadStatus"].Value = status;
                                    break;
                                case NirvanaWorkFlows.Import:
                                    row.Cells["ImportStatus"].Value = status;
                                    break;
                                case NirvanaWorkFlows.SMValidation:
                                    row.Cells["SMValidationStatus"].Value = status;
                                    break;

                                case NirvanaWorkFlows.Recon:
                                    row.Cells["ReconStatus"].Value = status;
                                    break;

                                case NirvanaWorkFlows.CnclAmnd:
                                    row.Cells["CnclAmndStatus"].Value = status;
                                    break;

                                case NirvanaWorkFlows.MarkPrice:
                                    row.Cells["MarkPriceStatus"].Value = status;
                                    break;

                                case NirvanaWorkFlows.Closing:
                                    row.Cells["ClosingStatus"].Value = status;
                                    break;

                                case NirvanaWorkFlows.Reporting:
                                    row.Cells["ReportingStatus"].Value = status;
                                    break;
                            }
                            #endregion
                            break;
                        }
                    }
                    grdMasterDashboard.Refresh();
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
        /// Set layout of grid columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMasterDashboard_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand grdDataBand = null;
                grdDataBand = grdMasterDashboard.DisplayLayout.Bands[0];

                if (grdMasterDashboard.DataSource != null)
                {
                    e.Layout.Override.RowSelectors = DefaultableBoolean.True;
                    e.Layout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;

                    SetGridColumns(grdDataBand);

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
        /// Set layout of grid columns
        /// </summary>
        /// <param name="gridBand"></param>
        private void SetGridColumns(UltraGridBand gridBand)
        {
            try
            {
                foreach (UltraGridColumn column in gridBand.Columns)
                {
                    column.CellActivation = Activation.NoEdit;
                    column.Hidden = true;
                    // column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    // column.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.IncludeHeader);
                }

                int visiblePosition = 0;


                UltraGridColumn ColAccountName = gridBand.Columns["FundName"];
                ColAccountName.Header.VisiblePosition = visiblePosition++;
                ColAccountName.Header.Column.Width = 100;
                ColAccountName.Header.Caption = "Account Name";
                ColAccountName.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColAccountName.NullText = String.Empty;
                ColAccountName.Hidden = false;

                UltraGridColumn ColCompanyName = gridBand.Columns["CompanyName"];
                ColCompanyName.Header.VisiblePosition = visiblePosition++;
                ColCompanyName.Header.Column.Width = 100;
                ColCompanyName.Header.Caption = "Client Name";
                ColCompanyName.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColCompanyName.NullText = String.Empty;
                ColCompanyName.Hidden = false;

                UltraGridColumn ColThirdPartyName = gridBand.Columns["ThirdPartyName"];
                ColThirdPartyName.Header.VisiblePosition = visiblePosition++;
                ColThirdPartyName.Header.Column.Width = 100;
                ColThirdPartyName.Header.Caption = "Third Party Name";
                ColThirdPartyName.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColThirdPartyName.NullText = String.Empty;
                ColThirdPartyName.Hidden = false;

                UltraGridColumn ColImportStatus = gridBand.Columns["ImportStatus"];
                ColImportStatus.Header.VisiblePosition = visiblePosition++;
                ColImportStatus.Header.Column.Width = 100;
                ColImportStatus.Header.Caption = "Batch Status";
                ColImportStatus.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColImportStatus.NullText = String.Empty;
                ColImportStatus.Hidden = false;


                UltraGridColumn ColFileUploadStatus = gridBand.Columns["FileUploadStatus"];
                ColFileUploadStatus.Header.VisiblePosition = visiblePosition++;
                ColFileUploadStatus.Header.Column.Width = 100;
                ColFileUploadStatus.Header.Caption = "File Retrieval Status";
                ColFileUploadStatus.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColFileUploadStatus.NullText = String.Empty;
                ColFileUploadStatus.Hidden = false;

                UltraGridColumn ColImportIntoAppStatus = gridBand.Columns["ImportIntoAppStatus"];
                ColImportIntoAppStatus.Header.VisiblePosition = visiblePosition++;
                ColImportIntoAppStatus.Header.Column.Width = 150;
                ColImportIntoAppStatus.Header.Caption = "Import Into App Status";
                ColImportIntoAppStatus.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColImportIntoAppStatus.NullText = String.Empty;
                ColImportIntoAppStatus.Hidden = false;

                UltraGridColumn ColSMValidationStatus = gridBand.Columns["SMValidationStatus"];
                ColSMValidationStatus.Header.VisiblePosition = visiblePosition++;
                ColSMValidationStatus.Header.Column.Width = 150;
                ColSMValidationStatus.Header.Caption = "SM Validation Status";
                ColSMValidationStatus.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColSMValidationStatus.NullText = String.Empty;
                ColSMValidationStatus.Hidden = false;

                UltraGridColumn colReconStatus = gridBand.Columns["ReconStatus"];
                // colReconStatus.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                colReconStatus.Width = 100;
                colReconStatus.Header.VisiblePosition = visiblePosition++;
                colReconStatus.Header.Caption = "Recon Status";
                colReconStatus.NullText = String.Empty;
                //  colReconStatus.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colReconStatus.Hidden = false;


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
        /// Set Color of cell based on their status
        /// </summary>
        /// <param name="cell"></param>
        private void SetAppearanceOfColumn(UltraGridCell cell)
        {
            try
            {
                NirvanaTaskStatus status;
                bool isParsed = Enum.TryParse(cell.Text, out status);
                if (isParsed)
                {
                    switch (status)
                    {
                        case NirvanaTaskStatus.Completed:
                        case NirvanaTaskStatus.Success:
                            cell.Appearance.BackColor = Color.Green;
                            cell.ActiveAppearance.BackColor = Color.Green;
                            cell.ButtonAppearance.BackColor = Color.Green;
                            break;
                        case NirvanaTaskStatus.Failure:
                        case NirvanaTaskStatus.Canceled:
                            cell.Appearance.BackColor = Color.Red;
                            cell.ActiveAppearance.BackColor = Color.Red;
                            cell.ButtonAppearance.BackColor = Color.Red;
                            break;

                        case NirvanaTaskStatus.Importing:
                        case NirvanaTaskStatus.PartialSuccess:
                        case NirvanaTaskStatus.PendingCompleted:
                        case NirvanaTaskStatus.Running:
                            // case NirvanaTaskStatus.Pending:
                            cell.Appearance.BackColor = Color.Yellow;
                            cell.ActiveAppearance.BackColor = Color.Yellow;
                            cell.ButtonAppearance.BackColor = Color.Yellow;
                            cell.ActiveAppearance.ForeColor = Color.Black;
                            cell.Appearance.ForeColor = Color.Black;

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

        /// <summary>
        /// Handle work flow status view 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMasterDashboard_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {

                foreach (UltraGridCell cell in e.Row.Cells)
                {
                    if (!String.IsNullOrWhiteSpace(cell.Text))
                        SetAppearanceOfColumn(cell);

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
        /// Handle click cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMasterDashboard_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
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
        /// Show accounts workflow details handling  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowDetailsHandler != null)
                {
                    if (grdMasterDashboard.ActiveRow != null)
                    {
                        // Modified by :sachin mishra 
                        //pusrpose for showing the alert message when no row selected http://jira.nirvanasolutions.com:8080/browse/CHMW-2500
                        if (grdMasterDashboard.ActiveRow.Cells != null && grdMasterDashboard.ActiveRow.Cells.Exists("FundID") && grdMasterDashboard.ActiveRow.Cells.Exists("ExecutionDate"))
                        {
                            int accountID;
                            int.TryParse(grdMasterDashboard.ActiveRow.Cells["FundID"].Text, out accountID);
                            DateTime date = DateTime.MinValue;
                            DateTime.TryParse(grdMasterDashboard.ActiveRow.Cells["ExecutionDate"].Text, out date);
                            ShowDetailsHandler(accountID, NirvanaWorkFlows.Import, date);
                        }
                        else
                            MessageBox.Show("No row selected!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
        /// focus on selected row near to mouse pointer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMasterDashboard_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    Point mousePoint = new Point(e.X, e.Y);
                    UIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(mousePoint);
                    UltraGridCell cell = element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
                    if (cell != null)
                    {
                        cell.Row.Activate();
                    }
                }
            }
            catch (Exception)
            {
                //Do Nothing as user can try again
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Saving to Excel file. This launches the Save dialog for the user to select the Save Path
                if (grdMasterDashboard.Rows.Count > 0)
                {
                    CreateExcel(ExcelUtilities.FindSavePathForExcel());
                }
                else
                    MessageBox.Show("No data to Export", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

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
            finally
            {
                //Any cleanup code
                this.Cursor = Cursors.Default;
            }
        }

        private void CreateExcel(String filepath)
        {
            try
            {
                if (filepath != null)
                {

                    ultraGridExcelExporter1.Export(grdMasterDashboard, filepath);
                    MessageBox.Show("Grid data successfully downloaded to " + filepath, "Dashboard Excel Exporter", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public event EventHandler RefreshData;
        private void refreshDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (RefreshData != null)
                {

                    RefreshData(this, null);
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

        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdMasterDashboard != null)
                {
                    if (grdMasterDashboard.DisplayLayout.Bands[0].Columns.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(_masterDashboardFilePath))
                        {
                            grdMasterDashboard.DisplayLayout.SaveAsXml(_masterDashboardFilePath);
                            MessageBox.Show(this, "Layout Saved.", "Master Dashboard", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        /// <summary>
        /// Load report layout xml if file exist
        /// </summary>
        private void LoadReportSaveLayoutXML()
        {
            try
            {
                _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                _masterDashboardLayoutDirectoryPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID;
                _masterDashboardFilePath = _masterDashboardLayoutDirectoryPath + @"\MasterDashboardLayout.xml";

                if (!Directory.Exists(_masterDashboardLayoutDirectoryPath))
                {
                    Directory.CreateDirectory(_masterDashboardLayoutDirectoryPath);
                }
                if (File.Exists(_masterDashboardFilePath))
                {
                    grdMasterDashboard.DisplayLayout.LoadFromXml(_masterDashboardFilePath);
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
