using Infragistics.Win.UltraWinGrid;
using Prana.Blotter.BusinessObjects;
using Prana.Blotter.Classes;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.Classes.Blotter;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientPreferences;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.TradeManager.Extension;
using Prana.Utilities.ImportExportUtilities;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Blotter
{
    public partial class OrderBlotterGrid : WorkingSubBlotterGrid
    {
        public OrderBlotterGrid()
        {
            InitializeComponent();
        }

        ISecurityMasterServices _securityMaster = null;
        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                _securityMaster = value;
                _securityMaster.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(_securityMaster_SecMstrDataResponse);
            }
        }

        CompanyUser _loginUser = null;
        public override event EventHandler TradeClick = null;
        public override event EventHandler ReplaceOrEditOrderClicked = null;
        private bool[] _rowsFilterOunt;
        List<TradeAuditEntry> _auditCollection = new List<TradeAuditEntry>();

        /// <summary>
        /// The _allocation proxy
        /// </summary>
        public new ProxyBase<IAllocationManager> _allocationProxy;

        /// <summary>
        /// Initializes the contol.
        /// </summary>
        /// <param name="blotterOdrColl">The blotter odr coll.</param>
        /// <param name="key">The key.</param>
        /// <param name="loginUser">The login user.</param>
        /// <param name="blotterPreferenceData">The blotter color prefs.</param>
        public override void InitContol(OrderBindingList blotterOdrColl, string key, CompanyUser loginUser, BlotterPreferenceData blotterPreferenceData)
        {
            try
            {
                _loginUser = loginUser;

                base.InitContol(blotterOdrColl, key, loginUser, blotterPreferenceData);
                string keyValue = OrderFields.BlotterTypes.SubOrders.ToString();
                if (key.StartsWith("Dynamic_Order_"))
                {
                    keyValue = key.Replace(BlotterConstants.CAPTION_ORDER_SEPARATOR, BlotterConstants.CAPTION_SUBORDER_SEPARATOR);
                }
                _allocationProxy = new ProxyBase<IAllocationManager>(BlotterConstants.LIT_ALLOCATION_END_POINT_ADDRESS_NAME);
                SubOrderBlotterGrid.InitContol(new OrderBindingList(), keyValue, loginUser, blotterPreferenceData);
                AddCheckBoxinGrid(dgBlotter);
                dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_INTERNALCOMMENTS].CellActivation = Activation.AllowEdit;


                //Setting format for some columns
                dgBlotter.DisplayLayout.Bands[0].Columns[Global.OrderFields.PROPERTY_DAY_AVERAGE_PRICE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                dgBlotter.DisplayLayout.Bands[0].Columns[Global.OrderFields.PROPERTY_DAY_EXECUTED_QUANTITY].Format = ApplicationConstants.FORMAT_QTY;
                dgBlotter.DisplayLayout.Bands[0].Columns[Global.OrderFields.PROPERTY_PERCENTEXECUTED].Format = ApplicationConstants.FORMAT_QTY;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        protected override void dgBlotter_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                base.dgBlotter_InitializeLayout(sender, e);
                e.Layout.Override.DefaultRowHeight = 20;

                foreach (UltraGridBand band in e.Layout.Bands)
                {
                    if (band.Index > 1)
                    {
                        band.Hidden = true;
                    }
                }
                if (!CustomThemeHelper.ApplyTheme)
                {
                    Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
                    appearance1.BackColor = System.Drawing.Color.Black;
                    this.dgBlotter.DisplayLayout.Appearance = appearance1;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        internal OrderBindingList GetCheckedRows()
        {
            OrderBindingList listCheckedOrders = new OrderBindingList();
            try
            {
                if (dgBlotter.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CHKBOX] != null)
                {
                    UltraGridRow[] rows = dgBlotter.Rows.GetFilteredInNonGroupByRows();
                    foreach (UltraGridRow row in rows)
                    {
                        if (row.Cells[OrderFields.PROPERTY_CHKBOX].Text == true.ToString())
                        {
                            listCheckedOrders.Add((OrderSingle)row.ListObject);
                        }
                    }
                }
                if (listCheckedOrders.Count == 0)
                {
                    if (dgBlotter.ActiveRow != null && dgBlotter.ActiveRow.ToString() != "Infragistics.Win.UltraWinGrid.UltraGridFilterRow")
                    {
                        OrderSingle order = dgBlotter.ActiveRow.ListObject as OrderSingle;
                        listCheckedOrders.Add(order);
                    }
                    // else just pop up an empty trading ticket but if we are in Order Blotter
                    // we must give him a warning to click on some line so that a sub order can 
                    // be generated.
                    else
                    {
                        //MessageBox.Show("Please select an Order to Generate a Sub");
                        UpdateBlotterStatusBar("Please select an Order to Generate a Sub");
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return listCheckedOrders;
        }
        /// <summary>
        /// Get Checked Count
        /// </summary>
        /// <returns></returns>
        internal string GetCheckedCount()
        {
            int rowsCount = 0;
            double selectedTargetQtySUM = 0;
            try
            {
                foreach (UltraGridRow row in dgBlotter.Rows)
                {
                    if (row.Cells[OrderFields.PROPERTY_CHKBOX].Text == true.ToString())
                    {
                        rowsCount++;
                        selectedTargetQtySUM += Convert.ToDouble(row.Cells[OrderFields.PROPERTY_QUANTITY].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return rowsCount >= 1 ? ("Count(Selected Orders) : " + rowsCount + "  ;  Sum(Target Qty) : " + selectedTargetQtySUM.ToString("#,##0")) : "";
        }

        private OrderBindingList VerifyTradesList(OrderBindingList listCheckedRows, System.EventArgs e)
        {
            TranferTradeRules transferTradeRules = CachedDataManager.GetInstance.GetTransferTradeRules();
            List<int> errorCodes = new List<int>();
            OrderBindingList listCheckedValidatedRows = new OrderBindingList();
            if (!transferTradeRules.IsAllowUserToGenerateSub)
            {
                foreach (OrderSingle order in listCheckedRows)
                {
                    if (order.CompanyUserID.Equals(_loginUser.CompanyUserID))
                    {
                        ValidateTradesInList(ref errorCodes, ref listCheckedValidatedRows, order);
                    }
                    else
                    {
                        if (!errorCodes.Contains(BlotterConstants.ErrorCode_UserNotAllowed))
                            errorCodes.Add(BlotterConstants.ErrorCode_UserNotAllowed);
                    }
                }
            }
            else
            {
                foreach (OrderSingle order in listCheckedRows)
                {
                    ValidateTradesInList(ref errorCodes, ref listCheckedValidatedRows, order);
                }
            }
            TradingTicketPrefManager.GetInstance.Initialise(Prana.BusinessObjects.AppConstants.TradingTicketPreferenceType.Company, CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, CommonDataCache.CachedDataManager.GetInstance.GetCompanyID());
            TradingTicketPrefManager.GetInstance.GetPreferenceBindingData(false, false);
            bool? isShowTargetQTY = TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.IsShowTargetQTY;
            foreach (int errorCode in errorCodes)
            {
                switch (errorCode)
                {
                    case BlotterConstants.ErrorCode_UserNotAllowed:
                        if (listCheckedRows.Count > 1)
                        {
                            MessageBox.Show("You do not have permissions to generate new subs for few Trades", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("You do not have permissions to generate new subs for this Trade", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    case BlotterConstants.ErrorCode_Cancelled:
                        if (listCheckedRows.Count > 1)
                        {
                            MessageBox.Show("Cannot Trade few orders in Cancelled state.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Cannot Trade order in Cancelled state.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        break;
                    case BlotterConstants.ErrorCode_TargetQtyFull:
                        if (listCheckedRows.Count > 1)
                        {
                            if (isShowTargetQTY.HasValue && isShowTargetQTY.Value)
                                MessageBox.Show("Cannot Generate Sub Order Since Target Quantity = Working Quantity for few trades.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            else
                                MessageBox.Show("This additional sub order exceeds the combined remaining, working, and executed quantities for few trades.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            if (isShowTargetQTY.HasValue && isShowTargetQTY.Value)
                            {
                                if (DialogResult.OK == MessageBox.Show("Cannot Generate Sub Order Since Target Quantity = Working Quantity. Launch ticket for new order?", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
                                {
                                    TradeClick(null, e);
                                }
                            }
                            else
                                if (DialogResult.OK == MessageBox.Show("This additional sub order exceeds the combined remaining, working, and executed quantities. Please launch a new trade, or increase the parent order quantity", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
                            {
                                TradeClick(null, e);
                            }

                        }
                        break;
                }
            }
            return listCheckedValidatedRows;
        }

        private void ValidateTradesInList(ref List<int> errorCodes, ref OrderBindingList listCheckedValidatedRows, OrderSingle order)
        {
            if (!order.OrderStatus.Equals("Cancelled"))
            {
                OrderSingle subOrder = (OrderSingle)order.Clone();
                subOrder.MsgType = FIXConstants.MSGOrder;
                subOrder.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDNewSub;
                subOrder.StagedOrderID = order.ParentClOrderID;

                //Bug Fix: OrigClOrderID of Stage order is cloned to newly sub although sub is not replaced yet.
                //https://jira.nirvanasolutions.com:8443/browse/PRANA-14469
                subOrder.OrigClOrderID = int.MinValue.ToString();
                subOrder.LastPrice = 0.0;
                subOrder.LastShares = 0.0;
                ValidationManagerExtension.GetOrderDetails(subOrder);
                if (subOrder.UnsentQty != double.MinValue)
                {
                    if (subOrder.UnsentQty > 0.0)
                    {
                        subOrder.Quantity = order.UnsentQty;
                        listCheckedValidatedRows.Add(subOrder);
                    }
                    else
                    {
                        if (!errorCodes.Contains(BlotterConstants.ErrorCode_TargetQtyFull))
                            errorCodes.Add(BlotterConstants.ErrorCode_TargetQtyFull);
                    }
                }
                else
                {
                    subOrder.Quantity = order.Quantity;
                    listCheckedValidatedRows.Add(subOrder);
                }
            }
            else
            {
                if (!errorCodes.Contains(BlotterConstants.ErrorCode_Cancelled))
                    errorCodes.Add(BlotterConstants.ErrorCode_Cancelled);
            }
        }

        /// <summary>
        /// Handles the Click event of the menuRepeatTrade control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        protected override void menuRepeatTrade_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (TradeClick != null)
                {
                    //if there is an active row then get the order details
                    OrderBindingList listCheckedRows = GetCheckedRows();
                    if (listCheckedRows.Count == 1)
                    {
                        OrderSingle or = (OrderSingle)dgBlotter.ActiveRow.ListObject;
                        OrderSingle orderRequest = GetReloadOrder(or, true);
                        TradeClick(orderRequest, e);
                    }
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

        protected override void menuTrade_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (TradeClick != null)
                {
                    //if there is an active row then get the order details
                    OrderBindingList listCheckedRows = GetCheckedRows();
                    OrderBindingList listCheckedValidatedRows = VerifyTradesList(listCheckedRows, e);

                    if (listCheckedRows.Count > 1)
                    {
                        TradeClick(listCheckedValidatedRows, e);
                    }
                    else
                    {
                        if (listCheckedRows.Count == 1)
                        {
                            if (listCheckedValidatedRows.Count > 0)
                                TradeClick(listCheckedValidatedRows[0], e);
                        }
                    }
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

        private void AddCheckBoxinGrid(UltraGrid grid)
        {
            try
            {
                if (!grid.DisplayLayout.Bands[0].Columns.Exists(OrderFields.PROPERTY_CHKBOX))
                {
                    grid.DisplayLayout.Bands[0].Columns.Add(OrderFields.PROPERTY_CHKBOX, "");
                }
                grid.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CHKBOX].DataType = typeof(bool);
                grid.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CHKBOX].CellClickAction = CellClickAction.Edit;
                grid.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CHKBOX].Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                grid.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CHKBOX].Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                grid.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CHKBOX].CellActivation = Activation.AllowEdit;
                SetCheckBoxAtFirstPosition(grid);
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

        private void SetCheckBoxAtFirstPosition(UltraGrid grid)
        {
            try
            {
                grid.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CHKBOX].Hidden = false;
                grid.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CHKBOX].Header.VisiblePosition = 0;
                if (grid.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CHKBOX].Width < 30)
                    grid.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CHKBOX].Width = 30;
                grid.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CHKBOX].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
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

        protected override void menuEditOrReplaceOrder_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (ReplaceOrEditOrderClicked != null)
                {
                    bool isCancelled = false;
                    bool isPemissionNeeded = false;
                    TranferTradeRules transferTradeRules = CachedDataManager.GetInstance.GetTransferTradeRules();
                    OrderBindingList listCheckedRows = GetCheckedRows();
                    OrderBindingList ordersToEdit = new OrderBindingList();
                    foreach (OrderSingle or in listCheckedRows)
                    {
                        if (!or.OrderStatus.Equals("Cancelled"))
                        {
                            if (!transferTradeRules.IsAllowAllUserToCancelReplaceRemove)
                            {
                                if (or.CompanyUserID.Equals(_loginUser.CompanyUserID))
                                {
                                    OrderSingle orRequest = (OrderSingle)or.Clone();
                                    orRequest.MsgType = FIXConstants.MSGOrderCancelReplaceRequest;
                                    orRequest.ModifiedUserId = _loginUser.CompanyUserID;
                                    ValidationManagerExtension.GetOrderDetails(orRequest);
                                    ordersToEdit.Add(orRequest);

                                    if (TradeManager.ValidationManager.IsOrderReplaceable(orRequest, or.CompanyUserID))
                                    {
                                        OrderSingle replaceOrder = (OrderSingle)orRequest.Clone();
                                        replaceOrder.MsgType = FIXConstants.MSGOrderCancelRequestFroze;
                                        TradeManagerExtension.GetInstance().SendMessageToPendingApprovalUI(replaceOrder);
                                    }
                                }
                                else
                                {
                                    isPemissionNeeded = true;
                                }
                            }
                            else
                            {
                                OrderSingle orRequest = (OrderSingle)or.Clone();
                                orRequest.MsgType = FIXConstants.MSGOrderCancelReplaceRequest;
                                orRequest.ModifiedUserId = _loginUser.CompanyUserID;
                                ValidationManagerExtension.GetOrderDetails(orRequest);
                                ordersToEdit.Add(orRequest);

                                if (TradeManager.ValidationManager.IsOrderReplaceable(orRequest, or.CompanyUserID))
                                {
                                    OrderSingle replaceOrder = (OrderSingle)orRequest.Clone();
                                    replaceOrder.MsgType = FIXConstants.MSGOrderCancelRequestFroze;
                                    TradeManagerExtension.GetInstance().SendMessageToPendingApprovalUI(replaceOrder);
                                }
                            }
                        }
                        else
                        {
                            isCancelled = true;
                        }
                    }

                    if (isCancelled)
                        UpdateBlotterStatusBar("Cannot Edit order in Cancelled state.");
                    if (isPemissionNeeded)
                    {
                        string message = listCheckedRows.Count > 1 ? " does not have permissions to Edit some Trades" : " does not have permissions to Edit this Trade";
                        MessageBox.Show(_loginUser.ShortName.ToUpper() + message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    if (ordersToEdit.Count == 1)
                    {
                        ReplaceOrEditOrderClicked(ordersToEdit[0], e);
                    }
                    else if (ordersToEdit.Count > 1)
                    {
                        ReplaceOrEditOrderClicked(ordersToEdit, e);
                    }
                }
                else
                {
                    //MessageBox.Show("Please select an order to Edit!");
                    UpdateBlotterStatusBar("Please select an order to Edit!");
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

        protected override void menuCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgBlotter.ActiveRow != null)
                    CancelAllSubs(new List<UltraGridRow>() { dgBlotter.ActiveRow }, "Do you want to cancel sub order(s) of this order?");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Removes the orders BTN clicked.
        /// </summary>
        internal void CancelAllSubsSelectedOrders()
        {
            try
            {
                List<UltraGridRow> selectBlotterOrderRows = dgBlotter.Rows.GetFilteredInNonGroupByRows().Where(row => row.Cells[OrderFields.PROPERTY_CHKBOX].Text == true.ToString()).ToList();
                CancelAllSubs(selectBlotterOrderRows, "Do you want to cancel sub order(s) of selected order(s)?");
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void CancelAllSubs(List<UltraGridRow> selectBlotterOrderRows, string message)
        {
            try
            {
                //If order doesn't have any sub order(s)
                if (selectBlotterOrderRows.Count == 1 && ((OrderSingle)selectBlotterOrderRows.FirstOrDefault().ListObject).OrderCollection == null)
                {
                    UpdateBlotterStatusBar("Order doesn't have any sub order(s)");
                    return;
                }

                if (selectBlotterOrderRows.Count() > 0)
                {
                    foreach (UltraGridRow row in selectBlotterOrderRows)
                    {
                        OrderSingle currentOrder = (OrderSingle)row.ListObject;
                        if (currentOrder.OrderCollection != null)
                        {
                            foreach (OrderSingle subOrder in currentOrder.OrderCollection)
                            {
                                if (subOrder != null)
                                {
                                    OrderSingle cancelSubOrder = (OrderSingle)subOrder.Clone();
                                    cancelSubOrder.MsgType = FIXConstants.MSGOrderCancelRequestFroze;
                                    cancelSubOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingCancel;
                                    cancelSubOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(cancelSubOrder.OrderStatusTagValue);
                                    TradeManagerExtension.GetInstance().SendMessageToPendingApprovalUI(cancelSubOrder);
                                }
                            }
                        }
                    }
                    DialogResult result = MessageBox.Show(this, message, BlotterConstants.PROPERTY_NIRVANA_BLOTTER, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    bool isAllOrdersCancellable = true;
                    if (result == DialogResult.Yes)
                    {
                        OrderBindingList cancelOrderCollection = new OrderBindingList();
                        foreach (UltraGridRow row in selectBlotterOrderRows)
                        {
                            OrderSingle currentOrder = (OrderSingle)row.ListObject;
                            if (currentOrder.OrderCollection != null && currentOrder.OrderCollection.Count > 0)
                            {
                                bool isAllSubOrdersCancellable = CancelSubOrders(cancelOrderCollection, currentOrder);
                                if (!isAllSubOrdersCancellable)
                                    isAllOrdersCancellable = false;
                            }
                        }

                        if (cancelOrderCollection.Count > 0)
                        {
                            var orderList = TradeManagerInstance.SendGroupCancelOrRolloverRequest(cancelOrderCollection);
                            if (isAllOrdersCancellable)
                            {
                                UpdateBlotterStatusBar("Sub order(s) has been cancelled.");
                                foreach (var item in orderList)
                                {
                                    AddAuditTrailCollection(item, TradeAuditActionType.ActionType.SubOrderCancelRequested, "Sub Order cancel requested");
                                }

                                // Save audit trail data
                                SaveAuditTrailData();
                            }
                        }

                        //If some order can not be cancel
                        if (!isAllOrdersCancellable)
                            UpdateBlotterStatusBar("One or more of the sub(s) selected couldn't be cancelled due to their pending fix status or not permitted to cancel.");
                    }
                    else
                    {
                        foreach (UltraGridRow row in selectBlotterOrderRows)
                        {
                            OrderSingle currentOrder = (OrderSingle)row.ListObject;
                            if (currentOrder.OrderCollection != null)
                            {
                                foreach (OrderSingle subOrder in currentOrder.OrderCollection)
                                {
                                    OrderSingle cancelSubOrder = (OrderSingle)subOrder.Clone();
                                    cancelSubOrder.MsgType = FIXConstants.MSGOrderCancelRequestUnFroze;
                                    cancelSubOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingCancel;
                                    cancelSubOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(cancelSubOrder.OrderStatusTagValue);
                                    TradeManagerExtension.GetInstance().SendMessageToPendingApprovalUI(cancelSubOrder);
                                }
                            }
                        }
                    }
                }
                else
                {
                    UpdateBlotterStatusBar(BlotterConstants.ORDER_PLEASE_SELECT_MESSAGE);
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
        /// Add Audit Trail Collection
        /// </summary>
        /// <param name="orRequest"></param>
        /// <param name="action"></param>
        private void AddAuditTrailCollection(OrderSingle orRequest, TradeAuditActionType.ActionType action, string comment = "", bool isMergeOrderAudit = false)
        {
            try
            {
                TradeAuditEntry audit = new TradeAuditEntry();
                audit.Action = action;
                // audit.OriginalValue = originalUser.ToString();
                audit.AUECLocalDate = DateTime.Now;
                audit.OriginalDate = orRequest.AUECLocalDate;
                if (!string.IsNullOrEmpty(comment))
                {
                    audit.Comment = comment;
                }
                else
                {
                    TradeAuditActionTypeConverter ac = TypeDescriptor.GetConverter(typeof(TradeAuditActionType.ActionType)) as TradeAuditActionTypeConverter;
                    audit.Comment = (string)ac.ConvertTo(null, System.Globalization.CultureInfo.CurrentCulture, action, typeof(string));
                }
                audit.CompanyUserId = _loginUser.CompanyUserID;
                audit.GroupID = string.Empty;
                audit.TaxLotID = string.Empty;
                audit.ParentClOrderID = orRequest.ParentClOrderID;
                audit.ClOrderID = orRequest.ClOrderID;
                audit.Symbol = orRequest.Symbol;
                audit.Level1ID = orRequest.Level1ID;
                audit.OrderSideTagValue = orRequest.OrderSideTagValue;
                audit.Source = Prana.BusinessObjects.TradeAuditActionType.ActionSource.Blotter;
                if (isMergeOrderAudit)
                {
                    audit.OriginalValue = orRequest.ClOrderID;
                }
                _auditCollection.Add(audit);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Save Audit Trail Data
        /// </summary>
        internal void SaveAuditTrailData(bool isMergedOrder = false, string finalStageOrderId = "")
        {
            try
            {
                if (isMergedOrder && _auditCollection.Count > 0)
                {
                    _auditCollection.ForEach(order =>
                    {
                        order.NewValue = finalStageOrderId;
                    });
                }
                var IsAuditSaved = AuditManager.Instance.SaveAuditList(_auditCollection);
                if (IsAuditSaved)
                {
                    _auditCollection.Clear();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        private bool CancelSubOrders(OrderBindingList cancelOrderCollection, OrderSingle currentOrder)
        {
            bool isAllSubOrdersCancellable = true;
            try
            {
                foreach (OrderSingle subOrder in currentOrder.OrderCollection)
                {
                    TranferTradeRules transferTradeRules = CachedDataManager.GetInstance.GetTransferTradeRules();
                    if (!transferTradeRules.IsAllowAllUserToCancelReplaceRemove)
                    {
                        if (subOrder.CompanyUserID == _loginUser.CompanyUserID)
                        {
                            if (!IsSubOrderCancellable(cancelOrderCollection, subOrder))
                                isAllSubOrdersCancellable = false;
                        }
                        else
                            isAllSubOrdersCancellable = false;
                    }
                    else
                    {
                        if (!IsSubOrderCancellable(cancelOrderCollection, subOrder))
                            isAllSubOrdersCancellable = false;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isAllSubOrdersCancellable;
        }

        private static bool IsSubOrderCancellable(OrderBindingList cancelOrderCollection, OrderSingle subOrder)
        {
            try
            {
                if (ValidationManager.ISOrderCancellable(subOrder) || Prana.TradeManager.ValidationManager.IsOrderStatusPendingComplianceApproval(subOrder))
                {
                    OrderSingle cancelRequest = (OrderSingle)subOrder.Clone();
                    cancelRequest.MsgType = FIXConstants.MSGOrderCancelRequest;
                    ValidationManagerExtension.GetOrderDetails(cancelRequest);
                    cancelRequest.TransactionTime = DateTime.Now.ToUniversalTime();
                    cancelOrderCollection.Add(cancelRequest);
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return true;
        }

        internal void RolloverAllSubs()
        {
            try
            {
                DialogResult result = MessageBox.Show("Do you want to rollover In-Market sub orders to respective parent order?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    base.RolloverAllSubOrders();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public override void dgBlotter_AfterRowExpanded(object sender, RowEventArgs e)
        {
            // if no child rows, don't expand the row
            if (e.Row.ChildBands.HasChildRows == false)
            {
                e.Row.Expanded = false;
            }
        }

        protected override void menuShowDetails_Click(object sender, EventArgs e)
        {
            base.menuShowDetails_Click(sender, e);
        }

        /// <summary>
        /// Occurs when [go to allocation clicked].
        /// </summary>
        public event EventHandler<EventArgs<string, DateTime, DateTime>> SubOrderBloterGoToAllocationClicked = null;

        /// <summary>
        /// Handles the GoToAllocationClicked event of the WorkingSubBlotterGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{System.String, DateTime}"/> instance containing the event data.</param>
        protected void SubOrderBlotterGrid_GoToAllocationClicked(object sender, EventArgs<string, DateTime, DateTime> e)
        {
            try
            {
                if (SubOrderBloterGoToAllocationClicked != null)
                    SubOrderBloterGoToAllocationClicked(this, new EventArgs<string, DateTime, DateTime>(e.Value, e.Value2, e.Value3));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Occurs when [Edit/Replace Order click].
        /// </summary>
        public virtual event EventHandler SubOrderBloterReplaceOrEditOrderClicked = null;

        /// <summary>
        /// Handles the ReplaceOrEditOrderClicked event of the SubOrderBlotterGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SubOrderBlotterGrid_ReplaceOrEditOrderClicked(object sender, System.EventArgs e)
        {
            try
            {
                if (SubOrderBloterReplaceOrEditOrderClicked != null)
                    SubOrderBloterReplaceOrEditOrderClicked(sender, e);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Occurs when [sub order blotter launch add fills].
        /// </summary>
        public virtual event EventHandler SubOrderBlotterLaunchAddFills = null;

        /// <summary>
        /// Handles the LaunchAddFills event of the SubOrderBlotterGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SubOrderBlotterGrid_LaunchAddFills(object sender, System.EventArgs e)
        {
            try
            {
                if (SubOrderBlotterLaunchAddFills != null)
                    SubOrderBlotterLaunchAddFills(sender, e);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Occurs when [sub order blotter launch audit trail].
        /// </summary>
        public virtual event EventHandler SubOrderBlotterLaunchAuditTrail = null;

        /// <summary>
        /// Handles the LaunchAuditTrail event of the SubOrderBlotterGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SubOrderBlotterGrid_LaunchAuditTrail(object sender, System.EventArgs e)
        {
            try
            {
                if (SubOrderBlotterLaunchAuditTrail != null)
                    SubOrderBlotterLaunchAuditTrail(sender, e);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Occurs when [sub order blotter grid trade click].
        /// </summary>
        public virtual event EventHandler SubOrderBlotterGridTradeClick = null;

        public virtual event EventHandler<EventArgs<string>> SubOrderBloterUpdateStatusBar = null;
        public virtual event EventHandler SubOrderBlotterDisableRolloverButton = null;

        /// <summary>
        /// Handles the TradeClick event of the SubOrderBlotterGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SubOrderBlotterGrid_TradeClick(object sender, System.EventArgs e)
        {
            try
            {
                if (SubOrderBlotterGridTradeClick != null)
                    SubOrderBlotterGridTradeClick(sender, e);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Update Allocation Status Of Group
        /// </summary>
        /// <param name="orderAllocationState"></param>
        internal List<string> UpdateAllocationDetailsOrderSubOrders(List<AllocationDetails> allocationDetails)
        {
            List<string> updatedOrderParentClOrderIDs = new List<string>();
            try
            {
                //Update Sub Order Blotter grid Account, Master fund and Strategy Values
                updatedOrderParentClOrderIDs.AddRange(BlotterUICommonMethods.UpdateAllocationDetails(allocationDetails, SubOrderBlotterGrid.dgBlotter));

                //Update Order Blotter Grid
                updatedOrderParentClOrderIDs.AddRange(BlotterUICommonMethods.UpdateAllocationDetails(allocationDetails, dgBlotter, true));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return updatedOrderParentClOrderIDs;
        }

        /// <summary>
        /// Saves the sub order grid layout.
        /// </summary>
        internal void SaveLayoutSubOrderBlotterGrid(PranaUltraGrid grid, string blotterPreferencesPath, string key)
        {
            try
            {
                if (grid == null)
                    grid = SubOrderBlotterGrid.dgBlotter;
                SubOrderBlotterGrid.SaveLayoutBlotterGrid(grid, key, blotterPreferencesPath);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Removes the orders BTN clicked.
        /// </summary>
        internal void RemoveSelectedOrders(PranaUltraGrid orderGrid)
        {
            try
            {
                List<UltraGridRow> selectBlotterOrderRows = new List<UltraGridRow>();
                if (orderGrid != null)
                    selectBlotterOrderRows = orderGrid.Rows.GetFilteredInNonGroupByRows().Where(row => row.Cells[OrderFields.PROPERTY_CHKBOX].Text == true.ToString()).ToList();
                else
                    selectBlotterOrderRows = dgBlotter.Rows.GetFilteredInNonGroupByRows().Where(row => row.Cells[OrderFields.PROPERTY_CHKBOX].Text == true.ToString()).ToList();
                RemoveOrdersBlotterGrid(selectBlotterOrderRows, "Do you want to remove selected order(s)?");
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void RemoveOrdersBlotterGrid(List<UltraGridRow> selectBlotterOrderRows, string message)
        {
            try
            {
                if (selectBlotterOrderRows.Count() > 0)
                {
                    DialogResult result = MessageBox.Show(this, message, BlotterConstants.PROPERTY_NIRVANA_BLOTTER, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        List<string> listParentClOrderId = new List<string>();
                        List<string> listParentClOrderIdOfSubOrders = new List<string>();
                        List<int> uniqueTradingAccounts = new List<int>();
                        bool isAllSubOrdersRemovable = IsOrdersHiddenAble(selectBlotterOrderRows, ref listParentClOrderId, ref listParentClOrderIdOfSubOrders, ref uniqueTradingAccounts);

                        //If row cannot be hidden then removed from UI and set IsHidden field value is 1 for that Order in DB
                        if (listParentClOrderId.Count > 0)
                        {
                            BlotterCacheManager.GetInstance().HideOrderFromBlotter(listParentClOrderId, listParentClOrderIdOfSubOrders, isAllSubOrdersRemovable, _loginUser.CompanyUserID, uniqueTradingAccounts);

                            // Save audit trail data
                            SaveAuditTrailData();
                        }

                        //If some orders are in Market, then displaying Message box to ask for view the details
                        if (!isAllSubOrdersRemovable)
                            UpdateBlotterStatusBar("One or more of the order(s) selected could not be removed because of working quantity greater than 0.");
                        else
                            UpdateBlotterStatusBar("Order(s) has been removed.");
                    }
                }
                else
                {
                    UpdateBlotterStatusBar(BlotterConstants.ORDER_PLEASE_SELECT_MESSAGE);
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Determines whether [is orders hidden able] [the specified select blotter order rows].
        /// </summary>
        /// <param name="selectBlotterOrderRows">The select blotter order rows.</param>
        /// <returns></returns>
        private bool IsOrdersHiddenAble(List<UltraGridRow> selectBlotterOrderRows, ref List<string> listParentClOrderId, ref List<string> listParentClOrderIdOfSubOrders, ref List<int> uniqueTradingAccounts)
        {
            bool isAllSubOrdersRemovable = true;
            try
            {
                foreach (UltraGridRow row in selectBlotterOrderRows)
                {
                    OrderSingle currentOrder = (OrderSingle)row.ListObject;
                    if (currentOrder.LeavesQty > 0)
                    {
                        isAllSubOrdersRemovable = false;
                    }
                    else
                    {
                        listParentClOrderId.Add(currentOrder.ParentClOrderID);

                        //Add details in audit trail
                        AddAuditTrailCollection(currentOrder, TradeAuditActionType.ActionType.OrderRemoved);

                        if (!uniqueTradingAccounts.Contains(currentOrder.TradingAccountID))
                        {
                            uniqueTradingAccounts.Add(currentOrder.TradingAccountID);
                        }

                        if (currentOrder.OrderCollection != null)
                        {
                            foreach (OrderSingle or in currentOrder.OrderCollection)
                            {
                                listParentClOrderIdOfSubOrders.Add(or.ParentClOrderID);
                                if (!uniqueTradingAccounts.Contains(or.TradingAccountID))
                                {
                                    uniqueTradingAccounts.Add(or.TradingAccountID);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isAllSubOrdersRemovable;
        }
        /// <summary>
        /// After Cell Change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgBlotter_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {

                if (e.Cell.Band.Columns[OrderFields.PROPERTY_CHKBOX].ToString().Equals(OrderFields.PROPERTY_CHKBOX))
                    UpdateBlotterCountStatusBar(GetCheckedCount());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
        private void dgBlotter_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                if (dgBlotter.InvokeRequired)
                {
                    MethodInvoker mi = delegate { dgBlotter_AfterRowActivate(sender, e); };
                    this.BeginInvoke(mi);
                }
                else
                {
                    if (dgBlotter.ActiveRow != null)
                    {
                        OrderSingle order = (OrderSingle)dgBlotter.ActiveRow.ListObject;
                        SubOrderBlotterGrid.LoadActiveOrderData(order);
                    }
                    else
                        ClearSubOrdersGrid();
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        internal void ClearSubOrdersGrid()
        {
            try
            {
                SubOrderBlotterGrid.ClearSubOrdersGrid();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clears the manual sub orders grid.
        /// </summary>
        internal void ClearManualSubOrdersGrid(string clOrderID)
        {
            try
            {
                foreach (var row in dgBlotter.Rows)
                {
                    OrderSingle stageOrder = (OrderSingle)row.ListObject;
                    if (stageOrder.OrderCollection != null)
                    {
                        OrderSingle subOrder = stageOrder.OrderCollection.FirstOrDefault(x => x.ClOrderID.Equals(clOrderID));
                        if (subOrder != null)
                        {
                            stageOrder.OrderCollection.Remove(subOrder);
                            if (row.Activated)
                                SubOrderBlotterGrid.RemoveOrderFromGrid(subOrder);
                            BlotterOrderCollections.GetInstance().UpdateStatusFromChildCollection(stageOrder);

                            decimal qty = Convert.ToDecimal(stageOrder.Quantity);
                            decimal cumQty = Convert.ToDecimal(stageOrder.CumQty);
                            if (qty == cumQty)
                                stageOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_Filled;
                            else if (cumQty == 0.0m)
                                stageOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_New;
                            else
                                stageOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_PartiallyFilled;

                            stageOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(stageOrder.OrderStatusTagValue);
                            stageOrder.PropertyHasChanged();

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

        protected override void menuRemoveOrderClicked(object sender, EventArgs e)
        {
            try
            {
                TranferTradeRules transferTradeRules = CachedDataManager.GetInstance.GetTransferTradeRules();
                if (!transferTradeRules.IsAllowAllUserToCancelReplaceRemove)
                {
                    OrderSingle or = (OrderSingle)dgBlotter.ActiveRow.ListObject;
                    if (or.CompanyUserID.Equals(_loginUser.CompanyUserID))
                    {
                        if (dgBlotter.ActiveRow != null)
                            RemoveOrdersBlotterGrid(new List<UltraGridRow>() { dgBlotter.ActiveRow }, "Do you want to remove this order?");
                    }
                    else
                    {
                        MessageBox.Show(_loginUser.ShortName.ToUpper() + " does not have permissions to Remove this Trade", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    if (dgBlotter.ActiveRow != null)
                        RemoveOrdersBlotterGrid(new List<UltraGridRow>() { dgBlotter.ActiveRow }, "Do you want to remove this order?");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        internal void UpdateSubOrderBlotterGrid(List<OrderSingle> incomingOrders)
        {
            try
            {
                if (dgBlotter.ActiveRow != null && incomingOrders != null && incomingOrders.Count > 0)
                {
                    OrderSingle order = (OrderSingle)dgBlotter.ActiveRow.ListObject;
                    if (incomingOrders.Any(incomingOrder => incomingOrder.StagedOrderID == order.ParentClOrderID))
                    {
                        SubOrderBlotterGrid.LoadActiveOrderData(order);
                    }
                }
                //UpdateBlotterCountStatusBar(GetCheckedCount());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void SubOrderBlotterGrid_UpdateStatusBar(object sender, Global.EventArgs<string> e)
        {
            try
            {
                if (SubOrderBloterUpdateStatusBar != null)
                    SubOrderBloterUpdateStatusBar(this, new EventArgs<string>(e.Value));
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void SubOrderBlotterGrid_DisableRolloverButton(object sender, EventArgs e)
        {
            try
            {
                if (SubOrderBlotterDisableRolloverButton != null)
                    SubOrderBlotterDisableRolloverButton(sender, new EventArgs());
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #region FileDragDrop
        /// <summary>
        /// Bharat Kumar Jangir (18-June-2013)
        /// Drag Drop files on order blotter
        /// </summary>

        bool _fileTypeDropAllowed = false;
        string _dropFilePath = string.Empty;

        // Checking dropped file extension
        private void OrderBlotterGrid_DragOver(object sender, DragEventArgs e)
        {
            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = ((IDataObject)e.Data).GetData("FileDrop") as Array;
                if (data != null)
                {
                    if ((data.Length == 1) && (data.GetValue(0) is String))
                    {
                        _dropFilePath = ((string[])data)[0];
                        string ext = Path.GetExtension(_dropFilePath).ToUpper();
                        if ((ext == ".CSV" || ext == ".XLS"))
                        {
                            e.Effect = DragDropEffects.Copy;
                            _fileTypeDropAllowed = true;
                        }
                        else
                        {
                            e.Effect = DragDropEffects.None;
                            _fileTypeDropAllowed = false;
                        }
                    }
                }
            }
        }

        private void OrderBlotterGrid_DragDrop(object sender, DragEventArgs e)
        {
            // Filling blotter if dropped filetype is allowed
            if (_fileTypeDropAllowed)
            {
                FillUltraGrid();
            }
        }

        private void FillUltraGrid()
        {
            DataTable inputDataTable = null;
            DataTable outputDataTable = null;
            try
            {
                if (File.Exists(_dropFilePath))
                {
                    // Reading dropped file data
                    inputDataTable = GetDataTableFromDifferentFileFormats(_dropFilePath);

                    if (inputDataTable != null)
                    {
                        AddPrimaryKey(inputDataTable);
                        inputDataTable.TableName = "PositionMaster";

                        // XSLT transformation
                        outputDataTable = XSLTTransform(inputDataTable);

                        if (outputDataTable != null)
                        {
                            lock (_locker)
                            {
                                for (int counter = 0; counter < outputDataTable.Rows.Count; counter++)
                                {
                                    OrderSingle order = GetOrderFromDataRow(outputDataTable.Rows[counter]);
                                    if (order.Symbol != string.Empty)
                                    {
                                        if (_pendingOrders.ContainsKey(order.Symbol))
                                        {
                                            _pendingOrders[order.Symbol].Add(order);
                                        }
                                        else
                                        {
                                            List<OrderSingle> orders = new List<OrderSingle>();
                                            orders.Add(order);
                                            _pendingOrders.Add(order.Symbol, orders);
                                        }
                                    }
                                }
                            }

                            List<string> symbolList = new List<string>((IEnumerable<string>)_pendingOrders.Keys);
                            RequestSecMasterSymbols(symbolList, 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private DataTable GetDataTableFromDifferentFileFormats(string dropFilePath)
        {
            DataTable dTable = null;
            try
            {
                string fileFormat = dropFilePath.Substring(dropFilePath.LastIndexOf(".") + 1);

                switch (fileFormat.ToUpperInvariant())
                {
                    case "CSV":
                        dTable = GeneralUtilities.GetDataTableFromUploadedDataFileBulkRead(dropFilePath);
                        break;
                    case "XLS":
                        dTable = FileReaderFactory.Create(DataSourceFileFormat.Excel).GetDataTableFromUploadedDataFile(dropFilePath);
                        break;
                }
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("File in use! Please close the file and retry.");
            }
            return dTable;
        }

        private void AddPrimaryKey(DataTable dt)
        {
            try
            {
                if (!dt.Columns.Contains("RowID"))
                {
                    dt.Columns.Add("RowID");
                    int rowID = 0;

                    foreach (DataRow row in dt.Rows)
                    {
                        row["RowID"] = rowID;
                        rowID++;
                    }
                    dt.PrimaryKey = new DataColumn[] { dt.Columns["RowID"] };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private DataTable XSLTTransform(DataTable dTable)
        {
            try
            {
                string tempPath = Application.StartupPath;

                string inputXML = tempPath + "\\InputXML.xml";
                string outputXML = tempPath + "\\OutPutXML.xml";

                string path = Application.StartupPath;
                string xsltName = "StagingBlotterImport.xslt";
                string xsltPath = path + "\\" + xsltName;

                dTable.WriteXml(inputXML);

                System.Xml.Xsl.XslCompiledTransform xslt = new System.Xml.Xsl.XslCompiledTransform();
                xslt.Load(xsltPath);
                xslt.Transform(inputXML, outputXML);

                DataSet ds = new DataSet();
                ds.ReadXml(outputXML);
                if (ds.Tables.Count > 0 && ds.Tables[0] != null)
                {
                    dTable = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return dTable;
        }

        private OrderSingle GetOrderFromDataRow(DataRow dTableRow)
        {
            OrderSingle _orderRequest = new OrderSingle();
            try
            {
                if (dTableRow.Table.Columns.Contains("Symbol"))
                {
                    _orderRequest.Symbol = Convert.ToString(dTableRow["Symbol"]);
                }

                _orderRequest.MsgType = FIXConstants.MSGOrder;
                _orderRequest.DiscretionInst = String.Empty;
                _orderRequest.DiscretionOffset = 0.0;

                _orderRequest.PNP = "0";
                _orderRequest.PegDifference = 0.0;
                _orderRequest.SwapParameters = null;

                if (dTableRow.Table.Columns.Contains("Quantity"))
                {
                    _orderRequest.Quantity = Convert.ToDouble(dTableRow["Quantity"]);
                }
                else
                {
                    _orderRequest.Quantity = 0.0;
                }

                if (dTableRow.Table.Columns.Contains("AvgPrice"))
                {
                    _orderRequest.AvgPrice = Convert.ToDouble(dTableRow["AvgPrice"]);
                }
                else
                {
                    _orderRequest.AvgPrice = 0.0;
                }

                if (dTableRow.Table.Columns.Contains("Price"))
                {
                    _orderRequest.Price = Convert.ToDouble(dTableRow["Price"]);
                }
                else
                {
                    _orderRequest.Price = 0.0;
                }

                if (dTableRow.Table.Columns.Contains("StopPrice"))
                {
                    _orderRequest.StopPrice = Convert.ToDouble(dTableRow["StopPrice"]);
                }
                else
                {
                    _orderRequest.StopPrice = 0.0;
                }

                if (dTableRow.Table.Columns.Contains("Text"))
                {
                    _orderRequest.Text = Convert.ToString(dTableRow["Text"]);
                }
                else
                {
                    _orderRequest.Text = String.Empty;
                }

                if (dTableRow.Table.Columns.Contains("OrderSideTagValue"))
                {
                    _orderRequest.OrderSideTagValue = Convert.ToString(dTableRow["OrderSideTagValue"]);
                }

                if (dTableRow.Table.Columns.Contains("Venue"))
                {
                    _orderRequest.Venue = Convert.ToString(dTableRow["Venue"]);
                }

                if (dTableRow.Table.Columns.Contains("VenueID"))
                {
                    _orderRequest.VenueID = Convert.ToInt32(dTableRow["VenueID"]);
                }

                if (dTableRow.Table.Columns.Contains("CounterPartyName"))
                {
                    _orderRequest.CounterPartyName = Convert.ToString(dTableRow["CounterPartyName"]);
                }

                if (dTableRow.Table.Columns.Contains("CounterPartyID"))
                {
                    _orderRequest.CounterPartyID = Convert.ToInt32(dTableRow["CounterPartyID"]);
                }

                if (dTableRow.Table.Columns.Contains("HandlingInstruction"))
                {
                    _orderRequest.HandlingInstruction = Convert.ToString(dTableRow["HandlingInstruction"]);
                }
                else
                {
                    _orderRequest.HandlingInstruction = "3";
                }

                if (dTableRow.Table.Columns.Contains("OrderTypeTagValue"))
                {
                    _orderRequest.OrderTypeTagValue = Convert.ToString(dTableRow["OrderTypeTagValue"]);
                }
                else
                {
                    _orderRequest.OrderTypeTagValue = "1";
                    _orderRequest.OrderType = "MARKET";
                }

                if (dTableRow.Table.Columns.Contains("TIF"))
                {
                    _orderRequest.TIF = Convert.ToString(dTableRow["TIF"]);
                }
                else
                {
                    _orderRequest.TIF = "0";
                }


                if (dTableRow.Table.Columns.Contains("Level1ID"))
                {
                    _orderRequest.Level1ID = Convert.ToInt32(dTableRow["Level1ID"]);
                }
                else if (dTableRow.Table.Columns.Contains("Account"))
                {
                    _orderRequest.Account = dTableRow["Account"].ToString().Trim();
                    _orderRequest.Level1ID = CachedDataManager.GetInstance.GetAccountID(_orderRequest.Account.Trim());
                }
                else
                {
                    _orderRequest.Level1ID = int.MinValue;
                }

                if (dTableRow.Table.Columns.Contains("Level2ID"))
                {
                    _orderRequest.Level2ID = Convert.ToInt32(dTableRow["Level2ID"]);
                }
                else if (dTableRow.Table.Columns.Contains("Strategy"))
                {
                    _orderRequest.Strategy = dTableRow["Strategy"].ToString().Trim();
                    _orderRequest.Level2ID = CachedDataManager.GetInstance.GetStrategyID(_orderRequest.Strategy.Trim());
                }
                else
                {
                    _orderRequest.Level2ID = int.MinValue;
                }

                if (dTableRow.Table.Columns.Contains("TradingAccountID"))
                {
                    _orderRequest.TradingAccountID = Convert.ToInt32(dTableRow["TradingAccountID"]);
                }
                else
                {
                    _orderRequest.TradingAccountID = 11;
                }

                if (dTableRow.Table.Columns.Contains("ExecutionInstruction"))
                {
                    _orderRequest.ExecutionInstruction = Convert.ToString(dTableRow["ExecutionInstruction"]);
                }
                else
                {
                    _orderRequest.ExecutionInstruction = "G";
                }

                if (dTableRow.Table.Columns.Contains("AUECID"))
                {
                    _orderRequest.AUECID = Convert.ToInt32(dTableRow["AUECID"]);
                }
                else
                {
                    _orderRequest.AUECID = 1;
                }

                if (dTableRow.Table.Columns.Contains("AssetID"))
                {
                    _orderRequest.AssetID = Convert.ToInt32(dTableRow["AssetID"]);
                }
                else
                {
                    _orderRequest.AssetID = 1;
                }

                if (dTableRow.Table.Columns.Contains("UnderlyingID"))
                {
                    _orderRequest.UnderlyingID = Convert.ToInt32(dTableRow["UnderlyingID"]);
                }
                else
                {
                    _orderRequest.UnderlyingID = 1;
                }

                _orderRequest.CompanyUserID = _loginUser.CompanyUserID;

                _orderRequest.TransactionTime = DateTime.Now.ToUniversalTime();

                if (dTableRow.Table.Columns.Contains("CumQty"))
                {
                    _orderRequest.CumQty = Convert.ToDouble(dTableRow["CumQty"]);
                }
                else
                {
                    _orderRequest.CumQty = 0.0;
                }

                _orderRequest.AlgoStrategyID = string.Empty;
                _orderRequest.AlgoProperties = new OrderAlgoStartegyParameters();

                if (dTableRow.Table.Columns.Contains("CurrencyID"))
                {
                    _orderRequest.CurrencyID = Convert.ToInt32(dTableRow["CurrencyID"]);
                }
                else
                {
                    _orderRequest.CurrencyID = 1;
                }

                if (dTableRow.Table.Columns.Contains("ExchangeID"))
                {
                    _orderRequest.ExchangeID = Convert.ToInt32(dTableRow["ExchangeID"]);
                }
                else
                {
                    _orderRequest.ExchangeID = 1;
                }

                if (dTableRow.Table.Columns.Contains("CommissionRate"))
                {
                    _orderRequest.CommissionRate = Convert.ToDouble(dTableRow["CommissionRate"]);
                }
                else
                {
                    _orderRequest.CommissionRate = 0.0;
                }

                if (dTableRow.Table.Columns.Contains("CalcBasis"))
                {
                    _orderRequest.CalcBasis = (CalculationBasis)Convert.ToInt32(dTableRow["CalcBasis"]);
                }
                else
                {
                    _orderRequest.CalcBasis = CalculationBasis.Auto;
                }

                _orderRequest.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDStaged;
                _orderRequest.ClientTime = DateTime.Now.ToUniversalTime().ToLongTimeString();
                _orderRequest.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(FIXConstants.ORDSTATUS_PendingNew.ToString());

                return _orderRequest;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
            return _orderRequest;
        }

        private void RequestSecMasterSymbols(List<string> symbolList, int symbology)
        {
            if (_securityMaster != null && _securityMaster.IsConnected)
            {
                foreach (string symbol in symbolList)
                {
                    SecMasterRequestObj reqObj = new SecMasterRequestObj();
                    reqObj.AddData(symbol, (ApplicationConstants.SymbologyCodes)symbology);
                    reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                    reqObj.HashCode = this.GetHashCode();
                    _securityMaster.SendRequest(reqObj);
                }
            }
        }

        /// <summary>
        /// This dictionary cache the symbolwise orders until we receive the response from the 
        /// securitymaster.
        /// </summary>
        Dictionary<string, List<OrderSingle>> _pendingOrders = new Dictionary<string, List<OrderSingle>>();
        object _locker = new object();

        /// <summary>
        /// On receiving response, forward the validated symbols to order blotter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _securityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            lock (_locker)
            {
                SecMasterBaseObj secMasterObj = e.Value;
                if (_pendingOrders.ContainsKey(secMasterObj.TickerSymbol))
                {
                    List<OrderSingle> orders = _pendingOrders[secMasterObj.TickerSymbol];
                    foreach (OrderSingle order in orders)
                    {
                        order.AssetID = secMasterObj.AssetID;
                        order.UnderlyingID = secMasterObj.UnderLyingID;
                        order.ExchangeID = secMasterObj.ExchangeID;
                        order.CurrencyID = secMasterObj.CurrencyID;
                        order.AUECID = secMasterObj.AUECID;

                        switch (secMasterObj.AssetCategory)
                        {
                            case AssetCategory.EquityOption:
                            case AssetCategory.Option:
                            case AssetCategory.FutureOption:
                            case AssetCategory.Future:
                                if (order.OrderSideTagValue == "1")
                                {
                                    order.OrderSideTagValue = "A";
                                    order.OrderSide = "Buy to Open";
                                }
                                if (order.OrderSideTagValue == "2")
                                {
                                    order.OrderSideTagValue = "D";
                                    order.OrderSide = "Sell to Close";
                                }
                                if (order.OrderSideTagValue == "5")
                                {
                                    order.OrderSideTagValue = "C";
                                    order.OrderSide = "Sell to Open";
                                }
                                // Buy to close remains same.
                                break;
                        }
                        TradeManagerInstance.SendBlotterTrades(order, 0);
                    }
                    _pendingOrders.Remove(secMasterObj.TickerSymbol);
                }
            }
        }
        #endregion

        internal UltraGridRow GetActiveRowOfOrderBlotter()
        {
            if (dgBlotter != null)
                return dgBlotter.ActiveRow;
            else
                return null;
        }

        private void dgBlotter_AfterHeaderCheckStateChanged(object sender, AfterHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                UltraGridRow[] Rows = dgBlotter.Rows.GetFilteredOutNonGroupByRows();

                if (_rowsFilterOunt != null)
                {
                    for (int i = 0; i < Rows.Length; i++)
                    {
                        Rows[i].Cells["Checkbox"].Value = _rowsFilterOunt[i];
                    }
                }

                UpdateBlotterCountStatusBar(GetCheckedCount());

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
        private void dgBlotter_BeforeHeaderCheckStateChanged(object sender, BeforeHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                UltraGridRow[] RowsFilterOut = dgBlotter.Rows.GetFilteredOutNonGroupByRows();
                _rowsFilterOunt = new bool[RowsFilterOut.Length];
                for (int i = 0; i < RowsFilterOut.Length; i++)
                {
                    _rowsFilterOunt[i] = (bool)RowsFilterOut[i].Cells["Checkbox"].Value;
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

        #region Merge Orders
        /// <summary>
        /// Check Conditon for merge Order
        /// </summary>

        internal bool CheckAgainForMerge(PranaUltraGrid orderGrid, ref OrderSingle valuesOrder, ref OrderSingle firstOrder, ref double totalStageQuantity, ref OrderBindingList listOfOrders, ref double limitPrice, ref double stopLimitPrice, ref string message)
        {
            try
            {
                message = string.Empty;
                List<UltraGridRow> selectBlotterOrderRows = null;
                if (orderGrid != null)
                    selectBlotterOrderRows = orderGrid.Rows.GetFilteredInNonGroupByRows().Where(row => row.Cells[OrderFields.PROPERTY_CHKBOX].Text == true.ToString()).ToList();
                else
                    selectBlotterOrderRows = dgBlotter.Rows.GetFilteredInNonGroupByRows().Where(row => row.Cells[OrderFields.PROPERTY_CHKBOX].Text == true.ToString()).ToList();
                if (selectBlotterOrderRows == null || selectBlotterOrderRows.Count <= 1)
                {
                    message = BlotterConstants.MSG_SELECT_TWO_OR_MORE_ROWS;
                    UpdateBlotterStatusBar(message);
                    MessageBox.Show(this, BlotterConstants.MSG_SELECT_TWO_OR_MORE_ROWS, BlotterConstants.PROPERTY_NIRVANA_BLOTTER, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                else
                {
                    valuesOrder = (OrderSingle)((OrderSingle)selectBlotterOrderRows[0].ListObject).Clone();
                    DateTime latestDateTime = valuesOrder.AUECLocalDate;
                    firstOrder = (OrderSingle)selectBlotterOrderRows[0].ListObject;
                    bool isValid = true;
                    bool isLimitOrder = true;
                    bool isStopLimitOrder = true;
                    totalStageQuantity = 0;
                    limitPrice = 0;
                    stopLimitPrice = 0;
                    string algoStrategyID = string.Empty;
                    string algostrategyName = string.Empty;
                    Dictionary<string, string> tagValueDictionary = new Dictionary<string, string>();
                    listOfOrders = new OrderBindingList();
                    if (firstOrder.Venue.Equals(OrderFields.CAPTION_ALGOSTRATEGYNAME))
                    {
                        algoStrategyID = firstOrder.AlgoProperties.AlgoStartegyID;
                        tagValueDictionary = firstOrder.AlgoProperties.TagValueDictionary;
                        algostrategyName = firstOrder.AlgoStrategyName;
                    }
                    AllocationOperationPreference originalPref = _allocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, firstOrder.Level1ID);
                    foreach (UltraGridRow row in selectBlotterOrderRows)
                    {
                        OrderSingle order = (OrderSingle)row.ListObject;
                        listOfOrders.Add(order);
                        if (order.IsUseCustodianBroker && order.CounterPartyID == int.MinValue)
                        {
                            message = BlotterConstants.MSG_ORDER_HAVE_MULTIPLE_BROKER;
                            isValid = false;
                            return false;
                        }
                        AllocationOperationPreference operationPreference = _allocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, order.Level1ID);
                        string fixedPreference = _allocationProxy.InnerChannel.GetAllocationSchemeNameByID(order.Level1ID);
                        if (order.Symbol != valuesOrder.Symbol || order.OrderSide != valuesOrder.OrderSide || order.CounterPartyName != valuesOrder.CounterPartyName || order.TradingAccountID != valuesOrder.TradingAccountID)
                        {
                            message = BlotterConstants.MSG_CRITERIA_SYMBOL_SIDE_BROKER_NOT_MATCHING;
                            isValid = false;
                            return false;
                        }
                        else if (order.UnexecutedQuantity != order.Quantity || order.LeavesQty > 0)
                        {
                            message = BlotterConstants.MSG_ONE_OR_MORE_ORDERS_IN_MARKET;
                            isValid = false;
                            return false;
                        }
                        else if (valuesOrder.Level1ID != order.Level1ID && (!string.IsNullOrEmpty(fixedPreference)
                            || (operationPreference != null && operationPreference.DefaultRule.RuleType != MatchingRuleType.None)
                            || (originalPref != null && originalPref.DefaultRule.RuleType != MatchingRuleType.None)))
                        {
                            if (operationPreference != null || !string.IsNullOrEmpty(fixedPreference) && valuesOrder.Level1ID != order.Level1ID)
                            {
                                message = BlotterConstants.MSG_DIFFERENT_ALLOCATION_IN_ORDERS;
                                isValid = false;
                                return false;
                            }
                        }
                        if (!string.IsNullOrEmpty(valuesOrder.Venue))
                        {
                            if (order.Venue != valuesOrder.Venue && !string.IsNullOrEmpty(order.Venue))
                            {
                                message = BlotterConstants.MSG_CRITERIA_VENUE_NOT_MATCHING;
                                isValid = false;
                                return false;
                            }
                            else if (order.Venue.Equals(OrderFields.CAPTION_ALGOSTRATEGYNAME))
                            {
                                if (order.AlgoProperties.AlgoStartegyID != null && !order.AlgoProperties.AlgoStartegyID.Equals(algoStrategyID))
                                {
                                    message = BlotterConstants.MSG_CRITERIA_ALGO_STRATEGY_ID_NOT_MATCHING;
                                    isValid = false;
                                    return false;
                                }
                                else if (order.AlgoProperties.TagValueDictionary.Count != tagValueDictionary.Count || order.AlgoStrategyName != algostrategyName)
                                {
                                    message = BlotterConstants.MSG_CRITERIA_ALGO_PROPERTIES_NOT_MATCHING;
                                    isValid = false;
                                    return false;
                                }
                                else if (order.AlgoProperties.TagValueDictionary.Count != 0)
                                {
                                    foreach (var pair in order.AlgoProperties.TagValueDictionary)
                                    {
                                        if (!tagValueDictionary.ContainsKey(pair.Key) || tagValueDictionary[pair.Key] != pair.Value)
                                        {
                                            message = BlotterConstants.MSG_CRITERIA_ALGO_PROPERTIES_NOT_MATCHING;
                                            isValid = false;
                                            return false;
                                        }
                                    }
                                }
                            }
                            else if (order.Venue != valuesOrder.Venue && string.IsNullOrEmpty(order.Venue))
                            {
                                valuesOrder.Venue = order.Venue;
                            }
                            if (!isValid)
                            {
                                return false;
                            }
                        }
                        valuesOrder.TIF = valuesOrder.TIF.Equals(order.TIF) ? valuesOrder.TIF : string.Empty;
                        valuesOrder.ExpireTime = valuesOrder.ExpireTime.Equals(order.ExpireTime) ? valuesOrder.ExpireTime : string.Empty;
                        DateTime dateTimeValue = order.AUECLocalDate;

                        // Compare with the latest datetime value found then update the latest FxRate.
                        if (dateTimeValue > latestDateTime && order.FXRate != 0.0)
                        {
                            latestDateTime = dateTimeValue;
                            valuesOrder.FXRate = order.FXRate;
                            valuesOrder.FXConversionMethodOperator = order.FXConversionMethodOperator;
                        }
                        if (valuesOrder.SettlementCurrencyID != order.SettlementCurrencyID)
                        {
                            valuesOrder.SettlementCurrencyID = int.MinValue;
                            valuesOrder.CurrencyID = int.MinValue;
                            valuesOrder.CurrencyName = string.Empty;
                        }
                        valuesOrder.TradingAccountID = valuesOrder.TradingAccountID == order.TradingAccountID ? valuesOrder.TradingAccountID : int.MinValue;
                        valuesOrder.OrderType = valuesOrder.OrderType.Equals(order.OrderType) ? valuesOrder.OrderType : string.Empty;
                        valuesOrder.TradeAttribute1 = valuesOrder.TradeAttribute1.Equals(order.TradeAttribute1) ? valuesOrder.TradeAttribute1 : string.Empty;
                        valuesOrder.TradeAttribute2 = valuesOrder.TradeAttribute2.Equals(order.TradeAttribute2) ? valuesOrder.TradeAttribute2 : string.Empty;
                        valuesOrder.TradeAttribute3 = valuesOrder.TradeAttribute3.Equals(order.TradeAttribute3) ? valuesOrder.TradeAttribute3 : string.Empty;
                        valuesOrder.TradeAttribute4 = valuesOrder.TradeAttribute4.Equals(order.TradeAttribute4) ? valuesOrder.TradeAttribute4 : string.Empty;
                        valuesOrder.TradeAttribute5 = valuesOrder.TradeAttribute5.Equals(order.TradeAttribute5) ? valuesOrder.TradeAttribute5 : string.Empty;
                        valuesOrder.TradeAttribute6 = valuesOrder.TradeAttribute6.Equals(order.TradeAttribute6) ? valuesOrder.TradeAttribute6 : string.Empty;
                        valuesOrder.TransactionSourceTag = valuesOrder.TransactionSourceTag == order.TransactionSourceTag ? valuesOrder.TransactionSourceTag : -1;
                        valuesOrder.ExecutionInstruction = valuesOrder.ExecutionInstruction.Equals(order.ExecutionInstruction) ? valuesOrder.ExecutionInstruction : string.Empty;
                        #region Limit Order/Stop Limit Order
                        if (isLimitOrder && order.OrderType.Equals(BlotterConstants.CONST_LIMIT))
                        {
                            limitPrice += order.Price * order.Quantity;
                            isStopLimitOrder = false;
                            stopLimitPrice = 0;
                        }
                        else if (isStopLimitOrder && order.OrderType.Equals(BlotterConstants.CONST_STOP_LIMIT))
                        {
                            limitPrice += order.Price * order.Quantity;
                            stopLimitPrice += order.StopPrice * order.Quantity;
                            isLimitOrder = false;
                        }
                        else
                        {
                            isLimitOrder = false;
                            limitPrice = 0;
                            isStopLimitOrder = false;
                            stopLimitPrice = 0;
                        }
                        #endregion
                        totalStageQuantity += order.Quantity;
                    }
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
            return true;
        }

        /// <summary>
        ///  Merge stage order
        /// </summary>
        internal void MergeOrdersClick(PranaUltraGrid orderGrid)
        {
            try
            {
                string message = string.Empty;
                List<UltraGridRow> selectBlotterOrderRows = null;
                if (orderGrid != null)
                    selectBlotterOrderRows = orderGrid.Rows.GetFilteredInNonGroupByRows().Where(row => row.Cells[OrderFields.PROPERTY_CHKBOX].Text == true.ToString()).ToList();
                else
                    selectBlotterOrderRows = dgBlotter.Rows.GetFilteredInNonGroupByRows().Where(row => row.Cells[OrderFields.PROPERTY_CHKBOX].Text == true.ToString()).ToList();
                if (selectBlotterOrderRows == null || selectBlotterOrderRows.Count <= 1)
                {
                    message = BlotterConstants.MSG_SELECT_TWO_OR_MORE_ROWS;
                    UpdateBlotterStatusBar(message);
                    MessageBox.Show(this, BlotterConstants.MSG_SELECT_TWO_OR_MORE_ROWS, BlotterConstants.PROPERTY_NIRVANA_BLOTTER, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    OrderSingle valuesOrder = (OrderSingle)((OrderSingle)selectBlotterOrderRows[0].ListObject).Clone();
                    OrderSingle firstOrder = (OrderSingle)selectBlotterOrderRows[0].ListObject;
                    bool isValid = true;
                    double totalStageQuantity = 0;
                    double limitPrice = 0;
                    double stopLimitPrice = 0;
                    OrderBindingList listOfOrders = new OrderBindingList();
                    Dictionary<string, string> tagValueDictionary = new Dictionary<string, string>();
                    isValid = CheckAgainForMerge(orderGrid, ref valuesOrder, ref firstOrder, ref totalStageQuantity, ref listOfOrders, ref limitPrice, ref stopLimitPrice, ref message);
                    if (isValid)
                    {
                        CustomMessageBox customMessage = new CustomMessageBox(BlotterConstants.CAPTION_MERGE_ORDERS, BlotterConstants.MSG_IN_CASE_OF_MERGING_POSSIBLE, true, CustomThemeHelper.PRODUCT_COMPANY_NAME, FormStartPosition.CenterScreen, MessageBoxButtons.YesNo);
                        DialogResult dialog = customMessage.ShowDialog();
                        if (dialog == DialogResult.Yes)
                        {
                            List<UltraGridRow> selectBlotterOrderRows1 = null;
                            if (orderGrid != null)
                                selectBlotterOrderRows1 = orderGrid.Rows.GetFilteredInNonGroupByRows().Where(row => row.Cells[OrderFields.PROPERTY_CHKBOX].Text == true.ToString()).ToList();
                            else
                                selectBlotterOrderRows1 = dgBlotter.Rows.GetFilteredInNonGroupByRows().Where(row => row.Cells[OrderFields.PROPERTY_CHKBOX].Text == true.ToString()).ToList();

                            if (selectBlotterOrderRows.Count == selectBlotterOrderRows1.Count)
                            {
                                foreach (UltraGridRow row in selectBlotterOrderRows1)
                                {
                                    OrderSingle order = (OrderSingle)row.ListObject;
                                    if (!order.OrderStatus.Equals("New") || order.LeavesQty > 0)
                                    {
                                        bool mergePossible = CheckAgainForMerge(orderGrid, ref valuesOrder, ref firstOrder, ref totalStageQuantity, ref listOfOrders, ref limitPrice, ref stopLimitPrice, ref message);
                                        if (!mergePossible)
                                        {
                                            UpdateBlotterStatusBar(message);
                                            MessageBox.Show(this, message, BlotterConstants.PROPERTY_NIRVANA_BLOTTER, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                            return;
                                        }
                                        else break;
                                    }
                                }
                            }
                            else if (selectBlotterOrderRows1.Count > 1)
                            {
                                bool mergePossible = CheckAgainForMerge(orderGrid, ref valuesOrder, ref firstOrder, ref totalStageQuantity, ref listOfOrders, ref limitPrice, ref stopLimitPrice, ref message);
                                if (!mergePossible)
                                {
                                    UpdateBlotterStatusBar(message);
                                    MessageBox.Show(this, message, BlotterConstants.PROPERTY_NIRVANA_BLOTTER, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return;
                                }
                            }
                            else
                            {
                                message = BlotterConstants.MSG_SELECT_TWO_OR_MORE_ROWS;
                                UpdateBlotterStatusBar(message);
                                MessageBox.Show(this, BlotterConstants.MSG_SELECT_TWO_OR_MORE_ROWS, BlotterConstants.PROPERTY_NIRVANA_BLOTTER, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                            int operationPrefId = firstOrder.Level1ID;
                            string accountName = firstOrder.Account;
                            AccountWiseQuantityDetails accountWiseQuantityDetails = GetAccountWiseQuantityDetail(listOfOrders);
                            List<int> accountIds = new List<int>();
                            if (listOfOrders.Any(ord => ord.Level1ID != operationPrefId) && accountWiseQuantityDetails.AccountWiseQuantity.Count > 1)
                            {
                                if (accountWiseQuantityDetails.AccountWiseQuantity.ContainsKey(int.MinValue))
                                {
                                    accountIds.Add(operationPrefId);
                                    operationPrefId = int.MinValue;
                                    accountName = BlotterConstants.PROPERTY_DASH;
                                }
                                else
                                {
                                    Dictionary<int, decimal> accountIdPercentage = new Dictionary<int, decimal>();
                                    accountIdPercentage = GetAccountIDPercentage(accountWiseQuantityDetails.AccountWiseQuantity, totalStageQuantity);
                                    SerializableDictionary<int, AccountValue> targetPercs = new SerializableDictionary<int, AccountValue>();
                                    foreach (var item in accountIdPercentage)
                                    {
                                        AccountValue fv = new AccountValue(item.Key, item.Value);
                                        fv.StrategyValueList.Add(new StrategyValue(0, 100, 0));
                                        targetPercs.Add(item.Key, fv);
                                    }
                                    var allocPreference = CreateAllocationPreference(valuesOrder.Symbol, targetPercs);
                                    operationPrefId = allocPreference.OperationPreferenceId;
                                    accountIds = allocPreference.TargetPercentage.Keys.ToList();
                                    accountName = listOfOrders.Any(x => x.Account.Equals(BlotterConstants.PROPERTY_DASH)) ? BlotterConstants.PROPERTY_DASH : BlotterConstants.CAPTION_MULTIPLE;
                                }
                            }
                            else if (accountWiseQuantityDetails.AccountWiseQuantity.Count == 1)
                            {
                                accountIds.Add(operationPrefId);
                                operationPrefId = int.MinValue;
                            }
                            valuesOrder.Quantity = totalStageQuantity;
                            valuesOrder.StopPrice = stopLimitPrice;
                            valuesOrder.Price = limitPrice;
                            OrderSingle finalStageOrder = CreateMergeStageOrder(valuesOrder, firstOrder, operationPrefId, accountName);
                            finalStageOrder.IsUseCustodianBroker = !listOfOrders.Any(a => !a.IsUseCustodianBroker);
                            if (finalStageOrder.IsUseCustodianBroker)
                            {
                                finalStageOrder.AccountBrokerMapping = JsonHelper.SerializeObject(TradeManager.TradeManager.GetInstance().CreateAccountBrokerMapping(accountIds, finalStageOrder.CounterPartyID));
                            }
                            bool mergeOrderResult = TradeManager.TradeManager.GetInstance().SendBlotterTrades(finalStageOrder);
                            if (mergeOrderResult)
                            {
                                //Add details in audit trail for Merge Orders
                                listOfOrders.ToList().ForEach(order =>
                                {
                                    AddAuditTrailCollection(order, TradeAuditActionType.ActionType.MergeOrder, string.Empty, true);
                                });
                                BlotterCacheManager.GetInstance().HideOrderFromBlotter(accountWiseQuantityDetails.ParentClOrderIds, new List<string>(), true, _loginUser.CompanyUserID, accountWiseQuantityDetails.UniqueTradingAccounts);
                                message = selectBlotterOrderRows.Count + BlotterConstants.CONST_BLANK_SPACE + BlotterConstants.MSG_MERGE_SUCCESSFUL;
                            }
                            else
                                message = BlotterConstants.MSG_MERGE_ERROR_OCCURED;
                        }
                        else
                            message = BlotterConstants.MSG_MERGE_CANCELLED;
                        UpdateBlotterStatusBar(message);
                    }
                    else
                    {
                        UpdateBlotterStatusBar(message);
                        MessageBox.Show(this, message, BlotterConstants.PROPERTY_NIRVANA_BLOTTER, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
        /// Creates new merged stage order 
        /// </summary>
        /// <returns>OrderSingle or</returns>
        private OrderSingle CreateMergeStageOrder(OrderSingle valuesOrder, OrderSingle or, int originalAllocationPreferenceID, string accoutName)
        {
            OrderSingle stageOrder = (OrderSingle)or.Clone();
            try
            {
                stageOrder.Quantity = valuesOrder.Quantity;
                stageOrder.CompanyUserID = _loginUser.CompanyUserID;
                stageOrder.VenueID = CommonDataCache.CachedDataManager.GetInstance.GetVenueID(valuesOrder.Venue);
                stageOrder.Venue = valuesOrder.Venue;
                stageOrder.OrderType = valuesOrder.OrderType;
                stageOrder.OrderTypeTagValue = TagDatabaseManager.GetInstance.GetOrderTypeValue(valuesOrder.OrderType);
                stageOrder.MsgType = FIXConstants.MSGOrder;
                stageOrder.FXRate = valuesOrder.FXRate;
                stageOrder.TradeAttribute1 = valuesOrder.TradeAttribute1;
                stageOrder.TradeAttribute2 = valuesOrder.TradeAttribute2;
                stageOrder.TradeAttribute3 = valuesOrder.TradeAttribute3;
                stageOrder.TradeAttribute4 = valuesOrder.TradeAttribute4;
                stageOrder.TradeAttribute5 = valuesOrder.TradeAttribute5;
                stageOrder.TradeAttribute6 = valuesOrder.TradeAttribute6;
                stageOrder.InternalComments = string.Empty;
                stageOrder.CommissionAmt = 0.0;
                stageOrder.CommissionRate = 0.0;
                stageOrder.SoftCommissionAmt = 0.0;
                stageOrder.SoftCommissionRate = 0.0;
                stageOrder.ExecutionInstruction = valuesOrder.ExecutionInstruction;
                stageOrder.AvgPrice = 0;
                stageOrder.StopPrice = 0;
                stageOrder.LastPrice = 0;
                stageOrder.Price = 0;
                stageOrder.TIF = valuesOrder.TIF;
                stageOrder.ExpireTime = valuesOrder.ExpireTime;
                stageOrder.FXConversionMethodOperator = valuesOrder.FXConversionMethodOperator;
                stageOrder.SettlementCurrencyID = valuesOrder.SettlementCurrencyID;
                stageOrder.CurrencyID = valuesOrder.CurrencyID;
                stageOrder.CurrencyName = valuesOrder.CurrencyName;
                stageOrder.SoftCommissionCalcBasis = 0;
                stageOrder.Strategy = string.Empty;
                stageOrder.Text = string.Empty;
                stageOrder.InternalComments = string.Empty;
                stageOrder.ProcessDate = DateTimeConstants.MinValue;
                if ((valuesOrder.OrderType.Equals(BlotterConstants.CONST_LIMIT) || valuesOrder.OrderType.Equals(BlotterConstants.CONST_STOP_LIMIT)) && valuesOrder.Price > 0)
                    stageOrder.Price = valuesOrder.Price / valuesOrder.Quantity;
                stageOrder.ClientTime = DateTime.Now.ToUniversalTime().ToLongTimeString();
                stageOrder.TransactionTime = DateTime.Now.ToUniversalTime();
                stageOrder.OriginalAllocationPreferenceID = originalAllocationPreferenceID;
                if ((originalAllocationPreferenceID == int.MinValue && accoutName.Equals(BlotterConstants.PROPERTY_DASH)) || originalAllocationPreferenceID != int.MinValue)
                    stageOrder.Level1ID = originalAllocationPreferenceID;
                stageOrder.Account = accoutName;
                stageOrder.TradingAccountID = valuesOrder.TradingAccountID;
                stageOrder.TradingAccountName = valuesOrder.TradingAccountID != int.MinValue ? CommonDataCache.CachedDataManager.GetInstance.GetTradingAccountText(valuesOrder.TradingAccountID) : string.Empty;
                stageOrder.Description = BlotterConstants.CAPTION_MERGE_ORDERS;
                stageOrder.TransactionSourceTag = valuesOrder.TransactionSourceTag == (int)TransactionSource.PST ? -1 : valuesOrder.TransactionSourceTag;
                stageOrder.ActualCompanyUserID = _loginUser.CompanyUserID;
                string userName = CachedDataManager.GetInstance.GetUserText(_loginUser.CompanyUserID);
                stageOrder.ActualCompanyUserName = userName;
                stageOrder.BorrowerBroker = string.Empty;
                stageOrder.BorrowerID = string.Empty;
                stageOrder.ShortRebate = 0;
                stageOrder.CompanyUserName = userName;
                stageOrder.IsManualOrder = false;
                stageOrder.IsStageRequired = false;

                if (stageOrder.TransactionSourceTag == (int)TransactionSource.Rebalancer)
                {
                    stageOrder.TransactionSource = TransactionSource.None;
                    stageOrder.TransactionSourceTag = -1;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return stageOrder;
        }

        /// <summary>
        /// Get preference wise account quantity , ParentClOrderId , UniqueTradingAccount
        /// </summary>
        /// <param name="listcheckedRows"></param>
        /// <returns></returns>
        private AccountWiseQuantityDetails GetAccountWiseQuantityDetail(OrderBindingList listOfOrders)
        {
            AccountWiseQuantityDetails details = new AccountWiseQuantityDetails();
            details.AccountWiseQuantity = new Dictionary<int, double>();
            details.ParentClOrderIds = new List<string>();
            details.UniqueTradingAccounts = new List<int>();
            try
            {
                foreach (OrderSingle order in listOfOrders)
                {
                    details.ParentClOrderIds.Add(order.ParentClOrderID);
                    if (!details.UniqueTradingAccounts.Contains(order.TradingAccountID))
                        details.UniqueTradingAccounts.Add(order.TradingAccountID);
                    if (order.Account != "Multiple")
                    {
                        if (details.AccountWiseQuantity.ContainsKey(order.Level1ID))
                            details.AccountWiseQuantity[order.Level1ID] += order.Quantity;
                        else
                            details.AccountWiseQuantity[order.Level1ID] = order.Quantity;
                    }
                    else
                    {
                        AllocationOperationPreference operationPreference = _allocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, order.Level1ID);
                        if (operationPreference != null)
                        {
                            Dictionary<int, decimal> accountIdWithAlocatedPercent = operationPreference.GetSelectedAccountsWithPostionPct();
                            foreach (var item in accountIdWithAlocatedPercent)
                            {
                                if (details.AccountWiseQuantity.ContainsKey(item.Key))
                                {
                                    double quan = (order.Quantity * (double)(item.Value / 100));
                                    details.AccountWiseQuantity[item.Key] += (order.Quantity * (double)(item.Value / 100));
                                }
                                else
                                    details.AccountWiseQuantity[item.Key] = (order.Quantity * (double)(item.Value / 100));
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
            return details;
        }

        /// <summary>
        /// Get preference wise account Percentage
        /// </summary>
        /// <returns></returns>
        private static Dictionary<int, decimal> GetAccountIDPercentage(Dictionary<int, double> accountQuantity, double totalQuantity)
        {
            Dictionary<int, decimal> accountPercentage = new Dictionary<int, decimal>();
            try
            {
                foreach (var account in accountQuantity)
                {
                    accountPercentage[account.Key] = (((decimal)account.Value * 100) / (decimal)totalQuantity);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return accountPercentage;
        }

        /// <summary>
        /// CreateAllocationPreference
        /// </summary>
        /// <param name="Symbol"></param>
        /// <param name="targetPercs"></param>
        /// <returns></returns>
        private AllocationOperationPreference CreateAllocationPreference(String Symbol, SerializableDictionary<int, AccountValue> targetPercs)
        {
            AllocationOperationPreference allocationOperationPreference = null;
            try
            {
                string prefName = "*Custom#_" + Symbol + "_" + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + "_" + DateTime.Now.Ticks;
                PreferenceUpdateResult result = _allocationProxy.InnerChannel.AddPreference(prefName, CachedDataManager.GetInstance.GetCompanyID(), AllocationPreferencesType.CalculatedAllocationPreference, false);
                allocationOperationPreference = result.Preference;
                if (allocationOperationPreference != null)
                {
                    allocationOperationPreference.TryUpdateTargetPercentage(targetPercs);
                    //Set Default rule for Allocation
                    AllocationRule defaulfRule = new AllocationRule();
                    defaulfRule.BaseType = AllocationBaseType.CumQuantity;
                    defaulfRule.RuleType = MatchingRuleType.None;
                    defaulfRule.PreferenceAccountId = -1;
                    defaulfRule.MatchClosingTransaction = MatchClosingTransactionType.None;
                    allocationOperationPreference.TryUpdateDefaultRule(defaulfRule);
                    _allocationProxy.InnerChannel.UpdatePreference(allocationOperationPreference);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationOperationPreference;
        }

        #endregion

        /// <summary>
        /// Unwires the events.
        /// </summary>
        public override void UnwireEvents()
        {
            try
            {
                if (_securityMaster != null)
                    _securityMaster.SecMstrDataResponse -= new EventHandler<EventArgs<SecMasterBaseObj>>(_securityMaster_SecMstrDataResponse);
                base.UnwireEvents();
                if (_allocationProxy != null)
                    _allocationProxy.Dispose();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
    }
}