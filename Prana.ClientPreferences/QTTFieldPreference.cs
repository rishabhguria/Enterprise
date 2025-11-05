using Prana.BusinessObjects;
using System;

namespace Prana.ClientPreferences
{
    [Serializable]
    public class QTTFieldPreference
    {
        public int Allocation { get; set; }

        public int Broker { get; set; }

        public int Venue { get; set; }

        public string TIF { get; set; }

        public string OrderTypeTagValue { get; set; }

        public string AlgoStrategyID { get; set; }

        public string AlgoStrategyName { get; set; }

        public SerializableDictionary<string, string> Tags { get; set; }

        public QTTFieldPreference()
        {
            Allocation = 0;
            Broker = 0;
            Venue = 0;
            TIF = String.Empty;
            OrderTypeTagValue = String.Empty;
            AlgoStrategyID = String.Empty;
            AlgoStrategyName = String.Empty;
            Tags = new SerializableDictionary<string, string>();
        }

    }
}
