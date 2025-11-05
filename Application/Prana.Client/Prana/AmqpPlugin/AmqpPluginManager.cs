using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.EventArguments;
using Prana.BusinessObjects.Compliance.Constants;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.CommonDataCache;
using Prana.ComplianceEngine.ComplianceAlertPopup;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.AmqpPlugin
{

    public delegate void OverrideRequestReceivedHandler(String message, DataSet data);

    /// <summary>
    /// Singleton class for managing all Amqp server activity
    /// </summary>
    internal class AmqpPluginManager : IDisposable
    {
        static AmqpPluginManager _amqpPluginManager;

        /// <summary>
        /// THe user ID of the current user
        /// </summary>
        private String _userId;

        private object _amqpBindLocker = new object();

        /// <summary>
        /// Event to send data for pending approval UI
        /// </summary>
        public event EventHandler<EventArgs<DataSet>> PendingApprovalInfoDataSet;

        /// <summary>
        /// Event to send data for pending approval UI
        /// </summary>
        public event EventHandler<EventArgs<DataSet,bool>> PendingApprovalFrozeUnfroze;

        //private bool _isBinded = false;

        /// <summary>
        /// Singleton pattern implemented
        /// </summary>
        /// <returns>Live instance of class</returns>
        internal static AmqpPluginManager GetInstance()
        {
            if (_amqpPluginManager == null)
                _amqpPluginManager = new AmqpPluginManager();

            return _amqpPluginManager;
        }

        /// <summary>
        /// Private constructor to implement singleton pattern
        /// </summary>
        private AmqpPluginManager()
        {

        }

        /// <summary>
        /// Setting up Post trade Pop up on UI
        /// </summary>
        /// <param name="form"></param>
        /// <param name="userId"></param>
        public void SetupCompliancePostPopUp(Form form, int userId)
        {
            try
            {
                _complianceAlertPostPopUp = new ComplianceAlertPopUp(RulePackage.PostTrade);
                _complianceAlertPostPopUp.Owner = form;
                _complianceAlertPostPopUp.Show();
                _complianceAlertPostPopUp.InitialiseControlsForPostPopUp(userId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        #region Private properties
        /// <summary>
        /// Raised when any override request is received from server for any order sent by user
        /// </summary>
        internal event OverrideRequestReceivedHandler OverrideRequestReceived;

        //String _hostName;
        String _responseQueueName = "Nirvana.PreTradeClientOverideResponse";
        String _overrideRequestExchangeName;
        String _notificationExchangeName;
        //Notification _notificationForm= new Notification();
        AlertForm _alertForm;

        ComplianceAlertPopUp _complianceAlertPostPopUp;
        #endregion

        /// <summary>
        /// Initialise the manager for given user
        /// </summary>
        /// <param name="userId">UserId for which communication will be done by manager</param>
        internal void Initialise(String userId)
        {
            try
            {
                _userId = userId;
                LoadOverrideAppSettings();
                InitializeSender(userId);
                InitializeReceiver(userId);
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
        /// App settings required for communication will be loaded
        /// </summary>
        private void LoadOverrideAppSettings()
        {
            try
            {
                //_hostName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_AmqpServer);
                _responseQueueName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_ClientFeedBackOnOverrideResponse);
                _overrideRequestExchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_ClientFeedBackOnOverrideRequest);
                _notificationExchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_NotificationExchange);

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
        /// Initilises AmqpSenders
        /// </summary>
        private void InitializeSender(String userId)
        {
            try
            {

                if (ComplianceCacheManager.GetPreTradeModuleEnabledForUser(Convert.ToInt32(userId)))
                    AmqpHelper.InitializeSender(PreTradeConstants.Const_OverrideResponse, _responseQueueName, MediaType.Queue);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Initialises amq receiver for a given userid (which will be used as routing key)
        /// </summary>
        /// <param name="userId">userid of the given user which will be used as routing key</param>
        private void InitializeReceiver(String userId)
        {
            try
            {
                AmqpHelper.Started += new ListenerStarted(helperOverrideRule_Started);

                if (ComplianceCacheManager.GetPreTradeModuleEnabledForUser(Convert.ToInt32(userId)))
                {
                    List<String> key = new List<string>();
                    key.Add(userId);
                    AmqpHelper.InitializeListenerForExchange(_overrideRequestExchangeName, MediaType.Exchange_Direct, key);
                }
                List<String> keyNotification = new List<string>();
                List<String> keyApproval = new List<string>();
                List<String> keyFroze = new List<string>();
                List<String> keyUnfroze = new List<string>();
                if (ComplianceCacheManager.GetPreTradeModuleEnabledForUser(Convert.ToInt32(userId)))
                {
                    keyNotification.Add("PreAlertFromNotificationManager_" + userId);
                    keyApproval.Add("ProcessPendingApproval");
                    keyFroze.Add("PendingApprovalRowFroze");
                    keyUnfroze.Add("PendingApprovalRowUnfroze");
                    //keyNotification.Add(userId);
                    //AmqpHelper.InitializeListenerForExchange(_notificationExchangeName, MediaType.Exchange_Direct, new List<String>() { "PreAlertFromNotificationManager" });
                    AmqpHelper.InitializeListenerForExchange(_notificationExchangeName, MediaType.Exchange_Direct, new List<String>() { userId });
                }
                if (ComplianceCacheManager.GetPostTradeModuleEnabledForUser(Convert.ToInt32(userId)))
                {
                    keyNotification.Add("PostAlertFromNotificationManager_" + userId);
                    //AmqpHelper.InitializeListenerForExchange(_notificationExchangeName, MediaType.Exchange_Direct, new List<String>() { "PostAlertFromNotificationManager" });
                }
                if (keyNotification.Count > 0)
                    AmqpHelper.InitializeListenerForExchange(_notificationExchangeName, MediaType.Exchange_Direct, keyNotification);
                if (keyApproval.Count > 0)
                    AmqpHelper.InitializeListenerForExchange(_notificationExchangeName, MediaType.Exchange_Direct, keyApproval);
                if (keyFroze.Count > 0)
                    AmqpHelper.InitializeListenerForExchange(_notificationExchangeName, MediaType.Exchange_Direct, keyFroze);
                if (keyUnfroze.Count > 0)
                    AmqpHelper.InitializeListenerForExchange(_notificationExchangeName, MediaType.Exchange_Direct, keyUnfroze);
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
        /// Event listener when any of the listener has been configured and started
        /// </summary>
        /// <param name="sender">Instance of receiver which has been started</param>
        /// <param name="e"></param>
        void helperOverrideRule_Started(Object sender, ListenerStartedEventArguments e)
        {
            try
            {
                if (e.AmqpReceiver.MediaName == _overrideRequestExchangeName)
                {
                    e.AmqpReceiver.AmqpDataReceived += new DataReceived(helperOverrideRule_DataRecieved);
                    e.AmqpReceiver.Stopped += new ListenerStopped(amqpReceiver_Stopped);
                }
                else if (e.AmqpReceiver.MediaName == _notificationExchangeName)
                {
                    //if (notificationForm == null)
                    //    notificationForm = new Notification();
                    lock (_amqpBindLocker)
                    {
                        //if (!_isBinded)
                        //{
                        e.AmqpReceiver.AmqpDataReceived += new Prana.AmqpAdapter.Delegates.DataReceived(Notification_DataReceived);
                        e.AmqpReceiver.Stopped += new ListenerStopped(amqpReceiver_StoppedNotification);
                        //    _isBinded = true;
                        //}
                    }
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

        /// <summary>
        /// Event listener when receiver has been stopped (This is for noification)
        /// </summary>
        /// <param name="sender">Instance of receiver which has been stopped</param>
        /// <param name="e">Cause of stopping</param>
        void amqpReceiver_StoppedNotification(Object sender, ListenerStoppedEventArguments e)
        {
            try
            {
                e.AmqpReceiver.AmqpDataReceived -= new Prana.AmqpAdapter.Delegates.DataReceived(Notification_DataReceived);
                e.AmqpReceiver.Stopped -= new ListenerStopped(amqpReceiver_StoppedNotification);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Event listener when receiver has been stopped (This is for override notification)
        /// </summary>
        /// <param name="sender">Instance of receiver which has been stopped</param>
        /// <param name="e">Cause of stopping</param>
        void amqpReceiver_Stopped(Object sender, ListenerStoppedEventArguments e)
        {
            try
            {
                e.AmqpReceiver.AmqpDataReceived -= new DataReceived(helperOverrideRule_DataRecieved);
                e.AmqpReceiver.Stopped -= new ListenerStopped(amqpReceiver_Stopped);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Event listener when any notification received
        /// </summary>
        /// <param name="sender">Alert data in form of dataset</param>
        /// <param name="e">Name of media</param>

        void Notification_DataReceived(Object sender, DataReceivedEventArguments e)
        {
            try
            {

                if (e.MediaName == _notificationExchangeName)
                {
                    if (e.RoutingKey.Equals("_" + _userId))
                    {
                        String ruleName = e.DsReceived.Tables["alert1"].Rows[0]["RuleName"].ToString();
                        if (ruleName == "N/A" || ruleName == "Missing Information Alert" || ruleName.Contains("Compliance failed to process correctly")
                            || ruleName == "Something went wrong !! Please Contact Support.")
                        {
                            String dimension = MessageFormatter.FormatRuleNameForAlert(e.DsReceived.Tables[0].Rows[0]["dimension"].ToString());
                            if (_complianceAlertPostPopUp != null && !String.IsNullOrEmpty(ruleName) && ruleName != "N/A")
                                _complianceAlertPostPopUp.NewAlertReceived(e.DsReceived.Tables[0].Rows[0]["RuleType"].ToString(), ruleName + dimension, e.DsReceived);
                        }
                    }
                    else if (e.RoutingKey.Equals("_PreAlertFromNotificationManager_" + _userId + "_PostAlertFromNotificationManager_" + _userId) || e.RoutingKey.Equals("_PostAlertFromNotificationManager_" + _userId) || e.RoutingKey.Equals("_PreAlertFromNotificationManager_" + _userId))
                    {
                        String ruleName = e.DsReceived.Tables[0].Rows[0]["name"].ToString();
                        String ruleType = e.DsReceived.Tables[0].Rows[0]["RuleType"].ToString();
                        String dimension = MessageFormatter.FormatRuleNameForAlert(e.DsReceived.Tables[0].Rows[0]["dimension"].ToString());

                        if (_complianceAlertPostPopUp != null && !String.IsNullOrEmpty(ruleName) && ruleName != "N/A")
                            _complianceAlertPostPopUp.NewAlertReceived(ruleType, ruleName + dimension, e.DsReceived);
                    }
                    // Pending Approval Alerts Response
                    else if (e.RoutingKey.Equals("_ProcessPendingApproval"))
                    {
                        if (PendingApprovalInfoDataSet != null)
                            PendingApprovalInfoDataSet(this, new EventArgs<DataSet>(e.DsReceived));
                    }
                    else if(e.RoutingKey.Equals("_PendingApprovalRowFroze"))
                    {
                        if (PendingApprovalFrozeUnfroze != null)
                            PendingApprovalFrozeUnfroze(this, new EventArgs<DataSet,bool>(e.DsReceived,true));
                    }
                    else if (e.RoutingKey.Equals("_PendingApprovalRowUnfroze"))
                    {
                        if (PendingApprovalFrozeUnfroze != null)
                            PendingApprovalFrozeUnfroze(this, new EventArgs<DataSet, bool>(e.DsReceived, false));
                    }
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

        /// <summary>
        /// Override request received
        /// </summary>
        /// <param name="sender">Override data</param>
        /// <param name="e">Name of the media</param>      
        void helperOverrideRule_DataRecieved(Object sender, DataReceivedEventArguments e)
        {
            try
            {
                if (e.MediaName == _overrideRequestExchangeName)
                {
                    String message = "";
                    //MessageFormatter.FormatToOverrideMessage(e.DsReceived, out message);
                    if (OverrideRequestReceived != null)
                        OverrideRequestReceived(message, e.DsReceived);
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

        /// <summary>
        /// Response of override request
        /// </summary>
        /// <param name="data">Data of request</param>
        /// <param name="doAllowOrder">True if to allow by trade server otherwise false</param>
        /*internal void SendOverrideResponse(DataSet data, bool doAllowOrder)
        {
            try
            {
                //DataSet dsTemp = MessageFormatter.FormatToOverrrideResponseMessage(data, doAllowOrder);
                var response = new { Response = new { IsAllowed = doAllowOrder, OrderId = data.Tables[0].Rows[0]["OrderId"], UserId = data.Tables[0].Rows[0]["UserId"], blocked = true, isApprovalRequired = false } };
                AmqpHelper.SendObject(response, "OverrideResponse", null);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }*/

        /// <summary>
        /// Close all connection
        /// </summary>
        //internal void Close()
        //{
        //    try
        //    {
        //        AmqpHelper.Started -= new ListenerStarted(helperOverrideRule_Started);
        //        AmqpHelper.CloseAllConnection();
        //    }
        //    catch (Exception ex)
        //    {
        //        //Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        public void Dispose()
        {
            try
            {
                //if (_notificationForm != null)
                //{
                //    _notificationForm.Dispose();
                //    _notificationForm = null;
                //}
                if (_alertForm != null)
                {
                    _alertForm.Dispose();
                    _alertForm = null;
                }
                if (_complianceAlertPostPopUp != null)
                {
                    _complianceAlertPostPopUp.Dispose();
                    _complianceAlertPostPopUp = null;
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Send a dismiss event to server so that the notifications can be sent to the notification manager
        /// </summary>
        /// <param name="dataSet"></param>
        //internal void SendDismissEvent(DataSet data)
        //{
        //    try
        //    {
        //        //DataSet dsTemp = MessageFormatter.FormatToOverrrideResponseMessage(data, doAllowOrder);
        //        var response = new { Response = new { IsAllowed = false, OrderId = data.Tables[0].Rows[0]["OrderId"], UserId = data.Tables[0].Rows[0]["UserId"], blocked = true, isDismissed = true, isApprovalRequired = false } };
        //        AmqpHelper.SendObject(response, "OverrideResponse", null);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// Send Approval Response
        /// </summary>
        /// <param name="data"></param>
        /// <param name="approvalRequired"></param>
        /*internal void SendApprovalResponse(DataSet data, bool approvalRequired)
        {
            try
            {
                var response = new { Response = new { IsAllowed = approvalRequired, OrderId = data.Tables[0].Rows[0]["OrderId"], UserId = data.Tables[0].Rows[0]["UserId"], isApprovalRequired = approvalRequired } };
                AmqpHelper.SendObject(response, "OverrideResponse", null);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }*/

        /// <summary>
        /// Send Override or Bloecked alerts response
        /// </summary>
        /// <param name="data"></param>
        /// <param name="approvalRequired"></param>
        /// <param name="popUpType"></param>
        internal void SendResponse(DataSet data, bool approvalRequired, AlertPopUpType popUpType)
        {
            try
            {
                var response = new object();
                if (popUpType == AlertPopUpType.Override)
                {
                    response = new { Response = new { IsAllowed = approvalRequired, OrderId = data.Tables[0].Rows[0]["OrderId"], UserId = data.Tables[0].Rows[0]["UserId"], blocked = true, isApprovalRequired = false, popUpType = popUpType } };
                }
                else
                {
                    response = new { Response = new { IsAllowed = approvalRequired, OrderId = data.Tables[0].Rows[0]["OrderId"], UserId = data.Tables[0].Rows[0]["UserId"], isApprovalRequired = approvalRequired, popUpType = popUpType } };
                }
                AmqpHelper.SendObject(response, PreTradeConstants.Const_OverrideResponse, null);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
    }
}