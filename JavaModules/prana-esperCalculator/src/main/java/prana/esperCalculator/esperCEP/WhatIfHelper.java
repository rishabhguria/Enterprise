package prana.esperCalculator.esperCEP;

import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;

import prana.businessObjects.complianceLevel.Alert;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.businessObjects.rule.RuleType;
import prana.esperCalculator.commonCode.CEPManager;
import prana.esperCalculator.main.WhatIfManager;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

import com.espertech.esper.common.client.fireandforget.EPFireAndForgetQueryResult;

/**
 * 
 * @author abhinav.pandey
 *
 *         A helper/common class for util methods in esper
 */
public class WhatIfHelper {

	/**
	 * Retrieve security details for given symbol
	 * 
	 * @param symbol
	 * @return
	 * @throws Exception
	 *             throws exception id could not fetch the security details
	 */
	public static HashMap<String, Object> getSecurityDetailsForSymbol(String symbol) throws Exception {
		HashMap<String, Object> res = null;
		try {
			String _pullQuery = "Select distinct " + "S.tickerSymbol as symbol," + "S.fxSymbol as fxSymbol," // + "case
					+ "S.underlyingSymbol" + " as underlyingSymbol," + " S.currencyId as currencyId, "
					+ " S.leadCurrencyId as leadCurrencyId, " + " S.vsCurrencyId as vsCurrencyId, "
					+ " A.asset as asset, " + " A.assetId as assetId " + "from SecurityWindow as S left outer join "
					+ "AuecWindow as A on A.auecId = S.auecId " + "where tickerSymbol='@XXXXX'";

			String query = _pullQuery.replace("@XXXXX", symbol.replace("\\", "\\\\"));

			PranaLogManager.logOnly(query);
			EPFireAndForgetQueryResult result;
			int count = 0;
			do {
				result = CEPManager.executeQuery(query);
				if(result.getArray() == null)
				{
					Thread.sleep(1000);
				}
				count++;
			} while (result.getArray() == null && count < WhatIfManager.securityRetryCount);

			if (count > 1)
			{
				if(result.getArray() == null)
					PranaLogManager.logOnly("Security not found in SecurityWindow for symbol " + symbol + " with retry count: " + count);
				else
					PranaLogManager.logOnly("Security found in SecurityWindow for symbol " + symbol + " with retry count: " + count);
					
			}
			
			if (result != null && result.getArray() != null && result.getArray().length > 0) {
				res = new HashMap<>();
				res.put("symbol", result.getArray()[0].get("symbol"));
				res.put("underlyingSymbol", result.getArray()[0].get("underlyingSymbol"));
				res.put("fxSymbol", result.getArray()[0].get("fxSymbol"));
				res.put("currencyId", result.getArray()[0].get("currencyId"));
				res.put("currencyId", result.getArray()[0].get("currencyId"));
				res.put("leadCurrencyId", result.getArray()[0].get("leadCurrencyId"));
				res.put("vsCurrencyId", result.getArray()[0].get("vsCurrencyId"));
				res.put("asset", result.getArray()[0].get("asset"));
				res.put("assetId", result.getArray()[0].get("assetId"));

			} else
				throw new Exception("Security not found in SecurityWindow for symbol: " + symbol);
		} catch (Exception ex) {
			PranaLogManager
					.error(ex.getMessage() + ", Could Not fetch security for the given symbol [ " + symbol + " ]", ex);
		}
		return res;
	}

