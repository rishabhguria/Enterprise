using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.Allocation.Core.CacheStore
{
    internal class AllocationPostInfoCache
    {
        #region Singleton Instance
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static readonly AllocationPostInfoCache _singeltonInstance = new AllocationPostInfoCache();

        /// <summary>
        /// returns instance of AllocationPostInfoCache
        /// </summary>
        internal static AllocationPostInfoCache Instance
        {
            get
            {
                return _singeltonInstance;
            }
        }

        /// <summary>
        /// Constructor that initialize cache.
        /// </summary>
        private AllocationPostInfoCache()
        {
            _allocationPostInfoCache = new Dictionary<string, AllocationParameter>();
            _userAllocationPostInfoCache = new Dictionary<string, Dictionary<int, AllocationParameter>>();
        }

        #endregion


        #region Cache Locker Objects

        /// <summary>
        /// Locker object for cache.
        /// </summary>
        private readonly object _cacheLockerObject = new object();

        /// <summary>
        /// Cache for storing parameter for group ids
        /// </summary>
        private Dictionary<string, AllocationParameter> _allocationPostInfoCache;

        /// <summary>
        /// group user wise parameters.
        /// </summary>
        private Dictionary<string, Dictionary<int, AllocationParameter>> _userAllocationPostInfoCache;

        #endregion

        #region Cache Operations

        /// <summary>
        /// Add or update parameter for groups.
        /// </summary>
        /// <param name="groupList">List of allocation groups</param>
        /// <param name="parameter">Allocation Parameter</param>
        internal void AddUpdateParameterForGroup(List<AllocationGroup> groupList, AllocationParameter parameter)
        {
            try
            {
                /*---------------------------------------------------------------------
                 * If group is valid then add or update parameter in cache.
                 ---------------------------------------------------------------------*/
                lock (_cacheLockerObject)
                {
                    foreach (AllocationGroup group in groupList)
                    {
                        if (IsGroupsPostInfoToBeSaved(group))
                        {
                            if (_allocationPostInfoCache.ContainsKey(group.GroupID))
                                _allocationPostInfoCache[group.GroupID] = parameter;
                            else
                                _allocationPostInfoCache.Add(group.GroupID, parameter);
                        }
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

        /// <summary>
        /// Checking if group id filled or partially filled.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private bool IsGroupsPostInfoToBeSaved(AllocationGroup group)
        {
            try
            {
                /*-------------------------------------------------
                 * Assuming  if quantity is greater than cum qty then it is partially filled.
                 * 
                 -------------------------------------------------*/
                if (group.CumQty < group.Quantity)
                    return true;
                else
                    return false;
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
        /// Updates user cache when user updates from allcoation status from allocationn UI.
        /// </summary>
        /// <param name="groupList">List of groups</param>
        /// <param name="userId">user id</param>
        /// <param name="parameter">Allcoation parameter</param>
        internal void AddUpdateUserParameterForGroups(List<AllocationGroup> groupList, int userId, AllocationParameter parameter)
        {
            try
            {
                /*-----------------------------------------------------------------------------------------------------
                 * if post info cache contains group id then only update in user cache.
                 * if user cache contain group id and also user id then update
                 * if user cache contain group id and not user id then add user id and parameter
                 * if user cache does not contains then add group user and parameter.
                 -----------------------------------------------------------------------------------------------------*/

                if (!parameter.IsPreview)
                {
                    lock (_cacheLockerObject)
                    {
                        foreach (AllocationGroup group in groupList)
                        {
                            string groupId = group.GroupID;
                            if (_allocationPostInfoCache.ContainsKey(groupId))
                            {
                                if (_userAllocationPostInfoCache.ContainsKey(groupId))
                                {
                                    if (_userAllocationPostInfoCache[groupId].ContainsKey(userId))
                                        _userAllocationPostInfoCache[groupId][userId] = parameter;
                                    else
                                        _userAllocationPostInfoCache[groupId].Add(userId, parameter);
                                }
                                else
                                {
                                    _userAllocationPostInfoCache.Add(groupId, new Dictionary<int, AllocationParameter>());
                                    _userAllocationPostInfoCache[groupId].Add(userId, parameter);
                                }
                            }
                            else
                            {
                                _allocationPostInfoCache.Add(groupId, parameter);
                                if (_userAllocationPostInfoCache.ContainsKey(groupId))
                                {
                                    if (_userAllocationPostInfoCache[groupId].ContainsKey(userId))
                                        _userAllocationPostInfoCache[groupId][userId] = parameter;
                                    else
                                        _userAllocationPostInfoCache[groupId].Add(userId, parameter);
                                }
                                else
                                {
                                    _userAllocationPostInfoCache.Add(groupId, new Dictionary<int, AllocationParameter>());
                                    _userAllocationPostInfoCache[groupId].Add(userId, parameter);
                                }
                            }
                        }
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

        /// <summary>
        /// Update main cache on save from user cache
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="groupIdList">list of group id</param>
        internal void UpdatePostInfoCacheFromUser(int userId, List<string> groupIdList)
        {
            try
            {
                /*--------------------------------------------------------------
                 * if save and user cache contains group id and user id then replace parameter in group cache
                 * and remove group id from user cache.
                 */
                lock (_cacheLockerObject)
                {
                    foreach (string groupId in groupIdList)
                    {
                        if (_userAllocationPostInfoCache.ContainsKey(groupId) && _userAllocationPostInfoCache[groupId].ContainsKey(userId))
                        {
                            _allocationPostInfoCache[groupId] = DeepCopyHelper.Clone(_userAllocationPostInfoCache[groupId][userId]);
                            _userAllocationPostInfoCache.Remove(groupId);
                        }
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

        /// <summary>
        /// checks groupid exists in cache or not.
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>bool</returns>
        internal bool IsExistsInPostGroupCache(string groupId)
        {
            try
            {
                lock (_cacheLockerObject)
                {
                    if (_allocationPostInfoCache.ContainsKey(groupId))
                        return true;
                    else
                        return false;
                }
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
        /// returns clone of parameter for group id
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>AllocationParameter</returns>
        internal AllocationParameter GetAllocationParameter(string groupId)
        {
            try
            {
                lock (_cacheLockerObject)
                {
                    return DeepCopyHelper.Clone(_allocationPostInfoCache[groupId]);
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Adds the update parameter for group from tt.
        /// </summary>
        /// <param name="groupList">The group list.</param>
        /// <param name="parameter">The parameter.</param>
        internal void AddUpdateParameterForGroupFromTT(List<AllocationGroup> groupList, ref AllocationParameter parameter)
        {
            try
            {
                //For virtual Parameter generated for MF allcoation do not fetch parameter from cache as already done in AllocateByPreference
                if (parameter.IsVirtual)
                {
                    AddUpdateParameterForGroup(groupList, parameter);
                }
                else
                {
                    //if IsExistsInPostGroupCache true then get parameter else add parameter.
                    //Assuming if user id is -1, and there will be only one group in grouplist.
                    if (IsExistsInPostGroupCache(groupList[0].GroupID))
                    {
                        parameter = GetAllocationParameter(groupList[0].GroupID);
                        parameter.UpdateUserId(-1);
                    }
                    else
                        AddUpdateParameterForGroup(groupList, parameter);
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
        /// Gets the current parameter for group.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        /// <returns></returns>
        internal AllocationParameter GetCurrentParameterForGroup(AllocationGroup allocationGroup)
        {
            AllocationParameter param = null;
            try
            {
                if (IsExistsInPostGroupCache(allocationGroup.GroupID))
                {
                    param = GetAllocationParameter(allocationGroup.GroupID);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return param;
        }

        #endregion
    }
}
