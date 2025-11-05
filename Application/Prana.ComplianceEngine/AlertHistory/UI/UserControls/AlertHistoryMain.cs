using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.Compliance.EventArguments;
using Prana.CommonDataCache;
using Prana.ComplianceEngine.AlertHistory.BLL;
using Prana.ComplianceEngine.ComplianceAlertPopup;
using Prana.ComplianceEngine.RuleDefinition.BLL;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Prana.ComplianceEngine.AlertHistory.UI.UserControls
{
    public partial class AlertHistoryMain : UserControl
    {
        public AlertHistoryMain()
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
        /// Binds All Events
        /// </summary>
        private void BindAllEvents()
        {
            try
            {
                alertOperations1.GetAlertHistory += alertOperations1_GetAlertHistory;
                AlertHistoryManager.GetInstance().UpdateAlertGridEvent += AlertHistoryMainNew_UpdateAlertGridEvent;

                // bind events to listen for change in rule (rename and delete)
                RuleManager.GetInstance().RenameRuleOperationCompleted += AlertHistoryMain_RenameRuleOperationCompleted;
                RuleManager.GetInstance().RuleOperationCompleted += AlertHistoryMain_RuleOperationCompleted;
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
        /// Update the history if rules are modified(deleted)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AlertHistoryMain_RuleOperationCompleted(object sender, RuleOperationEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { AlertHistoryMain_RuleOperationCompleted(sender, e); };
                    this.BeginInvoke(del);
                }
                else
                {
                    if (e.OperationType == RuleOperations.DeleteRule)
                    {
                        foreach (RuleBase r in e.RuleList)
                            alertGrid1.Delete(r.RuleId);
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
        /// update the history if a rule is renamed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AlertHistoryMain_RenameRuleOperationCompleted(object sender, RuleOperationEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { AlertHistoryMain_RenameRuleOperationCompleted(sender, e); };
                    this.BeginInvoke(del);
                }
                else
                {
                    if (e.OperationType == RuleOperations.RenameRule)
                    {
                        alertGrid1.Rename(e.OldValue, e.RuleList[0].RuleName, e.RuleList[0].RuleId);
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
        /// Updates alert in grid if grid is showing current alerts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void AlertHistoryMainNew_UpdateAlertGridEvent(object sender, UpdateAlertGridEventArgs args)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { AlertHistoryMainNew_UpdateAlertGridEvent(sender, args); };
                    this.BeginInvoke(del);
                }
                else
                {
                    if (args.DsRecieved.Tables.Count > 0)
                    {
                        //bool isUpdateGrid = alertOperations1.GetIsUpdateGrid();
                        //if (alertsType == AlertHistoryConstants.OPTION_HISTORICAL_TAG)
                        //{
                        //    return;
                        //}
                        //else

                        // Updating the alert history grid in case of single user, so will perform update operation
                        if (alertOperations1.GetIsUpdateGrid() && args.Operation == AlertHistoryOperations.Update)
                        {
                            alertGrid1.UpdateAlerts(args.DsRecieved);
                        }
                        //Updating the alert history grid in case of multi user, so will perform GetData operation
                        else if (args.Operation == AlertHistoryOperations.GetData)
                        {
                            alertOperations1.UpdateAlertGrid();
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
        /// Loads alert history for the date range.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void alertOperations1_GetAlertHistory(object sender, GetAlertEventArgs args)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { alertOperations1_GetAlertHistory(sender, args); };
                    this.BeginInvoke(del);
                }
                else
                {
                    switch (args.Operation)
                    {
                        case AlertHistoryOperations.GetData:
                            UpdateAlertGrid(args.StartDate, args.EndDate, args.PageNo, args.PageSize);
                            break;
                        case AlertHistoryOperations.Export:
                            ExportAlerts();

                            break;
                        case AlertHistoryOperations.Archive:
                            //Checking if number of rows affected is greater than zero than show alerts archived else other message.
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-4474 
                            int rowsAffected = AlertHistoryManager.GetInstance().ArchiveAlerts(alertGrid1.GetSelectedRows(), 1);
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show(this, rowsAffected + " Alerts Archived", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show(this, "There are no rows to archive.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            UpdateAlertGrid(args.StartDate, args.EndDate, args.PageNo, args.PageSize);
                            break;
                        case AlertHistoryOperations.Delete:
                            rowsAffected = AlertHistoryManager.GetInstance().ArchiveAlerts(alertGrid1.GetSelectedRows(), 0);
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show(this, rowsAffected + " Alerts Deleted", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show(this, "There are no rows to delete.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            UpdateAlertGrid(args.StartDate, args.EndDate, args.PageNo, args.PageSize);
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
        /// Exports all alerts in grid in excel format.
        /// </summary>
        private void ExportAlerts()
        {
            try
            {
                if (alertGrid1.CanExport())
                {
                    String folderPath = "";
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Filter = "Excel Worksheets|*.xls;|All Files|*.*";
                    dialog.Title = "Save an Excel File";

                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        folderPath = dialog.FileName;
                    }
                    if (String.IsNullOrEmpty(folderPath))
                        MessageBox.Show(this, "No path selected", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        alertGrid1.ExportAlerts(folderPath);
                        MessageBox.Show(this, "Alerts Exported", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                    MessageBox.Show(this, "There is no row to export.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Add alerts to Grid for the given date range.
        /// </summary>
        /// <param name="dateFrom">From Date</param> 
        /// <param name="dateTo">To Date</param> 
        /// <param name="pageNumber">For No. of Pages</param> 
        /// <param name="pageSize">For Page Size</param> 
        private void UpdateAlertGrid(DateTime dateFrom, DateTime dateTo, int pageNumber, int pageSize)
        {
            try
            {
                int totalRows; //Total No. of Rows
                int totalPages; // Total No. of Pages in a AlertHistory Grid
                string sortedColumnName = alertGrid1.SortedColumn(); // Sorted Column Name, applied sorting on Grid
                Dictionary<string, string> filteredColumns = alertGrid1.FilteredColumn();
                string filterValues = AlertHistoryManager.GetInstance().MakeQuery(filteredColumns); // Filtered Column Name, filter applied on the Grid

                DataSet ds = AlertHistoryManager.GetInstance().GetAlertHistory(dateFrom, dateTo, pageNumber, pageSize, sortedColumnName, filterValues, out totalRows);
                alertGrid1.InitializeAlertHistory(GetAlertHistoryDataWithCensorValue(ds));
                totalPages = Convert.ToInt32(Math.Ceiling((double)totalRows / pageSize)); //Calculate Total No. Of Pages

                if (totalPages == 0)
                    totalPages = 1;
                if (pageNumber > totalPages)  // If all the row deleted of last page
                {
                    pageNumber--;
                    alertGrid1.InitializeAlertHistory(AlertHistoryManager.GetInstance().GetAlertHistory(dateFrom, dateTo, pageNumber, pageSize, sortedColumnName, filterValues, out totalRows));
                }
                alertOperations1.SetTotalPages(totalPages);
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
        ///  Checking weather column value needs to censor or not
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        internal DataSet GetAlertHistoryDataWithCensorValue(DataSet ds)
        {
            try
            {
                if (CachedDataManager.IsMarketDataBlocked && CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI
                         && !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                {
                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataColumn column in table.Columns)
                        {
                            if (column.ColumnName == ComplainceConstants.CONST_ColumnParameter || column.ColumnName == ComplainceConstants.CONST_ColumnDescription 
                                || column.ColumnName == ComplainceConstants.CONST_ColumnSummary)
                            {
                                foreach (DataRow row in table.Rows)
                                { 
                                    row[column] = ComplainceConstants.CONST_CensorValue;
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
            return ds;
        }

        /// <summary>
        /// Unwind all events
        /// </summary>
        /// <returns></returns>
        internal bool CloseStart()
        {
            try
            {
                alertOperations1.CloseStart();
                alertOperations1.GetAlertHistory -= alertOperations1_GetAlertHistory;
                AlertHistoryManager.GetInstance().UpdateAlertGridEvent -= AlertHistoryMainNew_UpdateAlertGridEvent;
                RuleManager.GetInstance().RenameRuleOperationCompleted -= AlertHistoryMain_RenameRuleOperationCompleted;
                RuleManager.GetInstance().RuleOperationCompleted -= AlertHistoryMain_RuleOperationCompleted;
                return true;
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
                return false;
            }
        }

        /// <summary>
        /// Loads grid for current alerts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void alertGrid1_Load(object sender, EventArgs e)
        {
            try
            {
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    UpdateAlertGrid(DateTime.Now.Date, DateTime.Now.Date.AddDays(1), 1, 20);
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

        private void AlertHistoryMain_Load(object sender, EventArgs e)
        {
            try
            {
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    BindAllEvents();
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

        public void ExportData(string gridName, string filePath)
        {
            try
            {
                this.alertGrid1.ExportData(gridName, filePath);
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

    }
}
