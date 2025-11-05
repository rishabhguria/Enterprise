using Prana.BusinessObjects;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.LogManager;
using Prana.ServiceConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Act40OrderGeneratorTool.Cache
{
    class Account
    {
        #region SingiltonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static Account _singiltonObject = null;

        /// <summary>
        /// private cunstructor
        /// </summary>
        private Account()
        {

        }

        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        internal static Account GetInstance()
        {
            lock (_lock)
            {
                if (_singiltonObject == null)
                    _singiltonObject = new Account();
                return _singiltonObject;
            }
        }
        #endregion

        private Dictionary<String, Double> _accountCache = null;

        /// <summary>
        /// Loads the account list from Esper calculator
        /// </summary>
        /// <returns></returns>
        internal Boolean Initialize()
        {
            try
            {
                DataTable dt = ComplianceServiceConnector.GetInstance().GetCalculations(Compression.Account, new List<String>() { "accountNav" });
                if (dt != null)
                {
                    _accountCache = new Dictionary<String, Double>();
                    var rows = dt.Select("", "accountShortName");
                    foreach (DataRow row in rows)
                    {
                        _accountCache.Add(row["accountShortName"].ToString(), Convert.ToDouble(row["accountNav"]));
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a bindable list of accounts
        /// </summary>
        /// <returns></returns>
        internal List<EnumerationValue> GetAccounts()
        {
            List<EnumerationValue> lst = new List<EnumerationValue>();
            var list = Prana.CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict().Values.ToList();
            list.Sort();
            foreach (String account in list)
            {
                lst.Add(new EnumerationValue(account, account));
            }
            return lst;
        }

        internal Boolean AccountExists(String account)
        {
            return Prana.CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict().Values.ToList().Contains(account);
        }


        internal Double GetNAV(String account)
        {
            try
            {
                return _accountCache[account];
            }
            catch (Exception)
            {
                return 0.0;
            }
        }
    }
}
