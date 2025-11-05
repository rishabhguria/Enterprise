using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Prana.SocketCommunication
{
    class SessionData
    {
        public static readonly object lockClientSocket = new object();
        private static Dictionary<string, SocketConnection> _clientSocketTable = null;

        private static Dictionary<string, string> _clientList = null;
        private static Dictionary<string, string> _disconnectedUserList = null;
        private static Dictionary<string, List<string>> _dictTradingAccounts = null;
        static List<string> _users = new List<string>();
        public static readonly string ExposurePNLServiceID = ConfigurationManager.AppSettings[PranaServerConstants.EXPOSURE_PNL_SERVICES];

        static SessionData()
        {
            _clientSocketTable = new Dictionary<string, SocketConnection>();
            _dictTradingAccounts = new Dictionary<string, List<string>>();
            _clientList = new Dictionary<string, string>();
            _disconnectedUserList = new Dictionary<string, string>();
        }

        public static SocketConnection GetConnectedClientsSocket(string userID)
        {
            if(userID != null)
            {
                lock (lockClientSocket)
                {
                    if (_clientSocketTable.ContainsKey(userID))
                    {
                        return _clientSocketTable[userID];
                    }
                }
            }
            return null;
        }

        public static List<SocketConnection> GetConnectedClientsSockets(string tradingAccount)
        {
            List<SocketConnection> sockectConn = new List<SocketConnection>();

            lock (lockClientSocket)
            {
                if (_dictTradingAccounts.ContainsKey(tradingAccount))
                {
                    List<string> connectedUsersInTradingAccount = _dictTradingAccounts[tradingAccount];
                    foreach (string userID in connectedUsersInTradingAccount)
                    {
                        sockectConn.Add(_clientSocketTable[userID]);
                    }
                }
            }
            return sockectConn;
        }

        public static List<string> ConnectedUsers
        {
            get
            {
                return _users;
            }
        }

        public static bool ClientExist(string userID)
        {
            return _clientList.ContainsKey(userID);
        }

        public static ConnectionProperties RemoveClient(string username)
        {
            foreach (KeyValuePair<string, string> client in _clientList)
            {
                if (client.Value == username)
                {
                    return RemoveClientDetails(client.Key);
                }
            }
            return null;
        }

        public static ConnectionProperties RemoveClientDetails(string userID)
        {
            ConnectionProperties connProperties = null;
            try
            {
                lock (lockClientSocket)
                {
                    if (_clientList.ContainsKey(userID))
                    {
                        if (_clientSocketTable.ContainsKey(userID))
                        {
                            connProperties = _clientSocketTable[userID].ConnProperties;
                            _clientSocketTable[userID].DisConnect("");
                            _clientSocketTable.Remove(userID);
                            _clientList.Remove(userID);
                            if (!_disconnectedUserList.ContainsKey(connProperties.IdentifierID))
                                _disconnectedUserList.Add(connProperties.IdentifierID, connProperties.IdentifierName);
                            foreach (KeyValuePair<string, List<string>> keyValueTradingAccount in _dictTradingAccounts)
                            {
                                if (keyValueTradingAccount.Value.Contains(userID))
                                {
                                    keyValueTradingAccount.Value.Remove(userID);
                                }
                            }
                        }
                    }
                    if (_users.Contains(userID))
                        _users.Remove(userID);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return connProperties;
        }

        public static void RemoveClientDetailsWCF(string userID)
        {
            lock (lockClientSocket)
            {
                _clientList.Remove(userID);
            }
        }

        public static bool AddClientDetails(object sender, ConnectionProperties connProperties)
        {
            try
            {
                SocketConnection conn = (SocketConnection)sender;
                conn.ConnProperties.IdentifierID = connProperties.IdentifierID;
                conn.ConnProperties.IdentifierName = connProperties.IdentifierName;
                conn.ConnProperties.HandlerType = connProperties.HandlerType;
                lock (lockClientSocket)
                {
                    if (ClientExist(connProperties.IdentifierID))
                    {
                        conn.DisConnect("Duplicate User" + connProperties.IdentifierName);
                        return false;
                    }
                    _clientSocketTable.Add(connProperties.IdentifierID, conn);
                    _clientList.Add(connProperties.IdentifierID, connProperties.IdentifierName);
                    conn.UserID = connProperties.IdentifierID;
                    conn.UserName = connProperties.IdentifierName;
                    conn.HandlerType = connProperties.HandlerType;

                    // Add Trading CashAccounts
                    if (connProperties.TradingAccounts != null)
                    {
                        foreach (string tradingAccount in connProperties.TradingAccounts)
                        {
                            if (!_dictTradingAccounts.ContainsKey(tradingAccount))
                            {
                                List<string> userList = new List<string>();
                                userList.Add(connProperties.IdentifierID);
                                _dictTradingAccounts.Add(tradingAccount, userList);
                            }
                            else
                            {
                                if (!_dictTradingAccounts[tradingAccount].Contains(connProperties.IdentifierID))
                                    _dictTradingAccounts[tradingAccount].Add(connProperties.IdentifierID);
                            }
                        }
                    }
                    if (!_users.Contains(conn.UserID))
                        _users.Add(conn.UserID);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool AddClientDetailsWCF(ConnectionProperties connectionProperties)
        {
            lock (lockClientSocket)
            {
                if (!_clientList.ContainsKey(connectionProperties.IdentifierID))
                {
                    _clientList.Add(connectionProperties.IdentifierID, connectionProperties.IdentifierName);
                    return true;
                }
                return false;
            }
        }

        public static void DisConnectAllClients()
        {
            try
            {
                lock (lockClientSocket)
                {
                    if (_clientList != null)
                    {
                        foreach (KeyValuePair<string, string> user in _clientList)
                        {
                            if (_clientSocketTable.ContainsKey(user.Key))
                                _clientSocketTable[user.Key].DisConnect("");
                        }
                        _clientList.Clear();
                    }
                    _clientSocketTable.Clear();
                    _dictTradingAccounts.Clear();
                    _users = new List<string>();
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

        public static Dictionary<string, string> ClientList
        {
            get { return _clientList; }
        }

        public static List<SocketConnection> GetAllSockets(HandlerType handlerType)
        {
            List<SocketConnection> listSocket = new List<SocketConnection>();
            lock (lockClientSocket)
            {
                foreach (KeyValuePair<string, SocketConnection> hSocket in _clientSocketTable)
                {
                    if (hSocket.Value.HandlerType == handlerType)
                    {
                        listSocket.Add(hSocket.Value);
                    }
                }
            }
            return listSocket;
        }

        public static List<SocketConnection> GetAllSockets()
        {
            List<SocketConnection> listSocket = new List<SocketConnection>();
            lock (lockClientSocket)
            {
                foreach (KeyValuePair<string, SocketConnection> hSocket in _clientSocketTable)
                {
                    listSocket.Add(hSocket.Value);
                }
            }
            return listSocket;
        }
    }
}