using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
 
    public class AllocatedTrades 
    {
        private int _ID;

        /// <summary>
        /// Gets or sets the allocation ID.
        /// </summary>
        /// <value>The allocation ID.</value>
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _side;

        /// <summary>
        /// Gets or sets the side.
        /// </summary>
        /// <value>The side.</value>
        public string Side
        {
            get { return _side; }
            set { _side = value; }
        }

        private string _symbol;

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>The symbol.</value>
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private int _quantity;

        /// <summary>
        /// Gets or sets the executed quantity.
        /// </summary>
        /// <value>The executed quantity.</value>
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        private Fund _fund;

        /// <summary>
        /// Gets or sets the fund value.
        /// </summary>
        /// <value>The fund value.</value>
        public Fund FundValue
        {
           get
            {
                if (_fund == null)
                {
                    _fund = new Fund();
                }
                return _fund;
            }
            set { _fund = value; }
        }

        private double _averagePrice;

        /// <summary>
        /// Gets or sets the average price.
        /// </summary>
        /// <value>The average price.</value>
        public double AveragePrice
        {
            get { return _averagePrice; }
            set { _averagePrice = value; }
        }
    }

}
