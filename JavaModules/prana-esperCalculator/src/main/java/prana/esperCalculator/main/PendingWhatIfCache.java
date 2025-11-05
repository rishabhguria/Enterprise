package prana.esperCalculator.main;

import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import prana.amqpAdapter.AmqpHelper;
import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.esperCalculator.cacheClasses.WhatIfCacheObject;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.esperCEP.WhatIfHelper;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

/**
 * Stores all the Orders/Baskets till they have been validated by compliance
 * 
 * @author abhinav.pandey
 * 
 */
public class PendingWhatIfCache {

	/**
	 * Singleton pattern implemented
	 */
	private static PendingWhatIfCache _pendingWhatIfCacheSingiltonObject;
	private long _startTime;
	private static boolean _isValidatedSymbolDataReceived;

	/**
	 * Private constructor to implement dual cache
	 */
	private PendingWhatIfCache() {
		try {
			_pendingWhatIfCache = new HashMap<String, WhatIfCacheObject>();

			// Initializing AmqpSender for cancellation
			String otherDataExchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST, ConfigurationConstants.KEY_EXCHANGE_OTHERDATA);
			_amqpCancelMessageSender = AmqpHelper.getSender(otherDataExchangeName, ExchangeType.Direct,
					MediaType.Exchange, false);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/*
	 * Return true, If Pending WhatIf cache is empty
	 */
	public boolean isEmpty() {
		int size = 0;
		try {
			synchronized (_lockerObject) {
				size = _pendingWhatIfCache.size();
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
		}
		return size == 0;
	}

	/**
	 * Returns singleton instance of the cache
	 * 
	 * @return singleton instance of the cache
	 */
	public static PendingWhatIfCache getInstance() {
		if (_pendingWhatIfCacheSingiltonObject == null)
			_pendingWhatIfCacheSingiltonObject = new PendingWhatIfCache();
		return _pendingWhatIfCacheSingiltonObject;
	}

	/**
	 * Reference of the single cache object <BasketId, Order>
	 */
	private HashMap<String, WhatIfCacheObject> _pendingWhatIfCache;

	/**
	 * The object locker
	 */
	private Object _lockerObject = new Object();

	/**
	 * This amqp sender is used to send cancel message to trade server (currently in
	 * case if live feed is disconnected)
	 */
	private IAmqpSender _amqpCancelMessageSender;

	/**
	 * Add the tax lot to the cache
	 * 
	 * @param basketid
	 * @param taxlot
	 */
	public void addWhatIfOrder(HashMap<String, Object> taxlot) {
		try {
			String basketid = taxlot.get("GroupID").toString();
			synchronized (_lockerObject) {
				if (_pendingWhatIfCache.containsKey(basketid))
					_pendingWhatIfCache.get(basketid).AddTrade(taxlot);
				else {
					_pendingWhatIfCache.put(basketid, new WhatIfCacheObject(basketid));
					_pendingWhatIfCache.get(basketid).AddTrade(taxlot);
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * 
	 * @param basketId
	 * @param count
	 * @return
	 * @throws Exception
	 */
	public boolean eomReceived(String basketId, int count) throws Exception {
		try {
			return _pendingWhatIfCache.get(basketId).eomReceived(count);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}
	}

	/**
	 * Get the symbols in the basket
	 * 
	 * @param basketId
	 * @return
	 */
	public List<String> getRequiredSymbols(String basketId) {
		try {
			return _pendingWhatIfCache.get(basketId).getRequiredSymbols();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}
	}

	/**
	 * Inform the basket that pricing will not be required for the symbol
	 * 
	 * @param basketId
	 * @param symbol
	 */
	public void pricingNotRequiredForSymbol(String basketId, String symbol) {
		try {
			_pendingWhatIfCache.get(basketId).symbolDataNotRequired(symbol);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * 
	 * @param symbol
	 * @return orders that have got all required pricing
	 */
	public List<String> symbolDataReceived(String symbol) {
		synchronized (_lockerObject) {
			try {
				List<String> basketIds = new ArrayList<String>();
				for (String basketid : _pendingWhatIfCache.keySet()) {
					if (_pendingWhatIfCache.get(basketid).symbolDataReceived(symbol))
						basketIds.add(basketid);
				}
				return basketIds;
			} catch (Exception ex) {
				PranaLogManager.error(ex);
				return null;
			}
		}
	}

	public HashMap<String, WhatIfCacheObject> getBasketsForSymbols(String symbol) {
		synchronized (_lockerObject) {
			try {
				HashMap<String, WhatIfCacheObject> baskets = new HashMap<String, WhatIfCacheObject>();
				for (String basketid : _pendingWhatIfCache.keySet()) {
					if (_pendingWhatIfCache.get(basketid).symbolDataReceived(symbol)) {
						baskets.put(basketid, _pendingWhatIfCache.get(basketid));
					}
				}
				return baskets;
			} catch (Exception ex) {
				PranaLogManager.error(ex);
				return null;
			}
		}
	}

	/**
	 * Returns Basket for the which all symbols data is received.
	 */
	public void removeBasketsFromCollectionForSymbols(HashMap<String, WhatIfCacheObject> baskets) {
		synchronized (_lockerObject) {
			try {
				for (String basketid : baskets.keySet()) {
					_pendingWhatIfCache.remove(basketid);
				}
			} catch (Exception ex) {
				PranaLogManager.error(ex);
			}
		}
	}

	/**
	 * Gets all tax lots in the given basket
	 * 
	 * @param basketId
	 * @return
	 */
	public List<HashMap<String, Object>> getTaxlotsinBasket(String basketId) {
		try {
			return _pendingWhatIfCache.get(basketId).getTaxlots();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}
	}
	
	/**
	 * Gets all sorted taxlots in the given basket
	 * @param basketId
	 * @return
	 */
	public HashMap<Integer, List<HashMap<String, Object>>> getSortedTaxlotsinBasket(String basketId) {
		try {
			List<HashMap<String, Object>> taxlots = getTaxlotsinBasket(basketId);
			HashMap<Integer, List<HashMap<String, Object>>> sortedTaxlots = new HashMap<Integer, List<HashMap<String, Object>>>();
			for (HashMap<String, Object> taxlot : taxlots) {
				int key = Integer.parseInt(taxlot.get("SideRank").toString());
				if (sortedTaxlots.containsKey(key))
					sortedTaxlots.get(key).add(taxlot);
				else {
					List<HashMap<String, Object>> ls = new ArrayList<HashMap<String, Object>>();
					ls.add(taxlot);
					sortedTaxlots.put(key, ls);
				}
			}
			return sortedTaxlots;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}
	}

	/**
	 * Remove the basket/order from the cache
	 * 
	 * @param basketId
	 */
	public boolean checkIfExistsInCache(String basketId) {
		try {
			synchronized (_lockerObject) {
				if (_pendingWhatIfCache.containsKey(basketId)) {
					return true;
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return false;
	}

	public void removeFromCache(String basketId) {
		try {
			synchronized (_lockerObject) {
				if (_pendingWhatIfCache.containsKey(basketId)) {
					_pendingWhatIfCache.remove(basketId);
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * GEts the orders that are still in the cache
	 * 
	 * @return
	 */
	public List<String> getPendingOrders() {
		try {
			List<String> basketIds = new ArrayList<String>();
			for (String basketid : _pendingWhatIfCache.keySet()) {
				basketIds.add(basketid);
			}
			return basketIds;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}
	}

	/**
	 * Cleat the pending trade cache
	 */
	public void clear() {
		try {
			_pendingWhatIfCache.clear();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public void startTimer() {
		try {
			_startTime = System.currentTimeMillis();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public long getStartTime() {
		try {
			return _startTime;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return 0;
		}
	}

	/**
	 * Gets the userid for the current basket
	 * 
	 * @param basketid
	 * @return
	 */
	public int getUserId(String basketid) {
		try {
			return Integer
					.parseInt(_pendingWhatIfCache.get(basketid).getTaxlots().get(0).get("CompanyUserID").toString());
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return -1;
		}
	}

	/**
	 * Check if the basket id is still in the cache
	 * 
	 * @param basketId
	 * @return
	 */
	public boolean contains(String basketId) {
		return _pendingWhatIfCache.containsKey(basketId);
	}

	/**
	 * Cancels the order/basket. Does nothing if the order is already processed.
	 * 
	 * @param basketId
	 */
	public void cancelBasket(String basketId) {
		try {
			synchronized (_lockerObject) {
				if (!_pendingWhatIfCache.containsKey(basketId))
					return;
				else if (_pendingWhatIfCache.containsKey(basketId)
						&& _pendingWhatIfCache.get(basketId).getIsPricesReceived()) {
					// The basket is still not processed
					WhatIfHelper.sendCancellationMessage(_amqpCancelMessageSender, basketId, getUserId(basketId),
							"Taking Longer Time In Calculation",
							"The trade(s) could not be validated because its calculation is taking longer time.");

					// Send an EOM to server after live feed disconnected alert
					WhatIfHelper.sendEomMessage(_amqpCancelMessageSender, basketId,
							PendingWhatIfCache.getInstance().getUserId(basketId));

					_pendingWhatIfCache.remove(basketId);

					PranaLogManager.info("Cancelled message because its calculation is taking longer time");
				} else {
					// The basket is still not processed
					WhatIfHelper.sendCancellationMessage(_amqpCancelMessageSender, basketId, getUserId(basketId),
							"Price not available",
							"The trade(s) could not be validated as the prices are not available.");

					// Send an EOM to server after live feed disconnected alert
					WhatIfHelper.sendEomMessage(_amqpCancelMessageSender, basketId,
							PendingWhatIfCache.getInstance().getUserId(basketId));

					_pendingWhatIfCache.remove(basketId);
					PranaLogManager.info("Cancelled message because prices are not available");
				}
			}
		} catch (IOException e) {
			PranaLogManager.error(e);
		}
	}
	
	/**
	 * Sets isValidatedSymbolDataReceived
	 * 
	 * @param isValidatedSymbolDataReceived
	 */
	public static boolean setIsValidatedSymbolDataReceived(boolean isValidatedSymbolDataReceived) {
		try {
			return _isValidatedSymbolDataReceived = isValidatedSymbolDataReceived;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}
	}
	
	/**
	 * Gets isValidatedSymbolDataReceived
	 * 
	 * @return
	 */
	public static boolean getIsValidatedSymbolDataReceived() {
		try {
			return _isValidatedSymbolDataReceived;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}
	}
}