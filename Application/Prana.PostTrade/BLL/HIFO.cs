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
    class HIFO : AlgoBase
    {
       #region IClosingAlgo Members

        public override void SortBuyList(List<AllocatedTrade> taxlots)
        {
            //Need to be sorted in Descending Order 
        taxlots.Sort(delegate(AllocatedTrade t1, AllocatedTrade t2) { return (t1.AveragePrice.CompareTo(t2.AveragePrice)*(-1)); });
        }

        public override void SortSellList(List<AllocatedTrade> taxlots)
        {
            //Need to be sorted in Ascending order for HIFO
            taxlots.Sort(delegate(AllocatedTrade t1, AllocatedTrade t2) { return t1.AveragePrice.CompareTo(t2.AveragePrice); });
        }
     #endregion
    }
}