using Prana.BusinessObjects.AppConstants;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class ServiceClient
    {
        private Dictionary<string, ServiceClient> _childs;

        public ServiceClient(string ipAddress, int port)
        {
            _serviceEndPoint = new ServiceEndPoint(ipAddress, port);
            _identifierID = ipAddress;
            _name = ipAddress;
        }
        public ServiceClient(string ipAddress, int port, string identifierID, string identifierName)
        {
            _serviceEndPoint = new ServiceEndPoint(ipAddress, port);
            _identifierID = identifierID;
            _name = identifierName;
        }
        public ServiceClient(ServiceEndPoint endPoint, string identifierID, string identifierName)
        {
            _serviceEndPoint = endPoint;
            _identifierID = identifierID;
            _name = identifierName;
        }

        private string _name;

        public string IdentifierName
        {
            get { return _name; }

        }

        private string _identifierID;

        public string IdentifierID
        {
            get { return _identifierID; }

        }

        private ServiceEndPoint _serviceEndPoint;

        public ServiceEndPoint ServiceEndPoint
        {
            get { return _serviceEndPoint; }
            //set { _serviceEndPoint = value; }
        }
        /// <summary>
        /// changes the endpoint status . if endpoint is disconnected all clients 
        /// at that end point are also disconnected
        /// </summary>
        /// <param name="connectionStatus"></param>
        public void ChangeEndPointStatus(PranaInternalConstants.ConnectionStatus connectionStatus)
        {
            _serviceEndPoint.ChangeStatus(connectionStatus);

            _status = connectionStatus;

            if (_childs != null && connectionStatus != PranaInternalConstants.ConnectionStatus.CONNECTED)
            {
                foreach (KeyValuePair<string, ServiceClient> child in _childs)
                {
                    child.Value.ChangeStatus(connectionStatus);
                }
            }
        }

        public bool AddUsers(ServiceClient parent)
        {
            // .IdentifierID, serviceClient.IdentifierName, serviceClient.Status
            bool result = false;
            if (_childs == null)
            {
                _childs = new Dictionary<string, ServiceClient>();
            }
            ServiceClient client = new ServiceClient(_serviceEndPoint, parent.IdentifierID, parent.IdentifierName);
            client.UserType = parent.UserType;
            client.ChangeStatus(parent.Status);
            if (!_childs.ContainsKey(client.IdentifierID))
            {
                _childs.Add(client.IdentifierID, client);
                result = true;
            }
            else
            {
                _childs[client.IdentifierID] = client;
                result = false;
            }
            return result;
        }


        public ServiceClient GetUser(string userID)
        {
            if (userID.Contains(":"))
            {
                userID = userID.Split(':')[2];
            }
            if (_childs.ContainsKey(userID))
                return _childs[userID];
            else
                return null;
        }


        private DateTime _lastConnectedTime;

        public DateTime LastConnectedTime
        {
            get { return _lastConnectedTime; }
            set { _lastConnectedTime = value; }
        }

        private PranaInternalConstants.ConnectionStatus _status = PranaInternalConstants.ConnectionStatus.DISCONNECTED;

        public void ChangeStatus(PranaInternalConstants.ConnectionStatus status)
        {
            _status = status;
        }
        public PranaInternalConstants.ConnectionStatus Status
        {
            get { return _status; }
        }
        public String ConnStatus
        {
            get { return _status.ToString(); }
        }

        public string UserKey
        {
            get
            {
                return PortKey + ":" + _identifierID;
            }
        }
        public string PortKey
        {
            get
            {
                return _serviceEndPoint.IPAddress + ":" + _serviceEndPoint.Port.ToString();
            }
        }
        private ConnectedEntityTypes _userType;

        public ConnectedEntityTypes UserType
        {
            get { return _userType; }
            set { _userType = value; }
        }

        public List<ServiceClient> GetChilds(ConnectedEntityTypes connType)
        {
            List<ServiceClient> list = new List<ServiceClient>();
            if (_childs != null)
            {
                foreach (KeyValuePair<string, ServiceClient> item in _childs)
                {
                    if (connType == item.Value.UserType)
                    {
                        list.Add(item.Value);
                    }
                }
            }
            return list;
        }
        public int GetConnectedUsers(ConnectedEntityTypes connType)
        {
            int count = 0;
            if (_childs != null)
            {
                foreach (KeyValuePair<string, ServiceClient> item in _childs)
                {
                    if (connType == item.Value.UserType && item.Value.Status == PranaInternalConstants.ConnectionStatus.CONNECTED)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        //private string  _servicename;

        public string ServiceName
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _machineName;

        public string MachineName
        {
            get { return _machineName; }
            set { _machineName = value; }
        }
    }
}
