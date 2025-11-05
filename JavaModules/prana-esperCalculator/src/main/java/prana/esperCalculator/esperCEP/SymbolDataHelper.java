package prana.esperCalculator.esperCEP;

import java.text.SimpleDateFormat;
import java.util.Arrays;
import java.util.Collections;
import java.util.Date;
import java.util.HashMap;
import java.util.Set;
import java.util.stream.Collectors;

import com.espertech.esper.common.client.EventBean;
import prana.esperCalculator.cacheClasses.SymbolDataDualCache;
import prana.esperCalculator.commonCode.CEPManager;
import prana.esperCalculator.communication.DataInitializationRequestProcessor;
import prana.esperCalculator.constants.CollectorConstants;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.main.PendingWhatIfCache;
import prana.esperCalculator.objects.SymbolData;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

/**
 * This class runs in background and send all data stored in SymbolDataDualCache
 * 
 * @author dewashish
 * 
 */
public class SymbolDataHelper implements Runnable {
	int _count = 0;

	// Added delay of 3 Sec while sending post Symbol data to Esper.
	// SleepPostOnWhatIf is configurable, you can change delay time in config file.
	public int _sleepPostOnWhatIf = 3000;

	/*
	 * If IsLiveMode config value is true, then calculation will be done on actual
	 * live prices If IsLiveMode config value is false, then calculation will be
	 * done on manual prices (According to +2 and -2 logic)
	 */
	boolean _isLiveMode = true;

	/*
	 * If IsSymbolDataEOMRequiredOnStaticPrices config value is true, then
	 * SymbolDataEOM will be generated for Stale and Live prices(Calculation on
	 * every cycle).
	 * If IsSymbolDataEOMRequiredOnStaticPrices config value is false, then
	 * SymbolDataEOM will not be generated for Stale prices(Calculation on Live
	 * prices only).
	 */
	boolean _isSymbolDataEOMRequiredOnStalePrices = true;

	/**
	 * Date format parser initialized with a format specified in configuration.
	 */
	SimpleDateFormat _parserSdf = new SimpleDateFormat("MM/dd/yyyy HH:mm:ss.SSS XXX");

	/**
	 * Set of symbols that require logging.
	 * This is populated from the configuration settings.
	 */
	public Set<String> symbolsRequireLogging;

	@Override
	public void run() {
		try {
			int symbolDataHelperDelay = Integer.parseInt(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_SYMBOL_DATA_HELPER_DELAY)) * 1000;

			_sleepPostOnWhatIf = Integer.parseInt(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_SLEEP_POST_ON_WHAT_IF)) * 1000;

