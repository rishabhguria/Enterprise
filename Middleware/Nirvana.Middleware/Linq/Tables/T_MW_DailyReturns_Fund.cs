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
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.T_MW_DailyReturns_Fund")]
    public partial class T_MW_DailyReturns_Fund
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
        private double _TTWRplus1;
        /// <summary>
        /// 
        /// </summary>
        private double _TTWR;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public T_MW_DailyReturns_Fund()
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
    }
}
