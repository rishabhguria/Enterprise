package prana.esperCalculator.cacheClasses;

import java.math.BigDecimal;
import java.util.Arrays;
import java.util.HashMap;

import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;
import prana.esperCalculator.constants.CollectorConstants;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.esperUDF.Misc;
import prana.esperCalculator.main.WhatIfManager;
import prana.esperCalculator.objects.SymbolData;

/**
 * Use of dual cache handle slower data consumption
 * 
 * @author dewashish
 * 
 */
public class SymbolDataDualCache {

	/**
	 * Singleton pattern implemented
	 */
	private static SymbolDataDualCache _symbolDataDualCache;

	private static boolean _isLiveMode = false;

	/**
	 * Private constructor to implement dual cache
	 */
	private SymbolDataDualCache() {
		try {
			_isLiveMode = Boolean.parseBoolean(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_IS_LIVE_MODE));
			
			PranaLogManager.logOnly("_isLiveMode:" + _isLiveMode);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Returns singleton instance of dual cache
	 * 
	 * @return singleton instance of dual cache
	 */
	public static SymbolDataDualCache getInstance() {
		if (_symbolDataDualCache == null)
			_symbolDataDualCache = new SymbolDataDualCache();
		return _symbolDataDualCache;
	}

	/**
	 * Reference of input cache. In this cache data received from producer is filled
	 */
	private HashMap<String, SymbolData> _intermediateSymbolDataCacheInput = new HashMap<String, SymbolData>();

	/**
	 * Reference of Output cache. This reference is used to send data to ESPER
	 */
	private HashMap<String, SymbolData> _intermediateSymbolDataCacheOutput = new HashMap<String, SymbolData>();
	
	/**
	 * Holds the most-recent SymbolData we accepted for every symbol.
	 * Used to detect whether the new tick really changed.
	 */
	private final HashMap<String, SymbolData> _lastSymbolDataCache = new HashMap<>();

	/**
	 * Single locker object used when switching cache or filling the input cache. As
	 * output cache is used by only one thread(Event sender thread), while input
	 * cache is used by both thread
	 */
	private Object _lockerObject = new Object();

	/**
	 * This method added the received symbol data to input cache
	 * 
	 * @param symbol
	 *            Key for input cache is symbol
	 * @param map
	 *            Value of symbol data
	 */
	public void addToCache(String symbol, HashMap<String, Object> map) {

		try {
			if (map != null && map.containsKey(CollectorConstants.BETA_5_YR)) {
				SymbolData symbolData = new SymbolData();

				if (map.containsKey(CollectorConstants.DELTA)) {
					symbolData.delta = Double.valueOf(map.get(CollectorConstants.DELTA).toString());
					if (symbolData.delta == 0)
						symbolData.delta = 1;
				}

				if (map.containsKey(CollectorConstants.CONVERSION_METHOD)) {
					int methodId = Integer.parseInt(map.get(CollectorConstants.CONVERSION_METHOD).toString());
					if (methodId == 1)
						symbolData.conversionMethod = CollectorConstants.CONVERSION_METHOD_OPERATOR_DIVIDE;
					else
						symbolData.conversionMethod = CollectorConstants.CONVERSION_METHOD_OPERATOR_MULTIPLY;
				}

				symbolData.beta5yr = Double.valueOf(map.get(CollectorConstants.BETA_5_YR).toString());

				symbolData.selectedfeedPX = (double) map.get(CollectorConstants.SELECTED_FEED_PRICE);
				symbolData.ask = (double) map.get(CollectorConstants.ASK);
				symbolData.bid = (double) map.get(CollectorConstants.BID);
				symbolData.low = (double) map.get(CollectorConstants.LOW);
				symbolData.high = (double) map.get(CollectorConstants.HIGH);
				symbolData.open = (double) map.get(CollectorConstants.OPEN);
				symbolData.lastPrice = map.get(CollectorConstants.LAST_PRICE_SYMBOLDATA);
				symbolData.markPrice = map.get(CollectorConstants.MARK_PRICE);
				symbolData.openInterest = (double) map.get(CollectorConstants.OPEN_INTEREST);
				symbolData.avgVol20Days = (double) map.get(CollectorConstants.AVERAGE_VOLUME_20_DAY);

				// First converting SharesOutStanding object value to double and then getting
				// the BigDecimal value. 
				// https://jira.nirvanasolutions.com:8443/browse/PRANA-37833
				symbolData.sharesOutStandings = BigDecimal
						.valueOf(((Number) map.get(CollectorConstants.SHARES_OUTSTANDING)).doubleValue());
				
				symbolData.symbol = map.get(CollectorConstants.SYMBOL).toString();
				symbolData.underlyingSymbol = map.get(CollectorConstants.UNDERLYING_SYMBOL).toString();
				symbolData.categoryCode = Integer.parseInt(map.get(CollectorConstants.CATEGORY_CODE).toString());
				symbolData.pricingStatus = Integer.parseInt(map.get(CollectorConstants.PRICING_STATUS).toString());
				int marketDataProvider = Integer.parseInt(map.get(CollectorConstants.MARKET_DATA_PROVIDER).toString());
				String asset = Misc.getAsset(symbolData.categoryCode);
				
				synchronized (_lockerObject) {
					// Check if the asset is not empty and requires pricing
					boolean requiresPricing = !asset.isEmpty() && Arrays.asList(WhatIfManager.getInstance().assetsRequiringPricing).contains(asset);

					if (requiresPricing) {
					    // For live mode, check non-zero values for selectedfeedPX, ask, or bid
						if (_isLiveMode && (symbolData.selectedfeedPX != 0.0 || symbolData.ask != 0.0 || symbolData.bid != 0.0 || (symbolData.pricingStatus != 0 && marketDataProvider == 1))) {
					    	SymbolData previous = _lastSymbolDataCache.get(symbol);
					        if (hasDataChanged(previous, symbolData)) {
					            _intermediateSymbolDataCacheInput.put(symbol, symbolData);
					            _lastSymbolDataCache.put(symbol, symbolData);
					        }
					    }
					    // For non-live mode, add to cache without checking selectedfeedPX, ask, or bid values
					    else if (!_isLiveMode) {
					        _intermediateSymbolDataCacheInput.put(symbol, symbolData);
					    }
					}
				}
			}

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * This method simply switches the cache (by reference)
	 */
	private void switchCache() {
		try {
			synchronized (_lockerObject) {
				if (_intermediateSymbolDataCacheInput != null && !_intermediateSymbolDataCacheInput.isEmpty()) {

					// switching cache
					HashMap<String, SymbolData> interReference = _intermediateSymbolDataCacheInput;
					_intermediateSymbolDataCacheInput = _intermediateSymbolDataCacheOutput;
					_intermediateSymbolDataCacheOutput = interReference;
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * This is wrapper for output cache
	 * 
	 * @return Output cache
	 */
	public HashMap<String, SymbolData> getLatestSymbolDataCache() {
		try {
			clearOutputCache();
			switchCache();
			return _intermediateSymbolDataCacheOutput;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}
	}

	/**
	 * Before switching this method clears the output cache
	 */
	private void clearOutputCache() {
		try {
			synchronized (_lockerObject) {
				_intermediateSymbolDataCacheOutput.clear();
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
	
	/**
	 * Compares the fields that matter for pricing and returns true only
	 * when at least one of them is different.
	 */
	private boolean hasDataChanged(SymbolData oldD, SymbolData newD) {

	    if (oldD == null) return true;            // first time for this symbol

	    // doubles
	    if (oldD.selectedfeedPX != newD.selectedfeedPX) return true;
	    if (oldD.ask            != newD.ask)            return true;
	    if (oldD.bid            != newD.bid)            return true;
	    if (oldD.low            != newD.low)            return true;
	    if (oldD.high           != newD.high)           return true;
	    if (oldD.open           != newD.open)           return true;
	    if (oldD.openInterest   != newD.openInterest)   return true;
	    if (oldD.avgVol20Days   != newD.avgVol20Days)   return true;
	    if (oldD.beta5yr        != newD.beta5yr)        return true;
	    if (oldD.delta          != newD.delta)          return true;
	    if (oldD.pricingStatus  != newD.pricingStatus)  return true;

	    // objects (handle nulls defensively)
	    if (!java.util.Objects.equals(oldD.lastPrice,  newD.lastPrice))  return true;
	    if (!java.util.Objects.equals(oldD.markPrice,  newD.markPrice))  return true;
	    if (!java.util.Objects.equals(oldD.sharesOutStandings, newD.sharesOutStandings)) return true;
	    if (!java.util.Objects.equals(oldD.conversionMethod,  newD.conversionMethod))    return true;

	    return false;   // no differences detected
	}
}
