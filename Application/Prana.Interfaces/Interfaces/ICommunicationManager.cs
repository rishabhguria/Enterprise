using Prana.BusinessObjects;
using Prana.Global;
using System;

namespace Prana.Interfaces
{
    /// <summary>
    /// Summary description for ICommunicationManager.
    /// </summary>
    public interface ICommunicationManager
    {
        event EventHandler Connected;
        event EventHandler Disconnected;
        void SendMessage(string message);
        void SendMessage(QueueMessage message);
        PranaInternalConstants.ConnectionStatus ConnectionStatus { get; }
        PranaInternalConstants.ConnectionStatus Connect(ConnectionProperties properties);
        void DisConnect();
        event MessageReceivedDelegate MessageReceived;
        bool ShouldRetry
        {
            get;
            set;
        }

        bool IsMonitoringConnection
        {
            get;
            set;
        }
    }

    public delegate void MessageReceivedDelegate(object sender, EventArgs<QueueMessage> e);
    public delegate void ConnectionMessageReceivedDelegate(object sender, EventArgs<ConnectionProperties> e);
    public delegate void ExceptionDelegate(object sender, EventArgs<Exception> e);

    public class CounterPartyDetails
    {
        private int connectionID;
        public int ConnectionID
        {
            get { return connectionID; }
            set { connectionID = value; }

        }

        private int counterPartyID;
        public int CounterPartyID
        {
            get { return counterPartyID; }
            set { counterPartyID = value; }

        }

        private string counterPartyName;
        public string CounterPartyName
        {
            get { return counterPartyName; }
            set { counterPartyName = value; }

        }

        private PranaInternalConstants.ConnectionStatus connStatus;
        public PranaInternalConstants.ConnectionStatus ConnStatus
        {
            get { return connStatus; }
            set { connStatus = value; }
        }

        private PranaServerConstants.OriginatorType originatorType;
        public PranaServerConstants.OriginatorType OriginatorType
        {
            get { return originatorType; }
            set { originatorType = value; }
        }

        private PranaServerConstants.BrokerConnectionType brokerConnectionType;
        public PranaServerConstants.BrokerConnectionType BrokerConnectionType
        {
            get { return brokerConnectionType; }
            set { brokerConnectionType = value; }
        }
    }
}
