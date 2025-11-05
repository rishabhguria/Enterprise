using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class HostedService
    {
        // Name of the service
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        // Is the service started
        private bool _isStarted;
        public bool IsStarted
        {
            get
            {
                return _isStarted;
            }
        }

        public HostedService(string name, bool isStarted)
        {
            _name = name;
            _isStarted = isStarted;
        }
    }
}
