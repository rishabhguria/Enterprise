using Prana.BusinessObjects;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.FIX;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.PreTrade.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.PreTrade.CacheStore
{
    class PendingTradeCache
    {
        private Dictionary<String, PendingTradeInfo> _pendingBaskets = new Dictionary<String, PendingTradeInfo>();
        private readonly object _pendingBasketsLock = new object();
        #region SingiltonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static PendingTradeCache _pendingTradeCache = null;

        /// <summary>
        /// private cunstructor
        /// </summary>
        private PendingTradeCache()
        { }

        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        internal static PendingTradeCache GetInstance()
        {
            lock (_lock)
            {
                if (_pendingTradeCache == null)
                    _pendingTradeCache = new PendingTradeCache();
                return _pendingTradeCache;
            }
        }
        #endregion

        /// <summary>
        /// Creates a new basket or adds to the existing one
        /// </summary>
        /// <param name="basketId"></param>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <param name="pranaMessage"></param>
        /// <param name="taxlots"></param>
        internal void AddToBasket(Order order, PranaMessage pranaMessage, List<TaxLot> taxlots, bool isSimulation = false, string simID = "")
        {
            try
            {
                String basketId = String.IsNullOrWhiteSpace(order.MultiTradeId) ? order.OrderID : order.MultiTradeId;
                String basketName = String.IsNullOrWhiteSpace(order.MultiTradeName) ? order.OrderID : order.MultiTradeName;

                int nirvanaMsgType = int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value);
                taxlots.ForEach(x => x.GroupID = basketId);
                // using the basket id as key to solve the scenario : different user + same multi trade name
                //String key = basketId + userId;
                basketId = isSimulation ? simID : basketId;
                if (!_pendingBaskets.ContainsKey(basketId))
                    _pendingBaskets.Add(basketId, new PendingTradeInfo(basketName, order.ModifiedUserId, GetTradeType(nirvanaMsgType)));

                //taxlots.ForEach(x => x.LotId = string.IsNullOrWhiteSpace(order.StagedOrderID) ? string.IsNullOrWhiteSpace(order.OrderID) ? order.ClOrderID : order.OrderID : order.StagedOrderID);

                _pendingBaskets[basketId].AddOrder(order.OrderID, pranaMessage);
                _pendingBaskets[basketId].AddTaxLots(taxlots);
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
        /// Returns order baskeId.
        /// </summary>
        internal string GetBasketId(Order order)
        {
            string basketId = string.Empty;
            try
            {
               basketId = String.IsNullOrWhiteSpace(order.MultiTradeId) ? order.OrderID : order.MultiTradeId;
               
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return basketId;
        }

        /// <summary>
        /// Returns Pre trade type
        /// </summary>
        /// <param name="nirvanaMsgType"></param>
        /// <returns></returns>
        private PreTradeType GetTradeType(int nirvanaMsgType)
        {
            try
            {
                switch (nirvanaMsgType)
                {
                    case (int)OrderFields.PranaMsgTypes.ORDStaged:
                        return PreTradeType.Stage;
                    default:
                        return PreTradeType.Trade;
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return PreTradeType.Trade;
            }
        }

        /// <summary>
        /// Returns all taxlots in a basket
        /// </summary>
        /// <param name="basketid"></param>
        /// <returns></returns>
        internal List<TaxLot> GetTaxlots(String basketid)
        {
            try
            {
                if (_pendingBaskets.ContainsKey(basketid))
                    return _pendingBaskets[basketid].Taxlots;
                else
                    throw new Exception(String.Format("The order {0} was not received previously.", basketid));
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
                return null;
            }
        }

        // NOTE :  THIS MIGHT DEGRADE PERFORMANCE
        //internal string GetBasketIDFromTaxlotID(string taxlotid)
        //{
        //    try
        //    {
        //        return _pendingBaskets.Where(x => x.Value.GetTaxlots().Any(q => q.GroupID == taxlotid)).ToList()[0].Key;
        //    }
        //    catch (Exception ex)
        //    {

        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //        return null;
        //    }
        //}

        /// <summary>
        /// Store the alert for the basket
        /// </summary>
        /// <param name="basketid"></param>
        /// <param name="alert"></param>
        internal void AddAlert(string basketid, Alert alert)
        {
            try
            {
                if (_pendingBaskets.ContainsKey(basketid))
                    _pendingBaskets[basketid].AddAlert(alert);
                else
                    throw new Exception(String.Format("Alert received for an order that was not in the cache. Alert : {0}, Order : {1} ", alert.RuleName, basketid));
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
        /// Return the basket for the basket id
        /// </summary>
        /// <param name="basketid"></param>
        /// <returns></returns>
        internal PendingTradeInfo GetBasket(string basketid)
        {
            try
            {
                if (_pendingBaskets.ContainsKey(basketid))
                    return _pendingBaskets[basketid];
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Update Alerts in Pending cache
        /// </summary>
        /// <param name="alerts"></param>
        /// <param name="basketid"></param>
        /// <returns></returns>
        internal List<Alert> UpdateAlerts(List<Alert> alerts, string basketid)
        {
            try
            {
                if (_pendingBaskets.ContainsKey(basketid))
                {
                    foreach (Alert x in alerts)
                    {
                        _pendingBaskets[basketid].TriggeredAlerts.Where(d => d.AlertId == x.AlertId).ToList().ForEach(d => d.UserNotes = x.UserNotes);
                    }
                    return _pendingBaskets[basketid].TriggeredAlerts;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Remove the basket from the cache
        /// </summary>
        /// <param name="basketid"></param>
        internal void RemoveBasket(string basketid)
        {
            try
            {
                if (_pendingBaskets.ContainsKey(basketid))
                    _pendingBaskets.Remove(basketid);
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
        /// Removes and returns the order from the basket
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        internal Dictionary<String, PranaMessage> PopOrder(string orderId)
        {
            try
            {
                lock (_pendingBasketsLock)
                {

                    if (_pendingBaskets.Count > 0)
                    {
                        Dictionary<String, PranaMessage> order = new Dictionary<String, PranaMessage>();
                        String basketid = _pendingBaskets.Where(x => x.Value.PendingOrderCache.ContainsKey(orderId)).ToList()[0].Key;
                        order.Add(orderId, _pendingBaskets[basketid].PendingOrderCache[orderId]);
                        _pendingBaskets[basketid].PendingOrderCache.Remove(orderId);
                        if (_pendingBaskets[basketid].PendingOrderCache.Count == 0)
                            _pendingBaskets.Remove(basketid);
                        return order;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Return all the pending orders
        /// </summary>
        /// <returns></returns>
        internal Dictionary<string, PranaMessage> GetAllCachedMessages()
        {
            try
            {
                Dictionary<String, PranaMessage> result = new Dictionary<string, PranaMessage>(); ;
                foreach (String basketId in _pendingBaskets.Keys)
                {
                    foreach (String orderId in _pendingBaskets[basketId].PendingOrderCache.Keys)
                        result.Add(orderId, DeepCopyHelper.Clone<PranaMessage>(_pendingBaskets[basketId].PendingOrderCache[orderId]));
                }
                return result;
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

        internal Dictionary<String, PendingTradeInfo> GetAllBaskets()
        {
            return _pendingBaskets;
        }

        /// <summary>
        /// Returns the basket name for the given basket ID
        /// </summary>
        /// <param name="basketId"></param>
        /// <returns></returns>
        internal String GetBasketName(String basketId)
        {
            try
            {
                if (_pendingBaskets.ContainsKey(basketId))
                    return _pendingBaskets[basketId].MultiTradeName;

                return String.Empty;
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
        /// Inform about EOM Received
        /// </summary>
        /// <param name="basketid"></param>
        internal void EOMReceived(string basketid)
        {
            try
            {
                if (_pendingBaskets != null && _pendingBaskets.ContainsKey(basketid))
                    _pendingBaskets[basketid].EOMReceived();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Check if the EOM has been received
        /// </summary>
        /// <returns></returns>
        internal Boolean IsEOMReceived(string basketid)
        {
            try
            {
                if (_pendingBaskets != null && _pendingBaskets.ContainsKey(basketid))
                    return _pendingBaskets[basketid].IsEOMReceived();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return false;
        }

        internal string GetBasketOrderIds(string basketid)
        {
            try
            {
                if (_pendingBaskets != null && _pendingBaskets.ContainsKey(basketid))
                    return _pendingBaskets[basketid].GetClOrderId();
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
