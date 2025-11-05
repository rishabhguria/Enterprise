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
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.T_SubAccountCashValue")]
    public partial class T_SubAccountCashValue : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
        private int _FundID;
        private int _SubAccountID;
        private double _CashValue;
        private int _CurrencyID;
        private string _CashID;
        private string _TaxLotID;
        private System.DateTime _TradedDate;
        private System.Nullable<System.DateTime> _PayOutDate;
        private System.Nullable<double> _FXRate;
        private bool _IsAutomatic;
        private string _Symbol;
        private string _PBDesc;
        private System.Nullable<System.DateTime> _AccrueDate;
        private EntityRef<T_CompanyFund> _T_CompanyFund;
        private EntityRef<T_SubAccount> _T_SubAccount;
        void OnLoaded()
        {
        }
        void OnValidate(System.Data.Linq.ChangeAction action)
        {
        }
        void OnCreated()
        {
        }
        void OnFundIDChanging(int value)
        {
        }
        void OnFundIDChanged()
        {
        }
        void OnSubAccountIDChanging(int value)
        {
        }
        void OnSubAccountIDChanged()
        {
        }
        void OnCashValueChanging(double value)
        {
        }
        void OnCashValueChanged()
        {
        }
        void OnCurrencyIDChanging(int value)
        {
        }
        void OnCurrencyIDChanged()
        {
        }
        void OnCashIDChanging(string value)
        {
        }
        void OnCashIDChanged()
        {
        }
        void OnTaxLotIDChanging(string value)
        {
        }
        void OnTaxLotIDChanged()
        {
        }
        void OnTradedDateChanging(System.DateTime value)
        {
        }
        void OnTradedDateChanged()
        {
        }
        void OnPayOutDateChanging(System.Nullable<System.DateTime> value)
        {
        }
        void OnPayOutDateChanged()
        {
        }
        void OnFXRateChanging(System.Nullable<double> value)
        {
        }
        void OnFXRateChanged()
        {
        }
        void OnIsAutomaticChanging(bool value)
        {
        }
        void OnIsAutomaticChanged()
        {
        }
        void OnSymbolChanging(string value)
        {
        }
        void OnSymbolChanged()
        {
        }
        void OnPBDescChanging(string value)
        {
        }
        void OnPBDescChanged()
        {
        }
        void OnAccrueDateChanging(System.Nullable<System.DateTime> value)
        {
        }
        void OnAccrueDateChanged()
        {
        }
        public T_SubAccountCashValue()
        {
            this._T_CompanyFund = default(EntityRef<T_CompanyFund>);
            this._T_SubAccount = default(EntityRef<T_SubAccount>);
            OnCreated();
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
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SubAccountID", DbType = "Int NOT NULL")]
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
                    if (this._T_SubAccount.HasLoadedOrAssignedValue)
                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
                    this.OnSubAccountIDChanging(value);
                    this.SendPropertyChanging();
                    this._SubAccountID = value;
                    this.SendPropertyChanged("SubAccountID");
                    this.OnSubAccountIDChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CashValue", DbType = "Float NOT NULL")]
        public double CashValue
        {
            get
            {
                return this._CashValue;
            }
            set
            {
                if ((this._CashValue != value))
                {
                    this.OnCashValueChanging(value);
                    this.SendPropertyChanging();
                    this._CashValue = value;
                    this.SendPropertyChanged("CashValue");
                    this.OnCashValueChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CurrencyID", DbType = "Int NOT NULL")]
        public int CurrencyID
        {
            get
            {
                return this._CurrencyID;
            }
            set
            {
                if ((this._CurrencyID != value))
                {
                    this.OnCurrencyIDChanging(value);
                    this.SendPropertyChanging();
                    this._CurrencyID = value;
                    this.SendPropertyChanged("CurrencyID");
                    this.OnCurrencyIDChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CashID", DbType = "VarChar(50) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string CashID
        {
            get
            {
                return this._CashID;
            }
            set
            {
                if ((this._CashID != value))
                {
                    this.OnCashIDChanging(value);
                    this.SendPropertyChanging();
                    this._CashID = value;
                    this.SendPropertyChanged("CashID");
                    this.OnCashIDChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TaxLotID", DbType = "VarChar(50)")]
        public string TaxLotID
        {
            get
            {
                return this._TaxLotID;
            }
            set
            {
                if ((this._TaxLotID != value))
                {
                    this.OnTaxLotIDChanging(value);
                    this.SendPropertyChanging();
                    this._TaxLotID = value;
                    this.SendPropertyChanged("TaxLotID");
                    this.OnTaxLotIDChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TradedDate", DbType = "DateTime NOT NULL")]
        public System.DateTime TradedDate
        {
            get
            {
                return this._TradedDate;
            }
            set
            {
                if ((this._TradedDate != value))
                {
                    this.OnTradedDateChanging(value);
                    this.SendPropertyChanging();
                    this._TradedDate = value;
                    this.SendPropertyChanged("TradedDate");
                    this.OnTradedDateChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PayOutDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> PayOutDate
        {
            get
            {
                return this._PayOutDate;
            }
            set
            {
                if ((this._PayOutDate != value))
                {
                    this.OnPayOutDateChanging(value);
                    this.SendPropertyChanging();
                    this._PayOutDate = value;
                    this.SendPropertyChanged("PayOutDate");
                    this.OnPayOutDateChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FXRate", DbType = "Float")]
        public System.Nullable<double> FXRate
        {
            get
            {
                return this._FXRate;
            }
            set
            {
                if ((this._FXRate != value))
                {
                    this.OnFXRateChanging(value);
                    this.SendPropertyChanging();
                    this._FXRate = value;
                    this.SendPropertyChanged("FXRate");
                    this.OnFXRateChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IsAutomatic", DbType = "Bit NOT NULL")]
        public bool IsAutomatic
        {
            get
            {
                return this._IsAutomatic;
            }
            set
            {
                if ((this._IsAutomatic != value))
                {
                    this.OnIsAutomaticChanging(value);
                    this.SendPropertyChanging();
                    this._IsAutomatic = value;
                    this.SendPropertyChanged("IsAutomatic");
                    this.OnIsAutomaticChanged();
                }
            }
        }
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
                    this.OnSymbolChanging(value);
                    this.SendPropertyChanging();
                    this._Symbol = value;
                    this.SendPropertyChanged("Symbol");
                    this.OnSymbolChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PBDesc", DbType = "VarChar(3000)")]
        public string PBDesc
        {
            get
            {
                return this._PBDesc;
            }
            set
            {
                if ((this._PBDesc != value))
                {
                    this.OnPBDescChanging(value);
                    this.SendPropertyChanging();
                    this._PBDesc = value;
                    this.SendPropertyChanged("PBDesc");
                    this.OnPBDescChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_AccrueDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> AccrueDate
        {
            get
            {
                return this._AccrueDate;
            }
            set
            {
                if ((this._AccrueDate != value))
                {
                    this.OnAccrueDateChanging(value);
                    this.SendPropertyChanging();
                    this._AccrueDate = value;
                    this.SendPropertyChanged("AccrueDate");
                    this.OnAccrueDateChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "T_CompanyFund_T_SubAccountCashValue", Storage = "_T_CompanyFund", ThisKey = "FundID", OtherKey = "CompanyFundID", IsForeignKey = true)]
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
                        previousValue.T_SubAccountCashValues.Remove(this);
                    }
                    this._T_CompanyFund.Entity = value;
                    if ((value != null))
                    {
                        value.T_SubAccountCashValues.Add(this);
                        this._FundID = value.CompanyFundID;
                    }
                    else
                        this._FundID = default(int);
                    this.SendPropertyChanged("T_CompanyFund");
                }
            }
        }
        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "T_SubAccount_T_SubAccountCashValue", Storage = "_T_SubAccount", ThisKey = "SubAccountID", OtherKey = "SubAccountID", IsForeignKey = true)]
        public T_SubAccount T_SubAccount
        {
            get
            {
                return this._T_SubAccount.Entity;
            }
            set
            {
                T_SubAccount previousValue = this._T_SubAccount.Entity;
                if (((previousValue != value) || (this._T_SubAccount.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        this._T_SubAccount.Entity = null;
                        previousValue.T_SubAccountCashValues.Remove(this);
                    }
                    this._T_SubAccount.Entity = value;
                    if ((value != null))
                    {
                        value.T_SubAccountCashValues.Add(this);
                        this._SubAccountID = value.SubAccountID;
                    }
                    else
                        this._SubAccountID = default(int);
                    this.SendPropertyChanged("T_SubAccount");
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
