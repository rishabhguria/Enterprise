// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 09-03-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-10-2014
// ***********************************************************************
// <copyright file="AllocationGroupCache.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Core.Comparers;
using Prana.Allocation.Core.DataAccess;
using Prana.Allocation.Core.Extensions;
using Prana.Allocation.Core.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Concurrent;

/// <summary>
/// The CacheStore namespace.
/// </summary>
namespace Prana.Allocation.Core.CacheStore
{
    /// <summary>
    /// Class AllocationGroupCache.
    /// </summary>
    internal class AllocationGroupCache
    {
        /// <summary>
        /// Singleton instance of the class
        /// </summary>
        private static readonly AllocationGroupCache _singeltonInstance = new AllocationGroupCache();

        /// <summary>
        /// Instance method to return the singleton instance of the object in the memory
        /// </summary>
        /// <value>The instance.</value>
        internal static AllocationGroupCache Instance
        {
            get { return _singeltonInstance; }
        }

        /// <summary>
        /// Private constructor to restrict object creation
        /// </summary>
        private AllocationGroupCache()
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

        #region Cache Objects

        /// <summary>
        /// Locker object for cache
        /// </summary>
        private static readonly object _cacheLockerObject = new object();

        /// <summary>
        /// Map of allocation group keyed by GroupID (as string)
        /// </summary>
        private ConcurrentDictionary<string, AllocationGroup> _allocationGroupCache = new ConcurrentDictionary<string, AllocationGroup>();

        /// <summary>
        /// DateTime object which contains the start date for which allocation groups are in cache
        /// </summary>
        private DateTime _startDateInCache = DateTime.UtcNow;

        #endregion

