using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.EventArguments;
using Prana.BusinessObjects.Compliance.Delegates;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.Compliance.EventArguments;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.ComplianceEngine.AlertHistory.DAL
{
    internal class AlertsAmqpPlugin
    {
        internal event UpdateAlertGrid UpdateNewAlertEvent;
        String _notificationExchange = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_NotificationExchange);

        private static AlertsAmqpPlugin _alertsAmqpPlugin;
        private static Object _lockerObject = new Object();

        /// <summary>
        /// Singleton instance
        /// </summary>
        /// <returns></returns>
        public static AlertsAmqpPlugin GetInstance()
        {
            lock (_lockerObject)
            {
                if (_alertsAmqpPlugin == null)
                    _alertsAmqpPlugin = new AlertsAmqpPlugin();
            }
            return _alertsAmqpPlugin;
        }

        /// <summary>
        /// Initializes Amqp Listener for listening current triggering 
        /// alerts. To Notification exchange with AlertFromNotificationManager routing key.
        /// And Initializes Amqp Listener and initializeSender to update alert history grid in case of multi user 
        /// </summary>
        internal void InitializeAmqp()
        {
            try
            {
                AmqpHelper.Started += new ListenerStarted(AmqpHelper_Started);
                AmqpHelper.Stopped += new ListenerStopped(AmqpHelper_Stopped);
                List<String> key = new List<string>();
                // key.Add(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString());
                key.Add("AlertFromNotificationManager");
                AmqpHelper.InitializeListenerForExchange(_notificationExchange, MediaType.Exchange_Direct, key);

                //Initializes Amqp Listener and initializeSender to update alert history grid in case of multi user
                List<String> key1 = new List<string>();
                key1.Add("AlertHistoryDeleted");
                AmqpHelper.InitializeListenerForExchange(_notificationExchange, MediaType.Exchange_Direct, key1);

                AmqpHelper.InitializeSender("AlertHistoryDeleted", _notificationExchange, MediaType.Exchange_Direct);
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
        /// 
        /// </summary>
        /// <param name="amqpReceiver"></param>
        private void AmqpHelper_Started(Object sender, ListenerStartedEventArguments e)
        {
            try
            {
                if (e.AmqpReceiver.MediaName == _notificationExchange)
                    e.AmqpReceiver.AmqpDataReceived += amqpReceiver_AmqpDataReceived;
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
        /// On recieving data with routing key AlertFromNotificationManager raises update alert grid event.
        /// </summary>
        /// <param name="dsReceived"></param>
        /// <param name="mediaName"></param>
        /// <param name="mediaType"></param>
        /// <param name="routingKey"></param>
        void amqpReceiver_AmqpDataReceived(Object sender, DataReceivedEventArguments e)
        {
            try
            {
                if (e.RoutingKey == "_AlertFromNotificationManager")
                {
                    if (e.DsReceived.Tables.Count > 0)
                    {
                        if (UpdateNewAlertEvent != null)
                            UpdateNewAlertEvent(this, new UpdateAlertGridEventArgs { DsRecieved = e.DsReceived, Operation = AlertHistoryOperations.Update });
                    }
                }

                // Receiving data to update alert history grid in case of multi user
                else if (e.RoutingKey == "_AlertHistoryDeleted")
                {
                    if (!e.DsReceived.Tables[0].Rows[0]["UserID"].Equals(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString()))
                    {
                        // pdate table
                        if (UpdateNewAlertEvent != null)
                            UpdateNewAlertEvent(this, new UpdateAlertGridEventArgs { DsRecieved = e.DsReceived, Operation = AlertHistoryOperations.GetData });
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

        private void AmqpHelper_Stopped(Object sender, ListenerStoppedEventArguments e)
        {
            try
            {
                if (e.AmqpReceiver.MediaName == _notificationExchange)
                    e.AmqpReceiver.AmqpDataReceived -= amqpReceiver_AmqpDataReceived;
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
        /// Tells the EXPNL to re initialize the cache after archive/delete And in case of multi user also
        /// </summary>
        internal void PublishArchiveAlert()
        {
            try
            {
                Dictionary<String, String> savedMessage = new Dictionary<string, string>();
                savedMessage.Add("ApplicationStatus", "Saved");
                AmqpHelper.SendObject(savedMessage, "RuleSaveSender", "RuleNoficationSettingsSaved");

                //tells the expnl to re initialize the cache after archive/delete in case of multi user
                DataSet ds = new DataSet();
                ds.Tables.Add();
                ds.Tables[0].Columns.Add("UserID");
                ds.Tables[0].Rows.Add();
                ds.Tables[0].Rows[0]["UserID"] = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString();
                AmqpHelper.SendObject(ds, "AlertHistoryDeleted", "AlertHistoryDeleted");
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
