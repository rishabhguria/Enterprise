package prana.esperCalculator.constants;

import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

/**
 * Contains all constants required for configuration
 * 
 * @author dewashish
 * 
 */
public class ConfigurationConstants {

	public static final String LOGGER_CONFIGURATION_PATH = "target/resources/conf/log4j2.xml";
	public static final String APPLICTION_CONF_PATH_ESPER = "target/resources/conf/prana-esperCalculator-Config.xml";
	public static final String APPLICTION_CONF_PATH_BASKET = "target/resources/conf/prana-basketCompliance-Config.xml";
	public static final String APPLICTION_CONF_PATH_BASKET_BATCH = "StartBasketComplianceService.bat";
	public static final String COMPILED_JAR_FILES = "target/resources/compiledJars/";

	public static String SECTION_APP_SETTINGS = "AppSettings";
	public static final String SECTION_CUSTOM_RULE_PATH = "CustomRuleConfigPaths";
	public static final String SECTION_EXCHANGE_LIST = "Exchanges";
	public static final String SECTION_QUEUE_LIST = "Queues";
	public static final String SECTION_STATEMENT_EXCHANGE = "StatementsToExchange";
	public static final String SECTION_STATEMENT_QUEUE = "StatementsToQueue";
	public static final String SECTION_NAMED_WINDOW = "NamedWindowList";
	public static final String SECTION_STATEMENT_LOGGING = "StatementsToLog";
	public static final String SECTION_STATEMENT_DETECTION = "StatementsForDetection";
	public static final String SECTION_DIVISOR_MEMORY_MONITOR = "DivisorMemoryMonitor";
	public static final String SECTION_GENERAL_SECTION = "GeneralSection";
	public static final String KEY_APP_SETTINGS_AMQPSERVER = "AmqpServer";
	public static final String KEY_APP_SETTINGS_AMQP_VHOST = "Vhost";
	public static final String KEY_APP_SETTINGS_BASKET="BasketCompliance";
	public static final String KEY_APP_SETTINGS_ESPER="EsperCalculation";
	public static final String KEY_APP_SETTINGS_RELEASE_MODE_BASKET="RELEASE_FOLDER_NAME_BASKET";
	public static final String KEY_APP_SETTINGS_RELEASE_MODE_ESPER="RELEASE_FOLDER_NAME_ESPER";
	public static final String KEY_APP_SETTINGS_AMQP_USERID = "UserId";
	public static final String KEY_APP_SETTINGS_AMQP_PASSWORD = "Password";
	public static final String KEY_APP_SETTINGS_EPL_DIRECTORY = "EplDirectory";
	public static final String KEY_APP_SETTINGS_COMMON_EPL_DIRECTORY = "CommonEplDirectory";
	public static final String KEY_APP_SETTINGS_EPL_DIRECTORY_EXPRESSIONS = "EplDirectoryExpressions";
	public static final String KEY_APP_SETTINGS_ESPER_CONFIG_FILE = "EsperConfigFile";
	public static final String KEY_APP_SETTINGS_BASKET_COMPLIANCE_CONFIG_FILE = "BasketComplianceConfigFile";
	public static final String KEY_APP_SETTINGS_WINDOW_DUMP_DIRECTORY = "WindowDumpDirectory";
	public static final String KEY_APP_SETTINGS_WINDOW_DETECTION_DUMP_DIRECTORY = "WindowDetectionDumpDirectory";
	public static final String KEY_APP_SETTINGS_SOCKET_LOCK_PORT = "SocketLockPort";
	public static final String KEY_APP_SETTINGS_TTL_LIVEFEED = "TTLForLiveFeedQueue";
	public static final String KEY_APP_SETTINGS_START_STATEMENT_MONITOR = "StartStatementMonitor";
	public static final String KEY_APP_SETTINGS_MONITORING_EPL = "MonitoringEpl";
	public static final String KEY_APP_SETTINGS_DETECTION_EPL = "DetectionEpl";
	public static final String KEY_APP_SETTINGS_INTERCEPTOR = "InterceptorDirectory";
	public static final String KEY_APP_SETTINGS_WINDOW_VALIDATOR = "WindowValidatorDirectory";
	public static final String KEY_APP_SETTINGS_PRICING_REQUIRED = "PricingRequired";
	public static final String KEY_APP_SETTINGS_SYMBOLS_REQUIRE_LOGGING = "SymbolsRequireLogging";
	public static final String KEY_APP_SETTINGS_DUMP_INTERVAL_FOR_DETECTION = "DumpIntervalForDetection";
	public static final String KEY_APP_SETTINGS_TAXLOT_HELPER_DELAY = "TaxlotHelperDelay";
	public static final String KEY_APP_SETTINGS_SYMBOL_DATA_HELPER_DELAY = "SymbolDataHelperDelay";
	public static final String KEY_APP_SETTINGS_MISSING_INFORMATION_REQUIRED = "MissingInformationRequired";
	public static final String KEY_APP_SETTINGS_SLEEP_POST_ON_WHAT_IF = "SleepPostOnWhatIf";
	public static final String KEY_APP_SETTINGS_IS_LIVE_MODE = "IsLiveMode";
	public static final String KEY_APP_SETTINGS_SECURITY_RETRY_COUNT = "SecurityRetryCount";
	public static final String KEY_APP_SETTINGS_EVENTS_LOGGING = "EventsLogging";
	public static final String KEY_APP_SETTINGS_SYMBOL_DATA_UPDATE_LOGGING = "SymbolDataUpdateLogging";
	public static final String KEY_APP_SETTINGS_MAIL_HOST_NAME = "HostName";
	public static final String KEY_APP_SETTINGS_MAIL_SMTP_PORT = "SMTPPort";
	public static final String KEY_APP_SETTINGS_MAIL_USER_NAME = "MailUserName";
	public static final String KEY_APP_SETTINGS_MAIL_PASSWORD = "MailPassword";
	public static final String KEY_APP_SETTINGS_MAIL_SENDER_NAME = "SenderName";
	public static final String KEY_APP_SETTINGS_MAIL_RECIEVER_TO = "RecieverTo";
	public static final String KEY_APP_SETTINGS_MAIL_RECIEVER_CC = "RecieverCC";
	public static final String KEY_APP_SETTINGS_MAIL_RECIEVER_BCC = "RecieverBCC";
	public static final String KEY_APP_SETTINGS_MAIL_BOUNCE_ADDRESS = "BounceAddress";
	public static final String KEY_EXCHANGE_LIVEFEED = "LiveFeedExchangeName";
	public static final String KEY_EXCHANGE_OTHERDATA = "OtherEventStreams";
	public static final String KEY_EXCHANGE_WHATIF_PORTFOLIO = "WhatIfPortfolioExchangeName";
	public static final String KEY_EXCHANGE_LOGGING = "LoggingExchangeName";
	public static final String KEY_EXCHANGE_NOTIFICATION = "NotificationExchangeName";
	public static final String KEY_EXCHANGE_ESPER_REQUEST = "EsperRequestExchangeName";
	public static final String KEY_EXCHANGE_BASKET_COMPLIANCE_EXCHANGE = "BasketComplianceRequestExchangeName";
	public static final String KEY_EXCHANGE_BASKET_COMPLIANCE_SYMBOL_EXCHANGE = "BasketComplianceRequestSymbolExchangeName";
	public static final String KEY_EXCHANGE_RTPNL_COMPRESSIONS_EXCHANGE = "RtpnlCompressionsExchangeName";
	public static final String KEY_QUEUE_LIVEFEED = "EsperLiveFeedQueue";
	public static final String KEY_QUEUE_ORDER = "OrderQueueName";
	public static final String KEY_QUEUE_SYMBOL_DATA = "SymbolDataQueueName";
	public static final String KEY_QUEUE_SECURITY_DETAILS = "SecurityDetailsQueueName";
	public static final String KEY_QUEUE_WHATIF_ORDER = "WhatIfTaxlotQueueName";
	public static final String KEY_QUEUE_BASKET_COMPLIANCE = "BasketComplianceQueueName";
	public static final String SIMPLE_DATE_FORMAT = "MM/dd/yyyy HH:mm:ss XXX";
	public static final String KEY_DIVISOR_MEMORY_MONITOR_MEMORY_CHECK_INTERVAL = "MemoryCheckInterval";
	public static final String KEY_DIVISOR_MEMORY_MONITOR_USED_MEMORY_PERCENTAGE = "UsedMemoryPercentage";
	public static final String KEY_DIVISOR_MEMORY_MONITOR_TIMER_INCREMENT = "TimerIncrement";
	public static final String KEY_DIVISOR_MEMORY_MONITOR_PERCENTAGE_INCREMENT = "PercentageIncrement";
	public static final String KEY_DIVISOR_MEMORY_MONITOR_MAX_INCREMENT_LIMIT = "MaxIncrementLimit";
	public static final String KEY_DIVISOR_MEMORY_MONITOR_TIME_TO_PERCENTAGE_RATIO = "TimeToPercentageRatio";
	public static final String KEY_POST_WITH_INMARKET_INSTAGE = "PostWithInMarketInStage";
	public static final String KEY_APP_SETTINGS_HEARTBEAT_TIMER = "HeartBeatTimer";
	public static final String KEY_APP_SETTINGS_PRICING_TIMEOUT = "PricingTimeout";
	public static final String CAPTION_KEY_POST = "'Post'";
	public static final String CAPTION_KEY_POST_AND_INMARKET = "'Post', 'InTradeMarket'";
	public static final String CAPTION_KEY_POSTMARKETANDSTAGE = "'Post', 'InTradeMarket', 'InTradeStage'";
	public static final String KEY_APP_SETTINGS_EVENTS_EMAIL_PERMSSION = "EmailErrorNotificationPermission";
	public static final String KEY_APP_SETTINGS_EVENTS_EMAIL_TIMEOUT = "EmailErrorNotificationTimeoutSeconds";
	public static Boolean EMAIL_ERROR_NOTIFICATION_PERMISSION = false;
	public static Integer EMAIL_ERROR_NOTIFICATION_TIMEOUT_SECONDS = 60;
	public static String INTERCEPTOR_FILE_NAME = "InterceptorFileName";
	public static final String WHAT_IF_SYMBOL_DATA = "WhatIfSymbolData";
	public static final String SYMBOL_VALIATED_FROM_TT = "SymbolValidatedFromTT";
	public static final String IS_BASKETCOMPLIANCE_ENABLED ="IsBasketComplianceEnabled";
	public static final String IS_BASKETCOMPLIANCE_STARTED ="IsBasketComplianceStarted";
	public static final String KEY_APP_SETTINGS_IS_EPL_COMPILATION_REQUIRED = "IsEPLCompilationRequired";
	public static final String KEY_APP_SETTINGS_NO_OF_DAYS_TO_COMPILE_EPL_AFTER = "NoOfDaysToCompileEPLAfter";
	public static final String KEY_APP_SETTINGS_PRECISION_AFTER_DECIMAL = "PrecisionAfterDecimal";
	public static String ROUTING_KEY_RULE_COMPRESSION_INFO = "SendRuleNameWithCompression";
	public static final String KEY_IS_SYMBOL_DATA_CHANGED = "IsSymbolDataChanged";
	public static final String KEY_APP_SETTINGS_IS_SYMBOL_DATA_EOM_REQUIRED = "IsSymbolDataEOMRequiredOnStalePrices";
	public static String ROUTING_KEY_FX_FWD_PRICE_AVAILABILITY = "FxFwdPriceAvailableInPricingInput";
	public static String ROUTING_KEY_DYNAMIC_UDA_CACHE = "DynamicUDACache";
	
