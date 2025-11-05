using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
//using Prana.CommonDataCache;
namespace Prana.BusinessObjects.SecurityMasterBusinessObjects
{
    [Serializable]
    public class SecMasterUIObj
    {
        //SecMasterCoreObject _SecMasterCoreObj=null ;
        string _underLyingSymbol = string.Empty;
        int _auecID = int.MinValue;
        int _assetID = int.MinValue;
        int _underlyingID = int.MinValue;
        int _exchangeID = int.MinValue;
        int _currencyID = int.MinValue;
        private List<string> _symbologyMapping = new List<string>();

        public virtual void FillData(object[] row, int offset, DateTime dateTime)
        {
            int AssetID = 0;

            int UnderLyingSymbol = 1;
            int AUECID = 2;
            int UnderLyingID = 3;
            int ExchangeID = 4;
            int CurrencyID = 5;
            int LongName = Prana.Global.ApplicationConstants.SymbologyCodesCount + 6;
            int Sector = Prana.Global.ApplicationConstants.SymbologyCodesCount + 7;

            if (row[AssetID] != System.DBNull.Value)
            {
                _assetID = int.Parse(row[AssetID].ToString());
            }

            if (row[UnderLyingSymbol] != System.DBNull.Value)
            {
                _underLyingSymbol = row[UnderLyingSymbol].ToString();
            }
            if (row[AUECID] != System.DBNull.Value)
            {
                _auecID = int.Parse(row[AUECID].ToString());
            }
            if (row[UnderLyingID] != System.DBNull.Value)
            {
                _underlyingID = int.Parse(row[UnderLyingID].ToString());
            }
            if (row[ExchangeID] != System.DBNull.Value)
            {
                _exchangeID = int.Parse(row[ExchangeID].ToString());
            }
            if (row[CurrencyID] != System.DBNull.Value)
            {
                _currencyID = int.Parse(row[CurrencyID].ToString());
            }

            for (int count = 6; count < (Prana.Global.ApplicationConstants.SymbologyCodesCount + 6); count++)
            {
                if (row[count] != null)
                    _symbologyMapping.Add(row[count].ToString().ToUpper());
                else
                    _symbologyMapping.Add(string.Empty);
            }

            _longName = row[LongName].ToString();
            _sector = row[Sector].ToString();

        }


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
            get { return _auecID; }
            set { _auecID = value; }

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

        private string _proxySymbol = string.Empty;

        public string ProxySymbol
        {
            get { return _proxySymbol; }
            set { _proxySymbol = value; }
        }

        private string _ticker = string.Empty;

        public string TickerSymbol
        {
            get { return _ticker; }
            set { _ticker = value; }
        }

        private string _factSetSymbol = string.Empty;
        public string FactSetSymbol
        {
            get { return _factSetSymbol; }
            set { _factSetSymbol = value; }

        }