        /// <summary>
        /// Do the initialization of cache from database
        /// <para>This method loads data from database so it can be used to re-initialize</para><para>Calling this method multiple time will only reload data from database, still avoid calling this more than once (if re-initialization is not required)</para>
        /// </summary>
        internal void Initialize()
        {
            try
            {
                lock (_cacheLockerObject)
                {
                    _allocationGroupCache = AllocationGroupDataManager.GetGroups(DateTime.UtcNow, DateTime.UtcNow.AddDays(-1), null);
                    foreach (var group in _allocationGroupCache.Values)
                    {
                        if (!group.IsManuallyModified)
                        {
                            GroupCache.GetInstance().UpdateAutoGroupsCache(group);
                        }
                        if (group.OrderCount > 1)
                        {
                            GroupCache.GetInstance().MarkGroupDirty(group);
                        }
                    };
                    _startDateInCache = DateTime.UtcNow.AddDays(-1);
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
        /// Save the given list of allocation group provided
        /// </summary>
        /// <param name="groups">List of allocation groups provided</param>
        /// <returns>true if saved successfully, otherwise false</returns>
        internal int SaveGroups(List<AllocationGroup> groups)
        {
            try
            {
                lock (_cacheLockerObject)
                {
                    foreach (var group in groups)
                    {
                        if (group.PersistenceStatus == ApplicationConstants.PersistenceStatus.UnGrouped && !_allocationGroupCache.ContainsKey(group.GroupID))
                        {
                            return -1;
                        }
                    }
                    int result = AllocationGroupDataManager.SaveGroups(groups);
                    if (result > 0)
                    {
                        UpdateGroupsCache(groups);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return -1;
            }
        }


        /// <summary>
        /// Updates the groups cache.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal bool UpdateGroupsCache(List<AllocationGroup> groups)
        {
            try
            {
                foreach (AllocationGroup group in groups)
                {
                    var key = group.GroupID.ToString();
                    if (group.PersistenceStatus == ApplicationConstants.PersistenceStatus.Deleted|| group.PersistenceStatus == ApplicationConstants.PersistenceStatus.UnGrouped)
                    {
                        _allocationGroupCache.TryRemove(key, out _);
                    }
                    else
                    {
          
                        foreach (AllocationOrder order in group.Orders)
                        {
                            order.OriginalCumQty = order.CumQty;
                        }
                          _allocationGroupCache.TryRemove(key, out _);
                        // Used deep copy to maintain state of taxlot qty in group.Taxlots after closing
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-7147
                        _allocationGroupCache[key] = DeepCopyHelper.Clone(group);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Returns groups with matching group ids
        /// </summary>
        /// <param name="predicate">list of group id</param>
        /// <returns>list of allocation group</returns>
        internal List<AllocationGroup> GetGroups(Expression<Func<AllocationGroup, bool>> predicate, bool isFetchFromDB = true)
        {
            try
            {
                lock (_cacheLockerObject)
                {
 				  // Applying filter and returning List of allocation group
                  if (_allocationGroupCache != null)
                  {
                        List<AllocationGroup> groups = _allocationGroupCache.Values.AsQueryable().Where(predicate).ToList();
                        UpdateGroupBasicDetails(groups, isFetchFromDB);
                        return groups;
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
            return null;
        }

        /// <summary>
        /// Updates the group basic details.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <param name="isFetchFromDB">if set to <c>true</c> [is fetch from database].</param>
        private static void UpdateGroupBasicDetails(List<AllocationGroup> groups, bool isFetchFromDB)
        {
            try
            {
                //Getting security master objects for symbols in allocation group list
                Dictionary<string, SecMasterBaseObj> secmasterCollection = CacheHelper.GetSecurityDetails(groups);
                Parallel.ForEach(groups, group =>
                {
                    //Update group closing status
                    CacheHelper.UpdateClosingStatus(group);
                    //Updating security master object when gettind data.
                    if (secmasterCollection.ContainsKey(group.Symbol))
                        group.CopyBasicDetails(secmasterCollection[group.Symbol]);
                    if (!isFetchFromDB)
                        group.UpdateSchemeName();
                });
                // added logging when currency ID is blank or have int.MinValue, in this case Currency will be blank, PRANA-10662
                groups.ForEach(x => { if (string.IsNullOrWhiteSpace(x.CurrencyID.ToString()) || x.CurrencyID == int.MinValue) CacheHelper.InformationLogging("Currency ID is blank for " + x.Symbol + " trade having groupID: " + x.GroupID, null); });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        internal async Task<List<AllocationGroup>> GetGroupsAsync(DateTime toDate, DateTime fromDate, AllocationPrefetchFilter filterList, int userId)
        {
            try
            {
                List<AllocationGroup> allocationGroups = await System.Threading.Tasks.Task.Run(() => AllocationGroupCache.Instance.GetGroups(toDate, fromDate, filterList, userId));
                return allocationGroups;
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
        /// Returns the list of group for given date range along with the filter list provided
        /// </summary>
        /// <param name="toDate">Date upto which data is required (including)</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="filterList">List of filter which will be applied</param>
        /// <returns>List of allocation groups</returns>
        internal List<AllocationGroup> GetGroups(DateTime toDate, DateTime fromDate, AllocationPrefetchFilter filterList, int userId)
        {
            try
            {
                Expression<Func<AllocationGroup, bool>> predicate = null;
                List<AllocationGroup> groups = new List<AllocationGroup>();
                bool isFetchFromDB = true;
                lock (_cacheLockerObject)
                {
                    //As discussed with Gaurav Sir, For Reporting Client no need to maintain Cache.
                    if (!Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("CreateCacheInAllocation")))
                    {
                        _allocationGroupCache = AllocationGroupDataManager.GetGroups(toDate, fromDate, filterList);
                        //if (groups.Count > 0)
                        //    UpdateGroupBasicDetails(groups, isFetchFromDB);                        
                        groups = _allocationGroupCache.Values.ToList(); ;
                    }
                    else
                    {
                        // This config entry is used in case there are no publishig from any module to Allocation. Recent example 
                        // includes SS-Parallel in which publishig from third party module was not developed.
                        // In this case allocation will try to fetch data from database whenever "get" is requested from client
                        bool useCacheInAllocation = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("UseCacheInAllocation"));

                        // If use cache is set to true then it uses the original flow and will fetch data from database if requested date is out of range in cache.
                        if (useCacheInAllocation)
                        {
                            // Re-fetching data from database in case groups required are from date before that are stored in cache
                            if (fromDate.Date < _startDateInCache)
                            {
                                _allocationGroupCache = AllocationGroupDataManager.GetGroups(DateTime.UtcNow > toDate ? DateTime.UtcNow : toDate, fromDate, null);
                                _startDateInCache = fromDate;
                            }
                            else
                            {
                                isFetchFromDB = false;
                            }
                        }
                        else
                        {
                            // In case Use cache is set to false then it will try to find smaller date and will use that date to fetch data from database
                            // After updating cache it will update the variable "_startDateInCache" also
                            DateTime smallerDate = fromDate.Date < _startDateInCache ? fromDate.Date : _startDateInCache;
                            _allocationGroupCache = AllocationGroupDataManager.GetGroups(DateTime.UtcNow > toDate ? DateTime.UtcNow : toDate, smallerDate, null);
                            _startDateInCache = smallerDate;
                        }

                    }
                    //https://jira.nirvanasolutions.com:8443/browse/PRANA-38881 
                    if (_allocationGroupCache != null && _allocationGroupCache.Count > 0)
                    {
                        //Modified By Faisal Shah, Was getting Data for extra date due to adding a day so added time.
                        predicate = AllocationGroupPredicates.GetPredicateFromFilter(toDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59),fromDate.Date,filterList,userId);
                    }
                }

                // Applying filter and getting updated group list
                if (predicate != null)
                {
                    groups = GetGroups(predicate, isFetchFromDB);
                }
                return groups;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        public int SaveGroupsPostTrade(string connString, XmlSaveHandler xmlSaveMgr, List<AllocationGroup> groups)
        {
            int rowsAffected = 0;
            try
            {
                lock (_cacheLockerObject)
                {
                    if (connString == string.Empty)
                    {
                        rowsAffected = xmlSaveMgr.SaveGroupXmls();
                    }
                    else
                    {
                        rowsAffected = xmlSaveMgr.SaveGroupXmls(connString);
                    }
                    // UpdateState(groups);
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
            return rowsAffected;
        }

      
    }
}