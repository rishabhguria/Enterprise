package prana.esperCalculator.cacheClasses;

import java.util.HashMap;
import java.util.LinkedHashMap;

import prana.utility.logging.PranaLogManager;

/**
 * Use of dual cache handle slower data consumption
 * 
 * @author dewashish
 * 
 */
public class TaxlotDualCache {

	/**
	 * Singleton pattern implemented
	 */
	private static TaxlotDualCache _taxlotDualCache;

	/**
	 * Private constructor to implement dual cache
	 */
	private TaxlotDualCache() {
	}

	/**
	 * Returns singleton instance of dual cache
	 * 
	 * @return singleton instance of dual cache
	 */
	public static TaxlotDualCache getInstance() {
		if (_taxlotDualCache == null)
			_taxlotDualCache = new TaxlotDualCache();
		return _taxlotDualCache;
	}

	/**
	 * Reference of input cache. In this cache data received from producer is filled
	 */
	private HashMap<String, HashMap<String, Object>> _intermediateTaxlotCacheInput = new HashMap<>();

	/**
	 * Reference of Output cache. This reference is used to send data to esper
	 */
	private HashMap<String, HashMap<String, Object>> _intermediateTaxlotCacheOutput = new HashMap<>();

	/**
	 * Single locker object used when switching cache or filling the input cache. As
	 * output cache is used by only one thread(Event sender thread), while input
	 * cache is used by both thread
	 */
	private Object _lockerObject = new Object();

	/**
	 * This method added the received symbol data to input cache
	 * 
	 * @param taxlotId
	 *            Key for input cache is symbol
	 * @param map
	 *            Value of symbol data
	 */
	public void addToCache(String taxlotId, LinkedHashMap<String, Object> map) {

		try {
			synchronized (_lockerObject) {
				_intermediateTaxlotCacheInput.put(taxlotId, map);
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
				if (_intermediateTaxlotCacheInput != null && !_intermediateTaxlotCacheInput.isEmpty()) {

					// switching cache
					HashMap<String, HashMap<String, Object>> interReference = _intermediateTaxlotCacheInput;
					_intermediateTaxlotCacheInput = _intermediateTaxlotCacheOutput;
					_intermediateTaxlotCacheOutput = interReference;
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
			return _intermediateTaxlotCacheOutput;
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
				_intermediateTaxlotCacheOutput.clear();
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}