package prana.basketComplianceService.basketCEP;

import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;

import com.espertech.esper.common.client.fireandforget.EPFireAndForgetQueryResult;

import prana.businessObjects.complianceLevel.Alert;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.businessObjects.rule.RuleType;
import prana.esperCalculator.commonCode.CEPManager;
import prana.esperCalculator.main.WhatIfManager;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

/**
 * 
 * @author abhinav.pandey
 *
 *         A helper/common class for util methods in esper
 */
public class WhatIfHelper {
	
	/**
	 * Extracting security information
	 * 
	 * @param symbol
	 */
	public static HashMap<String, Object> getSecurityInformationForSymbol(String symbol) throws Exception {
		HashMap<String, Object> hashMap = null;
		try {
			String _pullQuery = "Select distinct S.tickerSymbol as symbol, S.fxSymbol as fxSymbol from SecurityWindow as S where tickerSymbol='@XXXXX'";
			String query = _pullQuery.replace("@XXXXX", symbol.replace("\\", "\\\\"));
			PranaLogManager.logOnly(query);
			EPFireAndForgetQueryResult result;
			int count = 0;
			do {
				result = CEPManager.executeQuery(query);
				if (result.getArray().length == 0) {
					Thread.sleep(1000);
				}
				count++;
			} while (result.getArray().length == 0 && count < WhatIfManager.securityRetryCount);

			if (count > 1) {
				if (result.getArray().length == 0)
					PranaLogManager.logOnly("Security not found in SecurityWindow for symbol " + symbol
							+ " with retry count: " + count);
				else
					PranaLogManager.logOnly(
							"Security found in SecurityWindow for symbol " + symbol + " with retry count: " + count);
			}

			if (result != null && result.getArray() != null && result.getArray().length > 0) {
				{
					hashMap = new HashMap<>();
					hashMap.put("symbol", result.getArray()[0].get("symbol"));
					hashMap.put("fxSymbol", result.getArray()[0].get("fxSymbol"));
					return hashMap;
				}
			} else
				throw new Exception("Security not found in SecurityWindow for symbol: " + symbol);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage()
					+ ", Could Not fetch WhatIf Security infromation for the given symbol [ " + symbol + " ]", ex);
		}
		return hashMap;
	}
	
	/**
	 * Sends compliance error alert, stopping any further validation by sending EOM
	 * to trade server
	 * 
	 * @param orderId
	 * @param userId
	 * @param summary
	 * @param description
	 * @throws IOException
	 *             throws IO exception is sending is failed
	 */
	public static void sendComplianceFailAlert(IAmqpSender amqpCancelMessageSender, String orderId, int userId)
			throws IOException {
		try {
			Alert alertCancel = new Alert();
			alertCancel.setName("Something went wrong !! Please Contact Support.");
			alertCancel.setCompressionLevel("None");
			alertCancel.setViolated(true);
			alertCancel.setIsEOM(false);
			alertCancel.setOrderId(orderId);
			alertCancel.setUserId(userId);
			alertCancel.setRuleType(RuleType.Basket.toString());
			alertCancel.setDescription("Compliance validation failed.");
			alertCancel.setDimension("N/A");
			alertCancel.setParameters("No rule validated.");
			alertCancel.setSummary(
					"Compliance Engine did not run for this trade and it has been blocked as per your preference. Please try to enter the trade again, and if the issue persists please contact your client support representative.");
			SimpleDateFormat sdf = new SimpleDateFormat("M/d/yyyy HH:mm:ss");
			sdf.format(new Date());
			alertCancel.setValidationTime(sdf.format(new Date()));
			alertCancel.setBlocked(true);
			String messageString = JSONMapper.getStringForObject(alertCancel);
			amqpCancelMessageSender.sendData(messageString, "TradeCancelled");

			PranaLogManager.info(messageString);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
		}

	}
	
	/**
	 * Sends basket compliance error alert, stopping any further validation by sending EOM
	 * to trade server
	 * 
	 * @param orderId
	 * @param userId
	 * @param summary
	 * @param description
	 * @throws IOException
	 *             throws IO exception is sending is failed
	 */
	public static void sendBasketComplianceNotStartedAlert(IAmqpSender amqpCancelMessageSender, String orderId, int userId)
			throws IOException {
		try {
			Alert alertCancel = new Alert();
			alertCancel.setName("Basket compliance is not started completely");
			alertCancel.setCompressionLevel("None");
			alertCancel.setViolated(true);
			alertCancel.setIsEOM(false);
			alertCancel.setOrderId(orderId);
			alertCancel.setUserId(userId);
			alertCancel.setRuleType(RuleType.None.toString());
			alertCancel.setDescription("Basket compliance not started correctly ");
			alertCancel.setDimension("N/A");
			alertCancel.setParameters("N/A");
			alertCancel.setActualResult("N/A");
			alertCancel.setThreshold("N/A");
			alertCancel.setSummary("Basket Compliance Engine cannot serve as either Rule Mediator is down or Basket Compliance is not started completely.");
			SimpleDateFormat sdf = new SimpleDateFormat("M/d/yyyy HH:mm:ss");
			sdf.format(new Date());
			alertCancel.setValidationTime(sdf.format(new Date()));
			alertCancel.setBlocked(true);
			String messageString = JSONMapper.getStringForObject(alertCancel);
			amqpCancelMessageSender.sendData(messageString, "TradeCancelled");

			PranaLogManager.info(messageString);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
		}

	}

	/**
	 * Send an EOM message to Trade server
	 * 
	 * @param orderId
	 * @param userId
	 * @throws IOException
	 */
	public static void sendEomMessage(IAmqpSender amqpCancelMessageSender, String orderId, int userId)
			throws IOException {
		try {
			Alert eomMsg = new Alert();
			eomMsg.setIsEOM(true);
			eomMsg.setOrderId(orderId);
			eomMsg.setUserId(userId);
			eomMsg.setRuleType(RuleType.Basket.toString());
			SimpleDateFormat sdf = new SimpleDateFormat("M/d/yyyy HH:mm:ss");
			sdf.format(new Date());
			eomMsg.setValidationTime(sdf.format(new Date()));

			String jsonString = JSONMapper.getStringForObject(eomMsg);
			amqpCancelMessageSender.sendData(jsonString, "TradeCancelled");
			PranaLogManager.info(jsonString);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
		}
	}
}