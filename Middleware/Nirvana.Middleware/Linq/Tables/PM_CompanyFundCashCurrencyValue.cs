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
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.PM_CompanyFundCashCurrencyValue")]
    public partial class PM_CompanyFundCashCurrencyValue : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
        private int _CashCurrencyID;
        private System.DateTime _Date;
        private int _FundID;
        private System.Nullable<int> _BaseCurrencyID;
        private System.Nullable<double> _CashValueBase;
        private System.Nullable<int> _LocalCurrencyID;
        private double _CashValueLocal;
        private EntityRef<T_CompanyFund> _T_CompanyFund;
        void OnLoaded()
        {
        }
        void OnValidate(System.Data.Linq.ChangeAction action)
        {
        }
        void OnCreated()
        {
        }
        void OnCashCurrencyIDChanging(int value)
        {
        }
        void OnCashCurrencyIDChanged()
        {
        }
        void OnDateChanging(System.DateTime value)
        {
        }
        void OnDateChanged()
        {
        }
        void OnFundIDChanging(int value)
        {
        }
        void OnFundIDChanged()
        {
        }
        void OnBaseCurrencyIDChanging(System.Nullable<int> value)
        {
        }
        void OnBaseCurrencyIDChanged()
        {
        }
        void OnCashValueBaseChanging(System.Nullable<double> value)
        {
        }
        void OnCashValueBaseChanged()
        {
        }
        void OnLocalCurrencyIDChanging(System.Nullable<int> value)
        {
        }
        void OnLocalCurrencyIDChanged()
        {
        }
        void OnCashValueLocalChanging(double value)
        {
        }
        void OnCashValueLocalChanged()
        {
        }
        public PM_CompanyFundCashCurrencyValue()
        {
            this._T_CompanyFund = default(EntityRef<T_CompanyFund>);
            OnCreated();
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CashCurrencyID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int CashCurrencyID
        {
            get
            {
                return this._CashCurrencyID;
            }
            set
            {
                if ((this._CashCurrencyID != value))
                {
                    this.OnCashCurrencyIDChanging(value);
                    this.SendPropertyChanging();
                    this._CashCurrencyID = value;
                    this.SendPropertyChanged("CashCurrencyID");
                    this.OnCashCurrencyIDChanged();
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
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FundID", DbType = "Int NOT NULL")]
        public int FundID
        {
            get
            {
                return this._FundID;
            }
            set
            {
                if ((this._FundID != value))
                {
                    if (this._T_CompanyFund.HasLoadedOrAssignedValue)
                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
                    this.OnFundIDChanging(value);
                    this.SendPropertyChanging();
                    this._FundID = value;
                    this.SendPropertyChanged("FundID");
                    this.OnFundIDChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_BaseCurrencyID", DbType = "Int")]
        public System.Nullable<int> BaseCurrencyID
        {
            get
            {
                return this._BaseCurrencyID;
            }
            set
            {
                if ((this._BaseCurrencyID != value))
                {
                    this.OnBaseCurrencyIDChanging(value);
                    this.SendPropertyChanging();
                    this._BaseCurrencyID = value;
                    this.SendPropertyChanged("BaseCurrencyID");
                    this.OnBaseCurrencyIDChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CashValueBase", DbType = "Float")]
        public System.Nullable<double> CashValueBase
        {
            get
            {
                return this._CashValueBase;
            }
            set
            {
                if ((this._CashValueBase != value))
                {
                    this.OnCashValueBaseChanging(value);
                    this.SendPropertyChanging();
                    this._CashValueBase = value;
                    this.SendPropertyChanged("CashValueBase");
                    this.OnCashValueBaseChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LocalCurrencyID", DbType = "Int")]
        public System.Nullable<int> LocalCurrencyID
        {
            get
            {
                return this._LocalCurrencyID;
            }
            set
            {
                if ((this._LocalCurrencyID != value))
                {
                    this.OnLocalCurrencyIDChanging(value);
                    this.SendPropertyChanging();
                    this._LocalCurrencyID = value;
                    this.SendPropertyChanged("LocalCurrencyID");
                    this.OnLocalCurrencyIDChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CashValueLocal", DbType = "Float NOT NULL")]
        public double CashValueLocal
        {
            get
            {
                return this._CashValueLocal;
            }
            set
            {
                if ((this._CashValueLocal != value))
                {
                    this.OnCashValueLocalChanging(value);
                    this.SendPropertyChanging();
                    this._CashValueLocal = value;
                    this.SendPropertyChanged("CashValueLocal");
                    this.OnCashValueLocalChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "T_CompanyFund_PM_CompanyFundCashCurrencyValue", Storage = "_T_CompanyFund", ThisKey = "FundID", OtherKey = "CompanyFundID", IsForeignKey = true)]
        public T_CompanyFund T_CompanyFund
        {
            get
            {
                return this._T_CompanyFund.Entity;
            }
            set
            {
                T_CompanyFund previousValue = this._T_CompanyFund.Entity;
                if (((previousValue != value) || (this._T_CompanyFund.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        this._T_CompanyFund.Entity = null;
                        previousValue.PM_CompanyFundCashCurrencyValues.Remove(this);
                    }
                    this._T_CompanyFund.Entity = value;
                    if ((value != null))
                    {
                        value.PM_CompanyFundCashCurrencyValues.Add(this);
                        this._FundID = value.CompanyFundID;
                    }
                    else
                        this._FundID = default(int);
                    this.SendPropertyChanged("T_CompanyFund");
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
