using Prana.BusinessObjects.Enums;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.ReconciliationNew
{
    public class CachedDataRecon
    {
        string _applicationVersion = null;

        public string ApplicationVersion
        {
            get { return _applicationVersion; }
        }

        #region Dashboard Cache Field
        /// <summary>
        /// <string,string>is <ExecutionName,DashboardFilePath>
        /// </summary>
        private Dictionary<string, string> _dictDashboardBatches = new Dictionary<string, string>();
        public Dictionary<string, string> DictDashboardBatches
        {
            get
            {
                return _dictDashboardBatches;
            }
        }
        #endregion

        static AuthAction _permissionLevel = AuthAction.None;

        public AuthAction PermissionLevel
        {
            get { return _permissionLevel; }
        }


        private static CachedDataRecon _cachedData = null;

        static CachedDataRecon()
        {
            try
            {
                int UserID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                _cachedData = new CachedDataRecon();

                SetCommonData(UserID);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        public static CachedDataRecon GetInstance()
        {
            return _cachedData;
        }

        static Dictionary<int, string> _accounts = new Dictionary<int, string>();
        static Dictionary<int, List<int>> _companyAccounts = new Dictionary<int, List<int>>();
        static Dictionary<int, List<int>> _companyThirdPartyAccounts = new Dictionary<int, List<int>>();

        /// <summary>
        /// function to initialize accounts, thirdparty and company dictionaries.
        /// </summary>
        /// <param name="UserID"></param>
        public static void SetCommonData(int UserID)
        {
            try
            {
                _accounts.Clear();
                _companyAccounts.Clear();
                _companyThirdPartyAccounts.Clear();
                //Modified By Faisal Shah 0n 06/25/2014
                // Had to refresh the cache for accounts . So created a new method GetAllPermittedAccounts()
                // in ClientsCommonDataManager.cs and removed that from DatabaseManager.cs
                DataTable dtAccounts = WindsorContainerManager.GetAllPermittedAccounts(UserID);
                //DataTable dtAccounts = DatabaseManager.GetInstance().GetAllPermittedAccounts(UserID);
                FillAccountDictionaries(dtAccounts);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Function to fill Account dictionaries alongwith company and thirdparty ID
        /// </summary>
        /// <param name="dtAccounts"></param>
        private static void FillAccountDictionaries(DataTable dtAccounts)
        {
            try
            {
                foreach (DataRow dr in dtAccounts.Rows)
                {
                    int thirdPartyID = 0;
                    if (!_accounts.ContainsKey(int.Parse(dr[0].ToString().Trim())))
                    {
                        _accounts.Add(int.Parse(dr[0].ToString()), dr[1].ToString());
                    }
                    if (_companyAccounts.ContainsKey(int.Parse(dr[2].ToString().Trim())))
                    {
                        _companyAccounts[int.Parse(dr[2].ToString())].Add(int.Parse(dr[0].ToString()));
                    }
                    else
                    {
                        List<int> accountCollection = new List<int>();
                        accountCollection.Add(int.Parse(dr[0].ToString()));
                        _companyAccounts.Add(int.Parse(dr[2].ToString()), accountCollection);
                    }
                    int.TryParse(dr[3].ToString(), out thirdPartyID);
                    if (_companyThirdPartyAccounts.ContainsKey(thirdPartyID))
                    {
                        _companyThirdPartyAccounts[thirdPartyID].Add(int.Parse(dr[0].ToString()));
                    }
                    else
                    {
                        List<int> accountCollection = new List<int>();
                        accountCollection.Add(int.Parse(dr[0].ToString()));
                        _companyThirdPartyAccounts.Add(thirdPartyID, accountCollection);
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
        }

        public Dictionary<int, string> Accounts
        {
            get { return _accounts; }
        }

        public Dictionary<int, List<int>> CompanyAccounts
        {
            get { return _companyAccounts; }
        }

        public Dictionary<int, List<int>> CompanyThirdPartyAccounts
        {
            get { return _companyThirdPartyAccounts; }
        }

        /// <summary>
        ///  Sets the permission in Cache
        /// </summary>
        /// <param name="permissionLevel"></param>
        internal void SetUserPermissionLevel(AuthAction permissionLevel)
        {
            try
            {
                _permissionLevel = permissionLevel;
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
    }
}