	public static boolean getSecurityInformationForSymbol(String symbol) throws Exception {
		try {
			String _pullQuery = "Select distinct "
					+ "S.tickerSymbol as symbol from SecurityWindow as S where tickerSymbol='@XXXXX'";
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
				return true;
			} else
				throw new Exception("Security not found in SecurityWindow for symbol: " + symbol);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage()
					+ ", Could Not fetch WhatIf Security infromation for the given symbol [ " + symbol + " ]", ex);
		}
		return false;
	}

	public static boolean getSymbolDataInformationForSymbol(String symbol) throws Exception {
		try {
			String _pullQuery = "Select distinct "
					+ "S.symbol as symbol from SymbolDataWindow as S where symbol='@XXXXX'";
			String query = _pullQuery.replace("@XXXXX", symbol.replace("\\", "\\\\"));
			PranaLogManager.logOnly(query);
			EPFireAndForgetQueryResult result = CEPManager.executeQuery(query);

			if (result.getArray().length > 0) {
				return true;
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage()
					+ "Could Not fetch SymbolData infromation for the given symbol [ " + symbol + " ]", ex);
		}
		return false;
	}

	/**
	 * Sends cancellation message to trade server
	 * 
	 * @param orderId
	 * @param userId
	 * @param summary
	 * @param description
	 * @throws IOException
	 *             throws IO exception is sending is failed
	 */
	public static void sendCancellationMessage(IAmqpSender amqpCancelMessageSender, String orderId, int userId,
			String description, String summary) throws IOException {
		try {
			// XXX: Static Alert object is created every time livefeed is not available.
			// Need to move to static place
			Alert alertCancel = new Alert();
			alertCancel.setName("N/A");
			alertCancel.setCompressionLevel("None");
			alertCancel.setViolated(true);
			alertCancel.setIsEOM(false);
			alertCancel.setOrderId(orderId);
			alertCancel.setUserId(userId);
			alertCancel.setRuleType(RuleType.PreTrade.toString());
			alertCancel.setDescription(description);
			alertCancel.setDimension("N/A");
			alertCancel.setParameters("No rule validated");
			alertCancel.setSummary(summary);
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
	 * Sends missing info trade server
	 * 
	 * @param orderId
	 * @param userId
	 * @param summary
	 * @param description
	 * @throws IOException
	 *             throws IO exception is sending is failed
	 */
	public static void sendMissingInformationAlert(IAmqpSender amqpCancelMessageSender, String orderId, int userId,
			String description, String summary, String parameter) throws IOException {
		try {
			Alert alertCancel = new Alert();
			alertCancel.setName("Missing Information Alert");
			alertCancel.setCompressionLevel("None");
			alertCancel.setViolated(true);
			alertCancel.setIsEOM(false);
			alertCancel.setOrderId(orderId);
			alertCancel.setUserId(userId);
			alertCancel.setRuleType(RuleType.PreTrade.toString());
			alertCancel.setDescription(description);
			alertCancel.setDimension("N/A");
			alertCancel.setParameters(parameter);
			alertCancel.setSummary(summary);
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
			alertCancel.setRuleType(RuleType.PreTrade.toString());
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
	 * Sends compliance not started alert, stopping any further validation by
	 * sending EOM to trade server
	 * 
	 * @param orderId
	 * @param userId
	 * @param summary
	 * @param description
	 * @throws IOException
	 *             throws IO exception is sending is failed
	 */
	public static void sendComplianceNotStartedAlert(IAmqpSender amqpCancelMessageSender, String orderId, int userId, String summary)
			throws IOException {
		try {
			Alert alertCancel = new Alert();
			alertCancel.setName("Esper is not started completely");
			alertCancel.setCompressionLevel("None");
			alertCancel.setViolated(true);
			alertCancel.setIsEOM(false);
			alertCancel.setOrderId(orderId);
			alertCancel.setUserId(userId);
			alertCancel.setRuleType(RuleType.None.toString());
			alertCancel.setDescription(summary);
			alertCancel.setDimension("N/A");
			alertCancel.setParameters("N/A");
			alertCancel.setActualResult("N/A");
			alertCancel.setThreshold("N/A");
			alertCancel.setSummary("Compliance Engine cannot serve as either Rule Mediator is down or Esper is not started completely.");
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
	 * Sends on going refresh error alert, stopping any further validation by
	 * sending EOM to trade server
	 * 
	 * @param orderId
	 * @param userId
	 * @param summary
	 * @param description
	 * @throws IOException
	 *             throws IO exception is sending is failed
	 */
	public static void sendOnGoingRefreshAlert(IAmqpSender amqpCancelMessageSender, String orderId, int userId,
			String summary) throws IOException {
		try {
			Alert alertCancel = new Alert();
			alertCancel.setName("Calculation engine algorithm unable to complete successfully");
			alertCancel.setCompressionLevel("None");
			alertCancel.setViolated(true);
			alertCancel.setIsEOM(false);
			alertCancel.setOrderId(orderId);
			alertCancel.setUserId(userId);
			alertCancel.setRuleType(RuleType.None.toString());
			alertCancel.setDescription(summary);
			alertCancel.setDimension("N/A");
			alertCancel.setActualResult("N/A");
			alertCancel.setThreshold("N/A");
			alertCancel.setDimension("N/A");
			alertCancel.setParameters("N/A");
			alertCancel.setSummary(summary);
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
			eomMsg.setRuleType(RuleType.PreTrade.toString());
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