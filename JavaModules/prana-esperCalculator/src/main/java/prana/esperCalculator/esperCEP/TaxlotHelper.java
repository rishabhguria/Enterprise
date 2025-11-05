package prana.esperCalculator.esperCEP;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;

import prana.esperCalculator.cacheClasses.MarketDataDualCache;
import prana.esperCalculator.cacheClasses.StageDataDualCache;
import prana.esperCalculator.cacheClasses.TaxlotDualCache;
import prana.esperCalculator.commonCode.CEPManager;
import prana.esperCalculator.commonCode.TaxlotManager;
import prana.esperCalculator.commonCode.TaxlotManager.TaxlotState;
import prana.esperCalculator.commonCode.TaxlotManager.TaxlotType;
import prana.esperCalculator.communication.DataInitializationRequestProcessor;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.main.PendingWhatIfCache;
import prana.esperCalculator.objects.Taxlot;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

/**
 * This class runs in background and send all data stored in SymbolDataDualCache
 * 
 * @author dewashish
 * 
 */
public class TaxlotHelper implements Runnable {
	private SimpleDateFormat parserSDF = new SimpleDateFormat(ConfigurationConstants.SIMPLE_DATE_FORMAT);

	// Added delay of 3 Sec while sending taxlot Symbol data to Esper.
	// SleepPostOnWhatIf is configurable, you can change delay time in config file.
	int _sleepPostOnWhatIf = 3000;

	@Override
	public void run() {
		try {

			int taxlotHelperDelay = Integer.parseInt(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_TAXLOT_HELPER_DELAY)) * 1000;

			_sleepPostOnWhatIf = Integer.parseInt(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_SLEEP_POST_ON_WHAT_IF)) * 1000;

			Object taxlotHelperObject = new Object();

