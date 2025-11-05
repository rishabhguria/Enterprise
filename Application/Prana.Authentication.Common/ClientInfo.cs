using System.Collections.Generic;

namespace Prana.Authentication.Common
{
    public class ClientInfo
    {
        private int _companyUserId = int.MinValue;
        public int CompanyUserId
        {
            get { return _companyUserId; }
            set { _companyUserId = value; }
        }

        private Dictionary<int, IClientConnectivityServiceCallback> _callback = new Dictionary<int, IClientConnectivityServiceCallback>();
        public Dictionary<int, IClientConnectivityServiceCallback> Callback
        {
            get { return _callback; }
        }
    }
}
