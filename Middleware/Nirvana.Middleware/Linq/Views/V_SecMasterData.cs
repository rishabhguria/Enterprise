#pragma warning disable 1591

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

    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.V_SecMasterData")]
    public partial class V_SecMasterData
    {

        private System.Nullable<int> _AUECID;

        private string _TickerSymbol;

        private string _CompanyName;

        private double _Delta;

        private string _AssetName;

        private string _SecurityTypeName;

        private string _SectorName;

        private string _SubSectorName;

        private string _CountryName;

        private string _UnderLyingSymbol;

        private string _BloombergSymbol;

        private string _ISINSymbol;

        private string _SEDOLSymbol;

        private string _CUSIPSymbol;

        private string _PutOrCall;

        private System.Nullable<double> _StrikePrice;

        private System.Nullable<double> _Multiplier;

        private string _ReutersSymbol;

        private System.Nullable<System.DateTime> _ExpirationDate;

        private int _LeadCurrencyID;

        private int _VsCurrencyID;

        private string _LeadCurrency;

        private string _VsCurrency;

        private System.Nullable<int> _CurrencyID;

        private string _OSISymbol;

        private string _IDCOSymbol;

        private string _OpraSymbol;

        private System.Nullable<int> _AssetId;

        private System.Nullable<double> _Coupon;

        private System.Nullable<System.DateTime> _IssueDate;

        private System.Nullable<System.DateTime> _MaturityDate;

        private System.Nullable<System.DateTime> _FirstCouponDate;

        private System.Nullable<int> _CouponFrequencyID;

        private System.Nullable<int> _AccrualBasisID;

        private System.Nullable<int> _BondTypeID;

        private System.Nullable<int> _IsZero;

        private int _IsNDF;

        private System.DateTime _FixingDate;

        private double _UnderlyingDelta;

        public V_SecMasterData()
        {
        }

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

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TickerSymbol", DbType = "VarChar(100) NOT NULL", CanBeNull = false)]
        public string TickerSymbol
        {
            get
            {
                return this._TickerSymbol;
            }
            set
            {
                if ((this._TickerSymbol != value))
                {
                    this._TickerSymbol = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CompanyName", DbType = "VarChar(500)")]
        public string CompanyName
        {
            get
            {
                return this._CompanyName;
            }
            set
            {
                if ((this._CompanyName != value))
                {
                    this._CompanyName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Delta", DbType = "Float NOT NULL")]
        public double Delta
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

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_AssetName", DbType = "VarChar(20)")]
        public string AssetName
        {
            get
            {
                return this._AssetName;
            }
            set
            {
                if ((this._AssetName != value))
                {
                    this._AssetName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SecurityTypeName", DbType = "NVarChar(20)")]
        public string SecurityTypeName
        {
            get
            {
                return this._SecurityTypeName;
            }
            set
            {
                if ((this._SecurityTypeName != value))
                {
                    this._SecurityTypeName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SectorName", DbType = "VarChar(30)")]
        public string SectorName
        {
            get
            {
                return this._SectorName;
            }
            set
            {
                if ((this._SectorName != value))
                {
                    this._SectorName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SubSectorName", DbType = "VarChar(50)")]
        public string SubSectorName
        {
            get
            {
                return this._SubSectorName;
            }
            set
            {
                if ((this._SubSectorName != value))
                {
                    this._SubSectorName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CountryName", DbType = "VarChar(20)")]
        public string CountryName
        {
            get
            {
                return this._CountryName;
            }
            set
            {
                if ((this._CountryName != value))
                {
                    this._CountryName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UnderLyingSymbol", DbType = "VarChar(20)")]
        public string UnderLyingSymbol
        {
            get
            {
                return this._UnderLyingSymbol;
            }
            set
            {
                if ((this._UnderLyingSymbol != value))
                {
                    this._UnderLyingSymbol = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_BloombergSymbol", DbType = "VarChar(200)")]
        public string BloombergSymbol
        {
            get
            {
                return this._BloombergSymbol;
            }
            set
            {
                if ((this._BloombergSymbol != value))
                {
                    this._BloombergSymbol = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ISINSymbol", DbType = "VarChar(20)")]
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

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SEDOLSymbol", DbType = "VarChar(20)")]
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

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CUSIPSymbol", DbType = "VarChar(20)")]
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

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PutOrCall", DbType = "VarChar(4) NOT NULL", CanBeNull = false)]
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

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ReutersSymbol", DbType = "VarChar(20)")]
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

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LeadCurrencyID", DbType = "Int NOT NULL")]
        public int LeadCurrencyID
        {
            get
            {
                return this._LeadCurrencyID;
            }
            set
            {
                if ((this._LeadCurrencyID != value))
                {
                    this._LeadCurrencyID = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_VsCurrencyID", DbType = "Int NOT NULL")]
        public int VsCurrencyID
        {
            get
            {
                return this._VsCurrencyID;
            }
            set
            {
                if ((this._VsCurrencyID != value))
                {
                    this._VsCurrencyID = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LeadCurrency", DbType = "NVarChar(4) NOT NULL", CanBeNull = false)]
        public string LeadCurrency
        {
            get
            {
                return this._LeadCurrency;
            }
            set
            {
                if ((this._LeadCurrency != value))
                {
                    this._LeadCurrency = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_VsCurrency", DbType = "NVarChar(4) NOT NULL", CanBeNull = false)]
        public string VsCurrency
        {
            get
            {
                return this._VsCurrency;
            }
            set
            {
                if ((this._VsCurrency != value))
                {
                    this._VsCurrency = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CurrencyID", DbType = "Int")]
        public System.Nullable<int> CurrencyID
        {
            get
            {
                return this._CurrencyID;
            }
            set
            {
                if ((this._CurrencyID != value))
                {
                    this._CurrencyID = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OSISymbol", DbType = "VarChar(25)")]
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

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IDCOSymbol", DbType = "VarChar(25)")]
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

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OpraSymbol", DbType = "VarChar(20)")]
        public string OpraSymbol
        {
            get
            {
                return this._OpraSymbol;
            }
            set
            {
                if ((this._OpraSymbol != value))
                {
                    this._OpraSymbol = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_AssetId", DbType = "Int")]
        public System.Nullable<int> AssetId
        {
            get
            {
                return this._AssetId;
            }
            set
            {
                if ((this._AssetId != value))
                {
                    this._AssetId = value;
                }
            }
        }

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

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CouponFrequencyID", DbType = "Int")]
        public System.Nullable<int> CouponFrequencyID
        {
            get
            {
                return this._CouponFrequencyID;
            }
            set
            {
                if ((this._CouponFrequencyID != value))
                {
                    this._CouponFrequencyID = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_AccrualBasisID", DbType = "Int")]
        public System.Nullable<int> AccrualBasisID
        {
            get
            {
                return this._AccrualBasisID;
            }
            set
            {
                if ((this._AccrualBasisID != value))
                {
                    this._AccrualBasisID = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_BondTypeID", DbType = "Int")]
        public System.Nullable<int> BondTypeID
        {
            get
            {
                return this._BondTypeID;
            }
            set
            {
                if ((this._BondTypeID != value))
                {
                    this._BondTypeID = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IsZero", DbType = "Int")]
        public System.Nullable<int> IsZero
        {
            get
            {
                return this._IsZero;
            }
            set
            {
                if ((this._IsZero != value))
                {
                    this._IsZero = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IsNDF", DbType = "Int NOT NULL")]
        public int IsNDF
        {
            get
            {
                return this._IsNDF;
            }
            set
            {
                if ((this._IsNDF != value))
                {
                    this._IsNDF = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FixingDate", DbType = "DateTime NOT NULL")]
        public System.DateTime FixingDate
        {
            get
            {
                return this._FixingDate;
            }
            set
            {
                if ((this._FixingDate != value))
                {
                    this._FixingDate = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UnderlyingDelta", DbType = "Float NOT NULL")]
        public double UnderlyingDelta
        {
            get
            {
                return this._UnderlyingDelta;
            }
            set
            {
                if ((this._UnderlyingDelta != value))
                {
                    this._UnderlyingDelta = value;
                }
            }
        }
    }
}
