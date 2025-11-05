using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class InputParametersCollection
    {
        #region private variables
        private Dictionary<string, fxInfo> _dictFXSymbols = new Dictionary<string, fxInfo>();
        private Dictionary<string, SubscriberViewInputs> _dictSubscriberInputs = new Dictionary<string, SubscriberViewInputs>();
        private List<string> _listUniqueSymbols = new List<string>();
        private string _userID = string.Empty;
        #endregion

        #region properties
        public Dictionary<string, fxInfo> DictFXSymbols
        {
            get { return _dictFXSymbols; }
            set { _dictFXSymbols = value; }
        }

        public Dictionary<string, SubscriberViewInputs> DictSubscriberInputs
        {
            get { return _dictSubscriberInputs; }
            set { _dictSubscriberInputs = value; }
        }

        public List<string> ListUniqueSymbols
        {
            get { return _listUniqueSymbols; }
            set { _listUniqueSymbols = value; }
        }

        public string UserID
        {
            set { _userID = value; }
            get { return _userID; }
        }
        #endregion
    }
}