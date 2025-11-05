using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class Position
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


        private DateTime _lastActivityDate;

        /// <summary>
        /// Gets or sets the last activity date.
        /// This is the date when this position was last updated. 
        /// </summary>
        /// <value>The last activity date.</value>
        public DateTime LastActivityDate
        {
            get { return _lastActivityDate; }
            set { _lastActivityDate = value; }
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
        /// This will be the net quantity in the , fund, symbol.
        /// 
        /// </summary>
        /// <value>The executed quantity.</value>
        public int Quantity
        {
            get {                     
                //Sugandh todo - it will be calculated here based on the quantities and their sides in the allocated trades collection
                // lying in this collection.
                  return _quantity; }
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

        private PositionType _positionType;

        public PositionType PositionType
        {
            get { return _positionType; }
            set { _positionType = value; }
        }
	


        SortableSearchableList<AllocatedTrades> allocatedTrades = new SortableSearchableList<AllocatedTrades>();
	
        
    }
}
