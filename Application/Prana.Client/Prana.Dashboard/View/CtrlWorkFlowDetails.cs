using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.ClientCommon;
using Prana.Dashboard.BLL;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.Dashboard.View
{
    public partial class CtrlWorkFlowDetails : UserControl
    {
        static WorkFlowDetailsLayout _WorkFlowDetailsLayout = null;
        static string _WorkFlowDetailsLayoutFilePath = string.Empty;
        static string _WorkFlowDetailsLayoutDirectoryPath = string.Empty;
        static int _userID = int.MinValue;

        public CtrlWorkFlowDetails()
        {
            InitializeComponent();
        }
        public static WorkFlowDetailsLayout WorkFlowDetailsLayout
        {
            get
            {
                if (_WorkFlowDetailsLayout == null)
                {
                    _WorkFlowDetailsLayout = GetWorkFlowDetailsLayout();
                }
                return _WorkFlowDetailsLayout;
            }
        }
        /// <summary>
        /// bind data to UI
        /// </summary>
        /// <param name="list"></param>
        internal void FillData(List<WorkflowItem> list)
        {
            try
            {
                DataSet ds = GeneralUtilities.CreateDataSetFromCollection(list, null);
                DataTable detailsDT = ds.Tables[0];
                if (!detailsDT.Columns.Contains("FundName"))
                    detailsDT.Columns.Add("FundName");

                if (!detailsDT.Columns.Contains("Workflow"))
                    detailsDT.Columns.Add("Workflow");

                if (!detailsDT.Columns.Contains("Status"))
                    detailsDT.Columns.Add("Status");

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int accountId = Convert.ToInt32(row["FundID"].ToString());

                    NirvanaTaskStatus taskStatus = (NirvanaTaskStatus)Enum.ToObject(typeof(NirvanaTaskStatus), Convert.ToInt32(row["StatusID"].ToString()));
                    NirvanaWorkFlows workFlow = (NirvanaWorkFlows)Enum.ToObject(typeof(NirvanaWorkFlows), Convert.ToInt32(row["EventID"].ToString()));
                    row["AccountName"] = CommonDataCache.CachedDataManager.GetInstance.GetAccountText(accountId);
                    row["Workflow"] = workFlow;
                    row["Status"] = taskStatus;

                }


                grdWorkflowDetails.DataSource = detailsDT;
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
        /// Set layout of grid columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdWorkflowDetails_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.Override.RowSelectors = DefaultableBoolean.True;
                e.Layout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
                grdWorkflowDetails.DisplayLayout.Override.AllowGroupBy = DefaultableBoolean.True;
                UltraGridBand grdDataBand = null;
                grdDataBand = grdWorkflowDetails.DisplayLayout.Bands[0];

                if (WorkFlowDetailsLayout.workFlowGridColumns.Count > 0)
                {
                    List<ColumnData> listArchiveColData = WorkFlowDetailsLayout.workFlowGridColumns;
                    SetGridColumnLayout(grdWorkflowDetails, listArchiveColData);
                    foreach (string col in AllWorkFlowGridColumns)
                    {
                        if (grdDataBand.Columns.Exists(col))
                        {
                            UltraGridColumn column = grdDataBand.Columns[col];
                            column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                        }
                    }
                }
                else
                {
                    //load default layout
                    SetColumnsForWorkFlowGrid(grdWorkflowDetails.DisplayLayout.Bands[0]);

                }
                // load the saveout file if it exists
                //LoadReportSaveLayoutXML();
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
        private void SetColumnsForWorkFlowGrid(UltraGridBand gridBand)
        {
            try
            {
                Infragistics.Win.UltraWinGrid.ColumnsCollection columns = gridBand.Columns;

                UltraWinGridUtils.SetColumns(DisplayableWorkFlowGridColumns, grdWorkflowDetails);
                foreach (UltraGridColumn col in columns)
                {
                    col.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                }

                foreach (string col in AllWorkFlowGridColumns)
                {

                    if (columns.Exists(col))
                    {
                        UltraGridColumn column = columns[col];
                        column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                        column.CellActivation = Activation.NoEdit;
                    }
                }

                int visiblePosition = 0;


                UltraGridColumn ColAccountName = gridBand.Columns["FundName"];
                ColAccountName.Header.VisiblePosition = visiblePosition++;
                ColAccountName.Header.Column.Width = 100;
                ColAccountName.Header.Caption = "Account Name";
                ColAccountName.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColAccountName.NullText = String.Empty;
                ColAccountName.Hidden = false;

                UltraGridColumn ColContextValue = gridBand.Columns["ContextValue"];
                ColContextValue.Header.VisiblePosition = visiblePosition++;
                ColContextValue.Header.Column.Width = 200;
                ColContextValue.Header.Caption = "Format Name";
                ColContextValue.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColContextValue.NullText = String.Empty;
                ColContextValue.SortIndicator = SortIndicator.Ascending;
                ColContextValue.Hidden = false;

                UltraGridColumn ColWorkflow = gridBand.Columns["Workflow"];
                ColWorkflow.Header.VisiblePosition = visiblePosition++;
                ColWorkflow.Header.Column.Width = 100;
                ColWorkflow.Header.Caption = "Work flow";
                ColWorkflow.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColWorkflow.SortIndicator = SortIndicator.Ascending;
                ColWorkflow.NullText = String.Empty;
                ColWorkflow.Hidden = false;


                UltraGridColumn ColStatus = gridBand.Columns["Status"];
                ColStatus.Header.VisiblePosition = visiblePosition++;
                ColStatus.Header.Column.Width = 100;
                ColStatus.Header.Caption = "Status";
                ColStatus.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColStatus.NullText = String.Empty;
                ColStatus.Hidden = false;


                UltraGridColumn ColEventRunTime = gridBand.Columns["EventRunTime"];
                ColEventRunTime.Header.VisiblePosition = visiblePosition++;
                ColEventRunTime.Header.Column.Width = 150;
                ColEventRunTime.Header.Caption = "Run Time";
                ColEventRunTime.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColEventRunTime.NullText = String.Empty;
                ColEventRunTime.Hidden = false;

                UltraGridColumn ColFileExecutionDate = gridBand.Columns["FileExecutionDate"];
                ColFileExecutionDate.Header.VisiblePosition = visiblePosition++;
                ColFileExecutionDate.Header.Column.Width = 150;
                ColFileExecutionDate.Header.Caption = "File Execution Date";
                ColFileExecutionDate.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColFileExecutionDate.NullText = String.Empty;
                ColFileExecutionDate.Hidden = false;



                UltraGridColumn ColComments = gridBand.Columns["Comments"];
                ColComments.Header.VisiblePosition = visiblePosition++;
                ColComments.Header.Column.Width = 300;
                ColComments.Header.Caption = "Comments";
                ColComments.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColComments.NullText = String.Empty;
                ColComments.Hidden = false;

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
        private void grdWorkflowDetails_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {
                foreach (UltraGridCell cell in e.Row.Cells)
                {
                    if (!String.IsNullOrWhiteSpace(cell.Text))
                        SetAppearanceOfColumn(cell);
                }
                if (e.Row.Cells.Exists("Status") && (e.Row.Cells["Status"].Value.ToString() == NirvanaTaskStatus.Pending.ToString()))
                {
                    if (e.Row.Cells.Exists("FileExecutionDate"))
                    {
                        e.Row.Cells["FileExecutionDate"].Value = string.Empty;
                    }
                    if (e.Row.Cells.Exists("EventRunTime"))
                    {
                        e.Row.Cells["EventRunTime"].Value = string.Empty;
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
        /// Added By Faisal Shah
        /// List Contains all the columns that are to be Shown to the user on the grid
        /// </summary>
        public List<string> DisplayableWorkFlowGridColumns
        {
            get
            {
                List<string> workFlowGridColumns = new List<string>();
                workFlowGridColumns.Add("FundName");
                workFlowGridColumns.Add("Workflow");
                workFlowGridColumns.Add("Status");
                workFlowGridColumns.Add("EventRunTime");
                workFlowGridColumns.Add("FileExecutionDate");
                workFlowGridColumns.Add("ContextValue");
                workFlowGridColumns.Add("Comments");
                return workFlowGridColumns;
            }
        }
        /// <summary>
        /// Added By Faisal Shah
        /// List Contains all the columns that are to be Shown to the user both Visible and Hidden
        /// </summary>
        public List<string> AllWorkFlowGridColumns
        {
            get
            {
                List<string> workFlowGridColumns = new List<string>();
                workFlowGridColumns.Add("AccountName");
                workFlowGridColumns.Add("Workflow");
                workFlowGridColumns.Add("Status");
                workFlowGridColumns.Add("EventRunTime");
                workFlowGridColumns.Add("FileExecutionDate");
                workFlowGridColumns.Add("ContextValue");
                workFlowGridColumns.Add("Comments");
                return workFlowGridColumns;
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
        private static WorkFlowDetailsLayout GetWorkFlowDetailsLayout()
        {
            _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            _WorkFlowDetailsLayoutDirectoryPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID;
            _WorkFlowDetailsLayoutFilePath = _WorkFlowDetailsLayoutDirectoryPath + @"\WorkFlowDetailsLayout.xml";

            WorkFlowDetailsLayout workFlowLayout = new WorkFlowDetailsLayout();
            try
            {
                if (!Directory.Exists(_WorkFlowDetailsLayoutDirectoryPath))
                {
                    Directory.CreateDirectory(_WorkFlowDetailsLayoutDirectoryPath);
                }
                if (File.Exists(_WorkFlowDetailsLayoutFilePath))
                {
                    using (FileStream fs = File.OpenRead(_WorkFlowDetailsLayoutFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(WorkFlowDetailsLayout));
                        workFlowLayout = (WorkFlowDetailsLayout)serializer.Deserialize(fs);
                    }
                }

                _WorkFlowDetailsLayout = workFlowLayout;
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

            return workFlowLayout;
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
                if (grdWorkflowDetails != null)
                {
                    if (grdWorkflowDetails.DisplayLayout.Bands[0].Columns.Count > 0)
                    {
                        WorkFlowDetailsLayout.workFlowGridColumns = GetGridColumnLayout(grdWorkflowDetails);
                    }
                }
                SaveWorkFlowGridLayout();
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
        /// <param name="grdWorkflowDetails"></param>
        /// <returns></returns>
        private List<ColumnData> GetGridColumnLayout(UltraGrid grdWorkflowDetails)
        {
            List<ColumnData> listGridCols = new List<ColumnData>();
            UltraGridBand band = grdWorkflowDetails.DisplayLayout.Bands[0];
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
        private void SaveWorkFlowGridLayout()
        {
            try
            {

                using (XmlTextWriter writer = new XmlTextWriter(_WorkFlowDetailsLayoutFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(WorkFlowDetailsLayout));
                    serializer.Serialize(writer, _WorkFlowDetailsLayout);

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

        public static void SetGridColumnLayout(UltraGrid grid, List<ColumnData> listColData)
        {
            List<ColumnData> listSortedGridCols = new List<ColumnData>();
            UltraGridBand band = grid.DisplayLayout.Bands[0];
            ColumnsCollection gridColumns = band.Columns;// Just for readability ;)
            listColData.Sort();
            grid.DisplayLayout.ViewStyleBand = ViewStyleBand.OutlookGroupBy;
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
                        if (colData.IsGroupByColumn)
                        {
                            grid.DisplayLayout.Bands[0].SortedColumns.Add(colData.Key, false, true);
                        }

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

    }
}
