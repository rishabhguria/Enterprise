using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Delegates;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.Compliance.EventArguments;
using Prana.BusinessObjects.Compliance.Interfaces;
using Prana.CommonDataCache;
using Prana.ComplianceEngine.RuleDefinition.DAL;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Prana.ComplianceEngine.RuleDefinition.BLL
{
    internal class RuleManager : IDisposable
    {
        //Dictionary<RuleBase, bool> ruleOperationStatus = new Dictionary<RuleBase, bool>();
        //bool _isOperationComplete = true;

        private static RuleManager _ruleManager;
        private static Object _lockerObject = new Object();
        internal event RuleLoadedHandler AllRulesLoaded;
        internal event RuleOperationHandler RuleOperationCompleted;
        internal event RuleOperationHandler RenameRuleOperationCompleted;
        internal event RuleLoadedHandler LoadGroups;
        //internal event GroupOperationHandler GroupOperationCompleted;

        internal event RuleOperationFromDifferentClientHandler RuleOperationFromDifferentClient;

        #region Singleton instance       

        internal static RuleManager GetInstance()
        {
            lock (_lockerObject)
            {
                if (_ruleManager == null)
                    _ruleManager = new RuleManager();

            }
            return _ruleManager;
        }

        internal static void DisposeInstance()
        {
            lock (_lockerObject)
            {
                try
                {
                    _ruleManager.Dispose();
                    _ruleManager.DisposeGroup();

                    _ruleManager = null;

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

        /// <summary>
        /// 
        /// </summary>
        private RuleManager()
        {
            Initialize();
            //RuleFactoryHandler.GetRuleHandlerFor(RuleCategory.CustomRule).RuleLoaded += RuleManager_RuleLoaded;
        }

        #endregion

        private void Initialize()
        {
            try
            {
                BindRuleHandlerEvents();
                InitializeRuleLoadStatus();
                InitializeProcessHandler();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }





        /// <summary>
        /// Initializes Process manager.
        /// </summary>
        private void InitializeProcessHandler()
        {
            try
            {
                _ruleBaseProcessHandler = new ProcessManager<RuleBase>();
                _ruleBaseProcessHandler.ProcessComplete += _ruleBaseProcessHandler_ProcessComplete;
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
        /// When unapproved list count comes to 0 this event is raised and processing on UI and cache is done for operation complete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void _ruleBaseProcessHandler_ProcessComplete(object sender, ProcessCompletedEventArg<RuleBase> args)
        {
            try
            {
                RuleOperations operation = (RuleOperations)Enum.Parse(typeof(RuleOperations), args.ProcessTag, true);
                switch (operation)
                {
                    case RuleOperations.ExportRule:

                        RuleFactoryHandler.GetCommonRuleHandler().ExportRuleDef(args.ApprovedObjects, ComplianceCacheManager.GetImportExportPath());

                        if (RuleOperationCompleted != null)
                        {
                            bool hasError = args.FailedObjects.Count > 0 ? true : false;
                            String errorMessage = args.Message;
                            RuleOperationEventArgs argu = new RuleOperationEventArgs { RuleList = args.ApprovedObjects, Category = RuleCategory.None, OperationType = RuleOperations.ExportRule, OldValue = null, HasError = hasError, ErrorMessage = errorMessage, FailedRuleList = args.FailedObjects };
                            RuleOperationCompleted(this, argu);
                        }
                        break;
                    case RuleOperations.ImportRule:

                        // RuleCache.GetInstance().AddOrUpdateRules(args.ApprovedObjects);
                        SaveUpdateRule(args.ApprovedObjects);
                        GroupCache.GetInstance().AddUpdateRuleInGroup(args.ApprovedObjects, operation, String.Empty);
                        if (RuleOperationCompleted != null)
                        {
                            bool hasError = args.FailedObjects.Count > 0 ? true : false;
                            String errorMessage = args.Message;
                            RuleOperationEventArgs argu = new RuleOperationEventArgs { RuleList = args.ApprovedObjects, Category = RuleCategory.None, OperationType = RuleOperations.ImportRule, OldValue = null, HasError = hasError, ErrorMessage = errorMessage, FailedRuleList = args.FailedObjects };
                            RuleOperationCompleted(this, argu);
                        }
                        break;
                    case RuleOperations.AddRule:
                        args.ApprovedObjects[0].Notification = new NotificationSetting();
                        args.ApprovedObjects[0].Notification.EmailSubject = "Nirvana: Compliance and Alerting - " + args.ApprovedObjects[0].Package.ToString() + ": ";
                        // RuleCache.GetInstance().AddOrUpdateRules(args.ApprovedObjects);
                        SaveUpdateRule(args.ApprovedObjects);
                        GroupCache.GetInstance().AddUpdateRuleInGroup(args.ApprovedObjects, operation, String.Empty);
                        if (RuleOperationCompleted != null)
                        {
                            bool hasError = args.FailedObjects.Count > 0 ? true : false;
                            String errorMessage = args.Message;
                            RuleOperationEventArgs argu = new RuleOperationEventArgs { RuleList = args.ApprovedObjects, Category = RuleCategory.None, OperationType = RuleOperations.AddRule, OldValue = null, HasError = hasError, ErrorMessage = errorMessage, FailedRuleList = args.FailedObjects };
                            RuleOperationCompleted(this, argu);
                        }
                        break;
                    case RuleOperations.DeleteRule:
                        //Exporting rule before deleting so that it can be recovered if deleted by mistake.
                        RuleFactoryHandler.GetCommonRuleHandler().ExportRuleDef(args.ApprovedObjects, ComplianceCacheManager.GetImportExportPath());
                        RuleCache.GetInstance().DeleteRule(args.ApprovedObjects);
                        RuleFactoryHandler.GetCommonRuleHandler().DeleteRule(args.ApprovedObjects);
                        //RuleFactoryHandler.GetCommonRuleHandler().ArchiveAlerts(args.ApprovedObjects);
                        GroupCache.GetInstance().AddUpdateRuleInGroup(args.ApprovedObjects, operation, String.Empty);
                        if (RuleOperationCompleted != null)
                        {
                            bool hasError = args.FailedObjects.Count > 0 ? true : false;
                            String errorMessage = args.Message;
                            RuleOperationEventArgs argu = new RuleOperationEventArgs { RuleList = args.ApprovedObjects, Category = RuleCategory.None, OperationType = RuleOperations.DeleteRule, OldValue = null, HasError = hasError, ErrorMessage = errorMessage, FailedRuleList = args.FailedObjects };
                            RuleOperationCompleted(this, argu);
                        }
                        break;

                    case RuleOperations.EnableRule:
                        RuleCache.GetInstance().EnableDisableRule(args.ApprovedObjects, true);
                        if (RuleOperationCompleted != null)
                        {
                            bool hasError = args.FailedObjects.Count > 0 ? true : false;
                            String errorMessage = args.Message;
                            RuleOperationEventArgs argu = new RuleOperationEventArgs { RuleList = args.ApprovedObjects, Category = RuleCategory.None, OperationType = RuleOperations.EnableRule, OldValue = null, HasError = hasError, ErrorMessage = errorMessage, FailedRuleList = args.FailedObjects };
                            RuleOperationCompleted(this, argu);

                        }
                        break;
                    case RuleOperations.DisableRule:
                        RuleCache.GetInstance().EnableDisableRule(args.ApprovedObjects, false);
                        if (RuleOperationCompleted != null)
                        {
                            bool hasError = args.FailedObjects.Count > 0 ? true : false;
                            String errorMessage = args.Message;
                            RuleOperationEventArgs argu = new RuleOperationEventArgs { RuleList = args.ApprovedObjects, Category = RuleCategory.None, OperationType = RuleOperations.DisableRule, OldValue = null, HasError = hasError, ErrorMessage = errorMessage, FailedRuleList = args.FailedObjects };
                            RuleOperationCompleted(this, argu);
                        }
                        break;
                    case RuleOperations.RenameRule:
                        SaveUpdateRule(args.ApprovedObjects);
                        if (RuleOperationCompleted != null)
                        {
                            bool hasError = args.FailedObjects.Count > 0 ? true : false;
                            String errorMessage = args.Message;
                            RuleOperationEventArgs argu = new RuleOperationEventArgs { RuleList = args.ApprovedObjects, Category = RuleCategory.None, OperationType = RuleOperations.RenameRule, OldValue = null, HasError = hasError, ErrorMessage = errorMessage, FailedRuleList = args.FailedObjects };
                            RuleOperationCompleted(this, argu);
                        }
                        break;
                    case RuleOperations.Build:
                        if (RuleOperationCompleted != null)
                        {
                            bool hasError = args.FailedObjects.Count > 0 ? true : false;
                            if (hasError)
                                RuleCache.GetInstance().EnableDisableRule(args.FailedObjects, false);
                            String errorMessage = args.Message;
                            RuleOperationEventArgs argu = new RuleOperationEventArgs { RuleList = args.ApprovedObjects, Category = RuleCategory.None, OperationType = RuleOperations.Build, OldValue = null, HasError = hasError, ErrorMessage = errorMessage, FailedRuleList = args.FailedObjects };
                            RuleOperationCompleted(this, argu);
                        }
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
        /// Initialize status of all category to false before loading.
        /// </summary>
        private void InitializeRuleLoadStatus()
        {
            try
            {
                lock (loadStatusLocker)
                {
                    allCategoryRuleLoadStatus.Clear();

                    foreach (RuleCategory cate in Enum.GetValues(typeof(RuleCategory)))
                    {
                        if (cate != RuleCategory.None)
                        {
                            allCategoryRuleLoadStatus.Add(cate, false);
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

        ProcessManager<RuleBase> _ruleBaseProcessHandler;




        /// <summary>
        /// Binds events to handlers of Rule category.
        /// </summary>
        private void BindRuleHandlerEvents()
        {
            try
            {
                foreach (RuleCategory category in Enum.GetValues(typeof(RuleCategory)))
                {
                    if (category != RuleCategory.None)
                    {
                        IRuleHandler handler = RuleFactoryHandler.GetRuleHandlerFor(category);
                        handler.RuleLoaded += handler_RuleLoaded;
                        handler.RuleOperation += handler_RuleOperation;
                        //handler.ExportComplete += handler_ExportComplete;
                    }
                }
                GroupDataHandler.GetInstance().GroupOperationResponse += RuleManager_GroupOperationResponse;
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

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="args"></param>
        //void Handler_ExportComplete(RuleBase args, bool isPassed, String failedMessage)
        //{
        //    try
        //    {

        //        if (isPassed)
        //            _ruleBaseProcessHandler.SetApproved(args);
        //        else
        //            _ruleBaseProcessHandler.SetFailed(args,failedMessage);


        //        //bool isExportComplete = true;

        //        //if (ruleOperationStatus.ContainsKey(args))
        //        //    ruleOperationStatus[args] = true;

        //        //foreach (RuleBase ruleId in ruleOperationStatus.Keys)
        //        //{
        //        //    if (!ruleOperationStatus[ruleId])
        //        //    {
        //        //        isExportComplete = false;
        //        //        break;
        //        //    }
        //        //}
        //        //if (isExportComplete)
        //        //{
        //        //    List<RuleBase> ruleList = new List<RuleBase>();
        //        //    foreach (RuleBase ruleId in ruleOperationStatus.Keys)
        //        //    {
        //        //        ruleList.Add(GetRule(ruleId.RuleId));
        //        //    }
        //        //    RuleFactoryHandler.GetCommonRuleHandler().ExportRuleDef(ruleList, ComplianceCacheManager.GetImportExportPath(CachedData.CompanyID));
        //        //    ruleOperationStatus.Clear();
        //        //    _isOperationComplete = true;
        //        //    if (RuleOperationCompleted != null)
        //        //        RuleOperationCompleted(this,new RuleOperationEventArgs(ruleList,RuleCategory.None,RuleOperations.ExportRule,null));
        //        //}
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
        //    }

        //}


        object loadStatusLocker = new object();
        Dictionary<RuleCategory, Boolean> allCategoryRuleLoadStatus = new Dictionary<RuleCategory, bool>();

        /// <summary>
        /// Event raised by handlers after loading all rules.
        /// Once all category rules it updates rule cache with notification settings.
        /// Rules which are not having notification settings in DB are saved in DB with defaults settings.
        /// And raises all rule loaded event on which tree loads rule.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args">Contains list of rules and category</param>
        void handler_RuleLoaded(object sender, RuleLoadEventArgs args)
        {
            try
            {
                RuleCache.GetInstance().AddOrUpdateRules(args.RuleList, false);

                bool ruleLoadPending = false;
                lock (loadStatusLocker)
                {
                    if (allCategoryRuleLoadStatus.ContainsKey(args.category))
                        allCategoryRuleLoadStatus[args.category] = true;

                    foreach (RuleCategory category in allCategoryRuleLoadStatus.Keys)
                    {
                        if (allCategoryRuleLoadStatus[category] == false)
                        {
                            ruleLoadPending = true;
                            break;
                        }
                    }
                }


                if (!ruleLoadPending)
                {
                    //Load notification settings and update to cache
                    //Updates Group id for rules from DB
                    RuleCache.GetInstance().UpdateGroupId(GroupDataHandler.GetInstance().GetGroupIdForRules());

                    Dictionary<String, NotificationSetting> notificationList = RuleFactoryHandler.GetCommonRuleHandler().GetNotificationSettings();

                    RuleCache.GetInstance().AddOrUpdateNotificationSettings(notificationList);

                    List<RuleBase> newNoti = RuleCache.GetInstance().UpdateDefaultNotificationIfNotExist(new NotificationSetting());

                    if (newNoti.Count > 0)
                    {
                        SaveUpdateRule(newNoti);
                        Dictionary<String, NotificationSetting> updatedNotificationList = RuleFactoryHandler.GetCommonRuleHandler().GetNotificationSettings();
                        RuleCache.GetInstance().AddOrUpdateNotificationSettings(updatedNotificationList);
                    }
                    //Gets list of group from DB
                    List<GroupBase> groupList = GroupDataHandler.GetInstance().GetGroupList();
                    //Gets list of rules in group from rule cache
                    RuleCache.GetInstance().GetGroupWiseRuleList(ref groupList);
                    //Updates group cache for all groups laoded
                    GroupCache.GetInstance().AddUpdateGroup(groupList);

                    foreach (RuleCategory catComp in Enum.GetValues(typeof(RuleCategory)))
                    {
                        List<RuleBase> rules = RuleCache.GetInstance().GetRulesCategoryWise(catComp);
                        RuleLoadEventArgs argComplete = new RuleLoadEventArgs(rules, catComp, groupList);
                        if (AllRulesLoaded != null)
                            AllRulesLoaded(this, argComplete);
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

            //CommonRuleHandler handler = RuleFactoryHandler.GetCommonRuleHandler();
            //handler.GetNotificationSettings(ref args);

        }

        /// <summary>
        /// After completion of rule operation updates cache  and DB 
        /// Raises Rule operation completed event to update controls on UI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void handler_RuleOperation(object sender, RuleOperationEventArgs args)
        {
            try
            {



                RuleOperations ruleOperation = args.OperationType;
                List<RuleBase> updationFromDifferentClient = new List<RuleBase>();
                switch (ruleOperation)
                {
                    case RuleOperations.AddRule:
                        foreach (RuleBase rule in args.RuleList)
                        {
                            if (_ruleBaseProcessHandler.ExistsInUnapproved(rule))
                            {
                                RuleBase oldRule = _ruleBaseProcessHandler.GetUnApprovedObj(rule);
                                rule.GroupId = oldRule.GroupId;
                                UpdateProcessManager(rule, args.HasError, args.ErrorMessage);
                            }
                            else
                            {
                                if (!args.HasError)
                                    updationFromDifferentClient.Add(rule);
                            }
                        }
                        break;
                    case RuleOperations.EnableRule:
                    case RuleOperations.DisableRule:
                    case RuleOperations.Build:

                        foreach (RuleBase rule in args.RuleList)
                        {
                            if (_ruleBaseProcessHandler.ExistsInUnapproved(rule))
                            {
                                UpdateProcessManager(rule, args.HasError, args.ErrorMessage);
                            }
                            else
                            {
                                if (args.HasError)
                                    rule.Enabled = false;
                                updationFromDifferentClient.Add(rule);
                            }
                        }
                        break;
                    case RuleOperations.DeleteRule:
                    case RuleOperations.ExportRule:

                        foreach (RuleBase rule in args.RuleList)
                        {
                            if (_ruleBaseProcessHandler.ExistsInUnapproved(rule))
                            {
                                RuleBase oldRule = _ruleBaseProcessHandler.GetUnApprovedObj(rule);
                                rule.GroupId = oldRule.GroupId;
                                rule.Notification = oldRule.Notification;
                                UpdateProcessManager(rule, args.HasError, args.ErrorMessage);
                            }
                            else
                            {
                                if (!args.HasError)
                                    updationFromDifferentClient.Add(rule);
                            }
                        }
                        break;
                    case RuleOperations.RenameRule:
                        List<RuleBase> ruleList = RuleCache.GetInstance().RenameRule(args.RuleList, args.OldValue);
                        RuleFactoryHandler.GetCommonRuleHandler().RenameRuleInAlerts(args.OldValue, args.RuleList);
                        GroupCache.GetInstance().AddUpdateRuleInGroup(ruleList, ruleOperation, args.OldValue);
                        foreach (RuleBase rule in ruleList)
                        {
                            if (_ruleBaseProcessHandler.ExistsInUnapproved(rule))
                            {
                                UpdateProcessManager(rule, args.HasError, args.ErrorMessage);
                                //Raises even to update group UI when rule is rename.
                                if (RenameRuleOperationCompleted != null)
                                    RenameRuleOperationCompleted(this, new RuleOperationEventArgs { RuleList = ruleList, Category = RuleCategory.None, OperationType = RuleOperations.RenameRule, OldValue = args.OldValue });
                            }
                            else
                            {
                                if (!args.HasError)
                                    updationFromDifferentClient.Add(rule);
                            }
                        }
                        break;
                    case RuleOperations.ImportRule:
                        foreach (RuleBase rule in args.RuleList)
                        {
                            if (_ruleBaseProcessHandler.ExistsInUnapproved(rule))
                            {
                                RuleBase tempRule = _ruleBaseProcessHandler.GetUnApprovedObj(rule);
                                rule.GroupId = tempRule.GroupId;
                                rule.Notification = tempRule.Notification;
                                UpdateProcessManager(rule, args.HasError, args.ErrorMessage);
                            }
                            else
                            {
                                if (!args.HasError)
                                    updationFromDifferentClient.Add(rule);
                            }
                        }
                        break;

                }


                if (updationFromDifferentClient != null && updationFromDifferentClient.Count > 0)
                {
                    UpdateCacheAndReloadRules(updationFromDifferentClient, ruleOperation, args.OldValue);
                }

                //if (RuleOperationCompleted != null)
                //    RuleOperationCompleted(this, args);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }

            }
        }

        /// <summary>
        /// Upates Rule cache and reloads rules in tree when operation is done by different user.
        /// </summary>
        /// <param name="updationFromDifferentClient"></param>
        /// <param name="operation"></param>
        private void UpdateCacheAndReloadRules(List<RuleBase> updationFromDifferentClient, RuleOperations operation, string oldValue)
        {
            try
            {
                if (operation != RuleOperations.ExportRule)
                {
                    RuleOperationEventArgs args = new RuleOperationEventArgs();
                    args.IsOperationFromDifferentClient = true;
                    args.OperationType = operation;
                    args.OldValue = oldValue;
                    Dictionary<String, NotificationSetting> settings;
                    Dictionary<String, String> groupId;
                    while (true)
                    {
                        groupId = GroupDataHandler.GetInstance().GetGroupIdForRules(updationFromDifferentClient);
                        settings = RuleFactoryHandler.GetCommonRuleHandler().GetNotificationSettings(updationFromDifferentClient);
                        if (settings != null && updationFromDifferentClient.Count == settings.Count && groupId != null && groupId.Count == updationFromDifferentClient.Count)
                            break;

                        Thread.Sleep(500);
                    }

                    foreach (RuleBase rule in updationFromDifferentClient)
                    {
                        if (settings.ContainsKey(rule.RuleId))
                            rule.Notification = settings[rule.RuleId];
                        if (groupId.ContainsKey(rule.RuleId))
                            rule.GroupId = groupId[rule.RuleId];

                        //Fill args events                               
                        args.RuleList.Add(rule);
                    }
                    RuleCache.GetInstance().AddOrUpdateRules(updationFromDifferentClient);

                    if (RuleOperationFromDifferentClient != null)
                        RuleOperationFromDifferentClient(this, args);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Sets approved and failed list in Process queue.
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="hasError"></param>
        /// <param name="errorMessage"></param>
        private void UpdateProcessManager(RuleBase rule, bool hasError, String errorMessage)
        {
            try
            {

                if (hasError)
                    _ruleBaseProcessHandler.SetFailed(rule, errorMessage);
                else
                    _ruleBaseProcessHandler.SetApproved(rule);

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Load all rules from user defined and custom rule handlers.
        /// </summary>
        internal void LoadRules()
        {
            try
            {
                InitializeRuleLoadStatus();
                RuleCache.GetInstance().Clear();
                GroupCache.GetInstance().Clear();
                if (LoadGroups != null)
                    LoadGroups(this, new RuleLoadEventArgs { GroupList = null, RuleList = null });

                foreach (RuleCategory category in Enum.GetValues(typeof(RuleCategory)))
                {
                    if (category != RuleCategory.None)
                    {
                        IRuleHandler handler = RuleFactoryHandler.GetRuleHandlerFor(category);
                        handler.GetAllRulesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Load rules from hadler of category.
        /// </summary>
        /// <param name="category"></param>
        //internal void LoadRules(RuleCategory category)
        //{
        //    try
        //    {
        //        RuleCache.GetInstance().Clear();
        //        if (category != RuleCategory.None)
        //        {
        //            IRuleHandler handler = RuleFactoryHandler.GetRuleHandlerFor(category);
        //            handler.GetAllRulesAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //}


        #region IDisposable Members

        /// <summary>
        /// Unbinds all events while disposing.
        /// </summary>
        public void Dispose()
        {

            try
            {
                foreach (RuleCategory category in Enum.GetValues(typeof(RuleCategory)))
                {
                    if (category != RuleCategory.None)
                    {
                        IRuleHandler handler = RuleFactoryHandler.GetRuleHandlerFor(category);
                        handler.RuleLoaded -= handler_RuleLoaded;
                        handler.RuleOperation -= handler_RuleOperation;
                        handler.Dispose();
                    }
                }
                RuleCache.GetInstance().Clear();
                RuleFactoryHandler.GetCommonRuleHandler().DisposeInstance();
                RuleCache.GetInstance().DisposeInstance();
                _ruleBaseProcessHandler.ProcessComplete -= _ruleBaseProcessHandler_ProcessComplete;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        /// <summary>
        /// Get rule for rule Id from cache.
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        internal RuleBase GetRule(string ruleId)
        {
            try
            {
                return RuleCache.GetInstance().GetRule(ruleId);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Loads Notification frequency for drop down menu.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, string> LoadNotificationFrequency()
        {
            try
            {
                CommonRuleHandler handler = RuleFactoryHandler.GetCommonRuleHandler();
                return handler.GetAllNotificationFrequency();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Save/Updates rule in DB.
        /// </summary>
        /// <param name="ruleList"></param>
        internal void SaveUpdateRule(List<RuleBase> ruleList)
        {
            try
            {
                RuleFactoryHandler.GetCommonRuleHandler().SaveUpdateRule(ruleList);
                RuleCache.GetInstance().AddOrUpdateRules(ruleList);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Operates on rule using handler of rule category.
        /// </summary>
        /// <param name="rulePackage"></param>
        /// <param name="ruleCategory"></param>
        /// <param name="ruleName"></param>
        /// <param name="ruleOperations"></param>
        /// <param name="ruleId"></param>
        /// <param name="oldRuleName">Used only for rename operation in other cases it is null</param>
        /// <param name="tag"></param>
        internal void OperateOnRule(RulePackage rulePackage, RuleCategory ruleCategory, string ruleName, RuleOperations ruleOperations, String ruleId, String oldRuleName, Object tag)
        {
            try
            {
                if (_ruleBaseProcessHandler.IsProcessComplete())
                {
                    List<RuleBase> ruleList = new List<RuleBase>();
                    if (ruleName != String.Empty)
                    {
                        RuleBase rule = null;
                        if (!String.IsNullOrEmpty(ruleId))
                        {
                            rule = GetRule(ruleId);
                            if (ruleOperations == RuleOperations.RenameRule)
                                rule.RuleName = ruleName;
                        }
                        else
                        {
                            if (ruleCategory.Equals(RuleCategory.CustomRule))
                                rule = new CustomRuleDefinition();
                            else if (ruleCategory.Equals(RuleCategory.UserDefined))
                                rule = new UserDefinedRule();

                            rule.Package = rulePackage;
                            rule.Category = ruleCategory;
                            rule.RuleName = ruleName;
                            rule.RuleId = ruleId;
                            rule.GroupId = "-1";
                        }
                        ruleList.Add(rule);
                    }
                    else if (ruleCategory != RuleCategory.None)
                    {
                        ruleList.AddRange(RuleCache.GetInstance().GetRulesPackageCategoryWise(ruleCategory, rulePackage));
                    }
                    else if (rulePackage != RulePackage.None)
                    {
                        foreach (RuleCategory category in Enum.GetValues(typeof(RuleCategory)))
                        {
                            if (category != RuleCategory.None)
                            {
                                ruleList.AddRange(RuleCache.GetInstance().GetRulesPackageCategoryWise(category, rulePackage));
                            }
                        }
                    }
                    _ruleBaseProcessHandler.SetProcessQueue(ruleList, ruleOperations.ToString());
                    List<RuleBase> clonedRuleList = ruleList.Select(rule => rule.DeepClone()).ToList();

                    foreach (RuleBase rule in clonedRuleList)
                    {
                        IRuleHandler handler = RuleFactoryHandler.GetRuleHandlerFor(rule.Category);
                        handler.OperationOnRule(rule.Package, rule.RuleName, ruleOperations, rule.RuleId, oldRuleName, tag);
                    }

                }


            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Contains 3 parameters
        /// package name, category,ruleName
        /// If selected node is package node then category and rule name is none and empty respectively
        /// else is category node then rule name is empty
        /// else if rule node then all parameters contains value.
        /// </summary>
        /// <param name="rulePackage"></param>
        /// <param name="ruleCategory"></param>
        /// <param name="p"></param>
        internal void ExportRule(RulePackage rulePackage, RuleCategory ruleCategory, string ruleId)
        {
            try
            {


                String importExportPath = ComplianceCacheManager.GetImportExportPath();
                if (!String.IsNullOrEmpty(ruleId))
                {
                    List<RuleBase> ruleList = new List<RuleBase>();
                    RuleBase rule = GetRule(ruleId);
                    ruleList.Add(rule);

                    _ruleBaseProcessHandler.SetProcessQueue(ruleList, RuleOperations.ExportRule.ToString());
                    IRuleHandler handler = RuleFactoryHandler.GetRuleHandlerFor(ruleCategory);
                    handler.ExportRule(ruleList, importExportPath);

                }
                else if (ruleCategory != RuleCategory.None)
                {
                    List<RuleBase> categoryRuleList = RuleCache.GetInstance().GetRulesPackageCategoryWise(ruleCategory, rulePackage);
                    _ruleBaseProcessHandler.SetProcessQueue(categoryRuleList, RuleOperations.ExportRule.ToString());
                    IRuleHandler handler = RuleFactoryHandler.GetRuleHandlerFor(ruleCategory);
                    handler.ExportRule(categoryRuleList, importExportPath);
                }
                else if (rulePackage != RulePackage.None)
                {
                    List<RuleBase> packageRuleList = new List<RuleBase>();
                    foreach (RuleCategory category in Enum.GetValues(typeof(RuleCategory)))
                    {
                        if (category != RuleCategory.None)
                        {
                            List<RuleBase> allRuleList = RuleCache.GetInstance().GetRulesPackageCategoryWise(category, rulePackage);
                            packageRuleList.AddRange(allRuleList);
                            IRuleHandler handler = RuleFactoryHandler.GetRuleHandlerFor(category);
                            handler.ExportRule(allRuleList, importExportPath);
                        }
                    }
                    _ruleBaseProcessHandler.SetProcessQueue(packageRuleList, RuleOperations.ExportRule.ToString());
                }

                //IRuleHandler handler = RuleFactoryHandler.GetRuleHandlerFor(ruleCategory);
                //handler.ExportRule(rulePackage,rule.RuleName,ruleId,importExportPath);

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sending request to handlers for importing rules.
        /// </summary>
        /// <param name="importDefCache"></param>
        internal void ImportRules(Dictionary<string, ImportDefinition> importDefCache)
        {
            try
            {
                List<RuleBase> ruleList = importDefCache.Values.ToList<RuleBase>();
                if (ruleList.Count > 0)
                {
                    _ruleBaseProcessHandler.SetProcessQueue(ruleList, RuleOperations.ImportRule.ToString());
                    foreach (string key in importDefCache.Keys)
                    {
                        RuleFactoryHandler.GetRuleHandlerFor(importDefCache[key].Category).ImportRule(importDefCache[key]);
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
        /// Updates Group cache, rule cache, and DB after every group operation.
        /// </summary>
        /// <param name="unassignedRuleList">List of which are unassigned</param>
        /// <param name="groupList"></param>
        /// <param name="renamedDict"></param>
        /// <param name="addedDict"></param>
        /// <param name="deletedList"></param>
        /// <returns></returns>
        internal bool SaveGroupSettings(List<String> unassignedRuleList, Dictionary<string, GroupBase> groupList, Dictionary<string, string> renamedDict, Dictionary<string, GroupBase> addedDict, List<string> deletedList)
        {
            try
            {
                GroupCache.GetInstance().AddUpdateGroup(new List<GroupBase>(addedDict.Values));
                GroupCache.GetInstance().RenameGroup(renamedDict);
                GroupCache.GetInstance().AddUpdateGroup(new List<GroupBase>(groupList.Values));
                GroupCache.GetInstance().DeleteGroup(deletedList);
                Dictionary<String, String> groupId = new Dictionary<string, string>();
                foreach (String id in unassignedRuleList)
                {
                    if (groupId.ContainsKey(id))
                        groupId[id] = "-1";
                    else
                        groupId.Add(id, "-1");
                }
                foreach (String key in groupList.Keys)
                {
                    foreach (RuleBase rule in groupList[key].RuleList)
                    {
                        if (groupId.ContainsKey(rule.RuleId))
                            groupId[rule.RuleId] = key;
                        else
                            groupId.Add(rule.RuleId, key);
                    }

                }
                RuleCache.GetInstance().UpdateGroupId(groupId);
                GroupDataHandler.GetInstance().AddUpdateGroupIdInRules(groupId);
                GroupDataHandler.GetInstance().AddUpdateGroup(new List<GroupBase>(groupList.Values));
                GroupDataHandler.GetInstance().DeleteGroup(deletedList);
                GroupDataHandler.GetInstance().PublishSavedSettings();
                return true;
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
                return false;
            }
        }

        /// <summary>
        /// Update group UI if updated by other user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RuleManager_GroupOperationResponse(object sender, GroupOperationsEventArgs e)
        {
            try
            {
                RuleCache.GetInstance().UpdateGroupId(GroupDataHandler.GetInstance().GetGroupIdForRules());
                List<GroupBase> groupList = GroupDataHandler.GetInstance().GetGroupList();
                RuleCache.GetInstance().GetGroupWiseRuleList(ref groupList);
                GroupCache.GetInstance().AddUpdateGroup(groupList);

                List<RuleBase> ruleList = new List<RuleBase>();
                foreach (RuleCategory cat in Enum.GetValues(typeof(RuleCategory)))
                {
                    if (cat != RuleCategory.None)
                        ruleList.AddRange(RuleCache.GetInstance().GetRulesPackageCategoryWise(cat, RulePackage.PostTrade));
                }
                if (LoadGroups != null)
                    LoadGroups(this, new RuleLoadEventArgs { GroupList = groupList, RuleList = ruleList });
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
        /// Get group for Group is from group cache.
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        internal GroupBase GetGroupForId(string groupId)
        {
            try
            {
                return GroupCache.GetInstance().GetGroupForId(groupId);
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
        /// Unwind group events
        /// clears and disposes cache.
        /// </summary>
        internal void DisposeGroup()
        {
            try
            {
                GroupDataHandler.GetInstance().GroupOperationResponse -= RuleManager_GroupOperationResponse;
                GroupCache.GetInstance().Clear();
                GroupCache.GetInstance().Dispose();
                //GroupDataHandler.GetInstance().Dispose();
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