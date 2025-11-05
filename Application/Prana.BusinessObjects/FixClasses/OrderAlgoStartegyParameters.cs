using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class OrderAlgoStartegyParameters
    {
        private string _algostartegyID;

        public string AlgoStartegyID
        {
            get { return _algostartegyID; }
            set { _algostartegyID = value; }
        }

        public OrderAlgoStartegyParameters()
        {

        }

        public OrderAlgoStartegyParameters(string message)
        {
            string[] algoList = message.Split(Seperators.SEPERATOR_5);
            foreach (string algoItem in algoList)
            {
                if (algoItem != string.Empty)
                {
                    string[] itemArray = algoItem.Split(Seperators.SEPERATOR_6);
                    _tagValueDictionary.Add(itemArray[0], itemArray[1]);
                }
            }
        }

        private Dictionary<string, string> _tagValueDictionary = new Dictionary<string, string>();

        public Dictionary<string, string> TagValueDictionary
        {
            get { return _tagValueDictionary; }
            set { _tagValueDictionary = value; }
        }
        public override string ToString()
        {
            return GetStringFromCollection(_tagValueDictionary);
        }
        private string GetStringFromCollection(Dictionary<string, string> collection)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> item in collection)
            {
                sb.Append(item.Key + Seperators.SEPERATOR_6 + item.Value);
                sb.Append(Seperators.SEPERATOR_5);
            }
            return sb.ToString();
        }

        public OrderAlgoStartegyParameters Clone()
        {
            OrderAlgoStartegyParameters cloned = new OrderAlgoStartegyParameters();
            cloned.AlgoStartegyID = this._algostartegyID;
            cloned.TagValueDictionary = new Dictionary<string, string>(this._tagValueDictionary);
            return cloned;
        }
    }
}
