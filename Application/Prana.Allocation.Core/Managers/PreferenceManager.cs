using Prana.Allocation.Common.Definitions;
using Prana.Allocation.Core.CacheStore;
using Prana.Allocation.Core.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.Allocation.Core.Managers
{
    public class PreferenceManager
    {
        #region Members

        /// <summary>
        /// The _singleton
        /// </summary>
        private static PreferenceManager _singleton = new PreferenceManager();

        /// <summary>
        /// The _locker
        /// </summary>
        private static readonly object _locker = new object();

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets the get instance.
        /// </summary>
        /// <value>
        /// The get instance.
        /// </value>
        internal static PreferenceManager GetInstance
        {
            get
            {
                if (_singleton == null)
                {
                    lock (_locker)
                    {
                        if (_singleton == null)
                            _singleton = new PreferenceManager();
                    }
                }
                return _singleton;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="PreferenceManager" /> class from being created.
        /// </summary>
        private PreferenceManager()
        {
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal void Initialize()
        {
            try
            {
                GeneralPreferencesCache.Instance.Initialize();
                CalculatedPreferenceCache.Instance.Initialize();
                MasterFundPreferenceCache.Instance.Initialize();
                FixedPreferenceCache.Instance.Initialize();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Constructors

        #region Calculated Preferences Methods

        /// <summary>
        /// Returns all of the allocation operation preferences set for given company
        /// </summary>
        /// <param name="companyId">Id of the company for which preference is required</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// Collection of AllocationOperationPreference objects
        /// </returns>
        public List<AllocationOperationPreference> GetCalculatedPreferencesByCompanyId(int companyId, int userId)
        {
            try
            {
                AllocationCompanyWisePref companyWisePref = GetCompanyWisePreference(companyId);
                List<AllocationOperationPreference> allocationOperationPreferences = CalculatedPreferenceCache.Instance.GetCalculatedPreferencesByCompanyId(companyId, userId, companyWisePref);
                return allocationOperationPreferences;
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
        /// Gets the preference by identifier.
        /// </summary>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <returns>
        /// AllocationOperationPreference.
        /// </returns>
        public AllocationOperationPreference GetPreferenceById(int preferenceId)
        {
            try
            {
                return CalculatedPreferenceCache.Instance.GetPreferenceById(preferenceId);
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
        /// Gets the preference by identifier.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <returns>
        /// AllocationOperationPreference.
        /// </returns>
        public AllocationOperationPreference GetPreferenceById(int companyId, int userId, int preferenceId)
        {
            AllocationOperationPreference preference = null;
            try
            {
                List<AllocationOperationPreference> companyAllocationPreferences = CalculatedPreferenceCache.Instance.GetPreferencesByCompanyId(userId);
                if (companyAllocationPreferences != null && companyAllocationPreferences.Count > 0)
                {
                    preference = companyAllocationPreferences.FirstOrDefault(pref => pref.OperationPreferenceId == preferenceId);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return preference;
        }

        /// <summary>
        /// This method imports AllocationOperationPreference
        /// </summary>
        /// <param name="preference">Preference which to be imported</param>
        /// <returns></returns>
        public PreferenceUpdateResult ImportPreference(AllocationOperationPreference preference)
        {
            try
            {
                PreferenceUpdateResult result = CalculatedPreferenceCache.Instance.ImportPreference(preference);
                if (result != null && result.Preference != null)
                {
                    StateCacheStore.Instance.UpdatePreferenceWiseDate(result.Preference.OperationPreferenceId, result.Preference.UpdateDateTime);
                    PublishingHelper.PublishPreferenceUpdate(-1, result.Preference.OperationPreferenceId);
                }
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return new PreferenceUpdateResult { Error = "Something went wrong cannot import preference. Please contact administrator", Preference = null };
            }
        }

        /// <summary>
        /// This method imports AllocationMasterFundPreference
        /// </summary>
        /// <param name="preference">Preference which to be imported</param>
        /// <returns></returns>
        public PreferenceUpdateResult ImportMasterFundPreference(AllocationMasterFundPreference masterfundpreference, List<AllocationOperationPreference> masterfundcalculatedPref)
        {
            try
            {
                PreferenceUpdateResult result = null;
                if (masterfundpreference != null)
                {
                    result = MasterFundPreferenceCache.Instance.AddPreference(masterfundpreference.MasterFundPreferenceName, masterfundpreference.CompanyId, true);
                    if (result != null && result.MasterFundPreference != null)
                    {
                        masterfundpreference.MasterFundPreferenceId = result.MasterFundPreference.MasterFundPreferenceId;
                        masterfundpreference.MasterFundPreferenceName = result.MasterFundPreference.MasterFundPreferenceName;
                        List<AllocationOperationPreference> mfCalculatedPrefs = new List<AllocationOperationPreference>(); ;
                        List<int> MasterFundIds = new List<int>(masterfundpreference.MasterFundPreference.Keys);
                        foreach (int MFid in MasterFundIds)
                        {
                            String calcPrefName = AllocationStringConstants.MF_CALCULATED_PREF_PREFIX + result.MasterFundPreference.MasterFundPreferenceId + "_" + CachedDataManager.GetInstance.GetMasterFund(MFid);
                            AllocationOperationPreference calculatedpref = masterfundcalculatedPref.FirstOrDefault(x => x.OperationPreferenceId == masterfundpreference.MasterFundPreference[MFid]);
                            if (calculatedpref != null)
                            {
                                PreferenceUpdateResult childResult = CalculatedPreferenceCache.Instance.ImportPreference(calculatedpref);
                                if (childResult != null && childResult.Preference != null)
                                {
                                    mfCalculatedPrefs.Add(childResult.Preference);
                                    masterfundpreference.MasterFundPreference[MFid] = childResult.Preference.OperationPreferenceId;
                                }
                            }
                        }
                        result = SaveMasterFundPreference(masterfundpreference, mfCalculatedPrefs);
                    }
                }
                else
                    result = new PreferenceUpdateResult() { Error = "MasterFund preference is invalid.", Preference = null };
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return new PreferenceUpdateResult { Error = "Something went wrong cannot import preference. Please contact administrator", Preference = null };
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
        public PreferenceUpdateResult CopyPreference(int preferenceId, string name, AllocationPreferencesType allocationPrefType)
        {
            try
            {
                PreferenceUpdateResult result = null;
                switch (allocationPrefType)
                {
                    case AllocationPreferencesType.CalculatedAllocationPreference:
                        result = CalculatedPreferenceCache.Instance.CopyPreference(preferenceId, name);
                        if (result != null && result.Preference != null)
                            PublishingHelper.PublishPreferenceUpdate(-1, result.Preference.OperationPreferenceId);
                        break;
                    case AllocationPreferencesType.MasterFundAllocationPreference:
                        AllocationMasterFundPreference mfPref = MasterFundPreferenceCache.Instance.GetMasterFundPreferenceById(preferenceId);
                        if (mfPref != null)
                        {
                            result = MasterFundPreferenceCache.Instance.AddPreference(name, mfPref.CompanyId, true);
                            if (result != null && result.MasterFundPreference != null)
                            {
                                mfPref.MasterFundPreferenceId = result.MasterFundPreference.MasterFundPreferenceId;
                                mfPref.MasterFundPreferenceName = result.MasterFundPreference.MasterFundPreferenceName;
                                List<AllocationOperationPreference> mfCalculatedPrefs = new List<AllocationOperationPreference>();
                                List<int> MFIds = new List<int>(mfPref.MasterFundPreference.Keys);
                                foreach (int MFId in MFIds)
                                {
                                    String calcPrefName = AllocationStringConstants.MF_CALCULATED_PREF_PREFIX + result.MasterFundPreference.MasterFundPreferenceId + "_" + CachedDataManager.GetInstance.GetMasterFund(MFId);
                                    PreferenceUpdateResult childResult = CalculatedPreferenceCache.Instance.CopyPreference(mfPref.MasterFundPreference[MFId], calcPrefName);
                                    if (childResult != null && childResult.Preference != null)
                                    {
                                        mfCalculatedPrefs.Add(childResult.Preference);
                                        mfPref.MasterFundPreference[MFId] = childResult.Preference.OperationPreferenceId;
                                    }
                                }
                                result = SaveMasterFundPreference(mfPref, mfCalculatedPrefs);
                            }
                        }
                        else
                            result = new PreferenceUpdateResult() { Error = "MasterFund preference is invalid.", Preference = null };
                        break;
                }
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return new PreferenceUpdateResult { Preference = null, Error = "Something went wrong. Cannot copy preference. Please contact adminstrator" };
            }
        }

        #endregion Methods

        #region MasterFund Preferences Methods

        /// <summary>
        /// Gets all master funds ratio.
        /// </summary>
        /// <returns></returns>
        internal DataSet GetAllMasterFundsRatio()
        {
            DataSet ds = null;
            try
            {
                ds = MasterFundPreferenceCache.Instance.GetAllMasterFundsRatio();
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
                result = MasterFundPreferenceCache.Instance.SaveMasterFundTargetRatio(ds);
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
        /// Saves the master fund preference.
        /// </summary>
        /// <param name="mfPreference">The mf preference.</param>
        /// <param name="mfCalculatedPrefs">The mf calculated prefs.</param>
        /// <returns></returns>
        internal PreferenceUpdateResult SaveMasterFundPreference(AllocationMasterFundPreference mfPreference, List<AllocationOperationPreference> mfCalculatedPrefs)
        {
            try
            {
                PreferenceUpdateResult result;
                bool isValid = false;
                // Check preference for validity in cache
                AllocationMasterFundPreference mfPref = MasterFundPreferenceCache.Instance.GetMasterFundPreferenceById(mfPreference.MasterFundPreferenceId);

                if (mfPref != null && mfPreference.IsValid())
                    isValid = true;

                if (isValid && !String.IsNullOrWhiteSpace(mfPreference.MasterFundPreferenceName))
                {
                    result = CalculatedPreferenceCache.Instance.SaveMFCalculatedPreferences(mfPreference, mfCalculatedPrefs);

                    if (string.IsNullOrWhiteSpace(result.Error))
                        result = MasterFundPreferenceCache.Instance.SaveMasterFundPreference(mfPreference, mfCalculatedPrefs);
                }
                else
                    result = new PreferenceUpdateResult() { Error = "MasterFund preference is invalid.", Preference = null };

                // If saving is successful in database then update preference in cache
                if (result != null && string.IsNullOrWhiteSpace(result.Error))
                {
                    //Update MasterFund Preference
                    MasterFundPreferenceCache.Instance.UpdateMasterFundPrefCache(result.MasterFundPreference);

                    //Update MasterFund Calculated Preference
                    CalculatedPreferenceCache.Instance.UpdateCalculatedPrefCache(result.MasterFundCalculatedPreferences);

                    //publish master fund pref
                    if (result.MasterFundPreference != null)
                    {
                        PublishingHelper.PublishPreferenceUpdate(-1, result.MasterFundPreference.MasterFundPreferenceId);

                        //Publish masterfund calculated preference
                        result.MasterFundCalculatedPreferences.ForEach(calPref =>
                        {
                            StateCacheStore.Instance.UpdatePreferenceWiseDate(calPref.OperationPreferenceId, calPref.UpdateDateTime);
                            PublishingHelper.PublishPreferenceUpdate(-1, calPref.OperationPreferenceId);
                        });
                    }

                    return result;
                }
                else
                {
                    result.Error += " Could not update masterfund preference";
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
        /// Get MasterFund Pref By CompanyId
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        internal List<AllocationMasterFundPreference> GetMasterFundPrefByCompanyId(int companyId, int userId)
        {
            try
            {
                List<AllocationMasterFundPreference> result = new List<AllocationMasterFundPreference>();
                AccountCollection accounts = WindsorContainerManager.GetAccounts(userId);
                List<int> accountList = accounts.Cast<Account>().Select(x => x.AccountID).ToList();
                StrategyCollection strategies = WindsorContainerManager.GetStrategies(userId);
                List<int> strategyList = strategies.Cast<Strategy>().Select(x => x.StrategyID).ToList();
                Dictionary<int, string> masterFundCollection = CachedDataManager.GetInstance.GetAllMasterFunds();
                List<int> masterFundList = (masterFundCollection != null) ? masterFundCollection.Keys.ToList() : new List<int>();

                foreach (AllocationMasterFundPreference mfPref in MasterFundPreferenceCache.Instance.GetMasterFundPrefByCompanyId(userId))
                {
                    HashSet<int> masterFundListInPref = MasterFundPreferenceCache.Instance.GetMFPrefMasterFundList(mfPref);
                    HashSet<int> accountListInPref = new HashSet<int>();
                    HashSet<int> strategyListInPref = new HashSet<int>();
                    CalculatedPreferenceCache.Instance.GetMFPreferenceAccountStrategyList(mfPref, ref accountListInPref, ref strategyListInPref);
                    if (masterFundListInPref.Count.Equals(masterFundListInPref.Intersect(masterFundList).Count()) && accountListInPref.Count.Equals(accountListInPref.Intersect(accountList).Count()) && strategyListInPref.Count.Equals(strategyListInPref.Intersect(strategyList).Count()))
                    {
                        result.Add(mfPref);
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
        /// Gets the master fund preference by identifier.
        /// </summary>
        /// <param name="mfPreferenceId">The mf preference identifier.</param>
        /// <returns></returns>
        public AllocationMasterFundPreference GetMasterFundPreferenceById(int mfPreferenceId)
        {
            AllocationMasterFundPreference mfPref = new AllocationMasterFundPreference();
            try
            {
                mfPref = MasterFundPreferenceCache.Instance.GetMasterFundPreferenceById(mfPreferenceId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return mfPref;
        }

        #endregion

        #region Fixed Preferences Methods

        /// <summary>
        /// Pbebs the mapping.
        /// </summary>
        /// <param name="pbName">Name of the pb.</param>
        /// <param name="ebID">The eb identifier.</param>
        /// <returns></returns>
        internal bool PBEBMapping(string pbName, int ebID)
        {
            bool isMapped = false;
            try
            {
                isMapped = FixedPreferenceCache.Instance.PBEBMapping(pbName, ebID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isMapped;
        }

        /// <summary>
        /// Clears the cache master fund based positions.
        /// </summary>
        internal void ClearCacheMasterFundBasedPositions()
        {
            try
            {
                FixedPreferenceCache.Instance.ClearCacheMasterFundBasedPositions();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the account wise postion in cache.
        /// </summary>
        /// <param name="group">The group.</param>
        internal void UpdateAccountWisePostionInCache(BusinessObjects.AllocationGroup group)
        {
            try
            {
                FixedPreferenceCache.Instance.UpdateAccountWisePostionInCache(group);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the currency list for allocation scheme.
        /// </summary>
        /// <returns></returns>
        internal List<string> GetCurrencyListForAllocationScheme()
        {
            List<string> currencyList = new List<string>();
            try
            {
                currencyList = FixedPreferenceCache.Instance.GetCurrencyListForAllocationScheme();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return currencyList;
        }

        /// <summary>
        /// Gets all allocation scheme names.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, string> GetAllAllocationSchemeNames()
        {
            Dictionary<int, string> allocationScheme = null;
            try
            {
                allocationScheme = FixedPreferenceCache.Instance.GetAllAllocationSchemes();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationScheme;
        }

        /// <summary>
        /// Gets the allocation schemes by source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        internal Dictionary<int, string> GetAllocationSchemesBySource(FixedPreferenceCreationSource source)
        {
            Dictionary<int, string> allocationScheme = null;
            try
            {
                allocationScheme = FixedPreferenceCache.Instance.GetAllocationSchemesBySource(source);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationScheme;
        }

        /// <summary>
        /// Gets the name of the allocation scheme by.
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <returns></returns>
        internal AllocationFixedPreference GetAllocationSchemeByName(string allocationSchemeName)
        {
            AllocationFixedPreference allocationScheme = null;
            try
            {
                allocationScheme = FixedPreferenceCache.Instance.GetAllocationSchemeByName(allocationSchemeName);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationScheme;
        }

        /// <summary>
        /// Gets the allocation scheme key.
        /// </summary>
        /// <returns></returns>
        internal AllocationSchemeKey GetAllocationSchemeKey()
        {
            try
            {
                return FixedPreferenceCache.Instance.GetAllocationSchemeKey();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return AllocationSchemeKey.SymbolSide;
            }
        }

        /// <summary>
        /// Gets the allocation scheme name by identifier.
        /// </summary>
        /// <param name="allocationSchemeID">The allocation scheme identifier.</param>
        /// <returns></returns>
        internal string GetAllocationSchemeNameByID(int allocationSchemeID)
        {
            string prefName = string.Empty;
            try
            {
                prefName = FixedPreferenceCache.Instance.GetAllocationSchemeNameByID(allocationSchemeID);
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
        /// Gets the allocation scheme symbol wise.
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        internal void GetAllocationSchemeSymbolWise(string allocationSchemeName)
        {
            try
            {
                FixedPreferenceCache.Instance.GetAllocationSchemeSymbolWise(allocationSchemeName);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Saves the allocation scheme.
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <param name="allocationSchemeDate">The allocation scheme date.</param>
        /// <param name="allocationSchemeXML">The allocation scheme XML.</param>
        /// <param name="schemeID">The scheme identifier.</param>
        /// <returns></returns>
        internal int SaveAllocationScheme(AllocationFixedPreference fixedPref)
        {
            int allocationSchemeID = 0;
            try
            {
                allocationSchemeID = FixedPreferenceCache.Instance.SaveAllocationScheme(fixedPref);
                if (fixedPref.IsPrefVisible && allocationSchemeID > 0)
                {
                    // publish scheme on allocation UI
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-7811    
                    PublishingHelper.PublishAllocationSchemeUpdated(-1, allocationSchemeID);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationSchemeID;
        }

        /// <summary>
        /// Deletes the allocation scheme.
        /// </summary>
        /// <param name="allocationSchemeID">The allocation scheme identifier.</param>
        /// <param name="schemeName">Name of the scheme.</param>
        /// <returns></returns>
        internal int DeleteAllocationScheme(int allocationSchemeID, string schemeName)
        {
            int affectedRow = 0;
            try
            {
                affectedRow = FixedPreferenceCache.Instance.DeleteAllocationScheme(allocationSchemeID, schemeName);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return affectedRow;
        }

        /// <summary>
        /// Gets the symbol wise dictionary.
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <returns></returns>
        internal Dictionary<string, List<DataRow>> GetSymbolWiseDictionary(string allocationSchemeName)
        {
            Dictionary<string, List<DataRow>> symbolwiseDict = new Dictionary<string, List<DataRow>>();
            try
            {
                symbolwiseDict = FixedPreferenceCache.Instance.GetSymbolWiseDictionary(allocationSchemeName);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return symbolwiseDict;
        }

        /// <summary>
        /// Gets the positions from symbol and namewise account allocation scheme.
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <param name="key">The key.</param>
        /// <param name="lstRows">The LST rows.</param>
        /// <returns></returns>
        internal string GetPositionsFromSymbolAndNamewiseAccountAllocationScheme(string allocationSchemeName, string key, ref List<DataRow> lstRows)
        {
            string errMessage = string.Empty;
            try
            {
                errMessage = FixedPreferenceCache.Instance.GetPositionsFromSymbolAndNamewiseAccountAllocationScheme(allocationSchemeName, key, ref lstRows);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return errMessage;
        }

        #endregion

        #region General Preferences Methods

        /// <summary>
        /// Gets Default rule for company
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <returns>
        /// AllcoationRule
        /// </returns>
        public AllocationCompanyWisePref GetCompanyWisePreference(int companyId)
        {
            try
            {
                AllocationCompanyWisePref rule = GeneralPreferencesCache.Instance.GetCompanyWisePreference(companyId);
                return rule;
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
        /// Gets Auto Grouping Rules
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns>
        /// AutoGroupingRules
        /// </returns>
        public AutoGroupingRules GetAutoGroupingPreferences()
        {
            try
            {
                return GeneralPreferencesCache.Instance.AutoGroupingRulePref;
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
        /// Gets the automatic grouping funds.
        /// </summary>
        /// <returns></returns>
        public List<int> GetAutoGroupingFunds()
        {
            try
            {
                return GeneralPreferencesCache.Instance.AutoGroupingFunds;
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
        /// Saves the attribute names.
        /// </summary>
        /// <param name="ds">The ds.</param>
        internal void SaveAttributeNames(DataSet ds)
        {
            try
            {
                GeneralPreferencesCache.Instance.SaveAttributeNames(ds);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the attribute names.
        /// </summary>
        /// <returns></returns>
        internal DataSet GetAttributeNames()
        {
            try
            {
                return GeneralPreferencesCache.Instance.GetAttributeNames();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return new DataSet();
        }

        /// <summary>
        /// Saves Default rule for company.
        /// </summary>
        /// <param name="defaultPref">The default preference.</param>
        /// <returns></returns>
        public bool SaveCompanyWisePreference(AllocationCompanyWisePref defaultPref)
        {
            try
            {
                bool result = GeneralPreferencesCache.Instance.SaveCompanyWisePreference(defaultPref);
                //publish preferences on client after saving, PRANA-11962
                if (result)
                    PublishingHelper.PublishPreferenceUpdate(-1, -1);
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return false;
            }
        }

        #endregion

        #region Common Methods

        /// <summary>
        /// Gets the invisible allocation preferences.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public Dictionary<int, string> GetInvisibleAllocationPreferences()
        {
            Dictionary<int, string> preferencesDict = new Dictionary<int, string>();
            try
            {
                preferencesDict = CalculatedPreferenceCache.Instance.GetAllInVisiblePreferences();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return preferencesDict;
        }

        /// <summary>
        /// Gets the allocation preferences.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        internal Dictionary<int, string> GetAllocationPreferences(int companyId, int userId, bool isLevelingPerferenceRequired, bool isProrataByNavPerferenceRequired)
        {
            Dictionary<int, string> preferencesDict = new Dictionary<int, string>();
            try
            {
                AllocationCompanyWisePref companyWisePref = GetCompanyWisePreference(companyId);
                if (companyWisePref.EnableMasterFundAllocation && !companyWisePref.IsOneSymbolOneMasterFundAllocation)
                {
                    List<AllocationMasterFundPreference> mfPrefs = GetMasterFundPrefByCompanyId(companyId, userId);

                    foreach (AllocationMasterFundPreference preference in mfPrefs)
                    {
                        if (!isLevelingPerferenceRequired && preference.IsLevelingUsed())
                            continue;
                        if (!isProrataByNavPerferenceRequired && preference.IsProrataByNAVUsed())
                            continue;

                        preferencesDict.Add(preference.MasterFundPreferenceId, preference.MasterFundPreferenceName);
                    }
                }
                else
                {
                    List<AllocationOperationPreference> calcPrefs = CalculatedPreferenceCache.Instance.GetPreferencesByCompanyId(userId);

                    foreach (AllocationOperationPreference preference in calcPrefs)
                    {
                        if (preference.IsPrefVisible)
                        {
                            if (!isLevelingPerferenceRequired && preference.IsLevelingUsed())
                                continue;
                            if (!isProrataByNavPerferenceRequired && preference.IsProrataByNAVUsed())
                                continue;

                            preferencesDict.Add(preference.OperationPreferenceId, preference.OperationPreferenceName);
                        }
                    }
                }
                //adding fixed preferences
                Dictionary<int, string> fixedPrefs = GetAllAllocationSchemeNames();
                foreach (KeyValuePair<int, string> fp in fixedPrefs)
                {
                    preferencesDict.Add(fp.Key, fp.Value);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return preferencesDict;
        }

        /// <summary>
        /// Gets the allocation fixed and calculated preference name by identifier.
        /// </summary>
        /// <param name="prefId">The identifier.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        internal string GetAllocationPreferenceNameById(int prefId)
        {
            string prefName = "Manual";
            try
            {
                if (!string.IsNullOrWhiteSpace(GetAllocationSchemeNameByID(prefId)))
                    prefName = GetAllocationSchemeNameByID(prefId);
                else if (!string.IsNullOrWhiteSpace(CalculatedPreferenceCache.Instance.GetCalculatedPrefNameById(prefId)))
                    prefName = CalculatedPreferenceCache.Instance.GetCalculatedPrefNameById(prefId);
                else if (!string.IsNullOrWhiteSpace(MasterFundPreferenceCache.Instance.GetMFPrefNameById(prefId)))
                    prefName = MasterFundPreferenceCache.Instance.GetMFPrefNameById(prefId);
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
        /// Gets the allocation fixed and calculated preference Id by Name.
        /// </summary>
        /// <param name="prefName">The Name.</param>
        /// <returns>
        /// System.Int.
        /// </returns>
        internal int GetAllocationPreferenceIdByName(string prefName)
        {
            int prefId = 0;
            try
            {
                if (FixedPreferenceCache.Instance.GetAllocationSchemeIdByName(prefName) != 0)
                    prefId = FixedPreferenceCache.Instance.GetAllocationSchemeIdByName(prefName);
                else if (CalculatedPreferenceCache.Instance.GetCalculatedPrefIdByName(prefName) != 0)
                    prefId = CalculatedPreferenceCache.Instance.GetCalculatedPrefIdByName(prefName);
                else if (MasterFundPreferenceCache.Instance.GetMFPrefIdByName(prefName) != 0)
                    prefId = MasterFundPreferenceCache.Instance.GetMFPrefIdByName(prefName);
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
        /// This method update AllocationOperationPreference for given company
        /// </summary>
        /// <param name="preference">Preference which to be updated</param>
        /// <returns>
        /// True if updated successfully, otherwise false
        /// </returns>
        public PreferenceUpdateResult UpdatePreference(AllocationOperationPreference preference)
        {
            try
            {
                PreferenceUpdateResult result = CalculatedPreferenceCache.Instance.UpdatePreference(preference);
                if (result != null && result.Preference != null)
                {
                    StateCacheStore.Instance.UpdatePreferenceWiseDate(result.Preference.OperationPreferenceId, result.Preference.UpdateDateTime);
                    PublishingHelper.PublishPreferenceUpdate(-1, result.Preference.OperationPreferenceId);
                }
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return new PreferenceUpdateResult { Error = "Something went wrong cannot update preference. Please contact administrator", Preference = null };
            }
        }

        /// <summary>
        /// Renames preference of given preferenceId
        /// </summary>
        /// <param name="preferenceId">Id of preference which to be Renamed</param>
        /// <param name="name">New name of preference which to be Renamed</param>
        /// <param name="allocationPrefType">Type of the allocation preference.</param>
        /// <returns>
        /// PreferenceUpdate result
        /// </returns>
        public PreferenceUpdateResult RenamePreference(int preferenceId, string name, AllocationPreferencesType allocationPrefType)
        {
            try
            {
                PreferenceUpdateResult result = null;
                switch (allocationPrefType)
                {
                    case AllocationPreferencesType.CalculatedAllocationPreference:
                        result = CalculatedPreferenceCache.Instance.RenamePreference(preferenceId, name);
                        break;

                    case AllocationPreferencesType.MasterFundAllocationPreference:
                        result = MasterFundPreferenceCache.Instance.RenamePreference(preferenceId, name);
                        break;
                }
                if (result != null && result.Preference != null)
                {
                    StateCacheStore.Instance.UpdatePreferenceWiseDate(result.Preference.OperationPreferenceId, result.Preference.UpdateDateTime);
                    PublishingHelper.PublishPreferenceUpdate(-1, result.Preference.OperationPreferenceId);
                }

                //Publish Master fund Preference
                if (result != null && result.MasterFundPreference != null)
                {
                    PublishingHelper.PublishPreferenceUpdate(-1, result.MasterFundPreference.MasterFundPreferenceId);
                }
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return new PreferenceUpdateResult { Preference = null, Error = "Something went wrong. Cannot rename preference. Please contact administrator" };
            }
        }

        /// <summary>
        /// Adds the given preference information to database and cache
        /// </summary>
        /// <param name="name">Name of the preference</param>
        /// <param name="companyId">companyId of the preference</param>
        /// <param name="allocationPrefType">Type of the allocation preference.</param>
        /// <param name="isPrefVisible">if set to <c>true</c> [is preference visible].</param>
        /// <returns>
        /// Update result of the preference
        /// </returns>
        public PreferenceUpdateResult AddPreference(string name, int companyId, AllocationPreferencesType allocationPrefType, bool isPrefVisible, string rebalancerFileName = "")
        {
            try
            {
                PreferenceUpdateResult result = null;
                switch (allocationPrefType)
                {
                    case AllocationPreferencesType.CalculatedAllocationPreference:
                        result = CalculatedPreferenceCache.Instance.AddPreference(name, companyId, isPrefVisible, rebalancerFileName);
                        break;
                    case AllocationPreferencesType.MasterFundAllocationPreference:
                        result = MasterFundPreferenceCache.Instance.AddPreference(name, companyId, isPrefVisible);
                        break;
                }
                if (result != null && result.Preference != null)
                {
                    StateCacheStore.Instance.UpdatePreferenceWiseDate(result.Preference.OperationPreferenceId, result.Preference.UpdateDateTime);
                    PublishingHelper.PublishPreferenceUpdate(-1, result.Preference.OperationPreferenceId);
                }

                //Publish Master fund Preference
                if (result != null && result.MasterFundPreference != null)
                {
                    PublishingHelper.PublishPreferenceUpdate(-1, result.MasterFundPreference.MasterFundPreferenceId);
                }
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return new PreferenceUpdateResult { Preference = null, Error = "Something went wrong. Cannot add preference. Please contact adminstrator" };
            }
        }

        /// <summary>
        /// Delete preference of given preferenceId
        /// </summary>
        /// <param name="preferenceId">Id of preference which to be deleted</param>
        /// <param name="allocationPrefType">Type of the allocation preference.</param>
        /// <returns>
        /// PreferenceUpdate result
        /// </returns>
        public PreferenceUpdateResult DeletePreference(int preferenceId, AllocationPreferencesType allocationPrefType)
        {
            try
            {
                AllocationMasterFundPreference mfPref = null;
                if (allocationPrefType.Equals(AllocationPreferencesType.MasterFundAllocationPreference))
                    mfPref = MasterFundPreferenceCache.Instance.GetMasterFundPreferenceById(preferenceId).Clone();

                PreferenceUpdateResult result = null;
                switch (allocationPrefType)
                {
                    case AllocationPreferencesType.CalculatedAllocationPreference:
                        result = CalculatedPreferenceCache.Instance.DeletePreference(preferenceId);
                        break;
                    case AllocationPreferencesType.MasterFundAllocationPreference:
                        List<int> mfCalculatedPref = new List<int>();
                        result = MasterFundPreferenceCache.Instance.DeletePreference(preferenceId, ref mfCalculatedPref);
                        if (result.Error == null)
                        {
                            CalculatedPreferenceCache.Instance.UpdateMFCalculatedPreferences(mfCalculatedPref);
                        }
                        break;
                }
                if (result != null && result.Error == null)
                {
                    if (mfPref != null)
                    {
                        mfPref.MasterFundPreference.Values.ToList().ForEach(calPrefId =>
                        {
                            PublishingHelper.PublishPreferenceUpdate(-1, calPrefId);
                        });
                    }
                    PublishingHelper.PublishPreferenceUpdate(-1, preferenceId);
                }
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return new PreferenceUpdateResult { Preference = null, Error = "Something went wrong. Cannot delete preference. Please contact adminstrator" };
            }
        }

        /// <summary>
        /// This method refreshes the allocation preference cache
        /// </summary>
        internal void RefreshPreferenceCache()
        {
            try
            {
                CalculatedPreferenceCache.Instance.Initialize();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion
    }
}
