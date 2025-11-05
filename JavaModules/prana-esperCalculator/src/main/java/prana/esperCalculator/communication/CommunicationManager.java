package prana.esperCalculator.communication;

import java.util.ArrayList;

import prana.amqpAdapter.AmqpHelper;
import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpListenerCallback;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.esperCalculator.amqpCollectors.AmqpListenerBasketCompliance;
import prana.esperCalculator.amqpCollectors.AmqpListenerCallbackOtherData;
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
			initializeAmqpListenersAndCollectors();
			DataInitializationRequestProcessor.getInstance().startCommunicationProcess();
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/**
	 * This method initialize listeners
	 * 
	 * @throws Exception
	 */
	private static void initializeAmqpListenersAndCollectors() throws Exception {
		try {
			initializeOtherDataListener();
			initializeAmqpDataFlowCollectors();
			initializeRefreshListener();
			initializeImportExportListener();
		//	initializeWhatIfSymbolSnapshot();
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/*
	private static void initializeWhatIfSymbolSnapshot() {
		PranaLogManager.info("Initializing other event streams");
		try {
			String exchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST, ConfigurationConstants.KEY_EXCHANGE_OTHERDATA);
			ArrayList<String> routingKeyList = new ArrayList<>();
			routingKeyList.add(ConfigurationConstants.WHAT_IF_SYMBOL_DATA);
			routingKeyList.add(ConfigurationConstants.SYMBOL_VALIATED_FROM_TT);
			IAmqpListenerCallback amqpListenerCallbackWhatIfSnapshot = new AmqpListenerWhatIfSnapshot();
			AmqpHelper.startListener(exchangeName, ExchangeType.Direct, MediaType.Exchange, routingKeyList,
					amqpListenerCallbackWhatIfSnapshot, true);
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	 */
	public static void initializeBasketComplianceRequestExchange() {
		PranaLogManager.logOnly("Initializing Basket Compliance streams");
		try {
			String exchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST, ConfigurationConstants.KEY_EXCHANGE_BASKET_COMPLIANCE_EXCHANGE);
			ArrayList<String> routingKeyList = new ArrayList<>();
			routingKeyList.add("InitializationRequestForEsper");
			routingKeyList.add("EsperPostData");
			routingKeyList.add("EsperRunningStatus");
			routingKeyList.add("SymbolDataRequest");

			String symbolExchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST,
					ConfigurationConstants.KEY_EXCHANGE_BASKET_COMPLIANCE_SYMBOL_EXCHANGE);
			ArrayList<String> routingKeyListSymbolData = new ArrayList<>();
			routingKeyListSymbolData.add("SymbolDataWindowData");

			IAmqpListenerCallback amqpListenerBasketCompliance = new AmqpListenerBasketCompliance();
			AmqpHelper.startListener(exchangeName, ExchangeType.Direct, MediaType.Exchange, routingKeyList,
					amqpListenerBasketCompliance, true);

			IAmqpListenerCallback amqpListenerBasketComplianceSymbolData = new AmqpListenerBasketCompliance();
			AmqpHelper.startListener(symbolExchangeName, ExchangeType.Direct, MediaType.Exchange,
					routingKeyListSymbolData, amqpListenerBasketComplianceSymbolData, true);
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	private static void initializeImportExportListener() throws Exception {
		try {
			String exchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST, ConfigurationConstants.KEY_EXCHANGE_OTHERDATA);
			IAmqpSender amqpSender = AmqpHelper.getSender(exchangeName, ExchangeType.Direct, MediaType.Exchange, true);
			ArrayList<String> routingKeyList = new ArrayList<>();
			routingKeyList.add("ExportCustomRule");
			routingKeyList.add("ImportCustomRule");
			IAmqpListenerCallback importExportListener = new ImportExportListener(amqpSender);

			AmqpHelper.startListener(exchangeName, ExchangeType.Direct, MediaType.Exchange, routingKeyList,
					importExportListener, true);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
		}
	}

	/**
	 * This method initializes Refresh listener
	 * 
	 * @throws Exception
	 */
	private static void initializeRefreshListener() throws Exception {

		try {
			ArrayList<String> routingKeyListRefresh = new ArrayList<>();
			routingKeyListRefresh.add("RefreshData");
			IAmqpListenerCallback refreshListener = new RefreshListener();
			String exchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST, ConfigurationConstants.KEY_EXCHANGE_OTHERDATA);

			AmqpHelper.startListener(exchangeName, ExchangeType.Direct, MediaType.Exchange, routingKeyListRefresh,
					refreshListener, true);
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
			String RuleExchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
			ConfigurationConstants.SECTION_EXCHANGE_LIST, ConfigurationConstants.KEY_EXCHANGE_ESPER_REQUEST);
			ArrayList<String> ruleList = new ArrayList<>();
			ruleList.add(ConfigurationConstants.ROUTING_KEY_RULE_COMPRESSION_INFO);
			// XXX: Move these routing key list to config
			ArrayList<String> routingKeyList = new ArrayList<>();
			routingKeyList.add("YesterdayFxRates");
			routingKeyList.add("DbNav");
			routingKeyList.add("MarkPrice");
			routingKeyList.add("CashEvents");
			routingKeyList.add("AuecLocalDates");
			routingKeyList.add("LiveFeedStatus");
			routingKeyList.add("AccountCollection");
			routingKeyList.add("DayEndCash");
			routingKeyList.add("Security");
			routingKeyList.add("AuecDetails");
			routingKeyList.add("OrderSides");
			routingKeyList.add("YesterdayFxRates");
			routingKeyList.add("CounterPartyCollection");
			routingKeyList.add("VenueCollection");
			routingKeyList.add("InitCompleteInfo");
			routingKeyList.add(ConfigurationConstants.ROUTING_KEY_RULE_COMPRESSION_INFO);
			routingKeyList.add("HistoricalTaxlots");
			routingKeyList.add("Preferences");
			routingKeyList.add("AccountNavPreferences");
			routingKeyList.add("StrategyCollection");
			routingKeyList.add("OrderTypeTags");
			routingKeyList.add("Currency");
			routingKeyList.add("YearlyHolidaysEvent");
			routingKeyList.add("WeeklyHolidaysEvent");
			routingKeyList.add("CustomRuleRequest");
			routingKeyList.add("HistoricalTaxlotCompleted");
			routingKeyList.add("InitialLiveFeed");
			routingKeyList.add("BetaForSymbol");
			routingKeyList.add("PMCalculationPrefs");
			routingKeyList.add("StartOfMonthCapitalAccount");
			routingKeyList.add("UserDefinedMTDPnl");
			routingKeyList.add("AccountWiseNRA");
			routingKeyList.add("AvgVolCustomDays");
			routingKeyList.add("AccrualForAccount");
			routingKeyList.add("StartOfDayAccrualForAccount");
			routingKeyList.add("DayAccrualForAccount");
			routingKeyList.add("DailyCreditLimit");
			routingKeyList.add("InTradeMarket");
			routingKeyList.add("CalculationRequest");
			routingKeyList.add("InTradeStage");
			routingKeyList.add("InitRequestCalculationService");
			routingKeyList.add("CommunicationResponseForEsper");
			routingKeyList.add("SymbolDataTimerInterval");
			routingKeyList.add("FxFwdPriceAvailableInPricingInput");
			routingKeyList.add("DeleteTaxlotFromEsper");

			// Currently only one thread is being used
			IAmqpListenerCallback amqpListenerCallbackOtherData = new AmqpListenerCallbackOtherData();
			AmqpHelper.startListener(exchangeName, ExchangeType.Direct, MediaType.Exchange, routingKeyList,
					amqpListenerCallbackOtherData, true);

			IAmqpListenerCallback amqpListenerCallbackRuleName = new AmqpListenerCallbackOtherData();
			AmqpHelper.startListener(RuleExchangeName, ExchangeType.Direct, MediaType.Exchange, ruleList,
					amqpListenerCallbackRuleName, true);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/**
	 * This method initializes data flow for amqp listener
	 * 
	 * @throws Exception
	 */
	private static void initializeAmqpDataFlowCollectors() throws Exception {
		try {
			AmqpCollectorHelper.initializeOrderCollector();
			AmqpCollectorHelper.initializeWhatIfOrderCollector();
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
}
