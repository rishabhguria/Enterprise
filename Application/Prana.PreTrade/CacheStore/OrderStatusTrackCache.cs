using System;
using System.Collections.Generic;
using Prana.LogManager;
using Prana.BusinessObjects.Compliance.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.PreTrade.CacheStore
{
    /// <summary>
    /// To store the Order Status for compliance Orders
    /// Defect 16788: Broker acknowledged the order and then user replace the same and it goes to CO for approval and then user cancel it from blotter
    /// </summary>
    internal class OrderStatusTrackCache
    {
        private Dictionary<String, ComplianceOrderStatus> _orderStatusCache = new Dictionary<String, ComplianceOrderStatus>();

        /// <summary>
        /// Stores the ClOrderId that have been acknowledged.
        /// </summary>
        private Dictionary<String,String> _acknowledgedClOrderId = new Dictionary<String,String>();

        #region SingiltonInstance
        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static OrderStatusTrackCache _orderStatusTrackCache = null;

        /// <summary>
        /// private cunstructor
        /// </summary>
        private OrderStatusTrackCache()
        { }

        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        internal static OrderStatusTrackCache GetInstance()
        {
            lock (_lock)
            {
                if (_orderStatusTrackCache == null)
                    _orderStatusTrackCache = new OrderStatusTrackCache();
                return _orderStatusTrackCache;
            }
        }
        #endregion
        /// <summary>
        /// Add or update Cache.
        /// </summary>
        /// <param name="ClOrderID" name="orderStatus"></param>
        public void AddOrUpdateStatusToOrderStatusTrackCache(string ClOrderID,ComplianceOrderStatus OrderStatus)
        {
            try
            {
                if (_orderStatusCache.ContainsKey(ClOrderID))
                    _orderStatusCache[ClOrderID] = OrderStatus;
                else
                    _orderStatusCache.Add(ClOrderID, OrderStatus);
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
        /// Fetches status from Cache.
        /// </summary>
        /// <param name="ClOrderId"></param>
        public ComplianceOrderStatus GetOrderStatusFromOrderStatusTrackCache(string ClOrderId)
        {
            try
            {
                if (_orderStatusCache.ContainsKey(ClOrderId))
                    return _orderStatusCache[ClOrderId];
                else
                    return ComplianceOrderStatus.None;
            }
            catch (Exception ex)
            {
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
        public void AddOrUpdateAcknowledgeOrderId(string clOrderId,string orgClOrderId)
        {
            try
            {
                if (_acknowledgedClOrderId.ContainsKey(orgClOrderId))
                    _acknowledgedClOrderId[orgClOrderId] = clOrderId;
                else
                    _acknowledgedClOrderId.Add(orgClOrderId, clOrderId);
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
        /// Gets the Acknowledged ClOrder Id.
        /// </summary>
        /// <param name="orgClOrderId"></param>
        /// <returns></returns>
        public string GetAcknowledgedClOrderId(string orgClOrderId)
        {
            try
            {
                if (_acknowledgedClOrderId.ContainsKey(orgClOrderId))
                    return _acknowledgedClOrderId[orgClOrderId];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

    }
}
