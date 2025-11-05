using Prana.BusinessObjects.FIX;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects.Compliance.Alerting
{
    [Serializable]
    public class PreTradeApprovalInfo
    {

        #region Private Variables

        /// <summary>
        ///Multi Trade Name 
        /// </summary>
        private String _multiTradeName = String.Empty;

        /// <summary>
        /// User Id
        /// </summary>
        private int _userId = int.MinValue;

        /// <summary>
        /// Request Time
        /// </summary>
        private DateTime _requestTime = DateTime.MinValue;

        /// <summary>
        /// List of Pending order cache
        /// </summary>
        private List<PranaMessage> _pendingOrderCache = new List<PranaMessage>();

        /// <summary>
        /// List of Triggered Alert
        /// </summary>
        private List<Alert> _triggeredAlerts = new List<Alert>();

        /// <summary>
        /// Pending Approval Order Details
        /// </summary>
        List<PendingApprovalOrderDetails> _pendingOrderDetails = new List<PendingApprovalOrderDetails>();

        /// <summary>
        /// to check if the trading source is rebalancer or not
        /// </summary>
        private Boolean _isTradeFromRebalancer = false;

        #endregion


        #region Property Definition


        /// <summary>
        /// Contains the multitrade name or the order id incase of a singe trade
        /// </summary>
        public String MultiTradeName
        {
            get { return _multiTradeName; }
            set { _multiTradeName = value; }
        }

        /// <summary>
        /// The userid for the orders
        /// </summary>
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        /// <summary>
        /// Incoming time of the basket
        /// </summary>
        public DateTime RequestTime
        {
            get { return _requestTime; }
            set { _requestTime = value; }
        }

        /// <summary>
        /// Contains the grouped orders [orderid-prana message]
        /// </summary>
        public List<PranaMessage> PendingOrderCache
        {
            get { return _pendingOrderCache; }
            set { _pendingOrderCache = value; }
        }


        /// <summary>
        /// Alerts triggered for this basket
        /// </summary>
        public List<Alert> TriggeredAlerts
        {
            get { return _triggeredAlerts; }
            set { _triggeredAlerts = value; }
        }

        /// <summary>
        /// Pending Approval Order Details
        /// </summary>
        public List<PendingApprovalOrderDetails> PendingOrderDetails
        {
            get { return _pendingOrderDetails; }
            set { _pendingOrderDetails = value; }
        }

        /// <summary>
        /// To check the trading source is rebalancer or not.
        /// </summary>
        public Boolean IsTradeFromRebalancer
        {
            get { return _isTradeFromRebalancer; }
            set { _isTradeFromRebalancer = value; }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PreTradeApprovalInfo()
        {
            this._multiTradeName = String.Empty;
            this._requestTime = DateTime.MinValue;
            this._userId = int.MinValue;
            this._triggeredAlerts = new List<Alert>();
            this._pendingOrderCache = new List<PranaMessage>();
            this._pendingOrderDetails = new List<PendingApprovalOrderDetails>();
            this._isTradeFromRebalancer = false;
        }

        /// <summary>
        /// Constructor to set basketName, user Id, Orders, trigger alert
        /// </summary>
        /// <param name="basketName">basketName</param>
        /// <param name="userid">userid</param>
        /// <param name="orders">orders</param>
        /// <param name="triggeredAlerts">triggeredAlerts</param>
        public PreTradeApprovalInfo(String basketName, int userid, List<PranaMessage> orders, List<Alert> triggeredAlerts, List<PendingApprovalOrderDetails> pendingOrderDetails, Boolean isTradeFromRebalancer)
        {
            try
            {
                this._userId = userid;
                this._multiTradeName = basketName;
                _pendingOrderCache = new List<PranaMessage>(orders);
                _triggeredAlerts = new List<Alert>(triggeredAlerts);
                this._requestTime = DateTime.Now;
                this._pendingOrderDetails = new List<PendingApprovalOrderDetails>(pendingOrderDetails);
                this._isTradeFromRebalancer = isTradeFromRebalancer;
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
    }
}
