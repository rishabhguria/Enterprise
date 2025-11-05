using Prana.BusinessObjects;
using Prana.Interfaces;
using System.Collections.Generic;

namespace Prana.OptionServer
{
    internal class SubscriberInfo
    {
        private List<ILiveFeedCallback> _subscribersList = new List<ILiveFeedCallback>();
        public List<ILiveFeedCallback> SubscribersList
        {
            get { return _subscribersList; }
            set { _subscribersList = value; }
        }

        private SymbolData _data;
        public SymbolData Data
        {
            get { return _data; }
            set { _data = value; }
        }

        private bool _saveSymbolInDb = true;
        public bool SaveSymbolInDb
        {
            get { return _saveSymbolInDb; }
            set { _saveSymbolInDb = value; }
        }
    }
}