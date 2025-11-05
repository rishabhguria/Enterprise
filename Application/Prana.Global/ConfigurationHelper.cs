using Prana.LogManager;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Resources;
/*The resource file stores items as name-value pair. To get string values from the resource files (.resx) added in your project, use the following code.*/
//Name Spaces Required

namespace Prana.Global
{
    /// <summary>
    /// Author      : Rajat
    /// Date        : Feb 07 2008
    /// Description : As soon as this class loads, it fetches all values from the app.config file and keep it in a key value pair dictionary for now.
    // TODO : As done for resources file. That whatever is there in the resources file, comes up in a file as property. You can check how the connectionstring is used dynamically by the enterprise library.
    /// </summary>
    public class ConfigurationHelper
    {
        NameValueCollection _availableModules = null;
        NameValueCollection _futuresMultipliers = null;
        NameValueCollection _ricExchangeMapping = null;
        NameValueCollection _pranaExchangeMapping = null;
        NameValueCollection _ricFutureRootSymbolMapping = null;
        NameValueCollection _ricFutureSymbolToExchange = null;
        NameValueCollection _liveFeedSection = null;
        NameValueCollection _underlyingGroupings = null;
        NameValueCollection _appSettings = null;
        NameValueCollection _masterFundAssociation = null;
        NameValueCollection _availableTools = null;
        NameValueCollection _futureExpirationMonthCodes = null;
        NameValueCollection _dataHubTradeImport = null;
        NameValueCollection _esignalSymbolLimits = null;
        NameValueCollection _optionCallPutMonthCodes = null;
        ResourceManager _resManager = null;
        NameValueCollection _masterStrategyAssociation = null;
        NameValueCollection _regexExpression = null;
        NameValueCollection _PlugIns = null;
        NameValueCollection _defaultSwapParameters = null;
        NameValueCollection _pbEbMappingForAllocation = null;
        NameValueCollection _nonUsableCurrencyForSwap = null;
        NameValueCollection _ThemeAndWhiteLabeling = null;
        NameValueCollection _ComplianceSettings = null;
        NameValueCollection _CurrenciesToUpdate = null;
        NameValueCollection _factSetSettings = null;
        NameValueCollection _bloombergSettings = null;
        NameValueCollection _activSettings = null;
        NameValueCollection _sapiSettings = null;

        #region Section constants
        public const string SECTION_availableModules = "availableModules";
        public const string SECTION_futuresMultipliers = "futuresMultipliers";
        public const string SECTION_RICExchangeMapping = "RICExchangeMapping";
        public const string SECTION_PranaExchangeMapping = "PranaExchangeMapping";
        public const string SECTION_RICFutureRootSymbolMapping = "RICFutureRootSymbolMapping";
        public const string SECTION_RICFutureSymbolToExchange = "RICFutureSymbolToExchange";
        public const string SECTION_LiveFeed = "LiveFeed";
        public const string SECTION_AppSetting = "appSettings";
        public const string SECTION_underlyingGroupings = "UnderlyingGroupings";
        public const string SECTION_masterFundAssociation = "MasterFundInformation";
        public const string SECTION_availableTools = "AvailableTools";
        public const string SECTION_FutureExpirationMonthCodes = "FutureExpirationMonthCodes";
        public const string SECTION_DataHubTradeImport = "DataHubTradeImport";
        public const string SECTION_EsignalSymbolLimits = "EsignalSymbolLimits";
        public const string SECTION_OptionCallPutMonthCodes = "OptionCallPutMonthCodes";
        public const string SECTION_masterStrategyAssociation = "MasterStrategyInformation";
        public const string SECTION_regexExpression = "RegexExpression";
        public const string SECTION_PlugIns = "PlugIns";
        public const string SECTION_DefaultSwapParameters = "DefaultSwapParameters";
        public const string SECTION_NonUsableCurrencyForSwap = "NonUsableCurrencyForSwap";
        public const string SECTION_PBEBMappingForAllocation = "PBEBMappingForAllocation";
        public const string SECTION_ComplianceSettings = "Compliance";
        public const string SECTION_ThemeAndWhiteLabeling = "ThemeAndWhiteLabeling";
        public const string SECTION_CurrenciesToUpdate = "CurrenciesToUpdate";
        public const string SECTION_FactSetSettings = "FactSetSettings";
        public const string SECTION_ActivSettings = "ActivSettings";
        public const string SECTION_BloombergSettings = "BloombergSettings";
        public const string SECTION_SAPISettings = "SAPISettings";
        #endregion