			_isLiveMode = Boolean.parseBoolean(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_IS_LIVE_MODE));

			_isSymbolDataEOMRequiredOnStalePrices = Boolean.parseBoolean(ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_IS_SYMBOL_DATA_EOM_REQUIRED));

			PranaLogManager.info("Symboldata helper started to send data to esper");

			String symbolsList = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_SYMBOLS_REQUIRE_LOGGING);

			symbolsRequireLogging = symbolsList != null ? Arrays.stream(symbolsList.split(",")).map(String::trim)
					.filter(s -> !s.isEmpty()).collect(Collectors.toSet()) : Collections.emptySet();
			PranaLogManager.logOnly("Symbols Require Logging: " + symbolsRequireLogging);

			Object symbolDataHelperObject = new Object();
			while (true) {
				double timeInterval = 0;
				if (DataInitializationRequestProcessor.getInstance()._isEsperStarted) {
					timeInterval = sendDataToEngine();
				}
				if (timeInterval < symbolDataHelperDelay) {
					synchronized (symbolDataHelperObject) {
						symbolDataHelperObject.wait(symbolDataHelperDelay);
					}
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private double sendDataToEngine() {
		double diff = 0;
		try {
			HashMap<String, SymbolData> symbolDataCache = SymbolDataDualCache.getInstance().getLatestSymbolDataCache();
			_count++;
			if (symbolDataCache != null && !symbolDataCache.isEmpty()) {
				CEPManager.setVariableValue(ConfigurationConstants.KEY_IS_SYMBOL_DATA_CHANGED, false);
				long startTime = System.currentTimeMillis();
			//	PranaLogManager.logOnly(
				//		_parserSdf.format(new Date()) + " - Cycle #" + _count + ": Sending " + symbolDataCache.size()
			//					+ " symbol data events to Esper engine (start).");
				for (String key : symbolDataCache.keySet()) {
					int sendTime = 0;
					long startTimeSymbol = System.currentTimeMillis();
					while (!PendingWhatIfCache.getInstance().isEmpty()
							|| PendingWhatIfCache.getIsValidatedSymbolDataReceived()) {
						sendTime = sendTime + _sleepPostOnWhatIf;
						Thread.sleep(_sleepPostOnWhatIf);
						CEPManager.notifyIfTimerExceedsLimit(sendTime);
					}
					if (sendTime > 0)
						PranaLogManager
								.logOnly(key + ", Symbol sending was delayed for " + sendTime / 1000 + " seconds");
					sendSymbolData(symbolDataCache.get(key));
					long endTimeSymbol = System.currentTimeMillis();
					double diffSymbol = endTimeSymbol - startTimeSymbol;

					Boolean symbolUpdateLoggingPermission = Boolean.parseBoolean(ConfigurationHelper.getInstance()
							.getValueBySectionAndKey(ConfigurationConstants.SECTION_APP_SETTINGS,
									ConfigurationConstants.KEY_APP_SETTINGS_SYMBOL_DATA_UPDATE_LOGGING));
					if (symbolUpdateLoggingPermission)
						PranaLogManager.logOnly("symbol key: " + key + " , Symbol Data updation time duration : "
								+ diffSymbol / 1000 + " seconds");
				}

				// Sending EOM End Message for this Cycle and SymbolData cycle completed Event
				if (_isSymbolDataEOMRequiredOnStalePrices || Boolean.parseBoolean(
						CEPManager.getVariableValue(ConfigurationConstants.KEY_IS_SYMBOL_DATA_CHANGED).toString())) {
					// Starting NAV Calculations for this cycle.
					startNavCalculation();

					// Sending SymbolDataEOM
					int sendTimeForEOM = 0;
					while (!PendingWhatIfCache.getInstance().isEmpty()
							|| PendingWhatIfCache.getIsValidatedSymbolDataReceived()) {
						sendTimeForEOM = sendTimeForEOM + _sleepPostOnWhatIf;
						Thread.sleep(_sleepPostOnWhatIf);
						CEPManager.notifyIfTimerExceedsLimit(sendTimeForEOM);
					}
					if (sendTimeForEOM > 0)
						PranaLogManager.logOnly(
								"Symbol Data EOM sending was delayed for " + sendTimeForEOM / 1000 + " seconds");
					sendSymbolDataEOM();
				}

				long endTime = System.currentTimeMillis();
				diff = endTime - startTime;
				PranaLogManager.logOnly(
						_parserSdf.format(new Date()) + " - Cycle #" + _count + ": Sent " + symbolDataCache.size()
								+ " symbol data events to Esper engine in " + (diff / 1000) + " seconds.");

				// Sending SymbolDataWindow data to Basket Compliance
				sendSymbolDataWindowDataToBasketCompliance();
				
			} else if (_isSymbolDataEOMRequiredOnStalePrices) {
				PranaLogManager.logOnly(
						_parserSdf.format(new Date()) + " - Cycle #" + _count
								+ ": No symbol data found, running NAV calculations and sending EOM events (EOM required on stale prices).");
				// Starting NAV Calculations for this cycle.
				startNavCalculation();
				// Sending EOM End Message for this Cycle and SymbolData cycle completed Event
				sendSymbolDataEOM();
				// Sending SymbolDataWindow data to Basket Compliance
				sendSymbolDataWindowDataToBasketCompliance();
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return diff;
	}

	/*
	 * Starting NAV calculations for the current cycle.
	 */
	private void startNavCalculation() {
	    try {
	      //  PranaLogManager.logOnly(_parserSdf.format(new Date()) + " - Cycle #" + _count + ": Starting NAV calculations.");
	        CEPManager.getEPRuntime().getEventService()
	                .sendEventObjectArray(new Object[] { true, "StartNavCalculation" }, "InitComplete");
	    } catch (Exception ex) {
	        PranaLogManager.error(ex);
	    }
	}
	
	/*
	 * Sending SymbolDataWindow data to Basket Compliance.
	 */
	private void sendSymbolDataWindowDataToBasketCompliance() {
		try {
			if ((boolean) CEPManager.getVariableValue(ConfigurationConstants.IS_BASKETCOMPLIANCE_STARTED)) {
			//	PranaLogManager.logOnly(_parserSdf.format(new Date())
				//		+ " - Sending SymbolDataWindow data to Basket Compliance started...");
				EventBean[] eventBeanArray = DataInitializationRequestProcessor.getInstance()
						.getWindowDataFrom("SymbolDataWindow");
				for (EventBean eventBean : eventBeanArray) {
					DataInitializationRequestProcessor.getInstance().sendSymbolDataForBasketCompliance(
							JSONMapper.getStringForObject(eventBean.getUnderlying()), "SymbolDataWindowData");
				}
				PranaLogManager.logOnly(_parserSdf.format(new Date())
						+ " - SymbolDataWindow data sent to Basket Compliance Completely");
			}
		} catch (Exception ex) {
	        PranaLogManager.error(ex);
	    }
	}

	/*
	 * Sending EOM End Message for this Cycle and SymbolData cycle completed Event
	 */
	private void sendSymbolDataEOM() {
	    if (CEPManager.getEPRuntime() != null && (PendingWhatIfCache.getInstance().isEmpty()
	            && !PendingWhatIfCache.getIsValidatedSymbolDataReceived())) {
	     //   PranaLogManager.logOnly(_parserSdf.format(new Date()) + " - Cycle #" + _count + ": Dispatching SymbolDataCycleCompleted event.");
	        CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] { _count },
	                CollectorConstants.SYMBOL_DATA_CYCLE_COMPLETED_EVENT_NAME);

	      //  PranaLogManager.logOnly(_parserSdf.format(new Date()) + " - Cycle #" + _count + ": Dispatching SymbolDataEndEvent (EOM).");
	        CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] { _count },
	                CollectorConstants.SYMBOL_DATA_END_EVENT_NAME);
	    }
	}

	private void sendSymbolData(SymbolData symbolData) {
		try {
			if (symbolData != null) {

				// If IsLiveMode value is false, then calculation will be done manually changed
				// in price (According to +2 and -2 Logic)
				if (!_isLiveMode) {
					if (_count % 2 == 0) {
						symbolData.selectedfeedPX = symbolData.selectedfeedPX + 2;
						symbolData.ask = symbolData.ask + 2;
						symbolData.low = symbolData.low + 2;
						symbolData.bid = symbolData.bid + 2;
						symbolData.high = symbolData.high + 2;
						symbolData.open = symbolData.open + 2;
					} else {
						symbolData.selectedfeedPX = symbolData.selectedfeedPX - 2;
						symbolData.ask = symbolData.ask - 2;
						symbolData.low = symbolData.low - 2;
						symbolData.bid = symbolData.bid - 2;
						symbolData.high = symbolData.high - 2;
						symbolData.open = symbolData.open - 2;
					}
				}

				CEPManager.getEPRuntime().getEventService()
						.sendEventObjectArray(new Object[] { symbolData.symbol, symbolData.underlyingSymbol,
								symbolData.ask, symbolData.bid, symbolData.low, symbolData.high, symbolData.open, 0.0,
								symbolData.lastPrice, symbolData.selectedfeedPX, symbolData.conversionMethod,
								symbolData.markPrice, symbolData.delta, symbolData.beta5yr, symbolData.categoryCode,
								symbolData.openInterest, symbolData.avgVol20Days, symbolData.sharesOutStandings,
								symbolData.pricingStatus },
								CollectorConstants.SYMBOL_DATA_EVENT_NAME);

				if (!symbolsRequireLogging.isEmpty() && symbolsRequireLogging.contains(symbolData.symbol)) {
					PranaLogManager.logOnly(_parserSdf.format(new Date()) + " - Sending information for Symbol: "
							+ symbolData.symbol + " with SelectedFeedPrice : " + symbolData.selectedfeedPX);
				}
			}
			symbolData = null;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}