using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using System;

namespace Prana.Authentication.Common
{
    [Serializable]
    public class AuthenticatedUserInfo
    {
        private int _companyUserId = int.MinValue;
        public int CompanyUserId
        {
            get { return _companyUserId; }
            set { _companyUserId = value; }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        private string _token = string.Empty;
        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }

        private AuthenticationTypes _authenticationType;

        public AuthenticationTypes AuthenticationType
        {
            get { return _authenticationType; }
            set { _authenticationType = value; }
        }

        private CompanyUser _companyUser = null;
        public CompanyUser CompanyUser
        {
            get { return _companyUser; }
            set { _companyUser = value; }
        }

        MarketDataProvider _companyMarketDataProvider;
        public MarketDataProvider CompanyMarketDataProvider
        {
            get { return _companyMarketDataProvider; }
            set { _companyMarketDataProvider = value; }
        }

        private FactSetContractType _companyFactSetContractType;
        public FactSetContractType CompanyFactSetContractType
        {
            get { return _companyFactSetContractType; }
            set { _companyFactSetContractType = value; }
        }

        bool _isMarketDataBlocked;
        public bool IsMarketDataBlocked
        {
            get { return _isMarketDataBlocked; }
            set { _isMarketDataBlocked = value; }
        }
    }

    public enum AuthenticationTypes
    {
        EnterpriseLoggedIn,
        WebLoggedIn,
        InvalidCredentials,
        WebAlreadyLoggedInForAnotherWebSession,
        InvalidPassword,
    }
}