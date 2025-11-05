//using Prana.PM.ApplicationConstants;
using Csla;
using Csla.Validation;
using Newtonsoft.Json.Linq;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Prana.BusinessObjects.PositionManagement
{
    [XmlInclude(typeof(Account))]
    [Serializable]
    [System.Runtime.InteropServices.ComVisible(false)]
    public class Position : BusinessBase<Position>, IKeyable, IFilterable, INotifyPropertyChangedCustom
    {
        #region Constants
        const string CONST_Symbol = "Symbol";
        const string CONST_PositionStartQuantity = "PositionStartQuantity";
        const string CONST_AveragePrice = "OpenAveragePrice";
        const string CONST_AUECID = "AUECID";
        const string CONST_CASHSETTLEDPRICE = "CashSettledPrice";
        const int CONST_ZERO = 0;
        const double CONST_AVG_PRICE_ZERO = 0.0;
        #endregion

        public Position()
        {
            MarkAsChild();
        }

        private string _ID = string.Empty;
        /// <summary>
        /// Gets or sets the Start Taxlot ID.
        /// </summary>
        /// <value>The Start Taxlot ID.</value>        
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _closingID;
        public string ClosingID
        {
            get { return _closingID; }
            set { _closingID = value; }
        }

        private bool _isUnwind = false;
        [Browsable(false)]
        public bool IsUnwind
        {
            get { return _isUnwind; }
            set { _isUnwind = value; }
        }

        private DateTime _tradeDate = DateTimeConstants.MinValue;
        /// <summary>
        /// Gets or sets the last activity date.
        /// This is the date when this position was last updated. 
        /// This is similar to the the AUECPositionCloseDate.
        /// </summary>
        /// <value>The last activity date.</value>
        // [Browsable(false)]
        public DateTime TradeDate
        {
            get { return _tradeDate; }
            set { _tradeDate = value; }
        }

        private DateTime _closingTradeDate = DateTimeConstants.MinValue;
        /// <summary>
        /// Gets or sets the last activity date.
        /// This is the date when this position was last updated. 
        /// This is similar to the the AUECPositionCloseDate.
        /// </summary>
        /// <value>The last activity date.</value>
        public DateTime ClosingTradeDate
        {
            get { return _closingTradeDate; }
            set { _closingTradeDate = value; }
        }

        private string _description;
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [XmlIgnore]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _symbol;
        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>The symbol.</value>
        [XmlIgnore]
        public string Symbol
        {
            get { return _symbol; }
            set
            {
                _symbol = value;
            }
        }

        private int _accountID;
        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        private Account _account;
        /// <summary>
        /// Gets or sets the account value.
        /// </summary>
        /// <value>The account value.</value>
        public Account AccountValue
        {
            get
            {
                if (_account == null)
                {
                    _account = new Account();
                }
                return _account;
            }
            set { _account = value; }
        }

        private double _openAveragePrice = 0.0;
        /// <summary>
        /// Gets or sets the average price.
        /// </summary>
        /// <value>The average price.</value>
        //[XmlIgnore]
        public double OpenAveragePrice
        {
            get { return _openAveragePrice; }
            set
            {
                _openAveragePrice = value;
            }
        }

        private double _closedAveragePrice = 0.0;
        /// <summary>
        /// Gets or sets the average price.
        /// </summary>
        /// <value>The average price.</value>
        //[XmlIgnore]
        public double ClosedAveragePrice
        {
            get { return _closedAveragePrice; }
            set
            {
                _closedAveragePrice = value;
            }
        }


        private string _side;
        /// <summary>
        /// Gets or sets the side.
        /// </summary>
        /// <value>The side.</value>
        [XmlIgnore]
        public string Side
        {
            get { return _side; }
            set { _side = value; }
        }
        private string _closingSide;
        /// <summary>
        /// Gets or sets the side.
        /// </summary>
        /// <value>The side.</value>
        // [Browsable(false)]
        [XmlIgnore]
        public string ClosingSide
        {
            get { return _closingSide; }
            set { _closingSide = value; }
        }

        private bool _isExpired_Settled = false;
        [XmlIgnore]
        public bool IsExpired_Settled
        {
            get { return _isExpired_Settled; }
            set
            {
                _isExpired_Settled = value;

            }
        }

        private bool? _isManualyExerciseAssign = null;
        [Browsable(false)]
        public virtual bool? IsManualyExerciseAssign
        {
            get { return _isManualyExerciseAssign; }
            set { _isManualyExerciseAssign = value; }
        }

        private AssetCategory _assetCategoryValue = AssetCategory.Equity;
        /// <summary>
        /// determines the asset type on the setting of auecid
        /// </summary>
        [XmlIgnore]
        public AssetCategory AssetCategoryValue
        {
            get { return _assetCategoryValue; }
            set { _assetCategoryValue = value; }
        }
        //private BusinessObjects.AppConstants.Underlying _underlying;

        protected string _underlyingName = string.Empty;
        public string UnderlyingName
        {
            get { return _underlyingName; }
            set { _underlyingName = value; }
        }

        private string _exchange;
        public string Exchange
        {
            get { return _exchange; }
            set { _exchange = value; }
        }

        private int _currencyID;
        public int CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }
        }

        private string _currency;
        public string Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        private int _auecID;
        [XmlIgnore]
        public int AUECID
        {
            get { return _auecID; }
            set
            {
                _auecID = value;
            }
        }

        private double _notionalValue = 0;
        [XmlIgnore]
        public double NotionalValue
        {
            get { return _notionalValue; }
            set { _notionalValue = value; }
        }
        private double _multiplier = 1;

        /// <summary>
        /// Gets or sets the multiplier.
        /// </summary>
        /// <value>The multiplier.</value>
        [XmlIgnore]
        public double Multiplier
        {
            get { return _multiplier; }
            set { _multiplier = value; }
        }

        protected int _vsCurrencyID;
        [Browsable(false)]
        public virtual int VsCurrencyID
        {
            get { return _vsCurrencyID; }
            set { _vsCurrencyID = value; }
        }

        protected int _tradedCurrencyID;
        [Browsable(false)]
        public virtual int LeadCurrencyID
        {
            get { return _tradedCurrencyID; }
            set { _tradedCurrencyID = value; }
        }

        private bool _isNDF;
        [Browsable(false)]
        public bool IsNDF
        {
            get { return _isNDF; }
            set { _isNDF = value; }
        }

        //Narendra Kumar Jangir, Sept 13 2013
        //IsCurrencySettle field is newly introduced to remove ambiguity of isNDF, now it is in user's hand that he want PNL or actual currencies.
        //For FX/FX-Forward only
        private bool _isCurrencySettle;
        [Browsable(false)]
        public bool IsCurrencySettle
        {
            get { return _isCurrencySettle; }
            set { _isCurrencySettle = value; }
        }

        private double _fxRate;
        [Browsable(false)]
        public double FxRate
        {
            get { return _fxRate; }
            set { _fxRate = value; }
        }

        #region Quantity fields
        private double _closedQty;
        /// <summary>
        /// Gets or sets my property.
        /// </summary>
        /// <value>My property.</value>
        public double ClosedQty
        {
            get { return _closedQty; }
            set { _closedQty = value; }
        }

        private double _notionalChange = 0;
        /// <summary>
        /// Gets or sets my property.
        /// </summary>
        /// <value>My property.</value>
        public double NotionalChange
        {
            get { return _notionalChange; }
            set { _notionalChange = value; }
        }

        private DateTime _timeOfSaveUTC;
        ///Bring the data in the utc datetime and assign after converting to local date time.
        ///Always returns the local datetime while always sets datetime in UTC.
        ///changed this logic thus we are showing aueclocaldate Abhishek
        public DateTime TimeOfSaveUTC
        {
            get { return _timeOfSaveUTC; }
            set { _timeOfSaveUTC = value; }
        }

        private string _taxLotClosingId;
        [Browsable(false)]
        public string TaxLotClosingId
        {
            get { return _taxLotClosingId; }
            set { _taxLotClosingId = value; }
        }
        #endregion

        #region Commission & Fees fields
        private double _positionTotalCommissionandFees = 0.0;

        /// <summary>
        /// Gets or sets the commission.
        /// </summary>
        /// <value>The commission.</value>
        [XmlIgnore]
        public double PositionTotalCommissionandFees
        {
            get { return _positionTotalCommissionandFees; }
            set { _positionTotalCommissionandFees = value; }
        }

        private double _closingTotalCommissionandFees = 0.0;

        [XmlIgnore]
        public double ClosingTotalCommissionandFees
        {
            get { return _closingTotalCommissionandFees; }
            set { _closingTotalCommissionandFees = value; }

        }

        private int _intClosingMode;
        /// <summary>
        /// We have created this property as we needed to bind the enum type property to grid combo
        // TODO : Remove this if possible.
        /// </summary>
        [Browsable(false)]
        public int IntClosingMode
        {
            get { return _intClosingMode; }
            set
            {
                _intClosingMode = value;
                _closingMode = (ClosingMode)value;
            }
        }
        private PositionTag _positionalTag;

        public PositionTag PositionalTag
        {
            get { return _positionalTag; }
            set { _positionalTag = value; }
        }

        private PositionTag _closingPositiontag;
        public PositionTag ClosingPositionTag
        {
            get { return _closingPositiontag; }
            set { _closingPositiontag = value; }
        }

        private ClosingMode _closingMode = ClosingMode.None;
        /// <summary>
        /// Gets or sets the taxlots which create the position
        /// Child taxlots which creates this position
        /// </summary>
        /// <value>The position allocated trades.</value>

        public ClosingMode ClosingMode
        {
            get { return _closingMode; }
            set
            {
                _closingMode = value;
                _intClosingMode = Convert.ToInt32(_closingMode);
            }
        }
        #endregion

        private double _costBasisRealizedPNL = 0.0;
        [System.Xml.Serialization.XmlIgnore()]
        public double CostBasisRealizedPNL
        {
            get { return _costBasisRealizedPNL; }
            set { _costBasisRealizedPNL = value; }
        }

        [Browsable(false)]
        public double CostBasisRealizedPNLBase
        {
            get
            {
                return (_costBasisRealizedPNL * _fxRate);
            }
        }

        private double _costBasisGrossPNL = 0.0;
        [System.Xml.Serialization.XmlIgnore()]
        public double CostBasisGrossPNL
        {
            get { return _costBasisGrossPNL; }
            set { _costBasisGrossPNL = value; }
        }

        [Browsable(false)]
        public double CostBasisGrossPNLBase
        {
            get
            {
                return (_costBasisGrossPNL * _fxRate);
            }
        }

        private int _strategyID;
        [XmlIgnore]
        public int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }

        private String _strategy;
        [XmlIgnore]
        public String Strategy
        {
            get { return _strategy; }
            set { _strategy = value; }
        }

        private string _positionSide;
        public string PositionSide
        {
            get { return _positionSide; }
            set { _positionSide = value; }
        }

        //http://jira.nirvanasolutions.com:8080/browse/CHMW-1602
        private int _closingAlgo;
        public int ClosingAlgo
        {
            get { return _closingAlgo; }
            set { _closingAlgo = value; }
        }

        //Narendra Kumar Jangir, Oct 10 2013
        //PositionState is added to check that Opening or closing transaction is updated from edit trade UI.
        //For FX and futures closed trade, updated position will be published to cash activity so that PNL can be updated
        private Prana.Global.ApplicationConstants.TaxLotState _positionState;
        [Browsable(false)]
        public virtual Prana.Global.ApplicationConstants.TaxLotState PositionState
        {
            get { return _positionState; }
            set { _positionState = value; }
        }

        #region Swap Params


        private SwapParameters _positionswapParameters;
        [Browsable(false)]
        public SwapParameters PositionSwapParameters
        {
            get { return _positionswapParameters; }
            set { _positionswapParameters = value; }
        }
        private SwapParameters _closedswapParameters;
        [Browsable(false)]
        public SwapParameters ClosedSwapParameters
        {
            get { return _closedswapParameters; }
            set { _closedswapParameters = value; }
        }
        private bool _isPositionSwapped = false;

        [Browsable(false)]
        public bool IsPositionSwapped
        {
            get { return _isPositionSwapped; }
            set { _isPositionSwapped = value; }
        }
        private bool _isClosedSwapped = false;

        [Browsable(false)]
        public bool IsClosedSwapped
        {
            get { return _isClosedSwapped; }
            set { _isClosedSwapped = value; }
        }

        private bool _isClosingSaved = true;
        [Browsable(false)]
        public bool IsClosingSaved
        {
            get { return _isClosingSaved; }
            set { _isClosingSaved = value; }
        }
        #endregion

        protected override object GetIdValue()
        {
            return _taxLotClosingId;
        }

        [Browsable(false)]
        public virtual bool? IsCopyTradeAttrbsPrefUsed{ get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value><c>true</c> if this instance is dirty; otherwise, <c>false</c>.</value>
        [Browsable(false)]
        [XmlIgnore]
        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

        #region ICloneable Members



        #endregion

        public void CalculateCommissionAndFees()
        {
            //CalculateCommission();
            //CalculateFees();
            //CalculateOtherFees();
        }

        #region Trade Attribute members
        public string TradeAttribute1 { get; set; } = string.Empty;
        public string TradeAttribute2 { get; set; } = string.Empty;
        public string TradeAttribute3 { get; set; } = string.Empty;
        public string TradeAttribute4 { get; set; } = string.Empty;
        public string TradeAttribute5 { get; set; } = string.Empty;
        public string TradeAttribute6 { get; set; } = string.Empty;
        public string TradeAttribute7 { get; set; } = string.Empty;
        public string TradeAttribute8 { get; set; } = string.Empty;
        public string TradeAttribute9 { get; set; } = string.Empty;
        public string TradeAttribute10 { get; set; } = string.Empty;
        public string TradeAttribute11 { get; set; } = string.Empty;
        public string TradeAttribute12 { get; set; } = string.Empty;
        public string TradeAttribute13 { get; set; } = string.Empty;
        public string TradeAttribute14 { get; set; } = string.Empty;
        public string TradeAttribute15 { get; set; } = string.Empty;
        public string TradeAttribute16 { get; set; } = string.Empty;
        public string TradeAttribute17 { get; set; } = string.Empty;
        public string TradeAttribute18 { get; set; } = string.Empty;
        public string TradeAttribute19 { get; set; } = string.Empty;
        public string TradeAttribute20 { get; set; } = string.Empty;
        public string TradeAttribute21 { get; set; } = string.Empty;
        public string TradeAttribute22 { get; set; } = string.Empty;
        public string TradeAttribute23 { get; set; } = string.Empty;
        public string TradeAttribute24 { get; set; } = string.Empty;
        public string TradeAttribute25 { get; set; } = string.Empty;
        public string TradeAttribute26 { get; set; } = string.Empty;
        public string TradeAttribute27 { get; set; } = string.Empty;
        public string TradeAttribute28 { get; set; } = string.Empty;
        public string TradeAttribute29 { get; set; } = string.Empty;
        public string TradeAttribute30 { get; set; } = string.Empty;
        public string TradeAttribute31 { get; set; } = string.Empty;
        public string TradeAttribute32 { get; set; } = string.Empty;
        public string TradeAttribute33 { get; set; } = string.Empty;
        public string TradeAttribute34 { get; set; } = string.Empty;
        public string TradeAttribute35 { get; set; } = string.Empty;
        public string TradeAttribute36 { get; set; } = string.Empty;
        public string TradeAttribute37 { get; set; } = string.Empty;
        public string TradeAttribute38 { get; set; } = string.Empty;
        public string TradeAttribute39 { get; set; } = string.Empty;
        public string TradeAttribute40 { get; set; } = string.Empty;
        public string TradeAttribute41 { get; set; } = string.Empty;
        public string TradeAttribute42 { get; set; } = string.Empty;
        public string TradeAttribute43 { get; set; } = string.Empty;
        public string TradeAttribute44 { get; set; } = string.Empty;
        public string TradeAttribute45 { get; set; } = string.Empty;
        #endregion

        #region Trade Attribute Utilities
        private static readonly Dictionary<string, Action<Position, string>> _attributeSetters =
            new Dictionary<string, Action<Position, string>>
            {
                { "TradeAttribute7",  (obj, val) => obj.TradeAttribute7 = val },
                { "TradeAttribute8",  (obj, val) => obj.TradeAttribute8 = val },
                { "TradeAttribute9",  (obj, val) => obj.TradeAttribute9 = val },
                { "TradeAttribute10", (obj, val) => obj.TradeAttribute10 = val },
                { "TradeAttribute11", (obj, val) => obj.TradeAttribute11 = val },
                { "TradeAttribute12", (obj, val) => obj.TradeAttribute12 = val },
                { "TradeAttribute13", (obj, val) => obj.TradeAttribute13 = val },
                { "TradeAttribute14", (obj, val) => obj.TradeAttribute14 = val },
                { "TradeAttribute15", (obj, val) => obj.TradeAttribute15 = val },
                { "TradeAttribute16", (obj, val) => obj.TradeAttribute16 = val },
                { "TradeAttribute17", (obj, val) => obj.TradeAttribute17 = val },
                { "TradeAttribute18", (obj, val) => obj.TradeAttribute18 = val },
                { "TradeAttribute19", (obj, val) => obj.TradeAttribute19 = val },
                { "TradeAttribute20", (obj, val) => obj.TradeAttribute20 = val },
                { "TradeAttribute21", (obj, val) => obj.TradeAttribute21 = val },
                { "TradeAttribute22", (obj, val) => obj.TradeAttribute22 = val },
                { "TradeAttribute23", (obj, val) => obj.TradeAttribute23 = val },
                { "TradeAttribute24", (obj, val) => obj.TradeAttribute24 = val },
                { "TradeAttribute25", (obj, val) => obj.TradeAttribute25 = val },
                { "TradeAttribute26", (obj, val) => obj.TradeAttribute26 = val },
                { "TradeAttribute27", (obj, val) => obj.TradeAttribute27 = val },
                { "TradeAttribute28", (obj, val) => obj.TradeAttribute28 = val },
                { "TradeAttribute29", (obj, val) => obj.TradeAttribute29 = val },
                { "TradeAttribute30", (obj, val) => obj.TradeAttribute30 = val },
                { "TradeAttribute31", (obj, val) => obj.TradeAttribute31 = val },
                { "TradeAttribute32", (obj, val) => obj.TradeAttribute32 = val },
                { "TradeAttribute33", (obj, val) => obj.TradeAttribute33 = val },
                { "TradeAttribute34", (obj, val) => obj.TradeAttribute34 = val },
                { "TradeAttribute35", (obj, val) => obj.TradeAttribute35 = val },
                { "TradeAttribute36", (obj, val) => obj.TradeAttribute36 = val },
                { "TradeAttribute37", (obj, val) => obj.TradeAttribute37 = val },
                { "TradeAttribute38", (obj, val) => obj.TradeAttribute38 = val },
                { "TradeAttribute39", (obj, val) => obj.TradeAttribute39 = val },
                { "TradeAttribute40", (obj, val) => obj.TradeAttribute40 = val },
                { "TradeAttribute41", (obj, val) => obj.TradeAttribute41 = val },
                { "TradeAttribute42", (obj, val) => obj.TradeAttribute42 = val },
                { "TradeAttribute43", (obj, val) => obj.TradeAttribute43 = val },
                { "TradeAttribute44", (obj, val) => obj.TradeAttribute44 = val },
                { "TradeAttribute45", (obj, val) => obj.TradeAttribute45 = val },
            };

        private static readonly Dictionary<string, Func<Position, string>> _attributeGetters =
            new Dictionary<string, Func<Position, string>>
            {
                { "TradeAttribute7",  (obj) => obj.TradeAttribute7 },
                { "TradeAttribute8",  (obj) => obj.TradeAttribute8 },
                { "TradeAttribute9",  (obj) => obj.TradeAttribute9 },
                { "TradeAttribute10", (obj) => obj.TradeAttribute10 },
                { "TradeAttribute11", (obj) => obj.TradeAttribute11 },
                { "TradeAttribute12", (obj) => obj.TradeAttribute12 },
                { "TradeAttribute13", (obj) => obj.TradeAttribute13 },
                { "TradeAttribute14", (obj) => obj.TradeAttribute14 },
                { "TradeAttribute15", (obj) => obj.TradeAttribute15 },
                { "TradeAttribute16", (obj) => obj.TradeAttribute16 },
                { "TradeAttribute17", (obj) => obj.TradeAttribute17 },
                { "TradeAttribute18", (obj) => obj.TradeAttribute18 },
                { "TradeAttribute19", (obj) => obj.TradeAttribute19 },
                { "TradeAttribute20", (obj) => obj.TradeAttribute20 },
                { "TradeAttribute21", (obj) => obj.TradeAttribute21 },
                { "TradeAttribute22", (obj) => obj.TradeAttribute22 },
                { "TradeAttribute23", (obj) => obj.TradeAttribute23 },
                { "TradeAttribute24", (obj) => obj.TradeAttribute24 },
                { "TradeAttribute25", (obj) => obj.TradeAttribute25 },
                { "TradeAttribute26", (obj) => obj.TradeAttribute26 },
                { "TradeAttribute27", (obj) => obj.TradeAttribute27 },
                { "TradeAttribute28", (obj) => obj.TradeAttribute28 },
                { "TradeAttribute29", (obj) => obj.TradeAttribute29 },
                { "TradeAttribute30", (obj) => obj.TradeAttribute30 },
                { "TradeAttribute31", (obj) => obj.TradeAttribute31 },
                { "TradeAttribute32", (obj) => obj.TradeAttribute32 },
                { "TradeAttribute33", (obj) => obj.TradeAttribute33 },
                { "TradeAttribute34", (obj) => obj.TradeAttribute34 },
                { "TradeAttribute35", (obj) => obj.TradeAttribute35 },
                { "TradeAttribute36", (obj) => obj.TradeAttribute36 },
                { "TradeAttribute37", (obj) => obj.TradeAttribute37 },
                { "TradeAttribute38", (obj) => obj.TradeAttribute38 },
                { "TradeAttribute39", (obj) => obj.TradeAttribute39 },
                { "TradeAttribute40", (obj) => obj.TradeAttribute40 },
                { "TradeAttribute41", (obj) => obj.TradeAttribute41 },
                { "TradeAttribute42", (obj) => obj.TradeAttribute42 },
                { "TradeAttribute43", (obj) => obj.TradeAttribute43 },
                { "TradeAttribute44", (obj) => obj.TradeAttribute44 },
                { "TradeAttribute45", (obj) => obj.TradeAttribute45 },
            };

        /// <summary>
        /// Sets trade attribute values from a JSON string.
        /// </summary>
        public virtual void SetTradeAttribute(string json)
        {
            try
            {
                if (!string.IsNullOrEmpty(json) && IsValidJSON(json, out JArray array))
                {
                    foreach (JObject obj in array)
                    {
                        string name = (string)obj["Name"];
                        string value = (string)obj["Value"];
                        if (_attributeSetters.TryGetValue(name, out var setter))
                        {
                            setter(this, value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Sets trade attribute values from a dictionary.
        /// </summary>
        public virtual void SetTradeAttribute(Dictionary<string, string> attributeValues)
        {
            try
            {
                foreach (var attributePair in attributeValues)
                {
                    if (_attributeSetters.TryGetValue(attributePair.Key, out var setter))
                    {
                        setter(this, attributePair.Value ?? string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// This method checks the correctness of string being in JSON format
        /// </summary>
        /// <param name="json"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool IsValidJSON(string json, out JArray result)
        {
            result = null;
            try
            {
                result = JArray.Parse(json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region IKeyable Members

        public string GetKey()
        {
            return this.TaxLotClosingId;
        }

        public void Update(IKeyable item)
        {
            TaxLot taxlot = (TaxLot)item;
            if (_ID.Equals(taxlot.TaxLotID))
            {
                _openAveragePrice = taxlot.AvgPrice;
                if (taxlot.TaxLotQty != 0)
                {
                    this.PositionTotalCommissionandFees = (taxlot.TotalCommissionandFees * (this.ClosedQty / taxlot.TaxLotQty));
                }
            }
            else
            {
                _closedAveragePrice = taxlot.AvgPrice;
                if (taxlot.TaxLotQty != 0)
                {
                    this.ClosingTotalCommissionandFees = (taxlot.TotalCommissionandFees * (this.ClosedQty / taxlot.TaxLotQty));
                }
            }
        }
        #endregion

        #region IFilterable Members

        public DateTime GetDate()
        {
            return this.ClosingTradeDate;
        }

        public DateTime GetDateModified()
        {
            return this.ClosingTradeDate;
        }

        public virtual string GetSymbol()
        {
            return this.Symbol;
        }
        public virtual int GetAccountID()
        {
            return this.AccountValue.ID;
        }
        #endregion

        #region INotifyPropertyChangedCustom Members

        [field: NonSerialized]
        new public virtual event PropertyChangedEventHandler PropertyChanged;

        new public virtual void PropertyHasChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, null);
            }
        }
        #endregion

        #region Validation Rules
        protected override void AddBusinessRules()
        {
            //Narendra Kumar Jangir, Aug 12, 2013
            //http://jira.nirvanasolutions.com:8080/browse/KIN-66
            //Key already exists error on closing UI

            //ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_Symbol);
            //ValidationRules.AddRule(CommonRules.MinValue<long>, new CommonRules.MinValueRuleArgs<long>(CONST_PositionStartQuantity, CONST_ZERO));
            //ValidationRules.AddRule(CommonRules.MinValue<double>, new CommonRules.MinValueRuleArgs<double>(CONST_AveragePrice, CONST_AVG_PRICE_ZERO));
            //ValidationRules.AddRule(CommonRules.MinValue<int>, new CommonRules.MinValueRuleArgs<int>(CONST_AUECID, CONST_ZERO));

            //ValidationRules.AddRule(AddNewPositionRules.AUECRequired, CONST_AUECID);
            //ValidationRules.AddRule(AddNewPositionRules.LowValue_HighValue_MatchCheck, CONST_HighValue);

            //ValidationRules.AddRule(CustomClass.CashSettledPriceCheck, CONST_CASHSETTLEDPRICE);
        }

        #endregion

        [System.Runtime.InteropServices.ComVisible(false)]
        public class AddNewPositionRules : RuleArgs
        {
            public AddNewPositionRules(string validation)
                : base(validation)
            {
            }
            public static bool AUECRequired(object target, RuleArgs e)
            {
                Position finalTarget = target as Position;
                if (finalTarget != null)
                {
                    if (finalTarget.AUECID <= 0)
                    {
                        e.Description = "AUEC required";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        [System.Runtime.InteropServices.ComVisible(false)]
        public class CustomClass : RuleArgs
        {
            public CustomClass(string validation)
                : base(validation)
            {
            }
        }
    }
}
