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
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.PM_DailyBeta")]
    public partial class PM_DailyBeta : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
        private int _DayBetaID;
        private System.DateTime _Date;
        private string _Symbol;
        private double _Beta;
        void OnLoaded()
        {
        }
        void OnValidate(System.Data.Linq.ChangeAction action)
        {
        }
        void OnCreated()
        {
        }
        void OnDayBetaIDChanging(int value)
        {
        }
        void OnDayBetaIDChanged()
        {
        }
        void OnDateChanging(System.DateTime value)
        {
        }
        void OnDateChanged()
        {
        }
        void OnSymbolChanging(string value)
        {
        }
        void OnSymbolChanged()
        {
        }
        void OnBetaChanging(double value)
        {
        }
        void OnBetaChanged()
        {
        }
        public PM_DailyBeta()
        {
            OnCreated();
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DayBetaID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int DayBetaID
        {
            get
            {
                return this._DayBetaID;
            }
            set
            {
                if ((this._DayBetaID != value))
                {
                    this.OnDayBetaIDChanging(value);
                    this.SendPropertyChanging();
                    this._DayBetaID = value;
                    this.SendPropertyChanged("DayBetaID");
                    this.OnDayBetaIDChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Date", DbType = "DateTime NOT NULL")]
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
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Symbol", DbType = "VarChar(100) NOT NULL", CanBeNull = false)]
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
                    this.OnSymbolChanging(value);
                    this.SendPropertyChanging();
                    this._Symbol = value;
                    this.SendPropertyChanged("Symbol");
                    this.OnSymbolChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Beta", DbType = "Float NOT NULL")]
        public double Beta
        {
            get
            {
                return this._Beta;
            }
            set
            {
                if ((this._Beta != value))
                {
                    this.OnBetaChanging(value);
                    this.SendPropertyChanging();
                    this._Beta = value;
                    this.SendPropertyChanged("Beta");
                    this.OnBetaChanged();
                }
            }
        }
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void SendPropertyChanging()
        {
            if ((this.PropertyChanging != null))
                this.PropertyChanging(this, emptyChangingEventArgs);
        }
        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
