using Csla;
using System;

namespace Prana.PM.BLL
{
    [Serializable()]
    public class OrderSides : NameValueListBase<string, string>
    {

        private OrderSides()
        {
            IsReadOnly = false;
            this.Add(new NameValuePair(Prana.BusinessObjects.FIXConstants.SIDE_Buy, "BUY"));
            this.Add(new NameValuePair(Prana.BusinessObjects.FIXConstants.SIDE_Buy_Closed, "BUY TO COVER"));
            this.Add(new NameValuePair(Prana.BusinessObjects.FIXConstants.SIDE_Sell, "SELL"));
            this.Add(new NameValuePair(Prana.BusinessObjects.FIXConstants.SIDE_SellShort, "SELL SHORT"));
            IsReadOnly = true;
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <returns></returns>
        public static OrderSides GetList()
        {
            OrderSides sideList = new OrderSides();



            return sideList;


        }





    }
}
