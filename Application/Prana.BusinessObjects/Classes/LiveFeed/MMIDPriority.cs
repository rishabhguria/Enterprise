using System.Collections.Generic;

namespace Prana.BusinessObjects.LiveFeed
{
    public class MMIDPriority : IComparer<MarketMaker>
    {
        #region IComparer<MarketMaker> Members

        public int Compare(MarketMaker x, MarketMaker y)
        {
            int nameComp = x.MMID.CompareTo(y.MMID);
            return nameComp;
        }

        #endregion
    }
}
