package prana.ruleEngineMediator.communication;

import java.util.ArrayList;

import prana.amqpAdapter.AmqpHelper;
import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpListenerCallback;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.businessObjects.rule.RuleType;
import prana.ruleEngineMediator.constants.ComplianceLevelConstants;
import prana.ruleEngineMediator.constants.ConfigurationConstants;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

public class AmqpListenerHelper {

	static void intializeAllAmqpListeners() throws Exception {

		try {
			initializeRuleValidationListener();
			initailizeBuildListener();
			initializeCommunicationListener();
			initializeImportExportListener();
			initializeGroupOperationListener();
			initializeCommunicationListenerForBasketCompliance();
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
	
	private static void initializeCommunicationListenerForBasketCompliance() throws Exception {
		try {
			String exchangeName = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_EXCHANGE_LIST,
							ConfigurationConstants.KEY_EXCHANGE_BASKET_COMPLIANCE_EXCHANGE);

			IAmqpSender amqpSender = AmqpHelper.getSender(exchangeName,
					ExchangeType.Direct, MediaType.Exchange, false);

			ArrayList<String> routingKeyListInit = new ArrayList<>();
			routingKeyListInit.add("InitializationRequestForRuleMediator");
			AmqpListenerEsperRequest amqpListenerBasketCompliance = new AmqpListenerEsperRequest(
					amqpSender);

			AmqpHelper.startListener(exchangeName, ExchangeType.Direct,
					MediaType.Exchange, routingKeyListInit,
					amqpListenerBasketCompliance, false);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	private static void initializeGroupOperationListener() throws Exception {
		try {
                       String exchange = ConfigurationHelper.getInstance()
				.getValueBySectionAndKey(
						ConfigurationConstants.SECTION_EXCHANGE_LIST,
						ConfigurationConstants.KEY_EXCHANGE_OTHER_DATA);
		IAmqpSender amqpSender = AmqpHelper.getSender(exchange,
				ExchangeType.Direct, MediaType.Exchange, true);
		ArrayList<String> routingKeyList = new ArrayList<>();
		routingKeyList.add("GroupOperationRequest");
		IAmqpListenerCallback _amqpCallbackListener = new AmqpListenerGroupOperationRequest(
				amqpSender);

		AmqpHelper
				.startListener(exchange, ExchangeType.Direct,
						MediaType.Exchange, routingKeyList,
						_amqpCallbackListener, true);
                 } catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	private static void initializeImportExportListener() throws Exception {
		try {
			String exchange = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_EXCHANGE_LIST,
							ConfigurationConstants.KEY_EXCHANGE_OTHER_DATA);
			IAmqpSender amqpSender = AmqpHelper.getSender(exchange,
					ExchangeType.Direct, MediaType.Exchange, true);
			ArrayList<String> routingKeyList = new ArrayList<>();
			routingKeyList.add("ExportUserDefinedRule");
			routingKeyList.add("ImportUserDefinedRule");
			IAmqpListenerCallback _amqpCallbackListener = new AmqpListenerImportExportRequest(
					amqpSender);

			AmqpHelper.startListener(exchange, ExchangeType.Direct,
					MediaType.Exchange, routingKeyList, _amqpCallbackListener,
					true);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	private static void initializeCommunicationListener() throws Exception {
		try {

			String outExchange = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_EXCHANGE_LIST,
							ConfigurationConstants.KEY_EXCHANGE_OTHER_DATA);

			String inExchange = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_EXCHANGE_LIST,
							ConfigurationConstants.KEY_EXCHANGE_ESPER_REQUEST);

			IAmqpSender amqpSender = AmqpHelper.getSender(outExchange,
					ExchangeType.Direct, MediaType.Exchange, false);

			ArrayList<String> routingKeyListInit = new ArrayList<>();
			routingKeyListInit.add("RuleMediatorEngine");
			IAmqpListenerCallback _amqpCallbackListenerInit = new AmqpListenerEsperRequest(
					amqpSender);

			AmqpHelper.startListener(inExchange, ExchangeType.Direct,
					MediaType.Exchange, routingKeyListInit,
					_amqpCallbackListenerInit, false);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	private static void initailizeBuildListener() throws Exception {
		try {
			String outExchange = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_EXCHANGE_LIST,
							ConfigurationConstants.KEY_EXCHANGE_BUILD_RESPONSE);

			String inExchange = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_EXCHANGE_LIST,
							ConfigurationConstants.KEY_EXCHANGE_BUILD_REQUEST);

			IAmqpSender amqpSender = AmqpHelper.getSender(outExchange,
					ExchangeType.Fanout, MediaType.Exchange, false);

			IAmqpListenerCallback _amqpCallbackListenerInit = new AmqpListenerRuleOperation(
					amqpSender);

			AmqpHelper.startListener(inExchange, ExchangeType.Fanout,
					MediaType.Exchange, null, _amqpCallbackListenerInit, false);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	private static void initializeRuleValidationListener() throws Exception {
		try {

			// Initializing for pre-trade
			String inExchangePreTrade = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_EXCHANGE_LIST,
							ConfigurationConstants.KEY_EXCHANGE_PRE_TRADE_RECIEVER);

			String outQueuePreTrade = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_QUEUE_LIST,
							ConfigurationConstants.KEY_QUEUE_PRE_TRADE_VALIDATION);

			IAmqpSender amqpSenderPreTrade = AmqpHelper
					.getSender(outQueuePreTrade, ExchangeType.None,
							MediaType.Queue, false);

			IAmqpListenerCallback callbackPreTrade = new AmqpListenerRuleValidator(
					amqpSenderPreTrade, RuleType.PreTrade);

			AmqpHelper.startListener(inExchangePreTrade, ExchangeType.Direct,
					MediaType.Exchange, getRoutingKeyList(RuleType.PreTrade),
					callbackPreTrade, true);

			// Now initializing for post-trade
			String inExchangePostTrade = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_EXCHANGE_LIST,
							ConfigurationConstants.KEY_EXCHANGE_POST_TRADE_RECIEVER);

			String outExchangePostTrade = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_EXCHANGE_LIST,
							ConfigurationConstants.KEY_EXCHANGE_NOTIFICATION);

			IAmqpSender amqpSenderPostTrade = AmqpHelper.getSender(
					outExchangePostTrade, ExchangeType.Direct,
					MediaType.Exchange, false);

			IAmqpListenerCallback callbackPostTrade = new AmqpListenerRuleValidator(
					amqpSenderPostTrade, RuleType.PostTrade);

			AmqpHelper.startListener(inExchangePostTrade, ExchangeType.Direct,
					MediaType.Exchange, getRoutingKeyList(RuleType.PostTrade),
					callbackPostTrade, true);
			
			// Initializing for basket
			String inExchangeBasket = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST,
					ConfigurationConstants.KEY_EXCHANGE_BASKET_COMPLIANCE_EXCHANGE);

			String outQueueBasket = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_QUEUE_LIST, ConfigurationConstants.KEY_QUEUE_PRE_TRADE_VALIDATION);

			IAmqpSender amqpSenderBasket = AmqpHelper.getSender(outQueueBasket, ExchangeType.None, MediaType.Queue,
					false);

			IAmqpListenerCallback callbackBasket = new AmqpListenerRuleValidator(amqpSenderBasket, RuleType.Basket);

			AmqpHelper.startListener(inExchangeBasket, ExchangeType.Direct, MediaType.Exchange,
					getRoutingKeyList(RuleType.Basket), callbackBasket, true);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	private static ArrayList<String> getRoutingKeyList(RuleType ruleType)
			throws Exception {
		try {
			ArrayList<String> routingKeyList = new ArrayList<>();
			routingKeyList
					.add(ComplianceLevelConstants.ACCOUNT_SYMBOL_COMPLIANCE_LEVEL);
			routingKeyList
					.add(ComplianceLevelConstants.SYMBOL_COMPLIANCE_LEVEL);
			routingKeyList
					.add(ComplianceLevelConstants.GLOBAL_COMPLIANCE_LEVEL);
			routingKeyList.add(ComplianceLevelConstants.TRADE_COMPLIANCE_LEVEL);
			routingKeyList.add(ComplianceLevelConstants.ACCOUNT_COMPLIANCE_LEVEL);
			routingKeyList
					.add(ComplianceLevelConstants.MASTER_FUND_COMPLIANCE_LEVEL);
			routingKeyList
					.add(ComplianceLevelConstants.UNDERLYING_COMPLIANCE_LEVEL);
			routingKeyList
					.add(ComplianceLevelConstants.MASTERFUND_SYMBOL_COMPLIANCE_LEVEL);
			routingKeyList
					.add(ComplianceLevelConstants.MASTERFUND_UNDERLYINGSYMBOL_COMPLIANCE_LEVEL);
			routingKeyList
					.add(ComplianceLevelConstants.ACCOUNT_UNDERLYINGSYMBOL_COMPLIANCE_LEVEL);
			routingKeyList.add(ComplianceLevelConstants.ASSET_COMPLIANCE_LEVEL);
			routingKeyList
					.add(ComplianceLevelConstants.SUBSECTOR_COMPLIANCE_LEVEL);
			routingKeyList
					.add(ComplianceLevelConstants.SECTOR_COMPLIANCE_LEVEL);
			routingKeyList.add(ComplianceLevelConstants.CUSTOM_RULE_KEY);

			if (ruleType == RuleType.PreTrade)
				routingKeyList.add(ComplianceLevelConstants.EOM_PRE_TRADE_KEY);
			
			if (ruleType == RuleType.Basket)
				routingKeyList.add(ComplianceLevelConstants.EOM_BASKET_KEY);

			return routingKeyList;
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

}
