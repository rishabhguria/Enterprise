using Prana.BusinessObjects.FIX;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class QueueMessage
    {
        private string _msgType;
        private Object _message;
        private string _tradingAccountId;
        private string _userID;
        private int _hashCode;
        private List<string> _freeUsers;
        private bool _isMultiBroker = false; //For updating order in case of multi broker

        private Dictionary<string, string> _usersToSendDynamicData;
        public Dictionary<string, string> UsersToSendDynamicData
        {
            get { return _usersToSendDynamicData; }
            set { _usersToSendDynamicData = value; }
        }

        public QueueMessage()
        {
            _msgType = string.Empty;
            _tradingAccountId = string.Empty;
            _userID = string.Empty;
            _hashCode = int.MinValue;
        }

        public QueueMessage(string msgType, Object message)
            : this()
        {
            _msgType = msgType;
            _message = message;
        }

        //Added New Constructor in case of sending trade to queue. As need to update fields in case of multi broker. PRANA-10501
        public QueueMessage(string msgType, string tradingAccountID, string userID, Object message, bool isMultiBroker)
            : this()
        {
            _msgType = msgType;
            _message = message;
            _userID = userID;
            _tradingAccountId = tradingAccountID;
            _isMultiBroker = isMultiBroker;
        }

        public bool IsMultiBroker { get { return _isMultiBroker; } }

        public QueueMessage(string msgType, string tradingAccountID, string userID, Object message)
            : this()
        {
            _msgType = msgType;
            _message = message;
            _userID = userID;
            _tradingAccountId = tradingAccountID;
        }

        public QueueMessage(string msgType, List<string> freeUsers, Object message)
            : this()
        {
            _msgType = msgType;
            _message = message;
            _freeUsers = freeUsers;
        }

        public QueueMessage(string msgType, Dictionary<string, string> usersToSendDynamicData)
            : this()
        {
            _msgType = msgType;
            _usersToSendDynamicData = usersToSendDynamicData;
        }

        public string MsgType
        {
            get { return _msgType; }
            set { _msgType = value; }
        }

        public Object Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public string TradingAccountID
        {
            get { return _tradingAccountId; }
            set { _tradingAccountId = value; }
        }

        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        public List<string> FreeUsers
        {
            get { return _freeUsers; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("MsgType=");
            sb.Append(_msgType);
            sb.Append(Seperators.SEPERATOR_9);
            sb.Append("TradingAccountID=");
            sb.Append(_tradingAccountId);
            sb.Append(Seperators.SEPERATOR_9);
            sb.Append("UserID=");
            sb.Append(_userID);
            sb.Append(Seperators.SEPERATOR_9);
            sb.Append("RequestID=");
            sb.Append(_requestID);
            sb.Append(Seperators.SEPERATOR_9);
            sb.Append("HashCode=");
            sb.Append(_hashCode.ToString());
            sb.Append(Seperators.SEPERATOR_10);
            //Raturi: check for null reference
            //http://jira.nirvanasolutions.com:8080/browse/PRANA-6820
            if (_message != null)
                sb.Append(_message.ToString());
            sb.Append(Seperators.SEPERATOR_10);
            if (_serviceEndPoint != null)
            {
                sb.Append(_serviceEndPoint.IPAddress.ToString());
                sb.Append(Seperators.SEPERATOR_10);
                sb.Append(_serviceEndPoint.Port.ToString());
            }
            return sb.ToString();
        }

        public string ToString(string userID)
        {
            StringBuilder sb = new StringBuilder();
            if (_usersToSendDynamicData.ContainsKey(userID))
            {
                sb.Append("MsgType=");
                sb.Append(_msgType);
                sb.Append(Seperators.SEPERATOR_9);
                sb.Append("TradingAccountID=");
                sb.Append(_tradingAccountId);
                sb.Append(Seperators.SEPERATOR_9);
                sb.Append("UserID=");
                sb.Append(_userID);
                sb.Append(Seperators.SEPERATOR_9);
                sb.Append("RequestID=");
                sb.Append(_requestID);
                sb.Append(Seperators.SEPERATOR_9);
                sb.Append("HashCode=");
                sb.Append(_hashCode.ToString());
                sb.Append(Seperators.SEPERATOR_10);
                sb.Append(_usersToSendDynamicData[userID]);
                sb.Append(Seperators.SEPERATOR_10);
                if (_serviceEndPoint != null)
                {
                    sb.Append(_serviceEndPoint.IPAddress.ToString());
                    sb.Append(Seperators.SEPERATOR_10);
                    sb.Append(_serviceEndPoint.Port.ToString());
                }
            }
            return sb.ToString();
        }

        public QueueMessage(PranaMessage pranaMsg)
            : this()
        {
            _msgType = pranaMsg.MessageType;
            _message = pranaMsg;
            if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_TradingAccountID))
            {
                _tradingAccountId = pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_TradingAccountID].Value;
            }
            else if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_CompanyUserID))
            {
                _userID = pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CompanyUserID].Value;
            }
        }

        public QueueMessage(string msg)
            : this()
        {
            string[] data = msg.Split(Seperators.SEPERATOR_10);
            string[] headerData = data[0].Split(Seperators.SEPERATOR_9);
            _msgType = headerData[0].Split('=')[1];
            _tradingAccountId = headerData[1].Split('=')[1];
            _userID = headerData[2].Split('=')[1];
            _requestID = headerData[3].Split('=')[1];
            _hashCode = Convert.ToInt32(headerData[4].Split('=')[1]);
            _message = data[1];
            if (data.Length > 3)
            {
                _serviceEndPoint = new ServiceEndPoint(data[2], int.Parse(data[3]));
            }
            data = null;
            headerData = null;
            msg = null;
        }

        private string _requestID = string.Empty;
        public string RequestID
        {
            get { return _requestID; }
            set { _requestID = value; }
        }

        public int HashCode
        {
            get { return _hashCode; }
            set { _hashCode = value; }
        }

        private ServiceEndPoint _serviceEndPoint;
        public ServiceEndPoint ServiceEndPoint
        {
            get { return _serviceEndPoint; }
            set { _serviceEndPoint = value; }
        }
    }
}
