package prana.ruleEngineMediator.communication;

import java.util.HashMap;

import prana.amqpAdapter.AmqpHelper;
import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpListenerCallback;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.businessObjects.rule.RuleType;
import prana.ruleEngineMediator.constants.ConfigurationConstants;
import prana.ruleEngineMediator.ruleService.RuleServiceManager;
import prana.ruleEngineMediator.ruleService.ShardineUtility;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

//import prana.ruleEngineMediator.ConfigurationManager;

public class AmqpListenerRuleOperation implements IAmqpListenerCallback {

	private IAmqpSender _amqpSender;

	public AmqpListenerRuleOperation(IAmqpSender amqpSender) {
		_amqpSender = amqpSender;
	}

	/*
	 * Method to send updated rule information to esper and basket
	 */
	public void SendUpdatedRuleToEsperAndBasketComp() {
		try {
			String message = ShardineUtility._ruleNameWithCompression.toString();
			message = message.substring(1, message.length() - 1);
			String requestExchangeEsper = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST, ConfigurationConstants.KEY_EXCHANGE_ESPER_REQUEST);
			IAmqpSender _amqpInitializationRequestSenderEsper = AmqpHelper.getSender(requestExchangeEsper,
					ExchangeType.Direct, MediaType.Exchange, false);
			_amqpInitializationRequestSenderEsper.sendData(message, ConfigurationConstants.ROUTING_KEY_RULE_COMPRESSION_INFO);
			String requestExchangeBasket = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST,
					ConfigurationConstants.KEY_EXCHANGE_BASKET_COMPLIANCE_EXCHANGE);
			IAmqpSender _amqpInitializationRequestSenderBasket = AmqpHelper.getSender(requestExchangeBasket,
					ExchangeType.Direct, MediaType.Exchange, false);
			PranaLogManager.info("Updated rules data sent to Esper and Basket Compliance");
			_amqpInitializationRequestSenderBasket.sendData(message, ConfigurationConstants.ROUTING_KEY_RULE_COMPRESSION_INFO);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}

	}
	
	/**
	 * Receives data from Client for loading and applying operations on user
	 * defined rules.
	 */
	@Override
	public void amqpDataReceived(String jsonReceivedData, String routingKey) {
		try {

			/*
			 * PranaLogManager.info("Request received on Rule mediator :" +
			 * jsonReceivedData);
			 */
			/**
			 * If json string contains LoadRules then it will load for all
			 * packages in Enum RuleType and returns all rules in the form of
			 * hash map(converted to JSON). else converts json string to Hash
			 * map and applies required operation in shardine utility.
			 */
			
			HashMap<String, Object> map = JSONMapper.getHashMap("{\"HashMap\":"
					+ jsonReceivedData + "}");
			String operationType = (String) map.get("operationType");
			switch (operationType) {
			case "Build":
				boolean buildStatus = RuleServiceManager.buildPackage();
				if (!buildStatus) {
					ShardineUtility.EnableDisableRules(
							(String) map.get("packageName"),
							(String) map.get("ruleName"), true);

					boolean buildStatusFinal = RuleServiceManager
							.buildPackage();
					if (buildStatusFinal) {
						PranaLogManager
								.info("Rule has some error,Build Failed. Package: "
										+ (String) map.get("packageName")
										+ ", RuleName: "
										+ (String) map.get("ruleName")
										+ "\nSo rule is disabled.");
					} else {
						throw new Exception(
								"Package build not successful after enabling rule. "
										+ "Tried to revert Save. But Failed...\n "
										+ "Please contact administrator");
					}
				}
				//Gets if rule is enabled or disabled after build operation.
				//To maintain same state on UI.
				map.put("enabled",
						ShardineUtility.getEnableStateAfterBuild(
								(String) map.get("packageName"),
								(String) map.get("ruleName")));

				map.put("operationStatus", buildStatus);
				String buildRes = JSONMapper.getStringForObject(map);
				_amqpSender.sendData(buildRes, "");
				break;
			case "LoadRules":
				HashMap<String, HashMap<String, String>> rules = new HashMap<String, HashMap<String, String>>();
				for (RuleType packageName : RuleType.values()) {
					if(!packageName.equals(RuleType.Basket) && !packageName.equals(RuleType.None))
						rules.putAll(ShardineUtility.loadAllRules(packageName.toString()));
				}
				if (rules.size() == 0) {
					HashMap<String, String> ruleMap = new HashMap<String, String>();
					ruleMap.put("operationType", operationType);
					ruleMap.put("operationStatus", "false");
					rules.put(operationType, ruleMap);
				} else {
					String operationStatusPre = String
							.valueOf(RuleServiceManager.buildPreTradePackage());

					String operationStatusPost = String
							.valueOf(RuleServiceManager.buildPostTradePackage());

					for (String key : rules.keySet()) {

						if (rules.get(key).get("ruleType")
								.equals(RuleType.PreTrade.toString()))
							rules.get(key).put("operationStatus",
									operationStatusPre);
						else if (rules.get(key).get("ruleType")
								.equals(RuleType.PostTrade.toString()))
							rules.get(key).put("operationStatus",
									operationStatusPost);
					}
				}
				String res = JSONMapper.getStringForObject(rules);
				_amqpSender.sendData(res, "");
				break;
			case "AddRule":
				HashMap<String, HashMap<String, String>> ruleMap = new HashMap<String, HashMap<String, String>>();

				ruleMap = ShardineUtility.CreateRule(
						(String) map.get("packageName"),
						(String) map.get("ruleName"));
				String operationStatus = String.valueOf(RuleServiceManager
						.buildPackage());
				for (String key : ruleMap.keySet()) {
					ruleMap.get(key).put("operationStatus", operationStatus);
					ruleMap.get(key).put("ActionUser", String.valueOf(map.get("ActionUser")));//appending userid for every rule
				}
				String addResponse = JSONMapper.getStringForObject(ruleMap);
				_amqpSender.sendData(addResponse, "");
				SendUpdatedRuleToEsperAndBasketComp();
				break;
			case "DeleteRule":
				// Exporting rule before deleting so that it can be recovered if
				// deleted by mistake.
				ShardineUtility.exportRule(map.get("packageName").toString(),
						map.get("ruleName").toString(), map
								.get("directoryPath").toString(),
						map.get("ruleCategory").toString());

				ShardineUtility.deleteRules((String) map.get("packageName"),
						(String) map.get("ruleName"));
				map.put("operationStatus",
						String.valueOf(RuleServiceManager.buildPackage()));
				String response = JSONMapper.getStringForObject(map);
				_amqpSender.sendData(response, "");
				SendUpdatedRuleToEsperAndBasketComp();
				break;
			case "RenameRule":
				HashMap<String, HashMap<String, String>> ruleRenameMap = new HashMap<String, HashMap<String, String>>();

				ruleRenameMap = ShardineUtility.renameAsset(
						(String) map.get("oldRuleName"),
						(String) map.get("ruleName"),
						(String) map.get("packageName"));

				// map.put("operationStatus", String.valueOf(renameResponse));
				String renameStatus = String.valueOf(RuleServiceManager
						.buildPackage());
				for (String key : ruleRenameMap.keySet()) {
					ruleRenameMap.get(key).put("oldRuleId",map.get("ruleId").toString());
					ruleRenameMap.get(key).put("operationStatus", renameStatus);
					ruleRenameMap.get(key).put("ActionUser", String.valueOf(map.get("ActionUser")));//appending userid for every rule
				}
				String renameRes = JSONMapper.getStringForObject(ruleRenameMap);
				_amqpSender.sendData(renameRes, "");
				SendUpdatedRuleToEsperAndBasketComp();
				break;
			case "EnableRule":
				ShardineUtility.EnableDisableRules(
						(String) map.get("packageName"),
						(String) map.get("ruleName"), false);

				boolean enableStatus = RuleServiceManager.buildPackage();

				if (enableStatus) {
					map.put("operationStatus", String.valueOf(enableStatus));
					String enableResponse = JSONMapper.getStringForObject(map);
					_amqpSender.sendData(enableResponse, "");
					SendUpdatedRuleToEsperAndBasketComp();
				} else {
					ShardineUtility.EnableDisableRules(
							(String) map.get("packageName"),
							(String) map.get("ruleName"), true);

					boolean enableStatusFinal = RuleServiceManager
							.buildPackage();
					if (enableStatusFinal) {
						map.put("operationStatus", String.valueOf(enableStatus));
						String enableResponse = JSONMapper
								.getStringForObject(map);
						_amqpSender.sendData(enableResponse, "");

						PranaLogManager
								.info("Rule has some error,Build Failed. Package: "
										+ (String) map.get("packageName")
										+ ", RuleName: "
										+ (String) map.get("ruleName")
										+ "\nSo rule is disabled.");

				} else {

						map.put("operationStatus",
								String.valueOf(enableStatusFinal));
						String enableResponse = JSONMapper
								.getStringForObject(map);
						_amqpSender.sendData(enableResponse, "");

						throw new Exception(
								"Package build not successful after enabling rule. "
										+ "Tried to revert enable. But Failed...\n "
										+ "Please contact administrator");

					}

				}

				break;
			case "DisableRule":
				ShardineUtility.EnableDisableRules(
						(String) map.get("packageName"),
						(String) map.get("ruleName"), true);
				map.put("operationStatus",
						String.valueOf(RuleServiceManager.buildPackage()));
				String disableResponse = JSONMapper.getStringForObject(map);
				_amqpSender.sendData(disableResponse, "");
				SendUpdatedRuleToEsperAndBasketComp();
				break;
			}

		} catch (Exception ex) {
			try {
				HashMap<String, String> buildResponse = new HashMap<>();
				buildResponse.put("Response",
						"Build: Error\n" + ex.getMessage());
				String res = JSONMapper.getStringForObject(buildResponse);
				_amqpSender.sendData(res, "");
				PranaLogManager.error(ex);
				} catch (Exception internalEx) {
				PranaLogManager.error(internalEx);		
			}
		}

	}

	@Override
	public void amqpRecieverStarted() {
		PranaLogManager.info("Build service started");

	}

	@Override
	public void amqpRecieverStopped(String message, Exception ex) {
		PranaLogManager.error(ex,message);	
	}

}
