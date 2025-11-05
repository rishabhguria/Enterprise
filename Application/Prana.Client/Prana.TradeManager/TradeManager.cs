using Infragistics.Win;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.Compliance.Constants;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.FIX;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.TradeManager.Extension;
using Prana.TradeManager.Forms;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using System.Windows.Forms;

namespace Prana.TradeManager
{
    public delegate void UpdateDictionaryByDbOrdershandler(object sender, EventArgs<OrderBindingList> e);
    public delegate void DisplayCustomPopUphandler(object sender, EventArgs<string, string> e);

    public class TradeManager : ITradeManager, IDisposable, IPublishing
    {
        #region Private Variables
        private Dictionary<int, CounterPartyDetails> _counterPartiesConnectionStatus = new Dictionary<int, CounterPartyDetails>();
        //private System.Threading.Timer _tmrSendToBlotter = null;
        // private List<OrderSingle> _listOrders = null;
        private static ProxyBase<IPranaPositionServices> _pranaPositionServices = null;
        /// <summary>
        /// The sec master services
        /// </summary>
        private ISecurityMasterServices _secMasterServices = null;

        /// <summary>
        /// Gets or sets the sec master services.
        /// </summary>
        /// <value>
        /// The sec master services.
        /// </value>
        public ISecurityMasterServices SecMasterServices
        {
            set { _secMasterServices = value; }
            get { return _secMasterServices; }
        }

        private static TradeManager _tradeManager = null;
        const string FORM_NAME = "TradeManager :";
        private static Prana.BusinessObjects.PriceSymbolValidation priceSymbolSettings = new Prana.BusinessObjects.PriceSymbolValidation();
        private static int _userID;

        /// <summary>
        /// TradeManagerExtensionInstance
        /// </summary>
        internal static TradeManagerExtension _tradeManagerExtensionInstance;
        internal static TradeManagerExtension TradeManagerExtensionInstance
        {
            get
            {
                if (_tradeManagerExtensionInstance == null)
                {
                    _tradeManagerExtensionInstance = TradeManagerExtension.GetInstance();
                }
                return _tradeManagerExtensionInstance;
            }
        }
        //private ITradeQueueProcessor _tradeReceivedQueue;
        //const int CONST_BLOTTERUPDATEINTERVAL = 200;
        //public System.Timers.Timer _initiativeTimer = null;

        #endregion

        public event UpdateBlotterCollectionOnBlotterThreadHandler UpdateBlotterCollectionOnBlotterThreadMarshal;
        public event EventHandler ClearWorkingSubOrderCollection;
        public event EventHandler RefreshBlotterEvent;
        public event EventHandler BlotterRefreshCompleteEvent;
        public event UpdateDictionaryByDbOrdershandler UpdateDictionaryByDbOrders;
        public event DisplayCustomPopUphandler DisplayCustomPopUp;
        public delegate void AlgoReplaceOrderEditHandler(object sender, EventArgs<OrderSingle> e);
        public event AlgoReplaceOrderEditHandler AlgoReplaceEditHandler;
        public event AlgoValidTradeHandler AlgoValidTradeToBlotterUI;

        //Based on BlotterUpdateTimeInterval, we are updating data on Blotter UI.
        private int _blotterUpdateTimeInterval = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("BlotterUpdateTimeInterval"));
        private Dictionary<string, OrderSingle> _dictMessageSendToBeBlotter = new Dictionary<string, OrderSingle>();
        private System.Timers.Timer _blotterUpdateTimer = new System.Timers.Timer();
        private object _lockerDictMessageToBeSendToBlotter = new object();
        private const string CONST_ImportStarted = "ImportStarted";
        /// <summary>
        /// proxy for IPublishing
        /// </summary>
        private DuplexProxyBase<ISubscription> _proxy;
        /// <summary>
        /// The column account
        /// </summary>
        private const string COLUMN_ACCOUNT = "Account";
        /// <summary>
        /// The column broker
        /// </summary>
        private const string COLUMN_BROKER = "Broker";
        /// <summary>
        /// The column qty
        /// </summary>
        private const string COLUMN_QTY = "Qty";

        /// <summary>
        /// sets true when import has started and blotter refresh is required.
        /// </summary>
        private bool _isBlotterRefreshRequired = true;
        public bool IsBlotterRefreshRequired
        {
            get { return _isBlotterRefreshRequired; }
            set { _isBlotterRefreshRequired = value; }
        }

