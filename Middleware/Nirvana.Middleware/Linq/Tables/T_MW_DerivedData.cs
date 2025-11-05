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
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.T_MW_DerivedData")]
    public partial class T_MW_DerivedData
    {
        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<System.DateTime> _CalculatedFromDate;
        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<System.DateTime> _CalculatedToDate;
        /// <summary>
        /// 
        /// </summary>
        private string _TimeFrameType;
        /// <summary>
        /// 
        /// </summary>
        private string _DataType;
        /// <summary>
        /// 
        /// </summary>
        private string _Groupfield;
        /// <summary>
        /// 
        /// </summary>
        private string _Entity;
        /// <summary>
        /// 
        /// </summary>
        private System.Nullable<double> _Value;
        /// <summary>
        /// 
        /// </summary>
        private string _Comments;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public T_MW_DerivedData()
        {
        }
        /// <summary>
        /// Gets or sets the calculated from date.
        /// </summary>
        /// <value>The calculated from date.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CalculatedFromDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> CalculatedFromDate
        {
            get
            {
                return this._CalculatedFromDate;
            }
            set
            {
                if ((this._CalculatedFromDate != value))
                    this._CalculatedFromDate = value;
            }
        }
        /// <summary>
        /// Gets or sets the calculated to date.
        /// </summary>
        /// <value>The calculated to date.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CalculatedToDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> CalculatedToDate
        {
            get
            {
                return this._CalculatedToDate;
            }
            set
            {
                if ((this._CalculatedToDate != value))
                    this._CalculatedToDate = value;
            }
        }
        /// <summary>
        /// Gets or sets the type of the time frame.
        /// </summary>
        /// <value>The type of the time frame.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TimeFrameType", DbType = "VarChar(30)")]
        public string TimeFrameType
        {
            get
            {
                return this._TimeFrameType;
            }
            set
            {
                if ((this._TimeFrameType != value))
                    this._TimeFrameType = value;
            }
        }
        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DataType", DbType = "VarChar(MAX)")]
        public string DataType
        {
            get
            {
                return this._DataType;
            }
            set
            {
                if ((this._DataType != value))
                    this._DataType = value;
            }
        }
        /// <summary>
        /// Gets or sets the groupfield.
        /// </summary>
        /// <value>The groupfield.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Groupfield", DbType = "VarChar(30)")]
        public string Groupfield
        {
            get
            {
                return this._Groupfield;
            }
            set
            {
                if ((this._Groupfield != value))
                    this._Groupfield = value;
            }
        }
        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        /// <value>The entity.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Entity", DbType = "VarChar(MAX)")]
        public string Entity
        {
            get
            {
                return this._Entity;
            }
            set
            {
                if ((this._Entity != value))
                    this._Entity = value;
            }
        }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Value", DbType = "Float")]
        public System.Nullable<double> Value
        {
            get
            {
                return this._Value;
            }
            set
            {
                if ((this._Value != value))
                    this._Value = value;
            }
        }
        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Comments", DbType = "VarChar(50)")]
        public string Comments
        {
            get
            {
                return this._Comments;
            }
            set
            {
                if ((this._Comments != value))
                    this._Comments = value;
            }
        }
    }
}
