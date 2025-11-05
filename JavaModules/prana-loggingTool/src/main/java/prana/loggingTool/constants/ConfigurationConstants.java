package prana.loggingTool.constants;

public class ConfigurationConstants {
	public static final String LOGGER_CONFIGURATION_PATH = "target/resources/conf/log4j2.xml";
	public static final String APPLICTION_CONF_PATH = "target/resources/conf/prana-loggingTool-Config.xml";

	public static final String SECTION_APP_SETTINGS = "AppSettings";
	public static final String SECTION_EXCHANGE_LIST = "Exchanges";
	public static final String SECTION_QUEUE_LIST = "Queues";

	public static final String KEY_APP_SETTINGS_AMQPSERVER = "AmqpServer";
	public static final String KEY_APP_SETTINGS_AMQP_VHOST = "Vhost";
	public static final String KEY_APP_SETTINGS_AMQP_USERID = "UserId";
	public static final String KEY_APP_SETTINGS_AMQP_PASSWORD = "Password";
	public static final String KEY_APP_SETTINGS_EVENTS_LOGGING = "EventsLogging";

	public static final String KEY_EXCHANGE_PRE_TRADE_RECIEVER = "PreTradeExchangeName";
	public static final String KEY_QUEUE_PRE_TRADE_VALIDATION = "PreTradeValidationQueue";

	public static final String KEY_APP_SETTINGS_SOCKET_LOCK_PORT = "SocketLockPort";

	public static final String SIMPLE_DATE_FORMAT_1 = "EEE MMM dd HH:mm:ss z yyyy";
	public static final String SIMPLE_DATE_FORMAT_2 = "M/d/yyyy HH:mm:ss";

	public static String PRE_TRADE_COMPLIANCE = "PreTradeCompliance";
	public static String POST_TRADE_COMPLIANCE = "PostTradeCompliance";

	public static final String KEY_APP_SETTINGS_HEARTBEAT_TIMER = "HeartBeatTimer";

}
