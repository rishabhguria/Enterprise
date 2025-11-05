using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects.Classes.ThirdParty;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.Admin.Controls.ThirdParty
{
    public partial class TimedBatchSchedulingForm : Form
    {
        private const string CONST_SERIAL_NO_KEY = "Id";
        private const string CONST_DELETE = "Delete";
        private const string CONST_PAUSE_RESUME = "PauseResume";

        public List<ThirdPartyTimeBatch> UpdatedTimeBatches = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedBatchSchedulingForm"/> class.
        /// </summary>
        public TimedBatchSchedulingForm()
        {
            try
            {
                InitializeComponent();
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
        /// Sets the time batches for the grid
        /// </summary>
        /// <param name="timeBatches"></param>
        public void BindGridData(List<ThirdPartyTimeBatch> timeBatches)
        {
            try
            {
                List<ThirdPartyTimeBatch> grdDataSource = DeepCopyHelper.Clone(timeBatches);
                grdTimedBatch.DataSource = grdDataSource;
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
        /// Handles the Load event for the TimedBatch form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TimedBatch_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                InitializeGridColumns();
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
        /// Configures and binds the data to the grdTimedBatch grid. 
        /// </summary>
        private void InitializeGridColumns()
        {
            try
            {
                // Configure the ID column
                var idColumn = grdTimedBatch.DisplayLayout.Bands[0].Columns.Add(CONST_SERIAL_NO_KEY, "S.No");
                idColumn.Width = 10;
                idColumn.CellActivation = Activation.NoEdit;
                idColumn.Header.VisiblePosition = 0;
                idColumn.Header.Appearance.TextHAlign = HAlign.Center;
                idColumn.CellAppearance.TextHAlign = HAlign.Center;
                idColumn.Header.Appearance.FontData.Bold = DefaultableBoolean.True;

                // Configure the BatchRunTime column
                var timeColumn = grdTimedBatch.DisplayLayout.Bands[0].Columns["BatchRunTime"];
                timeColumn.Header.Caption = "Scheduled Time (EST)";
                timeColumn.Width = 50;
                timeColumn.Header.VisiblePosition = 1;
                timeColumn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.TimeWithSpin;
                timeColumn.Header.Appearance.TextHAlign = HAlign.Center;
                timeColumn.CellAppearance.TextHAlign = HAlign.Center;
                timeColumn.MaskInput = "hh:mm:ss EST";
                timeColumn.Header.Appearance.FontData.Bold = DefaultableBoolean.True;

                // Configure the Pause Resume column
                var pauseResumeColumn = grdTimedBatch.DisplayLayout.Bands[0].Columns.Add(CONST_PAUSE_RESUME, "");
                pauseResumeColumn.Width = 20;
                pauseResumeColumn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                pauseResumeColumn.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                pauseResumeColumn.CellButtonAppearance.Image = Properties.Resources.btn_Pause;
                pauseResumeColumn.CellButtonAppearance.ImageHAlign = HAlign.Center;
                pauseResumeColumn.CellButtonAppearance.ImageVAlign = VAlign.Middle;

                // Configure the Delete column
                var deleteColumn = grdTimedBatch.DisplayLayout.Bands[0].Columns.Add(CONST_DELETE, "");
                deleteColumn.Width = 20;
                deleteColumn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                deleteColumn.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                deleteColumn.CellButtonAppearance.Image = Properties.Resources.btn_delete;
                deleteColumn.CellButtonAppearance.ImageHAlign = HAlign.Center;
                deleteColumn.CellButtonAppearance.ImageVAlign = VAlign.Middle;
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
        /// Intialize the row when added to the grdTimedBatch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdTimedBatch_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.Cells.Exists(CONST_SERIAL_NO_KEY))
                {
                    e.Row.Cells[CONST_SERIAL_NO_KEY].Value = e.Row.Index + 1;
                }
                if (e.Row.Cells.Exists(CONST_PAUSE_RESUME) && e.Row.ListObject != null)
                {
                    e.Row.Cells[CONST_PAUSE_RESUME].ButtonAppearance.Image = ((ThirdPartyTimeBatch)e.Row.ListObject).IsPaused ? Properties.Resources.btn_Resume : Properties.Resources.btn_Pause;
                    e.Row.Cells[CONST_PAUSE_RESUME].ToolTipText = ((ThirdPartyTimeBatch)e.Row.ListObject).IsPaused ? "Resume" : "Pause";
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
        /// This method added the row in form when clicked the add button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var timeBatches = grdTimedBatch.DataSource as List<ThirdPartyTimeBatch>;
                if (timeBatches != null)
                {
                    timeBatches.Add(new ThirdPartyTimeBatch
                    {
                        ID = int.MinValue,
                        BatchRunTime = BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, BusinessObjects.TimeZoneInfo.EasternTimeZone),
                        IsPaused = false
                    });
                    grdTimedBatch.DataBind();
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
        /// Handles the click event on a cell button in the grdTimedBatch grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdTimedBatch_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                var grdDataSource = grdTimedBatch.DataSource as List<ThirdPartyTimeBatch>; 
                if (grdDataSource != null)
                {
                    var rowData = (ThirdPartyTimeBatch)e.Cell.Row.ListObject;
                    if (e.Cell.Column.Key == CONST_DELETE)
                    {
                        if (rowData != null)
                        {
                            grdDataSource.Remove(rowData);
                            foreach (UltraGridRow row in grdTimedBatch.Rows)
                            {
                                row.Cells[CONST_SERIAL_NO_KEY].Value = row.Index + 1;
                            }
                        }
                    }
                    else if (e.Cell.Column.Key == CONST_PAUSE_RESUME)
                    {
                        rowData.IsPaused = !rowData.IsPaused;
                        e.Cell.ButtonAppearance.Image = rowData.IsPaused ? Properties.Resources.btn_Resume : Properties.Resources.btn_Pause;
                        e.Cell.ToolTipText = rowData.IsPaused ? "Resume" : "Pause";
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
        /// This method saves the form when clicked the save button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var timeBatches = grdTimedBatch.DataSource as List<ThirdPartyTimeBatch>;
                if (timeBatches != null)
                {
                    timeBatches.Sort((x, y) => x.BatchRunTime.CompareTo(y.BatchRunTime));
                    if (IsValidTimeGap(timeBatches))
                    {
                        UpdatedTimeBatches = DeepCopyHelper.Clone(timeBatches);

                        foreach (UltraGridRow row in grdTimedBatch.Rows)
                        {
                            row.Cells[CONST_SERIAL_NO_KEY].Value = row.Index + 1;
                        }
                        grdTimedBatch.Refresh();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Batches can only be scheduled at intervals greater than 15 minutes.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// The method sorts the list by BatchRunTime and batches have the time gaps of 15 minutes. 
        /// </summary>
        /// <param name="timeBatches"></param>
        private bool IsValidTimeGap(List<ThirdPartyTimeBatch> timeBatches)
        {
            try
            {
                for (int i = 1; i < timeBatches.Count; i++)
                {
                    TimeSpan timeDifference = timeBatches[i].BatchRunTime - timeBatches[i - 1].BatchRunTime;
                    if (timeDifference <= TimeSpan.FromMinutes(15))
                    {
                        return false;
                    }
                }
                return true;
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
                return false;
            }
        }
    }
}