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
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.PM_DayMarkPrice")]
    public partial class PM_DayMarkPrice : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
        private int _DayMarkPriceID;
        private System.DateTime _Date;
        private string _Symbol;
        private double _ApplicationMarkPrice;
        private double _PrimeBrokerMarkPrice;
        private double _FinalMarkPrice;
        private bool _IsActive;
        private double _ForwardPoints;
        void OnLoaded()
        {
        }
        void OnValidate(System.Data.Linq.ChangeAction action)
        {
        }
        void OnCreated()
        {
        }
        void OnDayMarkPriceIDChanging(int value)
        {
        }
        void OnDayMarkPriceIDChanged()
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
        void OnApplicationMarkPriceChanging(double value)
        {
        }
        void OnApplicationMarkPriceChanged()
        {
        }
        void OnPrimeBrokerMarkPriceChanging(double value)
        {
        }
        void OnPrimeBrokerMarkPriceChanged()
        {
        }
        void OnFinalMarkPriceChanging(double value)
        {
        }
        void OnFinalMarkPriceChanged()
        {
        }
        void OnIsActiveChanging(bool value)
        {
        }
        void OnIsActiveChanged()
        {
        }
        void OnForwardPointsChanging(double value)
        {
        }
        void OnForwardPointsChanged()
        {
        }
        public PM_DayMarkPrice()
        {
            OnCreated();
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DayMarkPriceID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int DayMarkPriceID
        {
            get
            {
                return this._DayMarkPriceID;
            }
            set
            {
                if ((this._DayMarkPriceID != value))
                {
                    this.OnDayMarkPriceIDChanging(value);
                    this.SendPropertyChanging();
                    this._DayMarkPriceID = value;
                    this.SendPropertyChanged("DayMarkPriceID");
                    this.OnDayMarkPriceIDChanged();
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
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ApplicationMarkPrice", DbType = "Float NOT NULL")]
        public double ApplicationMarkPrice
        {
            get
            {
                return this._ApplicationMarkPrice;
            }
            set
            {
                if ((this._ApplicationMarkPrice != value))
                {
                    this.OnApplicationMarkPriceChanging(value);
                    this.SendPropertyChanging();
                    this._ApplicationMarkPrice = value;
                    this.SendPropertyChanged("ApplicationMarkPrice");
                    this.OnApplicationMarkPriceChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PrimeBrokerMarkPrice", DbType = "Float NOT NULL")]
        public double PrimeBrokerMarkPrice
        {
            get
            {
                return this._PrimeBrokerMarkPrice;
            }
            set
            {
                if ((this._PrimeBrokerMarkPrice != value))
                {
                    this.OnPrimeBrokerMarkPriceChanging(value);
                    this.SendPropertyChanging();
                    this._PrimeBrokerMarkPrice = value;
                    this.SendPropertyChanged("PrimeBrokerMarkPrice");
                    this.OnPrimeBrokerMarkPriceChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FinalMarkPrice", DbType = "Float NOT NULL")]
        public double FinalMarkPrice
        {
            get
            {
                return this._FinalMarkPrice;
            }
            set
            {
                if ((this._FinalMarkPrice != value))
                {
                    this.OnFinalMarkPriceChanging(value);
                    this.SendPropertyChanging();
                    this._FinalMarkPrice = value;
                    this.SendPropertyChanged("FinalMarkPrice");
                    this.OnFinalMarkPriceChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IsActive", DbType = "Bit NOT NULL")]
        public bool IsActive
        {
            get
            {
                return this._IsActive;
            }
            set
            {
                if ((this._IsActive != value))
                {
                    this.OnIsActiveChanging(value);
                    this.SendPropertyChanging();
                    this._IsActive = value;
                    this.SendPropertyChanged("IsActive");
                    this.OnIsActiveChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ForwardPoints", DbType = "Float NOT NULL")]
        public double ForwardPoints
        {
            get
            {
                return this._ForwardPoints;
            }
            set
            {
                if ((this._ForwardPoints != value))
                {
                    this.OnForwardPointsChanging(value);
                    this.SendPropertyChanging();
                    this._ForwardPoints = value;
                    this.SendPropertyChanged("ForwardPoints");
                    this.OnForwardPointsChanged();
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
