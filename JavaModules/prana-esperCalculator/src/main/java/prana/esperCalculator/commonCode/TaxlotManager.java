package prana.esperCalculator.commonCode;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.LinkedHashMap;
import java.util.List;

import com.espertech.esper.common.client.EventBean;
import com.espertech.esper.common.client.fireandforget.EPFireAndForgetQueryResult;

import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.objects.Taxlot;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

public class TaxlotManager {

	static SimpleDateFormat parserSdf = new SimpleDateFormat(ConfigurationConstants.SIMPLE_DATE_FORMAT);

	/**
	 * Sends a Taxlot DTO as an event to the CEP engine for further processing.
	 *
	 * @param taxlot The Taxlot object containing all trade, swap, and metadata
	 *               information.
	 */
	public static void sendTaxlotToCEPEngine(Taxlot taxlot, boolean needsDeletionInBasket) {
		try {
			// If taxlot is updated & post type, first remove old version from CEP
			if ((CEPManager._epRunTime.getURI().equals(ConfigurationConstants.CEP_ENGINE_COMPLIANCE)
					&& taxlot.taxlotState == TaxlotState.Updated && taxlot.taxlotType == TaxlotType.Post)
					|| needsDeletionInBasket) {
				List<Taxlot> oldTaxlots = getTaxlotsFromWindow(taxlot.taxlotId);
				if (oldTaxlots != null && !oldTaxlots.isEmpty()) {
					for (Taxlot oldTaxlot : oldTaxlots) {
						// Send "Deleted" event for each old taxlot
						sendTaxlotEvent(taxlot, oldTaxlot, true);
					}
				}
			}
			// Send the updated/new Taxlot event
			sendTaxlotEvent(taxlot, null, false);
		} catch (Exception ex) {
			PranaLogManager.error("Failed to send Taxlot event to CEP", ex);
		}
	}

	/**
	 * Fetches a Taxlot object from the CEP window for the given taxlotId.
	 *
	 * @param taxlotId the ID of the Taxlot to fetch.
	 * @return the Taxlot object if found, otherwise null.
	 */
	private static List<Taxlot> getTaxlotsFromWindow(String taxlotId) {
		List<Taxlot> taxlots = new ArrayList<>();
		try {
			String query = "Select * from TaxlotWindow " + "where taxlotId = \"" + taxlotId + "\""
					+ "and taxlotType = \"Post\"";
			EPFireAndForgetQueryResult result = CEPManager.executeQuery(query);
			EventBean[] eventBeanArray = result.getArray();

			if (eventBeanArray != null && eventBeanArray.length > 0) {
				for (EventBean eventBean : eventBeanArray) {
					String jsonString = JSONMapper.getStringForObject(eventBean.getUnderlying());
					@SuppressWarnings("unchecked")
					LinkedHashMap<String, Object> dataMap = (LinkedHashMap<String, Object>) (JSONMapper
							.getLinkedHashMap(jsonString)).get("HashMap");

					if (dataMap != null) {
						Taxlot taxlot = mapDataMapToTaxlot(dataMap, taxlotId);
						taxlots.add(taxlot);
					}
				}
			} else {
				PranaLogManager.logOnly("No taxlot found in window for taxlotId: " + taxlotId);
			}
		} catch (Exception ex) {
			PranaLogManager.error("Failed to retrieve taxlots from CEP", ex);
		}
		return taxlots;
	}

	/**
	 * Maps the provided data from a LinkedHashMap to a Taxlot object.
	 * <p>
	 * This method safely extracts values from the given map using
	 * {@link TaxlotManager}'s type-safe utility methods, avoiding null pointer
	 * issues and incorrect type casts.
	 * </p>
	 *
	 * @param dataMap  LinkedHashMap containing taxlot field values.
	 * @param taxlotId Unique taxlot ID to set on the Taxlot object.
	 * @return A populated Taxlot instance, or null if mapping fails.
	 */
	private static Taxlot mapDataMapToTaxlot(LinkedHashMap<String, Object> dataMap, String taxlotId) {
		try {
			Taxlot mappedTaxlot = new Taxlot();
			mappedTaxlot.accountId = TaxlotManager.getIntSafe(dataMap.get("accountId"), "accountId");
			mappedTaxlot.symbol = TaxlotManager.getStringSafe(dataMap.get("symbol"), "symbol");
			mappedTaxlot.assetId = TaxlotManager.getIntSafe(dataMap.get("assetId"), "assetId");
			mappedTaxlot.strategyId = TaxlotManager.getIntSafe(dataMap.get("strategyId"), "strategyId");
			mappedTaxlot.isSwapped = TaxlotManager.getBooleanSafe(dataMap.get("isSwapped"), "isSwapped");
			mappedTaxlot.orderSide = TaxlotManager.getStringSafe(dataMap.get("orderSide"), "orderSide");
			mappedTaxlot.auecLocalDate = TaxlotManager.getDateSafe(dataMap.get("auecLocalDate"), "auecLocalDate",
					parserSdf);
			mappedTaxlot.tradeAttribute1 = TaxlotManager.getStringSafe(dataMap.get("tradeAttribute1"),
					"tradeAttribute1");
			mappedTaxlot.tradeAttribute2 = TaxlotManager.getStringSafe(dataMap.get("tradeAttribute2"),
					"tradeAttribute2");
			mappedTaxlot.tradeAttribute3 = TaxlotManager.getStringSafe(dataMap.get("tradeAttribute3"),
					"tradeAttribute3");
			mappedTaxlot.tradeAttribute4 = TaxlotManager.getStringSafe(dataMap.get("tradeAttribute4"),
					"tradeAttribute4");
			mappedTaxlot.tradeAttribute5 = TaxlotManager.getStringSafe(dataMap.get("tradeAttribute5"),
					"tradeAttribute5");
			mappedTaxlot.tradeAttribute6 = TaxlotManager.getStringSafe(dataMap.get("tradeAttribute6"),
					"tradeAttribute6");
			mappedTaxlot.taxlotId = taxlotId;
			mappedTaxlot.taxlotState = TaxlotState.Deleted;
			mappedTaxlot.taxlotType = TaxlotType.Post;
			return mappedTaxlot;
		} catch (Exception ex) {
			PranaLogManager.error("Failed to map data map to Taxlot for taxlotId: " + taxlotId, ex);
			return null;
		}
	}

