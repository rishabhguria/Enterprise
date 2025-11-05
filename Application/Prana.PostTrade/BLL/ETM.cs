using System;
using System.Collections.Generic;
using System.Text;
using Prana.Interfaces;
using Prana.BusinessObjects.PositionManagement;
using Csla;
using System.ComponentModel;
using Prana.BusinessObjects;

namespace Prana.PostTrade
{
    class ETM : AlgoBase
    {
        #region IClosingAlgo Members

        public override void SortBuyList(List<AllocatedTrade> taxlots)
        {
            //Need to be sorted in Descending Order 
            taxlots.Sort(delegate(AllocatedTrade t1, AllocatedTrade t2) { return (t1.AveragePrice.CompareTo(t2.AveragePrice) * (-1)); });
        }

        public override void SortSellList(List<AllocatedTrade> taxlots)
        {
            //Need to be sorted in Ascending order for HIFO
            taxlots.Sort(delegate(AllocatedTrade t1, AllocatedTrade t2) { return t1.AveragePrice.CompareTo(t2.AveragePrice); });
        }
        public override List<AllocatedTrade> GetTaxLotsByDate(DateTime date, List<AllocatedTrade> listOftaxlots)
        {
            //DateTime oneyearbackdate = date.AddYears(1);
            DateTime oneyearbackdate = date.AddDays(-1).AddYears(-1);
            List<AllocatedTrade> taxlotList = new List<AllocatedTrade>();
            //DateTime matcheddate = date;
            foreach (AllocatedTrade sortedItem in listOftaxlots)
            {
                //if (oneyearbackdate <= sortedItem.TradeDate )
                //{
                if (sortedItem.TradeDate.Date <= oneyearbackdate)
                {
                    taxlotList.Add(sortedItem);
                    //matcheddate = sortedItem.TradeDate;
                }
                else
                {
                    break;
                }
                //if (sortedItem.TradeDate > matcheddate)
                //{
                //    break;
                //}
            }
            if (taxlotList.Count > 0)
            {
                return taxlotList;
            }
            return base.GetTaxLotsByDate(date, listOftaxlots);
        }
        #endregion
    }
}