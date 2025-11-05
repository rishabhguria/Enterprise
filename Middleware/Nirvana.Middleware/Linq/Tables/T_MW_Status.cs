using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Nirvana.Middleware.Linq
{

    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.T_MW_Status")]
    public partial class T_MW_Status : INotifyPropertyChanging, INotifyPropertyChanged
    {

        /// <summary>
        /// 
        /// </summary>
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        /// <summary>
        /// 
        /// </summary>
        private System.DateTime _Date;

        /// <summary>
        /// 
        /// </summary>
        private string _Generic;

        /// <summary>
        /// 
        /// </summary>
        private string _Veda;

        /// <summary>
        /// 
        /// </summary>
        private string _Cache;

        /// <summary>
        /// 
        /// </summary>
        private string _Transactions;

        /// <summary>
        /// 
        /// </summary>
        private string _DerivedData;

        /// <summary>
        /// 
        /// </summary>
        private string _DailyReturns;

        #region Extensibility Method Definitions
        /// <summary>
        /// Called when [loaded].
        /// </summary>
        /// <remarks></remarks>
        partial void OnLoaded();
        /// <summary>
        /// Called when [validate].
        /// </summary>
        /// <param name="action">The action.</param>
        /// <remarks></remarks>
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        /// <summary>
        /// Called when [created].
        /// </summary>
        /// <remarks></remarks>
        partial void OnCreated();
        /// <summary>
        /// Called when [date changing].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks></remarks>
        partial void OnDateChanging(System.DateTime value);
        /// <summary>
        /// Called when [date changed].
        /// </summary>
        /// <remarks></remarks>
        partial void OnDateChanged();
        /// <summary>
        /// Called when [generic changing].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks></remarks>
        partial void OnGenericChanging(string value);

        /// <summary>
        /// Called when [veda changing].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks></remarks>
        partial void OnVedaChanging(string value);

        /// <summary>
        /// Called when [cache changing].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks></remarks>
        partial void OnCacheChanging(string value);

        /// <summary>
        /// Called when [generic changed].
        /// </summary>
        /// <remarks></remarks>
        partial void OnGenericChanged();

        /// <summary>
        /// Called when [veda changed].
        /// </summary>
        /// <remarks></remarks>
        partial void OnVedaChanged();

        /// <summary>
        /// Called when [cache changed].
        /// </summary>
        /// <remarks></remarks>
        partial void OnCacheChanged();

        /// <summary>
        /// Called when [transactions changing].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks></remarks>
        partial void OnTransactionsChanging(string value);
        /// <summary>
        /// Called when [transactions changed].
        /// </summary>
        /// <remarks></remarks>
        partial void OnTransactionsChanged();
        /// <summary>
        /// Called when [derived data changing].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks></remarks>
        partial void OnDerivedDataChanging(string value);
        /// <summary>
        /// Called when [derived data changed].
        /// </summary>
        /// <remarks></remarks>
        partial void OnDerivedDataChanged();
        /// <summary>
        /// Called when [daily returns changing].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks></remarks>
        partial void OnDailyReturnsChanging(string value);
        /// <summary>
        /// Called when [daily returns changed].
        /// </summary>
        /// <remarks></remarks>
        partial void OnDailyReturnsChanged();
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public T_MW_Status()
        {
            OnCreated();
        }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Date", DbType = "DateTime NOT NULL", IsPrimaryKey = true)]
        public System.DateTime Date
        {
            get
            {
                return this._Date;
            }
            set
            {
                if ((this._Date != value))
                {
                    this.OnDateChanging(value);
                    this.SendPropertyChanging();
                    this._Date = value;
                    this.SendPropertyChanged("Date");
                    this.OnDateChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the generic.
        /// </summary>
        /// <value>The generic.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Generic", DbType = "NVarChar(200)")]
        public string Generic
        {
            get
            {
                return this._Generic;
            }
            set
            {
                if ((this._Generic != value))
                {
                    this.OnGenericChanging(value);
                    this.SendPropertyChanging();
                    this._Generic = value;
                    this.SendPropertyChanged("Generic");
                    this.OnGenericChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the veda.
        /// </summary>
        /// <value>The veda.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Veda", DbType = "NVarChar(200)")]
        public string Veda
        {
            get
            {
                return this._Veda;
            }
            set
            {
                if ((this._Veda != value))
                {
                    this.OnVedaChanging(value);
                    this.SendPropertyChanging();
                    this._Veda = value;
                    this.SendPropertyChanged("Veda");
                    this.OnVedaChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the cache.
        /// </summary>
        /// <value>The cache.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Cache", DbType = "NVarChar(200)")]
        public string Cache
        {
            get
            {
                return this._Cache;
            }
            set
            {
                if ((this._Cache != value))
                {
                    this.OnCacheChanging(value);
                    this.SendPropertyChanging();
                    this._Cache = value;
                    this.SendPropertyChanged("Cache");
                    this.OnCacheChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        /// <value>The transactions.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Transactions", DbType = "NVarChar(200)")]
        public string Transactions
        {
            get
            {
                return this._Transactions;
            }
            set
            {
                if ((this._Transactions != value))
                {
                    this.OnTransactionsChanging(value);
                    this.SendPropertyChanging();
                    this._Transactions = value;
                    this.SendPropertyChanged("Transactions");
                    this.OnTransactionsChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the derived data.
        /// </summary>
        /// <value>The derived data.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DerivedData", DbType = "NVarChar(200)")]
        public string DerivedData
        {
            get
            {
                return this._DerivedData;
            }
            set
            {
                if ((this._DerivedData != value))
                {
                    this.OnDerivedDataChanging(value);
                    this.SendPropertyChanging();
                    this._DerivedData = value;
                    this.SendPropertyChanged("DerivedData");
                    this.OnDerivedDataChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the daily returns.
        /// </summary>
        /// <value>The daily returns.</value>
        /// <remarks></remarks>
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DailyReturns", DbType = "NVarChar(200)")]
        public string DailyReturns
        {
            get
            {
                return this._DailyReturns;
            }
            set
            {
                if ((this._DailyReturns != value))
                {
                    this.OnDailyReturnsChanging(value);
                    this.SendPropertyChanging();
                    this._DailyReturns = value;
                    this.SendPropertyChanged("DailyReturns");
                    this.OnDailyReturnsChanged();
                }
            }
        }

        /// <summary>
        /// Occurs when a property value is changing.
        /// </summary>
        /// <remarks></remarks>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <remarks></remarks>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Sends the property changing.
        /// </summary>
        /// <remarks></remarks>
        protected virtual void SendPropertyChanging()
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        /// <summary>
        /// Sends the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <remarks></remarks>
        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