        #region Appsetting Config keys
        // TODO : Have to add all the appsettings keys in memory
        public const string CONFIGKEY_LivePricesFileReadInterval = "LivePricesFileReadInterval";
        public const string CONFIGKEY_LatestLivePricesFilePath = "LatestLivePricesFilePath";
        public const string CONFIGKEY_AllPricesFileReadInterval = "AllPricesFileReadInterval";
        public const string CONFIGKEY_AllLivePricesFilePath = "AllLivePricesFilePath";
        public const string CONFIGKEY_PricingFileRetrievalMode = "PricingFileRetrievalMode";
        public const string CONFIG_APPSETTING_NoOfDaysAsCurrentForAllocation = "NoOfDaysAsCurrentForAllocation";
        public const string CONFIG_APPSETTING_AccountLockReleaseInterval = "AccountLockReleaseInterval";
        public const string CONFIG_APPSETTING_AutoApproveAmendments = "AutoApproveAmendments";
        public const string CONFIG_APPSETTING_IsApplyToleranceOnReconReport = "IsApplyToleranceOnReconReport";
        public const string CONFIG_APPSETTING_CashFlowCompressorKey = "CashFlowCompressorKey";
        public const string CONFIG_APPSETTING_ImportCleanUpInterval = "ImportCleanUpInterval";
        public const string CONFIG_APPSETTING_SymbologyForLivePricesFile = "SymbologyForLivePricesFile";

        public const string CONFIG_APPSETTING_FtpName = "FtpName";
        public const string CONFIG_APPSETTING_FtpType = "FtpType";
        public const string CONFIG_APPSETTING_Host = "Host";
        public const string CONFIG_APPSETTING_Port = "Port";
        public const string CONFIG_APPSETTING_UsePassive = "UsePassive";
        public const string CONFIG_APPSETTING_UserName = "UserName";
        public const string CONFIG_APPSETTING_Password = "Password";
        public const string CONFIG_APPSETTING_KeyFile = "KeyFile";
        public const string CONFIG_APPSETTING_PassPhrase = "PassPhrase";
        public const string CONFIG_APPSETTING_FXTickerSymbolDateFormat = "FXTickerSymbolDateFormat";
        public const string CONFIG_APPSETTING_FXBloombergSymbolDateFormat = "FXBloombergSymbolDateFormat";
        public const string CONFIG_APPSETTING_BlotterUpdateTimeInterval = "BlotterUpdateTimeInterval";
        
        public const string CONFIG_APPSETTING_SettlementCurrencySubAccounts = "SettlementCurrencySubAccounts";
        public const string CONFIG_DATAHUBTRADEIMPORT_SenderAddress = "MailSenderAddress";
        public const string CONFIG_DATAHUBTRADEIMPORT_SenderName = "MailSenderName";
        public const string CONFIG_DATAHUBTRADEIMPORT_RecieverAddress = "MailRecieverAddress";
        public const string CONFIG_DATAHUBTRADEIMPORT_HostName = "MailHostName";
        public const string CONFIG_DATAHUBTRADEIMPORT_EnableSSL = "MailEnableSSL";
        public const string CONFIG_DATAHUBTRADEIMPORT_Password = "MailPassword";
        public const string CONIFG_DATAHUBTRADEIMPORT_PORT = "MailPort";
        #endregion

