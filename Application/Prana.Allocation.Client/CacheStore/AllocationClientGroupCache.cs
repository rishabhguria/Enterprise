using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Prana.Allocation.Client.CacheStore
{
    /// <summary>
    /// Allocation groups Cache
    /// </summary>
    sealed class AllocationClientGroupCache : IDisposable
    {
        #region Members

        /// <summary>
        /// The _singelton instance
        /// </summary>
        private static AllocationClientGroupCache _singeltonInstance = new AllocationClientGroupCache();

        /// <summary>
        /// The _allocated groups
        /// </summary>
        private GenericBindingList<AllocationGroup> _allocatedGroups = new GenericBindingList<AllocationGroup>();

        /// <summary>
        /// The _unallocated groups
        /// </summary>
        private GenericBindingList<AllocationGroup> _unAllocatedGroups = new GenericBindingList<AllocationGroup>();

        /// <summary>
        /// The _dict groups
        /// </summary>
        private Dictionary<string, AllocationGroup> _dictGroups = new Dictionary<string, AllocationGroup>();

        /// <summary>
        /// The _dictunsaved cancel amend
        /// </summary>
        private Dictionary<string, AllocationGroup> _dictunsavedCancelAmend = new Dictionary<string, AllocationGroup>();

        /// <summary>
        /// The _deleted groups
        /// </summary>
        private AllocationGroupCollection _deletedGroups = new AllocationGroupCollection();

        /// <summary>
        /// The _deleted omitted groups
        /// </summary>
        private AllocationGroupCollection _deletedOmittedGroups = new AllocationGroupCollection();

        /// <summary>
        /// The _allocation groups cache lock
        /// </summary>
        private ReaderWriterLockSlim _allocationGroupsCacheLock = new ReaderWriterLockSlim();

        /// <summary>
        /// The _dictunderlying exercised groups
        /// </summary>
        private Dictionary<string, List<AllocationGroup>> _dictunderlyingExercisedGroups = new Dictionary<string, List<AllocationGroup>>();

        /// <summary>
        /// The _singleton locker
        /// </summary>
        private static readonly object _singletonLocker = new object();

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets the get instance.
        /// </summary>
        /// <value>
        /// The get instance.
        /// </value>
        internal static AllocationClientGroupCache GetInstance
        {
            get
            {
                if (_singeltonInstance == null)
                {
                    lock (_singletonLocker)
                    {
                        if (_singeltonInstance == null)
                            _singeltonInstance = new AllocationClientGroupCache();
                    }
                }
                return _singeltonInstance;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="AllocationClientGroupCache"/> class from being created.
        /// </summary>
        private AllocationClientGroupCache()
        {
            try
            {
                Initialize();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Adds the deleted group.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        internal void AddDeletedGroup(AllocationGroup allocationGroup)
        {
            _allocationGroupsCacheLock.EnterWriteLock();
            try
            {
                if (!_deletedGroups.Contains(allocationGroup))
                    _deletedGroups.Add(allocationGroup);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _allocationGroupsCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Adds the deleted omitted group.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        internal void AddDeletedOmittedGroup(AllocationGroup allocationGroup)
        {
            _allocationGroupsCacheLock.EnterWriteLock();
            try
            {
                if (!_deletedOmittedGroups.Contains(allocationGroup))
                    _deletedOmittedGroups.Add(allocationGroup);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _allocationGroupsCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Adds the dictionary group.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        internal void AddDictGroup(AllocationGroup allocationGroup)
        {
            _allocationGroupsCacheLock.EnterWriteLock();
            try
            {
                if (_dictGroups.ContainsKey(allocationGroup.GroupID))
                    _dictGroups[allocationGroup.GroupID] = allocationGroup;
                else
                    _dictGroups.Add(allocationGroup.GroupID, allocationGroup);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _allocationGroupsCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Adds the dictionary unsaved cancel ammend.
        /// </summary>
        /// <param name="GroupID">The group identifier.</param>
        /// <param name="allocationGroup">The allocation group.</param>
        internal void AddDictUnsavedCancelAmmend(string GroupID, AllocationGroup allocationGroup)
        {
            _allocationGroupsCacheLock.EnterWriteLock();
            try
            {
                if (!_dictunsavedCancelAmend.ContainsKey(GroupID))
                    _dictunsavedCancelAmend.Add(GroupID, allocationGroup);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _allocationGroupsCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Adds the exercised groups.
        /// </summary>
        /// <param name="UnderlyingGroup">The underlying group.</param>
        /// <param name="ClosingGroup">The closing group.</param>
        internal void AddExercisedGroups(AllocationGroup UnderlyingGroup, AllocationGroup ClosingGroup)
        {
            _allocationGroupsCacheLock.EnterWriteLock();
            try
            {
                if (UnderlyingGroup.PersistenceStatus.Equals(ApplicationConstants.PersistenceStatus.ReAllocated))
                {
                    List<AllocationGroup> groups = new List<AllocationGroup>();
                    groups.Add(UnderlyingGroup);
                    groups.Add(ClosingGroup);
                    if (!_dictunderlyingExercisedGroups.ContainsKey(UnderlyingGroup.GroupID))
                        _dictunderlyingExercisedGroups.Add(UnderlyingGroup.GroupID, groups);
                    else
                        _dictunderlyingExercisedGroups[UnderlyingGroup.GroupID] = groups;
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
                _allocationGroupsCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// add group in cache and respective allocation type
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        internal void AddGroup(AllocationGroup allocationGroup)
        {
            _allocationGroupsCacheLock.EnterWriteLock();
            try
            {
                if (!_dictGroups.ContainsKey(allocationGroup.GroupID))
                {
                    _dictGroups.Add(allocationGroup.GroupID, allocationGroup);
                    if (Math.Round(allocationGroup.AllocatedQty, 4) < Math.Round(allocationGroup.CumQty, 4))
                    {
                        _unAllocatedGroups.Add(allocationGroup);
                    }
                    else if (allocationGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                    {
                        _unAllocatedGroups.Add(allocationGroup);
                    }
                    else if (allocationGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    {
                        _allocatedGroups.Add(allocationGroup);
                    }
                }
                else
                {
                    _dictGroups[allocationGroup.GroupID] = allocationGroup;
                    if (Math.Round(allocationGroup.AllocatedQty, 4) < Math.Round(allocationGroup.CumQty, 4))
                    {
                        _allocatedGroups.Remove(allocationGroup);
                        if (!_unAllocatedGroups.Contains(allocationGroup))
                        {
                            _unAllocatedGroups.Add(allocationGroup);
                        }
                        else
                        {
                            //   if (_dictGroups[allocationGroup.GroupID].CumQty <= allocationGroup.CumQty)
                            _unAllocatedGroups.UpdateItem(allocationGroup);
                        }
                    }
                    else if (allocationGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                    {
                        _allocatedGroups.Remove(allocationGroup);
                        if (!_unAllocatedGroups.Contains(allocationGroup))
                        {
                            _unAllocatedGroups.Add(allocationGroup);
                        }
                    }
                    else if (allocationGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    {
                        if (_unAllocatedGroups.Contains(allocationGroup))
                        {
                            _unAllocatedGroups.Remove(allocationGroup);
                        }

                        if (!_allocatedGroups.Contains(allocationGroup))
                        {
                            _allocatedGroups.Add(allocationGroup);
                        }
                        else
                        {
                            //if (_dictGroups[allocationGroup.GroupID].CumQty <= allocationGroup.CumQty)
                            _allocatedGroups.UpdateItem(allocationGroup);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _allocationGroupsCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Adds the unallocated group.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        internal void AddUnallocatedGroup(AllocationGroup allocationGroup)
        {
            _allocationGroupsCacheLock.EnterWriteLock();
            try
            {
                if (_unAllocatedGroups.Contains(allocationGroup))
                    _unAllocatedGroups.UpdateItem(allocationGroup);
                else
                    _unAllocatedGroups.Add(allocationGroup);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _allocationGroupsCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Add UpdateGroupList in cache
        /// </summary>
        /// <param name="updatedGroupLists"></param>
        internal void AddUpdateGroups(List<AllocationGroup> updatedGroupLists)
        {
            try
            {
                foreach (AllocationGroup group in updatedGroupLists)
                {
                    AddGroup(group);
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
        /// Anything is changed.
        /// </summary>
        /// <returns></returns>
        internal bool AnythingChanged()
        {
            _allocationGroupsCacheLock.EnterReadLock();
            try
            {
                foreach (AllocationGroup group in _unAllocatedGroups)
                {
                    if (IsGroupStatusChanged(group))
                    {
                        return true;
                    }
                }
                foreach (AllocationGroup group in _allocatedGroups)
                {
                    if (IsGroupStatusChanged(group))
                    {
                        return true;
                    }
                }
                foreach (AllocationGroup group in _deletedGroups)
                {
                    if (IsGroupStatusChanged(group))
                    {
                        return true;
                    }
                }
                foreach (AllocationGroup group in _deletedOmittedGroups)
                {
                    if (IsGroupStatusChanged(group))
                    {
                        return true;
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
                _allocationGroupsCacheLock.ExitReadLock();
            }
            return false;
        }

        /// <summary>
        ///     This method confirms that there is no duplicate entry in the _deletedGroups cache
        ///     This method is used only to check duplicate entry in _deletedGroups cache
        /// </summary>
        /// <param name="group"></param>
        /// <returns>returns true if </returns>
        internal bool CheckDuplicate(AllocationGroup group)
        {
            _allocationGroupsCacheLock.EnterReadLock();
            try
            {
                return _deletedGroups.Cast<AllocationGroup>().Any(algroup => algroup.Orders[0].ParentClOrderID.Equals(group.Orders[0].ParentClOrderID));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _allocationGroupsCacheLock.ExitReadLock();
            }
            return false;
        }

        /// <summary>
        /// Clears the caches.
        /// </summary>
        internal void ClearCaches()
        {
            _allocationGroupsCacheLock.EnterWriteLock();
            try
            {
                _unAllocatedGroups.Clear();
                _dictunsavedCancelAmend.Clear();
                _allocatedGroups.Clear();
                _dictGroups.Clear();
                _deletedGroups.Clear();
                _deletedOmittedGroups.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _allocationGroupsCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Clears the deleted groups.
        /// </summary>
        internal void ClearDeletedGroups()
        {
            _allocationGroupsCacheLock.EnterWriteLock();
            try
            {
                _deletedGroups.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _allocationGroupsCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Clears the deleted omitted groups.
        /// </summary>
        internal void ClearDeletedOmittedGroups()
        {
            _allocationGroupsCacheLock.EnterWriteLock();
            try
            {
                _deletedOmittedGroups.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _allocationGroupsCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Clears the dictionary unsaved cancel amend.
        /// </summary>
        internal void ClearDictUnsavedCancelAmend()
        {
            _allocationGroupsCacheLock.EnterWriteLock();
            try
            {
                _dictunsavedCancelAmend.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _allocationGroupsCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Clears the exercised groups dictionary.
        /// </summary>
        internal void ClearExercisedGroupsDictionary()
        {
            _allocationGroupsCacheLock.EnterWriteLock();
            try
            {
                if (_dictunderlyingExercisedGroups != null)
                    _dictunderlyingExercisedGroups.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _allocationGroupsCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Deletes the group.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        internal void DeleteGroup(string groupId)
        {
            _allocationGroupsCacheLock.EnterWriteLock();
            try
            {
                if (_dictGroups.ContainsKey(groupId))
                {
                    AllocationGroup deletedGroup = _dictGroups[groupId];
                    _unAllocatedGroups.Remove(deletedGroup);
                    _allocatedGroups.Remove(deletedGroup);
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
                _allocationGroupsCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Dictionaries the unsaved contains key.
        /// </summary>
        /// <param name="keyGroupId">The key group identifier.</param>
        /// <returns></returns>
        internal bool DictUnsavedContainsKey(string keyGroupId)
        {
            _allocationGroupsCacheLock.EnterReadLock();
            bool result = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(keyGroupId))
                    result = _dictunsavedCancelAmend.ContainsKey(keyGroupId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

            finally
            {
                _allocationGroupsCacheLock.ExitReadLock();
            }
            return result;
        }

        /// <summary>
        /// Gets the allocated groups.
        /// </summary>
        /// <returns></returns>
        internal GenericBindingList<AllocationGroup> GetAllocatedGroups()
        {
            _allocationGroupsCacheLock.EnterReadLock();
            try
            {
                return _allocatedGroups;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
            finally
            {
                _allocationGroupsCacheLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Dictionaries the unsaved contains key.
        /// </summary>
        /// <param name="keyGroupId">The key group identifier.</param>
        /// <returns></returns>
        internal AllocationGroup GetDictUnsavedKeyValue(string keyGroupId)
        {
            _allocationGroupsCacheLock.EnterReadLock();
            AllocationGroup group = null;
            try
            {
                group = _dictunsavedCancelAmend[keyGroupId];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _allocationGroupsCacheLock.ExitReadLock();
            }
            return group;
        }

        /// <summary>
        /// Gets the group.
        /// </summary>
        /// <param name="groupID">The group identifier.</param>
        /// <returns></returns>
        internal AllocationGroup GetGroup(string groupID)
        {
            try
            {
                if (_dictGroups.ContainsKey(groupID))
                    return _dictGroups[groupID];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Gets the unallocated groups.
        /// </summary>
        /// <returns></returns>
        internal GenericBindingList<AllocationGroup> GetUnallocatedGroups()
        {
            _allocationGroupsCacheLock.EnterReadLock();
            try
            {
                return _unAllocatedGroups;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
            finally
            {
                _allocationGroupsCacheLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            try
            {
                if (_allocationGroupsCacheLock == null)
                    _allocationGroupsCacheLock = new ReaderWriterLockSlim();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Determines whether [is group deleted] [the specified group identifier].
        /// </summary>
        /// <param name="groupID">The group identifier.</param>
        /// <returns></returns>
        internal bool IsGroupDeleted(string groupID)
        {
            _allocationGroupsCacheLock.EnterReadLock();
            try
            {
                if (_deletedGroups.Cast<AllocationGroup>().Any(deletedgroup => deletedgroup.GroupID == groupID))
                {
                    return true;
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
                _allocationGroupsCacheLock.ExitReadLock();
            }
            return false;
        }

        /// <summary>
        /// Determines whether [is groups dirty] [the specified group].
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        internal bool IsGroupsDirty(AllocationGroup group)
        {
            _allocationGroupsCacheLock.EnterReadLock();
            try
            {
                //Narendra Kumar Jangir 2013 Mar 05
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-2238
                //dirty groups should be checked on the basis of parentclorderID
                if (_dictGroups.Where(
                    algroup => (algroup.Value.Orders.Count > 0) && (algroup.Value.Orders[0].ParentClOrderID != null) &&
                               !algroup.Value.Orders[0].ParentClOrderID.Equals(string.Empty) &&
                               (group.Orders.Count > 0) &&
                               (group.Orders[0].ParentClOrderID != null) &&
                               !group.Orders[0].ParentClOrderID.Equals(string.Empty))
                    .Any(
                        algroup =>
                            algroup.Value.Orders[0].ParentClOrderID.Equals(group.Orders[0].ParentClOrderID) &&
                            (algroup.Value.PersistenceStatus != group.PersistenceStatus)))
                {
                    return true;
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
                _allocationGroupsCacheLock.ExitReadLock();
            }
            return false;
        }

        /// <summary>
        /// Determines whether [is group status changed] [the specified group].
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        private bool IsGroupStatusChanged(AllocationGroup group)
        {
            bool isChanged = false;
            try
            {
                switch (group.PersistenceStatus)
                {
                    case ApplicationConstants.PersistenceStatus.UnGrouped:
                    case ApplicationConstants.PersistenceStatus.New:
                    case ApplicationConstants.PersistenceStatus.ReAllocated:
                    case ApplicationConstants.PersistenceStatus.Deleted:
                        isChanged = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isChanged;
        }

        /// <summary>
        /// Removes the dictionary group.
        /// </summary>
        /// <param name="groupID">The group identifier.</param>
        internal void RemoveDictGroup(string groupID)
        {
            _allocationGroupsCacheLock.EnterWriteLock();
            try
            {
                if (_dictGroups.ContainsKey(groupID))
                    _dictGroups.Remove(groupID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _allocationGroupsCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes the unallocated group.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        internal void RemoveUnallocatedGroup(List<AllocationGroup> allocationGroups)
        {
            _allocationGroupsCacheLock.EnterWriteLock();
            try
            {
                foreach (AllocationGroup allocationGroup in allocationGroups)
                {
                    if (_unAllocatedGroups.Contains(allocationGroup))
                        _unAllocatedGroups.Remove(allocationGroup);
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
                _allocationGroupsCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes the unallocated group.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        internal void RemoveUnallocatedGroup(AllocationGroup allocationGroup)
        {
            _allocationGroupsCacheLock.EnterWriteLock();
            try
            {
                if (_unAllocatedGroups.Contains(allocationGroup))
                    _unAllocatedGroups.Remove(allocationGroup);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                _allocationGroupsCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Sets the default persistence status.
        /// </summary>
        /// <param name="group">The group.</param>
        internal void SetDefaultPersistenceStatus(AllocationGroup group)
        {
            try
            {
                group.PersistenceStatus = ApplicationConstants.PersistenceStatus.NotChanged;
                group.RemoveDeletedTaxlotsFromResetDictionary();
                foreach (TaxLot taxlot in group.GetAllTaxlots())
                {
                    taxlot.TaxLotState = ApplicationConstants.TaxLotState.NotChanged;
                }
                group.IsAnotherTaxlotAttributesUpdated = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the default persistence status for groups.
        /// </summary>
        internal void SetDefaultPersistenceStatusForGroups()
        {
            _allocationGroupsCacheLock.EnterWriteLock();
            try
            {
                foreach (AllocationGroup group in _unAllocatedGroups)
                {
                    SetDefaultPersistenceStatus(group);
                }
                foreach (AllocationGroup group in _allocatedGroups)
                {
                    SetDefaultPersistenceStatus(group);
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
                _allocationGroupsCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Updates the and get groups for save.
        /// </summary>
        /// <param name="lstDirtyGroups">The LST dirty groups.</param>
        /// <param name="groups">The groups.</param>
        internal void UpdateAndGetGroupsForSave(List<AllocationGroup> lstDirtyGroups, List<AllocationGroup> groups)
        {
            _allocationGroupsCacheLock.EnterWriteLock();
            try
            {
                foreach (var group in _unAllocatedGroups)
                {
                    if (group.TradeActionsList.Contains(TradeAuditActionType.ActionType.Commission_Changed))
                        group.IsCommissionChanged = true;

                    if (group.TradeActionsList.Contains(TradeAuditActionType.ActionType.SoftCommission_Changed))
                        group.IsSoftCommissionChanged = true;

                    if (group.PersistenceStatus != ApplicationConstants.PersistenceStatus.NotChanged)
                    {
                        group.ModifiedUserId = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                        if (group.ClosingStatus != ClosingStatus.Open)
                            lstDirtyGroups.Add(group);
                        else
                            groups.Add(group);
                    }
                }
                foreach (var group in _allocatedGroups)
                {
                    if (group.PersistenceStatus != ApplicationConstants.PersistenceStatus.NotChanged)
                    {
                        //20180315 (https://jira.nirvanasolutions.com:8443/browse/PRANA-27222)
                        //This scenario is now handled through data dirty checks while data is published from closing UI 
                        //and requiring data referesh in case the data is now dirty

                        //Mukul : 20131118:
                        //Allocated Groups which are partially or fully closed and have there persistence status as reallocated shouldnt be allowed to save
                        // as that will corrupt the data..This is a temporary solution as the ideal way to handle these scenarios is through data dirty checks while
                        //data is published from closing UI as the persistence status should be same for the both the published group and binded group otherwise the data is dirty and 
                        //needs to be refreshed..

                        //if ((group.ClosingStatus != ClosingStatus.Open) &&
                        //    (group.PersistenceStatus == ApplicationConstants.PersistenceStatus.ReAllocated))
                        //{
                        //    //Sandeep: 20131106 (http://jira.nirvanasolutions.com:8080/browse/PRANA-2874)
                        //    //lstDirtyGroups contain those groups whose closing status has been changed to closed or partially closed after they were reallocated..
                        //    //Scenario..
                        //    //1) user opens close Trade by right click a closing transaction..
                        //    //2.) user reallocates the same closing transaction..
                        //    //3.) closes that group from close order UI..
                        //    //4.) clicks save from main allocation UI..
                        //    //5.) That group should not allowed to save as it will corrupt the data in Pm_Taxlots table and remove the closing trail entry..
                        //    lstDirtyGroups.Add(group);
                        //}
                        //else
                        //{
                        group.ModifiedUserId = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                        if (group.TradeActionsList.Contains(TradeAuditActionType.ActionType.Commission_Changed))
                            group.IsCommissionChanged = true;

                        if (group.TradeActionsList.Contains(TradeAuditActionType.ActionType.SoftCommission_Changed))
                            group.IsSoftCommissionChanged = true;

                        groups.Add(group);
                        //}
                    }
                }
                foreach (AllocationGroup group in _deletedGroups)
                {
                    if (group.ClosingStatus != ClosingStatus.Open)
                        lstDirtyGroups.Add(group);
                    else
                    {
                        group.PersistenceStatus = ApplicationConstants.PersistenceStatus.UnGrouped;
                        groups.Add(group);
                    }
                }
                foreach (AllocationGroup group in _deletedOmittedGroups)
                {
                    group.PersistenceStatus = ApplicationConstants.PersistenceStatus.Deleted;
                    if (_unAllocatedGroups.Contains(group))
                        _unAllocatedGroups.Remove(group);
                    else
                        groups.Add(group);
                }
                if (_dictunderlyingExercisedGroups != null)
                {
                    foreach (var kp in _dictunderlyingExercisedGroups)
                    {
                        groups.AddRange(kp.Value);
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
                _allocationGroupsCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Updates the group closing status.
        /// </summary>
        /// <param name="taxlotsList">The taxlots list.</param>
        internal bool UpdateGroupClosingStatus(List<TaxLot> taxlotsList)
        {
            _allocationGroupsCacheLock.EnterWriteLock();
            bool isDirtyData = false;
            try
            {
                //collect all taxlots of a group in a dictionary
                Dictionary<string, List<TaxLot>> groupIDWiseTaxlotsDict = new Dictionary<string, List<TaxLot>>();
                foreach (TaxLot publishedTaxlot in taxlotsList)
                {
                    if (groupIDWiseTaxlotsDict.ContainsKey(publishedTaxlot.GroupID))
                    {
                        List<TaxLot> taxlotList = groupIDWiseTaxlotsDict[publishedTaxlot.GroupID];
                        taxlotList.Add(publishedTaxlot);
                        groupIDWiseTaxlotsDict[publishedTaxlot.GroupID] = taxlotList;
                    }
                    else
                    {
                        List<TaxLot> taxlotList = new List<TaxLot>();
                        taxlotList.Add(publishedTaxlot);
                        groupIDWiseTaxlotsDict.Add(publishedTaxlot.GroupID, taxlotList);
                    }
                }


                foreach (KeyValuePair<string, List<TaxLot>> kvp in groupIDWiseTaxlotsDict)
                {
                    AllocationGroup group = GetGroup(kvp.Key);

                    if (group == null)
                    {
                        foreach (AllocationGroup grp in _deletedGroups)
                        {
                            if (grp.GroupID.Equals(kvp.Key))
                            {
                                isDirtyData = true;
                                break;
                            }
                        }
                        if (isDirtyData) break;
                    }
                    else if (group.PersistenceStatus != ApplicationConstants.PersistenceStatus.NotChanged)
                    {
                        isDirtyData = true;
                        break;
                    }
                    else
                    {
                        List<TaxLot> groupTaxlotList = group.TaxLots;

                        List<TaxLot> publishedTaxlotList = groupIDWiseTaxlotsDict[kvp.Key];

                        //update group taxlots
                        foreach (TaxLot updatedtaxlot in publishedTaxlotList)
                        {
                            foreach (TaxLot innerTaxLot in groupTaxlotList)
                            {
                                if (updatedtaxlot.TaxLotID.Equals(innerTaxLot.TaxLotID))
                                {
                                    innerTaxLot.ClosingStatus = updatedtaxlot.ClosingStatus;
                                    innerTaxLot.ClosingMode = updatedtaxlot.ClosingMode;
                                    innerTaxLot.ClosingDate = updatedtaxlot.ClosingDate;
                                    innerTaxLot.ClosingAlgo = updatedtaxlot.ClosingAlgo;
                                    break;
                                }
                            }
                        }

                        //update group close status   
                        group.ClosingStatus = Prana.BusinessObjects.AppConstants.ClosingStatus.Open;
                        int closeCount = 0;
                        foreach (TaxLot taxlot in groupTaxlotList)
                        {
                            UpdateGroupStatus(group, taxlot);
                            group.ClosingAlgoText = Enum.GetName(typeof(PostTradeEnums.CloseTradeAlogrithm),
                                taxlot.ClosingAlgo);
                            //update the minimum of close trade of all taxlots in a group
                            if (taxlot.ClosingDate != DateTimeConstants.MinValue
                                && (group.ClosingDate > taxlot.ClosingDate
                                    || group.ClosingDate == DateTimeConstants.MinValue))
                            {
                                group.ClosingDate = taxlot.ClosingDate;
                            }
                            if ((taxlot.ClosingStatus ==
                                 Prana.BusinessObjects.AppConstants.ClosingStatus.PartiallyClosed))
                            {
                                group.ClosingStatus =
                                    Prana.BusinessObjects.AppConstants.ClosingStatus.PartiallyClosed;
                                closeCount++;
                                break;
                            }
                            if (taxlot.ClosingStatus == Prana.BusinessObjects.AppConstants.ClosingStatus.Closed)
                            {
                                closeCount += 2;
                            }
                        }

                        if ((closeCount / 2) == groupTaxlotList.Count)
                            group.ClosingStatus = Prana.BusinessObjects.AppConstants.ClosingStatus.Closed;
                        else if ((closeCount / 2) > 0)
                            group.ClosingStatus = Prana.BusinessObjects.AppConstants.ClosingStatus.PartiallyClosed;
                        if (group.ClosingStatus == Prana.BusinessObjects.AppConstants.ClosingStatus.Open)
                            group.ClosingDate = DateTimeConstants.MinValue;

                        //update allocated groups collection
                        if (_allocatedGroups.Contains(group))
                        {
                            _allocatedGroups.Update(group);
                        }
                    }
                }
                return isDirtyData;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return true;
            }
            finally
            {
                _allocationGroupsCacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Updates the group status.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="taxlot">The taxlot.</param>
        private void UpdateGroupStatus(AllocationGroup group, TaxLot taxlot)
        {
            try
            {
                if (taxlot.ClosingMode.Equals(Prana.BusinessObjects.AppConstants.ClosingMode.CorporateAction))
                    group.GroupStatus = PostTradeEnums.Status.CorporateAction;
                else if (taxlot.ClosingMode.Equals(Prana.BusinessObjects.AppConstants.ClosingMode.Exercise))
                    group.GroupStatus = PostTradeEnums.Status.IsExercised;
                else if (taxlot.ClosingMode.Equals(Prana.BusinessObjects.AppConstants.ClosingMode.Physical))
                    group.GroupStatus = PostTradeEnums.Status.Exercise;
                else if (taxlot.ClosingMode.Equals(Prana.BusinessObjects.AppConstants.ClosingMode.Physical))
                    group.GroupStatus = PostTradeEnums.Status.Exercise;
                else if (taxlot.ClosingMode.Equals(Prana.BusinessObjects.AppConstants.ClosingMode.CostBasisAdjustment))
                    group.GroupStatus = PostTradeEnums.Status.CostBasisAdjustment;
                else
                    group.GroupStatus = PostTradeEnums.Status.None;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Updates the repository with sec master information.
        /// </summary>
        /// <param name="list">The list.</param>
        internal void UpdateRepositoryWithSecMasterInfo(SecMasterbaseList list)
        {
            _allocationGroupsCacheLock.EnterWriteLock();
            try
            {
                foreach (AllocationGroup unallocatedGroup in _unAllocatedGroups)
                {
                    foreach (SecMasterBaseObj obj in list)
                    {
                        if (unallocatedGroup.Symbol.Equals(obj.TickerSymbol))
                        {
                            unallocatedGroup.CopyBasicDetails(obj);
                        }

                    }
                }
                foreach (AllocationGroup allocatedGroup in _allocatedGroups)
                {
                    foreach (SecMasterBaseObj baseobj in list)
                    {
                        if (allocatedGroup.Symbol.Equals(baseobj.TickerSymbol))
                        {
                            allocatedGroup.CopyBasicDetails(baseobj);
                        }
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
                _allocationGroupsCacheLock.ExitWriteLock();

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
                    if (_singeltonInstance != null)
                        _singeltonInstance = null;
                    if (_allocatedGroups != null)
                        _allocatedGroups = null;
                    if (_deletedGroups != null)
                        _deletedGroups = null;
                    if (_deletedOmittedGroups != null)
                        _deletedOmittedGroups = null;
                    if (_dictGroups != null)
                        _dictGroups = null;
                    if (_dictunderlyingExercisedGroups != null)
                        _dictunderlyingExercisedGroups = null;
                    if (_dictunsavedCancelAmend != null)
                        _dictunsavedCancelAmend = null;
                    if (_unAllocatedGroups != null)
                        _unAllocatedGroups = null;
                    if (_allocationGroupsCacheLock != null)
                    {
                        _allocationGroupsCacheLock.Dispose();
                        _allocationGroupsCacheLock = null;
                    }
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
