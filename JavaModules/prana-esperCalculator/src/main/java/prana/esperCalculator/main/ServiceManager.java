package prana.esperCalculator.main;

import java.util.Timer;

import prana.amqpAdapter.AmpqHeartbeat;
import prana.amqpAdapter.AmqpHelper;
import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.esperCalculator.commonCode.ServiceManagerCommon;
import prana.esperCalculator.communication.CommunicationManager;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.esperCEP.EsperCEPManager;
import prana.esperCalculator.esperCEP.SymbolDataHelper;
import prana.esperCalculator.esperCEP.TaxlotHelper;
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
			setUncaughtExceptionHandler();
			ServiceManagerCommon.initializeAmqpService();
			ServiceManagerCommon.initailizeJSONMapper();
			initailizeEsperCEPEngine();
			ServiceManagerCommon.initializeShell();
			initializeCommunication();
			startHeartbeat();
			initializeInterceptor();
			initializeWindowValidator();
			initializeDualCache();
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/**
	 * Start the validator to load files
	 */
	private static void initializeWindowValidator() {
		try {
			String validatorQueryPath = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_WINDOW_VALIDATOR);
			WindowValidator.getInstance().initialize(validatorQueryPath);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Start the intercepter service to load files
	 */
	private static void initializeInterceptor() {
		try {
			String interceptorPath = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_INTERCEPTOR);

			Interceptor interceptor = new Interceptor(interceptorPath);
			Thread interceptorThread = new Thread(interceptor);
			interceptorThread.setName("InterceptorThread");
			interceptorThread.start();

		} catch (Exception ex) {
			PranaLogManager.error(ex);
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
			AmpqHeartbeat beater = new AmpqHeartbeat("Esper", heartbeatintervel, _ampqHeartbeatSeander);
			Timer timer = new Timer();
			timer.schedule(beater, 0, heartbeatintervel);
			PranaLogManager.info("Esper Heartbeat Service started.");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Sets exception-handler for uncaught exceptions
	 * 
	 * @throws Exception
	 */
	private static void setUncaughtExceptionHandler() throws Exception {
		try {
			Thread.setDefaultUncaughtExceptionHandler(new DefaultUncaughtExceptionHandler());
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
	/**
	 * Initializes dual cache implemented for symboldata, as consumption speed is
	 * slower than producer
	 * 
	 * @throws Exception
	 *             Throws exception if could not start communication
	 */
	private static void initializeDualCache() throws Exception {
		try {
			/*String priceRequired = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_PRICING_REQUIRED);

			if (priceRequired.length() != 0) {*/
				SymbolDataHelper symbolDataHelper = new SymbolDataHelper();
				Thread threadSymbolDataHelper = new Thread(symbolDataHelper);
				threadSymbolDataHelper.setName("SymbolDataHelperThread");
				threadSymbolDataHelper.start();
			// }

			// Starting taxlot helper which will send data to espr engine
			TaxlotHelper taxlotHelper = new TaxlotHelper();
			Thread threadTaxlot = new Thread(taxlotHelper);
			threadTaxlot.setName("TaxlotHelperThread");
			threadTaxlot.start();
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
	 * Initializes Epser CEP engine
	 * 
	 * @throws Exception
	 *             Throws exception if could not start CEP engine
	 */
	private static void initailizeEsperCEPEngine() throws Exception {
		try {
			EsperCEPManager.initializeCepEngine();
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
			ConfigurationHelper.getInstance()
					.initializeConfiguration(ConfigurationConstants.APPLICTION_CONF_PATH_ESPER);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
}
