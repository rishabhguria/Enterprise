using Csla;
using Csla.Validation;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Prana.BusinessObjects
{
    [Serializable(), System.Runtime.InteropServices.ComVisible(false)]
    public class PositionMaster : BusinessBase<PositionMaster> //ImportBase inheritance removed.
    {
        private string _symbology;
        public string Symbology
        {
            get { return _symbology; }
            set { _symbology = value; }
        }

        //CHMW-2305 [Implementation] import dashboard updates-Part 2
        private string _importTag;
        public string ImportTag
        {
            get { return _importTag; }
            set { _importTag = value; }
        }
        public PositionMaster()
        {
            MarkAsChild();
        }

        /// <summary>
        /// will act as a key to the position master file.
        /// </summary>
        private int _rowIndex;
        public int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }

        private bool _IsSymbolMapped = false;
        public bool IsSymbolMapped
        {
            get { return _IsSymbolMapped; }
            set { _IsSymbolMapped = value; }
        }

        public const string VALID = "Validated";
        public const string INVALID = "NotValidated";

        #region Import Base Class properties

        private int _aUECID = 0;
        public int AUECID
        {
            get { return _aUECID; }
            set
            {
                _aUECID = value;
                PropertyHasChanged(ApplicationConstants.CONST_AUECID);
            }
        }

        private int _assetID = 0;
        public int AssetID
        {
            get { return _assetID; }
            set
            {
                _assetID = value;
                PropertyHasChanged(ApplicationConstants.CONST_ASSETID);
            }
        }

        private AssetCategory _assetType;
        public AssetCategory AssetType
        {
            get { return _assetType; }
            set
            {
                _assetType = value;
                PropertyHasChanged(ApplicationConstants.CONST_ASSETNAME);
            }
        }

        private int _call_Put = 0;
        public int Call_Put
        {
            get { return _call_Put; }
            set { _call_Put = value; }
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

        private double _strikePrice = 0;
        public double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }

        private string _cUSIP = string.Empty;
        public string CUSIP
        {
            get { return _cUSIP; }
            set { _cUSIP = value; }
        }

        private string _sEDOL = string.Empty;
        public string SEDOL
        {
            get { return _sEDOL; }
            set { _sEDOL = value; }
        }

        private string _iSIN = string.Empty;
        public string ISIN
        {
            get { return _iSIN; }
            set { _iSIN = value; }
        }

        private string _rIC = string.Empty;
        public string RIC
        {
            get { return _rIC; }
            set { _rIC = value; }
        }

        private string _osiOptionSymbol = string.Empty;
        [Browsable(false)]
        public string OSIOptionSymbol
        {
            get { return _osiOptionSymbol; }
            set { _osiOptionSymbol = value; }
        }

        private string _idcoOptionSymbol = string.Empty;
        [Browsable(false)]
        public string IDCOOptionSymbol
        {
            get { return _idcoOptionSymbol; }
            set { _idcoOptionSymbol = value; }
        }

        private string _opraOptionSymbol = string.Empty;
        [Browsable(false)]
        public string OpraOptionSymbol
        {
            get { return _opraOptionSymbol; }
            set { _opraOptionSymbol = value; }
        }

        private string _symbol = string.Empty;
        public string Symbol
        {
            get { return _symbol; }
            set
            {
                _symbol = value;
                PropertyHasChanged(ApplicationConstants.CONST_SYMBOL);
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

        private string _sideTagValue = string.Empty;
        public string SideTagValue
        {
            get { return _sideTagValue; }
            set
            {
                _sideTagValue = value;
                PropertyHasChanged(ApplicationConstants.CONST_SIDETAGVALUE);
            }
        }

        private string _side = string.Empty;
        public string Side
        {
            get { return _side; }
            set
            {
                _side = value;
                PropertyHasChanged(ApplicationConstants.CONST_SIDE);
            }
        }

        private int _venueID = 0;
        public int VenueID
        {
            get { return _venueID; }
            set { _venueID = value; }
        }

        private int _counterPartyID = 0;
        public int CounterPartyID
        {
            get { return _counterPartyID; }
            set { _counterPartyID = value; }
        }

        private string _executingBroker = string.Empty;
        public string ExecutingBroker
        {
            get { return _executingBroker; }
            set { _executingBroker = value; }
        }

        private int _userID = 0;
        public int UserID
        {
            get { return _userID; }
            set
            {
                _userID = value;
                PropertyHasChanged(ApplicationConstants.CONST_USERID);
            }
        }

        private int _tradingAccountID = 0;
        public int TradingAccountID
        {
            get { return _tradingAccountID; }
            set
            {
                _tradingAccountID = value;
                PropertyHasChanged(ApplicationConstants.CONST_TRADINGACCOUNTID);
            }
        }

        private string _aUECLocalDate = string.Empty;
        public string AUECLocalDate
        {
            get { return _aUECLocalDate; }
            set { _aUECLocalDate = value; }
        }

        private int _strategyID = 0;
        public int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }

        private string _strategy = string.Empty;
        public string Strategy
        {
            get { return _strategy; }
            set { _strategy = value; }
        }
        #endregion

        #region Is Security Approved Status

        public const string APPROVED = "Approved";
        public const string UNAPPROVED = "UnApproved";

        private bool _IsSecApproved = false;
        public bool IsSecApproved
        {
            get { return _IsSecApproved; }
            set
            {
                _IsSecApproved = value;
                if (_IsSecApproved)
                    SecApprovalStatus = APPROVED;
                else
                    SecApprovalStatus = UNAPPROVED;
                PropertyHasChanged(ApplicationConstants.CONST_IS_SECURITY_APPROVED);
                PropertyHasChanged(ApplicationConstants.CONST_SEC_APPROVED_STATUS);
            }
        }

        private String _secApprovalStatus;
        public String SecApprovalStatus
        {
            get { return _secApprovalStatus; }
            set
            {
                _secApprovalStatus = value;
                PropertyHasChanged(ApplicationConstants.CONST_SEC_APPROVED_STATUS);
            }
        }

        #endregion

        private string _validationStatus = ApplicationConstants.ValidationStatus.None.ToString();
        public string ValidationStatus
        {
            get { return _validationStatus; }
            set
            {
                _validationStatus = value;
            }
        }

        private string _validationError = string.Empty;
        public string ValidationError
        {
            get { return _validationError; }
            set { _validationError = value; }
        }

        private string _mismatchType = string.Empty;
        public string MismatchType
        {
            get { return _mismatchType; }
            set { _mismatchType = value; }
        }
        private string _misMatchDetails;
        public string MisMatchDetails
        {
            get { return _misMatchDetails; }
            set { _misMatchDetails = value; }
        }

        private double _multiplier = 0;
        public double Multiplier
        {
            get { return _multiplier; }
            set { _multiplier = value; }
        }

        private double _netPosition = 0;
        public double NetPosition
        {
            get { return _netPosition; }
            set
            {
                _netPosition = value;
                PropertyHasChanged(ApplicationConstants.CONST_NETPOSITION);
            }
        }

        private int _secFees = 0;
        public int SecFees
        {
            get { return _secFees; }
            set { _secFees = value; }
        }

        private double _optionPremiumAdjustment;
        public double OptionPremiumAdjustment
        {
            get { return _optionPremiumAdjustment; }
            set { _optionPremiumAdjustment = value; }
        }

        private double _startQuantity = 0;
        public double StartQuantity
        {
            get { return _startQuantity; }
            set { _startQuantity = value; }
        }

        private int _taxLotID = 0;
        public int TaxLotID
        {
            get { return _taxLotID; }
            set { _taxLotID = value; }
        }

        private double _commission = 0;
        public double Commission
        {
            get { return _commission; }
            set { _commission = value; }
        }

        private double _softCommission = 0;
        public double SoftCommission
        {
            get { return _softCommission; }
            set { _softCommission = value; }
        }

        private int _leadCurrencyID;
        public int LeadCurrencyID
        {
            get { return _leadCurrencyID; }
            set { _leadCurrencyID = value; }
        }

        private int _vsCurrencyID;
        public int VsCurrencyID
        {
            get { return _vsCurrencyID; }
            set { _vsCurrencyID = value; }
        }

        private double _costBasis = 0;
        public double CostBasis
        {
            get { return _costBasis; }
            set
            {
                //Do not update if value is same
                if (_costBasis != value)
                {
                    _costBasis = value;
                }
                PropertyHasChanged(ApplicationConstants.CONST_COSTBASIS);
            }
        }

        private double _fees = 0;
        public double Fees
        {
            get { return _fees; }
            set { _fees = value; }
        }

        private double _clearingBrokerFee = 0;
        public double ClearingBrokerFee
        {
            get { return _clearingBrokerFee; }
            set { _clearingBrokerFee = value; }
        }

        private string _createdByID = string.Empty;
        public string CreatedByID
        {
            get { return _createdByID; }
            set { _createdByID = value; }
        }

        private string _derivativeRootSymbol = string.Empty;
        public string DerivativeRootSymbol
        {
            get { return _derivativeRootSymbol; }
            set { _derivativeRootSymbol = value; }
        }

        private string _derivativeUnderlyingSymbol = string.Empty;
        public string DerivativeUnderlyingSymbol
        {
            get { return _derivativeUnderlyingSymbol; }
            set { _derivativeUnderlyingSymbol = value; }
        }

        private int _accountID = 0;
        public int AccountID
        {
            get { return _accountID; }
            set
            {
                _accountID = value;
                PropertyHasChanged("AccountID");
            }
        }

        private string _accountName = string.Empty;
        public string AccountName
        {
            get { return _accountName; }
            set
            {
                _accountName = value;
                PropertyHasChanged(ApplicationConstants.CONST_FUNDNAME);
            }
        }

        private string _positionType = string.Empty;
        public string PositionType
        {
            get { return _positionType; }
            set { _positionType = value; }
        }

        private string _pBSymbol = string.Empty;
        public string PBSymbol
        {
            get { return _pBSymbol; }
            set { _pBSymbol = value; }
        }

        private string _pBAssetType = string.Empty;
        public string PBAssetType
        {
            get { return _pBAssetType; }
            set { _pBAssetType = value; }
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

        /// <summary>
        /// Current status of trade/ taxlot
        /// Added by om shiv, Nov, 14
        /// </summary>
        protected NirvanaWorkFlowsStats _workflowState = NirvanaWorkFlowsStats.None;
        public virtual NirvanaWorkFlowsStats WorkflowState
        {
            get { return _workflowState; }
            set { _workflowState = value; }
        }

        private string _expirationDate = string.Empty;
        public string ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }

        private string _positionStartDate = string.Empty;
        public string PositionStartDate
        {
            get { return _positionStartDate; }
            set
            {
                _positionStartDate = value;
                PropertyHasChanged("PositionStartDate");
            }
        }

        private int _uploadID = 0;
        public int UploadID
        {
            get { return _uploadID; }
            set { _uploadID = value; }
        }

        private int _currencyID = 0;
        public int CurrencyID
        {
            get { return _currencyID; }
            set
            {
                _currencyID = value;
                UpdateSettlementCurrency();
            }
        }

        private int _companyID = 0;
        public int CompanyID
        {
            get { return _companyID; }
            set
            {
                _companyID = value;
                PropertyHasChanged(ApplicationConstants.CONST_COMPANYID);
            }
        }

        private int _stateIDForStrategy = 0;
        public int StateIDForStrategy
        {
            get { return _stateIDForStrategy; }
            set { _stateIDForStrategy = value; }
        }

        private string _processDate;
        public string ProcessDate
        {
            get { return _processDate; }
            set { _processDate = value; }
        }

        private string _originalPurchaseDate;
        public string OriginalPurchaseDate
        {
            get { return _originalPurchaseDate; }
            set { _originalPurchaseDate = value; }
        }

        private string _positionSettlementDate = string.Empty;
        public string PositionSettlementDate
        {
            get { return _positionSettlementDate; }
            set { _positionSettlementDate = value; }
        }

        private string _positionExpirationDate = string.Empty;
        public string PositionExpirationDate
        {
            get { return _positionExpirationDate; }
            set { _positionExpirationDate = value; }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _internalComments = string.Empty;
        public string InternalComments
        {
            get { return _internalComments; }
            set { _internalComments = value; }
        }

        private string _bloomberg = string.Empty;
        public string Bloomberg
        {
            get { return _bloomberg; }
            set { _bloomberg = value; }
        }

        private double _stampDuty;
        /// <summary>
        /// Gets or sets the StampDuty.
        /// </summary>
        /// <value>The StampDuty.</value>
        public double StampDuty
        {
            get { return _stampDuty; }
            set { _stampDuty = value; }
        }

        private double _transactionLevy;
        /// <summary>
        /// Gets or sets the TransactionLevy.
        /// </summary>
        /// <value>The TransactionLevy.</value>
        public double TransactionLevy
        {
            get { return _transactionLevy; }
            set { _transactionLevy = value; }
        }

        private double _clearingFee;
        /// <summary>
        /// Gets or sets the ClearingFee.
        /// </summary>
        /// <value>The ClearingFee.</value>
        public double ClearingFee
        {
            get { return _clearingFee; }
            set { _clearingFee = value; }
        }

        private double _taxOnCommissions;
        /// <summary>
        /// Gets or sets the TaxOnCommissions.
        /// </summary>
        /// <value>The TaxOnCommissions.</value>
        public double TaxOnCommissions
        {
            get { return _taxOnCommissions; }
            set { _taxOnCommissions = value; }
        }

        private double _miscFees;
        /// <summary>
        /// Gets or sets the MiscFees.
        /// </summary>
        /// <value>The MiscFees.</value>
        public double MiscFees
        {
            get { return _miscFees; }
            set { _miscFees = value; }
        }

        private double _secFee;
        /// <summary>
        /// Gets or sets the SecFee.
        /// </summary>
        /// <value>The SecFee.</value>
        public double SecFee
        {
            get { return _secFee; }
            set { _secFee = value; }
        }

        private double _occFee;
        /// <summary>
        /// Gets or sets the OccFee.
        /// </summary>
        /// <value>The OccFee.</value>
        public double OccFee
        {
            get { return _occFee; }
            set { _occFee = value; }
        }

        private double _orfFee;
        /// <summary>
        /// Gets or sets the OrfFee.
        /// </summary>
        /// <value>The OrfFee.</value>
        public double OrfFee
        {
            get { return _orfFee; }
            set { _orfFee = value; }
        }

        private string _groupID = string.Empty;
        public string GroupID
        {
            get { return _groupID; }
            set { _groupID = value; }
        }

        private string _lotId = string.Empty;
        /// <summary>
        /// Added By Sandeep as on 22-Feb-2013
        /// this keep the lot ID send by the Nirvana client i.e. user
        /// </summary>    
        public string LotId
        {
            get { return _lotId; }
            set { _lotId = value; }
        }

        private string _externalTransId;
        /// <summary>
        /// Added By Sandeep as on 25-Feb-2013
        /// this keep the External Transaction ID send by the Nirvana client side i.e. user
        /// </summary>   
        public virtual string ExternalTransId
        {
            get { return _externalTransId; }
            set { _externalTransId = value; }
        }

        private string _transactionType = string.Empty;
        /// <summary>
        /// Added By Narendra Jangir as on 03-Sept-2013
        /// This field transaction type of trade either it is new/cancel/rebook/Expiry/Exercise trade
        /// </summary>   
        public virtual string TransactionType
        {
            get { return _transactionType; }
            set { _transactionType = value; }
        }

        /// <summary>
        /// Sandeep Singh, Oct 19 2014
        /// This field specifies origin of the trade from where it is generated
        /// </summary>
        protected TransactionSource _transactionSource;
        public virtual TransactionSource TransactionSource
        {
            get { return _transactionSource; }
            set { _transactionSource = value; }
        }

        private string _expiredTaxlotID = string.Empty;
        /// <summary>
        /// expired taxlotID aginst which position has generated
        /// </summary>
        [Browsable(false)]
        public string ExpiredTaxlotID
        {
            get { return _expiredTaxlotID; }
            set { _expiredTaxlotID = value; }
        }

        private double _expiredQty = 0;
        /// <summary>
        /// expired Qty against which position has generated
        /// </summary>
        [Browsable(false)]
        public double ExpiredQty
        {
            get { return _expiredQty; }
            set { _expiredQty = value; }
        }

        private string _taxLotClosingId = string.Empty;
        public string TaxLotClosingId
        {
            get { return _taxLotClosingId; }
            set { _taxLotClosingId = value; }
        }

        /// <summary>
        /// Sets the import status of the position master object
        /// </summary>
        // http://jira.nirvanasolutions.com:8080/browse/CHMW-1294
        private string _importStatus = Prana.BusinessObjects.AppConstants.ImportStatus.None.ToString();
        public string ImportStatus
        {
            get { return _importStatus; }
            set { _importStatus = value; }
        }

        /// <summary>
        /// set import file ID to check duplicate data from import 
        /// </summary>
        private int _importFileID;
        public int ImportFileID
        {
            get { return _importFileID; }
            set { _importFileID = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value><c>true</c> if this instance is dirty; otherwise, <c>false</c>.</value>
        [Browsable(false)]
        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

        /// <summary>
        /// This Object's hashcode will be the unique id for the object.
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return _assetID;
        }

        //for defining source
        private OrderFields.PranaMsgTypes _pranaMsgType;
        [Browsable(false)]
        [XmlIgnore]
        public OrderFields.PranaMsgTypes PranaMsgType
        {
            get { return _pranaMsgType; }
            set
            {
                _pranaMsgType = value;
                _intPranaMsgType = (int)_pranaMsgType;
            }
        }

        private int _intPranaMsgType = int.MinValue;
        [Browsable(false)]
        public int IntPranaMsgType
        {
            get { return _intPranaMsgType; }
            set
            {
                _intPranaMsgType = value;
                _pranaMsgType = (OrderFields.PranaMsgTypes)_intPranaMsgType;
            }
        }

        private string _externalOrderID;
        [Browsable(false)]
        public string ExternalOrderID
        {
            get { return _externalOrderID; }
            set { _externalOrderID = value; }
        }

        int _putOrCall = int.MinValue;
        [Browsable(false)]
        public int PutCall
        {
            get { return _putOrCall; }
            set { _putOrCall = value; }
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

        private bool _isManualyExerciseAssign = false;
        [Browsable(false)]

        public bool IsManualyExerciseAssign
        {
            get { return _isManualyExerciseAssign; }
            set { _isManualyExerciseAssign = value; }
        }

        private DateTime _firstCouponDate = DateTimeConstants.MinValue;
        [Browsable(false)]
        public DateTime FirstCouponDate
        {
            get { return _firstCouponDate; }
            set { _firstCouponDate = value; }
        }

        private DateTime _dateIssue = DateTimeConstants.MinValue;
        [Browsable(false)]
        public DateTime IssueDate
        {
            get { return _dateIssue; }
            set { _dateIssue = value; }
        }

        private DateTime _dateMaturity = DateTimeConstants.MinValue;
        [Browsable(false)]
        public DateTime MaturityDate
        {
            get { return _dateMaturity; }
            set { _dateMaturity = value; }
        }

        private double _coupon;
        [Browsable(false)]
        public double Coupon
        {
            get { return _coupon; }
            set { _coupon = value; }
        }

        private double _accruedInterest;
        [Browsable(false)]
        public double AccruedInterest
        {
            get { return _accruedInterest; }
            set { _accruedInterest = value; }
        }

        private string _tradeAttribute1 = string.Empty;
        [Browsable(false)]
        public string TradeAttribute1
        {
            get { return _tradeAttribute1; }
            set { _tradeAttribute1 = value; }
        }

        private string _tradeAttribute2 = string.Empty;
        [Browsable(false)]
        public string TradeAttribute2
        {
            get { return _tradeAttribute2; }
            set { _tradeAttribute2 = value; }
        }

        private string _tradeAttribute3 = string.Empty;
        [Browsable(false)]
        public string TradeAttribute3
        {
            get { return _tradeAttribute3; }
            set { _tradeAttribute3 = value; }
        }

        private string _tradeAttribute4 = string.Empty;
        [Browsable(false)]
        public string TradeAttribute4
        {
            get { return _tradeAttribute4; }
            set { _tradeAttribute4 = value; }
        }

        private string _tradeAttribute5 = string.Empty;
        [Browsable(false)]
        public string TradeAttribute5
        {
            get { return _tradeAttribute5; }
            set { _tradeAttribute5 = value; }
        }

        private string _tradeAttribute6 = string.Empty;
        [Browsable(false)]
        public string TradeAttribute6
        {
            get { return _tradeAttribute6; }
            set { _tradeAttribute6 = value; }
        }

        //added RoundLot field, PRANA-12674
        private decimal _roundLot = 1;
        [Browsable(false)]
        public virtual decimal RoundLot
        {
            get { return _roundLot; }
            set { _roundLot = value; }
        }

        #region Swap Related Properties

        private int _isSwapped;
        public virtual int IsSwapped
        {
            get { return _isSwapped; }
            set { _isSwapped = value; }
        }

        #region properties
        private double _notionalValue;
        private int _dayCount;
        private double _benchMarkRate;
        private double _differential;
        private string _firstResetDate;
        private string _resetFrequency;
        private string _origTransDate;
        private string _swapDescription;

        public virtual double NotionalValue
        {
            get { return _notionalValue; }
            set { _notionalValue = value; }
        }

        public virtual int DayCount
        {
            get { return _dayCount; }
            set { _dayCount = value; }
        }

        public virtual double BenchMarkRate
        {
            get { return _benchMarkRate; }
            set { _benchMarkRate = value; }
        }

        public virtual double Differential
        {
            get { return _differential; }
            set { _differential = value; }
        }

        public virtual string FirstResetDate
        {
            get { return _firstResetDate; }
            set { _firstResetDate = value; }
        }

        public virtual string ResetFrequency
        {
            get { return _resetFrequency; }
            set { _resetFrequency = value; }
        }

        /// <summary>
        /// The creation date of original Swap of which this is a rollover
        /// </summary>
        public virtual string OrigTransDate
        {
            get { return _origTransDate; }
            set { _origTransDate = value; }
        }

        public virtual string SwapDescription
        {
            get { return _swapDescription; }
            set { _swapDescription = value; }
        }
        #endregion
        #endregion

        private int _settlementCurrencyID;
        public virtual int SettlementCurrencyID
        {
            get { return _settlementCurrencyID; }
            set { _settlementCurrencyID = value; UpdateSettlementCurrency(); }
        }

        private string _settlCurrencyName;
        public virtual string SettlCurrencyName
        {
            get { return _settlCurrencyName; }
            set { _settlCurrencyName = value; }
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

        private Prana.BusinessObjects.AppConstants.Operator _FXConversionMethodOperator = Prana.BusinessObjects.AppConstants.Operator.M;
        public Prana.BusinessObjects.AppConstants.Operator FXConversionMethodOperator
        {
            get { return _FXConversionMethodOperator; }
            set { _FXConversionMethodOperator = value; }
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

        private double _avgFXRate = 0.0;
        public double FXRate
        {
            get { return _avgFXRate; }
            set { _avgFXRate = value; }
        }

        private double _markPrice = 0.0;
        public double MarkPrice
        {
            get { return _markPrice; }
            set { _markPrice = value; }
        }

        private string _underlyingSymbol;
        [Browsable(false)]
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }

        /// <summary>
        /// Specifies change type of the trade
        /// </summary>
        private int _changeType = (int)AppConstants.ChangeType.NoTrade;
        public int ChangeType
        {
            get { return _changeType; }
            set { _changeType = value; }
        }

        private static AccountCollection _accountsList = new AccountCollection();
        [Browsable(false)]
        public static AccountCollection AccountsList
        {
            get
            {
                return _accountsList;
            }
            set
            {
                _accountsList = value;
                CustomRules.AccountsList = value;
            }
        }

        private static int _totalAccounts = 0;
        [Browsable(false)]
        public static int TotalAccounts
        {
            get
            {
                return _totalAccounts;
            }
            set
            {
                _totalAccounts = value;
                CustomRules.TotalAccounts = value;
            }
        }

        #region Additional Trade Attributes

        [Browsable(false)]
        public virtual string TradeAttribute7 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute8 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute9 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute10 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute11 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute12 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute13 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute14 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute15 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute16 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute17 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute18 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute19 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute20 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute21 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute22 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute23 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute24 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute25 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute26 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute27 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute28 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute29 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute30 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute31 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute32 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute33 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute34 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute35 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute36 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute37 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute38 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute39 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute40 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute41 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute42 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute43 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute44 { get; set; } = string.Empty;
        [Browsable(false)]
        public virtual string TradeAttribute45 { get; set; } = string.Empty;

        #endregion Additional Trade Attributes

        #region AdditionalTradeAttributes Utilities
        private static readonly Dictionary<string, Action<PositionMaster, string>> _attributeSetters =
           new Dictionary<string, Action<PositionMaster, string>>
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

        private static readonly Dictionary<string, Func<PositionMaster, string>> _attributeGetters =
            new Dictionary<string, Func<PositionMaster, string>>
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

        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(CustomRules.AUECIDCheck, ApplicationConstants.CONST_AUECID);
            ValidationRules.AddRule(CustomRules.AssetIDCheck, ApplicationConstants.CONST_ASSETID);
            ValidationRules.AddRule(CustomRules.NetPositionCheck, ApplicationConstants.CONST_NETPOSITION);
            ValidationRules.AddRule(CustomRules.SideTagValueCheck, ApplicationConstants.CONST_SIDETAGVALUE);
            ValidationRules.AddRule(CustomRules.SideCheck, ApplicationConstants.CONST_SIDE);
            ValidationRules.AddRule(CustomRules.AccountPermissionCheck, ApplicationConstants.CONST_FUNDNAME);
            ValidationRules.AddRule(CustomRules.ExchangeCheck, ApplicationConstants.CONST_EXCHANGEID);
            ValidationRules.AddRule(CustomRules.UnderLyingCheck, ApplicationConstants.CONST_UNDERLYINGID);
            ValidationRules.AddRule(CustomRules.UserIDCheck, ApplicationConstants.CONST_USERID);
            ValidationRules.AddRule(CustomRules.TradingAccountIDCheck, ApplicationConstants.CONST_TRADINGACCOUNTID);
            ValidationRules.AddRule(CustomRules.DateCheck, "PositionStartDate");
            ValidationRules.AddRule(CustomRules.IsSecurityApprovedCheck, ApplicationConstants.CONST_SEC_APPROVED_STATUS);
            ValidationRules.AddRule(CustomRules.CounterPartyIDCheck, ApplicationConstants.CONST_COUNTERPARTYID);
        }

        [System.Runtime.InteropServices.ComVisible(false)]
        public class CustomRules : RuleArgs
        {
            public CustomRules(string validation)
                : base(validation)
            {
            }

            private static AccountCollection _accountsList = new AccountCollection();
            public static AccountCollection AccountsList
            {
                get { return _accountsList; }
                set { _accountsList = value; }
            }

            private static int _totalAccounts = 0;
            [Browsable(false)]
            public static int TotalAccounts
            {
                get { return _totalAccounts; }
                set { _totalAccounts = value; }
            }

            public static void CheckValidationForAllFields(PositionMaster finalTarget)
            {
                if (finalTarget.AUECID <= 0 || finalTarget.AssetID <= 0 || finalTarget.NetPosition <= 0 ||
                       string.IsNullOrEmpty(finalTarget.SideTagValue) || string.IsNullOrEmpty(finalTarget.Side) ||
                    finalTarget._exchangeID.Equals(ApplicationConstants.CONST_ZERO) || finalTarget._underlyingID.Equals(ApplicationConstants.CONST_ZERO) ||
                    finalTarget.UserID.Equals(ApplicationConstants.CONST_ZERO) || finalTarget.TradingAccountID.Equals(ApplicationConstants.CONST_ZERO) ||
                    finalTarget.PositionStartDate.Equals(string.Empty) || finalTarget.PositionStartDate.Equals(DateTimeConstants.DateTimeMinVal) || !finalTarget.IsSecApproved || (!string.IsNullOrEmpty(finalTarget.AccountName) && !AccountsList.Contains(finalTarget.AccountName)) || (string.IsNullOrEmpty(finalTarget.AccountName) && AccountsList.Count - 1 < TotalAccounts) ||
                    !NAVLockDateRule.ValidateNAVLockDate(finalTarget.PositionStartDate) || ((finalTarget.SideTagValue.Equals(FIXConstants.SIDE_Sell) 
                    || finalTarget.SideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || finalTarget.SideTagValue.Equals(FIXConstants.SIDE_Sell_Closed)) 
                    && !NAVLockDateRule.ValidateNAVLockDate(finalTarget.OriginalPurchaseDate)))
                {
                    if (finalTarget.AUECID <= 0)
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.NotExists.ToString();
                    }
                    else if (!finalTarget.IsSecApproved)
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.UnApproved.ToString();
                    }
                    else if ((!string.IsNullOrEmpty(finalTarget.AccountName) && !AccountsList.Contains(finalTarget.AccountName)) || (string.IsNullOrEmpty(finalTarget.AccountName) && AccountsList.Count - 1 < TotalAccounts))
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.NonPermittedAccounts.ToString();
                    }
                    else if (!NAVLockDateRule.ValidateNAVLockDate(finalTarget.PositionStartDate))
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.NonValidated.ToString();
                    }
                    // In case of close side trades (Sell, Buy to Close, Sell to Close) where the Trade Date is after the NAV lock date & original purchase date is before the NAV lock date then system need to block these trades
                    else if ((finalTarget.SideTagValue.Equals(FIXConstants.SIDE_Sell) || finalTarget.SideTagValue.Equals(FIXConstants.SIDE_Buy_Closed)
                        || finalTarget.SideTagValue.Equals(FIXConstants.SIDE_Sell_Closed)) && !NAVLockDateRule.ValidateNAVLockDate(finalTarget.OriginalPurchaseDate))
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.NonValidated.ToString();
                    }
                    else
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.MissingData.ToString();

                        if (string.IsNullOrEmpty(finalTarget.SideTagValue))
                        {
                            finalTarget.Description = "Side is missing";
                            SetOrRemoveValidationError(finalTarget, "Side is missing", true);
                        }
                        else
                        {
                            SetOrRemoveValidationError(finalTarget, "Side is missing", false);
                        }
                        if (finalTarget.PositionStartDate.Equals(string.Empty) || finalTarget.PositionStartDate.Equals(DateTimeConstants.DateTimeMinVal))
                        {
                            finalTarget.Description = "Position start date is missing";
                            SetOrRemoveValidationError(finalTarget, "Position start date is missing", true);
                        }
                        else
                        {
                            SetOrRemoveValidationError(finalTarget, "Position start date is missing", false);
                        }
                        if (finalTarget.TradingAccountID.Equals(ApplicationConstants.CONST_ZERO))
                        {
                            finalTarget.Description = "Trading Account is missing";
                            SetOrRemoveValidationError(finalTarget, "Trading Account is missing", true);
                        }
                        else
                        {
                            SetOrRemoveValidationError(finalTarget, "Trading Account is missing", false);
                        }
                    }
                }
                //we are checking if there is no message set in Validation Error. Which means the security is Approved
                else if (!string.IsNullOrEmpty(finalTarget.MismatchType))
                {
                    finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.NonValidated.ToString();
                }
                else
                {
                    finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.Validated.ToString();
                }

            }

            public static bool AUECIDCheck(object target, RuleArgs e)
            {
                PositionMaster finalTarget = target as PositionMaster;
                if (finalTarget != null)
                {
                    CheckValidationForAllFields(finalTarget);
                    if (finalTarget.AUECID <= 0)
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.NotExists.ToString();
                        e.Description = "AUECID required";
                        //changed message on Proceed to vlidation UI for invalid security
                        SetOrRemoveValidationError(finalTarget, "Security identifying information not found [AUECID required]", true);
                        return false;
                    }
                    else
                    {
                        SetOrRemoveValidationError(finalTarget, "Security identifying information not found [AUECID required]", false);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            public static bool AssetIDCheck(object target, RuleArgs e)
            {
                PositionMaster finalTarget = target as PositionMaster;
                if (finalTarget != null)
                {
                    CheckValidationForAllFields(finalTarget);

                    if (finalTarget.AssetID <= 0 && finalTarget.IsSecApproved)
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.MissingData.ToString();
                        e.Description = "AssetID required";
                        SetOrRemoveValidationError(finalTarget, "AssetID required", true);
                        return false;
                    }
                    else
                    {
                        SetOrRemoveValidationError(finalTarget, "AssetID required", false);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            public static bool NetPositionCheck(object target, RuleArgs e)
            {
                PositionMaster finalTarget = target as PositionMaster;
                if (finalTarget != null)
                {
                    CheckValidationForAllFields(finalTarget);
                    if (finalTarget.NetPosition <= 0 && finalTarget.IsSecApproved)
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.MissingData.ToString();
                        e.Description = "Invalid Quantity";
                        SetOrRemoveValidationError(finalTarget, "Invalid Quantity", true);
                        return false;
                    }
                    else
                    {
                        SetOrRemoveValidationError(finalTarget, "Invalid Quantity", false);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            public static bool SideTagValueCheck(object target, RuleArgs e)
            {
                PositionMaster finalTarget = target as PositionMaster;
                if (finalTarget != null)
                {
                    CheckValidationForAllFields(finalTarget);

                    if (finalTarget._symbol != null)
                    {
                        if (string.IsNullOrEmpty(finalTarget.SideTagValue) && finalTarget.IsSecApproved)
                        {
                            finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.MissingData.ToString();
                            e.Description = "Side required";
                            SetOrRemoveValidationError(finalTarget, "Side required", true);
                            return false;
                        }
                        else
                        {
                            SetOrRemoveValidationError(finalTarget, "Side required", false);
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

            // added check rule on side of trade - omshiv
            public static bool SideCheck(object target, RuleArgs e)
            {
                PositionMaster finalTarget = target as PositionMaster;
                if (finalTarget != null)
                {
                    CheckValidationForAllFields(finalTarget);

                    if (finalTarget._symbol != null)
                    {
                        if (string.IsNullOrEmpty(finalTarget.Side) && finalTarget.IsSecApproved)
                        {
                            finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.MissingData.ToString();
                            e.Description = "Side required";
                            SetOrRemoveValidationError(finalTarget, "Side required", true);
                            return false;
                        }
                        else
                        {
                            SetOrRemoveValidationError(finalTarget, "Side required", false);
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

            public static bool AveragePriceCheck(object target, RuleArgs e)
            {
                PositionMaster finalTarget = target as PositionMaster;
                if (finalTarget != null)
                {
                    CheckValidationForAllFields(finalTarget);

                    if (finalTarget.CostBasis < 0 && finalTarget.IsSecApproved)
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.MissingData.ToString();
                        e.Description = "Average price required";
                        SetOrRemoveValidationError(finalTarget, "Average price required", false);
                        return false;
                    }
                    else
                    {
                        SetOrRemoveValidationError(finalTarget, "Average price required", false);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            public static bool AccountCheck(object target, RuleArgs e)
            {
                PositionMaster finalTarget = target as PositionMaster;
                if (finalTarget != null)
                {
                    if (finalTarget.AccountName != null)
                    {
                        CheckValidationForAllFields(finalTarget);
                        if (finalTarget.AccountName.Equals(string.Empty) && finalTarget.IsSecApproved)
                        {
                            finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.MissingData.ToString();
                            e.Description = "Account Name required";
                            SetOrRemoveValidationError(finalTarget, "Account Name required", true);
                            return false;
                        }
                        else
                        {
                            SetOrRemoveValidationError(finalTarget, "Account Name required", false);
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

            public static bool AccountPermissionCheck(object target, RuleArgs e)
            {
                PositionMaster finalTarget = target as PositionMaster;
                if (finalTarget != null)
                {
                    if (finalTarget.AccountName != null)
                    {
                        CheckValidationForAllFields(finalTarget);

                        if ((!string.IsNullOrEmpty(finalTarget.AccountName) && !AccountsList.Contains(finalTarget.AccountName)) || (string.IsNullOrEmpty(finalTarget.AccountName) && AccountsList.Count - 1 < TotalAccounts))
                        {
                            finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.NonPermittedAccounts.ToString();
                            e.Description = "Account Is not Permitted to the User";
                            SetOrRemoveValidationError(finalTarget, "Account Permission required", true);
                            return false;
                        }
                        else
                        {
                            SetOrRemoveValidationError(finalTarget, "Account Permission required", false);
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

            public static bool ExchangeCheck(object target, RuleArgs e)
            {
                PositionMaster finalTarget = target as PositionMaster;
                if (finalTarget != null)
                {
                    CheckValidationForAllFields(finalTarget);

                    if (finalTarget.ExchangeID.Equals(ApplicationConstants.CONST_ZERO) && finalTarget.IsSecApproved)
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.MissingData.ToString();
                        e.Description = "Exchange required";
                        SetOrRemoveValidationError(finalTarget, "Exchange required", true);
                        return false;
                    }
                    else
                    {
                        SetOrRemoveValidationError(finalTarget, "Exchange required", false);
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
                PositionMaster finalTarget = target as PositionMaster;
                if (finalTarget != null)
                {
                    CheckValidationForAllFields(finalTarget);

                    if (finalTarget.UnderlyingID.Equals(ApplicationConstants.CONST_ZERO) && finalTarget.IsSecApproved)
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.MissingData.ToString();
                        e.Description = "Underlying required";
                        SetOrRemoveValidationError(finalTarget, "Underlying required", true);
                        return false;
                    }
                    else
                    {
                        SetOrRemoveValidationError(finalTarget, "Underlying required", false);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            public static bool UserIDCheck(object target, RuleArgs e)
            {
                PositionMaster finalTarget = target as PositionMaster;
                if (finalTarget != null)
                {
                    CheckValidationForAllFields(finalTarget);
                    if (finalTarget.UserID.Equals(ApplicationConstants.CONST_ZERO) && finalTarget.IsSecApproved)
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.MissingData.ToString();
                        e.Description = "User required";
                        SetOrRemoveValidationError(finalTarget, "User required", true);
                        return false;
                    }
                    else
                    {
                        SetOrRemoveValidationError(finalTarget, "User required", false);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            public static bool CompanyIDCheck(object target, RuleArgs e)
            {
                PositionMaster finalTarget = target as PositionMaster;
                if (finalTarget != null)
                {
                    CheckValidationForAllFields(finalTarget);

                    if (finalTarget.CompanyID < 0 && finalTarget.IsSecApproved)
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.MissingData.ToString();
                        e.Description = "CompanyID required";
                        SetOrRemoveValidationError(finalTarget, "CompanyID required", true);
                        return false;
                    }
                    else
                    {
                        SetOrRemoveValidationError(finalTarget, "CompanyID required", false);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            public static bool TradingAccountIDCheck(object target, RuleArgs e)
            {
                PositionMaster finalTarget = target as PositionMaster;
                if (finalTarget != null)
                {
                    CheckValidationForAllFields(finalTarget);

                    if (finalTarget.TradingAccountID.Equals(ApplicationConstants.CONST_ZERO))
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.MissingData.ToString();
                        e.Description = "Trading Account ID required";
                        SetOrRemoveValidationError(finalTarget, "Trading Account ID required", true);
                        return false;
                    }
                    else
                    {
                        SetOrRemoveValidationError(finalTarget, "Trading Account ID required", false);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            public static bool DateCheck(object target, RuleArgs e)
            {
                PositionMaster finalTarget = target as PositionMaster;
                if (finalTarget != null)
                {
                    CheckValidationForAllFields(finalTarget);

                    if ((string.IsNullOrEmpty(finalTarget.PositionStartDate) || finalTarget.PositionStartDate.Equals(DateTimeConstants.DateTimeMinVal)) && finalTarget.IsSecApproved)
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.MissingData.ToString();
                        e.Description = "Date required";
                        SetOrRemoveValidationError(finalTarget, "Date required", true);
                        return false;
                    }
                    else if (!NAVLockDateRule.ValidateNAVLockDate(finalTarget.PositionStartDate))
                    {
                        e.Description = "The date you’ve chosen for this action precedes your NAV Lock date (" + NAVLockDateRule.NAVLockDate.Value.ToShortDateString() + "). Please reach out to your Support Team for further assistance.";
                        return false;
                    }
                    // In case of close side trades (Sell, Buy to Close, Sell to Close) where the Trade Date is after the NAV lock date & original purchase date is before the NAV lock date then system need to block these trades
                    else if ((finalTarget.SideTagValue.Equals(FIXConstants.SIDE_Sell) || finalTarget.SideTagValue.Equals(FIXConstants.SIDE_Buy_Closed)
                        || finalTarget.SideTagValue.Equals(FIXConstants.SIDE_Sell_Closed)) && !NAVLockDateRule.ValidateNAVLockDate(finalTarget.OriginalPurchaseDate))
                    {
                        e.Description = "The date you’ve chosen for this action precedes your NAV Lock date (" + NAVLockDateRule.NAVLockDate.Value.ToShortDateString() + "). Please reach out to your Support Team for further assistance.";
                        return false;
                    }
                    else
                    {
                        SetOrRemoveValidationError(finalTarget, "Date required", false);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            public static bool AccountIDCheck(object target, RuleArgs e)
            {
                PositionMaster finalTarget = target as PositionMaster;
                if (finalTarget != null)
                {
                    CheckValidationForAllFields(finalTarget);

                    if (finalTarget.AccountID <= 0)
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.MissingData.ToString();
                        e.Description = "Account Name not validated";
                        SetOrRemoveValidationError(finalTarget, "Account Name not validated", true);
                        return false;
                    }
                    else
                    {
                        SetOrRemoveValidationError(finalTarget, "Account Name not validated", false);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            //  Checking security is already aprroved or not
            public static bool IsSecurityApprovedCheck(object target, RuleArgs e)
            {
                PositionMaster finalTarget = target as PositionMaster;
                Boolean isApproved = false;
                if (finalTarget != null)
                {
                    CheckValidationForAllFields(finalTarget);

                    if (finalTarget.SecApprovalStatus.Equals(UNAPPROVED) && finalTarget.AUECID > 0)
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.UnApproved.ToString();
                        e.Description = "Security not Approved";
                        SetOrRemoveValidationError(finalTarget, "Security not Approved", true);
                        isApproved = false;
                    }
                    else
                    {
                        SetOrRemoveValidationError(finalTarget, "Security not Approved", false);
                        isApproved = true;
                    }
                }
                return isApproved;
            }

            public static bool CounterPartyIDCheck(object target, RuleArgs e)
            {
                PositionMaster finalTarget = target as PositionMaster;
                if (finalTarget != null)
                {
                    CheckValidationForAllFields(finalTarget);

                    if (finalTarget.CounterPartyID < 0)
                    {
                        finalTarget.ValidationStatus = ApplicationConstants.ValidationStatus.MissingData.ToString();
                        e.Description = "Broker required";
                        SetOrRemoveValidationError(finalTarget, "Broker required", true);
                        return false;
                    }
                    else
                    {
                        SetOrRemoveValidationError(finalTarget, "Broker required", false);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Sets or removes validation error in the 
            /// </summary>
            /// <param name="positionMaster"></param>
            /// <param name="message"></param>
            /// <param name="isAddOrRemoveErrorMessage"></param>
            private static void SetOrRemoveValidationError(PositionMaster positionMaster, string message, bool isAddOrRemoveErrorMessage)
            {
                StringBuilder errorMessage = new StringBuilder(positionMaster.ValidationError);
                if (isAddOrRemoveErrorMessage)
                {
                    if (!errorMessage.ToString().Contains(message))
                    {
                        errorMessage.Append(message + Seperators.SEPERATOR_8);
                    }
                }
                else
                {
                    errorMessage.Replace(message + Seperators.SEPERATOR_8, string.Empty);
                }
                positionMaster.ValidationError = errorMessage.ToString();
            }
        }
    }
}

