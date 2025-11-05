using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Reflection;
using System.Linq.Expressions;
using System.ComponentModel;


namespace Nirvana.Middleware.Linq
{
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.PM_DailyDividendYield")]
    public partial class PM_DailyDividendYield
    {
        public PM_DailyDividendYield()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<int> _DayDividendYieldID;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<System.DateTime> _Date;

        /// <summary>
        /// 
        /// </summary>
        private string _Symbol;

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _DividendYield;

        /// <summary>
        /// Gets or sets the DayDividendYieldID.
        /// </summary>
        /// <value>The DayDividendYieldID.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DayDividendYieldID", DbType = "Int")]
        public System.Nullable<int> DayDividendYieldID
        {
            get
            {
                return this._DayDividendYieldID;
            }
            set
            {
                if ((this._DayDividendYieldID != value))
                {
                    this._DayDividendYieldID = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Date.
        /// </summary>
        /// <value>The Date.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Date", DbType = "DateTime")]
        public System.Nullable<System.DateTime> Date
        {
            get
            {
                return this._Date;
            }
            set
            {
                if ((this._Date != value))
                {
                    this._Date = value;
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
        /// Gets or sets the Dividend Yield
        /// </summary>
        /// <value>The Dividend Yield.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DividendYield", DbType = "Float")]
        public System.Nullable<double> DividendYield
        {
            get
            {
                return this._DividendYield;
            }
            set
            {
                if ((this._DividendYield != value))
                {
                    this._DividendYield = value;
                }
            }
        }
    }
}
