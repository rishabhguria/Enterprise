using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ComplianceEngine.RuleDefinition.BLL
{
    /// <summary>
    /// This class stores cache for group notification settings.
    /// </summary>
    internal class GroupCache
    {
        /// <summary>
        /// Cache for storing group notification settings
        /// Key group id value group base
        /// </summary>
        private Dictionary<String, GroupBase> _groupNotificationCache = new Dictionary<string, GroupBase>();

        /// <summary>
        /// Locker object for cache
        /// </summary>
        private object _groupCacheLocker = new Object();

        #region singletonInstance
        /// <summary>
        /// creates singleton instance
        /// </summary>
        private static GroupCache _groupCache;
        private static object _groupSingletonLocker = new Object();

        internal static GroupCache GetInstance()
        {
            lock (_groupSingletonLocker)
            {
                if (_groupCache == null)
                    _groupCache = new GroupCache();
                return _groupCache;
            }
        }


        private GroupCache()
        {

        }

        #endregion

        /// <summary>
        /// Add or update groups in cache
        /// </summary>
        /// <param name="groupList"></param>
        internal void AddUpdateGroup(List<GroupBase> groupList)
        {
            try
            {
                lock (_groupCacheLocker)
                {
                    foreach (GroupBase group in groupList)
                    {
                        if (_groupNotificationCache.ContainsKey(group.GroupId))
                        {
                            _groupNotificationCache[group.GroupId] = group;
                        }
                        else
                        {
                            _groupNotificationCache.Add(group.GroupId, group);
                        }
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


        /// <summary>
        /// Rename Groups in cache
        /// </summary>
        /// <param name="renamedDict"></param>
        internal void RenameGroup(Dictionary<string, string> renamedDict)
        {
            try
            {
                lock (_groupCacheLocker)
                {
                    foreach (String key in renamedDict.Keys)
                    {
                        if (_groupNotificationCache.ContainsKey(key))
                        {
                            _groupNotificationCache[key].GroupName = renamedDict[key];
                        }
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

        /// <summary>
        /// Delete group from cache
        /// </summary>
        /// <param name="deletedList"></param>
        internal void DeleteGroup(List<string> deletedList)
        {
            try
            {
                lock (_groupCacheLocker)
                {
                    foreach (String key in deletedList)
                    {
                        if (_groupNotificationCache.ContainsKey(key))
                        {
                            _groupNotificationCache.Remove(key);
                        }
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

        /// <summary>
        /// Returns Group base for Group ID
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        internal GroupBase GetGroupForId(string groupId)
        {
            try
            {
                lock (_groupCacheLocker)
                {
                    if (_groupNotificationCache.ContainsKey(groupId))
                        return _groupNotificationCache[groupId].DeepClone();
                    else
                        return null;
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
                return null;
            }
        }

        /// <summary>
        /// If rule is renamed then update rule name and rule Id if exists in group.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="oldValue"></param>
        private void RenameRuleInGroup(List<RuleBase> list, string oldValue)
        {
            try
            {
                lock (_groupCacheLocker)
                {
                    foreach (RuleBase renamedRule in list)
                    {
                        if (_groupNotificationCache.ContainsKey(renamedRule.GroupId))
                        {
                            foreach (RuleBase rule in _groupNotificationCache[renamedRule.GroupId].RuleList)
                            {
                                if (rule.RuleId == oldValue)
                                {
                                    rule.RuleName = renamedRule.RuleName;
                                    rule.RuleId = renamedRule.RuleId;
                                    if (!String.IsNullOrEmpty(renamedRule.RuleURL))
                                        rule.RuleURL = renamedRule.RuleURL;
                                    break;
                                }
                            }
                        }
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

        /// <summary>
        /// Update Group cache in case of Rename and Delete rule operation
        /// </summary>
        /// <param name="list"></param>
        /// <param name="operation"></param>
        /// <param name="oldValue">for rename only contains old rule Id</param>
        internal void AddUpdateRuleInGroup(List<RuleBase> list, RuleOperations operation, string oldValue)
        {
            try
            {
                switch (operation)
                {
                    case RuleOperations.DeleteRule:
                        DeleteRuleInGroup(list);
                        break;
                    case RuleOperations.RenameRule:
                        RenameRuleInGroup(list, oldValue);
                        break;
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

        /// <summary>
        /// Deletes rule in group.
        /// </summary>
        /// <param name="list"></param>
        private void DeleteRuleInGroup(List<RuleBase> list)
        {
            try
            {
                lock (_groupCacheLocker)
                {
                    foreach (RuleBase deletedRule in list)
                    {
                        if (_groupNotificationCache.ContainsKey(deletedRule.GroupId) && _groupNotificationCache[deletedRule.GroupId].RuleList.Contains(deletedRule))
                        {
                            _groupNotificationCache[deletedRule.GroupId].RuleList.Remove(deletedRule);
                        }
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

        /// <summary>
        /// Returns list of group base in cache
        /// </summary>
        /// <returns></returns>
        //internal List<GroupBase> GetGroupList()
        //{
        //    try
        //    {
        //        List<GroupBase> groupList = new List<GroupBase>();
        //        lock (_groupCacheLocker)
        //        {
        //            foreach (String key in _groupNotificationCache.Keys)
        //            {
        //                groupList.Add(_groupNotificationCache[key].DeepClone());
        //            }
        //        }
        //        return groupList;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //        return null;
        //    }

        //}

        /// <summary>
        /// Disposes singleton instance
        /// </summary>
        internal void Dispose()
        {
            try
            {
                lock (_groupSingletonLocker)
                {
                    _groupCache = null;
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

        /// <summary>
        /// Clears group notification cache.
        /// </summary>
        internal void Clear()
        {
            try
            {
                lock (_groupCacheLocker)
                {
                    _groupNotificationCache.Clear();
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
    }
}
