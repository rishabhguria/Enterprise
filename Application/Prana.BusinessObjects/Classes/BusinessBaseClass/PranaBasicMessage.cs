using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.BusinessBaseClass;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Prana.BusinessObjects
{
    [KnownType(typeof(AllocationGroup))]
    [Serializable]
    public class PranaBasicMessage : AdditionalTradeAttributes
    {
        protected double _contractMultiplier = 0.0;
        protected string _companyName;

        protected string _orderSideTagValue = string.Empty;
        public virtual string OrderSideTagValue
        {
            get { return _orderSideTagValue; }
            set { _orderSideTagValue = value; }
        }

        protected bool _isStageRequired = false;

        [Browsable(false)]
        public virtual bool IsStageRequired
        {
            get { return _isStageRequired; }
            set { _isStageRequired = value; }
        }

        protected bool _isPricingAvailable;

        [Browsable(false)]
        public virtual bool IsPricingAvailable
        {
            get { return _isPricingAvailable; }
            set { _isPricingAvailable = value; }
        }

        protected bool _isManualOrder = false;

        [Browsable(false)]
        public virtual bool IsManualOrder
        {
            get { return _isManualOrder; }
            set { _isManualOrder = value; }
        }

        protected string _changeComment = string.Empty;
        /// <summary>
        /// Used to save the comment by user, when he changes the trades other than the normal workflow. 
        /// The comments are sent to the T_TradeAudit table later.
        /// </summary>
        [XmlIgnore]
        public virtual string ChangeComment
        {
            get { return _changeComment; }
            set { _changeComment = value; }
        }

        protected int _modifiedUserId = int.MinValue;
        public virtual int ModifiedUserId
        {
            get { return _modifiedUserId; }
            set { _modifiedUserId = value; }
        }

        protected string _orderSide = string.Empty;
        [XmlIgnore]
        public virtual string OrderSide
        {
            get { return _orderSide; }
            set { _orderSide = value; }
        }

        protected string _orderTypeTagValue = string.Empty;
        [Browsable(false)]
        public virtual string OrderTypeTagValue
        {
            get { return _orderTypeTagValue; }
            set { _orderTypeTagValue = value; }
        }

        protected string _orderType = string.Empty;
        [XmlIgnore]
        public virtual string OrderType
        {
            get { return _orderType; }
            set { _orderType = value; }
        }

        protected string _symbol = string.Empty;
        public virtual string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        protected double _quantity = 0;
        public virtual double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        protected int _originalAllocationPreferenceID = 0;
        [Browsable(false)]
        public virtual int OriginalAllocationPreferenceID
        {
            get { return _originalAllocationPreferenceID; }
            set { _originalAllocationPreferenceID = value; }
        }

        protected double _avgPrice = 0;
        public virtual double AvgPrice
        {
            get
            {
                // We were getting NaN in AvgPrice field in some case, to pretend that we added this check at a common place. PRANA-37180 & PRANA-37152
                return Double.IsNaN(_avgPrice) ? 0.0 : _avgPrice;
            }
            set
            {
                if (_avgPrice != value)
                {
                    _avgPrice = value;
                }
            }
        }

        public virtual double AvgPriceBase
        {
            get
            {
                double value = 0;
                if (_avgPrice == 0 || _avgFXRateForTrade == 0)
                {
                    value = 0;
                }
                else if (_assetID == (int)AssetCategory.FX || _assetID == (int)AssetCategory.FXForward || _assetID == (int)AssetCategory.Forex)
                {
                    if (VsCurrencyID == 1)
                        value = _avgPrice;
                    else
                        value = 1 / _avgPrice;
                }
                else if (_FXConversionMethodOperator.Equals(Operator.M.ToString()))
                {
                    value = _avgPrice * _avgFXRateForTrade;
                }
                else if (_FXConversionMethodOperator.Equals(Operator.D.ToString()))
                {
                    value = _avgPrice / _avgFXRateForTrade;
                }
                else
                {
                    value = _avgPrice * _avgFXRateForTrade;
                }
                value = Math.Round(value, 14);
                return value;
            }
        }

        protected int _assetID = int.MinValue;
        [Browsable(false)]
        public virtual int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
        }

        protected string _assetName = string.Empty;
        [XmlIgnore]

        public virtual string AssetName
        {
            get { return _assetName; }
            set { _assetName = value; }
        }

        protected int _underlyingID = int.MinValue;
        [Browsable(false)]
        public virtual int UnderlyingID
        {
            get { return _underlyingID; }
            set { _underlyingID = value; }
        }

        protected string _underlyingName = string.Empty;
        [XmlIgnore]
        public virtual string UnderlyingName
        {
            get { return _underlyingName; }
            set { _underlyingName = value; }
        }

        protected int _exchangeID = int.MinValue;
        [Browsable(false)]
        public virtual int ExchangeID
        {
            get { return _exchangeID; }
            set { _exchangeID = value; }
        }

        protected string _exchangeName = string.Empty;
        public virtual string ExchangeName
        {
            get { return _exchangeName; }
            set { _exchangeName = value; }
        }

        protected int _currencyID = int.MinValue;
        public virtual int CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; UpdateSettlementCurrency(); }
        }

        protected string _currencyName = string.Empty;
        [XmlIgnore]
        public virtual string CurrencyName
        {
            get { return _currencyName; }
            set { _currencyName = value; }
        }

        protected int _auecID = int.MinValue;
        public virtual int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        protected int _tradingAccountID = int.MinValue;
        [Browsable(false)]
        public virtual int TradingAccountID
        {
            get { return _tradingAccountID; }
            set { _tradingAccountID = value; }
        }

        protected string _tradingAccountName = string.Empty;
        [XmlIgnore]
        public virtual string TradingAccountName
        {
            get { return _tradingAccountName; }
            set { _tradingAccountName = value; }
        }

        protected int _userID = int.MinValue;
        [Browsable(false)]
        public virtual int CompanyUserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        /// <summary>
        /// The actual user identifier
        /// </summary>
        protected int _actualUserId = int.MinValue;
        /// <summary>
        /// Gets or sets the actual company user identifier.
        /// </summary>
        /// <value>
        /// The actual company user identifier.
        /// </value>
        [Browsable(false)]
        public virtual int ActualCompanyUserID
        {
            get { return _actualUserId; }
            set { _actualUserId = value; }
        }

        protected string _CompanyUserName = string.Empty;
        [XmlIgnore]
        public virtual string CompanyUserName
        {
            get { return _CompanyUserName; }
            set { _CompanyUserName = value; }
        }

        /// <summary>
        /// The actual company user name
        /// </summary>
        protected string _actualCompanyUserName = string.Empty;
        /// <summary>
        /// Gets or sets the actual name of the company user.
        /// </summary>
        /// <value>
        /// The actual name of the company user.
        /// </value>
        [XmlIgnore]
        public virtual string ActualCompanyUserName
        {
            get { return _actualCompanyUserName; }
            set { _actualCompanyUserName = value; }
        }

        protected int _counterPartyID = int.MinValue;
        [Browsable(false)]
        public virtual int CounterPartyID
        {
            get { return _counterPartyID; }
            set { _counterPartyID = value; }
        }

        protected string _counterPartyName = string.Empty;
        [XmlIgnore]
        public virtual string CounterPartyName
        {
            get { return _counterPartyName; }
            set { _counterPartyName = value; }
        }

        protected int _venueID = int.MinValue;
        [Browsable(false)]
        public virtual int VenueID
        {
            get { return _venueID; }
            set { _venueID = value; }
        }

        protected string _venue = string.Empty;
        [XmlIgnore]
        public virtual string Venue
        {
            get { return _venue; }
            set { _venue = value; }
        }

        protected double _cumQty = 0;
        public virtual double CumQty
        {
            get { return _cumQty; }
            set { _cumQty = value; }
        }

        protected double _originalCumQty = 0;
        [Browsable(false)]
        public virtual double OriginalCumQty
        {
            get { return _originalCumQty; }
            set { _originalCumQty = value; }
        }

        protected double _cumQtyForSubOrder = 0;
        [Browsable(false)]
        public virtual double CumQtyForSubOrder
        {
            get { return _cumQtyForSubOrder; }
            set { _cumQtyForSubOrder = value; }
        }

        #region Settlement currency fields
        protected int _settlementCurrencyID;
        public virtual int SettlementCurrencyID
        {
            get { return _settlementCurrencyID; }
            set { _settlementCurrencyID = value; UpdateSettlementCurrency(); }
        }
        #endregion

        protected string _FXConversionMethodOperator = string.Empty;
        public virtual string FXConversionMethodOperator
        {
            get { return _FXConversionMethodOperator; }
            set { _FXConversionMethodOperator = value; }
        }

        public virtual double ContractMultiplier
        {
            get { return _contractMultiplier; }
            set { _contractMultiplier = value; }
        }

        public virtual string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; }
        }

        /// <summary>
        /// Current status of trade/ taxlot
        /// Added by om shiv, Nov, 14, NirvanaWorkFlowsStats enum
        /// </summary>
        protected int _workflowState;
        public virtual int WorkflowState
        {
            get { return _workflowState; }
            set { _workflowState = value; }
        }

        /// <summary>
        /// Date on which trade inserted in prana system
        /// Added by om shiv, sept 14
        /// </summary>
        protected DateTime _nirvanaProcessDate = DateTimeConstants.MinValue;
        public virtual DateTime NirvanaProcessDate
        {
            get { return _nirvanaProcessDate; }
            set { _nirvanaProcessDate = value; }
        }

        protected DateTime _aUECLocalDate = DateTimeConstants.MinValue;
        public virtual DateTime AUECLocalDate
        {
            get { return _aUECLocalDate; }
            set { _aUECLocalDate = value; }
        }

        protected DateTime _processDate = DateTimeConstants.MinValue;
        public virtual DateTime ProcessDate
        {
            get { return _processDate; }
            set { _processDate = value; }
        }

        protected DateTime _originalPurchaseDate = DateTimeConstants.MinValue;
        public virtual DateTime OriginalPurchaseDate
        {
            get { return _originalPurchaseDate; }
            set { _originalPurchaseDate = value; }
        }

        protected DateTime _closingDate = DateTimeConstants.MinValue;
        public virtual DateTime ClosingDate
        {
            get { return _closingDate; }
            set { _closingDate = value; }
        }

        protected DateTime _settlementDate = DateTimeConstants.MinValue;
        public virtual DateTime SettlementDate
        {
            get { return _settlementDate; }
            set { _settlementDate = value; }
        }

        protected DateTime _expirationDate = DateTimeConstants.MinValue;
        public virtual DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }

        }

        protected string _description = string.Empty;
        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        protected string _underLyingsymbol;
        public virtual string UnderlyingSymbol
        {
            get { return _underLyingsymbol; }
            set { _underLyingsymbol = value; }
        }

        private decimal _m2mProfit_Loss;
        public virtual decimal M2MProfitLoss
        {
            get { return _m2mProfit_Loss; }
            set { _m2mProfit_Loss = value; }
        }


        protected double _accruedInterest;
        public virtual double AccruedInterest
        {
            get { return _accruedInterest; }
            set { _accruedInterest = value; }
        }

        private AccrualBasis _accrualBasis;
        [Browsable(false)]
        public virtual AccrualBasis AccrualBasis
        {
            get { return _accrualBasis; }
            set { _accrualBasis = value; }
        }

        private SecurityType _bondType;
        [Browsable(false)]
        public virtual SecurityType BondType
        {
            get { return _bondType; }
            set { _bondType = value; }
        }

        private CouponFrequency _freq;
        [Browsable(false)]
        public virtual CouponFrequency Freq
        {
            get { return _freq; }
            set { _freq = value; }
        }

        private bool _isZero;
        [Browsable(false)]
        public virtual bool IsZero
        {
            get { return _isZero; }
            set { _isZero = value; }
        }

        private DateTime _firstCouponDate = DateTimeConstants.MinValue;
        [Browsable(false)]
        public virtual DateTime FirstCouponDate
        {
            get { return _firstCouponDate; }
            set { _firstCouponDate = value; }
        }

        private DateTime _dateMaturity = DateTimeConstants.MinValue;
        [Browsable(false)]
        public virtual DateTime MaturityDate
        {
            get { return _dateMaturity; }
            set { _dateMaturity = value; }
        }

        private DateTime _dateIssue = DateTimeConstants.MinValue;
        [Browsable(false)]
        public virtual DateTime IssueDate
        {
            get { return _dateIssue; }
            set { _dateIssue = value; }
        }

        protected double _couponRate;
        [Browsable(false)]
        public virtual double CouponRate
        {
            get { return _couponRate; }
            set { _couponRate = value; }
        }

        protected double _delta;
        public virtual double Delta
        {
            get { return _delta; }
            set { _delta = value; }
        }

        protected int _vsCurrencyID;
        public virtual int VsCurrencyID
        {
            get { return _vsCurrencyID; }
            set { _vsCurrencyID = value; }
        }

        protected int _tradedCurrencyID;
        public virtual int LeadCurrencyID
        {
            get { return _tradedCurrencyID; }
            set { _tradedCurrencyID = value; }
        }

        protected double _strikePrice = 0.0;
        public virtual double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }

        protected int _putOrCall = int.MinValue;
        public virtual int PutOrCall
        {
            get { return _putOrCall; }
            set { _putOrCall = value; }
        }

        protected double _avgFXRateForTrade = 0.0;
        public virtual double FXRate
        {
            get { return _avgFXRateForTrade; }
            set { _avgFXRateForTrade = value; }
        }

        protected bool _isModified = false;
        [Browsable(false)]
        public virtual bool IsModified
        {
            get { return _isModified; }
            set { _isModified = value; }
        }

        protected bool _isNDF = false;
        public virtual bool IsNDF
        {
            get { return _isNDF; }
            set { _isNDF = value; }
        }

        protected decimal _roundLot = 1;
        [Browsable(false)]
        public virtual decimal RoundLot
        {
            get { return _roundLot; }
            set { _roundLot = value; }
        }

        protected double _sharesOutstanding = 0;
        [Browsable(false)]
        public virtual double SharesOutstanding
        {
            get { return _sharesOutstanding; }
            set { _sharesOutstanding = value; }
        }

        protected DateTime _fixingDate = DateTimeConstants.MinValue;
        [Browsable(false)]
        public virtual DateTime FixingDate
        {
            get { return _fixingDate; }
            set { _fixingDate = value; }
        }

        protected string _bloombergSymbol = string.Empty;
        public virtual string BloombergSymbol
        {
            get { return _bloombergSymbol; }
            set { _bloombergSymbol = value; }
        }

        protected string _bloombergSymbolWithExchangeCode = string.Empty;
        public virtual string BloombergSymbolWithExchangeCode
        {
            get { return _bloombergSymbolWithExchangeCode; }
            set { _bloombergSymbolWithExchangeCode = value; }
        }

        protected string _factSetSymbol = string.Empty;
        public virtual string FactSetSymbol
        {
            get { return _factSetSymbol; }
            set { _factSetSymbol = value; }
        }

        protected string _activSymbol = string.Empty;
        public virtual string ActivSymbol
        {
            get { return _activSymbol; }
            set { _activSymbol = value; }
        }

        protected string _sedolSymbol = string.Empty;
        public virtual string SEDOLSymbol
        {
            get { return _sedolSymbol; }
            set { _sedolSymbol = value; }
        }

        protected string _cusipSymbol = string.Empty;
        public virtual string CusipSymbol
        {
            get { return _cusipSymbol; }
            set { _cusipSymbol = value; }
        }

        protected string _isinSymbol = string.Empty;
        public virtual string ISINSymbol
        {
            get { return _isinSymbol; }
            set { _isinSymbol = value; }
        }

        protected string _osiSymbol = string.Empty;
        public virtual string OSISymbol
        {
            get { return _osiSymbol; }
            set { _osiSymbol = value; }
        }

        protected string _idcoSymbol = string.Empty;
        public virtual string IDCOSymbol
        {
            get { return _idcoSymbol; }
            set { _idcoSymbol = value; }
        }
        protected double _underlyingDelta = 1.0;
        public virtual double UnderlyingDelta
        {
            get { return _underlyingDelta; }
            set { _underlyingDelta = value; }
        }

        protected double _commissionAmt = 0.0;
        public virtual double CommissionAmt
        {
            get { return _commissionAmt; }
            set { _commissionAmt = value; }
        }

        protected double _softCommissionAmt = 0.0;
        public virtual double SoftCommissionAmt
        {
            get { return _softCommissionAmt; }
            set { _softCommissionAmt = value; }
        }

        protected double _commissionRate = 0.0;
        public virtual double CommissionRate
        {
            get { return _commissionRate; }
            set { _commissionRate = value; }
        }

        protected CalculationBasis _calcBasis = CalculationBasis.Auto;
        public virtual CalculationBasis CalcBasis
        {
            get { return _calcBasis; }
            set { _calcBasis = value; }
        }

        protected double _softCommissionRate = 0.0;
        public virtual double SoftCommissionRate
        {
            get { return _softCommissionRate; }
            set { _softCommissionRate = value; }
        }

        protected CalculationBasis _softCommissionCalcBasis = CalculationBasis.Auto;
        public virtual CalculationBasis SoftCommissionCalcBasis
        {
            get { return _softCommissionCalcBasis; }
            set { _softCommissionCalcBasis = value; }
        }

        public virtual object Clone()
        {
            return Prana.Global.Utilities.DeepCopyHelper.Clone(this);
        }

        //set trade attribute value to default string.Empty in case of null, PRANA-17726
        protected string _tradeAttribute1 = string.Empty;
        public virtual string TradeAttribute1
        {
            get { return _tradeAttribute1; }
            set { _tradeAttribute1 = (value == null) ? string.Empty : value; }
        }

        protected string _tradeAttribute2 = string.Empty;
        public virtual string TradeAttribute2
        {
            get { return _tradeAttribute2; }
            set { _tradeAttribute2 = (value == null) ? string.Empty : value; }
        }

        protected string _tradeAttribute3 = string.Empty;
        public virtual string TradeAttribute3
        {
            get { return _tradeAttribute3; }
            set { _tradeAttribute3 = (value == null) ? string.Empty : value; }
        }


        protected string _tradeAttribute4 = string.Empty;
        public virtual string TradeAttribute4
        {
            get { return _tradeAttribute4; }
            set { _tradeAttribute4 = (value == null) ? string.Empty : value; }
        }

        protected string _tradeAttribute5 = string.Empty;
        public virtual string TradeAttribute5
        {
            get { return _tradeAttribute5; }
            set { _tradeAttribute5 = (value == null) ? string.Empty : value; }
        }

        protected string _tradeAttribute6 = string.Empty;
        public virtual string TradeAttribute6
        {
            get { return _tradeAttribute6; }
            set { _tradeAttribute6 = (value == null) ? string.Empty : value; }
        }

        protected string _internalComments = string.Empty;
        public virtual string InternalComments
        {
            get { return _internalComments; }
            set { _internalComments = value; }
        }

        protected string _proxySymbol = string.Empty;
        public virtual string ProxySymbol
        {
            get { return _proxySymbol; }
            set { _proxySymbol = value; }
        }

        /// <summary>
        /// Narendra Kumar Jangir, Aug 22 2013
        /// This field specifies Transaction type of trade
        /// </summary>
        protected string _transactionType = string.Empty;
        public virtual string TransactionType
        {
            get { return _transactionType; }
            set { _transactionType = value; }
        }

        /// <summary>
        /// Sandeep Singh, January 31, 2014
        /// This field is used in Corporate Action Spin-off when Notional value changes
        /// </summary>
        protected double _notionalChange = 0;
        [Browsable(false)]
        public virtual double NotionalChange
        {
            get { return _notionalChange; }
            set { _notionalChange = value; }
        }

        /// <summary>
        /// Sandeep Singh, Oct 19 2014
        /// This field specifies origin of the trade
        /// </summary>
        protected TransactionSource _transactionSource;
        //[Browsable(false)]
        public virtual TransactionSource TransactionSource
        {
            get { return _transactionSource; }
            set { _transactionSource = value; }
        }

        /// <summary>
        /// Sandeep Singh, Oct 19 2014
        /// This field specifies origin of the trade
        /// </summary>
        protected int _transactionSourceTag;
        [Browsable(false)]
        public virtual int TransactionSourceTag
        {
            get { return _transactionSourceTag; }
            set { _transactionSourceTag = value; }
        }
        protected string _reutersSymbol = string.Empty;
        public virtual string ReutersSymbol
        {
            get { return _reutersSymbol; }
            set { _reutersSymbol = value; }
        }

        /// <summary>
        /// Specifies change type of the trade
        /// </summary>
        protected int _changeType = (int)AppConstants.ChangeType.NoTrade;
        public virtual int ChangeType
        {
            get { return _changeType; }
            set { _changeType = value; }
        }

        protected bool _isCurrencyFuture;
        [Browsable(false)]
        public virtual bool IsCurrencyFuture
        {
            get { return _isCurrencyFuture; }
            set { _isCurrencyFuture = value; }
        }

        private bool _otherChkBox;
        [Browsable(false)]
        public virtual bool OtherChkBox
        {
            get { return _otherChkBox; }
            set { _otherChkBox = value; }
        }

        protected int _nirvanaLocateID;
        public virtual int NirvanaLocateID
        {
            get { return _nirvanaLocateID; }
            set { _nirvanaLocateID = value; }
        }

        protected string _borrowerID;
        public virtual string BorrowerID
        {
            get { return _borrowerID; }
            set { _borrowerID = value; }
        }

        protected double _shortRebate;
        public virtual double ShortRebate
        {
            get { return _shortRebate; }
            set { _shortRebate = value; }
        }

        protected string _borrowerBroker;
        public virtual string BorrowerBroker
        {
            get { return _borrowerBroker; }
            set { _borrowerBroker = value; }
        }

        protected string _executionTimeLastFill;
        public virtual string ExecutionTimeLastFill
        {
            get { return _executionTimeLastFill; }
            set { _executionTimeLastFill = value; }
        }

        protected double _averagePriceForCompliance = 0;
        [Browsable(false)]
        public virtual double AvgPriceForCompliance
        {
            get { return Double.IsNaN(_averagePriceForCompliance) ? 0.0 : _averagePriceForCompliance; }
            set { _averagePriceForCompliance = value; }
        }

        protected bool _isUseCustodianBroker = false;
        [Browsable(false)]
        public virtual bool IsUseCustodianBroker
        {
            get { return _isUseCustodianBroker; }
            set { _isUseCustodianBroker = value; }
        }
        
        protected string _accountBrokerMapping = string.Empty;
        [Browsable(false)]
        public virtual string AccountBrokerMapping
        {
            get { return _accountBrokerMapping; }
            set { _accountBrokerMapping = value; }
        }

        protected bool _isSamsaraUser = false;
        [Browsable(false)]
        public virtual bool IsSamsaraUser
        {
            get { return _isSamsaraUser; }
            set { _isSamsaraUser = value; }
        }

        /// <summary>
        /// This field specifies whether the trade is executed with OTD Broker
        /// </summary>
        protected bool _isMultiBrokerTrade = false;
        [Browsable(false)]
        public virtual bool IsMultiBrokerTrade
        {
            get { return _isMultiBrokerTrade; }
            set { _isMultiBrokerTrade = value; }
        }

        /// <summary>
        /// Update Settlement Currency to trade currency
        /// </summary>
        private void UpdateSettlementCurrency()
        {
            if (_settlementCurrencyID < 1)
            {
                _settlementCurrencyID = _currencyID;
            }
        }

        public virtual void CopyBasicDetails(PranaBasicMessage basicMsg)
        {
            _orderSideTagValue = basicMsg.OrderSideTagValue;
            _orderSide = basicMsg.OrderSide;
            _orderTypeTagValue = basicMsg.OrderTypeTagValue;
            _orderType = basicMsg.OrderType;
            _symbol = basicMsg.Symbol;
            _underLyingsymbol = basicMsg.UnderlyingSymbol;
            _quantity = basicMsg.Quantity;
            _avgPrice = basicMsg.AvgPrice;
            _assetID = basicMsg.AssetID;
            _assetName = basicMsg.AssetName;
            _underlyingID = basicMsg.UnderlyingID;
            _underlyingName = basicMsg.UnderlyingName;
            _exchangeID = basicMsg.ExchangeID;
            _exchangeName = basicMsg.ExchangeName;
            _currencyID = basicMsg.CurrencyID;
            _currencyName = basicMsg.CurrencyName;
            _auecID = basicMsg.AUECID;
            _tradingAccountID = basicMsg.TradingAccountID;
            _tradingAccountName = basicMsg.TradingAccountName;
            _userID = basicMsg.CompanyUserID;
            _CompanyUserName = basicMsg.CompanyUserName;
            _counterPartyID = basicMsg.CounterPartyID;
            _counterPartyName = basicMsg.CounterPartyName;
            _venueID = basicMsg.VenueID;
            _venue = basicMsg.Venue;
            _cumQty = basicMsg.CumQty;
            _contractMultiplier = basicMsg.ContractMultiplier;
            _companyName = basicMsg.CompanyName;
            _aUECLocalDate = basicMsg.AUECLocalDate;
            _processDate = basicMsg.ProcessDate;
            _workflowState = basicMsg.WorkflowState;
            _nirvanaProcessDate = basicMsg.NirvanaProcessDate;
            if (_nirvanaProcessDate <= DateTimeConstants.MinValue)
            {
                _nirvanaProcessDate = basicMsg.AUECLocalDate;
            }
            _originalPurchaseDate = basicMsg.OriginalPurchaseDate;
            _settlementDate = basicMsg.SettlementDate;
            _expirationDate = basicMsg.ExpirationDate;
            _description = basicMsg.Description;
            _internalComments = basicMsg.InternalComments;

            if (_avgFXRateForTrade == 0.0)
            {
                _avgFXRateForTrade = basicMsg.FXRate;
            }
            //Since PM_taxlots contains FXConversionMethodOperator as null, hence object functions can not  
            //be used here. FXConversionMethodOperator.Equals(string.Empty) is a wrong way.
            if (_FXConversionMethodOperator == null || _FXConversionMethodOperator == string.Empty)
            {
                _FXConversionMethodOperator = basicMsg.FXConversionMethodOperator;
            }
            if (_settlementCurrencyID == 0)
            {
                _settlementCurrencyID = basicMsg.SettlementCurrencyID;
            }
            _delta = basicMsg.Delta;
            _accrualBasis = basicMsg.AccrualBasis;
            _bondType = basicMsg.BondType;
            _freq = basicMsg.Freq;
            _firstCouponDate = basicMsg.FirstCouponDate;
            _dateMaturity = basicMsg.MaturityDate;
            _isZero = basicMsg.IsZero;
            _dateIssue = basicMsg.IssueDate;
            _couponRate = basicMsg.CouponRate;
            _vsCurrencyID = basicMsg.VsCurrencyID;
            _sharesOutstanding = basicMsg.SharesOutstanding;
            _tradedCurrencyID = basicMsg.LeadCurrencyID;
            _strikePrice = basicMsg.StrikePrice;
            _putOrCall = basicMsg.PutOrCall;
            _isNDF = basicMsg.IsNDF;
            _fixingDate = basicMsg.FixingDate;
            _description = basicMsg.Description;
            _internalComments = basicMsg.InternalComments;
            _roundLot = basicMsg.RoundLot;
            // Kuldeep A.: this check is put here as Underlying Delta Of basicMsg refers to Leveraged Factor of underlying symbol while Delta of basicMsg refers to actual leveraged factor value. So in case of below asset classes we need to update leveraged factor by the 
            // value of their underlying symbol's leveraged factor.
            if (basicMsg.AssetID == (int)AssetCategory.EquityOption || basicMsg.AssetID == (int)AssetCategory.FutureOption || basicMsg.AssetID == (int)AssetCategory.FXOption || basicMsg.AssetID == (int)AssetCategory.ConvertibleBond)
            {
                _underlyingDelta = basicMsg.UnderlyingDelta;
            }
            else
            {
                _underlyingDelta = basicMsg.Delta;
            }
            _bloombergSymbol = basicMsg.BloombergSymbol;
            _bloombergSymbolWithExchangeCode = basicMsg.BloombergSymbolWithExchangeCode;
            _idcoSymbol = basicMsg.IDCOSymbol;
            _isinSymbol = basicMsg.ISINSymbol;
            _sedolSymbol = basicMsg.SEDOLSymbol;
            _osiSymbol = basicMsg.OSISymbol;
            _cusipSymbol = basicMsg.CusipSymbol;
            _factSetSymbol = basicMsg.FactSetSymbol;
            _activSymbol = basicMsg.ActivSymbol;
            _commissionAmt = basicMsg.CommissionAmt;
            _commissionRate = basicMsg.CommissionRate;
            _calcBasis = basicMsg.CalcBasis;
            _softCommissionAmt = basicMsg.SoftCommissionAmt;
            _softCommissionRate = basicMsg.SoftCommissionRate;
            _softCommissionCalcBasis = basicMsg.SoftCommissionCalcBasis;
            if (string.IsNullOrEmpty(_tradeAttribute1))
            {
                _tradeAttribute1 = basicMsg.TradeAttribute1;
            }
            if (string.IsNullOrEmpty(_tradeAttribute2))
            {
                _tradeAttribute2 = basicMsg.TradeAttribute2;
            }
            if (string.IsNullOrEmpty(_tradeAttribute3))
            {
                _tradeAttribute3 = basicMsg.TradeAttribute3;
            }
            if (string.IsNullOrEmpty(_tradeAttribute4))
            {
                _tradeAttribute4 = basicMsg.TradeAttribute4;
            }
            if (string.IsNullOrEmpty(_tradeAttribute5))
            {
                _tradeAttribute5 = basicMsg.TradeAttribute5;
            }
            if (string.IsNullOrEmpty(_tradeAttribute6))
            {
                _tradeAttribute6 = basicMsg.TradeAttribute6;
            }
            foreach (var attribute in GetTradeAttributesAsDict())
            {
                if (string.IsNullOrEmpty(attribute.Value))
                {
                    SetTradeAttributeValue(attribute.Key, basicMsg.GetTradeAttributeValue(attribute.Key));
                }
            }

            _internalComments = basicMsg._internalComments;
            _changeComment = basicMsg.ChangeComment;
            _proxySymbol = basicMsg.ProxySymbol;
            _transactionType = basicMsg.TransactionType;
            _notionalChange = basicMsg.NotionalChange;
            _transactionSource = basicMsg.TransactionSource;
            _transactionSourceTag = basicMsg.TransactionSourceTag;
            _reutersSymbol = basicMsg.ReutersSymbol;
            _changeType = basicMsg.ChangeType;
            _isCurrencyFuture = basicMsg.IsCurrencyFuture;
            _originalAllocationPreferenceID = basicMsg.OriginalAllocationPreferenceID;
            _nirvanaLocateID = basicMsg.NirvanaLocateID;
            _borrowerID = basicMsg.BorrowerID;
            _borrowerBroker = basicMsg.BorrowerBroker;
            _shortRebate = basicMsg.ShortRebate;
            _isPricingAvailable = basicMsg.IsPricingAvailable;
            _executionTimeLastFill = basicMsg.ExecutionTimeLastFill;
            _averagePriceForCompliance = basicMsg.AvgPriceForCompliance;
            _isSamsaraUser = basicMsg.IsSamsaraUser;
            _isUseCustodianBroker = basicMsg.IsUseCustodianBroker;
        }

        public virtual void CopyBasicDetails(SecMasterBaseObj secMasterObject)
        {
            _contractMultiplier = secMasterObject.Multiplier;
            _bloombergSymbol = secMasterObject.BloombergSymbol;
            _bloombergSymbolWithExchangeCode = secMasterObject.BloombergSymbolWithExchangeCode;
            _cusipSymbol = secMasterObject.CusipSymbol;
            _companyName = secMasterObject.LongName;
            _isinSymbol = secMasterObject.ISINSymbol;
            _sedolSymbol = secMasterObject.SedolSymbol;
            _delta = secMasterObject.Delta;
            _roundLot = secMasterObject.RoundLot;
            _proxySymbol = secMasterObject.ProxySymbol;
            _reutersSymbol = secMasterObject.ReutersSymbol;
            _factSetSymbol = secMasterObject.FactSetSymbol;
            _activSymbol = secMasterObject.ActivSymbol;

            switch (secMasterObject.AssetCategory)
            {
                case AssetCategory.Equity:
                    SecMasterEquityObj equityObject = (SecMasterEquityObj)secMasterObject;
                    _delta = equityObject.Delta;
                    break;

                case AssetCategory.Future:
                    SecMasterFutObj futureObj = (SecMasterFutObj)secMasterObject;
                    _contractMultiplier = futureObj.Multiplier;
                    _companyName = futureObj.LongName;
                    _expirationDate = futureObj.ExpirationDate;
                    _isCurrencyFuture = futureObj.IsCurrencyFuture;
                    break;

                case AssetCategory.EquityOption:
                case AssetCategory.FutureOption:
                    SecMasterOptObj optObject = (SecMasterOptObj)secMasterObject;
                    _strikePrice = optObject.StrikePrice;
                    _putOrCall = optObject.PutOrCall;
                    _osiSymbol = optObject.OSIOptionSymbol;
                    _idcoSymbol = optObject.IDCOOptionSymbol;
                    _expirationDate = optObject.ExpirationDate;
                    _isCurrencyFuture = optObject.IsCurrencyFuture;
                    break;
                case AssetCategory.FixedIncome:
                case AssetCategory.ConvertibleBond:
                    SecMasterFixedIncome fixedIncomeObj = (SecMasterFixedIncome)secMasterObject;
                    _accrualBasis = fixedIncomeObj.AccrualBasis;
                    _bondType = fixedIncomeObj.BondType;
                    _dateIssue = fixedIncomeObj.IssueDate;
                    _isZero = fixedIncomeObj.IsZero;
                    _firstCouponDate = fixedIncomeObj.FirstCouponDate;
                    _freq = fixedIncomeObj.Frequency;
                    _couponRate = fixedIncomeObj.Coupon;
                    _expirationDate = fixedIncomeObj.MaturityDate;
                    break;

                case AssetCategory.PrivateEquity:
                case AssetCategory.CreditDefaultSwap:
                    break;

                case AssetCategory.FXForward:
                    SecMasterFXForwardObj forwardObject = (SecMasterFXForwardObj)secMasterObject;
                    _isNDF = forwardObject.IsNDF;
                    _fixingDate = forwardObject.FixingDate;
                    break;

                case AssetCategory.Option:
                case AssetCategory.Cash:
                case AssetCategory.FX:
                case AssetCategory.Forex:
                case AssetCategory.FXOption:
                case AssetCategory.Indices:
                    break;

                default:
                    break;
            }
        }

        public virtual void FillData(object[] row, int offset)
        {
            int Symbol = offset + 0;
            int CounterPartyID = offset + 1;
            int VenueID = offset + 2;

            int SideTagValue = offset + 3;
            int OrderTypeTag = offset + 4;
            int AUECID = offset + 5;
            int AssetID = offset + 6;
            int UnderLyingID = offset + 7;
            int ExchangeID = offset + 8;
            int CurrencyID = offset + 9;

            int TradingAcID = offset + 10;
            int Quantity = offset + 11;
            int CumQty = offset + 12;
            int Price = offset + 13;
            int CompanyUserID = offset + 14;
            int CompanyName = offset + 15;
            int AUECLocalDate = offset + 26;
            int SettlementDate = offset + 27;
            int ExpirationDate = offset + 28;
            int ProcessDate = offset + 29;
            int OriginalPurchaseDate = offset + 30;

            _orderSideTagValue = row[SideTagValue].ToString();
            _symbol = row[Symbol].ToString();
            _counterPartyID = int.Parse(row[CounterPartyID].ToString());
            _venueID = int.Parse(row[VenueID].ToString());
            _quantity = Convert.ToDouble(row[Quantity]);
            _cumQty = Convert.ToDouble((row[CumQty]));
            _tradingAccountID = int.Parse(row[TradingAcID].ToString());
            _avgPrice = Convert.ToDouble(row[Price]);
            _orderTypeTagValue = row[OrderTypeTag].ToString().Trim();
            _auecID = int.Parse(row[AUECID].ToString());
            _assetID = int.Parse(row[AssetID].ToString());
            _underlyingID = int.Parse(row[UnderLyingID].ToString());
            _exchangeID = int.Parse(row[ExchangeID].ToString());
            _currencyID = int.Parse(row[CurrencyID].ToString());
            _userID = int.Parse(row[CompanyUserID].ToString());
            _companyName = row[CompanyName].ToString();
            _aUECLocalDate = Convert.ToDateTime(row[AUECLocalDate]);
            _settlementDate = Convert.ToDateTime(row[SettlementDate]);
            _expirationDate = Convert.ToDateTime(row[ExpirationDate]);
            _processDate = Convert.ToDateTime(row[ProcessDate]);
            _originalPurchaseDate = Convert.ToDateTime(row[OriginalPurchaseDate]);
        }
    }
}