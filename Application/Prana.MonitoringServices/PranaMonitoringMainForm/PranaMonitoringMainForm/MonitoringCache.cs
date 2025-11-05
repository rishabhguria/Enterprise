using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.Utilities.MiscUtilities;
using Prana.Interfaces;
using Prana.SocketCommunication;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using Prana.LogManager;

namespace Prana.MonitoringServices
{
    public class MonitoringCache
    {
        private static MonitoringCache _monitoringCache;
        static PranaBinaryFormatter binaryFormatter = new PranaBinaryFormatter();
        public delegate void RefreshDataHandler(string ipAddress, int port);
        public delegate void ServerAdditionHandler(MonitoringConnection conn);
        public event RefreshDataHandler Refresh;
        XmlSerializer SerializerObj = new XmlSerializer(typeof(List<MonitoringConnection>));

        //name of machine + each Monitoring Connection(All Ports)
        Dictionary<string, MonitoringConnection> _dictMonitoring = new Dictionary<string, MonitoringConnection>();

        // ipportkey + each Port Connection
        Dictionary<string, ServiceClient> _dictMachines = new Dictionary<string, ServiceClient>();

        //ipportkey+ Connection Object
        private Dictionary<string, ICommunicationManager> _dictconnList = new Dictionary<string, ICommunicationManager>();

