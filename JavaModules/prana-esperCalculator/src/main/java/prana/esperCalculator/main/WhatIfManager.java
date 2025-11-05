package prana.esperCalculator.main;

import java.io.IOException;
import java.math.BigDecimal;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;

import com.espertech.esper.common.client.fireandforget.EPFireAndForgetQueryResult;

import prana.amqpAdapter.AmqpHelper;
import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.esperCalculator.constants.CollectorConstants;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.commonCode.CEPManager;
import prana.esperCalculator.commonCode.TaxlotManager;
import prana.esperCalculator.commonCode.TaxlotManager.TaxlotState;
import prana.esperCalculator.commonCode.TaxlotManager.TaxlotType;
import prana.esperCalculator.communication.DataInitializationRequestProcessor;
import prana.esperCalculator.esperCEP.WhatIfHelper;
import prana.esperCalculator.objects.Taxlot;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;
import prana.esperCalculator.cacheClasses.WhatIfCacheObject;

/**
 * This class handles all operations required for whatif order
 * 
 * @author dewashish
 * 
 */
public class WhatIfManager {
	/**
	 * Description used when livefeed is disconnected and trade is canceled
	 */
	static String _liveFeedDisconnectedDescription = "LiveFeed disconnected";

	/**
	 * Description used when information is missing in compliance
	 */
	static String _missingInformationDescription = "Missing information";

	/**
	 * Summary used when livefeed is disconnected and trade is canceled
	 */
	static String _liveFeedDisconnectedSummary = "LiveFeed is  not available, so cannot validate all rules. Please contact Nirvana Support to check livefeed connection.";

	/**
	 * Singleton instance
	 */
	private static WhatIfManager _whatIfManager;

	private static SimpleDateFormat _simplesdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss.SSS");

	/**
	 * Getinstance method for singleton pattern
	 * 
	 * @return singleton instance in memory
	 */
	public static WhatIfManager getInstance() {
		if (_whatIfManager == null)
			_whatIfManager = new WhatIfManager();
		return _whatIfManager;
	}

	/**
	 * Livefeed status, used to send request to pricing only if livefeed is
	 * connected
	 */
	private static boolean _liveFeedStatus = false;

	/**
	 * This amqp sender is used to send cancel message to trade server (currently in
	 * case if live feed is disconnected)
	 */
	private IAmqpSender _amqpCancelMessageSender;

	/**
	 * This simpledate format is used to convert date into proper format for sending
	 * in esper engine
	 */
	private SimpleDateFormat parserSdf;

	/**
	 * A list of asset classes that require pricing from pricing server
	 */
	public String[] assetsRequiringPricing;

	/**
	 * Summary for whatIfTaxlot
	 */
	static String whatIfTaxlotSummary;
	
	/**
	 * Security Retry Count
	 */
	public static int securityRetryCount = 10;

