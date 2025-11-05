using Prana.BusinessObjects;
using Prana.Interfaces;
using System.Collections.Generic;

namespace Prana.OptionCalculator.Common
{
    public class SubscriberCollection
    {
        static Dictionary<string, SubscriberData> _subscriberCollection = new Dictionary<string, SubscriberData>();
        private static IPricingService _pricingService = null;

        public static void Initiate(IPricingService pricingService)
        {
            _pricingService = pricingService;
        }

        private static void RegisterSubscriber(string id)
        {
            if (!_subscriberCollection.ContainsKey(id))
            {
                _subscriberCollection.Add(id, new SubscriberData());
            }
        }

        public static void UnRegisterSymbols(string id)
        {
            List<string> newList = new List<string>();
            if (_subscriberCollection.ContainsKey(id))
            {
                SubscriberData subData = _subscriberCollection[id];
                newList = subData.SubscriberSymbolList;
                _subscriberCollection.Remove(id);
            }

            #region check ref Count
            List<string> deleteList = new List<string>();
            foreach (string symbol in newList)
            {
                bool found = false;
                foreach (KeyValuePair<string, SubscriberData> _subsCriberDataKeyValue in _subscriberCollection)
                {
                    if (_subsCriberDataKeyValue.Value.SubscriberSymbolList.Contains(symbol))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    deleteList.Add(symbol);
                }
            }
            #endregion
        }

        public static void RegisterSymbolsForSnapShot(InputParametersCollection inputParams)
        {
            //writer lock
            if (!_subscriberCollection.ContainsKey(inputParams.UserID))
            {
                RegisterSubscriber(inputParams.UserID);
            }

            //Add to snapshot request list
            _subscriberCollection[inputParams.UserID].AddSnapShotInputParameters(inputParams);
        }

        public static SubscriberViewData GetSubscriberView(string userID, string subscriberViewID)
        {
            if (_subscriberCollection.ContainsKey(userID))
            {
                SubscriberData subscriber = _subscriberCollection[userID];
                return subscriber.GetSubscriberView(subscriberViewID);
            }
            return null;
        }

        public static Dictionary<string, SubscriberData> SubscriberCollectionData
        {
            get
            {
                return _subscriberCollection;
            }
        }

        public static bool IsLiveDataRequested()
        {
            bool result = false;
            foreach (KeyValuePair<string, SubscriberData> item in _subscriberCollection)
            {
                if (item.Value.SubscriberSymbolList.Count > 0)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
