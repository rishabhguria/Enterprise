using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class FilterDataByAUECDateWise : FilterData
    {
        private Dictionary<int, DateTime> _lsAUECDates = new Dictionary<int, DateTime>();

        public Dictionary<int, DateTime> ListOfAUECDates
        {
            get { return _lsAUECDates; }
            set { _lsAUECDates = value; }
        }


        public override List<IFilterable> Filterdata(ref object[] data, string Topic, int userId)
        {
            List<IFilterable> dataToSend = new List<IFilterable>();
            if (ListOfAUECDates != null && ListOfAUECDates.Count > 0)
            {
                foreach (IFilterable publishdata in data)
                {
                    if (ListOfAUECDates.ContainsValue(publishdata.GetDate().Date))
                    {
                        dataToSend.Add(publishdata);
                    }
                }
            }
            int i = 0;
            Object[] filteredData = new Object[dataToSend.Count];
            foreach (IFilterable publishData in dataToSend)
            {
                filteredData[i] = publishData;
                i++;
            }
            data = filteredData;
            return dataToSend;
        }
    }
}
