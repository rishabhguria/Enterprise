using Prana.Allocation.Core.DataAccess;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prana.Allocation.Core.CacheStore
{
    internal sealed class CalculatedPreferenceCache
    {
        #region Members

        /// <summary>
        /// The singelton instance
        /// </summary>
        private static CalculatedPreferenceCache _singeltonInstance = new CalculatedPreferenceCache();

        /// <summary>
        /// The locker
        /// </summary>
        private static readonly object _locker = new object();

        /// <summary>
        /// Locker object for cache
        /// </summary>
        private readonly object _calcPrefCacheLocker = new object();

        /// <summary>
        /// Id wise preference for allocation
        /// </summary>
        private SerializableDictionary<int, AllocationOperationPreference> _calculatedPreferencesCache;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        internal static CalculatedPreferenceCache Instance
        {
            get
            {
                if (_singeltonInstance == null)
                {
                    lock (_locker)
                    {
                        if (_singeltonInstance == null)
                            _singeltonInstance = new CalculatedPreferenceCache();
                    }
                }
                return _singeltonInstance;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="CalculatedPreferenceCache" /> class from being created.
        /// </summary>
        private CalculatedPreferenceCache()
        {
            try
            {
                // Do cache object initialization which should be done before Initialize() method
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal void Initialize()
        {
            try
            {
                lock (_calcPrefCacheLocker)
                {
                    _calculatedPreferencesCache = DataAccess.AllocationPrefDataManager.GetCompanyWisePreference();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Adds the given preference information to database and cache
        /// </summary>
        /// <param name="name">Name of the preference</param>
        /// <param name="companyId">companyId of the preference</param>
        /// <param name="isPrefVisible">if set to <c>true</c> [is preference visible].</param>
        /// <returns>
        /// Update result of the preference
        /// </returns>
        internal PreferenceUpdateResult AddPreference(string name, int companyId, bool isPrefVisible, string rebalancerFileName = "")
        {
            try
            {
                PreferenceUpdateResult result = null;
                bool isValid = false;
                // Check preference for validity in cache
                AllocationOperationPreference pref = GetPreferenceByCompanyIdAndName(name, companyId);
                if (pref == null && !String.IsNullOrWhiteSpace(name))
                    isValid = true;

                if (isValid)
                    result = DataAccess.AllocationPrefDataManager.AddPreference(companyId, name, AllocationPreferencesType.CalculatedAllocationPreference, isPrefVisible, rebalancerFileName);
                else
                    result = new PreferenceUpdateResult() { Error = "Preference is invalid.", Preference = null };

                // If saving is successful in database then update preference in cache
                if (result.Error == null)
                {
                    lock (_calcPrefCacheLocker)
                    {
                        if (_calculatedPreferencesCache.ContainsKey(result.Preference.OperationPreferenceId))
                            _calculatedPreferencesCache.Remove(result.Preference.OperationPreferenceId);

                        _calculatedPreferencesCache.Add(result.Preference.OperationPreferenceId, result.Preference.Clone());
                    }
                }
                else
                {
                    result.Error += " Could not add preference";
                }
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Copy preference with new name
        /// </summary>
        /// <param name="preferenceId">preferenceId from which to be copied</param>
        /// <param name="name">Name of the new preference</param>
        /// <returns>
        /// PreferenceUpdate result
        /// </returns>
        internal PreferenceUpdateResult CopyPreference(int preferenceId, string name)
        {
            try
            {
                PreferenceUpdateResult result;
                bool isValid = false;

                int companyId = GetPreferenceById(preferenceId).CompanyId;

                // Check preference for validity in cache
                AllocationOperationPreference pref = GetPreferenceByCompanyIdAndName(name, companyId);
                if (pref == null && !String.IsNullOrWhiteSpace(name))
                    isValid = true;

                if (isValid)
                    result = DataAccess.AllocationPrefDataManager.CopyPreference(preferenceId, name);
                else
                    result = new PreferenceUpdateResult() { Error = "Preference is invalid.", Preference = null };

                // If saving is successful in database then update preference in cache
                if (result.Error == null)
                {
                    lock (_calcPrefCacheLocker)
                    {
                        if (_calculatedPreferencesCache.ContainsKey(result.Preference.OperationPreferenceId))
                            _calculatedPreferencesCache.Remove(result.Preference.OperationPreferenceId);

                        _calculatedPreferencesCache.Add(result.Preference.OperationPreferenceId, result.Preference.Clone());
                    }
                    return result;
                }
                else
                {
                    //Exception ex = new Exception("Could not update preference", result.Error);
                    result.Error += " Could not copy preference";
                    return result;// If saving is unsuccessful in database then return false
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Delete preference of given preferenceId
        /// </summary>
        /// <param name="preferenceId">Id of preference which to be deleted</param>
        /// <returns>
        /// PreferenceUpdate result
        /// </returns>
        internal PreferenceUpdateResult DeletePreference(int preferenceId)
        {
            try
            {
                PreferenceUpdateResult result = null;
                bool isValid = true;

                // Check preference for validity in cache
                AllocationOperationPreference pref = GetPreferenceById(preferenceId);
                if (pref == null)
                    isValid = false;

                if (isValid)
                    result = DataAccess.AllocationPrefDataManager.DeletePreference(preferenceId);
                else
                    result = new PreferenceUpdateResult() { Error = "Preference doen not exist.", Preference = null };

                // If saving is successful in database then update preference in cache
                if (result.Error == null)
                {
                    lock (_calcPrefCacheLocker)
                    {
                        if (_calculatedPreferencesCache.ContainsKey(preferenceId))
                            _calculatedPreferencesCache.Remove(preferenceId);
                    }
                }
                else
                {
                    result.Error += " Could not delete preference";
                }

                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Import the AllocationOperationPreference
        /// </summary>
        /// <param name="preference">Preference to be imported</param>
        /// <returns>
        /// PreferenceUpdateResult, not successful if contains error
        /// </returns>
        internal PreferenceUpdateResult ImportPreference(AllocationOperationPreference preference)
        {
            try
            {
                PreferenceUpdateResult result;
                bool isValid = false;

                // Check preference for validity in cache
                if (!GetAllPreferenceId().Contains(preference.OperationPreferenceId) && preference.IsValid())
                    isValid = true;

                if (isValid && !String.IsNullOrWhiteSpace(preference.OperationPreferenceName))
                    result = DataAccess.AllocationPrefDataManager.ImportPreference(preference);
                else
                    result = new PreferenceUpdateResult() { Error = string.Format("\"{0}\" is an invalid preference.", preference.OperationPreferenceName), Preference = null };

                // If saving is successful in database then update preference in cache
                if (result.Error == null)
                {
                    lock (_calcPrefCacheLocker)
                    {
                        if (_calculatedPreferencesCache.ContainsKey(result.Preference.OperationPreferenceId))
                            _calculatedPreferencesCache.Remove(preference.OperationPreferenceId);

                        _calculatedPreferencesCache.Add(result.Preference.OperationPreferenceId, result.Preference.Clone());
                    }
                    return result;
                }
                else
                {
                    //Exception ex = new Exception("Could not update preference", result.Error);
                    result.Error += " Could not update preference";
                    return result;// If saving is unsuccessful in database then return false
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Rename the preference with given preferenceId
        /// </summary>
        /// <param name="preferenceId">Id of the preference which will be renamed</param>
        /// <param name="name">New Name of the preference</param>
        /// <returns>
        /// Update result object
        /// </returns>
        internal PreferenceUpdateResult RenamePreference(int preferenceId, string name)
        {
            try
            {
                PreferenceUpdateResult result = null;
                bool isValid = false;

                int companyId = GetPreferenceById(preferenceId).CompanyId;
                // Check preference for validity in cache
                AllocationOperationPreference pref = GetPreferenceByCompanyIdAndName(name, companyId);
                if (pref == null && !String.IsNullOrWhiteSpace(name))
                    isValid = true;

                if (isValid)
                    result = DataAccess.AllocationPrefDataManager.RenamePreference(preferenceId, name);
                else
                    result = new PreferenceUpdateResult() { Error = "Preference is invalid.", Preference = null };

                // If saving is successful in database then update preference in cache
                if (result.Error == null)
                {
                    lock (_calcPrefCacheLocker)
                    {
                        if (_calculatedPreferencesCache.ContainsKey(preferenceId))
                            _calculatedPreferencesCache.Remove(preferenceId);
                        _calculatedPreferencesCache.Add(result.Preference.OperationPreferenceId, result.Preference);
                    }
                }
                else
                {
                    result.Error += " Could not rename preference";
                }
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Update the AllocationOperationPreference for given company. This method actually removes the old preference and replace with new one.
        /// <para>This also updates in database</para>
        /// </summary>
        /// <param name="preference">Preference to be updated</param>
        /// <returns>
        /// PreferenceUpdateResult, not successful if contains error
        /// </returns>
        internal PreferenceUpdateResult UpdatePreference(AllocationOperationPreference preference)
        {
            try
            {
                PreferenceUpdateResult result;
                bool isValid = false;
                // Check preference for validity in cache
                // AllocationOperationPreference pref = GetPreferenceByCompanyIdAndName(preference.OperationPreferenceName, preference.CompanyId);

                if (GetAllPreferenceId().Contains(preference.OperationPreferenceId) && preference.IsValid())
                    isValid = true;

                if (isValid && !String.IsNullOrWhiteSpace(preference.OperationPreferenceName))
                    result = DataAccess.AllocationPrefDataManager.UpdatePreference(preference);
                else
                    result = new PreferenceUpdateResult() { Error = "Preference is invalid.", Preference = null };

                // If saving is successful in database then update preference in cache
                if (result.Error == null)
                {
                    lock (_calcPrefCacheLocker)
                    {
                        if (_calculatedPreferencesCache.ContainsKey(preference.OperationPreferenceId))
                            _calculatedPreferencesCache.Remove(preference.OperationPreferenceId);

                        _calculatedPreferencesCache.Add(result.Preference.OperationPreferenceId, result.Preference.Clone());
                    }
                    return result;
                }
                else
                {
                    //Exception ex = new Exception("Could not update preference", result.Error);
                    result.Error += " Could not update preference";
                    return result;// If saving is unsuccessful in database then return false
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Returns all the ids for which preferences are stored in system
        /// </summary>
        /// <returns>
        /// List&lt;System.Int32&gt;.
        /// </returns>
        internal List<int> GetAllPreferenceId()
        {
            try
            {
                List<int> result = null;
                lock (_calcPrefCacheLocker)
                {
                    result = _calculatedPreferencesCache.Keys.ToList();
                }
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Gets all in visible preferences.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, string> GetAllInVisiblePreferences()
        {
            try
            {
                Dictionary<int, string> preferencesDict = new Dictionary<int, string>();
                lock (_calcPrefCacheLocker)
                {
                    preferencesDict = _calculatedPreferencesCache.Values.Where(x => !x.IsPrefVisible).ToDictionary(y => y.OperationPreferenceId, y => y.OperationPreferenceName);
                }
                return preferencesDict;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        internal List<int> GetAllVisiblePreferenceId()
        {
            try
            {
                List<int> result = new List<int>();
                lock (_calcPrefCacheLocker)
                {
                    result = _calculatedPreferencesCache.Values.Where(x => x.IsPrefVisible).Select(y => y.OperationPreferenceId).Distinct().ToList();
                }
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Gets the calculated preference account strategy list.
        /// </summary>
        /// <param name="pref">The preference.</param>
        /// <param name="accountListInPref">The account list in preference.</param>
        /// <param name="strategyListInPref">The strategy list in preference.</param>
        private static void GetCalculatedPreferenceAccountStrategyList(AllocationOperationPreference pref, HashSet<int> accountListInPref, HashSet<int> strategyListInPref)
        {
            try
            {
                //TODO: After creating new combined class for Allocation Rule + Target Percentage, remove similar code
                switch (pref.DefaultRule.RuleType)
                {
                    case MatchingRuleType.None:
                    case MatchingRuleType.SinceInception:
                    case MatchingRuleType.SinceLastChange:
                        foreach (int accountId in pref.TargetPercentage.Keys)
                        {
                            accountListInPref.Add(accountId);
                            foreach (var strategy in pref.TargetPercentage[accountId].StrategyValueList)
                            {
                                strategyListInPref.Add(strategy.StrategyId);
                            }
                        }
                        break;

                    case MatchingRuleType.Prorata:
                    case MatchingRuleType.ProrataByNAV:
                    case MatchingRuleType.Leveling:
                        if (pref.DefaultRule.ProrataAccountList != null && pref.DefaultRule.ProrataAccountList.Count > 0)
                            accountListInPref.UnionWith(pref.DefaultRule.ProrataAccountList);
                        break;
                }

                if (pref.DefaultRule.PreferenceAccountId != -1)
                    accountListInPref.Add(pref.DefaultRule.PreferenceAccountId);

                foreach (CheckListWisePreference checkListPref in pref.CheckListWisePreference.Values)
                {
                    switch (checkListPref.Rule.RuleType)
                    {
                        case MatchingRuleType.None:
                        case MatchingRuleType.SinceInception:
                        case MatchingRuleType.SinceLastChange:
                            foreach (int accountId in checkListPref.TargetPercentage.Keys)
                            {
                                accountListInPref.Add(accountId);
                                foreach (var strategy in checkListPref.TargetPercentage[accountId].StrategyValueList)
                                {
                                    strategyListInPref.Add(strategy.StrategyId);
                                }
                            }
                            break;

                        case MatchingRuleType.Prorata:
                        case MatchingRuleType.ProrataByNAV:
                        case MatchingRuleType.Leveling:
                            if (checkListPref.Rule.ProrataAccountList != null && checkListPref.Rule.ProrataAccountList.Count > 0)
                                accountListInPref.UnionWith(checkListPref.Rule.ProrataAccountList);
                            break;
                    }

                    if (checkListPref.Rule.PreferenceAccountId != -1)
                        accountListInPref.Add(checkListPref.Rule.PreferenceAccountId);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Returns all preferences for given companyId
        /// </summary>
        /// <param name="companyId">Id of the company for which data is required</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="companyWisePref">The company wise preference.</param>
        /// <returns>
        /// List of AllocationOperationPreference for given parameters
        /// </returns>
        internal List<AllocationOperationPreference> GetCalculatedPreferencesByCompanyId(int companyId, int userId, AllocationCompanyWisePref companyWisePref)
        {
            try
            {
                List<AllocationOperationPreference> result = GetPreferencesByCompanyId(userId);
                if (companyWisePref.EnableMasterFundAllocation && !companyWisePref.IsOneSymbolOneMasterFundAllocation)
                    return result.Where(pref => pref.OperationPreferenceName.StartsWith(AllocationStringConstants.MF_CALCULATED_PREF_PREFIX)).ToList();
                else
                    return result.Where(pref => pref.IsPrefVisible).ToList();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Gets the calculated preference name by identifier.
        /// </summary>
        /// <param name="prefId">The preference identifier.</param>
        /// <returns></returns>
        internal string GetCalculatedPrefNameById(int prefId)
        {
            string prefName = string.Empty; try
            {
                lock (_calcPrefCacheLocker)
                {
                    if (_calculatedPreferencesCache.ContainsKey(prefId))
                        prefName = _calculatedPreferencesCache[prefId].OperationPreferenceName;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return prefName;
        }

        /// <summary>
        /// Gets the calculated preference Id by Name.
        /// </summary>
        /// <param name="prefName">The preference Name.</param>
        /// <returns></returns>
        internal int GetCalculatedPrefIdByName(string prefName)
        {
            int prefId = 0;
            try
            {
                lock (_calcPrefCacheLocker)
                {
                    return _calculatedPreferencesCache.FirstOrDefault(x => x.Value.OperationPreferenceName == prefName).Value == null ? 0 : _calculatedPreferencesCache.FirstOrDefault(x => x.Value.OperationPreferenceName == prefName).Key;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return prefId;
        }

        /// <summary>
        /// Gets the mf preference account strategy list.
        /// </summary>
        /// <param name="mfPref">The mf preference.</param>
        /// <param name="accountListInPref">The account list in preference.</param>
        /// <param name="strategyListInPref">The strategy list in preference.</param>
        internal void GetMFPreferenceAccountStrategyList(AllocationMasterFundPreference mfPref, ref HashSet<int> accountListInPref, ref HashSet<int> strategyListInPref)
        {
            try
            {
                //update account and startegy list
                lock (_calcPrefCacheLocker)
                {
                    foreach (int calcPrefId in mfPref.MasterFundPreference.Values)
                    {
                        AllocationOperationPreference pref = _calculatedPreferencesCache[calcPrefId];
                        GetCalculatedPreferenceAccountStrategyList(pref, accountListInPref, strategyListInPref);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Returns the preference object for given companyId and name
        /// </summary>
        /// <param name="name">Name of the preference</param>
        /// <param name="companyId">companyId for which preference is required</param>
        /// <returns>
        /// AllocationOperationPreference for given parameters
        /// </returns>
        private AllocationOperationPreference GetPreferenceByCompanyIdAndName(string name, int companyId)
        {
            try
            {
                AllocationOperationPreference result = null;
                lock (_calcPrefCacheLocker)
                {
                    foreach (int prefId in _calculatedPreferencesCache.Keys)
                    {
                        if (_calculatedPreferencesCache[prefId].CompanyId == companyId && _calculatedPreferencesCache[prefId].OperationPreferenceName.ToUpper() == name.ToUpper())
                        {
                            result = _calculatedPreferencesCache[prefId].Clone();
                            break;
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Returns the Preference for given company (Cloned instance)
        /// </summary>
        /// <param name="preferenceId">Id of the preference for which data is required</param>
        /// <returns>
        /// AllocationOperationPreference object for the company, null if not exists
        /// </returns>
        internal AllocationOperationPreference GetPreferenceById(int preferenceId)
        {
            try
            {
                AllocationOperationPreference result = null;
                lock (_calcPrefCacheLocker)
                {
                    if (_calculatedPreferencesCache.ContainsKey(preferenceId))
                        result = _calculatedPreferencesCache[preferenceId].Clone();
                }
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Gets the preferences by company identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        internal List<AllocationOperationPreference> GetPreferencesByCompanyId(int userId)
        {
            try
            {
                List<AllocationOperationPreference> result = new List<AllocationOperationPreference>();
                AccountCollection accounts = WindsorContainerManager.GetAccounts(userId);
                List<int> accountList = accounts.Cast<Account>().Select(x => x.AccountID).ToList();
                StrategyCollection strategies = WindsorContainerManager.GetStrategies(userId);
                List<int> strategyList = strategies.Cast<Strategy>().Select(x => x.StrategyID).ToList();

                lock (_calcPrefCacheLocker)
                {
                    foreach (int prefId in _calculatedPreferencesCache.Keys)
                    {
                        AllocationOperationPreference pref = _calculatedPreferencesCache[prefId];
                        HashSet<int> accountListInPref = new HashSet<int>();
                        HashSet<int> strategyListInPref = new HashSet<int>();

                        GetCalculatedPreferenceAccountStrategyList(pref, accountListInPref, strategyListInPref);
                        if (accountListInPref.Count.Equals(accountListInPref.Intersect(accountList).Count()) && strategyListInPref.Count.Equals(strategyListInPref.Intersect(strategyList).Count()))
                        {
                            result.Add(pref);
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Saves the mf calculated preferences.
        /// </summary>
        /// <param name="mfPreference">The mf preference.</param>
        /// <param name="mfCalculatedPrefs">The mf calculated prefs.</param>
        /// <returns></returns>
        internal PreferenceUpdateResult SaveMFCalculatedPreferences(AllocationMasterFundPreference mfPreference, List<AllocationOperationPreference> mfCalculatedPrefs)
        {
            PreferenceUpdateResult result = new PreferenceUpdateResult();
            try
            {
                Parallel.ForEach(mfCalculatedPrefs, (calPref, state) =>
                    {
                        lock (_calcPrefCacheLocker)
                        {
                            if (!_calculatedPreferencesCache.ContainsKey(calPref.OperationPreferenceId))
                            {
                                result = DataAccess.AllocationPrefDataManager.AddPreference(calPref.CompanyId, calPref.OperationPreferenceName, AllocationPreferencesType.CalculatedAllocationPreference, false);
                                if (!string.IsNullOrWhiteSpace(result.Error))
                                    state.Break();
                                else
                                {
                                    KeyValuePair<int, int> mfAndCalPrefId = mfPreference.MasterFundPreference.FirstOrDefault(x => x.Value == calPref.OperationPreferenceId);
                                    if (!mfAndCalPrefId.Equals(new KeyValuePair<int, int>()))
                                        mfPreference.AddUpdateMasterFundPreference(mfAndCalPrefId.Key, result.Preference.OperationPreferenceId);
                                    calPref.OperationPreferenceId = result.Preference.OperationPreferenceId;
                                }
                            }
                        }
                    });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// Updates the calculated preference cache.
        /// </summary>
        /// <param name="calcPrefs">The calculate prefs.</param>
        internal void UpdateCalculatedPrefCache(List<AllocationOperationPreference> calcPrefs)
        {
            try
            {
                if (calcPrefs != null)
                {
                    calcPrefs.ForEach(calPref =>
                        {
                            lock (_calcPrefCacheLocker)
                            {
                                if (_calculatedPreferencesCache.ContainsKey(calPref.OperationPreferenceId))
                                    _calculatedPreferencesCache[calPref.OperationPreferenceId] = calPref.Clone();
                                else
                                    _calculatedPreferencesCache.Add(calPref.OperationPreferenceId, calPref.Clone());
                            }
                        });
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the mf calculated preferences.
        /// </summary>
        /// <param name="mfCalculatedPref">The mf calculated preference.</param>
        internal void UpdateMFCalculatedPreferences(List<int> mfCalculatedPref)
        {
            try
            {
                lock (_calcPrefCacheLocker)
                {
                    foreach (int calPrefId in mfCalculatedPref)
                    {
                        if (_calculatedPreferencesCache.ContainsKey(calPrefId))
                            _calculatedPreferencesCache.Remove(calPrefId);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Methods
    }
}