	/**
	 * Private constructor to implement singleton instance
	 */
	private WhatIfManager() {
		try {
			// Initializing AmqpSender for cancellation
			String otherDataExchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST, ConfigurationConstants.KEY_EXCHANGE_OTHERDATA);

			_amqpCancelMessageSender = AmqpHelper.getSender(otherDataExchangeName, ExchangeType.Direct,
					MediaType.Exchange, false);

			// SimpleDateFormat initialization
			parserSdf = new SimpleDateFormat(ConfigurationConstants.SIMPLE_DATE_FORMAT);

			String priceRequired = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_PRICING_REQUIRED);
			securityRetryCount = Integer.parseInt(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_SECURITY_RETRY_COUNT));

			assetsRequiringPricing = priceRequired.split("/");

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Handles the operation when any what if order is arrived
	 * 
	 * @param whatIfTaxlot
	 *                     What if Order
	 */

	public void handleWhatIfOrder(HashMap<String, Object> whatIfTaxlot) {
		try {
			/**
			 * The logic : get the taxlot check if eom if not then store else send to Esper
			 */
			if (whatIfTaxlot.containsKey("IsEom")) {
				PendingWhatIfCache.getInstance().startTimer();
				String basketId = whatIfTaxlot.get("BasketId").toString();
				int count = Integer.parseInt(whatIfTaxlot.get("Count").toString());
				if (!DataInitializationRequestProcessor.getInstance()._isRuleMediatorInitialized) {
					WhatIfHelper.sendComplianceNotStartedAlert(_amqpCancelMessageSender, basketId,
							PendingWhatIfCache.getInstance().getUserId(basketId), whatIfTaxlotSummary);
					// Send an EOM to server after esper disconnected alert
					WhatIfHelper.sendEomMessage(_amqpCancelMessageSender, basketId,
							PendingWhatIfCache.getInstance().getUserId(basketId));
					PendingWhatIfCache.getInstance().removeFromCache(basketId);
					return;
				}

				if (DataInitializationRequestProcessor.getInstance().getRefreshFlag()) {
					WhatIfHelper.sendOnGoingRefreshAlert(_amqpCancelMessageSender, basketId,
							PendingWhatIfCache.getInstance().getUserId(basketId), whatIfTaxlotSummary);
					// Send an EOM to server after on-going refresh alert
					WhatIfHelper.sendEomMessage(_amqpCancelMessageSender, basketId,
							PendingWhatIfCache.getInstance().getUserId(basketId));
					PendingWhatIfCache.getInstance().removeFromCache(basketId);
					return;
				}

				PranaLogManager.info("EOM received for pre trade. BasketId: " + basketId + " [" + count + "] @"
						+ _simplesdf.format(new Date()));
				HashSet<String> symbolsToValidate = new HashSet<String>();

				for (String symbol : PendingWhatIfCache.getInstance().getRequiredSymbols(basketId)) {
					// If the SymbolData is already exist for the symbol then no need to request for
					// snapshot
					boolean isSymbolDataAvailable = WhatIfHelper.getSymbolDataInformationForSymbol(symbol);
					PranaLogManager.logOnly(symbol + ", security requested for whatif order.");
					boolean isSecurityDetailsAvailable = WhatIfHelper.getSecurityInformationForSymbol(symbol);
					if (!isSecurityDetailsAvailable) {
						throw new Exception();
					} else if (!isSymbolDataAvailable) {
						symbolsToValidate.add(symbol);
					}
				}
				if (PendingWhatIfCache.getInstance().eomReceived(basketId, count)) {
					// Pricing not required for any symbol send all tax lots to compliance
					int userId = 0;
					HashMap<Integer, List<HashMap<String, Object>>> sortedTaxlots = PendingWhatIfCache.getInstance()
							.getSortedTaxlotsinBasket(basketId);
					if (sortedTaxlots.size() > 0) {
						for (int key : sortedTaxlots.keySet()) {
							for (HashMap<String, Object> taxlot : sortedTaxlots.get(key)) {
								String symbol = taxlot.get(CollectorConstants.SYMBOL).toString();
								if (symbolsToValidate.size() > 0 && symbolsToValidate.contains(symbol)) {
									overrideSymbolDataForPricesNotAvailable(taxlot, symbol);
									symbolsToValidate.remove(symbol);
								}
								sendWhatIfTaxlot(taxlot);
								int tempUser = Integer.parseInt(taxlot.get("CompanyUserID").toString());
								if (userId == 0)
									userId = tempUser;
								else if (userId != tempUser)
									userId = -1;
							}
						}
						// Send EOM to compliance
						sendEomToEsper(basketId, userId);
					}
				} else {
					WhatIfHelper.sendComplianceFailAlert(_amqpCancelMessageSender, basketId,
							PendingWhatIfCache.getInstance().getUserId(basketId));
					WhatIfHelper.sendEomMessage(_amqpCancelMessageSender, basketId,
							PendingWhatIfCache.getInstance().getUserId(basketId));
					PendingWhatIfCache.getInstance().removeFromCache(basketId);
				}
			} else {
				PendingWhatIfCache.getInstance().addWhatIfOrder(whatIfTaxlot);
				whatIfTaxlotSummary = "Compliance failed to process correctly "
						+ whatIfTaxlot.get("OrderSide").toString() + " " + whatIfTaxlot.get("Symbol").toString() + " "
						+ whatIfTaxlot.get("ExecutedQty") + " @" + whatIfTaxlot.get("AvgPrice").toString() + " for "
						+ whatIfTaxlot.get("OrderType").toString() + " Order";
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/*
	 * private void sendLiveFeedCancelAlert(String basketId) { try {
	 * WhatIfHelper.sendCancellationMessage(_amqpCancelMessageSender, basketId,
	 * PendingWhatIfCache.getInstance().getUserId(basketId),
	 * _liveFeedDisconnectedDescription, _liveFeedDisconnectedSummary);
	 * 
	 * // Send an EOM to server after live feed disconnected alert
	 * WhatIfHelper.sendEomMessage(_amqpCancelMessageSender, basketId,
	 * PendingWhatIfCache.getInstance().getUserId(basketId));
	 * PendingWhatIfCache.getInstance().removeFromCache(basketId);
	 * PranaLogManager.info("Cancelled message because livefeed is not available");
	 * } catch (Exception ex) { PranaLogManager.error(ex); } }
	 */

	private void sendEomToEsper(String basketId, int userId) {
		PranaLogManager.info("Starting aggregation for pre trade. BasketId: " + basketId + ", UserId: " + userId + " @"
				+ _simplesdf.format(new Date()));
		CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] { basketId, userId },
				"BasketAggreation");
		PranaLogManager.info("Starting sending data for pre trade. BasketId: " + basketId + ", UserId: " + userId + " @"
				+ _simplesdf.format(new Date()));
		CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] { basketId, userId },
				"BasketEOM");
		long estimatedTime = System.currentTimeMillis() - PendingWhatIfCache.getInstance().getStartTime();
		PendingWhatIfCache.getInstance().removeFromCache(basketId);
		if (!PendingWhatIfCache.getInstance().checkIfExistsInCache(basketId)) {
			PranaLogManager.info("Pre trade processed. BasketId: " + basketId + ", UserId: " + userId
					+ " ,Time taken in sec is: " + estimatedTime / 1000 + " " + " @" + _simplesdf.format(new Date()));
		} else {
			PranaLogManager.info("Pre trade not processed correctly. BasketId: " + basketId + ", UserId: " + userId
					+ " ,Time taken in sec is: " + estimatedTime / 1000 + " " + " @" + _simplesdf.format(new Date()));
		}
	}

	/**
	 * Handles the event when Livefeed status is changed
	 * 
	 * @param status
	 *               either true of false
	 */
	public void liveFeedStatusChanged(boolean status) {
		try {

			if (_liveFeedStatus != status) {
				_liveFeedStatus = status;
				if (!_liveFeedStatus) {
					for (String basketId : PendingWhatIfCache.getInstance().getPendingOrders()) {

						WhatIfHelper.sendCancellationMessage(_amqpCancelMessageSender, basketId,
								PendingWhatIfCache.getInstance().getUserId(basketId), _liveFeedDisconnectedDescription,
								_liveFeedDisconnectedSummary);

						WhatIfHelper.sendEomMessage(_amqpCancelMessageSender, basketId,
								PendingWhatIfCache.getInstance().getUserId(basketId));
					}

					// Clearing pending taxlot if livefeed is disconnected
					PendingWhatIfCache.getInstance().clear();
				}

				DateFormat dateFormat = new SimpleDateFormat("MM/dd/yyyy HH:mm:ss");
				Date date = new Date();
				PranaLogManager
						.info("LiveFeed Status Changed to " + _liveFeedStatus + " at " + dateFormat.format(date));
			}
		} catch (IOException ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Request to pricing for livefeed of given security
	 * 
	 * @param symbol
	 * @return true is sending successful
	 * @throws Exception
	 *                   throws exception id could not request to pricing
	 */
	/*
	 * private boolean requestForSymbol(HashMap<String, Object> res) throws
	 * Exception { try { if (res != null) { res.put("TypeOfRequest",
	 * "LiveFeedRequestSnapShot"); String jsonObject =
	 * JSONMapper.getStringForObject(res);
	 * _amqpPriceRequestSender.sendData(jsonObject, "Pricing");
	 * 
	 * PranaLogManager.info("Snapshotrequest: " + jsonObject); return true; } }
	 * catch (Exception ex) { PranaLogManager.error(ex.getMessage(), ex); throw ex;
	 * } return false; }
	 */
	/**
	 * Performs operations when symboldata is received for requested symbol
	 * 
	 * @param symbolData
	 */
	public void whatIfSymbolDataReceived(LinkedHashMap<String, Object> symbolData) {
		try {
			if (symbolData != null) {
				PranaLogManager.info("SymbolData received for symbol " + symbolData.get(CollectorConstants.SYMBOL)
						+ " through WhatIfSymbolData routingKey");
				/*
				 * Getting all baskets for which all symbol's data is received.
				 */
				HashMap<String, WhatIfCacheObject> baskets = PendingWhatIfCache.getInstance()
						.getBasketsForSymbols(symbolData.get(CollectorConstants.SYMBOL).toString());

				for (String basketId : baskets.keySet()) {
					baskets.get(basketId).setIsPricesReceived(true);
				}
				// Sending SymbolData
				sendSymbolData(symbolData);

				String response = WindowValidator.getInstance()
						.check(symbolData.get(CollectorConstants.SYMBOL).toString());

				PendingWhatIfCache.getInstance().removeBasketsFromCollectionForSymbols(baskets);
				for (String basketId : baskets.keySet()) {
					int userId = 0;
					for (HashMap<String, Object> taxlot : baskets.get(basketId).getTaxlots()) {
						sendWhatIfTaxlot(taxlot);
						int tempUser = Integer.parseInt(taxlot.get("CompanyUserID").toString());
						if (userId == 0)
							userId = tempUser;
						else if (userId != tempUser)
							userId = -1;

						if (response != null) {
							WhatIfHelper.sendMissingInformationAlert(_amqpCancelMessageSender, basketId, userId,
									_missingInformationDescription, response,
									"Symbol : " + symbolData.get(CollectorConstants.SYMBOL).toString());
						}
					}
					sendEomToEsper(basketId, userId);
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/*
	 * Sending SymbolData for the symbol for which prices are not available
	 */
	public void overrideSymbolDataForPricesNotAvailable(HashMap<String, Object> taxlot, String symbol) {
		try {
			if (taxlot != null) {
				PranaLogManager.logOnly("SymbolData is Sending with avg/limit/stop price for symbol " + symbol);
				PendingWhatIfCache.setIsValidatedSymbolDataReceived(true);

				String conversionMethod = taxlot.get(CollectorConstants.FX_CONVERSION_METHOD).toString();

				CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] { symbol,
						taxlot.get(CollectorConstants.UNDERLYING_SYMBOL), 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
						Double.parseDouble(taxlot.get(CollectorConstants.AVG_PRICE).toString()), conversionMethod,
						Double.parseDouble(taxlot.get(CollectorConstants.MARK_PRICE).toString()), 1.0, 1.0,
						Integer.parseInt(taxlot.get(CollectorConstants.ASSET_ID).toString()), 0.0, 0.0,
						BigDecimal.ZERO, 0 }, CollectorConstants.SYMBOL_DATA_EVENT_NAME);
				PendingWhatIfCache.setIsValidatedSymbolDataReceived(false);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Performs operations when symboldata is received if symbol gets validated on
	 * TT
	 * 
	 * @param symbolData
	 */
	public void ValidatedSymbolDataReceivedFromTT(LinkedHashMap<String, Object> symbolData) {
		try {
			if (symbolData != null) {
				PranaLogManager.logOnly("SymbolData received for symbol " + symbolData.get(CollectorConstants.SYMBOL)
						+ " through SymbolValidatedFromTT");
				PendingWhatIfCache.setIsValidatedSymbolDataReceived(true);
				// Sending SymbolData
				sendSymbolData(symbolData);
				PendingWhatIfCache.setIsValidatedSymbolDataReceived(false);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Sends symboldata
	 * 
	 * @param symbolData
	 */
	private void sendSymbolData(LinkedHashMap<String, Object> symbolData) {
		/* FxSymbolData fields */
		String conversionMethod = CollectorConstants.CONVERSION_METHOD_OPERATOR_MULTIPLY;

		double delta = 1;
		if (symbolData.containsKey(CollectorConstants.DELTA)) {
			delta = Double.valueOf(symbolData.get(CollectorConstants.DELTA).toString());
			if (delta == 0)
				delta = 1;
		}

		if (symbolData.containsKey(CollectorConstants.CONVERSION_METHOD)) {
			int methodId = Integer.parseInt(symbolData.get(CollectorConstants.CONVERSION_METHOD).toString());
			if (methodId == 1)
				conversionMethod = CollectorConstants.CONVERSION_METHOD_OPERATOR_DIVIDE;
			else
				conversionMethod = CollectorConstants.CONVERSION_METHOD_OPERATOR_MULTIPLY;
		}
		double beta5yr = 1;
		if (symbolData.containsKey(CollectorConstants.BETA_5_YR)) {
			beta5yr = Double.valueOf(symbolData.get(CollectorConstants.BETA_5_YR).toString());
			if (beta5yr == 0)
				beta5yr = 1;
		}

		// First converting SharesOutStanding object value to double and then getting
		// the BigDecimal value. 
		// https://jira.nirvanasolutions.com:8443/browse/PRANA-37833
		BigDecimal sharesOutStandings = BigDecimal
				.valueOf(((Number) symbolData.get(CollectorConstants.SHARES_OUTSTANDING)).doubleValue());
		int pricingStatus = Integer.parseInt(symbolData.get(CollectorConstants.PRICING_STATUS).toString());
		
		CEPManager.getEPRuntime().getEventService()
				.sendEventObjectArray(new Object[] { symbolData.get(CollectorConstants.SYMBOL),
						symbolData.get(CollectorConstants.UNDERLYING_SYMBOL), symbolData.get(CollectorConstants.ASK),
						symbolData.get(CollectorConstants.BID), symbolData.get(CollectorConstants.LOW),
						symbolData.get(CollectorConstants.HIGH), symbolData.get(CollectorConstants.OPEN), 0.0, // XXX:
																												// Closing
																												// price
																												// might
																												// need
																												// to
																												// remove
						symbolData.get(CollectorConstants.LAST_PRICE),
						symbolData.get(CollectorConstants.SELECTED_FEED_PRICE), conversionMethod, // Might not
																									// be in
																									// symbolData
																									// of
																									// different
																									// asset
																									// type
						symbolData.get(CollectorConstants.MARK_PRICE), delta, beta5yr,
						symbolData.get(CollectorConstants.CATEGORY_CODE),
						symbolData.get(CollectorConstants.OPEN_INTEREST),
						symbolData.get(CollectorConstants.AVERAGE_VOLUME_20_DAY), sharesOutStandings, pricingStatus },
						CollectorConstants.SYMBOL_DATA_EVENT_NAME);
	}

	// Sends what if taxlot to Esper Engine.
	private void sendWhatIfTaxlot(HashMap<String, Object> whatIfTaxlot) {
		try {
			String symbol = TaxlotManager.getStringSafe(whatIfTaxlot.get("Symbol"), "Symbol");
			int accountId = TaxlotManager.getIntSafe(whatIfTaxlot.get("Level1ID"), "Level1ID");
			int strategyId = TaxlotManager.getIntSafe(whatIfTaxlot.get("Level2ID"), "Level2ID");
			String basketId = TaxlotManager.getStringSafe(whatIfTaxlot.get("GroupID"), "GroupID");

			PranaLogManager.logOnly("WhatIf taxlot sending for symbol: " + symbol + ", basketId: " + basketId);

			Date aUECLocalDate = TaxlotManager.getDateSafe(whatIfTaxlot.get("AUECLocalDate"), "AUECLocalDate",
					parserSdf);
			Date settlementDate = TaxlotManager.getDateSafe(whatIfTaxlot.get("SettlementDate"), "SettlementDate",
					parserSdf);
			String clOrderId = TaxlotManager.getStringSafe(whatIfTaxlot.get("LotId"), "LotId");

			@SuppressWarnings("unchecked")
			HashMap<String, Object> swapParameters = (HashMap<String, Object>) whatIfTaxlot.get("SwapParameters");
			boolean isSwap = TaxlotManager.checkAndLogSwap(whatIfTaxlot.get("IsSwapped"), swapParameters);

			double benchMarkRate = 0.0;
			double differential = 0.0;
			double swapNotional = 0.0;
			double dayCount = 0.0;

			if (isSwap) {
				benchMarkRate = TaxlotManager.getDoubleSafe(swapParameters.get("BenchMarkRate"), "BenchMarkRate");
				differential = TaxlotManager.getDoubleSafe(swapParameters.get("Differential"), "Differential");
				swapNotional = TaxlotManager.getDoubleSafe(swapParameters.get("NotionalValue"), "NotionalValue");
				dayCount = TaxlotManager.getDoubleSafe(swapParameters.get("DayCount"), "DayCount");
			}

			Taxlot whatIfTaxlotObj = new Taxlot();
			whatIfTaxlotObj.basketId = basketId;
			whatIfTaxlotObj.taxlotId = TaxlotManager.getStringSafe(whatIfTaxlot.get("TaxLotID"), "TaxLotID");
			whatIfTaxlotObj.clOrderId = clOrderId;
			whatIfTaxlotObj.taxlotType = TaxlotType.WhatIf;
			whatIfTaxlotObj.taxlotState = TaxlotState.New;
			whatIfTaxlotObj.symbol = symbol;
			whatIfTaxlotObj.underlyingSymbol = TaxlotManager.getStringSafe(whatIfTaxlot.get("UnderlyingSymbol"),
					"UnderlyingSymbol");
			whatIfTaxlotObj.quantity = TaxlotManager.getDoubleSafe(whatIfTaxlot.get("TaxLotQty"), "TaxLotQty");
			whatIfTaxlotObj.avgPrice = TaxlotManager.getDoubleSafe(whatIfTaxlot.get("AvgPrice"), "AvgPrice");
			whatIfTaxlotObj.orderSideTagValue = TaxlotManager.getStringSafe(whatIfTaxlot.get("OrderSideTagValue"),
					"OrderSideTagValue");
			whatIfTaxlotObj.accountId = accountId;
			whatIfTaxlotObj.counterPartyId = TaxlotManager.getIntSafe(whatIfTaxlot.get("CounterPartyID"),
					"CounterPartyID");
			whatIfTaxlotObj.venueId = TaxlotManager.getIntSafe(whatIfTaxlot.get("VenueID"), "VenueID");
			whatIfTaxlotObj.auecLocalDate = aUECLocalDate;
			whatIfTaxlotObj.settlementDate = settlementDate;
			whatIfTaxlotObj.userId = TaxlotManager.getStringSafe(whatIfTaxlot.get("CompanyUserID"), "CompanyUserID");
			whatIfTaxlotObj.strategyId = strategyId;
			whatIfTaxlotObj.orderTypeTagValue = TaxlotManager.getStringSafe(whatIfTaxlot.get("OrderTypeTagValue"),
					"OrderTypeTagValue");
			whatIfTaxlotObj.benchMarkRate = benchMarkRate;
			whatIfTaxlotObj.differential = differential;
			whatIfTaxlotObj.swapNotional = swapNotional;
			whatIfTaxlotObj.dayCount = dayCount;
			whatIfTaxlotObj.isSwapped = isSwap;
			whatIfTaxlotObj.avgFxRateForTrade = TaxlotManager.getDoubleSafe(whatIfTaxlot.get("FXRate"), "FXRate");
			whatIfTaxlotObj.conversionMethodOperator = "M";
			whatIfTaxlotObj.tif = TaxlotManager.getStringSafe(whatIfTaxlot.get("TIF"), "TIF");
			whatIfTaxlotObj.orderSide = TaxlotManager.getStringSafe(whatIfTaxlot.get("OrderSide"), "OrderSide");
			whatIfTaxlotObj.counterParty = TaxlotManager.getStringSafe(whatIfTaxlot.get("CounterPartyName"),
					"CounterPartyName");
			whatIfTaxlotObj.venue = TaxlotManager.getStringSafe(whatIfTaxlot.get("Venue"), "Venue");
			whatIfTaxlotObj.sideMultiplier = TaxlotManager.getIntSafe(whatIfTaxlot.get("SideMultiplier"),
					"SideMultiplier");
			whatIfTaxlotObj.orderType = TaxlotManager.getStringSafe(whatIfTaxlot.get("OrderType"), "OrderType");
			whatIfTaxlotObj.underlyingAsset = TaxlotManager.getStringSafe(whatIfTaxlot.get("UnderlyingName"),
					"UnderlyingName");
			whatIfTaxlotObj.limitPrice = TaxlotManager.getDoubleSafe(whatIfTaxlot.get("LimitPrice"), "LimitPrice");
			whatIfTaxlotObj.stopPrice = TaxlotManager.getDoubleSafe(whatIfTaxlot.get("StopPrice"), "StopPrice");
			whatIfTaxlotObj.isWhatIfTradeStreamRequired = TaxlotManager
					.getBooleanSafe(whatIfTaxlot.get("IsWhatIfTradeStreamRequired"), "IsWhatIfTradeStreamRequired");
			whatIfTaxlotObj.assetId = TaxlotManager.getIntSafe(whatIfTaxlot.get("AssetID"), "AssetID");
			whatIfTaxlotObj.asset = TaxlotManager.getStringSafe(whatIfTaxlot.get("AssetName"), "AssetName");
			whatIfTaxlotObj.auecId = TaxlotManager.getIntSafe(whatIfTaxlot.get("AUECID"), "AUECID");
			whatIfTaxlotObj.multiplier = TaxlotManager.getDoubleSafe(whatIfTaxlot.get("ContractMultiplier"),
					"ContractMultiplier");
			whatIfTaxlotObj.tradeAttribute1 = TaxlotManager.getStringSafe(whatIfTaxlot.get("TradeAttribute1"),
					"TradeAttribute1");
			whatIfTaxlotObj.tradeAttribute2 = TaxlotManager.getStringSafe(whatIfTaxlot.get("TradeAttribute2"),
					"TradeAttribute2");
			whatIfTaxlotObj.tradeAttribute3 = TaxlotManager.getStringSafe(whatIfTaxlot.get("TradeAttribute3"),
					"TradeAttribute3");
			whatIfTaxlotObj.tradeAttribute4 = TaxlotManager.getStringSafe(whatIfTaxlot.get("TradeAttribute4"),
					"TradeAttribute4");
			whatIfTaxlotObj.tradeAttribute5 = TaxlotManager.getStringSafe(whatIfTaxlot.get("TradeAttribute5"),
					"TradeAttribute5");
			whatIfTaxlotObj.tradeAttribute6 = TaxlotManager.getStringSafe(whatIfTaxlot.get("TradeAttribute6"),
					"TradeAttribute6");

			// Send to CEP
			TaxlotManager.sendTaxlotToCEPEngine(whatIfTaxlotObj, false);

			PranaLogManager.logOnly("WhatIf taxlot sent for symbol: " + symbol + ", basketId: " + basketId);

			EPFireAndForgetQueryResult result = CEPManager
					.executeQuery("Select masterFundId from AccountWindow Where accountId = " + accountId);
			int masterFundId = result.getArray().length > 0
					? TaxlotManager.getIntSafe(result.getArray()[0].get("masterFundId"), "masterFundId")
					: -1;

			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(
					new Object[] { symbol, accountId, masterFundId, strategyId, TaxlotManager.TaxlotState.New.toString() },
					"StartNavCalculationsForPreTrade");

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}