using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;

namespace Prana.MonitoringServices
{
    public class MonitoringConnection
    {
        private string  _ipAddress=string.Empty;
        public string  IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        private string _machineName;
        public string Name
        {
            get { return _machineName; }
            set { _machineName = value; }
        }
	
        private string _ports = string.Empty;
        public string  Ports
        {
            get { return _ports; }
            set { _ports = value; }
        }

        private string _serviceNames = string.Empty;
        public string ServiceNames
        {
            get { return _serviceNames; }
            set { _serviceNames = value; }
        }

        private string _tradeServer = "Connect";
        public string TradeServer
        {
            get { return _tradeServer; }
            set { _tradeServer = value; }
        }

        private string _expnlServer = "Connect";
        public string ExpnlServer
        {
            get { return _expnlServer; }
            set { _expnlServer = value; }
        }

        private string _pricingServer="Connect";
        public string PricingServer
        {
            get { return _pricingServer; }
            set { _pricingServer = value; }
        }

        public MonitoringConnection Clone()
        {
            MonitoringConnection conn = new MonitoringConnection();
            conn.IpAddress = _ipAddress;
            conn.Ports = _ports;
            conn.TradeServer = _tradeServer;
            conn.ExpnlServer = _expnlServer;
            conn.PricingServer = _pricingServer;
            return conn;
        }
    }
}
