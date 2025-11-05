package prana.ruleEngineMediator.main;

import java.net.HttpURLConnection;
import java.net.URL;
import java.util.Timer;

import prana.amqpAdapter.AmpqHeartbeat;
import prana.amqpAdapter.AmqpHelper;
import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.ruleEngineMediator.communication.CommunicationManager;
import prana.ruleEngineMediator.constants.ConfigurationConstants;
import prana.ruleEngineMediator.ruleService.RuleServiceManager;
import prana.utility.configuration.ApplicationHelper;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

public class ServiceManager {

	public static void initializeService() throws Exception {
		try {
			initializeComponents();
			setUpApplicationEnvironment();
			initializeAmqpService();
			initailizeJSONMapper();

			if (!checkForDrools()) {
				// wait till jBoss has started
				
				int sleepInterval = Integer
						.parseInt(ConfigurationHelper
								.getInstance()
								.getValueBySectionAndKey(
										ConfigurationConstants.SECTION_APP_SETTINGS,
										ConfigurationConstants.KEY_APP_SETTINGS_HEARTBEAT_TIMER));


				PranaLogManager
						.error("jBoss not yet started.\n"
								+ "Rule Mediator will automatically resume start-up once jBoss starts.\n"
								+ "Waiting for jBoss...", null);

				while (true) {
					if (checkForDrools())
						break;
					Thread.sleep(sleepInterval);
				}
			}

			initializeRuleService();
			intializeCommunicationServices();
			startHeartbeat();
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	/**
	 * /** This method checks is Drools service is started
	 * 
	 * @return
	 */
	private static boolean checkForDrools() {

		try {
			String ruleUrl = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER);

			final URL url = new URL("http://" + ruleUrl);
			final HttpURLConnection urlConn = (HttpURLConnection) url
					.openConnection();
			urlConn.setConnectTimeout(1000 * 10);
			urlConn.connect();
			if (urlConn.getResponseCode() == HttpURLConnection.HTTP_OK) {
				PranaLogManager.info("Connected to " + ruleUrl + "\n");
				return true;
			} else
				return false;

		} catch (Exception ex) {
			PranaLogManager.logOnly(ex.getMessage());
			return false;
		}
	}

	/**
	 * starts a heart-beat sender for Rule-Mediator
	 * 
	 * @throws Exception
	 */
	private static void startHeartbeat() throws Exception {

		try {
			int heartbeatintervel = Integer
					.parseInt(ConfigurationHelper
							.getInstance()
							.getValueBySectionAndKey(
									ConfigurationConstants.SECTION_APP_SETTINGS,
									ConfigurationConstants.KEY_APP_SETTINGS_HEARTBEAT_TIMER));

			String exchange = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_EXCHANGE_LIST,
							ConfigurationConstants.KEY_EXCHANGE_OTHER_DATA);

			IAmqpSender _ampqHeartbeatSeander = AmqpHelper.getSender(exchange,
					ExchangeType.Direct, MediaType.Exchange, false);

			AmpqHeartbeat beater = new AmpqHeartbeat("RuleMediator",
					heartbeatintervel, _ampqHeartbeatSeander);

			Timer timer = new Timer();
			timer.schedule(beater, 0, heartbeatintervel);
			PranaLogManager.info("Rule Mediator Heartbeat Service started.");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Initializes required communications
	 * 
	 * @throws Exception
	 */
	private static void intializeCommunicationServices() throws Exception {
		try {
			CommunicationManager.initializeCommunication();
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	/**
	 * Initializes rule processing services
	 * 
	 * @throws Exception
	 */
	private static void initializeRuleService() throws Exception {
		try {
			RuleServiceManager.initializeRuleService();
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	/**
	 * Initializes AMQP services
	 * 
	 * @throws Exception
	 *             throws exception if could not initialize AMQP
	 */
	private static void initializeAmqpService() throws Exception {
		try {
			String hostName = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_AMQPSERVER);
			String vhost = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);
			String userId = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_AMQP_USERID);
			String password = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_AMQP_PASSWORD);

			AmqpHelper.initialize(hostName, vhost, userId, password);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	/**
	 * Sets up Object mapper and make it usable
	 * 
	 * @throws Exception
	 *             throws exception if could not set up Objcet mapper
	 */
	private static void initailizeJSONMapper() throws Exception {
		try {
			JSONMapper
					.initializeMapperForRuleMediator(ConfigurationConstants.SIMPLE_DATE_FORMAT_1);

		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	/**
	 * This method set up application instance such as port no
	 * 
	 * @throws Exception
	 */
	private static void setUpApplicationEnvironment() throws Exception {
		try {
			ApplicationHelper
					.lockPortForSingleInstance(ConfigurationHelper
							.getInstance()
							.getValueBySectionAndKey(
									ConfigurationConstants.SECTION_APP_SETTINGS,
									ConfigurationConstants.KEY_APP_SETTINGS_SOCKET_LOCK_PORT));
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/**
	 * Initializes other components as logger and configuration
	 * 
	 * @throws Exception
	 *             throws exception if could not initialize Logger and
	 *             configuration
	 */
	private static void initializeComponents() throws Exception {

		try {
			PranaLogManager
					.initializeLogger(ConfigurationConstants.LOGGER_CONFIGURATION_PATH);
			ConfigurationHelper.getInstance().initializeConfiguration(
					ConfigurationConstants.APPLICTION_CONF_PATH);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
}
