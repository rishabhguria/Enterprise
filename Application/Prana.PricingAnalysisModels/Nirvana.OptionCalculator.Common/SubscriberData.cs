using Prana.BusinessObjects;
using System.Collections.Generic;

namespace Prana.OptionCalculator.Common
{
    public class SubscriberData
    {
        #region private variables

        private Dictionary<string, SubscriberViewData> _dictSubscriberViews = new Dictionary<string, SubscriberViewData>();

        private List<string> _symbolList = new List<string>();

        #endregion private variables

        #region constructor

        public SubscriberData()
        {
        }

        #endregion constructor

        #region properties

        public Dictionary<string, SubscriberViewData> DictSubscriberViews
        {
            get { return _dictSubscriberViews; }
            set { _dictSubscriberViews = value; }
        }

        public List<string> SubscriberSymbolList
        {
            get { return _symbolList; }
        }

        #endregion properties

        #region public-members

        public void AddSnapShotInputParameters(InputParametersCollection inputParametersCollection)
        {
            foreach (SubscriberViewInputs inputs in inputParametersCollection.DictSubscriberInputs.Values)
            {
                if (!_dictSubscriberViews.ContainsKey(inputs.ID))
                {
                    RegisterSubscriberView(inputs.ID);
                }
                _dictSubscriberViews[inputs.ID].AddSubscriberInputParameters(inputs);
            }
        }

        public SubscriberViewData GetSubscriberView(string subscriberViewID)
        {
            if (_dictSubscriberViews.ContainsKey(subscriberViewID))
            {
                return _dictSubscriberViews[subscriberViewID];
            }
            return null;
        }

        public List<SubscriberViewData> GetVeiwsToBeProcessed(SymbolData data)
        {
            List<SubscriberViewData> listViews = new List<SubscriberViewData>();
            lock (_dictSubscriberViews)
            {
                foreach (SubscriberViewData subscriberView in _dictSubscriberViews.Values)
                {
                    if (subscriberView.UpdateSnapShotResponseifProcessingRequired(data))
                    {
                        listViews.Add(subscriberView);
                    }
                }
            }
            return listViews;
        }

        public List<SubscriberViewData> GetViewsToBeProcessed()
        {
            List<SubscriberViewData> listViews = new List<SubscriberViewData>();
            lock (_dictSubscriberViews)
            {
                foreach (SubscriberViewData subscriberView in _dictSubscriberViews.Values)
                {
                    listViews.Add(subscriberView);
                }
            }
            return listViews;
        }

        public Dictionary<string, SymbolData> SetSymbolsData(Dictionary<string, SymbolData> calculatedGreeks)
        {
            Dictionary<string, SymbolData> fullOptionData = new Dictionary<string, SymbolData>();
            foreach (KeyValuePair<string, SymbolData> optiondata in calculatedGreeks)
            {
                fullOptionData.Add(optiondata.Key, optiondata.Value);
            }
            return fullOptionData;
        }

        #endregion public-members

        #region private members

        private void RegisterSubscriberView(string id)
        {
            if (!_dictSubscriberViews.ContainsKey(id))
            {
                _dictSubscriberViews.Add(id, new SubscriberViewData());
            }
        }

        #endregion private members
    }
}