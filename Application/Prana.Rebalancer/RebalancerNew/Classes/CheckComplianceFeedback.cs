using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.EventArguments;
using Prana.ComplianceEngine.ComplianceAlertPopup;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Rebalancer.RebalancerNew.Classes
{
    internal class CheckComplianceFeedback
    {
        internal event EventHandler<EventArgs<DataSet>> FeedbackMessageReceived;
        /// <summary>
        /// Exchange name where notifications are sent
        /// </summary>
        private string _notificationExchangeName = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        internal CheckComplianceFeedback()
        {
            try
            {
                _notificationExchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_NotificationExchange);
                AmqpHelper.InitializeListenerForExchange(_notificationExchangeName, MediaType.Exchange_Direct, new List<string>() { ComplainceConstants.CONST_FEEDBACK_MESSAGE });

                AmqpHelper.Started += new ListenerStarted(AmqpHelper_Started);
                AmqpHelper.Stopped += new ListenerStopped(AmqpHelper_Stopped);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// AmqpHelper_Stopped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AmqpHelper_Stopped(object sender, ListenerStoppedEventArguments e)
        {
            try
            {
                if (e.AmqpReceiver.MediaName == _notificationExchangeName)
                    e.AmqpReceiver.AmqpDataReceived -= new DataReceived(FeedbackMessage);
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
        /// AmqpHelper_Started
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AmqpHelper_Started(object sender, ListenerStartedEventArguments e)
        {
            try
            {
                if (e.AmqpReceiver.MediaName == _notificationExchangeName)
                    e.AmqpReceiver.AmqpDataReceived += new DataReceived(FeedbackMessage);
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
        /// FeedbackMessage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FeedbackMessage(object sender, DataReceivedEventArguments e)
        {
            try
            {
                if (e.RoutingKey.Equals("_FeedbackMessage"))
                {
                    if (FeedbackMessageReceived != null)
                        FeedbackMessageReceived(this, new EventArgs<DataSet>(e.DsReceived));
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
    }
}
