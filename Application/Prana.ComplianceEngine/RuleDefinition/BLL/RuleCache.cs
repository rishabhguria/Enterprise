using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.LogManager;
using Prana.ServiceConnector;
using System;
using System.Collections.Generic;

namespace Prana.ComplianceEngine.RuleDefinition.BLL
{
    internal class RuleCache
    {
        private static RuleCache _ruleChacheSingletonObject;
        private static object lockerObject = new object();
        private object cacheLocker = new object();
        private Dictionary<String, RuleBase> _ruleCache = new Dictionary<String, RuleBase>();

        public static RuleCache GetInstance()
        {
            lock (lockerObject)
            {
                if (_ruleChacheSingletonObject == null)
                    _ruleChacheSingletonObject = new RuleCache();
                return _ruleChacheSingletonObject;
            }
        }


        internal void DisposeInstance()
        {
            try
            {
                lock (lockerObject)
                {
                    _ruleChacheSingletonObject = null;
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
        /// Add and update rules on rule cache.
        /// </summary>
        /// <param name="ruleList"></param>
        internal void AddOrUpdateRules(List<RuleBase> ruleList, Boolean isCacheUpdateRequiredAtServer = true)
        {
            try
            {
                lock (cacheLocker)
                {
                    foreach (RuleBase rule in ruleList)
                    {
                        String key = rule.RuleId;
                        if (_ruleCache.ContainsKey(key))
                            _ruleCache[key] = rule;
                        else
                        {
                            _ruleCache.Add(key, rule);
                            if (isCacheUpdateRequiredAtServer)
                            {
                                //ComplianceCacheManager.AddRuleInCache(rule.RuleName);
                                ComplianceServiceConnector.GetInstance().AddRuleInCache(rule.RuleName);
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
        /// Clears rule cache
        /// </summary>
        internal void Clear()
        {
            try
            {
                lock (cacheLocker)
                {
                    _ruleCache.Clear();
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
        ///Returns rule for the rule id 
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        internal RuleBase GetRule(string ruleId)
        {
            try
            {
                lock (cacheLocker)
                {
                    if (_ruleCache.ContainsKey(ruleId))
                    {

                        return _ruleCache[ruleId].DeepClone();
                    }
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
        /// Updates Notification settings of rule when loaded from DB
        /// </summary>
        /// <param name="notificationList"></param>
        internal void AddOrUpdateNotificationSettings(Dictionary<string, NotificationSetting> notificationList)
        {
            try
            {
                lock (cacheLocker)
                {
                    foreach (String id in notificationList.Keys)
                    {
                        if (_ruleCache.ContainsKey(id))
                            _ruleCache[id].Notification = notificationList[id];
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
        /// Adds default notification for rules which are not in DB.
        /// </summary>
        /// <param name="notificationSetting"></param>
        /// <returns></returns>
        internal List<RuleBase> UpdateDefaultNotificationIfNotExist(NotificationSetting notificationSetting)
        {
            try
            {
                List<RuleBase> newRuleBase = new List<RuleBase>();

                lock (cacheLocker)
                {
                    foreach (String id in _ruleCache.Keys)
                    {
                        if (_ruleCache[id].Notification == null)
                        {
                            _ruleCache[id].Notification = notificationSetting;
                            newRuleBase.Add(_ruleCache[id].DeepClone());
                        }
                    }
                }
                return newRuleBase;
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
        /// Deletes rule from Cache
        /// </summary>
        /// <param name="list"></param>
        internal void DeleteRule(List<RuleBase> list)
        {
            try
            {
                lock (cacheLocker)
                {
                    foreach (RuleBase rule in list)
                    {
                        if (_ruleCache.ContainsKey(rule.RuleId))
                        {
                            _ruleCache.Remove(rule.RuleId);
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
        /// delete rule at old rule id in and adds updated rule at new rule id.
        /// for user defined rules rule name, rule id, rule url changes,
        /// and for custom rules only rule name changes.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="oldValue"></param>
        /// <returns></returns>
        internal List<RuleBase> RenameRule(List<RuleBase> list, String oldValue)
        {
            try
            {
                List<RuleBase> ruleList = new List<RuleBase>();
                String oldRuleName, newRuleName;
                lock (cacheLocker)
                {
                    foreach (RuleBase rule in list)
                    {
                        if (_ruleCache.ContainsKey(oldValue))
                        {
                            RuleBase tempRule = _ruleCache[oldValue];
                            oldRuleName = tempRule.RuleName;
                            newRuleName = rule.RuleName;
                            tempRule.RuleName = rule.RuleName;
                            tempRule.RuleId = rule.RuleId;
                            if (!String.IsNullOrEmpty(rule.RuleURL))
                                tempRule.RuleURL = rule.RuleURL;
                            _ruleCache.Remove(oldValue);
                            _ruleCache.Add(rule.RuleId, tempRule);
                            ruleList.Add(_ruleCache[rule.RuleId]);
                            // ComplianceCacheManager.UpdateRenamedRule(oldRuleName, newRuleName);
                            ComplianceServiceConnector.GetInstance().UpdateRenamedRule(oldRuleName, newRuleName);
                        }
                    }
                }
                return ruleList;
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
        /// Sets Enable field for the rule.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="enable"></param>
        internal void EnableDisableRule(List<RuleBase> list, bool enable)
        {
            try
            {
                lock (cacheLocker)
                {
                    foreach (RuleBase rule in list)
                    {
                        if (_ruleCache.ContainsKey(rule.RuleId))
                        {
                            _ruleCache[rule.RuleId].Enabled = enable;
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
        /// returns rules from rule cache for the rule category
        /// </summary>
        /// <param name="catComp"></param>
        /// <returns></returns>
        internal List<RuleBase> GetRulesCategoryWise(RuleCategory catComp)
        {
            try
            {
                List<RuleBase> ruleList = new List<RuleBase>();
                lock (cacheLocker)
                {

                    foreach (String id in _ruleCache.Keys)
                    {
                        if (_ruleCache[id].Category == catComp)
                            ruleList.Add(_ruleCache[id].DeepClone());
                    }
                }
                return ruleList;
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
        /// Checks if rule Name exists in cache or not.
        /// </summary>
        /// <param name="ruleName"></param>
        /// <returns></returns>
        internal bool IsRuleNameExists(string ruleName, RulePackage packageName)
        {
            try
            {
                foreach (string ruleId in _ruleCache.Keys)
                {
                    if (_ruleCache[ruleId].RuleName == ruleName && _ruleCache[ruleId].Package == packageName)
                        return true;
                }
                return false;
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
                return true;
            }
        }

        /// <summary>
        /// Retuns list of RuleBase for category in particular package.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="rulePackage"></param>
        /// <returns></returns>
        internal List<RuleBase> GetRulesPackageCategoryWise(RuleCategory category, RulePackage rulePackage)
        {
            try
            {
                List<RuleBase> ruleList = new List<RuleBase>();
                lock (cacheLocker)
                {

                    foreach (String id in _ruleCache.Keys)
                    {
                        if (_ruleCache[id].Category == category && rulePackage == _ruleCache[id].Package)
                            ruleList.Add(_ruleCache[id].DeepClone());
                    }
                }
                return ruleList;
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
        /// Updates GroupId for rules after assignment and unassignment
        /// </summary>
        /// <param name="groupRuleCache"></param>
        internal void UpdateGroupId(Dictionary<string, string> groupRuleCache)
        {
            try
            {
                lock (cacheLocker)
                {
                    foreach (String id in groupRuleCache.Keys)
                    {
                        if (_ruleCache.ContainsKey(id))
                            _ruleCache[id].GroupId = groupRuleCache[id];
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
        /// Get Rule list for groups in list
        /// </summary>
        /// <param name="groupList"></param>
        internal void GetGroupWiseRuleList(ref List<GroupBase> groupList)
        {
            try
            {
                lock (cacheLocker)
                {
                    foreach (String id in _ruleCache.Keys)
                    {
                        if (_ruleCache[id].GroupId != "-1")
                        {
                            foreach (GroupBase group in groupList)
                            {
                                if (group.GroupId == _ruleCache[id].GroupId)
                                    group.RuleList.Add(_ruleCache[id].DeepClone());
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
    }
}