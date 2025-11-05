package prana.basketComplianceService.main;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;

import prana.esperCalculator.commonCode.CEPManager;
import prana.esperCalculator.commonCode.TaxlotManager;
import prana.esperCalculator.commonCode.TaxlotManager.TaxlotState;
import prana.esperCalculator.commonCode.TaxlotManager.TaxlotType;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.objects.Taxlot;
import prana.utility.logging.PranaLogManager;

/**
 * Use of dual cache handle slower data consumption
 * 
 * @author Ankit Jain
 * 
 */
public class TaxlotCache {

	/**
	 * Singleton pattern implemented
	 */
	private static TaxlotCache _taxlotCache;

	/**
	 * Private constructor to implement dual cache
	 */
	private TaxlotCache() {
	}

	/**
	 * Returns singleton instance of dual cache
	 * 
	 * @return singleton instance of dual cache
	 */
	public static TaxlotCache getInstance() {
		if (_taxlotCache == null)
			_taxlotCache = new TaxlotCache();
		return _taxlotCache;
	}

	/**
	 * Reference of input cache. In this cache data received from producer is filled
	 */
	private HashMap<String, HashMap<String, Object>> _intermediateTaxlotCacheInput = new HashMap<>();

	/*
	 * masterFundAccountList
	 */
	public HashMap<Integer, List<Integer>> masterFundAccountList = new HashMap<>();

	/**
	 * Single locker object used when switching cache or filling the input cache. As
	 * output cache is used by only one thread(Event sender thread), while input
	 * cache is used by both thread
	 */
	private Object _lockerObject = new Object();
	
	SimpleDateFormat parserSDF = new SimpleDateFormat(ConfigurationConstants.SIMPLE_DATE_FORMAT);

