using System;
using System.Collections.Generic;



namespace Prana.BusinessObjects
{
    [Serializable]
    public class FilterDataByToDate : FilterData
    {
        private DateTime _toDate = DateTime.MinValue;


        public DateTime ToDate
        {
            get { return _toDate; }
            set { _toDate = value; }
        }

        public override List<IFilterable> Filterdata(ref System.Object[] dataToFilter, string TopicName, int userId)
        {
            List<IFilterable> dataToSend = new List<IFilterable>();


            foreach (IFilterable publishdata in dataToFilter)
            {
                if (publishdata.GetDate().Date <= _toDate.Date)
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
