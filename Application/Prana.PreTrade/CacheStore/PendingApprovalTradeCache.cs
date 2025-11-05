using Prana.BusinessObjects;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.FIX;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.PreTrade.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.PreTrade.CacheStore
{
    /// <summary>
    /// 
    /// </summary>
    internal class PendingApprovalTradeCache
    {
        private Dictionary<String, PreTradeApprovalInfo> _pendingApprovalCache = new Dictionary<string, PreTradeApprovalInfo>();
        private Dictionary<String, PendingTradeInfo> _pendingBaskets = new Dictionary<String, PendingTradeInfo>();

        private static readonly Object _pendingApprovalCacheLocker = new Object();

        #region SingletonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static PendingApprovalTradeCache _pendingApprovalTradeCache = null;

        /// <summary>
        /// private cunstructor
        /// </summary>
        private PendingApprovalTradeCache()
        { }

        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        internal static PendingApprovalTradeCache GetInstance()
        {
            try
            {
                lock (_lock)
                {
                    if (_pendingApprovalTradeCache == null)
                        _pendingApprovalTradeCache = new PendingApprovalTradeCache();
                    return _pendingApprovalTradeCache;
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
                return null;
            }
        }
        #endregion

        /// <summary>
        /// Add basket to cache which is sent for approval
        /// </summary>
        /// <param name="basket"></param>
        internal void AddToCache(PendingTradeInfo basket, String basketId, Boolean isTradeFromRebalancer)
        {
            try
            {
                PreTradeApprovalInfo preTradeApprovalInfo = new PreTradeApprovalInfo(basketId, basket.UserId, basket.GetOrders(), basket.TriggeredAlerts, basket.GetPendingApprovalOrderDetails(), isTradeFromRebalancer);

                List<string> uniqueRuleName = (from c in basket.TriggeredAlerts.AsEnumerable()
                                               where !string.IsNullOrWhiteSpace(c.RuleName)
                                               select c.RuleName).Distinct().ToList();
                Dictionary<string, List<int>> rulesAllowedForOverride = CommonDataCache.ComplianceCacheManager.GetOverriddenRulePermission(uniqueRuleName);

                basket.TriggeredAlerts.ForEach(x => { x.OverrideUserId = (rulesAllowedForOverride.Keys.Contains(x.RuleName)) ? ConvertToCSV(rulesAllowedForOverride[x.RuleName]) : String.Empty; });
                basket.IsTradeFromRebalancer = isTradeFromRebalancer;
                preTradeApprovalInfo.TriggeredAlerts.Where(x => x.Blocked == true).ToList().ForEach(y => y.PreTradeActionType = PreTradeActionType.NoAction);
                preTradeApprovalInfo.TriggeredAlerts.Where(x => x.Blocked == false).ToList().ForEach(y => y.PreTradeActionType = PreTradeActionType.Allowed);

                lock (_pendingApprovalCacheLocker)
                {
                    if (!_pendingApprovalCache.ContainsKey(basketId))
                        _pendingApprovalCache.Add(basketId, preTradeApprovalInfo);
                    _pendingBaskets.Add(basketId, basket);
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
        /// Convert List to CSV string
        /// </summary>
        /// <param name="list">List</param>
        /// <returns>CSV String</returns>
        private string ConvertToCSV(List<int> list)
        {
            try
            {
                string csvUserId = string.Join(",", list);
                return csvUserId;
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
        /// Returns basket on the basis of basket id
        /// </summary>
        /// <param name="basketId"></param>
        /// <returns></returns>
        internal PreTradeApprovalInfo GetBasketForApproval(string basketId)
        {
            try
            {
                lock (_pendingApprovalCacheLocker)
                {
                    if (_pendingApprovalCache.ContainsKey(basketId))
                        return _pendingApprovalCache[basketId];
                    else
                        return null;
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
                return null;
            }
        }

        /// <summary>
        /// Update alert status in cache
        /// </summary>
        /// <param name="alerts"></param>
        /// <returns></returns>
        internal PreTradeActionType UpdateAlerts(List<Alert> alerts)
        {
            try
            {
                lock (_pendingApprovalCacheLocker)
                {
                    if (alerts[0].PreTradeActionType == PreTradeActionType.Blocked)
                    {
                        foreach (PreTradeApprovalInfo pTAI in _pendingApprovalCache.Values)
                        {
                            foreach (Alert alert in pTAI.TriggeredAlerts)
                            {
                                if (alert.OrderId.Equals(alerts[0].OrderId))
                                {
                                    if (alerts.Contains(alert))
                                    {
                                        //Update the preTradeActionType, OverrideUserId, ActionUser and ActionUserName in Cache
                                        alert.ComplianceOfficerNotes = alerts[0].ComplianceOfficerNotes;
                                    }
                                }
                            }
                        }
                        return PreTradeActionType.Blocked;
                    }
                    else if (alerts[0].PreTradeActionType == PreTradeActionType.Allowed)
                    {
                        //  _pendingApprovalCache[alerts[0].OrderId].TriggeredAlerts.Where(x => x.Equals(alerts)).ToList().ForEach(y => y.PreTradeActionType = PreTradeActionType.Allowed);
                        foreach (PreTradeApprovalInfo pTAI in _pendingApprovalCache.Values)
                        {
                            foreach (Alert alert in pTAI.TriggeredAlerts)
                            {
                                if (alert.OrderId.Equals(alerts[0].OrderId))
                                {
                                    if (alerts.Contains(alert))
                                    {
                                        //Update the preTradeActionType, OverrideUserId, ActionUser and ActionUserName in Cache
                                        alert.PreTradeActionType = PreTradeActionType.Allowed;
                                        alert.OverrideUserId = alerts[0].OverrideUserId;
                                        alert.ActionUser = alerts[0].ActionUser;
                                        alert.ActionUserName = alerts[0].ActionUserName;
                                        alert.ComplianceOfficerNotes = alerts[0].ComplianceOfficerNotes;
                                    }
                                }
                            }
                        }
                        bool allAllowed = (from q in _pendingApprovalCache[alerts[0].OrderId].TriggeredAlerts
                                           where q.PreTradeActionType == PreTradeActionType.NoAction
                                           select q).Count() == 0;
                        if (allAllowed)
                            return PreTradeActionType.Allowed;
                    }
                    return PreTradeActionType.NoAction;
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
                return PreTradeActionType.NoAction;
            }
        }

        /// <summary>
        /// Returns Pending Trade Info for processing after response from client
        /// </summary>
        /// <param name="basketId"></param>
        /// <returns></returns>
        internal PendingTradeInfo GetPendingBasket(string basketId)
        {
            try
            {
                lock (_pendingApprovalCacheLocker)
                {
                    if (_pendingBaskets.ContainsKey(basketId))
                        return _pendingBaskets[basketId];
                    else
                        return null;
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
                return null;
            }
        }

        /// <summary>
        /// Getting Pending Approval Data
        /// </summary>
        /// <returns></returns>
        internal List<PreTradeApprovalInfo> GetPendingApprovalData()
        {
            try
            {
                List<PreTradeApprovalInfo> result = new List<PreTradeApprovalInfo>();
                lock (_pendingApprovalCacheLocker)
                {
                    foreach (string key in _pendingApprovalCache.Keys)
                    {
                        result.Add(_pendingApprovalCache[key]);
                    }
                    return result;
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
                return null;
            }
        }

        /// <summary>
        /// Remove Alerts from Cache
        /// </summary>
        /// <param name="p"></param>
        internal void RemoveFromCache(string basketId)
        {
            try
            {
                lock (_pendingApprovalCacheLocker)
                {
                    if (_pendingApprovalCache.ContainsKey(basketId))
                        _pendingApprovalCache.Remove(basketId);
                    if (_pendingBaskets.ContainsKey(basketId))
                        _pendingBaskets.Remove(basketId);
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
        /// Pop order for Pending Approval Trades
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        internal Dictionary<string, PranaMessage> PopOrder(string orderId)
        {
            try
            {
                lock (_pendingApprovalCacheLocker)
                {
                    if (_pendingApprovalCache.Count > 0 && _pendingBaskets.Count > 0)
                    {
                        String basketid = _pendingBaskets.Where(x => x.Value.PendingOrderCache.ContainsKey(orderId)).ToList()[0].Key;
                        Dictionary<string, PranaMessage> order = new Dictionary<String, PranaMessage>();

                        order.Add(orderId, _pendingBaskets[basketid].PendingOrderCache[orderId]);
                        _pendingBaskets[basketid].PendingOrderCache.Remove(orderId);
                        if (_pendingBaskets[basketid].PendingOrderCache.Count == 0)
                            _pendingBaskets.Remove(basketid);

                        _pendingApprovalCache[basketid].PendingOrderCache.Remove(order[orderId]);
                        if (_pendingApprovalCache[basketid].PendingOrderCache.Count == 0)
                            _pendingApprovalCache.Remove(basketid);

                        return order;
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
            return null;
        }

        /// <summary>
        /// Getting the Pending Approval Order cache
        /// </summary>
        /// <returns></returns>
        internal Dictionary<String, PranaMessage> GetPendingApprovalOrderCache()
        {
            try
            {
                lock (_pendingApprovalCacheLocker)
                {
                    Dictionary<String, PranaMessage> result = new Dictionary<string, PranaMessage>();
                    foreach (String basketId in _pendingBaskets.Keys)
                    {
                        foreach (String orderId in _pendingBaskets[basketId].PendingOrderCache.Keys)
                            result.Add(orderId, DeepCopyHelper.Clone<PranaMessage>(_pendingBaskets[basketId].PendingOrderCache[orderId]));
                    }
                    return result;
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
                return null;
            }
        }

        /// <summary>
        /// Getting the Pending Approval Order cache order id wise
        /// </summary>
        /// <returns></returns>
        internal Dictionary<String, PranaMessage> GetPendingApprovalOrderCacheOrderIdWise()
        {
            Dictionary<String, PranaMessage> result = new Dictionary<string, PranaMessage>();
            try
            {
                lock (_pendingApprovalCacheLocker)
                {
                    if (_pendingBaskets != null)
                    {
                        foreach (String basketId in _pendingBaskets.Keys)
                        {
                            foreach (KeyValuePair<string, PranaMessage> kvpPranaMessage in _pendingBaskets[basketId].PendingOrderCache)
                            {
                                if (kvpPranaMessage.Value != null && kvpPranaMessage.Value.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagClOrdID))
                                {
                                    string orderId = kvpPranaMessage.Value.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                                    if (orderId != null && !result.ContainsKey(orderId))
                                    {
                                        result.Add(orderId, DeepCopyHelper.Clone<PranaMessage>(kvpPranaMessage.Value));
                                    }
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// To get triggered Alerts Data
        /// </summary>
        /// <returns></returns>
        internal List<Alert> GetTriggerAlerts(string orderId, string clOrderid = null)
        {
            try
            {
                List<Alert> alerts = new List<Alert>();
                IEnumerable<KeyValuePair<String, PendingTradeInfo>> matchedOrders;
                lock (_pendingApprovalCacheLocker)
                {
                    if (_pendingApprovalCache.Count > 0)
                    {
                        string basketId = string.Empty;
                        if (clOrderid != null)
                            matchedOrders = _pendingBaskets.Where(x => x.Value.PendingOrderCache.ContainsKey(orderId) && (x.Value.PendingOrderCache[orderId].FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value.ToString() == clOrderid));
                        else
                            matchedOrders = _pendingBaskets.Where(x => x.Value.PendingOrderCache.ContainsKey(orderId));
                        if (matchedOrders != null && matchedOrders.Count() > 0)
                        {
                            foreach (var val in matchedOrders)
                            {
                                basketId = val.Key;
                                foreach (string key in _pendingApprovalCache.Keys)
                                {
                                    if ((!String.IsNullOrEmpty(basketId)))
                                    {
                                        if (key.Equals(basketId))
                                            alerts.AddRange(_pendingApprovalCache[key].TriggeredAlerts);
                                    }
                                }
                            }
                        }
                        else
                        {
                            basketId = orderId;
                            foreach (string key in _pendingApprovalCache.Keys)
                            {
                                if ((!String.IsNullOrEmpty(basketId)))
                                {
                                    if (key.Equals(basketId))
                                        alerts.AddRange(_pendingApprovalCache[key].TriggeredAlerts);
                                }
                            }
                        }
                        return alerts;
                    }
                    else
                    {
                        return null;
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
                return null;
            }
        }

        /// <summary>
        /// Returns the no of remaining alerts to approve
        /// </summary>
        /// <returns></returns>
        internal int GetRemainingAlertsCount(string orderId)
        {
            try
            {
                int a = 0;
                lock (_pendingApprovalCacheLocker)
                {
                    if (_pendingApprovalCache.Count > 0)
                    {
                        string basketId = string.Empty;
                        IEnumerable<KeyValuePair<String, PendingTradeInfo>> matchedOrders = _pendingBaskets.Where(x => x.Value.PendingOrderCache.ContainsKey(orderId));
                        if (matchedOrders != null && matchedOrders.Count() > 0)
                        {
                            foreach (var val in matchedOrders)
                            {
                                basketId = val.Key;
                            }
                        }
                        else
                            basketId = orderId;

                        foreach (string key in _pendingApprovalCache.Keys)
                        {
                            if ((!String.IsNullOrEmpty(basketId)))
                            {
                                if (key.Equals(basketId))
                                    a = _pendingApprovalCache[key].TriggeredAlerts.Where(x => !x.PreTradeActionType.Equals(PreTradeActionType.Allowed)).Count();
                            }
                        }
                        return a;
                    }
                    else
                    {
                        return 0;
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
                return 0;
            }
        }
    }
}
