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
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.T_Company")]
    public partial class T_Company : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
        private int _CompanyID;
        private string _Name;
        private string _Address1;
        private string _Address2;
        private int _CompanyTypeID;
        private string _Telephone;
        private string _Fax;
        private string _PrimaryContactFirstName;
        private string _PrimaryContactLastName;
        private string _PrimaryContactTitle;
        private string _PrimaryContactEMail;
        private string _PrimaryContactTelephone;
        private string _PrimaryContactCell;
        //the list of columns deleted from T_Company table SVN 
        //private string _SecondaryContactFirstName;
        //private string _SecondaryContactLastName;
        //private string _SecondaryContactTitle;
        //private string _SecondaryContactEMail;
        //private string _SecondaryContactTelephone;
        //private string _SecondaryContactCell;
        //private string _TechnologyContactFirstName;
        //private string _TechnologyContactLastName;
        //private string _TechnologyContactTitle;
        //private string _TechnologyContactEMail;
        //private string _TechnologyContactTelephone;
        //private string _TechnologyContactCell;
        private string _ShortName;
        private string _Login;
        private string _Password;
        private System.Nullable<int> _CountryID;
        private System.Nullable<int> _StateID;
        private string _Zip;
        private System.Nullable<int> _BaseCurrencyID;
        private System.Nullable<int> _SupportsMultipleCurrencies;
        private string _City;
        private System.Nullable<int> _DefaultAUECID;
        private EntitySet<T_CompanyFund> _T_CompanyFunds;
        void OnLoaded()
        {
        }
        void OnValidate(System.Data.Linq.ChangeAction action)
        {
        }
        void OnCreated()
        {
        }
        void OnCompanyIDChanging(int value)
        {
        }
        void OnCompanyIDChanged()
        {
        }
        void OnNameChanging(string value)
        {
        }
        void OnNameChanged()
        {
        }
        void OnAddress1Changing(string value)
        {
        }
        void OnAddress1Changed()
        {
        }
        void OnAddress2Changing(string value)
        {
        }
        void OnAddress2Changed()
        {
        }
        void OnCompanyTypeIDChanging(int value)
        {
        }
        void OnCompanyTypeIDChanged()
        {
        }
        void OnTelephoneChanging(string value)
        {
        }
        void OnTelephoneChanged()
        {
        }
        void OnFaxChanging(string value)
        {
        }
        void OnFaxChanged()
        {
        }
        void OnPrimaryContactFirstNameChanging(string value)
        {
        }
        void OnPrimaryContactFirstNameChanged()
        {
        }
        void OnPrimaryContactLastNameChanging(string value)
        {
        }
        void OnPrimaryContactLastNameChanged()
        {
        }
        void OnPrimaryContactTitleChanging(string value)
        {
        }
        void OnPrimaryContactTitleChanged()
        {
        }
        void OnPrimaryContactEMailChanging(string value)
        {
        }
        void OnPrimaryContactEMailChanged()
        {
        }
        void OnPrimaryContactTelephoneChanging(string value)
        {
        }
        void OnPrimaryContactTelephoneChanged()
        {
        }
        void OnPrimaryContactCellChanging(string value)
        {
        }
        void OnPrimaryContactCellChanged()
        {
        }
        void OnSecondaryContactFirstNameChanging(string value)
        {
        }
        void OnSecondaryContactFirstNameChanged()
        {
        }
        void OnSecondaryContactLastNameChanging(string value)
        {
        }
        void OnSecondaryContactLastNameChanged()
        {
        }
        void OnSecondaryContactTitleChanging(string value)
        {
        }
        void OnSecondaryContactTitleChanged()
        {
        }
        void OnSecondaryContactEMailChanging(string value)
        {
        }
        void OnSecondaryContactEMailChanged()
        {
        }
        void OnSecondaryContactTelephoneChanging(string value)
        {
        }
        void OnSecondaryContactTelephoneChanged()
        {
        }
        void OnSecondaryContactCellChanging(string value)
        {
        }
        void OnSecondaryContactCellChanged()
        {
        }
        void OnTechnologyContactFirstNameChanging(string value)
        {
        }
        void OnTechnologyContactFirstNameChanged()
        {
        }
        void OnTechnologyContactLastNameChanging(string value)
        {
        }
        void OnTechnologyContactLastNameChanged()
        {
        }
        void OnTechnologyContactTitleChanging(string value)
        {
        }
        void OnTechnologyContactTitleChanged()
        {
        }
        void OnTechnologyContactEMailChanging(string value)
        {
        }
        void OnTechnologyContactEMailChanged()
        {
        }
        void OnTechnologyContactTelephoneChanging(string value)
        {
        }
        void OnTechnologyContactTelephoneChanged()
        {
        }
        void OnTechnologyContactCellChanging(string value)
        {
        }
        void OnTechnologyContactCellChanged()
        {
        }
        void OnShortNameChanging(string value)
        {
        }
        void OnShortNameChanged()
        {
        }
        void OnLoginChanging(string value)
        {
        }
        void OnLoginChanged()
        {
        }
        void OnPasswordChanging(string value)
        {
        }
        void OnPasswordChanged()
        {
        }
        void OnCountryIDChanging(System.Nullable<int> value)
        {
        }
        void OnCountryIDChanged()
        {
        }
        void OnStateIDChanging(System.Nullable<int> value)
        {
        }
        void OnStateIDChanged()
        {
        }
        void OnZipChanging(string value)
        {
        }
        void OnZipChanged()
        {
        }
        void OnBaseCurrencyIDChanging(System.Nullable<int> value)
        {
        }
        void OnBaseCurrencyIDChanged()
        {
        }
        void OnSupportsMultipleCurrenciesChanging(System.Nullable<int> value)
        {
        }
        void OnSupportsMultipleCurrenciesChanged()
        {
        }
        void OnCityChanging(string value)
        {
        }
        void OnCityChanged()
        {
        }
        void OnDefaultAUECIDChanging(System.Nullable<int> value)
        {
        }
        void OnDefaultAUECIDChanged()
        {
        }
        public T_Company()
        {
            this._T_CompanyFunds = new EntitySet<T_CompanyFund>(new Action<T_CompanyFund>(this.attach_T_CompanyFunds), new Action<T_CompanyFund>(this.detach_T_CompanyFunds));
            OnCreated();
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CompanyID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
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
                    this.OnCompanyIDChanging(value);
                    this.SendPropertyChanging();
                    this._CompanyID = value;
                    this.SendPropertyChanged("CompanyID");
                    this.OnCompanyIDChanged();
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
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Address1", DbType = "VarChar(100) NOT NULL", CanBeNull = false)]
        public string Address1
        {
            get
            {
                return this._Address1;
            }
            set
            {
                if ((this._Address1 != value))
                {
                    this.OnAddress1Changing(value);
                    this.SendPropertyChanging();
                    this._Address1 = value;
                    this.SendPropertyChanged("Address1");
                    this.OnAddress1Changed();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Address2", DbType = "VarChar(100)")]
        public string Address2
        {
            get
            {
                return this._Address2;
            }
            set
            {
                if ((this._Address2 != value))
                {
                    this.OnAddress2Changing(value);
                    this.SendPropertyChanging();
                    this._Address2 = value;
                    this.SendPropertyChanged("Address2");
                    this.OnAddress2Changed();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CompanyTypeID", DbType = "Int NOT NULL")]
        public int CompanyTypeID
        {
            get
            {
                return this._CompanyTypeID;
            }
            set
            {
                if ((this._CompanyTypeID != value))
                {
                    this.OnCompanyTypeIDChanging(value);
                    this.SendPropertyChanging();
                    this._CompanyTypeID = value;
                    this.SendPropertyChanged("CompanyTypeID");
                    this.OnCompanyTypeIDChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Telephone", DbType = "VarChar(20) NOT NULL", CanBeNull = false)]
        public string Telephone
        {
            get
            {
                return this._Telephone;
            }
            set
            {
                if ((this._Telephone != value))
                {
                    this.OnTelephoneChanging(value);
                    this.SendPropertyChanging();
                    this._Telephone = value;
                    this.SendPropertyChanged("Telephone");
                    this.OnTelephoneChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Fax", DbType = "VarChar(20)")]
        public string Fax
        {
            get
            {
                return this._Fax;
            }
            set
            {
                if ((this._Fax != value))
                {
                    this.OnFaxChanging(value);
                    this.SendPropertyChanging();
                    this._Fax = value;
                    this.SendPropertyChanged("Fax");
                    this.OnFaxChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PrimaryContactFirstName", DbType = "VarChar(50) NOT NULL", CanBeNull = false)]
        public string PrimaryContactFirstName
        {
            get
            {
                return this._PrimaryContactFirstName;
            }
            set
            {
                if ((this._PrimaryContactFirstName != value))
                {
                    this.OnPrimaryContactFirstNameChanging(value);
                    this.SendPropertyChanging();
                    this._PrimaryContactFirstName = value;
                    this.SendPropertyChanged("PrimaryContactFirstName");
                    this.OnPrimaryContactFirstNameChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PrimaryContactLastName", DbType = "VarChar(50)")]
        public string PrimaryContactLastName
        {
            get
            {
                return this._PrimaryContactLastName;
            }
            set
            {
                if ((this._PrimaryContactLastName != value))
                {
                    this.OnPrimaryContactLastNameChanging(value);
                    this.SendPropertyChanging();
                    this._PrimaryContactLastName = value;
                    this.SendPropertyChanged("PrimaryContactLastName");
                    this.OnPrimaryContactLastNameChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PrimaryContactTitle", DbType = "VarChar(50)")]
        public string PrimaryContactTitle
        {
            get
            {
                return this._PrimaryContactTitle;
            }
            set
            {
                if ((this._PrimaryContactTitle != value))
                {
                    this.OnPrimaryContactTitleChanging(value);
                    this.SendPropertyChanging();
                    this._PrimaryContactTitle = value;
                    this.SendPropertyChanged("PrimaryContactTitle");
                    this.OnPrimaryContactTitleChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PrimaryContactEMail", DbType = "VarChar(50) NOT NULL", CanBeNull = false)]
        public string PrimaryContactEMail
        {
            get
            {
                return this._PrimaryContactEMail;
            }
            set
            {
                if ((this._PrimaryContactEMail != value))
                {
                    this.OnPrimaryContactEMailChanging(value);
                    this.SendPropertyChanging();
                    this._PrimaryContactEMail = value;
                    this.SendPropertyChanged("PrimaryContactEMail");
                    this.OnPrimaryContactEMailChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PrimaryContactTelephone", DbType = "VarChar(50) NOT NULL", CanBeNull = false)]
        public string PrimaryContactTelephone
        {
            get
            {
                return this._PrimaryContactTelephone;
            }
            set
            {
                if ((this._PrimaryContactTelephone != value))
                {
                    this.OnPrimaryContactTelephoneChanging(value);
                    this.SendPropertyChanging();
                    this._PrimaryContactTelephone = value;
                    this.SendPropertyChanged("PrimaryContactTelephone");
                    this.OnPrimaryContactTelephoneChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PrimaryContactCell", DbType = "VarChar(50)")]
        public string PrimaryContactCell
        {
            get
            {
                return this._PrimaryContactCell;
            }
            set
            {
                if ((this._PrimaryContactCell != value))
                {
                    this.OnPrimaryContactCellChanging(value);
                    this.SendPropertyChanging();
                    this._PrimaryContactCell = value;
                    this.SendPropertyChanged("PrimaryContactCell");
                    this.OnPrimaryContactCellChanged();
                }
            }
        }
        //[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SecondaryContactFirstName", DbType = "VarChar(50)")]
        //public string SecondaryContactFirstName
        //{
        //    get
        //    {
        //        return this._SecondaryContactFirstName;
        //    }
        //    set
        //    {
        //        if ((this._SecondaryContactFirstName != value))
        //        {
        //            this.OnSecondaryContactFirstNameChanging(value);
        //            this.SendPropertyChanging();
        //            this._SecondaryContactFirstName = value;
        //            this.SendPropertyChanged("SecondaryContactFirstName");
        //            this.OnSecondaryContactFirstNameChanged();
        //        }
        //    }
        //}
        //[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SecondaryContactLastName", DbType = "VarChar(50)")]
        //public string SecondaryContactLastName
        //{
        //    get
        //    {
        //        return this._SecondaryContactLastName;
        //    }
        //    set
        //    {
        //        if ((this._SecondaryContactLastName != value))
        //        {
        //            this.OnSecondaryContactLastNameChanging(value);
        //            this.SendPropertyChanging();
        //            this._SecondaryContactLastName = value;
        //            this.SendPropertyChanged("SecondaryContactLastName");
        //            this.OnSecondaryContactLastNameChanged();
        //        }
        //    }
        //}
        //[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SecondaryContactTitle", DbType = "VarChar(50)")]
        //public string SecondaryContactTitle
        //{
        //    get
        //    {
        //        return this._SecondaryContactTitle;
        //    }
        //    set
        //    {
        //        if ((this._SecondaryContactTitle != value))
        //        {
        //            this.OnSecondaryContactTitleChanging(value);
        //            this.SendPropertyChanging();
        //            this._SecondaryContactTitle = value;
        //            this.SendPropertyChanged("SecondaryContactTitle");
        //            this.OnSecondaryContactTitleChanged();
        //        }
        //    }
        //}
        //[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SecondaryContactEMail", DbType = "VarChar(50)")]
        //public string SecondaryContactEMail
        //{
        //    get
        //    {
        //        return this._SecondaryContactEMail;
        //    }
        //    set
        //    {
        //        if ((this._SecondaryContactEMail != value))
        //        {
        //            this.OnSecondaryContactEMailChanging(value);
        //            this.SendPropertyChanging();
        //            this._SecondaryContactEMail = value;
        //            this.SendPropertyChanged("SecondaryContactEMail");
        //            this.OnSecondaryContactEMailChanged();
        //        }
        //    }
        //}
        //[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SecondaryContactTelephone", DbType = "VarChar(50)")]
        //public string SecondaryContactTelephone
        //{
        //    get
        //    {
        //        return this._SecondaryContactTelephone;
        //    }
        //    set
        //    {
        //        if ((this._SecondaryContactTelephone != value))
        //        {
        //            this.OnSecondaryContactTelephoneChanging(value);
        //            this.SendPropertyChanging();
        //            this._SecondaryContactTelephone = value;
        //            this.SendPropertyChanged("SecondaryContactTelephone");
        //            this.OnSecondaryContactTelephoneChanged();
        //        }
        //    }
        //}
        //[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SecondaryContactCell", DbType = "VarChar(50)")]
        //public string SecondaryContactCell
        //{
        //    get
        //    {
        //        return this._SecondaryContactCell;
        //    }
        //    set
        //    {
        //        if ((this._SecondaryContactCell != value))
        //        {
        //            this.OnSecondaryContactCellChanging(value);
        //            this.SendPropertyChanging();
        //            this._SecondaryContactCell = value;
        //            this.SendPropertyChanged("SecondaryContactCell");
        //            this.OnSecondaryContactCellChanged();
        //        }
        //    }
        //}
        //[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TechnologyContactFirstName", DbType = "VarChar(50)")]
        //public string TechnologyContactFirstName
        //{
        //    get
        //    {
        //        return this._TechnologyContactFirstName;
        //    }
        //    set
        //    {
        //        if ((this._TechnologyContactFirstName != value))
        //        {
        //            this.OnTechnologyContactFirstNameChanging(value);
        //            this.SendPropertyChanging();
        //            this._TechnologyContactFirstName = value;
        //            this.SendPropertyChanged("TechnologyContactFirstName");
        //            this.OnTechnologyContactFirstNameChanged();
        //        }
        //    }
        //}
        //[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TechnologyContactLastName", DbType = "VarChar(50)")]
        //public string TechnologyContactLastName
        //{
        //    get
        //    {
        //        return this._TechnologyContactLastName;
        //    }
        //    set
        //    {
        //        if ((this._TechnologyContactLastName != value))
        //        {
        //            this.OnTechnologyContactLastNameChanging(value);
        //            this.SendPropertyChanging();
        //            this._TechnologyContactLastName = value;
        //            this.SendPropertyChanged("TechnologyContactLastName");
        //            this.OnTechnologyContactLastNameChanged();
        //        }
        //    }
        //}
        //[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TechnologyContactTitle", DbType = "VarChar(50)")]
        //public string TechnologyContactTitle
        //{
        //    get
        //    {
        //        return this._TechnologyContactTitle;
        //    }
        //    set
        //    {
        //        if ((this._TechnologyContactTitle != value))
        //        {
        //            this.OnTechnologyContactTitleChanging(value);
        //            this.SendPropertyChanging();
        //            this._TechnologyContactTitle = value;
        //            this.SendPropertyChanged("TechnologyContactTitle");
        //            this.OnTechnologyContactTitleChanged();
        //        }
        //    }
        //}
        //[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TechnologyContactEMail", DbType = "VarChar(50)")]
        //public string TechnologyContactEMail
        //{
        //    get
        //    {
        //        return this._TechnologyContactEMail;
        //    }
        //    set
        //    {
        //        if ((this._TechnologyContactEMail != value))
        //        {
        //            this.OnTechnologyContactEMailChanging(value);
        //            this.SendPropertyChanging();
        //            this._TechnologyContactEMail = value;
        //            this.SendPropertyChanged("TechnologyContactEMail");
        //            this.OnTechnologyContactEMailChanged();
        //        }
        //    }
        //}
        //[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TechnologyContactTelephone", DbType = "VarChar(50)")]
        //public string TechnologyContactTelephone
        //{
        //    get
        //    {
        //        return this._TechnologyContactTelephone;
        //    }
        //    set
        //    {
        //        if ((this._TechnologyContactTelephone != value))
        //        {
        //            this.OnTechnologyContactTelephoneChanging(value);
        //            this.SendPropertyChanging();
        //            this._TechnologyContactTelephone = value;
        //            this.SendPropertyChanged("TechnologyContactTelephone");
        //            this.OnTechnologyContactTelephoneChanged();
        //        }
        //    }
        //}
        //[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TechnologyContactCell", DbType = "VarChar(50)")]
        //public string TechnologyContactCell
        //{
        //    get
        //    {
        //        return this._TechnologyContactCell;
        //    }
        //    set
        //    {
        //        if ((this._TechnologyContactCell != value))
        //        {
        //            this.OnTechnologyContactCellChanging(value);
        //            this.SendPropertyChanging();
        //            this._TechnologyContactCell = value;
        //            this.SendPropertyChanged("TechnologyContactCell");
        //            this.OnTechnologyContactCellChanged();
        //        }
        //    }
        //}
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ShortName", DbType = "VarChar(50)")]
        public string ShortName
        {
            get
            {
                return this._ShortName;
            }
            set
            {
                if ((this._ShortName != value))
                {
                    this.OnShortNameChanging(value);
                    this.SendPropertyChanging();
                    this._ShortName = value;
                    this.SendPropertyChanged("ShortName");
                    this.OnShortNameChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Login", DbType = "VarChar(50)")]
        public string Login
        {
            get
            {
                return this._Login;
            }
            set
            {
                if ((this._Login != value))
                {
                    this.OnLoginChanging(value);
                    this.SendPropertyChanging();
                    this._Login = value;
                    this.SendPropertyChanged("Login");
                    this.OnLoginChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Password", DbType = "VarChar(50)")]
        public string Password
        {
            get
            {
                return this._Password;
            }
            set
            {
                if ((this._Password != value))
                {
                    this.OnPasswordChanging(value);
                    this.SendPropertyChanging();
                    this._Password = value;
                    this.SendPropertyChanged("Password");
                    this.OnPasswordChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CountryID", DbType = "Int")]
        public System.Nullable<int> CountryID
        {
            get
            {
                return this._CountryID;
            }
            set
            {
                if ((this._CountryID != value))
                {
                    this.OnCountryIDChanging(value);
                    this.SendPropertyChanging();
                    this._CountryID = value;
                    this.SendPropertyChanged("CountryID");
                    this.OnCountryIDChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_StateID", DbType = "Int")]
        public System.Nullable<int> StateID
        {
            get
            {
                return this._StateID;
            }
            set
            {
                if ((this._StateID != value))
                {
                    this.OnStateIDChanging(value);
                    this.SendPropertyChanging();
                    this._StateID = value;
                    this.SendPropertyChanged("StateID");
                    this.OnStateIDChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Zip", DbType = "VarChar(50)")]
        public string Zip
        {
            get
            {
                return this._Zip;
            }
            set
            {
                if ((this._Zip != value))
                {
                    this.OnZipChanging(value);
                    this.SendPropertyChanging();
                    this._Zip = value;
                    this.SendPropertyChanged("Zip");
                    this.OnZipChanged();
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
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SupportsMultipleCurrencies", DbType = "Int")]
        public System.Nullable<int> SupportsMultipleCurrencies
        {
            get
            {
                return this._SupportsMultipleCurrencies;
            }
            set
            {
                if ((this._SupportsMultipleCurrencies != value))
                {
                    this.OnSupportsMultipleCurrenciesChanging(value);
                    this.SendPropertyChanging();
                    this._SupportsMultipleCurrencies = value;
                    this.SendPropertyChanged("SupportsMultipleCurrencies");
                    this.OnSupportsMultipleCurrenciesChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_City", DbType = "VarChar(50)")]
        public string City
        {
            get
            {
                return this._City;
            }
            set
            {
                if ((this._City != value))
                {
                    this.OnCityChanging(value);
                    this.SendPropertyChanging();
                    this._City = value;
                    this.SendPropertyChanged("City");
                    this.OnCityChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DefaultAUECID", DbType = "Int")]
        public System.Nullable<int> DefaultAUECID
        {
            get
            {
                return this._DefaultAUECID;
            }
            set
            {
                if ((this._DefaultAUECID != value))
                {
                    this.OnDefaultAUECIDChanging(value);
                    this.SendPropertyChanging();
                    this._DefaultAUECID = value;
                    this.SendPropertyChanged("DefaultAUECID");
                    this.OnDefaultAUECIDChanged();
                }
            }
        }
        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "T_Company_T_CompanyFund", Storage = "_T_CompanyFunds", ThisKey = "CompanyID", OtherKey = "CompanyID")]
        public EntitySet<T_CompanyFund> T_CompanyFunds
        {
            get
            {
                return this._T_CompanyFunds;
            }
            set
            {
                this._T_CompanyFunds.Assign(value);
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
        private void attach_T_CompanyFunds(T_CompanyFund entity)
        {
            this.SendPropertyChanging();
            entity.T_Company = this;
        }
        private void detach_T_CompanyFunds(T_CompanyFund entity)
        {
            this.SendPropertyChanging();
            entity.T_Company = null;
        }
    }
}