        private TradeManager()
        {
            try
            {
                AlgoReplaceManager.GetInstance().AlgoReplaceOrderEditToTradingTkt += new AlgoReplaceManager.AlgoReplaceOrderEdit(TradeManager_AlgoReplaceOrderEdit);
                AlgoReplaceManager.GetInstance().AlgoValidTradeToUIThread += new AlgoValidTradeHandler(TradeManager_AlgoValidTradeToUIThread);

                System.Threading.Tasks.Task.Factory.StartNew(() => ConsumeBufferMessageAsync(TradeManagerExtensionInstance.dataBuffer)).ConfigureAwait(false);
                TradeManagerExtensionInstance.ApplicationPath = Application.StartupPath;
                TradeManagerExtensionInstance.LoadUserEnteredTradesCache();
                _blotterUpdateTimer.Interval = _blotterUpdateTimeInterval;
                _blotterUpdateTimer.Elapsed += _blotterUpdateTimer_Elapsed;
                _blotterUpdateTimer.Start();
                BlotterOrderCollections.GetInstance().SendBlotterTradesEventHandler += SendBlotterTradesEvent;
                AlgoReplaceManager.GetInstance().ShowAlgoPromptWindowEventHandler += ShowAlgoPromptWindowEvent;
                BlotterOrderCollections.GetInstance().ShowCustomMessageBoxEventHandler += ShowCustomMessageBoxEvent;
                TradeManagerExtensionInstance.CheckForDuplicateTradeEvent += CheckForDuplicateTrade;
                TradeManagerExtensionInstance.UpdateTradeAttributeListEvent += updateTradeAttributesList;
                TradeManagerExtensionInstance.FixBrokerDownEventHandler += ShowFixBrokerDownMessageEvent;
                TradeManagerExtensionInstance.ShowMessageBoxOnEnterpise += ShowMessageBoxEvent;
                MakeProxy();
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

        private void ShowMessageBoxEvent(object sender, EventArgs<string> e)
        {
            try
            {
                MessageBox.Show(e.Value);
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
        /// Event for ShowCustomMessageBoxEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowCustomMessageBoxEvent(object sender, EventArgs<OrderSingle> e)
        {
            try
            {
                OrderSingle incomingOrder = e.Value as OrderSingle;
                string orderside = TagDatabaseManager.GetInstance.GetOrderSideText(incomingOrder.OrderSideTagValue);
                string ordertype = TagDatabaseManager.GetInstance.GetOrderTypeText(incomingOrder.OrderTypeTagValue);
                string message = string.Empty;
                if (ordertype == "Limit")
                    message = orderside + ", " + incomingOrder.Quantity + " " + incomingOrder.Symbol + " at " + ordertype + " Price: " + incomingOrder.Price + " to " + incomingOrder.CounterPartyName + Environment.NewLine + "Reason : '" + incomingOrder.Text + "'" + Environment.NewLine + "Please resend the order with correct information or contact" + Environment.NewLine + "your support representative.";
                else
                    message = orderside + ", " + incomingOrder.Quantity + " " + incomingOrder.Symbol + " at " + ordertype + " Price to " + incomingOrder.CounterPartyName + Environment.NewLine + "Reason : '" + incomingOrder.Text + "'" + Environment.NewLine + "Please resend the order with correct information or contact" + Environment.NewLine + "your support representative.";

                string title = "Trader : " + CachedDataManager.GetInstance.LoggedInUser.FirstName;
                CustomMessageBox messagebox = new CustomMessageBox(title, message, true, CustomThemeHelper.REJECT_POPUP);
                messagebox.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Event for ShowAlgoPromptWindowEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAlgoPromptWindowEvent(object sender, EventArgs<string, string, OrderSingle> e)
        {
            try
            {
                AlgoPromptWindow promptWinNew = new AlgoPromptWindow(e.Value, e.Value2, false, true, e.Value3);
                promptWinNew.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Event for SendBlotterTradesEventHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendBlotterTradesEvent(object sender, EventArgs<OrderSingle> e)
        {
            try
            {
                SendBlotterTrades(e.Value);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void _blotterUpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                lock (_lockerDictMessageToBeSendToBlotter)
                {
                    if (_dictMessageSendToBeBlotter.Count > 0 && TradingTktPrefs.IsTTPrefInitialized)
                    {
                        UpdateBlotterCollectionOnBlotterThread_PullMachanism(_dictMessageSendToBeBlotter.Values.ToList());
                        _dictMessageSendToBeBlotter.Clear();
                    }

                    _blotterUpdateTimer.Stop();
                    _blotterUpdateTimer.Start();
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

        public static void CreatePositionServicesProxy()
        {
            try
            {
                if (_pranaPositionServices == null)
                {
                    _pranaPositionServices = new ProxyBase<IPranaPositionServices>("TradePositionServiceEndpointAddress");
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
        /// Set CashAccounts to be locked for current user
        /// Returns bool (true if all account are locked else false)
        /// </summary>
        /// <param name="accountsToBeLocked"></param>
        /// <returns></returns>
        public static bool SetAccountsLockStatus(List<int> accountsToBeLocked)
        {
            bool isSucessuful = false;
            try
            {
                int userID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                if (_pranaPositionServices == null)
                {
                    CreatePositionServicesProxy();
                }
                isSucessuful = _pranaPositionServices.InnerChannel.SetAccountsLockStatus(userID, accountsToBeLocked);
                if (isSucessuful)
                {
                    CachedDataManager.GetInstance.UpdateAccountLockData(accountsToBeLocked);
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
            return isSucessuful;
        }


        void UpdateBlotterCollectionOnBlotterThread_PullMachanism(List<OrderSingle> Orders)
        {
            try
            {

                if (UpdateBlotterCollectionOnBlotterThreadMarshal != null)
                {
                    UpdateBlotterCollectionOnBlotterThreadMarshal(this, new EventArgs<List<OrderSingle>>(Orders));
                }
                else
                {
                    BlotterOrderCollections.GetInstance().UpdateBlotterCollection(Orders);
                }
                foreach (OrderSingle incomingOrder in Orders)
                {
                    if ((bool)TradingTktPrefs.TradingTicketRulesPrefs.IsPendingNewTradeAlert &&
                        !incomingOrder.IsManualOrder && incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew &&
                            CachedDataManager.GetInstance.LoggedInUser.CompanyUserID == incomingOrder.CompanyUserID)
                    {
                        CheckPendingNewStatus(incomingOrder);
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

        /// <summary>
        /// Checks the pending new status.
        /// </summary>
        /// <param name="incomingOrder">The incoming order.</param>
        private async void CheckPendingNewStatus(OrderSingle incomingOrder)
        {
            try
            {
                await System.Threading.Tasks.Task.Delay(1000 * (int)TradingTktPrefs.TradingTicketRulesPrefs.PendingNewOrderAlertTime);
                string message = BlotterOrderCollections.GetInstance().ShowPendingPopUpMessage(incomingOrder);
                if (!string.IsNullOrEmpty(message) && DisplayCustomPopUp != null)
                {
                    string title = "Trader : " + CachedDataManager.GetInstance.GetUserText(incomingOrder.CompanyUserID);
                    DisplayCustomPopUp(this, new EventArgs<string, string>(title, message));
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

        void TradeManager_AlgoValidTradeToUIThread(object sender, EventArgs<OrderSingle> e)
        {
            try
            {
                if (AlgoValidTradeToBlotterUI != null)
                {
                    AlgoValidTradeToBlotterUI(this, e);
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

        async System.Threading.Tasks.Task<OrderSingle> ConsumeBufferMessageAsync(IReceivableSourceBlock<OrderSingle> source)
        {
            try
            {
                // Read from the source buffer until the source buffer has no 
                // available output data.
                while (await source.OutputAvailableAsync())
                {
                    OrderSingle message;
                    while (source.TryReceive(out message))
                    {
                        if (message != null)
                        {
                            lock (_lockerDictMessageToBeSendToBlotter)
                            {
                                if (message.Text.Equals(PreTradeConstants.MSG_PENDING_COMPLIANCE_APPROVAL_CHANGE_ORDER_STATUS) && message.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_PendingNew))
                                {
                                    message.Text = string.Empty;
                                    message.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingReplace;
                                }

                                if (_dictMessageSendToBeBlotter.ContainsKey(message.ParentClOrderID))
                                {
                                    if (_dictMessageSendToBeBlotter[message.ParentClOrderID].OrderSeqNumber <= message.OrderSeqNumber && _dictMessageSendToBeBlotter[message.ParentClOrderID].OrderSeqNumber != long.MinValue)
                                    {
                                        _dictMessageSendToBeBlotter[message.ParentClOrderID] = message;
                                    }
                                    else
                                    {
                                        if (message.Text.Equals(PreTradeConstants.MsgTradePending) || message.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_PendingReplace) || message.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_PendingCancel))
                                            _dictMessageSendToBeBlotter[message.ParentClOrderID] = message;
                                    }
                                }
                                else
                                {
                                    _dictMessageSendToBeBlotter.Add(message.ParentClOrderID, message);
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
            return null;
        }

        public void ResetTimers(bool shouldStart)
        {
            try
            {
                return;
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

        void TradeManager_AlgoReplaceOrderEdit(object sender, EventArgs<OrderSingle> e)
        {
            try
            {
                if (AlgoReplaceEditHandler != null)
                {
                    AlgoReplaceEditHandler(this, e);
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

        public static TradeManager GetInstance()
        {
            if (_tradeManager == null)
            {
                _tradeManager = new TradeManager();
            }
            return _tradeManager;
        }

        public delegate void SendToDeskHandler(object sender, EventArgs<TradingInstruction> e);

        public int UserID
        {
            get
            {
                return _userID;
            }
            set
            {
                _userID = value;
                GetDataAsync();
            }
        }

        private int _companyID;
        public int CompanyID
        {
            get
            {
                return _companyID;
            }
            set
            {
                _companyID = value;
            }
        }

        public Prana.BusinessObjects.PriceSymbolValidation PriceSymbolSetting
        {
            get { return priceSymbolSettings; }
            set { priceSymbolSettings = value; }
        }

        #region Price Symbol Validaiton Properties
        public static void GetTradePrefs()
        {
            priceSymbolSettings = TicketManager.GetPriceSymbolSettings(_userID);
        }

        public static void GetNewPriceSymbolSettings(Prana.BusinessObjects.PriceSymbolValidation newPriceSymbolSettings)
        {
            try
            {
                priceSymbolSettings.CompanyUserID = newPriceSymbolSettings.CompanyUserID;
                priceSymbolSettings.RiskCtrlCheck = newPriceSymbolSettings.RiskCtrlCheck;
                priceSymbolSettings.RiskValue = newPriceSymbolSettings.RiskValue;
                priceSymbolSettings.ValidateSymbolCheck = newPriceSymbolSettings.ValidateSymbolCheck;
                priceSymbolSettings.LimitPriceCheck = newPriceSymbolSettings.LimitPriceCheck;
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

        public static void GetNewValidationPopUpSettings(Prana.BusinessObjects.ConfirmationPopUp newConfirmationPopupSettings)
        {
            try
            {
                ValidationManager._confirmationPopUpPref.ISCXL = newConfirmationPopupSettings.ISCXL;
                ValidationManager._confirmationPopUpPref.ISCXLReplace = newConfirmationPopupSettings.ISCXLReplace;
                ValidationManager._confirmationPopUpPref.ISNewOrder = newConfirmationPopupSettings.ISNewOrder;
                ValidationManager._confirmationPopUpPref.IsManualOrder = newConfirmationPopupSettings.IsManualOrder;
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
        #endregion

        #region Trade conditions Validations

        public bool ValidateAUECandCV(Prana.BusinessObjects.Order order)
        {
            if (CachedDataManager.GetInstance.CheckTradePermissionByCVandAUECID(order.AUECID, order.CounterPartyID, order.VenueID))
            {
                if (TradeManagerExtensionInstance.GetCounterPartyConnectionSatus(order.CounterPartyID) == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    return true;
                }
                else
                {
                    throw new Exception("CounterParty not Connected.");
                }
            }
            else
            {
                throw new Exception("Check if you have permission for the AUEC. \nAlso Check that the CounterParty Venue you want to trade has permissions to trade the same AUEC.");
            }

        }
        #endregion

        #region ITradeManager Members

        public void SendTradeToServer(Prana.BusinessObjects.OrderSingle order)
        {
        }

        public bool IsWithinLimits(Prana.BusinessObjects.OrderSingle orRequest, double marketPrice)
        {
            bool IsWithinLimit = true;
            if (priceSymbolSettings.RiskCtrlCheck)
            {
                double riskValueUpper = marketPrice + (priceSymbolSettings.RiskValue / 100.00) * marketPrice; // Upper Limit for Price
                double riskValueLower = marketPrice - (priceSymbolSettings.RiskValue / 100.00) * marketPrice; // Upper Limit for Price

                // check for upper limit breach
                if (orRequest.Price > riskValueUpper)
                {
                    string str = "Price greater than " + (priceSymbolSettings.RiskValue) + " % limit set! Proceed?";
                    if ((MessageBox.Show(str, "Warning!", MessageBoxButtons.YesNo)) == DialogResult.No)
                    {
                        IsWithinLimit = false;
                        return IsWithinLimit;
                    }
                }

                // check for lower limit breach
                if (orRequest.Price < riskValueLower)
                {
                    string str = "Price lower than " + (priceSymbolSettings.RiskValue) + " % limit set! Proceed?";
                    if ((MessageBox.Show(str, "Warning!", MessageBoxButtons.YesNo)) == DialogResult.No)
                    {
                        IsWithinLimit = false;
                        return IsWithinLimit;
                    }
                }
            }
            return IsWithinLimit;
        }
        #endregion

        #region Blotter Orders Collections
        public OrderBindingList WorkingSubBlotterCollection
        {
            get { return BlotterOrderCollections.GetInstance().WorkingSubsTabCollection; }
        }

        public OrderBindingList OrderBlotterCollection
        {
            get { return BlotterOrderCollections.GetInstance().OrdersTabCollection; }
        }
        public Dictionary<string, OrderSingle> WorkingSubOrderDictionary
        {
            get { return BlotterOrderCollections.GetInstance().DictParentClOrderIDCollection; }
        }
        #endregion

        #region Send Trade Methods
        public OrderBindingList SendGroupCancelOrRolloverRequest(OrderBindingList orderCollection)
        {
            OrderBindingList validatedCollection = new OrderBindingList();
            try
            {
                foreach (OrderSingle orderRequest in orderCollection)
                {
                    if (ValidationManager.ValidateOrder(orderRequest, _userID))
                    {
                        OrderSingle orderSingle = new OrderSingle();
                        orderSingle = (OrderSingle)orderRequest.Clone();
                        orderSingle.ModifiedUserId = UserID;
                        validatedCollection.Add(orderSingle);
                    }
                }
                TradeManagerExtensionInstance.SendGroupCancelOrRolloverRequest(validatedCollection);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return validatedCollection;
        }

        /// <summary>
        /// Event to show fix connection down message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowFixBrokerDownMessageEvent(object sender, EventArgs<OrderSingle> e)
        {
            try
            {
                OrderSingle order = e.Value as OrderSingle;
                MessageBox.Show("Fix Connection for Broker " + order.CounterPartyName + " is down." + Environment.NewLine + "Please resend your order.", "Fix Disconnection Notice", MessageBoxButtons.OK);
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
        /// for trades other than that of trading ticket
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool SendBlotterTrades(Prana.BusinessObjects.OrderSingle order)
        {
            bool tradeSuccessful = false;
            try
            {
                if (ValidationManager.ValidateOrder(order, _userID))
                {
                    if (TradeManagerExtensionInstance.SendTradeAfterCheckCPConnection(order))
                    {
                        tradeSuccessful = true;
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
            return tradeSuccessful;
        }

        public bool IsTradeMergedRemovedEdited = false;
        /// <summary>
        /// for trades from trading tkt where validations are required
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="marketPrice">The market price.</param>
        /// <param name="isComingFromTT">if set to <c>true</c> [is coming from tt].</param>
        /// <returns></returns>
        public bool SendBlotterTrades(Prana.BusinessObjects.OrderSingle order, double marketPrice, bool isComingFromTT = false, bool isOrderValidated = false)
        {
            bool tradeSuccessful = false;
            try
            {
                if (TradeManagerExtensionInstance.CheckTradeConditions(order))
                {
                    if (isOrderValidated || ValidationManager.ValidateOrder(order, _userID, isComingFromTT))
                    {
                        if (CachedDataManager.GetInstance.IsAlgoBrokerFromID(order.CounterPartyID) && (order.AlgoSyntheticRPLParent != string.Empty) && (order.MsgType != FIXConstants.MSGOrderCancelReplaceRequest))
                        {
                            OrderSingle cancelOrder = AlgoReplaceManager.GetInstance().GetCancelAndSaveReplaceOrder(order);
                            if (SendBlotterTrades(cancelOrder))
                            {
                                order = AlgoReplaceManager.GetInstance().GetReplaceOrder(cancelOrder.ParentClOrderID);

                                if (order == null)
                                {
                                    tradeSuccessful = true;
                                }
                                else
                                {
                                    UpdateQuantityFromChildCollection(order, cancelOrder);
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        if (order != null && TradeManagerExtensionInstance.SendTradeAfterCheckCPConnection(order, isComingFromTT))
                        {
                            tradeSuccessful = true;
                        }
                    }

                    else
                    {
                        if (ValidationManager.IsTradeMergedRemovedEdited)
                        {
                            IsTradeMergedRemovedEdited = true;
                            ValidationManager.IsTradeMergedRemovedEdited = false;
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
            return tradeSuccessful;
        }

        /// <summary>
        /// for trades from trading tkt when custodian preference is on
        /// </summary>
        public bool SendBlotterMultipleTrades(OrderSingle order, Dictionary<int, int> accountBrokerMapping, AllocationOperationPreference allocationOperationPreference, bool isStage, bool isComingFromTT = false, bool isOrderValidated = false)
        {
            bool tradeSuccessful = false;
            try
            {
                if (TradeManagerExtensionInstance.CheckTradeConditions(order, accountBrokerMapping))
                {
                    if (isOrderValidated || ValidationManager.ValidateOrder(order, _userID, isComingFromTT))
                    {
                        if (CachedDataManager.GetInstance.IsAlgoBrokerFromID(order.CounterPartyID) && (order.AlgoSyntheticRPLParent != string.Empty) && (order.MsgType != FIXConstants.MSGOrderCancelReplaceRequest))
                        {
                            OrderSingle cancelOrder = AlgoReplaceManager.GetInstance().GetCancelAndSaveReplaceOrder(order);
                            if (SendBlotterTrades(cancelOrder))
                            {
                                order = AlgoReplaceManager.GetInstance().GetReplaceOrder(cancelOrder.ParentClOrderID);

                                if (order == null)
                                {
                                    tradeSuccessful = true;
                                }
                                else
                                {
                                    UpdateQuantityFromChildCollection(order, cancelOrder);
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }

                        HashSet<int> disconnectedCP = TradeManagerExtensionInstance.CheckAllFixConnectionsStatus(order, accountBrokerMapping);
                        if (disconnectedCP.Count == 0)
                        {
                            if (!isStage && isComingFromTT)
                            {
                                MessageWithGridPopup confirmationWindow = new MessageWithGridPopup(string.Empty);
                                DataTable orderSummary = GetOrderSummary(order, allocationOperationPreference, accountBrokerMapping);
                                string message = order.IsManualOrder ? "Your trades will be filled with following brokers. Do you wish to continue?" : "Your trade is being routed to the following brokers. Do you wish to continue?";
                                DialogResult confirmationResult = confirmationWindow.ShowDialog(message, orderSummary, MessageBoxButtons.YesNo);
                                if (confirmationResult != DialogResult.Yes)
                                {
                                    return false;
                                }
                            }
                            tradeSuccessful = TradeManagerExtensionInstance.SendValidatedTrades(order, isComingFromTT);
                        }
                        else
                        {
                            if (isComingFromTT)
                                ShowDisconnectedBrokerPopUp(disconnectedCP);
                            return false;
                        }
                    }
                    else
                    {
                        if (ValidationManager.IsTradeMergedRemovedEdited)
                        {
                            IsTradeMergedRemovedEdited = true;
                            ValidationManager.IsTradeMergedRemovedEdited = false;
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
            return tradeSuccessful;
        }

        /// <summary>
        /// Returns the DataTable for Order Confirmation Window
        /// <summary>
        private DataTable GetOrderSummary(OrderSingle order, AllocationOperationPreference allocationOperationPreference, Dictionary<int, int> accountBrokerMapping)
        {
            DataTable orderSummary = new DataTable();
            try
            {
                double quantity = order.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManualSub ? order.CumQty : order.CumQtyForSubOrder;
                orderSummary.Columns.Add(COLUMN_ACCOUNT);
                orderSummary.Columns.Add(COLUMN_BROKER);
                orderSummary.Columns.Add(COLUMN_QTY);

                if (allocationOperationPreference != null)
                {
                    var allocatedAccountsTargetPercentage = allocationOperationPreference.TargetPercentage;
                    if (order.TransactionSource == TransactionSource.PST)
                    {
                        allocatedAccountsTargetPercentage = GetTargetPercentageForPTTOrder(allocationOperationPreference, order.OrderSideTagValue);
                    }
                    foreach (KeyValuePair<int, AccountValue> kvp in allocatedAccountsTargetPercentage)
                    {
                        var account = CachedDataManager.GetInstance.GetAccount(kvp.Key);
                        var broker = CachedDataManager.GetInstance.GetCounterPartyText(accountBrokerMapping[kvp.Key]);
                        var accountValue = kvp.Value.Value;
                        var qty = Math.Round(Convert.ToDouble(accountValue) * quantity / 100, 10);
                        orderSummary.Rows.Add(account, broker, qty);
                    }
                }
                else
                {
                    var account = CachedDataManager.GetInstance.GetAccount(order.Level1ID);
                    var broker = CachedDataManager.GetInstance.GetCounterPartyText(accountBrokerMapping[order.Level1ID]);
                    var qty = quantity;
                    orderSummary.Rows.Add(account, broker, qty);
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
            return orderSummary;
        }

        /// <summary>
        /// Show Message box for disconnected brokers
        /// </summary>
        private void ShowDisconnectedBrokerPopUp(HashSet<int> disconnectedCP)
        {
            try
            {
                DataTable disConnectedFix = new DataTable();
                disConnectedFix.Columns.Add(COLUMN_BROKER);
                foreach (int counterPartyId in disconnectedCP)
                {
                    disConnectedFix.Rows.Add(CachedDataManager.GetInstance.GetCounterPartyText(counterPartyId));
                }
                MessageWithGridPopup disconnectedFixDetails = new MessageWithGridPopup(string.Empty);
                disconnectedFixDetails.ShowDialog("The connections to the following brokers are disconnected, thus we cannot proceed with the trade.", disConnectedFix, MessageBoxButtons.OK);
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

        private void UpdateQuantityFromChildCollection(OrderSingle order, OrderSingle cancelOrder)
        {
            try
            {
                if (order.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSub || order.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSubChild)
                {
                    //case new order Qty is less than previous Qty
                    if (cancelOrder.Quantity < order.Quantity)
                    {
                        order.CumQty = 0.0;
                        order.LeavesQty = order.LeavesQty - (cancelOrder.LeavesQty + cancelOrder.CumQty);
                    }
                    else
                    {
                        //case new order Qty is greater than previous Qty
                        order.CumQty = 0.0;
                        order.LeavesQty = 0.0;
                    }
                }
                else
                {
                    order.CumQty = 0.0;
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
        /// Checks for duplicate trade.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        private bool CheckForDuplicateTrade(object sender, EventArgs<OrderSingle> e)
        {
            bool allowTrade = true;
            int timeInterval = 0;
            try
            {
                if (Convert.ToBoolean(TradingTktPrefs.TradingTicketRulesPrefs.IsDuplicateTradeAlert))
                {
                    timeInterval = Convert.ToInt32(TradingTktPrefs.TradingTicketRulesPrefs.DuplicateTradeAlertTime);
                }
                UserAction userAction = UserAction.None;
                string userActionType = string.Empty;
                if (TradeManagerExtensionInstance.ExistsInUserTradesCache(e.Value, timeInterval))
                {
                    DialogResult result = MessageBox.Show("This seems to be a duplicate trade. Do you wish to continue?", "TradingTicket", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    userAction = result == DialogResult.Yes ? UserAction.Yes : UserAction.No;
                    userActionType = result == DialogResult.Yes ? "Duplicate trade allowed by user." : "Duplicate trade rejected by user.";
                    if (result == DialogResult.No)
                        allowTrade = false;
                }
                TradeManagerExtensionInstance.AddTradesToUserTradesCache(e.Value, userAction, userActionType);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return allowTrade;
        }
        #endregion

        public bool SendTradingInstructionAccept()
        {
            if (TradeManagerExtensionInstance.CheckServerStatus())
            {
                return true;
            }
            return false;
        }

        #region New Thread Get DB trades and Prefs
        private static OrderBindingList _oldOrders = new OrderBindingList();
        private void GetDataAsync()
        {
            if (ClearWorkingSubOrderCollection != null)
            {
                ClearWorkingSubOrderCollection(this, new EventArgs());
            }
            else
            {
                BlotterOrderCollections.GetInstance().ClearAllCollections();
            }

            BackgroundWorker backGroundWorker = new BackgroundWorker();
            backGroundWorker.DoWork += new DoWorkEventHandler(backGroundWorker_DoWork);
            backGroundWorker.RunWorkerAsync();
            backGroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backGroundWorker_RunWorkerCompleted);
        }

        void backGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                GetTradePrefs();
                TradeManagerExtensionInstance.AddClearanceSchedulerTasks();
                TradeManagerExtensionInstance.SetupAUECWiseClearanceTime();
                //GetDBTrades();
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

        void backGroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (UpdateDictionaryByDbOrders != null)
                {
                    UpdateDictionaryByDbOrders(this, new EventArgs<OrderBindingList>(_oldOrders));
                }
                else
                {
                    BlotterOrderCollections.GetInstance().UpdateDictionarybyDBOrders(_oldOrders);
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

        private static void GetDBTrades()
        {
            try
            {
                CompanyUser loginUser = new CompanyUser();
                loginUser.CompanyUserID = _userID;
                BlotterPreferenceManager.GetInstance().SetUser(loginUser);
                _oldOrders = DBTradeManager.GetInstance().GetBlotterLaunchData(_userID);
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
        /// Sets the name of the blotter linked tab.
        /// </summary>
        /// <param name="linkedTabName">Name of the linked tab.</param>
        public void SetBlotterLinkedTabName(string linkedTabName)
        {
            try
            {
                DBTradeManager.GetInstance().SetBlotterLinkedTabName(linkedTabName, _userID);
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
        /// Gets the name of the blotter linked tab.
        /// </summary>
        /// <returns></returns>
        public string GetBlotterLinkedTabName()
        {
            try
            {
                return DBTradeManager.GetInstance().GetBlotterLinkedTabName(_userID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        #endregion

        public void GetBlotterDataFromDB()
        {
            try
            {
                if (RefreshBlotterEvent != null)
                {
                    RefreshBlotterEvent(this, new EventArgs());
                }
                else
                {
                    GetBlotterDataOnBlotterThread();
                }
                _isBlotterRefreshRequired = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public void GetBlotterDataOnBlotterThread()
        {
            try
            {
                //_tradeReceivedQueue.IsUpdating = false;
                GetBlotterData();
                //_tradeReceivedQueue.IsUpdating = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private async void GetBlotterData()
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() => GetDBTrades());

                if (UpdateDictionaryByDbOrders != null)
                {
                    UpdateDictionaryByDbOrders(this, new EventArgs<OrderBindingList>(_oldOrders));
                }
                else
                {
                    BlotterOrderCollections.GetInstance().UpdateDictionarybyDBOrders(_oldOrders);
                }
                if (BlotterRefreshCompleteEvent != null)
                    BlotterRefreshCompleteEvent(this, new EventArgs());
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

        void updateTradeAttributesList(object sender, EventArgs<OrderSingle> e)
        {
            string[] attributes = new string[6];
            attributes[0] = e.Value.TradeAttribute1;
            attributes[1] = e.Value.TradeAttribute2;
            attributes[2] = e.Value.TradeAttribute3;
            attributes[3] = e.Value.TradeAttribute4;
            attributes[4] = e.Value.TradeAttribute5;
            attributes[5] = e.Value.TradeAttribute6;
            TradeAttributesCache.updateCache(attributes);
        }

        public delegate void OrderSingleMessageDelegate(object sender, EventArgs<PranaMessage> e);


        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                TradeManagerExtensionInstance.UserTradesDispose();

                if (_blotterUpdateTimer != null)
                {
                    _blotterUpdateTimer.Elapsed -= _blotterUpdateTimer_Elapsed;
                    _blotterUpdateTimer.Dispose();
                }
                if (_proxy != null)
                {
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_UpdateTradeAttributePref);
                    _proxy.InnerChannel.UnSubscribe(CONST_ImportStarted);
                    _proxy = null;
                }
            }
        }
        #endregion

        public void HideOrderFromBlotterGrid(string commaSaperateParentClOrderId)
        {
            try
            {
                BlotterOrderCollections.GetInstance().HideOrderFromBlotterGrid(commaSaperateParentClOrderId);
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
        /// Saves the share outstanding value in sm.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="shareOutstanding">The share outstanding.</param>
        public void SaveShareOutstandingValueInSM(string symbol, double shareOutstanding)
        {
            try
            {
                _secMasterServices.SaveShareOutstanding(symbol, shareOutstanding);
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
        /// Gets the share outstanding value from sm.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        public double GetShareOutstandingValueFromSM(string symbol)
        {
            try
            {
                SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();
                secMasterRequestObj.AddData(symbol, ApplicationConstants.SymbologyCodes.TickerSymbol);
                secMasterRequestObj.HashCode = this.GetHashCode();
                List<SecMasterBaseObj> secMasterCollection = _secMasterServices.GetSMCachedData(secMasterRequestObj);
                if (secMasterCollection != null && secMasterCollection.Count > 0)
                {
                    return secMasterCollection[0].SharesOutstanding;
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
            return double.MinValue;
        }

        #region IPublishing Methods
        /// <summary>
        /// Publish method of IPublishing.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="topicName"></param>
        public void Publish(MessageData e, string topicName)
        {
            try
            {
                switch (e.TopicName)
                {
                    case CONST_ImportStarted:
                        _isBlotterRefreshRequired = false;
                        break;
                    case Topics.Topic_UpdateTradeAttributePref:
                        if (e.EventData != null && e.EventData.Count > 0)
                        {
                            DataSet ds = JsonHelper.DeserializeToObject<DataSet>(e.EventData[0].ToString());
                            CachedDataManager.RefreshAttibutesCache(ds);
                            TradeAttributesCache.KeepRecords = CachedDataManager.GetInstance.GetAttributeKeepRecords();
                        }
                        break;
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
        /// getReceiverUniqueName method of IPublishing.
        /// </summary>
        /// <returns></returns>
        public string getReceiverUniqueName()
        {
            return "TradeManager";
        }
        /// <summary>
        /// getReceiverUniqueName method of IPublishing.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// Create proxy subscription.
        /// </summary>
        private void MakeProxy()
        {
            try
            {
                _proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);
                _proxy.Subscribe(CONST_ImportStarted, null);
                _proxy.Subscribe(Topics.Topic_UpdateTradeAttributePref, null);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public Dictionary<int, int> CreateAccountBrokerMapping(List<int> accountIds, int defaultBrokerId)
        {
            var result = new Dictionary<int, int>();
            try
            {
                var accountBrokerMapping = CachedDataManager.GetInstance.GetAccountWiseExecutingBrokerMapping();
                foreach (var accountId in accountIds)
                {
                    var brokerId = accountBrokerMapping.ContainsKey(accountId) && accountBrokerMapping[accountId] != int.MinValue ? accountBrokerMapping[accountId] : defaultBrokerId;
                    result.Add(accountId, brokerId);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// Gets the account broker mapping for selected fund
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, int> GetAccountBrokerMappingForSelectedFund(int fundId, ValueList brokersValueList, AllocationOperationPreference operationPreference, OrderSingle incomingOrderRequest)
        {
            Dictionary<int, int> accountBrokerMapping = new Dictionary<int, int>();
            try
            {
                Dictionary<int, int> accountWiseExecutingBroker = CachedDataManager.GetInstance.GetAccountWiseExecutingBrokerMapping();
                List<int> accounts = new List<int>();
                if (!String.IsNullOrEmpty(CachedDataManager.GetInstance.GetAccount(fundId)))
                {
                    accounts.Add(fundId);
                }
                else
                {
                    if (operationPreference != null)
                    {
                        accounts = operationPreference.TargetPercentage.Select(account => account.Key).ToList();
                        if (incomingOrderRequest != null && incomingOrderRequest.TransactionSource == TransactionSource.PST)
                        {
                            accounts = GetTargetPercentageForPTTOrder(operationPreference, incomingOrderRequest.OrderSideTagValue).Keys.ToList();
                        }
                    }
                }

                //The list of available brokers for current user
                List<int> userBrokers = new List<int>();
                foreach (ValueListItem vl in brokersValueList.ValueListItems)
                {
                    userBrokers.Add(Convert.ToInt32(vl.DataValue));
                }

                int defaultBrokerId = TradingTktPrefs.UserTradingTicketUiPrefs.Broker.HasValue ? (int)TradingTktPrefs.UserTradingTicketUiPrefs.Broker
                                    : (TradingTktPrefs.CompanyTradingTicketUiPrefs.Broker.HasValue ? (int)TradingTktPrefs.CompanyTradingTicketUiPrefs.Broker : int.MinValue);
                defaultBrokerId = (userBrokers.Contains(defaultBrokerId)) ? defaultBrokerId : int.MinValue;

                //Store the default broker for accounts for which broker is not mapped or mapped broker is unavailable for current user
                foreach (int accountId in accounts)
                {
                    accountBrokerMapping[accountId] = (accountWiseExecutingBroker.ContainsKey(accountId) && userBrokers.Contains(accountWiseExecutingBroker[accountId])) ? accountWiseExecutingBroker[accountId]
                                                    : defaultBrokerId;
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
            return accountBrokerMapping;
        }

        /// <summary>
        /// Gets the Target percentage for PTT Orders
        /// </summary>
        /// /// <returns>SerializableDictionary<int, AccountValue></returns>
        public SerializableDictionary<int, AccountValue> GetTargetPercentageForPTTOrder(AllocationOperationPreference aop, String tagSideValue)
        {
            SerializableDictionary<int, AccountValue> allocatedAccountsTargetPercentage = aop.TargetPercentage;
            try
            {
                if (aop.CheckListWisePreference != null && aop.CheckListWisePreference.Count == 1)
                {
                    CheckListWisePreference clwp = aop.CheckListWisePreference.First().Value;
                    if (clwp.OrderSideList != null && clwp.OrderSideList.Count == 1
                        && tagSideValue.Equals(clwp.OrderSideList[0]))
                    {
                        allocatedAccountsTargetPercentage = aop.CheckListWisePreference.First().Value.TargetPercentage;
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
            return allocatedAccountsTargetPercentage;
        }

        /// <summary>
        /// Determines whether selected allocation pref is Valid for Custodian Broker
        /// </summary>
        /// <returns></returns>
        public bool IsAllocationPrefValidForCustodianBroker(OrderSingle order, AllocationOperationPreference operationPreference)
        {
            try
            {
                if (operationPreference != null && operationPreference.DefaultRule.RuleType == MatchingRuleType.None && operationPreference.DefaultRule.MatchClosingTransaction == MatchClosingTransactionType.None
                    && (operationPreference.CheckListWisePreference.Count == 0 || (order != null && order.TransactionSource == TransactionSource.PST)))
                {
                    return true;
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
            return false;
        }

        /// <summary>
        ///  This method is used to send the NAV lock date update
        /// </summary>
        /// <param name="lockDate">The NAV lock date in string format.</param>
        public void SendNAVLockDateUpdate(string lockDate)
        {
            try
            {
                TradeManagerExtensionInstance.SendNAVLockDateUpdate(lockDate);
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