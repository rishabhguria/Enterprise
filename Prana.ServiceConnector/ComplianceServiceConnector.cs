using Prana.BusinessObjects;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.DataSendingObjects;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.ServiceConnector
{
    public class ComplianceServiceConnector : IDisposable
    {
        /**
         * TODO : This is a common service helper
         * that is present in two projects (work area and TT)
         * Other than this other things need to be moved too
         * A common prokect known as compliance services. That calls all other services related tp compliance 
         * 
         * */

        /// <summary>
        /// proxy for the allocation service, used for pre allocation before sending order to compliance
        /// </summary>
        private ProxyBase<IPreTradeService> _preTradeService = null;

        #region SingletonInstance
        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static ComplianceServiceConnector _complianceServiceConnector = null;

        /// <summary>
        /// private constructor, Initialises the proxy
        /// </summary>
        private ComplianceServiceConnector()
        {
            try
            {
                _preTradeService = new ProxyBase<IPreTradeService>("TradePreTradeComplianceServiceEndpointAddress");
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
        /// Singleton instance
        /// </summary>
        /// <returns></returns>
        public static ComplianceServiceConnector GetInstance()
        {
            try
            {
                lock (_lock)
                {
                    if (_complianceServiceConnector == null)
                        _complianceServiceConnector = new ComplianceServiceConnector();
                    return _complianceServiceConnector;
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
        #endregion

        /// <summary>
        /// Wrapper for compliance service to check the trades
        /// </summary>
        /// <param name="pranaMessage"></param>
        public SimulationResult SimulateTrade(List<OrderSingle> orderSingle, PreTradeType preTradeType, int companyUserID, bool isRealTimePositions, bool isComingFromRebalancer)
        {
            try
            {
                var item = orderSingle.SingleOrDefault(order => order.Symbol.Equals("Cash"));
                if (item != null)
                    orderSingle.Remove(item);
                return _preTradeService.InnerChannel.SimulateTrades(orderSingle, preTradeType, companyUserID, isRealTimePositions, isComingFromRebalancer);
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
        /// Wrapper for compliance service to check the trades
        /// </summary>
        /// <param name="pranaMessage"></param>
        public List<TaxLot> GetTaxlotsForPreOrder(OrderSingle orderSingle, double orderQty)
        {
            try
            {
                return _preTradeService.InnerChannel.GetTaxlotsForPreOrder(orderSingle, orderQty);
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
        /// Wrapper for compliance service to get calculations
        /// </summary>
        /// <param name="compression"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public DataTable GetCalculations(Compression compression, List<String> fields)
        {
            try
            {
                return _preTradeService.InnerChannel.GetCalculationsFromEsper(compression, fields);
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
        /// Service wrapper to save the alerts to DB
        /// </summary>
        /// <param name="alerts"></param>
        public void SaveAlertsToHistory(List<Alert> alerts)
        {
            try
            {
                _preTradeService.InnerChannel.SendToNotificationManager(alerts);
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
        /// Remove In stage qty from compliance as User has removed order from Blotter UI
        /// </summary>
        /// <param name="parentClOrderIds"></param>
        public void HideOrderFromBlotter(List<string> listParentClOrderId)
        {
            try
            {
                _preTradeService.InnerChannel.HideOrderFromBlotter(listParentClOrderId);
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
        /// Sending CashInFlow to Basket Compliance Service
        /// </summary>
        /// <param name="cashFlow"></param>
        public void SendCashInFlowToBasketComplianceService(List<CashFlowToCompliance> cashFlow)
        {
            try
            {
                _preTradeService.InnerChannel.SendCashInFlowToBasketComplianceService(cashFlow);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Wrapper for compliance service for UpdateMultipleReplaceOrderAlerts
        /// </summary>
        /// <param name="orders"></param>
        public void UpdateMultipleReplaceOrderAlerts(List<OrderSingle> orders)
        {
            try
            {
                _preTradeService.InnerChannel.UpdateMultipleReplaceOrderAlerts(orders);
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

        #region IDisposable
        /// <summary>
        /// Dispose() calls Dispose(true)
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
                    if (_preTradeService != null)
                        _preTradeService.Dispose();
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
        /// Updates rule name
        /// </summary>
        /// <param name="oldRuleName">old rule name </param>
        /// <param name="newRuleName">new rule name </param>
        public void UpdateRenamedRule(string oldRuleName, string newRuleName)
        {
            try
            {
                _preTradeService.InnerChannel.UpdateRenamedRule(oldRuleName, newRuleName);
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
        /// Adds new rule
        /// </summary>
        /// <param name="addedRuleName">added rule name </param>
        public void AddRuleInCache(string addedRuleName)
        {
            try
            {
                _preTradeService.InnerChannel.AddRuleInCache(addedRuleName);
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
        /// Send updated alerts to cache in trade server
        /// </summary>
        /// <param name="alerts"></param>
        /// <param name="basketId"></param>
        public void UpdateAlerts(List<Alert> alerts, string basketId)
        {
            try
            {
                _preTradeService.InnerChannel.UpdateAlerts(alerts, basketId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        #endregion
    }
}