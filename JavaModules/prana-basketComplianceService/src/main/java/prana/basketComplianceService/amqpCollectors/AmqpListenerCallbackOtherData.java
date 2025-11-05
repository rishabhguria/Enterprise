/**
 * Holds all amqp data collectors
 */
package prana.basketComplianceService.amqpCollectors;

import java.text.SimpleDateFormat;
import java.util.HashMap;
import java.util.LinkedHashMap;

import prana.businessObjects.interfaces.IAmqpListenerCallback;
import prana.basketComplianceService.customRule.RuleManager;
import prana.esperCalculator.commonCode.CEPManager;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

/**
 * Callback listener file which perform actions after data is received from amqp
 * 
 * @author Chaturvedi Dewashish <dewashish@nirvana-sol.com>
 */
public class AmqpListenerCallbackOtherData implements IAmqpListenerCallback {
	SimpleDateFormat parserSDF;

	/*
	 * @see prana.amqpAdapter.IAmqpListenerCallback#AmqpRecieverStarted()
	 */
	@Override
	public void amqpRecieverStarted() {
		PranaLogManager.info("AmqpReceiver for Other data has STARTED\n");
	}

	/*
	 * @see prana.amqpAdapter.IAmqpListenerCallback#AmqpRecieverStopped()
	 */
	@Override
	public void amqpRecieverStopped(String message, Exception cause) {
		PranaLogManager.info("AmqpReceiver for Other data has STOPPED");
		PranaLogManager.error(cause, message);
	}

	/*
	 * @see
	 * prana.amqpAdapter.IAmqpListenerCallback#AmqpDataReceived(java.lang.String ,
	 * java.lang.String)
	 */
	@Override
	public void amqpDataReceived(String jsonReceivedData, String routingKey) {
		try {

			LinkedHashMap<String, Object> map = JSONMapper.getLinkedHashMap(jsonReceivedData);
			switch (routingKey) {
			case "CustomRuleRequest":
				if(map.containsKey("rulePackage") && !map.get("rulePackage").toString().equalsIgnoreCase("PostTrade"))
					RuleManager.operations(map);
				break;
			case "CashFlow":
				cashFlowReceived(map);
				break;
			case "SimulationPreference":
				CEPManager.setVariableWithDebugInfo("IsRealTimePositions",
						Boolean.parseBoolean(map.get("IsRealTimePositions").toString()));

				Boolean _isComingFromRebalancer = Boolean.parseBoolean(map.get("IsComingFromRebalancer").toString());
				if (!_isComingFromRebalancer) {
					CEPManager.getEPRuntime().getEventService()
							.sendEventObjectArray(new Object[] { _isComingFromRebalancer }, "ResetCashFlowWindow");
				}
				break;
			}

		} catch (Exception ex) {
			StringBuilder sb = new StringBuilder();
			sb.append(ex.getMessage());
			sb.append("\n----------------------");
			sb.append("\nError occured in CallBackOtherData");
			sb.append("\nRouting key : " + routingKey);
			sb.append("\nJson : " + jsonReceivedData);
			sb.append("\n----------------------");
			PranaLogManager.error(ex, sb.toString());
		}
	}

	/**
	 * @param map
	 */
	private void cashFlowReceived(HashMap<String, Object> map) {
		try {
			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] {
					Integer.parseInt(map.get("AccountId").toString()), Double.parseDouble(map.get("Cash").toString()) },
					"CashFlow");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}