        #region Config & Resource keys constants
        public const string CONFIGKEY_LiveFeed_Level1TimerStartDueTime = "Level1TimerStartDueTime";
        public const string CONFIGKEY_LiveFeed_TimerInterval = "TimerInterval";
        public const string CONFIGKEY_LiveFeed_Level1TimerIntervalMultiple = "Level1TimerIntervalMultiple";
        public const string CONFIGKEY_LiveFeed_OptionChainDataInterval = "OptionChainDataInterval";
        public const string CONFIGKEY_LiveFeed_EsignalDetails = "EsignalDetails";
        public const string CONFIGKEY_LiveFeed_ConnectionRetryDelay = "ConnectionRetryDelay";
        public const string CONFIGKEY_SendSymbolDataTimerInterval = "SendSymbolDataTimerInterval";

        public const string RESKEY_CSVParsingRegularExpression = "CSVParsingRegularExpression";
        public const string CONFIGKEY_availableModules_SecMaster = "Security Master";
        public const string CONFIGKEY_IsManualOrdersOverrideEnabled = "IsManualOrdersOverrideEnabled";
        public const string CONFIGKEY_IsPerformanceNumberColumnsEnabled = "IsPerformanceNumberColumnsEnabled";
        public const string CONFIGKEY_AllowedUserForScheduler = "AllowedUserForScheduler";
        public const string CONFIGKEY_PermissibleQuickTTInstances = "PermissibleQuickTTInstances";
        public const string CONFIGKEY_QuickTTValidationTimeout = "QuickTTValidationTimeout";
        public const string CONFIGKEY_PerformanceNumberColumnsForAccountOrMasterFund = "PerformanceNumberColumnsForAccountOrMasterFund";

        public const string CONFIGKEY_OptionChainMinNumberOfStrikes = "OptionChainMinNumberOfStrikes";
        public const string CONFIGKEY_OptionChainMaxNumberOfStrikes = "OptionChainMaxNumberOfStrikes";
        #endregion

        #region
        public const string HISTORICAL_DATA_CONNECTION_STRING = "HistoricalConnectionString";
        #endregion

        #region ComplianceConfigKeys
        //client
        public const string CONFIGKEY_RuleServer = "RuleServer";
        public const string CONFIGKEY_AmqpServer = "AmqpServer";
        public const string CONFIGKEY_Vhost = "VHost";
        public const string CONFIGKEY_VhostUserId = "vhostUserId";
        public const string CONFIGKEY_VhostPassword = "vhostPassword";
        public const string CONFIGKEY_RuleSixteenBExchangeName = "RuleSixteenBExchangeName";
        public const string CONFIGKEY_IsWashSaleEnabled = "IsWashSaleEnabled";
        public const string CONFIGKEY_ClientFeedBackOnOverrideRequest = "ClientFeedBackOnOverrideRequest";
        public const string CONFIGKEY_ClientFeedBackOnOverrideResponse = "ClientFeedBackOnOverrideResponse";
        public const string CONFIGKEY_RuleRequestExchange = "RuleRequestExchange";
        public const string CONFIGKEY_RuleResponseExchange = "RuleResponseExchange";
        public const string CONFIGKEY_NotificationExchange = "NotificationExchange";
        public const string CONFIGKEY_BasketComplianceExchange = "BasketComplianceExchangeName";

        //pricing
        public const string CONFIGKEY_LiveFeedQueue = "LiveFeedQueue";
        public const string CONFIGKEY_EsperRequestExchange = "EsperRequestExchange";
        public const string CONFIGKEY_OtherDataExchange = "OtherDataExchange";

        //server
        public const string CONFIGKEY_Is16BEnabled = "Is16BEnabled";
        public const string CONFIGKEY_OrderOutputQueue = "OrderOutputQueue";
        public const string CONFIGKEY_SymbolDataQueue = "SymbolDataQueue";
        public const string CONFIGKEY_SecurityDetailsQueue = "SecurityDetailsQueue";
        public const string CONFIGKEY_RuleResponseUserDefined = "RuleResponseUserDefined";
        public const string CONFIGKEY_RuleResponseSixteenB = "RuleResponseSixteenB";
        public const string CONFIGKEY_RuleResponseWashSale = "RuleResponseWashSale";
        public const string CONFIGKEY_OverrideRuleRequest = "OverrideRuleRequest";
        public const string CONFIGKEY_OverrideRuleResponse = "OverrideRuleResponse";
        public const string CONFIGKEY_BlockTradeOnFirstViolation = "BlockTradeOnFirstViolation";

