using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.FIX;
using Prana.CommonDataCache;
using Prana.Fix.FixDictionary;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.PreTrade.Classes
{
    /// <summary>
    /// This class encapsulates a logical basket.
    /// The basket may be a set of trades from a single multi trade ticket. Or even an individual trade.
    /// </summary>
    internal class PendingTradeInfo
    {
        #region Private Variables
        private String _multiTradeName;
        private int _userId;
        private DateTime _requestTime;
        private Dictionary<String, PranaMessage> _pendingOrderCache;
        private List<TaxLot> _pendingTaxlotCache;
        private List<Alert> _triggeredAlerts;
        private PreTradeType _tradeType;
        private Boolean _isEOMReceived = false;
        private Boolean _isTradeFromRebalancer = false;
        #endregion


        #region Property Definition

        public Boolean IsTradeFromRebalancer
        {
            get { return _isTradeFromRebalancer; }
            set { _isTradeFromRebalancer = value; }
        }

        public PreTradeType TradeType { get { return _tradeType; } }
        /// <summary>
        /// Contains the multitrade name or the order id incase of a singe trade
        /// </summary>
        public String MultiTradeName
        {
            get { return _multiTradeName; }
        }

        /// <summary>
        /// The userid for the orders
        /// </summary>
        public int UserId
        {
            get { return _userId; }
        }

        /// <summary>
        /// Incoming time of the basket
        /// </summary>
        public DateTime RequestTime
        {
            get { return _requestTime; }
        }

        /// <summary>
        /// Contains the grouped orders [orderid-prana message]
        /// </summary>
        public Dictionary<String, PranaMessage> PendingOrderCache
        {
            get { return _pendingOrderCache; }
        }

        /// <summary>
        /// Contains the individual taxlots in the grouped orders
        /// </summary>
        public List<TaxLot> Taxlots
        {
            get { return _pendingTaxlotCache; }
        }

        /// <summary>
        /// Alerts triggered for this basket
        /// </summary>
        public List<Alert> TriggeredAlerts
        {
            get { return _triggeredAlerts; }
            set { _triggeredAlerts = value; }
        }

        #endregion

        /// <summary>
        /// Create a new Trade Information Object (Basket)
        /// </summary>
        /// <param name="basketName"></param>
        /// <param name="userid"></param>
        internal PendingTradeInfo(String basketName, int userid, PreTradeType type)
        {
            this._userId = userid;
            this._multiTradeName = basketName;
            _pendingOrderCache = new Dictionary<String, PranaMessage>();
            _pendingTaxlotCache = new List<TaxLot>();
            _triggeredAlerts = new List<Alert>();
            this._requestTime = DateTime.Now;
            this._tradeType = type;
        }

        /// <summary>
        /// Add taxlots to the basket
        /// </summary>
        /// <param name="taxlots"></param>
        internal void AddTaxLots(List<TaxLot> taxlots)
        {
            try
            {
                _pendingTaxlotCache.AddRange(taxlots);
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
        /// Add order (Prana Message) to the basket
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="pranaMessage"></param>
        internal void AddOrder(String orderId, PranaMessage pranaMessage)
        {
            try
            {
                if (_pendingOrderCache.ContainsKey(orderId))
                    _pendingOrderCache[orderId] = pranaMessage;
                else
                    _pendingOrderCache.Add(orderId, pranaMessage);
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
        /// Add an alert to the basket
        /// </summary>
        /// <param name="alert"></param>
        internal void AddAlert(Alert alert)
        {
            try
            {
                alert.PreTradeType = this.TradeType;
                _triggeredAlerts.Add(alert);
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
        /// Inform about EOM received
        /// </summary>
        internal void EOMReceived()
        {
            _isEOMReceived = true;
        }

        /// <summary>
        /// Check if the EOM has been received
        /// </summary>
        /// <returns></returns>
        internal Boolean IsEOMReceived()
        {
            return _isEOMReceived;
        }

        /// <summary>
        /// Converts prana messages in basket to orders
        /// </summary>
        /// <returns></returns>
        internal List<PranaMessage> GetOrders()
        {
            try
            {
                List<PranaMessage> orderList = new List<PranaMessage>();
                orderList.AddRange(_pendingOrderCache.Select(x => (x.Value)).ToList());
                return orderList;
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        internal List<PendingApprovalOrderDetails> GetPendingApprovalOrderDetails()
        {
            try
            {
                List<Order> orderList = new List<Order>();
                List<PendingApprovalOrderDetails> pendingApprovalOrderDetails = new List<PendingApprovalOrderDetails>();
                orderList.AddRange(_pendingOrderCache.Select(x => Transformer.CreateOrder(x.Value)).ToList());
                foreach (Order order in orderList)
                {
                    PendingApprovalOrderDetails orderDetails = new PendingApprovalOrderDetails();
                    if (order.CompanyUserID != 0)
                        orderDetails.UserName = CachedDataManager.GetInstance.GetUserText(order.CompanyUserID);
                    if (string.IsNullOrEmpty(orderDetails.UserName))
                        orderDetails.UserName = "N/A";

                    orderDetails.ClOrderID = order.ClOrderID;
                    orderDetails.Broker = order.CounterPartyName;
                    orderDetails.ExpirationDate = order.ExpirationDate.ToString();
                    orderDetails.AccountName = CachedDataManager.GetInstance.GetAccountText(order.Level1ID);
                    orderDetails.Notional = order.NotionalValue;
                    orderDetails.Quantity = order.Quantity;
                    orderDetails.Symbol = order.Symbol;
                    orderDetails.UnderlyingSymbol = order.UnderlyingSymbol;
                    orderDetails.Trader = CachedDataManager.GetInstance.GetTradingAccountText(order.TradingAccountID);
                    orderDetails.TradePrice = order.AvgPrice;
                    orderDetails.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(order.OrderSideTagValue);
                    orderDetails.TradeNotes = order.InternalComments;
                    orderDetails.TradeDate = order.ProcessDate.ToString("MM/dd/yyyy HH:mm:ss");
                    if (order.OrderTypeTagValue == FIXConstants.ORDTYPE_Market && orderDetails.TradePrice == 0) // In case of Live order
                        orderDetails.TradePrice = order.AvgPriceForCompliance;
                    orderDetails.Notional = orderDetails.TradePrice * orderDetails.Quantity;

                    /*   bool isStageValueFromField = ComplianceCacheManager.GetStageValueFromField();
                       if (isStageValueFromField && order.OrderTypeTagValue.Equals("1"))
                       {
                           string fromFieldString = "_" + ComplianceCacheManager.GetStageValueFromFieldString().ToLower();
                           var taxlotFields = order.GetType().BaseType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                           var taxlotField = taxlotFields.SingleOrDefault(a => a.Name.ToLower().Contains(fromFieldString));
                           if (taxlotField != null)
                           {
                               double _txtDoubleValue;
                               if (Double.TryParse((taxlotField.GetValue(order)).ToString(), out _txtDoubleValue))
                               {
                                   orderDetails.TradePrice = _txtDoubleValue;
                                   orderDetails.Notional = _txtDoubleValue * orderDetails.Quantity;
                               }
                           }
                       }*/

                    if (order.MultiTradeId != null && orderDetails.TradeOrigination.Equals(TransactionSource.TradingTicket))
                    {
                        orderDetails.TradeOrigination = TransactionSource.MultiTradingTicket;
                    }
                    else if (order.MultiTradeId == null && orderDetails.TradeOrigination.Equals(TransactionSource.TradingTicket))
                    {
                        orderDetails.TradeOrigination = TransactionSource.SingleTrade;
                    }

                    if (!string.IsNullOrEmpty(order.AccountBrokerMapping))
                    {
                        Dictionary<int, int> accountBrokerMapping = JsonHelper.DeserializeToObject<Dictionary<int, int>>(order.AccountBrokerMapping);
                        int brokerID = accountBrokerMapping.First().Value;
                        bool isMappedAccountHaveSameBroker = true;
                        foreach (KeyValuePair<int, int> kvp in accountBrokerMapping)
                        {
                            isMappedAccountHaveSameBroker = isMappedAccountHaveSameBroker && (kvp.Value == brokerID);
                        }
                        orderDetails.Broker = isMappedAccountHaveSameBroker ? CommonDataCache.CachedDataManager.GetInstance.GetCounterPartyText(brokerID) : "N/A";
                        orderDetails.TradeOrigination = order.MultiTradeName != null ? TransactionSource.MultiTradingTicket : TransactionSource.SingleTrade;
                    }

                    string currencyText = string.Empty;
                    if (order.CurrencyID != int.MinValue)
                        currencyText = CachedDataManager.GetInstance.GetCurrencyText(order.CurrencyID);

                    orderDetails.TradeDetails = TagDatabaseManager.GetInstance.GetOrderSideText(order.OrderSideTagValue) + " " + String.Format("{0:n0}", order.Quantity) + " " + order.Symbol + " @" + orderDetails.TradePrice + ", Notional " + currencyText + " " + String.Format("{0:n0}", orderDetails.Notional);

                    if (!pendingApprovalOrderDetails.Contains(orderDetails))
                        pendingApprovalOrderDetails.Add(orderDetails);
                }
                return pendingApprovalOrderDetails;
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Returns Clorderid of basket.
        /// </summary>
        /// <returns></returns>
        internal string GetClOrderId()
        {
            try
            {
                if (_pendingOrderCache != null && _pendingOrderCache.Count > 0)
                    return String.Join(", ", _pendingOrderCache.Values.Where(x => x.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ParentClOrderID)).Select(x => x.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ParentClOrderID].Value));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return String.Empty;
        }

        /// <summary>
        /// Returns ClienOrderid of basket.
        /// </summary>
        /// <returns></returns>
        internal string GetClientOrderId()
        {
            try
            {
                if (_pendingOrderCache != null && _pendingOrderCache.Count > 0)
                    return String.Join(", ", _pendingOrderCache.Values.Where(x => x.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ClientOrderID)).Select(x => x.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ClientOrderID].Value));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return String.Empty;
        }
    }
}
