using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    class CashTransactionDetailReconItem
    {
        private CorporateActionNameID _corporateActionNameID;

        /// <summary>
        /// Gets or sets the corporate action name ID.
        /// </summary>
        /// <value>The corporate action name ID.</value>
        public CorporateActionNameID CorporateActionNameID
        {
            get {
                if (_corporateActionNameID==null)
                {
                    _corporateActionNameID = new CorporateActionNameID();
                }
                return _corporateActionNameID; }
            set { _corporateActionNameID = value; }
        }

        private Symbol _symbol;

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>The symbol.</value>
        public Symbol Symbol
        {
            get {
                if (_symbol==null)
                {
                    _symbol = new Symbol();
                }
                return _symbol; }
            set { _symbol = value; }
        }

        private double _applicationAmount;

        /// <summary>
        /// Gets or sets the application amount.
        /// </summary>
        /// <value>The application amount.</value>
        public double ApplicationAmount
        {
            get { return _applicationAmount; }
            set { _applicationAmount = value; }
        }

        private double _manualAmount;

        /// <summary>
        /// Gets or sets the manual amount.
        /// </summary>
        /// <value>The manual amount.</value>
        public double ManualAmount
        {
            get { return _manualAmount; }
            set { _manualAmount = value; }
        }

        private double _difference;

        /// <summary>
        /// Gets or sets the difference.
        /// </summary>
        /// <value>The difference.</value>
        public double Difference
        {
            get { return _difference; }
            set { _difference = value; }
        }
	
    }
}
