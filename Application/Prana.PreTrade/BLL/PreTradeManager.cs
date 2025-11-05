using Prana.AmqpAdapter.Amqp;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Constants;
using Prana.BusinessObjects.Compliance.DataSendingObjects;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.Compliance.EventArguments;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.FIX;
using Prana.CommonDataCache;
using Prana.Fix.FixDictionary;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PreTrade.CacheStore;
using Prana.PreTrade.Classes;
using Prana.PreTrade.Connectors;
using Prana.PubSubService.Interfaces;
using Prana.ServerCommon;
using Prana.ServiceConnector;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading;

namespace Prana.PreTrade.BLL
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    internal class PreTradeManager : IDisposable, IPublishing
    {
        /// <summary>
        /// USed for communicating with esper. Sending trades and receiving alerts
        /// </summary>
        private ComplianceConnector _complianceConnector;

        /// <summary>
        /// Used for sending alerts to client and receiving an override responce
        /// </summary>
        private ClientConnector _clientConnector;

        /// <summary>
        /// Used for sending notifications to the notification sender
        /// </summary>
        private NotificationConnector _notificationConnector;

        /// <summary>
        /// proxy for the allocation service, used for pre allocation before sending order to compliance
        /// </summary>
        private ProxyBase<IAllocationServices> _allocationServices = null;

        /// <summary>
        /// Raised when an order has been validated
        /// </summary>
        internal event EventHandler<RuleCheckRecievedArguments> RuleCheckReceived;

        /// <summary>
        /// Occurs when [pending approvel request received event].
        /// </summary>
        internal event EventHandler<EventArgs<List<PranaMessage>>> PendingApprovelNotificationRecived;

        /// <summary>
        /// Timer to regularly check for stuck trades
        /// </summary>
        System.Timers.Timer orderRefreshTimer = new System.Timers.Timer(5000);
        int stuckTradeTimeout = 5;

        private int _complianceValidationTimeout = 3000;
        private int _basketComplianceValidationTimeout = 60000;
        private int _complianceValidationTimeoutDisconnect = 10;
        private string _esperRuleBasketNotConnected = "\"Esper Engine\", \"Basket Compliance\" and \"Rule Mediator\" are not connected.";
        private string _esperBasketNotConnected = "\"Esper Engine\" and \"Basket Compliance\" are not connected.";
        private string _basketRuleNotConnected = "\"Basket Compliance\" and \"Rule Mediator\" engine are not connected.";
        private string _esperRuleNotConnected = "\"Esper Engine\" and \"Rule Mediator\" are not connected.";
        private string _esperNotConnected = "\"Esper Engine\" is  not connected.";
        private string _basketNotConnected = "\"Basket Compliance\" is not connected.";
        private string _ruleNotConnected = "\"Rule Mediator\" engine is not connected.";
        private string _orderfailed = " \nTrade Details: ";

        /// <summary>
        /// list contains string field of compliance rules.
        /// </summary>
        private static List<string> fieldLists = new List<string> { "Symbol", "UnderlyingSymbol", "AccountShortName", "AccountLongName", "MasterFundName", "Exchange" };

        #region SingiltonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static PreTradeManager _preTradeManager = null;

        /// <summary>
        /// FirstAlertReceived for client
        /// </summary>
        private List<int> _firstAlertReceived = new List<int>();

        /// <summary>
        /// stores the alerts which are required while replacing the order
        /// </summary>
        private Dictionary<string, List<Alert>> _dictReplaceAlerts = new Dictionary<string, List<Alert>>();

        /// <summary>
        /// The proxy publishing
        /// </summary>
        static ProxyBase<IPublishing> _proxyPublishing;

        /// <summary>
        /// Creates the publishing proxy.
        /// </summary>
        private static void CreatePublishingProxy()
        {
            try
            {
                _proxyPublishing = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy", ex), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        private static bool IsComplianceOrderUpdatesForSamsara(Dictionary<string, PranaMessage> originalMessage)
        {
            try
            {
                foreach (PranaMessage pranaMessage in originalMessage.Values)
                {
                    PranaMessage message = pranaMessage.Clone();
                    if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_IsSamsaraUser))
                    {
                       return Convert.ToBoolean(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IsSamsaraUser].Value);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return false;
        }

        /// <summary>
        /// Publish pending/override updated message for Web application
        /// </summary>
        /// <param name="originalMessage"></param>
        private static void PublishComplianceOrderUpdatesForWeb(Dictionary<string, PranaMessage> originalMessage, bool requiresApproval = false, bool requiresApprovalResponse = false, bool isAllowed = false)
        {
            try
            {
                #region Send order to Web application
                List<PranaMessage> orders = new List<PranaMessage>();
                string topicName = requiresApproval ? Topics.Topic_PendingOrderUpdatesForWeb : (requiresApprovalResponse ? Topics.Topic_PendingOrderUpdatesForWeb :Topics.Topic_OverrideOrderUpdatesForWeb);
                foreach (PranaMessage pranaMessage in originalMessage.Values)
                {
                    PranaMessage message = pranaMessage.Clone();
                    if (requiresApproval)
                        message.FIXMessage.InternalInformation.AddField(FIXConstants.TagText, PreTradeConstants.MsgTradePending);
                    else if(requiresApprovalResponse && !isAllowed)
                        message.FIXMessage.InternalInformation.AddField(FIXConstants.TagText, PreTradeConstants.MsgTradeReject);
                    else if (requiresApprovalResponse && isAllowed)
                        message.FIXMessage.InternalInformation.AddField(FIXConstants.TagText, string.Empty);
                    orders.Add(message);
                }
                MessageData eventData = new MessageData
                {
                    EventData = orders,
                    TopicName = topicName
                };
                _proxyPublishing.InnerChannel.Publish(eventData, topicName);
                #endregion
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// private cunstructor
        /// </summary>
        private PreTradeManager()
        {
            try
            {
                if (Prana.CommonDataCache.ComplianceCacheManager.GetPreComplianceModuleEnabled())
                {
                    _complianceConnector = new ComplianceConnector();
                    _complianceConnector.AlertReceived += _complianceConnector_AlertReceived_New;
                    _complianceConnector.CalculationResponseReceived += _complianceConnector_CalculationResponseReceived;
                    _clientConnector = new ClientConnector();
                    _clientConnector.OverrideResponseReceived += _clientConnector_OverrideResponseReceived;
                    _notificationConnector = new NotificationConnector();

                    orderRefreshTimer.Elapsed += orderRefreshTimer_Elapsed;
                    orderRefreshTimer.Start();

                    _complianceValidationTimeout = Convert.ToInt32(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, "ComplianceValidationTimeout"));
                    _basketComplianceValidationTimeout = Convert.ToInt32(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, "BasketComplianceValidationTimeout"));
                    _complianceValidationTimeoutDisconnect = Convert.ToInt32(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, "ComplianceValidationTimeOutDisconnect"));
                }
                _allocationServices = new ProxyBase<IAllocationServices>("TradeAllocationServiceEndpointAddress");
                CreatePublishingProxy();
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
        }

        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        internal static PreTradeManager GetInstance()
        {
            lock (_lock)
            {
                if (_preTradeManager == null)
                    _preTradeManager = new PreTradeManager();
                return _preTradeManager;
            }
        }
        #endregion
        bool isInitialized = false;

        /// <summary>
        /// Sends the order to compliance for validation if it a single order,
        /// else stores it incase of multi trade
        /// </summary>
        /// <param name="clonedMessage"></param>
        internal void ProcessOrder(PranaMessage pranaMessage)
        {
            try
            {
                PranaMessage clonedMessage = new PranaMessage(pranaMessage.ToString());

                PranaMessage originalMessage = null;// InTradeCache.Instance.GetPranaMessage(clOrderId);                
                PranaMessage stagedMessage = null;// InTradeCache.Instance.GetPranaMessage(pranaMessage);
                PranaMessage messagePendingApprovalOrderCache = null;

                bool isPendingApprovalOrderCacheContainsOrder = false;

                if (clonedMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                {
                    string originalClOrderId = clonedMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;
                    PranaMessage message = InTradeCache.Instance.GetPranaMessage(originalClOrderId);
                    
                    Dictionary<String, PranaMessage> dictPendingApprovalOrderCache = GetPendingApprovalOrderCacheOrderIdWise();
                    if (dictPendingApprovalOrderCache.ContainsKey(originalClOrderId))
                        messagePendingApprovalOrderCache = dictPendingApprovalOrderCache[originalClOrderId];

                    if(messagePendingApprovalOrderCache !=null)
                    {
                        isPendingApprovalOrderCacheContainsOrder = true;
                        message = messagePendingApprovalOrderCache;
                    }
                  
                    if (!(int.Parse(message.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) == (int)OrderFields.PranaMsgTypes.ORDStaged
                        && (int.Parse(clonedMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) == (int)OrderFields.PranaMsgTypes.ORDNewSub
                        || int.Parse(clonedMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) == (int)OrderFields.PranaMsgTypes.ORDNewSubChild)))
                        originalMessage = message;
                }

                if (originalMessage != null)
                {

                    double finalQuantity = Convert.ToDouble(clonedMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value);

                    double originalQty = Convert.ToDouble(originalMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value);
                    double originalCumQty;
                    if (originalMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagCumQty))
                        originalCumQty = Convert.ToDouble(originalMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                    else
                        originalCumQty = Convert.ToDouble(originalMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value);

                    if(isPendingApprovalOrderCacheContainsOrder && clonedMessage.MessageType.Equals(FIXConstants.MSGOrderCancelReplaceRequest))
                    {
                        int PranaMsgType = int.Parse(originalMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value);
                        if ((PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSub))
                           originalCumQty = originalQty; 
                    }
                    double finalValue = finalQuantity - originalQty + originalCumQty;
                    clonedMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value = finalValue.ToString();
                    if (originalMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_StagedOrderID))
                    {
                        string stagedId = originalMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_StagedOrderID].Value;
                        stagedMessage = InTradeCache.Instance.GetPranaMessage(stagedId);
                        if (stagedMessage != null)
                        {
                            double stagedQty = Convert.ToDouble(stagedMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value);
                            double stagedCumQty = Convert.ToDouble(stagedMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                            double finalStagedValue = stagedCumQty + originalQty - finalQuantity;
                            //if Stage QTY is less than 0 it will be only in case if Sub is replaced with qty greater than stage order.
                            if (finalStagedValue < 0)
                                finalStagedValue = 0;
                            stagedMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value = finalStagedValue.ToString();
                        }

                    }
                }
                else
                {
                    originalMessage = clonedMessage;

                    if (originalMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_StagedOrderID))
                    {
                        double finalQuantity = Convert.ToDouble(originalMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value);
                        string stagedId = originalMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_StagedOrderID].Value;
                        stagedMessage = InTradeCache.Instance.GetPranaMessage(stagedId);
                        if (stagedMessage != null)
                        {
                            double stagedQty = Convert.ToDouble(stagedMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value);
                            double stagedCumQty = Convert.ToDouble(stagedMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                            double finalStagedValue = stagedCumQty - finalQuantity;
                            //if Stage QTY is less than 0 it will be only in case if Sub is replaced with qty greater than stage order.
                            if (finalStagedValue < 0)
                                finalStagedValue = 0;
                            stagedMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value = finalStagedValue.ToString();
                        }

                    }
                }

                Order order = GetOrderFromPranaMessage(clonedMessage);

                //if (order.FXRate == 0)
                //{
                //    int _companyId = CommonDataCache.CachedDataManager.GetInstance.GetCompanyID();
                //    ConversionRate conversionRateTradeDt = ForexConverter.GetInstance(_companyId).GetConversionRateForCurrencyToBaseCurrency(order.CurrencyID, order.ProcessDate, order.Level1ID);
                //    if (conversionRateTradeDt != null)
                //        order.FXRate = conversionRateTradeDt.RateValue;
                //}

                if (order.AssetID == (int)Prana.BusinessObjects.AppConstants.AssetCategory.FX)
                {
                    return;
                }

                if (order.MsgType.Equals(FIXConstants.MSGOrderCancelReplaceRequest) && !order.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_PendingReplace))
                {
                    return;
                }

                bool isSingleTrade = false;
                if (string.IsNullOrWhiteSpace(order.MultiTradeId))
                {
                    order.MultiTradeId = IDGenerator.GenerateMultiTradeId();
                    isSingleTrade = true;
                }

                order.CumQty = order.Quantity;
                int msgType = Convert.ToInt32(order.PranaMsgType);
                List<TaxLot> taxlotList = GetTaxlotsForOrder(order);
                string clOrderId = "";
                if (originalMessage != null)
                {
                    clOrderId = originalMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                    taxlotList.ForEach(x => x.LotId = clOrderId);
                }
                if (stagedMessage != null && ComplianceCacheManager.GetInStaging())
                {
                    string stagedOrderId = stagedMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                    Order stagedOrder = GetOrderFromPranaMessage(stagedMessage);
                    double qty = stagedOrder.Quantity;
                    stagedOrder.CumQty = qty == 0 ? stagedOrder.CumQty : qty;
                    List<TaxLot> stagedTaxlots = GetTaxlotsForOrder(stagedOrder);

                    stagedTaxlots.ForEach(x =>
                    {
                        x.IsWhatIfTradeStreamRequired = false;
                        x.LotId = stagedOrderId;
                        if (qty == 0)
                            x.TaxLotQty = qty;
                    });
                    taxlotList.AddRange(stagedTaxlots);
                }

                PendingTradeCache.GetInstance().AddToBasket(order, pranaMessage, taxlotList);
                InformationReporter.GetInstance.ComplianceLogWrite("Pre trade order received for order id- " + order.ClOrderID + " BasketId: " + PendingTradeCache.GetInstance().GetBasketId(order));
                Logger.LoggerWrite("Pre trade order received for order id- " + order.ClOrderID, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                //PendingTradeCache.GetInstance().AddToBasket(basketId, basketName, order.OrderID, order.CompanyUserID, pranaMessage, GetTaxlotsForOrder(order));

                //InformationReporter.GetInstance.Write("Pre trade order received for order id- " + order.ClOrderID);
                //EnterpriseLibraryManager.LoggerWrite("Pre trade order received for order id- " + order.ClOrderID, EnterpriseLibraryConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);

                if (isSingleTrade)
                {
                    String basketId = String.IsNullOrWhiteSpace(order.MultiTradeId) ? order.OrderID : order.MultiTradeId;
                    if (_complianceConnector != null)
                    {
                        if (ComplianceCacheManager.GetBasketComplianceCheckPermissionUser(order.ModifiedUserId) && stagedMessage == null
                            && order.MsgType.Equals(FIXConstants.MSGOrderCancelReplaceRequest) && order.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_PendingReplace))
                            _complianceConnector.SendTaxlotsToBasketCompliance(PendingTradeCache.GetInstance().GetTaxlots(basketId));
                        else
                        {
                            if (order.MsgType.Equals(FIXConstants.MSGOrderCancelReplaceRequest) && order.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_PendingReplace))
                                _complianceConnector.SendTaxlotsToEsper(PendingTradeCache.GetInstance().GetTaxlots(basketId), true);
                            else
                                _complianceConnector.SendTaxlotsToEsper(PendingTradeCache.GetInstance().GetTaxlots(basketId));
                        }
                    }
                    InformationReporter.GetInstance.ComplianceLogWrite("Sending Pre trade to esper. Order id- " + order.OrderID + " Now waiting for validation" + " BasketId: " + PendingTradeCache.GetInstance().GetBasketId(order));
                    Logger.LoggerWrite("Sending Pre trade to esper. Order id- " + order.OrderID + " Now waiting for validation", LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    int esperTimeOut = _complianceValidationTimeout;
                    if (!AmqpAdapter.Amqp.ConnectionStatusManager._isEsperConnected || !AmqpAdapter.Amqp.ConnectionStatusManager._isRuleMediatorConnected)
                        esperTimeOut = _complianceValidationTimeoutDisconnect;
                    System.Timers.Timer cntDwnTimer = new System.Timers.Timer(esperTimeOut);
                    cntDwnTimer.Elapsed += (sender, e) => cntdown_Elapsed(sender, new EventArgs<String>(basketId));
                    cntDwnTimer.Start();
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
        /// Cancels the order and send it to compliance
        /// </summary>
        /// <param name="pranaMessage"></param>
        internal void CancelPendingComplianceApprovalTrades(PranaMessage pranaMessage)
        {
            try
            {
                Order orderCancel = GetOrderFromPranaMessage(pranaMessage);
                if (orderCancel != null)
                {
                    List<Alert> alerts = PendingApprovalTradeCache.GetInstance().GetTriggerAlerts(orderCancel.OrderID, orderCancel.OrigClOrderID);
                    if (alerts == null || alerts.Count == 0)
                        alerts = PendingApprovalTradeCache.GetInstance().GetTriggerAlerts(orderCancel.OrderID, null);
                    if (alerts == null || alerts.Count == 0)
                        alerts = PendingApprovalTradeCache.GetInstance().GetTriggerAlerts(orderCancel.OrigClOrderID);

                    if (alerts != null && alerts.Count > 0)
                    {
                        if (pranaMessage.MessageType.Equals(FIXConstants.MSGOrderCancelRequest))
                        {
                            alerts.ForEach(x =>
                            {
                                x.PreTradeActionType = PreTradeActionType.Blocked;
                                x.ActionUser = int.Parse(pranaMessage.UserID);
                            });
                            TradesApprovalReceived(alerts, true);
                            _notificationConnector.SendAlertsToNotificationManagerForFrozeUnfroze(alerts, "PendingApprovalRowUnfroze");
                        }
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
        }
        /// <summary>
        /// Add or update Cache.
        /// </summary>
        /// <param name="ClOrderID" name="orderStatus"></param>
        public void AddOrUpdateStatusToOrderStatusTrackCache(string ClOrderID, ComplianceOrderStatus orderStatus)
        {
            try
            {
                OrderStatusTrackCache.GetInstance().AddOrUpdateStatusToOrderStatusTrackCache(ClOrderID,orderStatus);
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
        /// Fetches status from Cache.
        /// </summary>
        /// <param name="ClOrderId"></param>
        public ComplianceOrderStatus GetOrderStatusFromOrderStatusTrackCache(string ClOrderID)
        {
            try
            {
                ComplianceOrderStatus status= ComplianceOrderStatus.None ;
                status=OrderStatusTrackCache.GetInstance().GetOrderStatusFromOrderStatusTrackCache(ClOrderID);
                return status;
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
            return ComplianceOrderStatus.None;
        }
        /// <summary>
        /// Adds or Updates the Acknowledged Order Id for the Original Cl Order Id.
        /// </summary>
        /// <param name="clOrderId"></param>
        /// <param name="orgClOrderId"></param>
        public void AddOrUpdateAcknowledgeOrderId(string clOrderId, string orgClOrderId)
        {
            try
            {
                OrderStatusTrackCache.GetInstance().AddOrUpdateAcknowledgeOrderId(clOrderId, orgClOrderId);
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
        /// Gets the Acknowledged ClOrder Id.
        /// </summary>
        /// <param name="orgClOrderId"></param>
        /// <returns></returns>
        public string GetAcknowledgedClOrderId(string orgClOrderId)
        {
            try
            {
                return OrderStatusTrackCache.GetInstance().GetAcknowledgedClOrderId(orgClOrderId);
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
            return null;
        }
        /// <summary>
        /// Freeze Unfreeze Pending ComplianceApproval Trades
        /// </summary>
        /// <param name="pranaMessage"></param>
        internal void FreezeUnfreezePendingComplianceApprovalTrades(PranaMessage pranaMessage)
        {
            try
            {
                Order orderFreezeUnfreeze = GetOrderFromPranaMessage(pranaMessage);
                if (orderFreezeUnfreeze != null)
                {
                    List<Alert> alerts = null;
                    if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrdStatus) && pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value.Equals(FIXConstants.ORDSTATUS_PendingReplace))
                    {
                        alerts = PendingApprovalTradeCache.GetInstance().GetTriggerAlerts(orderFreezeUnfreeze.OrderID, orderFreezeUnfreeze.OrigClOrderID);
                        if (alerts == null || alerts.Count == 0)
                            alerts = PendingApprovalTradeCache.GetInstance().GetTriggerAlerts(orderFreezeUnfreeze.OrigClOrderID, orderFreezeUnfreeze.OrigClOrderID);
                    }
                    else
                    {
                        alerts = PendingApprovalTradeCache.GetInstance().GetTriggerAlerts(orderFreezeUnfreeze.OrderID, orderFreezeUnfreeze.OrigClOrderID);
                        if (alerts == null || alerts.Count == 0)
                            alerts = PendingApprovalTradeCache.GetInstance().GetTriggerAlerts(orderFreezeUnfreeze.OrderID, null);
                        if (alerts == null || alerts.Count == 0)
                            alerts = PendingApprovalTradeCache.GetInstance().GetTriggerAlerts(orderFreezeUnfreeze.OrigClOrderID);
                    }

                    if (alerts != null && alerts.Count > 0)
                    {
                        if (pranaMessage.MessageType.Equals(FIXConstants.MSGOrderCancelRequestFroze))
                        {
                            _notificationConnector.SendAlertsToNotificationManagerForFrozeUnfroze(alerts, "PendingApprovalRowFroze");
                        }
                        else if (pranaMessage.MessageType.Equals(FIXConstants.MSGOrderCancelRequestUnFroze))
                        {
                            _notificationConnector.SendAlertsToNotificationManagerForFrozeUnfroze(alerts, "PendingApprovalRowUnfroze");
                        }
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
        }

        /// <summary>
        /// Replaces the order and send it to compliance
        /// </summary>
        /// <param name="pranaMessage"></param>
        internal void UpdateReplaceOrderAlerts(PranaMessage pranaMessage)
        {

            try
            {
                Order orderReplace = GetOrderFromPranaMessage(pranaMessage);
                if (orderReplace != null)
                {
                    List<Alert> alerts = PendingApprovalTradeCache.GetInstance().GetTriggerAlerts(orderReplace.OrderID, orderReplace.OrigClOrderID);
                    if (alerts == null || alerts.Count == 0)
                        alerts = PendingApprovalTradeCache.GetInstance().GetTriggerAlerts(orderReplace.OrigClOrderID, orderReplace.OrigClOrderID);

                    if (alerts != null && alerts.Count > 0)
                    {
                        if (pranaMessage.MessageType.Equals(FIXConstants.MSGOrderCancelReplaceRequest))
                        {
                            alerts.ForEach(x =>
                            {
                                x.ActionUser = int.Parse(pranaMessage.UserID);
                            });
                            if (!_dictReplaceAlerts.ContainsKey(orderReplace.ParentClOrderID))
                                _dictReplaceAlerts.Add(orderReplace.ParentClOrderID, alerts);
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
        /// Replace the trades and update compliance approval UI
        /// </summary>
        /// <param name="orderId"></param>
        private void ReplaceAndNotifyPendingComplianceApprovalTrades(string orderIds, string replaceAlertType = null)
        {
            try
            {
                List<string> lstOrderId = orderIds.Split(',').ToList();
                foreach (string orderId in lstOrderId)
                {
                  //  orderId.Trim();
                    if (_dictReplaceAlerts.ContainsKey(orderId))
                    {
                        if (!string.IsNullOrWhiteSpace(replaceAlertType))
                            ReplacePendingComplianceApprovalTrades(_dictReplaceAlerts[orderId], replaceAlertType);
                        _notificationConnector.SendAlertsToNotificationManagerForFrozeUnfroze(_dictReplaceAlerts[orderId], "PendingApprovalRowUnfroze");
                        _dictReplaceAlerts.Remove(orderId);
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
        /// Trades Replaced Trades Received from Client Side
        /// </summary>
        /// <param name="alerts"></param>
        private void ReplacePendingComplianceApprovalTrades(List<Alert> alerts, string replaceAlertType)
        {
            try
            {
                if (alerts.Count() > 0)
                {
                    alerts.ForEach(x =>
                    {
                        x.PreTradeActionType = PreTradeActionType.Blocked;
                    });
                    string actionUserName = CachedDataManager.GetInstance.GetUserText(alerts[0].ActionUser);
                    InformationReporter.GetInstance.ComplianceLogWrite("User: " + actionUserName + " " + alerts[0].PreTradeActionType + " " + alerts.Count + " alerts for Order Id : " + alerts[0].OrderId);
                    Logger.LoggerWrite("User: " + actionUserName + " " + alerts[0].PreTradeActionType + " " + alerts.Count + " alerts for Order Id : " + alerts[0].OrderId, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);

                    PreTradeActionType isAllowed = PendingApprovalTradeCache.GetInstance().UpdateAlerts(alerts);
                    PendingTradeInfo basket = PendingApprovalTradeCache.GetInstance().GetPendingBasket(alerts[0].OrderId);
                    _notificationConnector.IsTradeFromRebalancer = basket.IsTradeFromRebalancer;

                    if (isAllowed == PreTradeActionType.Blocked)
                    {
                        if (!alerts[0].OrderId.Contains("SIMULATION_"))
                            RuleCheckReceived(this, new RuleCheckRecievedArguments(basket.PendingOrderCache, false, true, false, false));
                        string auditEntryMsg = string.Empty;

                        basket.TriggeredAlerts.ForEach(x => x.Status = PreTradeConstants.MSG_TRADE_EXPIRED_REPLACED);
                        InformationReporter.GetInstance.ComplianceLogWrite(PreTradeConstants.MSG_TRADE_EXPIRED_REPLACED + " Order : " + basket.MultiTradeName + " BasketId: " + alerts[0].OrderId);
                        Logger.LoggerWrite(PreTradeConstants.MSG_TRADE_EXPIRED_REPLACED + " Order : " + basket.MultiTradeName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        auditEntryMsg = PreTradeConstants.MSG_TRADE_EXPIRED_REPLACED;

                        //Update alerts action user and pre trade action type
                        basket.TriggeredAlerts.ForEach(x => { x.ActionUser = alerts[0].ActionUser; x.PreTradeActionType = alerts[0].PreTradeActionType; });
                        _notificationConnector.SendAlertsToNotificationManager(basket.TriggeredAlerts, replaceAlertType);
                        PendingApprovalTradeCache.GetInstance().RemoveFromCache(alerts[0].OrderId);

                        //Adding Audit Trail in DB
                        AddOrderDataAuditEntryAndSaveInDB(basket.PendingOrderCache, TradeAuditActionType.ActionType.OrderBlocked, alerts[0].ActionUser, auditEntryMsg);
                    }
                    _notificationConnector.SendAlertsApprovalResponse(alerts, alerts[0].OrderId, isAllowed, alerts[0].ActionUser);

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


        void cntdown_Elapsed(object sender, EventArgs<String> e)
        {
            try
            {
                (sender as System.Timers.Timer).Stop();
                HandleComplianceValidationFailure(e.Value);
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
        /// Add an Error Alert to the list of alerts and proceed as usual
        /// </summary>
        /// <param name="basketId"></param>
        private void HandleComplianceValidationFailure(string basketId)
        {
            try
            {
                PendingTradeInfo basket = PendingTradeCache.GetInstance().GetBasket(basketId);
                if (basket == null)
                    return;
                if (PendingTradeCache.GetInstance().IsEOMReceived(basketId))
                    return;

                PendingTradeCache.GetInstance().EOMReceived(basketId);

                List<Order> orderList = new List<Order>();
                orderList.AddRange(basket.PendingOrderCache.Select(x => Transformer.CreateOrder(x.Value)).ToList());

                Alert complianceFailureAlert = Alert.GetComplianceFailureAlert(ComplianceCacheManager.GetBlockTradeOnComplianceFaliure());
                if (orderList.Count == 1)
                {
                    Order order = orderList[0];
                    if (!AmqpAdapter.Amqp.ConnectionStatusManager._isRuleMediatorConnected && !AmqpAdapter.Amqp.ConnectionStatusManager._isEsperConnected)
                    {
                        complianceFailureAlert.Description = _esperRuleNotConnected + _orderfailed;
                        InformationReporter.GetInstance.ComplianceLogWrite(_esperRuleNotConnected + "\n");
                    }
                    else if (!AmqpAdapter.Amqp.ConnectionStatusManager._isEsperConnected)
                    {
                        complianceFailureAlert.Description = _esperNotConnected + _orderfailed;
                        InformationReporter.GetInstance.ComplianceLogWrite(_esperNotConnected + "\n");
                    }
                    else if (!AmqpAdapter.Amqp.ConnectionStatusManager._isRuleMediatorConnected)
                    {
                        complianceFailureAlert.Description = _ruleNotConnected + _orderfailed;
                        InformationReporter.GetInstance.ComplianceLogWrite(_ruleNotConnected + "\n");
                    }
                    complianceFailureAlert.Description = complianceFailureAlert.Description + TagDatabaseManager.GetInstance.GetOrderSideText(order.OrderSideTagValue) + " " + order.Symbol + " " + order.Quantity + " @" + order.AvgPrice + " for " + TagDatabaseManager.GetInstance.GetOrderTypeText(order.OrderTypeTagValue) + " Order";
                }
                else
                {
                    complianceFailureAlert.Description = complianceFailureAlert.Description + " (Multiple Orders)";
                }

                complianceFailureAlert.OrderId = basketId;
                complianceFailureAlert.UserId = basket.UserId;
                complianceFailureAlert.AlertId = PreTradeConstants.CONST_FAILED_ALERT_ID;
                basket.AddAlert(complianceFailureAlert);   // should the currect alerts be cleared ???
                string tradeDetails = GetTradeDetails(basket);
                basket.TriggeredAlerts.ForEach(x =>
                {
                    x.AlertType = PermissionHelper.GetAlertTypeForRule(x.UserId, x.RuleName);
                    x.TradeDetails = tradeDetails;
                    List<string> updatedConstraints = UpdateConstraints(x.ConstraintFields, x.Threshold, x.ActualResult);
                    x.ConstraintFields = updatedConstraints[0];
                    x.Threshold = updatedConstraints[1];
                    x.ActualResult = updatedConstraints[2];
                });
                RuleOverrideType overrideType = GetOverridePermission(basket.UserId, basket.TriggeredAlerts, true);

                switch (overrideType)
                {
                    case RuleOverrideType.Soft:
                        // ask for override
                        basket.TriggeredAlerts.ForEach(x => x.OrderId = basketId); // REPLACE THE TAXLOT ID WITH BASKET ID WHILE SENDING TO CLIENT
                        _clientConnector.InformClientForRequest(basket.UserId, basket.TriggeredAlerts.ToList(), AlertPopUpType.Override);
                        InformationReporter.GetInstance.ComplianceLogWrite("Validation failed for " + basket.MultiTradeName + ". Waiting for user input." + " BasketId: " + basket.TriggeredAlerts[0].OrderId);
                        break;

                    case RuleOverrideType.RequiresApproval:
                        _clientConnector.InformClientForRequest(basket.UserId, basket.TriggeredAlerts.ToList(), AlertPopUpType.PendingApproval);
                        InformationReporter.GetInstance.ComplianceLogWrite("Validation received for " + basket.MultiTradeName + ". Trade sent for approval to Compliance Officer, as user does not have override request for one or more rules." + " BasketId: " + basket.TriggeredAlerts[0].OrderId);
                        break;

                    case RuleOverrideType.Hard:
                        // block the trade
                        basket.TriggeredAlerts.ForEach(x => x.Status = "Trade blocked");
                        if (RuleCheckReceived != null)
                            RuleCheckReceived(this, new RuleCheckRecievedArguments(basket.PendingOrderCache, false, false));

                        if (ComplianceCacheManager.GetBlockTradeOnComplianceFaliure() && basket.TriggeredAlerts != null && basket.TriggeredAlerts.Count > 0 &&
                                            (basket.TriggeredAlerts[0].RuleName == "N/A" || basket.TriggeredAlerts[0].RuleName == "Missing Information Alert"
                                            || basket.TriggeredAlerts[0].RuleName.Contains("Compliance Failed")
                                            || basket.TriggeredAlerts[0].RuleName == "Something went wrong !! Please Contact Support."))
                        {
                            _clientConnector.InformClientForRequest(basket.UserId, basket.TriggeredAlerts.ToList(), AlertPopUpType.Inform);
                        }

                        string msg = "Validation failed for " + basket.MultiTradeName + ". Trade blocked as user does not have override permission. " + basket.TriggeredAlerts[0].RuleName;
                        InformationReporter.GetInstance.ComplianceLogWrite(msg);
                        break;
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
        /// Use the allocation service to get the taxlots for an order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private List<TaxLot> GetTaxlotsForOrder(Order order, bool isForceAllocation = false)
        {
            try
            {
                //For manual trades cum quamtity should be tested.
                //While for fix trades cum quantity must be equal to quantity so assigning it for checking
                if (!(order.PranaMsgType == Convert.ToInt32(Prana.Global.OrderFields.PranaMsgTypes.ORDManual) || order.PranaMsgType == Convert.ToInt32(Prana.Global.OrderFields.PranaMsgTypes.ORDManualSub)))
                {

                    if (order.OrderTypeTagValue == FIXConstants.ORDTYPE_Limit || order.OrderTypeTagValue == FIXConstants.ORDTYPE_LimitOnClose || order.OrderTypeTagValue == FIXConstants.ORDTYPE_LimitOrBetter)
                        order.AvgPrice = order.Price;
                    else if (order.OrderTypeTagValue == FIXConstants.ORDTYPE_Stop || order.OrderTypeTagValue == FIXConstants.ORDTYPE_Stoplimit)
                        order.AvgPrice = order.StopPrice;
                    else if (order.OrderTypeTagValue == FIXConstants.ORDTYPE_Market && order.AvgPrice == 0) // In case of Live order
                        order.AvgPrice = order.AvgPriceForCompliance;
                }

                order.Venue = CommonDataCache.CachedDataManager.GetInstance.GetVenueText(order.VenueID);
                order.CounterPartyName = CommonDataCache.CachedDataManager.GetInstance.GetCounterPartyText(order.CounterPartyID);
                order.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(order.OrderSideTagValue);
                order.OrderType = TagDatabaseManager.GetInstance.GetOrderTypeText(order.OrderTypeTagValue);
                Dictionary<int, int> accountBrokerMapping = new Dictionary<int, int>();
                if (order.IsUseCustodianBroker && !string.IsNullOrEmpty(order.AccountBrokerMapping))
                {
                    accountBrokerMapping = JsonHelper.DeserializeToObject<Dictionary<int, int>>(order.AccountBrokerMapping);                   
                }

                //  order.CumQty = order.Quantity;

                AllocationGroup allocationGroup = _allocationServices.InnerChannel.CreateVirtualAllocationGroup(order, false, isForceAllocation);
                List<TaxLot> taxlots = allocationGroup.GetAllTaxlots();
                foreach (TaxLot tax in taxlots)
                {
                    if (tax.Level1ID == int.MinValue || tax.Level1ID == 0)
                        tax.Level1ID = -1;

                    if (tax.Level2ID == int.MinValue)
                        tax.Level2ID = -1;

                    //Set taxlot wise broker in case of custodian brokers
                    if (accountBrokerMapping.ContainsKey(tax.Level1ID))
                    {
                        tax.CounterPartyID = accountBrokerMapping[tax.Level1ID];
                        tax.CounterPartyName = CommonDataCache.CachedDataManager.GetInstance.GetCounterPartyText(accountBrokerMapping[tax.Level1ID]);
                    }

                    tax.TIF = TagDatabaseManager.GetInstance.GetTIFText(order.TIF);
                    if (tax.FXConversionMethodOperator.ToString().Trim().ToUpper() != "M")
                    {
                        if (tax.FXRate != 0)
                        {
                            tax.FXRate = 1 / tax.FXRate;
                        }
                        tax.FXConversionMethodOperator = "M";
                    }
                    tax.UnderlyingName = CachedDataManager.GetInstance.GetAssetText(tax.UnderlyingID);
                    tax.AssetCategoryValue = (AssetCategory)order.AssetID;
                    tax.LimitPrice = order.Price;
                    tax.StopPrice = order.StopPrice;
                    if (order.OrderTypeTagValue == FIXConstants.ORDTYPE_Market && order.AvgPrice == 0) // In case of Live order
                        tax.AvgPrice = order.AvgPriceForCompliance;

                    tax.AssetID = order.AssetID;
                    tax.AUECID = order.AUECID;
                    tax.ContractMultiplier = order.ContractMultiplier;
                    tax.OrderSideTagValue = order.OrderSideTagValue;
                    tax.SideMultiplier = Calculations.GetSideMultilpier(order.OrderSideTagValue);
                    tax.AssetName = tax.AssetCategoryValue.ToString();

                    tax.TradeAttribute1 = order.TradeAttribute1;
                    tax.TradeAttribute2 = order.TradeAttribute2;
                    tax.TradeAttribute3 = order.TradeAttribute3;
                    tax.TradeAttribute4 = order.TradeAttribute4;
                    tax.TradeAttribute5 = order.TradeAttribute5;
                    tax.TradeAttribute6 = order.TradeAttribute6;
                    tax.SetTradeAttribute(order.GetTradeAttributesAsDict());

                    decimal accountSymbolQuantityPost = _allocationServices.InnerChannel.GetCurrentStateForSymbolInAccount(order.Symbol, order.ModifiedUserId, tax.Level1ID);
                    decimal accountSymbolQuantityInMarket = 0;
                    if (ComplianceCacheManager.GetInMarket())
                        accountSymbolQuantityInMarket = Convert.ToDecimal(TradingRulesInMarketCache.GetInstance().GetInMarketNetPostionOfSymbolInAccount(order.Symbol, order.Level1ID));
                    if ((accountSymbolQuantityPost + accountSymbolQuantityInMarket) >= 0)
                    {
                        #region LongSideRank
                        if (order.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy))
                            tax.SideRank = 1;
                        else if (order.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Open))
                            tax.SideRank = 2;
                        else if (order.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed))
                            tax.SideRank = 3;
                        else if (order.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell))
                            tax.SideRank = 4;
                        else if (order.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed))
                            tax.SideRank = 5;
                        else if (order.OrderSideTagValue.Equals(FIXConstants.SIDE_SellShort))
                            tax.SideRank = 6;
                        else if (order.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Open))
                            tax.SideRank = 7;
                        #endregion
                    }
                    else
                    {
                        #region ShortSideRank
                        if (order.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Open))
                            tax.SideRank = 1;
                        else if (order.OrderSideTagValue.Equals(FIXConstants.SIDE_SellShort))
                            tax.SideRank = 2;
                        else if (order.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed))
                            tax.SideRank = 3;
                        else if (order.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell))
                            tax.SideRank = 4;
                        else if (order.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed))
                            tax.SideRank = 5;
                        else if (order.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Open))
                            tax.SideRank = 6;
                        else if (order.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy))
                            tax.SideRank = 7;
                        #endregion
                    }
                    /*
					// if isStageValueFromField is tue then it will set the Value of avgprice on the basis mapping in admin 
                    try
                    {
                        bool isStageValueFromField = ComplianceCacheManager.GetStageValueFromField();
                        if (isStageValueFromField && tax.OrderTypeTagValue.Equals("1"))
                        {
                            string fromFieldString = "_" + ComplianceCacheManager.GetStageValueFromFieldString().ToLower();
                            var taxlotFields = tax.GetType().BaseType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                            var taxlotField = taxlotFields.SingleOrDefault(a => a.Name.ToLower().Contains(fromFieldString));
                            if (taxlotField != null)
                            {
                                double _txtDoubleValue;
                                if (Double.TryParse((taxlotField.GetValue(tax)).ToString(), out _txtDoubleValue))
                                {
                                    tax.AvgPrice = _txtDoubleValue;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                    }
                     */

                    // if group id is null or empty then fetching from group cache.
                    if (String.IsNullOrEmpty(tax.GroupID))
                        tax.GroupID = _allocationServices.InnerChannel.GetGroupID(order.ParentClOrderID);

                    if (tax.TaxLotID.Length < tax.GroupID.Length)
                        tax.TaxLotID = tax.GroupID + tax.TaxLotID;

                    // if taxlot id is null or empty the taxlot will not be validated
                    // using a new taxlot ID
                    if (String.IsNullOrEmpty(tax.TaxLotID))
                        tax.TaxLotID = "WI" + IDGenerator.GenerateClientOrderID();
                }
                //  taxlots.ForEach(x => x.LotId = string.IsNullOrWhiteSpace(order.StagedOrderID) ? order.ClOrderID : order.StagedOrderID);
                return taxlots;
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
                return null;
            }
        }

        /// <summary>
        /// Gets the order object from prana message and formats it for compliance use
        /// </summary>
        /// <param name="pranaMessage"></param>
        /// <returns></returns>
        private Order GetOrderFromPranaMessage(PranaMessage pranaMessage)
        {
            try
            {
                Order order = Transformer.CreateOrder(pranaMessage);

                if (string.IsNullOrWhiteSpace(order.OrderID) || !string.IsNullOrEmpty(order.AccountBrokerMapping))
                {
                    if (!String.IsNullOrEmpty(order.ClOrderID))
                        order.OrderID = order.ClOrderID;
                    else
                        order.OrderID = "WI" + IDGenerator.GenerateClientOrderID();
                }

                order.CurrencyName = CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(order.CurrencyID);

                int nirvanaMsgType = int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value);
                //updating orderid if it is cancel replace
                //TODO: need to correct for replace chaining.
                if ((pranaMessage.MessageType == FIXConstants.MSGOrderCancelRequest || pranaMessage.MessageType == FIXConstants.MSGOrderRollOverRequest || pranaMessage.MessageType == FIXConstants.MSGOrderCancelReplaceRequest) && pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                {
                    order.StagedOrderID = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;
                }

                if (order.OrderStatusTagValue != string.Empty)
                {
                    order.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(order.OrderStatusTagValue.ToString());
                }


                return order;
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
                return null;
            }
        }

        /// <summary>
        /// All orders in a multi trade are received, send to esper
        /// </summary>
        /// <param name="multiTradeName"></param>
        /// <param name="noOfOrders"></param>
        internal void MultiTradeEOMReceived(string multiTradeId, String userID, int noOfOrders)
        {
            try
            {
                // TO-DO : add a check for the count
                //if(PendingTradeCache.GetInstance().GetOrders(multiTradeName).Count == noOfOrder)
                if (_complianceConnector != null)
                {
                    _complianceConnector.SendTaxlotsToEsper(PendingTradeCache.GetInstance().GetTaxlots(multiTradeId));
                }

                String multiTradeName = PendingTradeCache.GetInstance().GetBasketName(multiTradeId);

                InformationReporter.GetInstance.ComplianceLogWrite("Sending Pre trade to esper. Multitrade- " + multiTradeName + " Now waiting for validation");
                Logger.LoggerWrite("Sending Pre trade to esper. Multitrade- " + multiTradeName + " Now waiting for validation", LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);

                System.Timers.Timer cntDwnTimer = new System.Timers.Timer(_complianceValidationTimeout);
                cntDwnTimer.Elapsed += (sender, e) => cntdown_Elapsed(sender, new EventArgs<String>(multiTradeId));
                cntDwnTimer.Start();
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
        /// Process the alerts received for the pop up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        void _complianceConnector_AlertReceived_New(object sender, EventArgs<Alert> e)
        {
            try
            {
                // Note : the order id in the alert object is actually the taxlot group id (basket id)
                string basketid = e.Value.OrderId;
                if (e.Value.IsEOM)
                {
                    InformationReporter.GetInstance.ComplianceLogWrite(string.Format("EOM Received for {0} - {1}", basketid, e.Value.Description));
                    // Validation complete, process the alerts
                    if (SimulationCache.GetInstance().IsSimulation(basketid))
                    {
                        SimulationCache.GetInstance().ReceivedSimulationEOM(basketid);
                        return;
                    }

                    PendingTradeInfo basket = PendingTradeCache.GetInstance().GetBasket(basketid);
                    if (basket == null)
                    {
                        InformationReporter.GetInstance.ComplianceLogWrite("Order is no longer in pending order cache. OrderId: " + basketid);
                        return;
                    }

                    string orderIds = basket.GetClOrderId();
                    if (PendingTradeCache.GetInstance().IsEOMReceived(basketid))
                        return;

                    PendingTradeCache.GetInstance().EOMReceived(basketid);

                    int userId = basket.UserId;
                    string hardRuleMessage = string.Empty;
                    string message = string.Format("EOM Received for {0} - {1}", basketid, e.Value.Description);
                    if (basket.TriggeredAlerts.Count == 0)
                    {
                        ReplaceAndNotifyPendingComplianceApprovalTrades(orderIds, PreTradeConstants.MSG_NO_ALERT_RECEIVED);

                        // There were no alerts, pass the trade
                        if (RuleCheckReceived != null)
                            RuleCheckReceived(this, new RuleCheckRecievedArguments(basket.PendingOrderCache, true, false));
                        PendingTradeCache.GetInstance().RemoveBasket(basketid);
                        message = "Validation received for " + basket.MultiTradeName + ". Trade passed as no rules violated.";
                        PublishComplianceOrderUpdatesForWeb(basket.PendingOrderCache);
                        //For sending response to user even in case of no breach (For Samsara)
                        if(IsComplianceOrderUpdatesForSamsara(basket.PendingOrderCache))
                            _clientConnector.InformClientForRequest(userId, basket.TriggeredAlerts, AlertPopUpType.None);
                    }
                    else
                    {
                        string tradeDetails = GetTradeDetails(basket);
                       //basket.TriggeredAlerts.GroupBy(X => new { X.RuleName, X.Dimension }).Select(x => x.First()).ToList();
                        basket.TriggeredAlerts.ForEach(x =>
                        {
                            x.AlertType = PermissionHelper.GetAlertTypeForRule(userId, x.RuleName);
                            x.TradeDetails = tradeDetails;
                            List<string> updatedConstraints = UpdateConstraints(x.ConstraintFields, x.Threshold, x.ActualResult);
                            x.ConstraintFields = updatedConstraints[0];
                            x.Threshold = updatedConstraints[1];
                            x.ActualResult = updatedConstraints[2];
                        });
                        hardRuleMessage = String.Join(",", basket.TriggeredAlerts.Where(x => x.AlertType == AlertType.HardAlert).Select(x => x.RuleName).ToList());
                        basket.TriggeredAlerts[0].UserId = userId;
                        string ClOrderId=basket.GetClientOrderId();
                        RuleOverrideType overrideType = GetOverridePermission(basket.UserId, basket.TriggeredAlerts);
                        switch (overrideType)
                        {
                            case RuleOverrideType.Soft:
                                // Ask for override
                                basket.TriggeredAlerts.ForEach(x => x.OrderId = basketid); // REPLACE THE TAXLOT ID WITH BASKET ID WHILE SENDING TO CLIENT
                                _clientConnector.InformClientForRequest(userId, basket.TriggeredAlerts.ToList(), AlertPopUpType.Override);
                                break;
                            case RuleOverrideType.RequiresApproval:
                                OrderStatusTrackCache.GetInstance().AddOrUpdateStatusToOrderStatusTrackCache(ClOrderId, ComplianceOrderStatus.PendingApproval);
                                _clientConnector.InformClientForRequest(userId, basket.TriggeredAlerts.ToList(), AlertPopUpType.PendingApproval);
                                message = "Validation received for " + basket.MultiTradeName + ". Trade sent for approval to Compliance Officer, as user does not have override request for one or more rules.";
                                break;
                            case RuleOverrideType.Hard:
                                // Block the trade
                                basket.TriggeredAlerts.ForEach(x => x.Status = "Trade blocked");
                                if (RuleCheckReceived != null)
                                    RuleCheckReceived(this, new RuleCheckRecievedArguments(basket.PendingOrderCache, false, false));
                                _clientConnector.InformClientForRequest(userId, basket.TriggeredAlerts.ToList(), AlertPopUpType.Inform);

                                if (ComplianceCacheManager.GetBlockTradeOnComplianceFaliure() && basket.TriggeredAlerts != null && basket.TriggeredAlerts.Count > 0 &&
                                        (basket.TriggeredAlerts[0].RuleName == "N/A" || basket.TriggeredAlerts[0].RuleName == "Missing Information Alert"
                                        || basket.TriggeredAlerts[0].RuleName.Contains("Compliance Failed")
                                        || basket.TriggeredAlerts[0].RuleName == "Something went wrong !! Please Contact Support."))
                                {
                                    _clientConnector.InformClientForRequest(userId, basket.TriggeredAlerts.Where(x => x.Blocked).ToList(), AlertPopUpType.Inform);
                                }
                                message = "Validation received for " + basket.MultiTradeName + ". Trade blocked, as user does not have override request for one or more rules. Rules as follows-: " + hardRuleMessage + " BasketId: " + basketid;
                                InformationReporter.GetInstance.ComplianceLogWrite(message);
                                break;
                        }
                    }
                }
                else
                {
                    // Store the alert and wait for the EOM
                    if (SimulationCache.GetInstance().IsSimulation(basketid))
                    {
                        if (!_firstAlertReceived.Contains(e.Value.UserId))
                        {
                            _firstAlertReceived.Add(e.Value.UserId);
                            _clientConnector.SendFeedbackMessage(PreTradeConstants.CONST_VALIDATING_RULES, e.Value.UserId);
                            //InformationReporter.GetInstance.Write(string.Format("First alert received for {0}", basketid));
                        }
                        SimulationCache.GetInstance().AddAlerts(basketid, e.Value);
                        //Logger.LoggerWrite(string.Format("Alert Received : {0} for BasketId: {1}", e.Value.RuleName, basketid), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                    else
                    {
                        string orderIds = PendingTradeCache.GetInstance().GetBasketOrderIds(basketid);
                        Logger.LoggerWrite(string.Format("Alert Received : {0} for {1} Parent ClOrderIds: {2}", e.Value.RuleName, basketid, orderIds), LoggingConstants.CATEGORY_INFORMATION_REPORTER_COMPLIANCE, 1, 1, TraceEventType.Information);
                    }
                    PendingTradeCache.GetInstance().AddAlert(basketid, e.Value);
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
        /// To get the trade details for the alerts 
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        private string GetTradeDetails(PendingTradeInfo basket)
        {
            string tradeDetails = "Multiple Orders";
            try
            {
                List<Order> orderList = new List<Order>();
                orderList.AddRange(basket.PendingOrderCache.Select(x => Transformer.CreateOrder(x.Value)).ToList());
                if (orderList.Count == 1)
                {
                    string currencyText = string.Empty;
                    double tradePrice = orderList[0].AvgPrice; ;
                    if (orderList[0].OrderTypeTagValue == FIXConstants.ORDTYPE_Market && tradePrice == 0) // In case of Live order
                        tradePrice = orderList[0].AvgPriceForCompliance;
                    if (orderList[0].CurrencyID != int.MinValue)
                        currencyText = CachedDataManager.GetInstance.GetCurrencyText(orderList[0].CurrencyID);
                    tradeDetails = TagDatabaseManager.GetInstance.GetOrderSideText(orderList[0].OrderSideTagValue) + " " + String.Format("{0:n0}", orderList[0].Quantity) + " " + orderList[0].Symbol + " @" + tradePrice + ", Notional " + currencyText + " " + String.Format("{0:n0}", (tradePrice * orderList[0].Quantity));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return tradeDetails;
        }
        /// <summary>
        /// TO update Thresold and Actual Result if there is single numeric and multiple string values.
        /// </summary>
        /// <param name="constraintFields"></param>
        /// <param name="threshold"></param>
        /// <param name="actualResult"></param>
        /// <returns></returns>
        private List<string> UpdateConstraints(string constraintFields, string threshold, string actualResult)
        {
            List<string> finalList = new List<string>();
            try
            {
                List<string> constraintFieldsList = new List<string>();
                List<string> thresholdList = new List<string>();
                List<string> actualResultList = new List<string>();

                constraintFieldsList = constraintFields.Split(PreTradeConstants.CONST_SEPARATOR_CHAR.ToCharArray()).ToList();
                thresholdList = threshold.Split(PreTradeConstants.CONST_SEPARATOR_CHAR.ToCharArray()).ToList();
                actualResultList = actualResult.Split(PreTradeConstants.CONST_SEPARATOR_CHAR.ToCharArray()).ToList();

                double numericValue;
                string newConstraintFields = "";
                string newThreshold = "";
                string newActualResult = "";

                for (int i = 0; i < thresholdList.Count; i++)
                {
                    bool isNumeric = double.TryParse(thresholdList[i], out numericValue);
                    if (isNumeric && !fieldLists.Contains(constraintFieldsList[i]))
                    {
                        newConstraintFields += constraintFieldsList[i] + PreTradeConstants.CONST_SEPARATOR_CHAR;
                        newThreshold += thresholdList[i] + PreTradeConstants.CONST_SEPARATOR_CHAR;
                        newActualResult += actualResultList[i] + PreTradeConstants.CONST_SEPARATOR_CHAR;
                    }
                }
                if (newConstraintFields.Length > 0)
                {
                    newConstraintFields = newConstraintFields.Substring(0, newConstraintFields.Length - 1);
                    newThreshold = newThreshold.Substring(0, newThreshold.Length - 1);
                    newActualResult = newActualResult.Substring(0, newActualResult.Length - 1);

                    finalList.Add(newConstraintFields);
                    finalList.Add(newThreshold);
                    finalList.Add(newActualResult);
                }
                else
                {
                    finalList.Add("N/A");
                    finalList.Add("N/A");
                    finalList.Add("N/A");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return finalList;
        }

        /// <summary>
        /// Returns true is the user has permission to override all alerts
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="alerts"></param>
        /// <returns></returns>
        private RuleOverrideType GetOverridePermission(int userId, List<Alert> alerts, bool isFailedAlert = false)
        {
            RuleOverrideType result = RuleOverrideType.Soft;
            try
            {

                bool blockOnComplianceFailure = ComplianceCacheManager.GetBlockTradeOnComplianceFaliure();
                //If there is only one alert and it is either missing information, live ffed not connected or something went wrong alert, then set ruleOverride permission to be Override in this case
                if (alerts.Count == 1)
                {
                    RuleOverrideType ruleOverRideType = RuleOverrideType.Hard;
                    if (!blockOnComplianceFailure)
                        ruleOverRideType = RuleOverrideType.Soft;

                    switch (alerts[0].RuleName)
                    {
                        case "N/A":
                        case "Missing Information Alert":
                        case "Something went wrong !! Please Contact Support.":
                            return ruleOverRideType;
                    }
                    if (alerts[0].AlertId.Equals(PreTradeConstants.CONST_FAILED_ALERT_ID))
                    {
                        alerts[0].AlertType = AlertType.SoftAlert;
                        if (blockOnComplianceFailure)
                            alerts[0].AlertType = AlertType.HardAlert;
                        return ruleOverRideType;
                    }
                }
                else if (isFailedAlert)
                {
                    alerts.Where(alert => alert.AlertId.Equals(PreTradeConstants.CONST_FAILED_ALERT_ID)).FirstOrDefault().AlertType = blockOnComplianceFailure ? AlertType.HardAlert : AlertType.SoftAlert;
                }
                HashSet<string> uniqueRules = alerts.Select(x => x.RuleName).ToHashSet();
                result = PermissionHelper.GetOverridePermissionForRule(userId, uniqueRules);
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
        /// Process the override response received or the dismissed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _clientConnector_OverrideResponseReceived(object sender, EventArgs<DataSet> e)
        {
            try
            {
                bool isAllowed = Convert.ToBoolean(e.Value.Tables[0].Rows[0]["IsAllowed"].ToString());
                String basketId = e.Value.Tables[0].Rows[0]["orderId"].ToString();// +e.Value.Tables[0].Rows[0]["UserId"].ToString();
                bool isApprovalRequired = Convert.ToBoolean(e.Value.Tables[0].Rows[0]["isApprovalRequired"].ToString());
                int userId = Convert.ToInt32(e.Value.Tables[0].Rows[0]["UserId"].ToString());
                AlertPopUpType popUpType = AlertPopUpType.None;
                AlertPopUpResponse popUpResponse = AlertPopUpResponse.None;
                if (e.Value.Tables[0].Columns.Contains("popUpType"))
                    popUpType = (AlertPopUpType)Enum.Parse(typeof(AlertPopUpType), e.Value.Tables[0].Rows[0]["popUpType"].ToString());

                PendingTradeInfo basket = PendingTradeCache.GetInstance().GetBasket(basketId);

                if (basket == null)
                {
                    InformationReporter.GetInstance.ComplianceLogWrite("Order is no longer in pending order cache. OrderId: " + basketId);
                    Logger.LoggerWrite("Order is no longer in pending order cache. OrderId: " + basketId, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                }
                else if (isApprovalRequired && popUpType == AlertPopUpType.PendingApproval)
                {
                    string orderIds = basket.GetClOrderId();
                    ReplaceAndNotifyPendingComplianceApprovalTrades(orderIds, AlertType.RequiresApproval.ToString());

                    if (basketId.Contains("SIMULATION_") && _notificationConnector.IsTradeFromRebalancer)
                        PendingApprovalTradeCache.GetInstance().AddToCache(basket, basketId, true);
                    else
                        PendingApprovalTradeCache.GetInstance().AddToCache(basket, basketId, false);
                    if (basket.PendingOrderCache.Count > 0 && !basketId.Contains("SIMULATION_"))
                        PendingApprovelNotificationRecived(this, new EventArgs<List<PranaMessage>>(basket.PendingOrderCache.Values.ToList()));
                    _notificationConnector.SendAlertsToForApproval(PendingApprovalTradeCache.GetInstance().GetBasketForApproval(basketId));
                    PendingTradeCache.GetInstance().RemoveBasket(basketId);

                    //Adding Audit Trail in DB
                    AddOrderDataAuditEntryAndSaveInDB(basket.PendingOrderCache, TradeAuditActionType.ActionType.OrderPendingApproval, userId);

                    #region Send order update for web application in case of stage order
                    if (basketId.Contains("SIMULATION_"))
                        PublishComplianceOrderUpdatesForWeb(basket.PendingOrderCache, true);
                    #endregion
                }
                else if (RuleCheckReceived != null)
                {
                    if (!e.Value.Tables[0].Columns.Contains("isDismissed"))
                    {
                        string orderIds = basket.GetClOrderId();
                        if (isAllowed || popUpType == AlertPopUpType.None)
                        {
                            ReplaceAndNotifyPendingComplianceApprovalTrades(orderIds, AlertType.SoftAlert.ToString());

                            if (!basketId.Contains("SIMULATION_"))
                                RuleCheckReceived(this, new RuleCheckRecievedArguments(basket.PendingOrderCache, true, true));
                            popUpResponse = AlertPopUpResponse.Yes;
                            basket.TriggeredAlerts.ForEach(x => x.Status = "Trade passed as rule is overriden by user (" + CachedDataManager.GetInstance.GetUserText(x.UserId) + ").");
                            basket.TriggeredAlerts.ForEach(x => x.AlertPopUpResponse = popUpResponse);
                            InformationReporter.GetInstance.ComplianceLogWrite("Trade passed as user has overriden it. Order : " + basket.MultiTradeName + " BasketId: " + basketId + " Clicked: " + popUpResponse.ToString());
                            Logger.LoggerWrite("Trade passed as user has overriden it. Order : " + basket.MultiTradeName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                            //Adding Audit Trail in DB
                            AddOrderDataAuditEntryAndSaveInDB(basket.PendingOrderCache, TradeAuditActionType.ActionType.TradeOverriden, userId, "Trade passed as user has overriden it");
                            PublishComplianceOrderUpdatesForWeb(basket.PendingOrderCache);
                        }
                        else
                        {
                            if (popUpType == AlertPopUpType.Inform)
                            {
                                ReplaceAndNotifyPendingComplianceApprovalTrades(orderIds, AlertType.HardAlert.ToString());

                                popUpResponse = AlertPopUpResponse.Ok;
                                InformationReporter.GetInstance.ComplianceLogWrite("Trade blocked by user." + " Order : " + basket.MultiTradeName + " BasketId: " + basketId + " Clicked: " + popUpResponse.ToString());
                                Logger.LoggerWrite("Trade blocked by user." + " Order : " + basket.MultiTradeName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                basket.TriggeredAlerts.ForEach(x => x.Status = "Trade blocked by Hard Alert.");
                                basket.TriggeredAlerts.ForEach(x => x.AlertPopUpResponse = popUpResponse);
                                Logger.LoggerWrite("Trade blocked by Hard Alert." + " Order : " + basket.MultiTradeName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                            }
                            else
                            {
                                if (!basketId.Contains("SIMULATION_") && !_dictReplaceAlerts.ContainsKey(orderIds))
                                    RuleCheckReceived(this, new RuleCheckRecievedArguments(basket.PendingOrderCache, false, true));

                                ReplaceAndNotifyPendingComplianceApprovalTrades(orderIds);

                                if (popUpType == AlertPopUpType.PendingApproval)
                                    popUpResponse = AlertPopUpResponse.Cancel;
                                else if (popUpType == AlertPopUpType.Override)
                                    popUpResponse = AlertPopUpResponse.No;

                                else if (popUpType == AlertPopUpType.ComplianceCheck)
                                {
                                    popUpResponse = AlertPopUpResponse.Ok;
                                    InformationReporter.GetInstance.ComplianceLogWrite("Order is no longer in pending order cache. OrderId: " + basket.MultiTradeName + " BasketId: " + basketId + " Clicked: " + popUpResponse.ToString());
                                    Logger.LoggerWrite("Order is no longer in pending order cache. OrderId: " + basket.MultiTradeName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                }

                                basket.TriggeredAlerts.ForEach(x => x.Status = "Trade blocked by user (" + CachedDataManager.GetInstance.GetUserText(x.UserId) + ").");
                                basket.TriggeredAlerts.ForEach(x => x.AlertPopUpResponse = popUpResponse);
                                InformationReporter.GetInstance.ComplianceLogWrite("Trade blocked by user." + " Order : " + basket.MultiTradeName + " BasketId: " + basketId);
                                Logger.LoggerWrite("Trade blocked by user." + " Order : " + basket.MultiTradeName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                //Adding Audit Trail in DB
                                AddOrderDataAuditEntryAndSaveInDB(basket.PendingOrderCache, TradeAuditActionType.ActionType.OrderBlocked, userId, "Order Blocked by Trading User");
                            }
                        }
                    }
                    _notificationConnector.SendAlertsToNotificationManager(basket.TriggeredAlerts);
                    PendingTradeCache.GetInstance().RemoveBasket(basketId);
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
        /// Add Audit Trail Collection
        /// </summary>
        /// <param name="orRequest"></param>
        /// <param name="action"></param>
        private void AddOrderDataAuditEntryAndSaveInDB(Dictionary<String, PranaMessage> orRequest, TradeAuditActionType.ActionType action, int userId, string comment = null)
        {
            try
            {
                System.Threading.Tasks.Task.Run(() =>
            {
                List<TradeAuditEntry> auditCollection = new List<TradeAuditEntry>();
                foreach (var item in orRequest)
                {
                    var order = Transformer.CreateOrder(item.Value);
                    TradeAuditEntry audit = new TradeAuditEntry();
                    audit.Action = action;
                    audit.AUECLocalDate = DateTime.Now;
                    audit.OriginalDate = order.AUECLocalDate;
                    audit.CompanyUserId = userId;
                    audit.GroupID = string.Empty;
                    audit.TaxLotID = string.Empty;
                    audit.ParentClOrderID = order.ParentClOrderID;
                    audit.ClOrderID = order.ClOrderID;
                    audit.Symbol = order.Symbol;
                    audit.Level1ID = order.Level1ID;
                    audit.OrderSideTagValue = order.OrderSideTagValue;

                    if (!string.IsNullOrWhiteSpace(comment))
                    {
                        audit.Comment = comment;
                    }
                    else
                    {
                        TradeAuditActionTypeConverter ac = TypeDescriptor.GetConverter(typeof(TradeAuditActionType.ActionType)) as TradeAuditActionTypeConverter;
                        audit.Comment = (string)ac.ConvertTo(null, System.Globalization.CultureInfo.CurrentCulture, action, typeof(string));
                    }

                    audit.Source = Prana.BusinessObjects.TradeAuditActionType.ActionSource.Compliance;
                    auditCollection.Add(audit);
                }

                AuditManager.Instance.SaveAuditList(auditCollection);
            });

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Order was overridden from the server
        /// </summary>
        /// <param name="isAllowed"></param>
        /// <param name="orderId"></param>
        internal void OverrideTrade(bool isAllowed, String orderId)
        {
            Dictionary<String, PranaMessage> basket = PendingTradeCache.GetInstance().PopOrder(orderId);

            if (basket == null)
            {
                List<Alert> alerts = PendingApprovalTradeCache.GetInstance().GetTriggerAlerts(orderId);
                basket = PendingApprovalTradeCache.GetInstance().PopOrder(orderId);
                if (basket != null)
                {
                    _notificationConnector.SendAlertsApprovalResponse(alerts, alerts[0].OrderId, isAllowed ? PreTradeActionType.Allowed : PreTradeActionType.Blocked, alerts.Select(x1 => x1.ActionUser).ToList().First());
                }
            }
            if (basket == null)
            {
                InformationReporter.GetInstance.ComplianceLogWrite("Order is no longer in pending order cache. OrderId: " + orderId);
                Logger.LoggerWrite("Order is no longer in pending order cache. OrderId: " + orderId, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
            }
            else
            {
                if (RuleCheckReceived != null)
                {
                    if (isAllowed)
                    {
                        if (orderId.Contains("SIMULATION_") || orderId.Contains("WI"))
                            RuleCheckReceived(this, new RuleCheckRecievedArguments(basket, true, true, true));
                        else
                            RuleCheckReceived(this, new RuleCheckRecievedArguments(basket, true, true));
                        InformationReporter.GetInstance.ComplianceLogWrite("Order passed at server. Order : " + orderId);
                        Logger.LoggerWrite("Trade passed at server. Order : " + orderId, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                    else
                    {
                        if (!orderId.Contains("SIMULATION_") && !orderId.Contains("WI"))
                            RuleCheckReceived(this, new RuleCheckRecievedArguments(basket, false, true));
                        InformationReporter.GetInstance.ComplianceLogWrite("Trade blocked at server. Order : " + orderId);
                        Logger.LoggerWrite("Trade blocked at server. Order : " + orderId, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                }
            }
        }

        #region Dispose
        /// <summary>
        /// Dispose() calls Dispose(true)
        /// </summary>
        public void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
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
        /// Disposing Objects
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_allocationServices != null)
                        _allocationServices.Dispose();
                    if (orderRefreshTimer != null)
                        orderRefreshTimer.Dispose();
                    if (_proxy != null)
                        _proxy.Dispose();
                    if (_complianceConnector != null)
                        _complianceConnector.Dispose();
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
        #endregion

        /// <summary>
        /// Returns all orders
        /// </summary>
        /// <returns></returns>
        internal Dictionary<String, PranaMessage> GetAllCachedMessages()
        {
            return PendingTradeCache.GetInstance().GetAllCachedMessages();
        }

        /// <summary>
        /// Timer to check stuck trades and show on the trade server accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void orderRefreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                foreach (var order in PendingTradeCache.GetInstance().GetAllBaskets())
                {
                    int diffSecond = (int)(DateTime.Now - order.Value.RequestTime).TotalSeconds;
                    if (diffSecond > stuckTradeTimeout)
                    {
                        string basketId = string.Empty;
                        if (order.Value!=null && order.Value.TriggeredAlerts != null && order.Value.TriggeredAlerts.Count > 0)
                            basketId = order.Value.TriggeredAlerts[0].OrderId;
                        InformationReporter.GetInstance.ComplianceLogWrite("Order :- " + order.Value.MultiTradeName + " is stuck on trade server from last " + diffSecond + " seconds. Please check communications to compliance engine and if it is running. This might also happen when client has override permission and did not respond yet." + " BasketId: " + basketId);
                        Logger.LoggerWrite("Order :- " + order.Value.MultiTradeName + " is stuck on trade server from last " + diffSecond + " seconds. Please check communications to compliance engine and if it is running. This might also happen when client has override permission and did not respond yet.", LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Warning);
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
        /// Send the simulation basket to esper and wait for the alerts
        /// ManualResetEvent used for making the compliance call synchronous
        /// </summary>
        /// <param name="orderSingle"></param>
        internal SimulationResult SimulateTrades(List<OrderSingle> orderSingle, PreTradeType preTradeType, int companyUserID, bool isRealTimePositions, bool isComingFromRebalancer)
        {
            try
            {
                String simId = IDGenerator.GenerateSimulationId();
                ManualResetEvent simulationCompletion = new ManualResetEvent(false);
                SimulationCache.GetInstance().AddNewSimulation(simId, simulationCompletion);
                if (isComingFromRebalancer)
                    _notificationConnector.IsTradeFromRebalancer = true;
                else
                    _notificationConnector.IsTradeFromRebalancer = false;

                if (_complianceConnector != null)
                    _complianceConnector.SendSimulationPreferenceToBasketCompliance(isRealTimePositions, isComingFromRebalancer);
                List<TaxLot> basket = new List<TaxLot>();

                int ctr = 1;

                if (_clientConnector != null)
                    _clientConnector.SendFeedbackMessage(PreTradeConstants.CONST_CREATING_VIRTUAL_PORTFOLIO, companyUserID);
                // building the basket
                foreach (OrderSingle os in orderSingle)
                {
                    if (os.AssetID == (int)Prana.BusinessObjects.AppConstants.AssetCategory.FX)
                    {
                        continue;
                    }

                    PranaMessage msg = Transformer.CreatePranaMessageThroughReflection(os);
                    ServerCommonBusinessLogic.SetDateDetails(msg);

                    Order order = GetOrderFromPranaMessage(msg);
                    order.CumQty = order.Quantity;
                    String parentClOrderId = simId + ctr++;
                    order.ParentClOrderID = parentClOrderId;
                    order.ClOrderID = parentClOrderId;
                    //if (order.FXRate == 0)
                    //{
                    //    int _companyId = CommonDataCache.CachedDataManager.GetInstance.GetCompanyID();
                    //    ConversionRate conversionRateTradeDt = ForexConverter.GetInstance(_companyId).GetConversionRateForCurrencyToBaseCurrency(order.CurrencyID, order.ProcessDate, order.Level1ID);
                    //    if (conversionRateTradeDt != null)
                    //        order.FXRate = conversionRateTradeDt.RateValue;
                    //}

                    List<TaxLot> taxlots = GetTaxlotsForOrder(order);
                    taxlots.ForEach(x => x.LotId = order.ClOrderID);
                    basket.AddRange(taxlots);
                    PendingTradeCache.GetInstance().AddToBasket(order, msg, taxlots, true, simId);
                }
                basket.ForEach(x => x.GroupID = simId);


                if (basket.Count == 0)
                    return null;
                bool basketCompliancePermissionForUser = ComplianceCacheManager.GetBasketComplianceCheckPermissionUser(companyUserID);
                if (basketCompliancePermissionForUser)
                {
                    if (_complianceConnector != null)
                    {
                        _clientConnector.SendFeedbackMessage(PreTradeConstants.CONST_SENDING_DATA_FOR_CALCULATIONS, companyUserID);
                        _complianceConnector.SendTaxlotsToBasketCompliance(basket);
                    }
                }
                else
                {
                    if (_complianceConnector != null)
                    {
                        _complianceConnector.SendTaxlotsToEsper(basket);
                    }
                }
                InformationReporter.GetInstance.ComplianceLogWrite(string.Format("Simulation started for {0}", simId));
                Logger.LoggerWrite("Simulation started : " + simId, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);

                List<Alert> alerts = null;
                string hardRuleMessage = string.Empty;
                Boolean isOverridable = false;
                RuleOverrideType overrideType = RuleOverrideType.Soft;
                int basketTimeout = basketCompliancePermissionForUser ? _basketComplianceValidationTimeout : _complianceValidationTimeout;
                if (!AmqpAdapter.Amqp.ConnectionStatusManager._isEsperConnected || !AmqpAdapter.Amqp.ConnectionStatusManager._isRuleMediatorConnected || (basketCompliancePermissionForUser && !AmqpAdapter.Amqp.ConnectionStatusManager._isBasketconnected))
                {
                    Logger.LoggerWrite($"For simulationId {simId}, EsperConnection:{ConnectionStatusManager._isEsperConnected}, BasketConnection:{ConnectionStatusManager._isBasketconnected}, RuleMediaterConnection:{ConnectionStatusManager._isRuleMediatorConnected}");
                    basketTimeout = _complianceValidationTimeoutDisconnect;
                }
                    
                if (simulationCompletion.WaitOne(basketTimeout))
                {
                    if (_clientConnector != null && _firstAlertReceived.Contains(companyUserID))
                    {
                        _firstAlertReceived.Remove(companyUserID);
                        _clientConnector.SendFeedbackMessage(PreTradeConstants.CONST_GENERATING_RESULTS, companyUserID);
                    }
                    alerts = SimulationCache.GetInstance().GetAlerts(simId);

                    PendingTradeInfo basketFinal = PendingTradeCache.GetInstance().GetBasket(simId);
                    string tradeDetails = GetTradeDetails(basketFinal);
                    alerts.ForEach(x =>
                    {
                        x.PreTradeType = preTradeType;
                        x.AlertType = PermissionHelper.GetAlertTypeForRule(x.UserId, x.RuleName);
                        x.TradeDetails = tradeDetails;
                        List<string> updatedConstraints = UpdateConstraints(x.ConstraintFields, x.Threshold, x.ActualResult);
                        x.ConstraintFields = updatedConstraints[0];
                        x.Threshold = updatedConstraints[1];
                        x.ActualResult = updatedConstraints[2];
                    });
                    Boolean isBlocked = alerts.Any(x => x.Blocked);
                    overrideType = GetOverridePermission(orderSingle[0].CompanyUserID, alerts);
                    if (overrideType == RuleOverrideType.Hard)
                    {
                        hardRuleMessage = String.Join(",", alerts.Where(x => x.AlertType == AlertType.HardAlert).Select(x => x.RuleName).ToList());
                        String message = "Validation received for " + basketFinal.MultiTradeName + ". Trade blocked, as user does not have override request for one or more rules. Rules as follows-: " + hardRuleMessage + " BasketId: " + simId;
                        InformationReporter.GetInstance.ComplianceLogWrite(message);
                    }
                    if (isBlocked)
                        isOverridable = overrideType == RuleOverrideType.Soft || overrideType == RuleOverrideType.RequiresApproval;
                    else
                        isOverridable = true;
                }

                if (alerts == null)
                {
                    List<Alert> alertList = new List<Alert>();
                    alertList.Add(Alert.GetBasketComplianceFailureAlert(ComplianceCacheManager.GetBlockTradeOnComplianceFaliure()));
                    if (!AmqpAdapter.Amqp.ConnectionStatusManager._isRuleMediatorConnected && !AmqpAdapter.Amqp.ConnectionStatusManager._isEsperConnected && basketCompliancePermissionForUser && !AmqpAdapter.Amqp.ConnectionStatusManager._isBasketconnected)
                    {
                        alertList[0].Description = _esperRuleBasketNotConnected + _orderfailed;
                        InformationReporter.GetInstance.ComplianceLogWrite(_esperRuleBasketNotConnected + "\n");
                    }
                    else if (!AmqpAdapter.Amqp.ConnectionStatusManager._isEsperConnected && basketCompliancePermissionForUser && !AmqpAdapter.Amqp.ConnectionStatusManager._isBasketconnected)
                    {
                        alertList[0].Description = _esperBasketNotConnected + _orderfailed;
                        InformationReporter.GetInstance.ComplianceLogWrite(_esperBasketNotConnected + "\n");
                    }
                    else if (!AmqpAdapter.Amqp.ConnectionStatusManager._isRuleMediatorConnected && !AmqpAdapter.Amqp.ConnectionStatusManager._isEsperConnected)
                    {
                        alertList[0].Description = _esperRuleNotConnected + _orderfailed;
                        InformationReporter.GetInstance.ComplianceLogWrite(_esperRuleNotConnected + "\n");
                    }
                    else if (!AmqpAdapter.Amqp.ConnectionStatusManager._isBasketconnected && basketCompliancePermissionForUser && !AmqpAdapter.Amqp.ConnectionStatusManager._isRuleMediatorConnected)
                    {
                        alertList[0].Description = _basketRuleNotConnected + _orderfailed;
                        InformationReporter.GetInstance.ComplianceLogWrite(_basketRuleNotConnected + "\n");
                    }
                    else if (basketCompliancePermissionForUser && !AmqpAdapter.Amqp.ConnectionStatusManager._isBasketconnected)
                    {
                        alertList[0].Description = _basketNotConnected + _orderfailed;
                        InformationReporter.GetInstance.ComplianceLogWrite(_basketNotConnected + "\n");
                    }
                    else if (!AmqpAdapter.Amqp.ConnectionStatusManager._isEsperConnected)
                    {
                        alertList[0].Description = _esperNotConnected + _orderfailed;
                        InformationReporter.GetInstance.ComplianceLogWrite(_esperNotConnected + "\n");
                    }
                    else if (!AmqpAdapter.Amqp.ConnectionStatusManager._isRuleMediatorConnected)
                    {
                        alertList[0].Description = _ruleNotConnected + _orderfailed;
                        InformationReporter.GetInstance.ComplianceLogWrite(_ruleNotConnected + "\n");
                    }

                    alertList[0].OrderId = simId;
                    PendingTradeInfo basketFinal = PendingTradeCache.GetInstance().GetBasket(simId);
                    alertList[0].UserId = basketFinal.UserId;
                    basketFinal.AddAlert(alertList[0]);
                    string tradeDetails = GetTradeDetails(basketFinal);
                    basketFinal.TriggeredAlerts.ForEach(x =>
                    {
                        x.PreTradeType = preTradeType;
                        x.AlertType = PermissionHelper.GetAlertTypeForRule(x.UserId, x.RuleName);
                        x.TradeDetails = tradeDetails;
                        List<string> updatedConstraints = UpdateConstraints(x.ConstraintFields, x.Threshold, x.ActualResult);
                        x.ConstraintFields = updatedConstraints[0];
                        x.Threshold = updatedConstraints[1];
                        x.ActualResult = updatedConstraints[2];
                    });
                    overrideType = GetOverridePermission(companyUserID, alertList, true);
                    alerts = alertList;
                    Logger.LoggerWrite(alertList[0].Description + " BasketId: " + simId, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                }
                return new SimulationResult(isOverridable, alerts, overrideType, simId);

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

        /// <summary>
        /// Gets the taxlots for pre order.
        /// </summary>
        /// <param name="orderSingle">The order single.</param>
        /// <returns></returns>
        internal List<TaxLot> GetTaxlotsForPreOrder(OrderSingle orderSingle, double orderQty)
        {
            List<TaxLot> allTaxlots = new List<TaxLot>();
            try
            {
                PranaMessage msg = Transformer.CreatePranaMessageThroughReflection(orderSingle);
                ServerCommonBusinessLogic.SetDateDetails(msg);

                Order order = GetOrderFromPranaMessage(msg);
                order.CumQty = orderQty;
                allTaxlots = GetTaxlotsForOrder(order, true);
                allTaxlots = allTaxlots.Where(x => x.TaxLotState != ApplicationConstants.TaxLotState.Deleted).ToList();
                if (orderSingle.MsgType == FIXConstants.MSGOrderCancelReplaceRequest)
                {

                    PranaMessage oldMessage = InTradeCache.Instance.GetPranaMessage(orderSingle.ClOrderID);
                    if (oldMessage != null)
                    {
                        PranaMessage message = new PranaMessage(oldMessage.ToString());

                        Order oldOrder = GetOrderFromPranaMessage(message);
                        oldOrder.CumQty = oldOrder.Quantity;

                        List<TaxLot> origTaxlotList = GetTaxlotsForOrder(oldOrder, true);
                        if (origTaxlotList != null)
                        {
                            origTaxlotList = origTaxlotList.Where(x => x.TaxLotState != ApplicationConstants.TaxLotState.Deleted).ToList();
                            oldOrder.CumQty = oldOrder.Quantity - oldOrder.LeavesQty;
                            List<TaxLot> execTaxlotList = GetTaxlotsForOrder(oldOrder, true);
                            if (execTaxlotList != null)
                            {
                                foreach (TaxLot execTaxlot in execTaxlotList)
                                {
                                    TaxLot origTaxlot = origTaxlotList.FirstOrDefault(x =>
                                        x.Level1ID == execTaxlot.Level1ID && x.Level2ID == execTaxlot.Level2ID);
                                    if (origTaxlot != null)
                                    {
                                        origTaxlot.Quantity = origTaxlot.TaxLotQty;
                                        origTaxlot.ExecutedQty = execTaxlot.TaxLotQty;
                                    }
                                }
                            }

                            origTaxlotList.ForEach(x => x.TaxLotState = ApplicationConstants.TaxLotState.Deleted);
                            allTaxlots.AddRange(origTaxlotList);
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
            return allTaxlots;
        }

        #region Intrade
        internal void ProcessInTrade(List<PranaMessage> pranaMessageList, bool isStartUpData, bool isPublishDataFromAllocation)
        {
            try
            {
                bool isStartUpDataInitiated = isStartUpData;

                List<PranaMessage> messages = InTradeCache.Instance.UpdateAndGetAffectedMessages(pranaMessageList, isStartUpData);
                foreach (PranaMessage message in messages)
                {
                    int msgType = int.Parse(message.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value);

                    if ((msgType == (int)OrderFields.PranaMsgTypes.ORDStaged && ComplianceCacheManager.GetInStaging()) ||
                            (msgType != (int)OrderFields.PranaMsgTypes.ORDStaged &&
                            (ComplianceCacheManager.GetInMarket() || CachedDataManager.GetInstance.GetIsMarketDataPermissionEnabledForTradingRules())))
                    {

                        if (msgType == (int)OrderFields.PranaMsgTypes.ORDStaged || msgType == (int)OrderFields.PranaMsgTypes.ORDNewSub
                            || msgType == (int)OrderFields.PranaMsgTypes.ORDManualSub || msgType == (int)OrderFields.PranaMsgTypes.ORDManual)
                        {
                            double cumQty = Convert.ToDouble(message.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                            if (cumQty == 0 || message.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value == FIXConstants.ORDSTATUS_Cancelled)
                            {
                                message.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = message.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value;
                                message.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value = FIXConstants.ORDSTATUS_Cancelled;
                            }
                          
                            if (isStartUpData && message.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value == FIXConstants.ORDSTATUS_Cancelled)
                            {
                                continue;
                            }

                            if (isStartUpData || isPublishDataFromAllocation || cumQty == 0
                                || message.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value == FIXConstants.ORDSTATUS_New
                                || message.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value == FIXConstants.ORDSTATUS_Cancelled
                                || message.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value == FIXConstants.ORDSTATUS_Replaced || OrderCacheManager.HasMultiDayHistory(message))
                            {
                                Order order = GetOrderFromPranaMessage(message);

                                if (order.AssetID == (int)Prana.BusinessObjects.AppConstants.AssetCategory.FX)
                                    continue;
                                if (string.IsNullOrEmpty(order.Symbol))
                                {
                                    Logger.HandleException(new Exception("PreTradeManager: Symbols are blank for ClOrderID: " + order.ClOrderID), LoggingConstants.POLICY_LOGANDSHOW);
                                }

                                List<TaxLot> taxlotList = GetTaxlotsForOrder(order);

                                foreach (TaxLot taxlot in taxlotList)
                                {
                                    taxlot.LotId = order.ClOrderID;
                                    if (order.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_Cancelled))
                                    {
                                        taxlot.TaxLotState = ApplicationConstants.TaxLotState.Deleted;
                                    }
                                }
                                if ((msgType == (int)OrderFields.PranaMsgTypes.ORDStaged && ComplianceCacheManager.GetInStaging()) ||
                                (msgType != (int)OrderFields.PranaMsgTypes.ORDStaged && ComplianceCacheManager.GetInMarket()))
                                    _complianceConnector.SendInTradesToEsper(taxlotList, GetRoutingKey(msgType));
                                if (msgType != (int)OrderFields.PranaMsgTypes.ORDStaged)
                                {
                                    ExpnlServiceConnector.GetInstance().UpdateInMarketTaxlots(taxlotList, isStartUpDataInitiated);
                                    TradingRulesInMarketCache.GetInstance().addToCache(taxlotList, isStartUpDataInitiated);
                                    isStartUpDataInitiated = false;
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
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sends InTrade orders to Esper.
        /// </summary>
        /// <param name="pranaMessage"></param>
        private void ProcessInTradeOrder(PranaMessage pranaMessage)
        {
            try
            {
                Order order = GetOrderFromPranaMessage(pranaMessage);

                if (order.AssetID == (int)Prana.BusinessObjects.AppConstants.AssetCategory.FX)
                    return;

                if (order.OrderStatusTagValue == (FIXConstants.ORDSTATUS_PendingCancel) || order.OrderStatusTagValue == (FIXConstants.ORDSTATUS_PendingRollOver) || order.OrderStatusTagValue == (FIXConstants.ORDSTATUS_PendingReplace) || order.OrderStatusTagValue == (FIXConstants.ORDSTATUS_PendingNew))
                    return;

                int nirvanaMsgType = int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value);

                if (nirvanaMsgType != (int)OrderFields.PranaMsgTypes.ORDStaged && (order.LeavesQty == 0 && order.CumQty == 0))
                    return;
                else if (nirvanaMsgType != (int)OrderFields.PranaMsgTypes.ORDStaged && order.CumQty == order.Quantity)
                    order.OrderStatusTagValue = FIXConstants.ORDSTATUS_Cancelled;//as filled so sending as deleted in Intrade
                else if (nirvanaMsgType == (int)OrderFields.PranaMsgTypes.ORDStaged && order.CumQty == 0)
                    order.CumQty = order.Quantity;

                order.CumQty = order.LeavesQty == 0 ? order.CumQty : order.LeavesQty; //Updates order cumQty as working Quantity
                //String basketId = order.MultiTradeId == null || order.MultiTradeId == String.Empty ? order.OrderID : order.MultiTradeId;
                //String basketName = order.MultiTradeName == null || order.MultiTradeName == String.Empty ? order.OrderID : order.MultiTradeName;

                List<TaxLot> taxlotList = GetTaxlotsForOrder(order);//Gets TAxlots after allocation
                //taxlotList.ForEach(x => x.LotId = string.IsNullOrWhiteSpace(order.StagedOrderID) ? string.IsNullOrWhiteSpace(order.OrderID) ? order.ClOrderID : order.OrderID : order.StagedOrderID);
                taxlotList.ForEach(x => x.TaxLotState = order.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_Cancelled) ? ApplicationConstants.TaxLotState.Deleted : x.TaxLotState);
                if (_complianceConnector != null)
                {
                    _complianceConnector.SendInTradesToEsper(taxlotList, GetRoutingKey(nirvanaMsgType));
                }
                if (nirvanaMsgType != (int)OrderFields.PranaMsgTypes.ORDStaged)
                {
                    ExpnlServiceConnector.GetInstance().UpdateInMarketTaxlots(taxlotList);
                    TradingRulesInMarketCache.GetInstance().addToCache(taxlotList, false);
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
        /// Returns routing for in trade
        /// </summary>
        /// <param name="nirvanaMsgType"></param>
        /// <returns></returns>
        private string GetRoutingKey(int nirvanaMsgType)
        {

            if (nirvanaMsgType == (int)OrderFields.PranaMsgTypes.ORDStaged)
                return "InTradeStage";
            else
                return "InTradeMarket";

        }

        /// <summary>
        /// Process Intrade orders in loop
        /// </summary>
        /// <param name="pranaMessageList"></param>
        /// <param name="key"></param>
        internal void ProcessInTradeOrders(List<PranaMessage> pranaMessageList)
        {
            try
            {
                foreach (PranaMessage message in pranaMessageList)
                {
                    ProcessInTradeOrder(message);
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

        internal void HideOrderFromBlotter(List<string> listParentClOrderId)
        {
            try
            {
                foreach (string parentClOrderId in listParentClOrderId)
                {
                    PranaMessage pranaMessage = InTradeCache.Instance.GetPranaMessage(parentClOrderId);
                    if (pranaMessage != null)
                    {
                        PranaMessage message = new PranaMessage(pranaMessage.ToString());
                        int msgType = int.Parse(message.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value);

                        if ((msgType == (int)OrderFields.PranaMsgTypes.ORDStaged && ComplianceCacheManager.GetInStaging()) && InTradeCache.Instance.RemovePranaMessageFromCache(parentClOrderId))
                        {
                            Order order = GetOrderFromPranaMessage(message);

                            if (order.AssetID == (int)Prana.BusinessObjects.AppConstants.AssetCategory.FX)
                                continue;

                            if (string.IsNullOrEmpty(order.Symbol))
                            {
                                Logger.HandleException(new Exception("PreTradeManager > HideOrderFromBlotter: Symbols are blank for ClOrderID: " + order.ClOrderID), LoggingConstants.POLICY_LOGANDSHOW);
                            }

                            List<TaxLot> taxlotList = GetTaxlotsForOrder(order);
                            taxlotList.ForEach(x => x.LotId = order.ClOrderID);
                            taxlotList.ForEach(x => x.TaxLotState = ApplicationConstants.TaxLotState.Deleted);

                            _complianceConnector.SendInTradesToEsper(taxlotList, GetRoutingKey(msgType));
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

        #endregion

        #region Get calculation from Esper
        /// <summary>
        /// Synchrousnly return the requested caculateions after fetching the data from esper CEP engine
        /// </summary>
        /// <param name="compression"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        internal DataTable GetCalculationsFromEsper(Compression compression, List<String> fields)
        {
            try
            {
                String requestId = IDGenerator.GenerateSimulationId();
                ManualResetEvent mre = new ManualResetEvent(false);
                CalculationRequestCache.GetInstance().addNewRequest(requestId, mre);
                _complianceConnector.SendCalculationRequestToEsper(requestId, compression.ToString(), string.Join(",", fields));
                Logger.LoggerWrite("Calculation request sent to esper : " + requestId, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                if (mre.WaitOne(3000))
                {
                    Logger.LoggerWrite("Calculation request received from esper : " + requestId, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    return CalculationRequestCache.GetInstance().getCaclulations(requestId);
                }
                Logger.LoggerWrite("Calculation request failed : " + requestId, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
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
            return null;
        }

        /// <summary>
        /// Merge the received calculations in to one data table and update the cache
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _complianceConnector_CalculationResponseReceived(object sender, EventArgs<DataSet> e)
        {
            try
            {
                if (e.Value.Tables != null && e.Value.Tables.Count > 0)
                {
                    DataTable calculations = e.Value.Tables[0].Copy();
                    String reqId = e.Value.Tables[0].Rows[0]["RequestId"].ToString();
                    for (int i = 1; i < e.Value.Tables.Count; i++)
                    {
                        calculations.Rows.Add(e.Value.Tables[i].Rows[0].ItemArray);
                    }
                    CalculationRequestCache.GetInstance().addCalculations(reqId, calculations);
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
        #endregion

        /// <summary>
        /// Save alerts in the Database and add to alert history grid
        /// </summary>
        /// <param name="alerts"></param>
        public void SendToNotificationManager(List<Alert> alerts)
        {
            try
            {
                _notificationConnector.SendAlertsToNotificationManager(alerts);
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
        /// Trades Approval Received from Client Side
        /// </summary>
        /// <param name="alerts"></param>
        internal PreTradeActionType TradesApprovalReceived(List<Alert> alerts, bool isCancelOrder = false)
        {
            try
            {
                if (alerts.Count() > 0)
                {
                    string actionUserName = CommonDataCache.CachedDataManager.GetInstance.GetUserText(alerts[0].ActionUser);
                    InformationReporter.GetInstance.ComplianceLogWrite("User: " + actionUserName + " " + alerts[0].PreTradeActionType + " " + alerts.Count + " alerts for Order Id : " + alerts[0].OrderId);
                    Logger.LoggerWrite("User: " + actionUserName + " " + alerts[0].PreTradeActionType + " " + alerts.Count + " alerts for Order Id : " + alerts[0].OrderId, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);

                    PreTradeActionType isAllowed = PendingApprovalTradeCache.GetInstance().UpdateAlerts(alerts);
                    PendingTradeInfo basket = PendingApprovalTradeCache.GetInstance().GetPendingBasket(alerts[0].OrderId);
                    _notificationConnector.IsTradeFromRebalancer = basket.IsTradeFromRebalancer;
                    int remainingAlert = 0;
                    if (!isAllowed.Equals(PreTradeActionType.Blocked))
                        remainingAlert = PendingApprovalTradeCache.GetInstance().GetRemainingAlertsCount(alerts[0].OrderId);

                    InformationReporter.GetInstance.ComplianceLogWrite("Alerts waiting for Approval: " + remainingAlert + ", for Order Id: " + alerts[0].OrderId);
                    Logger.LoggerWrite("Alerts waiting for Approval: " + remainingAlert + ", for Order Id: " + alerts[0].OrderId, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    string ClOId = basket.GetClientOrderId();
                    if (isAllowed == PreTradeActionType.Allowed)
                    {
                        OrderStatusTrackCache.GetInstance().AddOrUpdateStatusToOrderStatusTrackCache(ClOId, ComplianceOrderStatus.Approved);
                        if (alerts[0].OrderId.Contains("SIMULATION_"))
                            RuleCheckReceived(this, new RuleCheckRecievedArguments(basket.PendingOrderCache, true, true, true));
                        else
                            RuleCheckReceived(this, new RuleCheckRecievedArguments(basket.PendingOrderCache, true, true));
                        basket.TriggeredAlerts.ForEach(x => x.Status = ("Trade passed as rule is overriden by user (" + actionUserName + ")."));
                        InformationReporter.GetInstance.ComplianceLogWrite("Trade passed as Compliance Officers has overriden it. Order : " + basket.MultiTradeName + " BasketId: " + basket.TriggeredAlerts[0].OrderId);
                        Logger.LoggerWrite("Trade passed as user has overriden it. Order : " + basket.MultiTradeName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);

                        //Clear old alerts and added updated alerts list to save in DB with updated Action User, pretrade action type
                        basket.TriggeredAlerts.Clear();
                        basket.TriggeredAlerts.AddRange(PendingApprovalTradeCache.GetInstance().GetTriggerAlerts(alerts[0].OrderId));
                        _notificationConnector.SendAlertsToNotificationManager(basket.TriggeredAlerts);
                        PendingApprovalTradeCache.GetInstance().RemoveFromCache(alerts[0].OrderId);

                        //Adding Audit Trail in DB
                        AddOrderDataAuditEntryAndSaveInDB(basket.PendingOrderCache, TradeAuditActionType.ActionType.OrderApproved, alerts[0].ActionUser, "Trade Approved by Compliance officer(" + actionUserName + ")");
                        
                        #region Send order update for web application
                        if (!isCancelOrder)
                            PublishComplianceOrderUpdatesForWeb(basket.PendingOrderCache, false, true, true);
                        #endregion
                    }
                    else if (isAllowed == PreTradeActionType.Blocked)
                    {
                        if (!alerts[0].OrderId.Contains("SIMULATION_"))
                            RuleCheckReceived(this, new RuleCheckRecievedArguments(basket.PendingOrderCache, false, true, false, isCancelOrder));
                        string auditEntryMsg = string.Empty;
                        if (isCancelOrder)
                        {
                            basket.TriggeredAlerts.ForEach(x => x.Status = PreTradeConstants.MSG_TRADE_EXPIRED);
                            InformationReporter.GetInstance.ComplianceLogWrite(PreTradeConstants.MSG_TRADE_EXPIRED + " Order : " + basket.MultiTradeName + " BasketId: " + alerts[0].OrderId);
                            Logger.LoggerWrite(PreTradeConstants.MSG_TRADE_EXPIRED + " Order : " + basket.MultiTradeName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                            auditEntryMsg = PreTradeConstants.MSG_TRADE_EXPIRED;
                        }
                        else
                        {
                            basket.TriggeredAlerts.ForEach(x => x.Status = "Trade blocked by user (" + actionUserName + ").");
                            InformationReporter.GetInstance.ComplianceLogWrite("Trade blocked by Compliance Officers. Order : " + basket.MultiTradeName + " BasketId: " + alerts[0].OrderId);
                            Logger.LoggerWrite("Trade blocked by user. Order : " + basket.MultiTradeName, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                            auditEntryMsg = "Trade Blocked by Compliance officer(" + actionUserName + ")";
                        }

                        //Update alerts action user and pre trade action type
                        basket.TriggeredAlerts.ForEach(x => { x.ActionUser = alerts[0].ActionUser; x.PreTradeActionType = alerts[0].PreTradeActionType; });
                        _notificationConnector.SendAlertsToNotificationManager(basket.TriggeredAlerts);
                        PendingApprovalTradeCache.GetInstance().RemoveFromCache(alerts[0].OrderId);

                        //Adding Audit Trail in DB
                        AddOrderDataAuditEntryAndSaveInDB(basket.PendingOrderCache, TradeAuditActionType.ActionType.OrderBlocked, alerts[0].ActionUser, auditEntryMsg);

                        #region Send order update for web application
                        if (!isCancelOrder)
                            PublishComplianceOrderUpdatesForWeb(basket.PendingOrderCache, false, true, false);
                        #endregion
                    }
                    _notificationConnector.SendAlertsApprovalResponse(alerts, alerts[0].OrderId, isAllowed, alerts[0].ActionUser);
                    return isAllowed;
                }
                return PreTradeActionType.NoAction;
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
                return PreTradeActionType.NoAction;
            }
        }

        DuplexProxyBase<ISubscription> _proxy;
        internal void MakeProxy()
        {
            try
            {
                if (!isInitialized)
                {
                    BackgroundWorker bgUnAllocateData = new BackgroundWorker();
                    bgUnAllocateData.DoWork += new DoWorkEventHandler(makeProxy_DoWork);
                    bgUnAllocateData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(makeProxy_RunWorkerCompleted);
                    bgUnAllocateData.RunWorkerAsync();
                    isInitialized = true;
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

        private void makeProxy_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                Thread.Sleep(5000);
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

        private void makeProxy_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);
                _proxy.Subscribe(Topics.Topic_UpdateInTrade, null);
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

        #region IPublishing Members

        public void Publish(MessageData e, string topicName)
        {
            try
            {
                Object[] dataList = (System.Object[])e.EventData;
                if (topicName.Equals(Topics.Topic_UpdateInTrade))
                {
                    List<PranaMessage> messageList = new List<PranaMessage>();

                    foreach (Object obj in dataList)
                    {
                        AllocationGroup group = (AllocationGroup)obj;
                        if (group.TransactionSource != TransactionSource.CreateTransactionUI && group.TransactionSource != TransactionSource.TradeImport)
                        {
                            if (group.AssetID == (int)Prana.BusinessObjects.AppConstants.AssetCategory.FX)
                                continue;

                            foreach (AllocationOrder order in group.Orders)
                            {
                                List<TaxLot> deletedTaxlotList = new List<TaxLot>();
                                PranaMessage cacheMessage = InTradeCache.Instance.GetPranaMessage(order.ClOrderID);
                                if (cacheMessage == null)
                                    continue;
                                PranaMessage message = new PranaMessage(cacheMessage.ToString());
                                int msgType = int.Parse(message.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value);
                                string orderStatus = message.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value;
                                if (e.IsRemoveManualExecution)
                                {
                                    if (message.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_StagedOrderID))
                                    {
                                        message.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = order.Quantity.ToString();
                                        string stagedOrderId = message.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_StagedOrderID].Value;
                                        PranaMessage stagedMessage = InTradeCache.Instance.GetPranaMessage(stagedOrderId);
                                        Order stagedOrder = GetOrderFromPranaMessage(stagedMessage);
                                        double orderQty = orderStatus.Equals(FIXConstants.EXECTYPE_Cancelled) ? order.CumQty : order.Quantity;
                                        InTradeCache.Instance.UpdateMessageInCache(stagedOrderId, orderQty, stagedOrder.CumQty, true);
                                        if (!order.ClOrderID.Equals(order.ParentClOrderID))
                                        {
                                            PranaMessage parentMessage = InTradeCache.Instance.GetPranaMessage(order.ParentClOrderID);
                                            Order parentOrder = GetOrderFromPranaMessage(parentMessage);
                                            InTradeCache.Instance.UpdateMessageInCache(order.ParentClOrderID, parentOrder.Quantity, parentOrder.CumQty, false);
                                            message.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value = Convert.ToString(parentOrder.Quantity - parentOrder.CumQty);
                                        }
                                    }
                                }
                                else
                                {
                                    message.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value = order.CumQty.ToString();
                                }
                                messageList.Add(message);

                                foreach (TaxLot taxlot in group.GetAllTaxlots())
                                {
                                    if (taxlot.TaxLotState == ApplicationConstants.TaxLotState.Deleted)
                                        deletedTaxlotList.Add(taxlot);
                                }
                                foreach (TaxLot x in deletedTaxlotList)
                                {
                                    x.LotId = order.ClOrderID;
                                    // taxlot.TIF =TagDatabaseManager.GetInstance.GetTIFText(order.TIF);
                                    x.Venue = CommonDataCache.CachedDataManager.GetInstance.GetVenueText(x.VenueID);
                                    x.CounterPartyName = CommonDataCache.CachedDataManager.GetInstance.GetCounterPartyText(x.CounterPartyID);
                                    x.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(x.OrderSideTagValue);
                                    x.OrderType = TagDatabaseManager.GetInstance.GetOrderTypeText(x.OrderTypeTagValue);
                                }

                                if (_complianceConnector != null)
                                {
                                    _complianceConnector.SendInTradesToEsper(deletedTaxlotList, GetRoutingKey(msgType));
                                }
                                if (msgType != (int)OrderFields.PranaMsgTypes.ORDStaged)
                                {
                                    ExpnlServiceConnector.GetInstance().UpdateInMarketTaxlots(deletedTaxlotList);
                                    TradingRulesInMarketCache.GetInstance().addToCache(deletedTaxlotList, false);
                                }
                            }
                        }
                    }
                    ProcessInTrade(messageList, false, true);
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
        public string getReceiverUniqueName()
        {
            return "Pre-TradeManager";
        }

        #endregion

        /// <summary>
        /// Getting Pending Approval Data
        /// </summary>
        /// <returns></returns>
        internal List<PreTradeApprovalInfo> GetPendingApprovalData()
        {
            try
            {
                return PendingApprovalTradeCache.GetInstance().GetPendingApprovalData();
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
                return null;
            }
        }

        /// <summary>
        /// Getting the Pending Approval Order cache
        /// </summary>
        /// <returns></returns>
        internal Dictionary<String, PranaMessage> GetPendingApprovalOrderCache()
        {
            try
            {
                return PendingApprovalTradeCache.GetInstance().GetPendingApprovalOrderCache();
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
                return null;
            }
        }

        /// <summary>
        /// Getting the Pending Approval Order cache order id wise
        /// </summary>
        /// <returns></returns>
        internal Dictionary<String, PranaMessage> GetPendingApprovalOrderCacheOrderIdWise()
        {
            try
            {
                return PendingApprovalTradeCache.GetInstance().GetPendingApprovalOrderCacheOrderIdWise();
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
                return null;
            }
        }

        /// <summary>
        /// Returns all orders present in the pre trade cache after checking Compliance validation Timeout
        /// </summary>
        /// <returns></returns>
        internal List<PranaMessage> GetComplianceCachedErrorOrders()
        {
            List<PranaMessage> pranaMsgList = new List<PranaMessage>();
            try
            {
                Dictionary<String, PendingTradeInfo> orderDetails = PendingTradeCache.GetInstance().GetAllBaskets();

                foreach (KeyValuePair<String, PendingTradeInfo> order in orderDetails)
                {
                    foreach (KeyValuePair<String, PranaMessage> pendingOrder in orderDetails[order.Key].PendingOrderCache)
                    {
                        int diffSecond = (int)(DateTime.Now - orderDetails[order.Key].RequestTime).TotalSeconds;
                        int complianceValidationTimeout = _complianceValidationTimeout / 1000;   //To convert Seconds from Miliseconds divided by 1000.

                        if (complianceValidationTimeout <= diffSecond)
                            pranaMsgList.Add(pendingOrder.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return pranaMsgList;
        }

        /// <summary>
        /// Sending CashInfow to Basket Compliance Service
        /// </summary>
        public void SendCashInFlowToBasketComplianceService(List<CashFlowToCompliance> cashFlow)
        {
            try
            {
                if (_complianceConnector != null)
                {
                    _complianceConnector.SendCashInFlowToBasketComplianceService(cashFlow);
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
        /// Updates rule name
        /// </summary>
        /// <param name="oldRuleName">old rule name </param>
        /// <param name="newRuleName">new rule name </param>
        public void UpdateRenamedRule(String oldRuleName, String newRuleName)
        {
            try
            {
                ComplianceCacheManager.UpdateRenamedRule(oldRuleName, newRuleName);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Adds new rule
        /// </summary>
        /// <param name="addedRuleName">added rule name </param>
        public void AddRuleInCache(String addedRuleName)
        {
            try
            {
                ComplianceCacheManager.AddRuleInCache(addedRuleName);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Update Alerts
        /// </summary>
        /// <param name="alerts"></param>
        /// <param name="basketId"></param>
        public void UpdatedAlerts(List<Alert> alerts, string basketId)
        {
            try
            {
                PendingTradeCache.GetInstance().UpdateAlerts(alerts, basketId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #region IServiceOnDemandStatus Members
        public async System.Threading.Tasks.Task<bool> HealthCheck()
        {
            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;

            return true;
        }
        #endregion
    }
}