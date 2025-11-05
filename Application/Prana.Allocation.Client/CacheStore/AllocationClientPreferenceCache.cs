using Prana.BusinessObjects.Classes.Allocation;
using Prana.ClientCommon;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Prana.Allocation.Client.CacheStore
{
    class AllocationClientPreferenceCache : IDisposable
    {
        #region Members

        /// <summary>
        /// The _singelton instance
        /// </summary>
        private static AllocationClientPreferenceCache _singeltonInstance = new AllocationClientPreferenceCache();

        /// <summary>
        /// The _locker
        /// </summary>
        private static readonly object _locker = new object();

        /// <summary>
        /// The _allocation operation preference cache
        /// </summary>
        private Dictionary<int, AllocationOperationPreference> _allocationOperationPreferenceCache;

        /// <summary>
        /// The _allocation mf preference cache
        /// </summary>
        private Dictionary<int, AllocationMasterFundPreference> _allocationMFPreferenceCache;

        /// <summary>
        /// The _operation preference cache lock
        /// </summary>
        private ReaderWriterLockSlim _operationPreferenceCacheLock = new ReaderWriterLockSlim();

        /// <summary>
        /// The _allocation fixed preferences cache
        /// </summary>
        private Dictionary<int, string> _allocationFixedPreferencesCache;

        /// <summary>
        /// The _allocation fixed preference cache lock
        /// </summary>
        private ReaderWriterLockSlim _allocationFixedPreferenceCacheLock = new ReaderWriterLockSlim();

        /// <summary>
        /// The _allocation master fund preference cache lock
        /// </summary>
        private ReaderWriterLockSlim _allocationMFPreferenceCacheLock = new ReaderWriterLockSlim();

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets the get instance.
        /// </summary><value>The get instance</value>
        internal static AllocationClientPreferenceCache GetInstance
        {
            get
            {
                if (_singeltonInstance == null)
                {
                    lock (_locker)
                    {
                        if (_singeltonInstance == null)
                            _singeltonInstance = new AllocationClientPreferenceCache();
                    }
                }
                return _singeltonInstance;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="AllocationClientPreferenceCache"/> class from being created.
        /// </summary>
        private AllocationClientPreferenceCache()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the allocation preference cache.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, AllocationOperationPreference> GetAllocationPreferenceCache()
        {
            Dictionary<int, AllocationOperationPreference> allocationOperationPreferences = new Dictionary<int, AllocationOperationPreference>();
            _operationPreferenceCacheLock.EnterReadLock();
            try
            {
                allocationOperationPreferences = _allocationOperationPreferenceCache;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _operationPreferenceCacheLock.ExitReadLock();
            }
            return allocationOperationPreferences;
        }

        /// <summary>
        /// Gets the fixed preferences list.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, string> GetFixedPreferencesList()
        {
            Dictionary<int, string> fixedPreferenceList = new Dictionary<int, string>();
            try
            {
                _allocationFixedPreferenceCacheLock.EnterReadLock();
                try
                {
                    fixedPreferenceList = new Dictionary<int, string>(_allocationFixedPreferencesCache);
                }
                finally
                {
                    _allocationFixedPreferenceCacheLock.ExitReadLock();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return fixedPreferenceList;
        }

        /// <summary>
        /// Gets the preferences list.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, string> GetPreferencesList()
        {
            Dictionary<int, string> preferenceList = new Dictionary<int, string>();
            try
            {
                _operationPreferenceCacheLock.EnterReadLock();
                try
                {
                    foreach (int key in _allocationOperationPreferenceCache.Keys)
                    {
                        if (!preferenceList.ContainsKey(key))
                        {
                            preferenceList.Add(key, _allocationOperationPreferenceCache[key].OperationPreferenceName);
                        }

                    }
                }
                finally
                {
                    _operationPreferenceCacheLock.ExitReadLock();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return preferenceList;
        }

        /// <summary>
        /// Gets the preferences list.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, string> GetFilteredPreferencesList()
        {
            Dictionary<int, string> preferenceList = new Dictionary<int, string>();
            try
            {
                _operationPreferenceCacheLock.EnterReadLock();
                try
                {
                    foreach (int key in _allocationOperationPreferenceCache.Keys)
                    {
                        if (_allocationOperationPreferenceCache[key].DefaultRule.RuleType == MatchingRuleType.Leveling &&
                           !AllocationSubModulePermission.IsLevelingPermitted)
                            continue;
                        else if (_allocationOperationPreferenceCache[key].DefaultRule.RuleType == MatchingRuleType.ProrataByNAV &&
                           !AllocationSubModulePermission.IsProrataByNavPermitted)
                            continue;

                        if (!preferenceList.ContainsKey(key))
                        {
                            preferenceList.Add(key, _allocationOperationPreferenceCache[key].OperationPreferenceName);
                        }

                    }
                    preferenceList = AllocationClientPreferenceManager.GetInstance.UpdateSorting(preferenceList);
                }
                finally
                {
                    _operationPreferenceCacheLock.ExitReadLock();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return preferenceList;
        }

        /// <summary>
        /// Initializes the specified allocation operation preference list.
        /// </summary>
        /// <param name="allocationOperationPrefList">The allocation operation preference list.</param>
        /// <param name="schemeList">The data table.</param>
        internal void Initialize(List<AllocationOperationPreference> allocationOperationPrefList, Dictionary<int, string> schemeList, List<AllocationMasterFundPreference> masterFundPrefList)
        {
            try
            {
                if (_operationPreferenceCacheLock == null)
                    _operationPreferenceCacheLock = new ReaderWriterLockSlim();

                if (_allocationFixedPreferenceCacheLock == null)
                    _allocationFixedPreferenceCacheLock = new ReaderWriterLockSlim();

                if (_allocationMFPreferenceCacheLock == null)
                    _allocationMFPreferenceCacheLock = new ReaderWriterLockSlim();

                //Update Calculated preference cache
                UpdateOperationPrefCache(allocationOperationPrefList);

                //Update Master Fund preference Cache
                UpdateMasterFundPrefCache(masterFundPrefList);

                //Update Fixed preference cache
                _operationPreferenceCacheLock.EnterWriteLock();

                try
                {
                    _allocationFixedPreferencesCache = schemeList;
                }
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                        throw;
                }
                finally
                {
                    _operationPreferenceCacheLock.ExitWriteLock();
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
        /// Removes the fixed preference from cache.
        /// </summary>
        /// <param name="schemeName">Name of the scheme.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        internal void RemoveFixedPreferenceFromCache(string schemeName)
        {
            _allocationFixedPreferenceCacheLock.EnterWriteLock();
            try
            {
                if (_allocationFixedPreferencesCache != null && _allocationFixedPreferencesCache.Count > 0 && _allocationFixedPreferencesCache.ContainsValue(schemeName))
                {
                    int myKey = _allocationFixedPreferencesCache.FirstOrDefault(x => x.Value.Equals(schemeName)).Key;
                    _allocationFixedPreferencesCache.Remove(myKey);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _allocationFixedPreferenceCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Updates the fixed preferences cache.
        /// </summary>
        /// <param name="fixedPreferences">The fixed preferences.</param>
        internal void UpdateFixedPreferencesCache(Dictionary<int, string> fixedPreferences)
        {
            _allocationFixedPreferenceCacheLock.EnterWriteLock();
            try
            {
                _allocationFixedPreferencesCache = fixedPreferences;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _allocationFixedPreferenceCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Updates the operation preference cache.
        /// </summary>
        /// <param name="list">The list.</param>
        internal void UpdateOperationPrefCache(List<AllocationOperationPreference> allocationOperationPref)
        {
            _operationPreferenceCacheLock.EnterWriteLock();
            try
            {
                if (_allocationOperationPreferenceCache == null)
                    _allocationOperationPreferenceCache = new Dictionary<int, AllocationOperationPreference>();
                else
                    _allocationOperationPreferenceCache.Clear();

                foreach (AllocationOperationPreference pref in allocationOperationPref)
                {
                    if (!pref.OperationPreferenceName.StartsWith("*Custom#_") && !pref.OperationPreferenceName.StartsWith("*WorkArea#_") && !pref.OperationPreferenceName.StartsWith("*PTT#_"))
                    {

                        if (_allocationOperationPreferenceCache.ContainsKey(pref.OperationPreferenceId))
                            _allocationOperationPreferenceCache[pref.OperationPreferenceId] = pref;
                        else
                            _allocationOperationPreferenceCache.Add(pref.OperationPreferenceId, pref);
                    }

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _operationPreferenceCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Ups the date preference cache.
        /// </summary>
        /// <param name="preferenceUpdateResult">The preference update result.</param>
        internal void UpDatePrefCache(AllocationOperationPreference calculatedPreference, int prefKey)
        {
            _operationPreferenceCacheLock.EnterWriteLock();
            try
            {
                if (calculatedPreference != null)
                {
                    if (_allocationOperationPreferenceCache.ContainsKey(calculatedPreference.OperationPreferenceId))
                        _allocationOperationPreferenceCache[calculatedPreference.OperationPreferenceId] = calculatedPreference;
                    else
                        _allocationOperationPreferenceCache.Add(calculatedPreference.OperationPreferenceId, calculatedPreference);
                }
                else
                {
                    _allocationOperationPreferenceCache.Remove(prefKey);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _operationPreferenceCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets the master fund preference list.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, string> GetMasterFundPreferenceList()
        {
            Dictionary<int, string> masterFundPreferenceList = new Dictionary<int, string>();
            _allocationMFPreferenceCacheLock.EnterReadLock();
            try
            {
                try
                {
                    foreach (int key in _allocationMFPreferenceCache.Keys)
                    {
                        if (!masterFundPreferenceList.ContainsKey(key))
                            masterFundPreferenceList.Add(key, _allocationMFPreferenceCache[key].MasterFundPreferenceName);
                    }
                }
                finally
                {
                    _allocationMFPreferenceCacheLock.ExitReadLock();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return masterFundPreferenceList;
        }

        /// <summary>
        /// Updates the master fund preference cache.
        /// </summary>
        /// <param name="allocationMFPreferences">The allocation mf preferences.</param>
        internal void UpdateMasterFundPrefCache(List<AllocationMasterFundPreference> allocationMFPreferences)
        {
            _allocationMFPreferenceCacheLock.EnterWriteLock();
            try
            {
                if (_allocationMFPreferenceCache == null)
                    _allocationMFPreferenceCache = new Dictionary<int, AllocationMasterFundPreference>();
                else
                    _allocationMFPreferenceCache.Clear();

                foreach (AllocationMasterFundPreference mfPref in allocationMFPreferences)
                {
                    if (_allocationMFPreferenceCache.ContainsKey(mfPref.MasterFundPreferenceId))
                        _allocationMFPreferenceCache[mfPref.MasterFundPreferenceId] = mfPref;
                    else
                        _allocationMFPreferenceCache.Add(mfPref.MasterFundPreferenceId, mfPref);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _allocationMFPreferenceCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets the allocation mf preference by preference identifier.
        /// </summary>
        /// <param name="prefId">The preference identifier.</param>
        /// <returns></returns>
        internal AllocationMasterFundPreference GetAllocationMFPreferenceByPrefId(int prefId)
        {
            _allocationMFPreferenceCacheLock.EnterReadLock();
            try
            {
                if (_allocationMFPreferenceCache.ContainsKey(prefId))
                    return _allocationMFPreferenceCache[prefId];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _allocationMFPreferenceCacheLock.ExitReadLock();
            }
            return null;
        }

        /// <summary>
        /// Adds the or update master fund preference cache.
        /// </summary>
        /// <param name="preferenceUpdateResult">The preference update result.</param>
        /// <param name="prefKey">The preference key.</param>
        internal void AddOrUpdateMasterFundPrefCache(PreferenceUpdateResult preferenceUpdateResult, int prefKey)
        {
            _allocationMFPreferenceCacheLock.EnterWriteLock();
            try
            {
                if (preferenceUpdateResult.MasterFundPreference != null)
                {
                    if (_allocationMFPreferenceCache.ContainsKey(preferenceUpdateResult.MasterFundPreference.MasterFundPreferenceId))
                        _allocationMFPreferenceCache[preferenceUpdateResult.MasterFundPreference.MasterFundPreferenceId] = preferenceUpdateResult.MasterFundPreference;
                    else
                        _allocationMFPreferenceCache.Add(preferenceUpdateResult.MasterFundPreference.MasterFundPreferenceId, preferenceUpdateResult.MasterFundPreference);
                }
                else
                {
                    _allocationMFPreferenceCache.Remove(prefKey);
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _allocationMFPreferenceCacheLock.ExitWriteLock();
            }
        }

        #endregion Methods

        #region IDisposable Members

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="p"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_allocationOperationPreferenceCache != null)
                        _allocationOperationPreferenceCache = null;
                    if (_allocationFixedPreferencesCache != null)
                        _allocationFixedPreferencesCache = null;
                    if (_allocationMFPreferenceCache != null)
                        _allocationMFPreferenceCache = null;

                    if (_allocationFixedPreferenceCacheLock != null)
                    {
                        _allocationFixedPreferenceCacheLock.Dispose();
                        _allocationFixedPreferenceCacheLock = null;
                    }
                    if (_operationPreferenceCacheLock != null)
                    {
                        _operationPreferenceCacheLock.Dispose();
                        _operationPreferenceCacheLock = null;
                    }
                    if (_allocationMFPreferenceCacheLock != null)
                    {
                        _allocationMFPreferenceCacheLock.Dispose();
                        _allocationMFPreferenceCacheLock = null;
                    }
                    if (_singeltonInstance != null)
                        _singeltonInstance = null;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion
    }
}