	public static final String DATA_LOADED_FOR_STAGE = "DataLoadedForStage";
	public static final String DATA_LOADED_FOR_PST = "DataLoadedForPST";
	public static final String CEP_ENGINE_COMPLIANCE  = "NirvanaCEPEngine";
	public static final String CEP_ENGINE_BASKET_COMPLIANCE  = "NirvanaBasketComplianceCEPEngine";
	public static final String PST  = "PST";
	public static final String STAGE  = "Stage";
	
	/* 
	 * A threshold value is time interval at which if pst request come, then fresh market snshot fr esper is not required.
	 * **/
	public static final String KEY_THRESHOLD_TIME_INTERVAL_FOR_MARKET_SNAPSHOT = "ThresholdTimeIntervalForMarketSnapshotInSecs";
	/*
	 * This block sets the values of EMAIL_ERROR_NOTIFICATION_PERMISSION and
	 * EMAIL_ERROR_NOTIFICATION_TIMEOUT_SECONDS by fetching the values specified in
	 * configuration file of this project.
	 */
	static {
		try {
			EMAIL_ERROR_NOTIFICATION_PERMISSION = Boolean.parseBoolean(ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(SECTION_APP_SETTINGS, KEY_APP_SETTINGS_EVENTS_EMAIL_PERMSSION));

			EMAIL_ERROR_NOTIFICATION_TIMEOUT_SECONDS = Integer.parseInt(ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(SECTION_APP_SETTINGS, KEY_APP_SETTINGS_EVENTS_EMAIL_TIMEOUT));

			// Here we are providing values for error logging permissions in utility project
			prana.utility.configuration.ConfigurationHelper.EMAIL_ERROR_NOTIFICATION_PERMISSION = EMAIL_ERROR_NOTIFICATION_PERMISSION;
			prana.utility.configuration.ConfigurationHelper.EMAIL_ERROR_NOTIFICATION_TIMEOUT_SECONDS = EMAIL_ERROR_NOTIFICATION_TIMEOUT_SECONDS;
		} catch (Exception ex) {
			ex.printStackTrace();
			PranaLogManager.error(ex.getMessage(), ex);
		}
	}
}

