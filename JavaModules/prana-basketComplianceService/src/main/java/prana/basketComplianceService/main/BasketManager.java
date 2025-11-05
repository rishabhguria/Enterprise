package prana.basketComplianceService.main;

import java.math.BigDecimal;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.List;

import prana.amqpAdapter.AmqpHelper;
import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.basketComplianceService.basketCEP.WhatIfHelper;
import prana.basketComplianceService.communication.DataInitializationRequestProcessor;
import prana.esperCalculator.constants.CollectorConstants;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.main.PendingWhatIfCache;
import prana.esperCalculator.objects.SymbolData;
import prana.esperCalculator.objects.Taxlot;
import prana.esperCalculator.commonCode.CEPManager;
import prana.esperCalculator.commonCode.TaxlotManager;
import prana.esperCalculator.commonCode.TaxlotManager.TaxlotState;
import prana.esperCalculator.commonCode.TaxlotManager.TaxlotType;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

/**
 * This class handles all operations required for Basket order
 * 
 * @author Ankit
 * 
 */
public class BasketManager {

	/**
	 * Singleton instance
	 */
	private static BasketManager _basketManager;

	/**
	 * Get instance method for singleton pattern
	 * 
	 * @return singleton instance in memory
	 */
	public static BasketManager getInstance() {
		if (_basketManager == null)
			_basketManager = new BasketManager();
		return _basketManager;
	}

	/**
	 * This AMQP sender is used to send cancel message to trade server (currently in
	 * case if live feed is disconnected)
	 */
	private IAmqpSender _amqpCancelMessageSender;

	/**
	 * This simple date format is used to convert date into proper format for
	 * sending in ESPER engine
	 */
	private SimpleDateFormat _parserSdf;

	private boolean _isRequestSent = false;
	private boolean _isRealTimePositions = Boolean.parseBoolean(CEPManager.getVariableValue("IsRealTimePositions").toString());

