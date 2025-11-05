/**
uju * Holds all amqp data collectors
 */
package prana.basketComplianceService.amqpCollectors;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.Set;
import java.lang.reflect.Field;
import java.math.BigDecimal;
import prana.businessObjects.interfaces.IAmqpListenerCallback;
import prana.businessObjects.uda.DynamicUDA;
import prana.basketComplianceService.communication.DataInitializationRequestProcessor;
import prana.basketComplianceService.main.SymbolDataCache;
import prana.basketComplianceService.main.TaxlotCache;
import prana.esperCalculator.commonCode.CEPManager;
import prana.esperCalculator.commonCode.TaxlotManager;
import prana.esperCalculator.commonCode.TaxlotManager.TaxlotState;
import prana.esperCalculator.commonCode.TaxlotManager.TaxlotType;
import prana.esperCalculator.constants.CollectorConstants;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.objects.Taxlot;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

/**
 * Callback listener file which perform actions after data is received from amqp
 * 
 * @author Chaturvedi Dewashish <dewashish@nirvana-sol.com>
 */
public class AmqpListenerCallbackBasketCompliance implements IAmqpListenerCallback {
	SimpleDateFormat parserSDF;

	/**
	 * @author Chaturvedi Dewashish <dewashish@nirvana-sol.com>
	 * @description Returns instance of listener
	 */
	public AmqpListenerCallbackBasketCompliance() {
		parserSDF = new SimpleDateFormat(ConfigurationConstants.SIMPLE_DATE_FORMAT);
	}

	/*
	 * @see prana.amqpAdapter.IAmqpListenerCallback#AmqpRecieverStarted()
	 */
	@Override
	public void amqpRecieverStarted() {
		PranaLogManager.info("AmqpReceiver for Basket Compliance has STARTED\n");
	}

	/*
	 * @see prana.amqpAdapter.IAmqpListenerCallback#AmqpRecieverStopped()
	 */
	@Override
	public void amqpRecieverStopped(String message, Exception cause) {
		PranaLogManager.info("AmqpReceiver for Basket Compliance has STOPPED");
		PranaLogManager.error(cause, message);
	}

	/**
	 * Counter for Esper taxlot received
	 */
	private long _historicalCounter = 0;
	SimpleDateFormat _parserSdf = new SimpleDateFormat(ConfigurationConstants.SIMPLE_DATE_FORMAT);

