using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;

namespace Prana.ClientCommon.BLL
{
    public class AccountLockManager
    {
        static ProxyBase<IPranaPositionServices> _pranaPositionServices = null;

        /// <summary>
        /// Create Position Services Proxy to connect with Trade Server
        /// </summary>
        public static void CreatePositionServicesProxy()
        {
            try
            {
                if (_pranaPositionServices == null)
                {
                    _pranaPositionServices = new ProxyBase<IPranaPositionServices>("TradePositionServiceEndpointAddress");
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Set the accounts to be locked by user on server Cache
        /// </summary>
        /// <param name="accountsToBeLocked"></param>
        /// <returns></returns>
        public static bool SetAccountsLockStatus(List<int> accountsToBeLocked)
        {
            bool isSucessuful = false;
            try
            {
                //GetUserPermittedCompanyList
                int userID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                if (_pranaPositionServices == null)
                {
                    CreatePositionServicesProxy();
                }
                //return true if the Account/CashAccounts required is not locked by any other user
                isSucessuful = _pranaPositionServices.InnerChannel.SetAccountsLockStatus(userID, accountsToBeLocked);
                if (isSucessuful)
                {
                    foreach (KeyValuePair<int, string> account in CachedDataManager.GetInstance.GetAccountsWithFullName())
                    {
                        //new Account Locked	
                        if (accountsToBeLocked.Contains(account.Key) && !CachedDataManager.GetInstance.GetLockedAccounts().Contains(account.Key))
                        {
                            CachedDataManager.GetInstance.ResetAccountsLockTimer(account.Key, 0);
                        }
                        //Account Released
                        if (!accountsToBeLocked.Contains(account.Key) && CachedDataManager.GetInstance.GetLockedAccounts().Contains(account.Key))
                        {
                            CachedDataManager.GetInstance.ReleaseAccount(account.Key);
                        }
                    }
                    CachedDataManager.GetInstance.SetLockedAccounts(accountsToBeLocked);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return isSucessuful;
        }
    }
}
