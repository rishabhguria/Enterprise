package prana.esperCalculator.customRule;

import java.util.HashMap;
import prana.amqpAdapter.AmqpHelper;
import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.rule.customRules.RuleDefinition;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.constants.CustomRuleConstants;
import prana.esperCalculator.commonCode.*;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

public class RuleManager {

	// AmqpHelper _amqpSender;
	public static void configure() {
		try {
			RuleManagerCommon.whatIfPortfolioExchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST, ConfigurationConstants.KEY_EXCHANGE_WHATIF_PORTFOLIO);
			RuleManagerCommon.otherDataExchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST, ConfigurationConstants.KEY_EXCHANGE_OTHERDATA);
			RuleManagerCommon.notificationExchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST, ConfigurationConstants.KEY_EXCHANGE_NOTIFICATION);

			RuleManagerCommon.RuleMap = RuleDataHelper.loadRuleSettings();
			initialize();
			RuleManagerCommon.attachListener();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public static void initialize() {
		try {
			synchronized (RuleManagerCommon.RuleMap) {
				PranaLogManager.info("Loading custom rules --------");

				for (String key : RuleManagerCommon.RuleMap.keySet()) {
					RuleDefinition node = RuleManagerCommon.RuleMap.get(key);
					RuleDeploymentManagerCommon.ConfigureCEPEngine(node);

				}
				String exchangeName = RuleManagerCommon.otherDataExchangeName;
				RuleManagerCommon._amqpSender = AmqpHelper.getSender(exchangeName, ExchangeType.Direct, MediaType.Exchange, false);
				PranaLogManager.info("Custom Rules Loaded");
			}

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public static void operations(HashMap<String, Object> map) {
		try {
			String operationType = map.get("OperationType").toString();
			boolean res = false;
			switch (operationType) {
			case "InitialisationRequest":
				sendAllRules();
				break;
			case "EnableRule":
				res = RuleManagerCommon.changeStateOfRule(map.get("RuleId").toString(), true);
				publishResponse(map, res);
				break;
			case "DisableRule":
				res = RuleManagerCommon.changeStateOfRule(map.get("RuleId").toString(), false);
				publishResponse(map, res);
				break;
			case "Delete":
				// Exporting rule before deleting so that it can be recovered if
				// deleted by mistake.
				RuleManagerCommon.exportRule(RuleManagerCommon.RuleMap.get(map.get("RuleId").toString()).getRuleType(),
						RuleManagerCommon.RuleMap.get(map.get("RuleId").toString()).getRuleName(), map.get("directoryPath").toString(),
						map.get("ruleCategoryS").toString(), map.get("RuleId").toString());
				res = RuleManagerCommon.deleteRule(map.get("RuleId").toString());
				if (res)
					PranaLogManager.info("Rule " + map.get("RuleId") + " deleted");
				else
					PranaLogManager.info("Rule " + map.get("RuleId") + " could not delete");
				publishResponse(map, res);
				break;
			case "RenameRule":

				RuleDefinition ruleDefRename = RuleManagerCommon.RuleMap.get(map.get("RuleId").toString());

				String confPathRename = ConfigurationHelper.getInstance().getValueBySectionAndKey(
						ConfigurationConstants.SECTION_CUSTOM_RULE_PATH, ruleDefRename.getClientName());

				res = RuleManagerCommon.updateRule(map.get("RuleId").toString(), CustomRuleConstants.RULE_CONF_NAME,
						map.get("NewName").toString(), confPathRename);
				if (res)
					PranaLogManager.info(
							"Rule Id: " + map.get("RuleId") + ", RuleName: " + map.get("NewName") + " is renamed");
				else
					PranaLogManager.info("Rule " + map.get("RuleId") + " could not rename");
				publishResponse(map, res);
				break;
			case "UpdateSummary":
				RuleDefinition ruleDef = RuleManagerCommon.RuleMap.get(map.get("RuleId").toString());
				String confPath = ConfigurationHelper.getInstance().getValueBySectionAndKey(
						ConfigurationConstants.SECTION_CUSTOM_RULE_PATH, ruleDef.getClientName());

				res = RuleManagerCommon.updateRule(map.get("RuleId").toString(), CustomRuleConstants.RULE_CONF_DESCRIPTION,
						map.get("Summary").toString(), confPath);
				publishResponse(map, res);
				break;
			case "Build":
				synchronized (RuleManagerCommon.RuleMap) {
					map.put("enabled", RuleManagerCommon.RuleMap.get(map.get("RuleId").toString()).getEnabled());
					// Save the constant values
					RuleDefinition modifiedRule = RuleManagerCommon.RuleMap.get(map.get("RuleId").toString());

					String confPathModify = ConfigurationHelper.getInstance().getValueBySectionAndKey(
							ConfigurationConstants.SECTION_CUSTOM_RULE_PATH, modifiedRule.getClientName());

					res = RuleManagerCommon.updateConstants(map.get("RuleId").toString(), map.get("tag").toString(), confPathModify);
					if (res)
						PranaLogManager.info(
								"Rule Id: " + map.get("RuleId") + ", RuleName: " + map.get("NewName") + " is saved");
				}
				publishResponse(map, true);
				PranaLogManager.info("Rule " + map.get("RuleId") + " Save Complete");
				break;
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private static void publishResponse(HashMap<String, Object> map, boolean res) {
		try {
			map.put("operationStatus", String.valueOf(res));
			String messageToSend = JSONMapper.getStringForObject(map);

			RuleManagerCommon._amqpSender.sendData(messageToSend, CustomRuleConstants.AMQP_ROUTING_KEY_CUSTOM_RULE_RESPONSE);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private static void sendAllRules() {
		try {
			synchronized (RuleManagerCommon.RuleMap) {
				if (RuleManagerCommon.RuleMap != null && RuleManagerCommon._amqpSender != null) {
					String messageToSend = JSONMapper.getStringForObject(RuleManagerCommon.RuleMap);
					RuleManagerCommon._amqpSender.sendData(messageToSend, CustomRuleConstants.AMQP_ROUTING_KEY_INITIALISE_RESPONSE);
					PranaLogManager.info("Custom rules Initialisation response sent");
				} else
					PranaLogManager.warn("Custom Rule manager is not initialized yet. Please wait until Esper is not started completely");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}