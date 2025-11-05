using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects.Classes.PositionManagement
{
    [Serializable]
    public class FilterDataForSameUsers : FilterData
    {

        public int UserId { get; set; }

        /// <summary>
        /// If User Id passed is same as Filter User id then do not publish any data.
        /// </summary>
        /// <param name="dataToFilter"></param>
        /// <param name="Topic"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public override List<IFilterable> Filterdata(ref object[] dataToFilter, string Topic, int userId)
        {
            try
            {
                List<IFilterable> dataToSend = new List<IFilterable>();

                if (UserId != userId)
                    foreach (IFilterable publishdata in dataToFilter)
                    {
                        dataToSend.Add(publishdata);
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
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return new List<IFilterable>();
            }
        }
    }
}
