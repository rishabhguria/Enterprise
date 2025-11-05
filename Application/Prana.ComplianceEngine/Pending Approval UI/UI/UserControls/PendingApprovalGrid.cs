using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.CommonDataCache;
using Prana.ComplianceEngine.ComplianceAlertPopup;
using Prana.ComplianceEngine.Pending_Approval_UI.BLL;
using Prana.ComplianceEngine.Pending_Approval_UI.UI.UserControls;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Prana.ComplianceEngine.Pending_Approval_UI
{
    public partial class PendingApprovalGrid : UserControl
    {

        UltraGridBand _currentBand = null;
        /// <summary>
        /// Pending Approval Data Cache
        /// </summary>
        BindingList<PendingApprovalData> _pendingApprovalDataCache = new BindingList<PendingApprovalData>();

        /// <summary>
        /// _thresholdActualResult object
        /// </summary>
        OpenAndBindDataThresholdActualResultView _thresholdActualResultobject = new OpenAndBindDataThresholdActualResultView();

        /// <summary>
        /// Lock Object
        /// </summary>
        private static Object _pendingApprovallock = new Object();

        /// <summary>
        ///Default Constructor
        /// </summary>
        public PendingApprovalGrid()
        {
            try
            {
                InitializeComponent();
                SetExportPermission();
                PendingApprovalManager.GetInstance().UpdateGrid += PendingApprovalGrid_UpdateGrid;
                PendingApprovalManager.GetInstance().UpdateGridForMultiUser += PendingApprovalGrid_UpdateGridForMultiUser;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
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
                    exportToExcelToolStripMenuItem.Enabled = false;
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
        /// Bind Pending Approval Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void BindPendingApprovalData(DataSet dataSet)
        {
            try
            {
                lock (_pendingApprovallock)
                {
                    List<Alert> alertsAllowed = Alert.GetAlertObjectFromDataTable(dataSet.Tables[PendingApprovalConstants.CONST_TRIGGERED_ALERTS]);
                    List<PendingApprovalOrderDetails> orderDetails = PendingApprovalOrderDetails.GetOrderFromDataTable(dataSet.Tables[PendingApprovalConstants.CONST_PENDING_ORDER_DETAILS]);
                    PendingApprovalData data = new PendingApprovalData();

                    data.BasketName = dataSet.Tables[PendingApprovalConstants.CONST_PRETRADE_APPROVAL_INFO].Rows[0][PendingApprovalConstants.CONST_MULTI_TRADE_NAME].ToString();
                    data.Alerts = alertsAllowed;

                    //to handle the case of multi trade when all trade have same symbol or counter party
                    data.Symbols = orderDetails.Select(t => t.Symbol).Distinct().Count() == 1 ? orderDetails[0].Symbol : "N/A";
                    data.Broker = orderDetails.Select(t => t.Broker).Distinct().Count() == 1 ? orderDetails[0].Broker : "N/A";
                    data.TradeOrigination = EnumHelper.GetDescription(orderDetails[0].TradeOrigination);
                    data.UserName = orderDetails[0].UserName;

                    if (orderDetails.Count == 1)
                    {
                        data.TradeDate = orderDetails[0].TradeDate;
                        data.Trader = orderDetails[0].Trader;
                        data.TradeDetails = orderDetails[0].TradeDetails;
                        data.TradeNotes = orderDetails[0].TradeNotes;
                        data.Quantity = orderDetails[0].Quantity;
                        data.TradePrice = orderDetails[0].TradePrice;
                        data.OrderSide = orderDetails[0].OrderSide;
                    }
                    // In case of multi trade ticket or when count is 0
                    else
                    {
                        data.Quantity = null;//changed to ' - ' in SetPropertiesForColumns() method
                        data.TradePrice = null;//changed to ' - ' in SetPropertiesForColumns() method
                        data.OrderSide = "N/A";
                        data.TradeDetails = "N/A";
                        data.TradeDate = "N/A";
                        data.Trader = "N/A";
                        data.TradeNotes = "N/A";
                    }
                    if (CachedDataManager.IsMarketDataBlocked && CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI
                           && !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                    {
                        if (!String.IsNullOrEmpty(data.TradeDetails))
                        {
                            string[] fields = data.TradeDetails.Split(new string[] { " " }, StringSplitOptions.None);
                            Array.Reverse(fields);
                            if (fields != null)
                            {
                                fields[ApplicationConstants.IndexOfNotional] = ApplicationConstants.CONST_CensorValue;
                                fields[ApplicationConstants.IndexOfTradePrice] = ApplicationConstants.CONST_CensorValue;
                            }
                            Array.Reverse(fields);      
                            data.TradeDetails = string.Join(" ", fields);
                        }
                        data.TradePrice = null;
                    }
                    //To check the new basket Id is exist or not in Grid DataSource 
                    int countBasketNameExist = _pendingApprovalDataCache.Count(pendingApprovalData => pendingApprovalData.BasketName.Equals(data.BasketName));

                    if ((!_pendingApprovalDataCache.Contains(data)) && countBasketNameExist == 0)
                        _pendingApprovalDataCache.Add(data);
                    SetThresholdAndActualResult();
                    SetCensorValues();
                    ultraPendingApprovalGrid.Refresh();
                    UpdatePendingApprovalGridFrozeUnfroze();
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
        /// Froze/Unfroze the alerts row.
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="isFrozen"></param>
        /// <param name="isUIOpen"></param>
        internal void BindPendingFrozeUnFroze(DataSet dataSet,bool isFrozen)
        {
            try
            {
                List<Alert> alertsAllowed = Alert.GetAlertObjectFromDataTable(dataSet.Tables[PendingApprovalConstants.CONST_ALERT]);
                UpdateFrozenAlerts(alertsAllowed, isFrozen);

                if (alertsAllowed != null && alertsAllowed.Count > 0)
                {
                    if(alertsAllowed[0].PreTradeActionType.Equals(PreTradeActionType.Blocked))
                        PendingApprovalGrid_UpdateGridForMultiUser(this, new EventArgs<List<Alert>, string, string, int>(alertsAllowed, alertsAllowed[0].OrderId, alertsAllowed[0].PreTradeActionType.ToString(), alertsAllowed[0].ActionUser));


                    else if (ultraPendingApprovalGrid != null)
                    {
                        foreach (UltraGridRow row in ultraPendingApprovalGrid.Rows)
                        {
                            if (alertsAllowed.FirstOrDefault(x => x.OrderId == row.Cells[PendingApprovalConstants.CONST_BASKET_NAME].Value.ToString()) != null)
                            {
                                if (isFrozen)
                                {
                                    if (row.Cells.Exists(PendingApprovalConstants.CONST_CHECKBOX))
                                    {
                                        row.Cells[PendingApprovalConstants.CONST_CHECKBOX].Value = false;
                                        row.Cells[PendingApprovalConstants.CONST_CHECKBOX].Activation = Activation.Disabled;
                                    }
                                    row.Activation = Activation.Disabled;
                                }
                                else
                                {
                                    if (!GetApproveBlockButtonState(row.Cells[PendingApprovalConstants.CONST_BASKET_NAME].Value.ToString()))
                                    {
                                        if (row.Cells.Exists(PendingApprovalConstants.CONST_CHECKBOX))
                                            row.Cells[PendingApprovalConstants.CONST_CHECKBOX].Activation = Activation.AllowEdit;
                                        row.Activation = Activation.AllowEdit;
                                    }
                                }
                            }
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
        /// Update the pending Approval grid for Froze/Unfrozen state
        /// </summary>
        private void UpdatePendingApprovalGridFrozeUnfroze()
        {
            try
            {
                if (ultraPendingApprovalGrid != null)
                {
                    foreach (UltraGridRow row in ultraPendingApprovalGrid.Rows)
                    {
                        if (CachedDataManager.GetInstance.GetPendingApprovalFrozenAlerts().Count > 0 && CachedDataManager.GetInstance.GetPendingApprovalFrozenAlerts().FirstOrDefault(x => x == row.Cells[PendingApprovalConstants.CONST_BASKET_NAME].Value.ToString()) != null)
                        {
                            if (row.Cells.Exists(PendingApprovalConstants.CONST_CHECKBOX))
                                row.Cells[PendingApprovalConstants.CONST_CHECKBOX].Activation = Activation.Disabled;
                            row.Activation = Activation.Disabled;
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
        /// update the frozen alerts cache
        /// </summary>
        /// <param name="alertsAllowed"></param>
        /// <param name="isFrozen"></param>
        private void UpdateFrozenAlerts(List<Alert> alertsAllowed, bool isFrozen)
        {
            try
            {
                if (alertsAllowed != null)
                {
                    foreach (Alert alert in alertsAllowed)
                    {
                        if (isFrozen)
                            CachedDataManager.GetInstance.AddPendingApprovalFrozenAlerts(alert.OrderId);
                        else
                            CachedDataManager.GetInstance.RemovePendingApprovalFrozenAlerts(alert.OrderId);
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
        /// Export To Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _thresholdActualResultobject.ExportToExcel(ultraPendingApprovalGrid, ultraGridExcelExporter1, PendingApprovalConstants.CONST_WORKBOOK_NAME);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Update Grid in case of Multi User
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PendingApprovalGrid_UpdateGridForMultiUser(object sender, EventArgs<List<Alert>, string, string, int> e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker pendingApprovalGridUpdateGridForMultiUser = delegate { PendingApprovalGrid_UpdateGridForMultiUser(sender, e); };
                    this.BeginInvoke(pendingApprovalGridUpdateGridForMultiUser);
                }
                else
                {
                    List<PendingApprovalData> removeBasket = new List<PendingApprovalData>();
                    lock (_pendingApprovallock)
                    {
                        //If alerts not allowed by Trade server
                        if (e.Value4 != -1)
                        {
                            //If pretrade action type is Allowed
                            if (e.Value3.Equals(PreTradeActionType.NoAction.ToString()))
                            {
                                List<Alert> alertsAllowed = e.Value;
                                foreach (PendingApprovalData data in _pendingApprovalDataCache)
                                {
                                    if (data.BasketName.Equals(e.Value2))
                                    {
                                        bool isRemove = true;
                                        data.Alerts.ForEach(x =>
                                        {
                                            if (!alertsAllowed.Contains(x)) isRemove = false;
                                            else
                                            {
                                                x.PreTradeActionType = PreTradeActionType.Allowed;
                                                x.ActionUser = e.Value4;
                                                x.ActionUserName = GetUserName(e.Value4);
                                                ChangeColor(e.Value2);
                                            }
                                        });

                                        if (isRemove)
                                            removeBasket.Add(data);
                                    }
                                }
                            }

                            //If pretrade action type is Blocked/ Allowed
                            if (e.Value3.Equals(PreTradeActionType.Blocked.ToString()) || e.Value3.Equals(PreTradeActionType.Allowed.ToString()))
                            {
                                foreach (PendingApprovalData data in _pendingApprovalDataCache)
                                {
                                    if (data.BasketName.Equals(e.Value2))
                                        removeBasket.Add(data);
                                }
                            }
                        }
                        //If alerts allowed by trade server
                        else if (e.Value4 == -1)
                        {
                            foreach (PendingApprovalData data in _pendingApprovalDataCache)
                            {
                                if (data.BasketName.Equals(e.Value2))
                                    removeBasket.Add(data);
                            }
                        }

                        if (removeBasket.Count > 0)
                            removeBasket.ForEach(basketName => _pendingApprovalDataCache.Remove(basketName));
                    }
                    ultraPendingApprovalGrid.Refresh();
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
        /// Get UserName on the basis of user Id
        /// </summary>
        /// <param name="userId"></param>
        private string GetUserName(int userId)
        {
            try
            {
                string defaultValue = "";
                string userName = defaultValue;
                if (userId != -1)
                {
                    if (userId != 0)
                        userName = CommonDataCache.CachedDataManager.GetInstance.GetUserText(userId);
                    if (string.IsNullOrEmpty(userName))
                        userName = defaultValue;
                }
                return userName;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return String.Empty;
            }
        }

        /// <summary>
        /// Change Alert Color if other user allowed the order
        /// </summary>
        /// <param name="alert"></param>
        private void ChangeColor(string basketName)
        {
            try
            {
                if (ultraPendingApprovalGrid.Rows != null)
                {
                    UltraGridRow[] rows = ultraPendingApprovalGrid.Rows.GetFilteredInNonGroupByRows();
                    foreach (UltraGridRow row in rows)
                    {
                        if (row.Cells[PendingApprovalConstants.CONST_BASKET_NAME].Value.ToString().Equals(basketName))
                        {
                            RowsCollection childRows = row.ChildBands[0].Rows;
                            foreach (UltraGridRow row1 in childRows)
                            {
                                if (Convert.ToInt32(row1.Cells[PendingApprovalConstants.CONST_ACTION_USER].Value) != -1)
                                {
                                    row1.Appearance.ForeColor = Color.Green;
                                    row1.Cells[PendingApprovalConstants.CONST_PRE_TRADE_ACTIONTYPE].Appearance.ForeColor = Color.Green;
                                    row1.Cells[PendingApprovalConstants.CONST_ALERT_TYPE].Appearance.ForeColor = Color.Green;
                                }
                            }
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
        /// Update Grid After Server Side Response
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PendingApprovalGrid_UpdateGrid(object sender, EventArgs<UltraGridRow, bool, string> e)
        {
            try
            {
                UpdateGridAfterServerSideResponse(e.Value, e.Value2, e.Value3);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Update Grid After Server Side Response
        /// </summary>
        /// <param name="alerts">List of Alerts</param>
        /// <param name="basketName">Basket Name</param>
        /// <param name="deletedAction">Deleted Action perform or not</param>
        private void UpdateGridAfterServerSideResponse(UltraGridRow row, bool deletedAction, string userName)
        {
            try
            {

                if (this.InvokeRequired)
                {
                    MethodInvoker updateGridAfterServerSideResponse = delegate { UpdateGridAfterServerSideResponse(row, deletedAction, userName); };
                    this.BeginInvoke(updateGridAfterServerSideResponse);
                }
                else
                {
                    if (deletedAction)
                    {
                        row.Delete(false);
                    }
                    else
                    {
                        row.Cells[PendingApprovalConstants.CONST_STATUS].Value = PendingApprovalConstants.CONST_ORDER_STATUS_MESSAGE;
                        row.Cells[PendingApprovalConstants.CONST_APPROVE_BUTTON].Activation = Activation.Disabled;
                        row.Cells[PendingApprovalConstants.CONST_BLOCK_BUTTON].Activation = Activation.Disabled;

                        RowsCollection childRows = row.ChildBands[0].Rows;
                        if (childRows != null)
                        {
                            foreach (UltraGridRow row1 in childRows)
                            {
                                if ((row1.Cells[PendingApprovalConstants.CONST_OVERRIDE_USER_ID].Value).ToString().Split(',').Contains(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString()))
                                {
                                    row1.Cells[PendingApprovalConstants.CONST_ACTION_USER].Value = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                                    row1.Cells[PendingApprovalConstants.CONST_OVERRIDE_USER_ID].Value = PendingApprovalHelperClass.RemoveUserId(row1.Cells[PendingApprovalConstants.CONST_OVERRIDE_USER_ID].Value.ToString());
                                }

                                if (((row1.Cells.Exists(PendingApprovalConstants.CONST_ACTION_USER_NAME)) && (Convert.ToInt32(row1.Cells[PendingApprovalConstants.CONST_ACTION_USER].Value) != -1)))
                                {
                                    row1.Cells[PendingApprovalConstants.CONST_ACTION_USER_NAME].Value = userName;
                                    row1.Appearance.ForeColor = Color.Green;
                                    row1.Cells[PendingApprovalConstants.CONST_PRE_TRADE_ACTIONTYPE].Value = PreTradeActionType.Allowed;
                                    row1.Cells[PendingApprovalConstants.CONST_PRE_TRADE_ACTIONTYPE].Appearance.ForeColor = Color.Green;
                                    row1.Cells[PendingApprovalConstants.CONST_ALERT_TYPE].Appearance.ForeColor = Color.Green;
                                }
                            }
                        }
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
        /// Update the Pending Approval Grid
        /// Data to be displayed in column cells is formatted here
        /// </summary>
        /// <param name="pendingApprovalData"></param>
        internal void UpdateGrid(Dictionary<string, PreTradeApprovalInfo> pendingApprovalData)
        {
            try
            {
                lock (_pendingApprovallock)
                {
                    foreach (KeyValuePair<string, PreTradeApprovalInfo> preTradeApprovalInfo1 in pendingApprovalData)
                    {
                        PreTradeApprovalInfo preTradeApprovalInfo = preTradeApprovalInfo1.Value;
                        PendingApprovalData data = new PendingApprovalData();
                        data.BasketName = preTradeApprovalInfo1.Key;
                        List<PendingApprovalOrderDetails> orderDetails = preTradeApprovalInfo.PendingOrderDetails;

                        //to handle the case of multi trade when all trade have same symbol or counter party
                        //also make changes in BindPendingApprovalData() for row updation
                        data.Symbols = orderDetails.Select(t => t.Symbol).Distinct().Count() == 1 ? orderDetails[0].Symbol : "N/A";
                        data.Broker = orderDetails.Select(t => t.Broker).Distinct().Count() == 1 ? orderDetails[0].Broker : "N/A";
                        data.TradeOrigination = EnumHelper.GetDescription(orderDetails[0].TradeOrigination);
                        data.UserName = orderDetails[0].UserName;

                        //in case of single trade
                        if (orderDetails.Count == 1)
                        {
                            data.TradeDate = orderDetails[0].TradeDate;
                            data.Trader = orderDetails[0].Trader;
                            data.TradeDetails = orderDetails[0].TradeDetails;
                            data.Quantity = orderDetails[0].Quantity;
                            data.TradePrice = orderDetails[0].TradePrice;
                            data.OrderSide = orderDetails[0].OrderSide;
                            data.TradeNotes = orderDetails[0].TradeNotes;
                        }
                        //in case of multi trade ticket or when count is 0
                        else
                        {
                            data.Quantity = null; // Changed to ' - ' in SetPropertiesForColumns() method
                            data.TradePrice = null; // Changed to ' - ' in SetPropertiesForColumns() method
                            data.OrderSide = "N/A";
                            data.TradeDetails = "N/A";
                            data.TradeDate = "N/A";
                            data.Trader = "N/A";
                            data.TradeNotes = "N/A";
                        }

                        data.Alerts = preTradeApprovalInfo.TriggeredAlerts;
                        if (CachedDataManager.IsMarketDataBlocked && CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI
                           && !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                        {
                            if (!String.IsNullOrEmpty(data.TradeDetails))
                            {
                                string[] fields = data.TradeDetails.Split(new string[] { " " }, StringSplitOptions.None);
                                Array.Reverse(fields);
                                if (fields != null)
                                {
                                    fields[ApplicationConstants.IndexOfNotional] = ApplicationConstants.CONST_CensorValue;
                                    fields[ApplicationConstants.IndexOfTradePrice] = ApplicationConstants.CONST_CensorValue;
                                }
                                Array.Reverse(fields);
                                data.TradeDetails = string.Join(" ", fields);
                            }
                            data.TradePrice = null;
                        }
                        //To check the new basket Id is exist or not in Grid DataSource 
                        int countBasketNameExist = _pendingApprovalDataCache.Count(dataCache => dataCache.BasketName.Equals(data.BasketName));

                        if ((!_pendingApprovalDataCache.Contains(data)) && countBasketNameExist == 0)
                            _pendingApprovalDataCache.Add(data);
                    }

                    ultraPendingApprovalGrid.DataSource = _pendingApprovalDataCache;
                    SetThresholdAndActualResult();
                    SetCensorValues();
                    //add check box column
                    AddCheckBoxPendingApprovalGrid(ultraPendingApprovalGrid);
                    SetPropertiesForColumns();
                    UpdatePendingApprovalGridFrozeUnfroze();
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
        /// Updates threshold and actual result's cell value
        /// </summary>
        private void SetThresholdAndActualResult()
        {
            try
            {
                foreach (UltraGridRow row in ultraPendingApprovalGrid.Rows)
                {
                    foreach (UltraGridRow childRow in row.ChildBands[0].Rows)
                    {
                        if (childRow.Cells[PendingApprovalConstants.CONST_CONSTRAINT_FIELDS].Text.Contains(PendingApprovalConstants.CONST_SEPARATOR_CHAR))
                        {
                            childRow.Cells[PendingApprovalConstants.CONST_THRESHOLD].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.URL;
                            childRow.Cells[PendingApprovalConstants.CONST_ACTUAL_RESULT].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.URL;
                            childRow.Cells[PendingApprovalConstants.CONST_THRESHOLD].Value = PendingApprovalConstants.CONST_MULTIPLE;
                            childRow.Cells[PendingApprovalConstants.CONST_ACTUAL_RESULT].Value = PendingApprovalConstants.CONST_MULTIPLE;
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
        /// To Set the censor value of the fields if user is not valid and market data is sapi and is blocked
        /// </summary>
        private void SetCensorValues()
        {
            try 
            {
                List<string> ListOfFieldData = new List<string>(ComplainceConstants.CONST_FieldDataStr.Split(','));
                if (CachedDataManager.IsMarketDataBlocked && CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI
                        && !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                {
                    foreach (UltraGridRow row in ultraPendingApprovalGrid.Rows)
                    {
                        foreach (UltraGridRow childRow in row.ChildBands[0].Rows)
                        {
                            List<string> fieldsList = new List<string>();
                            foreach (var field in ListOfFieldData)
                            {
                                fieldsList.Add(field.ToLower());
                            }
                            string ConstraintFields = childRow.Cells[PendingApprovalConstants.CONST_CONSTRAINT_FIELDS].OriginalValue.ToString();
                            if (!ConstraintFields.Contains(ComplainceConstants.CONST_SEPARATOR_CHAR) && !fieldsList.Contains(ConstraintFields.ToLower()) && !ConstraintFields.Equals(ComplainceConstants.CONST_NA))
                            {
                                childRow.Cells[PendingApprovalConstants.CONST_ACTUAL_RESULT].Value = ApplicationConstants.CONST_CensorValue;
                                childRow.Cells[PendingApprovalConstants.CONST_THRESHOLD].Value = ApplicationConstants.CONST_CensorValue;
                            }
                            childRow.Cells[PendingApprovalConstants.CONST_DESCRIPTION].Value = ApplicationConstants.CONST_CensorValue;
                            childRow.Cells[PendingApprovalConstants.CONST_PARAMETERS].Value = ApplicationConstants.CONST_CensorValue;
                            childRow.Cells[PendingApprovalConstants.CONST_SUMMARY].Value = ApplicationConstants.CONST_CensorValue;
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

        #region Grid column properties
        /// <summary>
        /// Set column properties to define min and max width and to determine the order of appearence
        /// </summary>
        private void SetPropertiesForColumns()
        {
            try
            {
                if (!this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns.Exists(PendingApprovalConstants.CONST_APPROVE_BUTTON))
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns.Add(PendingApprovalConstants.CONST_APPROVE_BUTTON, "");

                if (!this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns.Exists(PendingApprovalConstants.CONST_BLOCK_BUTTON))
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns.Add(PendingApprovalConstants.CONST_BLOCK_BUTTON, "");

                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_APPROVE_BUTTON].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_BLOCK_BUTTON].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;

                //to print ' N/A ' in case of multi trade or invalid trade
                if (CachedDataManager.IsMarketDataBlocked && CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI
                     && !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                   this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_TRADE_PRICE].NullText = ComplainceConstants.CONST_CensorValue;
                else
                   this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_TRADE_PRICE].NullText = "N/A";

                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_QUANTITY].NullText = "N/A";

                // this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_COMPLIANCE_OFFICER_NOTES].MinWidth = 150;

                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_APPROVE_BUTTON].MinWidth = 64;
                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_APPROVE_BUTTON].MaxWidth = 64;

                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_BLOCK_BUTTON].MinWidth = 58;
                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_BLOCK_BUTTON].MaxWidth = 58;

                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_APPROVE_BUTTON].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                this.ultraPendingApprovalGrid.Rows.Band.Columns[PendingApprovalConstants.CONST_APPROVE_BUTTON].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_APPROVE_BUTTON].CellButtonAppearance.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_APPROVE_BUTTON].CellButtonAppearance.ForeColor = System.Drawing.Color.White;
                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_APPROVE_BUTTON].CellButtonAppearance.TextHAlign = HAlign.Center;
                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_APPROVE_BUTTON].CellButtonAppearance.TextVAlign = VAlign.Middle;

                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Override.ButtonStyle = UIElementButtonStyle.Button3D;
                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Override.BorderStyleCell = UIElementBorderStyle.Rounded1Etched;

                // this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Override.AllowGroupBy = DefaultableBoolean.False;
                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_BLOCK_BUTTON].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                this.ultraPendingApprovalGrid.Rows.Band.Columns[PendingApprovalConstants.CONST_BLOCK_BUTTON].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_BLOCK_BUTTON].CellButtonAppearance.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_BLOCK_BUTTON].CellButtonAppearance.ForeColor = System.Drawing.Color.White;
                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_BLOCK_BUTTON].CellButtonAppearance.TextHAlign = HAlign.Center;
                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_BLOCK_BUTTON].CellButtonAppearance.TextVAlign = VAlign.Middle;

                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_APPROVE_BUTTON].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_BLOCK_BUTTON].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_APPROVE_BUTTON].Header.VisiblePosition = 1;
                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_BLOCK_BUTTON].Header.VisiblePosition = 2;

                if (this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns.Exists(ComplainceConstants.CONST_USER_NAME))
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_USER_NAME].Header.Caption = PendingApprovalConstants.CAP_USER_NAME;

                //for child bands
                for (int i = 1; i < 2; i++)
                {
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_VALIDATION_TIME].Format = "MM/dd/yyyy HH:mm:ss";
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_VALIDATION_TIME].SortIndicator = SortIndicator.Descending;

                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_RULE_ID].Hidden = true;
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_RULE_ID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_OVERRIDE_USER_ID].Hidden = true;
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_OVERRIDE_USER_ID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_USER_ID].Hidden = true;
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_USER_ID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_ISVIOLATED].Hidden = true;
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_ISVIOLATED].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_ISEOM].Hidden = true;
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_ISEOM].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_BLOCKED].Hidden = true;
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_BLOCKED].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_STATUS].Hidden = true;
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_STATUS].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_GROUPID].Hidden = true;
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_GROUPID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_ACTION_USER].Hidden = true;
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_ACTION_USER].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_CONSTRAINT_FIELDS].Hidden = true;
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_CONSTRAINT_FIELDS].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_COMPLIANCE_OFFICER_NOTES].Hidden = true;
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_COMPLIANCE_OFFICER_NOTES].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_TRADE_DETAILS].Hidden = true;
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_TRADE_DETAILS].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                    if (this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns.Exists(PendingApprovalConstants.CONST_DESCRIPTION))
                        this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[PendingApprovalConstants.CONST_DESCRIPTION].Header.Caption = "Description Of Rule";

                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[i].HeaderVisible = true;

                }
                //Deactivate all columns except checkbox. Because we do not want to update any other values in the pending approval grid.
                //Make all numerical columns comma separated and right aligned
                for (int i = 0; i < 2; i++)
                {
                    foreach (UltraGridColumn col in ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns)
                    {
                        if (!col.Key.ToString().Equals(PendingApprovalConstants.CONST_CHECKBOX))
                            col.CellActivation = Activation.NoEdit;
                        if (col.DataType == typeof(double) || col.DataType == typeof(double?))
                        {
                            col.Format = "###,##0.################";
                            col.CellAppearance.TextHAlign = HAlign.Right;
                        }
                    }
                }

                this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_COMPLIANCE_OFFICER_NOTES].CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
                this.ultraPendingApprovalGrid.DisplayLayout.Bands[1].HeaderVisible = true;
                this.ultraPendingApprovalGrid.DisplayLayout.Bands[1].Header.Caption = PendingApprovalConstants.CONST_ALERTS;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Initialize Row Event to set property
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraPendingApprovalGrid_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.Cells.Exists(PendingApprovalConstants.CONST_APPROVE_BUTTON))
                    e.Row.Cells[PendingApprovalConstants.CONST_APPROVE_BUTTON].Value = PendingApprovalConstants.CONST_APPROVE;
                if (e.Row.Cells.Exists(PendingApprovalConstants.CONST_BLOCK_BUTTON))
                    e.Row.Cells[PendingApprovalConstants.CONST_BLOCK_BUTTON].Value = PendingApprovalConstants.CONST_BLOCK;

                //If row band index is 0, the check for approve/block button state and set order status
                if (e.Row.Band.Index == 0)
                {
                    bool result = false;
                    if (GetApproveBlockButtonState(e.Row.Cells[PendingApprovalConstants.CONST_BASKET_NAME].Value.ToString()))
                    {
                        if (e.Row.Cells.Exists(PendingApprovalConstants.CONST_APPROVE_BUTTON))
                            e.Row.Cells[PendingApprovalConstants.CONST_APPROVE_BUTTON].Activation = Activation.Disabled;
                        if (e.Row.Cells.Exists(PendingApprovalConstants.CONST_BLOCK_BUTTON))
                            e.Row.Cells[PendingApprovalConstants.CONST_BLOCK_BUTTON].Activation = Activation.Disabled;
                        if (e.Row.Cells.Exists(PendingApprovalConstants.CONST_CHECKBOX))
                        {
                            //Disabling checkboxes so that the row is not included in bulk flow
                            e.Row.Cells[PendingApprovalConstants.CONST_CHECKBOX].Activation = Activation.Disabled;
                        }
                        result = true;
                    }

                    if (e.Row.Cells.Exists(PendingApprovalConstants.CONST_STATUS))
                    {
                        if (String.IsNullOrEmpty(e.Row.Cells[PendingApprovalConstants.CONST_STATUS].Text.ToString()))
                        {
                            if (result)
                                e.Row.Cells[PendingApprovalConstants.CONST_STATUS].Value = PendingApprovalConstants.CONST_ORDER_STATUS_MESSAGE;
                            else
                                e.Row.Cells[PendingApprovalConstants.CONST_STATUS].Value = PendingApprovalConstants.CONST_ORDER_STATUS_MESSAGE;
                            e.Row.Cells[PendingApprovalConstants.CONST_STATUS].Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                        }
                    }
                }

                //Change the value of Pretrade action type according to description 
                if (e.Row.Band.Index == 1 && e.Row.Cells.Exists(PendingApprovalConstants.CONST_PRE_TRADE_ACTIONTYPE))
                {
                    UltraGridColumn preTradeActionTypeCol = e.Row.Band.Columns[PendingApprovalConstants.CONST_PRE_TRADE_ACTIONTYPE];
                    ValueList preTradeActionTypeValueList = new ValueList();
                    foreach (PreTradeActionType value in Enum.GetValues(typeof(PreTradeActionType)))
                    {
                        preTradeActionTypeValueList.ValueListItems.Add(value.ToString(), EnumHelper.GetDescription(value));
                    }
                    SetValueList(e.Row.Band.Columns, PendingApprovalConstants.CONST_PRE_TRADE_ACTIONTYPE, PendingApprovalConstants.CONST_ACTION_PERFORMED_MESSAGE, preTradeActionTypeValueList);
                }

                //Check that Hard alerts Band are not contains Action User value is -1, then change the row color
                if ((e.Row.Band.Index == 1) && Convert.ToInt32(e.Row.Cells[PendingApprovalConstants.CONST_ACTION_USER].Value) != -1)
                {
                    e.Row.Appearance.ForeColor = Color.Green;
                    e.Row.Cells[PendingApprovalConstants.CONST_PRE_TRADE_ACTIONTYPE].Appearance.ForeColor = Color.Green;
                }

                //If the Override user id doesn't contain current user id, then alert color will be Red
                if ((e.Row.Band.Index == 1) && !((e.Row.Cells[PendingApprovalConstants.CONST_OVERRIDE_USER_ID].Value).ToString().Split(',').Contains(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString())) && Convert.ToInt32(e.Row.Cells[PendingApprovalConstants.CONST_ACTION_USER].Value) == -1)
                {
                    e.Row.Appearance.ForeColor = Color.Red;
                    e.Row.Cells[PendingApprovalConstants.CONST_PRE_TRADE_ACTIONTYPE].Appearance.ForeColor = Color.Red;
                    e.Row.Cells[PendingApprovalConstants.CONST_ALERT_TYPE].Appearance.ForeColor = Color.Red;
                }

                //If the Override user id contain current user id, then alert color will be Blue. Because he can override
                if ((e.Row.Band.Index == 1) && ((e.Row.Cells[PendingApprovalConstants.CONST_OVERRIDE_USER_ID].Value).ToString().Split(',').Contains(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString())) && Convert.ToInt32(e.Row.Cells[PendingApprovalConstants.CONST_ACTION_USER].Value) == -1)
                {
                    e.Row.Appearance.ForeColor = Color.Blue;
                    e.Row.Cells[PendingApprovalConstants.CONST_PRE_TRADE_ACTIONTYPE].Appearance.ForeColor = Color.Blue;
                    e.Row.Cells[PendingApprovalConstants.CONST_ALERT_TYPE].Appearance.ForeColor = Color.Blue;
                }

                //Check that Hard alerts and Soft alerts Band are contains UserId and User name column, then Update user name value
                if ((e.Row.Band.Index == 1) && e.Row.Cells.Exists(PendingApprovalConstants.CONST_USER_ID) && e.Row.Cells.Exists(PendingApprovalConstants.CONST_USER_NAME))
                    e.Row.Cells[PendingApprovalConstants.CONST_USER_NAME].Value = GetUserName(Convert.ToInt32(e.Row.Cells[PendingApprovalConstants.CONST_USER_ID].Value));

                if ((e.Row.Band.Index == 1 && e.Row.Cells.Exists(PendingApprovalConstants.CONST_ALERT_TYPE)))
                {
                    UltraGridColumn alertTypeColumn = e.Row.Band.Columns[PendingApprovalConstants.CONST_ALERT_TYPE];
                    ValueList alertTypeValueList = new ValueList();
                    foreach (AlertType value in Enum.GetValues(typeof(AlertType)))
                    {
                        alertTypeValueList.ValueListItems.Add(value.ToString(), EnumHelper.GetDescription(value));
                    }
                    SetValueList(e.Row.Band.Columns, PendingApprovalConstants.CONST_ALERT_TYPE, "Alert Type", alertTypeValueList);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        private void SetValueList(ColumnsCollection columnsCollection, string colKey, string colCaption, ValueList valueList)
        {
            try
            {
                columnsCollection[colKey].ValueList = valueList;
                columnsCollection[colKey].Header.Caption = colCaption;
                columnsCollection[colKey].CellActivation = Activation.NoEdit;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion

        /// <summary>
        /// Getting State of Approve/ Block button (Disable/Enable)
        /// </summary>
        /// <returns></returns>
        private bool GetApproveBlockButtonState(string basketId)
        {
            try
            {
                bool userIdExist = true;
                int userId = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;

                lock (_pendingApprovallock)
                {
                    foreach (PendingApprovalData data in _pendingApprovalDataCache)
                    {
                        if (data.BasketName.Equals(basketId))
                        {
                            data.Alerts.ForEach(x =>
                            {
                                if ((x.OverrideUserId.Split(',').Contains(userId.ToString())))
                                    userIdExist = false;
                            });
                        }
                    }
                }
                return userIdExist;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return false;
            }
        }

        /// <summary>
        /// Ultra Cell Button Clicked (Approve/ Block)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraPendingApprovalGrid_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                UltraGridRow row = ultraPendingApprovalGrid.ActiveRow;
                if (e.Cell.Value != null)
                {
                    if (e.Cell.Value.ToString().Equals(PendingApprovalConstants.CONST_APPROVE))
                    {
                        if (row != null)
                            PendingApprovalManager.GetInstance().ApproveBlockBtnClicked(PreTradeActionType.Allowed, row.Cells[PendingApprovalConstants.CONST_BASKET_NAME].Value.ToString(), row);
                    }
                    else if (e.Cell.Value.ToString().Equals(PendingApprovalConstants.CONST_BLOCK))
                    {
                        if (row != null)
                            PendingApprovalManager.GetInstance().ApproveBlockBtnClicked(PreTradeActionType.Blocked, row.Cells[PendingApprovalConstants.CONST_BASKET_NAME].Value.ToString(), row);
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
        /// Set Column chooser properties
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraPendingApprovalGrid_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                if (ultraPendingApprovalGrid.DataSource != null)
                {
                    ColumnChooserDialog dlg = new ColumnChooserDialog();
                    dlg.Owner = this.FindForm();
                    UltraGridColumnChooser cc = dlg.ColumnChooserControl;
                    cc.SourceGrid = ultraPendingApprovalGrid;
                    cc.CurrentBand = _currentBand;
                    cc.Style = ColumnChooserStyle.AllColumnsAndChildBandsWithCheckBoxes;
                    cc.MultipleBandSupport = MultipleBandSupport.SingleBandOnly;
                    dlg.Size = new Size(290, 410);
                    dlg.ColumnChooserControl.DisplayLayout.Override.FilterUIType = FilterUIType.FilterRow;
                    dlg.Show();
                    (dlg as Form).PaintDynamicForm();
                    var columnChooserGrid = dlg.ColumnChooserControl.Controls[PendingApprovalConstants.CONST_DISPLAY_GRID] as UltraGrid;
                    columnChooserGrid.InitializeLayout += columnChooserGrid_InitializeLayout;

                    for (int index = 0; index < cc.Controls.Count; index++)
                    {
                        var control = cc.Controls[index];
                        if (control is ComboBox)
                        {
                            cc.Controls.Remove(control);
                        }

                        if (control is UltraGrid)
                        {
                            var grid = (UltraGrid)control;
                            grid.InitializeRow += Grid_InitializeRow;
                        }
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
        /// Adding space between Column Header for each row in Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraPendingApprovalGrid_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                //Grid contains 2 Band, So same will execute 2 times
                for (int i = 0; i < 2; i++)
                {
                    foreach (UltraGridColumn col in ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns)
                    {
                        ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[col.Index].Header.Caption = SplitCamelCase(col.ToString());
                        ultraPendingApprovalGrid.DisplayLayout.Bands[i].Columns[col.Index].CellMultiLine = DefaultableBoolean.True;
                    }
                }

                e.Layout.Override.RowSizing = RowSizing.AutoFree;

                // load the savelayout file if it exists
                string gridLayoutFile = Application.StartupPath + PendingApprovalConstants.CONST_PRANA_PREFERENCES + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + PendingApprovalConstants.CONST_LAYOUT_FILE_NAME;
                if (File.Exists(gridLayoutFile))
                {
                    ultraPendingApprovalGrid.DisplayLayout.LoadFromXml(gridLayoutFile);
                    KeepFixedValuesUnchanged();
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
        /// Splits string in camel case.
        /// </summary>
        /// <param name="p">String</param>
        /// <returns>Split String by Camel case</returns>
        private string SplitCamelCase(string input)
        {
            try
            {
                return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return String.Empty;
            }
        }

        /// <summary>
        /// Save the layout to xml file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                string gridLayoutFile = Application.StartupPath + PendingApprovalConstants.CONST_PRANA_PREFERENCES + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + PendingApprovalConstants.CONST_LAYOUT_FILE_NAME;
                ultraPendingApprovalGrid.DisplayLayout.SaveAsXml(gridLayoutFile);
                MessageBox.Show(this, PendingApprovalConstants.CONST_LAYOUT_SAVED, PendingApprovalConstants.CONST_NIRVANA_COMPLIANCE, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        #region Column chosser properties

        public void ultraPendingApprovalGrid_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            try
            {
                (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private static void columnChooserGrid_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                // UltraGrid grid = (UltraGrid)sender;
                UltraGridLayout layout = e.Layout;
                UltraGridBand band = layout.Bands[0];
                if ((CheckKeyExistence(band, PendingApprovalConstants.CONST_VALUE)))
                {
                    band.Columns[PendingApprovalConstants.CONST_VALUE].FilterOperandStyle = FilterOperandStyle.Edit;
                    band.Columns[PendingApprovalConstants.CONST_VALUE].FilterOperatorDefaultValue = FilterOperatorDefaultValue.Contains;
                    ((UltraGrid)sender).InitializeLayout -= columnChooserGrid_InitializeLayout;
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        private static void Grid_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Index == 0)
                e.Row.Hidden = true;
        }

        private void ultraPendingApprovalGrid_MouseEnterElement(object sender, UIElementEventArgs e)
        {
            try
            {
                if (e.Element is ColumnChooserButtonUIElement)
                    _currentBand = (e.Element as ColumnChooserButtonUIElement).Band;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private static bool CheckKeyExistence(UltraGridBand band, string key)
        {
            try
            {
                foreach (UltraGridColumn col in band.Columns)
                {
                    if (col.Key.Equals(key))
                        return true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return false;
        }

        /// <summary>
        /// Performs the bulk approve.
        /// </summary>
        internal void PerformBulkApprove()
        {
            try
            {
                //Selecting only those rows where checkbox has be enabled
                var selectedRows = ultraPendingApprovalGrid.Rows.OfType<UltraGridRow>().Where(r => r.Cells[PendingApprovalConstants.CONST_CHECKBOX].Text.ToUpper().Equals("TRUE")).ToList();
                if (selectedRows.Count > 0)
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in selectedRows)
                    {
                        if (row != null)
                            PendingApprovalManager.GetInstance().ApproveBlockBtnClicked(PreTradeActionType.Allowed, row.Cells[PendingApprovalConstants.CONST_BASKET_NAME].Value.ToString(), row);
                    }
                }
                else
                    MessageBox.Show(this, PendingApprovalConstants.CONST_NOTHING_SELECTED, PendingApprovalConstants.CONST_NIRVANA_COMPLIANCE, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Performs the bulk block.
        /// </summary>
        internal void PerformBulkBlock()
        {
            try
            {
                //Selecting only those rows where checkbox has be enabled
                var selectedRows = ultraPendingApprovalGrid.Rows.OfType<UltraGridRow>().Where(r => r.Cells[PendingApprovalConstants.CONST_CHECKBOX].Text.ToUpper().Equals("TRUE")).ToList();
                if (selectedRows.Count > 0)
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in selectedRows)
                    {
                        if (row != null)
                            PendingApprovalManager.GetInstance().ApproveBlockBtnClicked(PreTradeActionType.Blocked, row.Cells[PendingApprovalConstants.CONST_BASKET_NAME].Value.ToString(), row);
                    }
                }
                else
                    MessageBox.Show(this, PendingApprovalConstants.CONST_NOTHING_SELECTED, PendingApprovalConstants.CONST_NIRVANA_COMPLIANCE, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        #endregion



        /// <summary>
        /// Adds the CheckBox on the pending approval grid.
        /// </summary>
        /// <param name="ultraPendingApprovalGrid">The ultra pending approval grid.</param>
        private void AddCheckBoxPendingApprovalGrid(UltraGrid ultraPendingApprovalGrid)
        {
            try
            {
                UltraWinGridUtils.AddCheckBox(ultraPendingApprovalGrid);
                ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_CHECKBOX].Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.None;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the AfterHeaderCheckStateChanged event of the ultraPendingApprovalGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AfterHeaderCheckStateChangedEventArgs"/> instance containing the event data.</param>
        private void ultraPendingApprovalGrid_AfterHeaderCheckStateChanged(object sender, AfterHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                //To select only filtered and enabled rows
                List<UltraGridRow> rows = ultraPendingApprovalGrid.Rows.GetFilteredInNonGroupByRows().Where(r =>
                    r.Cells[PendingApprovalConstants.CONST_CHECKBOX].Activation != Activation.Disabled).ToList();

                foreach (UltraGridRow row in rows)
                {
                    if (this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_CHECKBOX].GetHeaderCheckedState(ultraPendingApprovalGrid.Rows) == CheckState.Checked)
                        row.Cells[PendingApprovalConstants.CONST_CHECKBOX].Value = true;
                    else
                        row.Cells[PendingApprovalConstants.CONST_CHECKBOX].Value = false;
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
        /// Handles the AfterColPosChanged event of the ultraPendingApprovalGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AfterColPosChangedEventArgs"/> instance containing the event data.</param>
        private void ultraPendingApprovalGrid_AfterColPosChanged(object sender, AfterColPosChangedEventArgs e)
        {
            try
            {
                //this is done in order to keep the position of these three columns always fixed in the same order and same position
                if (this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns.Exists(PendingApprovalConstants.CONST_CHECKBOX))
                {
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_CHECKBOX].Header.VisiblePosition = 0;
                }
                if (this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns.Exists(PendingApprovalConstants.CONST_APPROVE_BUTTON))
                {
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_APPROVE_BUTTON].Header.VisiblePosition = 1;
                }
                if (this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns.Exists(PendingApprovalConstants.CONST_BLOCK_BUTTON))
                {
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_BLOCK_BUTTON].Header.VisiblePosition = 2;
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
        /// Keeps the fixed values unchanged even on loading a saved layout file.
        /// </summary>
        private void KeepFixedValuesUnchanged()
        {
            try
            {
                //this is done to enable checkboxes even if the user loads a previously saved layout
                this.ultraPendingApprovalGrid.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
                //this is done in order to keep the position of these two columns always fixed in the same order and same position
                if (this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns.Exists(PendingApprovalConstants.CONST_APPROVE_BUTTON))
                {
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_APPROVE_BUTTON].Header.VisiblePosition = 1;
                }
                if (this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns.Exists(PendingApprovalConstants.CONST_BLOCK_BUTTON))
                {
                    this.ultraPendingApprovalGrid.DisplayLayout.Bands[0].Columns[PendingApprovalConstants.CONST_BLOCK_BUTTON].Header.VisiblePosition = 2;
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
        /// Handles the ClickCell event of the ultraPendingApprovalGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ClickCellEventArgs"/> instance containing the event data.</param>
        private void ultraPendingApprovalGrid_CellClicked(object sender, ClickCellEventArgs e)
        {
            try
            {
                if (ultraPendingApprovalGrid.DataSource != null && ultraPendingApprovalGrid.ActiveCell != null && ultraPendingApprovalGrid.ActiveCell.Text != null && ultraPendingApprovalGrid.ActiveCell.Text.Contains(PendingApprovalConstants.CONST_MULTIPLE))
                {
                    string constraintFields = ultraPendingApprovalGrid.ActiveRow.Cells[PendingApprovalConstants.CONST_CONSTRAINT_FIELDS].OriginalValue.ToString();
                    string threshold = ultraPendingApprovalGrid.ActiveRow.Cells[PendingApprovalConstants.CONST_THRESHOLD].OriginalValue.ToString();
                    string actualResult = ultraPendingApprovalGrid.ActiveRow.Cells[PendingApprovalConstants.CONST_ACTUAL_RESULT].OriginalValue.ToString();
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
        public void ExportData(string gridName, string filePath)
        {
            try
            {
                string folder = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                UltraGridExcelExporter exporter = new UltraGridExcelExporter();
                if (gridName == "ultraPendingApprovalGrid")
                {
                    exporter.Export(ultraPendingApprovalGrid, filePath);
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

    }
}

