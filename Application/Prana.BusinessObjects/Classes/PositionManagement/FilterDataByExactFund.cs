using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class FilterDataByExactAccount : FilterData
    {
        private List<int> _lstAccountID = new List<int>();

        public List<int> GivenAccountID
        {
            get { return _lstAccountID; }
            set { _lstAccountID = value; }
        }

        public override List<IFilterable> Filterdata(ref System.Object[] dataToFilter, string TopicName, int userId)
        {
            List<IFilterable> dataToSend = new List<IFilterable>();
            foreach (IFilterable publishdata in dataToFilter)
            {
                if (_lstAccountID.Contains(publishdata.GetAccountID()))
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
