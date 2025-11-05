using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.SocketCommunication;
using System;

namespace Prana.PostTradeServices
{
    public class PostTradeServicesClient : IPostTradeServices, IDisposable
    {
        static PostTradeServicesClient _postTradeServicesClient = new PostTradeServicesClient();

        /// <summary>
        /// Singleton implementation
        /// </summary>
        public static PostTradeServicesClient Instance
        {
            get
            {
                return _postTradeServicesClient;
            }
        }

        ClientTradeCommManager _clientCommunicationManager = null;
        public event EventHandler Disconnected;
        public event EventHandler Connected;
        public event EventHandler<EventArgs<QueueMessage>> MissingTradesRecieved;

        void _clientCommunicationManager_MessageReceived(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                QueueMessage message = e.Value;
                switch (message.MsgType)
                {
                    case CustomFIXConstants.MSG_GET_MISSING_TRADES:

                        if (MissingTradesRecieved != null)
                        {
                            MissingTradesRecieved(this, new EventArgs<QueueMessage>(message));
                        }
                        break;

                    case "CP":
                        break;

                    default:
                        if (MessageReceived != null)
                        {
                            MessageReceived(this, new EventArgs<QueueMessage>(message));
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

        #region ISecurityMaster Members
        public void ConnectToServer()
        {
            try
            {
                if ((_clientCommunicationManager == null || (CachedDataManager.GetInstance.LoggedInUser != null && _clientCommunicationManager.ConnectionStatus != PranaInternalConstants.ConnectionStatus.CONNECTED)))
                {
                    //connect to server 
                    _clientCommunicationManager = new ClientTradeCommManager();
                    _clientCommunicationManager.IsMonitoringConnection = false;
                    //Wireup for receiving OrderMessages
                    _clientCommunicationManager.MessageReceived += new Prana.Interfaces.MessageReceivedDelegate(_clientCommunicationManager_MessageReceived);
                    _clientCommunicationManager.Disconnected += new EventHandler(_clientCommunicationManager_Disconnected);
                    _clientCommunicationManager.Connected += new EventHandler(_clientCommunicationManager_Connected);
                    ConnectionProperties connProperties = new ConnectionProperties();
                    connProperties.Port = ClientAppConfiguration.TradeServer.Port;
                    connProperties.ServerIPAddress = ClientAppConfiguration.TradeServer.IpAddress;
                    _smUserID = "Corp_Action" + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString();
                    connProperties.IdentifierID = _smUserID;
                    connProperties.IdentifierName = "Corp_Action" + CachedDataManager.GetInstance.LoggedInUser.FirstName.ToString();
                    connProperties.ConnectedServerName = "Corp_Action Server";
                    connProperties.HandlerType = HandlerType.PostTradeServicesHandler;
                    if (_clientCommunicationManager.Connect(connProperties) == PranaInternalConstants.ConnectionStatus.CONNECTED)
                    {
                        _isConnected = true;
                    }
                    else
                    {
                        _isConnected = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                //release
                if (_clientCommunicationManager != null)
                {
                    _clientCommunicationManager.DisConnect();
                    _clientCommunicationManager = null;
                }
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public async System.Threading.Tasks.Task ConnectToServerAsync()
        {
            ConnectToServer();

            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;
        }

        void _clientCommunicationManager_Connected(object sender, EventArgs e)
        {
            _isConnected = true;
            if (Connected != null)
            {
                Connected(sender, e);
            }
        }

        void _clientCommunicationManager_Disconnected(object sender, EventArgs e)
        {
            _isConnected = false;
            if (Disconnected != null)
            {
                Disconnected(sender, e);
            }
        }

        bool _isConnected = false;
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
            set
            {
                _isConnected = value;
            }
        }

        public void DisConnect()
        {
            try
            {
                if (_isConnected)
                {
                    _clientCommunicationManager.DisConnect();
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
        #endregion

        private string _smUserID;

        public void SendMessage(QueueMessage message)
        {
            if (_clientCommunicationManager != null && _clientCommunicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
            {
                message.UserID = _smUserID;

                _clientCommunicationManager.SendMessage(message);
            }
        }

        public event EventHandler<EventArgs<QueueMessage>> MessageReceived;

        /// <summary>
        /// In order to disable retryconnection to server in case of LogOut.
        /// See details : http://jira.nirvanasolutions.com:8080/browse/PRANA-1797
        /// </summary>
        /// <param name="shouldRetry"></param>
        public void Retry(bool shouldRetry)
        {
            _clientCommunicationManager.ShouldRetry = shouldRetry;
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
                //TODO: This change need to be reviewed as following line was commented earlier
                _clientCommunicationManager.Dispose();
            }
        }

        #endregion
    }
}