	/**
	 * 
	 * @param taxlot
	 */
	public void addOrUpdateToMasterFundCache(int masterFundId, int accountId) {
		try {
			List<Integer> accountIds = new ArrayList<Integer>();
			synchronized (_lockerObject) {
				if (masterFundAccountList.containsKey(masterFundId)) {
					accountIds = masterFundAccountList.get(masterFundId);
				}
				if (!accountIds.contains(accountId))
					accountIds.add(accountId);
				masterFundAccountList.put(masterFundId, accountIds);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * This method added the received symbol data to input cache
	 * 
	 * @param taxlotId + taxlotType
	 *            Key for input cache is symbol
	 * @param map
	 *            Value of symbol data
	 */
	public void addOrUpdateToCache(HashMap<String, Object> taxlot) {

		try {
			synchronized (_lockerObject) {
				String key = taxlot.get("taxlotId").toString() + taxlot.get("taxlotType").toString();
				if(!_intermediateTaxlotCacheInput.containsKey(key))
					sendTaxlotEvent(taxlot);
				_intermediateTaxlotCacheInput.put(key, taxlot);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void sendTaxlotEvent(HashMap<String, Object> taxlot) {
		try {
			Taxlot taxlotDto = new Taxlot();
			taxlotDto.basketId = TaxlotManager.getStringSafe(taxlot.get("basketId"), "basketId");
			taxlotDto.taxlotId = TaxlotManager.getStringSafe(taxlot.get("taxlotId"), "taxlotId");
			taxlotDto.clOrderId = TaxlotManager.getStringSafe(taxlot.get("clOrderId"), "clOrderId");
			taxlotDto.taxlotType = TaxlotType.valueOf(TaxlotManager.getStringSafe(taxlot.get("taxlotType"), "taxlotType"));
			taxlotDto.taxlotState = TaxlotState.valueOf(TaxlotManager.getStringSafe(taxlot.get("taxlotState"), "taxlotState"));
			taxlotDto.symbol = TaxlotManager.getStringSafe(taxlot.get("symbol"), "symbol");
			taxlotDto.underlyingSymbol = TaxlotManager.getStringSafe(taxlot.get("underlyingSymbol"),
					"underlyingSymbol");
			taxlotDto.quantity = TaxlotManager.getDoubleSafe(taxlot.get("quantity"), "quantity");
			taxlotDto.avgPrice = TaxlotManager.getDoubleSafe(taxlot.get("avgPrice"), "avgPrice");
			taxlotDto.orderSideTagValue = TaxlotManager.getStringSafe(taxlot.get("orderSideTagValue"),
					"orderSideTagValue");
			taxlotDto.accountId = TaxlotManager.getIntSafe(taxlot.get("accountId"), "accountId");
			taxlotDto.counterPartyId = TaxlotManager.getIntSafe(taxlot.get("counterPartyId"), "counterPartyId");
			taxlotDto.venueId = TaxlotManager.getIntSafe(taxlot.get("venueId"), "venueId");
			taxlotDto.auecLocalDate = TaxlotManager.getDateSafe(taxlot.get("auecLocalDate"), "auecLocalDate", parserSDF);
			taxlotDto.settlementDate = TaxlotManager.getDateSafe(taxlot.get("settlementDate"), "settlementDate", parserSDF);
			taxlotDto.userId = TaxlotManager.getStringSafe(taxlot.get("userId"), "userId");
			taxlotDto.strategyId = TaxlotManager.getIntSafe(taxlot.get("strategyId"), "strategyId");
			taxlotDto.orderTypeTagValue = TaxlotManager.getStringSafe(taxlot.get("orderTypeTagValue"),
					"orderTypeTagValue");
			taxlotDto.benchMarkRate = TaxlotManager.getDoubleSafe(taxlot.get("benchMarkRate"), "benchMarkRate");
			taxlotDto.differential = TaxlotManager.getDoubleSafe(taxlot.get("differential"), "differential");
			taxlotDto.swapNotional = TaxlotManager.getDoubleSafe(taxlot.get("swapNotional"), "swapNotional");
			taxlotDto.dayCount = TaxlotManager.getDoubleSafe(taxlot.get("dayCount"), "dayCount");
			taxlotDto.isSwapped = TaxlotManager.getBooleanSafe(taxlot.get("isSwapped"), "isSwapped");
			taxlotDto.avgFxRateForTrade = TaxlotManager.getDoubleSafe(taxlot.get("avgFxRateForTrade"),
					"avgFxRateForTrade");
			taxlotDto.conversionMethodOperator = TaxlotManager.getStringSafe(taxlot.get("conversionMethodOperator"),
					"conversionMethodOperator");
			taxlotDto.tif = TaxlotManager.getStringSafe(taxlot.get("tif"), "tif");
			taxlotDto.orderSide = TaxlotManager.getStringSafe(taxlot.get("orderSide"), "orderSide");
			taxlotDto.counterParty = TaxlotManager.getStringSafe(taxlot.get("counterParty"), "counterParty");
			taxlotDto.venue = TaxlotManager.getStringSafe(taxlot.get("venue"), "venue");
			taxlotDto.sideMultiplier = TaxlotManager.getIntSafe(taxlot.get("sideMultiplier"), "sideMultiplier");
			taxlotDto.orderType = TaxlotManager.getStringSafe(taxlot.get("orderType"), "orderType");
			taxlotDto.underlyingAsset = TaxlotManager.getStringSafe(taxlot.get("underlyingAsset"), "underlyingAsset");
			taxlotDto.limitPrice = TaxlotManager.getDoubleSafe(taxlot.get("limitPrice"), "limitPrice");
			taxlotDto.stopPrice = TaxlotManager.getDoubleSafe(taxlot.get("stopPrice"), "stopPrice");
			taxlotDto.isWhatIfTradeStreamRequired = TaxlotManager
					.getBooleanSafe(taxlot.get("isWhatIfTradeStreamRequired"), "isWhatIfTradeStreamRequired");
			taxlotDto.assetId = TaxlotManager.getIntSafe(taxlot.get("assetId"), "assetId");
			taxlotDto.asset = TaxlotManager.getStringSafe(taxlot.get("asset"), "asset");
			taxlotDto.auecId = TaxlotManager.getIntSafe(taxlot.get("auecId"), "auecId");
			taxlotDto.multiplier = TaxlotManager.getDoubleSafe(taxlot.get("multiplier"), "multiplier");
			taxlotDto.tradeAttribute1 = TaxlotManager.getStringSafe(taxlot.get("tradeAttribute1"), "tradeAttribute1");
			taxlotDto.tradeAttribute2 = TaxlotManager.getStringSafe(taxlot.get("tradeAttribute2"), "tradeAttribute2");
			taxlotDto.tradeAttribute3 = TaxlotManager.getStringSafe(taxlot.get("tradeAttribute3"), "tradeAttribute3");
			taxlotDto.tradeAttribute4 = TaxlotManager.getStringSafe(taxlot.get("tradeAttribute4"), "tradeAttribute4");
			taxlotDto.tradeAttribute5 = TaxlotManager.getStringSafe(taxlot.get("tradeAttribute5"), "tradeAttribute5");
			taxlotDto.tradeAttribute6 = TaxlotManager.getStringSafe(taxlot.get("tradeAttribute6"), "tradeAttribute6");

			// Send to CEP
			TaxlotManager.sendTaxlotToCEPEngine(taxlotDto, false);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
	
	/**
	 * Removes realTimePositions from cache if IsRealTimePositions true
	 */
	public void removeRealTimePositions() {
		try {
			synchronized (_lockerObject) {
				List<String> keyListToRemove = new ArrayList<>();
				SimpleDateFormat parserSDF = new SimpleDateFormat("MM/dd/yyyy");
				String todayDate = parserSDF.format(new Date());

				for (HashMap<String, Object> taxlot : _intermediateTaxlotCacheInput.values()) {
					Date aUECLocalDate = TaxlotManager.getDateSafe(taxlot.get("auecLocalDate"), "auecLocalDate",
							parserSDF);

					if (aUECLocalDate != null) {
						String aUECLocalDateToCheck = parserSDF.format(aUECLocalDate);
						if (todayDate.equals(aUECLocalDateToCheck)) {
							PranaLogManager
									.logOnly("Deleting taxlot from cache of AuecLocalDate : " + aUECLocalDateToCheck);

							Taxlot realTimeTaxlot = new Taxlot();
							realTimeTaxlot.basketId = TaxlotManager.getStringSafe(taxlot.get("basketId"), "basketId");
							realTimeTaxlot.taxlotId = TaxlotManager.getStringSafe(taxlot.get("taxlotId"), "taxlotId");
							realTimeTaxlot.clOrderId = TaxlotManager.getStringSafe(taxlot.get("clOrderId"),
									"clOrderId");
							realTimeTaxlot.taxlotType = TaxlotType.valueOf(TaxlotManager.getStringSafe(taxlot.get("taxlotType"),
									"taxlotType"));
							realTimeTaxlot.taxlotState = TaxlotState.Deleted;
							realTimeTaxlot.symbol = TaxlotManager.getStringSafe(taxlot.get("symbol"), "symbol");
							realTimeTaxlot.underlyingSymbol = TaxlotManager
									.getStringSafe(taxlot.get("underlyingSymbol"), "underlyingSymbol");
							realTimeTaxlot.quantity = TaxlotManager.getDoubleSafe(taxlot.get("quantity"), "quantity");
							realTimeTaxlot.avgPrice = TaxlotManager.getDoubleSafe(taxlot.get("avgPrice"), "avgPrice");
							realTimeTaxlot.orderSideTagValue = TaxlotManager
									.getStringSafe(taxlot.get("orderSideTagValue"), "orderSideTagValue");
							realTimeTaxlot.accountId = TaxlotManager.getIntSafe(taxlot.get("accountId"), "accountId");
							realTimeTaxlot.counterPartyId = TaxlotManager.getIntSafe(taxlot.get("counterPartyId"),
									"counterPartyId");
							realTimeTaxlot.venueId = TaxlotManager.getIntSafe(taxlot.get("venueId"), "venueId");
							realTimeTaxlot.auecLocalDate = aUECLocalDate;
							realTimeTaxlot.settlementDate = TaxlotManager.getDateSafe(taxlot.get("settlementDate"),
									"settlementDate", parserSDF);
							realTimeTaxlot.userId = TaxlotManager.getStringSafe(taxlot.get("userId"), "userId");
							realTimeTaxlot.strategyId = TaxlotManager.getIntSafe(taxlot.get("strategyId"),
									"strategyId");
							realTimeTaxlot.orderTypeTagValue = TaxlotManager
									.getStringSafe(taxlot.get("orderTypeTagValue"), "orderTypeTagValue");
							realTimeTaxlot.benchMarkRate = TaxlotManager.getDoubleSafe(taxlot.get("benchMarkRate"),
									"benchMarkRate");
							realTimeTaxlot.differential = TaxlotManager.getDoubleSafe(taxlot.get("differential"),
									"differential");
							realTimeTaxlot.swapNotional = TaxlotManager.getDoubleSafe(taxlot.get("swapNotional"),
									"swapNotional");
							realTimeTaxlot.dayCount = TaxlotManager.getDoubleSafe(taxlot.get("dayCount"), "dayCount");
							realTimeTaxlot.isSwapped = TaxlotManager.getBooleanSafe(taxlot.get("isSwapped"),
									"isSwapped");
							realTimeTaxlot.avgFxRateForTrade = TaxlotManager
									.getDoubleSafe(taxlot.get("avgFxRateForTrade"), "avgFxRateForTrade");
							realTimeTaxlot.conversionMethodOperator = TaxlotManager
									.getStringSafe(taxlot.get("conversionMethodOperator"), "conversionMethodOperator");
							realTimeTaxlot.tif = TaxlotManager.getStringSafe(taxlot.get("tif"), "tif");
							realTimeTaxlot.orderSide = TaxlotManager.getStringSafe(taxlot.get("orderSide"),
									"orderSide");
							realTimeTaxlot.counterParty = TaxlotManager.getStringSafe(taxlot.get("counterParty"),
									"counterParty");
							realTimeTaxlot.venue = TaxlotManager.getStringSafe(taxlot.get("venue"), "venue");
							realTimeTaxlot.sideMultiplier = TaxlotManager.getIntSafe(taxlot.get("sideMultiplier"),
									"sideMultiplier");
							realTimeTaxlot.orderType = TaxlotManager.getStringSafe(taxlot.get("orderType"),
									"orderType");
							realTimeTaxlot.underlyingAsset = TaxlotManager.getStringSafe(taxlot.get("underlyingAsset"),
									"underlyingAsset");
							realTimeTaxlot.limitPrice = TaxlotManager.getDoubleSafe(taxlot.get("limitPrice"),
									"limitPrice");
							realTimeTaxlot.stopPrice = TaxlotManager.getDoubleSafe(taxlot.get("stopPrice"),
									"stopPrice");
							realTimeTaxlot.isWhatIfTradeStreamRequired = TaxlotManager.getBooleanSafe(
									taxlot.get("isWhatIfTradeStreamRequired"), "isWhatIfTradeStreamRequired");
							realTimeTaxlot.assetId = TaxlotManager.getIntSafe(taxlot.get("assetId"), "assetId");
							realTimeTaxlot.asset = TaxlotManager.getStringSafe(taxlot.get("asset"), "asset");
							realTimeTaxlot.auecId = TaxlotManager.getIntSafe(taxlot.get("auecId"), "auecId");
							realTimeTaxlot.multiplier = TaxlotManager.getDoubleSafe(taxlot.get("multiplier"),
									"multiplier");
							realTimeTaxlot.tradeAttribute1 = TaxlotManager.getStringSafe(taxlot.get("tradeAttribute1"),
									"tradeAttribute1");
							realTimeTaxlot.tradeAttribute2 = TaxlotManager.getStringSafe(taxlot.get("tradeAttribute2"),
									"tradeAttribute2");
							realTimeTaxlot.tradeAttribute3 = TaxlotManager.getStringSafe(taxlot.get("tradeAttribute3"),
									"tradeAttribute3");
							realTimeTaxlot.tradeAttribute4 = TaxlotManager.getStringSafe(taxlot.get("tradeAttribute4"),
									"tradeAttribute4");
							realTimeTaxlot.tradeAttribute5 = TaxlotManager.getStringSafe(taxlot.get("tradeAttribute5"),
									"tradeAttribute5");
							realTimeTaxlot.tradeAttribute6 = TaxlotManager.getStringSafe(taxlot.get("tradeAttribute6"),
									"tradeAttribute6");

							// Send to CEP
							TaxlotManager.sendTaxlotToCEPEngine(realTimeTaxlot, false);

							String key = realTimeTaxlot.taxlotId + realTimeTaxlot.taxlotType;
							keyListToRemove.add(key);
						}
					}
				}

				for (String key : keyListToRemove) {
					_intermediateTaxlotCacheInput.remove(key);
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * RemoveTaxlot From Cache
	 * 
	 * @param key
	 */
	public void removeFromCache(String key) {

		try {
			synchronized (_lockerObject) {
				if (_intermediateTaxlotCacheInput.containsKey(key))
					_intermediateTaxlotCacheInput.remove(key);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Gets Tax lots
	 * 
	 * @return Output cache
	 */
	public HashMap<String, HashMap<String, Object>> getTaxlots(List<Integer> accountIdList, List<String> symbolList) {
		try {
			boolean isMasterFundCompressionEnabled = (boolean) CEPManager.getVariableValue("AnyMasterFundFlowCompressionEnabled");
			boolean isSymbolCompressionEnabled = (boolean) CEPManager.getVariableValue("AnySymbolFlowCompressionEnabled");
			/*if(CEPManager.ruletypeWithCompression!=null) {
				for (String value : CEPManager.ruletypeWithCompression.values()) {
					value = value.contains("_") ? value.replace("_", "")
							: (value.contains("-") ? value.replace("-", "") : value);
					PranaLogManager.logOnly("Value : " + value);
					if (value.equalsIgnoreCase("MasterFund") || value.equalsIgnoreCase("MasterFundSymbol")
							|| value.equalsIgnoreCase("MasterFundUnderlyingSymbol"))
						isMasterFundCompressionEnabled = true;
					else if (value.equalsIgnoreCase("Symbol") || value.equalsIgnoreCase("UnderlyingSymbol")
							|| value.equalsIgnoreCase("Sector") || value.equalsIgnoreCase("SubSector")
							|| value.equalsIgnoreCase("Asset"))
						isSymbolCompressionEnabled = true;
				}
			}*/
			PranaLogManager.logOnly(" isMasterFundCompressionEnabled = " + isMasterFundCompressionEnabled);
			PranaLogManager.logOnly(" isSymbolCompressionEnabled = " + isSymbolCompressionEnabled);
			/*
			 * Handling for MasteFund compression
			 */
			HashSet<Integer> accountIdListUpdated = new HashSet<Integer>();
			if (isMasterFundCompressionEnabled) {
				for (Integer accountId : accountIdList) {
					for (List<Integer> accountList : masterFundAccountList.values()) {
						if (accountList.contains(accountId)) {
							accountIdListUpdated.addAll(accountList);
						}
					}
				}
			} else {
				accountIdListUpdated.addAll(accountIdList);
			}
			PranaLogManager.logOnly("Account counts: " + accountIdListUpdated.size() + " and Account Id List for which Taxlots snapshot required: " + accountIdListUpdated);
			SimpleDateFormat parserSDF = new SimpleDateFormat("MM/dd/yyyy");
			String todayDate = parserSDF.format(new Date());
			// Adding Tax lots for the required accountIds
			HashMap<String, HashMap<String, Object>> listOfTaxlots = new HashMap<String, HashMap<String, Object>>();
			synchronized (_lockerObject) {
				for (HashMap<String, Object> taxlot : _intermediateTaxlotCacheInput.values()) {
					Integer accountIdTaxlot = Integer.parseInt(taxlot.get("accountId").toString());
					String symbol = taxlot.get("symbol").toString();
					String key = taxlot.get("taxlotId").toString() + taxlot.get("taxlotType").toString();
					Date aUECLocalDate = null;
					String aUECLocalDateToCheck = null;
					boolean isRealTimePref = Boolean
							.parseBoolean(CEPManager.getVariableValue("IsRealTimePositions").toString());
					if (taxlot.get("auecLocalDate") != null) {
						aUECLocalDate = parserSDF.parse(taxlot.get("auecLocalDate").toString());
						aUECLocalDateToCheck = parserSDF.format(aUECLocalDate);
					}
					if (accountIdListUpdated.contains(accountIdTaxlot)
							|| (isSymbolCompressionEnabled && symbolList.contains(symbol))) {
						if (isRealTimePref) {
							listOfTaxlots.put(key, taxlot);
						} else if (!isRealTimePref && aUECLocalDate != null
								&& (!todayDate.equals(aUECLocalDateToCheck))) {
							listOfTaxlots.put(key, taxlot);
						}
					} else if (isRealTimePref && aUECLocalDate != null && todayDate.equals(aUECLocalDateToCheck)) {
						listOfTaxlots.put(key, taxlot);
					}
				}
			}
			return listOfTaxlots;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return null;
	}
}