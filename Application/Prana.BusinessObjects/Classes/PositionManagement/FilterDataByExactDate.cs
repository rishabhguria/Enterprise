using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class FilterDataByExactDate : FilterData
    {
        private DateTime _givenDate = DateTime.MinValue;

        public DateTime GivenDate
        {
            get { return _givenDate; }
            set { _givenDate = value; }
        }

        public override List<IFilterable> Filterdata(ref System.Object[] dataToFilter, string TopicName, int userId)
        {
            List<IFilterable> dataToSend = new List<IFilterable>();
            foreach (IFilterable publishdata in dataToFilter)
            {
                if (publishdata.GetDate().Date.Date == _givenDate.Date.Date)
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
