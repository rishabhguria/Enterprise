package prana.ruleEngineMediator.communication;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;

import prana.businessObjects.complianceLevel.Alert;
import prana.businessObjects.complianceLevel.Asset;
import prana.businessObjects.complianceLevel.Account;
import prana.businessObjects.complianceLevel.Account_Symbol;
import prana.businessObjects.complianceLevel.Account_UnderlyingSymbol;
import prana.businessObjects.complianceLevel.Global;
import prana.businessObjects.complianceLevel.MasterFund;
import prana.businessObjects.complianceLevel.MasterFund_Symbol;
import prana.businessObjects.complianceLevel.MasterFund_UnderlyingSymbol;
import prana.businessObjects.complianceLevel.Sector;
import prana.businessObjects.complianceLevel.SubSector;
import prana.businessObjects.complianceLevel.Symbol;
import prana.businessObjects.complianceLevel.Trade;
import prana.businessObjects.complianceLevel.UnderlyingSymbol;
import prana.businessObjects.interfaces.IAmqpListenerCallback;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.businessObjects.rule.RuleType;
import prana.ruleEngineMediator.constants.ComplianceLevelConstants;
import prana.ruleEngineMediator.constants.ConfigurationConstants;
import prana.ruleEngineMediator.ruleService.RuleServiceManager;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;
//import prana.ruleEngineMediator.Constants;
import prana.utility.ruleFormatting.RuleFormatting;

//import prana.ruleEngineMediator.ConfigurationManager;

public class AmqpListenerRuleValidator implements IAmqpListenerCallback {

	IAmqpSender _amqpSender;
	RuleType _ruleType;

	SimpleDateFormat sdf = new SimpleDateFormat(
			ConfigurationConstants.SIMPLE_DATE_FORMAT_2);
	
	HashMap<String, Integer> _basketAlertsCount = new HashMap<String, Integer>();

