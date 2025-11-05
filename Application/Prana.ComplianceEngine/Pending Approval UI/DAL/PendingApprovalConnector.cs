using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.EventArguments;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;

namespace Prana.ComplianceEngine.Pending_Approval_UI.DAL
{
    internal class PendingApprovalConnector : IDisposable
    {
        /// <summary>
        /// proxy for the pending apporval UI to get data from server
        /// </summary>
        private ProxyBase<IPreTradeService> _preTradeService = null;

        /// <summary>
        /// Update Pending Approval Grid in case of Multi User 
        /// </summary>
        public event EventHandler<EventArgs<List<Alert>, string, string, int>> UpdateGridForMultiUser;

        /// <summary>
        /// Notification Exchange
        /// </summary>
        String _notificationExchange = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_NotificationExchange);

        #region SingiltonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static PendingApprovalConnector _PendingApprovalConnector = null;

        /// <summary>
        /// private cunstructor, Initialises the proxy
        /// </summary>
        private PendingApprovalConnector()
        {
            try
            {
                _preTradeService = new ProxyBase<IPreTradeService>("TradePreTradeComplianceServiceEndpointAddress");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        internal static PendingApprovalConnector GetInstance()
        {
            try
            {
                lock (_lock)
                {
                    if (_PendingApprovalConnector == null)
                        _PendingApprovalConnector = new PendingApprovalConnector();
                    return _PendingApprovalConnector;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }
        #endregion

        /// <summary>
        /// Initialize Amqp
        /// </summary>
        internal void InitializeAmqp()
        {
            try
            {
                AmqpHelper.Started += new ListenerStarted(AmqpHelper_Started);
                AmqpHelper.Stopped += new ListenerStopped(AmqpHelper_Stopped);

                //Initializes Amqp Listener and initializeSender to update Pending Approval grid in case of multi user
                List<String> key = new List<string>();
                key.Add("SendApprovalResponse");
                AmqpHelper.InitializeListenerForExchange(_notificationExchange, MediaType.Exchange_Direct, key);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AmqpHelper_Stopped(Object sender, ListenerStoppedEventArguments e)
        {
            try
            {
                if (e.AmqpReceiver.MediaName == _notificationExchange)
                    e.AmqpReceiver.AmqpDataReceived -= amqpReceiver_AmqpDataReceived;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// On recieving data with routing key SendApprovalRequest raises update pending approval grid event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void amqpReceiver_AmqpDataReceived(Object sender, DataReceivedEventArguments e)
        {
            try
            {
                if (e.RoutingKey.Equals("_SendApprovalResponse"))
                {
                    if (e.DsReceived.Tables.Count > 0)
                    {
                        List<Alert> alerts = Alert.GetAlertObjectFromDataTable(e.DsReceived.Tables["alert"]);
                        string basketId = e.DsReceived.Tables["rootobj"].Rows[0]["basketId1"].ToString();
                        string preTradeActionType = e.DsReceived.Tables["rootobj"].Rows[0]["preTradeActionType1"].ToString();
                        int actionUser = Convert.ToInt32(e.DsReceived.Tables["rootobj"].Rows[0]["actionUser1"]);
                        if (UpdateGridForMultiUser != null)
                            UpdateGridForMultiUser(this, new EventArgs<List<Alert>, string, string, int>(alerts, basketId, preTradeActionType, actionUser));
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Getting pending approval Data from the Server
        /// </summary>
        /// <param name="compression"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        internal Dictionary<String, PreTradeApprovalInfo> GetPendingApprovalData()
        {
            try
            {
                Dictionary<String, PreTradeApprovalInfo> result = new Dictionary<string, PreTradeApprovalInfo>();
                List<PreTradeApprovalInfo> pendingApprovalData = _preTradeService.InnerChannel.GetPendingApprovalData();

                foreach (var item in pendingApprovalData)
                {
                    result.Add(item.MultiTradeName, item);
                }
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        #region IDisposable

        /// <summary>
        /// Dispose() calls Dispose(true)
        /// </summary>
        public void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Approve/Block Button Clicked
        /// </summary>
        /// <param name="alerts"></param>
        internal PreTradeActionType ApproveBlockBtnClicked(List<Alert> alerts)
        {
            try
            {
                return _preTradeService.InnerChannel.ApproveBlockBtnClicked(alerts);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return PreTradeActionType.NoAction;
            }
        }
        #endregion
    }
}
