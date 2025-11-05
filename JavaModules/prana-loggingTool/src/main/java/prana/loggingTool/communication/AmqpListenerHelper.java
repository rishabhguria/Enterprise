package prana.loggingTool.communication;

import java.util.ArrayList;

import prana.amqpAdapter.AmqpHelper;
import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpListenerCallback;
import prana.businessObjects.rule.RuleType;
import prana.loggingTool.constants.ComplianceLevelConstants;
import prana.loggingTool.constants.ConfigurationConstants;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

public class AmqpListenerHelper {

	static void intializeAllAmqpListeners() throws Exception {

		try {
			initializeRuleValidationListener();
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

			IAmqpListenerCallback callbackPreTrade = new AmqpListenerRuleValidator(
					RuleType.PreTrade);

			AmqpHelper.startListener(inExchangePreTrade, ExchangeType.Direct,
					MediaType.Exchange, getRoutingKeyList(RuleType.PreTrade),
					callbackPreTrade, true);
			
			IAmqpListenerCallback callbackEventValidator = new AmqpListenerEventValidator(
					"EventsLogging");

			ArrayList<String> routingKey = new ArrayList<String>();
			routingKey.add("EventLogging");
			AmqpHelper.startListener(inExchangePreTrade, ExchangeType.Direct,
					MediaType.Exchange, routingKey,
					callbackEventValidator, true);

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

			if (ruleType == RuleType.PreTrade)
				routingKeyList.add(ComplianceLevelConstants.EOM_PRE_TRADE_KEY);
			
			routingKeyList.add(ComplianceLevelConstants.OTHER_LOGGING_KEY);

			return routingKeyList;
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

}
