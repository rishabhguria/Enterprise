using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Enums;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.PreTrade.Connectors
{
    /// <summary>
    /// This class send the alerts to the notification manager
    /// </summary>
    internal class NotificationConnector
    {
        /// <summary>
        /// Output queue on which _notification will be sent 
        /// </summary>
        String _notificationExchange = String.Empty;

        /// <summary>
        /// TO check if trades coming are from rebalancer
        /// </summary>
        private Boolean _isTradeFromRebalancer = false;
        public Boolean IsTradeFromRebalancer
        {
            get { return _isTradeFromRebalancer; }
            set { _isTradeFromRebalancer = value; }
        }

        internal NotificationConnector()
        {
            try
            {
                _notificationExchange = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_NotificationExchange);
                AmqpHelper.InitializeSender("Notification", _notificationExchange, MediaType.Exchange_Direct);
                AmqpHelper.InitializeSender("Approval", _notificationExchange, MediaType.Exchange_Direct);
                AmqpHelper.InitializeSender("ApprovalResponse", _notificationExchange, MediaType.Exchange_Direct);
                AmqpHelper.InitializeSender("PendingApprovalRowFroze", _notificationExchange, MediaType.Exchange_Direct);
                AmqpHelper.InitializeSender("PendingApprovalRowUnfroze", _notificationExchange, MediaType.Exchange_Direct);
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
        /// send notification object Notification manager
        /// </summary>
        /// <param name="obj"></param>
        internal void SendAlertsToNotificationManager(List<Alert> alerts,string replaceAlertType = null)
        {
            try
            {
                if (IsTradeFromRebalancer)
                {
                    var alertInfo = new { alertInfo = Alert.GetCombinedAlertsDataSet(alerts) };
                    AmqpHelper.SendObject(alertInfo, "Notification", "All");
                    IsTradeFromRebalancer = false;
                }
                else
                {
                    foreach (Alert alert in alerts)
                    {
                        DataSet dataSet = Alert.GetAlertDataSet(alert);
                        if(replaceAlertType != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            DataTable dtAlertType = dataSet.Tables[0];
                            dtAlertType.Columns.Add("ReplaceAlertType", typeof(string));
                            dtAlertType.Rows[0]["ReplaceAlertType"] = replaceAlertType;   
                        }
                        AmqpHelper.SendObject(dataSet, "Notification", "All");
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
        /// Sends alerts to notificationManager for Froze/Unfroze
        /// </summary>
        /// <param name="alerts"></param>
        /// <param name="routingKey"></param>
        internal void SendAlertsToNotificationManagerForFrozeUnfroze(List<Alert> alerts, string routingKey)
        {
            try
            {
                foreach (Alert alert in alerts)
                    AmqpHelper.SendObject(Alert.GetAlertDataSet(alert), routingKey, routingKey);
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
        /// Send Alerts for Approval
        /// </summary>
        /// <param name="basket"></param>
        internal void SendAlertsToForApproval(PreTradeApprovalInfo basket)
        {
            try
            {
                var preTradeApprovalInfo = new { preTradeApprovalInfo = basket };

                AmqpHelper.SendObject(preTradeApprovalInfo, "Approval", "SendApprovalRequest");
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
        /// Send Pending Approval Response to handle Multi user
        /// </summary>
        /// <param name="basket"></param>
        internal void SendAlertsApprovalResponse(List<Alert> alerts, string basketId, PreTradeActionType preTradeActionType, int actionUser)
        {
            try
            {
                Object a = new { preTradeActionType1 = preTradeActionType.ToString(), basketId1 = basketId, actionUser1 = actionUser, alert = alerts };
                Object ojbToSend = new { rootobj = a };
                AmqpHelper.SendObject(ojbToSend, "ApprovalResponse", "SendApprovalResponse");
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
    }
}
