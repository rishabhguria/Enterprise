using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.LogManager;
using System;
using System.Text;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// derived class for option Orders
    /// </summary>
    [Serializable]
    public class EPnLOrderOption : EPnlOrder
    {

        #region Properties
        /// <summary>
        /// Represents the Asset type the class represents
        /// </summary>
        private EPnLClassID _classID;
        public override EPnLClassID ClassID
        {
            get { return this._classID; }
        }

        private double _delta;
        public double Delta
        {
            get { return _delta; }
            set { _delta = value; }
        }

        private string _contractType;
        public string ContractType
        {
            get { return _contractType; }
            set { _contractType = value; }
        }

        private double _volatility;
        public double Volatility
        {
            get { return _volatility; }
            set { _volatility = value; }
        }

        private string _expirationDate;
        public string ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }

        private double _strikePrice;
        public double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }

        private double _underlyingMarkPrice;
        public double underlyingMarkprice
        {
            get { return _underlyingMarkPrice; }
            set { _underlyingMarkPrice = value; }
        }

        private bool _isCurrencyFuture;
        public bool IsCurrencyFuture
        {
            get { return _isCurrencyFuture; }
            set { _isCurrencyFuture = value; }
        }

        private OptionMoneyness _itmOtm;
        /// <summary>
        /// Gets or sets the itm otm.
        /// </summary>
        /// <value>
        /// The itm otm.
        /// </value>
        public OptionMoneyness ItmOtm
        {
            get { return _itmOtm; }
            set { _itmOtm = value; }
        }

        private double _percentOfITMOTM;
        /// <summary>
        /// Gets or sets the percent of underlying price.
        /// </summary>
        /// <value>
        /// The percent of underlying price.
        /// </value>
        public double PercentOfITMOTM
        {
            get { return _percentOfITMOTM; }
            set { _percentOfITMOTM = value; }
        }

        private double _intrinsicValue;
        /// <summary>
        /// Gets or sets the intrinsic value.
        /// </summary>
        /// <value>
        /// The intrinsic value.
        /// </value>
        public double IntrinsicValue
        {
            get { return _intrinsicValue; }
            set { _intrinsicValue = value; }
        }

        private int _daysToExpiry;
        /// <summary>
        /// Gets or sets the days to expiry.
        /// </summary>
        /// <value>
        /// The days to expiry.
        /// </value>
        public int DaysToExpiry
        {
            get { return _daysToExpiry; }
            set { _daysToExpiry = value; }
        }

        private double _gainLossIfExerciseAssign;
        /// <summary>
        /// Gets or sets the gain loss if exercise assign.
        /// </summary>
        /// <value>
        /// The gain loss if exercise assign.
        /// </value>
        public double GainLossIfExerciseAssign
        {
            get { return _gainLossIfExerciseAssign; }
            set { _gainLossIfExerciseAssign = value; }
        }

        #endregion

        public override string ToString()
        {
            string baseString = base.ToString();
            StringBuilder sb = new StringBuilder(baseString);
            sb.Append(this._delta);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(this._volatility);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(this._contractType);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(this._expirationDate);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(this._strikePrice);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(this._underlyingMarkPrice);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(this._isCurrencyFuture);
            sb.Append(Seperators.SEPERATOR_2);
            return sb.ToString();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EPnLOrderOption()
        {
            _classID = EPnLClassID.EPnLOrderOption;
            _delta = 0;
            _volatility = 0;
            _contractType = string.Empty;
            _expirationDate = string.Empty;
            _strikePrice = 0.0;
            _underlyingMarkPrice = 0.0;
            _isCurrencyFuture = false;
            _itmOtm = OptionMoneyness.NA;
            _percentOfITMOTM = 0.0;
            _intrinsicValue = 0.0;
            _daysToExpiry = 0;
            _gainLossIfExerciseAssign = 0.0;
        }

        /// <summary>
        /// returns new instance of this Order 
        /// </summary>
        /// <returns></returns>
        public new EPnLOrderOption Clone()
        {
            EPnLOrderOption optionOrder = new EPnLOrderOption();

            base.Clone(optionOrder);
            optionOrder.Delta = this._delta;
            optionOrder.Volatility = this._volatility;
            optionOrder.ContractType = this._contractType;
            optionOrder.StrikePrice = this._strikePrice;
            optionOrder.ExpirationDate = this._expirationDate;
            optionOrder.underlyingMarkprice = this._underlyingMarkPrice;
            optionOrder.IsCurrencyFuture = this._isCurrencyFuture;
            optionOrder.ItmOtm = this._itmOtm;
            optionOrder.PercentOfITMOTM = this._percentOfITMOTM;
            optionOrder.IntrinsicValue = this._intrinsicValue;
            optionOrder.DaysToExpiry = this._daysToExpiry;
            optionOrder.GainLossIfExerciseAssign = this._gainLossIfExerciseAssign;
            return optionOrder;
        }

        /// <summary>
        /// fills properties in Bindable object passed as argument
        /// </summary>
        /// <param name="bindableObject"></param>
        public override void GetBindableObject(ExposurePnlCacheItem bindableObject)
        {
            base.GetBindableObject(bindableObject);
            bindableObject.Delta = this._delta;
            bindableObject.Volatility = 100 * this._volatility;
            bindableObject.ContractType = this._contractType;
            bindableObject.StrikePrice = this._strikePrice;
            if (!string.IsNullOrEmpty(this.ExpirationDate))
            {
                bindableObject.ExpirationDate = Convert.ToDateTime(this._expirationDate);
            }
            else
                bindableObject.ExpirationDate = null;
            bindableObject.ItmOtm = this._itmOtm;
            bindableObject.PercentOfITMOTM = this._percentOfITMOTM;
            bindableObject.IntrinsicValue = this._intrinsicValue;
            bindableObject.DaysToExpiry = this._daysToExpiry;
            bindableObject.GainLossIfExerciseAssign = this._gainLossIfExerciseAssign;
        }

        public override void CopyBasicDetails(SecMasterBaseObj secMasterObject, bool isUnderlyingData)
        {
            try
            {
                base.CopyBasicDetails(secMasterObject, isUnderlyingData);
                if (!isUnderlyingData)
                {
                    SecMasterOptObj optObject = (SecMasterOptObj)secMasterObject;
                    if (optObject != null)
                    {
                        OsiSymbol = optObject.OSIOptionSymbol;
                        IdcoSymbol = optObject.IDCOOptionSymbol;
                        _expirationDate = optObject.ExpirationDate.ToShortDateString();
                        _strikePrice = optObject.StrikePrice;
                        _delta = optObject.Delta;
                        _contractType = ((OptionType)optObject.PutOrCall).ToString();
                        _isCurrencyFuture = optObject.IsCurrencyFuture;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}