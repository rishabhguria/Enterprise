using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.WCFConnectionMgr
{
    public class ConnectionMgr
    {
        private static Dictionary<string, List<IConnectionNotify>> _dictClientConnections = new Dictionary<string, List<IConnectionNotify>>();
        private static readonly object _dictClientConnectionsLock = new object();
        private static Dictionary<string, List<IConnectionNotify>> _dictServiceConnections = new Dictionary<string, List<IConnectionNotify>>();
        private static readonly object _dictServiceConnectionsLock = new object();
        private static Dictionary<string, PranaInternalConstants.ConnectionStatus> _dictServiceConnectionsStatus = new Dictionary<string, PranaInternalConstants.ConnectionStatus>();

        internal static void CreateConnection(String endpointConfigurationName, IConnectionNotify proxy)
        {
            try
            {
                string ipPortKey = GetIPPortKey(endpointConfigurationName);

                if (!string.IsNullOrEmpty(ipPortKey) && !ipPortKey.Equals(":0"))
                {
                    lock (_dictClientConnectionsLock)
                    {
                        if (_dictClientConnections.ContainsKey(ipPortKey))
                        {
                            if (!_dictClientConnections[ipPortKey].Contains(proxy))
                            {
                                _dictClientConnections[ipPortKey].Add(proxy);
                            }
                        }
                        else
                        {
                            List<IConnectionNotify> listOfConn = new List<IConnectionNotify>();
                            listOfConn.Add(proxy);
                            _dictClientConnections.Add(ipPortKey, listOfConn);
                        }
                    }
                }
                else
                {
                    string containerServiceName = GetContainerServiceName(endpointConfigurationName);

                    lock (_dictServiceConnectionsLock)
                    {
                        if (_dictServiceConnections.ContainsKey(containerServiceName))
                        {
                            if (!_dictServiceConnections[containerServiceName].Contains(proxy))
                            {
                                _dictServiceConnections[containerServiceName].Add(proxy);
                            }
                        }
                        else
                        {
                            List<IConnectionNotify> listOfConn = new List<IConnectionNotify>();
                            listOfConn.Add(proxy);
                            _dictServiceConnections.Add(containerServiceName, listOfConn);
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

        private static string GetIPPortKey(string endpointConfigurationName)
        {
            try
            {
                if (endpointConfigurationName.StartsWith("Pricing"))
                {
                    return GetIPPortKey(ClientAppConfiguration.PricingServer.IpAddress, ClientAppConfiguration.PricingServer.Port.ToString());
                }
                else if (endpointConfigurationName.StartsWith("Trade"))
                {
                    return GetIPPortKey(ClientAppConfiguration.TradeServer.IpAddress, ClientAppConfiguration.TradeServer.Port.ToString());
                }
                else if (endpointConfigurationName.StartsWith("Expnl"))
                {
                    return GetIPPortKey(ClientAppConfiguration.ExpnlServer.IpAddress, ClientAppConfiguration.ExpnlServer.Port.ToString());
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
            return string.Empty;
        }

        private static string GetContainerServiceName(string endpointConfigurationName)
        {
            try
            {
                if (endpointConfigurationName.StartsWith("Pricing"))
                {
                    return "Pricing";
                }
                else if (endpointConfigurationName.StartsWith("Trade"))
                {
                    return "Trade";
                }
                else if (endpointConfigurationName.StartsWith("Expnl"))
                {
                    return "Expnl";
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
            return string.Empty;
        }

        private static string GetIPPortKey(string ipAddress, string port)
        {
            return ipAddress + ":" + port;
        }

        public static void SetClientConnectionStatus(ConnectionProperties con, PranaInternalConstants.ConnectionStatus status)
        {
            try
            {
                string ipPortKey = GetIPPortKey(con.ServerIPAddress, con.Port.ToString());
                IConnectionNotify[] connectionsClone = null;

                lock (_dictClientConnectionsLock)
                {
                    if (_dictClientConnections.ContainsKey(ipPortKey))
                    {
                        List<IConnectionNotify> connections = _dictClientConnections[ipPortKey];
                        connectionsClone = new IConnectionNotify[connections.Count];
                        connections.CopyTo(connectionsClone);
                    }
                }

                // tell each proxy about connection/disconnection
                if (connectionsClone != null)
                {
                    foreach (IConnectionNotify connection in connectionsClone)
                    {
                        connection.Notify(status);
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

        internal static void SetServiceConnectionStatus(string endpointConfigurationName, PranaInternalConstants.ConnectionStatus status)
        {
            try
            {
                string containerServiceName = GetContainerServiceName(endpointConfigurationName);
                if (_dictServiceConnectionsStatus.ContainsKey(containerServiceName))
                {
                    _dictServiceConnectionsStatus[containerServiceName] = status;
                }
                else
                {
                    _dictServiceConnectionsStatus.Add(containerServiceName, status);
                }

                IConnectionNotify[] connectionsClone = null;

                lock (_dictServiceConnectionsLock)
                {
                    if (_dictServiceConnections.ContainsKey(containerServiceName))
                    {
                        List<IConnectionNotify> connections = _dictServiceConnections[containerServiceName];
                        connectionsClone = new IConnectionNotify[connections.Count];
                        connections.CopyTo(connectionsClone);
                    }
                }

                // tell each proxy about connection/disconnection
                if (connectionsClone != null)
                {
                    foreach (IConnectionNotify connection in connectionsClone)
                    {
                        connection.Notify(status);
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

        internal static PranaInternalConstants.ConnectionStatus GetServiceConnectionStatus(string endpointConfigurationName)
        {
            try
            {
                string containerServiceName = GetContainerServiceName(endpointConfigurationName);
                if (_dictServiceConnectionsStatus.ContainsKey(containerServiceName))
                {
                    return _dictServiceConnectionsStatus[containerServiceName];
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
            return PranaInternalConstants.ConnectionStatus.DISCONNECTED;
        }
    }
}
