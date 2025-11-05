using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Prana.BusinessObjects.PositionManagement;

namespace Prana.PostTrade
{
    class MFIFO : AlgoBase
    {


        public override void SortBuyList(List<Prana.BusinessObjects.PositionManagement.AllocatedTrade> taxlots)
        {
            taxlots.Sort(delegate(AllocatedTrade t1, AllocatedTrade t2) { return t1.TradeDate.Date.CompareTo(t2.TradeDate.Date); });
        }

        public override void SortSellList(List<Prana.BusinessObjects.PositionManagement.AllocatedTrade> taxlots)
        {
            taxlots.Sort(delegate(AllocatedTrade t1, AllocatedTrade t2) { return t1.TradeDate.Date.CompareTo(t2.TradeDate.Date); });
        }

        public override List<AllocatedTrade> GetTaxLotsByDate(DateTime date, List<AllocatedTrade> listOftaxlots)
        {
           
            //DateTime oneyearbackdate = date.AddDays(-1).AddYears(-1);
            List<AllocatedTrade> taxlotList = new List<AllocatedTrade>();

            foreach (AllocatedTrade sortedItem in listOftaxlots)
            {
               
                if (sortedItem.TradeDate.Date == date)
                {
                    taxlotList.Add(sortedItem);
                    
                }
                if(sortedItem.TradeDate.Date > date)
                {
                    break;
                }
             }
            
            if (taxlotList.Count > 0)
            {
                return taxlotList;
            }
            
            return base.GetTaxLotsByDate(date, listOftaxlots);
        }
     
    }
}
