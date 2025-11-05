using Prana.BusinessObjects.Compliance.Constants;
using Prana.BusinessObjects.Compliance.Delegates;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.Compliance.EventArguments;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;

namespace Prana.ComplianceEngine.AlertHistory.UI.UserControls
{
    public partial class AlertOperations : UserControl
    {
        public event GetAlertHandler GetAlertHistory;

        public AlertOperations()
        {
            try
            {
                InitializeComponent();
                SetExportPermission();

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
        /// To disable the export functionallity when needed.
        /// </summary>
        private void SetExportPermission()
        {
            try
            {
                if (CommonDataCache.CachedDataManager.CompanyMarketDataProvider == BusinessObjects.AppConstants.MarketDataProvider.SAPI && CommonDataCache.CachedDataManager.IsMarketDataBlocked)
                {
                    ultraBtnExport.Enabled = false;
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
        /// Sends request for loading alerts in grid for the given date range.
        /// If current alerts option is selected start and end date is DateTime.Now
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraBtnGetData_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate = DateTime.Now.Date;
                DateTime endDate = DateTime.Now.Date.AddDays(1);
                if (ultraOptHistory.CheckedItem.Tag.ToString() == AlertHistoryConstants.OPTION_HISTORICAL_TAG)
                {
                    startDate = Convert.ToDateTime(ultraClndrFrom.Value);
                    endDate = Convert.ToDateTime(ultraClndrTo.Value);
                }

                // checking if the startdate comes after the end date,
                //     if true then show message box
                // Jira issue : http://jira.nirvanasolutions.com:8080/browse/PRANA-4495
                if (startDate > endDate)
                {
                    MessageBox.Show(this, "Start date can not be after end date.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    if (GetAlertHistory != null)
                        GetAlertHistory(this, new GetAlertEventArgs { Operation = AlertHistoryOperations.GetData, StartDate = startDate, EndDate = endDate, PageNo = 1, PageSize = alertHistoryPaging1.GetPageSize() });
                    alertHistoryPaging1.Reset(); // Reseting paging buttons and upadating label 
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
        /// Sends request for Exportin alerts in grid.
        /// if alerts are filtered then filtered are exported.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraBtnExport_Click(object sender, EventArgs e)
        {
            try
            {

                if (GetAlertHistory != null)
                    GetAlertHistory(this, new GetAlertEventArgs { Operation = AlertHistoryOperations.Export, StartDate = DateTime.Now.Date, EndDate = DateTime.Now.Date });
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
        /// Archive alerts for the date range.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraBtnArchive_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate = DateTime.Now.Date;
                DateTime endDate = DateTime.Now.Date.AddDays(1);
                if (ultraOptHistory.CheckedItem.Tag.ToString() == AlertHistoryConstants.OPTION_HISTORICAL_TAG)
                {
                    startDate = Convert.ToDateTime(ultraClndrFrom.Value);
                    endDate = Convert.ToDateTime(ultraClndrTo.Value);
                }

                if (ultraOptHistory.CheckedItem.Tag.ToString() == AlertHistoryConstants.OPTION_CURRENT_TAG)
                {
                    endDate = endDate.AddDays(1);
                }
                if (GetAlertHistory != null)
                    GetAlertHistory(this, new GetAlertEventArgs { Operation = AlertHistoryOperations.Archive, StartDate = startDate, EndDate = endDate, PageNo = alertHistoryPaging1.GetCurrentPage(), PageSize = alertHistoryPaging1.GetPageSize() });
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
        /// sets calender from and to values when option button is changed.
        /// if current then both calender boxes are disabled.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraOptHistory_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (ultraOptHistory.CheckedItem.Tag.ToString() == AlertHistoryConstants.OPTION_HISTORICAL_TAG)
                {
                    ultraClndrFrom.Value = DateTime.Now.Date;
                    ultraClndrTo.Value = DateTime.Now.Date.AddDays(1);
                    ultraClndrFrom.Enabled = true;
                    ultraClndrTo.Enabled = true;
                }
                else if (ultraOptHistory.CheckedItem.Tag.ToString() == AlertHistoryConstants.OPTION_CURRENT_TAG)
                {
                    ultraClndrFrom.Value = DateTime.Now.Date;
                    ultraClndrTo.Value = DateTime.Now.Date.AddDays(1);
                    ultraClndrFrom.Enabled = false;
                    ultraClndrTo.Enabled = false;
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

        //internal string GetAlertType()
        //{
        //    return ultraOptHistory.CheckedItem.Tag.ToString();
        //}

        /// <summary>
        /// checks if grid to be updated when new alert arrives
        /// if historical alerts are shown then checks if any of the calender box is having current date as value.
        /// </summary>
        /// <returns></returns>
        internal bool GetIsUpdateGrid()
        {
            try
            {
                if (ultraOptHistory.CheckedItem.Tag.ToString() == AlertHistoryConstants.OPTION_CURRENT_TAG)
                    return true;
                else if (ultraOptHistory.CheckedItem.Tag.ToString() == AlertHistoryConstants.OPTION_HISTORICAL_TAG)
                {
                    if (ultraClndrFrom.Value.Equals(DateTime.Now.Date) || ultraClndrTo.Value.Equals(DateTime.Now.Date))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
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
        /// Raise an event to delete the selected rows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UltraButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate = DateTime.Now.Date;
                DateTime endDate = DateTime.Now.Date.AddDays(1);
                if (ultraOptHistory.CheckedItem.Tag.ToString() == AlertHistoryConstants.OPTION_HISTORICAL_TAG)
                {
                    startDate = Convert.ToDateTime(ultraClndrFrom.Value);
                    endDate = Convert.ToDateTime(ultraClndrTo.Value);
                }

                if (ultraOptHistory.CheckedItem.Tag.ToString() == AlertHistoryConstants.OPTION_CURRENT_TAG)
                {
                    endDate = endDate.AddDays(1);
                }
                if (GetAlertHistory != null)
                    GetAlertHistory(this, new GetAlertEventArgs { Operation = AlertHistoryOperations.Delete, StartDate = startDate, EndDate = endDate, PageNo = alertHistoryPaging1.GetCurrentPage(), PageSize = alertHistoryPaging1.GetPageSize() });
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
        /// Loading AlertOperation Control  as well as wiring event for pagging Control when page change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlertOperations_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
                alertHistoryPaging1.PageChanged += alertHistoryPaging1_pageChanged;
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
        /// Start the CloseStart Process
        /// </summary>
        /// <returns></returns>
        internal void CloseStart()
        {
            try
            {
                alertHistoryPaging1.PageChanged -= alertHistoryPaging1_pageChanged;   //Unwire the pagging Event
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
        /// AlertHistory Pagging Event When Page change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void alertHistoryPaging1_pageChanged(object sender, EventArgs<int, int> e)
        {
            try
            {
                DateTime startDate = DateTime.Now.Date;
                DateTime endDate = DateTime.Now.Date.AddDays(1);
                if (ultraOptHistory.CheckedItem.Tag.ToString() == AlertHistoryConstants.OPTION_HISTORICAL_TAG)
                {
                    startDate = Convert.ToDateTime(ultraClndrFrom.Value);
                    endDate = Convert.ToDateTime(ultraClndrTo.Value);
                }

                if (startDate > endDate)
                {
                    MessageBox.Show(this, "Start date can not be after end date.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    if (GetAlertHistory != null)
                        GetAlertHistory(this, new GetAlertEventArgs { Operation = AlertHistoryOperations.GetData, StartDate = startDate, EndDate = endDate, PageNo = e.Value, PageSize = e.Value2 });
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
        /// Used for changing the color of buttons. The indices and their colors are as follows:
        /// 0 & 3: For the Green Shade
        /// 1 & 4: For the Neutral Shade
        /// 2 & 5: For the Red Shade 
        /// </summary>

        private void SetButtonsColor()
        {
            try
            {

                ultraBtnGetData.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraBtnGetData.ForeColor = System.Drawing.Color.White;
                ultraBtnGetData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraBtnGetData.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraBtnGetData.UseAppStyling = false;
                ultraBtnGetData.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;


                ultraBtnExport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraBtnExport.ForeColor = System.Drawing.Color.White;
                ultraBtnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraBtnExport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraBtnExport.UseAppStyling = false;
                ultraBtnExport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;


                ultraBtnArchive.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraBtnArchive.ForeColor = System.Drawing.Color.White;
                ultraBtnArchive.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraBtnArchive.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraBtnArchive.UseAppStyling = false;
                ultraBtnArchive.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ultraButton1.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                ultraButton1.ForeColor = System.Drawing.Color.White;
                ultraButton1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraButton1.UseAppStyling = false;
                ultraButton1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// Setting Total No. of Pages
        /// </summary>
        /// <param name="totalPages">taking one int argument for total no of pages </param>
        internal void SetTotalPages(int totalPages)
        {
            try
            {
                alertHistoryPaging1.UpdateTotalPages(totalPages);
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
        /// Updating Alert History Grid in case of Multi User performed delete/archive operation
        /// </summary>
        internal void UpdateAlertGrid()
        {
            try
            {
                DateTime startDate = DateTime.Now.Date;
                DateTime endDate = DateTime.Now.Date.AddDays(1);
                if (ultraOptHistory.CheckedItem.Tag.ToString() == AlertHistoryConstants.OPTION_HISTORICAL_TAG)
                {
                    startDate = Convert.ToDateTime(ultraClndrFrom.Value);
                    endDate = Convert.ToDateTime(ultraClndrTo.Value);
                }

                if (startDate > endDate)
                {
                    MessageBox.Show(this, "Start date can not be after end date.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    if (GetAlertHistory != null)
                        GetAlertHistory(this, new GetAlertEventArgs { Operation = AlertHistoryOperations.GetData, StartDate = startDate, EndDate = endDate, PageNo = alertHistoryPaging1.GetCurrentPage(), PageSize = alertHistoryPaging1.GetPageSize() });
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
    }


}