	public AmqpListenerRuleValidator(IAmqpSender amqpSender, RuleType ruleType) {
		try {
			this._ruleType = ruleType;
			this._amqpSender = amqpSender;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	@Override
	public void amqpDataReceived(String jsonReceivedData, String routingKey) {
		Alert alert = null;
		try {
			if (routingKey.equals("EomPreTrade") || routingKey.equals("EomBasket")) {
				alert = new Alert();

				HashMap<String, Object> map = JSONMapper.getHashMap(jsonReceivedData);
				// .getMappedObject(jsonReceivedData);

				// HashMap<String, Object> map = ServiceManager.objectMapper
				// .readValue(jsonReceivedData, HashMap.class);

				// .readValue(jsonReceivedData, HashMap.class);
				if (_basketAlertsCount.containsKey(map.get("basketId").toString())) {
					alert.setDescription("Alerts count : " + _basketAlertsCount.get(map.get("basketId").toString()));
					_basketAlertsCount.remove(map.get("basketId").toString());
				}
				alert.setOrderId(map.get("basketId").toString());
				alert.setIsEOM(true);

				if (this._ruleType.compareTo(RuleType.PreTrade) == 0
						|| this._ruleType.compareTo(RuleType.Basket) == 0) {

					String jsonString = JSONMapper.getStringForObject(alert);
					// String jsonString = ServiceManager.objectMapper
					// .writeValueAsString(alert);

					// .writeValueAsString(alert);

					PranaLogManager.info("EoM message");

					_amqpSender.sendData(jsonString, "");

					PranaLogManager.info(jsonString);

				}
			} else if (routingKey.equals("CustomRule")) {

				/*
				 * alert = ConfigurationManager.ObjectMapper
				 * .readValue(jsonReceivedData,Alert.class);
				 */
				_amqpSender.sendData(jsonReceivedData, "");

			} else {
				// TODO: Needs to improve mapping
				HashMap<String, Object> map = JSONMapper.getHashMap(jsonReceivedData);
				// .getMappedObject(jsonReceivedData);
				// .readValue(jsonReceivedData, HashMap.class);

				Object complianceLevelObj = getComplianceObject(map, routingKey);

				ArrayList<Alert> alertList = RuleServiceManager.applyRule(complianceLevelObj, _ruleType);

				// _ruleExecutionHelper.applyRule(complianceLevelObj);

				if (alertList != null && alertList.size() > 0) {
					int userId = 0;
					String orderId = "";
					if (map.containsKey("userId") && map.get("userId") != null)
						userId = Integer.parseInt(map.get("userId").toString());
					if (map.containsKey("taxlotId") && map.get("taxlotId") != null)
						orderId = map.get("taxlotId").toString();

					if (!(this._ruleType.compareTo(RuleType.PostTrade) == 0) || map.get("basketId") != null) {
						String basketId = map.get("basketId").toString();
						if (!_basketAlertsCount.containsKey(basketId)) {
							_basketAlertsCount.put(basketId, alertList.size());
						} else {
							_basketAlertsCount.put(basketId, _basketAlertsCount.get(basketId) + alertList.size());
						}
					}
					for (Alert alertRule : alertList) {
						if (alertRule != null && alertRule.getViolated()) {

							alertRule.setUserId(userId);
							alertRule.setOrderId(orderId);
							alertRule.setCompressionLevel(routingKey);
							// sdf.format(new Date());
							alertRule.setValidationTime(sdf.format(new Date()));
							alertRule.setRuleType(this._ruleType.toString());
							alertRule.setName(RuleFormatting.getUIFormattedRule(alertRule.getName()));
							String jsonString = JSONMapper.getStringForObject(alertRule);
							// .writeValueAsString(alert);

							if (this._ruleType.compareTo(RuleType.PostTrade) == 0) {
								PranaLogManager.showOnly("Post-Trade Notification.");

								_amqpSender.sendData(jsonString, "PostTradeNotification");

								PranaLogManager.showOnly(jsonString);

							} else if (this._ruleType.compareTo(RuleType.PreTrade) == 0) {
								if (map.containsKey("basketId") && map.get("basketId") != null)
									alertRule.setOrderId(map.get("basketId").toString());
								jsonString = JSONMapper.getStringForObject(alertRule);
								PranaLogManager.info("Pre-Trade message");

								_amqpSender.sendData(jsonString, "");

								PranaLogManager.info(jsonString);

							} else if (this._ruleType.compareTo(RuleType.Basket) == 0) {
								if (map.containsKey("basketId") && map.get("basketId") != null)
									alertRule.setOrderId(map.get("basketId").toString());
								jsonString = JSONMapper.getStringForObject(alertRule);
								PranaLogManager.info("Basket message");

								_amqpSender.sendData(jsonString, "");

								PranaLogManager.info(jsonString);

							}
						}
					}
				}
			}

		} catch (Exception ex) {
			PranaLogManager.error(ex, "Error in " + _ruleType);
		} finally {
			alert = null;
		}
	}

	private Object getComplianceObject(HashMap<String, Object> map,
			String routingKey) {
		Object complianceObj = new Object();
		try {

			switch (routingKey) {

			case ComplianceLevelConstants.ACCOUNT_COMPLIANCE_LEVEL:
				Account complianceLevel = JSONMapper.getJavaTypeFromHashMap(map,
						Account.class);
				// .convertValue(map, Fund.class);
				complianceObj = complianceLevel;
				break;

			case ComplianceLevelConstants.ACCOUNT_SYMBOL_COMPLIANCE_LEVEL:
				Account_Symbol account_SymbolCL = JSONMapper.getJavaTypeFromHashMap(
						map, Account_Symbol.class);
				complianceObj = account_SymbolCL;
				break;

			case ComplianceLevelConstants.UNDERLYING_COMPLIANCE_LEVEL:
				UnderlyingSymbol underlyingSymbolCL = JSONMapper
						.getJavaTypeFromHashMap(map, UnderlyingSymbol.class);
				complianceObj = underlyingSymbolCL;
				break;

			case ComplianceLevelConstants.GLOBAL_COMPLIANCE_LEVEL:
				Global globalCL = JSONMapper.getJavaTypeFromHashMap(map,
						Global.class);
				complianceObj = globalCL;
				break;

			case ComplianceLevelConstants.MASTER_FUND_COMPLIANCE_LEVEL:
				MasterFund masterFundCL = JSONMapper.getJavaTypeFromHashMap(
						map, MasterFund.class);
				complianceObj = masterFundCL;
				break;

			case ComplianceLevelConstants.TRADE_COMPLIANCE_LEVEL:
				Trade taxlotCL = JSONMapper.getJavaTypeFromHashMap(map,
						Trade.class);
				complianceObj = taxlotCL;
				break;

			case ComplianceLevelConstants.MASTERFUND_SYMBOL_COMPLIANCE_LEVEL:
				MasterFund_Symbol masterFund_SymbolCL = JSONMapper
						.getJavaTypeFromHashMap(map, MasterFund_Symbol.class);
				complianceObj = masterFund_SymbolCL;
				break;

			case ComplianceLevelConstants.SYMBOL_COMPLIANCE_LEVEL:
				Symbol symbolCL = JSONMapper.getJavaTypeFromHashMap(map,
						Symbol.class);
				complianceObj = symbolCL;
				break;
			case ComplianceLevelConstants.MASTERFUND_UNDERLYINGSYMBOL_COMPLIANCE_LEVEL:
				MasterFund_UnderlyingSymbol MasterFund_UnderlyingSymbolCL = JSONMapper
						.getJavaTypeFromHashMap(map,
								MasterFund_UnderlyingSymbol.class);
				complianceObj = MasterFund_UnderlyingSymbolCL;
				break;

			case ComplianceLevelConstants.ACCOUNT_UNDERLYINGSYMBOL_COMPLIANCE_LEVEL:
				Account_UnderlyingSymbol account_UnderlyingSymbolCL = JSONMapper
						.getJavaTypeFromHashMap(map,
								Account_UnderlyingSymbol.class);
				complianceObj = account_UnderlyingSymbolCL;
				break;

			case ComplianceLevelConstants.ASSET_COMPLIANCE_LEVEL:
				Asset AssetCL = JSONMapper.getJavaTypeFromHashMap(map,
						Asset.class);
				complianceObj = AssetCL;
				break;

			case ComplianceLevelConstants.SUBSECTOR_COMPLIANCE_LEVEL:
				SubSector SubSectorCL = JSONMapper.getJavaTypeFromHashMap(map,
						SubSector.class);
				complianceObj = SubSectorCL;
				break;

			case ComplianceLevelConstants.SECTOR_COMPLIANCE_LEVEL:
				Sector SectorCL = JSONMapper.getJavaTypeFromHashMap(map,
						Sector.class);
				complianceObj = SectorCL;
				break;

			default:
				break;

			}

		} catch (Exception ex) {
			PranaLogManager.error(ex,"Error in " + _ruleType);
		}
		return complianceObj;

	}

	@Override
	public void amqpRecieverStarted() {

		PranaLogManager.info("Service started. RuleType: "
				+ String.valueOf(_ruleType));
	}

	@Override
	public void amqpRecieverStopped(String message, Exception ex) {
		PranaLogManager.info("Service stopped. RuleType: "
				+ String.valueOf(_ruleType));
		PranaLogManager.error(ex,"Error in " + _ruleType);

	}

}
