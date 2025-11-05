using System.Collections.Generic;

namespace Prana.PM.BLL
{
    public class ReconLogic : IComparer<ReconPosition>
    {

        public int CompareOrder(ReconPosition mmidX, ReconPosition mmidY)
        {

            int symbolComp = mmidX.Symbol.CompareTo(mmidY.Symbol);
            //if both symbols are equal
            if (symbolComp == 0)
            {
                int sideComp = mmidX.OrderSideTagValue.CompareTo(mmidY.OrderSideTagValue);
                if (sideComp == 0)
                {
                    int quantityComp = mmidX.Quantity.CompareTo(mmidY.Quantity);
                    if (quantityComp == 0)
                    {
                        int accountCompare = mmidX.AccountName.CompareTo(mmidY.AccountName);
                        return accountCompare;
                    }
                    return quantityComp;
                }
                return sideComp;// return sideComp * (-1);
            }
            return symbolComp;
        }

        //public int Compare 

        #region IComparer<MarketMaker> Members


        //int IComparer<OrderRecon > .Compare(MarketMaker x, MarketMaker y)
        //{
        //    if (x.BidAsk == "BID")
        //    {
        //        return CompareOrder(x, y, -1);
        //    }
        //    else
        //    {
        //        return CompareOrder(x, y, 1);
        //    }
        //}

        #endregion

        #region IComparer<ReconPosition> Members

        public int Compare(ReconPosition x, ReconPosition y)
        {
            return CompareOrder(x, y);
            //    throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
