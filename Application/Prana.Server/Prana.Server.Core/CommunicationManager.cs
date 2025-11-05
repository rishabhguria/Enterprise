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

namespace Prana.Server.Core
{


    public class CommunicationManager : IDisposable
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
        private static Dictionary<string, string> _exPNLSubscriptionLookup = null;

        private IQueueProcessor _inComMgrQueue;
        private IQueueProcessor _outComMgrQueue;

        int nPortListen = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["OrderRequestPort"]);
        Socket serverListener;
        bool _isServerClosed = false;
        static CommunicationManager _communicationManager = null;
        private string _exposurePNLServiceID = string.Empty;
        #endregion
        private bool _loggingEnabled = false;
        List<string> _tradingAccounts = null;

        public event MethodInvokeHandler RefreshExPNLData;
        #region Initialization

        public static CommunicationManager GetInstance()
		{
			if ( _communicationManager == null )
			{
				_communicationManager = new CommunicationManager();
			}
			return _communicationManager;
        }

		private CommunicationManager()
		{
			try
			{
                
              

			}
				#region Catch
			catch(Exception ex)
			{
				// Invoke our policy that is responsible for making sure no secure information
				// gets out of our layer.
				bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                //if (rethrow)
                //{
                //    throw;			
                //}
			}				
			#endregion
        }

        public void Initialise(IQueueProcessor inComMgrQueue, IQueueProcessor outComMgrQueue, List<string> tradingAccounts)
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

                _exPNLSubscriptionLookup = new Dictionary<string, string>();

                _inComMgrQueue = inComMgrQueue;
                _outComMgrQueue = outComMgrQueue;
                _outComMgrQueue.MessageQueued += new MessageReceivedHandler(_outComMgrQueue_MessageQueued);
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
                ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

            }
        }

        void newClient_MessageReceived(object sender, string message)
        {
            try
            {
                //message = message.Substring(0, message.Length - 2);

                switch (PranaMessageFormatter.GetMessageType(message))
                {

                    case FIXConstants.MSGLogon:
                        HandleClientConnect(sender, message);
                        break;
                    case FIXConstants.MSGLogout:
                        HandleClientDisConnect(message);
                        break;
                    //case FIXConstants.MSGHeartbeat:
                    //    HandleHeartBeatReceived(message);
                    //    break;
                    case PranaMessageConstants.MSG_COUNTERPARTY_CONNECTIONSTATUS_REQUEST:
                        // CounterPartyStatusReportRequest(this, e);

                        break;

                    case PranaMessageConstants.MSG_ExpPNLSubscription:
                        HandleExPnlSubscription(message);
                        break;

                    case PranaMessageConstants.MSG_ExpPNLRefreshData:
                        HandleExPnlRefreshData(message);
                        break;
                    default:
                        QueueMessage queueMessage = new QueueMessage(PranaMessageFormatter.GetMessageType(message), message,int.MinValue);
                        _inComMgrQueue.SendMessage(queueMessage);
                        if (_loggingEnabled)
                        {
                            Logger.Write("MessageReceived:=" + queueMessage.ToString(), ApplicationConstants.CATEGORY_FLAT_FILE_ClientMessages);
                        }
                        //MessageReceived(this, e);
                        break;

                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

            }
        }

        

        #endregion

        #region Message Handling

        

        void _outComMgrQueue_MessageQueued(object sender,QueueMessage  queueMessage)
        {
            try
            {

                string msg =  queueMessage.Message.ToString();
                string msgType = PranaMessageFormatter.GetMessageType(msg);
                if (queueMessage.TradingAccountID != int.MinValue)
                {
                    lock (lockClientSocket)
                    {
                        if (_dictTradingAccounts.ContainsKey(queueMessage.TradingAccountID.ToString()))
                        {
                            List<string> connectedUsersInTradingAccount = _dictTradingAccounts[queueMessage.TradingAccountID.ToString()];
                            foreach (string key in connectedUsersInTradingAccount)
                            {
                                _clientSocketTable[key].SendMessage(msg);
                            }
                        }

                    }
                }
                else
                {
                      
                    lock (lockClientSocket)
                    {
                        if (_clientSocketTable.ContainsKey(queueMessage.UserID.ToString()))
                        {
                            _clientSocketTable[queueMessage.UserID.ToString()].SendMessage(msg);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                ExceptionPolicy.HandleException(new Exception("Problem in Com Mgr at while sending Message := " + queueMessage.ToString()), ApplicationConstants.POLICY_LOGANDSHOW);
                //DisConnectAllClients();
            }

        }

       
        
        //void newClient_CounterPartyStatusReportRequest(object sender, MessageEventArgs e)
        //{
        //    CounterPartyStatusReportRequest(this, e);
        //}

        //void HandleHeartBeatReceived(string message)
        //{
        //    lock (lockClientSocket)
        //    {
        //        string userID = message.Split(',')[1].ToString();
        //        if (_clientSocketTable.ContainsKey(userID))
        //        {
        //            _clientSocketTable[userID].LastHeartBeatReceiveTime = System.DateTime.Now.ToUniversalTime();
                    
        //        }
        //    }
        //}         

        void HandleClientDisConnect(string message)
        {
            try
            {
                string userID = message.ToString().Split(',')[1];
                string userName = message.ToString().Split(',')[2];
                RemoveClientDetails(userID);
               
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
            }
                
           // }
            
            
        }

        void HandleClientConnect(object sender,string message)
        {

            string[] data = message.Split(Seperators.SEPERATOR_2);
            List<string> tradingAccounts = new List<string>();
            string  userID = data[1];
            string userName = data[2];
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
                for (int i = 3; i < data.Length; i++)
                {
                    tradingAccounts.Add(data[i]);
                }
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

        public event MethodInvokeHandler SendLatestDataToUser;
        private void HandleExPnlSubscription(string message)
        {
            string userID ;
            int subscriptionType;
            int exPNLDataType;

            try
            {
                PranaMessageFormatter.FromExPnlSubscriptionMsg(message, out userID, out subscriptionType, out exPNLDataType);
                string subscriptionStr = subscriptionType + Seperators.SEPERATOR_2.ToString()  + exPNLDataType;

                if (userID != string.Empty)
                {
                    if (_exPNLSubscriptionLookup.ContainsKey(userID))
                    {
                        _exPNLSubscriptionLookup[userID] = subscriptionStr;
                    }
                    else
                    {
                        _exPNLSubscriptionLookup.Add(userID, subscriptionStr);
                    }
                }

                if (SendLatestDataToUser != null)
                {
                    SendLatestDataToUser();
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
        }


        private void HandleExPnlRefreshData(string message)
        {
            if (RefreshExPNLData != null)
            {
                RefreshExPNLData();
            }
        }

        public void GetExPNLUserSubscriptionType(string userID, out ExPNLSubscriptionType subscriptionType, out ExPNLData dataType)
        {
            string result = string.Empty;
            subscriptionType = ExPNLSubscriptionType.None;
            dataType = ExPNLData.None;

            try
            {
                if (_exPNLSubscriptionLookup.ContainsKey(userID))
                {
                    result = _exPNLSubscriptionLookup[userID];
                    string[] subscriptionArr = result.Split(Seperators.SEPERATOR_2 );

                    subscriptionType = (ExPNLSubscriptionType)(Convert.ToInt32(subscriptionArr[0]));
                    dataType = (ExPNLData)(Convert.ToInt32(subscriptionArr[1]));
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
                            if (!_dictTradingAccounts[tradingAccount].Contains(userID))
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
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
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
                            if (Disconnected != null)
                            {
                                string disconnectedClient = user.Key + Seperators.SEPERATOR_1 + user.Value;
                                Disconnected(this, disconnectedClient);
                            }
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
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

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

   

    

    public delegate void MethodInvokeHandler();


}
