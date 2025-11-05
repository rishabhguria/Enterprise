using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.BasketTrading
{
    public class CxlRplceDetails
    {

        private string _orderType = string.Empty;
        private int _priceType = int.MinValue;
        private double _pricePercentage = double.MinValue;
        private double _quantityPercentage = double.MinValue;


        public string OrderType
        {
            get { return _orderType; }
            set { _orderType = value; }
        }

        public int PriceType
        {
            get { return _priceType; }
            set { _priceType = value; }
        }

        public double PricePercentage
        {
            get { return _pricePercentage; }
            set { _pricePercentage = value; }
        }

        public double QuantityPercentage
        {
            get { return _quantityPercentage; }
            set { _quantityPercentage = value; }
        }
        
    }
}
