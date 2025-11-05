using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Prana.BusinessObjects.PositionManagement;
namespace Prana.PostTrade
{
    class FIFO:AlgoBase
    {
        public override void SortBuyList(List<Prana.BusinessObjects.PositionManagement.AllocatedTrade> taxlots)
        {
            taxlots.Sort(delegate(AllocatedTrade t1, AllocatedTrade t2) { return t1.TradeDate.Date.CompareTo(t2.TradeDate.Date); });
        }

        public override void SortSellList(List<Prana.BusinessObjects.PositionManagement.AllocatedTrade> taxlots)
        {
            taxlots.Sort(delegate(AllocatedTrade t1, AllocatedTrade t2) { return t1.TradeDate.Date.CompareTo(t2.TradeDate.Date); });
        }

    }
}
