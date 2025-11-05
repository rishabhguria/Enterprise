using System;
using System.Collections.Generic;

namespace Prana.Allocation.Client.Definitions
{
    public class BulkChangesGroupLevel
    {
        #region Members

        /// <summary>
        /// The _avg price
        /// </summary>
        private decimal _avgPrice = 0;

        /// <summary>
        /// The _avg price rounding
        /// </summary>
        private int _avgPriceRounding = -1;

        /// <summary>
        /// The _ fx rate
        /// </summary>
        private decimal _FXRate = 0;

        /// <summary>
        /// The _accrued interest
        /// </summary>
        private decimal _accruedInterest;

        /// <summary>
        /// The _description
        /// </summary>
        private string _description = string.Empty;

        /// <summary>
        /// The _internal comments
        /// </summary>
        private string _internalComments = null;

        /// <summary>
        /// The _counter party identifier
        /// </summary>
        private int _counterPartyID = int.MinValue;

        /// <summary>
        /// The _FX conversion operator
        /// </summary>
        private string _fxConversionOperator = string.Empty;

        /// <summary>
        /// The _group wise
        /// </summary>
        private bool _groupWise = true;

        /// <summary>
        /// The _account i ds
        /// </summary>
        private List<int> _accountIDs = null;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the account i ds.
        /// </summary>
        /// <value>
        /// The account i ds.
        /// </value>
        public List<int> AccountIDs
        {
            get { return _accountIDs; }
            set { _accountIDs = value; }
        }

        /// <summary>
        /// Gets or sets the accrued interest.
        /// </summary>
        /// <value>
        /// The accrued interest.
        /// </value>
        public decimal AccruedInterest
        {
            get { return _accruedInterest; }
            set { _accruedInterest = value; }
        }

        /// <summary>
        /// Gets or sets the average price.
        /// </summary>
        /// <value>
        /// The average price.
        /// </value>
        public decimal AvgPrice
        {
            get { return _avgPrice; }
            set { _avgPrice = value; }
        }

        /// <summary>
        /// Gets or sets the average price rounding.
        /// </summary>
        /// <value>
        /// The average price rounding.
        /// </value>
        public int AvgPriceRounding
        {
            get { return _avgPriceRounding; }
            set { _avgPriceRounding = value; }
        }

        /// <summary>
        /// Gets or sets the counter party identifier.
        /// </summary>
        /// <value>
        /// The counter party identifier.
        /// </value>
        public int CounterPartyID
        {
            get { return _counterPartyID; }
            set { _counterPartyID = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Gets or sets the fx conversion operator.
        /// </summary>
        /// <value>
        /// The fx conversion operator.
        /// </value>
        public string FXConversionOperator
        {
            get { return _fxConversionOperator; }
            set { _fxConversionOperator = value; }
        }

        /// <summary>
        /// Gets or sets the fx rate.
        /// </summary>
        /// <value>
        /// The fx rate.
        /// </value>
        public decimal FXRate
        {
            get { return _FXRate; }
            set { _FXRate = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [group wise].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [group wise]; otherwise, <c>false</c>.
        /// </value>
        public bool GroupWise
        {
            get { return _groupWise; }
            set { _groupWise = value; }
        }

        /// <summary>
        /// Gets or sets the internal comments.
        /// </summary>
        /// <value>
        /// The internal comments.
        /// </value>
        public String InternalComments
        {
            get { return _internalComments; }
            set { _internalComments = value; }
        }

        #endregion Properties
    }
}
