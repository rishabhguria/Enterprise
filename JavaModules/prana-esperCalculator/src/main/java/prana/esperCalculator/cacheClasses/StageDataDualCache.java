package prana.esperCalculator.cacheClasses;

import java.util.HashMap;

import prana.utility.logging.PranaLogManager;

/**
 * Use of dual cache handle slower data consumption
 * 
 * @author dewashish
 * 
 */
public class StageDataDualCache {

	/**
	 * Singleton pattern implemented
	 */
	private static StageDataDualCache _inStageDualCache;

	/**
	 * Private constructor to implement dual cache
	 */
	private StageDataDualCache() {
	}

	/**
	 * Returns singleton instance of dual cache
	 * 
	 * @return singleton instance of dual cache
	 */
	public static StageDataDualCache getInstance() {
		if (_inStageDualCache == null)
			_inStageDualCache = new StageDataDualCache();
		return _inStageDualCache;
	}

	/**
	 * Reference of input cache. In this cache data received from producer is filled
	 */
	private HashMap<String, HashMap<String, Object>> _intermediateInStageCacheInput = new HashMap<>();

	/**
	 * Reference of Output cache. This reference is used to send data to ESPER
	 */
	private HashMap<String, HashMap<String, Object>> _intermediateInStageCacheOutput = new HashMap<>();

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
	public void addToCache(String taxlotId, HashMap<String, Object> map) {
		try {
			synchronized (_lockerObject) {
				if (_intermediateInStageCacheInput.containsKey(taxlotId)) {
					_intermediateInStageCacheInput.put(taxlotId, map);
				} else {
					_intermediateInStageCacheInput.put(taxlotId, map);
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
				if (_intermediateInStageCacheInput != null && !_intermediateInStageCacheInput.isEmpty()) {

					// switching cache
					HashMap<String, HashMap<String, Object>> interReference = _intermediateInStageCacheInput;
					_intermediateInStageCacheInput = _intermediateInStageCacheOutput;
					_intermediateInStageCacheOutput = interReference;
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
			return _intermediateInStageCacheOutput;
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
				_intermediateInStageCacheOutput.clear();
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}