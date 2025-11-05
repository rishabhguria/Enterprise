namespace Prana.ServiceGateway.Models
{
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
    }

    public enum AuthenticationTypes
    {
        EnterpriseLoggedIn,
        WebLoggedIn,
        InvalidCredentials,
        WebAlreadyLoggedInForAnotherWebSession
    }
}
