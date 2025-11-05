package prana.esperCalculator.cacheClasses;

import java.util.HashMap;

import prana.utility.logging.PranaLogManager;

/**
 * Use of dual cache handle slower data consumption
 * 
 * @author dewashish
 * 
 */
public class MarketDataDualCache {

	/**
	 * Singleton pattern implemented
	 */
	private static MarketDataDualCache _inMarketDualCache;

	/**
	 * Private constructor to implement dual cache
	 */
	private MarketDataDualCache() {
	}

	/**
	 * Returns singleton instance of dual cache
	 * 
	 * @return singleton instance of dual cache
	 */
	public static MarketDataDualCache getInstance() {
		if (_inMarketDualCache == null)
			_inMarketDualCache = new MarketDataDualCache();
		return _inMarketDualCache;
	}

	/**
	 * Reference of input cache. In this cache data received from producer is filled
	 */
	private HashMap<String, HashMap<String, Object>> _intermediateInMarketCacheInput = new HashMap<>();

	/**
	 * Reference of Output cache. This reference is used to send data to esper
	 */
	private HashMap<String, HashMap<String, Object>> _intermediateInMarketCacheOutput = new HashMap<>();

	/**
	 * Single locker object used when switching cache or filling the input cache. As
	 * output cache is used by only one thread(Event sender thread), while input
	 * cache is used by both thread
	 */
	private Object _lockerObject = new Object();

	/**
	 * This method added the received symboldata to input cache
	 * 
	 * @param taxlotId
	 *            Key for input cache is symbol
	 * @param map
	 *            Value of symboldata
	 */
	public void addToCache(String taxlotId, HashMap<String, Object> map) {
		try {
			synchronized (_lockerObject) {
				if (_intermediateInMarketCacheInput.containsKey(taxlotId)) {
					_intermediateInMarketCacheInput.put(taxlotId, map);
				} else {
					_intermediateInMarketCacheInput.put(taxlotId, map);
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
				if (_intermediateInMarketCacheInput != null && !_intermediateInMarketCacheInput.isEmpty()) {
					// switching cache
					HashMap<String, HashMap<String, Object>> interReference = _intermediateInMarketCacheInput;
					_intermediateInMarketCacheInput = _intermediateInMarketCacheOutput;
					_intermediateInMarketCacheOutput = interReference;
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
	public HashMap<String, HashMap<String, Object>> getLatestTaxlotCache() {
		try {
			clearOutputCache();
			switchCache();
			return _intermediateInMarketCacheOutput;
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
				_intermediateInMarketCacheOutput.clear();
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}
