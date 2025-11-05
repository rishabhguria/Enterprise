package prana.esperCalculator.commonCode;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedHashMap;

import prana.businessObjects.interfaces.IAmqpSender;
import prana.businessObjects.rule.customRules.RuleDefinition;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.constants.CustomRuleConstants;
import prana.esperCalculator.customRule.*;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

public class RuleManagerCommon {
	public static HashMap<String, RuleDefinition> RuleMap = new HashMap<String, RuleDefinition>();
	public static ArrayList<String> _enabled = new ArrayList<String>();
	public static IAmqpSender _amqpSender;
	public static String whatIfPortfolioExchangeName;
	public static String otherDataExchangeName;
	public static String notificationExchangeName;

	public static void attachListener() {
		try {
			PranaLogManager.info("Adding listeners to custom rule");
			String packageName;

			for (String key : RuleMap.keySet()) {
				RuleDefinition node = RuleMap.get(key);
				packageName = node.getRuleType();
				if (packageName.equalsIgnoreCase("preTrade")) {
					RuleDeploymentManagerCommon.addAllListener(whatIfPortfolioExchangeName, node, "CustomRule");
					RuleDeploymentManagerCommon.preTradeCustomEomEvents.add(node.getValidationCompletedEventName());

				} else if (packageName.equalsIgnoreCase("postTrade"))
					RuleDeploymentManagerCommon.addAllListener(notificationExchangeName, node, "PostTradeNotification");

			}

			PranaLogManager.info("Listeners added to custom rule.");
			RuleDeploymentManager.generateAndDeployCustomRuleEom(RuleDeploymentManagerCommon.preTradeCustomEomEvents);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
	
	public static void editConfig(String key, String prop, String value, String confPath) {
		try {
			RuleDataHelper.editConfig(key, prop, value, confPath);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
	
	public static boolean updateConstants(String ruleId, String constants, String confPathModify) {

		try {
			if (constants.isEmpty())
				return true;
			String valueTobeParsed = constants.substring(constants.indexOf("["), constants.lastIndexOf("]") + 1);

			ArrayList<LinkedHashMap<String, Object>> constantList = JSONMapper
					.getJavaTypeArrayFromString(valueTobeParsed);
			LinkedHashMap<String, String> constantKeyValue = new LinkedHashMap<String, String>();

			for (LinkedHashMap<String, Object> customRuleConstantDefination : constantList) {
				switch (customRuleConstantDefination.get("type").toString()) {
				case "int":
					CEPManager.setVariableValue(customRuleConstantDefination.get("name").toString(),
							Integer.parseInt(customRuleConstantDefination.get("value").toString()));
					break;
				case "double":
					CEPManager.setVariableValue(customRuleConstantDefination.get("name").toString(),
							Double.parseDouble(customRuleConstantDefination.get("value").toString()));
					break;
				case "boolean":
					CEPManager.setVariableValue(customRuleConstantDefination.get("name").toString(),
							Boolean.parseBoolean(customRuleConstantDefination.get("value").toString()));
					break;
				default:
					CEPManager.setVariableValue(customRuleConstantDefination.get("name").toString(),
							customRuleConstantDefination.get("value").toString());
				}

				constantKeyValue.put(customRuleConstantDefination.get("name").toString(),
						customRuleConstantDefination.get("value").toString());
			}

			HashMap<String, Object> a = new HashMap<String, Object>();
			a.put("consts", constantList);

			RuleDefinition ruleDef = RuleMap.get(ruleId);
			ruleDef.setConstants(JSONMapper.getStringForObject(a));

			RuleDataHelper.updateConstants(ruleId, confPathModify, constantKeyValue);

			return true;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}
	}
	
	public static boolean updateRule(String ruleId, String propName, String text, String confPath) {
		try {
			synchronized (RuleMap) {
				if (RuleMap.containsKey(ruleId)) {

					RuleDefinition ruleDef = RuleMap.get(ruleId);
					switch (propName) {

					case CustomRuleConstants.RULE_CONF_DESCRIPTION:
						ruleDef.setDescription(text);
						break;
					case CustomRuleConstants.RULE_CONF_NAME:
						RuleDeploymentManagerCommon.UpdateCustomRuleCache(ruleDef, false);
						ruleDef.setRuleName(text);
						RuleDeploymentManagerCommon.UpdateCustomRuleCache(ruleDef, true);
						break;
					}

					editConfig(ruleId, propName, text, confPath);
					return true;
				} else
					return false;
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}
	}

	public static boolean deleteRule(String ruleId) {
		try {
			synchronized (RuleMap) {
				if (RuleMap.containsKey(ruleId)) {

					RuleDefinition ruleDef = RuleMap.get(ruleId);

					boolean isOperationSuccessFull = RuleDeploymentManagerCommon
							.removeListenersFrom(ruleDef.getOutputStatementList());

					if (isOperationSuccessFull) {
						ruleDef.setIsDeleted(true);

						String confPath = ConfigurationHelper.getInstance().getValueBySectionAndKey(
								ConfigurationConstants.SECTION_CUSTOM_RULE_PATH, ruleDef.getClientName());

						editConfig(ruleId, CustomRuleConstants.RULE_CONF_DELETED, String.valueOf(true), confPath);
						return true;
					} else {
						return false;
					}
				} else
					return false;
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}
	}
	
	public static boolean changeStateOfRule(String ruleId, boolean enabled) {
		try {
			synchronized (RuleMap) {
				if (RuleMap.containsKey(ruleId)) {
					RuleDefinition ruleDef = RuleMap.get(ruleId);
					boolean isOperationSuccessFull = false;
					boolean prevState = ruleDef.getEnabled();
					if (prevState != enabled) {
						ruleDef.setEnabled(enabled);
						String packageName = ruleDef.getRuleType();
						if (enabled) {
							if (packageName.equalsIgnoreCase("preTrade")) {
								RuleDeploymentManagerCommon.ConfigureCEPEngine(ruleDef);
								isOperationSuccessFull = RuleDeploymentManagerCommon
										.addAllListener(whatIfPortfolioExchangeName, ruleDef, "CustomRule");

							} else if (packageName.equalsIgnoreCase("postTrade"))
								isOperationSuccessFull = RuleDeploymentManagerCommon.addAllListener(notificationExchangeName,
										ruleDef, "PostTradeNotification");

						} else {
							isOperationSuccessFull = RuleDeploymentManagerCommon
									.removeListenersFrom(ruleDef.getOutputStatementList());
							if (packageName.equalsIgnoreCase("preTrade"))
								RuleDeploymentManagerCommon.removeStatementsForRule(ruleDef);
						}

						if (isOperationSuccessFull) {
							String confPath = ConfigurationHelper.getInstance().getValueBySectionAndKey(
									ConfigurationConstants.SECTION_CUSTOM_RULE_PATH,
									(RuleMap.get(ruleId).getClientName()));

							editConfig(ruleId, CustomRuleConstants.RULE_CONF_ENABLED, String.valueOf(enabled),
									confPath);
							if (!enabled)
								RuleDeploymentManagerCommon.UpdateCustomRuleCache(ruleDef, false);
							else
								RuleDeploymentManagerCommon.UpdateCustomRuleCache(ruleDef, true);
							CEPManager.createEOMBasedOnEnabledRules();
							PranaLogManager.info("RuleId: " + ruleDef.getRuleId() + ", RuleName: "
									+ ruleDef.getRuleName() + " is " + (enabled ? "enabled" : "disabled"));

							return true;
						} else {
							ruleDef.setEnabled(prevState);
							PranaLogManager.info("RuleId: " + ruleDef.getRuleId() + ", RuleName: "
									+ ruleDef.getRuleName() + " is reverted to" + (prevState ? "enabled" : "disabled"));
							return false;
						}

					} else {
						PranaLogManager.info("State of rule " + "RuleId: " + ruleDef.getRuleId() + ", RuleName:"
								+ ruleDef.getRuleName() + " is already " + (enabled ? "enabled" : "disabled"));
						return true;
					}

				}
				return false;
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}

	}
	
	public static ArrayList<String> removeAllListener() {
		PranaLogManager.info("Refresh Started");
		PranaLogManager.info("Removing Rule Listeners");
		_enabled.clear();
		synchronized (RuleMap) {
			try {
				for (String key : RuleMap.keySet()) {
					if (RuleMap.get(key).getEnabled()) {
						boolean isOperationSuccessFull = RuleDeploymentManagerCommon
								.removeListenersFrom(RuleMap.get(key).getOutputStatementList());
						if (isOperationSuccessFull) {

							_enabled.add(key);
						}
					}
				}
				return _enabled;
			} catch (Exception ex) {

				PranaLogManager.error(ex);
				return null;
			}
		}
	}

	public static void addRuleListener() {
		try {
			PranaLogManager.info("ADD Rule Listeners");
			synchronized (RuleMap) {
				for (int i = 0; i < _enabled.size(); i++) {
					String packageName = RuleMap.get(_enabled.get(i)).getRuleType();
					RuleDefinition ruleDef = RuleMap.get(_enabled.get(i));
					if (packageName.equalsIgnoreCase("preTrade")) {
						RuleDeploymentManagerCommon.addAllListener(whatIfPortfolioExchangeName, ruleDef, "CustomRule");

					} else if (packageName.equalsIgnoreCase("postTrade"))
						RuleDeploymentManagerCommon.addAllListener(notificationExchangeName, ruleDef,
								"PostTradeNotification");
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public static String exportRule(String packageName, String ruleName, String directoryPath, String ruleCategory,
			String ruleId) {
		synchronized (RuleMap) {
			try {
				return ImportExportHelper.exportRule(packageName, ruleName, directoryPath, ruleCategory,
						RuleMap.get(ruleId));
			} catch (Exception ex) {
				PranaLogManager.error(ex);
				return null;
			}
		}
	}

	public static RuleDefinition importRule(String packageName, String oldRuleName, String directoryPath,
			String newRuleName) {
		try {
			synchronized (RuleMap) {
				RuleDefinition rule = ImportExportHelper.importRule(packageName, oldRuleName, directoryPath,
						newRuleName);
				if (rule != null) {
					RuleMap.put(rule.getRuleId(), rule);
					if (rule.getEnabled()) {
						RuleDeploymentManagerCommon.UpdateCustomRuleCache(rule, true);
					}
					return rule;
				} else
					return null;
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}
	}
}
