using Prana.Allocation.ClientLibrary.DataAccess;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Configuration;

namespace Prana.Allocation.Client.CacheStore
{
    /// <summary>
    /// Cache Class for Company Wise preferenece
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    [Serializable]
    sealed class AllocationCompanyWisePreferenceCache : IDisposable
    {
        #region Members

        /// <summary>
        /// The _allocation company wise preference
        /// </summary>
        private AllocationCompanyWisePref _allocationCompanyWisePref = null;

        /// <summary>
        /// The _singelton instance
        /// </summary>
        private static AllocationCompanyWisePreferenceCache _singeltonInstance = new AllocationCompanyWisePreferenceCache();

        /// <summary>
        /// The _singleton locker
        /// </summary>
        private static readonly object _singletonLocker = new object();

        /// <summary>
        /// The _cache locker
        /// </summary>
        private static readonly object _cacheLocker = new object();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the get instance.
        /// </summary>
        /// <value>
        /// The get instance.
        /// </value>
        internal static AllocationCompanyWisePreferenceCache GetInstance
        {
            get
            {
                if (_singeltonInstance == null)
                {
                    lock (_singletonLocker)
                    {
                        if (_singeltonInstance == null)
                            _singeltonInstance = new AllocationCompanyWisePreferenceCache();
                    }
                }
                return _singeltonInstance;
            }
        }

        #endregion Properties

        #region Constructors

        private AllocationCompanyWisePreferenceCache()
        { }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the company wise preferences.
        /// </summary>
        /// <returns></returns>
        internal AllocationCompanyWisePref GetCompanyWisePreferences()
        {
            try
            {
                lock (_cacheLocker)
                {
                    if (_allocationCompanyWisePref == null)
                    {
                        UpdateCompanyWisePreferences();
                    }
                    return DeepCopyHelper.Clone(_allocationCompanyWisePref);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Sets the company wise preferences.
        /// </summary>
        /// <param name="defaultPref">The default preference.</param>
        internal void SetCompanyWisePreferences(AllocationCompanyWisePref defaultPref)
        {
            try
            {
                lock (_cacheLocker)
                {
                    _allocationCompanyWisePref = defaultPref;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates the company wise preferences.
        /// </summary>
        internal void UpdateCompanyWisePreferences()
        {
            try
            {
                lock (_cacheLocker)
                {
                    _allocationCompanyWisePref = AllocationClientPreferenceDataManager.GetInstance().GetCompanyWisePreference(CachedDataManager.GetInstance.LoggedInUser.CompanyID, CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);

                    if (_allocationCompanyWisePref != null && (_allocationCompanyWisePref.SelectedProrataSchemeName == null || _allocationCompanyWisePref.SelectedProrataSchemeName.Equals(string.Empty)))
                    {
                        _allocationCompanyWisePref.SelectedProrataSchemeName = ConfigurationManager.AppSettings["GetPositionofADateAllocSchemeName"] ?? "Positions";
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

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
                    if (_singeltonInstance != null)
                        _singeltonInstance = null;
                    if (_allocationCompanyWisePref != null)
                        _allocationCompanyWisePref = null;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion IDisposable Members
    }
}
