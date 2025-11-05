package prana.basketComplianceService.main;

import java.math.BigDecimal;
import java.util.HashMap;
import prana.utility.logging.PranaLogManager;
import prana.esperCalculator.constants.CollectorConstants;
import prana.esperCalculator.objects.SymbolData;

/**
 * Use of dual cache handle slower data consumption
 * 
 * @author Ankit Jain
 * 
 */
public class SymbolDataCache {

	/**
	 * Singleton pattern implemented
	 */
	private static SymbolDataCache _symbolDataCache;

	/**
	 * Private constructor to implement dual cache
	 */
	private SymbolDataCache() {
	}

	/**
	 * Returns singleton instance of dual cache
	 * 
	 * @return singleton instance of dual cache
	 */
	public static SymbolDataCache getInstance() {
		if (_symbolDataCache == null)
			_symbolDataCache = new SymbolDataCache();
		return _symbolDataCache;
	}

	/**
	 * Reference of input cache. In this cache data received from producer is filled
	 */
	private HashMap<String, SymbolData> _intermediateSymbolDataCache = new HashMap<String, SymbolData>();

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
	public void addOrUpdateToCache(HashMap<String, Object> symbolData) {

		try {
			if (symbolData != null) {
				SymbolData symbolDataObject = new SymbolData();

				symbolDataObject.symbol = symbolData.get("symbol").toString();
				symbolDataObject.underlyingSymbol = symbolData.get("underlyingSymbol").toString();
				symbolDataObject.ask = Double.parseDouble(symbolData.get("askPrice").toString());
				symbolDataObject.bid = Double.parseDouble(symbolData.get("bidPrice").toString());
				symbolDataObject.low = Double.parseDouble(symbolData.get("lowPrice").toString());
				symbolDataObject.high = Double.parseDouble(symbolData.get("highPrice").toString());
				symbolDataObject.open = Double.parseDouble(symbolData.get("openPrice").toString());
				symbolDataObject.closingPrice = Double.parseDouble(symbolData.get("closePrice").toString());
				symbolDataObject.lastPrice = symbolData.get("lastPrice") != null ? Double.parseDouble(symbolData.get("lastPrice").toString()) : 0.0;
				symbolDataObject.selectedfeedPX = Double.parseDouble(symbolData.get("selectedFeedPrice").toString());
				symbolDataObject.conversionMethod = symbolData.get("conversionMethod").toString();
				symbolDataObject.markPrice = symbolData.get("markPrice") !=null ? Double.parseDouble(symbolData.get("markPrice").toString()) : 0.0;
				symbolDataObject.delta = Double.parseDouble(symbolData.get("delta").toString());
				symbolDataObject.beta5yr = Double.parseDouble(symbolData.get("beta5YearMonthly").toString());
				symbolDataObject.categoryCode = Integer.parseInt(symbolData.get("assetId").toString());
				symbolDataObject.openInterest = Double.parseDouble(symbolData.get("openInterest").toString());
				symbolDataObject.avgVol20Days = Double.parseDouble(symbolData.get("avgVolume20Days").toString());
				symbolDataObject.sharesOutStandings = BigDecimal
						.valueOf(((Number) symbolData.get("sharesOutstanding")).doubleValue());
				synchronized (_lockerObject) {
					_intermediateSymbolDataCache.put(symbolDataObject.symbol, symbolDataObject);					
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Getting SymbolData for the symbol
	 * 
	 * @return Output cache
	 */
	public SymbolData getSymbolDataForSymbol(String symbol) {
		try {
			if (_intermediateSymbolDataCache.containsKey(symbol))
				return _intermediateSymbolDataCache.get(symbol);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return null;
	}
}
