// ***********************************************************************
// Assembly         : Nirvana.Middleware
// Author           : MJCarlucci
// Created          : 04-29-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 04-29-2013
// ***********************************************************************
// <copyright file="T_MW_Transaction.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// Table_MiddleWare_Transaction
    /// </summary>
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.T_MW_Transactions")]
    public partial class T_MW_Transaction
    {

        /// <summary>
        /// The _ run date
        /// </summary>
        private System.Nullable<System.DateTime> _RunDate;

        /// <summary>
        /// The _ symbol
        /// </summary>
        private string _Symbol;

        /// <summary>
        /// The _ underlying symbol
        /// </summary>
        private string _UnderlyingSymbol;

        /// <summary>
        /// The _ strategy
        /// </summary>
        private string _Strategy;

        /// <summary>
        /// The _ master fund
        /// </summary>
        private string _MasterFund;

        /// <summary>
        /// The _ fund
        /// </summary>
        private string _Fund;

        /// <summary>
        /// The _ currency
        /// </summary>
        private string _Currency;

        /// <summary>
        /// The _ asset
        /// </summary>
        private string _Asset;

        /// <summary>
        /// The _ underlyer
        /// </summary>
        private string _Underlyer;

        /// <summary>
        /// The _ exchange
        /// </summary>
        private string _Exchange;

        /// <summary>
        /// The _ UDA sector
        /// </summary>
        private string _UDASector;

        /// <summary>
        /// The _ UDA country
        /// </summary>
        private string _UDACountry;

        /// <summary>
        /// The _ UDA security type
        /// </summary>
        private string _UDASecurityType;

        /// <summary>
        /// The _ UDA asset class
        /// </summary>
        private string _UDAAssetClass;

        /// <summary>
        /// The _ UDA sub sector
        /// </summary>
        private string _UDASubSector;

        /// <summary>
        /// The _ trade currency
        /// </summary>
        private string _TradeCurrency;

        /// <summary>
        /// The _ side
        /// </summary>
        private string _Side;

        /// <summary>
        /// The _ counter party
        /// </summary>
        private string _CounterParty;

        /// <summary>
        /// The _ prime broker
        /// </summary>
        private string _PrimeBroker;

        /// <summary>
        /// The _ trader
        /// </summary>
        private string _Trader;

        /// <summary>
        /// The _ security name
        /// </summary>
        private string _SecurityName;

        /// <summary>
        /// The _ trade date
        /// </summary>
        private System.Nullable<System.DateTime> _TradeDate;

        /// <summary>
        /// The _ settle date
        /// </summary>
        private System.Nullable<System.DateTime> _SettleDate;

        /// <summary>
        /// The _ quantity
        /// </summary>
        private System.Nullable<double> _Quantity;

        /// <summary>
        /// The _ multiplier
        /// </summary>
        private System.Nullable<double> _Multiplier;

        /// <summary>
        /// The _ side multiplier
        /// </summary>
        private string _SideMultiplier;

        /// <summary>
        /// The _ avg price
        /// </summary>
        private System.Nullable<double> _AvgPrice;

        /// <summary>
        /// The _ put or call
        /// </summary>
        private string _PutOrCall;

        /// <summary>
        /// The _ is swapped
        /// </summary>
        private System.Nullable<bool> _IsSwapped;

        /// <summary>
        /// The _ commission local
        /// </summary>
        private System.Nullable<double> _CommissionLocal;

        /// <summary>
        /// The _ commission base
        /// </summary>
        private System.Nullable<double> _CommissionBase;

        /// <summary>
        /// The _ fees local
        /// </summary>
        private System.Nullable<double> _FeesLocal;

        /// <summary>
        /// The _ fees base
        /// </summary>
        private System.Nullable<double> _FeesBase;

        /// <summary>
        /// The _ other fees local
        /// </summary>
        private System.Nullable<double> _OtherFeesLocal;

        /// <summary>
        /// The _ other fees base
        /// </summary>
        private System.Nullable<double> _OtherFeesBase;

        /// <summary>
        /// The _ Option Premium Adjustment
        /// </summary>
        private System.Nullable<double> _OptionPremiumAdjustment;

        /// <summary>
        /// The _ Option Premium AdjustmentBase
        /// </summary>
        private System.Nullable<double> _OptionPremiumAdjustmentBase;

        /// <summary>
        /// The _ FX rate_ trade date
        /// </summary>
        private System.Nullable<double> _FXRate_TradeDate;

        /// <summary>
        /// The _ mark FX rate_ trade date
        /// </summary>
        private System.Nullable<double> _MarkFXRate_TradeDate;

        /// <summary>
        /// The _ mark FX rate_ settle date
        /// </summary>
        private System.Nullable<double> _MarkFXRate_SettleDate;

        /// <summary>
        /// The _ net amount base
        /// </summary>
        private System.Nullable<double> _NetAmountBase;

        /// <summary>
        /// The _ net amount local
        /// </summary>
        private System.Nullable<double> _NetAmountLocal;

        /// <summary>
        /// The _ principal amount base
        /// </summary>
        private System.Nullable<double> _PrincipalAmountBase;

        /// <summary>
        /// The _ principal amount local
        /// </summary>
        private System.Nullable<double> _PrincipalAmountLocal;

        /// <summary>
        /// The _ trade origin
        /// </summary>
        private string _TradeOrigin;

        /// <summary>
        /// The _ open_ close tag
        /// </summary>
        private string _Open_CloseTag;

        /// <summary>
        /// The _ dividend local
        /// </summary>
        private System.Nullable<double> _DividendLocal;

        /// <summary>
        /// The _ dividend
        /// </summary>
        private System.Nullable<double> _Dividend;

        /// <summary>
        /// The _ bloom berg symbol
        /// </summary>
        private string _BloomBergSymbol;

        /// <summary>
        /// The _ sedol symbol
        /// </summary>
        private string _SedolSymbol;

        /// <summary>
        /// The _ OSI symbol
        /// </summary>
        private string _OSISymbol;

        /// <summary>
        /// The _ IDCO symbol
        /// </summary>
        private string _IDCOSymbol;

        /// <summary>
        /// The _ ISIN symbol
        /// </summary>
        private string _ISINSymbol;

        /// <summary>
        /// The _ sub account ID
        /// </summary>
        private System.Nullable<int> _SubAccountID;


        //Ankit20130807:Added 3 new columns

        private string _Descriptions;
                      
        private System.Nullable<System.DateTime> _ProcessDate;

        private string _GroupID;


        /// <summary>
        /// The _ CUSIPSymbol symbol
        /// </summary>
        private string _CUSIPSymbol;

        /// <summary>
        /// The _ ReutersSymbol symbol
        /// </summary>
        private string _ReutersSymbol;

        //Ankit20140304: Added Trade Attributes and Exchange Identifier

        private string _TradeAttribute1;

        private string _TradeAttribute2;

        private string _TradeAttribute3;
                
        private string _TradeAttribute4;
        
        private string _TradeAttribute5;
        
        private string _TradeAttribute6;

        private string _ExchangeIdentifier;

        //Ankit20140506: Added Transaction source and its acronym

        //private string _TransactionSourceAcronym;

        //private string _TransactionSourceName;

        private string _TransactionType;

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

        private string _InternalComments;
		
		private string _UnderlyingSymbolCompanyName;

        /// <summary>
        /// The _ ExpirationDate 
        /// </summary>
        private System.Nullable<System.DateTime> _ExpirationDate;


        /// <summary>
        /// Initializes a new instance of the <see cref="T_MW_Transaction"/> class.
        /// </summary>
        public T_MW_Transaction()
        {
        }

        /// <summary>
        /// Gets or sets the run date.
        /// </summary>
        /// <value>The run date.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RunDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> RunDate
        {
            get
            {
                return this._RunDate;
            }
            set
            {
                if ((this._RunDate != value))
                {
                    this._RunDate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>The symbol.</value>
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
        /// Gets or sets the strategy.
        /// </summary>
        /// <value>The strategy.</value>
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
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
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
        /// Gets or sets the asset.
        /// </summary>
        /// <value>The asset.</value>
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
        /// Gets or sets the UDA sector.
        /// </summary>
        /// <value>The UDA sector.</value>
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
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Side", DbType = "VarChar(20)")]
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
        /// Gets or sets the counter party.
        /// </summary>
        /// <value>The counter party.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CounterParty", DbType = "VarChar(100)")]
        public string CounterParty
        {
            get
            {
                return this._CounterParty;
            }
            set
            {
                if ((this._CounterParty != value))
                {
                    this._CounterParty = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the prime broker.
        /// </summary>
        /// <value>The prime broker.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PrimeBroker", DbType = "VarChar(100)")]
        public string PrimeBroker
        {
            get
            {
                return this._PrimeBroker;
            }
            set
            {
                if ((this._PrimeBroker != value))
                {
                    this._PrimeBroker = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the trader.
        /// </summary>
        /// <value>The trader.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Trader", DbType = "VarChar(100)")]
        public string Trader
        {
            get
            {
                return this._Trader;
            }
            set
            {
                if ((this._Trader != value))
                {
                    this._Trader = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the security.
        /// </summary>
        /// <value>The name of the security.</value>
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
        /// Gets or sets the trade date.
        /// </summary>
        /// <value>The trade date.</value>
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
        /// Gets or sets the settle date.
        /// </summary>
        /// <value>The settle date.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SettleDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> SettleDate
        {
            get
            {
                return this._SettleDate;
            }
            set
            {
                if ((this._SettleDate != value))
                {
                    this._SettleDate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Quantity", DbType = "Float")]
        public System.Nullable<double> Quantity
        {
            get
            {
                return this._Quantity;
            }
            set
            {
                if ((this._Quantity != value))
                {
                    this._Quantity = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the multiplier.
        /// </summary>
        /// <value>The multiplier.</value>
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
        /// Gets or sets the avg price.
        /// </summary>
        /// <value>The avg price.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_AvgPrice", DbType = "Float")]
        public System.Nullable<double> AvgPrice
        {
            get
            {
                return this._AvgPrice;
            }
            set
            {
                if ((this._AvgPrice != value))
                {
                    this._AvgPrice = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the put or call.
        /// </summary>
        /// <value>The put or call.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PutOrCall", DbType = "VarChar(5)")]
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
        /// Gets or sets the is swapped.
        /// </summary>
        /// <value>The is swapped.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IsSwapped", DbType = "Bit")]
        public System.Nullable<bool> IsSwapped
        {
            get
            {
                return this._IsSwapped;
            }
            set
            {
                if ((this._IsSwapped != value))
                {
                    this._IsSwapped = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the commission local.
        /// </summary>
        /// <value>The commission local.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CommissionLocal", DbType = "Float")]
        public System.Nullable<double> CommissionLocal
        {
            get
            {
                return this._CommissionLocal;
            }
            set
            {
                if ((this._CommissionLocal != value))
                {
                    this._CommissionLocal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the commission base.
        /// </summary>
        /// <value>The commission base.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CommissionBase", DbType = "Float")]
        public System.Nullable<double> CommissionBase
        {
            get
            {
                return this._CommissionBase;
            }
            set
            {
                if ((this._CommissionBase != value))
                {
                    this._CommissionBase = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the fees local.
        /// </summary>
        /// <value>The fees local.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FeesLocal", DbType = "Float")]
        public System.Nullable<double> FeesLocal
        {
            get
            {
                return this._FeesLocal;
            }
            set
            {
                if ((this._FeesLocal != value))
                {
                    this._FeesLocal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the fees base.
        /// </summary>
        /// <value>The fees base.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FeesBase", DbType = "Float")]
        public System.Nullable<double> FeesBase
        {
            get
            {
                return this._FeesBase;
            }
            set
            {
                if ((this._FeesBase != value))
                {
                    this._FeesBase = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the other fees local.
        /// </summary>
        /// <value>The other fees local.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OtherFeesLocal", DbType = "Float")]
        public System.Nullable<double> OtherFeesLocal
        {
            get
            {
                return this._OtherFeesLocal;
            }
            set
            {
                if ((this._OtherFeesLocal != value))
                {
                    this._OtherFeesLocal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the other fees base.
        /// </summary>
        /// <value>The other fees base.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OtherFeesBase", DbType = "Float")]
        public System.Nullable<double> OtherFeesBase
        {
            get
            {
                return this._OtherFeesBase;
            }
            set
            {
                if ((this._OtherFeesBase != value))
                {
                    this._OtherFeesBase = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets Option Premium Adjustment.
        /// </summary>
        /// <value>The Option Premium Adjustment.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OptionPremiumAdjustment", DbType = "Float")]
        public System.Nullable<double> OptionPremiumAdjustment
        {
            get
            {
                return this._OptionPremiumAdjustment;
            }
            set
            {
                if ((this._OptionPremiumAdjustment != value))
                {
                    this._OptionPremiumAdjustment = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets Option Premium AdjustmentBase.
        /// </summary>
        /// <value>The Option Premium Adjustment Base.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OptionPremiumAdjustmentBase", DbType = "Float")]
        public System.Nullable<double> OptionPremiumAdjustmentBase
        {
            get
            {
                return this._OptionPremiumAdjustmentBase;
            }
            set
            {
                if ((this._OptionPremiumAdjustmentBase != value))
                {
                    this._OptionPremiumAdjustmentBase = value;
                }
            }
        }


        /// <summary>
        /// Gets or sets the FX rate_ trade date.
        /// </summary>
        /// <value>The FX rate_ trade date.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FXRate_TradeDate", DbType = "Float")]
        public System.Nullable<double> FXRate_TradeDate
        {
            get
            {
                return this._FXRate_TradeDate;
            }
            set
            {
                if ((this._FXRate_TradeDate != value))
                {
                    this._FXRate_TradeDate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the mark FX rate_ trade date.
        /// </summary>
        /// <value>The mark FX rate_ trade date.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_MarkFXRate_TradeDate", DbType = "Float")]
        public System.Nullable<double> MarkFXRate_TradeDate
        {
            get
            {
                return this._MarkFXRate_TradeDate;
            }
            set
            {
                if ((this._MarkFXRate_TradeDate != value))
                {
                    this._MarkFXRate_TradeDate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the mark FX rate_ settle date.
        /// </summary>
        /// <value>The mark FX rate_ settle date.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_MarkFXRate_SettleDate", DbType = "Float")]
        public System.Nullable<double> MarkFXRate_SettleDate
        {
            get
            {
                return this._MarkFXRate_SettleDate;
            }
            set
            {
                if ((this._MarkFXRate_SettleDate != value))
                {
                    this._MarkFXRate_SettleDate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the net amount base.
        /// </summary>
        /// <value>The net amount base.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_NetAmountBase", DbType = "Float")]
        public System.Nullable<double> NetAmountBase
        {
            get
            {
                return this._NetAmountBase;
            }
            set
            {
                if ((this._NetAmountBase != value))
                {
                    this._NetAmountBase = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the net amount local.
        /// </summary>
        /// <value>The net amount local.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_NetAmountLocal", DbType = "Float")]
        public System.Nullable<double> NetAmountLocal
        {
            get
            {
                return this._NetAmountLocal;
            }
            set
            {
                if ((this._NetAmountLocal != value))
                {
                    this._NetAmountLocal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the principal amount base.
        /// </summary>
        /// <value>The principal amount base.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PrincipalAmountBase", DbType = "Float")]
        public System.Nullable<double> PrincipalAmountBase
        {
            get
            {
                return this._PrincipalAmountBase;
            }
            set
            {
                if ((this._PrincipalAmountBase != value))
                {
                    this._PrincipalAmountBase = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the principal amount local.
        /// </summary>
        /// <value>The principal amount local.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PrincipalAmountLocal", DbType = "Float")]
        public System.Nullable<double> PrincipalAmountLocal
        {
            get
            {
                return this._PrincipalAmountLocal;
            }
            set
            {
                if ((this._PrincipalAmountLocal != value))
                {
                    this._PrincipalAmountLocal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the trade origin.
        /// </summary>
        /// <value>The trade origin.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TradeOrigin", DbType = "VarChar(100)")]
        public string TradeOrigin
        {
            get
            {
                return this._TradeOrigin;
            }
            set
            {
                if ((this._TradeOrigin != value))
                {
                    this._TradeOrigin = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the open_ close tag.
        /// </summary>
        /// <value>The open_ close tag.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Open_CloseTag", DbType = "VarChar(5)")]
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
        /// Gets or sets the dividend local.
        /// </summary>
        /// <value>The dividend local.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DividendLocal", DbType = "Float")]
        public System.Nullable<double> DividendLocal
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
        /// Gets or sets the bloom berg symbol.
        /// </summary>
        /// <value>The bloom berg symbol.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_BloomBergSymbol", DbType = "VarChar(100)")]
        public string BloomBergSymbol
        {
            get
            {
                return this._BloomBergSymbol;
            }
            set
            {
                if ((this._BloomBergSymbol != value))
                {
                    this._BloomBergSymbol = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the sedol symbol.
        /// </summary>
        /// <value>The sedol symbol.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SedolSymbol", DbType = "VarChar(50)")]
        public string SedolSymbol
        {
            get
            {
                return this._SedolSymbol;
            }
            set
            {
                if ((this._SedolSymbol != value))
                {
                    this._SedolSymbol = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the OSI symbol.
        /// </summary>
        /// <value>The OSI symbol.</value>
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
        /// Gets or sets the IDCO symbol.
        /// </summary>
        /// <value>The IDCO symbol.</value>
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
        /// Gets or sets the ISIN symbol.
        /// </summary>
        /// <value>The ISIN symbol.</value>
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
        /// Gets or sets the sub account ID.
        /// </summary>
        /// <value>The sub account ID.</value>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SubAccountID", DbType = "Int")]
        public System.Nullable<int> SubAccountID
        {
            get
            {
                return this._SubAccountID;
            }
            set
            {
                if ((this._SubAccountID != value))
                {
                    this._SubAccountID = value;
                }
            }
        }


        //Ankit20130807:Added 3 new columns        

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Descriptions", DbType = "VarChar(500)")]
        public string Descriptions
        {
            get
            {
                return this._Descriptions;
            }
            set
            {
                if ((this._Descriptions != value))
                {
                    this._Descriptions = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ProcessDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> ProcessDate
        {
            get
            {
                return this._ProcessDate;
            }
            set
            {
                if ((this._ProcessDate != value))
                {
                    this._ProcessDate = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_GroupID", DbType = "VarChar(50)")]
        public string GroupID
        {
            get
            {
                return this._GroupID;
            }
            set
            {
                if ((this._GroupID != value))
                {
                    this._GroupID = value;
                }
            }
        }

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

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ReutersSymbol", DbType = "VarChar(50)")]
        public string ReutersSymbol
        {
            get
            {
                return this._ReutersSymbol;
            }
            set
            {
                if ((this._ReutersSymbol != value))
                {
                    this._ReutersSymbol = value;
                }
            }
        }

        //Ankit20140304: Added Trade Attributes and Exchange Identifier

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TradeAttribute1", DbType = "VarChar(200)")]
        public string TradeAttribute1
        {
            get
            {
                return this._TradeAttribute1;
            }
            set
            {
                if ((this._TradeAttribute1 != value))
                {
                    this._TradeAttribute1 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TradeAttribute2", DbType = "VarChar(200)")]
        public string TradeAttribute2
        {
            get
            {
                return this._TradeAttribute2;
            }
            set
            {
                if ((this._TradeAttribute2 != value))
                {
                    this._TradeAttribute2 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TradeAttribute3", DbType = "VarChar(200)")]
        public string TradeAttribute3
        {
            get
            {
                return this._TradeAttribute3;
            }
            set
            {
                if ((this._TradeAttribute3 != value))
                {
                    this._TradeAttribute3 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TradeAttribute4", DbType = "VarChar(200)")]
        public string TradeAttribute4
        {
            get
            {
                return this._TradeAttribute4;
            }
            set
            {
                if ((this._TradeAttribute4 != value))
                {
                    this._TradeAttribute4 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TradeAttribute5", DbType = "VarChar(200)")]
        public string TradeAttribute5
        {
            get
            {
                return this._TradeAttribute5;
            }
            set
            {
                if ((this._TradeAttribute5 != value))
                {
                    this._TradeAttribute5 = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TradeAttribute6", DbType = "VarChar(200)")]
        public string TradeAttribute6
        {
            get
            {
                return this._TradeAttribute6;
            }
            set
            {
                if ((this._TradeAttribute6 != value))
                {
                    this._TradeAttribute6 = value;
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

        //Ankit20140506: Added Transaction source and its acronym

        //[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TransactionSourceAcronym", DbType = "VarChar(50)")]
        //public string TransactionSourceAcronym
        //{
        //    get
        //    {
        //        return this._TransactionSourceAcronym;
        //    }
        //    set
        //    {
        //        if ((this._TransactionSourceAcronym != value))
        //        {
        //            this._TransactionSourceAcronym = value;
        //        }
        //    }
        //}

        //[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TransactionSourceName", DbType = "VarChar(200)")]
        //public string TransactionSourceName
        //{
        //    get
        //    {
        //        return this._TransactionSourceName;
        //    }
        //    set
        //    {
        //        if ((this._TransactionSourceName != value))
        //        {
        //            this._TransactionSourceName = value;
        //        }
        //    }
        //}


        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TransactionType", DbType = "VarChar(200)")]
        public string TransactionType
        {
            get
            {
                return this._TransactionType;
            }
            set
            {
                if ((this._TransactionType != value))
                {
                    this._TransactionType = value;
                }
            }
        }


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
        /// <summary>
        /// _InternalComments
        /// </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_InternalComments", DbType = "VarChar(500)")]
        public string InternalComments
        {
            get
            {
                return this._InternalComments;
            }
            set
            {
                if ((this._InternalComments != value))
                {
                    this._InternalComments = value;
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


        /// <summary>
        /// Gets or sets the Expiration date.
        /// </summary>
        /// <value>The Expiration Date.</value>
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



    }
}
