using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ConnectionProperties
    {
        int _port = int.MinValue;
        string identifierID = string.Empty;
        string identifierName = string.Empty;
        CompanyUser _user;
        HandlerType _handlerType;
        string _serverIPAddress = string.Empty;
        public string ServerIPAddress
        {
            get { return _serverIPAddress; }
            set { _serverIPAddress = value; }

        }
        public int Port
        {
            get { return _port; }
            set { _port = value; }

        }
        public HandlerType HandlerType
        {
            get { return _handlerType; }
            set { _handlerType = value; }

        }
        public string IdentifierName
        {
            get { return identifierName; }
            set { identifierName = value; }

        }
        public string IdentifierID
        {
            get { return identifierID; }
            set { identifierID = value; }

        }
        public CompanyUser User
        {
            get { return _user; }
            set { _user = value; }

        }
        public string ConnectedServerName = string.Empty;
        private List<string> _tradingAccounts;

        public List<string> TradingAccounts
        {
            get { return _tradingAccounts; }
            set { _tradingAccounts = value; }
        }

    }
}
