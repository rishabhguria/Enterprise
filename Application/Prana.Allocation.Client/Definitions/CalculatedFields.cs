using System;
using System.Collections.Generic;

namespace Prana.Allocation.Client.Definitions
{
    internal class CalculatedFields
    {
        #region Members

        /// <summary>
        /// The _field name
        /// </summary>
        private string _fieldName;

        /// <summary>
        /// The _assets with commission in net amount
        /// </summary>
        private List<int> _assetsWithCommissionInNetAmount;

        /// <summary>
        /// The _precision format
        /// </summary>
        private string _precisionFormat;

        /// <summary>
        /// The _avg price
        /// </summary>
        private double _avgPrice;

        /// <summary>
        /// The _fxrate
        /// </summary>
        private double _fxrate;

        /// <summary>
        /// The _total commission and fee
        /// </summary>
        private double _totalCommissionAndFee;

        /// <summary>
        /// The _cum qty
        /// </summary>
        private double _cumQty;

        /// <summary>
        /// The _contract multiplier
        /// </summary>
        private double _contractMultiplier;

        /// <summary>
        /// The _FX conversion operator
        /// </summary>
        private string _fxConversionOperator;

        /// <summary>
        /// The _order side tag value
        /// </summary>
        private int _sideMultiplier;

        /// <summary>
        /// The _asset identifier
        /// </summary>
        private int _assetId;

        /// <summary>
        /// The _accrued interest
        /// </summary>
        private double _accruedInterest;

        /// <summary>
        /// The _auec loca date time/
        /// </summary>
        private DateTime _auecLocaDateTime;

        /// <summary>
        /// The currency identifier
        /// </summary>
        private int _currencyId;

        /// <summary>
        /// The vs currency identifier
        /// </summary>
        private int _vsCurrencyId;

        /// <summary>
        /// The lead currency identifier
        /// </summary>
        private int _leadCurrencyId;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the accrued interest.
        /// </summary>
        /// <value>
        /// The accrued interest.
        /// </value>
        public double AccruedInterest
        {
            get { return _accruedInterest; }
            set { _accruedInterest = value; }
        }

        /// <summary>
        /// Gets or sets the asset identifier.
        /// </summary>
        /// <value>
        /// The asset identifier.
        /// </value>
        public int AssetId
        {
            get { return _assetId; }
            set { _assetId = value; }
        }

        /// <summary>
        /// Gets or sets the assets with commission in net amount.
        /// </summary>
        /// <value>
        /// The assets with commission in net amount.
        /// </value>
        public List<int> AssetsWithCommissionInNetAmount
        {
            get { return _assetsWithCommissionInNetAmount; }
            set { _assetsWithCommissionInNetAmount = value; }
        }

        /// <summary>
        /// Gets or sets the auec loca date time.
        /// </summary>
        /// <value>
        /// The auec loca date time.
        /// </value>
        public DateTime AuecLocaDateTime
        {
            get { return _auecLocaDateTime; }
            set { _auecLocaDateTime = value; }
        }

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
        /// Gets or sets the cum qty.
        /// </summary>
        /// <value>
        /// The cum qty.
        /// </value>
        public double CumQty
        {
            get { return _cumQty; }
            set { _cumQty = value; }
        }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        /// <summary>
        /// Gets or sets the fx conversion operator.
        /// </summary>
        /// <value>
        /// The fx conversion operator.
        /// </value>
        public string FxConversionOperator
        {
            get { return _fxConversionOperator; }
            set { _fxConversionOperator = value; }
        }

        /// <summary>
        /// Gets or sets the fxrate.
        /// </summary>
        /// <value>
        /// The fxrate.
        /// </value>
        public double Fxrate
        {
            get { return _fxrate; }
            set
            {
                if (value != 0)
                    _fxrate = FxConversionOperator.Equals("D") ? (1 / value) : value;
                else
                    _fxrate = value;
            }
        }

        /// <summary>
        /// Gets the notional value.
        /// </summary>
        /// <value>
        /// The notional value.
        /// </value>
        public double NotionalValue { get { return AvgPrice * CumQty * ContractMultiplier; } }

        /// <summary>
        /// Gets or sets the precision format.
        /// </summary>
        /// <value>
        /// The precision format.
        /// </value>
        public string PrecisionFormat
        {
            get { return _precisionFormat; }
            set { _precisionFormat = value; }
        }

        /// <summary>
        /// Gets or sets the order side tag value.
        /// </summary>
        /// <value>
        /// The order side tag value.
        /// </value>
        public int SideMultiplier
        {
            get { return _sideMultiplier; }
            set { _sideMultiplier = value; }
        }

        /// <summary>
        /// Gets or sets the total commission and fee.
        /// </summary>
        /// <value>
        /// The total commission and fee.
        /// </value>
        public double TotalCommissionAndFee
        {
            get { return _totalCommissionAndFee; }
            set { _totalCommissionAndFee = value; }
        }

        /// <summary>
        /// Gets or sets the currency identifier.
        /// </summary>
        /// <value>
        /// The currency identifier.
        /// </value>
        public int CurrencyId
        {
            get { return _currencyId; }
            set { _currencyId = value; }
        }

        /// <summary>
        /// Gets or sets the vs currency identifier.
        /// </summary>
        /// <value>
        /// The vs currency identifier.
        /// </value>
        public int VSCurrencyId
        {
            get { return _vsCurrencyId; }
            set { _vsCurrencyId = value; }
        }

        /// <summary>
        /// Gets or sets the lead currency identifier.
        /// </summary>
        /// <value>
        /// The lead currency identifier.
        /// </value>
        public int LeadCurrencyId
        {
            get { return _leadCurrencyId; }
            set { _leadCurrencyId = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the total commission.
        /// </summary>
        /// <returns></returns>
        //private double GetTotalCommission()
        //{
        //    try
        //    {
        //        if ((AssetId == (int)AssetCategory.FixedIncome || AssetId == (int)AssetCategory.ConvertibleBond))
        //            return _totalCommissionAndFee + AccruedInterest;
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //            throw;
        //    }
        //    return _totalCommissionAndFee;
        //}

        #endregion Methods
    }
}
