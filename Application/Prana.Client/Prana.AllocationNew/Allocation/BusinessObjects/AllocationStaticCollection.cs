using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Prana.CommonDataCache;

namespace Prana.AllocationNew
{
    class AllocationStaticCollection
    {
        static AccountCollection _accounts = new AccountCollection();
        static StrategyCollection _strategies = new StrategyCollection();
        static AllocationStaticCollection()
        {
            _accounts.Clear();
            _strategies = CachedDataManager.GetInstance.GetUserStrategies();
            AccountCollection accounts = CommonDataCache.CachedDataManager.GetInstance.GetUserAccounts();
            foreach (Account account in accounts)
            {
                if (account.AccountID != int.MinValue)
                {
                    _accounts.Add(account);
                }
            }
        }

        public static AccountCollection Accounts
        {
            get { return _accounts; }
        }
        public static StrategyCollection Strategies
        {
            get { return _strategies; }
        }
    }
}
