using Csla;
using Csla.Validation;
//using Prana.Utilities.DateTimeUtilities;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    [Serializable, System.Runtime.InteropServices.ComVisible(false)]
    [System.Xml.Serialization.XmlInclude(typeof(OTCPosition))]
    public class OTCPosition : BusinessBase<OTCPosition>
    {
        //
        public OTCPosition()
        {
            MarkAsChild();
        }

        public OTCPosition(int assetID, int underLyingID, int exchangeID, int currencyID, string symbol, int vsCurrencyID, long positionStartQuantity, double averagePrice)
        {
            MarkAsChild();
            //_assetID = assetID;
            this.AssetID = assetID;
            this.UnderlyingID = underLyingID;
            this.ExchangeID = exchangeID;
            this.CurrencyID = currencyID;
            this.Symbol = symbol;
            this.PositionStartQuantity = positionStartQuantity;
            this.AveragePrice = averagePrice;
            this.TransactionSource = TransactionSource.CreateTransactionUI;
        }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        [System.Xml.Serialization.XmlIgnore()]
        public DateTime StartDate
        {
            get
            {
                if (_startDate == null || _startDate == DateTimeConstants.MinValue)
                {
                    _startDate = DateTime.UtcNow;
                }
                return _startDate;

            }
            set
            {
                _startDate = value;
                PropertyHasChanged(ApplicationConstants.CONST_STARTDATE);
                PropertyHasChanged(ApplicationConstants.CONST_SETTLEMENTDATE);
                PropertyHasChanged(ApplicationConstants.CONST_EXPIRATIONDATE);
                PropertyHasChanged(ApplicationConstants.CONST_PROCESSDATE);

            }
        }

        private DateTime _settlementDate = DateTimeConstants.MinValue;
        /// <summary>
        /// Gets or sets the settlement date.
        /// </summary>
        /// <value>The settlement date.</value>
        [System.Xml.Serialization.XmlIgnore()]
        public DateTime SettlementDate
        {
            get
            {
                if (_settlementDate == null || _settlementDate == DateTimeConstants.MinValue)
                {
                    _settlementDate = DateTime.UtcNow;
                }
                return _settlementDate;

            }
            set
            {
                _settlementDate = value;
                PropertyHasChanged(ApplicationConstants.CONST_SETTLEMENTDATE);
                PropertyHasChanged(ApplicationConstants.CONST_EXPIRATIONDATE);
            }
        }


        private DateTime _expirationDate = DateTimeConstants.MinValue;
        /// <summary>
        /// Gets or sets the settlement date.
        /// </summary>
        /// <value>The settlement date.</value>
        [System.Xml.Serialization.XmlIgnore()]
        public DateTime ExpirationDate
        {
            get
            {
                if (_expirationDate == null || _expirationDate == DateTimeConstants.MinValue)
                {
                    _expirationDate = DateTime.UtcNow;
                }
                return _expirationDate;

            }
            set
            {
                _expirationDate = value;
                PropertyHasChanged(ApplicationConstants.CONST_EXPIRATIONDATE);
            }
        }

        private DateTime _processDate = DateTimeConstants.MinValue;
        [System.Xml.Serialization.XmlIgnore()]
        public DateTime ProcessDate
        {
            get
            {
                if (_processDate == null || _processDate == DateTimeConstants.MinValue)
                {
                    _processDate = _startDate;
                }
                return _processDate;
            }
            set
            {
                _processDate = value;
                PropertyHasChanged(ApplicationConstants.CONST_PROCESSDATE);
                PropertyHasChanged(ApplicationConstants.CONST_ORIGINALPURCHASEDATE);
            }
        }

        private DateTime _originalPurchaseDate = DateTimeConstants.MinValue;
        [System.Xml.Serialization.XmlIgnore()]
        public DateTime OriginalPurchaseDate
        {
            get
            {
                if (_originalPurchaseDate == null || _originalPurchaseDate == DateTimeConstants.MinValue)
                {
                    _originalPurchaseDate = _processDate;
                }
                return _originalPurchaseDate;
            }
            set
            {
                _originalPurchaseDate = value;
                PropertyHasChanged(ApplicationConstants.CONST_ORIGINALPURCHASEDATE);
            }
        }

        private bool _isOptionActivated = false;
        public bool IsOptionActivated
        {
            get
            {
                return _isOptionActivated;
            }
            set
            {
                _isOptionActivated = value;
            }
        }

        private OptionType _optionType;
        public OptionType OptionType
        {
            get { return _optionType; }
            set { _optionType = value; }
        }

        private bool _isUnderlyingValidated = false;
        public bool IsUnderlyingValidated
        {
            get
            {
                return _isUnderlyingValidated;
            }
            set
            {
                _isUnderlyingValidated = value;
            }
        }

        private AssetCategory _underlyingAssetCategory;
        public AssetCategory UnderlyingAssetCategory
        {
            get
            {
                return _underlyingAssetCategory;
            }
            set
            {
                _underlyingAssetCategory = value;
            }
        }

        private int _underlyingAUECID;
        public int UnderlyingAUECID
        {
            get { return _underlyingAUECID; }
            set { _underlyingAUECID = value; }
        }

        private double _strikePriceMultiplier;
        public double StrikePriceMultiplier
        {
            get { return _strikePriceMultiplier; }
            set { _strikePriceMultiplier = value; }
        }

        private string _esignalOptionRoot = string.Empty;
        public string EsignalOptionRoot
        {
            get { return _esignalOptionRoot; }
            set { _esignalOptionRoot = value; }
        }

        private string _bloombergOptionRoot = string.Empty;
        public string BloombergOptionRoot
        {
            get { return _bloombergOptionRoot; }
            set { _bloombergOptionRoot = value; }
        }

        private int _counterPartyID;
        public int CounterPartyID
        {
            get { return _counterPartyID; }
            set { _counterPartyID = value; }
        }

        private int _venueID;
        public int VenueID
        {
            get { return _venueID; }
            set { _venueID = value; }
        }

        private int _assetID;
        public int AssetID
        {
            get { return _assetID; }
            set
            {
                _assetID = value;
                PropertyHasChanged(ApplicationConstants.CONST_ASSETID);
                PropertyHasChanged(ApplicationConstants.CONST_SYMBOL);
                PropertyHasChanged(ApplicationConstants.CONST_VSCURRENCYID);
                PropertyHasChanged(ApplicationConstants.CONST_TRADEDCURRENCYID);
            }
        }

        private int _underlyingID = 0;
        public int UnderlyingID
        {
            get { return _underlyingID; }
            set
            {
                _underlyingID = value;
                PropertyHasChanged(ApplicationConstants.CONST_UNDERLYINGID);
            }
        }

        private int _exchangeID = 0;
        public int ExchangeID
        {
            get { return _exchangeID; }
            set
            {
                _exchangeID = value;
                PropertyHasChanged(ApplicationConstants.CONST_EXCHANGEID);
            }
        }

        private int _currencyID = 0;
        public int CurrencyID
        {
            get { return _currencyID; }
            set
            {
                _currencyID = value;
                PropertyHasChanged(ApplicationConstants.CONST_CURRENCYID);
            }
        }

        private int _settlementCurrencyID;
        public virtual int SettlementCurrencyID
        {
            get { return _settlementCurrencyID; }
            set { _settlementCurrencyID = value; }
        }

        private double _payReceive;
        public double PayReceive
        {
            get { return _payReceive; }
            set { _payReceive = value; }
        }



        private string _sideTagValue = string.Empty;
        /// <summary>
        /// We have created this property as we needed to bind the enum type property to grid combo
        // TODO : Remove this if possible.
        /// </summary>
        public string SideTagValue
        {
            get { return _sideTagValue; }
            set
            {
                _sideTagValue = value;
            }
        }
        private int _call_Put = 0;

        [Browsable(false)]
        public int Call_Put
        {
            get { return _call_Put; }
            set
            {
                _call_Put = value;
            }
        }
        private double _strikePrice = 0;
        public double StrikePrice
        {
            get { return _strikePrice; }
            set
            {
                _strikePrice = value;
            }
        }

        #region Position_Class_Properties

        private Guid _ID;

        /// <summary>
        /// Gets or sets the position ID.
        /// </summary>
        /// <value>The allocation ID.</value>
        public Guid ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        protected override object GetIdValue()
        {
            return _ID;
        }

        private string _startTaxLotID = string.Empty;

        /// <summary>
        /// Gets or sets the start tax lot ID.
        /// </summary>
        /// <value>The start tax lot ID.</value>        
        public string StartTaxLotID
        {
            get { return _startTaxLotID; }
            set { _startTaxLotID = value; }
        }


        private DateTime _modifiedAt;

        /// <summary>
        /// Gets or sets the last activity date.
        /// This is the date when this position was last updated. 
        /// </summary>
        /// <value>The last activity date.</value>
        [Browsable(false)]
        public DateTime ModifiedAt
        {
            get { return _modifiedAt; }
            set { _modifiedAt = value; }
        }

        [System.ComponentModel.Browsable(false)]
        /// Created a new property in the string because UTC includes a timeZone character in the datetime and then it becomes a problem 
        /// to convert it to string (xml recognizes string).
        public string ModifiedAtString
        {
            get { return _modifiedAt.ToString("MMM dd yyyy HH:mm:ss:fff"); }
        }

        private DateTime _startDate = DateTimeConstants.MinValue;
        [System.ComponentModel.Browsable(false)]
        public string StartDateString
        {
            get { return _startDate.ToString("MMM dd yyyy HH:mm:ss:fff"); }
        }

        private DateTime _endDate = DateTimeConstants.MinValue;
        /// <summary>
        /// Gets or sets the end Date
        /// </summary>
        /// <value>The start date.</value>        
        public virtual DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }


        private string _description;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private int _modifiedBy;

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        /// <value>The modified by.</value>
        [Browsable(false)]
        public int ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        private string _symbol;

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>The symbol.</value>
        public string Symbol
        {
            get { return _symbol; }
            set
            {
                //if (value == null)
                //{
                //    value = string.Empty;
                //}
                _symbol = value;
                PropertyHasChanged(ApplicationConstants.CONST_SYMBOL);
            }
        }

        private int _accountID;
        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        private TransactionSource _transactionSource;
        //[Browsable(false)]
        public TransactionSource TransactionSource
        {
            get { return _transactionSource; }
            set { _transactionSource = value; }
        }

        private string _transactionType = string.Empty;
        public string TransactionType
        {
            get { return _transactionType; }
            set { _transactionType = value; }
        }

        private Prana.BusinessObjects.PositionManagement.Account _account;

        /// <summary>
        /// Gets or sets the account value.
        /// </summary>
        /// <value>The account value.</value>
        public Prana.BusinessObjects.PositionManagement.Account AccountValue
        {
            get
            {
                if (_account == null)
                {
                    _account = new Prana.BusinessObjects.PositionManagement.Account();
                }
                return _account;
            }
            set { _account = value; }
        }

        private double _averagePrice = 0.0;

        /// <summary>
        /// Gets or sets the average price.
        /// </summary>
        /// <value>The average price.</value>
        public double AveragePrice
        {
            get { return _averagePrice; }
            set
            {
                _averagePrice = value;
                PropertyHasChanged(ApplicationConstants.CONST_AVERAGEPRICE);
            }
        }

        private double _markPriceForMonth = 0.0;

        /// <summary>
        /// Gets or sets the mark price for month.
        /// </summary>
        /// <value>The mark price for month.</value>
        public double MarkPriceForMonth
        {
            get { return _markPriceForMonth; }
            set { _markPriceForMonth = value; }
        }

        private double _monthToDateRealizedProfit = 0.0;

        /// <summary>
        /// Gets or sets the month to date realized profit.
        /// </summary>
        /// <value>The month to date realized profit.</value>
        public double MonthToDateRealizedProfit
        {
            get { return _monthToDateRealizedProfit; }
            set { _monthToDateRealizedProfit = value; }
        }

        private double _symbolAveragePrice = 0.0;

        /// <summary>
        /// Gets or sets the mark price for month.
        /// </summary>
        /// <value>The mark price for month.</value>
        public double SymbolAveragePrice
        {
            get { return _symbolAveragePrice; }
            set { _symbolAveragePrice = value; }
        }

        private double _avgPriceRealizedPL = 0.0;

        /// <summary>
        /// Gets or sets the month to date realized profit.
        /// </summary>
        /// <value>The month to date realized profit.</value>
        public double AvgPriceRealizedPL
        {
            get { return _avgPriceRealizedPL; }
            set { _avgPriceRealizedPL = value; }
        }
        private DateTime _aUECLocalDate = DateTimeConstants.MinValue;
        [Browsable(false)]
        public DateTime AUECLocalDate
        {
            get { return _aUECLocalDate; }
            set { _aUECLocalDate = value; }

        }

        [System.ComponentModel.Browsable(false)]
        public string AUECLocalDateString
        {
            get { return _aUECLocalDate.ToString("MMM dd yyyy HH:mm:ss:fff"); }
        }

        private bool _IsManuallyCreatedPosition = false;

        [Browsable(false)]
        public bool IsManuallyCreatedPosition
        {
            get { return _IsManuallyCreatedPosition; }
            set { _IsManuallyCreatedPosition = value; }
        }
        private PositionType _positionType;

        /// <summary>
        /// Gets or sets the type of the position.
        /// </summary>
        /// <value>The type of the position.</value>
        public PositionType PositionType
        {
            get { return _positionType; }
            set
            {
                _positionType = value;
                _intPositionType = Convert.ToInt32(_positionType);
            }
        }


        private int _intPositionType;
        /// <summary>
        /// We have created this property as we needed to bind the enum type property to grid combo
        // TODO : Remove this if possible.
        /// </summary>
        public int IntPositionType
        {
            get { return _intPositionType; }
            set
            {
                _intPositionType = value;
                _positionType = (PositionType)value;
            }
        }


        private string _side;

        /// <summary>
        /// Gets or sets the side.
        /// </summary>
        /// <value>The side.</value>
        // [Browsable(false)]
        public string Side
        {
            get { return _side; }
            set { _side = value; }
        }

        private int _auecID;

        //[Browsable(false)]
        public int AUECID
        {
            get { return _auecID; }
            set
            {
                _auecID = value;
                PropertyHasChanged(ApplicationConstants.CONST_AUECID);
            }
        }

        private double _notionalValue = 0;
        private double _openQty = 0;
        /// <summary>
        /// Gets or sets the open qty.
        /// </summary>
        /// <value>The open qty.</value>
        public double OpenQty
        {
            get { return _openQty; }
            set { _openQty = value; }
        }
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
        //[Browsable(false)]
        public double Multiplier
        {
            get { return _multiplier; }
            set
            {
                _multiplier = value;
                PropertyHasChanged(ApplicationConstants.CONST_Multiplier);
            }
        }

        // private long _closedQty;

        /// <summary>
        /// Gets or sets my property.
        /// </summary>
        /// <value>My property.</value>
        public double ClosedQty
        {
            get
            {
                return _positionStartQuantity - _openQty;
            }
        }

        private PostionStatus _status;

        public PostionStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private double _positionStartQuantity = 0.0;

        /// <summary>
        /// Gets or sets the position start quantity.
        /// </summary>
        /// <value>The position start quantity.</value>

        public double PositionStartQuantity
        {
            get { return _positionStartQuantity; }
            set
            {
                _positionStartQuantity = value;
                PropertyHasChanged(ApplicationConstants.CONST_POSITIONSTARTQUANTITY);
            }
        }

        private bool _isActive = true;
        [Browsable(false)]
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }


        private double _costBasisRealizedPNL = 0.0;
        [System.Xml.Serialization.XmlIgnore()]
        public double CostBasisRealizedPNL
        {
            get { return _costBasisRealizedPNL; }
            set { _costBasisRealizedPNL = value; }
        }

        private double _commision = 0.0;

        /// <summary>
        /// Gets or sets the commission.
        /// </summary>
        /// <value>The commission.</value>
        public double Commission
        {
            get { return _commision; }
            set { _commision = value; }
        }

        private double _softCommision = 0.0;

        /// <summary>
        /// Gets or sets the soft commission.
        /// </summary>
        /// <value>The soft commission.</value>
        public double SoftCommission
        {
            get { return _softCommision; }
            set { _softCommision = value; }
        }

        private double _fees = 0.0;

        /// <summary>
        /// Gets or sets the fees.
        /// </summary>
        /// <value>The fees.</value>
        public double Fees
        {
            get { return _fees; }
            set { _fees = value; }
        }


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

        private string _recordType;

        /// <summary>
        /// Set this field for Portfolio Records, for closing history , 
        /// populate with Opening/Closing
        /// </summary>
        /// <value>The type of the record(Opening/Closing).</value>
        public string RecordType
        {
            get { return _recordType; }
            set { _recordType = value; }
        }

        private int _strategyID;
        public int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }

        private String _strategy;

        public String Strategy
        {
            get { return _strategy; }
            set { _strategy = value; }
        }

        private string _forexConversion;
        [Browsable(false)]
        /// <summary>
        /// Set this field for Forex conversion.
        /// </summary>
        /// <value>The type of the record(Opening/Closing).</value>
        public string ForexConversion
        {
            get { return _forexConversion; }
            set { _forexConversion = value; }
        }

        //private AllocatedTradesList _positionTaxLots = new AllocatedTradesList();

        ///// <summary>
        ///// Gets or sets the taxlots which create the position
        ///// Child taxlots which creates this position
        ///// </summary>
        ///// <value>The position allocated trades.</value>
        //[Browsable(false)]
        //public AllocatedTradesList PositionTaxLots
        //{
        //    get { return _positionTaxLots; }
        //    set { _positionTaxLots = value; }
        //}

        private DateTime _auecLocalDateToday = DateTimeConstants.MinValue;
        /// <summary>
        /// It contains the auec local date for today
        /// </summary>
        //[Browsable(false)]
        public DateTime AUECLocalDateToday
        {
            get { return _auecLocalDateToday; }
            set { _auecLocalDateToday = value; }
        }

        private string _expiredTaxlotID = string.Empty;
        /// <summary>
        /// expired taxlotID aginst which position has generated
        /// </summary>

        public string ExpiredTaxlotID
        {
            get { return _expiredTaxlotID; }
            set { _expiredTaxlotID = value; }
        }
        private double _expiredQty = 0;
        /// <summary>
        /// expired Qty aginst which position has generated
        /// </summary>

        public double ExpiredQty
        {
            get { return _expiredQty; }
            set { _expiredQty = value; }
        }
        #endregion


        private double _stampDuty;
        /// <summary>
        /// Gets or sets the StampDuty.
        /// </summary>
        /// <value>The StampDuty.</value>
        public double StampDuty
        {
            get { return _stampDuty; }
            set
            {
                _stampDuty = value;
            }
        }
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

        private double _secFee;
        /// <summary>
        /// Gets or sets the SecFee.
        /// </summary>
        /// <value>The SecFee.</value>
        public double SecFee
        {
            get { return _secFee; }
            set
            {
                _secFee = value;
            }
        }

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


        private Prana.BusinessObjects.AppConstants.Operator _fxConversionMethodOperator = Prana.BusinessObjects.AppConstants.Operator.M;

        public Prana.BusinessObjects.AppConstants.Operator FXConversionMethodOperator
        {
            get { return _fxConversionMethodOperator; }
            set
            {
                _fxConversionMethodOperator = value;
            }
        }

        private CommisionSource _commissionSource = CommisionSource.Auto;
        public CommisionSource CommissionSource
        {
            get { return _commissionSource; }
            set { _commissionSource = value; }
        }

        private CommisionSource _softCommissionSource = CommisionSource.Auto;
        public CommisionSource SoftCommissionSource
        {
            get { return _softCommissionSource; }
            set { _softCommissionSource = value; }
        }

        private string _ifPayReceiveChanges = Prana.BusinessObjects.AppConstants.PayReceiveChanges.AdjustAvgPrice.ToString();
        public string IfPayReceiveChanges
        {
            get { return _ifPayReceiveChanges; }
            set { _ifPayReceiveChanges = value; }
        }

        private double _avgFXRate = 0.0;

        public double FXRate
        {
            get { return _avgFXRate; }
            set { _avgFXRate = value; }
        }

        private string _requestedSymbol = string.Empty;
        [Browsable(false)]
        public string RequestedSymbol
        {
            get { return _requestedSymbol; }
            set { _requestedSymbol = value; }
        }

        private float _delta;
        public float Delta
        {
            get { return _delta; }
            set { _delta = value; }
        }

        private int _leadCurrencyID;

        public int LeadCurrencyID
        {
            get { return _leadCurrencyID; }
            set { _leadCurrencyID = value; }
        }


        private int _vsCurrencyID;
        [Browsable(false)]
        public int VsCurrencyID
        {
            get { return _vsCurrencyID; }
            set { _vsCurrencyID = value; }
        }

        private string _tradeAttribute1 = string.Empty;
       
        public string TradeAttribute1
        {
            get { return _tradeAttribute1; }
            set { _tradeAttribute1 = value; }
        }

        private string _tradeAttribute2 = string.Empty;
        
        public string TradeAttribute2
        {
            get { return _tradeAttribute2; }
            set { _tradeAttribute2 = value; }
        }

        private string _tradeAttribute3 = string.Empty;
   
        public string TradeAttribute3
        {
            get { return _tradeAttribute3; }
            set { _tradeAttribute3 = value; }
        }

        private string _tradeAttribute4 = string.Empty;
       
        public string TradeAttribute4
        {
            get { return _tradeAttribute4; }
            set { _tradeAttribute4 = value; }
        }

        private string _tradeAttribute5 = string.Empty;
    
        public string TradeAttribute5
        {
            get { return _tradeAttribute5; }
            set { _tradeAttribute5 = value; }
        }

        private string _tradeAttribute6 = string.Empty;

        public string TradeAttribute6
        {
            get { return _tradeAttribute6; }
            set { _tradeAttribute6 = value; }
        }

        private string _taxLotClosingId = string.Empty;

        public string TaxLotClosingId
        {
            get { return _taxLotClosingId; }
            set { _taxLotClosingId = value; }
        }
        private string _underlyingSymbol;
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }

            set { _underlyingSymbol = value; }
        }

        private double _accruedInterest;

        public double AccruedInterest
        {
            get { return _accruedInterest; }
            set { _accruedInterest = value; }
        }

        private double _coupon;
        [Browsable(false)]
        public double Coupon
        {
            get { return _coupon; }
            set { _coupon = value; }
        }

        private DateTime _dateIssue = DateTimeConstants.MinValue;
        [Browsable(false)]
        public DateTime IssueDate
        {
            get { return _dateIssue; }
            set { _dateIssue = value; }
        }

        private DateTime _firstCouponDate = DateTimeConstants.MinValue;
        [Browsable(false)]
        public DateTime FirstCouponDate
        {
            get { return _firstCouponDate; }
            set { _firstCouponDate = value; }
        }
        private DateTime _dateMaturity = DateTimeConstants.MinValue;
        [Browsable(false)]
        public DateTime MaturityDate
        {
            get { return _dateMaturity; }
            set { _dateMaturity = value; }
        }
        private AccrualBasis _accrualBasis;
        [Browsable(false)]
        public AccrualBasis AccrualBasis
        {
            get { return _accrualBasis; }
            set { _accrualBasis = value; }
        }
        private SecurityType _bondType;
        [Browsable(false)]
        public SecurityType BondType
        {
            get { return _bondType; }
            set { _bondType = value; }
        }
        private CouponFrequency _freq;
        [Browsable(false)]
        public CouponFrequency Freq
        {
            get { return _freq; }
            set { _freq = value; }
        }
        private bool _isZero;
        [Browsable(false)]
        public bool IsZero
        {
            get { return _isZero; }
            set { _isZero = value; }
        }

        private bool _isExerciseAssignManual = false;
        [Browsable(false)]
        public bool IsExerciseAssignManual
        {
            get { return _isExerciseAssignManual; }
            set { _isExerciseAssignManual = value; }
        }
        private int _daysToSettlement;
        [Browsable(false)]
        public int DaysToSettlement
        {
            get { return _daysToSettlement; }
            set { _daysToSettlement = value; }
        }

        #region Validation Rules

        protected override void AddBusinessRules()
        {
            //ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), ApplicationConstants.CONST_SYMBOL);
            //ValidationRules.AddRule(CommonRules.StringRequired, ApplicationConstants.CONST_SYMBOL);
            ValidationRules.AddRule(BusinessRules.SymbolCheck, ApplicationConstants.CONST_SYMBOL);
            ValidationRules.AddRule(BusinessRules.AssetCheck, ApplicationConstants.CONST_ASSETID);
            ValidationRules.AddRule(BusinessRules.UnderLyingCheck, ApplicationConstants.CONST_UNDERLYINGID);
            ValidationRules.AddRule(BusinessRules.ExchangeCheck, ApplicationConstants.CONST_EXCHANGEID);
            ValidationRules.AddRule(BusinessRules.CurrencyCheck, ApplicationConstants.CONST_CURRENCYID);
            ValidationRules.AddRule(BusinessRules.QuantityCheck, ApplicationConstants.CONST_POSITIONSTARTQUANTITY);
            //ValidationRules.AddRule(BusinessRules.AveragePriceCheck, ApplicationConstants.CONST_AVERAGEPRICE);
            ValidationRules.AddRule(BusinessRules.SettlementDateCheck, ApplicationConstants.CONST_SETTLEMENTDATE);
            ValidationRules.AddRule(BusinessRules.SettlementDateToTradeDateCheck, ApplicationConstants.CONST_SETTLEMENTDATE);
            ValidationRules.AddRule(BusinessRules.StartDateCheck, ApplicationConstants.CONST_STARTDATE);
            ValidationRules.AddRule(BusinessRules.ProcessDateCheck, ApplicationConstants.CONST_PROCESSDATE);
            ValidationRules.AddRule(BusinessRules.OriginalPurchaseDateCheck, ApplicationConstants.CONST_ORIGINALPURCHASEDATE);
        }

        //public void ST()
        //{
        //    ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), ApplicationConstants.CONST_Symbol);
        //    PropertyHasChanged(ApplicationConstants.CONST_Symbol);
        //}

        [System.Runtime.InteropServices.ComVisible(false)]
        public class BusinessRules : RuleArgs
        {
            public BusinessRules(string validation)
                : base(validation)
            {
            }

            public static bool SymbolCheck(object target, RuleArgs e)
            {
                OTCPosition finalTarget = target as OTCPosition;
                if (finalTarget != null)
                {
                    if (finalTarget._symbol != null)
                    {
                        if (!finalTarget._assetID.Equals(5) && finalTarget._symbol.Equals(string.Empty))
                        {
                            e.Description = "Symbol required";
                            return false;
                        }
                        else
                        {
                            return true;
                        }
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

            public static bool AssetCheck(object target, RuleArgs e)
            {
                OTCPosition finalTarget = target as OTCPosition;
                if (finalTarget != null)
                {
                    if (finalTarget._assetID.Equals(ApplicationConstants.CONST_ZERO))
                    {
                        e.Description = "Asset required";
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

            public static bool UnderLyingCheck(object target, RuleArgs e)
            {
                OTCPosition finalTarget = target as OTCPosition;
                if (finalTarget != null)
                {
                    if (finalTarget._underlyingID.Equals(ApplicationConstants.CONST_ZERO))
                    {
                        e.Description = "Underlying required";
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

            public static bool ExchangeCheck(object target, RuleArgs e)
            {
                OTCPosition finalTarget = target as OTCPosition;
                if (finalTarget != null)
                {
                    if (finalTarget._exchangeID.Equals(ApplicationConstants.CONST_ZERO))
                    {
                        e.Description = "Exchange required";
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

            public static bool CurrencyCheck(object target, RuleArgs e)
            {
                OTCPosition finalTarget = target as OTCPosition;
                if (finalTarget != null)
                {
                    if (finalTarget._currencyID.Equals(ApplicationConstants.CONST_ZERO))
                    {
                        e.Description = "Currency required";
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

            public static bool QuantityCheck(object target, RuleArgs e)
            {
                OTCPosition finalTarget = target as OTCPosition;
                if (finalTarget != null)
                {
                    if (finalTarget._positionStartQuantity.Equals(ApplicationConstants.CONST_ZERO))
                    {
                        e.Description = "Quantity required";
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

            public static bool AveragePriceCheck(object target, RuleArgs e)
            {
                OTCPosition finalTarget = target as OTCPosition;
                if (finalTarget != null)
                {
                    if (finalTarget._averagePrice.Equals(ApplicationConstants.CONST_AVG_PRICE_ZERO))
                    {
                        e.Description = "Average price required";
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

            public static bool SettlementDateCheck(object target, RuleArgs e)
            {
                OTCPosition finalTarget = target as OTCPosition;
                if (finalTarget != null)
                {
                    if (finalTarget._settlementDate.Equals(DateTime.MinValue) && finalTarget._assetID.Equals(5))
                    {
                        e.Description = "Settlement date required";
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

            public static bool SettlementDateToTradeDateCheck(object target, RuleArgs e)
            {
                OTCPosition finalTarget = target as OTCPosition;
                if (finalTarget != null)
                {
                    if (finalTarget._settlementDate.Date < finalTarget.StartDate.Date)
                    {
                        e.Description = "Please check the settlement date. It can not be less than trade date";
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

            //public static bool ExpirationDateToSettlementDateCheck(object target, RuleArgs e)
            //{
            //    OTCPosition finalTarget = target as OTCPosition;
            //    if (finalTarget != null)
            //    {
            //        if (finalTarget._expirationDate.Date > finalTarget.SettlementDate.Date)
            //        {
            //            e.Description = "Expiration can not be greater than settlement date";
            //            return false;
            //        }
            //        else
            //        {
            //            return true;
            //        }
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}

            public static bool StartDateCheck(object target, RuleArgs e)
            {
                OTCPosition finalTarget = target as OTCPosition;
                if (finalTarget != null)
                {
                    if (finalTarget.StartDate.Date > finalTarget.AUECLocalDateToday.Date)
                    {
                        e.Description = "Transaction start date can not be greater today's AUEC date.";
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

            public static bool ProcessDateCheck(object target, RuleArgs e)
            {
                OTCPosition finalTarget = target as OTCPosition;
                if (finalTarget != null)
                {
                    if (finalTarget.ProcessDate.Date < finalTarget.StartDate.Date)
                    {
                        e.Description = "Process Date can not be less than Transaction startdate .";
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

            public static bool OriginalPurchaseDateCheck(object target, RuleArgs e)
            {
                OTCPosition finalTarget = target as OTCPosition;
                if (finalTarget != null)
                {
                    if (finalTarget.OriginalPurchaseDate.Date > finalTarget.ProcessDate.Date)
                    {
                        e.Description = "Original purchase date can not be greater than Process Date.";
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

            public static bool ExpirationDateToTradeDateCheck(object target, RuleArgs e)
            {
                OTCPosition finalTarget = target as OTCPosition;
                if (finalTarget != null)
                {
                    if ((finalTarget._assetID != (int)AssetCategory.Equity || finalTarget._assetID != (int)AssetCategory.PrivateEquity || finalTarget._assetID != (int)AssetCategory.CreditDefaultSwap) && finalTarget._expirationDate.Date < finalTarget.SettlementDate.Date)
                    {
                        e.Description = "Please check the expiration date. It can not be less than settlement date";
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

        #endregion


        public DateTime ParentTradeDate { get; set; }

        private double _optionPremiumAdjustment;
        public double OptionPremiumAdjustment
        {
            get { return _optionPremiumAdjustment; }
            set { _optionPremiumAdjustment = value; }
        }

        private double _optionPremiumAdjustmentUnit;
        [Browsable(false)]
        public double OptionPremiumAdjustmentUnit
        {
            get { return _optionPremiumAdjustmentUnit; }
            set { _optionPremiumAdjustmentUnit = value; }
        }

        //added RoundLot field, PRANA-12674
        private decimal _roundLot = 1;
        [Browsable(false)]
        public virtual decimal RoundLot
        {
            get { return _roundLot; }
            set { _roundLot = value; }
        }

        #region Additional Trade Attributes

        public virtual string TradeAttribute7 { get; set; } = string.Empty;
        public virtual string TradeAttribute8 { get; set; } = string.Empty;
        public virtual string TradeAttribute9 { get; set; } = string.Empty;
        public virtual string TradeAttribute10 { get; set; } = string.Empty;
        public virtual string TradeAttribute11 { get; set; } = string.Empty;
        public virtual string TradeAttribute12 { get; set; } = string.Empty;
        public virtual string TradeAttribute13 { get; set; } = string.Empty;
        public virtual string TradeAttribute14 { get; set; } = string.Empty;
        public virtual string TradeAttribute15 { get; set; } = string.Empty;
        public virtual string TradeAttribute16 { get; set; } = string.Empty;
        public virtual string TradeAttribute17 { get; set; } = string.Empty;
        public virtual string TradeAttribute18 { get; set; } = string.Empty;
        public virtual string TradeAttribute19 { get; set; } = string.Empty;
        public virtual string TradeAttribute20 { get; set; } = string.Empty;
        public virtual string TradeAttribute21 { get; set; } = string.Empty;
        public virtual string TradeAttribute22 { get; set; } = string.Empty;
        public virtual string TradeAttribute23 { get; set; } = string.Empty;
        public virtual string TradeAttribute24 { get; set; } = string.Empty;
        public virtual string TradeAttribute25 { get; set; } = string.Empty;
        public virtual string TradeAttribute26 { get; set; } = string.Empty;
        public virtual string TradeAttribute27 { get; set; } = string.Empty;
        public virtual string TradeAttribute28 { get; set; } = string.Empty;
        public virtual string TradeAttribute29 { get; set; } = string.Empty;
        public virtual string TradeAttribute30 { get; set; } = string.Empty;
        public virtual string TradeAttribute31 { get; set; } = string.Empty;
        public virtual string TradeAttribute32 { get; set; } = string.Empty;
        public virtual string TradeAttribute33 { get; set; } = string.Empty;
        public virtual string TradeAttribute34 { get; set; } = string.Empty;
        public virtual string TradeAttribute35 { get; set; } = string.Empty;
        public virtual string TradeAttribute36 { get; set; } = string.Empty;
        public virtual string TradeAttribute37 { get; set; } = string.Empty;
        public virtual string TradeAttribute38 { get; set; } = string.Empty;
        public virtual string TradeAttribute39 { get; set; } = string.Empty;
        public virtual string TradeAttribute40 { get; set; } = string.Empty;
        public virtual string TradeAttribute41 { get; set; } = string.Empty;
        public virtual string TradeAttribute42 { get; set; } = string.Empty;
        public virtual string TradeAttribute43 { get; set; } = string.Empty;
        public virtual string TradeAttribute44 { get; set; } = string.Empty;
        public virtual string TradeAttribute45 { get; set; } = string.Empty;

        #endregion Additional Trade Attributes

        private static readonly Dictionary<string, Action<OTCPosition, string>> _attributeSetters =
           new Dictionary<string, Action<OTCPosition, string>>
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

        private static readonly Dictionary<string, Func<OTCPosition, string>> _attributeGetters =
            new Dictionary<string, Func<OTCPosition, string>>
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

        #region AdditionalTradeAttributes Utilities
        /// <summary>
        /// Returns all trade attributes as a dictionary.
        /// </summary>
        public virtual Dictionary<string, string> GetTradeAttributesAsDict()
        {
            var attributesArray = new Dictionary<string, string>();
            try
            {
                foreach (var entry in _attributeGetters)
                {
                    attributesArray.Add(entry.Key, entry.Value(this));
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return attributesArray;
        }

        /// <summary>
        /// Gets a single trade attribute value by name.
        /// </summary>
        public virtual string GetTradeAttributeValue(string attributeName)
        {
            try
            {
                return _attributeGetters.TryGetValue(attributeName, out var getter) ? getter(this) : null;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                return string.Empty;
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
        #endregion
    }
}
