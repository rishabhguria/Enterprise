using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.AuditManager.Definitions.Constants;
using Prana.AuditManager.Definitions.Interface;
using Prana.LogManager;
using Prana.PM.BLL;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace Prana.Admin.CommonControls
{
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.BatchCreated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.BatchUpdated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.BatchApproved, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.BatchDeleted, ShowAuditUI = true)]
    public partial class CntrlBatchSetup : UserControl, IAuditSource
    {
        TaskSchedulerForm ctrlTaskScheduler = new TaskSchedulerForm();
        ValueList _vlFormat = new ValueList();
        ValueList _vlSchedule = new ValueList();
        ValueList _vlScanResult = new ValueList();
        private bool _isSaveRequired = false;

        public int _thirdPartID = 0;

        [AuditManager.Attributes.AuditSourceConstAttri]
        public CntrlBatchSetup()
        {
            try
            {
                InitializeComponent();
                //this.InitializeControl(1);
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
        /// Create the value lists for the batch setup format and the schedule 
        /// </summary>
        private void MakeFormatValueList()
        {
            try
            {
                _vlFormat.ValueListItems.Clear();
                foreach (int formatID in BatchSetupManager.dictFormat.Keys)
                {
                    _vlFormat.ValueListItems.Add(formatID, BatchSetupManager.dictFormat[formatID]);
                }

                _vlSchedule.ValueListItems.Clear();
                _vlSchedule.ValueListItems.Add(-1, "None");
                _vlSchedule.ValueListItems.Add(0, "One time");
                _vlSchedule.ValueListItems.Add(1, "Daily");
                _vlSchedule.ValueListItems.Add(2, "Weekly");
                _vlSchedule.ValueListItems.Add(3, "Monthly");

                _vlScanResult.ValueListItems.Clear();
                _vlScanResult.ValueListItems.Add(0, "None");
                _vlScanResult.ValueListItems.Add(1, "Pass");
                _vlScanResult.ValueListItems.Add(2, "Fail");
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
            //foreach (UltraGridRow ugRow in grdBatchSetup.Rows)
            //{
            //    int i = (int)ugRow.Cells["FormatName"].Value;
            //    foreach(ValueListItem vl in _vlFormat.ValueListItems)
            //    {
            //        if(i==Convert.ToInt32(vl))
            //        {
            //            _vlFormat.ValueListItems.Remove(ugRow.Cells["FormatName"].Value);
            //        }
            //    }

            //    }
        }

        /// <summary>
        /// Initialize the control when it is loaded
        /// </summary>
        /// <param name="thirdPartyID">ID of the third party</param>
        public void InitializeControl(int thirdPartyID)
        {
            try
            {
                this._thirdPartID = thirdPartyID;
                BatchSetupManager.InitializeData(thirdPartyID);
                cmbAccount.DataSource = BatchSetupManager.GetCurrentScheduleAccounts(0);
                grdBatchSetup.DataSource = BatchSetupManager.GetBatchDetails();
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
        /// Initialize the layout of the grid for display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdBatchSetup_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                // Modified by Ankit Gupta on 13 Oct, 2014.
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1587
                //Set the HeaderCheckBoxVisibility so it will display the CheckBox whenever a CheckEditor is used within the UltraGridColumn 
                e.Layout.Override.HeaderCheckBoxVisibility = HeaderCheckBoxVisibility.WhenUsingCheckEditor;
                //Set the HeaderChe.LayouteckBoxSynchronization so all rows within the GridBand will be synchronized with the CheckBox 
                e.Layout.Override.HeaderCheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                //if (grdBatchSetup.Rows.Count > 0)
                //{
                //    UltraWinGridUtils.EnableFixedFilterRow(e);
                //}
                //Set back color black and fore color white
                e.Layout.Override.ActiveRowAppearance.BackColor = Color.Black;
                e.Layout.Override.ActiveRowAppearance.BackColor2 = Color.Black;
                e.Layout.Override.ActiveRowAppearance.ForeColor = Color.White;

                e.Layout.Override.RowAppearance.BackColor = Color.Black;
                e.Layout.Override.RowAppearance.BackColor2 = Color.Black;
                e.Layout.Override.RowAppearance.ForeColor = Color.White;

                e.Layout.Override.ActiveCellAppearance.BackColor = Color.Black;
                e.Layout.Override.ActiveCellAppearance.BackColor2 = Color.Black;
                e.Layout.Override.ActiveCellAppearance.ForeColor = Color.White;
                //added by: Bharat raturi, 25 apr 2014
                //purpose: allow row filtering
                //e.Layout.Override.AllowRowFiltering = DefaultableBoolean.True;
                foreach (UltraGridColumn column in e.Layout.Bands[0].Columns)
                {
                    //following line auto adjust width of columns of ultragrid according to text size of header.
                    column.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All);
                }

                MakeFormatValueList();

                if (cmbAccount.DataSource != null)
                {
                    if (!cmbAccount.DisplayLayout.Bands[0].Columns.Exists("Selected"))
                    {
                        UltraGridColumn colSelectAccount = cmbAccount.DisplayLayout.Bands[0].Columns.Add();
                        colSelectAccount.Key = "Selected";
                        colSelectAccount.Header.Caption = String.Empty;
                        colSelectAccount.Width = 25;
                        colSelectAccount.CellActivation = Activation.Disabled;
                        colSelectAccount.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                        colSelectAccount.DataType = typeof(bool);
                        colSelectAccount.Header.VisiblePosition = 1;
                    }
                    //cmbAccount.Enabled = false;
                    cmbAccount.CheckedListSettings.CheckStateMember = "Selected";
                    cmbAccount.CheckedListSettings.EditorValueSource = EditorWithComboValueSource.CheckedItems;
                    cmbAccount.ReadOnly = true;
                    cmbAccount.CheckedListSettings.ListSeparator = " , ";
                    cmbAccount.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
                    cmbAccount.DisplayMember = "AccountName";
                    cmbAccount.ValueMember = "AccountID";
                    //cmbAccount.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Select All";
                    cmbAccount.DisplayLayout.Bands[0].Columns[0].Hidden = true;
                }

                //Grid layout 
                UltraGridBand band = e.Layout.Bands[0];
                if (!band.Columns.Exists("SelectBatch"))
                {
                    UltraGridColumn colSelect = band.Columns.Add("SelectBatch");
                    colSelect.Header.Caption = String.Empty;
                    colSelect.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    colSelect.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colSelect.DataType = typeof(bool);
                    colSelect.DefaultCellValue = false;
                    colSelect.Header.VisiblePosition = 1;
                    colSelect.Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center;
                    colSelect.Width = 15;
                }

                if (!band.Columns.Exists("EditBatch"))
                {
                    UltraGridColumn colEdit = band.Columns.Add("EditBatch");
                    colEdit.Header.Caption = "EditBatch";
                    colEdit.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    colEdit.NullText = "Edit";
                    colEdit.Header.Caption = "Edit Schedule";
                    colEdit.Header.Appearance.TextHAlign = HAlign.Center;
                    colEdit.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colEdit.Header.VisiblePosition = 13;
                }

                UltraGridColumn colFormatName = band.Columns["FormatName"];
                colFormatName.Header.Caption = "Format Name";
                colFormatName.Header.Appearance.TextHAlign = HAlign.Center;
                colFormatName.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                colFormatName.CellActivation = Activation.NoEdit;
                colFormatName.Header.VisiblePosition = 2;
                //colFormatName.ValueList = _vlFormat;

                UltraGridColumn colAccount = band.Columns["Account"];
                colAccount.Header.Caption = "Accounts";
                colAccount.Header.Appearance.TextHAlign = HAlign.Center;
                colAccount.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colAccount.EditorComponent = cmbAccount;
                colAccount.CellActivation = Activation.ActivateOnly;
                colAccount.Header.VisiblePosition = 3;

                UltraGridColumn colThirdPartyType = band.Columns["ThirdPartyType"];
                colThirdPartyType.Header.Caption = "Third Party Type";
                colThirdPartyType.Header.Appearance.TextHAlign = HAlign.Center;
                colThirdPartyType.CellActivation = Activation.NoEdit;
                colThirdPartyType.Header.VisiblePosition = 4;

                UltraGridColumn colEnableTolerance = band.Columns["EnablePriceTolerance"];
                colEnableTolerance.Header.Caption = "Enable Price Tolerance";
                colEnableTolerance.Header.Appearance.TextHAlign = HAlign.Center;
                //colEnableTolerance.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colEnableTolerance.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                //colEnableTolerance.CellActivation = Activation.NoEdit;
                colEnableTolerance.Header.VisiblePosition = 5;

                UltraGridColumn colPrice = band.Columns["PriceCheckTolerance"];
                colPrice.Header.Caption = "Price Check Tolerance";
                colPrice.Header.Appearance.TextHAlign = HAlign.Center;
                colPrice.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Double;
                //colPrice.CellActivation = Activation.NoEdit;
                colPrice.Header.VisiblePosition = 6;

                UltraGridColumn colSchedule = band.Columns["Schedule"];
                colSchedule.Header.Caption = "Schedule";
                colSchedule.Header.Appearance.TextHAlign = HAlign.Center;
                colSchedule.ValueList = _vlSchedule;
                colSchedule.CellActivation = Activation.NoEdit;
                colSchedule.Header.VisiblePosition = 7;

                UltraGridColumn colExecTime = band.Columns["ExecTime"];
                colExecTime.Header.Caption = "Execution Time";
                colExecTime.Header.Appearance.TextHAlign = HAlign.Center;
                colExecTime.CellActivation = Activation.NoEdit;
                colExecTime.Header.VisiblePosition = 8;

                UltraGridColumn colAutoExecution = band.Columns["AutoExec"];
                colAutoExecution.Header.Caption = "Auto Execution";
                colAutoExecution.Header.Appearance.TextHAlign = HAlign.Center;
                colAutoExecution.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colAutoExecution.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                //colAutoExecution.CellActivation = Activation.NoEdit;
                colAutoExecution.Header.VisiblePosition = 9;

                UltraGridColumn colnxtExecTime = band.Columns["NxtExecTime"];
                colnxtExecTime.Header.Caption = "Next Execution Time";
                colnxtExecTime.Header.Appearance.TextHAlign = HAlign.Center;
                colnxtExecTime.CellActivation = Activation.NoEdit;
                colnxtExecTime.Header.VisiblePosition = 10;

                UltraGridColumn colLastExecTime = band.Columns["LastExecTime"];
                colLastExecTime.Header.Caption = "Last Execution Time";
                colLastExecTime.Header.Appearance.TextHAlign = HAlign.Center;
                colLastExecTime.CellActivation = Activation.NoEdit;
                colLastExecTime.Header.VisiblePosition = 11;

                UltraGridColumn colLastScanResult = band.Columns["LastScanResult"];
                colLastScanResult.Header.Caption = "Last Scan Result";
                colLastScanResult.Header.Appearance.TextHAlign = HAlign.Center;
                colLastScanResult.CellActivation = Activation.NoEdit;
                colLastScanResult.ValueList = _vlScanResult;
                colLastScanResult.Header.VisiblePosition = 12;

                band.Columns["BatchID"].Hidden = true;
                band.Columns["BatchID"].Header.Appearance.TextHAlign = HAlign.Center;
                band.Columns["CronExpression"].Hidden = true;
                band.Columns["CronExpression"].Header.Appearance.TextHAlign = HAlign.Center;

                //foreach (UltraGridRow row in grdBatchSetup.Rows)
                //{
                //    row.Activation = Activation.NoEdit;
                //    row.Cells["SelectBatch"].Activation = Activation.AllowEdit;
                //    row.Cells["EditBatch"].Activation = Activation.AllowEdit;
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
        /// Add the new row to the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    grdBatchSetup.DisplayLayout.Bands[0].AddNew();
            //}
            //catch (Exception ex)
            //{
            //    // Invoke our policy that is responsible for making sure no secure information
            //    // gets out of our layer.
            //    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

            //    if (rethrow)
            //    {
            //        throw;
            //    }
            //}
            this.SaveBatchDetails();
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Create accounts list for the selected batch once the drop down of the account combo is activated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAccount_AfterDropDown(object sender, EventArgs e)
        {
            //try
            //{
            //    int scheduleID = Convert.ToInt32(grdBatchSetup.ActiveRow.Cells["BatchID"].Value);
            //    cmbAccount.DataSource = BatchSetupManager.GetCurrentScheduleAccounts(scheduleID);
            //}
            //catch (Exception ex)
            //{
            //    // Invoke our policy that is responsible for making sure no secure information
            //    // gets out of our layer.
            //    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            //    if (rethrow)
            //    {
            //        throw;
            //    }
            //}
        }

        /// <summary>
        /// Show the task scheduler on the click of the edit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdBatchSetup_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key == "EditBatch")
                {
                    //TaskSchedulerForm taskForm = new TaskSchedulerForm();
                    //ctrlTaskScheduler = new TaskSchedulerForm();
                    if (grdBatchSetup.ActiveRow.Cells["CronExpression"].Value != DBNull.Value)
                    {
                        string cronExp = Convert.ToString(grdBatchSetup.ActiveRow.Cells["CronExpression"].Value);
                        ctrlTaskScheduler.GetCronToFill(cronExp);
                    }
                    ctrlTaskScheduler.ShowDialog(this.Parent);
                    DialogResult dr = ctrlTaskScheduler.DialogResult;
                    if (dr == DialogResult.OK)
                    {
                        string cronExp = ctrlTaskScheduler.GetCronExpression();
                        grdBatchSetup.ActiveRow.Cells["CronExpression"].Value = cronExp;
                        int batchID = int.Parse(grdBatchSetup.ActiveRow.Cells["BatchID"].Value.ToString());
                        foreach (DataRow row in ((DataTable)grdBatchSetup.DataSource).Rows)
                        {
                            if (batchID == int.Parse(row["BatchID"].ToString()))
                            {
                                BatchSetupManager.FillCronDetails(cronExp, row);
                            }
                        }
                        if (!_isSaveRequired)
                        {
                            _isSaveRequired = true;
                        }
                        //CronDescription cronDetail=CronUtility.GetCronDescriptionObject(cronExp);
                        //int schedule = (int)cronDetail.Type;
                        //DateTime startTime = Convert.ToDateTime(cronDetail.StartDate.ToString() + cronDetail.StartTime.ToString());
                        //DateTime nxtExecTime=startTime
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
        /// Save the details of the batch in the database
        /// </summary>
        /// <returns>The number of affected records in the database</returns>
        public int SaveBatchDetails()
        {
            int i = 0;
            try
            {
                if (!_isSaveRequired)
                {
                    return 1;
                }
                DataTable dt = (DataTable)grdBatchSetup.DataSource;
                i = BatchSetupManager.SaveBatchDetails(dt);
                AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, AuditBatchData(dt), AuditManager.Definitions.Enum.AuditAction.BatchUpdated);
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
            return i;
        }

        /// <summary>
        /// Function to get Batch details dictionary
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        private Dictionary<String, List<String>> AuditBatchData(DataTable dtBatch)
        {
            Dictionary<String, List<String>> auditDataForBatch = new Dictionary<string, List<string>>();
            List<int> batchIdList = new List<int>();
            try
            {
                auditDataForBatch.Add(CustomAuditSourceConstants.AuditSourceTypeBatch, new List<string>());
                auditDataForBatch[CustomAuditSourceConstants.AuditSourceTypeBatch].Add(_thirdPartID.ToString());
                batchIdList = (from row in dtBatch.AsEnumerable()
                               select row.Field<int>("BatchID")).ToList();

                foreach (int id in batchIdList)
                {
                    auditDataForBatch[CustomAuditSourceConstants.AuditSourceTypeBatch].Add(id.ToString());
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
            return auditDataForBatch;
        }

        private void cmbAccount_AfterCloseUp(object sender, EventArgs e)
        {
            //try
            //{
            //    int i = int.Parse(grdBatchSetup.ActiveRow.Cells["BatchID"].Value.ToString());
            //    foreach (DataRow row in ((DataTable)grdBatchSetup.DataSource).Rows)
            //    {
            //        if (i ==Convert.ToInt32(row["BatchID"]))
            //        {
            //            grdBatchSetup.ActiveRow.Cells["Account"].Value = row["Account"];
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // Invoke our policy that is responsible for making sure no secure information
            //    // gets out of our layer.
            //    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            //    if (rethrow)
            //    {
            //        throw;
            //    }
            //}
        }

        private void cmbAccount_MouseLeave(object sender, EventArgs e)
        {
            //try
            //{
            //    int i = int.Parse(grdBatchSetup.ActiveRow.Cells["BatchID"].Value.ToString());
            //    foreach (DataRow row in ((DataTable)grdBatchSetup.DataSource).Rows)
            //    {
            //        if (i == Convert.ToInt32(row["BatchID"]))
            //        {
            //            grdBatchSetup.ActiveRow.Cells["Account"].Value = row["Account"];
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // Invoke our policy that is responsible for making sure no secure information
            //    // gets out of our layer.
            //    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            //    if (rethrow)
            //    {
            //        throw;
            //    }
            //}
        }

        private void cmbAccount_MouseEnter(object sender, EventArgs e)
        {
            //try
            //{
            //    int scheduleID = Convert.ToInt32(grdBatchSetup.ActiveRow.Cells["BatchID"].Value);
            //    cmbAccount.DataSource = BatchSetupManager.GetCurrentScheduleAccounts(scheduleID);
            //}
            //catch (Exception ex)
            //{
            //    // Invoke our policy that is responsible for making sure no secure information
            //    // gets out of our layer.
            //    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            //    if (rethrow)
            //    {
            //        throw;
            //    }
            //}
        }

        /// <summary>
        /// To find batch id of currently active row
        /// </summary>
        /// <returns></returns>
        private void grdBatchSetup_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                if (this.grdBatchSetup.ActiveRow != null)
                {
                    //UltraGridRow activeRow = this.grdBatchSetup.ActiveRow;
                    //int batchID = Convert.ToInt32(activeRow.Cells["BatchID"].Value);
                    GetBatchID();
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
        public List<string> getFormatName()
        {
            List<string> formatName = new List<string>();
            foreach (UltraGridRow row in grdBatchSetup.Rows)
            {
                if ((bool)row.Cells["SelectBatch"].Value == true)
                {
                    formatName.Add(row.Cells["FormatName"].Value.ToString());
                }
            }
            return formatName;
        }

        /// <summary>
        /// Function to set AuditRefresh attribute on batchID
        /// </summary>
        /// <param name="batchID"></param>
        [AuditManager.Attributes.AuditRefreshMethAttri(AuditManager.Attributes.AuditMehodType.Arguments, 0)]
        public void GetBatchID() { }

        private void grdBatchSetup_CellChange(object sender, CellEventArgs e)
        {
            if (!_isSaveRequired)
            {
                _isSaveRequired = true;
            }
        }

        private void grdBatchSetup_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {

                e.Cancel = true;
                if (grdBatchSetup.DataSource != null)
                {
                    (this.FindForm()).AddCustomColumnChooser(this.grdBatchSetup);
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

        private void grdBatchSetup_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();

        }
    }
}
