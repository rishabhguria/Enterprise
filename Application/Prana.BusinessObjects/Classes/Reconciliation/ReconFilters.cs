using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ReconFilters
    {

        private SerializableDictionary<int, string> _dictAssets = new SerializableDictionary<int, string>();
        public SerializableDictionary<int, string> DictAssets
        {
            get { return _dictAssets; }
            set { _dictAssets = value; }
        }

        private SerializableDictionary<int, string> _dicAUECs = new SerializableDictionary<int, string>();
        public SerializableDictionary<int, string> DictAUECs
        {
            get { return _dicAUECs; }
            set { _dicAUECs = value; }
        }

        private SerializableDictionary<int, string> _dictAccounts = new SerializableDictionary<int, string>();
        public SerializableDictionary<int, string> DictAccounts
        {
            get { return _dictAccounts; }
            set { _dictAccounts = value; }
        }

        private SerializableDictionary<int, string> _dictMasterFunds = new SerializableDictionary<int, string>();
        public SerializableDictionary<int, string> DictMasterFunds
        {
            get { return _dictMasterFunds; }
            set { _dictMasterFunds = value; }
        }

        private SerializableDictionary<int, string> _dictBrokers = new SerializableDictionary<int, string>();
        public SerializableDictionary<int, string> DictBrokers
        {
            get { return _dictBrokers; }
            set { _dictBrokers = value; }
        }

        private SerializableDictionary<int, string> _dictPrimeBrokers = new SerializableDictionary<int, string>();
        public SerializableDictionary<int, string> DictPrimeBrokers
        {
            get { return _dictPrimeBrokers; }
            set { _dictPrimeBrokers = value; }
        }

        private SerializableDictionary<int, string> _dictThirParty = new SerializableDictionary<int, string>();
        public SerializableDictionary<int, string> DictThirParty
        {
            get { return _dictThirParty; }
            set { _dictThirParty = value; }
        }

    }
}
