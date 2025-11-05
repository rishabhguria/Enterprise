using Csla;
using Csla.Validation;
using Prana.BusinessObjects.AppConstants;
using System;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    [Serializable, System.Runtime.InteropServices.ComVisible(false)]
    public class UserOptModelInput : BusinessBase<UserOptModelInput>
    {
        public UserOptModelInput()
        {

        }
        public UserOptModelInput(string symbol)
        {
            _symbol = symbol;
        }

        public const string VALID = "Validated";
        public const string INVALID = "NotValidated";

        private double _historicalVol = 0;
        [Browsable(false)]
        public double HistoricalVol
        {
            get { return _historicalVol; }
            set { _historicalVol = value; }
        }

        private bool _historicalVolUsed;
        [Browsable(false)]
        public bool HistoricalVolUsed
        {
            get { return _historicalVolUsed; }
            set { _historicalVolUsed = value; }
        }

        private double _volatility = 0;
        public double Volatility
        {
            get { return _volatility; }
            set
            {
                _volatility = value;
                PropertyHasChanged("Volatility");
            }
        }

        private bool _volatilityUsed;
        public bool VolatilityUsed
        {
            get { return _volatilityUsed; }
            set { _volatilityUsed = value; }
        }

        private double _intRate = 0;
        public double IntRate
        {
            get { return _intRate; }
            set
            {
                _intRate = value;
                PropertyHasChanged("IntRate");
            }
        }

        private bool _intRateUsed;
        public bool IntRateUsed
        {
            get { return _intRateUsed; }
            set { _intRateUsed = value; }
        }

        private bool _isHistorical = false;
        public bool IsHistorical
        {
            get { return _isHistorical; }
            set { _isHistorical = value; }
        }

        private double _dividend = 0;
        /// <summary>
        /// This is dividend yield and not the absolute value of dividend.
        /// </summary>
        public double Dividend
        {
            get { return _dividend; }
            set
            {
                _dividend = value;
                PropertyHasChanged("Dividend");
            }
        }

        private bool _dividendUsed;
        public bool DividendUsed
        {
            get { return _dividendUsed; }
            set { _dividendUsed = value; }
        }

        /// <summary>
        /// The stock borrow cost
        /// </summary>
        private double _stockBorrowCost = 0;

        /// <summary>
        /// Gets or sets the stock borrow cost.
        /// </summary>
        /// <value>
        /// The stock borrow cost.
        /// </value>
        public double StockBorrowCost
        {
            get { return _stockBorrowCost; }
            set
            {
                _stockBorrowCost = value;
                PropertyHasChanged("StockBorrowCost");
            }
        }

        /// <summary>
        /// The stock borrow cost used
        /// </summary>
        private bool _stockBorrowCostUsed;
        public bool StockBorrowCostUsed
        {
            get { return _stockBorrowCostUsed; }
            set { _stockBorrowCostUsed = value; }
        }

        private double _delta = 0;
        public double Delta
        {
            get { return _delta; }
            set
            {
                _delta = value;
                PropertyHasChanged("Delta");
            }
        }

        private bool _deltaUsed;
        public bool DeltaUsed
        {
            get { return _deltaUsed; }
            set { _deltaUsed = value; }
        }

        private double _lastPrice = 0;
        /// <summary>
        /// This is UserPrice and not the lastprice
        /// </summary>
        public double LastPrice
        {
            get { return _lastPrice; }
            set
            {
                _lastPrice = value;
                PropertyHasChanged("LastPrice");
            }
        }

        private bool _lastPriceUsed;
        /// <summary>
        /// This is IsUserPriceUsed and not the LastPriceUsed
        /// </summary>
        public bool LastPriceUsed
        {
            get { return _lastPriceUsed; }
            set { _lastPriceUsed = value; }
        }

        private double _forwardPoints = 0;
        public double ForwardPoints
        {
            get { return _forwardPoints; }
            set
            {
                _forwardPoints = value;
                PropertyHasChanged("ForwardPoints");
            }
        }

        private bool _theoreticalPriceUsed;
        public bool TheoreticalPriceUsed
        {
            get { return _theoreticalPriceUsed; }
            set { _theoreticalPriceUsed = value; }
        }

        /// <summary>
        /// Gets or sets SMSharesOutstanding.
        /// </summary>
        /// <value>
        /// The SMSharesOutstanding.
        /// </value>
        private double _smSharesOutstanding = 0;
        public double SMSharesOutstanding
        {
            get { return _smSharesOutstanding; }
            set
            {
                _smSharesOutstanding = value;
                PropertyHasChanged("SMSharesOutstanding");
            }
        }

        /// <summary>
        /// Gets or sets SMSharesOutstandingUsed.
        /// </summary>
        /// <value>
        /// The SMSharesOutstandingUsed.
        /// </value>
        private bool _smSharesOutstandingUsed;
        public bool SMSharesOutstandingUsed
        {
            get { return _smSharesOutstandingUsed; }
            set { _smSharesOutstandingUsed = value; }
        }

        private double _sharesOutstanding;
        public double SharesOutstanding
        {
            get { return _sharesOutstanding; }
            set
            {
                _sharesOutstanding = value;
                PropertyHasChanged("SharesOutstanding");
            }
        }

        private bool _sharesOutstandingUsed;
        public bool SharesOutstandingUsed
        {
            get { return _sharesOutstandingUsed; }
            set { _sharesOutstandingUsed = value; }
        }

        private bool _proxySymbolUsed;
        public bool ProxySymbolUsed
        {
            get { return _proxySymbolUsed; }
            set
            {
                _proxySymbolUsed = value;
                PropertyHasChanged("ProxySymbolUsed");
            }
        }

        private string _proxySymbol;
        public string ProxySymbol
        {
            get { return _proxySymbol; }
            set
            {
                _proxySymbol = value;
            }
        }

        private bool _forwardPointsUsed;
        public bool ForwardPointsUsed
        {
            get { return _forwardPointsUsed; }
            set { _forwardPointsUsed = value; }
        }




        private int _auecID;
        [Browsable(false)]

        public int AuecID
        {
            get { return _auecID; }
            set
            {
                _auecID = value;
                PropertyHasChanged("AuecID");
            }
        }
        private int _assetID = -1;

        [Browsable(false)]
        public int AssetID
        {
            get
            {
                return _assetID;
            }
            set
            {
                _assetID = value;
            }
        }

        private string _symbol;
        public string Symbol
        {
            get { return _symbol; }
            set
            {
                _symbol = value;
                PropertyHasChanged("Symbol");
            }
        }

        private string _underlyingSymbol;
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
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

        private string _validationError = string.Empty;
        public string ValidationError
        {
            get { return _validationError; }
            set
            {
                _validationError = value;
            }
        }

        /// <summary>
        /// will act as a key to the user model input.
        /// </summary>
        private int _rowIndex;
        [Browsable(false)]
        public int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }

        /// <summary>
        /// Sets the import status of the user model input object
        /// </summary>
        private string _importStatus = Prana.BusinessObjects.AppConstants.ImportStatus.None.ToString();
        public string ImportStatus
        {
            get { return _importStatus; }
            set { _importStatus = value; }
        }

        private string _cusip = string.Empty;
        [Browsable(false)]
        public string CUSIP
        {
            get { return _cusip; }
            set
            {
                _cusip = value;
            }
        }

        private string _sedol = string.Empty;
        [Browsable(false)]
        public string SEDOL
        {
            get { return _sedol; }
            set
            {
                _sedol = value;
            }
        }

        private string _isin = string.Empty;
        [Browsable(false)]
        public string ISIN
        {
            get { return _isin; }
            set
            {
                _isin = value;
            }
        }

        private string _ric = string.Empty;
        [Browsable(false)]
        public string RIC
        {
            get { return _ric; }
            set
            {
                _ric = value;
            }
        }

        private string _bloomberg = string.Empty;
        public string Bloomberg
        {
            get { return _bloomberg; }
            set
            {
                _bloomberg = value;
            }
        }

        private string _osiOptionSymbol;
        [Browsable(false)]
        public string OSIOptionSymbol
        {
            get { return _osiOptionSymbol; }
            set { _osiOptionSymbol = value; }
        }

        private string _idcoOptionSymbol;
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

        private string _pBSymbol = string.Empty;
        public string PBSymbol
        {
            get { return _pBSymbol; }
            set
            {
                _pBSymbol = value;
            }
        }

        private string _pSSymbol = string.Empty;
        public string PSSymbol
        {
            get { return _pSSymbol; }
            set
            {
                _pSSymbol = value;
            }
        }

        private string _securityDescription = string.Empty;
        public string SecurityDescription
        {
            get { return _securityDescription; }
            set
            {
                _securityDescription = value;
            }
        }

        [Browsable(false)]
        private int _vsCurrencyID;
        public int VsCurrencyID
        {
            get { return _vsCurrencyID; }
            set { _vsCurrencyID = value; }
        }

        [Browsable(false)]
        private int _leadCurrencyID;
        public int LeadCurrencyID
        {
            get { return _leadCurrencyID; }
            set { _leadCurrencyID = value; }
        }

        [Browsable(false)]
        private double _strikePrice = double.MinValue;
        /// <summary>
        /// Strike price for the options symbol
        /// </summary>
        /// 
        public double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }

        [Browsable(false)]
        private OptionType _putOrCall = OptionType.NONE;
        /// <summary>
        /// Same int as used in Prana.BusinessObjects.FIXConstants class
        /// </summary>
        public OptionType PutorCall
        {
            get { return _putOrCall; }
            set { _putOrCall = value; }
        }

        [Browsable(false)]
        private DateTime _expirationDate = DateTime.MinValue;
        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }

        [Browsable(false)]
        public override bool IsValid
        {
            get { return base.IsValid; }
        }
        [Browsable(false)]
        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

        [Browsable(false)]
        private bool _isDirtyToSave = false;
        public bool IsDirtyToSave
        {
            get { return _isDirtyToSave; }
            set { _isDirtyToSave = value; }
        }

        private bool _closingMarkUsed;
        public bool ClosingMarkUsed
        {
            get { return _closingMarkUsed; }
            set { _closingMarkUsed = value; }
        }


        private bool _manualInput;
        public bool ManualInput
        {
            get { return _manualInput; }
            set { _manualInput = value; }
        }

        private string _symbology;
        public string Symbology
        {
            get { return _symbology; }
            set { _symbology = value; }
        }

        private Prana.Global.ApplicationConstants.PersistenceStatus _persistenceStatus;
        public Prana.Global.ApplicationConstants.PersistenceStatus PersistenceStatus
        {
            get { return _persistenceStatus; }
            set { _persistenceStatus = value; }
        }
        private string _bloombergSymbolWithExchangeCode = string.Empty;
        public string BloombergSymbolWithExchangeCode
        {
            get { return _bloombergSymbolWithExchangeCode; }
            set{_bloombergSymbolWithExchangeCode = value;}
        }

        protected override object GetIdValue()
        {
            return _symbol;
        }
        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(CustomRules.SymbolCheck, "Symbol");
            ValidationRules.AddRule(CustomRules.AUECIDCheck, "AuecID");
            // ValidationRules.AddRule(CustomRules.VolatilityCheck, "Volatility");
            //ValidationRules.AddRule(CustomRules.LastPriceCheck, "LastPrice");
            //ValidationRules.AddRule(CustomRules.InterestRateCheck, "IntRate");
            //ValidationRules.AddRule(CustomRules.DividendCheck, "Dividend");
        }

        [System.Runtime.InteropServices.ComVisible(false)]
        public class CustomRules : RuleArgs
        {
            public CustomRules(string validation)
                : base(validation)
            {
            }

            public static bool AUECIDCheck(object target, RuleArgs e)
            {
                UserOptModelInput finalTarget = target as UserOptModelInput;
                if (finalTarget != null)
                {
                    if (
                        string.IsNullOrEmpty(finalTarget.Symbol) ||
                        finalTarget.AuecID <= 0 //||
                                                //finalTarget.Volatility < 0 ||
                                                //finalTarget.Dividend < 0 ||
                                                //finalTarget.IntRate < 0 ||
                                                //finalTarget.LastPrice < 0
                        )
                    {
                        finalTarget.Validated = INVALID;
                    }
                    else
                    {
                        finalTarget.Validated = VALID;
                    }

                    if (finalTarget.AuecID <= 0)
                    {
                        e.Description = "Symbol not validated";
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
            public static bool VolatilityCheck(object target, RuleArgs e)
            {
                UserOptModelInput finalTarget = target as UserOptModelInput;
                if (finalTarget != null)
                {
                    if (
                       string.IsNullOrEmpty(finalTarget.Symbol) ||
                        finalTarget.AuecID <= 0 ||
                        finalTarget.Volatility < 0 ||
                        finalTarget.Dividend < 0 ||
                        finalTarget.StockBorrowCost < 0 ||
                        finalTarget.IntRate < 0 ||
                        finalTarget.LastPrice < 0
                        )
                    {
                        finalTarget.Validated = INVALID;
                    }
                    else
                    {
                        finalTarget.Validated = VALID;
                    }
                    if (finalTarget.Volatility < 0)
                    {
                        e.Description = "Negative Value not allowed";
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
            public static bool LastPriceCheck(object target, RuleArgs e)
            {
                UserOptModelInput finalTarget = target as UserOptModelInput;
                if (finalTarget != null)
                {

                    if (
                       string.IsNullOrEmpty(finalTarget.Symbol) ||
                        finalTarget.AuecID <= 0 ||
                        finalTarget.Volatility < 0 ||
                        finalTarget.Dividend < 0 ||
                        finalTarget.StockBorrowCost < 0 ||
                        finalTarget.IntRate < 0 ||
                        finalTarget.LastPrice < 0
                        )
                    {
                        finalTarget.Validated = INVALID;
                    }
                    else
                    {
                        finalTarget.Validated = VALID;
                    }

                    if (finalTarget.LastPrice < 0)
                    {
                        e.Description = "Negative Value not allowed";
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
            public static bool DividendCheck(object target, RuleArgs e)
            {
                UserOptModelInput finalTarget = target as UserOptModelInput;
                if (finalTarget != null)
                {
                    if (
                       string.IsNullOrEmpty(finalTarget.Symbol) ||
                        finalTarget.AuecID <= 0 ||
                        finalTarget.Volatility < 0 ||
                        finalTarget.Dividend < 0 ||
                        finalTarget.StockBorrowCost < 0 ||
                        finalTarget.IntRate < 0 ||
                        finalTarget.LastPrice < 0
                        )
                    {
                        finalTarget.Validated = INVALID;
                    }
                    else
                    {
                        finalTarget.Validated = VALID;
                    }
                    if (finalTarget.Dividend < 0)
                    {
                        e.Description = "Negative Value not allowed";
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
            public static bool StockBorrowCostCheck(object target, RuleArgs e)
            {
                UserOptModelInput finalTarget = target as UserOptModelInput;
                if (finalTarget != null)
                {
                    if (
                       string.IsNullOrEmpty(finalTarget.Symbol) ||
                        finalTarget.AuecID <= 0 ||
                        finalTarget.Volatility < 0 ||
                        finalTarget.Dividend < 0 ||
                        finalTarget.StockBorrowCost < 0 ||
                        finalTarget.IntRate < 0 ||
                        finalTarget.LastPrice < 0
                        )
                    {
                        finalTarget.Validated = INVALID;
                    }
                    else
                    {
                        finalTarget.Validated = VALID;
                    }
                    if (finalTarget.StockBorrowCost < 0)
                    {
                        e.Description = "Negative Value not allowed";
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
            public static bool InterestRateCheck(object target, RuleArgs e)
            {
                UserOptModelInput finalTarget = target as UserOptModelInput;
                if (finalTarget != null)
                {
                    if (
                        string.IsNullOrEmpty(finalTarget.Symbol) ||
                         finalTarget.AuecID <= 0 ||
                         finalTarget.Volatility < 0 ||
                         finalTarget.Dividend < 0 ||
                         finalTarget.StockBorrowCost < 0 ||
                         finalTarget.IntRate < 0 ||
                         finalTarget.LastPrice < 0
                         )
                    {
                        finalTarget.Validated = INVALID;
                    }
                    else
                    {
                        finalTarget.Validated = VALID;
                    }
                    if (finalTarget.IntRate < 0)
                    {
                        e.Description = "Negative Value not allowed";
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
            public static bool SymbolCheck(object target, RuleArgs e)
            {
                UserOptModelInput finalTarget = target as UserOptModelInput;
                if (finalTarget != null)
                {
                    if (
                        string.IsNullOrEmpty(finalTarget.Symbol) ||
                         finalTarget.AuecID <= 0 ||
                         finalTarget.Volatility < 0 ||
                         finalTarget.Dividend < 0 ||
                         finalTarget.StockBorrowCost < 0 ||
                         finalTarget.IntRate < 0 ||
                         finalTarget.LastPrice < 0
                       )
                    {
                        finalTarget.Validated = INVALID;
                    }
                    else
                    {
                        finalTarget.Validated = VALID;
                    }

                    if (string.IsNullOrEmpty(finalTarget.Symbol))
                    {
                        e.Description = "Symbol not validated";
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
    }
}
