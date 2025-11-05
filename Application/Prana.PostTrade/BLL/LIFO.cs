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
    class LIFO : AlgoBase
    {
        #region IClosingAlgo Members
        
        //For LIFO both buy and sell taxlots list are sorted in descending order on the basis of Trade date

        public override void SortBuyList(List<AllocatedTrade> taxlots)
        {
            taxlots.Sort(delegate(AllocatedTrade t1, AllocatedTrade t2) { return (t1.TradeDate.Date.CompareTo(t2.TradeDate.Date) * (-1)); });
        }

        public override void SortSellList(List<AllocatedTrade> taxlots)
        {
            taxlots.Sort(delegate(AllocatedTrade t1, AllocatedTrade t2) { return (t1.TradeDate.Date.CompareTo(t2.TradeDate.Date) * (-1)); });
        }

        #endregion
    }
}