	/**
	 * Private constructor to implement singleton instance
	 */
	private BasketManager() {
		try {
			// Initializing AmqpSender for cancellation
			String otherDataExchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST, ConfigurationConstants.KEY_EXCHANGE_OTHERDATA);

			_amqpCancelMessageSender = AmqpHelper.getSender(otherDataExchangeName, ExchangeType.Direct,
					MediaType.Exchange, false);

			// SimpleDateFormat initialization
			_parserSdf = new SimpleDateFormat(ConfigurationConstants.SIMPLE_DATE_FORMAT);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	int _sleep = 100;
	List<Integer> _accountIdList = new ArrayList<Integer>();
	List<String> _symbolList = new ArrayList<String>();

	/**
	 * Handles the operation when any what if order is arrived
	 * 
	 * @param basketTaxlot
	 *            What if Order
	 */
	public void handleBasket(List<HashMap<String, Object>> basketTaxlots) {
		try {
			_isRealTimePositions = Boolean.parseBoolean(CEPManager.getVariableValue("IsRealTimePositions").toString());
			DataInitializationRequestProcessor.getInstance();
			boolean isBasketStartedCompletely = 
					DataInitializationRequestProcessor._isBasketServiceStarted 
					 && DataInitializationRequestProcessor.getInstance()._isRuleMediatorInitialized;
			/**
			 * The logic : get the tax lot check if EOM if not then store else send to ESPER
			 */
			if (basketTaxlots != null && basketTaxlots.size() > 0 && !basketTaxlots.get(0).containsKey("IsEom")) {
				int userId = Integer.parseInt(basketTaxlots.get(0).get("CompanyUserID").toString());
				PranaLogManager.logOnly("Basket Orders received from Trade Server for UserID: " + userId);
				PendingWhatIfCache.getInstance().startTimer();
			
				if(!isBasketStartedCompletely) {
					String basketId = basketTaxlots.get(0).get("GroupID").toString();
					WhatIfHelper.sendBasketComplianceNotStartedAlert(_amqpCancelMessageSender, basketId, userId);
					// Send an EOM to server after basket compliance disconnected alert
					WhatIfHelper.sendEomMessage(_amqpCancelMessageSender, basketId, userId);
					PendingWhatIfCache.getInstance().removeFromCache(basketId);
					return;
				}
				
				if (!_isRequestSent) {
					PranaLogManager.info("Sending Post data request to Esper Engine for WhatIf at: "
							+ _parserSdf.format(new Date()) + " for UserID: " + userId);
					DataInitializationRequestProcessor.getInstance().sendRequestWithMessage(Integer.toString(userId), "EsperPostData");
					_isRequestSent = true;
				}
				 
				
				int sendTime = 0;
				while (!(Boolean) CEPManager.getVariableValue(ConfigurationConstants.DATA_LOADED_FOR_STAGE)) {
					sendTime = sendTime + _sleep;
					Thread.sleep(_sleep);
					CEPManager.notifyIfTimerExceedsLimit(sendTime);
				} 
				
				if (sendTime > 0)
					PranaLogManager.info("Basket Taxlot sending was delayed for " + sendTime + " milliseconds");
				
				long symbolDataStartTime = System.currentTimeMillis();
				for (HashMap<String, Object> basketTaxlot : basketTaxlots) {
					String symbol = basketTaxlot.get(CollectorConstants.SYMBOL).toString();
					PendingWhatIfCache.getInstance().addWhatIfOrder(basketTaxlot);
					int accountId = Integer.parseInt(basketTaxlot.get("Level1ID").toString());
					if (!_accountIdList.contains(accountId)) {
						_accountIdList.add(accountId);
					}
					if (!_symbolList.contains(symbol)) {
						_symbolList.add(symbol);
						sendBasketSymbolData(basketTaxlot, symbol);
					}
				}
	
				long estimatedTime = System.currentTimeMillis() - symbolDataStartTime;
				PranaLogManager.logOnly("Symbol Data delay for  "+ _symbolList.size() + " symbols is : " + ((double)(estimatedTime / 1000)) + " seconds");
			} else {
				if(!isBasketStartedCompletely)
					return;
				
				PranaLogManager.logOnly("IsRealTimePositions : "+ _isRealTimePositions);
				if (!_isRealTimePositions) {
					TaxlotCache.getInstance().removeRealTimePositions();
				}

				// Sending Snapshot taxlots to Basket
				HashMap<String, HashMap<String, Object>> taxlotList = TaxlotCache.getInstance()
						.getTaxlots(_accountIdList, _symbolList);
				sendSnapshotTaxlotsData(taxlotList, TaxlotType.WhatIf.toString());

				String basketId = basketTaxlots.get(0).get("BasketId").toString();
				int count = Integer.parseInt(basketTaxlots.get(0).get("Count").toString());
				PranaLogManager.info("EOM received for pre trade. BasketId: " + basketId + " [" + count + "] @ " + _parserSdf.format(new Date()));
				
				if (PendingWhatIfCache.getInstance().eomReceived(basketId, count)) {
					int userId = 0;
					HashMap<Integer, List<HashMap<String, Object>>> sortedTaxlots = PendingWhatIfCache.getInstance()
							.getSortedTaxlotsinBasket(basketId);

					for (int key : sortedTaxlots.keySet()) {
						for (HashMap<String, Object> taxlot : sortedTaxlots.get(key)) {
							String symbol = taxlot.get(CollectorConstants.SYMBOL).toString();
							// Check Security exists or not in SecurityWindow
							boolean isSecurityDetailsAvailable = WhatIfHelper
									.getSecurityInformationForSymbol(symbol) != null;
							if (!isSecurityDetailsAvailable) {
								throw new Exception();
							}
							sendBasketTaxlot(taxlot);
							int tempUser = Integer.parseInt(taxlot.get("CompanyUserID").toString());
							if (userId == 0)
								userId = tempUser;
							else if (userId != tempUser)
								userId = -1;
						}
					}
					CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] { 0 },
							"StartNavCalculationsForBasket");
					// Send EOM to basket compliance
					sendEomToBasketCompliance(basketId, userId);
				} else {
					WhatIfHelper.sendComplianceFailAlert(_amqpCancelMessageSender, basketId,
							PendingWhatIfCache.getInstance().getUserId(basketId));
					WhatIfHelper.sendEomMessage(_amqpCancelMessageSender, basketId,
							PendingWhatIfCache.getInstance().getUserId(basketId));
					PendingWhatIfCache.getInstance().removeFromCache(basketId);
				}
				_isRequestSent = false;
				CEPManager.setVariableWithDebugInfo(ConfigurationConstants.DATA_LOADED_FOR_STAGE, false);
				_accountIdList.clear();
				_symbolList.clear();
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		} finally {
			CEPManager.setVariableWithDebugInfo(ConfigurationConstants.DATA_LOADED_FOR_STAGE, false);
		}
	}

