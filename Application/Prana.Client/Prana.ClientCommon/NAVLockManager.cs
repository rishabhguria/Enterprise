using Prana.BusinessObjects;
using Prana.ClientCommon.Data_Layer;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ClientCommon
{
    public class NAVLockManager : IDisposable
    {
        private static object _lockerObject = new object();
        static NAVLockManager _NAVLockManager = null;
        static Dictionary<int, NAVLockItem> _accountNAVlockDetailDict = new Dictionary<int, NAVLockItem>();

        /// <summary>
        /// 
        /// </summary>
        public static NAVLockManager GetInstance
        {
            get
            {

                lock (_lockerObject)
                {
                    if (_NAVLockManager == null)
                    {
                        _NAVLockManager = new NAVLockManager();
                        _accountNAVlockDetailDict = CommonDataManager.GetAccountNAVLockDetails();
                    }
                    return _NAVLockManager;
                }
            }
        }

        /// <summary>
        /// get NAV Lock Item Details of a account
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="TradeDate"></param>
        /// <returns></returns>
        public NAVLockItem getNAVLockItemDetails(int AccountId)
        {
            try
            {
                if (_accountNAVlockDetailDict.ContainsKey(AccountId))
                {
                    return _accountNAVlockDetailDict[AccountId];
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
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="TradeDate"></param>
        /// <returns></returns>
        public Boolean ValidateTrade(int AccountId, DateTime TradeDate)
        {
            bool isTradeAllowed = false;
            try
            {
                Boolean isAccountNAVLockingEnabled = CachedDataManager.GetInstance.IsNAVLockingEnabled();

                if (!isAccountNAVLockingEnabled || !_accountNAVlockDetailDict.ContainsKey(AccountId))
                    return true;

                NAVLockItem accountNAVDetail = _accountNAVlockDetailDict[AccountId];

                //  DateTime tradeDateInGMT = TradeDate.ToUniversalTime();
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1185
                int result = DateTime.Compare(accountNAVDetail.LockAppliedDate.Date, TradeDate.Date);
                if (result < 0)
                {
                    isTradeAllowed = true;
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
            return isTradeAllowed;
        }

        /// <summary>
        /// Refresh Account wise nav lock details
        /// </summary>
        public void RefreshAccountNavLockDetails()
        {
            try
            {
                _accountNAVlockDetailDict.Clear();
                _accountNAVlockDetailDict = CommonDataManager.GetAccountNAVLockDetails();
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


        public DateTime GetNavLockDateForAccount(int accountID)
        {
            DateTime navLockDate = DateTime.MinValue;
            try
            {
                if (_accountNAVlockDetailDict.ContainsKey(accountID))
                {
                    NAVLockItem accountNAVDetail = _accountNAVlockDetailDict[accountID];
                    navLockDate = accountNAVDetail.LockAppliedDate.Date;
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
            return navLockDate;
        }
        /// <summary>
        /// Returns accountName and NavLock Item as a Dictionary
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, NAVLockItem> GetNavLockItemDetails()
        {
            Dictionary<string, NAVLockItem> accountNavLockItemDetails = new Dictionary<string, NAVLockItem>();
            try
            {
                if (_accountNAVlockDetailDict != null)
                {
                    foreach (KeyValuePair<int, NAVLockItem> kvp in _accountNAVlockDetailDict)
                    {
                        string accountName = CachedDataManager.GetInstance.GetAccountText(kvp.Key).ToString();
                        if (!accountNavLockItemDetails.ContainsKey(accountName))
                        {
                            accountNavLockItemDetails.Add(accountName, kvp.Value);
                        }
                        else if (accountNavLockItemDetails[accountName] != kvp.Value)
                        {
                            accountNavLockItemDetails[accountName] = kvp.Value;
                        }
                    }
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
            return accountNavLockItemDetails;
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    if (_accountNAVlockDetailDict != null)
                        _accountNAVlockDetailDict = null;
                    if (_NAVLockManager != null)
                        _NAVLockManager = null;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        #endregion
    }
}
