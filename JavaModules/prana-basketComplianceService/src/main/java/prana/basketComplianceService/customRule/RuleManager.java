package prana.basketComplianceService.customRule;

import java.util.HashMap;
import prana.businessObjects.rule.customRules.RuleDefinition;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.constants.CustomRuleConstants;
import prana.esperCalculator.customRule.RuleDataHelper;
import prana.esperCalculator.commonCode.*;
import prana.esperCalculator.communication.DataInitializationRequestProcessor;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

public class RuleManager {
	
	// AmqpHelper _amqpSender;
	public static void configure() {
		try {
			RuleManagerCommon.whatIfPortfolioExchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST, ConfigurationConstants.KEY_EXCHANGE_WHATIF_PORTFOLIO);
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
					if (node.getEnabled() && !node.getRuleType().equals("PostTrade"))
						RuleDeploymentManagerCommon.UpdateCustomRuleCache(node, true);
				}
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
			case "EnableRule":
				DataInitializationRequestProcessor.getInstance()._isEsperStarted = true;
				res = RuleManagerCommon.changeStateOfRule(map.get("RuleId").toString(), true);
				break;
			case "DisableRule":
				DataInitializationRequestProcessor.getInstance()._isEsperStarted = true;
				res = RuleManagerCommon.changeStateOfRule(map.get("RuleId").toString(), false);
				break;
			case "Delete":
				// Exporting rule before deleting so that it can be recovered if
				// deleted by mistake.
				RuleManagerCommon.exportRule(RuleManagerCommon.RuleMap.get(map.get("RuleId").toString()).getRuleType(),
						RuleManagerCommon.RuleMap.get(map.get("RuleId").toString()).getRuleName(), map.get("directoryPath").toString(),
						map.get("ruleCategoryS").toString(), map.get("RuleId").toString());
				res = RuleManagerCommon.deleteRule(map.get("RuleId").toString());
				if (res) {
					RuleDefinition ruleDefDelete = RuleManagerCommon.RuleMap.get(map.get("RuleId").toString());
					PranaLogManager.info("Rule " + map.get("RuleId") + " deleted");
					RuleDeploymentManagerCommon.UpdateCustomRuleCache(ruleDefDelete, false);
				} else
					PranaLogManager.info("Rule " + map.get("RuleId") + " could not delete");
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
				break;
			case "UpdateSummary":
				RuleDefinition ruleDef = RuleManagerCommon.RuleMap.get(map.get("RuleId").toString());
				String confPath = ConfigurationHelper.getInstance().getValueBySectionAndKey(
						ConfigurationConstants.SECTION_CUSTOM_RULE_PATH, ruleDef.getClientName());

				res = RuleManagerCommon.updateRule(map.get("RuleId").toString(), CustomRuleConstants.RULE_CONF_DESCRIPTION,
						map.get("Summary").toString(), confPath);
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
				PranaLogManager.info("Rule " + map.get("RuleId") + " Save Complete");
				break;
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}