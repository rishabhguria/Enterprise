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
    class MasterFund
    {
        #region SingiltonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static MasterFund _singiltonObject = null;

        /// <summary>
        /// private cunstructor
        /// </summary>
        private MasterFund()
        {

        }

        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        internal static MasterFund GetInstance()
        {
            lock (_lock)
            {
                if (_singiltonObject == null)
                    _singiltonObject = new MasterFund();
                return _singiltonObject;
            }
        }
        #endregion

        private Dictionary<String, Double> _masterFundCache = null;

        /// <summary>
        /// Loads the account list from Esper calculator
        /// </summary>
        /// <returns></returns>
        internal Boolean Initialize()
        {
            try
            {
                DataTable dt = ComplianceServiceConnector.GetInstance().GetCalculations(Compression.MasterFund, new List<String>() { "masterFundNav" });
                if (dt != null)
                {
                    _masterFundCache = new Dictionary<String, Double>();
                    var rows = dt.Select("", "masterFundName");
                    foreach (DataRow row in rows)
                    {
                        _masterFundCache.Add(row["masterFundName"].ToString(), Convert.ToDouble(row["masterFundNav"]));
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
        internal List<EnumerationValue> GetMasterFunds()
        {
            List<EnumerationValue> lst = new List<EnumerationValue>();
            var list = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAllMasterFunds().Values.ToList();
            list.Sort();
            foreach (String account in list)
            {
                lst.Add(new EnumerationValue(account, account));
            }
            return lst;
        }

        internal Boolean MasterFundExists(String account)
        {
            return Prana.CommonDataCache.CachedDataManager.GetInstance.GetAllMasterFunds().Values.ToList().Contains(account);
        }

        internal Double GetNAV(String account)
        {
            try
            {
                return _masterFundCache[account];
            }
            catch (Exception)
            {
                return 0.0;
            }
        }
    }
}
