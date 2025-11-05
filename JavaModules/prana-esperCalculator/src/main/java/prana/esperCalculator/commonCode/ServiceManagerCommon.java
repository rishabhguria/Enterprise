package prana.esperCalculator.commonCode;

import prana.amqpAdapter.AmqpHelper;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.shell.ShellManager;
import prana.utility.configuration.ApplicationHelper;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

public class ServiceManagerCommon {
	
	/**
	 * This method set up application instance such as port no
	 * 
	 * @throws Exception
	 */
	public static void setUpApplicationEnvironment() throws Exception {
		try {
			ApplicationHelper.lockPortForSingleInstance(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_SOCKET_LOCK_PORT));
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/**
	 * Initializes the shell
	 * 
	 * @throws Exception
	 *             Throws exception if could not start shell
	 */
	public static void initializeShell() throws Exception {
		try {
			Thread tShell = new Thread(ShellManager.getInstance());
			tShell.start();
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
	public static void initailizeJSONMapper() throws Exception {
		try {
			JSONMapper.initializeMapperForEsperCalculator(ConfigurationConstants.SIMPLE_DATE_FORMAT);
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
	public static void initializeAmqpService() throws Exception {

		try {
			String hostName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQPSERVER);
			String vhost = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);
			String userId = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_USERID);
			String password = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_PASSWORD);

			AmqpHelper.initialize(hostName, vhost, userId, password);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
}