        public const string CONFIGKEY_BasketComplianceRequestExchange = "BasketComplianceRequestExchange";
        public const string CONFIGKEY_BasketComplianceQueue = "BasketComplianceQueue";

        //expnl
        public const string CONFIGKEY_OrderQueueName = "OrderQueueName";
        public const string CONFIGKEY_LiveFeedExchangeName = "LiveFeedExchangeName";
        public const string CONFIGKEY_HistoricalPositionQueueName = "HistoricalPositionQueueName";
        public const string CONFIGKEY_RuleFeedSixteenBQueue = "RuleFeedSixteenBQueue";

        //RTPNL
        public const string CONFIGKEY_RtpnlCompressionsExchange = "RtpnlCompressionsExchange";
        public const string CONFIGKEY_PostCompressionsExchange = "PostCompressionsExchange";
        #endregion

        private static ConfigurationHelper _instance = null;
        private static readonly object _lockerObj = new object();

        #region Singleton Instance
        private ConfigurationHelper()
        {
            ///Will be called single time as only one instance will be build.
            LoadAllSections();
            InitResourceManager();
        }

        public static ConfigurationHelper Instance
        {
            get
            {
                //lock needed here as different threads(other than UI threads) also, 
                if (_instance == null)
                {
                    lock (_lockerObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new ConfigurationHelper();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        public string GetValueBySectionAndKey(string sectionName, string key)
        {
            try
            {
                switch (sectionName)
                {
                    case SECTION_availableModules:
                        return _availableModules[key];

                    case SECTION_futuresMultipliers:
                        return _futuresMultipliers[key];

                    case SECTION_RICExchangeMapping:
                        return _ricExchangeMapping[key];

                    case SECTION_PranaExchangeMapping:
                        return _pranaExchangeMapping[key];

                    case SECTION_RICFutureRootSymbolMapping:
                        return _ricFutureRootSymbolMapping[key];

                    case SECTION_RICFutureSymbolToExchange:
                        return _ricFutureSymbolToExchange[key];

                    case SECTION_LiveFeed:
                        return _liveFeedSection[key];

                    case SECTION_underlyingGroupings:
                        return _underlyingGroupings[key];

                    case SECTION_masterFundAssociation:
                        return _masterFundAssociation[key];

                    case SECTION_availableTools:
                        return _availableTools[key];

                    case SECTION_FutureExpirationMonthCodes:
                        return _futureExpirationMonthCodes[key];

                    case SECTION_DataHubTradeImport:
                        return _dataHubTradeImport[key];

                    case SECTION_EsignalSymbolLimits:
                        return _esignalSymbolLimits[key];

                    case SECTION_OptionCallPutMonthCodes:
                        return _optionCallPutMonthCodes[key];

                    case SECTION_masterStrategyAssociation:
                        return _masterStrategyAssociation[key];

                    case SECTION_regexExpression:
                        return _regexExpression[key];

                    case SECTION_PlugIns:
                        return _PlugIns[key];

                    case SECTION_DefaultSwapParameters:
                        return _defaultSwapParameters[key];

                    case SECTION_PBEBMappingForAllocation:
                        return _pbEbMappingForAllocation[key];

                    case SECTION_NonUsableCurrencyForSwap:
                        return _nonUsableCurrencyForSwap[key];

                    case SECTION_ComplianceSettings:
                        return _ComplianceSettings[key];

                    case SECTION_ThemeAndWhiteLabeling:
                        return _ThemeAndWhiteLabeling[key];

                    case SECTION_CurrenciesToUpdate:
                        return _CurrenciesToUpdate[key];

                    case SECTION_FactSetSettings:
                        return _factSetSettings[key];

                    case SECTION_BloombergSettings:
                        return _bloombergSettings[key];

                    case SECTION_ActivSettings:
                        return _activSettings[key];

                    case SECTION_SAPISettings:
                        return _sapiSettings[key];

                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// Gets the option call put month code key.
        /// </summary>
        /// <param name="OptionCallPutMonthCodeValue">string</param>
        /// <returns></returns>
        public string GetOptionCallPutMonthCodeKey(string OptionCallPutMonthCodeValue)
        {
            string result = string.Empty;
            try
            {
                foreach (string key in _optionCallPutMonthCodes.Keys)
                {
                    if (_optionCallPutMonthCodes[key] == OptionCallPutMonthCodeValue)
                    {
                        result = key;
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
            return result;
        }

        public void RefreshSection()
        {
            //TODO : Check the utility for this function
            //System.Configuration.ConfigurationManager.RefreshSection();
            try
            {
                LoadSectionBySectionName(SECTION_LiveFeed);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetAppSettingValueByKey(string key)
        {
            string value = string.Empty;
            try
            {
                value = _appSettings[key];
            }
            catch (Exception)
            {

                throw;
            }
            return value;
        }

        public string GetCSVParsingRegularExpression()
        {
            string value = string.Empty;
            try
            {
                value = _resManager.GetString(RESKEY_CSVParsingRegularExpression);
            }
            catch (Exception)
            {

                throw;
            }
            return value;
        }

        /// <summary>
        /// Only fetches the appsettings section
        /// </summary>
        public System.Collections.Specialized.NameValueCollection LoadSectionBySectionName(string sectionName)
        {
            try
            {
                switch (sectionName)
                {
                    case SECTION_availableModules:
                        return _availableModules = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_availableModules);

                    case SECTION_futuresMultipliers:
                        return _futuresMultipliers = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_futuresMultipliers);

                    case SECTION_RICExchangeMapping:
                        return _ricExchangeMapping = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_RICExchangeMapping);

                    case SECTION_PranaExchangeMapping:
                        return _pranaExchangeMapping = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_PranaExchangeMapping);

                    case SECTION_RICFutureRootSymbolMapping:
                        return _ricFutureRootSymbolMapping = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_RICFutureRootSymbolMapping);

                    case SECTION_RICFutureSymbolToExchange:
                        return _ricFutureSymbolToExchange = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_RICFutureSymbolToExchange);

                    case SECTION_LiveFeed:
                        return _liveFeedSection = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_LiveFeed);

                    case SECTION_underlyingGroupings:
                        return _underlyingGroupings = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_underlyingGroupings);

                    case SECTION_masterFundAssociation:
                        return _masterFundAssociation = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_masterFundAssociation);

                    case SECTION_availableTools:
                        return _availableTools = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_availableTools);

                    case SECTION_FutureExpirationMonthCodes:
                        return _futureExpirationMonthCodes = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_FutureExpirationMonthCodes);

                    case SECTION_DataHubTradeImport:
                        return _dataHubTradeImport = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_DataHubTradeImport);

                    case SECTION_EsignalSymbolLimits:
                        return _esignalSymbolLimits = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_EsignalSymbolLimits);

                    case SECTION_OptionCallPutMonthCodes:
                        return _optionCallPutMonthCodes = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_OptionCallPutMonthCodes);

                    case SECTION_masterStrategyAssociation:
                        return _masterStrategyAssociation = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_masterStrategyAssociation);

