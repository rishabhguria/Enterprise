using Prana.BusinessObjects;
using Prana.CommonDataCache;
namespace Prana.ClientCommon
{
    public class PermissionCache
    {
        static CompanyUser _LoggedInUser;
        public static void SetDetails(CompanyUser user)
        {
            bool shouldReset = false;
            if (_LoggedInUser == null)
            {
                shouldReset = true;
            }
            else if (_LoggedInUser.CompanyUserID != user.CompanyUserID)
            {
                shouldReset = true;
                _LoggedInUser = user;
            }
            else
            {
                _LoggedInUser = user;
            }

            if (shouldReset)
            {
                CachedDataManager.GetInstance.StartCaching(_LoggedInUser);
                TradingTktPrefs.SetClientCache(user);
            }
        }
        public CompanyUser LoggedInUser
        {
            get { return _LoggedInUser; }
        }
    }
}
