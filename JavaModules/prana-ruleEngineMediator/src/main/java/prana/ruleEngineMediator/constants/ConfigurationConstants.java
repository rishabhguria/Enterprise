package prana.ruleEngineMediator.constants;

import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

public class ConfigurationConstants {
	
	public static final String LOGGER_CONFIGURATION_PATH = "target/resources/conf/log4j2.xml";
	public static final String APPLICTION_CONF_PATH = "target/resources/conf/prana-ruleMediator-Config.xml";

	public static String SECTION_APP_SETTINGS = "AppSettings";
	public static final String SECTION_EXCHANGE_LIST = "Exchanges";
	public static final String SECTION_QUEUE_LIST = "Queues";

	public static final String KEY_APP_SETTINGS_AMQPSERVER = "AmqpServer";
	public static final String KEY_APP_SETTINGS_AMQP_VHOST = "Vhost";
	public static final String KEY_APP_SETTINGS_AMQP_USERID = "UserId";
	public static final String KEY_APP_SETTINGS_AMQP_PASSWORD = "Password";

	public static final String KEY_APP_SETTINGS_RULESERVER = "RuleServer";
	public static final String KEY_APP_SETTINGS_RULESERVER_USERID = "RuleServerUserId";
	public static final String KEY_APP_SETTINGS_RULESERVER_PASSWORD = "RuleServerPassword";
	public static final String KEY_APP_SETTINGS_RULE_TEMPLATE = "RuleTemplate";

	public static final String KEY_EXCHANGE_POST_TRADE_RECIEVER = "PostTradeExchangeName";
	public static final String KEY_EXCHANGE_PRE_TRADE_RECIEVER = "PreTradeExchangeName";
	public static final String KEY_EXCHANGE_OTHER_DATA = "OtherEventStreams";
	public static final String KEY_EXCHANGE_BUILD_REQUEST = "BuildRequestExchangeName";
	public static final String KEY_EXCHANGE_BUILD_RESPONSE = "BuildResponseExchangeName";
	public static final String KEY_EXCHANGE_NOTIFICATION = "NotificationExchangeName";
	public static final String KEY_EXCHANGE_ESPER_REQUEST = "EsperRequestExchangeName";
	public static final String KEY_EXCHANGE_BASKET_COMPLIANCE_EXCHANGE = "BasketComplianceRequestExchangeName";

	public static final String KEY_QUEUE_PRE_TRADE_VALIDATION = "PreTradeValidationQueue";

	public static final String KEY_APP_SETTINGS_SOCKET_LOCK_PORT = "SocketLockPort";

	public static final String SIMPLE_DATE_FORMAT_1 = "EEE MMM dd HH:mm:ss z yyyy";
	public static final String SIMPLE_DATE_FORMAT_2 = "M/d/yyyy HH:mm:ss";

	public static String PRE_TRADE_COMPLIANCE = "PreTradeCompliance";
	public static String POST_TRADE_COMPLIANCE = "PostTradeCompliance";
	public static String BASKET_COMPLIANCE = "BasketCompliance";
	public static String ROUTING_KEY_RULE_COMPRESSION_INFO = "SendRuleNameWithCompression";

	public static final String KEY_APP_SETTINGS_HEARTBEAT_TIMER = "HeartBeatTimer";
	public static final String KEY_APP_SETTINGS_IS_ALLOWED_MARGE_IN_PARAMETER = "IsAllowedMargeInParameter";
	
	public static final String KEY_APP_SETTINGS_EVENTS_EMAIL_PERMSSION = "EmailErrorNotificationPermission";
	public static final String KEY_APP_SETTINGS_EVENTS_EMAIL_TIMEOUT = "EmailErrorNotificationTimeoutSeconds";
	
	public static Boolean EMAIL_ERROR_NOTIFICATION_PERMISSION = false;
	public static Integer EMAIL_ERROR_NOTIFICATION_TIMEOUT_SECONDS = 60;
	
	public static final String KEY_VALUE_SEPARATOR = "~";
	
	/*
	 * This block sets the values of EMAIL_ERROR_NOTIFICATION_PERMISSION and EMAIL_ERROR_NOTIFICATION_TIMEOUT_SECONDS
	 * by fetching the values specified in configuration file of this project.
	 */
	static{
		try {
			EMAIL_ERROR_NOTIFICATION_PERMISSION = Boolean.parseBoolean(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					SECTION_APP_SETTINGS,KEY_APP_SETTINGS_EVENTS_EMAIL_PERMSSION));
		
			EMAIL_ERROR_NOTIFICATION_TIMEOUT_SECONDS = Integer.parseInt(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					SECTION_APP_SETTINGS,KEY_APP_SETTINGS_EVENTS_EMAIL_TIMEOUT));
			
			//Here we are providing values for error logging permissions in utility project
			prana.utility.configuration.ConfigurationHelper.EMAIL_ERROR_NOTIFICATION_PERMISSION = EMAIL_ERROR_NOTIFICATION_PERMISSION;
			prana.utility.configuration.ConfigurationHelper.EMAIL_ERROR_NOTIFICATION_TIMEOUT_SECONDS = EMAIL_ERROR_NOTIFICATION_TIMEOUT_SECONDS;
		} catch (Exception ex) {
			// TODO Auto-generated catch block
			ex.printStackTrace();
			PranaLogManager.error(ex.getMessage(), ex);
		}
	}
}
