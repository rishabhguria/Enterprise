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
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.T_MW_DailyReturns")]
    public partial class T_MW_DailyReturn
    {
        /// <summary>
        /// 
        /// </summary>
        private System.DateTime _rundate;
        /// <summary>
        /// 
        /// </summary>
        private double _adjBMV;
        /// <summary>
        /// 
        /// </summary>
        private double _adjEMV;
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
        private string _Symbol;
        /// <summary>
        /// 
        /// </summary>
        private string _acctSide;
        /// <summary>
        /// 
        /// </summary>
        private string _expSide;
        /// <summary>
        /// 
        /// </summary>
        private double _DailyWeight;
        /// <summary>
        /// 
        /// </summary>
        private double _TTWRplus1;
        /// <summary>
        /// 
        /// </summary>
        private double _TTWR;
        /// <summary>
        /// 
        /// </summary>
        private double _TTWRcont;
        /// <summary>
        /// 
        /// </summary>
        private bool _isPoorData;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public T_MW_DailyReturn()
        {
        }
        /// <summary>
        /// Gets or sets the rundate.
        /// </summary>
        /// <value>The rundate.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_rundate", DbType = "DateTime NOT NULL")]
        public System.DateTime rundate
        {
            get
            {
                return this._rundate;
            }
            set
            {
                if ((this._rundate != value))
                    this._rundate = value;
            }
        }
        /// <summary>
        /// Gets or sets the adj BMV.
        /// </summary>
        /// <value>The adj BMV.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_adjBMV", DbType = "Float NOT NULL")]
        public double adjBMV
        {
            get
            {
                return this._adjBMV;
            }
            set
            {
                if ((this._adjBMV != value))
                    this._adjBMV = value;
            }
        }
        /// <summary>
        /// Gets or sets the adj EMV.
        /// </summary>
        /// <value>The adj EMV.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_adjEMV", DbType = "Float NOT NULL")]
        public double adjEMV
        {
            get
            {
                return this._adjEMV;
            }
            set
            {
                if ((this._adjEMV != value))
                    this._adjEMV = value;
            }
        }
        /// <summary>
        /// Gets or sets the fund.
        /// </summary>
        /// <value>The fund.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Fund", DbType = "VarChar(200) NOT NULL", CanBeNull = false)]
        public string Fund
        {
            get
            {
                return this._Fund;
            }
            set
            {
                if ((this._Fund != value))
                    this._Fund = value;
            }
        }
        /// <summary>
        /// Gets or sets the asset.
        /// </summary>
        /// <value>The asset.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Asset", DbType = "VarChar(50) NOT NULL", CanBeNull = false)]
        public string Asset
        {
            get
            {
                return this._Asset;
            }
            set
            {
                if ((this._Asset != value))
                    this._Asset = value;
            }
        }
        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>The symbol.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Symbol", DbType = "VarChar(200) NOT NULL", CanBeNull = false)]
        public string Symbol
        {
            get
            {
                return this._Symbol;
            }
            set
            {
                if ((this._Symbol != value))
                    this._Symbol = value;
            }
        }
        /// <summary>
        /// Gets or sets the acct side.
        /// </summary>
        /// <value>The acct side.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_acctSide", DbType = "VarChar(5) NOT NULL", CanBeNull = false)]
        public string acctSide
        {
            get
            {
                return this._acctSide;
            }
            set
            {
                if ((this._acctSide != value))
                    this._acctSide = value;
            }
        }
        /// <summary>
        /// Gets or sets the exp side.
        /// </summary>
        /// <value>The exp side.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_expSide", DbType = "VarChar(5) NOT NULL", CanBeNull = false)]
        public string expSide
        {
            get
            {
                return this._expSide;
            }
            set
            {
                if ((this._expSide != value))
                    this._expSide = value;
            }
        }
        /// <summary>
        /// Gets or sets the daily weight.
        /// </summary>
        /// <value>The daily weight.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DailyWeight", DbType = "Float NOT NULL")]
        public double DailyWeight
        {
            get
            {
                return this._DailyWeight;
            }
            set
            {
                if ((this._DailyWeight != value))
                    this._DailyWeight = value;
            }
        }
        /// <summary>
        /// Gets or sets the TTW rplus1.
        /// </summary>
        /// <value>The TTW rplus1.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TTWRplus1", DbType = "Float NOT NULL")]
        public double TTWRplus1
        {
            get
            {
                return this._TTWRplus1;
            }
            set
            {
                if ((this._TTWRplus1 != value))
                    this._TTWRplus1 = value;
            }
        }
        /// <summary>
        /// Gets or sets the TTWR.
        /// </summary>
        /// <value>The TTWR.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TTWR", DbType = "Float NOT NULL")]
        public double TTWR
        {
            get
            {
                return this._TTWR;
            }
            set
            {
                if ((this._TTWR != value))
                    this._TTWR = value;
            }
        }
        /// <summary>
        /// Gets or sets the TTW rcont.
        /// </summary>
        /// <value>The TTW rcont.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TTWRcont", DbType = "Float NOT NULL")]
        public double TTWRcont
        {
            get
            {
                return this._TTWRcont;
            }
            set
            {
                if ((this._TTWRcont != value))
                    this._TTWRcont = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is poor data.
        /// </summary>
        /// <value><c>true</c> if this instance is poor data; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_isPoorData", DbType = "Bit NOT NULL")]
        public bool isPoorData
        {
            get
            {
                return this._isPoorData;
            }
            set
            {
                if ((this._isPoorData != value))
                    this._isPoorData = value;
            }
        }
    }
}
