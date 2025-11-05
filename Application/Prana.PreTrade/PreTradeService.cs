using Prana.BusinessObjects;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.DataSendingObjects;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.Compliance.EventArguments;
using Prana.BusinessObjects.FIX;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PreTrade.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;

namespace Prana.PreTrade
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class PreTradeService : IPreTradeService
    {
        #region IPreTradeService Members
        public event EventHandler<RuleCheckRecievedArguments> RuleCheckReceived;

        /// <summary>
        /// Occurs when [pending approvel request received].
        /// </summary>
        public event EventHandler<EventArgs<List<PranaMessage>>> SendPendingApprovelNotificationEvent;

        public PreTradeService()
        {
            try
            {
                PreTradeManager.GetInstance().RuleCheckReceived += PreTradeService_ruleCheckReceived;
                PreTradeManager.GetInstance().PendingApprovelNotificationRecived += PreTradeService_PendingApprovelNotificationRecived;
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
        /// Handles the PendingApprovelNotificationRecived event of the PreTradeService control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{List{PranaMessage}}"/> instance containing the event data.</param>
        private void PreTradeService_PendingApprovelNotificationRecived(object sender, EventArgs<List<PranaMessage>> e)
        {
            try
            {
                if (SendPendingApprovelNotificationEvent != null)
                {
                    SendPendingApprovelNotificationEvent(this, e);
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

        public void MakeProxy()
        {
            PreTradeManager.GetInstance().MakeProxy();
        }

        /// <summary>
        /// Validate a pre trade order
        /// </summary>
        /// <param name="pranaMessage"></param>
        public void ProcessOrder(PranaMessage pranaMessage)
        {
            try
            {
                PreTradeManager.GetInstance().ProcessOrder(pranaMessage);
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
        /// Cancel a pre trade order
        /// </summary>
        /// <param name="pranaMessage"></param>
        public void CancelPendingComplianceApprovalTrades(PranaMessage pranaMessage)
        {
            try
            {
                PreTradeManager.GetInstance().CancelPendingComplianceApprovalTrades(pranaMessage);
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
        /// Add or update Cache.
        /// </summary>
        /// <param name="CLOrderID" name="OrderStatus"></param>
        public void AddOrUpdateStatusToOrderStatusTrackCache(String CLOrderID, ComplianceOrderStatus OrderStatus)
        {
            try
            { 
                PreTradeManager.GetInstance().AddOrUpdateStatusToOrderStatusTrackCache(CLOrderID, OrderStatus);
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
        /// Fetches status from Cache.
        /// </summary>
        /// <param name="ClOrderId"></param>
        public ComplianceOrderStatus GetOrderStatusFromOrderStatusTrackCache(string ClOrderID)
        {
            try
            {
               return PreTradeManager.GetInstance().GetOrderStatusFromOrderStatusTrackCache(ClOrderID);
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
                PreTradeManager.GetInstance().AddOrUpdateAcknowledgeOrderId(clOrderId, orgClOrderId);
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
        /// Gets the Acknowledged ClOrder Id.
        /// </summary>
        /// <param name="orgClOrderId"></param>
        /// <returns></returns>
        public string GetAcknowledgedClOrderId(string orgClOrderId)
        {
            try
            {
                return PreTradeManager.GetInstance().GetAcknowledgedClOrderId(orgClOrderId);
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
        /// Freeze Unfreeze Pending Compliance Approval Trades
        /// </summary>
        /// <param name="pranaMessage"></param>
        public void FreezeUnfreezePendingComplianceApprovalTrades(PranaMessage pranaMessage)
        {
            try
            {
                PreTradeManager.GetInstance().FreezeUnfreezePendingComplianceApprovalTrades(pranaMessage);
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
        /// Replace a pre trade order
        /// </summary>
        /// <param name="pranaMessage"></param>
        public void UpdateReplaceOrderAlerts(PranaMessage pranaMessage)
        {
            try
            {
                PreTradeManager.GetInstance().UpdateReplaceOrderAlerts(pranaMessage);
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
        /// Replace multiple Pending Compliance Approval Trades
        /// </summary>
        /// <param name="orders"></param>
        public void UpdateMultipleReplaceOrderAlerts(List<OrderSingle> orders)
        {
            try
            {
                foreach (OrderSingle order in orders)
                {
                    PranaMessage pranaMessage = Fix.FixDictionary.Transformer.CreatePranaMessageThroughReflection(order);
                    PreTradeManager.GetInstance().UpdateReplaceOrderAlerts(pranaMessage);
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
        /// Inform about multi trade EOM
        /// </summary>
        /// <param name="multiTradeId"></param>
        /// <param name="userId"></param>
        /// <param name="noOfOrders"></param>
        public void InformAboutMultiTradeEOM(string multiTradeId, String userId, int noOfOrders)
        {
            try
            {
                PreTradeManager.GetInstance().MultiTradeEOMReceived(multiTradeId, userId, noOfOrders);
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
        /// Override a order
        /// </summary>
        /// <param name="isAllowed"></param>
        /// <param name="orderId"></param>
        public void OverideTrade(bool isAllowed, string orderId)
        {
            try
            {
                PreTradeManager.GetInstance().OverrideTrade(isAllowed, orderId);
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

        private void PreTradeService_ruleCheckReceived(object sender, RuleCheckRecievedArguments e)
        {
            if (RuleCheckReceived != null)
                RuleCheckReceived(this, e);
        }

        /// <summary>
        /// Returns all the pending orders
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, PranaMessage> GetAllCachedMessages()
        {
            try
            {
                return PreTradeManager.GetInstance().GetAllCachedMessages();
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
        /// Send the simulation basket to esper and wait for the alerts
        /// </summary>
        /// <param name="pranaMessages"></param>
        public SimulationResult SimulateTrades(List<OrderSingle> orderSingle, PreTradeType preTradeType, int companyUserID, bool isRealTimePositions, bool isComingFromRebalancer)
        {
            try
            {
                return PreTradeManager.GetInstance().SimulateTrades(orderSingle, preTradeType, companyUserID, isRealTimePositions, isComingFromRebalancer);
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
        /// <param name="orderQty"></param>
        /// <returns></returns>
        public List<TaxLot> GetTaxlotsForPreOrder(OrderSingle orderSingle, double orderQty)
        {
            try
            {
                return PreTradeManager.GetInstance().GetTaxlotsForPreOrder(orderSingle, orderQty);
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
        /// Send Working Qty trade to esper
        /// </summary>
        /// <param name="pranaMessage"></param>
        public void SendInTradeToEsper(List<PranaMessage> pranaMessageList, bool isStartUpData)
        {
            try
            {
                PreTradeManager.GetInstance().ProcessInTrade(pranaMessageList, isStartUpData, false);
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
        /// Returns the calculations (fields) as a data set for the given compression
        /// </summary>
        /// <param name="compression"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public DataTable GetCalculationsFromEsper(Compression compression, List<String> fields)
        {
            try
            {
                return PreTradeManager.GetInstance().GetCalculationsFromEsper(compression, fields);
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
        /// Save alerts in the Database and add to alert history grid
        /// </summary>
        /// <param name="alerts"></param>
        public void SendToNotificationManager(List<Alert> alerts)
        {
            try
            {
                PreTradeManager.GetInstance().SendToNotificationManager(alerts);
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
        /// Getting the Pending Approval Data
        /// </summary>
        /// <returns></returns>
        public List<PreTradeApprovalInfo> GetPendingApprovalData()
        {
            try
            {
                return PreTradeManager.GetInstance().GetPendingApprovalData();
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
        /// Getting the Pending Approval Data
        /// </summary>
        /// <returns></returns>
        public PreTradeActionType ApproveBlockBtnClicked(List<Alert> alerts)
        {
            try
            {
                return PreTradeManager.GetInstance().TradesApprovalReceived(alerts);
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
        /// Getting the Pending Approval Order cache
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, PranaMessage> GetPendingApprovalOrderCache()
        {
            try
            {
                return PreTradeManager.GetInstance().GetPendingApprovalOrderCache();
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
        /// Remove In stage qty from compliance as User has removed order from Blotter UI
        /// </summary>
        /// <returns></returns>
        public void HideOrderFromBlotter(List<string> listParentClOrderId)
        {
            try
            {
                PreTradeManager.GetInstance().HideOrderFromBlotter(listParentClOrderId);
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
        /// Returns all orders present in the pre trade cache after checking Compliance validation Timeout
        /// </summary>
        /// <returns></returns>
        public List<PranaMessage> GetComplianceCachedErrorOrders()
        {
            try
            {
                return PreTradeManager.GetInstance().GetComplianceCachedErrorOrders();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Sending CashInFlow to Basket Compliance Service
        /// </summary>
        public void SendCashInFlowToBasketComplianceService(List<CashFlowToCompliance> cashFlow)
        {
            try
            {
                PreTradeManager.GetInstance().SendCashInFlowToBasketComplianceService(cashFlow);
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
                PreTradeManager.GetInstance().UpdateRenamedRule(oldRuleName, newRuleName);
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
        /// Adds new rule
        /// </summary>
        /// <param name="addedRuleName">added rule name </param>
        public void AddRuleInCache(String addedRuleName)
        {
            try
            {
                PreTradeManager.GetInstance().AddRuleInCache(addedRuleName);
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
        /// Update Alerts
        /// </summary>
        /// <param name="alerts"></param>
        /// <param name="basketId"></param>
        public void UpdateAlerts(List<Alert> alerts, string basketId)
        {
            try
            {
                PreTradeManager.GetInstance().UpdatedAlerts(alerts, basketId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion

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