using System;
using System.Collections.Generic;
using System.Text;
using Prana.AmqpAdapter.Amqp;
using Prana.Global;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.Delegates;
using System.Data;
using Prana.AmqpAdapter.Interfaces;
using Prana.RuleEngine.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Prana.RuleEngine.BusinessLogic
{

    internal delegate void AllRuleReceivedHandler();
    internal delegate void RuleOperationReceivedHandler(DataSet dsRecieved);

    internal class CustomRuleManager
    {
        static CustomRuleManager _customRuleManager;

        internal static CustomRuleManager GetInstance()
        {
            if (_customRuleManager == null)
                _customRuleManager = new CustomRuleManager();

            return _customRuleManager;
        }

        internal static void DisposeSingletonInstance()
        {
            _customRuleManager = null;
        }


        /// <summary>
        /// Private constructor to implement singleton pattern
        /// </summary>
        private CustomRuleManager()
        {

        }

        #region events

        internal event AllRuleReceivedHandler AllRuleReceived;
        internal event RuleOperationReceivedHandler RuleOperationReceived;

        #endregion

        #region Private properties

        String initialiseResponseRoutingKey = "InitialiseResponse";
        //String initialiseRequestRoutingKey = "InitialiseRequest";
        String customRuleResponseRoutingKey = "CustomRuleResponse";
        String customRuleRequestRoutingKey = "CustomRuleRequest";

        Boolean _isInitialised = false;
        Boolean _isImport = false;

        //String _hostName;
        String _exchangeName;
        String _cahaeKey = "CustomRuleRequest";
        Dictionary<String, CustomRule> _ruleCache = new Dictionary<string, CustomRule>();

        List<IAmqpReceiver> amqpReceiverList = new List<IAmqpReceiver>();

        int _userId = -1;

        #endregion


        internal void Initialise(int userId)
        {
            LoadAppSettings(userId);
            IntialiseAmqpPlugins();

        }

        private void IntialiseAmqpPlugins()
        {
            //lock (amqpReceiverList)
            //{
            //    amqpReceiverList = new List<IAmqpReceiver>();
            //}
            try
            {
                AmqpHelper.Started += new ListenerStarted(AmqpHelper_Started);
                AmqpHelper.Stopped += new ListenerStopped(AmqpHelper_Stopped);
                AmqpHelper.InitializeSender(_cahaeKey,  _exchangeName, MediaType.Exchange_Direct);

                List<String> routingKey = new List<string>();
                routingKey.Add(customRuleResponseRoutingKey);//routingKey.Add(initialiseResponseRoutingKey);
                AmqpHelper.InitializeListenerForExchange( _exchangeName, MediaType.Exchange_Direct, routingKey);


                List<String> routingKeyInit = new List<string>();
                routingKeyInit.Add(initialiseResponseRoutingKey);
                AmqpHelper.InitializeListenerForExchange(_exchangeName, MediaType.Exchange_Direct, routingKeyInit);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void AmqpHelper_Stopped(IAmqpReceiver amqpReceiver, ListenerStopCause cause)
        {
            try
            {
                if (amqpReceiver.MediaName == _exchangeName)
                    amqpReceiver.AmqpDataReceived -= new DataReceived(amqpReceiver_AmqpDataReceived);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void AmqpHelper_Started(IAmqpReceiver amqpReceiver)
        {
            try
            {

                lock (amqpReceiverList)
                {
                    if (amqpReceiver.MediaName == _exchangeName)
                    {

                        amqpReceiverList.Add(amqpReceiver);

                        amqpReceiver.AmqpDataReceived += new DataReceived(amqpReceiver_AmqpDataReceived);
                        SendInitialiseRequest();
                    }

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private String SendInitialiseRequest()
        {
            try
            {
                return SendCustomRuleRequest("InitialisationRequest", null, String.Empty);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }

        }


        internal String SendCustomRuleRequest(String operationType, CustomRule rule, String text)
        {
            try
            {
                Dictionary<String, String> requestDict = new Dictionary<string, string>();
                requestDict.Add("OperationType", operationType);
                String response = "Could not apply operation on rule.\n";
                bool doSend = false;
                switch (operationType)
                {
                    case "InitialisationRequest":
                        doSend = true;
                        if (text == "Import")
                            _isImport = true;
                        break;
                    case "Enable":
                    case "Disable":
                    case "Delete":
                        requestDict.Add("RuleId", rule.RuleId);
                        doSend = true;
                        break;
                    case "Rename":

                        String renResponse = ValidateRename(rule.RuleType,text);
                        if (String.IsNullOrEmpty(renResponse))
                        {
                            requestDict.Add("RuleId", rule.RuleId);
                            requestDict.Add("NewName", text);
                            doSend = true;

                        }
                        else
                            response += renResponse;
                        break;
                    case "UpdateSummary":
                        String sumResponse = ValidateSummary(text);
                        if (String.IsNullOrEmpty(sumResponse))
                        {
                            requestDict.Add("RuleId", rule.RuleId);
                            requestDict.Add("Summary", text);
                            doSend = true;
                        }
                        else
                            response += sumResponse;
                        break;

                }
                if (doSend)
                {
                    AmqpHelper.SendObject(requestDict, _cahaeKey, customRuleRequestRoutingKey);
                    return String.Empty;
                }
                else
                {
                    return response;
                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        Regex regex = new Regex(@"^[a-zA-Z0-9][-@\$%&_.a-zA-Z0-9 ]*[a-zA-Z0-9]$");
        private String ValidateSummary(String text)
        {

            try
            {
                if (String.IsNullOrEmpty(text))
                    return "Summary cannot be blank";
                else
                    return String.Empty;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return "Error occured";
            }
        }

        private String ValidateRename(String packageName,String text)
        {
            try
            {
                if (String.IsNullOrEmpty(text))
                {
                    return "Rule Name cannot be blank";
                }
                else if (!regex.IsMatch(text))
                {
                    return @"Rule name can only have alphanumeric characters, space and @ _ . - & $ % "
                        +"special characters and can not start/end with special characters.";

                }
                else if (IsCustomRuleNamePresent(packageName+"Compliance", text))
                {
                    return "This rule name already present in custom rule";
                }
                else
                    return String.Empty;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return "Error occured";
            }

        }

        internal bool IsCustomRuleNamePresent(String packageName,String text)
        {
            bool isPresent = false;
            try
            {
                lock (_ruleCache)
                {
                    foreach (String key in _ruleCache.Keys)
                    {
                        if (_ruleCache[key].Name == text && _ruleCache[key].RuleType + "Compliance" == packageName)
                        {
                            isPresent = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return isPresent;
        }


        void amqpReceiver_AmqpDataReceived(DataSet dsReceived, string mediaName, MediaType mediaType, string routingKey)
        {
            try
            {

                if (routingKey == "_" + initialiseResponseRoutingKey)
                    HandleAllRuleReceived(dsReceived);
                else if (routingKey == "_" + customRuleResponseRoutingKey)
                    RuleOperationResponseReceived(dsReceived);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void RuleOperationResponseReceived(DataSet dsReceived)
        {
            try
            {

                lock (_ruleCache)
                {
                    _ruleCache = ApplyOperation(dsReceived);
                }

                if (RuleOperationReceived != null)
                    RuleOperationReceived(dsReceived);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private Dictionary<string, CustomRule> ApplyOperation(DataSet dsReceived)
        {
            try
            {

                String ruleId = dsReceived.Tables[0].Rows[0]["ruleId"].ToString();
                String operationType = dsReceived.Tables[0].Rows[0]["OperationType"].ToString();
                switch (operationType)
                {
                    case "Enable":
                        _ruleCache[ruleId].Enabled = true;
                        break;
                    case "Disable":
                        _ruleCache[ruleId].Enabled = false;
                        break;
                    case "Delete":
                        _ruleCache.Remove(ruleId);
                        break;
                    case "Rename":
                        _ruleCache[ruleId].Name = dsReceived.Tables[0].Rows[0]["NewName"].ToString();
                        break;
                    case "UpdateSummary":

                        break;


                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return _ruleCache;
        }

        private void HandleAllRuleReceived(DataSet dsReceived)
        {
            try
            {
                lock (_ruleCache)
                {
                    if (!_isInitialised || _isImport)
                    {


                        _ruleCache = MapAllRules(dsReceived);
                        WriteAllRuleHtmlFile();
                        _isInitialised = true;
                        _isImport = false;



                        if (AllRuleReceived != null)
                            AllRuleReceived();
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private Dictionary<string, CustomRule> MapAllRules(DataSet dsReceived)
        {
            try
            {

                foreach (DataTable dt in dsReceived.Tables)
                {


                    foreach (DataRow dr in dt.Rows)
                    {
                        String key = dr["ruleId"].ToString();
                        if (!_ruleCache.ContainsKey(key))
                        {
                            _ruleCache.Add(key, new CustomRule());
                        }
                        foreach (String keys in _ruleCache.Keys)
                        {
                            if (key.Equals(keys))
                            {
                                _ruleCache[keys].RuleId = key;
                                _ruleCache[keys].CompressionLevel = dr["compressionLevel"].ToString();
                                _ruleCache[keys].Description = dr["description"].ToString();
                                _ruleCache[keys].Enabled = Boolean.Parse(dr["enabled"].ToString());
                                _ruleCache[keys].Name = dr["name"].ToString();
                                _ruleCache[keys].RuleType = dr["ruleType"].ToString();
                                _ruleCache[keys].IsDeleted = Boolean.Parse(dr["isDeleted"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return _ruleCache;
        }



        private void LoadAppSettings(int userId)
        {
            try
            {
                _userId = userId;
                //_hostName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_AmqpServer);
                _exchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_OtherDataExchange);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        internal Dictionary<string, CustomRule> GetAllRules(String ruleType)
        {
            try
            {

                Dictionary<String, CustomRule> packageRule = new Dictionary<string, CustomRule>();
                foreach (String key in _ruleCache.Keys)
                {
                    if ((_ruleCache[key].RuleType + "Compliance").Equals(ruleType) && _ruleCache[key].IsDeleted == false)
                        
                        packageRule.Add(key, _ruleCache[key]);
                }
                return packageRule;
            }
            catch (Exception ex)
            {                
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }


        internal CustomRule GetRuleByRuleId(string packageName, string ruleName)
        {
            try
            {

                lock (_ruleCache)
                {
                    foreach (String key in _ruleCache.Keys)
                    {
                        if (_ruleCache[key].RuleType + "Compliance" == packageName && _ruleCache[key].Name == ruleName)
                            return _ruleCache[key];
                    }
                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        internal void CloseAllConnection()
        {
            try
            {
                lock (amqpReceiverList)
                {
                    foreach (IAmqpReceiver amqp in amqpReceiverList)
                    {
                        amqp.CloseListener();
                        amqp.AmqpDataReceived -= new DataReceived(amqpReceiver_AmqpDataReceived);
                    }
                    amqpReceiverList.Clear();
                }
                AmqpHelper.Started -= new ListenerStarted(AmqpHelper_Started);
                AmqpHelper.Stopped -= new ListenerStopped(AmqpHelper_Stopped);

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        //String phRuleName = "#RULE_NAME#";
        //String phDescription = "#DESCRIPTION#";
        //String phCompressionLevel = "#COMPRESSION_LEVEL#";
        //String customRulePath = Application.StartupPath + "\\CustomRuleHtml";
        //String customRuleTemplatePath = Application.StartupPath + "\\CustomRule.htm";

        private void WriteAllRuleHtmlFile()
        {

            try
            {
                if (!Directory.Exists(Constants.CUSTOM_RULE_PATH))
                    Directory.CreateDirectory(Constants.CUSTOM_RULE_PATH);

                if (!Directory.Exists(Constants.CUSTOM_RULE_PATH + "\\" + _userId.ToString()))
                    Directory.CreateDirectory(Constants.CUSTOM_RULE_PATH + "\\" + _userId.ToString());

                String htmlTemplate = null;
                using (StreamReader read = new StreamReader(Constants.CUSTOM_RULE_TEMPLATE_PATH))
                {
                    htmlTemplate = read.ReadToEnd();
                }

                if (!String.IsNullOrEmpty(htmlTemplate))
                {
                    lock (_ruleCache)
                    {
                        foreach (String key in _ruleCache.Keys)
                        {
                            String htmlOutputPath = Constants.CUSTOM_RULE_PATH + "\\" + _userId.ToString() + "\\" + _ruleCache[key].RuleId + ".htm";

                            StringBuilder builder = new StringBuilder();
                            builder.Append(htmlTemplate);
                            builder.Replace(Constants.PH_RULE_NAME, _ruleCache[key].Name);
                            builder.Replace(Constants.PH_COMPLIANCE_LEVEL, _ruleCache[key].CompressionLevel);
                            builder.Replace(Constants.PH_DESCRIPTION, _ruleCache[key].Description);


                            using (StreamWriter writer = new StreamWriter(htmlOutputPath, false))
                            {
                                writer.Write(builder.ToString());
                            }


                            _ruleCache[key].HtmlPath = htmlOutputPath;

                        }
                    }
                }
                else
                    throw new Exception("Could not read custom rule template");

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }



        internal void ChangeStateOfAllRules(string selectedPackageName, bool isDisabled)
        {
            try
            {
                Dictionary<String, CustomRule> packageRule = GetAllRules(selectedPackageName);
                String operationType;
                if (isDisabled)
                    operationType = "Disable";
                else
                    operationType = "Enable";
                foreach (String key in packageRule.Keys)
                {
                    SendCustomRuleRequest(operationType, packageRule[key], String.Empty);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