        Dictionary<string, List<string>> _errorMessages = new Dictionary<string, List<string>>();

        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MonitoringCache));
        static MonitoringCache()
        {
            if (_monitoringCache == null)
            {
                _monitoringCache = new MonitoringCache();
            }
        }

        public static MonitoringCache GetInstance
        {
            get
            {
                return _monitoringCache;
            }
        }

        public void Initlise()
        {
            string path = GetPath();
            if (File.Exists(path))
            {
                // Create a new file stream for reading the XML file
                FileStream ReadFileStream = new FileStream(GetPath(), FileMode.Open, FileAccess.Read, FileShare.Read);

                // Load the object saved above by using the Deserialize function
                List<MonitoringConnection> _listOfConnections = (List<MonitoringConnection>)SerializerObj.Deserialize(ReadFileStream);
                foreach (MonitoringConnection conn in _listOfConnections)
                {
                    AddIpNode(conn);
                }

                // Cleanup
                ReadFileStream.Close();
            }
            else
            {
                File.Create(path);

            }
        }

        private string GetPath()
        {

            return System.Windows.Forms.Application.StartupPath + "\\XmlConnection.xml";

        }

        public List<ServiceClient> AddIpNode(MonitoringConnection conn)
        {
            string[] ports = conn.Ports.Split(',');
            List<ServiceClient> listOfConnectionTrails = new List<ServiceClient>();

            try
            {
                String[] serviceNames = conn.ServiceNames.Split(',');
                if (conn.Name == string.Empty)
                {
                    conn.Name = conn.IpAddress;
                }

                if (!_dictMonitoring.ContainsKey(conn.Name))
                {
                    _dictMonitoring.Add(conn.Name, conn);

                    int count = 0;
                    foreach (string port in ports)
                    {
                        ServiceClient serviceClient = new ServiceClient(conn.IpAddress, int.Parse(port));
                        string serviceName = serviceNames[count];
                        string ipportKey = GetPortKey(conn.IpAddress, port);
                        List<string> serviceandMachineNames = new List<string>();
                        serviceClient.ServiceName = serviceName;
                        serviceClient.MachineName = conn.Name;

                        if (!_dictMachines.ContainsKey(ipportKey))
                        {
                            _dictMachines.Add(ipportKey, serviceClient);
                        }

                        if (serviceClient.Status != PranaInternalConstants.ConnectionStatus.CONNECTED)
                        {
                            ConnectToServer(serviceClient);
                        }
                        count++;
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
            return listOfConnectionTrails;
        }

        public bool AddUser(ServiceClient serviceClient)
        {
            bool result = false;
            try
            {
                if (serviceClient.IdentifierID == "MonitoringServices")
                    return false;

                string ipAddress = serviceClient.ServiceEndPoint.IPAddress;
                int port = serviceClient.ServiceEndPoint.Port;
                String ipportKey = GetPortKey(ipAddress, port);
                if (_dictMachines.ContainsKey(GetPortKey(ipAddress, port)))
                {

                    result = _dictMachines[ipportKey].AddUsers(serviceClient);
                }
                else
                {
                    result = false;
                }
                if (Refresh != null)
                {
                    Refresh(serviceClient.ServiceEndPoint.IPAddress, serviceClient.ServiceEndPoint.Port);
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
            return result;
        }

        public void ChangeEndPointStatus(string ipAddress, int port, PranaInternalConstants.ConnectionStatus status)
        {
            String ipportKey = GetPortKey(ipAddress, port);
            if (_dictMachines.ContainsKey(ipportKey))
            {
                _dictMachines[ipportKey].ChangeEndPointStatus(status);
            }
        }

        public void ChangeUserStatus(string ipAddress, int port, string userID, PranaInternalConstants.ConnectionStatus status)
        {
            String ipportKey = GetPortKey(ipAddress, port);
            if (_dictMachines.ContainsKey(ipportKey))
            {
                ServiceClient client = _dictMachines[ipportKey].GetUser(userID);
                if (client != null)
                    client.ChangeStatus(status);
                if (Refresh != null)
                {
                    Refresh(ipAddress, port);
                }

            }
        }

        public ServiceClient GetServiceClient(string ipAddress, int port)
        {
            String ipportKey = GetPortKey(ipAddress, port);
            return GetServiceClient(ipportKey);

        }

        public ServiceClient GetServiceClient(string ipportKey)
        {
            if (_dictMachines.ContainsKey(ipportKey))
            {
                return _dictMachines[ipportKey];
            }
            else
                return null;
        }

        public List<ServiceClient> GetDisconnectedEndPoints()
        {
            List<ServiceClient> clientsNotConnected = new List<ServiceClient>();
            foreach (KeyValuePair<string, ServiceClient> ipPortItem in _dictMachines)
            {
                if (ipPortItem.Value.ServiceEndPoint.Status != PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    clientsNotConnected.Add(ipPortItem.Value);
                }
            }
            return clientsNotConnected;
        }

        public void AddException(string message, string key)
        {
            try
            {
                if (!_errorMessages.ContainsKey(key))
                {
                    _errorMessages.Add(key, new List<string>());
                }

                _errorMessages[key].Add(message);
                string[] data = key.Split(':');
                string ipAddress = data[0];
                int port = int.Parse(data[1]);
                if (Refresh != null)
                {
                    Refresh(ipAddress, port);
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

        public List<string> GetExceptions(string ipAddress, int port)
        {
            string key = GetPortKey(ipAddress, port);
            if (_errorMessages.ContainsKey(key))
            {
                return _errorMessages[key];
            }
            return new List<string>(); ;
        }

        public List<string> GetExceptions(string ipPortKey)
        {

            if (_errorMessages.ContainsKey(ipPortKey))
            {
                return _errorMessages[ipPortKey];
            }
            return new List<string>(); ;
        }

        public string GetPortKey(string ipAddress, int port)
        {
            return ipAddress + ":" + port.ToString();
        }

        public string GetPortKey(string ipAddress, string port)
        {
            return ipAddress + ":" + port;
        }

        Dictionary<string, PerformanceParameters> _dictPerformanceDetails = new Dictionary<string, PerformanceParameters>();
        public void AddPerformanceDetail(string key, PerformanceParameters parameter)
        {
            if (!_dictPerformanceDetails.ContainsKey(key))
            {
                _dictPerformanceDetails.Add(key, parameter);
            }
            else
            {
                _dictPerformanceDetails[key] = parameter;
            }
        }

        public PerformanceParameters GetPerformanceDetail(string key)
        {
            if (_dictPerformanceDetails.ContainsKey(key))
            {
                return _dictPerformanceDetails[key];
            }
            return new PerformanceParameters("", "");
        }

        public List<MonitoringConnection> GetAllMachineDetails()
        {
            List<MonitoringConnection> list = new List<MonitoringConnection>();
            foreach (KeyValuePair<string, MonitoringConnection> item in _dictMonitoring)
            {
                list.Add(item.Value);
            }
            return list;
        }

        public MonitoringConnection GetMachineDetails(string ipAddress, string port)
        {
            String portKey = GetPortKey(ipAddress, port);
            if (_dictMachines.ContainsKey(portKey))
            {
                string machineName = _dictMachines[portKey].MachineName;
                return _dictMonitoring[machineName];
            }
            return null;
        }

        public void ConnectToServer(string ipPortKey)
        {
            if (_dictMachines.ContainsKey(ipPortKey))
            {
                ConnectToServer(_dictMachines[ipPortKey]);
            }

        }

        public void Disconnect(string ipPortKey)
        {
            if (_dictconnList.ContainsKey(ipPortKey))
            {
                _dictconnList[ipPortKey].DisConnect();
            }
        }

        public ICommunicationManager ConnectToServer(ServiceClient serviceClient)
        {
            ICommunicationManager communicationManager = new ClientTradeCommManager();
            try
            {
                ConnectionProperties connProperties = new ConnectionProperties();
                ServiceEndPoint endPoint = serviceClient.ServiceEndPoint;
                if (serviceClient.Status != PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    _dictconnList.Remove(serviceClient.PortKey);
                    _dictconnList.Add(serviceClient.PortKey, communicationManager);

                    connProperties.ServerIPAddress = endPoint.IPAddress;
                    connProperties.Port = endPoint.Port;
                    connProperties.IdentifierID = "MonitoringServices";
                    connProperties.IdentifierName = "MonitoringServices";
                    connProperties.ConnectedServerName = "MonitoringServices";
                    connProperties.HandlerType = HandlerType.MonitoringServices;
                    communicationManager.Connect(connProperties);
                    communicationManager.Disconnected += new EventHandler(communicationManager_Disconnected);
                    communicationManager.MessageReceived += new MessageReceivedDelegate(communicationManager_MessageReceived);
                    serviceClient.ChangeEndPointStatus(communicationManager.ConnectionStatus);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return communicationManager;
        }

        void bgWorkerConnect_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                ConnectionProperties connProperties = (ConnectionProperties)e.Result;
                if (Refresh != null)
                {
                    Refresh(connProperties.ServerIPAddress, connProperties.Port);
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

        void bgWorkerConnect_DoWork(object sender, DoWorkEventArgs e)
        {
            ICommunicationManager communicationManager = new ClientTradeCommManager();
            ConnectionProperties connProperties = new ConnectionProperties();
            try
            {
                ServiceClient serviceClient = (ServiceClient)e.Argument;
                ServiceEndPoint endPoint = serviceClient.ServiceEndPoint;
                if (serviceClient.Status != PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    connProperties.ServerIPAddress = endPoint.IPAddress;
                    connProperties.Port = endPoint.Port;
                    connProperties.IdentifierID = "MonitoringServices";
                    connProperties.IdentifierName = "MonitoringServices";
                    connProperties.ConnectedServerName = "MonitoringServices";
                    connProperties.HandlerType = HandlerType.MonitoringServices;
                    communicationManager.Connect(connProperties);
                    communicationManager.Disconnected += new EventHandler(communicationManager_Disconnected);
                    communicationManager.MessageReceived += new MessageReceivedDelegate(communicationManager_MessageReceived);
                    serviceClient.ChangeEndPointStatus(communicationManager.ConnectionStatus);
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
            finally
            {
                e.Result = connProperties;
            }
        }

        void communicationManager_MessageReceived(object sender, EventArgs<QueueMessage> e)
        {

            try
            {
                QueueMessage qMsg = e.Value;
                ServiceEndPoint serviceEndPoint = qMsg.ServiceEndPoint;
                switch (qMsg.MsgType)
                {
                    case FIXConstants.MSGLogon:
                        ArrayList logOnData = (ArrayList)binaryFormatter.DeSerialize(qMsg.Message.ToString());
                        ConnectionProperties connPropertiesConn = (ConnectionProperties)logOnData[0];
                        Dictionary<string, string> connectedClients = (Dictionary<string, string>)logOnData[1];
                        ServiceClient serviceClient = new ServiceClient(serviceEndPoint.IPAddress, serviceEndPoint.Port, connPropertiesConn.IdentifierID, connPropertiesConn.IdentifierName);

                        serviceClient.ChangeEndPointStatus(PranaInternalConstants.ConnectionStatus.CONNECTED);
                        foreach (KeyValuePair<string, string> connectedUser in connectedClients)
                        {
                            ServiceClient serviceUser = new ServiceClient(serviceEndPoint.IPAddress, serviceEndPoint.Port, connectedUser.Key, connectedUser.Value);
                            serviceUser.ChangeEndPointStatus(PranaInternalConstants.ConnectionStatus.CONNECTED);
                            serviceUser.UserType = ConnectedEntityTypes.Users;
                            AddUser(serviceUser);
                        }
                        break;
                    case FIXConstants.MSGLogout:
                        ConnectionProperties connPropertiesDisConn = (ConnectionProperties)binaryFormatter.DeSerialize(qMsg.Message.ToString());
                        ChangeUserStatus(serviceEndPoint.IPAddress, serviceEndPoint.Port, connPropertiesDisConn.IdentifierID, PranaInternalConstants.ConnectionStatus.DISCONNECTED);

                        logMessage(connPropertiesDisConn.ServerIPAddress, connPropertiesDisConn.Port.ToString(), "User", connPropertiesDisConn.IdentifierName, "Disconnected");
                        break;
                    case FIXConstants.MSGHeartbeat:
                        break;
                    case CustomFIXConstants.MSG_COUNTERPARTY_CONNECTIONSTATUS_REPORT:
                        string message = qMsg.Message.ToString();
                        ServiceClient serviceClientCP = AdminMessageHandler.GetCounterPartyStatus(message);
                        serviceClientCP.ServiceEndPoint.IPAddress = qMsg.ServiceEndPoint.IPAddress;
                        serviceClientCP.ServiceEndPoint.Port = qMsg.ServiceEndPoint.Port;
                        serviceClientCP.UserType = ConnectedEntityTypes.FixSessions;

                        if (serviceClientCP.Status != PranaInternalConstants.ConnectionStatus.CONNECTED)
                        {
                            logMessage(serviceClientCP.ServiceEndPoint.IPAddress, serviceClientCP.ServiceEndPoint.Port.ToString(), ConnectedEntityTypes.FixSessions.ToString(), serviceClientCP.IdentifierName, serviceClientCP.Status.ToString());
                        }

                        AddUser(serviceClientCP);
                        break;

                    case CustomFIXConstants.MSG_ExceptionRaised:

                        string msgException = binaryFormatter.DeSerialize(qMsg.Message.ToString()).ToString();
                        AddException(msgException, GetPortKey(serviceEndPoint.IPAddress, serviceEndPoint.Port));

                        logMessage(serviceEndPoint.IPAddress, serviceEndPoint.Port.ToString(), "", "", msgException);
                        break;

                    case CustomFIXConstants.MSG_PerformanceReport:
                        PerformanceParameters parameters = (PerformanceParameters)binaryFormatter.DeSerialize(qMsg.Message.ToString());
                        AddPerformanceDetail(GetPortKey(serviceEndPoint.IPAddress, serviceEndPoint.Port), parameters);
                        break;

                    case CustomFIXConstants.MSG_StatusMessge:
                        string msgStatus = binaryFormatter.DeSerialize(qMsg.Message.ToString()).ToString();
                        string[] dataStatus = msgStatus.Split(':');
                        string identifierID = dataStatus[0];
                        int connStatus = int.Parse(dataStatus[1]);

                        ServiceClient clientStatus = new ServiceClient(serviceEndPoint, identifierID, identifierID);
                        clientStatus.ChangeStatus((PranaInternalConstants.ConnectionStatus)connStatus);
                        clientStatus.UserType = ConnectedEntityTypes.MiscConnection;
                        if (connStatus != (int)PranaInternalConstants.ConnectionStatus.CONNECTED)
                        {
                            logMessage(serviceEndPoint.IPAddress, serviceEndPoint.Port.ToString(), identifierID, "", "Disconnected");
                        }
                        AddUser(clientStatus);
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

        void communicationManager_Disconnected(object sender, EventArgs e)
        {
            try
            {
                SocketConnection socketConn = (SocketConnection)sender;
                ServiceClient serviceClient = GetServiceClient(socketConn.ConnProperties.ServerIPAddress, socketConn.ConnProperties.Port);
                serviceClient.ChangeEndPointStatus(PranaInternalConstants.ConnectionStatus.DISCONNECTED);
                logMessage(socketConn.ConnProperties.ServerIPAddress, socketConn.ConnProperties.Port.ToString(), "", "", "Disconnected");
                if (Refresh != null)
                {
                    Refresh(socketConn.ConnProperties.ServerIPAddress, socketConn.ConnProperties.Port);
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

        void logMessage(string ipaddress, string port, string type, string name, string status)
        {
            try
            {
                string downServiceName = GetServiceName(ipaddress, port);
                string downServiceMachineName = GetMachineName(ipaddress, port);
                string msg = type + ":" + name + ":" + status;
                logger.Error(" Please check following System   : " + " Customer Name=" + downServiceMachineName + ", Service Name=" + downServiceName + " IpAddress:Port=" + ipaddress + ":" + port.ToString() + " " + "\n" + msg);
            }
            catch (Exception ex)
            {
            }
        }

        public string GetServiceName(string ipAddress, string port)
        {
            return GetServiceName(GetPortKey(ipAddress, port));

        }

        public string GetServiceName(string ipPortKey)
        {
            return _dictMachines[ipPortKey].ServiceName;
        }

        public string GetServiceName(string ipAddress, int port)
        {
            return GetServiceName(GetPortKey(ipAddress, port.ToString()));
        }

        public string GetMachineName(string ipAddress, string port)
        {
            string portKey = GetPortKey(ipAddress, port);
            return GetMachineName(portKey);
        }

        public string GetMachineName(string ipPortKey)
        {
            if (_dictMachines.ContainsKey(ipPortKey))
                return _dictMachines[ipPortKey].MachineName;
            else
                return ipPortKey;
        }

        public string GetIPPortKeyForMachineAndService(string machineName, string serviceName)
        {
            foreach (KeyValuePair<string, ServiceClient> serviceClient in _dictMachines)
            {
                if (serviceClient.Value.ServiceName.Equals(serviceName) && serviceClient.Value.MachineName.Equals(machineName))
                {
                    return serviceClient.Key;
                }
            }
            return string.Empty;

        }

        public List<MonitoringConnection> getAllConnections()
        {
            List<MonitoringConnection> list = new List<MonitoringConnection>();
            foreach (KeyValuePair<String, MonitoringConnection> conn in _dictMonitoring)
            {
                list.Add(conn.Value);
            }
            return list;
        }

        public void RemoveConnection(string name)
        {
            if (_dictMonitoring.ContainsKey(name))
            {
                MonitoringConnection conn = _dictMonitoring[name];

                string ipaddress = conn.IpAddress;
                string[] ports = conn.Ports.Split(',');
                foreach (string port in ports)
                {
                    string ipportkey = GetPortKey(ipaddress, port);
                    if (_dictMachines.ContainsKey(ipportkey))
                    {
                        ServiceClient client = _dictMachines[ipportkey];
                        Disconnect(ipportkey);
                        _dictMachines.Remove(ipportkey);
                    }

                    if (_dictconnList.ContainsKey(ipportkey))
                    {
                        _dictconnList.Remove(ipportkey);
                    }
                }
                _dictMonitoring.Remove(name);

            }
        }

        public void SaveSettings()
        {
            try
            {
                // Create a new file stream to write the serialized object to a file
                TextWriter WriteFileStream = new StreamWriter(GetPath());
                SerializerObj.Serialize(WriteFileStream, getAllConnections());
                WriteFileStream.Close();
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
    }
}