                    case SECTION_regexExpression:
                        return _regexExpression = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_regexExpression);

                    case SECTION_PlugIns:
                        return _PlugIns = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_PlugIns);

                    case SECTION_DefaultSwapParameters:
                        return _defaultSwapParameters = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_DefaultSwapParameters);

                    case SECTION_PBEBMappingForAllocation:
                        return _pbEbMappingForAllocation = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_PBEBMappingForAllocation);

                    case SECTION_NonUsableCurrencyForSwap:
                        return _nonUsableCurrencyForSwap = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_NonUsableCurrencyForSwap);

                    case SECTION_ComplianceSettings:
                        return _PlugIns = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_ComplianceSettings);

                    case SECTION_ThemeAndWhiteLabeling:
                        return _PlugIns = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_ThemeAndWhiteLabeling);

                    case SECTION_CurrenciesToUpdate:
                        return _CurrenciesToUpdate = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_CurrenciesToUpdate);

                    case SECTION_FactSetSettings:
                        return _factSetSettings = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_FactSetSettings);

                    case SECTION_ActivSettings:
                        return _activSettings = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_ActivSettings);

                    case SECTION_SAPISettings:
                        return _sapiSettings = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_SAPISettings);

                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        private void LoadAllSections()
        {
            try
            {
                _availableModules = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_availableModules);
                _futuresMultipliers = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_futuresMultipliers);
                _ricExchangeMapping = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_RICExchangeMapping);
                _pranaExchangeMapping = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_PranaExchangeMapping);
                _ricFutureRootSymbolMapping = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_RICFutureRootSymbolMapping);
                _ricFutureSymbolToExchange = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_RICFutureSymbolToExchange);
                _liveFeedSection = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_LiveFeed);
                _appSettings = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_AppSetting);
                _futuresMultipliers = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_underlyingGroupings);
                _masterFundAssociation = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_masterFundAssociation);
                _availableTools = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_availableTools);
                _futureExpirationMonthCodes = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_FutureExpirationMonthCodes);
                _dataHubTradeImport = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_DataHubTradeImport);
                _esignalSymbolLimits = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_EsignalSymbolLimits);
                _optionCallPutMonthCodes = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_OptionCallPutMonthCodes);
                _masterStrategyAssociation = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_masterStrategyAssociation);
                _regexExpression = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_regexExpression);
                _PlugIns = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_PlugIns);
                _defaultSwapParameters = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_DefaultSwapParameters);
                _pbEbMappingForAllocation = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_PBEBMappingForAllocation);
                _nonUsableCurrencyForSwap = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_NonUsableCurrencyForSwap);
                _ComplianceSettings = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_ComplianceSettings);
                _ThemeAndWhiteLabeling = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_ThemeAndWhiteLabeling);
                _factSetSettings = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_FactSetSettings);
                _bloombergSettings = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_BloombergSettings);
                _activSettings = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_ActivSettings);
                _sapiSettings = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection(SECTION_SAPISettings);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void InitResourceManager()
        {
            // Create the resource manager. 
            Assembly assembly = this.GetType().Assembly;
            _resManager = new ResourceManager("Prana.Global.Resource1", assembly);
        }

        public NameValueCollection GetSectionBySectionName(string sectionName)
        {
            try
            {
                switch (sectionName)
                {
                    case SECTION_FutureExpirationMonthCodes:
                        return _futureExpirationMonthCodes;

                    case SECTION_DataHubTradeImport:
                        return _dataHubTradeImport;

                    case SECTION_EsignalSymbolLimits:
                        return _esignalSymbolLimits;

                    case SECTION_OptionCallPutMonthCodes:
                        return _optionCallPutMonthCodes;

                    case SECTION_availableModules:
                        return _availableModules;

                    case SECTION_futuresMultipliers:
                        return _futuresMultipliers;

                    case SECTION_RICExchangeMapping:
                        return _ricExchangeMapping;

                    case SECTION_PranaExchangeMapping:
                        return _pranaExchangeMapping;

                    case SECTION_RICFutureRootSymbolMapping:
                        return _ricFutureRootSymbolMapping;

                    case SECTION_RICFutureSymbolToExchange:
                        return _ricFutureSymbolToExchange;

                    case SECTION_LiveFeed:
                        return _liveFeedSection;

                    case SECTION_underlyingGroupings:
                        return _underlyingGroupings;

                    case SECTION_masterFundAssociation:
                        return _masterFundAssociation;

                    case SECTION_availableTools:
                        return _availableTools;

                    case SECTION_masterStrategyAssociation:
                        return _masterStrategyAssociation;

                    case SECTION_regexExpression:
                        return _regexExpression;

                    case SECTION_PlugIns:
                        return _PlugIns;

                    case SECTION_DefaultSwapParameters:
                        return _defaultSwapParameters;

                    case SECTION_ComplianceSettings:
                        return _ComplianceSettings;

                    case SECTION_ThemeAndWhiteLabeling:
                        return _ThemeAndWhiteLabeling;

                    case SECTION_CurrenciesToUpdate:
                        return _CurrenciesToUpdate;

                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
    }
}
