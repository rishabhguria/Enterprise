using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.EventArguments;
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
    /// This class does the following tasks :
    /// 1. Send override request to the client
    /// 2. Get the override responce from the client
    /// </summary>
    internal class ClientConnector
    {
        internal event EventHandler<EventArgs<DataSet>> OverrideResponseReceived;
        /// <summary>
        /// Response queue name on which client will reply for override
        /// </summary>
        private String _overrideRuleResponse = String.Empty;

        /// <summary>
        /// Request queue name on which client will reply for override
        /// </summary>
        private String _overrideRuleRequestSenderExchange = String.Empty;

        /// <summary>
        /// Exchange name where notifications are sent
        /// </summary>
        private String _notificationExchangeName = String.Empty;

        internal ClientConnector()
        {
            try
            {
                _overrideRuleResponse = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_OverrideRuleResponse);
                _overrideRuleRequestSenderExchange = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_OverrideRuleRequest);
                _notificationExchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_NotificationExchange);

                // configure sender
                AmqpHelper.InitializeSender("OverrideRequest", _overrideRuleRequestSenderExchange, MediaType.Exchange_Direct);
                AmqpHelper.InitializeSender("BlockedAlerts", _notificationExchangeName, MediaType.Exchange_Direct);
                AmqpHelper.InitializeSender("FeedbackMessage", _notificationExchangeName, MediaType.Exchange_Direct);

                // initialise listener
                AmqpHelper.Started += new ListenerStarted(AmqpStarted);
                AmqpHelper.InitializeListenerForQueue(_overrideRuleResponse);
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


        private void AmqpStarted(object sender, ListenerStartedEventArguments e)
        {
            try
            {
                if (e.AmqpReceiver.MediaName == _overrideRuleResponse)
                    e.AmqpReceiver.AmqpDataReceived += new DataReceived(OverrideResponse);
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
        /// inform the manager about the response
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OverrideResponse(object sender, DataReceivedEventArguments e)
        {
            try
            {
                if (OverrideResponseReceived != null)
                    OverrideResponseReceived(this, new EventArgs<DataSet>(e.DsReceived));
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
        /// Send the  alert information to user to request for override,block,sendtocomp,softalertwithnotes
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="hardAlerts"></param>
        /// <param name="requestType"></param>
        internal void InformClientForRequest(int userId, List<Alert> alertList, AlertPopUpType popUpType)
        {
            try
            {
                var alerts = alertList.Count == 0 ? new { alerts = alertList, popUpType = popUpType, userId = userId, orderId = String.Empty } : new { alerts = alertList, popUpType = popUpType, userId = alertList[0].UserId, orderId = alertList[0].OrderId };
                var finalAlerts = new { finalAlerts = alerts };
                AmqpHelper.SendObject(finalAlerts, "OverrideRequest", userId.ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sends simulate trade's feedback message to client
        /// </summary>
        /// <param name="feedbackMessage"></param>
        /// <param name="userId"></param>
        internal void SendFeedbackMessage(String feedbackMessage, int userId)
        {
            try
            {
                var message = new { Response = new { FeedbackMessage = feedbackMessage, UserId = userId } };
                AmqpHelper.SendObject(message, "FeedbackMessage", "FeedbackMessage");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
