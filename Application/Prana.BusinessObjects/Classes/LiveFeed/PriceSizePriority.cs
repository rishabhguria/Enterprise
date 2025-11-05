using System.Collections.Generic;

namespace Prana.BusinessObjects.LiveFeed
{
    public sealed class PriceSizePriority : IComparer<MarketMaker>
    {
        public int CompareOrder(MarketMaker mmidX, MarketMaker mmidY, int sortingOrder)
        {
            int priceComp = mmidX.Price.CompareTo(mmidY.Price);

            //if both prices are equal
            if (priceComp == 0)
            {
                int sizeComp = mmidX.Size.CompareTo(mmidY.Size);
                if (sizeComp == 0)
                {
                    int nameComp = mmidX.MMID.CompareTo(mmidY.MMID);
                    return nameComp;
                }
                return sizeComp * (-1);

            }
            return priceComp * sortingOrder;
        }

        //public int Compare 

        #region IComparer<MarketMaker> Members

        int IComparer<MarketMaker>.Compare(MarketMaker x, MarketMaker y)
        {
            if (x.BidAsk == "BID")
            {
                return CompareOrder(x, y, -1);
            }
            else
            {
                return CompareOrder(x, y, 1);
            }
        }

        #endregion
    }
}
