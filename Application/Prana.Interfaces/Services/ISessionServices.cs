using Prana.BusinessObjects;

namespace Prana.Interfaces
{
    public interface ISessionServices
    {
        void SetLoggedInUser(CompanyUser loginUser);

        CompanyUser GetLoggedInUser();

    }
}