	/**
	 * Sending EOM to Basket compliance
	 * 
	 */
	private void sendEomToBasketCompliance(String basketId, int userId) {
		PranaLogManager.info("Starting aggregation for pre trade. BasketId: " + basketId + ", UserId: " + userId + " @"
				+ _parserSdf.format(new Date()));
		CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] { basketId, userId },
				"BasketAggreation");
		PranaLogManager.info("Starting sending data for pre trade. BasketId: " + basketId + ", UserId: " + userId + " @"
				+ _parserSdf.format(new Date()));
		CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] { basketId, userId },
				"BasketEOM");
		long estimatedTime = System.currentTimeMillis() - PendingWhatIfCache.getInstance().getStartTime();
		PendingWhatIfCache.getInstance().removeFromCache(basketId);
		if (!PendingWhatIfCache.getInstance().checkIfExistsInCache(basketId)) {
			PranaLogManager.info(
					"Pre trade processed. BasketId: " + basketId + ", UserId: " + userId + " ,Time taken in sec is: "
							+ ((double) estimatedTime / 1000) + " " + " @" + _parserSdf.format(new Date()));
		} else {
			PranaLogManager.info("Pre trade not processed correctly. BasketId: " + basketId + ", UserId: " + userId
					+ " ,Time taken in sec is: " + ((double) estimatedTime / 1000) + " " + " @"
					+ _parserSdf.format(new Date()));
		}
	}

	/**
	 * Extracting prices from Tax lot and sending them to Basket compliance
	 * 
	 * @param symbolData
	 */
	public void sendBasketSymbolData(HashMap<String, Object> taxlot, String symbol) {
		try {
			if (taxlot != null) {
				SymbolData symbolData = SymbolDataCache.getInstance().getSymbolDataForSymbol(symbol);
				double selectedFeedPrice = Double.parseDouble(taxlot.get(CollectorConstants.AVG_PRICE).toString());
				SendSymbolData(taxlot, symbol, symbolData, selectedFeedPrice);
				// PranaLogManager.logOnly("Basket SymbolData is Sending with avg/limit/stop price for symbol " + symbol+ ", Selected feed price: " + selectedFeedPrice);
				// Sending SymbolData for Underlying Symbol
				if (symbolData != null) {
					String underlyingSymbol = symbolData.underlyingSymbol;
					if (!underlyingSymbol.equals(symbol)) {
						symbolData = SymbolDataCache.getInstance().getSymbolDataForSymbol(underlyingSymbol);
						if (symbolData != null) {
							selectedFeedPrice = symbolData.selectedfeedPX;
						}
						/*PranaLogManager
						.logOnly("Basket SymbolData is Sending with avg/limit/stop price for Underlying Symbol "
								+ underlyingSymbol + ", Selected feed price: "+ selectedFeedPrice);*/
						SendSymbolData(taxlot, underlyingSymbol, symbolData, selectedFeedPrice);
					}
				}

				// Sending SymbolData for FX Symbol
				HashMap<String, Object> securityInfo = WhatIfHelper.getSecurityInformationForSymbol(symbol);
				if (securityInfo != null) {
					String fxSymbol = securityInfo.get("fxSymbol").toString();
					if (!fxSymbol.equals("USD-USD")) {
						symbolData = SymbolDataCache.getInstance().getSymbolDataForSymbol(fxSymbol);
						if (symbolData != null) {
							if (!_isRealTimePositions)
								selectedFeedPrice = Double.parseDouble(symbolData.markPrice.toString());
							else
								selectedFeedPrice = symbolData.selectedfeedPX != 0 ? symbolData.selectedfeedPX
										: Double.parseDouble(taxlot.get("FXRate").toString());
						}
						//PranaLogManager.logOnly("Basket SymbolData is Sending with avg/limit/stop price for Fx Symbol " + fxSymbol + ", Selected feed price: " + selectedFeedPrice);
						SendSymbolData(taxlot, fxSymbol, symbolData, selectedFeedPrice);
					}
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Sending SymbolData
	 * 
	 * @param taxlot
	 * @param symbol
	 * @param symbolData
	 * @param selectedFeedPrice
	 */
	private void SendSymbolData(HashMap<String, Object> taxlot, String symbol, SymbolData symbolData,
			double selectedFeedPrice) {
		try {
			if (symbolData != null) {
				double lastPrice = TaxlotManager.getDoubleSafe(symbolData.lastPrice, "lastPrice");
				CEPManager.getEPRuntime().getEventService()
						.sendEventObjectArray(new Object[] { symbol, symbolData.underlyingSymbol,
								symbolData.ask, symbolData.bid, symbolData.low, symbolData.high, symbolData.open,
								symbolData.closingPrice, lastPrice, selectedFeedPrice,
								symbolData.conversionMethod, symbolData.markPrice,
								symbolData.delta, symbolData.beta5yr, symbolData.categoryCode, symbolData.openInterest,
								symbolData.avgVol20Days, symbolData.sharesOutStandings },
								CollectorConstants.SYMBOL_DATA_EVENT_NAME);
			} else {
				String underlyingSymbol = taxlot.get(CollectorConstants.UNDERLYING_SYMBOL) != null
						? taxlot.get(CollectorConstants.UNDERLYING_SYMBOL).toString()
						: symbol;
				CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] { symbol,
						underlyingSymbol, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, selectedFeedPrice,
						taxlot.get(CollectorConstants.FX_CONVERSION_METHOD).toString(),
						Double.parseDouble(taxlot.get(CollectorConstants.MARK_PRICE).toString()), 0.0, 0.0,
						Integer.parseInt(taxlot.get(CollectorConstants.ASSET_ID).toString()), 0.0, 0.0, BigDecimal.valueOf(0.0) },
						CollectorConstants.SYMBOL_DATA_EVENT_NAME);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public void sendSnapshotTaxlotsData(HashMap<String, HashMap<String, Object>> taxlotList, String requestType) {
		try {
			PranaLogManager.logOnly(
					"Snapshot taxlots are sending with count: " + taxlotList.size() + " requested for " + requestType);
			for (HashMap<String, Object> taxlot : taxlotList.values()) {
				String symbol = taxlot.get("symbol").toString();
				if (!_isRealTimePositions) {
					if (!_symbolList.contains(symbol)) {
						_symbolList.add(symbol);
						SymbolData symbolData = SymbolDataCache.getInstance().getSymbolDataForSymbol(symbol);
						if (symbolData != null) {
							double selectedFeedPrice = (double) symbolData.markPrice;
							double lastPrice = TaxlotManager.getDoubleSafe(symbolData, "lastPrice");
							CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] { symbol,
									symbolData.underlyingSymbol, symbolData.ask, symbolData.bid, symbolData.low,
									symbolData.high, symbolData.open, symbolData.closingPrice, lastPrice,
									selectedFeedPrice, symbolData.conversionMethod, symbolData.markPrice,
									symbolData.delta, symbolData.beta5yr, symbolData.categoryCode,
									symbolData.openInterest, symbolData.avgVol20Days, symbolData.sharesOutStandings },
									CollectorConstants.SYMBOL_DATA_EVENT_NAME);
						}
					}
				}

				Taxlot snapshotTaxlot = new Taxlot();
				snapshotTaxlot.basketId = TaxlotManager.getStringSafe(taxlot.get("basketId"), "basketId");
				snapshotTaxlot.taxlotId = TaxlotManager.getStringSafe(taxlot.get("taxlotId"), "taxlotId");
				snapshotTaxlot.clOrderId = TaxlotManager.getStringSafe(taxlot.get("clOrderId"), "clOrderId");
				snapshotTaxlot.taxlotType = TaxlotType.valueOf(TaxlotManager.getStringSafe(taxlot.get("taxlotType"), "taxlotType"));
				snapshotTaxlot.taxlotState = TaxlotState.valueOf(TaxlotManager.getStringSafe(taxlot.get("taxlotState"), "taxlotState"));
				snapshotTaxlot.symbol = symbol;
				snapshotTaxlot.underlyingSymbol = TaxlotManager.getStringSafe(taxlot.get("underlyingSymbol"),
						"underlyingSymbol");
				snapshotTaxlot.quantity = TaxlotManager.getDoubleSafe(taxlot.get("quantity"), "quantity");
				snapshotTaxlot.avgPrice = TaxlotManager.getDoubleSafe(taxlot.get("avgPrice"), "avgPrice");
				snapshotTaxlot.orderSideTagValue = TaxlotManager.getStringSafe(taxlot.get("orderSideTagValue"),
						"orderSideTagValue");
				snapshotTaxlot.accountId = TaxlotManager.getIntSafe(taxlot.get("accountId"), "accountId");
				snapshotTaxlot.counterPartyId = TaxlotManager.getIntSafe(taxlot.get("counterPartyId"),
						"counterPartyId");
				snapshotTaxlot.venueId = TaxlotManager.getIntSafe(taxlot.get("venueId"), "venueId");
				snapshotTaxlot.auecLocalDate = TaxlotManager.getDateSafe(taxlot.get("auecLocalDate"), "auecLocalDate",
						_parserSdf);
				snapshotTaxlot.settlementDate = TaxlotManager.getDateSafe(taxlot.get("settlementDate"),
						"settlementDate", _parserSdf);
				snapshotTaxlot.userId = TaxlotManager.getStringSafe(taxlot.get("userId"), "userId");
				snapshotTaxlot.strategyId = TaxlotManager.getIntSafe(taxlot.get("strategyId"), "strategyId");
				snapshotTaxlot.orderTypeTagValue = TaxlotManager.getStringSafe(taxlot.get("orderTypeTagValue"),
						"orderTypeTagValue");
				snapshotTaxlot.benchMarkRate = TaxlotManager.getDoubleSafe(taxlot.get("benchMarkRate"),
						"benchMarkRate");
				snapshotTaxlot.differential = TaxlotManager.getDoubleSafe(taxlot.get("differential"), "differential");
				snapshotTaxlot.swapNotional = TaxlotManager.getDoubleSafe(taxlot.get("swapNotional"), "swapNotional");
				snapshotTaxlot.dayCount = TaxlotManager.getDoubleSafe(taxlot.get("dayCount"), "dayCount");
				snapshotTaxlot.isSwapped = TaxlotManager.getBooleanSafe(taxlot.get("isSwapped"), "isSwapped");
				snapshotTaxlot.avgFxRateForTrade = TaxlotManager.getDoubleSafe(taxlot.get("avgFxRateForTrade"),
						"avgFxRateForTrade");
				snapshotTaxlot.conversionMethodOperator = TaxlotManager
						.getStringSafe(taxlot.get("conversionMethodOperator"), "conversionMethodOperator");
				snapshotTaxlot.tif = TaxlotManager.getStringSafe(taxlot.get("tif"), "tif");
				snapshotTaxlot.orderSide = TaxlotManager.getStringSafe(taxlot.get("orderSide"), "orderSide");
				snapshotTaxlot.counterParty = TaxlotManager.getStringSafe(taxlot.get("counterParty"), "counterParty");
				snapshotTaxlot.venue = TaxlotManager.getStringSafe(taxlot.get("venue"), "venue");
				snapshotTaxlot.sideMultiplier = TaxlotManager.getIntSafe(taxlot.get("sideMultiplier"),
						"sideMultiplier");
				snapshotTaxlot.orderType = TaxlotManager.getStringSafe(taxlot.get("orderType"), "orderType");
				snapshotTaxlot.underlyingAsset = TaxlotManager.getStringSafe(taxlot.get("underlyingAsset"),
						"underlyingAsset");
				snapshotTaxlot.limitPrice = TaxlotManager.getDoubleSafe(taxlot.get("limitPrice"), "limitPrice");
				snapshotTaxlot.stopPrice = TaxlotManager.getDoubleSafe(taxlot.get("stopPrice"), "stopPrice");
				snapshotTaxlot.isWhatIfTradeStreamRequired = TaxlotManager
						.getBooleanSafe(taxlot.get("isWhatIfTradeStreamRequired"), "isWhatIfTradeStreamRequired");
				snapshotTaxlot.assetId = TaxlotManager.getIntSafe(taxlot.get("assetId"), "assetId");
				snapshotTaxlot.asset = TaxlotManager.getStringSafe(taxlot.get("asset"), "asset");
				snapshotTaxlot.auecId = TaxlotManager.getIntSafe(taxlot.get("auecId"), "auecId");
				snapshotTaxlot.multiplier = TaxlotManager.getDoubleSafe(taxlot.get("multiplier"), "multiplier");
				snapshotTaxlot.tradeAttribute1 = TaxlotManager.getStringSafe(taxlot.get("tradeAttribute1"),
						"tradeAttribute1");
				snapshotTaxlot.tradeAttribute2 = TaxlotManager.getStringSafe(taxlot.get("tradeAttribute2"),
						"tradeAttribute2");
				snapshotTaxlot.tradeAttribute3 = TaxlotManager.getStringSafe(taxlot.get("tradeAttribute3"),
						"tradeAttribute3");
				snapshotTaxlot.tradeAttribute4 = TaxlotManager.getStringSafe(taxlot.get("tradeAttribute4"),
						"tradeAttribute4");
				snapshotTaxlot.tradeAttribute5 = TaxlotManager.getStringSafe(taxlot.get("tradeAttribute5"),
						"tradeAttribute5");
				snapshotTaxlot.tradeAttribute6 = TaxlotManager.getStringSafe(taxlot.get("tradeAttribute6"),
						"tradeAttribute6");

				// Send to CEP
				TaxlotManager.sendTaxlotToCEPEngine(snapshotTaxlot, false);
			}
			PranaLogManager.logOnly("Completed snapshot taxlots sending with count: " + taxlotList.size()
					+ " for request " + requestType);

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	// Sends what if tax lot to ESPER Engine.
	private void sendBasketTaxlot(HashMap<String, Object> basketTaxlot) {
		try {
			Date settlementDate = basketTaxlot.get("SettlementDate") != null
					? TaxlotManager.getDateSafe(basketTaxlot.get("SettlementDate"), "SettlementDate", _parserSdf)
					: new Date();

			@SuppressWarnings("unchecked")
			HashMap<String, Object> swapParameters = (HashMap<String, Object>) basketTaxlot.get("SwapParameters");

			boolean isSwap = TaxlotManager.checkAndLogSwap(basketTaxlot.get("IsSwapped"), swapParameters);
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

			Taxlot whatIfTaxlot = new Taxlot();
			whatIfTaxlot.basketId = TaxlotManager.getStringSafe(basketTaxlot.get("GroupID"), "GroupID");
			whatIfTaxlot.taxlotId = TaxlotManager.getStringSafe(basketTaxlot.get("TaxLotID"), "TaxLotID");
			whatIfTaxlot.clOrderId = TaxlotManager.getStringSafe(basketTaxlot.get("LotId"), "LotId");
			whatIfTaxlot.taxlotType = TaxlotType.WhatIf;
			whatIfTaxlot.taxlotState = TaxlotState.New;
			whatIfTaxlot.symbol = TaxlotManager.getStringSafe(basketTaxlot.get("Symbol"), "Symbol");
			whatIfTaxlot.underlyingSymbol = TaxlotManager.getStringSafe(basketTaxlot.get("UnderlyingSymbol"),
					"UnderlyingSymbol");
			whatIfTaxlot.quantity = TaxlotManager.getDoubleSafe(basketTaxlot.get("TaxLotQty"), "TaxLotQty");
			whatIfTaxlot.avgPrice = TaxlotManager.getDoubleSafe(basketTaxlot.get("AvgPrice"), "AvgPrice");
			whatIfTaxlot.orderSideTagValue = TaxlotManager.getStringSafe(basketTaxlot.get("OrderSideTagValue"),
					"OrderSideTagValue");
			whatIfTaxlot.accountId = TaxlotManager.getIntSafe(basketTaxlot.get("Level1ID"), "Level1ID");
			whatIfTaxlot.counterPartyId = TaxlotManager.getIntSafe(basketTaxlot.get("CounterPartyID"),
					"CounterPartyID");
			whatIfTaxlot.venueId = TaxlotManager.getIntSafe(basketTaxlot.get("VenueID"), "VenueID");
			whatIfTaxlot.auecLocalDate = TaxlotManager.getDateSafe(basketTaxlot.get("AUECLocalDate"), "AUECLocalDate",
					_parserSdf);
			whatIfTaxlot.settlementDate = settlementDate;
			whatIfTaxlot.userId = TaxlotManager.getStringSafe(basketTaxlot.get("CompanyUserID"), "CompanyUserID");
			whatIfTaxlot.strategyId = TaxlotManager.getIntSafe(basketTaxlot.get("Level2ID"), "Level2ID");
			whatIfTaxlot.orderTypeTagValue = TaxlotManager.getStringSafe(basketTaxlot.get("OrderTypeTagValue"),
					"OrderTypeTagValue");
			whatIfTaxlot.benchMarkRate = benchMarkRate;
			whatIfTaxlot.differential = differential;
			whatIfTaxlot.swapNotional = swapNotional;
			whatIfTaxlot.dayCount = dayCount;
			whatIfTaxlot.isSwapped = isSwap;
			whatIfTaxlot.avgFxRateForTrade = TaxlotManager.getDoubleSafe(basketTaxlot.get("FXRate"), "FXRate");
			whatIfTaxlot.conversionMethodOperator = "M";
			whatIfTaxlot.tif = TaxlotManager.getStringSafe(basketTaxlot.get("TIF"), "TIF");
			whatIfTaxlot.orderSide = TaxlotManager.getStringSafe(basketTaxlot.get("OrderSide"), "OrderSide");
			whatIfTaxlot.counterParty = TaxlotManager.getStringSafe(basketTaxlot.get("CounterPartyName"),
					"CounterPartyName");
			whatIfTaxlot.venue = TaxlotManager.getStringSafe(basketTaxlot.get("Venue"), "Venue");
			whatIfTaxlot.sideMultiplier = TaxlotManager.getIntSafe(basketTaxlot.get("SideMultiplier"),
					"SideMultiplier");
			whatIfTaxlot.orderType = TaxlotManager.getStringSafe(basketTaxlot.get("OrderType"), "OrderType");
			whatIfTaxlot.underlyingAsset = TaxlotManager.getStringSafe(basketTaxlot.get("UnderlyingName"),
					"UnderlyingName");
			whatIfTaxlot.limitPrice = TaxlotManager.getDoubleSafe(basketTaxlot.get("LimitPrice"), "LimitPrice");
			whatIfTaxlot.stopPrice = TaxlotManager.getDoubleSafe(basketTaxlot.get("StopPrice"), "StopPrice");
			whatIfTaxlot.isWhatIfTradeStreamRequired = true;
			whatIfTaxlot.assetId = TaxlotManager.getIntSafe(basketTaxlot.get("AssetID"), "AssetID");
			whatIfTaxlot.asset = TaxlotManager.getStringSafe(basketTaxlot.get("AssetName"), "AssetName");
			whatIfTaxlot.auecId = TaxlotManager.getIntSafe(basketTaxlot.get("AUECID"), "AUECID");
			whatIfTaxlot.multiplier = TaxlotManager.getDoubleSafe(basketTaxlot.get("ContractMultiplier"),
					"ContractMultiplier");
			whatIfTaxlot.tradeAttribute1 = TaxlotManager.getStringSafe(basketTaxlot.get("TradeAttribute1"),
					"TradeAttribute1");
			whatIfTaxlot.tradeAttribute2 = TaxlotManager.getStringSafe(basketTaxlot.get("TradeAttribute2"),
					"TradeAttribute2");
			whatIfTaxlot.tradeAttribute3 = TaxlotManager.getStringSafe(basketTaxlot.get("TradeAttribute3"),
					"TradeAttribute3");
			whatIfTaxlot.tradeAttribute4 = TaxlotManager.getStringSafe(basketTaxlot.get("TradeAttribute4"),
					"TradeAttribute4");
			whatIfTaxlot.tradeAttribute5 = TaxlotManager.getStringSafe(basketTaxlot.get("TradeAttribute5"),
					"TradeAttribute5");
			whatIfTaxlot.tradeAttribute6 = TaxlotManager.getStringSafe(basketTaxlot.get("TradeAttribute6"),
					"TradeAttribute6");

			// Send to CEP
			TaxlotManager.sendTaxlotToCEPEngine(whatIfTaxlot, false);

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}