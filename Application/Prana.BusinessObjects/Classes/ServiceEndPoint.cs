using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ServiceEndPoint
    {
        public ServiceEndPoint(string ipAddress, int port)
        {
            _port = port;
            _ipAddress = ipAddress;

        }
        private int _port;

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }
        private string _ipAddress;

        public string IPAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }


        private PranaInternalConstants.ConnectionStatus _status;

        public void ChangeStatus(PranaInternalConstants.ConnectionStatus status)
        {
            _status = status;
        }

        public PranaInternalConstants.ConnectionStatus Status
        {
            get { return _status; }
            //set { _status = value; }
        }

    }
}
