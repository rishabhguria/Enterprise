using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class DBSettings
    {
        private string _clientDB;

        public string ClientDB
        {
            get { return _clientDB; }
            set { _clientDB = value; }
        }

        private string _secDB;

        public string SecDB
        {
            get { return _secDB; }
            set { _secDB = value; }
        }

    }
}
