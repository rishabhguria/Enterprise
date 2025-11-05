namespace Nirvana.Middleware.Linq
{
    using System.Data.Linq;
    using System.Data.Linq.Mapping;
    using System.Data;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;
    using System.Linq.Expressions;
    using System.ComponentModel;
    using System;
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.T_MW_GenericPNL")]
    public partial class T_MW_GenericPNL
    {

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<System.DateTime> _Rundate;

        /// <summary>
        /// 
        /// </summary>
        private string _Symbol;

        /// <summary>
        /// 
        /// </summary>
        private string _UnderlyingSymbol;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<System.DateTime> _TradeDate;

        /// <summary>
        /// 
        /// </summary>
        private string _Strategy;

        /// <summary>
        /// 
        /// </summary>
        private string _MasterFund;

        /// <summary>
        /// 
        /// </summary>
        private string _Fund;

        /// <summary>
        /// 
        /// </summary>
        private string _Asset;

        /// <summary>
        /// 
        /// </summary>
        private string _Underlyer;

        /// <summary>
        /// 
        /// </summary>
        private string _Exchange;

        /// <summary>
        /// 
        /// </summary>
        private string _Currency;

        /// <summary>
        /// 
        /// </summary>
        private string _UDASector;

        /// <summary>
        /// 
        /// </summary>
        private string _UDACountry;

        /// <summary>
        /// 
        /// </summary>
        private string _UDASecurityType;

        /// <summary>
        /// 
        /// </summary>
        private string _UDAAssetClass;

        /// <summary>
        /// 
        /// </summary>
        private string _UDASubSector;

        /// <summary>
        /// 
        /// </summary>
        private string _TradeCurrency;

        /// <summary>
        /// 
        /// </summary>
        private string _Side;

        /// <summary>
        /// 
        /// </summary>
        private string _SecurityName;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _UnitCostLocal;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _UnitCostBase;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _ClosingPriceLocal;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _ClosingPriceBase;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _OpeningFXRate;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _TradeDateFXRate;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _BeginningFXRate;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _EndingFXRate;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _BeginningPriceLocal;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _EndingPriceLocal;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _BeginningPriceBase;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _EndingPriceBase;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _BeginningQuantity;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _EndingQuantity;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _Multiplier;

        /// <summary>
        /// 
        /// </summary>
        private string _SideMultiplier;

        /// <summary>
        /// 
        /// </summary>
        private string _Broker;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _TotalOpenCommissionAndFees_Local;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _TotalOpenCommissionAndFees_Base;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _TotalClosedCommissionAndFees_Local;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _TotalClosedCommissionAndFees_Base;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _BeginningMarketValueLocal;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _BeginningMarketValueBase;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _BeginningMarketValue_BaseD2FX;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _EndingMarketValueLocal;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _EndingMarketValueBase;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _UnrealizedTotalGainOnCostD0_Local;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _UnrealizedTotalGainOnCostD0_Base;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _UnrealizedTotalGainOnCostD2_Local;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _UnrealizedTotalGainOnCostD2_Base;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _ChangeInUnrealizedPNL_Local;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _ChangeInUnrealizedPNL;

        /// <summary>
        /// 
        /// </summary>
        private double _DividendLocal;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _Dividend;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<System.DateTime> _ClosingDate;

        /// <summary>
        /// 
        /// </summary>
        private string _Open_CloseTag;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _Delta;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _Beta;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _impliedVol;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _Theta;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _Rho;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _Vega;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _Gamma;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _DeltaExposureLocal;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _DeltaExposureBase;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _BetaExposureLocal;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _BetaExposureBase;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _AverageVolume;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _AverageLiquidation;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _UnderlyingSymbolPrice;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _UnrealizedTradingGainOnCostD0_Base;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _UnrealizedFXGainOnCostD0_Base;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _UnrealizedTradingGainOnCostD2_Base;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _UnrealizedFXGainOnCostD2_Base;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _TotalCost_Local;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _TotalCost_Base;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _TotalCost_BaseD0FX;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _TotalCost_BaseD2FX;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _TotalRealizedPNLOnCostLocal;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _RealizedTradingPNLOnCost;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _RealizedFXPNLOnCost;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _TotalRealizedPNLOnCost;

        /// <summary>
        /// 
        /// </summary>
        /// 

        //Ankit20140304:Removing Short and Long Term Unrealized PNL

        //private System.Nullable<double> _ShortTermTotalRealizedPNL;

        ///// <summary>
        ///// 
        ///// </summary>
        //private System.Nullable<double> _LongTermTotalRealizedPNL;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<int> _AUECID;

        /// <summary>
        /// 
        /// </summary>
        private string _PutOrCall;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _StrikePrice;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<System.DateTime> _ExpirationDate;

        /// <summary>
        /// 
        /// </summary>
        private string _CUSIPSymbol;

        /// <summary>
        /// 
        /// </summary>
        private string _SEDOLSymbol;

        /// <summary>
        /// 
        /// </summary>
        private string _ISINSymbol;

        /// <summary>
        /// 
        /// </summary>
        private string _BloombergSYmbol;

        /// <summary>
        /// 
        /// </summary>
        private string _ReutersSYmbol;

        /// <summary>
        /// 
        /// </summary>
        private string _IDCOSymbol;

        /// <summary>
        /// 
        /// </summary>
        private string _OSISymbol;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _Coupon;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<System.DateTime> _IssueDate;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<System.DateTime> _MaturityDate;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<System.DateTime> _FirstCouponDate;

        /// <summary>
        /// 
        /// </summary>
        private string _CouponFrequency;

        /// <summary>
        /// 
        /// </summary>
        private string _AccrualBasis;

        /// <summary>
        /// 
        /// </summary>
        private bool _ISSwapped;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _NotionalValue;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _BenchMarkRate;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _Differential;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _OrigCostBasis;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _DayCount;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<System.DateTime> _FirstResetDate;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<System.DateTime> _OrigTransDate;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _SwapInterestLeg_Local;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _SwapInterestLeg_Base;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _UnrealizedTradingPNLMTM;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _UnrealizedFXPNLMTM;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _RealizedTradingPNLMTM;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _RealizedFXPNLMTM;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _TotalRealizedPNLMTM;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _TotalUnrealizedPNLMTM;

        /// <summary>
        /// 
        /// </summary>
        private double _TradePNLMTMBase;

        /// <summary>
        /// 
        /// </summary>
        private double _FxPNLMTMBase;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _TotalUnrealizedPNLMTM_Local;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _TotalRealizedPNLMTMLocal;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _TotalPNLMTMLocal;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _TotalPNLMTMBase;

        /// <summary>
        /// 
        /// </summary>
        private string _TaxlotID;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<System.Guid> _TaxlotClosingID;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<System.DateTime> _StartDate;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<System.DateTime> _EndDate;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<int> _DatesDiff;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<System.DateTime> _OriginalPurchaseDate;

        //Ankit20140304:Added Trade Attributes, Proceeds and Exchange Identifier

        /// <summary>
        /// Settlement Date Column (nullable date Type)
        /// </summary>
        private System.Nullable<System.DateTime>  _SettlementDate;

        private string _OpenTradeAttribute1;

        private string _OpenTradeAttribute2;

        private string _OpenTradeAttribute3;

        private string _OpenTradeAttribute4;

        private string _OpenTradeAttribute5;

        private string _OpenTradeAttribute6;

        private string _ClosedTradeAttribute1;

        private string _ClosedTradeAttribute2;

        private string _ClosedTradeAttribute3;

        private string _ClosedTradeAttribute4;

        private string _ClosedTradeAttribute5;

        private string _ClosedTradeAttribute6;

        private double _TotalProceeds_Local;

        private double _TotalProceeds_Base;

        private string _ExchangeIdentifier;

        //Ankit20140513: Added leveraged factor and Transaction Source

        private double _LeveragedFactor;

        //private string _OpenTransactionSource;

        //private string _ClosedTransactionSource;

        private string _BaseCurrency;

        private string _SettlCurrency;

        private System.Nullable<double> _SettlCurrFxRate;

        private System.Nullable<double> _SettlCurrAmt;

        /* http://jira.nirvanasolutions.com:8080/browse/PRANA-8110 */

        private string _RiskCurrency;

        private string _Issuer;

        private string _CountryOfRisk;

        private string _Region;

        private string _Analyst;

        private string _UCITSEligibleTag;

        private string _LiquidTag;

        private string _MarketCap;

        private string _CustomUDA1;

        private string _CustomUDA2;

        private string _CustomUDA3;

        private string _CustomUDA4;

        private string _CustomUDA5;

        private string _CustomUDA6;

        private string _CustomUDA7;

        private System.Nullable<double> _SharesOutstanding;
		private string _UnderlyingSymbolCompanyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public T_MW_GenericPNL()
        {
        }

        /// <summary>
        /// Gets or sets the rundate.
        /// </summary>
        /// <value>The rundate.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Rundate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> Rundate
        {
            get
            {
                return this._Rundate;
            }
            set
            {
                if ((this._Rundate != value))
                {
                    this._Rundate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>The symbol.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Symbol", DbType = "VarChar(100)")]
        public string Symbol
        {
            get
            {
                return this._Symbol;
            }
            set
            {
                if ((this._Symbol != value))
                {
                    this._Symbol = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the underlying symbol.
        /// </summary>
        /// <value>The underlying symbol.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UnderlyingSymbol", DbType = "VarChar(100)")]
        public string UnderlyingSymbol
        {
            get
            {
                return this._UnderlyingSymbol;
            }
            set
            {
                if ((this._UnderlyingSymbol != value))
                {
                    this._UnderlyingSymbol = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the trade date.
        /// </summary>
        /// <value>The trade date.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TradeDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> TradeDate
        {
            get
            {
                return this._TradeDate;
            }
            set
            {
                if ((this._TradeDate != value))
                {
                    this._TradeDate = value;
                }
            }
        }

        /// <summary>
        /// Settlement Date 
		/// Mostly for Fx trade the date on which it is settled
        /// </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SettlementDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> SettlementDate
        {
            get
            {
                return this._SettlementDate;
            }
            set
            {
                if ((this._SettlementDate != value))
                {
                    this._SettlementDate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the strategy.
        /// </summary>
        /// <value>The strategy.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Strategy", DbType = "VarChar(100)")]
        public string Strategy
        {
            get
            {
                return this._Strategy;
            }
            set
            {
                if ((this._Strategy != value))
                {
                    this._Strategy = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the master fund.
        /// </summary>
        /// <value>The master fund.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_MasterFund", DbType = "VarChar(100)")]
        public string MasterFund
        {
            get
            {
                return this._MasterFund;
            }
            set
            {
                if ((this._MasterFund != value))
                {
                    this._MasterFund = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the fund.
        /// </summary>
        /// <value>The fund.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Fund", DbType = "VarChar(100)")]
        public string Fund
        {
            get
            {
                return this._Fund;
            }
            set
            {
                if ((this._Fund != value))
                {
                    this._Fund = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the asset.
        /// </summary>
        /// <value>The asset.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Asset", DbType = "VarChar(100)")]
        public string Asset
        {
            get
            {
                return this._Asset;
            }
            set
            {
                if ((this._Asset != value))
                {
                    this._Asset = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the underlyer.
        /// </summary>
        /// <value>The underlyer.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Underlyer", DbType = "VarChar(100)")]
        public string Underlyer
        {
            get
            {
                return this._Underlyer;
            }
            set
            {
                if ((this._Underlyer != value))
                {
                    this._Underlyer = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the exchange.
        /// </summary>
        /// <value>The exchange.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Exchange", DbType = "VarChar(100)")]
        public string Exchange
        {
            get
            {
                return this._Exchange;
            }
            set
            {
                if ((this._Exchange != value))
                {
                    this._Exchange = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Currency", DbType = "VarChar(10)")]
        public string Currency
        {
            get
            {
                return this._Currency;
            }
            set
            {
                if ((this._Currency != value))
                {
                    this._Currency = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the UDA sector.
        /// </summary>
        /// <value>The UDA sector.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UDASector", DbType = "VarChar(100)")]
        public string UDASector
        {
            get
            {
                return this._UDASector;
            }
            set
            {
                if ((this._UDASector != value))
                {
                    this._UDASector = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the UDA country.
        /// </summary>
        /// <value>The UDA country.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UDACountry", DbType = "VarChar(100)")]
        public string UDACountry
        {
            get
            {
                return this._UDACountry;
            }
            set
            {
                if ((this._UDACountry != value))
                {
                    this._UDACountry = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of the UDA security.
        /// </summary>
        /// <value>The type of the UDA security.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UDASecurityType", DbType = "VarChar(100)")]
        public string UDASecurityType
        {
            get
            {
                return this._UDASecurityType;
            }
            set
            {
                if ((this._UDASecurityType != value))
                {
                    this._UDASecurityType = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the UDA asset class.
        /// </summary>
        /// <value>The UDA asset class.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UDAAssetClass", DbType = "VarChar(100)")]
        public string UDAAssetClass
        {
            get
            {
                return this._UDAAssetClass;
            }
            set
            {
                if ((this._UDAAssetClass != value))
                {
                    this._UDAAssetClass = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the UDA sub sector.
        /// </summary>
        /// <value>The UDA sub sector.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UDASubSector", DbType = "VarChar(100)")]
        public string UDASubSector
        {
            get
            {
                return this._UDASubSector;
            }
            set
            {
                if ((this._UDASubSector != value))
                {
                    this._UDASubSector = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the trade currency.
        /// </summary>
        /// <value>The trade currency.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TradeCurrency", DbType = "VarChar(10)")]
        public string TradeCurrency
        {
            get
            {
                return this._TradeCurrency;
            }
            set
            {
                if ((this._TradeCurrency != value))
                {
                    this._TradeCurrency = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the side.
        /// </summary>
        /// <value>The side.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Side", DbType = "VarChar(10)")]
        public string Side
        {
            get
            {
                return this._Side;
            }
            set
            {
                if ((this._Side != value))
                {
                    this._Side = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the security.
        /// </summary>
        /// <value>The name of the security.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SecurityName", DbType = "VarChar(500)")]
        public string SecurityName
        {
            get
            {
                return this._SecurityName;
            }
            set
            {
                if ((this._SecurityName != value))
                {
                    this._SecurityName = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the unit cost local.
        /// </summary>
        /// <value>The unit cost local.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UnitCostLocal", DbType = "Float")]
        public System.Nullable<double> UnitCostLocal
        {
            get
            {
                return this._UnitCostLocal;
            }
            set
            {
                if ((this._UnitCostLocal != value))
                {
                    this._UnitCostLocal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the unit cost base.
        /// </summary>
        /// <value>The unit cost base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UnitCostBase", DbType = "Float")]
        public System.Nullable<double> UnitCostBase
        {
            get
            {
                return this._UnitCostBase;
            }
            set
            {
                if ((this._UnitCostBase != value))
                {
                    this._UnitCostBase = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the closing price local.
        /// </summary>
        /// <value>The closing price local.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ClosingPriceLocal", DbType = "Float")]
        public System.Nullable<double> ClosingPriceLocal
        {
            get
            {
                return this._ClosingPriceLocal;
            }
            set
            {
                if ((this._ClosingPriceLocal != value))
                {
                    this._ClosingPriceLocal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the closing price base.
        /// </summary>
        /// <value>The closing price base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ClosingPriceBase", DbType = "Float")]
        public System.Nullable<double> ClosingPriceBase
        {
            get
            {
                return this._ClosingPriceBase;
            }
            set
            {
                if ((this._ClosingPriceBase != value))
                {
                    this._ClosingPriceBase = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the opening FX rate.
        /// </summary>
        /// <value>The opening FX rate.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OpeningFXRate", DbType = "Float")]
        public System.Nullable<double> OpeningFXRate
        {
            get
            {
                return this._OpeningFXRate;
            }
            set
            {
                if ((this._OpeningFXRate != value))
                {
                    this._OpeningFXRate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the trade date FX rate.
        /// </summary>
        /// <value>The trade date FX rate.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TradeDateFXRate", DbType = "Float")]
        public System.Nullable<double> TradeDateFXRate
        {
            get
            {
                return this._TradeDateFXRate;
            }
            set
            {
                if ((this._TradeDateFXRate != value))
                {
                    this._TradeDateFXRate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the beginning FX rate.
        /// </summary>
        /// <value>The beginning FX rate.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_BeginningFXRate", DbType = "Float")]
        public System.Nullable<double> BeginningFXRate
        {
            get
            {
                return this._BeginningFXRate;
            }
            set
            {
                if ((this._BeginningFXRate != value))
                {
                    this._BeginningFXRate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the ending FX rate.
        /// </summary>
        /// <value>The ending FX rate.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_EndingFXRate", DbType = "Float")]
        public System.Nullable<double> EndingFXRate
        {
            get
            {
                return this._EndingFXRate;
            }
            set
            {
                if ((this._EndingFXRate != value))
                {
                    this._EndingFXRate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the beginning price local.
        /// </summary>
        /// <value>The beginning price local.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_BeginningPriceLocal", DbType = "Float")]
        public System.Nullable<double> BeginningPriceLocal
        {
            get
            {
                return this._BeginningPriceLocal;
            }
            set
            {
                if ((this._BeginningPriceLocal != value))
                {
                    this._BeginningPriceLocal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the ending price local.
        /// </summary>
        /// <value>The ending price local.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_EndingPriceLocal", DbType = "Float")]
        public System.Nullable<double> EndingPriceLocal
        {
            get
            {
                return this._EndingPriceLocal;
            }
            set
            {
                if ((this._EndingPriceLocal != value))
                {
                    this._EndingPriceLocal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the beginning price base.
        /// </summary>
        /// <value>The beginning price base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_BeginningPriceBase", DbType = "Float")]
        public System.Nullable<double> BeginningPriceBase
        {
            get
            {
                return this._BeginningPriceBase;
            }
            set
            {
                if ((this._BeginningPriceBase != value))
                {
                    this._BeginningPriceBase = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the ending price base.
        /// </summary>
        /// <value>The ending price base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_EndingPriceBase", DbType = "Float")]
        public System.Nullable<double> EndingPriceBase
        {
            get
            {
                return this._EndingPriceBase;
            }
            set
            {
                if ((this._EndingPriceBase != value))
                {
                    this._EndingPriceBase = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the beginning quantity.
        /// </summary>
        /// <value>The beginning quantity.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_BeginningQuantity", DbType = "Float")]
        public System.Nullable<double> BeginningQuantity
        {
            get
            {
                return this._BeginningQuantity;
            }
            set
            {
                if ((this._BeginningQuantity != value))
                {
                    this._BeginningQuantity = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the ending quantity.
        /// </summary>
        /// <value>The ending quantity.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_EndingQuantity", DbType = "Float")]
        public System.Nullable<double> EndingQuantity
        {
            get
            {
                return this._EndingQuantity;
            }
            set
            {
                if ((this._EndingQuantity != value))
                {
                    this._EndingQuantity = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the multiplier.
        /// </summary>
        /// <value>The multiplier.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Multiplier", DbType = "Float")]
        public System.Nullable<double> Multiplier
        {
            get
            {
                return this._Multiplier;
            }
            set
            {
                if ((this._Multiplier != value))
                {
                    this._Multiplier = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the side multiplier.
        /// </summary>
        /// <value>The side multiplier.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SideMultiplier", DbType = "VarChar(5)")]
        public string SideMultiplier
        {
            get
            {
                return this._SideMultiplier;
            }
            set
            {
                if ((this._SideMultiplier != value))
                {
                    this._SideMultiplier = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the broker.
        /// </summary>
        /// <value>The broker.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Broker", DbType = "VarChar(100)")]
        public string Broker
        {
            get
            {
                return this._Broker;
            }
            set
            {
                if ((this._Broker != value))
                {
                    this._Broker = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the total open commission and fees_ local.
        /// </summary>
        /// <value>The total open commission and fees_ local.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TotalOpenCommissionAndFees_Local", DbType = "Float")]
        public System.Nullable<double> TotalOpenCommissionAndFees_Local
        {
            get
            {
                return this._TotalOpenCommissionAndFees_Local;
            }
            set
            {
                if ((this._TotalOpenCommissionAndFees_Local != value))
                {
                    this._TotalOpenCommissionAndFees_Local = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the total open commission and fees_ base.
        /// </summary>
        /// <value>The total open commission and fees_ base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TotalOpenCommissionAndFees_Base", DbType = "Float")]
        public System.Nullable<double> TotalOpenCommissionAndFees_Base
        {
            get
            {
                return this._TotalOpenCommissionAndFees_Base;
            }
            set
            {
                if ((this._TotalOpenCommissionAndFees_Base != value))
                {
                    this._TotalOpenCommissionAndFees_Base = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the total closed commission and fees_ local.
        /// </summary>
        /// <value>The total closed commission and fees_ local.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TotalClosedCommissionAndFees_Local", DbType = "Float")]
        public System.Nullable<double> TotalClosedCommissionAndFees_Local
        {
            get
            {
                return this._TotalClosedCommissionAndFees_Local;
            }
            set
            {
                if ((this._TotalClosedCommissionAndFees_Local != value))
                {
                    this._TotalClosedCommissionAndFees_Local = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the total closed commission and fees_ base.
        /// </summary>
        /// <value>The total closed commission and fees_ base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TotalClosedCommissionAndFees_Base", DbType = "Float")]
        public System.Nullable<double> TotalClosedCommissionAndFees_Base
        {
            get
            {
                return this._TotalClosedCommissionAndFees_Base;
            }
            set
            {
                if ((this._TotalClosedCommissionAndFees_Base != value))
                {
                    this._TotalClosedCommissionAndFees_Base = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the beginning market value local.
        /// </summary>
        /// <value>The beginning market value local.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_BeginningMarketValueLocal", DbType = "Float")]
        public System.Nullable<double> BeginningMarketValueLocal
        {
            get
            {
                return this._BeginningMarketValueLocal;
            }
            set
            {
                if ((this._BeginningMarketValueLocal != value))
                {
                    this._BeginningMarketValueLocal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the beginning market value base.
        /// </summary>
        /// <value>The beginning market value base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_BeginningMarketValueBase", DbType = "Float")]
        public System.Nullable<double> BeginningMarketValueBase
        {
            get
            {
                return this._BeginningMarketValueBase;
            }
            set
            {
                if ((this._BeginningMarketValueBase != value))
                {
                    this._BeginningMarketValueBase = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the beginning market value_ base d2 FX.
        /// </summary>
        /// <value>The beginning market value_ base d2 FX.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_BeginningMarketValue_BaseD2FX", DbType = "Float")]
        public System.Nullable<double> BeginningMarketValue_BaseD2FX
        {
            get
            {
                return this._BeginningMarketValue_BaseD2FX;
            }
            set
            {
                if ((this._BeginningMarketValue_BaseD2FX != value))
                {
                    this._BeginningMarketValue_BaseD2FX = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the ending market value local.
        /// </summary>
        /// <value>The ending market value local.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_EndingMarketValueLocal", DbType = "Float")]
        public System.Nullable<double> EndingMarketValueLocal
        {
            get
            {
                return this._EndingMarketValueLocal;
            }
            set
            {
                if ((this._EndingMarketValueLocal != value))
                {
                    this._EndingMarketValueLocal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the ending market value base.
        /// </summary>
        /// <value>The ending market value base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_EndingMarketValueBase", DbType = "Float")]
        public System.Nullable<double> EndingMarketValueBase
        {
            get
            {
                return this._EndingMarketValueBase;
            }
            set
            {
                if ((this._EndingMarketValueBase != value))
                {
                    this._EndingMarketValueBase = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the unrealized total gain on cost D0_ local.
        /// </summary>
        /// <value>The unrealized total gain on cost D0_ local.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UnrealizedTotalGainOnCostD0_Local", DbType = "Float")]
        public System.Nullable<double> UnrealizedTotalGainOnCostD0_Local
        {
            get
            {
                return this._UnrealizedTotalGainOnCostD0_Local;
            }
            set
            {
                if ((this._UnrealizedTotalGainOnCostD0_Local != value))
                {
                    this._UnrealizedTotalGainOnCostD0_Local = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the unrealized total gain on cost D0_ base.
        /// </summary>
        /// <value>The unrealized total gain on cost D0_ base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UnrealizedTotalGainOnCostD0_Base", DbType = "Float")]
        public System.Nullable<double> UnrealizedTotalGainOnCostD0_Base
        {
            get
            {
                return this._UnrealizedTotalGainOnCostD0_Base;
            }
            set
            {
                if ((this._UnrealizedTotalGainOnCostD0_Base != value))
                {
                    this._UnrealizedTotalGainOnCostD0_Base = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the unrealized total gain on cost D2_ local.
        /// </summary>
        /// <value>The unrealized total gain on cost D2_ local.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UnrealizedTotalGainOnCostD2_Local", DbType = "Float")]
        public System.Nullable<double> UnrealizedTotalGainOnCostD2_Local
        {
            get
            {
                return this._UnrealizedTotalGainOnCostD2_Local;
            }
            set
            {
                if ((this._UnrealizedTotalGainOnCostD2_Local != value))
                {
                    this._UnrealizedTotalGainOnCostD2_Local = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the unrealized total gain on cost D2_ base.
        /// </summary>
        /// <value>The unrealized total gain on cost D2_ base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UnrealizedTotalGainOnCostD2_Base", DbType = "Float")]
        public System.Nullable<double> UnrealizedTotalGainOnCostD2_Base
        {
            get
            {
                return this._UnrealizedTotalGainOnCostD2_Base;
            }
            set
            {
                if ((this._UnrealizedTotalGainOnCostD2_Base != value))
                {
                    this._UnrealizedTotalGainOnCostD2_Base = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the change in unrealized PN l_ local.
        /// </summary>
        /// <value>The change in unrealized PN l_ local.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ChangeInUnrealizedPNL_Local", DbType = "Float")]
        public System.Nullable<double> ChangeInUnrealizedPNL_Local
        {
            get
            {
                return this._ChangeInUnrealizedPNL_Local;
            }
            set
            {
                if ((this._ChangeInUnrealizedPNL_Local != value))
                {
                    this._ChangeInUnrealizedPNL_Local = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the change in unrealized PNL.
        /// </summary>
        /// <value>The change in unrealized PNL.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ChangeInUnrealizedPNL", DbType = "Float")]
        public System.Nullable<double> ChangeInUnrealizedPNL
        {
            get
            {
                return this._ChangeInUnrealizedPNL;
            }
            set
            {
                if ((this._ChangeInUnrealizedPNL != value))
                {
                    this._ChangeInUnrealizedPNL = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the dividend local.
        /// </summary>
        /// <value>The dividend local.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DividendLocal", DbType = "Float NOT NULL")]
        public double DividendLocal
        {
            get
            {
                return this._DividendLocal;
            }
            set
            {
                if ((this._DividendLocal != value))
                {
                    this._DividendLocal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the dividend.
        /// </summary>
        /// <value>The dividend.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Dividend", DbType = "Float")]
        public System.Nullable<double> Dividend
        {
            get
            {
                return this._Dividend;
            }
            set
            {
                if ((this._Dividend != value))
                {
                    this._Dividend = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the closing date.
        /// </summary>
        /// <value>The closing date.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ClosingDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> ClosingDate
        {
            get
            {
                return this._ClosingDate;
            }
            set
            {
                if ((this._ClosingDate != value))
                {
                    this._ClosingDate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the open_ close tag.
        /// </summary>
        /// <value>The open_ close tag.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Open_CloseTag", DbType = "VarChar(20)")]
        public string Open_CloseTag
        {
            get
            {
                return this._Open_CloseTag;
            }
            set
            {
                if ((this._Open_CloseTag != value))
                {
                    this._Open_CloseTag = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the delta.
        /// </summary>
        /// <value>The delta.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Delta", DbType = "Float")]
        public System.Nullable<double> Delta
        {
            get
            {
                return this._Delta;
            }
            set
            {
                if ((this._Delta != value))
                {
                    this._Delta = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the beta.
        /// </summary>
        /// <value>The beta.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Beta", DbType = "Float")]
        public System.Nullable<double> Beta
        {
            get
            {
                return this._Beta;
            }
            set
            {
                if ((this._Beta != value))
                {
                    this._Beta = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the implied vol.
        /// </summary>
        /// <value>The implied vol.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_impliedVol", DbType = "Float")]
        public System.Nullable<double> impliedVol
        {
            get
            {
                return this._impliedVol;
            }
            set
            {
                if ((this._impliedVol != value))
                {
                    this._impliedVol = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the theta.
        /// </summary>
        /// <value>The theta.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Theta", DbType = "Float")]
        public System.Nullable<double> Theta
        {
            get
            {
                return this._Theta;
            }
            set
            {
                if ((this._Theta != value))
                {
                    this._Theta = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the rho.
        /// </summary>
        /// <value>The rho.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Rho", DbType = "Float")]
        public System.Nullable<double> Rho
        {
            get
            {
                return this._Rho;
            }
            set
            {
                if ((this._Rho != value))
                {
                    this._Rho = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the vega.
        /// </summary>
        /// <value>The vega.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Vega", DbType = "Float")]
        public System.Nullable<double> Vega
        {
            get
            {
                return this._Vega;
            }
            set
            {
                if ((this._Vega != value))
                {
                    this._Vega = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the gamma.
        /// </summary>
        /// <value>The gamma.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Gamma", DbType = "Float")]
        public System.Nullable<double> Gamma
        {
            get
            {
                return this._Gamma;
            }
            set
            {
                if ((this._Gamma != value))
                {
                    this._Gamma = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the delta exposure local.
        /// </summary>
        /// <value>The delta exposure local.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DeltaExposureLocal", DbType = "Float")]
        public System.Nullable<double> DeltaExposureLocal
        {
            get
            {
                return this._DeltaExposureLocal;
            }
            set
            {
                if ((this._DeltaExposureLocal != value))
                {
                    this._DeltaExposureLocal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the delta exposure base.
        /// </summary>
        /// <value>The delta exposure base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DeltaExposureBase", DbType = "Float")]
        public System.Nullable<double> DeltaExposureBase
        {
            get
            {
                return this._DeltaExposureBase;
            }
            set
            {
                if ((this._DeltaExposureBase != value))
                {
                    this._DeltaExposureBase = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the beta exposure local.
        /// </summary>
        /// <value>The beta exposure local.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_BetaExposureLocal", DbType = "Float")]
        public System.Nullable<double> BetaExposureLocal
        {
            get
            {
                return this._BetaExposureLocal;
            }
            set
            {
                if ((this._BetaExposureLocal != value))
                {
                    this._BetaExposureLocal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the beta exposure base.
        /// </summary>
        /// <value>The beta exposure base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_BetaExposureBase", DbType = "Float")]
        public System.Nullable<double> BetaExposureBase
        {
            get
            {
                return this._BetaExposureBase;
            }
            set
            {
                if ((this._BetaExposureBase != value))
                {
                    this._BetaExposureBase = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the average volume.
        /// </summary>
        /// <value>The average volume.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_AverageVolume", DbType = "Float")]
        public System.Nullable<double> AverageVolume
        {
            get
            {
                return this._AverageVolume;
            }
            set
            {
                if ((this._AverageVolume != value))
                {
                    this._AverageVolume = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the average liquidation.
        /// </summary>
        /// <value>The average liquidation.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_AverageLiquidation", DbType = "Float")]
        public System.Nullable<double> AverageLiquidation
        {
            get
            {
                return this._AverageLiquidation;
            }
            set
            {
                if ((this._AverageLiquidation != value))
                {
                    this._AverageLiquidation = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the underlying symbol price.
        /// </summary>
        /// <value>The underlying symbol price.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UnderlyingSymbolPrice", DbType = "Float")]
        public System.Nullable<double> UnderlyingSymbolPrice
        {
            get
            {
                return this._UnderlyingSymbolPrice;
            }
            set
            {
                if ((this._UnderlyingSymbolPrice != value))
                {
                    this._UnderlyingSymbolPrice = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the unrealized trading gain on cost D0_ base.
        /// </summary>
        /// <value>The unrealized trading gain on cost D0_ base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UnrealizedTradingGainOnCostD0_Base", DbType = "Float")]
        public System.Nullable<double> UnrealizedTradingGainOnCostD0_Base
        {
            get
            {
                return this._UnrealizedTradingGainOnCostD0_Base;
            }
            set
            {
                if ((this._UnrealizedTradingGainOnCostD0_Base != value))
                {
                    this._UnrealizedTradingGainOnCostD0_Base = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the unrealized FX gain on cost D0_ base.
        /// </summary>
        /// <value>The unrealized FX gain on cost D0_ base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UnrealizedFXGainOnCostD0_Base", DbType = "Float")]
        public System.Nullable<double> UnrealizedFXGainOnCostD0_Base
        {
            get
            {
                return this._UnrealizedFXGainOnCostD0_Base;
            }
            set
            {
                if ((this._UnrealizedFXGainOnCostD0_Base != value))
                {
                    this._UnrealizedFXGainOnCostD0_Base = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the unrealized trading gain on cost D2_ base.
        /// </summary>
        /// <value>The unrealized trading gain on cost D2_ base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UnrealizedTradingGainOnCostD2_Base", DbType = "Float")]
        public System.Nullable<double> UnrealizedTradingGainOnCostD2_Base
        {
            get
            {
                return this._UnrealizedTradingGainOnCostD2_Base;
            }
            set
            {
                if ((this._UnrealizedTradingGainOnCostD2_Base != value))
                {
                    this._UnrealizedTradingGainOnCostD2_Base = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the unrealized FX gain on cost D2_ base.
        /// </summary>
        /// <value>The unrealized FX gain on cost D2_ base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UnrealizedFXGainOnCostD2_Base", DbType = "Float")]
        public System.Nullable<double> UnrealizedFXGainOnCostD2_Base
        {
            get
            {
                return this._UnrealizedFXGainOnCostD2_Base;
            }
            set
            {
                if ((this._UnrealizedFXGainOnCostD2_Base != value))
                {
                    this._UnrealizedFXGainOnCostD2_Base = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the total cost_ local.
        /// </summary>
        /// <value>The total cost_ local.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TotalCost_Local", DbType = "Float")]
        public System.Nullable<double> TotalCost_Local
        {
            get
            {
                return this._TotalCost_Local;
            }
            set
            {
                if ((this._TotalCost_Local != value))
                {
                    this._TotalCost_Local = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the total cost_ base.
        /// </summary>
        /// <value>The total cost_ base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TotalCost_Base", DbType = "Float")]
        public System.Nullable<double> TotalCost_Base
        {
            get
            {
                return this._TotalCost_Base;
            }
            set
            {
                if ((this._TotalCost_Base != value))
                {
                    this._TotalCost_Base = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the total cost_ base d0 FX.
        /// </summary>
        /// <value>The total cost_ base d0 FX.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TotalCost_BaseD0FX", DbType = "Float")]
        public System.Nullable<double> TotalCost_BaseD0FX
        {
            get
            {
                return this._TotalCost_BaseD0FX;
            }
            set
            {
                if ((this._TotalCost_BaseD0FX != value))
                {
                    this._TotalCost_BaseD0FX = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the total cost_ base d2 FX.
        /// </summary>
        /// <value>The total cost_ base d2 FX.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TotalCost_BaseD2FX", DbType = "Float")]
        public System.Nullable<double> TotalCost_BaseD2FX
        {
            get
            {
                return this._TotalCost_BaseD2FX;
            }
            set
            {
                if ((this._TotalCost_BaseD2FX != value))
                {
                    this._TotalCost_BaseD2FX = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the total realized PNL on cost local.
        /// </summary>
        /// <value>The total realized PNL on cost local.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TotalRealizedPNLOnCostLocal", DbType = "Float")]
        public System.Nullable<double> TotalRealizedPNLOnCostLocal
        {
            get
            {
                return this._TotalRealizedPNLOnCostLocal;
            }
            set
            {
                if ((this._TotalRealizedPNLOnCostLocal != value))
                {
                    this._TotalRealizedPNLOnCostLocal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the realized trading PNL on cost.
        /// </summary>
        /// <value>The realized trading PNL on cost.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RealizedTradingPNLOnCost", DbType = "Float")]
        public System.Nullable<double> RealizedTradingPNLOnCost
        {
            get
            {
                return this._RealizedTradingPNLOnCost;
            }
            set
            {
                if ((this._RealizedTradingPNLOnCost != value))
                {
                    this._RealizedTradingPNLOnCost = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the realized FXPNL on cost.
        /// </summary>
        /// <value>The realized FXPNL on cost.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RealizedFXPNLOnCost", DbType = "Float")]
        public System.Nullable<double> RealizedFXPNLOnCost
        {
            get
            {
                return this._RealizedFXPNLOnCost;
            }
            set
            {
                if ((this._RealizedFXPNLOnCost != value))
                {
                    this._RealizedFXPNLOnCost = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the total realized PNL on cost.
        /// </summary>
        /// <value>The total realized PNL on cost.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TotalRealizedPNLOnCost", DbType = "Float")]
        public System.Nullable<double> TotalRealizedPNLOnCost
        {
            get
            {
                return this._TotalRealizedPNLOnCost;
            }
            set
            {
                if ((this._TotalRealizedPNLOnCost != value))
                {
                    this._TotalRealizedPNLOnCost = value;
                }
            }
        }

        //Ankit20140304:Removing Short and Long Term Unrealized PNL

        /// <summary>
        /// Gets or sets the short term total realized PNL.
        /// </summary>
        /// <value>The short term total realized PNL.</value>
        /// <remarks></remarks>
        //////[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ShortTermTotalRealizedPNL", DbType = "Float")]
        //////public System.Nullable<double> ShortTermTotalRealizedPNL
        //////{
        //////    get
        //////    {
        //////        return this._ShortTermTotalRealizedPNL;
        //////    }
        //////    set
        //////    {
        //////        if ((this._ShortTermTotalRealizedPNL != value))
        //////        {
        //////            this._ShortTermTotalRealizedPNL = value;
        //////        }
        //////    }
        //////}

        ///////// <summary>
        ///////// Gets or sets the long term total realized PNL.
        ///////// </summary>
        ///////// <value>The long term total realized PNL.</value>
        ///////// <remarks></remarks>
        //////[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LongTermTotalRealizedPNL", DbType = "Float")]
        //////public System.Nullable<double> LongTermTotalRealizedPNL
        //////{
        //////    get
        //////    {
        //////        return this._LongTermTotalRealizedPNL;
        //////    }
        //////    set
        //////    {
        //////        if ((this._LongTermTotalRealizedPNL != value))
        //////        {
        //////            this._LongTermTotalRealizedPNL = value;
        //////        }
        //////    }
        //////}

        /// <summary>
        /// Gets or sets the AUECID.
        /// </summary>
        /// <value>The AUECID.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_AUECID", DbType = "Int")]
        public System.Nullable<int> AUECID
        {
            get
            {
                return this._AUECID;
            }
            set
            {
                if ((this._AUECID != value))
                {
                    this._AUECID = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the put or call.
        /// </summary>
        /// <value>The put or call.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PutOrCall", DbType = "VarChar(10)")]
        public string PutOrCall
        {
            get
            {
                return this._PutOrCall;
            }
            set
            {
                if ((this._PutOrCall != value))
                {
                    this._PutOrCall = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the strike price.
        /// </summary>
        /// <value>The strike price.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_StrikePrice", DbType = "Float")]
        public System.Nullable<double> StrikePrice
        {
            get
            {
                return this._StrikePrice;
            }
            set
            {
                if ((this._StrikePrice != value))
                {
                    this._StrikePrice = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>The expiration date.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ExpirationDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> ExpirationDate
        {
            get
            {
                return this._ExpirationDate;
            }
            set
            {
                if ((this._ExpirationDate != value))
                {
                    this._ExpirationDate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the CUSIP symbol.
        /// </summary>
        /// <value>The CUSIP symbol.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CUSIPSymbol", DbType = "VarChar(50)")]
        public string CUSIPSymbol
        {
            get
            {
                return this._CUSIPSymbol;
            }
            set
            {
                if ((this._CUSIPSymbol != value))
                {
                    this._CUSIPSymbol = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the SEDOL symbol.
        /// </summary>
        /// <value>The SEDOL symbol.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SEDOLSymbol", DbType = "VarChar(50)")]
        public string SEDOLSymbol
        {
            get
            {
                return this._SEDOLSymbol;
            }
            set
            {
                if ((this._SEDOLSymbol != value))
                {
                    this._SEDOLSymbol = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the ISIN symbol.
        /// </summary>
        /// <value>The ISIN symbol.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ISINSymbol", DbType = "VarChar(50)")]
        public string ISINSymbol
        {
            get
            {
                return this._ISINSymbol;
            }
            set
            {
                if ((this._ISINSymbol != value))
                {
                    this._ISINSymbol = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the bloomberg S ymbol.
        /// </summary>
        /// <value>The bloomberg S ymbol.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_BloombergSYmbol", DbType = "VarChar(50)")]
        public string BloombergSYmbol
        {
            get
            {
                return this._BloombergSYmbol;
            }
            set
            {
                if ((this._BloombergSYmbol != value))
                {
                    this._BloombergSYmbol = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the reuters S ymbol.
        /// </summary>
        /// <value>The reuters S ymbol.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ReutersSYmbol", DbType = "VarChar(50)")]
        public string ReutersSYmbol
        {
            get
            {
                return this._ReutersSYmbol;
            }
            set
            {
                if ((this._ReutersSYmbol != value))
                {
                    this._ReutersSYmbol = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the IDCO symbol.
        /// </summary>
        /// <value>The IDCO symbol.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IDCOSymbol", DbType = "VarChar(50)")]
        public string IDCOSymbol
        {
            get
            {
                return this._IDCOSymbol;
            }
            set
            {
                if ((this._IDCOSymbol != value))
                {
                    this._IDCOSymbol = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the OSI symbol.
        /// </summary>
        /// <value>The OSI symbol.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OSISymbol", DbType = "VarChar(50)")]
        public string OSISymbol
        {
            get
            {
                return this._OSISymbol;
            }
            set
            {
                if ((this._OSISymbol != value))
                {
                    this._OSISymbol = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the coupon.
        /// </summary>
        /// <value>The coupon.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Coupon", DbType = "Float")]
        public System.Nullable<double> Coupon
        {
            get
            {
                return this._Coupon;
            }
            set
            {
                if ((this._Coupon != value))
                {
                    this._Coupon = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the issue date.
        /// </summary>
        /// <value>The issue date.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IssueDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> IssueDate
        {
            get
            {
                return this._IssueDate;
            }
            set
            {
                if ((this._IssueDate != value))
                {
                    this._IssueDate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the maturity date.
        /// </summary>
        /// <value>The maturity date.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_MaturityDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> MaturityDate
        {
            get
            {
                return this._MaturityDate;
            }
            set
            {
                if ((this._MaturityDate != value))
                {
                    this._MaturityDate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the first coupon date.
        /// </summary>
        /// <value>The first coupon date.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FirstCouponDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> FirstCouponDate
        {
            get
            {
                return this._FirstCouponDate;
            }
            set
            {
                if ((this._FirstCouponDate != value))
                {
                    this._FirstCouponDate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the coupon frequency.
        /// </summary>
        /// <value>The coupon frequency.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CouponFrequency", DbType = "VarChar(20)")]
        public string CouponFrequency
        {
            get
            {
                return this._CouponFrequency;
            }
            set
            {
                if ((this._CouponFrequency != value))
                {
                    this._CouponFrequency = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the accrual basis.
        /// </summary>
        /// <value>The accrual basis.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_AccrualBasis", DbType = "VarChar(20)")]
        public string AccrualBasis
        {
            get
            {
                return this._AccrualBasis;
            }
            set
            {
                if ((this._AccrualBasis != value))
                {
                    this._AccrualBasis = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [IS swapped].
        /// </summary>
        /// <value><c>true</c> if [IS swapped]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ISSwapped", DbType = "Bit NOT NULL")]
        public bool ISSwapped
        {
            get
            {
                return this._ISSwapped;
            }
            set
            {
                if ((this._ISSwapped != value))
                {
                    this._ISSwapped = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the notional value.
        /// </summary>
        /// <value>The notional value.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_NotionalValue", DbType = "Float")]
        public System.Nullable<double> NotionalValue
        {
            get
            {
                return this._NotionalValue;
            }
            set
            {
                if ((this._NotionalValue != value))
                {
                    this._NotionalValue = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the bench mark rate.
        /// </summary>
        /// <value>The bench mark rate.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_BenchMarkRate", DbType = "Float")]
        public System.Nullable<double> BenchMarkRate
        {
            get
            {
                return this._BenchMarkRate;
            }
            set
            {
                if ((this._BenchMarkRate != value))
                {
                    this._BenchMarkRate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the differential.
        /// </summary>
        /// <value>The differential.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Differential", DbType = "Float")]
        public System.Nullable<double> Differential
        {
            get
            {
                return this._Differential;
            }
            set
            {
                if ((this._Differential != value))
                {
                    this._Differential = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the orig cost basis.
        /// </summary>
        /// <value>The orig cost basis.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OrigCostBasis", DbType = "Float")]
        public System.Nullable<double> OrigCostBasis
        {
            get
            {
                return this._OrigCostBasis;
            }
            set
            {
                if ((this._OrigCostBasis != value))
                {
                    this._OrigCostBasis = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the day count.
        /// </summary>
        /// <value>The day count.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DayCount", DbType = "Float")]
        public System.Nullable<double> DayCount
        {
            get
            {
                return this._DayCount;
            }
            set
            {
                if ((this._DayCount != value))
                {
                    this._DayCount = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the first reset date.
        /// </summary>
        /// <value>The first reset date.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FirstResetDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> FirstResetDate
        {
            get
            {
                return this._FirstResetDate;
            }
            set
            {
                if ((this._FirstResetDate != value))
                {
                    this._FirstResetDate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the orig trans date.
        /// </summary>
        /// <value>The orig trans date.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OrigTransDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> OrigTransDate
        {
            get
            {
                return this._OrigTransDate;
            }
            set
            {
                if ((this._OrigTransDate != value))
                {
                    this._OrigTransDate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the swap interest leg_ local.
        /// </summary>
        /// <value>The swap interest leg_ local.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SwapInterestLeg_Local", DbType = "Float")]
        public System.Nullable<double> SwapInterestLeg_Local
        {
            get
            {
                return this._SwapInterestLeg_Local;
            }
            set
            {
                if ((this._SwapInterestLeg_Local != value))
                {
                    this._SwapInterestLeg_Local = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the swap interest leg_ base.
        /// </summary>
        /// <value>The swap interest leg_ base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SwapInterestLeg_Base", DbType = "Float")]
        public System.Nullable<double> SwapInterestLeg_Base
        {
            get
            {
                return this._SwapInterestLeg_Base;
            }
            set
            {
                if ((this._SwapInterestLeg_Base != value))
                {
                    this._SwapInterestLeg_Base = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the unrealized trading PNLMTM.
        /// </summary>
        /// <value>The unrealized trading PNLMTM.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UnrealizedTradingPNLMTM", DbType = "Float")]
        public System.Nullable<double> UnrealizedTradingPNLMTM
        {
            get
            {
                return this._UnrealizedTradingPNLMTM;
            }
            set
            {
                if ((this._UnrealizedTradingPNLMTM != value))
                {
                    this._UnrealizedTradingPNLMTM = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the unrealized FXPNLMTM.
        /// </summary>
        /// <value>The unrealized FXPNLMTM.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UnrealizedFXPNLMTM", DbType = "Float")]
        public System.Nullable<double> UnrealizedFXPNLMTM
        {
            get
            {
                return this._UnrealizedFXPNLMTM;
            }
            set
            {
                if ((this._UnrealizedFXPNLMTM != value))
                {
                    this._UnrealizedFXPNLMTM = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the realized trading PNLMTM.
        /// </summary>
        /// <value>The realized trading PNLMTM.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RealizedTradingPNLMTM", DbType = "Float")]
        public System.Nullable<double> RealizedTradingPNLMTM
        {
            get
            {
                return this._RealizedTradingPNLMTM;
            }
            set
            {
                if ((this._RealizedTradingPNLMTM != value))
                {
                    this._RealizedTradingPNLMTM = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the realized FXPNLMTM.
        /// </summary>
        /// <value>The realized FXPNLMTM.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RealizedFXPNLMTM", DbType = "Float")]
        public System.Nullable<double> RealizedFXPNLMTM
        {
            get
            {
                return this._RealizedFXPNLMTM;
            }
            set
            {
                if ((this._RealizedFXPNLMTM != value))
                {
                    this._RealizedFXPNLMTM = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the total realized PNLMTM.
        /// </summary>
        /// <value>The total realized PNLMTM.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TotalRealizedPNLMTM", DbType = "Float")]
        public System.Nullable<double> TotalRealizedPNLMTM
        {
            get
            {
                return this._TotalRealizedPNLMTM;
            }
            set
            {
                if ((this._TotalRealizedPNLMTM != value))
                {
                    this._TotalRealizedPNLMTM = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the total unrealized PNLMTM.
        /// </summary>
        /// <value>The total unrealized PNLMTM.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TotalUnrealizedPNLMTM", DbType = "Float")]
        public System.Nullable<double> TotalUnrealizedPNLMTM
        {
            get
            {
                return this._TotalUnrealizedPNLMTM;
            }
            set
            {
                if ((this._TotalUnrealizedPNLMTM != value))
                {
                    this._TotalUnrealizedPNLMTM = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the trade PNLMTM base.
        /// </summary>
        /// <value>The trade PNLMTM base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TradePNLMTMBase", DbType = "Float NOT NULL")]
        public double TradePNLMTMBase
        {
            get
            {
                return this._TradePNLMTMBase;
            }
            set
            {
                if ((this._TradePNLMTMBase != value))
                {
                    this._TradePNLMTMBase = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the fx PNLMTM base.
        /// </summary>
        /// <value>The fx PNLMTM base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FxPNLMTMBase", DbType = "Float NOT NULL")]
        public double FxPNLMTMBase
        {
            get
            {
                return this._FxPNLMTMBase;
            }
            set
            {
                if ((this._FxPNLMTMBase != value))
                {
                    this._FxPNLMTMBase = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the total unrealized PNLMT m_ local.
        /// </summary>
        /// <value>The total unrealized PNLMT m_ local.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TotalUnrealizedPNLMTM_Local", DbType = "Float")]
        public System.Nullable<double> TotalUnrealizedPNLMTM_Local
        {
            get
            {
                return this._TotalUnrealizedPNLMTM_Local;
            }
            set
            {
                if ((this._TotalUnrealizedPNLMTM_Local != value))
                {
                    this._TotalUnrealizedPNLMTM_Local = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the total realized PNLMTM local.
        /// </summary>
        /// <value>The total realized PNLMTM local.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TotalRealizedPNLMTMLocal", DbType = "Float")]
        public System.Nullable<double> TotalRealizedPNLMTMLocal
        {
            get
            {
                return this._TotalRealizedPNLMTMLocal;
            }
            set
            {
                if ((this._TotalRealizedPNLMTMLocal != value))
                {
                    this._TotalRealizedPNLMTMLocal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the total PNLMTM local.
        /// </summary>
        /// <value>The total PNLMTM local.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TotalPNLMTMLocal", DbType = "Float")]
        public System.Nullable<double> TotalPNLMTMLocal
        {
            get
            {
                return this._TotalPNLMTMLocal;
            }
            set
            {
                if ((this._TotalPNLMTMLocal != value))
                {
                    this._TotalPNLMTMLocal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the total PNLMTM base.
        /// </summary>
        /// <value>The total PNLMTM base.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TotalPNLMTMBase", DbType = "Float")]
        public System.Nullable<double> TotalPNLMTMBase
        {
            get
            {
                return this._TotalPNLMTMBase;
            }
            set
            {
                if ((this._TotalPNLMTMBase != value))
                {
                    this._TotalPNLMTMBase = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the taxlot ID.
        /// </summary>
        /// <value>The taxlot ID.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TaxlotID", DbType = "VarChar(50)")]
        public string TaxlotID
        {
            get
            {
                return this._TaxlotID;
            }
            set
            {
                if ((this._TaxlotID != value))
                {
                    this._TaxlotID = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the taxlot closing ID.
        /// </summary>
        /// <value>The taxlot closing ID.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TaxlotClosingID", DbType = "UniqueIdentifier")]
        public System.Nullable<System.Guid> TaxlotClosingID
        {
            get
            {
                return this._TaxlotClosingID;
            }
            set
            {
                if ((this._TaxlotClosingID != value))
                {
                    this._TaxlotClosingID = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_StartDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> StartDate
        {
            get
            {
                return this._StartDate;
            }
            set
            {
                if ((this._StartDate != value))
                {
                    this._StartDate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>The end date.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_EndDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> EndDate
        {
            get
            {
                return this._EndDate;
            }
            set
            {
                if ((this._EndDate != value))
                {
                    this._EndDate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the dates diff.
        /// </summary>
        /// <value>The dates diff.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DatesDiff", DbType = "Int")]
        public System.Nullable<int> DatesDiff
        {
            get
            {
                return this._DatesDiff;
            }
            set
            {
                if ((this._DatesDiff != value))
                {
                    this._DatesDiff = value;
                }
            }
        }


        /// <summary>
        /// Gets or sets the rundate.
        /// </summary>
        /// <value>The rundate.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OriginalPurchaseDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> OriginalPurchaseDate
        {
            get
            {
                return this._OriginalPurchaseDate;
            }
            set
            {
                if ((this._OriginalPurchaseDate != value))
                {
                    this._OriginalPurchaseDate = value;
                }
            }
        }

        //Ankit20140304:Added Trade Attributes, Proceeds and Exchange Identifier


        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OpenTradeAttribute1", DbType = "VarChar(200)")]
        public string OpenTradeAttribute1
        {
            get
            {
                return this._OpenTradeAttribute1;
            }
            set
            {
                if ((this._OpenTradeAttribute1 != value))
                {
                    this._OpenTradeAttribute1 = value;
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OpenTradeAttribute2", DbType = "VarChar(200)")]
        public string OpenTradeAttribute2
        {
            get
            {
                return this._OpenTradeAttribute2;
            }
            set
            {
                if ((this._OpenTradeAttribute2 != value))
                {
                    this._OpenTradeAttribute2 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OpenTradeAttribute3", DbType = "VarChar(200)")]
        public string OpenTradeAttribute3
        {
            get
            {
                return this._OpenTradeAttribute3;
            }
            set
            {
                if ((this._OpenTradeAttribute3 != value))
                {
                    this._OpenTradeAttribute3 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OpenTradeAttribute4", DbType = "VarChar(200)")]
        public string OpenTradeAttribute4
        {
            get
            {
                return this._OpenTradeAttribute4;
            }
            set
            {
                if ((this._OpenTradeAttribute4 != value))
                {
                    this._OpenTradeAttribute4 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OpenTradeAttribute5", DbType = "VarChar(200)")]
        public string OpenTradeAttribute5
        {
            get
            {
                return this._OpenTradeAttribute5;
            }
            set
            {
                if ((this._OpenTradeAttribute5 != value))
                {
                    this._OpenTradeAttribute5 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OpenTradeAttribute6", DbType = "VarChar(200)")]
        public string OpenTradeAttribute6
        {
            get
            {
                return this._OpenTradeAttribute6;
            }
            set
            {
                if ((this._OpenTradeAttribute6 != value))
                {
                    this._OpenTradeAttribute6 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ClosedTradeAttribute1", DbType = "VarChar(200)")]
        public string ClosedTradeAttribute1
        {
            get
            {
                return this._ClosedTradeAttribute1;
            }
            set
            {
                if ((this._ClosedTradeAttribute1 != value))
                {
                    this._ClosedTradeAttribute1 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ClosedTradeAttribute2", DbType = "VarChar(200)")]
        public string ClosedTradeAttribute2
        {
            get
            {
                return this._ClosedTradeAttribute2;
            }
            set
            {
                if ((this._ClosedTradeAttribute2 != value))
                {
                    this._ClosedTradeAttribute2 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ClosedTradeAttribute3", DbType = "VarChar(200)")]
        public string ClosedTradeAttribute3
        {
            get
            {
                return this._ClosedTradeAttribute3;
            }
            set
            {
                if ((this._ClosedTradeAttribute3 != value))
                {
                    this._ClosedTradeAttribute3 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ClosedTradeAttribute4", DbType = "VarChar(200)")]
        public string ClosedTradeAttribute4
        {
            get
            {
                return this._ClosedTradeAttribute4;
            }
            set
            {
                if ((this._ClosedTradeAttribute4 != value))
                {
                    this._ClosedTradeAttribute4 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ClosedTradeAttribute5", DbType = "VarChar(200)")]
        public string ClosedTradeAttribute5
        {
            get
            {
                return this._ClosedTradeAttribute5;
            }
            set
            {
                if ((this._ClosedTradeAttribute5 != value))
                {
                    this._ClosedTradeAttribute5 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ClosedTradeAttribute6", DbType = "VarChar(200)")]
        public string ClosedTradeAttribute6
        {
            get
            {
                return this._ClosedTradeAttribute6;
            }
            set
            {
                if ((this._ClosedTradeAttribute6 != value))
                {
                    this._ClosedTradeAttribute6 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TotalProceeds_Local", DbType = "Float Not NULL")]
        public double TotalProceeds_Local
        {
            get
            {
                return this._TotalProceeds_Local;
            }
            set
            {
                if ((this._TotalProceeds_Local != value))
                {
                    this._TotalProceeds_Local = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TotalProceeds_Base", DbType = "Float Not NULL")]
        public double TotalProceeds_Base
        {
            get
            {
                return this._TotalProceeds_Base;
            }
            set
            {
                if ((this._TotalProceeds_Base != value))
                {
                    this._TotalProceeds_Base = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ExchangeIdentifier", DbType = "VarChar(50)")]
        public string ExchangeIdentifier
        {
            get
            {
                return this._ExchangeIdentifier;
            }
            set
            {
                if ((this._ExchangeIdentifier != value))
                {
                    this._ExchangeIdentifier = value;
                }
            }
        }

        //Ankit20140513: Added leveraged factor and Transaction Source
        
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LeveragedFactor", DbType = "Float Not NULL")]
        public double LeveragedFactor
        {
            get
            {
                return this._LeveragedFactor;
            }
            set
            {
                if ((this._LeveragedFactor != value))
                {
                    this._LeveragedFactor = value;
                }
            }
        }

        //[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OpenTransactionSource", DbType = "VarChar(50)")]
        //public string OpenTransactionSource
        //{
        //    get
        //    {
        //        return this._OpenTransactionSource;
        //    }
        //    set
        //    {
        //        if ((this._OpenTransactionSource != value))
        //        {
        //            this._OpenTransactionSource = value;
        //        }
        //    }
        //}

        //[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ClosedTransactionSource", DbType = "VarChar(50)")]
        //public string ClosedTransactionSource
        //{
        //    get
        //    {
        //        return this._ClosedTransactionSource;
        //    }
        //    set
        //    {
        //        if ((this._ClosedTransactionSource != value))
        //        {
        //            this._ClosedTransactionSource = value;
        //        }
        //    }
        //}

        ////Sandeep Singh[4 April 2015]: BaseCurrency,SettlCurrency,SettlCurrFxRate,SettlCurrAmt

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_BaseCurrency", DbType = "VarChar(10)")]
        public string BaseCurrency
        {
            get
            {
                return this._BaseCurrency;
            }
            set
            {
                if ((this._BaseCurrency != value))
                {
                    this._BaseCurrency = value;
                }
            }
        }


        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SettlCurrency", DbType = "VarChar(10)")]
        public string SettlCurrency
        {
            get
            {
                return this._SettlCurrency;
            }
            set
            {
                if ((this._SettlCurrency != value))
                {
                    this._SettlCurrency = value;
                }
            }
        }


        /// <summary>
        /// Gets or sets settlement Currency FX Rate.
        /// </summary>
        /// <value>Settlement Currency FX Rate.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SettlCurrFxRate", DbType = "Float")]
        public System.Nullable<double> SettlCurrFxRate
        {
            get
            {
                return this._SettlCurrFxRate;
            }
            set
            {
                if ((this._SettlCurrFxRate != value))
                {
                    this._SettlCurrFxRate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets settlement Currency amount.
        /// </summary>
        /// <value>Settlement Currency amount.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SettlCurrAmt", DbType = "Float")]
        public System.Nullable<double> SettlCurrAmt
        {
            get
            {
                return this._SettlCurrAmt;
            }
            set
            {
                if ((this._SettlCurrAmt != value))
                {
                    this._SettlCurrAmt = value;
                }
            }
        }


        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RiskCurrency", DbType = "VarChar(100)")]
        public string RiskCurrency
        {
            get
            {
                return this._RiskCurrency;
            }
            set
            {
                if ((this._RiskCurrency != value))
                {
                    this._RiskCurrency = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Issuer", DbType = "VarChar(100)")]
        public string Issuer
        {
            get
            {
                return this._Issuer;
            }
            set
            {
                if ((this._Issuer != value))
                {
                    this._Issuer = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CountryOfRisk", DbType = "VarChar(100)")]
        public string CountryOfRisk
        {
            get
            {
                return this._CountryOfRisk;
            }
            set
            {
                if ((this._CountryOfRisk != value))
                {
                    this._CountryOfRisk = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Region", DbType = "VarChar(100)")]
        public string Region
        {
            get
            {
                return this._Region;
            }
            set
            {
                if ((this._Region != value))
                {
                    this._Region = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Analyst", DbType = "VarChar(100)")]
        public string Analyst
        {
            get
            {
                return this._Analyst;
            }
            set
            {
                if ((this._Analyst != value))
                {
                    this._Analyst = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UCITSEligibleTag", DbType = "VarChar(100)")]
        public string UCITSEligibleTag
        {
            get
            {
                return this._UCITSEligibleTag;
            }
            set
            {
                if ((this._UCITSEligibleTag != value))
                {
                    this._UCITSEligibleTag = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LiquidTag", DbType = "VarChar(100)")]
        public string LiquidTag
        {
            get
            {
                return this._LiquidTag;
            }
            set
            {
                if ((this._LiquidTag != value))
                {
                    this._LiquidTag = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_MarketCap", DbType = "VarChar(100)")]
        public string MarketCap
        {
            get
            {
                return this._MarketCap;
            }
            set
            {
                if ((this._MarketCap != value))
                {
                    this._MarketCap = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CustomUDA1", DbType = "VarChar(100)")]
        public string CustomUDA1
        {
            get
            {
                return this._CustomUDA1;
            }
            set
            {
                if ((this._CustomUDA1 != value))
                {
                    this._CustomUDA1 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CustomUDA2", DbType = "VarChar(100)")]
        public string CustomUDA2
        {
            get
            {
                return this._CustomUDA2;
            }
            set
            {
                if ((this._CustomUDA2 != value))
                {
                    this._CustomUDA2 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CustomUDA3", DbType = "VarChar(100)")]
        public string CustomUDA3
        {
            get
            {
                return this._CustomUDA3;
            }
            set
            {
                if ((this._CustomUDA3 != value))
                {
                    this._CustomUDA3 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CustomUDA4", DbType = "VarChar(100)")]
        public string CustomUDA4
        {
            get
            {
                return this._CustomUDA4;
            }
            set
            {
                if ((this._CustomUDA4 != value))
                {
                    this._CustomUDA4 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CustomUDA5", DbType = "VarChar(100)")]
        public string CustomUDA5
        {
            get
            {
                return this._CustomUDA5;
            }
            set
            {
                if ((this._CustomUDA5 != value))
                {
                    this._CustomUDA5 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CustomUDA6", DbType = "VarChar(100)")]
        public string CustomUDA6
        {
            get
            {
                return this._CustomUDA6;
            }
            set
            {
                if ((this._CustomUDA6 != value))
                {
                    this._CustomUDA6 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CustomUDA7", DbType = "VarChar(100)")]
        public string CustomUDA7
        {
            get
            {
                return this._CustomUDA7;
            }
            set
            {
                if ((this._CustomUDA7 != value))
                {
                    this._CustomUDA7 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SharesOutstanding", DbType = "Float")]
        public System.Nullable<double> SharesOutstanding
        {
            get
            {
                return this._SharesOutstanding;
            }
            set
            {
                if ((this._SharesOutstanding != value))
                {
                    this._SharesOutstanding = value;
                }
            }
        }
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UnderlyingSymbolCompanyName", DbType = "VarChar(100)")]
        public string UnderlyingSymbolCompanyName
        {
            get
            {
                return this._UnderlyingSymbolCompanyName;
            }
            set
            {
                if ((this._UnderlyingSymbolCompanyName != value))
                {
                    this._UnderlyingSymbolCompanyName = value;
                }
            }
        }

    }
}
