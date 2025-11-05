using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Constants;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Prana.ComplianceEngine.ComplianceAlertPopup
{
    public partial class ComplianceAlertPopupUC : UserControl
    {
        /// <summary>
        /// ComplianceAlertPopupUC
        /// </summary>
        public ComplianceAlertPopupUC()
        {
            InitializeComponent();
            SetExportPermissions();
        }

        /// <summary>
        /// CloseCompliancePopUp
        /// </summary>
        public event EventHandler CloseCompliancePopUp;

        /// <summary>
        /// HideCompliancePopUp
        /// </summary>
        public event EventHandler<EventArgs<bool>> HideCompliancePopUp;

        /// <summary>
        /// _thresholdActualResultobject
        /// </summary>
        readonly OpenAndBindDataThresholdActualResultView _thresholdActualResultobject = new OpenAndBindDataThresholdActualResultView();

        /// <summary>
        /// _alertsDataCache
        /// </summary>
        private readonly BindingList<Alert> _alertsDataCache = new BindingList<Alert>();

        /// <summary>
        /// _ruleType
        /// </summary>
        private RulePackage _ruleType;

        /// <summary>
        /// Datable _dtListViewSource
        /// </summary>
        readonly DataTable _dtListViewSource = new DataTable(ComplainceConstants.CONST_ALERTS);

        /// <summary>
        /// Lock object
        /// </summary>
        private static readonly object _complianceAlertlock = new object();

        /// <summary>
        /// isAnyHardlert
        /// </summary>
        private bool _isValidationRequired = true;

        /// <summary>
        /// _isTradeAllowed
        /// </summary>
        private bool _isTradeAllowed = false;

        /// <summary>
        /// _isTradeHardAlert
        /// </summary>
        private bool _isTradeHard = false;
        /// <summary>
        /// Is Trade Allowed
        /// </summary>
        public bool IsTradeAllowed
        {
            get { return _isTradeAllowed; }
        }

        /// <summary>
        /// delegate
        /// </summary>
        /// <param name="ruleTpe"></param>
        /// <param name="ruleName"></param>
        /// <param name="postdataAlerts"></param>
        delegate void MainThreadDelegate(string ruleTpe, string ruleName, DataSet postdataAlerts);

        /// <summary>
        /// Binding Compliance Alert Data
        /// </summary>
        /// <param name="alertPopUpType"></param>
        /// <param name="dataAlerts"></param>
        public void BindingComplianceAlertData(AlertPopUpType alertPopUpType, List<Alert> alertsList, bool isOnlyHardAlerts = false)
        {
            try
            {
                lock (_complianceAlertlock)
                {
                    switch (alertPopUpType)
                    {
                        case AlertPopUpType.Override:
                            responseButton.Text = ComplainceConstants.CONST_CAPS_YES;
                            cancelButton.Text = ComplainceConstants.CONST_CAPS_NO;
                            bottomMsgLabel.Text = ComplainceConstants.CONST_MESSAGE_OVERRIDE;
                            break;
                        case AlertPopUpType.ComplianceCheck:
                            _isValidationRequired = false;
                            cancelButton.Visible = false;
                            exportButton.Visible = false;
                            break;
                        case AlertPopUpType.Inform:
                            _isValidationRequired = false;
                            _isTradeHard = true;
                            cancelButton.Visible = false;
                            if (isOnlyHardAlerts)
                                alertsList = alertsList.Where(alert => alert.AlertType.Equals(AlertType.HardAlert)).ToList();
                            break;
                        case AlertPopUpType.PendingApproval:
                            responseButton.Text = ComplainceConstants.CONST_CAPS_SEND;
                            cancelButton.Text = ComplainceConstants.CONST_CAPS_CANCEL;
                            bottomMsgLabel.Text = ComplainceConstants.CONST_MESSAGE_PENDINGAPPROVAL;
                            break;
                    }

                    DateTime valTime = DateTime.Now;
                    if (alertsList != null && alertsList.Count > 0)
                    {
                        if (alertsList[0].AlertId.Equals(PreTradeConstants.CONST_FAILED_ALERT_ID))
                        {
                            exportButton.Visible = false;
                            if (Convert.ToInt32(alertsList[0].AlertType).Equals(Convert.ToInt32(AlertType.HardAlert)))
                                cancelButton.Visible = false;
                            alertGrid.DisplayLayout.Override.CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
                            alertGrid.DisplayLayout.Bands[0].Override.RowSizing = RowSizing.AutoFree;
                        }
                        DateTime.TryParse(alertsList[alertsList.Count - 1].ValidationTime.ToString(), out valTime);
                        titleUltraLabel.Text = ComplainceConstants.CONST_PRE_TRADE + valTime.Date.ToString(ComplainceConstants.CONST_DATE_FORMAT) + ", " + ComplainceConstants.CONST_TIME + valTime.ToString(ComplainceConstants.CONST_TIME_FORMAT);
                        Logger.LoggerWrite(string.Format(titleUltraLabel.Text + " BasketId: {0} - Alerts count: {1}", alertsList[0].OrderId, alertsList.Count), LoggingConstants.CATEGORY_GENERAL_COMPLIANCE);
                    }

                    for (int i = 0; i < alertsList.Count; i++)
                        _alertsDataCache.Add(alertsList[i]);

                    alertGrid.DataSource = _alertsDataCache;
                    IntializeRowsForPreTrade();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Get Update alertgrid alerts 
        /// </summary>
        /// <returns></returns>
        public List<Alert> GetUpdatedAlerts()
        {
            List<Alert> updatedAlerts = null;
            try
            {
                updatedAlerts = _alertsDataCache.ToList();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return updatedAlerts;
        }

        /// <summary>
        /// Sets column properties to define order and appearence
        /// </summary>
        private void AlertGrid_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                if (!alertGrid.DisplayLayout.Bands[0].Columns.Contains(ComplainceConstants.CONST_ALERT_TYPE_NAME))
                {
                    e.Layout.Bands[0].Columns.Add(ComplainceConstants.CONST_ALERT_TYPE_NAME);
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_ALERT_TYPE_NAME].DataType = typeof(string);
                }

                #region Hide columns

                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_ALERT_ID))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_ALERT_ID].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_ALERT_TYPE))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_ALERT_TYPE].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_VALIDATION_TIME))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_VALIDATION_TIME].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_ORDER_ID))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_ORDER_ID].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_ISVIOLATED))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_ISVIOLATED].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_ISEOM))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_ISEOM].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_COMPRESSION_LEVEL))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_COMPRESSION_LEVEL].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_USER_ID))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_USER_ID].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_PACKAGE_NAME))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_PACKAGE_NAME].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_SUMMARY))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_SUMMARY].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_BLOCKED))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_BLOCKED].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_RULE_ID))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_RULE_ID].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_STATUS))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_STATUS].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_GROUPID))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_GROUPID].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_PRE_TRADE_TYPE))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_PRE_TRADE_TYPE].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_PRE_TRADE_ACTIONTYPE))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_PRE_TRADE_ACTIONTYPE].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_ACTION_USER))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_ACTION_USER].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_ACTION_USER_NAME))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_ACTION_USER_NAME].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_OVERRIDE_USER_ID))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_OVERRIDE_USER_ID].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_USER_NAME))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_USER_NAME].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_COMPLIANCE_OFFICER_NOTES))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_COMPLIANCE_OFFICER_NOTES].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_CONSTRAINT_FIELDS))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_CONSTRAINT_FIELDS].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_TRADE_DETAILS))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_TRADE_DETAILS].Hidden = true;
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_ALERT_POP_UP_RESPONSE))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_ALERT_POP_UP_RESPONSE].Hidden = true;

                #endregion

                #region Column Formatting

                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_ALERT_TYPE))
                {
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_ALERT_TYPE_NAME].Header.VisiblePosition = 0;
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_ALERT_TYPE_NAME].Header.Caption = ComplainceConstants.CAPS_ALERT_TYPE;
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_ALERT_TYPE].SortIndicator = SortIndicator.Ascending;
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_ALERT_TYPE_NAME].PerformAutoResize(PerformAutoSizeType.AllRowsInBand, true);
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_ALERT_TYPE_NAME].Width = 125;
                }
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_RULE_NAME))
                {
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_RULE_NAME].Header.VisiblePosition = 1;
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_RULE_NAME].Header.Caption = ComplainceConstants.CAPS_RULE_NAME;
                }
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_DESCRIPTION))
                {
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_DESCRIPTION].Header.VisiblePosition = 2;
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_DESCRIPTION].Header.Caption = ComplainceConstants.CAPS_DESCRIPTION;
                    //e.Layout.Bands[0].Columns[ComplainceConstants.CONST_DESCRIPTION].Width = _isComplianceFailedAlert ? 240 : 150;
                }
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_DIMENSION))
                {
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_DIMENSION].Header.VisiblePosition = 3;
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_DIMENSION].Header.Caption = ComplainceConstants.CAPS_DIMENSIONS;
                }
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_THRESHOLD))
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_THRESHOLD].Header.VisiblePosition = 4;

                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_ACTUAL_RESULT))
                {
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_ACTUAL_RESULT].Header.VisiblePosition = 5;
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_ACTUAL_RESULT].Header.Caption = ComplainceConstants.CAPS_ACTUAL_RESULT;
                }
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_PARAMETERS))
                {
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_PARAMETERS].Header.VisiblePosition = 6;
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_PARAMETERS].Header.Caption = ComplainceConstants.CAPS_COMMENTS;
                }
                if (e.Layout.Bands[0].Columns.Exists(ComplainceConstants.CONST_USER_NOTES))
                {
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_USER_NOTES].Header.VisiblePosition = 7;
                    e.Layout.Bands[0].Columns[ComplainceConstants.CONST_USER_NOTES].Header.Caption = ComplainceConstants.CAPS_USER_NOTES;
                    //e.Layout.Bands[0].Columns[ComplainceConstants.CONST_USER_NOTES].MinWidth = 252;
                }

                #endregion

                foreach (UltraGridColumn col in e.Layout.Bands[0].Columns)
                {
                    if (col.ToString() != ComplainceConstants.CONST_USER_NOTES)
                        col.CellActivation = Activation.NoEdit;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// alertGrid_IntializeRow for Post trade alerts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlertGrid_IntializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row != null && _ruleType == RulePackage.PostTrade)
                {
                    SetLinAndPrecision(e.Row);
                    if (e.Row.Cells.Exists(ComplainceConstants.CONST_TIME_TRIGGERED))
                        e.Row.Cells[ComplainceConstants.CONST_TIME_TRIGGERED].Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// SetLinAndPrecision
        /// </summary>
        /// <param name="row"></param>
        private void SetLinAndPrecision(UltraGridRow row)
        {
            try
            {
                if (row.Cells.Exists(ComplainceConstants.CONST_CONSTRAINT_FIELDS))
                {
                    if (row.Cells[ComplainceConstants.CONST_CONSTRAINT_FIELDS].Text.Contains(ComplainceConstants.CONST_SEPARATOR_CHAR))
                    {
                        row.Cells[ComplainceConstants.CONST_THRESHOLD].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.URL;
                        row.Cells[ComplainceConstants.CONST_ACTUAL_RESULT].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.URL;
                        row.Cells[ComplainceConstants.CONST_THRESHOLD].Value = ComplainceConstants.CONST_MULTIPLE;
                        row.Cells[ComplainceConstants.CONST_ACTUAL_RESULT].Value = ComplainceConstants.CONST_MULTIPLE;
                    }
                    else
                    {
                        double numericValue;
                        if (double.TryParse(Convert.ToString(row.Cells[ComplainceConstants.CONST_THRESHOLD].Value), out numericValue))
                        {
                            row.Cells[ComplainceConstants.CONST_THRESHOLD].Value = string.Format(ComplainceConstants.CONST_PRECISION_FORMAT, numericValue);
                        }
                        if (double.TryParse(Convert.ToString(row.Cells[ComplainceConstants.CONST_ACTUAL_RESULT].Value), out numericValue))
                        {
                            row.Cells[ComplainceConstants.CONST_ACTUAL_RESULT].Value = string.Format(ComplainceConstants.CONST_PRECISION_FORMAT, numericValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets colour, activation and link for Pre trade alertGrid cells. 
        /// </summary>
        private void IntializeRowsForPreTrade()
        {
            try
            {
                foreach (UltraGridRow row in alertGrid.Rows)
                {
                    #region SetColorforCells

                    if (row.Cells.Exists(ComplainceConstants.CONST_ALERT_TYPE))
                    {
                        row.Cells[ComplainceConstants.CONST_ALERT_TYPE_NAME].Appearance.ForeColor = Color.White;
                        if (row.Cells[ComplainceConstants.CONST_ALERT_TYPE].Value.ToString().Equals(ComplainceConstants.CONST_HARD_ALERT))
                        {
                            _isValidationRequired = false;
                            row.Cells[ComplainceConstants.CONST_ALERT_TYPE_NAME].Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))));
                            row.Cells[ComplainceConstants.CONST_ALERT_TYPE_NAME].Value = EnumHelper.GetDescription(AlertType.HardAlert);
                        }
                        else if (row.Cells[ComplainceConstants.CONST_ALERT_TYPE].Value.ToString().Equals(ComplainceConstants.CONST_SOFT_ALERT))
                        {
                            row.Cells[ComplainceConstants.CONST_ALERT_TYPE_NAME].Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(156)))), ((int)(((byte)(46)))));
                            row.Cells[ComplainceConstants.CONST_ALERT_TYPE_NAME].Value = EnumHelper.GetDescription(AlertType.SoftAlert);
                        }
                        else if (row.Cells[ComplainceConstants.CONST_ALERT_TYPE].Value.ToString().Equals(ComplainceConstants.CONST_SOFT_ALERT_WITH_NOTES))
                        {
                            row.Cells[ComplainceConstants.CONST_ALERT_TYPE_NAME].Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(99)))), ((int)(((byte)(160)))));
                            row.Cells[ComplainceConstants.CONST_ALERT_TYPE_NAME].Value = EnumHelper.GetDescription(AlertType.SoftAlertWithNotes);
                        }
                        else if (row.Cells[ComplainceConstants.CONST_ALERT_TYPE].Value.ToString().Equals(ComplainceConstants.CONST_REQUIRES_APPROVAL))
                        {
                            row.Cells[ComplainceConstants.CONST_ALERT_TYPE_NAME].Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(165)))), ((int)(((byte)(0)))));
                            row.Cells[ComplainceConstants.CONST_ALERT_TYPE_NAME].Value = EnumHelper.GetDescription(AlertType.RequiresApproval);
                            row.Cells[ComplainceConstants.CONST_USER_NOTES].Value = ComplainceConstants.CONST_PLEASE_APPROVE;
                        }
                    }

                    #endregion

                    #region Set UserNotes Column Activation 

                    if (row.Cells.Exists(ComplainceConstants.CONST_USER_NOTES))
                    {
                        row.Cells[ComplainceConstants.CONST_USER_NOTES].Activation = Activation.NoEdit;
                        if (_isValidationRequired && row.Cells[ComplainceConstants.CONST_ALERT_TYPE].Value.ToString().Equals(ComplainceConstants.CONST_SOFT_ALERT_WITH_NOTES))
                            row.Cells[ComplainceConstants.CONST_USER_NOTES].Activation = Activation.AllowEdit;
                    }
                    #endregion

                    SetLinAndPrecision(row);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// alertGrid_BeforeExitEditMode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlertGrid_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
        {
            try
            {
                if (alertGrid.ActiveCell != null && _isValidationRequired)
                {
                    if (alertGrid.ActiveCell.Column.Key == ComplainceConstants.CONST_USER_NOTES && string.IsNullOrWhiteSpace(alertGrid.ActiveCell.Text.ToString()))
                        alertGrid.ActiveRow.DataErrorInfo.SetColumnError(ComplainceConstants.CONST_USER_NOTES, ComplainceConstants.CONST_USER_NOTES_STATEMENT);
                    else if (alertGrid.ActiveCell.Column.Key == ComplainceConstants.CONST_USER_NOTES && !string.IsNullOrWhiteSpace(alertGrid.ActiveCell.Text.ToString()))
                        alertGrid.ActiveRow.DataErrorInfo.SetColumnError(ComplainceConstants.CONST_USER_NOTES, string.Empty);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Click exporttoExcel button on UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportButton_Click(object sender, EventArgs e)
        {
            try
            {
                _thresholdActualResultobject.ExportToExcel(alertGrid, ultraGridExcelExporter, ComplainceConstants.CONST_ALERTS);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the ClickCell event of the AlertGrid_ClickCell control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ClickCellEventArgs"/> instance containing the event data.</param>
        private void AlertGrid_ClickCell(object sender, ClickCellEventArgs e)
        {
            try
            {
                if (alertGrid.DataSource != null && alertGrid.ActiveCell != null && (alertGrid.ActiveCell.Column.Key.Equals(ComplainceConstants.CONST_THRESHOLD) || alertGrid.ActiveCell.Column.Key.Equals(ComplainceConstants.CONST_ACTUAL_RESULT)) && alertGrid.ActiveCell.Text != null && alertGrid.ActiveCell.Text.Contains(ComplainceConstants.CONST_MULTIPLE))
                {
                    string constraintFields = alertGrid.ActiveRow.Cells[ComplainceConstants.CONST_CONSTRAINT_FIELDS].OriginalValue.ToString();
                    string threshold = alertGrid.ActiveRow.Cells[ComplainceConstants.CONST_THRESHOLD].OriginalValue.ToString();
                    string actualResult = alertGrid.ActiveRow.Cells[ComplainceConstants.CONST_ACTUAL_RESULT].OriginalValue.ToString();
                    _thresholdActualResultobject.OpenAndBindDataThresholdActualResultView1(constraintFields, threshold, actualResult);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Intialises Properties of Columns and Alerts.
        /// </summary>
        /// <param name="userId"></param>
        public void InitialiseControlsForPostPopUp(int userId)
        {
            try
            {
                LoadListViewDataDefinition();
                //LoadAlertCount(userId);
                if (this._dtListViewSource.Rows.Count == 0)
                    HideCompliancePopUpForm(false);

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        ///  Ocurrs when new alert is receive and pop up need to be shown
        /// </summary>
        /// <param name="ruleType"></param>
        /// <param name="ruleName"></param>
        /// <param name="postdataAlerts"></param>
        public void NewAlertReceived(string ruleType, string ruleName, DataSet postdataAlerts)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MainThreadDelegate del = this.NewAlertReceived;
                    this.BeginInvoke(del, new object[] { ruleType, ruleName, postdataAlerts });
                }
                else
                {
                    SetPropertiesForPostTrade();
                    string dimension = postdataAlerts.Tables[0].Rows[0][ComplainceConstants.CONST_DIMENSION].ToString();
                    string description = postdataAlerts.Tables[0].Rows[0][ComplainceConstants.CONST_SUMMARY].ToString();
                    string threshold = postdataAlerts.Tables[0].Rows[0][ComplainceConstants.CONST_THRESHOLD].ToString();
                    string actualResult = postdataAlerts.Tables[0].Rows[0][ComplainceConstants.CONST_ACTUAL_RESULT].ToString();
                    string comment = postdataAlerts.Tables[0].Rows[0][ComplainceConstants.CONST_PARAMETERS].ToString();
                    string constraintFields = postdataAlerts.Tables[0].Rows[0][ComplainceConstants.CONST_CONSTRAINT_FIELDS].ToString();

                    if (CachedDataManager.IsMarketDataBlocked && CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI
                    && !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                    {
                        List<string> ListOfFieldData = new List<string>(ComplainceConstants.CONST_FieldDataStr.Split(','));
                        List<string> fieldsList = new List<string>();
                        foreach (var field in ListOfFieldData)
                        {
                            fieldsList.Add(field.ToLower());
                        }
                        if (!constraintFields.Contains(ComplainceConstants.CONST_SEPARATOR_CHAR) && !fieldsList.Contains(constraintFields.ToLower()) && !constraintFields.Equals(ComplainceConstants.CONST_NA))
                        {
                            actualResult = ComplainceConstants.CONST_CensorValue;
                            threshold = ComplainceConstants.CONST_CensorValue;
                        }
                        comment = ComplainceConstants.CONST_CensorValue;
                        description = ComplainceConstants.CONST_CensorValue;
                    }

                    string filter = ComplainceConstants.CONST_KEY + "='" + ruleType + ruleName + "'";
                    if (_dtListViewSource.Select(filter).Length > 0)
                        _dtListViewSource.Select(filter)[0][ComplainceConstants.CONST_TIME_TRIGGERED] = Convert.ToInt32(_dtListViewSource.Select(filter)[0]["#TimeTriggered"]) + 1;
                    else
                        _dtListViewSource.Rows.Add(new object[] { ruleType, ruleName, description, dimension, threshold, actualResult, comment, 1, ruleType + ruleName, constraintFields });

                    alertGrid.Refresh();
                    HideCompliancePopUpForm(true);
                    this.BringToFront();
                    System.Media.SystemSounds.Exclamation.Play();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Set validation time Lable,Button Properties and AlertType when post alert occurs.
        /// </summary>
        private void SetPropertiesForPostTrade()
        {
            try
            {
                DateTime valTime = DateTime.Now;
                responseButton.Visible = true;
                cancelButton.Visible = false;
                titleUltraLabel.Text = ComplainceConstants.CONST_POST_TRADE + valTime.Date.ToString(ComplainceConstants.CONST_DATE_FORMAT) + ", " + ComplainceConstants.CONST_TIME + valTime.ToString(ComplainceConstants.CONST_TIME_FORMAT);
                _ruleType = RulePackage.PostTrade;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// To Create a datable for post alerts 
        /// </summary>
        public void LoadListViewDataDefinition()
        {
            try
            {
                _dtListViewSource.Columns.Add(ComplainceConstants.CONST_RULE_TYPE, typeof(string)).SetOrdinal(0);
                _dtListViewSource.Columns.Add(ComplainceConstants.CONST_RULE_NAME, typeof(string)).SetOrdinal(1);
                _dtListViewSource.Columns.Add(ComplainceConstants.CONST_DESCRIPTION, typeof(string)).SetOrdinal(2);
                _dtListViewSource.Columns.Add(ComplainceConstants.CONST_DIMENSION, typeof(string)).SetOrdinal(3);
                _dtListViewSource.Columns.Add(ComplainceConstants.CONST_THRESHOLD, typeof(string)).SetOrdinal(4);
                _dtListViewSource.Columns.Add(ComplainceConstants.CONST_ACTUAL_RESULT, typeof(string)).SetOrdinal(5);
                _dtListViewSource.Columns.Add(ComplainceConstants.CAPS_COMMENTS, typeof(string)).SetOrdinal(6); ;
                _dtListViewSource.Columns.Add(ComplainceConstants.CONST_TIME_TRIGGERED, typeof(double)).SetOrdinal(7);
                _dtListViewSource.Columns.Add(ComplainceConstants.CONST_KEY, typeof(string)).SetOrdinal(8);
                _dtListViewSource.Columns.Add(ComplainceConstants.CONST_CONSTRAINT_FIELDS, typeof(string)).SetOrdinal(9);

                if (_dtListViewSource.Columns.Contains(ComplainceConstants.CONST_RULE_TYPE))
                    _dtListViewSource.Columns[ComplainceConstants.CONST_RULE_TYPE].ColumnMapping = MappingType.Hidden;
                if (_dtListViewSource.Columns.Contains(ComplainceConstants.CONST_KEY))
                    _dtListViewSource.Columns[ComplainceConstants.CONST_KEY].ColumnMapping = MappingType.Hidden;
                if (_dtListViewSource.Columns.Contains(ComplainceConstants.CONST_CONSTRAINT_FIELDS))
                    _dtListViewSource.Columns[ComplainceConstants.CONST_CONSTRAINT_FIELDS].ColumnMapping = MappingType.Hidden;
                if (_dtListViewSource.Columns.Contains(ComplainceConstants.CONST_TIME_TRIGGERED))
                    _dtListViewSource.Columns[ComplainceConstants.CONST_TIME_TRIGGERED].Caption = ComplainceConstants.CAPS_TIME_TRIGGERED;
                alertGrid.DataSource = _dtListViewSource;
                if (alertGrid.DisplayLayout.Bands[0].Columns.Exists(ComplainceConstants.CONST_ALERT_TYPE_NAME))
                    alertGrid.DisplayLayout.Bands[0].Columns[ComplainceConstants.CONST_ALERT_TYPE_NAME].Hidden = true;
                alertGrid.DisplayLayout.Bands[0].Columns[ComplainceConstants.CAPS_COMMENTS].MinWidth = 300;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// For a given user load all alert count for given user
        /// For pre-trade - Alert by given user occured today
        /// For post-trade - All alert occured today
        /// </summary>
        /// <param name="userId">User Id for which data will be loaded</param>
        /*private void LoadAlertCount(int userId)
        {
            try
            {
                DataTable tempDt = DatabaseManager.DatabaseManager.ExecuteDataSet(ComplainceConstants.CONST_ALERT_COUNT, new object[] { userId }).Tables[0];
                if (tempDt.Rows.Count > 0)
                {
                    foreach (DataRow dr in tempDt.Rows)
                    {
                        string ruleType = dr[ComplainceConstants.CONST_RULE_TYPE].ToString();
                        string ruleName = dr[ComplainceConstants.CONST_RULE_NAME].ToString();
                        _dtListViewSource.Rows.Add(new object[] { ruleName, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, dr["TriggerCount"].ToString(), ruleType + ruleName });
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                    throw;
            }
        }*/

        /// <summary>
        /// To disable the  export functionallity.
        /// </summary>
        private void SetExportPermissions()
        {
            try
            {
                if (CommonDataCache.CachedDataManager.CompanyMarketDataProvider == BusinessObjects.AppConstants.MarketDataProvider.SAPI && CommonDataCache.CachedDataManager.IsMarketDataBlocked)
                {
                    exportButton.Enabled = false;
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
        /// Response Button Click Event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResponseButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_ruleType == RulePackage.PostTrade)
                {
                    _dtListViewSource.Rows.Clear();
                    CloseCompliancePopUpForm();
                }
                else
                {
                    if (_isValidationRequired)
                    {
                        bool userNotesError = false;
                        foreach (UltraGridRow row in alertGrid.Rows)
                        {
                            if (row.Cells.Exists(ComplainceConstants.CONST_ALERT_TYPE) && row.Cells[ComplainceConstants.CONST_ALERT_TYPE].Value.ToString().Equals(ComplainceConstants.CONST_SOFT_ALERT_WITH_NOTES)
                                && string.IsNullOrWhiteSpace(row.Cells[ComplainceConstants.CONST_USER_NOTES].Text.ToString()))
                            {
                                userNotesError = true;
                                row.DataErrorInfo.SetColumnError(ComplainceConstants.CONST_USER_NOTES, ComplainceConstants.CONST_USER_NOTES_STATEMENT);
                            }
                        }
                        if (!userNotesError)
                        {
                            _isTradeAllowed = true;
                            CloseCompliancePopUpForm();
                        }
                    }
                    else
                    {
                        _isTradeAllowed = true;
                        if (_isTradeHard)
                            _isTradeAllowed = false;
                        CloseCompliancePopUpForm();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// CancelButton_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            _isTradeAllowed = false;
            CloseCompliancePopUpForm();
        }

        /// <summary>
        /// CloseCompliancePopUpForm
        /// </summary>
        private void CloseCompliancePopUpForm()
        {
            if (CloseCompliancePopUp != null)
                CloseCompliancePopUp(this, null);
        }

        /// <summary>
        /// HideCompliancePopUpForm
        /// </summary>
        private void HideCompliancePopUpForm(bool isVisible)
        {
            if (HideCompliancePopUp != null)
                HideCompliancePopUp(this, new EventArgs<bool>(isVisible));
        }

        /// <summary>
        /// Sets the theme for user control.
        /// </summary>
        internal void SetThemeForUserControl()
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(alertGrid, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_COMPLIANCE_POPUP);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
