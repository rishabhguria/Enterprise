using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{

    /// <summary>
    /// Example usage : FilterDataForLastDateModified is used for subscribing the closing data on PM. As PM requires the 
    /// closing data less than or equal to T-1. This is to remove the previous date's closed taxlots but does not update 
    /// the closing qty for any transactions done today but involved in closing. 
    /// </summary>
    [Serializable]

    public class FilterDataForLastDateModified : FilterData
    {
        private DateTime _tillDate;

        public DateTime TillDate
        {
            get { return _tillDate; }
            set { _tillDate = value; }
        }



        //filters the data for the last date modified of the object till the above date.
        public override List<IFilterable> Filterdata(ref object[] dataToFilter, string Topic, int userId)
        {
            List<IFilterable> dataToSend = new List<IFilterable>();
            try
            {

                Dictionary<string, IFilterable> dictDataToSend = new Dictionary<string, IFilterable>();
                foreach (IFilterable publishdata in dataToFilter)
                {
                    if (publishdata.GetDateModified().Date <= _tillDate.Date)
                    {
                        IKeyable keyableData = publishdata as IKeyable;
                        if (keyableData != null)
                        {
                            string key = keyableData.GetKey();
                            if (!dictDataToSend.ContainsKey(key))
                            {
                                dictDataToSend.Add(key, publishdata);
                            }
                            else
                            {
                                if (dictDataToSend[key].GetDateModified().Date <= publishdata.GetDateModified().Date)
                                {
                                    dictDataToSend[key] = publishdata;
                                }
                            }
                        }
                    }
                }
                dataToSend.AddRange(dictDataToSend.Values);
                Object[] filteredData = new Object[dataToSend.Count];
                int i = 0;
                foreach (IFilterable publishData in dataToSend)
                {
                    filteredData[i] = publishData;
                    i++;
                }
                dataToFilter = filteredData;
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }


            return dataToSend;
        }
    }
}
