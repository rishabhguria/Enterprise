using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.EventArguments;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Constants;
using Prana.Global;
using Prana.LogManager;
using Prana.NotificationManager.BLL.Processor;
using Prana.NotificationManager.Delegates;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.NotificationManager.BLL.AmqpConnector
{
    internal static class AmqpConnectionManager
    {

        const String preKey = "All";
        const String postKey = "PostTradeNotification";

        static String notificationExchangeName;
        //static String amqpHostName;
        public static event AlertDelegate alertEvent;
        public static event RuleSavedHandler RuleSaved;
        public static event PreTradeApprovalInfoEvent PreTradeApprovalInfoEvent;

        /// <summary>
        /// Initialize AMQP for notification.
        /// </summary>
        internal static void InitializeAmqpConnection()
        {
            try
            {
                LoadAppSettings();
                AmqpHelper.Started += new ListenerStarted(AmqpStarted);
                AmqpHelper.Stopped += AmqpHelper_Stopped;
                //"PreTradeNotification","PostTradeNotification"
                List<String> subscriber = new List<string>();
                subscriber.Add("RuleNoficationSettingsSaved");
                AmqpHelper.InitializeListenerForExchange(notificationExchangeName, MediaType.Exchange_Direct, subscriber);

                List<String> key = new List<string>();
                key.Add(preKey);
                key.Add(postKey);

                AmqpHelper.InitializeListenerForExchange(notificationExchangeName, MediaType.Exchange_Direct, key);
                AmqpHelper.InitializeSender("NotificationSender", notificationExchangeName, MediaType.Exchange_Direct);

                List<String> approvalKey = new List<string>();
                approvalKey.Add("SendApprovalRequest");
                AmqpHelper.InitializeListenerForExchange(notificationExchangeName, MediaType.Exchange_Direct, approvalKey);
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
        /// <param name="cause"></param>
        static void AmqpHelper_Stopped(Object sender, ListenerStoppedEventArguments e)
        {
            try
            {
                e.AmqpReceiver.AmqpDataReceived -= new DataReceived(amqpReceiver_AmqpDataReceivedRuleNotification);
                e.AmqpReceiver.AmqpDataReceived -= new DataReceived(amqpReceiver_AmqpDataReceived);
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
        private static void LoadAppSettings()
        {
            //amqpHostName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_AmqpServer);
            try
            {
                notificationExchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_NotificationExchange);
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
        static void AmqpStarted(Object sender, ListenerStartedEventArguments e)
        {
            try
            {
                if (e.AmqpReceiver.MediaName == notificationExchangeName)
                {
                    if (e.AmqpReceiver.RoutingKey.Contains("RuleNoficationSettingsSaved"))
                    {
                        e.AmqpReceiver.AmqpDataReceived += new DataReceived(amqpReceiver_AmqpDataReceivedRuleNotification);
                    }
                    else
                    {
                        e.AmqpReceiver.AmqpDataReceived += new DataReceived(amqpReceiver_AmqpDataReceived);
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
        /// 
        /// </summary>
        /// <param name="dsReceived"></param>
        /// <param name="mediaName"></param>
        /// <param name="mediaType"></param>
        /// <param name="routingKey"></param>
        static void amqpReceiver_AmqpDataReceivedRuleNotification(Object sender, DataReceivedEventArguments e)
        {
            try
            {
                if (RuleSaved != null)
                {
                    RuleSaved(e.DsReceived);
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
        /// 
        /// </summary>
        /// <param name="dsReceived"></param>
        /// <param name="mediaName"></param>
        /// <param name="mediaType"></param>
        /// <param name="routingKey"></param>
        static void amqpReceiver_AmqpDataReceived(Object sender, DataReceivedEventArguments e)
        {
            try
            {
                DataRow firstRow = e.DsReceived.Tables[0].Rows[0];
                bool isCancel = false;
                string replaceAlertType = string.Empty;
                if (firstRow!=null && firstRow.Table.Columns.Contains("Status") && firstRow["Status"].ToString().Equals(PreTradeConstants.MSG_TRADE_EXPIRED))
                    isCancel = true;
                if (firstRow != null && firstRow.Table.Columns.Contains("Status") && firstRow["Status"].ToString().Equals(PreTradeConstants.MSG_TRADE_EXPIRED_REPLACED))
                    replaceAlertType = firstRow["ReplaceAlertType"].ToString();

                if (e.RoutingKey.Equals("_SendApprovalRequest"))
                {
                    if (PreTradeApprovalInfoEvent != null)
                        PreTradeApprovalInfoEvent(e.DsReceived);
                }
                else
                {
                    if (alertEvent != null)
                    {
                        if (e.DsReceived.Tables[0].Rows.Count > 1)
                        {
                            EmailProcessor.TotalAlerts = e.DsReceived.Tables[0].Rows.Count;
                            DataSet ds = new DataSet();

                            foreach (DataRow row in e.DsReceived.Tables[0].Rows)
                            {
                                ds = Alert.RowToDataset(row);
                                Alert finalAlert = Alert.GetAlertObject(ds);
                                alertEvent(finalAlert, isCancel, replaceAlertType);
                            }
                        }
                        else
                        {
                            EmailProcessor.TotalAlerts = 1;
                            Alert alert = Alert.GetAlertObject(e.DsReceived);
                            alertEvent(alert, isCancel, replaceAlertType);
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
        }

        //private static Alert GetAlertObject(DataSet data)
        //{
        //    Alert alert = new Alert();
        //    if (data.Tables[0].Columns.Contains("ValidationTime"))
        //        alert.ValidationTime = Convert.ToDateTime(data.Tables[0].Rows[0]["ValidationTime"]);
        //    if (data.Tables[0].Columns.Contains("RuleId"))
        //        alert.RuleId = data.Tables[0].Rows[0]["RuleId"].ToString();
        //    if (data.Tables[0].Columns.Contains("Name"))
        //        alert.RuleName = data.Tables[0].Rows[0]["Name"].ToString();
        //    if (data.Tables[0].Columns.Contains("UserId"))
        //        alert.UserId = Convert.ToInt32(data.Tables[0].Rows[0]["UserId"]);
        //    if (data.Tables[0].Columns.Contains("RuleType"))
        //        alert.PackageName = (RulePackage)Enum.Parse(typeof(RulePackage), data.Tables[0].Rows[0]["RuleType"].ToString());
        //    if (data.Tables[0].Columns.Contains("Summary"))
        //        alert.Summary = data.Tables[0].Rows[0]["Summary"].ToString();
        //    if (data.Tables[0].Columns.Contains("CompressionLevel"))
        //        alert.CompressionLevel = data.Tables[0].Rows[0]["CompressionLevel"].ToString();
        //    if (data.Tables[0].Columns.Contains("Parameters"))
        //        alert.Parameters = data.Tables[0].Rows[0]["Parameters"].ToString();
        //    if (data.Tables[0].Columns.Contains("OrderId"))
        //        alert.OrderId = data.Tables[0].Rows[0]["OrderId"].ToString();
        //    if (data.Tables[0].Columns.Contains("Status"))
        //        alert.Status = data.Tables[0].Rows[0]["Status"].ToString();
        //    if (data.Tables[0].Columns.Contains("Description"))
        //        alert.Description = data.Tables[0].Rows[0]["Description"].ToString();
        //    if (data.Tables[0].Columns.Contains("Dimension"))
        //        alert.Dimension = data.Tables[0].Rows[0]["Dimension"].ToString();
        //    return alert;
        //}

    }
}