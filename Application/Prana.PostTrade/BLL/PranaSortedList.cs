using System;
using System.Collections.Generic;

namespace Prana.PostTrade
{
    class PranaSortedList
    {
        SortedList<DateTime, string> _sortedList = new SortedList<DateTime, string>();

        //List<DateTime> list = new List<DateTime>();

        private void AddList(List<DateTime> buyTaxLotDates, List<DateTime> sellTaxLotDates)
        {
            foreach (DateTime date in sellTaxLotDates)
            {
                if (!_sortedList.ContainsKey(date.Date))
                {
                    _sortedList.Add(date.Date, "");
                }

            }
            foreach (DateTime date in buyTaxLotDates)
            {
                if (!_sortedList.ContainsKey(date.Date))
                {
                    _sortedList.Add(date.Date, "");
                }
            }
        }

        public IList<DateTime> getAllActionDates(List<DateTime> buyTaxLotDates, List<DateTime> sellTaxLotDates)
        {
            AddList(buyTaxLotDates, sellTaxLotDates);
            return _sortedList.Keys;
        }
    }
}