	/*
	 * @see
	 * prana.amqpAdapter.IAmqpListenerCallback#AmqpDataReceived(java.lang.String ,
	 * java.lang.String)
	 */
	@Override
	public void amqpDataReceived(String jsonReceivedData, String routingKey) {
		boolean isSpecificRoutingKey = routingKey.equals(ConfigurationConstants.ROUTING_KEY_RULE_COMPRESSION_INFO) || routingKey.equals("DataSentFromEsper");

		LinkedHashMap<String, Object> map = isSpecificRoutingKey ? null : JSONMapper.getLinkedHashMap(jsonReceivedData);

		@SuppressWarnings("unchecked")
		HashMap<String, Object> hashMap = isSpecificRoutingKey ? null : (HashMap<String, Object>) map.get("HashMap");

		try {
			switch (routingKey) {
				case "SendRuleNameWithCompression":
					CEPManager.GetRuleNameWithCompression(jsonReceivedData);
					CEPManager.createEOMBasedOnEnabledRules();
					break;
				case "InitCompleteInfo":
					DataInitializationRequestProcessor.getInstance().initializationInfoReceived(map);
					break;
				case "NavAndStartingPositionOfAccountsRequest":
					DataInitializationRequestProcessor.getInstance().initializationInfoReceived(map);
					break;
				/*
				 * case "AccountDivisorWindowData": case "MasterFundDivisorWindowData": case
				 * "GlobalDivisorWindowData":
				 * CEPManager.getEPRuntime().getEventService().sendEventMap(hashMap,
				 * routingKey); break;
				 */
				case "DataSentFromEsper":
				if (jsonReceivedData.contains(ConfigurationConstants.PST))
					CEPManager.setVariableWithDebugInfo(ConfigurationConstants.DATA_LOADED_FOR_PST, true);
				else
					CEPManager.setVariableWithDebugInfo(ConfigurationConstants.DATA_LOADED_FOR_STAGE, true);
				PranaLogManager.info("Complete Post data received from esper at : " + _parserSdf.format(new Date()) + " for " + jsonReceivedData);
				DataInitializationRequestProcessor.lastMarketSnapshotUpdateTime = System.currentTimeMillis(); 
				break;
			case "Security":
				securityReceived(hashMap);
				break;
				case "AuecDetails":
					auecDetailsReceived(hashMap);
					break;
				case "StrategyCollection":
					strategyCollectionReceived(hashMap);
					break;
				case "AccountCollection":
					accountCollectionReceived(hashMap);
					break;
				case "PmCalculationPreferenceWindowData":
					pmCalculationPreference(hashMap);
					break;
				case "SymbolDataWindowDataInit":
					SymbolDataReceived(hashMap);
					break;
				case "SymbolDataWindowData":
					if (DataInitializationRequestProcessor._isBasketServiceStarted) {
						int sendTime = 0;
						while ((Boolean) CEPManager.getVariableValue(ConfigurationConstants.DATA_LOADED_FOR_STAGE)
								|| (Boolean) CEPManager.getVariableValue(ConfigurationConstants.DATA_LOADED_FOR_PST)) {
							sendTime = sendTime + 100;
							Thread.sleep(100);
						CEPManager.notifyIfTimerExceedsLimit(sendTime);
					}
					if (sendTime > 0)
						PranaLogManager.info("Basket SymbolData sending was delayed for " + sendTime + " milliseconds");
					SymbolDataReceived(hashMap);
				}
				break;
				case "TaxlotWindowData":
					TaxlotBaseDataReceived(hashMap);
					break;
				case "DeleteTaxlotData":
					TaxlotDeleteDataReceived(map, hashMap);
					break;
				case "YearlyHolidaysEvent":
					CEPManager.getEPRuntime().getEventService()
							.sendEventObjectArray(new Object[] { Integer.parseInt(hashMap.get("auecId").toString()),
									parserSDF.parse(hashMap.get("holiday").toString()) }, "YearlyHolidaysEvent");
					break;
				case "WeeklyHolidaysEvent":
					CEPManager.getEPRuntime().getEventService()
							.sendEventObjectArray(new Object[] { Integer.parseInt(hashMap.get("auecId").toString()),
									Integer.parseInt(hashMap.get("holiday").toString()) }, "WeeklyHolidaysEvent");
				break;
			case "Permissions":
				int postWithInMarketInStage = Integer.parseInt(map.get("PostWithInMarketInStageValue").toString());
				CEPManager.setVariableValue(ConfigurationConstants.KEY_POST_WITH_INMARKET_INSTAGE, postWithInMarketInStage);

				boolean isM2MIncludedInCash = Boolean.parseBoolean(map.get("IsM2MIncludedInCash").toString());
					CEPManager.setVariableValue("IsM2MIncludedInCash", isM2MIncludedInCash);

					String companyBaseCurrency = map.get("CompanyBaseCurrency").toString();
					CEPManager.setVariableValue("CompanyBaseCurrency", companyBaseCurrency);

					boolean isCreditLimitBoxPositionAllowed = Boolean
							.parseBoolean(map.get("IsCreditLimitBoxPositionAllowed").toString());
				CEPManager.setVariableValue("IsCreditLimitBoxPositionAllowed", isCreditLimitBoxPositionAllowed);

				boolean EquitySwapsMarketValueAsEquity = Boolean
						.parseBoolean(map.get("EquitySwapsMarketValueAsEquity").toString());
				CEPManager.setVariableValue("EquitySwapsMarketValueAsEquity", EquitySwapsMarketValueAsEquity);

				boolean calculateFxGainLossOnForexForwards = Boolean
						.parseBoolean(map.get("CalculateFxGainLossOnForexForwards").toString());
				CEPManager.setVariableValue("CalculateFxGainLossOnForexForwards", calculateFxGainLossOnForexForwards);

				boolean calculateFxGainLossOnSwaps = Boolean
							.parseBoolean(map.get("CalculateFxGainLossOnSwaps").toString());
					CEPManager.setVariableValue("CalculateFxGainLossOnSwaps", calculateFxGainLossOnSwaps);

					boolean setFxToZero = Boolean.parseBoolean(map.get("SetFxToZero").toString());
					CEPManager.setVariableValue("SetFxToZero", setFxToZero);

					PranaLogManager.info("-------------------------------------");
					PranaLogManager.info("Preferences received as:");
					PranaLogManager.info("IsM2MIncludedInCash: " + isM2MIncludedInCash);
					PranaLogManager.info("CompanyBaseCurrency: " + companyBaseCurrency);
					PranaLogManager.info("IsCreditLimitBoxPositionAllowed: " + isCreditLimitBoxPositionAllowed);
				PranaLogManager.info("EquitySwapsMarketValueAsEquity: " + EquitySwapsMarketValueAsEquity);
				PranaLogManager.info("CalculateFxGainLossOnForexForwards: " + calculateFxGainLossOnForexForwards);
				PranaLogManager.info("CalculateFxGainLossOnSwaps: " + calculateFxGainLossOnSwaps);
				PranaLogManager.info("SetFxToZero: " + setFxToZero);
				PranaLogManager.info(ConfigurationConstants.KEY_POST_WITH_INMARKET_INSTAGE + ": " + postWithInMarketInStage);

				PranaLogManager.info("-------------------------------------");
				break;
			case "DayEndCashAccount":
				boolean _isRealTimePositions = Boolean.parseBoolean(CEPManager.getVariableValue("IsRealTimePositions").toString());
				Double nonTradingCashAmount = _isRealTimePositions ? Double.valueOf(hashMap.get("nonTradingCashAmount").toString()) : 0.0;
				Date today = parserSDF.parse(hashMap.get("today").toString());
					CEPManager.getEPRuntime().getEventService()
							.sendEventObjectArray(
									new Object[] { Integer.parseInt(hashMap.get("accountId").toString()),
											Double.valueOf(hashMap.get("cashAmount").toString()), today,
											hashMap.get("accountShortName").toString(),
											Integer.parseInt(hashMap.get("masterFundId").toString()),
											hashMap.get("masterFundName").toString(),
											nonTradingCashAmount },
									"DayEndCashEvent");
					break;
				case "AccountNavPreference":
					CEPManager.getEPRuntime().getEventService()
							.sendEventObjectArray(
									new Object[] { Integer.parseInt(hashMap.get("accountId").toString()),
											Boolean.parseBoolean(hashMap.get("isNavSaved").toString()) },
									"AccountNavPreference");
					break;
				case "AccrualForAccount":
					CEPManager.getEPRuntime().getEventService()
							.sendEventObjectArray(new Object[] { Integer.parseInt(hashMap.get("accountId").toString()),
									Double.valueOf(hashMap.get("startOfDayAccrual").toString()),
									Double.valueOf(hashMap.get("dayAccrual").toString()) }, "AccrualForAccount");
					break;
				case "DbNav":
					CEPManager.getEPRuntime().getEventService()
							.sendEventObjectArray(new Object[] { Integer.parseInt(hashMap.get("accountId").toString()),
									Double.valueOf(hashMap.get("startOfDayNav").toString()),
									Double.valueOf(hashMap.get("shadowNav").toString()) }, "DbNav");
					break;
			}
		} catch (Exception ex) {
			StringBuilder sb = new StringBuilder();
			sb.append(ex.getMessage());
			sb.append("\n----------------------");
			sb.append("\nError occured in CallbackBasketCompliance");
			sb.append("\nRouting key : " + routingKey);
			sb.append("\nJson : " + jsonReceivedData);
			sb.append("\n----------------------");
			PranaLogManager.error(ex, sb.toString());
		}
	}

