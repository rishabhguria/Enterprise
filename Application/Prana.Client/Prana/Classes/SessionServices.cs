using Prana.BusinessObjects;
using Prana.Interfaces;

namespace Prana
{
    public class SessionServices : ISessionServices
    {
        CompanyUser _loginUser = null;
        public void SetLoggedInUser(CompanyUser loginUser)
        {
            _loginUser = loginUser;
        }
        public CompanyUser GetLoggedInUser()
        {
            return _loginUser;
        }
    }
}
