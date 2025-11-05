using System;
using System.Collections.Generic;



namespace Prana.BusinessObjects
{
    [Serializable]
    public class FilterDataByFromDate : FilterData
    {
        private DateTime _fromDate = DateTime.MinValue;
        public DateTime FromDate
        {
            get { return _fromDate; }
            set { _fromDate = value; }
        }

        public override List<IFilterable> Filterdata(ref System.Object[] dataToFilter, string TopicName, int userId)
        {

            List<IFilterable> dataToSend = new List<IFilterable>();
            foreach (IFilterable publishdata in dataToFilter)
            {
                if (publishdata.GetDate().Date >= _fromDate.Date)
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
