using Csla;
using Csla.Validation;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using System;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    [Serializable, System.Runtime.InteropServices.ComVisible(false)]
    public class SecMasterUpdateDataByImportUI : BusinessBase<SecMasterUpdateDataByImportUI>
    {

        public SecMasterUpdateDataByImportUI()
        {

        }

        public const string VALID = "Validated";

        public const string INVALID = "NotValidated";

        string _underLyingSymbol = string.Empty;
        int _auecID = int.MinValue;
        int _assetID = int.MinValue;
        int _underlyingID = int.MinValue;
        int _exchangeID = int.MinValue;
        int _currencyID = int.MinValue;

        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }

        }
        public int UnderLyingID
        {
            get { return _underlyingID; }
            set { _underlyingID = value; }

        }
        [Browsable(false)]
        public int AUECID
        {
            get
            {
                return _auecID;

            }
            set
            {
                _auecID = value;
                PropertyHasChanged("AUECID");
            }

        }
        public int ExchangeID
        {
            get { return _exchangeID; }
            set { _exchangeID = value; }

        }
        public int CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }

        }



        private string _ticker = string.Empty;

        public string TickerSymbol
        {
            get
            {
                return _ticker;
            }
            set
            {
                _ticker = value;
                PropertyHasChanged("TickerSymbol");
            }
        }
        private string _reuters = string.Empty;

        public string ReutersSymbol
        {
            get { return _reuters; }
            set { _reuters = value; }
        }

        private string _bloomberg = string.Empty;

        public string BloombergSymbol
        {
            get { return _bloomberg; }
            set { _bloomberg = value; }
        }
        private string _isin = string.Empty;

        public string ISINSymbol
        {
            get { return _isin; }
            set { _isin = value; }
        }
        private string _sedol = string.Empty;

        public string SedolSymbol
        {
            get { return _sedol; }
            set { _sedol = value; }
        }

        private string _cusip = string.Empty;

        public string CusipSymbol
        {
            get { return _cusip; }
            set { _cusip = value; }
        }

        private string _osiOptionSymbol = string.Empty;

        public string OSIOptionSymbol
        {
            get { return _osiOptionSymbol; }
            set { _osiOptionSymbol = value; }
        }

        private string _idcoOptionSymbol = string.Empty;

        public string IDCOOptionSymbol
        {
            get { return _idcoOptionSymbol; }
            set { _idcoOptionSymbol = value; }
        }

        private string _opraOptionSymbol = string.Empty;

        public string OPRAOptionSymbol
        {
            get { return _opraOptionSymbol; }
            set { _opraOptionSymbol = value; }
        }


        private string _longName;

        public string LongName
        {
            get { return _longName; }
            set { _longName = value; }
        }
        private string _sector;

        public string Sector
        {
            get { return _sector; }
            set { _sector = value; }
        }

        public string UnderLyingSymbol
        {
            get { return _underLyingSymbol; }
            set { _underLyingSymbol = value; }

        }

        /// <summary>
        /// 2 represents None in enum OptionType (D:\NirvanaOMS\SourceCode\Dev\Prana_CA\Application\Prana.BusinessObjects\Classes\Enums.cs)
        /// </summary>
        private int _putOrCall = 2;

        public int PutOrCall
        {
            get { return _putOrCall; }
            set { _putOrCall = value; }
        }

        private double _strikePrice = 0.0;

        public double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }
        double _multiplier = 1;
        public double Multiplier
        {
            get { return _multiplier; }
            set { _multiplier = value; }
        }
        [Browsable(false)]
        public int MaturityMonth
        {
            get
            {
                string s = string.Empty;
                if (_expirationDate.Month.ToString().Length == 1)
                {
                    s = '0' + _expirationDate.Month.ToString();
                }
                else
                {
                    s = _expirationDate.Month.ToString();
                }
                return Convert.ToInt32(_expirationDate.Year.ToString() + s);
            }

            // set { _expirationOrSettlementDate.Month.ToString()+_expirationOrSettlementDate.Year.ToString() = value; }

        }

        [Browsable(false)]
        public int MaturityDay
        {
            get
            {
                return _expirationDate.Day;
            }

            // set { _expirationOrSettlementDate.Month.ToString()+_expirationOrSettlementDate.Year.ToString() = value; }

        }

        private DateTime _expirationDate = DateTimeConstants.MinValue;

        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }

        //[Browsable(false)]
        //public int SettleMentDate
        //{
        //    get { return Convert.ToInt32(_expirationOrSettlementDate.Date.Day); }
        //    //set { _expirationOrSettlementDate.Date = value; }
        //}

        //private int _expirationDate = 0;
        //[Browsable(false)]
        //public int Expirationdate
        //{
        //    get { return _expirationOrSettlementDate.Date.Day; }
        //    //set { _expirationDate = value; }
        //}
        private SymbolType _symbolType = SymbolType.Unchanged;
        [Browsable(false)]
        public SymbolType SymbolType
        {
            get { return _symbolType; }
            set { _symbolType = value; }
        }
        private Int64 _symbol_PK;
        [Browsable(false)]
        public Int64 Symbol_PK
        {
            get { return _symbol_PK; }
            set { _symbol_PK = value; }
        }
        private float _delta = 1;

        public float Delta
        {
            get { return _delta; }
            set { _delta = value; }
        }

        private int _leadCurrencyID = int.MinValue;

        public int LeadCurrencyID
        {
            get { return _leadCurrencyID; }
            set { _leadCurrencyID = value; }
        }
        private int _vsCurrencyID = int.MinValue;

        public int VsCurrencyID
        {
            get { return _vsCurrencyID; }
            set { _vsCurrencyID = value; }
        }

        private int _securityTypeID;

        public int SecurityTypeID
        {
            get { return _securityTypeID; }
            set { _securityTypeID = value; }
        }

        private int _accrualBasisID;

        public int AccrualBasisID
        {
            get { return _accrualBasisID; }
            set { _accrualBasisID = value; }
        }

        private int _couponFrequencyID;

        public int CouponFrequencyID
        {
            get { return _couponFrequencyID; }
            set { _couponFrequencyID = value; }
        }

        private DateTime _firstCouponDate = DateTimeConstants.MinValue;
        public DateTime FirstCouponDate
        {
            get { return _firstCouponDate; }
            set { _firstCouponDate = value; }
        }
        private double _coupon = 0.0;
        public double Coupon
        {
            get { return _coupon; }
            set { _coupon = value; }
        }

        private bool _isZero = false;

        // IsZero - set true if zero coupon bond, can also set coupon above to 0
        public bool IsZero
        {
            get { return _isZero; }
            set { _isZero = value; }
        }
        private DateTime _dateIssue = DateTimeConstants.MinValue;

        // IssueDate - issue date of the security
        public DateTime IssueDate
        {
            get { return _dateIssue; }
            set { _dateIssue = value; }
        }
        private string _countryCode;
        // CountryCode - 2 digit country code, such as Thomson Reuters codes, blank defaults to US
        public string CountryCode
        {
            get { return _countryCode; }
            set { _countryCode = value; }
        }

        private int _daysToSettlement = 1;

        public int DaysToSettlement
        {
            get { return _daysToSettlement; }
            set { _daysToSettlement = value; }
        }

        private string _cutOffTime;

        public string CutOffTime
        {
            get { return _cutOffTime; }
            set { _cutOffTime = value; }
        }

        private int _UDAAssetClassID;

        public int UDAAssetClassID
        {
            get { return _UDAAssetClassID; }
            set { _UDAAssetClassID = value; }
        }

        private int _UDASecurityTypeID;

        public int UDASecurityTypeID
        {
            get { return _UDASecurityTypeID; }
            set { _UDASecurityTypeID = value; }
        }

        private int _UDASectorID;

        public int UDASectorID
        {
            get { return _UDASectorID; }
            set { _UDASectorID = value; }
        }

        private int _UDASubSectorID;

        public int UDASubSectorID
        {
            get { return _UDASubSectorID; }
            set { _UDASubSectorID = value; }
        }

        private int _UDACountryID;

        public int UDACountryID
        {
            get { return _UDACountryID; }
            set { _UDACountryID = value; }
        }

        private string _UDAAssetClass;

        public string UDAAssetClass
        {
            get { return _UDAAssetClass; }
            set { _UDAAssetClass = value; }
        }

        private string _UDASecurityType;

        public string UDASecurityType
        {
            get { return _UDASecurityType; }
            set { _UDASecurityType = value; }
        }

        private string _UDASector;

        public string UDASector
        {
            get { return _UDASector; }
            set { _UDASector = value; }
        }

        private string _UDASubSector;

        public string UDASubSector
        {
            get { return _UDASubSector; }
            set { _UDASubSector = value; }
        }

        private string _UDACountry;

        public string UDACountry
        {
            get { return _UDACountry; }
            set { _UDACountry = value; }
        }

        private string _validated = INVALID;
        public string Validated
        {
            get { return _validated; }
            set
            {
                _validated = value;
            }
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
            return _ticker;
        }

        string _existingUnderlyingSymbol;
        public string ExistingUnderlyingSymbol
        {
            get
            {
                return _existingUnderlyingSymbol;
            }
            set
            {
                _existingUnderlyingSymbol = value;
            }
        }

        string _existinglongName;
        public string ExistingLongName
        {
            get
            {
                return _existinglongName;
            }
            set
            {
                _existinglongName = value;
            }
        }

        double _existingMultiplier;
        public double ExistingMultiplier
        {
            get
            {
                return _existingMultiplier;
            }
            set
            {
                _existingMultiplier = value;
            }
        }

        /// <summary>
        /// Dictionary to store dynamic UDA
        /// </summary>
        private SerializableDictionary<String, Object> _dynamicUDA = new SerializableDictionary<string, object>();
        public SerializableDictionary<String, Object> DynamicUDA
        {
            get { return _dynamicUDA; }
            set
            {
                _dynamicUDA = value;
            }
        }

        private string pbSymbol;

        public string PBSymbol
        {
            get { return pbSymbol; }
            set { pbSymbol = value; }
        }

        private decimal _roundLot = 1;

        public decimal RoundLot
        {
            get { return _roundLot; }
            set { _roundLot = value; }
        }

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
            set
            {
                _validationError = value;
            }
        }


        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(CustomRules.SymbolCheck, "TickerSymbol");
            ValidationRules.AddRule(CustomRules.AUECIDCheck, "AUECID");
        }

        [System.Runtime.InteropServices.ComVisible(false)]
        public class CustomRules : RuleArgs
        {
            public CustomRules(string validation)
                : base(validation)
            {
            }

            public static bool SymbolCheck(object target, RuleArgs e)
            {
                SecMasterUpdateDataByImportUI finalTarget = target as SecMasterUpdateDataByImportUI;
                if (finalTarget != null)
                {
                    if (
                        string.IsNullOrEmpty(finalTarget.TickerSymbol) ||
                         finalTarget.AUECID <= 0 || finalTarget.AUECID == int.MinValue)
                    {
                        finalTarget.Validated = INVALID;
                    }
                    else
                    {
                        finalTarget.Validated = VALID;
                    }

                    if (string.IsNullOrEmpty(finalTarget.TickerSymbol))
                    {
                        e.Description = "Symbol not found in SM";
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

            public static bool AUECIDCheck(object target, RuleArgs e)
            {
                SecMasterUpdateDataByImportUI finalTarget = target as SecMasterUpdateDataByImportUI;
                if (finalTarget != null)
                {
                    if (string.IsNullOrEmpty(finalTarget.TickerSymbol) ||
                        finalTarget.AUECID <= 0 || finalTarget.AUECID == int.MinValue)
                    {
                        finalTarget.Validated = INVALID;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(finalTarget.ValidationError))
                        {
                            finalTarget.Validated = INVALID;
                        }
                        else
                        {
                            finalTarget.Validated = VALID;
                        }
                    }

                    if (finalTarget.AUECID <= 0 || finalTarget.AUECID == int.MinValue)
                    {
                        e.Description = "Symbol not found in SM";
                        return false;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(finalTarget.ValidationError))
                        {
                            e.Description = finalTarget.ValidationError;
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }

        }

    }
}