	/**
	 * Logs the details of a Taxlot for debugging/auditing purposes.
	 *
	 * @param taxlot the Taxlot to log.
	 */
	private static void logTaxlotDetails(Taxlot taxlot, boolean isDeleteEvent) {
		try {
			PranaLogManager.logOnly(String.format(
					"Taxlot Details for" + " symbol: %s," + " with isDeleteEvent: %s" + " taxlotId: %s,"
							+ " taxlotType: %s," + " taxlotState: %s,"
							+ " quantity: %f," + " avgPrice: %f," + " accountId: %d," + " assetId: %d,"
							+ " strategyId: %d," + " orderSide: %s," + " isSwapped: %b," + " auecLocalDate: %s" + " tradeAttribute1: %s,"
							+ " tradeAttribute2: %s," + " tradeAttribute3: %s," + " tradeAttribute4: %s,"
							+ " tradeAttribute5: %s," + " tradeAttribute6: %s",
					taxlot.symbol, isDeleteEvent, taxlot.taxlotId, taxlot.taxlotType.toString(),
					taxlot.taxlotState.toString(),
					taxlot.quantity, taxlot.avgPrice, taxlot.accountId, taxlot.assetId, taxlot.strategyId,
					taxlot.orderSide, taxlot.isSwapped,taxlot.auecLocalDate, taxlot.tradeAttribute1, taxlot.tradeAttribute2,
					taxlot.tradeAttribute3, taxlot.tradeAttribute4, taxlot.tradeAttribute5, taxlot.tradeAttribute6));
		} catch (Exception ex) {
			PranaLogManager.error("Failed to send Taxlot event to CEP", ex);
		}
	}

	/**
	 * Builds and sends a Taxlot event to the CEP engine.
	 *
	 * @param taxlot        the current Taxlot data.
	 * @param oldTaxlot     the old Taxlot data (used only for delete events).
	 * @param state         the taxlotState to send ("Deleted", "New", etc.).
	 * @param isDeleteEvent whether this is a deletion event (uses oldTaxlot data
	 *                      for some fields).
	 */
	private static void sendTaxlotEvent(Taxlot taxlot, Taxlot oldTaxlot, boolean isDeleteEvent) {
		try {
			// Decide the source of certain fields once based on delete event flag
			Taxlot source = isDeleteEvent ? oldTaxlot : taxlot;

			if (source.isSwapped && !isDeleteEvent) {
				source.assetId = 0;
				source.asset = "EquitySwap";
			}
			if(CEPManager._epRunTime.getURI().equals(ConfigurationConstants.CEP_ENGINE_COMPLIANCE))
				logTaxlotDetails(source, isDeleteEvent);

			Object[] eventData = new Object[] { taxlot.basketId, taxlot.taxlotId, taxlot.clOrderId,
					source.taxlotType.toString(), source.taxlotState.toString(), source.symbol, taxlot.underlyingSymbol,
					taxlot.quantity, taxlot.avgPrice, taxlot.orderSideTagValue, source.accountId, taxlot.counterPartyId,
					taxlot.venueId, source.auecLocalDate, taxlot.settlementDate, taxlot.userId, source.strategyId,
					taxlot.orderTypeTagValue, taxlot.benchMarkRate, taxlot.differential, taxlot.swapNotional,
					taxlot.dayCount, source.isSwapped, taxlot.avgFxRateForTrade, taxlot.conversionMethodOperator,
					taxlot.tif, source.orderSide, taxlot.counterParty, taxlot.venue, taxlot.sideMultiplier,
					taxlot.orderType, taxlot.underlyingAsset, taxlot.limitPrice, taxlot.stopPrice,
					taxlot.isWhatIfTradeStreamRequired, source.assetId, taxlot.asset, taxlot.auecId, taxlot.multiplier,
					source.tradeAttribute1, source.tradeAttribute2, source.tradeAttribute3, source.tradeAttribute4,
					source.tradeAttribute5, source.tradeAttribute6 };

			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(eventData, "Taxlot");
		} catch (Exception ex) {
			PranaLogManager.error("Failed to send Taxlot event to CEP", ex);
		}
	}