			PranaLogManager.info("Taxlot helper started to send data to esper");
			while (true) {
				sendDataToEngine();
				// Delaying taxlot input to engine by some time according to configuration
				synchronized (taxlotHelperObject) {
					taxlotHelperObject.wait(taxlotHelperDelay);
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void sendDataToEngine() {
		try {
			HashMap<String, HashMap<String, Object>> taxlotDualCache = TaxlotDualCache.getInstance()
					.getLatestTaxlotCache();
			HashMap<String, HashMap<String, Object>> marketDualCache = MarketDataDualCache.getInstance()
					.getLatestTaxlotCache();
			HashMap<String, HashMap<String, Object>> stageDualCache = StageDataDualCache.getInstance()
					.getLatestTaxlotCache();

			sendCollectedData(taxlotDualCache, TaxlotType.Post);
			sendCollectedData(stageDualCache, TaxlotType.InTradeStage);
			sendCollectedData(marketDualCache, TaxlotType.InTradeMarket);

			if (!taxlotDualCache.isEmpty() || !marketDualCache.isEmpty() || !stageDualCache.isEmpty())
				AdviseSymbolHelper.getInstance().resetAdviseList();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void sendCollectedData(HashMap<String, HashMap<String, Object>> dataDualCache, TaxlotType type) {
		try {
			if (dataDualCache != null && !dataDualCache.isEmpty()) {
				long startTime = System.currentTimeMillis();
				PranaLogManager.logOnly("Taxlot cache size " + dataDualCache.size() + ", " + type + " sending to esper engine started");
				for (String key : dataDualCache.keySet()) {
					int sendTime = 0;
					while (!PendingWhatIfCache.getInstance().isEmpty()) {
						sendTime = sendTime + _sleepPostOnWhatIf;
						Thread.sleep(_sleepPostOnWhatIf);
						CEPManager.notifyIfTimerExceedsLimit(sendTime);
					}
					if (sendTime > 0)
						PranaLogManager
								.info(key + ", " + type + " sending was delayed for " + sendTime / 1000 + " seconds");

					if (type.equals(TaxlotType.Post))
						sendTaxlot(dataDualCache.get(key));
					else
						sendInTradeTaxlot(dataDualCache.get(key), type);
				}
				long endTime = System.currentTimeMillis();
				double diff = endTime - startTime;
				//if (diff > 2000) {
					PranaLogManager.logOnly(dataDualCache.size() + ", " + type
							+ " Taxlot data events sent to esper engine in " + diff / 1000 + " seconds");
				//}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void sendTaxlot(HashMap<String, Object> taxlot) {
		try {
			if (taxlot != null) {
				Taxlot taxlotObj = new Taxlot();

				taxlotObj.isSwapped = TaxlotManager.getBooleanSafe(taxlot.get("IsSwapped"), "IsSwapped");
				taxlotObj.benchMarkRate = 0.0;
				taxlotObj.differential = 0.0;
				taxlotObj.swapNotional = 0.0;
				taxlotObj.dayCount = 0.0;

				if (taxlotObj.isSwapped) {
					HashMap<String, Object> swapParameters = JSONMapper.convertObject(taxlot.get("SwapParameters"));
					taxlotObj.benchMarkRate = TaxlotManager.getDoubleSafe(swapParameters.get("BenchMarkRate"),
							"BenchMarkRate");
					taxlotObj.differential = TaxlotManager.getDoubleSafe(swapParameters.get("Differential"),
							"Differential");
					taxlotObj.swapNotional = TaxlotManager.getDoubleSafe(swapParameters.get("NotionalValue"),
							"NotionalValue");
					taxlotObj.dayCount = TaxlotManager.getDoubleSafe(swapParameters.get("DayCount"), "DayCount");
				}

				taxlotObj.taxlotState = TaxlotState.New;
				taxlotObj.conversionMethodOperator = "M";
				if (taxlot.containsKey("FXConversionMethodOnTradeDate")) {
					int methodId = TaxlotManager.getIntSafe(taxlot.get("FXConversionMethodOnTradeDate"),
							"FXConversionMethodOnTradeDate");
					if (methodId == 1)
						taxlotObj.conversionMethodOperator = "D";
				}

				String epnlOrderState = TaxlotManager.getStringSafe(taxlot.get("EpnlOrderState"), "EpnlOrderState");
				switch (epnlOrderState) {
					case "0":
					case "1":
						taxlotObj.taxlotState = TaxlotState.New;
						break;
					case "2":
						taxlotObj.taxlotState = TaxlotState.Updated;
						break;
					case "3":
						taxlotObj.taxlotState = TaxlotState.Deleted;
						break;
				}

				taxlotObj.auecLocalDate = TaxlotManager.getDateSafe(taxlot.get("AUECLocalDate"), "AUECLocalDate",
						parserSDF);
				taxlotObj.settlementDate = TaxlotManager.getDateSafe(taxlot.get("SettlementDate"), "SettlementDate",
						parserSDF);

				taxlotObj.basketId = TaxlotManager.getStringSafe(taxlot.get("ID"), "ID");
				taxlotObj.taxlotId = TaxlotManager.getStringSafe(taxlot.get("ID"), "ID");
				taxlotObj.clOrderId = TaxlotManager.getStringSafe(taxlot.get("ID"), "ID");
				taxlotObj.taxlotType = TaxlotType.Post;
				taxlotObj.symbol = TaxlotManager.getStringSafe(taxlot.get("Symbol"), "Symbol");
				taxlotObj.underlyingSymbol = TaxlotManager.getStringSafe(taxlot.get("UnderlyingSymbol"),
						"UnderlyingSymbol");
				taxlotObj.quantity = TaxlotManager.getDoubleSafe(taxlot.get("Quantity"), "Quantity");
				taxlotObj.avgPrice = TaxlotManager.getDoubleSafe(taxlot.get("AvgPrice"), "AvgPrice");
				taxlotObj.orderSideTagValue = TaxlotManager.getStringSafe(taxlot.get("OrderSideTagValue"),
						"OrderSideTagValue");
				taxlotObj.accountId = TaxlotManager.getIntSafe(taxlot.get("Level1ID"), "Level1ID");
				taxlotObj.counterPartyId = TaxlotManager.getIntSafe(taxlot.get("CounterPartyId"), "CounterPartyId");
				taxlotObj.venueId = TaxlotManager.getIntSafe(taxlot.get("VenueId"), "VenueId");
				taxlotObj.userId = "NA";
				taxlotObj.strategyId = TaxlotManager.getIntSafe(taxlot.get("Level2ID"), "Level2ID");
				taxlotObj.orderTypeTagValue = TaxlotManager.getStringSafe(taxlot.get("OrderTypeTagValue"),
						"OrderTypeTagValue");
				taxlotObj.avgFxRateForTrade = TaxlotManager.getDoubleSafe(taxlot.get("FXRateOnTradeDate"),
						"FXRateOnTradeDate");
				taxlotObj.tif = TaxlotManager.getStringSafe(taxlot.get("TIF"), "TIF");
				taxlotObj.orderSide = TaxlotManager.getStringSafe(taxlot.get("OrderSide"), "OrderSide");
				taxlotObj.counterParty = TaxlotManager.getStringSafe(taxlot.get("CounterPartyName"),
						"CounterPartyName");
				taxlotObj.venue = TaxlotManager.getStringSafe(taxlot.get("Venue"), "Venue");
				taxlotObj.sideMultiplier = TaxlotManager.getIntSafe(taxlot.get("SideMultiplier"), "SideMultiplier");
				taxlotObj.orderType = TaxlotManager.getStringSafe(taxlot.get("OrderType"), "OrderType");
				taxlotObj.underlyingAsset = TaxlotManager.getStringSafe(taxlot.get("UnderlyingName"), "UnderlyingName");
				taxlotObj.limitPrice = TaxlotManager.getDoubleSafe(taxlot.get("LimitPrice"), "LimitPrice");
				taxlotObj.stopPrice = TaxlotManager.getDoubleSafe(taxlot.get("StopPrice"), "StopPrice");
				taxlotObj.isWhatIfTradeStreamRequired = true;
				taxlotObj.assetId = TaxlotManager.getIntSafe(taxlot.get("Asset"), "Asset");
				taxlotObj.asset = TaxlotManager.getStringSafe(taxlot.get("AssetName"), "AssetName");
				taxlotObj.auecId = TaxlotManager.getIntSafe(taxlot.get("AUECID"), "AUECID");
				taxlotObj.multiplier = TaxlotManager.getDoubleSafe(taxlot.get("Multiplier"), "Multiplier");
				taxlotObj.tradeAttribute1 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute1"),
						"TradeAttribute1");
				taxlotObj.tradeAttribute2 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute2"),
						"TradeAttribute2");
				taxlotObj.tradeAttribute3 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute3"),
						"TradeAttribute3");
				taxlotObj.tradeAttribute4 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute4"),
						"TradeAttribute4");
				taxlotObj.tradeAttribute5 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute5"),
						"TradeAttribute5");
				taxlotObj.tradeAttribute6 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute6"),
						"TradeAttribute6");

				// Send to CEP
				TaxlotManager.sendTaxlotToCEPEngine(taxlotObj, false);
				
				if (taxlotObj.taxlotState.equals(TaxlotState.Updated)
						|| taxlotObj.taxlotState.equals(TaxlotState.Deleted)) {
					taxlot.put("TaxlotType", TaxlotType.Post.toString());
					DataInitializationRequestProcessor.getInstance().sendDeletedDataForBasketCompliance(taxlot,
							"DeleteTaxlotData");
				}
			}

			AdviseSymbolHelper.getInstance()
					.updateAdviceList(TaxlotManager.getStringSafe(taxlot.get("Symbol"), "Symbol"));

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Send In Trade taxlots to Esper
	 * 
	 * @param taxlot
	 * @param type
	 */
	private void sendInTradeTaxlot(HashMap<String, Object> taxlot, TaxlotType type) {
		try {
			SimpleDateFormat parserSdf = new SimpleDateFormat(ConfigurationConstants.SIMPLE_DATE_FORMAT);

			Date aUECLocalDate = TaxlotManager.getDateSafe(taxlot.get("AUECLocalDate"), "AUECLocalDate", parserSdf);
			Date settlementDate = TaxlotManager.getDateSafe(taxlot.get("SettlementDate"), "SettlementDate", parserSdf);
			String clOrderId = TaxlotManager.getStringSafe(taxlot.get("LotId"), "LotId");

			@SuppressWarnings("unchecked")
			HashMap<String, Object> swapParameters = (HashMap<String, Object>) taxlot.get("SwapParameters");
			boolean isSwap = TaxlotManager.checkAndLogSwap(taxlot.get("IsSwapped"), swapParameters);

			double benchMarkRate = 0.0;
			double differential = 0.0;
			double swapNotional = 0.0;
			double dayCount = 0.0;

			TaxlotState taxlotState = TaxlotState.None;
			switch (TaxlotManager.getStringSafe(taxlot.get("TaxLotState"), "TaxLotState")) {
				case "0":
				case "1":
					taxlotState = TaxlotState.New;
					break;
				case "2":
					taxlotState = TaxlotState.Updated;
					break;
				case "3":
					taxlotState = TaxlotState.Deleted;
					break;
			}

			if (isSwap) {
				benchMarkRate = TaxlotManager.getDoubleSafe(swapParameters.get("BenchMarkRate"), "BenchMarkRate");
				differential = TaxlotManager.getDoubleSafe(swapParameters.get("Differential"), "Differential");
				swapNotional = TaxlotManager.getDoubleSafe(swapParameters.get("NotionalValue"), "NotionalValue");
				dayCount = TaxlotManager.getDoubleSafe(swapParameters.get("DayCount"), "DayCount");
			}

			Taxlot inTradeTaxlot = new Taxlot();
			inTradeTaxlot.basketId = TaxlotManager.getStringSafe(taxlot.get("GroupID"), "GroupID");
			inTradeTaxlot.taxlotId = TaxlotManager.getStringSafe(taxlot.get("TaxLotID"), "TaxLotID");
			inTradeTaxlot.clOrderId = clOrderId;
			inTradeTaxlot.taxlotType = type;
			inTradeTaxlot.taxlotState = taxlotState;
			inTradeTaxlot.symbol = TaxlotManager.getStringSafe(taxlot.get("Symbol"), "Symbol");
			inTradeTaxlot.underlyingSymbol = TaxlotManager.getStringSafe(taxlot.get("UnderlyingSymbol"),
					"UnderlyingSymbol");
			inTradeTaxlot.quantity = TaxlotManager.getDoubleSafe(taxlot.get("TaxLotQty"), "TaxLotQty");
			inTradeTaxlot.avgPrice = TaxlotManager.getDoubleSafe(taxlot.get("AvgPrice"), "AvgPrice");
			inTradeTaxlot.orderSideTagValue = TaxlotManager.getStringSafe(taxlot.get("OrderSideTagValue"),
					"OrderSideTagValue");
			inTradeTaxlot.accountId = TaxlotManager.getIntSafe(taxlot.get("Level1ID"), "Level1ID");
			inTradeTaxlot.counterPartyId = TaxlotManager.getIntSafe(taxlot.get("CounterPartyID"), "CounterPartyID");
			inTradeTaxlot.venueId = TaxlotManager.getIntSafe(taxlot.get("VenueID"), "VenueID");
			inTradeTaxlot.auecLocalDate = aUECLocalDate;
			inTradeTaxlot.settlementDate = settlementDate;
			inTradeTaxlot.userId = TaxlotManager.getStringSafe(taxlot.get("CompanyUserID"), "CompanyUserID");
			inTradeTaxlot.strategyId = TaxlotManager.getIntSafe(taxlot.get("Level2ID"), "Level2ID");
			inTradeTaxlot.orderTypeTagValue = TaxlotManager.getStringSafe(taxlot.get("OrderTypeTagValue"),
					"OrderTypeTagValue");
			inTradeTaxlot.benchMarkRate = benchMarkRate;
			inTradeTaxlot.differential = differential;
			inTradeTaxlot.swapNotional = swapNotional;
			inTradeTaxlot.dayCount = dayCount;
			inTradeTaxlot.isSwapped = isSwap;
			inTradeTaxlot.avgFxRateForTrade = TaxlotManager.getDoubleSafe(taxlot.get("FXRate"), "FXRate");
			inTradeTaxlot.conversionMethodOperator = "M";
			inTradeTaxlot.tif = TaxlotManager.getStringSafe(taxlot.get("TIF"), "TIF");
			inTradeTaxlot.orderSide = TaxlotManager.getStringSafe(taxlot.get("OrderSide"), "OrderSide");
			inTradeTaxlot.counterParty = TaxlotManager.getStringSafe(taxlot.get("CounterPartyName"),
					"CounterPartyName");
			inTradeTaxlot.venue = TaxlotManager.getStringSafe(taxlot.get("Venue"), "Venue");
			inTradeTaxlot.sideMultiplier = TaxlotManager.getIntSafe(taxlot.get("SideMultiplier"), "SideMultiplier");
			inTradeTaxlot.orderType = TaxlotManager.getStringSafe(taxlot.get("OrderType"), "OrderType");
			inTradeTaxlot.underlyingAsset = TaxlotManager.getStringSafe(taxlot.get("UnderlyingName"), "UnderlyingName");
			inTradeTaxlot.limitPrice = TaxlotManager.getDoubleSafe(taxlot.get("LimitPrice"), "LimitPrice");
			inTradeTaxlot.stopPrice = TaxlotManager.getDoubleSafe(taxlot.get("StopPrice"), "StopPrice");
			inTradeTaxlot.isWhatIfTradeStreamRequired = true;
			inTradeTaxlot.assetId = TaxlotManager.getIntSafe(taxlot.get("AssetID"), "AssetID");
			inTradeTaxlot.asset = TaxlotManager.getStringSafe(taxlot.get("AssetName"), "AssetName");
			inTradeTaxlot.auecId = TaxlotManager.getIntSafe(taxlot.get("AUECID"), "AUECID");
			inTradeTaxlot.multiplier = TaxlotManager.getDoubleSafe(taxlot.get("ContractMultiplier"),
					"ContractMultiplier");
			inTradeTaxlot.tradeAttribute1 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute1"),
					"TradeAttribute1");
			inTradeTaxlot.tradeAttribute2 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute2"),
					"TradeAttribute2");
			inTradeTaxlot.tradeAttribute3 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute3"),
					"TradeAttribute3");
			inTradeTaxlot.tradeAttribute4 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute4"),
					"TradeAttribute4");
			inTradeTaxlot.tradeAttribute5 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute5"),
					"TradeAttribute5");
			inTradeTaxlot.tradeAttribute6 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute6"),
					"TradeAttribute6");

			// Send to CEP
			TaxlotManager.sendTaxlotToCEPEngine(inTradeTaxlot, false);

			AdviseSymbolHelper.getInstance()
					.addAdviceForSymbol(TaxlotManager.getStringSafe(taxlot.get("Symbol"), "Symbol"));

			if (inTradeTaxlot.taxlotState.equals(TaxlotState.Deleted)) {
				taxlot.put("TaxlotType", type);
				DataInitializationRequestProcessor.getInstance().sendDeletedDataForBasketCompliance(taxlot,
						"DeleteTaxlotData");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}