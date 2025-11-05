using System;
using System.Data;
using System.IO;
using Prana.Server.Core; 
using Prana.BusinessObjects; 
//using Prana.BusinessLogic; 
using System.Net.Sockets;
using System.Net;
using System.Configuration;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Prana.QueueManager;
using Prana.ServerClientCommon;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.FIX;
namespace Prana.Server.Core
{


    public class ServerTradeCommManager : IDisposable
	{
        #region  Public Variables
        public event MessageReceivedDelegate Connected;
        public event MessageReceivedDelegate Disconnected;
        #endregion

        #region Private Variables
        private static object lockClientSocket= new object();
        
        private bool bDisposed = false;

        private static Dictionary<string, SocketConnection> _clientSocketTable = null;
        private static Dictionary<string, string> _clientList =null ;
        private static Dictionary<string, List<string>> _dictTradingAccounts = null;
        
        /// <summary>
        /// 
        /// </summary>
        private static Dictionary<string, ExPNLSubscriptionType> _exPNLSubscriptionLookup = null;

        private ITradeQueueProcessor _inComMgrQueue;
        private ITradeQueueProcessor _outComMgrQueue;

        const string FORM_NAME = "Commnuication Manager : ";
        int nPortListen = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["OrderRequestPort"]);
        Socket serverListener;
        bool _isServerClosed = false;
        static ServerTradeCommManager _communicationManager = null;
        private string _exposurePNLServiceID = string.Empty;
        #endregion
        private bool _loggingEnabled = false;
        List<string> _tradingAccounts = null;
        #region Initialization

        public static ServerTradeCommManager GetInstance()
		{
			if ( _communicationManager == null )
			{
				_communicationManager = new ServerTradeCommManager();
			}
			return _communicationManager;
        }

		private ServerTradeCommManager()
		{
			try
			{

			}
				#region Catch
			catch(Exception ex)
			{
				// Invoke our policy that is responsible for making sure no secure information
				// gets out of our layer.
				bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                //if (rethrow)
                //{
                //    throw;			
                //}
			}				
			#endregion
        }

        public void Initialise(ITradeQueueProcessor inComMgrQueue, ITradeQueueProcessor outComMgrQueue, List<string> tradingAccounts)
        {
            _tradingAccounts = tradingAccounts;
            _isServerClosed = false;
            IPAddress[] aryLocalAddr = null;
            String strHostName = "";
            try
            {
                _clientSocketTable = new Dictionary<string, SocketConnection>();
                _dictTradingAccounts = new Dictionary<string, List<string>>();
                _clientList = new Dictionary<string, string>();

                _exPNLSubscriptionLookup = new Dictionary<string, ExPNLSubscriptionType>();

                _inComMgrQueue = inComMgrQueue;
                _outComMgrQueue = outComMgrQueue;
                _outComMgrQueue.MessageQueued += new PranaMessageReceivedHandler(_outComMgrQueue_MessageQueued);
                // NOTE: DNS lookups are nice and all but quite time consuming.
                strHostName = Dns.GetHostName();
                IPHostEntry ipEntry = Dns.GetHostByName(strHostName);
                aryLocalAddr = ipEntry.AddressList;
                _loggingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["LOGGINGENABLED"]);
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
            serverListener.Bind(new IPEndPoint(aryLocalAddr[0], nPortListen));
            serverListener.Listen(1000);
            serverListener.BeginAccept(new AsyncCallback(WaitingForClient), serverListener);
            _exposurePNLServiceID=ConfigurationManager.AppSettings[PranaServerConstants.EXPOSURE_PNL_SERVICES];
        }
        
      

        void WaitingForClient(IAsyncResult ar)
        {
            try
            {
                Socket listener = (Socket)ar.AsyncState;
                SocketConnection newClient = null;
                lock (lockClientSocket)
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
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

            }
        }

