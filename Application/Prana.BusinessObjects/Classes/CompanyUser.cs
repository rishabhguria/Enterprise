using System;
using System.Collections;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for CompanyUser.
    /// </summary>
    [Serializable]
    public class CompanyUser
    {
        public CompanyUser()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Local variables	
        int _companyUserID = int.MinValue;
        string _loginID = string.Empty;
        int _companyID = int.MinValue;
        string _lastName = string.Empty;
        string _firstName = string.Empty;
        string _shortName = string.Empty;
        string _title = string.Empty;
        string _mailingAddress = string.Empty;
        string _eMail = string.Empty;
        string _telephoneWork = string.Empty;
        string _telephoneHome = string.Empty;
        string _telephoneMobile = string.Empty;
        string _fax = string.Empty;
        List<TradingAccount> _tradingAccounts = new List<TradingAccount>();
        string _companyName = string.Empty;
        List<string> _marketDataTypes = new List<string>();
        string _factSetUsernameAndSerialNumber = string.Empty;
        bool _isFactSetSupportUser = false;
        string _marketDataAccessIPAddresses = string.Empty;
        string _activUsername = string.Empty;
        string _activPassword = string.Empty;
        bool _hasPowerBIAccess = false;
        string _sapiUsername = string.Empty;
        #endregion

        #region Properties
        public int CompanyUserID
        {
            get { return _companyUserID; }
            set { _companyUserID = value; }
        }

        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }

        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string ShortName
        {
            get { return _shortName; }
            set { _shortName = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string MailingAddress
        {
            get { return _mailingAddress; }
            set { _mailingAddress = value; }
        }

        public string EMail
        {
            get { return _eMail; }
            set { _eMail = value; }
        }

        public string TelephoneWork
        {
            get { return _telephoneWork; }
            set { _telephoneWork = value; }
        }

        public string TelephoneHome
        {
            get { return _telephoneHome; }
            set { _telephoneHome = value; }
        }

        public string TelephoneMobile
        {
            get { return _telephoneMobile; }
            set { _telephoneMobile = value; }
        }

        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        public string LoginID
        {
            get { return _loginID; }
            set { _loginID = value; }
        }

        public List<TradingAccount> TradingAccounts
        {
            get { return _tradingAccounts; }
            set { _tradingAccounts = value; }
        }

        public List<string> MarketDataTypes
        {
            get { return _marketDataTypes; }
            set { _marketDataTypes = value; }
        }

        public string FactSetUsernameAndSerialNumber
        {
            get { return _factSetUsernameAndSerialNumber; }
            set { _factSetUsernameAndSerialNumber = value; }
        }

        public bool IsFactSetSupportUser
        {
            get { return _isFactSetSupportUser; }
            set { _isFactSetSupportUser = value; }
        }

        public string MarketDataAccessIPAddresses
        {
            get { return _marketDataAccessIPAddresses; }
            set { _marketDataAccessIPAddresses = value; }
        }

        public string ActivUsername
        {
            get { return _activUsername; }
            set { _activUsername = value; }
        }

        public string ActivPassword
        {
            get { return _activPassword; }
            set { _activPassword = value; }
        }
        public bool HasPowerBIAccess
        {
            get { return _hasPowerBIAccess; }
            set { _hasPowerBIAccess = value; }
        }
        public string SapiUsername
        {
            get { return _sapiUsername; }
            set { _sapiUsername = value; }
        }
        #endregion
    }
}
