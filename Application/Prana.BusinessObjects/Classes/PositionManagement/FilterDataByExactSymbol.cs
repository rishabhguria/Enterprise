using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class FilterDataByExactSymbol : FilterData
    {
        private string _symbol = string.Empty;

        public string GivenSymbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        public override List<IFilterable> Filterdata(ref System.Object[] dataToFilter, string TopicName, int userId)
        {
            List<IFilterable> dataToSend = new List<IFilterable>();
            foreach (IFilterable publishdata in dataToFilter)
            {
                if (publishdata.GetSymbol().Equals(_symbol, StringComparison.InvariantCultureIgnoreCase))
                {
                    dataToSend.Add(publishdata);
                }
            }
            int i = 0;
            Object[] filteredData = new Object[dataToSend.Count];
            foreach (IFilterable publishData in dataToSend)
            {
                filteredData[i] = publishData;
                i++;
            }
            dataToFilter = filteredData;
            return dataToSend;
        }
    }
}
