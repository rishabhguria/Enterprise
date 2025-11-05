using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.EventArguments;
using Prana.AmqpAdapter.Json;
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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Prana.ComplianceEngine.RuleDefinition.DAL
{
    internal class CustomRuleHandler : IRuleHandler
    {
        String _exchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_OtherDataExchange);
        String _initialiseResponseRoutingKey = "InitialiseResponse";
        String _cacheKey = "CustomRuleRequest";
        String customRuleRequestRoutingKey = "CustomRuleRequest";
        String _customRuleResponse = "CustomRuleResponse";

        int _userId = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
        bool _isInitialized = true;
        private static CustomRuleHandler _customRuleHandler;
        private static Object _customRuleLockerObject = new Object();
        private static Dictionary<string, string> customRuleConstantValue = new Dictionary<string, string>();

        /// <summary>
        /// Initalizes AMQP plugin
        /// </summary>
        private CustomRuleHandler()
        {
            try
            {
                InitializeAmqp();
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
        /// returns singleton instance for this handler
        /// </summary>
        /// <returns></returns>
        public static IRuleHandler GetInstance()
        {
            lock (_customRuleLockerObject)
            {
                if (_customRuleHandler == null)
                    _customRuleHandler = new CustomRuleHandler();
                return _customRuleHandler;
            }
            //return singleton instance
        }



        #region IRuleHandler Members

        /// <summary>
        /// Loads all rule from Esper in background thread.
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // e.Error
        }

        /// <summary>
        /// Sends initalization request to esper
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {

            try
            {
                while (!SendInitializationRequest())
                {
                    Thread.Sleep(1000);
                }
                //Dictionary<String, String> requestDict = new Dictionary<string, string>();
                //requestDict.Add("OperationType", "InitialisationRequest");
                //if (!AmqpHelper.SendObject(requestDict, _cacheKey, customRuleRequestRoutingKey))
                //{
                //    Thread.Sleep(1000);
                //    if (!AmqpHelper.SendObject(requestDict, _cacheKey, customRuleRequestRoutingKey))
                //    {
                //        throw new Exception("Could not send initialization request for CustomRules");
                //    }
                //    //throw new Exception("Could not send initialization request for CustomRules");
                //}
                e.Result = "Initialization request sent";
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
        /// Sends initalization request to esper returns true or false accordingly.
        /// </summary>
        /// <returns></returns>
        private bool SendInitializationRequest()
        {
            try
            {
                _isInitialized = false;
                Dictionary<String, String> requestDict = new Dictionary<string, string>();
                requestDict.Add("OperationType", "InitialisationRequest");
                return AmqpHelper.SendObject(requestDict, _cacheKey, customRuleRequestRoutingKey);

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

        public event RuleLoadedHandler RuleLoaded;

        /// <summary>
        /// Sends request for operations on rule to esper.
        /// </summary>
        /// <param name="rulePackage"></param>
        /// <param name="ruleName"></param>
        /// <param name="ruleOperations"></param>
        /// <param name="ruleId"></param>
        /// <param name="oldRuleName">Required only for rename operation</param>
        /// <param name="tag"></param>
        public void OperationOnRule(RulePackage rulePackage, string ruleName, RuleOperations ruleOperations, String ruleId, String oldRuleName, Object tag)
        {
            try
            {
                Dictionary<String, String> operation = new Dictionary<string, string>();
                operation.Add("ActionUser", Convert.ToString(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID));
                operation.Add("Method", "OperationOnRuleCRH");
                operation.Add("OperationType", ruleOperations.ToString());
                operation.Add("NewName", ruleName);
                operation.Add("RuleId", ruleId);
                operation.Add("directoryPath", ComplianceCacheManager.GetImportExportPath());
                operation.Add("ruleCategory", RuleCategory.CustomRule.ToString());
                operation.Add("rulePackage", rulePackage.ToString());
                operation.Add("tag", tag != null ? tag.ToString() : "");
                AmqpHelper.SendObject(operation, _cacheKey, customRuleRequestRoutingKey);
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


        public event RuleOperationHandler RuleOperation;

        public void ExportRule(List<RuleBase> ruleList, string importExportPath)
        {
            try
            {
                foreach (RuleBase rule in ruleList)
                {
                    Dictionary<String, String> export = new Dictionary<string, string>();
                    export.Add("ActionUser", Convert.ToString(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID));
                    export.Add("Method", "ExportRuleCRH");
                    export.Add("directoryPath", importExportPath);
                    export.Add("packageName", rule.Package.ToString());
                    export.Add("ruleName", rule.RuleName);
                    export.Add("ruleId", rule.RuleId);
                    export.Add("ruleCategory", rule.Category.ToString());
                    export.Add("OperationType", "ExportRule");
                    AmqpHelper.SendObject(export, _cacheKey, CustomRuleConstants.CUSTOM_RULE_EXPORT_ROUTING_KEY);
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

        //public event RuleImportExportHandler ExportComplete;

        #endregion

        /// <summary>
        /// Initializes sender and listener for initialization and rule operations.
        /// </summary>
        public void InitializeAmqp()
        {

            try
            {
                AmqpHelper.Started += new ListenerStarted(AmqpHelper_Started);
                AmqpHelper.Stopped += new ListenerStopped(AmqpHelper_Stopped);
                AmqpHelper.InitializeSender(_cacheKey, _exchangeName, MediaType.Exchange_Direct);

                List<String> routingKeyInit = new List<string>();
                routingKeyInit.Add(_initialiseResponseRoutingKey);
                AmqpHelper.InitializeListenerForExchange(_exchangeName, MediaType.Exchange_Direct, routingKeyInit);

                List<String> routingKeyOp = new List<string>();
                routingKeyOp.Add(_customRuleResponse);
                AmqpHelper.InitializeListenerForExchange(_exchangeName, MediaType.Exchange_Direct, routingKeyOp);

                List<String> routingKeyImportExport = new List<string>();
                routingKeyImportExport.Add("CustomRuleImportExportComplete");
                AmqpHelper.InitializeListenerForExchange(_exchangeName, MediaType.Exchange_Direct, routingKeyImportExport);
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
        /// 
        /// </summary>
        /// <param name="amqpReceiver"></param>
        /// <param name="cause"></param>
        private void AmqpHelper_Stopped(Object sender, ListenerStoppedEventArguments e)
        {
            try
            {
                if (e.AmqpReceiver.MediaName == _exchangeName)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amqpReceiver"></param>
        private void AmqpHelper_Started(Object sender, ListenerStartedEventArguments e)
        {
            try
            {
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

        /// <summary>
        /// Recieves data after rule Initialization and Operation.
        /// </summary>
        /// <param name="dsReceived"></param>
        /// <param name="mediaName"></param>
        /// <param name="mediaType"></param>
        /// <param name="routingKey"></param>
        private void amqpReceiver_AmqpDataReceived(Object sender, DataReceivedEventArguments e)
        {

            try
            {
                if (e.RoutingKey == "_" + _initialiseResponseRoutingKey && !_isInitialized)
                {
                    _isInitialized = true;
                    HandleAllRulesRecieved(e.DsReceived);

                }
                else if (e.RoutingKey == "_" + _customRuleResponse)
                {
                    RuleOperations operationType = (RuleOperations)Enum.Parse(typeof(RuleOperations), e.DsReceived.Tables[0].Rows[0]["OperationType"].ToString(), true);

                    if (operationType == RuleOperations.Build)
                    {
                        List<RuleBase> ruleList = new List<RuleBase>();
                        RuleBase rule = new CustomRuleDefinition();
                        rule.RuleId = e.DsReceived.Tables[0].Rows[0]["RuleId"].ToString();
                        rule.RuleName = e.DsReceived.Tables[0].Rows[0]["NewName"].ToString();
                        rule.Enabled = Convert.ToBoolean(e.DsReceived.Tables[0].Rows[0]["enabled"].ToString());
                        rule.Package = (RulePackage)Enum.Parse(typeof(RulePackage), e.DsReceived.Tables[0].Rows[0]["rulePackage"].ToString(), true);
                        rule.RuleURL = GetUrl(rule.RuleId);
                        ((CustomRuleDefinition)rule).ConstantsDefinationAsJSon = e.DsReceived.Tables[0].Rows[0]["tag"].ToString();
                        ruleList.Add(rule);
                        UpdateRuleConstants(ruleList);
                        if (RuleOperation != null)
                            RuleOperation(this, new RuleOperationEventArgs { RuleList = ruleList, Category = RuleCategory.CustomRule, OperationType = RuleOperations.Build, OldValue = "", HasError = false, ErrorMessage = "" });
                        RuleHandlerHelper.UserComplianceActionLoggerResponse(rule, EnumHelper.GetDescription(operationType), RuleNavigatorConstants.UNSUCCESSFUL, Convert.ToInt32(e.DsReceived.Tables[0].Rows[0]["ActionUser"]));
                    }
                    if (operationType == RuleOperations.RenameRule)
                    {
                        List<RuleBase> ruleList = new List<RuleBase>();
                        RuleBase rule = new CustomRuleDefinition();
                        rule.RuleId = e.DsReceived.Tables[0].Rows[0]["RuleId"].ToString();
                        rule.RuleName = e.DsReceived.Tables[0].Rows[0]["NewName"].ToString();
                        rule.Package = (RulePackage)Enum.Parse(typeof(RulePackage), e.DsReceived.Tables[0].Rows[0]["rulePackage"].ToString(), true);
                        rule.RuleURL = GetUrl(rule.RuleId);
                        ruleList.Add(rule);
                        if (RuleOperation != null)
                            RuleOperation(this, new RuleOperationEventArgs { RuleList = ruleList, Category = RuleCategory.CustomRule, OperationType = RuleOperations.RenameRule, OldValue = ruleList[0].RuleId, HasError = false, ErrorMessage = "" });
                        RuleHandlerHelper.UserComplianceActionLoggerResponse(rule, EnumHelper.GetDescription(operationType), RuleNavigatorConstants.SUCCESSFUL, Convert.ToInt32(e.DsReceived.Tables[0].Rows[0]["ActionUser"]));
                    }
                    if (operationType == RuleOperations.EnableRule)
                    {
                        List<RuleBase> ruleList = new List<RuleBase>();
                        RuleBase rule = new CustomRuleDefinition();
                        rule.RuleId = e.DsReceived.Tables[0].Rows[0]["RuleId"].ToString();
                        rule.RuleName = e.DsReceived.Tables[0].Rows[0]["NewName"].ToString();
                        rule.Package = (RulePackage)Enum.Parse(typeof(RulePackage), e.DsReceived.Tables[0].Rows[0]["rulePackage"].ToString(), true);
                        rule.Enabled = true;
                        rule.RuleURL = GetUrl(rule.RuleId);
                        ruleList.Add(rule);
                        if (RuleOperation != null)
                            RuleOperation(this, new RuleOperationEventArgs { RuleList = ruleList, Category = RuleCategory.CustomRule, OperationType = RuleOperations.EnableRule, OldValue = null, HasError = false, ErrorMessage = "" });
                        RuleHandlerHelper.UserComplianceActionLoggerResponse(rule, EnumHelper.GetDescription(operationType), RuleNavigatorConstants.SUCCESSFUL, Convert.ToInt32(e.DsReceived.Tables[0].Rows[0]["ActionUser"]));
                    }
                    if (operationType == RuleOperations.DisableRule)
                    {
                        List<RuleBase> ruleList = new List<RuleBase>();
                        RuleBase rule = new CustomRuleDefinition();
                        rule.RuleId = e.DsReceived.Tables[0].Rows[0]["RuleId"].ToString();
                        rule.RuleName = e.DsReceived.Tables[0].Rows[0]["NewName"].ToString();
                        rule.Package = (RulePackage)Enum.Parse(typeof(RulePackage), e.DsReceived.Tables[0].Rows[0]["rulePackage"].ToString(), true);
                        rule.Enabled = false;
                        rule.RuleURL = GetUrl(rule.RuleId);
                        ruleList.Add(rule);
                        if (RuleOperation != null)
                            RuleOperation(this, new RuleOperationEventArgs { RuleList = ruleList, Category = RuleCategory.CustomRule, OperationType = RuleOperations.DisableRule, OldValue = null, HasError = false, ErrorMessage = "" });
                        RuleHandlerHelper.UserComplianceActionLoggerResponse(rule, EnumHelper.GetDescription(operationType), RuleNavigatorConstants.SUCCESSFUL, Convert.ToInt32(e.DsReceived.Tables[0].Rows[0]["ActionUser"]));
                    }
                }
                else if (e.RoutingKey == "_CustomRuleImportExportComplete")
                {
                    if (e.DsReceived.Tables[0].Rows[0]["responseType"].ToString() == "CustomRuleExportComplete")
                    {
                        String ruleId = e.DsReceived.Tables[0].Rows[0]["ruleId"].ToString();
                        RulePackage packageName = (RulePackage)Enum.Parse(typeof(RulePackage), e.DsReceived.Tables[0].Rows[0]["packageName"].ToString(), true);
                        //RuleCategory ruleCategory = (RuleCategory)Enum.Parse(typeof(RuleCategory), e.DsReceived.Tables[0].Rows[0]["ruleCategory"].ToString(), true);

                        List<RuleBase> ruleList = new List<RuleBase>();
                        RuleBase rule = new CustomRuleDefinition();
                        rule.RuleId = ruleId;
                        rule.Package = packageName;
                        rule.RuleName = e.DsReceived.Tables[0].Rows[0]["ruleName"].ToString();
                        rule.Category = RuleCategory.CustomRule;
                        ruleList.Add(rule);
                        if (RuleOperation != null)
                            RuleOperation(this, new RuleOperationEventArgs { RuleList = ruleList, Category = RuleCategory.CustomRule, OperationType = RuleOperations.ExportRule, OldValue = null, HasError = false, ErrorMessage = "" });
                        if (Convert.ToBoolean(e.DsReceived.Tables[0].Rows[0]["operationStatus"].ToString()))
                            RuleHandlerHelper.UserComplianceActionLoggerResponse(rule, "Export Rule", RuleNavigatorConstants.SUCCESSFUL, Convert.ToInt32(e.DsReceived.Tables[0].Rows[0]["ActionUser"]));
                        else
                            RuleHandlerHelper.UserComplianceActionLoggerResponse(rule, "Export Rule", RuleNavigatorConstants.UNSUCCESSFUL, Convert.ToInt32(e.DsReceived.Tables[0].Rows[0]["ActionUser"]));

                    }

                    if (e.DsReceived.Tables[0].Rows[0]["responseType"].ToString() == "CustomRuleImportComplete")
                    {
                        bool operationStatus = Convert.ToBoolean(e.DsReceived.Tables[0].Rows[0]["operationStatus"].ToString());
                        if (operationStatus)
                        {
                            String ruleId = e.DsReceived.Tables[0].Rows[0]["ruleId"].ToString();
                            RulePackage packageName = (RulePackage)Enum.Parse(typeof(RulePackage), e.DsReceived.Tables[0].Rows[0]["packageName"].ToString(), true);
                            // RuleCategory ruleCategory = (RuleCategory)Enum.Parse(typeof(RuleCategory), e.DsReceived.Tables[0].Rows[0]["ruleCategory"].ToString(), true);

                            List<RuleBase> ruleList = new List<RuleBase>();
                            CustomRuleDefinition rule = new CustomRuleDefinition();
                            rule.RuleId = ruleId;
                            rule.Package = packageName;
                            rule.Category = RuleCategory.CustomRule;
                            rule.Enabled = Convert.ToBoolean(e.DsReceived.Tables[0].Rows[0]["enabled"].ToString());
                            rule.Description = e.DsReceived.Tables[0].Rows[0]["description"].ToString();
                            rule.RuleName = e.DsReceived.Tables[0].Rows[0]["newRuleName"].ToString();
                            ruleList.Add(rule);

                            WriteAllRuleHtmlFile(ruleList);
                            if (RuleOperation != null)
                                RuleOperation(this, new RuleOperationEventArgs { RuleList = ruleList, Category = RuleCategory.CustomRule, OperationType = RuleOperations.ImportRule, OldValue = null, HasError = false, ErrorMessage = "" });
                            RuleHandlerHelper.UserComplianceActionLoggerResponse(rule, "Import Rule", RuleNavigatorConstants.SUCCESSFUL, Convert.ToInt32(e.DsReceived.Tables[0].Rows[0]["ActionUser"]));

                        }
                        else
                        {
                            RulePackage packageName = (RulePackage)Enum.Parse(typeof(RulePackage), e.DsReceived.Tables[0].Rows[0]["packageName"].ToString(), true);
                            //RuleCategory ruleCategory = (RuleCategory)Enum.Parse(typeof(RuleCategory), e.DsReceived.Tables[0].Rows[0]["ruleCategory"].ToString(), true);

                            List<RuleBase> ruleList = new List<RuleBase>();
                            CustomRuleDefinition rule = new CustomRuleDefinition();
                            rule.Package = packageName;
                            rule.Category = RuleCategory.CustomRule;
                            rule.RuleName = e.DsReceived.Tables[0].Rows[0]["newRuleName"].ToString();
                            rule.RuleURL = String.Empty;
                            ruleList.Add(rule);
                            if (RuleOperation != null)
                                RuleOperation(this, new RuleOperationEventArgs { RuleList = ruleList, Category = RuleCategory.CustomRule, OperationType = RuleOperations.ImportRule, OldValue = null, HasError = true, ErrorMessage = rule.RuleName });
                            RuleHandlerHelper.UserComplianceActionLoggerResponse(rule, "Import Rule", RuleNavigatorConstants.UNSUCCESSFUL, Convert.ToInt32(e.DsReceived.Tables[0].Rows[0]["ActionUser"]));
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
                RuleBase rule = new CustomRuleDefinition();
                rule.RuleId = e.DsReceived.Tables[0].Rows[0]["RuleId"].ToString();
                rule.RuleName = e.DsReceived.Tables[0].Rows[0]["NewName"].ToString();
                rule.Enabled = Convert.ToBoolean(e.DsReceived.Tables[0].Rows[0]["enabled"].ToString());
                rule.Package = (RulePackage)Enum.Parse(typeof(RulePackage), e.DsReceived.Tables[0].Rows[0]["rulePackage"].ToString(), true);
                RuleHandlerHelper.UserComplianceActionLoggerResponse(rule, e.DsReceived.Tables[0].Rows[0]["OperationType"].ToString(), RuleNavigatorConstants.UNSUCCESSFUL, Convert.ToInt32(e.DsReceived.Tables[0].Rows[0]["ActionUser"]));
            }
        }

        /// <summary>
        /// Update Existing Rule Constants with the new constants
        /// </summary>
        /// <param name="ruleList"></param>
        private void UpdateRuleConstants(List<RuleBase> ruleList)
        {
            try
            {
                lock (ruleList)
                {
                    for (int key = 0; key < ruleList.Count; key++)
                    {
                        CustomRuleDefinition rule = new CustomRuleDefinition();
                        rule = (CustomRuleDefinition)ruleList[key];
                        String htmlOutputPath = CustomRuleConstants.CUSTOM_RULE_PATH + "\\" + _userId.ToString() + "\\" + ruleList[key].RuleId + ".htm";
                        String htmlTemplate = null;
                        // Getting the existing html file for the rule to replace constants
                        using (StreamReader read = new StreamReader(htmlOutputPath))
                        {
                            htmlTemplate = read.ReadToEnd();
                        }
                        // Getting the constant with correct format
                        String constant = "{" + String.Format("\"HashMap\":{0}", rule.ConstantsDefinationAsJSon) + "}";
                        // String builder that replace old constant to new constant
                        StringBuilder builder = new StringBuilder();
                        builder.Append(RegzTableContantsBlank(htmlTemplate));
                        builder.Replace(CustomRuleConstants.PH_CONSTANTHTML, GetHtmlFromJson(constant));
                        //Write new updated file into the html existing file
                        using (StreamWriter writer = new StreamWriter(htmlOutputPath, false))
                        {
                            writer.Write(builder.ToString());
                        }
                        ruleList[key].RuleURL = htmlOutputPath;
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
        /// Return string replace old constant with the tag #Rule Constant#
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        private string RegzTableContantsBlank(String builder)
        {
            try
            {
                string table_pattern = "<table class[^>]*>(?:.|\n)*?</table>";
                return Regex.Replace(builder, table_pattern, "<table class=\"constantList\">" + CustomRuleConstants.PH_CONSTANTHTML + "</table>");
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
        /// Raises event with rule list 
        /// </summary>
        /// <param name="dsReceived"></param>
        private void HandleAllRulesRecieved(DataSet dsReceived)
        {
            try
            {
                List<RuleBase> ruleList = new List<RuleBase>();
                for (int i = 0; i < dsReceived.Tables.Count; i++)
                {
                    RuleBase rule = new CustomRuleDefinition(dsReceived.Tables[i].Rows[0]);
                    ruleList.Add(rule);
                    AddCustomRuleComboListToCache(dsReceived.Tables[i].Rows[0]["constants"].ToString());
                }
                WriteAllRuleHtmlFile(ruleList);

                if (RuleLoaded != null)
                    RuleLoaded(this, new RuleLoadEventArgs(ruleList, RuleCategory.CustomRule));
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
        /// This method is to add values of comboList to the cache 
        /// </summary>
        /// <param name="json"></param>
        private void AddCustomRuleComboListToCache(String json)
        {
            try
            {
                DataSet ds = JsonHelper.Deserialize(json);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["type"].ToString().Equals("Combo"))
                        {
                            string tempValues = row["comboList"].ToString();
                            string[] splitValues = tempValues.Split(',');
                            if (splitValues.Count() > 1)
                                if (!customRuleConstantValue.ContainsKey(row["name"].ToString()))
                                    customRuleConstantValue.Add(row["name"].ToString(), tempValues);
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
        }

        /// <summary>
        /// Write HTMLs for all custom rules at startup path from template for displaying custom rules. 
        /// </summary>
        /// <param name="ruleList"></param>
        private void WriteAllRuleHtmlFile(List<RuleBase> ruleList)
        {
            try
            {
                if (!Directory.Exists(CustomRuleConstants.CUSTOM_RULE_PATH))
                    Directory.CreateDirectory(CustomRuleConstants.CUSTOM_RULE_PATH);

                if (!Directory.Exists(CustomRuleConstants.CUSTOM_RULE_PATH + "\\" + _userId.ToString()))
                    Directory.CreateDirectory(CustomRuleConstants.CUSTOM_RULE_PATH + "\\" + _userId.ToString());

                String htmlTemplate = null;
                using (StreamReader read = new StreamReader(CustomRuleConstants.CUSTOM_RULE_TEMPLATE_PATH))
                {
                    htmlTemplate = read.ReadToEnd();
                }

                if (!String.IsNullOrEmpty(htmlTemplate))
                {
                    lock (ruleList)
                    {
                        for (int key = 0; key < ruleList.Count; key++)
                        {
                            CustomRuleDefinition rule = new CustomRuleDefinition();
                            rule = (CustomRuleDefinition)ruleList[key];
                            String htmlOutputPath = CustomRuleConstants.CUSTOM_RULE_PATH + "\\" + _userId.ToString() + "\\" + ruleList[key].RuleId + ".htm";

                            StringBuilder builder = new StringBuilder();
                            builder.Append(htmlTemplate);
                            builder.Replace(CustomRuleConstants.PH_RULE_NAME, ruleList[key].RuleName);
                            // builder.Replace(CustomRuleConstants.PH_COMPLIANCE_LEVEL, rule.CompressionLevel);
                            builder.Replace(CustomRuleConstants.PH_DESCRIPTION, rule.Description);
                            builder.Replace(CustomRuleConstants.PH_CONSTANTHTML, GetHtmlFromJson(rule.ConstantsDefinationAsJSon));
                            using (StreamWriter writer = new StreamWriter(htmlOutputPath, false))
                            {
                                writer.Write(builder.ToString());
                            }
                            ruleList[key].RuleURL = htmlOutputPath;
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
        /// Create a dynamic html table based on the constants
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private String GetHtmlFromJson(String json)
        {
            try
            {
                StringBuilder html = new StringBuilder();
                DataSet ds = JsonHelper.Deserialize(json);
                string[] splitValues;
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        splitValues = row["comboList"].ToString().Split(',');
                        if (row["type"].ToString().ToLower().Equals("boolean"))
                            html.Append(String.Format("<tr><td>{0} <td><select  name='{1}' value='{2}' displayname='{0}' id='{1}' valuetype='{3}'><option value='true'>True</option><option value='false' {4}>False</option></select> </tr> \n", row["displayName"], row["name"], row["value"], row["type"], row["value"].ToString().ToLower().Equals("false") ? "selected" : ""));
                        else if (row["type"].ToString().Equals("Combo") && splitValues.Count() > 1)
                        {
                            string result = "<tr><td>{0} <td><select name='{1}' value='{2}' displayname='{0}' id='{1}' valuetype='{3}' >";
                            foreach (string s in splitValues)
                            {
                                result = string.Concat(result, "<option value ='" + s + "'>" + s + "</option>");
                            }
                            result = result + "</select>  </tr> \n";
                            html.Append(String.Format(result, row["displayName"], row["name"], row["value"], row["type"]));
                        }
                        else
                            html.Append(String.Format("<tr><td>{0} <td><input type='text' name='{1}' value='{2}' displayname='{0}' id='{1}' valuetype='{3}' />  </tr> \n", row["displayName"], row["name"], row["value"], row["type"]));
                    }
                }
                else
                {
                    html.Append("<tr><td><i><small>no custom rule constants</small></i></tr>");
                }
                return html.ToString();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
                return "<tr><td><i><small>failed to load constants</small></i></tr>";
            }
        }

        /// <summary>
        /// To get the updated combobox values in the complaince UI in custom rule constants.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetComboListforCustomRule(string name, string value)
        {
            try
            {
                string[] finalValue = null;
                var str = value;
                if (customRuleConstantValue != null && customRuleConstantValue.ContainsKey(name))
                {
                    finalValue = customRuleConstantValue[name].Split(',');
                    int temp = Array.IndexOf(finalValue, value);
                    string valueAtIndex0 = finalValue[0];
                    finalValue[temp] = valueAtIndex0;
                    finalValue[0] = value;
                }
                if (finalValue != null)
                {
                    str = String.Join(",", finalValue);
                }
                return str;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return value;
            }
        }

        //internal String SendCustomRuleRequest(String operationType, RuleBase rule, String text)
        //{
        //    Dictionary<String, String> requestDict = new Dictionary<string, string>();
        //    requestDict.Add("OperationType", operationType);
        //    String response = "Could not apply operation on rule.\n";
        //    bool doSend = false;
        //    switch (operationType)
        //    {
        //        case "InitialisationRequest":
        //            doSend = true;
        //            //if (text == "Import")
        //            // _isImport = true;
        //            break;
        //        case "Enable":
        //        case "Disable":
        //        case "Delete":
        //            requestDict.Add("RuleId", rule.RuleId);
        //            doSend = true;
        //            break;
        //        //case "Rename":

        //        //    String renResponse = ValidateRename(rule.Package,text);
        //        //    if (String.IsNullOrEmpty(renResponse))
        //        //    {
        //        //        requestDict.Add("RuleId", rule.RuleId);
        //        //        requestDict.Add("NewName", text);
        //        //        doSend = true;

        //        //    }
        //        //    else
        //        //        response += renResponse;
        //        //    break;


        //    }
        //    if (doSend)
        //    {
        //        bool sentSuccessfully = false;
        //        int retryCounter = 5;
        //        while (!sentSuccessfully && retryCounter >= 0)
        //        {
        //            sentSuccessfully = AmqpHelper.SendObject(requestDict, _cacheKey, customRuleRequestRoutingKey);
        //            retryCounter--;
        //            if (!sentSuccessfully)
        //                System.Threading.Thread.Sleep(500);
        //        }
        //        return String.Empty;
        //    }
        //    else
        //    {
        //        return response;
        //    }
        //}




        #region IDisposable Members

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion


        #region IRuleHandler Members


        public void ImportRule(ImportDefinition importDefinition)
        {
            try
            {
                Dictionary<String, String> requestDict = new Dictionary<string, string>();
                requestDict.Add("ActionUser", Convert.ToString(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID));
                requestDict.Add("Method", "ImportRuleCRH");
                requestDict.Add("OperationType", "ImportRule");
                requestDict.Add("packageName", importDefinition.Package.ToString());
                requestDict.Add("directoryPath", importDefinition.DirectoryPath);
                requestDict.Add("oldRuleName", importDefinition.OldRuleName);
                requestDict.Add("newRuleName", importDefinition.RuleName);
                requestDict.Add("ruleName", importDefinition.RuleName);
                requestDict.Add("ruleCategory", importDefinition.Category.ToString());
                AmqpHelper.SendObject(requestDict, _cacheKey, CustomRuleConstants.CUSTOM_RULE_IMPORT_ROUTING_KEY);
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

        #endregion

        /// <summary>
        /// Update Url for all rules after rule operation
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        private string GetUrl(String ruleId)
        {
            try
            {
                if (Directory.Exists(CustomRuleConstants.CUSTOM_RULE_PATH) && Directory.Exists(CustomRuleConstants.CUSTOM_RULE_PATH + "\\" + _userId.ToString()))
                    return CustomRuleConstants.CUSTOM_RULE_PATH + "\\" + _userId.ToString() + "\\" + ruleId + ".htm";
                else
                    return String.Empty;
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