        private string _activSymbol = string.Empty;
        public string ActivSymbol
        {
            get { return _activSymbol; }
            set { _activSymbol = value; }
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
        private string _bloombergSymbolWithExchangeCode = string.Empty;

        public string BloombergSymbolWithExchangeCode
        {
            get { return _bloombergSymbolWithExchangeCode; }
            set { _bloombergSymbolWithExchangeCode = value; }
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


        private string _longName = string.Empty;

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



        private int _bondTypeID;

        public int BondTypeID
        {
            get { return _bondTypeID; }
            set { _bondTypeID = value; }
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

        //private DateTime _dateMaturity = DateTimeConstants.MinValue;
        //// MaturityDate - security maturity date
        //public DateTime MaturityDate
        //{
        //    get { return _dateMaturity; }
        //    set { _dateMaturity = value; }
        //}

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

        private int _collateralTypeID;
        public int CollateralTypeID
        {
            get { return _collateralTypeID; }
            set { _collateralTypeID = value; }
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

        private DateTime _creationDate;


        public DateTime CreationDate
        {
            get { return _creationDate; }
            set { _creationDate = value; }
        }
        private DateTime _modifiedDate;


        public DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }

        private bool _isNDF;
        public bool IsNDF
        {
            get { return _isNDF; }
            set { _isNDF = value; }
        }

        private DateTime _fixingDate = DateTimeConstants.MinValue;
        public DateTime FixingDate
        {
            get { return _fixingDate; }
            set { _fixingDate = value; }
        }

        private decimal _roundLot = 1;

        public decimal RoundLot
        {
            get { return _roundLot; }
            set { _roundLot = value; }
        }

        //Narendra Kumar Jangir
        //May 23 2014
        //We are addign RequestedSymbology and RequestedSymbol because we need these fields in CH symbol management UI.
        private int _requestedSymbology;
        public int RequestedSymbology
        {
            get { return _requestedSymbology; }
            set { _requestedSymbology = value; }
        }

        private string _requestedSymbol;

        public string RequestedSymbol
        {
            get { return _requestedSymbol; }
            set { _requestedSymbol = value; }
        }

        private Prana.BusinessObjects.SecMasterConstants.SecMasterSourceOfData _dataSource = SecMasterConstants.SecMasterSourceOfData.None;
        public Prana.BusinessObjects.SecMasterConstants.SecMasterSourceOfData DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }

        #region Approved status related fields
        private Boolean _isSecApproved = false;

        public Boolean IsSecApproved
        {
            get { return _isSecApproved; }
            set
            {
                _isSecApproved = value;

                if (_isSecApproved)
                    _secApprovalStatus = ApplicationConstants.CONST_APPROVED;
                else
                    _secApprovalStatus = ApplicationConstants.CONST_UN_APPROVED;
            }
        }
        private string _secApprovalStatus;

        public string SecApprovalStatus
        {
            get { return _secApprovalStatus; }
            set { _secApprovalStatus = value; }
        }

        //modified by omshiv, Now _approvedBy value will be UserName_clietName
        private string _approvedBy;

        public string ApprovedBy
        {
            get { return _approvedBy; }
            set { _approvedBy = value; }
        }

        private String _createdBy = String.Empty;
        public String CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        private String _modifiedBy = String.Empty;
        public String ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        private String _BBGID = String.Empty;
        public String BBGID
        {
            get { return _BBGID; }
            set { _BBGID = value; }
        }

        private int _primarySymbology;
        public int PrimarySymbology
        {
            get { return _primarySymbology; }
            set { _primarySymbology = value; }
        }

        private DateTime _approvalDate = DateTimeConstants.MinValue;
        public DateTime ApprovalDate
        {
            get { return _approvalDate; }
            set { _approvalDate = value; }
        }

        private string _comments = "";
        public string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        private double _strikePriceMultiplier = 1;
        public double StrikePriceMultiplier
        {
            get { return _strikePriceMultiplier; }
            set { _strikePriceMultiplier = value; }
        }

        private double _sharesOutstanding = 0;
        public double SharesOutstanding
        {
            get { return _sharesOutstanding; }
            set { _sharesOutstanding = value; }
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
        #endregion


        #region UDA DATA related fields


        private int _udaAssetClassID = int.MinValue;
        private int _udaSecurityTypeID = int.MinValue;
        private int _udaSubSectorID = int.MinValue;
        private int _udaSectorID = int.MinValue;
        private int _udaCountryID = int.MinValue;


        public int UDAAssetClassID
        {
            get { return _udaAssetClassID; }
            set { _udaAssetClassID = value; }
        }
        public int UDASecurityTypeID
        {
            get { return _udaSecurityTypeID; }
            set { _udaSecurityTypeID = value; }
        }
        public int UDASectorID
        {
            get { return _udaSectorID; }
            set { _udaSectorID = value; }
        }
        public int UDASubSectorID
        {
            get { return _udaSubSectorID; }
            set { _udaSubSectorID = value; }
        }
        public int UDACountryID
        {
            get { return _udaCountryID; }
            set { _udaCountryID = value; }
        }

        #endregion
        //  cehck for setting default UDA from underlyig symbol or root symbol or not
        private bool _useUDAFromUnderlyingOrRoot = false;
        public bool UseUDAFromUnderlyingOrRoot
        {
            get { return _useUDAFromUnderlyingOrRoot; }
            set { _useUDAFromUnderlyingOrRoot = value; }
        }

        /// <summary>
        /// Dictionary to store dynamic UDA
        /// </summary>
        private SerializableDictionary<String, Object> _dynamicUDA = new SerializableDictionary<string, object>();
        public SerializableDictionary<String, Object> DynamicUDA
        {
            get { return _dynamicUDA; }
            set { _dynamicUDA = value; }
        }

        private bool _isCurrencyFuture = false;
        public bool IsCurrencyFuture
        {
            get { return _isCurrencyFuture; }
            set { _isCurrencyFuture = value; }
        }
    }
}
