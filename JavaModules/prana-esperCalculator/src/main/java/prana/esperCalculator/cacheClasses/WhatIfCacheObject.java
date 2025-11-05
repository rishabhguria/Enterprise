package prana.esperCalculator.cacheClasses;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import prana.utility.logging.PranaLogManager;

/**
 * Stores the basket order
 * 
 * @author abhinav.pandey
 * 
 */
public class WhatIfCacheObject {

	/**
	 * The basket it of the trade
	 */
	private String _basketId;

	private boolean _isPriceReceived;

	/**
	 * Collection of all the <taxlotIds-taxlots>, taxlot will be the smallest unit
	 * of the trade
	 */
	private HashMap<String, HashMap<String, Object>> _whatIfTradeCollection;

	/**
	 * Whether the EOM has been received from the trade server, Signifies that all
	 * the tax lots have been received
	 */
	private boolean _isEomReceived;

	/**
	 * The collection of symbols whose pricing will be required, The boolean value
	 * signifies whether the pricing has been received or not
	 */
	private HashMap<String, Boolean> _requiredSymbolCollection;

	/**
	 * Constructor
	 * 
	 * @param basketId
	 */
	public WhatIfCacheObject(String basketId) {
		this._basketId = basketId;
		_isEomReceived = false;
		_whatIfTradeCollection = new HashMap<String, HashMap<String, Object>>();
		_requiredSymbolCollection = new HashMap<String, Boolean>();
	}

	/**
	 * Add a tax lot to the cache and update the cache
	 * 
	 * @param taxlot
	 */
	public void AddTrade(HashMap<String, Object> taxlot) {
		try {
			String taxlotId = taxlot.get("TaxLotID").toString();
			if (_whatIfTradeCollection.containsKey(taxlotId)) {
				_whatIfTradeCollection.get(taxlotId).put(taxlotId, taxlot);
			} else {
				_whatIfTradeCollection.put(taxlotId, taxlot);
			}

			String symbol = taxlot.get("Symbol").toString();
			if (!_requiredSymbolCollection.containsKey(symbol))
				_requiredSymbolCollection.put(symbol, false);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Get all taxlots in the current basket
	 * 
	 * @return
	 */
	public List<HashMap<String, Object>> getTaxlots() {
		try {
			List<HashMap<String, Object>> taxlots = new ArrayList<HashMap<String, Object>>();
			for (String taxlotId : _whatIfTradeCollection.keySet())
				taxlots.add(_whatIfTradeCollection.get(taxlotId));
			return taxlots;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}
	}

	/**
	 * Informs that the eom is received
	 * 
	 * @param tradeCount
	 * @return the list of symbol that are their in the order (for pricing
	 *         validation)
	 * @throws Exception
	 */
	public boolean eomReceived(int tradeCount) throws Exception {

		try {
			if (tradeCount != _whatIfTradeCollection.size())
				throw new Exception("Eom was received before the trades. whatIfCache count: "+ _whatIfTradeCollection.size() + ", Trade count: "+ tradeCount);
			_isEomReceived = true;
			return true;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}
	}

	/**
	 * Updates the symbol data cache and checks if pricing for all the symbols, has
	 * been received
	 * 
	 * @param symbol
	 * @return AllSymbolReceived
	 */
	public boolean symbolDataReceived(String symbol) {

		try {
			if (!_isEomReceived)
				return false;

			if (_requiredSymbolCollection.containsKey(symbol))
				_requiredSymbolCollection.put(symbol, true);

			for (String key : _requiredSymbolCollection.keySet()) {
				if (!_requiredSymbolCollection.get(key))
					return false;
			}
			return true;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}
	}

	/**
	 * Get the symbols in the order for fetching pricing
	 * 
	 * @return
	 */
	public List<String> getRequiredSymbols() {
		try {
			List<String> symbols = new ArrayList<String>();
			for (String key : _requiredSymbolCollection.keySet())
				symbols.add(key);
			return symbols;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}
	}

	/**
	 * The pricing for the symbol is not required
	 * 
	 * @param symbol
	 */
	public void symbolDataNotRequired(String symbol) {
		try {
			if (_requiredSymbolCollection.containsKey(symbol))
				_requiredSymbolCollection.remove(symbol);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Returns the basket id of the current object
	 * 
	 * @return
	 */
	public String getBasketId() {
		try {
			return _basketId;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}
	}

	public boolean setIsPricesReceived(boolean isPriceReceived) {
		try {
			return _isPriceReceived = isPriceReceived;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}
	}

	public boolean getIsPricesReceived() {
		try {
			return _isPriceReceived;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}
	}
}
