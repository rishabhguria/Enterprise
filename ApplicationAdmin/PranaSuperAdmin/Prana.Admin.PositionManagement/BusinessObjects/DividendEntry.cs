using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class DividendEntry : CorporateActionEntry
    {

        


        private DateTime _dividendXDate;

        /// <summary>
        /// Gets or sets the dividend X date.
        /// </summary>
        /// <value>The dividend X date.</value>
        public DateTime DividendXDate
        {
            get { return _dividendXDate; }
            set { _dividendXDate = value; }
        }

        private double _expectedDividend;

        public double ExpectedDividend
        {
            get { return _expectedDividend; }
            set { _expectedDividend = value; }
        }

        private long _positionQuantity;

        /// <summary>
        /// Gets or sets the position quantity.
        /// </summary>
        /// <value>The position quantity.</value>
        public long PositionQuantity
        {
            get { return _positionQuantity; }
            set { _positionQuantity = value; }
        }
	


    }
}
