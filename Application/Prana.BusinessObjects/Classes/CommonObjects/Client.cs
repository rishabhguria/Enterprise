using System;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for Client.
    /// </summary>
    [Serializable]
    public class Client
    {
        int _clientID = int.MinValue;
        string _name = string.Empty;

        public Client(int id, string name)
        {
            _clientID = id;
            _name = name;
        }
        public int ClientID
        {
            get { return _clientID; }
            set { _clientID = value; }
        }

        public string ClientName
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
