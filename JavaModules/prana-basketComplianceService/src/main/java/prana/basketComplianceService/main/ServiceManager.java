package prana.basketComplianceService.main;

import java.util.Timer;

import prana.amqpAdapter.AmpqHeartbeat;
import prana.amqpAdapter.AmqpHelper;
import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.basketComplianceService.basketCEP.BasketComplianceCEPManager;
import prana.basketComplianceService.communication.CommunicationManager;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.commonCode.CEPManager;
import prana.esperCalculator.commonCode.ServiceManagerCommon;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

/**
 * Service pattern is implemented
 * 
 * @author dewashish
 * 
 */
public class ServiceManager {

	/**
	 * Initializes all services required for esper calculation engine
	 * 
	 * @throws Exception
	 *             Throws if any error occurs in service initialization
	 */
	static void initializeService() throws Exception {

		try {
			initializeComponents();
			ServiceManagerCommon.setUpApplicationEnvironment();
			ServiceManagerCommon.initializeAmqpService();
			ServiceManagerCommon.initailizeJSONMapper();
			initailizeBasketComplianceCEPEngine();
			ServiceManagerCommon.initializeShell();
			initializeCommunication();
			startHeartbeat();
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/**
	 * Initializes all communication connection required for esper
	 * 
	 * @throws Exception
	 *             Throws exception if could not start communication
	 */
	private static void initializeCommunication() throws Exception {
		try {
			CommunicationManager.initializeCommunication();
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/**
	 * starts a heart-beat sender for Esper
	 * 
	 * @throws Exception
	 */
	private static void startHeartbeat() throws Exception {
		try {
			int heartbeatintervel = Integer.parseInt(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_HEARTBEAT_TIMER));
			String exchange = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST, ConfigurationConstants.KEY_EXCHANGE_OTHERDATA);

			IAmqpSender _ampqHeartbeatSeander = AmqpHelper.getSender(exchange, ExchangeType.Direct, MediaType.Exchange,
					false);
			AmpqHeartbeat beater = new AmpqHeartbeat("BasketCompliance", heartbeatintervel, _ampqHeartbeatSeander);
			Timer timer = new Timer();
			timer.schedule(beater, 0, heartbeatintervel);
			PranaLogManager.info("BasketCompliance Heartbeat Service started.");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
	
	/**
	 * Initializes Epser CEP engine
	 * 
	 * @throws Exception
	 *             Throws exception if could not start CEP engine
	 */
	private static void initailizeBasketComplianceCEPEngine() throws Exception {
		try {
			BasketComplianceCEPManager.initializeCepEngine();
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/**
	 * Initializes other components as logger and configuration
	 * 
	 * @throws Exception
	 *             throws exception if could not initialize Logger and configuration
	 */
	private static void initializeComponents() throws Exception {

		try {
			PranaLogManager.initializeLogger(ConfigurationConstants.LOGGER_CONFIGURATION_PATH);
			ConfigurationHelper.getInstance().initializeConfiguration(ConfigurationConstants.APPLICTION_CONF_PATH_BASKET);
			//loading Custom rules config path from esper config
			ConfigurationHelper.getInstance().loadCustomRuleConfigFromEsper(CEPManager.getEsperDirectoryPath(ConfigurationConstants.APPLICTION_CONF_PATH_ESPER));
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
}