	/**
	 * TaxlotDeleteDataReceived for Post/InTrade taxlot
	 * 
	 */
	@SuppressWarnings("unchecked")
	private void TaxlotDeleteDataReceived(LinkedHashMap<String, Object> map, HashMap<String, Object> taxlot) {
		try {
			if (taxlot != null) {
				sendInTadeTaxlotDeletion(taxlot);
			} else {
				sendPostTaxlotDeletion((HashMap<String, Object>) map.get("LinkedHashMap"));
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * TaxlotDeleteDataReceived for Post taxlot
	 * 
	 */
	private void sendPostTaxlotDeletion(HashMap<String, Object> taxlot) {
		try {
			@SuppressWarnings("unchecked")
			HashMap<String, Object> swapParameters = (HashMap<String, Object>) taxlot.get("SwapParameters");

			boolean isSwap = TaxlotManager.checkAndLogSwap(taxlot.get("IsSwapped"), swapParameters);
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

			String conversionMethod = "M";
			if (taxlot.containsKey("FXConversionMethodOnTradeDate")) {
				int methodId = TaxlotManager.getIntSafe(taxlot.get("FXConversionMethodOnTradeDate"), "FXConversionMethodOnTradeDate");
				if (methodId == 1)
					conversionMethod = "D";
			}

			String taxlotId = TaxlotManager.getStringSafe(taxlot.get("ID"), "ID");
			TaxlotType taxlotType = TaxlotType.Post;
			
			Taxlot removePostTaxlot = new Taxlot();
			removePostTaxlot.basketId = taxlotId;
			removePostTaxlot.taxlotId = taxlotId;
			removePostTaxlot.clOrderId = taxlotId;
			removePostTaxlot.taxlotType = taxlotType;
			removePostTaxlot.taxlotState = TaxlotState.Deleted;
			removePostTaxlot.symbol = TaxlotManager.getStringSafe(taxlot.get("Symbol"), "Symbol");
			removePostTaxlot.underlyingSymbol = TaxlotManager.getStringSafe(taxlot.get("UnderlyingSymbol"), "UnderlyingSymbol");
			removePostTaxlot.quantity = TaxlotManager.getDoubleSafe(taxlot.get("Quantity"), "Quantity");
			removePostTaxlot.avgPrice = TaxlotManager.getDoubleSafe(taxlot.get("AvgPrice"), "AvgPrice");
			removePostTaxlot.orderSideTagValue = TaxlotManager.getStringSafe(taxlot.get("OrderSideTagValue"), "OrderSideTagValue");
			removePostTaxlot.accountId = TaxlotManager.getIntSafe(taxlot.get("Level1ID"), "Level1ID");
			removePostTaxlot.counterPartyId = TaxlotManager.getIntSafe(taxlot.get("CounterPartyId"), "CounterPartyId");
			removePostTaxlot.venueId = TaxlotManager.getIntSafe(taxlot.get("VenueId"), "VenueId");
			removePostTaxlot.auecLocalDate = TaxlotManager.getDateSafe(taxlot.get("AUECLocalDate"), "AUECLocalDate", parserSDF);
			removePostTaxlot.settlementDate = TaxlotManager.getDateSafe(taxlot.get("SettlementDate"), "SettlementDate", parserSDF);
			removePostTaxlot.userId = "NA";
			removePostTaxlot.strategyId = TaxlotManager.getIntSafe(taxlot.get("Level2ID"), "Level2ID");
			removePostTaxlot.orderTypeTagValue = TaxlotManager.getStringSafe(taxlot.get("OrderTypeTagValue"), "OrderTypeTagValue");
			removePostTaxlot.benchMarkRate = benchMarkRate;
			removePostTaxlot.differential = differential;
			removePostTaxlot.swapNotional = swapNotional;
			removePostTaxlot.dayCount = dayCount;
			removePostTaxlot.isSwapped = isSwap;
			removePostTaxlot.avgFxRateForTrade = TaxlotManager.getDoubleSafe(taxlot.get("FXRateOnTradeDate"), "FXRateOnTradeDate");
			removePostTaxlot.conversionMethodOperator = conversionMethod;
			removePostTaxlot.tif = TaxlotManager.getStringSafe(taxlot.get("TIF"), "TIF");
			removePostTaxlot.orderSide = TaxlotManager.getStringSafe(taxlot.get("OrderSide"), "OrderSide");
			removePostTaxlot.counterParty = TaxlotManager.getStringSafe(taxlot.get("CounterPartyName"), "CounterPartyName");
			removePostTaxlot.venue = TaxlotManager.getStringSafe(taxlot.get("Venue"), "Venue");
			removePostTaxlot.sideMultiplier = TaxlotManager.getIntSafe(taxlot.get("SideMultiplier"), "SideMultiplier");
			removePostTaxlot.orderType = TaxlotManager.getStringSafe(taxlot.get("OrderType"), "OrderType");
			removePostTaxlot.underlyingAsset = TaxlotManager.getStringSafe(taxlot.get("UnderlyingName"), "UnderlyingName");
			removePostTaxlot.limitPrice = TaxlotManager.getDoubleSafe(taxlot.get("LimitPrice"), "LimitPrice");
			removePostTaxlot.stopPrice = TaxlotManager.getDoubleSafe(taxlot.get("StopPrice"), "StopPrice");
			removePostTaxlot.isWhatIfTradeStreamRequired = true;
			removePostTaxlot.assetId = TaxlotManager.getIntSafe(taxlot.get("Asset"), "Asset");
			removePostTaxlot.asset = TaxlotManager.getStringSafe(taxlot.get("AssetName"), "AssetName");
			removePostTaxlot.auecId = TaxlotManager.getIntSafe(taxlot.get("AUECID"), "AUECID");
			removePostTaxlot.multiplier = TaxlotManager.getDoubleSafe(taxlot.get("Multiplier"), "Multiplier");
			removePostTaxlot.tradeAttribute1 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute1"), "TradeAttribute1");
			removePostTaxlot.tradeAttribute2 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute2"), "TradeAttribute2");
			removePostTaxlot.tradeAttribute3 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute3"), "TradeAttribute3");
			removePostTaxlot.tradeAttribute4 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute4"), "TradeAttribute4");
			removePostTaxlot.tradeAttribute5 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute5"), "TradeAttribute5");
			removePostTaxlot.tradeAttribute6 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute6"), "TradeAttribute6");

			// Remove taxlot from cache
			TaxlotCache.getInstance().removeFromCache(taxlotId + taxlotType);

			// Send to CEP
			TaxlotManager.sendTaxlotToCEPEngine(removePostTaxlot, true);

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * TaxlotDeleteDataReceived for InTrade taxlot
	 * 
	 */
	private void sendInTadeTaxlotDeletion(HashMap<String, Object> taxlot) {
		try {
			@SuppressWarnings("unchecked")
			HashMap<String, Object> swapParameters = (HashMap<String, Object>) taxlot.get("SwapParameters");

			boolean isSwap = TaxlotManager.checkAndLogSwap(taxlot.get("IsSwapped"), swapParameters);
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

			String taxlotId = TaxlotManager.getStringSafe(taxlot.get("TaxLotID"), "TaxLotID");
			String taxlotType = TaxlotManager.getStringSafe(taxlot.get("TaxlotType"), "TaxlotType");
			

	        Taxlot removeInTradeTaxlot = new Taxlot();
	        removeInTradeTaxlot.basketId = TaxlotManager.getStringSafe(taxlot.get("GroupID"), "GroupID");
	        removeInTradeTaxlot.taxlotId = taxlotId;
	        removeInTradeTaxlot.clOrderId = TaxlotManager.getStringSafe(taxlot.get("LotId"), "LotId");;
	        removeInTradeTaxlot.taxlotType = TaxlotType.valueOf(taxlotType);
	        removeInTradeTaxlot.taxlotState = TaxlotState.Deleted;
	        removeInTradeTaxlot.symbol = TaxlotManager.getStringSafe(taxlot.get("Symbol"), "Symbol");
	        removeInTradeTaxlot.underlyingSymbol = TaxlotManager.getStringSafe(taxlot.get("UnderlyingSymbol"), "UnderlyingSymbol");
	        removeInTradeTaxlot.quantity = TaxlotManager.getDoubleSafe(taxlot.get("TaxLotQty"), "TaxLotQty");
	        removeInTradeTaxlot.avgPrice = TaxlotManager.getDoubleSafe(taxlot.get("AvgPrice"), "AvgPrice");
	        removeInTradeTaxlot.orderSideTagValue = TaxlotManager.getStringSafe(taxlot.get("OrderSideTagValue"), "OrderSideTagValue");
	        removeInTradeTaxlot.accountId = TaxlotManager.getIntSafe(taxlot.get("Level1ID"), "Level1ID");
	        removeInTradeTaxlot.counterPartyId = TaxlotManager.getIntSafe(taxlot.get("CounterPartyID"), "CounterPartyID");
	        removeInTradeTaxlot.venueId = TaxlotManager.getIntSafe(taxlot.get("VenueID"), "VenueID");
	        removeInTradeTaxlot.auecLocalDate = TaxlotManager.getDateSafe(taxlot.get("auecLocalDate"), "auecLocalDate", parserSDF);
	        removeInTradeTaxlot.settlementDate = TaxlotManager.getDateSafe(taxlot.get("settlementDate"), "settlementDate", parserSDF);
	        removeInTradeTaxlot.userId = TaxlotManager.getStringSafe(taxlot.get("CompanyUserID"), "CompanyUserID");
	        removeInTradeTaxlot.strategyId = TaxlotManager.getIntSafe(taxlot.get("Level2ID"), "Level2ID");
	        removeInTradeTaxlot.orderTypeTagValue = TaxlotManager.getStringSafe(taxlot.get("OrderTypeTagValue"), "OrderTypeTagValue");
	        removeInTradeTaxlot.benchMarkRate = benchMarkRate;
	        removeInTradeTaxlot.differential = differential;
	        removeInTradeTaxlot.swapNotional = swapNotional;
	        removeInTradeTaxlot.dayCount = dayCount;
	        removeInTradeTaxlot.isSwapped = isSwap;
	        removeInTradeTaxlot.avgFxRateForTrade = TaxlotManager.getDoubleSafe(taxlot.get("FXRate"), "FXRate");
	        removeInTradeTaxlot.conversionMethodOperator = "M";
	        removeInTradeTaxlot.tif = TaxlotManager.getStringSafe(taxlot.get("TIF"), "TIF");
	        removeInTradeTaxlot.orderSide = TaxlotManager.getStringSafe(taxlot.get("OrderSide"), "OrderSide");
	        removeInTradeTaxlot.counterParty = TaxlotManager.getStringSafe(taxlot.get("CounterPartyName"), "CounterPartyName");
	        removeInTradeTaxlot.venue = TaxlotManager.getStringSafe(taxlot.get("Venue"), "Venue");
	        removeInTradeTaxlot.sideMultiplier = TaxlotManager.getIntSafe(taxlot.get("SideMultiplier"), "SideMultiplier");
	        removeInTradeTaxlot.orderType = TaxlotManager.getStringSafe(taxlot.get("OrderType"), "OrderType");
	        removeInTradeTaxlot.underlyingAsset = TaxlotManager.getStringSafe(taxlot.get("UnderlyingName"), "UnderlyingName");
	        removeInTradeTaxlot.limitPrice = TaxlotManager.getDoubleSafe(taxlot.get("LimitPrice"), "LimitPrice");
	        removeInTradeTaxlot.stopPrice = TaxlotManager.getDoubleSafe(taxlot.get("StopPrice"), "StopPrice");
	        removeInTradeTaxlot.isWhatIfTradeStreamRequired = true;
	        removeInTradeTaxlot.assetId = TaxlotManager.getIntSafe(taxlot.get("AssetID"), "AssetID");
	        removeInTradeTaxlot.asset = TaxlotManager.getStringSafe(taxlot.get("AssetName"), "AssetName");
	        removeInTradeTaxlot.auecId = TaxlotManager.getIntSafe(taxlot.get("AUECID"), "AUECID");
	        removeInTradeTaxlot.multiplier = TaxlotManager.getDoubleSafe(taxlot.get("ContractMultiplier"), "ContractMultiplier");
	        removeInTradeTaxlot.tradeAttribute1 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute1"), "TradeAttribute1");
	        removeInTradeTaxlot.tradeAttribute2 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute2"), "TradeAttribute2");
	        removeInTradeTaxlot.tradeAttribute3 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute3"), "TradeAttribute3");
	        removeInTradeTaxlot.tradeAttribute4 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute4"), "TradeAttribute4");
	        removeInTradeTaxlot.tradeAttribute5 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute5"), "TradeAttribute5");
	        removeInTradeTaxlot.tradeAttribute6 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute6"), "TradeAttribute6");

			// Remove from cache
			TaxlotCache.getInstance().removeFromCache(taxlotId + taxlotType);

	        // Send to CEP
	        TaxlotManager.sendTaxlotToCEPEngine(removeInTradeTaxlot, false);

	    } catch (Exception ex) {
	        PranaLogManager.error(ex);
	    }
	}

	private void securityReceived(HashMap<String, Object> map) {
		try {
			/*if (DataInitializationRequestProcessor._isBasketServiceStarted) {
				PranaLogManager.logOnly("Security details received for " + map.get("tickerSymbol").toString() + " at : " + _parserSdf.format(new Date()));
			}*/
			Date expirationDate = null;
			if (map.get("expirationDate") != null)
				expirationDate = parserSDF.parse(map.get("expirationDate").toString());
			
			DynamicUDA.getInstance().setSymbolDynamicUDAData(map.get("tickerSymbol").toString(), getDynamicUDAFromSecurity(map));

			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] {
					Integer.parseInt(map.get("assetId").toString()), map.get("tickerSymbol").toString(),
					Integer.parseInt(map.get("auecId").toString()), map.get("underlyingSymbol").toString(),
					Double.parseDouble(map.get("leveragedFactor").toString()), map.get("longName").toString(),
					Double.parseDouble(map.get("multiplier").toString()),
					Integer.parseInt(map.get("currencyId").toString()), expirationDate, map.get("putOrCall").toString(),
					Double.parseDouble(map.get("strikePrice").toString()),
					Integer.parseInt(map.get("leadCurrencyId").toString()),
					Integer.parseInt(map.get("vsCurrencyId").toString()),
					Boolean.parseBoolean(map.get("isCurrencyFuture").toString()), map.get("udaAsset").toString(),
					map.get("udaSecurityType").toString(), map.get("udaSector").toString(),
					map.get("udaSubSector").toString(), map.get("udaCountry").toString(),
					map.get("riskCurrency").toString(), map.get("issuer").toString(),
					map.get("countryOfRisk").toString(), map.get("region").toString(), map.get("analyst").toString(),
					map.get("ucitsEligibleTag").toString(), map.get("liquidTag").toString(),
					map.get("marketCap").toString(), map.get("customUDA1").toString(), map.get("customUDA2").toString(),
					map.get("customUDA3").toString(), map.get("customUDA4").toString(),
					map.get("customUDA5").toString(), map.get("customUDA6").toString(),
					map.get("customUDA7").toString(), Double.parseDouble(map.get("roundLot").toString()),
					map.get("currencyText").toString(), map.get("fxSymbol").toString(),
					BigDecimal.valueOf(((Number) map.get("sharesOutstanding")).doubleValue()),map.get("customUDA8").toString(),map.get("customUDA9").toString(),map.get("customUDA10").toString(),map.get("customUDA11").toString(),map.get("customUDA12").toString(),
					map.get("bloombergSymbol").toString(), map.get("activSymbol").toString(), map.get("factSetSymbol").toString(), map.get("bloombergSymbolWithExchangeCode")}, "Security");

			if (DataInitializationRequestProcessor._isBasketServiceStarted) {
				PranaLogManager.logOnly("Security details inserted for " + map.get("tickerSymbol").toString() + " at : "
						+ _parserSdf.format(new Date()));
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
	
	/**
	 * Creates collection of Dynamic UDA data of the symbol.
	 * @param map
	 * @return
	 */
	private HashMap<String, Object> getDynamicUDAFromSecurity(HashMap<String, Object> map) {
		HashMap<String, Object> dynamicUDAData = new HashMap<String, Object>();
		try {
			dynamicUDAData.put("riskCurrency", map.get("riskCurrency"));
			dynamicUDAData.put("issuer", map.get("issuer"));
			dynamicUDAData.put("countryOfRisk", map.get("countryOfRisk"));
			dynamicUDAData.put("region", map.get("region"));
			dynamicUDAData.put("analyst", map.get("analyst"));
			dynamicUDAData.put("ucitsEligibleTag", map.get("ucitsEligibleTag"));
			dynamicUDAData.put("liquidTag", map.get("liquidTag"));
			dynamicUDAData.put("marketCap", map.get("marketCap"));
			dynamicUDAData.put("customUDA1", map.get("customUDA1"));
			dynamicUDAData.put("customUDA2", map.get("customUDA2"));
			dynamicUDAData.put("customUDA3", map.get("customUDA3"));
			dynamicUDAData.put("customUDA4", map.get("customUDA4"));
			dynamicUDAData.put("customUDA5", map.get("customUDA5"));
			dynamicUDAData.put("customUDA6", map.get("customUDA6"));
			dynamicUDAData.put("customUDA7", map.get("customUDA7"));
			dynamicUDAData.put("customUDA8", map.get("customUDA8"));
			dynamicUDAData.put("customUDA9", map.get("customUDA9"));
			dynamicUDAData.put("customUDA10", map.get("customUDA10"));
			dynamicUDAData.put("customUDA11", map.get("customUDA11"));
			dynamicUDAData.put("customUDA12", map.get("customUDA12"));
		}
		catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return dynamicUDAData;
	}
	

	private void auecDetailsReceived(HashMap<String, Object> map) {
		try {
			Date today = parserSDF.parse(map.get("today").toString());
			Date yesterDay = parserSDF.parse(map.get("yesterDay").toString());
			CEPManager.getEPRuntime().getEventService()
					.sendEventObjectArray(new Object[] { Integer.parseInt(map.get("auecId").toString()), yesterDay,
							today, Integer.parseInt(map.get("assetId").toString()), map.get("asset").toString(),
							Integer.parseInt(map.get("exchangeId").toString()), map.get("exchangeName").toString(),
							Integer.parseInt(map.get("underlyingId").toString()), map.get("underlying").toString(),
							Integer.parseInt(map.get("currencyId").toString()), map.get("currency").toString(),
							map.get("fxSymbol").toString() }, "AuecDetails");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void strategyCollectionReceived(HashMap<String, Object> map) throws Exception {
		try {
			CEPManager.getEPRuntime().getEventService()
					.sendEventObjectArray(new Object[] { Integer.parseInt(map.get("strategyId").toString()),
							map.get("strategyFullName").toString(), map.get("strategyName").toString(),
							Integer.parseInt(map.get("companyId").toString()),
							Integer.parseInt(map.get("masterStrategyId").toString()),
							map.get("masterStrategyName").toString() }, "StrategyCollection");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void accountCollectionReceived(HashMap<String, Object> map) throws Exception {
		try {
			int accountId = Integer.parseInt(map.get("accountId").toString());
			int masterFundId = Integer.parseInt(map.get("masterFundId").toString());
			TaxlotCache.getInstance().addOrUpdateToMasterFundCache(masterFundId, accountId);
			CEPManager.getEPRuntime().getEventService()
					.sendEventObjectArray(new Object[] { accountId, map.get("accountLongName").toString(),
							map.get("accountShortName").toString(), Integer.parseInt(map.get("companyId").toString()),
							masterFundId, map.get("masterFundName").toString(), 
							Integer.parseInt(map.get("baseCurrencyId").toString()) }, "AccountCollection");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void pmCalculationPreference(HashMap<String, Object> map) {
		try {
			int accountId = Integer.valueOf(map.get("accountId").toString());
			double highWaterMark = Double.valueOf(map.get("highWaterMark").toString());
			double stopout = Double.valueOf(map.get("stopOut").toString());
			double traderPayoutPercent = Double.valueOf(map.get("traderPayoutPercent").toString());
			double longDebitBalance = Double.valueOf(map.get("longDebitBalance").toString());
			double shortCreditBalance = Double.valueOf(map.get("shortCreditBalance").toString());
			double shortCreditLimit = Double.valueOf(map.get("shortCreditLimit").toString());
			double longDebitLimit = Double.valueOf(map.get("longDebitLimit").toString());
			double capital = Double.valueOf(map.get("capital").toString());
			double mtdPnl = Double.valueOf(map.get("mtdPnl").toString());

			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(
					new Object[] { accountId, highWaterMark, stopout, traderPayoutPercent, longDebitBalance,
							shortCreditBalance, shortCreditLimit, longDebitLimit, capital, mtdPnl, },
					"PMCalculationPrefs");

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * 
	 * @param cashEvents
	 */
	void sendCashEventsToEsper(HashMap<String, Object> cashEvents) {
		try {
			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(
					new Object[] { cashEvents.get("cashAmount"), cashEvents.get("date"), cashEvents.get("AccountId") },
					"CashEventBaseCurrency");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	void sendDBNavToEsper(HashMap<String, Object> dbNav) {
		try {
			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(
					new Object[] { dbNav.get("AccountId"), dbNav.get("StartOfDayNav"), dbNav.get("CurrentNav") },
					"DbNav");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * 
	 * @param markPrice
	 */
	void sendMarkPriceToEsper(HashMap<String, Object> markPrice) {
		try {
			CEPManager.getEPRuntime().getEventService()
					.sendEventObjectArray(new Object[] { markPrice.get("Symbol"),
							Double.parseDouble(markPrice.get("MarkPrice").toString()),
							Integer.parseInt(markPrice.get("AssetID").toString()) }, "MarkPrice");
			PranaLogManager.logOnly("MarkPrice received for symbol: " + markPrice.get("Symbol") + ", MarkPrice: "
					+ markPrice.get("MarkPrice"));
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	void sendYesterdayFxRatesToEsper(HashMap<String, Object> yesterdayFxRates) {
		try {
			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] {
					yesterdayFxRates.get("symbol"), yesterdayFxRates.get("date"), yesterdayFxRates.get("fxRate") },
					"YesterdayFxRates");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	void sendYesterdayNavToEsper(HashMap<String, Object> yesterdayNav) {
		try {
			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(
					new Object[] { yesterdayNav.get("AccountId"), yesterdayNav.get("date"), yesterdayNav.get("nav") },
					"YesterDayNav");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void TaxlotBaseDataReceived(HashMap<String, Object> taxlot) {
		try {
			String fieldName = null;
			Set<String> keySet = taxlot.keySet();
			TaxlotEntities taxlotEntities = new TaxlotEntities();
			Field fields[] = TaxlotEntities.class.getDeclaredFields();
			for (Field field : fields) {
				fieldName = field.getName();
				if (keySet.contains(fieldName) && taxlot.get(fieldName) == null) {
					taxlot.replace(fieldName, taxlot.get(fieldName), field.get(taxlotEntities));
				}
			}

			if (DataInitializationRequestProcessor._isBasketServiceStarted) {
				TaxlotCache.getInstance().addOrUpdateToCache(taxlot);
			} else {
				_historicalCounter++;
				PranaLogManager.info("Esper Taxlot Received: " + _historicalCounter);
				TaxlotCache.getInstance().addOrUpdateToCache(taxlot);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/*
	 * Symbol Data snapshot from Esper
	 */
	private void SymbolDataReceived(HashMap<String, Object> symbolData) {
		try {
			String symbol = symbolData.get("symbol").toString();
			if (!symbol.isEmpty()) {
				SymbolDataCache.getInstance().addOrUpdateToCache(symbolData);

				int assetId = Integer.parseInt(symbolData.get("assetId").toString());
				double selectedFeedPrice = Double.parseDouble(symbolData.get("selectedFeedPrice").toString());

				CEPManager.getEPRuntime().getEventService()
						.sendEventObjectArray(
								new Object[] { symbol, symbolData.get("underlyingSymbol"),
										Double.parseDouble(symbolData.get("askPrice").toString()),
										Double.parseDouble(symbolData.get("bidPrice").toString()),
										Double.parseDouble(symbolData.get("lowPrice").toString()),
										Double.parseDouble(symbolData.get("highPrice").toString()),
										Double.parseDouble(symbolData.get("openPrice").toString()),
										Double.parseDouble(symbolData.get("closePrice").toString()),
										Double.parseDouble(symbolData.get("lastPrice").toString()), selectedFeedPrice,
										symbolData.get("conversionMethod"),
										symbolData.get("markPrice"),
										Double.parseDouble(symbolData.get("delta").toString()),
										Double.parseDouble(symbolData.get("beta5YearMonthly").toString()), assetId,
										Double.parseDouble(symbolData.get("openInterest").toString()),
										Double.parseDouble(symbolData.get("avgVolume20Days").toString()),
										BigDecimal.valueOf(
												((Number) symbolData.get("sharesOutstanding")).doubleValue()) },
								CollectorConstants.SYMBOL_DATA_EVENT_NAME);

			}

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}