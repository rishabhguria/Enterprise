// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 08-30-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-08-2014
// ***********************************************************************
// <copyright file="PreferenceCacheStore.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Common.Definitions;
using Prana.Allocation.Core.DataAccess;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

/// <summary>
/// The CacheStore namespace.
/// </summary>
namespace Prana.Allocation.Core.CacheStore
{
    /// <summary>
    /// Singleton class for storing Preference cache for allocation
    /// <para>This definition is cache for storing all preferences. There are several methods associated with class to perform operation</para>
    /// </summary>
    internal sealed class GeneralPreferencesCache
    {
        #region Members

        /// <summary>
        /// Singleton instance of the class
        /// </summary>
        private static GeneralPreferencesCache _singeltonInstance = new GeneralPreferencesCache();

        /// <summary>
        /// The locker
        /// </summary>
        private static readonly object _locker = new object();

        /// <summary>
        /// The company wise preference cache locker
        /// </summary>
        private readonly object _companyPrefCacheLocker = new object();

        /// <summary>
        /// Company Id wise Default Allcoation rule
        /// </summary>
        private Dictionary<int, AllocationCompanyWisePref> _companyWisePrefCache;

        /// <summary>
        /// Store the auto grouping prefs  which is fetched from the database
        /// </summary>
        private AutoGroupingRules _autoGroupingRules = new AutoGroupingRules();

        /// <summary>
        /// The automatic grouping funds
        /// </summary>
        private List<int> _autoGroupingFunds = new List<int>();

        #endregion Members

        #region Properties

        /// <summary>
        /// Instance method to return the singleton instance of the object in the memory
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        internal static GeneralPreferencesCache Instance
        {
            get
            {
                if (_singeltonInstance == null)
                {
                    lock (_locker)
                    {
                        if (_singeltonInstance == null)
                            _singeltonInstance = new GeneralPreferencesCache();
                    }
                }
                return _singeltonInstance;
            }
        }

        /// <summary>
        /// This is the property which is use to get the auto groping rules which is fetched from database
        /// </summary>
        public AutoGroupingRules AutoGroupingRulePref
        {
            get { return _autoGroupingRules; }
        }

        /// <summary>
        /// Gets or sets the automatic grouping funds.
        /// </summary>
        /// <value>
        /// The automatic grouping funds.
        /// </value>
        public List<int> AutoGroupingFunds
        {
            get { return _autoGroupingFunds; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Private constructor to restrict object creation
        /// </summary>
        private GeneralPreferencesCache()
        {
            try
            {
                // Do cache object initialization which should be done before Initialize() method
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

        /// <summary>
        /// Do the initialization of cache from database
        /// <para>This method loads data from database so it can be used to re-initialize</para><para>Calling this method multiple time will only reload data from database, still avoid calling this more than once (if re-initialization is not required)</para>
        /// </summary>
        internal void Initialize()
        {
            try
            {
                lock (_companyPrefCacheLocker)
                {
                    _companyWisePrefCache = AllocationPrefDataManager.GetAllDefaultRule();
                }
                string funds;
                _autoGroupingRules = AllocationPrefDataManager.GetAutoGroupingPreference(out funds);
                if (!string.IsNullOrWhiteSpace(funds))
                    _autoGroupingFunds = funds.Split(',').Select(int.Parse).ToList();
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
        /// Returns default allocation rule for comapny id
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <returns></returns>
        internal AllocationCompanyWisePref GetCompanyWisePreference(int companyId)
        {
            try
            {
                lock (_companyPrefCacheLocker)
                {
                    if (_companyWisePrefCache.ContainsKey(companyId))
                        return _companyWisePrefCache[companyId];
                    else
                        //Return default rule when not exists
                        return new AllocationCompanyWisePref()
                        {
                            CompanyId = companyId,
                            DefaultRule = new AllocationRule()
                            {
                                BaseType = AllocationBaseType.CumQuantity,
                                MatchClosingTransaction = MatchClosingTransactionType.None,
                                PreferenceAccountId = -1,
                                RuleType = MatchingRuleType.None
                            },
                            AllocationCheckSidePref = new AllocationCheckSidePref()
                            {
                                DoCheckSideSystem = true

                            },
                            AllowEditPreferences = false,
                            //DisableCheckSideForAssets = new List<int>(),
                            AssetsWithCommissionInNetAmount = new List<int>(),
                            PrecisionDigit = 2, //Standard format for digits is 2, so setting default precision digit is 2
                            MsgOnBrokerChange = true,
                            RecalculateOnBrokerChange = false,
                            MsgOnAllocation = true,
                            RecalculateOnAllocation = false,
                            EnableMasterFundAllocation = false,
                            IsOneSymbolOneMasterFundAllocation = false,
                            SelectedProrataSchemeName = string.Empty,
                            SelectedProrataSchemeBasis = (AllocationSchemeKey.SymbolSide).ToString(),
                            SetSchemeGenerationAttributesProrata = false

                        };
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
        /// Saves the attribute names.
        /// </summary>
        /// <param name="ds">The ds.</param>
        internal void SaveAttributeNames(DataSet ds)
        {
            try
            {
                AllocationPrefDataManager.SaveAttributeNames(ds);
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
                return AllocationPrefDataManager.GetAttributeNames();
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
        internal bool SaveCompanyWisePreference(AllocationCompanyWisePref defaultPref)
        {
            try
            {
                lock (_companyPrefCacheLocker)
                {
                    bool result = AllocationPrefDataManager.SaveCompanyWisePreference(defaultPref);
                    if (result)
                    {
                        if (_companyWisePrefCache.ContainsKey(defaultPref.CompanyId))
                        {
                            //  var allocationCheckSidePref = _companyWisePrefCache[defaultPref.CompanyId].AllocationCheckSidePref;
                            //defaultPref.AllocationCheckSidePref = allocationCheckSidePref;
                            _companyWisePrefCache[defaultPref.CompanyId] = defaultPref;
                        }
                        else
                            _companyWisePrefCache.Add(defaultPref.CompanyId, defaultPref);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return false;
            }
        }

        #endregion Methods
    }
}
