using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.EventArguments;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Compliance.Constants;
using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Delegates;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.Compliance.EventArguments;
using Prana.BusinessObjects.Compliance.Interfaces;
using Prana.CommonDataCache;
using Prana.ComplianceEngine.RuleDefinition.Helper;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Prana.ComplianceEngine.RuleDefinition.DAL
{
    internal class UserDefinedRuleHandler : IRuleHandler
    {
        private static UserDefinedRuleHandler _userDefinedRuleHandler;
        private static Object _userDefinedRuleLockerObject = new Object();

        String _importExportCacheKey = "UserDefinedImportExportCommunication";
        String _importExportExchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_OtherDataExchange);


        String _ruleBuildRequestExchange = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_RuleRequestExchange);
        String _ruleBuildResponseExchange = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_RuleResponseExchange);


        String _cacheKey = "Build";
        CompanyUser _companyUser = CachedDataManager.GetInstance.LoggedInUser;
        bool _isPowerUser;
        bool _isInitialized = true;
        #region singleton Instance

        public static IRuleHandler GetInstance()
        {
            lock (_userDefinedRuleLockerObject)
            {
                if (_userDefinedRuleHandler == null)
                    _userDefinedRuleHandler = new UserDefinedRuleHandler();
                return _userDefinedRuleHandler;
            }
            //return singleton instance
        }
        private UserDefinedRuleHandler()
        {
            InitializeAmqp();
            _isPowerUser = ComplianceCacheManager.GetPowerUserCheck(_companyUser.CompanyUserID);
        }

        #endregion



        #region IRuleHandler Members


        /// <summary>
        /// Event is raised as all rules are loaded from Rule Engine Mediator.
        /// </summary>

        public event RuleLoadedHandler RuleLoaded;

        /// <summary>
        /// Event is raised when operation complete recieved from Rule Mediator.
        /// </summary>
        public event RuleOperationHandler RuleOperation;

        /// <summary>
        /// Get all rules from drools through Rule Mediator.
        /// </summary>
        public void GetAllRulesAsync()
        {
            try
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += worker_DoWork;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.RunWorkerAsync();
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

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (!SendUserDefinedRuleRequest())
                {
                    Thread.Sleep(1000);
                }

                e.Result = "Initialization request sent";
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
        /// Send request for rule operations to drools through rule mediator.
        /// </summary>
        /// <param name="rulePackage"></param>
        /// <param name="ruleName"></param>
        /// <param name="ruleOperations">Enable, Disable, Rename, Create, Delete</param>
        /// <param name="ruleId"></param>
        /// <param name="oldRuleName">used only in case of rename, null in all other operations</param>
        /// <param name="tag"></param>
        public void OperationOnRule(RulePackage rulePackage, string ruleName, RuleOperations ruleOperations, String ruleId, String oldRuleName, Object tag)
        {
            try
            {
                Dictionary<String, String> operation = new Dictionary<string, string>();
                operation.Add("ActionUser", Convert.ToString(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID));
                operation.Add("Method", "OperationOnRuleUDRH");
                operation.Add("directoryPath", ComplianceCacheManager.GetImportExportPath());
                operation.Add("ruleCategory", RuleCategory.UserDefined.ToString());
                operation.Add("operationType", ruleOperations.ToString());
                operation.Add("packageName", rulePackage.ToString());
                operation.Add("ruleName", ruleName);
                operation.Add("ruleId", ruleId);
                operation.Add("oldRuleName", oldRuleName);
                operation.Add("tag", tag != null ? tag.ToString() : "");
                AmqpHelper.SendObject(operation, _cacheKey, null);
                RuleHandlerHelper.UserComplianceActionLogger(operation);


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


        public void ExportRule(List<RuleBase> ruleList, string importExportPath)
        {
            try
            {
                foreach (RuleBase rule in ruleList)
                {
                    Dictionary<String, String> export = new Dictionary<string, string>();
                    export.Add("ActionUser", Convert.ToString(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID));
                    export.Add("Method", "ExportRuleUDRH");
                    export.Add("directoryPath", importExportPath);
                    export.Add("packageName", rule.Package.ToString());
                    export.Add("ruleName", rule.RuleName);
                    export.Add("ruleId", rule.RuleId);
                    export.Add("ruleCategory", rule.Category.ToString());
                    export.Add("operationType", "ExportRule");
                    AmqpHelper.SendObject(export, _importExportCacheKey, "ExportUserDefinedRule");
                    RuleHandlerHelper.UserComplianceActionLogger(export);
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

        public void ImportRule(ImportDefinition importDefinition)
        {
            try
            {
                Dictionary<String, String> requestDict = new Dictionary<string, string>();
                requestDict.Add("ActionUser", Convert.ToString(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID));
                requestDict.Add("Method", "ImportRuleUDRH");
                requestDict.Add("packageName", importDefinition.Package.ToString());
                requestDict.Add("directoryPath", importDefinition.DirectoryPath);
                requestDict.Add("oldRuleName", importDefinition.OldRuleName);
                requestDict.Add("newRuleName", importDefinition.RuleName);
                requestDict.Add("ruleName", importDefinition.RuleName);
                requestDict.Add("ruleCategory", importDefinition.Category.ToString());
                requestDict.Add("operationType", "ImportRule");
                AmqpHelper.SendObject(requestDict, _importExportCacheKey, "ImportUserDefinedRule");
                RuleHandlerHelper.UserComplianceActionLogger(requestDict);
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

        //public event RuleImportExportHandler ExportComplete;

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region AmqpInitialization

        private void InitializeAmqp()
        {
            try
            {
                AmqpHelper.Started += new ListenerStarted(AmqpHelper_Started);
                AmqpHelper.Stopped += new ListenerStopped(AmqpHelper_Stopped);
                AmqpHelper.InitializeSender(_cacheKey, _ruleBuildRequestExchange, MediaType.Exchange_Fanout);


                AmqpHelper.InitializeListenerForExchange(_ruleBuildResponseExchange, MediaType.Exchange_Fanout, null);

                AmqpHelper.InitializeSender(_importExportCacheKey, _importExportExchangeName, MediaType.Exchange_Direct);

                List<String> exportRoutingKey = new List<string>();
                exportRoutingKey.Add("UserDefinedExportComplete");
                AmqpHelper.InitializeListenerForExchange(_importExportExchangeName, MediaType.Exchange_Direct, exportRoutingKey);
                List<String> importRoutingKey = new List<string>();
                importRoutingKey.Add("UserDefinedImportComplete");
                AmqpHelper.InitializeListenerForExchange(_importExportExchangeName, MediaType.Exchange_Direct, importRoutingKey);
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

        private void AmqpHelper_Stopped(Object sender, ListenerStoppedEventArguments e)
        {
            try
            {
                if (e.AmqpReceiver.MediaName == _ruleBuildRequestExchange || e.AmqpReceiver.MediaName == _importExportExchangeName)
                    e.AmqpReceiver.AmqpDataReceived -= new DataReceived(amqpReceiver_AmqpDataReceived);
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



        private void AmqpHelper_Started(Object sender, ListenerStartedEventArguments e)
        {
            try
            {
                if (e.AmqpReceiver.MediaName == _ruleBuildResponseExchange || e.AmqpReceiver.MediaName == _importExportExchangeName)
                    e.AmqpReceiver.AmqpDataReceived += new DataReceived(amqpReceiver_AmqpDataReceived);
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

        private void amqpReceiver_AmqpDataReceived(Object sender, DataReceivedEventArguments e)
        {

            try
            {
                if (e.RoutingKey == "Fanout")
                {
                    RuleOperationResponseRecieved(e.DsReceived);
                }
                else if (e.RoutingKey == "_UserDefinedExportComplete")
                {
                    String ruleId = e.DsReceived.Tables[0].Rows[0]["ruleId"].ToString();
                    RulePackage packageName = (RulePackage)Enum.Parse(typeof(RulePackage), e.DsReceived.Tables[0].Rows[0]["packageName"].ToString(), true);
                    //RuleCategory ruleCategory = (RuleCategory)Enum.Parse(typeof(RuleCategory), e.DsReceived.Tables[0].Rows[0]["ruleCategory"].ToString(), true);

                    List<RuleBase> ruleList = new List<RuleBase>();
                    RuleBase rule = new UserDefinedRule();
                    rule.RuleId = ruleId;
                    rule.Package = packageName;
                    rule.Category = RuleCategory.UserDefined;
                    rule.RuleName = e.DsReceived.Tables[0].Rows[0]["ruleName"].ToString();
                    ruleList.Add(rule);
                    if (RuleOperation != null)
                    {
                        RuleOperation(this, new RuleOperationEventArgs { RuleList = ruleList, Category = RuleCategory.UserDefined, OperationType = RuleOperations.ExportRule, OldValue = null, HasError = false, ErrorMessage = "" });
                        if (!string.IsNullOrEmpty(e.DsReceived.Tables[0].Rows[0]["filePath"].ToString()))//file path is null if jboss server is not selected
                            RuleHandlerHelper.UserComplianceActionLoggerResponse(rule, "Export Rule", RuleNavigatorConstants.SUCCESSFUL, Convert.ToInt32(e.DsReceived.Tables[0].Rows[0]["ActionUser"]));
                        else
                            RuleHandlerHelper.UserComplianceActionLoggerResponse(rule, "Export Rule", RuleNavigatorConstants.UNSUCCESSFUL, Convert.ToInt32(e.DsReceived.Tables[0].Rows[0]["ActionUser"]));
                    }
                }
                else if (e.RoutingKey == "_UserDefinedImportComplete")
                {
                    HandleAllRulesRecieved(e.DsReceived);
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





        #endregion

        /// <summary>
        /// Operation response received from rule mediator.
        /// All operation done except Import and Export
        /// </summary>
        /// <param name="dsReceived"></param>
        private void RuleOperationResponseRecieved(DataSet dsReceived)
        {


            try
            {
                if (dsReceived.Tables[0].Rows[0]["operationType"].ToString() == "LoadRules")
                {
                    if (!_isInitialized)
                    {
                        HandleAllRulesRecieved(dsReceived);
                        _isInitialized = true;
                    }

                }
                else
                {
                    RuleOperations operationType = (RuleOperations)Enum.Parse(typeof(RuleOperations), dsReceived.Tables[0].Rows[0]["operationType"].ToString(), true);
                    bool operationStatus = Convert.ToBoolean(dsReceived.Tables[0].Rows[0]["operationStatus"].ToString());
                    //If operation successfull Rule Id is sen to Rule manager to delete rule from cache and DB.
                    switch (operationType)
                    {
                        case RuleOperations.Build:
                            List<RuleBase> buildRuleList = new List<RuleBase>();
                            RuleBase buildRule = new UserDefinedRule();
                            buildRule.RuleId = dsReceived.Tables[0].Rows[0]["ruleId"].ToString();
                            buildRule.RuleName = dsReceived.Tables[0].Rows[0]["ruleName"].ToString();
                            buildRule.Package = (RulePackage)Enum.Parse(typeof(RulePackage), dsReceived.Tables[0].Rows[0]["packageName"].ToString(), true);
                            buildRule.Category = RuleCategory.UserDefined;
                            buildRule.RuleURL = GetUrl(dsReceived.Tables[0].Rows[0]["ruleId"].ToString());
                            buildRule.Enabled = Convert.ToBoolean(dsReceived.Tables[0].Rows[0]["enabled"].ToString());
                            buildRuleList.Add(buildRule);
                            if (operationStatus)
                            {
                                if (RuleOperation != null)
                                    RuleOperation(this, new RuleOperationEventArgs { RuleList = buildRuleList, Category = RuleCategory.UserDefined, OperationType = RuleOperations.Build, OldValue = null, HasError = false, ErrorMessage = buildRule.RuleName });
                                RuleHandlerHelper.UserComplianceActionLoggerResponse(buildRule, EnumHelper.GetDescription(operationType), RuleNavigatorConstants.SUCCESSFUL, Convert.ToInt32(dsReceived.Tables[0].Rows[0]["ActionUser"]));//to log user action on rules
                            }
                            else
                            {
                                if (RuleOperation != null)
                                    RuleOperation(this, new RuleOperationEventArgs { RuleList = buildRuleList, Category = RuleCategory.UserDefined, OperationType = RuleOperations.Build, OldValue = null, HasError = true, ErrorMessage = buildRule.RuleName });
                                RuleHandlerHelper.UserComplianceActionLoggerResponse(buildRule, EnumHelper.GetDescription(operationType), RuleNavigatorConstants.UNSUCCESSFUL, Convert.ToInt32(dsReceived.Tables[0].Rows[0]["ActionUser"]));//to log user action on rules
                            }
                            break;
                        case RuleOperations.DeleteRule:

                            List<RuleBase> deleteRuleList = new List<RuleBase>();
                            RuleBase deleteRule = new UserDefinedRule();
                            deleteRule.RuleId = dsReceived.Tables[0].Rows[0]["ruleId"].ToString();
                            deleteRule.RuleName = dsReceived.Tables[0].Rows[0]["ruleName"].ToString();
                            deleteRule.Package = (RulePackage)Enum.Parse(typeof(RulePackage), dsReceived.Tables[0].Rows[0]["packageName"].ToString(), true);
                            deleteRule.Category = RuleCategory.UserDefined;
                            deleteRule.RuleURL = GetUrl(dsReceived.Tables[0].Rows[0]["ruleId"].ToString());

                            deleteRuleList.Add(deleteRule);
                            if (operationStatus)
                            {
                                if (RuleOperation != null)
                                    RuleOperation(this, new RuleOperationEventArgs { RuleList = deleteRuleList, Category = RuleCategory.UserDefined, OperationType = RuleOperations.DeleteRule, OldValue = null, HasError = false, ErrorMessage = deleteRule.RuleName });
                                RuleHandlerHelper.UserComplianceActionLoggerResponse(deleteRule, EnumHelper.GetDescription(operationType), RuleNavigatorConstants.SUCCESSFUL, Convert.ToInt32(dsReceived.Tables[0].Rows[0]["ActionUser"]));//to log user action on rules
                            }
                            else
                            {
                                if (RuleOperation != null)
                                    RuleOperation(this, new RuleOperationEventArgs { RuleList = deleteRuleList, Category = RuleCategory.UserDefined, OperationType = RuleOperations.DeleteRule, OldValue = null, HasError = true, ErrorMessage = deleteRule.RuleName });
                                RuleHandlerHelper.UserComplianceActionLoggerResponse(deleteRule, EnumHelper.GetDescription(operationType), RuleNavigatorConstants.UNSUCCESSFUL, Convert.ToInt32(dsReceived.Tables[0].Rows[0]["ActionUser"]));
                            }

                            break;

                        //After rename rule Id and Rule URL are also changed for the rule. Old value parameter in Rule Operation Evnt args is null for other operations.
                        case RuleOperations.RenameRule:

                            // String clientName = "admin";


                            if (operationStatus)
                            {
                                List<RuleBase> ruleList = new List<RuleBase>();
                                RuleBase rule = new UserDefinedRule();
                                rule.RuleId = dsReceived.Tables[0].Rows[0]["ruleId"].ToString();
                                rule.RuleName = dsReceived.Tables[0].Rows[0]["ruleName"].ToString();
                                rule.Package = (RulePackage)Enum.Parse(typeof(RulePackage), dsReceived.Tables[0].Rows[0]["ruleType"].ToString(), true);
                                rule.Category = RuleCategory.UserDefined;
                                rule.RuleURL = GetUrl(dsReceived.Tables[0].Rows[0]["ruleId"].ToString());
                                //String param = "client=" + clientName + "&assetsUUIDs=" + dsReceived.Tables[0].Rows[0]["ruleId"].ToString() + "&isPowerUser=" + _isPowerUser;

                                // rule.RuleURL = UserDefinedRuleConstants.GUVNOR_STANDALONE_BASE_URL + param;
                                rule.Enabled = Convert.ToBoolean(dsReceived.Tables[0].Rows[0]["enabled"].ToString());
                                String oldRuleId = dsReceived.Tables[0].Rows[0]["oldRuleId"].ToString();
                                ruleList.Add(rule);
                                if (RuleOperation != null)
                                    RuleOperation(this, new RuleOperationEventArgs { RuleList = ruleList, Category = RuleCategory.UserDefined, OperationType = RuleOperations.RenameRule, OldValue = oldRuleId, HasError = false, ErrorMessage = rule.RuleName });
                                RuleHandlerHelper.UserComplianceActionLoggerResponse(rule, EnumHelper.GetDescription(operationType), RuleNavigatorConstants.SUCCESSFUL, Convert.ToInt32(dsReceived.Tables[0].Rows[0]["ActionUser"]));
                            }
                            else
                            {
                                RuleBase rule = new UserDefinedRule();
                                rule.RuleId = dsReceived.Tables[0].Rows[0]["ruleId"].ToString();
                                rule.RuleName = dsReceived.Tables[0].Rows[0]["ruleName"].ToString();
                                rule.Package = (RulePackage)Enum.Parse(typeof(RulePackage), dsReceived.Tables[0].Rows[0]["ruleType"].ToString(), true);
                                rule.Category = RuleCategory.UserDefined;
                                rule.RuleURL = GetUrl(dsReceived.Tables[0].Rows[0]["ruleId"].ToString());
                                RuleHandlerHelper.UserComplianceActionLoggerResponse(rule, EnumHelper.GetDescription(operationType), RuleNavigatorConstants.UNSUCCESSFUL, Convert.ToInt32(dsReceived.Tables[0].Rows[0]["ActionUser"]));
                            }

                            break;

                        //Rule Base only contains only rule Id of the rule is passed which is enabled or disabled.
                        case RuleOperations.EnableRule:


                            if (operationStatus)
                            {
                                List<RuleBase> ruleList = new List<RuleBase>();
                                RuleBase rule = new UserDefinedRule();
                                rule.RuleId = dsReceived.Tables[0].Rows[0]["ruleId"].ToString();
                                rule.RuleName = dsReceived.Tables[0].Rows[0]["ruleName"].ToString();
                                rule.Enabled = true;
                                rule.Category = RuleCategory.UserDefined;
                                rule.Package = (RulePackage)Enum.Parse(typeof(RulePackage), dsReceived.Tables[0].Rows[0]["packageName"].ToString(), true);
                                rule.RuleURL = GetUrl(dsReceived.Tables[0].Rows[0]["ruleId"].ToString());
                                ruleList.Add(rule);
                                if (RuleOperation != null)
                                    RuleOperation(this, new RuleOperationEventArgs { RuleList = ruleList, Category = RuleCategory.UserDefined, OperationType = RuleOperations.EnableRule, OldValue = null, HasError = false, ErrorMessage = rule.RuleName });
                                RuleHandlerHelper.UserComplianceActionLoggerResponse(rule, EnumHelper.GetDescription(operationType), RuleNavigatorConstants.SUCCESSFUL, Convert.ToInt32(dsReceived.Tables[0].Rows[0]["ActionUser"]));//to log user action on rules
                            }
                            else
                            {
                                List<RuleBase> ruleList = new List<RuleBase>();
                                RuleBase rule = new UserDefinedRule();
                                rule.RuleId = dsReceived.Tables[0].Rows[0]["ruleId"].ToString();
                                rule.RuleName = dsReceived.Tables[0].Rows[0]["ruleName"].ToString();
                                rule.Enabled = false;
                                rule.Category = RuleCategory.UserDefined;
                                rule.Package = (RulePackage)Enum.Parse(typeof(RulePackage), dsReceived.Tables[0].Rows[0]["packageName"].ToString(), true);
                                rule.RuleURL = GetUrl(dsReceived.Tables[0].Rows[0]["ruleId"].ToString());
                                ruleList.Add(rule);
                                if (RuleOperation != null)
                                    RuleOperation(this, new RuleOperationEventArgs { RuleList = ruleList, Category = RuleCategory.UserDefined, OperationType = RuleOperations.EnableRule, OldValue = null, HasError = true, ErrorMessage = rule.RuleName });
                                RuleHandlerHelper.UserComplianceActionLoggerResponse(rule, EnumHelper.GetDescription(operationType), RuleNavigatorConstants.UNSUCCESSFUL, Convert.ToInt32(dsReceived.Tables[0].Rows[0]["ActionUser"]));//to log user action on rules
                            }




                            break;

                        case RuleOperations.DisableRule:

                            if (operationStatus)
                            {
                                List<RuleBase> ruleList = new List<RuleBase>();
                                RuleBase rule = new UserDefinedRule();
                                rule.RuleId = dsReceived.Tables[0].Rows[0]["ruleId"].ToString();
                                rule.RuleName = dsReceived.Tables[0].Rows[0]["ruleName"].ToString();
                                rule.Enabled = false;
                                rule.Category = RuleCategory.UserDefined;
                                rule.Package = (RulePackage)Enum.Parse(typeof(RulePackage), dsReceived.Tables[0].Rows[0]["packageName"].ToString(), true);
                                rule.RuleURL = GetUrl(dsReceived.Tables[0].Rows[0]["ruleId"].ToString());
                                ruleList.Add(rule);
                                if (RuleOperation != null)
                                    RuleOperation(this, new RuleOperationEventArgs { RuleList = ruleList, Category = RuleCategory.UserDefined, OperationType = RuleOperations.DisableRule, OldValue = null, HasError = false, ErrorMessage = rule.RuleName });
                                RuleHandlerHelper.UserComplianceActionLoggerResponse(rule, EnumHelper.GetDescription(operationType), RuleNavigatorConstants.SUCCESSFUL, Convert.ToInt32(dsReceived.Tables[0].Rows[0]["ActionUser"]));//to log user action on ruless
                            }
                            else
                            {
                                List<RuleBase> ruleList = new List<RuleBase>();
                                RuleBase rule = new UserDefinedRule();
                                rule.RuleId = dsReceived.Tables[0].Rows[0]["ruleId"].ToString();
                                rule.RuleName = dsReceived.Tables[0].Rows[0]["ruleName"].ToString();
                                rule.Enabled = false;
                                rule.Category = RuleCategory.UserDefined;
                                rule.Package = (RulePackage)Enum.Parse(typeof(RulePackage), dsReceived.Tables[0].Rows[0]["packageName"].ToString(), true);
                                rule.RuleURL = GetUrl(dsReceived.Tables[0].Rows[0]["ruleId"].ToString());
                                ruleList.Add(rule);
                                if (RuleOperation != null)
                                    RuleOperation(this, new RuleOperationEventArgs { RuleList = ruleList, Category = RuleCategory.UserDefined, OperationType = RuleOperations.DisableRule, OldValue = null, HasError = true, ErrorMessage = rule.RuleName });
                                RuleHandlerHelper.UserComplianceActionLoggerResponse(rule, EnumHelper.GetDescription(operationType), RuleNavigatorConstants.UNSUCCESSFUL, Convert.ToInt32(dsReceived.Tables[0].Rows[0]["ActionUser"]));//to log user action on rules
                            }



                            break;

                        //In case of Load rules and create rule this method is called.
                        case RuleOperations.AddRule:
                            HandleAllRulesRecieved(dsReceived);//addrule logging defined inside this method
                            RuleBase addedRule = new UserDefinedRule();
                            addedRule.RuleName = dsReceived.Tables[0].Rows[0]["ruleName"].ToString();
                            addedRule.Category = RuleCategory.UserDefined;
                            addedRule.Package = (RulePackage)Enum.Parse(typeof(RulePackage), dsReceived.Tables[0].Rows[0]["ruleType"].ToString(), true);
                            if (operationStatus)
                            {
                                RuleHandlerHelper.UserComplianceActionLoggerResponse(addedRule, EnumHelper.GetDescription(operationType), RuleNavigatorConstants.SUCCESSFUL, Convert.ToInt32(dsReceived.Tables[0].Rows[0]["ActionUser"]));
                            }
                            else
                            {
                                RuleHandlerHelper.UserComplianceActionLoggerResponse(addedRule, EnumHelper.GetDescription(operationType), RuleNavigatorConstants.UNSUCCESSFUL, Convert.ToInt32(dsReceived.Tables[0].Rows[0]["ActionUser"]));
                            }
                            break;
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
        /// Send Load request to Rule mediator, to get all rules.
        /// </summary>
        /// <returns></returns>
        private Boolean SendUserDefinedRuleRequest()
        {
            try
            {
                _isInitialized = false;
                Dictionary<String, String> requestDict = new Dictionary<string, string>();
                requestDict.Add("operationType", "LoadRules");
                return AmqpHelper.SendObject(requestDict, _cacheKey, null);
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
        /// Convert dsRecieved to list of Rule Base. If operationType is Load RuleLoaded event is raised and if create then RuleOperation event is raised.
        /// </summary>
        /// <param name="dsReceived"></param>
        private void HandleAllRulesRecieved(DataSet dsReceived)
        {

            try
            {
                //String clientName = "admin";//Constants.CLIENT_NAME;
                //_isPowerUser = ComplianceCacheManager.GetPowerUserCheck(_companyUser.CompanyUserID);
                String operationType = "";
                List<RuleBase> ruleList = new List<RuleBase>();
                bool operationStatus = true;
                StringBuilder errorMessage = new StringBuilder();
                for (int i = 0; i < dsReceived.Tables.Count; i++)
                {
                    operationType = dsReceived.Tables[i].Rows[0]["operationType"].ToString();
                    RuleBase rule = null;
                    //rule = new UserDefinedRule(dsReceived.Tables[i].Rows[0]);

                    if (Convert.ToBoolean(dsReceived.Tables[i].Rows[0]["operationStatus"].ToString()))
                    {

                        // String param = "client=" + clientName + "&assetsUUIDs=" + dsReceived.Tables[i].Rows[0]["uuid"].ToString() + "&isPowerUser=" + _isPowerUser;
                        rule = new UserDefinedRule(dsReceived.Tables[i].Rows[0]);
                        rule.RuleURL = GetUrl(dsReceived.Tables[i].Rows[0]["uuid"].ToString());
                        ruleList.Add(rule);
                    }
                    else
                    {
                        operationStatus = false;
                        if (rule != null)
                        {
                            errorMessage.AppendLine(rule.RuleName);
                        }
                        //break;
                    }
                }
                //WriteAllRuleHtmlFile(ruleList);
                //_isInitialised = true;

                if (operationType.Equals("LoadRules"))
                {
                    if (RuleLoaded != null)
                        RuleLoaded(this, new RuleLoadEventArgs { RuleList = ruleList, category = RuleCategory.UserDefined, HasError = operationStatus, ErrorMessage = errorMessage.ToString() });
                    StringBuilder log_message = new StringBuilder();
                    log_message.Append("UserID : " + Convert.ToString(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID));
                    log_message.Append("  UserName : " + Convert.ToString(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.ShortName));
                    log_message.Append("  Rules Loaded");

                    RuleHandlerHelper.LogAndDispose(log_message, "ComplianceUserActionLog", 1, 1, TraceEventType.Information);
                }
                else if (operationType == RuleOperations.AddRule.ToString())
                {
                    if (RuleOperation != null)
                        RuleOperation(this, new RuleOperationEventArgs { RuleList = ruleList, Category = RuleCategory.UserDefined, OperationType = RuleOperations.AddRule, OldValue = null, HasError = !operationStatus, ErrorMessage = errorMessage.ToString() });


                }
                else if (operationType == RuleOperations.ImportRule.ToString())
                {
                    RuleBase rule = new UserDefinedRule();
                    rule.RuleId = dsReceived.Tables[0].Rows[0]["ruleId"].ToString();
                    rule.RuleName = dsReceived.Tables[0].Rows[0]["ruleName"].ToString();
                    rule.Enabled = false;
                    rule.Category = RuleCategory.UserDefined;
                    rule.Package = (RulePackage)Enum.Parse(typeof(RulePackage), dsReceived.Tables[0].Rows[0]["ruleType"].ToString(), true);
                    rule.RuleURL = GetUrl(dsReceived.Tables[0].Rows[0]["ruleId"].ToString());
                    if (RuleOperation != null)
                    {
                        RuleOperation(this, new RuleOperationEventArgs { RuleList = ruleList, Category = RuleCategory.UserDefined, OperationType = RuleOperations.ImportRule, OldValue = null, HasError = !operationStatus, ErrorMessage = errorMessage.ToString() });
                        RuleHandlerHelper.UserComplianceActionLoggerResponse(rule, EnumHelper.GetDescription(RuleOperations.ImportRule), RuleNavigatorConstants.SUCCESSFUL, Convert.ToInt32(dsReceived.Tables[0].Rows[0]["ActionUser"]));
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
        /// Update Url for all rules after rule operation
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        private string GetUrl(String ruleId)
        {
            try
            {
                String param = "client=" + UserDefinedRuleConstants.CLIENT_NAME + "&assetsUUIDs=" + ruleId + "&isPowerUser=" + _isPowerUser;
                return UserDefinedRuleConstants.GUVNOR_STANDALONE_BASE_URL + param;
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
                return String.Empty;
            }
        }


    }
}
