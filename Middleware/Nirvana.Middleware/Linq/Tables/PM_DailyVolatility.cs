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
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.PM_DailyVolatility")]
    public partial class PM_DailyVolatility
    {
        public PM_DailyVolatility()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<int> _DayVolatilityID;

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
        private System.Nullable<double> _Volatility;

        /// <summary>
        /// Gets or sets the DayVolatilityID.
        /// </summary>
        /// <value>DayVolatilityID</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DayVolatilityID", DbType = "Int")]
        public System.Nullable<int> DayVolatilityID
        {
            get
            {
                return this._DayVolatilityID;
            }
            set
            {
                if ((this._DayVolatilityID != value))
                {
                    this._DayVolatilityID = value;
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
        /// Gets or sets the Volatility
        /// </summary>
        /// <value>Volatility</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Volatility", DbType = "Float")]
        public System.Nullable<double> Volatility
        {
            get
            {
                return this._Volatility;
            }
            set
            {
                if ((this._Volatility != value))
                {
                    this._Volatility = value;
                }
            }
        }
    }
}
