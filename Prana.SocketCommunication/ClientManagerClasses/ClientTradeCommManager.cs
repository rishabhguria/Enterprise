using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.QueueManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;

namespace Prana.SocketCommunication
{
    public class ClientTradeCommManager : ICommunicationManager, IDisposable
    {
        #region Private Variables

        private SocketConnection _socketConn = null;
        private IQueueProcessor _socketQueue;
        private ConnectionProperties _connProperties = null;
        private int reconnectInterval = 0;
        private readonly object lockConnection = new object();
        #endregion

        public ClientTradeCommManager()
        {
            try
            {
                _socketQueue = new QueueProcessor();
                _socketQueue.MessageQueued += new EventHandler<EventArgs<QueueMessage>>(_socketQueue_MessageQueued);
                int.TryParse(ConfigurationManager.AppSettings["AutoReconnectInterval"].ToString(), out reconnectInterval);
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

        private void _socketQueue_MessageQueued(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                QueueMessage qmsg = e.Value;
                switch (qmsg.MsgType)
                {
                    case FIXConstants.MSGHeartbeat:

                        break;
                    case CustomFIXConstants.MSG_RESPONSE_COMPLETED:

                        InvokeResponseCompleted(qmsg);
                        break;
                    default: // Non Admin Messages
                        if (MessageReceived != null)
                        {
                            if (MessageReceived != null)
                            {
                                MessageReceived(sender, new EventArgs<QueueMessage>(qmsg));
                            }
                        }
                        break;
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

        #region Socket Connection Related Methods
        /// <summary>
        /// create a new socket for connecting
        /// </summary>
        /// <param name="connProperties"></param>
        /// <returns></returns>
        public PranaInternalConstants.ConnectionStatus Connect(ConnectionProperties connProperties)
        {
            try
            {
                lock (lockConnection)
                {
                    _connProperties = connProperties;
                    _socketConn = new SocketConnection();
                    _socketConn.MessageReceived += new MessageReceivedDelegate(_socketConn_MessageReceived);
                    _socketConn.Disconnected += new EventHandler(_socketConn_Disconnected);
                    _socketConn.Connected += new EventHandler(_socketConn_Connected);
                    _socketConn.SocketException += new ExceptionDelegate(_socketConn_SocketException);
                    if (_socketConn.Connect(_connProperties) == PranaInternalConstants.ConnectionStatus.CONNECTED)
                    {
                        SendMessage(new QueueMessage(AdminMessageHandler.CreateLogOnMessage(_connProperties)));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
            return _socketConn.ConnectionStatus;
        }

        void _socketConn_Connected(object sender, EventArgs e)
        {
            try
            {
                if (_isMonitoringConnection)
                {
                    ConnectionMgr.SetClientConnectionStatus(_connProperties, _socketConn.ConnectionStatus);
                    if (Connected != null)
                    {
                        Connected(sender, e);
                    }
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

        void _socketConn_SocketException(object sender, EventArgs<Exception> e)
        {
            Exception ex = e.Value;
            SocketConnection socketConn = (SocketConnection)sender;
            socketConn.DisConnect(ex.Message);
        }

        private bool _shouldRetry = true;
        public bool ShouldRetry
        {
            get { return _shouldRetry; }
            set { _shouldRetry = value; }
        }

        System.Threading.Timer retryTimer = null;
        private void RetryConnection()
        {
            if (reconnectInterval > 0 && ShouldRetry)
            {
                retryTimer = new Timer(RetryConnectionCallBack, null, reconnectInterval * 1000, System.Threading.Timeout.Infinite);
            }
        }

        private void RetryConnectionCallBack(object state)
        {
            if (ShouldRetry == true)
            {
                Connect(_connProperties);
            }
        }

        void _socketConn_Disconnected(object sender, EventArgs e)
        {
            try
            {
                if (_isMonitoringConnection)
                {
                    ConnectionMgr.SetClientConnectionStatus(_connProperties, _socketConn.ConnectionStatus);
                    if (Disconnected != null)
                    {
                        Disconnected(sender, e);
                    }
                }
                // retry for connection is disconnection has happened
                RetryConnection();
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

        void _socketConn_MessageReceived(object sender, EventArgs<QueueMessage> e)
        {
            QueueMessage qMsg = e.Value;
            _socketQueue.SendMessage(qMsg);
        }

        public void SendGenericMessage(string message)
        {
            _socketConn.SendGenericMessage(message);
        }

        public void SendMessage(string message)
        {
            QueueMessage qMsg = new QueueMessage();
            qMsg.Message = message;
            SendMessage(qMsg); // refresh/issue -- we are queueing message
        }

        public void SendMessage(QueueMessage message)
        {
            if (message.RequestID != string.Empty && message.HashCode != int.MinValue)
            {
                if (!_requestedList.ContainsKey(message.RequestID))
                {
                    _requestedList.Add(message.RequestID, message.HashCode);
                }
            }
            if (_socketConn.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
            {
                _socketConn.SendMessage(message.ToString());
            }
        }
        #endregion

        #region Properties

        public PranaInternalConstants.ConnectionStatus ConnectionStatus
        {
            get
            {
                if (_socketConn == null)
                    return PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                else
                    return
                    _socketConn.ConnectionStatus;
            }
        }
        private bool _isMonitoringConnection = true;
        public bool IsMonitoringConnection
        {
            set { _isMonitoringConnection = value; }
            get { return _isMonitoringConnection; }
        }
        #endregion

        public void DisConnect()
        {
            try
            {
                if (_socketConn != null && _socketConn.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    _socketConn.DisConnect("User Requested to Disconnect Server");
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

        #region Events
        public event MessageReceivedDelegate MessageReceived;
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event CompletedReceivedDelegate ResponseCompleted;
        public delegate void CompletedReceivedDelegate(object sender, EventArgs<QueueMessage> e);
        #endregion

        private Dictionary<string, int> _requestedList = new Dictionary<string, int>();
        public void AddRequest(string requestID, int hashCode)
        {
            _requestedList.Add(requestID, hashCode);
        }

        delegate void AsyncInvokeDelegate(Delegate del, EventArgs<QueueMessage> e);
        private static void InvokeDelegate(Delegate sink, EventArgs<QueueMessage> e)
        {
            try
            {
                sink.DynamicInvoke(sink, e);
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

        void InvokeResponseCompleted(QueueMessage qMsg)
        {
            try
            {
                string requestID = qMsg.RequestID;
                if (ResponseCompleted != null)
                {
                    Delegate[] subscriberList = ResponseCompleted.GetInvocationList();

                    AsyncInvokeDelegate invoker = new AsyncInvokeDelegate(InvokeDelegate);
                    foreach (Delegate subscriber in subscriberList)
                    {
                        int subscriberHashCode = subscriber.Target.GetHashCode();
                        if (_requestedList.ContainsKey(requestID) && subscriberHashCode == _requestedList[requestID])
                        {
                            _requestedList.Remove(requestID);
                            invoker.BeginInvoke(subscriber, new EventArgs<QueueMessage>(qMsg), null, null);
                        }
                    }
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

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                retryTimer.Dispose();
                _socketConn.Dispose();
            }
        }
        #endregion
    }
}

