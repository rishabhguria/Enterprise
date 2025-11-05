package prana.loggingTool.main;

import prana.amqpAdapter.AmqpHelper;
import prana.loggingTool.communication.CommunicationManager;
import prana.loggingTool.constants.ConfigurationConstants;
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

		

			
			intializeCommunicationServices();
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
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
