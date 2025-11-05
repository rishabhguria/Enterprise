using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using System.Data;


namespace Prana.AllocationNew
{
    class AccountStrategyMapping
    {

        static AccountCollection accounts = new AccountCollection();
        static StrategyCollection strategies =new StrategyCollection();
        static AccountStrategyMapping()
        {
            AccountCollection accounts1 = CommonDataCache.CachedDataManager.GetInstance.GetUserAccounts();
            foreach (Account account in accounts1)
            {
                if (account.AccountID != int.MinValue)
                {
                    accounts.Add(account);
                }
            }
            StrategyCollection strategies1 = CommonDataCache.CachedDataManager.GetInstance.GetUserStrategies();
            foreach (Strategy strategy in strategies1)
            {
                if (strategy.StrategyID != int.MinValue)
                {
                    strategies.Add(strategy);
                }
            }
        }
        public static  DataTable GetAccountTable()
        {
            int rowCount = 1;
            DataTable dt = new DataTable();
            dt.Columns.Add("Account");
            dt.Columns.Add("Account %");
            dt.Columns.Add("Account Qty");
            foreach (Strategy strategy in strategies)
            {
                dt.Columns.Add(strategy.Name+" %");
                dt.Columns.Add(strategy.Name+" Qty");
                rowCount++;
            }
            foreach (Account account in accounts)
            {
                object[] row = new object[(rowCount)*2];
                row[0] = account.Name;
                dt.Rows.Add(row);
            }
            return dt;
            
        }
        public static int GetAccountLocation(int accountID)
        {
            return accounts.IndexOf(accountID);
        }
        public static int GetStrategyLocation(int strategyID)
        {
            return strategies.IndexOf(strategyID);
        }

        public static int GetAccountIDByLocation(int locationID)
        {

            return ((Account)accounts[locationID]).AccountID;
        }
        public static int GetStrategyIDByLocation(int locationID)
        {
            return ((Strategy )strategies [locationID]).StrategyID;
        }
        
    }
}
