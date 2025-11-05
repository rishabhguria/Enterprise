using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Prana.SocketCommunication
{
    public class ServerCustomCommunicationManager : IDisposable
    {
        #region  Public Variables
        public event ConnectionMessageReceivedDelegate Connected;
        public event ConnectionMessageReceivedDelegate Disconnected;
        public Action<int> UserExceptionDelegate;
        #endregion

        #region Private Variables
        private int nPortListen = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("OrderRequestPort"));
        private Socket serverListener;
        private bool _isServerClosed = false;
        private static ServerCustomCommunicationManager _communicationManager = null;
        private static List<string> _tradingAccounts = null;
        #endregion

        #region Initialization
        public static ServerCustomCommunicationManager GetInstance()
        {
            return _communicationManager;
        }

        static ServerCustomCommunicationManager()
        {
            try
            {
                if (_communicationManager == null)
                {
                    _communicationManager = new ServerCustomCommunicationManager();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public void Initialise(List<IQueueProcessor> inComMgrQueueList, List<IQueueProcessor> outComMgrQueueList, List<string> tradingAccounts)
        {
            try
            {
                _tradingAccounts = tradingAccounts;
                _isServerClosed = false;
                IPAddress[] aryLocalAddr = null;
                String strHostName = "";
                try
                {
                    foreach (IQueueProcessor inComMgrQueue in inComMgrQueueList)
                    {
                        HandlerFactory.RegisterHandler(inComMgrQueue);
                    }

                    foreach (IQueueProcessor outComMgrQueue in outComMgrQueueList)
                    {
                        outComMgrQueue.MessageQueued += new EventHandler<EventArgs<QueueMessage>>(_outComMgrQueue_MessageQueued);
                    }
                    //TODO: improve the way the ipEntry is found
                    strHostName = Dns.GetHostName();

                    IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);

                    /**
                     * http://jira.nirvanasolutions.com:8080/browse/PRANA-6497
                     * Correcting the changes done in revision no - 20170 - Microsoft managed recommended rules
                     * if machine contain more than one NIC then there could be more than 2 IP addresses.
                     * Each NIC may contain 1 IPv4 and 1 IPv6 addresses as our system should work on IPv4 addresses only so put a filter
                     * based on AddressFamily.InterNetwork. After this filter only IPv4 addresses will be in variable "aryLocalAddr".
                     * Using first IPv4 address to bind data to it.
                     */
                    aryLocalAddr = (from g in ipEntry.AddressList
                                    where g.AddressFamily == AddressFamily.InterNetwork
                                    select g).ToArray();
                }
                catch (Exception ex)
                {
                    throw new Exception("Prana: Error trying to get local address.", ex);
                }

                // Verify we got an IP address. Tell the user if we did
                if (aryLocalAddr == null || aryLocalAddr.Length < 1)
                {
                    throw new Exception("Prana: Unable to get local address");
                }
                serverListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                /* http://jira.nirvanasolutions.com:8080/browse/PRANA-6497
                 * Reverting back to use first value in list so used aryLocalAddr[0] again.
                 * Initally it was aryLocalAddr[0], changed to aryLocalAddr[1] in revision no - 20170, correcting it back to aryLocalAddr[0]
                 * along with changes above
                 */

                _ipEndPoint = new IPEndPoint(aryLocalAddr[0], nPortListen);
                serverListener.Bind(_ipEndPoint);
                serverListener.Listen(1000);
                serverListener.BeginAccept(new AsyncCallback(WaitingForClient), serverListener);
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

        IPEndPoint _ipEndPoint;
        public IPEndPoint EndPoint
        {
            get { return _ipEndPoint; }
        }
        #endregion

        #region Message Handling
        void WaitingForClient(IAsyncResult ar)
        {
            try
            {
                Socket listener = (Socket)ar.AsyncState;
                SocketConnection newClient = null;
                lock (SessionData.lockClientSocket)
                {
                    if (!_isServerClosed)
                    {
                        newClient = new SocketConnection(listener.EndAccept(ar));
                        listener.BeginAccept(new AsyncCallback(WaitingForClient), listener);
                        newClient.SocketException += new ExceptionDelegate(newClient_SocketException);
                        newClient.MessageReceived += new MessageReceivedDelegate(newClient_MessageReceived);
                        newClient.Connect();
                    }
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        void newClient_MessageReceived(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                QueueMessage qMsg = e.Value;
                switch (qMsg.MsgType)
                {
                    case FIXConstants.MSGLogon:
                        HandleClientConnect(sender, qMsg);
                        break;
                    case FIXConstants.MSGLogout:
                        HandleClientDisConnect(sender, qMsg);
                        break;
                    case FIXConstants.MSGHeartbeat:

                        break;
                    case CustomFIXConstants.MSG_COUNTERPARTY_CONNECTIONSTATUS_REQUEST:

                        break;

                    default:
                        SocketConnection socketConn = (SocketConnection)sender;
                        HandlerFactory.SendToHandler(qMsg, socketConn.HandlerType);

                        if (LoggingConstants.LoggingEnabled)
                        {
                            Logger.LoggerWrite("MessageReceived:=" + qMsg.ToString(), LoggingConstants.CATEGORY_FLAT_FILE_ClientMessages);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        void _outComMgrQueue_MessageQueued(object sender, EventArgs<QueueMessage> e)
        {
            QueueMessage queueMessage = e.Value;
            try
            {
                IQueueProcessor processor = (IQueueProcessor)sender;

                if (queueMessage.UsersToSendDynamicData != null)
                {
                    string dynamicMSG;
                    //raturi: use for loop instead of foreach to prevent errors when collection is modified(user logs out)
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-6728
                    for (int i = ConnectedUsers.Count - 1; i >= 0; i--)
                    {
                        if (queueMessage.UsersToSendDynamicData.ContainsKey(ConnectedUsers[i]))
                        {
                            dynamicMSG = queueMessage.ToString(ConnectedUsers[i]);

                            Logger.LoggerWrite("Sending new data to User : " + ConnectedUsers[i], LoggingConstants.CATEGORY_GENERAL);
                            SocketConnection socket = SessionData.GetConnectedClientsSocket(ConnectedUsers[i]);
                            if (socket != null)
                            {
                                socket.SendMessage(dynamicMSG);
                            }
                        }
                    }
                }
                else
                {
                    string msg = queueMessage.ToString();
                    if (queueMessage.TradingAccountID != string.Empty)// send the message based on Trading Account
                    {
                        List<SocketConnection> listSockets = SessionData.GetConnectedClientsSockets(queueMessage.TradingAccountID);
                        //raturi: use for loop instead of foreach to prevent errors when collection is modified(user logs out)
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-6728
                        for (int i = listSockets.Count - 1; i >= 0; i--)
                        {
                            listSockets[i].SendMessage(msg);
                        }
                    }
                    else if (queueMessage.UserID != string.Empty)//// send the message based on UserID
                    {
                        SocketConnection socket = SessionData.GetConnectedClientsSocket(queueMessage.UserID);
                        if (socket != null)
                        {
                            socket.SendMessage(msg);
                        }
                    }
                    // Special handling done for expnl. If users are busy then we will skip sending messages to them.
                    else if (queueMessage.FreeUsers != null)
                    {
                        //raturi: use for loop instead of foreach to prevent errors when collection is modified(user logs out)
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-6728
                        for (int i = ConnectedUsers.Count - 1; i >= 0; i--)
                        {
                            if (queueMessage.FreeUsers.Contains(ConnectedUsers[i]))
                            {
                                Logger.LoggerWrite("Sending new data to User : " + ConnectedUsers[i], LoggingConstants.CATEGORY_GENERAL);
                                SocketConnection socket = SessionData.GetConnectedClientsSocket(ConnectedUsers[i]);
                                if (socket != null)
                                {
                                    socket.SendMessage(msg);
                                }
                            }
                        }
                    }
                    else// Send to All
                    {
                        List<SocketConnection> listSockets = SessionData.GetAllSockets(processor.HandlerType);
                        //raturi: use for loop instead of foreach to prevent errors when collection is modified(user logs out)
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-6728
                        for (int i = listSockets.Count - 1; i >= 0; i--)
                        {
                            listSockets[i].SendMessage(msg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                Logger.HandleException(new Exception("Problem in Com Mgr at while sending Message := " + queueMessage.ToString()), LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            // If disposing equals true, dispose all managed 
            // and unmanaged resources.
            if (disposing)
            {
                if (serverListener != null)
                {
                    serverListener.Dispose();
                }
            }
        }
        #endregion

        #region User Connection and Disconnection
        void HandleClientConnect(object sender, QueueMessage qMsg)
        {
            try
            {
                SocketConnection socketConn = (SocketConnection)sender;
                string message = qMsg.Message.ToString();

                ConnectionProperties connProperties = AdminMessageHandler.GetConnectionProperties(message);

                if (SessionData.ExposurePNLServiceID == connProperties.IdentifierID)
                {
                    connProperties.TradingAccounts = _tradingAccounts;
                }

                PranaMessage pranaMsgMonitoring = new PranaMessage(message);
                pranaMsgMonitoring.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ServiceEndPoint, socketConn.ConnProperties.ServerIPAddress + ":" + socketConn.ConnProperties.Port);

                if (Connected != null && SessionData.AddClientDetails(sender, connProperties))
                {
                    Connected(sender, new EventArgs<ConnectionProperties>(socketConn.ConnProperties));
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

        public void HandleClientConnectWCF(ConnectionProperties connectionProperties)
        {
            try
            {
                SessionData.AddClientDetailsWCF(connectionProperties);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        void HandleClientDisConnect(object sender, QueueMessage qMsg)
        {
            try
            {
                SocketConnection socketConn = (SocketConnection)sender;
                string message = qMsg.Message.ToString();
                PranaMessage PranaMsg = new PranaMessage(message);
                string userID = PranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CompanyUserID].Value;
                RemoveClientDetails(userID);
                if (socketConn.HandlerType != HandlerType.MonitoringServices)
                {
                    PranaMessage pranaMsgMonitoring = new PranaMessage(message);
                    pranaMsgMonitoring.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ServiceEndPoint, socketConn.ConnProperties.ServerIPAddress + ":" + socketConn.ConnProperties.Port);
                    QueueMessage qMsgMonitoring = new QueueMessage(pranaMsgMonitoring);
                    SendMsgToAllUsers(HandlerType.MonitoringServices, qMsgMonitoring);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        void RemoveClientDetails(string userID)
        {
            try
            {
                ConnectionProperties connProperties = SessionData.RemoveClientDetails(userID);
                if (connProperties != null)
                {
                    if (Disconnected != null)
                    {
                        Disconnected(this, new EventArgs<ConnectionProperties>(connProperties));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        public void HandleClientDisconnectWCF(string userID)
        {
            try
            {
                SessionData.RemoveClientDetailsWCF(userID);
                RemoveClient(userID);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        public bool RemoveClient(string username)
        {
            try
            {
                ConnectionProperties connProperties = SessionData.RemoveClient(username);
                if (connProperties != null)
                {
                    if (Disconnected != null)
                    {
                        Disconnected(this, new EventArgs<ConnectionProperties>(connProperties));
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
            return true;
        }

        /// <summary>
        /// Sends CounterParty Status Report to each Clients except Exposure PNL Calculator
        /// </summary>
        /// <param name="statusReport"></param>
        public void SendMsgToAllUsers(QueueMessage msg)
        {
            try
            {
                List<SocketConnection> listSocket = SessionData.GetAllSockets();
                //raturi: use for loop instead of foreach to prevent errors when collection is modified(user logs out)
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-6728
                for (int i = listSocket.Count - 1; i >= 0; i--)
                {
                    listSocket[i].SendMessage(msg.ToString());
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

        public void SendMsgToAllUsers(HandlerType handlerType, QueueMessage msg)
        {
            try
            {
                List<SocketConnection> listSocket = SessionData.GetAllSockets(handlerType);
                for (int i = listSocket.Count - 1; i >= 0; i--)
                {
                    listSocket[i].SendMessage(msg.ToString());
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
        /// sends to all users
        /// </summary>
        /// <param name="statusReport"></param>
        public void SendMsgToUser(QueueMessage msg, string userID)
        {
            SocketConnection socket = SessionData.GetConnectedClientsSocket(userID);
            if (socket != null)
                socket.SendMessage(msg.ToString());
        }

        /// <summary>
        /// DIsconnects all Clients
        /// </summary>
        public void DisConnectAllClients()
        {
            try
            {
                foreach (KeyValuePair<string, string> user in SessionData.ClientList)
                {
                    SocketConnection socketConn = SessionData.GetConnectedClientsSocket(user.Key);
                    if (socketConn != null && Disconnected != null)
                    {
                        Disconnected(this, new EventArgs<ConnectionProperties>(socketConn.ConnProperties));
                    }
                }
                SessionData.DisConnectAllClients();
                serverListener.Close();
                _isServerClosed = true;
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

        #region Exception
        void newClient_SocketException(object sender, EventArgs<Exception> e)
        {
            try
            {
                SocketConnection socketConn = (SocketConnection)sender;
                // if userID is null then socket connection will not be dipsosed and thus there will be a number of sockets alive in system...causing hang at last.
                // so changing this to identifier ID as it is assigned at the time when socket connection is created.
                if (socketConn.ConnProperties.IdentifierID != null)
                {
                    //clear the accounts locked by user
                    int userID;
                    if (UserExceptionDelegate != null && int.TryParse(socketConn.ConnProperties.IdentifierID, out userID))
                    {
                        UserExceptionDelegate(userID);
                    }
                    RemoveClientDetails(socketConn.ConnProperties.IdentifierID);
                }
                // Log the Message
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Removing " + socketConn.ConnProperties.IdentifierName, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
            }
            catch (Exception ex1)
            {
                Logger.HandleException(ex1, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        #endregion

        public List<string> ConnectedUsers
        {
            get
            {
                return SessionData.ConnectedUsers;
            }
        }

        public Dictionary<string, string> ConnectedClientIDNames
        {
            get { return SessionData.ClientList; }
        }

        public bool IsUserConnected(string userID)
        {
            return SessionData.ClientExist(userID);
        }
    }

    public delegate void MethodInvokeHandler();
    public delegate void StringListHandler(List<string> strArr);
}
