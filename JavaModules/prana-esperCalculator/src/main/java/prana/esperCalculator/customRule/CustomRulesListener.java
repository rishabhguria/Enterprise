package prana.esperCalculator.customRule;

import java.text.SimpleDateFormat;

import prana.businessObjects.complianceLevel.Alert;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.businessObjects.interfaces.IDisposable;
import prana.businessObjects.rule.customRules.RuleDefinition;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.constants.CustomRuleConstants;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

import com.espertech.esper.common.client.EventBean;
import com.espertech.esper.runtime.client.EPRuntime;
import com.espertech.esper.runtime.client.EPStatement;
import com.espertech.esper.runtime.client.UpdateListener;

public class CustomRulesListener implements UpdateListener, IDisposable {
	IAmqpSender _amqpSender;
	String _routingKey;
	RuleDefinition _ruleDef;
	SimpleDateFormat sdf = new SimpleDateFormat(ConfigurationConstants.SIMPLE_DATE_FORMAT);

	public CustomRulesListener(IAmqpSender amqpSender, String routingKey, RuleDefinition ruleDef) {
		this._amqpSender = amqpSender;
		this._routingKey = routingKey;
		this._ruleDef = ruleDef;
	}

	@Override
	public void update(EventBean[] newEvents, EventBean[] oldEvents, EPStatement statement, EPRuntime runtime) {
		try {
			if (newEvents != null) {
				for (EventBean eventBean : newEvents) {
					try {
						Boolean isViolated = (Boolean) eventBean.get(CustomRuleConstants.RULE_LISTENER_ISVIOLATED);
						String threshold = (String) eventBean.get(CustomRuleConstants.RULE_CONF_THRESHOLD);
						String actualResult = (String) eventBean.get(CustomRuleConstants.RULE_CONF_ACTUALRESULT);
						String constraintField = (String) eventBean.get(CustomRuleConstants.RULE_CONF_CONSTRAINTFIELD);
						if (isViolated) {
							Alert a = getDefaultAlertObject();
							a.setCompressionLevel(_ruleDef.getCompressionLevel());
							a.setDescription(eventBean.get(CustomRuleConstants.RULE_LISTENER_SUMMARY).toString());
							a.setName(_ruleDef.getRuleName());
							a.setParameters(eventBean.get(CustomRuleConstants.RULE_LISTENER_PARAMETERS).toString());
							a.setRuleType(_ruleDef.getRuleType());
							a.setViolated(isViolated);
							a.setBlocked(_ruleDef.getBlocked());
							a.setValidationTime(
									sdf.format(eventBean.get(CustomRuleConstants.RULE_LISTENER_VALIDATION_TIME)));
							a.setIsEOM(false);
							a.setSummary(_ruleDef.getDescription());
							a.setThreshold(threshold);
							a.setActualResult(actualResult);
							a.setConstraintFields(constraintField);

							// Tax lot id for post trade
							a.setOrderId(eventBean.get(CustomRuleConstants.RULE_LISTENER_TAXLOTID).toString());

							// Basket id for PreTrade
							if (_ruleDef.getRuleType().equals("PreTrade"))
								a.setOrderId(eventBean.get(CustomRuleConstants.RULE_LISTENER_BASKETID).toString());

							if (eventBean.get(CustomRuleConstants.RULE_LISTENER_USERID) != null)
								a.setUserId(Integer
										.parseInt(eventBean.get(CustomRuleConstants.RULE_LISTENER_USERID).toString()));
							else
								a.setUserId(0);

							a.setDimension(eventBean.get(CustomRuleConstants.RULE_LISTENER_DIMENSION).toString());
							String message = JSONMapper.getStringForObject(a);
							PranaLogManager.logOnly("Pre-Trade message for Custom Rule");
							PranaLogManager.logOnly(message);
							if (_routingKey == null || _routingKey.compareTo("") == 0)
								_amqpSender.sendData(message, "");
							else
								_amqpSender.sendData(message, _routingKey);
						}
					} catch (Exception ex) {
						PranaLogManager.error(ex);
					}
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private Alert getDefaultAlertObject() {
		Alert defaultAlert = new Alert();
		defaultAlert.setCompressionLevel("Not available");
		defaultAlert.setDescription("Not available");
		defaultAlert.setIsEOM(false);
		defaultAlert.setRuleType("Not available");
		defaultAlert.setSummary("Not available");
		return defaultAlert;
	}

	public void disposeListener() {
		try {
			this._amqpSender.closeChannelAndConnection();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}