
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
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.T_SubAccounts")]
    public partial class T_SubAccount : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
        private int _SubAccountID;
        private string _Name;
        private string _Acronym;
        private int _AccountID;
        private EntitySet<T_SubAccountCashValue> _T_SubAccountCashValues;
        void OnLoaded()
        {
        }
        void OnValidate(System.Data.Linq.ChangeAction action)
        {
        }
        void OnCreated()
        {
        }
        void OnSubAccountIDChanging(int value)
        {
        }
        void OnSubAccountIDChanged()
        {
        }
        void OnNameChanging(string value)
        {
        }
        void OnNameChanged()
        {
        }
        void OnAcronymChanging(string value)
        {
        }
        void OnAcronymChanged()
        {
        }
        void OnAccountIDChanging(int value)
        {
        }
        void OnAccountIDChanged()
        {
        }
        public T_SubAccount()
        {
            this._T_SubAccountCashValues = new EntitySet<T_SubAccountCashValue>(new Action<T_SubAccountCashValue>(this.attach_T_SubAccountCashValues), new Action<T_SubAccountCashValue>(this.detach_T_SubAccountCashValues));
            OnCreated();
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SubAccountID", DbType = "Int NOT NULL", IsPrimaryKey = true)]
        public int SubAccountID
        {
            get
            {
                return this._SubAccountID;
            }
            set
            {
                if ((this._SubAccountID != value))
                {
                    this.OnSubAccountIDChanging(value);
                    this.SendPropertyChanging();
                    this._SubAccountID = value;
                    this.SendPropertyChanged("SubAccountID");
                    this.OnSubAccountIDChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Name", DbType = "VarChar(50) NOT NULL", CanBeNull = false)]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if ((this._Name != value))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._Name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Acronym", DbType = "VarChar(50) NOT NULL", CanBeNull = false)]
        public string Acronym
        {
            get
            {
                return this._Acronym;
            }
            set
            {
                if ((this._Acronym != value))
                {
                    this.OnAcronymChanging(value);
                    this.SendPropertyChanging();
                    this._Acronym = value;
                    this.SendPropertyChanged("Acronym");
                    this.OnAcronymChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_AccountID", DbType = "Int NOT NULL")]
        public int AccountID
        {
            get
            {
                return this._AccountID;
            }
            set
            {
                if ((this._AccountID != value))
                {
                    this.OnAccountIDChanging(value);
                    this.SendPropertyChanging();
                    this._AccountID = value;
                    this.SendPropertyChanged("AccountID");
                    this.OnAccountIDChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "T_SubAccount_T_SubAccountCashValue", Storage = "_T_SubAccountCashValues", ThisKey = "SubAccountID", OtherKey = "SubAccountID")]
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
        private void attach_T_SubAccountCashValues(T_SubAccountCashValue entity)
        {
            this.SendPropertyChanging();
            entity.T_SubAccount = this;
        }
        private void detach_T_SubAccountCashValues(T_SubAccountCashValue entity)
        {
            this.SendPropertyChanging();
            entity.T_SubAccount = null;
        }
    }
}
