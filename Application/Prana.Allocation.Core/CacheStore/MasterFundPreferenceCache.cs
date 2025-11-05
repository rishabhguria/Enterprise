using Prana.Allocation.Core.DataAccess;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.Allocation.Core.CacheStore
{
    internal sealed class MasterFundPreferenceCache
    {
        #region Members

        /// <summary>
        /// The singelton instance
        /// </summary>
        private static MasterFundPreferenceCache _singeltonInstance = new MasterFundPreferenceCache();

        /// <summary>
        /// The locker
        /// </summary>
        private static readonly object _locker = new object();

        /// <summary>
        /// The mf cache locker object
        /// </summary>
        private readonly object _mfPrefCacheLocker = new object();

        /// <summary>
        /// The mf ratio cache locker
        /// </summary>
        private readonly object _mfRatioCacheLocker = new object();

        /// <summary>
        /// The company wise master fund preference cache
        /// </summary>
        private SerializableDictionary<int, AllocationMasterFundPreference> _masterFundPreferencesCache;

        /// <summary>
        /// Master funds
        /// </summary>
        private DataSet _mfRatioCache;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        internal static MasterFundPreferenceCache Instance
        {
            get
            {
                if (_singeltonInstance == null)
                {
                    lock (_locker)
                    {
                        if (_singeltonInstance == null)
                            _singeltonInstance = new MasterFundPreferenceCache();
                    }
                }
                return _singeltonInstance;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="MasterFundPreferenceCache" /> class from being created.
        /// </summary>
        private MasterFundPreferenceCache()
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
                lock (_mfRatioCacheLocker)
                {
                    _mfRatioCache = AllocationPrefDataManager.GetAllMasterFunds();
                }
                lock (_mfPrefCacheLocker)
                {
                    _masterFundPreferencesCache = AllocationPrefDataManager.GetAllocationMasterFundPreferences();
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
        /// Adds the preference.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="isPrefVisible">if set to <c>true</c> [is preference visible].</param>
        /// <returns></returns>
        internal PreferenceUpdateResult AddPreference(string name, int companyId, bool isPrefVisible)
        {
            try
            {
                PreferenceUpdateResult result = null;
                bool isValid = false;

                // Check Masterfund preference for validity in cache
                AllocationMasterFundPreference mfPref = GetMasterFundPrefByComIdAndName(name, companyId);
                if (mfPref == null && !String.IsNullOrWhiteSpace(name))
                    isValid = true;

                if (isValid)
                    result = AllocationPrefDataManager.AddPreference(companyId, name, AllocationPreferencesType.MasterFundAllocationPreference, isPrefVisible);
                else
                    result = new PreferenceUpdateResult() { Error = "Master Fund Preference is invalid.", Preference = null };

                // If saving is successful in database then update preference in cache
                if (result.Error == null)
                {
                    lock (_mfPrefCacheLocker)
                    {
                        if (_masterFundPreferencesCache.ContainsKey(result.MasterFundPreference.MasterFundPreferenceId))
                            _masterFundPreferencesCache[result.MasterFundPreference.MasterFundPreferenceId] = result.MasterFundPreference.Clone();
                        else
                            _masterFundPreferencesCache.Add(result.MasterFundPreference.MasterFundPreferenceId, result.MasterFundPreference.Clone());
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
        /// Deletes the preference.
        /// </summary>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <param name="mfCalculatedPref">The mf calculated preference.</param>
        /// <returns></returns>
        internal PreferenceUpdateResult DeletePreference(int preferenceId, ref List<int> mfCalculatedPref)
        {
            try
            {
                PreferenceUpdateResult result = null;

                // Check preference for validity in cache
                AllocationMasterFundPreference mfPreference = GetMasterFundPreferenceById(preferenceId);

                if (mfPreference != null)
                {
                    result = AllocationPrefDataManager.DeleteMasterFundPreference(preferenceId);
                    // If saving is successful in database then update preference in cache
                    if (result.Error == null)
                    {
                        mfCalculatedPref = mfPreference.MasterFundPreference.Values.ToList();
                        lock (_mfPrefCacheLocker)
                        {
                            if (_masterFundPreferencesCache.ContainsKey(preferenceId))
                                _masterFundPreferencesCache.Remove(preferenceId);
                        }
                    }
                    else
                    {
                        result.Error += " Could not delete preference";
                    }
                }
                else
                    result = new PreferenceUpdateResult() { Error = "Preference does not exist. Could not delete preference", MasterFundPreference = null };

                

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
        /// Renames the preference.
        /// </summary>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        internal PreferenceUpdateResult RenamePreference(int preferenceId, string name)
        {
            try
            {
                PreferenceUpdateResult result = null;
                bool isValid = false;

                AllocationMasterFundPreference allocationMasterFundPref = GetMasterFundPreferenceById(preferenceId);

                if (allocationMasterFundPref != null)
                {
                    int mfPreferenceCompanyId = allocationMasterFundPref.CompanyId;

                    // Check preference for validity in cache
                    AllocationMasterFundPreference allocationMFPRef = GetMasterFundPrefByComIdAndName(name, mfPreferenceCompanyId);
                    if (allocationMFPRef == null && !String.IsNullOrWhiteSpace(name))
                        isValid = true;

                    if (isValid)
                        result = AllocationPrefDataManager.RenameMasterFundPreference(preferenceId, name);
                    else
                        result = new PreferenceUpdateResult() { Error = "Preference is invalid.", MasterFundPreference = null };

                    // If saving is successful in database then update preference in cache
                    if (result.Error == null)
                    {
                        lock (_mfPrefCacheLocker)
                        {
                            if (_masterFundPreferencesCache.ContainsKey(result.MasterFundPreference.MasterFundPreferenceId))
                                _masterFundPreferencesCache[result.MasterFundPreference.MasterFundPreferenceId] = result.MasterFundPreference.Clone();
                            else
                                _masterFundPreferencesCache.Add(result.MasterFundPreference.MasterFundPreferenceId, result.MasterFundPreference.Clone());
                        }
                    }
                    else
                    {
                        result.Error += " Could not rename preference";
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
                lock (_locker)
                {
                    result = _masterFundPreferencesCache.Keys.ToList();
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
        /// Gets the name of the master fund preference by company identifier and.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <returns></returns>
        private AllocationMasterFundPreference GetMasterFundPrefByComIdAndName(string name, int companyId)
        {
            try
            {
                AllocationMasterFundPreference result = null;

                lock (_mfPrefCacheLocker)
                {
                    foreach (int prefId in _masterFundPreferencesCache.Keys)
                    {
                        if (_masterFundPreferencesCache[prefId].CompanyId == companyId && _masterFundPreferencesCache[prefId].MasterFundPreferenceName.ToUpper() == name.ToUpper())
                        {
                            result = _masterFundPreferencesCache[prefId].Clone();
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
        /// Gets the master fund preference by company identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        internal List<AllocationMasterFundPreference> GetMasterFundPrefByCompanyId(int userId)
        {
            List<AllocationMasterFundPreference> result = new List<AllocationMasterFundPreference>();
            try
            {
                lock (_mfPrefCacheLocker)
                {
                    foreach (int mfPrefId in _masterFundPreferencesCache.Keys)
                    {
                        AllocationMasterFundPreference mfPref = _masterFundPreferencesCache[mfPrefId].Clone();


                        result.Add(mfPref);
                    }
                }
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
        /// Gets the master fund preference by identifier.
        /// </summary>
        /// <param name="mfPreferenceId">The mf preference identifier.</param>
        /// <returns></returns>
        internal AllocationMasterFundPreference GetMasterFundPreferenceById(int mfPreferenceId)
        {
            AllocationMasterFundPreference result = null;
            try
            {
                lock (_mfPrefCacheLocker)
                {
                    if (_masterFundPreferencesCache.ContainsKey(mfPreferenceId))
                        result = _masterFundPreferencesCache[mfPreferenceId].Clone();
                }
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
        /// Gets the mf preference master fund list.
        /// </summary>
        /// <param name="mfPref">The mf preference.</param>
        /// <returns></returns>
        internal HashSet<int> GetMFPrefMasterFundList(AllocationMasterFundPreference mfPref)
        {
            HashSet<int> masterFundListInPref = new HashSet<int>();
            try
            {
                //update masterfund list
                switch (mfPref.DefaultRule.RuleType)
                {
                    case MatchingRuleType.None:
                        foreach (int mfId in mfPref.MasterFundTargetPercentage.Keys)
                        {
                            if (mfPref.MasterFundTargetPercentage[mfId] >= 0.0M)
                                masterFundListInPref.Add(mfId);
                        }
                        break;

                    case MatchingRuleType.Prorata:
                    case MatchingRuleType.ProrataByNAV:
                    case MatchingRuleType.Leveling:
                        if (mfPref.DefaultRule.ProrataAccountList != null && mfPref.DefaultRule.ProrataAccountList.Count > 0)
                            masterFundListInPref.UnionWith(mfPref.DefaultRule.ProrataAccountList);
                        break;
                }

                if (mfPref.DefaultRule.PreferenceAccountId != -1)
                    masterFundListInPref.Add(mfPref.DefaultRule.PreferenceAccountId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return masterFundListInPref;
        }

        /// <summary>
        /// Gets the mf preference name by identifier.
        /// </summary>
        /// <param name="prefId">The preference identifier.</param>
        /// <returns></returns>
        internal string GetMFPrefNameById(int prefId)
        {
            string prefName = string.Empty; try
            {
                lock (_mfPrefCacheLocker)
                {
                    if (_masterFundPreferencesCache.ContainsKey(prefId))
                        prefName = _masterFundPreferencesCache[prefId].MasterFundPreferenceName;
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
        /// Gets the mf preference Id by Name.
        /// </summary>
        /// <param name="prefName">The preference Name.</param>
        /// <returns></returns>
        internal int GetMFPrefIdByName(string prefName)
        {
            int prefId = 0;
            try
            {
                lock (_mfPrefCacheLocker)
                {
                    return _masterFundPreferencesCache.FirstOrDefault(x => x.Value.MasterFundPreferenceName == prefName).Value == null ? 0 : _masterFundPreferencesCache.FirstOrDefault(x => x.Value.MasterFundPreferenceName == prefName).Key;
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
        /// Saves the master fund preference.
        /// </summary>
        /// <param name="mfPreference">The mf preference.</param>
        /// <param name="mfCalculatedPrefs">The mf calculated prefs.</param>
        /// <returns></returns>
        internal PreferenceUpdateResult SaveMasterFundPreference(AllocationMasterFundPreference mfPreference, List<AllocationOperationPreference> mfCalculatedPrefs)
        {
            try
            {
                PreferenceUpdateResult result = AllocationPrefDataManager.SaveMasterFundPreference(mfPreference, mfCalculatedPrefs);
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
        /// Updates the master fund preference cache.
        /// </summary>
        /// <param name="mfPref">The mf preference.</param>
        internal void UpdateMasterFundPrefCache(AllocationMasterFundPreference mfPref)
        {
            try
            {
                if (mfPref != null)
                {
                    lock (_mfPrefCacheLocker)
                    {
                        if (_masterFundPreferencesCache.ContainsKey(mfPref.MasterFundPreferenceId))
                            _masterFundPreferencesCache[mfPref.MasterFundPreferenceId] = mfPref.Clone();
                        else
                            _masterFundPreferencesCache.Add(mfPref.MasterFundPreferenceId, mfPref.Clone());
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
        /// Gets all master funds.
        /// </summary>
        /// <returns></returns>
        internal DataSet GetAllMasterFundsRatio()
        {
            DataSet ds = new DataSet();
            try
            {
                lock (_mfRatioCacheLocker)
                {
                    if (_mfRatioCache != null)
                        ds = _mfRatioCache;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return ds;
        }

        /// <summary>
        /// Saves the master fund target ratio.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        internal bool SaveMasterFundTargetRatio(DataSet ds)
        {
            bool result = false;
            try
            {
                result = AllocationPrefDataManager.SaveMasterFundTargetRatio(ds);
                if (result)
                {
                    lock (_mfRatioCacheLocker)
                    {
                        _mfRatioCache = ds;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }
        #endregion Methods
    }
}
