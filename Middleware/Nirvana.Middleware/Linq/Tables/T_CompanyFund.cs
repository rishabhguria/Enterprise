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
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.T_CompanyFunds")]
    public partial class T_CompanyFund : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
        private int _CompanyFundID;
        private string _FundName;
        private string _FundShortName;
        private int _CompanyID;
        private System.Nullable<int> _FundTypeID;
        private System.Nullable<int> _UIOrder;
        private EntitySet<PM_CompanyFundCashCurrencyValue> _PM_CompanyFundCashCurrencyValues;
        private EntitySet<T_SubAccountCashValue> _T_SubAccountCashValues;
        private EntityRef<T_Company> _T_Company;
        void OnLoaded()
        {
        }
        void OnValidate(System.Data.Linq.ChangeAction action)
        {
        }
        void OnCreated()
        {
        }
        void OnCompanyFundIDChanging(int value)
        {
        }
        void OnCompanyFundIDChanged()
        {
        }
        void OnFundNameChanging(string value)
        {
        }
        void OnFundNameChanged()
        {
        }
        void OnFundShortNameChanging(string value)
        {
        }
        void OnFundShortNameChanged()
        {
        }
        void OnCompanyIDChanging(int value)
        {
        }
        void OnCompanyIDChanged()
        {
        }
        void OnFundTypeIDChanging(System.Nullable<int> value)
        {
        }
        void OnFundTypeIDChanged()
        {
        }
        void OnUIOrderChanging(System.Nullable<int> value)
        {
        }
        void OnUIOrderChanged()
        {
        }
        public T_CompanyFund()
        {
            this._PM_CompanyFundCashCurrencyValues = new EntitySet<PM_CompanyFundCashCurrencyValue>(new Action<PM_CompanyFundCashCurrencyValue>(this.attach_PM_CompanyFundCashCurrencyValues), new Action<PM_CompanyFundCashCurrencyValue>(this.detach_PM_CompanyFundCashCurrencyValues));
            this._T_SubAccountCashValues = new EntitySet<T_SubAccountCashValue>(new Action<T_SubAccountCashValue>(this.attach_T_SubAccountCashValues), new Action<T_SubAccountCashValue>(this.detach_T_SubAccountCashValues));
            this._T_Company = default(EntityRef<T_Company>);
            OnCreated();
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CompanyFundID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int CompanyFundID
        {
            get
            {
                return this._CompanyFundID;
            }
            set
            {
                if ((this._CompanyFundID != value))
                {
                    this.OnCompanyFundIDChanging(value);
                    this.SendPropertyChanging();
                    this._CompanyFundID = value;
                    this.SendPropertyChanged("CompanyFundID");
                    this.OnCompanyFundIDChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FundName", DbType = "VarChar(50) NOT NULL", CanBeNull = false)]
        public string FundName
        {
            get
            {
                return this._FundName;
            }
            set
            {
                if ((this._FundName != value))
                {
                    this.OnFundNameChanging(value);
                    this.SendPropertyChanging();
                    this._FundName = value;
                    this.SendPropertyChanged("FundName");
                    this.OnFundNameChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FundShortName", DbType = "VarChar(50) NOT NULL", CanBeNull = false)]
        public string FundShortName
        {
            get
            {
                return this._FundShortName;
            }
            set
            {
                if ((this._FundShortName != value))
                {
                    this.OnFundShortNameChanging(value);
                    this.SendPropertyChanging();
                    this._FundShortName = value;
                    this.SendPropertyChanged("FundShortName");
                    this.OnFundShortNameChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CompanyID", DbType = "Int NOT NULL")]
        public int CompanyID
        {
            get
            {
                return this._CompanyID;
            }
            set
            {
                if ((this._CompanyID != value))
                {
                    if (this._T_Company.HasLoadedOrAssignedValue)
                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
                    this.OnCompanyIDChanging(value);
                    this.SendPropertyChanging();
                    this._CompanyID = value;
                    this.SendPropertyChanged("CompanyID");
                    this.OnCompanyIDChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FundTypeID", DbType = "Int")]
        public System.Nullable<int> FundTypeID
        {
            get
            {
                return this._FundTypeID;
            }
            set
            {
                if ((this._FundTypeID != value))
                {
                    this.OnFundTypeIDChanging(value);
                    this.SendPropertyChanging();
                    this._FundTypeID = value;
                    this.SendPropertyChanged("FundTypeID");
                    this.OnFundTypeIDChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UIOrder", DbType = "Int")]
        public System.Nullable<int> UIOrder
        {
            get
            {
                return this._UIOrder;
            }
            set
            {
                if ((this._UIOrder != value))
                {
                    this.OnUIOrderChanging(value);
                    this.SendPropertyChanging();
                    this._UIOrder = value;
                    this.SendPropertyChanged("UIOrder");
                    this.OnUIOrderChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "T_CompanyFund_PM_CompanyFundCashCurrencyValue", Storage = "_PM_CompanyFundCashCurrencyValues", ThisKey = "CompanyFundID", OtherKey = "FundID")]
        public EntitySet<PM_CompanyFundCashCurrencyValue> PM_CompanyFundCashCurrencyValues
        {
            get
            {
                return this._PM_CompanyFundCashCurrencyValues;
            }
            set
            {
                this._PM_CompanyFundCashCurrencyValues.Assign(value);
            }
        }
        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "T_CompanyFund_T_SubAccountCashValue", Storage = "_T_SubAccountCashValues", ThisKey = "CompanyFundID", OtherKey = "FundID")]
        public EntitySet<T_SubAccountCashValue> T_SubAccountCashValues
        {
            get
            {
                return this._T_SubAccountCashValues;
            }
            set
            {
                this._T_SubAccountCashValues.Assign(value);
            }
        }
        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "T_Company_T_CompanyFund", Storage = "_T_Company", ThisKey = "CompanyID", OtherKey = "CompanyID", IsForeignKey = true)]
        public T_Company T_Company
        {
            get
            {
                return this._T_Company.Entity;
            }
            set
            {
                T_Company previousValue = this._T_Company.Entity;
                if (((previousValue != value) || (this._T_Company.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        this._T_Company.Entity = null;
                        previousValue.T_CompanyFunds.Remove(this);
                    }
                    this._T_Company.Entity = value;
                    if ((value != null))
                    {
                        value.T_CompanyFunds.Add(this);
                        this._CompanyID = value.CompanyID;
                    }
                    else
                        this._CompanyID = default(int);
                    this.SendPropertyChanged("T_Company");
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
        private void attach_PM_CompanyFundCashCurrencyValues(PM_CompanyFundCashCurrencyValue entity)
        {
            this.SendPropertyChanging();
            entity.T_CompanyFund = this;
        }
        private void detach_PM_CompanyFundCashCurrencyValues(PM_CompanyFundCashCurrencyValue entity)
        {
            this.SendPropertyChanging();
            entity.T_CompanyFund = null;
        }
        private void attach_T_SubAccountCashValues(T_SubAccountCashValue entity)
        {
            this.SendPropertyChanging();
            entity.T_CompanyFund = this;
        }
        private void detach_T_SubAccountCashValues(T_SubAccountCashValue entity)
        {
            this.SendPropertyChanging();
            entity.T_CompanyFund = null;
        }
    }
}
