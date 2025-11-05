package prana.basketComplianceService.communication;

import java.util.ArrayList;

import prana.amqpAdapter.AmqpHelper;
import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpListenerCallback;  
import prana.basketComplianceService.amqpCollectors.AmqpListenerCallbackBasketCompliance;
import prana.basketComplianceService.amqpCollectors.AmqpListenerCallbackOtherData;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

/**
 * This is main exposed class to handle all communication related actions
 * 
 * @author dewashish
 * 
 */
public class CommunicationManager {

	/**
	 * This method start listener and initialize communication
	 * 
	 * @throws Exception
	 */
	public static void initializeCommunication() throws Exception {
		try {
			initializeBasketComplianceListener();
			AmqpCollectorHelper.initializeBasketComplianceOrderCollector();
			DataInitializationRequestProcessor.getInstance().startCommunicationProcess();
			initializeOtherDataListener();
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
	
	/**
	 * This method initializes other data listener with defined routing keys
	 * 
	 * @throws Exception
	 */
	private static void initializeBasketComplianceListener() throws Exception {
		try {
			PranaLogManager.info("Initializing Basket Compliance streams");

			String exchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST, ConfigurationConstants.KEY_EXCHANGE_BASKET_COMPLIANCE_EXCHANGE);
			IAmqpListenerCallback basketComplianceListener = new AmqpListenerCallbackBasketCompliance();

			String symbolExchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST,
					ConfigurationConstants.KEY_EXCHANGE_BASKET_COMPLIANCE_SYMBOL_EXCHANGE);
			ArrayList<String> routingKeyListSymbolData = new ArrayList<>();
			routingKeyListSymbolData.add("SymbolDataWindowData");

			ArrayList<String> routingKeyList = new ArrayList<>();
			routingKeyList.add("InitCompleteInfo");
			routingKeyList.add(ConfigurationConstants.ROUTING_KEY_RULE_COMPRESSION_INFO);
			routingKeyList.add("AccountDivisorWindowData");
			routingKeyList.add("MasterFundDivisorWindowData");
			routingKeyList.add("GlobalDivisorWindowData");
			routingKeyList.add("TaxlotWindowData");
			routingKeyList.add("DeleteTaxlotData");
			routingKeyList.add("PmCalculationPreferenceWindowData");			
			routingKeyList.add("AccountCollection");
			routingKeyList.add("AuecDetails");
			routingKeyList.add("Security");
			routingKeyList.add("StrategyCollection");
			routingKeyList.add("DataSentFromEsper");
			routingKeyList.add("SymbolDataEOMSent");
			routingKeyList.add("WeeklyHolidaysEvent");
			routingKeyList.add("YearlyHolidaysEvent");
			routingKeyList.add("SymbolDataWindowDataInit");
			routingKeyList.add("Permissions");
			routingKeyList.add("DayEndCashAccount");
			routingKeyList.add("AccountNavPreference");
			routingKeyList.add("AccrualForAccount");
			routingKeyList.add("DbNav");
			routingKeyList.add("NavAndStartingPositionOfAccountsResponse"); 
			routingKeyList.add("NavAndStartingPositionOfAccountsRequest"); 
			AmqpHelper.startListener(exchangeName, ExchangeType.Direct, MediaType.Exchange, routingKeyList,
					basketComplianceListener, true);

			IAmqpListenerCallback basketComplianceListenerSymbolData = new AmqpListenerCallbackBasketCompliance();
			AmqpHelper.startListener(symbolExchangeName, ExchangeType.Direct, MediaType.Exchange,
					routingKeyListSymbolData, basketComplianceListenerSymbolData, true);

		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
	
	/**
	 * This method initializes other data listener with defined routing keys
	 * 
	 * @throws Exception
	 */
	private static void initializeOtherDataListener() throws Exception {
		try {
			PranaLogManager.info("Initializing other event streams");

			String exchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST, ConfigurationConstants.KEY_EXCHANGE_OTHERDATA);
			ArrayList<String> routingKeyList = new ArrayList<>();
			routingKeyList.add("CustomRuleRequest");
			routingKeyList.add("CashFlow");
			routingKeyList.add("SimulationPreference");
			
			// Currently only one thread is being used
			IAmqpListenerCallback amqpListenerCallbackOtherData = new AmqpListenerCallbackOtherData();
			AmqpHelper.startListener(exchangeName, ExchangeType.Direct, MediaType.Exchange, routingKeyList,
					amqpListenerCallbackOtherData, true);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
}