        void newClient_MessageReceived(object sender, string message)
        {
            try
            {
                //message = message.Substring(0, message.Length - 2);
                PranaMessage PranaMsg = new PranaMessage(message);
                switch (PranaMsg.MessageType)
                {

                    case FIXConstants.MSGLogon:
                        HandleClientConnect(sender, PranaMsg);
                        break;
                    case FIXConstants.MSGLogout:
                        HandleClientDisConnect(PranaMsg);
                        break;
                    //case FIXConstants.MSGHeartbeat:
                    //    HandleHeartBeatReceived(PranaMsg);
                    //    break;
                    case PranaMessageConstants.MSG_COUNTERPARTY_CONNECTIONSTATUS_REQUEST:
                        // CounterPartyStatusReportRequest(this, e);

                        break;

                    case PranaMessageConstants.MSG_ExpPNLSubscription:
                        HandleExPnlSubscription(message);
                        break;
                    default:
                       // QueueMessage queueMessage = new QueueMessage(PranaMsg.MessageType, PranaMsg, int.MinValue);
                        _inComMgrQueue.SendMessage(PranaMsg);
                        if (_loggingEnabled)
                        {
                            Logger.Write("MessageReceived:=" + PranaMsg.ToString(), ApplicationConstants.CATEGORY_FLAT_FILE_ClientMessages);
                        }
                        //MessageReceived(this, e);
                        break;

                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

            }
        }

        

        #endregion

        #region Message Handling

        

        void _outComMgrQueue_MessageQueued(object sender,PranaMessage  queueMessage)
        {
            try
            {
                string msg =  queueMessage.ToString();
                if (queueMessage.TradingAccountID != string.Empty)
                {
                    string tradingAcctID = queueMessage.TradingAccountID;
                    lock (lockClientSocket)
                    {
                        if (_dictTradingAccounts.ContainsKey(tradingAcctID))
                        {
                            List<string> connectedUsersInTradingAccount = _dictTradingAccounts[tradingAcctID];
                            foreach (string key in connectedUsersInTradingAccount)
                            {
                                _clientSocketTable[key].SendMessage(msg);
                            }
                        }

                    }
                }
                else
                {
                    if (!queueMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_CompanyUserID))
                    {
                        bool isAnyclientConneted = false;
                        if (queueMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_OriginatorType))
                        {
                            foreach (KeyValuePair<string, SocketConnection> connClients in _clientSocketTable)
                            {
                                isAnyclientConneted = true;

                                connClients.Value.SendMessage(msg);
                            }
                            if (!isAnyclientConneted)
                            {
                                throw new Exception("no client connected");
                            }
                        }
                    }
                    else
                    {
                        string TAGUserRequestID = queueMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CompanyUserID];
                        lock (lockClientSocket)
                        {
                            if (_clientSocketTable.ContainsKey(TAGUserRequestID))
                            {

                                _clientSocketTable[TAGUserRequestID].SendMessage(msg);
                            }
                        }

                    }

                }
                
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                ExceptionPolicy.HandleException(new Exception("Problem in Com Mgr at while sending Message := " + queueMessage.ToString()), ApplicationConstants.POLICY_LOGANDSHOW);
                //DisConnectAllClients();
            }

        }

       
        
        //void newClient_CounterPartyStatusReportRequest(object sender, MessageEventArgs e)
        //{
        //    CounterPartyStatusReportRequest(this, e);
        //}

        //void HandleHeartBeatReceived(PranaMessage PranaMsg)
        //{
        //    lock (lockClientSocket)
        //    {
        //        string userID = PranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CompanyUserID];
        //        if (_clientSocketTable.ContainsKey(userID))
        //        {
        //            _clientSocketTable[userID].LastHeartBeatReceiveTime = System.DateTime.Now.ToUniversalTime();
        //        }
        //    }
        //}

        void HandleClientDisConnect(PranaMessage PranaMsg)
        {
            try
            {                
                string userID = PranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CompanyUserID];
               // string userName = PranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CompanyUserName];
                RemoveClientDetails(userID);
               
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
                
           // }
            
            
        }

        void HandleClientConnect(object sender, PranaMessage PranaMsg)
        {
            string userID = PranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CompanyUserID];
            string userName = PranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CompanyUserName];
            List<string> tradingAccounts = null;
            SocketConnection conn = (SocketConnection)sender;
            conn.UserID = userID;
            if (_exposurePNLServiceID == userID)
            {
                // Get All Trading Accounts 
                tradingAccounts = _tradingAccounts;
            }
                // Get Trading Accounts from Message
            else
            {
                string strtradingAccts = PranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_TradingAccountID];
                tradingAccounts = Prana.Utilities.MiscUtilities.GeneralUtilities.GetListFromString(strtradingAccts, Prana.BusinessObjects.Seperators.SEPERATOR_1);
                
            }
            if (!ClientExist(userID))
            {
                   
                    AddClientDetails(userID, userName, tradingAccounts, conn);
                    if (Connected != null)
                    {
                        Connected(this, userID + Seperators.SEPERATOR_1 + userName);
                    }
            }
            else
            {
                conn.DisconnectSocket();
            }
        }

        private void HandleExPnlSubscription(string message)
        {
            string userID = string.Empty;
            try
            {
                //TODO
                //ExPNLSubscriptionType subscriptionType = Transformer.FromExPnlSubscriptionMsg(message, out userID);

                //if (userID != string.Empty)
                //{
                //    if (_exPNLSubscriptionLookup.ContainsKey(userID))
                //    {
                //        _exPNLSubscriptionLookup[userID] = subscriptionType;
                //    }
                //    else
                //    {
                //        _exPNLSubscriptionLookup.Add(userID, subscriptionType);
                //    }
                //}
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public ExPNLSubscriptionType GetExPNLUserSubscriptionType(string userID)
        {
            ExPNLSubscriptionType result = ExPNLSubscriptionType.None;
            try
            {
                if (_exPNLSubscriptionLookup.ContainsKey(userID))
                {
                    result = _exPNLSubscriptionLookup[userID];
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return result;
        }
            #endregion

        #region Clearing
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);

        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed.
        private void Dispose(bool disposing)
        {


            // Check to see if Dispose has already been called.
            if (!this.bDisposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                   
                }
            }
            bDisposed = true;
        }

        #endregion

        
        /// <summary>
        /// Sends CounterParty Status Report to each Clients except Exposure PNL Calculator
        /// </summary>
        /// <param name="statusReport"></param>

        public void SendCounterPartyConnectionStatus(string statusReport)
        {
            lock (lockClientSocket)
            {
                
                    foreach (KeyValuePair<string, SocketConnection> hSocket in _clientSocketTable)
                    {
                        if (hSocket.Key != _exposurePNLServiceID)
                            hSocket.Value.SendMessage(statusReport);
                    }
               
            }
        }
        /// <summary>
        /// Sends CounterParty Connection update to Connected User
        /// </summary>
        /// <param name="statusReport"></param>
        /// <param name="userID"></param>
        public void SendCounterPartyConnectionStatus(string statusReport, string  userID)
        {
            lock (lockClientSocket)
            {
                if(userID != _exposurePNLServiceID)
                _clientSocketTable[userID].SendMessage(statusReport);
            }
        }

        public void SendCachedDropCopies(string dropCopy, string userID)
        {
            lock (lockClientSocket)
            {
                if (userID != _exposurePNLServiceID)
                    _clientSocketTable[userID].SendMessage(dropCopy);
            }
        }

        #region User Connection and Disconnection
        private static bool AddClientDetails(string userID,string userName, List<string> tradingAccounts,SocketConnection conn)
        {
            try
            {
                lock (lockClientSocket)
                {
                    _clientSocketTable.Add(userID, conn);
                    _clientList.Add(userID, userName);
                    conn.UserID = userID;
                    conn.UserName = userName;
                    // Add Trading Accounts
                    foreach (string tradingAccount in tradingAccounts)
                    {
                        if (!_dictTradingAccounts.ContainsKey(tradingAccount))
                        {
                            List<string> userList = new List<string>();
                            userList.Add(userID);
                            _dictTradingAccounts.Add(tradingAccount, userList);
                        }
                        else
                        {
                            if(!_dictTradingAccounts[tradingAccount].Contains(userID))
                            _dictTradingAccounts[tradingAccount].Add(userID);
                        }
                    }

                    return true;
                   
                }
            }
            catch (Exception ex)
            {
                Exception newEX = new Exception("Error in Adding Client :" + userID +" Error" +ex.Message);
                throw newEX;
            }
            
        }
        public bool  RemoveClient(string username)
        {
            foreach (KeyValuePair<string, string> client in _clientList)
            {
                if (client.Value == username)
                {
                    RemoveClientDetails(client.Key);
                    return true;
                }

            }
            return false;
        }
        private  void  RemoveClientDetails(string userID)
        {
            try
            {
                lock (lockClientSocket)
                {
                    if (_clientList != null)
                    {
                        if (_clientList.ContainsKey(userID))
                        {
                            if (Disconnected != null)
                            {

                                Disconnected(this, userID + Seperators.SEPERATOR_1 + _clientList[userID]);
                            }
                            _clientSocketTable[userID].DisconnectSocket();
                           _clientSocketTable.Remove(userID);
                            _clientList.Remove(userID);
                            foreach (KeyValuePair<string, List<string>> keyValueTradingAccount in _dictTradingAccounts)
                            {
                                if (keyValueTradingAccount.Value.Contains(userID))
                                {
                                    keyValueTradingAccount.Value.Remove(userID);
                                }
                            }
                        }
                    }
                    
                   
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        private static bool ClientExist(string userID)
        {
            return _clientList.ContainsKey(userID);
        }
        /// <summary>
        /// DIsconnects all Clients
        /// </summary>
        public void DisConnectAllClients()
        {
            try
            {
                lock (lockClientSocket)
                {
                    if (_clientList !=null)
                    {
                        foreach (KeyValuePair<string, string> user in _clientList)
                        {
                            _clientSocketTable[user.Key].DisconnectSocket();

                        }
                    }

                    
                    serverListener.Close();
                    _isServerClosed = true;
                    _clientSocketTable = null;
                    _dictTradingAccounts = null;
                    _clientList = null;
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

       

        #region Exception 
        void newClient_SocketException(object sender,Exception ex)
        {
            try
            {
                SocketConnection socketConn = (SocketConnection)sender;
                RemoveClientDetails(socketConn.UserID);

                // Log the Message
                ExceptionPolicy.HandleException(new Exception("Removing " + socketConn.UserName ), ApplicationConstants.POLICY_LOGANDSHOW);
            }
            catch (Exception ex1)
            {
                ExceptionPolicy.HandleException(ex1, ApplicationConstants.POLICY_LOGANDSHOW);
            }
        }
        #endregion

        public List<string> ConnectedUsers
        {
            get { 
                List<string> _users=new List<string>();
                if (_clientList == null)
                {
                    return _users;
                }

                foreach (KeyValuePair<string, string> user in _clientList)
                {
                    _users.Add(user.Key);
                }
                return _users ; 
            }
        }



    }

   

    




}
