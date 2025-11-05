using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class BasicOrderInfo
    {
        #region Variable Declaration
        /// <summary>
        /// The symbol
        /// </summary>
        private string _symbol = string.Empty;
        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>
        /// The symbol.
        /// </value>
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }
        /// <summary>
        /// The auec identifier
        /// </summary>
        private int _auecID;
        /// <summary>
        /// Gets or sets the auecid.
        /// </summary>
        /// <value>
        /// The auecid.
        /// </value>
        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        /// <summary>
        /// The level1 identifier
        /// </summary>
        private int _level1ID = int.MaxValue;
        /// <summary>
        /// Gets or sets the level1 identifier.
        /// </summary>
        /// <value>
        /// The level1 identifier.
        /// </value>
        public int Level1ID
        {
            get { return _level1ID; }
            set { _level1ID = value; }
        }

        /// <summary>
        /// The average price
        /// </summary>
        private double _avgPrice = 0.0;
        /// <summary>
        /// Gets or sets the average price.
        /// </summary>
        /// <value>
        /// The average price.
        /// </value>
        public double AvgPrice
        {
            get { return _avgPrice; }
            set { _avgPrice = value; }
        }


        /// <summary>
        /// The quantity
        /// </summary>
        private double _quantity = 0.0;
        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public virtual double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        /// <summary>
        /// The contract multiplier
        /// </summary>
        private double _contractMultiplier = 0.0;
        /// <summary>
        /// Gets or sets the contract multiplier.
        /// </summary>
        /// <value>
        /// The contract multiplier.
        /// </value>
        public double ContractMultiplier
        {
            get { return _contractMultiplier; }
            set { _contractMultiplier = value; }
        }

        /// <summary>
        /// The order side tag value
        /// </summary>
        private string _orderSideTagValue = string.Empty;
        /// <summary>
        /// Gets or sets the order side tag value.
        /// </summary>
        /// <value>
        /// The order side tag value.
        /// </value>
        public string OrderSideTagValue
        {
            get { return _orderSideTagValue; }
            set { _orderSideTagValue = value; }
        }

        /// <summary>
        /// The counter party identifier
        /// </summary>
        private int _counterPartyID = int.MinValue;

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
        /// The venue identifier
        /// </summary>
        private int _venueID = int.MinValue;

        /// <summary>
        /// Gets or sets the venue identifier.
        /// </summary>
        /// <value>
        /// The venue identifier.
        /// </value>
        public int VenueID
        {
            get { return _venueID; }
            set { _venueID = value; }
        }

        /// <summary>
        /// The commision
        /// </summary>
        private double _commision = 0.0;
        /// <summary>
        /// Gets or sets the commission.
        /// </summary>
        /// <value>
        /// The commission.
        /// </value>
        public double Commission
        {
            get { return _commision; }
            set { _commision = value; }
        }


        private double _secFee = 0.0;
        public double SecFee
        {
            get { return _secFee; }
            set { _secFee = value; }
        }

        /// <summary>
        /// The soft commision
        /// </summary>
        private double _softCommision = 0.0;
        /// <summary>
        /// Gets or sets the soft commission.
        /// </summary>
        /// <value>
        /// The soft commission.
        /// </value>
        public double SoftCommission
        {
            get { return _softCommision; }
            set { _softCommision = value; }
        }

        /// <summary>
        /// The stamp duty
        /// </summary>
        private double _stampDuty;
        /// <summary>
        /// Gets or sets the stamp duty.
        /// </summary>
        /// <value>
        /// The stamp duty.
        /// </value>
        public double StampDuty
        {
            get { return _stampDuty; }
            set
            {
                _stampDuty = value;
            }
        }

        /// <summary>
        /// The transaction levy
        /// </summary>
        private double _transactionLevy;
        /// <summary>
        /// Gets or sets the TransactionLevy.
        /// </summary>
        /// <value>The TransactionLevy.</value>
        public double TransactionLevy
        {
            get { return _transactionLevy; }
            set
            {
                _transactionLevy = value;
            }
        }

        /// <summary>
        /// The clearing fee
        /// </summary>
        private double _clearingFee;
        /// <summary>
        /// Gets or sets the ClearingFee.
        /// </summary>
        /// <value>The ClearingFee.</value>
        public double ClearingFee
        {
            get { return _clearingFee; }
            set
            {
                _clearingFee = value;
            }
        }

        /// <summary>
        /// The tax on commissions
        /// </summary>
        private double _taxOnCommissions;
        /// <summary>
        /// Gets or sets the TaxOnCommissions.
        /// </summary>
        /// <value>The TaxOnCommissions.</value>
        public double TaxOnCommissions
        {
            get { return _taxOnCommissions; }
            set
            {
                _taxOnCommissions = value;
            }
        }

        /// <summary>
        /// The misc fees
        /// </summary>
        private double _miscFees;
        /// <summary>
        /// Gets or sets the MiscFees.
        /// </summary>
        /// <value>The MiscFees.</value>
        public double MiscFees
        {
            get { return _miscFees; }
            set
            {
                _miscFees = value;
            }
        }

        /// <summary>
        /// The occ fee
        /// </summary>
        private double _occFee;
        /// <summary>
        /// Gets or sets the OccFee.
        /// </summary>
        /// <value>The OccFee.</value>
        public double OccFee
        {
            get { return _occFee; }
            set
            {
                _occFee = value;
            }
        }

        /// <summary>
        /// The orf fee
        /// </summary>
        private double _orfFee;
        /// <summary>
        /// Gets or sets the OrfFee.
        /// </summary>
        /// <value>The OrfFee.</value>
        public double OrfFee
        {
            get { return _orfFee; }
            set
            {
                _orfFee = value;
            }
        }

        /// <summary>
        /// The clearing broker fee
        /// </summary>
        private double _clearingBrokerFee = 0.0;
        /// <summary>
        /// Gets or sets the Clearing Broker Fee.
        /// </summary>
        /// <value>The Clearing Broker Fee.</value>
        public double ClearingBrokerFee
        {
            get { return _clearingBrokerFee; }
            set { _clearingBrokerFee = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicOrderInfo"/> class.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="auecID">The auec identifier.</param>
        /// <param name="level1ID">The level1 identifier.</param>
        /// <param name="avgPrice">The average price.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="contractMultiplier">The contract multiplier.</param>
        /// <param name="orderSideTagValue">The order side tag value.</param>
        public BasicOrderInfo(string symbol, int auecID, int level1ID, double avgPrice, double quantity, double contractMultiplier, string orderSideTagValue)
        {
            _symbol = symbol;
            _auecID = auecID;
            _level1ID = level1ID;
            _avgPrice = avgPrice;
            _quantity = quantity;
            _contractMultiplier = contractMultiplier;
            _orderSideTagValue = orderSideTagValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicOrderInfo"/> class.
        /// </summary>
        /// <param name="otcPos">The otc position.</param>
        public BasicOrderInfo(OTCPosition otcPos)
        {
            _symbol = otcPos.Symbol;
            _auecID = otcPos.AUECID;
            _level1ID = otcPos.AccountID;
            _avgPrice = otcPos.AveragePrice;
            _quantity = otcPos.PositionStartQuantity;
            _contractMultiplier = otcPos.Multiplier;
            _orderSideTagValue = otcPos.SideTagValue;
            _counterPartyID = otcPos.CounterPartyID;
            _venueID = otcPos.VenueID;
        }
    }
}