	/**
	 * Safely retrieves a string value from the provided object.
	 *
	 * @param value The object to convert to string.
	 * @param key   The field name, used for logging in case of error.
	 * @return The string representation, or empty string if null or error occurs.
	 */
	public static String getStringSafe(Object value, String key) {
		try {
			return value != null ? value.toString() : "";
		} catch (Exception ex) {
			PranaLogManager.error("Error getting String for key: " + key, ex);
			return "";
		}
	}

	/**
	 * Safely retrieves an integer value from the provided object.
	 *
	 * @param value The object to parse as integer.
	 * @param key   The field name, used for logging in case of error.
	 * @return The parsed integer value, or 0 if null or error occurs.
	 */
	public static int getIntSafe(Object value, String key) {
		try {
			return value != null ? Integer.parseInt(value.toString()) : 0;
		} catch (Exception ex) {
			PranaLogManager.error("Error getting int for key: " + key, ex);
			return 0;
		}
	}

	/**
	 * Safely retrieves a double value from the provided object.
	 *
	 * @param value The object to parse as double.
	 * @param key   The field name, used for logging in case of error.
	 * @return The parsed double value, or 0.0 if null or error occurs.
	 */
	public static double getDoubleSafe(Object value, String key) {
		try {
			return value != null ? Double.parseDouble(value.toString()) : 0.0;
		} catch (Exception ex) {
			PranaLogManager.error("Error getting double for key: " + key, ex);
			return 0.0;
		}
	}

	/**
	 * Safely retrieves a boolean value from the provided object.
	 *
	 * @param value The object to parse as boolean.
	 * @param key   The field name, used for logging in case of error.
	 * @return The parsed boolean value, or false if null or error occurs.
	 */
	public static boolean getBooleanSafe(Object value, String key) {
		try {
			return value != null && Boolean.parseBoolean(value.toString());
		} catch (Exception ex) {
			PranaLogManager.error("Error getting boolean for key: " + key, ex);
			return false;
		}
	}

	/**
	 * Safely parses a date from the provided object using the given formatter.
	 *
	 * @param value The object to parse as Date.
	 * @param key   The field name, used for logging in case of error.
	 * @param sdf   The date formatter to use for parsing.
	 * @return The parsed Date object, or null if value is null or parsing fails.
	 */
	public static Date getDateSafe(Object value, String key, SimpleDateFormat sdf) {
		try {
			return value != null ? sdf.parse(value.toString()) : null;
		} catch (Exception ex) {
			PranaLogManager.error("Error parsing date for key: " + key, ex);
			return null;
		}
	}

	/**
	 * Enum representing the possible states of a taxlot.
	 */
	public enum TaxlotState {
		Deleted, New, Updated, None
	}

	/**
	 * Enum representing the different types of taxlots.
	 */
	public enum TaxlotType {
		WhatIf, Post, InTradeMarket, InTradeStage;
	}

	/*
	 * Checks if a taxlot is swapped and logs unexpected scenarios.
	 *
	 * This method is used to detect cases where `isSwapped` is false
	 * while `swapParameters` are present, which should normally not occur.
	 * It logs the calling class, method, and line number to help trace the cause.
	 *
	 * @param isSwappedValue The value representing whether the taxlot is swapped.
	 * @param swapParameters Additional swap-related parameters that may be present.
	 * @return true if the taxlot is considered swapped after this check; false otherwise.
	 */
	public static boolean checkAndLogSwap(Object isSwappedValue, Object swapParameters) {
	    boolean isSwapped = getBooleanSafe(isSwappedValue, "IsSwapped");

	    if (!isSwapped && swapParameters != null) {
	        logCallerInfo(isSwappedValue, swapParameters);
	        isSwapped = true;
	    }

	    return isSwapped;
	}

	/*
	 * Logs the caller's class, method, and line number along with the swap scenario.
	 * Any exceptions during logging are caught and reported via PranaLogManager.error.
	 */
	public static void logCallerInfo(Object isSwappedValue, Object swapParameters) {
	    try {
	        StackTraceElement[] stackTrace = Thread.currentThread().getStackTrace();
	        StackTraceElement caller = stackTrace.length > 2 ? stackTrace[2] : stackTrace[1];

	        PranaLogManager.logOnly(String.format(
	            "Unexpected call from: %s.%s (Line %d)",
	            caller.getClassName(), caller.getMethodName(), caller.getLineNumber()
	        ));
	        PranaLogManager.info(String.format(
	            "IsSwapped is false and swapParameters is not null. isSwapped: %s, swapParameters: %s",
	            isSwappedValue, swapParameters
	        ));
	    } catch (Exception e) {
	        PranaLogManager.error("Error while logging unexpected swap scenario", e);
	    }
	}
}
