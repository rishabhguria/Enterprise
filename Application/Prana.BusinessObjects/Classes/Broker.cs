using System;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for Broker.
    /// </summary>
    [Serializable]
    public class Broker
    {
        int _brokerID = int.MinValue;
        string _name = string.Empty;

        public int BrokerID
        {
            get { return _brokerID; }
            set { _brokerID = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
